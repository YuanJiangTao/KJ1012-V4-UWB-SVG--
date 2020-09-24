using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KJ1012.Domain;

namespace KJ1012.CollectionCenter.SocketService
{
    public class SocketServer : ISocketServer
    {
        private readonly int _maxConnectNum;    //最大连接数
        private readonly IBufferManager _bufferManager;
        const int OpsToAlloc = 2;
        private Socket _listenSocket;            //监听Socket
        private readonly ISocketEventPool _pool;
        readonly Semaphore _maxNumberAcceptedClients;
        private readonly byte[] _inOptionValues = new byte[12];


        #region 定义委托

        /// <summary>
        /// 接收到客户端的数据
        /// </summary>
        /// <param name="token">客户端</param>
        /// <param name="buff">客户端数据</param>
        public delegate void OnReceiveData(UserToken token, byte[] buff);
        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="token">客户端</param>
        public delegate void OnClientConnected(UserToken token);
        /// <summary>
        /// 服务开始监听
        /// </summary>
        /// <param name="ipEndPoint">服务地址</param>
        public delegate void OnListening(IPEndPoint ipEndPoint);
        /// <summary>
        /// 客户端关闭
        /// </summary>
        /// <param name="token">客户端</param>
        public delegate void OnClientClose(UserToken token);
        /// <summary>
        /// 错误事件
        /// </summary>
        /// <param name="exception">错误对象</param>
        public delegate void OnError(Exception exception);

        #endregion

        #region 定义事件
        /// <summary>
        /// 接收到客户端的数据事件
        /// </summary>
        public event OnReceiveData ReceiveData;
        public event OnClientConnected ClientConnected;
        public event OnListening Listening;
        public event OnClientClose ClientClose;
        public event OnError Error;

        #endregion

        #region 定义属性

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        private List<UserToken> ClientList { get; set; }

        #endregion

        public SocketServer()
        {
            int numConnections = 200;
            int receiveBufferSize = 1024;
            _maxConnectNum = numConnections;
            // allocate buffers such that the maximum number of sockets can have one outstanding read and 
            //write posted to the socket simultaneously  
            _bufferManager = new BufferManager(receiveBufferSize * numConnections * OpsToAlloc, receiveBufferSize);

            _pool = new SocketEventPool(numConnections);
            _maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numConnections">最大连接数</param>
        /// <param name="receiveBufferSize">缓存区大小</param>
        public SocketServer(int numConnections, int receiveBufferSize)
        {
            _maxConnectNum = numConnections;
            // allocate buffers such that the maximum number of sockets can have one outstanding read and 
            //write posted to the socket simultaneously  
            _bufferManager = new BufferManager(receiveBufferSize * numConnections * OpsToAlloc, receiveBufferSize);

            _pool = new SocketEventPool(numConnections);
            _maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds 
            // against memory fragmentation
            _bufferManager.InitBuffer();
            ClientList = new List<UserToken>();
            // preallocate pool of SocketAsyncEventArgs objects

            for (int i = 0; i < _maxConnectNum; i++)
            {
                var readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += IO_Completed;
                readWriteEventArg.UserToken = new UserToken();

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                _bufferManager.SetBuffer(readWriteEventArg);
                // add SocketAsyncEventArg to the pool
                _pool.Push(readWriteEventArg);
            }
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="localEndPoint"></param>
        public bool Start(IPEndPoint localEndPoint)
        {
            try
            {
                ClientList.Clear();
                _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _listenSocket.Bind(localEndPoint);
                // start the server with a listen backlog of 100 connections
                _listenSocket.Listen(_maxConnectNum);
                // post accepts on the listening socket
                Listening?.Invoke(localEndPoint);
                StartAccept(null);
                return true;
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            foreach (UserToken token in ClientList)
            {
                var socketAsyncEventArgs = new SocketAsyncEventArgs
                {
                    UserToken = token
                };

                CloseClientSocket(socketAsyncEventArgs);
            }
            try
            {
                _listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                // ignored
            }

            _listenSocket.Close();
            lock (ClientList) { ClientList.Clear(); }
        }

        // Begins an operation to accept a connection request from the client 
        //
        // <param name="acceptEventArg">The context object to use when issuing 
        // the accept operation on the server's listening socket</param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += AcceptEventArg_Completed;
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
            }

            _maxNumberAcceptedClients.WaitOne();
            if (!_listenSocket.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync 
        // operations and is invoked when an accept operation is complete
        //
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                // Get the socket for the accepted client connection and put it into the 
                //ReadEventArg object user token
                SocketAsyncEventArgs readEventArgs = _pool.Pop();
                UserToken userToken = (UserToken)readEventArgs.UserToken;
                userToken.Socket = e.AcceptSocket;
                userToken.ConnectTime = DateTime.Now;
                userToken.Remote = e.AcceptSocket.RemoteEndPoint;
                userToken.IpAddress = ((IPEndPoint)e.AcceptSocket.RemoteEndPoint).Address;

                lock (ClientList) { ClientList.Add(userToken); }

                if (!e.AcceptSocket.ReceiveAsync(readEventArgs))
                {
                    ProcessReceive(readEventArgs);
                }
                //设置keep-alive

                e.AcceptSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                e.AcceptSocket.IOControl(IOControlCode.KeepAliveValues, _inOptionValues, null);

                ClientConnected?.Invoke(userToken);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }

            // Accept the next connection request
            if (e.SocketError == SocketError.OperationAborted) return;
            StartAccept(e);
        }


        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }


