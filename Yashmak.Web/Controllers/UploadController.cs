namespace Yashmak.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using MvcFileUploader;
    using MvcFileUploader.Models;

    using Yashmak.Data.Common.Repository;
    using Yashmak.Data.Models;

    public class UploadController : Controller
    {
        private readonly IRepository<File> repository;

        public UploadController(IRepository<File> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult FileUpload(int? filenodeid)
        {
            return this.PartialView(filenodeid);
        }

        [HttpPost]
        public ActionResult FileUpload(int? filenodeid, string test)
        {
            filenodeid = filenodeid == 0 ? null : filenodeid;
            var username = this.User.Identity.Name;
            var savePath = this.Server.MapPath(string.Format(@"~/App_Data/{0}/", username));
            var userId = this.User.Identity.GetUserId();
            var userFilesInCurrentDir = this.repository.All().Where(d => d.ParentId == filenodeid && d.UserId == userId);

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
                            x.DeleteUrl = this.Url.Action("DeleteFile", new { filenodeid = filenodeid });
                            x.StorageDirectory = savePath;
                            x.UrlPrefix = string.Format("/App_Data/{0}", username);
                                
                                // this is used to generate the relative url of the file

                            // overriding defaults
                            x.FileName = saveFileName;

                            // default is filename suffixed with filetimestamp
                            x.ThrowExceptions = true;

                            // default is false, if false exception message is set in error property
                        });

                if (userFilesInCurrentDir.Any(f => f.FileName == curFileName))
                {
                    st.error = "This name alredy exists in folder!";
                }
                else
                {
                    this.repository.Add(
                        new File
                            {
                                FileName = curFileName, 
                                IsDirectory = false, 
                                ModifiedOn = DateTime.Now, 
                                ParentId = filenodeid, 
                                UserId = userId, 
                                Size = st.size, 
                                PathToFile = st.SavedFileName
                            });
                    this.repository.SaveChanges();
                }

                statuses.Add(st);
            }

            // statuses.ForEach(x => x.thumbnailUrl = x.url + "?width=80&height=80");
            // statuses.ForEach(x => x.url = this.Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));
            if (filenodeid == 13)
            {
                var rnd = new Random();
                statuses.ForEach(
                    x =>
                        {
                            // setting the error property removes the deleteUrl, thumbnailUrl and url property values
                            x.error = rnd.Next(0, 2) > 0
                                          ? "We do not have any entity with unlucky Id : '13'"
                                          : string.Format("Your file size is {0} bytes which is un-acceptable", x.size);

                            // delete file by using FullPath property
                            if (System.IO.File.Exists(x.FullPath))
                            {
                                System.IO.File.Delete(x.FullPath);
                            }
                        });
            }

            var viewresult = this.Json(new { files = statuses });

            // for IE8 which does not accept application/json
            if (this.Request.Headers["Accept"] != null && !this.Request.Headers["Accept"].Contains("application/json"))
            {
                viewresult.ContentType = "text/plain";
            }

            return viewresult;
        }

        // here i am receving the extra info injected
        [HttpPost] // should accept only post
        public ActionResult DeleteFile(int? filenodeid, string fileUrl)
        {
            var filePath = this.Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var viewresult = this.Json(new { error = string.Empty });

            // for IE8 which does not accept application/json
            if (this.Request.Headers["Accept"] != null && !this.Request.Headers["Accept"].Contains("application/json"))
            {
                viewresult.ContentType = "text/plain";
            }

            return viewresult; // trigger success
        }

        public ActionResult DownloadFile(string fileUrl, string mimetype)
        {
            var filePath = this.Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, mimetype);
            }

            return new HttpNotFoundResult("File not found");
        }
    }
}