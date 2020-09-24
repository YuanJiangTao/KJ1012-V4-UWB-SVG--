using System;

namespace KJ1012.DataDB.Models
{
    public partial class AdClassInstitution
    {
        public Guid Id { get; set; }
        public Guid InstitutionId { get; set; }
        public int ItemType { get; set; }
        public Guid ItemId { get; set; }
        public int Sort { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual AdInstitution Institution { get; set; }
    }
}
