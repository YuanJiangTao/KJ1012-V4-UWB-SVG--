using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public class BaseStationListGroupProtocol : BaseProtocol<BaseStationListGroupModel>
    {
        public override int ProtocolLength => 5;

        public override int ProtocolId => 30;
        public BaseStationListGroupProtocol(IEngine engine, ITypeFinder typeFinder) : base(engine, typeFinder)
        {
        }

        protected override BaseStationListGroupModel ExecProtocolParsing(byte[] receiveBytes)
        {
            return new BaseStationListGroupModel
            {
                ProtocolId = ProtocolId,
                RequestDeviceType = (DeviceTypeEnum)receiveBytes[1],
                RequestNum = receiveBytes[2],
                RequestSubstation = receiveBytes[3]
            };
        }
    }
}

