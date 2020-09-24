using KJ1012.CollectionCenter.Protocol.ProtocolModel;
using KJ1012.Core.Helper;
using KJ1012.Core.Infrastructure;
using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.Protocol.Protocol
{
    public class DeviceStateGroupProtocol : BaseProtocol<DeviceStateGroupModel>
    {
        public override int ProtocolLength => 6;

        public override int ProtocolId => 3;
        public DeviceStateGroupProtocol(IEngine engine, ITypeFinder typeFinder) : base(engine, typeFinder)
        {
        }
        protected override DeviceStateGroupModel ExecProtocolParsing(byte[] receiveBytes)
        {
            return new DeviceStateGroupModel
            {
                ProtocolId = ProtocolId,
                DeviceType = (DeviceTypeEnum)receiveBytes[1],
                DeviceNum = CommonHelper.BytesToInt(receiveBytes[2], receiveBytes[3]),
                DeviceState = receiveBytes[4]
            };
        }
    }
}

