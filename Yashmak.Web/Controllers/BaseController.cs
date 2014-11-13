namespace Yashmak.Web.Controllers
{
    using System.Web.Mvc;

    using Yashmak.Data;
    using Yashmak.Data.Models;

    public abstract class BaseController : Controller
    {
        protected BaseController(IYashmakData data)
        {
            this.Data = data;
        }

        protected IYashmakData Data { get; set; }

        protected AppUser CurrentUser { get; set; }
    }
}