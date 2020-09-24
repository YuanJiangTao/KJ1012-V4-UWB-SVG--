using System;

namespace KJ1012.DataDB.Models
{
    public partial class UniDeviceRelation
    {
        public Guid Id { get; set; }
        public Guid FaceDeviceId { get; set; }
        public Guid? IeoId { get; set; }
        public int? ChannelId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
