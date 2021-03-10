using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.IRepository;
using IncidentManagement.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.ComprehensiveAssessmentResponse;

namespace IncidentManagement.Service.Service
{
   public class ComprehensiveAssessmentService:IComprehensiveAssessmentService
    {
        #region Private
        private IComprehensiveAssessmentRepository _ComprehensiveAssessmentRepository;
        #endregion

        public ComprehensiveAssessmentService(IComprehensiveAssessmentRepository IComprehensiveAssessmentRepository)
        {
            _ComprehensiveAssessmentRepository = IComprehensiveAssessmentRepository;
        }

        public async Task<ComprehensiveAssessmentDetailResponse> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            return await _ComprehensiveAssessmentRepository.InsertModifyComprehensiveAssessmentDetail(comprehensiveAssessmentRequest);
        }
        public async Task<ComprehensiveAssessmentDetailResponse> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            return await _ComprehensiveAssessmentRepository.HandleAssessmentVersioning(comprehensiveAssessmentRequest);
        }
        public async Task<ComprehensiveAssessmentDetailResponse> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            return await _ComprehensiveAssessmentRepository.GetComprehensiveAssessmentDetail(comprehensiveAssessmentRequest);
        }
        public async Task<ComprehensiveAssessmentPDFResponse> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            return await _ComprehensiveAssessmentRepository.PrintAssessmentPDF(comprehensiveAssessmentRequest);
        }

        public async Task<ComprehensiveAssessmentDetailResponse> UploadOfflinePDF(string json, string files)
        {
            return await _ComprehensiveAssessmentRepository.UploadOfflinePDF(json);
        }
        
    }
}
