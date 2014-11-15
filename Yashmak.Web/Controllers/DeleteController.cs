namespace Yashmak.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using EntityFramework.Extensions;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Controllers.Base;
    using Yashmak.Web.ViewModels.File;

    public class DeleteController : FileBaseController
    {
        public DeleteController(IYashmakData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Delete(int? filenodeid)
        {
            var fileViewModel = Mapper.Map<FileViewModel>(this.GetFileNode(filenodeid));
            return this.PartialView(filenodeid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? filenodeid, string fileUrl)
        {
            var fileNode = this.GetFileNode(filenodeid);

            if (filenodeid == null || filenodeid == 0 || (fileNode.UserId != this.UserId))
            {
                return
                    this.Json(
                        "You're trying to delete things that do not exist or do not belong to you! ");
            }

            fileNode.IsDeleted = true;
            this.Data.Files.All()
                .Where(f => f.ParentId == filenodeid)
                .Update(f => new File { IsDeleted = true });

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Files", new { filenodeid = fileNode.ParentId });
        }
    }
}