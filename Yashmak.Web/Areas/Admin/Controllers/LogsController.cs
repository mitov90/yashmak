namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Yashmak.Data;
    using Yashmak.Web.Areas.Admin.Controllers.Base;

    public class LogsController : KendoGridAdministrationController
    {
        public LogsController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            return this.Data.Logs.All();
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Logs.GetById(id) as T;
        }
    }
}