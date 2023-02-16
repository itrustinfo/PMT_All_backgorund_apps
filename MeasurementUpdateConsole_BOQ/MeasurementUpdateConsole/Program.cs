using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementUpdateConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.MeasurementUpdate();
           
        }

        public string GetConnectionString()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Connection.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                // return "Data Source=192.168.1.11\\SQLEXPRESS;Initial Catalog=Healthicare;User ID=sa;Password=itrust";
                // return "Data Source=localhost\\SQL2008;Initial Catalog=ERPDemo;Integrated Security=false;User ID=sa;Password=itrust@123";
                return sInputLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        public string GetWorkPackageUID()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\WorkPackage.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();
                // return "Data Source=192.168.1.11\\SQLEXPRESS;Initial Catalog=Healthicare;User ID=sa;Password=itrust";
                // return "Data Source=localhost\\SQL2008;Initial Catalog=ERPDemo;Integrated Security=false;User ID=sa;Password=itrust@123";
                return sInputLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        private void MeasurementUpdate()
        {
            try
            {
                System.Data.SqlClient.SqlConnection MyConnection = new System.Data.SqlClient.SqlConnection();
                MyConnection.ConnectionString = GetConnectionString();
                System.Data.SqlClient.SqlDataAdapter MyAdapter = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.DataSet MyDataset = new System.Data.DataSet();
                DataSet dsWkpg = new DataSet();
                DataSet dsMailsentdate = new DataSet();
                string WorkPackageUID = GetWorkPackageUID();
                DataSet dsUsers = new DataSet();
                string NextUID = string.Empty;
               
               
              
                System.Data.SqlClient.SqlCommand MyCommand = new System.Data.SqlClient.SqlCommand();
                MyCommand.Connection = MyConnection;
                MyAdapter.SelectCommand = new System.Data.SqlClient.SqlCommand("SELECT MeasurementBook.* FROM MeasurementBook INNER JOIN Tasks ON MeasurementBook.TaskUID = Tasks.TaskUID where MeasurementBook.Delete_Flag = 'N' and (GroupBOQItems='2' or GroupBOQItems='1') and Achieved_Date is not null and WorkPackageUID ='" + WorkPackageUID + "' Order by TaskUID", MyConnection);
                string Frequency = string.Empty;
                if (MyConnection.State == System.Data.ConnectionState.Closed)
                {
                    MyConnection.Open();
                }
                MyDataset.Clear();
                MyAdapter.Fill(MyDataset);
                string FilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs.txt";
                System.IO.StreamWriter testfile = null;
               
                    testfile = new System.IO.StreamWriter(FilePath, true);
                    testfile.WriteLine("======================..");
                    int result = 0;
                if (MyDataset.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("Storing Measurement updates. Please wait...");
                    foreach (System.Data.DataRow MyRow in MyDataset.Tables[0].Rows)
                    {
                        if (NextUID != MyRow["TaskUID"].ToString())
                        {
                            Console.WriteLine("Storing Measurement updates for TaskUID :- " + MyRow["TaskUID"].ToString());
                            testfile.WriteLine(DateTime.Now + "Storing Measurement updates for TaskUID :-" + MyRow["TaskUID"].ToString());
                            result = InsertorUpdateTaskMeasurementBook(Guid.NewGuid(), new Guid(MyRow["TaskUID"].ToString()), MyRow["UnitforProgress"].ToString(), "0", MyRow["Description"].ToString(), DateTime.Now, "", new Guid(MyRow["CreatedByUID"].ToString()), MyRow["Remarks"].ToString(),Convert.ToDateTime(MyRow["Achieved_Date"].ToString()));
                            Console.WriteLine("result :-" + result + " : UID:" + Guid.NewGuid() + ",TaskUID:" +  new Guid(MyRow["TaskUID"].ToString()) + ",UnitforProgress:" + MyRow["UnitforProgress"].ToString() + "Quantity : 0" + ",Description:" + MyRow["Description"].ToString() + ",CreatedByUID:" +  new Guid(MyRow["CreatedByUID"].ToString()) + ",Remarks=" + MyRow["Remarks"].ToString() + ",Achieved Date" + Convert.ToDateTime(MyRow["Achieved_Date"].ToString()));
                            testfile.WriteLine("result :-" + result + " : UID:" + Guid.NewGuid() + ",TaskUID:" + new Guid(MyRow["TaskUID"].ToString()) + ",UnitforProgress:" + MyRow["UnitforProgress"].ToString() + "Quantity : 0" + ",Description:" + MyRow["Description"].ToString() + ",CreatedByUID:" + new Guid(MyRow["CreatedByUID"].ToString()) + ",Remarks=" + MyRow["Remarks"].ToString() + ",Achieved Date" + Convert.ToDateTime(MyRow["Achieved_Date"].ToString()));
                            NextUID = MyRow["TaskUID"].ToString();
                        }
                        else
                        {
                            NextUID = MyRow["TaskUID"].ToString();
                        }

                    }
                }
                testfile.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);
                Console.ReadLine();
            }
        }


        public int InsertorUpdateTaskMeasurementBook(Guid UID, Guid TaskUID, string UnitforProgress, string Quantity, string Description, DateTime CreatedDate, string Upload_File, Guid CreatedByUID, string Remarks, DateTime Achieved_Date)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_InsertorUpdateMeasurementBook_Auto"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@UID", UID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@UnitforProgress", UnitforProgress);
                        cmd.Parameters.AddWithValue("@Quantity", Quantity);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@Upload_File", Upload_File);
                        cmd.Parameters.AddWithValue("@CreatedByUID", CreatedByUID);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@Achieved_Date", Achieved_Date);
                        sresult = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                sresult = 0;
            }
            return sresult;
        }

    }
}
