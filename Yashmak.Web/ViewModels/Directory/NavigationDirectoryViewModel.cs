namespace Yashmak.Web.ViewModels.Directory
{
    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class NavigationDirectoryViewModel : IMapFrom<File>
    {
        public int? Id { get; set; }

        public string FileName { get; set; }
    }
}