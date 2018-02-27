using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Service;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 充值业务接口
    /// </summary>
    public interface IRechargeRecordService : IService<RechargeRecordEntity, Guid>
    {
        /// <summary>
        /// 跳转至支付平台(表单字符串)
        /// </summary>
        string GetPaymentPlatformHtml(RechargeRecordEntity model);

        /// <summary>
        /// 充值记录支付成功操作
        /// </summary>
        /// <param name="rechargeRecordId"></param>
        /// <returns></returns>
        bool RechargeSuccess(string tradeNo, decimal moneyValue, string rechargeRecordId=null);
    }
}
