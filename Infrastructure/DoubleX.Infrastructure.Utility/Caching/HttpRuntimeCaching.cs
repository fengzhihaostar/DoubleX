using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 应用程序的Cache
    /// </summary>
    public class HttpRuntimeCaching: AbsCaching
    {
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <returns>List[object]</returns>
        public override List<object> Query()
        {
            List<object> list = new List<object>();
            IDictionaryEnumerator Enum = HttpRuntime.Cache.GetEnumerator();
            while (Enum.MoveNext())
            {
                list.Add(Enum);
            }
            return list;
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回object</returns>
        public override object Get(string key)
        {
            Cache cache = HttpRuntime.Cache;
            if (cache != null)
            {
                return cache[key];
            }
            return null;
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <typeparam name="TEntity">返回的类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>返回的类型对象</returns>
        public override TEntity Get<TEntity>(string key)
        {
            return default(TEntity);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        public override void Set(string key, object obj)
        {
            Cache cache = HttpRuntime.Cache;
            if (cache != null)
            {
                cache.Insert(key, obj);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="expirationDateTime">过期时间(绝对即：指定在XXX时候过期)</param>
        public override void Set(string key, object obj, DateTime expirationDateTime)
        {

            Cache cache = HttpRuntime.Cache;
            if (cache != null)
            {
                cache.Insert(key, obj, null, expirationDateTime, Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="slidingExpirationTimeSpan">过期时间(相对即：多少时间内未使用过期)</param>
        public override void Set(string key, object obj, TimeSpan slidingExpirationTimeSpan)
        {
            Cache cache = HttpRuntime.Cache;
            if (cache != null)
            {
                cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, slidingExpirationTimeSpan);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="filePaths">文件依赖</param>
        public override void Set(string key, object obj, List<string> filePaths)
        {
            if (filePaths != null && filePaths.Count>0)
            {
                CacheDependency dependencies = new CacheDependency(filePaths.ToArray());
                Cache cache = HttpRuntime.Cache;
                if (cache != null)
                {
                    cache.Insert(key, obj, dependencies);
                }
            }
        }

        /// <summary>
        /// 移除缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">键Key</param>
        public override void Remove(string key)
        {
            Cache cache = HttpRuntime.Cache;
            if (cache != null)
            {
                cache.Remove(key);
            }
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public override void Clear()
        {
            Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator Enum = cache.GetEnumerator();
            while (Enum.MoveNext())
            {
                cache.Remove(Enum.Key.ToString());
            }
        }
    }
}
