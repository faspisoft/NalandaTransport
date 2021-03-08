<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRStatus.aspx.cs" MasterPageFile="~/Site1.Master"
    Inherits="nalanadatransport.GRStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-info">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-12">
                    <b>Detail Status Of GR No </b><span class="badge">
                        <asp:Label ID="lbltbb" Style="font-size: 15px;" runat="server" Text=""></asp:Label></span>
                    
                </div>
            </div>
        </div>
        <div class="panel-body" style="min-height: 300px;">
            <asp:Repeater ID="rptrDetails" runat="server">
                <HeaderTemplate>
                    <table id="tableCustomer" class="table table-responsive table-striped">
                        <thead>
                            <tr><th>
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
                    <tr><td>
                            <%# Eval("Vdate")%>
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
        </div>
    </div>
</asp:Content>
