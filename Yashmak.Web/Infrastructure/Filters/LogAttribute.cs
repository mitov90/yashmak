namespace Yashmak.Web.Infrastructure.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Ninject;

    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;

    public class LogAttribute : ActionFilterAttribute
    {
        [Inject]
        public IRepository<Log> Contex { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var dateTime = DateTime.Now;
            var ip = HttpContext.Current.Request.UserHostAddress;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var action = HttpContext.Current.Request.Url.LocalPath;

            var log = new Log() { Action = action, DateTime = dateTime, UserId = userId, Ip = ip };
            this.Contex.Add(log);
            this.Contex.SaveChanges();

            base.OnActionExecuted(filterContext);
        }
    }
}