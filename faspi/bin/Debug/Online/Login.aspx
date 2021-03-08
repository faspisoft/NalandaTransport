<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site2.Master" CodeBehind="Login.aspx.cs"
    Inherits="nalanadatransport.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style>
.navbar-default .navbar-collapse .navbar-nav li a
{
	font-size:18px;
	color:rgb(81,41,111);
}
.footer-heading
{
    margin-top:0px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid"  style="background:url(img/login_img.jpg); margin-top:-50px; min-height:460px;">
    <div class="container" >
       <br />
       <br />
        <div class="row">
                                 
            <div class="well col-sm-4 col-sm-offset-4" >
            <h1 style="margin:0px;padding:0px;">Log In</h1>
            <hr />
                <div class="form-group">
                    <label>
                        User Name</label>
                    <asp:TextBox ID="txtUserName" class="form-control" runat="server" type="text" placeholder="Username" />
                </div>
                <div class="form-group">
                    <label>
                        Password</label>
                    <asp:TextBox ID="txtPassword" class="form-control" runat="server" type="Password"
                        placeholder="Password" TextMode="Password" />
                </div>
                <center>
                <asp:Button class="btn btn-danger btn-block" ID="btnlogin" runat="server" Text="Login"
                     BackColor="BlanchedAlmond" ForeColor="Black" OnClick="btnlogin_Click" />
                </center>
                
               
            </div>
        </div>
       <br />
       <br />
      
       
           
    </div>
    </div>
</asp:Content>
