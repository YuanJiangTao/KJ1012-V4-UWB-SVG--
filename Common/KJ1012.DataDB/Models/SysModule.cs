using System;
using System.Collections.Generic;

namespace KJ1012.DataDB.Models
{
    public partial class SysModule
    {
        public SysModule()
        {
            InverseParent = new HashSet<SysModule>();
            SysModuleButton = new HashSet<SysModuleButton>();
        }

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Component { get; set; }
        public int ModuleType { get; set; }
        public int SerialNum { get; set; }
        public string Url { get; set; }
        public short State { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual SysModule Parent { get; set; }
        public virtual ICollection<SysModule> InverseParent { get; set; }
        public virtual ICollection<SysModuleButton> SysModuleButton { get; set; }
    }
}
