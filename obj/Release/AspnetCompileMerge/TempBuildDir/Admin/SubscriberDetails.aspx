<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscriberDetails.aspx.cs" Inherits="AllLifePricingWeb.Admin.SubscriberDetails" %>

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
                        <telerik:AjaxUpdatedControl ControlID="RadTreeViewUserMenus" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    
    </div>
        <asp:Panel ID="Panel1" runat="server" GroupingText="Subscriber Details">
            <table width=100% border=0>
                <tr>
                    <td width="100%">
                        <table border=0 align=center>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Subscriber Name:"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="RadTextBoxSubscriberName" Runat="server">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                                </td>
                                <td>
                                     <telerik:RadTextBox ID="RadTextBoxPassword" Runat="server" TextMode="Password">
                                    </telerik:RadTextBox>
                                     <telerik:RadButton ID="RadButtonChangePwd" runat="server" ButtonType="LinkButton" OnClick="RadButton1_Click" Text="Change password">
                                     </telerik:RadButton>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Subscriber Code:"></asp:Label>
                                </td>
                                <td>
                                     <telerik:RadTextBox ID="RadTextBoxSubscriberCode" Runat="server">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Subscriber Status:"></asp:Label>
                                </td>
                                <td>
                                     <asp:CheckBox ID="CheckBoxStatus" runat="server" />
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Access to returnRisk:"></asp:Label>
                                </td>
                                <td>
                                     <asp:CheckBox ID="CheckBoxRisk" runat="server" />
                                </td>
                                  <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Access to returnPremium:"></asp:Label>
                                </td>
                                <td>
                                     <asp:CheckBox ID="CheckBoxPremium" runat="server" />
                                </td>
                            </tr>
                                  <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Access to returnCover:"></asp:Label>
                                </td>
                                <td>
                                     <asp:CheckBox ID="CheckBoxCover" runat="server" />
                                </td>
                            </tr>
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
                                            <asp:Label ID="Label7" runat="server" Text="Current Password:"></asp:Label>
                                        </td>
                                        <td>
                                             <telerik:RadTextBox ID="RadTextBoxCurrentPassword" Runat="server" TextMode="Password">
                                             </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="New Password:"></asp:Label>
                                        </td>
                                        <td>
                                             <telerik:RadTextBox ID="RadTextBoxNewPassword" Runat="server" TextMode="Password">
                                             </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="Confirm Password:"></asp:Label>
                                        </td>
                                        <td>
                                             <telerik:RadTextBox ID="RadTextBoxConfirmPassword" Runat="server" TextMode="Password">
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
                        <table border=0 align=center>
                            <tr>
                                <td>
                                    <telerik:RadButton ID="RadButtonSave" runat="server" OnClick="RadButtonSave_Click" Text="Save">
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    <telerik:RadButton ID="RadButtonClose" runat="server" OnClick="RadButtonClose_Click" Text="Close">
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
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Modal="true" EnableShadow="true" ReloadOnShow="true" 
     ShowContentDuringLoad="false" VisibleStatusbar="false">
    </telerik:RadWindowManager>
    </form>
    </body>
</html>
