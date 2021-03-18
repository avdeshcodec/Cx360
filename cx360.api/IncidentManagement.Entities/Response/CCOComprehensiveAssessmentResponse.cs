using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace IncidentManagement.Entities.Response
{
    public class CCOComprehensiveAssessmentResponse : BaseResponse
    {
       
        public class CCOComprehensiveAssessmentDetailResponse
        {
            public string CCO_CompAssessment { get; set; }
            public List<CCO_CompAssessment> CO_CompAssessment => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            public string CCO_EligibilityInformation { get; set; }
            public List<CCO_CompAssessment> CO_EligibilityInformation => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_EligibilityInformation);

            public string CCO_Communication { get; set; }
            public List<CCO_CompAssessment> CO_Communication => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_Communication);

            //public string CCO_CompAssessment { get; set; }
            //public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);

            // public string CCO_CompAssessment { get; set; }
            // public List<CCO_CompAssessment> CO_CompAssessments => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this.CCO_CompAssessment);
            //public override List<CCO_CompAssessment> => JsonConvert.DeserializeObject<List<CCO_CompAssessment>>(this);

        }
        public class CCO_CompAssessment
        {
            public int CompAssessmentId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompanyId { get; set; }
            public int ClientId { get; set; }
            public string IndividualMiddleName { get; set; }
            public string IndividualSuffix { get; set; }
            public string Nickname { get; set; }
            public int TABSId { get; set; }
            public int MedicaidId { get; set; }
            public DateTime DateofBirth { get; set; }
            public string Gender { get; set; }
            public int PreferredGender { get; set; }
            public string Race { get; set; }
            public string Ethnicity { get; set; }
            public string PhoneNumber { get; set; }
            public string StreetAddress1 { get; set; }
            public string StreetAddress2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZIPCode { get; set; }
            public int LivingSituation { get; set; }
            public string WillowbrookStatus { get; set; }
            public int RepresentationStatus { get; set; }
            public int CABRepContact1 { get; set; }
            public int CABRepContact2 { get; set; }
            public string ExpectationsforCommunityInclusion { get; set; }
            public string HospitalStaffingCoverage { get; set; }
            public int UnknownCount { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_EligibilityInformation
        {
            public int EligibilityInformationId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public DateTime MCOEnrollmentDate { get; set; }
            public int MCOName { get; set; }
            public int OPWDDEligibility { get; set; }
            public DateTime ICFEligibilityDeterminationDate { get; set; }
            public DateTime MedicaidExpirationDate { get; set; }
            public DateTime HHConsentDate { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }

        public class CCO_Communication
        {
            public int CommunicationId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemExpressiveCommunicationSkill { get; set; }
            public int MemReceptiveCommunicationSkill { get; set; }
            public int MemPrimaryLanguage { get; set; }
            public int MemPrimarySpokenLanguage { get; set; }
            public int MemPrimaryWrittenLanguage { get; set; }
            public int MemAbleToReadPrimaryLanguage { get; set; }
            public int MemMultiLingual { get; set; }
            public int MemMultiLingualLanguages { get; set; }
            public bool Interpreter { get; set; }
            public bool Translator { get; set; }
            public bool InterpreterAndTranslator { get; set; }
            public bool NotApplicable { get; set; }
            public int MemWantToImproveCommunicate { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }

        }
        public class CCO_MemberProviders
        {
            public int MemberProviderId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public string PrimaryCarePhysician { get; set; }
            public string Dentist { get; set; }
            public string Psychiatrist { get; set; }
            public string Psychologist { get; set; }
            public string EyeDoctor { get; set; }
            public string Pharmacy { get; set; }
            public string Hospital { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_CircleofSupport
        {
            public int CircleofSupportId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int ProfessionalContactId { get; set; }
            public int UtilizeServices { get; set; }
            public int MemberSatisfied { get; set; }
            public int MemberDissatisfied { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }

        }
        public class CCO_GuardianshipAndAdvocacy
        {
            public int GuardianshipAndAdvocacyId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int WhoHelpMemberMakeDecisionInLife { get; set; }
            public int HowPersonHelpMemberMakeDecision { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_AdvancedDirectivesFuturePlanning
        {
            public int AdvancedDirectivesFuturePlanningId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemHaveHealthCareProxy { get; set; }
            public int HealthCareProxyName { get; set; }
            public int MemLearnAdvancedHealthProxies { get; set; }
            public int MemSurrogateDesMakingCommittee { get; set; }
            public int MemUtiCommitAproveBehavioralSupportPlan { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_IndependentLivingSkill
        {
            public int IndependentLivingSkillId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public string ExplainConsent { get; set; }
            public int IndvCurrentLevelOfHousingStability { get; set; }
            public bool Pest { get; set; }
            public bool Mold { get; set; }
            public bool LeadPaint { get; set; }
            public bool LackOfHeat { get; set; }
            public bool Oven { get; set; }
            public bool SmokeDetectorMissing { get; set; }
            public bool WaterLeakes { get; set; }
            public bool NoneOfTheAbove { get; set; }
            public int LevelOfPersonalHygiene { get; set; }
            public string ExplainPersonalHygiene { get; set; }
            public int LevelOfSupportForToiletingNeed { get; set; }
            public int IndvExperConstipationDiarrheaVomiting { get; set; }
            public int IndvExperForConstipationDiarrheaVomitingInLastMonths { get; set; }
            public int IndvBowelObstructionReqHospitalization { get; set; }
            public int SupportForConstipationConcern { get; set; }
            public string ExplainSupportResultConstipationNeed { get; set; }
            public int LevelOfSuppHandFaceWash { get; set; }
            public int ChooseAnsSupportForDentalOralCare { get; set; }
            public string ExplainSupportResultDentalOralCare { get; set; }
            public int LevelOfSuppTrimNails { get; set; }
            public int LevelOfSuppSneezeCough { get; set; }
            public int LevelOfSuppPPEMask { get; set; }
            public int LevelOfSuppMoveSafely { get; set; }
            public int SuppForRiskToFall { get; set; }
            public string ExplainSuppForRiskToFall { get; set; }
            public int FallenInLastThreeMonths { get; set; }
            public int HowManyTimeMemberFallenInPast { get; set; }
            public int ConcernForIndividualVision { get; set; }
            public string ExpConcernForIndividualVision { get; set; }
            public int ConcernForIndividualHearing { get; set; }
            public string ExpConcernForIndividualHearing { get; set; }
            public bool NoConcernForSkinIntegrity { get; set; }
            public bool ReqPositioningSchedule { get; set; }
            public bool ReqDailySkinInspection { get; set; }
            public bool ReqAdaptiveEquipment { get; set; }
            public bool ReqSkinBarrierCream { get; set; }
            public bool ProvideEducationWhereAppropriate { get; set; }
            public string ExplainSupportSkinIntegrity { get; set; }
            public bool NoConcernForNutritionalNeed { get; set; }
            public bool ReqConsistencyFood { get; set; }
            public bool ReqConsistencyFluid { get; set; }
            public bool ReqReduceCalorieDiet { get; set; }
            public bool ReqHighCalorieDiet { get; set; }
            public bool ReqFiberCalciumElementToDiet { get; set; }
            public bool ReqSweetSaltFatElementRemove { get; set; }
            public bool RestrictedFluid { get; set; }
            public bool EnteralNutrition { get; set; }
            public bool ReqDietarySupplement { get; set; }
            public bool ReqAssitMealPreparation { get; set; }
            public bool ReqEducation { get; set; }
            public bool ReqAssitMealPlanning { get; set; }
            public bool ReqSupervisionDuringMeal { get; set; }
            public bool AdapEquDuringMeal { get; set; }
            public bool IndvMaintAdequateDiet { get; set; }
            public string ExpSupptNutritionalCareNeed { get; set; }
            public int RiskForChoking { get; set; }
            public int SupptOnChokingAspiration { get; set; }
            public string ExpSupptResultChokingAspirationNeed { get; set; }
            public int SwallowingEvaluationNeed { get; set; }
            public int SupptThatIndvNeedOnAcidReflux { get; set; }
            public string ExpSupptResultAcidRefluxNeed { get; set; }
            public int SupptNeedForMealPreparation { get; set; }
            public int SupptNeedForMealPlanning { get; set; }
            public int IndvWorriedAboutFoodInPast { get; set; }
            public int IndvRanOutOfFoodInPast { get; set; }
            public int IndvElecGasOilWaterThreatedInPast { get; set; }
            public int LevelOfSupptForCleaning { get; set; }
            public int MoneyManagementNeedOfMember { get; set; }
            public string ExpAssistanceForBudgeting { get; set; }
            public bool MemLearnToManageOwnMoney { get; set; }
            public int MedicationPrescribedByProvider { get; set; }
            public int IndvAbilityAdministerMedication { get; set; }
            public int IndvNeedReminderForMedication { get; set; }
            public int MedicationReminderMethod { get; set; }
            public int TakingMedicationAsPrescribed { get; set; }
            public int IndvRefuseForMedication { get; set; }
            public string ExpIndvRefuseForMedication { get; set; }
            public string ExpSupptMedicationAdministration { get; set; }
            public int IndvAbleToAccessOwnPhone { get; set; }
            public int IndvAbleToCallEmergency { get; set; }
            public int IndvAbleToAccessInternet { get; set; }
            public int IndvCallApplicableContactInPhone { get; set; }
            public int IndvNeedTransportation { get; set; }
            public string ExpTransportationNeed { get; set; }
            public int IndvLackedForTransportationInPastMonths { get; set; }
            public int IndvLearnToDrive { get; set; }
            public int IndvWantVehicleOwnership { get; set; }
            public int IndvIndependentUsingTransportation { get; set; }
            public int ConcernsWithBehavior { get; set; }
            public string HowIndvMentalHealth { get; set; }
            public int IndvCommunicateHealthConcern { get; set; }
            public string ExpIndvAbilityCommHealthConcern { get; set; }
            public int IndvAttendAllHealthService { get; set; }
            public string ExpSupptIndvAttendAllHealthService { get; set; }
            public int IndvSuppToHelpADLS { get; set; }
            public int IndvDifficultyRememberingThings { get; set; }
            public bool FollowTwoStepInstruction { get; set; }
            public bool SpeakInFullSentence { get; set; }
            public bool PretendPlay { get; set; }
            public bool ImitateOther { get; set; }
            public bool DrawCircle { get; set; }
            public bool RunWithoutFalling { get; set; }
            public bool UpDownStepOneFootPerStep { get; set; }
            public int IndvRecvPreschoolService { get; set; }
            public int IndvHaveFireSafetyNeed { get; set; }
            public string ExpIndvFireSafetyConcern { get; set; }
            public int IndvHaveInfoAboutFireStartStoppedEtc { get; set; }
            public int IndvEvacuateDuringFire { get; set; }
            public string ExpAbilityToMaintainSafetyInEmergency { get; set; }
            public int IsBackupPlanWhenNoHCBSProvider { get; set; }
            public int SupervisionNeedOfTheMember { get; set; }
            public string ExpSupervisionNeed { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }

        }
        public class CCO_SocialServiceNeed
        {
            public int SocialServiceNeedId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int IndvRepresentativePay { get; set; }
            public int TypeOfRepresentativePay { get; set; }
            public bool SocialSecurity { get; set; }
            public bool SSI { get; set; }
            public bool SSDI { get; set; }
            public bool DisabledAdultChild { get; set; }
            public bool OtherFinancialResource { get; set; }
            public int IndvPrivateInsuranceProvider { get; set; }
            public string IndvInsurerName { get; set; }
            public string PrivateInsurerId { get; set; }
            public bool HUDVoucher { get; set; }
            public bool ISSHousingSubsidy { get; set; }
            public bool OtherHousingAssistance { get; set; }
            public int MemInvolCriminalJusticeSystem { get; set; }
            public string ExpInvolCriminalJusticeSystem { get; set; }
            public int MemCurrOnProbation { get; set; }
            public int ProbationContact { get; set; }
            public int MemNeedLegalAid { get; set; }
            public int CrimJustSystemImpactHousing { get; set; }
            public string ExpCrimJustSystemImpactHousing { get; set; }
            public int CrimJustSystemImpactEmployment { get; set; }
            public string ExpCrimJustSystemImpactEmployment { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_HealthPromotion
        {
            public int HealthPromotionId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemHospitalizedInLastMonths { get; set; }
            public DateTime MemRecentHospitalized { get; set; }
            public DateTime MemLastAnnualPhysicalExam { get; set; }
            public DateTime MemLastDentalExam { get; set; }
            public bool NoConcernsAtThisTime { get; set; }
            public bool DentalHygieneSupport { get; set; }
            public bool Presedation { get; set; }
            public bool Dentures { get; set; }
            public bool MIPS { get; set; }
            public bool Other { get; set; }
            public bool Orthodondist { get; set; }
            public string ExpSupptResultDentalOralCare { get; set; }
            public DateTime MemLastEyeExam { get; set; }
            public int MemHadColonoscopy { get; set; }
            public DateTime MemRecentColonoscopy { get; set; }
            public int MemHadMammogram { get; set; }
            public DateTime MemRecentMammogram { get; set; }
            public int MemHadCervicalCancerExam { get; set; }
            public DateTime MemRecentCervicalCancerExam { get; set; }
            public int MemHadProstateExam { get; set; }
            public DateTime MemRecentProstateExam { get; set; }
            public int MemDementiaInPastMonths { get; set; }
            public DateTime MemRecentDementia { get; set; }
            public double MemHeight { get; set; }
            public double MemWeight { get; set; }
            public int BMI { get; set; }
            public int MemConcernAboutSleep { get; set; }
            public int MemAwakeDuringNight { get; set; }
            public int MemHadDiabeticScreening { get; set; }
            public DateTime MemRecentDiabeticScreening { get; set; }
            public bool NoConcernForDiabetes { get; set; }
            public bool RequiredMedicationForDiabetes { get; set; }
            public bool AssistanceWithDiabetesMonitoring { get; set; }
            public bool MedicationAdministration { get; set; }
            public bool DietaryModification { get; set; }
            public bool EducationTraining { get; set; }
            public string ExpSupptResultForMemDiabetes { get; set; }
            public bool NoRespiratoryConcern { get; set; }
            public bool RequiresMedicationForRespConcren { get; set; }
            public bool UseCPAPMachine { get; set; }
            public bool UseNebulizer { get; set; }
            public bool UseOxygen { get; set; }
            public bool ExerciseRestrictions { get; set; }
            public bool OtherTherapies { get; set; }
            public string ExpServicesRespiratoryNeed { get; set; }
            public bool NoConcernsForCholesterol { get; set; }
            public bool ModifiedDiet { get; set; }
            public bool CholesterolLoweringMedications { get; set; }
            public bool IncreaseExercise { get; set; }
            public bool EncourageWeightLossForCholesterol { get; set; }
            public bool ProvideAssistanceWithMealPlanning { get; set; }
            public bool ProvideEducationToThePerson { get; set; }
            public string ExpSupptForHighCholesterol { get; set; }
            public bool NoConcernForHighBloodPressure { get; set; }
            public bool EncourageWeightLossForHighBloodPressure { get; set; }
            public bool BloodPressureMonitoringPlan { get; set; }
            public bool ReduceSaltIntake { get; set; }
            public bool EncourageExercise { get; set; }
            public bool MedicationRequired { get; set; }
            public string ExpSupptForHighBloodPressure { get; set; }
            public int MemBloodTestForLeadPoisoning { get; set; }
            public DateTime MemRecentBloodTestForLeadPoisoning { get; set; }
            public int MemSexuallyActive { get; set; }
            public bool BirthControlOral { get; set; }
            public bool BirthControlProphylactic { get; set; }
            public bool NaturalFamilyPlanning { get; set; }
            public bool NoBirthControl { get; set; }
            public bool Unknown { get; set; }
            public int STIInPastMonths { get; set; }
            public int MemHIVPositive { get; set; }
            public DateTime MemLastHIVAppointment { get; set; }
            public int MemExerciseInWeekForThirtyMintues { get; set; }
            public int MemInterestedIncPhysicalActivity { get; set; }
            public int MemHaveSeizureDisorder { get; set; }
            public int MemNeedSupptOnSeizure { get; set; }
            public string ExpMemSupptExpectedForSeizureDisorder { get; set; }
            public int HealthRelatedConcernsNotAddressed { get; set; }
            public string ExpHealthConcerns { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }

        public class CCO_BehavioralHealth
        {
            public int BehavioralHealthId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemberBeenDiagnosed { get; set; }
            public bool PrevAnxiety { get; set; }
            public bool PrevDepression { get; set; }
            public bool PrevADHD { get; set; }
            public bool PrevPanicDisorder { get; set; }
            public bool PrevPsychosis { get; set; }
            public bool PrevSchizophrenia { get; set; }
            public bool PrevBipolarDisorder { get; set; }
            public bool PrevPostTraumaticStressDisorder { get; set; }
            public bool PrevObsessiveCompulsiveDisorder { get; set; }
            public bool PrevEatingDisorder { get; set; }
            public bool PrevImpulsiveControlDisorder { get; set; }
            public bool PrevPersonalityDisorder { get; set; }
            public bool PrevBorderlinePersonalityDisorder { get; set; }
            public bool CurreAnxiety { get; set; }
            public bool CurreDepression { get; set; }
            public bool CurreADHD { get; set; }
            public bool CurrePanicDisorder { get; set; }
            public bool CurrePsychosis { get; set; }
            public bool CurreSchizophrenia { get; set; }
            public bool CurreBipolarDisorder { get; set; }
            public bool CurrePostTraumaticStressDisorder { get; set; }
            public bool CurreObsessiveCompulsiveDisorder { get; set; }
            public bool CurreEatingDisorder { get; set; }
            public bool CurreImpulsiveControlDisorder { get; set; }
            public bool CurrePersonalityDisorder { get; set; }
            public bool CurreBorderlinePersonalityDisorder { get; set; }
            public string DiagnosMemberAcuteChronicHealthCondition { get; set; }
            public int PsychiatricConditionInterfereWithMem { get; set; }
            public int SourceOfMentalHealthDiagnos { get; set; }
            public bool OutpatientOneToOneTherapy { get; set; }
            public bool OutpatientGroupTherapy { get; set; }
            public bool PsychiatricMedication { get; set; }
            public bool FamilyTherapy { get; set; }
            public bool ParentSupportAndTraining { get; set; }
            public bool PeerMentor { get; set; }
            public bool PROSClinic { get; set; }
            public bool AcuteInpatientTreatment { get; set; }
            public bool LongTermInpatientTreatment { get; set; }
            public bool Other { get; set; }
            public DateTime MemRecentPsychiatricHospitalization { get; set; }
            public int MemRecvSuicidalThoughtsInPast { get; set; }
            public int MemMonitoredForSuicidalRisk { get; set; }
            public string NatureOfSelfHarmBehavior { get; set; }
            public int MemMonitoredForSelfHarmRisk { get; set; }
            public int MemMedicationMonitoringPlan { get; set; }
            public int MedicationMonitoredByPsychiatrist { get; set; }
            public int PscyhiatricMonitoringFrequency { get; set; }
            public int MemHistoryOfTrauma { get; set; }
            public int MemPhysicallyHurtOthers { get; set; }
            public int MemInsultOthers { get; set; }
            public int MemThreatenOthers { get; set; }
            public int MemScreamCurseOthers { get; set; }
            public int MemSmoke { get; set; }
            public int IndvDrinkAlcohol { get; set; }
            public int IndvUseRecreationalDrugs { get; set; }
            public int PersonBeenPrescribedPRN { get; set; }
            public string ReasonForPRNMedication { get; set; }
            public int FrequencyForPRNGiven { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_ChallengingBehaviors
        {
            public int ChallengingBehaviorId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemHasChallengingBehavior { get; set; }
            public bool SelfHarmfulBehavior { get; set; }
            public bool PhysicallyHurtOther { get; set; }
            public bool HarmOther { get; set; }
            public bool DestructionOfProperty { get; set; }
            public bool DisruptiveBehavior { get; set; }
            public bool UnusualBehavior { get; set; }
            public bool Withdrawal { get; set; }
            public bool SociallyOffensiveBehavior { get; set; }
            public bool PersistentlyUncooperative { get; set; }
            public bool ProblemWithSelfcare { get; set; }
            public bool Pica { get; set; }
            public bool Elopement { get; set; }
            public bool Other { get; set; }
            public string MemChallengingBehaviorManifests { get; set; }
            public bool OutpatientOneToOneTherpay { get; set; }
            public bool OutpatientGroupTherapy { get; set; }
            public bool PsychiatricMedication { get; set; }
            public bool FamilyTherapy { get; set; }
            public bool ParentSupportAndTraining { get; set; }
            public bool PeerMentor { get; set; }
            public bool PROSClinic { get; set; }
            public bool AcuteInpatientTreatment { get; set; }
            public bool LongTermInpatientTreatment { get; set; }
            public bool OtherChalleningBehaviorInPast { get; set; }
            public int RestrictiveEater { get; set; }
            public int MemShowAggressiveOnMeals { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_BehavioralSupportPlan
        {
            public int BehavioralSupportPlanId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemHaveBehavioralSupportPlan { get; set; }
            public int MemHumanRightApproval { get; set; }
            public int MemReqPhyInterventionInPastForSafety { get; set; }
            public int PhyInterventionPartOfSupportPlan { get; set; }
            public int MemSupptPlanContainRestrictiveIntervention { get; set; }
            public bool SCIPR { get; set; }
            public bool Medication { get; set; }
            public bool RightLimitation { get; set; }
            public bool TimeOut { get; set; }
            public bool MechanicalRestrainingDevice { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_CommunityParticipation
        {
            public int CommunityParticipationId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemCurreSelfDirectSupportService { get; set; }
            public int MemWishSelfDirectSupportService { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_Education
        {
            public int EducationId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemCompletedEducationLevel { get; set; }
            public int MemCurreSchoolEducation { get; set; }
            public int CurreEducationMeetNeed { get; set; }
            public int MemPursuingAdditionalEducation { get; set; }
            public int ChooseCurrentEducation { get; set; }
            public string DescribeSupptResultInEducationSetting { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_TransitionPlanning
        {
            public int TransitionPlanningId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public string DescribePrevocationalSkill { get; set; }
            public int MemCompetitivelyEmployed { get; set; }
            public int MemCurreReceivingServices { get; set; }
            public DateTime StartDateACCESVRService { get; set; }
            public bool VocationalCounseling { get; set; }
            public bool AssessmentsAndEvaluations { get; set; }
            public bool RehabilitationTechnology { get; set; }
            public bool SpecialTransportation { get; set; }
            public bool AdaptiveDriverTraining { get; set; }
            public bool WorkReadiness { get; set; }
            public bool TuitionFeesTextbooks { get; set; }
            public bool NoteTaker { get; set; }
            public bool YouthService { get; set; }
            public bool PhysicalMentalRestoration { get; set; }
            public bool HomeVehicleWorksite { get; set; }
            public bool JobDevelopmentPlacement { get; set; }
            public bool WorkTryOut { get; set; }
            public bool JobCoaching { get; set; }
            public bool OccupationalToolEquipment { get; set; }
            public bool GoodsInventoryEquipment { get; set; }
            public bool OccupationalBusinessLicense { get; set; }
            public bool TicketToWork { get; set; }
            public bool PASS { get; set; }
            public bool WelfareToWork { get; set; }
            public bool Other { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }
        public class CCO_Employment
        {
            public int EmploymentId { get; set; }
            public int CompAssessmentVersioningId { get; set; }
            public int CompAssessmentId { get; set; }
            public int MemCurrentlyHCBSSupptService { get; set; }
            public int MemCurreEmploymentStatus { get; set; }
            public int MemWishIncCurreLevelOfEmployment { get; set; }
            public int MemSatisfiedWithCurrentEmployer { get; set; }
            public string EmployerName { get; set; }
            public string EmployerLocation { get; set; }
            public DateTime StartDateOfCurrentJob { get; set; }
            public DateTime TerminationDateOfRecentJob { get; set; }
            public int ReasonToChangeEmploymentStatus { get; set; }
            public int MemHoursWorkInWeek { get; set; }
            public int MemEarnInWeek { get; set; }
            public int MemPaycheck { get; set; }
            public int DescMemEmploymentSetting { get; set; }
            public int SatisfiedCurrentEmploymentSetting { get; set; }
            public int MemWorkInIntegratedSetting { get; set; }
            public string Status { get; set; }
            public string Active { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedBy { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public string RecordDeleted { get; set; }
        }

       
    }
}

