namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            return this.View();
        }
    }
}