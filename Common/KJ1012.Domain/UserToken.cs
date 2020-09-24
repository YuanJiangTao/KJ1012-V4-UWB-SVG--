using System;
using System.Net;
using System.Net.Sockets;

namespace KJ1012.Domain
{
    public class UserToken
    {
        public int Id { get; set; }
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>
        /// 远程地址
        /// </summary>
        public EndPoint Remote { get; set; }

        /// <summary>
        /// 通信SOKET
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
        /// <summary>
        /// 连接对象类型
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 工作方式
        /// </summary>
        public int WorkingMode { get; set; } = 0;

    public string Key => DeviceType.ToString("00") + Id.ToString("X2");
    }
}
