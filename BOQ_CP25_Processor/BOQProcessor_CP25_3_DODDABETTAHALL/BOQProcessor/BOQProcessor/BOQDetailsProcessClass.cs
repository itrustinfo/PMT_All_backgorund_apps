using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace BOQProcessor
{
    class BOQDetailsProcessClass
    {
        //static string connectionString = @"server=localhost\SQLEXPRESS;Database=ONTB_Arun_11June; user id=sa;password=itrust@123;";
        //static string connectionString = @"server=localhost\SQLEXPRESS2019;Database=iPIMS-DM-ONTB; user id=sa;password=itrust@123;";
        static string connectionString = @"server=ONTBSERVER;Database=iPIMS-DM-ONTB; user id=sa;password=Password@123;";
        int OrderBy = 2000; //Total records 581
        internal void UpdateBOQDetails(string uid, Guid workPackageId, string itemNo, string description, string quantity, string unit,
            string inrRate, string jpyRate, string usdRate, string inrAmount, string jpyAmount, string usdAmount, string parentId, Guid projectuid,
            string typeOfBOQ,int Orderby, string Duties, string ExWorks, string LocalTransport,string GST)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertBOQDetails_CP25"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@uid", uid);
                        cmd.Parameters.AddWithValue("@typeOfBOQ", typeOfBOQ);
                        cmd.Parameters.AddWithValue("@itemNo", itemNo);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@unit", unit);
                        cmd.Parameters.AddWithValue("@inrRate", inrRate);
                        cmd.Parameters.AddWithValue("@jpyRate", jpyRate);
                        cmd.Parameters.AddWithValue("@usdRate", usdRate);
                        cmd.Parameters.AddWithValue("@inrAmount", inrAmount);
                        cmd.Parameters.AddWithValue("@jpyAmount", jpyAmount);
                        cmd.Parameters.AddWithValue("@usdAmount", usdAmount);
                        cmd.Parameters.AddWithValue("@parentId", parentId);
                        cmd.Parameters.AddWithValue("@projectuid", projectuid);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", workPackageId);
                        cmd.Parameters.AddWithValue("@sOrder", Orderby);
                        cmd.Parameters.AddWithValue("@Duties", Duties);
                        cmd.Parameters.AddWithValue("@ExWorks", ExWorks);
                        cmd.Parameters.AddWithValue("@LocalTransport", LocalTransport);
                        cmd.Parameters.AddWithValue("@GST", GST);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //sresult = true;
                    }
                }
                // return sresult;
            }
            catch (Exception ex)
            {
                //  return sresult = false;
                Console.WriteLine("Error :" + ex.Message);
                Console.ReadLine();
            }
        }

        public void processPageData(string pageName, string parentUid, string fileName, string fileExt,Guid projectId,Guid workpackgeId)
        {
            try
            {
                string previouscolumnName = "";
                DataTable dtData = new DataTable();
                BOQDetailsProcessClass boqObj = new BOQDetailsProcessClass();
                string conn = string.Empty;
                Program pgObj = new Program();
                DataTable dtexcel = new DataTable();
               
                if (fileExt.CompareTo(".xls") == 0)
                    conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                else
                    conn = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES'");
                using (OleDbConnection con = new OleDbConnection(conn))
                {
                    try
                    {
                        OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [" + pageName + "$]", con); //here we read data from sheet1  
                        oleAdpt.Fill(dtexcel); //fill excel data into dataTable
                        string columnName = "";
                        bool colummAdded = false;

                        foreach (DataRow dr in dtexcel.Rows)
                        {
                            //if (colummAdded)
                            //{
                            //    for (int i = 0; i < dr.ItemArray.Length; i++)
                            //    {
                            //        if (dr.ItemArray[i].ToString() != "" && dr.ItemArray[0].ToString() !="" && dr.ItemArray[1].ToString() != "")
                            //        {
                            //            columnName = dtData.Columns[i].ColumnName;
                            //            if (columnName.EndsWith(i.ToString()))
                            //            {
                            //                dtData.Columns[i].ColumnName = columnName.Remove(columnName.Length - i.ToString().Length, i.ToString().Length);
                            //            }
                            //            dtData.Columns[i].ColumnName = dr.ItemArray[i].ToString() + "-" + dtData.Columns[i].ColumnName;
                            //        }
                            //    }
                            //    colummAdded = false;
                            //}else
                             if (dtData.Columns.Count == 0)
                            {
                                if (dr.ItemArray[1].ToString() == "Item No.")
                                {
                                    //for (int i = 0; i < dr.ItemArray.Length; i++)
                                    //{

                                    //    if (dr.ItemArray[i].ToString() == "")
                                    //    {
                                    //        dtData.Columns.Add(previouscolumnName + i);
                                    //    }
                                    //    else
                                    //    {
                                    //        previouscolumnName = dr.ItemArray[i].ToString().Trim();
                                    //        dtData.Columns.Add(dr.ItemArray[i].ToString().Trim());
                                    //    }
                                    //}
                                    dtData.Columns.Add("Level");
                                    dtData.Columns.Add("Item No.");
                                    dtData.Columns.Add("Description");
                                    dtData.Columns.Add("Quantity");
                                    dtData.Columns.Add("Unit");
                                    dtData.Columns.Add("Rate");
                                    dtData.Columns.Add("Duties");
                                    dtData.Columns.Add("ExWork");
                                    dtData.Columns.Add("LocalTransport");
                                    dtData.Columns.Add("GST");
                                    dtData.Columns.Add("Amount");
                                    dtData.Columns.Add("uid");
                                    dtData.Columns.Add("parentUid");
                                    colummAdded = true;
                                }
                            }
                            else
                            {
                                if (dr.ItemArray[0].ToString() != "" && dr.ItemArray[1].ToString() != "")
                                {
                                    DataRow dtcolumns = dtData.NewRow();
                                    for (int i = 0; i < 11; i++)
                                    {
                                        dtcolumns[i] = dr.ItemArray[i];
                                    }
                                    dtcolumns["uid"] = Guid.NewGuid();
                                    if (Convert.ToInt32(dtcolumns[0].ToString()) - 1 != 1)
                                    {
                                        dtcolumns["parentUId"] = getParentId(dtData, Convert.ToInt32(dtcolumns[0].ToString()) - 1).ToString();
                                    }
                                    else
                                    {
                                        dtcolumns["parentUId"] = parentUid;
                                    }
                                 
                                    dtData.Rows.Add(dtcolumns);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                UpdateDatatoDatabase(dtData,projectId,workpackgeId);
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateDatatoDatabase(DataTable dtBOQDetails,Guid projectId,Guid workPackageId)
        {
            try
            {
                foreach (DataRow boqRow in dtBOQDetails.Rows)
                {
                    OrderBy += 1;
                    UpdateBOQDetails(boqRow["uid"].ToString(),
                                           workPackageId,
                                           boqRow["Item No."].ToString(),
                                           boqRow["Description"].ToString(),
                                           boqRow["Quantity"].ToString(),
                                           boqRow["Unit"].ToString(),
                                           boqRow["Rate"].ToString(),
                                          "",
                                           "",
                                           boqRow["Amount"].ToString(),
                                          "",
                                           "",
                                           boqRow["parentUid"].ToString(),
                                           projectId, "Detailed Summary"
                                           , OrderBy
                                           , boqRow["Duties"].ToString(), boqRow["ExWork"].ToString(), boqRow["LocalTransport"].ToString(), boqRow["GST"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        
        }

        private object getParentId(DataTable dtData, int level)
        {
            Guid parentId = new Guid();
            for (int i = dtData.Rows.Count - 1; i >= 0; i--)
            {
                if (dtData.Rows[i][0].ToString() == level.ToString())
                {
                    parentId = new Guid(dtData.Rows[i]["uid"].ToString());
                    break;
                }
            }
            return parentId;

        }

    }
}
