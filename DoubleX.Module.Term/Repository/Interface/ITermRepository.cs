using DoubleX.Infrastructure.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Term
{
    /// <summary>
    /// 数据持久接口
    /// </summary>
    public interface ITermRepository : IRepository<TermEntity, Guid>
    {
        List<string> StatisticsUserIds(Guid projectId);

        List<string> GetProjectIdsByAccountId(string accountId);
    }
}
