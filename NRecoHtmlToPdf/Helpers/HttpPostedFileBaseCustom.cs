using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NReco_HtmlToPdf.Helpers
{
    public class HttpPostedFileBaseCustom : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;
        public HttpPostedFileBaseCustom(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
        }
        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }
        public override Stream InputStream { get; }
    }
}
