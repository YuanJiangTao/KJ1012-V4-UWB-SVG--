
namespace KJ1012.Core.Plugins
{
    /// <summary>
    /// 代表应用程序的描述符扩展
    /// </summary>
    public interface IDescriptor
    {
        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        string SystemName { get; set; }

        /// <summary>
        /// 获取或设置友好显示名称
        /// </summary>
         string FriendlyName { get; set; }
    }
}