using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.IRepository;
using IncidentManagement.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.CCOComprehensiveAssessmentResponse;
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

        public async Task<ComprehensiveAssessmentDetailResponse> InsertModifyComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            return await _ComprehensiveAssessmentRepository.InsertModifyComprehensiveAssessmentDetail(comprehensiveAssessmentRequest, companyId);
        }
        public async Task<ComprehensiveAssessmentDetailResponse> HandleAssessmentVersioning(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            return await _ComprehensiveAssessmentRepository.HandleAssessmentVersioning(comprehensiveAssessmentRequest, companyId);
        }
        public async Task<ComprehensiveAssessmentDetailResponse> GetComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            return await _ComprehensiveAssessmentRepository.GetComprehensiveAssessmentDetail(comprehensiveAssessmentRequest, companyId);
        }
        public async Task<ComprehensiveAssessmentPDFResponse> PrintAssessmentPDF(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest, string companyId)
        {
            return await _ComprehensiveAssessmentRepository.PrintAssessmentPDF(comprehensiveAssessmentRequest, companyId);
        }

        public async Task<ComprehensiveAssessmentDetailResponse> UploadOfflinePDF(string json, string files, string companyId)
        {
            return await _ComprehensiveAssessmentRepository.UploadOfflinePDF(json, companyId);
        }
        public async Task<CCOComprehensiveAssessmentDetailResponse> GetCCOComprehensiveAssessmentDetail(ComprehensiveAssessmentRequest comprehensiveAssessmentRequest)
        {
            return await _ComprehensiveAssessmentRepository.GetCCOComprehensiveAssessmentDetail(comprehensiveAssessmentRequest);
        }

    }
}
