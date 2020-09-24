using System;

namespace KJ1012.DataDB.Models
{
    public partial class SysRole
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public bool IsSysRole { get; set; }
        public bool IsActive { get; set; }
        public Guid? Creator { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
