using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 随机数辅助类
    /// </summary>
    public class RandHelper
    {
        /// <summary>
        /// 随机数字
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRandNumber(int len)
        {
            string str = string.Empty;
            var _random = new Random();
            for (int i = 0; i < len; i++)
            {
                str += _random.Next(0, 9);
            }
            return str;
        }

        /**
       * 生成随机串，随机串包含字母或数字
       * @return 随机串
       */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
