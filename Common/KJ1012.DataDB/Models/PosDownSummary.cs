using System;

namespace KJ1012.DataDB.Models
{
    public partial class PosDownSummary
    {
        public Guid Id { get; set; }
        public int MemberCount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
