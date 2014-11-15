namespace Yashmak.Data
{
    using System;
    using System.Collections.Generic;

    using Yashmak.Data.Common.Models;
    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;
    using Yashmak.Data.Repositories;

    public class YashmakData : IYashmakData
    {
        private readonly IYashmakDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public YashmakData(IYashmakDbContext context)
        {
            this.context = context;
        }

        public IYashmakDbContext Context
        {
            get { return this.context; }
        }

        public IDeletableEntityRepository<File> Files
        {
            get { return this.GetDeletableEntityRepository<File>(); }
        }

        public IDeletableEntityRepository<Message> Messages
        {
            get { return this.GetDeletableEntityRepository<Message>(); }
        }

        public IRepository<AppUser> Users
        {
            get { return this.GetRepository<AppUser>(); }
        }

        public IRepository<ShareName> Sharenames
        {
            get { return this.GetRepository<ShareName>(); }
        }

        public IRepository<Permission> Permissions
        {
            get { return this.GetRepository<Permission>(); }
        }

        public IRepository<Log> Logs
        {
            get { return this.GetRepository<Log>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        private IDeletableEntityRepository<T> GetDeletableEntityRepository<T>()
            where T : class, IDeletableEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(DeletableEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeof(T)];
        }
    }
}