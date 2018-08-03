using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XiongJun.Caches.RedisCache;
using XiongJun.Core.Infrastructure;

namespace Libraries.UnitTest
{
    [TestFixture]
    public class RedisCaheTest
    {
        public RedisCaheTest() { }

        [Test]
        public void TestRedisCache()
        {
            var config = new Config()
            {
                RedisCachingEnabled=true,
                RedisCachingConnectionString= "192.168.3.61:6379,abortConnect=false,syncTimeout=3000,DatabaseId=11"
            };
            IRedisConnectionWrapper _IRedisConnectionWrapper=new RedisConnectionWrapper(config);
          var RedisCacheManager=new RedisCacheManager(null, _IRedisConnectionWrapper,config);
            RedisCacheManager.Set("aa","dddd",3000);
        }
    }
}
