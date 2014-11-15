namespace Yashmak.Web.Areas.Admin.ViewModels.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;

    using AutoMapper;

    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class UserViewModel : IHaveCustomMappings, IMapFrom<AppUser>
    {

        public string Id { get; set; }

        public string UserName { get; set; }

        public int FilesCount { get; set; }

        public int UsedSpace { get; set; }

        public string Role { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AppUser, UserViewModel>()
                         .ForMember(m => m.Id, opt => opt.MapFrom(t => t.Id))
                         .ForMember(m => m.UserName, opt => opt.MapFrom(t => t.UserName))
                         .ForMember(
                             m => m.Role, 
                             opt => opt.MapFrom(t => t.Roles.FirstOrDefault().RoleId))
                         .ForMember(m => m.FilesCount, opt => opt.MapFrom(t => t.Files.Count))
                         .ForMember(
                             m => m.UsedSpace, 
                             opt => opt.MapFrom(t => t.Files.Sum(f => f.Size)))
                         .ReverseMap();
        }


    }
}