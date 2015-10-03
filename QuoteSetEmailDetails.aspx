<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuoteSetEmailDetails.aspx.cs" Inherits="AllLifePricingWeb.QuoteSetEmailDetails" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">       
        <script type="text/javascript">
            function Close(args) {
                GetRadWindow().BrowserWindow.PopupWindowResponseClose();
                GetRadWindow().close();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)

                return oWindow;
            }

        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .auto-style1 {
            width: 121px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
    
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadBtnSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTxtEmailUsername" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadTxtEmailPassword" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="lblinfo" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" IsSticky="True" Skin="Default" Style="position: absolute; top: 0; left: 0; height: 100%; width: 100%; margin:0; padding:0;">            
        </telerik:RadAjaxLoadingPanel>        
        <table width=100% border=0>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lblMessage" runat="server" Text="Please enter your email details below:" Font-Bold="False" Font-Size="Large" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>                
                <td class="auto-style1">
                    <asp:Image ID="Image1" runat="server" Height="134px" ImageUrl="~/EmailLogin2.jpg" Width="121px" />
                </td>
                <td width="80%" style="background-color: #445E8A">
                    <table border=0 align=center>                        
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Username:" ForeColor="White"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="RadTxtEmailUsername" runat="server"></telerik:RadTextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailUsername" runat="server" ErrorMessage="Username required" ControlToValidate="RadTxtEmailUsername" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Password:" ForeColor="White"></asp:Label>
                            </td>
                            <td>                                
                                <telerik:RadTextBox ID="RadTxtEmailPassword" runat="server" TextMode="Password"></telerik:RadTextBox>                                
                            </td>
                            <td>                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailPassword" runat="server" ErrorMessage="Password required" ControlToValidate="RadTxtEmailPassword" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: center">
                                <telerik:RadButton ID="RadBtnSave" runat="server" Text="Save details" OnClick="RadBtnSave_Click" ValidationGroup="btnSave">
                                    <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="3"></Icon>
                                </telerik:RadButton>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblinfo" runat="server" Text="" ForeColor="Red"></asp:Label>        
    </div>
    </form>
</body>
</html>
