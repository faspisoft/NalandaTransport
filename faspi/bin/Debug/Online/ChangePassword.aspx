<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="nalanadatransport.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container ">
        <div class="col-lg-6 col-lg-offset-3">
        <div class="panel panel-info">
                    <div class="panel-heading">
                       
            <h4 style="margin: 0px">
                Change Password</h4>
            </div>
            <div class="panel-body">
            <asp:HiddenField ID="docid" runat="server" Value="0" />
            <div class="form-group">
                <label for="txtOld">
                    Current Password :
                    <asp:RequiredFieldValidator ID="ValidateCPwd" ForeColor="Red" runat="server" ErrorMessage="Required"
                        ControlToValidate="txtOld"></asp:RequiredFieldValidator></label>
                <asp:TextBox ID="txtOld" CssClass="form-control" placeholder="Old Password" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtNew">
                    New Password :
                    <asp:RequiredFieldValidator ID="RequiredNPwd" ForeColor="Red" runat="server"
                        ErrorMessage="Required" ControlToValidate="txtNew"></asp:RequiredFieldValidator></label>
                <asp:TextBox ID="txtNew" CssClass="form-control" placeholder="New Password" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtCon">
                    Confirm Password :
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" runat="server"
                        ErrorMessage="Required" ControlToValidate="txtCon"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareCPwd" ForeColor="Red" runat="server" ControlToValidate="txtCon"
                        ControlToCompare="txtNew" ErrorMessage="Not Match"></asp:CompareValidator>
                 
                </label>
                <asp:TextBox ID="txtCon" CssClass="form-control" placeholder="Confirm Password" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <center>
                    <asp:Label ID="lblErr" runat="server" ForeColor="Red" Text=""></asp:Label>
                </center>
            </div>

            <asp:Button runat="server" ID="btnSubmit" Text="Save" CssClass="btn btn-success" OnClick="btnSubmit_Click" />

           </div>
           </div>
           </div>
           
        </div>
   


</asp:Content>
