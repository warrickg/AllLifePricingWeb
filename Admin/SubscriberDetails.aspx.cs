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
using System.Security.Cryptography;
using System.Text;
using Telerik.Web.UI;

namespace AllLifePricingWeb.Admin
{
    public partial class SubscriberDetails : System.Web.UI.Page
    {
        SqlConnection sqlConnectionX;
        SqlCommand sqlCommandX;
        SqlParameter sqlParam;
        SqlDataReader sqlDR;
        string strMenuIDs = string.Empty;

        Int32 SubscriberID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            SubscriberID = Convert.ToInt32(Request.QueryString["SID"]);
            //lblInfo.Text = SubscriberID.ToString();

            if (!IsPostBack)
            {
                if (SubscriberID > 0)
                {                    
                    Get_SubscriberDetails(SubscriberID);
                    lblPassword.Visible = false;
                    RadTextBoxPassword.Visible = false;
                }
                else
                {
                    RadButtonChangePwd.Visible = false;
                }
                
            }
        }       

        private void Get_SubscriberDetails(Int32 SubscriberID)
        {
            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Select_SubscriberByID";
                sqlParam = new SqlParameter("SubscriberID", SubscriberID);
                sqlCommandX.Parameters.Add(sqlParam);

                SqlDataReader dr = sqlCommandX.ExecuteReader();

                while (dr.Read())
                {
                    RadTextBoxSubscriberName.Text = dr.GetString(0);
                    //string strPasswordEncrypted = dr.GetString(1);
                    RadTextBoxSubscriberCode.Text = dr.GetInt32(2).ToString();
                    string strSubscriberStatus = dr.GetString(3);
                    if (strSubscriberStatus == "1")
                    {
                        CheckBoxStatus.Checked = true;
                    }

                    string strreturnRisk = dr.GetString(4);
                    if (strreturnRisk == "T")
                    {
                        CheckBoxRisk.Checked = true;
                    }

                    string strreturnPremium = dr.GetString(5);
                    if (strreturnPremium == "T")
                    {
                        CheckBoxPremium.Checked = true;
                    }

                    string strreturnCover = dr.GetString(6);
                    if (strreturnCover == "T")
                    {
                        CheckBoxCover.Checked = true;
                    }

                }
                sqlConnectionX.Close();
            }
                catch(Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
            finally
            {
                
            }
        }

        protected void RadButtonClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseAndRebind();", true);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            switch (e.Argument)
            {
                case "CloseNoSave":
                    RadWindowManager1.Windows.Clear();
                    break;
                case "Rebind":
                    RadWindowManager1.Windows.Clear();
                    if (Session["RCUserTocopy"] != null)
                    {
                        Int32 intUserToCopy = Convert.ToInt32(Session["RCUserTocopy"]);
                        Session.Remove("SearchClickedRCUserSearch");
                        Session.Remove("RCUserTocopy");
                    }
                    break;
                default:
                    break;
            } 
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            //RadWindow newWindow = new RadWindow();
            //newWindow.NavigateUrl = "UserDetails_ChangePwd.aspx?SubscriberID=0";
            //newWindow.Modal = true;
            //newWindow.Height = 300;
            //newWindow.Width = 450;
            ////newWindow.VisibleStatusbar = false;
            //newWindow.DestroyOnClose = true;
            //newWindow.Title = "Change password";
            //newWindow.VisibleOnPageLoad = true;
            //newWindow.Behaviors = WindowBehaviors.Resize;
            //newWindow.Behaviors = WindowBehaviors.Close;
            //RadWindowManager1.Windows.Add(newWindow);

