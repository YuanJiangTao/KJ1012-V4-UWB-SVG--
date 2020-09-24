using System;

namespace KJ1012.Core.Data
{
    public class OrganizationTree
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public int Serial { get; set; }
        public string Title { get; set; }
    }
}
