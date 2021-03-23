using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.CCOComprehensiveAssessmentResponse;
using static IncidentManagement.Entities.Response.ComprehensiveAssessmentResponse;

namespace IncidentManagement.Repository.IRepository
{
  public  interface IComprehensiveAssessmentRepository
    {
        #region interface methods
        Task<ComprehensiveAssessmentDetailResponse> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest,string companyId);

        Task<ComprehensiveAssessmentDetailResponse> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId);
        Task<ComprehensiveAssessmentDetailResponse> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId);
        Task<ComprehensiveAssessmentPDFResponse> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId);
        Task<ComprehensiveAssessmentDetailResponse> UploadOfflinePDF(string json, string companyId);
        Task<CCOComprehensiveAssessmentDetailResponse> GetCCOComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId);

        Task<CCOComprehensiveAssessmentDetailResponse> HandleCCOComprehensiveAssessmentVersioning(CCOComprehensiveAssessmentRequest cCOComprehensiveAssessmentRequest);
        Task<CCOComprehensivePDFResponse> FillableCCOComprehensiveAssessmentPDF(FillableCCOComprehensiveAssessmentPDFRequest fillableCCOComprehensiveAssessmentPDFRequest);
     
        #endregion
    }
}
