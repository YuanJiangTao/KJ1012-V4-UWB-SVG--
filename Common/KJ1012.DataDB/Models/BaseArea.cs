using System;
using System.Collections.Generic;

namespace KJ1012.DataDB.Models
{
    public partial class BaseArea
    {
        public BaseArea()
        {
            BaseAreaDevice = new HashSet<BaseAreaDevice>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxNum { get; set; }
        public int AreaType { get; set; }
        public bool IsGround { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<BaseAreaDevice> BaseAreaDevice { get; set; }
    }
}
