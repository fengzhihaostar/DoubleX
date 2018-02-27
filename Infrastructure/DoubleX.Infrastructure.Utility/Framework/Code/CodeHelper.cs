using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ThoughtWorks;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 一维码/二维码/条码辅助类
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// 获取二维码图片字节数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetQRCodeByte(string data, Encoding coding = null)
        {
            MemoryStream ms = GetQRCodeStream(data, coding);
            if (ms == null)
                return null;
            return ms.GetBuffer();
        }

        /// <summary>
        /// 获取二维码图片流数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream GetQRCodeStream(string data, Encoding coding = null)
        {
            if (string.IsNullOrWhiteSpace(data))
                return null;

            if (coding == null)
                coding = Encoding.Default;

            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(data, coding);

            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            return ms;
        }
    }
}
