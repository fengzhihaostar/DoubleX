using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using DoubleX.Infrastructure.Utility;


namespace DoubleX.Infrastructure.Core.Payment
{
    /// <summary>
    /// 微信支付配置信息
    /// </summary>
    public class WxPayConfig
    {

        //uth
        //<!--↓↓↓↓↓微信支付配置↓↓↓↓↓-->
        //<!--公众账号ID-->
        //<add key="WxAPPID" value="wx14c959a511684f10"/>
        //<!--商户号-->
        //<add key="WxMCHID" value="1415597902"/>
        //<!--商户密钥-->
        //<add key="WxKEY" value="uth12345uth12345uth12345uth12345"/>
        //<!--证书路径-->
        //<add key="WxSSLCERT_PATH" value="cert/apiclient_cert.p12"/>
        //<add key="WxSSLCERT_PASSWORD" value="1415597902"/>
        //<!--通知Url-->
        //<add key="WxNOTIFY_URL" value="http://ss.sesamespell.com:62888/PayNotify/ResultNotifyPage"/>
        //<!--系统后台IP-->
        //<add key="WxIP" value="0.0.0.0"/>
        //<!--代理服务器设置-->
        //<add key="WxPROXY_URL" value="http://0.0.0.0:0"/>
        //<add key="WxBody" value="UTH国际官方收款账户"/>
        //<!--↑↑↑↑↑UTH微信支付配置↑↑↑↑↑-->


        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string APPID = "wx14c959a511684f10";
        public const string MCHID = "1415597902";
        public const string KEY = "uth12345uth12345uth12345uth12345";

        public const string APPSECRET = "51c56b886b5be869567dd389b3e5d1d6";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12"; //可放在网站目录/xxx/目录下
        public const string SSLCERT_PASSWORD = "1415597902";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */

        public static string NOTIFY_URL { get { return notifyUrl; } set { notifyUrl = value; } }
        protected static string notifyUrl = string.Format("{0}/payment/wxnotify", UrlsHelper.GetUrl(isAndPath: false));

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;
    }
}
