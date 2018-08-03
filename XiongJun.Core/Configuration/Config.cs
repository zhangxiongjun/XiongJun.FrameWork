using System;
using System.Collections.Generic;
using System.Text;

namespace XiongJun.Core.Configuration
{
    public class Config
    {
        /// <summary>
        /// Gets or sets a value indicating whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        
        /// <summary>
        /// Gets or sets Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; set; }

        public bool UseUnsafeLoadAssembly { get; set; }

        public bool ClearPluginShadowDirectoryOnStartup { get; set; }
    }
}
