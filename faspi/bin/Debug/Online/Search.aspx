<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" MasterPageFile="~/Site1.Master"
    Inherits="nalanadatransport.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3>
       G.R. Search</h3>
    <div class="panel panel-info">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-2">
                    <label>
                        Enter GR No
                    </label>
                </div>
                <div class="col-lg-4">
                    <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnsubmit" CssClass="btn btn-info" runat="server" Text="Ok" OnClick="btnsubmit_Click" />
                </div>
            </div>
        </div>
        <div class="panel-body">
            <asp:Panel ID="panel1" runat="server">
                <div class="row">
                    <div class="col-lg-6">
                        <h3 style="text-align: center;">
                            Booking Details</h3>
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    GR.No.</label>
                                <asp:TextBox ID="txtGrNo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    Date</label>
                                <asp:TextBox ID="txtDate" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    Consigner</label>
                                <asp:TextBox ID="txtConsigner" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    Consignee</label>
                                <asp:TextBox ID="txtConsignee" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    Source</label>
                                <asp:TextBox ID="txtSource" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    Destination</label>
                                <asp:TextBox ID="txtDestination" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-4 form-group">
                                <label>
                                    Quantity</label>
                                <asp:TextBox ID="txtQuantity" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 form-group">
                                <label>
                                    Weight</label>
                                <asp:TextBox ID="txtWeight" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 form-group">
                                <label>
                                    C.H.Weight</label>
                                <asp:TextBox ID="txtChWeight" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-1">
                    </div>
                    <div class="col-lg-5">
                        <h3 style="text-align: center;">
                            Dispatch Details</h3>
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    Challan Number</label>
                                <asp:TextBox ID="txtChallanNo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    Date</label>
                                <asp:TextBox ID="txtDispatchDate" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 form-group">
                                <label>
                                    Gaadi Number</label>
                                <asp:TextBox ID="txtGaadinumber" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-6 form-group">
                                <label>
                                    Driver Name</label>
                                <asp:TextBox ID="txtDriverName" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
