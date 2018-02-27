namespace DoubleX.Module.Member.Migrations
{
    using Infrastructure.Core.Model;
    using Infrastructure.Utility;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DoubleX.Module.Member.MemberContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DoubleX.Module.Member.MemberContext context)
        {
            var defaultUser = new MemberEntity()
            {
                Account = "user1",
                Password = MD5Helper.Get("123456"),
                RealName = "userA",
                Sex = EnumHelper.GetValue(EnumSex.±£ÃÜ),
                Email = "",
                EmailIsVerify = false,
                Mobile = "",
                MobileIsVerify = false,
                Credits = "",
                Birthday = DateTimeHelper.DefaultDateTime,
                Country = "",
                Area = "",
                Address = "",
                LastLoginIP = "#",
                LastLoginDt = DateTimeHelper.DefaultDateTime,
                Balance = 0,
                Type = EnumHelper.GetValue(EnumMemberType.Default),
                State = EnumHelper.GetValue(EnumMemberState.ÆôÓÃ),
                IsDelete = false,
                CreateId = Guid.Empty,
                CreateDt = DateTime.Now,
                LastId = Guid.NewGuid(),
                LastDt = DateTime.Now
            };

            context.MemberEntities.AddOrUpdate(
              p => p.Id,defaultUser);
        }
    }
}
