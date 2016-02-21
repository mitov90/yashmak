namespace Yashmak.Web.Areas.Admin.ViewModels.Users
{
    using System.Linq;

    using AutoMapper;

    using Yashmak.Common;
    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class UserViewModel : IHaveCustomMappings, IMapFrom<AppUser>
    {
        public int FilesCount { get; set; }

        public string Id { get; set; }

        public RoleViewModel Role { get; set; }

        public int UsedSpace { get; set; }

        public string UserName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AppUser, UserViewModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(t => t.Id))
                .ForMember(m => m.UserName, opt => opt.MapFrom(t => t.UserName))
                .ForMember(m => m.FilesCount, opt => opt.MapFrom(t => t.Files.Count))
                .ForMember(
                    m => m.Role, 
                    opt =>
                    opt.MapFrom(
                        t =>
                        t.Roles.FirstOrDefault().UserId != null
                            ? new RoleViewModel
                                  {
                                      Name = Constants.AdminRole
                                  }
                            : new RoleViewModel
                                  {
                                      Name = Constants.NonPaidUser
                                  }))
                .ForMember(m => m.UsedSpace, opt => opt.MapFrom(t => t.Files.Sum(f => f.Size)))
                .ReverseMap();
        }
    }
}