#0.EF ����MySql Code First ��MySql ����֧��.
#1. DBContext ʾ��
//��ʶMySQL ���� �뾲̬���췽�� DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration()); ������ͬ
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