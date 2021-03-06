﻿namespace Yashmak.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Controllers.Base;
    using Yashmak.Web.ViewModels.Directory;

    [Authorize]
    public class DirectoryController : BaseController
    {
        public DirectoryController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult CreateFolder(int? filenodeid)
        {
            var viewModel = new NavigationDirectoryViewModel
                {
                    Id = null, 
                    FileName = "Private Home"
                };
            if (filenodeid != 0 && filenodeid != null)
            {
                viewModel.Id = filenodeid;
                var dir = this.Data.Files.GetById((int)filenodeid);
                if (dir.UserId != this.User.Identity.GetUserId())
                {
                    this.RedirectToAction("Index", "Files");
                }

                viewModel.FileName = dir.FileName;
            }

            object err;
            if (this.TempData.TryGetValue("Error", out err))
            {
                this.ModelState.AddModelError(string.Empty, err as string);
            }

            return this.View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateFolder(NavigationDirectoryViewModel directory)
        {
            if (this.ModelState.IsValid)
            {
                if (this.ExistDuplicatedName(directory))
                {
                    this.TempData["Error"] =
                        "Name alredy exists in parent folder, try change the name!";
                    return this.RedirectToAction("CreateFolder", "Directory",new {filenodeid = directory.Id});
                }

                this.Data.Files.Add(
                    new File
                        {
                            IsDirectory = true, 
                            FileName = directory.FileName, 
                            ParentId = directory.Id == 0 ? null : directory.Id, 
                            UserId = this.UserId, 
                            ModifiedOn = DateTime.Now, 
                        });
                this.Data.SaveChanges();
            }

            return this.RedirectToAction("Index", "Files", new { fileId = directory.Id });
        }

        private bool ExistDuplicatedName(NavigationDirectoryViewModel directory)
        {
            var parentId = directory.Id == 0 ? null : directory.Id;

            var existDuplicatedName =
                this.Data.Files.All()
                    .Where(d => d.ParentId == parentId && d.UserId == this.UserId)
                    .Any(d => d.FileName == directory.FileName);
            return existDuplicatedName;
        }
    }
}