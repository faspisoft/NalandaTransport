<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRTrack.aspx.cs" MasterPageFile="~/Site2.Master"
    Inherits="nalanadatransport.GRTrack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-info">
        <%--<div class="panel-heading">
            <div class="row">
                <div class="col-lg-12">
                    <center>
                        <asp:Label ID="lblTitle" runat="server" Text="Search G.R. No."></asp:Label></center>
                </div>
            </div>
        </div>--%>
        <div class="panel-body" style="min-height: 300px;">
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="v1" runat="server">
                    <div class="row">
                        <div class="col-lg-4 col-md-6 col-sm-6 col-lg-offset-4 col-md-offset-3 col-sm-offset-3">
                            <div class="row well">
                                <div class="form-group col-lg-12">
                                    <h3>
                                        Enter GR No
                                    </h3>
                                </div>
                                <div class="form-group col-lg-12">
                                    <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group col-lg-12">
                                    <asp:Button ID="btnsubmit" CssClass="btn btn-info" runat="server" Text="Search" OnClick="btnsubmit_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="v2" runat="server">
                    <asp:Repeater ID="rptrDetails" runat="server" OnItemCommand="rptrDetails_ItemCommand">
                        <HeaderTemplate>
                            <table id="tableCustomer" class="table table-responsive table-striped">
                                <thead>
                                    <tr>
                                        <th>
                                            G.R. No.
                                        </th>
                                        <th>
                                           G.R. Date
                                        </th>
                                        <th>
                                            Consigner
                                        </th>
                                        <th>
                                           Consignee
                                        </th>
                                        <th>
                                           Source
                                        </th>
                                        <th>
                                           Destination
                                        </th>
                                        <th>
                                           Status
                                        </th>
                                        <th></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("grno") %>
                                </td>
                                <td>
                                    <%# Eval("grdate","{0:dd-MMM-yyyy}") %>
                                </td>
                                <td>
                                    <%# Eval("consigner") %>
                                </td>
                                <td>
                                    <%# Eval("consignee") %>
                                </td>
                                <td>
                                    <%# Eval("source") %>
                                </td>
                                <td>
                                    <%# Eval("Destination") %>
                                </td>
                                <td>
                                    <%# Faspi.Functions.TittleCase(Eval("entrytype")) %>
                                </td>
                                <td>
                                 <asp:LinkButton ID="LinkButton1" ForeColor="Blue" CssClass="brn brn-info" Text="Show Detail"
                                        CommandName="SELECT" CommandArgument='<%# Eval("vi_id") %>' runat="server"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:View>
                <asp:View ID="v3" runat="server">
                    <asp:Repeater ID="rptStatus" runat="server">
                        <HeaderTemplate>
                            <table id="tableCustomer" class="table table-responsive table-striped">
                                <thead>
                                    <tr>
                                        <th>
                                            Reff. Date
                                        </th>
                                        <th>
                                            Reff. No.
                                        </th>
                                        <th>
                                            At
                                        </th>
                                        <th>
                                            To
                                        </th>
                                        <th>
                                            Status
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("vdate") %>
                                </td>
                                <td>
                                    <%# Eval("reffno") %>
                                </td>
                                <td>
                                    <%# Eval("source") %>
                                </td>
                                <td>
                                    <%# Eval("destination") %>
                                </td>
                                <td>
                                    <%# Faspi.Functions.TittleCase(Eval("entrytype")) %>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>
