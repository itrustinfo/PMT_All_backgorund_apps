using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace GarbageCollectorONTB
{
    class Program
    {
        static string phy_path_doc = "D:\\NJS Projects\\2023-02-06_ONTB_STP_Flow_Blob\\Latest-ProjectMonitoring-Tool-Oct-main\\ProjectManagementTool";
        static int count = 0;
        static int gcount = 0;
        //
        static string report_path = AppContext.BaseDirectory.Replace("bin\\Debug\\", "");

        static int total_offline_documents_deleted = 0;
        static int total_offline_issue_documents_deleted = 0;
        static int total_offline_issue_status_documents_deleted = 0;
        static int total_offline_bank_documents_deleted = 0;
        static int total_offline_insurance_documents_deleted = 0;
        static int total_offline_insurance_receipts_deleted = 0;
        static int total_offline_rabills_deleted = 0;
        static int total_offline_photographs_deleted = 0;
        static void Main(string[] args)
        {
             

            Console.WriteLine("started...please wait");

            if (!RemoveOfflineDocFiles())
            {
                Console.WriteLine("Something went wrong.");
                Console.ReadKey();
            }

            Console.WriteLine("finished... docs deleted :" + count);
            Console.WriteLine("finished... General docs deleted :" + gcount);
            //
            Console.WriteLine("Total Issue documents : " + total_offline_issue_documents_deleted);
            Console.WriteLine("Total Issue status Documents : " + total_offline_issue_status_documents_deleted);
            Console.WriteLine("Total Bank Documents : " + total_offline_bank_documents_deleted);
            Console.WriteLine("Total Insurance Documents : " + total_offline_insurance_documents_deleted);
            Console.WriteLine("Total Insurance Receipts : " + total_offline_insurance_receipts_deleted);
            Console.WriteLine("Total RA Bills : " + total_offline_rabills_deleted);
            Console.WriteLine("Total Photographs : " + total_offline_photographs_deleted);
            //
            Console.ReadKey();
        }


        public static Boolean RemoveOfflineDocFiles()
        {
            try
            {
                DBActions db_transact = new DBActions();

                //Delete downloaded docs from _PreviewLoad folder
                string donwloadfolder = phy_path_doc + "\\_PreviewLoad\\";
                Directory.Delete(donwloadfolder, true);
                Directory.CreateDirectory(donwloadfolder);

                RemoveGeneralDocs();

                DataSet ds1 = db_transact.GetWorkPackages();

               
                foreach (DataRow row in ds1.Tables[0].Rows)
                {

                    
                    RemoveActualDocumenst(row.ItemArray[0].ToString());

                    RemoveDocumentStatusDocs(row.ItemArray[0].ToString());
                    RemoveDocumentVersionDocs(row.ItemArray[0].ToString());
                    RemoveDocumentAtatchmentDocs(row.ItemArray[0].ToString());
                    RemoveGeneralDocs();
                    //saji
                    RemoveOfflineIssueDocs(row.ItemArray[1].ToString());

                    RemoveOfflineIssueRemarkDocs(row.ItemArray[1].ToString());

                    RemoveOfflineBankDocs(row.ItemArray[1].ToString());

                    RemoveOfflineInsuranceDocs(row.ItemArray[1].ToString());

                    RemoveOfflineInsuranceReceiptDocs(row.ItemArray[1].ToString());

                    RemoveOfflineRABills(row.ItemArray[1].ToString());

                    RemoveOfflinePhotographs(row.ItemArray[1].ToString());


                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


      

        public static void RemoveActualDocumenst(string projectuid)
        {
            DBActions db_transact = new DBActions();

            string doc_path = "";
            string phy_path = "";

            DataSet ds = null;

            ds = db_transact.garbage_GetAllDocumentsby_ProjectUID(new Guid(projectuid));

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    doc_path = dr["ActualDocument_Path"].ToString();

                    try
                    {
                        phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["ActualDocumentUID"].ToString()), "ActualDocuments", "ActualDocumentUID");
                            count++;
                        }

                        //delete any copy or download files....
                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_copy" + getExtension;

                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                         }
                        //
                        outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void RemoveDocumentStatusDocs(string projectuid)
        {
            DBActions db_transact = new DBActions();

            string doc_path = "";
            string phy_path = "";

            DataSet ds = null;

            ds = db_transact.grbage_GetDocumentStatusDocsby_ProjectUID(new Guid(projectuid));

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    

                    try
                    {
                        if (!string.IsNullOrEmpty(dr["LinkToReviewFile"].ToString()) && dr["LinkToReviewFile"] != DBNull.Value)
                        {
                            doc_path = dr["LinkToReviewFile"].ToString();
                            phy_path = phy_path_doc + "\\_modal_pages\\" + doc_path.Replace('/', '\\');
                        }

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["StatusUID"].ToString()), "DocumentStatus", "StatusUID");
                            count++;
                        }

                        if (!string.IsNullOrEmpty(dr["CoverLetterFile"].ToString()) && dr["CoverLetterFile"] != DBNull.Value)
                        {
                            doc_path = dr["CoverLetterFile"].ToString();
                            phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');

                        }

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["StatusUID"].ToString()), "DocumentStatus", "StatusUID");
                            count++;
                        }
                        //
                        //delete any copy or download files....
                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_copy" + getExtension;

                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                                              }
                        //
                        outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void RemoveDocumentVersionDocs(string projectuid)
        {
            DBActions db_transact = new DBActions();

            string doc_path = "";
            string phy_path = "";

            DataSet ds = null;

            ds = db_transact.grbage_GetDocumentVersionDocsby_ProjectUID(new Guid(projectuid));

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {


                    try
                    {
                        if (!string.IsNullOrEmpty(dr["Doc_FileName"].ToString()) && dr["Doc_FileName"] != DBNull.Value)
                        {
                            doc_path = dr["Doc_FileName"].ToString();
                            phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');
                        }

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion", "DocVersion_UID");
                            count++;
                        }

                        if (!string.IsNullOrEmpty(dr["Doc_CoverLetter"].ToString()) && dr["Doc_CoverLetter"] != DBNull.Value)
                        {
                            doc_path = dr["Doc_CoverLetter"].ToString();
                            phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');
                        }

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["DocVersion_UID"].ToString()), "DocumentVesrion", "DocVersion_UID");
                            count++;
                        }
                        //
                        //delete any copy or download files....
                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_copy" + getExtension;

                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                        //
                        outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void RemoveDocumentAtatchmentDocs(string projectuid)
        {
            DBActions db_transact = new DBActions();

            string doc_path = "";
            string phy_path = "";

            DataSet ds = null;

            ds = db_transact.garbage_GetAllDocumentsAttachmentsby_ProjectUID(new Guid(projectuid));

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {


                    try
                    {
                       

                        if (!string.IsNullOrEmpty(dr["AttachmentFile"].ToString()) && dr["AttachmentFile"] != DBNull.Value)
                        {
                            doc_path = dr["AttachmentFile"].ToString();
                            phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');
                        }

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["AttachmentUID"].ToString()), "DocumentsAttachments", "AttachmentUID");
                            count++;
                        }
                        //delete any copy or download files....
                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_copy" + getExtension;

                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                        //
                        outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void RemoveGeneralDocs()
        {
            DBActions db_transact = new DBActions();

            string doc_path = "";
            string phy_path = "";

            DataSet ds = null;

            ds = db_transact.garbage_GetAll_GeneralDocuments();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {


                    try
                    {


                        if (!string.IsNullOrEmpty(dr["GeneralDocument_Path"].ToString()) && dr["GeneralDocument_Path"] != DBNull.Value)
                        {
                            doc_path = dr["GeneralDocument_Path"].ToString();
                            phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');
                        }

                        if (File.Exists(phy_path))
                        {
                            File.Delete(phy_path);
                            db_transact.updategrabageFlag(new Guid(dr["GeneralDocumentUID"].ToString()), "GeneralDocuments", "GeneralDocumentUID");
                            gcount++;
                        }
                        //delete any copy or download files....
                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_copy" + getExtension;

                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                        //
                        outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        //saji
        public static void RemoveOfflineIssueDocs(string workpackage_uid)
        {
            try
            {


                DBActions db_transact = new DBActions();

                string doc_path = "", doc_name = "";


                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllIssueDocsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();
                        doc_name = row1.ItemArray[2].ToString();

                        phy_path = phy_path_doc + doc_path.Replace('/', '\\') + doc_name;

                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("Issue", Convert.ToInt32(row1.ItemArray[3].ToString()), "");
                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Issue," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_issue_documents_deleted = total_offline_issue_documents_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Issue," + phy_path + ",failure");
                            }

                        }


                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "");

                        outPath = outPath.Replace("_DE", "") + getExtension;


                        if (File.Exists(outPath))
                        {
                            try
                            {
                                File.Delete(outPath);

                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Issue," + outPath + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_issue_documents_deleted = total_offline_issue_documents_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Issue," + outPath + ",failure");
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Issue," + ex.Message + ",failure");
            }
        }

        public static void RemoveOfflineIssueRemarkDocs(string workpackage_uid)
        {
            try
            {


                DBActions db_transact = new DBActions();

                string doc_path = "", doc_name = "";



                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllIssueRemarkDocsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {

                    phy_path = "";

                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();
                        doc_name = row1.ItemArray[2].ToString();

                        doc_path = doc_path.Replace('~', ' ');

                        doc_path = doc_path.Replace('/', '\\');

                        phy_path = phy_path_doc + doc_path + doc_name;

                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("Issue Status", Convert.ToInt32(row1.ItemArray[3].ToString()), "");
                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Issue Status," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_issue_status_documents_deleted = total_offline_issue_status_documents_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Issue Status," + phy_path + ",failure");
                            }

                        }

                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "");

                        outPath = outPath.Replace("_DE", "") + getExtension;

                        if (File.Exists(outPath))
                        {
                            try
                            {
                                File.Delete(outPath);
                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Issue Status," + outPath + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_issue_status_documents_deleted = total_offline_issue_status_documents_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Issue Status," + outPath + ",failure");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Issue Status," + ex.Message + ",failure");
            }
        }

        public static void RemoveOfflineBankDocs(string workpackage_uid)
        {
            try
            {

                DBActions db_transact = new DBActions();

                string doc_path = "", doc_name = "";



                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllBankDocsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    phy_path = "";

                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();
                        doc_path = doc_path.Replace('~', ' ');

                        doc_path = doc_path.Replace('/', '\\');

                        phy_path = phy_path_doc + doc_path;

                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("Bank Document", 0, row1.ItemArray[3].ToString());
                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Bank Document," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_bank_documents_deleted = total_offline_bank_documents_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Bank Document," + phy_path + ",failure");
                            }

                        }

                        //string path = phy_path;
                        //string getExtension = Path.GetExtension(path);
                        //string outPath = path.Replace(getExtension, "") + "_1" + getExtension;

                        //if (File.Exists(outPath))
                        //{
                        //    File.Delete(outPath);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Bank Document," + ex.Message + ",failure");
            }
        }

        public static void RemoveOfflineInsuranceDocs(string workpackage_uid)
        {
            try
            {


                DBActions db_transact = new DBActions();

                string doc_path = "", doc_name = "";



                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllInsuranceDocsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    phy_path = "";

                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();

                        doc_path = doc_path.Replace('~', ' ');
                        doc_path = doc_path.Replace('/', '\\');

                        phy_path = phy_path_doc + doc_path;

                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("Insurance Document", 0, row1.ItemArray[3].ToString());

                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Insurance Document," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_insurance_documents_deleted = total_offline_insurance_documents_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Insurance Document," + phy_path + ",failure");
                            }


                        }

                        //string path = phy_path;
                        //string getExtension = Path.GetExtension(path);
                        //string outPath = path.Replace(getExtension, "") + "_1" + getExtension;

                        //if (File.Exists(outPath))
                        //{
                        //    File.Delete(outPath);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Insurance Document," + ex.Message + ",failure");
            }
        }

        public static void RemoveOfflineInsuranceReceiptDocs(string workpackage_uid)
        {
            try
            {


                DBActions db_transact = new DBActions();

                string doc_path = ""; // doc_name = "";

                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllInsuranceReceiptDocsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    phy_path = "";

                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();

                        doc_path = doc_path.Replace('~', ' ');
                        doc_path = doc_path.Replace('/', '\\');

                        phy_path = phy_path_doc + doc_path;


                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("Insurance Receipt", 0, row1.ItemArray[2].ToString());
                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Premium Receipt," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_insurance_receipts_deleted = total_offline_insurance_receipts_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Premium Receipt," + phy_path + ",failure");
                            }


                        }

                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "") + "_1" + getExtension;

                        if (File.Exists(outPath))
                        {
                            try
                            {
                                File.Delete(outPath);

                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Premium Receipt," + outPath + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_insurance_receipts_deleted = total_offline_insurance_receipts_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Premium Receipt," + outPath + ",failure");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Premium Receipt," + ex.Message + ",failure");
            }
        }

        public static void RemoveOfflineRABills(string workpackage_uid)
        {
            try
            {

                DBActions db_transact = new DBActions();

                string doc_path = "";


                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllRABillsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    phy_path = "";

                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();
                        doc_path = doc_path.Replace('~', ' ');
                        doc_path = doc_path.Replace('/', '\\');

                        phy_path = phy_path_doc + doc_path;

                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("RABill", 0, row1.ItemArray[2].ToString());

                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "RA Bill," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_rabills_deleted = total_offline_rabills_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "RA Bill," + phy_path + ",failure");
                            }

                        }

                        string path = phy_path;
                        string getExtension = Path.GetExtension(path);
                        string outPath = path.Replace(getExtension, "");

                        outPath = outPath.Replace("_DE", "") + getExtension;

                        if (File.Exists(outPath))
                        {
                            try
                            {
                                File.Delete(outPath);

                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "RA Bill," + outPath + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_rabills_deleted = total_offline_rabills_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "RA Bill," + outPath + ",failure");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "RA Bill," + ex.Message + ",failure");
            }
        }

        public static void RemoveOfflinePhotographs(string workpackage_uid)
        {
            try
            {

                DBActions db_transact = new DBActions();

                string doc_path = ""; //, doc_name = "";


                string phy_path = "";

                DataSet ds = null;

                ds = db_transact.GetAllPhotographsByWorkPackageUID(new Guid(workpackage_uid));


                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    phy_path = "";

                    if (row1.ItemArray[0].ToString() != null)
                    {
                        doc_path = row1.ItemArray[1].ToString();
                        doc_path = doc_path.Replace('~', ' ');
                        doc_path = doc_path.Replace('/', '\\');

                        phy_path = phy_path_doc + doc_path;


                        if (File.Exists(phy_path))
                        {
                            try
                            {
                                File.Delete(phy_path);
                                db_transact.UpdateGarbageCollectorFlag("Photograph", 0, row1.ItemArray[3].ToString());

                                File.AppendAllText(report_path + "GarbageCollectionSuccessReport.txt", "Photograph," + phy_path + ",success");
                                total_offline_documents_deleted = total_offline_documents_deleted + 1;
                                total_offline_photographs_deleted = total_offline_photographs_deleted + 1;
                            }
                            catch
                            {
                                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Photograph," + phy_path + ",failure");
                            }

                        }

                        //string path = phy_path;
                        //string getExtension = Path.GetExtension(path);
                        //string outPath = path.Replace(getExtension, "") + "_DE" + getExtension;

                        //if (File.Exists(outPath))
                        //{
                        //    File.Delete(outPath);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText(report_path + "GarbageCollectionFailureReport.txt", "Photograph," + ex.Message + ",failure");
            }

        }
    }
}
