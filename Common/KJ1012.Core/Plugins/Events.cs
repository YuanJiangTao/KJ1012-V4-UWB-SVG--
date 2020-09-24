using System.Collections.Generic;

namespace KJ1012.Core.Plugins
{
    /// <summary>
    /// 插件上传事件
    /// </summary>
    public class PluginsUploadedEvent
    {
        #region Ctor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uploadedPlugins">上传插件</param>
        public PluginsUploadedEvent(IList<PluginDescriptor> uploadedPlugins)
        {
            this.UploadedPlugins = uploadedPlugins;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 上传插件
        /// </summary>
        public IList<PluginDescriptor> UploadedPlugins { get; private set; }

        #endregion
    }
}