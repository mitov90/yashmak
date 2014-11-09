namespace Yashmak.Web.Controllers
{
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
    }
}