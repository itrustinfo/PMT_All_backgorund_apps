using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP26_Contarctordocscheck
{
    class Program
    {
        static string phy_path_doc = "D:\\NJS Projects\\2023-02-06_ONTB_STP_Flow_Issue\\Latest-ProjectMonitoring-Tool-Oct-main\\ProjectManagementTool";
        static int count = 0;
        static int countfails = 0;
        static int countcoverlttr = 0;
        static int countcoverlttrfails = 0;
        static void Main(string[] args)
        {
            Program pg = new Program();
            pg.startPrg();
            //
            Console.WriteLine("Total doc count :" + count  + ",File not found : " + countfails);
            Console.WriteLine("Total coverletter count :" + countcoverlttr + ",File not found : " + countcoverlttrfails);
            Console.ReadLine();
        }

        private void startPrg()
        {
            //for CP-26
            string doc_path = "";
            string phy_path = "";
            DataSet dsdocs = GetDashboardContractotDocsSubmitted_Details(new Guid("680A1DD9-B46E-4969-8DD6-E3B8F1A1E5DE"));
            if (File.Exists(AppContext.BaseDirectory + "errorcvrletterlog.txt"))
            {
                File.Delete(AppContext.BaseDirectory + "errorcvrletterlog.txt");
            }
            if (File.Exists(AppContext.BaseDirectory + "errordoclog.txt"))
            {
                File.Delete(AppContext.BaseDirectory + "errordoclog.txt");
            }
            foreach (DataRow dr in dsdocs.Tables[0].Rows)
            {
                doc_path = dr["ActualDocument_Path"].ToString();
              
                try
                {
                    phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');

                    if (File.Exists(phy_path))
                    {
                          count++;
                    }
                    else
                    {
                        countfails++;
                        File.AppendAllText(AppContext.BaseDirectory + "errordoclog.txt", "Date:" + DateTime.Now.ToString() + Environment.NewLine);
                        File.AppendAllText(AppContext.BaseDirectory + "errordoclog.txt", "path not found :," + phy_path + " ; UID : " + dr["ActualDocumentUID"].ToString() + Environment.NewLine);
                    }

                    if (dr["CoverLetterUID"] != DBNull.Value || dr["CoverLetterUID"].ToString() != "")
                    {
                        DataSet dscvr = ActualDocuments_SelectBy_ActualDocumentUID(new Guid(dr["CoverLetterUID"].ToString()));
                        if(dscvr.Tables[0].Rows.Count > 0)
                        {
                            doc_path = dr["ActualDocument_Path"].ToString();
                            phy_path = phy_path_doc + doc_path.Substring(1).Replace('/', '\\');

                            if (File.Exists(phy_path))
                            {
                                countcoverlttr++;
                            }
                            else
                            {
                                countcoverlttrfails++;
                                File.AppendAllText(AppContext.BaseDirectory + "errorcvrletterlog.txt", "Date:" + DateTime.Now.ToString() + Environment.NewLine);
                                File.AppendAllText(AppContext.BaseDirectory + "errorcvrletterlog.txt", "path not found :," + phy_path + " ; UID : " + dr["CoverLetterUID"].ToString() + Environment.NewLine);
                            }
                        }
                    }

                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public string GetConnectionString()
        {

            return System.Configuration.ConfigurationManager.ConnectionStrings["PMConnectionString"].ToString();

        }

        public DataSet GetDashboardContractotDocsSubmitted_Details(Guid ProjectUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("usp_GetDashboardContractotDocsSubmitted_Details_fordoccheck", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.CommandTimeout = 900;
                cmd.SelectCommand.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public DataSet ActualDocuments_SelectBy_ActualDocumentUID(Guid ActualDocumentUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                SqlDataAdapter cmd = new SqlDataAdapter("ActualDocuments_SelectBy_ActualDocumentUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@ActualDocumentUID", ActualDocumentUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }
    }
}
