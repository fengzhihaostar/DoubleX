using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Html辅助类
    /// </summary>
    public class HtmlsHelper
    {

        ///<summary>
        ///移除Html标签
        ///</summary>
        ///<param   name="NoHTML">包括HTML的源码   </param>
        ///<returns>已经去除后的文字</returns>
        public static string Remove(string htmlStr)
        {
            if (string.IsNullOrWhiteSpace(htmlStr))
                return htmlStr;

            //删除脚本
            htmlStr = Regex.Replace(htmlStr, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);

            var groupList=Regex.Match("", @"<script[^>]*?>.*?</script>");

            //删除HTML
            htmlStr = Regex.Replace(htmlStr, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"-->", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            htmlStr = Regex.Replace(htmlStr, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);

            htmlStr = htmlStr.Replace("<", "");
            htmlStr = htmlStr.Replace(">", "");
            htmlStr = htmlStr.Replace("\r", "");
            htmlStr = htmlStr.Replace("\n", "");
            htmlStr = htmlStr.Replace("\t", "");
            htmlStr = htmlStr.Replace("\\", "");

            htmlStr = HttpUtility.HtmlEncode(htmlStr).Trim();
            return htmlStr;
        }

        /// <summary>
        /// 编码Html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 转码Html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
    }
}
