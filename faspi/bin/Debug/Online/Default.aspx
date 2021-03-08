<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="nalanadatransport.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="myCarousel" class="my-carousel carcarousel slide" data-ride="carousel">
    <!-- Indicators -->
    <ol class="carousel-indicators">
      <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
      <li data-target="#myCarousel" data-slide-to="1"></li>
      <li data-target="#myCarousel" data-slide-to="2"></li>
    </ol>

    <!-- Wrapper for slides -->
    <div class="carousel-inner">

      <div class="item active">
        <img src="img/1.jpg" alt="Los Angeles" style="width:100%;">
        <div class="carousel-caption">
          <h2>Nalanda Express Transport Corp. (Regd.)</h2>
          
        </div>
      </div>

      <div class="item">
        <img src="images/truck2.jpg" alt="Chicago" style="width:100%;">
        <div class="carousel-caption" >
        <h2>Nalanda Express Transport Corp. (Regd.)</h2>
          </div>
        
      </div>
    
      <div class="item">
        <img src="images/truck1.jpg" alt="New York" style="width:100%;">
        <div class="carousel-caption">
          <h2>Nalanda Express Transport Corp. (Regd.)</h2>
                  </div>
      </div>
  
    </div>

    <!-- Left and right controls -->
  <!---  <a class="left carousel-control" href="#myCarousel" data-slide="prev">
      <span class="glyphicon glyphicon-chevron-left"></span>
      <span class="sr-only">Previous</span>
    </a>
    <a class="right carousel-control" href="#myCarousel" data-slide="next">
      <span class="glyphicon glyphicon-chevron-right"></span>
      <span class="sr-only">Next</span>
    </a>--->
  </div>
<div class=" container caption"  >
<h1 class="pull-right" style="margin-right:-140px;">Nalanda Express Transport Corp.(Regd.)</h1>
</div>
   
</asp:Content>
