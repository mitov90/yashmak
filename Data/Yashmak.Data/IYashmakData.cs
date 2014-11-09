namespace Yashmak.Data
{
    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;

    public interface IYashmakData
    {
        IRepository<AppUser> Users { get; }

        IRepository<Log> Logs { get; }

        IRepository<File> Files { get; }

        void SaveChanges();
    }
}