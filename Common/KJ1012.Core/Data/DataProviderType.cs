using System.Runtime.Serialization;

namespace KJ1012.Core.Data
{
    /// <summary>
    /// 数据库类型枚举值
    /// </summary>
    public enum DataProviderType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "SqlServer")]
        SqlServer
    }
}