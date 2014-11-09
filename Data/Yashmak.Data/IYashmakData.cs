namespace Yashmak.Data
{
    using Yashmak.Data.Repositories;
    using Yashmak.Models;

    public interface IYashmakData
    {
        IRepository<AppUser> Users { get; }

        IRepository<Log> Logs { get; }

        IRepository<File> Files { get; }

        void SaveChanges();
    }
}