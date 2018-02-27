using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Infrastructure.Core.Caching
{
    /// <summary>
    /// 缓存操作辅助类(自定义提供者，提共都需继承AbsCaching抽象类)
    /// </summary>
    public class CachingHelper<TProvider> where TProvider : AbsCaching, new()
    {
        /// <summary>
        /// 缓存提共者
        /// </summary>
        protected static AbsCaching cacheManager
        {
            get
            {
                return new TProvider() as AbsCaching;
            }
        }

        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <returns>List[object]</returns>
        public static List<object> Query()
        {
            return cacheManager.Query();
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回object</returns>
        public static object Get(string key)
        {
            return cacheManager.Get(key);
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <typeparam name="TEntity">返回的类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>返回的类型对象</returns>
        public static TEntity Get<TEntity>(string key)
        {
            return cacheManager.Get<TEntity>(key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        public static void Set(string key, object obj)
        {
            cacheManager.Set(key, obj);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="expirationDateTime">过期时间(绝对即：指定在XXX时候过期)</param>
        public static void Set(string key, object obj, DateTime expirationDateTime)
        {
            cacheManager.Set(key, obj, expirationDateTime);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// 
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="slidingExpirationTimeSpan">过期时间(相对即：多少时间内未使用过期)</param>
        public static void Set(string key, object obj, TimeSpan slidingExpirationTimeSpan)
        {
            cacheManager.Set(key, obj, slidingExpirationTimeSpan);
        }

        /// <summary>
        /// 移除缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">键Key</param>
        public static void Remove(string key)
        {
            cacheManager.Remove(key);
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void Clear()
        {
            cacheManager.Clear();
        }
    }

    /// <summary>
    /// 缓存操作辅助类(使用默认提供都RuntimeCaching)
    /// </summary>
    public class CachingHelper : CachingHelper<RuntimeCaching>
    {
    }
}
