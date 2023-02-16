using System;
using System.Data.OleDb;
using System.Data;

namespace BOQProcessor
{
    class Program
    {
        //local
        // static   Guid workPackageId = new Guid("000ED2E2-26A6-4CE0-8364-20F66DEFDF0C");
        // static  Guid projectId = new Guid("5A83781E-14B6-4B84-B472-A8ADF361AF56");
        // server
        static Guid workPackageId = new Guid("5124CA05-57F1-4BF2-A4AA-FCEE96642AAF");
        static Guid projectId = new Guid("E12C47F9-F3B5-45D5-98CB-B438F48F32A8");
        static void Main(string[] args)
        {
            //string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\RAB 02 - R1.xlsx";
             //string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Bill of Quantities_CP-09.xlsx";
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\BOQ_CP25_BILISHIVALLI.xlsx";
            DataTable dt = ReadBOQExcel(fileName, ".xlsx");
        }

        private static DataTable ReadBOQExcel(string fileName, string fileExt)

        {
            DataTable dtData = new DataTable();
            int OrderBy = 3000;
            try
            {
                BOQDetailsProcessClass boqObj = new BOQDetailsProcessClass();


                //boqObj.processPageData("SOD", "5B78EE1D-7C39-42EF-88D4-1B5B80F15E0C", fileName, fileExt, projectId, workPackageId);


                string conn = string.Empty;
                DataTable dtexcel = new DataTable();
                if (fileExt.CompareTo(".xls") == 0)
                    conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                else
                    conn = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES'");
                using (OleDbConnection con = new OleDbConnection(conn))
                {
                    try
                    {
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Grand Summary$]", con); //here we read data from sheet1  
                        oleAdpt.Fill(dtexcel); //fill excel data into dataTable
                        Boolean datafound = false;
                        dtData.Columns.Add("Page");
                        dtData.Columns.Add("Bill No.");
                        dtData.Columns.Add("Description");
                        dtData.Columns.Add("Local(INR)-Amount");
                        dtData.Columns.Add("GST");
                        dtData.Columns.Add("Price");
                        dtData.Columns.Add("uid");
                        foreach (DataRow dr in dtexcel.Rows)
                        {
                            if (datafound && dr.ItemArray[0].ToString() != "")
                            {
                                DataRow dtcolumns = dtData.NewRow();
                                for (int i = 0; i < 6; i++)
                                {
                                    dtcolumns[i] = dr.ItemArray[i];
                                }
                                dtcolumns["uid"] = Guid.NewGuid();
                                dtData.Rows.Add(dtcolumns);
                            }
                            else if (dr.ItemArray[0].ToString() == "Page")
                            {
                                datafound = true;

                            }

                        }
                        OrderBy += 1;
                        string parentid = Guid.NewGuid().ToString(); //"F18B7009-8FEA-484B-8130-CACE4EC461A6";
                        boqObj.UpdateBOQDetails(parentid.ToString(),
                                 workPackageId,
                                 "STP 4 BILISHIVALLI",
                                "STP 4 BILISHIVALLI",
                                "",
                                 "",
                                "",
                                 "",
                                 "",
                                 "0",
                                "",
                                 "",
                                 "",
                                 projectId, "Grand Summary", OrderBy,"0", "0", "0","0");
                        foreach (DataRow dr in dtData.Rows)
                        {
                           
                            OrderBy += 1;
                            boqObj.UpdateBOQDetails(dr["uid"].ToString(),
                                       workPackageId,
                                       dr["Bill No."].ToString(),
                                       dr["Description"].ToString(),
                                      "",
                                       "",
                                      "",
                                       "",
                                       "",
                                       dr["Local(INR)-Amount"].ToString(),
                                      "",
                                       "",
                                       parentid,
                                       projectId, "Grand Summary", OrderBy, "0", "0", "0", dr["GST"].ToString());
                            boqObj.processPageData(dr["Page"].ToString(), dr["Uid"].ToString(), fileName, fileExt, projectId, workPackageId);
                        }
                        Console.WriteLine("Completed CP-25 BILISHIVALLI");
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
