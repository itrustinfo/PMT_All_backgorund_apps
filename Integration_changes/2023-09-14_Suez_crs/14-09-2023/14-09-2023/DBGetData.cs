internal int Invoice_Document_InsertUpdate(Guid Document_UID, Guid InvoiceUID, Guid WorkpackageUID, string DocumentPath, Guid UploadedBy, string Description)
        {
            int cnt = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_Invoice_Document_InsertorUpdate"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Document_UID", Document_UID);
                        cmd.Parameters.AddWithValue("@InvoiceUID", InvoiceUID);
                        cmd.Parameters.AddWithValue("@WorkpackageUID", WorkpackageUID);
                        cmd.Parameters.AddWithValue("@Document_Path", DocumentPath);                       
                        cmd.Parameters.AddWithValue("@Uploaded_Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UserID", UploadedBy);
                        cmd.Parameters.AddWithValue("@Description", Description);
                        con.Open();
                        cnt = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                  
                }

                
            }

            catch (Exception ex)
            {
                //  return sresult = false;
            }
            return cnt;

        }

--------------------------------------
internal DataTable GetInvoiceDocuement(Guid InvoiceUID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {
                    SqlDataAdapter cmd = new SqlDataAdapter("usp_Invoice_Document_Select", con);
                    cmd.SelectCommand.Parameters.AddWithValue("@InvoiceUID", InvoiceUID);
                    cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                    cmd.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

--------------------------------------------

 public int InvoiceDocuement_Delete(Guid DocumentUID, Guid UserUID)
        {
            int sresult = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConnectionString()))
                {

                    using (SqlCommand cmd = new SqlCommand("usp_Invoice_Document_Delete"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@DocumentUID", DocumentUID);
                        cmd.Parameters.AddWithValue("@UserUID", UserUID);
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