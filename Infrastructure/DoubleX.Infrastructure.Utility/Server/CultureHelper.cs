using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 区域性信息帮助类
    /// </summary>
    public class CultureHelper
    {

        /// <summary>
        /// 获取区域性信息
        /// </summary>
        /// <param name="culture">区域标识</param>
        /// <param name="defaultCulture">默认标识</param>
        /// <returns>返回 CultureInfo</returns>
        public static CultureInfo GetCulture(string culture = null, string defaultCulture = "zh-CN")
        {
            if (string.IsNullOrEmpty(culture) && Thread.CurrentThread.CurrentUICulture != null)
            {
                culture = Thread.CurrentThread.CurrentUICulture.Name;
            }
            if (string.IsNullOrEmpty(culture) && Thread.CurrentThread.CurrentCulture != null)
            {
                culture = Thread.CurrentThread.CurrentCulture.Name;
            }
            if (string.IsNullOrEmpty(culture) && !string.IsNullOrEmpty(defaultCulture))
            {
                culture = defaultCulture;
            }
            if (string.IsNullOrEmpty(culture))
            {
                return new CultureInfo("zh-CN");
            }
            return new CultureInfo(culture);
        }
    }
}
