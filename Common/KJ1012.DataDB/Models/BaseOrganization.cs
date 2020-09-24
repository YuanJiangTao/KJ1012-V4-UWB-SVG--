using System;
using System.Collections.Generic;

namespace KJ1012.DataDB.Models
{
    public partial class BaseOrganization
    {
        public BaseOrganization()
        {
            BaseMember = new HashSet<BaseMember>();
        }

        public Guid Id { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string PhoneNum { get; set; }
        public string Manager { get; set; }
        public Guid? ParentId { get; set; }
        public string Describtion { get; set; }
        public bool DeleteMark { get; set; }
        public int MinMinutes { get; set; }
        public int MaxMinutes { get; set; }
        public bool IsHide { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<BaseMember> BaseMember { get; set; }
    }
}
