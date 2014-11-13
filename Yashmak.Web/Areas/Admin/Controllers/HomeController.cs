namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Navigation()
        {
            return this.View();
        }
    }
}