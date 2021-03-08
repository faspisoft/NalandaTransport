<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="CustConsineeRegister.aspx.cs"
    Inherits="nalanadatransport.CustConsineeRegister" %>

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
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h4>
                        Inword Register (Last 60 Days)</h4>
                </div>
                <div class="panel-body" style="min-height: 300px;">
                    <div class="row" style="margin: 10px;">
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
                                                Source
                                            </th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" Text='<%# Eval("grno") %>' CommandName="SELECT"
                                            CommandArgument='<%# Eval("vi_id") %>' runat="server"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("grdate","{0:dd-MMM-yyyy}") %>
                                    </td>
                                    <td>
                                        <%# Eval("consigner") %>
                                    </td>
                                    <td>
                                        <%# Eval("Destination") %>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" Text='<%# Faspi.Functions.TittleCase(Eval("entrytype")) %>' CommandName="SELECT"
                                            CommandArgument='<%# Eval("vi_id") %>' runat="server"></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
