using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik;
using Telerik.Reporting.Processing;
using Telerik.Web.UI;

namespace AllLifePricingWeb
{
    public partial class QuoteGenerateLetter : System.Web.UI.Page
    {
        string strMagnumID = string.Empty;
        string strQuoteAuditID = string.Empty;
        Int32 intUserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                //Response.Redirect("~/Login.aspx?ID=9");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mykey", "CloseSessionExpired('9');", true);
            }

            lblInfo.Text = "";
            strMagnumID = Request.QueryString["MagnumID"].ToString();
            strQuoteAuditID = Request.QueryString["QuoteAuditID"].ToString();

            if (!IsPostBack)
            {                
                RadPanelBar1.Items[0].Expanded = true;
                WS.AllLifePrincingClient WS = new WS.AllLifePrincingClient();

                string strSettingValue = string.Empty;

                #region "SQL Section"
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
                    sqlCommandX.CommandText = "spx_SELECT_QuoteOptionAuditTrailByAuditTrailID";

                    sqlParam = new SqlParameter("QuoteAuditTrailID", strQuoteAuditID);
                    sqlCommandX.Parameters.Add(sqlParam);

                    sqlDR = sqlCommandX.ExecuteReader();
                    DataTable dt = new DataTable("AuditTrail");
                    dt.Load(sqlDR);

                    sqlDR.Close();
                    sqlCommandX.Cancel();
                    sqlCommandX.Dispose();
                    sqlConnectionX.Close();

                    Telerik.Web.UI.RadGrid OptionsGrid = (Telerik.Web.UI.RadGrid)RadPanelBar1.FindItemByValue("PanelItem1").FindControl("RadGridOptions");

                    OptionsGrid.DataSource = dt;
                    OptionsGrid.DataBind();
                    sqlConnectionX.Close();
                    sqlConnectionX.Dispose();

                }
                catch (Exception ex)
                {
                    lblInfo.Text = ex.Message;
                }

