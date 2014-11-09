namespace Yashmak.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Yashmak.Models;

    public interface IYashmakDbContex : IDisposable
    {
        IDbSet<AppUser> Users { get; set; }

        IDbSet<Log> Logs { get; set; }

        IDbSet<File> Files { get; set; }

        void SaveChanges();

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}