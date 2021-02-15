using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Request
{
   public class ComprehensiveAssessmentRequest: CommonRequestParameter
    {
        public string TabName { get; set; }
        public string Json { get; set; }
        public string JsonChildFirstTable { get; set; }
        public string JsonChildSecondTable { get; set; }

        public int? ComprehensiveAssessmentId { get; set; }
        public int?AssessmentVersioningId { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentVersion { get; set; }
        public int? PreviousComprehensiveAssessmentId { get; set; }
    }
}
