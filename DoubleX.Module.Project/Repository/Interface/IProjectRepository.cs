using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Module.Project.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Project
{
    /// <summary>
    /// 数据持久接口
    /// </summary>
    public interface IProjectRepository : IRepository<ProjectEntity, Guid>
    {

    }
}
