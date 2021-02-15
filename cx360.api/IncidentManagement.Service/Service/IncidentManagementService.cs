using IncidentManagement.Repository.IRepository;
using IncidentManagement.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncidentManagement.Entities.Request;
using IncidentManagement.Entities.Response;
using IncidentManagement.Entities.Common;
using IncidentManagement.Repository.Common;

namespace IncidentManagement.Service.Service
{
  public  class IncidentManagementService:IIncidentManagementService
    {
        #region Private
        private IIncidentManagementRepository _IIncidentManagementRepository;
        #endregion

        public IncidentManagementService(IIncidentManagementRepository IIncidentManagementRepository)
        {
            _IIncidentManagementRepository = IIncidentManagementRepository;
        }

        public async Task<IncidentManagementGeneralRespnse> GetIncidentManagement(IncidentManagementRequest incidentManagementRequest)
        {
            return await _IIncidentManagementRepository.GetIncidentManagement(incidentManagementRequest);
        }
      

        public async Task<IncidentManagementTabsResponse> InsertModifyTabDetails(IncidentManagementTabRequest incidentManagementTabRequest)
        {
           return await _IIncidentManagementRepository.InsertModifyTabDetails( incidentManagementTabRequest);
        }

        public async Task<AllPDFResponse> FillableStateFormPDF(FillablePDFRequest fillablePDFRequest)
        {
            return await _IIncidentManagementRepository.FillableStateFormPDF(fillablePDFRequest);

        }

        public async Task<IncidentManagementGeneralRespnse> DeleteMasterRecord(IncidentManagementRequest incidentManagementRequest)
        {
            return await _IIncidentManagementRepository.DeleteMasterRecord(incidentManagementRequest);

        }
        public async Task<IncidentManagementGeneralRespnse> EditAllRecord(IncidentManagementRequest incidentManagementRequest)
        {
            return await _IIncidentManagementRepository.EditAllRecord(incidentManagementRequest);

        }

        public async Task<AllPDFUploadResponse> UploadPDFFiles(string json, string files)
        {
            return await _IIncidentManagementRepository.UploadPDFFiles(json, files);
        }

        public async Task<AllPDFUploadResponse> DownloadUPloadedFile(IncidentManagementRequest incidentManagementRequest)
        {
            return await _IIncidentManagementRepository.DownloadUPloadedFile(incidentManagementRequest);
        }

        

    }
}
