<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserChangeOwnDetails.aspx.cs" Inherits="AllLifePricingWeb.Admin.UserChangeOwnDetails" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelsRenderMode="Inline">
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
                        <telerik:AjaxUpdatedControl ControlID="Image1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" Skin="Default" IsSticky="true" Style="position: absolute; top: 0; left: 0; height: 100%; width: 100%; margin:0; padding:0;">    
    </telerik:RadAjaxLoadingPanel>
    <br />
    <br />
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
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/green_ok.png" Visible="False" />
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
    <asp:Label ID="lblInfo" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
