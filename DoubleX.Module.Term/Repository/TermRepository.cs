using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Module.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Term
{
    public class TermRepository : EFRepository<TermEntity, Guid>, ITermRepository
    {
        public TermRepository()
            : base(new TermContext())
        {
        }

        public List<string> StatisticsUserIds(Guid projectId)
        {

            var res = this.dbContext.Set<TermEntity>().Where(x => x.ProjectId == projectId).GroupBy(x => x.UserId).Select(y => (new { count = y.Count(), userid = y.Key })).ToList();
            List<string> userIds = new List<string>();
            foreach (var item in res)
            {
                userIds.Add(item.userid);
            }

            return userIds;
        }

        public List<string> GetProjectIdsByAccountId(string accountId)
        {
            string sql = "select Id from TB_Project where AccountId = @AccountId";
            return dbContext.Database.SqlQuery<string>(sql, DBHelper.GetParameter("@AccountId", accountId)).ToList();
        }
    }
}
