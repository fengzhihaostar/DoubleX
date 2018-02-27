using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Guid工具类
    /// </summary>
    public class GuidHelper
    {

        /// <summary>
        /// 获取对象GUID
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回GUID对象</returns>
        public static Guid Get(object obj, Guid? defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = Guid.Empty;

            if (obj == null)
                return defaultValue.Value;

            Guid returnValue = defaultValue.Value;
            Guid.TryParse(obj.ToString(), out returnValue);
            return returnValue;
        }


        /// <summary>
        /// 获取字符串GUID
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回GUID对象</returns>
        public static Guid Get(string str, Guid? defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = Guid.Empty;

            if (string.IsNullOrWhiteSpace(str))
                return defaultValue.Value;

            str = str.Trim();

            Guid returnValue = defaultValue.Value;
            Guid.TryParse(str, out returnValue);
            return returnValue;
        }


        /// <summary>
        ///生成新的GUID
        /// </summary>
        /// <returns></returns>
        public static Guid NewId()
        {
            return Guid.NewGuid();
        }
    }
}
