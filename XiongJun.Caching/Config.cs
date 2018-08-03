using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiongJun.Caching
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
    }
}
