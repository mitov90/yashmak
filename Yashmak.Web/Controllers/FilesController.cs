﻿namespace Yashmak.Web.Controllers
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
    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;
    using Yashmak.Web.Models.Directory;

    using File = Yashmak.Data.Models.File;

    public class FilesController : Controller
    {
        private readonly IDeletableEntityRepository<File> repository;

        public FilesController(IDeletableEntityRepository<File> data)
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
            var curFile = this.repository.GetById(Convert.ToInt32(filenodeid));
            var navView = GetPath(curFile);

            var files =
                this.repository.All()
                    .Where(f => f.UserId == userId && f.ParentId == filenodeid)
                    .Include(f => f.Parent)
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
                return this.Content("Don't try silly things!");
            }

            var fileNode = this.repository.GetById((int)filenodeid);

            if (fileNode.UserId != this.User.Identity.GetUserId())
            {
                return this.Content("You're trying to delete things that not belong to you!");
            }

            fileNode.IsDeleted = true;
            this.repository.All()
                .Where(f => f.ParentId == filenodeid)
                .Update(f => new File { IsDeleted = true });

            this.repository.SaveChanges();

            return this.RedirectToAction("Index", "Files", new { filenodeid = fileNode.ParentId });
        }

        public ActionResult Download(int fileNodeId)
        {
            var curUserId = this.User.Identity.GetUserId();
            var curUserName = this.User.Identity.Name;
            var fileNode = this.repository.GetById(fileNodeId);
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
                            "~/App_Data/" + fileNode.User.UserName + "/" + fileNode.PathToFile);
                    var stream = new FileStream(pathToFile, FileMode.Open);
                    return File(
                        stream, 
                        MimeAssistant.GetMimeType(fileNode.FileName), 
                        fileNode.FileName);
                }
                else
                {
                    // return zip stream for folder
                }
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