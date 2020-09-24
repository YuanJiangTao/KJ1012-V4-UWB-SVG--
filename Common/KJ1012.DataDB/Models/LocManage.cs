using System;

namespace KJ1012.DataDB.Models
{
    public partial class LocManage
    {
        public Guid Id { get; set; }
        public Guid? TypeId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public int? TerminalId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
