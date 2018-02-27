using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Member;
using DoubleX.Module.Trade;

namespace DoubleX.Applaction.Apisite.Areas.Api.Controllers
{
    /// <summary>
    /// 交易信息接口
    /// </summary>
    public class TradeController : ApiController
    {
        protected ICostRecordService costRecordService;
        protected IRechargeRecordService rechargeRecordService;
        protected IMemberService memberService;
        public TradeController(ICostRecordService iCostRecordService, IRechargeRecordService iPaymentRecordService, IMemberService iMemberService)
        {
            costRecordService = iCostRecordService;
            rechargeRecordService = iPaymentRecordService;
            memberService = iMemberService;
        }

        #region 充值操作

        /// <summary>
        /// 充值添加
        /// </summary>
        public HttpResponseMessage RechargeRecordAdd(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var model = JsonHelper.Deserialize<RechargeRecordEntity>(request.JsonString);
                if (VerifyHelper.IsEmpty(model))
                {
                    throw new DefaultException(EnumResultCode.请求错误);
                }

                model.AccountId = WebHelper.GetMemberId(request);
                model.RechargeState = EnumHelper.GetValue(EnumRechargeState.待支付);
                model.Descript = "账户充值";
                model.CreateId = GuidHelper.Get(WebHelper.GetContext().Member.MemberId);
                model.CreateDt = DateTime.Now;
                model.LastId = model.CreateId;
                model.LastDt = model.CreateDt;
                rechargeRecordService.Insert(model);
                result.Obj = model;
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        /// <summary>
        /// 充值查询
        /// </summary>
        public HttpResponseMessage RechargeRecordQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                var predicate = WebHelper.GetOwnerPredicate<RechargeRecordEntity>(x => x.AccountId, request);

                var list = rechargeRecordService.Query(requestQuery, predicate, out total);
                result.Obj = new ResultQueryModel(requestQuery, total, list);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        #region 支付宝通知


        ///// <summary>
        ///// 支付添加
        ///// </summary>
        //[HttpGet, HttpPost]
        //public HttpResponseMessage AlipayReturn(RequestModel request)
        //{
        //    var errorMessage = new HttpResponseMessage { Content = new StringContent("fail", System.Text.Encoding.UTF8) };
        //    var successMessage = new HttpResponseMessage { Content = new StringContent("success", System.Text.Encoding.UTF8) };

        //    var result = WebHelper.GetResult(request);
        //    if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
        //    {


        //        string isSuccess = JsonHelper.GetValue(request.Obj, "is_success");
        //        if (isSuccess.ToLower() != "t")
        //        {
        //            return errorMessage;
        //        }

        //        string tradStatus = JsonHelper.GetValue(request.Obj, "trade_status");
        //        //提交成功,交易失败
        //        if (tradStatus.ToLower() != "trade_success")
        //        {

        //        }

        //        Guid id = GuidHelper.Get(JsonHelper.GetValue(request.Obj, "out_trade_no"));
        //        if (VerifyHelper.IsEmpty(id))
        //        {
        //            throw new DefaultException(EnumResultCode.请求错误);
        //        }

        //        //有信息，且己有状态标识成功
        //        var model = rechargeRecordService.Get(x => x.Id == id);
        //        if (model.RechargeState == EnumHelper.GetValue(EnumRechargeState.己支付) || !VerifyHelper.IsEmpty(model.PaymentTypeRefValue))
        //        {
        //            return successMessage;
        //        }

        //        decimal tradeValue = DecimalHelper.Get(JsonHelper.GetValue(request.Obj, "total_fee"));
        //        string tradeNo = StringHelper.Get(JsonHelper.GetValue(request.Obj, "trade_no"));

        //        model.RechargeState = EnumHelper.GetValue(EnumRechargeState.己支付);
        //        model.PaymentTypeRefValue = tradeNo;
        //        model.MoneyValue = tradeValue;
        //        model.LastDt = DateTime.Now;
        //        rechargeRecordService.Update(model);

        //        CostRecordEntity costModel = new CostRecordEntity();
        //        costModel.AccountId = model.AccountId;
        //        costModel.CostType = 1;//EnumHelper.GetValue(1)
        //        costModel.CostTypeRefValue = StringHelper.Get(model.Id);
        //        costModel.MoneyValue = tradeValue;
        //        costModel.Descript = model.Descript;
        //        costModel.CreateId = Guid.Empty;
        //        costModel.CreateDt = DateTime.Now;
        //        costModel.LastId = costModel.LastId;
        //        costModel.LastDt = DateTime.Now;
        //        costRecordService.Insert(costModel);


        //        //total_fee，

        //        //body=UTH%E4%BA%A7%E5%93%81%E4%BB%98%E8%B4%B9
        //        //&buyer_email=carl900%40yeah.net
        //        //&buyer_id=2088702985319424
        //        //&exterface=create_direct_pay_by_user
        //        //&extra_common_param=UTHInternal
        //        //&is_success=T
        //        //&notify_id=RqPnCoPT3K9%252Fvwbh3InZdaihXn7kG%252Fc16VvrKGePubDikkPJ5J6QoHz3ohr8kbGYe0hh
        //        //&notify_time=2017-02-10+16%3A50%3A41
        //        //&notify_type=trade_status_sync
        //        //&out_trade_no=626e5f06-e0af-4c6d-8e23-f2b91125e39c
        //        //&payment_type=1
        //        //&seller_email=caiwuzhuanyong%40utranshub.com
        //        //&seller_id=2088221838965785
        //        //&subject=%E8%B4%A6%E6%88%B7%E5%85%85%E5%80%BC
        //        //&total_fee=0.02
        //        //&trade_no=2017021021001004420267489781
        //        //&trade_status=TRADE_SUCCESS
        //        //&sign=7b00e4d951c9f8b0fbd935af94abd857
        //        //&sign_type=MD5

        //        //var model = JsonHelper.Deserialize<PaymentRecordEntity>(request.JsonString);
        //        //if (VerifyHelper.IsEmpty(model))
        //        //{
        //        //    throw new DefaultException(EnumResultCode.请求错误);
        //        //}
        //    }
        //    return WebApiHelper.ToHttpResponseMessage(result);
        //}


        #endregion

        #endregion

        #region 费用操作

        /// <summary>
        /// 流量查询
        /// </summary>
        public HttpResponseMessage CostRecordQuery(RequestModel request)
        {
            var result = WebHelper.GetResult(request);
            if (result.Code == EnumHelper.GetValue(EnumResultCode.操作成功))
            {
                var total = 0L;
                var requestQuery = JsonHelper.Deserialize<RequestQueryModel>(request.JsonString, isNewObj: true);
                var predicate = WebHelper.GetOwnerPredicate<CostRecordEntity>(x => x.AccountId, request);

                var list = costRecordService.Query(requestQuery, predicate, out total);
                result.Obj = new ResultQueryModel(requestQuery, total, list);
            }
            return WebApiHelper.ToHttpResponseMessage(result);
        }

        #endregion
    }
}