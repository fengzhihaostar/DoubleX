using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 缓存操作抽象方法
    /// </summary>
    public abstract class AbsCaching
    {
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <returns>List[object]</returns>
        public virtual List<object> Query()
        {
            return new List<object>();
        }

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回object</returns>
        abstract public object Get(string key);

        /// <summary>
        /// 获取缓存(根据缓存Key)
        /// </summary>
        /// <typeparam name="TEntity">返回的类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>返回的类型对象</returns>
        abstract public TEntity Get<TEntity>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        abstract public void Set(string key, object obj);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="expirationDateTime">过期时间(绝对即：指定在XXX时候过期)</param>
        abstract public void Set(string key, object obj, DateTime expirationDateTime);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="slidingExpirationTimeSpan">过期时间(相对即：多少时间内未使用过期)</param>
        abstract public void Set(string key, object obj, TimeSpan slidingExpirationTimeSpan);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键Key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="filePaths">文件依赖</param>
        abstract public void Set(string key, object obj, List<string> filePaths);

        /// <summary>
        /// 移除缓存(根据缓存Key)
        /// </summary>
        /// <param name="key">键Key</param>
        abstract public void Remove(string key);

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        abstract public void Clear();

    }
}
