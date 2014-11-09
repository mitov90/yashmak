namespace Yashmak.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;
    using Yashmak.Web.Models.Directory;

    public class FilesController : Controller
    {
        private readonly IRepository<File> dbContext;

        public FilesController(IRepository<File> data)
        {
            this.dbContext = data;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(File file)
        {
            if (this.ModelState.IsValid)
            {
                file.UserId = this.User.Identity.GetUserId();
                this.dbContext.Add(file);
                this.dbContext.SaveChanges();
            }

            return this.View(file);
        }

        public ActionResult CreateDirectory()
        {
            return this.View();
        }

        public ActionResult ViewFolder(int? fileId)
        {
            var userId = this.HttpContext.User.Identity.GetUserId();
            var curFile = this.dbContext.GetById(Convert.ToInt32(fileId));
            var navView = GetPath(curFile);

            var files =
                this.dbContext.All()
                    .Where(f => f.UserId == userId && f.ParentId == fileId)
                    .Include(f => f.Parent)
                    .OrderByDescending(f => f.IsDirectory)
                    .Project()
                    .To<FileViewModel>();
            var dirView = new DirectoryViewModel { Files = files, NavigationModels = navView };
            

            return this.PartialView("_ViewFolder", dirView);
        }

        [NonAction]
        private static IEnumerable<NavigationDirectoryViewModel> GetPath(File fileNode)
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
    }
}