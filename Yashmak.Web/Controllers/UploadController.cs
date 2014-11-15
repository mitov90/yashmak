namespace Yashmak.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

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
            
            var userFilesInCurrentDir =
                this.Data.Files.All().Where(d => d.ParentId == filenodeid && d.UserId == UserId);

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
                            x.StorageDirectory = savePath;
                            x.FileName = saveFileName;
                            x.ThrowExceptions = true;

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
                            UserId = UserId, 
                            Size = st.size, 
                            PathToFile = st.SavedFileName
                        };
                    this.Data.Files.Add(file);
                    this.Data.SaveChanges();
                    
                    st.deleteUrl = this.Url.Action(
                        "DeleteFile", 
                        new {filenodeid = file.Id, filePath = st.FullPath });
                    
                }

                statuses.Add(st);
            }

            var viewresult = this.Json(new { files = statuses });

            return viewresult;
        }

        [HttpPost]
        public ActionResult DeleteFile(int? filenodeid, string filePath)
        {
            var file = this.Data.Files.GetById((int)filenodeid);
            if (file.UserId != this.UserId)
            {
                return this.HttpNotFound("File not found!");
            }

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            this.Data.Files.Delete(file);

            var viewresult = this.Json(new { error = string.Empty });

            return viewresult; 
        }
    }
}