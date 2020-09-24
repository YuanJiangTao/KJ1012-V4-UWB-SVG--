using System;

namespace KJ1012.DataDB.Models
{
    public partial class WarnTerminal
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public int TerminalState { get; set; }
        public int Station { get; set; }
        public int Distance { get; set; }
        public int Direction { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public short? RecoveryType { get; set; }
        public string RecoveryRemark { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
