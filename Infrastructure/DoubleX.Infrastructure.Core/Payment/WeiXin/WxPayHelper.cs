using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;


namespace DoubleX.Infrastructure.Core.Payment
{
    /// <summary>
    /// 微信支付辅助类
    /// </summary>
    public class WxPayHelper
    {
        #region 扫码支付

        /**
        * 生成扫描支付模式一URL
        * @param productId 商品ID
        * @return 模式一URL
        */
        public static string GetPrePayUrl(string productId)
        {
            WxPayData data = new WxPayData();
            data.SetValue("appid", WxPayConfig.APPID);//公众帐号id
            data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            data.SetValue("time_stamp", GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", RandHelper.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;
            return url;
        }

        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public static string GetPayUrl(string rechargeRecordId, string tags, string descript, decimal moneyValue, out string tradeNo)
        {
            tradeNo = RandHelper.GenerateNonceStr();
            WxPayData data = new WxPayData();
            data.SetValue("body", descript);        //商品描述
            data.SetValue("attach", "");            //附加数据
            data.SetValue("out_trade_no", tradeNo); //随机字符串
            data.SetValue("total_fee", StringHelper.Get(IntHelper.Get(moneyValue * 100)));//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", tags);//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", rechargeRecordId);//商品ID
            WxPayData result = UnifiedOrder(data);//调用统一下单接口
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
            return url;
        }

        #endregion

        #region 订单查询

        /**
        *    
        * 查询订单
        * @param WxPayData inputObj 提交给查询订单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回订单查询结果，其他抛异常
        */
        public static WxPayData OrderQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", RandHelper.GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            var requestResult = CoreHelper.GetHttpResult(url, xml, contentType: KeyModel.ContentType.Xml);
            string response = requestResult.Obj; // HttpService.Post(xml, url, false, timeOut);


            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            return result;
        }

        #endregion

        #region 微信统一操作接口

        /**
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData UnifiedOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new WxPayException("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new WxPayException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", WxPayConfig.NOTIFY_URL);//异步通知url
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("spbill_create_ip", WxPayConfig.IP);//终端ip	  	    
            inputObj.SetValue("nonce_str", RandHelper.GenerateNonceStr());//随机字符串

            //签名
            inputObj.SetValue("sign", inputObj.MakeSign());

            string xml = inputObj.ToXml();
            var start = DateTime.Now;
            var requestResult = CoreHelper.GetHttpResult(url, xml, contentType: KeyModel.ContentType.Xml);
            string response = requestResult.Obj; // HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            //ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }

        #endregion

        #region  私有方法-辅助操作

        /**
        * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        */
        private static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", WxPayConfig.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        private static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private static string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }

        #endregion
    }

    #region http连接基础类，负责底层的http通信,注释使用框架辅助类

    /// <summary>
    /// http连接基础类，负责底层的http通信
    /// </summary>
    //public class HttpService {

    //    public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    //    {
    //        //直接确认，否则打不开    
    //        return true;
    //    }

    //    public static string Post(string xml, string url, bool isUseCert, int timeout)
    //    {
    //        System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

    //        string result = "";//返回结果

    //        HttpWebRequest request = null;
    //        HttpWebResponse response = null;
    //        Stream reqStream = null;

    //        try
    //        {
    //            //设置最大连接数
    //            ServicePointManager.DefaultConnectionLimit = 200;
    //            //设置https验证方式
    //            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
    //            {
    //                ServicePointManager.ServerCertificateValidationCallback =
    //                        new RemoteCertificateValidationCallback(CheckValidationResult);
    //            }

    //            /***************************************************************
    //            * 下面设置HttpWebRequest的相关属性
    //            * ************************************************************/
    //            request = (HttpWebRequest)WebRequest.Create(url);

    //            request.Method = "POST";
    //            request.Timeout = timeout * 1000;

