using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysAccount
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid? RoleId { get; set; }
        public short SystemId { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Deleted { get; set; }
        public Guid? Creator { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual SysAccountPassword SysAccountPassword { get; set; }
    }
}
