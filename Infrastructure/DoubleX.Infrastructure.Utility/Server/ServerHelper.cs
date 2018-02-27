using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 服务器工具类
    /// </summary>
    public class ServerHelper
    {
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        public static ServerInfo Get()
        {
            return new ServerInfo();
        }
        
    }

    /// <summary>
    /// 服务器信息
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 服务器域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// .NET解释引擎版本
        /// </summary>
        public string DoNetCLR { get; set; }

        /// <summary>
        /// 服务器操作系统版本
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// 服务器IIS版本
        /// </summary>
        public string IISVersion { get; set; }

        /// <summary>
        /// CPU类型
        /// </summary>
        public string CPUType { get; set; }

        /// <summary>
        /// CPU个数
        /// </summary>
        public int CPUCount { get; set; }

        /// <summary>
        /// HTTP访问端口
        /// </summary>
        public string HTTPPORT { get; set; }

        /// <summary>
        /// 域名主机
        /// </summary>
        public string HOST { get; set; }

        /// <summary>
        /// 服务器区域语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 虚拟目录的绝对路径  
        /// </summary>
        public string RhysicalPath { get; set; }

        /// <summary>
        /// 执行文件的绝对路径
        /// </summary>
        public string TranslatedPath { get; set; }

        public ServerInfo()
        {
            MachineName = HttpContext.Current.Server.MachineName;
            IP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            Domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            DoNetCLR = ".NET CLR" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
            OSVersion = ""+Environment.OSVersion;
            IISVersion = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            CPUType = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").ToString();
            CPUCount = int.Parse(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
            HTTPPORT = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            HOST = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            Language = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            RhysicalPath = HttpContext.Current.Request.ServerVariables["APPL_RHYSICAL_PATH"];
            TranslatedPath = HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"];
        }
    }
}
