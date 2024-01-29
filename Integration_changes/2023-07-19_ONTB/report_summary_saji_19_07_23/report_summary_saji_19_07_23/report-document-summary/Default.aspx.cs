using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.Configuration;
using ProjectManagementTool.DAL;
using ProjectManagementTool.Models;

namespace ProjectManager._content_pages.report_document_summary
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindView();
                }
            }
        }

        protected void BindView()
        {
            List<tClass2> SummaryList = new List<tClass2>();

            SummaryList.Add(new tClass2 { Project = "CP-02", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "64954970-99dd-4d6b-b379-17df270300ba" });

            SummaryList.Add(new tClass2 { Project = "CP-03", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "d7646a77-98f2-4316-9ecc-59abac159381" });

            SummaryList.Add(new tClass2 { Project = "CP-04", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "5a111cb1-0960-4274-b35e-57a6f5149595" });

            SummaryList.Add(new tClass2 { Project = "CP-07", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "f98ea953-db54-4e16-93a3-09ba0a98d5b1" });

            SummaryList.Add(new tClass2 { Project = "CP-08", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "f98ea953-db54-4e16-93a3-09ba0a98d5b1" });

            SummaryList.Add(new tClass2 { Project = "CP-09", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "2f66efc0-ff27-4cf6-a041-b9b2e05b9217" });

            SummaryList.Add(new tClass2 { Project = "CP-10", Total = 0, Pending = 0, ActionTaken = 0, ProjectId = "7b0c39ce-f72c-4064-a879-609671f5ba27" });

            SummaryList.Add(new tClass2 { Project = "CP-12", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "3810dd45-a4d3-47d6-b85a-f094a0c5b37a" });

            SummaryList.Add(new tClass2 { Project = "CP-13", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "8a8cbabf-fed7-4def-aba4-c97822bff3f9" });


            DataSet ds = null;
            

            foreach (var item in SummaryList)
            {
                ds = getdt.GetFlow2OlddocsCount(item.Project.ToString(), "Pending Documents");
                if (ds != null)  item.Pending = Convert.ToInt32( ds.Tables[0].Rows[0].ItemArray[0].ToString());

                ds = getdt.GetFlow2OlddocsCount(item.Project.ToString(), "Action Taken Documents");
                if (ds != null) item.ActionTaken = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());

                item.Total = item.Pending + item.ActionTaken;
            }

            GrdDocumentSummary.DataSource = SummaryList;

            GrdDocumentSummary.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            ExportGridToPDF();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        private void ExportGridToExcel()
        {
           
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Report__document_summary" + DateTime.Now.Ticks + ".xls";

            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GrdDocumentSummary.GridLines = GridLines.Both;
            GrdDocumentSummary.HeaderStyle.Font.Bold = true;
            GrdDocumentSummary.RenderControl(htmltextwrtter);

            string s = htmltextwrtter.InnerWriter.ToString();

            string x = ""; // WebConfigurationManager.AppSettings["Domain"];
            string y = ""; //Session["Username"].ToString();
            string z = ""; //Request.QueryString["ProjectName"];

            string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + x + " Projectwise Document Summary Report for water projects as on date " + y + " </asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>" + z + "</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";
            Response.Write(HTMLstring);
            Response.End();

        }

        private void ExportGridToPDF()
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "Projectwise Document Summary Report for water projects as on date.";
            cell.ColumnSpan = 4;
            row.Controls.Add(cell);

            row.BackColor = ColorTranslator.FromHtml("#D3D3D3");
            GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(0, row);


            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=document_summary.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GrdDocumentSummary.RenderControl(hw);

            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

    }
}