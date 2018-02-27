using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Service;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Infrastructure.Core.Model;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 费用业务
    /// </summary>
    public class CostRecordService : DefaultService<CostRecordRepository,CostRecordEntity, Guid>, ICostRecordService
    {
        public CostRecordService()
            : base(new CostRecordRepository())
        {

        }
    }
}
