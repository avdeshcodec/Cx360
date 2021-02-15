using Newtonsoft.Json;
using IncidentManagement.API.Filter;
using IncidentManagement.Entities.Common;
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.Common;
using IncidentManagement.Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using iTextSharp.text.pdf;


using Ionic.Zip;
using iTextSharp.text;
using System.Configuration;
using shortid;

namespace IncidentManagement.API.Controllers
{
    [RoutePrefix("IncidentManagementAPI")]
    public class IncidentManagementAPIController : ApiController
    {
        #region Private
        private IIncidentManagementService _IIncidentManagementService = null;
        private System.Net.Http.HttpResponseMessage httpResponseMessage = null;
        private string connectionString = null;
        BaseResponse baseResponse = null;
        IncidentManagementGeneralRespnse incidentManagementGeneralRespnse = null;

        IncidentManagementTabsResponse incidentManagementTabsResponse = null;
        CommonFunctions common = null;
        AllPDFResponse allPDFResponse = null;
        AllPDFUploadResponse allPDFUploadResponse = null;
        #endregion
        public IncidentManagementAPIController(IIncidentManagementService IIncidentManagementService)
        {
            _IIncidentManagementService = IIncidentManagementService;
        }

        /// <summary>
        /// Get GetIncident Management
        /// </summary>
        /// <remarks>This API is used to get incident management general tab.</remarks>
        /// <param name="incidentManagementRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetIncidentManagement")]
        [ActionName("GetIncidentManagement")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetIncidentManagement(IncidentManagementRequest incidentManagementRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                incidentManagementGeneralRespnse = new IncidentManagementGeneralRespnse();
                if (ModelState.IsValid && incidentManagementRequest != null)
                {
                   

                    incidentManagementGeneralRespnse = await _IIncidentManagementService.GetIncidentManagement(incidentManagementRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                }

            }
            catch (Exception Ex)
            {
                incidentManagementGeneralRespnse.Success = false;
                incidentManagementGeneralRespnse.IsException = true;
                incidentManagementGeneralRespnse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
         /// <summary>
        /// Get GetIncident Management
        /// </summary>
        /// <remarks>This API is used to get incident management general tab.</remarks>
        /// <param name="incidentManagementRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteMasterRecord")]
        [ActionName("DeleteMasterRecord")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> DeleteMasterRecord(IncidentManagementRequest incidentManagementRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                incidentManagementGeneralRespnse = new IncidentManagementGeneralRespnse();
                if (ModelState.IsValid && incidentManagementRequest != null)
                {
                    

                    incidentManagementGeneralRespnse = await _IIncidentManagementService.DeleteMasterRecord(incidentManagementRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                }

            }
            catch (Exception Ex)
            {
                incidentManagementGeneralRespnse.Success = false;
                incidentManagementGeneralRespnse.IsException = true;
                incidentManagementGeneralRespnse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }  
        
        /// <summary>
        /// Get GetIncident Management
        /// </summary>
        /// <remarks>This API is used to get incident management general tab.</remarks>
        /// <param name="incidentManagementRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditAllRecord")]
        [ActionName("EditAllRecord")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> EditAllRecord(IncidentManagementRequest incidentManagementRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                incidentManagementGeneralRespnse = new IncidentManagementGeneralRespnse();
                if (ModelState.IsValid && incidentManagementRequest != null)
                {
                   

                    incidentManagementGeneralRespnse = await _IIncidentManagementService.EditAllRecord(incidentManagementRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                }

            }
            catch (Exception Ex)
            {
                incidentManagementGeneralRespnse.Success = false;
                incidentManagementGeneralRespnse.IsException = true;
                incidentManagementGeneralRespnse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API inserts modifies the tabs details based on data.</remarks>
        /// <param name="incidentManagementTabRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertModifyTabDetails")]
        [ActionName("InsertModifyTabDetails")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> InsertModifyTabDetails(IncidentManagementTabRequest incidentManagementTabRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                incidentManagementTabsResponse = new IncidentManagementTabsResponse();
                if (ModelState.IsValid && incidentManagementTabRequest != null)
                {

                    incidentManagementTabsResponse = await _IIncidentManagementService.InsertModifyTabDetails(incidentManagementTabRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementTabsResponse);
                }
            }
            catch (Exception Ex)
            {
                incidentManagementTabsResponse.Success = false;
                incidentManagementTabsResponse.IsException = true;
                incidentManagementTabsResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementTabsResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

     
        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API fills the state form pdf .</remarks>
        /// <param name="fillablePDFRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FillableStateFormPDF")]
        [ActionName("FillableStateFormPDF")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> FillableStateFormPDF(FillablePDFRequest fillablePDFRequest)
        {
            try
            {
                common = new CommonFunctions();
                httpResponseMessage = new HttpResponseMessage();
                allPDFResponse = new AllPDFResponse();
                if (ModelState.IsValid && fillablePDFRequest != null)
                {

                    
                    allPDFResponse = await _IIncidentManagementService.FillableStateFormPDF(fillablePDFRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, allPDFResponse);
                    Stream stream = CommonFunctions.GetFilesStream(allPDFResponse.AllPDF[0].FileName);
                    httpResponseMessage.Content = new StreamContent(stream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "fileNameOfYourChoice";
                }
            }
            catch (Exception Ex)
            {
                incidentManagementGeneralRespnse.Success = false;
                incidentManagementGeneralRespnse.IsException = true;
                incidentManagementGeneralRespnse.Message = Ex.Message;
                CommonFunctions.LogError(Ex);
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
            }
            return httpResponseMessage;
        }


        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API fills the state form pdf .</remarks>
        /// <param name="fillablePDFRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadPDFFiles")]
        [ActionName("UploadPDFFiles")]
        public async Task<HttpResponseMessage> UploadPDFFiles()
        {
            httpResponseMessage = new HttpResponseMessage();
            allPDFUploadResponse = new AllPDFUploadResponse();
            string fileName = string.Empty;
           
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    var uloadPath = ConfigurationManager.AppSettings["UploadPDF"];
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
                                string name = p.Value;
                                if (name.StartsWith("\"") && name.EndsWith("\"")) name = name.Substring(1, name.Length - 2);
                                string value = await file.ReadAsStringAsync();
                                attributes.Add(name, value);
                            }

                        }


                    }
                    attributes.Add("PDFDocument", uloadPath+fileName);
                    
                    var json=  JsonConvert.SerializeObject(attributes);
                    var pdfFile = JsonConvert.SerializeObject(files);
                    allPDFUploadResponse = await _IIncidentManagementService.UploadPDFFiles(json, pdfFile);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, allPDFUploadResponse);

                }
            }
            catch(Exception Ex)
            {
                incidentManagementGeneralRespnse.Success = false;
                incidentManagementGeneralRespnse.IsException = true;
                incidentManagementGeneralRespnse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;

        }

        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API downloads the uploaded pdf .</remarks>
        /// <param name="incidentManagementRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DownloadUPloadedFile")]
        [ActionName("DownloadUPloadedFile")]
        public async Task<HttpResponseMessage> DownloadUPloadedFile(IncidentManagementRequest incidentManagementRequest)
        {
            try
            {
                common = new CommonFunctions();
                httpResponseMessage = new HttpResponseMessage();
                allPDFUploadResponse = new AllPDFUploadResponse();
                if (ModelState.IsValid && incidentManagementRequest != null)
                {
                    allPDFUploadResponse = await _IIncidentManagementService.DownloadUPloadedFile(incidentManagementRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, allPDFUploadResponse);
                    Stream stream = CommonFunctions.GetFilesStream(allPDFUploadResponse.UploadedPDFResponse[0].PDFDocument);
                    httpResponseMessage.Content = new StreamContent(stream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName ="test.pdf"
                    };
                    httpResponseMessage.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "fileNameOfYourChoice.pdf";
                }
            }
            catch (Exception Ex)
            {
                incidentManagementGeneralRespnse.Success = false;
                incidentManagementGeneralRespnse.IsException = true;
                incidentManagementGeneralRespnse.Message = Ex.Message;
                CommonFunctions.LogError(Ex);
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, incidentManagementGeneralRespnse);
            }
            return httpResponseMessage;
        }

    }
}
