<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="BookRegister.aspx.cs"
    Inherits="nalanadatransport.BookRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="row">
        <div class="col-lg-12">
            <h4>
                Booking Register</h4>
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <label>
                                Date From</label>
                            <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="Calendarextender" PopupButtonID="imgPopup" runat="server"
                                TargetControlID="txtDateFrom" Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <label>
                                Date To</label>
                            <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="Calendarextender1" PopupButtonID="imgPopup" runat="server"
                                TargetControlID="txtDateTo" Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <label>
                                Source</label>
                            <asp:DropDownList ID="ddlSource" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2  col-md-2 col-sm-2">
                            <label>
                                Destination</label>
                            <asp:DropDownList ID="ddlDestination" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <label>
                                Location</label>
                            <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnSearch" runat="server" CssClass="form-control" Text="Search" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                    <hr style="margin:5px;" />
                      <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12">
                      <label style="margin-right:10px">
                        To Pay : <span class="badge"><asp:Label ID="lbltopay" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
                    
               
                     <label style="margin-right:10px">
                        Paid : <span class="badge">
                    <asp:Label ID="lblpaid" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
               
                     <label style="margin-right:10px">
                        Billed : <span class="badge">
                    <asp:Label ID="lbltbb" style="font-size:15px;" runat="server" Text="0.0"></asp:Label></span> </label>
                
                   
                </div>
            </div>
                </div>
                <div class="panel-body" style="min-height: 300px;">
                    <div class="row">
                        <asp:Repeater ID="rptrDetails" OnItemCommand="rptrDetails_ItemCommand" runat="server">
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
                                                <b>G.R. No.</b>
                                                <asp:LinkButton ID="LinkButton1" Text='<%# Eval("grno") %>' CommandName="SELECT"
                                                    CommandArgument='<%# Eval("vi_id") %>' runat="server"></asp:LinkButton>
                                            </div>
                                            <div class="col-lg-6  col-md-6 col-sm-6">
                                                <b>Date</b>
                                                <asp:Label ID="lblDescription" Text='<%# Eval("grdate","{0:dd-MMM-yyyy}") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-lg-2  col-md-2 col-sm-2">
                                            <div class="col-lg-8  col-md-8 col-sm-8">
                                                <b>Consigner</b>
                                                <asp:Label ID="lblPacking" Text='<%# Eval("consigner") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-sm-4">
                                                <b>Consignee</b>
                                                <asp:Label ID="lblActive" Text='<%# Eval("consignee") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                            <div class="col-lg-4 col-md-4 col-sm-4">
                                                <b>Source</b>
                                                <asp:Label ID="Label1" Text='<%# Eval("source") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-sm-4">
                                                <b>Destination</b>
                                                <asp:Label ID="Label2" Text='<%# Eval("Destination") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-sm-4">
                                                <b>Description</b>
                                                <asp:Label ID="Label3" Text='<%# Eval("description") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-md-5 col-sm-5">
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <b>Packing</b>
                                                <asp:Label ID="Label4" Text='<%# Eval("packing") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <b>Delivery Type</b>
                                                <asp:Label ID="Label5" Text='<%# Eval("deliverytype") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <b>GR.Type</b>
                                                <asp:Label ID="Label6" Text='<%# Eval("gr_type") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <b>Total Paid</b>
                                                <asp:Label ID="Label7" Text='<%# Eval("total_paid") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <b>Total Pay</b>
                                                <asp:Label ID="Label8" Text='<%# Eval("total_pay") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                <b>Total Billed</b>
                                                <asp:Label ID="Label9" Text='<%# Eval("total_billed") %>' runat="server" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table></div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
