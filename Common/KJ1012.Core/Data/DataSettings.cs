using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KJ1012.Core.Data
{
    /// <summary>
    /// 数据库配置信息
    /// </summary>
    public class DataSettings
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DataProviderType DataProvider { get; set; } = DataProviderType.Unknown;
        /// <summary>
        /// 数据库版本
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// 判断数据库信息是否已配置
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public bool IsValid => DataProvider != DataProviderType.Unknown && !string.IsNullOrEmpty(ConnectionString);
    }
}