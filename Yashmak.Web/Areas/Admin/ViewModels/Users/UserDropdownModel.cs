namespace Yashmak.Web.Areas.Admin.ViewModels.Users
{
    using AutoMapper;

    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class UserDropdownModel : IHaveCustomMappings, IMapFrom<AppUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AppUser, UserViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(m => m.UserName, opt => opt.MapFrom(t => t.UserName));
        }
    }
}