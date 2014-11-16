namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Areas.Admin.Controllers.Base;
    using Yashmak.Web.Areas.Admin.ViewModels.Files;
    using Yashmak.Web.Areas.Admin.ViewModels.Messages;
    using Yashmak.Web.Areas.Admin.ViewModels.Users;

    using Constants = Yashmak.Common.Constants;

    public class UsersController : KendoGridAdministrationController
    {
        public UsersController(IYashmakData data)
            : base(data)
        {
        }

        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(MessageInputModel message)
        {
            if (this.ModelState.IsValid)
            {
                var receiver = this.Data.Users.GetById(message.ReceiverId);
                if (receiver != null)
                {
                    var newMessage = Mapper.Map<Message>(message);
                    this.Data.Messages.Add(newMessage);
                    this.Data.SaveChanges();
                    return this.Json("OK");
                }
            }

            return this.Json("Bad model");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(
            [DataSourceRequest] DataSourceRequest request, 
            [Bind(Prefix = "models")] IEnumerable<UserViewModel> users)
        {
            if (users != null && this.ModelState.IsValid)
            {
                var userManager =
                    new UserManager<AppUser>(new UserStore<AppUser>(this.Data.Context.DbContext));
                foreach (var user in users)
                {
                    if (user.Role.Name == Constants.AdminRole)
                    {
                        userManager.AddToRole(user.Id, Constants.AdminRole);
                    }
                    else
                    {
                        userManager.RemoveFromRole(user.Id, Constants.AdminRole);
                    }
                }

                this.Data.SaveChanges();
            }

            return this.Json(users.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Index()
        {
            this.ViewBag.Roles = new List<RoleViewModel>
                {
                    new RoleViewModel
                        {
                            Id = "1", 
                            Name = Constants.AdminRole
                        }, 
                    new RoleViewModel
                        {
                            Id = "2", 
                            Name = Constants.NonPaidUser
                        }
                };
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            var data =
                this.Data.Users.All()
                    .Include(u => u.Roles)
                    .Include(u => u.Files)
                    .Project()
                    .To<UserViewModel>();
            return data;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }
    }
}