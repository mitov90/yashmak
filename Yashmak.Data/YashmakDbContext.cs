namespace Yashmak.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Models;

    public class YashmakDbContext : IdentityDbContext<User>
    {
        public YashmakDbContext()
            : base("name=YashmakContext")
        {
        }

        public static YashmakDbContext Create()
        {
            return new YashmakDbContext();
        }
    }
}