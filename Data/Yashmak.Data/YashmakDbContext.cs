namespace Yashmak.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Data.Common.CodeFirstConventions;
    using Yashmak.Data.Common.Models;
    using Yashmak.Data.Migrations;
    using Yashmak.Data.Models;

    public class YashmakDbContext : IdentityDbContext<AppUser>, IYashmakDbContext
    {
        public YashmakDbContext()
            : base("name=YashmakContext")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<YashmakDbContext, Configuration>());
        }

        public DbContext DbContext
        {
            get { return this; }
        }

        public virtual IDbSet<File> Files { get; set; }

        public virtual IDbSet<Message> Messages { get; set; }

        public virtual IDbSet<ShareName> ShareNames { get; set; }

        public virtual IDbSet<Permission> Permissions { get; set; }

        public virtual IDbSet<Log> Logs { get; set; }

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

        public static YashmakDbContext Create()
        {
            return new YashmakDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new IsUnicodeAttributeConvention());

            base.OnModelCreating(modelBuilder);

            // Without this call EntityFramework won't be able to configure the identity model
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