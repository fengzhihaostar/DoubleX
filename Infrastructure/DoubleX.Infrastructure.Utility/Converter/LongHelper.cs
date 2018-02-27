using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    public class LongHelper
    {
        /// <summary>
        /// 获取对象长整型整数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回long对象</returns>
        public static long Get(object value, long defaultValue = 0)
        {
            if (value == null)
                return defaultValue;
            long returnValue = defaultValue;
            long.TryParse(value.ToString(), out returnValue);
            return returnValue;
        }

        /// <summary>
        /// 获取字符串长整型整数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回long对象</returns>
        public static long Get(string str, long defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(str))
                return defaultValue;

            str = str.Trim();

            long returnValue = defaultValue;
            long.TryParse(str, out returnValue);
            return returnValue;
        }
    }
}
