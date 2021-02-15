using Newtonsoft.Json;
using IncidentManagement.Entities.Common;
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.Common;
using IncidentManagement.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using iTextSharp.text.pdf;
using System.IO;

namespace IncidentManagement.Repository.Repository
{
    public class IncidentManagementRepository : IIncidentManagementRepository
    {
        #region private
        BaseResponse baseResponse = null;
        Boolean isSuccess = false;
        IncidentManagementGeneralRespnse incidentManagementGeneralRespnse = null;
        IncidentManagementTabsResponse incidentManagementTabsResponse = null;
        AllPDFResponse allPDFResponse = null;

        AllPDFUploadResponse allPDFUploadResponse = null;

        #endregion


        public async Task<IncidentManagementGeneralRespnse> GetIncidentManagement(IncidentManagementRequest incidentManagementRequest)
        {


            incidentManagementGeneralRespnse = new IncidentManagementGeneralRespnse();
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(incidentManagementRequest.TabName);
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = incidentManagementRequest.Json;
                        cmd.Parameters.Add("@reportedby", SqlDbType.VarChar).Value = incidentManagementRequest.ReportedBy;
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
                    incidentManagementGeneralRespnse.GeneralTab = JsonConvert.DeserializeObject<List<GeneralTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return incidentManagementGeneralRespnse;
        }
        public async Task<IncidentManagementGeneralRespnse> EditAllRecord(IncidentManagementRequest incidentManagementRequest)
        {

            incidentManagementGeneralRespnse = new IncidentManagementGeneralRespnse();
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(incidentManagementRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@incidentManagementId", SqlDbType.VarChar).Value = incidentManagementRequest.IncidentManagementId;
                        //cmd.Parameters.Add("@url", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings["ReadFile"].ToString();
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0)
                {
                    string injurysubTable = "";
                    string opwdd147Table = "";
                    Thread.Sleep(4000);
                    string incidentManagementGeneral = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    string incidentManagementInjury = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[1]);
                    string incidentMedicationError = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[2]);
                    string incidentDeath = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[3]);
                    string incidentOther = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[4]);
                    string statesFormOPWDD147 = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[5]);
                    if (statesFormOPWDD147 != "[]")
                    {
                        opwdd147Table = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[13]);
                    }
                    string statesFormOPWDD148 = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[6]);
                    string statesFormOPWDD150 = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[7]);
                    string statesFormJonathanLaw = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[8]);

                    string stateForms = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[9]);
                    string injuryWitness = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[10]);
                    if (incidentManagementInjury != "[]")
                    {
                        injurysubTable = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[11]);
                    }
                    if (incidentMedicationError != "[]")
                    {
                        injurysubTable = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[12]);
                    }
                    string medicationErrorWitness = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[14]);
                    string deathWitness = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[15]);
                    string otherWitness = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[16]);
                    string satffInvolved = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[17]);
                    string uploadPdf = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[18]);

                    incidentManagementGeneralRespnse.IncidentManagementGeneralList = JsonConvert.DeserializeObject<List<IncidentManagementGeneralList>>(incidentManagementGeneral);
                    incidentManagementGeneralRespnse.IncidentManagementInjuryList = JsonConvert.DeserializeObject<List<IncidentManagementInjuryList>>(incidentManagementInjury);
                    incidentManagementGeneralRespnse.IncidentManagementMedicationErrorList = JsonConvert.DeserializeObject<List<IncidentManagementMedicationErrorList>>(incidentMedicationError);
                    incidentManagementGeneralRespnse.IncidentManagementDeathList = JsonConvert.DeserializeObject<List<IncidentManagementDeathList>>(incidentDeath);
                    incidentManagementGeneralRespnse.IncidentManagementOtherList = JsonConvert.DeserializeObject<List<IncidentManagementOtherList>>(incidentOther);
                    incidentManagementGeneralRespnse.IncidentManagementOPWDD147List = JsonConvert.DeserializeObject<List<IncidentManagementOPWDD147List>>(statesFormOPWDD147);
                    incidentManagementGeneralRespnse.IncidentManagementOPWDD148List = JsonConvert.DeserializeObject<List<IncidentManagementOPWDD148List>>(statesFormOPWDD148);
                    incidentManagementGeneralRespnse.IncidentManagementOPWDD150List = JsonConvert.DeserializeObject<List<IncidentManagementOPWDD150List>>(statesFormOPWDD150);
                    incidentManagementGeneralRespnse.IncidentManagementJonathanLawList = JsonConvert.DeserializeObject<List<IncidentManagementJonathanLawList>>(statesFormJonathanLaw);
                    incidentManagementGeneralRespnse.StateFormList = JsonConvert.DeserializeObject<List<StateFormList>>(stateForms);
                    incidentManagementGeneralRespnse.InjuryWitness = JsonConvert.DeserializeObject<List<InjuryWitness>>(injuryWitness);
                    if (incidentManagementInjury != "[]")
                    {
                        incidentManagementGeneralRespnse.InjurySubTableList = JsonConvert.DeserializeObject<List<InjurySubTableList>>(injurysubTable);
                    }
                    if (incidentMedicationError != "[]")
                    {
                        incidentManagementGeneralRespnse.InjurySubTableList = JsonConvert.DeserializeObject<List<InjurySubTableList>>(injurysubTable);
                    }

                    if (statesFormOPWDD147 != "[]")
                    {
                        incidentManagementGeneralRespnse.Opwdd147SubTable = JsonConvert.DeserializeObject<List<Opwdd147SubTable>>(opwdd147Table);

                    }
                    incidentManagementGeneralRespnse.MediWitness = JsonConvert.DeserializeObject<List<MediWitness>>(medicationErrorWitness);
                    incidentManagementGeneralRespnse.DeatWitness = JsonConvert.DeserializeObject<List<DeatWitness>>(deathWitness);
                    incidentManagementGeneralRespnse.OtheWitness = JsonConvert.DeserializeObject<List<OtheWitness>>(otherWitness);
                    incidentManagementGeneralRespnse.SatfInvolved = JsonConvert.DeserializeObject<List<SatfInvolved>>(satffInvolved);
                    incidentManagementGeneralRespnse.UploadedPDFResponse = JsonConvert.DeserializeObject<List<UploadedPDFResponse>>(uploadPdf);

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return incidentManagementGeneralRespnse;
        }

        public async Task<IncidentManagementTabsResponse> InsertModifyTabDetails(IncidentManagementTabRequest incidentManagementTabRequest)
        {
            incidentManagementTabsResponse = new IncidentManagementTabsResponse();
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(incidentManagementTabRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = incidentManagementTabRequest.Json;
                        cmd.Parameters.Add("@reportedby", SqlDbType.VarChar).Value = incidentManagementTabRequest.ReportedBy;
                        con.Open();
                        if (incidentManagementTabRequest.JsonChild != null)
                        {
                            cmd.Parameters.Add("@jsonchild", SqlDbType.VarChar).Value = incidentManagementTabRequest.JsonChild;
                        }
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));

                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    incidentManagementTabsResponse.AllTabs = JsonConvert.DeserializeObject<List<AllTabs>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return incidentManagementTabsResponse;
        }


        public async Task<IncidentManagementGeneralRespnse> DeleteMasterRecord(IncidentManagementRequest incidentManagementRequest)
        {
            incidentManagementGeneralRespnse = new IncidentManagementGeneralRespnse();
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(incidentManagementRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@incidentManagementId", SqlDbType.VarChar).Value = incidentManagementRequest.IncidentManagementId;
                        cmd.Parameters.Add("@reportedby", SqlDbType.VarChar).Value = incidentManagementRequest.ReportedBy;
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
                    incidentManagementGeneralRespnse.GeneralTab = JsonConvert.DeserializeObject<List<GeneralTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return incidentManagementGeneralRespnse;
        }



        public async Task<AllPDFResponse> FillableStateFormPDF(FillablePDFRequest fillablePDFRequest)
        {

            allPDFResponse = new AllPDFResponse();

            string tabName = fillablePDFRequest.TabName;
            string pdfTemplate = CommonFunctions.GetFillablePDFPath(fillablePDFRequest.TabName);
            string newTemplatePDf = string.Empty;
            try
            {
                switch (tabName)
                {
                    case "OPWDD147":
                        newTemplatePDf = GetOPWDDTabPDFTemplate(tabName, pdfTemplate, fillablePDFRequest);
                        break;
                    case "OPWDD148":
                        newTemplatePDf = GetOPWDDTabPDFTemplate(tabName, pdfTemplate, fillablePDFRequest);
                        break;
                    case "OPWDD150":
                        newTemplatePDf = GetOPWDDTabPDFTemplate(tabName, pdfTemplate, fillablePDFRequest);
                        break;

                }


                DataTable dataTable = new DataTable();
                dataTable.Clear();
                dataTable.Columns.Add("FileName");
                DataRow dataRow = dataTable.NewRow();
                dataRow["FileName"] = newTemplatePDf;
                dataTable.Rows.Add(dataRow);
                string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTable);
                allPDFResponse.AllPDF = JsonConvert.DeserializeObject<List<AllPDF>>(dataSetString);
                return allPDFResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        private string GetOPWDDTabPDFTemplate(string tabName, string pdfPath, FillablePDFRequest fillablePDFRequest)
        {
            DataSet dataSet = new DataSet();
            string newpdfPath = string.Empty;
            try
            {
                string storeProcedure = CommonFunctions.GetMappedStoreProcedure(tabName);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetStateFormsData", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@tabname", SqlDbType.VarChar).Value = fillablePDFRequest.TabName;
                        cmd.Parameters.Add("@incidentmanagementid", SqlDbType.Int).Value = fillablePDFRequest.IncidentManagementId;
                        cmd.Parameters.Add("@stateformid", SqlDbType.Int).Value = fillablePDFRequest.StateFormId;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    switch (tabName)
                    {
                        case "OPWDD147":
                            newpdfPath = FillOPDD147TabPDFTemplate(pdfPath, dataSet);
                            break;
                        case "OPWDD148":
                            newpdfPath = FillOPDD148TabPDFTemplate(pdfPath, dataSet.Tables[0]);
                            break;
                        case "OPWDD150":
                            newpdfPath = FillOPDD150TabPDFTemplate(pdfPath, dataSet.Tables[0]);
                            break;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return newpdfPath;
        }

        private string FillOPDD147TabPDFTemplate(string pdfPath, DataSet dataSetFillPDF)
        {
            string newFile = ConfigurationManager.AppSettings["TempPDF"].ToString() + "completed_147_fillable.pdf";
            PdfReader pdfReader = new PdfReader(pdfPath);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            DataTable dataTableFillPDF = dataSetFillPDF.Tables[0];
            DataTable dataTableNotifications = null;
            if (dataSetFillPDF.Tables.Count > 1)
            {
                dataTableNotifications = dataSetFillPDF.Tables[1];

            }

            AcroFields pdfFormFields = pdfStamper.AcroFields;
            pdfFormFields.GenerateAppearances = false;
            DataRow row = dataTableFillPDF.Rows[0];

            string Month = string.Empty;
            string Day = string.Empty;
            string Year = string.Empty;
            string Hour = string.Empty;
            string Minute = string.Empty;
            string AMPM = string.Empty;

            pdfFormFields.SetField("AgencycompletingForm", row["AgencyCompletingForm"].ToString());
            pdfFormFields.SetField("Facility", row["Facility"].ToString());
            pdfFormFields.SetField("ProgramType", row["ProgramType147"].ToString());
            pdfFormFields.SetField("Address", row["Address147"].ToString());
            pdfFormFields.SetField("Phone", row["Phone147"].ToString());
            pdfFormFields.SetField("MasterIncidentNumber", row["MasterIncidentNumber"].ToString());
            pdfFormFields.SetField("AgencyIncidentNumber", row["AgencyIncidentNumber"].ToString());
            pdfFormFields.SetField("IncidentPreviouslyReported", row["IncidentPreviouslyReported"].ToString());
            pdfFormFields.SetField("NAME_OF_PERSONS_RECEIVING_SERVICES_Last_First", row["IndividualName147"].ToString());
            pdfFormFields.SetField("DOB", row["DOB147"].ToString());
            pdfFormFields.SetField("TabId", row["TabId147"].ToString());
            pdfFormFields.SetField("Gender", row["Gender147"].ToString());


            pdfFormFields.SetField("ReceivesMedication", row["RecievesMedication"].ToString());
            pdfFormFields.SetField("IncidentWas", row["DateTimeIncidentWas"].ToString());

            if (row["DateTimeIncidentWas"].ToString() == "0")
            {
                if (!string.IsNullOrEmpty(row["ObservedDateTime"].ToString()))
                {

                    DateTime Observed = Convert.ToDateTime(row["ObservedDateTime"]);
                    Month = Observed.Month.ToString();
                    Day = Observed.Day.ToString();
                    Year = Observed.Year.ToString();
                   

                }
                if (!string.IsNullOrEmpty(row["Form147ObservedTime"].ToString()))
                {
                    DateTime ObservedTime = Convert.ToDateTime(row["Form147ObservedTime"]);
                    Hour = ObservedTime.Hour.ToString();
                    Minute = ObservedTime.Minute.ToString();
                    AMPM = ObservedTime.ToString("tt", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(row["DiscoveredDateTime"].ToString()))
                {

                    DateTime Discovered = Convert.ToDateTime(row["DiscoveredDateTime"]);
                    Month = Discovered.Month.ToString();
                    Day = Discovered.Day.ToString();
                    Year = Discovered.Year.ToString();
                   

                }
                if (!string.IsNullOrEmpty(row["Form147DiscoveredTime"].ToString()))
                {

                    DateTime Discovered = Convert.ToDateTime(row["Form147DiscoveredTime"]);
                    Hour = Discovered.Hour.ToString();
                    Minute = Discovered.Minute.ToString();
                    AMPM = Discovered.ToString("tt", CultureInfo.InvariantCulture);
                }
            }

            pdfFormFields.SetField("IncidentWasMO", Month);
            pdfFormFields.SetField("IncidentWasDAY", Day);
            pdfFormFields.SetField("IncidentWasYR", Year);
            pdfFormFields.SetField("IncidentWasHR", Hour);
            pdfFormFields.SetField("IncidentWasMIN", Minute);
            pdfFormFields.SetField("IncidentWasAMPM", AMPM);

            if (!string.IsNullOrEmpty(row["IncidentOccuredDateTime"].ToString()))
            {
                pdfFormFields.SetField("IncidentOcurredMO", Convert.ToDateTime(row["IncidentOccuredDateTime"]).Month.ToString());
                pdfFormFields.SetField("IncidentOcurredDAY", Convert.ToDateTime(row["IncidentOccuredDateTime"]).Day.ToString());
                pdfFormFields.SetField("IncidentOcurredYR", Convert.ToDateTime(row["IncidentOccuredDateTime"]).Year.ToString());
                
            }
            if (!string.IsNullOrEmpty(row["IncidentOccuredTime"].ToString()))
            {
                pdfFormFields.SetField("IncidentOcurredHR", Convert.ToDateTime(row["IncidentOccuredTime"]).Hour.ToString());
                pdfFormFields.SetField("IncidentOcurredMIN", Convert.ToDateTime(row["IncidentOccuredTime"]).Minute.ToString());
                pdfFormFields.SetField("IncidentOccuredAMPM", Convert.ToDateTime(row["IncidentOccuredTime"]).ToString("tt", CultureInfo.InvariantCulture));
            }


            pdfFormFields.SetField("PRSPresentAtIncident", row["PRSPresentAtIncident"].ToString());
            pdfFormFields.SetField("ERSPresentAtIncident", row["ERSPresentAtIncident"].ToString());
            pdfFormFields.SetField("ReportableIncident_AbuseNeglect", row["ReportableIncident_AbuseNeglect"].ToString());
            pdfFormFields.SetField("SeriousNotableOccurrences", row["SeriousNotableOccurrences"].ToString());
            pdfFormFields.SetField("MinornotableOccurrences", row["MinornotableOccurrences"].ToString());
            pdfFormFields.SetField("Reportable_SignificantIncidents", row["Reportable_SignificantIncidents"].ToString());
            pdfFormFields.SetField("IncidentOccurenceLocation", row["IncidentOccurenceLocation"].ToString());

            pdfFormFields.SetField("IncidentDescription", row["IncidentDescription"].ToString());
            pdfFormFields.SetField("ActionsTakenToSafeGuardPerson", row["ActionsTakenToSafeGuardPerson"].ToString());

            /////////////////////////////////////////////////////////////////////////
            pdfFormFields.SetField("JusticeCenter", row["JusticeCenter"].ToString());
            if (!string.IsNullOrEmpty(row["JusticeCenteDateTime"].ToString()))
            {
                pdfFormFields.SetField("JusticeCenteDate", Convert.ToDateTime(row["JusticeCenteDateTime"]).ToShortDateString());
               
            }
            if (!string.IsNullOrEmpty(row["JusticeCenterTime"].ToString()))
            {
                pdfFormFields.SetField("JusticeCenterTime", Convert.ToDateTime(row["JusticeCenterTime"]).ToShortTimeString());

            }
            pdfFormFields.SetField("JusticeCenterIdentifier", row["JusticeCenterIdentifier"].ToString());
            pdfFormFields.SetField("ReportedBy", row["ReportedBy"].ToString());
            /////////////////////////////////////////////////////////////////////////////////////


            pdfFormFields.SetField("LawEnforcementOfficialNotified", row["LawEnforcementOfficialNotified"].ToString());
            if (!string.IsNullOrEmpty(row["OfficialNotifiedDateTime"].ToString()))
            {
                pdfFormFields.SetField("OfficialNotifiedDate", Convert.ToDateTime(row["OfficialNotifiedDateTime"]).ToShortDateString());

            }
            if (!string.IsNullOrEmpty(row["OfficialNotifiedTime"].ToString()))
            {
                pdfFormFields.SetField("OfficialNotifiedTime", Convert.ToDateTime(row["OfficialNotifiedTime"]).ToShortTimeString());

            }
            pdfFormFields.SetField("LawEnforcementAgencyName", row["LawEnforcementAgencyName"].ToString());
            pdfFormFields.SetField("PermanentAddress_PhoneNumber", row["PermanentAddress_PhoneNumber"].ToString());
            /////////////////////////////////////////////////////////////////////////////////////////////////


            pdfFormFields.SetField("SOIRA", row["SOIRA"].ToString());
            pdfFormFields.SetField("VOIRA", row["VOIRA"].ToString());
            pdfFormFields.SetField("SOICF", row["SOICF"].ToString());
            pdfFormFields.SetField("VOICF", row["VOICF"].ToString());
            pdfFormFields.SetField("FC", row["FC"].ToString());
            pdfFormFields.SetField("DC", row["DC"].ToString());
            pdfFormFields.SetField("CR", row["CR"].ToString());
            pdfFormFields.SetField("Other", row["Other"].ToString());
            pdfFormFields.SetField("OtherSpecify", row["OtherSpecify"].ToString());


            pdfFormFields.SetField("PartyCompletingItemsName", row["PartyCompletingItemsName"].ToString());
            pdfFormFields.SetField("PartyCompletingItemsTitle", row["PartyCompletingItemsTitle"].ToString());
            pdfFormFields.SetField("PartyCompletingItemsDate", row["PartyCompletingItemsDate"].ToString());

            pdfFormFields.SetField("PartyReviewingItemsName", row["PartyReviewingItemsName"].ToString());
            pdfFormFields.SetField("PartyReviewingItemsTitle", row["PartyReviewingItemsTitle"].ToString());
            pdfFormFields.SetField("PartyReviewingItemsDate", row["PartyReviewingItemsDate"].ToString());


            pdfFormFields.SetField("AdditionalStepsToSafeGuardPerson", row["AdditionalStepsToSafeGuardPerson"].ToString());
            pdfFormFields.SetField("PartyCompletingItemsName28", row["PartyCompletingItem28"].ToString());
            pdfFormFields.SetField("PartyCompletingItemsTitle28", row["PartyCompletingItemTitle28"].ToString());
            pdfFormFields.SetField("PartyCompletingItemsDate28", row["PartyCompletingItemDate28"].ToString());

            if (dataTableNotifications != null && dataTableNotifications.Rows.Count > 0)
            {
                FillNotifications(pdfFormFields, dataTableNotifications);
            }



            // flatten the form to remove editting options, set it to false
            // to leave the form open to subsequent manual edits
            pdfStamper.FormFlattening = false;
            pdfStamper.Dispose();
            // close the pdf
            pdfStamper.Close();

            return newFile;
        }

        private void FillNotifications(AcroFields pdfFormFields, DataTable dataTableNotifications)
        {
            var IMURow = dataTableNotifications.AsEnumerable().Where
            (row => row.Field<int>("NotificationType") == 1);
            if (IMURow.Count() > 0)
            {

                pdfFormFields.SetField("IMUDate", IMURow.First()[4].ToString());
                pdfFormFields.SetField("IMUTime", IMURow.First()[5].ToString());


                pdfFormFields.SetField("IMUPersonContacted", IMURow.First()[6].ToString());
                pdfFormFields.SetField("IMUReportedBy", IMURow.First()[7].ToString());
                pdfFormFields.SetField("IMUMethod", IMURow.First()[8].ToString());

            }

            var DDSRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 2);
            if (DDSRow.Count() > 0)
            {

                pdfFormFields.SetField("DDSDate", DDSRow.First()[4].ToString());
                pdfFormFields.SetField("DDSTime", DDSRow.First()[5].ToString());


                pdfFormFields.SetField("DDSPersonContacted", DDSRow.First()[6].ToString());
                pdfFormFields.SetField("DDSReportedBy", DDSRow.First()[7].ToString());
                pdfFormFields.SetField("DDSMethod", DDSRow.First()[8].ToString());

            }
            var FGRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 3);
            if (FGRow.Count() > 0)
            {

                pdfFormFields.SetField("FGDate", FGRow.First()[4].ToString());
                pdfFormFields.SetField("FGTime", FGRow.First()[5].ToString());


                pdfFormFields.SetField("FGPersonContacted", FGRow.First()[6].ToString());
                pdfFormFields.SetField("FGReportedBy", FGRow.First()[7].ToString());
                pdfFormFields.SetField("FGMethod", FGRow.First()[8].ToString());

            }
            var SCRow = dataTableNotifications.AsEnumerable().Where
            (row => row.Field<int>("NotificationType") == 4);
            if (SCRow.Count() > 0)
            {

                pdfFormFields.SetField("SCDate", SCRow.First()[4].ToString());
                pdfFormFields.SetField("SCTime", SCRow.First()[5].ToString());


                pdfFormFields.SetField("SCPersonContacted", SCRow.First()[6].ToString());
                pdfFormFields.SetField("SCReportedBy", SCRow.First()[7].ToString());
                pdfFormFields.SetField("SCMethod", SCRow.First()[8].ToString());

            }
            var QIRow = dataTableNotifications.AsEnumerable().Where
             (row => row.Field<int>("NotificationType") == 5);
            if (QIRow.Count() > 0)
            {

                pdfFormFields.SetField("QIDate", QIRow.First()[4].ToString());
                pdfFormFields.SetField("QITime", QIRow.First()[5].ToString());


                pdfFormFields.SetField("QIPersonContacted", QIRow.First()[6].ToString());
                pdfFormFields.SetField("QIReportedBy", QIRow.First()[7].ToString());
                pdfFormFields.SetField("QIMethod", QIRow.First()[8].ToString());

            }
            var EDRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 6);
            if (EDRow.Count() > 0)
            {

                pdfFormFields.SetField("EDDate", EDRow.First()[4].ToString());
                pdfFormFields.SetField("EDTime", EDRow.First()[5].ToString());


                pdfFormFields.SetField("EDPersonContacted", EDRow.First()[6].ToString());
                pdfFormFields.SetField("EDReportedBy", EDRow.First()[7].ToString());
                pdfFormFields.SetField("EDMethod", EDRow.First()[8].ToString());

            }
            var NYCRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 7);
            if (NYCRow.Count() > 0)
            {

                pdfFormFields.SetField("NYCDate", NYCRow.First()[4].ToString());
                pdfFormFields.SetField("NYCTime", NYCRow.First()[5].ToString());


                pdfFormFields.SetField("NYCPersonContacted", NYCRow.First()[6].ToString());
                pdfFormFields.SetField("NYCReportedBy", NYCRow.First()[7].ToString());
                pdfFormFields.SetField("NYCMethod", NYCRow.First()[8].ToString());

            }
            var NYPRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 8);
            if (NYPRow.Count() > 0)
            {

                pdfFormFields.SetField("NYPDate", NYPRow.First()[4].ToString());
                pdfFormFields.SetField("NYPTime", NYPRow.First()[5].ToString());


                pdfFormFields.SetField("NYPPersonContacted", NYPRow.First()[6].ToString());
                pdfFormFields.SetField("NYPReportedBy", NYPRow.First()[7].ToString());
                pdfFormFields.SetField("NYPMethod", NYPRow.First()[8].ToString());

            }
            var SOWRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 9);
            if (SOWRow.Count() > 0)
            {

                pdfFormFields.SetField("SOWDate", SOWRow.First()[4].ToString());
                pdfFormFields.SetField("SOWTime", SOWRow.First()[5].ToString());


                pdfFormFields.SetField("SOWPersonContacted", SOWRow.First()[6].ToString());
                pdfFormFields.SetField("SOWReportedBy", SOWRow.First()[7].ToString());
                pdfFormFields.SetField("SOWMethod", SOWRow.First()[8].ToString());

            }
            var MHLRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 10);
            if (MHLRow.Count() > 0)
            {
                pdfFormFields.SetField("MHLDate", MHLRow.First()[4].ToString());
                pdfFormFields.SetField("MHLTime", MHLRow.First()[5].ToString());



                pdfFormFields.SetField("MHLPersonContacted", MHLRow.First()[6].ToString());
                pdfFormFields.SetField("MHLReportedBy", MHLRow.First()[7].ToString());
                pdfFormFields.SetField("MHLMethod", MHLRow.First()[8].ToString());

            }
            var BVTRow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 11);
            if (BVTRow.Count() > 0)
            {

                pdfFormFields.SetField("BVDate", BVTRow.First()[4].ToString());
                pdfFormFields.SetField("BVTime", BVTRow.First()[5].ToString());


                pdfFormFields.SetField("BVPersonContacted", BVTRow.First()[6].ToString());
                pdfFormFields.SetField("BVReportedBy", BVTRow.First()[7].ToString());
                pdfFormFields.SetField("BVMethod", BVTRow.First()[8].ToString());

            }
            var CDERow = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 12);
            if (CDERow.Count() > 0)
            {

                pdfFormFields.SetField("CDEDate", CDERow.First()[4].ToString());
                pdfFormFields.SetField("CDETime", CDERow.First()[5].ToString());


                pdfFormFields.SetField("CDEPersonContacted", CDERow.First()[6].ToString());
                pdfFormFields.SetField("CDEReportedBy", CDERow.First()[7].ToString());
                pdfFormFields.SetField("CDEMethod", CDERow.First()[8].ToString());

            }
            var Other1Row = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 13);
            if (Other1Row.Count() > 0)
            {

                pdfFormFields.SetField("Other1Date", Other1Row.First()[4].ToString());
                pdfFormFields.SetField("Other1Time", Other1Row.First()[5].ToString());


                pdfFormFields.SetField("Other1PersonContacted", Other1Row.First()[6].ToString());
                pdfFormFields.SetField("Other1ReportedBy", Other1Row.First()[7].ToString());
                pdfFormFields.SetField("Other1Method", Other1Row.First()[8].ToString());

            }
            var Other2Row = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 14);
            if (Other2Row.Count() > 0)
            {

                pdfFormFields.SetField("Other2Date", Other2Row.First()[4].ToString());
                pdfFormFields.SetField("Other2Time", Other2Row.First()[5].ToString());


                pdfFormFields.SetField("Other2PersonContacted", Other2Row.First()[6].ToString());
                pdfFormFields.SetField("Other2ReportedBy", Other2Row.First()[7].ToString());
                pdfFormFields.SetField("Other2Method", Other2Row.First()[8].ToString());

            }
            var Other3Row = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 15);
            if (Other3Row.Count() > 0)
            {

                pdfFormFields.SetField("Other3Date", Other3Row.First()[4].ToString());
                pdfFormFields.SetField("Other3Time", Other3Row.First()[5].ToString());


                pdfFormFields.SetField("Other3PersonContacted", Other3Row.First()[6].ToString());
                pdfFormFields.SetField("Other3ReportedBy", Other3Row.First()[7].ToString());
                pdfFormFields.SetField("Other3Method", Other3Row.First()[8].ToString());

            }
            var Other4Row = dataTableNotifications.AsEnumerable().Where
           (row => row.Field<int>("NotificationType") == 16);
            if (Other4Row.Count() > 0)
            {

                pdfFormFields.SetField("Other4Date", Other4Row.First()[4].ToString());
                pdfFormFields.SetField("Other4Time", Other4Row.First()[5].ToString());


                pdfFormFields.SetField("Other4PersonContacted", Other4Row.First()[6].ToString());
                pdfFormFields.SetField("Other4ReportedBy", Other4Row.First()[7].ToString());
                pdfFormFields.SetField("Other4Method", Other4Row.First()[8].ToString());

            }
        }
        private string FillOPDD148TabPDFTemplate(string pdfPath, DataTable dataTableFillPDF)
        {
            string newFile = ConfigurationManager.AppSettings["TempPDF"].ToString() + "completed_148_fillable.pdf";
            PdfReader pdfReader = new PdfReader(pdfPath);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));

            AcroFields pdfFormFields = pdfStamper.AcroFields;
            pdfFormFields.GenerateAppearances = false;
            DataRow row = dataTableFillPDF.Rows[0];

            pdfFormFields.SetField("topmostSubform[0].Page1[0].ContactInfo[0]", row["AddInfoContact"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].TelephoneAt[0]", row["AtTelephone"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].NameReceiving[0]", row["PersonReceivingServicesName"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].ClassOfIncident[0]", row["PreliminaryClassificationOfIncident"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0]._27_PRINT_NAME_OF_PARTY_REVIEWING_FORM_Date[0]", row["IncidentOccurredOrWasDiscoveredDate"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].Name[0]", row["RelationshipToPersonReceivingServices"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].reportName[0]", row["ReportProvidedTo"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].AgencyCoForm[0]", row["AgencyCompletingThisForm"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0]._27_PRINT_NAME_OF_PARTY_REVIEWING_FORM_Date[1]", row["InitialnotificationProvidedToPersonReceivingDate"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0]._27_PRINT_NAME_OF_PARTY_REVIEWING_FORM_Date[2]", row["ReportCompletedDate"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].Name[1]", row["NameOfPersonCompletingThisReport"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].TextField1[0]", row["ImmediateStepsTakenInResponse"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].MasterIncidentNum[0]", row["MasterIncidentNumber"].ToString());
            pdfFormFields.SetField("topmostSubform[0].Page1[0].PHONENumber[0]", row["PhoneNumber"].ToString());


            // flatten the form to remove editting options, set it to false
            // to leave the form open to subsequent manual edits
            pdfStamper.FormFlattening = false;

            // close the pdf
            pdfStamper.Close();

            return newFile;
        }
        private string FillOPDD150TabPDFTemplate(string pdfPath, DataTable dataTableFillPDF)
        {
            string newFile = ConfigurationManager.AppSettings["TempPDF"].ToString() + "completed_150_fillable.pdf";
            PdfReader pdfReader = new PdfReader(pdfPath);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));

            AcroFields pdfFormFields = pdfStamper.AcroFields;
            pdfFormFields.GenerateAppearances = false;
            DataRow row = dataTableFillPDF.Rows[0];

            // The first worksheet and W-4 form
            pdfFormFields.SetField("1 REPORTING AGENCY", row["ReportingAgency"].ToString());
            pdfFormFields.SetField("2 PROGRAM TYPE", row["ProgramType"].ToString());
            pdfFormFields.SetField("3 PROGRAM ADDRESS", row["ProgramAddress"].ToString());
            pdfFormFields.SetField("4 ADDRESS WHEN EVENTSITUATION OCCURRED", row["EventAddress"].ToString());
            pdfFormFields.SetField("AREA", row["Event_SituationReferenceNumber"].ToString());
            pdfFormFields.SetField("5 PHONE", row["Phone"].ToString());
            pdfFormFields.SetField("6 EVENTSITUATION REFERENCE NUMBER", row["Event_SituationReferenceNumber"].ToString());
            pdfFormFields.SetField("7 PERSON COMPLETING REPORT", row["PersonCompletingReport"].ToString());
            pdfFormFields.SetField("8 NAME OF INVOLVED INDIVIDUAL Last First", row["IndividualName"].ToString());
            pdfFormFields.SetField("9 DATE OF BIRTH", row["DOB"].ToString());
            pdfFormFields.SetField("1", row["Gender"].ToString());
            pdfFormFields.SetField("11 TABS ID if applicable", row["TabId"].ToString());
            pdfFormFields.SetField("Discovered-Observed", row["DateTimeEvent_Situation"].ToString());
            pdfFormFields.SetField("Date Observed/Discovered", row["Event_SituationDate"].ToString());
            pdfFormFields.SetField("Time Observed/Discovered", row["Event_SituationTime"].ToString());
            pdfFormFields.SetField("AMPM1", row["AM_PM1"].ToString());
            pdfFormFields.SetField("Date Event Occurred", row["Event_SituationOccureDate"].ToString());
            pdfFormFields.SetField("Time Event Occurred", row["Event_SituationOccureTime"].ToString());
            pdfFormFields.SetField("AMPM2", row["Event_SituationOccureAM_PM"].ToString());
            pdfFormFields.SetField("Preliminary Classification", row["PreliminaryClassification"].ToString());
            pdfFormFields.SetField("Adult Protective Services", row["AdultProtectiveServices"].ToString());
            pdfFormFields.SetField("Assessing and monitoring the individual", row["AssessMonitorIndividual"].ToString());
            pdfFormFields.SetField("Family Members", row["FamilyMembers"].ToString());
            pdfFormFields.SetField("Educating the individual about", row["EducateIndividualChoices"].ToString());
            pdfFormFields.SetField("Hospital", row["Hospital"].ToString());
            pdfFormFields.SetField("Law Enforcement", row["LawEnforcement"].ToString());
            pdfFormFields.SetField("Interview involved individuals andor", row["InterviewInvolvedIndividuals"].ToString());
            pdfFormFields.SetField("Office of Professional Discipline", row["ProfessionalDisciplineOffice"].ToString());
            pdfFormFields.SetField("School", row["School"].ToString());
            pdfFormFields.SetField("Offering to make referral to appropriate", row["OfferingReferralAppropriateService"].ToString());
            pdfFormFields.SetField("Statewide Central Register of Child", row["StatewideCentralRegisterChildAbuseAndMaltreatment"].ToString());
            pdfFormFields.SetField("Review records and other relevant", row["ReviewRecordIOtherRelevant"].ToString());
            pdfFormFields.SetField("Other_2", row["Other"].ToString());
            pdfFormFields.SetField("17 DESCRIPTION OF EVENTSITUATION Initial Findings in IRMA", row["DescriptionOfEventSituation"].ToString());
            pdfFormFields.SetField("18 SUMMARY OF RESOLUTION OF EVENTSITUATION Conclusions in IRMA", row["SummaryResloutionOfEvent_Situation"].ToString());

            pdfFormFields.SetField("CONTACTRow1", row["NotificationContact"].ToString());
            pdfFormFields.SetField("DATERow1", row["NotificationDate"].ToString());
            pdfFormFields.SetField("TIMERow1", row["NotificationTime"].ToString());
            pdfFormFields.SetField("PERSON CONTACTEDRow1", row["PersonContacted"].ToString());
            pdfFormFields.SetField("REPORTED BYRow1", row["ReportedBy"].ToString());
            pdfFormFields.SetField("METHODRow1", row["Method"].ToString());

            pdfFormFields.SetField("CONTACTRow2", row["NotificationContact1"].ToString());
            pdfFormFields.SetField("DATERow2", row["NotificationDateTime1"].ToString());
            pdfFormFields.SetField("TIMERow2", row["NotificationTime1"].ToString());
            pdfFormFields.SetField("PERSON CONTACTEDRow2", row["PersonContacted1"].ToString());
            pdfFormFields.SetField("REPORTED BYRow2", row["ReportedBy1"].ToString());
            pdfFormFields.SetField("METHODRow2", row["Method1"].ToString());

            pdfFormFields.SetField("20 PRINT NAME OF PARTY COMPLETING FORM", row["PartyCompletingFormName"].ToString());
            pdfFormFields.SetField("TITLE", row["PartyCompletingFormTitle"].ToString());
            pdfFormFields.SetField("DATE", row["PartyCompletingFormDate"].ToString());

            // flatten the form to remove editting options, set it to false
            // to leave the form open to subsequent manual edits
            pdfStamper.FormFlattening = false;

            // close the pdf
            pdfStamper.Close();

            return newFile;
        }



        public async Task<AllPDFUploadResponse> UploadPDFFiles(string json, string files)
        {
            allPDFUploadResponse = new AllPDFUploadResponse();
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_UploadPDF", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = json;
                        //cmd.Parameters.Add("@url", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings["ReadFile"].ToString();
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
                    allPDFUploadResponse.UploadedPDFResponse = JsonConvert.DeserializeObject<List<UploadedPDFResponse>>(dataSetString);
                }
                return allPDFUploadResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public async Task<AllPDFUploadResponse> DownloadUPloadedFile(IncidentManagementRequest incidentManagementRequest)
        {
            allPDFUploadResponse = new AllPDFUploadResponse();
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetUploadedPDFPath", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@stateformpdfversioningid", SqlDbType.Int).Value = incidentManagementRequest.StateFormPDFVersioningId;
                        cmd.Parameters.Add("@incidentmanagementid", SqlDbType.Int).Value = incidentManagementRequest.IncidentManagementId;
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
                    allPDFUploadResponse.UploadedPDFResponse = JsonConvert.DeserializeObject<List<UploadedPDFResponse>>(dataSetString);
                }
                return allPDFUploadResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


    }


}
