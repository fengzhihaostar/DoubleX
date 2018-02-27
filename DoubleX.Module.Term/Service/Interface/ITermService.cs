using DoubleX.Infrastructure.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Term
{
    public interface ITermService : IService<TermEntity, Guid>
    {
        List<string> StatisticsUserIds(Guid projectId);

        List<string> GetProjectIdsByAccountId(string accountId);
    }
}
