<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuoteLoad.aspx.cs" Inherits="AllLifePricingWeb.QuoteLoad" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">       
        <script type="text/javascript">
            function CloseAndRebind(args) {
                GetRadWindow().BrowserWindow.PopupWindowResponse(args);
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
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="HiddenFieldSelected_QuoteOptionAuditTrailID" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadBtnOpenQuote">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    </UpdatedControls>
                </telerik:AjaxSetting>                
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <br />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" Skin="Default" IsSticky="true" Style="position: absolute; top: 0; left: 0; height: 100%; width: 100%;">            
            
        </telerik:RadAjaxLoadingPanel>
        <table style="width: 100%;">
            <tr>
                <td>
                    <telerik:RadGrid ID="RadGrid1" runat="server" ShowStatusBar="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowPaging="True"
                    OnDetailTableDataBind="RadGrid1_DetailTableDataBind" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender" GroupPanelPosition="Top">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView EnableHierarchyExpandAll="true" DataKeyNames="AuditID">
                            <DetailTables>
                                <telerik:GridTableView EnableHierarchyExpandAll="true" DataKeyNames="QuoteOptionAuditTrailID" Width="100%"
                                    runat="server" AllowSorting="False">
                                    <Columns>
                                        <telerik:GridBoundColumn SortExpression="QuoteOptionAuditTrailID" HeaderText="QuoteOptionAuditTrailID" HeaderButtonType="TextButton"
                                            DataField="QuoteOptionAuditTrailID" UniqueName="QuoteOptionAuditTrailID">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="OptionNumber" HeaderText="OptionNumber" HeaderButtonType="TextButton" DataField="OptionNumber" UniqueName="OptionNumber" >
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="CoverLife" HeaderText="CoverLife" HeaderButtonType="TextButton" DataField="CoverLife" UniqueName="CoverLife">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="PremiumLife" HeaderText="PremiumLife" HeaderButtonType="TextButton" DataField="PremiumLife" UniqueName="PremiumLife">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="CoverDisability" HeaderText="CoverDisability" HeaderButtonType="TextButton" DataField="CoverDisability" UniqueName="CoverDisability">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="PremiumDisability" HeaderText="PremiumDisability" HeaderButtonType="TextButton" DataField="PremiumDisability" UniqueName="PremiumDisability">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="Selected" HeaderText="Accepted" HeaderButtonType="TextButton" DataField="Selected" UniqueName="Selected">
                                        </telerik:GridBoundColumn>                                        
                                        <telerik:GridTemplateColumn UniqueName="checked" DataField="checked" HeaderText="Select" > 
                                            <ItemTemplate> 
                                                <asp:CheckBox ID="CheckBoxSelect" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBoxSelect_CheckedChanged" /> 
                                            </ItemTemplate>  
                                        </telerik:GridTemplateColumn>
                                    </Columns>                                    
                                </telerik:GridTableView>
                            </DetailTables>
                            <Columns>
                                <telerik:GridBoundColumn SortExpression="AuditID" HeaderText="AuditID" HeaderButtonType="TextButton"
                                    DataField="AuditID" UniqueName="AuditID" FilterControlAltText="Filter AuditID column">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="QuoteDate" HeaderText="Quote Date" HeaderButtonType="TextButton"
                                    DataField="QuoteDate" UniqueName="QuoteDate" FilterControlAltText="Filter QuoteDate column">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="ClientName" HeaderText="Client Name" HeaderButtonType="TextButton"
                                    DataField="ClientName" UniqueName="ClientName" FilterControlAltText="Filter ClientName column">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>               
            </tr>           
        </table>
        <asp:Label ID="lblInfo" runat="server" Text="" ForeColor="Red"></asp:Label>
        <br />        
        <br />        
        <table width=100% border=0>
            <tr>
                <td width="100%">                                    
                    <table border=0 align=center>
                        <tr>
                            <td>
                                <telerik:RadButton ID="RadBtnOpenQuote" runat="server" Text="Open Selected Quote" OnClick="RadBtnOpenQuote_Click">                                    
                                    <Icon PrimaryIconCssClass="rbOk" PrimaryIconLeft="4" PrimaryIconTop="3"></Icon>
                                </telerik:RadButton>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadButton ID="RadBtnClose" runat="server" Text="Close Window" OnClick="RadBtnClose_Click">
                                    <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="3"></Icon>
                                </telerik:RadButton>
                            </td>
                        </tr>                       
                    </table>
                </td>
             </tr>
         </table>
        <asp:HiddenField ID="HiddenFieldSelected_QuoteOptionAuditTrailID" runat="server" />
    </div>
    </form>
</body>
</html>
