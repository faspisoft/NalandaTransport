<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="PodUpdate.aspx.cs"
    Inherits="nalanadatransport.PodUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
    function previewFile() {
        var preview = document.querySelector('#<%=Image2.ClientID %>');
        var file = document.querySelector('#<%=FileUpload2.ClientID %>').files[0];
        var reader = new FileReader();

        reader.onloadend = function () {
            preview.src = reader.result;
        }

        if (file) {
            reader.readAsDataURL(file);
        } else {
            preview.src = "";
        }
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdocid" runat="server" />
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="v1" runat="server">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-lg-2   col-md-2 col-sm-2">
                            <label>
                                Enter GR No
                            </label>
                        </div>
                        <div class="col-lg-4  col-md-4 col-sm-4">
                            <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-4  col-md-4 col-sm-4">
                            <asp:Button ID="Button1" CssClass="btn btn-info" runat="server" Text="Search" OnClick="btnsubmit_Click" />
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="min-height: 300px;">
                <center>
                            <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                        </center>
                    <asp:Repeater ID="rptrDetails" runat="server" OnItemCommand="rptrDetails_ItemCommand">
                        <HeaderTemplate>
                        <div class="table-responsive">
                            <table id="tableCustomer" class="table table-striped">
                                <thead>
                                    <tr>
                                        <th>
                                            GR Date
                                        </th>
                                        <th>
                                            GR Number
                                        </th>
                                        <th style="text-align: left;">
                                            From
                                        </th>
                                        <th style="text-align: left;">
                                            To
                                        </th>
                                        <th>
                                            Pod Status
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" Text='<%# Eval("vdate","{0:dd-MMM-yyyy}") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="Label4" Text='<%# Eval("invoiceno") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="lblFirmName" Text='<%# Eval("stationfrom") %>' runat="server" />-
                                    <asp:Label ID="Label2" Text='<%# Eval("Source") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="lblActive" Text='<%# Eval("stationto") %>' runat="server" />-
                                    <asp:Label ID="Label3" Text='<%# Eval("Destination") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" CommandName="image" CommandArgument='<%# Eval("Vi_id") %>'
                                        runat="server"><%# Eval("podstatus") %></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table></div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </asp:View>
        <asp:View ID="v2" runat="server">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-sm-3  col-md-3 col-sm-3">
                            <asp:Label ID="lblDate" Text="" runat="server" />
                        </div>
                        <div class="col-sm-3 col-md-3 col-sm-3">
                            <asp:Label ID="lblInvno" Text="" runat="server" />
                        </div>
                        <div class="col-sm-3 col-md-3 col-sm-3">
                            <asp:Label ID="lblFrom" Text="" runat="server" />
                        </div>
                        <div class="col-sm-3 col-md-3 col-sm-3">
                            <asp:Label ID="lblDestination" Text="" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="panel-body" style="min-height: 300px;">
                    <div class="row">
                        <div class="col-sm-4 col-sm-offset-4">
                        <center>
                            <asp:Label ID="lblMsgImg" runat="server" Text=""></asp:Label>
                        </center>
                            <asp:Image ID="Image2" ImageUrl="~/images/upload.jpg" Height="99%" Width="99%" runat="server"
                                CssClass="img img-thumbnail" />
                            <asp:FileUpload ID="FileUpload2" runat="server" onchange="previewFile()" />
                            <br />
                            <br />
                            <center>
                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-info" Text="Save" OnClick="Button1_Click" />
                                <asp:Button ID="btnback" runat="server" CssClass="btn btn-danger" Text="Back" OnClick="Btnback_Click" />
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
