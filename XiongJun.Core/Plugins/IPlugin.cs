using System;
using System.Collections.Generic;
using System.Text;

namespace XiongJun.Core.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        string GetConfigurationPageUrl();

        /// <summary>
        /// Gets or sets the plugin descriptor
        /// </summary>
        PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// Install plugin
        /// </summary>
        void Install();

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        void Uninstall();
    }
}
