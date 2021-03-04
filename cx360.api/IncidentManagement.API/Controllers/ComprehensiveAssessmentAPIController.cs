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

namespace IncidentManagement.API.Controllers
{
    [RoutePrefix("ComprehensiveAssessmentAPI")]
    public class ComprehensiveAssessmentAPIController : ApiController
    {
        #region Private
        private System.Net.Http.HttpResponseMessage httpResponseMessage = null;
        private IComprehensiveAssessmentService _ComprehensiveAssessmentService = null;
        private ComprehensiveAssessmentDetailResponse cadResponse = null;
        private ComprehensiveAssessmentPDFResponse comprehensiveAssessmentPDFResponse = null;
        CommonFunctions common = null;
        private readonly DocumentUpload _documentUpload;
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
                    cadResponse = await _ComprehensiveAssessmentService.InsertModifyComprehensiveAssessmentDetail(comprehensiveAssessmentRequest);
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
                    cadResponse = await _ComprehensiveAssessmentService.HandleAssessmentVersioning(comprehensiveAssessmentRequest);
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


                    cadResponse = await _ComprehensiveAssessmentService.GetComprehensiveAssessmentDetail(comprehensiveAssessmentRequest);
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

                    comprehensiveAssessmentPDFResponse = await _ComprehensiveAssessmentService.PrintAssessmentPDF(comprehensiveAssessmentRequest);
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
        public async Task<HttpResponseMessage> UploadOfflinePDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            try
            {
                var filePath = _documentUpload.saveDocumentInFolder(comprehensiveAssessmentRequest.OfflinePDF,"PDF");

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
