using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Response
{

    public class IncidentManagementGeneralRespnse : BaseResponse
    {
        public List<GeneralTab> GeneralTab { get; set; }
        public List<IncidentManagementGeneralList> IncidentManagementGeneralList { get; set; }
        public List<IncidentManagementInjuryList> IncidentManagementInjuryList { get; set; }
        public List<IncidentManagementDeathList> IncidentManagementDeathList { get; set; }
        public List<IncidentManagementMedicationErrorList> IncidentManagementMedicationErrorList { get; set; }
        public List<IncidentManagementOtherList> IncidentManagementOtherList { get; set; }
        public List<IncidentManagementOPWDD147List> IncidentManagementOPWDD147List { get; set; }
        public List<IncidentManagementOPWDD148List> IncidentManagementOPWDD148List { get; set; }
        public List<IncidentManagementOPWDD150List> IncidentManagementOPWDD150List { get; set; }
        public List<IncidentManagementJonathanLawList> IncidentManagementJonathanLawList { get; set; }
        public List<InjurySubTableList> InjurySubTableList { get; set; }
        public List<MedicationErrorSubFieldList> MedicationErrorSubFieldList { get; set; }
        public List<StateFormList> StateFormList { get; set; }
        public List<InjuryWitness> InjuryWitness { get; set; }
        public List<Opwdd147SubTable> Opwdd147SubTable { get; set; }
        public List<MediWitness> MediWitness { get; set; }
        public List<DeatWitness> DeatWitness { get; set; }
        public List<OtheWitness> OtheWitness { get; set; }
        public List<SatfInvolved> SatfInvolved { get; set; }
        public List<UploadedPDFResponse> UploadedPDFResponse { get; set; }
    }

    public class OPWDD150Details
    {
        public int StatesFormOPWDD150Id { get; set; }
        public int IncidentManagementId { get; set; }
        public int? Event_SituationReferenceNumber { get; set; }
        public string PersonCompletingReport { get; set; }
        public char? DateTimeEvent_Situation { get; set; }
        public string ObservedDateTime { get; set; }
        public string DiscoveredDateTime { get; set; }
        public string DateTimeEvent_SituationOccure { get; set; }
        public int? PreliminaryClassification { get; set; }
        public char? AdultProtectiveServices { get; set; }
        public char? FamilyMembers { get; set; }
        public char? Hospital { get; set; }
        public char? LawEnforcement { get; set; }
        public char? ProfessionalDisciplineOffice { get; set; }
        public char? School { get; set; }
        public char? StatewideCentralRegisterChildAbuseAndMaltreatment { get; set; }
       // public int ActionTaken { get; set; }
        public string SummaryResloutionOfEvent_Situation { get; set; }
        public string NotificationContact { get; set; }
        public string NotificationDateTime { get; set; }
        public string PersonContacted { get; set; }
        public string ReportedBy { get; set; }
        public string Method { get; set; }
        public string PartyCompletingFormName { get; set; }
        public string PartyCompletingFormTitle { get; set; }
        public string PartyCompletingFormDate { get; set; }

    }
    public class GeneralTab
    {
        public int IncidentManagementId { get; set; }

        public string Incident { get; set; }

        public string Description { get; set; }

        public string IncidentType { get; set; }
        public string Site { get; set; }
        public string IncidentDateTime { get; set; }
        public string Department { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int? IncidentInjuryId { get; set; }
        public string TimeOfInjury { get; set; }
        //public int InjuryLocation { get; set; }
        public int? IncidentMedicationErrorId { get; set; }
        //public string TimeOfMedicationError { get; set; }
        public int? TypeOfMedError { get; set; }
      //  public string MedicalAttentionNeeded { get; set; }
        public string NotifiedDate { get; set; }
        public int? InjuryType { get; set; }
        //public string InjuryType { get; set; }
        public string Location { get; set; }

        public int? Medication { get; set; }
        public string DescriptionOfTheError { get; set; }
        public string DescriptionOfTheIncident { get; set; }
        public string TimeOfEvent { get; set; }
        public string TimeOfDeath { get; set; }
        public int? DeathLocation { get; set; }
      //  public int InjuryWas { get; set; }
        public string InjuryLocation { get; set; }
        public string InjuryCause { get; set; }
        public string InjurySeverity { get; set; }
        public string InjuryColor { get; set; }

        public string TimeOfMedicationError { get; set; }
        public string CauseOfMedError { get; set; }
        public string SeverityOfMedError { get; set; }
        public int? MedicalAttentionNeeded { get; set; }
        public string CauseOfDeath { get; set; }
        public string FrontBody { get; set; }
        public string BackBody { get; set; }



    }
    public class IncidentManagementTabsResponse : BaseResponse
    {
        public List<AllTabs> AllTabs { get; set; }
    }
    public class AllTabs
    {
        public int IncidentManagementId { get; set; }
        public int InjuryBodyPartId { get; set; }
        public int MedicationErrorSubFieldId { get; set; }
        public int IncidentInjuryId { get; set; }
        public int IncidentMedicationErrorId { get; set; }
        public int IncidentDeathId { get; set; }
        public int StatesFormOPWDD150Id { get; set; }
        public int StateFormJonathanLawnotificationId { get; set; }
        public int StatesFormOPWDD148Id { get; set; }
        public int StatesFormOPWDD147Id { get; set; }
        public int OPWDD147NotificationId { get; set; }
        public string JSONData { get; set; }
        public int StateFormPDFVersioningId { get; set; }
        public int TypeOfForm { get; set; }
        public int StateFormId { get; set; }
        public int IncidentOtherId { get; set; }
    }

    public class AllPDFResponse
    {
        public List<AllPDF> AllPDF { get; set; }
    }
    public class AllPDF
    {
        public string FileName { get; set; }

        public string  DownloadFile { get; set; }
    }
    public class IncidentManagementGeneralList
    {
        public int IncidentManagementId { get; set; }
        public int? ClientId { get; set; }
        public string Incident { get; set; }
        public int? IncidentType { get; set; }
        public string Department { get; set; }
        public string IncidentDate { get; set; }
        public string IncidentTime { get; set; }
        public int? Site { get; set; }
        public string ReportedDate { get; set; }
        public int? ReportedBy { get; set; }
        public int? ReportedRelationshipToStaf { get; set; }
        public int? Location { get; set; }
        public string DescriptionOfTheIncident { get; set; }
        public int? AbuseSuspected { get; set; }
        public int? NeglectSuspected { get; set; }
        public int? ExploitationSuspected { get; set; }

        public string IncidentTypeDescription { get; set; }
    }
    public class IncidentManagementInjuryList
    {
        public int? IncidentInjuryId { get; set; }
        public int IncidentManagementId { get; set; }
        public string TimeOfInjury { get; set; }
        public char? TimeOfInjuryUnknown { get; set; }
        public bool? InjuryWas { get; set; }
        public string ObservedDate { get; set; }
        public string ObservedTime { get; set; }
        public string DiscoveredDate { get; set; }
        public string DiscoveredTime { get; set; }
        public int? InjuryLocation { get; set; }
        public int? InjuryCause { get; set; }
        public int? InjurySeverity { get; set; }
        public int? InjuryColor { get; set; }
        public string InjurySizeLength { get; set; }
        public string InjurySizeWidth { get; set; }
        public string InjurySizeDepth { get; set; }
        public int? TreatedByStaff { get; set; }
        public string TreatmentDate { get; set; }
        public string Description { get; set; }
        public string Witness { get; set; }
    }
    public class IncidentManagementMedicationErrorList
    {
        public int? IncidentMedicationErrorId { get; set; }
        public int IncidentManagementId { get; set; }
        public string TimeOfMedicationError { get; set; }
        public char? TimeOfMedicationErrorUnknown { get; set; }
        public string DiscoveredDate { get; set; }
        public string DiscoveredTime { get; set; }
        public int? TypeOfMedError { get; set; }
        public int? CauseOfMedError { get; set; }
        public int? SeverityOfMedError { get; set; }
        public bool? MedicalAttentionNeeded { get; set; }
        public string StaffInvolved { get; set; }
        public bool? PrescriberNotified { get; set; }
        public string NameOfPrescriber { get; set; }
        public string NotifiedDate { get; set; }
        public string Witness { get; set; }
    }
    public class IncidentManagementDeathList
    {
        public int IncidentManagementId  { get; set; }
        public string TimeOfDeath { get; set; }
        public char? TimeOfDeathUnknown { get; set; }
        public string DiscoveredDate { get; set; }
        public string DiscoveredTime { get; set; }
        public int DeathLocation { get; set; }
        //public int CauseOfDeath { get; set; }
        public string CauseOfDeath { get; set; }
        public string DeathDeterminedBy { get; set; }
        public string DateOfLastMedicalExam { get; set; }
        public bool? AutopsyConsent { get; set; }
        public string PersonRequestingConsent { get; set; }
        public string PersonConsenting { get; set; }
        public string PersonDenyingConsent { get; set; }
        public bool? AutopsyDone { get; set; }
        public string AutopsyDate { get; set; }

        public string Witness { get; set; }
        public string Description { get; set; }
        public int? IncidentDeathId { get; set; }
    }
    public class IncidentManagementOtherList
    {
        public int? IncidentOtherId { get; set; }
        public int IncidentManagementId { get; set; }
        public string TimeOfEvent { get; set; }
        public char? TimeOfEventUnknown { get; set; }
        public bool? EventWas { get; set; }
        public string ObservedDate { get; set; }
        public string ObservedTime { get; set; }
        public string DiscoveredDate { get; set; }
        public string DiscoveredTime { get; set; }
        public int? EventLocation { get; set; }
        public int? CauseOfDeath { get; set; }
        public string Description { get; set; }
        public string witness { get; set; }
    }
    public class IncidentManagementOPWDD147List
    {
        public int? StatesFormOPWDD147Id { get; set; }
        public int IncidentManagementId { get; set; }
        public int? MasterIncidentNumber { get; set; }
        public int? AgencyIncidentNumber { get; set; }
        public bool? IncidentPreviouslyReported { get; set; }
        public bool? DateTimeIncidentWas { get; set; }
        public string ObservedDate { get; set; }
        public string ObservedTime { get; set; }
        public string DiscoveredDate { get; set; }
        public string DiscoveredTime { get; set; }
        public string IncidentOccuredDate { get; set; }
        public string IncidentOccuredTime { get; set; }
        public int? PRSPresentAtIncident { get; set; }
        public int? ERSPresentAtIncident { get; set; }
        public int? ReportableIncident_AbuseNeglect { get; set; }
        public bool? SeriousNotableOccurrences { get; set; }
        public bool? MinornotableOccurrences { get; set; }
        public int? Reportable_SignificantIncidents { get; set; }
        public int? IncidentOccurenceLocation { get; set; }
        public string IncidentDescription { get; set; }
        public bool? JusticeCenter { get; set; }
        public string JusticeCenteDate { get; set; }
        public string JusticeCenteTime { get; set; }
        public string JusticeCenterIdentifier { get; set; }
        public string ReportedBy { get; set; }
        public bool? LawEnforcementOfficialNotified { get; set; }
        public string OfficialNotifiedDate { get; set; }
        public string OfficialNotifiedTime { get; set; }
        public string PermanentAddress_PhoneNumber { get; set; }
        public char? SOIRA { get; set; }
        public char? VOIRA { get; set; }
        public char? SOICF { get; set; }
        public char? VOICF { get; set; }
        public char? FC { get; set; }
        public char? DC { get; set; }
        public char? CR { get; set; }
        public string Other { get; set; }
        public string PartyCompletingItemsName { get; set; }
        public string PartyCompletingItemsTitle { get; set; }
        public string PartyCompletingItemsDate { get; set; }
        public string PartyReviewingItemsName { get; set; }
        public string PartyReviewingItemsTitle { get; set; }
        public string PartyReviewingItemsDate { get; set; }


        public string AgencyCompletingForm { get; set; }
        public string Facility { get; set; }
        public string ProgramType147 { get; set; }
        public string Address147 { get; set; }
        public string Phone147 { get; set; }
        public char? Gender147 { get; set; }
        public int? TabId147 { get; set; }
        public string IndividualName147 { get; set; }
        public string DOB147 { get; set; }

        public string PartyCompletingItem28 { get; set; }
        public string PartyCompletingItemTitle28 { get; set; }
        public string PartyCompletingItemDate28 { get; set; }
        public string AdditionalStepsToSafeGuardPerson { get; set; }
        public string ActionsTakenToSafeGuardPerson { get; set; }
        public string LawEnforcementAgencyName { get; set; }
        public string OtherSpecify { get; set; }

    }
    public class IncidentManagementOPWDD148List
    {
        public int? StatesFormOPWDD148Id { get; set; }
        public int IncidentManagementId { get; set; }
        public string PersonReceivingServicesName { get; set; }
        public string IncidentOccurredOrWasDiscoveredDate { get; set; }
        public string PreliminaryClassificationOfIncident { get; set; }
        public string AgencyCompletingThisForm { get; set; }
        public string MasterIncidentNumber { get; set; }
        public string ReportProvidedTo { get; set; }
        public string RelationshipToPersonReceivingServices { get; set; }
        public string PhoneNumber { get; set; }
        public string InitialnotificationProvidedToPersonReceivingDate { get; set; }
        public string ImmediateStepsTakenInResponse { get; set; }
        public string NameOfPersonCompletingThisReport { get; set; }
        public string ReportCompletedDate { get; set; }

        public string AddInfoContact { get; set; }
        public string AtTelephone { get; set; }
    }
    public class IncidentManagementOPWDD150List
    {
        public int? StatesFormOPWDD150Id { get; set; }
        public int IncidentManagementId { get; set; }
        public int? Event_SituationReferenceNumber { get; set; }
        public string PersonCompletingReport { get; set; }
        public bool DateTimeEvent_Situation { get; set; }
        public string ObservedDate { get; set; }
        public string ObservedTime { get; set; }
        public string DiscoveredDate { get; set; }
        public string DiscoveredTime { get; set; }
        public string Event_SituationOccureDate { get; set; }
        public string Event_SituationOccureTime { get; set; }
        public string DateTimeEvent_SituationOccure { get; set; }
        public int? PreliminaryClassification { get; set; }
        public char? AdultProtectiveServices { get; set; }
        public char? FamilyMembers { get; set; }
        public char? Hospital { get; set; }
        public char? LawEnforcement { get; set; }
        public char? ProfessionalDisciplineOffice { get; set; }
        public char? School { get; set; }
        public char? StatewideCentralRegisterChildAbuseAndMaltreatment { get; set; }
        public string DescriptionOfEventSituation { get; set; }
        public char? AssessMonitorIndividual { get; set; }
        public char? EducateIndividualChoices { get; set; }
        public char? InterviewInvolvedIndividuals { get; set; }
        public char? OfferingReferralAppropriateService { get; set; }
        public char? ReviewRecordIOtherRelevant { get; set; }
        public char? Other { get; set; }
        public string SummaryResloutionOfEvent_Situation { get; set; }
        public string NotificationContact { get; set; }
        public string NotificationDate { get; set; }
        public string NotificationTime { get; set; }
        public string PersonContacted { get; set; }
        public string ReportedBy { get; set; }
        public string Method { get; set; }
        public string PartyCompletingFormName { get; set; }
        public string PartyCompletingFormTitle { get; set; }
        public string PartyCompletingFormDate { get; set; }

        public string ReportingAgency { get; set; }
        public string ProgramType { get; set; }
        public string ProgramAddress { get; set; }
        public string EventAddress { get; set; }
        public string Phone { get; set; }
        public string IndividualName { get; set; }
        public string DOB { get; set; }
        public char? Gender { get; set; }
        public int? TabId { get; set; }

        public string NotificationContact1 { get; set; }
        public string NotificationDate1 { get; set; }
        public string NotificationTime1 { get; set; }
        public string PersonContacted1 { get; set; }
        public string ReportedBy1 { get; set; }
        public string Method1 { get; set; }

    }
    public class IncidentManagementJonathanLawList
    {
        public int? StateFormJonathanLawnotificationId { get; set; }
        public int IncidentManagementId { get; set; }
        public int? ActionType { get; set; }
        public string NameOfPersonnotified { get; set; }
        public int? PersonRelationship { get; set; }
        public string NotificationDate { get; set; }
        public string NotificationTime { get; set; }
        public int? MethodOfNotification { get; set; }
        public int? NotifiedByStaff { get; set; }
        public string Comments { get; set; }
    }

    public class InjurySubTableList
    {

        public int IncidentMedicationErrorId { get; set; }
        public int? StatesFormOPWDD147Id { get; set; }
        public int? IncidentInjuryId { get; set; }
        public string JSONData { get; set; }
    }
    public class Opwdd147SubTable
    {
        
        public int? StatesFormOPWDD147Id { get; set; }
        public string JSONData { get; set; }
    }
    public class MedicationErrorSubFieldList
    {
        public int MedicationErrorSubFieldId { get; set; }
        public int? IncidentMedicationErrorId { get; set; }
        public int? Medication { get; set; }
        public string DescriptionOfTheError { get; set; }
    }

    public class StateFormList
    {
        public int IncidentManagementId { get; set; }
        public int? StateFormId { get; set; }
        public char? OPWDD147 { get; set; }
        public char? OPWDD148 { get; set; }
        public char? OPWDD150 { get; set; }
        public char? JonathanLaw { get; set; }
    }
    public class InjuryWitness
    {
        public int InjuryWitnessId { get; set; }

        public string Witness { get; set; }
    }
    public class MediWitness
    {
        public int? MedicationWitnessId { get; set; }

        public string MedicationWitness { get; set; }
        
    }
    public class DeatWitness
    {
        public int? DeathWitnessId { get; set; }

        public string DeathWitness { get; set; }
       

    }
    public class OtheWitness
    {
        public int? OtherWitnessId { get; set; }

        public string OtherWitness { get; set; }


    }
    public class SatfInvolved
    {
       public int? MedicationStaffId { get; set; }
        public string SatffInvolved { get; set; }


    }

    public class AllPDFUploadResponse
    {
        public List<UploadedPDFResponse> UploadedPDFResponse { get; set; }
    }

    public class UploadedPDFResponse
    {
        public string DownloadLink { get; set; }

        public int? StateFormPDFVersioningId { get; set; }

        public int? StatesFormOPWDD147Id { get; set; }
        public int? StatesFormOPWDD148Id { get; set; }
        public int? StatesFormOPWDD150Id { get; set; }
        public string PDFDocument { get; set; }
        public string FormType { get; set; }
        public string UploadDate {get;set;}
        public string Version { get; set; }
    }
}
