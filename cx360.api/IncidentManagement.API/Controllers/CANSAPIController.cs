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
using static IncidentManagement.Entities.Response.CANSResponse;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
namespace IncidentManagement.API.Controllers
{
    [RoutePrefix("CANSAPI")]
    public class CANSAPIController: ApiController
    {
        #region Private
        private System.Net.Http.HttpResponseMessage httpResponseMessage = null;
        private ICANSService _ICANSService = null;
        private CANSResponse cansResponse = null;
        CommonFunctions common = null;
        private CANSAssessmentPDFResponse cANSAssessmentPDFResponse = null;

        #endregion

        public CANSAPIController(ICANSService ICANSService)
        {
            _ICANSService = ICANSService;
        }
        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API inserts modifies the tabs details based on data.</remarks>
        /// <param name="cansRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertModifyCANSTabs")]
        [ActionName("InsertModifyCANSTabs")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> InsertModifyCANSTabs(CANSRequest cansRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();

                if (ModelState.IsValid && cansRequest != null)
                {
                    cansResponse = await _ICANSService.InsertModifyCANSTabs(cansRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                }
            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Insert/modify/delete the treatment plan details
        /// </summary>
        /// <remarks>This API Insert/modify/delete the tabs details based on data.</remarks>
        /// <param name="treatmentPlanRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCansTreatmentPlanFields")]
        [ActionName("AddCansTreatmentPlanFields")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> AddCansTreatmentPlanFields(TreatmentPlanRequest treatmentPlanRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();

                if (ModelState.IsValid && treatmentPlanRequest != null)
                {
                    cansResponse = await _ICANSService.AddCansTreatmentPlanFields(treatmentPlanRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                }
            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
        /// <summary>
        /// Get treament plan  details
        /// </summary>
        /// <remarks>This API returns the treatment plan details.</remarks>
        /// <param name="cansRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTreatmentPlanDetails")]
        [ActionName("GetTreatmentPlanDetails")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetTreatmentPlanDetails(CANSRequest cansRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();

                if (ModelState.IsValid && cansRequest != null)
                {
                    cansResponse = await _ICANSService.GetTreatmentPlanDetails(cansRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                }
            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Manage treatment plan service interventions actions
        /// </summary>
        /// <remarks>This API returns the Service Interventins data.</remarks>
        /// <param name="cansRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ManageServiceInterventionActions")]
        [ActionName("ManageServiceInterventionActions")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> ManageServiceInterventionActions(CANSRequest cansRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();

                if (ModelState.IsValid && cansRequest != null)
                {
                    cansResponse = await _ICANSService.ManageServiceInterventionActions(cansRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                }
            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Manage treatment plan service interventions actions
        /// </summary>
        /// <remarks>This API returns the Service Interventins data.</remarks>
        /// <param name="cansRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleCansVersioning")]
        [ActionName("HandleCansVersioning")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleCansVersioning(CANSRequest cansRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();
                if (ModelState.IsValid && cansRequest != null)
                {
                    cansResponse = await _ICANSService.HandleCansVersioning(cansRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                }

            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }


        /// <summary>
        /// Get CANS Assessment
        /// </summary>
        /// <remarks>This API is used to CANS Assessment.</remarks>
        /// <param name="cansRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCANSAssessmentDetails")]
        [ActionName("GetCANSAssessmentDetails")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetCANSAssessmentDetails(CANSRequest cansRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();
                if (ModelState.IsValid && cansRequest != null)
                {


                    cansResponse = await _ICANSService.GetCANSAssessmentDetails(cansRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                }

            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Import xml for Cans Type
        /// </summary>
        /// <remarks>This API is used to export xml file and .</remarks>
        /// <param name="xmlImportRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateAndImportXML")]
        [ActionName("GenerateAndImportXML")]
       // [AuthorizeUser]
        public async Task<HttpResponseMessage> GenerateAndImportXML(XMLImportRequest xmlImportRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cansResponse = new CANSResponse();
                if (ModelState.IsValid && xmlImportRequest != null)
                {


                    cansResponse = await _ICANSService.GenerateAndImportXML(xmlImportRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                    if (!File.Exists(cansResponse.CommonCANSRsponse[0].FileName))
                    {
                        httpResponseMessage.StatusCode = HttpStatusCode.NotFound;
                        httpResponseMessage.ReasonPhrase = string.Format("File not found: {0} .","file");
                        throw new HttpResponseException(httpResponseMessage);
                    }
                    Stream stream = CommonFunctions.GetFilesStream(cansResponse.CommonCANSRsponse[0].FileName);

                    httpResponseMessage.Content = new StreamContent(stream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "test.pdf"
                    };
                    httpResponseMessage.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "fileNameOfYourChoice.pdf";
                }

            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Get cans Assessment for pdf printing
        /// </summary>
        /// <remarks>This API is used to  get Cans Assessment.</remarks>
        /// <param name="cANSRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("PrintAssessmentPDF")]
        [ActionName("PrintAssessmentPDF")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> PrintAssessmentPDF(CANSRequest cANSRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                cANSAssessmentPDFResponse = new CANSAssessmentPDFResponse();
                if (ModelState.IsValid && cANSRequest != null)
                {

                    cANSAssessmentPDFResponse = await _ICANSService.PrintAssessmentPDF(cANSRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cANSAssessmentPDFResponse);
                    Stream stream = CommonFunctions.GetFilesStream(cANSAssessmentPDFResponse.CANSAssessmentPDF[0].FileName);
                    httpResponseMessage.Content = new StreamContent(stream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "fileNameOfYourChoice";
                }

            }
            catch (Exception Ex)
            {
                cansResponse.Success = false;
                cansResponse.IsException = true;
                cansResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, cansResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
    }
}