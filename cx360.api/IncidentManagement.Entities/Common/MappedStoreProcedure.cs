using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Common
{
  public static class MappedStoreProcedure
    {
        public static readonly string GeneralTab = "usp_InsertModifyIncidentManagementGeneral";
        public static readonly string InjuryTab = "usp_InsertModifyIncidentManagementInjury";
        public static readonly string DeathTab = "usp_InsertModifyIncidentManagementDeath";
        public static readonly string MedicationTab = "usp_InsertModifyIncidentManagementMedicationError";
        public static readonly string OtherTab = "usp_InsertModifyIncidentManagementOther";

        public static readonly string StateFormTab = "usp_InsertModifyStateForm";
        public static readonly string OPWDD147Tab = "usp_InsertModifyOPWDD147";
        public static readonly string OPWDD148Tab = "usp_InsertModifyOPWDD148";
        public static readonly string OPWDD150Tab = "usp_InsertModifyOPWDD150";
        public static readonly string JonathanLawTab = "usp_InsertModifyJonathanLaw";

        public static readonly string MasterPage = "usp_GetMasterPageDetails";
        public static readonly string OPWDD150Details = "usp_GetStateFormDetails";
        public static readonly string DeleteMasterRecord = "usp_DeleteMasterRecords";
        public static readonly string EditAllRecord = "usp_GetEditAllRecord";


        public static readonly string LifePlan = "usp_InsertModifyLifePlanDetail";
        public static readonly string MeetingHistory = "usp_InsertModifyMeetingHistoryDetail";
        public static readonly string GetMeetingHistory = "usp_GetMeetingHistorySummary";
        public static readonly string IndividualSafe = "usp_InsertModifyIndividualSafe";//GetIndividualSafe
        public static readonly string GetIndividualSafe = "usp_GetIndividualSafeDetail";
        public static readonly string AssessmentNarrativeSummary = "usp_InsertModifyAssessmentNarrativeSummary";
        public static readonly string GetAssessmentNarrativeSummaryTab = "usp_GetAssessmentNarrativeSummary";
        public static readonly string InsertOutcomesSupportStrategies = "usp_InsertModifyOutcomesSupportStrategies";
        public static readonly string GetOutcomesSupportStrategies = "usp_GetOutcomesSupportStrategies";
        public static readonly string GetMedicaidStatePlanAuthorizedServies = "usp_GetMedicaidStatePlanAuthorizedServies";
        public static readonly string InsertHCBSWaiver = "usp_InsertModifyHCBSWaiver";
        public static readonly string GetFundalNaturalCommunityResources = "usp_GetFundalNaturalCommunityResources";
        public static readonly string InsertFundalNaturalCommunityResources = "usp_InsertModifyFundalNaturalCommunityResources";
        public static readonly string LifePlanMasterPage = "usp_GetLifePlanRecords";
        public static readonly string GetLifePlanRecords = "usp_GetLifePlanRecords";

        public static readonly string GetLifePlanPDFDetails = "usp_GetLifePlanPDFDetails";

        public static readonly string PublishLifePlanVersion = "usp_PublishLifePlanVersion";
        public static readonly string CreateNewVersion = "usp_CreateNewVersion";
        public static readonly string InsertLifePlanNotifications = "usp_InsertModifyLifeplanNotifications";

        

        //Comprehensive assessment
        public static readonly string ComprehensiveAssessment = "usp_InsertModifyComprehensiveAssessment";
        public static readonly string AssessmentMedical = "usp_InsertModifyMedicalSection";

        public static readonly string AssessmentMedicalHealth = "usp_InsertModifyMentalHealthSection";
        public static readonly string AssessmentFinancial = "usp_InsertModifyFinancailSection";
        public static readonly string AssessmentHousing = "usp_InsertModifyHousingSection";
        public static readonly string AssessmentDomesticViolance = "usp_InsertModifyDomesticViolanceSection";
        public static readonly string AssessmentLegal = "usp_InsertModifyLegalSection";

        public static readonly string AreasSafeguardReview = "usp_InsertModifyAreasSafeguardReview";        public static readonly string IndependentLivingSkills = "usp_InsertModifyIndependentLivingSkills";        public static readonly string BehavioralSupportServices = "usp_InsertModifyBehavioralSupportServices";        public static readonly string EducationalVocationalStatus = "usp_InsertModifyEducationalVocationalStatus";        public static readonly string SelfDirectedServices = "usp_InsertModifySelfDirectedServices";        public static readonly string TransitionPlanning = "usp_InsertModifyTransitionPlanning";        public static readonly string SafetyPlan = "usp_InsertModifySafetyPlan";        public static readonly string SubstanceAbuseScreening = "usp_AssessmentSubstanceAbuseScreening";        public static readonly string DepressionScreening = "usp_InsertModifyDepressionScreeningSection";        public static readonly string SafetyRiskAssessment = "usp_InsertModifySafetyRiskAssessmentSection";        public static readonly string General = "usp_InsertModifyGeneralSection";

        public static readonly string PublishAssessment = "usp_PublishAssessmentVersion";
        public static readonly string CreateNewVersionAssessment = "usp_CreateNewVersionAssessment";
        public static readonly string AllComprehensiveAssessmentDetails = "usp_GetComprehensiveAssessmentDetails";

        //CANS store procedures list 
        public static readonly string TreatmentPlan = "usp_InsertModifyTreatmentPlan";
        public static readonly string TreatmentPlanDetails = "usp_GetCansTreatmentPlanDetails";
        public static readonly string ChangeTreatmentPlanPosition = "usp_ModifyTreatmentPlanPositions";
        public static readonly string UpdateGoalItems = "usp_InsertModifyCANSTreatmentPlanGoals";
        public static readonly string CloneTreatmentPlan = "usp_DeleteAndCopyTreatmentPlan";
        public static readonly string DeleteTreatmentPlan = "usp_DeleteAndCopyTreatmentPlan";
        public static readonly string SummaryPrioritzedNeedsStrength = "usp_InsertModifySummaryPrioritzedNeedsStrength";
        public static readonly string ServiceInterventionObjecives = "usp_GetServiceInerventionObjctives";
        public static readonly string ServiceInterventions = "usp_InsertModifyServiceInterventions";
        public static readonly string PublishCansModule = "usp_PublishCansVersion";

        public static readonly string CreateNewVersionCans = "usp_CreateNewVerionCans";
        public static readonly string GeneralInformation = "usp_InsertModifyGeneralInformation";
        public static readonly string TraumaExposure = "usp_InsertModifyTraumaExposure";
        public static readonly string PresentingProblemAndImpact = "usp_InsertModifyPresentinProblemAndImpact";
        public static readonly string Safety = "usp_InsertModifySafety";
        public static readonly string SubstanceUseHistory = "usp_InsertModifySubstanceUseHistory";
        public static readonly string PlacementHistory = "usp_InsertModifyPlacementHistory";
        public static readonly string ClientStrength = "usp_InsertModifyClientStrength";
        public static readonly string FamilyInformation = "usp_InsertModifyFamilyInformation";
        public static readonly string NeedsResourceAssessment = "usp_InsertModifyNeedsResourceAssessment";
        public static readonly string DsmDiagnosis = "usp_InsertModifyDsmDiagnosis";
        public static readonly string MentalHealthSummary = "usp_InsertModifyMentalHealthSummary";
        public static readonly string AddClientFunctioningEvaluation = "usp_InsertModifyAddClientFunctioningEvaluation";
        public static readonly string PresentinProblemAndImpact = "usp_InsertModifyPresentinProblemAndImpact";
        public static readonly string PsychiatricInformation = "usp_InsertModifyPsychiatricInformation";


        public static readonly string GeneralInformatioinHRA = "usp_InsertModifyGeneralInformatioinHRA";
        public static readonly string CaregiverAddendum = "usp_InsertModifyCaregiverAddendum";
        public static readonly string GeneralInformationDCFS = "usp_InsertModifyGeneralInformationDCFS";
        public static readonly string SexuallyAggrBehavior = "usp_InsertModifySexuallyAggrBehavior";
        public static readonly string ParentGuardSafety = "usp_InsertModifyParentGuardSafety";
        public static readonly string ParentGuardWellbeing = "usp_InsertModifyParentGuardWellbeing";
        public static readonly string ParentGuardPermananence = "usp_InsertModifyParentGuardPermananence";
        public static readonly string SubstituteCommitPermananence = "usp_InsertModifySubstituteCommitPermananence";
        public static readonly string IntactFamilyService = "usp_InsertModifyIntactFamilyService";
        public static readonly string IntensivePlacementStabilization = "usp_InsertModifyIntensivePlacementStabilization";
        public static readonly string HealthStatus = "usp_InsertModifyHealthStatus";
        public static readonly string DevelopmentHistory = "usp_InsertModifyDevelopmentHistory";
        public static readonly string Medication = "usp_InsertModifyMedication";
        public static readonly string MedicalHistory = "usp_InsertModifyMedicalHistory";
        public static readonly string PresentingSignatures = "usp_InsertModifyCansSignature";
        public static readonly string IndividualTreatmentPlan = "usp_IndividualTreatmentPlan";

        //public static readonly string LifePlanMasterPage = "usp_GetLifePlanDataDetails";
        //public static readonly string GetLifePlanRecords = "usp_GetLifePlanRecords";

        //CCO Comprehensive assessment
        public static readonly string CCO_ComprehensiveAssessment = "usp_InsertModifyCCO_ComprehensiveAssessment";
        public static readonly string CCO_EligibilityInformation = "usp_InsertModifyCCO_EligibilityInformationSection";
        public static readonly string CCO_CommunicationLanguage = "usp_InsertModifyCCO_CommunicationSection";
    }
}
