<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-data-form.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_data_form" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>

    <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
           
        });
    </script>
   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
<form id="frmDataViewModal" runat="server">
    
    <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
        <div style="text-align:center;padding:10px"><h3><b>CP-24 COMMENT SHEET</b></h3></div>
        <div class="row">
        <div class="col-lg-12">
            
        <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-12">
                    <th class="col-md-2">Contractor Letter Ref.No.</th>
                    <th class="col-md-6"></th>
                    <th class="col-md-2">Date</th>
                    <th class="col-md-4">DD-MM-YYYY</th>

                </tr>
                <tr class="col-12">
                    <th class="col-md-2">ONTB Ref.No.</th>
                    <th class="col-md-6"></th>
                    <th class="col-md-2">Date</th>
                    <th class="col-md-4">DD-MM-YYYY</th>
                </tr>
            </thead>

        </table>
         <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-12">
                    <th class="col-md-12">
                        Plant: Bengaluru Water Supply and Sewerage Project (Phase 3) [Cauvery Water Supply Scheme (CWSS) Stage V] CP-24 – Design/Engineering, Construction, Commissioning of Centralized SCADA Centre for Monitoring & Control of Water Treatment Plants and Distribution System, Sewerage SCADA for Monitoring of all Sewerage Facilities of BWSSB with Comprehensive Operations and Maintenance of Seven (7) Years (Contract Package – BWSSP (III)/JICA/CP-24) under JICA Loan ID – P299
                    </th>
                </tr>
            </thead>
        </table>
        <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-md-12">
                    <th class="col-md-6">Document Description.</th>
                    <th class="col-md-2"></th>
                    <th class="col-md-4">Status/Action Code X</th>
                    <th class="col-md-2"></th>
                </tr>
                 <tr class="col-md-12">
                    <th>
                        <table>
                        <thead>
                            <tr class="col-md-12">
                                <th class="col-md-6">Document No.</th>
                                <th class="col-md-6"></th>
                            </tr>
                            <tr class="col-md-12">
                                <th class="col-md-6">Revision No.</th>
                                <th class="col-md-6"></th>
                            </tr>
                        </thead>
                        </table>
                    </th>
                    <th class="col-md-2"></th>
                    <th class="col-md-4"></th>
                    <th class="col-md-2"></th>
                </tr>
               
            </thead>

        </table>
       </div>
            </div>
            <div class="col-sm-12" style="padding:0px">
                <div class="table-responsive" style="padding:0px">
                        <asp:GridView ID="GrdDataList" runat="server" Width="100%" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="false"  >
                        <Columns>
                            <asp:BoundField DataField="Id"  ItemStyle-HorizontalAlign="Left" HeaderText="Serial No." HtmlEncode="False" />
                            <asp:TemplateField HeaderText="Comment">
                             <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"
                                     Text= '<%# Eval("Comment") %>' BorderStyle="None" Width="100%" TextMode="MultiLine"></asp:TextBox>
                            </ItemTemplate>
                            </asp:TemplateField>
                           <asp:BoundField DataField="Status"  ItemStyle-HorizontalAlign="Left" HeaderText="Status" HtmlEncode="False" />
                         </Columns>
                    </asp:GridView>
                 </div>
             </div>
    </div>
    
    <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>
 </form>
</asp:Content>
