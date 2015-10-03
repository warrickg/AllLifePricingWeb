using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllLifePricingWeb
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            //string strReturnVal;

            //strReturnVal = WS.MyTest(TextBox1.Text, Convert.ToInt16(TextBox2.Text));
            //Label1.Text = strReturnVal;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            //string strReturnVal;

            //strReturnVal = WS.MyTestDecrypt(TextBox1.Text, TextBox3.Text);
            //Label2.Text = strReturnVal;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable ReturnDT;

            ReturnDT = WS.returnPremium(TextBox4.Text, TextBox5.Text, TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, TextBox11.Text, TextBox12.Text, TextBox13.Text);
            //ReturnDT = WS.loginSubscriber(TextBox4.Text, TextBox5.Text, TextBox6.Text);
            GridView1.DataSource = ReturnDT;
            GridView1.DataBind();
            //Label2.Text = strReturnVal;
        }
    }
}