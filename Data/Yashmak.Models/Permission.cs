namespace Yashmak.Data.Models
{
    using System.Collections.Generic;

    public class Permission
    {
        public int Id { get; set; }

        public AccessType AccessType { get; set; }

        public virtual ICollection<ShareName> AuthorizedUsers { get; set; }
    }
}