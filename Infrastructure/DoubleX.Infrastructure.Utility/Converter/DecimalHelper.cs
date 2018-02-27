using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 浮点数工具类
    /// </summary>
    public class DecimalHelper
    {
        /// <summary>
        /// 获取对象浮点数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回decimal对象</returns>
        public static decimal Get(object obj, decimal defaultValue = 0)
        {
            if (obj == null)
                return defaultValue;
            decimal returnValue = defaultValue;
            decimal.TryParse(obj.ToString(), out returnValue);
            return returnValue;
        }

        /// <summary>
        /// 获取字符串浮点数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回decimal对象</returns>
        public static decimal Get(string str, decimal defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(str))
                return defaultValue;

            str = str.Trim();

            decimal returnValue = defaultValue;
            decimal.TryParse(str, out returnValue);
            return returnValue;
        }

    }
}
