using System;

namespace KJ1012.DataDB.Models
{
    public partial class PosPosition
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public short TerminalState { get; set; }
        public short ReceiveFrom { get; set; }
        public short Direction { get; set; }
        public int Station { get; set; }
        public int Distance { get; set; }
        public int? NextStation { get; set; }
        public DateTime PositionTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
