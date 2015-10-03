using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllLifePricingWeb
{
    public partial class Testing : System.Web.UI.Page
    {
        WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                //string AppPath = HttpContext.Current.Request.ApplicationPath;
                //AppPath += "/Login.aspx";
                //Response.Redirect(AppPath);
                Response.Redirect("Login.aspx?ID=9", true);
            } 
        }

        protected void RadButtonRunPremium_Click(object sender, EventArgs e)
        {            
            DataTable ReturnDT;

            ReturnDT = WS.returnPremium(TextBox4.Text, TextBox5.Text, TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, TextBox12.Text, TextBox13.Text);
                        
            GridView1.DataSource = ReturnDT;
            GridView1.DataBind();
            //Label2.Text = strReturnVal;
        }

        protected void RadButtonRunRisk_Click(object sender, EventArgs e)
        {
            DataTable ReturnDT;

            ReturnDT = WS.returnRisk(TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox14.Text, TextBox15.Text, TextBox16.Text, TextBox17.Text, TextBox31.Text, TextBox18.Text, TextBox32.Text, TextBox33.Text, TextBox19.Text, TextBox20.Text, TextBoxDurationModifierCode.Text);

            GridView2.DataSource = ReturnDT;
            GridView2.DataBind();
        }

        protected void RadButtonRunCover_Click(object sender, EventArgs e)
        {
            DataTable ReturnDT;

            ReturnDT = WS.returnCover(TextBox21.Text, TextBox22.Text, TextBox23.Text, TextBox24.Text, TextBox25.Text, TextBox26.Text, TextBox27.Text, TextBox28.Text, TextBox29.Text, TextBox30.Text);

            GridView3.DataSource = ReturnDT;
            GridView3.DataBind();
        }
    }
}