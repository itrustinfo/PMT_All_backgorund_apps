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

        static void Main(string[] args)
        {
             

            Console.WriteLine("started...");

            if (!RemoveOfflineDocFiles())
            {
                Console.WriteLine("Something went wrong.");
                Console.ReadKey();
            }

            Console.WriteLine("finished...");
            Console.ReadKey();
        }


        public static Boolean RemoveOfflineDocFiles()
        {
            try
            {
                DBActions db_transact = new DBActions();

                DataSet ds1 = db_transact.GetWorkPackages();

               
                foreach (DataRow row in ds1.Tables[0].Rows)
                {

                    //RemoveOfflineIssueDocs(row.ItemArray[1].ToString());
                    RemoveActualDocumenst(row.ItemArray[0].ToString());
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
