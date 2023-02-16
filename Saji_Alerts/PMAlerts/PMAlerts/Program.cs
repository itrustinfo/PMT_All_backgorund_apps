using System;
using System.Data;
using System.Threading;

namespace PMAlerts
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //string msg = "Enter a number (0/1/2/3/4) 1.Tasks 2. Bank Guarantees 3. Insurances 4. Issues ";
            //int choice = 0;
            //Console.WriteLine(msg);
            
            //choice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("started...");

            //if (!GetAllTasksForAlerts())
            //{
            //    Console.WriteLine("Something went wrong with tasks");
            //    Console.ReadKey();
            //}
  
            if (!InsertBankGuaranteeIntoAlerts())
            {
                Console.WriteLine("Something went wrong with Bank Guarantee");
                Console.ReadKey();
            }
                
            if (!InsertInsuranceIntoAlerts())
            {
                Console.WriteLine("Something went wrong with Insurance Policy");
                Console.ReadKey();
            }
 
            if (!InsertIssueIntoAlerts())
            {
                Console.WriteLine("Something went wrong with Issues");
                Console.ReadKey();
            }
                

            //switch (choice)
            //{
            //    case 1:
            //        GetAllTasksForAlerts();
            //        Console.WriteLine("Tasks...added.");
            //        break;
            //    case 2:
            //        InsertBankGuaranteeIntoAlerts();
            //        Console.WriteLine("Bank Guarantees...added.");
            //        break;
            //    case 3:
            //        InsertInsuranceIntoAlerts();
            //        Console.WriteLine("Insurance Policies...added.");
            //        break;
            //    case 4:
            //        InsertIssueIntoAlerts();
            //        Console.WriteLine("Issues...added.");
            //        break;
            //}


            Console.WriteLine("finished...");
            Console.ReadKey();


        }

        public static  Boolean GetAllTasksForAlerts()
        {
            try 
            {
                DBActions sAction = new DBActions();
           
            #region old code arun
            //DataSet ds = sAction.GetWorkPackages_By_ProjectUID(new Guid("B179FA6D-3D76-46C9-BD5C-FCB5417240C7"));
            ////DataSet ds = sAction.GetWorkPackages_By_ProjectUID(new Guid("241221F9-1AAB-4306-B9A6-7522F13CF523"));
            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        Console.WriteLine("Processing WorkPackage : " + ds.Tables[0].Rows[i]["Name"].ToString());
            //        string Error = string.Empty;
            //        try
            //        {
            //            DataSet ds1 = sAction.GetAllDelayedTasks_by_WorkPackageUID(new Guid(ds.Tables[0].Rows[i]["WorkPackageUID"].ToString()));
            //            if (ds1.Tables[0].Rows.Count > 0)
            //            {
            //                Console.WriteLine("Total Delayed task count: " + ds1.Tables[0].Rows.Count);
            //                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
            //                {
            //                    Console.WriteLine("Procesing task : " + ds1.Tables[0].Rows[j]["Name"].ToString());
            //                    if (ds1.Tables[0].Rows[j]["Owner"].ToString() != "")
            //                    {
            //                        string sHtmlString = "";
            //                        string CC = sAction.getUserEmails(new Guid(ds1.Tables[0].Rows[j]["TaskUID"].ToString()));
            //                        if (CC != "")
            //                        {
            //                            CC = CC.TrimEnd(',');
            //                        }

            //                        //string TaskNames = sAction.getTaskParents(new Guid(ds1.Tables[0].Rows[j]["TaskUID"].ToString())) + ds1.Tables[0].Rows[j]["Name"].ToString();
            //                        string TaskNames = ds1.Tables[0].Rows[j]["Name"].ToString();
            //                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
            //                           "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
            //                              "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
            //                        sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
            //                                           "Dear, " + "User" +
            //                                           "<br/><br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are delayed Task details. <br/><br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>WorkPackage : " + sAction.getWorkPackageNameby_WorkPackageUID(new Guid(ds1.Tables[0].Rows[j]["WorkPackageUID"].ToString())) + "<br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Task Name : " + TaskNames + "<br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Percentage Done : " + ds1.Tables[0].Rows[j]["StatusPer"].ToString() + "%<br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Start Date : " + ds1.Tables[0].Rows[j]["StartDate"].ToString() + "<br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Planned EndDate : " + ds1.Tables[0].Rows[j]["PlannedEndDate"].ToString() + "<br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Projected EndDate : " + ds1.Tables[0].Rows[j]["ProjectedEndDate"].ToString() + "<br/></div>";
            //                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager.</div></div></body></html>";

            //                        Console.WriteLine("Inserting task : " + ds1.Tables[0].Rows[j]["Name"].ToString());
            //                        int Cnt = sAction.InsertAlerts(Guid.NewGuid(), new Guid(ds1.Tables[0].Rows[j]["TaskUID"].ToString()), "Schedule Date for Task : " + TaskNames + " was " + Convert.ToDateTime(ds1.Tables[0].Rows[j]["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy") + ". It has been Delayed.", DateTime.Now, new Guid(ds1.Tables[0].Rows[j]["Owner"].ToString()), new Guid(ds.Tables[0].Rows[i]["ProjectUID"].ToString()), new Guid(ds.Tables[0].Rows[i]["WorkPackageUID"].ToString()));
            //                        if (Cnt > 0)
            //                        {
            //                            Console.WriteLine("Inserted");
            //                            //string retStatus = sAction.SendMail(sAction.getUserEmail_by_UserUID(new Guid(ds1.Tables[0].Rows[j]["Owner"].ToString())), TaskNames + " Task alert", sHtmlString, CC, "");
            //                            //if (retStatus == "Success")
            //                            //{

            //                            //}
            //                        }
            //                        else
            //                        {
            //                            int sendcount = sAction.checkAlertMailSent(new Guid(ds1.Tables[0].Rows[j]["TaskUID"].ToString()));
            //                            if (sendcount > 0)
            //                            {
            //                                //string retStatus = sAction.SendMail(sAction.getUserEmail_by_UserUID(new Guid(ds1.Tables[0].Rows[j]["Owner"].ToString())), TaskNames + " Task alert", sHtmlString, CC, "");
            //                                //if (retStatus == "Success")
            //                                //{

            //                                //}
            //                            }
            //                            //DataSet dsAlert = sAction.getAlertLevel(new Guid(ds1.Tables[0].Rows[j]["TaskUID"].ToString()));
            //                            //if (dsAlert.Tables[0].Rows.Count > 0)
            //                            //{

            //                            //}
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                Thread.Sleep(10000);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Error = ex.Message;
            //            Console.WriteLine("Error: " + ex.Message);
            //            Thread.Sleep(10000);
            //        }

            //    }
            //}
            #endregion

                DataTable dt = new DataTable();
                DataTable dtmilestone = new DataTable();

                dt = sAction.getAllTasksForAlert();

                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToDateTime(dr["PlannedStartDate"]) < DateTime.Now && dr["Status"].ToString() == "P")
                    {
                        Console.WriteLine("Processing task : " + dr["Name"].ToString() + ": Start has been delayed");
                        Console.WriteLine("Inserting task : " + dr["Name"].ToString());
                        int Cnt = sAction.InsertAlerts(Guid.NewGuid(), new Guid(dr["TaskUID"].ToString()), "Schedule Start Date for Task : " + dr["Name"].ToString() + " was " + Convert.ToDateTime(dr["PlannedStartDate"].ToString()).ToString("dd/MM/yyyy") + ". It has been Delayed.", DateTime.Now, new Guid(dr["Owner"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), "P");
                        if (Cnt > 0)
                        {
                            Console.WriteLine("Alert Inserted for task " + dr["Name"].ToString());
                            Thread.Sleep(1000);
                        }
                        // Check for milestone Completion
                    }
                    else if (Convert.ToDateTime(dr["PlannedEndDate"]) < DateTime.Now && dr["Status"].ToString() == "I")
                    {
                        Console.WriteLine("Processing task : " + dr["Name"].ToString() + ": Start has been delayed");
                        Console.WriteLine("Inserting task : " + dr["Name"].ToString());
                        int Cnt = sAction.InsertAlerts(Guid.NewGuid(), new Guid(dr["TaskUID"].ToString()), "Schedule End Date for Task : " + dr["Name"].ToString() + " was " + Convert.ToDateTime(dr["PlannedEndDate"].ToString()).ToString("dd/MM/yyyy") + ". It has been Delayed.", DateTime.Now, new Guid(dr["Owner"].ToString()), new Guid(dr["ProjectUID"].ToString()), new Guid(dr["WorkPackageUID"].ToString()), "I");
                        if (Cnt > 0)
                        {
                            Console.WriteLine("Alert Inserted for task " + dr["Name"].ToString());
                            Thread.Sleep(1000);
                        }
                        // Check for milestone Completion
                    }

                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static  Boolean InsertBankGuaranteeIntoAlerts()
        {
            try
            {
                DBActions db_transact = new DBActions();
                DataTable dt = db_transact.getAllBankGuarantees();

                string alert_msg = "";
                               

                foreach (DataRow item in dt.Rows)
                {
                    Guid new_id = Guid.NewGuid();
                    alert_msg = "Bank guarantee (" + item.ItemArray[3].ToString() + ") expiring on " + Convert.ToDateTime(item.ItemArray[4].ToString()).ToString("dd/MM/yyyy");
                    db_transact.InsertAlerts(new_id, new Guid(item.ItemArray[2].ToString()), alert_msg, DateTime.Today.Date, new Guid("419D71BD-FDD4-4A92-898C-A044BFA7803D"), new Guid(item.ItemArray[0].ToString()), new Guid(item.ItemArray[1].ToString()), "");
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static  Boolean InsertInsuranceIntoAlerts()
        {
            try
            {
                DBActions db_transact = new DBActions();
                DataTable dt = db_transact.getAllInsurances();

                string alert_msg = "";
               

                foreach (DataRow item in dt.Rows)
                {
                    Guid new_id = Guid.NewGuid();
                    alert_msg = "Insurance Policy No (" + item.ItemArray[0].ToString() + ") of " + item.ItemArray[1].ToString() + " maturing on " + Convert.ToDateTime(item.ItemArray[5].ToString()).ToString("dd/MM/yyyy");

                    db_transact.InsertAlerts(new_id, new Guid(item.ItemArray[2].ToString()), alert_msg, DateTime.Today.Date, new Guid("419D71BD-FDD4-4A92-898C-A044BFA7803D"), new Guid(item.ItemArray[3].ToString()), new Guid(item.ItemArray[4].ToString()), "");
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static  Boolean InsertIssueIntoAlerts()
        {
            try
            {
                DBActions db_transact = new DBActions();
                DataTable dt = db_transact.getAllIssues();

                string alert_msg = "";
               

                foreach (DataRow item in dt.Rows)
                {
                    Guid new_id = Guid.NewGuid();

                    alert_msg = "An issue (" + item.ItemArray[4].ToString() + ") is added on " + Convert.ToDateTime(item.ItemArray[5].ToString()).ToString("dd/MM/yyyy") ;

                    db_transact.InsertAlerts(new_id, new Guid(item.ItemArray[1].ToString()), alert_msg, DateTime.Today.Date, new Guid("419D71BD-FDD4-4A92-898C-A044BFA7803D"), new Guid(item.ItemArray[2].ToString()), new Guid(item.ItemArray[3].ToString()), "");
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
