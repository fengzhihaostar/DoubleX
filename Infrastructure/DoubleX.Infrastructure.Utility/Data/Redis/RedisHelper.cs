using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Redis 辅助类
    /// </summary>
    public class RedisHelper
    {
        #region RedisClient全局唯一

        private static PooledRedisClientManager manage = null;

        private static object _lock = new object();

        public static void Init(string redisUri)
        {
            manage = CreateManage(redisUri);
        }

        public static PooledRedisClientManager GetManage()
        {
            lock (_lock)
            {
                if (manage == null)
                    throw new Exception("PooledRedisClientManager is Null");
                return manage;
            }
        }

        #endregion

        #region 设置获取Redis信息

        /// <summary>
        /// 设置Redis对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <param name="model"></param>
        /// <param name="expiresAt"></param>
        /// <param name="timespan"></param>
        /// <param name="redisUri"></param>
        /// <returns></returns>
        public static bool Set<TEntity>(string key, TEntity model, DateTime? expiresAt = null, TimeSpan? timespan = null, string redisUri = null)
        {
            #region old

            //bool res = false;
            //try
            //{
            //    IRedisClient client = null;
            //    if (!string.IsNullOrWhiteSpace(redisUri))
            //    {
            //        client = CreateClietManage(redisUri);
            //    }
            //    if (client == null)
            //    {
            //        client = GetClient();
            //    }
            //    if (client == null)
            //    {
            //        return false;
            //    }

            //    if (expiresAt != null)
            //    {
            //        res = client.Set<TEntity>(key, model, expiresAt.Value);
            //    }
            //    else if (timespan != null)
            //    {
            //        res = client.Set<TEntity>(key, model, timespan.Value);
            //    }
            //    else
            //    {
            //        res = client.Set<TEntity>(key, model);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    res = false;
            //}
            //return res;

            #endregion
            bool res = false;
            using (var client = GetManage().GetClient())
            {
                try
                {
                    if (expiresAt != null)
                    {
                        res = client.Set<TEntity>(key, model, expiresAt.Value);
                    }
                    else if (timespan != null)
                    {
                        res = client.Set<TEntity>(key, model, timespan.Value);
                    }
                    else
                    {
                        res = client.Set<TEntity>(key, model);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return res;
        }

        /// <summary>
        /// 获取Redis对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <param name="redisUri"></param>
        /// <returns></returns>
        public static TEntity Get<TEntity>(string key, string redisUri = null)
        {
            var model = default(TEntity);
            using (var client = GetManage().GetClient())
            {
                try
                {
                    model = client.Get<TEntity>(key);
                }
                catch (Exception ex)
                {
                }
            }
            return model;

            //try
            //{
            //    IRedisClient client = null;
            //    if (!string.IsNullOrWhiteSpace(redisUri))
            //    {
            //        client = CreateClietManage(redisUri);
            //    }
            //    if (client == null)
            //    {
            //        client = GetClient();
            //    }
            //    if (client == null)
            //    {
            //        return default(TEntity);
            //    }

            //    return client.Get<TEntity>(key);
            //}
            //catch (Exception ex)
            //{
            //}
            //return default(TEntity);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取RedisClient
        /// </summary>
        /// <param name="redisUri"></param>
        /// <returns></returns>
        private static IRedisClient CreateClietManage(string redisUri)
        {
            if (VerifyHelper.IsEmpty(redisUri))
                return null;


            var list = StringHelper.ToKeyValueList(redisUri);
            if (VerifyHelper.IsEmpty(list))
                return null;

            try
            {
                var reads = list.FirstOrDefault(x => String.Equals(x.Key, "Reads", StringComparison.CurrentCultureIgnoreCase)).Value.Split(',');
                var writes = list.FirstOrDefault(x => String.Equals(x.Key, "Writes", StringComparison.CurrentCultureIgnoreCase)).Value.Split(',');
                var maxWritePoolSize = IntHelper.Get(list.FirstOrDefault(x => String.Equals(x.Key, "MaxWritePoolSize", StringComparison.CurrentCultureIgnoreCase)).Value);
                var maxReadPoolSize = IntHelper.Get(list.FirstOrDefault(x => String.Equals(x.Key, "MaxReadPoolSize", StringComparison.CurrentCultureIgnoreCase)).Value);
                var autoStart = BoolHelper.Get(list.FirstOrDefault(x => String.Equals(x.Key, "AutoStart", StringComparison.CurrentCultureIgnoreCase)).Value);

                PooledRedisClientManager manage = new PooledRedisClientManager(reads, writes, new RedisClientManagerConfig
                {
                    MaxWritePoolSize = maxWritePoolSize,
                    MaxReadPoolSize = maxReadPoolSize,
                    AutoStart = autoStart,
                });
                return manage.GetClient();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static PooledRedisClientManager CreateManage(string redisUri)
        {
            if (VerifyHelper.IsEmpty(redisUri))
                throw new Exception("RedisUri is Null");


            var list = StringHelper.ToKeyValueList(redisUri);
            if (VerifyHelper.IsEmpty(list))
                throw new Exception("RedisUri is Null");

            try
            {
                var reads = list.FirstOrDefault(x => String.Equals(x.Key, "Reads", StringComparison.CurrentCultureIgnoreCase)).Value.Split(',');
                var writes = list.FirstOrDefault(x => String.Equals(x.Key, "Writes", StringComparison.CurrentCultureIgnoreCase)).Value.Split(',');
                var maxWritePoolSize = IntHelper.Get(list.FirstOrDefault(x => String.Equals(x.Key, "MaxWritePoolSize", StringComparison.CurrentCultureIgnoreCase)).Value);
                var maxReadPoolSize = IntHelper.Get(list.FirstOrDefault(x => String.Equals(x.Key, "MaxReadPoolSize", StringComparison.CurrentCultureIgnoreCase)).Value);
                var autoStart = BoolHelper.Get(list.FirstOrDefault(x => String.Equals(x.Key, "AutoStart", StringComparison.CurrentCultureIgnoreCase)).Value);

                PooledRedisClientManager manage = new PooledRedisClientManager(reads, writes, new RedisClientManagerConfig
                {
                    MaxWritePoolSize = maxWritePoolSize,
                    MaxReadPoolSize = maxReadPoolSize,
                    AutoStart = autoStart,
                });
                return manage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
