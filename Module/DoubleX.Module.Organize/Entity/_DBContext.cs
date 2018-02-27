using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace DoubleX.Module.Organize
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class OrganizeContext : DbContext
    {
        public OrganizeContext()
            : base("name=DatabaseEntities")
        {
           
        }

        public DbSet<EmployeeEntity> EmployeeEntities { get; set; }
       
    }
}
