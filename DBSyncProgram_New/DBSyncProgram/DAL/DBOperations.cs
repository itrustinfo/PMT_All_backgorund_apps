using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSyncProgram.DAL
{
    class DBOperations
    {
        public string GetDestinationConnectionString()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\ConnectionDest.txt";
                srFileReader = System.IO.File.OpenText(sFileName);
                sInputLine = srFileReader.ReadLine();

                return sInputLine;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return string.Empty;
            }
        }

        public string GetSourceConnectionString()
        {
            try
            {
                string sFileName = null;
                System.IO.StreamReader srFileReader = null;
                string sInputLine = null;
                sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\ConnectionSource.txt";
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

            public int Submittal_Insert_or_Update_Flow(Guid DocumentUID, Guid WorkPackageUID, Guid ProjectUID, Guid TaskUID, string DocName,
           Guid Doc_Category, string Doc_RefNumber, string Doc_Type, double Doc_Budget, Guid FlowUID, DateTime Flow_StartDate,
           Guid FlowStep1_UserUID, DateTime FlowStep1_TargetDate, string FlowStep2_UserUID, string FlowStep2_TargetDate,
           string FlowStep3_UserUID, string FlowStep3_TargetDate, string FlowStep4_UserUID, string FlowStep4_TargetDate, string FlowStep5_UserUID, string FlowStep5_TargetDate, int EstimatedDocuments, string Remarks, string DocumentSearchType,DateTime CreatedDate,string DeletedFlag)
        {
            
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetDestinationConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("dbsync_InsertOrUpdateDocuments"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@TaskUID", TaskUID);
                        cmd.Parameters.AddWithValue("@DocName", DocName);
                        cmd.Parameters.AddWithValue("@Doc_Category", Doc_Category);
                        cmd.Parameters.AddWithValue("@Doc_RefNumber", Doc_RefNumber);
                        cmd.Parameters.AddWithValue("@Doc_Type", Doc_Type);
                        cmd.Parameters.AddWithValue("@Doc_Budget", Doc_Budget);
                        cmd.Parameters.AddWithValue("@FlowUID", FlowUID);
                        cmd.Parameters.AddWithValue("@Flow_StartDate", Flow_StartDate);
                        cmd.Parameters.AddWithValue("@FlowStep1_UserUID", FlowStep1_UserUID);
                        cmd.Parameters.AddWithValue("@FlowStep1_TargetDate", FlowStep1_TargetDate);
                        if (!string.IsNullOrEmpty(FlowStep2_UserUID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep2_UserUID", new Guid(FlowStep2_UserUID));
                            cmd.Parameters.AddWithValue("@FlowStep2_TargetDate", Convert.ToDateTime(FlowStep2_TargetDate));
                        }
                        if (!string.IsNullOrEmpty(FlowStep3_UserUID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep3_UserUID", FlowStep3_UserUID);
                            cmd.Parameters.AddWithValue("@FlowStep3_TargetDate", FlowStep3_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep4_UserUID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep4_UserUID", FlowStep4_UserUID);
                            cmd.Parameters.AddWithValue("@FlowStep4_TargetDate", FlowStep4_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep5_UserUID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep5_UserUID", FlowStep5_UserUID);
                            cmd.Parameters.AddWithValue("@FlowStep5_TargetDate", FlowStep5_TargetDate);
                        }
                            cmd.Parameters.AddWithValue("@EstimatedDocuments", EstimatedDocuments);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@DocumentSearchType", DocumentSearchType);
                        cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@DeletedFlag", DeletedFlag);
                        con.Open();
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

        public int updateSubmittalAddFlag(Guid DocumentUID)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update Documents set ServerCopiedAdd='Y',CopiedAddDate='" + DateTime.Now + "' Where DocumentUID='" + DocumentUID + "'"))
                    {
                      
                        cmd.Connection = con;
                        con.Open();
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

        public int updateSubmittalUpdateFlag(Guid DocumentUID)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update Documents set ServerCopiedUpdate='Y',CopiedUpdateDate='" + DateTime.Now + "' Where DocumentUID='" + DocumentUID + "'"))
                    {

                        cmd.Connection = con;
                        con.Open();
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

        public int InsertintoDbsyncLogs(Guid UID,string Type,string Result,string Description)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Dbsync_logs]([UID],[Type],[Result],[Description]) " + 
     "VALUES('" + UID +"','" + Type +"','" + Result +"','" + Description +"')"))
                    {

                        cmd.Connection = con;
                        con.Open();
                        sresult = (int)cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
               
            }
            catch (Exception ex)
            {
                return sresult = 0;
            }

            return sresult;
        }

        public int Document_Insert_or_Update_ActualDocuments(Guid ActualDocumentUID, Guid ProjectUID, Guid WorkPackageUID, Guid DocumentUID, string ProjectRef_Number,
         string Ref_Number, string Doc_Type, DateTime IncomingRec_Date, Guid FlowUID, string ActualDocument_Name, string Description, double ActualDocument_Version, string ActualDocument_Type,
         string Media_HC, string Media_SC, string Media_SCEF, string Media_HCR, string Media_SCR, string Media_NA, string ActualDocument_Path, string Remarks,
         string FileRef_Number, string ActualDocument_CurrentStatus,
          string FlowStep1_TargetDate, string FlowStep2_TargetDate, string FlowStep3_TargetDate, string FlowStep4_TargetDate, string FlowStep5_TargetDate, string ActualDocument_Originator, string Document_Date, string ActualDocument_RelativePath, string ActualDocument_DirectoryName, string ActualDocument_CreatedDate,string Delete_Flag)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetDestinationConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("dbsync_InsertOrUpdate_ActualDocuments"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@ActualDocumentUID", ActualDocumentUID);
                        cmd.Parameters.AddWithValue("@ProjectUID", ProjectUID);
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@ProjectRef_Number", ProjectRef_Number);
                        cmd.Parameters.AddWithValue("@Ref_Number", Ref_Number);
                        cmd.Parameters.AddWithValue("@Doc_Type", Doc_Type);
                        cmd.Parameters.AddWithValue("@IncomingRec_Date", IncomingRec_Date);
                        cmd.Parameters.AddWithValue("@FlowUID", FlowUID);
                        cmd.Parameters.AddWithValue("@ActualDocument_Name", ActualDocument_Name);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        cmd.Parameters.AddWithValue("@ActualDocument_Version", ActualDocument_Version);
                        cmd.Parameters.AddWithValue("@ActualDocument_Type", ActualDocument_Type);
                        cmd.Parameters.AddWithValue("@Media_HC", Media_HC);
                        cmd.Parameters.AddWithValue("@Media_SC", Media_SC);
                        cmd.Parameters.AddWithValue("@Media_SCEF", Media_SCEF);
                        cmd.Parameters.AddWithValue("@Media_HCR", Media_HCR);
                        cmd.Parameters.AddWithValue("@Media_SCR", Media_SCR);
                        cmd.Parameters.AddWithValue("@Media_NA", Media_NA);
                        cmd.Parameters.AddWithValue("@ActualDocument_Path", ActualDocument_Path);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@FileRef_Number", FileRef_Number);
                        cmd.Parameters.AddWithValue("@ActualDocument_CurrentStatus", ActualDocument_CurrentStatus);
                        if (!string.IsNullOrEmpty(FlowStep1_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep1_TargetDate", FlowStep1_TargetDate);

                        }
                        if (!string.IsNullOrEmpty(FlowStep2_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep2_TargetDate", FlowStep2_TargetDate);

                        }
                        if (!string.IsNullOrEmpty(FlowStep3_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep3_TargetDate", FlowStep3_TargetDate);

                        }
                        if (!string.IsNullOrEmpty(FlowStep4_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep4_TargetDate", FlowStep4_TargetDate);

                        }
                        if (!string.IsNullOrEmpty(FlowStep5_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep5_TargetDate", FlowStep5_TargetDate);

                        }   
                        cmd.Parameters.AddWithValue("@ActualDocument_Originator", ActualDocument_Originator);
                        if (!string.IsNullOrEmpty(Document_Date))
                        {
                            cmd.Parameters.AddWithValue("@Document_Date", Document_Date);
                        }
                      
                        cmd.Parameters.AddWithValue("@ActualDocument_RelativePath", ActualDocument_RelativePath);
                        cmd.Parameters.AddWithValue("@ActualDocument_DirectoryName", ActualDocument_DirectoryName);
                        
                        cmd.Parameters.AddWithValue("@ActualDocument_CreatedDate", ActualDocument_CreatedDate);
                        cmd.Parameters.AddWithValue("@Delete_Flag", Delete_Flag);
                        con.Open();
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

        public int updateActualDocumentsAddFlag(Guid DocumentUID)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update ActualDocuments set ServerCopiedAdd='Y',CopiedAddDate='" + DateTime.Now + "' Where ActualDocumentUID='" + DocumentUID + "'"))
                    {

                        cmd.Connection = con;
                        con.Open();
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

        public int updateActualDocumentsUpdateFlag(Guid DocumentUID)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update ActualDocuments set ServerCopiedUpdate='Y',CopiedUpdateDate='" + DateTime.Now + "' Where ActualDocumentUID='" + DocumentUID + "'"))
                    {

                        cmd.Connection = con;
                        con.Open();
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

        public int InsertorUpdateDocumentStatus(Guid StatusUID, Guid DocumentUID, double Version, string ActivityType, string Activity_Budget, DateTime ActivityDate,
          string LinkToReviewFile, Guid AcivityUserUID, string Status_Comments, string Current_Status, string Ref_Number, string DocumentDate, string CoverLetterFile,string Delete_Flag)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetDestinationConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("dbsync_InsertorUpdate_DocumentStatus"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@StatusUID", StatusUID);
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@Version", Version);
                        cmd.Parameters.AddWithValue("@ActivityType", ActivityType);
                       
                        cmd.Parameters.AddWithValue("@ActivityDate", ActivityDate);
                        cmd.Parameters.AddWithValue("@LinkToReviewFile", LinkToReviewFile);
                        cmd.Parameters.AddWithValue("@AcivityUserUID", AcivityUserUID);
                        cmd.Parameters.AddWithValue("@Status_Comments", Status_Comments);
                        cmd.Parameters.AddWithValue("@Current_Status", Current_Status);
                        if (!string.IsNullOrEmpty(Ref_Number))
                        {
                            cmd.Parameters.AddWithValue("@Ref_Number", Ref_Number);
                        }
                        if (!string.IsNullOrEmpty(Activity_Budget))
                        {
                            cmd.Parameters.AddWithValue("@Activity_Budget",double.Parse(Activity_Budget));
                        }
                        if (!string.IsNullOrEmpty(DocumentDate))
                        {
                            cmd.Parameters.AddWithValue("@DocumentDate", DocumentDate);
                        }
                        
                        cmd.Parameters.AddWithValue("@CoverLetterFile", CoverLetterFile);
                        cmd.Parameters.AddWithValue("@Delete_Flag", Delete_Flag);
                        con.Open();
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

        public int InsertDocumentorUpdateVersion(Guid DocVersion_UID, Guid DocStatus_UID, Guid DocumentUID, string Doc_Type, string Doc_FileName, string Doc_Comments,int Doc_Version,string Doc_Status,string Doc_StatusDate,string Delete_Flag)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetDestinationConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("dbsync_InsertorUpdate_DocumentVersion"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocVersion_UID", DocVersion_UID);
                        cmd.Parameters.AddWithValue("@DocStatus_UID", DocStatus_UID);
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@Doc_Version", Doc_Version);
                        cmd.Parameters.AddWithValue("@Doc_Type", Doc_Type);
                        cmd.Parameters.AddWithValue("@Doc_FileName", Doc_FileName);
                        cmd.Parameters.AddWithValue("@Doc_Status", Doc_Status);
                        cmd.Parameters.AddWithValue("@Doc_StatusDate", Doc_StatusDate);
                        cmd.Parameters.AddWithValue("@Doc_Comments", Doc_Comments);
                        cmd.Parameters.AddWithValue("@Delete_Flag", Delete_Flag);
                        con.Open();
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


        public int updateAddFlag(Guid UID,string tablename,string primaryKeyname)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update " + tablename + " set ServerCopiedAdd='Y',CopiedAddDate='" + DateTime.Now + "' Where " + primaryKeyname + "='" + UID + "'"))
                    {

                        cmd.Connection = con;
                        con.Open();
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


        public int updateAddFlagInt(int UID, string tablename, string primaryKeyname)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update " + tablename + " set ServerCopiedAdd='Y',CopiedAddDate='" + DateTime.Now + "' Where " + primaryKeyname + "=" + UID + ""))
                    {

                        cmd.Connection = con;
                        con.Open();
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

        public int updateUpdateFlag(Guid UID, string tablename, string primaryKeyname)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update " + tablename + " set ServerCopiedUpdate='Y',CopiedUpdateDate='" + DateTime.Now + "' Where " + primaryKeyname + "='" + UID + "'"))
                    {

                        cmd.Connection = con;
                        con.Open();
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


        public int updateUpdateFlagInt(int UID, string tablename, string primaryKeyname)
        {
            int sresult = 0;
            try

            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("Update " + tablename + " set ServerCopiedUpdate='Y',CopiedUpdateDate='" + DateTime.Now + "' Where " + primaryKeyname + "=" + UID + ""))
                    {

                        cmd.Connection = con;
                        con.Open();
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

        public int InsertorUpdateDocumentflowdata(Guid DocumentFlow_UID, Guid FlowMasterUID, Guid DocumentUID, Guid FlowStep1_UserID, Guid FlowStep1_UserRole, DateTime FlowStep1_TargetDate, string FlowStep1_DisplayName,
            string FlowStep2_UserID, string FlowStep2_UserRole, string FlowStep2_TargetDate, string FlowStep2_DisplayName,
            string FlowStep3_UserID, string FlowStep3_UserRole, string FlowStep3_TargetDate, string FlowStep3_DisplayName,
            string FlowStep4_UserID, string FlowStep4_UserRole, string FlowStep4_TargetDate, string FlowStep4_DisplayName,
            string FlowStep5_UserID, string FlowStep5_UserRole, string FlowStep5_TargetDate, string FlowStep5_DisplayName,
            string FlowStep6_UserID, string FlowStep6_UserRole, string FlowStep6_TargetDate, string FlowStep6_DisplayName,string Delete_Flag)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetDestinationConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("dbsync_InsertorUpdate_Documentflowdata"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocumentFlow_UID", DocumentFlow_UID);
                        cmd.Parameters.AddWithValue("@FlowMasterUID", FlowMasterUID);
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@FlowStep1_UserID", FlowStep1_UserID);
                        cmd.Parameters.AddWithValue("@FlowStep1_UserRole", FlowStep1_UserRole);
                        cmd.Parameters.AddWithValue("@FlowStep1_TargetDate", FlowStep1_TargetDate);
                        cmd.Parameters.AddWithValue("@FlowStep1_DisplayName", FlowStep1_DisplayName);
                        if (!string.IsNullOrEmpty(FlowStep2_UserID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep2_UserID", FlowStep2_UserID);
                        }
                        if (!string.IsNullOrEmpty(FlowStep2_UserRole))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep2_UserRole", FlowStep2_UserRole);
                        }
                        if (!string.IsNullOrEmpty(FlowStep2_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep2_TargetDate", FlowStep2_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep2_DisplayName))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep2_DisplayName", FlowStep2_DisplayName);
                        }
                        //
                        if (!string.IsNullOrEmpty(FlowStep3_UserID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep3_UserID", FlowStep3_UserID);
                        }
                        if (!string.IsNullOrEmpty(FlowStep3_UserRole))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep3_UserRole", FlowStep3_UserRole);
                        }
                        if (!string.IsNullOrEmpty(FlowStep3_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep3_TargetDate", FlowStep3_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep3_DisplayName))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep3_DisplayName", FlowStep3_DisplayName);
                        }
                        //
                        if (!string.IsNullOrEmpty(FlowStep4_UserID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep4_UserID", FlowStep4_UserID);
                        }
                        if (!string.IsNullOrEmpty(FlowStep4_UserRole))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep4_UserRole", FlowStep4_UserRole);
                        }
                        if (!string.IsNullOrEmpty(FlowStep4_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep4_TargetDate", FlowStep4_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep4_DisplayName))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep4_DisplayName", FlowStep4_DisplayName);
                        }
                        //
                        if (!string.IsNullOrEmpty(FlowStep5_UserID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep5_UserID", FlowStep5_UserID);
                        }
                        if (!string.IsNullOrEmpty(FlowStep5_UserRole))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep5_UserRole", FlowStep5_UserRole);
                        }
                        if (!string.IsNullOrEmpty(FlowStep5_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep5_TargetDate", FlowStep5_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep5_DisplayName))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep5_DisplayName", FlowStep5_DisplayName);
                        }
                        //
                        if (!string.IsNullOrEmpty(FlowStep6_UserID))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep6_UserID", FlowStep6_UserID);
                        }
                        if (!string.IsNullOrEmpty(FlowStep6_UserRole))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep6_UserRole", FlowStep6_UserRole);
                        }
                        if (!string.IsNullOrEmpty(FlowStep6_TargetDate))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep6_TargetDate", FlowStep6_TargetDate);
                        }
                        if (!string.IsNullOrEmpty(FlowStep6_DisplayName))
                        {
                            cmd.Parameters.AddWithValue("@FlowStep6_DisplayName", FlowStep6_DisplayName);
                        }
                        cmd.Parameters.AddWithValue("@Delete_Flag", Delete_Flag);
                        con.Open();
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

        public int GetStatusDocCount(Guid WorkPackageUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(GetSourceConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("dbsync_GetStatusDocCount"))
                    {

                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WorkPackageUID", WorkPackageUID);
                        SqlParameter parmOUT = new SqlParameter("@Count", SqlDbType.Int);
                        parmOUT.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(parmOUT);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        int returnVALUE = (int)cmd.Parameters["@Count"].Value;
                        sresult = returnVALUE;
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

        public DataSet getDbsyncSettings()
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(GetSourceConnectionString());
            SqlDataAdapter da = new SqlDataAdapter("Select * From Dbsync_Settings Where WorkPackageName='CP-26'", con);
            da.Fill(ds);
            return ds;
        }

    }
}
