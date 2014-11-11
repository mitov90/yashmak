namespace Yashmak.Data.Models
{
    using System.Collections.Generic;

    public class Permission
    {
        public Permission()
        {
            this.AuthorizedUsers = new HashSet<ShareName>();
        }

        public int Id { get; set; }

        public AccessType AccessType { get; set; }

        public ICollection<ShareName> AuthorizedUsers { get; set; }
    }
}