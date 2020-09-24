using System;

namespace KJ1012.DataDB.Models
{
    public partial class AdOrderClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AdvanceTime { get; set; }
        public Guid InstitutionId { get; set; }
        public int Priority { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual AdInstitution Institution { get; set; }
    }
}
