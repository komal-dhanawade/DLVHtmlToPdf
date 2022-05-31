using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NReco_HtmlToPdf.Models;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Web.Routing;
using System.Net.Http.Headers;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using NonActionAttribute = System.Web.Http.NonActionAttribute;
using System.Web.Http.Cors;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace NReco_HtmlToPdf.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : ApiController
    {
        // GET: Home
        [HttpGet]
        [Route("api/CheckResponse")]
        public HttpResponseMessage Index()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("api/GetPdfString")]
        public HttpResponseMessage GetPDFString(DLVModel model)
        {
            try
            {

                //< link rel =\"stylesheet\" href=\"" + Server.MapPath("~/assets/css/bootstrap.min.css") + "\" />
                //var path = Server.MapPath("~/assets/css/bootstrap.css");
                //ControllerContext.Request;
                //var newPath = .HttpContext.Server.MapPath("~/expense.component.html");
                //var viewPath = "~/Views/Home/DLV.cshtml";
                //return File(pdfFile, contentType);
                var renderedView = RenderViewToString("home", model.Configurations != null ? model.Configurations.ViewName : "" , model);
                var pdfBytes = ConvertHtmlToPdf(renderedView, model);
                var pdfString = Convert.ToBase64String(pdfBytes);
                return Request.CreateResponse(HttpStatusCode.OK, pdfString);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
        [HttpPost]
        [Route("api/GetPdfFile")]
        public HttpResponseMessage GetPDFFile(DLVModel model)
        {
            try
            {
                var renderedView = RenderViewToString("home", model.Configurations != null ? model.Configurations.ViewName : "", model);
                var pdfBytes = ConvertHtmlToPdf(renderedView, model);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                };
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "DLV.pdf",
                    };

                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/pdf");

                return result;
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [NonAction]
        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            var context = HttpContext.Current;
            var contextBase = new HttpContextWrapper(context);
            var routeData = new RouteData();
            routeData.Values.Add("controller", controllerName);

            var controllerContext = new ControllerContext(contextBase,
                                                         routeData,
                                                         new EmptyController());

            var razorViewEngine = new RazorViewEngine();
            var razorViewResult = razorViewEngine.FindView(controllerContext,
                                                            viewName,
                                                            "",
                                                            false);

            var writer = new StringWriter();
            var viewContext = new ViewContext(controllerContext,
                                             razorViewResult.View,
                                             new ViewDataDictionary(viewData),
                                             new TempDataDictionary(),
                                             writer);
            razorViewResult.View.Render(viewContext, writer);
            return writer.ToString();
        }

        [NonAction]
        public byte[] ConvertHtmlToPdf(string html, DLVModel model)
        {
            try
            {
                model.Configurations.Margin = new Margin() { Bottom = "20", Top = "20", Left = "10", Right = "10" };
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                var footerHtmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/CustomHTML/footer.html");
                //htmlToPdf.PageFooterHtml = "<span style='background-color:yellow;'> Page [page] </span>";
                htmlToPdf.CustomWkHtmlArgs = "--enable-local-file-access ";
                //htmlToPdf.CustomWkHtmlArgs += ("--footer-center [page]");
                htmlToPdf.CustomWkHtmlArgs += ("--footer-html " + footerHtmlPath);

                if(model.Configurations.Margin != null && !string.IsNullOrEmpty(model.Configurations.Margin.Bottom))
                    htmlToPdf.CustomWkHtmlArgs += (" --margin-bottom " +   model.Configurations.Margin.Bottom);

                if (model.Configurations.Margin != null && !string.IsNullOrEmpty(model.Configurations.Margin.Top))
                    htmlToPdf.CustomWkHtmlArgs += (" --margin-top " + model.Configurations.Margin.Top);

                if (model.Configurations.Margin != null && !string.IsNullOrEmpty(model.Configurations.Margin.Left))
                    htmlToPdf.CustomWkHtmlArgs += (" --margin-left " + model.Configurations.Margin.Left);

                if (model.Configurations.Margin != null && !string.IsNullOrEmpty(model.Configurations.Margin.Right))
                    htmlToPdf.CustomWkHtmlArgs += (" --margin-right " + model.Configurations.Margin.Right);

                htmlToPdf.Quiet = false;
                htmlToPdf.LogReceived += (sender, e) => {
                    System.Diagnostics.Debug.WriteLine("WkHtmlToPdf Log: {0}", e.Data);
                };
                var pdfBytes = htmlToPdf.GeneratePdf(html);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

    }
    class EmptyController : ControllerBase
    {
        protected override void ExecuteCore() { }
    }
}