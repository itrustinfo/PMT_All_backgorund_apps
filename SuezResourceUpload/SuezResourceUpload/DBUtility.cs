using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuezResourceUpload
{
    class DBUtility
    {
        //static string connectionString = @"server=DESKTOP-N0V2F5N\SQLEXPRESS;Database=iPIMS-DM-NJSEI; user id=sa;password=itrust@123;";
        //static string connectionString = @"server=DESKTOP-N0V2F5N\SQLEXPRESS;Database=iPIMS-DM-ONTB; user id=sa;password=itrust@123;";
        //string connectionString = @"Data Source=ONTBSERVER;Initial Catalog=iPIMS-DM-ONTB;Integrated Security=false;User ID=sa;Password=Password@123;";
        //string connectionString = @"server=DESKTOP-N0V2F5N\SQLEXPRESS2014;Database=ProjectManagerServer; user id=sa;password=itrust;";
        // string connectionString = @"server=DESKTOP-KHI3CEQ;Database=iPIMS-DM-ONTB; user id=sa;password=itrust;";
        string connectionString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\connectionString.txt");
        public Boolean InsertorUpdateTaskScheduleVesrion(Guid TaskScheduleVersion_UID, Guid TaskUID, string TaskScheduleType)
        {
            Boolean sresult = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertTaskScheduleVersion"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@TaskScheduleVersion_UID", TaskScheduleVersion_UID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@TaskScheduleType", TaskScheduleType);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        sresult = true;
                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = false;
            }
        }

        public Boolean InsertorUpdateTaskSchedule(Guid TaskScheduleUID, Guid WorkpacageUID, Guid TaskUID, DateTime StartDate, DateTime EndDate, decimal Schedule_Value, string Schedule_Type)
        {
            Boolean sresult = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateTaskSchedule"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@TaskScheduleUID", TaskScheduleUID);
                        cmd.Parameters.AddWithValue("@WorkpacageUID", WorkpacageUID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Schedule_Value", Schedule_Value);
                        cmd.Parameters.AddWithValue("@Schedule_Type", Schedule_Type);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        sresult = true;
                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = false;
            }
        }

        public Boolean InsertorUpdateTaskSchedule_From_Excel(Guid TaskScheduleUID, Guid WorkpacageUID, Guid TaskUID, DateTime StartDate, DateTime EndDate, decimal Schedule_Value, string Schedule_Type, decimal Achieved_Value, DateTime Achieved_Date)
        {
            Boolean sresult = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateTaskSchedule_from_Excel"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@TaskScheduleUID", TaskScheduleUID);
                        cmd.Parameters.AddWithValue("@WorkpacageUID", WorkpacageUID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Schedule_Value", Schedule_Value);
                        cmd.Parameters.AddWithValue("@Schedule_Type", Schedule_Type);
                        cmd.Parameters.AddWithValue("@Achieved_Value", Achieved_Value);
                        cmd.Parameters.AddWithValue("@Achieved_Date", Achieved_Date);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        sresult = true;
                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = false;
            }
        }

        public int InsertorUpdateSubTask_From_Excel_For_ConstructionProgram(Guid TaskUID, Guid WorkPackageUID, Guid ProjectUID, Guid Workpackage_Option, string Owner, string Name, string Description, string Status, Double Basic_Budget,
        Double ActualExpenditure, int TaskLevel, Double GST, Double TotalBudget, Double StatusPer, string Currency, string Currency_CultureInfo, int Task_Order, string ParentTaskID, string Task_Section, string UnitforProgress,
        string startDate, string finishDate, string UnitOfQuantity, string weightage)
        {
            int sresult = 0;
            try
            {
                if (UnitOfQuantity == "")
                {
                    UnitOfQuantity = "0";
                }
                if (weightage == "")
                {
                    weightage = "0";
                }
                weightage =weightage.TrimEnd('%');
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertMainTasks_From_NetworkProgram"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@Workpackage_Option", Workpackage_Option);
                        cmd.Parameters.AddWithValue("@Owner", Owner);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@Basic_Budget", Basic_Budget);
                        cmd.Parameters.AddWithValue("@GST", GST);
                        cmd.Parameters.AddWithValue("@Budget", TotalBudget);
                        cmd.Parameters.AddWithValue("@ActualExpenditure", ActualExpenditure);
                        cmd.Parameters.AddWithValue("@TaskLevel", TaskLevel);
                        cmd.Parameters.AddWithValue("@StatusPer", StatusPer);
                        cmd.Parameters.AddWithValue("@Currency", Currency);
                        cmd.Parameters.AddWithValue("@Currency_CultureInfo", Currency_CultureInfo);
                        cmd.Parameters.AddWithValue("@Task_Order", Task_Order);
                        cmd.Parameters.AddWithValue("@ParentTaskID", ParentTaskID);
                        cmd.Parameters.AddWithValue("@Task_Section", Task_Section);
                        cmd.Parameters.AddWithValue("@UnitforProgress", UnitforProgress);
                        cmd.Parameters.AddWithValue("@UnitOfQuantity", UnitOfQuantity);
                        cmd.Parameters.AddWithValue("@weightage", weightage);
                        if (DateTime.TryParse(startDate, out DateTime start))
                        {
                            cmd.Parameters.AddWithValue("@StartDate", start);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
                        }
                        if (DateTime.TryParse(finishDate, out DateTime finish))
                        {
                            cmd.Parameters.AddWithValue("@FinishDate", finish);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@FinishDate", DBNull.Value);
                        }
                        cmd.CommandTimeout = 900;
                        con.Open();
                        sresult = (int)cmd.ExecuteNonQuery();
                        con.Close();


                    }

                }
            }
            catch (Exception ex)
            {
                LogWrite(ex.Message);
                return sresult = 0;
                
            }
            return sresult;

        }

        public DataSet GetSelectedOption_By_WorkpackageUID(Guid WorkPackageUID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlDataAdapter cmd = new SqlDataAdapter("GetSelectedOption_By_WorkpackageUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

       

        public string GetTaskUID_By_WorkPackageID_TName(Guid WorkPackageUID, string Name)
        {
            string retval = string.Empty;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("GetTaskUID_By_WorkPackageID_TName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Parameters.AddWithValue("@Name", Name);
                retval = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                retval = "";
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retval;
        }

        public string GetTaskLevel_By_WorkPackageID_TName(Guid WorkPackageUID, string Name)
        {
            string retval = string.Empty;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("GetTaskLevel_By_WorkPackageID_TName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                cmd.Parameters.AddWithValue("@Name", Name);
                retval = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                retval = "";
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retval;
        }


        public int GetTaskLevel_By_TaskUID(Guid TaskUID)
        {
            int retval = 0;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetTaskLevelBy_TaskUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                retval = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                retval = 0;
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retval;
        }

        public DataTable GetTaskDetails_TaskUID(string TaskUID)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlDataAdapter cmd = new SqlDataAdapter("usp_getTaskDetails_By_TaskUID", con);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@TaskUID", TaskUID);
                cmd.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        public void LogWrite(string logMessage)
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    w.WriteLine("{0}-{1}", DateTime.Now, logMessage);
                   
                }
            }
            catch (Exception ex)
            {
            }
        }

        internal int UpdateTaskValue(string taskName, string targetValue, Guid projectId,string revised_taskWeightage)
        {
            int retval = 0;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_UpdateTargetValue_Task", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taskName", taskName);
                cmd.Parameters.AddWithValue("@targetValue", targetValue.TrimEnd('%'));
                cmd.Parameters.AddWithValue("@projectId", projectId);
                cmd.Parameters.AddWithValue("@revised_taskWeightage", revised_taskWeightage.TrimEnd('%'));
                retval = (int)cmd.ExecuteNonQuery();
                if(retval>0)
                {
                    LogWrite($"Inserted Data for {taskName} and {targetValue.TrimEnd('%')} and ProjectId {projectId}");
                }
                else
                {
                    LogWrite($"Not Inserted Data for {taskName} and {targetValue.TrimEnd('%')} and ProjectId {projectId}");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                retval = 0;
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retval;
        }

        
        private string GetTaskUID_By_Projectuid_TName(Guid projectuid, string taskName)
        {
            string retval = string.Empty;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("GetTaskUID_By_ProjectID_TName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projectuid", projectuid);
                cmd.Parameters.AddWithValue("@Name", taskName);
                retval = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                retval = "";
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retval;
        }
        public Boolean InsertorUpdateTaskSchedule(Guid TaskScheduleUID, Guid WorkpacageUID, Guid TaskUID, DateTime StartDate, DateTime EndDate, decimal Schedule_Value, string Schedule_Type,string revisedValue)
        {
            Boolean sresult = false;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateTaskSchedule_Revised"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@TaskScheduleUID", TaskScheduleUID);
                        cmd.Parameters.AddWithValue("@WorkpacageUID", WorkpacageUID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@StartDate", StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", EndDate);
                        cmd.Parameters.AddWithValue("@Schedule_Value", Schedule_Value);
                        cmd.Parameters.AddWithValue("@Schedule_Type", Schedule_Type);
                        cmd.Parameters.AddWithValue("@revisedValue", revisedValue);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        sresult = true;
                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = false;
            }
        }

        public int ResourceDeployment_Update(Guid UID, Guid ReourceDeploymentUID, float Deployed, DateTime DeployedDate, string Remarks)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_ResourceDeployment_Update"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@ReourceDeploymentUID", ReourceDeploymentUID);
                        cmd.Parameters.AddWithValue("@Deployed", Deployed);
                        cmd.Parameters.AddWithValue("@DeployedDate", DeployedDate);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        sresult = (int)cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                return sresult;
            }
            catch (Exception ex)
            {
                return sresult = 0;
            }
        }

        public string GetResourceDeploymentUID(Guid ResourceUID,DateTime SelectedMonth)
        {
            string retval = string.Empty;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {

                if (con.State == ConnectionState.Closed) con.Open();
                SqlCommand cmd = new SqlCommand("usp_GetResourceDeploymentUID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ResourceUID", ResourceUID);
                cmd.Parameters.AddWithValue("@SelectedMonth", SelectedMonth);
                retval = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                retval = "";
                if (con.State == ConnectionState.Open) con.Close();
            }
            return retval;
        }
    }
}
