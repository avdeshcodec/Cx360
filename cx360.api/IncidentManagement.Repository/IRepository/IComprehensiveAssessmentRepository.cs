using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.ComprehensiveAssessmentResponse;

namespace IncidentManagement.Repository.IRepository
{
  public  interface IComprehensiveAssessmentRepository
    {
        #region interface methods
        Task<ComprehensiveAssessmentDetailResponse> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest);

        Task<ComprehensiveAssessmentDetailResponse> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest);
        Task<ComprehensiveAssessmentDetailResponse> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest);
        Task<ComprehensiveAssessmentPDFResponse> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest);
        Task<ComprehensiveAssessmentDetailResponse> UploadOfflinePDF(string json );


        #endregion
    }
}
