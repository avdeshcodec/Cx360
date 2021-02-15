using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Repository.IRepository
{
    public  interface IIncidentManagementRepository
    {
        Task<IncidentManagementGeneralRespnse> GetIncidentManagement(IncidentManagementRequest incidentManagementRequest);
        Task<IncidentManagementTabsResponse> InsertModifyTabDetails(IncidentManagementTabRequest incidentManagementTabRequest);
        Task<AllPDFResponse> FillableStateFormPDF(FillablePDFRequest fillablePDFRequest);

        Task<IncidentManagementGeneralRespnse> DeleteMasterRecord(IncidentManagementRequest incidentManagementRequest);
        Task<IncidentManagementGeneralRespnse> EditAllRecord(IncidentManagementRequest incidentManagementRequest);
        Task<AllPDFUploadResponse> UploadPDFFiles(string json, string files);
        Task<AllPDFUploadResponse> DownloadUPloadedFile(IncidentManagementRequest incidentManagementRequest);
    }
}
