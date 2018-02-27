using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleX.Infrastructure.Utility;
using DoubleX.Infrastructure.Core.Repository;

namespace DoubleX.Module.Organize
{
    /// <summary>
    /// 职员数据持久操作
    /// </summary>
    public class EmployeeRepository : EFRepository<EmployeeEntity, Guid>, IRepository<EmployeeEntity, Guid>
    {
        public EmployeeRepository()
            : base(new OrganizeContext())
        {
        }
    }
}
