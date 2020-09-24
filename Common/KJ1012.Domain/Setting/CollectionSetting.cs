namespace KJ1012.Domain.Setting
{
    public class CollectionSetting
    {
        /// <summary>
        /// 支持最大分站数
        /// </summary>
        public int MaxSubstationCount { get; set; } = 16;
        /// <summary>
        /// 分站端口号
        /// </summary>
        public int SubStationPort { get; set; } = 6061;
        /// <summary>
        /// 地面信息接口端口号
        /// </summary>
        public int UpDataInterfacePort{ get; set; } = 6062;
        /// <summary>
        /// 井下接口端口号
        /// </summary>
        public int DownDataInterfacePort { get; set; } = 6063;
        /// <summary>
        /// 设备维护工具端口号
        /// </summary>
        public int ToolInterfacePort { get; set; } = 6064;
        /// <summary>
        /// 误码测试程序端口号
        /// </summary>
        public int ErrorRateInterfacePort { get; set; } = 6065;
        /// <summary>
        /// TCP服务端口号
        /// </summary>
        public int SocketServerPort { get; set; } = 6060;
        public int SocketServerPort2 { get; set; } = 7070;
        /// <summary>
        /// 矿井编号
        /// </summary>
        public int UserId { get; set; } = 0;
    }
}
