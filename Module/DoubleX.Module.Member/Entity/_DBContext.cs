using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace DoubleX.Module.Member
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class MemberContext : DbContext
    {
        public MemberContext()
            : base("name=DatabaseEntities")
        {
        }
        
        public DbSet<MemberEntity> MemberEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<MemberDetailModel>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
