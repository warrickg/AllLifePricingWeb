<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuoteEmailQuick.aspx.cs" Inherits="AllLifePricingWeb.QuoteEmailQuick" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">       
        <script type="text/javascript">
            function CloseReset(args) {
                GetRadWindow().BrowserWindow.PopupWindowResponseCloseReset();
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
    <div>
    
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>                       
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadBtnSendEmail" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadbtnClose" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenFieldEmailUserName" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenFieldEmailUserPassword" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="Image1" UpdatePanelCssClass="" /> 
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadBtnSendEmail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadTxtEmailTo" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadTxtSubject" />
                        <telerik:AjaxUpdatedControl ControlID="RadEditor1" />
                        <telerik:AjaxUpdatedControl ControlID="lblInfo" />
                        <telerik:AjaxUpdatedControl ControlID="RadBtnSendEmail" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadbtnClose" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="RequiredFieldValidatorEmailTo" UpdatePanelCssClass="" />
                        <telerik:AjaxUpdatedControl ControlID="Image1" UpdatePanelCssClass="" />                        
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadbtnClose">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" IsSticky="True" Skin="Default" Style="position: absolute; top: 0; left: 0; height: 100%; width: 100%; margin:0; padding:0;">            
        </telerik:RadAjaxLoadingPanel>
        <table width=100% border=0>
            <tr>
                <td width="100%">
                    <table border=0 align=center>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Email to:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="RadTxtEmailTo" runat="server" Width="300px"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailTo" runat="server" ErrorMessage="Email required" ControlToValidate="RadTxtEmailTo" ForeColor="Red" ValidationGroup="btnSend"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Subject:"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="RadTxtSubject" runat="server" Width="600px" Text="Requested Life insurance quotes"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Attachment:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:Label ID="lblAttachments" runat="server"></asp:Label>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width=100% border=0>
            <tr>
                <td width="100%">
                    <table border=0 align=center>
                        <tr>
                            <td>
                                <telerik:RadEditor ID="RadEditor1" Runat="server" EditModes="Design" ContentFilters="MakeUrlsAbsolute, DefaultFilters">
                                    <Tools>
                                        <telerik:EditorToolGroup Tag="MainToolbar">
                                            <telerik:EditorTool Name="AjaxSpellCheck" />
                                            <telerik:EditorTool Name="SelectAll" ShortCut="CTRL+A / CMD+A" />
                                            <telerik:EditorTool Name="Cut" ShortCut="CTRL+X / CMD+X" />
                                            <telerik:EditorTool Name="Copy" ShortCut="CTRL+C / CMD+C" />
                                            <telerik:EditorTool Name="Paste" ShortCut="CTRL+V / CMD+V" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorSplitButton Name="Undo">
                                            </telerik:EditorSplitButton>
                                            <telerik:EditorSplitButton Name="Redo">
                                            </telerik:EditorSplitButton>
                                        </telerik:EditorToolGroup>                                                                                
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorDropDown Name="FormatBlock">
                                            </telerik:EditorDropDown>
                                            <telerik:EditorDropDown Name="FontName">
                                            </telerik:EditorDropDown>
                                            <telerik:EditorDropDown Name="RealFontSize">
                                            </telerik:EditorDropDown>
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorTool Name="AbsolutePosition" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="Bold" ShortCut="CTRL+B / CMD+B" />
                                            <telerik:EditorTool Name="Italic" ShortCut="CTRL+I / CMD+I" />
                                            <telerik:EditorTool Name="Underline" ShortCut="CTRL+U / CMD+U" />
                                            <telerik:EditorTool Name="StrikeThrough" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="JustifyLeft" />
                                            <telerik:EditorTool Name="JustifyCenter" />
                                            <telerik:EditorTool Name="JustifyRight" />
                                            <telerik:EditorTool Name="JustifyFull" />
                                            <telerik:EditorTool Name="JustifyNone" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="Indent" />
                                            <telerik:EditorTool Name="Outdent" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="InsertOrderedList" />
                                            <telerik:EditorTool Name="InsertUnorderedList" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="ToggleTableBorder" />
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorSplitButton Name="ForeColor">
                                            </telerik:EditorSplitButton>
                                            <telerik:EditorSplitButton Name="BackColor">
                                            </telerik:EditorSplitButton>
                                            <telerik:EditorDropDown Name="ApplyClass">
                                            </telerik:EditorDropDown>
                                        </telerik:EditorToolGroup>                                       
                                    </Tools>
                                    <Content>
                                    
</Content>
                                    <ImageManager EnableImageEditor="False" EnableThumbnailLinking="False" />
                                    <TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
                                </telerik:RadEditor>
                            </td>
                        </tr>
                    </table>
                    </td>
            </tr>
        </table>
        <table width=100% border=0>
            <tr>
                <td width="100%">
                    <table border=0 align=center>
                        <tr>
                            <td>
                                <asp:Label ID="lblInfo" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>                    
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <table border=0 align=center>
                        <tr>                            
                            <td>
                                <telerik:RadButton ID="RadBtnSendEmail" runat="server" Text="Send Email" OnClick="RadBtnSendEmail_Click" ValidationGroup="btnSend">
                                    <Icon PrimaryIconCssClass="rbMail" PrimaryIconLeft="4" PrimaryIconTop="3"></Icon>
                                </telerik:RadButton>
                            </td>
                            <td>                                
                                &nbsp;&nbsp;

                            </td>
                            <td>
                                <telerik:RadButton ID="RadbtnClose" runat="server" Text="Close window" OnClick="RadbtnClose_Click" Visible="false"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center" colspan="3">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/green_ok.png" Visible="False" />
                            </td>
                        </tr>                   
                    </table>
                </td>
            </tr>
        </table>
    </div>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <asp:HiddenField ID="HiddenFieldEmailUserName" runat="server" />
        <asp:HiddenField ID="HiddenFieldEmailUserPassword" runat="server" />
        <asp:HiddenField ID="HiddenFieldUserFullName" runat="server" />
        <asp:HiddenField ID="HiddenFieldUserTelephone" runat="server" />
    </form>
</body>
</html>
