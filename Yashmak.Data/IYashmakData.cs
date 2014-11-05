namespace Yashmak.Data
{
    using Yashmak.Data.Repositories;
    using Yashmak.Models;

    internal interface IYashmakData
    {
        IRepository<User> Users { get; }

        void SaveChanges();
    }
}