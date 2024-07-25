using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Autosendmail
{
    class AutoSendMails
    {
        static void Main(string[] args)
        {
            AutoSendMails obj = new AutoSendMails();
            obj.SendMail_New();
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
        public void SendMail_New()
        {
           
            try
            {
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;
                int count = 1;
                String Path = AppDomain.CurrentDomain.BaseDirectory;
                DataTable dtemailCred = GetEmailCredentials();
                string username = dtemailCred.Rows[0][0].ToString();
                string password = dtemailCred.Rows[0][1].ToString();
                while (true)
                {
                    MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("SELECT * FROM MailQueue Where MailSent = 'N' OR MailSent = '1'", MyConnection);
                   
                    if (MyConnection.State == System.Data.ConnectionState.Closed)
                    {
                        MyConnection.Open();
                    }
                    MyDataset.Clear();
                    MyAdapter.Fill(MyDataset);

                    if (MyDataset.Tables[0].Rows.Count > 0)
                    {
                        count = 1;
                        Console.WriteLine("Sending Mails. Please wait...");
                        foreach (System.Data.DataRow MyRow in MyDataset.Tables[0].Rows)
                        {
                            System.Threading.Thread.Sleep(3000);
                            Console.WriteLine("Sending mail " + count.ToString() + " of " + MyDataset.Tables[0].Rows.Count.ToString());
                            bool MailReSend = false;
                            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                            if (!string.IsNullOrWhiteSpace(MyRow["ToEmailID"].ToString()))
                            {
                                message.From = new System.Net.Mail.MailAddress(MyRow["FromEmailID"].ToString());
                               // message.To.Add(new System.Net.Mail.MailAddress(MyRow["ToEmailID"].ToString()));
                                if (MyRow["ToEmailID"] != DBNull.Value)
                                {
                                    string[] CCId = MyRow["ToEmailID"].ToString().Split(',');
                                    if (CCId.Length > 0)
                                    {
                                        foreach (string CCEmail in CCId)
                                        {
                                            if (!string.IsNullOrEmpty(CCEmail))
                                            {
                                                message.To.Add(new System.Net.Mail.MailAddress(CCEmail));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        message.To.Add(new System.Net.Mail.MailAddress(MyRow["ToEmailID"].ToString()));
                                    }
                                }


                                if (MyRow["CCTo"] != DBNull.Value)
                                {
                                    string[] CCId = MyRow["CCTo"].ToString().Split(',');
                                    if (CCId.Length > 0)
                                    {
                                        foreach (string CCEmail in CCId)
                                        {
                                            if (!string.IsNullOrEmpty(CCEmail))
                                            {
                                                message.CC.Add(new System.Net.Mail.MailAddress(CCEmail));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        message.CC.Add(new System.Net.Mail.MailAddress(MyRow["CCTo"].ToString()));
                                    }
                                }

                                //for attachments
                                if (MyRow["Attachment"] != DBNull.Value)
                                {
                                    string[] CCId = MyRow["Attachment"].ToString().Split(',');
                                    if (CCId.Length > 0)
                                    {
                                        foreach (string CCEmail in CCId)
                                        {
                                            if (!string.IsNullOrEmpty(CCEmail))
                                            {
                                                //message.CC.Add(new System.Net.Mail.MailAddress(CCEmail));
                                                string getExtension = System.IO.Path.GetExtension(CCEmail);
                                                string getfilename = System.IO.Path.GetFileNameWithoutExtension(CCEmail);
                                                string outPath = Path + getfilename +  "_download" + getExtension;
                                                 DecryptFile(CCEmail, outPath);
                                                System.IO.FileInfo file = new System.IO.FileInfo(outPath);
                                                message.Attachments.Add(new System.Net.Mail.Attachment(outPath));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // message.CC.Add(new System.Net.Mail.MailAddress(MyRow["CCTo"].ToString()));
                                        string getExtension = System.IO.Path.GetExtension(MyRow["Attachment"].ToString());
                                        string getfilename = System.IO.Path.GetFileNameWithoutExtension(MyRow["Attachment"].ToString());
                                        string outPath = Path + getfilename + "_download" + getExtension;
                                        DecryptFile(MyRow["Attachment"].ToString(), outPath);
                                        System.IO.FileInfo file = new System.IO.FileInfo(outPath);
                                        message.Attachments.Add(new System.Net.Mail.Attachment(outPath));
                                        //message.Attachments.Add(new System.Net.Mail.Attachment(MyRow["Attachment"].ToString()));
                                    }

                                    //
                                    
                                }
                               

                                String Body = MyRow["Body"].ToString();
                                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Body.Trim('?').Replace("\\r\\n", "<br/>"), null, MediaTypeNames.Text.Html);



                                //if (Body.Contains("logo_hc.jpg"))
                                //{
                                //    LinkedResource lr2 = new LinkedResource((new StreamReader(Path + "\\Images\\Edify School.jpg")).BaseStream, MediaTypeNames.Image.Jpeg);
                                //    lr2.ContentId = "logo_hc.jpg";
                                //    av1.LinkedResources.Add(lr2);
                                //}


                                message.IsBodyHtml = true;
                                message.AlternateViews.Add(av1);

                                message.Subject = MyRow["Subject"].ToString();
                                message.IsBodyHtml = true;

                                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                                System.Net.NetworkCredential myCredential;
                                //myCredential = new System.Net.NetworkCredential("itrustramya@gmail.com", "itrustramya123");
                                //client.Host = "smtp.gmail.com";
                                //client.Port = 587;
                                client.UseDefaultCredentials = false;
                                myCredential = new System.Net.NetworkCredential(username, password);
                                client.Port = 587;
                                client.Host = "smtp.gmail.com";
                                //client.Host = "smtp.office365.com";
                                client.Credentials = myCredential;
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                client.EnableSsl = true;

                                try
                                {
                                    client.Send(message);

                                    MyCommand.CommandText = "DELETE FROM MailQueue Where MailUID= '" + MyRow["MailUID"].ToString() + "'";
                                    if (MyConnection.State == System.Data.ConnectionState.Closed)
                                    {
                                        MyConnection.Open();
                                    }
                                    MyCommand.ExecuteNonQuery();

                                    MyCommand.Parameters.Clear();
                                    MyCommand.CommandText = "INSERT INTO[dbo].[MailQueue_Sent] ([MailUID],[UserUID],[ToEmailID],[FromEmailID],[Subject],[Body],[CCTo],[Attachment],[MailSentDate])" +
         " VALUES (@MailUID,@UserUID,@ToEmailID,@FromEmailID,@Subject,@Body,@CCTo,@Attachment,@MailSentDate)";
                                    MyCommand.Parameters.AddWithValue("@MailUID", MyRow["MailUID"].ToString());
                                    MyCommand.Parameters.AddWithValue("@UserUID", MyRow["UserUID"].ToString());
                                    MyCommand.Parameters.AddWithValue("@ToEmailID", MyRow["ToEmailID"].ToString());
                                    MyCommand.Parameters.AddWithValue("@FromEmailID", MyRow["FromEmailID"].ToString());
                                    MyCommand.Parameters.AddWithValue("@Subject", MyRow["Subject"].ToString());
                                    MyCommand.Parameters.AddWithValue("@Body", MyRow["Body"].ToString());
                                    MyCommand.Parameters.AddWithValue("@CCTo", MyRow["CCTo"].ToString());
                                    MyCommand.Parameters.AddWithValue("@Attachment", MyRow["Attachment"].ToString());
                                    MyCommand.Parameters.AddWithValue("@MailSentDate", DateTime.Now);
                                    MyCommand.ExecuteNonQuery();
                                    count += 1;

                                   // System.Threading.Thread.Sleep(30000);
                                }
                                catch (System.Net.Mail.SmtpFailedRecipientsException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    MailReSend = true;
                                }
                                catch (System.Net.Mail.SmtpFailedRecipientException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    MailReSend = true;
                                }
                                catch (System.Net.Mail.SmtpException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    MailReSend = true;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    MailReSend = true;
                                }


                                if (MailReSend)
                                {
                                    if (MyRow["MailSent"].ToString() == "N")
                                    {
                                        MyCommand.CommandText = "UPDATE MailQueue SET MailSent = '1' WHERE MailUID = '" + MyRow["MailUID"].ToString() + "'";
                                        if (MyConnection.State == System.Data.ConnectionState.Closed)
                                        {
                                            MyConnection.Open();
                                        }
                                        MyCommand.ExecuteNonQuery();

                                        count += 1;
                                    }
                                    else if (MyRow["MailSent"].ToString() == "1")
                                    {
                                        MyCommand.CommandText = "UPDATE MailQueue SET MailSent = 'F' WHERE MailUID = '" + MyRow["MailUID"].ToString() + "'";
                                        if (MyConnection.State == System.Data.ConnectionState.Closed)
                                        {
                                            MyConnection.Open();
                                        }
                                        MyCommand.ExecuteNonQuery();


                                        count += 1;
                                    }
                                }
                            }
                        }
                        Console.WriteLine("Next check happens after 15 sec.");
                        System.Threading.Thread.Sleep(15000);
                    }
                    else
                    {
                        Console.WriteLine("No mails to send. Next check happens after 15 sec.");
                        System.Threading.Thread.Sleep(15000);
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

        public void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                //RMCrypto.Padding = PaddingMode.None;
                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                {
                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);
                }



                //fsOut.Close();
                //fsOut.Dispose();
                cs.Close();
                fsCrypt.Close();
                fsCrypt.Dispose();

            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
            }
        }
    }
}
