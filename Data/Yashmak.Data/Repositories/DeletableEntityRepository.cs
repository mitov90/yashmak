namespace Yashmak.Data.Repositories
{
    using System.Linq;

    using Yashmak.Data.Common;
    using Yashmak.Data.Common.Repository;

    public class DeletableEntityRepository<T> : Repository<T>, IDeletableEntityRepository<T>
        where T : class, IDeletableEntity
    {
        public DeletableEntityRepository(IYashmakDbContex context)
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