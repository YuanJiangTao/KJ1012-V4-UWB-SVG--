using System;

namespace KJ1012.DataDB.Models
{
    public partial class AdDaySummary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TerminalId { get; set; }
        public Guid Schedule { get; set; }
        public short Times { get; set; }
        public int Minutes { get; set; }
        public int? EffectiveMinutes { get; set; }
        public bool NotEnough { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime WorkDay { get; set; }
    }
}
