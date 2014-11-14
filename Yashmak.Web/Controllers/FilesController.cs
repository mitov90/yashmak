namespace Yashmak.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using EntityFramework.Extensions;

    using Yashmak.Common;
    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Controllers.Base;
    using Yashmak.Web.ViewModels.File;

    public class FilesController : FileBaseController
    {
        public FilesController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index(int? fileId)
        {
            return this.View(fileId);
        }

        public ActionResult ViewFolder(int? filenodeid)
        {
            var fileNode = this.GetFileNode(filenodeid);
            var navView = this.GetPath(fileNode);

            var files =
                this.Data.Files.All()
                    .Where(f => f.UserId == this.UserId && f.ParentId == filenodeid)
                    .Include(f => f.Parent)
                    .Include(f => f.Permission)
                    .OrderByDescending(f => f.IsDirectory)
                    .Project()
                    .To<FileViewModel>();

            var dirView = this.CreateDirView(filenodeid, files, navView);

            return this.PartialView("_ViewFolder", dirView);
        }

        public ActionResult ViewFile(int fileNodeId)
        {
            var fileViewModel = AutoMapper.Mapper.Map<FileViewModel>(this.GetFileNode(fileNodeId));

            return this.PartialView(fileViewModel);
        }

        public ActionResult Download(int fileNodeId)
        {
            var curUserName = this.User.Identity.Name;
            var fileNode = this.GetFileNode(fileNodeId);

            if (fileNode == null)
            {
                return this.Json(new { message = "Not existing file, Redirecting to Err page" });
            }

            if (!PermissionManager.CheckPermission(fileNode, this.UserId, curUserName))
            {
                return this.Json(new { message = "Not authorized, Access denied!" });
            }

            return fileNode.IsDirectory
                       ? this.DownloadFolder(fileNodeId, curUserName, fileNode)
                       : this.DownloadFile(fileNode);
        }
    }
}