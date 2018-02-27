#0.EF 连接MySql Code First 需MySql 环境支持.
#1. DBContext 示列
//标识MySQL 配置 与静态构造方法 DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration()); 作用相同
[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
public class OrganizeContext : DbContext
{
    public OrganizeContext()
        : base("name=DatabaseEntities")
    {
        
    }

    //static OrganizeContext()
    //{
    //    DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
    //}

    public DbSet<EmployeeEntity> EmployeeEntities { get; set; }
   
    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);
    //}

}