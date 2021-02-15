using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Response
{
   public class BaseResponse
    {
        private bool p_success = true;

        public Boolean Success
        {
            get
            {
                return p_success;
            }
            set
            {
                p_success = value;
            }
        }
        public string Message { get; set; }

        private bool p_isException = false;

        public Boolean IsException
        {
            get
            {
                return p_isException;
            }
            set
            {
                p_isException = value;
            }
        }
    }
}
