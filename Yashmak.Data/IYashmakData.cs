namespace Yashmak.Data
{
    using Yashmak.Data.Repositories;
    using Yashmak.Models;

    internal interface IYashmakData
    {
        IRepository<AppUser> Users { get; }

        IRepository<Log> Logs { get; }

        void SaveChanges();
    }
}