using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Base64帮助类
    /// </summary>
    public class Base64Helper
    {
        /// <summary>
        /// 获取Base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="codeing"></param>
        /// <returns></returns>
        public static string Get(byte[] bytes, Encoding codeing = null)
        {
            if (bytes == null)
                return "";
            if (bytes.Length == 0)
                return "";

            if (codeing == null)
                codeing = Encoding.Default;

            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 编码字符串
        /// </summary>
        public static string Encode(string str, Encoding codeing = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";

            if (codeing == null)
                codeing = Encoding.Default;

            byte[] strs = codeing.GetBytes(str);
            try
            {
                return Convert.ToBase64String(strs, 0, strs.Length);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 解码字符串
        /// </summary>
        public static string Decode(string str, Encoding codeing = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";

            if (codeing == null)
                codeing = Encoding.Default;

            char[] strs = str.ToCharArray();
            try
            {
                byte[] bOutput = System.Convert.FromBase64String(str);
                return codeing.GetString(bOutput);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
