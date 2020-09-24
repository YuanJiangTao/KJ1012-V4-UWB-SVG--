using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysDictionary
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public short Sort { get; set; }
        public bool State { get; set; }
        public string ExtendField { get; set; }
        public bool IsSystem { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
