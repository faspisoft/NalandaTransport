<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="Reports.aspx.cs"
    Inherits="nalanadatransport.Reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="panel panel-info">
        <div class="panel-heading">
            Reports
        </div>
        <div class="panel-body"style="min-height:300px;">
            <div class="row">
                <div class="col-md-6 col-md-offset-3">
                    <div class="row ">
                        <div class="col-lg-6">
                            <label>
                                Date From</label>
                            <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="Calendarextender" PopupButtonID="imgPopup" runat="server"
                                TargetControlID="txtDateFrom" Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </div>
                        <div class="col-lg-6">
                            <label>
                                Date To</label>
                            <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server" OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="Calendarextender1" PopupButtonID="imgPopup" runat="server"
                                TargetControlID="txtDateTo" Format="dd-MMM-yyyy">
                            </cc1:CalendarExtender>
                        </div>
                        <div class="col-lg-6">
                            <label>
                                Location</label>
                            <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-6">
                            <label>
                                Stock Category</label>
                            <asp:DropDownList ID="ddlStockCategory" CssClass="form-control" runat="server">
                                <asp:ListItem Value="Step1">Booked</asp:ListItem>
                                <asp:ListItem Value="Step2">ToBeDelivered</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                        </div>
                        
                        <div class="col-lg-12">
                            <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                            <asp:Button ID="BtnCancel" CssClass="btn btn-danger pull-right" runat="server" Text="Cancel" OnClick="BtnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
