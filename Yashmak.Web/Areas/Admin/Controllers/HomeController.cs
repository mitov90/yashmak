namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using Yashmak.Data;
    using Yashmak.Web.Areas.Admin.Controllers.Base;

    public class HomeController : AdminController
    {
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