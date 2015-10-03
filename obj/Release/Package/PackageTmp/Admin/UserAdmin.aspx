<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="AllLifePricingWeb.Admin.UserAdmin" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                        function ShowEditForm_old(id, rowIndex) {
                            var grid = $find("<%= RadGridUsers.ClientID %>");

                            var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                            grid.get_masterTableView().selectItem(rowControl, true);

                            window.radopen("EditFormCS.aspx?UserID=" + id, "UserListDialog");
                            return false;

                        }

                        function ShowInsertForm(id, rowIndex) {
                            var oWindow = radopen('UserDetails.aspx?UserID=0', "RadWindowManager1");
                            //oWindow.maximize(); 
                        }

                        function refreshGrid(arg) {
                            if (!arg) {
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                            }
                            else {
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RebindAndNavigate");
                            }
                        }
                        function RowDblClick(sender, eventArgs) {
                            window.radopen("UserDetails.aspx?UserID=" + eventArgs.getDataKeyValue("UserID"), "RadWindowManager1");
                        }
                    </script>
                </telerik:RadCodeBlock>
        <br />
        <div>
        <table style="width: 70%; text-align: left" align="center">            
            <tr>            
                <td>

                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGridUsers" UpdatePanelCssClass="" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>

                    <br />
                    <telerik:RadButton ID="RadButtonAddUser" runat="server" Text="Add User" OnClick="RadButtonAddUser_Click">
                        <Icon PrimaryIconCssClass="rbAdd" PrimaryIconLeft="4" PrimaryIconTop="3"></Icon>
                    </telerik:RadButton>

                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="RadGridUsers" runat="server" AllowFilteringByColumn="True" 
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" Width="100%" OnNeedDataSource="RadGrid1_NeedDataSource" PageSize="30">
                        <GroupingSettings CaseSensitive="false" />
                        <AlternatingItemStyle BackColor="#E0E0E0" />
                        <MasterTableView DataKeyNames="UserID" ClientDataKeyNames="UserID">                                 
                                <RowIndicatorColumn Visible="True" 
                                    FilterControlAltText="Filter RowIndicator column">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" 
                                    FilterControlAltText="Filter ExpandColumn column">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="False" UniqueName="TemplateEditColumn" HeaderText="Edit">
                                        <ItemTemplate>
                                            <a href="#"  
                                                onclick="var oWindow=radopen('UserDetails.aspx?UserID=<%# DataBinder.Eval(Container.DataItem, "UserID") %>', 'RadWindowManager1');">                                                                                             
                                                Edit
                                            </a>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="UserID" 
                                        FilterControlAltText="Filter column column" UniqueName="column" 
                                        Visible="False">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Username" 
                                        FilterControlAltText="Filter column1 column" HeaderText="User Name" 
                                        UniqueName="column1" ItemStyle-HorizontalAlign="Left">
<ColumnValidationSettings>
<ModelErrorMessage Text=""></ModelErrorMessage>
</ColumnValidationSettings>

                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </telerik:GridBoundColumn>                                                                         
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>                                
                            </MasterTableView>
                        <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="500px" />
                                <ClientEvents OnRowDblClick="RowDblClick" />
                            </ClientSettings>
                            <FilterMenu EnableImageSprites="False">
                                <WebServiceSettings>
                                    <ODataSettings InitialContainerName="">
                                    </ODataSettings>
                                </WebServiceSettings>
                            </FilterMenu>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
                </td>
            </tr>
            <tr>
                <td>

                    <asp:Label ID="lblInfo" runat="server" ForeColor="Red"></asp:Label>

                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="850px" 
                        Height="800px" Modal="true" EnableShadow="true" ReloadOnShow="true" 
                        ShowContentDuringLoad="false" VisibleStatusbar="false">
                    </telerik:RadWindowManager>
                    <telerik:RadWindow ID="RadWindow1" runat="server"></telerik:RadWindow>
                </td>
            </tr>
        </table>
    </div>
    
</asp:Content>
