using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WkpgMasterDataSendMail
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.StoreMails();
            //
            obj.StoreRemiderMails();
        }

        public string GetConnectionString()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Connection.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                // return "Data Source=192.168.1.11\\SQLEXPRESS;Initial Catalog=Healthicare;User ID=sa;Password=itrust";
                // return "Data Source=localhost\\SQL2008;Initial Catalog=ERPDemo;Integrated Security=false;User ID=sa;Password=itrust@123";
                return sInputLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        public string GetMailDate()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\MailDate.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                // return "Data Source=192.168.1.11\\SQLEXPRESS;Initial Catalog=Healthicare;User ID=sa;Password=itrust";
                // return "Data Source=localhost\\SQL2008;Initial Catalog=ERPDemo;Integrated Security=false;User ID=sa;Password=itrust@123";
                return sInputLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        public string GetReminderMailDate()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\ReminderMailDate.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                // return "Data Source=192.168.1.11\\SQLEXPRESS;Initial Catalog=Healthicare;User ID=sa;Password=itrust";
                // return "Data Source=localhost\\SQL2008;Initial Catalog=ERPDemo;Integrated Security=false;User ID=sa;Password=itrust@123";
                return sInputLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        private void StoreMails()
        {
            try
            {
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                DataSet dsWkpg = new DataSet();
                DataSet dsMailsentdate = new DataSet();
                DateTime lastsentdate;
                DataSet dsUsers = new DataSet();
                string Username = string.Empty;
                string ToEmailID = string.Empty;
                int MailDate = int.Parse(GetMailDate());
                bool Tosent = false;
                string Subject = "Project WorkPackage Master Data Update Alert !";
                string sHtmlString = string.Empty;
                string PrjName = string.Empty;
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("SELECT * From PrjMasterMailSettings", MyConnection);
                string Frequency = string.Empty;
                if (MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    MyConnection.Open();
                }
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);

                if (MyDataset.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("Storing Mails for users for master data updates. Please wait...");
                    foreach (System.Data.DataRow MyRow in MyDataset.Tables[0].Rows)
                    {
                        sHtmlString = "";
                        dsWkpg = GetPrjMasterMailSettings(new Guid(MyRow["ProjectUID"].ToString()), new Guid(MyRow["WorkPackageUID"].ToString()));
                        if(dsWkpg.Tables[1].Rows.Count > 0)
                        {
                            Frequency = dsWkpg.Tables[1].Rows[0]["Frequency"].ToString();
                            PrjName = getProjectNameby_ProjectUID(new Guid(MyRow["ProjectUID"].ToString())) + " (" + getWorkPackageNameby_WorkPackageUID(new Guid(MyRow["WorkPackageUID"].ToString())) + ")";
                            if (Frequency == "Monthly")
                            {
                                //store  the mails for each user
                                dsMailsentdate = GetUserlastMailSent(new Guid(MyRow["ProjectUID"].ToString()), new Guid(MyRow["WorkPackageUID"].ToString()), new Guid(MyRow["UserUID"].ToString()));
                                if (dsMailsentdate.Tables[0].Rows.Count > 0)
                                {
                                    lastsentdate = Convert.ToDateTime(dsMailsentdate.Tables[0].Rows[0]["MailSentDate"].ToString());
                                    if (lastsentdate.Month == DateTime.Now.Month && lastsentdate.Year == DateTime.Now.Year)
                                    {
                                        Tosent = false;
                                    }
                                }
                                else
                                {
                                    Tosent = true;
                                }

                                if (Tosent && DateTime.Now.Day == MailDate)
                                {
                                    dsUsers = get_UserDetails(new Guid(MyRow["UserUID"].ToString()));

                                    if (dsUsers.Tables[0].Rows.Count > 0)
                                    {
                                        ToEmailID = dsUsers.Tables[0].Rows[0]["EmailID"].ToString();
                                        Username = dsUsers.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsUsers.Tables[0].Rows[0]["LastName"].ToString();
                                      
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                          "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                             "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";

                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";

                                        //else
                                        //{
                                        //    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        //}
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                   "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear " + Username + ",<br/><br/><span style='font-weight:bold;'> Please update the WorkPackage Master data for " + PrjName + "..</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>Budget </b></td></tr>" +
                                                        "<tr><td><b>Actual Expenditure </b></td></tr>" +
                                                        "<tr><td><b>Project End Date </b></td></tr>";
                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                        // added on 02/11/2020
                                        DataTable dtemailCred = GetEmailCredentials();
                                        Guid MailUID = Guid.NewGuid();
                                        StoreEmaildataToMailQueue(MailUID, new Guid(MyRow["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                        //


                                        MyCommand.Parameters.Clear();
                                        MyCommand.CommandText = "INSERT INTO[dbo].[WkpgMasterDataMailSent] ([ProjectUID],[WorkPackageUID],[UserUID],[MailSentDate],[Frequency])" +
                      " VALUES (@ProjectUID,@WorkPackageUID,@UserUID,@MailSentDate,@Frequency)";
                                        MyCommand.Parameters.AddWithValue("@ProjectUID", MyRow["ProjectUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@WorkPackageUID", MyRow["WorkPackageUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@UserUID", MyRow["UserUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@MailSentDate", DateTime.Now);
                                        MyCommand.Parameters.AddWithValue("@Frequency", Frequency);
                                        MyCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            else if (Frequency == "Quarterly")
                            {
                                //store  the mails for each user
                                dsMailsentdate = GetUserlastMailSent(new Guid(MyRow["ProjectUID"].ToString()), new Guid(MyRow["WorkPackageUID"].ToString()), new Guid(MyRow["UserUID"].ToString()));
                                if (dsMailsentdate.Tables[0].Rows.Count > 0)
                                {
                                    lastsentdate = Convert.ToDateTime(dsMailsentdate.Tables[0].Rows[0]["MailSentDate"].ToString());
                                    if (lastsentdate.Month == 1 || lastsentdate.Month == 4 || lastsentdate.Month == 8 || lastsentdate.Month == 12)
                                    {
                                        if (lastsentdate.Year == DateTime.Now.Year)
                                        {
                                            Tosent = false;
                                        }
                                    }
                                }
                                else
                                {
                                    Tosent = true;
                                }

                                if (Tosent && DateTime.Now.Day == MailDate)
                                {
                                    dsUsers = get_UserDetails(new Guid(MyRow["UserUID"].ToString()));

                                    if (dsUsers.Tables[0].Rows.Count > 0)
                                    {
                                        ToEmailID = dsUsers.Tables[0].Rows[0]["EmailID"].ToString();
                                        Username = dsUsers.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsUsers.Tables[0].Rows[0]["LastName"].ToString();
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                          "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                             "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";

                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";

                                        //else
                                        //{
                                        //    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        //}
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                  "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear " + Username + ",<br/><br/><span style='font-weight:bold;'> Please update the WorkPackage Master data for " + PrjName + "..</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>•Budget </b></td></tr>" +
                                                        "<tr><td><b>•Actual Expenditure </b></td></tr>" +
                                                        "<tr><td><b>•Project End Date </b></td></tr>";
                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                        // added on 02/11/2020
                                        DataTable dtemailCred = GetEmailCredentials();
                                        Guid MailUID = Guid.NewGuid();
                                        StoreEmaildataToMailQueue(MailUID, new Guid(MyRow["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                        //


                                        MyCommand.Parameters.Clear();
                                        MyCommand.CommandText = "INSERT INTO[dbo].[WkpgMasterDataMailSent] ([ProjectUID],[WorkPackageUID],[UserUID],[MailSentDate],[Frequency])" +
                    " VALUES (@ProjectUID,@WorkPackageUID,@UserUID,@MailSentDate,@Frequency)";
                                        MyCommand.Parameters.AddWithValue("@ProjectUID", MyRow["ProjectUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@WorkPackageUID", MyRow["WorkPackageUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@UserUID", MyRow["UserUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@MailSentDate", DateTime.Now);
                                        MyCommand.Parameters.AddWithValue("@Frequency", Frequency);
                                        MyCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {

            }
        }

        private void StoreRemiderMails()
        {
            try
            {
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                DataSet dsWkpg = new DataSet();
                DataSet dsMailsentdate = new DataSet();
                DateTime lastsentdate;
                DataSet dsUsers = new DataSet();
                string Username = string.Empty;
                string ToEmailID = string.Empty;
                string PrjName = string.Empty;
                int MailDate = int.Parse(GetMailDate()) + int.Parse(GetReminderMailDate());
                bool Tosent = false;
                string Subject = "Reminder : Project WorkPackage Master Data Update Alert !";
                string sHtmlString = string.Empty;
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("SELECT * From PrjMasterMailSettings", MyConnection);
                string Frequency = string.Empty;
                if (MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    MyConnection.Open();
                }
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);

                if (MyDataset.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("Storing Reminder Mails for users for master data updates. Please wait...");
                    foreach (System.Data.DataRow MyRow in MyDataset.Tables[0].Rows)
                    {
                        sHtmlString = "";
                        dsWkpg = GetPrjMasterMailSettings(new Guid(MyRow["ProjectUID"].ToString()), new Guid(MyRow["WorkPackageUID"].ToString()));
                        if (dsWkpg.Tables[1].Rows.Count > 0)
                        {
                            Frequency = dsWkpg.Tables[1].Rows[0]["Frequency"].ToString();
                            PrjName = getProjectNameby_ProjectUID(new Guid(MyRow["ProjectUID"].ToString())) + " (" + getWorkPackageNameby_WorkPackageUID(new Guid(MyRow["WorkPackageUID"].ToString())) + ")";
                            if (Frequency == "Monthly")
                            {
                                //store  the mails for each user
                                if (!checkDataUpdated(new Guid(MyRow["WorkPackageUID"].ToString())))
                                {
                                    Tosent = true;
                                }
                                dsMailsentdate = GetUserlastMailSent(new Guid(MyRow["ProjectUID"].ToString()), new Guid(MyRow["WorkPackageUID"].ToString()), new Guid(MyRow["UserUID"].ToString()));
                                if (dsMailsentdate.Tables[0].Rows.Count > 0)
                                {
                                    lastsentdate = Convert.ToDateTime(dsMailsentdate.Tables[0].Rows[0]["MailSentDate"].ToString());
                                    if (lastsentdate.Month == DateTime.Now.Month && lastsentdate.Date == DateTime.Now.Date && lastsentdate.Year == DateTime.Now.Year)
                                    {
                                        Tosent = false;
                                    }
                                }
                                else
                                {
                                    Tosent = true;
                                }

                               
                                if (Tosent && DateTime.Now.Day == MailDate)
                                {
                                    dsUsers = get_UserDetails(new Guid(MyRow["UserUID"].ToString()));

                                    if (dsUsers.Tables[0].Rows.Count > 0)
                                    {
                                        ToEmailID = dsUsers.Tables[0].Rows[0]["EmailID"].ToString();
                                        Username = dsUsers.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsUsers.Tables[0].Rows[0]["LastName"].ToString();
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                          "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                             "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";

                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";

                                        //else
                                        //{
                                        //    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        //}
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                   "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear " + Username + ",<br/><br/><span style='font-weight:bold;'> Please update the WorkPackage Master data for " + PrjName + "..</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>Budget </b></td></tr>" +
                                                        "<tr><td><b>Actual Expenditure </b></td></tr>" +
                                                        "<tr><td><b>Projected End Date </b></td></tr>";
                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                        // added on 02/11/2020
                                        DataTable dtemailCred = GetEmailCredentials();
                                        Guid MailUID = Guid.NewGuid();
                                        StoreEmaildataToMailQueue(MailUID, new Guid(MyRow["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                        //


                                        MyCommand.Parameters.Clear();
                                        MyCommand.CommandText = "INSERT INTO[dbo].[WkpgMasterDataMailSent] ([ProjectUID],[WorkPackageUID],[UserUID],[MailSentDate],[Frequency])" +
                      " VALUES (@ProjectUID,@WorkPackageUID,@UserUID,@MailSentDate,@Frequency)";
                                        MyCommand.Parameters.AddWithValue("@ProjectUID", MyRow["ProjectUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@WorkPackageUID", MyRow["WorkPackageUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@UserUID", MyRow["UserUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@MailSentDate", DateTime.Now);
                                        MyCommand.Parameters.AddWithValue("@Frequency", Frequency);
                                        MyCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            else if (Frequency == "Quarterly")
                            {
                                //store  the mails for each user
                               
                                if (DateTime.Now.Month == 1 || DateTime.Now.Month == 4 || DateTime.Now.Month == 8 || DateTime.Now.Month == 12)
                                {
                                    if (!checkDataUpdated(new Guid(MyRow["WorkPackageUID"].ToString())))
                                    {
                                        Tosent = true;
                                    }

                                    dsMailsentdate = GetUserlastMailSent(new Guid(MyRow["ProjectUID"].ToString()), new Guid(MyRow["WorkPackageUID"].ToString()), new Guid(MyRow["UserUID"].ToString()));
                                    if (dsMailsentdate.Tables[0].Rows.Count > 0)
                                    {
                                        lastsentdate = Convert.ToDateTime(dsMailsentdate.Tables[0].Rows[0]["MailSentDate"].ToString());
                                        if (lastsentdate.Month == DateTime.Now.Month && lastsentdate.Date == DateTime.Now.Date && lastsentdate.Year == DateTime.Now.Year)
                                        {
                                            Tosent = false;
                                        }
                                    }
                                    else
                                    {
                                        Tosent = true;
                                    }
                                }

                                if (Tosent && DateTime.Now.Day == MailDate)
                                {
                                    dsUsers = get_UserDetails(new Guid(MyRow["UserUID"].ToString()));

                                    if (dsUsers.Tables[0].Rows.Count > 0)
                                    {
                                        ToEmailID = dsUsers.Tables[0].Rows[0]["EmailID"].ToString();
                                        Username = dsUsers.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsUsers.Tables[0].Rows[0]["LastName"].ToString();
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                          "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                             "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";

                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";

                                        //else
                                        //{
                                        //    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        //}
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                  "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear " + Username + ",<br/><br/><span style='font-weight:bold;'> Please update the WorkPackage Master data for " + PrjName + "..</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>•Budget </b></td></tr>" +
                                                        "<tr><td><b>•Actual Expenditure </b></td></tr>" +
                                                        "<tr><td><b>•Project End Date </b></td></tr>";
                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                        // added on 02/11/2020
                                        DataTable dtemailCred = GetEmailCredentials();
                                        Guid MailUID = Guid.NewGuid();
                                        StoreEmaildataToMailQueue(MailUID, new Guid(MyRow["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                        //


                                        MyCommand.Parameters.Clear();
                                        MyCommand.CommandText = "INSERT INTO[dbo].[WkpgMasterDataMailSent] ([ProjectUID],[WorkPackageUID],[UserUID],[MailSentDate],[Frequency])" +
                    " VALUES (@ProjectUID,@WorkPackageUID,@UserUID,@MailSentDate,@Frequency)";
                                        MyCommand.Parameters.AddWithValue("@ProjectUID", MyRow["ProjectUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@WorkPackageUID", MyRow["WorkPackageUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@UserUID", MyRow["UserUID"].ToString());
                                        MyCommand.Parameters.AddWithValue("@MailSentDate", DateTime.Now);
                                        MyCommand.Parameters.AddWithValue("@Frequency", Frequency);
                                        MyCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {

            }
        }

        public DataSet GetPrjMasterMailSettings(Guid ProjectUID, Guid WorkPackageUID)
        {
            Program obj = new Program();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());
            try
            {
                SqlDataAdapter cmd = new SqlDataAdapter("usp_PrjMasterMailSettings", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;

            }
            return ds;
        }

        public DataSet GetUserlastMailSent(Guid ProjectUID, Guid WorkPackageUID,Guid UserUID)
        {
            Program obj = new Program();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());
            try
            {
                SqlDataAdapter cmd = new SqlDataAdapter("select Top 1 (MailSentDate) From [WkpgMasterDataMailSent] Where UserUID='" + UserUID +"' and WorkPackageUID='" + WorkPackageUID + "' and ProjectUID='" + ProjectUID + "' Order by MailSentDate DEsc", con);
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;

            }
            return ds;
        }

        public DataTable GetEmailCredentials()
        {
            Program obj = new Program();
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(obj.GetConnectionString());
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

        public DataSet get_UserDetails(Guid sUserUID)
        {
            Program obj = new Program();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(obj.GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_Get_UserDetails", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@UserUID", sUserUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public void StoreEmaildataToMailQueue(Guid MailUID, Guid UserUID, string FromEmailID, string ToEmailID, string Subject, string Body, string CCTo, string Attachment)
        {
            try
            {
                Program obj = new Program();
                using (SqlConnection SqlConn = new SqlConnection(obj.GetConnectionString()))
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

        public DataSet GetWorkPackageDataHistory(Guid WorkPackageUID, string type)
        {
            Program obj = new Program();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(obj.GetConnectionString());
            try
            {
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetWorkPackageDataHistory", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.SelectCommand.Parameters.AddWithValue("@type", type);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;

            }
            return ds;
        }

        private bool checkDataUpdated(Guid WorkPackageUID)
        {
            bool updated = false;
            DataSet dsdata = new DataSet();
            int count = 0;
            dsdata = GetWorkPackageDataHistory(WorkPackageUID, "budget");
            if (dsdata.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                {
                    count = 1;
                }
            }
            else
            {
                count = 1;
            }
           
            dsdata = GetWorkPackageDataHistory(WorkPackageUID, "expenditure");
            if (dsdata.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                {
                    count = 1;
                }
            }
            else
            {
                count = 1;
            }

            dsdata = GetWorkPackageDataHistory(WorkPackageUID, "enddate");
            if (dsdata.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Month != DateTime.Now.Month && Convert.ToDateTime(dsdata.Tables[0].Rows[0]["CreatedDate"]).Year != DateTime.Now.Year)
                {
                    count = 1;
                }
            }
            else
            {
                count = 1;
            }
            if (count == 1)
            {
                updated = false;
            }
           else
            {
                updated = true;
            }
            return updated;
        }

        public string getWorkPackageNameby_WorkPackageUID(Guid WorkPackageUID)
        {
            Program obj = new Program();
            string sUser = "";
            try
            {
                SqlConnection con = new SqlConnection(obj.GetConnectionString());
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

        public string getProjectNameby_ProjectUID(Guid ProjectUID)
        {
            Program obj = new Program();
            string sUser = "";
            try
            {
                SqlConnection con = new SqlConnection(obj.GetConnectionString());
                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_getProjectNameby_ProjectUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                sUser = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                sUser = "Error : " + ex.Message;
            }
            return sUser;
        }

    }
}
