namespace Yashmak.Web.Controllers
{
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Data.Models;

    public class FilesController : Controller
    {
        private readonly IYashmakData dbContext;

        public FilesController(IYashmakData data)
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
                file.UserId = User.Identity.GetUserId();
                this.dbContext.Files.Add(file);
                this.dbContext.SaveChanges();
            }

            return this.View(file);
        }
    }
}