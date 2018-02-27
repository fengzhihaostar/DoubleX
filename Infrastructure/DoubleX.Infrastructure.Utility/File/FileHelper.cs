using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 将内容写入文件并保存
        /// </summary>
        public static void WriterFile(string filePath, string context)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new Exception("FileHelper -> SaveFile  filePath is null");
            }

            if (string.IsNullOrEmpty(context))
            {
                context = "";
            }
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            string dicPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs); // 创建写入流
            sw.WriteLine(context); // 写入
            sw.Close(); //关闭文件
        }
    }
}
