using System;

namespace KJ1012.DataDB.Models
{
    public partial class WarnDevice
    {
        public Guid Id { get; set; }
        public Guid? DeviceId { get; set; }
        public int DeviceState { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public short? RecoveryType { get; set; }
        public string RecoveryRemark { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual BaseDevice Device { get; set; }
    }
}
