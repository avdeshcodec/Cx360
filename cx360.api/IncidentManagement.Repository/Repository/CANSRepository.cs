using IncidentManagement.Entities.Common;
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.IRepository;
using IncidentManagement.Entities.XMLGeneration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.CANSResponse;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using System.Web;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Xml;

namespace IncidentManagement.Repository.Repository
{
    public class CANSRepository : ICANSRepository
    {
        #region Private
        CANSResponse cansResponse = null;
        private int batchID = -1;
        private string fileName = string.Empty;
        private int reportedBy = -1;
        CANSAssessmentPDFResponse cANSAssessmentPDFResponse = null;
        string ParentLegalGuardianSignature = string.Empty;
        #endregion

        public async Task<CANSResponse> InsertModifyCANSTabs(CANSRequest cansRequest)
        {

            bool checkVersion = true;

            if (cansRequest.TabName == "GeneralInformation")
            {
                checkVersion = ValidateCANSDraft(cansRequest);
            }
            cansResponse = new CANSResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(cansRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                if (checkVersion)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand(sp, con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            if (cansRequest.TabName == "UpdateGoalItems")
                            {
                                cmd.Parameters.Add("@goalid", SqlDbType.Int).Value = cansRequest.GoalID;
                                cmd.Parameters.Add("@goalstatus", SqlDbType.VarChar).Value = cansRequest.GoalStatus;
                                cmd.Parameters.Add("@completeddate", SqlDbType.Date).Value = cansRequest.CompletedDate;
                            }
                            if (cansRequest.JsonChildFirstTable != null)
                            {
                                cmd.Parameters.Add("@jsonchildfirst", SqlDbType.VarChar).Value = cansRequest.JsonChildFirstTable;
                            }
                            if (cansRequest.JsonChildSecondTable != null)
                            {
                                cmd.Parameters.Add("@jsonchildsecond", SqlDbType.VarChar).Value = cansRequest.JsonChildSecondTable;
                            }
                            if (cansRequest.JsonChildThirdTable != null)
                            {
                                cmd.Parameters.Add("@jsonchildthird", SqlDbType.VarChar).Value = cansRequest.JsonChildThirdTable;
                            }
                            if (cansRequest.Json != null)
                            {
                                cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = cansRequest.Json;

                            }
                            cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cansRequest.ReportedBy;
                            con.Open();

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                            sqlDataAdapter.SelectCommand = cmd;
                            await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                            con.Close();
                        }
                    }
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                        cansResponse.CommonCANSRsponse = JsonConvert.DeserializeObject<List<CommonCANSRsponse>>(dataSetString);
                    }
                }
                else
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Clear();
                    dataTable.Columns.Add("ValidatedRecord");
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["ValidatedRecord"] = false;
                    dataTable.Rows.Add(dataRow);
                    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTable);
                    cansResponse.CommonCANSRsponse = JsonConvert.DeserializeObject<List<CommonCANSRsponse>>(dataSetString);
                    return cansResponse;
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;

        }
        public bool ValidateCANSDraft(CANSRequest cansRequest)
        {
            bool recordValidated = true;

            DataTable dataSet = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ValidateCANSDraft", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.NVarChar).Value = cansRequest.Json;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cansRequest.ReportedBy;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                }
                if (dataSet.Rows.Count > 0)
                {
                    recordValidated = Convert.ToBoolean(dataSet.Rows[0]["Validated"]);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return recordValidated;
        }

        public async Task<CANSResponse> GetTreatmentPlanDetails(CANSRequest cansRequest)
        {
            cansResponse = new CANSResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(cansRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        if (cansRequest.TabName == "ChangeTreatmentPlanPosition")
                        {
                            cmd.Parameters.Add("@datasetxml", SqlDbType.VarChar).Value = cansRequest.Json;
                            cmd.Parameters.Add("@tabname", SqlDbType.VarChar).Value = cansRequest.TableName;
                            cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cansRequest.ReportedBy;

                        }
                        else if (cansRequest.TabName == "EditTreatmentPlan" || cansRequest.TabName == "DeleteTreatmentPlan")
                        {
                            cmd.Parameters.Add("@canstreatmentplanid", SqlDbType.Int).Value = cansRequest.TreatmentPlanId;
                            cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cansRequest.ReportedBy;
                            cmd.Parameters.Add("@tabname", SqlDbType.VarChar).Value = cansRequest.TabName;
                        }
                        else if (cansRequest.TabName == "TreatmentPlanDetails")
                        {
                            cmd.Parameters.Add("@treatmentplanid", SqlDbType.Int).Value = cansRequest.TreatmentPlanId;
                        }
                        else if (cansRequest.TabName == "ServiceInterventionObjecives")
                        {
                            cmd.Parameters.Add("@generalinformationid", SqlDbType.Int).Value = cansRequest.GeneralInformationID;
                        }
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    cansResponse.CommonCANSRsponse = JsonConvert.DeserializeObject<List<CommonCANSRsponse>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;

        }

        public async Task<CANSResponse> AddCansTreatmentPlanFields(TreatmentPlanRequest treatmentPlanRequest)
        {
            cansResponse = new CANSResponse();
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_AddCansTreatmentPlanFields", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        UpdateTreatmentPlanFieldsParam(treatmentPlanRequest);
                        cmd.Parameters.Add("@keyid", SqlDbType.Int).Value = treatmentPlanRequest.KeyId;
                        cmd.Parameters.Add("@tabname", SqlDbType.VarChar).Value = treatmentPlanRequest.TabName;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = treatmentPlanRequest.ReportedBy;
                        cmd.Parameters.Add("@parentid", SqlDbType.Int).Value = treatmentPlanRequest.ParentId;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = treatmentPlanRequest.Action;
                        cmd.Parameters.Add("@desc", SqlDbType.VarChar).Value = treatmentPlanRequest.Description;

                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    cansResponse.CommonCANSRsponse = JsonConvert.DeserializeObject<List<CommonCANSRsponse>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;
        }
        public void UpdateTreatmentPlanFieldsParam(TreatmentPlanRequest treatmentPlanRequest)
        {
            try
            {
                if (treatmentPlanRequest.Action == "DeleteTreatmentPlanObjective" || treatmentPlanRequest.Action == "InsertTreatmentPlanObjective" || treatmentPlanRequest.Action == "ModifyTreatmentPlanObjective")
                {
                    if (treatmentPlanRequest.Action == "DeleteTreatmentPlanObjective")
                    {
                        treatmentPlanRequest.Action = "Delete";
                    }
                    var data = (JObject)JsonConvert.DeserializeObject(treatmentPlanRequest.Json);
                    treatmentPlanRequest.ParentId = data["GoalId"].Value<int>();
                    treatmentPlanRequest.KeyId = data["ObjectiveId"].Value<int>();
                    treatmentPlanRequest.TabName = "TreatmentPlanObjectives";
                    treatmentPlanRequest.Description = data["Objective"].Value<string>();
                }
                else if (treatmentPlanRequest.Action == "DeleteTreatmentPlanGoal" || treatmentPlanRequest.Action == "InsertTreatmentPlanGoal" || treatmentPlanRequest.Action == "ModifyTreatmentPlanGoal")
                {
                    if (treatmentPlanRequest.Action == "DeleteTreatmentPlanGoal")
                    {
                        treatmentPlanRequest.Action = "Delete";
                    }
                    var data = (JObject)JsonConvert.DeserializeObject(treatmentPlanRequest.Json);
                    treatmentPlanRequest.ParentId = data["ItemId"].Value<int>();
                    treatmentPlanRequest.KeyId = data["GoalId"].Value<int>();
                    treatmentPlanRequest.TabName = "TreatmentPlanGoals";
                    treatmentPlanRequest.Description = data["Goal"].Value<string>();
                }
                else if (treatmentPlanRequest.Action == "DeleteTreatmentPlanItem" || treatmentPlanRequest.Action == "InsertTreatmentPlanItem" || treatmentPlanRequest.Action == "ModifyTreatmentPlanItem")
                {
                    if (treatmentPlanRequest.Action == "DeleteTreatmentPlanItem")
                    {
                        treatmentPlanRequest.Action = "Delete";
                    }
                    var data = (JObject)JsonConvert.DeserializeObject(treatmentPlanRequest.Json);
                    treatmentPlanRequest.ParentId = data["TreatmentPlanId"].Value<int>();
                    treatmentPlanRequest.KeyId = data["ItemId"].Value<int>();
                    treatmentPlanRequest.TabName = "TreatmentPlanItems";
                    treatmentPlanRequest.Description = data["Item"].Value<string>();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public async Task<CANSResponse> ManageServiceInterventionActions(CANSRequest cansRequest)
        {
            cansResponse = new CANSResponse();
            DataSet dataSet = new DataSet();
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ManageServiceInerventionRequests", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@serviceinterventionid", SqlDbType.Int).Value = cansRequest.ServiceInterventionID;
                        cmd.Parameters.Add("@generalinformationid", SqlDbType.Int).Value = cansRequest.GeneralInformationID;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cansRequest.ReportedBy;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = cansRequest.TabName;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    cansResponse.CommonCANSRsponse = JsonConvert.DeserializeObject<List<CommonCANSRsponse>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;
        }

        public async Task<CANSResponse> HandleCansVersioning(CANSRequest cansRequest)
        {
            cansResponse = new CANSResponse();
            DataSet dataSet = new DataSet();
            string sp = CommonFunctions.GetMappedStoreProcedure(cansRequest.TabName);
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@generalinformationid", SqlDbType.VarChar).Value = cansRequest.GeneralInformationID;
                        cmd.Parameters.Add("@cansverisoningid", SqlDbType.VarChar).Value = cansRequest.CansVersioningID;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cansRequest.ReportedBy;

                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;

                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));

                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {

                    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    cansResponse.NewVersionDetails = JsonConvert.DeserializeObject<List<NewVersionDetails>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;
        }


        public async Task<CANSResponse> GetCANSAssessmentDetails(CANSRequest cansRequest)
        {
            cansResponse = new CANSResponse();
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetCANSAssessmentDetails", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@generalInformationId", SqlDbType.VarChar).Value = cansRequest.GeneralInformationID;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0)
                {
                    string GeneralInformation = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    string TraumaExposure = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[1]);
                    string PresentingProblemAndImpact = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[2]);
                    string Safety = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[3]);
                    string SubstanceUseHistory = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[4]);
                    string PlacementHistory = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[5]);
                    string PsychiatricInformation = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[6]);
                    string ClientStrengths = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[7]);
                    string FamilyInformation = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[8]);
                    string NeedsResourceAssessment = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[9]);
                    string DSMDiagnosis = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[10]);
                    string MentalHealthSummary = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[11]);
                    string AddClientFunctioningEvaluations = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[12]);
                    string GeneralInformationHRA = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[13]);
                    string HealthStatus = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[15]);
                    string Medications = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[14]);
                    string DevelopmentHistory = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[16]);
                    string MedicalHistory = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[17]);
                    string CaregiverAddendum = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[18]);
                    string GeneralInformationDCFS = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[19]);
                    string SexuallyAggrBehavior = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[20]);
                    string ParentGuardSafety = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[21]);
                    string ParentGuardWellbeing = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[22]);
                    string ParentGuardPermananence = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[23]);
                    string SubstituteCommitPermananence = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[24]);
                    string IntactFamilyService = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[25]);
                    string IntensivePlacementStabilization = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[26]);

                    string AllChildTables = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[27]);

                    string IndividualTreatmentPlan = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[28]);
                    string CansSignature = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[29]);

                    cansResponse.GeneralInformation = JsonConvert.DeserializeObject<List<GeneralInformationSection>>(GeneralInformation);
                    cansResponse.TraumaExposure = JsonConvert.DeserializeObject<List<TraumaExposureSection>>(TraumaExposure);
                    cansResponse.PresentingProblemAndImpact = JsonConvert.DeserializeObject<List<PresentingProblemAndImpactSection>>(PresentingProblemAndImpact);
                    cansResponse.Safety = JsonConvert.DeserializeObject<List<SafetySection>>(Safety);
                    cansResponse.SubstanceUseHistory = JsonConvert.DeserializeObject<List<SubstanceUseHistorySection>>(SubstanceUseHistory);
                    cansResponse.PlacementHistory = JsonConvert.DeserializeObject<List<PlacementHistorySection>>(PlacementHistory);
                    cansResponse.PsychiatricInformation = JsonConvert.DeserializeObject<List<PsychiatricInformationSection>>(PsychiatricInformation);
                    cansResponse.ClientStrengths = JsonConvert.DeserializeObject<List<ClientStrengthsSection>>(ClientStrengths);
                    cansResponse.FamilyInformation = JsonConvert.DeserializeObject<List<FamilyInformationSection>>(FamilyInformation);
                    cansResponse.NeedsResourceAssessment = JsonConvert.DeserializeObject<List<NeedsResourceAssessmentSection>>(NeedsResourceAssessment);
                    cansResponse.DSMDiagnosis = JsonConvert.DeserializeObject<List<DSMDiagnosisSection>>(DSMDiagnosis);
                    cansResponse.MentalHealthSummary = JsonConvert.DeserializeObject<List<MentalHealthSummarySection>>(MentalHealthSummary);
                    cansResponse.AddClientFunctioningEvaluations = JsonConvert.DeserializeObject<List<AddClientFunctioningEvaluationsSection>>(AddClientFunctioningEvaluations);
                    cansResponse.GeneralInformationHRA = JsonConvert.DeserializeObject<List<GeneralInformationHRASection>>(GeneralInformationHRA);
                    cansResponse.HealthStatus = JsonConvert.DeserializeObject<List<HealthStatusSection>>(HealthStatus);
                    cansResponse.Medications = JsonConvert.DeserializeObject<List<MedicationsSection>>(Medications);
                    cansResponse.DevelopmentHistory = JsonConvert.DeserializeObject<List<DevelopmentHistorySection>>(DevelopmentHistory);
                    cansResponse.MedicalHistory = JsonConvert.DeserializeObject<List<MedicalHistorySection>>(MedicalHistory);
                    cansResponse.CaregiverAddendum = JsonConvert.DeserializeObject<List<CaregiverAddendumSection>>(CaregiverAddendum);
                    cansResponse.GeneralInformationDCFS = JsonConvert.DeserializeObject<List<GeneralInformationDCFSSection>>(GeneralInformationDCFS);
                    cansResponse.SexuallyAggrBehavior = JsonConvert.DeserializeObject<List<SexuallyAggrBehaviorSection>>(SexuallyAggrBehavior);
                    cansResponse.ParentGuardSafety = JsonConvert.DeserializeObject<List<ParentGuardSafetySection>>(ParentGuardSafety);
                    cansResponse.ParentGuardWellbeing = JsonConvert.DeserializeObject<List<ParentGuardWellbeingSection>>(ParentGuardWellbeing);
                    cansResponse.ParentGuardPermananence = JsonConvert.DeserializeObject<List<ParentGuardPermananenceSection>>(ParentGuardPermananence);
                    cansResponse.SubstituteCommitPermananence = JsonConvert.DeserializeObject<List<SubstituteCommitPermananenceSection>>(SubstituteCommitPermananence);
                    cansResponse.IntactFamilyService = JsonConvert.DeserializeObject<List<IntactFamilyServiceSection>>(IntactFamilyService);
                    cansResponse.IntensivePlacementStabilization = JsonConvert.DeserializeObject<List<IntensivePlacementStabilizationSection>>(IntensivePlacementStabilization);
                    cansResponse.IndividualTreatmentPlan = JsonConvert.DeserializeObject<List<IndividualTreatmentPlan>>(IndividualTreatmentPlan);
                    cansResponse.CansSignature = JsonConvert.DeserializeObject<List<CansSignature>>(CansSignature);
                    cansResponse.AllChildTables = JsonConvert.DeserializeObject<List<AllChildTables>>(AllChildTables);

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;
        }


        public async Task<CANSResponse> GenerateAndImportXML(XMLImportRequest xmlImportRequest)
        {
            cansResponse = new CANSResponse();
            DataSet dataSet = new DataSet();
            string importedXML = string.Empty;
            string xmlFilePath = string.Empty;
            reportedBy = Convert.ToInt32(xmlImportRequest.ReportedBy);
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_ImportCansXML", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@begindate", SqlDbType.VarChar).Value = xmlImportRequest.BeginDate;
                        cmd.Parameters.Add("@endate", SqlDbType.VarChar).Value = xmlImportRequest.EndDate;
                        cmd.Parameters.Add("@importtype", SqlDbType.VarChar).Value = xmlImportRequest.ImportType;
                        cmd.Parameters.Add("@recordtotal", SqlDbType.VarChar).Value = xmlImportRequest.RecordTotal;
                        cmd.Parameters.Add("@runtype", SqlDbType.VarChar).Value = xmlImportRequest.RunType;
                        cmd.Parameters.Add("@reportedby", SqlDbType.VarChar).Value = xmlImportRequest.ReportedBy;
                        con.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (xmlImportRequest.ImportType == "HFSStaging" && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    batchID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["BatchID"]);
                    DateTime currentUTC = DateTime.UtcNow;
                    string strTemp = currentUTC.ToString("yyyyMMddHHmmss");
                    fileName = batchID + "_" + strTemp;
                    xmlFilePath = GenerateHFSXML(dataSet);
                }
                else if (xmlImportRequest.ImportType == "HRAStaging" && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    batchID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["BatchID"]);
                    DateTime currentUTC = DateTime.UtcNow;
                    string strTemp = currentUTC.ToString("yyyyMMddHHmmss");
                    fileName = batchID +"_" +strTemp;
                    xmlFilePath = GenerateHRAXML(dataSet);
                }
                else
                {
                    xmlFilePath = "";
                }
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("FileName");
                DataRow dataRow = dataTable.NewRow();
                dataRow["FileName"] = xmlFilePath;
                dataTable.Rows.Add(dataRow);
                string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTable);
                cansResponse.CommonCANSRsponse = JsonConvert.DeserializeObject<List<CommonCANSRsponse>>(dataSetString);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cansResponse;
        }

        public string GenerateHFSXML(DataSet dataset)
        {
            string importedXML = string.Empty;
            DataSet dataSetCans = new DataSet();
            DataTable dataTable = (dataset.Tables[1].DefaultView).ToTable();
            string xmlClientRow = string.Empty;
            try
            {
                importedXML = HFSXML.GenerateHeaderTrailer(dataset, importedXML);
                XDocument root = XDocument.Parse(importedXML);
                XElement header = root.Descendants().Where(x => x.Name.LocalName == "header").FirstOrDefault();
                foreach (DataRow rows in dataTable.Rows)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                    {
                        //Create the SqlCommand object
                        using (SqlCommand cmd = new SqlCommand("usp_GetHFSXMLData", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add("@generalinformationid", SqlDbType.Int).Value = Convert.ToInt32(rows["GeneralInformationID"]);
                            cmd.Parameters.Add("@cansversioningid", SqlDbType.Int).Value = Convert.ToInt32(rows["CansVersioningID"]);
                            cmd.Parameters.Add("@clientid", SqlDbType.Int).Value = Convert.ToInt32(rows["ClientID"]);
                            cmd.Parameters.Add("@canshfsxmlstagingid", SqlDbType.Int).Value = Convert.ToInt32(rows["CansHFSXMLStagingID"]);
                            con.Open();

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                            sqlDataAdapter.SelectCommand = cmd;
                            sqlDataAdapter.Fill(dataSetCans);
                            con.Close();
                        }
                    }
                    xmlClientRow = HFSXML.GenerateHFSClientRow(dataSetCans, xmlClientRow);
                    XDocument row = XDocument.Parse(xmlClientRow);
                    XElement rowData = row.Descendants().Where(x => x.Name.LocalName == "row").FirstOrDefault();
                    var count = root.Descendants("row").Count();
                    if (count == 0)
                    {
                        root.Root.Element("header").AddAfterSelf(rowData);
                    }
                    else
                    {
                        root.Descendants("row").Where(x => x.Name.LocalName == "row").Last().AddAfterSelf(rowData);

                    }
                    dataSetCans.Clear();
                }
                root.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
                string filePath = CreateXMLFile(root, "HFSStaging");
             return   filePath = UpdateXMLFile(filePath, "HFSStaging");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
           
        }
        public string GenerateHRAXML(DataSet dataset )
        {
            string importedXML = string.Empty;
            DataSet dataSetCans = new DataSet();
            DataTable dataTable = (dataset.Tables[1].DefaultView).ToTable();
            string xmlClientRow = string.Empty;
            try
            {
                importedXML = HRAXML.GenerateHeaderTrailer(dataset, importedXML);
                XDocument root = XDocument.Parse(importedXML);
                XElement header = root.Descendants().Where(x => x.Name.LocalName == "header").FirstOrDefault();
                foreach (DataRow rows in dataTable.Rows)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                    {
                        //Create the SqlCommand object
                        using (SqlCommand cmd = new SqlCommand("usp_GetHRAXMLData", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add("@generalinformationid", SqlDbType.Int).Value = Convert.ToInt32(rows["GeneralInformationID"]);
                            cmd.Parameters.Add("@cansversioningid", SqlDbType.Int).Value = Convert.ToInt32(rows["CansVersioningID"]);
                            cmd.Parameters.Add("@clientid", SqlDbType.Int).Value = Convert.ToInt32(rows["ClientID"]);
                            cmd.Parameters.Add("@canshraxmlstagingid", SqlDbType.Int).Value = Convert.ToInt32(rows["CansHRAXMLStagingID"]);
                            con.Open();

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                            sqlDataAdapter.SelectCommand = cmd;
                            sqlDataAdapter.Fill(dataSetCans);
                            con.Close();
                        }
                    }
                    xmlClientRow = HRAXML.GenerateHRAClientRow(dataSetCans, xmlClientRow);
                    XDocument row = XDocument.Parse(xmlClientRow);
                    XElement rowData = row.Descendants().Where(x => x.Name.LocalName == "row").FirstOrDefault();
                    var count = root.Descendants("row").Count();
                    if (count == 0)
                    {
                        root.Root.Element("header").AddAfterSelf(rowData);
                    }
                    else
                    {
                        root.Descendants("row").Where(x => x.Name.LocalName == "row").Last().AddAfterSelf(rowData);
                    }
                    dataSetCans.Clear();
                }
                root.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
                string filePath = CreateXMLFile(root, "HRAStaging");
                filePath = UpdateXMLFile(filePath, "HRAStaging");

                return filePath;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
           
        }
        public string CreateXMLFile(XDocument doc,string cansFileType)
        {
            string uloadPath = string.Empty;
            
            string documentUplpoadPath = string.Empty;
            try
            {
                if (cansFileType == "HRAStaging")
                {
                     uloadPath = ConfigurationManager.AppSettings["HRAXML"];

                }
                else
                {
                     uloadPath = ConfigurationManager.AppSettings["HFSXML"];
                }
                DirectoryInfo di = new DirectoryInfo(uloadPath);
                di = new DirectoryInfo(uloadPath);
                if (!string.IsNullOrEmpty(uloadPath))
                {
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    documentUplpoadPath = uloadPath + @"\" + batchID;
                    di = new DirectoryInfo(documentUplpoadPath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                documentUplpoadPath = di.FullName + @"\" + fileName + ".xml";

                if (!File.Exists(documentUplpoadPath))
                {

                    // SAVE THE FILES IN THE FOLDER.
                    doc.Save(documentUplpoadPath);

                }
                return documentUplpoadPath;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public string UpdateXMLFile(string filePath, string xmlFileType)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_UpdateCANSXMLFilePath", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@batchid", SqlDbType.Int).Value = batchID;
                        cmd.Parameters.Add("@filepath", SqlDbType.VarChar).Value = filePath;
                        cmd.Parameters.Add("@xmlfiletype", SqlDbType.VarChar).Value = xmlFileType;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = reportedBy;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataTable);
                        con.Close();
                    }

                }
                if (dataTable.Rows.Count > 0)
                {
                    filePath = dataTable.Rows[0]["XMLFilePath"].ToString();
                  
                }
                return filePath;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public async Task<CANSAssessmentPDFResponse> PrintAssessmentPDF(CANSRequest cansRequest)
        {
            cANSAssessmentPDFResponse = new CANSAssessmentPDFResponse();
            string pdfTemplate = CommonFunctions.GetFillablePDFPath(cansRequest.TabName);
            string newTemplatePDf = string.Empty;
            try
            {
                newTemplatePDf = GetCANSAssessmentPDFTemplate(cansRequest.TabName, ConfigurationManager.AppSettings["FillablePDF"].ToString() + "Cans_Assessment.pdf", cansRequest);

                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("FileName");
                DataRow dataRow = dataTable.NewRow();
                dataRow["FileName"] = newTemplatePDf;
                dataTable.Rows.Add(dataRow);
                string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTable);
                cANSAssessmentPDFResponse.CANSAssessmentPDF = JsonConvert.DeserializeObject<List<CANSAssessmentPDF>>(dataSetString);
                return cANSAssessmentPDFResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private string GetCANSAssessmentPDFTemplate(string tabName, string pdfPath, CANSRequest fillablePDFRequest)
        {
            DataSet dataSet = new DataSet();
            string newpdfPath = string.Empty;
            try
            {
                string storeProcedure = CommonFunctions.GetMappedStoreProcedure(tabName);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetCANSAssessmentDetailsPDF", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@generalInformationId", SqlDbType.Int).Value = fillablePDFRequest.GeneralInformationID;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0)
                {

                    newpdfPath = CANSAssessmentDFTemplate(pdfPath, dataSet, fillablePDFRequest);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return newpdfPath;
        }
        public string CANSAssessmentDFTemplate(string pdfPath, DataSet dataSetFillPDF, CANSRequest fillablePDFRequest)
        {
            string newFile = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "Cans_Completed_Assessment.pdf";
            string NewCANSDocument = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "NewCANSDocument.pdf";
            string newFileAddMedicalHistory = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "NewCANSMedicalHistory.pdf";
            string newFileAddPrioritzedNeedsStrength = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "NewCANSDocument.pdf";
            string newFileAddHealthMedication = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "NewCANSPrioritzedNeedsStrength.pdf";
            string newFileAddCANSAssessment = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "NewAddCANSAssessment.pdf";

            try
            {
                PdfReader pdfReader = new PdfReader(pdfPath);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;

                DataTable dataTableGeneralInformation = dataSetFillPDF.Tables[0];
                DataTable dataTableTraumaExposure = dataSetFillPDF.Tables[1];
                DataTable dataTablePresentingProblemAndImpact = dataSetFillPDF.Tables[2];
                DataTable dataTableSafety = dataSetFillPDF.Tables[3];
                DataTable dataTableSubstanceUseHistory = dataSetFillPDF.Tables[4];
                DataTable dataTablePlacementHistory = dataSetFillPDF.Tables[5];
                DataTable dataTablePsychiatricInformation = dataSetFillPDF.Tables[6];
                DataTable dataTableClientStrengths = dataSetFillPDF.Tables[7];
                DataTable dataTableFamilyInformation = dataSetFillPDF.Tables[8];
                DataTable dataTableNeedsResourceAssessment = dataSetFillPDF.Tables[9];
                DataTable dataTableDSMDiagnosis = dataSetFillPDF.Tables[10];
                DataTable dataTableMentalHealthSummary = dataSetFillPDF.Tables[11];
                DataTable dataTableAddClientFunctioningEvaluations = dataSetFillPDF.Tables[12];
                DataTable dataTableGeneralInformationHRA = dataSetFillPDF.Tables[13];
                DataTable dataTableHealthStatus = dataSetFillPDF.Tables[15];
                DataTable dataTableMedications = dataSetFillPDF.Tables[14];
                DataTable dataTableDevelopmentHistory = dataSetFillPDF.Tables[16];
                DataTable dataTableMedicalHistory = dataSetFillPDF.Tables[17];
                DataTable dataTableCaregiverAddendum = dataSetFillPDF.Tables[18];
                DataTable dataTableGeneralInformationDCFS = dataSetFillPDF.Tables[19];
                DataTable dataTableSexuallyAggrBehavior = dataSetFillPDF.Tables[20];
                DataTable dataTableParentGuardSafety = dataSetFillPDF.Tables[21];
                DataTable dataTableParentGuardWellbeing = dataSetFillPDF.Tables[22];
                DataTable dataTableParentGuardPermananence = dataSetFillPDF.Tables[23];
                DataTable dataTableSubstituteCommitPermananence = dataSetFillPDF.Tables[24];
                DataTable dataTableIntactFamilyService = dataSetFillPDF.Tables[25];
                DataTable dataTableIntensivePlacementStabilization = dataSetFillPDF.Tables[26];
                DataTable dataTableSignatures = dataSetFillPDF.Tables[40];

                DataTable dataTableAllChildTables = dataSetFillPDF.Tables[27];

                DataTable dataTableIndividualTreatmentPlan = dataSetFillPDF.Tables[39];

                pdfFormFields.GenerateAppearances = false;

                string status = string.Empty;


                if (dataTableGeneralInformation.Rows.Count > 0)
                {
                    DataRow row = dataTableGeneralInformation.Rows[0];
                    pdfFormFields.SetField("CansType", row["CansType"].ToString());
                    pdfFormFields.SetField("ClientName", row["ClientName"].ToString());
                    pdfFormFields.SetField("Dateofbirth", row["Dateofbirth"].ToString());
                    pdfFormFields.SetField("Rin", row["Rin"].ToString());
                    pdfFormFields.SetField("Gender", row["Gender"].ToString());
                    pdfFormFields.SetField("RefSource", row["RefSource"].ToString());
                    pdfFormFields.SetField("DateFirstCont", row["DateFirstCont"].ToString());
                    pdfFormFields.SetField("Phone", row["Phone"].ToString());
                    pdfFormFields.SetField("Language", row["Language"].ToString());
                    pdfFormFields.SetField("InterpreterServices", row["InterpreterServices"].ToString());
                    pdfFormFields.SetField("InterSpoken", row["InterSpoken"].ToString());
                    pdfFormFields.SetField("InterOther", row["InterOther"].ToString());
                    //pdfFormFields.SetField("Address", row["Address"].ToString() + " " + row["City"].ToString() + " " + row["State"].ToString());
                    pdfFormFields.SetField("Address", row["Address"].ToString() + " " + row["City"].ToString() + " " + row["State"].ToString() + " " + row["ZipCode"].ToString());
                    pdfFormFields.SetField("City", row["City"].ToString());
                    pdfFormFields.SetField("State", row["State"].ToString());
                    pdfFormFields.SetField("ZipCode", row["ZipCode"].ToString());
                    pdfFormFields.SetField("County", row["County"].ToString());
                    pdfFormFields.SetField("UsCitizen", row["UsCitizen"].ToString());
                    pdfFormFields.SetField("Race", row["Race"].ToString());
                    pdfFormFields.SetField("Ethnicity", row["Ethnicity"].ToString());
                    pdfFormFields.SetField("InsurCoverage", row["InsurCoverage"].ToString());
                    pdfFormFields.SetField("InsuranceCompany", row["InsuranceCompany"].ToString());
                    pdfFormFields.SetField("DCFSInvolvements", row["DCFSInvolvement"].ToString());
                    pdfFormFields.SetField("Caregiver", row["Caregiver"].ToString());

                    pdfFormFields.SetField("HouseHoldSize", row["HouseHoldSize"].ToString());
                    pdfFormFields.SetField("HouseHoldIncome", row["HouseHoldIncome"].ToString());
                    pdfFormFields.SetField("MaritalStatus", row["MaritalStatus"].ToString());
                    pdfFormFields.SetField("GuardianStatus", row["GuardianStatus"].ToString());
                    pdfFormFields.SetField("GuardStatusOth", row["GuardStatusOth"].ToString());
                    pdfFormFields.SetField("EmploymentStatus", row["EmploymentStatus"].ToString());
                    pdfFormFields.SetField("LivingArrangement", row["LivingArrangement"].ToString());
                    pdfFormFields.SetField("livArrangeOther", row["livArrangeOther"].ToString());
                    pdfFormFields.SetField("EducationLevel", row["EducationLevel"].ToString());
                    pdfFormFields.SetField("ParentFirstName", row["ParentFirstName"].ToString());
                    pdfFormFields.SetField("ParentLastName", row["ParentLastName"].ToString());
                    pdfFormFields.SetField("RelationshipToClient", row["RelationshipToClient"].ToString());
                    pdfFormFields.SetField("ParentPhone", row["ParentPhone"].ToString());
                    pdfFormFields.SetField("ParentAddress", row["ParentAddress"].ToString());
                    pdfFormFields.SetField("ParentCity", row["ParentCity"].ToString());
                    pdfFormFields.SetField("ParentZip", row["ParentZip"].ToString());
                    pdfFormFields.SetField("ParentState", row["ParentState"].ToString());
                    pdfFormFields.SetField("ParentCounty", row["ParentCounty"].ToString());
                    pdfFormFields.SetField("EmergConFirstName", row["EmergConFirstName"].ToString());
                    pdfFormFields.SetField("EmergConLastName", row["EmergConLastName"].ToString());
                    pdfFormFields.SetField("EcRelToClient", row["EcRelToClient"].ToString());
                    pdfFormFields.SetField("EcAddress", row["EcAddress"].ToString());
                    pdfFormFields.SetField("EcCity", row["EcCity"].ToString());
                    pdfFormFields.SetField("EcState", row["EcState"].ToString());
                    pdfFormFields.SetField("EcZip", row["EcZip"].ToString());
                    pdfFormFields.SetField("EcPhone", row["EcPhone"].ToString());
                    pdfFormFields.SetField("DocumentStatus", row["DocumentStatus"].ToString());
                    pdfFormFields.SetField("DocumentVersion", row["DocumentVersion"].ToString());

                    status = row["DocumentStatus"].ToString();

                    //fillMedicalSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    //fillMentalHealthSectionPDF(pdfFormFields, dataSetFillPDF, status);
                }
                if (dataTableTraumaExposure.Rows.Count > 0)
                {
                    DataRow row = dataTableTraumaExposure.Rows[0];
                    pdfFormFields.SetField("TeDisruptionsCaregiving", row["TeDisruptionsCaregiving$"].ToString());
                    pdfFormFields.SetField("TeEmotionalAbuse", row["TeEmotionalAbuse$"].ToString());
                    pdfFormFields.SetField("TeMedicalTrauma", row["TeMedicalTrauma$"].ToString());
                    pdfFormFields.SetField("TeNaturalDisaster", row["TeNaturalDisaster$"].ToString());
                    pdfFormFields.SetField("TeNeglect", row["TeNeglect$"].ToString());
                    pdfFormFields.SetField("TeParentalCriminalBehavior", row["TeParentalCriminalBehavior$"].ToString());
                    pdfFormFields.SetField("TePhysicalAbuse", row["TePhysicalAbuse$"].ToString());
                    pdfFormFields.SetField("TeSexualAbuse", row["TeSexualAbuse$"].ToString());
                    pdfFormFields.SetField("TeVictimCriminalActivity", row["TeVictimCriminalActivity$"].ToString());
                    pdfFormFields.SetField("TeWarTerrorismAffected", row["TeWarTerrorismAffected$"].ToString());
                    pdfFormFields.SetField("TeWitnessCommunityViolence", row["TeWitnessCommunityViolence$"].ToString());
                    pdfFormFields.SetField("TewWitnessFamilyViolence", row["TewWitnessFamilyViolence$"].ToString());
                    pdfFormFields.SetField("TeSupportingInfo", row["TeSupportingInfo"].ToString());
                    pdfFormFields.SetField("StatusTrauma", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "StatusTrauma");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "StatusTrauma");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "StatusTrauma");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "StatusTrauma");
                    }
                }
                if (dataTablePresentingProblemAndImpact.Rows.Count > 0)
                {
                    DataRow row = dataTablePresentingProblemAndImpact.Rows[0];
                    pdfFormFields.SetField("BeNeedAdjustTrauma", row["BeNeedAdjustTrauma$"].ToString());
                    pdfFormFields.SetField("BeNeedAngerControl", row["BeNeedAngerControl$"].ToString());
                    pdfFormFields.SetField("BeNeedAntisocial", row["BeNeedAntisocial$"].ToString());
                    pdfFormFields.SetField("BeNeedAnxiety", row["BeNeedAnxiety$"].ToString());
                    pdfFormFields.SetField("BeNeedAtypical", row["BeNeedAtypical$"].ToString());
                    pdfFormFields.SetField("BeNeedDepression", row["BeNeedDepression$"].ToString());
                    pdfFormFields.SetField("BeNeedEatingDist", row["BeNeedEatingDist$"].ToString());
                    pdfFormFields.SetField("BeNeedFailTrive", row["BeNeedFailTrive$"].ToString());
                    pdfFormFields.SetField("BeNeedImpulsivity", row["BeNeedImpulsivity$"].ToString());
                    pdfFormFields.SetField("BeNeedInterpersonal", row["BeNeedInterpersonal$"].ToString());
                    pdfFormFields.SetField("BeNeedMania", row["BeNeedMania$"].ToString());
                    pdfFormFields.SetField("BeNeedOppositional", row["BeNeedOppositional$"].ToString());
                    pdfFormFields.SetField("BeNeedPsychosis", row["BeNeedPsychosis$"].ToString());
                    pdfFormFields.SetField("BeNeedRegulatory", row["BeNeedRegulatory$"].ToString());
                    pdfFormFields.SetField("BeNeedSomatization", row["BeNeedSomatization$"].ToString());
                    pdfFormFields.SetField("BeNeedSubstance", row["BeNeedSubstance$"].ToString());
                    pdfFormFields.SetField("TraumaAttachment", row["TraumaAttachment$"].ToString());
                    pdfFormFields.SetField("TraumaAvoidance", row["TraumaAvoidance$"].ToString());
                    pdfFormFields.SetField("TraumaDissaociation", row["TraumaDissaociation$"].ToString());
                    pdfFormFields.SetField("TraumaDysregulation", row["TraumaDysregulation$"].ToString());
                    pdfFormFields.SetField("TraumaGrief", row["TraumaGrief$"].ToString());
                    pdfFormFields.SetField("TraumaHyperarousal", row["TraumaHyperarousal$"].ToString());
                    pdfFormFields.SetField("TraumaIntrusions", row["TraumaIntrusions$"].ToString());
                    pdfFormFields.SetField("TraumaNumbing", row["TraumaNumbing$"].ToString());
                    pdfFormFields.SetField("LifeBasicActivities", row["LifeBasicActivities$"].ToString());
                    pdfFormFields.SetField("LifeCommunication", row["LifeCommunication$"].ToString());
                    pdfFormFields.SetField("LifeDecisionMaking", row["LifeDecisionMaking$"].ToString());
                    pdfFormFields.SetField("LifeDevelopmental", row["LifeDevelopmental$"].ToString());
                    pdfFormFields.SetField("LifeElimination", row["LifeElimination$"].ToString());
                    pdfFormFields.SetField("LifeFamFunctioning", row["LifeFamFunctioning$"].ToString());
                    pdfFormFields.SetField("LifeFunctionalCommunication", row["LifeFunctionalCommunication$"].ToString());
                    pdfFormFields.SetField("LifeIndependentLiving", row["LifeIndependentLiving$"].ToString());
                    pdfFormFields.SetField("LifeIntimateRelationships", row["LifeIntimateRelationships$"].ToString());
                    pdfFormFields.SetField("LifeJobFunctioning", row["LifeJobFunctioning$"].ToString());
                    pdfFormFields.SetField("LifeLegal", row["LifeLegal$"].ToString());
                    pdfFormFields.SetField("LifeLivingSituation", row["LifeLivingSituation$"].ToString());
                    pdfFormFields.SetField("LifeLoneliness", row["LifeLoneliness$"].ToString());
                    pdfFormFields.SetField("LifeMedicalPhysical", row["LifeMedicalPhysical$"].ToString());
                    pdfFormFields.SetField("LifeMedication", row["LifeMedication$"].ToString());
                    pdfFormFields.SetField("LifeMotor", row["LifeMotor$"].ToString());
                    pdfFormFields.SetField("LifeParentalRole", row["LifeParentalRole$"].ToString());
                    pdfFormFields.SetField("LifePersistence", row["LifePersistence$"].ToString());
                    pdfFormFields.SetField("LifeRecreation", row["LifeRecreation$"].ToString());
                    pdfFormFields.SetField("LifeResidentialStability", row["LifeResidentialStability$"].ToString());
                    pdfFormFields.SetField("LifeRoutines", row["LifeRoutines$"].ToString());
                    pdfFormFields.SetField("LifeSchoolPreschoolDaycare", row["LifeSchoolPreschoolDaycare$"].ToString());
                    pdfFormFields.SetField("LifeSensory", row["LifeSensory$"].ToString());
                    pdfFormFields.SetField("LifeSexualDevelopment", row["LifeSexualDevelopment$"].ToString());
                    pdfFormFields.SetField("LifeSleep", row["LifeSleep$"].ToString());
                    pdfFormFields.SetField("LifeSocialFunctioning", row["LifeSocialFunctioning$"].ToString());
                    pdfFormFields.SetField("LifeTransportation", row["LifeTransportation$"].ToString());
                    pdfFormFields.SetField("DDAutism", row["DDAutism$"].ToString());
                    pdfFormFields.SetField("DDCognivtive", row["DDCognivtive$"].ToString());
                    pdfFormFields.SetField("DDDevelopmental", row["DDDevelopmental$"].ToString());
                    pdfFormFields.SetField("DDMotor", row["DDMotor$"].ToString());
                    pdfFormFields.SetField("DDRegulatory", row["DDRegulatory$"].ToString());
                    pdfFormFields.SetField("DDSelfcare", row["DDSelfcare$"].ToString());
                    pdfFormFields.SetField("DDSensory", row["DDSensory$"].ToString());
                    pdfFormFields.SetField("SPDAchievement", row["SPDAchievement$"].ToString());
                    pdfFormFields.SetField("SPDAttendance", row["SPDAttendance$"].ToString());
                    pdfFormFields.SetField("SPDBehavior", row["SPDBehavior$"].ToString());
                    pdfFormFields.SetField("SPDPreschoolDaycare", row["SPDPreschoolDaycare$"].ToString());
                    pdfFormFields.SetField("SPDTeacherRelationship", row["SPDTeacherRelationship$"].ToString());
                    // pdfFormFields.SetField("SPDSchoolneeds", row["SPDSchoolneeds"].ToString());
                    pdfFormFields.SetField("EmploymentCareerAspirations", row["EmploymentCareerAspirations$"].ToString());
                    pdfFormFields.SetField("EmploymentJobAttendance", row["EmploymentJobAttendance$"].ToString());
                    pdfFormFields.SetField("EmploymentJobPerformance", row["EmploymentJobPerformance$"].ToString());
                    pdfFormFields.SetField("EmploymentJobRelations", row["EmploymentJobRelations$"].ToString());
                    pdfFormFields.SetField("EmploymentJobSkills", row["EmploymentJobSkills$"].ToString());
                    pdfFormFields.SetField("EmploymentJobTime", row["EmploymentJobTime$"].ToString());
                    pdfFormFields.SetField("ParentingInvolvement", row["ParentingInvolvement$"].ToString());
                    pdfFormFields.SetField("ParentingKnowledgeOfNeeds", row["ParentingKnowledgeOfNeeds$"].ToString());
                    pdfFormFields.SetField("ParentingMaritalViolence", row["ParentingMaritalViolence$"].ToString());
                    pdfFormFields.SetField("ParentingOrganization", row["ParentingOrganization$"].ToString());
                    pdfFormFields.SetField("ParentingSupervision", row["ParentingSupervision$"].ToString());
                    pdfFormFields.SetField("IndependentCommDeviceUse", row["IndependentCommDeviceUse$"].ToString());
                    pdfFormFields.SetField("IndependentHouseWork", row["IndependentHouseWork$"].ToString());
                    pdfFormFields.SetField("IndependentHousingSafety", row["IndependentHousingSafety$"].ToString());
                    pdfFormFields.SetField("IndependentMealPrep", row["IndependentMealPrep$"].ToString());
                    pdfFormFields.SetField("IndependentMoneyManagement", row["IndependentMoneyManagement$"].ToString());
                    pdfFormFields.SetField("IndependentShopping", row["IndependentShopping$"].ToString());
                    pdfFormFields.SetField("PresentingpProbSuppInfo", row["PresentingpProbSuppInfo"].ToString());
                    pdfFormFields.SetField("PresentingProblemImpactStatus", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "PresentingProblemImpactStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "PresentingProblemImpactStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "PresentingProblemImpactStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "PresentingProblemImpactStatus");
                    }
                }
                if (dataTableSafety.Rows.Count > 0)
                {
                    DataRow row = dataTableSafety.Rows[0];
                    pdfFormFields.SetField("Rbvictexpl", row["Rbvictexpl$"].ToString());
                    pdfFormFields.SetField("Rbselfharm", row["Rbselfharm$"].ToString());
                    pdfFormFields.SetField("Rbflightrisk", row["Rbflightrisk$"].ToString());
                    pdfFormFields.SetField("Rbsuiciderisk", row["Rbsuiciderisk$"].ToString());
                    pdfFormFields.SetField("Rbintenmisb", row["Rbintenmisb$"].ToString());
                    pdfFormFields.SetField("Rbrunaway", row["Rbrunaway$"].ToString());
                    pdfFormFields.SetField("Rbsexprobehav", row["Rbsexprobehav$"].ToString());
                    pdfFormFields.SetField("Rbbullying", row["Rbbullying$"].ToString());
                    pdfFormFields.SetField("Rbdelcrimbehav", row["Rbdelcrimbehav$"].ToString());
                    pdfFormFields.SetField("Rbselfmutil", row["Rbselfmutil$"].ToString());
                    pdfFormFields.SetField("Rbothselfharm", row["Rbothselfharm$"].ToString());
                    pdfFormFields.SetField("Rbdngrtothers", row["Rbdngrtothers$"].ToString());
                    pdfFormFields.SetField("Rbfiresetting", row["Rbfiresetting$"].ToString());
                    pdfFormFields.SetField("Rbgrdisability", row["Rbgrdisability$"].ToString());
                    pdfFormFields.SetField("Rbhoarding", row["Rbhoarding$"].ToString());
                    pdfFormFields.SetField("Runfrequency", row["Runfrequency$"].ToString());
                    pdfFormFields.SetField("Runconsistdest", row["Runconsistdest$"].ToString());
                    pdfFormFields.SetField("runsafetydest", row["runsafetydest$"].ToString());
                    pdfFormFields.SetField("Runillegacts", row["Runillegacts$"].ToString());
                    pdfFormFields.SetField("Runreturnonown", row["Runreturnonown$"].ToString());
                    pdfFormFields.SetField("Runinvolvothers", row["Runinvolvothers$"].ToString());
                    pdfFormFields.SetField("Runrealexpect", row["Runrealexpect$"].ToString());
                    pdfFormFields.SetField("Runplanning", row["Runplanning$"].ToString());
                    pdfFormFields.SetField("Spbhypersex", row["Spbhypersex$"].ToString());
                    pdfFormFields.SetField("Spbhirisksexbeh", row["Spbhirisksexbeh$"].ToString());
                    pdfFormFields.SetField("Spbmastur", row["Spbmastur$"].ToString());
                    pdfFormFields.SetField("Spbsexaggr", row["Spbsexaggr$"].ToString());
                    pdfFormFields.SetField("Spbsexreactbeh", row["Spbsexreactbeh$"].ToString());
                    pdfFormFields.SetField("Sabrelationship", row["Sabrelationship$"].ToString());
                    pdfFormFields.SetField("Sabphysforce", row["Sabphysforce$"].ToString());
                    pdfFormFields.SetField("Sabplanning", row["Sabplanning$"].ToString());
                    pdfFormFields.SetField("Sabagediff", row["Sabagediff$"].ToString());
                    pdfFormFields.SetField("Sabpowerdifferential", row["Sabpowerdifferential$"].ToString());
                    pdfFormFields.SetField("Sabtypesexact", row["Sabtypesexact$"].ToString());
                    pdfFormFields.SetField("Sabresptoaccusation", row["Sabresptoaccusation$"].ToString());
                    pdfFormFields.SetField("Dangerousnesshostility", row["Dangerousnesshostility$"].ToString());
                    pdfFormFields.SetField("Dangerousnessparanthinking", row["Dangerousnessparanthinking$"].ToString());
                    pdfFormFields.SetField("Dangerousnessecondgainsanger", row["Dangerousnessecondgainsanger$"].ToString());
                    pdfFormFields.SetField("Dangerousnessviolenthinking", row["Dangerousnessviolenthinking$"].ToString());
                    pdfFormFields.SetField("Dangerousnessintent", row["Dangerousnessintent$"].ToString());
                    pdfFormFields.SetField("Dangerousnessplanning", row["Dangerousnessplanning$"].ToString());
                    pdfFormFields.SetField("Dangerousnessviolencehistory", row["Dangerousnessviolencehistory$"].ToString());
                    pdfFormFields.SetField("Dangerousnessawareviolencepotential", row["Dangerousnessawareviolencepotential$"].ToString());
                    pdfFormFields.SetField("Dangerousnessresptoconsequences", row["Dangerousnessresptoconsequences$"].ToString());
                    pdfFormFields.SetField("Dangerousnesscommitselfctrl", row["Dangerousnesscommitselfctrl$"].ToString());
                    pdfFormFields.SetField("Firesetseriousness", row["Firesetseriousness$"].ToString());
                    pdfFormFields.SetField("Firesethistory", row["Firesethistory$"].ToString());
                    pdfFormFields.SetField("Firesetplanning", row["Firesetplanning$"].ToString());
                    pdfFormFields.SetField("Firesetuseaccelerants", row["Firesetuseaccelerants$"].ToString());
                    pdfFormFields.SetField("Firesetintentoharm", row["Firesetintentoharm$"].ToString());
                    pdfFormFields.SetField("Firesetcommunsafety", row["Firesetcommunsafety$"].ToString());
                    pdfFormFields.SetField("Firesetresponsetoaccusation", row["Firesetresponsetoaccusation$"].ToString());
                    pdfFormFields.SetField("Firesetremorse", row["Firesetremorse$"].ToString());
                    pdfFormFields.SetField("Firesetlikelihoodfuturefireset", row["Firesetlikelihoodfuturefireset$"].ToString());
                    pdfFormFields.SetField("Riskbehaviorsupportinfo", row["Riskbehaviorsupportinfo"].ToString());
                    pdfFormFields.SetField("Justcrimeseriousness", row["Justcrimeseriousness$"].ToString());
                    pdfFormFields.SetField("Justcrimehistory", row["Justcrimehistory$"].ToString());
                    pdfFormFields.SetField("Justcrimearrests", row["Justcrimearrests$"].ToString());
                    pdfFormFields.SetField("Justcrimeplanning", row["Justcrimeplanning$"].ToString());
                    pdfFormFields.SetField("Justcrimecommunsafety", row["Justcrimecommunsafety$"].ToString());
                    pdfFormFields.SetField("Justcrimelegalcompliance", row["Justcrimelegalcompliance$"].ToString());
                    pdfFormFields.SetField("Justcrimepeerinfluences", row["Justcrimepeerinfluences$"].ToString());
                    pdfFormFields.SetField("Justcrimenvironinfluences", row["Justcrimenvironinfluences$"].ToString());
                    pdfFormFields.SetField("Justcrimeust", row["Justcrimeust"].ToString());
                    pdfFormFields.SetField("Justcrimengri", row["Justcrimengri"].ToString());
                    pdfFormFields.SetField("Justcrimeustdate", row["Justcrimeustdate"].ToString());
                    pdfFormFields.SetField("Justcrimengridate", row["Justcrimengridate"].ToString());
                    pdfFormFields.SetField("Justicesupportinginformation", row["Justicesupportinginformation"].ToString());
                    pdfFormFields.SetField("Safetyfactorscurrentenvironment", row["Safetyfactorscurrentenvironment"].ToString());
                    pdfFormFields.SetField("SPDEdTesting", row["SPDEdTesting"].ToString());
                    pdfFormFields.SetField("SPDStudentStudyTeam", row["SPDStudentStudyTeam"].ToString());
                    pdfFormFields.SetField("SPD504Plan", row["SPD504Plan"].ToString());
                    pdfFormFields.SetField("SPDIEP", row["SPDIEP"].ToString());
                    pdfFormFields.SetField("SPDCredRecovery", row["SPDCredRecovery"].ToString());
                    pdfFormFields.SetField("SPDTutoring", row["SPDTutoring"].ToString());
                    pdfFormFields.SetField("SafetyStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SafetyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "SafetyStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SafetyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "SafetyStatus");
                    }
                }

                if (dataTableSubstanceUseHistory.Rows.Count > 0)
                {
                    DataRow row = dataTableSubstanceUseHistory.Rows[0];
                    pdfFormFields.SetField("Substanceusehistseverity", row["Substanceusehistseverity$"].ToString());
                    pdfFormFields.SetField("Substanceusehistduration", row["Substanceusehistduration$"].ToString());
                    pdfFormFields.SetField("Substanceusehiststageofrecovery", row["Substanceusehiststageofrecovery$"].ToString());
                    pdfFormFields.SetField("Substanceusehistenvironinfluences", row["Substanceusehistenvironinfluences$"].ToString());
                    pdfFormFields.SetField("Substanceusehistpeerinfluences", row["Substanceusehistpeerinfluences$"].ToString());
                    pdfFormFields.SetField("Substanceusehistparentinfluence", row["Substanceusehistparentinfluence$"].ToString());
                    pdfFormFields.SetField("Substanceusehistrecovsupcommun", row["Substanceusehistrecovsupcommun$"].ToString());
                    pdfFormFields.SetField("Substanceusehistsuppinfo", row["Substanceusehistsuppinfo"].ToString());
                    pdfFormFields.SetField("Subabusehisttreatment", row["Subabusehisttreatment$"].ToString());
                    pdfFormFields.SetField("SubstanceUseHistoryStatus", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SubstanceUseHistoryStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "SubstanceUseHistoryStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SubstanceUseHistoryStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "SubstanceUseHistoryStatus");
                    }
                }

                if (dataTablePlacementHistory.Rows.Count > 0)
                {
                    DataRow row = dataTablePlacementHistory.Rows[0];
                    pdfFormFields.SetField("OutOfHomePlacementHistory", row["OutOfHomePlacementHistory"].ToString());
                    pdfFormFields.SetField("PlacementHistory", row["PlacementHistory"].ToString());
                    pdfFormFields.SetField("PlacementHistoryStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "PlacementHistoryStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "PlacementHistoryStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "PlacementHistoryStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "PlacementHistoryStatus");
                    }
                }
                if (dataTablePsychiatricInformation.Rows.Count > 0)
                {
                    DataRow row = dataTablePsychiatricInformation.Rows[0];
                    pdfFormFields.SetField("Psychiatricproblems", row["Psychiatricproblems"].ToString());
                    pdfFormFields.SetField("Gmhhpriorpsychologicalassessment", row["Gmhhpriorpsychologicalassessment"].ToString());
                    pdfFormFields.SetField("Gmhhpriorpsyschologicalassessmentdate", row["Gmhhpriorpsyschologicalassessmentdate"].ToString());
                    pdfFormFields.SetField("Gmhhpriorpsychologicalassessmentiq", row["Gmhhpriorpsychologicalassessmentiq"].ToString());
                    pdfFormFields.SetField("Gmhhpriorpsychiatricevaluation", row["Gmhhpriorpsychiatricevaluation"].ToString());
                    pdfFormFields.SetField("Gmhhpriorpsychiatricevaluationdate", row["Gmhhpriorpsychiatricevaluationdate"].ToString());
                    pdfFormFields.SetField("Gmhhassessmentpsychologicaltesting", row["Gmhhassessmentpsychologicaltesting"].ToString());
                    pdfFormFields.SetField("Gmhhpsychiatricevaluation", row["Gmhhpsychiatricevaluation"].ToString());
                    pdfFormFields.SetField("Gmhhprioroutpatientmentalhealthservices", row["Gmhhprioroutpatientmentalhealthservices"].ToString());
                    pdfFormFields.SetField("Mentalstatappearancebehavior", row["Mentalstatappearancebehavior"].ToString());
                    pdfFormFields.SetField("Mentalstathreatening", row["Mentalstathreatening$"].ToString());
                    pdfFormFields.SetField("Mentalstatsuicidal", row["Mentalstatsuicidal$"].ToString());
                    pdfFormFields.SetField("Mentalstathomicidal", row["Mentalstathomicidal$"].ToString());
                    pdfFormFields.SetField("Mentalstatimpulsecontrol", row["Mentalstatimpulsecontrol"].ToString());
                    pdfFormFields.SetField("Mentalstathallucinatory", row["Mentalstathallucinatory$"].ToString());
                    pdfFormFields.SetField("Mentalstatdelusional", row["Mentalstatdelusional$"].ToString());
                    pdfFormFields.SetField("Mentalstatjudgement", row["Mentalstatjudgement"].ToString());
                    pdfFormFields.SetField("Mentalstatmemory", row["Mentalstatmemory"].ToString());
                    pdfFormFields.SetField("Mentalstatexpansive", row["Mentalstatexpansive"].ToString());
                    pdfFormFields.SetField("Mentalstatlabile", row["Mentalstatlabile"].ToString());
                    pdfFormFields.SetField("Mentalstatmoodangry", row["Mentalstatmoodangry"].ToString());
                    pdfFormFields.SetField("Mentalstatmoodwnl", row["Mentalstatmoodwnl"].ToString());
                    pdfFormFields.SetField("Mentalstatmooddepressed", row["Mentalstatmooddepressed"].ToString());
                    pdfFormFields.SetField("Mentalstatmoodanxious", row["Mentalstatmoodanxious"].ToString());
                    pdfFormFields.SetField("Mentalstatmoodmanic", row["Mentalstatmoodmanic"].ToString());
                    pdfFormFields.SetField("Mentalstataffectangry", row["Mentalstataffectangry"].ToString());
                    pdfFormFields.SetField("Mentalstataffectwnl", row["Mentalstataffectwnl"].ToString());
                    pdfFormFields.SetField("Mentalstatconstricted", row["Mentalstatconstricted"].ToString());
                    pdfFormFields.SetField("Mentalstatflat", row["Mentalstatflat"].ToString());
                    pdfFormFields.SetField("Mentalstatinappropriate", row["Mentalstatinappropriate"].ToString());
                    pdfFormFields.SetField("Mentalstatsad", row["Mentalstatsad"].ToString());
                    pdfFormFields.SetField("Mentalstatinsight", row["Mentalstatinsight"].ToString());
                    pdfFormFields.SetField("Mentalstatorientation", row["Mentalstatorientation"].ToString());
                    pdfFormFields.SetField("Mentalstatcognition", row["Mentalstatcognition"].ToString());
                    pdfFormFields.SetField("Mentalstatdepressed", row["Mentalstatdepressed"].ToString());
                    pdfFormFields.SetField("Mentalstatmanic", row["Mentalstatmanic"].ToString());
                    pdfFormFields.SetField("Mentalstatanxious", row["Mentalstatanxious"].ToString());
                    pdfFormFields.SetField("PsychiatricInformationStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "PsychiatricInformationStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "PsychiatricInformationStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "PsychiatricInformationStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "PsychiatricInformationStatus");
                    }
                }
                if (dataTableClientStrengths.Rows.Count > 0)
                {
                    DataRow row = dataTableClientStrengths.Rows[0];
                    pdfFormFields.SetField("ClientStrengthsFamilySupport", row["ClientStrengthsFamilySupport"].ToString());
                    pdfFormFields.SetField("ClientStrengthsInterSocialConnect", row["ClientStrengthsInterSocialConnect"].ToString());
                    pdfFormFields.SetField("ClientStrengthsNaturalSupp", row["ClientStrengthsNaturalSupp"].ToString());
                    pdfFormFields.SetField("ClientStrengthSpiritualReligious", row["ClientStrengthSpiritualReligious"].ToString());
                    pdfFormFields.SetField("ClientStrengthsEducatSetting", row["ClientStrengthsEducatSetting"].ToString());
                    pdfFormFields.SetField("ClientStrengthsRelatPermanence", row["ClientStrengthsRelatPermanence"].ToString());
                    pdfFormFields.SetField("ClientStrengthsResiliency", row["ClientStrengthsResiliency"].ToString());
                    pdfFormFields.SetField("ClientStrengthsOptimism", row["ClientStrengthsOptimism"].ToString());
                    pdfFormFields.SetField("ClientStrengthsTalentsInterests", row["ClientStrengthsTalentsInterests"].ToString());
                    pdfFormFields.SetField("ClientStrengthsCultIdentity", row["ClientStrengthsCultIdentity"].ToString());
                    pdfFormFields.SetField("ClientStrengthsCommunConnection", row["ClientStrengthsCommunConnection"].ToString());
                    pdfFormFields.SetField("ClientStrengthsInvolveCare", row["ClientStrengthsInvolveCare"].ToString());
                    pdfFormFields.SetField("ClientStrengthsVocational", row["ClientStrengthsVocational"].ToString());
                    pdfFormFields.SetField("ClientStrengthsJobHistVolunteer", row["ClientStrengthsJobHistVolunteer"].ToString());
                    pdfFormFields.SetField("ClientStrengthSelfCare", row["ClientStrengthSelfCare"].ToString());
                    pdfFormFields.SetField("ClientStrengthSupportingInformation", row["ClientStrengthSupportingInformation"].ToString());
                    pdfFormFields.SetField("ClientStrengthStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ClientStrengthStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "ClientStrengthStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ClientStrengthStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "ClientStrengthStatus");
                    }
                }
                if (dataTableFamilyInformation.Rows.Count > 0)
                {
                    DataRow row = dataTableFamilyInformation.Rows[0];
                    pdfFormFields.SetField("RelevantFamilyHistory", row["RelevantFamilyHistory"].ToString());
                    pdfFormFields.SetField("CulturalFactorsLanguage", row["CulturalFactorsLanguage$"].ToString());
                    pdfFormFields.SetField("CulturalFactorsStress", row["CulturalFactorsStress$"].ToString());
                    pdfFormFields.SetField("CulturalFactorsTraditionsRituals", row["CulturalFactorsTraditionsRituals$"].ToString());
                    pdfFormFields.SetField("CulturalFactorSupportingInformation", row["CulturalFactorSupportingInformation"].ToString());
                    pdfFormFields.SetField("FamilyInformationStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "FamilyInformationStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "FamilyInformationStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "FamilyInformationStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "FamilyInformationStatus");
                    }
                }
                if (dataTableNeedsResourceAssessment.Rows.Count > 0)
                {
                    DataRow row = dataTableNeedsResourceAssessment.Rows[0];
                    pdfFormFields.SetField("NeedsAssessmentNone", row["NeedsAssessmentNone"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentAccessToFood", row["NeedsAssessmentAccessToFood"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentClothing", row["NeedsAssessmentClothing"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentEducationalTesting", row["NeedsAssessmentEducationalTesting"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentEmployment", row["NeedsAssessmentEmployment"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentFinancialAssistance", row["NeedsAssessmentFinancialAssistance"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentImmigrationAssistance", row["NeedsAssessmentImmigrationAssistance"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentLegalAssistance", row["NeedsAssessmentLegalAssistance"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentMentalHealthService", row["NeedsAssessmentMentalHealthService"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentMentoring", row["NeedsAssessmentMentoring"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentOther", row["NeedsAssessmentOther"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentOtherDescription", row["NeedsAssessmentOtherDescription"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentPhysicalHealth", row["NeedsAssessmentPhysicalHealth"].ToString());
                    pdfFormFields.SetField("NeedsAssessmentShelter", row["NeedsAssessmentShelter"].ToString());
                    pdfFormFields.SetField("NeedsStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "NeedsStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "NeedsStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "NeedsStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "NeedsStatus");
                    }
                }
                if (dataTableDSMDiagnosis.Rows.Count > 0)
                {
                    DataRow row = dataTableDSMDiagnosis.Rows[0];
                    pdfFormFields.SetField("DiagnosisStatus", row["Status"].ToString());
                }
                if (dataTableMentalHealthSummary.Rows.Count > 0)
                {
                    DataRow row = dataTableMentalHealthSummary.Rows[0];
                    pdfFormFields.SetField("MentalHealthSummary", row["MentalHealthSummary"].ToString());
                    pdfFormFields.SetField("MentalHealthAssesssmentStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MentalHealthAssesssmentStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "MentalHealthAssesssmentStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MentalHealthAssesssmentStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "MentalHealthAssesssmentStatus");
                    }
                }
                if (dataTableAddClientFunctioningEvaluations.Rows.Count > 0)
                {
                    DataRow row = dataTableAddClientFunctioningEvaluations.Rows[0];
                    pdfFormFields.SetField("NoAdditionalEvaluations", row["NoAdditionalEvaluations"].ToString());
                    pdfFormFields.SetField("AdditionalEvaluations", row["AdditionalEvaluations"].ToString());
                    pdfFormFields.SetField("AdditionalClientStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "AdditionalClientStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "AdditionalClientStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "AdditionalClientStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "AdditionalClientStatus");
                    }
                }
                // individual treatment 
                if (dataTableIndividualTreatmentPlan.Rows.Count > 0)
                {
                    DataRow row = dataTableIndividualTreatmentPlan.Rows[0];
                    pdfFormFields.SetField("TreatmentVisionStatement", row["TreatmentVisionStatement"].ToString());
                    pdfFormFields.SetField("ClientServicePreferences", row["ClientServicePreferences"].ToString());
                    pdfFormFields.SetField("TreatmentPlanDate", row["TreatmentPlanDate"].ToString());
                    pdfFormFields.SetField("IndividualTreatmentStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IndividualTreatmentStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "IndividualTreatmentStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IndividualTreatmentStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "IndividualTreatmentStatus");
                    }
                }
                //Signatures
                if (dataTableSignatures.Rows.Count > 0)
                {
                    DataRow row = dataTableSignatures.Rows[0];
                    pdfFormFields.SetField("SignClientName", row["ClientName"].ToString());
                    pdfFormFields.SetField("ClientSignatureDate", row["ClientSignatureDate"].ToString());
                    //   ClientSignature = row["ClientSignature"];
                    ParentLegalGuardianSignature = row["ParentLegalGuardianSignature"].ToString();
                    pdfFormFields.SetField("ParentLegalGuardianName", row["ParentLegalGuardianName"].ToString());
                    pdfFormFields.SetField("ParentLegalGuardianSignatureDate", row["ParentLegalGuardianSignatureDate"].ToString());
                    pdfFormFields.SetField("LPHAApprovalName", row["LPHAApprovalName"].ToString());
                    pdfFormFields.SetField("LPHASignature", row["LPHASignature"].ToString());
                    pdfFormFields.SetField("LPHAStaffName", row["LPHAStaffName"].ToString());
                    pdfFormFields.SetField("LPHAStaffTitle", row["LPHAStaffTitle"].ToString());
                    pdfFormFields.SetField("LPHASignedDateTime", row["LPHASignedDateTime"].ToString());
                    pdfFormFields.SetField("QMHPF2F", row["QMHPF2F"].ToString());
                    pdfFormFields.SetField("QMHPSignature", row["QMHPSignature"].ToString());
                    pdfFormFields.SetField("QMHPStaffName", row["QMHPStaffName"].ToString());
                    pdfFormFields.SetField("QMHPStaffTitle", row["QMHPStaffTitle"].ToString());
                    pdfFormFields.SetField("QMHPSignedDateTime", row["QMHPSignedDateTime"].ToString());
                    pdfFormFields.SetField("MHPName", row["MHPName"].ToString());
                    pdfFormFields.SetField("MHPSigature", row["MHPSigature"].ToString());
                    pdfFormFields.SetField("MHPStaffName", row["MHPStaffName"].ToString());
                    pdfFormFields.SetField("MHPStaffTitle", row["MHPStaffTitle"].ToString());
                    pdfFormFields.SetField("MHPSignedDateTime", row["MHPSignedDateTime"].ToString());
                    pdfFormFields.SetField("SignatureStatus", row["Status"].ToString());
                }
                if (dataTableGeneralInformationHRA.Rows.Count > 0)
                {
                    DataRow row = dataTableGeneralInformationHRA.Rows[0];
                    pdfFormFields.SetField("StaffName", row["StaffName"].ToString());
                    pdfFormFields.SetField("HRAClientFirstName", row["ClientFirstName"].ToString());
                    pdfFormFields.SetField("HRAClientLastName", row["ClientLastName"].ToString());
                    pdfFormFields.SetField("RecipientId", row["RecipientId"].ToString());
                    pdfFormFields.SetField("ClientDateOfBirth", row["ClientDateOfBirth"].ToString());
                    pdfFormFields.SetField("ClientGender", row["ClientGender"].ToString());
                    pdfFormFields.SetField("ClientHeightFeet", row["ClientHeightFeet"].ToString());
                    pdfFormFields.SetField("ClientHeightInches", row["ClientHeightInches"].ToString());
                    pdfFormFields.SetField("HRAClientWeight", row["ClientWeight"].ToString());
                    pdfFormFields.SetField("PrimaryPhysicianName", row["PrimaryPhysicianName"].ToString());
                    pdfFormFields.SetField("LastPhysicalExamDate", row["LastPhysicalExamDate"].ToString());
                    pdfFormFields.SetField("PhysicalExamDue", row["PhysicalExamDue"].ToString());
                    pdfFormFields.SetField("LastFluShotDate", row["LastFluShotDate"].ToString());
                    pdfFormFields.SetField("GeneralHRAStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "GeneralHRAStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "GeneralHRAStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "GeneralHRAStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "GeneralHRAStatus");
                    }
                }
                if (dataTableHealthStatus.Rows.Count > 0)
                {
                    DataRow row = dataTableHealthStatus.Rows[0];
                    pdfFormFields.SetField("ClientSelfReportedPhysicalHealth", row["ClientSelfReportedPhysicalHealth"].ToString());
                    pdfFormFields.SetField("DailySnackIntake", row["DailySnackIntake"].ToString());
                    pdfFormFields.SetField("DailyFruitVegetableIntake", row["DailyFruitVegetableIntake"].ToString());
                    pdfFormFields.SetField("RegularPhysicalActivity", row["RegularPhysicalActivity$"].ToString());
                    pdfFormFields.SetField("PhysicalActivityFrequency", row["PhysicalActivityFrequency"].ToString());
                    pdfFormFields.SetField("TobaccoUse", row["TobaccoUse$"].ToString());
                    pdfFormFields.SetField("AlcoholUse", row["AlcoholUse$"].ToString());
                    pdfFormFields.SetField("AlcoholConsumptionFrequency", row["AlcoholConsumptionFrequency"].ToString());
                    pdfFormFields.SetField("FaintingHistory", row["FaintingHistory$"].ToString());
                    pdfFormFields.SetField("FaintingDescription", row["FaintingDescription"].ToString());
                    pdfFormFields.SetField("KnownAllergy", row["KnownAllergy$"].ToString());
                    pdfFormFields.SetField("AllergyDescription", row["AllergyDescription"].ToString());
                    pdfFormFields.SetField("FallingHistory", row["FallingHistory$"].ToString());
                    pdfFormFields.SetField("FallingDescription", row["FallingDescription"].ToString());
                    pdfFormFields.SetField("RequestQuitSmoking", row["RequestQuitSmoking$"].ToString());
                    pdfFormFields.SetField("HealthConcerns", row["HealthConcerns$"].ToString());
                    pdfFormFields.SetField("HealthConcernsDescription", row["HealthConcernsDescription"].ToString());
                    pdfFormFields.SetField("GeneralIllness", row["GeneralIllness$"].ToString());
                    pdfFormFields.SetField("GeneralIllnessDescription", row["GeneralIllnessDescription"].ToString());
                    pdfFormFields.SetField("BreathingIssueMedicated", row["BreathingIssueMedicated"].ToString());
                    pdfFormFields.SetField("BreathingIssues", row["BreathingIssues$"].ToString());
                    pdfFormFields.SetField("BreathingIssuesCause", row["BreathingIssuesCause"].ToString());
                    pdfFormFields.SetField("BreathingIssuesCauseDescription", row["BreathingIssuesCauseDescription"].ToString());
                    pdfFormFields.SetField("HeadInjury", row["HeadInjury$"].ToString());
                    pdfFormFields.SetField("HeadInjuryDate", row["HeadInjuryDate"].ToString());
                    pdfFormFields.SetField("MemoryLapses", row["MemoryLapses$"].ToString());
                    pdfFormFields.SetField("CurrentDateAware", row["CurrentDateAware$"].ToString());
                    pdfFormFields.SetField("AboveAverageUrination", row["AboveAverageUrination$"].ToString());
                    pdfFormFields.SetField("AboveAverageThirst", row["AboveAverageThirst$"].ToString());
                    pdfFormFields.SetField("SpecialBloodSugarDiet", row["SpecialBloodSugarDiet$"].ToString());
                    pdfFormFields.SetField("SpecialDietDescription", row["SpecialDietDescription"].ToString());
                    pdfFormFields.SetField("BloodSugarMedicated", row["BloodSugarMedicated$"].ToString());
                    pdfFormFields.SetField("ChronicPain", row["ChronicPain$"].ToString());
                    pdfFormFields.SetField("PainMedicationHistory", row["PainMedicationHistory$"].ToString());
                    pdfFormFields.SetField("PainMedicationCategory", row["PainMedicationCategory"].ToString());
                    pdfFormFields.SetField("PainMedicationDescription", row["PainMedicationDescription"].ToString());
                    pdfFormFields.SetField("PainIntensityLocationDescription", row["PainIntensityLocationDescription"].ToString());
                    pdfFormFields.SetField("SexuallyActive", row["SexuallyActive$"].ToString());
                    pdfFormFields.SetField("STDProtection", row["STDProtection"].ToString());
                    pdfFormFields.SetField("LastSTDTestDate", row["LastSTDTestDate"].ToString());
                    pdfFormFields.SetField("STDDiagnosed", row["STDDiagnosed$"].ToString());
                    pdfFormFields.SetField("STDDiagnosisDescription", row["STDDiagnosisDescription"].ToString());
                    pdfFormFields.SetField("WomenHealthProviderVisit", row["WomenHealthProviderVisit"].ToString());
                    pdfFormFields.SetField("WomenHealthProviderVisitDate", row["WomenHealthProviderVisitDate"].ToString());
                    pdfFormFields.SetField("MenstrualCycleorMenopauseIssue", row["MenstrualCycleorMenopauseIssue$"].ToString());
                    pdfFormFields.SetField("MenstrualCycleorMenopauseDescription", row["MenstrualCycleorMenopauseDescription"].ToString());
                    pdfFormFields.SetField("PregnancyHistory", row["PregnancyHistory"].ToString());
                    pdfFormFields.SetField("PregnancyOutcomeDescription", row["PregnancyOutcomeDescription"].ToString());
                    pdfFormFields.SetField("HealthStatusStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "HealthStatusStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "HealthStatusStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "HealthStatusStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "HealthStatusStatus");
                    }
                }
                if (dataTableMedications.Rows.Count > 0)
                {
                    DataRow row = dataTableMedications.Rows[0];
                    pdfFormFields.SetField("PsychotropicMedicationUse", row["PsychotropicMedicationUse"].ToString());
                    pdfFormFields.SetField("MedicationCompliance", row["MedicationCompliance"].ToString());
                    pdfFormFields.SetField("PsychotropicLabWork", row["PsychotropicLabWork"].ToString());
                    pdfFormFields.SetField("MedicationStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MedicationStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "MedicationStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MedicationStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "MedicationStatus");
                    }
                }
                if (dataTableDevelopmentHistory.Rows.Count > 0)
                {
                    DataRow row = dataTableDevelopmentHistory.Rows[0];
                    pdfFormFields.SetField("ClientMotherPrenatalCare", row["ClientMotherPrenatalCare"].ToString());
                    pdfFormFields.SetField("ClientMotherPregnancyComplications", row["ClientMotherPregnancyComplications"].ToString());
                    pdfFormFields.SetField("ClientMotherPregnancyComplicationsDescription", row["ClientMotherPregnancyComplicationsDescription"].ToString());
                    pdfFormFields.SetField("ClientBirthStatus", row["ClientBirthStatus"].ToString());
                    pdfFormFields.SetField("ClientInUteroSubstanceExposure", row["ClientInUteroSubstanceExposure"].ToString());
                    pdfFormFields.SetField("ClientInUteroSubstanceExposureDescription", row["ClientInUteroSubstanceExposureDescription"].ToString());
                    pdfFormFields.SetField("ClientMotherLaborIssues", row["ClientMotherLaborIssues"].ToString());
                    pdfFormFields.SetField("ClientMotherLaborIssuesDescription", row["ClientMotherLaborIssuesDescription"].ToString());
                    pdfFormFields.SetField("ClientWeight", row["ClientWeight"].ToString());
                    pdfFormFields.SetField("DevelopmentMilestoneCrawlAge", row["DevelopmentMilestoneCrawlAge"].ToString());
                    pdfFormFields.SetField("DevelopmentMilestoneWalkAge", row["DevelopmentMilestoneWalkAge"].ToString());
                    pdfFormFields.SetField("DevelopmentMilestoneTalkAge", row["DevelopmentMilestoneTalkAge"].ToString());
                    pdfFormFields.SetField("DevelopmentMilestoneToiletTrainedAge", row["DevelopmentMilestoneToiletTrainedAge"].ToString());
                    pdfFormFields.SetField("FamilyHistoryBehavioralProblems", row["FamilyHistoryBehavioralProblems"].ToString());
                    pdfFormFields.SetField("SupportingClientHistoryDescription", row["SupportingClientHistoryDescription"].ToString());
                    pdfFormFields.SetField("DevelopmentHistoryStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DevelopmentHistoryStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "DevelopmentHistoryStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DevelopmentHistoryStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "DevelopmentHistoryStatus");
                    }
                }
                if (dataTableMedicalHistory.Rows.Count > 0)
                {
                    DataRow row = dataTableMedicalHistory.Rows[0];
                    pdfFormFields.SetField("EmergencyRoomFrequency", row["EmergencyRoomFrequency"].ToString());
                    pdfFormFields.SetField("EmergencyRoomVisitDescription", row["EmergencyRoomVisitDescription"].ToString());
                    pdfFormFields.SetField("PsychiatricallyHospitalized", row["PsychiatricallyHospitalized"].ToString());
                    pdfFormFields.SetField("SupportingHospitalHistoryDescription", row["SupportingHospitalHistoryDescription"].ToString());
                    pdfFormFields.SetField("MedicalHistoryStatus", row["Status"].ToString());
                    pdfFormFields.SetField("IsAdditionalPagesNeeded", row["IsAdditionalPagesNeeded"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IsAdditionalPagesNeeded");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "IsAdditionalPagesNeeded");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IsAdditionalPagesNeeded");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "IsAdditionalPagesNeeded");
                    }
                }
                if (dataTableCaregiverAddendum.Rows.Count > 0)
                {
                    DataRow row = dataTableCaregiverAddendum.Rows[0];
                    pdfFormFields.SetField("AddendumClientFirstName", row["ClientFirstName"].ToString());
                    pdfFormFields.SetField("AddendumClientLastName", row["ClientLastName"].ToString());
                    pdfFormFields.SetField("ClientRIN", row["ClientRIN"].ToString());
                    pdfFormFields.SetField("StaffCompletingForm", row["StaffCompletingForm"].ToString());
                    pdfFormFields.SetField("DateCompleted", row["DateCompleted"].ToString());
                    pdfFormFields.SetField("CaregiverFullName", row["CaregiverFullName"].ToString());
                    pdfFormFields.SetField("CaregiverRelationshipToClient", row["CaregiverRelationshipToClient"].ToString());
                    pdfFormFields.SetField("CaregiverAddtlPrimary", row["CaregiverAddtlPrimary"].ToString());
                    pdfFormFields.SetField("CaregiverSupervision", row["CaregiverSupervision$"].ToString());
                    pdfFormFields.SetField("CaregiverInvolvementWithCare", row["CaregiverInvolvementWithCare$"].ToString());
                    pdfFormFields.SetField("CaregiverKnowledge", row["CaregiverKnowledge$"].ToString());
                    pdfFormFields.SetField("CaregiverSocialResources", row["CaregiverSocialResources$"].ToString());
                    pdfFormFields.SetField("CaregiverFinancialResources", row["CaregiverFinancialResources$"].ToString());
                    pdfFormFields.SetField("CaregiverResidentialStability", row["CaregiverResidentialStability$"].ToString());
                    pdfFormFields.SetField("CaregiverMedicalPhysical", row["CaregiverMedicalPhysical$"].ToString());
                    pdfFormFields.SetField("CaregiverMentalHealth", row["CaregiverMentalHealth$"].ToString());
                    pdfFormFields.SetField("CaregiverSubstanceUse", row["CaregiverSubstanceUse$"].ToString());
                    pdfFormFields.SetField("CaregiverDevelopmental", row["CaregiverDevelopmental$"].ToString());
                    pdfFormFields.SetField("CaregiverOrganization", row["CaregiverOrganization$"].ToString());
                    pdfFormFields.SetField("CaregiverSafety", row["CaregiverSafety$"].ToString());
                    pdfFormFields.SetField("CaregiverFamilyStress", row["CaregiverFamilyStress$"].ToString());
                    pdfFormFields.SetField("CaregiverMaritalPartnerViolence", row["CaregiverMaritalPartnerViolence$"].ToString());
                    pdfFormFields.SetField("CaregiverMilitaryTrans", row["CaregiverMilitaryTrans$"].ToString());
                    pdfFormFields.SetField("CaregiverSelfcareLivingSkills", row["CaregiverSelfcareLivingSkills$"].ToString());
                    pdfFormFields.SetField("CaregiverEmployEducFunc", row["CaregiverEmployEducFunc$"].ToString());
                    pdfFormFields.SetField("CaregiverLegalInvolvement", row["CaregiverLegalInvolvement$"].ToString());
                    pdfFormFields.SetField("CaregiverFamRelationToSystem", row["CaregiverFamRelationToSystem$"].ToString());
                    pdfFormFields.SetField("CaregiverAccessToChildCare", row["CaregiverAccessToChildCare$"].ToString());
                    pdfFormFields.SetField("CaregiverEmathyChildren", row["CaregiverEmathyChildren$"].ToString());
                    pdfFormFields.SetField("CaregiverSupportingInformation", row["CaregiverSupportingInformation"].ToString());
                    pdfFormFields.SetField("CaregiverStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "CaregiverStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "CaregiverStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "CaregiverStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "CaregiverStatus");
                    }
                }
                if (dataTableGeneralInformationDCFS.Rows.Count > 0)
                {
                    DataRow row = dataTableGeneralInformationDCFS.Rows[0];
                    pdfFormFields.SetField("DCFSYouthName", row["DCFSYouthName"].ToString());
                    pdfFormFields.SetField("DCFSRIN", row["DCFSRIN"].ToString());
                    pdfFormFields.SetField("DCFSStaffCompletingForm", row["DCFSStaffCompletingForm"].ToString());
                    pdfFormFields.SetField("DCFSCompletedDate", row["DCFSCompletedDate"].ToString());
                    pdfFormFields.SetField("DCFSInvlvYthInCare", row["DCFSInvlvYthInCare"].ToString());
                    pdfFormFields.SetField("DCFSInvlvIntFam", row["DCFSInvlvIntFam"].ToString());
                    pdfFormFields.SetField("DCFSInvlvIPS", row["DCFSInvlvIPS"].ToString());
                    pdfFormFields.SetField("DCFSInvolvement", row["DCFSInvolvement"].ToString());
                    pdfFormFields.SetField("DCFSStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DCFSStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "DCFSStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DCFSStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "DCFSStatus");
                    }
                }
                if (dataTableSexuallyAggrBehavior.Rows.Count > 0)
                {
                    DataRow row = dataTableSexuallyAggrBehavior.Rows[0];
                    pdfFormFields.SetField("SXAGBhTemporalConsist", row["SXAGBhTemporalConsist$"].ToString());
                    pdfFormFields.SetField("SXAGBhsHistsexAbuseBeh", row["SXAGBhsHistsexAbuseBeh$"].ToString());
                    pdfFormFields.SetField("SXAGbhSeveritySxAbuse", row["SXAGbhSeveritySxAbuse$"].ToString());
                    pdfFormFields.SetField("SXAGbhPriorTreatmnt", row["SXAGbhPriorTreatmnt$"].ToString());
                    pdfFormFields.SetField("SXAGbhSuppInfo", row["SXAGbhSuppInfo"].ToString());
                    pdfFormFields.SetField("SexuallyStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SexuallyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "SexuallyStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SexuallyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "SexuallyStatus");
                    }
                }
                if (dataTableParentGuardSafety.Rows.Count > 0)
                {
                    DataRow row = dataTableParentGuardSafety.Rows[0];
                    pdfFormFields.SetField("Pgsafediscipln", row["Pgsafediscipln$"].ToString());
                    pdfFormFields.SetField("Pgsafehomecond", row["Pgsafehomecond$"].ToString());
                    pdfFormFields.SetField("Pgsafefrsutrtoler", row["Pgsafefrsutrtoler$"].ToString());
                    pdfFormFields.SetField("Pgsafemaltreatment", row["Pgsafemaltreatment$"].ToString());
                    pdfFormFields.SetField("Pgsafesuppinfo", row["Pgsafesuppinfo"].ToString());
                    pdfFormFields.SetField("ParentSafetyStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ParentSafetyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "ParentSafetyStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ParentSafetyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "ParentSafetyStatus");
                    }
                }
                if (dataTableParentGuardWellbeing.Rows.Count > 0)
                {
                    DataRow row = dataTableParentGuardWellbeing.Rows[0];
                    pdfFormFields.SetField("Pgwelltraumreact", row["Pgwelltraumreact$"].ToString());
                    pdfFormFields.SetField("Pgwellimpactownbeh", row["Pgwellimpactownbeh$"].ToString());
                    pdfFormFields.SetField("Pgwelleffctparntapprch", row["Pgwelleffctparntapprch$"].ToString());
                    pdfFormFields.SetField("Pgwellindeplivskills", row["Pgwellindeplivskills$"].ToString());
                    pdfFormFields.SetField("Pgwellcntctcasewrker", row["Pgwellcntctcasewrker$"].ToString());
                    pdfFormFields.SetField("Pgwellresponsmaltrtmnt", row["Pgwellresponsmaltrtmnt$"].ToString());
                    pdfFormFields.SetField("Pgwellrelatwthabuser", row["Pgwellrelatwthabuser$"].ToString());
                    pdfFormFields.SetField("Pgwellsuppinfo", row["Pgwellsuppinfo"].ToString());
                    pdfFormFields.SetField("ParentWellbeingSafety", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ParentWellbeingSafety");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "ParentWellbeingSafety");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ParentWellbeingSafety");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "ParentWellbeingSafety");
                    }
                }
                if (dataTableParentGuardPermananence.Rows.Count > 0)
                {
                    DataRow row = dataTableParentGuardPermananence.Rows[0];
                    pdfFormFields.SetField("Pgpermfamconnect", row["Pgpermfamconnect$"].ToString());
                    pdfFormFields.SetField("Pgpermpersonaltrtmnt", row["Pgpermpersonaltrtmnt$"].ToString());
                    pdfFormFields.SetField("Pgpermparticvisit", row["Pgpermparticvisit$"].ToString());
                    pdfFormFields.SetField("Pgpermcommitreunifi", row["Pgpermcommitreunifi$"].ToString());
                    pdfFormFields.SetField("Pgpermsuppinfo", row["Pgpermsuppinfo"].ToString());
                    pdfFormFields.SetField("ParentPermanenceStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ParentPermanenceStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "ParentPermanenceStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "ParentPermanenceStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "ParentPermanenceStatus");
                    }
                }
                if (dataTableSubstituteCommitPermananence.Rows.Count > 0)
                {
                    DataRow row = dataTableSubstituteCommitPermananence.Rows[0];
                    pdfFormFields.SetField("Subcomitpermna", row["Subcomitpermna"].ToString());
                    pdfFormFields.SetField("Subcomitpermcollabothprnt", row["Subcomitpermcollabothprnt$"].ToString());
                    pdfFormFields.SetField("Subcomitpermsupptpermplan", row["Subcomitpermsupptpermplan$"].ToString());
                    pdfFormFields.SetField("Subcomitperminclusythfstfam", row["Subcomitperminclusythfstfam$"].ToString());
                    pdfFormFields.SetField("Subcomitpermsuppinfo", row["Subcomitpermsuppinfo"].ToString());
                    pdfFormFields.SetField("SubstituteStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SubstituteStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "SubstituteStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SubstituteStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "SubstituteStatus");
                    }
                }
                if (dataTableIntactFamilyService.Rows.Count > 0)
                {
                    DataRow row = dataTableIntactFamilyService.Rows[0];
                    pdfFormFields.SetField("Intfamsvcna", row["Intfamsvcna"].ToString());
                    pdfFormFields.SetField("Intfamsvccaregvrcollab", row["Intfamsvccaregvrcollab$"].ToString());
                    pdfFormFields.SetField("Intfamsvcfamconflict", row["Intfamsvcfamconflict$"].ToString());
                    pdfFormFields.SetField("Intfamsvcfamcommunic", row["Intfamsvcfamcommunic$"].ToString());
                    pdfFormFields.SetField("Intfamsvcfamroleapprop", row["Intfamsvcfamroleapprop$"].ToString());
                    pdfFormFields.SetField("Intfamsvchomemaint", row["Intfamsvchomemaint$"].ToString());
                    pdfFormFields.SetField("Intfamsvcsuppinfo", row["Intfamsvcsuppinfo"].ToString());
                    pdfFormFields.SetField("IntactFamilyStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IntactFamilyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "IntactFamilyStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IntactFamilyStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "IntactFamilyStatus");
                    }
                }
                if (dataTableIntensivePlacementStabilization.Rows.Count > 0)
                {
                    DataRow row = dataTableIntensivePlacementStabilization.Rows[0];
                    pdfFormFields.SetField("Ipsna", row["Ipsna"].ToString());
                    pdfFormFields.SetField("Ipsouthyrsincare", row["Ipsouthyrsincare$"].ToString());
                    pdfFormFields.SetField("Ipsyouthplacemnthist", row["Ipsyouthplacemnthist$"].ToString());
                    pdfFormFields.SetField("Ipssubcargvrknwythdevneeds", row["Ipssubcargvrknwythdevneeds$"].ToString());
                    pdfFormFields.SetField("Ipssubcargvrdiscipline", row["Ipssubcargvrdiscipline$"].ToString());
                    pdfFormFields.SetField("Ipssubcargvrmngmntemot", row["Ipssubcargvrmngmntemot$"].ToString());
                    pdfFormFields.SetField("Ipssuppinfo", row["Ipssuppinfo"].ToString());
                    pdfFormFields.SetField("IntensivePlacementStatus", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IntensivePlacementStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "IntensivePlacementStatus");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "IntensivePlacementStatus");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "IntensivePlacementStatus");
                    }
                }

                pdfStamper.FormFlattening = false;
                pdfStamper.Dispose();
                // close the pdf
                pdfStamper.Close();
                pdfReader.Dispose();
                pdfReader.Close();
                ///////////////////////////////////
                ///

                DataTable dataTableMembersFamilyConstellation = null;
                DataTable dataTableEstablishedSupports = null;
                DataTable dataTableSubstanceAbuseTreatment = null;
                DataTable dataTableOutpatientMentalHealthServices = null;
                DataTable dataTableDsmDiagnosis = null;



                PdfPTable tableMembersFamilyConstellation = new PdfPTable(4);
                PdfPTable tableEstablishedSupports = new PdfPTable(5);
                PdfPTable tableSubstanceAbuseTreatment = new PdfPTable(4);
                PdfPTable tableOutpatientMentalHealthServices = new PdfPTable(4);
                PdfPTable tableDsmDiagnosis = new PdfPTable(4);


                tableMembersFamilyConstellation.WidthPercentage = 100f;
                tableEstablishedSupports.WidthPercentage = 100f;
                tableSubstanceAbuseTreatment.WidthPercentage = 100f;
                tableOutpatientMentalHealthServices.WidthPercentage = 100f;
                tableDsmDiagnosis.WidthPercentage = 100f;


                //     dataTableMedical = dataSetFillPDF.Tables[10];

                iTextSharp.text.Font fntTableFontHdr = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fntTableFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                // iTextSharp.text.Font pageTextFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                //create a document object
                // var doc = new Document(PageSize.LETTER);
                PdfReader reader = new PdfReader(newFile);
                PdfStamper pdfStamper1 = new PdfStamper(reader, new FileStream(NewCANSDocument, FileMode.Create));
                //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(NewCANSDocument, FileMode.Create));
                //  doc.Open();
                // int pageNum = reader.NumberOfPages;


                tableMembersFamilyConstellation.AddCell(new PdfPCell(new Phrase("Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMembersFamilyConstellation.AddCell(new PdfPCell(new Phrase("Age", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMembersFamilyConstellation.AddCell(new PdfPCell(new Phrase("Result to Client", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMembersFamilyConstellation.AddCell(new PdfPCell(new Phrase("Living in Home", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow roweMembersFamilyConstellation = null;
                dataTableMembersFamilyConstellation = dataSetFillPDF.Tables[27];
                if (dataSetFillPDF.Tables[27].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableMembersFamilyConstellation.Rows.Count; i++)
                    {
                        roweMembersFamilyConstellation = dataTableMembersFamilyConstellation.Rows[i];
                        tableMembersFamilyConstellation.AddCell(new Phrase(roweMembersFamilyConstellation["FamilyMemberName"].ToString(), fntTableFont));
                        tableMembersFamilyConstellation.AddCell(new Phrase(roweMembersFamilyConstellation["FamilyMemberAge"].ToString(), fntTableFont));
                        tableMembersFamilyConstellation.AddCell(new Phrase(roweMembersFamilyConstellation["FamilyMemberRelation"].ToString(), fntTableFont));
                        tableMembersFamilyConstellation.AddCell(new Phrase(roweMembersFamilyConstellation["FamilyMemberInHome"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableMembersFamilyConstellation.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableEstablishedSupports.AddCell(new PdfPCell(new Phrase("Estabilished Supports", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableEstablishedSupports.AddCell(new PdfPCell(new Phrase("Agency", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableEstablishedSupports.AddCell(new PdfPCell(new Phrase("Contact Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableEstablishedSupports.AddCell(new PdfPCell(new Phrase("Phone", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableEstablishedSupports.AddCell(new PdfPCell(new Phrase("Email", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowEstablishedSupports = null;
                dataTableEstablishedSupports = dataSetFillPDF.Tables[28];
                if (dataSetFillPDF.Tables[28].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableEstablishedSupports.Rows.Count; i++)
                    {
                        rowEstablishedSupports = dataTableEstablishedSupports.Rows[i];
                        tableEstablishedSupports.AddCell(new Phrase(rowEstablishedSupports["EstabilishedSupports"].ToString(), fntTableFont));
                        tableEstablishedSupports.AddCell(new Phrase(rowEstablishedSupports["EsAgency"].ToString(), fntTableFont));
                        tableEstablishedSupports.AddCell(new Phrase(rowEstablishedSupports["EsContact"].ToString(), fntTableFont));
                        tableEstablishedSupports.AddCell(new Phrase(rowEstablishedSupports["EsPhone"].ToString(), fntTableFont));
                        tableEstablishedSupports.AddCell(new Phrase(rowEstablishedSupports["EsEmail"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableEstablishedSupports.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 5, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                tableSubstanceAbuseTreatment.AddCell(new PdfPCell(new Phrase("When", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableSubstanceAbuseTreatment.AddCell(new PdfPCell(new Phrase("Where", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableSubstanceAbuseTreatment.AddCell(new PdfPCell(new Phrase("With Whom", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableSubstanceAbuseTreatment.AddCell(new PdfPCell(new Phrase("Reason", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow roweSubstanceAbuseTreatment = null;
                dataTableSubstanceAbuseTreatment = dataSetFillPDF.Tables[29];
                if (dataSetFillPDF.Tables[29].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableSubstanceAbuseTreatment.Rows.Count; i++)
                    {
                        roweSubstanceAbuseTreatment = dataTableSubstanceAbuseTreatment.Rows[i];
                        tableSubstanceAbuseTreatment.AddCell(new Phrase(roweSubstanceAbuseTreatment["Subabusehistwhen"].ToString(), fntTableFont));
                        tableSubstanceAbuseTreatment.AddCell(new Phrase(roweSubstanceAbuseTreatment["Subabusehistwhere"].ToString(), fntTableFont));
                        tableSubstanceAbuseTreatment.AddCell(new Phrase(roweSubstanceAbuseTreatment["Subabusehistwithwhom"].ToString(), fntTableFont));
                        tableSubstanceAbuseTreatment.AddCell(new Phrase(roweSubstanceAbuseTreatment["Subabusehistreason"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableSubstanceAbuseTreatment.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableOutpatientMentalHealthServices.AddCell(new PdfPCell(new Phrase("When", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableOutpatientMentalHealthServices.AddCell(new PdfPCell(new Phrase("Where", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableOutpatientMentalHealthServices.AddCell(new PdfPCell(new Phrase("With Whom", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableOutpatientMentalHealthServices.AddCell(new PdfPCell(new Phrase("Reason", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowOutpatientMentalHealthServices = null;
                dataTableOutpatientMentalHealthServices = dataSetFillPDF.Tables[30];
                if (dataSetFillPDF.Tables[30].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableOutpatientMentalHealthServices.Rows.Count; i++)
                    {
                        rowOutpatientMentalHealthServices = dataTableOutpatientMentalHealthServices.Rows[i];
                        tableOutpatientMentalHealthServices.AddCell(new Phrase(rowOutpatientMentalHealthServices["Mentalhealthhistwhen"].ToString(), fntTableFont));
                        tableOutpatientMentalHealthServices.AddCell(new Phrase(rowOutpatientMentalHealthServices["Mentalhealthhistwhere"].ToString(), fntTableFont));
                        tableOutpatientMentalHealthServices.AddCell(new Phrase(rowOutpatientMentalHealthServices["Mentalhealthhistwithwhom"].ToString(), fntTableFont));
                        tableOutpatientMentalHealthServices.AddCell(new Phrase(rowOutpatientMentalHealthServices["Mentalhealthhistreason"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableOutpatientMentalHealthServices.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableDsmDiagnosis.AddCell(new PdfPCell(new Phrase("Diagnostic Code", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableDsmDiagnosis.AddCell(new PdfPCell(new Phrase("DSM - 5 Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableDsmDiagnosis.AddCell(new PdfPCell(new Phrase("ICD - 10 Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableDsmDiagnosis.AddCell(new PdfPCell(new Phrase("Diagnosis", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowDsmDiagnosis = null;
                dataTableDsmDiagnosis = dataSetFillPDF.Tables[31];
                if (dataSetFillPDF.Tables[31].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableDsmDiagnosis.Rows.Count; i++)
                    {
                        rowDsmDiagnosis = dataTableDsmDiagnosis.Rows[i];
                        tableDsmDiagnosis.AddCell(new Phrase(rowDsmDiagnosis["DiagnosticCodeDescription"].ToString(), fntTableFont));
                        tableDsmDiagnosis.AddCell(new Phrase(rowDsmDiagnosis["ICD5Name"].ToString(), fntTableFont));
                        tableDsmDiagnosis.AddCell(new Phrase(rowDsmDiagnosis["ICD10Name"].ToString(), fntTableFont));
                        tableDsmDiagnosis.AddCell(new Phrase(rowDsmDiagnosis["Diagnosis"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableDsmDiagnosis.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }



                //add the table to document
                Paragraph paragraphMembersFamilyConstellation = new Paragraph("Members of Family Constellation");
                tableMembersFamilyConstellation.SpacingAfter = 10;
                tableMembersFamilyConstellation.SpacingBefore = 10;
                tableEstablishedSupports.SpacingAfter = 10;
                tableEstablishedSupports.SpacingBefore = 10;



                // paragraph.SpacingAfter = 30;
                var cb = pdfStamper1.GetOverContent(1);
                var ct = new ColumnText(cb);
                ct.Alignment = Element.ALIGN_CENTER;
                ct.SetSimpleColumn(20, 10, PageSize.A4.Width - 20, PageSize.A4.Height - 600);
                ct.AddElement(paragraphMembersFamilyConstellation);
                ct.AddElement(tableMembersFamilyConstellation);
                ct.AddElement(tableEstablishedSupports);
                ct.Go();

                var cbSubstanceAbuseTreatment = pdfStamper1.GetOverContent(7);
                var ctSubstanceAbuseTreatment = new ColumnText(cbSubstanceAbuseTreatment);
                ctSubstanceAbuseTreatment.Alignment = Element.ALIGN_CENTER;
                ctSubstanceAbuseTreatment.SetSimpleColumn(20, 10, PageSize.A4.Width - 20, PageSize.A4.Height - 740);
                ctSubstanceAbuseTreatment.AddElement(tableSubstanceAbuseTreatment);
                ctSubstanceAbuseTreatment.Go();

                var cbOutpatientMentalHealthServices = pdfStamper1.GetOverContent(8);
                var ctOutpatientMentalHealthServices = new ColumnText(cbOutpatientMentalHealthServices);
                ctOutpatientMentalHealthServices.Alignment = Element.ALIGN_CENTER;
                ctOutpatientMentalHealthServices.SetSimpleColumn(20, 10, PageSize.A4.Width - 20, PageSize.A4.Height - 370);
                ctOutpatientMentalHealthServices.AddElement(tableOutpatientMentalHealthServices);
                ctOutpatientMentalHealthServices.Go();

                var cbDsmDiagnosis = pdfStamper1.GetOverContent(10);
                var ctDsmDiagnosis = new ColumnText(cbDsmDiagnosis);
                ctDsmDiagnosis.Alignment = Element.ALIGN_CENTER;
                ctDsmDiagnosis.SetSimpleColumn(20, 10, PageSize.A4.Width - 20, PageSize.A4.Height - 480);
                ctDsmDiagnosis.AddElement(tableDsmDiagnosis);
                ctDsmDiagnosis.Go();




                pdfStamper1.Dispose();
                // close the pdf
                pdfStamper1.Close();
                reader.Dispose();
                reader.Close();

                //-------------------------------------------------------------------------------------------------------//

                DataTable dataTableSummaryPrioritzedNeedsStrengths = null;
                DataTable dataTableServiceIntervensions = null;
                DataTable dataTableCansTreatmentPlans = null;

                PdfPTable tableSummaryPrioritzedNeedsStrengths = new PdfPTable(3);
                PdfPTable tableServiceIntervensions = new PdfPTable(6);
                PdfPTable tableCansTreatmentPlans = new PdfPTable(7);

                tableSummaryPrioritzedNeedsStrengths.WidthPercentage = 100f;
                tableServiceIntervensions.WidthPercentage = 100f;
                tableCansTreatmentPlans.WidthPercentage = 100f;

                tableSummaryPrioritzedNeedsStrengths.AddCell(new PdfPCell(new Phrase("Section Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableSummaryPrioritzedNeedsStrengths.AddCell(new PdfPCell(new Phrase("Item Text", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableSummaryPrioritzedNeedsStrengths.AddCell(new PdfPCell(new Phrase("Item Option", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowSummaryPrioritzedNeedsStrengths = null;
                dataTableSummaryPrioritzedNeedsStrengths = dataSetFillPDF.Tables[32];
                if (dataSetFillPDF.Tables[32].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableSummaryPrioritzedNeedsStrengths.Rows.Count; i++)
                    {
                        rowSummaryPrioritzedNeedsStrengths = dataTableSummaryPrioritzedNeedsStrengths.Rows[i];
                        tableSummaryPrioritzedNeedsStrengths.AddCell(new Phrase(rowSummaryPrioritzedNeedsStrengths["SectionName"].ToString(), fntTableFont));
                        tableSummaryPrioritzedNeedsStrengths.AddCell(new Phrase(rowSummaryPrioritzedNeedsStrengths["ItemText"].ToString(), fntTableFont));
                        tableSummaryPrioritzedNeedsStrengths.AddCell(new Phrase(rowSummaryPrioritzedNeedsStrengths["ItemOption"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableSummaryPrioritzedNeedsStrengths.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Start Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Treatment Plan", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Cans Items", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Client Goal", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Goal Status", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Cans Treatment Plan Onjective", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("Completed Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowCansTreatmentPlans = null;
                dataTableCansTreatmentPlans = dataSetFillPDF.Tables[38];
                if (dataSetFillPDF.Tables[38].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableCansTreatmentPlans.Rows.Count; i++)
                    {
                        rowCansTreatmentPlans = dataTableCansTreatmentPlans.Rows[i];
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["StartDate"].ToString(), fntTableFont));
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["TreatmentPlan"].ToString(), fntTableFont));
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["CansItems"].ToString(), fntTableFont));
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["ClientGoal"].ToString(), fntTableFont));
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["GoalStatus"].ToString(), fntTableFont));
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["CansTreatmentPlanObjctive"].ToString(), fntTableFont));
                        tableCansTreatmentPlans.AddCell(new Phrase(rowCansTreatmentPlans["CompletedDate"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("Objectives", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("Service Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("Mode", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("Place Of Service", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("Duration", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("Agency and Staff Responsible", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowServiceIntervensions = null;
                dataTableServiceIntervensions = dataSetFillPDF.Tables[33];
                if (dataSetFillPDF.Tables[33].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableServiceIntervensions.Rows.Count; i++)
                    {
                        rowServiceIntervensions = dataTableServiceIntervensions.Rows[i];
                        tableServiceIntervensions.AddCell(new Phrase(rowServiceIntervensions["GoalObjSvcNum"].ToString(), fntTableFont));
                        tableServiceIntervensions.AddCell(new Phrase(rowServiceIntervensions["ServiceType"].ToString(), fntTableFont));
                        tableServiceIntervensions.AddCell(new Phrase(rowServiceIntervensions["ServiceMode"].ToString(), fntTableFont));
                        tableServiceIntervensions.AddCell(new Phrase(rowServiceIntervensions["ServicePlace"].ToString(), fntTableFont));
                        tableServiceIntervensions.AddCell(new Phrase(rowServiceIntervensions["ServiceDuration"].ToString(), fntTableFont));
                        tableServiceIntervensions.AddCell(new Phrase(rowServiceIntervensions["AgencyAndStaffResponsible"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableServiceIntervensions.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                }
                string statusSummaryPrioritzedNeedsStrengths = string.Empty;
                if (dataTableSummaryPrioritzedNeedsStrengths.Rows.Count > 0)
                {
                    DataRow row = dataTableSummaryPrioritzedNeedsStrengths.Rows[0];
                    statusSummaryPrioritzedNeedsStrengths = row["Status"].ToString();
                    if (row["Status"].ToString() == "")
                    {
                        statusSummaryPrioritzedNeedsStrengths = "Start";
                    }
                }


                var doc1 = new Document(PageSize.A4);
                PdfReader reader1 = new PdfReader(NewCANSDocument);
                PdfWriter writer1 = PdfWriter.GetInstance(doc1, new FileStream(newFileAddCANSAssessment, FileMode.Create));
                doc1.Open();
                iTextSharp.text.Font pageHeaderFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                PdfPTable paragraphSummaryPrioritzedNeedsStrength = new PdfPTable(1);
                PdfPTable paragraphCansTreatmentPlans = new PdfPTable(1);
                PdfPTable paragraphServiceInterventions = new PdfPTable(1);
                paragraphSummaryPrioritzedNeedsStrength.WidthPercentage = 110f;
                paragraphCansTreatmentPlans.WidthPercentage = 110f;
                paragraphServiceInterventions.WidthPercentage = 110f;
                paragraphSummaryPrioritzedNeedsStrength.AddCell(new PdfPCell(new Phrase("14. SUMMARY OF PRIORITIZED CANS NEEDS AND STRENGTHS                " + statusSummaryPrioritzedNeedsStrengths + "", pageHeaderFont)) { Colspan = 1, PaddingBottom = 10, Border = 0, BackgroundColor = new BaseColor(205, 217, 234) });
                paragraphCansTreatmentPlans.AddCell(new PdfPCell(new Phrase("16. TREATMENT GOALS AND OBJECTIVE", pageHeaderFont)) { Colspan = 1, PaddingBottom = 10, Border = 0, BackgroundColor = new BaseColor(205, 217, 234) });
                paragraphServiceInterventions.AddCell(new PdfPCell(new Phrase("17. SERVICES / INTERVENTIONS", pageHeaderFont)) { Colspan = 1, PaddingBottom = 10, Border = 0, BackgroundColor = new BaseColor(205, 217, 234) });
                Paragraph paragraphSummaryPrioritzedNeedsStrengths = new Paragraph("14a. CANS Actionable Items to Consider for Treatment Planning", pageHeaderFont);
                Paragraph paragraphSummaryPrioritzedNeedsStrengths1 = new Paragraph("Add Summary Of Prioritized CANS Needs And Strengths Section items");
                // Paragraph paragraphCansTreatmentPlans = new Paragraph("16. TREATMENT GOALS AND OBJECTIVE", pageHeaderFont);
                Paragraph paragraphCansTreatmentPlans1 = new Paragraph("All treatment goals and objectives should be stated in client/family language and should relate back to the CANS actionable items identified in box 14a. Goals are specific, observable outcomes related to functioning that result from targeting symptoms and behaviors. Objectives are the specific steps to reach the goal.");
                // Paragraph paragraphServiceInterventions = new Paragraph("17. SERVICES / INTERVENTIONS", pageHeaderFont);
                tableSummaryPrioritzedNeedsStrengths.SpacingAfter = 10;
                tableSummaryPrioritzedNeedsStrengths.SpacingBefore = 10;
                tableServiceIntervensions.SpacingBefore = 20;
                tableServiceIntervensions.SpacingAfter = 20;
                tableCansTreatmentPlans.SpacingBefore = 20;
                tableCansTreatmentPlans.SpacingAfter = 20;

                doc1.Add(paragraphSummaryPrioritzedNeedsStrength);
                doc1.Add(paragraphSummaryPrioritzedNeedsStrengths);
                doc1.Add(paragraphSummaryPrioritzedNeedsStrengths1);
                doc1.Add(tableSummaryPrioritzedNeedsStrengths);
                doc1.Add(paragraphCansTreatmentPlans);
                doc1.Add(paragraphCansTreatmentPlans1);
                doc1.Add(tableCansTreatmentPlans);
                doc1.Add(paragraphServiceInterventions);
                doc1.Add(tableServiceIntervensions);

                doc1.Close();
                writer1.Dispose();
                writer1.Close();

                PdfReader readerSummaryPrioritzedNeedsStrengths = new PdfReader(newFileAddCANSAssessment);
                PdfStamper stamperHealthMedication = new PdfStamper(reader1, new FileStream(newFileAddHealthMedication, FileMode.Create));
                PdfImportedPage page3 = null;
                for (var i = 1; i <= readerSummaryPrioritzedNeedsStrengths.NumberOfPages; i++)
                {
                    int pageNo = 11 + i;
                    int insertpage = 11 + i;
                    stamperHealthMedication.InsertPage(insertpage, readerSummaryPrioritzedNeedsStrengths.GetPageSize(1));
                    PdfContentByte pb1 = stamperHealthMedication.GetUnderContent(pageNo);
                    page3 = stamperHealthMedication.GetImportedPage(readerSummaryPrioritzedNeedsStrengths, i);
                    pb1.AddTemplate(page3, 0, 0);
                }
                //close the stamper
                stamperHealthMedication.Dispose();
                stamperHealthMedication.Close();
                readerSummaryPrioritzedNeedsStrengths.Dispose();
                readerSummaryPrioritzedNeedsStrengths.Close();
                reader1.Dispose();
                reader1.Close();


                //-------------------------------------------------------------------------------------------------------------------//


                DataTable dataTableMedicationDetail = null;
                PdfPTable tableMedicationDetail = new PdfPTable(6);
                tableMedicationDetail.WidthPercentage = 100f;

                tableMedicationDetail.AddCell(new PdfPCell(new Phrase("Medication Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicationDetail.AddCell(new PdfPCell(new Phrase("Prescriber", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicationDetail.AddCell(new PdfPCell(new Phrase("Dosage", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicationDetail.AddCell(new PdfPCell(new Phrase("Date Started", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicationDetail.AddCell(new PdfPCell(new Phrase("Date Ended", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicationDetail.AddCell(new PdfPCell(new Phrase("Medication Issues", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowMedicationDetail = null;
                dataTableMedicationDetail = dataSetFillPDF.Tables[34];
                if (dataSetFillPDF.Tables[34].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableMedicationDetail.Rows.Count; i++)
                    {
                        rowMedicationDetail = dataTableMedicationDetail.Rows[i];
                        tableMedicationDetail.AddCell(new Phrase(rowMedicationDetail["MedicationName"].ToString(), fntTableFont));
                        tableMedicationDetail.AddCell(new Phrase(rowMedicationDetail["MedicationPrescriberName"].ToString(), fntTableFont));
                        tableMedicationDetail.AddCell(new Phrase(rowMedicationDetail["MedicationDosage"].ToString(), fntTableFont));
                        tableMedicationDetail.AddCell(new Phrase(rowMedicationDetail["MedicationPrescriptionBeginDate"].ToString(), fntTableFont));
                        tableMedicationDetail.AddCell(new Phrase(rowMedicationDetail["MedicationPrescriptionEndDate"].ToString(), fntTableFont));
                        tableMedicationDetail.AddCell(new Phrase(rowMedicationDetail["MedicationIssues"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableMedicationDetail.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableMedicationDetail.SpacingBefore = 20;
                tableMedicationDetail.SpacingAfter = 20;


                var doc2 = new Document(PageSize.A4);
                PdfReader reader2 = new PdfReader(newFileAddHealthMedication);
                PdfWriter writer2 = PdfWriter.GetInstance(doc2, new FileStream(newFileAddCANSAssessment, FileMode.Create));
                doc2.Open();
                tableMedicationDetail.SpacingBefore = 20;
                tableMedicationDetail.SpacingAfter = 20;
                Paragraph paragraphMedicationDetail = new Paragraph("Individual regularly receive lab work Details.");
                doc2.Add(paragraphMedicationDetail);
                doc2.Add(tableMedicationDetail);
                doc2.Close();
                writer2.Dispose();
                writer2.Close();

                PdfReader readerNew = new PdfReader(newFileAddCANSAssessment);
                PdfStamper stamperMedication = new PdfStamper(reader2, new FileStream(newFileAddPrioritzedNeedsStrength, FileMode.Create));
                PdfImportedPage page4 = null;
                for (var i = 1; i <= readerNew.NumberOfPages; i++)
                {
                    int pageNo = 12 + readerSummaryPrioritzedNeedsStrengths.NumberOfPages + i;
                    int insertpage = 12 + readerSummaryPrioritzedNeedsStrengths.NumberOfPages + i;
                    stamperMedication.InsertPage(insertpage, readerNew.GetPageSize(1));
                    PdfContentByte pb1 = stamperMedication.GetUnderContent(pageNo);
                    page4 = stamperMedication.GetImportedPage(readerNew, i);
                    pb1.AddTemplate(page4, 0, 0);
                }

                if (dataSetFillPDF.Tables[40].Rows.Count > 0)
                {
                    DataRow rowclient = dataTableSignatures.Rows[0];
                    object clientSig = rowclient["ClientSignature"];
                    string clientSign = BitConverter.ToString((byte[])clientSig);
                    object ParentSig = rowclient["ParentLegalGuardianSignature"];
                    string ParentSign = BitConverter.ToString((byte[])ParentSig);
                    if (clientSign != "")
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance((byte[])clientSig);
                        img.ScaleToFit(160F, 300F);
                        var cbSignatures = stamperMedication.GetOverContent(12 + readerSummaryPrioritzedNeedsStrengths.NumberOfPages);
                        var ctSignatures = new ColumnText(cbSignatures);
                        ctSignatures.Alignment = Element.ALIGN_CENTER;
                        ctSignatures.SetSimpleColumn(60, 10, PageSize.A4.Width - 30, PageSize.A4.Height - 140);
                        ctSignatures.AddElement(img);
                        ctSignatures.Go();
                    }
                    if (ParentSign != "")
                    {
                        iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance((byte[])ParentSig);
                        img1.ScaleToFit(160F, 300F);
                        var cbParentSignatures = stamperMedication.GetOverContent(12 + readerSummaryPrioritzedNeedsStrengths.NumberOfPages);
                        var ctParentSignatures = new ColumnText(cbParentSignatures);
                        ctParentSignatures.Alignment = Element.ALIGN_CENTER;
                        ctParentSignatures.SetSimpleColumn(60, 10, PageSize.A4.Width - 10, PageSize.A4.Height - 240);
                        ctParentSignatures.AddElement(img1);
                        ctParentSignatures.Go();
                    }

                }
                //close the stamper
                stamperMedication.Dispose();
                stamperMedication.Close();
                readerNew.Dispose();
                readerNew.Close();
                reader2.Dispose();
                reader2.Close();

                //-----------------------------------------------------------------------------------------------------------------------------//

                DataTable dataTableMedicalHistoryPsychHospital = null;
                DataTable dataMedicalHistoryAdditHospital = null;
                DataTable dataTableMedicalHistoryProvider = null;

                PdfPTable tableMedicalHistoryPsychHospital = new PdfPTable(4);
                PdfPTable tableMedicalHistoryAdditHospital = new PdfPTable(4);
                PdfPTable tableMedicalHistoryProvider = new PdfPTable(3);

                tableMedicalHistoryPsychHospital.WidthPercentage = 100f;
                tableMedicalHistoryAdditHospital.WidthPercentage = 100f;
                tableMedicalHistoryProvider.WidthPercentage = 100f;

                tableMedicalHistoryPsychHospital.AddCell(new PdfPCell(new Phrase("Hospital Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryPsychHospital.AddCell(new PdfPCell(new Phrase("Location (City, State)", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryPsychHospital.AddCell(new PdfPCell(new Phrase("Date Hospitalized", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryPsychHospital.AddCell(new PdfPCell(new Phrase("Reason(s)", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowMedicalHistoryPsychHospital = null;
                dataTableMedicalHistoryPsychHospital = dataSetFillPDF.Tables[35];
                if (dataSetFillPDF.Tables[35].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableMedicalHistoryPsychHospital.Rows.Count; i++)
                    {
                        rowMedicalHistoryPsychHospital = dataTableMedicalHistoryPsychHospital.Rows[i];
                        tableMedicalHistoryPsychHospital.AddCell(new Phrase(rowMedicalHistoryPsychHospital["PsychHospitalName"].ToString(), fntTableFont));
                        tableMedicalHistoryPsychHospital.AddCell(new Phrase(rowMedicalHistoryPsychHospital["PsychHospitalLocation"].ToString(), fntTableFont));
                        tableMedicalHistoryPsychHospital.AddCell(new Phrase(rowMedicalHistoryPsychHospital["PsychHospitalizationDate"].ToString(), fntTableFont));
                        tableMedicalHistoryPsychHospital.AddCell(new Phrase(rowMedicalHistoryPsychHospital["ReasonHospitalizedPsych"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableMedicalHistoryPsychHospital.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableMedicalHistoryAdditHospital.AddCell(new PdfPCell(new Phrase("Hospital Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryAdditHospital.AddCell(new PdfPCell(new Phrase("Location (City, State)", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryAdditHospital.AddCell(new PdfPCell(new Phrase("Date Hospitalized", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryAdditHospital.AddCell(new PdfPCell(new Phrase("Reason(s)", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowMedicalHistoryAdditHospital = null;
                dataMedicalHistoryAdditHospital = dataSetFillPDF.Tables[36];
                if (dataSetFillPDF.Tables[36].Rows.Count > 0)
                {
                    for (var i = 0; i < dataMedicalHistoryAdditHospital.Rows.Count; i++)
                    {
                        rowMedicalHistoryAdditHospital = dataMedicalHistoryAdditHospital.Rows[i];
                        tableMedicalHistoryAdditHospital.AddCell(new Phrase(rowMedicalHistoryAdditHospital["HospitalName"].ToString(), fntTableFont));
                        tableMedicalHistoryAdditHospital.AddCell(new Phrase(rowMedicalHistoryAdditHospital["HospitalLocation"].ToString(), fntTableFont));
                        tableMedicalHistoryAdditHospital.AddCell(new Phrase(rowMedicalHistoryAdditHospital["HospitalizationDate"].ToString(), fntTableFont));
                        tableMedicalHistoryAdditHospital.AddCell(new Phrase(rowMedicalHistoryAdditHospital["ReasonHospitalized"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableMedicalHistoryAdditHospital.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableMedicalHistoryProvider.AddCell(new PdfPCell(new Phrase("Provider Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryProvider.AddCell(new PdfPCell(new Phrase("Specialty", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalHistoryProvider.AddCell(new PdfPCell(new Phrase("Service(s) Provided", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowMedicalHistoryProvider = null;
                dataTableMedicalHistoryProvider = dataSetFillPDF.Tables[37];
                if (dataSetFillPDF.Tables[37].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTableMedicalHistoryProvider.Rows.Count; i++)
                    {
                        rowMedicalHistoryProvider = dataTableMedicalHistoryProvider.Rows[i];
                        tableMedicalHistoryProvider.AddCell(new Phrase(rowMedicalHistoryProvider["ProviderName"].ToString(), fntTableFont));
                        tableMedicalHistoryProvider.AddCell(new Phrase(rowMedicalHistoryProvider["ProviderServices"].ToString(), fntTableFont));
                        tableMedicalHistoryProvider.AddCell(new Phrase(rowMedicalHistoryProvider["ProviderSpecialty"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableMedicalHistoryProvider.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 3, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableMedicalHistoryPsychHospital.SpacingBefore = 20;
                tableMedicalHistoryPsychHospital.SpacingAfter = 20;
                tableMedicalHistoryAdditHospital.SpacingBefore = 20;
                tableMedicalHistoryAdditHospital.SpacingAfter = 20;
                tableMedicalHistoryProvider.SpacingBefore = 20;
                tableMedicalHistoryProvider.SpacingAfter = 20;

                var doc3 = new Document(PageSize.A4);
                PdfReader reader3 = new PdfReader(newFileAddPrioritzedNeedsStrength);
                PdfWriter writer3 = PdfWriter.GetInstance(doc3, new FileStream(newFileAddCANSAssessment, FileMode.Create));
                doc3.Open();
                Paragraph paragraphMedicalHistoryPsychHospital = new Paragraph("Has the individual ever been psychiatrically hospitalized?");
                Paragraph paragraphMedicalHistoryAdditHospital = new Paragraph("List all additional hospitalizations the individual has experienced. Attach additional pages as needed");
                Paragraph paragraphMedicalHistoryProvider = new Paragraph("List the names and specialties of the providers currently providing medical treatment to the individual. Attach additional pages as needed.");


                doc3.Add(paragraphMedicalHistoryPsychHospital);
                doc3.Add(tableMedicalHistoryPsychHospital);
                doc3.Add(paragraphMedicalHistoryAdditHospital);
                doc3.Add(tableMedicalHistoryAdditHospital);
                doc3.Add(paragraphMedicalHistoryProvider);
                doc3.Add(tableMedicalHistoryProvider);
                // doc3.Add(img1);

                doc3.Close();
                writer3.Dispose();
                writer3.Close();



                PdfReader readerMedicalHistoy = new PdfReader(newFileAddCANSAssessment);
                PdfStamper stamperMedicalHistoy = new PdfStamper(reader3, new FileStream(newFileAddMedicalHistory, FileMode.Create));
                PdfImportedPage page5 = null;
                for (var i = 1; i <= readerMedicalHistoy.NumberOfPages; i++)
                {
                    int pageNo = 15 + readerSummaryPrioritzedNeedsStrengths.NumberOfPages + readerNew.NumberOfPages + i;
                    int insertpage = 15 + readerSummaryPrioritzedNeedsStrengths.NumberOfPages + readerNew.NumberOfPages + i;
                    stamperMedicalHistoy.InsertPage(insertpage, readerMedicalHistoy.GetPageSize(1));
                    PdfContentByte pb1 = stamperMedicalHistoy.GetUnderContent(pageNo);
                    page5 = stamperMedicalHistoy.GetImportedPage(readerMedicalHistoy, i);
                    pb1.AddTemplate(page5, 0, 0);
                }

                iTextSharp.text.Font pageTextFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                for (int i = 1; i <= reader3.NumberOfPages; i++)
                {

                    ColumnText.ShowTextAligned(stamperMedicalHistoy.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + reader3.NumberOfPages, pageTextFont), 590f, 5f, 0);
                    ColumnText.ShowTextAligned(stamperMedicalHistoy.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), pageTextFont), 2f, 5f, 0);

                }




                //close the stamper
                stamperMedicalHistoy.Dispose();
                stamperMedicalHistoy.Close();
                readerMedicalHistoy.Dispose();
                readerMedicalHistoy.Close();
                reader3.Dispose();
                reader3.Close();






            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return newFileAddMedicalHistory;
        }


        private void ShowPublishedStatus(AcroFields pdfFormFields, bool blankSection, string pdfId)
        {
            if (blankSection == true)
            {
                pdfFormFields.SetField("" + pdfId + "", "N/A");


            }
            else
            {
                pdfFormFields.SetField("" + pdfId + "", "");
            }
        }
        private void showDrafStatus(AcroFields pdfFormFields, bool blankSection, string status, string pdfId)
        {
            if (blankSection == true)
            {
                pdfFormFields.SetField("" + pdfId + "", "Start");
            }
            else
            {
                pdfFormFields.SetField("" + pdfId + "", status);
            }
        }
    }
}
