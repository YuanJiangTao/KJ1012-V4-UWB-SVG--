using System;
using System.Collections.Generic;

namespace KJ1012.DataDB.Models
{
    public partial class BaseDevice
    {
        public BaseDevice()
        {
            InverseSubstation = new HashSet<BaseDevice>();
            WarnDevice = new HashSet<WarnDevice>();
        }

        public Guid Id { get; set; }
        public int DeviceType { get; set; }
        public int DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public int? SerialNum { get; set; }
        public Guid? SubstationId { get; set; }
        public double? AddressX { get; set; }
        public double? AddressY { get; set; }
        public string AddressDescribe { get; set; }
        public DateTime? LastCheckTime { get; set; }
        public int DeviceState { get; set; }
        public string Describe { get; set; }
        public string Extend1 { get; set; }
        public string Extend2 { get; set; }
        public string Extend3 { get; set; }
        public string Extend4 { get; set; }
        public string Extend5 { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual BaseDevice Substation { get; set; }
        public virtual BaseAreaDevice BaseAreaDevice { get; set; }
        public virtual ICollection<BaseDevice> InverseSubstation { get; set; }
        public virtual ICollection<WarnDevice> WarnDevice { get; set; }
    }
}
