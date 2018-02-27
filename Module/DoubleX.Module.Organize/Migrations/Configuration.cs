namespace DoubleX.Module.Organize.Migrations
{
    using Infrastructure.Utility;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DoubleX.Module.Organize.OrganizeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DoubleX.Module.Organize.OrganizeContext context)
        {
            var adminModel = new EmployeeEntity()
            {
                Account = "admin",
                Password = MD5Helper.Get("admin888"),
                IsDelete = false,
                CreateId = Guid.Empty,
                CreateDt = DateTime.Now,
                LastId = Guid.NewGuid(),
                LastDt = DateTime.Now
            };

            context.EmployeeEntities.AddOrUpdate(
              p => p.Id,
              adminModel
            );

        }
    }
}
