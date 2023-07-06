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
        static string phy_path_issue_doc = "D:\\NJS Projects\\2023-02-06_ONTB_STP_Flow_Blob\\Latest-ProjectMonitoring-Tool-Oct-main\\ProjectManagementTool";
        static int count = 0;
        static int gcount = 0;
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
            Console.ReadKey();
        }


        public static Boolean RemoveOfflineDocFiles()
        {
            try
            {
                DBActions db_transact = new DBActions();

                //Delete downloaded docs from _PreviewLoad folder
                string donwloadfolder = phy_path_issue_doc + "\\_PreviewLoad\\";
                Directory.Delete(donwloadfolder, true);
                Directory.CreateDirectory(donwloadfolder);

                RemoveGeneralDocs();

                DataSet ds1 = db_transact.GetWorkPackages();

               
                foreach (DataRow row in ds1.Tables[0].Rows)
                {

                    //RemoveOfflineIssueDocs(row.ItemArray[1].ToString());
                    //RemoveActualDocumenst(row.ItemArray[0].ToString());

                    //RemoveDocumentStatusDocs(row.ItemArray[0].ToString());
                    //RemoveDocumentVersionDocs(row.ItemArray[0].ToString());
                    //RemoveDocumentAtatchmentDocs(row.ItemArray[0].ToString());
                    //RemoveGeneralDocs();


                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static void RemoveOfflineIssueDocs(string workpackage_uid)
        {
            DBActions db_transact = new DBActions();

            string doc_path = "", doc_name = "";

           // string phy_path_issue_doc = "C:\\Users\\SAJI AUGUSTIN\\source\\Latest-ProjectMonitoring-Tool-Oct-main1\\ProjectManagementTool";

            string phy_path = "";

            DataSet ds = null;

            ds = db_transact.GetAllIssueDocsByWorkPackageUID(new Guid(workpackage_uid));


            foreach (DataRow row1 in ds.Tables[0].Rows)
            {
                if (row1.ItemArray[0].ToString() != null)
                {
                    doc_path = row1.ItemArray[1].ToString();
                    doc_name = row1.ItemArray[2].ToString();

                    phy_path = phy_path_issue_doc + doc_path.Replace('/', '\\') + doc_name;

                    if (File.Exists(phy_path))
                    {
                        File.Delete(phy_path);
                    }
                }
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
                        phy_path = phy_path_issue_doc + doc_path.Substring(1).Replace('/', '\\');

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
                            phy_path = phy_path_issue_doc + "\\_modal_pages\\" + doc_path.Replace('/', '\\');
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
                            phy_path = phy_path_issue_doc + doc_path.Substring(1).Replace('/', '\\');

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
                            phy_path = phy_path_issue_doc + doc_path.Substring(1).Replace('/', '\\');
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
                            phy_path = phy_path_issue_doc + doc_path.Substring(1).Replace('/', '\\');
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
                            phy_path = phy_path_issue_doc + doc_path.Substring(1).Replace('/', '\\');
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
                            phy_path = phy_path_issue_doc + doc_path.Substring(1).Replace('/', '\\');
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
    }
}
