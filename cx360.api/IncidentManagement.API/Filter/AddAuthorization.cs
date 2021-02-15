using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace IncidentManagement.API.Filter
{
    public class AddAuthorization:IOperationFilter
    {
        /// <summary>
        /// Adds an authorization header to the given operation in Swagger.
        /// </summary>
        /// <param name="operation">The Swashbuckle operation.</param>
        /// <param name="schemaRegistry">The Swashbuckle schema registry.</param>
        /// <param name="apiDescription">The Swashbuckle api description.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation == null) return;

            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            var parameter = new Parameter
            {
                description = "Authorize token(return by GetToken API) Eg:- Basic {token}",
                @in = "header",
                name = "Authorization",
                required = true,
                type = "string",
            };

            if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                parameter.required = false;

            }
            //operation.parameters.Add(new Parameter
            //{
            //    name = "Source",
            //    description = "It require Key name of connection string.",
            //    @in = "header",
            //    type = "string",
            //    required = true
            //});


       

            operation.parameters.Add(parameter);
        }

    }
}