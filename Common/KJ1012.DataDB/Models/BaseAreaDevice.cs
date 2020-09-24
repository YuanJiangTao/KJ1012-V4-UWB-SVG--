using System;

namespace KJ1012.DataDB.Models
{
    public partial class BaseAreaDevice
    {
        public Guid Id { get; set; }
        public Guid AreaId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual BaseArea Area { get; set; }
        public virtual BaseDevice Device { get; set; }
    }
}
