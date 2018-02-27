using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Utility.Framework;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;


namespace DoubleX.Infrastructure.Core.Payment
{
    /// <summary>
    /// 微信通知
    /// </summary>
    public class WxPayNotify
    {
        public HttpContext context { get; set; }

        public WxPayNotify(HttpContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream requestInputStream = context.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = requestInputStream.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            requestInputStream.Flush();
            requestInputStream.Close();
            requestInputStream.Dispose();

            //支付日志
            Log4netHelper.Get(KeyModel.Log.PayNotification).AsyncWriter(builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
            }
            return data;
        }

        //派生类需要重写这个方法，进行不同的回调处理
        public virtual void ProcessNotify()
        {

        }
    }

    /// <summary>
    /// 微信扫码支付通知
    /// </summary>
    public class WxPayNativeNotify : WxPayNotify
    {
        /// <summary>
        /// 通知支付成功执行委托操作
        /// </summary>
        Func<string, decimal, string, bool> fun { get; set; }

        public WxPayNativeNotify(HttpContext context)
            : base(context)
        {
        }

        public WxPayNativeNotify(HttpContext context, Func<string, decimal, string, bool> fun)
            : base(context)
        {
            this.fun = fun;
        }

        //通知信息
        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            Log4netHelper.Get(KeyModel.Log.PayNotification).AsyncWriter(notifyData.ToJson());

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                context.Response.Write(res.ToXml());
                context.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            string tradeNo = StringHelper.Get(notifyData.GetValue("out_trade_no"));
            decimal moneyValue = DecimalHelper.Get(notifyData.GetValue("total_fee")) / 100;
            string rechrageRecordId = "";


            bool isFunction = false;
            if (QueryOrder(transaction_id))
            {
                //执执判断是否执行成功
                isFunction = fun(tradeNo, moneyValue, rechrageRecordId);
            }

            if (!isFunction)
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                context.Response.Write(res.ToXml());
                context.Response.End();
            }
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                context.Response.Write(res.ToXml());
                context.Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayHelper.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
