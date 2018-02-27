using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Caching;

namespace DoubleX.Infrastructure.Core.Config
{
    /// <summary>
    /// 配置文件操作基类
    /// </summary>
    public abstract class AbsConfigs<TEntity> where TEntity : class,new()
    {
        /// <summary>
        /// 配置文件初始
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="cacheKey">缓存Key</param>
        protected static void Init(string sectionName, string cacheKey)
        {
            TEntity section = ConfigHelper.GetSection<TEntity>(sectionName);
            if (section != null)
            {
                CachingHelper.Set(cacheKey, section);
            }
        }


        /// <summary>
        /// 根据Key获取配置项
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="key">Key键</param>
        /// <param name="groupKey">组key</param>
        /// <returns>返回Value</returns>
        protected static ConfigListItem GetDefaultItem(string cacheKey, string key, string groupKey = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            TEntity section = CachingHelper.Get(cacheKey) as TEntity;
            if (section != null)
            {
                var item = ConfigHelper.GetDefaultItem<TEntity,ConfigListItem>(section, key);
                return item;
            }
            return null;
        }

        /// <summary>
        /// 根据Key获取配置项
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="key">Key键</param>
        /// <param name="groupKey">组key</param>
        /// <returns>返回Value</returns>
        protected static ConfigListItem GetGroupItem(string cacheKey, string key, string groupKey = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            TEntity section = CachingHelper.Get(cacheKey) as TEntity;
            if (section != null)
            {
                var item = ConfigHelper.GetGroupItem<TEntity, ConfigGroupItem, ConfigListItem>(section, groupKey, key);
                return item;
            }
            return null;
        }
    }
}
