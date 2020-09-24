using System;
using System.Collections.Generic;
namespace KJ1012.Core.Data
{
    public class PatrolDeviceModel
    {
        public Guid PatrolId { get; set; }
        public List<int> DeviceNumList { get; set; }
    }
}
