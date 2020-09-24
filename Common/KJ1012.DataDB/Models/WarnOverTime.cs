using System;

namespace KJ1012.DataDB.Models
{
    public partial class WarnOverTime
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public short? OverTimeType { get; set; }
        public Guid? TypeId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
