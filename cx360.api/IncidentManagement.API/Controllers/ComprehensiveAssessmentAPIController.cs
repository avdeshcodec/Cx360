using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IncidentManagement.Entities.Common;
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Service.IService;
using IncidentManagement.API.Filter;
using static IncidentManagement.Entities.Response.ComprehensiveAssessmentResponse;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json;
using static IncidentManagement.Entities.Response.CCOComprehensiveAssessmentResponse;

namespace IncidentManagement.API.Controllers
{
    [RoutePrefix("ComprehensiveAssessmentAPI")]
    public class ComprehensiveAssessmentAPIController : ApiController
    {
        #region Private
        private System.Net.Http.HttpResponseMessage httpResponseMessage = null;
        private IComprehensiveAssessmentService _ComprehensiveAssessmentService = null;
        private ComprehensiveAssessmentDetailResponse cadResponse = null;
        private CCOComprehensiveAssessmentDetailResponse ccoResponse = null;
        private ComprehensiveAssessmentPDFResponse comprehensiveAssessmentPDFResponse = null;
        CommonFunctions common = null;
        private readonly DocumentUpload _documentUpload;
        private string companyId = null;

        #endregion

        public ComprehensiveAssessmentAPIController(IComprehensiveAssessmentService iComprehensiveAssessmentService)
        {
            _ComprehensiveAssessmentService = iComprehensiveAssessmentService;
            _documentUpload = new DocumentUpload();
        }

        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API inserts modifies the tabs details based on data.</remarks>
        /// <param name="comprehensiveAssessmentRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertModifyComprehensiveAssessmentDetail")]
        [ActionName("InsertModifyComprehensiveAssessmentDetail")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cadResponse = new ComprehensiveAssessmentDetailResponse();

                if (ModelState.IsValid && comprehensiveAssessmentRequest != null)
                {
                    if (Request.Headers.Contains("Source"))
                    {
                        companyId = Request.Headers.GetValues("Source").First();
                    }
                    cadResponse = await _ComprehensiveAssessmentService.InsertModifyComprehensiveAssessmentDetail(comprehensiveAssessmentRequest, companyId);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                }
            }
            catch (Exception Ex)
            {
                cadResponse.Success = false;
                cadResponse.IsException = true;
                cadResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
        [HttpPost]
        [Route("HandleAssessmentVersioning")]
        [ActionName("HandleAssessmentVersioning")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cadResponse = new ComprehensiveAssessmentDetailResponse();
                if (ModelState.IsValid && comprehensiveAssessmentRequest != null)
                {
                    if (Request.Headers.Contains("Source"))
                    {
                        companyId = Request.Headers.GetValues("Source").First();
                    }
                    cadResponse = await _ComprehensiveAssessmentService.HandleAssessmentVersioning(comprehensiveAssessmentRequest, companyId);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                }

            }
            catch (Exception Ex)
            {
                cadResponse.Success = false;
                cadResponse.IsException = true;
                cadResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Get Comprehensive Assessment
        /// </summary>
        /// <remarks>This API is used to Comprehensive Assessment.</remarks>
        /// <param name="comprehensiveAssessmentRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetComprehensiveAssessmentDetail")]
        [ActionName("GetComprehensiveAssessmentDetail")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cadResponse = new ComprehensiveAssessmentDetailResponse();
                if (ModelState.IsValid && comprehensiveAssessmentRequest != null)
                {

                    if (Request.Headers.Contains("Source"))
                    {
                        companyId = Request.Headers.GetValues("Source").First();
                    }
                    cadResponse = await _ComprehensiveAssessmentService.GetComprehensiveAssessmentDetail(comprehensiveAssessmentRequest, companyId);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                }

            }
            catch (Exception Ex)
            {
                cadResponse.Success = false;
                cadResponse.IsException = true;
                cadResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }


        /// <summary>
        /// Get Comprehensive Assessment for pdf printing
        /// </summary>
        /// <remarks>This API is used to  get Comprehensive Assessment.</remarks>
        /// <param name="comprehensiveAssessmentRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PrintAssessmentPDF")]
        [ActionName("PrintAssessmentPDF")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                comprehensiveAssessmentPDFResponse = new ComprehensiveAssessmentPDFResponse();
                if (ModelState.IsValid && comprehensiveAssessmentRequest != null)
                {
                    if (Request.Headers.Contains("Source"))
                    {
                        companyId = Request.Headers.GetValues("Source").First();
                    }
                    comprehensiveAssessmentPDFResponse = await _ComprehensiveAssessmentService.PrintAssessmentPDF(comprehensiveAssessmentRequest, companyId);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, comprehensiveAssessmentPDFResponse);
                    Stream stream = CommonFunctions.GetFilesStream(comprehensiveAssessmentPDFResponse.ComprehensiveAssessmentPDF[0].FileName);
                    httpResponseMessage.Content = new StreamContent(stream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "fileNameOfYourChoice";
                }

            }
            catch (Exception Ex)
            {
                cadResponse.Success = false;
                cadResponse.IsException = true;
                cadResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
        [HttpPost]
        [Route("UploadOfflinePDF")]
        [ActionName("UploadOfflinePDF")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> UploadOfflinePDF()
        {
            try
            {
                if (Request.Headers.Contains("Source"))
                {
                    companyId = Request.Headers.GetValues("Source").First();
                }

                httpResponseMessage = new HttpResponseMessage();
                comprehensiveAssessmentPDFResponse = new ComprehensiveAssessmentPDFResponse();
                string fileName = string.Empty;

                if (Request.Content.IsMimeMultipartContent())
                {
                    var uloadPath = ConfigurationManager.AppSettings["UploadOfflinePDF"];
                    int iUploadedCnt = 0;
                    System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                    // CHECK THE FILE COUNT.
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf.ContentLength > 0)
                        {
                            int lastIndex = hpf.FileName.LastIndexOf(".");
                            fileName = hpf.FileName.Substring(0, lastIndex);
                            DateTime currentUTC = DateTime.UtcNow;
                            string strTemp = currentUTC.ToString("yyyyMMddHHmmss");
                            fileName = fileName + "_" + strTemp + ".pdf";
                            // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                            if (!File.Exists(uloadPath + Path.GetFileName(fileName)))
                            {

                                // SAVE THE FILES IN THE FOLDER.
                                hpf.SaveAs(uloadPath + Path.GetFileName(fileName));

                            }
                        }
                    }

                    Dictionary<string, string> attributes = new Dictionary<string, string>();
                    Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
                    var provider = new MultipartMemoryStreamProvider();
                    await Request.Content.ReadAsMultipartAsync(provider);

                    foreach (var file in provider.Contents)
                    {

                        if (file.Headers.ContentDisposition.FileName == null)
                        {
                            foreach (NameValueHeaderValue p in file.Headers.ContentDisposition.Parameters)
                            {
                                string clientId = p.Value;
                                if (clientId.StartsWith("\"") && clientId.EndsWith("\"")) clientId = clientId.Substring(1, clientId.Length - 2);
                                string value = await file.ReadAsStringAsync();
                                attributes.Add(clientId, value);
                            }

                        }


                    }
                    attributes.Add("PDFDocument", uloadPath + fileName);

                    var json = JsonConvert.SerializeObject(attributes);
                    var pdfFile = JsonConvert.SerializeObject(files);
                    var data = await _ComprehensiveAssessmentService.UploadOfflinePDF(json, pdfFile, companyId);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, data);

                }
            }

            catch (Exception Ex)
            {
                cadResponse.Success = false;
                cadResponse.IsException = true;
                cadResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }


        /// <summary>
        /// Get Comprehensive Assessment
        /// </summary>
        /// <remarks>This API is used to Comprehensive Assessment.</remarks>
        /// <param name="comprehensiveAssessmentRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCCOComprehensiveAssessmentDetail")]
        [ActionName("GetCCOComprehensiveAssessmentDetail")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetCCOComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cadResponse = new ComprehensiveAssessmentDetailResponse();
                if (ModelState.IsValid && comprehensiveAssessmentRequest != null)
                {
                    ccoResponse = await _ComprehensiveAssessmentService.GetCCOComprehensiveAssessmentDetail(comprehensiveAssessmentRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                }

            }
            catch (Exception Ex)
            {
                cadResponse.Success = false;
                cadResponse.IsException = true;
                cadResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cadResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }


    }
}

