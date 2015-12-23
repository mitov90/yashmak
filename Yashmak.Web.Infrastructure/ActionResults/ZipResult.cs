using System;
using System.IO;

namespace Yashmak.Web.Infrastructure.ActionResults
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Ionic.Zip;

    public class ZipResult : ActionResult
    {
        private readonly IEnumerable<Tuple<string, Stream>> _files;

        private string _fileName;

        public ZipResult(params Tuple<string, Stream>[] files)
        {
            this._files = files;
        }

        public ZipResult(IEnumerable<Tuple<string, Stream>> files)
        {
            this._files = files;
        }

        public string FileName
        {
            get { return this._fileName ?? "files.zip"; }
            set { this._fileName = value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (var zf = new ZipFile())
            {
                foreach (var file in this._files)
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