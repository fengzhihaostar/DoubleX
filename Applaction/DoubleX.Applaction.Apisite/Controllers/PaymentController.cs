using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Payment;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Member;
using DoubleX.Module.Trade;

namespace DoubleX.Applaction.Apisite.Controllers
{
    public class PaymentController : MvcBaseController
    {
        protected IMemberService memberService;
        protected IRechargeRecordService rechargeRecordService;
        public PaymentController(IMemberService iMemberService, IRechargeRecordService iRechargeRecordService)
        {
            memberService = iMemberService;
            rechargeRecordService = iRechargeRecordService;
        }

        /// <summary>
        /// 页面-支付跳转
        /// </summary>
        /// <returns></returns>
        public ActionResult ToPlatform(RequestModel request)
        {
            

            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                Guid id = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "Id"));
                var rechargeRecordModel = rechargeRecordService.Get(x => x.Id == id);
                if (VerifyHelper.IsEmpty(id) || VerifyHelper.IsEmpty(rechargeRecordModel))
                {
                    throw new MessageException(EnumMessageCode.信息错误);
                }
                result.Obj = rechargeRecordModel;
                ViewBag.PayHtml = rechargeRecordService.GetPaymentPlatformHtml(rechargeRecordModel);

                //支付申请
                string logMsg = string.Format("{0}----\r\n---{1}", JsonHelper.Serialize(rechargeRecordModel), ViewBag.PayHtml);
                Log4netHelper.Get(KeyModel.Log.PaySubmit).AsyncWriter(logMsg);
            }
            return View(result);
        }

        /// <summary>
        /// 页面-支付宝支付跳转
        /// </summary>
        public ActionResult AlipayReturn(RequestModel request)
        {
            //异步记录日志
            Log4netHelper.Get(KeyModel.Log.PayNotification).AsyncWriter(request.JsonString);

            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                //支付失败
                string returnSuccess = JsonHelper.GetValue(request.Obj, "is_success");
                string tradStatus = JsonHelper.GetValue(request.Obj, "trade_status");
                if (!(returnSuccess.ToLower() == "t" && tradStatus.ToLower() == "trade_success"))
                {
                    throw new MessageException(EnumMessageCode.支付失败);
                }
                string tradeNo = JsonHelper.GetValue(request.Obj, "out_trade_no");
                decimal moneyValue = DecimalHelper.Get(JsonHelper.GetValue(request.Obj, "total_fee"));
                string rechrageRecordId = JsonHelper.GetValue(request.Obj, "trade_no");
                if (VerifyHelper.IsEmpty(tradeNo) || VerifyHelper.IsEmpty(moneyValue))
                {
                    throw new MessageException(EnumMessageCode.信息错误);
                }
                try
                {
                    rechargeRecordService.RechargeSuccess(tradeNo, moneyValue, rechrageRecordId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return new RedirectResult(WebHelper.GetMemberUrl("/trade/rechargerecord"));
        }

        /// <summary>
        /// 页面-支付宝支付通知
        /// </summary>
        public ActionResult AlipayNotify(RequestModel request)
        {
            //支付宝通知日志
            Log4netHelper.Get(KeyModel.Log.PayNotification).AsyncWriter(request.JsonString);

            string tradStatus = JsonHelper.GetValue(request.Obj, "trade_status");
            if (!(tradStatus.ToLower() == "trade_success"))
            {
                return Content("fail");
            }

            string tradeNo = JsonHelper.GetValue(request.Obj, "out_trade_no");
            decimal moneyValue = DecimalHelper.Get(JsonHelper.GetValue(request.Obj, "total_fee"));
            string rechrageRecordId = JsonHelper.GetValue(request.Obj, "trade_no");

            if (VerifyHelper.IsEmpty(tradeNo) || VerifyHelper.IsEmpty(moneyValue))
            {
                throw new MessageException(EnumMessageCode.信息错误);
            }
            try
            {
                rechargeRecordService.RechargeSuccess(tradeNo, moneyValue, rechrageRecordId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Content("success");
        }

        /// <summary>
        ///  页面-微信支付通知
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult WxNotify(RequestModel request)
        {
            //微信支付通知日志
            Log4netHelper.Get(KeyModel.Log.PayNotification).AsyncWriter(request.JsonString);

            //支付成功操作委托
            Func<string, decimal, string, bool> function = (string tradeNo, decimal money, string rechargeRecordId) =>
            {
                return rechargeRecordService.RechargeSuccess(tradeNo, money, rechargeRecordId);
            };

            //微信支付通知处理
            WxPayNativeNotify not = new WxPayNativeNotify(System.Web.HttpContext.Current, function);
            not.ProcessNotify();

            return Content("success");
        }
    }
}