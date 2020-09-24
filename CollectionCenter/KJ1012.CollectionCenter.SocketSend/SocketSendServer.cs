using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using KJ1012.Core.Helper;
using KJ1012.Domain;

namespace KJ1012.CollectionCenter.SocketSend
{
    public class SocketSendServer : ISocketSendServer
    {
        private readonly ISocketClientDictionary<string, UserToken> _socketClientDictionary;
        private readonly ILogger<SocketSendServer> _logger;

        public SocketSendServer(ISocketClientDictionary<string, UserToken> socketClientDictionary,
            ILogger<SocketSendServer> logger)
        {
            _socketClientDictionary = socketClientDictionary;
            _logger = logger;
        }

        public void SendMessage(int deviceType, int deviceNum, byte[] bytes, bool isAddCheck = true)
        {
            string sendKey = string.Concat(deviceType.ToString("00"), deviceNum.ToString("X2"));
            SendMessage(sendKey, bytes, isAddCheck);
        }

        public void SendMessage(string id, byte[] bytes, bool isAddCheck = true)
        {
            if (!_socketClientDictionary.ContainsKey(id)) return;
            var userToken = _socketClientDictionary[id];
            if (userToken != null)
            {
                if (userToken.Socket == null || !userToken.Socket.Connected)
                    return;
                try
                {
                    var sendBytes = bytes;
                    if (isAddCheck)
                    {
                        IList<byte> byteList = new List<byte>(bytes)
                        {
                            ConstDefine.CheckUserId
                        };
                        sendBytes = byteList.ToArray();
                    }
                    _logger.LogInformation($"s [{userToken.Remote}]：{CommonHelper.BytesToHexStr(sendBytes)}");
                    //新建异步发送对象, 发送消息
                    SocketAsyncEventArgs sendArg = new SocketAsyncEventArgs { UserToken = userToken };
                    sendArg.SetBuffer(sendBytes, 0, sendBytes.Length);
                    userToken.Socket.SendAsync(sendArg);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"s [{userToken.Remote}] fail：{ex.Message}");
                }
            }
        }

        public void SendMessage(string id, string message, bool isAddCheck = true)
        {
            byte[] bytes = CommonHelper.HexStringToBytes(message);
            SendMessage(id, bytes, isAddCheck);
        }
    }
}