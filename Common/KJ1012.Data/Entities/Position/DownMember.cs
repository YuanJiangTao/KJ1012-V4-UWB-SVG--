using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KJ1012.Data.Entities.Position
{
    public partial class DownMember : BaseEntity
    {
        public int TerminalId { get; set; }
        public int? TerminalState { get; set; }
        public int SinsState { get; set; }
        public int? Station { get; set; }
        public int? Distance { get; set; }
        public int? Direction { get; set; }
        public int? NextStation { get; set; }
        public int DataFrom { get; set; }
        public DateTime PositionTime { get; set; }
        public Guid ClassId { get; set; }
        public bool IsUniqueId { get; set; }

        [NotMapped]
        public static string TableName => "Pos_DownMember";
    }
}
