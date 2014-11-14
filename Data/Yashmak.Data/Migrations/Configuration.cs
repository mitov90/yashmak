namespace Yashmak.Data.Migrations
{
    using System.Data.Entity.Migrations;

    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<YashmakDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(YashmakDbContext context)
        {

        }
    }
}