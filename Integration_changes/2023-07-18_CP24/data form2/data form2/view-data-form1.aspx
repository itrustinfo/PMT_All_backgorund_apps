<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-data-form1.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_data_form1" %>
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
                     <th class="col-md-2"><asp:label id="lblCRefNo" runat="server"></asp:label></th>
                    <th class="col-md-2">DD-MM-YYYY</th>
                    <th class="col-md-2"></th>
                    <th class="col-md-4">DD-MM-YYYY</th>

                </tr>
                <tr class="col-12">
                    <th class="col-md-2">ONTB Ref.No.</th>
                    <th class="col-md-2"><asp:label id="lblONTBRefNo" runat="server"></asp:label></th>
                    <th class="col-md-2">DD-MM-YYYY</th>
                    <th class="col-md-2"></th>
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
                 <tr class="col-12">
                    <th class="col-md-12">
                        Document Description : XXXXXXXX
                    </th>
                </tr>
            </thead>
        </table>
        <div>
        <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-lg-12">
                    <th class="col-lg-8">Document No. XXXXXXXXXX</th>
                    <th class="col-lg-4">Status/Action Code X</th>
                 </tr>
           </thead>
        </table>
       </div>
       </div>
            </div>
            <div class="col-sm-12" style="padding:0px">
                <div class="table-responsive" style="padding:0px">
                        <asp:GridView ID="GrdDataList" runat="server" Width="100%" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="false"  >
                        <Columns>
                            <asp:BoundField DataField="Revision"  ItemStyle-HorizontalAlign="Left" HeaderText="Revision" HtmlEncode="False" />
                            <asp:BoundField DataField="SerialNo"  ItemStyle-HorizontalAlign="Left" HeaderText="Serial No." HtmlEncode="False" />
                            <asp:BoundField DataField="Comment1"  ItemStyle-HorizontalAlign="Left" HeaderText="ONTB Comments" HtmlEncode="False" />
                            <asp:BoundField DataField="Reply1"  ItemStyle-HorizontalAlign="Left" HeaderText="Contractor Replies" HtmlEncode="False" />
                        </Columns>
                    </asp:GridView>
                 </div>
             </div>
    </div>
    
    <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>
 </form>
</asp:Content>
