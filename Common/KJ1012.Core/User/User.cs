using System;

namespace KJ1012.Core.User
{
   public class User
    {
        public Guid AccountId { get; set; }
        public string UserName { get; set; }
        public Guid RoleId { get; set; }
        public Guid SvrId { get; set; }
        public short SystemId { get; set; } = 1000;
    }
}