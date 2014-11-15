namespace Yashmak.Web.Areas.Admin.ViewModels.Files
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using AutoMapper;

    using Yashmak.Data.Models;
    using Yashmak.Web.Areas.Admin.ViewModels.Base;
    using Yashmak.Web.Infrastructure.Mapping;

    public class FileViewModel : AdministrationViewModel, IMapFrom<File>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required]
        [MinLength(1)]
        public string FileName { get; set; }

        public int Size { get; set; }

        public bool IsDirectory { get; set; }

        public int? ParentId { get; set; }

        public string UserName { get; set; }

        public string Permission { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<File, FileViewModel>()
                         .ForMember(m => m.Id, opt => opt.MapFrom(t => t.Id))
                         .ForMember(m => m.FileName, opt => opt.MapFrom(t => t.FileName))
                         .ForMember(m => m.Size, opt => opt.MapFrom(t => t.Size))
                         .ForMember(m => m.IsDirectory, opt => opt.MapFrom(t => t.IsDirectory))
                         .ForMember(m => m.ParentId, opt => opt.MapFrom(t => t.ParentId))
                         .ForMember(m => m.UserName, opt => opt.MapFrom(t => t.User.UserName))
                         .ForMember(
                             m => m.Permission, 
                             opt =>
                             opt.MapFrom(
                                 t =>
                                 t.Permission.AccessType.ToString() == string.Empty
                                     ? "Parent"
                                     : t.Permission.AccessType.ToString()))
                         .ForMember(m => m.FileName, opt => opt.MapFrom(t => t.FileName))
                         .ReverseMap();
        }
    }
}