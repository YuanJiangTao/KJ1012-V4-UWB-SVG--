using System;

namespace KJ1012.DataDB.Models
{
    public partial class UniAttendance
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public Guid UserId { get; set; }
        public DateTime UniqueTime { get; set; }
        public short UniqueType { get; set; }
        public bool IsUnique { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
