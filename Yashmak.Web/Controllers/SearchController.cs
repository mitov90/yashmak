namespace Yashmak.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;
    using Yashmak.Web.Models.Directory;

    public class SearchController : Controller
    {
        private readonly IDeletableEntityRepository<File> repository;

        public SearchController(IDeletableEntityRepository<File> repository)
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
                this.repository.All()
                    .Where(f => f.UserId == userId && f.FileName.Contains(query))
                    .Project()
                    .To<FileViewModel>();

            return this.View(queryable);
        }
    }
}