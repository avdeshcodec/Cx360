using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Repository.Common
{
    public  class ConnectionString
    {
        public static async Task<string>  GetConnectionString(string companyId)
        {
            string connectionString = string.Empty;
            DataSet dataSet = new DataSet();
            if (!string.IsNullOrEmpty(companyId))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["connectionString"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetConnectionString", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_CompanyID", SqlDbType.VarChar).Value = companyId;
                        cmd.Parameters.Add("@P_Action", SqlDbType.VarChar).Value = "GetConnection";
                        con.Open();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = cmd;
                        await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                        con.Close();

                    }
                }
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                     connectionString = dataSet.Tables[0].Rows[0]["SecureDBConn"].ToString();
                }
            }
            else
            {
                connectionString= ConfigurationManager.AppSettings["localhost"].ToString();

            }
            return connectionString;
        }

    }
}
