using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.Protocol.ProtocolModel
{
    public class DeviceStateGroupModel: BaseProtocolModel
    {
        public DeviceTypeEnum DeviceType { get; set; }
        public int DeviceNum { get; set; }
        public int DeviceState { get; set; }
    }
}
