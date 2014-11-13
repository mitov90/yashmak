namespace Yashmak.Data
{
    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;

    public interface IYashmakData
    {
        IYashmakDbContext Context { get; }

        IDeletableEntityRepository<File> Files { get; }

        IRepository<AppUser> Users { get; }

        IRepository<ShareName> Sharenames { get; }

        IRepository<Permission> Permissions { get; }

        IRepository<Log> Logs { get; }

        int SaveChanges();
    }
}