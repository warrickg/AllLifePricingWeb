<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AllLifePricingWeb.Login1" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <telerik:RadAjaxManager runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadButtonLogin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadTextBoxUsername" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTextBoxPassword" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTextBoxCode" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

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
     <br />
    <telerik:RadPageLayout ID="RadPageLayout1" runat="server">
        <telerik:LayoutRow RowType="Generic">
                <Rows>                         
                    <telerik:LayoutRow RowType="Generic" CssClass="content">
                        <Rows>
                            <telerik:LayoutRow RowType="Container" WrapperHtmlTag="Div">
                                <Columns>
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12" >   
                                        <div class="shadow1">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="RadButtonLogin">        
                                                <table class="loginTable">
                                                <tr>
                                                    <td><asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label></td>
                                                    <td>
                                                        <telerik:RadTextBox ID="RadTextBoxUsername" Runat="server" LabelWidth="64px" Resize="None" Width="160px">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label></td>
                                                    <td>
                                                        <telerik:RadTextBox ID="RadTextBoxPassword" Runat="server" LabelWidth="64px" Resize="None" Width="160px" TextMode="Password">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>       
                                                <tr>
                                                    <td>

                                                    </td>
                                                    <td>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: center">
                                                        <telerik:RadButton ID="RadButtonLogin" runat="server" Text="Login" OnClick="RadButtonLogin_Click">
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>    
                                            </asp:Panel>
                                            <table class="nav-justified">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblInfo" runat="server" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            </div>
                                    </telerik:LayoutColumn> 
                                </Columns>
                            </telerik:LayoutRow>
                        </Rows>
                    </telerik:LayoutRow>                    
                </Rows>
            </telerik:LayoutRow>
    </telerik:RadPageLayout>
</asp:Content>