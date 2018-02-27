using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Service;

namespace DoubleX.Module.Traffic
{
    /// <summary>
    /// 流量业务接口
    /// </summary>
    public interface ITrafficService : IService<TrafficEntity, Guid>
    {
        List<Traffic_ProjectName> ReadTraffic(RequestQueryModel query, string accountId, out long total);
    }
}
