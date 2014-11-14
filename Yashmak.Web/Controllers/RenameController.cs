namespace Yashmak.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Yashmak.Data;
    using Yashmak.Web.Controllers.Base;
    using Yashmak.Web.ViewModels.File;

    public class RenameController : FileBaseController
    {
        public RenameController(IYashmakData data)
            : base(data)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(FileViewModel fileModel)
        {
            var fileNode = this.GetFileNode(fileModel.Id);

            if (fileNode.UserId != this.UserId)
            {
                return this.HttpNotFound("File not found");
            }

            var existFileName =
                this.Data.Files.All()
                    .Any(f => f.ParentId == fileNode.ParentId && f.FileName == fileModel.FileName);

            if (existFileName)
            {
                this.ModelState.AddModelError(string.Empty, "Such filename exist in this folder!");
                return this.View(fileModel);
            }

            fileNode.FileName = fileModel.FileName;
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Files", new { filenodeid = fileNode.ParentId });
        }

        public ActionResult Rename(int? filenodeid)
        {
            var fileNode =
                this.Data.Files.All()
                    .Where(f => f.Id == filenodeid)
                    .Project()
                    .To<FileViewModel>()
                    .FirstOrDefault();

            return this.View(fileNode);
        }
    }
}