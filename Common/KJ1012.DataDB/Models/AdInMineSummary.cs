using System;

namespace KJ1012.DataDB.Models
{
    public partial class AdInMineSummary
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TerminalId { get; set; }
        public Guid? MemberId { get; set; }
        public int Times { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
