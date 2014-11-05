namespace Yashmak.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Data.Migrations;
    using Yashmak.Models;

    public class YashmakDbContext : IdentityDbContext<AppUser>, IYashmakDbContex
    {
        public YashmakDbContext()
            : base("name=YashmakContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<YashmakDbContext, Configuration>());
        }

        public virtual IDbSet<Log> Logs { get; set; }

        public static YashmakDbContext Create()
        {
            return new YashmakDbContext();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}