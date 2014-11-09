namespace Yashmak.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    using Yashmak.Data.Common;
    using Yashmak.Data.Common.Repository;

    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly IYashmakDbContex context;

        private readonly IDbSet<T> set;

        public Repository()
            : this(new YashmakDbContext())
        {
        }

        public Repository(IYashmakDbContex context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public virtual IQueryable<T> All()
        {
            return this.set.AsQueryable();
        }

        public virtual T GetById(int id)
        {
            return this.set.Find(id);
        }

        public virtual void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public virtual void Update(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Modified);
        }

        public virtual void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public virtual void Delete(int id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual void Detach(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Detached);
        }

        int IRepository<T>.SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        private void ChangeEntityState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}