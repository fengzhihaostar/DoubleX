using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace DoubleX.Module.Traffic
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class TrafficContext : DbContext
    {
        public TrafficContext()
            : base("name=DatabaseEntities")
        {
        }

        public DbSet<TrafficEntity> TrafficEntities { get; set; }
    }
}
