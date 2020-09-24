using System;

namespace KJ1012.DataDB.Models
{
    public partial class DevManage
    {
        public Guid Id { get; set; }
        public Guid DeviceType { get; set; }
        public Guid DeviceOwner { get; set; }
        public string DeviceName { get; set; }
        public int TerminalId { get; set; }
        public double? AddressX { get; set; }
        public double? AddressY { get; set; }
        public string AddressDescribe { get; set; }
        public short? DeviceState { get; set; }
        public DateTime? PositionTime { get; set; }
        public bool IsMoveWarn { get; set; }
        public bool IsLossWarn { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
