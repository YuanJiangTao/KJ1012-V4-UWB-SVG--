using System.Collections.Generic;

namespace KJ1012.Core.Plugins
{
    /// <summary>
    /// Plugin finder
    /// </summary>
    public interface IPluginFinder
    {
        /// <summary>
        /// 获取所有插件组名
        /// </summary>
        /// <returns>Plugins groups</returns>
        IEnumerable<string> GetPluginGroups();

        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <typeparam name="T">要获取的插件类型</typeparam>
        /// <param name="loadMode">加载插件的方式</param>
        /// <param name="group">根据组名称筛选插件，当组名为空时查询所有插件</param>
        /// <returns>插件列表</returns>
        IEnumerable<T> GetPlugins<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly, string group = null) where T : class, IPlugin;

        /// <summary>
        /// 获取插件信息列表
        /// </summary>
        /// <param name="loadMode">加载插件的方式</param>
        /// <param name="group">根据组名称筛选插件，当组名为空时查询所有插件</param>
        /// <returns>插件信息</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly, string group = null);

        /// <summary>
        /// 获取插件信息列表
        /// </summary>
        /// <typeparam name="T">要获取的插件类型</typeparam>
        /// <param name="loadMode">加载插件的方式</param>
        /// <param name="group">根据组名称筛选插件，当组名为空时查询所有插件</param>
        /// <returns>插件信息列表</returns>
        IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly, string group = null) where T : class, IPlugin;

        /// <summary>
        /// 根据插件系统名称获取插件信息列表
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        /// <param name="loadMode">加载插件的方式</param>
        /// <returns>>插件信息</returns>
        PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly);

        /// <summary>
        /// 根据插件系统名称获取插件信息
        /// </summary>
        /// <typeparam name="T">要获取的插件类型</typeparam>
        /// <param name="systemName">插件系统名称</param>
        /// <param name="loadMode">加载插件的方式</param>
        /// <returns>>插件信息</returns>
        PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.InstalledOnly)
            where T : class, IPlugin;

        /// <summary>
        /// 重新加载插件
        /// </summary>
        void ReloadPlugins();
    }
}