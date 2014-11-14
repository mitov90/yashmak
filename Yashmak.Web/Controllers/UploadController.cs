namespace Yashmak.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using MvcFileUploader;
    using MvcFileUploader.Models;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Controllers.Base;

    using Constants = Yashmak.Common.Constants;

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
            return this.PartialView(filenodeid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileUpload(int? filenodeid, string test)
        {
            filenodeid = filenodeid == 0 ? null : filenodeid;
            var username = this.User.Identity.Name;
            var savePath =
                this.Server.MapPath(string.Format(@"~{0}{1}/", Constants.UserFilesPath, username));
            var userId = this.User.Identity.GetUserId();
            var userFilesInCurrentDir =
                this.Data.Files.All().Where(d => d.ParentId == filenodeid && d.UserId == userId);

            var statuses = new List<ViewDataUploadFileResult>();

            for (var i = 0; i < this.Request.Files.Count; i++)
            {
                var curFile = i;
                var curFileName = this.Request.Files[curFile].FileName;
                var saveFileName = Guid.NewGuid().ToString();
                var st = FileSaver.StoreFile(
                    x =>
                        {
                            x.File = this.Request.Files[curFile];

                            // note how we are adding an additional value to be posted with delete request
                            // and giving it the same value posted with upload
                            x.StorageDirectory = savePath;

                            // TODO: CHange the download url
                            x.UrlPrefix = string.Format("{0}{1}", Constants.UserFilesPath, username);

                            // overriding defaults
                            x.FileName = saveFileName;

                            // default is filename suffixed with filetimestamp
                            x.ThrowExceptions = true;

                            // default is false, if false exception message is set in error property
                        });

                if (userFilesInCurrentDir.Any(f => f.FileName == curFileName))
                {
                    st.error = "This name alredy exists in folder!";
                    if (System.IO.File.Exists(st.FullPath))
                    {
                        System.IO.File.Delete(st.FullPath);
                    }
                }
                else
                {
                    var file = new File
                        {
                            FileName = curFileName, 
                            IsDirectory = false, 
                            ModifiedOn = DateTime.Now, 
                            ParentId = filenodeid, 
                            UserId = userId, 
                            Size = st.size, 
                            PathToFile = st.SavedFileName
                        };
                    this.Data.Files.Add(file);
                    this.Data.SaveChanges();
                    
                    st.deleteUrl = this.Url.Action(
                        "DeleteFile", 
                        new { filenodeid = file.Id, filePath = st.FullPath });
                }

                statuses.Add(st);
            }

            // TODO: fix downloading and thumbnail
            // statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80");
            // statuses.ForEach(x => x.url = this.Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));
            var viewresult = this.Json(new { files = statuses });

            return viewresult;
        }

        // here i am receving the extra info injected
        [HttpPost] // should accept only post
        public ActionResult DeleteFile(int? filenodeid, string filePath)
        {
            var file = this.Data.Files.GetById((int)filenodeid);
            if (file.UserId != this.UserId)
            {
                return
                    this.Json(new { error = "Don't try delete files that do not belongs to you!" });
            }

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            this.Data.Files.Delete(file);

            var viewresult = this.Json(new { error = string.Empty });

            return viewresult; // trigger success
        }

        public ActionResult DownloadFile(string fileUrl, string mimetype)
        {
            var filePath = this.Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
            {
                return this.File(filePath, mimetype);
            }

            return new HttpNotFoundResult("File not found");
        }
    }
}