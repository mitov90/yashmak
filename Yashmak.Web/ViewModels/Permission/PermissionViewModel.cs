namespace Yashmak.Web.Models.Permission
{
    using Yashmak.Data.Models;

    public class PermissionViewModel
    {
        public string FileName { get; set; }

        public int Id { get; set; }

        public AccessType AccessType { get; set; }

        public string People { get; set; }
    }
}