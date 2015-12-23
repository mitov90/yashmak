using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFileUploader.Models;
using Yashmak.Data;
using Yashmak.Data.Models;
using Yashmak.IO;
using Yashmak.Web.Controllers.Base;

namespace Yashmak.Web.Controllers
{
    [Authorize]
    public class UploadController : BaseController
    {
        public UploadController(IYashmakData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult FileUpload(int? filenodeid)
        {
            return PartialView(filenodeid);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult FileUpload(int? filenodeid, string test)
        {
            filenodeid = filenodeid == 0 ? null : filenodeid;

            var userFilesInCurrentDir = Data.Files
                .All().Where(d => d.ParentId == filenodeid && d.UserId == UserId);

            var statuses = new List<ViewDataUploadFileResult>();
            var provider = new AzureStorageProvider();

            for (var i = 0; i < Request.Files.Count; i++)
            {
                var curFileIndex = i;
                var curFile = Request.Files[curFileIndex];
                var curFileName = curFile.FileName;
                var saveFileName = Guid.NewGuid().ToString().Replace("-", string.Empty);


                var storedFile = new ViewDataUploadFileResult
                {
                    name = curFileName,
                    size = curFile.ContentLength,
                    type = curFile.ContentType,
                    Title = curFileName,
                    deleteType = "POST"
                };

                if (userFilesInCurrentDir.Any(f => f.FileName == curFileName))
                {
                    storedFile.error = "This name already exists in folder!";
                }
                else
                {
                    provider.UploadStream(UserId, saveFileName, curFile.InputStream);
                    var file = new File
                    {
                        FileName = curFileName,
                        IsDirectory = false,
                        ModifiedOn = DateTime.Now,
                        ParentId = filenodeid,
                        UserId = UserId,
                        Size = storedFile.size,
                        PathToFile = saveFileName
                    };
                    Data.Files.Add(file);
                    Data.SaveChanges();

                    storedFile.deleteUrl = Url.Action(
                        "DeleteFile",
                        new {filenodeid = file.Id});
                }

                statuses.Add(storedFile);
            }

            return Json(new {files = statuses});
        }

        [HttpPost]
        public ActionResult DeleteFile(int? filenodeid)
        {
            var file = Data.Files.GetById((int) filenodeid);
            if (file.UserId != UserId)
            {
                return HttpNotFound("File not found!");
            }

            Data.Files.Delete(file);
            Data.SaveChanges();

            return Json(new {error = string.Empty});
        }
    }
}