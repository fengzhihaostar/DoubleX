using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Repository;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 充值数据持久接口
    /// </summary>
    public interface IRechargeRecordRepository : IRepository<RechargeRecordEntity, Guid>
    {
        /// <summary>
        /// 同步账号余额
        /// </summary>
        /// <param name="memberId">账号Id</param>
        /// <param name="rechargeValue">充值金额</param>
        int SyncAccountBalance(Guid memberId,decimal rechargeValue);
    }
}
