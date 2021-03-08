<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testcon.aspx.cs" Inherits="nalanadatransport.testcon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" ></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
