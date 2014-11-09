namespace Yashmak.Web.Infrastructure.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Ninject;

    using Yashmak.Data;
    using Yashmak.Data.Models;

    public class LogAttribute : ActionFilterAttribute
    {
        [Inject]
        public IYashmakDbContex Contex { get; set; }

        // public LogAttribute(IYashmakDbContex dbContex)
        // {
        // this.dbContex = dbContex;
        // }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var dateTime = DateTime.Now;
            var ip = HttpContext.Current.Request.UserHostAddress;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var action = HttpContext.Current.Request.Url.LocalPath;

            var log = new Log() { Action = action, DateTime = dateTime, UserId = userId, Ip = ip };
            this.Contex.Logs.Add(log);
            this.Contex.SaveChanges();

            base.OnActionExecuted(filterContext);
        }
    }
}