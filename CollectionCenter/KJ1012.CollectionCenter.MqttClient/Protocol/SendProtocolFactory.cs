using System.Collections.Generic;
using System.Linq;

namespace KJ1012.CollectionCenter.MqttClient.Protocol
{
    public class SendProtocolFactory : ISendProtocolFactory
    {
        private readonly IEnumerable<ISendProtocol> _sendProtocols;

        public SendProtocolFactory(IEnumerable<ISendProtocol> sendProtocols)
        {
            _sendProtocols = sendProtocols;
        }


        public ISendProtocol Create(int protocolId)
        {
            return _sendProtocols.FirstOrDefault(r => r.IsMatch(protocolId));
        }
    }
}
