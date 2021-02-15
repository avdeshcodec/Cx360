using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Response
{
  public class LifePlanResponse: BaseResponse
    {
        public class LifePlanDetailTabResponse : BaseResponse
        {
            public List<AllTab> AllTab { get; set; }
            public List<LifPlanDetailsData> LifPlanDetailsData { get; set; }
        }
        public class AllTab
        {
            public int LifePlanId { get; set; }
            public int AssessmentNarrativeSummaryId { get; set; }
            public int MedicaidStatePlanAuthorizedServiesId { get; set; }
            public int FundalNaturalCommunityResources { get; set; }
            public int SupportStrategieId { get; set; }
            public int IndividualPlanOfProtectionId { get; set; }
            public int MeetingId { get; set; }
            public string JSONData { get; set; }
            public string DocumentStatus { get; set; }
            public string DocumentVersion { get; set; }
            public int DocumentVersionId { get; set; }
           
            public int ClientId { get; set; }
            public bool? ValidatedRecord { get; set; }

          public  string JSONDataPrevious { get; set; }
          public string JSONDataCurrent { get; set; }

            public string Documentname { get; set; }
            public string DocumentFileName { get; set; }
            public string Status { get; set; }
        }



        public class LifPlanDetailsData
        {
            public int LifePlanId { get; set; }
            public int ClientId { get; set; }
            public int CompanyId { get; set; }
            public int CreatedBy { get; set; }
            public int LastModifiedBy { get; set; }
            public string Actions { get; set; }
            public string DateOfBirth { get; set; }
            public string MemberAddress1 { get; set; }
            public string MemberAddress2 { get; set; }
            public string Phone { get; set; }
            public string Medicaid { get; set; }
            public string Medicare { get; set; }
            public string EnrollmentDate { get; set; }
            public string WillowbrookMember { get; set; }
            public string EffectiveFromDate { get; set; }
            public string EffectiveToDate { get; set; }
            public string AddressCCO { get; set; }
            public string PhoneCCO { get; set; }
            public string Fax { get; set; }
            public string ProviderID { get; set; }
            public string Active { get; set; }
            public string CreatedOn { get; set; }
            public string LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
            public string DocumentStatus { get; set; }
            public string DocumentVersion { get; set; }
            public int DocumentVersionId { get; set; }
            public bool? LatestVersion { get; set; }
            public string Status { get; set; }
        }


       

        
       
       

      
        
    }
    public class MeetingHistorySummaryResponse : BaseResponse
    {
        public List<MeetingHistorySummaryTab> MeetingHistorySummaryTab { get; set; }
    }
    public class MeetingHistorySummaryTab
    {
        public int MeetingId { get; set; }
        public int LifePlanId { get; set; }
        public DateTime? PlanerReviewDate { get; set; }
        public string MeetingReason { get; set; }
        public string MemberAttendance { get; set; }
        public int? MyProperty { get; set; }
        public int? TypeOfMeeting { get; set; }
        public string TypeOfMeetingText { get; set; }
        public string InformationDiscussed { get; set; }
        public string InformationPresented { get; set; }
        public char Active { get; set; }

        public string Actions { get; set; }


    }
    public class IndividualSafeSummaryResponse : BaseResponse
    {
        public List<IndividualSafeSummaryTab> IndividualSafeSummaryTab { get; set; }
    }
    public class IndividualSafeSummaryTab
    {
        public int IndividualPlanOfProtectionID { get; set; }
        public int LifePlanId { get; set; }
        public string GoalValuedOutcome { get; set; }
        public string ProviderAssignedGoal { get; set; }
        public string ProviderLocation { get; set; }
        public int ServicesTypeId { get; set; }
        public string ServicesType { get; set; }
        public int FrequencyId { get; set; }
        public string Frequency { get; set; }
        public int QuantityId { get; set; }
        public string Quantity { get; set; }
        public int TimeFrameId { get; set; }
        public string TimeFrame { get; set; }
        public string SpecialConsiderations { get; set; }
        public char Active { get; set; }

        public string Actions { get; set; }

    }

    public class AssessmentNarrativeSummaryResponse : BaseResponse
    {
        public List<AssessmentNarrativeSummaryTab> AssessmentNarrativeSummaryTab { get; set; }
    }
    public class AssessmentNarrativeSummaryTab
    {
        public int AssessmentNarrativeSummaryId { get; set; }
        public int LifePlanId { get; set; }
        public string MyHome { get; set; }
        public string MyWork { get; set; }
        public string MyHealthAndMedication { get; set; }
        public string MyRelationships { get; set; }
        public char Active { get; set; }
    }
    public class OutcomesSupportStrategiesResponse : BaseResponse
    {
        public List<OutcomesSupportStrategiesTab> OutcomesSupportStrategiesTab { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }


    }
    public class OutcomesSupportStrategiesTab
    {
        public int SupportStrategieId { get; set; }
        public int LifePlanId { get; set; }
        public string  CqlPomsGoal { get; set; }
       public int CqlPomsGoalId { get; set; }
        public string CcoGoal { get; set; }
        public string ProviderAssignedGoal { get; set; }
        public string ProviderLocation { get; set; }
        public int ServicesTypeId { get; set; }
        public string ServicesType { get; set; }
        public int FrequencyId { get; set; }
        public string Frequency { get; set; }
        public int QuantityId { get; set; }
        public string Quantity { get; set; }
        public int TimeFrameId { get; set; }
        public string TimeFrame { get; set; }
        public string SpecialConsiderations { get; set; }
        public char Active { get; set; }

        public string Actions { get; set; }

    }
    public class LifePlanExportedRecordsResponse : BaseResponse    {        public List<SuggestedOutcomesSupportStrategiesTab> suggestedOutcomesSupportStrategiesTab { get; set; }        public List<MeetingHistorySummaryTab> MeetingHistorySummaryTab { get; set; }        public int PageIndex { get; set; }        public int PageSize { get; set; }        public int RecordCount { get; set; }    }
    public class SuggestedOutcomesSupportStrategiesTab
    {
        public int SuggestedSupportStrategieId { get; set; }
        public int LifePlanId { get; set; }
        public string CqlPomsGoal { get; set; }
        public int CqlPomsGoalId { get; set; }
        public string CcoGoal { get; set; }
        public string ProviderAssignedGoal { get; set; }
        public string ProviderLocation { get; set; }
        public int ProviderLocationId { get; set; }
        public int ServicesTypeId { get; set; }
        public string ServicesType { get; set; }
        public int FrequencyId { get; set; }
        public string Frequency { get; set; }
        public int QuantityId { get; set; }
        public string Quantity { get; set; }
        public int TimeFrameId { get; set; }
        public string TimeFrame { get; set; }
        public string SpecialConsiderations { get; set; }
        public char Active { get; set; }

        public string Actions { get; set; }

    }
    public class HCBSWaiverResponse : BaseResponse
    {
        public List<HCBSWaiverTab> HCBSWaiverTab { get; set; }
    }
    public class HCBSWaiverTab
    {
        public int MedicaidStatePlanAuthorizedServiesId { get; set; }
        public int LifePlanId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FacilityCode { get; set; }
        public string FacilityName { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public int Unit { get; set; }
        public string Comments { get; set; }
        public char Active { get; set; }
        public string CombinedDate { get; set; }
        public string Actions { get; set; }

    }
    public class FundalNaturalCommunityResourcesResponse : BaseResponse
    {
        public List<FundalNaturalCommunityResourcesTab> fundalNaturalCommunityResourcesTab { get; set; }
    }
    public class FundalNaturalCommunityResourcesTab
    {
        public int FundalNaturalCommunityResourcesId { get; set; }
        public int LifePlanId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public char Active { get; set; }
        public string Name { get; set; }
        public string Actions { get; set; }
    }

    public class LifePlanPDFResponse
    {
        public List<LifePlanPDF> LifePlanPDF { get; set; }
    }
    public class LifePlanPDF
    {
        public string FileName { get; set; }

        public string DownloadFile { get; set; }
    }
}
