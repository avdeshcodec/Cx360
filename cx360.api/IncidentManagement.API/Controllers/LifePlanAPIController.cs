using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Service.IService;
using IncidentManagement.API.Filter;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IncidentManagement.Entities.Common;
using static IncidentManagement.Entities.Response.LifePlanResponse;
using System.Net.Http.Headers;

namespace IncidentManagement.API.Controllers
{
    [RoutePrefix("LifePlanAPI")]
    public class LifePlanAPIController : ApiController
    {




        #region Private 
        private System.Net.Http.HttpResponseMessage httpResponseMessage = null;
        private LifePlanDetailTabResponse lpdResponse = null;
        private string connectionString = null;
        private ILifePlanService _LifePlanService = null;
        MeetingHistorySummaryResponse mhsResponse = null;
        BaseResponse baseResponse = null;
        IndividualSafeSummaryResponse issResponse = null;
        AssessmentNarrativeSummaryResponse assessmentNarrativeSummaryResponse = null;
        OutcomesSupportStrategiesResponse outcomesSupportStrategiesResponse = null;
        HCBSWaiverResponse hCBSWaiverResponse = null;
        FundalNaturalCommunityResourcesResponse fundalNaturalCommunityResourcesResponse = null;
        LifePlanExportedRecordsResponse lifePlanExportedRecordsResponse = null;
        CommonFunctions common = null;
        LifePlanPDFResponse lifePlanPDFResponse = null;
        #endregion






        public LifePlanAPIController(ILifePlanService LLifePlanService)
        {
            _LifePlanService = LLifePlanService;
        }

