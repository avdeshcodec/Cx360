using IncidentManagement.Entities.Common;
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.LifePlanResponse;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using System.Web;
using PdfSharp.Pdf.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Net;
using System.Text;

namespace IncidentManagement.Repository.Repository
{
    public class LIfePlanRepository : ILIfePlanRepository
    {
        #region Private
        LifePlanDetailTabResponse lpdResponse = null;

        MeetingHistorySummaryResponse mhsResponse = null;

        IndividualSafeSummaryResponse issResponse = null;

        AssessmentNarrativeSummaryResponse assessmentNarrativeSummaryResponse = null;

        OutcomesSupportStrategiesResponse outcomesSupportStrategiesResponse = null;

        HCBSWaiverResponse hCBSWaiverResponse = null;

        FundalNaturalCommunityResourcesResponse fundalNaturalCommunityResourcesResponse = null;

        LifePlanPDFResponse lifePlanPDFResponse = null;
        LifePlanExportedRecordsResponse lifePlanExportedRecordsResponse = null;
        MemberRepresentativeResponse MemberRepresentativeResponse = null;
        MemberRightResponse MemberRightResponse = null;

        #endregion



        public async Task<LifePlanDetailTabResponse> InsertModifyLifePlanDetail(LifePlanDetailRequest lpdRequest)
        {

            bool checkVersion = true;

            if (lpdRequest.TabName == "LifePlan")
            {
                checkVersion = ValidateLifePlanDraft(lpdRequest);
            }





            lpdResponse = new LifePlanDetailTabResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
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
                            cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = lpdRequest.Json;
                            cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = lpdRequest.ReportedBy;
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
                        lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetString);
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
                    lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetString);
                    return lpdResponse;
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return lpdResponse;

        }


        public async Task<LifePlanDetailTabResponse> HandleLifePlanDetail(LifePlanDetailRequest lpdRequest)
        {
            lpdResponse = new LifePlanDetailTabResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@reportedby", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    lpdResponse.LifPlanDetailsData = JsonConvert.DeserializeObject<List<LifPlanDetailsData>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return lpdResponse;

        }

        public async Task<MeetingHistorySummaryResponse> HandleMeetingDetail(LifePlanDetailRequest lpdRequest)
        {
            mhsResponse = new MeetingHistorySummaryResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            DataSet dataSet = new DataSet();
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    mhsResponse.MeetingHistorySummaryTab = JsonConvert.DeserializeObject<List<MeetingHistorySummaryTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return mhsResponse;

        }


        public async Task<IndividualSafeSummaryResponse> HandleIndividualSafeRecords(LifePlanDetailRequest lpdRequest)
        {
            issResponse = new IndividualSafeSummaryResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    issResponse.IndividualSafeSummaryTab = JsonConvert.DeserializeObject<List<IndividualSafeSummaryTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return issResponse;

        }

        public async Task<AssessmentNarrativeSummaryResponse> HandleAssessmentNarrativeSummary(LifePlanDetailRequest lpdRequest)
        {
            assessmentNarrativeSummaryResponse = new AssessmentNarrativeSummaryResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    assessmentNarrativeSummaryResponse.AssessmentNarrativeSummaryTab = JsonConvert.DeserializeObject<List<AssessmentNarrativeSummaryTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return assessmentNarrativeSummaryResponse;

        }

        public async Task<OutcomesSupportStrategiesResponse> HandleOutcomesStrategies(LifePlanDetailRequest lpdRequest)
        {
            outcomesSupportStrategiesResponse = new OutcomesSupportStrategiesResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    outcomesSupportStrategiesResponse.OutcomesSupportStrategiesTab = JsonConvert.DeserializeObject<List<OutcomesSupportStrategiesTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return outcomesSupportStrategiesResponse;

        }

        public async Task<HCBSWaiverResponse> HandleHCBSWaiver(LifePlanDetailRequest lpdRequest)
        {
            hCBSWaiverResponse = new HCBSWaiverResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    hCBSWaiverResponse.HCBSWaiverTab = JsonConvert.DeserializeObject<List<HCBSWaiverTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return hCBSWaiverResponse;

        }

        public async Task<FundalNaturalCommunityResourcesResponse> HandleFundalNaturalCommunityResources(LifePlanDetailRequest lpdRequest)
        {
            fundalNaturalCommunityResourcesResponse = new FundalNaturalCommunityResourcesResponse();

            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    fundalNaturalCommunityResourcesResponse.fundalNaturalCommunityResourcesTab = JsonConvert.DeserializeObject<List<FundalNaturalCommunityResourcesTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return fundalNaturalCommunityResourcesResponse;

        }
        public async Task<LifePlanDetailTabResponse> HandleLifeNotifications(LifePlanDetailRequest lpdRequest)
        {
            lpdResponse = new LifePlanDetailTabResponse();
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetLifePlanNotificationsDetail", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return lpdResponse;

        }

        public async Task<LifePlanPDFResponse> FillableLifePlanPDF(FillableLifePlanPDFRequest fillableLifePlanPDFRequest)
        {
            lifePlanPDFResponse = new LifePlanPDFResponse();

            string tabName = fillableLifePlanPDFRequest.TabName;
            string pdfTemplate = CommonFunctions.GetFillablePDFPath(fillableLifePlanPDFRequest.TabName);
            string newTemplatePDf = string.Empty;

            DataTable dataTablePath = null;
            try
            {
                switch (tabName)
                {
                    case "LifePlanPDF":
                        dataTablePath = GetLifePlanPDFTemplate(tabName, pdfTemplate, fillableLifePlanPDFRequest);
                        break;
                }
                string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTablePath);
                lifePlanPDFResponse.LifePlanPDF = JsonConvert.DeserializeObject<List<LifePlanPDF>>(dataSetString);
                return lifePlanPDFResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private DataTable GetLifePlanPDFTemplate(string tabName, string pdfPath, FillableLifePlanPDFRequest fillablePDFRequest)
        {
            DataSet dataSet = new DataSet();
            string newpdfPath = string.Empty;
            DataTable dataTablePath = null;
            try
            {
                string storeProcedure = CommonFunctions.GetMappedStoreProcedure(tabName);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetLifePlanPDFDetails", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.Int).Value = fillablePDFRequest.LifePlanId;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {

                    dataTablePath = LifePlanPDFTemplate(pdfPath, dataSet, fillablePDFRequest, tabName);

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return dataTablePath;
        }

        private DataTable LifePlanPDFTemplate(string pdfPath, DataSet dataSetFillPDF, FillableLifePlanPDFRequest fillablePDFRequest, string tabName)
        {
            string newFile = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed_lifeplan.pdf";
            string newFile1 = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed1_lifeplan.pdf";
            string finaldoc = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed2_lifeplan.pdf";
            string finaldoc1 = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed3_lifeplan.pdf";
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(pdfPath);
            // iTextSharp.text.pdf.PdfReader pdfReader8 = new iTextSharp.text.pdf.PdfReader(ConfigurationManager.AppSettings["DocumentFile"].ToString() + "form_Requirements_Updates_Changes_ForDev.docx");

            DataTable dataTable = new DataTable();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            try
            {

                DataTable Lifeplan = dataSetFillPDF.Tables[0];
                DataTable MeetingHistory = dataSetFillPDF.Tables[1];
                DataTable AssessmentNarrativeSummary = dataSetFillPDF.Tables[2];
                DataTable Outcomes_SupportStrategies = dataSetFillPDF.Tables[3];
                DataTable IndividualPlanOfProtection = dataSetFillPDF.Tables[4];
                DataTable MedicaidStatePlanAuthorizedServies = dataSetFillPDF.Tables[5];
                DataTable FundalNaturalCommunityResources = dataSetFillPDF.Tables[6];
                DataTable AcknowledgementAndAgreement = dataSetFillPDF.Tables[7];
                DataTable MemberRights = dataSetFillPDF.Tables[8];
                DataTable MemberRepresentativeApproval = dataSetFillPDF.Tables[9];
                DataTable Documents = dataSetFillPDF.Tables[9];

                AcroFields pdfFormFields = pdfStamper.AcroFields;
                pdfFormFields.GenerateAppearances = false;
                DataRow row = Lifeplan.Rows[0];

                pdfFormFields.SetField("IndividualName", fillablePDFRequest.IndividualName);
                pdfFormFields.SetField("DateOfBirth", fillablePDFRequest.DateOfBirth);
                pdfFormFields.SetField("MemberAddress", row["MemberAddress"].ToString() + ' ' + fillablePDFRequest.AddressLifePlan);
                pdfFormFields.SetField("Phone", row["Phone"].ToString());
                pdfFormFields.SetField("Medicaid", row["Medicaid"].ToString());
                pdfFormFields.SetField("Medicare", row["Medicare"].ToString());
                pdfFormFields.SetField("EffectiveFromDate", row["EffectiveFromDate"].ToString());
                pdfFormFields.SetField("EffectiveToDate", row["EffectiveToDate"].ToString());
                pdfFormFields.SetField("EnrollmentDate", row["EnrollmentDate"].ToString());
                pdfFormFields.SetField("WillowbookerMember", row["WillowbrookMember"].ToString());
                pdfFormFields.SetField("AddressCCO", row["AddressCCO"].ToString() + ' ' + fillablePDFRequest.AddressCCO);
                pdfFormFields.SetField("PhoneCCO", row["PhoneCCO"].ToString());
                pdfFormFields.SetField("Fax", row["Fax"].ToString());
                pdfFormFields.SetField("ProviderID", row["ProviderID"].ToString());
                pdfFormFields.SetField("Status", row["DocumentStatus"].ToString());
                pdfFormFields.SetField("Version", row["DocumentVersion"].ToString());
                pdfFormFields.SetField("CareManagerFirstName", row["CareManagerFirstName"].ToString());
                pdfFormFields.SetField("CareManagerLastName", row["CareManagerLastName"].ToString());
                pdfFormFields.SetField("IncludeDurableMediEquipment", row["IncludeDurableMediEquipment"].ToString() == "Y" ? "Yes" : "No");
                pdfFormFields.SetField("IncludeDiagnosis", row["IncludeDiagnosis"].ToString() == "Y" ? "Yes" : "No");
                pdfFormFields.SetField("IncludeAllergies", row["IncludeAllergies"].ToString() == "Y" ? "Yes" : "No");
                pdfFormFields.SetField("IncludeMedications", row["IncludeMedications"].ToString() == "Y" ? "Yes" : "No");
                pdfFormFields.SetField("LifePlanType", row["LifePlanType"].ToString());

                //Will create pdfTable instance and will paas column numbers as parameters.
                PdfPTable table = new PdfPTable(4);
                PdfPTable tableOutcomes_SupportStrategies = new PdfPTable(9);
                PdfPTable tableIndividualPlanOfProtection = new PdfPTable(8);
                PdfPTable tableMedicaidStatePlanAuthorizedServies = new PdfPTable(5);
                PdfPTable tableFundalNaturalCommunityResources = new PdfPTable(4);
                PdfPTable tableAssessmentNarrativeSummary = new PdfPTable(2);
                PdfPTable tableAcknowledgementAndAgreement = new PdfPTable(6);
                PdfPTable tableMemberRights = new PdfPTable(1);
                PdfPTable tableMemberRepresentativeApproval = new PdfPTable(6);
                PdfPTable tableDocuments = new PdfPTable(2);
                PdfPTable tableMeetingAttendance = new PdfPTable(3);

                table.WidthPercentage = 100f;
                tableOutcomes_SupportStrategies.WidthPercentage = 100f;
                tableIndividualPlanOfProtection.WidthPercentage = 100f;
                tableMedicaidStatePlanAuthorizedServies.WidthPercentage = 100f;
                tableFundalNaturalCommunityResources.WidthPercentage = 100f;
                tableAssessmentNarrativeSummary.WidthPercentage = 100f;
                tableAcknowledgementAndAgreement.WidthPercentage = 100f;
                tableMemberRights.WidthPercentage = 100f;
                tableMemberRepresentativeApproval.WidthPercentage = 100f;
                tableDocuments.WidthPercentage = 100f;
                tableMeetingAttendance.WidthPercentage = 80f;

                iTextSharp.text.Font fntTableFontHdr = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fntTableFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font pageTextFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                PdfPCell cell = new PdfPCell(new Phrase("Meeting History")) { PaddingBottom = 10 };
                cell.Colspan = 4;
                cell.BackgroundColor = new BaseColor(204, 204, 204);
                cell.HorizontalAlignment = 1;
                table.SpacingBefore = 30f;


                if (MeetingHistory.Rows.Count > 0)
                {
                    for (var i = 0; i < MeetingHistory.Rows.Count; i++)
                    {
                        table.AddCell(cell);
                        table.AddCell(new PdfPCell(new Phrase("Note Type")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                        table.AddCell(new PdfPCell(new Phrase("Event Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                        table.AddCell(new PdfPCell(new Phrase("Subject")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                        table.AddCell(new PdfPCell(new Phrase("Meeting Reason")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                        DataRow rowMeetingHistory = MeetingHistory.Rows[i];
                        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["TypeOfMeeting"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["PlanerReviewDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["MeetingReason"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["MemberAttendance"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                        var filtered = new List<JSONData>();

                        foreach (var e in fillablePDFRequest.JSONData)
                        {
                            if (e.meetingAttendanceId == Convert.ToInt32(rowMeetingHistory["MeetingId"]))
                            {
                                filtered.Add(e);
                            }
                        }
                        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Member Attendance")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, PaddingBottom = 10 });
                        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Contact Name")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Relationship To Member")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Method")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                        if (filtered.Count > 0)
                        {
                            foreach (var e in filtered)
                            {
                                if (e.meetingAttendanceId == Convert.ToInt32(rowMeetingHistory["MeetingId"]))
                                {
                                    tableMeetingAttendance.AddCell(new PdfPCell(new Phrase(e.ContactName.ToString(), fntTableFont)) { PaddingBottom = 10 });
                                    tableMeetingAttendance.AddCell(new PdfPCell(new Phrase(e.RelationshipToMember.ToString(), fntTableFont)) { PaddingBottom = 10 });
                                    tableMeetingAttendance.AddCell(new PdfPCell(new Phrase(e.Method.ToString(), fntTableFont)) { PaddingBottom = 10 });
                                }
                            }
                        }
                        else
                        {
                            tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                        }
                        table.AddCell(new PdfPCell(tableMeetingAttendance) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10, PaddingLeft = 10, PaddingRight = 10, PaddingTop = 10 });
                        table.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.TOP_BORDER });
                        filtered = null;
                        tableMeetingAttendance.DeleteBodyRows();
                    }
                }
                else
                {
                    table.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                DataRow rowAssessmentNarrativeSummary = null;
                if (AssessmentNarrativeSummary.Rows.Count > 0)
                {
                    rowAssessmentNarrativeSummary = AssessmentNarrativeSummary.Rows[0];
                }
                tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Section I")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("ASSESSMENT NARRATIVE SUMMARY")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("This section includes relevant personal history and appropriate contextual information, as well as skills, abilities, aspirations, needs, interests, reasonable accommodations, cultural considerations, meaningful activities, challenges, etc., learned during the person - centered planning process, record review and any assessments reviewed and / or completed.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });

                if (AssessmentNarrativeSummary.Rows.Count > 0)
                {
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Introducing Me :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["IntroducingMe"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Home :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyHome"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Let Me Tell You About My Day :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["TellYouAboutMyDay"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Health and My Medications :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyHealthAndMedication"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Relationship :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyRelationships"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Happiness :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyHappiness"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My School/ Learning :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MySchool"].ToString(), fntTableFont)) { PaddingBottom = 10 });



                }
                else
                {
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Introducing Me :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Home :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Let Me Tell You About My Day :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Health and My Medications :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Relationship :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Happiness :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My School/ Learning :")) { PaddingBottom = 10 });
                    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                    //tableAssessmentNarrativeSummary.AddCell("");
                }


                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Section II")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("OUTCOMES AND SUPPORT STRATEGIES")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("This section includes measurable/observable personal outcomes that are developed by the person and his/her IDT using person-centered planning. It describes provider goals and corresponding staff activities identified to meet the CCO goal / valued outcome.It captures the following information: goal description, valued outcomes, action steps, responsible party, service type, timeframe for action steps and Personal Outcome Measures.Evidence of achievement must be reflected in monthly notes from assigned providers.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("CQL POMS Goal/Valued OutCome", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("CCO Goal/Valued OutCome", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Provider Assigned Goal", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Provider / Location", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Service Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Frequency", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Quantity", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Time Frame", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Special Considerations", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                if (Outcomes_SupportStrategies.Rows.Count > 0)
                {
                    for (var i = 0; i < Outcomes_SupportStrategies.Rows.Count; i++)
                    {
                        DataRow rowOutcomes_SupportStrategies = Outcomes_SupportStrategies.Rows[i];
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["CqlPomsGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["CcoGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["ProviderAssignedGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["ProviderLocation"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["ServicesType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["Frequency"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["Quantity"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["TimeFrame"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["SpecialConsiderations"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    }
                }
                else
                {
                    tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 9, HorizontalAlignment = 1, PaddingBottom = 10 });
                }



                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Section III")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 8, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Individual Safeguards/Individual Plan of Protection (IPOP)")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 8, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Compilation of all supports and services needed for a person to remain safe, healthy and comfortable across all settings (including Part 686 requirements for IPOP).This section details the provider goals and corresponding staff activities required to maintain desired personal safety")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 8, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Goal Valued Outcome")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Provider Assigned Goal")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Provider/Location")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Service Type")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Frequency")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Quantity")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Time Frame")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Special Considerations")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                if (IndividualPlanOfProtection.Rows.Count > 0)
                {
                    for (var i = 0; i < IndividualPlanOfProtection.Rows.Count; i++)
                    {
                        DataRow rowIndividualPlanOfProtection = IndividualPlanOfProtection.Rows[i];
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["GoalValuedOutcome"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["ProviderAssignedGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["ProviderLocation"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["ServicesType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["Frequency"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["Quantity"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["TimeFrame"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["SpecialConsiderations"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                    }
                }
                else
                {
                    tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 8, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Section IV")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 5, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("HCBS Wavier and Medicaid State Plan Authorized Services")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 5, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("This section of the Life Plan includes a listing of all HCBS Waiver and State Plan services that have been authorized for the individual. ")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 5, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Authorized Service")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Provider")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Effective Dates")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Unit Of Measure")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Comments")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                if (MedicaidStatePlanAuthorizedServies.Rows.Count > 0)
                {
                    for (var i = 0; i < MedicaidStatePlanAuthorizedServies.Rows.Count; i++)
                    {
                        DataRow rowMedicaidStatePlanAuthorizedServies = MedicaidStatePlanAuthorizedServies.Rows[i];
                        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["AuthorizedService"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["Provider"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["EffectiveDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["UnitOfMeasure"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["Comments"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                    }
                }
                else
                {
                    tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 5, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Section V")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("All Suports and Services; Funded and Natural/Community Resources")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("This section identifies the services and support givers in a person’s life along with the needed contact information. Additionally, all Natural Supports and Community Resources that help the person be a valued individual of his or her community and live successfully on a day - to - day basis at home, at work, at school, or in other community locations should be listed with contact information as appropriate.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Contact Type")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Relationship")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Name")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Orginization")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                if (FundalNaturalCommunityResources.Rows.Count > 0)
                {
                    for (var i = 0; i < FundalNaturalCommunityResources.Rows.Count; i++)
                    {
                        DataRow rowFundalNaturalCommunityResources = FundalNaturalCommunityResources.Rows[i];
                        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["ContactType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["Relationship"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["Name"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["Orginization"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                    }
                }
                else
                {
                    tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                DataRow rowMemberRight = null;
                if (MemberRights.Rows.Count > 0)
                {
                    rowMemberRight = MemberRights.Rows[0];
                }
                //tableMemberRights.AddCell(new PdfPCell(new Phrase("Section VI")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableMemberRights.AddCell(new PdfPCell(new Phrase("Member Rights")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, PaddingBottom = 10 });
                tableMemberRights.AddCell(new PdfPCell(new Phrase("My Care Manager has informed me of:")) { Colspan = 9, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });




                if (MemberRights.Rows.Count > 0)
                {
                    tableMemberRights.AddCell(new PdfPCell(new Phrase("My rights under the Americans With Disabilities Act(ADA) : " + rowMemberRight["RightsUnderAmericansDisabilitiesAct"].ToString(), fntTableFont)) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                    tableMemberRights.AddCell(new PdfPCell(new Phrase("How to obtain reasonable accommodations (my reasonable accommodations are listed in my Life Plan) : " + rowMemberRight["Provider"].ToString(), fntTableFont)) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                    tableMemberRights.AddCell(new PdfPCell(new Phrase(" How to file a grievance or an appeal : " + rowMemberRight["GrievanceAppeal"].ToString(), fntTableFont)) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                }
                else
                {
                    tableMemberRights.AddCell(new PdfPCell(new Phrase("My rights under the Americans With Disabilities Act(ADA) : ")) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                    tableMemberRights.AddCell(new PdfPCell(new Phrase("How to obtain reasonable accommodations (my reasonable accommodations are listed in my Life Plan) :")) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                    tableMemberRights.AddCell(new PdfPCell(new Phrase("How to file a grievance or an appeal : ")) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                }



                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Section VII")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Member Representative Approval")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Member Name")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Member Approval Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Representative")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Representative Approval Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Committee Approver")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Committee Approval Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                if (MemberRepresentativeApproval.Rows.Count > 0)
                {
                    for (var i = 0; i < MemberRepresentativeApproval.Rows.Count; i++)
                    {
                        DataRow rowMemberRepresentativeApproval = MemberRepresentativeApproval.Rows[i];
                        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["MemberName"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["MemberApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["Representative"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["RepresentativeApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["CommitteeApprover"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["CommitteeApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                    }
                }
                else
                {
                    tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Section VI")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Acknowledgement and Agreements")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("This section includes measurable/observable personal outcomes that are developed by the person and his/her IDT using person-centered planning. It describes provider goals and corresponding staff activities identified to meet the CCO goal / valued outcome.It captures the following information: goal description, valued outcomes, action steps, responsible party, service type, timeframe for action steps and Personal Outcome Measures.Evidence of achievement must be reflected in monthly notes from assigned providers.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Notification Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Provider", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Notification Reason", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Notification Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Acknowledge and Agree Status", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Aceptance / Acknowledgement Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                if (AcknowledgementAndAgreement.Rows.Count > 0)
                {
                    for (var i = 0; i < AcknowledgementAndAgreement.Rows.Count; i++)
                    {
                        DataRow rowAcknowledgementAndAgreement = AcknowledgementAndAgreement.Rows[i];
                        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["Provider"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationReason"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationAckAgreeStatus"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["AceptanceAcknowledgementDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    }
                }
                else
                {
                    tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                //tableDocuments.AddCell(new PdfPCell(new Phrase("Section IX")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 2, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                tableDocuments.AddCell(new PdfPCell(new Phrase("Documents")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 2, HorizontalAlignment = 1, PaddingBottom = 10 });
                tableDocuments.AddCell(new PdfPCell(new Phrase("Document Title", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                tableDocuments.AddCell(new PdfPCell(new Phrase("Attach Document", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                if (Documents.Rows.Count > 0)
                {
                    for (var i = 0; i < Documents.Rows.Count; i++)
                    {
                        DataRow rowDocument = Documents.Rows[i];
                        tableDocuments.AddCell(new PdfPCell(new Phrase(rowDocument["MemberName"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                        tableDocuments.AddCell(new PdfPCell(new Phrase(rowDocument["MemberApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                    }
                }
                else
                {
                    tableDocuments.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 2, HorizontalAlignment = 1, PaddingBottom = 10 });
                }


                pdfStamper.FormFlattening = false;
                pdfStamper.Dispose();
                // close the pdf
                pdfStamper.Close();

                var pagesize = new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4);
                //Set Background color of pdf    
                pagesize.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                var left_margin = 15;
                var top_margin = 25;
                var bottom_margin = 25;


                // create a iTextSharp.text.Document object:    
                iTextSharp.text.Document doc = new iTextSharp.text.Document(pagesize, left_margin, 10, top_margin, bottom_margin);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(newFile1, FileMode.Create));
                iTextSharp.text.Font mainFont = new iTextSharp.text.Font();
                iTextSharp.text.Font boldFont = new iTextSharp.text.Font();
                mainFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
                boldFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
                doc.Open();

                table.SpacingAfter = 30;
                tableAssessmentNarrativeSummary.SpacingAfter = 30;
                tableFundalNaturalCommunityResources.SpacingAfter = 30;
                tableIndividualPlanOfProtection.SpacingAfter = 30;
                tableMedicaidStatePlanAuthorizedServies.SpacingAfter = 30;
                tableOutcomes_SupportStrategies.SpacingAfter = 30;
                tableMemberRepresentativeApproval.SpacingAfter = 30;
                tableAcknowledgementAndAgreement.SpacingAfter = 30;
                tableDocuments.SpacingAfter = 30;
                tableMemberRights.SpacingAfter = 30;






                doc.Add(table);
                doc.Add(tableMeetingAttendance);
                doc.Add(tableAssessmentNarrativeSummary);
                doc.Add(tableOutcomes_SupportStrategies);
                doc.Add(tableIndividualPlanOfProtection);
                doc.Add(tableMedicaidStatePlanAuthorizedServies);
                doc.Add(tableFundalNaturalCommunityResources);
                doc.Add(tableMemberRights);
                doc.Add(tableMemberRepresentativeApproval);
                doc.Add(tableAcknowledgementAndAgreement);
                doc.Add(tableDocuments);
                doc.Close();

                FileStream fs = new FileStream(finaldoc, FileMode.Create);
                iTextSharp.text.pdf.PdfReader readerNewFile = new iTextSharp.text.pdf.PdfReader(newFile);
                iTextSharp.text.pdf.PdfReader readerNewFile1 = new iTextSharp.text.pdf.PdfReader(newFile1);

                using (Document document = new Document())
                using (PdfCopy copy = new PdfCopy(document, fs))
                {
                    document.Open();
                    copy.AddDocument(readerNewFile);
                    copy.AddDocument(readerNewFile1);
                    document.Close();
                    readerNewFile.Dispose();
                    readerNewFile.Dispose();
                    readerNewFile1.Close();
                    readerNewFile1.Close();
                    fs.Dispose();
                    fs.Close();

                }
                // for paging and date stamp
                FileStream fs1 = new FileStream(finaldoc1, FileMode.Create);
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(newFile);
                iTextSharp.text.pdf.PdfReader fsreader1 = new iTextSharp.text.pdf.PdfReader(newFile1);

                using (Document document1 = new Document())
                using (PdfCopy copy1 = new PdfCopy(document1, fs1))
                {
                    document1.Open();
                    copy1.AddDocument(reader);
                    copy1.AddDocument(fsreader1);
                    document1.Close();
                    reader.Dispose();
                    fsreader1.Dispose();
                    reader.Close();
                    fsreader1.Close();
                    fs1.Dispose();
                    fs1.Close();
                }
                iTextSharp.text.pdf.PdfReader pdfReader1 = new iTextSharp.text.pdf.PdfReader(finaldoc1);
                PdfStamper pdfStamper1 = new PdfStamper(pdfReader1, new FileStream(finaldoc, FileMode.Create));
                for (int i = 1; i <= pdfReader1.NumberOfPages; i++)
                {
                    ColumnText.ShowTextAligned(pdfStamper1.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + pdfReader1.NumberOfPages), 568f, 15f, 0);
                    ColumnText.ShowTextAligned(pdfStamper1.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), pageTextFont), 2f, 15f, 0);
                }
                pdfStamper1.FormFlattening = false;
                pdfStamper1.Dispose();
                // close the pdf

                pdfStamper1.Close();
                pdfReader1.Dispose();
                pdfReader1.Close();



                if (tabName == "PublishLifePlanVersion")
                {
                    dataTable = UploadPublishedPDFDocument(finaldoc, dataSetFillPDF.Tables[0], fillablePDFRequest);
                }
                else
                {
                    dataTable.Clear();
                    dataTable.Columns.Add("FileName");
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["FileName"] = finaldoc;
                    dataTable.Rows.Add(dataRow);
                }
                pdfStamper.Dispose();
                pdfStamper.Close();

                return dataTable;
            }
            catch (Exception Ex)
            {
                pdfStamper.Dispose();
                pdfStamper.Close();
                throw Ex;
            }

        }
        private DataTable UploadPublishedPDFDocument(string finaldoc, DataTable dataTableLIfePlan, FillableLifePlanPDFRequest fillablePDFRequest)
        {
            PdfSharp.Pdf.PdfDocument originalDocument = PdfSharp.Pdf.IO.PdfReader.Open(finaldoc, PdfDocumentOpenMode.Import);
            PdfSharp.Pdf.PdfDocument outputPdf = new PdfSharp.Pdf.PdfDocument();

            DataRow dataRowLifePlan = dataTableLIfePlan.Rows[0];
            string documentUplpoadPath = string.Empty;
            string fileNamewithExtension = string.Empty;
            string fullDocumentPath = string.Empty;
            string encryptedFolderName = string.Empty;
            string uploadFilePath = string.Empty;
            DataTable dataTablePublishedDocument = null;
            try
            {
                foreach (PdfSharp.Pdf.PdfPage page in originalDocument.Pages)
                {
                    outputPdf.AddPage(page);
                }

                fileNamewithExtension = "PublishedPDF_" + dataRowLifePlan["DocumentVersion"] + "_" + fillablePDFRequest.IndividualName.Trim().Replace(",", "_").ToString().Replace(" ", string.Empty) + ".pdf";
                //documentUplpoadPath = "C:\\Bitbucket\\cx360.api\\IncidentManagement.API\\Upload\\";
                documentUplpoadPath = fillablePDFRequest.FilePath;
                DirectoryInfo di = new DirectoryInfo(documentUplpoadPath);
                di = new DirectoryInfo(documentUplpoadPath);
                if (!string.IsNullOrEmpty(documentUplpoadPath))
                {
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    documentUplpoadPath = documentUplpoadPath + @"\" + "Documents";
                    di = new DirectoryInfo(documentUplpoadPath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                //Folder Name  encryption
                //encryptedFolderName = CommonFunctions.GetEncodedFolderName("Client" + dataRowLifePlan["ClientId"]);
                //encryptedFolderName = encryptedFolderName.Replace(@"\", "").Replace(@"/", "");
                //encryptedFolderName = documentUplpoadPath + @"\" + ;
                di = new DirectoryInfo(documentUplpoadPath + @"\" + "Client" + dataRowLifePlan["ClientId"]);

                if (!di.Exists)
                {
                    di.Create();
                }
                DateTime currentUTC = DateTime.UtcNow;
                string strTemp = currentUTC.ToString("yyyyMMddHHmmss");
                uploadFilePath = di.FullName + @"\" + strTemp + "~" + fileNamewithExtension;


                if (!string.IsNullOrEmpty(documentUplpoadPath))
                {
                    //fullDocumentPath = encryptedFolderName + @"\" + strTemp + "~" + fileNamewithExtension;
                    //if (di.Exists)
                    //{

                    MemoryStream stream = new MemoryStream();
                    outputPdf.Save(uploadFilePath);
                    //}
                }
                else
                {
                    fullDocumentPath = strTemp + "~" + fileNamewithExtension;
                }



                dataTablePublishedDocument = new DataTable();
                dataTablePublishedDocument.Clear();
                dataTablePublishedDocument.Columns.Add("Documentname");
                dataTablePublishedDocument.Columns.Add("DocumentFileName");
                dataTablePublishedDocument.Columns.Add("ClientId");
                DataRow dataRow = dataTablePublishedDocument.NewRow();
                dataRow["Documentname"] = strTemp + "~" + fileNamewithExtension;
                dataRow["DocumentFileName"] = uploadFilePath;
                dataRow["ClientId"] = dataRowLifePlan["ClientId"];
                dataTablePublishedDocument.Rows.Add(dataRow);
            }

            catch (Exception Ex)
            {
                throw Ex;
            }

            return dataTablePublishedDocument;
        }

        public async Task<LifePlanDetailTabResponse> HandleLifePlanVersioning(FillableLifePlanPDFRequest lpdRequest)
        {
            lpdResponse = new LifePlanDetailTabResponse();
            string tabName = lpdRequest.TabName;
            string sp = CommonFunctions.GetMappedStoreProcedure(tabName);
            DataSet dataSet = new DataSet();
            DataTable dataTablePDFPath = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@documentversionid", SqlDbType.VarChar).Value = lpdRequest.DocumentVersionId;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = lpdRequest.ReportedBy;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;

                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;

                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));

                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    //if (tabName == "PublishLifePlanVersion")
                    //{
                    //    dataTablePDFPath = GetLifePlanPDFTemplate(tabName, ConfigurationManager.AppSettings["FillablePDF"].ToString() + "Lifeplan.pdf", lpdRequest);
                    //    string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTablePDFPath);
                    //    lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetString);
                    //}
                    //else
                    //{
                        string dataSetString = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                        lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetString);
                    //}
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return lpdResponse;
        }

        private bool ValidateLifePlanDraft(LifePlanDetailRequest lpdRequest)
        {
            bool recordValidated = true;

            DataTable dataSet = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ValidateLifePlanDraft", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = lpdRequest.Json;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = lpdRequest.ReportedBy;
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


        public async Task<LifePlanDetailTabResponse> GetMasterAuditRecords(LifePlanDetailRequest lpdRequest)
        {
            lpdResponse = new LifePlanDetailTabResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetMasterAuditRecord", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@documentversionid", SqlDbType.VarChar).Value = lpdRequest.DocumentVersionId;
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
                    lpdResponse.LifPlanDetailsData = JsonConvert.DeserializeObject<List<LifPlanDetailsData>>(dataSetString);

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return lpdResponse;
        }
        public async Task<LifePlanDetailTabResponse> GetChildAuditRecords(LifePlanDetailRequest lpdRequest)
        {
            lpdResponse = new LifePlanDetailTabResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetAuditChildRecords", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@currentlifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@previouslifeplanid", SqlDbType.VarChar).Value = lpdRequest.PreviousLifePlanId;
                        cmd.Parameters.Add("@section", SqlDbType.VarChar).Value = lpdRequest.TabName;
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
                    lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetString);

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return lpdResponse;
        }
        public async Task<LifePlanExportedRecordsResponse> LifePlanEXportedRecords(LifePlanDetailRequest lpdRequest)        {            lifePlanExportedRecordsResponse = new LifePlanExportedRecordsResponse();            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;            DataSet dataSet = new DataSet();            try            {                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))                {                    using (SqlCommand cmd = new SqlCommand("usp_GetLifePlanExportedRecords", con))                    {                        cmd.CommandType = System.Data.CommandType.StoredProcedure;                        cmd.Parameters.Add("@tabname", SqlDbType.VarChar).Value = lpdRequest.TabName;                        cmd.Parameters.Add("@clientid", SqlDbType.VarChar).Value = lpdRequest.ClientId;                        con.Open();                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();                        sqlDataAdapter.SelectCommand = cmd;                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));                        con.Close();                    }                }                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)                {                    if (lpdRequest.TabName == "GetSuggestedOutcomesSupportStrategies")                    {                        string dataSetStringSuggestedOutcomes = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);                        lifePlanExportedRecordsResponse.suggestedOutcomesSupportStrategiesTab = JsonConvert.DeserializeObject<List<SuggestedOutcomesSupportStrategiesTab>>(dataSetStringSuggestedOutcomes);                    }                    if (lpdRequest.TabName == "GetNoteMeetingHistory")                    {                        string dataSetStringNoteMeetingHistory = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);                        lifePlanExportedRecordsResponse.MeetingHistorySummaryTab = JsonConvert.DeserializeObject<List<MeetingHistorySummaryTab>>(dataSetStringNoteMeetingHistory);                    }                }            }            catch (Exception Ex)            {                throw Ex;            }            return lifePlanExportedRecordsResponse;        }
        public async Task<LifePlanDetailTabResponse> InsertModifysubmissionForm(SubmissionFormRequest submissionFormRequest)        {
            lpdResponse = new LifePlanDetailTabResponse();            DataSet dataSet = new DataSet();            try            {                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))                {                    using (SqlCommand cmd = new SqlCommand("usp_InsertModifySubmissionFormData", con))                    {                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@submissiondecisionformid", SqlDbType.Int).Value = submissionFormRequest.UD_SubmissionDecisionFormID;                        cmd.Parameters.Add("@formname", SqlDbType.VarChar).Value = submissionFormRequest.FormName;                        cmd.Parameters.Add("@status", SqlDbType.Int).Value = submissionFormRequest.Status;                        cmd.Parameters.Add("@clientid", SqlDbType.Int).Value = submissionFormRequest.ClientID;                        cmd.Parameters.Add("@keyfieldid", SqlDbType.Int).Value = submissionFormRequest.KeyFieldID;                        cmd.Parameters.Add("@submittedto", SqlDbType.Int).Value = submissionFormRequest.SubmittedTo;                        cmd.Parameters.Add("@submissionMessage", SqlDbType.VarChar).Value = submissionFormRequest.SubmissionMessage;                        cmd.Parameters.Add("@electronicsignature", SqlDbType.VarChar).Value = submissionFormRequest.ElectronicSignature;                        cmd.Parameters.Add("@electronicsignature_signedon", SqlDbType.DateTime).Value = submissionFormRequest.ElectronicSignature_SignedOn;                        cmd.Parameters.Add("@submittedon", SqlDbType.DateTime).Value = submissionFormRequest.SubmittedOn;                        cmd.Parameters.Add("@stafftitle", SqlDbType.VarChar).Value = submissionFormRequest.StaffTitle;
                        cmd.Parameters.Add("@staffname", SqlDbType.VarChar).Value = submissionFormRequest.StaffName;                        cmd.Parameters.Add("@tabname", SqlDbType.VarChar).Value = submissionFormRequest.TabName;                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = submissionFormRequest.ReportedBy;                        con.Open();                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();                        sqlDataAdapter.SelectCommand = cmd;                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));                        con.Close();                    }                }                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)                {                        string dataSetStringSubmissionForm = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    lpdResponse.AllTab = JsonConvert.DeserializeObject<List<AllTab>>(dataSetStringSubmissionForm);                }            }            catch (Exception Ex)            {                throw Ex;            }            return lpdResponse;        }

        public async Task<MemberRepresentativeResponse> HandleMemberRepresentative(LifePlanDetailRequest lpdRequest)
        {
            MemberRepresentativeResponse = new MemberRepresentativeResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    MemberRepresentativeResponse.MemberRepresentativeTab = JsonConvert.DeserializeObject<List<MemberRepresentativeTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return MemberRepresentativeResponse;

        }

        public async Task<MemberRightResponse> HandleMemberRight(LifePlanDetailRequest lpdRequest)
        {
            MemberRightResponse = new MemberRightResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(lpdRequest.TabName);
            string resultString = Regex.Match(lpdRequest.Json, @"\d+").Value;
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@lifeplanid", SqlDbType.VarChar).Value = lpdRequest.LifePlanId;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = lpdRequest.Mode;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(resultString);
                        cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar).Value = lpdRequest.ReportedBy;
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
                    MemberRightResponse.MemberRightTab = JsonConvert.DeserializeObject<List<MemberRightTab>>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return MemberRightResponse;

        }

    }
}
