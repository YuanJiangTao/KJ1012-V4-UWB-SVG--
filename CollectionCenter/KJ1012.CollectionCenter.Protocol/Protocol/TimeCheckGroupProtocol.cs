using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public class TimeCheckGroupProtocol : BaseProtocol<TimeCheckGroupModel>
    {
        public override int ProtocolLength => 4;

        public override int ProtocolId => 10;
        public TimeCheckGroupProtocol(IEngine engine, ITypeFinder typeFinder) : base(engine, typeFinder)
        {
        }

        protected override TimeCheckGroupModel ExecProtocolParsing(byte[] receiveBytes)
        {
            return new TimeCheckGroupModel
            {
                ProtocolId = ProtocolId,
                RequestDeviceType = (DeviceTypeEnum)receiveBytes[1],
                RequestId = receiveBytes[2]
            };
        }

    }
}

