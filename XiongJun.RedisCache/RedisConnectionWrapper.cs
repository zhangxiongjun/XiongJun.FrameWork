using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RedLock;
using StackExchange.Redis;

namespace XiongJun.Caching.RedisCache
{
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        #region Fields

        private readonly Config _config;

        private readonly Lazy<string> _connectionString;
        //volatile的作用是它可以保证线程之间的可见性（可以理解为1号线程写完了后2号负责读的线程是可见的，所以2号读线程始终可以得到最新的值）和指令的顺序性（专治乱序问题，weak ordering）
        private volatile ConnectionMultiplexer _connection;
        private volatile RedisLockFactory _redisLockFactory;
        private readonly object _lock = new object();

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="config">Config</param>
        public RedisConnectionWrapper(Config config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
            this._redisLockFactory = CreateRedisLockFactory();
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Get connection string to Redis cache from configuration
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        /// <summary>
        /// Get connection to Redis servers
        /// </summary>
        /// <returns></returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                //Connection disconnected. Disposing connection...
                _connection?.Dispose();

                //Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// Create instance of RedisLockFactory
        /// </summary>
        /// <returns>RedisLockFactory</returns>
        protected RedisLockFactory CreateRedisLockFactory()
        {
            //get password and value whether to use ssl from connection string
            var password = string.Empty;
            var useSsl = false;
            foreach (var option in _connectionString.Value.Split(',').Where(option => option.Contains('=')))
            {
                switch (option.Substring(0, option.IndexOf('=')).Trim().ToLowerInvariant())
                {
                    case "password":
                        password = option.Substring(option.IndexOf('=') + 1).Trim();
                        break;
                    case "ssl":
                        bool.TryParse(option.Substring(option.IndexOf('=') + 1).Trim(), out useSsl);
                        break;
                }
            }



            //create RedisLockFactory for using Redlock distributed lock algorithm
            return new RedisLockFactory(GetEndPoints().Select(endPoint => new RedisLockEndPoint
            {
                EndPoint = endPoint,
                Password = password,
                Ssl = useSsl
            }));
        }

        #endregion

        public void Dispose()
        {
            //dispose ConnectionMultiplexer
            _connection?.Dispose();

            //dispose RedisLockFactory
            _redisLockFactory?.Dispose();
        }

        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1);
            }
        }

        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            //use RedLock library
            using (var redisLock = _redisLockFactory.Create(resource, expirationTime))
            {
                //ensure that lock is acquired
                if (!redisLock.IsAcquired)
                    return false;

                //perform action
                action();

                return true;
            }
        }
    }
}
