using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace API.Models.DAL
{
    public class SQLHelper
    {
        public string GetDataFromSQL(string procedure, List<SQLHelperParams> parms)
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["PFPDBConnection"].ConnectionString;

            try
            {
                SqlConnection conn = new SqlConnection(connString);
                SqlCommand sqlcmd = new SqlCommand(procedure, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (parms != null && parms.Count > 0)
                {
                    foreach (SQLHelperParams parm in parms)
                    {
                        sqlcmd.Parameters.AddWithValue(parm.Param, parm.Value);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                conn.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class SQLHelperParams
    {
        public string Param { get; set; }
        public object Value { get; set; }
    }
}