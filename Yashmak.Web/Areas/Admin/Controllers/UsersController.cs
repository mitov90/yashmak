namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Yashmak.Data;
    using Yashmak.Web.Areas.Admin.Controllers.Base;

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
            throw new System.NotImplementedException();
        }

        protected override T GetById<T>(object id)
        {
            throw new System.NotImplementedException();
        }
    }
}