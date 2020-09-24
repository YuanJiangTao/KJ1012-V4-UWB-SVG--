using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysPeriodDate
    {
        public Guid Id { get; set; }
        public DateTime PeriodDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
