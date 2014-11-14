namespace Yashmak.Web.Controllers.Base
{
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Data.Models;

    public abstract class BaseController : Controller
    {
        protected BaseController(IYashmakData data)
        {
            this.Data = data;
        }

        public string UserId
        {
            get { return this.HttpContext.User.Identity.GetUserId(); }
        }

        protected IYashmakData Data { get; set; }

        protected AppUser CurrentUser
        {
            get { return this.Data.Users.GetById(this.UserId); }
        }
    }
}