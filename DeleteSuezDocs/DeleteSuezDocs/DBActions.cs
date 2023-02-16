using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Threading;

namespace DeleteSuezDocs
{
    public class DBActions
    {
        public string GetConnectionString()
        {

            return System.Configuration.ConfigurationManager.ConnectionStrings["PMConnectionString"].ToString();

        }


        public int GetDashboardContractotDocsSubmitted(Guid ProjectUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_GetDashboardContractotDocsSubmitted"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        con.Open();
                        sresult = (int)cmd.ExecuteScalar();
                        con.Close();

                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = 0;
            }
        }

        public DataSet GetDashboardContractotDocsSubmitted_Details(Guid ProjectUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetDashboardContractotDocsSubmitted_Details", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int Documents_Delete_by_DocID(Guid DocumentUID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Documents_Delete_by_DocID"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
                        con.Open();
                        sresult = (int)cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = 0;
            }
        }

       
       

        public int ActualDocuments_Delete_by_DocID(Guid ActualDocumentUID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("ActualDocuments_Delete_by_DocID"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@ActualDocumentUID", ActualDocumentUID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
                        con.Open();
                        sresult = (int)cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = 0;
            }
        }

    }
}
