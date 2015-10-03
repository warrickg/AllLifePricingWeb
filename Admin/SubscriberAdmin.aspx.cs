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
using AllLifePricingWeb.Classes;

namespace AllLifePricingWeb.Admin
{
    public partial class SubscriberAdmin : System.Web.UI.Page
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
                Response.Redirect("~/Login.aspx?ID=9", true);
                //throw;
            }
        }

        private DataTable Get_Subscribers()
        {
            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Select_Subscribers";

                SqlDataReader dr = sqlCommandX.ExecuteReader();
                DataTable dt = new DataTable("Subscribers");
                dt.Load(dr);

                return dt;

                sqlConnectionX.Close();
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;

                DataTable dt = new DataTable("Result");
                dt.Columns.Add("Result", typeof(string));
                dt.Columns.Add("SubscriberID", typeof(string));

                DataRow dr = dt.NewRow();
                dr["Result"] = ex.Message;
                dr["SubscriberID"] = DBNull.Value;
                dt.Rows.Add(dr);

                return dt;

            }
        }

        private void BindDataGrid()
        {
            WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable DTUsers = Get_Subscribers();
            RadGridSubscribers.DataSource = DTUsers;
            RadGridSubscribers.Rebind();            
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable DTUsers = Get_Subscribers();
            RadGridSubscribers.DataSource = DTUsers;         
        }

        protected void RadButtonAddSubscriber_Click(object sender, EventArgs e)
        {
            RadWindow newWindow = new RadWindow();
            newWindow.NavigateUrl = "SubscriberDetails.aspx?SID=0";
            newWindow.Modal = true;
            newWindow.Height = 600;
            newWindow.Width = 850;
            //newWindow.VisibleStatusbar = false;
            newWindow.DestroyOnClose = true;
            newWindow.Title = "New Subscriber";
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
                RadGridSubscribers.MasterTableView.CurrentPageIndex = RadGridSubscribers.MasterTableView.PageCount - 1;
                RadGridSubscribers.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                RadGridSubscribers.MasterTableView.SortExpressions.Clear();
                RadGridSubscribers.MasterTableView.GroupByExpressions.Clear();
                RadGridSubscribers.MasterTableView.CurrentPageIndex = RadGridSubscribers.MasterTableView.PageCount - 1;
                RadGridSubscribers.Rebind();
            }
        }
    }
}