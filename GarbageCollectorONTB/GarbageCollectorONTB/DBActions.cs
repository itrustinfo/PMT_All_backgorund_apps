using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Threading;

namespace GarbageCollectorONTB
{
    public class DBActions
    {
        public string GetConnectionString()
        {
           
            return System.Configuration.ConfigurationManager.ConnectionStrings["PMConnectionString"].ToString();

        }


        public DataSet GetWorkPackages_By_ProjectUID(Guid ProjectUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("Usp_getWorkPackge_by_ProjectUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error : " + ex.Message);
            }
            return ds;
        }

        public string getWorkPackageNameby_WorkPackageUID(Guid WorkPackageUID)
        {
            string sUser = "";
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_getWorkPackageNameby_WorkPackageUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                sUser = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                sUser = "Error : " + ex.Message;
            }
            return sUser;
        }

        public DataSet GetAllDelayedTasks_by_WorkPackageUID(Guid WorkPackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllDelayedTasks_by_WorkPackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;

                Console.WriteLine("Error : in delay " + ex.Message);
                Thread.Sleep(10000);
            }
            return ds;
        }

        public string getTaskParents(Guid TaskUID)
        {
            string ParentTasks = "";
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_getTaskParentNames", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                ParentTasks = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                ParentTasks = "Error : " + ex.Message;
            }
            return ParentTasks;
        }

        public string getUserEmails(Guid TaskUID)
        {
            string ParentTasks = "";
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_getUserEmails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                ParentTasks = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                ParentTasks = "";
            }
            return ParentTasks;
        }

        public string SendMail(string toEmailID, string Subject, string sHtmlString, string cc, string Attachment)
        {
            try
            {
                MailMessage mm = new MailMessage();
                DataTable dtemailCred = GetEmailCredentials();
                mm.To.Add(toEmailID);
                mm.From = new MailAddress(dtemailCred.Rows[0][0].ToString(), "Project Manager");
                mm.Subject = Subject;
                if (cc != "")
                {
                    string[] CCId = cc.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        mm.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                    }
                }
                if (Attachment != "")
                {
                    mm.Attachments.Add(new Attachment(Attachment));
                }
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(dtemailCred.Rows[0][0].ToString(), dtemailCred.Rows[0][1].ToString());
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                mm.IsBodyHtml = true;
                mm.Body = string.Format(sHtmlString);
                client.Send(mm);
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failure : " + ex.Message;
            }
        }

        public DataTable GetEmailCredentials()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
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

        public string getUserEmail_by_UserUID(Guid UserUID)
        {
            string ParentTasks = "";
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_getUserEmail_By_UserUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserUID", UserUID);
                ParentTasks = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                ParentTasks = "";
            }
            return ParentTasks;
        }

        public int checkAlertMailSent(Guid TaskUID)
        {
            int Cnt = 0;
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_checkAlertMailSent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                Cnt = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                Cnt = 0;
            }
            return Cnt;
        }

        // added on 18/11/2020
        public DataTable getAllTasksForAlert()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllTasksForAlert", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;

        }

        public DataTable getAllBankGuarantees()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllBankGuarantees", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;

        }

        public DataTable getAllInsurances()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllInsurances", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;

        }

        public DataTable getAllIssues()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetAllIssues", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;

        }

        public DataSet GetTaskScheduleDatesforGraph(Guid WorkpackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetTaskScheduleDatesforGraph", con);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetTaskScheduleValuesForGraph(Guid WorkpackageUID, DateTime startdate, DateTime enddate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetTaskScheduleValuesForGraph", con);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkpackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@StartDate", startdate);
                cmd.SelectCommand.Parameters.AddWithValue("@EndDate", enddate);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet GetTaskActualValuesForGraph(Guid WorkpackageUID, DateTime startdate, DateTime enddate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetTaskActualValuesForGraph", con);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkpackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@StartDate", startdate);
                cmd.SelectCommand.Parameters.AddWithValue("@EndDate", enddate);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int InsertGraphPhysicalProgressValues(Guid PhysicalProgress_UID, Guid ProjectUID, Guid WorkPackageUID, DateTime Start_Date, decimal PlanValue,decimal CumPlanValue,decimal AchValue,decimal CumAchValue)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("InsertPhysicalProgressGraphValues"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@GraphPhysicalProgressUID", PhysicalProgress_UID);
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@StartDate", Start_Date);
                        cmd.Parameters.AddWithValue("@PlanValue", PlanValue);
                        cmd.Parameters.AddWithValue("@CumPlanValue", CumPlanValue);
                        cmd.Parameters.AddWithValue("@AchValue", AchValue);
                        cmd.Parameters.AddWithValue("@CumAchValue", CumAchValue);
                        
                        con.Open();
                        cnt = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }

        public int DeleteGraphPhysicalProgressValues(Guid ProjectUID, Guid WorkPackageUID)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("DeletePhysicalProgressGraphValues"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        
                        con.Open();
                        cnt = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }

        public DataSet GetWorkPackages()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("garbage_GetWorkPackges", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error : " + ex.Message);
            }

            return ds;
        }
        public DataSet GetAllIssueDocsByWorkPackageUID(Guid WorkPackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("GetAllIssueDocsByWorkPackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("WPUID", WorkPackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error : " + ex.Message);
            }

            return ds;
        }

        public DataSet garbage_GetAllDocumentsby_ProjectUID(Guid ProjectUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("garbage_GetAllDocumentsby_ProjectUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                Console.WriteLine("Error : " + ex.Message);
            }

            return ds;
        }

    }
}
