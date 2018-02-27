using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 整数工具类
    /// </summary>
    public class IntHelper
    {
        /// <summary>
        /// 获取对象整数
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回int对象</returns>
        public static int Get(object obj, int defaultValue = 0)
        {
            if (obj == null)
                return defaultValue;
            int returnValue = defaultValue;

            //传入decimail 如1 ToString 后变成了1.00 int.TryParse报错
            try
            {
                returnValue = Convert.ToInt32(obj);
            }
            catch
            {
                int.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// 获取字符串整数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回int对象</returns>
        public static int Get(string str, int defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(str))
                return defaultValue;

            str = str.ToLower().Trim();
            if (str == "true" || str == "false")
            {
                return str == "true" ? 1 : 0;
            }

            int returnValue = defaultValue;
            int.TryParse(str, out returnValue);
            return returnValue;
        }

        /// <summary>
        /// 获取bool整数
        /// </summary>
        /// <param name="obj">bool 对象</param>
        /// <returns>返回int对象</returns>
        public static int Get(bool obj)
        {
            return obj ? 1 : 0;
        }
    }
}
