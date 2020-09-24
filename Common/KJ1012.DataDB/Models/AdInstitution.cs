using System;
using System.Collections.Generic;

namespace KJ1012.DataDB.Models
{
    public partial class AdInstitution
    {
        public AdInstitution()
        {
            AdClassInstitution = new HashSet<AdClassInstitution>();
            AdOrderClass = new HashSet<AdOrderClass>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<AdClassInstitution> AdClassInstitution { get; set; }
        public virtual ICollection<AdOrderClass> AdOrderClass { get; set; }
    }
}
