namespace KJ1012.Domain.Setting
{
    public class UniqueSetting
    {
        /// <summary>
        /// 唯一新检测定位卡信息开关
        /// </summary>
        public bool PositionDetectionSwitch { get; set; }
        /// <summary>
        /// 井口考勤缓存时间,默认5分钟
        /// </summary>
        public int WaitAttendanceCacheTime { get; set; }
        /// <summary>
        /// 开启班次最大人数限制
        /// </summary>
        public bool OpenClassMemberOver { get; set; }
        /// <summary>
        /// 班次最大限制下井人数
        /// </summary>
        public int ClassMemberMaxNum { get; set; }
    }
}
