namespace Yashmak.Web.ViewModels.Directory
{
    using System.Collections.Generic;
    using System.Linq;

    using Yashmak.Web.ViewModels.File;

    public class DirectoryViewModel
    {
        public int? FileNodeId { get; set; }

        public IQueryable<FileViewModel> Files { get; set; }

        public IEnumerable<NavigationDirectoryViewModel> NavigationModels { get; set; }
    }
}