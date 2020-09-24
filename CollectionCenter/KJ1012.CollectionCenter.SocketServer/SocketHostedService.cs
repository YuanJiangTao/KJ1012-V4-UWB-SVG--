using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KJ1012.CollectionCenter.Protocol.Protocol;
using KJ1012.CollectionCenter.SocketSend;
using KJ1012.Core.Helper;
using KJ1012.Domain;
using KJ1012.Domain.Enums;
using KJ1012.Domain.Setting;

namespace KJ1012.CollectionCenter.SocketService
{
    public class SocketHostedService : IHostedService, ISocketHostedService
    {
        private readonly ILogger<SocketHostedService> _logger;
        private readonly ISocketServer _socketTcpServer;
        private readonly ISocketClientDictionary<string, UserToken> _socketClientDictionary;
        private readonly IProtocolFactory _protocolFactory;
        private readonly ISocketSendServer _socketSendServer;
        private readonly CollectionSetting _kj1012CollectionSetting;
        private readonly byte[] _bitErrorTest = new byte[2];
        private bool _isReceive = true;

        public SocketHostedService(ISocketServer socketTcpServer,
            ISocketClientDictionary<string, UserToken> socketClientDictionary,
            IProtocolFactory protocolFactory,
            ISocketSendServer socketSendServer,
            IOptionsMonitor<CollectionSetting> kj1012CollectionSetting,
            ILogger<SocketHostedService> logger)
        {
            _socketTcpServer = socketTcpServer;
            _socketClientDictionary = socketClientDictionary;
            _protocolFactory = protocolFactory;
            _socketSendServer = socketSendServer;
            _kj1012CollectionSetting = kj1012CollectionSetting.CurrentValue;
            _socketTcpServer.Listening += SocketTcpServer_Listening;
            _socketTcpServer.ClientConnected += SocketTcpServer_ClientConnected;
            _socketTcpServer.ReceiveData += SocketTcpServer_ReceiveData;
            _socketTcpServer.ClientClose += SocketTcpServer_ClientClose;
            _socketTcpServer.Error += SocketTcpServer_Error;
            _socketTcpServer.SetKeepAlive(true, 1000, 500);
            _logger = logger;
        }
        //是否接受数据切换功能
        public void Switch(bool receive)
        {
            _isReceive = receive;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                _socketTcpServer.Init();
                //等待应用程序启动完成后在开启Tcp服务
                await Task.Delay(3000, cancellationToken);
                ConstDefine.CheckUserId = (byte)CommonHelper.MakeCheckUserId(_kj1012CollectionSetting.UserId);
                var result = _socketTcpServer.Start(new IPEndPoint(IPAddress.Any, _kj1012CollectionSetting.SocketServerPort));
                if (!result)
                {
                    _logger.LogError("socket server start fail");
                }
            }, cancellationToken);
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _socketTcpServer.Stop(), cancellationToken);
            _socketClientDictionary.Clear();
        }

        private void SocketTcpServer_ReceiveData(UserToken userToken, byte[] bytes)
        {
            if (!_isReceive) return;
            try
            {
                //包含误码测试解析
#if TEST
                if (userToken.WorkingMode != 0)
                {
                    ErrorTestProtocol(bytes);
                    return;
                }
#endif
                string message = CommonHelper.BytesToHexStr(bytes);
                //收到QUIT的十六进制消息51495554消息则服务器主动关闭客户端连接
                if (message == "51495554")
                {
                    userToken.Socket.Shutdown(SocketShutdown.Send);
                    return;
                }

                var deviceType = userToken.DeviceType;
                switch (deviceType)
                {
                    case 04:
                    case 06:
                    case 07:
                        {
#if TEST
                            var result = DeviceModeChange(bytes);
                            if (result) return;
#endif
                            _logger.LogInformation($"r [{userToken.Remote}]：{message}");
                            ProtocolMessage(bytes); break;
                        }
                    //包含误码测试解析
#if TEST
                    case 98:
                        {
                            if (_bitErrorTest[0] != 0)
                            {
                                _socketSendServer.SendMessage(_bitErrorTest[0], _bitErrorTest[1], bytes, false);
                            }
                            else
                            {
                                _socketSendServer.SendMessage(bytes[1], bytes[2], bytes, false);
                            }
                        }
                        break;
#endif
                    case 99: { _socketSendServer.SendMessage(bytes[1], bytes[2], bytes, false); } break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }
        }
#if TEST
        private void ErrorTestProtocol(byte[] bytes)
        {
            _socketSendServer.SendMessage(98, 0, bytes, false);
            //判断指定设备工作模式
            if (bytes[0] != 254 || bytes.Length != 6 || bytes[5] != ConstDefine.CheckUserId) return;
            byte model = bytes[3];
            if (model != 0)
            {
                _bitErrorTest[0] = bytes[1];
                _bitErrorTest[1] = bytes[2];
            }
            else
            {
                _bitErrorTest[0] = 0;
                _bitErrorTest[1] = 0;
            }

            var userTokenTest = _socketClientDictionary.Values.FirstOrDefault(w =>
                w.Id == bytes[2] && w.DeviceType == bytes[1]);
            if (userTokenTest != null) userTokenTest.WorkingMode = model;
        }

        private bool DeviceModeChange(byte[] bytes)
        {

            //判断指定设备工作模式
            if (bytes[0] == 254 && bytes.Length == 6
                                && bytes[5] == ConstDefine.CheckUserId)
            {
                _socketSendServer.SendMessage(98, 0, bytes, false);
                byte model = bytes[3];
                if (model != 0)
                {
                    _bitErrorTest[0] = bytes[1];
                    _bitErrorTest[1] = bytes[2];
                }
                else
                {
                    _bitErrorTest[0] = 0;
                    _bitErrorTest[1] = 0;
                }

                var userTokenTest = _socketClientDictionary.Values.FirstOrDefault(w =>
                    w.Id == bytes[2] && w.DeviceType == bytes[1]);
                if (userTokenTest != null) userTokenTest.WorkingMode = model;
                return true;
            }

            return false;
        }
#endif
        private void SocketTcpServer_ClientConnected(UserToken token)
        {
            try
            {
                _logger.LogInformation($"client connect [{token.Remote}]");
                SetSocketKey(token);
                string key = token.Key;
                if (!string.IsNullOrEmpty(key))
                {
                    //其它设备类型连接进来直接关闭
                    if (token.DeviceType == 0)
                    {
                        token.Socket.Shutdown(SocketShutdown.Send);
                        return;
                    }
                    if (_socketClientDictionary.ContainsKey(key))
                    {
                        //如果同一对象2次连接 关闭前一次连接
                        var socket = _socketClientDictionary[key].Socket;
                        if (socket.Connected)
                        {
                            socket.Shutdown(SocketShutdown.Send);
                        }
                        _socketClientDictionary[key] = token;
                    }
                    else
                    {
                        _socketClientDictionary.Add(key, token);
                    }

                    DeviceStateSend(token, 0);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }

        }

        private void SocketTcpServer_Listening(IPEndPoint ipEndPoint)
        {
            _logger.LogInformation("server listening on " + ipEndPoint.Port);
        }

        private void SocketTcpServer_ClientClose(UserToken token)
        {
            _logger.LogInformation($"client closed [{token.Remote}]");
            string key = token.Key;
            if (!string.IsNullOrEmpty(key))
            {
                if (_socketClientDictionary.ContainsKey(key))
                {
                    var socket = _socketClientDictionary[key].Socket;
                    if (!socket.Connected)
                    {
                        _socketClientDictionary.Remove(key, out token);

                        //模拟发送一个设备状态数据包
                        DeviceStateSend(token, 2);
                    }
                }
            }

        }

        private void SocketTcpServer_Error(Exception exception)
        {
            _logger.LogError(exception.InnerException?.Message ?? exception.Message);
        }

        private void DeviceStateSend(UserToken token, int state)
        {
            try
            {
                //模拟发送一个设备状态数据包
                byte[] sendBytes = { 3, (byte)token.DeviceType, 0, (byte)token.Id, (byte)state, ConstDefine.CheckUserId };
                //如果是分站状态，转发到地面信息接口和井下数据传输接口
                var deviceType = (DeviceTypeEnum)token.DeviceType;
                if (deviceType == DeviceTypeEnum.Substation)
                {
                    _socketSendServer.SendMessage(6, 0, sendBytes, false);
                    _socketSendServer.SendMessage(7, 0, sendBytes, false);
                }

                ProtocolMessage(sendBytes);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.Message ?? e.Message);
            }

        }

        private void ProtocolMessage(byte[] bytes)
        {
            try
            {
                do
                {
                    IProtocol protocolServices = _protocolFactory.Create(bytes);
                    if (protocolServices == null)
                    {
                        //没有找到对应解析数据对象时，判断数据中校验位字符串数量，大于1的情况，对一个校验位后面的数据再次循环解析
                        //如果校验位字符串为0或者1,说明数据已经不正确了，退出本次解析任务
                        if (bytes.Count(r => r == ConstDefine.CheckUserId) > 1)
                        {
                            bytes = bytes.Skip(Array.IndexOf(bytes, ConstDefine.CheckUserId) + 1).ToArray();
                        }
                        else break;
                    }
                    else
                    {
                        bytes = protocolServices.Receive(bytes);
                    }

                } while (bytes.Length > 0);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.InnerException?.Message ?? exception.Message);
            }
        }

        private void SetSocketKey(UserToken token)
        {
            IPEndPoint ipEndPoint = (IPEndPoint)token.Remote;
            int port = ipEndPoint.Port;
            int id = 0;
            int deviceType = 0;
            if (port == _kj1012CollectionSetting.SubStationPort)
            {
                var ip = token.IpAddress.ToString();
                var ids = ip.Split('.').ToArray();
                int.TryParse(ids[3], out int lastId);
                id = lastId % _kj1012CollectionSetting.MaxSubstationCount + 1;
                deviceType = 4;
            }
            //if (port >7000)
            //{
            //    var ip = token.IpAddress.ToString();
            //    var ids = ip.Split('.').ToArray();
            //    int.TryParse(ids[3], out int lastId);
            //    id = lastId % _KJ1012CollectionSetting.MaxSubstationCount + 1;
            //    deviceType = 4;
            //}
            else if (port == _kj1012CollectionSetting.UpDataInterfacePort)
            {
                deviceType = 7;
            }
            else if (port == _kj1012CollectionSetting.DownDataInterfacePort)
            {
                deviceType = 6;
            }
            else if (port == _kj1012CollectionSetting.ToolInterfacePort)
            {
                deviceType = 99;
            }
            else if (port == _kj1012CollectionSetting.ErrorRateInterfacePort)
            {
                deviceType = 98;
            }
            token.Id = id;
            token.DeviceType = deviceType;
        }
    }
}
