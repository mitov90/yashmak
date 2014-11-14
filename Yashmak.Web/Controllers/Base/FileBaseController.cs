namespace Yashmak.Web.Controllers.Base
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using Yashmak.Common;
    using Yashmak.Common.Assistants;
    using Yashmak.Data;
    using Yashmak.Web.Infrastructure.ActionResults;
    using Yashmak.Web.Infrastructure.Filters;
    using Yashmak.Web.ViewModels.Directory;
    using Yashmak.Web.ViewModels.File;

    using File = Yashmak.Data.Models.File;

    [Log]
    [Authorize]
    public abstract class FileBaseController : BaseController
    {
        protected FileBaseController(IYashmakData data)
            : base(data)
        {
        }

        [NonAction]
        protected ActionResult DownloadFile(File fileNode)
        {
            var pathToFile =
                this.Server.MapPath(
                    "~" + Constants.UserFilesPath + fileNode.User.UserName + "/" +
                    fileNode.PathToFile);
            var stream = new FileStream(pathToFile, FileMode.Open);
            return this.File(
                stream, 
                MimeAssistant.GetMimeType(fileNode.FileName), 
                fileNode.FileName);
        }

        [NonAction]
        protected ActionResult DownloadFolder(int fileNodeId, string curUserName, File fileNode)
        {
            var potentialDownloads =
                this.Data.Files.All()
                    .Include(f => f.Permission)
                    .Where(f => f.ParentId == fileNodeId && (!f.IsDirectory));

            var resultFilePath = new List<string>();
            foreach (var potentialFile in potentialDownloads)
            {
                if (PermissionManager.CheckPermission(potentialFile, this.UserId, curUserName))
                {
                    var mapPath =
                        this.Server.MapPath(
                            "~" + Constants.UserFilesPath + fileNode.User.UserName + "/" +
                            potentialFile.PathToFile);
                    resultFilePath.Add(mapPath);
                }
            }

            var zip = new ZipResult(resultFilePath) { FileName = fileNode.FileName + ".zip" };
            return zip;
        }

        [NonAction]
        protected File GetFileNode(int? filenodeid)
        {
            return this.Data.Files.All().FirstOrDefault(f => f.Id == filenodeid);
        }

        [NonAction]
        protected DirectoryViewModel CreateDirView(
            int? filenodeid, 
            IQueryable<FileViewModel> files, 
            IEnumerable<NavigationDirectoryViewModel> navView)
        {
            var dirView = new DirectoryViewModel
                {
                    Files = files, 
                    NavigationModels = navView, 
                    FileNodeId = filenodeid
                };
            return dirView;
        }

        [NonAction]
        protected IEnumerable<NavigationDirectoryViewModel> GetPath(File fileNode)
        {
            var list = new List<NavigationDirectoryViewModel>();

            while (fileNode != null)
            {
                list.Add(
                    new NavigationDirectoryViewModel
                        {
                            Id = fileNode.Id, 
                            FileName = fileNode.FileName
                        });
                fileNode = fileNode.Parent;
            }

            list.Add(new NavigationDirectoryViewModel { Id = null, FileName = "Home" });
            list.Reverse();
            return list;
        }
    }
}