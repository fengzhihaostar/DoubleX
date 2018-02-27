using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 逻辑工具类
    /// </summary>
    public class BoolHelper
    {
        /// <summary>
        /// 获取对象Bool值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回bool对象</returns>
        public static bool Get(object obj, bool defaultValue = false)
        {
            if (obj == null)
                return defaultValue;
            if (obj.ToString() == "1")
                return true;
            if (obj.ToString().ToLower() == "true")
                return true;
            if (obj.ToString() == "0")
                return false;
            return false;
        }

        /// <summary>
        /// 获取字符串Bool值
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="defaultValue">"false"</param>
        /// <returns></returns>
        public static bool Get(string value, string defaultValue = "false")
        {
            if (string.IsNullOrWhiteSpace(value))
                value = defaultValue;
            return value.ToLower().Trim() == "false" ? false : true;
        }

        /// <summary>
        /// 获取整数Bool值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="defaultValue">默认值 0</param>
        /// <returns></returns>
        public static bool Get(int value, int defaultValue = 0)
        {
            if (value == 0)
                return false;
            return true;
        }
    }
}
