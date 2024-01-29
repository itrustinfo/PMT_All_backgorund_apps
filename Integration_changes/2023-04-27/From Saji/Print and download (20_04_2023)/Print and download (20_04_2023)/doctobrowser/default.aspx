<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.doctobrowser._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">
             function PrintDiv(id) {
                    var data = document.getElementById(id).innerHTML;
                    var myWindow = window.open('', 'my div', 'height=600,width=800');
                    myWindow.document.write('<html><head><title></title>');
                    /*optional stylesheet*/ myWindow.document.write('<link rel="stylesheet" href="Css/style.css" type="text/css" />');
                    myWindow.document.write('</head><body >');
                    myWindow.document.write(data);
                    myWindow.document.write('</body></html>');
                    myWindow.document.close(); // necessary for IE >= 10

                    myWindow.onload = function () { // necessary if the div contain images

                        myWindow.focus(); // necessary for IE >= 10
                        myWindow.print();
                        myWindow.close();
                    };
             }
        </script>
    <%--project selection dropdowns--%>
        <div class="container-fluid">
            <div class="row">
              
                

        <div>
       
        </div>
        <div id="myDiv" style="background-color:#ffffff;padding:20px">
                     <div id="printreport" runat="server" visible="true" style="text-align:right;padding-top:10px;padding-right:20px">
                                         <input type="button"  value="Print" class="btn btn-primary" onclick="PrintDiv('imgDiv')" />
                    </div>   
            <div id="imgDiv">
             <div class="col-lg-12 grid-margin stretch-card">
              <div class="card">
                  <div style="text-align:center">
                      <div class="col-lg-12" style="text-align:center"><h3 style="padding:5px;margin:5px"><asp:label ID="lbl1" runat="server"></asp:label></h3></div>
                      <div class="col-lg-12" style="text-align:center"><h4 style="padding:5px;margin:5px"><asp:label ID="lbl2" runat="server"></asp:label></h4></div>
                      <div class="col-lg-12" style="text-align:center"><h5 style="padding:5px;margin:5px"><asp:label ID="lbl3" runat="server"></asp:label></h5></div>
                  </div>
                <div class="card-body"><h5 style="text-align:center; font-weight:bold; color:#006699;">ISSUE DETAILS</h5></div>

              </div>
                 </div>
                     <div class="col-lg-12 grid-margin stretch-card">
                          <div class="card">
                              <asp:GridView ID="GrdPrint" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" Width="100%"  OnRowDataBound="GrdPrint_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Issue_Description" HeaderText="Issue Description">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                                   
                            <asp:BoundField DataField="Issued_User" HeaderText="Reporting User" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Date" HeaderText="Date of Issue Occurrence" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                          
                            <asp:BoundField DataField="Actual_Closer_Date" HeaderText="Actual Closer Date"  DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Status" HeaderText="Status" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Issue_Remarks" HeaderText="Issue Remarks" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Issue_Uid" HeaderText="Issue ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" >
                            <HeaderStyle HorizontalAlign="Left" />
                             <ItemStyle CssClass="hiddencol"></ItemStyle>
                            </asp:BoundField>
<asp:TemplateField HeaderText="Uploaded Documents" ItemStyle-HorizontalAlign="Center">
               <ItemTemplate>
                  <asp:Panel ID="pnlUploadedDocs" runat="server" Visible="true" Style="position: relative">
                    <asp:GridView ID="gvUploadedDocs" runat="server" AutoGenerateColumns="false" 
                        AllowPaging="false" CssClass="ChildGrid"
                        DataKeyNames="doc_id">
                        <Columns>
                            <asp:HyperLinkField DataNavigateUrlFields="doc_path,doc_name" HeaderText="File Name" DataNavigateUrlFormatString="~/_content_pages/Doctoprint/default.aspx?path={0}&name={1}" DataTextField="doc_name" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
                                  
                                </Columns>                        
                            </asp:GridView>

            <div class="card">
                <div class="card-body"><h5 style="text-align:center; font-weight:bold; color:#006699;">ISSUE STATUS DETAILS</h5></div>

              </div>

                   <asp:GridView ID="gvIssueRemarks" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" Width="100%"
                        AllowPaging="false" 
                        DataKeyNames="IssueRemarksUID" OnRowDataBound="GrdIssueRemarks_RowDataBound">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="Issue_Status" HeaderText="Status" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Issue_Remarks" HeaderText="Remarks" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="IssueRemark_Date" HeaderText="Date" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="IssueRemarksUID" HeaderText="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" > <ItemStyle CssClass="hiddencol"></ItemStyle></asp:BoundField>
                            <asp:TemplateField HeaderText="Uploaded Documents" ItemStyle-HorizontalAlign="Center">
                               <ItemTemplate>
                                  <asp:Panel ID="pnlUploadedRemDocs" runat="server" Visible="true" Style="position: relative">
                                    <asp:GridView ID="gvUploadedRemDocs" runat="server" AutoGenerateColumns="false" 
                                        AllowPaging="false" CssClass="ChildGrid"
                                        DataKeyNames="uploaded_doc_id">
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="doc_path,doc_name" HeaderText="File Name" DataNavigateUrlFormatString="~/_content_pages/Doctoprint/default.aspx?path={0}&name={1}" DataTextField="doc_name" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <div style="padding:20px">
                <div class="col-lg-12">
                    <div>
                        <h4>Issue Images</h4>
                    </div>
            <asp:GridView ID="GrdIssueImages" runat="server"  AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" CssClass="table table-bordered" OnRowDataBound="GrdIssueImages_RowDataBound"  >
                         <Columns>
                         </Columns>
                     </asp:GridView>
                </div>
                <div class="col-lg-12">
                    <div>
                        <h4>Issue Status Images</h4>
                    </div>
                 <asp:GridView ID="GrdIssueStatusImages" runat="server"  AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%"  CssClass="table table-bordered" OnRowDataBound="GrdIssueStatusImages_RowDataBound"  AllowPaging="false" >
                         
                         <Columns>
                         </Columns>
                     </asp:GridView>
                </div>
                        </div>


                          </div>
                         </div>
             
                 </div>

           
            <%--<embed runat="server" src="/Documents/Issues/MonthlyMealPlan_Brochure_Rev24_DE_download.pdf" width="100%" height="2100" />--%>

                <div>
                   
                    </div>
                      <div>
                    
                   </div>
                 </div> 
        </div>
    
      </div>
    
</asp:Content>


