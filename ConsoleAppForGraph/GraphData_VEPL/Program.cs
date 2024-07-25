﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started...");

            if (!UpdatePhysicalProgressGraphData())
            {
                Console.WriteLine("Something went wrong.");
                //Console.ReadKey();
                System.Threading.Thread.Sleep(5000);
            }

            Console.WriteLine("finished...");
            System.Threading.Thread.Sleep(5000);
           // Console.ReadKey();
        }


        public static Boolean UpdatePhysicalProgressGraphData()
        {
            try
            {
                DBActions db_transact = new DBActions();

                DataSet ds = null;
                                
                DataSet ds1  = db_transact.GetWorkPackages();
                //not required...we need to update the tables not delete and insert
               // db_transact.DeleteGraphPhysicalProgressValues();

                foreach (DataRow row in ds1.Tables[0].Rows)
                {
                  //  ds = db_transact.GetTaskScheduleDatesforGraph(new Guid(row.ItemArray[1].ToString()));

                    DateTime startdate = DateTime.Now;
                    DateTime endate = DateTime.Now;
                    DataSet dsWorkpackage = db_transact.GetWorkPackages_By_WorkPackageUID(new Guid("0869D383-8FE0-49F6-8F9D-3EAE38A9F230"));
                    //
                    if(dsWorkpackage.Tables[0].Rows.Count > 0)
                    {
                        startdate = DateTime.Parse(dsWorkpackage.Tables[0].Rows[0]["StartDate"].ToString());
                        endate = DateTime.Parse(dsWorkpackage.Tables[0].Rows[0]["PlannedEndDate"].ToString());

                        startdate = DateTime.Parse(startdate.Month + "/01" + "/" + startdate.Year);
                        if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate1"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate1"].ToString() != "")
                        {
                            endate = DateTime.Parse(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate1"].ToString());
                        }
                        if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate2"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate2"].ToString() != "")
                        {
                            endate = DateTime.Parse(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate2"].ToString());
                        }
                        if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate3"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate3"].ToString() != "")
                        {
                            endate = DateTime.Parse(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate3"].ToString());
                        }
                        if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate4"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate4"].ToString() != "")
                        {
                            endate = DateTime.Parse(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate4"].ToString());
                        }
                    }

                    DataTable table = new DataTable();
                    table.Columns.Add("StartDate", typeof(string));
                    while (startdate <= endate)
                    {

                        table.Rows.Add(startdate);
                        
                        startdate = startdate.AddMonths(1);
                    }
                    ds = new DataSet();
                    ds.Tables.Add(table);

                    DataSet dsvalues1 = null;
                    DataSet dsvalues2 = null;
                    DataSet dsvalues3 = null;
                    decimal planvalue = 0;
                    decimal actualvalue = 0;
                    decimal revisedPlanvalue = 0;
                  //  decimal cumplanvalue = 0;
                   // decimal cumactualvalue = 0;

                    string ValuesString = "";

                    Guid new_id = Guid.NewGuid();

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        planvalue = 0;
                        actualvalue = 0;
                        revisedPlanvalue = 0;

                            dsvalues1 = db_transact.GetTaskScheduleValuesForGraph(new Guid(row.ItemArray[1].ToString()), Convert.ToDateTime(item["StartDate"].ToString()), Convert.ToDateTime(item["StartDate"].ToString()).AddMonths(1));

                            if (dsvalues1.Tables[0].Rows.Count > 0)
                            {
                                planvalue = decimal.Parse(dsvalues1.Tables[0].Rows[0]["TotalSchValue"].ToString());
                               // cumplanvalue += planvalue;
                            }

                            dsvalues2 = db_transact.GetTaskActualValuesForGraph(new Guid(row.ItemArray[1].ToString()), Convert.ToDateTime(item["StartDate"].ToString()), Convert.ToDateTime(item["StartDate"].ToString()).AddMonths(1));
                            
                            if (dsvalues2.Tables[0].Rows.Count > 0)
                            {
                                actualvalue = decimal.Parse(dsvalues2.Tables[0].Rows[0]["TotalAchValue"].ToString());
                               // cumactualvalue += actualvalue;
                            }

                            dsvalues3 = db_transact.GetTaskRevisedScheduleValuesForGraph(new Guid(row.ItemArray[1].ToString()), Convert.ToDateTime(item["StartDate"].ToString()), Convert.ToDateTime(item["StartDate"].ToString()).AddMonths(1));

                            if (dsvalues3.Tables[0].Rows.Count > 0)
                              revisedPlanvalue = decimal.Parse(dsvalues3.Tables[0].Rows[0]["TotalRevSchValue"].ToString());

                        //if (dsvalues3.Tables[0].Rows.Count > 0)
                        //{
                        //    foreach (DataRow r in dsvalues3.Tables[0].Rows)
                        //    {
                        //        revisedPlanvalue = revisedPlanvalue + decimal.Parse(r["TotalRevSchValue"].ToString());
                        //    }
                        //}

                        // cumactualvalue += actualvalue;

                        ValuesString = ValuesString + item["StartDate"].ToString() + "," + planvalue.ToString() + "," + actualvalue.ToString() + "," + revisedPlanvalue.ToString() + ";";

                    }

                    if (ValuesString.Length > 0)
                    {
                        ValuesString = ValuesString.Substring(0, ValuesString.Length - 1);
                    }

                    db_transact.InsertGraphPhysicalProgressValues(new_id, new Guid(row.ItemArray[0].ToString()), new Guid(row.ItemArray[1].ToString()), DateTime.Today.Date, ValuesString);
                    
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

