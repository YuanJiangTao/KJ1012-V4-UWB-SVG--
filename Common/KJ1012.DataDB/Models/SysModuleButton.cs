using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysModuleButton
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ButtonType { get; set; }
        public string Url { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual SysModule Module { get; set; }
    }
}
