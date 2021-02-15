using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Repository.IRepository;
using IncidentManagement.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.CANSResponse;

namespace IncidentManagement.Service.Service
{
   
    public class CANSService:ICANSService
    {
        #region Private
        private ICANSRepository _CANSRepository;
        #endregion
        public CANSService(ICANSRepository ICANSRepository)
        {
            _CANSRepository = ICANSRepository;
        }

        public async Task<CANSResponse> InsertModifyCANSTabs(CANSRequest cansRequest)
        {
            return await _CANSRepository.InsertModifyCANSTabs(cansRequest);
        }
        public async Task<CANSResponse> GetTreatmentPlanDetails(CANSRequest cansRequest)
        {
            return await _CANSRepository.GetTreatmentPlanDetails(cansRequest);
        }
        public async Task<CANSResponse> AddCansTreatmentPlanFields(TreatmentPlanRequest treatmentPlanRequest)
        {
            return await _CANSRepository.AddCansTreatmentPlanFields(treatmentPlanRequest);
        }
        public async Task<CANSResponse> ManageServiceInterventionActions(CANSRequest cansRequest)
        {
            return await _CANSRepository.ManageServiceInterventionActions(cansRequest);
        }
        public async Task<CANSResponse> HandleCansVersioning(CANSRequest cansRequest)
        {
            return await _CANSRepository.HandleCansVersioning(cansRequest);
        }
        public async Task<CANSResponse> GetCANSAssessmentDetails(CANSRequest cansRequest)
        {
            return await _CANSRepository.GetCANSAssessmentDetails(cansRequest);
        }
        public async Task<CANSResponse> GenerateAndImportXML(XMLImportRequest xmlImportRequest)
        {
            return await _CANSRepository.GenerateAndImportXML(xmlImportRequest);
        }
        public async Task<CANSAssessmentPDFResponse> PrintAssessmentPDF(CANSRequest cansRequest)
        {
            return await _CANSRepository.PrintAssessmentPDF(cansRequest);
        }

    }
}
