using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Service;

namespace DoubleX.Module.Trade
{
    /// <summary>
    /// 费用业务接口
    /// </summary>
    public interface ICostRecordService : IService<CostRecordEntity, Guid>
    {
    }
}
