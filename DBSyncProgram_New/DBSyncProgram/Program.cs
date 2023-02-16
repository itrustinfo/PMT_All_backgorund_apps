using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBSyncProgram.DAL;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DBSyncProgram
{
    class Program
    {
        DBOperations dbutility = new DBOperations();
        // for ONTB syncing to LNT
        string SourceDocPath = @"E:\iPIMS-DM-ONTB\site";
        string SourceDocPathReviewFile = @"E:\iPIMS-DM-ONTB\site\\_modal_pages\\";
        string wkPkgUID = "B4B61A63-558F-4A79-B956-1E1680ECA603";
        string PrjUID = "2F66EFC0-FF27-4CF6-A041-B9B2E05B9217";
        string serviceURL = "https://cp0203-api.itrustinfo.com/api/DbSync/";//"http://localhost:50385/api/DbSync/";
        string DestWordocReadPath = @"D:\sitedest";
        string SourceWordocReadPath = @"C:\site";

        // for LNT syncing to ONTB
        //string SourceDocPath = @"D:\iPIMS-CP09-13\site";
        //string SourceDocPathReviewFile = @"D:\iPIMS-CP09-13\site\\_modal_pages\\";
        //string wkPkgUID = "B4B61A63-558F-4A79-B956-1E1680ECA603";
        //string PrjUID = "2F66EFC0-FF27-4CF6-A041-B9B2E05B9217";
        //string serviceURL = "https://api.ontbmis.com/api/DbSync/";//"http://localhost:50385/api/DbSync/";
        //string DestWordocReadPath = @"D:\sitedest";
        //string SourceWordocReadPath = @"C:\site";


        //local
        //string SourceDocPath = @"D:\Projects\NJS_Prj_New\2021-09-16_new\ProjectMonitoringTool-Integration-main\ProjectManagementTool";
        //string SourceDocPathReviewFile = @"D:\Projects\NJS_Prj_New\2021-09-16_new\ProjectMonitoringTool-Integration-main\ProjectManagementTool\\_modal_pages\\";
        //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
        //string PrjUID = "2BBFA1EF-B427-4E19-ADD1-97DF91390F97";
        //string serviceURL = "http://localhost:50385/api/DbSync/";
        //string DestWordocReadPath = @"D:\sitedest\";
        //string SourceWordocReadPath = @"D:\Arun\";
        static void Main(string[] args)
        {
            Program dbs = new Program();
            dbs.FillSettings();
            dbs.StartSynching();
        }

        private void FillSettings()
        {
            DataSet dsSettings = new DataSet();
            dsSettings = dbutility.getDbsyncSettings();
            foreach(DataRow dr in dsSettings.Tables[0].Rows)
            {
                SourceDocPath = dr["SourceDocPath"].ToString();
                SourceDocPathReviewFile = dr["SourceDocPathReviewFile"].ToString();
                wkPkgUID = dr["WorkPackageUID"].ToString();
                PrjUID = dr["ProjectUID"].ToString();
                serviceURL = dr["serviceURL"].ToString();
                DestWordocReadPath = dr["DestWordocReadPath"].ToString();
                SourceWordocReadPath = dr["SourceWordocReadPath"].ToString();

            }
        }

        public string GetSourceConnectionString()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\ConnectionSource.txt";
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

        public string GetDestinationConnectionString()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\ConnectionDest.txt";
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

        private void StartSynching()
        {

            //string postData = "Username=superadmin&Password=admin";
            //string URL = "http://localhost:50385/api/DbSync/GetSubmittalDocumentFlows";
            //var data = webPostMethod(postData, URL);
            //Console.WriteLine(data.ToString());
            //Console.ReadLine();
            while (true)
            {
                SynchSubmittals();  // tested - escape caharcters

                SynchSubmittal_MultipleUsers();

                SynchActualDocuments(); // tested - escape caharcters

                SynchDocumentStatus(); // tested - escape caharcters

                SynchDocumentVersion(); // tested - escape caharcters

                SynchReferencNoHistory();

                SynchFlow_Master_Users();

                SynchMeasurementBook();

                SynchDocumentFlowData(); // tested - escape caharcters

                SyncDocumentsUploadLog(); // tested

                SynchWorddocRead(); // tested

                SynchRABill_Abstract(); // tested

                SynchRABills_Documents();

                SynchRABillsSync(); // tested

                SynchAssignJointInspectiontoRAbill(); //tested

                SynchInvoiceMaster(); // tested

                SynchDeductionsMaster(); // tested

                SynchInvoiceRABills(); // tested

                SynchInvoiceDeduction(); // tested

                SyncUserDetails(); //tested

                SyncUserProjects(); //tested

                SyncUserWorkPackages(); //tested

                SyncUserRolesMaster(); //tested

                SyncUserType_Functionality_Master(); //tested

                SyncUserType_Functionality_Mapping(); //tested

                SyncIssues(); //tested

                SyncIssueDocs();

                SyncIssueRemarks(); //tested

                SyncIssueRemarksDocs();

                SynchActivityDeleteLogs(); //tested

                SyncFinanceMileStones(); //tested

                SyncFinanceMileStoneMonth(); //tested

                SynchFinanceMileStoneMonth_EditedValues();  //tested

                SyncTaskSchedule(); //tested

                SyncTaskScheduleVersion(); //tested

                SyncTask(); //tested

                //added on 05/01/2023
                SyncResourceMaster();

                SyncResourceDeployment();

                SynchResourceDeploymentUpdate();
                //
                SyncDailyProgressReportMaster(); //tested

                SyncDailyProgress(); //tested

                //added on 20 / 08 / 2022
                SyncGFCReportMaster();

                SyncGFCStatus();

                SyncDesign_and_drawing_A_master();

                SyncDesign_and_drawing_works_A();

                Syncdesign_and_drawing_dwg_issue_master();

                SyncDesign_and_drawing_works_dwg_issue();

                Syncdesign_and_drawing_works_b_tt_master();

                SyncDesign_and_drawing_works_b_tt();

                SyncRABillPayments();

                Synchform_task_update();

                SynchCorrespondenceCCToUsers();

                UpdateDbsync_Status();
                Console.WriteLine("Next check happens after 10 seconds---------------");
                System.Threading.Thread.Sleep(10000);
                
            }
        }

        private void SynchSubmittals()
        {
            try
            {
                // source




               
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;
               
                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Documents where ServerCopiedAdd='N' and IsSync='Y' and WorkPackageUID='" + wkPkgUID  +"'", MyConnection);
                //
                int Estimateddocuments = 0;
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "SubmmitalSync";// "http://localhost:50385/api/DbSync/SubmmitalSync";
                var data = "";
                Console.WriteLine("Started Synching for Submittals Add : ");
                if(MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Submittals Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        if (dr["EstimatedDocuments"] != System.DBNull.Value)
                        {
                            Estimateddocuments = int.Parse(dr["EstimatedDocuments"].ToString());
                        }


                        postData = "DocumentUID=" + dr["DocumentUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&DocName=" + WebUtility.UrlEncode(dr["DocName"].ToString()) + "&Doc_Category=" + dr["Doc_Category"].ToString() + "&Doc_RefNumber=" + WebUtility.UrlEncode(dr["Doc_RefNumber"].ToString()) + "&Doc_Type=" + dr["Doc_Type"].ToString() +
                          "&Doc_Budget=" + dr["Doc_Budget"].ToString() + "&FlowUID=" + dr["FlowUID"].ToString() + "&Flow_StartDate=" + dr["Flow_StartDate"].ToString() + "&FlowStep1_UserUID=" + dr["FlowStep1_UserUID"].ToString() + "&FlowStep1_TargetDate=" + dr["FlowStep1_TargetDate"].ToString() + "&FlowStep2_UserUID=" + dr["FlowStep2_UserUID"].ToString() + "&FlowStep2_TargetDate=" + dr["FlowStep2_TargetDate"].ToString() + "&FlowStep3_UserUID=" + dr["FlowStep3_UserUID"].ToString() + "&FlowStep3_TargetDate=" + dr["FlowStep3_TargetDate"].ToString() + "&FlowStep4_UserUID=" + dr["FlowStep4_UserUID"].ToString() + "&FlowStep4_TargetDate=" + dr["FlowStep4_TargetDate"].ToString() + "&&FlowStep5_UserUID=" + dr["FlowStep5_UserUID"].ToString() + "&FlowStep5_TargetDate=" + dr["FlowStep5_TargetDate"].ToString() + "&Estimateddocuments=" + Estimateddocuments + "&Remarks=" + WebUtility.UrlEncode(dr["Remarks"].ToString()) + "&DocumentSearchType=" + dr["DocumentSearchType"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&IsSync=" + dr["IsSync"].ToString() +
                          "&FlowStep6_UserUID=" + dr["FlowStep6_UserUID"].ToString() + "&FlowStep6_TargetDate=" + dr["FlowStep6_TargetDate"].ToString() +
                          "&FlowStep7_UserUID=" + dr["FlowStep7_UserUID"].ToString() + "&FlowStep7_TargetDate=" + dr["FlowStep7_TargetDate"].ToString() +
                          "&FlowStep8_UserUID=" + dr["FlowStep8_UserUID"].ToString() + "&FlowStep8_TargetDate=" + dr["FlowStep8_TargetDate"].ToString() +
                           "&FlowStep9_UserUID=" + dr["FlowStep9_UserUID"].ToString() + "&FlowStep9_TargetDate=" + dr["FlowStep9_TargetDate"].ToString() +
                           "&FlowStep10_UserUID=" + dr["FlowStep10_UserUID"].ToString() + "&FlowStep10_TargetDate=" + dr["FlowStep10_TargetDate"].ToString() +
                           "&FlowStep11_UserUID=" + dr["FlowStep11_UserUID"].ToString() + "&FlowStep11_TargetDate=" + dr["FlowStep11_TargetDate"].ToString() +
                           "&FlowStep12_UserUID=" + dr["FlowStep12_UserUID"].ToString() + "&FlowStep12_TargetDate=" + dr["FlowStep12_TargetDate"].ToString() +
                           "&FlowStep13_UserUID=" + dr["FlowStep13_UserUID"].ToString() + "&FlowStep13_TargetDate=" + dr["FlowStep13_TargetDate"].ToString() +
                           "&FlowStep14_UserUID=" + dr["FlowStep14_UserUID"].ToString() + "&FlowStep14_TargetDate=" + dr["FlowStep14_TargetDate"].ToString() +
                           "&FlowStep15_UserUID=" + dr["FlowStep15_UserUID"].ToString() + "&FlowStep15_TargetDate=" + dr["FlowStep15_TargetDate"].ToString() +
                           "&FlowStep16_UserUID=" + dr["FlowStep16_UserUID"].ToString() + "&FlowStep16_TargetDate=" + dr["FlowStep16_TargetDate"].ToString() +
                           "&FlowStep17_UserUID=" + dr["FlowStep17_UserUID"].ToString() + "&FlowStep17_TargetDate=" + dr["FlowStep17_TargetDate"].ToString() +
                           "&FlowStep18_UserUID=" + dr["FlowStep18_UserUID"].ToString() + "&FlowStep18_TargetDate=" + dr["FlowStep18_TargetDate"].ToString() +
                           "&FlowStep19_UserUID=" + dr["FlowStep19_UserUID"].ToString() + "&FlowStep19_TargetDate=" + dr["FlowStep19_TargetDate"].ToString() +
                           "&FlowStep20_UserUID=" + dr["FlowStep20_UserUID"].ToString() + "&FlowStep20_TargetDate=" + dr["FlowStep20_TargetDate"].ToString() +
                           "&FlowStep1_IsMUser=" + dr["FlowStep1_IsMUser"].ToString() +
                           "&FlowStep2_IsMUser=" + dr["FlowStep2_IsMUser"].ToString() +
                           "&FlowStep3_IsMUser=" + dr["FlowStep3_IsMUser"].ToString() +
                           "&FlowStep4_IsMUser=" + dr["FlowStep4_IsMUser"].ToString() +
                           "&FlowStep5_IsMUser=" + dr["FlowStep5_IsMUser"].ToString() +
                           "&FlowStep6_IsMUser=" + dr["FlowStep6_IsMUser"].ToString() +
                           "&FlowStep7_IsMUser=" + dr["FlowStep7_IsMUser"].ToString() +
                           "&FlowStep8_IsMUser=" + dr["FlowStep8_IsMUser"].ToString() +
                           "&FlowStep9_IsMUser=" + dr["FlowStep9_IsMUser"].ToString() +
                           "&FlowStep10_IsMUser=" + dr["FlowStep10_IsMUser"].ToString() +
                           "&FlowStep11_IsMUser=" + dr["FlowStep11_IsMUser"].ToString() +
                           "&FlowStep12_IsMUser=" + dr["FlowStep12_IsMUser"].ToString() +
                           "&FlowStep13_IsMUser=" + dr["FlowStep13_IsMUser"].ToString() +
                           "&FlowStep14_IsMUser=" + dr["FlowStep14_IsMUser"].ToString() +
                           "&FlowStep15_IsMUser=" + dr["FlowStep15_IsMUser"].ToString() +
                           "&FlowStep16_IsMUser=" + dr["FlowStep16_IsMUser"].ToString() +
                           "&FlowStep17_IsMUser=" + dr["FlowStep17_IsMUser"].ToString() +
                           "&FlowStep18_IsMUser=" + dr["FlowStep18_IsMUser"].ToString() +
                           "&FlowStep19_IsMUser=" + dr["FlowStep19_IsMUser"].ToString() +
                           "&FlowStep20_IsMUser=" + dr["FlowStep20_IsMUser"].ToString();
                        //    dbutility.Submittal_Insert_or_Update_Flow(new Guid(dr["DocumentUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["TaskUID"].ToString()), dr["DocName"].ToString(), new Guid(dr["Doc_Category"].ToString()), dr["Doc_RefNumber"].ToString(), dr["Doc_Type"].ToString(),
                        //Convert.ToDouble(dr["Doc_Budget"].ToString()), new Guid(dr["FlowUID"].ToString()), DateTime.Parse(dr["Flow_StartDate"].ToString()), new Guid(dr["FlowStep1_UserUID"].ToString()), DateTime.Parse(dr["FlowStep1_TargetDate"].ToString()), dr["FlowStep2_UserUID"].ToString(), dr["FlowStep2_TargetDate"].ToString(), dr["FlowStep3_UserUID"].ToString(), dr["FlowStep3_TargetDate"].ToString(), dr["FlowStep4_UserUID"].ToString(), dr["FlowStep4_TargetDate"].ToString(), dr["FlowStep5_UserUID"].ToString(), dr["FlowStep5_TargetDate"].ToString(), Estimateddocuments, dr["Remarks"].ToString(), dr["DocumentSearchType"].ToString(), DateTime.Parse(dr["CreatedDate"].ToString()), dr["Delete_Flag"].ToString());
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateSubmittalAddFlag(new Guid(dr["DocumentUID"].ToString()));
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "Submittal Add", "Success", "");


                            Console.WriteLine("Synching for SubmittalUID Add : " + dr["DocumentUID"].ToString() + " Done");
                        }
                    }
                    catch(Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "Submittal Update", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Documents where ServerCopiedUpdate='N' and IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Submittals Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Submittals Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["EstimatedDocuments"] != System.DBNull.Value)
                        {
                            Estimateddocuments = int.Parse(dr["EstimatedDocuments"].ToString());
                        }

                        postData = "DocumentUID=" + dr["DocumentUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&DocName=" + WebUtility.UrlEncode(dr["DocName"].ToString()) + "&Doc_Category=" + dr["Doc_Category"].ToString() + "&Doc_RefNumber=" + WebUtility.UrlEncode(dr["Doc_RefNumber"].ToString()) + "&Doc_Type=" + dr["Doc_Type"].ToString() +
                            "&Doc_Budget=" + dr["Doc_Budget"].ToString() + "&FlowUID=" + dr["FlowUID"].ToString() + "&Flow_StartDate=" + dr["Flow_StartDate"].ToString() + "&FlowStep1_UserUID=" + dr["FlowStep1_UserUID"].ToString() + "&FlowStep1_TargetDate=" + dr["FlowStep1_TargetDate"].ToString() + "&FlowStep2_UserUID=" + dr["FlowStep2_UserUID"].ToString() + "&FlowStep2_TargetDate=" + dr["FlowStep2_TargetDate"].ToString() + "&FlowStep3_UserUID=" + dr["FlowStep3_UserUID"].ToString() + "&FlowStep3_TargetDate=" + dr["FlowStep3_TargetDate"].ToString() + "&FlowStep4_UserUID=" + dr["FlowStep4_UserUID"].ToString() + "&FlowStep4_TargetDate=" + dr["FlowStep4_TargetDate"].ToString() + "&&FlowStep5_UserUID=" + dr["FlowStep5_UserUID"].ToString() + "&FlowStep5_TargetDate=" + dr["FlowStep5_TargetDate"].ToString() + "&Estimateddocuments=" + Estimateddocuments + "&Remarks=" + WebUtility.UrlEncode(dr["Remarks"].ToString()) + "&DocumentSearchType=" + dr["DocumentSearchType"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&IsSync=" + dr["IsSync"].ToString() +
                            "&FlowStep6_UserUID=" + dr["FlowStep6_UserUID"].ToString() + "&FlowStep6_TargetDate=" + dr["FlowStep6_TargetDate"].ToString() +
                            "&FlowStep7_UserUID=" + dr["FlowStep7_UserUID"].ToString() + "&FlowStep7_TargetDate=" + dr["FlowStep7_TargetDate"].ToString() +
                            "&FlowStep8_UserUID=" + dr["FlowStep8_UserUID"].ToString() + "&FlowStep8_TargetDate=" + dr["FlowStep8_TargetDate"].ToString() +
                             "&FlowStep9_UserUID=" + dr["FlowStep9_UserUID"].ToString() + "&FlowStep9_TargetDate=" + dr["FlowStep9_TargetDate"].ToString() +
                             "&FlowStep10_UserUID=" + dr["FlowStep10_UserUID"].ToString() + "&FlowStep10_TargetDate=" + dr["FlowStep10_TargetDate"].ToString() +
                             "&FlowStep11_UserUID=" + dr["FlowStep11_UserUID"].ToString() + "&FlowStep11_TargetDate=" + dr["FlowStep11_TargetDate"].ToString() +
                             "&FlowStep12_UserUID=" + dr["FlowStep12_UserUID"].ToString() + "&FlowStep12_TargetDate=" + dr["FlowStep12_TargetDate"].ToString() +
                             "&FlowStep13_UserUID=" + dr["FlowStep13_UserUID"].ToString() + "&FlowStep13_TargetDate=" + dr["FlowStep13_TargetDate"].ToString() +
                             "&FlowStep14_UserUID=" + dr["FlowStep14_UserUID"].ToString() + "&FlowStep14_TargetDate=" + dr["FlowStep14_TargetDate"].ToString() +
                             "&FlowStep15_UserUID=" + dr["FlowStep15_UserUID"].ToString() + "&FlowStep15_TargetDate=" + dr["FlowStep15_TargetDate"].ToString() +
                             "&FlowStep16_UserUID=" + dr["FlowStep16_UserUID"].ToString() + "&FlowStep16_TargetDate=" + dr["FlowStep16_TargetDate"].ToString() +
                             "&FlowStep17_UserUID=" + dr["FlowStep17_UserUID"].ToString() + "&FlowStep17_TargetDate=" + dr["FlowStep17_TargetDate"].ToString() +
                             "&FlowStep18_UserUID=" + dr["FlowStep18_UserUID"].ToString() + "&FlowStep18_TargetDate=" + dr["FlowStep18_TargetDate"].ToString() +
                             "&FlowStep19_UserUID=" + dr["FlowStep19_UserUID"].ToString() + "&FlowStep19_TargetDate=" + dr["FlowStep19_TargetDate"].ToString() +
                             "&FlowStep20_UserUID=" + dr["FlowStep20_UserUID"].ToString() + "&FlowStep20_TargetDate=" + dr["FlowStep20_TargetDate"].ToString() +
                             "&FlowStep1_IsMUser=" + dr["FlowStep1_IsMUser"].ToString() +
                             "&FlowStep2_IsMUser=" + dr["FlowStep2_IsMUser"].ToString() +
                             "&FlowStep3_IsMUser=" + dr["FlowStep3_IsMUser"].ToString() +
                             "&FlowStep4_IsMUser=" + dr["FlowStep4_IsMUser"].ToString() +
                             "&FlowStep5_IsMUser=" + dr["FlowStep5_IsMUser"].ToString() +
                             "&FlowStep6_IsMUser=" + dr["FlowStep6_IsMUser"].ToString() +
                             "&FlowStep7_IsMUser=" + dr["FlowStep7_IsMUser"].ToString() +
                             "&FlowStep8_IsMUser=" + dr["FlowStep8_IsMUser"].ToString() +
                             "&FlowStep9_IsMUser=" + dr["FlowStep9_IsMUser"].ToString() +
                             "&FlowStep10_IsMUser=" + dr["FlowStep10_IsMUser"].ToString() +
                             "&FlowStep11_IsMUser=" + dr["FlowStep11_IsMUser"].ToString() +
                             "&FlowStep12_IsMUser=" + dr["FlowStep12_IsMUser"].ToString() +
                             "&FlowStep13_IsMUser=" + dr["FlowStep13_IsMUser"].ToString() +
                             "&FlowStep14_IsMUser=" + dr["FlowStep14_IsMUser"].ToString() +
                             "&FlowStep15_IsMUser=" + dr["FlowStep15_IsMUser"].ToString() +
                             "&FlowStep16_IsMUser=" + dr["FlowStep16_IsMUser"].ToString() +
                             "&FlowStep17_IsMUser=" + dr["FlowStep17_IsMUser"].ToString() +
                             "&FlowStep18_IsMUser=" + dr["FlowStep18_IsMUser"].ToString() +
                             "&FlowStep19_IsMUser=" + dr["FlowStep19_IsMUser"].ToString() +
                             "&FlowStep20_IsMUser=" + dr["FlowStep20_IsMUser"].ToString();
                        data = webPostMethod(postData, URL);
                        //dbutility.Submittal_Insert_or_Update_Flow(new Guid(dr["DocumentUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["TaskUID"].ToString()), dr["DocName"].ToString(), new Guid(dr["Doc_Category"].ToString()), dr["Doc_RefNumber"].ToString(), dr["Doc_Type"].ToString(),
                        //     Convert.ToDouble(dr["Doc_Budget"].ToString()), new Guid(dr["FlowUID"].ToString()), DateTime.Parse(dr["Flow_StartDate"].ToString()), new Guid(dr["FlowStep1_UserUID"].ToString()), DateTime.Parse(dr["FlowStep1_TargetDate"].ToString()), dr["FlowStep2_UserUID"].ToString(), dr["FlowStep2_TargetDate"].ToString(), dr["FlowStep3_UserUID"].ToString(), dr["FlowStep3_TargetDate"].ToString(), dr["FlowStep4_UserUID"].ToString(), dr["FlowStep4_TargetDate"].ToString(), dr["FlowStep5_UserUID"].ToString(), dr["FlowStep5_TargetDate"].ToString(), Estimateddocuments, dr["Remarks"].ToString(), dr["DocumentSearchType"].ToString(), DateTime.Parse(dr["CreatedDate"].ToString()), dr["Delete_Flag"].ToString());
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateSubmittalUpdateFlag(new Guid(dr["DocumentUID"].ToString()));
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "Submittal Update", "Success", "");
                            Console.WriteLine("Synching for SubmittalUID Update : " + dr["DocumentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "Submittal Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Submittals Add Done : ");
                //Console.ReadLine();
            }
            catch(Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Submittal Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SynchActualDocuments() // for documents uploaded
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ActualDocuments where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("select * From ActualDocuments Where ServerCopiedAdd='N' and DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID  + "')", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "ActualDocumentsSync";// "http://localhost:50385/api/DbSync/ActualDocumentsSync";
                var data = "";
                Console.WriteLine("Started Synching for ActualDocuments Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ActualDocuments Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        //dbutility.Document_Insert_or_Update_ActualDocuments(new Guid(dr["ActualDocumentUID"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), new Guid(dr["DocumentUID"].ToString()), dr["ProjectRef_Number"].ToString(), dr["Ref_Number"].ToString(),
                        //    dr["Doc_Type"].ToString(), DateTime.Parse(dr["IncomingRec_Date"].ToString()), new Guid(dr["FlowUID"].ToString()), dr["ActualDocument_Name"].ToString(), dr["Description"].ToString(), double.Parse(dr["ActualDocument_Version"].ToString()), dr["ActualDocument_Originator"].ToString(),
                        //    dr["Media_HC"].ToString(), dr["Media_SC"].ToString(), dr["Media_SCEF"].ToString(), dr["Media_HCR"].ToString(), dr["Media_SCR"].ToString(), dr["Media_NA"].ToString(), dr["ActualDocument_Path"].ToString(), dr["Remarks"].ToString(), dr["FileRef_Number"].ToString(), dr["ActualDocument_CurrentStatus"].ToString(),
                        //    dr["FlowStep1_TargetDate"].ToString(), dr["FlowStep2_TargetDate"].ToString(), dr["FlowStep3_TargetDate"].ToString(), dr["FlowStep4_TargetDate"].ToString(), dr["FlowStep5_TargetDate"].ToString(), dr["ActualDocument_Originator"].ToString(), dr["Document_Date"].ToString(), dr["ActualDocument_RelativePath"].ToString(), dr["ActualDocument_DirectoryName"].ToString(), dr["ActualDocument_CreatedDate"].ToString(), dr["Delete_Flag"].ToString());
                        // upload document file


                        if (uploaddoc(dr["ActualDocument_Path"].ToString()))
                        {
                            //
                            postData = "ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&DocumentUID=" + dr["DocumentUID"].ToString() + "&ProjectRef_Number=" + WebUtility.UrlEncode(dr["ProjectRef_Number"].ToString()) + "&Ref_Number=" + WebUtility.UrlEncode(dr["Ref_Number"].ToString()) +
                                "&Doc_Type=" + dr["Doc_Type"].ToString() + "&IncomingRec_Date=" + dr["IncomingRec_Date"].ToString() +
                               "&FlowUID=" + dr["FlowUID"].ToString() + "&ActualDocument_Name=" + WebUtility.UrlEncode(dr["ActualDocument_Name"].ToString()) + "&Description=" + WebUtility.UrlEncode(dr["Description"].ToString()) + "&ActualDocument_Version=" + dr["ActualDocument_Version"].ToString() + "&ActualDocument_Type=" + dr["ActualDocument_Type"].ToString() +
                               "&Media_HC=" + dr["Media_HC"].ToString() + "&Media_SC=" + dr["Media_SC"].ToString() + "&Media_SCEF=" + dr["Media_SCEF"].ToString() + "&Media_HCR=" + dr["Media_HCR"].ToString() + "&Media_SCR=" + dr["Media_SCR"].ToString() + "&Media_NA=" + dr["Media_NA"].ToString() + "&ActualDocument_Path=" + WebUtility.UrlEncode(dr["ActualDocument_Path"].ToString()) + "&Remarks=" + WebUtility.UrlEncode(dr["Remarks"].ToString()) + "&FileRef_Number=" + WebUtility.UrlEncode(dr["FileRef_Number"].ToString()) + "&ActualDocument_CurrentStatus=" + dr["ActualDocument_CurrentStatus"].ToString() +
                               "&FlowStep1_TargetDate=" + dr["FlowStep1_TargetDate"].ToString() + "&FlowStep2_TargetDate=" + dr["FlowStep2_TargetDate"].ToString() + "&FlowStep3_TargetDate=" + dr["FlowStep3_TargetDate"].ToString() +
                                "&FlowStep4_TargetDate=" + dr["FlowStep4_TargetDate"].ToString() + "&FlowStep5_TargetDate=" + dr["FlowStep5_TargetDate"].ToString() + "&ActualDocument_Originator=" + dr["ActualDocument_Originator"].ToString() +
                                "&Document_Date=" + dr["Document_Date"].ToString() + "&ActualDocument_RelativePath=" + WebUtility.UrlEncode(dr["ActualDocument_RelativePath"].ToString()) + "&ActualDocument_DirectoryName=" + WebUtility.UrlEncode(dr["ActualDocument_DirectoryName"].ToString()) +
                                "&ActualDocument_CreatedDate=" + dr["ActualDocument_CreatedDate"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&CoverLetterUID=" + dr["CoverLetterUID"].ToString() + "&SubmissionType=" + dr["SubmissionType"].ToString() +
                                "&FlowStep6_TargetDate=" + dr["FlowStep6_TargetDate"].ToString() +
                                "&FlowStep7_TargetDate=" + dr["FlowStep7_TargetDate"].ToString() +
                                "&FlowStep8_TargetDate=" + dr["FlowStep8_TargetDate"].ToString() +
                                "&FlowStep9_TargetDate=" + dr["FlowStep9_TargetDate"].ToString() +
                                "&FlowStep10_TargetDate=" + dr["FlowStep10_TargetDate"].ToString() +
                                "&FlowStep11_TargetDate=" + dr["FlowStep11_TargetDate"].ToString() +
                               "&FlowStep12_TargetDate=" + dr["FlowStep12_TargetDate"].ToString() +
                               "&FlowStep13_TargetDate=" + dr["FlowStep13_TargetDate"].ToString() +
                               "&FlowStep14_TargetDate=" + dr["FlowStep14_TargetDate"].ToString() +
                               "&FlowStep15_TargetDate=" + dr["FlowStep15_TargetDate"].ToString() +
                               "&FlowStep16_TargetDate=" + dr["FlowStep16_TargetDate"].ToString() +
                               "&FlowStep17_TargetDate=" + dr["FlowStep17_TargetDate"].ToString() +
                               "&FlowStep18_TargetDate=" + dr["FlowStep18_TargetDate"].ToString() +
                               "&FlowStep19_TargetDate=" + dr["FlowStep19_TargetDate"].ToString() +
                               "&FlowStep20_TargetDate=" + dr["FlowStep20_TargetDate"].ToString() ;
                            data = webPostMethod(postData, URL);
                            if (data.ToString().Contains("true"))
                            {
                                dbutility.updateActualDocumentsAddFlag(new Guid(dr["ActualDocumentUID"].ToString()));
                                dbutility.InsertintoDbsyncLogs(new Guid(dr["ActualDocumentUID"].ToString()), "ActualDocuments Add", "Success", "");
                                Console.WriteLine("Synching for ActualDocuments Add : " + dr["DocumentUID"].ToString() + " Done");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "ActualDocuments Update", "Error", ex.Message);
                    }
                }
                //
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ActualDocuments where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("select * From ActualDocuments Where ServerCopiedUpdate='N' and DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "')", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for ActualDocuments Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ActualDocuments Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        //dbutility.Document_Insert_or_Update_ActualDocuments(new Guid(dr["ActualDocumentUID"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), new Guid(dr["DocumentUID"].ToString()), dr["ProjectRef_Number"].ToString(), dr["Ref_Number"].ToString(),
                        //  dr["Doc_Type"].ToString(), DateTime.Parse(dr["IncomingRec_Date"].ToString()), new Guid(dr["FlowUID"].ToString()), dr["ActualDocument_Name"].ToString(), dr["Description"].ToString(), double.Parse(dr["ActualDocument_Version"].ToString()), dr["ActualDocument_Originator"].ToString(),
                        //  dr["Media_HC"].ToString(), dr["Media_SC"].ToString(), dr["Media_SCEF"].ToString(), dr["Media_HCR"].ToString(), dr["Media_SCR"].ToString(), dr["Media_NA"].ToString(), dr["ActualDocument_Path"].ToString(), dr["Remarks"].ToString(), dr["FileRef_Number"].ToString(), dr["ActualDocument_CurrentStatus"].ToString(),
                        //  dr["FlowStep1_TargetDate"].ToString(), dr["FlowStep2_TargetDate"].ToString(), dr["FlowStep3_TargetDate"].ToString(), dr["FlowStep4_TargetDate"].ToString(), dr["FlowStep5_TargetDate"].ToString(), dr["ActualDocument_Originator"].ToString(), dr["Document_Date"].ToString(), dr["ActualDocument_RelativePath"].ToString(), dr["ActualDocument_DirectoryName"].ToString(), dr["ActualDocument_CreatedDate"].ToString(), dr["Delete_Flag"].ToString());
                        postData = "ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&DocumentUID=" + dr["DocumentUID"].ToString() + "&ProjectRef_Number=" + WebUtility.UrlEncode(dr["ProjectRef_Number"].ToString()) + "&Ref_Number=" + WebUtility.UrlEncode(dr["Ref_Number"].ToString()) +
                                "&Doc_Type=" + dr["Doc_Type"].ToString() + "&IncomingRec_Date=" + dr["IncomingRec_Date"].ToString() +
                               "&FlowUID=" + dr["FlowUID"].ToString() + "&ActualDocument_Name=" + WebUtility.UrlEncode(dr["ActualDocument_Name"].ToString()) + "&Description=" + WebUtility.UrlEncode(dr["Description"].ToString()) + "&ActualDocument_Version=" + dr["ActualDocument_Version"].ToString() + "&ActualDocument_Type=" + dr["ActualDocument_Type"].ToString() +
                               "&Media_HC=" + dr["Media_HC"].ToString() + "&Media_SC=" + dr["Media_SC"].ToString() + "&Media_SCEF=" + dr["Media_SCEF"].ToString() + "&Media_HCR=" + dr["Media_HCR"].ToString() + "&Media_SCR=" + dr["Media_SCR"].ToString() + "&Media_NA=" + dr["Media_NA"].ToString() + "&ActualDocument_Path=" + WebUtility.UrlEncode(dr["ActualDocument_Path"].ToString()) + "&Remarks=" + WebUtility.UrlEncode(dr["Remarks"].ToString()) + "&FileRef_Number=" + WebUtility.UrlEncode(dr["FileRef_Number"].ToString()) + "&ActualDocument_CurrentStatus=" + dr["ActualDocument_CurrentStatus"].ToString() +
                               "&FlowStep1_TargetDate=" + dr["FlowStep1_TargetDate"].ToString() + "&FlowStep2_TargetDate=" + dr["FlowStep2_TargetDate"].ToString() + "&FlowStep3_TargetDate=" + dr["FlowStep3_TargetDate"].ToString() +
                                "&FlowStep4_TargetDate=" + dr["FlowStep4_TargetDate"].ToString() + "&FlowStep5_TargetDate=" + dr["FlowStep5_TargetDate"].ToString() + "&ActualDocument_Originator=" + dr["ActualDocument_Originator"].ToString() +
                                "&Document_Date=" + dr["Document_Date"].ToString() + "&ActualDocument_RelativePath=" + WebUtility.UrlEncode(dr["ActualDocument_RelativePath"].ToString()) + "&ActualDocument_DirectoryName=" + WebUtility.UrlEncode(dr["ActualDocument_DirectoryName"].ToString()) +
                                "&ActualDocument_CreatedDate=" + dr["ActualDocument_CreatedDate"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&CoverLetterUID=" + dr["CoverLetterUID"].ToString() + "&SubmissionType=" + dr["SubmissionType"].ToString() +
                                "&FlowStep6_TargetDate=" + dr["FlowStep6_TargetDate"].ToString() +
                                "&FlowStep7_TargetDate=" + dr["FlowStep7_TargetDate"].ToString() +
                                "&FlowStep8_TargetDate=" + dr["FlowStep8_TargetDate"].ToString() +
                                "&FlowStep9_TargetDate=" + dr["FlowStep9_TargetDate"].ToString() +
                                "&FlowStep10_TargetDate=" + dr["FlowStep10_TargetDate"].ToString() +
                                "&FlowStep11_TargetDate=" + dr["FlowStep11_TargetDate"].ToString() +
                               "&FlowStep12_TargetDate=" + dr["FlowStep12_TargetDate"].ToString() +
                               "&FlowStep13_TargetDate=" + dr["FlowStep13_TargetDate"].ToString() +
                               "&FlowStep14_TargetDate=" + dr["FlowStep14_TargetDate"].ToString() +
                               "&FlowStep15_TargetDate=" + dr["FlowStep15_TargetDate"].ToString() +
                               "&FlowStep16_TargetDate=" + dr["FlowStep16_TargetDate"].ToString() +
                               "&FlowStep17_TargetDate=" + dr["FlowStep17_TargetDate"].ToString() +
                               "&FlowStep18_TargetDate=" + dr["FlowStep18_TargetDate"].ToString() +
                               "&FlowStep19_TargetDate=" + dr["FlowStep19_TargetDate"].ToString() +
                               "&FlowStep20_TargetDate=" + dr["FlowStep20_TargetDate"].ToString();
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateActualDocumentsUpdateFlag(new Guid(dr["ActualDocumentUID"].ToString()));
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["ActualDocumentUID"].ToString()), "ActualDocuments Update", "Success", "");
                            Console.WriteLine("Synching for ActualDocumentsUID Update : " + dr["DocumentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "ActualDocuments Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for ActualDocuments Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "ActualDocuments Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
               // Console.ReadLine();

            }
        }

        private void SynchDocumentStatus()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentStatus where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DocumentStatusSync"; //"http://localhost:50385/api/DbSync/DocumentStatusSync";
                var data = "";


                Console.WriteLine("Started Synching for DocumentStatus Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentStatus Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        //dbutility.Document_Insert_or_Update_ActualDocuments(new Guid(dr["ActualDocumentUID"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), new Guid(dr["DocumentUID"].ToString()), dr["ProjectRef_Number"].ToString(), dr["Ref_Number"].ToString(),
                        //    dr["Doc_Type"].ToString(), DateTime.Parse(dr["IncomingRec_Date"].ToString()), new Guid(dr["FlowUID"].ToString()), dr["ActualDocument_Name"].ToString(), dr["Description"].ToString(), double.Parse(dr["ActualDocument_Version"].ToString()), dr["ActualDocument_Originator"].ToString(),
                        //    dr["Media_HC"].ToString(), dr["Media_SC"].ToString(), dr["Media_SCEF"].ToString(), dr["Media_HCR"].ToString(), dr["Media_SCR"].ToString(), dr["Media_NA"].ToString(), dr["ActualDocument_Path"].ToString(), dr["Remarks"].ToString(), dr["FileRef_Number"].ToString(), dr["ActualDocument_CurrentStatus"].ToString(),
                        //    dr["FlowStep1_TargetDate"].ToString(), dr["FlowStep2_TargetDate"].ToString(), dr["FlowStep3_TargetDate"].ToString(), dr["FlowStep4_TargetDate"].ToString(), dr["FlowStep5_TargetDate"].ToString(), dr["ActualDocument_Originator"].ToString(), dr["Document_Date"].ToString(), dr["ActualDocument_RelativePath"].ToString(), dr["ActualDocument_DirectoryName"].ToString(), dr["ActualDocument_CreatedDate"].ToString(), dr["Delete_Flag"].ToString());
                        //dbutility.InsertorUpdateDocumentStatus(new Guid(dr["StatusUID"].ToString()), new Guid(dr["DocumentUID"].ToString()), double.Parse(dr["Version"].ToString()), dr["ActivityType"].ToString(), dr["Activity_Budget"].ToString(),
                        //   DateTime.Parse(dr["ActivityDate"].ToString()), dr["LinkToReviewFile"].ToString(), new Guid(dr["AcivityUserUID"].ToString()), dr["Status_Comments"].ToString(), dr["Current_Status"].ToString(), dr["Ref_Number"].ToString(), dr["DocumentDate"].ToString(), dr["CoverLetterFile"].ToString(), dr["Delete_Flag"].ToString());
                        if(dr["CoverLetterFile"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["CoverLetterFile"].ToString()))
                            {
                                uploaddoc(dr["CoverLetterFile"].ToString());
                            }
                        }
                        if (dr["LinkToReviewFile"] != System.DBNull.Value)
                        {
                            if (!String.IsNullOrEmpty(dr["LinkToReviewFile"].ToString()))
                            {
                                UploadReviewFile(dr["LinkToReviewFile"].ToString());
                            }
                        }
                        postData = "StatusUID=" + dr["StatusUID"].ToString() + "&DocumentUID=" + dr["DocumentUID"].ToString() + "&Version=" + dr["Version"] + "&ActivityType=" + dr["ActivityType"].ToString() + "&Activity_Budget=" + dr["Activity_Budget"].ToString() + "&ActivityDate=" + dr["ActivityDate"].ToString() + "&LinkToReviewFile=" + WebUtility.UrlEncode(dr["LinkToReviewFile"].ToString()) + "&AcivityUserUID=" + dr["AcivityUserUID"].ToString() +
                          "&Status_Comments=" + WebUtility.UrlEncode(dr["Status_Comments"].ToString()) + "&Current_Status=" + dr["Current_Status"].ToString() + "&Ref_Number=" + WebUtility.UrlEncode(dr["Ref_Number"].ToString()) + "&DocumentDate=" + dr["DocumentDate"].ToString() + "&CoverLetterFile=" + WebUtility.UrlEncode(dr["CoverLetterFile"].ToString()) + "&Delete_Flag=" + dr["Delete_Flag"].ToString() +
                          "&Origin=" + dr["Origin"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString()  + "&Forwarded=" + dr["Forwarded"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["StatusUID"].ToString()), "DocumentStatus", "StatusUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["StatusUID"].ToString()), "DocumentStatus Add", "Success", "");
                            Console.WriteLine("Synching for DocumentStatus Add : " + dr["StatusUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["StatusUID"].ToString()), "DocumentStatus Add", "Error", ex.Message);
                    }
                }
                //
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentStatus where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentStatus where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for DocumentStatus Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentStatus Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "StatusUID=" + dr["StatusUID"].ToString() + "&DocumentUID=" + dr["DocumentUID"].ToString() + "&Version=" + dr["Version"] + "&ActivityType=" + dr["ActivityType"].ToString() + "&Activity_Budget=" + dr["Activity_Budget"].ToString() + "&ActivityDate=" + dr["ActivityDate"].ToString() + "&LinkToReviewFile=" + WebUtility.UrlEncode(dr["LinkToReviewFile"].ToString()) + "&AcivityUserUID=" + dr["AcivityUserUID"].ToString() +
                          "&Status_Comments=" + WebUtility.UrlEncode(dr["Status_Comments"].ToString()) + "&Current_Status=" + dr["Current_Status"].ToString() + "&Ref_Number=" + WebUtility.UrlEncode(dr["Ref_Number"].ToString()) + "&DocumentDate=" + dr["DocumentDate"].ToString() + "&CoverLetterFile=" + WebUtility.UrlEncode(dr["CoverLetterFile"].ToString()) + "&Delete_Flag=" + dr["Delete_Flag"].ToString() +
                          "&Origin=" + dr["Origin"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&Forwarded=" + dr["Forwarded"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["StatusUID"].ToString()), "DocumentStatus", "StatusUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["StatusUID"].ToString()), "DocumentStatus Update", "Success", "");
                            Console.WriteLine("Synching for DocumentStatusUID Update : " + dr["StatusUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["StatusUID"].ToString()), "DocumentStatus Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for DocumentStatus Add Done : ");
               // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "DocumentStatus Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
              //  Console.ReadLine();

            }
        }

        private void SynchDocumentVersion()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                //MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DocumentVersionSync"; //string URL = "http://localhost:50385/api/DbSync/DocumentVersionSync";
                var data = "";
                Console.WriteLine("Started Synching for DocumentVersion Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentVersion Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //dbutility.InsertDocumentorUpdateVersion(new Guid(dr["DocVersion_UID"].ToString()), new Guid(dr["DocStatus_UID"].ToString()), new Guid(dr["DocumentUID"].ToString()), dr["Doc_Type"].ToString(), dr["Doc_FileName"].ToString(), dr["Doc_Comments"].ToString(), int.Parse(dr["Doc_Version"].ToString()),
                        //     dr["Doc_Status"].ToString(), dr["Doc_StatusDate"].ToString(), dr["Delete_Flag"].ToString());
                        if (dr["Doc_FileName"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Doc_FileName"].ToString()))
                            {
                                uploaddoc(dr["Doc_FileName"].ToString());
                            }
                        }
                        if (dr["Doc_CoverLetter"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Doc_CoverLetter"].ToString()))
                            {
                                uploaddoc(dr["Doc_CoverLetter"].ToString());
                            }
                        }

                        postData = "DocVersion_UID=" + dr["DocVersion_UID"].ToString() + "&DocStatus_UID=" + dr["DocStatus_UID"].ToString() + "&DocumentUID=" + dr["DocumentUID"] + "&Doc_Type=" + dr["Doc_Type"].ToString() + "&Doc_FileName=" + WebUtility.UrlEncode(dr["Doc_FileName"].ToString()) + "&Doc_Comments=" + WebUtility.UrlEncode(dr["Doc_Comments"].ToString()) + "&Doc_Version=" + dr["Doc_Version"].ToString() + "&Doc_Status=" + dr["Doc_Status"].ToString() +
                          "&Doc_StatusDate=" + dr["Doc_StatusDate"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&Doc_CoverLetter=" + WebUtility.UrlEncode(dr["Doc_CoverLetter"].ToString());

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion", "DocVersion_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion Add", "Success", "");
                            Console.WriteLine("Synching for DocumentVesrion Add : " + dr["DocVersion_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for DocumentVesrion Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentVesrion Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "DocVersion_UID=" + dr["DocVersion_UID"].ToString() + "&DocStatus_UID=" + dr["DocStatus_UID"].ToString() + "&DocumentUID=" + dr["DocumentUID"] + "&Doc_Type=" + dr["Doc_Type"].ToString() + "&Doc_FileName=" + WebUtility.UrlEncode(dr["Doc_FileName"].ToString()) + "&Doc_Comments=" + WebUtility.UrlEncode(dr["Doc_Comments"].ToString()) + "&Doc_Version=" + dr["Doc_Version"].ToString() + "&Doc_Status=" + dr["Doc_Status"].ToString() +
                          "&Doc_StatusDate=" + dr["Doc_StatusDate"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&Doc_CoverLetter=" + WebUtility.UrlEncode(dr["Doc_CoverLetter"].ToString());

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion", "DocVersion_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion Update", "Success", "");
                            Console.WriteLine("Synching for DocumentVesrion Update : " + dr["DocVersion_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for DocumentVesrion Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "DocumentVesrion Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
               // Console.ReadLine();

            }
        }

        private void SynchReferencNoHistory()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                //MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ReferencNoHistory where ServerCopiedAdd='N' and ActualDocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "ReferencNoHistorySync"; //string URL = "http://localhost:50385/api/DbSync/DocumentVersionSync";
                var data = "";
                Console.WriteLine("Started Synching for ReferencNoHistory Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ReferencNoHistory Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "UID=" + dr["UID"].ToString() + "&ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&OriginatorReferenceNo=" + WebUtility.UrlEncode(dr["OriginatorReferenceNo"].ToString()) + "&ONTBRefNo=" + WebUtility.UrlEncode(dr["ONTBRefNo"].ToString()) + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "ReferencNoHistory", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ReferencNoHistory Add", "Success", "");
                            Console.WriteLine("Synching for ReferencNoHistory Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ReferencNoHistory Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ReferencNoHistory where ServerCopiedUpdate='N' and ActualDocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for ReferencNoHistory Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ReferencNoHistory Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&OriginatorReferenceNo=" + WebUtility.UrlEncode(dr["OriginatorReferenceNo"].ToString()) + "&ONTBRefNo=" + WebUtility.UrlEncode(dr["ONTBRefNo"].ToString()) + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "ReferencNoHistory", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ReferencNoHistory Update", "Success", "");
                            Console.WriteLine("Synching for ReferencNoHistory Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ReferencNoHistory Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for ReferencNoHistory Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "ReferencNoHistory Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                // Console.ReadLine();

            }
        }

        private void SynchSubmittal_MultipleUsers()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                //MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("select * From Submittal_MultipleUsers Where ServerCopiedAdd='N' and SubmittalUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "')", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "Submittal_MultipleUsersSync"; //string URL = "http://localhost:50385/api/DbSync/DocumentVersionSync";
                var data = "";
                Console.WriteLine("Started Synching for Submittal_MultipleUsers Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Submittal_MultipleUsers Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "UID=" + dr["UID"].ToString() + "&SubmittalUID=" + dr["SubmittalUID"].ToString() + "&Step=" + dr["Step"].ToString() + "&UserUID=" + dr["UserUID"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "Submittal_MultipleUsers", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Submittal_MultipleUsers Add", "Success", "");
                            Console.WriteLine("Synching for Submittal_MultipleUsers Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Submittal_MultipleUsers Add", "Error", ex.Message);
                    }
                }
               
                Console.WriteLine("Synching for Submittal_MultipleUsers Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Submittal_MultipleUsers Add", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                // Console.ReadLine();

            }
        }

        private void SynchFlow_Master_Users()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                //MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Flow_Master_Users where ServerCopiedAdd='N' and WorkpackageUID='" + wkPkgUID + "'", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "Flow_Master_UsersSync"; //string URL = "http://localhost:50385/api/DbSync/DocumentVersionSync";
                var data = "";
                Console.WriteLine("Started Synching for Flow_Master_Users Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Flow_Master_Users Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "UID=" + dr["UID"].ToString() + "&FlowUID=" + dr["FlowUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&WorkpackagecategoryUID=" + dr["WorkpackagecategoryUID"].ToString() + "&step=" + dr["step"].ToString() + "&UserUID=" + dr["UserUID"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() +
                          "&DeletedFlag=" + dr["DeletedFlag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "Flow_Master_Users", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Flow_Master_Users Add", "Success", "");
                            Console.WriteLine("Synching for Flow_Master_Users Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Flow_Master_Users Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Flow_Master_Users where ServerCopiedUpdate='N' and WorkpackageUID='" + wkPkgUID + "'", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Flow_Master_Users Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Flow_Master_Users Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&FlowUID=" + dr["FlowUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&WorkpackagecategoryUID=" + dr["WorkpackagecategoryUID"].ToString() + "&step=" + dr["step"].ToString() + "&UserUID=" + dr["UserUID"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() +
                           "&DeletedFlag=" + dr["DeletedFlag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "Flow_Master_Users", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Flow_Master_Users Update", "Success", "");
                            Console.WriteLine("Synching for Flow_Master_Users Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Flow_Master_Users Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Flow_Master_Users Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Flow_Master_Users Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                // Console.ReadLine();

            }
        }

        private void SynchMeasurementBook()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                //MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("select * From MeasurementBook Where ServerCopiedAdd='N' and TaskUID in (Select TaskUID From Tasks where WorkPackageUID='" + wkPkgUID + "')", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "MeasurementBookSync"; //string URL = "http://localhost:50385/api/DbSync/DocumentVersionSync";
                var data = "";
                Console.WriteLine("Started Synching for MeasurementBook Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for MeasurementBook Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "UID=" + dr["UID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&UnitforProgress=" + dr["UnitforProgress"] + "&Quantity=" + dr["Quantity"].ToString() + "&Description=" + dr["Description"].ToString() + "&Upload_File=" + dr["Upload_File"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&CreatedByUID=" + dr["CreatedByUID"].ToString() +
                          "&Remarks=" + dr["Remarks"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&Achieved_Date=" + dr["Achieved_Date"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "MeasurementBook", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "MeasurementBook Add", "Success", "");
                            Console.WriteLine("Synching for MeasurementBook Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "MeasurementBook Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentVesrion where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("select * From MeasurementBook Where ServerCopiedUpdate='N' and TaskUID in (Select TaskUID From Tasks where WorkPackageUID='" + wkPkgUID + "')", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for MeasurementBook Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for MeasurementBook Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&UnitforProgress=" + dr["UnitforProgress"] + "&Quantity=" + dr["Quantity"].ToString() + "&Description=" + dr["Description"].ToString() + "&Upload_File=" + dr["Upload_File"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&CreatedByUID=" + dr["CreatedByUID"].ToString() +
                            "&Remarks=" + dr["Remarks"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&Achieved_Date=" + dr["Achieved_Date"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "MeasurementBook", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "MeasurementBook Update", "Success", "");
                            Console.WriteLine("Synching for MeasurementBook Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "MeasurementBook Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for MeasurementBook Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "MeasurementBook Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                // Console.ReadLine();

            }
        }

        private void SynchDocumentFlowData()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentFlowData where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentFlowData where ServerCopiedAdd='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);

                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DocumentFlowDataSync"; ;// string URL = "http://localhost:50385/api/DbSync/DocumentFlowDataSync";
                var data = "";
                Console.WriteLine("Started Synching for DocumentFlowData Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentFlowData Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        //dbutility.InsertorUpdateDocumentflowdata(new Guid(dr["DocumentFlow_UID"].ToString()), new Guid(dr["FlowMasterUID"].ToString()), new Guid(dr["DocumentUID"].ToString()), new Guid(dr["FlowStep1_UserID"].ToString()), new Guid(dr["FlowStep1_UserRole"].ToString()), DateTime.Parse(dr["FlowStep1_TargetDate"].ToString()), dr["FlowStep1_DisplayName"].ToString(),
                        //    dr["FlowStep2_UserID"].ToString(), dr["FlowStep2_UserRole"].ToString(), dr["FlowStep2_TargetDate"].ToString(), dr["FlowStep2_DisplayName"].ToString(),
                        //    dr["FlowStep3_UserID"].ToString(), dr["FlowStep3_UserRole"].ToString(), dr["FlowStep3_TargetDate"].ToString(), dr["FlowStep3_DisplayName"].ToString(),
                        //    dr["FlowStep4_UserID"].ToString(), dr["FlowStep4_UserRole"].ToString(), dr["FlowStep4_TargetDate"].ToString(), dr["FlowStep4_DisplayName"].ToString(),
                        //    dr["FlowStep5_UserID"].ToString(), dr["FlowStep5_UserRole"].ToString(), dr["FlowStep5_TargetDate"].ToString(), dr["FlowStep5_DisplayName"].ToString(),
                        //    dr["FlowStep6_UserID"].ToString(), dr["FlowStep6_UserRole"].ToString(), dr["FlowStep6_TargetDate"].ToString(), dr["FlowStep6_DisplayName"].ToString(), dr["Delete_Flag"].ToString());
                        postData = "DocumentFlow_UID=" + dr["DocumentFlow_UID"].ToString() + "&FlowMasterUID=" + dr["FlowMasterUID"].ToString() + "&DocumentUID=" + dr["DocumentUID"] + "&FlowStep1_UserID=" + dr["FlowStep1_UserID"].ToString() + "&FlowStep1_UserRole=" + dr["FlowStep1_UserRole"].ToString() + "&FlowStep1_TargetDate=" + dr["FlowStep1_TargetDate"].ToString() + "&FlowStep1_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep1_DisplayName"].ToString()) +
                            "&FlowStep2_UserID=" + dr["FlowStep2_UserID"].ToString() + "&FlowStep2_UserRole=" + dr["FlowStep2_UserRole"].ToString() + "&FlowStep2_TargetDate=" + dr["FlowStep2_TargetDate"].ToString() + "&FlowStep2_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep2_DisplayName"].ToString()) +
                            "&FlowStep3_UserID=" + dr["FlowStep3_UserID"].ToString() + "&FlowStep3_UserRole=" + dr["FlowStep3_UserRole"].ToString() + "&FlowStep3_TargetDate=" + dr["FlowStep3_TargetDate"].ToString() + "&FlowStep3_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep3_DisplayName"].ToString()) +
                            "&FlowStep4_UserID=" + dr["FlowStep4_UserID"].ToString() + "&FlowStep4_UserRole=" + dr["FlowStep4_UserRole"].ToString() + "&FlowStep4_TargetDate=" + dr["FlowStep4_TargetDate"].ToString() + "&FlowStep4_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep4_DisplayName"].ToString()) +
                            "&FlowStep5_UserID=" + dr["FlowStep5_UserID"].ToString() + "&FlowStep5_UserRole=" + dr["FlowStep5_UserRole"].ToString() + "&FlowStep5_TargetDate=" + dr["FlowStep5_TargetDate"].ToString() + "&FlowStep5_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep5_DisplayName"].ToString()) +
                            "&FlowStep6_UserID=" + dr["FlowStep6_UserID"].ToString() + "&FlowStep6_UserRole=" + dr["FlowStep6_UserRole"].ToString() + "&FlowStep6_TargetDate=" + dr["FlowStep6_TargetDate"].ToString() + "&FlowStep6_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep6_DisplayName"].ToString()) + "&Delete_Flag=" + dr["Delete_Flag"].ToString();
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DocumentFlow_UID"].ToString()), "DocumentFlowData", "DocumentFlow_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentFlow_UID"].ToString()), "DocumentFlowData Add", "Success", "");
                            Console.WriteLine("Synching for DocumentFlowData Add : " + dr["DocumentFlow_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentFlow_UID"].ToString()), "DocumentFlowData Add", "Error", ex.Message);
                    }
                }
                //
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentFlowData where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentFlowData where ServerCopiedUpdate='N' and DocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for DocumentFlowData Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentFlowData Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "DocumentFlow_UID=" + dr["DocumentFlow_UID"].ToString() + "&FlowMasterUID=" + dr["FlowMasterUID"].ToString() + "&DocumentUID=" + dr["DocumentUID"] + "&FlowStep1_UserID=" + dr["FlowStep1_UserID"].ToString() + "&FlowStep1_UserRole=" + dr["FlowStep1_UserRole"].ToString() + "&FlowStep1_TargetDate=" + dr["FlowStep1_TargetDate"].ToString() + "&FlowStep1_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep1_DisplayName"].ToString()) +
                             "&FlowStep2_UserID=" + dr["FlowStep2_UserID"].ToString() + "&FlowStep2_UserRole=" + dr["FlowStep2_UserRole"].ToString() + "&FlowStep2_TargetDate=" + dr["FlowStep2_TargetDate"].ToString() + "&FlowStep2_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep2_DisplayName"].ToString()) +
                             "&FlowStep3_UserID=" + dr["FlowStep3_UserID"].ToString() + "&FlowStep3_UserRole=" + dr["FlowStep3_UserRole"].ToString() + "&FlowStep3_TargetDate=" + dr["FlowStep3_TargetDate"].ToString() + "&FlowStep3_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep3_DisplayName"].ToString()) +
                             "&FlowStep4_UserID=" + dr["FlowStep4_UserID"].ToString() + "&FlowStep4_UserRole=" + dr["FlowStep4_UserRole"].ToString() + "&FlowStep4_TargetDate=" + dr["FlowStep4_TargetDate"].ToString() + "&FlowStep4_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep4_DisplayName"].ToString()) +
                             "&FlowStep5_UserID=" + dr["FlowStep5_UserID"].ToString() + "&FlowStep5_UserRole=" + dr["FlowStep5_UserRole"].ToString() + "&FlowStep5_TargetDate=" + dr["FlowStep5_TargetDate"].ToString() + "&FlowStep5_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep5_DisplayName"].ToString()) +
                             "&FlowStep6_UserID=" + dr["FlowStep6_UserID"].ToString() + "&FlowStep6_UserRole=" + dr["FlowStep6_UserRole"].ToString() + "&FlowStep6_TargetDate=" + dr["FlowStep6_TargetDate"].ToString() + "&FlowStep6_DisplayName=" + WebUtility.UrlEncode(dr["FlowStep6_DisplayName"].ToString()) + "&Delete_Flag=" + dr["Delete_Flag"].ToString();
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DocumentFlow_UID"].ToString()), "DocumentFlowData", "DocumentFlow_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentFlow_UID"].ToString()), "DocumentFlowData Update", "Success", "");
                            Console.WriteLine("Synching for DocumentFlowData Update : " + dr["DocumentFlow_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentFlow_UID"].ToString()), "DocumentFlowData Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for DocumentFlowData Add Done : ");
               // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "DocumentFlowData Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
               // Console.ReadLine();

            }
        }

        private void SynchWorddocRead()
        {
            try
            {
                // source




                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From WordDocRead where ServerCopiedAdd='N' and DocumemtUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From WordDocRead where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "WorddocReadSync"; //string URL = "http://localhost:50385/api/DbSync/WorddocReadSync";
                var data = "";
                Console.WriteLine("Started Synching for Word doc Read Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Word doc read Add : ");
                }

                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        string docPath = dr["Doc_path"].ToString().Replace(SourceWordocReadPath,DestWordocReadPath);
                        postData = "UID=" + dr["UID"].ToString() + "&Doc_path=" + docPath + "&Status=" + dr["Status"] + "&HTML_Text=" + dr["HTML_Text"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DocumemtUID=" + dr["DocumemtUID"].ToString() + "&Encrypted=" + dr["Encrypted"].ToString();
                        
                           
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "WordDocRead", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "WordDocRead Add", "Success", "");
                            Console.WriteLine("Synching for WordDocRead Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "WordDocRead Add", "Error", ex.Message);
                    }
                }
                //
               
                Console.WriteLine("Synching for Word Doc Read Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Word Doc Read Add", "Error", ex.Message);
                Console.WriteLine(ex.Message);
               // Console.ReadLine();

            }
        }

        private void SyncDocumentsUploadLog()
        {
            try
            {
                // source




                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentsUploadLog where ServerCopiedAdd='N' and ActualDocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DocumentsUploadLog where ServerCopiedAdd='N' and ActualDocumentUID in (select ActualDocumentUID FRom [dbo].[ActualDocuments] where DocumentUID in (Select DocumentUID From Documents Where IsSync='Y' and WorkPackageUID='" + wkPkgUID + "'))", MyConnection);

                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DocumentsUploadLog"; //string URL = "http://localhost:50385/api/DbSync/DocumentsUploadLog";
                var data = "";
                Console.WriteLine("Started Synching for DocumentsUploadLog Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DocumentsUploadLog Add : ");
                }

                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        postData = "ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&UploadStartDate=" + dr["UploadStartDate"].ToString() + "&UploadEndDate=" + dr["UploadEndDate"] + "&UploadUserUID=" + dr["UploadUserUID"].ToString() + "&Duration=" + dr["Duration"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["ActualDocumentUID"].ToString()), "DocumentsUploadLog", "ActualDocumentUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["ActualDocumentUID"].ToString()), "DocumentsUploadLog Add", "Success", "");
                            Console.WriteLine("Synching for DocumentsUploadLog Add : " + dr["ActualDocumentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["ActualDocumentUID"].ToString()), "DocumentsUploadLog Add", "Error", ex.Message);
                    }
                }
                //

                Console.WriteLine("Synching for DocumentsUploadLog Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Word Doc Read Add", "Error", ex.Message);
                Console.WriteLine(ex.Message);
              //  Console.ReadLine();

            }
        }


        //private void SynchRABill_Abstract()
        //{
        //    try
        //    {
        //        // source
        //        //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
        //        System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
        //        MyConnection.ConnectionString = GetSourceConnectionString();
        //        System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
        //        System.Data.DataSet MyDataset = new System.Data.DataSet();
        //        System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
        //        MyCommand.Connection = MyConnection;

        //        // check the submittal table for any records to be added or updated.....
        //        MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABill_Abstract where ServerCopiedAdd='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
        //        //

        //        MyDataset.Clear();
        //        MyAdapter.Fill(MyDataset);
        //        string postData = "";
        //        string URL = serviceURL + "RABill_AbstractSync"; //string URL = "http://localhost:50385/api/DbSync/RABill_AbstractSync";
        //        var data = "";


        //        Console.WriteLine("Started Synching for RABill_Abstract Add : ");
        //        if (MyDataset.Tables[0].Rows.Count == 0)
        //        {
        //            Console.WriteLine("No Records found for RABill_Abstract Add : ");
        //        }
        //        foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
        //        {
        //            try
        //            {

        //                postData = "RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&RABillNumber=" + dr["RABillNumber"] + "&RABill_Date=" + dr["RABill_Date"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString(); 

        //                data = webPostMethod(postData, URL);
        //                if (data.ToString().Contains("true"))
        //                {
        //                    dbutility.updateAddFlag(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract", "RABillUid");
        //                    dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Add", "Success", "");
        //                    Console.WriteLine("Synching for RABill_Abstract Add : " + dr["RABillUid"].ToString() + " Done");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Add", "Error", ex.Message);
        //            }
        //        }
        //        //
        //        MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABill_Abstract where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
        //        //
        //        //if (MyConnection.State == System.Data.ConnectionState.Closed)
        //        //{
        //        //    MyConnection.Open();
        //        //}
        //        MyDataset.Clear();
        //        MyAdapter.Fill(MyDataset);
        //        Console.WriteLine("Started Synching for RABill_Abstract Update : ");
        //        if (MyDataset.Tables[0].Rows.Count == 0)
        //        {
        //            Console.WriteLine("No Records found for RABill_Abstract Update : ");
        //        }
        //        foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
        //        {
        //            try
        //            {
        //                postData = "RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&RABillNumber=" + dr["RABillNumber"] + "&RABill_Date=" + dr["RABill_Date"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString();


        //                data = webPostMethod(postData, URL);
        //                if (data.ToString().Contains("true"))
        //                {
        //                    dbutility.updateUpdateFlag(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract", "RABillUid");
        //                    dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Update", "Success", "");
        //                    Console.WriteLine("Synching for RABill_Abstract Update : " + dr["RABillUid"].ToString() + " Done");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Update", "Error", ex.Message);
        //            }
        //        }
        //        Console.WriteLine("Synching for RABill_Abstract Add Done : ");
        //      //  Console.ReadLine();
        //    }
        //    catch (Exception ex)
        //    {
        //        dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "RABill_Abstract Add/Update", "Error", ex.Message);
        //        Console.WriteLine(ex.Message);
        //     //   Console.ReadLine();

        //    }
        //}

        // Edited by Nikhil on 26-08-2022 for RABill_Amount
        private void SynchRABill_Abstract()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABill_Abstract where ServerCopiedAdd='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "RABill_AbstractSync"; //string URL = "http://localhost:50385/api/DbSync/RABill_AbstractSync";
                var data = "";


                Console.WriteLine("Started Synching for RABill_Abstract Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABill_Abstract Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&RABillNumber=" + dr["RABillNumber"] + "&RABill_Date=" + dr["RABill_Date"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString() + "&RABill_Amount=" + dr["RABill_Amount"].ToString()
                            + "&RABill_SubmissionAmount=" + dr["RABill_SubmissionAmount"].ToString() + "&RABill_SubmissionDate=" + dr["RABill_SubmissionDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract", "RABillUid");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Add", "Success", "");
                            Console.WriteLine("Synching for RABill_Abstract Add : " + dr["RABillUid"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABill_Abstract where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for RABill_Abstract Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABill_Abstract Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //postData = "RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&RABillNumber=" + dr["RABillNumber"] + "&RABill_Date=" + dr["RABill_Date"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString() + "&RABill_Amount=" + dr["RABill_Amount"].ToString();
                        postData = "RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&RABillNumber=" + dr["RABillNumber"] + "&RABill_Date=" + dr["RABill_Date"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString() + "&RABill_Amount=" + dr["RABill_Amount"].ToString()
                            + "&RABill_SubmissionAmount=" + dr["RABill_SubmissionAmount"].ToString() + "&RABill_SubmissionDate=" + dr["RABill_SubmissionDate"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract", "RABillUid");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Update", "Success", "");
                            Console.WriteLine("Synching for RABill_Abstract Update : " + dr["RABillUid"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["RABillUid"].ToString()), "RABill_Abstract Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for RABill_Abstract Add Done : ");
                //  Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "RABill_Abstract Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //   Console.ReadLine();

            }
        }

        //added on 29/08/2022 for nikhil
        private void SynchRABills_Documents()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABills_Documents where ServerCopiedAdd='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "RABills_DocumentsSync"; //string URL = "http://localhost:50385/api/DbSync/RABill_AbstractSync";
                var data = "";


                Console.WriteLine("Started Synching for RABills_Documents Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABills_Documents Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (uploaddoc(dr["FilePath"].ToString()))
                        {
                            postData = "DocumentUID=" + dr["DocumentUID"].ToString() + "&created_date=" + dr["created_date"].ToString() + "&RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&UserID=" + dr["UserID"].ToString() + "&FilePath=" + dr["FilePath"].ToString() + "&Description=" + dr["Description"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString();

                            data = webPostMethod(postData, URL);
                            if (data.ToString().Contains("true"))
                            {
                                dbutility.updateAddFlag(new Guid(dr["DocumentUID"].ToString()), "RABills_Documents", "DocumentUID");
                                dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "RABills_Documents Add", "Success", "");
                                Console.WriteLine("Synching for RABills_Documents Add : " + dr["DocumentUID"].ToString() + " Done");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "RABills_Documents Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABills_Documents where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for RABills_Documents Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABills_Documents Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DocumentUID=" + dr["DocumentUID"].ToString() + "&created_date=" + dr["created_date"].ToString() + "&RABillUid=" + dr["RABillUid"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&UserID=" + dr["UserID"].ToString() + "&FilePath=" + dr["FilePath"].ToString() + "&Description=" + dr["Description"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DocumentUID"].ToString()), "RABills_Documents", "DocumentUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "RABills_Documents Update", "Success", "");
                            Console.WriteLine("Synching for RABills_Documents Update : " + dr["DocumentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DocumentUID"].ToString()), "RABills_Documents Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for RABills_Documents Add Done : ");
                //  Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "RABills_Documents Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //   Console.ReadLine();

            }
        }

        private void SynchRABillsSync()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABills where ServerCopiedAdd='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "RABillsSync"; //string URL = "http://localhost:50385/api/DbSync/RABillsSync";
                var data = "";


                Console.WriteLine("Started Synching for RABills Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABills Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "itemUId=" + dr["itemUId"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"] + "&item_number=" + dr["item_number"].ToString() + "&item_desc=" + dr["item_desc"] + "&current_cost=" + dr["current_cost"].ToString() + "&cumulative_cost=" + dr["cumulative_cost"] + "&created_date=" + dr["created_date"].ToString() + "&RABillUid=" + dr["RABillUid"] + "&UID=" + dr["UID"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "RABills", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "RABills Add", "Success", "");
                            Console.WriteLine("Synching for RABills Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "RABills Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABills where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for RABills Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABills Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "itemUId=" + dr["itemUId"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"] + "&item_number=" + dr["item_number"].ToString() + "&item_desc=" + dr["item_desc"] + "&current_cost=" + dr["current_cost"].ToString() + "&cumulative_cost=" + dr["cumulative_cost"] + "&created_date=" + dr["created_date"].ToString() + "&RABillUid=" + dr["RABillUid"] + "&UID=" + dr["UID"].ToString() + "&DeleteFlag=" + dr["DeleteFlag"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "RABills", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "RABills Update", "Success", "");
                            Console.WriteLine("Synching for RABills Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "RABills Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for RABills Add Done : ");
               // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "RABills Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
              //  Console.ReadLine();

            }
        }

        private void SynchAssignJointInspectiontoRAbill()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From AssignJointInspectiontoRAbill where ServerCopiedAdd='N' and RABill_UID in (select RABillUid FRom [dbo].[RABill_Abstract] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                 //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "AssignJointInspectiontoRAbillSync"; //string URL = "http://localhost:50385/api/DbSync/RABillsSync";
                var data = "";


                Console.WriteLine("Started Synching for AssignJointInspectiontoRAbill Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for AssignJointInspectiontoRAbill Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "AssignJointInspectionUID=" + dr["AssignJointInspectionUID"].ToString() + "&RABill_UID=" + dr["RABill_UID"].ToString() + "&RABill_ItemUID=" + dr["RABill_ItemUID"] + "&InspectionUID=" + dr["InspectionUID"].ToString() + "&Assign_Date=" + dr["Assign_Date"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["AssignJointInspectionUID"].ToString()), "AssignJointInspectiontoRAbill", "AssignJointInspectionUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignJointInspectionUID"].ToString()), "AssignJointInspectiontoRAbill Add", "Success", "");
                            Console.WriteLine("Synching for AssignJointInspectiontoRAbill Add : " + dr["AssignJointInspectionUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "AssignJointInspectiontoRAbill Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From AssignJointInspectiontoRAbill where ServerCopiedUpdate='N' and RABill_UID in (select RABillUid FRom [dbo].[RABill_Abstract] where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for AssignJointInspectiontoRAbill Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for AssignJointInspectiontoRAbill Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "AssignJointInspectionUID=" + dr["AssignJointInspectionUID"].ToString() + "&RABill_UID=" + dr["RABill_UID"].ToString() + "&RABill_ItemUID=" + dr["RABill_ItemUID"] + "&InspectionUID=" + dr["InspectionUID"].ToString() + "&Assign_Date=" + dr["Assign_Date"];//


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["AssignJointInspectionUID"].ToString()), "AssignJointInspectiontoRAbill", "AssignJointInspectionUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignJointInspectionUID"].ToString()), "RABills Update", "Success", "");
                            Console.WriteLine("Synching for AssignJointInspectiontoRAbill Update : " + dr["AssignJointInspectionUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignJointInspectionUID"].ToString()), "AssignJointInspectiontoRAbill Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for AssignJointInspectiontoRAbill Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "AssignJointInspectiontoRAbill Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //  Console.ReadLine();

            }
        }

        private void SynchDeductionsMaster()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DeductionsMaster where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DeductionsMasterSync"; //string URL = "http://localhost:50385/api/DbSync/DeductionsMasterSync";
                var data = "";


                Console.WriteLine("Started Synching for DeductionsMaster Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DeductionsMaster Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&DeductionsDescription=" + dr["DeductionsDescription"].ToString() + "&Maxpercentage=" + dr["Maxpercentage"] + "&Order_By=" + dr["Order_By"].ToString();//;

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "DeductionsMaster", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DeductionsMaster Add", "Success", "");
                            Console.WriteLine("Synching for DeductionsMaster Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DeductionsMaster Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DeductionsMaster where ServerCopiedUpdate='N'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for DeductionsMaster Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DeductionsMaster Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DeductionsDescription=" + dr["DeductionsDescription"].ToString() + "&Maxpercentage=" + dr["Maxpercentage"] + "&Order_By=" + dr["Order_By"].ToString();//;


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "DeductionsMaster", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DeductionsMaster Update", "Success", "");
                            Console.WriteLine("Synching for DeductionsMaster Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DeductionsMaster Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for DeductionsMaster Add Done : ");
               // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "DeductionsMaster Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
               // Console.ReadLine();

            }
        }

        private void SynchInvoiceMaster()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From InvoiceMaster where ServerCopiedAdd='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "InvoiceMasterSync";// string URL = "http://localhost:50385/api/DbSync/InvoiceMasterSync";
                var data = "";


                Console.WriteLine("Started Synching for InvoiceMaster Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for InvoiceMaster Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "InvoiceMaster_UID=" + dr["InvoiceMaster_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"] + "&Invoice_Number=" + dr["Invoice_Number"].ToString() + "&Invoice_Desc=" + dr["Invoice_Desc"] + "&Invoice_Date=" + dr["Invoice_Date"].ToString() + "&Invoice_TotalAmount=" + dr["Invoice_TotalAmount"] + "&Invoice_DeductionAmount=" + dr["Invoice_DeductionAmount"].ToString() + "&Invoice_NetAmount=" + dr["Invoice_NetAmount"] + "&Currency=" + dr["Currency"].ToString() + "&Currency_CultureInfo=" + dr["Currency_CultureInfo"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["InvoiceMaster_UID"].ToString()), "InvoiceMaster", "InvoiceMaster_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceMaster_UID"].ToString()), "InvoiceMaster Add", "Success", "");
                            Console.WriteLine("Synching for InvoiceMaster Add : " + dr["InvoiceMaster_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceMaster_UID"].ToString()), "InvoiceMaster Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From InvoiceMaster where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for InvoiceMaster Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for InvoiceMaster Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "InvoiceMaster_UID=" + dr["InvoiceMaster_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"] + "&Invoice_Number=" + dr["Invoice_Number"].ToString() + "&Invoice_Desc=" + dr["Invoice_Desc"] + "&Invoice_Date=" + dr["Invoice_Date"].ToString() + "&Invoice_TotalAmount=" + dr["Invoice_TotalAmount"] + "&Invoice_DeductionAmount=" + dr["Invoice_DeductionAmount"].ToString() + "&Invoice_NetAmount=" + dr["Invoice_NetAmount"] + "&Currency=" + dr["Currency"].ToString() + "&Currency_CultureInfo=" + dr["Currency_CultureInfo"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["InvoiceMaster_UID"].ToString()), "InvoiceMaster", "InvoiceMaster_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceMaster_UID"].ToString()), "InvoiceMaster Update", "Success", "");
                            Console.WriteLine("Synching for InvoiceMaster Update : " + dr["InvoiceMaster_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceMaster_UID"].ToString()), "InvoiceMaster Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for InvoiceMaster Add Done : ");
              //  Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "InvoiceMaster Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
              //  Console.ReadLine();

            }
        }

        private void SynchInvoiceRABills()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Invoice_RABills where ServerCopiedAdd='N' and InvoiceMaster_UID in (select InvoiceMaster_UID FRom InvoiceMaster where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "InvoiceRABillsSync"; // string URL = "http://localhost:50385/api/DbSync/InvoiceRABillsSync";
                var data = "";


                Console.WriteLine("Started Synching for Invoice_RABills Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Invoice_RABills Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "InvoiceRABill_UID=" + dr["InvoiceRABill_UID"].ToString() + "&InvoiceMaster_UID=" + dr["InvoiceMaster_UID"].ToString() + "&RABillUid=" + dr["RABillUid"] + "&InvoiceRABill_Date=" + dr["InvoiceRABill_Date"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["InvoiceRABill_UID"].ToString()), "Invoice_RABills", "InvoiceRABill_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceRABill_UID"].ToString()), "Invoice_RABills Add", "Success", "");
                            Console.WriteLine("Synching for InvoiceMaster Add : " + dr["InvoiceRABill_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceRABill_UID"].ToString()), "Invoice_RABills Add", "Error", ex.Message);
                    }
                }
                //
                // MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From InvoiceMaster where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Invoice_RABills where ServerCopiedUpdate='N' and InvoiceMaster_UID in (select InvoiceMaster_UID FRom InvoiceMaster where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);

                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Invoice_RABills Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Invoice_RABills Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "InvoiceRABill_UID=" + dr["InvoiceRABill_UID"].ToString() + "&InvoiceMaster_UID=" + dr["InvoiceMaster_UID"].ToString() + "&RABillUid=" + dr["RABillUid"] + "&InvoiceRABill_Date=" + dr["InvoiceRABill_Date"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"];


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["InvoiceRABill_UID"].ToString()), "Invoice_RABills", "InvoiceRABill_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceRABill_UID"].ToString()), "Invoice_RABills Update", "Success", "");
                            Console.WriteLine("Synching for Invoice_RABills Update : " + dr["InvoiceRABill_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["InvoiceRABill_UID"].ToString()), "Invoice_RABills Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Invoice_RABills Add Done : ");
              //  Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Invoice_RABills Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
               // Console.ReadLine();

            }
        }

        private void SynchInvoiceDeduction()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Invoice_Deduction where ServerCopiedAdd='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "InvoiceDeductionSync"; ;// string URL = "http://localhost:50385/api/DbSync/InvoiceDeductionSync";
                var data = "";


                Console.WriteLine("Started Synching for Invoice_Deduction Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Invoice_Deduction Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "Invoice_DeductionUID=" + dr["Invoice_DeductionUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&InvoiceMaster_UID=" + dr["InvoiceMaster_UID"] + "&Deduction_UID=" + dr["Deduction_UID"].ToString() + "&Amount=" + dr["Amount"] + "&Currency=" + dr["Currency"].ToString() + "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] + "&Percentage=" + dr["Percentage"].ToString() + "&Deduction_Mode=" + dr["Deduction_Mode"] + "&Order_By=" + dr["Order_By"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["Invoice_DeductionUID"].ToString()), "Invoice_Deduction", "Invoice_DeductionUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Invoice_DeductionUID"].ToString()), "Invoice_Deduction Add", "Success", "");
                            Console.WriteLine("Synching for Invoice_Deduction Add : " + dr["Invoice_DeductionUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Invoice_DeductionUID"].ToString()), "Invoice_Deduction Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Invoice_Deduction where ServerCopiedUpdate='N' and WorkpackageUID ='" + wkPkgUID + "'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Invoice_Deduction Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Invoice_Deduction Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "Invoice_DeductionUID=" + dr["Invoice_DeductionUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&InvoiceMaster_UID=" + dr["InvoiceMaster_UID"] + "&Deduction_UID=" + dr["Deduction_UID"].ToString() + "&Amount=" + dr["Amount"] + "&Currency=" + dr["Currency"].ToString() + "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] + "&Percentage=" + dr["Percentage"].ToString() + "&Deduction_Mode=" + dr["Deduction_Mode"] + "&Order_By=" + dr["Order_By"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();
                        
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["Invoice_DeductionUID"].ToString()), "Invoice_Deduction", "Invoice_DeductionUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Invoice_DeductionUID"].ToString()), "Invoice_Deduction Update", "Success", "");
                            Console.WriteLine("Synching for Invoice_Deduction Update : " + dr["Invoice_DeductionUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Invoice_DeductionUID"].ToString()), "Invoice_Deduction Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Invoice_Deduction Add Done : ");
               // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Invoice_Deduction Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }


        private void SyncUserDetails()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserDetails where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "UserDetailsSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for UserDetails Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserDetails Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["Profile_Pic"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Profile_Pic"].ToString()))
                            {
                                uploaddoc(dr["Profile_Pic"].ToString());
                            }
                        }

                        postData = "UserUID=" + dr["UserUID"].ToString() + "&FirstName=" + dr["FirstName"].ToString() + "&LastName=" + dr["LastName"] + "&EmailID=" + dr["EmailID"].ToString() + "&Phonenumber=" + dr["Phonenumber"] + "&Mobilenumber=" + dr["Mobilenumber"].ToString() + "&Address1=" + dr["Address1"] + "&Address2=" + dr["Address2"].ToString() + "&Username=" + dr["Username"] + "&password=" + dr["password"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString() + "&DeletedDate=" + dr["DeletedDate"].ToString() + "&TypeOfUser=" + dr["TypeOfUser"].ToString() + "&Admin_Under=" + dr["Admin_Under"].ToString() + "&Project_Under=" + dr["Project_Under"].ToString() + "&Profile_Pic=" + dr["Profile_Pic"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UserUID"].ToString()), "UserDetails", "UserUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UserUID"].ToString()), "UserDetails Add", "Success", "");
                            Console.WriteLine("Synching for UserDetails Add : " + dr["UserUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UserUID"].ToString()), "UserDetails Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserDetails where ServerCopiedUpdate='N'", MyConnection);
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UserDetails Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserDetails Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["Profile_Pic"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Profile_Pic"].ToString()))
                            {
                                uploaddoc(dr["Profile_Pic"].ToString());
                            }
                        }
                        postData = "UserUID=" + dr["UserUID"].ToString() + "&FirstName=" + dr["FirstName"].ToString() + "&LastName=" + dr["LastName"] + "&EmailID=" + dr["EmailID"].ToString() + "&Phonenumber=" + dr["Phonenumber"] + "&Mobilenumber=" + dr["Mobilenumber"].ToString() + "&Address1=" + dr["Address1"] + "&Address2=" + dr["Address2"].ToString() + "&Username=" + dr["Username"] + "&password=" + dr["password"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString() + "&DeletedDate=" + dr["DeletedDate"].ToString() + "&TypeOfUser=" + dr["TypeOfUser"].ToString() + "&Admin_Under=" + dr["Admin_Under"].ToString() + "&Project_Under=" + dr["Project_Under"].ToString() + "&Profile_Pic=" + dr["Profile_Pic"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UserUID"].ToString()), "UserDetails", "UserUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UserUID"].ToString()), "UserDetails Update", "Success", "");
                            Console.WriteLine("Synching for UserDetails Update : " + dr["UserUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UserUID"].ToString()), "UserDetails Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UserDetails Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UserDetails Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncUserProjects()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserProjects where ServerCopiedAdd='N' and ProjectUID='" + PrjUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "UserProjectsSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for UserProjects Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserProjects Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "AssignID=" + dr["AssignID"].ToString() + "&UserUID=" + dr["UserUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] + "&UserRole=" + dr["UserRole"].ToString() + "&AssignDate=" + dr["AssignDate"] + "&Delete_Flag=" + dr["Delete_Flag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["AssignID"].ToString()), "UserProjects", "AssignID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignID"].ToString()), "UserProjects Add", "Success", "");
                            Console.WriteLine("Synching for UserProjects Add : " + dr["AssignID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignID"].ToString()), "UserProjects Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserProjects where ServerCopiedUpdate='N' and ProjectUID='" + PrjUID + "'", MyConnection);

              
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UserProjects Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserProjects Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "AssignID=" + dr["AssignID"].ToString() + "&UserUID=" + dr["UserUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] + "&UserRole=" + dr["UserRole"].ToString() + "&AssignDate=" + dr["AssignDate"] + "&Delete_Flag=" + dr["Delete_Flag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["AssignID"].ToString()), "UserProjects", "AssignID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignID"].ToString()), "UserProjects Update", "Success", "");
                            Console.WriteLine("Synching for UserProjects Update : " + dr["AssignID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["AssignID"].ToString()), "UserProjects Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UserProjects Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UserProjects Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncUserWorkPackages()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserWorkPackages where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "UserWorkPackagesSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for UserWorkPackages Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserWorkPackages Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&UserUID=" + dr["UserUID"] + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&Status=" + dr["Status"] + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&UpdatedDate=" + dr["UpdatedDate"].ToString() + "&Activity_Type=" + dr["Activity_Type"] + "&Activity_Id=" + dr["Activity_Id"] + "&UserRole_ID=" + dr["UserRole_ID"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "UserWorkPackages", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserWorkPackages Add", "Success", "");
                            Console.WriteLine("Synching for UserWorkPackages Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserWorkPackages Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserWorkPackages where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

             
                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UserWorkPackages Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserWorkPackages Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&UserUID=" + dr["UserUID"] + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&Status=" + dr["Status"] + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&UpdatedDate=" + dr["UpdatedDate"].ToString() + "&Activity_Type=" + dr["Activity_Type"] + "&Activity_Id=" + dr["Activity_Id"] + "&UserRole_ID=" + dr["UserRole_ID"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "UserWorkPackages", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserWorkPackages Update", "Success", "");
                            Console.WriteLine("Synching for UserWorkPackages Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserWorkPackages Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UserWorkPackages Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UserWorkPackages Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncUserRolesMaster()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserRolesMaster where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "UserRolesMasterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for UserRolesMaster Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserRolesMaster Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UserRole_ID=" + dr["UserRole_ID"].ToString() + "&UserRole_Desc=" + dr["UserRole_Desc"].ToString() + "&UserRole_Name=" + dr["UserRole_Name"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UserRole_ID"].ToString()), "UserRolesMaster", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UserRole_ID"].ToString()), "UserRolesMaster Add", "Success", "");
                            Console.WriteLine("Synching for UserRolesMaster Add : " + dr["UserRole_ID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UserRole_ID"].ToString()), "UserRolesMaster Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserRolesMaster where ServerCopiedUpdate='N'", MyConnection);


                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UserRolesMaster Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserRolesMaster Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UserRole_ID=" + dr["UserRole_ID"].ToString() + "&UserRole_Desc=" + dr["UserRole_Desc"].ToString() + "&UserRole_Name=" + dr["UserRole_Name"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UserRole_ID"].ToString()), "UserRolesMaster", "UserRole_ID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UserRole_ID"].ToString()), "UserRolesMaster Update", "Success", "");
                            Console.WriteLine("Synching for UserRolesMaster Update : " + dr["UserRole_ID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UserRole_ID"].ToString()), "UserRolesMaster Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UserRolesMaster Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UserRolesMaster Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncUserType_Functionality_Master()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserType_Functionality_Master where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "UserType_Functionality_MasterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for UserType_Functionality_Master Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserType_Functionality_Master Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&Functionality=" + dr["Functionality"].ToString() + "&Code=" + dr["Code"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "UserType_Functionality_Master", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Master Add", "Success", "");
                            Console.WriteLine("Synching for UserType_Functionality_Master Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Master Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserType_Functionality_Master where ServerCopiedUpdate='N'", MyConnection);


                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UserType_Functionality_Master Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserType_Functionality_Master Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&Functionality=" + dr["Functionality"].ToString() + "&Code=" + dr["Code"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "UserType_Functionality_Master", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Master Update", "Success", "");
                            Console.WriteLine("Synching for UserType_Functionality_Master Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Master Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UserType_Functionality_Master Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UserType_Functionality_Master Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncUserType_Functionality_Mapping()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserType_Functionality_Mapping where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "UserType_Functionality_MappingSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for UserType_Functionality_Mapping Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserType_Functionality_Mapping Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {

                        postData = "UID=" + dr["UID"].ToString() + "&UserType=" + dr["UserType"].ToString() + "&FunctionalityUID=" + dr["FunctionalityUID"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "UserType_Functionality_Mapping", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Mapping Add", "Success", "");
                            Console.WriteLine("Synching for UserType_Functionality_Mapping Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Mapping Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UserType_Functionality_Mapping where ServerCopiedUpdate='N'", MyConnection);


                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UserType_Functionality_Mapping Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UserType_Functionality_Mapping Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&UserType=" + dr["UserType"].ToString() + "&FunctionalityUID=" + dr["FunctionalityUID"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "UserType_Functionality_Mapping", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Mapping Update", "Success", "");
                            Console.WriteLine("Synching for UserType_Functionality_Mapping Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "UserType_Functionality_Mapping Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UserType_Functionality_Mapping Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UserType_Functionality_Mapping Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }


        private void SyncIssues()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Issues where ServerCopiedAdd='N' and WorkPackagesUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "IssuesSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for Issues Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Issues Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["Issue_Document"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Issue_Document"].ToString()))
                            {
                                uploaddoc(dr["Issue_Document"].ToString());
                            }
                        }

                        postData = "Issue_Uid=" + dr["Issue_Uid"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkPackagesUID=" + dr["WorkPackagesUID"] + "&TaskUID=" + dr["TaskUID"].ToString() + "&Issue_Description=" + dr["Issue_Description"] + "&Issue_Date=" + dr["Issue_Date"].ToString() + "&Issued_User=" + dr["Issued_User"] + "&Assigned_User=" + dr["Assigned_User"].ToString() + "&Assigned_Date=" + dr["Assigned_Date"] + "&Issue_ProposedCloser_Date=" + dr["Issue_ProposedCloser_Date"].ToString() + "&Approving_User=" + dr["Approving_User"] + "&Actual_Closer_Date=" + dr["Actual_Closer_Date"].ToString() + "&Issue_Status=" + dr["Issue_Status"] + "&Issue_Remarks=" + dr["Issue_Remarks"].ToString() + "&Issue_Document=" + dr["Issue_Document"] + "&Delete_Flag=" + dr["Delete_Flag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["Issue_Uid"].ToString()), "Issues", "Issue_Uid");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "Issues Add", "Success", "");
                            Console.WriteLine("Synching for Issues Add : " + dr["Issue_Uid"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "Issues Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Issues where ServerCopiedUpdate='N' and WorkPackagesUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Issues Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Issues Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["Issue_Document"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Issue_Document"].ToString()))
                            {
                                uploaddoc(dr["Issue_Document"].ToString());
                            }
                        }

                        postData = "Issue_Uid=" + dr["Issue_Uid"].ToString() + "&ProjectUID=" + dr["ProjectUID"].ToString() + "&WorkPackagesUID=" + dr["WorkPackagesUID"] + "&TaskUID=" + dr["TaskUID"].ToString() + "&Issue_Description=" + dr["Issue_Description"] + "&Issue_Date=" + dr["Issue_Date"].ToString() + "&Issued_User=" + dr["Issued_User"] + "&Assigned_User=" + dr["Assigned_User"].ToString() + "&Assigned_Date=" + dr["Assigned_Date"] + "&Issue_ProposedCloser_Date=" + dr["Issue_ProposedCloser_Date"].ToString() + "&Approving_User=" + dr["Approving_User"] + "&Actual_Closer_Date=" + dr["Actual_Closer_Date"].ToString() + "&Issue_Status=" + dr["Issue_Status"] + "&Issue_Remarks=" + dr["Issue_Remarks"].ToString() + "&Issue_Document=" + dr["Issue_Document"] + "&Delete_Flag=" + dr["Delete_Flag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["Issue_Uid"].ToString()), "Issues", "Issue_Uid");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "Issues Update", "Success", "");
                            Console.WriteLine("Synching for Issues Update : " + dr["Issue_Uid"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "Issues Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Issues Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Issues Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncIssueDocs()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UploadedIssueDocs Where ServerCopiedAdd='N' and Issue_Uid in (Select Issue_Uid From Issues where WorkPackagesUID ='" + wkPkgUID + "')", MyConnection);

                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "IssueDocsMain"; ;
                var data = "";


                Console.WriteLine("Started Synching for IssueDocsMain Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for IssueDocsMain Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["doc_path"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["doc_path"].ToString()))
                            {
                                uploaddoc("/" + dr["doc_path"].ToString()  + dr["doc_name"].ToString());
                            }
                        }

                        postData = "doc_id=" + dr["doc_id"].ToString() + "&doc_name=" + dr["doc_name"].ToString() + "&doc_path=" + dr["doc_path"] + "&Issue_Uid=" + dr["Issue_Uid"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlagInt(int.Parse(dr["doc_id"].ToString()), "UploadedIssueDocs", "doc_id");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "UploadedIssueDocs Add", "Success", "");
                            Console.WriteLine("Synching for UploadedIssueDocs Add : " + dr["doc_id"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "UploadedIssueDocs Add", "Error", ex.Message);
                    }
                }
                //
              //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UploadedDocs where ServerCopiedUpdate='N'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UploadedIssueDocs Where ServerCopiedUpdate='N' and Issue_Uid in (Select Issue_Uid From Issues where WorkPackagesUID ='" + wkPkgUID + "')", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for UploadedIssueDocs Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for UploadedIssueDocs Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "doc_id=" + dr["doc_id"].ToString() + "&doc_name=" + dr["doc_name"].ToString() + "&doc_path=" + dr["doc_path"] + "&Issue_Uid=" + dr["Issue_Uid"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlagInt(int.Parse(dr["doc_id"].ToString()), "UploadedIssueDocs", "doc_id");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "UploadedIssueDocs Update", "Success", "");
                            Console.WriteLine("Synching for UploadedIssueDocs Update : " + dr["doc_id"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Issue_Uid"].ToString()), "UploadedIssueDocs Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for UploadedIssueDocs Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "UploadedIssueDocs Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncIssueRemarks()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From IssueRemarks where ServerCopiedAdd='N' and Issue_Uid in (select Issue_Uid FRom Issues where WorkPackagesUID ='" + wkPkgUID + "')", MyConnection);

                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "IssueRemarksSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for IssueRemarks Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for IssueRemarks Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["Issue_Document"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Issue_Document"].ToString()))
                            {
                                uploaddoc(dr["Issue_Document"].ToString());
                            }
                        }

                        postData = "IssueRemarksUID=" + dr["IssueRemarksUID"].ToString() + "&Issue_Uid=" + dr["Issue_Uid"].ToString() + "&Issue_Status=" + dr["Issue_Status"] + "&Issue_Remarks=" + dr["Issue_Remarks"].ToString() + "&Issue_Document=" + dr["Issue_Document"] + "&IssueRemark_Date=" + dr["IssueRemark_Date"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"];// + "&Assigned_User=" + dr["Assigned_User"].ToString() + "&Assigned_Date=" + dr["Assigned_Date"] + "&Issue_ProposedCloser_Date=" + dr["Issue_ProposedCloser_Date"].ToString() + "&Approving_User=" + dr["Approving_User"] + "&Actual_Closer_Date=" + dr["Actual_Closer_Date"].ToString() + "&Issue_Status=" + dr["Issue_Status"] + "&Issue_Remarks=" + dr["Issue_Remarks"].ToString() + "&Issue_Document=" + dr["Issue_Document"] + "&Delete_Flag=" + dr["Delete_Flag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["IssueRemarksUID"].ToString()), "IssueRemarks", "IssueRemarksUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["IssueRemarksUID"].ToString()), "IssueRemarks Add", "Success", "");
                            Console.WriteLine("Synching for IssueRemarks Add : " + dr["IssueRemarksUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["IssueRemarksUID"].ToString()), "IssueRemarks Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From IssueRemarks where ServerCopiedUpdate='N' and Issue_Uid in (select Issue_Uid FRom Issues where WorkPackagesUID ='" + wkPkgUID + "')", MyConnection);

          
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for IssueRemarks Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for IssueRemarks Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["Issue_Document"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["Issue_Document"].ToString()))
                            {
                                uploaddoc(dr["Issue_Document"].ToString());
                            }
                        }

                        postData = "IssueRemarksUID=" + dr["IssueRemarksUID"].ToString() + "&Issue_Uid=" + dr["Issue_Uid"].ToString() + "&Issue_Status=" + dr["Issue_Status"] + "&Issue_Remarks=" + dr["Issue_Remarks"].ToString() + "&Issue_Document=" + dr["Issue_Document"] + "&IssueRemark_Date=" + dr["IssueRemark_Date"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"];// + "&Assigned_User=" + dr["Assigned_User"].ToString() + "&Assigned_Date=" + dr["Assigned_Date"] + "&Issue_ProposedCloser_Date=" + dr["Issue_ProposedCloser_Date"].ToString() + "&Approving_User=" + dr["Approving_User"] + "&Actual_Closer_Date=" + dr["Actual_Closer_Date"].ToString() + "&Issue_Status=" + dr["Issue_Status"] + "&Issue_Remarks=" + dr["Issue_Remarks"].ToString() + "&Issue_Document=" + dr["Issue_Document"] + "&Delete_Flag=" + dr["Delete_Flag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["IssueRemarksUID"].ToString()), "IssueRemarks", "IssueRemarksUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["IssueRemarksUID"].ToString()), "IssueRemarks Update", "Success", "");
                            Console.WriteLine("Synching for IssueRemarks Update : " + dr["IssueRemarksUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["IssueRemarksUID"].ToString()), "IssueRemarks Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for IssueRemarks Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "IssueRemarks Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncIssueRemarksDocs()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UploadedDocs Where ServerCopiedAdd ='N' and issue_remarks_uid in(select issue_remarks_uid From IssueRemarks Where Issue_Uid in (select Issue_Uid From Issues Where WorkPackagesUID='" + wkPkgUID + "'))", MyConnection);

                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "IssueRemarksDocs"; ;
                var data = "";


                Console.WriteLine("Started Synching for IssueRemarksDocs Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for IssueRemarksDocs Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        if (dr["doc_path"] != System.DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(dr["doc_path"].ToString()))
                            {
                                uploaddoc("/" + dr["doc_path"].ToString() + dr["doc_name"].ToString());
                            }
                        }

                        postData = "uploaded_doc_id=" + dr["uploaded_doc_id"].ToString() + "&doc_name=" + dr["doc_name"].ToString() + "&doc_path=" + dr["doc_path"] + "&issue_remarks_uid=" + dr["issue_remarks_uid"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlagInt(int.Parse(dr["uploaded_doc_id"].ToString()), "UploadedDocs", "uploaded_doc_id");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["issue_remarks_uid"].ToString()), "UploadedDocs Add", "Success", "");
                            Console.WriteLine("Synching for UploadedDocs Add : " + dr["uploaded_doc_id"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["issue_remarks_uid"].ToString()), "IssueRemarks UploadedDocs Add", "Error", ex.Message);
                    }
                }
                //
                //MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UploadedDocs where ServerCopiedUpdate='N'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From UploadedDocs Where ServerCopiedUpdate ='N' and issue_remarks_uid in(select issue_remarks_uid From IssueRemarks Where Issue_Uid in (select Issue_Uid From Issues Where WorkPackagesUID='" + wkPkgUID + "'))", MyConnection);


                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for IssueRemarksDocs Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for IssueRemarksDocs Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "uploaded_doc_id=" + dr["uploaded_doc_id"].ToString() + "&doc_name=" + dr["doc_name"].ToString() + "&doc_path=" + dr["doc_path"] + "&issue_remarks_uid=" + dr["issue_remarks_uid"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlagInt(int.Parse(dr["uploaded_doc_id"].ToString()), "UploadedDocs", "uploaded_doc_id");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["issue_remarks_uid"].ToString()), "UploadedDocs Update", "Success", "");
                            Console.WriteLine("Synching for UploadedDocs Update : " + dr["uploaded_doc_id"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["issue_remarks_uid"].ToString()), "IssueRemarks UploadedDocs Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for IssueRemarks UploadedDocs Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "IssueRemarks UploadedDocs Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SynchActivityDeleteLogs()
        {
            try
            {
                // source




                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ActivityDeleteLogs where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "ActivityDeleteLogsSync"; //string URL = "http://localhost:50385/api/DbSync/WorddocReadSync";
                var data = "";
                Console.WriteLine("Started Synching for ActivityDeleteLogs Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ActivityDeleteLogs Add : ");
                }

                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        postData = "DeleteLog_UID=" + dr["DeleteLog_UID"].ToString() + "&Activity_UID=" + dr["Activity_UID"].ToString() + "&Activity_UserUID=" + dr["Activity_UserUID"] + "&Activity_For=" + dr["Activity_For"].ToString() + "&Activity_Date=" + dr["Activity_Date"].ToString();//;


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DeleteLog_UID"].ToString()), "ActivityDeleteLogs", "DeleteLog_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DeleteLog_UID"].ToString()), "ActivityDeleteLogs Add", "Success", "");
                            Console.WriteLine("Synching for ActivityDeleteLogs Add : " + dr["DeleteLog_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DeleteLog_UID"].ToString()), "ActivityDeleteLogs Add", "Error", ex.Message);
                    }
                }
                //

                Console.WriteLine("Synching for ActivityDeleteLogs Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "ActivityDeleteLogs Add", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                // Console.ReadLine();

            }
        }


        private void SyncFinanceMileStones()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From FinanceMileStones where ServerCopiedAdd='N' and TaskUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "FinanceMileStonesSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for FinanceMileStones Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for FinanceMileStones Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "Finance_MileStoneUID=" + dr["Finance_MileStoneUID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&Finance_MileStoneName=" + dr["Finance_MileStoneName"] + "&Finance_MileStoneCreatedDate=" + dr["Finance_MileStoneCreatedDate"].ToString() + "&User_Created=" + dr["User_Created"] + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&IsMonth=" + dr["IsMonth"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["Finance_MileStoneUID"].ToString()), "FinanceMileStones", "Finance_MileStoneUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Finance_MileStoneUID"].ToString()), "FinanceMileStones Add", "Success", "");
                            Console.WriteLine("Synching for FinanceMileStones Add : " + dr["Finance_MileStoneUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Finance_MileStoneUID"].ToString()), "FinanceMileStones Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From FinanceMileStones where ServerCopiedUpdate='N' and TaskUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for FinanceMileStones Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for FinanceMileStones Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "Finance_MileStoneUID=" + dr["Finance_MileStoneUID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&Finance_MileStoneName=" + dr["Finance_MileStoneName"] + "&Finance_MileStoneCreatedDate=" + dr["Finance_MileStoneCreatedDate"].ToString() + "&User_Created=" + dr["User_Created"] + "&Delete_Flag=" + dr["Delete_Flag"].ToString() + "&IsMonth=" + dr["IsMonth"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["Finance_MileStoneUID"].ToString()), "FinanceMileStones", "Finance_MileStoneUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Finance_MileStoneUID"].ToString()), "FinanceMileStones Update", "Success", "");
                            Console.WriteLine("Synching for FinanceMileStones Update : " + dr["Finance_MileStoneUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Finance_MileStoneUID"].ToString()), "FinanceMileStones Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for FinanceMileStones Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "FinanceMileStones Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncFinanceMileStoneMonth()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From FinanceMileStoneMonth where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "FinanceMileStoneMonthSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for FinanceMileStoneMonthSync Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for FinanceMileStoneMonthSync Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {


                        postData = "FinMileStoneMonthUID=" + dr["FinMileStoneMonthUID"].ToString() + "&Finance_MileStoneUID=" + dr["Finance_MileStoneUID"].ToString() + "&AllowedPayment=" + dr["AllowedPayment"] + "&UserCreated=" + dr["UserCreated"].ToString() + "&CreatedDate=" + dr["CreatedDate"] + "&DeletedFlag=" + dr["DeletedFlag"].ToString() + "&Month=" + dr["Month"] + "&Year=" + dr["Year"].ToString() + "&MultiplyingFactor=" + dr["MultiplyingFactor"] + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&OrderBy=" + dr["OrderBy"] + "&DeletedBy=" + dr["DeletedBy"].ToString() + "&DeletedDate=" + dr["DeletedDate"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["FinMileStoneMonthUID"].ToString()), "FinanceMileStoneMonth", "FinMileStoneMonthUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["FinMileStoneMonthUID"].ToString()), "FinanceMileStoneMonth Add", "Success", "");
                            Console.WriteLine("Synching for FinanceMileStoneMonth Add : " + dr["FinMileStoneMonthUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["FinMileStoneMonthUID"].ToString()), "FinanceMileStoneMonth Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From FinanceMileStoneMonth where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for FinanceMileStoneMonth Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for FinanceMileStoneMonth Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "FinMileStoneMonthUID=" + dr["FinMileStoneMonthUID"].ToString() + "&Finance_MileStoneUID=" + dr["Finance_MileStoneUID"].ToString() + "&AllowedPayment=" + dr["AllowedPayment"] + "&UserCreated=" + dr["UserCreated"].ToString() + "&CreatedDate=" + dr["CreatedDate"] + "&DeletedFlag=" + dr["DeletedFlag"].ToString() + "&Month=" + dr["Month"] + "&Year=" + dr["Year"].ToString() + "&MultiplyingFactor=" + dr["MultiplyingFactor"] + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&OrderBy=" + dr["OrderBy"] + "&DeletedBy=" + dr["DeletedBy"].ToString() + "&DeletedDate=" + dr["DeletedDate"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["FinMileStoneMonthUID"].ToString()), "FinanceMileStoneMonth", "FinMileStoneMonthUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["FinMileStoneMonthUID"].ToString()), "FinanceMileStoneMonth Update", "Success", "");
                            Console.WriteLine("Synching for FinanceMileStoneMonth Update : " + dr["FinMileStoneMonthUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["FinMileStoneMonthUID"].ToString()), "FinanceMileStoneMonth Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for FinanceMileStoneMonth Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "FinanceMileStones Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SynchFinanceMileStoneMonth_EditedValues()
        {
            try
            {
             

                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From FinanceMileStoneMonth_EditedValues where ServerCopiedAdd='N' and FinMileStoneMonthUID in (Select FinMileStoneMonthUID From FinanceMileStoneMonth Where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "FinanceMileStoneMonth_EditedValuesSync"; //string URL = "http://localhost:50385/api/DbSync/WorddocReadSync";
                var data = "";
                Console.WriteLine("Started Synching for FinanceMileStoneMonth_EditedValues Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for FinanceMileStoneMonth_EditedValues Add : ");
                }

                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        //(object)dr["DocumentUID"] = System.DBNull.Value ? "NULL":"";
                        postData = "UID=" + dr["UID"].ToString() + "&FinMileStoneMonthUID=" + dr["FinMileStoneMonthUID"].ToString() + "&OldPaymentValue=" + dr["OldPaymentValue"] + "&NewPaymentValue=" + dr["NewPaymentValue"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&EditedBy=" + dr["EditedBy"].ToString();//;


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "FinanceMileStoneMonth_EditedValues", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "FinanceMileStoneMonth_EditedValues Add", "Success", "");
                            Console.WriteLine("Synching for FinanceMileStoneMonth_EditedValues Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "FinanceMileStoneMonth_EditedValues Add", "Error", ex.Message);
                    }
                }
                //

                Console.WriteLine("Synching for FinanceMileStoneMonth_EditedValues Add Done : ");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "FinanceMileStoneMonth_EditedValues Add", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                // Console.ReadLine();

            }
        }

        private void SyncTaskSchedule()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From TaskSchedule where ServerCopiedAdd='N' and WorkpacageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "TaskScheduleSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for TaskSchedule Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for TaskSchedule Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "TaskScheduleUID=" + dr["TaskScheduleUID"].ToString() + "&WorkpacageUID=" + dr["WorkpacageUID"].ToString() + "&TaskUID=" + dr["TaskUID"] + "&StartDate=" + dr["StartDate"].ToString() + "&EndDate=" + dr["EndDate"] + "&Schedule_Value=" + dr["Schedule_Value"].ToString() + "&Schedule_Type=" + dr["Schedule_Type"] + "&Created_Date=" + dr["Created_Date"].ToString() + "&TaskScheduleVersion=" + dr["TaskScheduleVersion"] + "&TaskSchedule_Approved=" + dr["TaskSchedule_Approved"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"] + "&Achieved_Value=" + dr["Achieved_Value"] + "&Achieved_Date=" + dr["Achieved_Date"].ToString() + "&Schedule_Per=" + dr["Schedule_Per"] + "&Achieved_Per=" + dr["Achieved_Per"] + "&revised_scheduled_value=" + dr["revised_scheduled_value"] + "&revised_acheivedvalue=" + dr["revised_acheivedvalue"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule", "TaskScheduleUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule Add", "Success", "");
                            Console.WriteLine("Synching for TaskSchedule Add : " + dr["TaskScheduleUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From TaskSchedule where ServerCopiedUpdate='N' and WorkpacageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for TaskSchedule Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for TaskSchedule Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "TaskScheduleUID=" + dr["TaskScheduleUID"].ToString() + "&WorkpacageUID=" + dr["WorkpacageUID"].ToString() + "&TaskUID=" + dr["TaskUID"] + "&StartDate=" + dr["StartDate"].ToString() + "&EndDate=" + dr["EndDate"] + "&Schedule_Value=" + dr["Schedule_Value"].ToString() + "&Schedule_Type=" + dr["Schedule_Type"] + "&Created_Date=" + dr["Created_Date"].ToString() + "&TaskScheduleVersion=" + dr["TaskScheduleVersion"] + "&TaskSchedule_Approved=" + dr["TaskSchedule_Approved"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"] + "&Achieved_Value=" + dr["Achieved_Value"] + "&Achieved_Date=" + dr["Achieved_Date"].ToString() + "&Schedule_Per=" + dr["Schedule_Per"] + "&Achieved_Per=" + dr["Achieved_Per"] + "&revised_scheduled_value=" + dr["revised_scheduled_value"] + "&revised_acheivedvalue=" + dr["revised_acheivedvalue"];//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule", "TaskScheduleUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule Update", "Success", "");
                            Console.WriteLine("Synching for TaskSchedule Update : " + dr["TaskScheduleUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "FinanceMileStoneMonth Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for TaskSchedule Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "TaskSchedule Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncTask()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "TaskSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for Tasks Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Tasks Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "TaskUID=" + dr["TaskUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&Workpackage_Option=" + dr["Workpackage_Option"].ToString() + "&Owner=" + dr["Owner"] + "&Task_Section=" + dr["Task_Section"].ToString() + "&Name=" + dr["Name"] + "&Description=" + dr["Description"].ToString() + "&RFPReference=" + dr["RFPReference"] + "&POReference=" + dr["POReference"].ToString() + "&StartDate=" + dr["StartDate"] +
                            "&PlannedEndDate=" + dr["PlannedEndDate"].ToString() + "&ProjectedEndDate=" + dr["ProjectedEndDate"] + "&Status=" + dr["Status"].ToString() + "&Currency=" + dr["Currency"] +
                            "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] +
                        "&Basic_Budget=" + dr["Basic_Budget"] +
                            "&GST=" + dr["GST"] +
                            "&Total_Budget=" + dr["Total_Budget"] +
                            "&ActualExpenditure=" + dr["ActualExpenditure"] +
                            "&RFPDocument=" + dr["RFPDocument"] +
                            "&NoOfDocuments=" + dr["NoOfDocuments"] +
                            "&TaskLevel=" + dr["TaskLevel"] +
                            "&ParentTaskID=" + dr["ParentTaskID"] +
                            "&UpdatedDate=" + dr["UpdatedDate"] +
                            "&StatusPer=" + dr["StatusPer"] +
                            "&UnitforProgress=" + dr["UnitforProgress"] +
                            "&UnitQuantity=" + dr["UnitQuantity"] +
                            "&PlannedStartDate=" + dr["PlannedStartDate"] +
                            "&ProjectedStartDate=" + dr["ProjectedStartDate"] +
                            "&ActualEndDate=" + dr["ActualEndDate"] +
                            "&Discipline=" + dr["Discipline"] +
                            "&MileStone=" + dr["MileStone"] +
                            "&Task_Weightage=" + dr["Task_Weightage"] +
                            "&Task_Type=" + dr["Task_Type"] +
                            "&Delete_Flag=" + dr["Delete_Flag"] +
                            "&Task_Order=" + dr["Task_Order"] +
                            "&BOQDetailsUID=" + dr["BOQDetailsUID"] +
                            "&GroupBOQItems=" + dr["GroupBOQItems"] +
                            "&Task_CulumativePercentage=" + dr["Task_CulumativePercentage"] +
                             "&CumulativeAchvQuantity=" + dr["CumulativeAchvQuantity"] +
                              "&InGraph=" + dr["InGraph"] +
                               "&Report1=" + dr["Report1"] +
                                "&Report2=" + dr["Report2"] +
                                 "&Report3=" + dr["Report3"] +
                                  "&Report4=" + dr["Report4"] +
                                   "&Report5=" + dr["Report5"] +
                        "&revised_unitquantity=" + dr["revised_unitquantity"] +
                        "&revised_cumulariveacheivedquantity=" + dr["revised_cumulariveacheivedquantity"] +
                        "&revised_weightage=" + dr["revised_weightage"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["TaskUID"].ToString()), "Tasks", "TaskUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "Tasks Add", "Success", "");
                            Console.WriteLine("Synching for Tasks Add : " + dr["TaskUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "TaskSchedule Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Tasks Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Tasks Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "TaskUID=" + dr["TaskUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&Workpackage_Option=" + dr["Workpackage_Option"].ToString() + "&Owner=" + dr["Owner"] + "&Task_Section=" + dr["Task_Section"].ToString() + "&Name=" + dr["Name"] + "&Description=" + dr["Description"].ToString() + "&RFPReference=" + dr["RFPReference"] + "&POReference=" + dr["POReference"].ToString() + "&StartDate=" + dr["StartDate"] +
                            "&PlannedEndDate=" + dr["PlannedEndDate"].ToString() + "&ProjectedEndDate=" + dr["ProjectedEndDate"] + "&Status=" + dr["Status"].ToString() + "&Currency=" + dr["Currency"] +
                            "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] +
                        "&Basic_Budget=" + dr["Basic_Budget"] +
                            "&GST=" + dr["GST"] +
                            "&Total_Budget=" + dr["Total_Budget"] +
                            "&ActualExpenditure=" + dr["ActualExpenditure"] +
                            "&RFPDocument=" + dr["RFPDocument"] +
                            "&NoOfDocuments=" + dr["NoOfDocuments"] +
                            "&TaskLevel=" + dr["TaskLevel"] +
                            "&ParentTaskID=" + dr["ParentTaskID"] +
                            "&UpdatedDate=" + dr["UpdatedDate"] +
                            "&StatusPer=" + dr["StatusPer"] +
                            "&UnitforProgress=" + dr["UnitforProgress"] +
                            "&UnitQuantity=" + dr["UnitQuantity"] +
                            "&PlannedStartDate=" + dr["PlannedStartDate"] +
                            "&ProjectedStartDate=" + dr["ProjectedStartDate"] +
                            "&ActualEndDate=" + dr["ActualEndDate"] +
                            "&Discipline=" + dr["Discipline"] +
                            "&MileStone=" + dr["MileStone"] +
                            "&Task_Weightage=" + dr["Task_Weightage"] +
                            "&Task_Type=" + dr["Task_Type"] +
                            "&Delete_Flag=" + dr["Delete_Flag"] +
                            "&Task_Order=" + dr["Task_Order"] +
                            "&BOQDetailsUID=" + dr["BOQDetailsUID"] +
                              "&GroupBOQItems=" + dr["GroupBOQItems"] +
                            "&Task_CulumativePercentage=" + dr["Task_CulumativePercentage"] +
                             "&CumulativeAchvQuantity=" + dr["CumulativeAchvQuantity"] +
                              "&InGraph=" + dr["InGraph"] +
                               "&Report1=" + dr["Report1"] +
                                "&Report2=" + dr["Report2"] +
                                 "&Report3=" + dr["Report3"] +
                                  "&Report4=" + dr["Report4"] +
                                   "&Report5=" + dr["Report5"] +
                        "&revised_unitquantity=" + dr["revised_unitquantity"] +
                        "&revised_cumulariveacheivedquantity=" + dr["revised_cumulariveacheivedquantity"] +
                        "&revised_weightage=" + dr["revised_weightage"];
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["TaskUID"].ToString()), "Tasks", "TaskUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "Tasks Update", "Success", "");
                            Console.WriteLine("Synching for Tasks Update : " + dr["TaskUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "Tasks Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Tasks Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Tasks Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }



        //private void SyncTaskSchedule()
        //{
        //    try
        //    {
        //        // source
        //        //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
        //        System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
        //        MyConnection.ConnectionString = GetSourceConnectionString();
        //        System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
        //        System.Data.DataSet MyDataset = new System.Data.DataSet();
        //        System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
        //        MyCommand.Connection = MyConnection;

        //        // check the submittal table for any records to be added or updated.....
        //        MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From TaskSchedule where ServerCopiedAdd='N' and WorkpacageUID='" + wkPkgUID + "'", MyConnection);
        //        //

        //        MyDataset.Clear();
        //        MyAdapter.Fill(MyDataset);
        //        string postData = "";
        //        string URL = serviceURL + "TaskScheduleSync"; ;
        //        var data = "";


        //        Console.WriteLine("Started Synching for TaskSchedule Add : ");
        //        if (MyDataset.Tables[0].Rows.Count == 0)
        //        {
        //            Console.WriteLine("No Records found for TaskSchedule Add : ");
        //        }
        //        foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
        //        {
        //            try
        //            {
        //                postData = "TaskScheduleUID=" + dr["TaskScheduleUID"].ToString() + "&WorkpacageUID=" + dr["WorkpacageUID"].ToString() + "&TaskUID=" + dr["TaskUID"] + "&StartDate=" + dr["StartDate"].ToString() + "&EndDate=" + dr["EndDate"] + "&Schedule_Value=" + dr["Schedule_Value"].ToString() + "&Schedule_Type=" + dr["Schedule_Type"] + "&Created_Date=" + dr["Created_Date"].ToString() + "&TaskScheduleVersion=" + dr["TaskScheduleVersion"] + "&TaskSchedule_Approved=" + dr["TaskSchedule_Approved"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"] + "&Achieved_Value=" + dr["Achieved_Value"] + "&Achieved_Date=" + dr["Achieved_Date"].ToString() + "&Schedule_Per=" + dr["Schedule_Per"] + "&Achieved_Per=" + dr["Achieved_Per"];//

        //                data = webPostMethod(postData, URL);
        //                if (data.ToString().Contains("true"))
        //                {
        //                    dbutility.updateAddFlag(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule", "TaskScheduleUID");
        //                    dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule Add", "Success", "");
        //                    Console.WriteLine("Synching for TaskSchedule Add : " + dr["TaskScheduleUID"].ToString() + " Done");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule Add", "Error", ex.Message);
        //            }
        //        }
        //        //
        //        MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From TaskSchedule where ServerCopiedUpdate='N' and WorkpacageUID='" + wkPkgUID + "'", MyConnection);




        //        //
        //        //if (MyConnection.State == System.Data.ConnectionState.Closed)
        //        //{
        //        //    MyConnection.Open();
        //        //}
        //        MyDataset.Clear();
        //        MyAdapter.Fill(MyDataset);
        //        Console.WriteLine("Started Synching for TaskSchedule Update : ");
        //        if (MyDataset.Tables[0].Rows.Count == 0)
        //        {
        //            Console.WriteLine("No Records found for TaskSchedule Update : ");
        //        }
        //        foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
        //        {
        //            try
        //            {
        //                postData = "TaskScheduleUID=" + dr["TaskScheduleUID"].ToString() + "&WorkpacageUID=" + dr["WorkpacageUID"].ToString() + "&TaskUID=" + dr["TaskUID"] + "&StartDate=" + dr["StartDate"].ToString() + "&EndDate=" + dr["EndDate"] + "&Schedule_Value=" + dr["Schedule_Value"].ToString() + "&Schedule_Type=" + dr["Schedule_Type"] + "&Created_Date=" + dr["Created_Date"].ToString() + "&TaskScheduleVersion=" + dr["TaskScheduleVersion"] + "&TaskSchedule_Approved=" + dr["TaskSchedule_Approved"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"] + "&Achieved_Value=" + dr["Achieved_Value"] + "&Achieved_Date=" + dr["Achieved_Date"].ToString() + "&Schedule_Per=" + dr["Schedule_Per"] + "&Achieved_Per=" + dr["Achieved_Per"];//

        //                data = webPostMethod(postData, URL);
        //                if (data.ToString().Contains("true"))
        //                {
        //                    dbutility.updateUpdateFlag(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule", "TaskScheduleUID");
        //                    dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "TaskSchedule Update", "Success", "");
        //                    Console.WriteLine("Synching for TaskSchedule Update : " + dr["TaskScheduleUID"].ToString() + " Done");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleUID"].ToString()), "FinanceMileStoneMonth Update", "Error", ex.Message);
        //            }
        //        }
        //        Console.WriteLine("Synching for TaskSchedule Add Done : ");
        //        // Console.ReadLine();
        //    }
        //    catch (Exception ex)
        //    {
        //        dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "TaskSchedule Add/Update", "Error", ex.Message);
        //        Console.WriteLine(ex.Message);
        //        //Console.ReadLine();

        //    }
        //}

        private void SyncTaskScheduleVersion()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From TaskScheduleVesrion where ServerCopiedAdd='N' and TaskUID in (Select TaskUID From Tasks Where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                ///

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "TaskScheduleVersionSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for TaskScheduleVesrion Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for TaskScheduleVesrion Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "TaskScheduleVersion_UID=" + dr["TaskScheduleVersion_UID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&TaskScheduleVersion=" + dr["TaskScheduleVersion"] + "&TaskScheduleType=" + dr["TaskScheduleType"].ToString() + "&TaskSchedule_Approved=" + dr["TaskSchedule_Approved"] + "&Delete_Flag=" + dr["Delete_Flag"].ToString();//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["TaskScheduleVersion_UID"].ToString()), "TaskScheduleVesrion", "TaskScheduleVersion_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleVersion_UID"].ToString()), "TaskScheduleVesrion Add", "Success", "");
                            Console.WriteLine("Synching for TaskScheduleVesrion Add : " + dr["TaskScheduleVersion_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleVersion_UID"].ToString()), "TaskScheduleVesrion Add", "Error", ex.Message);
                    }
                }
                //
             
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From TaskScheduleVesrion where ServerCopiedUpdate='N' and TaskUID in (Select TaskUID From Tasks Where WorkPackageUID ='" + wkPkgUID + "')", MyConnection);
                ///



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for TaskScheduleVesrion Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for TaskScheduleVesrion Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "TaskScheduleVersion_UID=" + dr["TaskScheduleVersion_UID"].ToString() + "&TaskUID=" + dr["TaskUID"].ToString() + "&TaskScheduleVersion=" + dr["TaskScheduleVersion"] + "&TaskScheduleType=" + dr["TaskScheduleType"].ToString() + "&TaskSchedule_Approved=" + dr["TaskSchedule_Approved"] + "&Delete_Flag=" + dr["Delete_Flag"].ToString();//

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["TaskScheduleVersion_UID"].ToString()), "TaskScheduleVesrion", "TaskScheduleVersion_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleVersion_UID"].ToString()), "TaskScheduleVesrion Update", "Success", "");
                            Console.WriteLine("Synching for TaskScheduleVesrion Update : " + dr["TaskScheduleVersion_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskScheduleVersion_UID"].ToString()), "TaskScheduleVesrion Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for TaskScheduleVesrion Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "TaskScheduleVesrion Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        //private void SyncTask()
        //{
        //    try
        //    {
        //        // source
        //        //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
        //        System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
        //        MyConnection.ConnectionString = GetSourceConnectionString();
        //        System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
        //        System.Data.DataSet MyDataset = new System.Data.DataSet();
        //        System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
        //        MyCommand.Connection = MyConnection;

        //        // check the submittal table for any records to be added or updated.....
        //        MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
        //        //

        //        MyDataset.Clear();
        //        MyAdapter.Fill(MyDataset);
        //        string postData = "";
        //        string URL = serviceURL + "TaskSync"; ;
        //        var data = "";


        //        Console.WriteLine("Started Synching for Tasks Add : ");
        //        if (MyDataset.Tables[0].Rows.Count == 0)
        //        {
        //            Console.WriteLine("No Records found for Tasks Add : ");
        //        }
        //        foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
        //        {
        //            try
        //            {
        //                postData = "TaskUID=" + dr["TaskUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
        //                    "&Workpackage_Option=" + dr["Workpackage_Option"].ToString() + "&Owner=" + dr["Owner"] + "&Task_Section=" + dr["Task_Section"].ToString() + "&Name=" + dr["Name"] + "&Description=" + dr["Description"].ToString() + "&RFPReference=" + dr["RFPReference"] + "&POReference=" + dr["POReference"].ToString() + "&StartDate=" + dr["StartDate"] +
        //                    "&PlannedEndDate=" + dr["PlannedEndDate"].ToString() + "&ProjectedEndDate=" + dr["ProjectedEndDate"] + "&Status=" + dr["Status"].ToString() + "&Currency=" + dr["Currency"] +
        //                    "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] +
        //                "&Basic_Budget=" + dr["Basic_Budget"] +
        //                    "&GST=" + dr["GST"] +
        //                    "&Total_Budget=" + dr["Total_Budget"] +
        //                    "&ActualExpenditure=" + dr["ActualExpenditure"] +
        //                    "&RFPDocument=" + dr["RFPDocument"] +
        //                    "&NoOfDocuments=" + dr["NoOfDocuments"] +
        //                    "&TaskLevel=" + dr["TaskLevel"] +
        //                    "&ParentTaskID=" + dr["ParentTaskID"] +
        //                    "&UpdatedDate=" + dr["UpdatedDate"] +
        //                    "&StatusPer=" + dr["StatusPer"] +
        //                    "&UnitforProgress=" + dr["UnitforProgress"] +
        //                    "&UnitQuantity=" + dr["UnitQuantity"] +
        //                    "&PlannedStartDate=" + dr["PlannedStartDate"] +
        //                    "&ProjectedStartDate=" + dr["ProjectedStartDate"] +
        //                    "&ActualEndDate=" + dr["ActualEndDate"] +
        //                    "&Discipline=" + dr["Discipline"] +
        //                    "&MileStone=" + dr["MileStone"] +
        //                    "&Task_Weightage=" + dr["Task_Weightage"] +
        //                    "&Task_Type=" + dr["Task_Type"] +
        //                    "&Delete_Flag=" + dr["Delete_Flag"] +
        //                    "&Task_Order=" + dr["Task_Order"] +
        //                    "&BOQDetailsUID=" + dr["BOQDetailsUID"] +
        //                    "&GroupBOQItems=" + dr["GroupBOQItems"] +
        //                    "&Task_CulumativePercentage=" + dr["Task_CulumativePercentage"] +
        //                     "&CumulativeAchvQuantity=" + dr["CumulativeAchvQuantity"] +
        //                      "&InGraph=" + dr["InGraph"] +
        //                       "&Report1=" + dr["Report1"] +
        //                        "&Report2=" + dr["Report2"] +
        //                         "&Report3=" + dr["Report3"] +
        //                          "&Report4=" + dr["Report4"] +
        //                           "&Report5=" + dr["Report5"] ;
                          
        //           data = webPostMethod(postData, URL);
        //                if (data.ToString().Contains("true"))
        //                {
        //                    dbutility.updateAddFlag(new Guid(dr["TaskUID"].ToString()), "Tasks", "TaskUID");
        //                    dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "Tasks Add", "Success", "");
        //                    Console.WriteLine("Synching for Tasks Add : " + dr["TaskUID"].ToString() + " Done");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "TaskSchedule Add", "Error", ex.Message);
        //            }
        //        }
        //        //
        //        MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




        //        //
        //        //if (MyConnection.State == System.Data.ConnectionState.Closed)
        //        //{
        //        //    MyConnection.Open();
        //        //}
        //        MyDataset.Clear();
        //        MyAdapter.Fill(MyDataset);
        //        Console.WriteLine("Started Synching for Tasks Update : ");
        //        if (MyDataset.Tables[0].Rows.Count == 0)
        //        {
        //            Console.WriteLine("No Records found for Tasks Update : ");
        //        }
        //        foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
        //        {
        //            try
        //            {
        //                postData = "TaskUID=" + dr["TaskUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
        //                    "&Workpackage_Option=" + dr["Workpackage_Option"].ToString() + "&Owner=" + dr["Owner"] + "&Task_Section=" + dr["Task_Section"].ToString() + "&Name=" + dr["Name"] + "&Description=" + dr["Description"].ToString() + "&RFPReference=" + dr["RFPReference"] + "&POReference=" + dr["POReference"].ToString() + "&StartDate=" + dr["StartDate"] +
        //                    "&PlannedEndDate=" + dr["PlannedEndDate"].ToString() + "&ProjectedEndDate=" + dr["ProjectedEndDate"] + "&Status=" + dr["Status"].ToString() + "&Currency=" + dr["Currency"] +
        //                    "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] +
        //                "&Basic_Budget=" + dr["Basic_Budget"] +
        //                    "&GST=" + dr["GST"] +
        //                    "&Total_Budget=" + dr["Total_Budget"] +
        //                    "&ActualExpenditure=" + dr["ActualExpenditure"] +
        //                    "&RFPDocument=" + dr["RFPDocument"] +
        //                    "&NoOfDocuments=" + dr["NoOfDocuments"] +
        //                    "&TaskLevel=" + dr["TaskLevel"] +
        //                    "&ParentTaskID=" + dr["ParentTaskID"] +
        //                    "&UpdatedDate=" + dr["UpdatedDate"] +
        //                    "&StatusPer=" + dr["StatusPer"] +
        //                    "&UnitforProgress=" + dr["UnitforProgress"] +
        //                    "&UnitQuantity=" + dr["UnitQuantity"] +
        //                    "&PlannedStartDate=" + dr["PlannedStartDate"] +
        //                    "&ProjectedStartDate=" + dr["ProjectedStartDate"] +
        //                    "&ActualEndDate=" + dr["ActualEndDate"] +
        //                    "&Discipline=" + dr["Discipline"] +
        //                    "&MileStone=" + dr["MileStone"] +
        //                    "&Task_Weightage=" + dr["Task_Weightage"] +
        //                    "&Task_Type=" + dr["Task_Type"] +
        //                    "&Delete_Flag=" + dr["Delete_Flag"] +
        //                    "&Task_Order=" + dr["Task_Order"] +
        //                    "&BOQDetailsUID=" + dr["BOQDetailsUID"] +
        //                      "&GroupBOQItems=" + dr["GroupBOQItems"] +
        //                    "&Task_CulumativePercentage=" + dr["Task_CulumativePercentage"] +
        //                     "&CumulativeAchvQuantity=" + dr["CumulativeAchvQuantity"] +
        //                      "&InGraph=" + dr["InGraph"] +
        //                       "&Report1=" + dr["Report1"] +
        //                        "&Report2=" + dr["Report2"] +
        //                         "&Report3=" + dr["Report3"] +
        //                          "&Report4=" + dr["Report4"] +
        //                           "&Report5=" + dr["Report5"];
        //                data = webPostMethod(postData, URL);
        //                if (data.ToString().Contains("true"))
        //                {
        //                    dbutility.updateUpdateFlag(new Guid(dr["TaskUID"].ToString()), "Tasks", "TaskUID");
        //                    dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "Tasks Update", "Success", "");
        //                    Console.WriteLine("Synching for Tasks Update : " + dr["TaskUID"].ToString() + " Done");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbutility.InsertintoDbsyncLogs(new Guid(dr["TaskUID"].ToString()), "Tasks Update", "Error", ex.Message);
        //            }
        //        }
        //        Console.WriteLine("Synching for Tasks Add Done : ");
        //        // Console.ReadLine();
        //    }
        //    catch (Exception ex)
        //    {
        //        dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Tasks Add/Update", "Error", ex.Message);
        //        Console.WriteLine(ex.Message);
        //        //Console.ReadLine();

        //    }
        //}

        private void SyncDailyProgressReportMaster()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DailyProgressReportMaster where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DailyProgressReportMasterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for DailyProgressReportMaster Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DailyProgressReportMaster Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DPR_UID"].ToString()), "DailyProgressReportMaster", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "DailyProgressReportMaster Add", "Success", "");
                            Console.WriteLine("Synching for DailyProgressReportMaster Add : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "DailyProgressReportMaster Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DailyProgressReportMaster where ServerCopiedUpdate='N'", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for DailyProgressReportMaster Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DailyProgressReportMaster Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DPR_UID"].ToString()), "DailyProgressReportMaster", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "DailyProgressReportMaster Update", "Success", "");
                            Console.WriteLine("Synching for DailyProgressReportMaster Update : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "DailyProgressReportMaster Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for DailyProgressReportMaster Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "DailyProgressReportMaster Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncDailyProgress()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DailyProgress where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "DailyProgressSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for DailyProgress Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DailyProgress Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&VillageName=" + dr["VillageName"] + "&PipeDia=" + dr["PipeDia"].ToString() + "&Quantity=" + dr["Quantity"] + "&RevisedQuantity=" + dr["RevisedQuantity"].ToString() + "&PipesReceived=" + dr["PipesReceived"] + "&PreviousQuantity=" + dr["PreviousQuantity"].ToString() + "&TodaysQuantity=" + dr["TodaysQuantity"] +
                            "&TotalQuantity=" + dr["TotalQuantity"].ToString() + "&Balance=" + dr["Balance"] + "&Remarks=" + dr["Remarks"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() +
                        "&ZoneName=" + dr["ZoneName"].ToString() +
                            "&DeletedFlag=" + dr["DeletedFlag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "DailyProgress", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DailyProgress Add", "Success", "");
                            Console.WriteLine("Synching for DailyProgress Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DailyProgress Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From DailyProgress where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for DailyProgress Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for DailyProgress Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                              "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&VillageName=" + dr["VillageName"] + "&PipeDia=" + dr["PipeDia"].ToString() + "&Quantity=" + dr["Quantity"] + "&RevisedQuantity=" + dr["RevisedQuantity"].ToString() + "&PipesReceived=" + dr["PipesReceived"] + "&PreviousQuantity=" + dr["PreviousQuantity"].ToString() + "&TodaysQuantity=" + dr["TodaysQuantity"] +
                              "&TotalQuantity=" + dr["TotalQuantity"].ToString() + "&Balance=" + dr["Balance"] + "&Remarks=" + dr["Remarks"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() +
                          "&ZoneName=" + dr["ZoneName"].ToString() +
                              "&DeletedFlag=" + dr["DeletedFlag"];
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "DailyProgress", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DailyProgress Update", "Success", "");
                            Console.WriteLine("Synching for DailyProgress Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "DailyProgress Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for DailyProgress Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "DailyProgress Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        //
        private void SyncGFCReportMaster()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From GFCReportMaster where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "GFCReportMasterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for GFCReportMaster Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for GFCReportMaster Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "GFC_UID=" + dr["GFC_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["GFC_UID"].ToString()), "GFCReportMaster", "GFC_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["GFC_UID"].ToString()), "GFCReportMaster Add", "Success", "");
                            Console.WriteLine("Synching for GFCReportMaster Add : " + dr["GFC_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["GFC_UID"].ToString()), "GFCReportMaster Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From GFCReportMaster where ServerCopiedUpdate='N'", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for GFCReportMaster Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for GFCReportMaster Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "GFC_UID=" + dr["GFC_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["GFC_UID"].ToString()), "GFCReportMaster", "GFC_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["GFC_UID"].ToString()), "GFCReportMaster Update", "Success", "");
                            Console.WriteLine("Synching for GFCReportMaster Update : " + dr["GFC_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["GFC_UID"].ToString()), "GFCReportMaster Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for GFCReportMaster Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "GFCReportMaster Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncGFCStatus()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From GFCStatus where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "GFCStatusSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for GFCStatusSync Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for GFCStatusSync Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&GFC_UID=" + dr["GFC_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&SubmittedLocation=" + dr["SubmittedLocation"] + "&SubmittedLength=" + dr["SubmittedLength"].ToString() + "&SubmittedDate=" + dr["SubmittedDate"].ToString() + "&ONTBReleasedLength=" + dr["ONTBReleasedLength"].ToString() + "&ONTBReleasedDate=" + dr["ONTBReleasedDate"].ToString() + "&ONTBReleasedBalance=" + dr["ONTBReleasedBalance"].ToString() + "&ONTBReleasedBalanceDate=" + dr["ONTBReleasedBalanceDate"].ToString() +
                            "&GFCApproved=" + dr["GFCApproved"].ToString() + "&Remarks=" + dr["Remarks"] + "&EEOfficeApproval=" + dr["EEOfficeApproval"].ToString() + "&NineSetsApproval=" + dr["NineSetsApproval"].ToString() +
                        "&Approved=" + dr["Approved"].ToString() +
                            "&DeletedFlag=" + dr["DeletedFlag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "GFCStatus", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "GFCStatus Add", "Success", "");
                            Console.WriteLine("Synching for GFCStatus Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "GFCStatus Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From GFCStatus where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for GFCStatus Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for GFCStatus Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&GFC_UID=" + dr["GFC_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                             "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&SubmittedLocation=" + dr["SubmittedLocation"] + "&SubmittedLength=" + dr["SubmittedLength"].ToString() + "&SubmittedDate=" + dr["SubmittedDate"].ToString() + "&ONTBReleasedLength=" + dr["ONTBReleasedLength"].ToString() + "&ONTBReleasedDate=" + dr["ONTBReleasedDate"].ToString() + "&ONTBReleasedBalance=" + dr["ONTBReleasedBalance"].ToString() + "&ONTBReleasedBalanceDate=" + dr["ONTBReleasedBalanceDate"].ToString() +
                             "&GFCApproved=" + dr["GFCApproved"].ToString() + "&Remarks=" + dr["Remarks"] + "&EEOfficeApproval=" + dr["EEOfficeApproval"].ToString() + "&NineSetsApproval=" + dr["NineSetsApproval"].ToString() +
                         "&Approved=" + dr["Approved"].ToString() +
                             "&DeletedFlag=" + dr["DeletedFlag"];
                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "GFCStatus", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "GFCStatus Update", "Success", "");
                            Console.WriteLine("Synching for GFCStatus Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "GFCStatus Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for GFCStatus Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "GFCStatus Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }
        //

        private void SyncDesign_and_drawing_A_master()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From design_and_drawing_A_master where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "Design_and_drawing_A_masterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for design_and_drawing_A_master Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for design_and_drawing_A_master Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_A_master", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_A_master Add", "Success", "");
                            Console.WriteLine("Synching for design_and_drawing_A_master Add : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["GFC_UID"].ToString()), "design_and_drawing_A_master Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From design_and_drawing_A_master where ServerCopiedUpdate='N'", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for design_and_drawing_A_master Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for design_and_drawing_A_master Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_A_master", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_A_master Update", "Success", "");
                            Console.WriteLine("Synching for design_and_drawing_A_master Update : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_A_master Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for design_and_drawing_A_master Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "design_and_drawing_A_master Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        /// <summary>
        ///  this is details for design and drawings Works A Master
        /// </summary>
        private void SyncDesign_and_drawing_works_A()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Design_and_drawing_works_A where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "Design_and_drawing_works_ASync"; ;
                var data = "";


                Console.WriteLine("Started Synching for Design_and_drawing_works_A Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Design_and_drawing_works_A Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&Heading=" + dr["Heading"] + "&subheading=" + dr["subheading"].ToString() + "&Units=" + dr["Units"].ToString() + "&SiteWorks=" + dr["SiteWorks"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString();
                       

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_A", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_A Add", "Success", "");
                            Console.WriteLine("Synching for Design_and_drawing_works_A Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_A Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Design_and_drawing_works_A where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Design_and_drawing_works_A Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Design_and_drawing_works_A Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&Heading=" + dr["Heading"] + "&subheading=" + dr["subheading"].ToString() + "&Units=" + dr["Units"].ToString() + "&SiteWorks=" + dr["SiteWorks"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_A", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_A Update", "Success", "");
                            Console.WriteLine("Synching for Design_and_drawing_works_A Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_A Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Design_and_drawing_works_A Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Design_and_drawing_works_A Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }


        //added on 20/08/2022 for nikhil  dbsync
        private void Syncdesign_and_drawing_dwg_issue_master()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From design_and_drawing_dwg_issue_master where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "design_and_drawing_dwg_issue_masterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for design_and_drawing_dwg_issue_master Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for design_and_drawing_dwg_issue_master Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_dwg_issue_master", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_dwg_issue_master Add", "Success", "");
                            Console.WriteLine("Synching for design_and_drawing_dwg_issue_master Add : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_dwg_issue_master Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From design_and_drawing_dwg_issue_master where ServerCopiedUpdate='N'", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for design_and_drawing_dwg_issue_master Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for design_and_drawing_dwg_issue_master Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_dwg_issue_master", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_dwg_issue_master Update", "Success", "");
                            Console.WriteLine("Synching for design_and_drawing_dwg_issue_master Update : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_dwg_issue_master Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for design_and_drawing_dwg_issue_master Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "design_and_drawing_dwg_issue_master Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }


        //added on 23/08/2022 for nikhil dbsync
        private void SyncRABillPayments()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABillPayments where ServerCopiedAdd='N' and InvoiceUID in (select InvoiceMaster_UID From InvoiceMaster Where WorkpackageUID ='" + wkPkgUID + "')", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "RABillPaymentsSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for RABillPayments Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABillPayments Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "PaymentUID=" + dr["PaymentUID"].ToString() + "&InvoiceUID=" + dr["InvoiceUID"].ToString() + "&RABillDesc=" + dr["RABillDesc"].ToString() +
                            "&Amount=" + dr["Amount"].ToString() + "&TotalDeductions=" + dr["TotalDeductions"].ToString() + "&NetAmount=" + dr["NetAmount"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString() +
                            "&FinMileStoneMonthUID=" + dr["FinMileStoneMonthUID"].ToString() + "&PaymentDate=" + dr["PaymentDate"].ToString();



                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["PaymentUID"].ToString()), "RABillPayments", "PaymentUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["PaymentUID"].ToString()), "RABillPayments Add", "Success", "");
                            Console.WriteLine("Synching for RABillPayments Add : " + dr["PaymentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["PaymentUID"].ToString()), "RABillPayments Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From RABillPayments where ServerCopiedUpdate='N' and InvoiceUID in (select InvoiceMaster_UID From InvoiceMaster Where WorkpackageUID ='" + wkPkgUID + "')", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for RABillPayments Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for RABillPayments Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "PaymentUID=" + dr["PaymentUID"].ToString() + "&InvoiceUID=" + dr["InvoiceUID"].ToString() + "&RABillDesc=" + dr["RABillDesc"].ToString() +
                             "&Amount=" + dr["Amount"].ToString() + "&TotalDeductions=" + dr["TotalDeductions"].ToString() + "&NetAmount=" + dr["NetAmount"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString() +
                             "&FinMileStoneMonthUID=" + dr["FinMileStoneMonthUID"].ToString() + "&PaymentDate=" + dr["PaymentDate"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["PaymentUID"].ToString()), "RABillPayments", "PaymentUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["PaymentUID"].ToString()), "RABillPayments Update", "Success", "");
                            Console.WriteLine("Synching for RABillPayments Update : " + dr["PaymentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["PaymentUID"].ToString()), "RABillPayments Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for RABillPayments Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "RABillPayments Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncDesign_and_drawing_works_dwg_issue()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Design_and_drawing_works_dwg_issue where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "design_and_drawing_dwg_issueSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for Design_and_drawing_works_dwg_issue Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Design_and_drawing_works_dwg_issue Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&Location=" + dr["Location"] + "&submittedbycontrator=" + dr["submittedbycontrator"].ToString() + "&submitteddate=" + dr["submitteddate"].ToString() +
                            "&approverbyontb=" + dr["approverbyontb"].ToString() + "&approverdate=" + dr["approverdate"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&ZoneName=" + dr["ZoneName"].ToString() +
                            "&DeletedFlag=" + dr["DeletedFlag"].ToString() + "&GFCApprovedByBWSSB=" + dr["GFCApprovedByBWSSB"].ToString() + "&GFCApprovedDate=" + dr["GFCApprovedDate"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_dwg_issue", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_dwg_issue Add", "Success", "");
                            Console.WriteLine("Synching for Design_and_drawing_works_dwg_issue Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_dwg_issue Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Design_and_drawing_works_dwg_issue where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Design_and_drawing_works_dwg_issue Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Design_and_drawing_works_dwg_issue Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&Location=" + dr["Location"] + "&submittedbycontrator=" + dr["submittedbycontrator"].ToString() + "&submitteddate=" + dr["submitteddate"].ToString() +
                            "&approverbyontb=" + dr["approverbyontb"].ToString() + "&approverdate=" + dr["approverdate"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&ZoneName=" + dr["ZoneName"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString() +
                            "&GFCApprovedByBWSSB=" + dr["GFCApprovedByBWSSB"].ToString() + "&GFCApprovedDate=" + dr["GFCApprovedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_dwg_issue", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_dwg_issue Update", "Success", "");
                            Console.WriteLine("Synching for Design_and_drawing_works_dwg_issue Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_dwg_issue Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Design_and_drawing_works_dwg_issue Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Design_and_drawing_works_dwg_issue Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void Syncdesign_and_drawing_works_b_tt_master()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From design_and_drawing_works_b_tt_master where ServerCopiedAdd='N'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "design_and_drawing_works_b_tt_masterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for design_and_drawing_works_b_tt_master Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for design_and_drawing_works_b_tt_master Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_works_b_tt_master", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_works_b_tt_master Add", "Success", "");
                            Console.WriteLine("Synching for design_and_drawing_works_b_tt_master Add : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_works_b_tt_master Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From design_and_drawing_works_b_tt_master where ServerCopiedUpdate='N'", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for design_and_drawing_works_b_tt_master Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for design_and_drawing_works_b_tt_master Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "DPR_UID=" + dr["DPR_UID"].ToString() + "&Description=" + dr["Description"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_works_b_tt_master", "DPR_UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_works_b_tt_master Update", "Success", "");
                            Console.WriteLine("Synching for design_and_drawing_works_b_tt_master Update : " + dr["DPR_UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["DPR_UID"].ToString()), "design_and_drawing_works_b_tt_master Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for design_and_drawing_works_b_tt_master Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "design_and_drawing_works_b_tt_master Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SyncDesign_and_drawing_works_b_tt()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Design_and_drawing_works_b_tt where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "Design_and_drawing_works_b_ttSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for Design_and_drawing_works_b_tt Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Design_and_drawing_works_b_tt Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&VillageName=" + dr["VillageName"] + "&Crossing=" + dr["Crossing"].ToString() + "&GFCStaus=" + dr["GFC Staus"].ToString() +
                            "&Length=" + dr["Length"].ToString() + "&Diameter=" + dr["Diameter"].ToString() + "&StartDepth=" + dr["StartDepth"].ToString() + "&StopDepth=" + dr["StopDepth"].ToString() +
                            "&Remarks=" + dr["Remarks"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&ZoneName=" + dr["ZoneName"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_b_tt", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_b_tt Add", "Success", "");
                            Console.WriteLine("Synching for Design_and_drawing_works_b_tt Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_b_tt Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Design_and_drawing_works_b_tt where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for Design_and_drawing_works_b_tt Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for Design_and_drawing_works_b_tt Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&DPR_UID=" + dr["DPR_UID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                            "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&VillageName=" + dr["VillageName"] + "&Crossing=" + dr["Crossing"].ToString() + "&GFCStaus=" + dr["GFC Staus"].ToString() +
                            "&Length=" + dr["Length"].ToString() + "&Diameter=" + dr["Diameter"].ToString() + "&StartDepth=" + dr["StartDepth"].ToString() + "&StopDepth=" + dr["StopDepth"].ToString() +
                            "&Remarks=" + dr["Remarks"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString() + "&ZoneName=" + dr["ZoneName"].ToString() + "&DeletedFlag=" + dr["DeletedFlag"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_b_tt", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_b_tt Update", "Success", "");
                            Console.WriteLine("Synching for Design_and_drawing_works_b_tt Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "Design_and_drawing_works_b_tt Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for Design_and_drawing_works_b_tt Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "Design_and_drawing_works_b_tt Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        //added on 05/01/2023
        private void SyncResourceMaster()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ResourceMaster where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "ResourceMasterSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for ResourceMaster Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ResourceMaster Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "ResourceUID=" + dr["ResourceUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                             "&ResourceName=" + dr["ResourceName"] +
                            "&CostType=" + dr["CostType"] +
                            "&Currency=" + dr["Currency"] +
                            "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] +
                            "&Resource_Owner=" + dr["Resource_Owner"] +
                        "&Basic_Budget=" + dr["Basic_Budget"] +
                            "&GST=" + dr["GST"] +
                            "&Total_Budget=" + dr["Total_Budget"] +
                            "&ResourceLevel=" + dr["ResourceLevel"] +
                            "&ParentResourceID=" + dr["ParentResourceID"] +
                            "&Resource_Description=" + dr["Resource_Description"] +
                            "&Unit_for_Measurement=" + dr["Unit_for_Measurement"] +
                            "&ResourceType_UID=" + dr["ResourceType_UID"] +
                            "&Delete_Flag=" + dr["Delete_Flag"] ;

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["ResourceUID"].ToString()), "ResourceMaster", "ResourceUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["ResourceUID"].ToString()), "ResourceMaster Add", "Success", "");
                            Console.WriteLine("Synching for Tasks Add : " + dr["ResourceUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["ResourceUID"].ToString()), "ResourceMaster Add", "Error", ex.Message);
                    }
                }
                //
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ResourceMaster where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for ResourceMaster Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ResourceMaster Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "ResourceUID=" + dr["ResourceUID"].ToString() + "&WorkPackageUID=" + dr["WorkPackageUID"].ToString() + "&ProjectUID=" + dr["ProjectUID"] +
                             "&ResourceName=" + dr["ResourceName"] +
                            "&CostType=" + dr["CostType"] +
                            "&Currency=" + dr["Currency"] +
                            "&Currency_CultureInfo=" + dr["Currency_CultureInfo"] +
                            "&Resource_Owner=" + dr["Resource_Owner"] +
                        "&Basic_Budget=" + dr["Basic_Budget"] +
                            "&GST=" + dr["GST"] +
                            "&Total_Budget=" + dr["Total_Budget"] +
                            "&ResourceLevel=" + dr["ResourceLevel"] +
                            "&ParentResourceID=" + dr["ParentResourceID"] +
                            "&Resource_Description=" + dr["Resource_Description"] +
                            "&Unit_for_Measurement=" + dr["Unit_for_Measurement"] +
                            "&ResourceType_UID=" + dr["ResourceType_UID"] +
                            "&Delete_Flag=" + dr["Delete_Flag"];

                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["ResourceUID"].ToString()), "ResourceMaster", "ResourceUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["ResourceUID"].ToString()), "ResourceMaster Update", "Success", "");
                            Console.WriteLine("Synching for ResourceMaster Update : " + dr["ResourceUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["ResourceUID"].ToString()), "ResourceMaster Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for ResourceMaster Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "ResourceMaster Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }


        //added on 05-01-2023 by Nikhil
        private void SyncResourceDeployment()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ResourceDeployment where ServerCopiedAdd='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "ResourceDeploymentSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for ResourceDeployment Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ResourceDeployment Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "ReourceDeploymentUID=" + dr["ReourceDeploymentUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&ResourceUID=" + dr["ResourceUID"].ToString() +
                            "&StartDate=" + dr["StartDate"].ToString() + "&EndDate=" + dr["EndDate"].ToString() + "&DeploymentType=" + dr["DeploymentType"].ToString() + "&Planned=" + dr["Planned"].ToString() +
                            "&PlannedDate=" + dr["PlannedDate"].ToString() + "&Deployed=" + dr["Deployed"].ToString() + "&DeployedDate=" + dr["DeployedDate"].ToString() + "&Remarks=" + dr["Remarks"].ToString()
                            + "&Created_Date=" + dr["Created_Date"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();



                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["ReourceDeploymentUID"].ToString()), "ResourceDeployment", "ReourceDeploymentUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["ReourceDeploymentUID"].ToString()), "ResourceDeployment Add", "Success", "");
                            Console.WriteLine("Synching for ReourceDeployment Add : " + dr["ReourceDeploymentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["ReourceDeploymentUID"].ToString()), "ResourceDeployment Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ResourceDeployment where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for ResourceDeployment Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ResourceDeployment Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "ReourceDeploymentUID=" + dr["ReourceDeploymentUID"].ToString() + "&WorkpackageUID=" + dr["WorkpackageUID"].ToString() + "&ResourceUID=" + dr["ResourceUID"].ToString() +
                            "&StartDate=" + dr["StartDate"].ToString() + "&EndDate=" + dr["EndDate"].ToString() + "&DeploymentType=" + dr["DeploymentType"].ToString() + "&Planned=" + dr["Planned"].ToString() +
                            "&PlannedDate=" + dr["PlannedDate"].ToString() + "&Deployed=" + dr["Deployed"].ToString() + "&DeployedDate=" + dr["DeployedDate"].ToString() + "&Remarks=" + dr["Remarks"].ToString()
                            + "&Created_Date=" + dr["Created_Date"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["ReourceDeploymentUID"].ToString()), "ResourceDeployment", "PaymentUID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["ReourceDeploymentUID"].ToString()), "ResourceDeployment Update", "Success", "");
                            Console.WriteLine("Synching for ResourceDeployment Update : " + dr["ReourceDeploymentUID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["ReourceDeploymentUID"].ToString()), "ResourceDeployment Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for ResourceDeployment Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "ResourceDeployment Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        //added on 05-01-2023 by Nikhil
        private void SynchResourceDeploymentUpdate()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ResourceDeploymentUpdate where ServerCopiedAdd='N' and  ReourceDeploymentUID in (Select ReourceDeploymentUID From ResourceDeployment Where WorkpackageUID='" + wkPkgUID + "')", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "ResourceDeploymentUpdateSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for ResourceDeploymentUpdate Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ResourceDeploymentUpdate Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&ReourceDeploymentUID=" + dr["ReourceDeploymentUID"].ToString() + "&Deployed=" + dr["Deployed"].ToString() +
                            "&DeployedDate=" + dr["DeployedDate"].ToString() + "&Remarks=" + dr["Remarks"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();



                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "ResourceDeploymentUpdate", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ResourceDeploymentUpdate Add", "Success", "");
                            Console.WriteLine("Synching for ResourceDeploymentUpdate Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ResourceDeploymentUpdate Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);
                
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From ResourceDeploymentUpdate where ServerCopiedUpdate='N' and  ReourceDeploymentUID in (Select ReourceDeploymentUID From ResourceDeployment Where WorkpackageUID='" + wkPkgUID + "')", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for ResourceDeploymentUpdate Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for ResourceDeploymentUpdate Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&ReourceDeploymentUID=" + dr["ReourceDeploymentUID"].ToString() + "&Deployed=" + dr["Deployed"].ToString() +
                            "&DeployedDate=" + dr["DeployedDate"].ToString() + "&Remarks=" + dr["Remarks"].ToString() + "&Delete_Flag=" + dr["Delete_Flag"].ToString();



                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "ResourceDeploymentUpdate", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ResourceDeploymentUpdate Update", "Success", "");
                            Console.WriteLine("Synching for ResourceDeploymentUpdate Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "ResourceDeploymentUpdate Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for ResourceDeploymentUpdate Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "ResourceDeploymentUpdate Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        //added on 17/01/2023 for nikhil
        private void Synchform_task_update()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From [form-task-update] where ServerCopiedAdd='N' and WorkpackageUID='" + wkPkgUID + "'", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "form_task_updateSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for [form-task-update] Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for [form-task-update] Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "Form_task_update_uid=" + dr["Form-task-update-uid"].ToString() + "&Taskuid=" + dr["Taskuid"].ToString() + "&TaskName=" + dr["TaskName"].ToString() +
                            "&projectuid=" + dr["projectuid"].ToString() + "&workpackageuid=" + dr["workpackageuid"].ToString();



                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["Form-task-update-uid"].ToString()), "[form-task-update]", "[Form-task-update-uid]");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Form-task-update-uid"].ToString()), "form-task-update Add", "Success", "");
                            Console.WriteLine("Synching for form-task-update Add : " + dr["Form-task-update-uid"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Form-task-update-uid"].ToString()), "[form-task-update] Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From [form-task-update] where ServerCopiedAdd='N' and WorkpackageUID='" + wkPkgUID + "'", MyConnection);



                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for form-task-update Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for form-task-update Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "Form_task_update_uid=" + dr["Form-task-update-uid"].ToString() + "&Taskuid=" + dr["Taskuid"].ToString() + "&TaskName=" + dr["TaskName"].ToString() +
                            "&projectuid=" + dr["projectuid"].ToString() + "&workpackageuid=" + dr["workpackageuid"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["Form-task-update-uid"].ToString()), "[form-task-update]", "[Form-task-update-uid]");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["Form-task-update-uid"].ToString()), "form-task-update Update", "Success", "");
                            Console.WriteLine("Synching for form-task-update Update : " + dr["Form-task-update-uid"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["Form-task-update-uid"].ToString()), "form-task-update Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for form-task-update Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "form-task-update Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void SynchCorrespondenceCCToUsers()
        {
            try
            {
                // source
                //string wkPkgUID = "28A6A63B-2573-40A8-BC89-E396C31CE516";
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetSourceConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;

                // check the submittal table for any records to be added or updated.....
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From CorrespondenceCCToUsers where ServerCopiedAdd='N' and  ActualDocumentUID in (Select ActualDocumentUID From ActualDocuments Where WorkpackageUID='" + wkPkgUID + "')", MyConnection);
                //

                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string postData = "";
                string URL = serviceURL + "CorrespondenceCCToUsersSync"; ;
                var data = "";


                Console.WriteLine("Started Synching for CorrespondenceCCToUsers Add : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for CorrespondenceCCToUsers Add : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&StatusUID=" + dr["StatusUID"].ToString() +
                            "&UserType=" + dr["UserType"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();


                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateAddFlag(new Guid(dr["UID"].ToString()), "CorrespondenceCCToUsers", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "CorrespondenceCCToUsers Add", "Success", "");
                            Console.WriteLine("Synching for CorrespondenceCCToUsers Add : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "CorrespondenceCCToUsers Add", "Error", ex.Message);
                    }
                }
                //
                //  MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From Tasks where ServerCopiedUpdate='N' and WorkPackageUID='" + wkPkgUID + "'", MyConnection);

                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("Select * From CorrespondenceCCToUsers where ServerCopiedAdd='N' and  ActualDocumentUID in (Select ActualDocumentUID From ActualDocuments Where WorkpackageUID='" + wkPkgUID + "')", MyConnection);




                //
                //if (MyConnection.State == System.Data.ConnectionState.Closed)
                //{
                //    MyConnection.Open();
                //}
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                Console.WriteLine("Started Synching for CorrespondenceCCToUsers Update : ");
                if (MyDataset.Tables[0].Rows.Count == 0)
                {
                    Console.WriteLine("No Records found for CorrespondenceCCToUsers Update : ");
                }
                foreach (DataRow dr in MyDataset.Tables[0].Rows) // get each row form source db and update it to destination db
                {
                    try
                    {
                        postData = "UID=" + dr["UID"].ToString() + "&ActualDocumentUID=" + dr["ActualDocumentUID"].ToString() + "&StatusUID=" + dr["StatusUID"].ToString() +
                            "&UserType=" + dr["UserType"].ToString() + "&CreatedDate=" + dr["CreatedDate"].ToString();



                        data = webPostMethod(postData, URL);
                        if (data.ToString().Contains("true"))
                        {
                            dbutility.updateUpdateFlag(new Guid(dr["UID"].ToString()), "CorrespondenceCCToUsers", "UID");
                            dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "CorrespondenceCCToUsers Update", "Success", "");
                            Console.WriteLine("Synching for CorrespondenceCCToUsers Update : " + dr["UID"].ToString() + " Done");
                        }
                    }
                    catch (Exception ex)
                    {
                        dbutility.InsertintoDbsyncLogs(new Guid(dr["UID"].ToString()), "CorrespondenceCCToUsers Update", "Error", ex.Message);
                    }
                }
                Console.WriteLine("Synching for CorrespondenceCCToUsers Add Done : ");
                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                dbutility.InsertintoDbsyncLogs(Guid.NewGuid(), "CorrespondenceCCToUsers Add/Update", "Error", ex.Message);
                Console.WriteLine(ex.Message);
                //Console.ReadLine();

            }
        }

        private void UpdateDbsync_Status()
        {
            // get the source docs count
            Int64 sourcecount = dbutility.GetStatusDocCount(new Guid(wkPkgUID));
            string URL = serviceURL + "InsertOrUpdateDocCount";
            string postData = "";
            postData = "ProjectUID=" + PrjUID + "&WorkPackageUID=" + wkPkgUID + "&SourceDocCount=" + sourcecount;
            var data = "";
            data = webPostMethod(postData, URL);
            if (data.ToString().Contains("true"))
            {
             
                Console.WriteLine("Synching for Dbsync_Status Done : ");
            }
        }

        public string webPostMethod(string postData, string URL)
        {
            string responseFromServer = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            ((HttpWebRequest)request).UserAgent =
                              "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 7.1; Trident/5.0)";
            request.Accept = "/";
            request.UseDefaultCredentials = true;
            request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        public bool uploaddoc(string filepath)
        {
            string testfile =filepath;// @"~/2bbfa1ef-b427-4e19-add1-97df91390f97/28a6a63b-2573-40a8-bc89-e396c31ce516/CoverLetter/sample cover letter_1.pdf";
            string filename = testfile.Substring(1).Split('/').Last();

            string relativepath = testfile.Substring(1).Replace(filename, "");

            string URL = serviceURL + "UploadDocs";

            string file = SourceDocPath + testfile.Substring(1); // @"D:\app_offline.htm";

            if (!File.Exists(file))
            {
                return false;
            }
            using (var form = new MultipartFormDataContent())
            {
                var Content = new ByteArrayContent(File.ReadAllBytes(file));
                Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(Content, "file", Path.GetFileName(file));
                form.Add(new StringContent(relativepath), "relativepath");
                using (HttpClient client = new HttpClient())
                {
                    //var response = client.PostAsync(@"http://localhost:50385/api/DbSync/UploadDocs", form);
                    var response = client.PostAsync(URL, form);
                    response.Wait();
                    return true;
                }
            }
        }

        public bool UploadReviewFile(string filepath)
        {
            string testfile = filepath;// @"~/2bbfa1ef-b427-4e19-add1-97df91390f97/28a6a63b-2573-40a8-bc89-e396c31ce516/CoverLetter/sample cover letter_1.pdf";
            string filename = testfile.Substring(1).Split('/').Last();

            string relativepath = testfile.ToString().Replace(filename, "");

            string URL = serviceURL + "UploadReviewFile";
            string file = SourceDocPathReviewFile + testfile; // @"D:\app_offline.htm";

            if (!File.Exists(file))
            {
                return false;
            }
            using (var form = new MultipartFormDataContent())
            {
                var Content = new ByteArrayContent(File.ReadAllBytes(file));
                Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(Content, "file", Path.GetFileName(file));
                form.Add(new StringContent(relativepath), "relativepath");
                using (HttpClient client = new HttpClient())
                {
                    // var response = client.PostAsync(@"http://localhost:50385/api/DbSync/UploadReviewFile", form);
                    var response = client.PostAsync(URL, form);
                    response.Wait();
                    return true;
                }
            }
        }

        
    }
}
