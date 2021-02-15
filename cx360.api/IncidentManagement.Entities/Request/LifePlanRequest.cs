using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Request
{
    public class LifePlanDetailRequest : CommonRequestParameter
    {
        public string TabName { get; set; }
        public string Json { get; set; }
        public string JsonChild { get; set; }
        public int? LifePlanId { get; set; }

        public string Mode { get; set; }
        public int? DocumentVersionId { get; set; }

        public string DocumentStatus { get; set; }

        public string DocumentVersion { get; set; }

        public int? PreviousLifePlanId { get; set; }
        public int ClientId { get; set; }

    }
    public class FillableLifePlanPDFRequest : CommonRequestParameter
    {

        public string TabName { get; set; }
        public int LifePlanId { get; set; }

        public int DocumentVersionId { get; set; }
        public string pdfJSON { get; set; }
        public string FilePath { get; set; }
        public string IndividualName { get; set; }
        public string DateOfBirth { get; set; }
        public string AddressLifePlan { get; set; }
        public string AddressCCO { get; set; }

    }
    public class SubmissionFormRequest : CommonRequestParameter
    {
        public int? UD_SubmissionDecisionFormID { get; set; }
        public string FormName { get; set; }
        public int ClientID { get; set; }
        public string KeyFieldID { get; set; }
        public string SubmittedTo { get; set; }
        public int Status { get; set; }
        public string ElectronicSignature { get; set; }
        public DateTime? ElectronicSignature_SignedOn { get; set; }
        public string StaffName { get; set; }
        public string StaffTitle { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public string SubmissionMessage { get; set; }
        public string TabName { get; set; }

    }
}