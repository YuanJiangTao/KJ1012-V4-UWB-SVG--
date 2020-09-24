using KJ1012.Domain.Enums;
using System;

namespace KJ1012.Core.Data
{
    public class DeviceModel
    {
        public Guid Id { get; set; }
        public DeviceTypeEnum DeviceType { get; set; }
        public string DeviceTypeName { get; set; }
        public int? SerialNum { get; set; }
        public int DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public int DeviceState { get; set; }
        public string AddressDescribe { get; set; }
        public string Extend1 { get; set; }
        public double? AddressX { get; set; }
        public double? AddressY { get; set; }
        public Guid? SubstationId { get; set; }
        public DateTime? LastCheckTime { get; set; }
        public string Describe { get; set; }
        public string SubName { get; set; }
    }
}
