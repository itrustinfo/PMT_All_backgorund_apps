using System;
using System.Data.OleDb;
using System.Data;

namespace BOQProcessor
{
    class Program
    {
        //local
        // static Guid workPackageId = new Guid("000ED2E2-26A6-4CE0-8364-20F66DEFDF0C");
        //  static Guid projectId = new Guid("5A83781E-14B6-4B84-B472-A8ADF361AF56");
        // server

        static Guid workPackageId = new Guid("879b44c8-c8c7-42d2-aaaa-7a5d957017de");
        static Guid projectId = new Guid("318e3d1c-2d55-4dfd-a1ae-316b10d2c478");
        static void Main(string[] args)
        {
            //string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\RAB 02 - R1.xlsx";
             //string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Bill of Quantities_CP-09.xlsx";
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\BOQ_CP27_ISPS_Hemigepura.xlsx";
            DataTable dt = ReadBOQExcel(fileName, ".xlsx");
        }

        private static DataTable ReadBOQExcel(string fileName, string fileExt)

        {
            DataTable dtData = new DataTable();
            int OrderBy = 8000;
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
                                 "ISPS 2 Hemmigepura ",
                                "ISPS 2 Hemmigepura ",
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
                        Console.WriteLine("Completed cP27 ISPS2 Hemmigepura ");
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
