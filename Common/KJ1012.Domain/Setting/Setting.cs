using System.Collections.Generic;

namespace KJ1012.Domain.Setting
{
    public class Setting
    {

        /// <summary>
        /// 人员超时时间(分钟)
        /// </summary>
        public int DownMemberTimeOut { get; set; } = 480;

        /// <summary>
        /// 矿井最大容纳人数
        /// </summary>
        public int MaxMember { get; set; } = 1000;
        /// <summary>
        /// 设备异常延迟推送时间（秒）
        /// </summary>
        public int DeviceWarnDelay { get; set; } = 300;
        /// <summary>
        /// 终端异常报警延迟（秒）
        /// </summary>
        public int TerminalWarnDelay { get; set; } = 60;

        public string MqttAddress { get; set; } = "127.0.0.1;";
        /// <summary>
        /// Redis的连接地址
        /// </summary>
        public string RedisConnection { get; set; }

        /// <summary>
        /// 地图比例尺（1：？）
        /// </summary>
        public float ScaleOfMap { get; set; } = 1;
        /// <summary>
        /// 车辆行驶的最大速度，单位m/s
        /// </summary>
        public int MaxDrivingSpeed { get; set; }
        /// <summary>
        /// 人行走的最大速度，单位m/s
        /// </summary>
        public int MaxWalkingSpeed { get; set; }

        /// <summary>
        /// 获取参考站时，查询距离最小值的定位数据
        /// </summary>
        public int PositionMinDistance { get; set; } = 30;
        /// <summary>
        /// 轨迹追随配置文件
        /// </summary>
        public List<PositionFollow> PositionFollows { get; set; } = new List<PositionFollow>();

        /// <summary>
        /// 机车定位卡开始标识卡号段
        /// </summary>
        public int LocMinTerminalId { get; set; } = 30000;
        /// <summary>
        /// 设备状态未知检测周期（单位：分钟）
        /// </summary>
        public int DeviceStateUnknownInterval { get; set; } = 15;

        /// <summary>
        /// 日志文件最大保存天数
        /// </summary>
        public int MaxDayLogSave { get; set; } = 7;

        /// <summary>
        /// 容许未录入系统终端处理
        /// </summary>
        public bool AllowNotInsert { get; set; } = false;

        /// <summary>
        /// 屏蔽异常设备信息
        /// </summary>
        public bool CloseDeviceWarn { get; set; } = false;
        /// <summary>
        /// 采集主机IP
        /// </summary>
        public string HostIp { get; set; }
        /// <summary>
        /// 数据库主机IP
        /// </summary>
        public string DataHostIp { get; set; }

        public int AttendanceId { get; set; } = 1001;
        //针对账号是否启用部门数据过滤
        public bool IsUseOrgFilter { get; set; } = false;
        /// <summary>
        /// 唯一性检测系统IP地址
        /// </summary>
        public string UniqueServiceIp { get; set; }
        /// <summary>
        /// 唯一性检测系统端口
        /// </summary>
        public string UniqueServicePort { get; set; }
        /// <summary>
        /// 唯一性检测系统端口
        /// </summary>
        public string UniqueCollectionPort { get; set; }
        /// <summary>
        /// 是否启用唯一性
        /// </summary>
        public bool IsUseUnique { get; set; } = false;

        /// <summary>
        /// 是否启用单点登录，同一账号只能一处登录
        /// </summary>
        public bool IsUseSingleOn { get; set; } = false;

        /// <summary>
        /// 是否启用设备报警联动，上级断线下级断线
        /// </summary>
        public bool IsUseDeviceLinkage { get; set; } = false;
        /// <summary>
        /// 是否启用设备报警联动，上级未恢复下级不能恢复
        /// </summary>
        public bool IsUseDeviceLinkage2 { get; set; } = false;
        /// <summary>
        /// 执行入井时是否判定轨迹的基站已在系统录入
        /// </summary>
        public bool IsAttendanceCheckStation { get; set; } = false;
        /// <summary>
        /// 人员超时时是否发生寻呼通知消息
        /// </summary>
        public bool IsTimeOutSendCall { get; set; } = false;
        /// <summary>
        /// 超时时发送报警文本消息
        /// </summary>
        public string TimeOutSendMessage { get; set; } = "您已在井下工作超时，请及时升井";
        /// <summary>
        /// 距离校准参数
        /// </summary>
        public int CheckDistance { get; set; } = 56;


        public Dictionary<string, NumberRange> TerminalIdRange { get; set; } = new Dictionary<string, NumberRange>();

    }
}
