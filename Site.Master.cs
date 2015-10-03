using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllLifePricingWeb
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        SqlConnection sqlConnectionX;
        SqlCommand sqlCommandX;
        SqlParameter sqlParam;
        SqlDataReader sqlDR;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                //ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                //ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
                if (Session["UserID"] != null)
                {
                    try
                    {
                        if (Session["UserMenu"] == null)
                        {
                            //WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();

                            //DataSet DS_UserMenu = WS.Get_UserMenu(Convert.ToInt32(Session["UserID"]));
                            //Session["UserMenu"] = DS_UserMenu;
                            DataSet dsUserMenu = Get_UserMenu(Convert.ToInt32(Session["UserID"]));
                            Session["UserMenu"] = dsUserMenu;
                           
                        }
                        RadMenu1.DataSource = (DataSet)Session["UserMenu"]; 
                        RadMenu1.DataFieldID = "MenuID";
                        RadMenu1.DataFieldParentID = "ParentID";   
                        RadMenu1.DataTextField = "Text";
                        RadMenu1.DataValueField = "MenuID";
                        RadMenu1.DataNavigateUrlField = "URL";
                        RadMenu1.DataBind();
                    }
                    catch (Exception)
                    {
                        Session.Remove("UserID");
                        Session.Remove("UserMenu");

                        //if (ex.Message == "This constraint cannot be enabled as not all values have corresponding parent values.")
                        //{
                        //    lblInfo.Text = "The menu cannot be displayed because there is a menu item that does not have a parent menu. Please check the user's access settings";
                        //}
                        //else
                        //{
                        //    lblInfo.Text = ex.Message;
                        //}
                        ////throw;
                    }
                }
                else
                {
                    RadMenu1.DataSource = null;
                }
            }
            else
            {
                // Validate the Anti-XSRF token
                //if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                //    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                //{
                //    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                //}
            }
        }       

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        public DataSet Get_UserMenu(int UserID)
        {
            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_UserMenu";

                sqlParam = new SqlParameter("UserID", UserID);
                sqlCommandX.Parameters.Add(sqlParam);

                SqlDataAdapter daX = new SqlDataAdapter(sqlCommandX);
                DataSet dsX = new DataSet();

                daX.Fill(dsX);

                return dsX;
            }
            finally
            {
                sqlConnectionX.Close();
            }
        }      
    }

}