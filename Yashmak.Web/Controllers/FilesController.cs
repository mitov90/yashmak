namespace Yashmak.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using EntityFramework.Extensions;

    using Microsoft.AspNet.Identity;

    using Yashmak.Common;
    using Yashmak.Common.Assistants;
    using Yashmak.Data;
    using Yashmak.Web.Infrastructure.ActionResults;
    using Yashmak.Web.Infrastructure.Filters;
    using Yashmak.Web.ViewModels.Directory;
    using Yashmak.Web.ViewModels.File;

    using Constants = Yashmak.Common.Constants;
    using File = Yashmak.Data.Models.File;

    [Log]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IYashmakData repository;

        public FilesController(IYashmakData data)
        {
            this.repository = data;
        }

        public ActionResult Index(int? fileId)
        {
            return this.View(fileId);
        }

        public ActionResult ViewFolder(int? filenodeid)
        {
            var userId = this.HttpContext.User.Identity.GetUserId();
            var curFile = this.repository.Files.GetById(Convert.ToInt32(filenodeid));
            var navView = GetPath(curFile);

            var files =
                this.repository.Files.All()
                    .Where(f => f.UserId == userId && f.ParentId == filenodeid)
                    .Include(f => f.Parent)
                    .Include(f => f.Permission)
                    .OrderByDescending(f => f.IsDirectory)
                    .Project()
                    .To<FileViewModel>();

            var dirView = new DirectoryViewModel
                {
                    Files = files, 
                    NavigationModels = navView, 
                    FileNodeId = filenodeid
                };

            return this.PartialView("_ViewFolder", dirView);
        }

        public ActionResult Delete(int? filenodeid, string fileUrl)
        {
            if (filenodeid == null || filenodeid == 0)
            {
                return this.Json("Don't try silly things!");
            }

            var fileNode = this.repository.Files.GetById((int)filenodeid);

            if (fileNode.UserId != this.User.Identity.GetUserId())
            {
                return this.Json("You're trying to delete things that not belong to you!");
            }

            fileNode.IsDeleted = true;
            this.repository.Files.All()
                .Where(f => f.ParentId == filenodeid)
                .Update(f => new File { IsDeleted = true });

            this.repository.SaveChanges();

            return this.RedirectToAction("Index", "Files", new { filenodeid = fileNode.ParentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(FileViewModel fileModel)
        {
            var fileNode = this.repository.Files.All().FirstOrDefault(f => f.Id == fileModel.Id);
            if (fileNode == null)
            {
                this.ModelState.AddModelError(string.Empty, "No such file");
                return this.View(fileModel);
            }

            if (fileNode.UserId != this.User.Identity.GetUserId())
            {
                return this.Json("You're trying to delete things that not belong to you!");
            }

            var existFileName =
                this.repository.Files.All()
                    .Any(f => f.ParentId == fileNode.ParentId && f.FileName == fileModel.FileName);
            if (existFileName)
            {
                this.ModelState.AddModelError(string.Empty, "Such filename exist in this folder!");
                return this.View(fileModel);
            }

            fileNode.FileName = fileModel.FileName;
            this.repository.SaveChanges();

            return this.RedirectToAction("Index", "Files", new { filenodeid = fileNode.ParentId });
        }

        public ActionResult Rename(int? filenodeid)
        {
            var fileNode =
                this.repository.Files.All()
                    .Where(f => f.Id == filenodeid)
                    .Project()
                    .To<FileViewModel>()
                    .FirstOrDefault();

            return this.View(fileNode);
        }

        public ActionResult Download(int fileNodeId)
        {
            var curUserId = this.User.Identity.GetUserId();
            var curUserName = this.User.Identity.Name;
            var fileNode = this.repository.Files.All().FirstOrDefault(f => f.Id == fileNodeId);
            if (fileNode == null)
            {
                return this.Json(new { message = "Not existing file, Redirecting to Err page" });
            }

            if (PermissionManager.CheckPermission(fileNode, curUserId, curUserName))
            {
                if (!fileNode.IsDirectory)
                {
                    // Download file
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

                var potentialDownloads =
                    this.repository.Files.All()
                        .Include(f => f.Permission)
                        .Where(f => f.ParentId == fileNodeId && (!f.IsDirectory));

                var resultFilePath = new List<string>();
                foreach (var potentialFile in potentialDownloads)
                {
                    if (PermissionManager.CheckPermission(potentialFile, curUserId, curUserName))
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

            return this.Json(new { message = "Not authorized, Access denied!" });
        }

        [NonAction]
        private static IEnumerable<NavigationDirectoryViewModel> GetPath(File fileNode)
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