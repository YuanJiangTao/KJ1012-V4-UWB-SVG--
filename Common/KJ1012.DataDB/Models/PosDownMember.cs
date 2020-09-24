using System;

namespace KJ1012.DataDB.Models
{
    public partial class PosDownMember
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public int? TerminalState { get; set; }
        public int? Station { get; set; }
        public int? Distance { get; set; }
        public int? Direction { get; set; }
        public int? NextStation { get; set; }
        public int DataFrom { get; set; }
        public DateTime PositionTime { get; set; }
        public Guid ClassId { get; set; }
        public bool IsUniqueId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
