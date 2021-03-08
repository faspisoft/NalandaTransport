<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRdetails.aspx.cs" MasterPageFile="~/Site1.Master"
    Inherits="nalanadatransport.GRdetails" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <script type = "text/javascript">
       function Confirm() {
           var confirm_value = document.createElement("INPUT");
           confirm_value.type = "hidden";
           confirm_value.name = "confirm_value";
           if (confirm("Do you want to del data?")) {
               confirm_value.value = "Yes";
           } else {
               confirm_value.value = "No";
           }
           document.forms[0].appendChild(confirm_value);
       }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                <asp:Button ID="btnsubmit" CssClass="btn btn-info" runat="server" Text="Search" OnClick="btnsubmit_Click" />
            </div>
        </div>
        </div>
        <div class="panel-body"style="min-height:300px;">

     
        <asp:Panel ID="panel2" runat="server" Visible="false">
            <asp:Repeater ID="rptrVoucherInfo" OnItemCommand=" rptrDetails_ItemCommand" runat="server">
                <HeaderTemplate>
                <div class="table-responsive">
                    <table id="tableCustomer" class="table table-striped">
                        <thead>
                            <tr>
                                <th>
                                    Details
                                </th>
                                <th>
                                    Select
                                </th>
                                 <th>
                                    Delete
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <div class="col-lg-2  col-md-2 col-sm-2">
                                <div class="col-lg-12">
                                    <b>Date:</b>
                                    <asp:Label ID="lblItmeserial" Text='<%# Eval("vdate","{0:dd-MMM-yyyy}") %>' runat="server" />
                                </div>
                            </div>
                            <div class="col-lg-4  col-md-4 col-sm-4">
                                <div class="col-lg-6  col-md-6 col-sm-6">
                                    <b>Consigner:</b>
                                    <asp:Label ID="lblDescription" Text='<%# Eval("consigner") %>' runat="server" />
                                </div>
                                <div class="col-lg-6  col-md-6 col-sm-6">
                                    <b>Consignee:</b>
                                    <asp:Label ID="lblPacking" Text='<%# Eval("consignee") %>' runat="server" />
                                </div>
                            </div>
                            <div class="col-lg-3  col-md-3 col-sm-3">
                                <div class="col-lg-6  col-md-6 col-sm-6">
                                    <b>Origin:</b>
                                    <asp:Label ID="lblActive" Text='<%# Eval("origin") %>' runat="server" />
                                </div>
                                <div class="col-lg-6  col-md-6 col-sm-6">
                                    <b>Destination:</b>
                                    <asp:Label ID="Label1" Text='<%# Eval("destination") %>' runat="server" />
                                </div>
                            </div>
                            <div class="col-lg-3  col-md-3 col-sm-3">
                                <div class="col-lg-6  col-md-6 col-sm-6">
                                    <b>Location:</b>
                                    <asp:Label ID="Label2" Text='<%# Eval("nick_name") %>' runat="server" />
                                </div>
                                <div class="col-lg-6  col-md-6 col-sm-6">
                                    <b>T. Freight:</b>
                                    <asp:Label ID="Label7" Text='<%# Eval("total_freight") %>' runat="server" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgbtnSelect" runat="server" Height="20PX" ImageUrl="~/images/Select.png"
                                Width="25px" CommandName="SELECT" CommandArgument='<%# Eval("Vi_id") %>' />
                        </td>
                         <td>
                            <asp:ImageButton ID="imgbtndelete" runat="server" Height="20PX" ImageUrl="~/images/delete.png"
                                Width="25px" CommandName="DELETE" CommandArgument='<%# Eval("Vi_id") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table></div>
                </FooterTemplate>
            </asp:Repeater>
        </asp:Panel>
        <asp:Panel ID="panel1" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-9  col-md-9 col-sm-9">
                    <div class="row">
                        <div class="col-lg-3   col-md-3 col-sm-3 form-group">
                            <label>
                                Date</label>
                            <asp:TextBox ID="txtDate" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Voucher Type</label>
                            <asp:TextBox ID="txtvoucherTypr" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                GR.No.</label>
                            <asp:TextBox ID="txtGrNo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Self</label>
                            <asp:TextBox ID="txtself" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Consigner</label>
                            <asp:TextBox ID="txtConsigner" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:Label ID="lblGst1" runat="server" Visible="false" Text="Gst"></asp:Label>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Origin</label>
                            <asp:TextBox ID="txtOrigin" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Consignee</label>
                            <asp:TextBox ID="txtConsignee" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:Label ID="lblGst2" runat="server" Visible="false" Text="Gst"></asp:Label>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Destination</label>
                            <asp:TextBox ID="txtDestination" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Private Mark</label>
                            <asp:TextBox ID="txtPrivateMark" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Invoice Number</label>
                            <asp:TextBox ID="txtinvoiceNumber" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Delivery At</label>
                            <asp:TextBox ID="txtDelieveryAt" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Value</label>
                            <asp:TextBox ID="txtValue" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Agent Name</label>
                            <asp:TextBox ID="txtAgentName" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Number</label>
                            <asp:TextBox ID="txtEwayNumber" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Remarks</label>
                            <asp:TextBox ID="txtRemarks" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <asp:Repeater ID="rptrDetails" runat="server">
                            <HeaderTemplate>
                                <table id="tableCustomer" class="table table-responsive table-striped">
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
                                        <div class="col-lg-2  col-md-2 col-sm-2">
                                            <div class="col-lg-6  col-md-6 col-sm-6">
                                                <b>S.No.</b>
                                                <asp:Label ID="lblItmeserial" Text='<%# Eval("itemsr") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-6  col-md-6 col-sm-6">
                                                <b>Item</b>
                                                <asp:Label ID="lblDescription" Text='<%# Eval("des_ac_id") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-lg-2  col-md-2 col-sm-2">
                                            <div class="col-lg-8  col-md-8 col-sm-8">
                                                <b>Packing</b>
                                                <asp:Label ID="lblPacking" Text='<%# Eval("packing") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-4  col-md-4 col-sm-4">
                                                <b>Quantity</b>
                                                <asp:Label ID="lblActive" Text='<%# Eval("quantity") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-lg-3  col-md-3 col-sm-3">
                                            <div class="col-lg-3  col-md-3 col-sm-3">
                                                <b>Multi</b>
                                                <asp:Label ID="Label1" Text='<%# Eval("multiplier") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-6  col-md-6 col-sm-6">
                                                <b>Weight</b>
                                                <asp:Label ID="Label2" Text='<%# Eval("weight") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-3  col-md-3 col-sm-3">
                                                <b>Ch.Weight</b>
                                                <asp:Label ID="Label3" Text='<%# Eval("chargedweight") %>' runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-lg-5  col-md-5 col-sm-5">
                                            <div class="col-lg-4  col-md-4 col-sm-4">
                                                <b>Rate On</b>
                                                <asp:Label ID="Label4" Text='<%# Eval("per") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-3  col-md-3 col-sm-3">
                                                <b>Rate</b>
                                                <asp:Label ID="Label5" Text='<%# Eval("Rate_am") %>' runat="server" />
                                            </div>
                                            <div class="col-lg-4  col-md-4 col-sm-4">
                                                <b>Amount</b>
                                                <asp:Label ID="Label6" Text='<%# Eval("amount") %>' runat="server" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div class="col-lg-6  col-md-6 col-sm-6">
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Total Nug</label>
                            <asp:TextBox ID="txtTotalNug" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Total Weight</label>
                            <asp:TextBox ID="txtTotalWeight" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Total Ch. Weight</label>
                            <asp:TextBox ID="txtTotalChWeight" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Total Amount</label>
                            <asp:TextBox ID="txtTotalAmount" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Delivery Type</label>
                            <asp:TextBox ID="txtDelievery" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                G.R. Type</label>
                            <asp:TextBox ID="txtGrtype" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Rate By</label>
                            <asp:TextBox ID="txtRateBy" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-lg-3  col-md-3 col-sm-3 form-group">
                            <label>
                                Delivery Address</label>
                            <asp:TextBox ID="txtDelieveryAddress" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3  col-md-3 col-sm-3">
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                G.R. Charge</label>
                            <asp:TextBox ID="txtGrNumber" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Hamali</label>
                            <asp:TextBox ID="txtHamali" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Local Cartage</label>
                            <asp:TextBox ID="txtLocalCartage" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Door Delivery</label>
                            <asp:TextBox ID="txtDoorDelievery" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Crossing Charge</label>
                            <asp:TextBox ID="txtCrossincharge" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Good Tax</label>
                            <asp:TextBox ID="txtGoodTax" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                GST</label>
                            <asp:TextBox ID="txtGst" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Dooor Delivery</label>
                            <asp:TextBox ID="txtDoorDelievery1" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Other Charges2</label>
                            <asp:TextBox ID="txtOtherCharges" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Other Charges32</label>
                            <asp:TextBox ID="txtOtherCharges32" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Round Off</label>
                            <asp:TextBox ID="txtRoundOff" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-9  col-md-9 col-sm-9">
                            <label>
                                Net Amount</label>
                            <asp:TextBox ID="txtNetamt" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        </div> 
        
    </div>
</asp:Content>
