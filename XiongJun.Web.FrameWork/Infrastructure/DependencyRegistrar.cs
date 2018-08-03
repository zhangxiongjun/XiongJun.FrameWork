using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using XiongJun.Caches.RedisCache;
using XiongJun.Core.Configuration;
using XiongJun.Core.Infrastructure;

namespace XiongJun.Web.FrameWork.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, Core.Configuration.Config config)
        {
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            }
        }
    }
}
