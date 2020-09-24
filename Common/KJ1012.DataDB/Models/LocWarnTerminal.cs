using System;

namespace KJ1012.DataDB.Models
{
    public partial class LocWarnTerminal
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public int TerminalState { get; set; }
        public int Station1 { get; set; }
        public int Station2 { get; set; }
        public int Distance1 { get; set; }
        public int Distance2 { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public short? RecoveryType { get; set; }
        public string RecoveryRemark { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
