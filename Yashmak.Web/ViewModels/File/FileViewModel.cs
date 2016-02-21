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

        public string FileName { get; set; }

        public int? Id { get; set; }

        public bool IsDirectory { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ParentId { get; set; }

        public PermissionViewModel Permission { get; set; }

        public int Size { get; set; }
    }
}