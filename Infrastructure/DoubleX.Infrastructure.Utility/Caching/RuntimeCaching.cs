using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 应用程序缓存
    /// </summary>
    public class RuntimeCaching:  AbsCaching
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        private ObjectCache objectCache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// 锁对象
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <returns>List[object]</returns>
        public override List<object> Query()
        {
            return new List<object>();
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回object</returns>
        public override object Get(string key)
        {
            return objectCache[key];
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <typeparam name="TEntity">返回的类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>返回的类型对象</returns>
        public override TEntity Get<TEntity>(string key)
        {
            var obj = objectCache[key];
            if (obj == null)
                return default(TEntity);
            return (TEntity)obj;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        public override void Set(string key, object obj)
        {
            Set<object>(key, obj);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="expirationDateTime">过期时间(绝对即：指定在XXX时候过期)</param>
        public override void Set(string key, object obj, DateTime expirationDateTime)
        {
            Set<object>(key, obj, absoluteExpiration: expirationDateTime);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="slidingExpirationTimeSpan">过期时间(相对即：多少时间内未使用过期)</param>
        public override void Set(string key, object obj, TimeSpan slidingExpirationTimeSpan)
        {
            Set<object>(key, obj, slidingExpiration: slidingExpirationTimeSpan);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="filePaths">文件依赖</param>
        public override void Set(string key, object obj, List<string> filePaths)
        {
            Set<object>(key, obj, filePaths: filePaths);
        }

        /// <summary>
        /// 移除缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">键Key</param>
        public override void Remove(string key)
        {

        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public override void Clear()
        {

        }

        /// <summary>
        /// 设置缓存(absoluteExpiration:过期时间; slidingExpiration:给定活动时间，该时间内未活动，则过期; filesPath:文件依赖)
        /// </summary>
        /// <typeparam name="TEntity">缓存对象类型</typeparam>
        /// <param name="key">根据缓存Key</param>
        /// <param name="obj">缓存内容</param>
        /// <param name="absoluteExpiration">绝对(指定时间)过期</param>
        /// <param name="slidingExpiration">相对(使用间隔)过期</param>
        /// <param name="filePaths">文件依赖</param>
        public void Set<TEntity>(string key, TEntity obj, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null, List<string> filePaths = null)
        {
            var item = new CacheItem(key, obj);
            var policy = CreatePolicy(absoluteExpiration, slidingExpiration, filePaths);
            lock (locker)
            {
                if (objectCache[key] == null)
                {
                    objectCache.Add(item, policy);
                }
                else
                {
                    objectCache.Set(item, policy);
                }
            }
        }

        /// <summary>
        /// 设置缓存过期策略及文件依赖
        /// </summary>
        /// <param name="absoluteExpiration">绝对(指定时间)过期</param>
        /// <param name="slidingExpiration">相对(使用间隔)过期</param>
        /// <param name="filePaths">文件依赖</param>
        /// <returns>CacheItemPolicy</returns>
        private CacheItemPolicy CreatePolicy(DateTime? absoluteExpiration, TimeSpan? slidingExpiration, List<string> filePaths = null)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }
            if (filePaths != null && filePaths.Count > 0)
            {
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));
            }
            policy.Priority = CacheItemPriority.Default;
            return policy;
        }
    }
}
