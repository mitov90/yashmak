using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Yashmak.Data;
using Yashmak.Web.Controllers.Base;
using Yashmak.Web.ViewModels.File;

namespace Yashmak.Web.Controllers
{
    public class RenameController : FileBaseController
    {
        public RenameController(IYashmakData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Rename(int? filenodeid)
        {
            object err;
            if (TempData.TryGetValue("Error", out err))
            {
                ModelState.AddModelError(string.Empty, err as string);
            }

            var fileNode =
                Data.Files.All()
                    .Where(f => f.Id == filenodeid)
                    .ProjectTo<FileViewModel>()
                    .FirstOrDefault();

            return View(fileNode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(FileViewModel fileModel)
        {
            var fileNode = GetFileNode(fileModel.Id);

            if (fileNode.UserId != UserId)
            {
                return HttpNotFound("File not found");
            }

            var existFileName =
                Data.Files.All()
                    .Any(f => f.ParentId == fileNode.ParentId && f.FileName == fileModel.FileName);

            if (existFileName)
            {
                TempData.Add("Error", "Such filename exist in this folder!");
                return RedirectToAction("Rename", "Rename", new {filenodeid = fileModel.Id});
            }

            fileNode.FileName = fileModel.FileName;
            Data.SaveChanges();

            return RedirectToAction("Index", "Files", new {filenodeid = fileNode.ParentId});
        }
    }
}