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
using Yashmak.Web.Areas.Admin.ViewModels.Messages;
using Yashmak.Web.Areas.Admin.ViewModels.Users;
using Constants = Yashmak.Common.Constants;

namespace Yashmak.Web.Areas.Admin.Controllers
{
    public class UsersController : KendoGridAdministrationController
    {
        public UsersController(IYashmakData data)
            : base(data)
        {
        }

        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(MessageInputModel message)
        {
            if (ModelState.IsValid)
            {
                var receiver = Data.Users.GetById(message.ReceiverId);
                if (receiver != null)
                {
                    var newMessage = Mapper.Map<Message>(message);
                    Data.Messages.Add(newMessage);
                    Data.SaveChanges();
                    return Json("OK");
                }
            }

            return Json("Bad model");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(
            [DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<UserViewModel> users)
        {
            if (users != null && ModelState.IsValid)
            {
                var userManager =
                    new UserManager<AppUser>(new UserStore<AppUser>(Data.Context.DbContext));
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

                Data.SaveChanges();
            }

            return Json(users.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Index()
        {
            ViewBag.Roles = new List<RoleViewModel>
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
            return View();
        }

        protected override IEnumerable GetData()
        {
            var data =
                Data.Users.All()
                    .Include(u => u.Roles)
                    .Include(u => u.Files)
                    .ProjectTo<UserViewModel>();
            return data;
        }

        protected override T GetById<T>(object id)
        {
            return Data.Users.GetById(id) as T;
        }
    }
}