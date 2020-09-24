using System;
using System.ComponentModel.DataAnnotations.Schema;
using KJ1012.Data.Entities.Base;

namespace KJ1012.Data.Entities.Warn
{
    public partial class DeviceWarn : BaseEntity
    {
        public Guid? DeviceId { get; set; }
        public int DeviceState { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public short? RecoveryType { get; set; }
        public string RecoveryRemark { get; set; }
        public int DelayTime { get; set; }
        public virtual Device Device { get; set; }

        [NotMapped]
        public static string TableName => "Warn_Device";
    }
}
