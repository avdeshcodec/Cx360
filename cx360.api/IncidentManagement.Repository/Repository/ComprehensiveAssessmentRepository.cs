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
using static IncidentManagement.Entities.Response.CCOComprehensiveAssessmentResponse;
using IncidentManagement.Repository.Common;

namespace IncidentManagement.Repository.Repository
{
  public  class ComprehensiveAssessmentRepository:IComprehensiveAssessmentRepository
    {
        #region Private
        ComprehensiveAssessmentDetailResponse comprehensiveAssessmentDetailResponse = null;
        CCOComprehensiveAssessmentDetailResponse cCOComprehensiveAssessmentDetailResponse = null;
        ComprehensiveAssessmentPDFResponse comprehensiveAssessmentPDFResponse = null;
        CCOComprehensiveAssessmentResponse cCOComprehensiveAssessmentResponse = null;
        CCOComprehensivePDFResponse CCOComprehensivePDFResponse = null;
        #endregion

        public async Task<ComprehensiveAssessmentDetailResponse> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {

            bool checkVersion = true;

            if (comprehensiveAssessmentRequest.TabName == "ComprehensiveAssessment")
            {
                checkVersion =await ValidateComprehensiveAssessmentDraft(comprehensiveAssessmentRequest, companyId);
            }
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                if (checkVersion)
                {
                    using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
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


        public async Task<bool> ValidateComprehensiveAssessmentDraft(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            bool recordValidated = true;

            DataTable dataSet = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ValidateComprehensiveAssessmentDraft", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@json", SqlDbType.VarChar).Value = comprehensiveAssessmentRequest.Json;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = comprehensiveAssessmentRequest.ReportedBy;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
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

        public async Task<ComprehensiveAssessmentDetailResponse> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            string sp = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
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
        public async Task<ComprehensiveAssessmentDetailResponse> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
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

        public async Task<ComprehensiveAssessmentPDFResponse> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            comprehensiveAssessmentPDFResponse = new ComprehensiveAssessmentPDFResponse();
            string pdfTemplate = CommonFunctions.GetFillablePDFPath(comprehensiveAssessmentRequest.TabName);
            string newTemplatePDf = string.Empty;
            try
            {
                newTemplatePDf =await GetAssessmentPDFTemplate(comprehensiveAssessmentRequest.TabName, ConfigurationManager.AppSettings["FillablePDF"].ToString() + "Comprehensive_Assessment.pdf", comprehensiveAssessmentRequest, companyId);

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

        private async Task<string> GetAssessmentPDFTemplate(string tabName, string pdfPath, ComprehensiveAssessmentRequest fillablePDFRequest, string companyId)
        {
            DataSet dataSet = new DataSet();
            string newpdfPath = string.Empty;
            try
            {
                string storeProcedure = CommonFunctions.GetMappedStoreProcedure(tabName);
                using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetComprehensiveAssessmentDetails", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add("@comprehensiveAssessmentId", SqlDbType.Int).Value = fillablePDFRequest.ComprehensiveAssessmentId;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
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

        public async Task<ComprehensiveAssessmentDetailResponse> UploadOfflinePDF(string json, string companyId)
        {
            comprehensiveAssessmentDetailResponse = new ComprehensiveAssessmentDetailResponse();
            comprehensiveAssessmentPDFResponse = new ComprehensiveAssessmentPDFResponse();
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
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
        public async Task<CCOComprehensiveAssessmentDetailResponse> GetCCOComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            cCOComprehensiveAssessmentDetailResponse = new CCOComprehensiveAssessmentDetailResponse;
            string storeProcedure = CommonFunctions.GetMappedStoreProcedure(comprehensiveAssessmentRequest.TabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(await ConnectionString.GetConnectionString(companyId)))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand("usp_GetCCOComprehensiveAssessmentDetails", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@compassessmentid", SqlDbType.Int).Value = comprehensiveAssessmentRequest.CompAssessmentId;
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
                    //var json = JsonConvert.DeserializeObject(dataSetString);
                    cCOComprehensiveAssessmentDetailResponse = JsonConvert.DeserializeObject<CCOComprehensiveAssessmentDetailResponse>(dataSetString);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return cCOComprehensiveAssessmentDetailResponse;
        }

        public async Task<CCOComprehensiveAssessmentDetailResponse> HandleCCOComprehensiveAssessmentVersioning(CCOComprehensiveAssessmentRequest cCOComprehensiveAssessmentRequest)
        {
            cCOComprehensiveAssessmentDetailResponse = new CCOComprehensiveAssessmentDetailResponse();
            string tabName = cCOComprehensiveAssessmentRequest.TabName;
            string sp = CommonFunctions.GetMappedStoreProcedure(tabName);
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(sp, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@compAssessmentId", SqlDbType.VarChar).Value = cCOComprehensiveAssessmentRequest.CompAssessmentId;
                        cmd.Parameters.Add("@documentversionid", SqlDbType.VarChar).Value = cCOComprehensiveAssessmentRequest.CompAssessmentVersioningId;
                        cmd.Parameters.Add("@reportedby", SqlDbType.Int).Value = cCOComprehensiveAssessmentRequest.ReportedBy;
                        cmd.Parameters.Add("@mode", SqlDbType.VarChar).Value = cCOComprehensiveAssessmentRequest.Mode;

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
                    cCOComprehensiveAssessmentDetailResponse.CCOComprehensiveAssessmentResponse = JsonConvert.DeserializeObject<List<CCOComprehensiveAssessmentResponse>>(dataSetString);
                    //}
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return cCOComprehensiveAssessmentDetailResponse;
        }


        
        public async Task<CCOComprehensivePDFResponse> FillableCCOComprehensiveAssessmentPDF(FillableCCOComprehensiveAssessmentPDFRequest fillableCCOComprehensiveAssessmentPDFRequest)
        {
            DataSet dataSet = new DataSet();
            string newpdfPath = string.Empty;
            string pdfTemplate = CommonFunctions.GetFillablePDFPath(fillableCCOComprehensiveAssessmentPDFRequest.TabName);
            DataTable dataTablePath = null;
            try
            {
                string storeProcedure = CommonFunctions.GetMappedStoreProcedure(fillableCCOComprehensiveAssessmentPDFRequest.TabName);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"].ToString()))
                {
                    //Create the SqlCommand object
                    using (SqlCommand cmd = new SqlCommand(storeProcedure, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@@ccoComprehensiveAssessmentId", SqlDbType.Int).Value = fillableCCOComprehensiveAssessmentPDFRequest.CompAssessmentId;
                        con.Open();

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {

                    dataTablePath = CCOComprehensiveAssessmentPDFTemplate(pdfTemplate, dataSet, fillableCCOComprehensiveAssessmentPDFRequest, fillableCCOComprehensiveAssessmentPDFRequest.TabName);

                }
                string dataSetString = CommonFunctions.ConvertDataTableToJson(dataTablePath);
                CCOComprehensivePDFResponse.CCOComprehensiveAssessmentPDF = JsonConvert.DeserializeObject<List<CCOComprehensiveAssessmentPDF>>(dataSetString);
                return CCOComprehensivePDFResponse;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        private DataTable CCOComprehensiveAssessmentPDFTemplate(string pdfPath, DataSet dataSetFillPDF, FillableCCOComprehensiveAssessmentPDFRequest fillablePDFRequest, string tabName)
        {
            string newFile = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "CCO_ComprehensiveAssessmentPDF.pdf";
            string newFile1 = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed1_lifeplan.pdf";
            string finaldoc = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed2_lifeplan.pdf";
            string finaldoc1 = ConfigurationManager.AppSettings["FillablePDF"].ToString() + "completed3_lifeplan.pdf";
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(pdfPath);
            // iTextSharp.text.pdf.PdfReader pdfReader8 = new iTextSharp.text.pdf.PdfReader(ConfigurationManager.AppSettings["DocumentFile"].ToString() + "form_Requirements_Updates_Changes_ForDev.docx");

            DataTable dataTable = new DataTable();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));
            try
            {

                DataTable CircleAndSupportTable = dataSetFillPDF.Tables[0];
                DataTable GuardianshipAndAdvocacyTable = dataSetFillPDF.Tables[1];
                DataTable MedicalHealthTable = dataSetFillPDF.Tables[2];
                DataTable MedicationsTable = dataSetFillPDF.Tables[3];

                //Create pdfTable instance and  paas column numbers as parameters.
                PdfPTable tableCircleAndSupport = new PdfPTable(4);
                PdfPTable tableGuardianshipAndAdvocacy = new PdfPTable(9);
                PdfPTable tableMedicalHealth = new PdfPTable(8);
                PdfPTable tableMedications = new PdfPTable(4);

                tableCircleAndSupport.WidthPercentage = 100f;
                tableGuardianshipAndAdvocacy.WidthPercentage = 100f;
                tableMedicalHealth.WidthPercentage = 100f;
                tableMedications.WidthPercentage = 100f;

                iTextSharp.text.Font fntTableFontHdr = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fntTableFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font pageTextFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);

                AcroFields pdfFormFields = pdfStamper.AcroFields;
                pdfFormFields.GenerateAppearances = false;

                fillCCOComprehensiveAssessmentSection(pdfFormFields, dataSetFillPDF);
                fillEligibiltyInformationSection(pdfFormFields, dataSetFillPDF);
                fillCommunication_LanguageSection(pdfFormFields, dataSetFillPDF);
                fillMemberProviderSection(pdfFormFields, dataSetFillPDF);
                //fillCircleAndSupportSection(pdfFormFields, dataSetFillPDF);
                fillGuardianshipAndAdvocacySection(pdfFormFields, dataSetFillPDF);
                fillAdvancedDirectivesFuturePlanningSection(pdfFormFields, dataSetFillPDF);
                fillIndependentLivingSkillsSection(pdfFormFields, dataSetFillPDF);
                fillSocialServiceNeedsSection(pdfFormFields, dataSetFillPDF);
                fillMedicalHealthSection(pdfFormFields, dataSetFillPDF);
                fillHealthPromotionSection(pdfFormFields, dataSetFillPDF);
                fillBehavioralHealthSection(pdfFormFields, dataSetFillPDF);
                fillChallengingBehaviorsSection(pdfFormFields, dataSetFillPDF);
                fillBehavioralSupportPlanSection(pdfFormFields, dataSetFillPDF);
                fillMedicationsSection(pdfFormFields, dataSetFillPDF);
                fillCommumunitySocialParticipationSection(pdfFormFields, dataSetFillPDF);
                fillEducationSection(pdfFormFields, dataSetFillPDF);
                fillTransitionPlanningSection(pdfFormFields, dataSetFillPDF);
                fillEmploymentSection(pdfFormFields, dataSetFillPDF);



                //DataRow row = Lifeplan.Rows[0];

                //pdfFormFields.SetField("IndividualName", fillablePDFRequest.IndividualName);
                //pdfFormFields.SetField("DateOfBirth", fillablePDFRequest.DateOfBirth);
                //pdfFormFields.SetField("MemberAddress", row["MemberAddress"].ToString() + ' ' + fillablePDFRequest.AddressLifePlan);
                //pdfFormFields.SetField("Phone", row["Phone"].ToString());
                //pdfFormFields.SetField("Medicaid", row["Medicaid"].ToString());
                //pdfFormFields.SetField("Medicare", row["Medicare"].ToString());
                //pdfFormFields.SetField("EffectiveFromDate", row["EffectiveFromDate"].ToString());
                //pdfFormFields.SetField("EffectiveToDate", row["EffectiveToDate"].ToString());
                //pdfFormFields.SetField("EnrollmentDate", row["EnrollmentDate"].ToString());
                //pdfFormFields.SetField("WillowbookerMember", row["WillowbrookMember"].ToString());
                //pdfFormFields.SetField("AddressCCO", row["AddressCCO"].ToString() + ' ' + fillablePDFRequest.AddressCCO);
                //pdfFormFields.SetField("PhoneCCO", row["PhoneCCO"].ToString());
                //pdfFormFields.SetField("Fax", row["Fax"].ToString());
                //pdfFormFields.SetField("ProviderID", row["ProviderID"].ToString());
                //pdfFormFields.SetField("Status", row["DocumentStatus"].ToString());
                //pdfFormFields.SetField("Version", row["DocumentVersion"].ToString());
                //pdfFormFields.SetField("CareManagerFirstName", row["CareManagerFirstName"].ToString());
                //pdfFormFields.SetField("CareManagerLastName", row["CareManagerLastName"].ToString());
                //pdfFormFields.SetField("IncludeDurableMediEquipment", row["IncludeDurableMediEquipment"].ToString() == "Y" ? "Yes" : "No");
                //pdfFormFields.SetField("IncludeDiagnosis", row["IncludeDiagnosis"].ToString() == "Y" ? "Yes" : "No");
                //pdfFormFields.SetField("IncludeAllergies", row["IncludeAllergies"].ToString() == "Y" ? "Yes" : "No");
                //pdfFormFields.SetField("IncludeMedications", row["IncludeMedications"].ToString() == "Y" ? "Yes" : "No");
                //pdfFormFields.SetField("LifePlanType", row["LifePlanType"].ToString());





                //PdfPCell cell = new PdfPCell(new Phrase("Meeting History")) { PaddingBottom = 10 };
                //cell.Colspan = 4;
                //cell.BackgroundColor = new BaseColor(204, 204, 204);
                //cell.HorizontalAlignment = 1;
                //table.SpacingBefore = 30f;


                //if (MeetingHistory.Rows.Count > 0)
                //{
                //    for (var i = 0; i < MeetingHistory.Rows.Count; i++)
                //    {
                //        table.AddCell(cell);
                //        table.AddCell(new PdfPCell(new Phrase("Note Type")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //        table.AddCell(new PdfPCell(new Phrase("Event Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //        table.AddCell(new PdfPCell(new Phrase("Subject")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //        table.AddCell(new PdfPCell(new Phrase("Meeting Reason")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //        DataRow rowMeetingHistory = MeetingHistory.Rows[i];
                //        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["TypeOfMeeting"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["PlanerReviewDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["MeetingReason"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        table.AddCell(new PdfPCell(new Phrase(rowMeetingHistory["MemberAttendance"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                //        var filtered = new List<JSONData>();

                //        foreach (var e in fillablePDFRequest.JSONData)
                //        {
                //            if (e.meetingAttendanceId == Convert.ToInt32(rowMeetingHistory["MeetingId"]))
                //            {
                //                filtered.Add(e);
                //            }
                //        }
                //        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Member Attendance")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, PaddingBottom = 10 });
                //        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Contact Name")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Relationship To Member")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //        tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("Method")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //        if (filtered.Count > 0)
                //        {
                //            foreach (var e in filtered)
                //            {
                //                if (e.meetingAttendanceId == Convert.ToInt32(rowMeetingHistory["MeetingId"]))
                //                {
                //                    tableMeetingAttendance.AddCell(new PdfPCell(new Phrase(e.ContactName.ToString(), fntTableFont)) { PaddingBottom = 10 });
                //                    tableMeetingAttendance.AddCell(new PdfPCell(new Phrase(e.RelationshipToMember.ToString(), fntTableFont)) { PaddingBottom = 10 });
                //                    tableMeetingAttendance.AddCell(new PdfPCell(new Phrase(e.Method.ToString(), fntTableFont)) { PaddingBottom = 10 });
                //                }
                //            }
                //        }
                //        else
                //        {
                //            tableMeetingAttendance.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                //        }
                //        table.AddCell(new PdfPCell(tableMeetingAttendance) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10, PaddingLeft = 10, PaddingRight = 10, PaddingTop = 10 });
                //        table.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.TOP_BORDER });
                //        filtered = null;
                //        tableMeetingAttendance.DeleteBodyRows();
                //    }
                //}
                //else
                //{
                //    table.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}

                //DataRow rowAssessmentNarrativeSummary = null;
                //if (AssessmentNarrativeSummary.Rows.Count > 0)
                //{
                //    rowAssessmentNarrativeSummary = AssessmentNarrativeSummary.Rows[0];
                //}
                //tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Section I")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("ASSESSMENT NARRATIVE SUMMARY")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("This section includes relevant personal history and appropriate contextual information, as well as skills, abilities, aspirations, needs, interests, reasonable accommodations, cultural considerations, meaningful activities, challenges, etc., learned during the person - centered planning process, record review and any assessments reviewed and / or completed.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });

                //if (AssessmentNarrativeSummary.Rows.Count > 0)
                //{
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Introducing Me :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["IntroducingMe"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Home :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyHome"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Let Me Tell You About My Day :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["TellYouAboutMyDay"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Health and My Medications :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyHealthAndMedication"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Relationship :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyRelationships"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Happiness :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MyHappiness"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My School/ Learning :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase(rowAssessmentNarrativeSummary["MySchool"].ToString(), fntTableFont)) { PaddingBottom = 10 });



                //}
                //else
                //{
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Introducing Me :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Home :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("Let Me Tell You About My Day :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Health and My Medications :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Relationship :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My Happiness :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("My School/ Learning :")) { PaddingBottom = 10 });
                //    tableAssessmentNarrativeSummary.AddCell(new PdfPCell(new Phrase("", fntTableFont)) { PaddingBottom = 10 });
                //    //tableAssessmentNarrativeSummary.AddCell("");
                //}


                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Section II")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("OUTCOMES AND SUPPORT STRATEGIES")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("This section includes measurable/observable personal outcomes that are developed by the person and his/her IDT using person-centered planning. It describes provider goals and corresponding staff activities identified to meet the CCO goal / valued outcome.It captures the following information: goal description, valued outcomes, action steps, responsible party, service type, timeframe for action steps and Personal Outcome Measures.Evidence of achievement must be reflected in monthly notes from assigned providers.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("CQL POMS Goal/Valued OutCome", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("CCO Goal/Valued OutCome", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Provider Assigned Goal", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Provider / Location", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Service Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Frequency", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Quantity", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Time Frame", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("Special Considerations", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //if (Outcomes_SupportStrategies.Rows.Count > 0)
                //{
                //    for (var i = 0; i < Outcomes_SupportStrategies.Rows.Count; i++)
                //    {
                //        DataRow rowOutcomes_SupportStrategies = Outcomes_SupportStrategies.Rows[i];
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["CqlPomsGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["CcoGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["ProviderAssignedGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["ProviderLocation"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["ServicesType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["Frequency"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["Quantity"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["TimeFrame"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase(rowOutcomes_SupportStrategies["SpecialConsiderations"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    }
                //}
                //else
                //{
                //    tableOutcomes_SupportStrategies.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 9, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}



                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Section III")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 8, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Individual Safeguards/Individual Plan of Protection (IPOP)")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 8, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Compilation of all supports and services needed for a person to remain safe, healthy and comfortable across all settings (including Part 686 requirements for IPOP).This section details the provider goals and corresponding staff activities required to maintain desired personal safety")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 8, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Goal Valued Outcome")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Provider Assigned Goal")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Provider/Location")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Service Type")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Frequency")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Quantity")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Time Frame")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("Special Considerations")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //if (IndividualPlanOfProtection.Rows.Count > 0)
                //{
                //    for (var i = 0; i < IndividualPlanOfProtection.Rows.Count; i++)
                //    {
                //        DataRow rowIndividualPlanOfProtection = IndividualPlanOfProtection.Rows[i];
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["GoalValuedOutcome"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["ProviderAssignedGoal"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["ProviderLocation"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["ServicesType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["Frequency"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["Quantity"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["TimeFrame"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase(rowIndividualPlanOfProtection["SpecialConsiderations"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                //    }
                //}
                //else
                //{
                //    tableIndividualPlanOfProtection.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 8, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}


                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Section IV")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 5, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("HCBS Wavier and Medicaid State Plan Authorized Services")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 5, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("This section of the Life Plan includes a listing of all HCBS Waiver and State Plan services that have been authorized for the individual. ")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 5, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Authorized Service")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Provider")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Effective Dates")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Unit Of Measure")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("Comments")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //if (MedicaidStatePlanAuthorizedServies.Rows.Count > 0)
                //{
                //    for (var i = 0; i < MedicaidStatePlanAuthorizedServies.Rows.Count; i++)
                //    {
                //        DataRow rowMedicaidStatePlanAuthorizedServies = MedicaidStatePlanAuthorizedServies.Rows[i];
                //        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["AuthorizedService"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["Provider"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["EffectiveDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["UnitOfMeasure"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase(rowMedicaidStatePlanAuthorizedServies["Comments"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                //    }
                //}
                //else
                //{
                //    tableMedicaidStatePlanAuthorizedServies.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 5, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}


                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Section V")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("All Suports and Services; Funded and Natural/Community Resources")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("This section identifies the services and support givers in a person’s life along with the needed contact information. Additionally, all Natural Supports and Community Resources that help the person be a valued individual of his or her community and live successfully on a day - to - day basis at home, at work, at school, or in other community locations should be listed with contact information as appropriate.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Contact Type")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Relationship")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Name")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("Orginization")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //if (FundalNaturalCommunityResources.Rows.Count > 0)
                //{
                //    for (var i = 0; i < FundalNaturalCommunityResources.Rows.Count; i++)
                //    {
                //        DataRow rowFundalNaturalCommunityResources = FundalNaturalCommunityResources.Rows[i];
                //        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["ContactType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["Relationship"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["Name"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase(rowFundalNaturalCommunityResources["Orginization"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                //    }
                //}
                //else
                //{
                //    tableFundalNaturalCommunityResources.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 4, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}


                //DataRow rowMemberRight = null;
                //if (MemberRights.Rows.Count > 0)
                //{
                //    rowMemberRight = MemberRights.Rows[0];
                //}
                ////tableMemberRights.AddCell(new PdfPCell(new Phrase("Section VI")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableMemberRights.AddCell(new PdfPCell(new Phrase("Member Rights")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, PaddingBottom = 10 });
                //tableMemberRights.AddCell(new PdfPCell(new Phrase("My Care Manager has informed me of:")) { Colspan = 9, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });




                //if (MemberRights.Rows.Count > 0)
                //{
                //    tableMemberRights.AddCell(new PdfPCell(new Phrase("My rights under the Americans With Disabilities Act(ADA) : " + rowMemberRight["RightsUnderAmericansDisabilitiesAct"].ToString(), fntTableFont)) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //    tableMemberRights.AddCell(new PdfPCell(new Phrase("How to obtain reasonable accommodations (my reasonable accommodations are listed in my Life Plan) : " + rowMemberRight["Provider"].ToString(), fntTableFont)) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //    tableMemberRights.AddCell(new PdfPCell(new Phrase(" How to file a grievance or an appeal : " + rowMemberRight["GrievanceAppeal"].ToString(), fntTableFont)) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //}
                //else
                //{
                //    tableMemberRights.AddCell(new PdfPCell(new Phrase("My rights under the Americans With Disabilities Act(ADA) : ")) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //    tableMemberRights.AddCell(new PdfPCell(new Phrase("How to obtain reasonable accommodations (my reasonable accommodations are listed in my Life Plan) :")) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, PaddingBottom = 10 });
                //    tableMemberRights.AddCell(new PdfPCell(new Phrase("How to file a grievance or an appeal : ")) { Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                //}



                ////tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Section VII")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Member Representative Approval")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Member Name")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Member Approval Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Representative")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Representative Approval Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Committee Approver")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("Committee Approval Date")) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //if (MemberRepresentativeApproval.Rows.Count > 0)
                //{
                //    for (var i = 0; i < MemberRepresentativeApproval.Rows.Count; i++)
                //    {
                //        DataRow rowMemberRepresentativeApproval = MemberRepresentativeApproval.Rows[i];
                //        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["MemberName"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["MemberApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["Representative"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["RepresentativeApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["CommitteeApprover"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase(rowMemberRepresentativeApproval["CommitteeApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });

                //    }
                //}
                //else
                //{
                //    tableMemberRepresentativeApproval.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}


                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Section VI")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Acknowledgement and Agreements")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, PaddingBottom = 10 });
                ////tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("This section includes measurable/observable personal outcomes that are developed by the person and his/her IDT using person-centered planning. It describes provider goals and corresponding staff activities identified to meet the CCO goal / valued outcome.It captures the following information: goal description, valued outcomes, action steps, responsible party, service type, timeframe for action steps and Personal Outcome Measures.Evidence of achievement must be reflected in monthly notes from assigned providers.")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 9, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Notification Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Provider", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Notification Reason", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Notification Type", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Acknowledge and Agree Status", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("Aceptance / Acknowledgement Date", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //if (AcknowledgementAndAgreement.Rows.Count > 0)
                //{
                //    for (var i = 0; i < AcknowledgementAndAgreement.Rows.Count; i++)
                //    {
                //        DataRow rowAcknowledgementAndAgreement = AcknowledgementAndAgreement.Rows[i];
                //        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["Provider"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationReason"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationType"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["NotificationAckAgreeStatus"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase(rowAcknowledgementAndAgreement["AceptanceAcknowledgementDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    }
                //}
                //else
                //{
                //    tableAcknowledgementAndAgreement.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 6, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}

                ////tableDocuments.AddCell(new PdfPCell(new Phrase("Section IX")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 2, HorizontalAlignment = 1, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, PaddingBottom = 10 });
                //tableDocuments.AddCell(new PdfPCell(new Phrase("Documents")) { BackgroundColor = new BaseColor(204, 204, 204), Colspan = 2, HorizontalAlignment = 1, PaddingBottom = 10 });
                //tableDocuments.AddCell(new PdfPCell(new Phrase("Document Title", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });
                //tableDocuments.AddCell(new PdfPCell(new Phrase("Attach Document", fntTableFontHdr)) { BackgroundColor = new BaseColor(204, 204, 204), PaddingBottom = 10 });

                //if (Documents.Rows.Count > 0)
                //{
                //    for (var i = 0; i < Documents.Rows.Count; i++)
                //    {
                //        DataRow rowDocument = Documents.Rows[i];
                //        tableDocuments.AddCell(new PdfPCell(new Phrase(rowDocument["MemberName"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //        tableDocuments.AddCell(new PdfPCell(new Phrase(rowDocument["MemberApprovalDate"].ToString(), fntTableFont)) { PaddingBottom = 10 });
                //    }
                //}
                //else
                //{
                //    tableDocuments.AddCell(new PdfPCell(new Phrase("No Records")) { Colspan = 2, HorizontalAlignment = 1, PaddingBottom = 10 });
                //}


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
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(newFile, FileMode.Create));
                iTextSharp.text.Font mainFont = new iTextSharp.text.Font();
                iTextSharp.text.Font boldFont = new iTextSharp.text.Font();
                mainFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
                boldFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
                doc.Open();

                //table.SpacingAfter = 30;
                //tableAssessmentNarrativeSummary.SpacingAfter = 30;
                //tableFundalNaturalCommunityResources.SpacingAfter = 30;
                //tableIndividualPlanOfProtection.SpacingAfter = 30;
                //tableMedicaidStatePlanAuthorizedServies.SpacingAfter = 30;
                //tableOutcomes_SupportStrategies.SpacingAfter = 30;
                //tableMemberRepresentativeApproval.SpacingAfter = 30;
                //tableAcknowledgementAndAgreement.SpacingAfter = 30;
                //tableDocuments.SpacingAfter = 30;
               // tableMemberRights.SpacingAfter = 30;






                //doc.Add(table);
                //doc.Add(tableMeetingAttendance);
                //doc.Add(tableAssessmentNarrativeSummary);
                //doc.Add(tableOutcomes_SupportStrategies);
                //doc.Add(tableIndividualPlanOfProtection);
                //doc.Add(tableMedicaidStatePlanAuthorizedServies);
                //doc.Add(tableFundalNaturalCommunityResources);
                //doc.Add(tableMemberRights);
                //doc.Add(tableMemberRepresentativeApproval);
                //doc.Add(tableAcknowledgementAndAgreement);
                //doc.Add(tableDocuments);
                doc.Close();

                //FileStream fs = new FileStream(finaldoc, FileMode.Create);
                //iTextSharp.text.pdf.PdfReader readerNewFile = new iTextSharp.text.pdf.PdfReader(newFile);
                //iTextSharp.text.pdf.PdfReader readerNewFile1 = new iTextSharp.text.pdf.PdfReader(newFile1);

                //using (Document document = new Document())
                //using (PdfCopy copy = new PdfCopy(document, fs))
                //{
                //    document.Open();
                //    copy.AddDocument(readerNewFile);
                //    copy.AddDocument(readerNewFile1);
                //    document.Close();
                //    readerNewFile.Dispose();
                //    readerNewFile.Dispose();
                //    readerNewFile1.Close();
                //    readerNewFile1.Close();
                //    fs.Dispose();
                //    fs.Close();

                //}
                //// for paging and date stamp
                //FileStream fs1 = new FileStream(finaldoc1, FileMode.Create);
                //iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(newFile);
                //iTextSharp.text.pdf.PdfReader fsreader1 = new iTextSharp.text.pdf.PdfReader(newFile1);

                //using (Document document1 = new Document())
                //using (PdfCopy copy1 = new PdfCopy(document1, fs1))
                //{
                //    document1.Open();
                //    copy1.AddDocument(reader);
                //    copy1.AddDocument(fsreader1);
                //    document1.Close();
                //    reader.Dispose();
                //    fsreader1.Dispose();
                //    reader.Close();
                //    fsreader1.Close();
                //    fs1.Dispose();
                //    fs1.Close();
                //}
                //iTextSharp.text.pdf.PdfReader pdfReader1 = new iTextSharp.text.pdf.PdfReader(finaldoc1);
                //PdfStamper pdfStamper1 = new PdfStamper(pdfReader1, new FileStream(finaldoc, FileMode.Create));
                //for (int i = 1; i <= pdfReader1.NumberOfPages; i++)
                //{
                //    ColumnText.ShowTextAligned(pdfStamper1.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + pdfReader1.NumberOfPages), 568f, 15f, 0);
                //    ColumnText.ShowTextAligned(pdfStamper1.GetOverContent(i), Element.ALIGN_LEFT, new Phrase("Printed on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), pageTextFont), 2f, 15f, 0);
                //}
                //pdfStamper1.FormFlattening = false;
                //pdfStamper1.Dispose();
                //// close the pdf

                //pdfStamper1.Close();
                //pdfReader1.Dispose();
                //pdfReader1.Close();



                if (tabName == "PublishLifePlanVersion")
                {
                  //  dataTable = UploadPublishedPDFDocument(finaldoc, dataSetFillPDF.Tables[0], fillablePDFRequest);
                }
                else
                {
                    dataTable.Clear();
                    dataTable.Columns.Add("FileName");
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["FileName"] = newFile;
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

        private void fillCCOComprehensiveAssessmentSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableComprehensiveAssessment;
            try
            {
                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableComprehensiveAssessment = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableComprehensiveAssessment.Rows[0];

                    pdfFormFields.SetField("IndividualMiddleName", row["IndividualMiddleName"].ToString());
                    pdfFormFields.SetField("IndividualSuffix", row["IndividualSuffix"].ToString());
                    pdfFormFields.SetField("Nickname", row["Nickname"].ToString());
                    pdfFormFields.SetField("TABSId", row["TABSId"].ToString());
                    pdfFormFields.SetField("MedicaidId", row["MedicaidId"].ToString());
                    pdfFormFields.SetField("DateofBirth", row["DateofBirth"].ToString());
                    pdfFormFields.SetField("Gender", row["Gender"].ToString());
                    pdfFormFields.SetField("PreferredGender", row["PreferredGender"].ToString());
                    pdfFormFields.SetField("Race", row["Race"].ToString());
                    pdfFormFields.SetField("Ethnicity", row["Ethnicity"].ToString());
                    pdfFormFields.SetField("PhoneNumber", row["PhoneNumber"].ToString());
                    pdfFormFields.SetField("StreetAddress1", row["StreetAddress1"].ToString());
                    pdfFormFields.SetField("StreetAddress2", row["StreetAddress2"].ToString());
                    pdfFormFields.SetField("City", row["City"].ToString());
                    pdfFormFields.SetField("State", row["State"].ToString());
                    pdfFormFields.SetField("ZipCode", row["ZipCode"].ToString());
                    pdfFormFields.SetField("LivingSituation", row["LivingSituation"].ToString());
                    pdfFormFields.SetField("WillowbrookStatus", row["WillowbrookStatus"].ToString());
                    pdfFormFields.SetField("RepresentationStatus", row["RepresentationStatus"].ToString());
                    pdfFormFields.SetField("CABRepContact1", row["CABRepContact1"].ToString());
                    pdfFormFields.SetField("CABRepContact2", row["CABRepContact2"].ToString());
                    pdfFormFields.SetField("ExpectationsforCommunityInclusion", row["ExpectationsforCommunityInclusion"].ToString());
                    pdfFormFields.SetField("HospitalStaffingCoverage", row["HospitalStaffingCoverage"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillEligibiltyInformationSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableEligibiltyInformation;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableEligibiltyInformation = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableEligibiltyInformation.Rows[0];

                    pdfFormFields.SetField("MCOEnrollmentDate", row["MCOEnrollmentDate"].ToString());
                    pdfFormFields.SetField("MCOName", row["MCOName"].ToString());
                    pdfFormFields.SetField("OPWDDEligibility", row["OPWDDEligibility"].ToString());
                    pdfFormFields.SetField("ICFEligibilityDeterminationDate", row["ICFEligibilityDeterminationDate"].ToString());
                    pdfFormFields.SetField("MedicaidExpirationDate", row["MedicaidExpirationDate"].ToString());
                    pdfFormFields.SetField("HHConsentDate", row["HHConsentDate"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillCommunication_LanguageSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableCommunicationLanguage;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableCommunicationLanguage = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableCommunicationLanguage.Rows[0];

                  
                    pdfFormFields.SetField("MemExpressiveCommunicationSkill", row["MemExpressiveCommunicationSkill"].ToString());
                    pdfFormFields.SetField("MemReceptiveCommunicationSkill", row["MemReceptiveCommunicationSkill"].ToString());
                    pdfFormFields.SetField("MemPrimaryLanguage", row["MemPrimaryLanguage"].ToString());
                    pdfFormFields.SetField("MemPrimarySpokenLanguage", row["MemPrimarySpokenLanguage"].ToString());
                    pdfFormFields.SetField("MemPrimaryWrittenLanguage", row["MemPrimaryWrittenLanguage"].ToString());
                    pdfFormFields.SetField("MemAbleToReadPrimaryLanguage", row["MemAbleToReadPrimaryLanguage"].ToString());
                    pdfFormFields.SetField("MemMultiLingual", row["MemMultiLingual"].ToString());
                    pdfFormFields.SetField("MemMultiLingualLanguages", row["MemMultiLingualLanguages"].ToString());
                    pdfFormFields.SetField("Interpreter", row["Interpreter"].ToString());
                    pdfFormFields.SetField("Translator", row["Translator"].ToString());
                    pdfFormFields.SetField("NotApplicable", row["NotApplicable"].ToString());
                    pdfFormFields.SetField("MemWantToImproveCommunicate", row["MemWantToImproveCommunicate"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillMemberProviderSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableMemberProvider;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableMemberProvider = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableMemberProvider.Rows[0];

                    pdfFormFields.SetField("PrimaryCarePhysician", row["PrimaryCarePhysician"].ToString());
                    pdfFormFields.SetField("Dentist", row["Dentist"].ToString());
                    pdfFormFields.SetField("Psychiatrist", row["Psychiatrist"].ToString());
                    pdfFormFields.SetField("Psychologist", row["Psychologist"].ToString());
                    pdfFormFields.SetField("EyeDoctor", row["EyeDoctor"].ToString());
                    pdfFormFields.SetField("Pharmacy", row["Pharmacy"].ToString());
                    pdfFormFields.SetField("Hospital", row["Hospital"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillCircleAndSupportSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableCircleAndSupport;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableCircleAndSupport = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableCircleAndSupport.Rows[0];

                    pdfFormFields.SetField("IndividualMiddleName", row["IndividualMiddleName"].ToString());
                    pdfFormFields.SetField("IndividualSuffix", row["IndividualSuffix"].ToString());
                    pdfFormFields.SetField("Nickname", row["Nickname"].ToString());
                    pdfFormFields.SetField("TABSId", row["TABSId"].ToString());
                    pdfFormFields.SetField("MedicaidId", row["MedicaidId"].ToString());
                    pdfFormFields.SetField("DateofBirth", row["DateofBirth"].ToString());
                    pdfFormFields.SetField("Gender", row["Gender"].ToString());
                    pdfFormFields.SetField("PreferredGender", row["PreferredGender"].ToString());
                    pdfFormFields.SetField("Race", row["Race"].ToString());
                    pdfFormFields.SetField("Ethnicity", row["Ethnicity"].ToString());
                    pdfFormFields.SetField("PhoneNumber", row["PhoneNumber"].ToString());
                    pdfFormFields.SetField("StreetAddress1", row["StreetAddress1"].ToString());
                    pdfFormFields.SetField("StreetAddress2", row["StreetAddress2"].ToString());
                    pdfFormFields.SetField("City", row["City"].ToString());
                    pdfFormFields.SetField("State", row["State"].ToString());
                    pdfFormFields.SetField("ZipCode", row["ZipCode"].ToString());
                    pdfFormFields.SetField("LivingSituation", row["LivingSituation"].ToString());
                    pdfFormFields.SetField("WillowbrookStatus", row["WillowbrookStatus"].ToString());
                    pdfFormFields.SetField("RepresentationStatus", row["RepresentationStatus"].ToString());
                    pdfFormFields.SetField("CABRepContact1", row["CABRepContact1"].ToString());
                    pdfFormFields.SetField("CABRepContact2", row["CABRepContact2"].ToString());
                    pdfFormFields.SetField("ExpectationsforCommunityInclusion", row["ExpectationsforCommunityInclusion"].ToString());
                    pdfFormFields.SetField("HospitalStaffingCoverage", row["HospitalStaffingCoverage"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillGuardianshipAndAdvocacySection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableGuardianshipAndAdvocacy;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableGuardianshipAndAdvocacy = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableGuardianshipAndAdvocacy.Rows[0];

                    pdfFormFields.SetField("NoActiveGuardian", row["NoActiveGuardian"].ToString());
                    pdfFormFields.SetField("NotApplicableGuardian", row["NotApplicableGuardian"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillAdvancedDirectivesFuturePlanningSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableAdvancedDirectivesFuturePlanning;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableAdvancedDirectivesFuturePlanning = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableAdvancedDirectivesFuturePlanning.Rows[0];

                    pdfFormFields.SetField("MemHaveHealthCareProxy", row["MemHaveHealthCareProxy"].ToString());
                    pdfFormFields.SetField("HealthCareProxyName", row["HealthCareProxyName"].ToString());
                    pdfFormFields.SetField("MemLearnAdvancedHealthProxies", row["MemLearnAdvancedHealthProxies"].ToString());
                    pdfFormFields.SetField("MemSurrogateDesMakingCommittee", row["MemSurrogateDesMakingCommittee"].ToString());
                    pdfFormFields.SetField("MemUtiCommitAproveBehavioralSupportPlan", row["MemUtiCommitAproveBehavioralSupportPlan"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillIndependentLivingSkillsSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableIndependentLivingSkills;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableIndependentLivingSkills = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableIndependentLivingSkills.Rows[0];

                  
                    pdfFormFields.SetField("ExplainConsent", row["ExplainConsent"].ToString());
                    pdfFormFields.SetField("IndvCurrentLevelOfHousingStability", row["IndvCurrentLevelOfHousingStability"].ToString());
                    pdfFormFields.SetField("Pest", row["Pest"].ToString());
                    pdfFormFields.SetField("Mold", row["Mold"].ToString());
                    pdfFormFields.SetField("LeadPaint", row["LeadPaint"].ToString());
                    pdfFormFields.SetField("LackOfHeat", row["LackOfHeat"].ToString());
                    pdfFormFields.SetField("Oven", row["Oven"].ToString());
                    pdfFormFields.SetField("SmokeDetectorMissing", row["SmokeDetectorMissing"].ToString());
                    pdfFormFields.SetField("WaterLeakes", row["WaterLeakes"].ToString());
                    pdfFormFields.SetField("NoneOfTheAbove", row["NoneOfTheAbove"].ToString());
                    pdfFormFields.SetField("LevelOfPersonalHygiene", row["LevelOfPersonalHygiene"].ToString());
                    pdfFormFields.SetField("ExplainPersonalHygiene", row["ExplainPersonalHygiene"].ToString());
                    pdfFormFields.SetField("LevelOfSupportForToiletingNeed", row["LevelOfSupportForToiletingNeed"].ToString());
                    pdfFormFields.SetField("IndvExperConstipationDiarrheaVomiting", row["IndvExperConstipationDiarrheaVomiting"].ToString());
                    pdfFormFields.SetField("IndvExperForConstipationDiarrheaVomitingInLastMonths", row["IndvExperForConstipationDiarrheaVomitingInLastMonths"].ToString());
                    pdfFormFields.SetField("IndvBowelObstructionReqHospitalization", row["IndvBowelObstructionReqHospitalization"].ToString());
                    pdfFormFields.SetField("SupportForConstipationConcern", row["SupportForConstipationConcern"].ToString());
                    pdfFormFields.SetField("ExplainSupportResultConstipationNeed", row["ExplainSupportResultConstipationNeed"].ToString());
                    pdfFormFields.SetField("LevelOfSuppHandFaceWash", row["LevelOfSuppHandFaceWash"].ToString());
                    pdfFormFields.SetField("ChooseAnsSupportForDentalOralCare", row["ChooseAnsSupportForDentalOralCare"].ToString());
                    pdfFormFields.SetField("ExplainSupportResultDentalOralCare", row["ExplainSupportResultDentalOralCare"].ToString());
                    pdfFormFields.SetField("LevelOfSuppTrimNails", row["LevelOfSuppTrimNails"].ToString());
                    pdfFormFields.SetField("LevelOfSuppSneezeCough", row["LevelOfSuppSneezeCough"].ToString());
                    pdfFormFields.SetField("LevelOfSuppPPEMask", row["LevelOfSuppPPEMask"].ToString());
                    pdfFormFields.SetField("LevelOfSuppMoveSafely", row["LevelOfSuppMoveSafely"].ToString());
                    pdfFormFields.SetField("SuppForRiskToFall", row["SuppForRiskToFall"].ToString());
                    pdfFormFields.SetField("ExplainSuppForRiskToFall", row["ExplainSuppForRiskToFall"].ToString());
                    pdfFormFields.SetField("FallenInLastThreeMonths", row["FallenInLastThreeMonths"].ToString());
                    pdfFormFields.SetField("HowManyTimeMemberFallenInPast", row["HowManyTimeMemberFallenInPast"].ToString());
                    pdfFormFields.SetField("ConcernForIndividualVision", row["ConcernForIndividualVision"].ToString());
                    pdfFormFields.SetField("ExpConcernForIndividualVision", row["ExpConcernForIndividualVision"].ToString());
                    pdfFormFields.SetField("ConcernForIndividualHearing", row["ConcernForIndividualHearing"].ToString());
                    pdfFormFields.SetField("ExpConcernForIndividualHearing", row["ExpConcernForIndividualHearing"].ToString());
                    pdfFormFields.SetField("NoConcernForSkinIntegrity", row["NoConcernForSkinIntegrity"].ToString());
                    pdfFormFields.SetField("ReqPositioningSchedule", row["ReqPositioningSchedule"].ToString());
                    pdfFormFields.SetField("ReqDailySkinInspection", row["ReqDailySkinInspection"].ToString());
                    pdfFormFields.SetField("ReqAdaptiveEquipment", row["ReqAdaptiveEquipment"].ToString());
                    pdfFormFields.SetField("ReqSkinBarrierCream", row["ReqSkinBarrierCream"].ToString());
                    pdfFormFields.SetField("ProvideEducationWhereAppropriate", row["ProvideEducationWhereAppropriate"].ToString());
                    pdfFormFields.SetField("ExplainSupportSkinIntegrity", row["ExplainSupportSkinIntegrity"].ToString());
                    pdfFormFields.SetField("NoConcernForNutritionalNeed", row["NoConcernForNutritionalNeed"].ToString());
                    pdfFormFields.SetField("ReqConsistencyFood", row["ReqConsistencyFood"].ToString());
                    pdfFormFields.SetField("ReqConsistencyFluid", row["ReqConsistencyFluid"].ToString());
                    pdfFormFields.SetField("ReqReduceCalorieDiet", row["ReqReduceCalorieDiet"].ToString());
                    pdfFormFields.SetField("ReqHighCalorieDiet", row["ReqHighCalorieDiet"].ToString());
                    pdfFormFields.SetField("ReqFiberCalciumElementToDiet", row["ReqFiberCalciumElementToDiet"].ToString());
                    pdfFormFields.SetField("ReqSweetSaltFatElementRemove", row["ReqSweetSaltFatElementRemove"].ToString());
                    pdfFormFields.SetField("RestrictedFluid", row["RestrictedFluid"].ToString());
                    pdfFormFields.SetField("EnteralNutrition", row["EnteralNutrition"].ToString());
                    pdfFormFields.SetField("ReqDietarySupplement", row["ReqDietarySupplement"].ToString());
                    pdfFormFields.SetField("ReqAssitMealPreparation", row["ReqAssitMealPreparation"].ToString());
                    pdfFormFields.SetField("ReqEducation", row["ReqEducation"].ToString());
                    pdfFormFields.SetField("ReqAssitMealPlanning", row["ReqAssitMealPlanning"].ToString());
                    pdfFormFields.SetField("ReqSupervisionDuringMeal", row["ReqSupervisionDuringMeal"].ToString());
                    pdfFormFields.SetField("AdapEquDuringMeal", row["AdapEquDuringMeal"].ToString());
                    pdfFormFields.SetField("IndvMaintAdequateDiet", row["IndvMaintAdequateDiet"].ToString());
                    pdfFormFields.SetField("ExpSupptNutritionalCareNeed", row["ExpSupptNutritionalCareNeed"].ToString());
                    pdfFormFields.SetField("RiskForChoking", row["RiskForChoking"].ToString());
                    pdfFormFields.SetField("SupptOnChokingAspiration", row["SupptOnChokingAspiration"].ToString());
                    pdfFormFields.SetField("ExpSupptResultChokingAspirationNeed", row["ExpSupptResultChokingAspirationNeed"].ToString());
                    pdfFormFields.SetField("SwallowingEvaluationNeed", row["SwallowingEvaluationNeed"].ToString());
                    pdfFormFields.SetField("SupptThatIndvNeedOnAcidReflux", row["SupptThatIndvNeedOnAcidReflux"].ToString());
                    pdfFormFields.SetField("ExpSupptResultAcidRefluxNeed", row["ExpSupptResultAcidRefluxNeed"].ToString());
                    pdfFormFields.SetField("SupptNeedForMealPreparation", row["SupptNeedForMealPreparation"].ToString());
                    pdfFormFields.SetField("SupptNeedForMealPlanning", row["SupptNeedForMealPlanning"].ToString());
                    pdfFormFields.SetField("IndvWorriedAboutFoodInPast", row["IndvWorriedAboutFoodInPast"].ToString());
                    pdfFormFields.SetField("IndvRanOutOfFoodInPast", row["IndvRanOutOfFoodInPast"].ToString());
                    pdfFormFields.SetField("IndvElecGasOilWaterThreatedInPast", row["IndvElecGasOilWaterThreatedInPast"].ToString());
                    pdfFormFields.SetField("LevelOfSupptForCleaning", row["LevelOfSupptForCleaning"].ToString());
                    pdfFormFields.SetField("MoneyManagementNeedOfMember", row["MoneyManagementNeedOfMember"].ToString());
                    pdfFormFields.SetField("ExpAssistanceForBudgeting", row["ExpAssistanceForBudgeting"].ToString());
                    pdfFormFields.SetField("MemLearnToManageOwnMoney", row["MemLearnToManageOwnMoney"].ToString());
                    pdfFormFields.SetField("MedicationPrescribedByProvider", row["MedicationPrescribedByProvider"].ToString());
                    pdfFormFields.SetField("IndvAbilityAdministerMedication", row["IndvAbilityAdministerMedication"].ToString());
                    pdfFormFields.SetField("IndvNeedReminderForMedication", row["IndvNeedReminderForMedication"].ToString());
                    pdfFormFields.SetField("MedicationReminderMethod", row["MedicationReminderMethod"].ToString());
                    pdfFormFields.SetField("TakingMedicationAsPrescribed", row["TakingMedicationAsPrescribed"].ToString());
                    pdfFormFields.SetField("IndvRefuseForMedication", row["IndvRefuseForMedication"].ToString());
                    pdfFormFields.SetField("ExpIndvRefuseForMedication", row["ExpIndvRefuseForMedication"].ToString());
                    pdfFormFields.SetField("ExpSupptMedicationAdministration", row["ExpSupptMedicationAdministration"].ToString());
                    pdfFormFields.SetField("IndvAbleToAccessOwnPhone", row["IndvAbleToAccessOwnPhone"].ToString());
                    pdfFormFields.SetField("IndvAbleToCallEmergency", row["IndvAbleToCallEmergency"].ToString());
                    pdfFormFields.SetField("IndvAbleToAccessInternet", row["IndvAbleToAccessInternet"].ToString());
                    pdfFormFields.SetField("IndvCallApplicableContactInPhone", row["IndvCallApplicableContactInPhone"].ToString());
                    pdfFormFields.SetField("IndvNeedTransportation", row["IndvNeedTransportation"].ToString());
                    pdfFormFields.SetField("ExpTransportationNeed", row["ExpTransportationNeed"].ToString());
                    pdfFormFields.SetField("IndvLackedForTransportationInPastMonths", row["IndvLackedForTransportationInPastMonths"].ToString());
                    pdfFormFields.SetField("IndvLearnToDrive", row["IndvLearnToDrive"].ToString());
                    pdfFormFields.SetField("IndvWantVehicleOwnership", row["IndvWantVehicleOwnership"].ToString());
                    pdfFormFields.SetField("IndvIndependentUsingTransportation", row["IndvIndependentUsingTransportation"].ToString());
                    pdfFormFields.SetField("ConcernsWithBehavior", row["ConcernsWithBehavior"].ToString());
                    pdfFormFields.SetField("HowIndvMentalHealth", row["HowIndvMentalHealth"].ToString());
                    pdfFormFields.SetField("IndvCommunicateHealthConcern", row["IndvCommunicateHealthConcern"].ToString());
                    pdfFormFields.SetField("ExpIndvAbilityCommHealthConcern", row["ExpIndvAbilityCommHealthConcern"].ToString());
                    pdfFormFields.SetField("IndvAttendAllHealthService", row["IndvAttendAllHealthService"].ToString());
                    pdfFormFields.SetField("ExpSupptIndvAttendAllHealthService", row["ExpSupptIndvAttendAllHealthService"].ToString());
                    pdfFormFields.SetField("IndvSuppToHelpADLS", row["IndvSuppToHelpADLS"].ToString());
                    pdfFormFields.SetField("IndvDifficultyRememberingThings", row["IndvDifficultyRememberingThings"].ToString());
                    pdfFormFields.SetField("FollowTwoStepInstruction", row["FollowTwoStepInstruction"].ToString());
                    pdfFormFields.SetField("SpeakInFullSentence", row["SpeakInFullSentence"].ToString());
                    pdfFormFields.SetField("PretendPlay", row["PretendPlay"].ToString());
                    pdfFormFields.SetField("ImitateOther", row["ImitateOther"].ToString());
                    pdfFormFields.SetField("DrawCircle", row["DrawCircle"].ToString());
                    pdfFormFields.SetField("RunWithoutFalling", row["RunWithoutFalling"].ToString());
                    pdfFormFields.SetField("UpDownStepOneFootPerStep", row["UpDownStepOneFootPerStep"].ToString());
                    pdfFormFields.SetField("IndvRecvPreschoolService", row["IndvRecvPreschoolService"].ToString());
                    pdfFormFields.SetField("IndvHaveFireSafetyNeed", row["IndvHaveFireSafetyNeed"].ToString());
                    pdfFormFields.SetField("ExpIndvFireSafetyConcern", row["ExpIndvFireSafetyConcern"].ToString());
                    pdfFormFields.SetField("IndvHaveInfoAboutFireStartStoppedEtc", row["IndvHaveInfoAboutFireStartStoppedEtc"].ToString());
                    pdfFormFields.SetField("IndvEvacuateDuringFire", row["IndvEvacuateDuringFire"].ToString());
                    pdfFormFields.SetField("ExpAbilityToMaintainSafetyInEmergency", row["ExpAbilityToMaintainSafetyInEmergency"].ToString());
                    pdfFormFields.SetField("IsBackupPlanWhenNoHCBSProvider", row["IsBackupPlanWhenNoHCBSProvider"].ToString());
                    pdfFormFields.SetField("SupervisionNeedOfTheMember", row["SupervisionNeedOfTheMember"].ToString());
                    pdfFormFields.SetField("ExpSupervisionNeed", row["ExpSupervisionNeed"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillSocialServiceNeedsSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableSocialServiceNeeds;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableSocialServiceNeeds = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableSocialServiceNeeds.Rows[0];

                    pdfFormFields.SetField("IndvRepresentativePay", row["IndvRepresentativePay"].ToString());
                    pdfFormFields.SetField("TypeOfRepresentativePay", row["TypeOfRepresentativePay"].ToString());
                    pdfFormFields.SetField("SocialSecurity", row["SocialSecurity"].ToString());
                    pdfFormFields.SetField("SSI", row["SSI"].ToString());
                    pdfFormFields.SetField("SSDI", row["SSDI"].ToString());
                    pdfFormFields.SetField("DisabledAdultChild", row["DisabledAdultChild"].ToString());
                    pdfFormFields.SetField("OtherFinancialResource", row["OtherFinancialResource"].ToString());
                    pdfFormFields.SetField("IndvPrivateInsuranceProvider", row["IndvPrivateInsuranceProvider"].ToString());
                    pdfFormFields.SetField("IndvInsurerName", row["IndvInsurerName"].ToString());
                    pdfFormFields.SetField("PrivateInsurerId", row["PrivateInsurerId"].ToString());
                    pdfFormFields.SetField("HUDVoucher", row["HUDVoucher"].ToString());
                    pdfFormFields.SetField("ISSHousingSubsidy", row["ISSHousingSubsidy"].ToString());
                    pdfFormFields.SetField("OtherHousingAssistance", row["OtherHousingAssistance"].ToString());
                    pdfFormFields.SetField("MemInvolCriminalJusticeSystem", row["MemInvolCriminalJusticeSystem"].ToString());
                    pdfFormFields.SetField("ExpInvolCriminalJusticeSystem", row["ExpInvolCriminalJusticeSystem"].ToString());
                    pdfFormFields.SetField("MemCurrOnProbation", row["MemCurrOnProbation"].ToString());
                    pdfFormFields.SetField("ProbationContact", row["ProbationContact"].ToString());
                    pdfFormFields.SetField("MemNeedLegalAid", row["MemNeedLegalAid"].ToString());
                    pdfFormFields.SetField("CrimJustSystemImpactHousing", row["CrimJustSystemImpactHousing"].ToString());
                    pdfFormFields.SetField("ExpCrimJustSystemImpactHousing", row["ExpCrimJustSystemImpactHousing"].ToString());
                    pdfFormFields.SetField("CrimJustSystemImpactEmployment", row["CrimJustSystemImpactEmployment"].ToString());
                    pdfFormFields.SetField("ExpCrimJustSystemImpactEmployment", row["ExpCrimJustSystemImpactEmployment"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());

                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillMedicalHealthSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableMedicalHealth;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableMedicalHealth = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableMedicalHealth.Rows[0];

                    
                    pdfFormFields.SetField("SeizureDisorder", row["SeizureDisorder"].ToString());
                    pdfFormFields.SetField("CerebralPalsy", row["CerebralPalsy"].ToString());
                    pdfFormFields.SetField("Spasticity", row["Spasticity"].ToString());
                    pdfFormFields.SetField("Asthma", row["Asthma"].ToString());
                    pdfFormFields.SetField("GERD", row["GERD"].ToString());
                    pdfFormFields.SetField("PICA", row["PICA"].ToString());
                    pdfFormFields.SetField("FailureToThrive", row["FailureToThrive"].ToString());
                    pdfFormFields.SetField("Arthritis", row["Arthritis"].ToString());
                    pdfFormFields.SetField("TypeOneDiabetes", row["TypeOneDiabetes"].ToString());
                    pdfFormFields.SetField("TypeTwoDiabetes", row["TypeTwoDiabetes"].ToString());
                    pdfFormFields.SetField("PreDiabetic", row["PreDiabetic"].ToString());
                    pdfFormFields.SetField("HIV", row["HIV"].ToString());
                    pdfFormFields.SetField("RecurrentUrinaryTractInfection", row["RecurrentUrinaryTractInfection"].ToString());
                    pdfFormFields.SetField("HeartDisease", row["HeartDisease"].ToString());
                    pdfFormFields.SetField("ChronicLungDisease", row["ChronicLungDisease"].ToString());
                    pdfFormFields.SetField("Stroke", row["Stroke"].ToString());
                    pdfFormFields.SetField("ChronicKidneyDisease", row["ChronicKidneyDisease"].ToString());
                    pdfFormFields.SetField("AlzheimerDisease", row["AlzheimerDisease"].ToString());
                    pdfFormFields.SetField("Cancer", row["Cancer"].ToString());
                    pdfFormFields.SetField("AllMemAllergies", row["AllMemAllergies"].ToString());
                    pdfFormFields.SetField("ExpAllMemAllergies", row["ExpAllMemAllergies"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void fillHealthPromotionSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableHealthPromotion;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableHealthPromotion = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableHealthPromotion.Rows[0];


                    pdfFormFields.SetField("MemHospitalizedInLastMonths", row["MemHospitalizedInLastMonths"].ToString());
                    pdfFormFields.SetField("MemRecentHospitalized", row["MemRecentHospitalized"].ToString());
                    pdfFormFields.SetField("MemLastAnnualPhysicalExam", row["MemLastAnnualPhysicalExam"].ToString());
                    pdfFormFields.SetField("MemLastDentalExam", row["MemLastDentalExam"].ToString());
                    pdfFormFields.SetField("NoConcernsAtThisTime", row["NoConcernsAtThisTime"].ToString());
                    pdfFormFields.SetField("DentalHygieneSupport", row["DentalHygieneSupport"].ToString());
                    pdfFormFields.SetField("Presedation", row["Presedation"].ToString());
                    pdfFormFields.SetField("Dentures", row["Dentures"].ToString());
                    pdfFormFields.SetField("MIPS", row["MIPS"].ToString());
                    pdfFormFields.SetField("Other", row["Other"].ToString());
                    pdfFormFields.SetField("Orthodondist", row["Orthodondist"].ToString());
                    pdfFormFields.SetField("ExpSupptResultDentalOralCare", row["ExpSupptResultDentalOralCare"].ToString());
                    pdfFormFields.SetField("MemLastEyeExam", row["MemLastEyeExam"].ToString());
                    pdfFormFields.SetField("MemHadColonoscopy", row["MemHadColonoscopy"].ToString());
                    pdfFormFields.SetField("MemRecentColonoscopy", row["MemRecentColonoscopy"].ToString());
                    pdfFormFields.SetField("MemHadMammogram", row["MemHadMammogram"].ToString());
                    pdfFormFields.SetField("MemRecentMammogram", row["MemRecentMammogram"].ToString());
                    pdfFormFields.SetField("MemHadCervicalCancerExam", row["MemHadCervicalCancerExam"].ToString());
                    pdfFormFields.SetField("MemRecentCervicalCancerExam", row["MemRecentCervicalCancerExam"].ToString());
                    pdfFormFields.SetField("MemHadProstateExam", row["MemHadProstateExam"].ToString());
                    pdfFormFields.SetField("MemRecentProstateExam", row["MemRecentProstateExam"].ToString());
                    pdfFormFields.SetField("MemDementiaInPastMonths", row["MemDementiaInPastMonths"].ToString());
                    pdfFormFields.SetField("MemRecentDementia", row["MemRecentDementia"].ToString());
                    pdfFormFields.SetField("MemHeight", row["MemHeight"].ToString());
                    pdfFormFields.SetField("MemWeight", row["MemWeight"].ToString());
                    pdfFormFields.SetField("BMI", row["BMI"].ToString());
                    pdfFormFields.SetField("MemConcernAboutSleep", row["MemConcernAboutSleep"].ToString());
                    pdfFormFields.SetField("MemAwakeDuringNight", row["MemAwakeDuringNight"].ToString());
                    pdfFormFields.SetField("MemHadDiabeticScreening", row["MemHadDiabeticScreening"].ToString());
                    pdfFormFields.SetField("MemRecentDiabeticScreening", row["MemRecentDiabeticScreening"].ToString());
                    pdfFormFields.SetField("NoConcernForDiabetes", row["NoConcernForDiabetes"].ToString());
                    pdfFormFields.SetField("RequiredMedicationForDiabetes", row["RequiredMedicationForDiabetes"].ToString());
                    pdfFormFields.SetField("AssistanceWithDiabetesMonitoring", row["AssistanceWithDiabetesMonitoring"].ToString());
                    pdfFormFields.SetField("MedicationAdministration", row["MedicationAdministration"].ToString());
                    pdfFormFields.SetField("DietaryModification", row["DietaryModification"].ToString());
                    pdfFormFields.SetField("EducationTraining", row["EducationTraining"].ToString());
                    pdfFormFields.SetField("ExpSupptResultForMemDiabetes", row["ExpSupptResultForMemDiabetes"].ToString());
                    pdfFormFields.SetField("NoRespiratoryConcern", row["NoRespiratoryConcern"].ToString());
                    pdfFormFields.SetField("RequiresMedicationForRespConcren", row["RequiresMedicationForRespConcren"].ToString());
                    pdfFormFields.SetField("UseCPAPMachine", row["UseCPAPMachine"].ToString());
                    pdfFormFields.SetField("UseNebulizer", row["UseNebulizer"].ToString());
                    pdfFormFields.SetField("UseOxygen", row["UseOxygen"].ToString());
                    pdfFormFields.SetField("ExerciseRestrictions", row["ExerciseRestrictions"].ToString());
                    pdfFormFields.SetField("OtherTherapies", row["OtherTherapies"].ToString());
                    pdfFormFields.SetField("ExpServicesRespiratoryNeed", row["ExpServicesRespiratoryNeed"].ToString());
                    pdfFormFields.SetField("NoConcernsForCholesterol", row["NoConcernsForCholesterol"].ToString());
                    pdfFormFields.SetField("ModifiedDiet", row["ModifiedDiet"].ToString());
                    pdfFormFields.SetField("CholesterolLoweringMedications", row["CholesterolLoweringMedications"].ToString());
                    pdfFormFields.SetField("IncreaseExercise", row["IncreaseExercise"].ToString());
                    pdfFormFields.SetField("EncourageWeightLossForCholesterol", row["EncourageWeightLossForCholesterol"].ToString());
                    pdfFormFields.SetField("ProvideAssistanceWithMealPlanning", row["ProvideAssistanceWithMealPlanning"].ToString());
                    pdfFormFields.SetField("ProvideEducationToThePerson", row["ProvideEducationToThePerson"].ToString());
                    pdfFormFields.SetField("ExpSupptForHighCholesterol", row["ExpSupptForHighCholesterol"].ToString());
                    pdfFormFields.SetField("NoConcernForHighBloodPressure", row["NoConcernForHighBloodPressure"].ToString());
                    pdfFormFields.SetField("EncourageWeightLossForHighBloodPressure", row["EncourageWeightLossForHighBloodPressure"].ToString());
                    pdfFormFields.SetField("BloodPressureMonitoringPlan", row["BloodPressureMonitoringPlan"].ToString());
                    pdfFormFields.SetField("ReduceSaltIntake", row["ReduceSaltIntake"].ToString());
                    pdfFormFields.SetField("EncourageExercise", row["EncourageExercise"].ToString());
                    pdfFormFields.SetField("MedicationRequired", row["MedicationRequired"].ToString());
                    pdfFormFields.SetField("ExpSupptForHighBloodPressure", row["ExpSupptForHighBloodPressure"].ToString());
                    pdfFormFields.SetField("MemBloodTestForLeadPoisoning", row["MemBloodTestForLeadPoisoning"].ToString());
                    pdfFormFields.SetField("MemRecentBloodTestForLeadPoisoning", row["MemRecentBloodTestForLeadPoisoning"].ToString());
                    pdfFormFields.SetField("MemSexuallyActive", row["MemSexuallyActive"].ToString());
                    pdfFormFields.SetField("BirthControlOral", row["BirthControlOral"].ToString());
                    pdfFormFields.SetField("BirthControlProphylactic", row["BirthControlProphylactic"].ToString());
                    pdfFormFields.SetField("NaturalFamilyPlanning", row["NaturalFamilyPlanning"].ToString());
                    pdfFormFields.SetField("NoBirthControl", row["NoBirthControl"].ToString());
                    pdfFormFields.SetField("Unknown", row["Unknown"].ToString());
                    pdfFormFields.SetField("STIInPastMonths", row["STIInPastMonths"].ToString());
                    pdfFormFields.SetField("MemHIVPositive", row["MemHIVPositive"].ToString());
                    pdfFormFields.SetField("MemLastHIVAppointment", row["MemLastHIVAppointment"].ToString());
                    pdfFormFields.SetField("MemExerciseInWeekForThirtyMintues", row["MemExerciseInWeekForThirtyMintues"].ToString());
                    pdfFormFields.SetField("MemInterestedIncPhysicalActivity", row["MemInterestedIncPhysicalActivity"].ToString());
                    pdfFormFields.SetField("MemHaveSeizureDisorder", row["MemHaveSeizureDisorder"].ToString());
                    pdfFormFields.SetField("MemNeedSupptOnSeizure", row["MemNeedSupptOnSeizure"].ToString());
                    pdfFormFields.SetField("ExpMemSupptExpectedForSeizureDisorder", row["ExpMemSupptExpectedForSeizureDisorder"].ToString());
                    pdfFormFields.SetField("HealthRelatedConcernsNotAddressed", row["HealthRelatedConcernsNotAddressed"].ToString());
                    pdfFormFields.SetField("ExpHealthConcerns", row["ExpHealthConcerns"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());



                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillBehavioralHealthSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableBehavioralHealth;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableBehavioralHealth = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableBehavioralHealth.Rows[0];

                    pdfFormFields.SetField("MemberBeenDiagnosed", row["MemberBeenDiagnosed"].ToString());
                    pdfFormFields.SetField("PrevAnxiety", row["PrevAnxiety"].ToString());
                    pdfFormFields.SetField("PrevDepression", row["PrevDepression"].ToString());
                    pdfFormFields.SetField("PrevADHD", row["PrevADHD"].ToString());
                    pdfFormFields.SetField("PrevPanicDisorder", row["PrevPanicDisorder"].ToString());
                    pdfFormFields.SetField("PrevPsychosis", row["PrevPsychosis"].ToString());
                    pdfFormFields.SetField("PrevSchizophrenia", row["PrevSchizophrenia"].ToString());
                    pdfFormFields.SetField("PrevBipolarDisorder", row["PrevBipolarDisorder"].ToString());
                    pdfFormFields.SetField("PrevPostTraumaticStressDisorder", row["PrevPostTraumaticStressDisorder"].ToString());
                    pdfFormFields.SetField("PrevObsessiveCompulsiveDisorder", row["PrevObsessiveCompulsiveDisorder"].ToString());
                    pdfFormFields.SetField("PrevEatingDisorder", row["PrevEatingDisorder"].ToString());
                    pdfFormFields.SetField("PrevImpulsiveControlDisorder", row["PrevImpulsiveControlDisorder"].ToString());
                    pdfFormFields.SetField("PrevPersonalityDisorder", row["PrevPersonalityDisorder"].ToString());
                    pdfFormFields.SetField("PrevBorderlinePersonalityDisorder", row["PrevBorderlinePersonalityDisorder"].ToString());
                    pdfFormFields.SetField("CurreAnxiety", row["CurreAnxiety"].ToString());
                    pdfFormFields.SetField("CurreDepression", row["CurreDepression"].ToString());
                    pdfFormFields.SetField("CurreADHD", row["CurreADHD"].ToString());
                    pdfFormFields.SetField("CurrePanicDisorder", row["CurrePanicDisorder"].ToString());
                    pdfFormFields.SetField("CurrePsychosis", row["CurrePsychosis"].ToString());
                    pdfFormFields.SetField("CurreSchizophrenia", row["CurreSchizophrenia"].ToString());
                    pdfFormFields.SetField("CurreBipolarDisorder", row["CurreBipolarDisorder"].ToString());
                    pdfFormFields.SetField("CurrePostTraumaticStressDisorder", row["CurrePostTraumaticStressDisorder"].ToString());
                    pdfFormFields.SetField("CurreObsessiveCompulsiveDisorder", row["CurreObsessiveCompulsiveDisorder"].ToString());

                    pdfFormFields.SetField("CurreEatingDisorder", row["CurreEatingDisorder"].ToString());
                    pdfFormFields.SetField("CurreImpulsiveControlDisorder", row["CurreImpulsiveControlDisorder"].ToString());
                    pdfFormFields.SetField("CurrePersonalityDisorder", row["CurrePersonalityDisorder"].ToString());
                    pdfFormFields.SetField("CurreBorderlinePersonalityDisorder", row["CurreBorderlinePersonalityDisorder"].ToString());
                    pdfFormFields.SetField("DiagnosMemberAcuteChronicHealthCondition", row["DiagnosMemberAcuteChronicHealthCondition"].ToString());
                    pdfFormFields.SetField("PsychiatricConditionInterfereWithMem", row["PsychiatricConditionInterfereWithMem"].ToString());
                    pdfFormFields.SetField("SourceOfMentalHealthDiagnos", row["SourceOfMentalHealthDiagnos"].ToString());
                    pdfFormFields.SetField("OutpatientOneToOneTherapy", row["OutpatientOneToOneTherapy"].ToString());
                    pdfFormFields.SetField("OutpatientGroupTherapy", row["OutpatientGroupTherapy"].ToString());
                    pdfFormFields.SetField("PsychiatricMedication", row["PsychiatricMedication"].ToString());
                    pdfFormFields.SetField("FamilyTherapy", row["FamilyTherapy"].ToString());
                    pdfFormFields.SetField("ParentSupportAndTraining", row["ParentSupportAndTraining"].ToString());
                    pdfFormFields.SetField("PeerMentor", row["PeerMentor"].ToString());
                    pdfFormFields.SetField("PROSClinic", row["PROSClinic"].ToString());
                    pdfFormFields.SetField("AcuteInpatientTreatment", row["AcuteInpatientTreatment"].ToString());
                    pdfFormFields.SetField("LongTermInpatientTreatment", row["LongTermInpatientTreatment"].ToString());
                    pdfFormFields.SetField("Other", row["Other"].ToString());
                    pdfFormFields.SetField("MemRecentPsychiatricHospitalization", row["MemRecentPsychiatricHospitalization"].ToString());
                    pdfFormFields.SetField("MemRecvSuicidalThoughtsInPast", row["MemRecvSuicidalThoughtsInPast"].ToString());
                    pdfFormFields.SetField("MemMonitoredForSuicidalRisk", row["MemMonitoredForSuicidalRisk"].ToString());
                    pdfFormFields.SetField("NatureOfSelfHarmBehavior", row["NatureOfSelfHarmBehavior"].ToString());
                    pdfFormFields.SetField("MemMonitoredForSelfHarmRisk", row["MemMonitoredForSelfHarmRisk"].ToString());
                    pdfFormFields.SetField("MemMedicationMonitoringPlan", row["MemMedicationMonitoringPlan"].ToString());
                    pdfFormFields.SetField("MedicationMonitoredByPsychiatrist", row["MedicationMonitoredByPsychiatrist"].ToString());
                    pdfFormFields.SetField("PscyhiatricMonitoringFrequency", row["PscyhiatricMonitoringFrequency"].ToString());
                    pdfFormFields.SetField("MemHistoryOfTrauma", row["MemHistoryOfTrauma"].ToString());
                    pdfFormFields.SetField("MemPhysicallyHurtOthers", row["MemPhysicallyHurtOthers"].ToString());
                    pdfFormFields.SetField("MemInsultOthers", row["MemInsultOthers"].ToString());
                    pdfFormFields.SetField("MemThreatenOthers", row["MemThreatenOthers"].ToString());
                    pdfFormFields.SetField("MemScreamCurseOthers", row["MemScreamCurseOthers"].ToString());
                    pdfFormFields.SetField("MemSmoke", row["MemSmoke"].ToString());
                    pdfFormFields.SetField("IndvDrinkAlcohol", row["IndvDrinkAlcohol"].ToString());
                    pdfFormFields.SetField("IndvUseRecreationalDrugs", row["IndvUseRecreationalDrugs"].ToString());
                    pdfFormFields.SetField("PersonBeenPrescribedPRN", row["PersonBeenPrescribedPRN"].ToString());
                    pdfFormFields.SetField("ReasonForPRNMedication", row["ReasonForPRNMedication"].ToString());
                    pdfFormFields.SetField("FrequencyForPRNGiven", row["FrequencyForPRNGiven"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillChallengingBehaviorsSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableChallengingBehaviors;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableChallengingBehaviors = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableChallengingBehaviors.Rows[0];

                    pdfFormFields.SetField("MemHasChallengingBehavior", row["MemHasChallengingBehavior"].ToString());
                    pdfFormFields.SetField("SelfHarmfulBehavior", row["SelfHarmfulBehavior"].ToString());
                    pdfFormFields.SetField("PhysicallyHurtOther", row["PhysicallyHurtOther"].ToString());
                    pdfFormFields.SetField("HarmOther", row["HarmOther"].ToString());
                    pdfFormFields.SetField("DestructionOfProperty", row["DestructionOfProperty"].ToString());
                    pdfFormFields.SetField("DisruptiveBehavior", row["DisruptiveBehavior"].ToString());
                    pdfFormFields.SetField("UnusualBehavior", row["UnusualBehavior"].ToString());
                    pdfFormFields.SetField("Withdrawal", row["Withdrawal"].ToString());
                    pdfFormFields.SetField("SociallyOffensiveBehavior", row["SociallyOffensiveBehavior"].ToString());
                    pdfFormFields.SetField("PersistentlyUncooperative", row["PersistentlyUncooperative"].ToString());
                    pdfFormFields.SetField("ProblemWithSelfcare", row["ProblemWithSelfcare"].ToString());
                    pdfFormFields.SetField("Pica", row["Pica"].ToString());
                    pdfFormFields.SetField("Elopement", row["Elopement"].ToString());
                    pdfFormFields.SetField("Other", row["Other"].ToString());
                    pdfFormFields.SetField("MemChallengingBehaviorManifests", row["MemChallengingBehaviorManifests"].ToString());
                    pdfFormFields.SetField("OutpatientOneToOneTherpay", row["OutpatientOneToOneTherpay"].ToString());
                    pdfFormFields.SetField("OutpatientGroupTherapy", row["OutpatientGroupTherapy"].ToString());
                    pdfFormFields.SetField("PsychiatricMedication", row["PsychiatricMedication"].ToString());
                    pdfFormFields.SetField("FamilyTherapy", row["FamilyTherapy"].ToString());
                    pdfFormFields.SetField("ParentSupportAndTraining", row["ParentSupportAndTraining"].ToString());
                    pdfFormFields.SetField("PeerMentor", row["PeerMentor"].ToString());
                    pdfFormFields.SetField("PROSClinic", row["PROSClinic"].ToString());
                    pdfFormFields.SetField("AcuteInpatientTreatment", row["AcuteInpatientTreatment"].ToString());
                    pdfFormFields.SetField("LongTermInpatientTreatment", row["LongTermInpatientTreatment"].ToString());
                    pdfFormFields.SetField("OtherChalleningBehaviorInPast", row["OtherChalleningBehaviorInPast"].ToString());
                    pdfFormFields.SetField("RestrictiveEater", row["RestrictiveEater"].ToString());
                    pdfFormFields.SetField("MemShowAggressiveOnMeals", row["MemShowAggressiveOnMeals"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillBehavioralSupportPlanSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableBehavioralSupportPlan;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableBehavioralSupportPlan = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableBehavioralSupportPlan.Rows[0];

                    pdfFormFields.SetField("MemHaveBehavioralSupportPlan", row["MemHaveBehavioralSupportPlan"].ToString());
                    pdfFormFields.SetField("MemHumanRightApproval", row["MemHumanRightApproval"].ToString());
                    pdfFormFields.SetField("MemReqPhyInterventionInPastForSafety", row["MemReqPhyInterventionInPastForSafety"].ToString());
                    pdfFormFields.SetField("PhyInterventionPartOfSupportPlan", row["PhyInterventionPartOfSupportPlan"].ToString());
                    pdfFormFields.SetField("MemSupptPlanContainRestrictiveIntervention", row["MemSupptPlanContainRestrictiveIntervention"].ToString());
                    pdfFormFields.SetField("SCIPR", row["SCIPR"].ToString());
                    pdfFormFields.SetField("Medication", row["Medication"].ToString());
                    pdfFormFields.SetField("RightLimitation", row["RightLimitation"].ToString());
                    pdfFormFields.SetField("TimeOut", row["TimeOut"].ToString());
                    pdfFormFields.SetField("MechanicalRestrainingDevice", row["MechanicalRestrainingDevice"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillMedicationsSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableMedications;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableMedications = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableMedications.Rows[0];

                    pdfFormFields.SetField("IndividualMiddleName", row["IndividualMiddleName"].ToString());
                    pdfFormFields.SetField("IndividualSuffix", row["IndividualSuffix"].ToString());
                    pdfFormFields.SetField("Nickname", row["Nickname"].ToString());
                    pdfFormFields.SetField("TABSId", row["TABSId"].ToString());
                    pdfFormFields.SetField("MedicaidId", row["MedicaidId"].ToString());
                    pdfFormFields.SetField("DateofBirth", row["DateofBirth"].ToString());
                    pdfFormFields.SetField("Gender", row["Gender"].ToString());
                    pdfFormFields.SetField("PreferredGender", row["PreferredGender"].ToString());
                    pdfFormFields.SetField("Race", row["Race"].ToString());
                    pdfFormFields.SetField("Ethnicity", row["Ethnicity"].ToString());
                    pdfFormFields.SetField("PhoneNumber", row["PhoneNumber"].ToString());
                    pdfFormFields.SetField("StreetAddress1", row["StreetAddress1"].ToString());
                    pdfFormFields.SetField("StreetAddress2", row["StreetAddress2"].ToString());
                    pdfFormFields.SetField("City", row["City"].ToString());
                    pdfFormFields.SetField("State", row["State"].ToString());
                    pdfFormFields.SetField("ZipCode", row["ZipCode"].ToString());
                    pdfFormFields.SetField("LivingSituation", row["LivingSituation"].ToString());
                    pdfFormFields.SetField("WillowbrookStatus", row["WillowbrookStatus"].ToString());
                    pdfFormFields.SetField("RepresentationStatus", row["RepresentationStatus"].ToString());
                    pdfFormFields.SetField("CABRepContact1", row["CABRepContact1"].ToString());
                    pdfFormFields.SetField("CABRepContact2", row["CABRepContact2"].ToString());
                    pdfFormFields.SetField("ExpectationsforCommunityInclusion", row["ExpectationsforCommunityInclusion"].ToString());
                    pdfFormFields.SetField("HospitalStaffingCoverage", row["HospitalStaffingCoverage"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillCommumunitySocialParticipationSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableCommumunitySocialParticipation;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableCommumunitySocialParticipation = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableCommumunitySocialParticipation.Rows[0];

                    pdfFormFields.SetField("MemCurreSelfDirectSupportService", row["MemCurreSelfDirectSupportService"].ToString());
                    pdfFormFields.SetField("MemWishSelfDirectSupportService", row["MemWishSelfDirectSupportService"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillEducationSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableEducation;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableEducation = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableEducation.Rows[0];

                    pdfFormFields.SetField("MemCompletedEducationLevel", row["MemCompletedEducationLevel"].ToString());
                    pdfFormFields.SetField("MemCurreSchoolEducation", row["MemCurreSchoolEducation"].ToString());
                    pdfFormFields.SetField("CurreEducationMeetNeed", row["CurreEducationMeetNeed"].ToString());
                    pdfFormFields.SetField("MemPursuingAdditionalEducation", row["MemPursuingAdditionalEducation"].ToString());
                    pdfFormFields.SetField("ChooseCurrentEducation", row["ChooseCurrentEducation"].ToString());
                    pdfFormFields.SetField("DescribeSupptResultInEducationSetting", row["DescribeSupptResultInEducationSetting"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillTransitionPlanningSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableTransitionPlanning;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableTransitionPlanning = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableTransitionPlanning.Rows[0];

                    pdfFormFields.SetField("DescribePrevocationalSkill", row["DescribePrevocationalSkill"].ToString());
                    pdfFormFields.SetField("MemCompetitivelyEmployed", row["MemCompetitivelyEmployed"].ToString());
                    pdfFormFields.SetField("MemCurreReceivingServices", row["MemCurreReceivingServices"].ToString());
                    pdfFormFields.SetField("StartDateACCESVRService", row["StartDateACCESVRService"].ToString());
                    pdfFormFields.SetField("VocationalCounseling", row["VocationalCounseling"].ToString());
                    pdfFormFields.SetField("AssessmentsAndEvaluations", row["AssessmentsAndEvaluations"].ToString());
                    pdfFormFields.SetField("RehabilitationTechnology", row["RehabilitationTechnology"].ToString());
                    pdfFormFields.SetField("SpecialTransportation", row["SpecialTransportation"].ToString());
                    pdfFormFields.SetField("AdaptiveDriverTraining", row["AdaptiveDriverTraining"].ToString());
                    pdfFormFields.SetField("WorkReadiness", row["WorkReadiness"].ToString());
                    pdfFormFields.SetField("TuitionFeesTextbooks", row["TuitionFeesTextbooks"].ToString());
                    pdfFormFields.SetField("NoteTaker", row["NoteTaker"].ToString());
                    pdfFormFields.SetField("YouthService", row["YouthService"].ToString());
                    pdfFormFields.SetField("PhysicalMentalRestoration", row["PhysicalMentalRestoration"].ToString());
                    pdfFormFields.SetField("HomeVehicleWorksite", row["HomeVehicleWorksite"].ToString());
                    pdfFormFields.SetField("JobDevelopmentPlacement", row["JobDevelopmentPlacement"].ToString());
                    pdfFormFields.SetField("WorkTryOut", row["WorkTryOut"].ToString());
                    pdfFormFields.SetField("JobCoaching", row["JobCoaching"].ToString());
                    pdfFormFields.SetField("OccupationalToolEquipment", row["OccupationalToolEquipment"].ToString());
                    pdfFormFields.SetField("GoodsInventoryEquipment", row["GoodsInventoryEquipment"].ToString());
                    pdfFormFields.SetField("OccupationalBusinessLicense", row["OccupationalBusinessLicense"].ToString());
                    pdfFormFields.SetField("TicketToWork", row["TicketToWork"].ToString());
                    pdfFormFields.SetField("PASS", row["PASS"].ToString());
                    pdfFormFields.SetField("WelfareToWork", row["WelfareToWork"].ToString());
                    pdfFormFields.SetField("Other", row["Other"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void fillEmploymentSection(AcroFields pdfFormFields, DataSet dataSetFillPDF)
        {
            DataTable dataTableEmployment;
            try
            {

                if (dataSetFillPDF.Tables.Count > 0 && dataSetFillPDF.Tables[10].Rows.Count > 0)
                {
                    dataTableEmployment = dataSetFillPDF.Tables[10];
                    DataRow row = dataTableEmployment.Rows[0];

                    pdfFormFields.SetField("MemCurrentlyHCBSSupptService", row["MemCurrentlyHCBSSupptService"].ToString());
                    pdfFormFields.SetField("MemCurreEmploymentStatus", row["MemCurreEmploymentStatus"].ToString());
                    pdfFormFields.SetField("MemWishIncCurreLevelOfEmployment", row["MemWishIncCurreLevelOfEmployment"].ToString());
                    pdfFormFields.SetField("MemSatisfiedWithCurrentEmployer", row["MemSatisfiedWithCurrentEmployer"].ToString());
                    pdfFormFields.SetField("EmployerName", row["EmployerName"].ToString());
                    pdfFormFields.SetField("EmployerLocation", row["EmployerLocation"].ToString());
                    pdfFormFields.SetField("StartDateOfCurrentJob", row["StartDateOfCurrentJob"].ToString());
                    pdfFormFields.SetField("TerminationDateOfRecentJob", row["TerminationDateOfRecentJob"].ToString());
                    pdfFormFields.SetField("ReasonToChangeEmploymentStatus", row["ReasonToChangeEmploymentStatus"].ToString());
                    pdfFormFields.SetField("MemHoursWorkInWeek", row["MemHoursWorkInWeek"].ToString());
                    pdfFormFields.SetField("MemEarnInWeek", row["MemEarnInWeek"].ToString());
                    pdfFormFields.SetField("MemPaycheck", row["MemPaycheck"].ToString());
                    pdfFormFields.SetField("DescMemEmploymentSetting", row["DescMemEmploymentSetting"].ToString());
                    pdfFormFields.SetField("SatisfiedCurrentEmploymentSetting", row["SatisfiedCurrentEmploymentSetting"].ToString());
                    pdfFormFields.SetField("MemWorkInIntegratedSetting", row["MemWorkInIntegratedSetting"].ToString());
                    pdfFormFields.SetField("Status", row["Status"].ToString());


                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

    }
}
