using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysConfig
    {
        public Guid Id { get; set; }
        public int TypeId { get; set; }
        public string ValueKey { get; set; }
        public string Value { get; set; }
        public string Extend1 { get; set; }
        public string Extend2 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
