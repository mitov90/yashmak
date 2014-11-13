namespace Yashmak.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Yashmak.Data.Common.Models;
    using Yashmak.Data.Common.Repository;

    public class DeletableEntityRepository<T> :
        GenericRepository<T>, 
        IDeletableEntityRepository<T> where T : class, IDeletableEntity
    {
        public DeletableEntityRepository(IYashmakDbContext context)
            : base(context)
        {
        }

        public override IQueryable<T> All()
        {
            return base.All().Where(x => !x.IsDeleted);
        }

        public IQueryable<T> AllWithDeleted()
        {
            return base.All();
        }

        public override void Delete(T entity)
        {
            entity.DeletedOn = DateTime.Now;
            entity.IsDeleted = true;

            var entry = this.Context.Entry(entity);
            entry.State = EntityState.Modified;
        }
    }
}