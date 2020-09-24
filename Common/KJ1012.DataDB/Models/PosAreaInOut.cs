using System;

namespace KJ1012.DataDB.Models
{
    public partial class PosAreaInOut
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public Guid AreaId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
