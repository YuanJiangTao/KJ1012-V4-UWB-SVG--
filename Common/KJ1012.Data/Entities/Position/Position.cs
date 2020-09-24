using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KJ1012.Data.Entities.Position
{
    public partial class Position : BaseEntity
    {
        public int TerminalId { get; set; }
        public int TerminalState { get; set; }
        public short ReceiveFrom { get; set; }
        public byte PositionWay { get; set; }
        public int Direction { get; set; }
        public int Station { get; set; }
        public int Distance { get; set; }
        public int NextStation { get; set; }
        public DateTime PositionTime { get; set; }
        [NotMapped]
        public static string TableName => "Pos_Position";

    }
}
