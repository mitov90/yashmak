namespace Yashmak.Web.ViewModels.File
{
    using System;

    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;
    using Yashmak.Web.ViewModels.Permission;

    public class FileViewModel : IMapFrom<File>
    {
        public FileViewModel()
        {
            this.IsDirectory = false;
        }

        public int? Id { get; set; }

        public string FileName { get; set; }

        public PermissionViewModel Permission { get; set; }

        public int? ParentId { get; set; }

        public int Size { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDirectory { get; set; }
    }
}