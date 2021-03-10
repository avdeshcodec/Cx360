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
using static IncidentManagement.Entities.Response.ComprehensiveAssessmentResponse;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using System.Web;
using System.Linq;

namespace IncidentManagement.Repository.Repository
{
  public  class ComprehensiveAssessmentRepository:IComprehensiveAssessmentRepository
    {
        #region Private
        ComprehensiveAssessmentDetailResponse comprehensiveAssessmentDetailResponse = null;
        ComprehensiveAssessmentPDFResponse comprehensiveAssessmentPDFResponse = null;
        #endregion

        public async Task<ComprehensiveAssessmentDetailResponse> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {

            bool checkVersion = true;

            if (comprehensiveAssessmentRequest.TabName == "ComprehensiveAssessment")
            {
                checkVersion = ValidateComprehensiveAssessmentDraft(comprehensiveAssessmentRequest);
            }
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
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
                            cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.Json;
                            cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = comprehensiveAssessmentRequest.ReportedBy;
                            if (!string.IsNullOrEmpty(comprehensiveAssessmentRequest.JsonChildFirstTable) &&(comprehensiveAssessmentRequest.JsonChildFirstTable != "null"))
                            {
                                cmd.Parameters.Add("@jsonchildfirsttable", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.JsonChildFirstTable;
                            }
                            if (!string.IsNullOrEmpty(comprehensiveAssessmentRequest.JsonChildSecondTable) && (comprehensiveAssessmentRequest.JsonChildFirstTable != "null"))
                            {
                                cmd.Parameters.Add("@jsonchildsecondchild", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.JsonChildSecondTable;
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
                        comprehensiveAssessmentDetailResponse.AllTabsComprehensiveAssessment = JsonConvert.DeserializeObject<List<AllTabsComprehensiveAssessment>>(dataSetString);
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
                    comprehensiveAssessmentDetailResponse.AllTabsComprehensiveAssessment = JsonConvert.DeserializeObject<List<AllTabsComprehensiveAssessment>>(dataSetString);
                    return comprehensiveAssessmentDetailResponse;
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return comprehensiveAssessmentDetailResponse;

        }


        public bool ValidateComprehensiveAssessmentDraft(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            bool recordValidated = true;

            DataTable dataSet = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ValidateComprehensiveAssessmentDraft", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.Json;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = comprehensiveAssessmentRequest.ReportedBy;
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

        public async Task<ComprehensiveAssessmentDetailResponse> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@compassessmentid", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.ComprehensiveAssessmentId;
                        cmd.Parameters.Add("@assessmentversionid", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.AssessmentVersioningId;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = comprehensiveAssessmentRequest.ReportedBy;

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
                    comprehensiveAssessmentDetailResponse.AllTabsComprehensiveAssessment = JsonConvert.DeserializeObject<List<AllTabsComprehensiveAssessment>>(dataSetString);
                    if(comprehensiveAssessmentRequest.TabName != "PublishAssessment")
                    {
                        string MedicalDiagnosis = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[1]);
                        string MedicalMedications = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[2]);
                        string MedicalHealthMedications = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[3]);
                        string FinancialMemberStatus = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[4]);
                        string FinancialMemberNeeds = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[5]);
                        string HousingSubsidies = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[6]);

                        string DomesticViolanceMemberRelationship = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[7]);



