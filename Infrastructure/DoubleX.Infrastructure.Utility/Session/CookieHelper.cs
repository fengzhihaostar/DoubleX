using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Cookie工具类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 获取Cookie(根据CookieKey)
        /// </summary>
        /// <param name="key">Cookie Key</param>
        /// <returns>返回string</returns>
        public static string Get(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            string str = "";
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">Cookie Key</param>
        /// <param name="value">Cookie值</param>
        public static void Set(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">Cookie Key</param>
        /// <param name="value">Cookie值</param>
        /// <param name="expirationDateTime">过期时间</param>
        public static void Set(string key, string value, DateTime? expirationDateTime)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value
            };
            if (expirationDateTime != null)
            {
                cookie.Expires = expirationDateTime.Value;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="key">Cookie Key</param>
        public static void Remove(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Remove(key);//集合中除不是浏览器真删
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 清空Cookie
        /// </summary>
        public static void Clear()
        {
           int count = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < count; i++)
            {
                HttpCookie hc = HttpContext.Current.Request.Cookies[i];
                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Remove(hc.Name);
                HttpContext.Current.Response.Cookies.Add(hc);
            }

        }
    }
}
