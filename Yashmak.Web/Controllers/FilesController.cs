using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Yashmak.Common;
using Yashmak.Data;
using Yashmak.Web.Controllers.Base;
using Yashmak.Web.ViewModels.File;

namespace Yashmak.Web.Controllers
{
    public class FilesController : FileBaseController
    {
        public FilesController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index(int? fileId, string sortOrder)
        {
            ViewBag.sortOrder = sortOrder;
            return View(fileId);
        }

        public ActionResult ViewFolder(int? filenodeid, string sortOrder)
        {
            var fileNode = GetFileNode(filenodeid);
            var navView = GetPath(fileNode);

            var files =
                Data.Files.All()
                    .Where(f => f.UserId == UserId && f.ParentId == filenodeid)
                    .Include(f => f.Parent)
                    .Include(f => f.Permission)
                    .OrderByDescending(f => f.IsDirectory);

            files = SortQuery(sortOrder, files);

            var result = files.ProjectTo<FileViewModel>();

            var dirView = CreateDirView(filenodeid, result, navView);
            return PartialView("_ViewFolder", dirView);
        }

        public ActionResult ViewFile(int fileNodeId)
        {
            var fileViewModel = Mapper.Map<FileViewModel>(GetFileNode(fileNodeId));

            return PartialView(fileViewModel);
        }

        public ActionResult Download(int fileNodeId)
        {
            var curUserName = User.Identity.Name;
            var fileNode = GetFileNode(fileNodeId);

            if (fileNode == null)
            {
                return Json(new {message = "Not existing file, Redirecting to Err page"});
            }

            if (!PermissionManager.CheckPermission(fileNode, UserId, curUserName))
            {
                return Json(new {message = "Not authorized, Access denied!"});
            }

            return fileNode.IsDirectory
                ? DownloadFolder(fileNodeId, curUserName, fileNode)
                : DownloadFile(fileNode);
        }
    }
}