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
        public IYashmakData Contex { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.User != null)
            {
                var dateTime = DateTime.Now;
                var ip = HttpContext.Current.Request.UserHostAddress;
                var userId = HttpContext.Current.User.Identity.GetUserName();
                var action = HttpContext.Current.Request.RawUrl;
                var log = new Log { Action = action, DateTime = dateTime, Username = userId, Ip = ip };
                this.Contex.Logs.Add(log);
                this.Contex.SaveChanges();
            }

            base.OnActionExecuted(filterContext);
        }
    }
}