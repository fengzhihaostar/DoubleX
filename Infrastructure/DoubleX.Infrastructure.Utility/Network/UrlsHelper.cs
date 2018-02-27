using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// /连接地址辅助类
    /// </summary>
    public class UrlsHelper
    {
        /// <summary>
        /// 获取当前请求Url(包含端口号&参数)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentReqeustUrl()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                return HttpContext.Current.Request.Url.ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取Url信息
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="isAndPath">是否包含路径</param>
        /// <returns>返回Url</returns>
        public static string GetUrl(string url = null, bool isAndPath = true)
        {
            url = string.IsNullOrWhiteSpace(url) ? GetCurrentReqeustUrl() : url;
            if (string.IsNullOrWhiteSpace(url))
                return "";

            Uri uri = new Uri(url);
            if (uri != null)
            {
                url = isAndPath ? string.Format("{0}://{1}/{2}", uri.Scheme, uri.Authority, uri.AbsolutePath) : string.Format("{0}://{1}", uri.Scheme, uri.Authority);
                url = url.TrimEnd('?');
                while (url.Contains("//"))
                {
                    url = url.Replace("//", "/");
                }
                url = url.Replace(":/", "://");
            }
            return url;
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        public static string GetDomain(string url = null)
        {
            url = string.IsNullOrWhiteSpace(url) ? GetCurrentReqeustUrl() : url;
            if (string.IsNullOrWhiteSpace(url))
                return "";

            Regex reg = new Regex(@"(http|https|ftp)://(?<domain>[^(:|/]*)", RegexOptions.IgnoreCase);
            Match m = reg.Match(url);
            return m.Groups["domain"].Value;
        }

        /// <summary>
        /// 获取Url参数部份
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetQueryString(string url = null)
        {
            url = string.IsNullOrWhiteSpace(url) ? GetCurrentReqeustUrl() : url;
            if (string.IsNullOrWhiteSpace(url))
                return "";

            Uri uri = new Uri(url);
            return string.IsNullOrWhiteSpace(uri.Query) ? "" : Decode(uri.Query.ToString().Trim('?'));
        }

        /// <summary>
        /// 获取Url参数部份并转为Lis[key,value]
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetQueryList(string url)
        {
            var queryString = GetQueryString(url);
            if (string.IsNullOrWhiteSpace(queryString))
            {
                return new List<KeyValuePair<string, string>>();
            }
            var queryList = new List<KeyValuePair<string, string>>();
            queryString.Split('&').ToList().ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x))
                {
                    var arr = x.Split('=');
                    queryList.Add(new KeyValuePair<string, string>(arr[0], arr.Length == 2 ? arr[1] : ""));
                }
            });
            return queryList;
        }

        /// <summary>
        /// 获取当前请求Query的值
        /// </summary>
        public static string GetQueryValue(string key, string url = null)
        {

            if (string.IsNullOrWhiteSpace(key))
                return "";

            string value = GetQueryList(url).Where(x => string.Compare(x.Key, key, true) == 0).FirstOrDefault().Value;

            //安全检测

            return string.IsNullOrWhiteSpace(value) ? "" : value;
        }

        /// <summary>
        /// 获取来源页面
        /// </summary>
        public static string GetRefUrl(string key = "_ref", string defaultUrl = "/", string url = null)
        {
            //如果当前请求地址中包含key
            var refUrl = GetQueryValue(key, url);

            if (string.IsNullOrWhiteSpace(refUrl) &&
                HttpContext.Current != null &&
                HttpContext.Current.Request != null &&
                HttpContext.Current.Request.UrlReferrer != null)
            {
                //ajax 访问中ajax的地址元_ref参数，但请求ajax的来源页中包含url
                var sourceRefUrl = GetQueryValue(key, HttpContext.Current.Request.UrlReferrer.ToString());
                if (!VerifyHelper.IsEmpty(sourceRefUrl)) {
                    refUrl = sourceRefUrl;
                }
                else
                {
                    refUrl = VerifyHelper.IsAjax() ? "" : HttpContext.Current.Request.UrlReferrer.ToString();
                }
            }

            //来源页为空或为当前自己
            if (string.IsNullOrWhiteSpace(refUrl) || GetCurrentReqeustUrl().ToLower().Contains(refUrl))
            {
                refUrl = defaultUrl;
            }
            return refUrl;

            ////如果当前请求为Ajax且来源(ajaxy请求所在页面)的地址含key
            //if (VerifyHelper.IsAjax() && string.IsNullOrWhiteSpace(url) && HttpContext.Current.Request.UrlReferrer != null)
            //{
            //    var item = UrlsHelper.GetQueryList(HttpContext.Current.Request.UrlReferrer.ToString()).FirstOrDefault(x => x.Key.ToLower() == key.ToLower());
            //    if (item != null && !string.IsNullOrWhiteSpace(item.Value))
            //    {
            //        url = item.Value;
            //    }
            //}

            ////如果当前及ajax的请求来源地址中都不包含key
        }

        /// <summary>
        /// 编码Url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 转码Html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

    }
}
