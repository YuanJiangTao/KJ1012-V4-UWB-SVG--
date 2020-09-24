using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.Protocol.ProtocolModel
{
    public class TimeCheckGroupModel: BaseProtocolModel
    {
        public DeviceTypeEnum RequestDeviceType { get; set; }
        public int RequestId { get; set; }
    }
}
