using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Project.Entity
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ProjectContext : DbContext
    {
        public ProjectContext()
            : base("name=DatabaseEntities")
        {
        }

        public DbSet<ProjectEntity> TrafficEntities { get; set; }
    }
}
