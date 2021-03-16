
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.LifePlanResponse;

namespace IncidentManagement.Service.IService
{
    public interface ILifePlanService
    {
        #region 
        Task<LifePlanDetailTabResponse> InsertModifyLifePlanDetail(LifePlanDetailRequest lpdRequest);
        Task<LifePlanDetailTabResponse> HandleLifePlanDetail(LifePlanDetailRequest lpdRequest);
        Task<MeetingHistorySummaryResponse> HandleMeetingDetail(LifePlanDetailRequest lpdRequest);
        Task<IndividualSafeSummaryResponse> HandleIndividualSafeRecords(LifePlanDetailRequest lpdRequest);
    
        Task<AssessmentNarrativeSummaryResponse> HandleAssessmentNarrativeSummary(LifePlanDetailRequest lpdRequest);

        Task<OutcomesSupportStrategiesResponse> HandleOutcomesStrategies(LifePlanDetailRequest lpdRequest);
        Task<HCBSWaiverResponse> HandleHCBSWaiver(LifePlanDetailRequest lpdRequest);

        Task<FundalNaturalCommunityResourcesResponse> HandleFundalNaturalCommunityResources(LifePlanDetailRequest lpdRequest);
        Task<LifePlanDetailTabResponse> HandleLifeNotifications(LifePlanDetailRequest lpdRequest);

        
        Task<LifePlanPDFResponse> FillableLifePlanPDF(FillableLifePlanPDFRequest fillableLifePlanPDFRequest);


        Task<LifePlanDetailTabResponse> HandleLifePlanVersioning(FillableLifePlanPDFRequest lpdRequest);

        Task<LifePlanDetailTabResponse> GetMasterAuditRecords(LifePlanDetailRequest lpdRequest);
        Task<LifePlanDetailTabResponse> GetChildAuditRecords(LifePlanDetailRequest lpdRequest);
        Task<LifePlanExportedRecordsResponse> LifePlanEXportedRecords(LifePlanDetailRequest lpdRequest);
        Task<LifePlanDetailTabResponse> InsertModifysubmissionForm(SubmissionFormRequest submissionFormRequest);
        Task<MemberRepresentativeResponse> HandleMemberRepresentative(LifePlanDetailRequest lpdRequest);
        Task<MemberRightResponse> HandleMemberRight(LifePlanDetailRequest lpdRequest);


        #endregion
    }
}
