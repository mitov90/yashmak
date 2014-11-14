namespace Yashmak.Web.Controllers
{
    using System.Web.Mvc;

    using Yashmak.Common;
    using Yashmak.Data;
    using Yashmak.Web.Controllers.Base;

    public class HomeController : BaseController
    {
        public HomeController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult About()
        {
            this.ViewBag.Message = Constants.Moto; 

            return this.View();
        }
    }
}