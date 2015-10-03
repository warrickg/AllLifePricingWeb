using AllLifePricingWeb.WS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AllLifePricingWeb.Classes;

namespace AllLifePricingWeb
{
    public partial class Login1 : System.Web.UI.Page
    {
        SqlConnection sqlConnectionX;
        SqlCommand sqlCommandX;
        SqlParameter sqlParam;
        SqlDataReader sqlDR;

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(Request.QueryString["ID"]))
            {
                case 1:
                    Session.Remove("UserID");
                    Session.Remove("UserMenu");
                    break;
                case 9:
                    lblInfo.Text = "Your session has expired. Please log in again";
                    break;
                default:
                    RadTextBoxUsername.Focus();
                    break;
            }           
        }

        protected void RadButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {                
                PricingUser User = new PricingUser();
                User.Username = RadTextBoxUsername.Text;
                User.Password = RadTextBoxPassword.Text;

                User = Userlogin(User);

                if (User.Result == "Success")
                {
                    DataSet dsUserMenu = Get_UserMenu(User.UserID);

                    Session["UserID"] = User.UserID;
                    Session["UserMenu"] = dsUserMenu;
                    Response.Redirect("Quote.aspx",false);
                }
                else
                {
                    lblInfo.Text = User.Result;
                }
            }
            catch (Exception ex)
            {

                lblInfo.Text = ex.Message;
            }
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

        public PricingUser Userlogin(PricingUser User_)
        {           

            PricingUser DBUser = new PricingUser();
            bool blnAreThereErrors = false;
            bool blnHasRows = false;

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_Pricing_UserAuth";

                sqlParam = new SqlParameter("UserName", User_.Username);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlDR = sqlCommandX.ExecuteReader();

                while (sqlDR.Read())
                {
                    DBUser.UserID = sqlDR.GetInt32(0);
                    DBUser.Username = sqlDR.GetString(1);
                    DBUser.Password = sqlDR.GetString(2);
                }

                blnHasRows = sqlDR.HasRows;

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                if (blnHasRows)
                {
                    //Check the password is correct
                    bool flag = VerifyHash(User_.Password, "SHA512", DBUser.Password);
                    if (flag != true)
                    {
                        blnAreThereErrors = true;
                        if (DBUser.Result != null)
                        {
                            DBUser.Result += ", User password is incorrect";
                        }
                        else
                        {
                            DBUser.Result = "User password is incorrect";
                        }
                    }
                    else
                    {
                        DBUser.Result = "Success";
                        DBUser.Password = "";
                    }
                }
                else
                {
                    DBUser.Result = "User does not exist";
                    DBUser.Password = "";
                }

            }
            catch (Exception ex)
            {
                DBUser.Result = ex.Message;
                return DBUser;
            }
            //finally
            //{
            //    sqlDR.Close();
            //    sqlDR.Dispose();
            //    sqlConnectionX.Close();
            //}

            return DBUser;
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

    }
}