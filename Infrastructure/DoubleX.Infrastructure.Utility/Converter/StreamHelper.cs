using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 字节流工具类
    /// </summary>
    public class StreamHelper
    {
        /// <summary>
        /// 获取字符串长整型整数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>Stream流</returns>
        public static Stream Get(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            if (strBytes == null)
                return null;
            return new MemoryStream(strBytes);
        }

        /// <summary>
        /// 将byte[]转为流
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>Stream流</returns>
        public static Stream Get(byte[] bytes)
        {
            if (bytes == null)
                return null;
            return new MemoryStream(bytes);
        }
        
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>Stream流</returns>
        public static Stream GetByFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                Stream stream = new MemoryStream(bytes);
                return stream;
            }
        }
    }
}
