using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IncidentManagement.Entities.Request
{
   public class IncidentManagementRequest: APILogIdParameter
    {
      
            public int IncidentManagementId { get; set; }

         public int IncidentType { get; set; }
        public string TabName { get; set; }
        public string Json { get; set; }
        public string ReportedBy { get; set; }

        public int StateFormPDFVersioningId { get; set; }

    }

    public class IncidentManagementTabRequest : CommonRequestParameter
    {

        public string TabName { get; set; }
        public string Json { get; set; }

        public string JsonChild { get; set; }

    }
    public class FillablePDFRequest : CommonRequestParameter
    {

        public string TabName { get; set; }
        public int IncidentManagementId { get; set; }

        public int StateFormId { get; set; }
        public string pdfJSON { get; set; }

        public string FileName { get; set; }

    }
    public class UploadPDFRequest
    {
        public int IncidentManagementId { set; get; }
        public int FormType { set; get; }
        public int StatesFormOPWDD147Id { set; get; }
        public int StatesFormOPWDD148Id { set; get; }
        public int StatesFormOPWDD150Id { set; get; }
        public int ReportedBy { set; get; }
        public string fileName { set; get; }
        public string mediaType { get; set; }
        public byte[] data { get; set; }
    }

    
}
