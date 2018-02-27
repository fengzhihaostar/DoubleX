using DoubleX.Infrastructure.Core.Repository;
using DoubleX.Module.Project.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Project
{
    public class ProjectRepository : EFRepository<ProjectEntity, Guid>, IProjectRepository
    {
        public ProjectRepository()
            : base(new ProjectContext())
        {
        }
    }
}
