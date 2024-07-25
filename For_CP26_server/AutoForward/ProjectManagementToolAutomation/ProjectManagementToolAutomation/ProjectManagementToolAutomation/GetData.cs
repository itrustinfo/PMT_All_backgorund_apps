using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementToolAutomation
{
    public class GetData
    {
        private string ConString = string.Empty;
        public GetData()
        {
            ConString = ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString;
        }

        public DataSet GetDocumentFlowAutomation()
        {
            return ExecuteStoreProcedure("USP_GetAllRequiredDataForDocumentFlowAutomation");
        }

        public DataSet GetDocumentFlowAutomationPMCAccepted()
        {
            return ExecuteStoreProcedure("USP_GetAllRequiredDataForDocumentFlowAutomationPMC");
        }
        public DataSet GetDocumentFlowAutomationStpAll()
        {
            return ExecuteStoreProcedure("USP_GetAllRequiredDataForDocumentFlowAutomationAllSTP");
        }
        public DataSet GetDocumentStatus(string DocumentUID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@DocumentUID", DocumentUID }
            };
            return ExecuteStoreProcedure("USP_GetDocumentStatus", parameters);
        }

        public DataSet GetFlowMasterUser(string FlowUID, string step)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@FlowUID", FlowUID },
                { "@Step", step }
            };
            return ExecuteStoreProcedure("USP_GetFlowMasterUser", parameters);
        }
        public string GetSubmittedMiltipleUser(Guid DocumentUID, Guid ActualDocumentUID, string Step, string Current_Status)
        {
            string employeeName = string.Empty;
            try
            {
                List<string> employeeNames = new List<string>();
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetSubmittedMiltipleUser"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@ActualDocumentUID", ActualDocumentUID);
                        cmd.Parameters.AddWithValue("@Step", Step);
                        cmd.Parameters.AddWithValue("@Current_Status", Current_Status);
                        con.Open();
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        while(dataReader.Read())
                        {
                            employeeNames.Add(dataReader["FirstName"].ToString());
                            //if (string.IsNullOrEmpty(employeeName))
                            //    employeeName = dataReader["FirstName"].ToString();
                            //else
                            //    employeeName = employeeName + ", " + dataReader["FirstName"].ToString();
                        }
                        con.Close();
                    }
                }

                employeeName = String.Join(", ", employeeNames.Select(a => a).Distinct().ToList());
            }
            catch (Exception ex)
            {
                employeeName = string.Empty;
            }
            return employeeName;
        }

        public string GetWorkFlowPackageCategory(Guid FlowUID)
        {
            string category = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_GetWorkFlowPackageCategory"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@FlowUID", FlowUID);
                        con.Open();
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            if (string.IsNullOrEmpty(category))
                                category = dataReader["WorkPackageCategory_Name"].ToString();
                            else
                                category = category + ", " + dataReader["WorkPackageCategory_Name"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                category = string.Empty;
            }
            return category;
        }

        public int InsertDocumentStatus(DataTable dataTable)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {

                    using (SqlCommand cmd = new SqlCommand("USP_InsertDocumentStatusTable"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@DocumentStatus", dataTable);
                        sresult = cmd.ExecuteNonQuery();
                        
                    }

                    foreach(DataRow data in dataTable.Rows)
                    {
                        string actualDocumentUID = data["DocumentUID"].ToString();
                        string currentStatus = data["ActivityType"].ToString();

                        using (SqlCommand cmd = new SqlCommand("USP_UpdateActualDocumentStatus"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@ActualDocumentUID", actualDocumentUID);
                            cmd.Parameters.AddWithValue("@UpdateStatus", currentStatus);
                            sresult = cmd.ExecuteNonQuery();
                        }

                        //added on 07/07/2022 to calculate fesh dates
                        StoreFreshTargetDatesforStatusChange(new Guid(actualDocumentUID),currentStatus);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                sresult = 0;
            }
            return sresult;
        }

        public DataTable GetEmailCredentials()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(ConString);
                SqlDataAdapter cmd = new SqlDataAdapter("Usp_GetMailCredentials", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }
        public void StoreEmaildataToMailQueue(Guid MailUID, Guid UserUID, string FromEmailID, string ToEmailID, string Subject, string Body, string CCTo, string Attachment)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(ConString))
                {
                    SqlCommand SqlCmd = new SqlCommand("ups_Insert_Mails", SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.AddWithValue("@MailUID", MailUID);
                    SqlCmd.Parameters.AddWithValue("@UserUID", UserUID);
                    SqlCmd.Parameters.AddWithValue("@FromEmailID", FromEmailID);
                    SqlCmd.Parameters.AddWithValue("@ToEmailID", ToEmailID);
                    SqlCmd.Parameters.AddWithValue("@Subject", Subject);
                    SqlCmd.Parameters.AddWithValue("@Body", Body);
                    SqlCmd.Parameters.AddWithValue("@CCTo", CCTo);
                    SqlCmd.Parameters.AddWithValue("@MailSentDate", DateTime.Now);
                    SqlCmd.Parameters.AddWithValue("@Attachment", Attachment);
                    SqlCmd.Parameters.AddWithValue("@MailSent", "N");
                    SqlConn.Open();
                    SqlCmd.ExecuteNonQuery();
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public int StoreFreshTargetDatesforStatusChange(Guid ActualDocumentUID, string CurrentStatus)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_StoreFreshTargetDatesforStatusChange"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@ActualDocumentUID", ActualDocumentUID);
                        cmd.Parameters.AddWithValue("@CurrentStatus", CurrentStatus);
                        sresult = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                sresult = 0;
            }
            return sresult;
        }


        private DataSet ExecuteStoreProcedure(string ProcedureName)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    SqlDataAdapter cmd = new SqlDataAdapter(ProcedureName, con);
                    cmd.SelectCommand.CommandTimeout = 0;
                    cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                    cmd.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        private DataSet ExecuteStoreProcedure(string ProcedureName, Dictionary<string, string> Parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    SqlDataAdapter cmd = new SqlDataAdapter(ProcedureName, con);
                    cmd.SelectCommand.CommandTimeout = 0;
                    cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                    foreach (var eachParameter in Parameters)
                    {
                        cmd.SelectCommand.Parameters.AddWithValue(eachParameter.Key, eachParameter.Value);
                    }
                    cmd.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }
    }
}
