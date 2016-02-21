using System.Collections;
using System.IO;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Kendo.Mvc.UI;
using Yashmak.Common;
using Yashmak.Common.Assistants;
using Yashmak.Data;
using Yashmak.Web.Areas.Admin.Controllers.Base;
using Yashmak.Web.Areas.Admin.ViewModels.Files;
using File = Yashmak.Data.Models.File;

namespace Yashmak.Web.Areas.Admin.Controllers
{
    using Model = File;
    using ViewModel = FileViewModel;

    public class FilesController : KendoGridAdministrationController
    {
        public FilesController(IYashmakData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, ViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                Data.Files.Delete(model.Id.Value);
                Data.SaveChanges();
            }

            return GridOperation(model, request);
        }

        [HttpGet]
        public ActionResult DownloadFile(int id)
        {
            var file = Data.Files.GetById(id);

            var pathToFile =
                Server.MapPath(
                    "~" + Constants.UserFilesPath + file.User.UserName + "/" +
                    file.PathToFile);
            var stream = new FileStream(pathToFile, FileMode.Open);
            return File(
                stream,
                MimeAssistant.GetMimeType(file.FileName),
                file.FileName);
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable GetData()
        {
            return Data.Files.AllWithDeleted().ProjectTo<ViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return Data.Files.GetById(id) as T;
        }
    }
}