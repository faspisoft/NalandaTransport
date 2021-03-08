<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadImages.aspx.cs" MasterPageFile="~/Site1.Master" Inherits="nalanadatransport.UploadImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   </asp:Content>

   <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   <div class="container">
   
   <div class="row">
   <div class="col-lg-4">
   </div>
   
   <div class="col-lg-4">
       <asp:Image ID="Image1" ImageUrl="~/images/upload.jpg"  Height="99%" Width="99%" runat="server" />
      
       <asp:FileUpload ID="FileUpload2" runat="server" />
       <br />
       <asp:Button ID="Button1" runat="server" CssClass="form-control" Text="Save" 
           onclick="Button1_Click" />
   </div>
   </div>
   
   </div>

       

</asp:Content>