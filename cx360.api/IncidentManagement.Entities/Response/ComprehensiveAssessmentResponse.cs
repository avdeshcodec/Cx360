using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Response
{
   public class ComprehensiveAssessmentResponse:BaseResponse
    {
        public class ComprehensiveAssessmentDetailResponse : BaseResponse
        {
            public List<AllTabsComprehensiveAssessment> AllTabsComprehensiveAssessment { get; set; }
            public List<ComprehensiveAssessmentData> ComprehensiveAssessmentData { get; set; }
            //fill Details
            public List<AreasSafeguardReviewDetails> AreasSafeguardReviewDetails { get; set; }
            public List<BehavioralSupportServicesDetails> BehavioralSupportServicesDetails { get; set; }
            public List<DepressionScreeningDetails> DepressionScreeningDetails { get; set; }
            public List<DomesticViolanceDetails> DomesticViolanceDetails { get; set; }
            public List<EducationalVocationalStatusDetails> EducationalVocationalStatusDetails { get; set; }
            public List<FinancialDetails> FinancialDetails { get; set; }
            public List<GeneralDetails> GeneralDetails { get; set; }
            public List<HousingDetails> HousingDetails { get; set; }
            public List<IndependentLivingSkillsDetails> IndependentLivingSkillsDetails { get; set; }
            public List<LegalDetails> LegalDetails { get; set; }
            public List<MedicalDetails> MedicalDetails { get; set; }
            public List<MedicalHelathDetails> MedicalHelathDetails { get; set; }
            public List<SafetyPlanDetails> SafetyPlanDetails { get; set; }
            public List<SafetyRiskDetails> SafetyRiskDetails { get; set; }
            public List<SelfDirectedServicesDetails> SelfDirectedServicesDetails { get; set; }
            public List<SubstanceAbuseScreeningDetails> SubstanceAbuseScreeningDetails { get; set; }
            public List<TransitionPlanningDetails> TransitionPlanningDetails { get; set; }
            public List<VersioningDetails> VersioningDetails { get; set; }
            public List<ComprehensiveAssessmentDetails> ComprehensiveAssessmentDetails { get; set; }
            public List<DomesticViolanceMemberRelationshipDetails> DomesticViolanceMemberRelationshipDetails { get; set; }
            public List<FinancialMemberNeedDetails> FinancialMemberNeedDetails { get; set; }
            public List<FinancialMemberStatusDetails> FinancialMemberStatusDetails { get; set; }
            public List<HousingSubsidyDetails> HousingSubsidyDetails { get; set; }
            public List<LegalCourtDateDetails> LegalCourtDateDetails { get; set; }
            public List<MedicalDiagnosisDetails> MedicalDiagnosisDetails { get; set; }
            public List<MedicalHealthMedicationsDetails> MedicalHealthMedicationsDetails { get; set; }
            public List<MedicalMedicationDetails> MedicalMedicationDetails { get; set; }

            public List<NewVersionDetails> NewVersionDetails { get; set; }
        }
        public class AllTabsComprehensiveAssessment
        {
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CommunicationId { get; set; }
            public int EligibilityInformationId { get; set; }
            public int MemberProviderId { get; set; }
            public int GuardianshipAndAdvocacyId { get; set; }
            public int AdvancedDirectivesFuturePlanningId { get; set; }
            public int IndependentLivingSkillId { get; set; }
            public int SocialServiceNeedId { get; set; }
            public string DocumentStatus { get; set; }
            public string DocumentVersion { get; set; }
            public bool? ValidatedRecord { get; set; }
            public int AssessmentMedicalId { get; set; }
            public int AssessmentMedicalHelathId { get; set; }
            public int AssessmentFinancialId { get; set; }
            public int AssessmentHousingId { get; set; }
            public int AssessmentDomesticViolanceId { get; set; }
            public int AssessmentLegalId { get; set; }
            public int AssessmentGeneralId { get; set; }
            public int AssessmentSafetyRiskId { get; set; }
            public int AssessmentDepressionScreeningId { get; set; }
            public int AssessmentSubstanceAbuseScreeningId { get; set; }
            public int AssessmentSafetyPlanId { get; set; }
            public string JSONDataFirstTable { get; set; }
            public string JSONDataSecondTable { get; set; }
            public string JSONData { get; set; }
            public string JSONMedicalDiagnosis { get; set; }
            public string JSONMedicalMedications { get; set; }

            public string JSONMedicalHealthMedications { get; set; }

            public string JSONFinancialMemberStatus { get; set; }

            public string JSONFinancialMemberNeeds { get; set; }

            public string JSONHousingSubsidies { get; set; }
            public string JSONDomesticViolanceRelationship { get; set; }
            public string JSONLegalCourtDates { get; set; }

            public int AssessmentAreasSafeguardReviewId { get; set; }            public int AssessmentBehavioralSupportServicesId { get; set; }            public int AssessmentEducationalVocationalStatusId { get; set; }            public int AssessmentIndependentLivingSkillsId { get; set; }            public int AssessmentSelfDirectedServicesId { get; set; }            public int AssessmentTransitionPlanningId { get; set; }

            public string Status { get; set; }


            public string MedicalStatus { get; set; }
            public string MedicalHealthStatus { get; set; }
            public string FinancialStatus { get; set; }
            public string HousingStatus { get; set; }
            public string DomesticViolanceStatus { get; set; }
            public string LegalStatus { get; set; }
            public string SafegaurdReviewStatus { get; set; }
            public string IndependentSkillsStatus { get; set; }
            public string BehavioralSupoortStatus { get; set; }
            public string EducationalVocationalStatus { get; set; }
            public string SelfDirectedStatus { get; set; }
            public string TransitionPlanningStatus { get; set; }
            public string GeneralStatus { get; set; }
            public string SafetyRiskStatus { get; set; }
            public string DepressionScreeningStatus { get; set; }
            public string SubstanceAbuseStatus { get; set; }
            public string SafetyPlanStatus { get; set; }
           

        }
        public class ComprehensiveAssessmentData
        {
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
        }
        public class NewVersionDetails
        {

        }
        public class MedicalMedicationDetails
        {
            public int MedicalMedicationId { get; set; }
            public int AssessmentMedicalId { get; set; }
            public string MedicationDosage { get; set; }
        }
        public class MedicalHealthMedicationsDetails
        {
            public int MedicalHealthMedicationsId { get; set; }
            public int AssessmentMedicalHelathId { get; set; }
            public string MedicationDosage { get; set; }
        }
        public class MedicalDiagnosisDetails
        {
            public int MedicalDiagnosisId { get; set; }
            public int AssessmentMedicalId { get; set; }
            public string LatestResult { get; set; }
            public int? Condition { get; set; }
            public string LastResultDate { get; set; }
            public string ConditionText { get; set; }
        }
        public class LegalCourtDateDetails
        {
            public int LegalCourtDateId { get; set; }
            public int AssessmentLegalId { get; set; }
            public string LegalDate { get; set; }
            public string LegalDateDetails { get; set; }
        }
        public class HousingSubsidyDetails
        {
            public int HousingSubsidyId { get; set; }
            public int AssessmentHousingId { get; set; }
            public int HousingSubsidyType { get; set; }
            public string HousingSubsidy { get; set; }
            public string RecievesDetailsCase { get; set; }
            public string OtherDetail { get; set; }
            public string Pending { get; set; }

            public string Status { get; set; }
        }
        public class FinancialMemberStatusDetails
        {
            public int FinancialMemberStatusId { get; set; }
            public int AssessmentFinancialId { get; set; }
            public int EntitlementsType { get; set; }
            public string Entitlements { get; set; }
            public string RecievesAmount { get; set; }
            public string RecertificationDate { get; set; }
            public string StableNoNeeds { get; set; }
            public string Status { get; set; }
        }
        public class FinancialMemberNeedDetails
        {
            public int FinancialMemberNeedId { get; set; }
            public int AssessmentFinancialId { get; set; }
            public int FinancialElementsType { get; set; }
            public string FinancialElement { get; set; }
            public string AssisstanceNeeded { get; set; }
            public string StableNoNeeds { get; set; }
        }
        public class DomesticViolanceMemberRelationshipDetails
        {
            public int DomesticViolanceMemberRelationshipId { get; set; }
            public int AssessmentDomesticViolanceId { get; set; }
            public string Name { get; set; }
            public string Relationship { get; set; }
            public string Status { get; set; }
        }
        public class ComprehensiveAssessmentDetails
        {
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ClientId { get; set; }
            public int CompanyId { get; set; }
            public string DateOfBirth { get; set; }
            public string Age { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string PhoneNumber { get; set; }
            public string DateOfAssessment { get; set; }
            public string RelationshipStatus { get; set; }
            public string Gender { get; set; }
            public string SexualOrientation { get; set; }
            public string Ethnicity { get; set; }
            public string Race { get; set; }
            public string LanguagesSpoken { get; set; }
            public string Reading { get; set; }
            public string Writing { get; set; }
            public string SupportNeeded { get; set; }
            public string Medicaid_Seq { get; set; }
            public string MCO { get; set; }
            public string Verified { get; set; }
            public string SS { get; set; }
            public int? ReachedBy { get; set; }

           public  bool? LatestVersion { get; set; }

            public string DocumentVersion { get; set; }

            public string DocumentStatus { get; set; }
        }
        public class VersioningDetails
        {
            public int AssessmentVersioningId { get; set; }
            public int CompanyId { get; set; }
            public float DocumentVersion { get; set; }
            public string VersionDate { get; set; }
            public int DocumentStatus { get; set; }
        }
        public class TransitionPlanningDetails
        {
            public int AssessmentTransitionPlanningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int CompanyId { get; set; }
            public bool? Vocational { get; set; }
            public string VocationalDetails { get; set; }
            public bool? PrevocationalSkills { get; set; }
            public string PrevocationalSkillsDetails { get; set; }
            public bool? HistoryOfEmployment { get; set; }
            public string HistoryOfEmploymentDetails { get; set; }
            public bool? AccessVR { get; set; }
            public string AccessVRDetails { get; set; }
            public bool? AccessToVocationalRehabilitation { get; set; }
            public string AccessToVocationalRehabilitationDetails { get; set; }
            public bool? TicketToWork { get; set; }
            public string TicketToWorkDetails { get; set; }
            public bool? WelfareToWork { get; set; }
            public string WelfareToWorkDetails { get; set; }
            public bool? DayActivitiesOverAge21 { get; set; }
            public string DayActivitiesOverAge21Details { get; set; }
            public bool? CompetitiveEmployment { get; set; }
            public string CompetitiveEmploymentDetails { get; set; }
            public bool? SEMP { get; set; }
            public string SEMPDetails { get; set; }
            public bool? EmploymentNotIntegrated { get; set; }
            public string EmploymentNotIntegratedDetails { get; set; }
            public bool? PathwayToEmployment { get; set; }
            public string PathwayToEmploymentDetails { get; set; }
            public bool? SiteBasedPrevocationalServices { get; set; }
            public string SiteBasedPrevocationalServicesDetails { get; set; }
            public bool? CommunityBasedPrevocationalServices { get; set; }
            public string CommunityBasedPrevocationalServicesDetails { get; set; }
            public bool? SelfDirectedIndividualizedBudget { get; set; }
            public string SelfDirectedIndividualizedBudgetDetails { get; set; }
            public bool? DayHabilitation { get; set; }
            public string DayHabilitationDetails { get; set; }
            public bool? DayHabilitationWithoutWalls { get; set; }
            public string DayHabilitationWithoutWallsDetails { get; set; }
            public bool? DayTreatment { get; set; }
            public string DayTreatmentDetails { get; set; }
            public bool? CommunityHabilitation { get; set; }
            public string CommunityHabilitationDetails { get; set; }
            public bool? CommunityFirstChoiceOption { get; set; }
            public string CommunityFirstChoiceOptionDetails { get; set; }
            public bool? MentalHealthProgram { get; set; }
            public string MentalHealthProgramDetails { get; set; }
            public bool? AdultDayServices { get; set; }

            public string AdultDayServicesDetails { get; set; }
            public bool? Volunteer { get; set; }
            public string VolunteerDetails { get; set; }
            public bool? Retired { get; set; }
            public string RetiredDetails { get; set; }
            public bool? NoStructuredDaytimeActivity { get; set; }
            public string NoStructuredDaytimeActivityDetails { get; set; }
            public bool? AdultEducation { get; set; }
            public string AdultEducationDetails { get; set; }
            public bool? InterestInServices { get; set; }
            public string InterestInServicesDetails { get; set; }
            public string Status { get; set; }
        }
        public class SubstanceAbuseScreeningDetails
        {
            public int AssessmentSubstanceAbuseScreeningId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int CompanyId { get; set; }
            public bool? UsedAlcohalOrOtherDrugs { get; set; }
            public bool? UsedPrescriptionOrMedication_Drugs { get; set; }
            public bool? FeltUseTooMuchAlcoholOrOther_Drugs { get; set; }
            public bool? TriedToCutDown_QuitDrinking_OtherDrug { get; set; }
            public bool? GoneToAnyoneForHelpBcozOfYourDrinking { get; set; }
            public char? BlackoutsOrOtherPeriods { get; set; }
            public char? InjuredHeadAfterDrinking { get; set; }
            public char? Convulsions_Delirium_Tremens { get; set; }
            public char? HepatitisOrOtherLiverProblems { get; set; }
            public char? Felt_sick_shakyOrDepressed { get; set; }
            public char? FeltCoke_BugsOrCrawlingFeeling { get; set; }
            public char? InjuredAfterDrinkingOrUsing { get; set; }
            public char? UsedNeedlesToShootDrugs { get; set; }
            public bool? DrinkingUseCausedProblemsWithYourFamily { get; set; }
            public bool? DrinkingUseCausedProblemsAtSchool_Work { get; set; }
            public bool? ArrestedOrOtherLegalProblems { get; set; }
            public bool? LostTemper_FightsWhileDrinking { get; set; }
            public bool? NeedingToDrinkOrUseDrugsMore { get; set; }
            public bool? TryingToGetAlcoholOrDrugs { get; set; }
            public bool? BreakRules_Laws { get; set; }
            public bool? FeelBadOrGuilty { get; set; }
            public bool? EverDrinkingOrOtherDrugProblem { get; set; }
            public bool? FamilyMembersEverDrinkingOrDrugProblem { get; set; }
            public bool? FeelDrinkingOrDrugProblemNow { get; set; }
            public int? TotalScore { get; set; }
            public string Status { get; set; }
        }
        public class SelfDirectedServicesDetails
        {
            public int AssessmentSelfDirectedServicesId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            //    public int CompanyId { get; set; }
            public bool? InterestToSelfDirectServices { get; set; }
            public bool? EducationOnSelfDirecting { get; set; }
            public string ServicesToSelfDirect { get; set; }
            public string SkillsAndResources { get; set; }
            public string IdentifyBarriersToSelfDirecting { get; set; }
            public string Status { get; set; }
        }
        public class SafetyRiskDetails
        {
            public int AssessmentSafetyRiskId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            //  public int CompanyId { get; set; }
            public bool? EverThoughtsHurtingYourself { get; set; }
            public string EverThoughtsHurtingYourselfDetail { get; set; }
            public bool? EverActedThoughtsHurtYourself { get; set; }
            public string EverActedThoughtsHurtYourselfDetail { get; set; }
            public string FeelingWellOrNeedHelp { get; set; }
            public bool? ThoughtsOfHarmingYourself { get; set; }
            public string ThoughtsOfHarmingYourselfDetail { get; set; }
            public bool? HavePlanToEndYourLife { get; set; }
            public string PlanToEndYourLife { get; set; }
            public bool? NearFutureActOnThatPlan { get; set; }
            public string NearFutureActOnThatPlanDetail { get; set; }
            public bool? EverAttemptedSuicideBefore { get; set; }
            public string EverAttemptedSuicideBeforeDetail { get; set; }
            public bool? EverCommittedSuicideInFamily { get; set; }
            public string EverCommittedSuicideInFamilyDetail { get; set; }
            public bool? FeltLikeHarmingYourself { get; set; }
            public string FeltLikeHarmingYourselfDetail { get; set; }
            public bool? HearVoicesToHarmOrKillYourself { get; set; }
            public string HearVoicesToHarmOrKillYourselfDetail { get; set; }
            public bool? FeelLikeThatActOnThoseVoices { get; set; }
            public string FeelLikeThatActOnThoseVoicesDetail { get; set; }
            public string Status { get; set; }
        }
        public class SafetyPlanDetails
        {
            public int AssessmentSafetyPlanId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            //  public int CompanyId { get; set; }
            public bool? LeavePerpetrator { get; set; }
            public bool? OrderProtection { get; set; }
            public string OrderProtectionDetail { get; set; }
            public bool? ChildrenInvolved { get; set; }
            public bool? ChildrenBeingAbused { get; set; }
            public bool? EverBeenAbused { get; set; }
            public bool? SafewayToLeave { get; set; }
            public string LeaveHomeExit { get; set; }
            public bool? IsMemberTrust_LeaveDocument { get; set; }
            public string ImpDocumenthas { get; set; }
            public string ImpDocumenthas_Address { get; set; }
            public bool? IsSafePlace_PersonToTakecareThings { get; set; }
            public string LeaveMyhomewillContact { get; set; }
            public bool? IsChildrenKnowToDial911 { get; set; }
            public string DirectionsOfMembersAddress { get; set; }
            public bool? IsSafePlaceInHomeToHide { get; set; }
            public string SafePlaceInHome { get; set; }
            public string Status { get; set; }
        }
        public class MedicalHelathDetails
        {
            public int AssessmentMedicalHelathId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            // public int CompanyId { get; set; }
            public char? BipolarDisorder { get; set; }
            public char? Schizophrenia { get; set; }
            public char? SevereDepression { get; set; }
            public char? SchizoaffectiveDisorder { get; set; }
            public char? NoneOfTheAbove { get; set; }
            public string TreatmentReceived { get; set; }
            public string ProvidersDetail { get; set; }
            public string MemberSeeProvidersList { get; set; }
            public char? ConfusionAboutMedicine { get; set; }
            public char? InstructionLanguage { get; set; }
            public char? DifficultyInMedication { get; set; }
            public char? MedicationSideEffects { get; set; }
            public char? UnderstandingOfMedicine { get; set; }
            public char? RememberingMedicine { get; set; }
            public char? OtherBarriers { get; set; }
            public string OherBarriersDescription { get; set; }
            public string MemberReactionOnMedication { get; set; }
            public string MemberReactionNotOnMedication { get; set; }
            public string AllergyPsychiatricMedications { get; set; }
            public string MemberBeenToED { get; set; }
            public string RecentVisitDate { get; set; }
            public string NoOfTimesMemberAdmitted { get; set; }
            public string DateOfRecentAdmission { get; set; }
            public string AgeOnsetSymptoms { get; set; }
            public string OtherDetailsPsychiatricHistory { get; set; }
            public bool? MemberExperiencedTrauma { get; set; }
            public string MemberExperiencedDescribe { get; set; }
            public bool? MemberRecievedHelp { get; set; }
            public bool? MemberWishToReffered { get; set; }
            public bool? PreviousDrugTreatment { get; set; }
            public string PreviousDrugTreatmentDetail { get; set; }
            public string MemberRecievesKindTreatment { get; set; }
            public string ListDetailsProviders { get; set; }
            public string NoOfTimesMemberSeeProviderList { get; set; }
            public string MemberAdmittedToDetoxTreatment { get; set; }
            public string DateRecentAdmission { get; set; }
            public string NoOfTimesMemberAdmittedRehab { get; set; }
            public string DateMostRecentVisit { get; set; }
            public string AgeOfFirstSubstance { get; set; }
            public string DurationOfSoberity { get; set; }
            public string TreatmentModalityEffective { get; set; }
            public string SubstanceUseTriggers { get; set; }
            public string MemberProtectItself { get; set; }
            public string PerceptionGoodAboutSubstance { get; set; }
            public string PerceptionNoGoodAboutSubstance { get; set; }
            public string OtherDetailSubstance { get; set; }
            public bool? DoesMemberUseTobacco { get; set; }
            public string MemberUseTobaccoSpecify { get; set; }
            public string NoOfTimeMemberSmokePerDay { get; set; }
            public bool? MemberCurrentlyUseECigarettes { get; set; }
            public bool? MemberCompletedCessationProgram { get; set; }
            public bool? MemberInteresedInProgram { get; set; }
            public bool? ReferralMade { get; set; }
            public string Status { get; set; }
        }
        public class MedicalDetails
        {
            public int AssessmentMedicalId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            //  public int CompanyId { get; set; }
            public int? MedicationsOnTime { get; set; }
            public int? MedicationMissed { get; set; }
            public int? LastMedicationMissed { get; set; }
            public int? Score { get; set; }
            public bool? ConsequencesDoses { get; set; }
            public bool? MedicationsInfo { get; set; }
            public string HospitalizationReason { get; set; }
            public string AllergiesList { get; set; }
            public bool? MemberHavePain { get; set; }
            public string PainArea { get; set; }
            public string PainOccurance { get; set; }
            public string PainFeeling { get; set; }
            public string PainReporting { get; set; }
            public string PainScaleWorst { get; set; }
            public string PainScaleBest { get; set; }
            public string PainMakesBetter { get; set; }
            public string PainMakeWorse { get; set; }
            public string PainReliefMethods { get; set; }
            public string PainAffects { get; set; }
            public string MemberSharingHealth { get; set; }
            public string PrimaryCareProvider { get; set; }
            public string Psychiatrist { get; set; }
            public string PainManagementPhysician { get; set; }
            public string Status { get; set; }
        }
        public class LegalDetails
        {
            public int AssessmentLegalId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            //public int CompanyId { get; set; }
            public string IncomeSupportExigentNeeds { get; set; }
            public string IncomeSupportOngoingLegalActivity { get; set; }
            public string IncomeSupportLegalHistory { get; set; }
            public string HousingUtilitiesExigentNeeds { get; set; }
            public string HousingUtilitiesOngoingLegalActivity { get; set; }
            public string HousingUtilitiesLegalHistory { get; set; }
            public string LegalStatusExigentNeeds { get; set; }
            public string LegalStatusOngoingLegalActivity { get; set; }
            public string LegalStatusLegalHistory { get; set; }
            public string PersonalAndFamilyExigentNeeds { get; set; }
            public string PersonalAndFamilyOngoingLegalActivity { get; set; }
            public string PersonalAndFamilyLegalActivity { get; set; }
            public bool? MemberRegisteredSexOffender { get; set; }
            public string MemberRegisteredSexOffenderDetail { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string County { get; set; }
            public bool? MemberEverIncarcerated { get; set; }
            public string MemberEverIncarceratedDetail { get; set; }
            public string ParoleOfficerName { get; set; }
            public string ParoleOfficerNumber { get; set; }
            public int? ParoleOfficerLengthOfTime { get; set; }
            public bool? ConsentToSpeakWithParoleOfficer { get; set; }
            public string ProbationOfficerName { get; set; }
            public string ProbationOfficerNumber { get; set; }
            public int? ProbationOfficerLenghtOfTime { get; set; }
            public bool? ConsentToSpeakWithProbationOfficer { get; set; }
            public bool? MemberHaveAttorney { get; set; }
            public string AttorneyNameAndContact { get; set; }
            public bool? ConsentToSpeakWithAttorney { get; set; }
            public bool? ClientNeedreferralLegalServices { get; set; }
            public bool? CourtOrderedServices { get; set; }
            public string CourtOrderedServicesDetail { get; set; }
            public bool? MemberAssisstancewithTransportation { get; set; }
            public string Status { get; set; }
        }
        public class IndependentLivingSkillsDetails
        {

            public int AssessmentIndependentLivingSkillsId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            // public int CompanyId { get; set; }
            public string LanguageSkills { get; set; }
            public string MemoryLearning { get; set; }
            public string AbilityToAction { get; set; }
            public string NeedsAssistanceEating { get; set; }
            public string MealPreparation { get; set; }
            public string HousekeepingCleanliness { get; set; }
            public string ManagingFinances { get; set; }
            public string ManagingMedications { get; set; }
            public string PhoneUse { get; set; }
            public string Transportation { get; set; }
            public string ProblematicSocialBehaviors { get; set; }
            public string HealthComponents { get; set; }
            public string IndividualHaveSupportToHelp { get; set; }
            public string DevelopmentalMilestones { get; set; }
            public string SelfPreservationSkills { get; set; }
            public string Status { get; set; }
        }
        public class HousingDetails
        {
            public int AssessmentHousingId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            // public int CompanyId { get; set; }
            public int? MemberCurrentlyLiving { get; set; }
            public int? HouseApartmentType { get; set; }
            public bool? LeaseOrMemberName { get; set; }
            public bool? PlaceofHouseStable { get; set; }
            public string FriendRelativeHomeDetail { get; set; }
            public string MemberRemainFriendRelativeHome { get; set; }
            public string MemberRemainParentImmediateGuardian { get; set; }
            public string MemberRemainRespiteCare { get; set; }
            public string MemberRemainHalfwayHouse { get; set; }
            public string MemberRemainHomelessStreet { get; set; }
            public string MemberRemainHomelessWithOthers { get; set; }
            public string MemberRemainHomelessRegistered { get; set; }
            public string SupportedHousingDetail { get; set; }
            public string MemberRemainsupportedHousing { get; set; }
            public bool? MemebrGiveConsentToSpeak { get; set; }
            public string TimeMemberResidingCurrentLocation { get; set; }
            public bool? ConcernsCurrentLiving { get; set; }
            public string ConcernsCurrentLivingDetails { get; set; }
            public bool? MemberRiskLoosingCurrentHousing { get; set; }
            public char RentArrears { get; set; }
            public string RentArrearsSpecifyAmount { get; set; }
            public char LossOfHousingSubsidy { get; set; }
            public char LandlordIssue { get; set; }
            public char Other { get; set; }
            public string OtherSpecify { get; set; }
            public bool? RecievedNoticeCityMarshal { get; set; }
            public string AddressLossOfHousing { get; set; }
            public bool? MemberInEvictionProceedings { get; set; }
            public bool? WorkingWithAttorney { get; set; }
            public bool? ConsentToSpeakAttorney { get; set; }
            public bool? APSInvolved { get; set; }
            public bool? OtherHousingOptions { get; set; }
            public string OtherHousingOptionsDetail { get; set; }
            public bool? AcceptReferralShelter { get; set; }
            public string Status { get; set; }
        }
        public class GeneralDetails
        {
            public int AssessmentGeneralId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            //  public int CompanyId { get; set; }
            public string Notes { get; set; }
            public int? ElectronicSignature { get; set; }
            public string Date { get; set; }
            public string StaffName { get; set; }
            public string StaffTitle { get; set; }
            public string StaffCredentials { get; set; }
            public string SignedDateTime { get; set; }
            public string Status { get; set; }

        }
        public class FinancialDetails
        {
            public int AssessmentFinancialId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            //   public int CompanyId { get; set; }
            public string MembersMonthlyIncome { get; set; }
            public string SourceOfIncome { get; set; }
            public string PeopleResideInHousehold { get; set; }
            public string PeopleResideInHouseholdDependents { get; set; }
            public string MonthlyCostOfRent { get; set; }
            public string Status { get; set; }
        }
        public class EducationalVocationalStatusDetails
        {
            public int AssessmentEducationalVocationalStatusId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            //  public int CompanyId { get; set; }
            public string Education { get; set; }
            public string LevelOfEducation { get; set; }
            public bool? InSchool { get; set; }
            public string PreSchoolInformation { get; set; }
            public string CurrentEducationalPlan { get; set; }
            public string CurrentServicesAndAccommodations { get; set; }
            public string MyDay { get; set; }
            public string Status { get; set; }

        }
        public class DomesticViolanceDetails
        {
            public int AssessmentDomesticViolanceId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            // public int CompanyId { get; set; }
            public bool? MemberFeelSafeWithPeople { get; set; }
            public string MemberFeelSafeWithPeopleDetail { get; set; }
            public bool? MemberNotFeelSafeWithPeople { get; set; }
            public string MemberNotFeelSafeWithPeopleDetail { get; set; }
            public bool? MemberVictimDomesticViolance { get; set; }
            public string MemberVictimDomesticViolanceDetail { get; set; }
            public bool? MemberFeelsLifeInDanger { get; set; }
            public string MemberFeelsLifeInDangerDetail { get; set; }
            public bool? UnderstandsDomesticViolance { get; set; }
            public bool? OtherPersonMakesAfraid { get; set; }
            public string OtherPersonMakesAfraidDetail { get; set; }
            public bool? DoneAnythingInLifePlan { get; set; }
            public string DoneAnythingInLifePlanDetail { get; set; }
            public string WhenPersonDisagreeAbove { get; set; }
            public bool? PhysicalFightingInRelationship { get; set; }
            public string PhysicalFightingInRelationshipDetail { get; set; }
            public bool? PoliceInvolvementInViolance { get; set; }
            public bool? ProtectionAgainstMember { get; set; }
            public bool? OtherPersonCriticizedMember { get; set; }
            public string OtherPersonCriticizedMemberDetial { get; set; }
            public bool? MemberEverInstitutionalization { get; set; }
            public string MemberEverInstitutionalizationDetail { get; set; }
            public bool? MemberIsolatedEverdayLiving { get; set; }
            public string MemberIsolatedEverdayLivingDetail { get; set; }
            public bool? ObligatedInSexWitihIdentifiedAbuser { get; set; }
            public string ObligatedInSexWitihIdentifiedAbuserDetail { get; set; }
            public bool? TouchedInSexualWithoutPermission { get; set; }
            public string TouchedInSexualWithoutPermissionDetail { get; set; }
            public bool? MemberMoneyUsedInappropriately { get; set; }
            public string MemberMoneyUsedInappropriatelyDetail { get; set; }
            public bool? ForcedToMakeFinancialDecisions { get; set; }
            public string ForcedToMakeFinancialDecisionsDetail { get; set; }
            public bool? ForcedToSignDocuments { get; set; }
            public string ForcedToSignDocumentsDetail { get; set; }

            public string Status { get; set; }
        }
        public class DepressionScreeningDetails
        {
            public int AssessmentDepressionScreeningId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int CompanyId { get; set; }
            public int? LittleInterestInDoingThings { get; set; }
            public int? FeelingDownOrDepressed { get; set; }
            public string Status { get; set; }
        }
        public class BehavioralSupportServicesDetails
        {
            public int AssessmentBehavioralSupportServicesId { get; set; }
            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int CompanyId { get; set; }
            public string ChallengingBehaviors { get; set; }
            public bool? BehaviorSupportPlan { get; set; }
            public string BehaviorSupportPlanDetails { get; set; }
            public string RestrictivePhysicalInterventions { get; set; }
            public string SkillsAndResources { get; set; }
            public string StrengthsOfIndividual { get; set; }
            public bool? IsEngagementInTreatmentPlan { get; set; }
            public string EngagementInTreatmentPlanDetails { get; set; }
            public string IdentifyBarriersToService { get; set; }
            public string Status { get; set; }
        }
        public class AreasSafeguardReviewDetails
        {
            public int AssessmentAreasSafeguardReviewId { get; set; }

            public int ComprehensiveAssessmentId { get; set; }
            public int AssessmentVersioningId { get; set; }
            public int CompanyId { get; set; }
            public bool? Choking { get; set; }
            public string ChokingDetails { get; set; }
            public bool? RiskOfFalls { get; set; }
            public string RiskOfFallsDetails { get; set; }
            public bool? SelfHarmBehaviors { get; set; }
            public string SelfHarmBehaviorsDetails { get; set; }
            public bool? FireSafety { get; set; }
            public string FireSafetyDetails { get; set; }
            public bool? SafetyInTheCommunity { get; set; }
            public string SafetyInTheCommunityDetails { get; set; }
            public bool? HousingFoodInstability { get; set; }
            public string HousingFoodInstabilityDetails { get; set; }
            public bool? EmergencyEvacuation { get; set; }
            public string EmergencyEvacuationDetails { get; set; }
            public bool? BackupPlanEmergencySituations { get; set; }
            public string BackupPlanEmergencySituationsDetails { get; set; }
            public bool? LevelOfIndependence { get; set; }
            public string LevelOfIndependenceDetails { get; set; }
            public bool? LevelOfSupervision { get; set; }
            public string LevelOfSupervisionDetails { get; set; }
            public string Status { get; set; }
        }

    }
    public class ComprehensiveAssessmentPDFResponse
    {
        public List<ComprehensiveAssessmentPDF> ComprehensiveAssessmentPDF { get; set; }
    }
    public class ComprehensiveAssessmentPDF
    {
        public string FileName { get; set; }

        public string DownloadFile { get; set; }
    }
}
