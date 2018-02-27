using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Infrastructure.Core.Exceptions
{
    /// <summary>
    /// 默认异常处理
    /// </summary>
    public class DefaultException : Exception
    {
        #region 类属性(EnumResultCode,IsLog)

        /// <summary>
        /// 是否写日志(默认不写)
        /// </summary>
        public bool WriteLog = true;

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// 异常Code
        /// </summary>
        public EnumResultCode Code
        {
            get { return _code; }
            set { _code = value; }
        }
        protected EnumResultCode _code = EnumResultCode.未知异常;


        #endregion

        #region 构造方法（属性设置）

        public DefaultException() { }

        public DefaultException(Exception exception, EnumResultCode? code = null)
        {
            if (!VerifyHelper.IsEmpty(exception))
            {
                Message = (exception.InnerException != null && exception.InnerException.Message != null) ?
                    exception.InnerException.Message : exception.Message;

                Message = !VerifyHelper.IsEmpty(Message) ? Message : exception.ToString();
            }

            if (code != null && code.HasValue)
            {
                Code = code.Value;
            }
        }

        public DefaultException(string message, EnumResultCode? code = null)
        {
            Message = message;
            if (code != null && code.HasValue)
            {
                Code = code.Value;
            }
        }

        public DefaultException(EnumResultCode code, Exception exception)
        {
            Code = code;
            Message = EnumHelper.GetName(code);
            if (!VerifyHelper.IsEmpty(exception))
            {
                Message = exception.ToString();
            }
        }

        /// <summary>
        /// 参数错误
        /// </summary>
        /// <param name="args">args 参数列表 </param>
        public DefaultException(EnumResultCode code, params string[] args)
        {
            Code = code;
            Message = EnumHelper.GetName(code);

            if (!VerifyHelper.IsEmpty(args))
            {
                StringBuilder msgBuild = new StringBuilder();
                for (int i = 0; i < args.Length; i++)
                {
                    msgBuild.AppendFormat("{0},", args[i]);
                }

                if (code != EnumResultCode.参数错误)
                {
                    Message = StringHelper.ToBuildString(msgBuild);
                }
                else
                {
                    Message = string.Format("{0}：({1})", EnumHelper.GetName(Code), StringHelper.ToBuildString(msgBuild));
                }
            }
        }

        #endregion
    }
}
