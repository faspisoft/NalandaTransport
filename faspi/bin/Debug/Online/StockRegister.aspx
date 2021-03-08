<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="StockRegister.aspx.cs"
    Inherits="nalanadatransport.StockRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="DataTable/jquery.dataTables.css" rel="stylesheet" type="text/css" />
    <script src="DataTable/jquery.dataTables.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tableCustomer').dataTable(
            {
                "lengthMenu": [[25, 50, -1], [25, 50, "All"]]
            }
            );
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="panel panel-info">
        <div class="panel-heading">
            <h4>
                Stock Register</h4>
            <hr style="margin: 0px; padding: 0px" />
            <div class="row ">
                <div class="col-lg-2 col-md-2 col-sm-2">
                    <label>
                        Date From</label>
                    <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server" ></asp:TextBox>
                    <cc1:CalendarExtender ID="Calendarextender" PopupButtonID="imgPopup" runat="server"
                        TargetControlID="txtDateFrom" Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2">
                    <label>
                        Date To</label>
                    <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server" ></asp:TextBox>
                    <cc1:CalendarExtender ID="Calendarextender1" PopupButtonID="imgPopup" runat="server"
                        TargetControlID="txtDateTo" Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3">
                    <label>
                        Location</label>
                    <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3">
                    <label>
                        Stock Category</label>
                    <asp:DropDownList ID="ddlStockCategory" CssClass="form-control" runat="server">
                        <asp:ListItem Value="Step1">Booked</asp:ListItem>
                        <asp:ListItem Value="Step2">ToBeDelivered</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2">
                    <br />
                    <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Search" OnClick="btnSubmit_Click" />
                </div>
            </div>
             <div class="row">
                <div class="col-lg-12">
                    <label style="margin-right:10px">
                        Total GR. : <span class="badge"><asp:Label ID="lblTotalGrnumber" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
              
                     <label style="margin-right:10px">
                        To Pay : <span class="badge"><asp:Label ID="lbltopay" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
                    
               
                     <label style="margin-right:10px">
                        Paid : <span class="badge">
                    <asp:Label ID="lblpaid" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
               
                     <label style="margin-right:10px">
                        T.B.B. : <span class="badge">
                    <asp:Label ID="lbltbb" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
                
                    <label style="margin-right:10px">
                        Total Quantity : <span class="badge">
                    <asp:Label ID="lbltotalquantity" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
                
                     <label style="margin-right:10px">
                        Total weight : <span class="badge">
                    <asp:Label ID="lbltotalweight" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
               
                     <label>
                        Total Freight : <span class="badge">
                    <asp:Label ID="lbltotalfreight" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
                </div>
            </div>
        </div>
        <div class="panel-body" style="min-height: 300px;">
           
            <div class="row">
                <asp:ImageButton Style="float: right" ID="ImageButton1" runat="server" ImageUrl="~/images/pdf.png"
                    Width="30px" Height="30px" Visible="false" OnClick="btnExport_Click" />
                <asp:ImageButton Style="float: right" ID="ImageButton2" runat="server" ImageUrl="~/images/Excel.png"
                    Width="30px" Height="30px" Visible="false" OnClick="imgbtnExcel_Click" />
            </div>
            <div class="row">
                <asp:Panel ID="panel1" runat="server">
                    <asp:Repeater ID="rptrDetails" OnItemCommand=" rptrDetails_ItemCommand" runat="server">
                        <HeaderTemplate>
                        <div class="table-responsive">
                            <table id="tableCustomer" class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>
                                            Details
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                        <div class="col-lg-6 col-md-6 col-sm-6">
                                            <b>Date:</b>
                                            <asp:Label ID="lblItmeserial" Text='<%# Eval("vdate","{0:dd-MMM-yyyy}") %>' runat="server" />
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6">
                                            <b>GR.No.:</b>
                                            <asp:LinkButton ID="LinkButton1" Text='<%# Eval("invoiceno") %>' CommandName="SELECT"
                                                CommandArgument='<%# Eval("invoiceno") %>' runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="col-lg-6 col-md-6 col-sm-6">
                                            <b>Consigner:</b>
                                            <asp:Label ID="lblActive" Text='<%# Eval("consigner") %>' runat="server" />
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-6">
                                            <b>Consignee:</b>
                                            <asp:Label ID="Label2" Text='<%# Eval("consignee") %>' runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <b>Origin:</b>
                                            <asp:Label ID="Label1" Text='<%# Eval("origin") %>' runat="server" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <b>Destination:</b>
                                            <asp:Label ID="Label3" Text='<%# Eval("destination") %>' runat="server" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <b>Payment Mode:</b>
                                            <asp:Label ID="Label7" Text='<%# Eval("paymentmode") %>' runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <b>Quantity:</b>
                                            <asp:Label ID="Label4" Text='<%# Eval("quantity") %>' runat="server" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <b>Weight:</b>
                                            <asp:Label ID="Label5" Text='<%# Eval("weight") %>' runat="server" />
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <b>Total Freight:</b>
                                            <asp:Label ID="Label6" Text='<%# Eval("total_freight") %>' runat="server" />
                                        </div>
                                    </div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table></div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
