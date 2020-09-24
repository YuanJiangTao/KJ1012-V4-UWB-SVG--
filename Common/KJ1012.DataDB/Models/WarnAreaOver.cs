using System;

namespace KJ1012.DataDB.Models
{
    public partial class WarnAreaOver
    {
        public Guid Id { get; set; }
        public short? AreaOverType { get; set; }
        public Guid? TypeId { get; set; }
        public int MaxNum { get; set; }
        public int? Max { get; set; }
        public DateTime? RecoveryTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
