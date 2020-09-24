namespace KJ1012.Domain.Enums
{
   public enum SubstationState
    {
        /// <summary>
        /// 正常
        /// </summary>
        SubstationOk = 0,
        /// <summary>
        /// 低电量
        /// </summary>
        SubstationPowerLow = 1,
        /// <summary>
        /// 通信不正常
        /// </summary>
        SubstationOffline = 2,
        /// <summary>
        ///  损坏
        /// </summary>
        SubstationBad = 3
    }
}
