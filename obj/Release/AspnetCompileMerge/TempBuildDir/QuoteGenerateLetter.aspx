<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuoteGenerateLetter.aspx.cs" Inherits="AllLifePricingWeb.QuoteGenerateLetter" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">       
        <script type="text/javascript">
            function CloseSessionExpired(args) {
                GetRadWindow().BrowserWindow.PopupWindowResponseCloseSessionExpired();
                GetRadWindow().close();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

                return oWindow;
            }

            function PopupWindowResponseClose(arg) {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Close");
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">        
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableAJAX="False">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGridOptions">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadPanelBar1" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadButtonGenerateLetter">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ReportViewer1" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div>            
            <table width=100% border=0>
                <tr>
                    <td width="100%">
                        <table border=0 align=center>
                            <tr>
                                <td>
                                    <telerik:RadPanelBar ID="RadPanelBar1" Runat="server" Width="700px">
                                        <Items> 
                                            <telerik:RadPanelItem runat="server" Text="Select the quote options you want to appear on the quote letter">  
                                                <Items> 
                                                    <telerik:RadPanelItem runat="server" Value="PanelItem1">  
                                                        <ItemTemplate> 
                                                            <telerik:RadGrid ID="RadGridOptions" runat="server" AutoGenerateColumns="false">
                                                                <MasterTableView>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn SortExpression="QuoteOptionAuditTrailID" HeaderText="QuoteOptionAuditTrailID" HeaderButtonType="TextButton"
                                                                            DataField="QuoteOptionAuditTrailID" UniqueName="QuoteOptionAuditTrailID" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn SortExpression="OptionNumber" HeaderText="Option Number" HeaderButtonType="TextButton" DataField="OptionNumber" UniqueName="OptionNumber" >
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn SortExpression="CoverLife" HeaderText="Cover Life" HeaderButtonType="TextButton" DataField="CoverLife" UniqueName="CoverLife">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn SortExpression="PremiumLife" HeaderText="Premium Life" HeaderButtonType="TextButton" DataField="PremiumLife" UniqueName="PremiumLife">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn SortExpression="CoverDisability" HeaderText="Cover Disability" HeaderButtonType="TextButton" DataField="CoverDisability" UniqueName="CoverDisability">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn SortExpression="PremiumDisability" HeaderText="Premium Disability" HeaderButtonType="TextButton" DataField="PremiumDisability" UniqueName="PremiumDisability">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn SortExpression="Selected" HeaderText="Accespted" HeaderButtonType="TextButton" DataField="Selected" UniqueName="Selected" Visible="false">
                                                                        </telerik:GridBoundColumn>                                        
                                                                        <telerik:GridTemplateColumn UniqueName="checked" DataField="checked" HeaderText="Selected" > 
                                                                            <ItemTemplate> 
                                                                                <asp:CheckBox ID="CheckBoxSelect" runat="server" AutoPostBack="True" /> 
                                                                            </ItemTemplate>  
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </ItemTemplate>
                                                    </telerik:RadPanelItem>
                                                </Items>
                                            </telerik:RadPanelItem>
                                        </Items>
                                    </telerik:RadPanelBar>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
             <table width=100% border=0>
                <tr>
                    <td width="100%">                                    
                        <table border=0 align=center>
                            <tr>
                                <td style="text-align: center" colspan="2">
                                    <telerik:RadButton ID="RadButtonGenerateLetter" runat="server" Text="Generate Letter" OnClick="RadButtonGenerateLetter_Click"></telerik:RadButton>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                               <td colspan="2">
                                   &nbsp;
                               </td>
                            </tr>                      
                            <tr>
                                <td>
                                    <telerik:RadButton ID="RadBtnSaveLetter" runat="server" OnClick="RadBtnSaveLetter_Click" Text="Save Letter to Quote" Visible="False">
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    <telerik:RadButton ID="RadBtnEmail" runat="server"  Text="Email the letter" Visible="False" OnClick="RadBtnEmail_Click">
                                    </telerik:RadButton>
                                </td>
                            </tr>   
                            <tr>
                                 <td colspan="2">
                                    <asp:Label ID="lblInfo" runat="server" ForeColor="#00CC00"></asp:Label>
                                    &nbsp;
                                </td>
                            </tr>                    
                        </table>
                    </td>
                 </tr>
             </table>
        </div>
        <div>
            <table width=100% border=0>
                <tr>
                    <td width="100%">                                    
                        <table border=0 align=center>
                            <tr>
                                <td>
                                    <telerik:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Width="700px" Visible="False" BorderColor="#445E88" BorderStyle="Solid" BorderWidth="1px" ShowHistoryButtons="False" ShowNavigationGroup="False" ShowParametersButton="False"></telerik:ReportViewer>
                                </td>
                            </tr>                       
                        </table>
                    </td>
                 </tr>
             </table>
        </div>
        <asp:HiddenField ID="HiddenFieldSavedFileName" runat="server" />
    </form>
</body>
</html>
