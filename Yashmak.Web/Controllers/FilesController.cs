namespace Yashmak.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;

    public class FilesController : Controller
    {
        private readonly IRepository<File> dbContext;

        public FilesController(IRepository<File> data)
        {
            this.dbContext = data;
        }

        // GET: Files
        public ActionResult Index()
        {
            var userId = this.HttpContext.User.Identity.GetUserId();
            var files = this.dbContext.All().Where(f => f.UserId == userId);
            return this.View(files);
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
            var files =
                this.dbContext.All()
                    .Where(f => f.UserId == userId && f.ParentId == fileId)
                    .Include(f => f.Parent)
                    .OrderByDescending(f => f.IsDirectory);

            return this.PartialView("_ViewFolder", files);
        }
    }
}