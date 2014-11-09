namespace Yashmak.Web.Models.Directory
{
    using System;

    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class FileViewModel : IMapFrom<File>
    {
        public FileViewModel()
        {
            this.IsDirectory = false;
        }

        public int? Id { get; set; }

        public string FileName { get; set; }

        public int Size { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDirectory { get; set; }
    }
}