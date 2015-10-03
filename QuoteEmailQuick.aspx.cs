using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace AllLifePricingWeb
{
    public partial class QuoteEmailQuick : System.Web.UI.Page
    {
        string strMagnumID = string.Empty;
        string strQuoteAuditID = string.Empty;
        string strFilePath = string.Empty;
        string strUserFullname = string.Empty;
        string strUserEmailUsername = string.Empty; string strUserEmailPassword = string.Empty; string strUserTelephone = string.Empty;
        string strEmailAddress = string.Empty;
        string mySalt = "AllLife";
        Int32 intUserID = 0;
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            strMagnumID = Request.QueryString["MagnumID"].ToString();
            strQuoteAuditID = Request.QueryString["QuoteAuditID"].ToString();
            strFilePath = Request.QueryString["FilePath"].ToString();

            string path = strFilePath;
            string[] pathArr = path.Split('\\');
            //string[] fileArr = pathArr.Last().Split('.');
            string strfileName = pathArr.Last().ToString();
            string strClientName = string.Empty; string strContent = string.Empty; 
            intUserID = Convert.ToInt32(Request.QueryString["UserID"]);

            try
            {
                lblAttachments.Text = strfileName;

                if (!IsPostBack)
                {
                    //DataSet DS = new DataSet();

                    #region "SQL"
                    SqlConnection sqlConnectionX;
                    SqlCommand sqlCommandX;
                    SqlParameter sqlParam;
                    SqlDataReader sqlDR;

                    sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                    sqlConnectionX.Open();

                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_SELECT_QuoteDetailByAuditTrailID";

                    sqlParam = new SqlParameter("QuoteAuditTrailID", strQuoteAuditID);
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    while (sqlDR.Read())
                    {
                        if (!sqlDR.IsDBNull(0))
                            strClientName = sqlDR.GetValue(0).ToString();
                        if (!sqlDR.IsDBNull(4))
                            strEmailAddress = sqlDR.GetValue(4).ToString();
                    }

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();

                    //Get the info for the user
                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_Select_UserByID";

                    sqlParam = new SqlParameter("UserID", intUserID);
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    while (sqlDR.Read())
                    {
                        if (!sqlDR.IsDBNull(4))
                        {
                            strUserFullname = sqlDR.GetValue(4).ToString();
                            HiddenFieldUserFullName.Value = strUserFullname;
                        }

                        if (!sqlDR.IsDBNull(5))
                        {
                            strUserEmailUsername = sqlDR.GetValue(5).ToString();
                            HiddenFieldEmailUserName.Value = strUserEmailUsername;
                        }

                        if (!sqlDR.IsDBNull(6))
                        {
                            //strUserEmailPassword = sqlDR.GetValue(6).ToString();
                            //HiddenFieldEmailUserPassword.Value = strUserEmailPassword;
                            strUserEmailPassword = sqlDR.GetValue(6).ToString();
                            if (strUserEmailPassword.Length > 0)
                            {
                                string decryptedstring = Decrypt(strUserEmailPassword, mySalt);
                                HiddenFieldEmailUserPassword.Value = decryptedstring;
                            }
                        }

                        if (!sqlDR.IsDBNull(7))
                        {
                            strUserTelephone = sqlDR.GetValue(7).ToString();
                            HiddenFieldUserTelephone.Value = strUserTelephone;
                        }
                    }

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();
                    sqlConnectionX.Close();
                    sqlConnectionX.Dispose();
                    #endregion

                    RadTxtEmailTo.Text = strEmailAddress;

                    #region "Email Content"
                    strContent = "Dear " + strClientName;
                    strContent += "<p>";
                    strContent += "Thank you for showing interest in our products. Please find attached the documents you have requested.";
                    strContent += "<p>";
                    strContent += "Please read through the documents and contact me if you need any further information.";
                    strContent += "<p>";
                    strContent += "We look forward to getting you covered as soon as possible to provide peace of mind to you and your beneficiaries.";
                    strContent += "<p>";
                    strContent += @"If you do not have a PDF reader installed on your computer you can download it here (<a href=""http://get.adobe.com/reader/"">http://get.adobe.com/reader/</a>).";
                    strContent += "<p>";
                    strContent += "Regards,";
                    strContent += "<p>";
                    strContent += strUserFullname;
                    strContent += "<p>";
                    strContent += "AllLife (Pty) Ltd.";
                    strContent += "<br />";
                    strContent += "Office: " + strUserTelephone;
                    strContent += "<br />";
                    strContent += "Fax: 0866171888";
                    strContent += "<br />";
                    strContent += @"<a href=""www.alllife.co.za"">www.alllife.co.za</a>";
                    strContent += "<p>";
                    strContent += "This email contains confidential information. It may also be legally privileged. Interception of this email is illegal. The information contained in this email is only for the use of the intended recipient. If you are not the intended recipient, any disclosure, copying and/or distribution of the contents of this email, or the taking of any action in reliance thereon, or pursuant thereto, is strictly prohibited. Should you have received this email in error, please notify us immediately by return email.";
                    strContent += "<p>";
                    strContent += "AllLife (Pty) Ltd shall not be liable if any variation is effected to any document or correspondence emailed unless that variation has been approved in writing by the individual dealing with the matter.";
                    #endregion

                    RadEditor1.Content = strContent;
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                //throw;
            }
        }

        private string GetSetting(string SettingName)
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
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", SettingName);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSettingValue = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                sqlConnectionX.Close();
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }

            return strSettingValue;
        }

        protected void RadBtnSendEmail_Click(object sender, EventArgs e)
        {
            string EmailUserName = string.Empty;
            string EmailPassword = string.Empty;
            string EmailSMTPServer = string.Empty;
            string EmailSMTPPort = string.Empty;
            string EmailEnableSsl = string.Empty;

            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            try
            {
                strFilePath = Request.QueryString["FilePath"].ToString();

                if (RadTxtEmailTo.Text.Trim() == "")
                {
                    lblInfo.Text = "Please enter an email address";
                }
                else
                {
                    //EmailUserName = GetSetting("EmailUserName");
                    //EmailPassword = GetSetting("EmailPassword");
                    //EmailUserName = "assa.support@astutesoft.co.za";
                    //EmailPassword = "AstuteSoft1";

                    EmailSMTPServer = GetSetting("EmailSMTPServer");
                    EmailSMTPPort = GetSetting("EmailSMTPPort");
                    EmailEnableSsl = GetSetting("EmailEnableSsl");

                    EmailUserName = HiddenFieldEmailUserName.Value.ToString();
                    EmailPassword = HiddenFieldEmailUserPassword.Value.ToString();
                    strUserFullname = HiddenFieldUserFullName.Value.ToString();

                    if (EmailUserName != string.Empty)
                    {
                        MailAddress fromAddress = new MailAddress(EmailUserName, strUserFullname);
                        NetworkCredential basicCredential = new NetworkCredential(EmailUserName, EmailPassword);
                        smtpClient.Host = EmailSMTPServer;
                        smtpClient.Port = Convert.ToInt16(EmailSMTPPort);
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.Credentials = new NetworkCredential(EmailUserName, EmailPassword);
                        //if (EmailUserName.Contains("gmail.com"))
                        if (EmailEnableSsl == "True")
                            smtpClient.EnableSsl = true;
                        else
                            smtpClient.EnableSsl = false;
                        smtpClient.Timeout = 1500000;
                        message.From = fromAddress;
                        message.To.Add(RadTxtEmailTo.Text.Trim());
                        message.Subject = RadTxtSubject.Text.Trim();
                        message.IsBodyHtml = true;

                        //message.Body = "Please find the attached report.\r\n\r\nThe report name is " + strReportName + " and the report was run from " + strStartDateValue.Substring(0, 10) + " to " + strEndDateValue.Substring(0, 10);
                        message.Body = RadEditor1.Content;

                        Attachment MailAttachement = new Attachment(strFilePath);
                        message.Attachments.Add(MailAttachement);

                        string PamphletPath = Server.MapPath("~/Content/");
                        string strPamphletName = GetSetting("PamphletLocation");
                        if (strPamphletName.Length > 0)
                        {
                            PamphletPath = System.IO.Path.Combine(PamphletPath, strPamphletName);
                            Attachment MailAttachementPamphlet = new Attachment(PamphletPath);
                            message.Attachments.Add(MailAttachementPamphlet);
                        }

                        // Send SMTP mail
                        lblInfo.Text = "Sending email...";
                        smtpClient.Send(message);
                        lblInfo.Text = "The email has been sent";
                        Image1.Visible = true;

                        #region "SQL"
                        SqlConnection sqlConnectionX;
                        SqlCommand sqlCommandX;
                        SqlParameter sqlParam;
                        //SqlDataReader sqlDR;

                        sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                        sqlConnectionX.Open();

                        sqlCommandX = new SqlCommand();
                        sqlCommandX.Connection = sqlConnectionX;
                        sqlCommandX.CommandType = CommandType.StoredProcedure;
                        sqlCommandX.CommandText = "spx_INSERT_LinkQuoteAuditIDEmail";

                        sqlParam = new SqlParameter("QuoteAuditTrailID", strQuoteAuditID);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("EmailAddress", RadTxtEmailTo.Text.Trim());
                        sqlCommandX.Parameters.Add(sqlParam);

                        sqlCommandX.ExecuteNonQuery();
                        sqlCommandX.Cancel();
                        sqlCommandX.Dispose();

                        #endregion

                        RadbtnClose.Visible = true;
                    }
                    else
                    {
                        OpenEmailDetailsWindow("There is currently no email account for you, please enter your details");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Client host rejected: Access denied"))
                {
                    RadWindowManager1.Windows.Clear();
                    RadWindow newWindow = new RadWindow();
                    newWindow.NavigateUrl = "QuoteSetEmailDetails.aspx?UserID=" + intUserID.ToString() + "&Msg=Access denied. Please re-enter your email details" + "&EmailUsername=" + HiddenFieldEmailUserName.Value.ToString();
                    newWindow.Modal = true;
                    newWindow.Height = 300;
                    newWindow.Width = 550;
                    //newWindow.VisibleStatusbar = false;
                    newWindow.DestroyOnClose = true;
                    newWindow.Title = "Enter email details";
                    newWindow.VisibleOnPageLoad = true;
                    newWindow.Behaviors = WindowBehaviors.Resize | WindowBehaviors.Close | WindowBehaviors.Move;
                    //newWindow.Behaviors = WindowBehaviors.Close;
                    //newWindow.Behaviors = WindowBehaviors.Move;
                    newWindow.VisibleStatusbar = false;
                    RadWindowManager1.Windows.Add(newWindow);
                }
                else
                {
                    lblInfo.Text = ex.Message;
                }
            }
        }

        private void OpenEmailDetailsWindow(string strMessage)
        {
            try
            {
                RadWindowManager1.Windows.Clear();
                RadWindow newWindow = new RadWindow();
                newWindow.NavigateUrl = "QuoteSetEmailDetails.aspx?UserID=" + intUserID.ToString() + "&Msg=" + strMessage + "&EmailUsername=" + HiddenFieldEmailUserName.Value.ToString();
                newWindow.Modal = true;
                newWindow.Height = 300;
                newWindow.Width = 550;
                //newWindow.VisibleStatusbar = false;
                newWindow.DestroyOnClose = true;
                newWindow.Title = "Enter email details";
                newWindow.VisibleOnPageLoad = true;
                newWindow.Behaviors = WindowBehaviors.Resize | WindowBehaviors.Close | WindowBehaviors.Move;
                //newWindow.Behaviors = WindowBehaviors.Close;
                //newWindow.Behaviors = WindowBehaviors.Move;
                newWindow.VisibleStatusbar = false;
                RadWindowManager1.Windows.Add(newWindow);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                // throw;
            }
        }

        protected void RadbtnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseReset('0');", true);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Close")
            {
                RadWindowManager1.Windows.Clear();

                // get the new email and password
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
                    sqlCommandX.CommandText = "spx_Select_UserByID";

                    sqlParam = new SqlParameter("UserID", intUserID);
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    while (sqlDR.Read())
                    {
                        if (!sqlDR.IsDBNull(4))
                        {
                            strUserFullname = sqlDR.GetValue(4).ToString();
                        }
                        if (!sqlDR.IsDBNull(5))
                        {
                            strUserEmailUsername = sqlDR.GetValue(5).ToString();
                            HiddenFieldEmailUserName.Value = strUserEmailUsername;
                        }
                        if (!sqlDR.IsDBNull(6))
                        {
                            strUserEmailPassword = sqlDR.GetValue(6).ToString();
                            string decryptedstring = Decrypt(strUserEmailPassword, mySalt);

                            HiddenFieldEmailUserPassword.Value = decryptedstring;
                        }
                    }

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();
                    sqlConnectionX.Close();
                    sqlConnectionX.Dispose();

                    // try send the email again
                    RadBtnSendEmail_Click(sender, e);
                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
    }
}