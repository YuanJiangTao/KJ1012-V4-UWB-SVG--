namespace KJ1012.Core.Plugins
{
    /// <summary>
    /// 指定加载插件的方式
    /// </summary>
    public enum LoadPluginsMode
    {
        /// <summary>
        /// 加载所有
        /// </summary>
        All = 0,
        /// <summary>
        /// 加载已注册插件
        /// </summary>
        InstalledOnly = 10,
        /// <summary>
        /// 只加载未注册插件
        /// </summary>
        NotInstalledOnly = 20
    }
}