using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Repository;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 费用数据持久操作
    /// </summary>
    public class CostRecordRepository : EFRepository<CostRecordEntity, Guid>, IRepository<CostRecordEntity, Guid>
    {
        public CostRecordRepository()
            : base(new TradeContext())
        {
        }
    }
}
