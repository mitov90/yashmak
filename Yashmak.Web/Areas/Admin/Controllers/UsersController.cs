namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Mvc;
    using System.Web.Security;

    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Areas.Admin.Controllers.Base;
    using Yashmak.Web.Areas.Admin.ViewModels.Users;

    using Constants = Yashmak.Common.Constants;

    public class UsersController : KendoGridAdministrationController
    {
        public UsersController(IYashmakData data)
            : base(data)
        {
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(
            [DataSourceRequest] DataSourceRequest request, 
            [Bind(Prefix = "models")] IEnumerable<UserViewModel> users)
        {
            if (users != null && this.ModelState.IsValid)
            {
                var userManager = new UserManager<AppUser>(new UserStore<AppUser>(this.Data.Context.DbContext));
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

        protected override IEnumerable GetData()
        {
            var user = this.Data.Users.All()
                           .Include(u => u.Files)
                           .Include(u => u.Roles)
                           .Project()
                           .To<UserViewModel>();
            return user;
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }

        public void ChangeAdminState(UserViewModel user)
        {
        }
    }
}