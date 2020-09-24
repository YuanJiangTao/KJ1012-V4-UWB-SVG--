namespace KJ1012.Core.Plugins
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 获取配置管理URL地址
        /// </summary>
        string GetConfigurationPageUrl();

        /// <summary>
        /// 获取或设置插件描述信息
        /// </summary>
        PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// 注册插件
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载插件
        /// </summary>
        void Uninstall();
    }
}
