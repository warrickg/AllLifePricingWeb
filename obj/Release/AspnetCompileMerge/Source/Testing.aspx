<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Testing.aspx.cs" Inherits="AllLifePricingWeb.Testing" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0">
    <Tabs>
        <telerik:RadTab runat="server" Text="Return Premium" Selected="True">
        </telerik:RadTab>
        <telerik:RadTab runat="server" Text="Return Risk">
        </telerik:RadTab>
        <telerik:RadTab runat="server" Text="Return Cover">
        </telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>
<telerik:RadMultiPage ID="RadMultiPage1" Runat="server" SelectedIndex="0">
    <telerik:RadPageView ID="RadPageView1" runat="server">
        Premium
        <table class="loginTable">
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="subscriber Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server">Magnum</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="subscriber Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox5" runat="server">!Password123</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="subscriber Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox6" runat="server">100</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="product Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox7" runat="server">ZAALDB2WL06</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="base Risk"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox8" runat="server">35MNS3Gold</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="risk Modifier"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox9" runat="server">T2A2C1&lt;5NormNormS0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="cover Value"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox10" runat="server">2000000</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" Text="quote Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox11" runat="server">2015-01-13</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="customer ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="quotation ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <telerik:RadButton ID="RadButtonRunPremium" runat="server" OnClick="RadButtonRunPremium_Click" Text="Run Premium">
                    </telerik:RadButton>
                </td>
            </tr>
            <caption>
                <br />
            </caption>

        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="DataTable result:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView2" runat="server">
        Risk
        <table class="loginTable">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="subscriber Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server">Magnum</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label13" runat="server" Text="subscriber Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server">!Password123</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label14" runat="server" Text="subscriber Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server">100</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label15" runat="server" Text="product Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox14" runat="server">ZAALDB2WL06</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label16" runat="server" Text="base Risk"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox15" runat="server">35MNS3</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 24px">
                    <asp:Label ID="Label17" runat="server" Text="risk Modifier"></asp:Label>
                </td>
                <td style="height: 24px">
                    <asp:TextBox ID="TextBox16" runat="server">T2A2C1&lt;5NormNormS0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label18" runat="server" Text="cover Value"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox17" runat="server">1000000</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label34" runat="server" Text="Premium Value"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox31" runat="server">160</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label19" runat="server" Text="quote Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox18" runat="server">2015-01-15</asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td style="height: 21px">
                    <asp:Label ID="Label35" runat="server" Text="Inception Date"></asp:Label>
                </td>
                <td style="height: 21px">
                    <asp:TextBox ID="TextBox32" runat="server">2015-01-01</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label36" runat="server" Text="Effective Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox33" runat="server">2016-01-01</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label20" runat="server" Text="customer ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox19" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label21" runat="server" Text="quotation ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label37" runat="server" Text="Duration Modifier Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxDurationModifierCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <telerik:RadButton ID="RadButtonRunRisk" runat="server" Text="Run Risk" OnClick="RadButtonRunRisk_Click">
                    </telerik:RadButton>
                </td>
            </tr>
            <caption>
                <br />
            </caption>

        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label22" runat="server" Text="DataTable result:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView2" runat="server">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </telerik:RadPageView>
    <telerik:RadPageView ID="RadPageView3" runat="server">
        Cover
        <table class="loginTable">
            <tr>
                <td>
                    <asp:Label ID="Label23" runat="server" Text="subscriber Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox21" runat="server">Magnum</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label24" runat="server" Text="subscriber Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox22" runat="server">!Password123</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label25" runat="server" Text="subscriber Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox23" runat="server">100</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label26" runat="server" Text="product Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox24" runat="server">ZAALDB2WL06</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 24px">
                    <asp:Label ID="Label27" runat="server" Text="base Risk"></asp:Label>
                </td>
                <td style="height: 24px">
                    <asp:TextBox ID="TextBox25" runat="server">35MNS3Gold</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label28" runat="server" Text="risk Modifier"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox26" runat="server">T2A2C1&lt;5NormNormS0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label29" runat="server" Text="Premium Value"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox27" runat="server">350</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label30" runat="server" Text="quote Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox28" runat="server">2015-01-13</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label31" runat="server" Text="customer ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox29" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label32" runat="server" Text="quotation ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox30" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <telerik:RadButton ID="RadButtonRunCover" runat="server" Text="Run Cover" OnClick="RadButtonRunCover_Click">
                    </telerik:RadButton>
                </td>
            </tr>
            <caption>
                <br />
            </caption>

        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label33" runat="server" Text="DataTable result:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView3" runat="server">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </telerik:RadPageView>
</telerik:RadMultiPage>
</asp:Content>
