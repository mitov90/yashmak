namespace Yashmak.Web.Areas.Admin.ViewModels.Users
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Web.Infrastructure.Mapping;

    public class RoleViewModel : IMapFrom<IdentityRole>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}