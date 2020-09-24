namespace KJ1012.Domain.Enums
{
    public enum AttendanceTypeEnum
    {
        /// <summary>
        /// 入井考勤
        /// </summary>
        AttendanceIn = 0,
        /// <summary>
        /// 出井考勤
        /// </summary>
        AttendanceOut = 1,
        /// <summary>
        /// 识别出井
        /// </summary>
        AttendanceFlagOut = 2,
        /// <summary>
        /// 识别入井
        /// </summary>
        AttendanceFlagIn = 3,
        /// <summary>
        /// 识别准备出井
        /// </summary>
        AttendanceFlagRdyOut = 4,
        /// <summary>
        /// 识别准备入井
        /// </summary>
        AttendanceFlagRdyIn = 5,
        /// <summary>
        /// （人工出井）
        /// </summary>
        AttendanceTerminalOut = 6,
        /// <summary>
        /// 自动出井
        /// </summary>
        AttendanceAutoOut = 7
    }
}
