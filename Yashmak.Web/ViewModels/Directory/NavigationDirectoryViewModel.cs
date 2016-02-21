namespace Yashmak.Web.ViewModels.Directory
{
    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class NavigationDirectoryViewModel : IMapFrom<File>
    {
        public string FileName { get; set; }

        public int? Id { get; set; }
    }
}