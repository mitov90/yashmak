namespace Yashmak.Web.Infrastructure.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;

    public class LogAttribute : ActionFilterAttribute
    {
        private IYashmakDbContex dbContex;

        public LogAttribute(IYashmakDbContex dbContex)
        {
            this.dbContex = dbContex;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var dateTime = DateTime.Now;
            var ip = HttpContext.Current.Request.UserHostAddress;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var url = HttpContext.Current.Request.Url.ToString();
            base.OnActionExecuted(filterContext);
        }
    }
}