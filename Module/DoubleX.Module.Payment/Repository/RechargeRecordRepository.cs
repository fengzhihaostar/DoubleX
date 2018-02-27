using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Module.Common;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 充值数据持久操作
    /// </summary>
    public class RechargeRecordRepository : EFRepository<RechargeRecordEntity, Guid>, IRechargeRecordRepository
    {
        public RechargeRecordRepository()
            : base(new TradeContext())
        {
        }

        /// <summary>
        /// 同步账号余额
        /// </summary>
        /// <param name="memberId">账号Id</param>
        /// <param name="rechargeValue">充值金额</param>
        public int SyncAccountBalance(Guid memberId, decimal rechargeValue)
        {
            if (VerifyHelper.IsEmpty(memberId) || VerifyHelper.IsEmpty(rechargeValue))
            {
                throw new DefaultException(EnumResultCode.参数错误, "memberId", "rechargeValue");
            }

            // new SqlParameter("@userId", userId ?? (object)DBNull.Value), 可空类型写法

            string sqlString = string.Format(@"update {0} set Balance=Balance+@moneyValue where Id=@id",
                TableNameKey.Member);

            return dbContext.Database.ExecuteSqlCommand(sqlString,
                DBHelper.GetParameter("@id", memberId),
                DBHelper.GetParameter("@moneyValue", rechargeValue)
            );
        }
    }
}
