namespace Yashmak.Web.Infrastructure.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Models;

    public class LogAttribute : ActionFilterAttribute
    {
        private readonly IYashmakDbContex dbContex;

        public LogAttribute(IYashmakDbContex dbContex)
        {
            this.dbContex = dbContex;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var dateTime = DateTime.Now;
            var ip = HttpContext.Current.Request.UserHostAddress;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var action = HttpContext.Current.Request.Url.ToString();

            var log = new Log { Action = action, DateTime = dateTime, UserId = userId, Ip = ip };

            this.dbContex.Logs.Add(log);
            this.dbContex.SaveChanges();

            base.OnActionExecuted(filterContext);
        }
    }
}