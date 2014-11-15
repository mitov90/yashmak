namespace Yashmak.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Yashmak.Data;
    using Yashmak.Web.Controllers.Base;
    using Yashmak.Web.ViewModels.File;

    [Authorize]
    public class SearchController : BaseController
    {
        public SearchController(IYashmakData data)
            : base(data)
        {
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Search(string query)
        {
            var queryable =
                this.Data.Files.All()
                    .Where(f => f.UserId == UserId && f.FileName.Contains(query))
                    .Project()
                    .To<FileViewModel>();

            return this.View(queryable);
        }
    }
}