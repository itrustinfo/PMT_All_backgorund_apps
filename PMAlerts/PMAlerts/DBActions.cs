using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PMAlerts
{
    public class DBActions
    {
        public string GetConnectionString()
        {
            //return "Data Source=DESKTOP-N0V2F5N\\SQLEXPRESS2014;Initial Catalog=ProjectManagerServer;User ID=sa; Password=itrust; Connection Timeout=180";
            string sFileName = null;
            System.IO.StreamReader srFileReader = null;
            string sInputLine = null;
            sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Connection.txt";
            srFileReader = System.IO.File.OpenText(sFileName);
            sInputLine = srFileReader.ReadLine();
               return sInputLine;
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

        public DataSet getAlertLevel(Guid TaskUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_getAlertLevel", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@TaskUID", TaskUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public int InsertAlerts(Guid Alert_UID, Guid TaskUID, string Alert_Text, DateTime Alert_Date, Guid Alert_User, Guid ProjectUID, Guid WorkPackageUID,string Status)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertAlerts"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Alert_UID", Alert_UID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@Alert_Text", Alert_Text);
                        cmd.Parameters.AddWithValue("@Alert_Date", Alert_Date);
                        cmd.Parameters.AddWithValue("@Alert_User", Alert_User);
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@Status", Status);
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
    }
}