            panl_Changepassword.Visible = true;
        }

        protected void RadButtonSave_Click(object sender, EventArgs e)
        {
            //Save all the user details
            if (SubscriberID > 0)
            {
                #region "Update the user"
                try
                {                    
                    string strPasswordHashed = string.Empty;
                    string strMenuIDs = string.Empty;
                    string strResult = string.Empty;

                    PricingSubscriber _User = new PricingSubscriber();

                    if (panl_Changepassword.Visible == true)
                    {
                        _User = SubscriberPassswordCheck(RadTextBoxSubscriberName.Text.Trim(), RadTextBoxCurrentPassword.Text.Trim());

                        if (_User.Result == "Success")
                        {
                            if (RadTextBoxNewPassword.Text.Trim() == RadTextBoxConfirmPassword.Text.Trim())
                            {
                                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                                sqlConnectionX.Open();

                                strPasswordHashed = ComputeHash(RadTextBoxNewPassword.Text.Trim(), "SHA512", null);

                                #region "Values"
                                string strSubscriberStatus = string.Empty;
                                string strRisk = string.Empty;
                                string strPremium = string.Empty;
                                string strCover = string.Empty;

                                if (CheckBoxStatus.Checked == true)
                                {
                                    strSubscriberStatus = "1";
                                }
                                else
                                { 
                                    strSubscriberStatus = "0"; 
                                }

                                if (CheckBoxRisk.Checked == true)
                                {
                                    strRisk = "T";
                                }
                                else
                                {
                                    strRisk = "F";
                                }

                                if (CheckBoxPremium.Checked == true)
                                {
                                    strPremium = "T";
                                }
                                else
                                {
                                    strPremium = "F";
                                }

                                if (CheckBoxCover.Checked == true)
                                {
                                    strCover = "T";
                                }
                                else
                                {
                                    strCover = "F";
                                }
                                #endregion

                                sqlCommandX = new SqlCommand();
                                sqlCommandX.Connection = sqlConnectionX;
                                sqlCommandX.CommandType = CommandType.StoredProcedure;
                                sqlCommandX.CommandText = "spx_UPDATE_Subscriber";
                                sqlParam = new SqlParameter("SubscriberID", SubscriberID);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("Subscribername", RadTextBoxSubscriberName.Text.Trim());
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("Password", strPasswordHashed);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("SubscriberCode", RadTextBoxSubscriberCode.Text.Trim());
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("SubscriberStatus", strSubscriberStatus);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("returnRisk", strRisk);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("returnPremium", strPremium);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("returnCover", strCover);
                                sqlCommandX.Parameters.Add(sqlParam);

                                sqlCommandX.ExecuteNonQuery();

                                //Close the window
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseAndRebind();", true);
                            }
                            else
                            {
                                lblInfo.Text = "The new password does not match the confirmation password";
                            }
                        }
                        else
                        {
                            lblInfo.Text = "The current password you entered is not correct";
                        }
                    }
                    else
                    {
                        //Update the userwithout changing the password
                        sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                        sqlConnectionX.Open();                        

                        #region "Values"
                        string strSubscriberStatus = string.Empty;
                        string strRisk = string.Empty;
                        string strPremium = string.Empty;
                        string strCover = string.Empty;

                        if (CheckBoxStatus.Checked == true)
                        {
                            strSubscriberStatus = "1";
                        }
                        else
                        {
                            strSubscriberStatus = "0";
                        }

                        if (CheckBoxRisk.Checked == true)
                        {
                            strRisk = "T";
                        }
                        else
                        {
                            strRisk = "F";
                        }

                        if (CheckBoxPremium.Checked == true)
                        {
                            strPremium = "T";
                        }
                        else
                        {
                            strPremium = "F";
                        }

                        if (CheckBoxCover.Checked == true)
                        {
                            strCover = "T";
                        }
                        else
                        {
                            strCover = "F";
                        }
                        #endregion

                        sqlCommandX = new SqlCommand();
                        sqlCommandX.Connection = sqlConnectionX;
                        sqlCommandX.CommandType = CommandType.StoredProcedure;
                        sqlCommandX.CommandText = "spx_UPDATE_Subscriber";
                        sqlParam = new SqlParameter("SubscriberID", SubscriberID);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("Subscribername", RadTextBoxSubscriberName.Text.Trim());
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("Password", "");
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("SubscriberCode", RadTextBoxSubscriberCode.Text.Trim());
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("SubscriberStatus", strSubscriberStatus);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("returnRisk", strRisk);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("returnPremium", strPremium);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("returnCover", strCover);
                        sqlCommandX.Parameters.Add(sqlParam);

                        sqlCommandX.ExecuteNonQuery();

                        //Close the window
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    }

                   
                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                }
                finally
                {
                    sqlConnectionX.Close();
                }
                #endregion
            }
            else
            {
                #region "create the new Subscriber"
                try
                {
                    sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                    sqlConnectionX.Open();

                    string strPasswordHashed = string.Empty;                    
                    string strResult = string.Empty;

                    strPasswordHashed = ComputeHash(RadTextBoxNewPassword.Text.Trim(), "SHA512", null);

                    #region "Values"
                    string strSubscriberStatus = string.Empty;
                    string strRisk = string.Empty;
                    string strPremium = string.Empty;
                    string strCover = string.Empty;

                    if (CheckBoxStatus.Checked == true)
                    {
                        strSubscriberStatus = "1";
                    }
                    else
                    {
                        strSubscriberStatus = "0";
                    }

                    if (CheckBoxRisk.Checked == true)
                    {
                        strRisk = "T";
                    }
                    else
                    {
                        strRisk = "F";
                    }

                    if (CheckBoxPremium.Checked == true)
                    {
                        strPremium = "T";
                    }
                    else
                    {
                        strPremium = "F";
                    }

                    if (CheckBoxCover.Checked == true)
                    {
                        strCover = "T";
                    }
                    else
                    {
                        strCover = "F";
                    }
                    #endregion

                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_UPDATE_Subscriber";
                    sqlParam = new SqlParameter("SubscriberID", SubscriberID);
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("Subscribername", RadTextBoxSubscriberName.Text.Trim());
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("Password", strPasswordHashed);
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("SubscriberCode", RadTextBoxSubscriberCode.Text.Trim());
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("SubscriberStatus", strSubscriberStatus);
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("returnRisk", strRisk);
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("returnPremium", strPremium);
                    sqlCommandX.Parameters.Add(sqlParam);
                    sqlParam = new SqlParameter("returnCover", strCover);
                    sqlCommandX.Parameters.Add(sqlParam);

                    SqlDataReader dr = sqlCommandX.ExecuteReader();

                    while (dr.Read())
                    {
                        strResult = dr.GetString(0);                        
                    }    
               
                    if (strResult == "Ok")
                    { 
                        //Close the window
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    }
                    else
                    {
                        lblInfo.Text = strResult;
                    }

                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                }
                finally
                {
                    sqlConnectionX.Close();
                }
                #endregion
            }
            
        }

        private static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
            saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        private static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {
            //plainText is the value the user will enter for the password
            //hashValue is the encrypted password

            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

        private PricingSubscriber SubscriberPassswordCheck(string SubscriberName, string password)
        {
            PricingSubscriber DBUser = new PricingSubscriber();

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Pricing_SubscriberAuth";

                sqlParam = new SqlParameter("SubscriberName", SubscriberName);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlDR = sqlCommandX.ExecuteReader();

                while (sqlDR.Read())
                {
                    DBUser.SubscriberID = sqlDR.GetInt32(0);
                    DBUser.Subscribername = sqlDR.GetString(1);
                    DBUser.Password = sqlDR.GetString(2);
                }
                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                //Check the password is correct
                bool flag = VerifyHash(password, "SHA512", DBUser.Password);
                if (flag != true)
                {                   
                    if (DBUser.Result != null)
                    {
                        DBUser.Result += "incorrect";
                    }
                    else
                    {
                        DBUser.Result = "incorrect";
                    }
                }
                else
                {
                    DBUser.Result = "Success";
                    DBUser.Password = "";
                }


            }
            catch (Exception)
            {
                //mySubscriber.ResultMessage = ex.Message;
            }
            finally
            {
                sqlDR.Close();
                sqlDR.Dispose();
                sqlConnectionX.Close();
            }

            return DBUser;
        }
    }

    
     public class PricingSubscriber
    {
        private int subscriberID;
        public int SubscriberID
        {
            get { return subscriberID; }
            set { subscriberID = value; }
        }

        private string subscribername;
        public string Subscribername
        {
            get { return subscribername; }
            set { subscribername = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string result;
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
    }
     
}