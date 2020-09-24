using KJ1012.Domain.Enums;

namespace KJ1012.CollectionCenter.Protocol.ProtocolModel
{
    public class ListGroupModel : BaseProtocolModel
    {
        public DeviceTypeEnum RequestDeviceType { get; set; }
        public int RequestNum { get; set; }
        public int RequestSubstation { get; set; }
    }
}
