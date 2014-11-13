namespace Yashmak.Web.Areas.Admin.Controllers
{
    using Yashmak.Data;
    using Yashmak.Web.Controllers;

    // [Authorize(Roles = "Admin")]
    public abstract class AdminController : BaseController
    {
        protected AdminController(IYashmakData data)
            : base(data)
        {
        }
    }
}