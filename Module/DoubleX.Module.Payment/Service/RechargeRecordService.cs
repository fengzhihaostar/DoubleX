using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Service;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Payment;
using System.Web;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 充值业务
    /// </summary>
    public class RechargeRecordService : DefaultService<RechargeRecordRepository, RechargeRecordEntity, Guid>, IRechargeRecordService
    {
        public RechargeRecordService()
            : base(new RechargeRecordRepository())
        {

        }

        /// <summary>
        /// 获取跳转至支付平台代码
        /// </summary>
        /// <param name="model"></param>
        public string GetPaymentPlatformHtml(RechargeRecordEntity model)
        {
            string rechargeRecordId = StringHelper.Get(model.Id), subject = "UTH充值", body = model.Descript;
            decimal money = DecimalHelper.Get(model.MoneyValue);

            string tradeNo = "", htmlCode = "";
            if (model.PaymentType == EnumHelper.GetValue(EnumPaymentType.支付宝))
            {
                AlipayModel pay = new AlipayModel(rechargeRecordId, subject, body, money);
                htmlCode = pay.FormHtml(out tradeNo);
            }
            if (model.PaymentType == EnumHelper.GetValue(EnumPaymentType.微信))
            {
                htmlCode = WxPayHelper.GetPayUrl(rechargeRecordId, subject, body, money, out tradeNo);
            }

            if (!VerifyHelper.IsEmpty(tradeNo) && !VerifyHelper.IsEmpty(htmlCode)) {
                model.PaymentTypeRefValue=tradeNo;
                Update(model);
                return htmlCode;
            }
            return "";
        }

        /// <summary>
        /// 充值记录支付成功操作
        /// </summary>
        /// <returns></returns>
        public bool RechargeSuccess(string tradeNo, decimal moneyValue, string rechargeRecordId = null)
        {
            if (VerifyHelper.IsEmpty(tradeNo) || VerifyHelper.IsEmpty(moneyValue))
                throw new DefaultException(EnumResultCode.参数错误, "tradeNo", "moneyValue");

            var model = Get(x => x.PaymentTypeRefValue == tradeNo);
            if (model.RechargeState == EnumHelper.GetValue(EnumRechargeState.己支付))
            {
                return true;
            }

            model.RechargeState = EnumHelper.GetValue(EnumRechargeState.己支付);
            //model.PaymentTypeRefValue = tradeNo;
            model.LastDt = DateTime.Now;
            Update(model);

            CostRecordEntity costModel = new CostRecordEntity();
            costModel.AccountId = model.AccountId;
            costModel.CostType = EnumHelper.GetValue(EnumCostType.充值记录);
            costModel.CostTypeRefValue = StringHelper.Get(model.Id);
            costModel.MoneyValue = moneyValue;
            costModel.Descript = model.Descript;
            costModel.CreateId = model.AccountId;
            costModel.CreateDt = DateTime.Now;
            costModel.LastId = model.AccountId;
            costModel.LastDt = DateTime.Now;

            var costRecordService = CoreHelper.GetResolve<ICostRecordService>();
            costRecordService.Insert(costModel);

            //账号充值
            repository.SyncAccountBalance(costModel.AccountId, moneyValue);

            return true;
        }
    }
}
