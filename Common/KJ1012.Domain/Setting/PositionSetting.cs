using System.Collections.Generic;

namespace KJ1012.Domain.Setting
{
    public class PositionSetting
    {
        /// <summary>
        /// 不可驾驶区域
        /// </summary>
        public int[] WalkingArea { get; set; }
        /// <summary>
        /// 参考站的方向配置
        /// </summary>
        public Dictionary<string, int[]> DirectionArray { get; set; }
        //配置路线中有相同段距离的
        public Dictionary<string, Dictionary<string, int>> SameDistanceConfig { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        public List<StationDistanceModel> StationDistance { get; set; } = new List<StationDistanceModel>();
    }
}
