namespace Yashmak.Web.ViewModels.Permission
{
    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class PermissionViewModel : IMapFrom<Permission>
    {
        public string FileName { get; set; }

        public int Id { get; set; }

        public AccessType AccessType { get; set; }

        public string People { get; set; }
    }
}