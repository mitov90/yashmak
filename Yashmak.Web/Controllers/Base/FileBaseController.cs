namespace Yashmak.Web.Controllers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using Yashmak.Common;
    using Yashmak.Common.Assistants;
    using Yashmak.Data;
    using Yashmak.IO;
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
        protected DirectoryViewModel CreateDirView(
            int? filenodeid, 
            IQueryable<FileViewModel> files, 
            IEnumerable<NavigationDirectoryViewModel> navView)
        {
            var dirView = new DirectoryViewModel { Files = files, NavigationModels = navView, FileNodeId = filenodeid };
            return dirView;
        }

        [NonAction]
        protected ActionResult DownloadFile(File fileNode)
        {
            var provider = new AzureStorageProvider();
            var stream = provider.StreamFile(fileNode.UserId, fileNode.PathToFile);
            return this.File(stream, MimeAssistant.GetMimeType(fileNode.FileName), fileNode.FileName);
        }

        [NonAction]
        protected ActionResult DownloadFolder(int fileNodeId, string curUserName, File fileNode)
        {
            var potentialDownloads =
                this.Data.Files.All()
                    .Include(f => f.Permission)
                    .Where(f => f.ParentId == fileNodeId && (!f.IsDirectory));

            var resultFiles = new List<Tuple<string, Stream>>();
            var provider = new AzureStorageProvider();
            foreach (var potentialFile in potentialDownloads)
            {
                if (PermissionManager.CheckPermission(potentialFile, this.UserId, curUserName))
                {
                    resultFiles.Add(
                        new Tuple<string, Stream>(
                            potentialFile.FileName, 
                            provider.StreamFile(this.UserId, potentialFile.PathToFile)));
                }
            }

            var zip = new ZipResult(resultFiles) { FileName = fileNode.FileName + ".zip" };
            return zip;
        }

        [NonAction]
        protected File GetFileNode(int? filenodeid)
        {
            return this.Data.Files.All().FirstOrDefault(f => f.Id == filenodeid);
        }

        [NonAction]
        protected IEnumerable<NavigationDirectoryViewModel> GetPath(File fileNode)
        {
            var list = new List<NavigationDirectoryViewModel>();

            while (fileNode != null)
            {
                list.Add(new NavigationDirectoryViewModel { Id = fileNode.Id, FileName = fileNode.FileName });
                fileNode = fileNode.Parent;
            }

            list.Add(new NavigationDirectoryViewModel { Id = null, FileName = "Home" });
            list.Reverse();
            return list;
        }

        [NonAction]
        protected IOrderedQueryable<File> SortQuery(string sortOrder, IOrderedQueryable<File> files)
        {
            this.ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            this.ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            this.ViewBag.SizeSortParm = sortOrder == "Size" ? "Size_desc" : "Size";
            this.ViewBag.PermSortParm = sortOrder == "Perm" ? "Perm_desc" : "Perm";

            switch (sortOrder)
            {
                case "Name_desc":
                    files = files.ThenByDescending(f => f.FileName);
                    break;
                case "Name":
                    files = files.ThenBy(f => f.FileName);
                    break;
                case "Date":
                    files = files.ThenBy(s => s.ModifiedOn);
                    break;
                case "Date_desc":
                    files = files.ThenByDescending(s => s.ModifiedOn);
                    break;
                case "Size":
                    files = files.ThenBy(s => s.Size);
                    break;
                case "Size_desc":
                    files = files.ThenByDescending(s => s.Size);
                    break;
                case "Perm":
                    files = files.ThenBy(s => s.Permission.AccessType);
                    break;
                case "Perm_desc":
                    files = files.ThenByDescending(s => s.Permission.AccessType);
                    break;
            }

            return files;
        }
    }
}