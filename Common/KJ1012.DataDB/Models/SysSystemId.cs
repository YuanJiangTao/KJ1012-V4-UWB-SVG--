using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysSystemId
    {
        public Guid Id { get; set; }
        public string SystemUserId { get; set; }
        public string SystemName { get; set; }
        public bool AllowSendMessage { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
