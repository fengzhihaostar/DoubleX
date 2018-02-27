using DoubleX.Infrastructure.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Term
{
    public class TermService : DefaultService<TermRepository, TermEntity, Guid>, ITermService
    {
        public TermService()
            : base(new TermRepository())
        {

        }

        public List<string> StatisticsUserIds(Guid projectId)
        {
            return this.repository.StatisticsUserIds(projectId);
        }

        public List<string> GetProjectIdsByAccountId(string accountId)
        {
            return this.repository.GetProjectIdsByAccountId(accountId);
        }
    }
}