        /// <summary>
        /// Insert modify tab details
        /// </summary>
        /// <remarks>This API inserts modifies the tabs details based on data.</remarks>
        /// <param name="lpdRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertModifyLifePlanDetail")]
        [ActionName("InsertModifyLifePlanDetail")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> InsertModifyLifePlanDetail(LifePlanDetailRequest lpdRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();

                if (ModelState.IsValid && lpdRequest != null)
                {
                    lpdResponse = await _LifePlanService.InsertModifyLifePlanDetail(lpdRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }
            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }




        // <summary>
        /// Get life plan details 
        /// </summary>
        /// <remarks>This API hanldes the meeting details the .</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleLifePlanDetail")]
        [ActionName("HandleLifePlanDetail")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleLifePlanDetail(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                   
       
                    lpdResponse = await _LifePlanService.HandleLifePlanDetail(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }

            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }


        /// <summary>
        /// handle meeting details 
        /// </summary>
        /// <remarks>This API returns the .</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleMeetingDetail")]
        [ActionName("HandleMeetingDetail")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleMeetingDetail(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                mhsResponse = new MeetingHistorySummaryResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {


                    mhsResponse = await _LifePlanService.HandleMeetingDetail(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, mhsResponse);
                }

            }
            catch (Exception Ex)
            {
                mhsResponse.Success = false;
                mhsResponse.IsException = true;
                mhsResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, mhsResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }




        /// <summary>
        /// handle IndividualSafeRecords details 
        /// </summary>
        /// <remarks>This API returns the IndividualSafeRecords.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleIndividualSafeRecords")]
        [ActionName("HandleIndividualSafeRecords")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleIndividualSafeRecords(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                issResponse = new IndividualSafeSummaryResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    issResponse = await _LifePlanService.HandleIndividualSafeRecords(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, issResponse);
                }

            }
            catch (Exception Ex)
            {
                issResponse.Success = false;
                issResponse.IsException = true;
                issResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, issResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }


        /// <summary>
        /// handle AssessmentNarrativeSummary details 
        /// </summary>
        /// <remarks>This API returns the AssessmentNarrativeSummary.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleAssessmentNarrativeSummary")]
        [ActionName("HandleAssessmentNarrativeSummary")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleAssessmentNarrativeSummary(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                assessmentNarrativeSummaryResponse = new AssessmentNarrativeSummaryResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    assessmentNarrativeSummaryResponse = await _LifePlanService.HandleAssessmentNarrativeSummary(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, assessmentNarrativeSummaryResponse);
                }

            }
            catch (Exception Ex)
            {
                assessmentNarrativeSummaryResponse.Success = false;
                assessmentNarrativeSummaryResponse.IsException = true;
                assessmentNarrativeSummaryResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, assessmentNarrativeSummaryResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }

        /// <summary>
        /// handle OutcomesStrategies details 
        /// </summary>
        /// <remarks>This API returns the OutcomesStrategies.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleOutcomesStrategies")]
        [ActionName("HandleOutcomesStrategies")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleOutcomesStrategies(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                outcomesSupportStrategiesResponse = new OutcomesSupportStrategiesResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    outcomesSupportStrategiesResponse = await _LifePlanService.HandleOutcomesStrategies(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, outcomesSupportStrategiesResponse);
                }

            }
            catch (Exception Ex)
            {
                outcomesSupportStrategiesResponse.Success = false;
                outcomesSupportStrategiesResponse.IsException = true;
                outcomesSupportStrategiesResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, outcomesSupportStrategiesResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }

        /// <summary>
        /// handle HCBSWaiver details 
        /// </summary>
        /// <remarks>This API returns the HCBSWaiver.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleHCBSWaiver")]
        [ActionName("HandleHCBSWaiver")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleHCBSWaiver(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                hCBSWaiverResponse = new HCBSWaiverResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    hCBSWaiverResponse = await _LifePlanService.HandleHCBSWaiver(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, hCBSWaiverResponse);
                }

            }
            catch (Exception Ex)
            {
                hCBSWaiverResponse.Success = false;
                hCBSWaiverResponse.IsException = true;
                hCBSWaiverResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, hCBSWaiverResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }

        /// <summary>
        /// handle FundalNaturalCommunityResources details 
        /// </summary>
        /// <remarks>This API returns the FundalNaturalCommunityResources.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleFundalNaturalCommunityResources")]
        [ActionName("HandleFundalNaturalCommunityResources")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleFundalNaturalCommunityResources(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                fundalNaturalCommunityResourcesResponse = new FundalNaturalCommunityResourcesResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    fundalNaturalCommunityResourcesResponse = await _LifePlanService.HandleFundalNaturalCommunityResources(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, fundalNaturalCommunityResourcesResponse);
                }

            }
            catch (Exception Ex)
            {
                fundalNaturalCommunityResourcesResponse.Success = false;
                fundalNaturalCommunityResourcesResponse.IsException = true;
                fundalNaturalCommunityResourcesResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, fundalNaturalCommunityResourcesResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }


        /// <summary>
        /// handle LifePlanNotifications details 
        /// </summary>
        /// <remarks>This API returns the LifePlanNotifications.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleLifeNotifications")]
        [ActionName("HandleLifeNotifications")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleLifeNotifications(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {


                    lpdResponse = await _LifePlanService.HandleLifeNotifications(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }

            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }

        /// <summary>
        /// handle LifePlanVersioning details 
        /// </summary>
        /// <remarks>This API returns the LifePlanVersioning.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("HandleLifePlanVersioning")]
        [ActionName("HandleLifePlanVersioning")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> HandleLifePlanVersioning(FillableLifePlanPDFRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    lpdResponse = await _LifePlanService.HandleLifePlanVersioning(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }

            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);

            }
            return httpResponseMessage;
        }




        /// <summary>
        /// tab details
        /// </summary>
        /// <remarks>This API fills the life plan pdf .</remarks>
        /// <param name="fillablePDFRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("FillableLifePlanPDF")]
        [ActionName("FillableLifePlanPDF")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> FillableLifePlanPDF(FillableLifePlanPDFRequest fillableLifePlanPDFRequest)
        {
            try
            {
                common = new CommonFunctions();
                httpResponseMessage = new HttpResponseMessage();
                lifePlanPDFResponse = new LifePlanPDFResponse();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && fillableLifePlanPDFRequest != null)
                {
                    lifePlanPDFResponse = await _LifePlanService.FillableLifePlanPDF(fillableLifePlanPDFRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lifePlanPDFResponse);
                    Stream stream = CommonFunctions.GetFilesStream(lifePlanPDFResponse.LifePlanPDF[0].FileName);
                    httpResponseMessage.Content = new StreamContent(stream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "fileNameOfYourChoice";
                }
            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }




        /// <summary>
        /// get the master audit detail 
        /// </summary>
        /// <remarks>This API fills the audit log  .</remarks>
        /// <param name="fillablePDFRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMasterAuditRecords")]
        [ActionName("GetMasterAuditRecords")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetMasterAuditRecords(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    lpdResponse = await _LifePlanService.GetMasterAuditRecords(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }

            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// get the auidt child tables
        /// </summary>
        /// <remarks>This API fills the child tbales audit log  .</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetChildAuditRecords")]
        [ActionName("GetChildAuditRecords")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> GetChildAuditRecords(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    lpdResponse = await _LifePlanService.GetChildAuditRecords(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }

            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
        /// <summary>
        /// handle SuggestedOutcomesStrategies details 
        /// </summary>
        /// <remarks>This API returns the SuggestedOutcomesStrategies.</remarks>
        /// <param name="lifePlanDetailRequest"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("LifePlanEXportedRecords")]
        [ActionName("LifePlanEXportedRecords")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> LifePlanEXportedRecords(LifePlanDetailRequest lifePlanDetailRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lifePlanExportedRecordsResponse = new LifePlanExportedRecordsResponse();
                if (ModelState.IsValid && lifePlanDetailRequest != null)
                {
                    lifePlanExportedRecordsResponse = await _LifePlanService.LifePlanEXportedRecords(lifePlanDetailRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lifePlanExportedRecordsResponse);
                }

            }
            catch (Exception Ex)
            {
                lifePlanExportedRecordsResponse.Success = false;
                lifePlanExportedRecordsResponse.IsException = true;
                lifePlanExportedRecordsResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lifePlanExportedRecordsResponse);
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Insert modify submissionform data
        /// </summary>
        /// <remarks>Insert modify submissionform data.</remarks>
        /// <param name="submissionForm"> Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertModifysubmissionForm")]
        [ActionName("InsertModifysubmissionForm")]
        [AuthorizeUser]
        public async Task<HttpResponseMessage> InsertModifysubmissionForm(SubmissionFormRequest submissionFormRequest)
        {
            try
            {
                httpResponseMessage = new HttpResponseMessage();
                lpdResponse = new LifePlanDetailTabResponse();
                if (ModelState.IsValid && submissionFormRequest != null)
                {
                    lpdResponse = await _LifePlanService.InsertModifysubmissionForm(submissionFormRequest);
                    httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                }

            }
            catch (Exception Ex)
            {
                lpdResponse.Success = false;
                lpdResponse.IsException = true;
                lpdResponse.Message = Ex.Message;
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, lpdResponse);
                CommonFunctions.LogError(Ex);
            }
            return httpResponseMessage;
        }
    }


}
