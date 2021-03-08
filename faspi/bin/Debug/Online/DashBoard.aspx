<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" MasterPageFile="~/Site1.Master"
    Inherits="nalanadatransport.scripts.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h4 style="margin: 2px">
            Dashboard</h4>
        <hr style="margin: 0px; padding: 0px;" />
        <div class="row">
            <div class="col-lg-6">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        Today</div>
                    <div class="panel-body">
                        <asp:Repeater ID="rptrtoday" OnItemCommand=" rptrDetailstoday_ItemCommand" runat="server">
                            <HeaderTemplate>
                            <div class="table-responsive">
                                <table id="tableCustomer" class="table table-responsive table-striped">
                                    <thead>
                                        <tr>
                                            <th>
                                                Location
                                            </th>
                                            <th style="text-align: right">
                                                GR
                                            </th>
                                            <th style="text-align: right">
                                                Freight
                                            </th>
                                            <th style="text-align: right">
                                                Paid
                                            </th>
                                            <th style="text-align: right">
                                                To Pay
                                            </th>
                                            <th style="text-align: right">
                                                T.B.B
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" Text='<%# Eval("nick_name") %>' CommandName="SELECT"
                                            CommandArgument='<%# Eval("nick_name") %>' runat="server"></asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <%# Eval("totalvnumber") %>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("totalamount"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_paid"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_pay"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_Billed"))%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        Yesterday</div>
                    <div class="panel-body">
                        <asp:Repeater ID="rptrYesterday" OnItemCommand=" rptrDetailsYesteday_ItemCommand"
                            runat="server">
                            <HeaderTemplate>
                            <div class="table-responsive">
                                <table id="tableCustomer" class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>
                                                Location
                                            </th>
                                            <th style="text-align: right">
                                                GR
                                            </th>
                                            <th style="text-align: right">
                                                Freight
                                            </th>
                                            <th style="text-align: right">
                                                Paid
                                            </th>
                                            <th style="text-align: right">
                                                To Pay
                                            </th>
                                            <th style="text-align: right">
                                                T.B.B
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" Text='<%# Eval("nick_name") %>' CommandName="SELECT"
                                            CommandArgument='<%# Eval("nick_name") %>' runat="server"></asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <%# Eval("totalvnumber") %>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("totalamount"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_paid"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_pay"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_Billed"))%>
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
        <div class="row">
            <div class="col-lg-6">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        Last 7 Days</div>
                    <div class="panel-body">
                        <asp:Repeater ID="rptrLast7Days" OnItemCommand=" rptrDetails15Days_ItemCommand" runat="server">
                            <HeaderTemplate>
                            <div class="table-responsive">
                                <table id="tableCustomer" class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>
                                                Location
                                            </th>
                                            <th style="text-align: right">
                                                GR
                                            </th>
                                            <th style="text-align: right">
                                                Freight
                                            </th>
                                            <th style="text-align: right">
                                                Paid
                                            </th>
                                            <th style="text-align: right">
                                                To Pay
                                            </th>
                                            <th style="text-align: right">
                                                T.B.B
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" Text='<%# Eval("nick_name") %>' CommandName="SELECT"
                                            CommandArgument='<%# Eval("nick_name") %>' runat="server"></asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <%# Eval("totalvnumber") %>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("totalamount"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_paid"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_pay"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_Billed"))%>
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
            <div class="col-lg-6">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        Till Today</div>
                    <div class="panel-body">
                        <asp:Repeater ID="rptrTillToday" OnItemCommand=" rptrDetailsTilltoday_ItemCommand"
                            runat="server">
                            <HeaderTemplate>
                            <div class="table-responsive">
                                <table id="tableCustomer" class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>
                                                Location
                                            </th>
                                            <th style="text-align: right">
                                                GR
                                            </th>
                                            <th style="text-align: right">
                                                Freight
                                            </th>
                                            <th style="text-align: right">
                                                Paid
                                            </th>
                                            <th style="text-align: right">
                                                To Pay
                                            </th>
                                            <th style="text-align: right">
                                                T.B.B
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" Text='<%# Eval("nick_name") %>' CommandName="SELECT"
                                            CommandArgument='<%# Eval("nick_name") %>' runat="server"></asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <%# Eval("totalvnumber") %>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("totalamount"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_paid"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_pay"))%>
                                    </td>
                                    <td align="right">
                                        <%# Faspi.Functions.IndianCurr(Eval("total_Billed"))%>
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