                        string LegalCourtDates = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[8]);

                        comprehensiveAssessmentDetailResponse.MedicalHealthMedicationsDetails = JsonConvert.DeserializeObject<List<MedicalHealthMedicationsDetails>>(MedicalHealthMedications);
                        comprehensiveAssessmentDetailResponse.MedicalMedicationDetails = JsonConvert.DeserializeObject<List<MedicalMedicationDetails>>(MedicalMedications);
                        comprehensiveAssessmentDetailResponse.MedicalDiagnosisDetails = JsonConvert.DeserializeObject<List<MedicalDiagnosisDetails>>(MedicalDiagnosis);
                        comprehensiveAssessmentDetailResponse.DomesticViolanceMemberRelationshipDetails = JsonConvert.DeserializeObject<List<DomesticViolanceMemberRelationshipDetails>>(DomesticViolanceMemberRelationship);
                        comprehensiveAssessmentDetailResponse.FinancialMemberNeedDetails = JsonConvert.DeserializeObject<List<FinancialMemberNeedDetails>>(FinancialMemberNeeds);
                        comprehensiveAssessmentDetailResponse.FinancialMemberStatusDetails = JsonConvert.DeserializeObject<List<FinancialMemberStatusDetails>>(FinancialMemberStatus);
                        comprehensiveAssessmentDetailResponse.HousingSubsidyDetails = JsonConvert.DeserializeObject<List<HousingSubsidyDetails>>(HousingSubsidies);
                        comprehensiveAssessmentDetailResponse.LegalCourtDateDetails = JsonConvert.DeserializeObject<List<LegalCourtDateDetails>>(LegalCourtDates);
                    }
                    
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return comprehensiveAssessmentDetailResponse;
        }
        public async Task<ComprehensiveAssessmentDetailResponse> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@comprehensiveAssessmentId", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.ComprehensiveAssessmentId;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0)
                {
                    string AssessmentAreasSafeguardReview = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[0]);
                    string AssessmentBehavioralSupportServices = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[1]);
                    string AssessmentDepressionScreening = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[2]);
                    string AssessmentDomesticViolance = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[3]);
                    string AssessmentEducationalVocationalStatus = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[4]);
                    string AssessmentFinancial = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[5]);
                    string AssessmentGeneral = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[6]);
                    string AssessmentHousing = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[7]);
                    string AssessmentIndependentLivingSkills = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[8]);
                    string AssessmentLegal = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[9]);
                    string AssessmentMedical = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[10]);
                    string AssessmentMedicalHealth = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[11]);
                    string AssessmentSafetyPlan = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[12]);
                    string AssessmentSafetyRisk = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[13]);
                    string AssessmentSelfDirectedServices = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[14]);
                    string ComprehensiveAssessment = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[15]);
                    string AssessmentTransitionPlanning = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[16]);
                    string AssessmentSubstanceAbuseScreening = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[17]);
                    string MedicalHealthMedications = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[18]);
                    string MedicalMedications = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[19]);
                    string MedicalDiagnosis = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[20]);
                    string DomesticViolanceMemberRelationship = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[21]);
                    string FinancialMemberNeeds = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[22]);
                    string FinancialMemberStatus = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[23]);
                    string HousingSubsidies = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[24]);
                    string LegalCourtDates = CommonFunctions.ConvertDataTableToJson(dataSet.Tables[25]);
                    comprehensiveAssessmentDetailResponse.AreasSafeguardReviewDetails = JsonConvert.DeserializeObject<List<AreasSafeguardReviewDetails>>(AssessmentAreasSafeguardReview);
                    comprehensiveAssessmentDetailResponse.BehavioralSupportServicesDetails = JsonConvert.DeserializeObject<List<BehavioralSupportServicesDetails>>(AssessmentBehavioralSupportServices);
                    comprehensiveAssessmentDetailResponse.DepressionScreeningDetails = JsonConvert.DeserializeObject<List<DepressionScreeningDetails>>(AssessmentDepressionScreening);
                    comprehensiveAssessmentDetailResponse.DomesticViolanceDetails = JsonConvert.DeserializeObject<List<DomesticViolanceDetails>>(AssessmentDomesticViolance);
                    comprehensiveAssessmentDetailResponse.EducationalVocationalStatusDetails = JsonConvert.DeserializeObject<List<EducationalVocationalStatusDetails>>(AssessmentEducationalVocationalStatus);
                    comprehensiveAssessmentDetailResponse.FinancialDetails = JsonConvert.DeserializeObject<List<FinancialDetails>>(AssessmentFinancial);
                    comprehensiveAssessmentDetailResponse.GeneralDetails = JsonConvert.DeserializeObject<List<GeneralDetails>>(AssessmentGeneral);
                    comprehensiveAssessmentDetailResponse.HousingDetails = JsonConvert.DeserializeObject<List<HousingDetails>>(AssessmentHousing);
                    comprehensiveAssessmentDetailResponse.IndependentLivingSkillsDetails = JsonConvert.DeserializeObject<List<IndependentLivingSkillsDetails>>(AssessmentIndependentLivingSkills);
                    comprehensiveAssessmentDetailResponse.LegalDetails = JsonConvert.DeserializeObject<List<LegalDetails>>(AssessmentLegal);
                    comprehensiveAssessmentDetailResponse.MedicalDetails = JsonConvert.DeserializeObject<List<MedicalDetails>>(AssessmentMedical);
                    comprehensiveAssessmentDetailResponse.MedicalHelathDetails = JsonConvert.DeserializeObject<List<MedicalHelathDetails>>(AssessmentMedicalHealth);
                    comprehensiveAssessmentDetailResponse.SafetyPlanDetails = JsonConvert.DeserializeObject<List<SafetyPlanDetails>>(AssessmentSafetyPlan);
                    comprehensiveAssessmentDetailResponse.SafetyRiskDetails = JsonConvert.DeserializeObject<List<SafetyRiskDetails>>(AssessmentSafetyRisk);
                    comprehensiveAssessmentDetailResponse.SelfDirectedServicesDetails = JsonConvert.DeserializeObject<List<SelfDirectedServicesDetails>>(AssessmentSelfDirectedServices);
                    comprehensiveAssessmentDetailResponse.ComprehensiveAssessmentDetails = JsonConvert.DeserializeObject<List<ComprehensiveAssessmentDetails>>(ComprehensiveAssessment);
                    comprehensiveAssessmentDetailResponse.TransitionPlanningDetails = JsonConvert.DeserializeObject<List<TransitionPlanningDetails>>(AssessmentTransitionPlanning);
                    comprehensiveAssessmentDetailResponse.SubstanceAbuseScreeningDetails = JsonConvert.DeserializeObject<List<SubstanceAbuseScreeningDetails>>(AssessmentSubstanceAbuseScreening);
                    comprehensiveAssessmentDetailResponse.MedicalHealthMedicationsDetails = JsonConvert.DeserializeObject<List<MedicalHealthMedicationsDetails>>(MedicalHealthMedications);
                    comprehensiveAssessmentDetailResponse.MedicalMedicationDetails = JsonConvert.DeserializeObject<List<MedicalMedicationDetails>>(MedicalMedications);
                    comprehensiveAssessmentDetailResponse.MedicalDiagnosisDetails = JsonConvert.DeserializeObject<List<MedicalDiagnosisDetails>>(MedicalDiagnosis);
                    comprehensiveAssessmentDetailResponse.DomesticViolanceMemberRelationshipDetails = JsonConvert.DeserializeObject<List<DomesticViolanceMemberRelationshipDetails>>(DomesticViolanceMemberRelationship);
                    comprehensiveAssessmentDetailResponse.FinancialMemberNeedDetails = JsonConvert.DeserializeObject<List<FinancialMemberNeedDetails>>(FinancialMemberNeeds);
                    comprehensiveAssessmentDetailResponse.FinancialMemberStatusDetails = JsonConvert.DeserializeObject<List<FinancialMemberStatusDetails>>(FinancialMemberStatus);
                    comprehensiveAssessmentDetailResponse.HousingSubsidyDetails = JsonConvert.DeserializeObject<List<HousingSubsidyDetails>>(HousingSubsidies);
                    comprehensiveAssessmentDetailResponse.LegalCourtDateDetails = JsonConvert.DeserializeObject<List<LegalCourtDateDetails>>(LegalCourtDates);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return comprehensiveAssessmentDetailResponse;
        }

        public async Task<ComprehensiveAssessmentPDFResponse> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            comprehensiveAssessmentPDFResponse = new ComprehensiveAssessmentPDFResponse();
            string pdfTemplate = CommonFunctions.GetFillablePDFPath(comprehensiveAssessmentRequest.TabName);
            string newTemplatePDf = string.Empty;
            try
            {
                newTemplatePDf = GetAssessmentPDFTemplate(comprehensiveAssessmentRequest.TabName, ConfigurationManager.AppSettings["FillablePDF"].ToString() + "Comprehensive_Assessment.pdf", comprehensiveAssessmentRequest);

                DataTable dataTable = new DataTable();  
                dataTable.Clear();
                dataTable.Columns.Add("FileName");
                DataRow dataRow = dataTable.NewRow();
                dataRow["FileName"] = newTemplatePDf;
                dataTable.Rows.Add(dataRow);
                string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTable);
                comprehensiveAssessmentPDFResponse.ComprehensiveAssessmentPDF = JsonConvert.DeserializeObject<List<ComprehensiveAssessmentPDF>>(dataSetString);
                return comprehensiveAssessmentPDFResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private string GetAssessmentPDFTemplate(string tabName, string pdfPath, ComprehensiveAssessmentRequest fillablePDFRequest)
        {
            DataSet dataSet = new DataSet();
            string newpdfPath = string.Empty;
            try
            {
                string storeProcedure = CommonFunctions.GetMappedStoreProcedure(tabName);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetComprehensiveAssessmentDetails", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add("@comprehensiveAssessmentId", SqlDbType.Int).Value = fillablePDFRequest.ComprehensiveAssessmentId;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[15].Rows.Count > 0)
                {
                   
                            newpdfPath = ComprehensiveAssessmentDFTemplate(pdfPath, dataSet, fillablePDFRequest);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return newpdfPath;
        }
        public string ComprehensiveAssessmentDFTemplate(string pdfPath, DataSet dataSetFillPDF, ComprehensiveAssessmentRequest fillablePDFRequest)
        {
            string newFile = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "Completed_Comprehensive_Assessment.pdf";
            string NewComprehensiveDocument = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "NewComprehensiveDocument.pdf";
            string newFileAddMedication = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "newFileAddMedication.pdf";
            string newFileAddFinancial = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "newFinancialTableMerge.pdf";
            string newFileAddHousingSubsidies = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "newHousingSubsidiesTableMerge.pdf";
            string newFileAddDomesticViolence = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "newFileAddDomesticViolenceMerge.pdf";
            string newFileAddLegal = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "newFileAddLegalMerge.pdf";
            string newFileAddHealthMedication = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "newFileAddHealthMedication.pdf";

            try
            {
                PdfReader pdfReader = new PdfReader(pdfPath);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;
                DataTable dataTableComprehensiveAssessment = dataSetFillPDF.Tables[15];
                pdfFormFields.GenerateAppearances = false;

                string status = string.Empty;


                if (dataTableComprehensiveAssessment.Rows.Count > 0)
                {
                    DataRow row = dataTableComprehensiveAssessment.Rows[0];
                    pdfFormFields.SetField("IndividualName", row["ClientName"].ToString());
                    pdfFormFields.SetField("DateOfBirth", row["DateOfBirth"].ToString());
                    pdfFormFields.SetField("Age", row["Age"].ToString());
                    pdfFormFields.SetField("Gender", row["Gender"].ToString());
                    pdfFormFields.SetField("ClientAddress", row["AddressLine1"].ToString()+" "+ row["AddressLine2"].ToString()+" "+ row["City"].ToString()+" "+ row["State"].ToString()+" "+ row["ZipCode"].ToString());
                    pdfFormFields.SetField("AddressLine2", row["AddressLine2"].ToString());
                    pdfFormFields.SetField("City", row["City"].ToString());
                    pdfFormFields.SetField("State", row["State"].ToString());
                    pdfFormFields.SetField("ZipCode", row["ZipCode"].ToString());
                    pdfFormFields.SetField("PhoneNumber", row["PhoneNumber"].ToString());
                    pdfFormFields.SetField("DateOfAssessment", row["DateOfAssessment"].ToString());
                    pdfFormFields.SetField("RelationshipStatus", row["RelationshipStatus"].ToString());
                    pdfFormFields.SetField("SexualOrientation", row["SexualOrientation"].ToString());
                    pdfFormFields.SetField("Ethnicity", row["Ethnicity"].ToString());
                    pdfFormFields.SetField("Race", row["Race"].ToString());
                    pdfFormFields.SetField("LanguagesSpoken", row["LanguagesSpoken"].ToString());
                    pdfFormFields.SetField("Reading", row["Reading"].ToString());
                    pdfFormFields.SetField("Writing", row["Writing"].ToString());
                    pdfFormFields.SetField("Medicaid_Seq", row["Medicaid_Seq"].ToString());
                    pdfFormFields.SetField("MCO", row["MCO"].ToString());
                    pdfFormFields.SetField("Verified", row["Verified"].ToString());
                    pdfFormFields.SetField("SS", row["SS"].ToString());
                    pdfFormFields.SetField("ReachedBy", row["ReachedBy"].ToString());
                    pdfFormFields.SetField("DocumentVersion", row["DocumentVersion"].ToString());
                    pdfFormFields.SetField("DocumentStatus", row["DocumentStatus"].ToString());

                    status = row["DocumentStatus"].ToString();

                    fillMedicalSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    fillMentalHealthSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    fillFinancialSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    fillHousingSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    fillDomesticViolancePDF(pdfFormFields, dataSetFillPDF, status);
                    fillLegalSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    fillSafeguardReviewPDF(pdfFormFields, dataSetFillPDF, status);
                    fillLivingSkillsPDF(pdfFormFields, dataSetFillPDF, status);
                    fillBehavioralSupportPDF(pdfFormFields, dataSetFillPDF, status);
                    fillVocationalStatusPDF(pdfFormFields, dataSetFillPDF, status);
                    fillSelfDirectedServicesPDF(pdfFormFields, dataSetFillPDF, status);
                    fillTransitionPlanningPDF(pdfFormFields, dataSetFillPDF, status);
                    fillGeneralSectionPDF(pdfFormFields, dataSetFillPDF, status);
                    fillSafetyRiskAssessmentPDF(pdfFormFields, dataSetFillPDF, status);
                    fillDepressionScreeningPDF(pdfFormFields, dataSetFillPDF, status);
                    fillSubstanceAbuseScreeningPDF(pdfFormFields, dataSetFillPDF, status);
                    fillSafetyPlanPDF(pdfFormFields, dataSetFillPDF, status);


                }
                



                pdfStamper.FormFlattening = false;
                pdfStamper.Dispose();
                // close the pdf
                pdfStamper.Close();
                pdfReader.Dispose();
                pdfReader.Close();
                ///////////////////////////////////
                ///

                DataTable dataTsbleMedicalDiagnosis = null;
                DataTable dataTsbleMedicalMedications = null;
                PdfPTable tableMedications = new PdfPTable(1);
                PdfPTable tableMedicalDiagonsis = new PdfPTable(3);
                tableMedications.WidthPercentage = 100f;
                tableMedicalDiagonsis.WidthPercentage = 100f;
                //     dataTableMedical = dataSetFillPDF.Tables[10];

                iTextSharp.text.Font fntTableFontHdr = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fntTableFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                // iTextSharp.text.Font pageTextFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                //create a document object
                var doc = new Document(PageSize.LETTER);
                PdfReader reader = new PdfReader(newFile);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(NewComprehensiveDocument, FileMode.Create));
                doc.Open();
                int pageNum = reader.NumberOfPages;

                tableMedicalDiagonsis.AddCell(new PdfPCell(new Phrase("Condition", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalDiagonsis.AddCell(new PdfPCell(new Phrase("Last Test Result", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMedicalDiagonsis.AddCell(new PdfPCell(new Phrase("Result Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowDiagonsis = null;
                dataTsbleMedicalDiagnosis = dataSetFillPDF.Tables[20];
                if (dataSetFillPDF.Tables[20].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTsbleMedicalDiagnosis.Rows.Count; i++)
                    {
                        rowDiagonsis = dataTsbleMedicalDiagnosis.Rows[i];
                        tableMedicalDiagonsis.AddCell(new Phrase(rowDiagonsis["ConditionText"].ToString(), fntTableFont));
                        tableMedicalDiagonsis.AddCell(new Phrase(rowDiagonsis["LatestResult"].ToString(), fntTableFont));
                        tableMedicalDiagonsis.AddCell(new Phrase(rowDiagonsis["LastResultDate"].ToString(), fntTableFont));

                    }
                }
                else
                {
                    tableMedicalDiagonsis.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 3, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableMedications.AddCell(new PdfPCell(new Phrase("Medication / Dosage", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                DataRow rowMedication = null;
                dataTsbleMedicalMedications = dataSetFillPDF.Tables[19];
                if (dataSetFillPDF.Tables[19].Rows.Count > 0)
                {
                    for (var i = 0; i < dataTsbleMedicalMedications.Rows.Count; i++)
                    {
                        rowMedication = dataTsbleMedicalMedications.Rows[i];
                        tableMedications.AddCell(new Phrase(rowMedication["MedicationDosage"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableMedications.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 1, HorizontalAlignment = 1, PaddingBottom = 10 });
                }
                //add the table to document
                tableMedications.SpacingAfter = 20;
                tableMedications.SpacingBefore = 20;
                tableMedicalDiagonsis.SpacingAfter = 20;
                tableMedicalDiagonsis.SpacingBefore = 20;
                // paragraph.SpacingAfter = 30;
                Paragraph paragraphMedicalDiagonsis = new Paragraph("1. Diagnoses");
                Paragraph paragraphMedicalDiagonsis1 = new Paragraph("List all current conditions and the most recent test date and result, if applicable, associated with each condition.For example: Hypertension(BP / date measured); Diabetes(HbA1c / result date); Asthma; Hyperlipidemia (LDLC / result date); Congestive Heart Failure; COPD; HIV / AIDS(CD4 count / result date); cancer; renal disease;liver disease; obesity; stroke history; vision / hearing impairment; neuropathy; incontinence, etc.");

                Paragraph paragraphMedicalMedication = new Paragraph("2. Medications (Prescriptions and Adherence)");
                Paragraph paragraphMedicalMedication1 = new Paragraph("List all current medications, including over - the - counter medicines and vitamins.In the event of a home visit,please ask the member to gather all of the medications in order to obtain the most accurate medication historypossible.");

                Paragraph paragraphMedicalMedication2 = new Paragraph("For Questions 2a – 2c, check the number next to the appropriate answer. Then add up the checked numbers to calculate a score.");

                doc.Add(paragraphMedicalDiagonsis);
                doc.Add(paragraphMedicalDiagonsis1);
                doc.Add(tableMedicalDiagonsis);
                doc.Add(paragraphMedicalMedication);
                doc.Add(paragraphMedicalMedication1);
                doc.Add(tableMedications);
                doc.Add(paragraphMedicalMedication2);


                doc.Close();
                writer.Dispose();
                writer.Close();
                PdfReader readerMedication = new PdfReader(NewComprehensiveDocument);
                PdfStamper stamperMedication = new PdfStamper(reader, new FileStream(newFileAddMedication, FileMode.Create));
                PdfImportedPage page2 = null;
                for (var i = 1; i <= readerMedication.NumberOfPages; i++)
                {
                    int pageNo = 1 + i;
                    int insertpage = 1 + i;
                    stamperMedication.InsertPage(insertpage, readerMedication.GetPageSize(1));
                    PdfContentByte pb = stamperMedication.GetUnderContent(pageNo);
                    page2 = stamperMedication.GetImportedPage(readerMedication, i);
                    pb.AddTemplate(page2, 0, 0);
                }
                //close the stamper
                stamperMedication.Dispose();
                stamperMedication.Close();
                readerMedication.Dispose();
                readerMedication.Close();
                reader.Dispose();
                reader.Close();


                //////////////////////////////////////////////////
                PdfPTable tableHealthMedications = new PdfPTable(1);
                tableHealthMedications.WidthPercentage = 100f;
                DataTable dataTableMedicalHealthMedications = null;
                tableHealthMedications.AddCell(new PdfPCell(new Phrase("Medication / Dosage", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });


                dataTableMedicalHealthMedications = dataSetFillPDF.Tables[18];

                if (dataSetFillPDF.Tables[18].Rows.Count > 0)
                {

                    for (var i = 0; i < dataTableMedicalHealthMedications.Rows.Count; i++)
                    {
                        DataRow row = dataTableMedicalHealthMedications.Rows[i];
                        tableHealthMedications.AddCell(new Phrase(row["MedicationDosage"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableHealthMedications.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 1, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                var doc1 = new Document(PageSize.LETTER);
                // PdfReader reader = new PdfReader(newFile);
                PdfWriter writer1 = PdfWriter.GetInstance(doc1, new FileStream(NewComprehensiveDocument, FileMode.Create));
                doc1.Open();
                Paragraph paragraphHealthMedication = new Paragraph("8e. What medications are prescribed for the conditions listed above ?");
                tableHealthMedications.SpacingAfter = 20;
                tableHealthMedications.SpacingBefore = 20;
                doc1.Add(paragraphHealthMedication);
                doc1.Add(tableHealthMedications);

                doc1.Close();
                writer1.Dispose();
                writer1.Close();

                PdfReader readerHealthMedication = new PdfReader(NewComprehensiveDocument);
                PdfReader readerHealthMedicationNew = new PdfReader(newFileAddMedication);
                PdfStamper stamperHealthMedication = new PdfStamper(readerHealthMedicationNew, new FileStream(newFileAddHealthMedication, FileMode.Create));
                PdfImportedPage page3 = null;
                for (var i = 1; i <= readerHealthMedication.NumberOfPages; i++)
                {
                    int pageNo = 4 + readerMedication.NumberOfPages + i;
                    int insertpage = 4 + readerMedication.NumberOfPages + i;
                    stamperHealthMedication.InsertPage(insertpage, readerHealthMedication.GetPageSize(1));
                    PdfContentByte pb1 = stamperHealthMedication.GetUnderContent(pageNo);
                    page3 = stamperHealthMedication.GetImportedPage(readerHealthMedication, i);
                    pb1.AddTemplate(page3, 0, 0);
                }
                //close the stamper
                stamperHealthMedication.Dispose();
                stamperHealthMedication.Close();
                readerHealthMedication.Dispose();
                readerHealthMedication.Close();
                readerHealthMedicationNew.Dispose();
                readerHealthMedicationNew.Close();

                //////////////////////////////////////////////

                PdfPTable tableMemberStatus = new PdfPTable(4);
                PdfPTable tableMemberNeeds = new PdfPTable(3);
                tableMemberStatus.WidthPercentage = 100f;
                tableMemberNeeds.WidthPercentage = 100f;

                DataTable dataTableMemberStatus = null;
                DataTable dataTableMemberNeeds = null;
                tableMemberStatus.AddCell(new PdfPCell(new Phrase("Member Status Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMemberStatus.AddCell(new PdfPCell(new Phrase("Receives/Amount", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMemberStatus.AddCell(new PdfPCell(new Phrase("Needs/Needs Recertification(list recertification date if applicable) ", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMemberStatus.AddCell(new PdfPCell(new Phrase("Stable/No Needs", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });


                dataTableMemberStatus = dataSetFillPDF.Tables[23];

                if (dataSetFillPDF.Tables[23].Rows.Count > 0)
                {

                    for (var i = 0; i < dataTableMemberStatus.Rows.Count; i++)
                    {

                        DataRow row = dataTableMemberStatus.Rows[i];
                        tableMemberStatus.AddCell(new Phrase(row["Entitlements"].ToString(), fntTableFont));
                        tableMemberStatus.AddCell(new Phrase(row["RecievesAmount"].ToString(), fntTableFont));
                        tableMemberStatus.AddCell(new Phrase(row["RecertificationDate"].ToString(), fntTableFont));
                        tableMemberStatus.AddCell(new Phrase(row["StableNoNeeds"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableMemberStatus.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                tableMemberNeeds.AddCell(new PdfPCell(new Phrase("Member Needs", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMemberNeeds.AddCell(new PdfPCell(new Phrase("Assistance Needed (Describe)", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableMemberNeeds.AddCell(new PdfPCell(new Phrase("Stable/No Needs", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });


                dataTableMemberNeeds = dataSetFillPDF.Tables[22];

                if (dataSetFillPDF.Tables[22].Rows.Count > 0)
                {

                    for (var i = 0; i < dataTableMemberNeeds.Rows.Count; i++)
                    {

                        DataRow row = dataTableMemberNeeds.Rows[i];
                        tableMemberNeeds.AddCell(new Phrase(row["FinancialElement"].ToString(), fntTableFont));
                        tableMemberNeeds.AddCell(new Phrase(row["AssisstanceNeeded"].ToString(), fntTableFont));
                        tableMemberNeeds.AddCell(new Phrase(row["StableNoNeeds"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableMemberNeeds.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 3, HorizontalAlignment = 1, PaddingBottom = 10 });
                }

                var doc2 = new Document(PageSize.LETTER);
                // PdfReader reader = new PdfReader(newFile);
                PdfWriter writer2 = PdfWriter.GetInstance(doc2, new FileStream(NewComprehensiveDocument, FileMode.Create));
                doc2.Open();
                Paragraph paragraphFinancialMemberStatus = new Paragraph("22. What is the member’s status regarding the following entitlements?");
                Paragraph paragraphFinancialMemberNeeds = new Paragraph("23. What are the member’s needs regarding the following financial elements?");
                tableMemberStatus.SpacingAfter = 20;
                tableMemberStatus.SpacingBefore = 20;
                tableMemberNeeds.SpacingAfter = 20;
                tableMemberNeeds.SpacingBefore = 20;
                doc2.Add(paragraphFinancialMemberStatus);
                doc2.Add(tableMemberStatus);
                doc2.Add(paragraphFinancialMemberNeeds);
                doc2.Add(tableMemberNeeds);
                doc2.Close();
                writer2.Dispose();
                writer2.Close();

                PdfReader readerFinancial = new PdfReader(NewComprehensiveDocument);
                PdfReader readerFinancialNew = new PdfReader(newFileAddHealthMedication);
                PdfStamper stamperFinancial = new PdfStamper(readerFinancialNew, new FileStream(newFileAddFinancial, FileMode.Create));
                PdfImportedPage page4 = null;
                for (var i = 1; i <= readerFinancial.NumberOfPages; i++)
                {
                    int pageNo = 7 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + i;
                    int insertpage = 7 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + i;
                    stamperFinancial.InsertPage(insertpage, readerFinancial.GetPageSize(1));
                    PdfContentByte pb1 = stamperFinancial.GetUnderContent(pageNo);
                    page4 = stamperFinancial.GetImportedPage(readerFinancial, i);
                    pb1.AddTemplate(page4, 0, 0);
                }
                //close the stamper
                stamperFinancial.Dispose();
                stamperFinancial.Close();
                readerFinancial.Dispose();
                readerFinancial.Close();
                readerFinancialNew.Dispose();
                readerFinancialNew.Close();

                ///////////////////////////////////////////////
                PdfPTable tableHousingSubsidies = new PdfPTable(4);
                tableHousingSubsidies.WidthPercentage = 100f;

                DataTable dataTableHousingSubsidies = null;
                tableHousingSubsidies.AddCell(new PdfPCell(new Phrase("Housing Subsidies Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableHousingSubsidies.AddCell(new PdfPCell(new Phrase("Other Details", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableHousingSubsidies.AddCell(new PdfPCell(new Phrase("Receives (Details,Including Case #)", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableHousingSubsidies.AddCell(new PdfPCell(new Phrase("Pending", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });

                dataTableHousingSubsidies = dataSetFillPDF.Tables[24];

                if (dataSetFillPDF.Tables[24].Rows.Count > 0)
                {

                    for (var i = 0; i < dataTableHousingSubsidies.Rows.Count; i++)
                    {

                        DataRow row = dataTableHousingSubsidies.Rows[i];
                        tableHousingSubsidies.AddCell(new Phrase(row["HousingSubsidy"].ToString(), fntTableFont));
                        tableHousingSubsidies.AddCell(new Phrase(row["OtherDetail"].ToString(), fntTableFont));
                        tableHousingSubsidies.AddCell(new Phrase(row["RecievesDetailsCase"].ToString(), fntTableFont));
                        tableHousingSubsidies.AddCell(new Phrase(row["Pending"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableHousingSubsidies.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });


                }
                var doc3 = new Document(PageSize.LETTER);
                // PdfReader reader = new PdfReader(newFile);
                PdfWriter writer3 = PdfWriter.GetInstance(doc3, new FileStream(NewComprehensiveDocument, FileMode.Create));
                doc3.Open();
                Paragraph paragraphHousingSubsidies = new Paragraph("26. Does the member receive any of the following housing subsidies?");
                tableHousingSubsidies.SpacingAfter = 20;
                tableHousingSubsidies.SpacingBefore = 20;
                doc3.Add(paragraphHousingSubsidies);
                doc3.Add(tableHousingSubsidies);
                doc3.Close();
                writer3.Dispose();
                writer3.Close();

                PdfReader readerHousingSubsidies = new PdfReader(NewComprehensiveDocument);
                PdfReader readerHousingSubsidiesNew = new PdfReader(newFileAddFinancial);
                PdfStamper stamperHousingSubsidies = new PdfStamper(readerHousingSubsidiesNew, new FileStream(newFileAddHousingSubsidies, FileMode.Create));
                PdfImportedPage page5 = null;
                for (var i = 1; i <= readerHousingSubsidies.NumberOfPages; i++)
                {
                    int pageNo = 8 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + i;
                    int insertpage = 8 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + i;
                    stamperHousingSubsidies.InsertPage(insertpage, readerHousingSubsidies.GetPageSize(1));
                    PdfContentByte pb1 = stamperHousingSubsidies.GetUnderContent(pageNo);
                    page5 = stamperHousingSubsidies.GetImportedPage(readerHousingSubsidies, i);
                    pb1.AddTemplate(page5, 0, 0);
                }
                //close the stamper
                stamperHousingSubsidies.Dispose();
                stamperHousingSubsidies.Close();
                readerHousingSubsidies.Dispose();
                readerHousingSubsidies.Close();
                readerHousingSubsidiesNew.Dispose();
                readerHousingSubsidiesNew.Close();

                /////////////////////////////////////////////////

                PdfPTable tableDomesticViolence = new PdfPTable(2);
                tableDomesticViolence.WidthPercentage = 100f;

                DataTable dataTableDomesticViolence = null;
                tableDomesticViolence.AddCell(new PdfPCell(new Phrase("Name", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableDomesticViolence.AddCell(new PdfPCell(new Phrase("Relationship", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });

                dataTableDomesticViolence = dataSetFillPDF.Tables[21];

                if (dataSetFillPDF.Tables[21].Rows.Count > 0)
                {

                    for (var i = 0; i < dataTableDomesticViolence.Rows.Count; i++)
                    {

                        DataRow row = dataTableDomesticViolence.Rows[i];
                        tableDomesticViolence.AddCell(new Phrase(row["Name"].ToString(), fntTableFont));
                        tableDomesticViolence.AddCell(new Phrase(row["Relationship"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableDomesticViolence.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 2, HorizontalAlignment = 1, PaddingBottom = 10 });
                }
                var doc4 = new Document(PageSize.LETTER);
                // PdfReader reader = new PdfReader(newFile);
                PdfWriter writer4 = PdfWriter.GetInstance(doc4, new FileStream(NewComprehensiveDocument, FileMode.Create));
                doc4.Open();
                Paragraph paragraphDomesticViolence = new Paragraph("30. List all the people who reside with the member, and their relationship to the member.");
                tableDomesticViolence.SpacingAfter = 20;
                tableDomesticViolence.SpacingBefore = 20;
                doc4.Add(paragraphDomesticViolence);
                doc4.Add(tableDomesticViolence);
                doc4.Close();
                writer4.Dispose();
                writer4.Close();

                PdfReader readerDomesticViolence = new PdfReader(NewComprehensiveDocument);
                PdfReader readerDomesticViolenceNew = new PdfReader(newFileAddHousingSubsidies);
                PdfStamper stamperDomesticViolence = new PdfStamper(readerDomesticViolenceNew, new FileStream(newFileAddDomesticViolence, FileMode.Create));
                PdfImportedPage page6 = null;
                for (var i = 1; i <= readerDomesticViolence.NumberOfPages; i++)
                {
                    int pageNo = 9 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + i;
                    int insertpage = 9 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + i;
                    stamperDomesticViolence.InsertPage(insertpage, readerDomesticViolence.GetPageSize(1));
                    PdfContentByte pb1 = stamperDomesticViolence.GetUnderContent(pageNo);
                    page6 = stamperDomesticViolence.GetImportedPage(readerDomesticViolence, i);
                    pb1.AddTemplate(page6, 0, 0);
                }

                //close the stamper
                stamperDomesticViolence.Dispose();
                stamperDomesticViolence.Close();
                readerDomesticViolence.Dispose();
                readerDomesticViolence.Close();
                readerDomesticViolenceNew.Dispose();
                readerDomesticViolenceNew.Close();

                /////////////////////////////////////////////////////
                PdfPTable tableLegal = new PdfPTable(2);
                tableLegal.WidthPercentage = 100f;

                DataTable dataTableLegal = null;
                tableLegal.AddCell(new PdfPCell(new Phrase("Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });
                tableLegal.AddCell(new PdfPCell(new Phrase("Detail", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204) });

                dataTableLegal = dataSetFillPDF.Tables[25];

                if (dataSetFillPDF.Tables[25].Rows.Count > 0)
                {

                    for (var i = 0; i < dataTableLegal.Rows.Count; i++)
                    {
                        DataRow row = dataTableLegal.Rows[i];
                        tableLegal.AddCell(new Phrase(row["LegalDate"].ToString(), fntTableFont));
                        tableLegal.AddCell(new Phrase(row["LegalDateDetails"].ToString(), fntTableFont));
                    }
                }
                else
                {
                    tableLegal.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 2, HorizontalAlignment = 1, PaddingBottom = 10 });
                }
                var doc5 = new Document(PageSize.LETTER);
                // PdfReader reader = new PdfReader(newFile);
                PdfWriter writer5 = PdfWriter.GetInstance(doc5, new FileStream(NewComprehensiveDocument, FileMode.Create));
                doc5.Open();
                Paragraph paragraphLegal = new Paragraph("53e. Upcoming Court Dates (if applicable)");
                tableLegal.SpacingAfter = 20;
                tableLegal.SpacingBefore = 20;
                doc5.Add(paragraphLegal);
                doc5.Add(tableLegal);
                doc5.Close();
                writer5.Dispose();
                writer5.Close();

                PdfReader readerLegal = new PdfReader(NewComprehensiveDocument);
                PdfReader readerLegalNew = new PdfReader(newFileAddDomesticViolence);
                PdfStamper stamperLegal = new PdfStamper(readerLegalNew, new FileStream(newFileAddLegal, FileMode.Create));
                PdfImportedPage page7 = null;
                for (var i = 1; i <= readerLegal.NumberOfPages; i++)
                {
                    int pageNo = 14 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + readerDomesticViolence.NumberOfPages + i;
                    int insertpage = 14 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + readerDomesticViolence.NumberOfPages + i;
                    stamperLegal.InsertPage(insertpage, readerLegal.GetPageSize(1));
                    PdfContentByte pb1 = stamperLegal.GetUnderContent(pageNo);
                    page7 = stamperLegal.GetImportedPage(readerLegal, i);
                    pb1.AddTemplate(page7, 0, 0);
                }

                iTextSharp.text.Font pageTextFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                for (int i = 1; i <= readerLegalNew.NumberOfPages; i++)
                {
                    if (27 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + readerDomesticViolence.NumberOfPages == i)
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 160f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 160f, 0);

                    }
                    if (25 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + readerDomesticViolence.NumberOfPages == i)
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 520f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 520f, 0);

                    }
                    if (24 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages + readerDomesticViolence.NumberOfPages == i)
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 590f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 590f, 0);

                    }
                    if (15 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages + readerHousingSubsidies.NumberOfPages == i)
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 160f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 160f, 0);

                    }
                    if (8 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages + readerFinancial.NumberOfPages == i)
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 130f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 130f, 0);

                    }
                    if (7 + readerMedication.NumberOfPages + readerHealthMedication.NumberOfPages == i)
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 130f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 130f, 0);

                    }
                    else
                    {
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + readerLegalNew.NumberOfPages), 590f, 15f, 0);
                        ColumnText.ShowTextAligned(stamperLegal.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")), 2f, 15f, 0);
                    }
                }

                stamperLegal.Dispose();
                stamperLegal.Close();
                readerLegal.Dispose();
                readerLegal.Close();
                readerLegalNew.Dispose();
                readerLegalNew.Close();
                ////////////////////////////////////////////////////


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return newFileAddLegal;
        }



        private void fillMedicalSectionPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF,string status)
        {
            DataTable dataTableMedical = null;
            DataTable dataTsbleMedicalDiagnosis = null;
            DataTable dataTsbleMedicalMedications = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableMedical = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableMedical.Rows[0];

                    pdfFormFields.SetField("MedicationsOnTime", row["MedicationsOnTime"].ToString());
                    pdfFormFields.SetField("MedicationMissed", row["MedicationMissed"].ToString());
                    pdfFormFields.SetField("LastMedicationMissed", row["LastMedicationMissed"].ToString());
                    pdfFormFields.SetField("Score", row["Score"].ToString());
                    pdfFormFields.SetField("ConsequencesDoses", row["ConsequencesDoses"].ToString());
                    pdfFormFields.SetField("MedicationsOnTime", row["MedicationsOnTime"].ToString());
                    pdfFormFields.SetField("MedicationMissed", row["MedicationMissed"].ToString());
                    pdfFormFields.SetField("LastMedicationMissed", row["LastMedicationMissed"].ToString());
                    pdfFormFields.SetField("Score", row["Score"].ToString());
                    pdfFormFields.SetField("ConsequencesDoses", row["ConsequencesDoses"].ToString());
                    pdfFormFields.SetField("MedicationsInfo", row["MedicationsInfo"].ToString());
                    pdfFormFields.SetField("HospitalizationReason", row["HospitalizationReason"].ToString());
                    pdfFormFields.SetField("AllergiesList", row["AllergiesList"].ToString());
                    pdfFormFields.SetField("MemberHavePain", row["MemberHavePain"].ToString());
                    pdfFormFields.SetField("PainArea", row["PainArea"].ToString());
                    pdfFormFields.SetField("PainOccurance", row["PainOccurance"].ToString());
                    pdfFormFields.SetField("PainFeeling", row["PainFeeling"].ToString());
                    pdfFormFields.SetField("PainReporting", row["PainReporting"].ToString());
                    pdfFormFields.SetField("PainScaleWorst", row["PainScaleWorst"].ToString());
                    pdfFormFields.SetField("PainScaleBest", row["PainScaleBest"].ToString());
                    pdfFormFields.SetField("PainMakesBetter", row["PainMakesBetter"].ToString());
                    pdfFormFields.SetField("PainMakeWorse", row["PainMakeWorse"].ToString());
                    pdfFormFields.SetField("PainReliefMethods", row["PainReliefMethods"].ToString());
                    pdfFormFields.SetField("PainAffects", row["PainAffects"].ToString());
                    pdfFormFields.SetField("MemberSharingHealth", row["MemberSharingHealth"].ToString());
                    pdfFormFields.SetField("PrimaryCareProvider", row["PrimaryCareProvider"].ToString());
                    pdfFormFields.SetField("Psychiatrist", row["Psychiatrist"].ToString());
                    pdfFormFields.SetField("PainManagementPhysician", row["PainManagementPhysician"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MedicalPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "MedicalPublished");
                    }

                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MedicalPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "MedicalPublished");
                    }
                }
                //if (dataSetFillPDF.Tables[20].Rows.Count > 0)
                //{
                //    dataTsbleMedicalDiagnosis = dataSetFillPDF.Tables[20];
                //    fillMedicalDiagnosis(pdfFormFields, dataTsbleMedicalDiagnosis);
                //}
                //if (dataSetFillPDF.Tables[19].Rows.Count > 0)
                //{
                //    dataTsbleMedicalMedications = dataSetFillPDF.Tables[19];
                //    fillMedicalMedications(pdfFormFields, dataTsbleMedicalMedications);
                //}
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


       

        private void fillMentalHealthSectionPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableMedicalHealth = null;
            DataTable dataTsbleMedicalHealthMedications = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[11].Rows.Count > 0)
                {
                    dataTableMedicalHealth = dataSetFillPDF.Tables[11];
                    DataRow row = dataTableMedicalHealth.Rows[0];

                    pdfFormFields.SetField("BipolarDisorder", row["BipolarDisorder"].ToString());
                    pdfFormFields.SetField("Schizophrenia", row["Schizophrenia"].ToString());
                    pdfFormFields.SetField("SevereDepression", row["SevereDepression"].ToString());
                    pdfFormFields.SetField("SchizoaffectiveDisorder", row["SchizoaffectiveDisorder"].ToString());
                    pdfFormFields.SetField("NoneOfTheAbove", row["NoneOfTheAbove"].ToString());
                    pdfFormFields.SetField("TreatmentReceived", row["TreatmentReceived"].ToString());
                    pdfFormFields.SetField("ProvidersDetail", row["ProvidersDetail"].ToString());
                    pdfFormFields.SetField("MemberSeeProvidersList", row["MemberSeeProvidersList"].ToString());
                    pdfFormFields.SetField("ConfusionAboutMedicine", row["ConfusionAboutMedicine"].ToString());
                    pdfFormFields.SetField("InstructionLanguage", row["InstructionLanguage"].ToString());
                    pdfFormFields.SetField("DifficultyInMedication", row["DifficultyInMedication"].ToString());
                    pdfFormFields.SetField("MedicationSideEffects", row["MedicationSideEffects"].ToString());
                    pdfFormFields.SetField("UnderstandingOfMedicine", row["UnderstandingOfMedicine"].ToString());
                    pdfFormFields.SetField("RememberingMedicine", row["RememberingMedicine"].ToString());
                    pdfFormFields.SetField("OtherBarriers", row["OtherBarriers"].ToString());
                    pdfFormFields.SetField("OherBarriersDescription", row["OherBarriersDescription"].ToString());
                    pdfFormFields.SetField("MemberReactionOnMedication", row["MemberReactionOnMedication"].ToString());
                    pdfFormFields.SetField("MemberReactionNotOnMedication", row["MemberReactionNotOnMedication"].ToString());
                    pdfFormFields.SetField("AllergyPsychiatricMedications", row["AllergyPsychiatricMedications"].ToString());
                    pdfFormFields.SetField("MemberBeenToED", row["MemberBeenToED"].ToString());
                    pdfFormFields.SetField("RecentVisitDate", row["RecentVisitDate"].ToString());
                    pdfFormFields.SetField("NoOfTimesMemberAdmitted", row["NoOfTimesMemberAdmitted"].ToString());
                    pdfFormFields.SetField("DateOfRecentAdmission", row["DateOfRecentAdmission"].ToString());
                    pdfFormFields.SetField("AgeOnsetSymptoms", row["AgeOnsetSymptoms"].ToString());
                    pdfFormFields.SetField("OtherDetailsPsychiatricHistory", row["OtherDetailsPsychiatricHistory"].ToString());
                    pdfFormFields.SetField("MemberExperiencedTrauma", row["MemberExperiencedTrauma"].ToString());
                    pdfFormFields.SetField("MemberExperiencedDescribe", row["MemberExperiencedDescribe"].ToString());
                    pdfFormFields.SetField("MemberRecievedHelp", row["MemberRecievedHelp"].ToString());
                    pdfFormFields.SetField("MemberWishToReffered", row["MemberWishToReffered"].ToString());
                    pdfFormFields.SetField("PreviousDrugTreatment", row["PreviousDrugTreatment"].ToString());
                    pdfFormFields.SetField("PreviousDrugTreatmentDetail", row["PreviousDrugTreatmentDetail"].ToString());
                    pdfFormFields.SetField("MemberRecievesKindTreatment", row["MemberRecievesKindTreatment"].ToString());
                    pdfFormFields.SetField("ListDetailsProviders", row["ListDetailsProviders"].ToString());
                    pdfFormFields.SetField("NoOfTimesMemberSeeProviderList", row["NoOfTimesMemberSeeProviderList"].ToString());
                    pdfFormFields.SetField("MemberAdmittedToDetoxTreatment", row["MemberAdmittedToDetoxTreatment"].ToString());
                    pdfFormFields.SetField("DateRecentAdmission", row["DateRecentAdmission"].ToString());
                    pdfFormFields.SetField("NoOfTimesMemberAdmittedRehab", row["NoOfTimesMemberAdmittedRehab"].ToString());
                    pdfFormFields.SetField("DateMostRecentVisit", row["DateMostRecentVisit"].ToString());
                    pdfFormFields.SetField("AgeOfFirstSubstance", row["AgeOfFirstSubstance"].ToString());
                    pdfFormFields.SetField("DurationOfSoberity", row["DurationOfSoberity"].ToString());
                    pdfFormFields.SetField("TreatmentModalityEffective", row["TreatmentModalityEffective"].ToString());
                    pdfFormFields.SetField("SubstanceUseTriggers", row["SubstanceUseTriggers"].ToString());
                    pdfFormFields.SetField("MemberProtectItself", row["MemberProtectItself"].ToString());
                    pdfFormFields.SetField("PerceptionGoodAboutSubstance", row["PerceptionGoodAboutSubstance"].ToString());
                    pdfFormFields.SetField("PerceptionNoGoodAboutSubstance", row["PerceptionNoGoodAboutSubstance"].ToString());
                    pdfFormFields.SetField("OtherDetailSubstance", row["OtherDetailSubstance"].ToString());
                    pdfFormFields.SetField("DoesMemberUseTobacco", row["DoesMemberUseTobacco"].ToString());
                    pdfFormFields.SetField("MemberUseTobaccoSpecify", row["MemberUseTobaccoSpecify"].ToString());
                    pdfFormFields.SetField("NoOfTimeMemberSmokePerDay", row["NoOfTimeMemberSmokePerDay"].ToString());
                    pdfFormFields.SetField("MemberCurrentlyUseECigarettes", row["MemberCurrentlyUseECigarettes"].ToString());
                    pdfFormFields.SetField("MemberCompletedCessationProgram", row["MemberCompletedCessationProgram"].ToString());
                    pdfFormFields.SetField("MemberInteresedInProgram", row["MemberInteresedInProgram"].ToString());
                    pdfFormFields.SetField("ReferralMade", row["ReferralMade"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MentalHealthPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "MentalHealthPublished");
                    }
                   
                }
                else{
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "MentalHealthPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "MentalHealthPublished");
                    }
                }
                //if (dataSetFillPDF.Tables[18].Rows.Count > 0)
                //{
                //    dataTsbleMedicalHealthMedications = dataSetFillPDF.Tables[18];
                //    fillMedicalhealthMeidcations(pdfFormFields, dataTsbleMedicalHealthMedications);
                //}
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        


        private void fillFinancialSectionPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableFinancial = null;
            DataTable dataTableMemberStatus = null;
            DataTable dataTableMemberNeeds = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[5].Rows.Count > 0)
                {
                    dataTableFinancial = dataSetFillPDF.Tables[5];
                    DataRow row = dataTableFinancial.Rows[0];

                    pdfFormFields.SetField("MembersMonthlyIncome", row["MembersMonthlyIncome"].ToString());
                    pdfFormFields.SetField("SourceOfIncome", row["SourceOfIncome"].ToString());
                    pdfFormFields.SetField("PeopleResideInHousehold", row["PeopleResideInHousehold"].ToString());
                    pdfFormFields.SetField("PeopleResideInHouseholdDependents", row["PeopleResideInHouseholdDependents"].ToString());
                    pdfFormFields.SetField("MonthlyCostOfRent", row["MonthlyCostOfRent"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "FinancialPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "FinancialPublished");
                    }



                   

                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "FinancialPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "FinancialPublished");
                    }
                }
                //if (dataSetFillPDF.Tables[23].Rows.Count > 0)
                //{
                //    dataTableMemberStatus = dataSetFillPDF.Tables[23];
                //    fillMemberStatus(pdfFormFields, dataTableMemberStatus);
                //}
                //if (dataSetFillPDF.Tables[22].Rows.Count > 0)
                //{
                //    dataTableMemberNeeds = dataSetFillPDF.Tables[22];
                //    fillMemberNeeds(pdfFormFields, dataTableMemberNeeds);
                //}
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
       
       
        private void fillHousingSectionPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableHousing = null;
            DataTable dataTableHousingSubsidy = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[7].Rows.Count > 0)
                {
                    dataTableHousing = dataSetFillPDF.Tables[7];
                    DataRow row = dataTableHousing.Rows[0];

                    pdfFormFields.SetField("MemberCurrentlyLiving", row["MemberCurrentlyLiving"].ToString());
                    pdfFormFields.SetField("HouseApartmentType", row["HouseApartmentType"].ToString());
                    pdfFormFields.SetField("LeaseOrMemberName", row["LeaseOrMemberName"].ToString());
                    pdfFormFields.SetField("PlaceofHouseStable", row["PlaceofHouseStable"].ToString());
                    pdfFormFields.SetField("FriendRelativeHomeDetail", row["FriendRelativeHomeDetail"].ToString());
                    pdfFormFields.SetField("MemberRemainFriendRelativeHome", row["MemberRemainFriendRelativeHome"].ToString());
                    pdfFormFields.SetField("MemberRemainParentImmediateGuardian", row["MemberRemainParentImmediateGuardian"].ToString());
                    pdfFormFields.SetField("MemberRemainRespiteCare", row["MemberRemainRespiteCare"].ToString());
                    pdfFormFields.SetField("MemberRemainHalfwayHouse", row["MemberRemainHalfwayHouse"].ToString());
                    pdfFormFields.SetField("MemberRemainHomelessStreet", row["MemberRemainHomelessStreet"].ToString());
                    pdfFormFields.SetField("MemberRemainHomelessWithOthers", row["MemberRemainHomelessWithOthers"].ToString());
                    pdfFormFields.SetField("MemberRemainHomelessRegistered", row["MemberRemainHomelessRegistered"].ToString());
                    pdfFormFields.SetField("SupportedHousingDetail", row["SupportedHousingDetail"].ToString());
                    pdfFormFields.SetField("MemberRemainsupportedHousing", row["MemberRemainsupportedHousing"].ToString());
                    pdfFormFields.SetField("MemebrGiveConsentToSpeak", row["MemebrGiveConsentToSpeak"].ToString());
                    pdfFormFields.SetField("TimeMemberResidingCurrentLocation", row["TimeMemberResidingCurrentLocation"].ToString());
                    pdfFormFields.SetField("ConcernsCurrentLiving", row["ConcernsCurrentLiving"].ToString());
                    pdfFormFields.SetField("ConcernsCurrentLivingDetails", row["ConcernsCurrentLivingDetails"].ToString());
                    pdfFormFields.SetField("MemberRiskLoosingCurrentHousing", row["MemberRiskLoosingCurrentHousing"].ToString());
                    pdfFormFields.SetField("RentArrears", row["RentArrears"].ToString());
                    pdfFormFields.SetField("RentArrearsSpecifyAmount", row["RentArrearsSpecifyAmount"].ToString());
                    pdfFormFields.SetField("LossOfHousingSubsidy", row["LossOfHousingSubsidy"].ToString());
                    pdfFormFields.SetField("LandlordIssue", row["LandlordIssue"].ToString());
                    pdfFormFields.SetField("Other", row["Other"].ToString());
                    pdfFormFields.SetField("OtherSpecify", row["OtherSpecify"].ToString());
                    pdfFormFields.SetField("RecievedNoticeCityMarshal", row["RecievedNoticeCityMarshal"].ToString());
                    pdfFormFields.SetField("AddressLossOfHousing", row["AddressLossOfHousing"].ToString());
                    pdfFormFields.SetField("MemberInEvictionProceedings", row["MemberInEvictionProceedings"].ToString());
                    pdfFormFields.SetField("WorkingWithAttorney", row["WorkingWithAttorney"].ToString());
                    pdfFormFields.SetField("ConsentToSpeakAttorney", row["ConsentToSpeakAttorney"].ToString());
                    pdfFormFields.SetField("APSInvolved", row["APSInvolved"].ToString());
                    pdfFormFields.SetField("OtherHousingOptions", row["OtherHousingOptions"].ToString());
                    pdfFormFields.SetField("OtherHousingOptionsDetail", row["OtherHousingOptionsDetail"].ToString());
                    pdfFormFields.SetField("AcceptReferralShelter", row["AcceptReferralShelter"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "HousingPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "HousingPublished");
                    }

                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "HousingPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "HousingPublished");
                    }
                }
                //if (dataSetFillPDF.Tables[24].Rows.Count > 0)
                //{
                //    dataTableHousingSubsidy = dataSetFillPDF.Tables[24];
                //    fillHousingSubsidy(pdfFormFields, dataTableHousingSubsidy);
                //}
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
       


        private void fillDomesticViolancePDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableDomesticViolance = null;
            DataTable dataTableDomesticViolanceMemberRelationship = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[3].Rows.Count > 0)
                {
                    dataTableDomesticViolance = dataSetFillPDF.Tables[3];
                    DataRow row = dataTableDomesticViolance.Rows[0];

                    pdfFormFields.SetField("MemberFeelSafeWithPeople", row["MemberFeelSafeWithPeople"].ToString());
                    pdfFormFields.SetField("MemberFeelSafeWithPeopleDetail", row["MemberFeelSafeWithPeopleDetail"].ToString());
                    pdfFormFields.SetField("MemberNotFeelSafeWithPeople", row["MemberNotFeelSafeWithPeople"].ToString());
                    pdfFormFields.SetField("MemberNotFeelSafeWithPeopleDetail", row["MemberNotFeelSafeWithPeopleDetail"].ToString());
                    pdfFormFields.SetField("MemberVictimDomesticViolance", row["MemberVictimDomesticViolance"].ToString());
                    pdfFormFields.SetField("MemberVictimDomesticViolanceDetail", row["MemberVictimDomesticViolanceDetail"].ToString());
                    pdfFormFields.SetField("MemberFeelsLifeInDanger", row["MemberFeelsLifeInDanger"].ToString());
                    pdfFormFields.SetField("MemberFeelsLifeInDangerDetail", row["MemberFeelsLifeInDangerDetail"].ToString());
                    pdfFormFields.SetField("UnderstandsDomesticViolance", row["UnderstandsDomesticViolance"].ToString());
                    pdfFormFields.SetField("OtherPersonMakesAfraid", row["OtherPersonMakesAfraid"].ToString());
                    pdfFormFields.SetField("OtherPersonMakesAfraidDetail", row["OtherPersonMakesAfraidDetail"].ToString());
                    pdfFormFields.SetField("DoneAnythingInLifePlan", row["DoneAnythingInLifePlan"].ToString());
                    pdfFormFields.SetField("DoneAnythingInLifePlanDetail", row["DoneAnythingInLifePlanDetail"].ToString());
                    pdfFormFields.SetField("WhenPersonDisagreeAbove", row["WhenPersonDisagreeAbove"].ToString());
                    pdfFormFields.SetField("PhysicalFightingInRelationship", row["PhysicalFightingInRelationship"].ToString());
                    pdfFormFields.SetField("PhysicalFightingInRelationshipDetail", row["PhysicalFightingInRelationshipDetail"].ToString());
                    pdfFormFields.SetField("PoliceInvolvementInViolance", row["PoliceInvolvementInViolance"].ToString());
                    pdfFormFields.SetField("ProtectionAgainstMember", row["ProtectionAgainstMember"].ToString());
                    pdfFormFields.SetField("OtherPersonCriticizedMember", row["OtherPersonCriticizedMember"].ToString());
                    pdfFormFields.SetField("OtherPersonCriticizedMemberDetial", row["OtherPersonCriticizedMemberDetial"].ToString());
                    pdfFormFields.SetField("MemberEverInstitutionalization", row["MemberEverInstitutionalization"].ToString());
                    pdfFormFields.SetField("MemberEverInstitutionalizationDetail", row["MemberEverInstitutionalizationDetail"].ToString());
                    pdfFormFields.SetField("MemberIsolatedEverdayLiving", row["MemberIsolatedEverdayLiving"].ToString());
                    pdfFormFields.SetField("MemberIsolatedEverdayLivingDetail", row["MemberIsolatedEverdayLivingDetail"].ToString());
                    pdfFormFields.SetField("ObligatedInSexWitihIdentifiedAbuser", row["ObligatedInSexWitihIdentifiedAbuser"].ToString());
                    pdfFormFields.SetField("ObligatedInSexWitihIdentifiedAbuserDetail", row["ObligatedInSexWitihIdentifiedAbuserDetail"].ToString());
                    pdfFormFields.SetField("TouchedInSexualWithoutPermission", row["TouchedInSexualWithoutPermission"].ToString());
                    pdfFormFields.SetField("TouchedInSexualWithoutPermissionDetail", row["TouchedInSexualWithoutPermissionDetail"].ToString());
                    pdfFormFields.SetField("MemberMoneyUsedInappropriately", row["MemberMoneyUsedInappropriately"].ToString());
                    pdfFormFields.SetField("MemberMoneyUsedInappropriatelyDetail", row["MemberMoneyUsedInappropriatelyDetail"].ToString());
                    pdfFormFields.SetField("ForcedToMakeFinancialDecisions", row["ForcedToMakeFinancialDecisions"].ToString());
                    pdfFormFields.SetField("ForcedToMakeFinancialDecisionsDetail", row["ForcedToMakeFinancialDecisionsDetail"].ToString());
                    pdfFormFields.SetField("ForcedToSignDocuments", row["ForcedToSignDocuments"].ToString());
                    pdfFormFields.SetField("ForcedToSignDocumentsDetail", row["ForcedToSignDocumentsDetail"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DomesticViolancePublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "DomesticViolancePublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DomesticViolancePublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "DomesticViolancePublished");
                    }
                }
                //if (dataSetFillPDF.Tables[21].Rows.Count > 0)
                //{
                //    dataTableDomesticViolanceMemberRelationship = dataSetFillPDF.Tables[21];
                //    fillDomesticViolance(pdfFormFields, dataTableDomesticViolanceMemberRelationship);
                //}

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
       
        

        private void fillLegalSectionPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableLegal = null;
            DataTable dataTableLegalCourtDates = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[9].Rows.Count > 0)
                {
                    dataTableLegal = dataSetFillPDF.Tables[9];
                    DataRow row = dataTableLegal.Rows[0];

                    pdfFormFields.SetField("IncomeSupportExigentNeeds", row["IncomeSupportExigentNeeds"].ToString());
                    pdfFormFields.SetField("IncomeSupportOngoingLegalActivity", row["IncomeSupportOngoingLegalActivity"].ToString());
                    pdfFormFields.SetField("IncomeSupportLegalHistory", row["IncomeSupportLegalHistory"].ToString());
                    pdfFormFields.SetField("HousingUtilitiesExigentNeeds", row["HousingUtilitiesExigentNeeds"].ToString());
                    pdfFormFields.SetField("HousingUtilitiesOngoingLegalActivity", row["HousingUtilitiesOngoingLegalActivity"].ToString());
                    pdfFormFields.SetField("HousingUtilitiesLegalHistory", row["HousingUtilitiesLegalHistory"].ToString());
                    pdfFormFields.SetField("LegalStatusExigentNeeds", row["LegalStatusExigentNeeds"].ToString());
                    pdfFormFields.SetField("LegalStatusOngoingLegalActivity", row["LegalStatusOngoingLegalActivity"].ToString());
                    pdfFormFields.SetField("LegalStatusLegalHistory", row["LegalStatusLegalHistory"].ToString());
                    pdfFormFields.SetField("PersonalAndFamilyExigentNeeds", row["PersonalAndFamilyExigentNeeds"].ToString());
                    pdfFormFields.SetField("PersonalAndFamilyOngoingLegalActivity", row["PersonalAndFamilyOngoingLegalActivity"].ToString());
                    pdfFormFields.SetField("PersonalAndFamilyLegalActivity", row["PersonalAndFamilyLegalActivity"].ToString());
                    pdfFormFields.SetField("MemberRegisteredSexOffender", row["MemberRegisteredSexOffender"].ToString());
                    pdfFormFields.SetField("MemberRegisteredSexOffenderDetail", row["MemberRegisteredSexOffenderDetail"].ToString());
                    pdfFormFields.SetField("State", row["State"].ToString());
                    pdfFormFields.SetField("City", row["City"].ToString());
                    pdfFormFields.SetField("County", row["County"].ToString());
                    pdfFormFields.SetField("MemberEverIncarcerated", row["MemberEverIncarcerated"].ToString());
                    pdfFormFields.SetField("MemberEverIncarceratedDetail", row["MemberEverIncarceratedDetail"].ToString());
                    pdfFormFields.SetField("ParoleOfficerName", row["ParoleOfficerName"].ToString());
                    pdfFormFields.SetField("ParoleOfficerNumber", row["ParoleOfficerNumber"].ToString());
                    pdfFormFields.SetField("ParoleOfficerLengthOfTime", row["ParoleOfficerLengthOfTime"].ToString());
                    pdfFormFields.SetField("ConsentToSpeakWithParoleOfficer", row["ConsentToSpeakWithParoleOfficer"].ToString());
                    pdfFormFields.SetField("ProbationOfficerName", row["ProbationOfficerName"].ToString());
                    pdfFormFields.SetField("ProbationOfficerNumber", row["ProbationOfficerNumber"].ToString());
                    pdfFormFields.SetField("ProbationOfficerLenghtOfTime", row["ProbationOfficerLenghtOfTime"].ToString());
                    pdfFormFields.SetField("ConsentToSpeakWithProbationOfficer", row["ConsentToSpeakWithProbationOfficer"].ToString());
                    pdfFormFields.SetField("MemberHaveAttorney", row["MemberHaveAttorney"].ToString());
                    pdfFormFields.SetField("AttorneyNameAndContact", row["AttorneyNameAndContact"].ToString());
                    pdfFormFields.SetField("ConsentToSpeakWithAttorney", row["ConsentToSpeakWithAttorney"].ToString());
                    pdfFormFields.SetField("ClientNeedreferralLegalServices", row["ClientNeedreferralLegalServices"].ToString());
                    pdfFormFields.SetField("CourtOrderedServices", row["CourtOrderedServices"].ToString());
                    pdfFormFields.SetField("CourtOrderedServicesDetail", row["CourtOrderedServicesDetail"].ToString());
                    pdfFormFields.SetField("MemberAssisstancewithTransportation", row["MemberAssisstancewithTransportation"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "LegalPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "LegalPublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "LegalPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "LegalPublished");
                    }
                }
                //if (dataSetFillPDF.Tables[25].Rows.Count > 0)
                //{
                //    dataTableLegalCourtDates = dataSetFillPDF.Tables[25];
                //    fillLegalDates(pdfFormFields, dataTableLegalCourtDates);
                //}
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
       
        private void fillSafeguardReviewPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableSafeguardReview = null;
            DataTable dataTable = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[0].Rows.Count > 0)
                {
                    dataTableSafeguardReview = dataSetFillPDF.Tables[0];
                    DataRow row = dataTableSafeguardReview.Rows[0];

                    pdfFormFields.SetField("Choking", row["Choking"].ToString());
                    pdfFormFields.SetField("ChokingDetails", row["ChokingDetails"].ToString());
                    pdfFormFields.SetField("RiskOfFalls", row["RiskOfFalls"].ToString());
                    pdfFormFields.SetField("RiskOfFallsDetails", row["RiskOfFallsDetails"].ToString());
                    pdfFormFields.SetField("SelfHarmBehaviors", row["SelfHarmBehaviors"].ToString());
                    pdfFormFields.SetField("SelfHarmBehaviorsDetails", row["SelfHarmBehaviorsDetails"].ToString());
                    pdfFormFields.SetField("FireSafety", row["FireSafety"].ToString());
                    pdfFormFields.SetField("FireSafetyDetails", row["FireSafetyDetails"].ToString());
                    pdfFormFields.SetField("SafetyInTheCommunity", row["SafetyInTheCommunity"].ToString());
                    pdfFormFields.SetField("SafetyInTheCommunityDetails", row["SafetyInTheCommunityDetails"].ToString());
                    pdfFormFields.SetField("HousingFoodInstability", row["HousingFoodInstability"].ToString());
                    pdfFormFields.SetField("HousingFoodInstabilityDetails", row["HousingFoodInstabilityDetails"].ToString());
                    pdfFormFields.SetField("EmergencyEvacuation", row["EmergencyEvacuation"].ToString());
                    pdfFormFields.SetField("EmergencyEvacuationDetails", row["EmergencyEvacuationDetails"].ToString());
                    pdfFormFields.SetField("BackupPlanEmergencySituations", row["BackupPlanEmergencySituations"].ToString());
                    pdfFormFields.SetField("BackupPlanEmergencySituationsDetails", row["BackupPlanEmergencySituationsDetails"].ToString());
                    pdfFormFields.SetField("LevelOfIndependence", row["LevelOfIndependence"].ToString());
                    pdfFormFields.SetField("LevelOfIndependenceDetails", row["LevelOfIndependenceDetails"].ToString());
                    pdfFormFields.SetField("LevelOfSupervision", row["LevelOfSupervision"].ToString());
                    pdfFormFields.SetField("LevelOfSupervisionDetails", row["LevelOfSupervisionDetails"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "AreaSafePublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "AreaSafePublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "AreaSafePublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "AreaSafePublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillLivingSkillsPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableLivingSkills = null;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[8].Rows.Count > 0)
                {
                    dataTableLivingSkills = dataSetFillPDF.Tables[8];
                    DataRow row = dataTableLivingSkills.Rows[0];

                    pdfFormFields.SetField("LanguageSkills", row["LanguageSkills"].ToString());
                    pdfFormFields.SetField("MemoryLearning", row["MemoryLearning"].ToString());
                    pdfFormFields.SetField("AbilityToAction", row["AbilityToAction"].ToString());
                    pdfFormFields.SetField("NeedsAssistanceEating", row["NeedsAssistanceEating"].ToString());
                    pdfFormFields.SetField("MealPreparation", row["MealPreparation"].ToString());
                    pdfFormFields.SetField("HousekeepingCleanliness", row["HousekeepingCleanliness"].ToString());
                    pdfFormFields.SetField("ManagingFinances", row["ManagingFinances"].ToString());
                    pdfFormFields.SetField("ManagingMedications", row["ManagingMedications"].ToString());
                    pdfFormFields.SetField("PhoneUse", row["PhoneUse"].ToString());
                    pdfFormFields.SetField("Transportation", row["Transportation"].ToString());
                    pdfFormFields.SetField("ProblematicSocialBehaviors", row["ProblematicSocialBehaviors"].ToString());
                    pdfFormFields.SetField("HealthComponents", row["HealthComponents"].ToString());
                    pdfFormFields.SetField("IndividualHaveSupportToHelp", row["IndividualHaveSupportToHelp"].ToString());
                    pdfFormFields.SetField("DevelopmentalMilestones", row["DevelopmentalMilestones"].ToString());
                    pdfFormFields.SetField("SelfPreservationSkills", row["SelfPreservationSkills"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "LivingSkillsPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "LivingSkillsPublished");
                    }
                }
                else{
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "LivingSkillsPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "LivingSkillsPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        private void fillBehavioralSupportPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableBehavioralSupport = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[1].Rows.Count > 0)
                {
                    dataTableBehavioralSupport = dataSetFillPDF.Tables[1];
                    DataRow row = dataTableBehavioralSupport.Rows[0];

                    pdfFormFields.SetField("ChallengingBehaviors", row["ChallengingBehaviors"].ToString());
                    pdfFormFields.SetField("BehaviorSupportPlan", row["BehaviorSupportPlan"].ToString());
                    pdfFormFields.SetField("BehaviorSupportPlanDetails", row["BehaviorSupportPlanDetails"].ToString());
                    pdfFormFields.SetField("RestrictivePhysicalInterventions", row["RestrictivePhysicalInterventions"].ToString());
                    pdfFormFields.SetField("SkillsAndResources", row["SkillsAndResources"].ToString());
                    pdfFormFields.SetField("StrengthsOfIndividual", row["StrengthsOfIndividual"].ToString());
                    pdfFormFields.SetField("IsEngagementInTreatmentPlan", row["IsEngagementInTreatmentPlan"].ToString());
                    pdfFormFields.SetField("EngagementInTreatmentPlanDetails", row["EngagementInTreatmentPlanDetails"].ToString());
                    pdfFormFields.SetField("IdentifyBarriersToService", row["IdentifyBarriersToService"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "BehavioralSupportPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "BehavioralSupportPublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "BehavioralSupportPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "BehavioralSupportPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private void fillVocationalStatusPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableVocatinalStatus = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[4].Rows.Count > 0)
                {
                    dataTableVocatinalStatus = dataSetFillPDF.Tables[4];
                    DataRow row = dataTableVocatinalStatus.Rows[0];

                    pdfFormFields.SetField("Education", row["Education"].ToString());
                    pdfFormFields.SetField("LevelOfEducation", row["LevelOfEducation"].ToString());
                    pdfFormFields.SetField("InSchool", row["InSchool"].ToString());
                    pdfFormFields.SetField("PreSchoolInformation", row["PreSchoolInformation"].ToString());
                    pdfFormFields.SetField("CurrentEducationalPlan", row["CurrentEducationalPlan"].ToString());
                    pdfFormFields.SetField("CurrentServicesAndAccommodations", row["CurrentServicesAndAccommodations"].ToString());
                    pdfFormFields.SetField("MyDay", row["MyDay"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "VocationalPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "VocationalPublished");
                    }
                }
                else{
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "VocationalPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "VocationalPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private void fillSelfDirectedServicesPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableDirectedServicesPDF = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[14].Rows.Count > 0)
                {
                    dataTableDirectedServicesPDF = dataSetFillPDF.Tables[14];
                    DataRow row = dataTableDirectedServicesPDF.Rows[0];

                    pdfFormFields.SetField("InterestToSelfDirectServices", row["InterestToSelfDirectServices"].ToString());
                    pdfFormFields.SetField("EducationOnSelfDirecting", row["EducationOnSelfDirecting"].ToString());
                    pdfFormFields.SetField("ServicesToSelfDirect", row["ServicesToSelfDirect"].ToString());
                    pdfFormFields.SetField("SkillAndResources", row["SkillsAndResources"].ToString());
                    pdfFormFields.SetField("IdentifyBarriersToSelfDirecting", row["IdentifyBarriersToSelfDirecting"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SelfDirectedPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "SelfDirectedPublished");
                    }

                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SelfDirectedPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "SelfDirectedPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private void fillTransitionPlanningPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableTransitionPlanning = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[16].Rows.Count > 0)
                {
                    dataTableTransitionPlanning = dataSetFillPDF.Tables[16];
                    DataRow row = dataTableTransitionPlanning.Rows[0];

                    pdfFormFields.SetField("Vocational", row["Vocational"].ToString());
                    pdfFormFields.SetField("VocationalDetails", row["VocationalDetails"].ToString());
                    pdfFormFields.SetField("PrevocationalSkills", row["PrevocationalSkills"].ToString());
                    pdfFormFields.SetField("PrevocationalSkillsDetails", row["PrevocationalSkillsDetails"].ToString());
                    pdfFormFields.SetField("HistoryOfEmployment", row["HistoryOfEmployment"].ToString());
                    pdfFormFields.SetField("HistoryOfEmploymentDetails", row["HistoryOfEmploymentDetails"].ToString());
                    pdfFormFields.SetField("AccessVR", row["AccessVR"].ToString());
                    pdfFormFields.SetField("AccessVRDetails", row["AccessVRDetails"].ToString());
                    pdfFormFields.SetField("AccessToVocationalRehabilitation", row["AccessToVocationalRehabilitation"].ToString());
                    pdfFormFields.SetField("AccessToVocationalRehabilitationDetails", row["AccessToVocationalRehabilitationDetails"].ToString());
                    pdfFormFields.SetField("TicketToWork", row["TicketToWork"].ToString());
                    pdfFormFields.SetField("TicketToWorkDetails", row["TicketToWorkDetails"].ToString());
                    pdfFormFields.SetField("WelfareToWork", row["WelfareToWork"].ToString());
                    pdfFormFields.SetField("WelfareToWorkDetails", row["WelfareToWorkDetails"].ToString());
                    pdfFormFields.SetField("DayActivitiesOverAge21", row["DayActivitiesOverAge21"].ToString());
                    pdfFormFields.SetField("DayActivitiesOverAge21Details", row["DayActivitiesOverAge21Details"].ToString());
                    pdfFormFields.SetField("CompetitiveEmployment", row["CompetitiveEmployment"].ToString());
                    pdfFormFields.SetField("CompetitiveEmploymentDetails", row["CompetitiveEmploymentDetails"].ToString());
                    pdfFormFields.SetField("SEMP", row["SEMP"].ToString());
                    pdfFormFields.SetField("SEMPDetails", row["SEMPDetails"].ToString());
                    pdfFormFields.SetField("EmploymentNotIntegrated", row["EmploymentNotIntegrated"].ToString());
                    pdfFormFields.SetField("EmploymentNotIntegratedDetails", row["EmploymentNotIntegratedDetails"].ToString());

                    pdfFormFields.SetField("PathwayToEmployment", row["PathwayToEmployment"].ToString());
                    pdfFormFields.SetField("PathwayToEmploymentDetails", row["PathwayToEmploymentDetails"].ToString());
                    pdfFormFields.SetField("SiteBasedPrevocationalServices", row["SiteBasedPrevocationalServices"].ToString());
                    pdfFormFields.SetField("SiteBasedPrevocationalServicesDetails", row["SiteBasedPrevocationalServicesDetails"].ToString());
                    pdfFormFields.SetField("CommunityBasedPrevocationalServices", row["CommunityBasedPrevocationalServices"].ToString());
                    pdfFormFields.SetField("CommunityBasedPrevocationalServicesDetails", row["CommunityBasedPrevocationalServicesDetails"].ToString());
                    pdfFormFields.SetField("SelfDirectedIndividualizedBudget", row["SelfDirectedIndividualizedBudget"].ToString());
                    pdfFormFields.SetField("SelfDirectedIndividualizedBudgetDetails", row["SelfDirectedIndividualizedBudgetDetails"].ToString());
                    pdfFormFields.SetField("DayHabilitation", row["DayHabilitation"].ToString());


                    pdfFormFields.SetField("DayHabilitationDetails", row["DayHabilitationDetails"].ToString());
                    pdfFormFields.SetField("DayHabilitationWithoutWalls", row["DayHabilitationWithoutWalls"].ToString());
                    pdfFormFields.SetField("DayHabilitationWithoutWallsDetails", row["DayHabilitationWithoutWallsDetails"].ToString());
                    pdfFormFields.SetField("DayTreatment", row["DayTreatment"].ToString());
                    pdfFormFields.SetField("DayTreatmentDetails", row["DayTreatmentDetails"].ToString());
                    pdfFormFields.SetField("CommunityHabilitation", row["CommunityHabilitation"].ToString());
                    pdfFormFields.SetField("CommunityHabilitationDetails", row["CommunityHabilitationDetails"].ToString());
                    pdfFormFields.SetField("CommunityFirstChoiceOption", row["CommunityFirstChoiceOption"].ToString());
                    pdfFormFields.SetField("CommunityFirstChoiceOptionDetails", row["CommunityFirstChoiceOptionDetails"].ToString());



                    pdfFormFields.SetField("MentalHealthProgram", row["MentalHealthProgram"].ToString());
                    pdfFormFields.SetField("MentalHealthProgramDetails", row["MentalHealthProgramDetails"].ToString());
                    pdfFormFields.SetField("AdultDayServices", row["AdultDayServices"].ToString());
                    pdfFormFields.SetField("AdultDayServicesDetails", row["AdultDayServicesDetails"].ToString());
                    pdfFormFields.SetField("Volunteer", row["Volunteer"].ToString());
                    pdfFormFields.SetField("VolunteerDetails", row["VolunteerDetails"].ToString());
                    pdfFormFields.SetField("Retired", row["Retired"].ToString());
                    pdfFormFields.SetField("RetiredDetails", row["RetiredDetails"].ToString());
                    pdfFormFields.SetField("NoStructuredDaytimeActivity", row["NoStructuredDaytimeActivity"].ToString());

                    pdfFormFields.SetField("NoStructuredDaytimeActivityDetails", row["NoStructuredDaytimeActivityDetails"].ToString());
                    pdfFormFields.SetField("AdultEducation", row["AdultEducation"].ToString());
                    pdfFormFields.SetField("AdultEducationDetails", row["AdultEducationDetails"].ToString());
                    pdfFormFields.SetField("InterestInServices", row["InterestInServices"].ToString());

                    pdfFormFields.SetField("InterestInServicesDetails", row["InterestInServicesDetails"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "TransitionPublsihed");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "TransitionPublsihed");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "TransitionPublsihed");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "TransitionPublsihed");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        private void fillGeneralSectionPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableGeneral = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[6].Rows.Count > 0)
                {
                    dataTableGeneral = dataSetFillPDF.Tables[6];
                    DataRow row = dataTableGeneral.Rows[0];

                    pdfFormFields.SetField("Notes", row["Notes"].ToString());
                    pdfFormFields.SetField("ElectronicSignature", row["ElectronicSignature"].ToString());
                    pdfFormFields.SetField("Date", row["Date"].ToString());
                    pdfFormFields.SetField("StaffName", row["StaffName"].ToString());
                    pdfFormFields.SetField("StaffTitle", row["StaffTitle"].ToString());
                    pdfFormFields.SetField("StaffCredentials", row["StaffCredentials"].ToString());
                    pdfFormFields.SetField("SignedDateTime", row["SignedDateTime"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "GeneralPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "GeneralPublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "GeneralPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "GeneralPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private void fillSafetyRiskAssessmentPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableRiskAssessment = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[13].Rows.Count > 0)
                {
                    dataTableRiskAssessment = dataSetFillPDF.Tables[13];
                    DataRow row = dataTableRiskAssessment.Rows[0];

                    pdfFormFields.SetField("EverThoughtsHurtingYourself", row["EverThoughtsHurtingYourself"].ToString());
                    pdfFormFields.SetField("EverThoughtsHurtingYourselfDetail", row["EverThoughtsHurtingYourselfDetail"].ToString());
                    pdfFormFields.SetField("EverActedThoughtsHurtYourself", row["EverActedThoughtsHurtYourself"].ToString());
                    pdfFormFields.SetField("EverActedThoughtsHurtYourselfDetail", row["EverActedThoughtsHurtYourselfDetail"].ToString());
                    pdfFormFields.SetField("FeelingWellOrNeedHelp", row["FeelingWellOrNeedHelp"].ToString());
                    pdfFormFields.SetField("ThoughtsOfHarmingYourself", row["ThoughtsOfHarmingYourself"].ToString());
                    pdfFormFields.SetField("ThoughtsOfHarmingYourselfDetail", row["ThoughtsOfHarmingYourselfDetail"].ToString());
                    pdfFormFields.SetField("HavePlanToEndYourLife", row["HavePlanToEndYourLife"].ToString());


                    pdfFormFields.SetField("PlanToEndYourLife", row["PlanToEndYourLife"].ToString());
                    pdfFormFields.SetField("NearFutureActOnThatPlan", row["NearFutureActOnThatPlan"].ToString());
                    pdfFormFields.SetField("NearFutureActOnThatPlanDetail", row["NearFutureActOnThatPlanDetail"].ToString());
                    pdfFormFields.SetField("EverAttemptedSuicideBefore", row["EverAttemptedSuicideBefore"].ToString());
                    pdfFormFields.SetField("EverAttemptedSuicideBeforeDetail", row["EverAttemptedSuicideBeforeDetail"].ToString());
                    pdfFormFields.SetField("EverCommittedSuicideInFamily", row["EverCommittedSuicideInFamily"].ToString());
                    pdfFormFields.SetField("EverCommittedSuicideInFamilyDetail", row["EverCommittedSuicideInFamilyDetail"].ToString());
                    pdfFormFields.SetField("FeltLikeHarmingYourself", row["FeltLikeHarmingYourself"].ToString());

                    pdfFormFields.SetField("FeltLikeHarmingYourselfDetail", row["FeltLikeHarmingYourselfDetail"].ToString());
                    pdfFormFields.SetField("HearVoicesToHarmOrKillYourself", row["HearVoicesToHarmOrKillYourself"].ToString());
                    pdfFormFields.SetField("HearVoicesToHarmOrKillYourselfDetail", row["HearVoicesToHarmOrKillYourselfDetail"].ToString());
                    pdfFormFields.SetField("FeelLikeThatActOnThoseVoices", row["FeelLikeThatActOnThoseVoices"].ToString());
                    pdfFormFields.SetField("FeelLikeThatActOnThoseVoicesDetail", row["FeelLikeThatActOnThoseVoicesDetail"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "RiskAssessmentPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "RiskAssessmentPublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "RiskAssessmentPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "RiskAssessmentPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        private void fillDepressionScreeningPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableGeneral = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[2].Rows.Count > 0)
                {
                    dataTableGeneral = dataSetFillPDF.Tables[2];
                    DataRow row = dataTableGeneral.Rows[0];

                    pdfFormFields.SetField("LittleInterestInDoingThings", row["LittleInterestInDoingThings"].ToString());
                    pdfFormFields.SetField("FeelingDownOrDepressed", row["FeelingDownOrDepressed"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());
                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DepressionPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "DepressionPublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "DepressionPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "DepressionPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private void fillSubstanceAbuseScreeningPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableSubstanceAbuseScreen = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[17].Rows.Count > 0)
                {
                    dataTableSubstanceAbuseScreen = dataSetFillPDF.Tables[17];
                    DataRow row = dataTableSubstanceAbuseScreen.Rows[0];

                    pdfFormFields.SetField("UsedAlcohalOrOtherDrugs", row["UsedAlcohalOrOtherDrugs"].ToString());
                    pdfFormFields.SetField("UsedPrescriptionOrMedication_Drugs", row["UsedPrescriptionOrMedication_Drugs"].ToString());
                    pdfFormFields.SetField("FeltUseTooMuchAlcoholOrOther_Drugs", row["FeltUseTooMuchAlcoholOrOther_Drugs"].ToString());
                    pdfFormFields.SetField("TriedToCutDown_QuitDrinking_OtherDrug", row["TriedToCutDown_QuitDrinking_OtherDrug"].ToString());
                    pdfFormFields.SetField("GoneToAnyoneForHelpBcozOfYourDrinking", row["GoneToAnyoneForHelpBcozOfYourDrinking"].ToString());
                    pdfFormFields.SetField("BlackoutsOrOtherPeriods", row["BlackoutsOrOtherPeriods"].ToString());
                    pdfFormFields.SetField("InjuredHeadAfterDrinking", row["InjuredHeadAfterDrinking"].ToString());
                    pdfFormFields.SetField("Convulsions_Delirium_Tremens", row["Convulsions_Delirium_Tremens"].ToString());


                    pdfFormFields.SetField("HepatitisOrOtherLiverProblems", row["HepatitisOrOtherLiverProblems"].ToString());
                    pdfFormFields.SetField("Felt_sick_shakyOrDepressed", row["Felt_sick_shakyOrDepressed"].ToString());
                    pdfFormFields.SetField("FeltCoke_BugsOrCrawlingFeeling", row["FeltCoke_BugsOrCrawlingFeeling"].ToString());
                    pdfFormFields.SetField("InjuredAfterDrinkingOrUsing", row["InjuredAfterDrinkingOrUsing"].ToString());
                    pdfFormFields.SetField("UsedNeedlesToShootDrugs", row["UsedNeedlesToShootDrugs"].ToString());
                    pdfFormFields.SetField("DrinkingUseCausedProblemsWithYourFamily", row["DrinkingUseCausedProblemsWithYourFamily"].ToString());
                    pdfFormFields.SetField("DrinkingUseCausedProblemsAtSchool_Work", row["DrinkingUseCausedProblemsAtSchool_Work"].ToString());
                    pdfFormFields.SetField("ArrestedOrOtherLegalProblems", row["ArrestedOrOtherLegalProblems"].ToString());

                    pdfFormFields.SetField("LostTemper_FightsWhileDrinking", row["LostTemper_FightsWhileDrinking"].ToString());
                    pdfFormFields.SetField("NeedingToDrinkOrUseDrugsMore", row["NeedingToDrinkOrUseDrugsMore"].ToString());
                    pdfFormFields.SetField("TryingToGetAlcoholOrDrugs", row["TryingToGetAlcoholOrDrugs"].ToString());
                    pdfFormFields.SetField("BreakRules_Laws", row["BreakRules_Laws"].ToString());
                    pdfFormFields.SetField("FeelBadOrGuilty", row["FeelBadOrGuilty"].ToString());
                    pdfFormFields.SetField("EverDrinkingOrOtherDrugProblem", row["EverDrinkingOrOtherDrugProblem"].ToString());

                    pdfFormFields.SetField("FamilyMembersEverDrinkingOrDrugProblem", row["FamilyMembersEverDrinkingOrDrugProblem"].ToString());
                    pdfFormFields.SetField("FeelDrinkingOrDrugProblemNow", row["FeelDrinkingOrDrugProblemNow"].ToString());
                    pdfFormFields.SetField("TotalScore", row["TotalScore"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "AbusePublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "AbusePublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "AbusePublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "AbusePublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private void fillSafetyPlanPDF(AcroFields pdfFormFields, DataSet dataSetFillPDF, string status)
        {
            DataTable dataTableSafetyPlan = null;

            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[12].Rows.Count > 0)
                {
                    dataTableSafetyPlan = dataSetFillPDF.Tables[12];
                    DataRow row = dataTableSafetyPlan.Rows[0];

                    pdfFormFields.SetField("LeavePerpetrator", row["LeavePerpetrator"].ToString());
                    pdfFormFields.SetField("OrderProtection", row["OrderProtection"].ToString());
                    pdfFormFields.SetField("OrderProtectionDetail", row["OrderProtectionDetail"].ToString());
                    pdfFormFields.SetField("ChildrenInvolved", row["ChildrenInvolved"].ToString());
                    pdfFormFields.SetField("ChildrenBeingAbused", row["ChildrenBeingAbused"].ToString());
                    pdfFormFields.SetField("EverBeenAbused", row["EverBeenAbused"].ToString());
                    pdfFormFields.SetField("SafewayToLeave", row["SafewayToLeave"].ToString());
                    pdfFormFields.SetField("LeaveHomeExit", row["LeaveHomeExit"].ToString());


                    pdfFormFields.SetField("IsMemberTrust_LeaveDocument", row["IsMemberTrust_LeaveDocument"].ToString());
                    pdfFormFields.SetField("ImpDocumenthas", row["ImpDocumenthas"].ToString());
                    pdfFormFields.SetField("ImpDocumenthas_Address", row["ImpDocumenthas_Address"].ToString());
                    pdfFormFields.SetField("IsSafePlace_PersonToTakecareThings", row["IsSafePlace_PersonToTakecareThings"].ToString());
                    pdfFormFields.SetField("LeaveMyhomewillContact", row["LeaveMyhomewillContact"].ToString());
                    pdfFormFields.SetField("IsChildrenKnowToDial911", row["IsChildrenKnowToDial911"].ToString());
                    pdfFormFields.SetField("DirectionsOfMembersAddress", row["DirectionsOfMembersAddress"].ToString());
                    pdfFormFields.SetField("IsSafePlaceInHomeToHide", row["IsSafePlaceInHomeToHide"].ToString());

                    pdfFormFields.SetField("SafePlaceInHome", row["SafePlaceInHome"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                    bool blankRow = false;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SafetyPlanPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, row["Status"].ToString(), "SafetyPlanPublished");
                    }
                }
                else
                {
                    bool blankRow = true;
                    if (status == "Published")
                    {
                        ShowPublishedStatus(pdfFormFields, blankRow, "SafetyPlanPublished");
                    }
                    else
                    {
                        showDrafStatus(pdfFormFields, blankRow, status, "SafetyPlanPublished");
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        private void ShowPublishedStatus(AcroFields pdfFormFields,bool blankSection,string pdfId)
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
        private void showDrafStatus(AcroFields pdfFormFields, bool blankSection,string status, string pdfId)
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

        public async Task<ComprehensiveAssessmentDetailResponse> UploadOfflinePDF(string json)
        {
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            comprehensiveAssessmentPDFResponse = new ComprehensiveAssessmentPDFResponse();
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_UploadOfflinePDF", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = json;
                        //cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = Convert.ToInt32(0);
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
                    comprehensiveAssessmentDetailResponse.AllTabsComprehensiveAssessment = JsonConvert.DeserializeObject<List<AllTabsComprehensiveAssessment>>(dataSetString);
                }
                return comprehensiveAssessmentDetailResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

    }
}
