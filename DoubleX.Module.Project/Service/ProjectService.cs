using DoubleX.Infrastructure.Core.Service;
using DoubleX.Module.Project.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Project
{
    public class ProjectService : DefaultService<ProjectRepository, ProjectEntity, Guid>, IProjectService
    {
        public ProjectService()
            : base(new ProjectRepository())
        {

        }
    }
}
