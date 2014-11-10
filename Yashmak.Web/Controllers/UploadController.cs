namespace Yashmak.Web.Controllers
{
    using System.Web.Mvc;

    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;

    public class UploadController : Controller
    {
        private IRepository<File> repository;

        public UploadController(IRepository<File> repository)
        {
            this.repository = repository;
        }

        public ActionResult FileUpload(int filenodeid)
        {
            throw new System.NotImplementedException();
        }
    }
}