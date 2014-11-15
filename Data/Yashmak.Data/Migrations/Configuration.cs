namespace Yashmak.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Data.Models;

    using Constants = Yashmak.Common.Constants;

    internal sealed class Configuration : DbMigrationsConfiguration<YashmakDbContext>
    {
        private UserManager<AppUser> userManager;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(YashmakDbContext context)
        {
            this.userManager = new UserManager<AppUser>(new UserStore<AppUser>(context));
            this.SeedRoles(context);
            this.SeedUsers(context);
        }

        private void SeedRoles(
            IdentityDbContext<AppUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> context)
        {
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(Constants.AdminRole));
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(Constants.NonPaidUser));
            context.SaveChanges();
        }

        private void SeedUsers(
            IdentityDbContext<AppUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var adminUser = new AppUser
                {
                    Email = "a@a.a", 
                    UserName = "a@a.a"
                };
            this.userManager.Create(adminUser, "asdasd");
            context.SaveChanges();
            this.userManager.AddToRole(adminUser.Id, Constants.AdminRole);
        }
    }
}