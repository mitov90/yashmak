namespace Yashmak.Web.Controllers.Base
{
    using System.Collections.Generic;
    using System.Linq;
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

        public string UserId => this.HttpContext.User.Identity.GetUserId();

        protected IYashmakData Data { get; set; }

        protected AppUser CurrentUser => this.Data.Users.GetById(this.UserId);

        protected List<Message> GetUserMessages()
        {
            var messages = new List<Message>();

            if (this.UserId == null)
                return messages;

            messages = this.Data.Users
                .GetById(this.UserId)
                .Messages
                .Where(m => !m.IsSeen)
                .ToList();

            foreach (var message in messages)
            {
                message.IsSeen = true;
            }

            this.Data.SaveChanges();

            return messages;
        }
    }
}