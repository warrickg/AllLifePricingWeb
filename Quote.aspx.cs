using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

namespace AllLifePricingWeb
{
    public partial class Quote : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RadDatePickerQuoteDate.SelectedDate = DateTime.Now.Date;
            lblInfo.Text = "";
            lblInfo2.Text = "";
            lblRequalify.Text = "";
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx?ID=9");
            }

            //HtmlMeta meta = new HtmlMeta();
            //meta.HttpEquiv = "Refresh";
            //meta.Content = "20";  //1200 seconds  = 20 minutes
            //this.Page.Header.Controls.Add(meta);

            //RadNotification1.ShowInterval = (Session.Timeout - 1) * 60000;
            //RadNotification1.Value = Page.ResolveClientUrl("SessionExpired.aspx");

            if (!Page.IsPostBack)
            {
                RadTxtMagnumID.Focus();
                PopulateOccupation();
                PopulateBenifitDropdowns();

                if (RadComboBoxTypeBenefitLife.Items.Count() > 1)
                {
                    RadioButtonHbA1c4.Visible = false;
                    RadioButtonHbA1c3.Visible = true;
                }
                else
                {
                    RadioButtonHbA1c4.Visible = true;
                    RadioButtonHbA1c3.Visible = false;
                }
            }            
        }
        
        private void PopulateOccupation()
        {
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
                sqlCommandX.CommandText = "spx_SELECT_Occupation";

                sqlDR = sqlCommandX.ExecuteReader();
                DataTable dtResult = new DataTable("Result");
                dtResult.Load(sqlDR);
                                
                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                RadComboBoxOccupation.DataTextField = "Occupation";
                RadComboBoxOccupation.DataValueField = "OccupationID";

                RadComboBoxItem cbDefaultItem = new RadComboBoxItem();
                cbDefaultItem.Value = "0";
                cbDefaultItem.Text = "- Please select an occupation -";
                RadComboBoxOccupation.Items.Add(cbDefaultItem);

                foreach (DataRow dataRow in dtResult.Rows)
                {                    
                    RadComboBoxItem cbItem = new RadComboBoxItem();
                    cbItem.Value = dataRow[0].ToString().Trim();
                    cbItem.Text = dataRow[1].ToString().Trim();
                    cbItem.Attributes.Add("Life", dataRow[2].ToString());
                    cbItem.Attributes.Add("ADW", dataRow[3].ToString());
                    cbItem.Attributes.Add("OCC", dataRow[4].ToString());
                    RadComboBoxOccupation.Items.Add(cbItem);
                }

                //RadComboBoxOccupation.DataSource = dtResult;
                //RadComboBoxOccupation.DataBind();

            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;   
            }
        }

        private void PopulateBenifitDropdowns()
        {
            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlParameter sqlParam;
            SqlDataReader sqlDR;
            bool blnEmLoadingEnabled = false;

            try
            {
                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_BenifitByUser";

                sqlParam = new SqlParameter("UserID", Convert.ToInt32(Session["UserID"]));
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                DataTable dtResult = new DataTable("Result");
                dtResult.Load(sqlDR);

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                RadComboBoxTypeBenefitLife.DataTextField = "Value";
                RadComboBoxTypeBenefitDisability.DataTextField = "Value";

                //RadComboBoxItem cbDefaultItem = new RadComboBoxItem();
                //cbDefaultItem.Value = "0";
                //cbDefaultItem.Text = "- None -";
                //RadComboBoxOccupation.Items.Add(cbDefaultItem);

                foreach (DataRow dataRow in dtResult.Rows)
                {
                    RadComboBoxItem cbItem = new RadComboBoxItem();
                    cbItem.Text = dataRow[0].ToString().Trim();                    
                    RadComboBoxTypeBenefitLife.Items.Add(cbItem);
                }

                foreach (DataRow dataRow in dtResult.Rows)
                {
                    RadComboBoxItem cbItem = new RadComboBoxItem();
                    cbItem.Text = dataRow[0].ToString().Trim();
                    RadComboBoxTypeBenefitDisability.Items.Add(cbItem);
                }

                //EM Loading

                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_EMLoadingByUser";

                sqlParam = new SqlParameter("UserID", Convert.ToInt32(Session["UserID"]));
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    blnEmLoadingEnabled = Convert.ToBoolean(sqlDR.GetValue(0));
                }

                if (blnEmLoadingEnabled == true)
                {
                    RadNumericTxtEMLoadingLife.Enabled = true;
                    RadNumericTxtEMLoadingDisability.Enabled = true;
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                sqlConnectionX.Close();
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;   
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
                lblInfo2.Text = ex.Message;
                //throw;
            }

                return strSettingValue;
        }

        protected void RadBtnQualifyClient_Click(object sender, EventArgs e)
        {
            WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
            DataTable ReturnDT;

            string strSubscriberName = string.Empty;
            string strSubscriberPassword = string.Empty;
            string strSubscriberCode = string.Empty;
            
            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlParameter sqlParam;
            SqlDataReader sqlDR;

            try
            {
                hideOption1Life();
                hideOption1Disability();
                hideOption2Life();
                hideOption2Disability();
                hideOption3Life();
                hideOption3Disability();
                hideOption4Life();
                hideOption4Disability();
                hideOption5Life();
                hideOption5Disability();

                lblRequalify.Text = "";

                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                #region "Get SubscriberName"
                strSubscriberName = GetSetting("SubscriberName");

                //sqlCommandX = new SqlCommand();
                //sqlCommandX.Connection = sqlConnectionX;
                //sqlCommandX.CommandType = CommandType.StoredProcedure;
                //sqlCommandX.CommandText = "spx_SELECT_Setting";

                //sqlParam = new SqlParameter("SettingName", "SubscriberName");
                //sqlCommandX.Parameters.Add(sqlParam);

                //sqlDR = sqlCommandX.ExecuteReader();
                //while (sqlDR.Read())
                //{
                //    strSubscriberName = sqlDR.GetValue(0).ToString();
                //}

                //sqlDR.Close();
                //sqlCommandX.Cancel();
                //sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberPassword"
                strSubscriberPassword = GetSetting("SubscriberPassword");
                //sqlCommandX = new SqlCommand();
                //sqlCommandX.Connection = sqlConnectionX;
                //sqlCommandX.CommandType = CommandType.StoredProcedure;
                //sqlCommandX.CommandText = "spx_SELECT_Setting";

                //sqlParam = new SqlParameter("SettingName", "SubscriberPassword");
                //sqlCommandX.Parameters.Add(sqlParam);

                //sqlDR = sqlCommandX.ExecuteReader();
                //while (sqlDR.Read())
                //{
                //    strSubscriberPassword = sqlDR.GetValue(0).ToString();
                //}

                //sqlDR.Close();
                //sqlCommandX.Cancel();
                //sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberCode"
                strSubscriberCode = GetSetting("SubscriberCode");
                //sqlCommandX = new SqlCommand();
                //sqlCommandX.Connection = sqlConnectionX;
                //sqlCommandX.CommandType = CommandType.StoredProcedure;
                //sqlCommandX.CommandText = "spx_SELECT_Setting";

                //sqlParam = new SqlParameter("SettingName", "SubscriberCode");
                //sqlCommandX.Parameters.Add(sqlParam);

                //sqlDR = sqlCommandX.ExecuteReader();
                //while (sqlDR.Read())
                //{
                //    strSubscriberCode = sqlDR.GetValue(0).ToString();
                //}

                //sqlDR.Close();
                //sqlCommandX.Cancel();
                //sqlCommandX.Dispose();
                #endregion

                bool blnTobaccoUse;
                int intHbA1c = 0;
                int intAlcoholUnitsPerDay = 0;
                string strEmployment = string.Empty; string strQualification = string.Empty; string strSpouseQualification = string.Empty;
                string strResultLife = string.Empty;
                string strResultDisability = string.Empty;
                int intClass = 0;                
                int intMarriedTypeID = 0;

                string strTobaccoSelection = RadioButtonListTobacco.SelectedValue;

                if (strTobaccoSelection == "non-smoker")
                    blnTobaccoUse = false;
                else
                    blnTobaccoUse = true;

                //if (RadioButtonListTobacco.Checked == true)
                //    blnTobaccoUse = true;
                //else
                //    blnTobaccoUse = false;

                string strAcoholSelection = RadioButtonListAlcohol.SelectedValue;

                if (strAcoholSelection == "non-drinker")
                    intAlcoholUnitsPerDay = 0;
                if (strAcoholSelection == "0 - 5")
                    intAlcoholUnitsPerDay = 5;
                if (strAcoholSelection == "> 5")
                    intAlcoholUnitsPerDay = 6;

                #region "HbA1c"
                if (RadioButtonHbA1c3.Checked == true)
                    intHbA1c = 4;
                if (RadioButtonHbA1c4.Checked == true)
                    intHbA1c = 4;
                if (RadioButtonHbA1c6.Checked == true)
                    intHbA1c = 6;
                if (RadioButtonHbA1c7.Checked == true)
                    intHbA1c = 7;
                if (RadioButtonHbA1c8.Checked == true)
                    intHbA1c = 8;
                if (RadioButtonHbA1c9.Checked == true)
                    intHbA1c = 9;
                if (RadioButtonHbA1c10.Checked == true)
                    intHbA1c = 10;
                if (RadioButtonHbA1c11.Checked == true)
                    intHbA1c = 11;
                if (RadioButtonHbA1c12.Checked == true)
                    intHbA1c = 12;
                if (RadioButtonHbA1c15.Checked == true)
                    intHbA1c = 15;
                #endregion

                #region "Married"
                if (RadioButtonMaritalStatusNotMarried.Checked == true)
                {
                    intMarriedTypeID = 0;
                }

                if (RadioButtonMaritalStatusMarried.Checked == true)
                {
                    intMarriedTypeID = 1;
                }
               
                #endregion

                if (RadioButtonEsc6.Checked == true)
                    HiddenFieldEscalationLife.Value = "6.00";
                if (RadioButtonEsc10.Checked == true)
                    HiddenFieldEscalationLife.Value = "10.00";

                //if (RadioButtonEscLife6.Checked == true)
                //    HiddenFieldEscalationLife.Value = "6.00";
                //if (RadioButtonEscLife10.Checked == true)
                //    HiddenFieldEscalationLife.Value = "10.00";

                //if (RadioButtonEscalationDis6.Checked == true)
                //    HiddenFieldEscalationDisablility.Value = "6.00";
                //if (RadioButtonEscalationDis10.Checked == true)
                //    HiddenFieldEscalationDisablility.Value = "10.00";

                strEmployment = RadComboBoxOccupation.SelectedItem.Text.ToString().Trim();

                #region "QualifyLife"
                ReturnDT = WS.QualifyLife(strSubscriberName, strSubscriberPassword, strSubscriberCode, RadTxtAgeOfNextBirthday.Text.Trim(), blnTobaccoUse.ToString(), intHbA1c.ToString(), RadNumericTxtBMI.Text.ToString().Trim(), RadNumericTxtPantSize.Text.Trim(), intAlcoholUnitsPerDay.ToString(), strEmployment);
                foreach (DataRow row in ReturnDT.Rows)
                {
                    strResultLife = row["Result"].ToString();
                }

                if (strResultLife == "Successful")
                {
                    LblQualificationMessage.Text = "ok";
                    PanelLife.GroupingText = "Life";
                    //PanelLife.Enabled = true;
                    RadioButtonEscLife6.Enabled = true;
                    RadioButtonEscLife10.Enabled = true;
                    RadNumericTxtCoverLife.Enabled = true;
                    RadNumericTxtPremiumLife.Enabled = true;
                    RadNumericTxtEMLoadingLife.Enabled = true;

                    RadioButtonQuoteLifeYes.Checked = true;
                    //RadioButtonEscLife6.Checked = true;
                    RadioButtonEsc6.Checked = true;
                }
                else
                {
                        LblQualificationMessage.Text = strResultLife;
                        PanelLife.GroupingText = "Life - Unavailable";
                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                        {
                            //PanelLife.Enabled = false;
                            RadioButtonEscLife6.Enabled = false;
                            RadioButtonEscLife10.Enabled = false;
                            RadNumericTxtCoverLife.Enabled = false;
                            RadNumericTxtPremiumLife.Enabled = false;
                            RadNumericTxtEMLoadingLife.Enabled = false;
                            hideOption1Life();
                        }
                        else
                        {
                            RadioButtonEscLife6.Enabled = true;
                            RadioButtonEscLife10.Enabled = true;
                            RadNumericTxtCoverLife.Enabled = true;
                            RadNumericTxtPremiumLife.Enabled = true;
                            RadNumericTxtEMLoadingLife.Enabled = true;

                            //RadioButtonQuoteLifeYes.Checked = true;
                            //RadioButtonEscLife6.Checked = true;
                            //RadioButtonEsc6.Checked = true;
                        }
                }
                #endregion

                //Does the client qualify for the disability
                //**********************************************************                        
                #region "QualifyDisability"

                if (RadioButtonNotMat.Checked)
                    strQualification = "No matric";
                if (RadioButtonMatriculated.Checked)
                    strQualification = "Matric";
                if (RadioButtonDiploma.Checked)
                    strQualification = "3 or 4 yr. Diploma/3 yr. Degree";
                if (RadioButtonDegree.Checked)
                    strQualification = "4 yr. Degree/professional qualification";

                if (RadioButtonSNotMat.Checked)
                    strSpouseQualification = "No matric";
                if (RadioButtonSMat.Checked)
                    strSpouseQualification = "Matric";
                if (RadioButtonSDip.Checked)
                    strSpouseQualification = "3 or 4 yr. Diploma/3 yr. Degree";
                if (RadioButtonSDegree.Checked)
                    strSpouseQualification = "4 yr. Degree/professional qualification";

                strEmployment = RadComboBoxOccupation.SelectedItem.Text.ToString().Trim();
                string strSpouseIncome = RadNumericTxtSpouseIncome.Text.Trim();
                if (strSpouseIncome == "")
                    strSpouseIncome = "0";

                string strProduct = string.Empty;

                #region "Get Default Diabetes Product"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "Default Diabetes Product");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strProduct = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                //Get the risk band
                string strRiskModifier = GetRiskModifier();

                ReturnDT = WS.returnRiskBand(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + "WL06", strRiskModifier, RadDatePickerQuoteDate.SelectedDate.ToString());

                string strRiskBand = ReturnDT.Rows[0][0].ToString();
                RadTxtRiskBand.Text = strRiskBand;

                ReturnDT = WS.QualifyDisability(strSubscriberName, strSubscriberPassword, strSubscriberCode, RadTxtAgeOfNextBirthday.Text.Trim(), blnTobaccoUse.ToString(), intHbA1c.ToString(), RadNumericTxtBMI.Text.ToString().Trim(), RadNumericTxtPantSize.Text.Trim(), intAlcoholUnitsPerDay.ToString(), strEmployment, strQualification, RadNumericTxtIncome.Text.Trim(), strSpouseIncome, strRiskBand);
                foreach (DataRow row in ReturnDT.Rows)
                {
                    strResultDisability = row["Result"].ToString();
                    intClass = Convert.ToInt16(row["Class"].ToString());                    
                }
                
                HiddenFieldClassOfLife.Value = intClass.ToString();

                if (strResultDisability == "Successful")
                {
                    lblDisabilityMessage.Text = "ok";
                    PanelDisability.GroupingText = "Disability";
                    //PanelDisability.Enabled = true;
                    RadioButtonTypeOfDisADW.Enabled = true;
                    RadioButtonTypeOfDisOCC.Enabled = true;
                    RadioButtonEscalationDis6.Enabled = true;
                    RadioButtonEscalationDis10.Enabled = true;
                    RadioButtonQuoteDisYes.Enabled = true;
                    RadioButtonQuoteDisNo.Enabled = true;
                    RadNumericTxtCoverAmnDis.Enabled = true;
                    RadNumericTxtPremiumDis.Enabled = true;
                    RadNumericTxtEMLoadingDisability.Enabled = true;

                    RadioButtonQuoteDisYes.Checked = true;
                    ////RadioButtonEscalationDis6.Checked = true;
                    ////if (intClass > 0)
                    ////{
                    ////    lblDisabilityMessage.Text += ", class:" + intClass.ToString();
                    ////}    


                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_Select_OccupationLimitsByOccupation";

                    sqlParam = new SqlParameter("Occupation", strEmployment);
                    sqlCommandX.Parameters.Add(sqlParam);

                    bool blnADW = false;
                    bool blnOCC = false;

                    sqlDR = sqlCommandX.ExecuteReader();
                    while (sqlDR.Read())
                    {
                        if (sqlDR.GetBoolean(1) == true)  //sql column 1 = ADW
                            blnADW = true;
                        if (sqlDR.GetBoolean(2) == true)  //sql column 2 = OCC
                            blnOCC = true;
                    }

                    sqlDR.Close();
                    sqlDR.Dispose();

                    if ((intClass >= 1) && (intClass < 4))
                    {
                        if (blnOCC == true)
                        {
                            RadioButtonTypeOfDisOCC.Enabled = true;
                            RadioButtonTypeOfDisOCC.Checked = true;
                            RadioButtonTypeOfDisADW.Enabled = true;
                            RadTxtDisabilityType.Text = "OCC";
                        }
                        else
                        {
                            if (blnADW == true)
                            {
                                RadioButtonTypeOfDisOCC.Enabled = false;
                                RadioButtonTypeOfDisOCC.Checked = false;
                                RadioButtonTypeOfDisADW.Enabled = true;
                                RadioButtonTypeOfDisADW.Checked = true;
                                RadTxtDisabilityType.Text = "ADW";
                            }
                            else
                            {
                                strResultDisability = "Occupation does not allow disability";
                                lblDisabilityMessage.Text = strResultDisability;
                                PanelDisability.GroupingText = "Disability - Unavailable";
                                //PanelDisability.Enabled = false;
                                RadioButtonTypeOfDisADW.Enabled = false;
                                RadioButtonTypeOfDisOCC.Enabled = false;
                                RadioButtonEscalationDis6.Enabled = false;
                                RadioButtonEscalationDis10.Enabled = false;
                                RadioButtonQuoteDisYes.Enabled = false;
                                RadioButtonQuoteDisNo.Enabled = false;
                                RadNumericTxtCoverAmnDis.Enabled = false;
                                RadNumericTxtPremiumDis.Enabled = false;
                                RadNumericTxtEMLoadingDisability.Enabled = false;

                                hideOption1Disability();
                            }
                        }
                    }


                    if (intClass > 3)
                    {                        
                        if (blnADW == true)
                        {
                            RadioButtonTypeOfDisOCC.Enabled = false;
                            RadioButtonTypeOfDisOCC.Checked = false;
                            RadioButtonTypeOfDisADW.Enabled = true;
                            RadioButtonTypeOfDisADW.Checked = true;
                            RadTxtDisabilityType.Text = "ADW";
                        }
                        else
                        {
                            strResultDisability = "Occupation does not allow disability";
                            lblDisabilityMessage.Text = strResultDisability;
                            PanelDisability.GroupingText = "Disability - Unavailable";
                            //PanelDisability.Enabled = false;
                            RadioButtonTypeOfDisADW.Enabled = false;
                            RadioButtonTypeOfDisOCC.Enabled = false;
                            RadioButtonEscalationDis6.Enabled = false;
                            RadioButtonEscalationDis10.Enabled = false;
                            RadioButtonQuoteDisYes.Enabled = false;
                            RadioButtonQuoteDisNo.Enabled = false;
                            RadNumericTxtCoverAmnDis.Enabled = false;
                            RadNumericTxtPremiumDis.Enabled = false;
                            RadNumericTxtEMLoadingDisability.Enabled = false;
                            hideOption1Disability();
                        }
                    }

                   
                }
                else
                {
                    if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                    {
                        lblDisabilityMessage.Text = strResultDisability;
                        PanelDisability.GroupingText = "Disability - Unavailable";

                        if ((RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB") && (RadComboBoxTypeBenefitLife.Items.Count() == 1))
                        {
                            //PanelDisability.Enabled = false;
                            RadioButtonTypeOfDisADW.Enabled = false;
                            RadioButtonTypeOfDisOCC.Enabled = false;
                            RadioButtonEscalationDis6.Enabled = false;
                            RadioButtonEscalationDis10.Enabled = false;
                            RadioButtonQuoteDisYes.Enabled = false;
                            RadioButtonQuoteDisNo.Enabled = false;
                            RadNumericTxtCoverAmnDis.Enabled = false;
                            RadNumericTxtPremiumDis.Enabled = false;
                            RadNumericTxtEMLoadingDisability.Enabled = false;

                            hideOption1Disability();
                        }
                        else
                        {
                            RadioButtonTypeOfDisADW.Enabled = true;
                            RadioButtonTypeOfDisOCC.Enabled = true;
                            RadioButtonEscalationDis6.Enabled = true;
                            RadioButtonEscalationDis10.Enabled = true;
                            RadioButtonQuoteDisYes.Enabled = true;
                            RadioButtonQuoteDisNo.Enabled = true;
                            RadNumericTxtCoverAmnDis.Enabled = true;
                            RadNumericTxtPremiumDis.Enabled = true;
                            RadNumericTxtEMLoadingDisability.Enabled = true;
                        }
                    }
                    else
                    {
                        RadioButtonTypeOfDisADW.Enabled = true;
                        RadioButtonTypeOfDisOCC.Enabled = true;
                        RadioButtonEscalationDis6.Enabled = true;
                        RadioButtonEscalationDis10.Enabled = true;
                        RadioButtonQuoteDisYes.Enabled = true;
                        RadioButtonQuoteDisNo.Enabled = true;
                        RadNumericTxtCoverAmnDis.Enabled = true;
                        RadNumericTxtPremiumDis.Enabled = true;
                        RadNumericTxtEMLoadingDisability.Enabled = true;

                        //RadioButtonQuoteDisYes.Checked = true;
                        //////RadioButtonEscalationDis6.Checked = true;
                        //////if (intClass > 0)
                        //////{
                        //////    lblDisabilityMessage.Text += ", class:" + intClass.ToString();
                        //////}    


                        sqlCommandX = new SqlCommand();
                        sqlCommandX.Connection = sqlConnectionX;
                        sqlCommandX.CommandType = CommandType.StoredProcedure;
                        sqlCommandX.CommandText = "spx_Select_OccupationLimitsByOccupation";

                        sqlParam = new SqlParameter("Occupation", strEmployment);
                        sqlCommandX.Parameters.Add(sqlParam);

                        bool blnADW = false;
                        bool blnOCC = false;

                        sqlDR = sqlCommandX.ExecuteReader();
                        while (sqlDR.Read())
                        {
                            if (sqlDR.GetBoolean(1) == true)  //sql column 1 = ADW
                                blnADW = true;
                            if (sqlDR.GetBoolean(2) == true)  //sql column 2 = OCC
                                blnOCC = true;
                        }

                        sqlDR.Close();
                        sqlDR.Dispose();

                        if ((intClass >= 1) && (intClass < 4))
                        {
                            if (blnOCC == true)
                            {
                                RadioButtonTypeOfDisOCC.Enabled = true;
                                RadioButtonTypeOfDisOCC.Checked = true;
                                RadioButtonTypeOfDisADW.Enabled = true;
                                RadTxtDisabilityType.Text = "OCC";
                            }
                            else
                            {
                                if (blnADW == true)
                                {
                                    RadioButtonTypeOfDisOCC.Enabled = false;
                                    RadioButtonTypeOfDisOCC.Checked = false;
                                    RadioButtonTypeOfDisADW.Enabled = true;
                                    RadioButtonTypeOfDisADW.Checked = true;
                                    RadTxtDisabilityType.Text = "ADW";
                                }
                                else
                                {
                                    strResultDisability = "Occupation does not allow disability";
                                    lblDisabilityMessage.Text = strResultDisability;
                                    PanelDisability.GroupingText = "Disability - Unavailable";
                                    if ((RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB") && (RadComboBoxTypeBenefitLife.Items.Count() == 1))
                                    {
                                        //PanelDisability.Enabled = false;
                                        RadioButtonTypeOfDisADW.Enabled = false;
                                        RadioButtonTypeOfDisOCC.Enabled = false;
                                        RadioButtonEscalationDis6.Enabled = false;
                                        RadioButtonEscalationDis10.Enabled = false;
                                        RadioButtonQuoteDisYes.Enabled = false;
                                        RadioButtonQuoteDisNo.Enabled = false;
                                        RadNumericTxtCoverAmnDis.Enabled = false;
                                        RadNumericTxtPremiumDis.Enabled = false;
                                        RadNumericTxtEMLoadingDisability.Enabled = false;

                                        hideOption1Disability();
                                    }
                                    else
                                    {
                                        RadioButtonTypeOfDisADW.Enabled = true;
                                        RadioButtonTypeOfDisOCC.Enabled = true;
                                        RadioButtonEscalationDis6.Enabled = true;
                                        RadioButtonEscalationDis10.Enabled = true;
                                        RadioButtonQuoteDisYes.Enabled = true;
                                        RadioButtonQuoteDisNo.Enabled = true;
                                        RadNumericTxtCoverAmnDis.Enabled = true;
                                        RadNumericTxtPremiumDis.Enabled = true;
                                        RadNumericTxtEMLoadingDisability.Enabled = true;
                                    }
                                }
                            }
                        }


                        if (intClass > 3)
                        {
                            if (blnADW == true)
                            {
                                RadioButtonTypeOfDisOCC.Enabled = false;
                                RadioButtonTypeOfDisOCC.Checked = false;
                                RadioButtonTypeOfDisADW.Enabled = true;
                                RadioButtonTypeOfDisADW.Checked = true;
                                RadTxtDisabilityType.Text = "ADW";
                            }
                            else
                            {
                                strResultDisability = "Occupation does not allow disability";
                                lblDisabilityMessage.Text = strResultDisability;
                                PanelDisability.GroupingText = "Disability - Unavailable";
                                if ((RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB") && (RadComboBoxTypeBenefitLife.Items.Count() == 1))
                                {
                                    //PanelDisability.Enabled = false;
                                    RadioButtonTypeOfDisADW.Enabled = false;
                                    RadioButtonTypeOfDisOCC.Enabled = false;
                                    RadioButtonEscalationDis6.Enabled = false;
                                    RadioButtonEscalationDis10.Enabled = false;
                                    RadioButtonQuoteDisYes.Enabled = false;
                                    RadioButtonQuoteDisNo.Enabled = false;
                                    RadNumericTxtCoverAmnDis.Enabled = false;
                                    RadNumericTxtPremiumDis.Enabled = false;
                                    RadNumericTxtEMLoadingDisability.Enabled = false;
                                    hideOption1Disability();
                                }
                                else
                                {
                                    RadioButtonTypeOfDisADW.Enabled = true;
                                    RadioButtonTypeOfDisOCC.Enabled = true;
                                    RadioButtonEscalationDis6.Enabled = true;
                                    RadioButtonEscalationDis10.Enabled = true;
                                    RadioButtonQuoteDisYes.Enabled = true;
                                    RadioButtonQuoteDisNo.Enabled = true;
                                    RadNumericTxtCoverAmnDis.Enabled = true;
                                    RadNumericTxtPremiumDis.Enabled = true;
                                    RadNumericTxtEMLoadingDisability.Enabled = true;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region "Add the quote to the Quote audit table"
                
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_INSERT_QuoteAuditTrail";

                #region "Parameters"
                string strGender = string.Empty;
                string strHighestQualification = string.Empty;
                string strDiabetesType = string.Empty;
                string strInsulinUse = string.Empty;
                string strTabletUse = string.Empty; string strDoctorsVisits = string.Empty;
                string strDiabeticControl = string.Empty; string strHbA1c = string.Empty;
                string strExercisePlan = string.Empty; string strExHowWellFollowed = string.Empty;
                string strEatingPlan = string.Empty; string strEatHowWellFollowed = string.Empty;
                string strHighBP = string.Empty; string strHighCholesterol = string.Empty;
                string strMedicalAid = string.Empty; string strAlcohol = string.Empty;
                string strAlcoholUnits = string.Empty; string strTobacco = string.Empty; decimal decEscalationLife = 0; decimal decEscalationDisability = 0;
                string strTobaccoUnits = string.Empty;
                string strLifeBenifit = string.Empty; string strDisabilityBenifit = string.Empty;
                int intQuoteLife = 1; int intQuoteDisability = 1;

                sqlParam = new SqlParameter("MagnumID", RadTxtMagnumID.Text);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("UserID", Session["UserID"].ToString());
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("ClientName", RadTxtClientNameAndSurname.Text);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("IDType", RadComboIDType.SelectedItem.Text.ToString());
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("IDNumber", RadTxtIDNumber.Text.Trim());
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteDate", Convert.ToDateTime(RadDatePickerQuoteDate.SelectedDate.ToString()).ToString("yyyy-MM-dd"));
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("EmailAddress", RadTxtEmail.Text);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("DOB", Convert.ToDateTime(RadDatePickerDOB.SelectedDate.ToString()).ToString("yyyy-MM-dd"));
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("AgeAtNextBD", RadTxtAgeOfNextBirthday.Text);                
                    if (RadioButtonMale.Checked == true)
                        strGender = "M";
                    if (RadioButtonFemale.Checked == true)
                        strGender = "F";
                    if (RadioButtonOther.Checked == true)
                        strGender = "M";
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("Gender", strGender);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("IDValidation", LabelValidationMsg.Text);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("HighestQualification", strQualification);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("OccupationID", Convert.ToInt16(RadComboBoxOccupation.SelectedItem.Value));
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("Income", Convert.ToDecimal(RadNumericTxtIncome.Text.Trim()));
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("@MarriedStatusID", intMarriedTypeID);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("SpouseQualification", strSpouseQualification);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("SpouseIncome", Convert.ToDecimal(strSpouseIncome));
                sqlCommandX.Parameters.Add(sqlParam);
                //sqlParam = new SqlParameter("DateOfDiagnisis", Convert.ToDateTime(RadDatePickerDateOfDiag.SelectedDate.ToString()).ToString("yyyy-MM-dd"));
                sqlParam = new SqlParameter("DateOfDiagnisis", Convert.ToDateTime(RadMonthYearPickerDateOfDiag.SelectedDate.ToString()).ToString("yyyy"));
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonDiabetesType1.Checked)
                {
                    strDiabetesType = "type1";
                    HiddenFieldRMDiabetesType.Value = "T1";
                }
                if (RadioButtonDiabetesType2.Checked)
                {
                    strDiabetesType = "type2";
                    HiddenFieldRMDiabetesType.Value = "T2";
                }
                if (RadioButtonDiabetesTypeNotSure.Checked)
                {
                    strDiabetesType = "NotSure";
                    HiddenFieldRMDiabetesType.Value = "T2";
                }
                sqlParam = new SqlParameter("DiabetesType", strDiabetesType);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonInsulinYes.Checked)
                {
                    strInsulinUse = "Yes";
                    HiddenFieldRMDiabetesType.Value = "T1";
                }
                if (RadioButtonInsulinNo.Checked)
                    strInsulinUse = "No";
                if (RadioButtonInsulinNotSure.Checked)
                    strInsulinUse = "NotSure";
                sqlParam = new SqlParameter("InsulineUse", strInsulinUse);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonTabletUseYes.Checked)
                    strTabletUse = "Yes";
                if (RadioButtonTabletUseNo.Checked)
                    strTabletUse = "No";
                if (RadioButtonTabletUseNotSure.Checked)
                    strTabletUse = "NotSure";
                sqlParam = new SqlParameter("TabletUse", strTabletUse);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonDVYes1.Checked)
                    strDoctorsVisits = "1";
                if (RadioButtonDVYes2.Checked)
                    strDoctorsVisits = "2";
                if (RadioButtonDVYes3.Checked)
                    strDoctorsVisits = "3";
                if (RadioButtonDVNo.Checked)
                    strDoctorsVisits = "4";
                sqlParam = new SqlParameter("DoctorsVisits", strDoctorsVisits);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonDiabetControlExcellent.Checked)
                    strDiabeticControl = "Excellent";
                if (RadioButtonDiabetControlGood.Checked)
                    strDiabeticControl = "Good";
                if (RadioButtonDiabetControlMod.Checked)
                    strDiabeticControl = "Moderate";
                if (RadioButtonDiabetControlPoor.Checked)
                    strDiabeticControl = "Poor";
                sqlParam = new SqlParameter("DiabeticControl", strDiabeticControl);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonHbA1c3.Checked)
                    strHbA1c = "4";
                if (RadioButtonHbA1c4.Checked)
                    strHbA1c = "0";
                if (RadioButtonHbA1c6.Checked)
                    strHbA1c = "6";
                if (RadioButtonHbA1c7.Checked)
                    strHbA1c = "7";
                if (RadioButtonHbA1c8.Checked)
                    strHbA1c = "8";
                if (RadioButtonHbA1c9.Checked)
                    strHbA1c = "9";
                if (RadioButtonHbA1c10.Checked)
                    strHbA1c = "10";
                if (RadioButtonHbA1c11.Checked)
                    strHbA1c = "11";
                if (RadioButtonHbA1c12.Checked)
                    strHbA1c = "12";
                if (RadioButtonHbA1c15.Checked)
                    strHbA1c = "15";
                if (RadioButtonHbA1cUnknown.Checked)
                    strHbA1c = "0";
                sqlParam = new SqlParameter("HbA1c", strHbA1c);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonExPYes.Checked)
                    strExercisePlan = "Yes";
                if (RadioButtonExPNo.Checked)
                    strExercisePlan = "No";
                sqlParam = new SqlParameter("ExercisePlan", strExercisePlan);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonExFollowedVW.Checked)
                    strExHowWellFollowed = "Very Well";
                if (RadioButtonExFollowedok.Checked)
                    strExHowWellFollowed = "Ok";
                if (RadioButtonExFollowedpoor.Checked)
                    strExHowWellFollowed = "Poor";
                sqlParam = new SqlParameter("ExHowWellFollowed", strExHowWellFollowed);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonEatPYes.Checked)
                    strEatingPlan = "Yes";
                if (RadioButtonEatPNo.Checked)
                    strEatingPlan = "No";
                sqlParam = new SqlParameter("EatingPlan", strEatingPlan);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonEatFollowedVW.Checked)
                    strEatHowWellFollowed = "Very Well";
                if (RadioButtonEatFollowedOk.Checked)
                    strEatHowWellFollowed = "Ok";
                if (RadioButtonEatFollowedPoor.Checked)
                    strEatHowWellFollowed = "Poor";
                sqlParam = new SqlParameter("EatHowWellFollowed", strEatHowWellFollowed);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonHighBPYes.Checked)
                    strHighBP = "Yes";
                if (RadioButtonHighBPNo.Checked)
                    strHighBP = "No";
                if (RadioButtonHighBP.Checked)
                    strHighBP = "Not sure";
                sqlParam = new SqlParameter("HighBP", strHighBP);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonCholesterolYes.Checked)
                    strHighCholesterol = "Yes";
                if (RadioButtonCholesterolNo.Checked)
                    strHighCholesterol = "No";
                if (RadioButtonCholesterolNotSure.Checked)
                    strHighCholesterol = "Not sure";
                sqlParam = new SqlParameter("HighCholesterol", strHighCholesterol);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonMedicalAidNone.Checked)
                    strMedicalAid = "None";
                if (RadioButtonMedicalAidNotSure.Checked)
                    strMedicalAid = "Not sure";
                if (RadioButtonMedicalAidComp.Checked)
                    strMedicalAid = "Comprehensive";
                if (RadioButtonMedicalAidHos.Checked)
                    strMedicalAid = "Hospital plan";
                sqlParam = new SqlParameter("MedicalAid", strMedicalAid);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadNumericTxtHeight.Text != "")
                    sqlParam = new SqlParameter("Height", Convert.ToDecimal(RadNumericTxtHeight.Text));
                else
                    sqlParam = new SqlParameter("Height", Convert.ToDecimal("0"));
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadNumericTxtWeight.Text != "")
                    sqlParam = new SqlParameter("Weight", Convert.ToDecimal(RadNumericTxtWeight.Text));
                else
                    sqlParam = new SqlParameter("Weight", Convert.ToDecimal("0"));
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadNumericTxtPantSize.Text != "")
                    sqlParam = new SqlParameter("PantSize", Convert.ToInt16(RadNumericTxtPantSize.Text));
                else
                    sqlParam = new SqlParameter("PantSize", Convert.ToDecimal("0"));
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadNumericTxtBMI.Text != "")
                    sqlParam = new SqlParameter("BMI", Convert.ToDecimal(RadNumericTxtBMI.Text));
                else
                    sqlParam = new SqlParameter("BMI", Convert.ToDecimal("0"));
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonListAlcohol.SelectedValue == "non-drinker")
                    strAlcohol = "No";
                else
                    strAlcohol = "Yes";

                //if (RadioButtonAlcoholYes.Checked)
                //    strAlcohol = "Yes";
                //if (RadioButtonAlcoholNo.Checked)
                //    strAlcohol = "No";
                sqlParam = new SqlParameter("Alcohol", strAlcohol);
                sqlCommandX.Parameters.Add(sqlParam);
                
                sqlParam = new SqlParameter("AlcoholUnits", RadioButtonListAlcohol.SelectedValue.ToString());
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonListTobacco.SelectedValue == "non-smoker")
                    strTobacco = "No";
                else
                    strTobacco = "Yes";

                //if (RadioButtonTobaccoYes.Checked)
                //    strTobacco = "Yes";
                //if (RadioButtonTobaccoNo.Checked)
                //    strTobacco = "No";
                sqlParam = new SqlParameter("Tobacco", strTobacco);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter("TobaccoUnits", RadioButtonListTobacco.SelectedValue.ToString());
                sqlCommandX.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter("QualifyLife", LblQualificationMessage.Text);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter("QualifyDisability", lblDisabilityMessage.Text);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter("RiskBand", RadTxtRiskBand.Text);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlParam = new SqlParameter("DisabilityType", RadTxtDisabilityType.Text);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonEsc6.Checked)
                    decEscalationLife = 6;
                if (RadioButtonEsc10.Checked)
                    decEscalationLife = 10;

                //if (RadioButtonEscLife6.Checked)
                //    decEscalationLife = 6;
                //if (RadioButtonEscLife10.Checked)
                //    decEscalationLife = 10;

                sqlParam = new SqlParameter("EscalationLife", decEscalationLife);
                sqlCommandX.Parameters.Add(sqlParam);

                //if (RadioButtonEscalationDis6.Checked)
                //    decEscalationDisability = 6;
                //if (RadioButtonEscalationDis10.Checked)
                //    decEscalationDisability = 10;
                decEscalationDisability = 0;

                sqlParam = new SqlParameter("EscalationDisability", decEscalationDisability);
                sqlCommandX.Parameters.Add(sqlParam);

                strLifeBenifit = RadComboBoxTypeBenefitLife.SelectedItem.Text;
                sqlParam = new SqlParameter("LifeBenifit", strLifeBenifit);
                sqlCommandX.Parameters.Add(sqlParam);

                strDisabilityBenifit = RadComboBoxTypeBenefitDisability.SelectedItem.Text;
                sqlParam = new SqlParameter("DisabilityBenifit", strDisabilityBenifit);
                sqlCommandX.Parameters.Add(sqlParam);


                if (RadioButtonQuoteLifeNo.Checked == true)
                    intQuoteLife = 0;
                else
                    intQuoteLife = 1;

                sqlParam = new SqlParameter("QuoteLife", intQuoteLife);
                sqlCommandX.Parameters.Add(sqlParam);

                if (RadioButtonQuoteDisNo.Checked == true)
                    intQuoteDisability = 0;
                else
                    intQuoteDisability = 1;

                sqlParam = new SqlParameter("QuoteDisability", intQuoteDisability);
                sqlCommandX.Parameters.Add(sqlParam);        
        


                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    HiddenFieldQuoteAuditID.Value = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #endregion

                //The below was added so that if ADB or ACDB are selected iit does not take the qualification into account
                if (RadComboBoxTypeBenefitLife.SelectedItem.Text != "FDB")
                {
                    strResultLife = "Successful";
                }

                if (RadComboBoxTypeBenefitDisability.SelectedItem.Text != "FDB")
                {
                    strResultLife = "Successful";
                }

                if ((strResultLife == "Successful") || (strResultDisability == "Successful"))
                {
                    LblOffer.Text = "We can offer you:";
                    LblAcceptedQuote.Visible = true;
                    //RadBtnOption1.Visible = true;
                    RadioButtonOption1.Visible = true;
                    //RadBtnOption2.Visible = true;
                    RadioButtonOption2.Visible = true;
                    //RadBtnOption3.Visible = true;
                    RadioButtonOption3.Visible = true;
                    RadBtnOption4.Visible = true;
                    RadioButtonOption4.Visible = true;
                    RadBtnOption5.Visible = true;
                    RadioButtonOption5.Visible = true;

                    RadBtnOption1_Click(sender, e);
                    RadBtnOption2_Click(sender, e);
                    RadBtnOption3_Click(sender, e);
                }
                else
                {
                    LblOffer.Text = "Unfortnatley we are unable to offer you cover at this time";
                    LblAcceptedQuote.Visible = false;
                    lblOr.Visible = false;
                    RadBtnOption1.Visible = false;
                    RadioButtonOption1.Visible = false;
                    RadBtnOption2.Visible = false;
                    RadioButtonOption2.Visible = false;
                    RadBtnOption3.Visible = false;
                    RadioButtonOption3.Visible = false;
                    RadBtnOption4.Visible = false;
                    RadioButtonOption4.Visible = false;
                    RadBtnOption5.Visible = false;
                    RadioButtonOption5.Visible = false;
                    lblSuitable0.Visible = false;
                    lblAlso.Visible = false;
                    lblSuitable.Visible = false;
                    lblSpecificCover.Visible = false;
                    lblSpecificPremium.Visible = false;

                }

                PanelQuotePresentation.Visible = true;
                RadBtnQualifyClient.Enabled = true;

                sqlConnectionX.Close();

            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;   
                //throw;
            }
        }

        protected void RadButtonValidateID_Click(object sender, EventArgs e)
        {
            AllLifeGeneric.AllLifeGenericClient ALG = new AllLifeGeneric.AllLifeGenericClient();
            AllLifeGeneric.IDValidationResult IDResult = new AllLifeGeneric.IDValidationResult();
            IDResult = ALG.IDValidation(RadTxtIDNumber.Text.Trim());

            string strDOB = RadDatePickerDOB.SelectedDate.ToString();
            string strGender = string.Empty;
            string strMessage = string.Empty;

            if (RadioButtonMale.Checked == true)
                strGender = "Male";
            if (RadioButtonFemale.Checked == true)
                strGender = "Female";
            if (RadioButtonOther.Checked == true)
                strGender = "Male";

            if (strDOB != "")
            {
                DateTime date = DateTime.Parse(strDOB);
                strDOB = date.ToString("yyMMdd");
            }
            else
            {
                strDOB = "";
            }

            if (strDOB != IDResult.DOB)
            {
                if (strMessage.Length == 0)
                {
                    strMessage = "Failed: Date of birth";
                }
                else
                {
                    strMessage += ", Failed: Date of birth";
                }
            }

            if (strGender != IDResult.Gender)
            {
                if (strMessage.Length == 0)
                {
                    strMessage = "Failed: Gender";
                }
                else
                {
                    strMessage += ", Failed: Gender";
                }
            }

            //LabelValidationMsg.Text = IDResult.Message;
            //strMessage = IDResult.Message;
            if ((IDResult.Message == "Successful") && (strMessage.Length == 0))
            {
                LabelValidationMsg.BackColor = System.Drawing.Color.Green;
                LabelValidationMsg.ForeColor = System.Drawing.Color.White;
                LabelValidationMsg.Text = IDResult.Message;
            }
            else
            {
                LabelValidationMsg.BackColor = System.Drawing.Color.Red;
                LabelValidationMsg.ForeColor = System.Drawing.Color.White;
                //LabelValidationMsg.Text = IDResult.Message;               
                LabelValidationMsg.Text = strMessage;
            }

            if (IDResult.Message == "Failed: Sum Check")
            {
                LabelValidationMsg.BackColor = System.Drawing.Color.Red;
                LabelValidationMsg.ForeColor = System.Drawing.Color.White;

                if (strMessage.Length == 0)
                {
                    LabelValidationMsg.Text = IDResult.Message;
                }
                else
                {
                    LabelValidationMsg.Text = strMessage + ", " + IDResult.Message;
                }
            }
           
        }

        protected void RadioButtonAlcoholYes_CheckedChanged(object sender, EventArgs e)
        {
            ////if (RadioButtonAlcoholYes.Checked == true)
            ////{
            ////    RadioButtonAlcoholnon.Enabled = true;
            ////    RadioButtonAlcohol05.Enabled = true;
            ////    RadioButtonAlcohol5plus.Enabled = true;
            ////}
            ////else
            ////{
            ////    RadioButtonAlcoholnon.Enabled = false;
            ////    RadioButtonAlcoholnon.Checked = false;
            ////    RadioButtonAlcohol05.Enabled = false;
            ////    RadioButtonAlcohol05.Checked = false;
            ////    RadioButtonAlcohol5plus.Enabled = false;
            ////    RadioButtonAlcohol5plus.Checked = false;
            ////}

            //if (RadioButtonAlcoholYes.Checked == true)
            //{
            //    RadioButtonListAlcohol.Enabled = true;                
            //}
            //else
            //{
            //    RadioButtonListAlcohol.Enabled = false;
            //    RadioButtonListAlcohol.ClearSelection();
            //    //foreach (ListItem li in RadioButtonListAlcohol)
            //    //{
            //    //    if (li.Selected)
            //    //    {
            //    //        li.Selected = false;
            //    //    }
            //    //}                       
            //}

            //ScriptManager.RegisterStartupScript(this, GetType(), "CheckQualifyButton", "CheckQualifyButton();", true);
        }

        protected void RadioButtonTobaccoYes_CheckedChanged(object sender, EventArgs e)
        {
            ////if (RadioButtonTobaccoYes.Checked == true)
            ////{
            ////    RadioButtonTobaccoUnits0.Enabled = true;
            ////    RadioButtonTobaccoUnits5.Enabled = true;
            ////    RadioButtonTobaccoUnits20.Enabled = true;
            ////    RadioButtonTobaccoUnits20plus.Enabled = true;
            ////}
            ////else
            ////{
            ////    RadioButtonTobaccoUnits0.Enabled = false;
            ////    RadioButtonTobaccoUnits0.Checked = false;
            ////    RadioButtonTobaccoUnits5.Enabled = false;
            ////    RadioButtonTobaccoUnits5.Checked = false;
            ////    RadioButtonTobaccoUnits20.Enabled = false;
            ////    RadioButtonTobaccoUnits20.Checked = false;
            ////    RadioButtonTobaccoUnits20plus.Enabled = false;
            ////    RadioButtonTobaccoUnits20plus.Checked = false;
            ////}
            //if (RadioButtonTobaccoYes.Checked == true)
            //{
            //    RadioButtonListTobacco.Enabled = true;
            //}
            //else
            //{
            //    RadioButtonListTobacco.Enabled = false;
            //    RadioButtonListTobacco.ClearSelection();                     
            //}

            //ScriptManager.RegisterStartupScript(this, GetType(), "CheckQualifyButton", "CheckQualifyButton();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RadBtnQualifyClient.Enabled = true;
        }

        protected void RadioButtonTypeOfDisADW_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButtonTypeOfDisOCC.Enabled == true)
            {
                RadAjaxManager1.ResponseScripts.Add("radconfirm('The client qualifies for OCC, you have selected ADW. Are you sure?', confirmCallBackFn);");
            }
        }

        protected void RadioButtonTypeOfDisOCC_CheckedChanged(object sender, EventArgs e)
        {
            //if (RadioButtonTypeOfDisOCC.Enabled == true)
            //{
            //    RadTxtDisabilityType.Text = "OCC";
            //}
            //else
            //{
            //    RadTxtDisabilityType.Text = "ADW";
            //}
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.ToString() == "ok")
            {
                //lblScript.Text = "OK";
                RadioButtonTypeOfDisADW.Checked = true;
                RadioButtonTypeOfDisOCC.Checked = false;
                //RadTxtDisabilityType.Text = "ADW";
            }
            else
            {
                //lblScript.Text = "Cancel";
                RadioButtonTypeOfDisADW.Checked = false;
                RadioButtonTypeOfDisOCC.Checked = true;
                //RadTxtDisabilityType.Text = "OCC";
            }

            if (e.Argument.ToString() == "ClearQuoteOk")
            {
                Response.Redirect("Quote.aspx", true);
            }

            if (e.Argument == "CloseReset")
            {
                RadWindowManager1.Windows.Clear();
                Response.Redirect("Quote.aspx", true);
            }
            
            if (e.Argument == "Rebind")
            {
                //Response.Redirect("ChecklistAdmin.aspx");
                RadWindowManager1.Windows.Clear();
                //RadAjaxManager1.ResponseScripts.Add("radalert('Rebind',300,100,'Rebind');");
            }

            if (e.Argument == "SessionExpired")
            {
                RadWindowManager1.Windows.Clear();
                Response.Redirect("~/Login.aspx?ID=9",true);
            }

            if (e.Argument == "CheckMagID")
            {
                //Check MagID IN DB
                #region "Check SQL
                SqlConnection sqlConnectionX;
                SqlCommand sqlCommandX;
                SqlParameter sqlParam;
                SqlDataReader sqlDR;

                try
                {
                    Int32 intRowCount = 0;
                    sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                    sqlConnectionX.Open();

                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_SELECT_QuoteAuditTrailCountByMagnumID";

                    sqlParam = new SqlParameter("MagnumID", RadTxtMagnumID.Text.Trim());
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    while (sqlDR.Read())
                    {
                        #region "Get values"
                        if (!sqlDR.IsDBNull(0))
                            intRowCount = Convert.ToInt32(sqlDR.GetValue(0));
                        #endregion
                    }                    

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();
                    sqlConnectionX.Close();

                    if (intRowCount > 0)
                    {
                        RadButtonLoadQuote.Enabled = true;
                    }
                    else
                    { 
                        RadButtonLoadQuote.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                }


                #endregion
            }

            if (e.Argument == "hideQuoteP")
            {
                PanelQuotePresentation.Visible = false;
                lblInfo.Text = "You have changed one of the risk modifier fields so you will need to re-quote to view the quote options";
                lblInfo2.Text = "You have changed one of the risk modifier fields so you will need to re-quote to view the quote options";
            }

            if (e.Argument.Length > 11)
            {
                #region "PopulatePage"
                if (e.Argument.Substring(0, 12) == "PopulatePage")
                {
                    RadWindowManager1.Windows.Clear();
                    //RadAjaxManager1.ResponseScripts.Add("radalert('" + e.Argument + "',300,100,'RebindAndNavigate');");

                    //get the data from the DB based on the Audit IDs and populate all the controls
                    SqlConnection sqlConnectionX;
                    SqlCommand sqlCommandX;
                    SqlParameter sqlParam;
                    SqlDataReader sqlDR;

                    string strClientName = string.Empty; string strIDtype = string.Empty; string strIDNumber = string.Empty;
                    string strQuoteDate = string.Empty; string strEmailAddress = string.Empty;

                    string strDOB = string.Empty; string strAgeAtNextBD = string.Empty; string strGender = string.Empty; string strIDValidation = string.Empty;
                    string strHighestQualification = string.Empty; string strOccupationID = string.Empty; string strIncome = string.Empty;
                    string strSpouseQualification = string.Empty; string strSpouseIncome = string.Empty; string strDateOfDiagnisis = string.Empty; string strDiabetesType = string.Empty;
                    string strInsulineUse = string.Empty; string strTabletUse = string.Empty; string strDoctorsVisits = string.Empty; string strDiabeticControl = string.Empty; string strHbA1c = string.Empty;
                    string strExercisePlan = string.Empty; string strExHowWellFollowed = string.Empty; string strEatingPlan = string.Empty; string strEatHowWellFollowed = string.Empty;
                    string strHighBP = string.Empty; string strHighCholesterol = string.Empty; string strMedicalAid = string.Empty; string strHeight = string.Empty; string strWeight = string.Empty; string strPantSize = string.Empty;
                    string strBMI = string.Empty; string strAlcohol = string.Empty; string strAlcoholUnits = string.Empty; string strTobacco = string.Empty; string strTobaccoUnits = string.Empty;
                    string strQualifyLife = string.Empty; string strQualifyDisability = string.Empty; string strRiskBand = string.Empty; string strDisabilityType = string.Empty;
                    string strEscalationLife = string.Empty; string strEscalationDisability = string.Empty;
                    string strLifeBenifit = string.Empty; string strDisabilityBenifit = string.Empty;
                    int intMarriedStatusID = 0; int intQuoteLife = 0; int intQuoteDisability = 0;

                    string strArgument = e.Argument.Substring(13);
                    string[] Arguments;
                    string strAuditID = string.Empty;
                    string strQuoteOptionAuditID = string.Empty;

                    if (strArgument != "AuditID=,QuoteOptionAuditTrailID=")
                    {
                        if (strArgument != "0")
                        {
                            #region "load the quote info"
                            Arguments = strArgument.Split(',');
                            strAuditID = Arguments[0].ToString();
                            int intpos = strAuditID.IndexOf("=");
                            strAuditID = strAuditID.Substring(intpos + 1);

                            HiddenFieldQuoteAuditID.Value = strAuditID;

                            strQuoteOptionAuditID = Arguments[1].ToString();
                            intpos = strQuoteOptionAuditID.IndexOf("=");
                            strQuoteOptionAuditID = strQuoteOptionAuditID.Substring(intpos + 1);

                            try
                            {
                                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                                sqlConnectionX.Open();

                                sqlCommandX = new SqlCommand();
                                sqlCommandX.Connection = sqlConnectionX;
                                sqlCommandX.CommandType = CommandType.StoredProcedure;
                                sqlCommandX.CommandText = "spx_SELECT_QuoteDetailByAuditTrailID";

                                sqlParam = new SqlParameter("QuoteAuditTrailID", strAuditID);
                                sqlCommandX.Parameters.Add(sqlParam);

                                sqlDR = sqlCommandX.ExecuteReader();
                                while (sqlDR.Read())
                                {
                                    #region "Get values"
                                    if (!sqlDR.IsDBNull(0))
                                        strClientName = sqlDR.GetValue(0).ToString();
                                    if (!sqlDR.IsDBNull(1))
                                        strIDtype = sqlDR.GetValue(1).ToString();
                                    if (!sqlDR.IsDBNull(2))
                                        strIDNumber = sqlDR.GetValue(2).ToString();
                                    if (!sqlDR.IsDBNull(3))
                                        strQuoteDate = sqlDR.GetValue(3).ToString();
                                    if (!sqlDR.IsDBNull(4))
                                        strEmailAddress = sqlDR.GetValue(4).ToString();
                                    if (!sqlDR.IsDBNull(5))
                                        strDOB = sqlDR.GetValue(5).ToString();
                                    if (!sqlDR.IsDBNull(6))
                                        strAgeAtNextBD = sqlDR.GetValue(6).ToString();
                                    if (!sqlDR.IsDBNull(7))
                                        strGender = sqlDR.GetValue(7).ToString();
                                    if (!sqlDR.IsDBNull(8))
                                        strIDValidation = sqlDR.GetValue(8).ToString();
                                    if (!sqlDR.IsDBNull(9))
                                        strHighestQualification = sqlDR.GetValue(9).ToString();
                                    if (!sqlDR.IsDBNull(10))
                                        strOccupationID = sqlDR.GetValue(10).ToString();
                                    if (!sqlDR.IsDBNull(11))
                                        strIncome = sqlDR.GetValue(11).ToString();
                                    if (!sqlDR.IsDBNull(12))
                                        strSpouseQualification = sqlDR.GetValue(12).ToString();
                                    if (!sqlDR.IsDBNull(13))
                                        strSpouseIncome = sqlDR.GetValue(13).ToString();
                                    if (!sqlDR.IsDBNull(14))
                                        strDateOfDiagnisis = sqlDR.GetValue(14).ToString();
                                    if (!sqlDR.IsDBNull(15))
                                        strDiabetesType = sqlDR.GetValue(15).ToString();
                                    if (!sqlDR.IsDBNull(16))
                                        strInsulineUse = sqlDR.GetValue(16).ToString();
                                    if (!sqlDR.IsDBNull(17))
                                        strTabletUse = sqlDR.GetValue(17).ToString();
                                    if (!sqlDR.IsDBNull(18))
                                        strDoctorsVisits = sqlDR.GetValue(18).ToString();
                                    if (!sqlDR.IsDBNull(19))
                                        strDiabeticControl = sqlDR.GetValue(19).ToString();
                                    if (!sqlDR.IsDBNull(20))
                                        strHbA1c = sqlDR.GetValue(20).ToString();
                                    if (!sqlDR.IsDBNull(21))
                                        strExercisePlan = sqlDR.GetValue(21).ToString();
                                    if (!sqlDR.IsDBNull(22))
                                        strExHowWellFollowed = sqlDR.GetValue(22).ToString();
                                    if (!sqlDR.IsDBNull(23))
                                        strEatingPlan = sqlDR.GetValue(23).ToString();
                                    if (!sqlDR.IsDBNull(24))
                                        strEatHowWellFollowed = sqlDR.GetValue(24).ToString();
                                    if (!sqlDR.IsDBNull(25))
                                        strHighBP = sqlDR.GetValue(25).ToString();
                                    if (!sqlDR.IsDBNull(26))
                                        strHighCholesterol = sqlDR.GetValue(26).ToString();
                                    if (!sqlDR.IsDBNull(27))
                                        strMedicalAid = sqlDR.GetValue(27).ToString();
                                    if (!sqlDR.IsDBNull(28))
                                        strHeight = sqlDR.GetValue(28).ToString();
                                    if (!sqlDR.IsDBNull(29))
                                        strWeight = sqlDR.GetValue(29).ToString();
                                    if (!sqlDR.IsDBNull(30))
                                        strPantSize = sqlDR.GetValue(30).ToString();
                                    if (!sqlDR.IsDBNull(31))
                                        strBMI = sqlDR.GetValue(31).ToString();
                                    if (!sqlDR.IsDBNull(32))
                                        strAlcohol = sqlDR.GetValue(32).ToString();
                                    if (!sqlDR.IsDBNull(33))
                                        strAlcoholUnits = sqlDR.GetValue(33).ToString();
                                    if (!sqlDR.IsDBNull(34))
                                        strTobacco = sqlDR.GetValue(34).ToString();
                                    if (!sqlDR.IsDBNull(35))
                                        strTobaccoUnits = sqlDR.GetValue(35).ToString();
                                    if (!sqlDR.IsDBNull(36))
                                        strQualifyLife = sqlDR.GetValue(36).ToString();
                                    if (!sqlDR.IsDBNull(37))
                                        strQualifyDisability = sqlDR.GetValue(37).ToString();
                                    if (!sqlDR.IsDBNull(38))
                                        strRiskBand = sqlDR.GetValue(38).ToString();
                                    if (!sqlDR.IsDBNull(39))
                                        strDisabilityType = sqlDR.GetValue(39).ToString();
                                    if (!sqlDR.IsDBNull(40))
                                        strEscalationLife = sqlDR.GetValue(40).ToString();
                                    if (!sqlDR.IsDBNull(41))
                                        strEscalationDisability = sqlDR.GetValue(41).ToString();
                                    if (!sqlDR.IsDBNull(42))
                                        intMarriedStatusID = sqlDR.GetInt32(42);
                                    if (!sqlDR.IsDBNull(43))
                                        intQuoteLife = sqlDR.GetInt32(43);
                                    if (!sqlDR.IsDBNull(44))
                                        intQuoteDisability = sqlDR.GetInt32(44);
                                    if (!sqlDR.IsDBNull(45))
                                        strLifeBenifit = sqlDR.GetValue(45).ToString();
                                    if (!sqlDR.IsDBNull(46))
                                        strDisabilityBenifit = sqlDR.GetValue(46).ToString();                                   
                                    
                                    #endregion
                                }

                                sqlDR.Close();
                                sqlCommandX.Cancel();
                                sqlCommandX.Dispose();
                                sqlConnectionX.Close();

                                RadTxtClientNameAndSurname.Text = strClientName;
                                lblValClient.Text = "";
                                RadComboBoxItem item = RadComboIDType.FindItemByText(strIDtype);
                                item.Selected = true;
                                RadTxtIDNumber.Text = strIDNumber;
                                if (strIDNumber.Length == 13)
                                    RadButtonValidateID.Enabled = true;

                                if (System.DateTime.Now >= Convert.ToDateTime(strQuoteDate).AddDays(30))
                                    RadDatePickerQuoteDate.SelectedDate = System.DateTime.Now;
                                else
                                    RadDatePickerQuoteDate.SelectedDate = Convert.ToDateTime(strQuoteDate);

                                RadTxtEmail.Text = strEmailAddress;
                                RadDatePickerDOB.SelectedDate = Convert.ToDateTime(strDOB);
                                int NumYears = Convert.ToDateTime(strQuoteDate).Year - Convert.ToDateTime(strDOB).Year;
                                if (Convert.ToDateTime(strQuoteDate).Month > Convert.ToDateTime(strDOB).Month || (Convert.ToDateTime(strQuoteDate).Month == Convert.ToDateTime(strDOB).Month && Convert.ToDateTime(strQuoteDate).Day > Convert.ToDateTime(strDOB).Day))
                                    NumYears++;
                                ///if (Convert.ToDateTime(strQuoteDate).Month >)
                                //double NumYears =  span.TotalDays
                                //int intANBcalced = Convert.ToDateTime(Convert.ToDateTime(strQuoteDate) - Convert.ToDateTime(strDOB)).Year;
                                lblValDOB.Text = "";

                                if (NumYears == Convert.ToInt16(strAgeAtNextBD))
                                    RadTxtAgeOfNextBirthday.Text = strAgeAtNextBD;
                                else
                                    RadTxtAgeOfNextBirthday.Text = NumYears.ToString();

                                string strAgeBracket = string.Empty;
                                
                                #region "Get Age Bracket"
                                if (Convert.ToInt16(strAgeAtNextBD) < 30)
                                {
                                    strAgeBracket = "A1";
                                }

                                if ((Convert.ToInt16(strAgeAtNextBD) >= 30) && (Convert.ToInt16(strAgeAtNextBD) < 40))
                                {
                                    strAgeBracket = "A2";
                                }

                                if ((Convert.ToInt16(strAgeAtNextBD) >= 40) && (Convert.ToInt16(strAgeAtNextBD) < 50))
                                {
                                    strAgeBracket = "A3";
                                }

                                if ((Convert.ToInt16(strAgeAtNextBD) >= 50) && (Convert.ToInt16(strAgeAtNextBD) < 60))
                                {
                                    strAgeBracket = "A4";
                                }

                                if ((Convert.ToInt16(strAgeAtNextBD) >= 60) && (Convert.ToInt16(strAgeAtNextBD) < 70))
                                {
                                    strAgeBracket = "A5";
                                }

                                if (Convert.ToInt16(strAgeAtNextBD) >= 70)
                                {
                                    strAgeBracket = "A6";
                                }
                                #endregion

                                HiddenFieldRBDOB.Value = strAgeBracket;

                                #region "Gender"
                                if (strGender == "M")
                                    RadioButtonMale.Checked = true;
                                if (strGender == "F")
                                    RadioButtonFemale.Checked = true;
                                if (strGender == "O")
                                    RadioButtonOther.Checked = true;
                                lblValGender.Text = "";
                                #endregion

                                #region "IDValidation label"
                                if (strIDValidation == "Successful")
                                {
                                    LabelValidationMsg.BackColor = System.Drawing.Color.Green;
                                    LabelValidationMsg.ForeColor = System.Drawing.Color.White;
                                    LabelValidationMsg.Text = strIDValidation;
                                }
                                else
                                {
                                    if (strIDValidation == "ID not validated")
                                    {
                                        //BackColor="#999999" ForeColor="White"
                                        LabelValidationMsg.BackColor = System.Drawing.ColorTranslator.FromHtml("#999999");
                                        LabelValidationMsg.ForeColor = System.Drawing.Color.White;
                                        LabelValidationMsg.Text = "ID not validated";
                                    }
                                    else
                                    {
                                        LabelValidationMsg.BackColor = System.Drawing.Color.Red;
                                        LabelValidationMsg.ForeColor = System.Drawing.Color.White;
                                        LabelValidationMsg.Text = strIDValidation;
                                    }
                                }
                                #endregion

                                #region "HighestQualification"
                                if (strHighestQualification == "No matric")
                                    RadioButtonNotMat.Checked = true;
                                if (strHighestQualification == "Matric")
                                    RadioButtonMatriculated.Checked = true;
                                if (strHighestQualification == "3 or 4 yr. Diploma/3 yr. Degree")
                                    RadioButtonDiploma.Checked = true;
                                if (strHighestQualification == "4 yr. Degree/professional qualification")
                                    RadioButtonDegree.Checked = true;
                                lblValHighQual.Text = "";
                                #endregion

                                RadComboBoxItem itemOccupation = RadComboBoxOccupation.FindItemByValue(strOccupationID);
                                if (itemOccupation != null)
                                    itemOccupation.Selected = true;

                                RadNumericTxtIncome.Text = strIncome;
                                lblValIncome.Text = "";

                                #region "SpouseQualification"
                                if (strSpouseQualification == "No matric")
                                    RadioButtonSNotMat.Checked = true;
                                if (strSpouseQualification == "Matric")
                                    RadioButtonSMat.Checked = true;
                                if (strSpouseQualification == "3 or 4 yr. Diploma/3 yr. Degree")
                                    RadioButtonSDip.Checked = true;
                                if (strSpouseQualification == "4 yr. Degree/professional qualification")
                                    RadioButtonSDegree.Checked = true;
                                lblSpouseQualification.Text = "";

                                RadNumericTxtSpouseIncome.Text = strSpouseIncome;
                                lblSpouseIncome.Text = "";
                                #endregion

                                //RadDatePickerDateOfDiag.SelectedDate = Convert.ToDateTime(strDateOfDiagnisis);
                                if (strDateOfDiagnisis.Length == 4)
                                {
                                    RadMonthYearPickerDateOfDiag.SelectedDate = Convert.ToDateTime(strDateOfDiagnisis + "-01-01");
                                }
                                else
                                {
                                    RadMonthYearPickerDateOfDiag.SelectedDate = Convert.ToDateTime(strDateOfDiagnisis);
                                }
                                lblValDateDiag.Text = "";

                                if (intMarriedStatusID == 0)
                                {
                                    RadioButtonMaritalStatusNotMarried.Checked = true;
                                    RadioButtonMaritalStatusMarried.Checked = false;
                                }

                                if (intMarriedStatusID == 1)
                                {
                                    RadioButtonMaritalStatusMarried.Checked = true;
                                    RadioButtonMaritalStatusNotMarried.Checked = false;
                                }

                                #region "Diabetes Type"
                                if (strDiabetesType == "type1")
                                {
                                    RadioButtonDiabetesType1.Checked = true;
                                    HiddenFieldRMDiabetesType.Value = "T1";
                                }
                                if (strDiabetesType == "type2")
                                {
                                    RadioButtonDiabetesType2.Checked = true;
                                    HiddenFieldRMDiabetesType.Value = "T2";
                                }
                                if (strDiabetesType == "NotSure")
                                {
                                    RadioButtonDiabetesTypeNotSure.Checked = true;
                                    HiddenFieldRMDiabetesType.Value = "T1";

                                    if (strInsulineUse == "Yes")
                                        RadioButtonInsulinYes.Checked = true;
                                    if (strInsulineUse == "No")
                                        RadioButtonInsulinNo.Checked = true;
                                    if (strInsulineUse == "NotSure")
                                        RadioButtonInsulinNotSure.Checked = true;

                                    if (strTabletUse == "Yes")
                                        RadioButtonTabletUseYes.Checked = true;
                                    if (strTabletUse == "No")
                                        RadioButtonTabletUseNo.Checked = true;
                                    if (strTabletUse == "NotSure")
                                        RadioButtonTabletUseNotSure.Checked = true;
                                }
                                lblValDiabetesType.Text = "";
                                #endregion

                                #region "Doc Visits"
                                if (strDoctorsVisits == "1")
                                    RadioButtonDVYes1.Checked = true;
                                if (strDoctorsVisits == "2")
                                    RadioButtonDVYes2.Checked = true;
                                if (strDoctorsVisits == "3")
                                    RadioButtonDVYes3.Checked = true;
                                if (strDoctorsVisits == "4")
                                    RadioButtonDVNo.Checked = true;
                                lblValDocVisits.Text = "";
                                #endregion

                                #region "DiabeticControl"
                                if (strDiabeticControl == "Excellent")
                                    RadioButtonDiabetControlExcellent.Checked = true;
                                if (strDiabeticControl == "Good")
                                    RadioButtonDiabetControlGood.Checked = true;
                                if (strDiabeticControl == "Moderate")
                                    RadioButtonDiabetControlMod.Checked = true;
                                if (strDiabeticControl == "Poor")
                                    RadioButtonDiabetControlPoor.Checked = true;
                                lblValDiabeticControl.Text = "";
                                #endregion

                                #region "HbA1c"
                                if (strHbA1c == "0")
                                    RadioButtonHbA1cUnknown.Checked = true;
                                if (strHbA1c == "4")
                                {
                                    if (RadioButtonHbA1c3.Visible == true)
                                    {
                                        RadioButtonHbA1c3.Checked = true;
                                    }
                                    else
                                    {
                                        RadioButtonHbA1c4.Checked = true;
                                    }
                                    //if (strHbA1c == "4")
                                    //    RadioButtonHbA1c4.Checked = true;
                                }
                                if (strHbA1c == "6")
                                    RadioButtonHbA1c6.Checked = true;
                                if (strHbA1c == "7")
                                    RadioButtonHbA1c7.Checked = true;
                                if (strHbA1c == "8")
                                    RadioButtonHbA1c8.Checked = true;
                                if (strHbA1c == "9")
                                    RadioButtonHbA1c9.Checked = true;
                                if (strHbA1c == "10")
                                    RadioButtonHbA1c10.Checked = true;
                                if (strHbA1c == "11")
                                    RadioButtonHbA1c11.Checked = true;
                                if (strHbA1c == "12")
                                    RadioButtonHbA1c12.Checked = true;
                                if (strHbA1c == "15")
                                    RadioButtonHbA1c15.Checked = true;
                                lblValHbA1c.Text = "";
                                #endregion

                                #region "ExercisePlan"
                                if (strExercisePlan == "Yes")
                                {
                                    RadioButtonExPYes.Checked = true;
                                    RadioButtonExPNo.Checked = false;

                                    if (strExHowWellFollowed == "Very Well")
                                    {
                                        RadioButtonExFollowedVW.Checked = true;
                                        RadioButtonExFollowedok.Checked = false;
                                        RadioButtonExFollowedpoor.Checked = false;
                                    }
                                    //if (string.Equals(strExHowWellFollowed, "OK", StringComparison.CurrentCultureIgnoreCase) == true)
                                    if (strExHowWellFollowed == "Ok")
                                    {
                                        RadioButtonExFollowedVW.Checked = false;
                                        RadioButtonExFollowedok.Checked = true;
                                        RadioButtonExFollowedpoor.Checked = false;
                                    }
                                    if (strExHowWellFollowed == "Poor")
                                    {
                                        RadioButtonExFollowedVW.Checked = false;
                                        RadioButtonExFollowedpoor.Checked = true;
                                        RadioButtonExFollowedok.Checked = false;
                                    }
                                }
                                if (strExercisePlan == "No")
                                {
                                    RadioButtonExPNo.Checked = true;
                                    RadioButtonExPYes.Checked = false;
                                }
                                lblValExP.Text = "";
                                #endregion

                                #region "EatingPlan"
                                if (strEatingPlan == "Yes")
                                {
                                    RadioButtonEatPYes.Checked = true;
                                    RadioButtonEatPNo.Checked = false;

                                    if (strEatHowWellFollowed == "Very Well")
                                    {
                                        RadioButtonEatFollowedVW.Checked = true;
                                        RadioButtonEatFollowedOk.Checked = false;
                                        RadioButtonEatFollowedPoor.Checked = false;
                                    }
                                    //if (string.Equals(strEatHowWellFollowed, "OK", StringComparison.CurrentCultureIgnoreCase) == true)
                                    if (strEatHowWellFollowed == "Ok")
                                    {
                                        RadioButtonEatFollowedVW.Checked = false;
                                        RadioButtonEatFollowedOk.Checked = true;
                                        RadioButtonEatFollowedPoor.Checked = false;

                                    }
                                    if (strEatHowWellFollowed == "Poor")
                                    {
                                        RadioButtonEatFollowedVW.Checked = false;
                                        RadioButtonEatFollowedOk.Checked = false;
                                        RadioButtonEatFollowedPoor.Checked = true;
                                    }
                                }
                                if (strEatingPlan == "No")
                                {
                                    RadioButtonEatPYes.Checked = false;
                                    RadioButtonEatPNo.Checked = true;
                                }
                                lblValEatP.Text = "";
                                #endregion

                                #region "HighBP"
                                if (strHighBP == "Yes")
                                    RadioButtonHighBPYes.Checked = true;
                                if (strHighBP == "No")
                                    RadioButtonHighBPNo.Checked = true;
                                if (strHighBP == "Not sure")
                                    RadioButtonHighBP.Checked = true;
                                lblvalBP.Text = "";
                                #endregion

                                #region "HighCholesterol"
                                if (strHighCholesterol == "Yes")
                                    RadioButtonCholesterolYes.Checked = true;
                                if (strHighCholesterol == "No")
                                    RadioButtonCholesterolNo.Checked = true;
                                if (strHighCholesterol == "Not sure")
                                    RadioButtonCholesterolNotSure.Checked = true;
                                lblValChol.Text = "";

                                #endregion

                                #region "MedicalAid"
                                if (strMedicalAid == "None")
                                    RadioButtonMedicalAidNone.Checked = true;
                                if (strMedicalAid == "Not sure")
                                    RadioButtonMedicalAidNotSure.Checked = true;
                                if (strMedicalAid == "Comprehensive")
                                    RadioButtonMedicalAidComp.Checked = true;
                                if (strMedicalAid == "Hospital plan")
                                    RadioButtonMedicalAidHos.Checked = true;
                                #endregion

                                #region "Height"
                                RadNumericTxtHeight.Text = strHeight;
                                lblValHeight.Text = "";
                                decimal decFeet = Convert.ToDecimal(strHeight) / Convert.ToDecimal(0.3048);
                                var values = decFeet.ToString().Split('.');
                                int firstValue = int.Parse(values[0]);
                                decimal decNewFeet = decFeet - Convert.ToDecimal(firstValue);
                                decimal decInches = decNewFeet * 12;

                                RadNumericTxtFeet.Text = firstValue.ToString();
                                RadNumericTxtInches.Text = decInches.ToString();

                                #endregion

                                #region "Weight"
                                RadNumericTxtWeight.Text = strWeight;
                                lblValWeight.Text = "";
                                
                                decimal decWeight = Convert.ToDecimal(strWeight);
                                                              
                                decimal decNearExact = decWeight / Convert.ToDecimal(0.45359237);
                                RadNumericTxtpounds.Text = decNearExact.ToString();                            

                                #endregion

                                #region "PantSize"
                                RadNumericTxtPantSize.Text = strPantSize;
                                lblValPants.Text = "";
                                #endregion

                                #region "BMI"
                                RadNumericTxtBMI.Text = strBMI;
                                #endregion

                                #region "Alcohol"
                                //if (strAlcohol == "Yes")
                                //{
                                //    RadioButtonAlcoholYes.Checked = true;

                                //    RadioButtonListAlcohol.Enabled = true;
                                //    RadioButtonListAlcohol.SelectedValue = strAlcoholUnits;

                                //}
                                if (strAlcohol == "No")
                                {
                                    //RadioButtonAlcoholNo.Checked = true;
                                    RadioButtonListAlcohol.SelectedValue = "non-drinker";
                                }
                                else
                                {
                                    RadioButtonListAlcohol.SelectedValue = strAlcoholUnits;
                                }
                                lblValAlcoholUnits.Text = "";

                                #endregion

                                #region "Tobacco"
                                //if (strTobacco == "Yes")
                                //{
                                //    RadioButtonTobaccoYes.Checked = true;
                                //    RadioButtonListTobacco.Enabled = true;
                                //    RadioButtonListTobacco.SelectedValue = strTobaccoUnits;
                                //}
                                if (strTobacco == "No")
                                {
                                    //RadioButtonTobaccoNo.Checked = true;
                                    RadioButtonListTobacco.SelectedValue = "non-smoker";
                                }
                                else
                                {
                                    RadioButtonListTobacco.SelectedValue = strTobaccoUnits;
                                }
                                lblValTobaccoUntis.Text = "";
                                #endregion

                                RadBtnQualifyClient.Enabled = true;
                                LblQualificationMessage.Text = strQualifyLife;
                                lblDisabilityMessage.Text = strQualifyDisability;
                                RadTxtRiskBand.Text = strRiskBand;
                                RadTxtDisabilityType.Text = strDisabilityType;                                

                                PanelQuotePresentation.Visible = true;

                                #region "Qualify Life"
                                if (strQualifyLife == "ok")
                                {
                                    LblOffer.Text = "We can offer you:";
                                    PanelLife.GroupingText = "Life";
                                    //PanelLife.Enabled = true;
                                    RadioButtonEscLife6.Enabled = true;
                                    RadioButtonEscLife10.Enabled = true;
                                    RadNumericTxtCoverLife.Enabled = true;
                                    RadNumericTxtPremiumLife.Enabled = true;
                                    RadNumericTxtEMLoadingLife.Enabled = true;

                                    RadioButtonQuoteLifeYes.Checked = true;                                    

                                    #region "EscalationLife"
                                    if (strEscalationLife == "6.00")
                                    {
                                        //RadioButtonEscLife6.Checked = true;
                                        RadioButtonEsc6.Checked = true;
                                        RadioButtonEsc10.Checked = false;
                                        HiddenFieldEscalationLife.Value = "6.00";
                                    }
                                    if (strEscalationLife == "10.00")
                                    {
                                        //RadioButtonEscLife10.Checked = true;
                                        RadioButtonEsc6.Checked = false;
                                        RadioButtonEsc10.Checked = true;
                                        HiddenFieldEscalationLife.Value = "10.00";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    PanelLife.GroupingText = "Life - Unavailable";
                                    //PanelLife.Enabled = false;
                                    //if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                    if ((strLifeBenifit == "FDB") && (RadComboBoxTypeBenefitLife.Items.Count() == 1))
                                    {
                                        RadioButtonEscLife6.Enabled = false;
                                        RadioButtonEscLife10.Enabled = false;
                                        RadNumericTxtCoverLife.Enabled = false;
                                        RadNumericTxtPremiumLife.Enabled = false;
                                        RadNumericTxtEMLoadingLife.Enabled = false;

                                        RadioButtonQuoteLifeNo.Checked = true;
                                        if (strQualifyDisability != "ok")
                                        {
                                            LblOffer.Text = "Unfortnatley we are unable to offer you cover at this time";
                                        }
                                    }
                                    else
                                    {
                                        RadioButtonEscLife6.Enabled = true;
                                        RadioButtonEscLife10.Enabled = true;
                                        RadNumericTxtCoverLife.Enabled = true;
                                        RadNumericTxtPremiumLife.Enabled = true;
                                        RadNumericTxtEMLoadingLife.Enabled = true;

                                        RadioButtonQuoteLifeYes.Checked = true;

                                        #region "EscalationLife"
                                        if (strEscalationLife == "6.00")
                                        {
                                            //RadioButtonEscLife6.Checked = true;
                                            RadioButtonEsc6.Checked = true;
                                            RadioButtonEsc10.Checked = false;
                                            HiddenFieldEscalationLife.Value = "6.00";
                                        }
                                        if (strEscalationLife == "10.00")
                                        {
                                            //RadioButtonEscLife10.Checked = true;
                                            RadioButtonEsc6.Checked = false;
                                            RadioButtonEsc10.Checked = true;
                                            HiddenFieldEscalationLife.Value = "10.00";
                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                #region "Qualify Disability"
                                if (strQualifyDisability == "ok")
                                {
                                    PanelDisability.GroupingText = "Disability";
                                    //PanelDisability.Enabled = true;
                                    RadioButtonTypeOfDisADW.Enabled = true;
                                    RadioButtonTypeOfDisOCC.Enabled = true;
                                    RadioButtonEscalationDis6.Enabled = true;
                                    RadioButtonEscalationDis10.Enabled = true;
                                    RadioButtonQuoteDisYes.Enabled = true;
                                    RadioButtonQuoteDisNo.Enabled = true;
                                    RadNumericTxtCoverAmnDis.Enabled = true;
                                    RadNumericTxtPremiumDis.Enabled = true;
                                    RadNumericTxtEMLoadingDisability.Enabled = true;

                                    RadioButtonQuoteDisYes.Checked = true;

                                    //#region "EscalationDisability"
                                    //if (strEscalationDisability == "6.00")
                                    //{
                                    //    RadioButtonEscalationDis6.Checked = true;
                                    //    HiddenFieldEscalationDisablility.Value = "6.00";
                                    //}
                                    //if (strEscalationDisability == "10.00")
                                    //{
                                    //    RadioButtonEscalationDis10.Checked = true;
                                    //    HiddenFieldEscalationDisablility.Value = "10.00";
                                    //}
                                    //#endregion

                                    if (strDisabilityType == "ADW")
                                    {
                                        RadioButtonTypeOfDisADW.Checked = true;
                                        RadioButtonTypeOfDisOCC.Checked = false;
                                    }
                                    if (strDisabilityType == "OCC")
                                    {
                                        RadioButtonTypeOfDisOCC.Checked = true;
                                        RadioButtonTypeOfDisADW.Checked = false;
                                    }
                                }
                                else
                                {
                                    PanelDisability.GroupingText = "Disability - Unavailable";
                                    //PanelDisability.Enabled = false;

                                    //if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                                    if ((strDisabilityBenifit == "FDB") && (RadComboBoxTypeBenefitDisability.Items.Count() == 1))
                                    {                                        
                                        RadioButtonTypeOfDisADW.Enabled = false;
                                        RadioButtonTypeOfDisOCC.Enabled = false;
                                        RadioButtonEscalationDis6.Enabled = false;
                                        RadioButtonEscalationDis10.Enabled = false;
                                        RadioButtonQuoteDisYes.Enabled = false;
                                        RadioButtonQuoteDisNo.Enabled = false;
                                        RadNumericTxtCoverAmnDis.Enabled = false;
                                        RadNumericTxtPremiumDis.Enabled = false;
                                        RadNumericTxtEMLoadingDisability.Enabled = false;

                                        RadioButtonQuoteDisNo.Checked = true;
                                        //hideOption1Disability();
                                    }
                                    else
                                    {
                                        RadioButtonTypeOfDisADW.Enabled = true;
                                        RadioButtonTypeOfDisOCC.Enabled = true;
                                        RadioButtonEscalationDis6.Enabled = true;
                                        RadioButtonEscalationDis10.Enabled = true;
                                        RadioButtonQuoteDisYes.Enabled = true;
                                        RadioButtonQuoteDisNo.Enabled = true;
                                        RadNumericTxtCoverAmnDis.Enabled = true;
                                        RadNumericTxtPremiumDis.Enabled = true;
                                        RadNumericTxtEMLoadingDisability.Enabled = true;

                                        RadioButtonQuoteDisYes.Checked = true;

                                        if (strDisabilityType == "ADW")
                                        {
                                            RadioButtonTypeOfDisADW.Checked = true;
                                            RadioButtonTypeOfDisOCC.Checked = false;
                                        }
                                        if (strDisabilityType == "OCC")
                                        {
                                            RadioButtonTypeOfDisOCC.Checked = true;
                                            RadioButtonTypeOfDisADW.Checked = false;
                                        }
                                    }
                                }
                                #endregion

                                #region "QuoteLife"
                                if (intQuoteLife == 1)
                                {
                                    RadioButtonQuoteLifeYes.Checked = true;
                                    RadioButtonQuoteLifeNo.Checked = false;
                                }
                                else
                                {
                                    RadioButtonQuoteLifeYes.Checked = false;
                                    RadioButtonQuoteLifeNo.Checked = true;
                                }
                                #endregion

                                #region "QuoteDisability"
                                if (intQuoteDisability == 1)
                                {
                                    RadioButtonQuoteDisYes.Checked = true;
                                    RadioButtonQuoteDisNo.Checked = false;
                                }
                                else
                                {
                                    RadioButtonQuoteDisYes.Checked = false;
                                    RadioButtonQuoteDisNo.Checked = true;
                                }
                                #endregion

                                #region "LifeBenifit"

                                if (strLifeBenifit != "")
                                    RadComboBoxTypeBenefitLife.FindItemByText(strLifeBenifit).Selected = true;

                                #endregion

                                #region "DisabilityBenifit"

                                if (strDisabilityBenifit != "")
                                    RadComboBoxTypeBenefitDisability.FindItemByText(strDisabilityBenifit).Selected = true;

                                #endregion

                                #region "Populate the options"
                                //make option 1, 2 and 3 visable
                                //RadBtnOption1.Visible = true;
                                RadioButtonOption1.Visible = true;
                                //RadBtnOption2.Visible = true;
                                RadioButtonOption2.Visible = true;
                                //RadBtnOption3.Visible = true;
                                RadioButtonOption3.Visible = true;
                                RadBtnOption4.Visible = true;
                                RadioButtonOption4.Visible = true;
                                RadBtnOption5.Visible = true;
                                RadioButtonOption5.Visible = true;

                                lblOr.Visible = true;
                                lblSuitable0.Visible = true;
                                lblSuitable.Visible = true;
                                lblAlso.Visible = true;
                                lblSpecificCover.Visible = true;
                                lblSpecificPremium.Visible = true;

                                #region "Get the data for the options"
                                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                                sqlConnectionX.Open();

                                sqlCommandX = new SqlCommand();
                                sqlCommandX.Connection = sqlConnectionX;
                                sqlCommandX.CommandType = CommandType.StoredProcedure;
                                sqlCommandX.CommandText = "spx_SELECT_QuoteOptionAuditTrailByAuditTrailID2";

                                sqlParam = new SqlParameter("QuoteAuditTrailID", strAuditID);
                                sqlCommandX.Parameters.Add(sqlParam);

                                sqlDR = sqlCommandX.ExecuteReader();
                                DataTable dtResult = new DataTable("Result");
                                dtResult.Load(sqlDR);

                                sqlDR.Close();
                                sqlCommandX.Cancel();
                                sqlCommandX.Dispose();
                                sqlConnectionX.Close();
                                #endregion

                                string strOptionNumber = string.Empty;
                                DataRow[] foundRowsOptionNumber = dtResult.Select("QuoteOptionAuditTrailID='" + strQuoteOptionAuditID + "'");
                                //strQuoteOptionAuditID
                                foreach (DataRow myDataRow in foundRowsOptionNumber)
                                {
                                    strOptionNumber = myDataRow["OptionNumber"].ToString();
                                }
                                DataRow[] foundRows;

                                #region "Load option 1"
                                if (strOptionNumber != "1")
                                {
                                    foundRows = dtResult.Select("OptionNumber='1'");
                                }
                                else
                                {
                                    foundRows = foundRowsOptionNumber;
                                }

                                if (foundRows.Count() > 0)
                                {
                                    if (foundRows[0]["Selected"].ToString() == "1")
                                        RadioButtonOption1.Checked = true;

                                    RadNumericTxtOption1RandValue.Visible = true;
                                    RadNumericTxtOption1RandValue.Text = foundRows[0]["CoverLife"].ToString();
                                    lblOp1_1.Visible = true;
                                    RadNumericTxtOption1Premium.Visible = true;
                                    RadNumericTxtOption1Premium.Text = foundRows[0]["PremiumLife"].ToString();
                                    lblOp1_2.Visible = true;
                                    if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                    {
                                        lblOp1_2.Text = "rands per month";
                                    }
                                    else
                                    {
                                        lblOp1_2.Text = "rands per month AND";
                                        RadNumericTxtOption1DisCover.Visible = true;
                                        RadNumericTxtOption1DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                        lblOp1_3.Visible = true;
                                        RadNumericTxtOption1DisPremium.Visible = true;
                                        RadNumericTxtOption1DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();
                                        lblOp1_4.Visible = true;
                                        RadNumericTxtOption1Total.Visible = true;
                                        RadNumericTxtOption1Total.Text = foundRows[0]["Total"].ToString();
                                        lblOp1_5.Visible = true;
                                    }

                                    ProcessOptionButton(1);
                                    SaveQuoteOptionInfo(1);
                                    populateSummaryGrid();
                                }
                                #endregion

                                #region "Load option 2"

                                if (strOptionNumber != "2")
                                {
                                    foundRows = dtResult.Select("OptionNumber='2'");
                                }
                                else
                                {
                                    foundRows = foundRowsOptionNumber;
                                }

                                if (foundRows.Count() > 0)
                                {
                                    if (foundRows[0]["Selected"].ToString() == "1")
                                        RadioButtonOption2.Checked = true;

                                    RadNumericTxtOption2RandValue.Visible = true;
                                    RadNumericTxtOption2RandValue.Text = foundRows[0]["CoverLife"].ToString();
                                    lblOp2_1.Visible = true;
                                    RadNumericTxtOption2Premium.Visible = true;
                                    RadNumericTxtOption2Premium.Text = foundRows[0]["PremiumLife"].ToString();
                                    lblOp2_2.Visible = true;
                                    if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                    {
                                        lblOp2_2.Text = "rands per month";
                                    }
                                    else
                                    {
                                        lblOp2_2.Text = "rands per month AND";
                                        RadNumericTxtOption2DisCover.Visible = true;
                                        RadNumericTxtOption2DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                        lblOp2_3.Visible = true;
                                        RadNumericTxtOption2DisPremium.Visible = true;
                                        RadNumericTxtOption2DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();
                                        lblOp2_4.Visible = true;
                                        RadNumericTxtOption2Total.Visible = true;
                                        RadNumericTxtOption2Total.Text = foundRows[0]["Total"].ToString();
                                        lblOp2_5.Visible = true;
                                    }

                                    ProcessOptionButton(2);
                                    SaveQuoteOptionInfo(2);
                                    populateSummaryGrid();
                                }
                                #endregion

                                #region "Load option 3"

                                if (strOptionNumber != "3")
                                {
                                    foundRows = dtResult.Select("OptionNumber='3'");
                                }
                                else
                                {
                                    foundRows = foundRowsOptionNumber;
                                }

                                if (foundRows.Count() > 0)
                                {
                                    if (foundRows[0]["Selected"].ToString() == "1")
                                        RadioButtonOption3.Checked = true;

                                    RadNumericTxtOption3RandValue.Visible = true;
                                    RadNumericTxtOption3RandValue.Text = foundRows[0]["CoverLife"].ToString();
                                    lblOp3_1.Visible = true;
                                    RadNumericTxtOption3Premium.Visible = true;
                                    RadNumericTxtOption3Premium.Text = foundRows[0]["PremiumLife"].ToString();
                                    lblOp3_2.Visible = true;
                                    if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                    {
                                        lblOp3_2.Text = "rands per month";
                                    }
                                    else
                                    {
                                        lblOp3_2.Text = "rands per month AND";
                                        RadNumericTxtOption3DisCover.Visible = true;
                                        RadNumericTxtOption3DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                        lblOp3_3.Visible = true;
                                        RadNumericTxtOption3DisPremium.Visible = true;
                                        RadNumericTxtOption3DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();
                                        lblOp3_4.Visible = true;
                                        RadNumericTxtOption3Total.Visible = true;
                                        RadNumericTxtOption3Total.Text = foundRows[0]["Total"].ToString();
                                        lblOp3_5.Visible = true;
                                    }

                                    ProcessOptionButton(3);
                                    SaveQuoteOptionInfo(3);
                                    populateSummaryGrid();
                                }
                                #endregion

                                if (strOptionNumber == "4")
                                {
                                    #region "Load option 4"

                                    hideOption4Life();
                                    hideOption4Disability();

                                    if (strOptionNumber != "4")
                                    {
                                        foundRows = dtResult.Select("OptionNumber='4'");
                                    }
                                    else
                                    {
                                        foundRows = foundRowsOptionNumber;
                                    }
                                    if (foundRows.Count() > 0)
                                    {
                                        if (foundRows[0]["Selected"].ToString() == "1")
                                            RadioButtonOption4.Checked = true;

                                        if (RadioButtonQuoteLifeNo.Checked == false)
                                        {
                                            RadNumericTxtOption4RandValue.Visible = true;
                                            RadNumericTxtOption4RandValue.Text = foundRows[0]["CoverLife"].ToString();
                                            RadNumericTxtCoverLife.Text = foundRows[0]["CoverLife"].ToString();
                                            lblOp4_1.Visible = true;
                                            RadNumericTxtOption4Premium.Visible = true;
                                            RadNumericTxtOption4Premium.Text = foundRows[0]["PremiumLife"].ToString();
                                            lblOp4_2.Visible = true;
                                        }

                                        //if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                        if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                                        {
                                            lblOp4_2.Text = "rands per month";
                                        }
                                        else
                                        {
                                            if (RadioButtonQuoteLifeNo.Checked == false)
                                            {
                                                lblOp4_2.Text = "rands per month AND";
                                                RadNumericTxtOption4DisCover.Visible = true;
                                                RadNumericTxtOption4DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                                RadNumericTxtCoverAmnDis.Text = foundRows[0]["CoverDisability"].ToString();
                                                lblOp4_3.Visible = true;
                                                RadNumericTxtOption4DisPremium.Visible = true;
                                                RadNumericTxtOption4DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();
                                                lblOp4_4.Visible = true;
                                                RadNumericTxtOption4Total.Visible = true;
                                                RadNumericTxtOption4Total.Text = foundRows[0]["Total"].ToString();
                                                lblOp4_5.Visible = true;
                                            }
                                            else
                                            {
                                                //only show the disability options
                                                //lblOp4_2.Text = "rands per month";
                                                RadNumericTxtOption4DisCover.Visible = true;
                                                RadNumericTxtOption4DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                                RadNumericTxtCoverAmnDis.Text = foundRows[0]["CoverDisability"].ToString();
                                                lblOp4_3.Visible = true;
                                                RadNumericTxtOption4DisPremium.Visible = true;
                                                RadNumericTxtOption4DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();
                                                //lblOp4_4.Visible = true;
                                                //lblOp4_4.Text = "rands per month";
                                                //RadNumericTxtOption4Total.Visible = true;
                                                RadNumericTxtOption4Total.Text = foundRows[0]["Total"].ToString();
                                                //lblOp4_5.Visible = true;
                                            }
                                        }

                                        ProcessOptionButton(4);
                                        SaveQuoteOptionInfo(4);
                                        populateSummaryGrid();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    hideOption4Life();
                                    hideOption4Disability();
                                    RadNumericTxtCoverLife.Text = "";

                                }

                                if (strOptionNumber == "5")
                                {
                                    #region "Load option 5"

                                    if (strOptionNumber != "5")
                                    {
                                        foundRows = dtResult.Select("OptionNumber='5'");
                                    }
                                    else
                                    {
                                        foundRows = foundRowsOptionNumber;
                                    }
                                    if (foundRows.Count() > 0)
                                    {
                                        if (foundRows[0]["Selected"].ToString() == "1")
                                            RadioButtonOption5.Checked = true;

                                        RadNumericTxtOption5RandValue.Visible = true;
                                        RadNumericTxtOption5RandValue.Text = foundRows[0]["CoverLife"].ToString();
                                        lblOp5_1.Visible = true;
                                        RadNumericTxtOption5Premium.Visible = true;
                                        RadNumericTxtOption5Premium.Text = foundRows[0]["PremiumLife"].ToString();
                                        RadNumericTxtPremiumLife.Text = foundRows[0]["PremiumLife"].ToString(); ;
                                        RadNumericTxtPremiumDis.Text = foundRows[0]["PremiumDisability"].ToString();
                                        lblOp5_2.Visible = true;
                                        if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                        {
                                            lblOp5_2.Text = "rands per month";
                                        }
                                        else
                                        {
                                            if (RadioButtonQuoteLifeNo.Checked == false)
                                            {
                                                lblOp5_2.Text = "rands per month AND";
                                                RadNumericTxtOption5DisCover.Visible = true;
                                                RadNumericTxtOption5DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                                RadNumericTxtCoverAmnDis.Text = foundRows[0]["CoverDisability"].ToString();
                                                lblOp5_3.Visible = true;
                                                RadNumericTxtOption5DisPremium.Visible = true;
                                                RadNumericTxtOption5DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();                                                
                                                lblOp5_4.Visible = true;
                                                RadNumericTxtOption5Total.Visible = true;
                                                RadNumericTxtOption5Total.Text = foundRows[0]["Total"].ToString();
                                                lblOp5_5.Visible = true;
                                            }
                                            else
                                            {
                                                lblOp5_2.Text = "rands per month AND";
                                                RadNumericTxtOption5DisCover.Visible = true;
                                                RadNumericTxtOption5DisCover.Text = foundRows[0]["CoverDisability"].ToString();
                                                lblOp5_3.Visible = true;
                                                RadNumericTxtOption5DisPremium.Visible = true;
                                                RadNumericTxtOption5DisPremium.Text = foundRows[0]["PremiumDisability"].ToString();
                                                lblOp5_4.Visible = true;
                                                RadNumericTxtOption5Total.Visible = true;
                                                RadNumericTxtOption5Total.Text = foundRows[0]["Total"].ToString();
                                                lblOp5_5.Visible = true;
                                            }
                                        }

                                        ProcessOptionButton(5);
                                        SaveQuoteOptionInfo(5);
                                        populateSummaryGrid();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    hideOption5Life();
                                    hideOption5Disability();
                                }

                                //if option is 4 or 5 then we need to make that option visable as well.
                                //strQuoteOptionAuditID


                                #endregion

                                //RadScriptManager.RegisterStartupScript(this, this.GetType(), "CheckQualifyButtonScript", "CheckQualifyButton();", true);                
                            }
                            catch (Exception ex)
                            {
                                lblInfo.Text = ex.Message;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        RadAjaxManager1.ResponseScripts.Add("radalert('Please note that you did not select a quote so nothing will be loaded',300,100,'Attention');");
                    }
                }
                #endregion
            }
        }        

        private void populateSummaryGrid()
        {
            DataTable SummaryDT = new DataTable();
            DataColumn DT1 = new DataColumn("Scenario");
            DataColumn DT2 = new DataColumn("Premium [Total]");
            DataColumn DT3 = new DataColumn("CoverAmount [Life]");
            DataColumn DT4 = new DataColumn("Premium [Life] incl. DC fee");
            DataColumn DT5 = new DataColumn("CoverAmount [Disability]");
            DataColumn DT6 = new DataColumn("Premium [Disability] incl. DC fee");

            //DT1.DataType = System.Type.GetType("System.String");
            SummaryDT.Columns.Add(DT1);
            SummaryDT.Columns.Add(DT2);
            SummaryDT.Columns.Add(DT3);
            SummaryDT.Columns.Add(DT4);
            SummaryDT.Columns.Add(DT5);
            SummaryDT.Columns.Add(DT6);

            #region "Option 1"
            if (RadNumericTxtOption1RandValue.Text != "")
            {
                DataRow row = SummaryDT.NewRow();
                row[DT1] = "Option 1";
                row[DT2] = RadNumericTxtOption1Total.Text;
                row[DT3] = RadNumericTxtOption1RandValue.Text;
                row[DT4] = RadNumericTxtOption1Premium.Text;
                row[DT5] = RadNumericTxtOption1DisCover.Text;
                row[DT6] = RadNumericTxtOption1DisPremium.Text;
                SummaryDT.Rows.Add(row);
            }
            #endregion

            #region "Option 2"
            if (RadNumericTxtOption2RandValue.Text != "")
            {
                DataRow row = SummaryDT.NewRow();
                row[DT1] = "Option 2";
                row[DT2] = RadNumericTxtOption2Total.Text;
                row[DT3] = RadNumericTxtOption2RandValue.Text;
                row[DT4] = RadNumericTxtOption2Premium.Text;
                row[DT5] = RadNumericTxtOption2DisCover.Text;
                row[DT6] = RadNumericTxtOption2DisPremium.Text;
                SummaryDT.Rows.Add(row);
            }
            #endregion

            #region "Option 3"
            if (RadNumericTxtOption2RandValue.Text != "")
            {
                DataRow row = SummaryDT.NewRow();
                row[DT1] = "Option 3";
                row[DT2] = RadNumericTxtOption3Total.Text;
                row[DT3] = RadNumericTxtOption3RandValue.Text;
                row[DT4] = RadNumericTxtOption3Premium.Text;
                row[DT5] = RadNumericTxtOption3DisCover.Text;
                row[DT6] = RadNumericTxtOption3DisPremium.Text;
                SummaryDT.Rows.Add(row);
            }
            #endregion

            #region "Option 4"
            if (RadNumericTxtOption2RandValue.Text != "")
            {
                DataRow row = SummaryDT.NewRow();
                row[DT1] = "Option 4";
                row[DT2] = RadNumericTxtOption4Total.Text;
                row[DT3] = RadNumericTxtOption4RandValue.Text;
                row[DT4] = RadNumericTxtOption4Premium.Text;
                row[DT5] = RadNumericTxtOption4DisCover.Text;
                row[DT6] = RadNumericTxtOption4DisPremium.Text;
                SummaryDT.Rows.Add(row);
            }
            #endregion

            #region "Option 5"
            if (RadNumericTxtOption2RandValue.Text != "")
            {
                DataRow row = SummaryDT.NewRow();
                row[DT1] = "Option 5";
                row[DT2] = RadNumericTxtOption5Total.Text;
                row[DT3] = RadNumericTxtOption5RandValue.Text;
                row[DT4] = RadNumericTxtOption5Premium.Text;
                row[DT5] = RadNumericTxtOption5DisCover.Text;
                row[DT6] = RadNumericTxtOption5DisPremium.Text;
                SummaryDT.Rows.Add(row);
            }
            #endregion

            RadGridSummary.DataSource = SummaryDT;
            RadGridSummary.DataBind();
            RadGridSummary.Rebind();
        }           

        private void hideOption1Life()
        {
            RadNumericTxtOption1RandValue.Visible = false;
            RadNumericTxtOption1RandValue.Text = "";
            lblOp1_1.Visible = false;
            RadNumericTxtOption1Premium.Visible = false;
            RadNumericTxtOption1Premium.Text = "";
            lblOp1_2.Visible = false;
        }

        private void hideOption2Life()
        {
            RadNumericTxtOption2RandValue.Visible = false;
            RadNumericTxtOption2RandValue.Text = "";
            lblOp2_1.Visible = false;
            RadNumericTxtOption2Premium.Visible = false;
            RadNumericTxtOption2Premium.Text = "";
            lblOp2_2.Visible = false;
        }

        private void hideOption3Life()
        {
            RadNumericTxtOption3RandValue.Visible = false;
            RadNumericTxtOption3RandValue.Text = "";
            lblOp3_1.Visible = false;
            RadNumericTxtOption3Premium.Visible = false;
            RadNumericTxtOption3Premium.Text = "";
            lblOp3_2.Visible = false;
        }

        private void hideOption4Life()
        {
            RadNumericTxtOption4RandValue.Visible = false;
            RadNumericTxtOption4RandValue.Text = "";
            lblOp4_1.Visible = false;
            RadNumericTxtOption4Premium.Visible = false;
            RadNumericTxtOption4Premium.Text = "";
            lblOp4_2.Visible = false;
        }

        private void hideOption5Life()
        {
            RadNumericTxtOption5RandValue.Visible = false;
            RadNumericTxtOption5RandValue.Text = "";
            lblOp5_1.Visible = false;
            RadNumericTxtOption5Premium.Visible = false;
            RadNumericTxtOption5Premium.Text = "";
            lblOp5_2.Visible = false;
        }

        private void hideOption1Disability()
        {
            RadNumericTxtOption1DisCover.Visible = false;
            RadNumericTxtOption1DisCover.Text = "";
            lblOp1_3.Visible = false;
            RadNumericTxtOption1DisPremium.Visible = false;
            RadNumericTxtOption1DisPremium.Text = "";
            lblOp1_4.Visible = false;
            RadNumericTxtOption1Total.Visible = false;
            //RadNumericTxtOption1Total.Text = "";
            lblOp1_5.Visible = false;
        }

        private void hideOption2Disability()
        {
            RadNumericTxtOption2DisCover.Visible = false;
            RadNumericTxtOption2DisCover.Text = "";
            lblOp2_3.Visible = false;
            RadNumericTxtOption2DisPremium.Visible = false;
            RadNumericTxtOption2DisPremium.Text = "";
            lblOp2_4.Visible = false;
            RadNumericTxtOption2Total.Visible = false;
           // RadNumericTxtOption2Total.Text = "";
            lblOp2_5.Visible = false;
        }

        private void hideOption3Disability()
        {
            RadNumericTxtOption3DisCover.Visible = false;
            RadNumericTxtOption3DisCover.Text = "";
            lblOp3_3.Visible = false;
            RadNumericTxtOption3DisPremium.Visible = false;
            RadNumericTxtOption3DisPremium.Text = "";
            lblOp3_4.Visible = false;
            RadNumericTxtOption3Total.Visible = false;
           // RadNumericTxtOption3Total.Text = "";
            lblOp3_5.Visible = false;
        }

        private void hideOption4Disability()
        {
            RadNumericTxtOption4DisCover.Visible = false;
            RadNumericTxtOption4DisCover.Text = "";
            lblOp4_3.Visible = false;
            RadNumericTxtOption4DisPremium.Visible = false;
            RadNumericTxtOption4DisPremium.Text = "";
            lblOp4_4.Visible = false;
            RadNumericTxtOption4Total.Visible = false;
            //RadNumericTxtOption4Total.Text = "";
            lblOp4_5.Visible = false;
        }

        private void hideOption5Disability()
        {
            RadNumericTxtOption5DisCover.Visible = false;
            RadNumericTxtOption5DisCover.Text = "";
            lblOp5_3.Visible = false;
            RadNumericTxtOption5DisPremium.Visible = false;
            RadNumericTxtOption5DisPremium.Text = "";
            lblOp5_4.Visible = false;
            RadNumericTxtOption5Total.Visible = false;
            //RadNumericTxtOption5Total.Text = "";
            lblOp5_5.Visible = false;
        }

        protected void RadBtnOption1_Click(object sender, EventArgs e)
        {
            ProcessOptionButton(1);
            SaveQuoteOptionInfo(1);
            populateSummaryGrid();

            #region "old code"
            /*
            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlParameter sqlParam;
            SqlDataReader sqlDR;

            try
            {
                //Get the income amount
                decimal decIncome = Convert.ToDecimal(RadNumericTxtIncome.Text.Trim());
                //Get the percentage from the setting table for Option 1
                decimal decPI = 0;
                decimal decRate = 0; decimal decRateLife = 0; decimal decRateDisability = 0;
                string strSubscriberName = string.Empty; string strSubscriberPassword = string.Empty; string strSubscriberCode = string.Empty;
                string strProduct = string.Empty; string strBaseRisk = string.Empty; string strRiskModifier = string.Empty; string strGender = string.Empty;
                Decimal decCover = 0; Decimal decPremium = 0;
                string strDiabetesType = string.Empty;
                string strAgeBracket = string.Empty; string strControlLevel = string.Empty; string strDiabetesDuration = string.Empty;
                string strCholesterolIndicator = string.Empty; string strBPIndicator = string.Empty; string strSmokerLevel = string.Empty;
                string strDateDiagnosed = string.Empty;string strQuoteDate = string.Empty;

                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                #region "Get Option1PI"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "Option1PI");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    decPI = Convert.ToDecimal(sqlDR.GetValue(0));
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberName"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "SubscriberName");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSubscriberName = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberPassword"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "SubscriberPassword");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSubscriberPassword = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberCode"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "SubscriberCode");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSubscriberCode = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get Default Diabetes Product"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "Default Diabetes Product");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strProduct = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion                

                if (RadioButtonMale.Checked)
                    strGender = "M";
                if (RadioButtonFemale.Checked)
                    strGender = "F";
                if (RadioButtonOther.Checked)
                    strGender = "M";

                //Compile the baserisk
                //[ANB][Gender][Smoker Status][Class of Life][Benefit Code]
                // Smoker status is always NS AND Benifit Code always WL - From meeting on 2015-06-9

                strBaseRisk = RadTxtAgeOfNextBirthday.Text.Trim() + strGender + "NS" + HiddenFieldClassOfLife.Value + "WL";

                //Compile the risk modifier
                //[Diabetes Type][Age Bracket at Quote][Control Level][Diabetes Duration][Cholesterol Indicator][BP Indicator][Smoker Level]
                

                if (RadioButtonDiabetesType1.Checked)
                    strDiabetesType = "T1";
                if (RadioButtonDiabetesType2.Checked)
                    strDiabetesType = "T2";
                if (RadioButtonDiabetesTypeNotSure.Checked)
                    strDiabetesType = "T2";

                #region "AgeBracket"
                if (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 30)
                {
		             strAgeBracket = "A1";   
	            }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 30) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 40))
                {
		             strAgeBracket = "A2";   
	            }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 40) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 50))
                {
		             strAgeBracket = "A3";   
	            }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 50) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 60))
                {
		             strAgeBracket = "A4";   
	            }
                
                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 60) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 70))
                {
		             strAgeBracket = "A5";   
	            }

                if (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 70)
                {
		             strAgeBracket = "A6";
                }
                #endregion

                #region "Control Level"
                //C1	<=7
                //C2	>7<=8
                //C3	>8<=9
                //C4	>9<=10
                //C5	>10<=11
                //C6	>11<=12
                //C7	>12<=14
                if ((RadioButtonHbA1c4.Checked == true) || (RadioButtonHbA1c6.Checked == true))
                    strControlLevel = "C1";
                if (RadioButtonHbA1c7.Checked == true)
                    strControlLevel = "C2";
                if (RadioButtonHbA1c8.Checked == true)
                    strControlLevel = "C3";
                if (RadioButtonHbA1c9.Checked == true)
                    strControlLevel = "C4";
                if (RadioButtonHbA1c10.Checked == true)
                    strControlLevel = "C5";
                if (RadioButtonHbA1c11.Checked == true)
                    strControlLevel = "C6";
                if (RadioButtonHbA1c12.Checked == true)
                    strControlLevel = "C7";

                if (RadioButtonHbA1cUnknown.Checked == true)
                {
                        //C3	1
                        //C4	2
                        //C5	3
                        //C6	4
                }

                
                #endregion

                #region "Diabetes Duration"
                
                    //Duration = CASE 
                    //WHEN DATEPART(year,Q.dtFieldValue) – D.iFieldValue < 5 THEN '<5'
                    //WHEN DATEPART(year,Q.dtFieldValue) – D.iFieldValue < 15 THEN '5_15'
                    //ELSE '>15'
                    //END

                    //<5	    Less than 5 years
                    //5_15	More than 5 but less than 15 years
                    //>15	    More than 15 years
                
                strDateDiagnosed = RadDatePickerDateOfDiag.SelectedDate.ToString();
                strQuoteDate = RadDatePickerQuoteDate.SelectedDate.ToString();

                DateTime DTDateDiagnosed = Convert.ToDateTime(strDateDiagnosed);
                DateTime DTQuoteDate = Convert.ToDateTime(strQuoteDate);

                int YearsPassed = DTQuoteDate.Year - DTDateDiagnosed.Year;
                if (YearsPassed < 5)
                    strDiabetesDuration = "<5";
                if ((YearsPassed >= 5) && (YearsPassed < 15))
                    strDiabetesDuration = "5_15";
                if (YearsPassed >= 15)
                    strDiabetesDuration = ">15";

                #endregion

                #region "Cholesterol Indicator"
                 
                    //High	Yes
                    //Norm	No
                    //Norm	Unsure
                 

                if (RadioButtonCholesterolYes.Checked == true)
                    strCholesterolIndicator = "High";
                if (RadioButtonCholesterolNo.Checked == true)
                    strCholesterolIndicator = "Norm";
                if (RadioButtonCholesterolNotSure.Checked == true)
                    strCholesterolIndicator = "Norm";

                #endregion

                #region "BP Indicator"
                   
                    //High	Yes
                    //Norm	No
                    //Norm	Unsure
                 

                if (RadioButtonHighBPYes.Checked == true)
                    strBPIndicator = "High";
                if (RadioButtonHighBPNo.Checked == true)
                    strBPIndicator = "Norm";
                if (RadioButtonHighBP.Checked == true)
                    strBPIndicator = "Norm";

                #endregion

                #region "Smoker Level"
                
                    //S0	Non Smoker (P_M2_Q12 = False or P_TOBACCOFREQ = 0)
                    //S1	<=2
                    //S2	>2;<=5
                    //S3	>5;<=20
                    //S4	>20
                 

                if (RadioButtonTobaccoNo.Checked == true)
                    strSmokerLevel = "S0";
                if (RadioButtonListTobacco.SelectedValue == "< 1")
                    strSmokerLevel = "S1";
                if (RadioButtonListTobacco.SelectedValue == "1 - 5")
                    strSmokerLevel = "S2";
                if (RadioButtonListTobacco.SelectedValue == "6 - 20")
                    strSmokerLevel = "S3";
                if (RadioButtonListTobacco.SelectedValue == "> 20")
                    strSmokerLevel = "S4";

                #endregion

                strRiskModifier = strDiabetesType + strAgeBracket + strControlLevel + strDiabetesDuration + strCholesterolIndicator + strBPIndicator + strSmokerLevel;

                //Calc Premium
                //e.g. Income * 6%
                decRate = (decIncome * decPI) / 100;
                //Split the Rate 50%-%0%
                decRateLife = decRate / 2;
                decRateDisability = decRate / 2;

                //Life
                //****************************************
                #region "Life"
                //Get the Cover
                WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
                DataTable ReturnDT;
                ReturnDT = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");

                foreach (DataRow row in ReturnDT.Rows)
                {
                    decCover = Convert.ToDecimal(row["Cover"].ToString());
                }

                //Match closest cover amount in the CoverRounding table
                #region "Match closest cover amount in the CoverRounding table"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                sqlParam = new SqlParameter("CoverAmount", decCover);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                sqlConnectionX.Close();

                //Get the premium
                ReturnDT = null;
                ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                foreach (DataRow row in ReturnDT.Rows)
                {
                    decPremium = Convert.ToDecimal(row["StandardPremium"].ToString());
                }
                #endregion

                if (PanelLife.GroupingText.Contains("Unavailable") == false)
                {
                    RadNumericTxtOption1RandValue.Visible = true;
                    RadNumericTxtOption1RandValue.Text = decCover.ToString();
                    lblOp1_1.Visible = true;
                    RadNumericTxtOption1Premium.Visible = true;
                    RadNumericTxtOption1Premium.Text = decPremium.ToString();
                    lblOp1_2.Visible = true;
                    if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                    {
                        lblOp1_2.Text = "rands per month";
                    }
                    else
                    {
                        lblOp1_2.Text = "rands per month AND";
                        decimal decTotal = ((decPremium + decPremium)) - (Convert.ToDecimal(1*0.01));  
                        RadNumericTxtOption1Total.Text = decTotal.ToString();
                    }
                }
                else
                {
                    hideOption1Life();
                }
                if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                {
                    RadNumericTxtOption1DisCover.Visible = true;
                    RadNumericTxtOption1DisCover.Text = decCover.ToString();
                    lblOp1_3.Visible = true;
                    RadNumericTxtOption1DisPremium.Visible = true;
                    RadNumericTxtOption1DisPremium.Text = decPremium.ToString();
                    lblOp1_4.Visible = true;
                    RadNumericTxtOption1Total.Visible = true;
                    lblOp1_5.Visible = true;
                }
                else
                {
                    hideOption1Disability();
                }

            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }    
            */
            #endregion
        }     

        protected void RadBtnOption2_Click(object sender, EventArgs e)
        {
            ProcessOptionButton(2);
            SaveQuoteOptionInfo(2);
            lblSuitable.Visible = true;
            lblSuitable0.Visible = true;
            lblAlso.Visible = true;            
            populateSummaryGrid();
        }

        protected void RadBtnOption3_Click(object sender, EventArgs e)
        {
            ProcessOptionButton(3);
            SaveQuoteOptionInfo(3);
            lblSpecificCover.Visible = true;
            lblSpecificPremium.Visible = true;
            populateSummaryGrid();
        }

        private string GetBaseRisk(int LifeOrDisability)  //1 = life; 2 = disability
        {
            string strReturn = string.Empty;
            string strGender = string.Empty;
            decimal decIncome = Convert.ToDecimal(RadNumericTxtIncome.Text.Trim());
            if (RadNumericTxtSpouseIncome.Text.Trim() == "")
            {
                RadNumericTxtSpouseIncome.Text = "0";
            }

            decimal decSpouseIncome = Convert.ToDecimal(RadNumericTxtSpouseIncome.Text.Trim());
            decIncome = decIncome + decSpouseIncome;
            //Get the percentage from the setting table for Option 1
            decimal decPI = 0;
            decimal decRate = 0; decimal decRateLife = 0; decimal decRateDisability = 0;
            string strQualification = string.Empty; int intQualification = 0;
            string strSpouseQualification = string.Empty; int intSpouseQualification = 0;
            int intClass = 0;
            string strBenefitCode = string.Empty;

            if (RadioButtonMale.Checked)
                strGender = "M";
            if (RadioButtonFemale.Checked)
                strGender = "F";
            if (RadioButtonOther.Checked)
                strGender = "M";

            //Compile the baserisk
            //[ANB][Gender][Smoker Status][Class of Life][Benefit Code]
            // Smoker status is always NS AND Benifit Code always WL - From meeting on 2015-06-9

            #region "If no class of life"
            //if (HiddenFieldClassOfLife.Value == "0")
            //{
                if (RadioButtonNotMat.Checked)
                {
                    strQualification = "No matric";
                    intQualification = 1;
                }
                if (RadioButtonMatriculated.Checked)
                {
                    strQualification = "Matric";
                    intQualification = 2;
                }
                if (RadioButtonDiploma.Checked)
                {
                    strQualification = "3 or 4 yr. Diploma/3 yr. Degree";
                    intQualification = 3;
                }
                if (RadioButtonDegree.Checked)
                { 
                    strQualification = "4 yr. Degree/professional qualification";
                    intQualification = 4;
                }

                //Spouse education
                if (RadioButtonSNotMat.Checked)
                {
                    strSpouseQualification = "No matric";
                    intSpouseQualification = 1;
                }
                if (RadioButtonSMat.Checked)
                {
                    strSpouseQualification = "Matric";
                    intSpouseQualification = 2;
                }
                if (RadioButtonSDip.Checked)
                {
                    strSpouseQualification = "3 or 4 yr. Diploma/3 yr. Degree";
                    intSpouseQualification = 3;
                }
                if (RadioButtonSDegree.Checked)
                { 
                    strSpouseQualification = "4 yr. Degree/professional qualification";
                    intSpouseQualification = 4;
                }

                if (intSpouseQualification > intQualification)
                {
                    strQualification = strSpouseQualification;
                }


                if ((decIncome >= 0) && (decIncome < 10499))
                {
                    switch (strQualification)
                    {
                        case "No matric":
                            intClass = 4;
                            break;
                        case "Matric":
                            intClass = 4;
                            break;
                        case "3 or 4 yr. Diploma/3 yr. Degree":
                            intClass = 3;
                            break;
                        case "4 yr. Degree/professional qualification":
                            intClass = 1;
                            break;
                    }
                }

                if ((decIncome >= 10500) && (decIncome < 15749))
                {
                    switch (strQualification)
                    {
                        case "No matric":
                            intClass = 4;
                            break;
                        case "Matric":
                            intClass = 3;
                            break;
                        case "3 or 4 yr. Diploma/3 yr. Degree":
                            intClass = 2;
                            break;
                        case "4 yr. Degree/professional qualification":
                            intClass = 1;
                            break;
                    }
                }

                if ((decIncome >= 15750) && (decIncome < 26249))
                {
                    switch (strQualification)
                    {
                        case "No matric":
                            intClass = 3;
                            break;
                        case "Matric":
                            intClass = 2;
                            break;
                        case "3 or 4 yr. Diploma/3 yr. Degree":
                            intClass = 1;
                            break;
                        case "4 yr. Degree/professional qualification":
                            intClass = 1;
                            break;
                    }
                }

                if ((decIncome >= 26250) && (decIncome < 41999))
                {
                    switch (strQualification)
                    {
                        case "No matric":
                            intClass = 2;
                            break;
                        case "Matric":
                            intClass = 2;
                            break;
                        case "3 or 4 yr. Diploma/3 yr. Degree":
                            intClass = 1;
                            break;
                        case "4 yr. Degree/professional qualification":
                            intClass = 1;
                            break;
                    }
                }

                if (decIncome >= 42000)
                {
                    switch (strQualification)
                    {
                        case "No matric":
                            intClass = 2;
                            break;
                        case "Matric":
                            intClass = 1;
                            break;
                        case "3 or 4 yr. Diploma/3 yr. Degree":
                            intClass = 1;
                            break;
                        case "4 yr. Degree/professional qualification":
                            intClass = 1;
                            break;
                    }
                }

                HiddenFieldClassOfLife.Value = intClass.ToString();
            //}

            #endregion


            //switch (RadComboBoxTypeBenefitLife.SelectedItem.Text.ToString().Trim())
            //{
            //    case "FDB":
            //        if (LifeOrDisability == 1)
            //            strBenefitCode = "WL";
            //        else // disability
            //        {
            //            if (RadioButtonTypeOfDisOCC.Checked)
            //                strBenefitCode = "OCC";
            //            else
            //                strBenefitCode = "ADW";
            //        }
            //        break;
            //    case "ADB":
            //        if (LifeOrDisability == 1)
            //            strBenefitCode = "ABWL";
            //        else // disability
            //        {
            //            if (RadioButtonTypeOfDisOCC.Checked)
            //                strBenefitCode = "ABOCC";
            //            else
            //                strBenefitCode = "ABADW";
            //        }
            //        break;
            //    case "ADCB":
            //        if (LifeOrDisability == 1)
            //            strBenefitCode = "ACBWL";
            //        else // disability
            //        {
            //            if (RadioButtonTypeOfDisOCC.Checked)
            //                strBenefitCode = "ACBOCC";
            //            else
            //                strBenefitCode = "ACBADW";
            //        }
            //        break;
            //}            

            strReturn = RadTxtAgeOfNextBirthday.Text.Trim() + strGender + "NS" + HiddenFieldClassOfLife.Value + strBenefitCode;

            if (LifeOrDisability == 1)   //1 = life; 2 = disability
            {
                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                {
                    strReturn = RadTxtAgeOfNextBirthday.Text.Trim() + strGender;
                }

                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                {
                    //strReturn = RadTxtAgeOfNextBirthday.Text.Trim() + strGender + "NS" + HiddenFieldClassOfLife.Value;
                    strReturn = RadTxtAgeOfNextBirthday.Text.Trim() + strGender;
                }
            }

            if (LifeOrDisability == 2)   //1 = life; 2 = disability
            {
                if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "ADB")
                {
                    //strReturn = RadTxtAgeOfNextBirthday.Text.Trim() + strGender;
                }

                if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "ACDB")
                {
                    strReturn = RadTxtAgeOfNextBirthday.Text.Trim() + strGender + "NS" + HiddenFieldClassOfLife.Value;
                }
            }

            return strReturn;
        }

        private string GetBenifitCode(int LifeOrDisability)  //1 = life; 2 = disability
        {
            string strProductType = string.Empty;

            if (LifeOrDisability == 1)
            {
                switch (RadComboBoxTypeBenefitLife.SelectedItem.Text.ToString().Trim())
                {
                    case "FDB":
                        if (LifeOrDisability == 1)  //1 = life; 2 = disability
                            strProductType = "WL";
                        else // disability
                        {
                            if (RadioButtonTypeOfDisOCC.Checked)
                                strProductType = "OCC";
                            else
                                strProductType = "ADW";
                        }
                        break;
                    case "ADB":
                        if (LifeOrDisability == 1)
                            strProductType = "ABWL";
                        else // disability
                        {
                            if (RadioButtonTypeOfDisOCC.Checked)
                                strProductType = "ABOCC";
                            else
                                strProductType = "ABADW";
                        }
                        break;
                    case "ACDB":
                        if (LifeOrDisability == 1)
                            strProductType = "ACBWL";
                        else // disability
                        {
                            if (RadioButtonTypeOfDisOCC.Checked)
                                strProductType = "ACBOCC";
                            else
                                strProductType = "ACBADW";
                        }
                        break;
                }
            }
            else
            {
                switch (RadComboBoxTypeBenefitDisability.SelectedItem.Text.ToString().Trim())
                {
                    case "FDB":
                        if (LifeOrDisability == 1)  //1 = life; 2 = disability
                            strProductType = "WL";
                        else // disability
                        {
                            if (RadioButtonTypeOfDisOCC.Checked)
                                strProductType = "OCC";
                            else
                                strProductType = "ADW";
                        }
                        break;
                    case "ADB":
                        if (LifeOrDisability == 1)
                            strProductType = "ABWL";
                        else // disability
                        {
                            if (RadioButtonTypeOfDisOCC.Checked)
                                strProductType = "ABOCC";
                            else
                                strProductType = "ABADW";
                        }
                        break;
                    case "ACDB":
                        if (LifeOrDisability == 1)
                            strProductType = "ACBWL";
                        else // disability
                        {
                            if (RadioButtonTypeOfDisOCC.Checked)
                                strProductType = "ACBOCC";
                            else
                                strProductType = "ACBADW";
                        }
                        break;
                }
            }

            return strProductType;
        }
        
        private string GetRiskModifier()
        {
            string strRiskModifier = string.Empty;
            string strDiabetesType = string.Empty;
            string strAgeBracket = string.Empty; string strControlLevel = string.Empty; string strDiabetesDuration = string.Empty;
            string strCholesterolIndicator = string.Empty; string strBPIndicator = string.Empty; string strSmokerLevel = string.Empty;
            string strDateDiagnosed = string.Empty; string strQuoteDate = string.Empty;
            string strOptionPISetting = string.Empty; string strControlFee = string.Empty;
            string strQualification = string.Empty; int intClass = 0;
            string strEscalation = string.Empty;
            string strDiabecticControl = string.Empty;

            #region "Diabetes Type"
            if (RadioButtonDiabetesType1.Checked)
                strDiabetesType = "T1";
            if (RadioButtonDiabetesType2.Checked)
                strDiabetesType = "T2";
            if (RadioButtonDiabetesTypeNotSure.Checked)
                strDiabetesType = "T2";

            //if insulin then T1 else T2
            if (RadioButtonInsulinYes.Checked)
                strDiabetesType = "T1";
            #endregion

            #region "AgeBracket"
            if (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 30)
            {
                strAgeBracket = "A1";
            }

            if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 30) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 40))
            {
                strAgeBracket = "A2";
            }

            if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 40) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 50))
            {
                strAgeBracket = "A3";
            }

            if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 50) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 60))
            {
                strAgeBracket = "A4";
            }

            if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 60) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 70))
            {
                strAgeBracket = "A5";
            }

            if (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 70)
            {
                strAgeBracket = "A6";
            }
            #endregion

            #region "Control Level"
            //C1	<=7
            //C2	>7<=8
            //C3	>8<=9
            //C4	>9<=10
            //C5	>10<=11
            //C6	>11<=12
            //C7	>12<=14
            if ((RadioButtonHbA1c3.Checked == true) || (RadioButtonHbA1c4.Checked == true) || (RadioButtonHbA1c6.Checked == true))
                strControlLevel = "C1";
            if (RadioButtonHbA1c7.Checked == true)
                strControlLevel = "C2";
            if (RadioButtonHbA1c8.Checked == true)
                strControlLevel = "C3";
            if (RadioButtonHbA1c9.Checked == true)
                strControlLevel = "C4";
            if (RadioButtonHbA1c10.Checked == true)
                strControlLevel = "C5";
            if (RadioButtonHbA1c11.Checked == true)
                strControlLevel = "C6";
            if (RadioButtonHbA1c12.Checked == true)
                strControlLevel = "C7";

            if (RadioButtonHbA1cUnknown.Checked == true)
            {
                //RadioButtonDiabetControlExcellent
                //RadioButtonDiabetControlGood
                //RadioButtonDiabetControlMod
                //RadioButtonDiabetControlPoor

                if (RadioButtonDiabetControlExcellent.Checked == true)
                    strDiabecticControl = "1";
                if (RadioButtonDiabetControlGood.Checked == true)
                    strDiabecticControl = "2";
                if (RadioButtonDiabetControlMod.Checked == true)
                    strDiabecticControl = "3";
                if (RadioButtonDiabetControlPoor.Checked == true)
                    strDiabecticControl = "4";

                switch (strDiabecticControl)
                {
                    case "1":
                        strControlLevel = "C3";
                        break;
                    case "2":
                        strControlLevel = "C4";
                        break;
                    case "3":
                        strControlLevel = "C5";
                        break;
                    case "4":
                        strControlLevel = "C6";
                        break;

                }
                //C3	1
                //C4	2
                //C5	3
                //C6	4
            }


            #endregion

            #region "Diabetes Duration"
            /*
                    Duration = CASE 
                    WHEN DATEPART(year,Q.dtFieldValue) – D.iFieldValue < 5 THEN '<5'
                    WHEN DATEPART(year,Q.dtFieldValue) – D.iFieldValue < 15 THEN '5_15'
                    ELSE '>15'
                    END

                    <5	    Less than 5 years
                    5_15	More than 5 but less than 15 years
                    >15	    More than 15 years
                */
            //strDateDiagnosed = RadDatePickerDateOfDiag.SelectedDate.ToString();
            strDateDiagnosed = RadMonthYearPickerDateOfDiag.SelectedDate.ToString();
            strQuoteDate = RadDatePickerQuoteDate.SelectedDate.ToString();

            DateTime DTDateDiagnosed = Convert.ToDateTime(strDateDiagnosed);
            DateTime DTQuoteDate = Convert.ToDateTime(strQuoteDate);

            int YearsPassed = DTQuoteDate.Year - DTDateDiagnosed.Year;
            if (YearsPassed < 5)
                strDiabetesDuration = "<5";
            if ((YearsPassed >= 5) && (YearsPassed < 15))
                strDiabetesDuration = "5_15";
            if (YearsPassed >= 15)
                strDiabetesDuration = ">15";

            #endregion

            #region "Cholesterol Indicator"
            /*    
                    High	Yes
                    Norm	No
                    Norm	Unsure
                 */

            if (RadioButtonCholesterolYes.Checked == true)
                strCholesterolIndicator = "High";
            if (RadioButtonCholesterolNo.Checked == true)
                strCholesterolIndicator = "Norm";
            if (RadioButtonCholesterolNotSure.Checked == true)
                strCholesterolIndicator = "Norm";

            #endregion

            #region "BP Indicator"
            /*    
                    High	Yes
                    Norm	No
                    Norm	Unsure
                 */

            if (RadioButtonHighBPYes.Checked == true)
                strBPIndicator = "High";
            if (RadioButtonHighBPNo.Checked == true)
                strBPIndicator = "Norm";
            if (RadioButtonHighBP.Checked == true)
                strBPIndicator = "Norm";

            #endregion

            #region "Smoker Level"
            /*    
                    S0	Non Smoker (P_M2_Q12 = False or P_TOBACCOFREQ = 0)
                    S1	<=2
                    S2	>2;<=5
                    S3	>5;<=20
                    S4	>20
                 */

            if (RadioButtonListTobacco.SelectedValue == "non-smoker")
                strSmokerLevel = "S0";
            if (RadioButtonListTobacco.SelectedValue == "< 1")
                strSmokerLevel = "S1";
            if (RadioButtonListTobacco.SelectedValue == "1 - 5")
                strSmokerLevel = "S2";
            if (RadioButtonListTobacco.SelectedValue == "6 - 20")
                strSmokerLevel = "S3";
            if (RadioButtonListTobacco.SelectedValue == "> 20")
                strSmokerLevel = "S4";

            #endregion

            ////#region "Escalation"
            ////if (RadioButtonEscLife6.Checked == true)
            ////    strEscalation = "06";
            ////if (RadioButtonEscLife10.Checked == true)
            ////    strEscalation = "10";

            ////#endregion

            strRiskModifier = strDiabetesType + strAgeBracket + strControlLevel + strDiabetesDuration + strCholesterolIndicator + strBPIndicator + strSmokerLevel + strEscalation;

            return strRiskModifier;
        }

        private void ProcessOptionButton(int buttonNumber)
        {
            SqlConnection sqlConnectionX;
            SqlCommand sqlCommandX;
            SqlCommand sqlCommandX2;
            SqlParameter sqlParam;
            SqlDataReader sqlDR; 
            SqlDataReader sqlDR2;

            try
            {
                //Get the income amount
                decimal decIncome = Convert.ToDecimal(RadNumericTxtIncome.Text.Trim());
                //Get the percentage from the setting table for Option 1
                decimal decPI = 0;
                decimal decRate = 0; decimal decRateLife = 0; decimal decRateDisability = 0;
                string strSubscriberName = string.Empty; string strSubscriberPassword = string.Empty; string strSubscriberCode = string.Empty;
                string strProduct = string.Empty; string strBaseRisk = string.Empty; string strRiskModifier = string.Empty; string strGender = string.Empty;
                Decimal decCover = 0; Decimal decPremium = 0; Decimal decPremiumUnrounded = 0; Decimal decPremiumFixedFee = 0;
                Decimal decCoverDisability = 0; Decimal decPremiumDisability = 0; Decimal decPremiumDisabilityUnrounded = 0;
                string strDiabetesType = string.Empty;
                string strAgeBracket = string.Empty; string strControlLevel = string.Empty; string strDiabetesDuration = string.Empty;
                string strCholesterolIndicator = string.Empty; string strBPIndicator = string.Empty; string strSmokerLevel = string.Empty;
                string strDateDiagnosed = string.Empty; string strQuoteDate = string.Empty;
                string strOptionPISetting = string.Empty; string strControlFee = string.Empty;
                string strQualification = string.Empty; int intClass = 0;
                //decimal decDesiredContribution = 0;
                string strRiskband = string.Empty;
                string strProductType = string.Empty; string strEscalation = string.Empty;
                int LifeOrDisability = 1;
                Decimal decCPRateLife = 0; Decimal decCPRateDisability = 0;

                strOptionPISetting = "Option" + buttonNumber.ToString() + "PI";

                sqlConnectionX = new SqlConnection(ConfigurationManager.AppSettings["SQLConnection"]);
                sqlConnectionX.Open();

                if ((buttonNumber == 1) || (buttonNumber == 2) || (buttonNumber == 3))
                {
                    #region "Get Option PI"
                    sqlCommandX = new SqlCommand();
                    sqlCommandX.Connection = sqlConnectionX;
                    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    sqlCommandX.CommandText = "spx_SELECT_Setting";

                    sqlParam = new SqlParameter("SettingName", strOptionPISetting);
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    while (sqlDR.Read())
                    {
                        decPI = Convert.ToDecimal(sqlDR.GetValue(0));
                    }

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();
                    #endregion
                }

                #region "Get SubscriberName"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "SubscriberName");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSubscriberName = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberPassword"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "SubscriberPassword");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSubscriberPassword = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get SubscriberCode"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "SubscriberCode");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strSubscriberCode = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                #region "Get Default Diabetes Product"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_SELECT_Setting";

                sqlParam = new SqlParameter("SettingName", "Default Diabetes Product");
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                while (sqlDR.Read())
                {
                    strProduct = sqlDR.GetValue(0).ToString();
                }

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                if (RadioButtonMale.Checked)
                    strGender = "M";
                if (RadioButtonFemale.Checked)
                    strGender = "F";
                if (RadioButtonOther.Checked)
                    strGender = "M";

                //Compile the baserisk
                //[ANB][Gender][Smoker Status][Class of Life][Benefit Code]
                // Smoker status is always NS AND Benifit Code always WL - From meeting on 2015-06-9

                #region "If no class of life"
                if (HiddenFieldClassOfLife.Value == "0")
                {
                    if (RadioButtonNotMat.Checked)
                        strQualification = "No matric";
                    if (RadioButtonMatriculated.Checked)
                        strQualification = "Matric";
                    if (RadioButtonDiploma.Checked)
                        strQualification = "3 or 4 yr. Diploma/3 yr. Degree";
                    if (RadioButtonDegree.Checked)
                        strQualification = "4 yr. Degree/professional qualification";

                    if ((decIncome >= 0) && (decIncome < 10499))
                    {
                        switch (strQualification)
                        {
                            case "No matric":
                                intClass = 4;
                                break;
                            case "Matric":
                                intClass = 4;
                                break;
                            case "3 or 4 yr. Diploma/3 yr. Degree":
                                intClass = 3;
                                break;
                            case "4 yr. Degree/professional qualification":
                                intClass = 1;
                                break;
                        }
                    }

                    if ((decIncome >= 10500) && (decIncome < 15749))
                    {
                        switch (strQualification)
                        {
                            case "No matric":
                                intClass = 4;
                                break;
                            case "Matric":
                                intClass = 3;
                                break;
                            case "3 or 4 yr. Diploma/3 yr. Degree":
                                intClass = 2;
                                break;
                            case "4 yr. Degree/professional qualification":
                                intClass = 1;
                                break;
                        }
                    }

                    if ((decIncome >= 15750) && (decIncome < 26249))
                    {
                        switch (strQualification)
                        {
                            case "No matric":
                                intClass = 3;
                                break;
                            case "Matric":
                                intClass = 2;
                                break;
                            case "3 or 4 yr. Diploma/3 yr. Degree":
                                intClass = 1;
                                break;
                            case "4 yr. Degree/professional qualification":
                                intClass = 1;
                                break;
                        }
                    }

                    if ((decIncome >= 26250) && (decIncome < 41999))
                    {
                        switch (strQualification)
                        {
                            case "No matric":
                                intClass = 2;
                                break;
                            case "Matric":
                                intClass = 2;
                                break;
                            case "3 or 4 yr. Diploma/3 yr. Degree":
                                intClass = 1;
                                break;
                            case "4 yr. Degree/professional qualification":
                                intClass = 1;
                                break;
                        }
                    }

                    if (decIncome >= 42000)
                    {
                        switch (strQualification)
                        {
                            case "No matric":
                                intClass = 2;
                                break;
                            case "Matric":
                                intClass = 1;
                                break;
                            case "3 or 4 yr. Diploma/3 yr. Degree":
                                intClass = 1;
                                break;
                            case "4 yr. Degree/professional qualification":
                                intClass = 1;
                                break;
                        }
                    }

                    HiddenFieldClassOfLife.Value = intClass.ToString();
                }                

                #endregion

                //strBaseRisk = RadTxtAgeOfNextBirthday.Text.Trim() + strGender + "NS" + HiddenFieldClassOfLife.Value + "WL";

                strBaseRisk = GetBaseRisk(1);  //1 = Life

                //Compile the risk modifier
                //[Diabetes Type][Age Bracket at Quote][Control Level][Diabetes Duration][Cholesterol Indicator][BP Indicator][Smoker Level]

                #region "Diabetes Type"
                if (RadioButtonDiabetesType1.Checked)
                    strDiabetesType = "T1";
                if (RadioButtonDiabetesType2.Checked)
                    strDiabetesType = "T2";
                if (RadioButtonDiabetesTypeNotSure.Checked)
                    strDiabetesType = "T2";

                //if insulin then T1 else T2
                if (RadioButtonInsulinYes.Checked)
                    strDiabetesType = "T1";
                #endregion

                #region "AgeBracket"
                if (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 30)
                {
                    strAgeBracket = "A1";
                }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 30) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 40))
                {
                    strAgeBracket = "A2";
                }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 40) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 50))
                {
                    strAgeBracket = "A3";
                }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 50) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 60))
                {
                    strAgeBracket = "A4";
                }

                if ((Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 60) && (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) < 70))
                {
                    strAgeBracket = "A5";
                }

                if (Convert.ToInt16(RadTxtAgeOfNextBirthday.Text.Trim()) >= 70)
                {
                    strAgeBracket = "A6";
                }
                #endregion

                #region "Control Level"
                //C1	<=7
                //C2	>7<=8
                //C3	>8<=9
                //C4	>9<=10
                //C5	>10<=11
                //C6	>11<=12
                //C7	>12<=14
                if ((RadioButtonHbA1c3.Checked == true) || (RadioButtonHbA1c4.Checked == true) || (RadioButtonHbA1c6.Checked == true))
                    strControlLevel = "C1";
                if (RadioButtonHbA1c7.Checked == true)
                    strControlLevel = "C2";
                if (RadioButtonHbA1c8.Checked == true)
                    strControlLevel = "C3";
                if (RadioButtonHbA1c9.Checked == true)
                    strControlLevel = "C4";
                if (RadioButtonHbA1c10.Checked == true)
                    strControlLevel = "C5";
                if (RadioButtonHbA1c11.Checked == true)
                    strControlLevel = "C6";
                if (RadioButtonHbA1c12.Checked == true)
                    strControlLevel = "C7";

                if (RadioButtonHbA1cUnknown.Checked == true)
                {
                    //C3	1
                    //C4	2
                    //C5	3
                    //C6	4
                }


                #endregion

                #region "Diabetes Duration"
                /*
                    Duration = CASE 
                    WHEN DATEPART(year,Q.dtFieldValue) – D.iFieldValue < 5 THEN '<5'
                    WHEN DATEPART(year,Q.dtFieldValue) – D.iFieldValue < 15 THEN '5_15'
                    ELSE '>15'
                    END

                    <5	    Less than 5 years
                    5_15	More than 5 but less than 15 years
                    >15	    More than 15 years
                */
                //strDateDiagnosed = RadDatePickerDateOfDiag.SelectedDate.ToString();
                strDateDiagnosed = RadMonthYearPickerDateOfDiag.SelectedDate.ToString();
                strQuoteDate = RadDatePickerQuoteDate.SelectedDate.ToString();

                DateTime DTDateDiagnosed = Convert.ToDateTime(strDateDiagnosed);
                DateTime DTQuoteDate = Convert.ToDateTime(strQuoteDate);

                int YearsPassed = DTQuoteDate.Year - DTDateDiagnosed.Year;
                if (YearsPassed < 5)
                    strDiabetesDuration = "<5";
                if ((YearsPassed >= 5) && (YearsPassed < 15))
                    strDiabetesDuration = "5_15";
                if (YearsPassed >= 15)
                    strDiabetesDuration = ">15";

                #endregion

                #region "Cholesterol Indicator"
                /*    
                    High	Yes
                    Norm	No
                    Norm	Unsure
                 */

                if (RadioButtonCholesterolYes.Checked == true)
                    strCholesterolIndicator = "High";
                if (RadioButtonCholesterolNo.Checked == true)
                    strCholesterolIndicator = "Norm";
                if (RadioButtonCholesterolNotSure.Checked == true)
                    strCholesterolIndicator = "Norm";

                #endregion

                #region "BP Indicator"
                /*    
                    High	Yes
                    Norm	No
                    Norm	Unsure
                 */

                if (RadioButtonHighBPYes.Checked == true)
                    strBPIndicator = "High";
                if (RadioButtonHighBPNo.Checked == true)
                    strBPIndicator = "Norm";
                if (RadioButtonHighBP.Checked == true)
                    strBPIndicator = "Norm";

                #endregion

                #region "Smoker Level"
                /*    
                    S0	Non Smoker (P_M2_Q12 = False or P_TOBACCOFREQ = 0)
                    S1	<=2
                    S2	>2;<=5
                    S3	>5;<=20
                    S4	>20
                 */

                if (RadioButtonListTobacco.SelectedValue == "non-smoker")
                    strSmokerLevel = "S0";
                if (RadioButtonListTobacco.SelectedValue == "< 1")
                    strSmokerLevel = "S1";
                if (RadioButtonListTobacco.SelectedValue == "1 - 5")
                    strSmokerLevel = "S2";
                if (RadioButtonListTobacco.SelectedValue == "6 - 20")
                    strSmokerLevel = "S3";
                if (RadioButtonListTobacco.SelectedValue == "> 20")
                    strSmokerLevel = "S4";

                #endregion

                //strRiskModifier = strDiabetesType + strAgeBracket + strControlLevel + strDiabetesDuration + strCholesterolIndicator + strBPIndicator + strSmokerLevel;
                strRiskModifier = GetRiskModifier();

                HiddenFieldRiskModifier.Value = strRiskModifier;
                HiddenFieldRMDiabetesType.Value = strDiabetesType;

                //Calc Premium
                //e.g. Income * 6%
                decRate = (decIncome * decPI) / 100;
                //Split the Rate 50%-%0%
                decRateLife = decRate / 2;
                decRateDisability = decRate / 2;

                DataTable ReturnDT;
                WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();
                bool blnUseLife = false;
                bool blnUseDisability = false;

                //Life
                //****************************************
                #region "Life"
                //Get the Cover     

                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                {
                    strRiskModifier = "ACCIDENTAL";
                }

                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                {
                    strRiskModifier = "ACCIDENTAL_CANCER";
                }

                if (RadNumericTxtEMLoadingLife.Text.Trim() == "")
                    RadNumericTxtEMLoadingLife.Text = "0";

                strProductType = GetBenifitCode(1); //1= life; 2 = Disability

                //if (RadioButtonEscLife6.Checked == true)
                if (RadioButtonEsc6.Checked == true)
                    strEscalation = "06";
                //if (RadioButtonEscLife10.Checked == true)
                if (RadioButtonEsc10.Checked == true)
                    strEscalation = "10";

                if (RadioButtonQuoteLifeNo.Checked == false)
                {
                    if ((buttonNumber != 4) && (buttonNumber != 5))
                    {                        
                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                        {
                            ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                        }
                        else
                        {
                            ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                        }
                        
                        //ReturnDT = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        //ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingLife.Text);

                        foreach (DataRow row in ReturnDT.Rows)
                        {
                            if (row["UnroudedCover"].ToString() == "")
                            {
                                decCover = 0;
                            }
                            else
                            {
                                //decCover = Convert.ToDecimal(row["Cover"].ToString());
                                decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                            }
                        }
                    }
                    else
                    {
                        if (buttonNumber == 4)
                        {
                            decCover = Convert.ToDecimal(RadNumericTxtCoverLife.Text.Trim());
                        }

                        if (buttonNumber == 5)
                        {
                            //decDesiredContribution = Convert.ToDecimal(RadNumericTxtDesireContribution.Text);
                            ////if the disability panel is unavalable then we don't spilt the month;y contribution
                            //if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                            //{
                            //    decDesiredContribution = decDesiredContribution / 2;
                            //}

                            if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, RadNumericTxtPremiumLife.Text, RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                            }
                            else
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, RadNumericTxtPremiumLife.Text, RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                            }
                          //ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, RadNumericTxtPremiumLife.Text, RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingLife.Text);

                            //ReturnDT = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decDesiredContribution.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");

                            foreach (DataRow row in ReturnDT.Rows)
                            {
                                if (row["Cover"].ToString() == "")
                                {
                                    decCover = 0;
                                }
                                else
                                {
                                    //decCover = Convert.ToDecimal(row["Cover"].ToString());
                                    decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                }
                            }
                        }
                    }


                    if (buttonNumber != 4)
                    {
                        //Match closest cover amount in the CoverRounding table
                        #region "Match closest cover amount in the CoverRounding table"
                        sqlCommandX = new SqlCommand();
                        sqlCommandX.Connection = sqlConnectionX;
                        sqlCommandX.CommandType = CommandType.StoredProcedure;
                        sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                        sqlParam = new SqlParameter("CoverAmount", decCover);
                        sqlCommandX.Parameters.Add(sqlParam);

                        sqlDR = sqlCommandX.ExecuteReader();
                        while (sqlDR.Read())
                        {
                            decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                        }

                        sqlDR.Close();
                        sqlCommandX.Cancel();
                        sqlCommandX.Dispose();
                        #endregion
                    }

                    //sqlConnectionX.Close();

                    //Get the premium
                    //if (buttonNumber != 5)
                    //{
                    ReturnDT = null;     
               
                    //66011307
                    
                    //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                    //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                    {
                        ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                    }
                    else
                    {
                        ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                    }
                    foreach (DataRow row in ReturnDT.Rows)
                    {
                        if (row["LoadedPremium"].ToString() != "")
                        {
                            decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                            decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                            decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                        }

                    }

                    decPremium = decPremium + decPremiumFixedFee;
                   
                    #region "If less than 130"
                    //if (decPremium < 130)  
                    //{
                    //    decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee

                    //    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                    //    {
                    //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingLife.Text);
                    //    }
                    //    else
                    //    {
                    //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingLife.Text);
                    //    }
                       
                    //    foreach (DataRow row in ReturnDT.Rows)
                    //    {
                    //        if (row["Cover"].ToString() == "")
                    //        {
                    //            decCover = 0;
                    //        }
                    //        else
                    //        {                                
                    //            decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                    //        }
                    //    }

                    //    //Match closest cover amount in the CoverRounding table
                    //    #region "Match closest cover amount in the CoverRounding table"
                    //    sqlCommandX = new SqlCommand();
                    //    sqlCommandX.Connection = sqlConnectionX;
                    //    sqlCommandX.CommandType = CommandType.StoredProcedure;
                    //    sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                    //    sqlParam = new SqlParameter("CoverAmount", decCover);
                    //    sqlCommandX.Parameters.Add(sqlParam);

                    //    sqlDR = sqlCommandX.ExecuteReader();
                    //    while (sqlDR.Read())
                    //    {
                    //        decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                    //    }

                    //    sqlDR.Close();
                    //    sqlCommandX.Cancel();
                    //    sqlCommandX.Dispose();
                    //    #endregion

                    //    decPremium = decPremium + decPremiumFixedFee;
                    //}

                    #endregion

                    //}
                    //else
                    //{
                    //    decPremium = Convert.ToDecimal(RadNumericTxtPremiumLife.Text);
                    //}
                }
                #endregion

                //Disability
                //****************************************
                #region "Disability"
                if (RadNumericTxtEMLoadingDisability.Text.Trim() == "")
                    RadNumericTxtEMLoadingDisability.Text = "0";

                //if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                //{
                strRiskModifier = GetRiskModifier();

                if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "ADB")
                {
                    strRiskModifier = "ACCIDENTAL";
                }

                if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "ACDB")
                {
                    strRiskModifier = "ACCIDENTAL_CANCER";
                }

                strBaseRisk = GetBaseRisk(2);  //2 = Disability                        

                strProductType = GetBenifitCode(2); //1= life; 2 = Disability

                //if (RadioButtonEscalationDis6.Checked == true)
                //    strEscalation = "06";
                //if (RadioButtonEscalationDis10.Checked == true)
                //    strEscalation = "10";

                //Get the Cover
                if (RadioButtonQuoteDisNo.Checked == false)
                {
                    DataTable ReturnDTDisability;
                    if ((buttonNumber != 4) && (buttonNumber != 5))
                    {
                        //ReturnDTDisability = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        //ReturnDTDisability = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decRateDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                        {
                            ReturnDTDisability = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                        }
                        else
                        {
                            ReturnDTDisability = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decRateLife.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                        }

                        foreach (DataRow row in ReturnDTDisability.Rows)
                        {
                            if (row["Cover"].ToString() == "")
                            {
                                decCoverDisability = 0;
                            }
                            else
                            {
                                decCoverDisability = Convert.ToDecimal(row["Cover"].ToString());
                            }

                            strRiskband = row["Riskband"].ToString();
                        }
                    }
                    else
                    {
                        if (buttonNumber == 4)
                        {
                            if (RadNumericTxtCoverAmnDis.Text.Trim() == "")
                                RadNumericTxtCoverAmnDis.Text = "0";

                            decCoverDisability = Convert.ToDecimal(RadNumericTxtCoverAmnDis.Text.Trim());
                        }

                        if (buttonNumber == 5)
                        {
                            //decDesiredContribution = Convert.ToDecimal(RadNumericTxtDesireContribution.Text);

                                    
                            //ReturnDTDisability = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, RadNumericTxtPremiumDis.Text, RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                            if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                            {
                                ReturnDTDisability = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, RadNumericTxtPremiumDis.Text.Trim(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                            }
                            else
                            {
                                ReturnDTDisability = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, RadNumericTxtPremiumDis.Text.Trim(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                            }

                            //ReturnDTDisability = WS.returnCover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decDesiredContribution.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");

                            foreach (DataRow row in ReturnDTDisability.Rows)
                            {
                                if (row["Cover"].ToString() == "")
                                {
                                    decCoverDisability = 0;
                                }
                                else
                                {
                                    decCoverDisability = Convert.ToDecimal(row["Cover"].ToString());
                                }

                                strRiskband = row["Riskband"].ToString();
                            }
                        }
                    }

                    //Match closest cover amount in the CoverRounding table
                    if (buttonNumber != 4)
                    {
                        #region "Match closest cover amount in the CoverRounding table"
                        sqlCommandX = new SqlCommand();
                        sqlCommandX.Connection = sqlConnectionX;
                        sqlCommandX.CommandType = CommandType.StoredProcedure;
                        sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                        sqlParam = new SqlParameter("CoverAmount", decCoverDisability);
                        sqlCommandX.Parameters.Add(sqlParam);

                        sqlDR = sqlCommandX.ExecuteReader();
                        while (sqlDR.Read())
                        {
                            decCoverDisability = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                        }

                        sqlDR.Close();
                        sqlCommandX.Cancel();
                        sqlCommandX.Dispose();
                        #endregion
                    }

                    //Get the premium
                    //if (buttonNumber != 5)
                    //{
                    ReturnDT = null;
                    //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct, strBaseRisk, strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                    //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                            
                    //ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingDisability.Text);
                    if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                    {
                        ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                    }
                    else
                    {
                        ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                    }


                    foreach (DataRow row in ReturnDT.Rows)
                    {
                        if (row["LoadedPremium"].ToString() != "")
                        {
                            decPremiumDisability = Convert.ToDecimal(row["LoadedPremium"].ToString());
                            decPremiumDisabilityUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                            decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                        }
                        else
                        {
                            decPremiumDisability = 0;
                            decPremiumDisabilityUnrounded = 0;
                            decPremiumFixedFee = 0;
                        }

                    }

                    decPremiumDisability = decPremiumDisability + decPremiumFixedFee;
                            
                    //}
                    //else
                    //{
                    //    decPremium = Convert.ToDecimal(RadNumericTxtPremiumLife.Text);
                    //}
                }
                //}
                #endregion               

                // "Button 1, 2 / 3 only"
                if ((buttonNumber != 4) && (buttonNumber != 5))
                {
                    #region "Button 1, 2 / 3 only"
                    if (decCover > 0)
                        decCPRateLife = (decPremiumUnrounded / decCover);
                    if (decCoverDisability > 0)
                        decCPRateDisability = (decPremiumDisabilityUnrounded / decCoverDisability);                    

                    if (RadioButtonQuoteLifeNo.Checked == false)
                    {
                        if ((PanelLife.GroupingText.Contains("Unavailable") == false) || (RadComboBoxTypeBenefitLife.SelectedItem.Text != "FDB"))
                        {
                            blnUseLife = true;
                        }
                    }

                    if (RadioButtonQuoteDisNo.Checked == false)
                    {
                        if ((PanelDisability.GroupingText.Contains("Unavailable") == false)  || (RadComboBoxTypeBenefitDisability.SelectedItem.Text != "FDB"))
                        {
                            blnUseDisability = true;
                        }
                    }

                    if ((blnUseLife == true) && (blnUseDisability == false))
                    {
                        #region "Life == true | Disability == false"
                        decCover = ((((decIncome * decPI) / 100)) / decCPRateLife);

                        if (buttonNumber != 4)
                        {
                            #region "Match closest cover amount in the CoverRounding table"
                            sqlCommandX = new SqlCommand();
                            sqlCommandX.Connection = sqlConnectionX;
                            sqlCommandX.CommandType = CommandType.StoredProcedure;
                            sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                            sqlParam = new SqlParameter("CoverAmount", decCover);
                            sqlCommandX.Parameters.Add(sqlParam);

                            sqlDR = sqlCommandX.ExecuteReader();
                            while (sqlDR.Read())
                            {
                                decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                            }

                            sqlDR.Close();
                            sqlCommandX.Cancel();
                            sqlCommandX.Dispose();
                            #endregion
                        }

                        strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                        strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                        ReturnDT = null;
                        //if (RadioButtonEscalationDis6.Checked == true)
                        //    strEscalation = "06";
                        //if (RadioButtonEscalationDis10.Checked == true)
                        //    strEscalation = "10";
                        
                        //if (RadioButtonEscLife6.Checked == true)
                        if (RadioButtonEsc6.Checked == true)
                            strEscalation = "06";
                        //if (RadioButtonEscLife10.Checked == true)
                        if (RadioButtonEsc10.Checked == true)
                            strEscalation = "10";
                        //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        //ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingDisability.Text);
                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingDisability.Text);
                        }
                        else
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingDisability.Text);
                        }

                        foreach (DataRow row in ReturnDT.Rows)
                        {
                            if (row["LoadedPremium"].ToString() != "")
                            {
                                decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());

                            }
                        }

                        decPremium = decPremium + decPremiumFixedFee;
                        #endregion
                    }

                    if ((blnUseLife == false) && (blnUseDisability == true))
                    {
                        #region "Life == false | Disability == true"
                        decCoverDisability = ((((decIncome * decPI) / 100)) / decCPRateDisability);

                        if (buttonNumber != 4)
                        {
                            #region "Match closest cover amount in the CoverRounding table"
                            sqlCommandX = new SqlCommand();
                            sqlCommandX.Connection = sqlConnectionX;
                            sqlCommandX.CommandType = CommandType.StoredProcedure;
                            sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                            sqlParam = new SqlParameter("CoverAmount", decCoverDisability);
                            sqlCommandX.Parameters.Add(sqlParam);

                            sqlDR = sqlCommandX.ExecuteReader();
                            while (sqlDR.Read())
                            {
                                decCoverDisability = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                            }

                            sqlDR.Close();
                            sqlCommandX.Cancel();
                            sqlCommandX.Dispose();
                            #endregion
                        }

                        //Recalculate the disability premium
                        strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                        strProductType = GetBenifitCode(2); //1= life; 2 = Disability
                        ReturnDT = null;
                        //if (RadioButtonEscalationDis6.Checked == true)
                        if (RadioButtonEsc6.Checked == true)
                            strEscalation = "06";
                        //if (RadioButtonEscalationDis10.Checked == true)
                        if (RadioButtonEsc10.Checked == true)
                            strEscalation = "10";
                        //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        //ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingDisability.Text);
                        if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                        }
                        else
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                        }

                        foreach (DataRow row in ReturnDT.Rows)
                        {
                            decPremiumDisability = Convert.ToDecimal(row["LoadedPremium"].ToString());
                            decPremiumDisabilityUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                            decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                        }

                        decPremiumDisability = decPremiumDisability + decPremiumFixedFee;
                        #endregion
                    }

                    if ((blnUseLife == true) && (blnUseDisability == true))
                    {
                        #region "Life == true | Disability == true"
                        //decCover = ((decPremiumUnrounded + decPremiumDisabilityUnrounded) / (decCPRateLife + decCPRateDisability));
                        decCover = ((((decIncome * decPI) / 100)) / (decCPRateLife + decCPRateDisability));

                        if (buttonNumber != 4)
                        {
                            #region "Match closest cover amount in the CoverRounding table"
                            sqlCommandX = new SqlCommand();
                            sqlCommandX.Connection = sqlConnectionX;
                            sqlCommandX.CommandType = CommandType.StoredProcedure;
                            sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                            sqlParam = new SqlParameter("CoverAmount", decCover);
                            sqlCommandX.Parameters.Add(sqlParam);

                            sqlDR = sqlCommandX.ExecuteReader();
                            while (sqlDR.Read())
                            {
                                decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                            }

                            sqlDR.Close();
                            sqlCommandX.Cancel();
                            sqlCommandX.Dispose();

                            #endregion
                        }

                        decCoverDisability = decCover;

                        //Recalculate the life premium
                        #region "Life"
                        strRiskModifier = GetRiskModifier();

                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                        {
                            strRiskModifier = "ACCIDENTAL";
                        }

                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                        {
                            strRiskModifier = "ACCIDENTAL_CANCER";
                        }

                        //Recalculate the life premium
                        strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                        strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                        ReturnDT = null;
                        //if (RadioButtonEscLife6.Checked == true)
                        if (RadioButtonEsc6.Checked == true)
                            strEscalation = "06";
                        //if (RadioButtonEscLife10.Checked == true)
                        if (RadioButtonEsc10.Checked == true)
                            strEscalation = "10";

                        //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        //ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingLife.Text);
                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                        }
                        else
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                        }
                        
                        foreach (DataRow row in ReturnDT.Rows)
                        {
                            if (row["LoadedPremium"].ToString() != "")
                            {
                                decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());

                            }
                        }
                        #endregion

                        decPremium = decPremium + decPremiumFixedFee;

                        //Recalculate the disability premium
                        #region "Disability"
                        strRiskModifier = GetRiskModifier();

                        if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "ADB")
                        {
                            strRiskModifier = "ACCIDENTAL";
                        }

                        if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "ACDB")
                        {
                            strRiskModifier = "ACCIDENTAL_CANCER";
                        }

                        strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                        strProductType = GetBenifitCode(2); //1= life; 2 = Disability
                        ReturnDT = null;
                        //if (RadioButtonEscalationDis6.Checked == true)
                        if (RadioButtonEsc6.Checked == true)
                            strEscalation = "06";
                        //if (RadioButtonEscalationDis10.Checked == true)
                        if (RadioButtonEsc10.Checked == true)
                            strEscalation = "10";
                        //ReturnDT = WS.returnPremium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "");
                        //ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", RadNumericTxtEMLoadingDisability.Text);
                        if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                        }
                        else
                        {
                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                        }

                        foreach (DataRow row in ReturnDT.Rows)
                        {
                            if (row["LoadedPremium"].ToString() != "")
                            {
                                decPremiumDisability = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                decPremiumDisabilityUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                            }
                            else
                            {
                                decPremiumDisability = 0;
                                decPremiumDisabilityUnrounded = 0;
                                decPremiumFixedFee = 0;
                            }
                        }
                        #endregion

                        decPremiumDisability = decPremiumDisability + decPremiumFixedFee;
                        #endregion
                    }
                    #endregion
                }
               
                //If less than 130       
                //if ((buttonNumber != 4) && (buttonNumber != 5))
                //2015-10-05 - included button option 5 because we were going to add the same logic below for option 5 only
                if (buttonNumber != 4)
                {
                    strControlFee = decPremiumFixedFee.ToString();

                    if (strControlFee == "")
                        strControlFee = "1";
                    
                    //2015-09-19 - The total (decPremium + decPremiumDisability) was commentd out. I am not going back to using the total
                    ///if (decPremium < 130)
                    //if ((decPremium + decPremiumDisability) < 130)
                    if ((((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)))) < 130)
                    {
                        #region "If less than 130"
                        //decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee
                        //2015-10-13  - commented out the below so that it sets the blnUseLife and\or blnUseDisability
                        //if (decPremiumDisability == 0)
                        //{
                        //    decPremium = 130;
                        //}
                        //else
                        //{
                            //decPremium = 130 - decPremiumDisability + 20;

                            if (RadioButtonQuoteLifeNo.Checked == false)
                            {
                                if ((PanelLife.GroupingText.Contains("Unavailable") == false) || (RadComboBoxTypeBenefitLife.SelectedItem.Text != "FDB"))
                                {
                                    blnUseLife = true;
                                }
                            }

                            if (RadioButtonQuoteDisNo.Checked == false)
                            {
                                if ((PanelDisability.GroupingText.Contains("Unavailable") == false) || (RadComboBoxTypeBenefitDisability.SelectedItem.Text != "FDB"))
                                {
                                    blnUseDisability = true;
                                }
                            }

                            if ((blnUseLife == true) && (blnUseDisability == true))
                            {
                                decPremium = 75;
                                decPremiumDisability = 75;
                            }

                            if ((blnUseLife == true) && (blnUseDisability == false))
                            {
                                decPremium = 130;
                                decPremiumDisability = 0;
                            }

                            if ((blnUseLife == false) && (blnUseDisability == true))
                            {
                                decPremium = 0;
                                decPremiumDisability = 130;
                            }
                        //}

                        strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                        strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                        //strRiskModifier = GetRiskModifier();

                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                        {
                            strRiskModifier = "ACCIDENTAL";
                        }

                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                        {
                            strRiskModifier = "ACCIDENTAL_CANCER";
                        }

                        ReturnDT = null;

                        if (blnUseLife == true)
                        {

                            if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                            }
                            else
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                            }

                            foreach (DataRow row in ReturnDT.Rows)
                            {
                                if (row["Cover"].ToString() == "")
                                {
                                    decCover = 0;
                                }
                                else
                                {
                                    decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                }
                            }

                            //Match closest cover amount in the CoverRounding table
                            #region "Match closest cover amount in the CoverRounding table"
                            sqlCommandX2 = new SqlCommand();
                            sqlCommandX2.Connection = sqlConnectionX;
                            sqlCommandX2.CommandType = CommandType.StoredProcedure;
                            sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                            sqlParam = new SqlParameter("CoverAmount", decCover);
                            sqlCommandX2.Parameters.Add(sqlParam);

                            sqlDR2 = sqlCommandX2.ExecuteReader();
                            while (sqlDR2.Read())
                            {
                                decCover = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                            }

                            sqlDR2.Close();
                            sqlCommandX2.Cancel();
                            sqlCommandX2.Dispose();
                            #endregion

                            if (decCover == 40000)
                            {
                                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                {
                                    ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                }
                                else
                                {
                                    ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                }

                                foreach (DataRow row in ReturnDT.Rows)
                                {
                                    if (row["LoadedPremium"].ToString() != "")
                                    {
                                        decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                        decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                        decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());

                                    }
                                }

                                decPremium = decPremium + decPremiumFixedFee;
                            }                            
                        }


                        if (RadioButtonQuoteDisNo.Checked == false)
                        {
                            //Disability
                            //Recalculate the disability premium
                            strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                            strProductType = GetBenifitCode(2); //1= life; 2 = Disability
                            ReturnDT = null;
                            if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                            }
                            else
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                            }

                            foreach (DataRow row in ReturnDT.Rows)
                            {
                                if (row["Cover"].ToString() == "")
                                {
                                    decCoverDisability = 0;
                                }
                                else
                                {
                                    decCoverDisability = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                }
                            }

                            //Match closest cover amount in the CoverRounding table
                            #region "Match closest cover amount in the CoverRounding table"
                            sqlCommandX2 = new SqlCommand();
                            sqlCommandX2.Connection = sqlConnectionX;
                            sqlCommandX2.CommandType = CommandType.StoredProcedure;
                            sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                            sqlParam = new SqlParameter("CoverAmount", decCoverDisability);
                            sqlCommandX2.Parameters.Add(sqlParam);

                            sqlDR2 = sqlCommandX2.ExecuteReader();
                            while (sqlDR2.Read())
                            {
                                decCoverDisability = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                            }

                            sqlDR2.Close();
                            sqlCommandX2.Cancel();
                            sqlCommandX2.Dispose();
                            #endregion

                            //decPremium = decPremium + decPremiumFixedFee;

                            if (decCoverDisability == 40000)
                            {
                                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                {
                                    ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                                }
                                else
                                {
                                    ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                                }

                                foreach (DataRow row in ReturnDT.Rows)
                                {
                                    if (row["LoadedPremium"].ToString() != "")
                                    {
                                        decPremiumDisability = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                        decPremiumDisabilityUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                        decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                                    }
                                    else
                                    {
                                        decPremiumDisability = 0;
                                        decPremiumDisabilityUnrounded = 0;
                                        decPremiumFixedFee = 0;
                                    }
                                }

                                decPremiumDisability = decPremiumDisability + decPremiumFixedFee;


                                if ((((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)))) < 130)
                                {
                                    //if the total premium is still less that 130 then use the rounded cover
                                    //decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee
                                    if (decPremiumDisability == 0)
                                    {
                                        decPremium = 130;
                                    }
                                    else
                                    {
                                        //decPremium = 130 - decPremiumDisability + 20;
                                        if ((blnUseLife == true) && (blnUseDisability == true))
                                        {
                                            decPremium = 75;
                                            decPremiumDisability = 75;
                                        }

                                        if ((blnUseLife == true) && (blnUseDisability == false))
                                        {
                                            decPremium = 130;
                                            decPremiumDisability = 0;
                                        }

                                        if ((blnUseLife == false) && (blnUseDisability == true))
                                        {
                                            decPremium = 0;
                                            decPremiumDisability = 130;
                                        }
                                    }

                                    strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                                    strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                                    //strRiskModifier = GetRiskModifier();

                                    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                                    {
                                        strRiskModifier = "ACCIDENTAL";
                                    }

                                    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                                    {
                                        strRiskModifier = "ACCIDENTAL_CANCER";
                                    }

                                    ReturnDT = null;

                                    if (blnUseLife == true)
                                    {

                                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        {
                                            ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        }
                                        else
                                        {
                                            ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        }

                                        foreach (DataRow row in ReturnDT.Rows)
                                        {
                                            if (row["Cover"].ToString() == "")
                                            {
                                                decCover = 0;
                                            }
                                            else
                                            {
                                                decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                            }
                                        }

                                        //Match closest cover amount in the CoverRounding table
                                        #region "Match closest cover amount in the CoverRounding table"
                                        sqlCommandX2 = new SqlCommand();
                                        sqlCommandX2.Connection = sqlConnectionX;
                                        sqlCommandX2.CommandType = CommandType.StoredProcedure;
                                        sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                                        sqlParam = new SqlParameter("CoverAmount", decCover);
                                        sqlCommandX2.Parameters.Add(sqlParam);

                                        sqlDR2 = sqlCommandX2.ExecuteReader();
                                        while (sqlDR2.Read())
                                        {
                                            decCover = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                                        }

                                        sqlDR2.Close();
                                        sqlCommandX2.Cancel();
                                        sqlCommandX2.Dispose();
                                        #endregion

                                        if (decCover == 40000)
                                        {
                                            if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                            {
                                                ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                            }
                                            else
                                            {
                                                ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                            }

                                            foreach (DataRow row in ReturnDT.Rows)
                                            {
                                                if (row["LoadedPremium"].ToString() != "")
                                                {
                                                    decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                                    decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                                    decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());

                                                }
                                            }

                                            decPremium = decPremium + decPremiumFixedFee;
                                        }
                                    }


                                    if (RadioButtonQuoteDisNo.Checked == false)
                                    {
                                        //Disability
                                        //Recalculate the disability premium
                                        strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                                        strProductType = GetBenifitCode(2); //1= life; 2 = Disability
                                        ReturnDT = null;
                                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        {
                                            ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        }
                                        else
                                        {
                                            ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        }

                                        foreach (DataRow row in ReturnDT.Rows)
                                        {
                                            if (row["Cover"].ToString() == "")
                                            {
                                                decCoverDisability = 0;
                                            }
                                            else
                                            {
                                                decCoverDisability = Convert.ToDecimal(row["Cover"].ToString());
                                            }
                                        }

                                        //Match closest cover amount in the CoverRounding table
                                        #region "Match closest cover amount in the CoverRounding table"
                                        sqlCommandX2 = new SqlCommand();
                                        sqlCommandX2.Connection = sqlConnectionX;
                                        sqlCommandX2.CommandType = CommandType.StoredProcedure;
                                        sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                                        sqlParam = new SqlParameter("CoverAmount", decCoverDisability);
                                        sqlCommandX2.Parameters.Add(sqlParam);

                                        sqlDR2 = sqlCommandX2.ExecuteReader();
                                        while (sqlDR2.Read())
                                        {
                                            decCoverDisability = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                                        }

                                        sqlDR2.Close();
                                        sqlCommandX2.Cancel();
                                        sqlCommandX2.Dispose();
                                        #endregion

                                        //decPremium = decPremium + decPremiumFixedFee;


                                        //decPremiumDisability = decPremiumDisability + decPremiumFixedFee;

                                    }
                                }
                            }
                        }
                        #endregion
                    }                    
                }
                            
                //If less than 130 for button 4 only
                if (buttonNumber == 4)
                {
                    #region "If less than 130 for button 4 only"
                    
                    strControlFee = decPremiumFixedFee.ToString();

                    if (strControlFee == "")
                        strControlFee = "1";
                   
                        #region "Life Only"
                        if (RadioButtonQuoteLifeYes.Checked == true && RadioButtonQuoteDisYes.Checked == false)
                        {
                            if (decPremium < 130)
                            {
                                decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee

                                //if (RadioButtonEscalationDis6.Checked == true)
                                if (RadioButtonEsc6.Checked == true)
                                    strEscalation = "06";
                                //if (RadioButtonEscalationDis10.Checked == true)
                                if (RadioButtonEsc10.Checked == true)
                                    strEscalation = "10";

                                strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                                strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                                ReturnDT = null;

                                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                {
                                    //ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                    ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                }
                                else
                                {
                                    //ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                    ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                }

                                foreach (DataRow row in ReturnDT.Rows)
                                {
                                    if (row["Cover"].ToString() == "")
                                    {
                                        decCover = 0;
                                    }
                                    else
                                    {
                                        decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                    }
                                }

                                //Match closest cover amount in the CoverRounding table
                                #region "Match closest cover amount in the CoverRounding table"
                                sqlCommandX2 = new SqlCommand();
                                sqlCommandX2.Connection = sqlConnectionX;
                                sqlCommandX2.CommandType = CommandType.StoredProcedure;
                                sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                                sqlParam = new SqlParameter("CoverAmount", decCover);
                                sqlCommandX2.Parameters.Add(sqlParam);

                                sqlDR2 = sqlCommandX2.ExecuteReader();
                                while (sqlDR2.Read())
                                {
                                    decCover = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                                }

                                sqlDR2.Close();
                                sqlCommandX2.Cancel();
                                sqlCommandX2.Dispose();
                                #endregion

                                //2015-10-13 - taken out because the premium from option 4 was R150 and not matching premium (130) on option 5 (e.g. MagID 67332124)
                                //decPremium = decPremium + decPremiumFixedFee;
                            }
                        }
                        #endregion

                        #region "Disability only"
                        if (RadioButtonQuoteDisYes.Checked == true && RadioButtonQuoteLifeYes.Checked == false)
                        {                            
                            if (decPremiumDisability < 130)
                            {
                                decPremiumDisability = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee
                                RadNumericTxtOption4Total.Text = (decPremium + decPremiumDisability).ToString();
                            }

                            //if (RadioButtonEscalationDis6.Checked == true)
                            if (RadioButtonEsc6.Checked == true)
                                strEscalation = "06";
                            //if (RadioButtonEscalationDis10.Checked == true)
                            if (RadioButtonEsc10.Checked == true)
                                strEscalation = "10";
                            //Recalculate the disability premium
                            strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                            strProductType = GetBenifitCode(2); //1= life; 2 = Disability

                            if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                            {
                                strRiskModifier = "ACCIDENTAL";
                            }

                            if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                            {
                                strRiskModifier = "ACCIDENTAL_CANCER";
                            }

                            ReturnDT = null;
                            if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                            }
                            else
                            {
                                ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingDisability.Text);
                            }

                            foreach (DataRow row in ReturnDT.Rows)
                            {
                                if (row["Cover"].ToString() == "")
                                {
                                    decCoverDisability = 0;
                                }
                                else
                                {
                                    decCoverDisability = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                }
                            }

                            //Match closest cover amount in the CoverRounding table
                            #region "Match closest cover amount in the CoverRounding table"
                            sqlCommandX2 = new SqlCommand();
                            sqlCommandX2.Connection = sqlConnectionX;
                            sqlCommandX2.CommandType = CommandType.StoredProcedure;
                            sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                            sqlParam = new SqlParameter("CoverAmount", decCoverDisability);
                            sqlCommandX2.Parameters.Add(sqlParam);

                            sqlDR2 = sqlCommandX2.ExecuteReader();
                            while (sqlDR2.Read())
                            {
                                decCoverDisability = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                            }

                            sqlDR2.Close();
                            sqlCommandX2.Cancel();
                            sqlCommandX2.Dispose();
                            #endregion

                            //decPremiumDisability = decPremiumDisability + decPremiumFixedFee;

                        }
                        #endregion

                        if (RadioButtonQuoteDisYes.Checked == true && RadioButtonQuoteLifeYes.Checked == true)
                        {
                            #region "Life and Disability"
                            if ((((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)))) < 130)
                            {
                                //if ((blnUseLife == true) && (blnUseDisability == true))
                                //{
                                decPremium = 75;
                                decPremiumDisability = 75;
                                //}                                                            

                                strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                                strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                                //strRiskModifier = GetRiskModifier();

                                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ADB")
                                {
                                    strRiskModifier = "ACCIDENTAL";
                                }

                                if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "ACDB")
                                {
                                    strRiskModifier = "ACCIDENTAL_CANCER";
                                }

                                ReturnDT = null;

                                #region "Life"
                                if (blnUseLife == true)
                                {

                                    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                    {
                                        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                    }
                                    else
                                    {
                                        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                    }

                                    foreach (DataRow row in ReturnDT.Rows)
                                    {
                                        if (row["Cover"].ToString() == "")
                                        {
                                            decCover = 0;
                                        }
                                        else
                                        {
                                            decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                        }
                                    }

                                    //Match closest cover amount in the CoverRounding table
                                    #region "Match closest cover amount in the CoverRounding table"
                                    sqlCommandX2 = new SqlCommand();
                                    sqlCommandX2.Connection = sqlConnectionX;
                                    sqlCommandX2.CommandType = CommandType.StoredProcedure;
                                    sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                                    sqlParam = new SqlParameter("CoverAmount", decCover);
                                    sqlCommandX2.Parameters.Add(sqlParam);

                                    sqlDR2 = sqlCommandX2.ExecuteReader();
                                    while (sqlDR2.Read())
                                    {
                                        decCover = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                                    }

                                    sqlDR2.Close();
                                    sqlCommandX2.Cancel();
                                    sqlCommandX2.Dispose();
                                    #endregion

                                    if (decCover == 40000)
                                    {
                                        if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        {
                                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        }
                                        else
                                        {
                                            ReturnDT = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decCover.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        }

                                        foreach (DataRow row in ReturnDT.Rows)
                                        {
                                            if (row["LoadedPremium"].ToString() != "")
                                            {
                                                decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                                                decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                                                decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());

                                            }
                                        }

                                        decPremium = decPremium + decPremiumFixedFee;
                                    }
                                }
                                #endregion

                                #region "Disability"
                                if (RadioButtonQuoteDisNo.Checked == false)
                                {
                                    //Disability
                                    //Recalculate the disability premium
                                    strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                                    strProductType = GetBenifitCode(2); //1= life; 2 = Disability
                                    ReturnDT = null;
                                    if (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB")
                                    {
                                        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                    }
                                    else
                                    {
                                        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremiumDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                    }

                                    foreach (DataRow row in ReturnDT.Rows)
                                    {
                                        if (row["Cover"].ToString() == "")
                                        {
                                            decCoverDisability = 0;
                                        }
                                        else
                                        {
                                            decCoverDisability = Convert.ToDecimal(row["Cover"].ToString());
                                        }
                                    }

                                    //Match closest cover amount in the CoverRounding table
                                    #region "Match closest cover amount in the CoverRounding table"
                                    sqlCommandX2 = new SqlCommand();
                                    sqlCommandX2.Connection = sqlConnectionX;
                                    sqlCommandX2.CommandType = CommandType.StoredProcedure;
                                    sqlCommandX2.CommandText = "spx_SELECT_CoverRounding";

                                    sqlParam = new SqlParameter("CoverAmount", decCoverDisability);
                                    sqlCommandX2.Parameters.Add(sqlParam);

                                    sqlDR2 = sqlCommandX2.ExecuteReader();
                                    while (sqlDR2.Read())
                                    {
                                        decCoverDisability = Convert.ToDecimal(sqlDR2.GetValue(0).ToString());
                                    }

                                    sqlDR2.Close();
                                    sqlCommandX2.Cancel();
                                    sqlCommandX2.Dispose();
                                    #endregion

                                    //decPremium = decPremium + decPremiumFixedFee;

                                    //decPremiumDisability = decPremiumDisability + decPremiumFixedFee;
                                }
                                #endregion
                            }
                            #endregion
                        }

                    #endregion
                }

                

                #region "EM Loading"
                //if (RadNumericTxtEMLoadingLife.Enabled == true)
                //{
                //    //EM loading calc is a straight sum onto premium as Final premium = final premium (without EM loading) + unloaded prem *max( (EM loading – 25),0) / 100

                //    if (RadNumericTxtEMLoadingLife.Text.Trim() == "")
                //        RadNumericTxtEMLoadingLife.Text = "0";

                //    decimal decEmLoading = Convert.ToDecimal(RadNumericTxtEMLoadingLife.Text.Trim());
                //    ////decPremium = decPremium + (decPremium * (decEmLoading - 25) / 100);
                //    //decPremium = decPremium + (decPremium * calculation(decEmLoading, 25) / 100); 
                //    strBaseRisk = GetBaseRisk(1);  //1= life; 2 = Disability
                //    strProductType = GetBenifitCode(1); //1= life; 2 = Disability
                //    DataTable DTEMLoading = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", decEmLoading.ToString());
                //    foreach (DataRow row in DTEMLoading.Rows)
                //    {
                //        if (row["LoadedPremium"].ToString() != "")
                //        {
                //            decPremium = Convert.ToDecimal(row["LoadedPremium"].ToString());
                //            decPremiumUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                //            decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                //        }
                //        else
                //        {
                //            decPremium = 0;
                //            decPremiumUnrounded = 0;
                //            decPremiumFixedFee = 0;
                //        }
                //    }

                //    decPremium = decPremium + decPremiumFixedFee;
                //}

                //if (RadNumericTxtEMLoadingDisability.Enabled == true)
                //{
                //    if (RadNumericTxtEMLoadingDisability.Text.Trim() == "")
                //        RadNumericTxtEMLoadingDisability.Text = "0";

                //    decimal decEmLoading = Convert.ToDecimal(RadNumericTxtEMLoadingDisability.Text.Trim());
                //    ////decPremiumDisability = decPremiumDisability + (decPremiumDisability * (decEmLoading - 25) / 100);
                //    //decPremiumDisability = decPremiumDisability + (decPremiumDisability * calculation(decEmLoading, 25) / 100);
                //    strBaseRisk = GetBaseRisk(2);  //1= life; 2 = Disability
                //    strProductType = GetBenifitCode(2); //1= life; 2 = Disability
                //    DataTable DTEMLoading = WS.returnEM_Affected_Premium(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decCoverDisability.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), "", "", decEmLoading.ToString());
                //    foreach (DataRow row in DTEMLoading.Rows)
                //    {
                //        if (row["LoadedPremium"].ToString() != "")
                //        {
                //            decPremiumDisability = Convert.ToDecimal(row["LoadedPremium"].ToString());
                //            decPremiumDisabilityUnrounded = Convert.ToDecimal(row["UnroudedStandardPremium"].ToString());
                //            decPremiumFixedFee = Convert.ToDecimal(row["FixedFee"].ToString());
                //        }
                //        else
                //        {
                //            decPremiumDisability = 0;
                //            decPremiumDisabilityUnrounded = 0;
                //            decPremiumFixedFee = 0;
                //        }
                //    }

                //    decPremiumDisability = decPremiumDisability + decPremiumFixedFee;
                //}
                #endregion                

                //strControlFee = GetSetting("ControlFee");

                strControlFee = decPremiumFixedFee.ToString();

                if (strControlFee == "")
                    strControlFee = "1";

                bool blnLifeOk = true;
                bool blnDisabilityOk = true;

                if (RadioButtonQuoteLifeNo.Checked == true)
                {
                    blnLifeOk = false;
                }

                if ((PanelLife.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB"))
                {
                    blnLifeOk = false;
                }

                if (RadioButtonQuoteDisNo.Checked == true)
                {
                    blnDisabilityOk = false;
                }

                if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                {
                    blnDisabilityOk = false;
                }

                if (RadioButtonQuoteLifeNo.Checked == false)
                {
                    if ((PanelLife.GroupingText.Contains("Unavailable") == false) || (RadComboBoxTypeBenefitLife.SelectedItem.Text != "FDB"))
                    {
                        switch (buttonNumber)
                        {
                            case 1:
                                #region "Button 1"
                                RadNumericTxtOption1RandValue.Visible = true;
                                RadNumericTxtOption1RandValue.Text = decCover.ToString();
                                lblOp1_1.Visible = true;
                                RadNumericTxtOption1Premium.Visible = true;
                                RadNumericTxtOption1Premium.Text = decPremium.ToString();
                                lblOp1_2.Visible = true;
                                if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                                {
                                    lblOp1_2.Text = "rands per month";
                                   
                                    RadNumericTxtOption1Total.Text = decPremium.ToString();
                                   
                                }
                                else
                                {
                                    lblOp1_2.Text = "rands per month AND";
                                    decimal decTotal = 0;

                                    if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    {
                                        decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    }
                                    else
                                    {
                                        decTotal = (decPremium + decPremiumDisability);
                                    }

                                    RadNumericTxtOption1Total.Text = decTotal.ToString();
                                }
                                #endregion
                                break;
                            case 2:
                                #region "Button 2"
                                RadNumericTxtOption2RandValue.Visible = true;
                                RadNumericTxtOption2RandValue.Text = decCover.ToString();
                                lblOp2_1.Visible = true;
                                RadNumericTxtOption2Premium.Visible = true;
                                RadNumericTxtOption2Premium.Text = decPremium.ToString();
                                lblOp2_2.Visible = true;
                                if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                                {
                                    lblOp2_2.Text = "rands per month";
                                    //if (Convert.ToInt32(RadNumericTxtOption1Premium.Text) >= 130)
                                    if (Convert.ToInt32(RadNumericTxtOption1Total.Text) >= 130)
                                    {
                                        #region "old"
                                        //if (decPremium < 130)
                                        //{
                                        //    #region "Get cover for R130"
                                        //    decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee

                                        //    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }
                                        //    else
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }

                                        //    foreach (DataRow row in ReturnDT.Rows)
                                        //    {
                                        //        if (row["Cover"].ToString() == "")
                                        //        {
                                        //            decCover = 0;
                                        //        }
                                        //        else
                                        //        {
                                        //            decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                        //        }
                                        //    }

                                        //    //Match closest cover amount in the CoverRounding table
                                        //    #region "Match closest cover amount in the CoverRounding table"
                                        //    sqlCommandX = new SqlCommand();
                                        //    sqlCommandX.Connection = sqlConnectionX;
                                        //    sqlCommandX.CommandType = CommandType.StoredProcedure;
                                        //    sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                                        //    sqlParam = new SqlParameter("CoverAmount", decCover);
                                        //    sqlCommandX.Parameters.Add(sqlParam);

                                        //    sqlDR = sqlCommandX.ExecuteReader();
                                        //    while (sqlDR.Read())
                                        //    {
                                        //        decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                                        //    }

                                        //    sqlDR.Close();
                                        //    sqlCommandX.Cancel();
                                        //    sqlCommandX.Dispose();
                                        //    #endregion

                                        //    decPremium = decPremium + decPremiumFixedFee;
                                        //    #endregion

                                        //    RadNumericTxtOption2RandValue.Text = decCover.ToString();
                                        //    RadNumericTxtOption2Premium.Text = decPremium.ToString();
                                        //}
                                        #endregion

                                        RadNumericTxtOption2Total.Text = decPremium.ToString();
                                    }
                                    else
                                    {
                                        hideOption2Life();
                                        lblSuitable0.Visible = false;
                                        lblOr.Visible = false;
                                    }
                                }
                                else
                                {
                                    lblOp2_2.Text = "rands per month AND";
                                    decimal decTotal = 0;

                                    if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    {
                                        decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    }
                                    else
                                    {
                                        decTotal = (decPremium + decPremiumDisability);
                                    }

                                    RadNumericTxtOption2Total.Text = decTotal.ToString();

                                    //if (Convert.ToInt32(RadNumericTxtOption1Premium.Text) > 130)
                                    if (Convert.ToInt32(RadNumericTxtOption1Total.Text) > 130)
                                    {
                                        //dont show option 2 if the total is the same as option 1
                                        if (decTotal == Convert.ToInt32(RadNumericTxtOption1Total.Text))
                                        {
                                            hideOption2Life();
                                            hideOption2Disability();
                                            lblSuitable0.Visible = false;
                                            lblOr.Visible = false;
                                        }

                                        #region "old"
                                        //if (decPremium < 130)
                                        //{
                                        //    #region "Get cover for R130"
                                        //    decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee

                                        //    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }
                                        //    else
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }

                                        //    foreach (DataRow row in ReturnDT.Rows)
                                        //    {
                                        //        if (row["Cover"].ToString() == "")
                                        //        {
                                        //            decCover = 0;
                                        //        }
                                        //        else
                                        //        {
                                        //            decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                        //        }
                                        //    }

                                        //    //Match closest cover amount in the CoverRounding table
                                        //    #region "Match closest cover amount in the CoverRounding table"
                                        //    sqlCommandX = new SqlCommand();
                                        //    sqlCommandX.Connection = sqlConnectionX;
                                        //    sqlCommandX.CommandType = CommandType.StoredProcedure;
                                        //    sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                                        //    sqlParam = new SqlParameter("CoverAmount", decCover);
                                        //    sqlCommandX.Parameters.Add(sqlParam);

                                        //    sqlDR = sqlCommandX.ExecuteReader();
                                        //    while (sqlDR.Read())
                                        //    {
                                        //        decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                                        //    }

                                        //    sqlDR.Close();
                                        //    sqlCommandX.Cancel();
                                        //    sqlCommandX.Dispose();
                                        //    #endregion

                                        //    decPremium = decPremium + decPremiumFixedFee;
                                        //    #endregion

                                        //    RadNumericTxtOption2RandValue.Text = decCover.ToString();
                                        //    RadNumericTxtOption2Premium.Text = decPremium.ToString();
                                        //}
                                        #endregion
                                    }
                                    else
                                    {
                                        hideOption2Life();
                                        hideOption2Disability();
                                        lblSuitable0.Visible = false;
                                        lblOr.Visible = false;
                                    }
                                }
                                #endregion
                                break;
                            case 3:
                                #region "Button 3"
                                RadNumericTxtOption3RandValue.Visible = true;
                                RadNumericTxtOption3RandValue.Text = decCover.ToString();
                                lblOp3_1.Visible = true;
                                RadNumericTxtOption3Premium.Visible = true;
                                RadNumericTxtOption3Premium.Text = decPremium.ToString();
                                lblOp3_2.Visible = true;
                                if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                                {
                                    lblOp3_2.Text = "rands per month";
                                    //if (Convert.ToInt32(RadNumericTxtOption2Premium.Text) > 130)
                                    if (Convert.ToInt32(RadNumericTxtOption2Total.Text) > 130)
                                    {
                                        #region "Old"
                                        //if (decPremium < 130)
                                        //{
                                        //    #region "Get cover for R130"
                                        //    decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee

                                        //    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }
                                        //    else
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }

                                        //    foreach (DataRow row in ReturnDT.Rows)
                                        //    {
                                        //        if (row["Cover"].ToString() == "")
                                        //        {
                                        //            decCover = 0;
                                        //        }
                                        //        else
                                        //        {
                                        //            decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                        //        }
                                        //    }

                                        //    ////Match closest cover amount in the CoverRounding table
                                        //    //#region "Match closest cover amount in the CoverRounding table"
                                        //    //sqlCommandX = new SqlCommand();
                                        //    //sqlCommandX.Connection = sqlConnectionX;
                                        //    //sqlCommandX.CommandType = CommandType.StoredProcedure;
                                        //    //sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                                        //    //sqlParam = new SqlParameter("CoverAmount", decCover);
                                        //    //sqlCommandX.Parameters.Add(sqlParam);

                                        //    //sqlDR = sqlCommandX.ExecuteReader();
                                        //    //while (sqlDR.Read())
                                        //    //{
                                        //    //    decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                                        //    //}

                                        //    //sqlDR.Close();
                                        //    //sqlCommandX.Cancel();
                                        //    //sqlCommandX.Dispose();
                                        //    //#endregion

                                        //    decPremium = decPremium + decPremiumFixedFee;
                                        //    #endregion

                                        //    RadNumericTxtOption3RandValue.Text = decCover.ToString();
                                        //    RadNumericTxtOption3Premium.Text = decPremium.ToString();
                                        //}
                                        #endregion

                                        RadNumericTxtOption3Total.Text = decPremium.ToString();
                                    }
                                    else
                                    {
                                        hideOption3Life();
                                        lblAlso.Visible = false;
                                    }
                                }
                                else
                                {
                                    lblOp3_2.Text = "rands per month AND";
                                    decimal decTotal = 0;

                                    if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    {
                                        decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    }
                                    else
                                    {
                                        decTotal = (decPremium + decPremiumDisability);
                                    }

                                    RadNumericTxtOption3Total.Text = decTotal.ToString();

                                    if (Convert.ToInt32(RadNumericTxtOption2Total.Text) > 130)
                                    {
                                        if (decTotal == Convert.ToInt32(RadNumericTxtOption2Total.Text))
                                        {
                                            hideOption3Life();
                                            //hideOption3Disability();
                                            lblAlso.Visible = false;
                                        }

                                        #region "Old"
                                        //if (decPremium < 130)
                                        //{
                                        //    #region "Get cover for R130"
                                        //    decPremium = 130; ///Making this 110 - this is the fee of 130 with out the fixed fee

                                        //    if (RadComboBoxTypeBenefitLife.SelectedItem.Text == "FDB")
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk + RadTxtRiskBand.Text.Trim(), strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }
                                        //    else
                                        //    {
                                        //        ReturnDT = WS.returnEM_Affected_Cover(strSubscriberName, strSubscriberPassword, strSubscriberCode, strProduct + strProductType + strEscalation, strBaseRisk, strRiskModifier, decPremium.ToString(), RadDatePickerQuoteDate.SelectedDate.ToString(), RadTxtMagnumID.Text.Trim(), "", RadNumericTxtEMLoadingLife.Text);
                                        //    }

                                        //    foreach (DataRow row in ReturnDT.Rows)
                                        //    {
                                        //        if (row["Cover"].ToString() == "")
                                        //        {
                                        //            decCover = 0;
                                        //        }
                                        //        else
                                        //        {
                                        //            decCover = Convert.ToDecimal(row["UnroudedCover"].ToString());
                                        //        }
                                        //    }

                                        //    ////Match closest cover amount in the CoverRounding table
                                        //    //#region "Match closest cover amount in the CoverRounding table"
                                        //    //sqlCommandX = new SqlCommand();
                                        //    //sqlCommandX.Connection = sqlConnectionX;
                                        //    //sqlCommandX.CommandType = CommandType.StoredProcedure;
                                        //    //sqlCommandX.CommandText = "spx_SELECT_CoverRounding";

                                        //    //sqlParam = new SqlParameter("CoverAmount", decCover);
                                        //    //sqlCommandX.Parameters.Add(sqlParam);

                                        //    //sqlDR = sqlCommandX.ExecuteReader();
                                        //    //while (sqlDR.Read())
                                        //    //{
                                        //    //    decCover = Convert.ToDecimal(sqlDR.GetValue(0).ToString());
                                        //    //}

                                        //    //sqlDR.Close();
                                        //    //sqlCommandX.Cancel();
                                        //    //sqlCommandX.Dispose();
                                        //    //#endregion

                                        //    decPremium = decPremium + decPremiumFixedFee;
                                        //    #endregion

                                        //    RadNumericTxtOption3RandValue.Text = decCover.ToString();
                                        //    RadNumericTxtOption3Premium.Text = decPremium.ToString();
                                        //}
                                        #endregion
                                    }
                                    else
                                    {
                                        hideOption3Life();
                                        hideOption3Disability();
                                        lblAlso.Visible = false;
                                    }

                                }
                                #endregion
                                break;
                            case 4:
                                #region "Button 4"
                                hideOption4Life();
                                hideOption4Disability();

                                RadNumericTxtOption4RandValue.Visible = true;
                                RadNumericTxtOption4RandValue.Text = decCover.ToString();
                                lblOp4_1.Visible = true;
                                RadNumericTxtOption4Premium.Visible = true;
                                RadNumericTxtOption4Premium.Text = decPremium.ToString();
                                lblOp4_2.Visible = true;
                                //if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                                {
                                    lblOp4_2.Text = "rands per month";
                                    //2015-10-12 - removed the below because it was making the premium and the total the same value on the quote letter
                                    //decimal decTotal = 0;

                                    //if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    //{
                                    //    decTotal = (decPremium - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee))));
                                    //}
                                    //else
                                    //{
                                    //    decTotal = (decPremium + decPremiumDisability);
                                    //}

                                    //RadNumericTxtOption4Total.Text = decTotal.ToString();
                                    RadNumericTxtOption4Total.Text = decPremium.ToString();
                                }
                                else
                                {
                                    if (RadioButtonQuoteDisNo.Checked == false)
                                    {
                                        lblOp4_2.Text = "rands per month AND";
                                        //decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        decimal decTotal = 0;

                                        if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                        {
                                            decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        }
                                        else
                                        {
                                            decTotal = (decPremium + decPremiumDisability);
                                        }
                                        RadNumericTxtOption4Total.Text = decTotal.ToString();
                                    }
                                    else
                                    {
                                        lblOp4_2.Text = "rands per month";
                                        //2015-10-12 - removed the below because it was making the premium and the total the same value on the quote letter
                                        //decimal decTotal = ((decPremium)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        //RadNumericTxtOption4Total.Text = decTotal.ToString();
                                        RadNumericTxtOption4Total.Text = decPremium.ToString();
                                    }
                                }

                                if (Convert.ToDecimal(RadNumericTxtOption4Total.Text) < 130)
                                {
                                    lblOp4_Problem.Text = "The total premium is too low, please try a different cover values";
                                    lblOp4_Problem.Visible = true;

                                    hideOption4Life();
                                    hideOption4Disability();
                                }
                                else
                                { 
                                    lblOp4_Problem.Text = "";
                                    lblOp4_Problem.Visible = false;
                                }

                                #endregion
                                break;
                            case 5:
                                #region "Button 5"
                                hideOption5Life();
                                hideOption5Disability();

                                RadNumericTxtOption5RandValue.Visible = true;
                                RadNumericTxtOption5RandValue.Text = decCover.ToString();
                                lblOp5_1.Visible = true;
                                RadNumericTxtOption5Premium.Visible = true;

                                RadNumericTxtOption5Premium.Text = decPremium.ToString();
                                //RadNumericTxtOption5Premium.Text = decDesiredContribution.ToString();


                                lblOp5_2.Visible = true;
                                //if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                                if ((PanelDisability.GroupingText.Contains("Unavailable") == true) && (RadComboBoxTypeBenefitDisability.SelectedItem.Text == "FDB"))
                                {
                                    lblOp5_2.Text = "rands per month";
                                    decimal decTotal = 0;
                                    //2015-10-12 - removed the below because it was making the premium and the total the same value on the quote letter
                                    //if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    //{
                                    //    decTotal = (decPremium - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee))));
                                    //}
                                    //else
                                    //{
                                    //    decTotal = (decPremium + decPremiumDisability);
                                    //}

                                    //RadNumericTxtOption5Total.Text = decTotal.ToString();
                                    RadNumericTxtOption5Total.Text = decPremium.ToString();
                                }
                                else
                                {
                                    if (RadioButtonQuoteDisNo.Checked == false)
                                    {
                                        lblOp5_2.Text = "rands per month AND";
                                        //decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        ////decimal decTotal = ((decDesiredContribution + decDesiredContribution)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));  
                                        decimal decTotal = 0;

                                        if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                        {
                                            decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        }
                                        else
                                        {
                                            decTotal = (decPremium + decPremiumDisability);
                                        }

                                        RadNumericTxtOption5Total.Text = decTotal.ToString();
                                    }
                                    else
                                    {
                                        lblOp5_2.Text = "rands per month";
                                        //2015-10-12 - removed the below because it was making the premium and the total the same value on the quote letter
                                        //decimal decTotal = ((decPremium)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        //RadNumericTxtOption5Total.Text = decTotal.ToString();
                                        RadNumericTxtOption5Total.Text = decPremium.ToString();
                                    }
                                }
                                #endregion
                                break;
                        }

                    }
                    else
                    {
                        switch (buttonNumber)
                        {
                            case 1:
                                hideOption1Life();
                                break;
                            case 2:
                                hideOption2Life();
                                break;
                            case 3:
                                hideOption3Life();
                                break;
                            case 4:
                                hideOption4Life();
                                break;
                            case 5:
                                hideOption5Life();
                                break;
                        }
                    }
                }

                sqlConnectionX.Close();

                if (RadioButtonQuoteDisNo.Checked == false)
                {
                    if ((PanelDisability.GroupingText.Contains("Unavailable") == false) || (RadComboBoxTypeBenefitDisability.SelectedItem.Text != "FDB"))
                    {
                        decimal decTotal = 0;

                        switch (buttonNumber)
                        {
                            case 1:
                                #region "Button 1"
                                RadNumericTxtOption1DisCover.Visible = true;
                                RadNumericTxtOption1DisCover.Text = decCoverDisability.ToString();
                                lblOp1_3.Visible = true;
                                RadNumericTxtOption1DisPremium.Visible = true;
                                RadNumericTxtOption1DisPremium.Text = decPremiumDisability.ToString();
                                lblOp1_4.Visible = true;
                                if (RadioButtonQuoteLifeNo.Checked == true)
                                {
                                    lblOp1_4.Text = "rands per month";
                                    //decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    
                                    if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    {
                                        decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    }
                                    else
                                    {
                                        decTotal = (decPremium + decPremiumDisability);
                                    }

                                    RadNumericTxtOption1Total.Text = decTotal.ToString();
                                    hideOption1Life();
                                    lblOp1_4.Visible = false;
                                    RadNumericTxtOption1Total.Visible = false;
                                    lblOp1_5.Visible = false;
                                }
                                else
                                {
                                    lblOp1_4.Text = "rands per month. So, that's a total monthly premium of";
                                    RadNumericTxtOption1Total.Visible = true;
                                    lblOp1_5.Visible = true;
                                }
                                #endregion
                                break;
                            case 2:
                                #region "Button 2"
                                if (Convert.ToInt32(RadNumericTxtOption1Total.Text) > 130)
                                {
                                    //decimal decTotal;
                                    decimal decTotalTemp;

                                    RadNumericTxtOption2DisCover.Visible = true;
                                    RadNumericTxtOption2DisCover.Text = decCoverDisability.ToString();
                                    lblOp2_3.Visible = true;
                                    RadNumericTxtOption2DisPremium.Visible = true;
                                    RadNumericTxtOption2DisPremium.Text = decPremiumDisability.ToString();
                                    lblOp2_4.Visible = true;
                                    if (RadioButtonQuoteLifeNo.Checked == true)
                                    {
                                        lblOp2_4.Text = "rands per month";
                                        //decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));

                                        if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                        {
                                            decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        }
                                        else
                                        {
                                            decTotal = (decPremium + decPremiumDisability);
                                        }

                                        RadNumericTxtOption2Total.Text = decTotal.ToString();
                                        hideOption2Life();
                                        lblOp2_4.Visible = false;
                                        RadNumericTxtOption2Total.Visible = false;
                                        lblOp2_5.Visible = false;
                                        
                                    }
                                    else
                                    {
                                        lblOp2_4.Text = "rands per month. So, that's a total monthly premium of";
                                        RadNumericTxtOption2Total.Visible = true;
                                        lblOp2_5.Visible = true;
                                    }

                                    decTotalTemp = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    //dont show option 2 if the total is the same as option 1
                                    if (decTotalTemp == Convert.ToInt32(RadNumericTxtOption1Total.Text))
                                    {
                                        //hideOption2Life();
                                        hideOption2Disability();
                                        lblSuitable0.Visible = false;
                                        lblOr.Visible = false;
                                        RadNumericTxtOption2Total.Text = "0";
                                    }
                                }
                                else
                                {
                                    hideOption2Life();
                                    hideOption2Disability();
                                    RadNumericTxtOption2Total.Text = "0";
                                }
                                #endregion
                                break;
                            case 3:
                                #region "Button 3"
                                if (Convert.ToInt32(RadNumericTxtOption2Total.Text) > 130)
                                {
                                    RadNumericTxtOption3DisCover.Visible = true;
                                    RadNumericTxtOption3DisCover.Text = decCoverDisability.ToString();
                                    lblOp3_3.Visible = true;
                                    RadNumericTxtOption3DisPremium.Visible = true;
                                    RadNumericTxtOption3DisPremium.Text = decPremiumDisability.ToString();
                                    lblOp3_4.Visible = true;
                                    if (RadioButtonQuoteLifeNo.Checked == true)
                                    {
                                        lblOp3_4.Text = "rands per month";
                                        //decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        //decimal decTotalTemp = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        decimal decTotalTemp = 0;

                                        if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                        {
                                            decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                            decTotalTemp = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        }
                                        else
                                        {
                                            decTotal = (decPremium + decPremiumDisability);
                                            decTotalTemp = (decPremium + decPremiumDisability);
                                        }

                                        
                                        RadNumericTxtOption3Total.Text = decTotal.ToString();
                                        hideOption3Life();
                                        lblOp3_4.Visible = false;
                                        RadNumericTxtOption3Total.Visible = false;
                                        lblOp3_5.Visible = false;
                                        //dont show option 2 if the total is the same as option 1
                                        if (decTotalTemp == Convert.ToInt32(RadNumericTxtOption2Total.Text))
                                        {
                                            //hideOption3Life();
                                            hideOption3Disability();
                                            
                                            RadNumericTxtOption3Total.Text = "0";
                                        }

                                    }
                                    else
                                    {
                                        lblOp3_4.Text = "rands per month. So, that's a total monthly premium of";
                                        RadNumericTxtOption3Total.Visible = true;
                                        lblOp3_5.Visible = true;
                                    }
                                }
                                else
                                {
                                    hideOption3Life();
                                    hideOption3Disability();
                                    RadNumericTxtOption3Total.Text = "0";
                                }
                                #endregion
                                break;
                            case 4:
                                #region "Button 4"
                                if (Convert.ToDecimal(RadNumericTxtOption4Total.Text) >= 130)
                                {
                                    RadNumericTxtOption4DisCover.Visible = true;
                                    RadNumericTxtOption4DisCover.Text = decCoverDisability.ToString();
                                    lblOp4_3.Visible = true;
                                    RadNumericTxtOption4DisPremium.Visible = true;
                                    RadNumericTxtOption4DisPremium.Text = decPremiumDisability.ToString();
                                    lblOp4_4.Visible = true;


                                    if (RadioButtonQuoteLifeNo.Checked == true)
                                    {                                        
                                        lblOp4_4.Text = "rands per month";
                                        decTotal = 0;

                                        if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                        {
                                            decTotal = (decPremium - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee))));
                                        }
                                        else
                                        {
                                            decTotal = (decPremium + decPremiumDisability);
                                        }

                                        RadNumericTxtOption4Total.Text = decTotal.ToString();

                                        hideOption4Life();
                                        RadNumericTxtOption4Total.Visible = false;
                                        lblOp4_5.Visible = false;
                                    }
                                    else
                                    {
                                        lblOp4_4.Text = "rands per month. So, that's a total monthly premium of";
                                        //decTotal = ((decPremium)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                        RadNumericTxtOption4Total.Text = decTotal.ToString();

                                        RadNumericTxtOption4Total.Visible = true;
                                        lblOp4_5.Visible = true;
                                    }
                                }
                                
                                #endregion
                                break;
                            case 5:
                                #region "Button 5"
                                RadNumericTxtOption5DisCover.Visible = true;
                                RadNumericTxtOption5DisCover.Text = decCoverDisability.ToString();
                                lblOp5_3.Visible = true;
                                RadNumericTxtOption5DisPremium.Visible = true;
                                //RadNumericTxtOption5DisPremium.Text = decPremium.ToString();
                                //RadNumericTxtOption5DisPremium.Text = decDesiredContribution.ToString();
                                RadNumericTxtOption5DisPremium.Text = decPremiumDisability.ToString();
                                lblOp5_4.Visible = true;
                                if (RadioButtonQuoteLifeNo.Checked == true)
                                {
                                    hideOption5Life();
                                    lblOp5_4.Text = "rands per month";
                                    decTotal = 0;

                                    if ((RadioButtonQuoteLifeNo.Checked == false && RadioButtonQuoteDisNo.Checked == false))
                                    {
                                        //decTotal = (decPremium - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee))));
                                        decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    }
                                    else
                                    {
                                        decTotal = (decPremium + decPremiumDisability);
                                    }

                                    RadNumericTxtOption5Total.Text = decTotal.ToString();
                                    RadNumericTxtOption5Total.Visible = false;
                                    lblOp5_5.Visible = false;
                                }
                                else
                                {
                                    lblOp5_4.Text = "rands per month. So, that's a total monthly premium of";
                                    decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                                    RadNumericTxtOption5Total.Text = decTotal.ToString();

                                    RadNumericTxtOption5Total.Visible = true;
                                    lblOp5_5.Visible = true;
                                }

                                #endregion
                                break;
                        }
                    }
                    else
                    {
                        switch (buttonNumber)
                        {
                            case 1:
                                hideOption1Disability();
                                break;
                            case 2:
                                hideOption2Disability();
                                break;
                            case 3:
                                hideOption3Disability();
                                break;
                            case 4:
                                hideOption4Disability();
                                break;
                            case 5:
                                hideOption5Disability();
                                break;
                        }
                    }
                }
                else
                {
                    switch (buttonNumber)
                    {
                        case 1:
                            hideOption1Disability();
                            break;
                        case 2:
                            hideOption2Disability();
                            break;
                        case 3:
                            hideOption3Disability();
                            break;
                        case 4:
                            hideOption4Disability();
                            break;
                        case 5:
                            hideOption5Disability();
                            break;
                    }
                }

                #region "old code"

                //if (RadioButtonQuoteLifeNo.Checked == false)
                //{
                //    if (PanelLife.GroupingText.Contains("Unavailable") == false)
                //    {
                //        switch (buttonNumber)
                //        {
                //            case 1:
                //                #region "Button 1"
                //                RadNumericTxtOption1RandValue.Visible = true;
                //                RadNumericTxtOption1RandValue.Text = decCover.ToString();
                //                lblOp1_1.Visible = true;
                //                RadNumericTxtOption1Premium.Visible = true;
                //                RadNumericTxtOption1Premium.Text = decPremium.ToString();
                //                lblOp1_2.Visible = true;
                //                if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                //                {
                //                    lblOp1_2.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp1_2.Text = "rands per month AND";
                //                    decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                //                    RadNumericTxtOption1Total.Text = decTotal.ToString();
                //                }
                //                #endregion
                //                break;
                //            case 2:
                //                #region "Button 2"
                //                RadNumericTxtOption2RandValue.Visible = true;
                //                RadNumericTxtOption2RandValue.Text = decCover.ToString();
                //                lblOp2_1.Visible = true;
                //                RadNumericTxtOption2Premium.Visible = true;
                //                RadNumericTxtOption2Premium.Text = decPremium.ToString();
                //                lblOp2_2.Visible = true;
                //                if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                //                {
                //                    lblOp2_2.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp2_2.Text = "rands per month AND";
                //                    decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                //                    RadNumericTxtOption2Total.Text = decTotal.ToString();
                //                }
                //                #endregion
                //                break;
                //            case 3:
                //                #region "Button 3"
                //                RadNumericTxtOption3RandValue.Visible = true;
                //                RadNumericTxtOption3RandValue.Text = decCover.ToString();
                //                lblOp3_1.Visible = true;
                //                RadNumericTxtOption3Premium.Visible = true;
                //                RadNumericTxtOption3Premium.Text = decPremium.ToString();
                //                lblOp3_2.Visible = true;
                //                if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                //                {
                //                    lblOp3_2.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp3_2.Text = "rands per month AND";
                //                    decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                //                    RadNumericTxtOption3Total.Text = decTotal.ToString();
                //                }
                //                #endregion
                //                break;
                //            case 4:
                //                #region "Button 4"
                //                RadNumericTxtOption4RandValue.Visible = true;
                //                RadNumericTxtOption4RandValue.Text = decCover.ToString();
                //                lblOp4_1.Visible = true;
                //                RadNumericTxtOption4Premium.Visible = true;
                //                RadNumericTxtOption4Premium.Text = decPremium.ToString();
                //                lblOp4_2.Visible = true;
                //                if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                //                {
                //                    lblOp4_2.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp4_2.Text = "rands per month AND";
                //                    decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                //                    RadNumericTxtOption4Total.Text = decTotal.ToString();
                //                }
                //                #endregion
                //                break;
                //            case 5:
                //                #region "Button 5"
                //                RadNumericTxtOption5RandValue.Visible = true;
                //                RadNumericTxtOption5RandValue.Text = decCover.ToString();
                //                lblOp5_1.Visible = true;
                //                RadNumericTxtOption5Premium.Visible = true;

                //                RadNumericTxtOption5Premium.Text = decPremium.ToString();
                //                //RadNumericTxtOption5Premium.Text = decDesiredContribution.ToString();

                //                lblOp5_2.Visible = true;
                //                if (PanelDisability.GroupingText.Contains("Unavailable") == true)
                //                {
                //                    lblOp5_2.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp5_2.Text = "rands per month AND";
                //                    decimal decTotal = ((decPremium + decPremiumDisability)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));
                //                    //decimal decTotal = ((decDesiredContribution + decDesiredContribution)) - (Convert.ToDecimal(1 * Convert.ToDecimal(strControlFee)));  
                //                    RadNumericTxtOption5Total.Text = decTotal.ToString();
                //                }
                //                #endregion
                //                break;
                //        }

                //    }
                //    else
                //    {
                //        switch (buttonNumber)
                //        {
                //            case 1:
                //                hideOption1Life();
                //                break;
                //            case 2:
                //                hideOption2Life();
                //                break;
                //            case 3:
                //                hideOption3Life();
                //                break;
                //            case 4:
                //                hideOption4Life();
                //                break;
                //            case 5:
                //                hideOption5Life();
                //                break;
                //        }
                //    }
                //}

                //if (RadioButtonQuoteDisNo.Checked == false)
                //{
                //    if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                //    {
                //        switch (buttonNumber)
                //        {
                //            case 1:
                //                #region "Button 1"
                //                RadNumericTxtOption1DisCover.Visible = true;
                //                RadNumericTxtOption1DisCover.Text = decCoverDisability.ToString();
                //                lblOp1_3.Visible = true;
                //                RadNumericTxtOption1DisPremium.Visible = true;
                //                RadNumericTxtOption1DisPremium.Text = decPremiumDisability.ToString();
                //                lblOp1_4.Visible = true;                                
                //                if (RadioButtonQuoteLifeNo.Checked == true)
                //                {
                //                    lblOp1_4.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp1_4.Text = "rands per month AND";
                //                    RadNumericTxtOption1Total.Visible = true;
                //                    lblOp1_5.Visible = true;
                //                }
                //                #endregion
                //                break;
                //            case 2:
                //                #region "Button 2"
                //                RadNumericTxtOption2DisCover.Visible = true;
                //                RadNumericTxtOption2DisCover.Text = decCoverDisability.ToString();
                //                lblOp2_3.Visible = true;
                //                RadNumericTxtOption2DisPremium.Visible = true;
                //                RadNumericTxtOption2DisPremium.Text = decPremiumDisability.ToString();
                //                lblOp2_4.Visible = true;                                
                //                if (RadioButtonQuoteLifeNo.Checked == true)
                //                {
                //                    lblOp2_4.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp2_4.Text = "rands per month AND";
                //                    RadNumericTxtOption2Total.Visible = true;
                //                    lblOp2_5.Visible = true;
                //                }
                //                #endregion
                //                break;
                //            case 3:
                //                #region "Button 3"
                //                RadNumericTxtOption3DisCover.Visible = true;
                //                RadNumericTxtOption3DisCover.Text = decCoverDisability.ToString();
                //                lblOp3_3.Visible = true;
                //                RadNumericTxtOption3DisPremium.Visible = true;
                //                RadNumericTxtOption3DisPremium.Text = decPremiumDisability.ToString();
                //                lblOp3_4.Visible = true;                              
                //                if (RadioButtonQuoteLifeNo.Checked == true)
                //                {
                //                    lblOp3_4.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp3_4.Text = "rands per month AND";
                //                    RadNumericTxtOption3Total.Visible = true;
                //                    lblOp3_5.Visible = true;
                //                }
                //                #endregion
                //                break;
                //            case 4:
                //                #region "Button 4"
                //                RadNumericTxtOption4DisCover.Visible = true;
                //                RadNumericTxtOption4DisCover.Text = decCoverDisability.ToString();
                //                lblOp4_3.Visible = true;
                //                RadNumericTxtOption4DisPremium.Visible = true;
                //                RadNumericTxtOption4DisPremium.Text = decPremiumDisability.ToString();
                //                lblOp4_4.Visible = true;                                
                //                if (RadioButtonQuoteLifeNo.Checked == true)
                //                {
                //                    lblOp4_4.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp4_4.Text = "rands per month AND";
                //                    RadNumericTxtOption4Total.Visible = true;
                //                    lblOp4_5.Visible = true;
                //                }
                //                #endregion
                //                break;
                //            case 5:
                //                #region "Button 5"
                //                RadNumericTxtOption5DisCover.Visible = true;
                //                RadNumericTxtOption5DisCover.Text = decCoverDisability.ToString();
                //                lblOp5_3.Visible = true;
                //                RadNumericTxtOption5DisPremium.Visible = true;
                //                //RadNumericTxtOption5DisPremium.Text = decPremium.ToString();
                //                //RadNumericTxtOption5DisPremium.Text = decDesiredContribution.ToString();
                //                RadNumericTxtOption5DisPremium.Text = decPremiumDisability.ToString();
                //                lblOp5_4.Visible = true;                                
                //                if (RadioButtonQuoteLifeNo.Checked == true)
                //                {
                //                    lblOp5_4.Text = "rands per month";
                //                }
                //                else
                //                {
                //                    lblOp5_4.Text = "rands per month AND";
                //                    RadNumericTxtOption5Total.Visible = true;
                //                    lblOp5_5.Visible = true;
                //                }

                //                #endregion
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        switch (buttonNumber)
                //        {
                //            case 1:
                //                hideOption1Disability();
                //                break;
                //            case 2:
                //                hideOption2Disability();
                //                break;
                //            case 3:
                //                hideOption3Disability();
                //                break;
                //            case 4:
                //                hideOption4Disability();
                //                break;
                //            case 5:
                //                hideOption5Disability();
                //                break;
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;   
            }
        }

        public decimal calculation(decimal a, decimal b)
        {
            decimal c = a - b;
            if (c < 0)
            {
                return 0;
            }
            else
            {
                return c;
            }
        }

        private void SaveQuoteOptionInfo(int buttonNumber)
        {
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
                sqlCommandX.CommandText = "spx_INSERT_QuoteOptionAuditTrail";

                #region "Parameters"
                decimal decCoverLife = 0; decimal decPremiumLife = 0; decimal decTotalLife = 0;
                decimal decCoverDisability = 0; decimal decPremiumDisability = 0;
                decimal decTotal = 0; string strDisabilityType = string.Empty;
                bool blnSelected = false;

                if (RadioButtonTypeOfDisADW.Checked == true)
                    strDisabilityType = "ADW";
                if (RadioButtonTypeOfDisOCC.Checked == true)
                    strDisabilityType = "OCC";

                #region "Switch"
                switch (buttonNumber)
                {

                    case 1:                        
                        decCoverLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption1RandValue.Text) ? "0" : RadNumericTxtOption1RandValue.Text);
                        decPremiumLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption1Premium.Text) ? "0" : RadNumericTxtOption1Premium.Text);

                        decCoverDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption1DisCover.Text) ? "0" : RadNumericTxtOption1DisCover.Text);
                        decPremiumDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption1DisPremium.Text) ? "0" : RadNumericTxtOption1DisPremium.Text);

                        decTotal = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption1Total.Text) ? "0" : RadNumericTxtOption1Total.Text);
                        blnSelected = RadioButtonOption1.Checked;

                        break;
                    case 2:
                        decCoverLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption2RandValue.Text) ? "0" : RadNumericTxtOption2RandValue.Text);
                        decPremiumLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption2Premium.Text) ? "0" : RadNumericTxtOption2Premium.Text);

                        decCoverDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption2DisCover.Text) ? "0" : RadNumericTxtOption2DisCover.Text);
                        decPremiumDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption2DisPremium.Text) ? "0" : RadNumericTxtOption2DisPremium.Text);

                        decTotal = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption2Total.Text) ? "0" : RadNumericTxtOption2Total.Text);
                        blnSelected = RadioButtonOption2.Checked;
                        break;
                    case 3:                        
                        decCoverLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption3RandValue.Text) ? "0" : RadNumericTxtOption3RandValue.Text);
                        decPremiumLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption3Premium.Text) ? "0" : RadNumericTxtOption3Premium.Text);                        

                        decCoverDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption3DisCover.Text) ? "0" : RadNumericTxtOption3DisCover.Text);
                        decPremiumDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption3DisPremium.Text) ? "0" : RadNumericTxtOption3DisPremium.Text);

                        decTotal = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption3Total.Text) ? "0" : RadNumericTxtOption3Total.Text);
                        blnSelected = RadioButtonOption3.Checked;
                        break;
                    case 4:
                        decCoverLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption4RandValue.Text) ? "0" : RadNumericTxtOption4RandValue.Text);
                        decPremiumLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption4Premium.Text) ? "0" : RadNumericTxtOption4Premium.Text);

                        decCoverDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption4DisCover.Text) ? "0" : RadNumericTxtOption4DisCover.Text);
                        decPremiumDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption4DisPremium.Text) ? "0" : RadNumericTxtOption4DisPremium.Text);

                        decTotal = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption4Total.Text) ? "0" : RadNumericTxtOption4Total.Text);
                        blnSelected = RadioButtonOption4.Checked;
                        break;
                    case 5:
                        decCoverLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption5RandValue.Text) ? "0" : RadNumericTxtOption5RandValue.Text);
                        decPremiumLife = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption5Premium.Text) ? "0" : RadNumericTxtOption5Premium.Text);

                        decCoverDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption5DisCover.Text) ? "0" : RadNumericTxtOption5DisCover.Text);
                        decPremiumDisability = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption5DisPremium.Text) ? "0" : RadNumericTxtOption5DisPremium.Text);

                        decTotal = Convert.ToDecimal(string.IsNullOrEmpty(RadNumericTxtOption5Total.Text) ? "0" : RadNumericTxtOption5Total.Text);
                        blnSelected = RadioButtonOption5.Checked;
                        break;
                }
                #endregion

                sqlParam = new SqlParameter("QuoteAuditTrailID", Convert.ToInt32(HiddenFieldQuoteAuditID.Value));
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("OptionNumber", buttonNumber);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("CoverLife", decCoverLife);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("PremiumLife", decPremiumLife);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("CoverDisability", decCoverDisability);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("PremiumDisability", decPremiumDisability);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("Total", decTotal);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("DisabilityType", strDisabilityType);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("Selected", blnSelected);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlCommandX.ExecuteNonQuery();

                #endregion

                sqlConnectionX.Close();

            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;   
            }

        }

        protected void RadBtnOption4_Click(object sender, EventArgs e)
        {
            bool blnLifeAvailable = false;
            bool blnDisabilityAvailable = false;
            bool blnContinue = true;


            try
            {
                //Check if client qualifies for life and/or disability 
                if (PanelLife.GroupingText.Contains("Unavailable") == false)
                {
                    blnLifeAvailable = true;
                }

                if (RadioButtonQuoteLifeNo.Checked == true)
                {
                    blnLifeAvailable = false;
                }

                if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                {
                    blnDisabilityAvailable = true;
                }

                if (RadioButtonQuoteDisNo.Checked == true)
                {
                    blnDisabilityAvailable = false;
                }

                if ((blnLifeAvailable == true) && (blnDisabilityAvailable == true))
                {
                    if ((RadNumericTxtCoverLife.Text.Trim() == "") || (RadNumericTxtCoverAmnDis.Text.Trim() == ""))
                    {
                        RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the cover for both life and disability sections ',300,100,'Data required');");
                        blnContinue = false;
                    }
                }

                if ((blnLifeAvailable == true) && (blnDisabilityAvailable == false))
                {
                    if (RadNumericTxtCoverLife.Text.Trim() == "")
                    {
                        RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the cover for the life section',300,100,'Data required');");
                        blnContinue = false;
                    }
                }

                if ((RadioButtonQuoteLifeNo.Checked == true) && (RadioButtonQuoteDisNo.Checked == true))
                {
                    RadAjaxManager1.ResponseScripts.Add("radalert('You have selected No Quote for life and disability so the system will not do anything',300,100,'Nothing to do');");
                    blnContinue = false;
                }
                else
                {
                    if ((blnLifeAvailable == false) && (blnDisabilityAvailable == false))
                    {
                        if (RadNumericTxtCoverAmnDis.Text.Trim() == "")
                        {
                            RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the cover for the disability section',300,100,'Data required');");
                            blnContinue = false;
                        }
                    }
                }

                if (blnContinue == true)
                {
                    ProcessOptionButton(4);
                    SaveQuoteOptionInfo(4);
                    populateSummaryGrid();
                    //RadButtonGenerateLetter.Enabled = true;
                    lblRequalify.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;
                //throw;
            }           
        }

        protected void RadBtnOption5_Click(object sender, EventArgs e)
        {
            bool blnLifeAvailable = false;
            bool blnDisabilityAvailable = false;
            bool blnContinue = true;


            try
            {
                //Check if client qualifies for life and/or disability 
                if (PanelLife.GroupingText.Contains("Unavailable") == false)
                {
                    blnLifeAvailable = true;
                }

                if (RadioButtonQuoteLifeNo.Checked == true)
                {
                    blnLifeAvailable = false;
                }

                if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                {
                    blnDisabilityAvailable = true;
                }

                if (RadioButtonQuoteDisNo.Checked == true)
                {
                    blnDisabilityAvailable = false;
                }

                if ((blnLifeAvailable == true) && (blnDisabilityAvailable == true))
                {
                    if ((RadNumericTxtPremiumLife.Text.Trim() == "") || (RadNumericTxtPremiumDis.Text.Trim() == ""))
                    {
                        RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the premium for both life and disability sections ',300,100,'Data required');");
                        blnContinue = false;
                    }
                }

                if ((blnLifeAvailable == true) && (blnDisabilityAvailable == false))
                {
                    if (RadNumericTxtPremiumLife.Text.Trim() == "")
                    {
                        RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the premium for the life section',300,100,'Data required');");
                        blnContinue = false;
                    }
                }

                if ((blnLifeAvailable == false) && (blnDisabilityAvailable == true))
                {
                    if (RadNumericTxtPremiumDis.Text.Trim() == "")
                    {
                        RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the premium for the disability section',300,100,'Data required');");
                        blnContinue = false;
                    }
                }

                if ((RadioButtonQuoteLifeNo.Checked == true) && (RadioButtonQuoteDisNo.Checked == true))
                {
                    RadAjaxManager1.ResponseScripts.Add("radalert('You have selected No Quote for life and disability so the system will not do anything',300,100,'Nothing to do');");
                    blnContinue = false;
                }
                else
                {
                    if ((blnLifeAvailable == false) && (blnDisabilityAvailable == false))
                    {
                        if (RadNumericTxtPremiumDis.Text.Trim() == "")
                        {
                            RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the premium for the disability section',300,100,'Data required');");
                            blnContinue = false;
                        }
                    }
                }


                //if (RadNumericTxtDesireContribution.Text.Trim() == "")
                //{
                //    RadAjaxManager1.ResponseScripts.Add("radalert('Please enter a value in for the desired monthly contribution',300,100,'Data required');");
                //    blnContinue = false;
                //}

                if (blnContinue == true)
                {
                    decimal decPremiumLife = 0;
                    decimal decPremiumDis = 0;

                    if (RadioButtonQuoteLifeNo.Checked == false)
                    {
                        decPremiumLife = decPremiumLife = Convert.ToDecimal(RadNumericTxtPremiumLife.Text);
                    }

                    if (RadioButtonQuoteDisNo.Checked == false)
                    {
                        if (PanelDisability.GroupingText.Contains("Unavailable") == false)
                        {
                            decPremiumDis = decPremiumDis = Convert.ToDecimal(RadNumericTxtPremiumDis.Text);
                        }
                        else
                        {
                            decPremiumDis = 0;
                        }
                    }

                    if ((decPremiumLife > 0) && (decPremiumDis > 0))
                    {
                        if ((decPremiumLife + decPremiumDis) < 130)
                        {
                            RadAjaxManager1.ResponseScripts.Add("radalert('The combined premium for life and disability can not be less than R130',300,100,'Combined premium too low');");
                            blnContinue = false;
                        }
                    }
                    else
                    {
                        if (decPremiumLife > 0)
                        {
                            if (decPremiumLife < 130)
                            {
                                RadAjaxManager1.ResponseScripts.Add("radalert('The premium for life can not be less than R130',300,100,'Combined premium too low');");
                                blnContinue = false;
                            }
                        }

                        if (decPremiumDis > 0)
                        {
                            if (decPremiumDis < 130)
                            {
                                RadAjaxManager1.ResponseScripts.Add("radalert('The premium for disability can not be less than R130',300,100,'Combined premium too low');");
                                blnContinue = false;
                            }
                        }
                    }

                    if (blnContinue == true)
                    {
                        ProcessOptionButton(5);
                        SaveQuoteOptionInfo(5);
                        populateSummaryGrid();
                        //RadButtonGenerateLetter.Enabled = true;
                        lblRequalify.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;
                //throw;
            }
        }       

        protected void RadButtonLoadQuote_Click(object sender, EventArgs e)
        {
            try
            {
                RadWindowManager1.Windows.Clear();
                RadWindow newWindow = new RadWindow();
                newWindow.NavigateUrl = "QuoteLoad.aspx?MagnumID=" + RadTxtMagnumID.Text;
                newWindow.Modal = true;
                newWindow.Height = 600;
                newWindow.Width = 850;
                //newWindow.VisibleStatusbar = false;
                newWindow.DestroyOnClose = true;
                newWindow.Title = "Quote selector";
                newWindow.VisibleOnPageLoad = true;
                newWindow.Behaviors = WindowBehaviors.Resize | WindowBehaviors.Close | WindowBehaviors.Move;
                //newWindow.Behaviors = WindowBehaviors.Close;
                RadWindowManager1.Windows.Add(newWindow);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
               // throw;
            }
        }

        protected void RadBtnCheckSession_Click(object sender, EventArgs e)
        {
            try
            {
                lblSessionCheck.BackColor = System.Drawing.Color.Green;
                lblSessionCheck.ForeColor = System.Drawing.Color.White;
                lblSessionCheck.Text = "Session is good";
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                // throw;
            }
        }

        protected void RadBtnClearQuote_Click(object sender, EventArgs e)
        {
            try
            {
                RadWindowManager1.Windows.Clear();
                RadAjaxManager1.ResponseScripts.Add("radconfirm('Warning: You are about to clear the quote data on the screen. Do you want to continue?', confirmClearQuoteFn);");
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                // throw;
            }
        }

        protected void RadButtonGenerateLetter_Click(object sender, EventArgs e)
        {
            try
            {
                RadWindowManager1.Windows.Clear();
                RadWindow newWindow = new RadWindow();
                newWindow.NavigateUrl = "QuoteGenerateLetter.aspx?MagnumID=" + RadTxtMagnumID.Text + "&QuoteAuditID=" + HiddenFieldQuoteAuditID.Value.ToString();
                newWindow.Modal = true;
                newWindow.Height = 800;
                newWindow.Width = 850;
                //newWindow.VisibleStatusbar = false;
                newWindow.DestroyOnClose = true;
                newWindow.Title = "Generate Quote letter";
                newWindow.VisibleOnPageLoad = true;
                newWindow.Behaviors = WindowBehaviors.Resize | WindowBehaviors.Close | WindowBehaviors.Move;
                //newWindow.Behaviors = WindowBehaviors.Close;
                RadWindowManager1.Windows.Add(newWindow);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                // throw;
            }
        }

        private void Update_QuoteOptionAuditTrail(int buttonNumber, bool blnSelected)
        {
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
                sqlCommandX.CommandText = "spx_Update_QuoteOptionAuditTrail";

                #region "Parameters"

                sqlParam = new SqlParameter("QuoteAuditTrailID", Convert.ToInt32(HiddenFieldQuoteAuditID.Value));
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("OptionNumber", buttonNumber);
                sqlCommandX.Parameters.Add(sqlParam);               
                sqlParam = new SqlParameter("Selected", blnSelected);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlCommandX.ExecuteNonQuery();

                #endregion

                sqlConnectionX.Close();

            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                lblInfo2.Text = ex.Message;   
            }

        }

        protected void RadioButtonOption1_CheckedChanged(object sender, EventArgs e)
        {
            Update_QuoteOptionAuditTrail(1, RadioButtonOption1.Checked);
        }

        protected void RadioButtonOption2_CheckedChanged(object sender, EventArgs e)
        {
            Update_QuoteOptionAuditTrail(2, RadioButtonOption2.Checked);
        }

        protected void RadioButtonOption3_CheckedChanged(object sender, EventArgs e)
        {
            Update_QuoteOptionAuditTrail(3, RadioButtonOption3.Checked);
        }

        protected void RadioButtonOption4_CheckedChanged(object sender, EventArgs e)
        {
            Update_QuoteOptionAuditTrail(4, RadioButtonOption4.Checked);
        }

        protected void RadioButtonOption5_CheckedChanged(object sender, EventArgs e)
        {
            Update_QuoteOptionAuditTrail(5, RadioButtonOption5.Checked);
        }

        protected void RadButtonSendLetter_Click(object sender, EventArgs e)
        {

        }

        protected void RadMonthYearPicker1_ViewCellCreated(object sender, MonthYearViewCellCreatedEventArgs e)
        {
            if (e.Cell.CellType == MonthYearViewCellType.MonthCell)
            {
                e.Cell.Style["display"] = "none";
            }
        }
    }
}