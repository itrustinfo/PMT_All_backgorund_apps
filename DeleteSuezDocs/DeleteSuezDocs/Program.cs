using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;

namespace DeleteSuezDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pg = new Program();
            pg.StartDelete();
        }

        public void StartDelete()
        {
            try
            {
                DBActions dbaction = new DBActions();
                DataSet ds = new DataSet();
                int count = 0;
                ds = dbaction.GetDashboardContractotDocsSubmitted_Details(new Guid("D7646A77-98F2-4316-9ECC-59ABAC159381"));
                Console.WriteLine("Enter code to go ahead");
                if (Console.ReadLine() == "itrustsuezadmin")
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        count++;
                        int result = dbaction.ActualDocuments_Delete_by_DocID(new Guid(dr["ActualDocumentUID"].ToString()), new Guid("419D71BD-FDD4-4A92-898C-A044BFA7803D"));
                        result = dbaction.Documents_Delete_by_DocID(new Guid(dr["DocumentUID"].ToString()), new Guid("419D71BD-FDD4-4A92-898C-A044BFA7803D"));
                        Console.WriteLine("Done for : " + count);
                    }
                    Console.WriteLine("Completed for : " + count);
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("code is wrong !");
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
