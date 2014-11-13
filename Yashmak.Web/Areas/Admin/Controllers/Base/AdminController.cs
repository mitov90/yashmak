namespace Yashmak.Web.Areas.Admin.Controllers.Base
{
    using System.Web.Mvc;

    using Yashmak.Common;
    using Yashmak.Data;
    using Yashmak.Web.Controllers;

    //[Authorize(Roles = Constants.AdminRole)]
    public abstract class AdminController : BaseController
    {
        protected AdminController(IYashmakData data)
            : base(data)
        {
        }
    }
}