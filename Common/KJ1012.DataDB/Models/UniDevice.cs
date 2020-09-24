using System;

namespace KJ1012.DataDB.Models
{
    public partial class UniDevice
    {
        public Guid Id { get; set; }
        public int DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIp { get; set; }
        public int DevicePort { get; set; }
        public int AttendanceType { get; set; }
        public int DeviceType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsEnable { get; set; }
        public string Descript { get; set; }
        public string Extend1 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
