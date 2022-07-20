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
using System.Web.Http.Description;
using NReco_HtmlToPdf.Helpers;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace NReco_HtmlToPdf.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : ApiController
    {
        private string _actionUrl;
        public HomeController()
        {
            _actionUrl = WebConfigurationManager.AppSettings["PDFGetPageNumberApplicationUrl"];
        }

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

        [ResponseType(typeof(PDFResponseModel))]
        [HttpPost]
        [Route("api/GetPdfFile")]
        public async Task<IHttpActionResult> GetPDFFile(DLVModel model)
        {
            PDFResponseModel pdfModel = new PDFResponseModel();
            try
            {
                var renderedView = RenderViewToString("home", model.Configurations != null ? model.Configurations.ViewName : "", model);
                var pdfBytes = ConvertHtmlToPdf(renderedView, model);
                var pdfString = Convert.ToBase64String(pdfBytes);
                //var result = new HttpResponseMessage(HttpStatusCode.OK)
                //{
                //    Content = new ByteArrayContent(pdfBytes)
                //};
                //result.Content.Headers.ContentDisposition =
                //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                //    {
                //        FileName = "DLV.pdf",
                //    };

                //result.Content.Headers.ContentType =
                //    new MediaTypeHeaderValue("application/pdf");

                //pdfModel.PDFFile = (HttpPostedFileBase)new HttpPostedFileBaseCustom(pdfBytes);
                pdfModel.PDFBase64String = pdfString;
                pdfModel.KeywordSearched = model.PDFOptions != null ? model.PDFOptions.Keyword : "";

                if (!string.IsNullOrEmpty(pdfModel.KeywordSearched))
                    pdfModel = await GetPDFPageNumber(pdfModel);

                return Ok(pdfModel);
            }
            catch(Exception ex)
            {
                pdfModel.StatusCode = HttpStatusCode.BadRequest;
                pdfModel.ErrorMessage = ex.Message;
                return Ok(pdfModel);
            }
        }

        [NonAction]
        public async Task<PDFResponseModel> GetPDFPageNumber(PDFResponseModel pdfDetails)
        {
            try
            {
                HttpContent keywordContent = new StringContent(pdfDetails.KeywordSearched);
                HttpContent pdfBase64Content = new StringContent(pdfDetails.PDFBase64String);
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(keywordContent, "Keyword");
                    formData.Add(pdfBase64Content, "PDFBase64String");
                    client.Timeout = TimeSpan.FromMinutes(1);
                    var response = await client.PostAsync(_actionUrl, formData);
                    if (!response.IsSuccessStatusCode)
                    {
                        pdfDetails.StatusCode = HttpStatusCode.Conflict;
                        pdfDetails.ErrorMessage = "Failed while getting page number";
                        pdfDetails.PageNumberOfKeyword = "0";
                        return pdfDetails;
                    }
                    pdfDetails.StatusCode = HttpStatusCode.OK;
                    pdfDetails.PageNumberOfKeyword = await response.Content.ReadAsStringAsync();
                }
                return pdfDetails;
            }
            catch(Exception ex)
            {
                pdfDetails.StatusCode = HttpStatusCode.BadRequest;
                pdfDetails.ErrorMessage = ex.Message;
                pdfDetails.PageNumberOfKeyword = "0";
                return pdfDetails;
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
                //model.Configurations.Margin = new Margin() { Bottom = "20", Top = "20", Left = "10", Right = "10" };
                html = html.Replace("{LogoImg}", $"src='{System.Web.Hosting.HostingEnvironment.MapPath("~/assets/images/checklistlogo.png")}'" );

                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                var footerHtmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/assets/CustomHTML/footer.html");
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