using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// MD5工具类
    /// </summary>
    public class MD5Helper
    {
        /// <summary>  
        /// md5加密
        /// </summary>
        public static string Get(string str, bool isBase64 = false, Encoding encoding = null)
        {
            if (str == null)
                str = "";

            if (encoding == null)
                encoding = System.Text.Encoding.UTF8;

            byte[] strByte = encoding.GetBytes(str);
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            byte[] md5Byte = m5.ComputeHash(strByte);
            return isBase64 ? Convert.ToBase64String(md5Byte) : BitConverter.ToString(md5Byte).Replace("-", "");
        }
    }
}
