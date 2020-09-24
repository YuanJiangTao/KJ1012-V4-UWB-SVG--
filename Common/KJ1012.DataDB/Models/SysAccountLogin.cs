using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysAccountLogin
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string LoginIp { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
