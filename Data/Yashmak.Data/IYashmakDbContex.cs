namespace Yashmak.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Yashmak.Data.Models;

    public interface IYashmakDbContex : IDisposable
    {
        IDbSet<AppUser> Users { get; set; }

        IDbSet<Log> Logs { get; set; }

        IDbSet<File> Files { get; set; }

        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IDbSet<T> Set<T>() where T : class;
    }
}