namespace Yashmak.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Data.Common.Models;
    using Yashmak.Data.Migrations;
    using Yashmak.Data.Models;

    public class YashmakDbContext : IdentityDbContext<AppUser>
    {
        public YashmakDbContext()
            : base("name=YashmakContext")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<YashmakDbContext, Configuration>());
        }

        public virtual IDbSet<File> Files { get; set; }

        public virtual IDbSet<ShareName> ShareNames { get; set; }

        public virtual IDbSet<Permission> Permissions { get; set; }

        public virtual IDbSet<Log> Logs { get; set; }

        public static YashmakDbContext Create()
        {
            return new YashmakDbContext();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo &&
                        ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}