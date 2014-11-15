namespace Yashmak.Web.Areas.Admin.ViewModels.Files
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using Yashmak.Data.Models;
    using Yashmak.Web.Areas.Admin.ViewModels.Base;
    using Yashmak.Web.Infrastructure.Mapping;

    public class FileViewModel : AdministrationViewModel, IMapFrom<File>
    {
        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required]
        [MinLength(1)]
        public string FileName { get; set; }

        public int Size { get; set; }

        public bool IsDirectory { get; set; }

        public int? ParentId { get; set; }


        public string UserId { get; set; }

        public AppUser User { get; set; }

        public Permission Permission { get; set; }

    }
}