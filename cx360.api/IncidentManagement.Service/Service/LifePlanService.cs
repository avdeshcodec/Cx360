using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.IRepository;
using IncidentManagement.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.LifePlanResponse;

namespace IncidentManagement.Service.Service
{
    public class LifePlanService : ILifePlanService
    {
        #region Private
        private ILIfePlanRepository _lifePlanRepository;
        #endregion

        public LifePlanService(ILIfePlanRepository lIfePlanRepository)
        {
            _lifePlanRepository = lIfePlanRepository;
        }
        public async Task<LifePlanDetailTabResponse> InsertModifyLifePlanDetail(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.InsertModifyLifePlanDetail(lpdRequest);
        }
        public async Task<LifePlanDetailTabResponse> HandleLifePlanDetail(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleLifePlanDetail(lpdRequest);
        }
        public async Task<MeetingHistorySummaryResponse> HandleMeetingDetail(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleMeetingDetail(lpdRequest);
        }
        public async Task<IndividualSafeSummaryResponse> HandleIndividualSafeRecords(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleIndividualSafeRecords(lpdRequest);
        }
        public async Task<AssessmentNarrativeSummaryResponse> HandleAssessmentNarrativeSummary(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleAssessmentNarrativeSummary(lpdRequest);
        }

        public async Task<OutcomesSupportStrategiesResponse> HandleOutcomesStrategies(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleOutcomesStrategies(lpdRequest);
        }
        public async Task<HCBSWaiverResponse> HandleHCBSWaiver(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleHCBSWaiver(lpdRequest);
        }
        public async Task<FundalNaturalCommunityResourcesResponse> HandleFundalNaturalCommunityResources(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleFundalNaturalCommunityResources(lpdRequest);
        }
        public async Task<LifePlanDetailTabResponse> HandleLifeNotifications(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleLifeNotifications(lpdRequest);
        }


        public async Task<LifePlanPDFResponse> FillableLifePlanPDF(FillableLifePlanPDFRequest fillableLifePlanPDFRequest)
        {
            return await _lifePlanRepository.FillableLifePlanPDF(fillableLifePlanPDFRequest);
        }
        public async Task<LifePlanDetailTabResponse> HandleLifePlanVersioning(FillableLifePlanPDFRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleLifePlanVersioning(lpdRequest);
        }

        public async Task<LifePlanDetailTabResponse> GetMasterAuditRecords(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.GetMasterAuditRecords(lpdRequest);
        }
        public async Task<LifePlanDetailTabResponse> GetChildAuditRecords(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.GetChildAuditRecords(lpdRequest);
        }

        public async Task<LifePlanExportedRecordsResponse> LifePlanEXportedRecords(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.LifePlanEXportedRecords(lpdRequest);
        }
        public async Task<LifePlanDetailTabResponse> InsertModifysubmissionForm(SubmissionFormRequest submissionFormRequest)
        {
            return await _lifePlanRepository.InsertModifysubmissionForm(submissionFormRequest);
        }
        public async Task<MemberRepresentativeResponse> HandleMemberRepresentative(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleMemberRepresentative(lpdRequest);
        }
        public async Task<MemberRightResponse> HandleMemberRight(LifePlanDetailRequest lpdRequest)
        {
            return await _lifePlanRepository.HandleMemberRight(lpdRequest);
        }


    }
}
