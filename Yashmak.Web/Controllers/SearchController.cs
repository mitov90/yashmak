namespace Yashmak.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Web.Models.Directory;

    [Authorize]
    public class SearchController : Controller
    {
        private readonly IYashmakData repository;

        public SearchController(IYashmakData repository)
        {
            this.repository = repository;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Search(string query)
        {
            var userId = this.User.Identity.GetUserId();
            var queryable =
                this.repository.Files.All()
                    .Where(f => f.UserId == userId && f.FileName.Contains(query))
                    .Project()
                    .To<FileViewModel>();

            return this.View(queryable);
        }
    }
}