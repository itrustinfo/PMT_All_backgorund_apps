using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;

namespace WordReadPrg
{
    class Program
    {
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
                return sInputLine;

            }
            catch (Exception ex)
            {
                string FilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
                System.IO.StreamWriter testfile = null;
                testfile = new System.IO.StreamWriter(FilePath, true);
                testfile.WriteLine("Error : " + ex.Message);
                testfile.Close();
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        public string ReadFilePath()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\FilePath.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                return sInputLine;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        public string ReadFilePath_help()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\FilePath_Help.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                return sInputLine;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }
        static void Main(string[] args)
        {
            Program pg = new Program();
            System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
            MyConnection.ConnectionString = pg.GetConnectionString();
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
            System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.DataSet MyDataset = new System.Data.DataSet();
            System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
            MyCommand.Connection = MyConnection;
            object documentFormat = 8;
            string randomName = DateTime.Now.Ticks.ToString();
            //string path = @"D:\Projects\NJS_Prj_New\PMT\ProjectManagementTool\ProjectManagementTool\_content_pages\Temp\";
            //object htmlFilePath = @"D:\Projects\NJS_Prj_New\PMT\ProjectManagementTool\ProjectManagementTool\_content_pages\Temp\" + randomName + ".htm";// Server.MapPath("~/_content_pages/Temp/") + randomName + ".htm";
            //string directoryPath = @"D:\Projects\NJS_Prj_New\PMT\ProjectManagementTool\ProjectManagementTool\_content_pages\Temp\" + randomName + "_files";// Server.MapPath("~/_content_pages/Temp/") + randomName + "_files";
            //object fileSavePath = @"D:\Projects\NJS_Prj_New\PMT\ProjectManagementTool\ProjectManagementTool\RegExcel\Audit - 06 April 2020.docx";  //  Server.MapPath("~") + Request.QueryString["file_path"];// Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName);

            System.IO.StreamWriter testfile = null;
            string path = "";
            object htmlFilePath = "";// @"C:\PMT\ProjectManagementTool\ProjectManagementTool\_content_pages\Temp\" + randomName + ".htm";// Server.MapPath("~/_content_pages/Temp/") + randomName + ".htm";
            string directoryPath = "";// @"C:\PMT\ProjectManagementTool\ProjectManagementTool\_content_pages\Temp\" + randomName + "_files";// Server.MapPath("~/_content_pages/Temp/") + randomName + "_files";
            object fileSavePath = "";// @"C:\PMT\ProjectManagementTool\ProjectManagementTool\RegExcel\Audit - 06 April 2020.docx";  //  Server.MapPath("~") + Request.QueryString["file_path"];// Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
            string relativepath = "~/_modal_pages/Temp/" + randomName + ".htm";
            object decryptpath = string.Empty;
           
          //  while (true)
           // {
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("SELECT * FROM WordDocRead Where [Status] = 'Pending'", MyConnection);

                if (MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    MyConnection.Open();
                }
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);

