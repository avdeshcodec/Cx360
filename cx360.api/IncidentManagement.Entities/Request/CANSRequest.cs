using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Request
{
  public  class CANSRequest: CommonRequestParameter
    {
        public string TabName { get; set; }
        public string Json { get; set; }
        public string JsonChildFirstTable { get; set; }
        public string JsonChildSecondTable { get; set; }
        public int? CansVersioningID { get; set; }
        public int? GeneralInformationID { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentVersion { get; set; }
        public int? PreviousGeneralInformationID { get; set; }

        public int? TreatmentPlanId { get; set; }

        public int? GoalID { get; set; }
        
        public int? ServiceInterventionID { get; set; }
        public string CansItems { get; set; }
        public string GoalStatus { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string JsonChildThirdTable { get; set; }

        public string TableName { get; set; }


    }
    public class TreatmentPlanRequest : CommonRequestParameter
    {
        public string Action { get; set; }
        public string Json { get; set; }
        public int? ParentId { get; set; }
        public int? KeyId { get; set; }
        public string Description { get; set; }
        public string TabName { get; set; }

     

    }
    public class XMLImportRequest : CommonRequestParameter
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ImportType { get; set; }

        public int? RecordTotal { get; set; }
        public int? RunType { get; set; }
    }
}
