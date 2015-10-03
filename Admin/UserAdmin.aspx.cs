using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace AllLifePricingWeb.Admin
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        SqlConnection sqlConnectionX;
        SqlCommand sqlCommandX;
        SqlParameter sqlParam;
        SqlDataReader sqlDR;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindDataGrid();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Response.Redirect("~'/Login.aspx?ID=9", true);
                //throw;
            }
        }

        private DataTable Get_Users()
        {
            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Select_Users";

                SqlDataReader dr = sqlCommandX.ExecuteReader();
                DataTable dt = new DataTable("Users");
                dt.Load(dr);

                return dt;

                sqlConnectionX.Close();
            }
            catch(Exception ex)
            {
                lblInfo.Text = ex.Message;

                DataTable dt = new DataTable("Result");
                dt.Columns.Add("Result", typeof(string));
                dt.Columns.Add("UserID", typeof(string));

                DataRow dr = dt.NewRow();
                dr["Result"] = ex.Message;
                dr["UserID"] = DBNull.Value;
                dt.Rows.Add(dr);

                return dt;
                
            }
        }

        private void BindDataGrid()
        {
            //WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable DTUsers = Get_Users();
            RadGridUsers.DataSource = DTUsers;
            RadGridUsers.Rebind();            
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable DTUsers = Get_Users();
            RadGridUsers.DataSource = DTUsers;
        }

        protected void RadButtonAddUser_Click(object sender, EventArgs e)
        {
            RadWindow newWindow = new RadWindow();
            newWindow.NavigateUrl = "UserDetails.aspx?UserID=0";
            newWindow.Modal = true;
            newWindow.Height = 600;
            newWindow.Width = 850;
            //newWindow.VisibleStatusbar = false;
            newWindow.DestroyOnClose = true;
            newWindow.Title = "New User";
            newWindow.VisibleOnPageLoad = true;
            newWindow.Behaviors = WindowBehaviors.Resize;
            newWindow.Behaviors = WindowBehaviors.Close;
            RadWindowManager1.Windows.Add(newWindow);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                //Response.Redirect("UserAdmin.aspx");
                RadWindowManager1.Windows.Clear();
                RadGridUsers.MasterTableView.CurrentPageIndex = RadGridUsers.MasterTableView.PageCount - 1;
                RadGridUsers.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridUsers.MasterTableView.SortExpressions.Clear();
                RadGridUsers.MasterTableView.GroupByExpressions.Clear();
                RadGridUsers.MasterTableView.CurrentPageIndex = RadGridUsers.MasterTableView.PageCount - 1;
                RadGridUsers.Rebind();
            }
        }
    }
}