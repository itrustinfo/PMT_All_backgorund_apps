using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSTP_Submittals_Add
{
    class Program
    {
        static Guid workPackageId = new Guid("5124CA05-57F1-4BF2-A4AA-FCEE96642AAF");
        static Guid projectId = new Guid("E12C47F9-F3B5-45D5-98CB-B438F48F32A8");
        static void Main(string[] args)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\MDL 150 MLD STP V Valley-R1_new.xlsx";
            Program pg = new Program();
            pg.ReadExcel(fileName, ".xlsx");
        }


        private void ReadExcel(string fileName, string fileExt)
        {
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
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [MDL 150 MLD STP V Valley-R1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel);

                    string refno = string.Empty;
                    string SubmittalName = string.Empty;
                    string TaskUID = string.Empty;
                    string CategoryUID = string.Empty;
                    string TargetDate = string.Empty;
                    foreach(DataRow dr in dtexcel.Rows)
                    {
                        if(dr[0].ToString() == "Y")
                        {
                            refno = dr[2].ToString()  + dr[3].ToString() +  dr[4].ToString()  + dr[5].ToString()  + "/" + dr[6].ToString() + "/"  + dr[7].ToString();
                            SubmittalName = dr[8].ToString();
                            TaskUID = dr[11].ToString();
                            CategoryUID = dr[12].ToString();
                            TargetDate = dr[13].ToString();
                            //
                            Guid SubmittalUID = Guid.NewGuid();
                            addsubmittal(SubmittalUID, refno,SubmittalName,new Guid(TaskUID), new Guid(CategoryUID),TargetDate);
                            //
                            addsubmittalUsers(SubmittalUID);
                        }
                        else
                        {
                            string str = dr[0].ToString();
                            string test = "";
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             }
        }

        public int addsubmittal(Guid DocumentUID,string DocRefNumber,string SubmittalName,Guid TaskUID,Guid CategoryUID,string TargetDate)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_VVSTP_Submittaladd"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@DocRefNumber", DocRefNumber);
                        cmd.Parameters.AddWithValue("@SubmittalName", SubmittalName);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@CategoryUID", CategoryUID);
                        if(!string.IsNullOrEmpty(TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@Step1TargetDate", TargetDate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Step1TargetDate", DateTime.Now.AddDays(1));
                        }
                        con.Open();
                        cnt = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }

        public int addsubmittalUsers(Guid SubmittalUID)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_addVSTPsubmittal_Users"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                       
                        cmd.Parameters.AddWithValue("@SubmittalUID", SubmittalUID);
                        con.Open();
                        cnt = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }

        public string GetConnectionString()
        {

            return System.Configuration.ConfigurationManager.ConnectionStrings["PMConnectionString"].ToString();

        }


    }
}
