using System;

namespace KJ1012.DataDB.Models
{
    public partial class LocLocomotive
    {
        public Guid Id { get; set; }
        public Guid? TypeId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string TerminalId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
