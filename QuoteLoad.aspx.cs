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

namespace AllLifePricingWeb
{
    public partial class QuoteLoad : System.Web.UI.Page
    {
        string strMagnumID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            strMagnumID = Request.QueryString["MagnumID"].ToString();
            if (!IsPostBack)
            {
                lblInfo.Text = "";
            }

            //Telerik
        }

        protected void RadGridCurrentLoad_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable ReturnDT;

            string strSettingValue = string.Empty;


            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlParameter sqlParam;
            SqlDataReader sqlDR;

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_QuoteAuditTrailByMagnumID";

                sqlParam = new SqlParameter("MagnumID", strMagnumID);
                sqlCommandX.Parameters.Add(sqlParam);

                //sqlDR = sqlCommandX.ExecuteReader();
                //while (sqlDR.Read())
                //{
                //    strSettingValue = sqlDR.GetValue(0).ToString();
                //}
                sqlDR = sqlCommandX.ExecuteReader();
                DataTable dt = new DataTable("AuditTrail");
                dt.Load(sqlDR);

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                sqlConnectionX.Close();

                //RadGridCurrentLoad.DataSource = dt;
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }


        }

        protected void RadGridCurrentLoad_DetailTableDataBind(object sender, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "Details":
                    {
                        //string strADLDocNo = dataItem.GetDataKeyValue("ReturnNo").ToString();
                        //WHWS.AstuteWSSoapClient WebService = new WHWS.AstuteWSSoapClient();
                        //WHWS.User _User = new WHWS.User();
                        //WHWS.WHProducts _WHProducts = new WHWS.WHProducts();
                        //WHWS.WHProduct _WHProduct = new WHWS.WHProduct();

                        //_User = (WHWS.User)Session["User"];
                        //_WHProduct.ADLType = "Return";
                        //_WHProduct.ASUUserID = _User.UserID;
                        //_WHProduct.ADLDocNo = Convert.ToInt32(strADLDocNo);

                        //DataTable DT = WebService.SELECT_CurrentWHTransactionHistoryDetails(_WHProduct);
                        //e.DetailTableView.DataSource = DT;
                        break;
                    }

                case "DetailTable":
                    {
                        string strADLDocNo = dataItem.GetDataKeyValue("ADLLine").ToString();
                        //WHWS.AstuteWSSoapClient WebService = new WHWS.AstuteWSSoapClient();
                        //WHWS.User _User = new WHWS.User();
                        //WHWS.WHProducts _WHProducts = new WHWS.WHProducts();
                        //WHWS.WHProduct _WHProduct = new WHWS.WHProduct();

                        //_User = (WHWS.User)Session["User"];
                        //_WHProduct.ADLType = "Return";
                        //_WHProduct.ASUUserID = _User.UserID;
                        //_WHProduct.ADLDocNo = Convert.ToInt32(strADLDocNo);

                        //DataTable DT = WebService.SELECT_CurrentWHTransactionHistoryDetails(_WHProduct);
                        //e.DetailTableView.DataSource = DT;
                        break;
                    }
            }

        }

        protected void RadBtnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseAndRebind('0');", true);
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();

            string strSettingValue = string.Empty;


            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlParameter sqlParam;
            SqlDataReader sqlDR;

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_QuoteAuditTrailByMagnumID";

                sqlParam = new SqlParameter("MagnumID", strMagnumID);
                sqlCommandX.Parameters.Add(sqlParam);

                //sqlDR = sqlCommandX.ExecuteReader();
                //while (sqlDR.Read())
                //{
                //    strSettingValue = sqlDR.GetValue(0).ToString();
                //}
                sqlDR = sqlCommandX.ExecuteReader();
                DataTable dt = new DataTable("AuditTrail");
                dt.Load(sqlDR);

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                sqlConnectionX.Close();

                RadGrid1.DataMember = "AuditID";
                RadGrid1.DataSource = dt;
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    RadGrid1.MasterTableView.Items[0].Expanded = true;
            //    RadGrid1.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            //}
        }

        protected void RadGrid1_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {

            GridDataItem parentItem = e.DetailTableView.ParentItem as GridDataItem;            
            if (parentItem.Edit)
            {
                return;
            }

            //if (e.DetailTableView.DataMember == "OrderDetails")
            //{
                //DataSet ds = (DataSet)e.DetailTableView.DataSource;
                //DataView dv = ds.Tables["OrderDetails"].DefaultView;
                //dv.RowFilter = "CustomerID = '" + parentItem["CustomerID"].Text + "'";
                //e.DetailTableView.DataSource = dv;

                GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                string strAuditTrailID = dataItem.GetDataKeyValue("AuditID").ToString();

                WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();

                string strSettingValue = string.Empty;

                SqlConnection sqlConnectionX;
                SqlCommand sqlCommandX;
                SqlParameter sqlParam;
                SqlDataReader sqlDR;

                try
                {
                    sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                    sqlConnectionX.Open();

                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_SELECT_QuoteOptionAuditTrailByAuditTrailID";

                    sqlParam = new SqlParameter("QuoteAuditTrailID", strAuditTrailID);
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    DataTable dt = new DataTable("AuditTrail");
                    dt.Load(sqlDR);

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();
                    sqlConnectionX.Close();

                    e.DetailTableView.DataSource = dt;
                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                }
            //}

        }

        protected void CheckBoxSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbx = (CheckBox)sender;
            GridDataItem itemSelected = (GridDataItem)chkbx.NamingContainer;
            string strQuoteOptionAuditTrailID = itemSelected.Cells[2].Text;
            bool chkSatus = chkbx.Checked;

            foreach (GridDataItem item in RadGrid1.Items)
            {
                if (item.HasChildItems == true)
                {
                    foreach (GridDataItem ChildItem in item.ChildItem.NestedTableViews[0].Items)
                    {
                        string s2 = ChildItem.Cells[0].Text;

                        if ((ChildItem.FindControl("CheckBoxSelect") as CheckBox).Checked == true)
                        {
                            if (strQuoteOptionAuditTrailID != ChildItem.Cells[2].Text)
                            {
                                (ChildItem.FindControl("CheckBoxSelect") as CheckBox).Checked = false;
                            }
                        }
                    }
                }
                //string s1 = item.Cells[1].Text;               
            }
        }

        protected void RadBtnOpenQuote_Click(object sender, EventArgs e)
        {
            string strAuditID = string.Empty; string strAuditIDtemp = string.Empty;
            string strQuoteOptionAuditTrailID = string.Empty;
            string strSendString = string.Empty;

            foreach (GridDataItem item in RadGrid1.Items)
            {
                if (item.HasChildItems == true)
                {
                    //strAuditIDtemp = item.Cells[2].ToString();

                    foreach (GridDataItem ChildItem in item.ChildItem.NestedTableViews[0].Items)
                    {
                        if ((ChildItem.FindControl("CheckBoxSelect") as CheckBox).Checked == true)
                        {
                            strQuoteOptionAuditTrailID = ChildItem.Cells[2].Text;
                            //strAuditID = strAuditIDtemp;
                            strAuditID = item.Cells[2].Text;
                            break;
                        }
                    }
                }               
            }

            if (strQuoteOptionAuditTrailID == "")
            {
                ///RadAjaxManager1.ResponseScripts.Add("radalert('Please note that you did not select a quote so nothing will be loaded',300,100,'Attention');");
                lblInfo.Text = "Please select a quote option before you can load a quote";
            }
            else
            {
                strSendString = "CloseAndRebind('AuditID=" + strAuditID + ",QuoteOptionAuditTrailID=" + strQuoteOptionAuditTrailID + "');";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseAndRebind('w');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", strSendString, true);
            }
        } 
       
    }
}