using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Module.Common;

namespace DoubleX.Module.Traffic
{
    /// <summary>
    /// 数据持久接口
    /// </summary>
    public interface ITrafficRepository : IRepository<TrafficEntity, Guid>
    {
    }
}
