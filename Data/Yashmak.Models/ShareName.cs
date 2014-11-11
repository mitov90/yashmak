namespace Yashmak.Data.Models
{
    public class ShareName
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}