using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Request
{
    public class APICompanyIdParameter
    {
        public int? CompanyId { get; set; }
    }

    public class APILogIdParameter 
    {
        public int? APILogId { get; set; }
    }

    public class CommonRequestParameter 
    {
        public int? ReportedBy { get; set; }
        public string Mode { get; set; }


    }

    public class ErrorLogRequest : APILogIdParameter
    {
        public string ExceptionMsg { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionSource { get; set; }
    }

 
}
