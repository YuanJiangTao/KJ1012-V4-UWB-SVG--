using System;
using System.Collections.Generic;

namespace KJ1012.Core.Data
{
    public class TreeModel
    {
        public TreeModel()
        {
            Children = new List<TreeModel>();
        }

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public object Extend { get; set; }
        public bool Expand { get; set; } = true;
        public List<TreeModel> Children { get; set; }
    }
}
