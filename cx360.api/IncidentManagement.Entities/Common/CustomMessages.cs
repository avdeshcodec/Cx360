using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Common
{
    /// <summary>
    /// This class contain all the error message in the application.
    /// </summary>
    public static class CustomErrorMessages
    {
        public static readonly string UNAUTHORIZED_ACCESS = "Unauthorized access";
        public static readonly string INTERNAL_ERROR = "Something went wrong. Please try again later.";
        public static readonly string INVALID_INPUTS = "Request contains invalid parameters. Please try again later.";
        public static readonly string CLIENT_ALREADY_EXIST = "Client already exist.";
        public static readonly string CLIENT_NOT_EXIST = "Client not exist.";
        public static readonly string COMPANYCONFIGURATION_INCORRECT = "Company configuration incorrect.";
        public static readonly string PASSWORD_EXPIRED = "The password is expired. Please contact security staff to reset it.";

    }


    /// <summary>
    /// This class contain all the success message in the application.
    /// </summary>
    public static class CustomSuccessMessages
    {
        public static readonly string AUTHORIZED_SUCCESS = "Authorized sucessfully.";
        public static readonly string CREATED_SUCCESS = "Created sucessfully.";
        public static readonly string USERNAMECHANGED_SUCCESS = "User name Changed sucessfully.";
        public static readonly string RECORD_DELETED_SUCCESS = "Record deleted sucessfully.";
        public static readonly string RECORD_UPDATED_SUCCESS = "Record updated sucessfully.";
        public static readonly string REQUEST_PROCESSED_SUCCESS = "Request processed sucessfully.";
        public static readonly string LOG_DELETED_SUCESS = "Logs deleted sucessfully.";
    }
    /// <summary>
    /// return the sp name based on 
    /// </summary>
    public static class StoreProcedureName
    {
        public static readonly string GENERAL_SP = "usp_InsertModifyIncidentManagementGeneral";
    }
}
