namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Collections;
    using System.IO;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Kendo.Mvc.UI;

    using Yashmak.Common;
    using Yashmak.Common.Assistants;
    using Yashmak.Data;
    using Yashmak.Web.Areas.Admin.Controllers.Base;

    using Model = Yashmak.Data.Models.File;
    using ViewModel = Yashmak.Web.Areas.Admin.ViewModels.Files.FileViewModel;

    public class FilesController : KendoGridAdministrationController
    {
        public FilesController(IYashmakData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, ViewModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                this.Data.Files.Delete(model.Id.Value);
                this.Data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }

        [HttpGet]
        public ActionResult DownloadFile(int id)
        {
            var file = this.Data.Files.GetById(id);

            var pathToFile =
                this.Server.MapPath(
                    "~" + Constants.UserFilesPath + file.User.UserName + "/" +
                    file.PathToFile);
            var stream = new FileStream(pathToFile, FileMode.Open);
            return this.File(
                stream, 
                MimeAssistant.GetMimeType(file.FileName), 
                file.FileName);
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            return this.Data.Files.All().Project().To<ViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Files.GetById(id) as T;
        }
    }
}