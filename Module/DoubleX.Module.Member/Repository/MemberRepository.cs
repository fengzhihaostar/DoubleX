using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Module.Common;

namespace DoubleX.Module.Member
{
    /// <summary>
    /// 会员数据持久操作
    /// </summary>
    public class MemberRepository : EFRepository<MemberEntity, Guid>, IMemberRepository
    {
        public MemberRepository()
            : base(new MemberContext())
        {
        }

        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="memberId">会员Id</param>
        public MemberDetailModel GetDetail(Guid memberId)
        {
            if (VerifyHelper.IsEmpty(memberId))
            {
                throw new DefaultException(EnumResultCode.参数错误, "memberId");
            }

            // new SqlParameter("@userId", userId ?? (object)DBNull.Value), 可空类型写法

            MemberDetailModel model = null;

            string sqlString = string.Format(@"
select t1.*,
IFNULL(t2.MoneyValue,0) as RechargeTotal,
IFNULL(t3.Size,0) as TrafficTotal,
IFNULL(t4.MoneyValue,0) as  ConsumeTotal,
IFNULL(t5.MoneyValue,0) as  MonthRechargeTotal,
IFNULL(t6.MoneyValue,0) as  MonthConsumeTotal,
IFNULL(t7.Size,0) as  MonthTrafficTotal
from {0} t1 
left join (select SUM(IFNULL(MoneyValue,0))  AS MoneyValue,AccountId from {1} WHERE AccountId=@id and  RechargeState={5}) t2 on t1.Id=t2.AccountId
left join (select SUM(IFNULL(Size,0))  AS Size,AccountId from {2} where AccountId=@id ) t3 on t1.Id=t3.AccountId
left join (select SUM(IFNULL(MoneyValue,0))  AS MoneyValue,AccountId from {3} WHERE AccountId=@id and  CostType={4}) t4 on t1.Id=t4.AccountId
left join (select SUM(IFNULL(MoneyValue,0))  AS MoneyValue,AccountId from {1} WHERE AccountId=@id and  RechargeState={5} and date_format(LastDt,'%Y-%m')=date_format(now(),'%Y-%m')) t5 on t1.Id=t5.AccountId
left join (select SUM(IFNULL(MoneyValue,0))  AS MoneyValue,AccountId from {3} WHERE AccountId=@id and  CostType={4} and date_format(LastDt,'%Y-%m')=date_format(now(),'%Y-%m')) t6 on t1.Id=t6.AccountId
left join (select SUM(IFNULL(Size,0))  AS Size,AccountId from {2} where AccountId=@id and date_format(LastDt,'%Y-%m')=date_format(now(),'%Y-%m')) t7 on t1.Id=t7.AccountId
where  t1.Id=@id",
                TableNameKey.Member,
                TableNameKey.TradeRechargeRecord,
                TableNameKey.Traffic,
                TableNameKey.TradeCostRecord,
                EnumHelper.GetValue(EnumCostType.消费记录),
                EnumHelper.GetValue(EnumRechargeState.己支付));

            model = dbContext.Database.SqlQuery<MemberDetailModel>(sqlString,
                DBHelper.GetParameter("@id", memberId)
            ).FirstOrDefault();
            return model;
        }
    }
}
