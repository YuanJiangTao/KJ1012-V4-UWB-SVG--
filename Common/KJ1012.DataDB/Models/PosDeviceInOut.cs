using System;

namespace KJ1012.DataDB.Models
{
    public partial class PosDeviceInOut
    {
        public Guid Id { get; set; }
        public int TernimalId { get; set; }
        public int DeviceNum { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
