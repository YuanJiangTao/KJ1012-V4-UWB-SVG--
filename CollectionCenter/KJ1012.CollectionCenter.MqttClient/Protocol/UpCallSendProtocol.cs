using Microsoft.Extensions.Options;
using KJ1012.CollectionCenter.SocketSend;
using KJ1012.Domain.Setting;

namespace KJ1012.CollectionCenter.MqttClient.Protocol
{
    public class UpCallSendProtocol:ISendProtocol
    {
        private readonly ISocketSendServer _socketSendServer;
        private readonly IOptionsMonitor<CollectionSetting> _kj1012CollectionSetting;

        public UpCallSendProtocol(ISocketSendServer socketSendServer,
            IOptionsMonitor<CollectionSetting> kj1012CollectionSetting)
        {
            _socketSendServer = socketSendServer;
            _kj1012CollectionSetting = kj1012CollectionSetting;
        }

        public bool IsMatch(int protocolId)
        {
            return protocolId == ProtocolId;
        }

        private int ProtocolId => 4;
        public void Receive(byte[] bytes)
        {
            var substationId = (int)bytes[1];
            if (substationId == 0)
            {
                var setting = _kj1012CollectionSetting.CurrentValue;
                for (int i = 1; i <= setting.MaxSubstationCount; i++)
                {
                    _socketSendServer.SendMessage(4,i,bytes);
                }
            }
            else
            {
                _socketSendServer.SendMessage(4, substationId, bytes);
            }
        }
    }
}
