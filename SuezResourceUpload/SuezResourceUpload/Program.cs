using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuezResourceUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Daily_labour_Report.xlsx";
            DataTable dt = ReadBOQExcel(fileName, ".xlsx");
        }

        private static DataTable ReadBOQExcel(string fileName, string fileExt)
        {
            DataTable dtData = new DataTable();
            DBUtility dbgetdata = new DBUtility();
            try
            {
                string conn = string.Empty;
                DataTable dtexcel = new DataTable();
                if (fileExt.CompareTo(".xls") == 0)
                    conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                else
                    conn = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'");
                using (OleDbConnection con = new OleDbConnection(conn))
                {
                    try
                    {
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Daily labour Report$]", con); //here we read data from sheet1  
                        oleAdpt.Fill(dtexcel); //fill excel data into dataTable
                        string ResourcedeploymentUID = string.Empty;
                        foreach(DataRow dr in dtexcel.Rows)
                        {
                            //03235d06-cd72-4720-a61c-ee24988593a7 -- live server ;7df92971-052b-44db-b4a8-4f89a105d1af (test server) ,DD2C9A13-FE2D-4FB8-9B47-B6395AD94C36 (local)---
                            ResourcedeploymentUID = dbgetdata.GetResourceDeploymentUID(new Guid("03235d06-cd72-4720-a61c-ee24988593a7"), DateTime.Parse(dr["Date"].ToString()));
                            dbgetdata.ResourceDeployment_Update(Guid.NewGuid(), new Guid(ResourcedeploymentUID), string.IsNullOrEmpty(dr["Labour Strength"].ToString()) ? 0 : float.Parse(dr["Labour Strength"].ToString()),DateTime.Parse(dr["Date"].ToString()),"");
                        }
                        Console.WriteLine("Rows Updated :" + dtexcel.Rows.Count);
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return dtData;
        }
    }
}
