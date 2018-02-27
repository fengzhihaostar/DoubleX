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

namespace DoubleX.Module.Traffic
{
    public class TrafficService : DefaultService<TrafficRepository, TrafficEntity, Guid>, ITrafficService
    {
        public TrafficService()
            : base(new TrafficRepository())
        {

        }

        public List<Traffic_ProjectName> ReadTraffic(RequestQueryModel query, string accountId, out long total)
        {
            return repository.ReadTraffic(query, accountId, out total);
        }

    }
}
