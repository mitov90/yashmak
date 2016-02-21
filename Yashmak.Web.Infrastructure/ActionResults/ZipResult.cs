namespace Yashmak.Web.Infrastructure.ActionResults
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;

    using Ionic.Zip;

    public class ZipResult : ActionResult
    {
        private readonly IEnumerable<Tuple<string, Stream>> files;

        private string fileName;

        public ZipResult(params Tuple<string, Stream>[] files)
        {
            this.files = files;
        }

        public ZipResult(IEnumerable<Tuple<string, Stream>> files)
        {
            this.files = files;
        }

        public string FileName
        {
            get
            {
                return this.fileName ?? "files.zip";
            }

            set
            {
                this.fileName = value;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (var zf = new ZipFile())
            {
                foreach (var file in this.files)
                {
                    zf.AddEntry(file.Item1, file.Item2);
                }

                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader(
                    "content-disposition", 
                    "attachment; filename=" + this.FileName);
                zf.Save(context.HttpContext.Response.OutputStream);
            }
        }
    }
}