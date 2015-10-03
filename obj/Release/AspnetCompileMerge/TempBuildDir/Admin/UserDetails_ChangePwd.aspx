<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDetails_ChangePwd.aspx.cs" Inherits="AllLifePricingWeb.Admin.UserDetails_ChangePwd" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    <script type="text/javascript">
        function refreshGrid(arg) {
            if (!arg) {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
            }
            else {
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest(arg);
            }
        }

        function CloseAndRebind(args) {
            GetRadWindow().BrowserWindow.refreshGrid(args);
            GetRadWindow().close();
        }

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

    </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
    
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTreeViewUserMenus" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadButtonChangePwd">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="panl_Changepassword" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadButtonSave">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadBtnChkFDB" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadBtnChkADB" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadBtnChkADCB" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadTreeViewUserMenus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    </div>
        <asp:Panel ID="Panel1" runat="server" GroupingText="User Details">
            <table width=100% border=0>
                <tr>
                    <td width="100%">
                        <table border=0 align=center>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="User fullname:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTextBoxUserFullname" Runat="server" TabIndex="1">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblTelephone" runat="server" Text="Telephone:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTxtTelephone" Runat="server" TabIndex="4">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTextBoxUsername" Runat="server" TabIndex="2">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblEmailUsername" runat="server" Text="Email Username:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTxtEmailUsername" Runat="server" TabIndex="5">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                                </td>
                                <td>
                                     <telerik:RadTextBox ID="RadTextBoxPassword" Runat="server" TextMode="Password" TabIndex="3">
                                    </telerik:RadTextBox>
                                     <telerik:RadButton ID="RadButtonChangePwd" runat="server" ButtonType="LinkButton" OnClick="RadButton1_Click" Text="Change password" TabIndex="3">
                                     </telerik:RadButton>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblEmailPassword" runat="server" Text="Email Password:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTxtEmailPassword" Runat="server" TextMode="Password" TabIndex="6">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Label runat="server" Font-Size="Smaller" ForeColor="Red" Text="Value is hidden for security reasons"></asp:Label>
                                </td>
                            </tr>
                        </table>
                         </td>
                        </tr>
                        <tr>
                            <td width="100%">
                            <asp:Panel ID="panl_Changepassword" runat="server" GroupingText="Change password" Visible="false">
                                <table border=0 align=center>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Current Password:"></asp:Label>
                                        </td>
                                        <td>
                                             <telerik:RadTextBox ID="RadTextBoxCurrentPassword" Runat="server" TextMode="Password" TabIndex="6">
                                             </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="New Password:"></asp:Label>
                                        </td>
                                        <td>
                                             <telerik:RadTextBox ID="RadTextBoxNewPassword" Runat="server" TextMode="Password" TabIndex="6">
                                             </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Confirm Password:"></asp:Label>
                                        </td>
                                        <td>
                                             <telerik:RadTextBox ID="RadTextBoxConfirmPassword" Runat="server" TextMode="Password" TabIndex="6">
                                             </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                             </asp:Panel>
                         </td>
                        </tr>
                         <tr>
                             <td width="100%">
                                 &nbsp;</td>
                </tr>
                         <tr>
                        <td width="100%">
                            &nbsp;</td>
                </tr>
                 <tr>
                    <td width="100%">
                        <table border=0 align=center>
                            <tr>
                                <td>
                                    <telerik:RadButton ID="RadButtonSave" runat="server" OnClick="RadButtonSave_Click" Text="Save" TabIndex="10">
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    <telerik:RadButton ID="RadButtonClose" runat="server" OnClick="RadButtonClose_Click" Text="Close" TabIndex="11">
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>                        
                     </td>
                 </tr>
            </table>            
        </asp:Panel>
    <asp:HiddenField ID="HiddenFieldUserID" runat="server" />
    <asp:HiddenField ID="HiddenFieldPwd" runat="server" />
    <asp:HiddenField ID="HiddenFieldEmailPwd" runat="server" />
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Modal="true" EnableShadow="true" ReloadOnShow="true" 
     ShowContentDuringLoad="false" VisibleStatusbar="false">
    </telerik:RadWindowManager>
    </form>
</body>
</html>
