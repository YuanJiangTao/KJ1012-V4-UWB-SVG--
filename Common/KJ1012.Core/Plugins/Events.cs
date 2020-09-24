using System.Collections.Generic;

namespace KJ1012.Core.Plugins
{
    /// <summary>
    /// ����ϴ��¼�
    /// </summary>
    public class PluginsUploadedEvent
    {
        #region Ctor

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="uploadedPlugins">�ϴ����</param>
        public PluginsUploadedEvent(IList<PluginDescriptor> uploadedPlugins)
        {
            this.UploadedPlugins = uploadedPlugins;
        }

        #endregion

        #region Properties

        /// <summary>
        /// �ϴ����
        /// </summary>
        public IList<PluginDescriptor> UploadedPlugins { get; private set; }

        #endregion
    }
}