    //            //设置代理服务器
    //            //WebProxy proxy = new WebProxy();                          //定义一个网关对象
    //            //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
    //            //request.Proxy = proxy;

    //            //设置POST的数据类型和长度
    //            request.ContentType = "text/xml";
    //            byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
    //            request.ContentLength = data.Length;

    //            //是否使用证书
    //            if (isUseCert)
    //            {
    //                string path = HttpContext.Current.Request.PhysicalApplicationPath;
    //                X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
    //                request.ClientCertificates.Add(cert);
    //                Log.Debug("WxPayApi", "PostXml used cert");
    //            }

    //            //往服务器写入数据
    //            reqStream = request.GetRequestStream();
    //            reqStream.Write(data, 0, data.Length);
    //            reqStream.Close();

    //            //获取服务端返回
    //            response = (HttpWebResponse)request.GetResponse();

    //            //获取服务端返回数据
    //            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
    //            result = sr.ReadToEnd().Trim();
    //            sr.Close();
    //        }
    //        catch (System.Threading.ThreadAbortException e)
    //        {
    //            Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
    //            Log.Error("Exception message: {0}", e.Message);
    //            System.Threading.Thread.ResetAbort();
    //        }
    //        catch (WebException e)
    //        {
    //            Log.Error("HttpService", e.ToString());
    //            if (e.Status == WebExceptionStatus.ProtocolError)
    //            {
    //                Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
    //                Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
    //            }
    //            throw new WxPayException(e.ToString());
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Error("HttpService", e.ToString());
    //            throw new WxPayException(e.ToString());
    //        }
    //        finally
    //        {
    //            //关闭连接和流
    //            if (response != null)
    //            {
    //                response.Close();
    //            }
    //            if (request != null)
    //            {
    //                request.Abort();
    //            }
    //        }
    //        return result;
    //    }

    //    /// <summary>
    //    /// 处理http GET请求，返回数据
    //    /// </summary>
    //    /// <param name="url">请求的url地址</param>
    //    /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
    //    public static string Get(string url)
    //    {
    //        System.GC.Collect();
    //        string result = "";

    //        HttpWebRequest request = null;
    //        HttpWebResponse response = null;

    //        //请求url以获取数据
    //        try
    //        {
    //            //设置最大连接数
    //            ServicePointManager.DefaultConnectionLimit = 200;
    //            //设置https验证方式
    //            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
    //            {
    //                ServicePointManager.ServerCertificateValidationCallback =
    //                        new RemoteCertificateValidationCallback(CheckValidationResult);
    //            }

    //            /***************************************************************
    //            * 下面设置HttpWebRequest的相关属性
    //            * ************************************************************/
    //            request = (HttpWebRequest)WebRequest.Create(url);

    //            request.Method = "GET";

    //            //设置代理
    //            //WebProxy proxy = new WebProxy();
    //            //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
    //            //request.Proxy = proxy;

    //            //获取服务器返回
    //            response = (HttpWebResponse)request.GetResponse();

    //            //获取HTTP返回数据
    //            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
    //            result = sr.ReadToEnd().Trim();
    //            sr.Close();
    //        }
    //        catch (System.Threading.ThreadAbortException e)
    //        {
    //            System.Threading.Thread.ResetAbort();
    //        }
    //        catch (WebException e) 
    //        {
    //            if (e.Status == WebExceptionStatus.ProtocolError)
    //            {
    //                //Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
    //                //Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
    //            }
    //            throw new WxPayException(e.ToString());
    //        }
    //        catch (Exception e)
    //        {
    //            //Log.Error("HttpService", e.ToString());
    //            throw new WxPayException(e.ToString());
    //        }
    //        finally
    //        {
    //            //关闭连接和流
    //            if (response != null)
    //            {
    //                response.Close();
    //            }
    //            if (request != null)
    //            {
    //                request.Abort();
    //            }
    //        }
    //        return result;
    //    }
    //}


    #endregion

}
