using System.Collections.Generic;
using System.Linq;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public class ProtocolFactory : IProtocolFactory
    {
        private readonly IEnumerable<IProtocol> _protocols;

        public ProtocolFactory(IEnumerable<IProtocol> protocols)
        {
            _protocols = protocols;
        }


        public IProtocol Create(byte[] bytes)
        {
            return _protocols.FirstOrDefault(r => r.IsMatch(bytes));
        }
    }
}
