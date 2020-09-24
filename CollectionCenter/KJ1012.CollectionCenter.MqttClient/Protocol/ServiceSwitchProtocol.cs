using System;
using KJ1012.CollectionCenter.SocketService;

namespace KJ1012.CollectionCenter.MqttClient.Protocol
{
    public class ServiceSwitchProtocol : ISendProtocol
    {
        private readonly ISocketHostedService _socketSendServer;

        public ServiceSwitchProtocol(ISocketHostedService socketSendServer)
        {
            _socketSendServer = socketSendServer;
        }

        public bool IsMatch(int protocolId)
        {
            return protocolId == ProtocolId;
        }

        private int ProtocolId => 1;
        public void Receive(byte[] bytes)
        {
            bool isReceive =Convert.ToBoolean(bytes[1]);
            _socketSendServer.Switch(isReceive);
        }
    }
}
