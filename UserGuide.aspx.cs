using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllLifePricingWeb
{
    public partial class UserGuide : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReadPDFFile();
        }

        private void ReadPDFFile()
        {
            string path = Server.MapPath(@"Content\UserGuide.PDF");
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                this.Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
        }
    }
}