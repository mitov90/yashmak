namespace Yashmak.Web.Infrastructure.ActionResults
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Ionic.Zip;

    public class ZipResult : ActionResult
    {
        private readonly IEnumerable<string> files;

        private string fileName;

        public ZipResult(params string[] files)
        {
            this.files = files;
        }

        public ZipResult(IEnumerable<string> files)
        {
            this.files = files;
        }

        public string FileName
        {
            get { return this.fileName ?? "files.zip"; }
            set { this.fileName = value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (var zf = new ZipFile())
            {
                zf.AddFiles(this.files, false, string.Empty);
                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader(
                    "content-disposition", 
                    "attachment; filename=" + this.FileName);
                zf.Save(context.HttpContext.Response.OutputStream);
            }
        }
    }
}