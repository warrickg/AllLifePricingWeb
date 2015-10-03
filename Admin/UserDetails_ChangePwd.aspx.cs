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
using System.IO;

namespace AllLifePricingWeb.Admin
{
    public partial class UserDetails_ChangePwd : System.Web.UI.Page
    {
        SqlConnection sqlConnectionX;
        SqlCommand sqlCommandX;
        SqlParameter sqlParam;
        SqlDataReader sqlDR;
        string strMenuIDs = string.Empty;
        string strBenifits = string.Empty;
        string mySalt = "AllLife";

        Int32 UserID = 0;

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        protected void Page_Load(object sender, EventArgs e)
        {
            //UserID = Convert.ToInt32(Request.QueryString["UserID"]);
            UserID = Convert.ToInt32(Session["UserID"]);

            if (UserID == null)
                UserID = 0;
            //lblInfo.Text = UserID.ToString();

            if (!IsPostBack)
            {
                if (UserID > 0)
                {
                    //get user deatils
                    Get_UserDetails(UserID);
                    lblPassword.Visible = false;
                    RadTextBoxPassword.Visible = false;
                }
                else
                {
                    RadButtonChangePwd.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseSessionExpired('9');", true);
                }
            }
        }

        private void Get_UserDetails(Int32 UserID)
        {
            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Select_UserByID";
                sqlParam = new SqlParameter("UserID", UserID);
                sqlCommandX.Parameters.Add(sqlParam);

                SqlDataReader dr = sqlCommandX.ExecuteReader();
                //DataTable dt = new DataTable("Users");
                //dt.Load(dr);

                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        HiddenFieldUserID.Value = dr.GetInt32(0).ToString();
                    if (!dr.IsDBNull(1))
                        RadTextBoxUsername.Text = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        HiddenFieldPwd.Value = dr.GetString(2);
                    if (!dr.IsDBNull(3))
                        strMenuIDs = dr.GetString(3);
                    if (!dr.IsDBNull(4))
                        RadTextBoxUserFullname.Text = dr.GetString(4);
                    if (!dr.IsDBNull(5))
                        RadTxtEmailUsername.Text = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                    {
                        RadTxtEmailPassword.Text = dr.GetString(6);
                        HiddenFieldEmailPwd.Value = dr.GetString(6);
                    }
                    if (!dr.IsDBNull(7))
                        RadTxtTelephone.Text = dr.GetString(7);
                    //if (!dr.IsDBNull(8))
                    //    strBenifits = dr.GetString(8);
                }              

                sqlConnectionX.Close();
            }
            catch (Exception ex)
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
            panl_Changepassword.Visible = true;
        }

        protected void RadButtonSave_Click(object sender, EventArgs e)
        {
            //Save all the user details
            if (UserID > 0)
            {
                #region "Update the user"
                try
                {
                    string strPasswordHashed = string.Empty;
                    string strMenuIDs = string.Empty;
                    string strResult = string.Empty;
                    string strUserFullName = string.Empty;
                    string strEmailUsername = string.Empty;
                    string strEmailPassword = string.Empty;
                    string strTelephone = string.Empty;
                    string strBenifits = string.Empty;

                    PricingUser _User = new PricingUser();

                    if (panl_Changepassword.Visible == true)
                    {
                        _User = UserPassswordCheck(RadTextBoxUsername.Text.Trim(), RadTextBoxCurrentPassword.Text.Trim());

                        if (_User.Result == "Success")
                        {
                            if (RadTextBoxNewPassword.Text.Trim() == RadTextBoxConfirmPassword.Text.Trim())
                            {
                                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                                sqlConnectionX.Open();

                                strPasswordHashed = ComputeHash(RadTextBoxNewPassword.Text.Trim(), "SHA512", null);
                                strUserFullName = RadTextBoxUserFullname.Text.Trim();
                                strEmailUsername = RadTxtEmailUsername.Text.Trim();
                                //strEmailPassword = RadTxtEmailPassword.Text.Trim();
                                strEmailPassword = Encrypt(RadTxtEmailPassword.Text.Trim(), mySalt);
                                strTelephone = RadTxtTelephone.Text.Trim();                                

                                if (strEmailPassword == "")
                                    strEmailPassword = HiddenFieldEmailPwd.Value.ToString();

                                sqlCommandX = new SqlCommand();
                                sqlCommandX.Connection = sqlConnectionX;
                                sqlCommandX.CommandType = CommandType.StoredProcedure;
                                sqlCommandX.CommandText = "spx_UPDATE_UserOwnDetails";
                                sqlParam = new SqlParameter("UserID", UserID);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("Username", RadTextBoxUsername.Text.Trim());
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("Password", strPasswordHashed);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("UserFullName", strUserFullName);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("EmailUsername", strEmailUsername);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("EmailPassword", strEmailPassword);
                                sqlCommandX.Parameters.Add(sqlParam);
                                sqlParam = new SqlParameter("Telephone", strTelephone);
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

                        strPasswordHashed = ComputeHash(RadTextBoxNewPassword.Text.Trim(), "SHA512", null);
                        strUserFullName = RadTextBoxUserFullname.Text.Trim();
                        strEmailUsername = RadTxtEmailUsername.Text.Trim();
                        //strEmailPassword = RadTxtEmailPassword.Text.Trim();
                        strEmailPassword = Encrypt(RadTxtEmailPassword.Text.Trim(), mySalt);

                        //string decryptedstring = Decrypt(encryptedstring, mySalt);

                        strTelephone = RadTxtTelephone.Text.Trim();
                       
                        if (strEmailPassword == "")
                            strEmailPassword = HiddenFieldEmailPwd.Value.ToString();

                        sqlCommandX = new SqlCommand();
                        sqlCommandX.Connection = sqlConnectionX;
                        sqlCommandX.CommandType = CommandType.StoredProcedure;
                        sqlCommandX.CommandText = "spx_UPDATE_UserOwnDetails";
                        sqlParam = new SqlParameter("UserID", UserID);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("Username", RadTextBoxUsername.Text.Trim());
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("Password", "");
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("UserFullName", strUserFullName);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("EmailUsername", strEmailUsername);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("EmailPassword", strEmailPassword);
                        sqlCommandX.Parameters.Add(sqlParam);
                        sqlParam = new SqlParameter("Telephone", strTelephone);
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

        private PricingUser UserPassswordCheck(string Username, string password)
        {
            PricingUser DBUser = new PricingUser();

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Pricing_UserAuth";

                sqlParam = new SqlParameter("UserName", Username);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlDR = sqlCommandX.ExecuteReader();

                while (sqlDR.Read())
                {
                    DBUser.UserID = sqlDR.GetInt32(0);
                    DBUser.Username = sqlDR.GetString(1);
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

        public static string Encrypt(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
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