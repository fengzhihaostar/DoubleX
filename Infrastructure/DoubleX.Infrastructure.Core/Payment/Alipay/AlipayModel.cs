using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;
using DoubleX.Infrastructure.Utility;

namespace DoubleX.Infrastructure.Core.Payment
{
    /// <summary>
    /// 支付宝辅助类
    /// </summary>
    public class AlipayModel
    {
        public AlipayModel() { }

        public AlipayModel(string rechargeRecordId, string subject, string body, decimal money)
        {

            this.RechargeRecordId = rechargeRecordId;
            this.Subject = subject;
            this.Body = body;
            if (VerifyHelper.IsEmpty(Body))
            {
                Body = Subject;
            }
            this.Money = money;
        }

        #region 接口属性

        /// <summary>
        /// 支付宝网关地址（新）
        /// </summary>
        public string Gateway { get { return gateway; } set { gateway = value; } }
        private string gateway = "https://mapi.alipay.com/gateway.do?";

        /// <summary>
        /// 参数字符编辑
        /// </summary>
        public string InputCharset { get { return inputCharset; } set { inputCharset = value; } }
        private string inputCharset = "UTF-8";

        /// <summary>
        /// 调用的接口名，无需修改
        /// </summary>
        public string ServiceName { get { return serviceName; } set { serviceName = value; } }
        private string serviceName = "create_direct_pay_by_user";

        /// <summary>
        /// // MD5密钥，安全检验码，由数字和字母组成的32位字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        /// </summary>
        public string Key { get { return key; } set { key = value; } } //"171gottbsmuw561q0gul2nj33vnoij42"
        private string key = "171gottbsmuw561q0gul2nj33vnoij42";

        /// <summary>
        /// 合作身份者ID/签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        /// </summary>
        public string PartnerNo { get { return partnerNo; } set { partnerNo = value; } } //2088221838965785
        private string partnerNo = "2088221838965785";

        /// <summary>
        ///  收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        /// </summary>
        public string SellerId { get { return sellerId; } set { sellerId = value; } }//2088221838965785
        private string sellerId = "2088221838965785";

        public string ExtraCommonParam { get { return extraCommonParam; } set { extraCommonParam = value; } } //"UTHInternal"
        private string extraCommonParam = "UTHInternal";

        /// <summary>
        /// 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        /// </summary>
        public string NotifyUrl { get { return notifyUrl; } set { notifyUrl = value; } }
        private string notifyUrl = string.Format("{0}/payment/alipaynotify", UrlsHelper.GetUrl(isAndPath: false));

        /// <summary>
        /// 页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        /// </summary>
        public string ReturnUrl { get { return returnUrl; } set { returnUrl = value; } }
        private string returnUrl = string.Format("{0}/payment/alipayreturn", UrlsHelper.GetUrl(isAndPath: false));

        /// <summary>
        /// 商口Id
        /// </summary>
        public string RechargeRecordId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 商品备注
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 商品金额
        /// </summary>
        public decimal Money { get; set; }

        #endregion

        /// <summary>
        /// 支付宝表单提交字符串
        /// </summary>
        public string FormHtml(out string tradeNo)
        {
            tradeNo = RandHelper.GenerateNonceStr();

            StringBuilder build = new StringBuilder();

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", StringHelper.Get(this.ServiceName));                                                 //调用的接口名，无需修改
            sParaTemp.Add("partner", StringHelper.Get(this.PartnerNo));                                                   // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
            sParaTemp.Add("seller_id", StringHelper.Get(this.SellerId));                                                  // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
            sParaTemp.Add("_input_charset", StringHelper.Get(this.InputCharset));                                         // 字符编码格式 目前支持 gbk 或 utf-8
            sParaTemp.Add("payment_type", "1");                                                         // 支付类型 ，无需修改
            sParaTemp.Add("notify_url", StringHelper.Get(this.NotifyUrl));                                                // 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
            sParaTemp.Add("return_url", StringHelper.Get(this.ReturnUrl));                                                // 页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问

            #region 防调鱼配置

            ////↓↓↓↓↓↓↓↓↓↓请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            ////防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
            //public static string anti_phishing_key = "";
            ////客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
            //public static string exter_invoke_ip = "";
            ////↑↑↑↑↑↑↑↑↑↑请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            sParaTemp.Add("anti_phishing_key", "");
            sParaTemp.Add("exter_invoke_ip", "");

            #endregion

            sParaTemp.Add("out_trade_no", StringHelper.Get(tradeNo));
            sParaTemp.Add("subject", StringHelper.Get(this.Subject));                           //商品名称
            sParaTemp.Add("total_fee", StringHelper.Get(this.Money));
            sParaTemp.Add("body", StringHelper.Get(this.Body));
            sParaTemp.Add("extra_common_param", StringHelper.Get(this.ExtraCommonParam));


            //对参数进行过滤排序及生成字符串
            Dictionary<string, string> par = FilterPara(sParaTemp);
            string prestr = CreateLinkString(par);

            par.Add("sign", AlipayMd5Sign(prestr, StringHelper.Get(this.Key), StringHelper.Get(this.InputCharset)));
            par.Add("sign_type", "MD5"); // 签名方式


            //构造请求信息
            build.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + StringHelper.Get(this.Gateway) + "_input_charset=" + StringHelper.Get(this.InputCharset) + "' method='GET'>");
            foreach (KeyValuePair<string, string> temp in par)
            {
                build.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }
            build.Append("<input type='submit' value='确认' style='display:none;'></form>");
            build.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            //submit按钮控件请不要含有name属性
            return build.ToString();
        }

        #region 私有方法

        /// <summary>
        /// 支付宝MD5签名
        /// </summary>
        /// <param name="prestr"></param>
        /// <param name="key"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        protected string AlipayMd5Sign(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);
            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        protected Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        protected string CreateLinkString(Dictionary<string, string> dicArray, bool isUrlEncode = false)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                if (!isUrlEncode)
                    prestr.Append(temp.Key + "=" + temp.Value + "&");
                else
                    prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        #endregion
    }
}
