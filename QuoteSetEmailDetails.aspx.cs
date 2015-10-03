using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllLifePricingWeb
{
    public partial class QuoteSetEmailDetails : System.Web.UI.Page
    {
        //Int32 UserID = 0;
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
            if (!IsPostBack)
            {
                //UserID = Convert.ToInt32(Request.QueryString["UserID"]);
                lblMessage.Text = Request.QueryString["Msg"].ToString();
                RadTxtEmailUsername.Text = Request.QueryString["EmailUsername"].ToString();
            }
        }

        protected void RadBtnSave_Click(object sender, EventArgs e)
        {
            //save the email details for this user ID
            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlParameter sqlParam;

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_UPDATE_UserEmailDetailsByID";

                sqlParam = new SqlParameter("UserID", Session["UserID"].ToString());
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("EmailUsername", RadTxtEmailUsername.Text.Trim());
                sqlCommandX.Parameters.Add(sqlParam);
                string strEmailPassword = Encrypt(RadTxtEmailPassword.Text.Trim(), mySalt);     
                //sqlParam = new SqlParameter("EmailPassword", RadTxtEmailPassword.Text.Trim());
                sqlParam = new SqlParameter("EmailPassword", strEmailPassword);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlCommandX.ExecuteNonQuery();

                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                sqlConnectionX.Close();
                sqlConnectionX.Dispose();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "Close('0');", true);
            }
            catch (Exception ex)
            {

                lblinfo.Text = ex.Message;
            }
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