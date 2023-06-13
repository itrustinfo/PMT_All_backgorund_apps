using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementToolAutomation
{
    public class DocumentFlow
    {
        private DataTable dtDocStatus;
        GetData getData = null;
        public DocumentFlow()
        {
            getData = new GetData();
        }
        private void ConstructDocumentStatusDt()
        {
            dtDocStatus = new DataTable();
            dtDocStatus.Columns.Add("StatusUID", typeof(Guid));
            dtDocStatus.Columns.Add("DocumentUID", typeof(Guid));
            dtDocStatus.Columns.Add("ActivityType", typeof(string));
            dtDocStatus.Columns.Add("ActivityDate", typeof(DateTime));
            dtDocStatus.Columns.Add("DocumentDate", typeof(DateTime));
            dtDocStatus.Columns.Add("Status_Comments", typeof(string));
        }
        private void AddRowToDocumentStatus(Guid DocumentUID, string ActivityType, string comment, DateTime dateTime)
        {
            if (dtDocStatus == null)
                ConstructDocumentStatusDt();
            DataRow dr = dtDocStatus.NewRow();
            dr["StatusUID"] = Guid.NewGuid();
            dr["DocumentUID"] = DocumentUID;
            dr["ActivityType"] = ActivityType;
            dr["ActivityDate"] = dateTime;
            dr["DocumentDate"] = dateTime;
            dr["Status_Comments"] = comment;

            dtDocStatus.Rows.Add(dr);
        }
        /// <summary>
        /// This function is to get the required data for Network Design DTL Reviewed, ONTB DTL Verified, AE Approval, AEE Approval, EE Approval, ACE Approval. 
        /// It will create a datatable. This datatable will be used to saved the data
        /// </summary>
        public void GetRequiredData(DateTime inputDate)
        {
            if(getData == null)
                getData = new GetData();
            ConstructDocumentStatusDt();
            DataSet dataSet = getData.GetDocumentFlowAutomation();
            if(dataSet != null && dataSet.Tables.Count == 3)
            {
                DataTable dtActualData = dataSet.Tables[0];
                DataTable dtDOcumentDetail = dataSet.Tables[1];

                DataTable dtFlowStatus = dataSet.Tables[2];

                foreach (DataRow drFlow in dtActualData.Rows)
                {
                    Guid FlowUID = new Guid(drFlow["FlowUID"].ToString());
                    string ActualDocumentUID = drFlow["ActualDocumentUID"].ToString();

                    DataTable dtDocumentDetailsForCurrentDocUID = new DataTable();
                    var docUID = dtDOcumentDetail.AsEnumerable().Where(r => r.Field<Guid>("DocumentUID") == new Guid(ActualDocumentUID));
                    if(docUID.Any())
                    {
                        dtDocumentDetailsForCurrentDocUID = docUID.CopyToDataTable();
                    }

                    foreach (DataRow dataRow in dtDocumentDetailsForCurrentDocUID.Rows)
                    {
                        string currentStatus = dataRow["Current_Status"].ToString();

                        DataRow drFlowCurrentStatus = dtFlowStatus.AsEnumerable().Where(r => r.Field<Guid>("FlowUID") == FlowUID && !r.Field<string>("Update_Status").ToLower().Contains("back") && r.Field<string>("Current_Status").Replace(" ", string.Empty) == currentStatus.Replace(" ", string.Empty)).OrderBy(r => r.Field<int>("ForFlow_Step")).FirstOrDefault();
                        if (drFlowCurrentStatus != null)
                        {
                            string updateStatus = drFlowCurrentStatus["Update_Status"].ToString();
                            if (!(currentStatus == "Network Design EE Approval" && updateStatus == "Contractor Submitted 9 Copies"))
                            {
                                string step = drFlowCurrentStatus["ForFlow_Step"].ToString();
                                string colName = "FlowStep" + step + "_TargetDate";
                                var flowStepTargetDate = drFlow[colName].ToString();
                                if (!string.IsNullOrEmpty(flowStepTargetDate))
                                {
                                    if (inputDate >= Convert.ToDateTime(flowStepTargetDate))
                                    {
                                        //added on 14/07/2022 by zuber for speicific condition
                                        if(currentStatus == "Code B-EE Approval")
                                        {
                                            drFlowCurrentStatus["Update_Status"] = "Code B-ACE Approval";
                                        }
                                        else if (currentStatus == "Code A-EE Approval")
                                        {
                                            drFlowCurrentStatus["Update_Status"] = "Code A-ACE Approval";
                                        }
                                        //
                                        AddRowToDocumentStatus(new Guid(ActualDocumentUID), drFlowCurrentStatus["Update_Status"].ToString(), "No Review has been done. Documents moved by the system to next approving user on expiry of time to approve.", inputDate);
                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        /// <summary>
        /// This function is to get the required data for PMC accepted, Work A, Work B and Vendor Flow automation. 
        /// It will create a datatable. This datatable will be used to saved the data
        /// </summary>
        public void GetRequiredDataPMC(DateTime inputDate)
        {   
            getData = new GetData();
            ConstructDocumentStatusDt();
            DataSet dataSet = getData.GetDocumentFlowAutomationPMCAccepted();
            if (dataSet != null && dataSet.Tables.Count == 3)
            {
                DataTable dtActualData = dataSet.Tables[0];
                DataTable dtDOcumentDetail = dataSet.Tables[1];

                DataTable dtFlowStatus = dataSet.Tables[2];

                foreach (DataRow drFlow in dtActualData.Rows)
                {
                    Guid FlowUID = new Guid(drFlow["FlowUID"].ToString());
                    string ActualDocumentUID = drFlow["ActualDocumentUID"].ToString();
                    Guid DocumentUID = new Guid(drFlow["DocumentUID"].ToString());

                    DataTable dtDocumentDetailsForCurrentDocUID = new DataTable();
                    var docUID = dtDOcumentDetail.AsEnumerable().Where(r => r.Field<Guid>("DocumentUID") == new Guid(ActualDocumentUID));
                    if (docUID.Any())
                    {
                        dtDocumentDetailsForCurrentDocUID = docUID.CopyToDataTable();
                    }
                    foreach (DataRow dataRow in dtDocumentDetailsForCurrentDocUID.Rows)
                    {
                        string currentStatus = dataRow["Current_Status"].ToString();

                       // DataRow drFlowCurrentStatus = dtFlowStatus.AsEnumerable().Where(r => r.Field<Guid>("FlowUID") == FlowUID && r.Field<string>("Current_Status").Replace(" ", string.Empty) == currentStatus.Replace(" ", string.Empty)).FirstOrDefault();
                       //changed by zuber on 07/07/2022
                        DataRow drFlowCurrentStatus = dtFlowStatus.AsEnumerable().Where(r => r.Field<Guid>("FlowUID") == FlowUID && r.Field<string>("Current_Status").Replace(" ", string.Empty) == currentStatus.Replace(" ", string.Empty)).OrderBy(r => r.Field<int>("ForFlow_Step")).FirstOrDefault();
                        if (drFlowCurrentStatus != null)
                        {
                            string step = drFlowCurrentStatus["ForFlow_Step"].ToString();
                            string colName = "FlowStep" + step + "_TargetDate";
                            var flowStepTargetDate = drFlow[colName].ToString();
                            string flowName = drFlowCurrentStatus["Flow_Name"].ToString();
                            if (!string.IsNullOrEmpty(flowStepTargetDate))
                            {   
                                if (inputDate >= Convert.ToDateTime(flowStepTargetDate))
                                {
                                    string employeName = string.Empty, discipline = string.Empty, comment = string.Empty;
                                    bool IsWorks = false;
                                    if (flowName.ToLower().Contains("works a"))
                                    {

                                        IsWorks = true;
                                        employeName = getData.GetSubmittedMiltipleUser(DocumentUID, new Guid(ActualDocumentUID), step, currentStatus);
                                        //discipline = getData.GetWorkFlowPackageCategory(FlowUID);
                                        //comment = "Concerned PMC Staff (" + employeName + ") and Discipline(" + discipline + ") has failed to take any action.";
                                        comment = "Concerned PMC Staff (" + employeName + ") has failed to take any action.";
                                    }
                                    else if (flowName.ToLower().Contains("works b") || flowName.ToLower().Contains("vendor approval"))
                                    {
                                        IsWorks = true;
                                        employeName = getData.GetSubmittedMiltipleUser(DocumentUID, new Guid(ActualDocumentUID), step, currentStatus);
                                        comment = "Concerned PMC Staff (" + employeName + ") has failed to take any action.";
                                    }
                                    if (IsWorks)
                                    {
                                        AddRowToDocumentStatus(new Guid(ActualDocumentUID), drFlowCurrentStatus["Update_Status"].ToString(), comment, inputDate);
                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        /// <summary>
        /// This function is to get the required data for STP Work A phase 4 & 5 Flow automation. 
        /// It will create a datatable. This datatable will be used to saved the data
        /// </summary>
        public void GetRequiredDataForStpWorksAStep4And5(DateTime inputDate)
        {
            getData = new GetData();
            ConstructDocumentStatusDt();
            DataSet dataSet = getData.GetDocumentFlowAutomationStpAll();
            
            if (dataSet != null && dataSet.Tables.Count == 3)
            {
                DataTable dtActualData = dataSet.Tables[0];
                DataTable dtDOcumentDetail = dataSet.Tables[1];

                DataTable dtFlowStatus = dataSet.Tables[2];
                List<string> lstForFlowStatus = new List<string> { "4", "5" };
                List<string> lstForFlowStatusWorksB = new List<string> { "4", "5", "11" };

                List<string> lstFlowName = new List<string>() { "Works A", "Vendor Approval" };

                //var worksAdata = dtFlowStatus.AsEnumerable().Where(r => lstFlowName.Contains(r.Field<string>("Flow_Name")) && lstForFlowStatus.Contains(r.Field<int>("ForFlow_Step")));
                
                //if(worksAdata.AsEnumerable().Any())
                //{
                //    dtFlowStatus = worksAdata.CopyToDataTable();
                //}
                foreach (DataRow drFlow in dtActualData.Rows)
                {
                    Guid FlowUID = new Guid(drFlow["FlowUID"].ToString());
                    string ActualDocumentUID = drFlow["ActualDocumentUID"].ToString();
                    Guid DocumentUID = new Guid(drFlow["DocumentUID"].ToString());
                    string ProjectRefNumber = drFlow["ProjectRef_Number"].ToString();
                    string RefNumber = drFlow["Ref_Number"].ToString();
                    string ActualDocument_Name = drFlow["ActualDocument_Name"].ToString();

                    DataTable dtDocumentDetailsForCurrentDocUID = new DataTable();
                    var docUID = dtDOcumentDetail.AsEnumerable().Where(r => r.Field<Guid>("DocumentUID") == new Guid(ActualDocumentUID));
                    if (docUID.Any())
                    {
                        dtDocumentDetailsForCurrentDocUID = docUID.CopyToDataTable();
                    }

                    foreach (DataRow dataRow in dtDocumentDetailsForCurrentDocUID.Rows)
                    {
                        string currentStatus = dataRow["Current_Status"].ToString();

                        List<DataRow> LstdrFlowCurrentStatus = dtFlowStatus.AsEnumerable().Where(r => r.Field<Guid>("FlowUID") == FlowUID && r.Field<string>("Current_Status").Replace(" ", string.Empty) == currentStatus.Replace(" ", string.Empty)).ToList();

                        DataRow drFlowCurrentStatus = dtFlowStatus.AsEnumerable().Where(r => r.Field<Guid>("FlowUID") == FlowUID && r.Field<string>("Current_Status").Replace(" ", string.Empty) == currentStatus.Replace(" ", string.Empty)).OrderBy(r => r.Field<int>("ForFlow_Step")).FirstOrDefault();
                        if (drFlowCurrentStatus != null)
                        {
                            string step = drFlowCurrentStatus["ForFlow_Step"].ToString();
                            string colName = "FlowStep" + step + "_TargetDate";
                            var flowStepTargetDate = drFlow[colName].ToString();
                            string flowName = drFlowCurrentStatus["Flow_Name"].ToString();
                            if (!string.IsNullOrEmpty(flowStepTargetDate))
                            {
                                if (inputDate >= Convert.ToDateTime(flowStepTargetDate))
                                {

                                    string comment = string.Empty;
                                    // this condition is for Works A & Works B. Step 4 & 5
                                    if (lstFlowName.Contains(drFlowCurrentStatus["Flow_Name"].ToString()) && lstForFlowStatus.Contains(step))
                                    {
                                        if (step == "4")
                                            comment = "No Review has been done by project coordinator. Documents moved by the system to next approving user.";
                                        else if (step == "5")
                                            comment = "No Review has been done by DTL. Documents moved by the system to next approving user.";
                                        string nextStatus = GetNextPhaseWorksAVendorApproval(dataRow, drFlowCurrentStatus, LstdrFlowCurrentStatus, ProjectRefNumber, RefNumber, ActualDocument_Name);

                                        if (!string.IsNullOrEmpty(nextStatus))
                                            AddRowToDocumentStatus(new Guid(ActualDocumentUID), nextStatus, comment, inputDate);
                                    }
                                    else if(drFlowCurrentStatus["Flow_Name"].ToString() == "Works B" && lstForFlowStatusWorksB.Contains(step))
                                    {
                                        if (step == "4")
                                            comment = "No Review has been done by project coordinator. Documents moved by the system to next approving user.";
                                        else if (step == "5")
                                            comment = "No Review has been done by by DTL. Documents moved by the system to next approving user.";
                                        else if(step == "11")
                                            comment = "No Review has been done by project coordinator. Documents moved by the system to next approving user.";
                                        string nextStatus = drFlowCurrentStatus["Update_Status"].ToString();
                                        if(step == "5")
                                        {
                                            if (ValidateIfLastStepAutoForwared(dataRow["DocumentUID"].ToString()))
                                            {
                                                SendEmail(currentStatus, drFlowCurrentStatus["FlowUID"].ToString(), drFlowCurrentStatus["ForFlow_Step"].ToString(), ActualDocument_Name, ProjectRefNumber, RefNumber);

                                                nextStatus = string.Empty;
                                            }
                                                
                                        }
                                        if (!string.IsNullOrEmpty(nextStatus))
                                            AddRowToDocumentStatus(new Guid(ActualDocumentUID), nextStatus, comment, inputDate);
                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        private string GetNextPhaseWorksAVendorApproval(DataRow DocumentCurrentRow, DataRow drFlowCurrentStatus, List<DataRow> LstdrFlowCurrentStatus, string ProjectRefNumber, string RefNumber, string ActualDocument_Name)
        {
            string nextStatus = drFlowCurrentStatus["Update_Status"].ToString();
            string currentStatus = DocumentCurrentRow["Current_Status"].ToString();
            if (drFlowCurrentStatus["ForFlow_Step"].ToString() == "4")
            {
                if(LstdrFlowCurrentStatus != null && LstdrFlowCurrentStatus.Count > 0)
                    nextStatus = "No Action by PC";
            }
            else if(drFlowCurrentStatus["ForFlow_Step"].ToString() == "5")
            {
                //Here we need to check if the 4th step has acted or its auto forwared
                //If its autoforwared then no need to act forward. If its not autoforwared in step 4, then we need autoforward here
                if (ValidateIfLastStepAutoForwared(DocumentCurrentRow["DocumentUID"].ToString()))
                {
                    SendEmail(currentStatus, drFlowCurrentStatus["FlowUID"].ToString(), drFlowCurrentStatus["ForFlow_Step"].ToString(), ActualDocument_Name, ProjectRefNumber, RefNumber);

                    return nextStatus = string.Empty;
                }
                if (Constants.Step5CurentNextStatusMap.ContainsKey(currentStatus))
                    nextStatus = Constants.Step5CurentNextStatusMap[currentStatus];
            }

            return nextStatus;
        }
        
        private bool ValidateIfLastStepAutoForwared(string DocumentUID)
        {
            bool IsAutoforwared = false;
            DataSet data = getData.GetDocumentStatus(DocumentUID);
            if (data != null && data.Tables.Count == 1 && data.Tables[0].Rows.Count > 0)
            {
                var LastDocStatus = data.Tables[0].AsEnumerable().OrderByDescending(r => r.Field<DateTime>("CreatedDate")).FirstOrDefault();
                if (LastDocStatus != null)
                {
                    if (LastDocStatus["Forwarded"].ToString() == "Y")
                    {
                        IsAutoforwared = true;
                    }
                }
            }
            return IsAutoforwared;
        }

        private void SendEmail(string currentStatus, string FlowUID, string step, string ActualDocument_Name, string ProjectRefNumber, string RefNumber)
        {
            string RefNostring = string.Empty;
            List<string> emailIds = new List<string>();
            string ToEmailID = string.Empty;
            string CC = string.Empty;
            DataSet ds = getData.GetFlowMasterUser(FlowUID, step);
            Guid UserUID = Guid.Empty;
            if(ds != null && ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    emailIds.Add(dr["EmailID"].ToString());
                    //if (string.IsNullOrEmpty(ToEmailID))
                    //{
                    //    ToEmailID = dr["EmailID"].ToString();
                    //    UserUID = new Guid(dr["UserUID"].ToString());
                    //}
                    //else
                    //    ToEmailID = ToEmailID + "," + dr["EmailID"].ToString();
                }
            }
            emailIds = emailIds.Select(r => r).Distinct().ToList();
            ToEmailID = String.Join(",", emailIds);
            emailIds.Clear();
            DataSet dsCC = getData.GetFlowMasterUser(FlowUID, (Convert.ToInt32(step) - 1).ToString());
            if (dsCC != null && dsCC.Tables.Count == 1)
            {
                foreach (DataRow dr in dsCC.Tables[0].Rows)
                {
                    emailIds.Add(dr["EmailID"].ToString());
                    //if (string.IsNullOrEmpty(CC))
                    //{
                    //    CC = dr["EmailID"].ToString();
                    //}
                    //else
                    //    CC = CC + "," + dr["EmailID"].ToString();
                }
            }
            emailIds = emailIds.Select(r => r).Distinct().ToList();
            CC = String.Join(",", emailIds);

            string sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                      "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                         "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
            sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                               "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
            if (ConfigurationManager.AppSettings["Domain"] == "NJSEI")
            {
                sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                RefNostring = "NJSEI Ref Number";
            }
            else
            {
                sHtmlString += "<div style='float:left; width:7%;'><h2>" + ConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                RefNostring = "ONTB Ref Number";
            }
            sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                       "</div>";
            sHtmlString += "<div style='width:100%; float:left;'><br/>Dear User,<br/><br/><span style='font-weight:bold;'> No Action taken by DTL for Dcouemnt " + ActualDocument_Name + ".</span> <br/><br/></div>";
            sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                            "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + ActualDocument_Name + "</td></tr>" +
                            "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + currentStatus + "</td></tr>" +
                            "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + RefNumber + "</td></tr>" +
                            "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNumber + "</td></tr>" +
                            "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd-MMM-yyyy") + "</td></tr>";
            sHtmlString += "</table></div>";
            sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";

            SendEMail(UserUID, ToEmailID, CC, "No Action taken by DTL for Document", sHtmlString);
        }

        private void SendEMail(Guid UserUID, string ToEmailID, string CC, string header, string body)
        {
            DataTable dtemailCred = getData.GetEmailCredentials();
            Guid MailUID = Guid.NewGuid();
            if (!string.IsNullOrEmpty(ToEmailID))
            {
                getData.StoreEmaildataToMailQueue(MailUID, UserUID, dtemailCred.Rows[0][0].ToString(), ToEmailID, header, body, CC, "");
            }
            
        }

        public void SaveDocumentStatusAutomatic()
        {
            if (getData == null)
                getData = new GetData();
            if (dtDocStatus != null && dtDocStatus.Rows.Count > 0)
            {
                int result = getData.InsertDocumentStatus(dtDocStatus);
                if(result > 0)
                    Console.WriteLine(dtDocStatus.Rows.Count + " documents moved to next set of approvers.");
            }
        }
    }
}
