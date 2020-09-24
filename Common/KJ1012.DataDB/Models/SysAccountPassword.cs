using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysAccountPassword
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string Salt { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual SysAccount Account { get; set; }
    }
}
