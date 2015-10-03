<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="AllLifePricingWeb.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    
        <br />
        <br />
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        returnPremium</div>
        <asp:Label ID="Label3" runat="server" Text="subscriberName"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox4" runat="server">Magnum</asp:TextBox>
        <br />
        <asp:Label ID="Label4" runat="server" Text="subscriberPassword"></asp:Label>
        <asp:TextBox ID="TextBox5" runat="server">!Password123</asp:TextBox>
        <br />
        <asp:Label ID="Label5" runat="server" Text="subscriberCode"></asp:Label>
        <asp:TextBox ID="TextBox6" runat="server">100</asp:TextBox>
        <br />
        <asp:Label ID="Label6" runat="server" Text="productCode"></asp:Label>
        <asp:TextBox ID="TextBox7" runat="server">ZAALDB2</asp:TextBox>
        <br />
        <asp:Label ID="Label7" runat="server" Text="baseRisk"></asp:Label>
        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label8" runat="server" Text="riskModifier"></asp:Label>
        <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label9" runat="server" Text="coverValue"></asp:Label>
        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label10" runat="server" Text="quoteDate"></asp:Label>
        <asp:TextBox ID="TextBox11" runat="server">2015-01-13</asp:TextBox>
        <br />
        <asp:Label ID="Label11" runat="server" Text="customerID"></asp:Label>
        <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label12" runat="server" Text="quotationID"></asp:Label>
        <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Test premiumReturn" />
        <br />
        <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </form>
</body>
</html>
