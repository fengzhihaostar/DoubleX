using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 信息校验类
    /// </summary>
    public class VerifyHelper
    {
        /// <summary>
        /// 判断对象是否为Null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(object obj) {
            return obj == null;
        }

        /// <summary>
        /// 判断对象是否Null或空(string null/"",list null/0,arr null/0)
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty(object obj) {

            if (obj == null)
                return true;

            string objType = obj.GetType().FullName;

            if (objType == "System.String" || objType == "Newtonsoft.Json.Linq.JValue")
            {
                return string.IsNullOrWhiteSpace(obj.ToString()) ? true : false;
            }
            else if (objType == "System.Guid")
            {
                return GuidHelper.Get(obj, defaultValue: Guid.Empty) == Guid.Empty;
            }
            //System.Web.HttpValueCollection(NameValueCollection)
            //Newtonsoft.Json.Linq.JArray
            return false;
        }

        /// <summary>
        /// 判断是否空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断是否空Guid（NULL Empty）
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsEmpty(Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty(ICollection list)
        {
            return !(list != null && list.Count > 0);
        }


        /// <summary>
        /// 判断是否支持Context
        /// </summary>
        public static bool IsSupperHttpContext()
        {
            //上下文无效(多线程，当前为非请求线程 )
            return HttpContext.Current != null;
        }

        /// <summary>
        /// 判断是否支持Cookie
        /// </summary>
        /// <returns></returns>
        public static bool IsSupperHttpCookie()
        {
            //上下文无效(多线程，当前为非请求线程 )
            var context = HttpContext.Current;
            if (context==null)
                return false;

            //判断是否有Cookies(Golable Start 无Cookies)
            return context.Request.Cookies != null;
        }

        /// <summary>
        /// 判断是否为IP地址
        /// </summary>
        public static bool IsIP(string  ipStr)
        {
            if (string.IsNullOrWhiteSpace(ipStr))
                return false;
            return Regex.IsMatch(ipStr, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 判断是否Ajax请求(WebForm)
        /// </summary>
        public static bool IsAjax(HttpContext context)
        {
            if (context == null)
                return false;
            return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        /// 判断是否Ajax请求(Mvc)
        /// </summary>
        public static bool IsAjax(HttpContextBase context = null)
        {
            if (context != null)
            {
                return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        }
    }
}
