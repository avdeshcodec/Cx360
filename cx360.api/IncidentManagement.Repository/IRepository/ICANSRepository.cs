using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IncidentManagement.Entities.Response.CANSResponse;

namespace IncidentManagement.Repository.IRepository
{
   public interface ICANSRepository
    {
        #region interface methods
        Task<CANSResponse> InsertModifyCANSTabs(CANSRequest cansRequest);
        Task<CANSResponse> GetTreatmentPlanDetails(CANSRequest cansRequest);
        Task<CANSResponse> AddCansTreatmentPlanFields(TreatmentPlanRequest treatmentPlanRequest);
        Task<CANSResponse> ManageServiceInterventionActions(CANSRequest cansRequest);
        Task<CANSResponse> HandleCansVersioning(CANSRequest cansRequest);
        Task<CANSResponse> GetCANSAssessmentDetails(CANSRequest cansRequest);

        Task<CANSResponse> GenerateAndImportXML(XMLImportRequest xmlImportRequest);
        Task<CANSAssessmentPDFResponse> PrintAssessmentPDF(CANSRequest cansRequest);
        #endregion
    }
}
