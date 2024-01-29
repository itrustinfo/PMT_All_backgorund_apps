
using ProjectManagementTool.DAL;
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

namespace ProjectManagementTool._modal_pages
{
    public partial class view_data_form : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataSet ds = new DataSet();
        string next = string.Empty;
        string prevstatus = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    GetDataList();
                }
            }
        }

        protected void GetDataList()
        {
            DataSet ds = getdata.GetDataList();

            GrdDataList.DataSource = ds;
            GrdDataList.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            foreach(GridViewRow  item in GrdDataList.Rows)
            {
                TextBox txtBox = item.FindControl("TextBox1") as TextBox;

                getdata.UpdateComment(Convert.ToInt32(item.Cells[0].Text), txtBox.Text);
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        }
    }
}