<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="Reapetor.aspx.cs" Inherits="nalanadatransport.Reapetor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
   </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
<div class="container">


                         <div class="row">
                          <div class="col-lg-2 form-group ">
                         <label>Total GR. Number</label>
                            <%-- <asp:TextBox ID="txtTotlaGrNumber" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>--%>
                              <asp:Label ID="lblTotalGrnumber" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                         </div>
                           <div class="col-lg-1 form-group">
                         
                         <label>To Pay</label>

                             
                          <asp:Label ID="lbltopay" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                          </div>
                           <div class="col-lg-1 form-group">
                         
                         <label>Paid</label>

                             
                         <asp:Label ID="lblpaid" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                          </div>
                           <div class="col-lg-1 form-group">
                         
                         <label>T.B.B.</label>

                             
                          <asp:Label ID="lbltbb" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                          </div>
                           <div class="col-lg-2 form-group ">
                         <label>Total Quantity</label>
                             
                         <asp:Label ID="lbltotalquantity" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                         </div>
                             <div class="col-lg-2 form-group">
                         <label>Total weight</label>
                             
                         <asp:Label ID="lbltotalweight" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                         </div>
                      
                           <div class="col-lg-2 form-group">
                         <label>Total Freight</label>
                             
                                                      <asp:Label ID="lbltotalfreight" CssClass="form-control"  runat="server" Text="Label"></asp:Label>
                         </div>
                       
                        


                       

                         
                         
                         </div>  
                         <div class="row">
     <asp:ImageButton style="float:right" ID="ImageButton1" runat="server" ImageUrl="~/images/pdf.png" Width="60px"
                         Height="60px" Visible="false" onclick="btnExport_Click" /> 
                          <asp:ImageButton style="float:right" ID="ImageButton2" runat="server" ImageUrl="~/images/Excel.png" Width="60px"
                         Height="60px" Visible="false" onclick="imgbtnExcel_Click" />  
                         </div>               
<div class="row">

<asp:Panel ID="panel1" runat="server">
 <asp:Repeater ID="rptrDetails" OnItemCommand=" rptrDetails_ItemCommand" runat="server" >
                        <HeaderTemplate>
                            <table id="tableCustomer" class="table table-responsive table-striped" >
                            
                                <thead>
                               
                                    <tr>
                                   
                                     <th>
                                        Details
                                        </th>
                                       
                                       
                                        
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                       
                        <ItemTemplate>
                          
                            <tr>
                            <td>
                            <div class="col-lg-2">
                            <div class="col-lg-6">
                            <b>Date:</b>
                                    <asp:Label ID="lblItmeserial" Text='<%# Eval("vdate","{0:dd-MMM-yyyy}") %>' runat="server" />
                              </div>
                              <div class="col-lg-6">
                              <b>GR.No.:</b>
                             
                                  <asp:LinkButton ID="LinkButton1" Text='<%# Eval("invoiceno") %>' CommandName="SELECT" CommandArgument='<%# Eval("invoiceno") %>' runat="server" />
                               
                                
                                     </div> 
                              </div>
                              <div class="col-lg-3">
                              <div class="col-lg-6">
                              <b>Consigner:</b>
                                    <asp:Label ID="lblActive" Text='<%# Eval("consigner") %>' runat="server" />
                               </div>
                                <div class="col-lg-6">
                                <b>Consignee:</b>
                                    <asp:Label ID="Label2" Text='<%# Eval("consignee") %>' runat="server" />
                                    </div>
                               </div>
                                
                                <div class="col-lg-4">
                                <div class="col-lg-4">
                                <b>Origin:</b>
                                    <asp:Label ID="Label1" Text='<%# Eval("origin") %>' runat="server" />
                               </div>
                              <div class="col-lg-4">
                              <b>Destination:</b>
                                    <asp:Label ID="Label3" Text='<%# Eval("destination") %>' runat="server" />
                               </div>
                               <div class="col-lg-4">
                              <b>Payment Mode:</b>
                                    <asp:Label ID="Label7" Text='<%# Eval("paymentmode") %>' runat="server" />
                               </div>
                                
                               </div>
                               <div class="col-lg-3">
                             
                              <div class="col-lg-4">
                               <b>Quantity:</b>
                                    <asp:Label ID="Label4" Text='<%# Eval("quantity") %>' runat="server" />
                               </div>
                               <div class="col-lg-4">
                                <b>Weight:</b>
                                    <asp:Label ID="Label5" Text='<%# Eval("weight") %>' runat="server" />
                                    </div>
                               
                               <div class="col-lg-4">
                                <b>Total Freight:</b>
                                    <asp:Label ID="Label6" Text='<%# Eval("total_freight") %>' runat="server" />
                                    </div>
                                    </div>
                               </div>
                                </td>
                                 
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                    </asp:Panel>
</div>


</div>


</asp:Content>