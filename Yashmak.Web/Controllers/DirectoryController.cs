﻿namespace Yashmak.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Filters;
    using Yashmak.Web.Models.Directory;

    [Authorize]
    [Log]
    public class DirectoryController : Controller
    {
        private readonly IYashmakData context;

        public DirectoryController(IYashmakData context)
        {
            this.context = context;
        }

        public ActionResult CreateFolder(int? filenodeid)
        {
            var viewModel = new NavigationDirectoryViewModel { Id = null, FileName = "Home" };
            if (filenodeid != 0 && filenodeid != null)
            {
                viewModel.Id = filenodeid;
                var dir = this.context.Files.GetById((int)filenodeid);
                if (dir.UserId != this.User.Identity.GetUserId())
                {
                    this.RedirectToAction("Index", "Files");
                }

                viewModel.FileName = dir.FileName;
            }

            return this.PartialView(viewModel);
        }

        // [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateFolder(NavigationDirectoryViewModel directory)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Files", new { fileId = directory.Id });
            }

            var userId = this.User.Identity.GetUserId();
            var newDir = new File
                {
                    IsDirectory = true, 
                    FileName = directory.FileName, 
                    ParentId = directory.Id == 0 ? null : directory.Id, 
                    UserId = userId, 
                    ModifiedOn = DateTime.Now, 
                };
            var parentId = directory.Id == 0 ? null : directory.Id;
            var dirsInUserFolder =
                this.context.Files.All().Where(d => d.ParentId == parentId && d.UserId == userId);
            if (dirsInUserFolder.Any(d => d.FileName == directory.FileName))
            {
                this.ModelState.AddModelError(
                    "Duplicated name!", 
                    "Name alredy exists in parent folder, try change the name!");
                return this.View(directory);
            }

            this.context.Files.Add(newDir);
            this.context.SaveChanges();

            return this.RedirectToAction("Index", "Files", new { filenodeid = directory.Id });
        }
    }
}