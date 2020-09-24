using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysAuthorize
    {
        public Guid Id { get; set; }
        public int? Category { get; set; }
        public Guid? ObjectId { get; set; }
        public int? ItemType { get; set; }
        public Guid? ItemId { get; set; }
        public int? SortCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
