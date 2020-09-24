namespace KJ1012.Domain.Enums
{
    public static class DomainEnumHelper
    {
        public static AttendanceStatusEnum ParseInOrOut(int attendanceType)
        {
            switch (attendanceType)
            {
                case 0:
                case 3: return AttendanceStatusEnum.In;
                case 1:
                case 2: return AttendanceStatusEnum.Out;
                case 4: return AttendanceStatusEnum.WaitOut;
                case 5: return AttendanceStatusEnum.WaitIn;
                default: return AttendanceStatusEnum.UnKnown;
            }
        }
        public static AttendanceStatusEnum ParseInOrOut(AttendanceTypeEnum attendanceType)
        {
            switch (attendanceType)
            {
                case AttendanceTypeEnum.AttendanceIn:
                case AttendanceTypeEnum.AttendanceFlagIn: return AttendanceStatusEnum.In;
                case AttendanceTypeEnum.AttendanceOut:
                case AttendanceTypeEnum.AttendanceFlagOut: return AttendanceStatusEnum.Out;
                case AttendanceTypeEnum.AttendanceFlagRdyOut: return AttendanceStatusEnum.WaitOut;
                case AttendanceTypeEnum.AttendanceFlagRdyIn: return AttendanceStatusEnum.WaitIn;
                default: return AttendanceStatusEnum.UnKnown;
            }
        }
    }
}
