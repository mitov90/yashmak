namespace Yashmak.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Yashmak.Data.Models;

    public interface IYashmakDbContext
    {
        IDbSet<File> Files { get; set; }

        IDbSet<ShareName> ShareNames { get; set; }

        IDbSet<Permission> Permissions { get; set; }

        IDbSet<Log> Logs { get; set; }

        DbContext DbContext { get; }

        int SaveChanges();

        void Dispose();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}