                if (MyDataset.Tables[0].Rows.Count > 0)
                {
                    testfile = new System.IO.StreamWriter(FilePath, true);
                    testfile.WriteLine(System.DateTime.Now + "       " + "Read program started...");
                    foreach (System.Data.DataRow MyRow in MyDataset.Tables[0].Rows)
                    {
                        try
                        {
                            randomName = DateTime.Now.Ticks.ToString();
                            path = pg.ReadFilePath();// @"D:\Projects\NJS_Prj_New\PMT\ProjectManagementTool\ProjectManagementTool\_content_pages\Temp\";
                            htmlFilePath = pg.ReadFilePath() + randomName + ".htm";// Server.MapPath("~/_content_pages/Temp/") + randomName + ".htm";
                            directoryPath = pg.ReadFilePath() + randomName + "_files";// Server.MapPath("~/_content_pages/Temp/") + randomName + "_files";

                            fileSavePath = MyRow["Doc_path"].ToString();// @"C:\PMT\ProjectManagementTool\ProjectManagementTool\RegExcel\Audit - 06 April 2020.docx";  //  Server.MapPath("~") + Request.QueryString["file_path"];// Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
                            relativepath = "~/_modal_pages/Temp/" + randomName + ".htm";
                            decryptpath = AppDomain.CurrentDomain.BaseDirectory + "\\Decryptpath\\" + randomName + ".docx";
                            if(MyRow["Doc_path"].ToString().Contains(".docx"))
                            {
                            decryptpath = AppDomain.CurrentDomain.BaseDirectory + "\\Decryptpath\\" + randomName + ".docx";
                            }
                        else
                        {
                            decryptpath = AppDomain.CurrentDomain.BaseDirectory + "\\Decryptpath\\" + randomName + ".doc";
                        }

                            if (MyRow["Encrypted"].ToString() == "N") // for help file
                            {
                                path = pg.ReadFilePath_help();
                                htmlFilePath = pg.ReadFilePath_help() + randomName + ".htm";
                                directoryPath = pg.ReadFilePath_help() + randomName + "_files";
                            }
                                //If Directory not present, create it.
                                if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            //Open the word document in background.
                            _Application applicationclass = new Application();
                            if (MyRow["Encrypted"].ToString() == "Y")
                            {
                                pg.DecryptFile(fileSavePath.ToString(), decryptpath.ToString());
                                applicationclass.Documents.Open(ref decryptpath);
                            }
                            else
                            {
                                applicationclass.Documents.Open(ref fileSavePath);
                                relativepath = "~/_content_pages/help/Temp/" + randomName + ".htm";
                            }

                            applicationclass.Visible = false;
                            Document document = applicationclass.ActiveDocument;

                            //Save the word document as HTML file.
                            document.SaveAs(ref htmlFilePath, ref documentFormat);

                            //Close the word document.
                            document.Close();

                            //Read the saved Html File.
                            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString(), System.Text.Encoding.Default);

                            //Loop and replace the Image Path.
                            //foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
                            //{
                            //    wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, "Temp/" + match.Groups[1].Value);
                            //}

                            wordHTML = wordHTML.Replace("<v:imagedata", "<img");
                            wordHTML = wordHTML.Replace(randomName + "_files", "Temp/" + randomName + "_files");
                            wordHTML = wordHTML.Replace("/Temp/Temp/", "/Temp/");
                            //Delete the Uploaded Word File.
                            System.IO.File.Delete(htmlFilePath.ToString());
                            //System.IO.File.
                            //Write the file
                            using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(htmlFilePath.ToString()))
                            {
                                outfile.Write(wordHTML);
                            }
                            //dvWord.InnerHtml = wordHTML;
                            MyCommand.Parameters.Clear();
                            Console.WriteLine("Read Word Success !" + fileSavePath);
                            MyCommand.Parameters.AddWithValue("@HTML_Text", relativepath);
                            MyCommand.CommandText = "Update WordDocRead set Status ='Completed',HTML_Text=@HTML_Text Where UID= '" + MyRow["UID"].ToString() + "'";


                            if (MyConnection.State == System.Data.ConnectionState.Closed)
                            {
                                MyConnection.Open();
                            }
                            MyCommand.ExecuteNonQuery();
                            //
                             testfile.WriteLine(DateTime.Now + "Read Word Success ! :- " + fileSavePath);
                        }
                        catch (Exception ex)
                        {
                            testfile.Close();
                            testfile = new System.IO.StreamWriter(FilePath, true);
                            Console.WriteLine("error occurred ;" + ex.Message);
                            testfile.WriteLine("error occurred ;" + System.DateTime.Now + " :---" + ex.Message);

                            MyCommand.CommandText = "Update WordDocRead set Status ='Failed' Where UID= '" + MyRow["UID"].ToString() + "'";


                            if (MyConnection.State == System.Data.ConnectionState.Closed)
                            {
                                MyConnection.Open();
                            }
                            MyCommand.ExecuteNonQuery();

                        }

                        finally
                        {
                            System.Threading.Thread.Sleep(10000);
                            Console.WriteLine("Next check happens after 10 sec.");
                         

                        }
                    }
                    testfile.Close();
                   
                }
                else
                {
                    Console.WriteLine("No Word Doc to read ! Next check after 5 mins");
                    System.Threading.Thread.Sleep(10000);
                }
           // }
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

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                fsOut.Dispose();
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
