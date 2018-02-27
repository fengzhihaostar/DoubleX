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
using DoubleX.Framework.Web;
using DoubleX.Framework.Web.Controllers;
using DoubleX.Module.Member;
using DoubleX.Module.Trade;

namespace DoubleX.Applaction.Apisite.Areas.Member.Controllers
{
    /// <summary>
    /// 交易相关控制器
    /// </summary>
    public class TradeController : MvcBaseController
    {
        protected IMemberService memberService;
        protected IRechargeRecordService paymentRecordService;
        public TradeController(IMemberService iMemberService,IRechargeRecordService iPaymentRecordService)
        {
            memberService = iMemberService;
            paymentRecordService = iPaymentRecordService;
        }

        /// <summary>
        /// 页面-充值支付
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge(RequestModel request)
        {
            var memberDetail = memberService.GetDetail(WebHelper.GetMemberId(request));
            if (VerifyHelper.IsEmpty(memberDetail))
                throw new MessageException(EnumMessageCode.信息错误);

            return View(memberDetail);
        }
        
        /// <summary>
        /// 页面-充值记录
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeRecord()
        {
            return View();
        }
        
        /// <summary>
        /// 页面-交易流水
        /// </summary>
        /// <returns></returns>
        public ActionResult CostRecord()
        {
            return View();
        }

    }
}