using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using ceTe.DynamicPDF.Rasterizer;
using GemBox.Pdf;
using GemBox.Document;
using Syncfusion.Compression.Zip;
using System.IO.Compression;
using ImageSaveFormat = GemBox.Pdf.ImageSaveFormat;
using GemBox.Pdf.Content;
using ProjectManagementTool.Models;

namespace ProjectManagementTool._content_pages.doctobrowser
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        DataSet ds = new DataSet();
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
                    BindIssues();
                    BindIssueStatus();

                   // Session["issue_status_preview"] = "yes";
                   
                        if (Request.QueryString["Issue_Uid"] != null)
                        {
                            DataSet ds = getdt.GetUploadedIssueImages(Request.QueryString["Issue_Uid"]);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                GrdIssueImages.DataSource = null;
                                GrdIssueImages.DataBind();

                                ImageField img = new ImageField();
                                img.HeaderText = "Issue Attached Images";
                                img.DataImageUrlField = "IssueImage";//Your Column Name Representing the image.

                                GrdIssueImages.Columns.Add(img);

                                GrdIssueImages.DataSource = ds;
                                GrdIssueImages.DataBind();
                                GrdIssueImages.HeaderRow.Visible = false;
                            }

                        DataSet ds1 = getdt.GetUploadedIssueStatusImagesByIssue_id(Request.QueryString["Issue_Uid"]);

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            GrdIssueStatusImages.DataSource = null;
                            GrdIssueStatusImages.DataBind();

                            ImageField img = new ImageField();
                            img.HeaderText = "Issue Attached Images";
                            img.DataImageUrlField = "IssueImage";//Your Column Name Representing the image.

                            GrdIssueStatusImages.Columns.Add(img);

                            GrdIssueStatusImages.DataSource = ds1;
                            GrdIssueStatusImages.DataBind();
                            GrdIssueStatusImages.HeaderRow.Visible = true;
                        }


                        }
                   
                }

                lbl1.Text = "ONTB-BWSSB Stage 5 Project Monitoring Tool";
                lbl2.Text = "Report Name: Issues (" +  DateTime.Now.Date.ToShortDateString() + ")";
                lbl3.Text = "Project Name :" + Session["ProjectName"].ToString();

                GrdPrint.Columns[7].Visible = false;
                gvIssueRemarks.Columns[3].Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "PrintDiv('imgDiv');", true);
            }
        }


        protected void BindIssues()
        {
                DataSet ds = getdt.getIssue_by_IssueId(new Guid(Request.QueryString["Issue_Uid"].ToString()));
                GrdPrint.DataSource = ds;
                GrdPrint.DataBind();
        }

        private void BindIssueStatus()
        {
            DataSet ds = getdt.GetIssueStatus_by_Issue_Uid(new Guid(Request.QueryString["Issue_Uid"].ToString()));
            gvIssueRemarks.DataSource = ds;
            gvIssueRemarks.DataBind();
        }

        private void BindIssueStatus(GridView gvRemarks)
        {
            DataSet ds = getdt.GetIssueStatus_by_Issue_Uid(new Guid(Request.QueryString["Issue_Uid"].ToString()));
            gvRemarks.DataSource = ds;
            gvRemarks.DataBind();
        }

        protected void GrdPrint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ds.Clear();
                ds = getdt.getUserDetails(new Guid(e.Row.Cells[2].Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                }

                string IssueUID = e.Row.Cells[7].Text;
                //GridView gvIssueRemarks = e.Row.FindControl("gvIssueRemarks") as GridView;
                //BindIssueStatus(IssueUID, gvIssueRemarks);


                GridView gvDocs = e.Row.FindControl("gvUploadedDocs") as GridView;
                BindUploadedDocuments(gvDocs);

                //foreach (GridViewRow r in gvIssueRemarks.Rows)
                //{
                //    GridView gvRemDocs = r.FindControl("gvUploadedRemDocs") as GridView;
                //    BindUploadedRemDocuments(r.Cells[3].Text, gvRemDocs);
                //}

            }
        }

        protected void GrdIssueRemarks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                GridView gvRemDocs = e.Row.FindControl("gvUploadedRemDocs") as GridView;
                BindUploadedRemDocuments(e.Row.Cells[3].Text, gvRemDocs);

            }
        }

        private void BindIssueStatus(string IssueId, GridView gvRemarks)
        {
            DataSet ds = getdt.GetIssueStatus_by_Issue_Uid(new Guid(IssueId));
            gvRemarks.DataSource = ds;
            gvRemarks.DataBind();
        }

        private void BindUploadedDocuments(GridView gvDocs)
        {
            DataSet ds1 = getdt.GetUploadedIssueDocuments(Request.QueryString["Issue_Uid"].ToString());
            gvDocs.DataSource = ds1;
            gvDocs.DataBind();
        }

        private void BindUploadedRemDocuments(string IssueRemId, GridView gvRemDocs)
        {
            DataSet ds = getdt.GetUploadedDocuments(IssueRemId);
            gvRemDocs.DataSource = ds;
            gvRemDocs.DataBind();
        }

        protected void GrdIssueImages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               

                Image issue_img = (Image)e.Row.Cells[0].Controls[0];

                string path = Server.MapPath(issue_img.ImageUrl);

                string Extension = Path.GetExtension(path);

               

                if (Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg" || Extension == ".bmp")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdt.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    issue_img.ImageUrl = fname;

                    issue_img.Attributes.Add("width", "100%");
                    issue_img.Attributes.Add("height", "620");
                    //issue_img.Attributes.Add("class", "img-fluid");
                }
                else if (Extension == ".pdf")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdt.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    string stringPath = Server.MapPath(fname);

                    //string zipFile = Server.MapPath("/Documents/Issues/Output.zip");

                    //if (File.Exists(zipFile)) File.Delete(zipFile);

                    //ConvertPdfToImages(stringPath);

                    //string zipPath = Server.MapPath("/Documents/Issues/Output.zip");
                    //string extractPath = Server.MapPath("/Documents/Issues/Output");

                    //if (Directory.Exists(extractPath)) DeleteDirectory(extractPath);

                    //ZipFile.ExtractToDirectory(zipPath, extractPath);

                    GridView pdfGridView = new GridView();

                    ImageField imgFld = new ImageField();

                    imgFld.HeaderText = "Image";
                    imgFld.DataImageUrlField = "ImgUrl";

                    pdfGridView.Columns.Add(imgFld);

                    List<imgClass> imgList = new List<imgClass>();

                    //foreach (string imgObj in Directory.GetFiles(extractPath))
                    //{
                    //    imgList.Add(new imgClass() { ImgUrl = "/Documents/Issues/Output/" + Path.GetFileName(imgObj) });
                    //}

                    PdfRasterizer rasterizer = new PdfRasterizer(stringPath);

                    string jpgFile = "";

                    for (int i = 0; i < rasterizer.Pages.Count; i++)
                    {
                        jpgFile = file.DirectoryName + "\\" + file.Name  + i.ToString() + ".jpg";
                        rasterizer.Pages[i].Draw(jpgFile, ImageFormat.Jpeg, ImageSize.Dpi72);

                        imgList.Add(new imgClass() { ImgUrl = "/Documents/Issues/" + Path.GetFileName(jpgFile) });
                    }


                    pdfGridView.DataSource = null;
                    pdfGridView.DataBind();

                    pdfGridView.DataSource = imgList;
                    pdfGridView.DataBind();

                    e.Row.Cells[0].Controls.Remove(issue_img);

                    e.Row.Cells[0].Controls.Add(pdfGridView);

                    foreach (GridViewRow gr in pdfGridView.Rows)
                    {
                        Image pdf_issue_img = (Image)gr.Cells[0].Controls[0];

                        gr.Cells[1].Visible = false;

                        pdf_issue_img.Attributes.Add("width", "100%");
                        pdf_issue_img.Attributes.Add("height", "100%");
                    }

                    pdfGridView.HeaderRow.Visible = false;

                   // Extension = Path.GetExtension(stringPath);

                   // //string jpgFile = path.Replace(Extension, "") + ".jpg";

                   // file = new FileInfo(jpgFile);

                   //// PdfRasterizer rasterizer = new PdfRasterizer(stringPath);

                   // for(int i=0; i < rasterizer.Pages.Count; i++)
                   // {
                   //     jpgFile = file.DirectoryName + "page" + i.ToString() + ".jpg";
                   //     rasterizer.Pages[i].Draw(jpgFile , ImageFormat.Jpeg, ImageSize.Dpi72);
                   // }

                   // issue_img.ImageUrl = "~/Documents/" + "Issuespage0.jpg";      // file.Name;

                   // // rasterizer.Draw(jpgFile, ImageFormat.Jpeg, ImageSize.Dpi72);


                   // issue_img.Attributes.Add("width", "100%");
                   // issue_img.Attributes.Add("height", "100%");
                }
                else if (Extension == ".docx" || Extension == ".doc")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdt.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    object fileName = Server.MapPath(fname);

                    GridView docGridView = new GridView();

                    ImageField imgFld = new ImageField();

                    imgFld.HeaderText = "Image";
                    imgFld.DataImageUrlField = "ImgUrl";

                    docGridView.Columns.Add(imgFld);

                    List<imgClass> imgList = new List<imgClass>();

                    if (System.IO.File.Exists(fileName.ToString()))
                    {
                        Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();
                        object missing = System.Type.Missing;

                        try
                        {
                            doc = word.Documents.Open(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                            doc.Activate();
                            foreach (Microsoft.Office.Interop.Word.Window window in doc.Windows)
                            {
                                foreach (Microsoft.Office.Interop.Word.Pane pane in window.Panes)
                                {
                                    for (var i = 1; i <= pane.Pages.Count; i++)
                                    {
                                        var bits = pane.Pages[i].EnhMetaFileBits;
                                        var target = System.IO.Path.Combine("\\" + fileName.ToString().Split('.')[0], string.Format("{1}_page_{0}", i, fileName.ToString().Split('.')[0]));

                                        try
                                        {
                                            using (var ms = new System.IO.MemoryStream((byte[])(bits)))
                                            {
                                                var image = System.Drawing.Image.FromStream(ms);
                                                var pngTarget = System.IO.Path.ChangeExtension(target, "png");
                                                image.Save(pngTarget, System.Drawing.Imaging.ImageFormat.Png);
                                              //  Response.Write("Image created with Name" + target);

                                                file = new FileInfo(pngTarget);

                                               // issue_img.ImageUrl = "~/Documents/Issues/" + file.Name ;
                                               // issue_img.Attributes.Add("width", "100%");
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                        
                                        }

                                        imgList.Add(new imgClass() { ImgUrl = "/Documents/Issues/" + file.Name });

                                    }
                                }
                            }

                            
                            doc.Close(Type.Missing, Type.Missing, Type.Missing);
                            word.Quit(Type.Missing, Type.Missing, Type.Missing);

                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                        }
                        finally
                        {
                          //  doc.Close(ref missing, ref missing, ref missing);
                            ((Microsoft.Office.Interop.Word._Application)word).Quit();
                        }

                        docGridView.DataSource = null;
                        docGridView.DataBind();

                        docGridView.DataSource = imgList;
                        docGridView.DataBind();

                        e.Row.Cells[0].Controls.Remove(issue_img);

                        e.Row.Cells[0].Controls.Add(docGridView);

                        foreach (GridViewRow gr in docGridView.Rows)
                        {
                            Image pdf_issue_img = (Image)gr.Cells[0].Controls[0];

                            gr.Cells[1].Visible = false;

                            pdf_issue_img.Attributes.Add("width", "100%");
                            pdf_issue_img.Attributes.Add("height", "100%");
                        }

                        docGridView.HeaderRow.Visible = false;
                    }
                    else
                    {
                        Response.Write("File not Found !!!");
                    }
                }

                Label imgLabel = new Label();
                imgLabel.Text = Path.GetFileName(path);
                e.Row.Cells[0].Controls.Add(imgLabel);
            }
        }

        protected void GrdIssueStatusImages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Image issue_img = (Image)e.Row.Cells[0].Controls[0];

                string path = Server.MapPath(issue_img.ImageUrl);

                string Extension = Path.GetExtension(path);

               

                if (Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg" || Extension == ".bmp")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdt.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    issue_img.ImageUrl = fname;

                    issue_img.Attributes.Add("width", "100%");
                    issue_img.Attributes.Add("height", "100%");
                    //issue_img.Attributes.Add("class", "img-fluid");
                }
                else if (Extension == ".pdf")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdt.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    string stringPath = Server.MapPath(fname);

                    // string zipFile = Server.MapPath("/Documents/Issues/Output.zip");

                    // if (File.Exists(zipFile)) File.Delete(zipFile);

                    // ConvertPdfToImages(stringPath);

                    //// string startPath = Server.MapPath("/Documents/Issues");
                    // string zipPath = Server.MapPath("/Documents/Issues/Output.zip");
                    // string extractPath = Server.MapPath("/Documents/Issues/Output");

                    // if (Directory.Exists(extractPath)) DeleteDirectory(extractPath);

                    // ZipFile.ExtractToDirectory(zipPath, extractPath);

                    GridView pdfGridView = new GridView();

                    ImageField imgFld = new ImageField();

                    imgFld.HeaderText = "Image";
                    imgFld.DataImageUrlField = "ImgUrl";

                    pdfGridView.Columns.Add(imgFld);

                    List<imgClass> imgList = new List<imgClass>();

                    //foreach (string imgObj in Directory.GetFiles(extractPath))
                    //{
                    //    imgList.Add(new imgClass() { ImgUrl = "/Documents/Issues/Output/" + Path.GetFileName(imgObj) });
                    //}

                    PdfRasterizer rasterizer = new PdfRasterizer(stringPath);

                    string jpgFile = "";

                    for (int i = 0; i < rasterizer.Pages.Count; i++)
                    {
                        jpgFile = file.DirectoryName + "\\" + file.Name  + i.ToString() + ".jpg";
                        rasterizer.Pages[i].Draw(jpgFile, ImageFormat.Jpeg, ImageSize.Dpi72);

                        imgList.Add(new imgClass() { ImgUrl = "/Documents/Issues/" + Path.GetFileName(jpgFile) });
                    }

                    pdfGridView.DataSource = null;
                    pdfGridView.DataBind();

                    pdfGridView.DataSource = imgList;
                    pdfGridView.DataBind();

                    e.Row.Cells[0].Controls.Remove(issue_img);

                    e.Row.Cells[0].Controls.Add(pdfGridView);

                    foreach (GridViewRow gr in pdfGridView.Rows)
                    {
                        Image pdf_issue_img = (Image)gr.Cells[0].Controls[0];

                        gr.Cells[1].Visible = false;

                        pdf_issue_img.Attributes.Add("width", "100%");
                        pdf_issue_img.Attributes.Add("height", "100%");
                    }

                    pdfGridView.HeaderRow.Visible = false;

                    //Extension = Path.GetExtension(stringPath);

                    //string jpgFile = path.Replace(Extension, "") + ".jpg";

                    //file = new FileInfo(jpgFile);

                    //PdfRasterizer rasterizer = new PdfRasterizer(stringPath);

                    //rasterizer.Draw(jpgFile, ImageFormat.Jpeg, ImageSize.Dpi72);

                    //issue_img.ImageUrl = "~/Documents/Issues/" + file.Name;

                    //issue_img.Attributes.Add("width", "100%");
                    //issue_img.Attributes.Add("height", "100%");
                }
                else if (Extension == ".docx" || Extension == ".doc")
                {
                    string outPath = path.Replace(Extension, "") + "_download" + Extension;
                    getdt.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

                    string fname = "/Documents/Issues/" + file.Name;

                    object fileName = Server.MapPath(fname);

                    GridView docGridView = new GridView();

                    ImageField imgFld = new ImageField();

                    imgFld.HeaderText = "Image";
                    imgFld.DataImageUrlField = "ImgUrl";

                    docGridView.Columns.Add(imgFld);

                    List<imgClass> imgList = new List<imgClass>();

                    if (System.IO.File.Exists(fileName.ToString()))
                    {
                        Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();
                        object missing = System.Type.Missing;

                        try
                        {
                            doc = word.Documents.Open(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                            doc.Activate();
                            foreach (Microsoft.Office.Interop.Word.Window window in doc.Windows)
                            {
                                foreach (Microsoft.Office.Interop.Word.Pane pane in window.Panes)
                                {
                                    for (var i = 1; i <= pane.Pages.Count; i++)
                                    {
                                        var bits = pane.Pages[i].EnhMetaFileBits;
                                        var target = System.IO.Path.Combine("\\" + fileName.ToString().Split('.')[0], string.Format("{1}_page_{0}", i, fileName.ToString().Split('.')[0]));

                                        try
                                        {
                                            using (var ms = new System.IO.MemoryStream((byte[])(bits)))
                                            {
                                                var image = System.Drawing.Image.FromStream(ms);
                                                var pngTarget = System.IO.Path.ChangeExtension(target, "png");
                                                image.Save(pngTarget, System.Drawing.Imaging.ImageFormat.Png);

                                                // Response.Write("Image created with Name" + target);

                                                file = new FileInfo(pngTarget);
                                            }
                                        }
                                        catch (System.Exception ex)
                                        { 
                                        
                                        }

                                        imgList.Add(new imgClass() { ImgUrl = "/Documents/Issues/" + file.Name });
                                    }
                                }
                            }
                            doc.Close(Type.Missing, Type.Missing, Type.Missing);
                            word.Quit(Type.Missing, Type.Missing, Type.Missing);

                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                        }
                        finally
                        {
                            doc.Close(ref missing, ref missing, ref missing);
                            ((Microsoft.Office.Interop.Word._Application)word).Quit();
                        }

                        docGridView.DataSource = null;
                        docGridView.DataBind();

                        docGridView.DataSource = imgList;
                        docGridView.DataBind();

                        e.Row.Cells[0].Controls.Remove(issue_img);

                        e.Row.Cells[0].Controls.Add(docGridView);

                        foreach (GridViewRow gr in docGridView.Rows)
                        {
                            Image pdf_issue_img = (Image)gr.Cells[0].Controls[0];

                            gr.Cells[1].Visible = false;

                            pdf_issue_img.Attributes.Add("width", "100%");
                            pdf_issue_img.Attributes.Add("height", "100%");
                        }

                        docGridView.HeaderRow.Visible = false;
                    }
                    else
                    {
                        Response.Write("File not Found !!!");
                    }
                }


                Label imgLabel = new Label();
                imgLabel.Text = Path.GetFileName(path);
                e.Row.Cells[0].Controls.Add(imgLabel);
            }
        }

        private void DeleteDirectory(string path)
        {
            // Delete all files from the Directory  
            foreach (string filename in Directory.GetFiles(path))
            {
                File.Delete(filename);
            }
            // Check all child Directories and delete files  
            foreach (string subfolder in Directory.GetDirectories(path))
            {
                DeleteDirectory(subfolder);
            }

            Directory.Delete(path);
            
        }

        protected void ConvertPdfToImages(string pdfFile)
        {
            GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Load a PDF document.
            using (var document = GemBox.Pdf.PdfDocument.Load(pdfFile))
            {
                var imageOptions = new GemBox.Pdf.ImageSaveOptions(ImageSaveFormat.Png);

                string file_path = Server.MapPath("/Documents/issues/Output.zip");
                // Create a ZIP file for storing PNG files.
                using (var archiveStream = File.OpenWrite(file_path))
                using (var archive = new System.IO.Compression.ZipArchive(archiveStream, ZipArchiveMode.Create))
                {
                    int CNT = 0;

                    CNT = document.Pages.Count > 2 ? 2 : document.Pages.Count;
                    // Iterate through the PDF pages.
                    for (int pageIndex = 0; pageIndex < CNT ; pageIndex++) // document.Pages.Count
                    {
                        // Add a white background color to the page.
                        var page = document.Pages[pageIndex];
                        var elements = page.Content.Elements;
                        var background = elements.AddPath(elements.First);
                        background.AddRectangle(0, 0, page.Size.Width, page.Size.Height);
                        background.Format.Fill.IsApplied = true;
                        background.Format.Fill.Color = PdfColor.FromRgb(1, 1, 1);

                        // Create a ZIP entry for each page.
                        var entry = archive.CreateEntry($"Page{pageIndex + 1}.png");

                        // Save each page as a PNG image to the ZIP entry.
                        using (var imageStream = new MemoryStream())
                        using (var entryStream = entry.Open())
                        {
                            imageOptions.PageNumber = pageIndex;
                            document.Save(imageStream, imageOptions);

                            imageStream.Position = 0;
                            imageStream.CopyTo(entryStream);
                        }
                    }
                }
            }
        }

    }
}