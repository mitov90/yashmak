namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using Yashmak.Data;

    public class HomeController : AdminController
    {
        // GET: Admin/Home
        public HomeController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Navigation()
        {
            return this.View();
        }
    }
}