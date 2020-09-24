namespace KJ1012.Domain.Enums
{
   public enum TerminalStateEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        TerminalStateOk = 0,
        /// <summary>
        /// 低电量
        /// </summary>
        TerminalStatePowerLow = 1,
        /// <summary>
        /// 设备故障
        /// </summary>
        TerminalStateLock = 2,
        /// <summary>
        /// 主动求救
        /// </summary>
        TerminalStateHelp = 4,
        /// <summary>
        /// 管道异常
        /// </summary>
        TerminalStateException = 8,
    }
}
