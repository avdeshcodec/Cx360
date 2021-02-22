using Newtonsoft.Json;
using IncidentManagement.Entities.Response;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
namespace IncidentManagement.Entities.Common
{
    public class CommonFunctions
    {
        public static string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];

        public static string GetConnectionString(string connectionString)
        {
            return ConfigurationManager.AppSettings[connectionString];
        }

        public static string CurrentUser { get; set; }

        public static BaseResponse InvalidRequest()
        {
            BaseResponse baseResponse = new BaseResponse();
            baseResponse.Message = CustomErrorMessages.INVALID_INPUTS;
            baseResponse.Success = false;
            return baseResponse;
        }

        public static Boolean ValidateToken(string Key, string Secret, string connectionstring)
        {
            return CheckKioskSecret(Key, Secret, connectionstring);
        }

        public static Boolean CheckKioskSecret(string Key, string Secret, string connectionstring)
        {
            Boolean IsAuthenticate = false;

            using (SqlConnection con = new SqlConnection(CommonFunctions.GetConnectionString(connectionstring)))
            {
                using (SqlCommand cmd = new SqlCommand("sp_CheckKioskSecret", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@key", Key);
                    cmd.Parameters.AddWithValue("@secret", Secret);

                    con.Open();

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = cmd;

                    DataSet tempDataSet = new DataSet();
                    sqlDataAdapter.Fill(tempDataSet);

                    if (tempDataSet.Tables.Count > 0 && tempDataSet.Tables[0].Rows.Count > 0)
                    {
                        IsAuthenticate = Convert.ToBoolean(tempDataSet.Tables[0].Rows[0]["Valid"]);
                    }
                }
            }
            return IsAuthenticate;
        }


        #region Data to Json convert or vice-versa
        /// <summary>
        /// This method will be used to convert DataSet to JSON string
        /// </summary>
        /// <param name="TempDataSet"></param>
        /// <returns>Returns string in JSON format</returns>
        private static string ConvertDataSetToJson(DataSet TempDataSet)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TempDataSet);
            return JSONString;
        }


        /// <summary>
        /// This method will be used to convert Datatable to JSON string
        /// </summary>
        /// <param name="TempDataTable"></param>
        /// <returns>Returns string in JSON format</returns>
        public static string ConvertDataTableToJson(DataTable TempDataTable)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(TempDataTable);
            return JSONString;
        }

        /// <summary>
        /// This method will be used to JSON string to DataSet
        /// </summary>
        /// <param name="TempDataSet"></param>
        /// <returns>Returns DataSet from JSON string</returns>
        public static DataSet ConvertJsonToDataSet(string json)
        {
            DataSet TempDataSet = JsonConvert.DeserializeObject<DataSet>(json);
            return TempDataSet;
        }

        /// <summary>
        /// This method will be used to JSON string to DataTable
        /// </summary>
        /// <param name="TempDataTable"></param>
        /// <returns>Returns DataSet from JSON string</returns>
        public static DataTable ConvertJsonToDataTable(string json)
        {
            DataTable TempDataTable = JsonConvert.DeserializeObject<DataTable>(json);
            return TempDataTable;
        }

        #endregion



        /// <summary>
        /// This function is used to get mapped store procedure with tab
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        public static string GetMappedStoreProcedure(string tabName)
        {
            string storeProcedure = string.Empty;
            switch (tabName)
            {
                case "GeneralTab":
                    storeProcedure = MappedStoreProcedure.GeneralTab;
                    break;
                case "InjuryTab":
                    storeProcedure = MappedStoreProcedure.InjuryTab;
                    break;
                case "DeathTab":
                    storeProcedure = MappedStoreProcedure.DeathTab;
                    break;
                case "MedicationTab":
                    storeProcedure = MappedStoreProcedure.MedicationTab;
                    break;
                case "OtherTab":
                    storeProcedure = MappedStoreProcedure.OtherTab;
                    break;
                case "StateFormTab":
                    storeProcedure = MappedStoreProcedure.StateFormTab;
                    break;
                case "OPWDD147Tab":
                    storeProcedure = MappedStoreProcedure.OPWDD147Tab;
                    break;
                case "OPWDD148Tab":
                    storeProcedure = MappedStoreProcedure.OPWDD148Tab;
                    break;
                case "OPWDD150Tab":
                    storeProcedure = MappedStoreProcedure.OPWDD150Tab;
                    break;
                case "JonathanLawTab":
                    storeProcedure = MappedStoreProcedure.JonathanLawTab;
                    break;
                case "MasterPage":
                    storeProcedure = MappedStoreProcedure.MasterPage;
                    break;
                case "OPWDD150Details":
                    storeProcedure = MappedStoreProcedure.OPWDD150Details;
                    break;
                case "DeleteMasterRecord":
                    storeProcedure = MappedStoreProcedure.DeleteMasterRecord;
                    break;
                case "EditAllRecord":
                    storeProcedure = MappedStoreProcedure.EditAllRecord;
                    break;
                case "LifePlan":
                    storeProcedure = MappedStoreProcedure.LifePlan;
                    break;
                case "AssessmentNarrativeSummary":
                    storeProcedure = MappedStoreProcedure.AssessmentNarrativeSummary;
                    break;
                case "GetAssessmentNarrativeSummaryTab":
                    storeProcedure = MappedStoreProcedure.GetAssessmentNarrativeSummaryTab;
                    break;
                case "MeetingHistory":
                    storeProcedure = MappedStoreProcedure.MeetingHistory;
                    break;
                case "GetMeetingHistory":
                    storeProcedure = MappedStoreProcedure.GetMeetingHistory;
                    break;
                case "IndividualSafe":
                    storeProcedure = MappedStoreProcedure.IndividualSafe;
                    break;
                case "GetIndividualSafe":
                    storeProcedure = MappedStoreProcedure.GetIndividualSafe;
                    break;
                case "InsertOutcomesSupportStrategies":
                    storeProcedure = MappedStoreProcedure.InsertOutcomesSupportStrategies;
                    break;
                case "GetOutcomesSupportStrategies":
                    storeProcedure = MappedStoreProcedure.GetOutcomesSupportStrategies;
                    break;
                case "InsertHCBSWaiver":
                    storeProcedure = MappedStoreProcedure.InsertHCBSWaiver;
                    break;
                case "GetMedicaidStatePlanAuthorizedServies":
                    storeProcedure = MappedStoreProcedure.GetMedicaidStatePlanAuthorizedServies;
                    break;
                case "InsertFundalNaturalCommunityResources":
                    storeProcedure = MappedStoreProcedure.InsertFundalNaturalCommunityResources;
                    break;
                case "GetFundalNaturalCommunityResources":
                    storeProcedure = MappedStoreProcedure.GetFundalNaturalCommunityResources;
                    break;
                case "LifePlanMasterPage":
                    storeProcedure = MappedStoreProcedure.LifePlanMasterPage;
                    break;
                case "GetLifePlanRecords":
                    storeProcedure = MappedStoreProcedure.GetLifePlanRecords;
                    break;
                case "LifePlanPDF":
                    storeProcedure = MappedStoreProcedure.GetLifePlanRecords;
                    break;
                case "PublishLifePlanVersion":
                    storeProcedure = MappedStoreProcedure.PublishLifePlanVersion;
                    break;
                case "CreateNewVersion":
                    storeProcedure = MappedStoreProcedure.CreateNewVersion;
                    break;
                case "InsertLifePlanNotifications":
                    storeProcedure = MappedStoreProcedure.InsertLifePlanNotifications;
                    break;
                case "ComprehensiveAssessment":
                    storeProcedure = MappedStoreProcedure.ComprehensiveAssessment;
                    break;
                case "AssessmentMedical":
                    storeProcedure = MappedStoreProcedure.AssessmentMedical;
                    break;
                case "AssessmentMedicalHealth":
                    storeProcedure = MappedStoreProcedure.AssessmentMedicalHealth;
                    break;
                case "AssessmentFinancial":
                    storeProcedure = MappedStoreProcedure.AssessmentFinancial;
                    break;
                case "AssessmentHousing":
                    storeProcedure = MappedStoreProcedure.AssessmentHousing;
                    break;
                case "AssessmentDomesticViolance":
                    storeProcedure = MappedStoreProcedure.AssessmentDomesticViolance;
                    break;
                case "AssessmentLegal":
                    storeProcedure = MappedStoreProcedure.AssessmentLegal;
                    break;
                case "AreasSafeguardReview":                    storeProcedure = MappedStoreProcedure.AreasSafeguardReview;                    break;                case "BehavioralSupportServices":                    storeProcedure = MappedStoreProcedure.BehavioralSupportServices;                    break;                case "EducationalVocationalStatus":                    storeProcedure = MappedStoreProcedure.EducationalVocationalStatus;                    break;                case "IndependentLivingSkills":                    storeProcedure = MappedStoreProcedure.IndependentLivingSkills;                    break;                case "SelfDirectedServices":                    storeProcedure = MappedStoreProcedure.SelfDirectedServices;                    break;                case "TransitionPlanning":                    storeProcedure = MappedStoreProcedure.TransitionPlanning;                    break;                case "General":                    storeProcedure = MappedStoreProcedure.General;                    break;                case "SafetyRisk":                    storeProcedure = MappedStoreProcedure.SafetyRiskAssessment;                    break;                case "DepressionScreening":                    storeProcedure = MappedStoreProcedure.DepressionScreening;                    break;                case "SubstanceAbuseScreening":                    storeProcedure = MappedStoreProcedure.SubstanceAbuseScreening;                    break;                case "SafetyPlan":                    storeProcedure = MappedStoreProcedure.SafetyPlan;                    break;
                case "PublishAssessment":                    storeProcedure = MappedStoreProcedure.PublishAssessment;                    break;
                case "AllComprehensiveAssessmentDetails":
                    storeProcedure = MappedStoreProcedure.AllComprehensiveAssessmentDetails;
                    break;
                // CANS Assessment
                case "GeneralInformation":
                    storeProcedure = MappedStoreProcedure.GeneralInformation;
                    break;
                case "TraumaExposure":
                    storeProcedure = MappedStoreProcedure.TraumaExposure;
                    break;
                case "PresentingProblemAndImpact":
                    storeProcedure = MappedStoreProcedure.PresentingProblemAndImpact;
                    break;
                case "Safety":
                    storeProcedure = MappedStoreProcedure.Safety;
                    break;
                case "SubstanceUseHistory":
                    storeProcedure = MappedStoreProcedure.SubstanceUseHistory;
                    break;
                case "PlacementHistory":
                    storeProcedure = MappedStoreProcedure.PlacementHistory;
                    break;
                case "ClientStrength":
                    storeProcedure = MappedStoreProcedure.ClientStrength;
                    break;
                case "FamilyInformation":
                    storeProcedure = MappedStoreProcedure.FamilyInformation;
                    break;
                case "NeedsResourceAssessment":
                    storeProcedure = MappedStoreProcedure.NeedsResourceAssessment;
                    break;
                case "DsmDiagnosis":
                    storeProcedure = MappedStoreProcedure.DsmDiagnosis;
                    break;
                case "MentalHealthSummary":
                    storeProcedure = MappedStoreProcedure.MentalHealthSummary;
                    break;
                case "AddClientFunctioningEvaluation":
                    storeProcedure = MappedStoreProcedure.AddClientFunctioningEvaluation;
                    break;
                case "PresentinProblemAndImpact":
                    storeProcedure = MappedStoreProcedure.PresentinProblemAndImpact;
                    break;
                case "PsychiatricInformation":
                    storeProcedure = MappedStoreProcedure.PsychiatricInformation;
                    break;
                case "CaregiverAddendum":
                    storeProcedure = MappedStoreProcedure.CaregiverAddendum;
                    break;
                case "GeneralInformationDCFS":
                    storeProcedure = MappedStoreProcedure.GeneralInformationDCFS;
                    break;
                case "SexuallyAggrBehavior":
                    storeProcedure = MappedStoreProcedure.SexuallyAggrBehavior;
                    break;
                case "ParentGuardSafety":
                    storeProcedure = MappedStoreProcedure.ParentGuardSafety;
                    break;
                case "ParentGuardWellbeing":
                    storeProcedure = MappedStoreProcedure.ParentGuardWellbeing;
                    break;
                case "ParentGuardPermananence":
                    storeProcedure = MappedStoreProcedure.ParentGuardPermananence;
                    break;
                case "SubstituteCommitPermananence":
                    storeProcedure = MappedStoreProcedure.SubstituteCommitPermananence;
                    break;
                case "IntactFamilyService":
                    storeProcedure = MappedStoreProcedure.IntactFamilyService;
                    break;
                case "IntensivePlacementStabilization":
                    storeProcedure = MappedStoreProcedure.IntensivePlacementStabilization;
                    break;
                case "GeneralInformatioinHRA":
                    storeProcedure = MappedStoreProcedure.GeneralInformatioinHRA;
                    break;
                case "HealthStatus":
                    storeProcedure = MappedStoreProcedure.HealthStatus;
                    break;
                case "DevelopmentHistory":
                    storeProcedure = MappedStoreProcedure.DevelopmentHistory;
                    break;
                case "Medication":
                    storeProcedure = MappedStoreProcedure.Medication;
                    break;
                case "MedicalHistory":
                    storeProcedure = MappedStoreProcedure.MedicalHistory;
                    break;
                case "CreateNewVersionAssessment":
                    storeProcedure = MappedStoreProcedure.CreateNewVersionAssessment;
                    break;
                case "TreatmentPlan":
                    storeProcedure = MappedStoreProcedure.TreatmentPlan;
                    break;
                case "TreatmentPlanDetails":
                    storeProcedure = MappedStoreProcedure.TreatmentPlanDetails;
                    break;
                case "ChangeTreatmentPlanPosition":
                    storeProcedure = MappedStoreProcedure.ChangeTreatmentPlanPosition;
                    break;
                case "UpdateGoalItems":
                    storeProcedure = MappedStoreProcedure.UpdateGoalItems;
                    break;
                case "EditTreatmentPlan":
                    storeProcedure = MappedStoreProcedure.CloneTreatmentPlan;
                    break;
                case "DeleteTreatmentPlan":
                    storeProcedure = MappedStoreProcedure.DeleteTreatmentPlan;
                    break;
                case "SummaryPrioritzedNeedsStrength":
                    storeProcedure = MappedStoreProcedure.SummaryPrioritzedNeedsStrength;
                    break;
                case "ServiceInterventionObjecives":
                    storeProcedure = MappedStoreProcedure.ServiceInterventionObjecives;
                    break;
                case "ServiceInterventions":
                    storeProcedure = MappedStoreProcedure.ServiceInterventions;
                    break;
                case "PublishCansModule":
                    storeProcedure = MappedStoreProcedure.PublishCansModule;
                    break;
                case "CreateNewVersionCans":
                    storeProcedure = MappedStoreProcedure.CreateNewVersionCans;
                    break;
                case "PresentingSignatures":
                    storeProcedure = MappedStoreProcedure.PresentingSignatures;
                    break;
                case "IndividualTreatmentPlan":
                    storeProcedure = MappedStoreProcedure.IndividualTreatmentPlan;
                    break;
                // CCO ComprehensiveAssessment
                case "CCO_ComprehensiveAssessment":
                    storeProcedure = MappedStoreProcedure.CCO_ComprehensiveAssessment;
                    break;
                case "CCO_EligibilityInformation":
                    storeProcedure = MappedStoreProcedure.CCO_EligibilityInformation;
                    break;
                case "CCO_CommunicationLanguage":
                    storeProcedure = MappedStoreProcedure.CCO_CommunicationLanguage;
                    break;
                case "CCO_MemberProviders":
                    storeProcedure = MappedStoreProcedure.CCO_MemberProviders;
                    break;
                case "CCO_CircleOfSupport":
                    storeProcedure = MappedStoreProcedure.CCO_MemberProviders;
                    break;
                case "CCO_GuardianshipAndAdvocacy":
                    storeProcedure = MappedStoreProcedure.CCO_GuardianshipAndAdvocacy;
                    break;
                case "CCO_AdvancedDirectivesFuturePlanning":
                    storeProcedure = MappedStoreProcedure.CCO_AdvancedDirectivesFuturePlanning;
                    break;
                case "CCO_IndependentLivingSkills":
                    storeProcedure = MappedStoreProcedure.CCO_IndependentLivingSkills;
                    break;
                case "CCO_SocialServiceNeed":
                    storeProcedure = MappedStoreProcedure.CCO_SocialServiceNeed;
                    break;
                case "CCO_MedicalHealth":
                    storeProcedure = MappedStoreProcedure.CCO_MedicalHealth;
                    break;
                case "CCO_HealthPromotion":
                    storeProcedure = MappedStoreProcedure.CCO_HealthPromotion;
                    break;
                case "CCO_BehavioralHealth":
                    storeProcedure = MappedStoreProcedure.CCO_BehavioralHealth;
                    break;
                case "CCO_ChallengingBehaviors":
                    storeProcedure = MappedStoreProcedure.CCO_ChallengingBehavior;
                    break;
                case "CCO_BehavioralSupportPlan":
                    storeProcedure = MappedStoreProcedure.CCO_BehavioralSupportPlan;
                    break;
                case "CCO_Medications":
                    storeProcedure = MappedStoreProcedure.CCO_Medications;
                    break;
                case "CCO_CommunitySocial":
                    storeProcedure = MappedStoreProcedure.CCO_CommunitySocial;
                    break;
                default:
                case "CCO_Education":
                    storeProcedure = MappedStoreProcedure.CCO_Education;
                    break;
                case "CCO_TransitionPlanning":
                    storeProcedure = MappedStoreProcedure.CCO_TransitionPlanning;
                    break;
                case "CCO_Employment":
                    storeProcedure = MappedStoreProcedure.CCO_TransitionPlanning;
                    break;

            }
            
            return storeProcedure;
        }

        public static string GetFillablePDFPath(string tabName)
        {
            string pathName = string.Empty;
            switch (tabName)
            {
                case "OPWDD147":
                    pathName = ConfigurationManager.AppSettings["FillablePDF"].ToString()+"opwdd-147.pdf";
                    break;
                case "OPWDD148":
                    pathName =ConfigurationManager.AppSettings["FillablePDF"].ToString()+"opwdd-148.pdf";
                    break;
                case "OPWDD150":
                    pathName = ConfigurationManager.AppSettings["FillablePDF"].ToString()+"opwdd-150.pdf";
                    break;
                case "LifePlanPDF":
                    pathName = ConfigurationManager.AppSettings["FillablePDF"].ToString()+"Lifeplan.pdf";
                    break;
                default:
                    // code block
                    break;
            }
            return pathName;
        }

        public static Stream GetFilesStream(string fileUrl)
        {
            if (File.Exists(fileUrl))
            {
                return new FileStream(fileUrl, FileMode.Open);
            }
            else return new FileStream("", FileMode.Open);

        }
        /// <summary>
        /// This function is used to log the error into ErrorLog.txt file
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
  
        public static void LogError(Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
        /// <summary>
        /// This function is used to encode the folder
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeValues(string value)
        {
            var sha256CryptoService = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(value);
            var hashValue = sha256CryptoService.ComputeHash(byteValue);
            return Convert.ToBase64String(hashValue);
        }
        /// <summary>
        /// This function is used to return thee encoded value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEncodedFolderName(string value)
        {
            var encodedValue = EncodeValues(value);
            if (string.IsNullOrWhiteSpace(encodedValue))
            {
                encodedValue = encodedValue.Replace(@"\", "").Replace(@"/", "");
            }
            return encodedValue;
        }
    }
}