                #endregion
            }
        }

        private void LoadReport()
        {
            strMagnumID = Request.QueryString["MagnumID"].ToString();
            strQuoteAuditID = Request.QueryString["QuoteAuditID"].ToString();
            string strQuoteOptionAuditTrailIDO1 = "0";
            string strQuoteOptionAuditTrailIDO2 = "0";
            string strQuoteOptionAuditTrailIDO3 = "0";
            string strQuoteOptionAuditTrailIDO4 = "0";
            string strQuoteOptionAuditTrailIDO5 = "0";
            string strMessageMain = string.Empty;
            string strMessage = string.Empty;
            bool blnContinue = true;
            int intcheckCount = 0;

            intUserID = Convert.ToInt32(Session["UserID"]);

            Telerik.Web.UI.RadGrid OptionsGrid = (Telerik.Web.UI.RadGrid)RadPanelBar1.FindItemByValue("PanelItem1").FindControl("RadGridOptions");

            foreach (GridDataItem item in OptionsGrid.Items)
            {
                if ((item.FindControl("CheckBoxSelect") as System.Web.UI.WebControls.CheckBox).Checked == true)
                {
                    intcheckCount += 1;
                    #region "Old"
                    //switch (item.Cells[3].Text.Trim())
                    //{
                    //    case "1":
                    //        if (strQuoteOptionAuditTrailIDO1 == "0")
                    //            strQuoteOptionAuditTrailIDO1 = item.Cells[2].Text;
                    //        else
                    //        {
                    //            blnContinue = false;
                    //        }
                    //        break;
                    //    case "2":
                    //        if (strQuoteOptionAuditTrailIDO2 == "0")
                    //            strQuoteOptionAuditTrailIDO2 = item.Cells[2].Text;
                    //        else
                    //        {
                    //            blnContinue = false;
                    //        }
                    //        break;
                    //    case "3":
                    //        if (strQuoteOptionAuditTrailIDO3 == "0")
                    //            strQuoteOptionAuditTrailIDO3 = item.Cells[2].Text;
                    //        else
                    //        {
                    //            blnContinue = false;
                    //        }
                    //        break;
                    //    case "4":
                    //        if (strQuoteOptionAuditTrailIDO4 == "0")
                    //            strQuoteOptionAuditTrailIDO4 = item.Cells[2].Text;
                    //        else
                    //        {
                    //            blnContinue = false;
                    //        }
                    //        break;
                    //    case "5":
                    //        if (strQuoteOptionAuditTrailIDO5 == "0")
                    //            strQuoteOptionAuditTrailIDO5 = item.Cells[2].Text;
                    //        else
                    //        {
                    //            blnContinue = false;
                    //        }
                    //        break;
                    //}
                    #endregion

                    switch (intcheckCount)
                    {
                        case 1:
                            strQuoteOptionAuditTrailIDO1 = item.Cells[2].Text;
                            break;
                        case 2:
                            strQuoteOptionAuditTrailIDO2 = item.Cells[2].Text;
                            break;
                        case 3:
                            strQuoteOptionAuditTrailIDO3 = item.Cells[2].Text;
                            break;
                        case 4:
                            strQuoteOptionAuditTrailIDO4 = item.Cells[2].Text;
                            break;
                        case 5:
                            strQuoteOptionAuditTrailIDO5 = item.Cells[2].Text;
                            break;
                    }
                }                              
            }

            if (intcheckCount > 5)
                blnContinue = false;

            if (blnContinue == false)
            {
                //lblInfo.Text = "You can only select one item for each option, please check the options";                
                lblInfo.Text = "You can only select a maximum of 5 options";
                ReportViewer1.Visible = false;
                ReportViewer1.ReportSource = null;
            }
            else
            {
                RadBtnSaveLetter.Visible = true;
                //RadBtnEmail.Visible = true;

                DataSet DS = new DataSet();

                #region "Old Code"
                ////Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
                ////instanceReportSource.ReportDocument = new Report.QuoteLetter();                
                ////this.ReportViewer1.ReportSource = instanceReportSource;

                //DataTable SummaryDT = new DataTable();
                //DataColumn DT1 = new DataColumn("Scenario");
                //DataColumn DT2 = new DataColumn("Premium [Total]");
                //DataColumn DT3 = new DataColumn("CoverAmount [Life]");
                //DataColumn DT4 = new DataColumn("Premium [Life] incl. DC fee");
                //DataColumn DT5 = new DataColumn("CoverAmount [Disability]");
                //DataColumn DT6 = new DataColumn("Premium [Disability] incl. DC fee");

                ////DT1.DataType = System.Type.GetType("System.String");
                //SummaryDT.Columns.Add(DT1);
                //SummaryDT.Columns.Add(DT2);
                //SummaryDT.Columns.Add(DT3);
                //SummaryDT.Columns.Add(DT4);
                //SummaryDT.Columns.Add(DT5);
                //SummaryDT.Columns.Add(DT6);

                //DataRow row = SummaryDT.NewRow();
                //row[DT1] = "Option 1";
                //row[DT2] = "";
                //row[DT3] = "";
                //row[DT4] = "";
                //row[DT5] = "";
                //row[DT6] = "";
                //SummaryDT.Rows.Add(row);
                //DS.Tables.Add(SummaryDT);
                #endregion

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
                sqlCommandX.CommandText = "spx_SELECT_QuoteDetailByAuditTrailID_Detailed";

                sqlParam = new SqlParameter("QuoteAuditTrailID", strQuoteAuditID);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption1ID", strQuoteOptionAuditTrailIDO1);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption2ID", strQuoteOptionAuditTrailIDO2);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption3ID", strQuoteOptionAuditTrailIDO3);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption4ID", strQuoteOptionAuditTrailIDO4);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption5ID", strQuoteOptionAuditTrailIDO5);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                DataTable dt = new DataTable("Info");
                dt.Load(sqlDR);
                DS.Tables.Add(dt);

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                sqlConnectionX.Close();
                #endregion

                // Creating and configuring the ObjectDataSource component:
                var objectDataSource = new Telerik.Reporting.ObjectDataSource();
                objectDataSource.DataSource = DS; // GetData returns a DataSet with three tables
                //objectDataSource.DataMember = "Product"; /// Indicating the exact table to bind to. If the DataMember is not specified the first data table will be used.
                //objectDataSource.CalculatedFields.Add(new Telerik.Reporting.CalculatedField("FullName", typeof(string), "=Fields.Name + ' ' + Fields.ProductNumber")); // Adding a sample calculated field.

                // Creating a new report
                Telerik.Reporting.Report report = new Telerik.Reporting.Report();

                //Telerik.Reporting.Report report = new Telerik.Reporting.Report();
                if ((dt.Rows[0][36].ToString() == "ok") && (dt.Rows[0][37].ToString() == "ok"))
                {
                    //Both quoteLife and QuoteDisability were selected
                    if ((dt.Rows[0][42].ToString() == "1") && (dt.Rows[0][43].ToString() == "1"))
                    {
                        report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetter"));
                    }

                    //if only QuoteLife was selected
                    if ((dt.Rows[0][42].ToString() == "1") && (dt.Rows[0][43].ToString() == "0"))
                    {
                        report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterLifeOnly"));
                    }

                    //if only QuoteDisability was selected
                    if ((dt.Rows[0][42].ToString() == "0") && (dt.Rows[0][43].ToString() == "1"))
                    {
                        //report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterDisabilityOnly"));
                        report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterDisOnly"));
                        //report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetter"));
                        //report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterLifeOnly"));
                    }

                }

                if (dt.Rows[0][36].ToString() != "ok")
                {
                    report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterDisabilityOnly"));
                }

                if (dt.Rows[0][37].ToString() != "ok")
                {
                    report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterLifeOnly"));
                }

                report.DocumentName = strMagnumID;
                // Assigning the ObjectDataSource component to the DataSource property of the report.
                report.DataSource = objectDataSource;
                // Use the InstanceReportSource to pass the report to the viewer for displaying
                Telerik.Reporting.InstanceReportSource reportSource = new Telerik.Reporting.InstanceReportSource();
                reportSource.ReportDocument = report;
                // Assigning the report to the report viewer.
                ReportViewer1.ReportSource = reportSource;
                ReportViewer1.RefreshReport();
                //ReportViewer1.RefreshData();

                ////Saving
                //string fileName = report.DocumentName + ".PDF";
                //string path = System.IO.Path.GetTempPath();
                //string filePath = System.IO.Path.Combine(path, fileName);

                //ReportProcessor reportProcessor = new ReportProcessor();
                //Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
                //instanceReportSource.ReportDocument = report;
                //RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                //using (FileStream fs = new FileStream(filePath, FileMode.Create))
                //{
                //    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                //}            

                //Telerik.Reporting.Report report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetter"));
                ////report.DocumentName = strTemplateName;
                ////report.DataSource = DS;
                //ReportViewer1.ReportSource = report;

                //var typeReportSource = new Telerik.Reporting.TypeReportSource();
                //typeReportSource.TypeName = "Telerik.Reporting.Examples.CSharp.ListBoundReport, CSharp.ReportLibrary";
                //this.ReportViewer1.ReportSource = typeReportSource;
            }
        }

        protected void RadButtonGenerateLetter_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = true;
            LoadReport();
        }

        protected void RadBtnSaveLetter_Click(object sender, EventArgs e)
        {
            //Saving

            strMagnumID = Request.QueryString["MagnumID"].ToString();
            strQuoteAuditID = Request.QueryString["QuoteAuditID"].ToString();
            string strQuoteOptionAuditTrailIDO1 = "0";
            string strQuoteOptionAuditTrailIDO2 = "0";
            string strQuoteOptionAuditTrailIDO3 = "0";
            string strQuoteOptionAuditTrailIDO4 = "0";
            string strQuoteOptionAuditTrailIDO5 = "0";
            string strMessageMain = string.Empty;
            string strMessage = string.Empty;

            try
            {
                Telerik.Web.UI.RadGrid OptionsGrid = (Telerik.Web.UI.RadGrid)RadPanelBar1.FindItemByValue("PanelItem1").FindControl("RadGridOptions");

                #region "Get options"
                foreach (GridDataItem item in OptionsGrid.Items)
                {
                    if ((item.FindControl("CheckBoxSelect") as System.Web.UI.WebControls.CheckBox).Checked == true)
                    {
                        switch (item.Cells[3].Text.Trim())
                        {
                            case "1":
                                if (strQuoteOptionAuditTrailIDO1 == "0")
                                    strQuoteOptionAuditTrailIDO1 = item.Cells[2].Text;

                                break;
                            case "2":
                                if (strQuoteOptionAuditTrailIDO2 == "0")
                                    strQuoteOptionAuditTrailIDO2 = item.Cells[2].Text;
                                break;
                            case "3":
                                if (strQuoteOptionAuditTrailIDO3 == "0")
                                    strQuoteOptionAuditTrailIDO3 = item.Cells[2].Text;
                                break;
                            case "4":
                                if (strQuoteOptionAuditTrailIDO4 == "0")
                                    strQuoteOptionAuditTrailIDO4 = item.Cells[2].Text;
                                break;
                            case "5":
                                if (strQuoteOptionAuditTrailIDO5 == "0")
                                    strQuoteOptionAuditTrailIDO5 = item.Cells[2].Text;
                                break;
                        }

                    }
                }
                #endregion

                DataSet DS = new DataSet();

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
                sqlCommandX.CommandText = "spx_SELECT_QuoteDetailByAuditTrailID_Detailed";

                sqlParam = new SqlParameter("QuoteAuditTrailID", strQuoteAuditID);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption1ID", strQuoteOptionAuditTrailIDO1);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption2ID", strQuoteOptionAuditTrailIDO2);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption3ID", strQuoteOptionAuditTrailIDO3);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption4ID", strQuoteOptionAuditTrailIDO4);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteOption5ID", strQuoteOptionAuditTrailIDO5);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlDR = sqlCommandX.ExecuteReader();
                DataTable dt = new DataTable("Info");
                dt.Load(sqlDR);
                DS.Tables.Add(dt);

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();

                #endregion

                // Creating and configuring the ObjectDataSource component:
                var objectDataSource = new Telerik.Reporting.ObjectDataSource();
                objectDataSource.DataSource = DS; // GetData returns a DataSet with three tables
                //objectDataSource.DataMember = "Product"; /// Indicating the exact table to bind to. If the DataMember is not specified the first data table will be used.
                //objectDataSource.CalculatedFields.Add(new Telerik.Reporting.CalculatedField("FullName", typeof(string), "=Fields.Name + ' ' + Fields.ProductNumber")); // Adding a sample calculated field.

                // Creating a new report
                Telerik.Reporting.Report report = new Telerik.Reporting.Report();

                //Telerik.Reporting.Report report = new Telerik.Reporting.Report();
                if ((dt.Rows[0][36].ToString() == "ok") && (dt.Rows[0][37].ToString() == "ok"))
                {
                    report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetter"));
                }

                if (dt.Rows[0][36].ToString() != "ok")
                {
                    report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterDisabilityOnly"));
                }

                if (dt.Rows[0][37].ToString() != "ok")
                {
                    report = (Telerik.Reporting.Report)Activator.CreateInstance(System.Reflection.Assembly.Load("Report").GetType("Report.QuoteLetterLifeOnly"));
                }

                DateTime now = DateTime.Now;
                //Console.WriteLine("Today is " + now.ToString("MMMM dd, yyyy") + ".");               

                report.DocumentName = now.ToString("ddMMhhmm") + "_" + strMagnumID + "_Quote requested";
                // Assigning the ObjectDataSource component to the DataSource property of the report.
                report.DataSource = objectDataSource;
                // Use the InstanceReportSource to pass the report to the viewer for displaying
                Telerik.Reporting.InstanceReportSource reportSource = new Telerik.Reporting.InstanceReportSource();
                reportSource.ReportDocument = report;

                string fileName = report.DocumentName + ".PDF";
                //string path = System.IO.Path.GetTempPath();
                //string strFolder = "~/files/";                

                string path = Server.MapPath("~/files/");
                path += now.ToString("yyyy");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                string filePath = System.IO.Path.Combine(path, fileName);                

                ReportProcessor reportProcessor = new ReportProcessor();
                Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
                instanceReportSource.ReportDocument = report;
                RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                }

                #region "Insert File location into DB"
                sqlCommandX = new SqlCommand();
                sqlCommandX.Connection = sqlConnectionX;
                sqlCommandX.CommandType = CommandType.StoredProcedure;
                sqlCommandX.CommandText = "spx_INSERT_QuoteFile";

                sqlParam = new SqlParameter("QuoteAuditTrailID", strQuoteAuditID);
                sqlCommandX.Parameters.Add(sqlParam);
                sqlParam = new SqlParameter("QuoteFile", filePath);
                sqlCommandX.Parameters.Add(sqlParam);

                sqlCommandX.ExecuteNonQuery();

                sqlDR.Close();
                sqlCommandX.Cancel();
                sqlCommandX.Dispose();
                #endregion

                sqlConnectionX.Close();

                HiddenFieldSavedFileName.Value = filePath;

                lblInfo.Text = "File saved successfully";

                RadBtnEmail.Visible = true;
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }

        protected void RadBtnEmail_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuoteEmailQuick.aspx?MagnumID=" + strMagnumID + "&QuoteAuditID=" + strQuoteAuditID + "&FilePath=" + HiddenFieldSavedFileName.Value.ToString() + "&UserID=" + Session["UserID"].ToString());
        }
    }
}