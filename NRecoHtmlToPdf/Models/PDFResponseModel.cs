using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace NReco_HtmlToPdf.Models
{
    public class PDFResponseModel : ResponseModel
    {
        public HttpPostedFileBase PDFFile { get; set; }
        public string PDFBase64String { get; set; }
        public int TotalPages { get; set; }
        public string KeywordSearched { get; set; }
        public string PageNumberOfKeyword { get; set; }
    }

    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
    }
}