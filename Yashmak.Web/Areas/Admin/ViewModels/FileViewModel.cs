namespace Yashmak.Web.Areas.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class FileViewModel : AdministrationViewModel, IMapFrom<File>
    {
        [HiddenInput(DisplayValue = false)]
        public int? Id { get; set; }

        [Required]
        [MinLength(1)]
        public string FileName { get; set; }

        public string PathToFile { get; set; }

        public int Size { get; set; }

        public bool IsDirectory { get; set; }

        public int? ParentId { get; set; }

        public virtual File Parent { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        public int? PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}