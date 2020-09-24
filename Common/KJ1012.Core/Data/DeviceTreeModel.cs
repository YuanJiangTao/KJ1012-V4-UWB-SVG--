using System;

namespace KJ1012.Core.Data
{
    public class DeviceTreeModel
    {
        public Guid DeviceId { get; set; }
        public string AddressDescribe { get; set; }
        public string DeviceName { get; set; }
        public int DeviceNum { get; set; }
        public string AreaName { get; set; }
        public Guid? AreaId { get; set; }
        public int Count { get; set; }
    }
}
