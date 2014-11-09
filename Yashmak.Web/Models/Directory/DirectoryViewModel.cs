namespace Yashmak.Web.Models.Directory
{
    using System.Collections.Generic;
    using System.Linq;

    public class DirectoryViewModel
    {
        public IQueryable<FileViewModel> Files { get; set; }

        public IEnumerable<NavigationDirectoryViewModel> NavigationModels { get; set; }
    }
}