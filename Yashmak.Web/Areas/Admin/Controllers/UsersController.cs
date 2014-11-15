namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Collections;
    using System.Data.Entity;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Yashmak.Data;
    using Yashmak.Web.Areas.Admin.Controllers.Base;
    using Yashmak.Web.Areas.Admin.ViewModels.Users;

    public class UsersController : KendoGridAdministrationController
    {
        // GET: Admin/Users
        public UsersController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            return
                this.Data.Users.All()
                    .Include(u => u.Files)
                    .Include(u => u.Roles)
                    .Project()
                    .To<UserViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }
    }
}