        // This method is invoked when an asynchronous receive operation completes. 
        // If the remote host closed the connection, then the socket is closed.  
        // If data was received then the data is echoed back to the client.
        //
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                // check if the remote host closed the connection
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    UserToken token = (UserToken)e.UserToken;
                    //将数据包交给后台处理,这里你也可以新开个线程来处理.加快速度.
                    ReceiveData?.Invoke(token, e.Buffer.Skip(e.Offset).Take(e.BytesTransferred).ToArray());
                    //继续接收. 为什么要这么写,请看Socket.ReceiveAsync方法的说明
                    if (!token.Socket.ReceiveAsync(e))
                        ProcessReceive(e);
                }
                else
                {
                    CloseClientSocket(e);
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
            }
        }

        // This method is invoked when an asynchronous send operation completes.  
        // The method issues another receive on the socket to read any additional 
        // data sent from the client
        //
        // <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // done echoing data back to the client
                UserToken token = (UserToken)e.UserToken;
                // read the next block of data send from the client
                bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        //关闭客户端
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            if (e.UserToken is UserToken token)
            {
                lock (ClientList)
                {
                    ClientList.Remove(token);
                }

                // close the socket associated with the client
                try
                {
                    token.Socket.Shutdown(SocketShutdown.Send);
                    ClientClose?.Invoke(token);
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex);
                }

                token.Socket.Close();
                _maxNumberAcceptedClients.Release();
                // Free the SocketAsyncEventArg so they can be reused by another client
                e.UserToken = new UserToken();
                _pool.Push(e);
            }
        }

        //关闭客户端
        public void CloseClient(UserToken token)
        {
            var socketAsyncEventArgs = new SocketAsyncEventArgs
            {
                UserToken = token
            };

            CloseClientSocket(socketAsyncEventArgs);
        }

        public void SetKeepAlive(bool isUseKeepAlive,int keepalivetime,int keepaliveinterval)
        {
            //是否启用Keep-Alive
            BitConverter.GetBytes(isUseKeepAlive).CopyTo(_inOptionValues, 0);
            //多长时间开始第一次探测
            BitConverter.GetBytes(keepalivetime).CopyTo(_inOptionValues,4);
            //探测时间间隔
            BitConverter.GetBytes(keepaliveinterval).CopyTo(_inOptionValues,8);
        }
    }
}
