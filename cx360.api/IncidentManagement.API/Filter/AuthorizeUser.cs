using IncidentManagement.Entities.Common;
using IncidentManagement.Entities.Request;
using IncidentManagement.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace IncidentManagement.API.Filter
{
    public class AuthorizeUser: AuthorizationFilterAttribute
    {
        private string token = string.Empty;
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (string.IsNullOrEmpty(actionContext.Request.Headers.GetValues("Token").First().ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = CustomErrorMessages.UNAUTHORIZED_ACCESS, Success = false });
                }
                else
                {
                    token = ConfigurationManager.AppSettings["Token"];
                    string staffAuthenticationToken = actionContext.Request.Headers.GetValues("Token").First().ToString();
                    if (!string.IsNullOrEmpty(staffAuthenticationToken))
                    {
                        if (token != staffAuthenticationToken)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = CustomErrorMessages.UNAUTHORIZED_ACCESS, Success = false });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
    }
}