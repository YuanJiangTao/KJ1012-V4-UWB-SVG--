using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KJ1012.Data.Entities.Warn
{
    public partial class TerminalWarn:BaseEntity
    {
        public int TerminalId { get; set; }
        public int TerminalState { get; set; }
        public int Station { get; set; }
        public int Distance { get; set; }
        public int Direction { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public short? RecoveryType { get; set; }
        public string RecoveryRemark { get; set; }
        [NotMapped]
        public static string TableName => "Warn_Terminal";
    }
}
