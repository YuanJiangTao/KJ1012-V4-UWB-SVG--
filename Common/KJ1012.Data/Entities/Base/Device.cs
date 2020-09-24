using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using KJ1012.Data.Entities.Warn;
using KJ1012.Domain.Enums;

namespace KJ1012.Data.Entities.Base
{
    public partial class Device:BaseEntity
    {
        public Device()
        {
            Substations = new HashSet<Device>();
            DeviceWarn = new HashSet<DeviceWarn>();
        }
        public DeviceTypeEnum DeviceType { get; set; }
        public int DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public int? SerialNum { get; set; }
        public Guid? SubstationId { get; set; }
        public double? AddressX { get; set; }
        public double? AddressY { get; set; }
        public string AddressDescribe { get; set; }
        public DateTime? LastCheckTime { get; set; }
        public int DeviceState { get; set; }
        public string Describe { get; set; }
        public string Extend1 { get; set; }
        public string Extend2 { get; set; }
        public string Extend3 { get; set; }
        public string Extend4 { get; set; }
        public string Extend5 { get; set; }
        public DateTime SysStartTime { get; set; }
        public DateTime SysEndTime { get; set; }
        //[JsonIgnore]
        public Device Substation { get; set; }
        [JsonIgnore]
        public ICollection<Device> Substations { get; set; }
        [JsonIgnore]
        public ICollection<DeviceWarn> DeviceWarn { get; set; }

        [NotMapped]
        public static string TableName => "Base_Device";
    }
}
