using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Module.Term
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class TermContext : DbContext
    {
        public TermContext()
            : base("name=DatabaseEntities")
        {
        }

        public DbSet<TermEntity> TrafficEntities { get; set; }
    }
}
