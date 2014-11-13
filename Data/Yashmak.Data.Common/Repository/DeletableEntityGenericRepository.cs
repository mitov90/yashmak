namespace Yashmak.Data.Common.Repository
{
    using System.Data.Entity;
    using System.Linq;

    using Yashmak.Data.Common.Models;

    public class DeletableEntityGenericRepository<T> : GenericRepository<T>, IDeletableEntityRepository<T>
        where T : class, IDeletableEntity
    {
        public DeletableEntityGenericRepository(DbContext context)
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
    }
}