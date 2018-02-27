using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Session工具类
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key">session key</param>
        /// <returns>返回object</returns>
        public static object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key">session key</param>
        /// <param name="obj">内容对象</param>
        public static void Set(string key, object obj)
        {
            HttpContext.Current.Session.Remove(key);
            HttpContext.Current.Session.Add(key, obj);
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key">session key</param>
        /// <param name="obj">内容对象</param>
        /// <param name="expiration">过期时间</param>
        public static void Set(string key, object obj, DateTime expiration)
        {
            HttpContext.Current.Session.Remove(key);
            HttpContext.Current.Session.Add(key, obj);
            HttpContext.Current.Session.Timeout = (int)(expiration - DateTime.Now).TotalMinutes; //有问题好像全局了
        }

        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="key">session key</param>
        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        /// <summary>
        /// 清空Session
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
