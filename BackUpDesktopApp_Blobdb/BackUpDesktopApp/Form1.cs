using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;

namespace BackUpDesktopApp
{
    public partial class Form1 : Form
    {
        bool Autostart = false;
        public Form1()
        {
            InitializeComponent();
            //First load data from db

            LoadData();
          
         
            //
        }
        Int16 Fullbackupday = 0;
        DateTime FullbackUpDate;
        DateTime CurrentFullbackupdate;
        string ErrorEmailTo = string.Empty;
        string ErrorCCTo = string.Empty;
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (rdBackUp.Checked)
            {
                StartbackUP();
            }
            else
            {
                //StartRestore();
            }
        }

        private void StartbackUP()
        {
            string databaseName = txtdbname.Text;//
            lstBoxLog.Items.Clear();
            lstBoxLog.ForeColor = System.Drawing.Color.DarkRed;
            string sourceFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_offline.htm");
            string destFile = System.IO.Path.Combine(txtSourcePath.Text, "App_offline.htm");
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
            DateTime starttime;
            DateTime endtime;
            string backuptype;
           
            System.IO.StreamWriter testfile = null;
            try
            {
                testfile = new System.IO.StreamWriter(FilePath, true);
                testfile.WriteLine("======================..");
                starttime = DateTime.Now;
                string displaytext = "BackUp program started for ..." + DateTime.Now.ToString("MMMdd");
                testfile.WriteLine(System.DateTime.Now + "  " + displaytext);
                if (txtdbname.Text == "" || txtDEstination.Text == "" || txtPassword.Text == "" || txtServer.Text == "" || txtUsername.Text == "" || txtBckfileName.Text == "")
                {

                    MessageBox.Show("Please Enter all the fields !");
                    return;
                }
                bool isfullbackup = false;
                FullbackUpDate = DateTime.Now;
              

                //Define a Backup object variable.    
                DataSet dsdbs = getBlobDatabasesforProject();
                foreach (DataRow dr in dsdbs.Tables[0].Rows)
                {
                    databaseName = dr["DbName"].ToString();
                    //
                    if (!isFullbackupPresent(dr["DbName"].ToString()))
                    {
                        isfullbackup = true;
                    }
                    else
                    {
                        FullbackUpDate = DateTime.Now;
                    }


                    // get the set full backup date
                    if (Fullbackupday == (int)DateTime.Now.DayOfWeek)
                    {
                        isfullbackup = true;
                    }

                    //

                    if (!isfullbackup)
                    {
                        backuptype = "Incr";
                    }
                    else
                    {
                        backuptype = "Full";
                    }
                    //

                    Backup sqlBackup = new Backup();
                    btnSubmit.Enabled = false;
                    ////Specify the type of backup, the description, the name, and the database to be backed up.    
                    sqlBackup.Action = BackupActionType.Database;
                    sqlBackup.BackupSetDescription = "BackUp of:" + dr["DbName"].ToString() + "on" + DateTime.Now.ToShortDateString();
                    sqlBackup.BackupSetName = backuptype;// "FullBackUp";
                    sqlBackup.Database = databaseName;
                    
                    ////Declare a BackupDeviceItem    
                    string destinationPath = txtDEstination.Text;
                    string backupfileName = dr["DbName"].ToString() + "_" + backuptype + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".bak";

                    //check if incr backups are present for all dates
                    //string foldernameIncr = "";
                    //DateTime curentdate =DateTime.Now;
                    //if (!isfullbackup)
                    //{
                    //    DateTime currentday = DateTime.Now.AddDays(-6);
                    //    for (int i = 0; i <= 5; i++)
                    //    {
                    //        if(isfullBackuppresent(currentday))
                    //        {
                    //            //do nothing
                    //        }
                    //       else if(!isIncrBackuppresent(currentday))
                    //        {
                    //            curentdate = currentday.AddDays(-1); ;
                    //            foldernameIncr = currentday.AddDays(-1).ToString("MMMdd");
                    //            break;
                    //        }

                    //        currentday = currentday.AddDays(1);
                    //    }

                    //}

                    //if (foldernameIncr!="") // incr for previous dates is not there
                    //{
                    //    foldernameIncr = foldernameIncr + "-" + DateTime.Now.ToString("dd");
                    //}
                    //else
                    //{
                    //    curentdate = FullbackUpDate.AddDays(-1);
                    //    foldernameIncr = DateTime.Now.ToString("MMMdd");
                    //}

                    // added on 03/03/2021
                    // string Odd_evenPath = foldernameIncr + "_" + backuptype;
                    //
                    // string destPath = destinationPath + "//" + Odd_evenPath;
                    // string deletedestpath = string.Empty;
                    //   if (Odd_evenPath % 2 == 0)
                    //   {
                    //       destPath = destinationPath + "//even//" + Odd_evenPath;
                    //       deletedestpath = destinationPath + "//even";
                    //       destinationPath = destinationPath + "//even";
                    //   }
                    //   else
                    //   {
                    //       destPath = destinationPath + "//odd//" + Odd_evenPath;
                    //       deletedestpath = destinationPath + "//odd";
                    //       destinationPath = destinationPath + "//odd";
                    //   }
                    //if (!Directory.Exists(destPath))
                    //{
                    //    if (Directory.Exists(destPath))
                    //    {
                    //        Directory.Delete(destPath, true);
                    //    }

                    //    Directory.CreateDirectory(destPath);
                    //    //

                    //}
                    //else
                    //{
                    //    Directory.Delete(destPath, true);
                    //    Directory.CreateDirectory(destPath);
                    //}
                    //else
                    //{
                    //    if (Directory.Exists(deletedestpath))
                    //    {
                    //        Directory.Delete(deletedestpath, true);
                    //    }
                    //    Directory.Delete(destPath, true);
                    //}
                    string destPathDB = txtDBDEstination.Text + "//" + DateTime.Now.ToString("MMMdd");
                    if (!Directory.Exists(destPathDB + "\\database"))
                    {
                        Directory.CreateDirectory(destPathDB + "\\database");
                    }
                    else
                    {
                        // Directory.Delete(destPathDB, true);
                        //  Directory.CreateDirectory(destPathDB + "\\database");
                        if (System.IO.File.Exists(destPathDB + "\\database\\" + backupfileName))
                        {
                            System.IO.File.Delete(destPathDB + "\\database\\" + backupfileName);
                        }
                    }

                    //copy app_offline file in root folder of the site


                    System.IO.File.Copy(sourceFile, destFile, true);

                    //
                    BackupDeviceItem deviceItem = new BackupDeviceItem(destPathDB + "\\database\\" + backupfileName, DeviceType.File);
                    ////Define Server connection    
                    lstBoxLog.Items.Add("Backup Started Sucessfully  at " + DateTime.Now);
                    lstBoxLog.Refresh();
                    //ServerConnection connection = new ServerConnection(frm.serverName, frm.userName, frm.password);    
                    ServerConnection connection = new ServerConnection(txtServer.Text, txtUsername.Text, txtPassword.Text);
                    ////To Avoid TimeOut Exception    
                    Server sqlServer = new Server(connection);
                    sqlServer.ConnectionContext.StatementTimeout = 60 * 60;
                    Database db = sqlServer.Databases[databaseName];

                    sqlBackup.Initialize = true;
                    sqlBackup.Checksum = true;
                    sqlBackup.ContinueAfterError = true;

                    ////Add the device to the Backup object.    
                    sqlBackup.Devices.Add(deviceItem);
                   
                   

                    sqlBackup.ExpirationDate = DateTime.Now.AddDays(365);
                    ////Specify that the log must be truncated after the backup is complete.    
                    sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

                    sqlBackup.FormatMedia = false;
                    //added for incremental
                    ////Set the Incremental property to False to specify that this is a full database backup.    
                    if (backuptype != "Full")
                    {
                        sqlBackup.Incremental = true;
                    }
                    else
                    {
                        sqlBackup.Incremental = false;
                    }
                    ////Run SqlBackup to perform the full database backup on the instance of SQL Server.    
                    sqlBackup.SqlBackup(sqlServer);
                    ////Remove the backup device from the Backup object.    
                    sqlBackup.Devices.Remove(deviceItem);
                    lstBoxLog.Items.Add("Backup Completed Sucessfully at " + DateTime.Now);
                    lstBoxLog.Items.Add(" File stored at " + destPathDB + "//database//" + backupfileName);
                    lstBoxLog.Refresh();
                    //
                    endtime = DateTime.Now;
                    StoreBackupLogs_dbblob(starttime, endtime, backuptype, databaseName);
                }
              
                //
               
                testfile.WriteLine(System.DateTime.Now + "       " + "BackUp program ended...");
                testfile.Close();
                // write to db
              

                System.Threading.Thread.Sleep(5000);
               System.Windows.Forms.Application.Exit();
            }
            catch (Exception ex)
            {
                //   MessageBox.Show("Error occurred :" + ex.Message);
                testfile.WriteLine(System.DateTime.Now + "       " + " :- Error occurred :" + ex.Message);
                testfile.Close();
                lstBoxLog.Items.Add("Error occurred :" + ex.Message);
                lstBoxLog.Refresh();
                btnSubmit.Enabled = true;
                // store email to send
                DataTable dtemailCred = GetEmailCredentials();
                Guid MailUID = Guid.NewGuid();
                if (ErrorEmailTo != "")
                {
                    StoreEmaildataToMailQueue(MailUID, MailUID, dtemailCred.Rows[0][0].ToString(), ErrorEmailTo, "Database ONTB Blob backups !", " :-Error occurred: " + ex.Message, ErrorCCTo, "");
                }
                //

                if (System.IO.File.Exists(destFile))
                {
                    System.IO.File.Delete(destFile);
                }
                System.Threading.Thread.Sleep(5000);
                System.Windows.Forms.Application.Exit();
            }
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

        public DataTable LoadData()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("Select * From BackupApplicationConfig", con);
                cmd.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtdbname.Text = dt.Rows[0]["DatabaseName"].ToString();
                    txtDEstination.Text = dt.Rows[0]["DestinationPath"].ToString();
                    txtUsername.Text = dt.Rows[0]["Username"].ToString();
                    txtPassword.Text = dt.Rows[0]["Password"].ToString();
                    txtServer.Text = dt.Rows[0]["ServerName"].ToString();
                    txtSourcePath.Text = dt.Rows[0]["SourceAppPath"].ToString();
                    txtBckfileName.Text = dt.Rows[0]["DB_backupFilename"].ToString();
                    Fullbackupday =Int16.Parse(dt.Rows[0]["FullBackupDay"].ToString());
                    txtDBDEstination.Text = dt.Rows[0]["DestinationDBPath"].ToString();
                    if(dt.Rows[0]["ErrorEmailTo"] != DBNull.Value)
                    {
                        ErrorEmailTo = dt.Rows[0]["ErrorEmailTo"].ToString();
                    }
                    if (dt.Rows[0]["ErrorEmailTo"] != DBNull.Value)
                    {
                        ErrorCCTo = dt.Rows[0]["ErrorCCTo"].ToString();
                    }
                    if (dt.Rows[0]["AutoStart"] != DBNull.Value)
                    {
                        if (dt.Rows[0]["AutoStart"].ToString() == "Y")
                        {
                            Autostart = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable LoadRestoreData()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("Select * From RestoreApplicationConfig", con);
                cmd.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtdbname.Text = dt.Rows[0]["DB_RestorePath"].ToString();
                    txtDEstination.Text = dt.Rows[0]["DestinationPath"].ToString();
                    txtUsername.Text = dt.Rows[0]["Username"].ToString();
                    txtPassword.Text = dt.Rows[0]["Password"].ToString();
                    txtServer.Text = dt.Rows[0]["ServerName"].ToString();
                    txtSourcePath.Text = dt.Rows[0]["SourceAppPath"].ToString();
                    txtBckfileName.Text = dt.Rows[0]["DB_backupFilename"].ToString();
                   
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        private void Form1_Shown_1(object sender, EventArgs e)
        {
            if (Autostart)
            {
                System.Threading.Thread.Sleep(3000);
                btnSubmit.PerformClick();
            }
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlCommand MyCommand = new SqlCommand();
                MyCommand.Connection = con;
                if (rdBackUp.Checked)
                {
                    MyCommand.CommandText = "DELETE FROM BackupApplicationConfig";
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    MyCommand.ExecuteNonQuery();


                    MyCommand.CommandText = "INSERT INTO [dbo].[BackupApplicationConfig]([ServerName],[Username],[Password],[DatabaseName],[DB_backupFilename],[SourceAppPath],[DestinationPath],[DestinationDBPath]) " +
" VALUES (@ServerName,@Username,@Password,@DatabaseName,@DB_backupFilename,@SourceAppPath,@DestinationPath,@DestinationDBPath)";
                    MyCommand.Parameters.AddWithValue("@ServerName", txtServer.Text);
                    MyCommand.Parameters.AddWithValue("@Username", txtUsername.Text);
                    MyCommand.Parameters.AddWithValue("@Password", txtPassword.Text);
                    MyCommand.Parameters.AddWithValue("@DatabaseName", txtdbname.Text);
                    MyCommand.Parameters.AddWithValue("@DB_backupFilename", txtBckfileName.Text);
                    MyCommand.Parameters.AddWithValue("@SourceAppPath", txtSourcePath.Text);
                    MyCommand.Parameters.AddWithValue("@DestinationPath", txtDEstination.Text);
                    MyCommand.Parameters.AddWithValue("@DestinationDBPath", txtDBDEstination.Text);

                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    MyCommand.CommandText = "DELETE FROM RestoreApplicationConfig";
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    MyCommand.ExecuteNonQuery();


                    MyCommand.CommandText = "INSERT INTO [dbo].[RestoreApplicationConfig]([ServerName],[Username],[Password],[DB_RestorePath],[DB_backupFilename],[SourceAppPath],[DestinationPath]) " +
    " VALUES (@ServerName,@Username,@Password,@DatabaseName,@DB_backupFilename,@SourceAppPath,@DestinationPath)";
                    MyCommand.Parameters.AddWithValue("@ServerName", txtServer.Text);
                    MyCommand.Parameters.AddWithValue("@Username", txtUsername.Text);
                    MyCommand.Parameters.AddWithValue("@Password", txtPassword.Text);
                    MyCommand.Parameters.AddWithValue("@DatabaseName", txtdbname.Text);
                    MyCommand.Parameters.AddWithValue("@DB_backupFilename", txtBckfileName.Text);
                    MyCommand.Parameters.AddWithValue("@SourceAppPath", txtSourcePath.Text);
                    MyCommand.Parameters.AddWithValue("@DestinationPath", txtDEstination.Text);

                    MyCommand.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show("Settings Updated SuccesFully !");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error occurred :" + ex.Message);
            }
        }

        private void rdRestore_CheckedChanged(object sender, EventArgs e)
        {
            lblDBRestorePath.Text = "Enter DB Restore Path";
            lblDBbckupName.Text = "Enter DB File Name";
            btnSubmit.Text = "Start Restore";
            lstBoxLog.Items.Clear();
            lstBoxLog.Refresh();
            LoadRestoreData();
            
        }

        private void rdBackUp_CheckedChanged(object sender, EventArgs e)
        {
            lblDBRestorePath.Text = "Enter Database Name";
            lblDBbckupName.Text = "DB Backup File Name";
            btnSubmit.Text = "Start BackUp";
            LoadData();
            lstBoxLog.Items.Clear();
            lstBoxLog.Refresh();

        }

        private void StartRestore()
        {
           
            lstBoxLog.Items.Clear();
            lstBoxLog.ForeColor = System.Drawing.Color.DarkRed;
            try
            {

                if (txtdbname.Text == "" || txtDEstination.Text == "" || txtPassword.Text == "" || txtServer.Text == "" || txtUsername.Text == "" || txtBckfileName.Text == "")
                {

                    MessageBox.Show("Please Enter all the fields !");
                    return;
                }
                // Restore the db back
                /*  ServerConnection connection = new ServerConnection(txtServer.Text, txtUsername.Text, txtPassword.Text);
                  Server sqlServer = new Server(connection);
                  Restore rstDatabase = new Restore();
                  rstDatabase.Action = RestoreActionType.Database;
                  rstDatabase.Database = txtBckfileName.Text;
                  BackupDeviceItem bkpDevice = new BackupDeviceItem(txtdbname.Text, DeviceType.File);
                  rstDatabase.Devices.Add(bkpDevice);
                  rstDatabase.ReplaceDatabase = true;
                  rstDatabase.SqlRestore(sqlServer);*/
                btnSubmit.Enabled = false;
                lstBoxLog.Items.Add("Restore of DB Started Sucessfully  at " + DateTime.Now);
                lstBoxLog.Refresh();
                Restore sqlRestore = new Restore();
                BackupDeviceItem deviceItem = new BackupDeviceItem(txtdbname.Text, DeviceType.File);
                sqlRestore.Devices.Add(deviceItem);
                sqlRestore.Database = txtBckfileName.Text;
                ServerConnection connection = new ServerConnection(txtServer.Text, txtUsername.Text, txtPassword.Text);
                Server sqlServer = new Server(connection);
                sqlRestore.Action = RestoreActionType.Database;

                string logFile = System.IO.Path.GetDirectoryName(txtdbname.Text);
                logFile = System.IO.Path.Combine(logFile, deviceItem.Name + "_Log.ldf");

                string dataFile = System.IO.Path.GetDirectoryName(txtdbname.Text);
                dataFile = System.IO.Path.Combine(dataFile, deviceItem.Name + ".mdf");

                Database db = sqlServer.Databases[txtBckfileName.Text];
                System.Data.DataTable logicalRestoreFiles = sqlRestore.ReadFileList(sqlServer);
                sqlRestore.RelocateFiles.Add(new RelocateFile(logicalRestoreFiles.Rows[0][0].ToString(), dataFile));
                sqlRestore.RelocateFiles.Add(new RelocateFile(logicalRestoreFiles.Rows[1][0].ToString(), logFile));
                sqlRestore.SqlRestore(sqlServer);
                db = sqlServer.Databases[txtBckfileName.Text];
                db.SetOnline();
                sqlServer.Refresh();
                lstBoxLog.Items.Add("Restore of DB " + txtBckfileName.Text + " Completed Sucessfully  at " + DateTime.Now);
                lstBoxLog.Refresh();

                //
                string sourcePath = txtSourcePath.Text;
                  string destPath = txtDEstination.Text;
                  lstBoxLog.Items.Add("Application Restore Started  at " + DateTime.Now);
                  lstBoxLog.Items.Add("Copy Files Please Wait ......");
                  lstBoxLog.Refresh();
                  if (!Directory.Exists(destPath))
                  {
                      Directory.CreateDirectory(destPath);
                  }
                  else
                  {
                      Directory.Delete(destPath, true);
                  }

                  //FileSystem.CopyDirectory(sourcePath, destPath, UIOption.AllDialogs);
                  lstBoxLog.Refresh();
                  FileSystem.CopyDirectory(sourcePath, destPath);
                  lstBoxLog.Items.Add("Application Restore Finished  at " + DateTime.Now);
                  lstBoxLog.Items.Add("Restore Diretory  at " + destPath);

                  lstBoxLog.Items.Add("Restore Complete !");
                  lstBoxLog.Refresh();
                  btnSubmit.Enabled = true;
                // MessageBox.Show("BackUp Completed !");
                // System.Threading.Thread.Sleep(5000);
                //  System.Windows.Forms.Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred :" + ex.Message);
                btnSubmit.Enabled = true;
            }
        }

        private void StoreBackupLogs(DateTime start,DateTime end,string type,string folder)
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            try
            {
                DateTime currentdate = FullbackUpDate;
               
                SqlCommand sqlcom = new SqlCommand();
                sqlcom.CommandText = "INSERT INTO [dbo].[BackupLogs]([backupdate],[Starttime],[endtime],[type],[folder]) VALUES" +
               "('" + currentdate + "','" + start + "','" + end  + "','" + type + "','" + folder + "')";
                sqlcom.Connection = con;
                con.Open();
                sqlcom.ExecuteNonQuery();
                con.Close();
            }
            catch(Exception ex)
            {
                if(con.State ==ConnectionState.Open)
                {
                    con.Close();
                }
            }
           
        }

        private void StoreBackupLogs_dbblob(DateTime start, DateTime end, string type, string dbname)
        {
            SqlConnection con = new SqlConnection(GetConnectionString());
            try
            {
                DateTime currentdate = FullbackUpDate;

                SqlCommand sqlcom = new SqlCommand();
                sqlcom.CommandText = "INSERT INTO [dbo].[BackupLogs_dbblob]([backupdate],[Starttime],[endtime],[type],[dbname]) VALUES" +
               "('" + currentdate + "','" + start + "','" + end + "','" + type + "','" + dbname + "')";
                sqlcom.Connection = con;
                con.Open();
                sqlcom.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }

        private bool isFullbackupPresent(string dbname)
        {
            bool sresult = false;
            DateTime currentday = DateTime.Now.AddDays(-6);
            for(int i=0; i<=5;i++)
            {
                if((int)currentday.DayOfWeek == Fullbackupday)
                {
                    
                    return isfullBackuppresent(currentday, dbname);
                }
                else
                {
                    currentday = currentday.AddDays(1);
                }
            }
            return sresult;
        }

       

        private bool isfullBackuppresent(DateTime sdate,string dbname)
        {
            bool sresult = false;
            SqlConnection con = new SqlConnection(GetConnectionString());
            try
            {
                SqlCommand cmd = new SqlCommand("select count(*) FRom [dbo].[BackupLogs_dbblob] Where type='Full' and dbname ='" + dbname + "' and backupdate >='" + sdate.ToString("yyyy-MM-dd") + "'", con);
                con.Open();
                if((int)cmd.ExecuteScalar() != 0)
                {
                    sresult = true;
                }
                con.Close();
            }
            catch(Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                
            }
            return sresult;
        }

        private bool isIncrBackuppresent(DateTime sdate)
        {
            bool sresult = false;
            SqlConnection con = new SqlConnection(GetConnectionString());
            try
            {
                SqlCommand cmd = new SqlCommand("select count(*) FRom [dbo].[BackupLogs] Where backupdate >='" + sdate.ToString("yyyy-MM-dd") + "'", con);
                con.Open();
                if ((int)cmd.ExecuteScalar() != 0)
                {
                    sresult = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            return sresult;
        }

        private bool isfullCurrentBackuppresent(DateTime sdate)
        {
            bool sresult = false;
            SqlConnection con = new SqlConnection(GetConnectionString());
            try
            {
                SqlCommand cmd = new SqlCommand("select count(*) FRom [dbo].[BackupLogs] Where type='Full' and backupdate ='" + sdate.ToString("yyyy-MM-dd") + "'", con);
                con.Open();
                if ((int)cmd.ExecuteScalar() != 0)
                {
                    sresult = true;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            return sresult;
        }

        public void StoreEmaildataToMailQueue(Guid MailUID, Guid UserUID, string FromEmailID, string ToEmailID, string Subject, string Body, string CCTo, string Attachment)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(GetConnectionString()))
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

        public DataSet getBlobDatabasesforProject()
        {
            DataSet dt = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("Select * From MasterDbforBlob", con);
                cmd.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            return dt;
        }
    }
}
