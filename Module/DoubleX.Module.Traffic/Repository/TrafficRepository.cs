using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core;
using DoubleX.Infrastructure.Core.Model;
using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Infrastructure.Core.Exceptions;
using DoubleX.Module.Common;

namespace DoubleX.Module.Traffic
{
    /// <summary>
    /// 流量数据持久操作
    /// </summary>
    public class TrafficRepository : EFRepository<TrafficEntity, Guid>, ITrafficRepository
    {
        public TrafficRepository()
            : base(new TrafficContext())
        {
        }


        public List<Traffic_ProjectName> ReadTraffic(RequestQueryModel query, string accountId, out long total)
        {
            if (query == null)
            {
                throw new DefaultException(EnumResultCode.参数错误, "query");
            }

            // new SqlParameter("@userId", userId ?? (object)DBNull.Value), 可空类型写法

            string sqlString = @"select A.*,B.ProjectName from TB_Traffic as A LEFT JOIN TB_Project as B ON A.ProjectId = B.Id where A.AccountId = @AccountId ";
            string totalSql = @"select count(Id) from TB_Traffic where AccountId = @AccountId ";

            total = dbContext.Database.SqlQuery<long>(totalSql, DBHelper.GetParameter("@AccountId", accountId)).FirstOrDefault();

            if (query.Sorting != null && query.Sorting.Count > 0)
            {
                sqlString += "order by ";
                foreach (var sort in query.Sorting)
                {
                    sqlString += sort.Key + " " + sort.Value + ",";
                }
            }
            sqlString = sqlString.Remove(sqlString.Length - 1, 1);
            sqlString += " limit @start,@icount ";

            return dbContext.Database.SqlQuery<Traffic_ProjectName>(sqlString,
                DBHelper.GetParameter("@AccountId", accountId),
                DBHelper.GetParameter("@start", query.PageIndex * query.PageSize),
                DBHelper.GetParameter("@icount", query.PageSize)).ToList();
        }
    }
}
