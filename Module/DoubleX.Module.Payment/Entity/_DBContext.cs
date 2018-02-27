using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace DoubleX.Module.Trade
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class TradeContext : DbContext
    {
        public TradeContext()
            : base("name=DatabaseEntities")
        {
        }

        public DbSet<CostRecordEntity> CostRecordEntities { get; set; }
        public DbSet<RechargeRecordEntity> PaymentRecordEntities { get; set; }

    }
}
