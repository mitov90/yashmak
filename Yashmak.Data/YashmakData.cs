namespace Yashmak.Data
{
    using System;
    using System.Collections.Generic;

    using Yashmak.Data.Repositories;
    using Yashmak.Models;

    internal class YashmakData : IYashmakData
    {
        private readonly IYashmakDbContex context;

        private readonly IDictionary<Type, object> repositories;

        public YashmakData()
            : this(new YashmakDbContext())
        {
        }

        private YashmakData(IYashmakDbContex context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<AppUser> Users
        {
            get
            {
                return this.GetRepository<AppUser>();
            }
        }

        public IRepository<Log> Logs
        {
            get
            {
                return this.GetRepository<Log>();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);
            if (this.repositories.ContainsKey(typeOfModel))
            {
                return (IRepository<T>)this.repositories[typeOfModel];
            }

            var type = typeof(Repository<T>);

            this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));

            return (IRepository<T>)this.repositories[typeOfModel];
        }
    }
}