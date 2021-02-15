using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Response
{
  public  class CANSResponse: BaseResponse
    {
        public List<CommonCANSRsponse> CommonCANSRsponse { get; set; }
        public List<NewVersionDetails> NewVersionDetails { get; set; }
        public List<GeneralInformationSection> GeneralInformation { get; set; }
        public List<TraumaExposureSection> TraumaExposure { get; set; }
        public List<PresentingProblemAndImpactSection> PresentingProblemAndImpact { get; set; }
        public List<SafetySection> Safety { get; set; }
        public List<SubstanceUseHistorySection> SubstanceUseHistory { get; set; }
        public List<PlacementHistorySection> PlacementHistory { get; set; }
        public List<PsychiatricInformationSection> PsychiatricInformation { get; set; }
        public List<ClientStrengthsSection> ClientStrengths { get; set; }
        public List<FamilyInformationSection> FamilyInformation { get; set; }
        public List<NeedsResourceAssessmentSection> NeedsResourceAssessment { get; set; }
        public List<DSMDiagnosisSection> DSMDiagnosis { get; set; }
        public List<MentalHealthSummarySection> MentalHealthSummary { get; set; }
        public List<AddClientFunctioningEvaluationsSection> AddClientFunctioningEvaluations { get; set; }

        public List<IndividualTreatmentPlan> IndividualTreatmentPlan { get; set; }

        public List<CansSignature> CansSignature { get; set; }

        public List<GeneralInformationHRASection> GeneralInformationHRA { get; set; }
        public List<HealthStatusSection> HealthStatus { get; set; }
        public List<MedicationsSection> Medications { get; set; }
        public List<DevelopmentHistorySection> DevelopmentHistory { get; set; }
        public List<MedicalHistorySection> MedicalHistory { get; set; }
        public List<CaregiverAddendumSection> CaregiverAddendum { get; set; }
        public List<GeneralInformationDCFSSection> GeneralInformationDCFS { get; set; }
        public List<SexuallyAggrBehaviorSection> SexuallyAggrBehavior { get; set; }
        public List<ParentGuardSafetySection> ParentGuardSafety { get; set; }
        public List<ParentGuardWellbeingSection> ParentGuardWellbeing { get; set; }
        public List<ParentGuardPermananenceSection> ParentGuardPermananence { get; set; }
        public List<SubstituteCommitPermananenceSection> SubstituteCommitPermananence { get; set; }
        public List<IntactFamilyServiceSection> IntactFamilyService { get; set; }
        public List<IntensivePlacementStabilizationSection> IntensivePlacementStabilization { get; set; }

        public List<GeneralInfoEstabilishedSupportsDetailsSection> GeneralInfoEstabilishedSupportsDetails { get; set; }
        public List<GeneralInfoFamilyMembersDetailsSection> GeneralInfoFamilyMembersDetails { get; set; }
        public List<SubstanceAbuseTreatmentSection> SubstanceAbuseTreatment { get; set; }
        public List<OutpatientMentalHealthServicesSection> OutpatientMentalHealthServices { get; set; }
        public List<MedicationsSection> MedicationDetail { get; set; }
        public List<MedicalHistoryPsychHospitalSection> MedicalHistoryPsychHospital { get; set; }
        public List<MedicalHistoryAdditHospitalSection> MedicalHistoryAdditHospital { get; set; }
        public List<MedicalHistoryProviderSection> MedicalHistoryProvider { get; set; }
        public List<AllChildTables> AllChildTables { get; set; }
    }
    public class CommonCANSRsponse
    {
        public int GeneralInformationID { get; set; }

        public int CansVersioningID { get; set; }

        public bool? ValidatedRecord { get; set; }
        public int CansTreatmentPlanId { get; set; }
        
        public int CansTreatmentPlanGoalID { get; set; }

        public int CansTreatmentPlanItemID { get; set; }
        public string JSONData { get; set; }

        public string CansTreatmentPlan { get; set; }

        public int GeneralInfoFamilyMembersId { get; set; }
        public int GeneraInfoEstabilishedSupportsId { get; set; }
        public int PresentinProblemAndImpactID { get; set; }
        public string Status { get; set; }
        public int ClientStrengthID { get; set; }
        public int SafetyID { get; set; }
        public int SummaryPrioritzedNeedStrengthID { get; set; }
        public int FamilyInformationID { get; set; }
        public int DsmDiagnosisID { get; set; }
        public int CaregiverAddendumID { get; set; }
        public int AddClientFunctioningEvaluationID { get; set; }
        public int DevelopmentHistoryID { get; set; }
        public int CansSignatureID { get; set; }
        public int ExternalStaffAssignmentID { get; set; }
        public int ExternalProviderID { get; set; }
        public int NeedsResourceAssessmentID { get; set; }
        public int MentalHealthSummaryID { get; set; }
        public int SexuallyAggrBehaviorID { get; set; }
        public int IntensivePlacementStabilizationID { get; set; }
        public int MedicationID { get; set; }
        public int IntactFamilyServiceID { get; set; }
        public int IndividualTreatmentPlanID { get; set; }
        public int GeneralInformatioinHRAID { get; set; }
        public int MedicalHistoryID { get; set; }
        public int HealthStatusID { get; set; }
        public int ServiceIntervensionID { get; set; }
        public int GeneralInformationDCFSID { get; set; }
        public int PsychiatricInformationID { get; set; }
        public int SubstituteCommitPermananenceID { get; set; }
        public int ParentGuardPermananenceID { get; set; }
        public int OutpatientMentalHealthServicesID { get; set; }
        public int ParentGuardSafetyID { get; set; }
        public int ParentGuardWellbeingID { get; set; }
        public int SubstanceAbuseTreatmentID { get; set; }
        public int PlacementHistoryID { get; set; }
        public int SubstanceUseHistoryID { get; set; }
        public int TraumaExposureID { get; set; }
        public string JSONChildFirstData { get; set; }
        public string JSONChildSecondData { get; set; }
        public string JSONChildThirdData { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentVersion { get; set; }
        public string FileName { get; set; }
    }
    public class NewVersionDetails
    {
        public int GeneralInformationID { get; set; }

        public int CansVersioningID { get; set; }

        public int CansTreatmentPlanId { get; set; }

        public int CansTreatmentPlanGoalID { get; set; }

        public int CansTreatmentPlanItemID { get; set; }
        public string JSONData { get; set; }

        public string CansTreatmentPlan { get; set; }

        public string JSONTreatmentPlan { get; set; }

        public string JSONDSMData { get; set; }

        public string JSONServiceInterventionsData { get; set; }

        public string JSONFamilyMembersData { get; set; }

        public string JSONEstablishedData { get; set; }

        public string JSONSubstanceAbuseData { get; set; }

        public string JSONOutpatientData { get; set; }

        public string JSONMedicalData { get; set; }

        public string JSONHistoryPsychData { get; set; }

        public string JSONHistAddData { get; set; }

        public string JSONHistProviderData { get; set; }
        public string JSONNeedsStrengthData { get; set; }
        public string JSONServiceObjectiveData { get; set; }

    }

    public class GeneralInformationSection
    {
        public int? GeneralInformationID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? ClientID { get; set; }
        public int? CansType { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public string Rin { get; set; }
        public string Gender { get; set; }
        public string RefSource { get; set; }
        public DateTime? DateFirstCont { get; set; }
        public string Phone { get; set; }
        public string Language { get; set; }
        public string InterpreterServices { get; set; }
        public string SpokenLangDescription { get; set; }
        public string InterOther { get; set; }
        public string InterSpoken { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? County { get; set; }
        public bool? UsCitizen { get; set; }
        public string Race { get; set; }
        public string RaceOth { get; set; }
        public string Ethnicity { get; set; }
        public bool? InsurCoverage { get; set; }
        public string InsuranceCompany { get; set; }   
        public bool? DCFSInvolvement { get; set; }
        public bool? Caregiver { get; set; }
        public int? HouseHoldSize { get; set; }
        public int? HouseHoldIncome { get; set; }
        public string MaritalStatus { get; set; }
        public string GuardianStatus { get; set; }
        public int? GuardianStatusId { get; set; }
        public string GuardStatusOth { get; set; }
        public string EmploymentStatus { get; set; }
        public int? EmploymentStatusId { get; set; }
        public string LivingArrangement { get; set; }
        public int? LivingArrangementId { get; set; }
        public string livArrangeOther { get; set; }
        public string EducationLevel { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public int? RelationshipToClient { get; set; }
        public string ParentPhone { get; set; }
        public string ParentAddress { get; set; }
        public string ParentCity { get; set; }
        public string ParentZip { get; set; }
        public string ParentState { get; set; }
        public string ParentCounty { get; set; }
        public string EmergConFirstName { get; set; }
        public string EmergConLastName { get; set; }
        public string EcRelToClient { get; set; }
        public string EcAddress { get; set; }
        public string EcCity { get; set; }
        public string EcState { get; set; }
        public string EcZip { get; set; }
        public string EcPhone { get; set; }
        public string EmergencyCounty { get; set; }
        public string Status { get; set; }

        public int? InterpreterServicesId { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? EducationLevelId { get; set; }
        public int? RaceId { get; set; }

        public string DocumentStatus { get; set; }
        public string DocumentVersion { get; set; }
        public bool? LatestVersion { get; set; }

        public string ZipCode { get; set; }
    }
    public class TraumaExposureSection
    {
        public int? TraumaExposureID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? TeDisruptionsCaregiving { get; set; }
        public int? TeEmotionalAbuse { get; set; }
        public int? TeMedicalTrauma { get; set; }
        public int? TeNaturalDisaster { get; set; }
        public int? TeNeglect { get; set; }
        public int? TeParentalCriminalBehavior { get; set; }
        public int? TePhysicalAbuse { get; set; }
        public int? TeSexualAbuse { get; set; }
        public int? TeVictimCriminalActivity { get; set; }
        public int? TeWarTerrorismAffected { get; set; }
        public int? TeWitnessCommunityViolence { get; set; }
        public int? TewWitnessFamilyViolence { get; set; }
        public string TeSupportingInfo { get; set; }
        public string Status { get; set; }
    }
    public class PresentingProblemAndImpactSection
    {
        public int? PresentinProblemAndImpactID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? BeNeedAdjustTrauma { get; set; }
        public int? BeNeedAngerControl { get; set; }
        public int? BeNeedAntisocial { get; set; }
        public int? BeNeedAnxiety { get; set; }
        public int? BeNeedAtypical { get; set; }
        public int? BeNeedDepression { get; set; }
        public int? BeNeedEatingDist { get; set; }
        public int? BeNeedFailTrive { get; set; }
        public int? BeNeedImpulsivity { get; set; }
        public int? BeNeedInterpersonal { get; set; }
        public int? BeNeedMania { get; set; }
        public int? BeNeedOppositional { get; set; }
        public int? BeNeedPsychosis { get; set; }
        public int? BeNeedRegulatory { get; set; }
        public int? BeNeedSomatization { get; set; }
        public int? BeNeedSubstance { get; set; }
        public int? TraumaAttachment { get; set; }
        public int? TraumaAvoidance { get; set; }
        public int? TraumaDissaociation { get; set; }
        public int? TraumaDysregulation { get; set; }
        public int? TraumaGrief { get; set; }
        public int? TraumaHyperarousal { get; set; }
        public int? TraumaIntrusions { get; set; }
        public int? TraumaNumbing { get; set; }
        public int? LifeBasicActivities { get; set; }
        public int? LifeCommunication { get; set; }
        public int? LifeDecisionMaking { get; set; }
        public int? LifeDevelopmental { get; set; }
        public int? LifeElimination { get; set; }
        public int? LifeFamFunctioning { get; set; }
        public int? LifeFunctionalCommunication { get; set; }
        public int? LifeIndependentLiving { get; set; }
        public int? LifeIntimateRelationships { get; set; }
        public int? LifeJobFunctioning { get; set; }
        public int? LifeLegal { get; set; }
        public int? LifeLivingSituation { get; set; }
        public int? LifeLoneliness { get; set; }
        public int? LifeMedicalPhysical { get; set; }
        public int? LifeMedication { get; set; }
        public int? LifeMotor { get; set; }
        public int? LifeParentalRole { get; set; }
        public int? LifePersistence { get; set; }
        public int? LifeRecreation { get; set; }
        public int? LifeResidentialStability { get; set; }
        public int? LifeRoutines { get; set; }
        public int? LifeSchoolPreschoolDaycare { get; set; }
        public int? LifeSensory { get; set; }
        public int? LifeSexualDevelopment { get; set; }
        public int? LifeSleep { get; set; }
        public int? LifeSocialFunctioning { get; set; }
        public int? LifeTransportation { get; set; }
        public int? DDAutism { get; set; }
        public int? DDCognivtive { get; set; }
        public int? DDDevelopmental { get; set; }
        public int? DDMotor { get; set; }
        public int? DDRegulatory { get; set; }
        public int? DDSelfcare { get; set; }
        public int? DDSensory { get; set; }
        public int? SPDAchievement { get; set; }
        public int? SPDAttendance { get; set; }
        public int? SPDBehavior { get; set; }
        public int? SPDPreschoolDaycare { get; set; }
        public int? SPDTeacherRelationship { get; set; }
        public int? EmploymentCareerAspirations { get; set; }
        public int? EmploymentJobAttendance { get; set; }
        public int? EmploymentJobPerformance { get; set; }
        public int? EmploymentJobRelations { get; set; }
        public int? EmploymentJobSkills { get; set; }
        public int? EmploymentJobTime { get; set; }
        public int? ParentingInvolvement { get; set; }
        public int? ParentingKnowledgeOfNeeds { get; set; }
        public int? ParentingMaritalViolence { get; set; }
        public int? ParentingOrganization { get; set; }
        public int? ParentingSupervision { get; set; }
        public int? IndependentCommDeviceUse { get; set; }
        public int? IndependentHouseWork { get; set; }
        public int? IndependentHousingSafety { get; set; }
        public int? IndependentMealPrep { get; set; }
        public int? IndependentMoneyManagement { get; set; }
        public int? IndependentShopping { get; set; }
        public string PresentingpProbSuppInfo { get; set; }
        public string Status { get; set; }
    }
    public class SafetySection
    {
        public int? SafetyID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? Rbvictexpl { get; set; }
        public int? Rbselfharm { get; set; }
        public int? Rbflightrisk { get; set; }
        public int? Rbsuiciderisk { get; set; }
        public int? Rbintenmisb { get; set; }
        public int? Rbrunaway { get; set; }
        public int? Rbsexprobehav { get; set; }
        public int? Rbbullying { get; set; }
        public int? Rbdelcrimbehav { get; set; }
        public int? Rbselfmutil { get; set; }
        public int? Rbothselfharm { get; set; }
        public int? Rbdngrtothers { get; set; }
        public int? Rbfiresetting { get; set; }
        public int? Rbgrdisability { get; set; }
        public int? Rbhoarding { get; set; }
        public int? Runfrequency { get; set; }
        public int? Runconsistdest { get; set; }
        public int? runsafetydest { get; set; }
        public int? Runillegacts { get; set; }
        public int? Runreturnonown { get; set; }
        public int? Runinvolvothers { get; set; }
        public int? Runrealexpect { get; set; }
        public int? Runplanning { get; set; }
        public int? Spbhypersex { get; set; }
        public int? Spbhirisksexbeh { get; set; }
        public int? Spbmastur { get; set; }
        public int? Spbsexaggr { get; set; }
        public int? Spbsexreactbeh { get; set; }


        public bool? SPDEdTesting { get; set; }
        public bool? SPDCredRecovery { get; set; }
        public bool? SPDStudentStudyTeam { get; set; }
        public bool? SPD504Plan { get; set; }
        public bool? SPDIEP { get; set; }
        public bool? SPDTutoring { get; set; }




        public int? Sabrelationship { get; set; }
        public int? Sabphysforce { get; set; }
        public int? Sabplanning { get; set; }
        public int? Sabagediff { get; set; }
        public int? Sabpowerdifferential { get; set; }
        public int? Sabtypesexact { get; set; }
        public int? Sabresptoaccusation { get; set; }
        public int? Dangerousnesshostility { get; set; }
        public int? Dangerousnessparanthinking { get; set; }
        public int? Dangerousnessecondgainsanger { get; set; }
        public int? Dangerousnessviolenthinking { get; set; }
        public int? Dangerousnessintent { get; set; }
        public int? Dangerousnessplanning { get; set; }
        public int? Dangerousnessviolencehistory { get; set; }
        public int? Dangerousnessawareviolencepotential { get; set; }
        public int? Dangerousnessresptoconsequences { get; set; }
        public int? Dangerousnesscommitselfctrl { get; set; }
        public int? Firesetseriousness { get; set; }
        public int? Firesethistory { get; set; }
        public int? Firesetplanning { get; set; }
        public int? Firesetuseaccelerants { get; set; }
        public int? Firesetintentoharm { get; set; }
        public int? Firesetcommunsafety { get; set; }
        public int? Firesetresponsetoaccusation { get; set; }
        public int? Firesetremorse { get; set; }
        public int? Firesetlikelihoodfuturefireset { get; set; }
        public string Riskbehaviorsupportinfo { get; set; }
        public int? Justcrimeseriousness { get; set; }
        public int? Justcrimehistory { get; set; }
        public int? Justcrimearrests { get; set; }
        public int? Justcrimeplanning { get; set; }
        public int? Justcrimecommunsafety { get; set; }
        public int? Justcrimelegalcompliance { get; set; }
        public int? Justcrimepeerinfluences { get; set; }
        public int? Justcrimenvironinfluences { get; set; }
        public bool? Justcrimeust { get; set; }
        public bool? Justcrimengri { get; set; }
        public DateTime? Justcrimeustdate { get; set; }
        public DateTime? Justcrimengridate { get; set; }
        public string Justicesupportinginformation { get; set; }
        public string Safetyfactorscurrentenvironment { get; set; }
        public string Status { get; set; }
    }
    public class SubstanceUseHistorySection
    {
        public int? SubstanceUseHistoryID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? Substanceusehistseverity { get; set; }
        public int? Substanceusehistduration { get; set; }
        public int? Substanceusehiststageofrecovery { get; set; }
        public int? Substanceusehistenvironinfluences { get; set; }
        public int? Substanceusehistpeerinfluences { get; set; }
        public int? Substanceusehistparentinfluence { get; set; }
        public int? Substanceusehistrecovsupcommun { get; set; }
        public string Substanceusehistsuppinfo { get; set; }
        public bool? Subabusehisttreatment { get; set; }
        public string Status { get; set; }
    }
    public class PlacementHistorySection
    {
        public int? PlacementHistoryID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public bool? OutOfHomePlacementHistory { get; set; }
        public string PlacementHistory { get; set; }
        public string Status { get; set; }
    }
    public class PsychiatricInformationSection
    {
        public int? PsychiatricInformationID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string Psychiatricproblems { get; set; }
        public bool? Gmhhpriorpsychologicalassessment { get; set; }
        public DateTime? Gmhhpriorpsyschologicalassessmentdate { get; set; }
        public string Gmhhpriorpsychologicalassessmentiq { get; set; }
        public bool? Gmhhpriorpsychiatricevaluation { get; set; }
        public DateTime? Gmhhpriorpsychiatricevaluationdate { get; set; }
        public bool? Gmhhassessmentpsychologicaltesting { get; set; }
        public bool? Gmhhpsychiatricevaluation { get; set; }
        public bool? Gmhhprioroutpatientmentalhealthservices { get; set; }
        public string Mentalstatappearancebehavior { get; set; }
        public int? Mentalstathreatening { get; set; }
        public int? Mentalstatsuicidal { get; set; }
        public int? Mentalstathomicidal { get; set; }
        public int? Mentalstatimpulsecontrol { get; set; }
        public int? Mentalstathallucinatory { get; set; }
        public int? Mentalstatdelusional { get; set; }
        public int? Mentalstatjudgement { get; set; }
        public int? Mentalstatmemory { get; set; }
        public bool? Mentalstatexpansive { get; set; }
        public bool? Mentalstatlabile { get; set; }
        public bool? Mentalstatmoodangry { get; set; }
        public bool? Mentalstatmoodwnl { get; set; }
        public bool? Mentalstatmooddepressed { get; set; }
        public bool? Mentalstatmoodanxious { get; set; }
        public bool? Mentalstatmoodmanic { get; set; }
        public bool? Mentalstataffectangry { get; set; }
        public bool? Mentalstataffectwnl { get; set; }
        public bool? Mentalstatconstricted { get; set; }
        public bool? Mentalstatflat { get; set; }
        public bool? Mentalstatinappropriate { get; set; }
        public bool? Mentalstatsad { get; set; }
        public bool? Mentalstatdepressed { get; set; }
        public bool? Mentalstatmanic { get; set; }
        public bool? Mentalstatanxious { get; set; }
        public int? Mentalstatinsight { get; set; }
        public int? Mentalstatorientation { get; set; }
        public int? Mentalstatcognition { get; set; }
        public string Status { get; set; }
    }
    public class ClientStrengthsSection
    {
        public int? ClientStrengthID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? ClientStrengthsFamilySupport { get; set; }
        public int? ClientStrengthsInterSocialConnect { get; set; }
        public int? ClientStrengthsNaturalSupp { get; set; }
        public int? ClientStrengthSpiritualReligious { get; set; }
        public int? ClientStrengthsEducatSetting { get; set; }
        public int? ClientStrengthsRelatPermanence { get; set; }
        public int? ClientStrengthsResiliency { get; set; }
        public int? ClientStrengthsOptimism { get; set; }
        public int? ClientStrengthsTalentsInterests { get; set; }
        public int? ClientStrengthsCultIdentity { get; set; }
        public int? ClientStrengthsCommunConnection { get; set; }
        public int? ClientStrengthsInvolveCare { get; set; }
        public int? ClientStrengthsVocational { get; set; }
        public int? ClientStrengthsJobHistVolunteer { get; set; }
        public int? ClientStrengthSelfCare { get; set; }
        public string ClientStrengthSupportingInformation { get; set; }
        public string Status { get; set; }
    }
    public class FamilyInformationSection
    {
        public int? FamilyInformationID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string RelevantFamilyHistory { get; set; }
        public int? CulturalFactorsLanguage { get; set; }
        public int? CulturalFactorsStress { get; set; }
        public int? CulturalFactorsTraditionsRituals { get; set; }
        public string CulturalFactorSupportingInformation { get; set; }
        public string Status { get; set; }
    }

    public class NeedsResourceAssessmentSection
    {
        public int? NeedsResourceAssessmentID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public bool? NeedsAssessmentNone { get; set; }
        public bool? NeedsAssessmentAccessToFood { get; set; }
        public bool? NeedsAssessmentClothing { get; set; }
        public bool? NeedsAssessmentEducationalTesting { get; set; }
        public bool? NeedsAssessmentEmployment { get; set; }
        public bool? NeedsAssessmentFinancialAssistance { get; set; }
        public bool? NeedsAssessmentImmigrationAssistance { get; set; }
        public bool? NeedsAssessmentLegalAssistance { get; set; }
        public bool? NeedsAssessmentMentalHealthService { get; set; }
        public bool? NeedsAssessmentMentoring { get; set; }
        public bool? NeedsAssessmentOther { get; set; }
        public string NeedsAssessmentOtherDescription { get; set; }
        public bool? NeedsAssessmentPhysicalHealth { get; set; }
        public bool? NeedsAssessmentShelter { get; set; }
        public string Status { get; set; }
    }

    public class DSMDiagnosisSection
    {
        public int? DsmDiagnosisID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? DiagnosticCode { get; set; }
        public string ICD5Name { get; set; }
        public string ICD10Name { get; set; }
        public bool? Diagnosis { get; set; }
        public string Status { get; set; }
    }
    public class MentalHealthSummarySection
    {
        public int? MentalHealthSummaryID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string MentalHealthSummary { get; set; }
        public string Status { get; set; }
    }
    public class AddClientFunctioningEvaluationsSection
    {
        public int? AddClientFunctioningEvaluationID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public bool? NoAdditionalEvaluations { get; set; }
        public string AdditionalEvaluations { get; set; }
        public string Status { get; set; }
    }

    public class IndividualTreatmentPlan
    {
        public int? IndividualTreatmentPlanID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }

        public string TreatmentVisionStatement { get; set; }

        public string ClientServicePreferences { get; set; }

        public DateTime? TreatmentPlanDate { get; set; }
        public string Status { get; set; }
    }

    public class CansSignature
    {
        public int? CansSignatureID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string ClientName { get; set; }

        public byte[] ClientSignature { get; set; }

        public string  ClientSignatureDate { get; set; }

        public string ParentLegalGuardianName { get; set; }

        public byte[] ParentLegalGuardianSignature { get; set; }

        public string ParentLegalGuardianSignatureDate { get; set; }

        public string LPHAApprovalName { get; set; }

        public string LPHASignature { get; set; }

        public string LPHAStaffName { get; set; }
        public string LPHAStaffTitle { get; set; }

        public string  LPHASignedDateTime { get; set; }

        public string QMHPF2F { get; set; }

        public string QMHPSignature { get; set; }

        public string QMHPStaffName { get; set; }
        public string QMHPStaffTitle { get; set; }

        public string QMHPSignedDateTime { get; set; }

        public string MHPName { get; set; }

        public string MHPSigature { get; set; }

        public string MHPStaffName { get; set; }
        public string MHPStaffTitle { get; set; }

        public string MHPSignedDateTime { get; set; }
        public string Status { get; set; }
    }
    public class GeneralInformationHRASection
    {
        public int? GeneralInformatioinHRAID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? ClientID { get; set; }
        public string StaffName { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public int? RecipientId { get; set; }
        public DateTime? ClientDateOfBirth { get; set; }
        public string ClientGender { get; set; }
        public string ClientHeightFeet { get; set; }
        public string ClientHeightInches { get; set; }
        public string ClientWeight { get; set; }
        public string PrimaryPhysicianName { get; set; }
        public DateTime? LastPhysicalExamDate { get; set; }
        public int? PhysicalExamDue { get; set; }
        public DateTime? LastFluShotDate { get; set; }
        public string Status { get; set; }
    }
    public class HealthStatusSection
    {
        public int? HealthStatusID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? ClientSelfReportedPhysicalHealth { get; set; }
        public int? DailySnackIntake { get; set; }
        public int? DailyFruitVegetableIntake { get; set; }
        public int? RegularPhysicalActivity { get; set; }
        public string PhysicalActivityFrequency { get; set; }
        public int? TobaccoUse { get; set; }
        public int? AlcoholUse { get; set; }
        public string AlcoholConsumptionFrequency { get; set; }
        public string  AlcoholConsumptionNumber { get; set; }
        public int? FaintingHistory { get; set; }
        public string FaintingDescription { get; set; }
        public int? KnownAllergy { get; set; }
        public string AllergyDescription { get; set; }
        public int? FallingHistory { get; set; }
        public string FallingDescription { get; set; }
        public int? RequestQuitSmoking { get; set; }
        public int? HealthConcerns { get; set; }
        public string HealthConcernsDescription { get; set; }
        public int? GeneralIllness { get; set; }
        public string GeneralIllnessDescription { get; set; }
        public int? BreathingIssueMedicated { get; set; }
        public int? BreathingIssues { get; set; }
        public int? BreathingIssuesCause { get; set; }
        public string BreathingIssuesCauseDescription { get; set; }
        public int? HeadInjury { get; set; }
        public DateTime? HeadInjuryDate { get; set; }
        public int? MemoryLapses { get; set; }
        public int? CurrentDateAware { get; set; }
        public int? AboveAverageUrination { get; set; }
        public int? AboveAverageThirst { get; set; }
        public int? SpecialBloodSugarDiet { get; set; }
        public string SpecialDietDescription { get; set; }
        public int? BloodSugarMedicated { get; set; }
        public int? ChronicPain { get; set; }
        public int? PainMedicationHistory { get; set; }
        public int? PainMedicationCategory { get; set; }
        public string PainMedicationDescription { get; set; }
        public string PainIntensityLocationDescription { get; set; }
        public int? SexuallyActive { get; set; }
        public int? STDProtection { get; set; }
        public DateTime? LastSTDTestDate { get; set; }
        public int? STDDiagnosed { get; set; }
        public string STDDiagnosisDescription { get; set; }
        public int? WomenHealthProviderVisit { get; set; }
        public DateTime? WomenHealthProviderVisitDate { get; set; }
        public int? MenstrualCycleorMenopauseIssue { get; set; }
        public string MenstrualCycleorMenopauseDescription { get; set; }
        public int? PregnancyHistory { get; set; }
        public string PregnancyOutcomeDescription { get; set; }
        public string Status { get; set; }
        public string RegularPhysicalActivityText { get; set; }
        public string TobaccoUseText { get; set; }

        public string AlcoholUseText { get; set; }

        public string FaintingHistoryText { get; set; }
        public string KnownAllergyText { get; set; }
        public string FallingHistoryText { get; set; }
        public string RequestQuitSmokingText { get; set; }
        public string HealthConcernsText { get; set; }
        public string GeneralIllnessText { get; set; }
        public string BreathingIssueMedicatedText { get; set; }
        public string BreathingIssuesText { get; set; }
        public string HeadInjuryText { get; set; }
        public string MemoryLapsesText { get; set; }
        public string CurrentDateAwareText { get; set; }
        public string AboveAverageUrinationText { get; set; }
        public string AboveAverageThirstText { get; set; }
        public string SpecialBloodSugarDietText { get; set; }
        public string BloodSugarMedicatedText { get; set; }
        public string ChronicPainText { get; set; }
        public string SexuallyActiveText { get; set; }
        public string MenstrualCycleorMenopauseIssueText { get; set; }
        public string PainMedicationHistoryText { get; set; }
        public string STDDiagnosedText { get; set; }

        

    }
    public class MedicationsSection
    {
        public int? MedicationID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? PsychotropicMedicationUse { get; set; }
        public string MedicationCompliance { get; set; }
        public int? PsychotropicLabWork { get; set; }
        public string Status { get; set; }
    }
    public class DevelopmentHistorySection
    {
        public int? DevelopmentHistoryID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? ClientMotherPrenatalCare { get; set; }
        public int? ClientMotherPregnancyComplications { get; set; }
        public string ClientMotherPregnancyComplicationsDescription { get; set; }
        public int? ClientBirthStatus { get; set; }
        public int? ClientInUteroSubstanceExposure { get; set; }
        public string ClientInUteroSubstanceExposureDescription { get; set; }
        public int? ClientMotherLaborIssues { get; set; }
        public string ClientMotherLaborIssuesDescription { get; set; }
        public string ClientWeight { get; set; }
        public string DevelopmentMilestoneCrawlAge { get; set; }
        public string DevelopmentMilestoneWalkAge { get; set; }
        public string DevelopmentMilestoneTalkAge { get; set; }
        public string DevelopmentMilestoneToiletTrainedAge { get; set; }
        public int? FamilyHistoryBehavioralProblems { get; set; }
        public string SupportingClientHistoryDescription { get; set; }
        public string Status { get; set; }
    }
    public class MedicalHistorySection
    {
        public int? MedicalHistoryID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? EmergencyRoomFrequency { get; set; }
        public string EmergencyRoomVisitDescription { get; set; }
        public int? PsychiatricallyHospitalized { get; set; }
        public string PsychiatricallyHospitalizedText { get; set; }
        public string SupportingHospitalHistoryDescription { get; set; }
        public string Status { get; set; }
        public bool? IsAdditionalPagesNeeded { get; set; }
    }
    public class CaregiverAddendumSection
    {
        public int? CaregiverAddendumID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientRIN { get; set; }
        public string StaffCompletingForm { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string CaregiverFullName { get; set; }
        public string CaregiverRelationshipToClient { get; set; }
        public string CaregiverAddtlPrimary { get; set; }
        public int? CaregiverSupervision { get; set; }
        public int? CaregiverInvolvementWithCare { get; set; }
        public int? CaregiverKnowledge { get; set; }
        public int? CaregiverSocialResources { get; set; }
        public int? CaregiverFinancialResources { get; set; }
        public int? CaregiverResidentialStability { get; set; }
        public int? CaregiverMedicalPhysical { get; set; }
        public int? CaregiverMentalHealth { get; set; }
        public int? CaregiverSubstanceUse { get; set; }
        public int? CaregiverDevelopmental { get; set; }
        public int? CaregiverOrganization { get; set; }
        public int? CaregiverSafety { get; set; }
        public int? CaregiverFamilyStress { get; set; }
        public int? CaregiverMaritalPartnerViolence { get; set; }
        public int? CaregiverMilitaryTrans { get; set; }
        public int? CaregiverSelfcareLivingSkills { get; set; }
        public int? CaregiverEmployEducFunc { get; set; }
        public int? CaregiverLegalInvolvement { get; set; }
        public int? CaregiverFamRelationToSystem { get; set; }
        public int? CaregiverAccessToChildCare { get; set; }
        public int? CaregiverEmathyChildren { get; set; }
        public string CaregiverSupportingInformation { get; set; }
        public string Status { get; set; }
    }
    public class GeneralInformationDCFSSection
    {
        public int? GeneralInformationDCFSID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string DCFSYouthName { get; set; }
        public string DCFSRIN { get; set; }
        public string DCFSStaffCompletingForm { get; set; }
        public DateTime? DCFSCompletedDate { get; set; }
        public bool? DCFSInvlvYthInCare { get; set; }
        public bool? DCFSInvlvIntFam { get; set; }
        public bool? DCFSInvlvIPS { get; set; }
        public bool? DCFSInvolvement { get; set; }
        public string Status { get; set; }
    }
    public class SexuallyAggrBehaviorSection
    {
        public int? SexuallyAggrBehaviorID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? SXAGBhTemporalConsist { get; set; }
        public int? SXAGBhsHistsexAbuseBeh { get; set; }
        public int? SXAGbhSeveritySxAbuse { get; set; }
        public int? SXAGbhPriorTreatmnt { get; set; }
        public string SXAGbhSuppInfo { get; set; }
        public string Status { get; set; }
    }
    public class ParentGuardSafetySection
    {
        public int? ParentGuardSafetyID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? Pgsafediscipln { get; set; }
        public int? Pgsafehomecond { get; set; }
        public int? Pgsafefrsutrtoler { get; set; }
        public int? Pgsafemaltreatment { get; set; }
        public string Pgsafesuppinfo { get; set; }
        public string Status { get; set; }
    }
    public class ParentGuardWellbeingSection
    {
        public int? ParentGuardWellbeingID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? Pgwelltraumreact { get; set; }
        public int? Pgwellimpactownbeh { get; set; }
        public int? Pgwelleffctparntapprch { get; set; }
        public int? Pgwellindeplivskills { get; set; }
        public int? Pgwellcntctcasewrker { get; set; }
        public int? Pgwellresponsmaltrtmnt { get; set; }
        public int? Pgwellrelatwthabuser { get; set; }
        public string Pgwellsuppinfo { get; set; }
        public string Status { get; set; }
    }
    public class ParentGuardPermananenceSection
    {
        public int? ParentGuardPermananenceID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public int? Pgpermfamconnect { get; set; }
        public int? Pgpermpersonaltrtmnt { get; set; }
        public int? Pgpermparticvisit { get; set; }
        public int? Pgpermcommitreunifi { get; set; }
        public string Pgpermsuppinfo { get; set; }
        public string Status { get; set; }
    }
    public class SubstituteCommitPermananenceSection
    {
        public int? SubstituteCommitPermananenceID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public bool? Subcomitpermna { get; set; }
        public int? Subcomitpermcollabothprnt { get; set; }
        public int? Subcomitpermsupptpermplan { get; set; }
        public int? Subcomitperminclusythfstfam { get; set; }
        public string Subcomitpermsuppinfo { get; set; }
        public string Status { get; set; }
    }
    public class IntactFamilyServiceSection
    {
        public int? IntactFamilyServiceID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public bool? Intfamsvcna { get; set; }
        public int? Intfamsvccaregvrcollab { get; set; }
        public int? Intfamsvcfamconflict { get; set; }
        public int? Intfamsvcfamcommunic { get; set; }
        public int? Intfamsvcfamroleapprop { get; set; }
        public int? Intfamsvchomemaint { get; set; }
        public string Intfamsvcsuppinfo { get; set; }
        public string Status { get; set; }
    }
    public class IntensivePlacementStabilizationSection
    {
        public int? IntensivePlacementStabilizationID { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public bool? Ipsna { get; set; }
        public int? Ipsouthyrsincare { get; set; }
        public int? Ipsyouthplacemnthist { get; set; }
        public int? Ipssubcargvrknwythdevneeds { get; set; }
        public int? Ipssubcargvrdiscipline { get; set; }
        public int? Ipssubcargvrmngmntemot { get; set; }
        public string Ipssuppinfo { get; set; }
        public string Status { get; set; }
    }

    public class GeneralInfoFamilyMembersDetailsSection
    {
        public int? GeneralInfoFamilyMembersId { get; set; }
        public int? GeneralInformationID { get; set; }
        public string FamilyMemberName { get; set; }
        public string FamilyMemberAge { get; set; }
        public string FamilyMemberRelation { get; set; }
        public int? FamilyMemberInHome { get; set; }
    }

    public class GeneralInfoEstabilishedSupportsDetailsSection
    {
        public int? GeneraInfoEstabilishedSupportsId { get; set; }
        public int? GeneralInformationID { get; set; }
        public string EstabilishedSupports { get; set; }
        public int? EstabilishedSupportsType { get; set; }
        public string EsAgency { get; set; }
        public string EsContact { get; set; }
        public string EsPhone { get; set; }
        public string EsEmail { get; set; }
    }

    public class SubstanceAbuseTreatmentSection
    {
        public int? SubstanceAbuseTreatmentID { get; set; }
        public int? SubstanceUseHistoryID { get; set; }
        public string Subabusehistwhen { get; set; }
        public string Subabusehistwhere { get; set; }
        public string Subabusehistwithwhom { get; set; }
        public string Subabusehistreason { get; set; }
    }

    public class OutpatientMentalHealthServicesSection
    {
        public int? OutpatientMentalHealthServicesID { get; set; }
        public int? PsychiatricInformationID { get; set; }
        public string Mentalhealthhistwhen { get; set; }
        public string Mentalhealthhistwhere { get; set; }
        public string Mentalhealthhistwithwhom { get; set; }
        public string Mentalhealthhistreason { get; set; }
    }

    public class MedicationDetailSection
    {
        public int? MedicationDetailID { get; set; }
        public int? MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string MedicationPrescriberName { get; set; }
        public string MedicationDosage { get; set; }
        public DateTime? MedicationPrescriptionBeginDate { get; set; }
        public DateTime? MedicationPrescriptionEndDate { get; set; }
        public string MedicationIssues { get; set; }
    }

    public class MedicalHistoryPsychHospitalSection
    {
        public int? MedicalHistoryPsychHospitalID { get; set; }
        public int? MedicalHistoryID { get; set; }
        public string PsychHospitalName { get; set; }
        public string PsychHospitalLocation { get; set; }
        public string PsychHospitalizationDate { get; set; }
        public string ReasonHospitalizedPsych { get; set; }
    }

    public class MedicalHistoryAdditHospitalSection
    {
        public int? MedicalHistoryAdditHospitalID { get; set; }
        public int? MedicalHistoryID { get; set; }
        public string HospitalName { get; set; }
        public string HospitalLocation { get; set; }
        public string HospitalizationDate { get; set; }
        public string ReasonHospitalized { get; set; }
    }

    public class MedicalHistoryProviderSection
    {
        public int? MedicalHistoryProviderID { get; set; }
        public int? MedicalHistoryID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderServices { get; set; }
        public string ProviderSpecialty { get; set; }
    }

    public class AllChildTables
    {
        public string JSONFamilyMembersData { get; set; }
        public string JSONEstabilishedSupportsData { get; set; }
        public string JSONSubstanceAbuseTreatmentData { get; set; }
        public string JSONOutpatientMentalHealthServicesData { get; set; }
        public string JSONMedicationDetailData { get; set; }
        public string JSONMedicalHistoryPsychData { get; set; }
        public string JSONMedicalHistoryAdditData { get; set; }
        public string JSONMedicalHistoryProviderData { get; set; }
        public string JSONDiagnosisData { get; set; }

        public string JSONServiceInterventionsData { get; set; }

        public string JSONTreatmentPlanData { get; set; }

        public string JSONNeedsStrengthData { get; set; }

    }

    public class CANSAssessmentPDFResponse
    {
        public List<CANSAssessmentPDF> CANSAssessmentPDF { get; set; }
    }
    public class CANSAssessmentPDF
    {
        public string FileName { get; set; }

        public string DownloadFile { get; set; }
    }
}
