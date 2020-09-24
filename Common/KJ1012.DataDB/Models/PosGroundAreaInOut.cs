using System;

namespace KJ1012.DataDB.Models
{
    public partial class PosGroundAreaInOut
    {
        public Guid Id { get; set; }
        public int Machine { get; set; }
        public int TerminalId { get; set; }
        public DateTime InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
