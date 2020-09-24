using System;

namespace KJ1012.DataDB.Models
{
    public partial class AdScreen
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public short Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
