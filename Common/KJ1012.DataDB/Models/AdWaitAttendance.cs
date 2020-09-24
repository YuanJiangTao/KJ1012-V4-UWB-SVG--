using System;

namespace KJ1012.DataDB.Models
{
    public partial class AdWaitAttendance
    {
        public Guid Id { get; set; }
        public int TerminalId { get; set; }
        public int? Machine { get; set; }
        public int Type { get; set; }
        public DateTime AttendanceTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
