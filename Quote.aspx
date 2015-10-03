<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Quote.aspx.cs" Inherits="AllLifePricingWeb.Quote" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
           table.mylist input {
              width: 10px;
              display: block;
              float: left;
           }
           table.mylist label {
              width: 200px;
              display: block;
              float: left;
           }
        </style>
                    <script type="text/javascript">                       

                        window.onload = function () {
                            setInterval("KeepSessionAlive()", 300000)
                        }

                        function KeepSessionAlive() {
                            //alert("test");
                            console.log("Test - KeepSessionAlive")
                            //url = "/KeepSessionAlive.ashx?";
                            url = "http://192.168.100.30/Site_AllLifePricingTest/KeepSessionAlive.ashx";
                            //url = "http://localhost/AllLifePricingWeb/KeepSessionAlive.ashx";
                            var xmlHttp = new XMLHttpRequest();
                            xmlHttp.open("GET", url, true);
                            xmlHttp.send();
                        }

                        function PopupWindowResponse(arg) {
                            //alert(arg);
                            if (!arg) {
                                //alert('no');
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
                            }
                            else {
                                //alert('yes');
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("PopulatePage|" + arg);
                            }
                        }      
                        
                        function PopupWindowResponseCloseReset(arg) {
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("CloseReset");                                                        
                        }

                        function PopupWindowResponseCloseSessionExpired(arg) {
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("SessionExpired");
                        }          
                        
                        function replaceAll(string, find, replace) {
                            return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
                        }

                        function WhyNoQualify(sender, args)
                        {
                            //HiddenFieldQualifyClient
                            CheckQualifyButton();
                            var HiddenFieldQualifyClient = document.getElementById("<%=HiddenFieldQualifyClient.ClientID%>");
                            var MissingValues = HiddenFieldQualifyClient.value
                            //MissingValues = MissingValues.replace(",","<br />");
                            var find = ',';
                            var re = new RegExp(find, 'g');

                            MissingValues = MissingValues.replace(re, '<br />');
                            //MissingValues = replaceAll(MissingValues, ",", "<br />");
                                
                            //alert("You need to correct the following to get a quote: " + HiddenFieldQualifyClient.value);
                            radalert("<strong>You need to correct the following to get a quote:</strong> <br/> " + MissingValues, 450, 100, "Please note");
                        }

                        function CheckQualifyButton() {
                            var CheckResult = "";
                            var hideQuotePresentationPanel = false;
                            //MagnumID
                            //***************************************************
                            var textBox = $find('<%= RadTxtMagnumID.ClientID %>');
                            var text = textBox.get_textBoxValue().toString();
                            //console.log(text);                            
                            var textLength = text.length;
                            //console.log(textLength);
                            var valName = document.getElementById("<%=lblValMagnum.ClientID%>");
                            var buttonload = $find('<%= RadButtonLoadQuote.ClientID %>');                            
                            if (textLength < 8) {
                                CheckResult = CheckResult + "MagnumID missing,";
                                valName.innerText = "*";
                                buttonload.set_enabled(false);
                            }
                            else
                            {          
                                if (valName.innerText == "*") {
                                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("CheckMagID");
                                    valName.innerText = "";
                                }
                            }

                            //Client name and surname
                            //***************************************************
                            textBox = $find('<%= RadTxtClientNameAndSurname.ClientID %>');
                            var text = textBox.get_textBoxValue().toString();
                            //console.log(text);
                            var valClient = document.getElementById("<%=lblValClient.ClientID%>");
                            textLength = text.length;
                            if (textLength == 0) {
                                CheckResult = CheckResult + "Client name and surname missing,";
                                valClient.innerText = "*";
                            }
                            else {
                                valClient.innerText = "";
                            }

                            //Date of birth
                            //***************************************************
                            var DOBDatePicker = $find('<%= RadDatePickerDOB.ClientID %>');                               
                            var valDOB = document.getElementById("<%=lblValDOB.ClientID%>");
                            
                            if (DOBDatePicker.get_textBox().control.get_value() == "")
                            {
                                CheckResult = CheckResult + "Date of birth missing,";
                                valDOB.innerText = "*";
                            }
                            else {                                
                                valDOB.innerText = "";
                            }

                            var HiddenFieldRBDOBVal = document.getElementById("<%=HiddenFieldRBDOB.ClientID%>");
                            if (HiddenFieldRBDOBVal.value != "")
                            {                              
                                var ClientDOB = $find('<%= RadTxtAgeOfNextBirthday.ClientID %>').toString();                                                                

                                if (ClientDOB < 30)
                                {
                                    ClientDOB = "A1";
                                }
                                else
                                {
                                    if (ClientDOB < 40) {
                                        ClientDOB = "A2";
                                    }
                                    else
                                    {
                                        if (ClientDOB < 50) {
                                            ClientDOB = "A3";
                                        }
                                        else
                                        {
                                            if (ClientDOB < 60) {
                                                ClientDOB = "A4";
                                            }
                                            else
                                            {
                                                if (ClientDOB < 70) {
                                                    ClientDOB = "A5";
                                                }
                                                else
                                                {
                                                    if (ClientDOB > 70) {
                                                        ClientDOB = "A6";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }                                

                                if (ClientDOB != HiddenFieldRBDOBVal)
                                {
                                    hideQuotePresentationPanel = true;
                                    HiddenFieldRBDOBVal.value = ClientDOB.value;
                                }
                            }

                            //Gender
                            //***************************************************                            
                            var valGender = document.getElementById("<%=lblValGender.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonMale.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonFemale.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonOther.ClientID%>").checked == false))
                            {
                                CheckResult = CheckResult + "Gender missing,";
                                valGender.innerText = "*";
                            }
                            else
                            {
                                valGender.innerText = "";
                            }

                            if (document.getElementById("<%=RadioButtonMale.ClientID%>").checked == true)
                            {
                                var textBoxDressSize = $find('<%= RadNumericTxtDressSize.ClientID %>');
                                textBoxDressSize.set_value("");
                                textBoxDressSize.disable();
                            }
                            else
                            {
                                var textBoxDressSize = $find('<%= RadNumericTxtDressSize.ClientID %>');
                                textBoxDressSize.enable();

                                var textBoxPantsSize = $find('<%= RadNumericTxtPantSize.ClientID %>');                                
                                var PantsSize = textBoxPantsSize.get_textBoxValue().toString();
                                var DressSize = textBoxDressSize.get_textBoxValue().toString();

                                if (PantsSize.length > 0)
                                    textBoxDressSize.set_value(parseInt(PantsSize) - 24);
                                else
                                    textBoxDressSize.set_value("");
                            }

                            //Highest Qualification
                            //***************************************************
                            var ValHighQual = document.getElementById("<%=lblValHighQual.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonNotMat.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonMatriculated.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDiploma.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDegree.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "Qualification missing,";
                                ValHighQual.innerText = "*";
                            }
                            else
                            {
                                ValHighQual.innerText = "";
                            }

                            //Income
                            //***************************************************
                            textBox = $find('<%= RadNumericTxtIncome.ClientID %>');
                            var ValIncome = document.getElementById("<%=lblValIncome.ClientID%>");
                            var text = textBox.get_textBoxValue().toString();
                            //console.log(text);
                            textLength = text.length;
                            if (textLength == 0) {
                                CheckResult = CheckResult + "Income missing,";
                                ValIncome.innerText = "*";
                            }
                            else
                            { ValIncome.innerText = ""; }


                            if (document.getElementById("<%=RadioButtonMaritalStatusMarried.ClientID%>").checked == true)
                            {
                                //Spouse Highest Qualification
                                //***************************************************
                                var ValSHighQual = document.getElementById("<%=lblSpouseQualification.ClientID%>");
                                if ((document.getElementById("<%=RadioButtonSNotMat.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonSMat.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonSDip.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonSDegree.ClientID%>").checked == false)) {
                                    CheckResult = CheckResult + "Spouse Qualification missing,";
                                    ValSHighQual.innerText = "*";
                                }
                                else {
                                    ValSHighQual.innerText = "";
                                }

                                //Income
                                //***************************************************
                                textBox = $find('<%= RadNumericTxtSpouseIncome.ClientID %>');
                                var ValSIncome = document.getElementById("<%=lblSpouseIncome.ClientID%>");
                                var text = textBox.get_textBoxValue().toString();
                                //console.log(text);
                                textLength = text.length;
                                if (textLength == 0) {
                                    CheckResult = CheckResult + "Spouse Income missing,";
                                    ValSIncome.innerText = "*";
                                }
                                else { ValSIncome.innerText = ""; }
                            }

                            if (document.getElementById("<%=RadioButtonMaritalStatusNotMarried.ClientID%>").checked == true)
                            {
                                var ValSHighQual = document.getElementById("<%=lblSpouseQualification.ClientID%>");
                                ValSHighQual.innerText = "";
                                var ValSIncome = document.getElementById("<%=lblSpouseIncome.ClientID%>");
                                ValSIncome.innerText = "";
                            }

                            //Date of diagnosis
                            //***************************************************                            
                            var ValDateDiag = document.getElementById("<%=lblValDateDiag.ClientID%>");
                            //var DOBDatePicker = $find('<%= RadDatePickerDateOfDiag.ClientID %>');
                            var DOBYearPicker = $find('<%= RadMonthYearPickerDateOfDiag.ClientID %>');
                            if (DOBYearPicker.get_textBox().control.get_value() == "") {
                                CheckResult = CheckResult + "Date of diagnosis missing,";
                                ValDateDiag.innerText = "*";
                            }
                            else
                            { ValDateDiag.innerText = ""; }
                           
                            //Diabetes diagnosis
                            //***************************************************
                            var ValDiabetesType = document.getElementById("<%=lblValDiabetesType.ClientID%>");                            

                            var valInsulin = document.getElementById("<%=lblValInsulin.ClientID%>");
                            var valTablet = document.getElementById("<%=lblValTablet.ClientID%>");

                            if ((document.getElementById("<%=RadioButtonDiabetesType1.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDiabetesType2.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDiabetesTypeNotSure.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "Diabetes diagnosis missing,";
                                ValDiabetesType.innerText = "*";
                            }
                            else
                            {
                                ValDiabetesType.innerText = "";
                               
                                if (document.getElementById("<%=RadioButtonDiabetesTypeNotSure.ClientID%>").checked == true)
                                {
                                    if ((document.getElementById("<%=RadioButtonInsulinYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonInsulinNo.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonInsulinNotSure.ClientID%>").checked == false)) {
                                        CheckResult = CheckResult + "Insulin missing,";
                                        valInsulin.innerText = "*";
                                    }
                                    else { valInsulin.innerText = ""; }

                                    if ((document.getElementById("<%=RadioButtonTabletUseYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonTabletUseNo.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonTabletUseNotSure.ClientID%>").checked == false)) {
                                        CheckResult = CheckResult + "Tablet missing,";
                                        valTablet.innerText = "*";
                                    }
                                    else { valTablet.innerText = ""; }
                                }
                                else { valInsulin.innerText = ""; valTablet.innerText = ""; }
                            }

                            var HiddenFieldRMDiabetesType = document.getElementById("<%=HiddenFieldRMDiabetesType.ClientID%>");
                            var ClientDiabetesType = "";
                            if (document.getElementById("<%=RadioButtonDiabetesType1.ClientID%>").checked == true)
                            {
                                ClientDiabetesType = "T1";
                            }
                            if (document.getElementById("<%=RadioButtonDiabetesType2.ClientID%>").checked == true) {
                                ClientDiabetesType = "T2";
                            }

                            if (document.getElementById("<%=RadioButtonDiabetesTypeNotSure.ClientID%>").checked == true) {
                                ClientDiabetesType = "T2";
                            }                            

                            if (HiddenFieldRMDiabetesType.value != "")
                            {
                                //alert(ClientDiabetesType)
                                //alert(HiddenFieldRMDiabetesType.value);

                                if (ClientDiabetesType != HiddenFieldRMDiabetesType)
                                {
                                    //document.getElementById("divQuotePresentation").style.display = 'none';
                                    hideQuotePresentationPanel = true;
                                    HiddenFieldRMDiabetesType.value = ClientDiabetesType.value;
                                }
                            }
                        

                            //Doctor visits
                            //***************************************************                            
                            var valDocVisits = document.getElementById("<%=lblValDocVisits.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonDVYes1.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDVYes2.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDVYes3.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDVNo.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "Doctor visits missing,";
                                valDocVisits.innerText = "*";
                            }
                            else { valDocVisits.innerText = ""; }

                            //Diabetes control
                            //***************************************************                            
                            var valDiabeticControl = document.getElementById("<%=lblValDiabeticControl.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonDiabetControlExcellent.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDiabetControlGood.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDiabetControlMod.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonDiabetControlPoor.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "Diabetes Control missing,";
                                valDiabeticControl.innerText = "*";
                            }
                            else { valDiabeticControl.innerText = ""; }

                            //HbA1C
                            //***************************************************
                            var element = document.getElementById('<%=RadioButtonHbA1c4.ClientID%>');
                            if (element != null) // && element.value == '')
                            {
                                //console.log("RadioButtonHbA1c4 is visable")
                                var valHbA1C = document.getElementById("<%=lblValHbA1c.ClientID%>");
                                if ((document.getElementById("<%=RadioButtonHbA1c3.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c4.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c6.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c7.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c8.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c9.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c10.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c11.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c12.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c15.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == false)) {
                                    CheckResult = CheckResult + "HbA1C missing,";
                                    valHbA1C.innerText = "*";
                                }
                                else {
                                    valHbA1C.innerText = "";

                                    if (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) {
                                        document.getElementById('<%=RadioButtonExPYes.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExPNo.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonExFollowedVW.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExFollowedok.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExFollowedpoor.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonEatPYes.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatPNo.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonEatFollowedVW.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatFollowedOk.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatFollowedPoor.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonMedicalAidNone.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidNotSure.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidComp.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidHos.ClientID %>').removeAttribute('disabled');
                                    }
                                    else {                                       

                                        if (document.getElementById('<%=RadioButtonHbA1c4.ClientID%>').checked == true) {
                                            document.getElementById('<%=RadioButtonExPYes.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonExPNo.ClientID %>').removeAttribute('disabled');

                                            document.getElementById('<%=RadioButtonExFollowedVW.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonExFollowedok.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonExFollowedpoor.ClientID %>').removeAttribute('disabled');

                                            document.getElementById('<%=RadioButtonEatPYes.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonEatPNo.ClientID %>').removeAttribute('disabled');

                                            document.getElementById('<%=RadioButtonEatFollowedVW.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonEatFollowedOk.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonEatFollowedPoor.ClientID %>').removeAttribute('disabled');

                                            document.getElementById('<%=RadioButtonMedicalAidNone.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonMedicalAidNotSure.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonMedicalAidComp.ClientID %>').removeAttribute('disabled');
                                            document.getElementById('<%=RadioButtonMedicalAidHos.ClientID %>').removeAttribute('disabled');
                                        }
                                        else {
                                            document.getElementById('<%= RadioButtonExPYes.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExPYes.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExPNo.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExPNo.ClientID %>').checked = false;

                                            document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').checked = false;

                                            document.getElementById('<%= RadioButtonEatPYes.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatPYes.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatPNo.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatPNo.ClientID %>').checked = false;

                                            document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').checked = false;

                                            document.getElementById('<%= RadioButtonMedicalAidNone.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonMedicalAidNone.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonMedicalAidNotSure.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonMedicalAidNotSure.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonMedicalAidComp.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonMedicalAidComp.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonMedicalAidHos.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonMedicalAidHos.ClientID %>').checked = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //console.log("RadioButtonHbA1c4 NOT visable")
                                var valHbA1C = document.getElementById("<%=lblValHbA1c.ClientID%>");
                                if ((document.getElementById("<%=RadioButtonHbA1c3.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c6.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c7.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c8.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c9.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c10.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c11.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c12.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c15.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == false)) {
                                    CheckResult = CheckResult + "HbA1C missing,";
                                    valHbA1C.innerText = "*";
                                }
                                else {
                                    valHbA1C.innerText = "";

                                    if (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) {
                                        document.getElementById('<%=RadioButtonExPYes.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExPNo.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonExFollowedVW.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExFollowedok.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExFollowedpoor.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonEatPYes.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatPNo.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonEatFollowedVW.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatFollowedOk.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatFollowedPoor.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonMedicalAidNone.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidNotSure.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidComp.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidHos.ClientID %>').removeAttribute('disabled');
                                    }
                                    else
                                    {

                                        document.getElementById('<%= RadioButtonExPYes.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExPYes.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonExPNo.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExPNo.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonEatPYes.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatPYes.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonEatPNo.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatPNo.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonMedicalAidNone.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidNone.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonMedicalAidNotSure.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidNotSure.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonMedicalAidComp.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidComp.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonMedicalAidHos.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidHos.ClientID %>').checked = false;

                                    }
                                }
                            }

                            <%--var valHbA1C = document.getElementById("<%=lblValHbA1c.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonHbA1c3.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c4.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c6.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c7.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c8.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c9.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c10.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c11.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c12.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1c15.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "HbA1C missing,";
                                valHbA1C.innerText = "*";
                            }
                            else
                            {
                                valHbA1C.innerText = "";

                                if (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true)
                                {
                                    document.getElementById('<%=RadioButtonExPYes.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonExPNo.ClientID %>').removeAttribute('disabled');

                                    document.getElementById('<%=RadioButtonExFollowedVW.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonExFollowedok.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonExFollowedpoor.ClientID %>').removeAttribute('disabled');

                                    document.getElementById('<%=RadioButtonEatPYes.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonEatPNo.ClientID %>').removeAttribute('disabled');                                    

                                    document.getElementById('<%=RadioButtonEatFollowedVW.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonEatFollowedOk.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonEatFollowedPoor.ClientID %>').removeAttribute('disabled');

                                    document.getElementById('<%=RadioButtonMedicalAidNone.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonMedicalAidNotSure.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonMedicalAidComp.ClientID %>').removeAttribute('disabled');
                                    document.getElementById('<%=RadioButtonMedicalAidHos.ClientID %>').removeAttribute('disabled');
                                }
                                else
                                {
                                    
                                    var element = document.getElementById('<%=RadioButtonHbA1c4.ClientID%>');
                                    if (element != null) // && element.value == '')
                                    {
                                        console.log("RadioButtonHbA1c4 is visable")
                                    }
                                    else
                                    {
                                        console.log("RadioButtonHbA1c4 NOT visable")
                                    }

                                    if (document.getElementById('<%=RadioButtonHbA1c4.ClientID%>').checked == true) {
                                        document.getElementById('<%=RadioButtonExPYes.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExPNo.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonExFollowedVW.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExFollowedok.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonExFollowedpoor.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonEatPYes.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatPNo.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonEatFollowedVW.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatFollowedOk.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonEatFollowedPoor.ClientID %>').removeAttribute('disabled');

                                        document.getElementById('<%=RadioButtonMedicalAidNone.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidNotSure.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidComp.ClientID %>').removeAttribute('disabled');
                                        document.getElementById('<%=RadioButtonMedicalAidHos.ClientID %>').removeAttribute('disabled');
                                    }
                                    else
                                    {
                                        document.getElementById('<%= RadioButtonExPYes.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExPYes.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonExPNo.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExPNo.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonEatPYes.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatPYes.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonEatPNo.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatPNo.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').checked = false;

                                        document.getElementById('<%= RadioButtonMedicalAidNone.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidNone.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonMedicalAidNotSure.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidNotSure.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonMedicalAidComp.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidComp.ClientID %>').checked = false;
                                        document.getElementById('<%= RadioButtonMedicalAidHos.ClientID %>').disabled = true;
                                        document.getElementById('<%= RadioButtonMedicalAidHos.ClientID %>').checked = false;
                                    }
                                }
                            }--%>                           

                            //Excersise plan
                            //***************************************************
                            var valExP = document.getElementById("<%=lblValExP.ClientID%>");
                            var valExFollowed = document.getElementById("<%=lblValExFollow.ClientID%>");
                            var valMedcialAid = document.getElementById("<%=lblValMedcialAid.ClientID%>");

                            var element = document.getElementById('<%=RadioButtonHbA1c4.ClientID%>');
                            if (element != null) // && element.value == '')
                            {
                                if ((document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) || (document.getElementById("<%=RadioButtonHbA1c4.ClientID%>").checked == true)) {
                                    if ((document.getElementById("<%=RadioButtonExPYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonExPNo.ClientID%>").checked == false)) {
                                        CheckResult = CheckResult + "Excersise plan missing,";
                                        valExP.innerText = "*";
                                    }
                                    else {
                                        valExP.innerText = "";

                                        if (document.getElementById("<%=RadioButtonExPYes.ClientID%>").checked == true) {
                                            if ((document.getElementById("<%=RadioButtonExFollowedVW.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonExFollowedok.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonExFollowedpoor.ClientID%>").checked == false)) {
                                                CheckResult = CheckResult + "Excersise followed missing,";
                                                valExFollowed.innerText = "*";
                                            }
                                            else {
                                                valExFollowed.innerText = "";
                                            }
                                        }
                                        else {
                                            document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').checked = false;
                                            valExFollowed.innerText = "";
                                        }
                                    }
                                }
                                else
                                {
                                    valExP.innerText = ""; valExFollowed.innerText = ""; valMedcialAid.innerText = "";
                                }
                            }
                            else {
                                //console.log("RadioButtonHbA1c4 NOT visable")
                                if ((document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true)) {
                                    if ((document.getElementById("<%=RadioButtonExPYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonExPNo.ClientID%>").checked == false)) {
                                        CheckResult = CheckResult + "Excersise plan missing,";
                                        valExP.innerText = "*";
                                    }
                                    else {
                                        valExP.innerText = "";

                                        if (document.getElementById("<%=RadioButtonExPYes.ClientID%>").checked == true) {
                                            if ((document.getElementById("<%=RadioButtonExFollowedVW.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonExFollowedok.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonExFollowedpoor.ClientID%>").checked == false)) {
                                                CheckResult = CheckResult + "Excersise followed missing,";
                                                valExFollowed.innerText = "*";
                                            }
                                            else {
                                                valExFollowed.innerText = "";
                                            }
                                        }
                                        else {
                                            document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').checked = false;
                                            valExFollowed.innerText = "";
                                        }
                                    }
                                }
                                else
                                {
                                    valExP.innerText = ""; valExFollowed.innerText = ""; valMedcialAid.innerText = "";
                                }
                            }
                            

                            //Eating plan
                            //***************************************************
                            var valEatP = document.getElementById("<%=lblValEatP.ClientID%>");
                            var valEatFollowed = document.getElementById("<%=lblEatFollow.ClientID%>");

                            var element = document.getElementById('<%=RadioButtonHbA1c4.ClientID%>');
                            if (element != null) // && element.value == '')
                            {
                                //console.log("RadioButtonHbA1c4 is visable")
                                //if (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) {
                                if ((document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) || (document.getElementById("<%=RadioButtonHbA1c4.ClientID%>").checked == true)) {
                                    if ((document.getElementById("<%=RadioButtonEatPYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonEatPNo.ClientID%>").checked == false)) {
                                        CheckResult = CheckResult + "Eating plan missing,";
                                        valEatP.innerText = "*";
                                    }
                                    else {
                                        valEatP.innerText = "";

                                        if (document.getElementById("<%=RadioButtonEatPYes.ClientID%>").checked == true) {
                                            if ((document.getElementById("<%=RadioButtonEatFollowedVW.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonEatFollowedOk.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonEatFollowedPoor.ClientID%>").checked == false)) {
                                                CheckResult = CheckResult + "Eating followed missing,";
                                                valEatFollowed.innerText = "*";
                                            }
                                            else {
                                                valEatFollowed.innerText = "";

                                            }
                                        }
                                        else {
                                            document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').checked = false;
                                            valEatFollowed.innerText = "";
                                        }
                                    }
                                }
                                else
                                {
                                    valEatP.innerText = ""; valEatFollowed.innerText = "";
                                }
                            }
                            else
                            {
                                //console.log("RadioButtonHbA1c4 NOT visable")
                                //if (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) {
                                if ((document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true)) {
                                    if ((document.getElementById("<%=RadioButtonEatPYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonEatPNo.ClientID%>").checked == false)) {
                                        CheckResult = CheckResult + "Eating plan missing,";
                                        valEatP.innerText = "*";
                                    }
                                    else {
                                        valEatP.innerText = "";

                                        if (document.getElementById("<%=RadioButtonEatPYes.ClientID%>").checked == true) {
                                            if ((document.getElementById("<%=RadioButtonEatFollowedVW.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonEatFollowedOk.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonEatFollowedPoor.ClientID%>").checked == false)) {
                                                CheckResult = CheckResult + "Eating followed missing,";
                                                valEatFollowed.innerText = "*";
                                            }
                                            else {
                                                valEatFollowed.innerText = "";

                                            }
                                        }
                                        else {
                                            document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').checked = false;
                                            document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').disabled = true;
                                            document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').checked = false;
                                            valEatFollowed.innerText = "";
                                        }
                                    }
                                }
                                else
                                {
                                    valEatP.innerText = ""; valEatFollowed.innerText = "";
                                }
                            }

                            

                            //High BP
                            //***************************************************
                            var valBP = document.getElementById("<%=lblvalBP.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonHighBPYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHighBPNo.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonHighBP.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "High BP missing,";
                                valBP.innerText = "*";
                            }
                            else { valBP.innerText = ""; }

                            //High Cholesterol
                            //***************************************************
                            var valChol = document.getElementById("<%=lblValChol.ClientID%>");
                            if ((document.getElementById("<%=RadioButtonCholesterolYes.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonCholesterolNo.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonCholesterolNotSure.ClientID%>").checked == false)) {
                                CheckResult = CheckResult + "High Cholesterol missing,";
                                valChol.innerText = "*";
                            }
                            else { valChol.innerText = ""; }

                            //Medical aid
                            //***************************************************
                            var valMedcialAid = document.getElementById("<%=lblValMedcialAid.ClientID%>");
                            if (document.getElementById("<%=RadioButtonHbA1cUnknown.ClientID%>").checked == true) {
                                if ((document.getElementById("<%=RadioButtonMedicalAidNone.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonMedicalAidNotSure.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonMedicalAidComp.ClientID%>").checked == false) && (document.getElementById("<%=RadioButtonMedicalAidHos.ClientID%>").checked == false)) {
                                    CheckResult = CheckResult + "Medical aid missing,";
                                    valMedcialAid.innerText = "*";
                                }
                                else { valMedcialAid.innerText = ""; }
                            }

                            //Height
                            //***************************************************
                            textBox = $find('<%= RadNumericTxtHeight.ClientID %>');
                            var textHeight = textBox.get_textBoxValue().toString();
                            var textBox2 = $find('<%= RadNumericTxtWeight.ClientID %>');
                            var textWeight = textBox2.get_textBoxValue().toString();
                            var textBox3 = $find('<%= RadNumericTxtPantSize.ClientID %>');
                            var textPantSize = textBox3.get_textBoxValue().toString();
                            var textBox4 = $find('<%= RadNumericTxtBMI.ClientID %>');
                            var textBMI = textBox4.get_textBoxValue().toString();
                            //console.log(textHeight);
                            textLength = textHeight.length;
                            var textLength2 = textWeight.length;
                            var textLength3 = textPantSize.length;

                            var valHeight = document.getElementById("<%=lblValHeight.ClientID%>");
                            var valWeight = document.getElementById("<%=lblValWeight.ClientID%>");
                            var valPants = document.getElementById("<%=lblValPants.ClientID%>");

                            //console.log(textHeight);
                            //console.log(textWeight);
                            //console.log(textPantSize);
                            //if (((textLength == 0) && (textLength2 == 0)) || ((textLength == 0) && (textLength3 == 0))) {
                            //if (((textHeight == "") && (textWeight == "")) || ((textHeight == "") && (textPantSize == ""))) {
                            if (textHeight == "")
                            {
                                if (textPantSize == "") {
                                    CheckResult = CheckResult + "Height missing,";
                                    valHeight.innerText = "*";
                                }
                                else {
                                    valHeight.innerText = "";
                                }
                            }
                            else
                            {
                                valHeight.innerText = "";
                                //if ((textWeight == "") && (textPantSize == "")) {
                                //    CheckResult = CheckResult + "Weight or pant size missing,";
                                //}                                                                
                            }

                            if (textWeight == "")
                            {
                                if (textPantSize == "")
                                {
                                    CheckResult = CheckResult + "Weight missing,";
                                    valWeight.innerText = "*";
                                }
                                else {
                                    valWeight.innerText = "";
                                }
                            }
                            else
                            {
                                valWeight.innerText = "";                               
                            }                           

                            if (textPantSize == "")
                            {
                                if ((textWeight == "") && (textPantSize == ""))
                                {
                                    CheckResult = CheckResult + "Pants size missing,";
                                    valPants.innerText = "*";
                                }
                                else 
                                {
                                    valPants.innerText = "";
                                }

                                if (textBMI == 0)
                                {
                                    CheckResult = CheckResult + "Pants size missing,";
                                    valPants.innerText = "*";
                                }
                                else
                                {
                                    valPants.innerText = "";
                                }
                            }
                            else {
                                valPants.innerText = "";
                            }
                            

                            //Alcohol
                            //***************************************************                            
                            var valAlcoholUnits = document.getElementById("<%=lblValAlcoholUnits.ClientID%>");
                            
                            //valAlcoholUnits.innerText = "";

                            //alert(1);
                            var elementRef = document.getElementById("RadioButtonListAlcohol");
                            var radioButtonListArray = elementRef.getElementsByTagName('input');
                            var checkedValues = '';
                            var atLeastOneChecked = false;

                            //alert(radioButtonListArray.length);

                            for (var i = 0; i < radioButtonListArray.length; i++) {
                                var radioButtonRef = radioButtonListArray[i];
                                //alert(radioButtonRef.checked);
                                //alert(radioButtonRef.value);
                                if (radioButtonRef.checked == true)
                                {
                                    atLeastOneChecked = true;
                                    //// To get the Text property, use this code:
                                    //var labelArray = radioButtonRef.value;
                                    ////alert(labelArray);                                            
                                    //if (labelArray.length > 0)
                                    //{
                                    //    if (checkedValues.length > 0)
                                    //        checkedValues += ', ';
                                    //    checkedValues += labelArray;
                                    //}
                                }
                            }

                            if (atLeastOneChecked == false)
                            {
                                CheckResult = CheckResult + "Alcohol missing,";
                                valAlcoholUnits.innerText = "*";
                            }
                            else { valAlcoholUnits.innerText = ""; }
                            //alert(userAnswer1);                           

                            //Tobacco  
                            //***************************************************
                            var valTobaccolUnits = document.getElementById("<%=lblValTobaccoUntis.ClientID%>");
                            //alert(1);
                            var elementRef = document.getElementById("RadioButtonListTobacco");
                            var radioButtonListArray = elementRef.getElementsByTagName('input');
                            var checkedValues = '';
                            var atLeastOneChecked = false;

                            //alert(radioButtonListArray.length);

                            for (var i = 0; i < radioButtonListArray.length; i++) {
                                var radioButtonRef = radioButtonListArray[i];
                                //alert(radioButtonRef.checked);
                                //alert(radioButtonRef.value);
                                if (radioButtonRef.checked == true) {
                                    atLeastOneChecked = true;
                                    //// To get the Text property, use this code:
                                    //var labelArray = radioButtonRef.value;
                                    ////alert(labelArray);                                            
                                    //if (labelArray.length > 0)
                                    //{
                                    //    if (checkedValues.length > 0)
                                    //        checkedValues += ', ';
                                    //    checkedValues += labelArray;
                                    //}
                                }
                            }

                            if (atLeastOneChecked == false) {
                                CheckResult = CheckResult + "Tobacco missing,";
                                valTobaccolUnits.innerText = "*";
                            }
                            else { valTobaccolUnits.innerText = ""; }
                            //alert(userAnswer1);

                            //console.log("CheckResult");
                            //console.log(CheckResult);

                            if (CheckResult == "")
                            {
                                CheckResult = "Ok"
                            }

                            console.log(CheckResult);
                            var HiddenFieldQualifyClient = document.getElementById("<%=HiddenFieldQualifyClient.ClientID%>");
                            var WhyCantIQuality = $find('<%= RadButtonWhyCantIQuality.ClientID %>');                            
                            HiddenFieldQualifyClient.value = CheckResult;

                            //return CheckResult;
                            var buttonQualify = $find('<%= RadBtnQualifyClient.ClientID %>');
                            if (CheckResult == "Ok") {                                
                                buttonQualify.set_enabled(true);
                                WhyCantIQuality.set_enabled(false);
                            }
                            else {
                                buttonQualify.set_enabled(false);
                                WhyCantIQuality.set_enabled(true);
                            }

                            if (hideQuotePresentationPanel == true)
                            {
                                var buttonGenerateLetter = $find('<%= RadButtonGenerateLetter.ClientID %>');
                                buttonGenerateLetter.set_enabled(false);
                                var lblRequalify = document.getElementById("<%=lblRequalify.ClientID%>");
                                lblRequalify.innerText = "You need to re-qualify after changing any risk modifier fields";
                                // console.log("hide pane'");
                                //$find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("hideQuoteP");
                            }
                            else
                            {
                                var lblRequalify = document.getElementById("<%=lblRequalify.ClientID%>");
                                lblRequalify.innerText = "";
                            }

                            //Escalation life
                            var panel = document.getElementById("<%= PanelQuotePresentation.ClientID %>");
                            if (panel)
                            {
                                //alert("Panel is visible");
                                var HiddenFieldEscalationLife = document.getElementById("<%=HiddenFieldEscalationLife.ClientID%>");
                                var ClientEscalationLife = "";
                                //if (document.getElementById("<%=RadioButtonEscLife6.ClientID%>").checked == true) 
                                if (document.getElementById("<%=RadioButtonEsc6.ClientID%>").checked == true)
                                {
                                    ClientEscalationLife = "6.00";
                                }
                                //if (document.getElementById("<%=RadioButtonEscLife10.ClientID%>").checked == true)
                                if (document.getElementById("<%=RadioButtonEsc10.ClientID%>").checked == true)
                                {
                                        ClientEscalationLife = "10.00";
                                    }

                                    if (ClientEscalationLife != HiddenFieldEscalationLife.value) {
                                        hideQuotePresentationPanel = true;
                                        //alert("requalify - Life");
                                        //alert(ClientEscalationLife);
                                        //alert(HiddenFieldEscalationLife.value);
                                        //HiddenFieldEscalationLife.value = ClientEscalationLife;
                                    }

                                <%--//Escalation Disability
                                var HiddenFieldEscalationDisablility = document.getElementById("<%=HiddenFieldEscalationDisablility.ClientID%>");
                                var ClientEscalationDisablility = "";
                                if (document.getElementById("<%=RadioButtonEscalationDis6.ClientID%>").checked == true)
                                {
                                    ClientEscalationDisablility = "6.00";
                                }
                                if (document.getElementById("<%=RadioButtonEscalationDis10.ClientID%>").checked == true)
                                {
                                    ClientEscalationDisablility = "10.00";
                                }

                                if (ClientEscalationDisablility != HiddenFieldEscalationDisablility.value)
                                {
                                        hideQuotePresentationPanel = true;
                                        //alert("requalify - Disablility");
                                        //alert(ClientEscalationDisablility);
                                        //alert(HiddenFieldEscalationDisablility.value);
                                        //HiddenFieldEscalationDisablility.value = ClientEscalationLife;
                                 }--%>

                                //Benifit - Life
                                var combolife = $find('<%=RadComboBoxTypeBenefitLife.ClientID %>');
                                var combolifeVal = combolife.get_selectedItem().get_text();
                                var HiddenFieldRadComboBoxTypeBenefitLife = document.getElementById("<%=HiddenFieldRadComboBoxTypeBenefitLife.ClientID%>");
                                if (combolifeVal != HiddenFieldRadComboBoxTypeBenefitLife.value)
                                {
                                    hideQuotePresentationPanel = true;
                                }

                                //Benifit - Disaility
                                var comboDisability = $find('<%=RadComboBoxTypeBenefitDisability.ClientID %>');
                                var comboDisabilityVal = combolife.get_selectedItem().get_text();
                                var HiddenFieldRadComboBoxTypeBenefitDisability = document.getElementById("<%=HiddenFieldRadComboBoxTypeBenefitDisability.ClientID%>");
                                if (comboDisabilityVal != HiddenFieldRadComboBoxTypeBenefitDisability.value)
                                {
                                    hideQuotePresentationPanel = true;
                                }

                            } //if (panel)
                            //else
                            //{
                            //    //alert("Panel is not visible");
                            //}

                            //Occupation
                            var ComboBoxOccupation = $find('<%=RadComboBoxOccupation.ClientID %>');
                            var OccupationVal = ComboBoxOccupation.get_selectedItem().get_text();
                            var HiddenFieldOccupation = document.getElementById("<%=HiddenFieldOccupation.ClientID%>");
                            if (OccupationVal != HiddenFieldOccupation.value) {
                                    hideQuotePresentationPanel = true;
                                }
                           

                        }

                        function OnKeyUpTxtCoverCalcLife() {
                            //RadioButtonListWhoIsPaying
                            var textBoxCoverLife = $find('<%= RadNumericTxtCoverLife.ClientID %>');
                            var strCoverLife = textBoxCoverLife.get_textBoxValue().toString();                            
                            strCoverLife = strCoverLife.replace("R ", "");
                            strCoverLife = strCoverLife.replace(" ", "");
                            strCoverLife = strCoverLife.replace(",00", "");
                            if (strCoverLife == "")
                                strCoverLife = 0
                            var CoverLife = parseFloat(strCoverLife);
                            var textBoxIncome = $find('<%= RadNumericTxtIncome.ClientID %>');
                            var textBoxSpouseIncome = $find('<%= RadNumericTxtSpouseIncome.ClientID %>');
                            var strIncome = textBoxIncome.get_textBoxValue().toString();
                            var strSpouseIncome = textBoxSpouseIncome.get_textBoxValue().toString();
                            strIncome = strIncome.replace("R ", "");
                            strIncome = strIncome.replace(" ", "");
                            strSpouseIncome = strSpouseIncome.replace("R ", "");
                            strSpouseIncome = strSpouseIncome.replace(" ", "");
                            //console.log(strIncome);
                            //console.log(strSpouseIncome);
                            var ComboBoxOccupation = $find('<%=RadComboBoxOccupation.ClientID %>');
                            var OccupationVal = ComboBoxOccupation.get_selectedItem().get_text();
                            var Income = parseFloat(strIncome);                        

                            if (OccupationVal == "House spouse (qualify for disability + same cover amt as spouse)")
                            {
                                //console.log("strIncome");
                                //console.log(strIncome);
                                if (strIncome == "0,00")
                                {
                                    Income = parseFloat(strSpouseIncome);
                                    //console.log("Income");
                                    //console.log(Income);
                                }
                            }

                            //var Income = parseFloat(strIncome);
                            var maxCover = (Income * 12) * 15;
                            //console.log("maxCover");
                            //console.log(maxCover);
                            if (maxCover < 350000)
                            {
                                maxCover = 350000;
                            }

                            if (maxCover > 10000000)
                            {
                                maxCover = 10000000
                            }

                            if (CoverLife > maxCover)
                            {
                                textBoxCoverLife.set_value(maxCover);
                            }
                        }

                        function OnKeyUpTxtCoverCalcDis() {
                            var textBoxCoverDis = $find('<%= RadNumericTxtCoverAmnDis.ClientID %>');
                            var strCoverDis = textBoxCoverDis.get_textBoxValue().toString();
                            strCoverDis = strCoverDis.replace("R ", "");
                            strCoverDis = strCoverDis.replace(" ", "");
                            strCoverDis = strCoverDis.replace(",00", "");
                            if (strCoverDis == "")
                                strCoverDis = 0
                            var CoverDis = parseFloat(strCoverDis);
                            var textBoxIncome = $find('<%= RadNumericTxtIncome.ClientID %>');
                            var textBoxSpouseIncome = $find('<%= RadNumericTxtSpouseIncome.ClientID %>');
                            var strSpouseIncome = textBoxSpouseIncome.get_textBoxValue().toString();
                            var strIncome = textBoxIncome.get_textBoxValue().toString();
                            strIncome = strIncome.replace("R ", "");
                            strIncome = strIncome.replace(" ", "");
                            strSpouseIncome = strSpouseIncome.replace("R ", "");
                            strSpouseIncome = strSpouseIncome.replace(" ", "");
                            var ComboBoxOccupation = $find('<%=RadComboBoxOccupation.ClientID %>');
                            var OccupationVal = ComboBoxOccupation.get_selectedItem().get_text();
                            var Income = parseFloat(strIncome);

                            if (OccupationVal == "House spouse (qualify for disability + same cover amt as spouse)") {
                                //console.log("strIncome");
                                //console.log(strIncome);
                                if (strIncome == "0,00") {
                                    Income = parseFloat(strSpouseIncome);
                                    //console.log("Income");
                                    //console.log(Income);
                                }
                            }

                            var maxCover = (Income * 12) * 15;
                            if (maxCover < 350000) {
                                maxCover = 350000;
                            }

                            console.log("maxCover");
                            console.log(maxCover);

                            if (maxCover > 10000000) {
                                maxCover = 10000000
                            }

                            if (CoverDis > maxCover) {
                                textBoxCoverDis.set_value(maxCover);
                            }
                        }

                        function OnKeyUpTxtPremiumCalcLife() {
                            var boolLifeInsuredisPaying = false;
                            
                            var elementRef = document.getElementById("RadioButtonListWhoIsPaying");
                            var radioButtonListArray = elementRef.getElementsByTagName('input');
                            for (var i = 0; i < radioButtonListArray.length; i++) {
                                var radioButtonRef = radioButtonListArray[i];
                                //console.log('value: ' + radioButtonRef.value);
                                if (radioButtonRef.checked == true) {
                                    atLeastOneChecked = true;
                                    // To get the Text property, use this code:
                                    var labelArray = radioButtonRef.value;
                                    //alert(labelArray);                                            
                                    //console.log('labelArray: ' + labelArray);
                                    if (labelArray.length > 0) {
                                        if (labelArray == "Life Insured is Paying") {
                                            boolLifeInsuredisPaying = true;
                                        }
                                    }
                                }
                            }

                            if (boolLifeInsuredisPaying == true) {
                                var textBoxPremiumLife = $find('<%= RadNumericTxtPremiumLife.ClientID %>');
                                var PremiumLife = parseFloat(textBoxPremiumLife.get_textBoxValue().toString());
                                var textBoxPremiumDis = $find('<%= RadNumericTxtPremiumDis.ClientID %>');
                                var strPremiumDis = textBoxPremiumDis.get_textBoxValue().toString();
                                strPremiumDis = strPremiumDis.replace("R ", "");
                                strPremiumDis = strPremiumDis.replace(" ", "");
                                strPremiumDis = strPremiumDis.replace(",00", "");
                                //console.log('strPremiumDis: ' + strPremiumDis);
                                if (strPremiumDis == "")
                                    strPremiumDis = 0
                                var PremiumDis = parseFloat(strPremiumDis);

                                var textBoxIncome = $find('<%= RadNumericTxtIncome.ClientID %>');
                                var strIncome = textBoxIncome.get_textBoxValue().toString();
                                //console.log('strIncome: ' + strIncome);
                                strIncome = strIncome.replace("R ", "");
                                //console.log('strIncome: ' + strIncome);
                                strIncome = strIncome.replace(" ", "");
                                //console.log('strIncome: ' + strIncome);
                                var Income = parseFloat(strIncome);

                                //console.log('PremiumLife: ' + PremiumLife);
                                //console.log('Income: ' + Income);
                                //console.log('PremiumDis: ' + PremiumDis);

                                var dec25percent = (Income * 25) / 100;
                                //console.log('dec25percent: ' + dec25percent);

                                var total = PremiumLife + PremiumDis;
                                //console.log('total: ' + total);

                                if ((total) > dec25percent) {
                                    //alert(dec25percent);
                                    var newValue = dec25percent - PremiumDis;
                                    textBoxPremiumLife.set_value(newValue);
                                }
                            }
                        }

                        function OnKeyUpTxtPremiumCalcDis()
                        {
                            var boolLifeInsuredisPaying = false;
                            console.log('start');

                            var elementRef = document.getElementById("RadioButtonListWhoIsPaying");
                            var radioButtonListArray = elementRef.getElementsByTagName('input');
                            for (var i = 0; i < radioButtonListArray.length; i++) {
                                var radioButtonRef = radioButtonListArray[i];                                
                                //console.log('value: ' + radioButtonRef.value);
                                if (radioButtonRef.checked == true) {
                                    atLeastOneChecked = true;
                                    // To get the Text property, use this code:
                                    var labelArray = radioButtonRef.value;
                                    //alert(labelArray);                                            
                                    //console.log('labelArray: ' + labelArray);
                                    if (labelArray.length > 0)
                                    {
                                        if (labelArray == "Life Insured is Paying") {
                                            boolLifeInsuredisPaying = true;
                                        }
                                    }
                                }
                            }

                            if (boolLifeInsuredisPaying == true) {
                                var textBoxPremiumLife = $find('<%= RadNumericTxtPremiumLife.ClientID %>');
                                var strPremiumLife = textBoxPremiumLife.get_textBoxValue().toString();
                                strPremiumLife = strPremiumLife.replace("R ", "");
                                strPremiumLife = strPremiumLife.replace(" ", "");
                                strPremiumLife = strPremiumLife.replace(",00", "");
                                //console.log('strPremiumLife: ' + strPremiumLife);

                                if (strPremiumLife == "")
                                    strPremiumLife = 0
                                var PremiumLife = parseFloat(strPremiumLife);

                                var textBoxPremiumDis = $find('<%= RadNumericTxtPremiumDis.ClientID %>');
                                var PremiumDis = parseFloat(textBoxPremiumDis.get_textBoxValue().toString());
                                var textBoxIncome = $find('<%= RadNumericTxtIncome.ClientID %>');
                                var strIncome = textBoxIncome.get_textBoxValue().toString();
                                strIncome = strIncome.replace("R ", "");
                                strIncome = strIncome.replace(" ", "");
                                var Income = parseFloat(strIncome);

                                //console.log('PremiumLife: ' + PremiumLife);
                                //console.log('Income: ' + Income);
                                //console.log('PremiumDis: ' + PremiumDis);

                                var dec25percent = (Income * 25) / 100;

                                //console.log('dec25percent: ' + dec25percent);

                                var total = PremiumLife + PremiumDis;
                                //console.log('total: ' + total);

                                if ((total) > dec25percent) {
                                    //alert(dec25percent);
                                    var newValue = dec25percent - PremiumLife;
                                    textBoxPremiumDis.set_value(newValue);
                                }
                            }
                        }

                        function OnKeyUpTxtDressSize()
                        {
                            if (document.getElementById("<%=RadioButtonFemale.ClientID%>").checked == true)
                            {
                                var textBoxPantsSize = $find('<%= RadNumericTxtPantSize.ClientID %>');

                                var textBoxDressSize = $find('<%= RadNumericTxtDressSize.ClientID %>');
                                var DressSize = textBoxDressSize.get_textBoxValue().toString();

                                if (DressSize.length > 0)
                                    textBoxPantsSize.set_value(parseInt(DressSize) + 24);
                                else
                                    textBoxPantsSize.set_value("");
                            }
                            CheckQualifyButton();
                        }

                        function OnKeyUpTxtPantsSize()
                        {                            
                            if (document.getElementById("<%=RadioButtonFemale.ClientID%>").checked == true)
                            {
                                var textBoxPantsSize = $find('<%= RadNumericTxtPantSize.ClientID %>');
                                var PantsSize = textBoxPantsSize.get_textBoxValue().toString();

                                var textBoxDressSize = $find('<%= RadNumericTxtDressSize.ClientID %>');
                                //var DressSize = textBoxPantsSize.get_textBoxValue().toString();
                                if (PantsSize.length > 0)
                                    textBoxDressSize.set_value(parseInt(PantsSize) - 24);
                                else
                                    textBoxDressSize.set_value("");
                            }
                            CheckQualifyButton();
                        }

                        function OnKeyupMagnumID(sender, eventArgs) {
                            CheckQualifyButton();
                        }

                        function OnKeyPressSerialText(sender, eventArgs) {
                            var char = eventArgs.get_keyCharacter();
                            //will allow just letters, numbers and "-" letter 
                            var exp = /[^a-zA-Z- ]/g;
                            if (exp.test(char)) {
                                eventArgs.set_cancel(true);
                            }
                        }

                        function calculateMetresToFeetAndInches(meters) {
                            var metres = meters;
                            //var metreInches = metres * 39.370078740157477;                            
                            //var feet = Math.floor(metreInches / 12);
                            //var inches = Math.floor(metreInches % 12);

                            var feet = metres / 0.3048;
                            //console.log('full feet: ' + feet);
                            var value = parseInt(feet.toString().split(".")[0], 10);
                            //console.log('value: ' + value);
                            var newfeet = feet - value;
                            //console.log('newfeet: ' + newfeet);
                            var inches = newfeet * 12;

                            //console.log('feet: ' + value);
                            //console.log('inches: ' + inches);

                            var textBoxMaskedHFeet = $find('<%= RadNumericTxtFeet.ClientID %>');
                            var textBoxMaskedHInches = $find('<%= RadNumericTxtInches.ClientID %>');

                            textBoxMaskedHFeet.set_value(value);
                            textBoxMaskedHInches.set_value(inches);
                        }

                        function calculateFeetAndInchesToMetres(Feet,Inches) {
                            var feet = Feet;
                            var inches = Inches;
                            var totalInches = feet * 12;
                            totalInches = totalInches + (inches * 1);
                            var cm = totalInches * 2.54;
                            var metres = cm / 100;
                            return metres.toFixed(2);
                        }

                        function lbsAndOzToK() {
                                            
                            var textBoxWeight = $find('<%= RadNumericTxtWeight.ClientID %>');
                            var textW = textBoxWeight.get_textBoxValue().toString();
                            var textBoxPounds = $find('<%= RadNumericTxtpounds.ClientID %>');
                            var pounds = textBoxPounds.get_textBoxValue().toString();
                            if (pounds.length == 0)
                                pounds = 0;
                           <%-- var textBoxOunces = $find('<%= RadNumericTxtOunces.ClientID %>');
                            var ounces = textBoxOunces.get_textBoxValue().toString();
                            if (ounces.length == 0)
                                ounces = 0;--%>
                            //console.log('pounds: ' + pounds);
                            ////console.log('ounces: ' + ounces);

                            if (pounds.length > 0) {
                                if (textW.length > 0) {
                                    //var pounds = pounds + ounces / 16;                            
                                    var kg = pounds * 0.45359237;
                                    //console.log('kg: ' + kg);                            
                                    textBoxWeight.set_value(kg);
                                }                                
                            }

                            CheckQualifyButton();
                        }

                        function kToLbs(pK) {
                            var textBoxWeight = $find('<%= RadNumericTxtWeight.ClientID %>');
                            var Weight = textBoxWeight.get_textBoxValue().toString();
                            var textBoxPounds = $find('<%= RadNumericTxtpounds.ClientID %>');

                            if (Weight.length > 0) {                                
                                //var textBoxOunces = $find('<%= RadNumericTxtOunces.ClientID %>');
                                console.log('kg: ' + Weight);

                                var nearExact = Weight / 0.45359237;
                                //console.log('nearExact: ' + nearExact);
                                var lbs = Math.floor(nearExact);
                                //console.log('lbs: ' + lbs);
                                //var oz = (nearExact - lbs) * 16;
                                textBoxPounds.set_value(nearExact);
                                //textBoxOunces.set_value(oz);

                                OnKeyRadTxtWeight();
                            }
                            else
                            {
                                console.log("should clear");
                                textBoxPounds.set_value("");
                                CheckQualifyButton();
                            }
                        }

                        function OnKeyUpMaskedTxtHeight()
                        {
                            var textBoxH = $find('<%= RadNumericTxtHeight.ClientID %>');
                            var textH = textBoxH.get_textBoxValue().toString();
                            var textBoxMaskedHFeet = $find('<%= RadNumericTxtFeet.ClientID %>');
                            var textMaskedHFeet = textBoxMaskedHFeet.get_textBoxValue().toString();
                            var textBoxMaskedHInches = $find('<%= RadNumericTxtInches.ClientID %>');
                            var textMaskedHInches = textBoxMaskedHInches.get_textBoxValue().toString();

                            if (textMaskedHFeet.length > 0)
                            {
                                var NewMeters = calculateFeetAndInchesToMetres(textMaskedHFeet, textMaskedHInches);
                                textBoxH.set_value(NewMeters);
                            }
                           

                            CheckQualifyButton();
                        }

                        function OnKeyRadTxtWeight(sender, eventArgs)
                        {
                            var textBoxW = $find('<%= RadNumericTxtWeight.ClientID %>');
                            var textW = textBoxW.get_textBoxValue().toString();
                            var textWLength = textW.length;
                            var textBoxH = $find('<%= RadNumericTxtHeight.ClientID %>');
                            var textH = textBoxH.get_textBoxValue().toString();
                            var textHLength = textH.length;
                            var textBoxBMI = $find('<%= RadNumericTxtBMI.ClientID %>');
                            var decW = parseFloat(textW);
                            var decH = parseFloat(textH);

                            if (textHLength > 0)
                            {
                                //heightclicked = true;
                                //console.log(textH);
                                //console.log('before');
                                calculateMetresToFeetAndInches(textH);
                                //console.log('after');

                                if (textWLength > 0)
                                {
                                    console.log("textH: " + textH);
                                    if (textH > 0)
                                    {
                                        //alert("yes -" + decW + "-" + decH);
                                        //console.log('decW: ' + decW);
                                        //console.log('decH: ' + decH);
                                        var BMI = (decW / (decH * decH));
                                        textBoxBMI.set_value(BMI);
                                    }
                                }
                                else
                                { textBoxBMI.set_value(0); }
                            }
                            else
                            {
                                if (textHLength == 0) {
                                    var textBoxMaskedHFeet = $find('<%= RadNumericTxtFeet.ClientID %>');
                                    var textBoxMaskedHInches = $find('<%= RadNumericTxtInches.ClientID %>');

                                    textBoxMaskedHFeet.set_value("");
                                    textBoxMaskedHInches.set_value("");
                                }

                                //alert('Please enter the height');
                                textBoxBMI.set_value(0);
                            }

                            CheckQualifyButton();
                        }

                        function CheckMagnumID()
                        {
                            var textBox = $find('<%= RadTxtMagnumID.ClientID %>');
                            var text = textBox.get_textBoxValue().toString();

                            text = text.replace(/[^0-9]/g, '');
                            textBox.set_value(text.substring(0, 13));

                            CheckQualifyButton();
                        }

                        function RadComboIDTypeSelectedInxedChanged(sender, eventArgs)
                        {                            
                            var combo = $find('<%=RadComboIDType.ClientID %>');
                            var textBox = $find('<%= RadTxtIDNumber.ClientID %>');                            
                            var text = textBox.get_textBoxValue().toString();

                            if (combo.get_selectedItem().get_value() == "ID Number")
                            {
                                text = text.replace(/[^0-9]/g, '');                                
                                textBox.set_value(text.substring(0, 13));
                            }

                        }

                        function OnKeyRadTxtIDNumber(sender, eventArgs)
                        {
                            var textBox = $find('<%= RadTxtIDNumber.ClientID %>');                            
                            var text = textBox.get_textBoxValue().toString();
                            
                            //alert(text);
                            //var textbox2 = $find('<%= RadTxtAgeOfNextBirthday.ClientID %>');
                            //textbox2.set_value(text);
                            
                            var buttonValidate = $find('<%= RadButtonValidateID.ClientID %>'); 

                            var combo = $find('<%=RadComboIDType.ClientID %>');
                            if (combo.get_selectedItem().get_value() == "ID Number")
                            {
                                //obj.value = obj.value.replace(/[^0-9]/g,'');
                                text = text.replace(/[^0-9]/g, '');
                                textBox.set_value(text);
                            }

                            var textLength = text.length;
                            //alert(textLength);
                            if (textLength >= 13)
                            {                                                                
                                //alert(combo.get_selectedItem().get_value());
                                if (combo.get_selectedItem().get_value() == "ID Number")
                                {                                    
                                    //alert('yes');
                                    buttonValidate.set_enabled(true);
                                    if (textLength > 13)
                                    {
                                        textBox.set_value(text.substring(0, 13));
                                    }
                                }                                                                
                            }
                            else
                            {                                
                                buttonValidate.set_enabled(false);
                            }
                        }

                        function limitText(limitField, limitNum) {
                            if (limitField.value.length > limitNum) {
                                limitField.value = limitField.value.substring(0, limitNum);
                            }
                        }

                        function DOBDateSelected(sender, eventArgs) {
                           
                            //alert("test1");
                            //document.getElementById("RadTxtMagnumID").value = "ready to go";                            
                            var textbox = $find('<%= RadTxtAgeOfNextBirthday.ClientID %>');
                            var quoteDateDatePicker = $find('<%= RadDatePickerQuoteDate.ClientID %>');
                            var quoteDate = quoteDateDatePicker.get_selectedDate().format("yyyy/MM/dd");

                            var DOBDatePicker = $find('<%= RadDatePickerDOB.ClientID %>');
                            var DOBDate = DOBDatePicker.get_selectedDate().format("yyyy/MM/dd");

                            //alert(quoteDate)                            

                            var dt1 = new Date(Date.parse(DOBDate));
                            var dt2 = new Date(Date.parse(quoteDate));

                            var dt3 = new Date(); var one_day = 1000 * 60 * 60 * 24;
                            var days = parseInt(dt2.getTime() - dt1.getTime()) / (one_day);
                            ///alert(parseInt(dt2.getTime() - dt1.getTime()) / (one_day));

                            var call = jarh(days);                            
                            //textbox.set_value(eventArgs.get_newValue());
                            textbox.set_value(call);

                            ////RadDatePickerDateOfDiag
                            //var diagnosisDateDatePicker = $find('<%= RadDatePickerDateOfDiag.ClientID %>');
                            var year = DOBDate.substring(0, 4);
                            var Month = DOBDate.substring(5, 2);
                            //alert(DOBDate);
                            //alert(year);
                            //alert(month);
                            //diagnosisDateDatePicker.set_minDate(new Date(year, month, 1));
                            var currentTime = new Date()
                            //// returns the month (from 0 to 11)
                            var currentMonth = currentTime.getMonth() + 1
                            //// returns the day of the month (from 1 to 31)
                            ////var day = currentTime.getDate()
                            //// returns the year (four digits)
                            var currentYear = currentTime.getFullYear()// + 1
                            ////alert(currentYear);
                            ////alert(currentMonth);
                            //diagnosisDateDatePicker.set_maxDate(new Date(currentYear, currentMonth, 1));
                            ////alert("done");
                            ////var diagnosisDate = diagnosisDateDatePicker.get_selectedDate().format("yyyy/MM/dd");

                            var diagnosisMonthYearPicker = $find('<%= RadMonthYearPickerDateOfDiag.ClientID %>');
                            diagnosisMonthYearPicker.set_minDate(new Date(year, month, 1));
                            diagnosisMonthYearPicker.set_maxDate(new Date(currentYear, currentMonth, 1));

                            CheckQualifyButton();
                            //handleGenderClick(myRadio);
                        }

                        function jarh(x) {
                            var y = 365;
                            var y2 = 31;
                            var remainder = x % y;
                            var casio = remainder % y2;
                            year = (x - remainder) / y;
                            month = (remainder - casio) / y2;

                            year = year + 1;                            
                            //var result = "--- Year ---" + year + "--- Month ---" + month + "--- Day ---" + casio;
                            var result = year;
                            //alert(result);
                            return result;
                        }                        
                        
                        function handleHbA1cClick(myRadio) {                            
                            CheckQualifyButton();
                        }

                        function handleMarriedClick(myRadio) {

                            if (myRadio.value == "RadioButtonMaritalStatusMarried")
                            {
                                document.getElementById('<%=RadioButtonSNotMat.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonSMat.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonSDip.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonSDegree.ClientID %>').removeAttribute('disabled');
                                var textBox = $find('<%= RadNumericTxtSpouseIncome.ClientID %>');                                
                                textBox.enable();
                            }

                            if (myRadio.value == "RadioButtonMaritalStatusNotMarried")
                            {
                                document.getElementById('<%= RadioButtonSNotMat.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonSNotMat.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonSMat.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonSMat.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonSDip.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonSDip.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonSDegree.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonSDegree.ClientID %>').checked = false;
                                var textBox = $find('<%= RadNumericTxtSpouseIncome.ClientID %>');
                                textBox.set_value("");
                                textBox.disable();
                            }
                            
                            CheckQualifyButton();
                        }

                        function handleNotMarriedClick(myRadio) {

                            CheckQualifyButton();
                        }

                        function handleDiabetesClick(myRadio) {
                            //alert('New value: ' + myRadio.value);
                            if (myRadio.value == "RadioButtonDiabetesTypeNotSure")
                            {
                                //alert('yes');
                                document.getElementById('<%=RadioButtonInsulinYes.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonInsulinNo.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonInsulinNotSure.ClientID %>').removeAttribute('disabled');                               

                                document.getElementById('<%=RadioButtonTabletUseYes.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonTabletUseNo.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonTabletUseNotSure.ClientID %>').removeAttribute('disabled');
                            }
                            else
                            {
                                //alert('no');
                                document.getElementById('<%= RadioButtonInsulinYes.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonInsulinYes.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonInsulinNo.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonInsulinNo.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonInsulinNotSure.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonInsulinNotSure.ClientID %>').checked = false;

                                document.getElementById('<%= RadioButtonTabletUseYes.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonTabletUseYes.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonTabletUseNo.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonTabletUseNo.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonTabletUseNotSure.ClientID %>').disabled = true;                                                                
                                document.getElementById('<%= RadioButtonTabletUseNotSure.ClientID %>').checked = false;
                            }

                            CheckQualifyButton();
                        }

                        
                        function handleGenderClick(myRadio) {

                            var textBox = $find('<%= RadTxtIDNumber.ClientID %>');
                            //var text = textBox.get_value().toString();
                            var text = textBox.get_textBoxValue().toString();

                            //alert(text);
                            //var textbox2 = $find('<%= RadTxtAgeOfNextBirthday.ClientID %>');
                            //textbox2.set_value(text);

                            var buttonValidate = $find('<%= RadButtonValidateID.ClientID %>');

                            var textLength = text.length;
                            //alert(textLength);
                            if (textLength >= 13) {
                                //alert('yes');
                                var combo = $find('<%=RadComboIDType.ClientID %>');
                                //alert(combo.get_selectedItem().get_value());
                                if (combo.get_selectedItem().get_value() == "ID Number") {
                                    buttonValidate.set_enabled(true);
                                }

                            }
                            else {
                                buttonValidate.set_enabled(false);
                            }

                            CheckQualifyButton();
                        }

                        function handleExcersisePlanClick(myRadio)
                        {
                            //alert('New value: ' + myRadio.value);
                            if (myRadio.value == "RadioButtonExPYes") {
                                //alert('yes');
                                document.getElementById('<%=RadioButtonExFollowedVW.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonExFollowedok.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonExFollowedpoor.ClientID %>').removeAttribute('disabled');

                            }
                            else {
                                //alert('no');
                                document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonExFollowedVW.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonExFollowedok.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonExFollowedpoor.ClientID %>').checked = false;

                            }

                            CheckQualifyButton();
                        }

                        function handleEatPlanClick(myRadio) {
                            //alert('New value: ' + myRadio.value);
                            if (myRadio.value == "RadioButtonEatPYes") {
                                //alert('yes');
                                document.getElementById('<%=RadioButtonEatFollowedVW.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonEatFollowedOk.ClientID %>').removeAttribute('disabled');
                                document.getElementById('<%=RadioButtonEatFollowedPoor.ClientID %>').removeAttribute('disabled');

                            }
                            else {
                                //alert('no');
                                document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonEatFollowedVW.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonEatFollowedOk.ClientID %>').checked = false;
                                document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').disabled = true;
                                document.getElementById('<%= RadioButtonEatFollowedPoor.ClientID %>').checked = false;

                            }

                            CheckQualifyButton();
                        }

                        function callConfirm() {
                            radconfirm('Are you sure?', confirmCallBackFn);
                        }

                        function confirmCallBackFn(arg) {
                            var ajaxManager = $find("<%=RadAjaxManager1.ClientID%>");
                            if (arg) {
                                ajaxManager.ajaxRequest('ok');
                            }
                            else {
                                ajaxManager.ajaxRequest('cancel');
                            }
                        }

                        function confirmClearQuoteFn(arg) {
                            var ajaxManager = $find("<%=RadAjaxManager1.ClientID%>");
                            if (arg) {
                                ajaxManager.ajaxRequest('ClearQuoteOk');
                            }
                            else {
                                ajaxManager.ajaxRequest('ClearQuoteCancel');
                            }
                        }

                        function popupOpeningDOB(sender, args) {
                            //args.CancelCalendarSynchronize = true;
                            //sender.Calendar.NavigateToDate([2006, 12, 19]);
                            var DOBDatePicker = $find('<%= RadDatePickerDOB.ClientID %>');
                            if (DOBDatePicker.get_selectedDate() == null) {                                
                                dateVar = new Date();                                
                                dateVar.dateAdd('year', -17);

                                DOBDatePicker.set_selectedDate(dateVar);
                            }
                        }

                        Date.prototype.dateAdd = function (size, value) {
                            value = parseInt(value);
                            var incr = 0;
                            switch (size) {
                                case 'day':
                                    incr = value * 24;
                                    this.dateAdd('hour', incr);
                                    break;
                                case 'hour':
                                    incr = value * 60;
                                    this.dateAdd('minute', incr);
                                    break;
                                case 'week':
                                    incr = value * 7;
                                    this.dateAdd('day', incr);
                                    break;
                                case 'minute':
                                    incr = value * 60;
                                    this.dateAdd('second', incr);
                                    break;
                                case 'second':
                                    incr = value * 1000;
                                    this.dateAdd('millisecond', incr);
                                    break;
                                case 'month':
                                    value = value + this.getUTCMonth();
                                    if (value / 12 > 0) {
                                        this.dateAdd('year', value / 12);
                                        value = value % 12;
                                    }
                                    this.setUTCMonth(value);
                                    break;
                                case 'millisecond':
                                    this.setTime(this.getTime() + value);
                                    break;
                                case 'year':
                                    this.setFullYear(this.getUTCFullYear() + value);
                                    break;
                                default:
                                    throw new Error('Invalid date increment passed');
                                    break;
                            }
                        }

                        //function OnKeyPressValidateID(sender, eventArgs) {
                        //    var c = eventArgs.get_keyCode();
                        //    if (c == 13) {
                        //        __doPostBack('RadButtonValidateID', '');
                        //    }
                        //}

                    </script>
                </telerik:RadCodeBlock>   
     
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelsRenderMode="Inline" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>         
            <telerik:AjaxSetting AjaxControlID="Button1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadBtnQualifyClient" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>   
            <telerik:AjaxSetting AjaxControlID="RadTxtMagnumID">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadButtonLoadQuote" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>  
            <telerik:AjaxSetting AjaxControlID="RadBtnCheckSession"> 
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSessionCheck" UpdatePanelCssClass="" />
                </UpdatedControls>               
            </telerik:AjaxSetting>   
            <telerik:AjaxSetting AjaxControlID="RadioButtonAlcoholYes">      
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListAlcohol" UpdatePanelCssClass="" />
                </UpdatedControls>         
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButtonAlcoholNo">      
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListAlcohol" UpdatePanelCssClass="" />
                </UpdatedControls>  
            </telerik:AjaxSetting>          
            <telerik:AjaxSetting AjaxControlID="RadioButtonTobaccoYes">      
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListTobacco" UpdatePanelCssClass="" />
                </UpdatedControls>         
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButtonTobaccoNo">      
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListTobacco" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnQualifyClient">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PanelQuotePresentation" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="LblQualificationMessage" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblDisabilityMessage" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteLifeYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteLifeNo" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteDisYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteDisNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRiskModifier" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtDisabilityType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblRequalify" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtRiskBand" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_Problem" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_5" UpdatePanelCssClass="" />                  
                    <telerik:AjaxUpdatedControl ControlID="LblAcceptedQuote" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldEscalationLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldEscalationDisablility" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscalationDis6" UpdatePanelCssClass="" />  
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscalationDis10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscLife6" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscLife10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEsc6" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEsc10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTabletUseYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTabletUseNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTabletUseNotSure" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonInsulinYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonInsulinNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonInsulinNotSure" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadButtonGenerateLetter">      
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadButtonValidateID">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LabelValidationMsg" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButtonTypeOfDisADW">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" UpdatePanelCssClass="" />
                     <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnClearQuote">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" UpdatePanelCssClass="" />
                     <telerik:AjaxUpdatedControl ControlID="RadBtnClearQuote" UpdatePanelCssClass="" />                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButtonTypeOfDisOCC">
                <UpdatedControls>
                     <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                     <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtDisabilityType" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>                                        
                    <telerik:AjaxUpdatedControl ControlID="RadButtonLoadQuote" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtDisabilityType" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="RadTxtClientNameAndSurname" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValClient" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboIDType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtIDNumber" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadDatePickerQuoteDate" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtEmail" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadDatePickerDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtAgeOfNextBirthday" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMale" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonFemale" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOther" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValGender" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadButtonValidateID" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="LabelValidationMsg" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonNotMat" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMatriculated" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiploma" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDegree" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="lblValHighQual" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="lblSpouseQualification" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxOccupation" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtIncome" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="lblValIncome" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="lblSpouseIncome" UpdatePanelCssClass="" /> 
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonSNotMat" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonSMat" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonSDip" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonSDegree" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtSpouseIncome" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadDatePickerDateOfDiag" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadMonthYearPickerDateOfDiag" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValDateDiag" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetesType1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetesType2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetesTypeNotSure" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValDiabetesType" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTabletUseYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTabletUseNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTabletUseNotSure" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonInsulinYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonInsulinNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonInsulinNotSure" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDVYes1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDVYes2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDVYes3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDVNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValDocVisits" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetControlExcellent" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetControlGood" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetControlMod" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonDiabetControlPoor" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValDiabeticControl" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c6" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c7" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c8" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c9" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c11" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c12" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1c15" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHbA1cUnknown" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValHbA1c" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonExPYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonExPNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValExP" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonExFollowedVW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonExFollowedok" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonExFollowedpoor" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEatPYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEatPNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValEatP" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEatFollowedVW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEatFollowedOk" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEatFollowedPoor" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHighBPYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHighBPNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonHighBP" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblvalBP" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonCholesterolYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonCholesterolNotSure" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonCholesterolNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValChol" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMedicalAidNone" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMedicalAidNotSure" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMedicalAidComp" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMedicalAidHos" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtHeight" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValHeight" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtWeight" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValWeight" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtPantSize" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValPants" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtBMI" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonAlcoholYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonAlcoholNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValAlcohol" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListAlcohol" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTobaccoYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTobaccoNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValTobacco" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListTobacco" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnQualifyClient" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="PanelLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="PanelQuotePresentation" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="PanelDisability" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="LblQualificationMessage" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblDisabilityMessage" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtRiskBand" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadTxtDisabilityType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteLifeYes" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteLifeNo" UpdatePanelCssClass="" /> 
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteDisYes" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonQuoteDisNo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadBtnOption5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonOption5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_Problem" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="Label36" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSuitable" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSuitable0" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblAlso" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSpecificCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSpecificPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtCoverLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtCoverAmnDis" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscalationDis6" UpdatePanelCssClass="" />  
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscalationDis10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscLife6" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEscLife10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEsc6" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonEsc10" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtDesireContribution" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="LblOffer" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValAlcoholUnits" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblValTobaccoUntis" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtFeet" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtInches" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtpounds" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldEscalationLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldEscalationDisablility" UpdatePanelCssClass="" />  
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMaritalStatusNotMarried" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonMaritalStatusMarried" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNotification1" UpdatePanelCssClass="" />                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnOption1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp1_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption1Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadGridSummary" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRiskModifier" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnOption2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp2_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption2Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSuitable" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSuitable0" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblAlso" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadGridSummary" UpdatePanelCssClass="" />                                     
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRiskModifier" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnOption3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp3_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption3Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSpecificCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblSpecificPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadGridSummary" UpdatePanelCssClass="" />  
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />    
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRiskModifier" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />       
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />    
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadBtnOption4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtCoverLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtPremiumLife" UpdatePanelCssClass="" />                    
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption4Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp4_Problem" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />     
                    <telerik:AjaxUpdatedControl ControlID="RadGridSummary" UpdatePanelCssClass="" />    
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />   
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRiskModifier" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>   
            <telerik:AjaxSetting AjaxControlID="RadBtnOption5">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtCoverAmnDis" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtPremiumDis" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5RandValue" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5Premium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5DisCover" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_3" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5DisPremium" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_4" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtOption5Total" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblOp5_5" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldClassOfLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadGridSummary" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisADW" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonTypeOfDisOCC" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadNumericTxtDesireContribution" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRiskModifier" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRMDiabetesType" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldRBDOB" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadioButtonListWhoIsPaying" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitLife" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="RadComboBoxTypeBenefitDisability" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="RadButtonLoadQuote">   
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                </UpdatedControls>            
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButtonOption1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButtonOption2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="RadioButtonOption3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="RadioButtonOption4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="RadioButtonOption5">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="HiddenFieldQuoteAuditID" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblInfo2" UpdatePanelCssClass="" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
        </AjaxSettings>
    </telerik:RadAjaxManager>       
    <br />
    <br />
    <asp:Label ID="lblInfo" runat="server" Text="" ForeColor="Red"></asp:Label>
    <asp:Panel ID="PanelClientInformation" runat="server" GroupingText="Client Information" CssClass="panelStyle">   
        <telerik:RadPageLayout runat="server" GridType="Fluid" ShowGrid="false" HtmlTag="None" >
            <telerik:LayoutRow RowType="Generic">
                <Rows>                         
                    <telerik:LayoutRow RowType="Generic" CssClass="content">
                        <Rows>
                            <telerik:LayoutRow RowType="Container" WrapperHtmlTag="Div">
                                <Columns>
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12" >                                        
                                            <Table ID="Table1" runat="server">
                                                <tr>
                                                    <td colspan="3">
                                                        <telerik:RadButton ID="RadBtnCheckSession" ButtonType="LinkButton" runat="server" Text="Check session" OnClick="RadBtnCheckSession_Click"></telerik:RadButton>
                                                        <asp:Label ID="lblSessionCheck" runat="server" Text="" ></asp:Label>   
                                                        <telerik:RadButton ID="RadBtnClearQuote" ButtonType="LinkButton" runat="server" Text="Clear Quote" OnClick="RadBtnClearQuote_Click"></telerik:RadButton>                                                     
                                                    </td>                                                    
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px;">
                                                        <asp:Label ID="lblMagnumID" runat="server" Text="Magnum ID:" ></asp:Label>
                                                    </td>
                                                    <td>      
                                                        <telerik:RadTextBox ID="RadTxtMagnumID" runat="server" TabIndex="1" MaxLength="8" onkeyup="CheckMagnumID()"></telerik:RadTextBox>                                                       
                                                        <asp:Label ID="lblValMagnum" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadButton ID="RadButtonLoadQuote" runat="server" Text="Load Quote" Enabled="false" OnClick="RadButtonLoadQuote_Click"></telerik:RadButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblIDType" runat="server" Text="ID Type:"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadComboBox ID="RadComboIDType" runat="server" TabIndex="3" OnClientSelectedIndexChanged="RadComboIDTypeSelectedInxedChanged">
                                                             <Items>
                                                                <telerik:RadComboBoxItem runat="server" Text="ID Number" Value="ID Number" />
                                                                <telerik:RadComboBoxItem runat="server" Text="Passport" Value="Passport" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblQuoteDate" runat="server" Text="Quote Date:"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <telerik:RadDatePicker ID="RadDatePickerQuoteDate" runat="server" MinDate="" TabIndex="5">
                                                            <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy"></DateInput>
                                                            <ClientEvents OnDateSelected="DOBDateSelected" />
                                                            <Calendar ID="Calendar1" runat="server"> 
                                                                <SpecialDays> 
                                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Red"> 
                                                                    </telerik:RadCalendarDay> 
                                                                </SpecialDays> 
                                                            </Calendar> 
                                                        </telerik:RadDatePicker>
                                                        <asp:Label ID="Label54" runat="server" Text="e.g. DD/MM/YYYY"></asp:Label>
                                                    </td>
                                                </tr>
                                            </Table>
                                    </telerik:LayoutColumn>                                    
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12">
                                        <Table ID="Table3" runat="server">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                   &nbsp;
                                                </td>
                                                 <td>
                                                    &nbsp;
                                                 </td>
                                                 <td>
                                                     &nbsp;
                                                 </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LabelClientNameAndSurname" runat="server" Text="Client name and surname:" ></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <telerik:RadTextBox ID="RadTxtClientNameAndSurname" runat="server" Width="250" TabIndex="2" onkeyup="CheckQualifyButton()">
                                                        <ClientEvents OnKeyPress="OnKeyPressSerialText"/>
                                                    </telerik:RadTextBox><asp:Label ID="lblValClient" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:Label ID="LabelID" runat="server" Text="ID No:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTxtIDNumber" runat="server" Width="250" TabIndex="4" onkeyup="OnKeyRadTxtIDNumber(event)">                                                        
                                                    </telerik:RadTextBox>
                                                </td>
                                                 <td style="vertical-align: middle">
                                                      
                                                 </td>
                                                 <td>
                                                     
                                                 </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:Label ID="LabelEmail" runat="server" Text="Email Address:"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <telerik:RadTextBox ID="RadTxtEmail" runat="server" Width="250" TabIndex="6"></telerik:RadTextBox> <asp:RegularExpressionValidator
                                                    id="emailValidator"
                                                    runat="server"
                                                    Display="Dynamic"
                                                    ErrorMessage="Please, enter valid e-mail address"
                                                    ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                                    ControlToValidate="RadTxtEmail" ForeColor="Red">
                                                </asp:RegularExpressionValidator>

                                                </td>                                                
                                            </tr>
                                        </Table>
                                    </telerik:LayoutColumn>
                                    
                                </Columns>
                            </telerik:LayoutRow>
                        </Rows>
                    </telerik:LayoutRow>                    
                </Rows>
            </telerik:LayoutRow>
        </telerik:RadPageLayout>
        </asp:Panel>
        <br />
        <asp:Panel ID="PanelQuoteInformation" runat="server" GroupingText="Quote Information">
            <telerik:RadPageLayout runat="server" GridType="Fluid" ShowGrid="false" HtmlTag="None" >
                <telerik:LayoutRow RowType="Generic">
                <Rows>                         
                    <telerik:LayoutRow RowType="Generic" CssClass="content">
                        <Rows>
                            <telerik:LayoutRow RowType="Container" WrapperHtmlTag="Div">
                                <Columns>
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12" >
                                        <Table ID="Table2" runat="server">
                                            <tr>
                                                <td width="100px">
                                                    <asp:Label ID="LabelDOB" runat="server" Text="Date of Birth:"></asp:Label>                                                    
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="RadDatePickerDOB" runat="server" MinDate="1940-01-01" TabIndex="7">                                                         
                                                        <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy"></DateInput>
                                                        <ClientEvents OnDateSelected="DOBDateSelected" />
                                                        <ClientEvents OnPopupOpening="popupOpeningDOB" />
                                                    </telerik:RadDatePicker><asp:Label ID="lblValDOB" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    <asp:Label ID="LabelDOBeg" runat="server" Text="e.g. DD/MM/YYYY"></asp:Label>
                                                </td>                                                   
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LabelGender" runat="server" Text="Gender:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonMale" runat="server" GroupName="Gender" Text="Male" TabIndex="8" Font-Size="Smaller" onclick="handleGenderClick(this);"/>
                                                    <asp:RadioButton ID="RadioButtonFemale" runat="server" GroupName="Gender" Text="Female" TabIndex="9" Font-Size="Smaller" onclick="handleGenderClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonOther" runat="server" GroupName="Gender" Text="Other" TabIndex="10" Font-Size="Smaller" onclick="handleGenderClick(this)"/>
                                                    <asp:Label ID="lblValGender" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    <telerik:RadButton ID="RadButtonValidateID" runat="server" OnClick="RadButtonValidateID_Click" Text="Validate ID" Enabled="true"></telerik:RadButton>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="LabelValidationMsg" runat="server" Text="ID not validated" BackColor="#999999" ForeColor="White"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LabelQualification" runat="server" Text="Highest qualification:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonNotMat" runat="server" GroupName="Qual" Text="No matric" TabIndex="11" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonMatriculated" runat="server" GroupName="Qual" Text="Matric" TabIndex="12" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonDiploma" runat="server" GroupName="Qual" Text="3 or 4 yr. Diploma/3 yr. Degree" TabIndex="13" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:RadioButton ID="RadioButtonDegree" runat="server" GroupName="Qual" Text="4 yr. Degree/professional qualification" TabIndex="14" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValHighQual" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label43" runat="server" Text="Occupation:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="RadComboBoxOccupation" runat="server" Width="250px" TabIndex="15" onclientselectedindexchanged="CheckQualifyButton"></telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LabelIncome" runat="server" Text="Income:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtIncome" TabIndex="16" runat="server" Culture="en-ZA" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="160px" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=" " onkeyup="CheckQualifyButton()">
                                                    <NegativeStyle Resize="None"></NegativeStyle>
                                                    <NumberFormat ZeroPattern="R n"></NumberFormat>
                                                    <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                                    <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                    <FocusedStyle Resize="None"></FocusedStyle>
                                                    <DisabledStyle Resize="None"></DisabledStyle>
                                                    <InvalidStyle Resize="None"></InvalidStyle>
                                                    <HoveredStyle Resize="None"></HoveredStyle>
                                                    <EnabledStyle Resize="None"></EnabledStyle></telerik:RadNumericTextBox>
                                                    <asp:Label ID="lblValIncome" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>   
                                            </tr>  
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label46" runat="server" Text="Marital Status:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonMaritalStatusMarried" runat="server" GroupName="MaritalStatus" Text="Married" TabIndex="17" Font-Size="Smaller" Checked="true" onclick="handleMarriedClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonMaritalStatusNotMarried" runat="server" GroupName="MaritalStatus" Text="Not Married" TabIndex="17" Font-Size="Smaller" onclick="handleMarriedClick(this)"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;
                                                </td>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;
                                                </td>
                                            </tr>    
                                             <tr>
                                                <td width="120px">
                                                    <asp:Label ID="LabelDOD" runat="server" Text="Year of diagnosis:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadMonthYearPicker ID="RadMonthYearPickerDateOfDiag" Runat="server" MonthYearNavigationSettings-DisableOutOfRangeMonths="true" Culture="en-ZA" HiddenInputTitleAttibute="Visually hidden input created for functionality purposes." MinDate="1900-01-01" OnViewCellCreated="RadMonthYearPicker1_ViewCellCreated" TabIndex="22">
                                                        <ClientEvents OnDateSelected="CheckQualifyButton" />
                                                        <DateInput DisplayDateFormat="yyyy" DateFormat="yyyy" LabelWidth="40%">
                                                        </DateInput>        
                                                    </telerik:RadMonthYearPicker>                                                    
                                                    <asp:Label ID="lblValDateDiag" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    <telerik:RadDatePicker ID="RadDatePickerDateOfDiag" runat="server" MinDate="1900-01-01" Visible="false">
                                                        <ClientEvents OnDateSelected="CheckQualifyButton" />
                                                    </telerik:RadDatePicker>
                                                </td>                                                   
                                            </tr>                                            
                                            <tr>
                                                <td >
                                                    <asp:Label ID="Label13" runat="server" Text="Diabetes Type:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonDiabetesType1" runat="server" GroupName="DiabetesType" Text="Type 1" Font-Size="Smaller" TabIndex="23" onclick="handleDiabetesClick(this);"/>
                                                    <asp:RadioButton ID="RadioButtonDiabetesType2" runat="server" GroupName="DiabetesType" Text="Type 2" Font-Size="Smaller" TabIndex="24" onclick="handleDiabetesClick(this);"/>
                                                    <asp:RadioButton ID="RadioButtonDiabetesTypeNotSure" runat="server" GroupName="DiabetesType" Text="Not sure" Font-Size="Smaller" TabIndex="25" onclick="handleDiabetesClick(this);" />                                                    
                                                    <asp:Label ID="lblValDiabetesType" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>    
                                            <tr>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;
                                                </td>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    <asp:Label ID="Label4" runat="server" Text="Doctor's visits:"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    <asp:RadioButton ID="RadioButtonDVYes1" runat="server" GroupName="DocVisits" Text="Yes, every 4-6 months" Font-Size="Smaller" TabIndex="32" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonDVYes2" runat="server" GroupName="DocVisits" Text="Yes, but not regular / not frequent" Font-Size="Smaller" TabIndex="33" onclick="handleHbA1cClick(this)"/>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:RadioButton ID="RadioButtonDVYes3" runat="server" GroupName="DocVisits" Text="Yes, but not sure" Font-Size="Smaller" TabIndex="34" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonDVNo" runat="server" GroupName="DocVisits" Text="No / Not known" Font-Size="Smaller" TabIndex="35" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValDocVisits" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label41" runat="server" Text="Diabetic Control:"></asp:Label>
                                                </td>
                                                <td >
                                                    <asp:RadioButton ID="RadioButtonDiabetControlExcellent" runat="server" GroupName="DiabetControl" Text="Excellent" Font-Size="Smaller" ToolTip="Excellently controlled over the last 3 years without many interruptions" TabIndex="36" onclick="handleHbA1cClick(this)"/>                                                                                                                                                            
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>

                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonDiabetControlGood" runat="server" GroupName="DiabetControl" Text="Good" Font-Size="Smaller" ToolTip="Well controlled but with some interruptions due to lifestyle factors" TabIndex="37" onclick="handleHbA1cClick(this)"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>

                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonDiabetControlMod" runat="server" GroupName="DiabetControl" Text="Moderate" Font-Size="Smaller" ToolTip="Trying to control your diabetes but with many interruptions of control, but working with your Doctor to improve your habits" TabIndex="38" onclick="handleHbA1cClick(this)"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">

                                                </td>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    <asp:RadioButton ID="RadioButtonDiabetControlPoor" runat="server" GroupName="DiabetControl" Text="Poor" Font-Size="Smaller" ToolTip="Not able to maintain good control most of the time" TabIndex="39" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValDiabeticControl" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>  
                                            <tr>
                                                <td>   
                                                    &nbsp;                                                 
                                                </td>
                                                <td> 
                                                    &nbsp;                                                  
                                                </td>
                                            </tr>                                         
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label42" runat="server" Text="Recent HbA1c%:"></asp:Label>
                                                </td>
                                                <td>                                                    
                                                    <asp:RadioButton ID="RadioButtonHbA1c3" runat="server" GroupName="HbA1c" Text="Post-UW < 5%" Font-Size="Smaller" TabIndex="40" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHbA1c4" runat="server" GroupName="HbA1c" Text="< 5%" Font-Size="Smaller" TabIndex="40" onclick="handleHbA1cClick(this)" />
                                                    <asp:RadioButton ID="RadioButtonHbA1c6" runat="server" GroupName="HbA1c" Text="5% - 6.99%" Font-Size="Smaller" TabIndex="41" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHbA1c7" runat="server" GroupName="HbA1c" Text="7% - 7.99%" Font-Size="Smaller" TabIndex="42" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHbA1c8" runat="server" GroupName="HbA1c" Text="8% - 8.99%" Font-Size="Smaller" TabIndex="43" onclick="handleHbA1cClick(this)"/>
                                                    &nbsp;
                                                    <asp:RadioButton ID="RadioButtonHbA1c9" runat="server" GroupName="HbA1c" Text="9% - 9.99%" Font-Size="Smaller" TabIndex="44" onclick="handleHbA1cClick(this)"/>                                                    
                                                    <asp:RadioButton ID="RadioButtonHbA1c10" runat="server" GroupName="HbA1c" Text="10% - 10.99%" Font-Size="Smaller" TabIndex="45" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHbA1c11" runat="server" GroupName="HbA1c" Text="11% - 11.99%" Font-Size="Smaller" TabIndex="46" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHbA1c12" runat="server" GroupName="HbA1c" Text="12% - 13.99%" Font-Size="Smaller" TabIndex="47" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHbA1c15" runat="server" GroupName="HbA1c" Text="≥ 14%" Font-Size="Smaller" TabIndex="48" onclick="handleHbA1cClick(this)"/>                                                    
                                                    <asp:RadioButton ID="RadioButtonHbA1cUnknown" runat="server" GroupName="HbA1c" Text="Unknown" Font-Size="Smaller" TabIndex="49" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValHbA1c" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>    
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>                                                                            
                                            <tr>
                                                <td>
                                                </td>
                                                <td>                                                    
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                </td>
                                                <td>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label58" runat="server" Text="|" ForeColor="white" Font-Size="Large"></asp:Label>
                                                </td>                                            
                                            </tr>  
                                            <tr>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin;">
                                                    <asp:Label ID="Label7" runat="server" Text="High BP:"></asp:Label>
                                                </td>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin;">
                                                    <asp:RadioButton ID="RadioButtonHighBPYes" runat="server" GroupName="HighBP" Text="Yes" Font-Size="Smaller" TabIndex="60" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHighBPNo" runat="server" GroupName="HighBP" Text="No" Font-Size="Smaller" TabIndex="61" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonHighBP" runat="server" GroupName="HighBP" Text="Not sure" Font-Size="Smaller" TabIndex="62" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblvalBP" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td>
                                                    <asp:Label ID="Label17" runat="server" Text="High Cholesterol:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButtonCholesterolYes" runat="server" GroupName="Cholesterol" Text="Yes" Font-Size="Smaller" TabIndex="63" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonCholesterolNo" runat="server" GroupName="Cholesterol" Text="No" Font-Size="Smaller" TabIndex="64" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonCholesterolNotSure" runat="server" GroupName="Cholesterol" Text="Not sure" Font-Size="Smaller" TabIndex="65" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValChol" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>                                                
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server" Text="Height:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtHeight" runat="server" Width="65px" MaxValue="2.4" MinValue="1.2" NumberFormat-DecimalDigits="2" IncrementSettings-InterceptArrowKeys="true" TabIndex="70" onkeyup="OnKeyRadTxtWeight()"></telerik:RadNumericTextBox>                                                    
                                                    <asp:Label ID="Label19" runat="server" Text="( m) Or  "></asp:Label><asp:Label ID="Label44" runat="server" Text="_" ForeColor="White"></asp:Label>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtFeet" runat="server" Width="50px" MaxValue="7" MinValue="3" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true" TabIndex="70" onkeyup="OnKeyUpMaskedTxtHeight()"></telerik:RadNumericTextBox>
                                                    <asp:Label ID="Label20" runat="server" Text="feet & "></asp:Label>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtInches" runat="server" Width="50px" TabIndex="70" NumberFormat-DecimalDigits="2" onkeyup="OnKeyUpMaskedTxtHeight()"></telerik:RadNumericTextBox>
                                                    <asp:Label ID="Label39" runat="server" Text="inches  "></asp:Label>
                                                    <asp:Label ID="lblValHeight" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>   
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label18" runat="server" Text="Weight:"></asp:Label>
                                                    
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtWeight" runat="server" Width="65px" MaxValue="560" MinValue="30" NumberFormat-DecimalDigits="2" IncrementSettings-InterceptArrowKeys="true" TabIndex="71" onkeyup="kToLbs()"></telerik:RadNumericTextBox>
                                                    <asp:Label ID="Label36" runat="server" Text="(kg) Or  "></asp:Label><asp:Label ID="Label40" runat="server" Text="_" ForeColor="White"></asp:Label>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtpounds" runat="server" Width="50px" MaxValue="1234" MinValue="66" NumberFormat-DecimalDigits="2" IncrementSettings-InterceptArrowKeys="true" TabIndex="71" onkeyup="lbsAndOzToK()"></telerik:RadNumericTextBox>
                                                    <asp:Label ID="Label38" runat="server" Text="pounds "></asp:Label>
                                                    <asp:Label ID="lblValWeight" runat="server" Text="*" ForeColor="Red"></asp:Label>                                                    
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtOunces" runat="server" Visible="false" Width="50px" TabIndex="71" NumberFormat-DecimalDigits="0" onkeyup="lbsAndOzToK()"></telerik:RadNumericTextBox>                                                    
                                                </td>   
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="BMI:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtBMI" runat="server" IncrementSettings-InterceptArrowKeys="true" ReadOnly="true"></telerik:RadNumericTextBox>
                                                </td>  
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                             <tr>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin;">
                                                    <asp:Label ID="Label11" runat="server" Text="Consume alcohol:"></asp:Label>
                                                </td>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin;">
                                                    <asp:RadioButtonList ID="RadioButtonListAlcohol" runat="server" Font-Size="Smaller" CssClass="radioVAlign" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static" TabIndex="74" onclick="CheckQualifyButton()">
                                                        <asp:ListItem>non-drinker</asp:ListItem>
                                                        <asp:ListItem>0 - 5</asp:ListItem>
                                                        <asp:ListItem>&gt; 5</asp:ListItem>
                                                    </asp:RadioButtonList><asp:Label ID="lblValAlcoholUnits" runat="server" Text="*" ForeColor="Red"></asp:Label>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label12" runat="server" Text="Uses tobacco:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="RadioButtonListTobacco" runat="server" Font-Size="Smaller" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static" TabIndex="76" onclick="CheckQualifyButton()">                                                        
                                                        <asp:ListItem>non-smoker</asp:ListItem>
                                                        <asp:ListItem>< 1</asp:ListItem>
                                                        <asp:ListItem>1 - 5</asp:ListItem>
                                                        <asp:ListItem>6 - 20</asp:ListItem>
                                                        <asp:ListItem>&gt; 20</asp:ListItem>
                                                    </asp:RadioButtonList><asp:Label ID="lblValTobaccoUntis" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>                                            
                                            </Table>
                                    </telerik:LayoutColumn>
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12">
                                        <Table ID="Table4" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LabelANB" runat="server" Text="Age at next birthday:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTxtAgeOfNextBirthday" runat="server" ReadOnly="true"></telerik:RadTextBox>
                                                </td>                                                   
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label47" runat="server" Text="|" ForeColor="white" Font-Size="X-Large"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>                                                                                                                                
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label51" runat="server" Text="|" ForeColor="white" Font-Size="X-Large"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>     
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label52" runat="server" Text="|" ForeColor="white" Font-Size="X-Large"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td style="width: 160px; border-top-style: dashed; border-top-color: #808080; border-top-width: thin; border-left-style: dashed; border-left-color: #808080; border-left-width:thin">
                                                    <asp:Label ID="Label2" runat="server" Text="Spouse's highest qualification:"></asp:Label>
                                                </td>
                                                <td style="border-right-style: dashed; border-right-color: #808080; border-right-width: thin; border-top-style: dashed; border-top-color: #808080; border-top-width: thin">
                                                    <asp:RadioButton ID="RadioButtonSNotMat" runat="server" GroupName="SQual" Text="No matric" TabIndex="17" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonSMat" runat="server" GroupName="SQual" Text="Matric" TabIndex="18" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonSDip" runat="server" GroupName="SQual" Text="3 or 4 yr. Diploma/3 yr. Degree" TabIndex="19" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:RadioButton ID="RadioButtonSDegree" runat="server" GroupName="SQual" Text="4 yr. Degree/professional qualification" TabIndex="20" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblSpouseQualification" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-left-style: dashed; border-left-color: #808080; border-left-width:thin;">
                                                    <asp:Label ID="Label3" runat="server" Text="Spouse's income:"></asp:Label>
                                                </td>
                                                <td style="border-right-style: dashed; border-right-color: #808080; border-right-width: thin;">
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtSpouseIncome" TabIndex="21" runat="server" Culture="en-ZA" DbValueFactor="1" LabelWidth="64px" Type="Currency" Width="160px" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator=" " onkeyup="CheckQualifyButton()">
                                                    <NegativeStyle Resize="None"></NegativeStyle>
                                                    <NumberFormat ZeroPattern="R n"></NumberFormat>
                                                    <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                                    <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                    <FocusedStyle Resize="None"></FocusedStyle>
                                                    <DisabledStyle Resize="None"></DisabledStyle>
                                                    <InvalidStyle Resize="None"></InvalidStyle>
                                                    <HoveredStyle Resize="None"></HoveredStyle>
                                                    <EnabledStyle Resize="None"></EnabledStyle></telerik:RadNumericTextBox>
                                                    <asp:Label ID="lblSpouseIncome" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>   
                                            </tr>
                                            <tr>
                                                <td style="border-left-style: dashed; border-left-color: #808080; border-left-width:thin; border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;
                                                </td>
                                                <td style="border-right-style: dashed; border-right-color: #808080; border-right-width: thin; border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;
                                                </td>
                                            </tr>    
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>                                                                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label50" runat="server" Text="|" ForeColor="white" Font-Size="Small"></asp:Label>
                                                </td>                                                
                                            </tr>    
                                            <tr>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin; border-left-style: dashed; border-left-color: #808080; border-left-width:thin">
                                                    <asp:Label ID="LabelInsulin" runat="server" Text="Insulin use:" Visible="true"></asp:Label>
                                                </td>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin; border-right-style: dashed; border-right-color: #808080; border-right-width:thin">
                                                    <asp:RadioButton ID="RadioButtonInsulinYes" runat="server" GroupName="Insulin" Text="Yes" Font-Size="Small" Enabled="true"  TabIndex="26" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonInsulinNo" runat="server" GroupName="Insulin" Text="No" Font-Size="Small"  Enabled="true" TabIndex="27" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonInsulinNotSure" runat="server" GroupName="Insulin" Text="Not sure" Font-Size="Small" Enabled="true" TabIndex="28" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValInsulin" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin; border-left-style: dashed; border-left-color: #808080; border-left-width:thin">
                                                    <asp:Label ID="Label14" runat="server" Text="Tablet use:"></asp:Label>
                                                </td>
                                                <td style="border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin; border-right-style: dashed; border-right-color: #808080; border-right-width:thin">
                                                    <asp:RadioButton ID="RadioButtonTabletUseYes" CssClass="radioVAlign" runat="server" GroupName="TabletUse" Text="Yes" Font-Size="Smaller" TabIndex="29" Enabled="true" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonTabletUseNo" CssClass="radioVAlign" runat="server" GroupName="TabletUse" Text="No" Font-Size="Smaller" TabIndex="30" Enabled="true" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonTabletUseNotSure" CssClass="radioVAlign" runat="server" GroupName="TabletUse" Text="Not sure" Font-Size="Smaller" TabIndex="31" Enabled="true" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValTablet" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                 <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                 <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label61" runat="server" Text="|" ForeColor="white" Font-Size="Small"></asp:Label>
                                                </td>                                            
                                            </tr>  
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label37" runat="server" Font-Size="Small" Text="Excellent - Excellently controlled over the last 3 years without many interruptions"></asp:Label>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label48" runat="server" Font-Size="Small" Text="Good - Well controlled but with some interruptions due to lifestyle factors"></asp:Label>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label53" runat="server" Font-Size="Small" Text="Moderate - Trying to control your diabetes but with many interruptions of control, but working with your Doctor to improve your habits"></asp:Label>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label57" runat="server" Font-Size="Small" Text="Poor - Not able to maintain good control most of the time"></asp:Label>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                    &nbsp;
                                                </td>
                                            </tr>     
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>                                            
                                           <tr>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin; border-left-style: dashed; border-left-color: #808080; border-left-width: thin;">
                                                    <asp:Label ID="Label5" runat="server" Text="Exercise plan:"></asp:Label>
                                                </td>
                                                <td  style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin; border-right-style: dashed; border-right-color: #808080; border-right-width: thin;">
                                                    <asp:RadioButton ID="RadioButtonExPYes" runat="server" GroupName="ExP" Enabled="true" Text="Yes" Font-Size="Smaller" TabIndex="50" onclick="handleExcersisePlanClick(this);"/>
                                                    <asp:RadioButton ID="RadioButtonExPNo" runat="server" GroupName="ExP" Enabled="true" Text="No" Font-Size="Smaller" TabIndex="51" onclick="handleExcersisePlanClick(this);"/>
                                                    <asp:Label ID="lblValExP" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    
                                                </td>
                                            </tr>    
                                            <tr>
                                                <td style="border-left-style: dashed; border-left-color: #808080; border-left-width: thin;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label15" runat="server" Text="How well followed:"></asp:Label>
                                                </td>
                                                <td style="border-right-style: dashed; border-right-color: #808080; border-right-width: thin;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="RadioButtonExFollowedVW" runat="server" GroupName="ExFollowed" Text="Very well" Font-Size="Smaller" Enabled="true" TabIndex="52" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonExFollowedok" runat="server" GroupName="ExFollowed" Text="Okay" Font-Size="Smaller" Enabled="true" TabIndex="53" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonExFollowedpoor" runat="server" GroupName="ExFollowed" Text="Poorly" Font-Size="Smaller" Enabled="true" TabIndex="54" onclick="handleHbA1cClick(this)"/>                                                    
                                                    <asp:Label ID="lblValExFollow" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-left-style: dashed; border-left-color: #808080; border-left-width: thin;">
                                                    <asp:Label ID="Label6" runat="server" Text="Eating plan:"></asp:Label>
                                                </td>
                                                <td style="border-right-style: dashed; border-right-color: #808080; border-right-width: thin;">
                                                    <asp:RadioButton ID="RadioButtonEatPYes" runat="server" GroupName="EatP" Enabled="true" Text="Yes" Font-Size="Smaller" TabIndex="55" onclick="handleEatPlanClick(this);"/>
                                                    <asp:RadioButton ID="RadioButtonEatPNo" runat="server" GroupName="EatP" Enabled="true" Text="No" Font-Size="Smaller" TabIndex="56" onclick="handleEatPlanClick(this);"/>
                                                    <asp:Label ID="lblValEatP" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>        
                                            <tr>
                                                <td style="border-left-style: dashed; border-left-color: #808080; border-left-width: thin; border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label16" runat="server" Text="How well followed:"></asp:Label>
                                                </td>
                                                <td style="border-right-style: dashed; border-right-color: #808080; border-right-width: thin;border-bottom-style: dashed; border-bottom-color: #808080; border-bottom-width: thin;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="RadioButtonEatFollowedVW" runat="server" GroupName="EatFollowed" Text="Very well" Font-Size="Smaller" Enabled="true" TabIndex="57" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonEatFollowedOk" runat="server" GroupName="EatFollowed" Text="Okay" Font-Size="Smaller" Enabled="true" TabIndex="58" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonEatFollowedPoor" runat="server" GroupName="EatFollowed" Text="Poorly" Font-Size="Smaller" Enabled="true" TabIndex="59" onclick="handleHbA1cClick(this)"/>                                                    
                                                    <asp:Label ID="lblEatFollow" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>                                                                                                                                                                                                                                                                                                   
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label59" runat="server" Text="|" ForeColor="white" Font-Size="Small"></asp:Label>
                                                </td>                                            
                                            </tr>  
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label60" runat="server" Text="|" ForeColor="white" Font-Size="Small"></asp:Label>
                                                </td>                                            
                                            </tr>                                            
                                            <tr>
                                               <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin;">
                                                    <asp:Label ID="Label8" runat="server" Text="Medical aid:"></asp:Label>
                                                </td>
                                                <td style="border-top-style: dashed; border-top-color: #808080; border-top-width: thin;">
                                                    <asp:RadioButton ID="RadioButtonMedicalAidNone" runat="server" GroupName="MedicalAid" Enabled="true" Text="None" Font-Size="Smaller" TabIndex="66" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonMedicalAidNotSure" runat="server" GroupName="MedicalAid" Enabled="true" Text="Not sure" Font-Size="Smaller" TabIndex="67" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonMedicalAidComp" runat="server" GroupName="MedicalAid" Enabled="true" Text="Comprehensive" Font-Size="Smaller" TabIndex="68" onclick="handleHbA1cClick(this)"/>
                                                    <asp:RadioButton ID="RadioButtonMedicalAidHos" runat="server" GroupName="MedicalAid" Enabled="true" Text="Hospital plan" Font-Size="Smaller" TabIndex="69" onclick="handleHbA1cClick(this)"/>
                                                    <asp:Label ID="lblValMedcialAid" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label49" runat="server" Text="|" ForeColor="white" Font-Size="Large"></asp:Label>
                                                </td>                                            
                                            </tr>                                              
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" Text="Pants size (inches):"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtPantSize" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true" TabIndex="72" onkeyup="OnKeyUpTxtPantsSize()"></telerik:RadNumericTextBox>
                                                    <asp:Label ID="lblValPants" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>   
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label45" runat="server" Text="Dress size:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadNumericTextBox ID="RadNumericTxtDressSize" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true" TabIndex="72" onkeyup="OnKeyUpTxtDressSize()"></telerik:RadNumericTextBox>
                                                </td>  
                                            </tr>
                                             <tr>
                                                <td>
                                                    
                                                </td>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    
                                                </td>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                            </Table>
                                    </telerik:LayoutColumn>
                                </Columns>
                            </telerik:LayoutRow>
                        </Rows>
                    </telerik:LayoutRow>
                </Rows>
                </telerik:LayoutRow>
            </telerik:RadPageLayout>
        </asp:Panel>
    &nbsp;         
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Runat="server" Skin="Default" IsSticky="false" Style="position: absolute; top: 0; left: 0; height: 100%; width: 100%; margin:0; padding:0;">    
        </telerik:RadAjaxLoadingPanel>
        <asp:Panel ID="PanelProductAndRiskQualification" runat="server" GroupingText="Product and risk qualification">
            <telerik:RadPageLayout runat="server" GridType="Fluid" ShowGrid="false" HtmlTag="None" >
            <telerik:LayoutRow RowType="Generic">
                <Rows>                         
                    <telerik:LayoutRow RowType="Generic" CssClass="content">
                        <Rows>
                            <telerik:LayoutRow RowType="Container" WrapperHtmlTag="Div">
                                <Columns>
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12" >                                        
                                            <Table ID="Table5" runat="server">
                                                <tr>
                                                    <td colspan="2">                                                        
                                                        <telerik:RadButton ID="RadBtnQualifyClient" runat="server" Text="Qualify Client" OnClick="RadBtnQualifyClient_Click" Enabled="false" ></telerik:RadButton>
                                                        <asp:HiddenField ID="HiddenFieldQuoteAuditID" runat="server" />
                                                        <asp:Label ID="lblRequalify" runat="server" Text="" ForeColor="Red" ></asp:Label>
                                                    </td>                                                                                                      
                                                </tr>
                                            </Table>
                                            <Table ID="Table7" runat="server">
                                                <tr>                                                    
                                                    <td>
                                                        <asp:Label ID="Label22" runat="server" Text="Life:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="LblQualificationMessage" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>                                              
                                                <tr>                                                    
                                                    <td>
                                                        <asp:Label ID="Label21" runat="server" Text="Disability:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDisabilityMessage" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadButton ID="RadButtonWhyCantIQuality" ButtonType="LinkButton" runat="server" Text="Why can't I qualify?" OnClientClicking="WhyNoQualify" AutoPostBack="false"></telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </Table>
                                    </telerik:LayoutColumn>                                    
                                    <telerik:LayoutColumn Span="6" SpanSm="8" SpanXs="12">
                                        <Table ID="Table6" runat="server">
                                            <tr>
                                                <td>
                                                    &nbsp; 
                                                </td>
                                                <td>
                                                    &nbsp; 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label24" runat="server" Text="Risk band:"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <telerik:RadTextBox ID="RadTxtRiskBand" runat="server" ></telerik:RadTextBox>
                                                </td>
                                            </tr>                                           
                                             <tr>
                                                <td>
                                                    <asp:Label ID="Label23" runat="server" Text="Disability type:"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTxtDisabilityType" runat="server" ></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </Table>
                                    </telerik:LayoutColumn>
                                    
                                </Columns>
                            </telerik:LayoutRow>
                        </Rows>
                    </telerik:LayoutRow>                    
                </Rows>
            </telerik:LayoutRow>
        </telerik:RadPageLayout>

            <table>
                <tr>
                    <td>

                    </td>
                </tr>
            </table>
        </asp:Panel>
    <asp:Label ID="lblInfo2" runat="server" Text="" ForeColor="Red"></asp:Label>
    <br />
    <div id="divQuotePresentation">
    <asp:Panel ID="PanelQuotePresentation" runat="server" GroupingText="Quote Presentation" Visible="false">
     <table> <!-- width=100% border=0>-->
            <tr> 
                <td> <!-- width="100%">-->
                    <table> <!-- border=0 align=center>-->
                        <tr>
                            <td>
                                <asp:Panel ID="PanelEscalationOption" runat="server" BorderStyle="Solid" BorderWidth="1px" GroupingText="Escalation Option">
                                <table style="text-align: center">                                   
                                     <tr>                                            
                                        <td>                                            
                                                                                   
                                            <asp:Label ID="Label62" runat="server" Text="Escalation:"></asp:Label>
                                                                                   
                                        </td>
                                         <td>
                                                <asp:RadioButton ID="RadioButtonEsc6" runat="server" GroupName="EscalationBtn" Text="6%" Font-Size="Smaller" onclick="handleHbA1cClick(this)" Checked="True"/>
                                                <asp:RadioButton ID="RadioButtonEsc10" runat="server" GroupName="EscalationBtn" Text="10%" Font-Size="Smaller" onclick="handleHbA1cClick(this)"/>
                                         </td>
                                      </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>   
                                <asp:Panel ID="PanelLife" runat="server" GroupingText="Life" BorderStyle="Solid" BorderWidth="1px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Text="Type of benefit:"></asp:Label>
                                            </td>
                                            <td>
                                                
                                                <telerik:RadComboBox ID="RadComboBoxTypeBenefitLife" Runat="server" onclientselectedindexchanged="CheckQualifyButton"></telerik:RadComboBox>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Text="Escalation:" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButtonEscLife6" runat="server" GroupName="EscalationLife" Text="6%" Font-Size="Smaller" onclick="handleHbA1cClick(this)" Visible="False"/>
                                                <asp:RadioButton ID="RadioButtonEscLife10" runat="server" GroupName="EscalationLife" Text="10%" Font-Size="Smaller" onclick="handleHbA1cClick(this)" Visible="False"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label27" runat="server" Text="Quote:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButtonQuoteLifeYes" runat="server" GroupName="QuoteLife" Text="Yes" Font-Size="Smaller" AutoPostBack="True"/>
                                                <asp:RadioButton ID="RadioButtonQuoteLifeNo" runat="server" GroupName="QuoteLife" Text="No" Font-Size="Smaller" AutoPostBack="True"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label28" runat="server" Text="Cover amount:"></asp:Label>
                                            </td>
                                            <td>                                                
                                                <telerik:RadNumericTextBox ID="RadNumericTxtCoverLife" Runat="server" Type="Currency" NumberFormat-DecimalDigits="2" Culture="en-ZA" onkeyup="OnKeyUpTxtCoverCalcLife()">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label29" runat="server" Text="Premium:" ></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtPremiumLife" Runat="server" Type="Currency" NumberFormat-DecimalDigits="2" Culture="en-ZA" onkeyup="OnKeyUpTxtPremiumCalcLife()">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label55" runat="server" Text="EM Loading:"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtEMLoadingLife" Runat="server" NumberFormat-DecimalDigits="0" onkeyup="OnKeyUpTxtPremiumCalcLife()" MaxValue="400" MinValue="0" Enabled="False">                                                    
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>                             
                            </td>
                        </tr>                        
                         <tr>
                            <td>   
                                <asp:Panel ID="PanelDisability" runat="server" GroupingText="Disability" BorderStyle="Solid" BorderWidth="1px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label30" runat="server" Text="Type of benifit:"></asp:Label>
                                            </td>
                                            <td>
                                                
                                                <telerik:RadComboBox ID="RadComboBoxTypeBenefitDisability" Runat="server" onclientselectedindexchanged="CheckQualifyButton"></telerik:RadComboBox>
                                                
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                <asp:Label ID="Label35" runat="server" Text="Type of disability:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButtonTypeOfDisADW" runat="server" GroupName="TypeOfDis" Text="ADW" Font-Size="Smaller" AutoPostBack="true" OnCheckedChanged="RadioButtonTypeOfDisADW_CheckedChanged"/>
                                                <asp:RadioButton ID="RadioButtonTypeOfDisOCC" runat="server" GroupName="TypeOfDis" Text="OCC" Font-Size="Smaller" AutoPostBack="true" OnCheckedChanged="RadioButtonTypeOfDisOCC_CheckedChanged"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label31" runat="server" Text="Escalation:" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButtonEscalationDis6" runat="server" GroupName="EscalationDis" Text="6%" Font-Size="Smaller" onclick="handleHbA1cClick(this)" Visible="False"/>
                                                <asp:RadioButton ID="RadioButtonEscalationDis10" runat="server" GroupName="EscalationDis" Text="10%" Font-Size="Smaller" onclick="handleHbA1cClick(this)" Visible="False"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label32" runat="server" Text="Quote:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButtonQuoteDisYes" runat="server" GroupName="QuoteDis" Text="Yes" Font-Size="Smaller" AutoPostBack="True"/>
                                                <asp:RadioButton ID="RadioButtonQuoteDisNo" runat="server" GroupName="QuoteDis" Text="No" Font-Size="Smaller" AutoPostBack="True"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label33" runat="server" Text="Cover amount:"></asp:Label>
                                            </td>
                                            <td>                                                
                                                 <telerik:RadNumericTextBox ID="RadNumericTxtCoverAmnDis" Runat="server" Type="Currency" NumberFormat-DecimalDigits="2" Culture="en-ZA" onkeyup="OnKeyUpTxtCoverCalcDis()">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label34" runat="server" Text="Premium:"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtPremiumDis" Runat="server" Type="Currency" NumberFormat-DecimalDigits="2" Culture="en-ZA" onkeyup="OnKeyUpTxtPremiumCalcDis()">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label56" runat="server" Text="EM Loading:"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtEMLoadingDisability" Runat="server" NumberFormat-DecimalDigits="0" onkeyup="OnKeyUpTxtPremiumCalcDis()" MaxValue="400" MinValue="0" Enabled="False">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>                             
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="PanelDesireContribution" runat="server" BorderStyle="Solid" BorderWidth="1px" GroupingText="Who is paying">
                                <table style="text-align: center">                                   
                                     <tr>                                            
                                        <td>                                            
                                            <asp:RadioButtonList ID="RadioButtonListWhoIsPaying" runat="server" Font-Size="Smaller" ClientIDMode="Static" CssClass="mylist" RepeatColumns="1">
                                                <asp:ListItem Selected="True">Life Insured is Paying</asp:ListItem>
                                                <asp:ListItem>Someone else is Paying</asp:ListItem>
                                            </asp:RadioButtonList>                                            
                                        </td>
                                      </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td rowspan="2" style="vertical-align: top; width: 100%;">
                   <asp:Panel ID="Panel7" runat="server" BorderStyle="Solid" BorderWidth="1px">
                       <table>
                           <tr>
                               <td>
                                    <table>
                                        <tr>
                                            <td style="text-align: center">

                                                <asp:Label ID="LblAcceptedQuote" runat="server" ForeColor="#0033CC">Accepted Quote</asp:Label>

                                            </td>
                                            <td style="text-align:center">
                                                <asp:Label ID="LblOffer" runat="server" ForeColor="#0033CC"></asp:Label>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <telerik:RadButton ID="RadBtnOption1" runat="server" Text="Option1" OnClick="RadBtnOption1_Click" Visible="False"></telerik:RadButton>
                                            </td>
                                            <td rowspan="2">                                                
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption1RandValue" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium" >
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp1_1" runat="server" Text=" rands of LIFE COVER at a premium of" Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption1Premium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Size="Medium" Font-Bold="True">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp1_2" runat="server" Text=" rands per month AND " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption1DisCover" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp1_3" runat="server" Text=" rands of DISABILITY COVER at a premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption1DisPremium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp1_4" runat="server" Text=" rands per month. So thats a total monthly premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption1Total" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp1_5" runat="server" Text=" rands per month." Visible="False"></asp:Label>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:RadioButton ID="RadioButtonOption1" Visible="false" GroupName="Options" runat="server" OnCheckedChanged="RadioButtonOption1_CheckedChanged" AutoPostBack="True" />
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td style="text-align: center">
                                                <asp:Label ID="lblOr" runat="server" Text="OR" ForeColor="#0033CC"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <telerik:RadButton ID="RadBtnOption2" runat="server" Text="Option2" OnClick="RadBtnOption2_Click" Visible="False">
                                                </telerik:RadButton>
                                            </td>
                                            <td rowspan="2">                                               
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption2RandValue" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp2_1" runat="server" Text=" rands of LIFE COVER at a premium of" Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption2Premium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp2_2" runat="server" Text=" rands per month AND " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption2DisCover" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp2_3" runat="server" Text=" rands of DISABILITY COVER at a premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption2DisPremium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp2_4" runat="server" Text=" rands per month. So thats a total monthly premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption2Total" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp2_5" runat="server" Text=" rands per month." Visible="False"></asp:Label> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:RadioButton ID="RadioButtonOption2" Visible="false" GroupName="Options" runat="server" OnCheckedChanged="RadioButtonOption2_CheckedChanged" AutoPostBack="True" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                &nbsp;</td>
                                            <td>

                                                <asp:Label ID="lblSuitable0" runat="server" ForeColor="#0033CC" Text="Which option is more suitable for you?" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblAlso" runat="server" Visible="False" Text="(If none), We can also offer you" ForeColor="#0033CC"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <telerik:RadButton ID="RadBtnOption3" Visible="False" runat="server" Text="Option3" OnClick="RadBtnOption3_Click">
                                                </telerik:RadButton>
                                            </td>
                                            <td rowspan="2">
                                               <telerik:RadNumericTextBox ID="RadNumericTxtOption3RandValue" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp3_1" runat="server" Text=" rands of LIFE COVER at a premium of" Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption3Premium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp3_2" runat="server" Text=" rands per month AND " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption3DisCover" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp3_3" runat="server" Text=" rands of DISABILITY COVER at a premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption3DisPremium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp3_4" runat="server" Text=" rands per month. So thats a total monthly premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption3Total" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp3_5" runat="server" Text=" rands per month." Visible="False"></asp:Label>                                                 
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:RadioButton ID="RadioButtonOption3" GroupName="Options" runat="server" OnCheckedChanged="RadioButtonOption3_CheckedChanged" AutoPostBack="True" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>                                            
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblSuitable" runat="server" ForeColor="#0033CC" Text="Is this suitable for you?" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>                                            
                                        </tr>
                                        <tr>
                                            <td>&nbsp; </td>
                                            <td>
                                                <asp:Label ID="lblSpecificCover" runat="server" ForeColor="#0033CC" Text="(If specific cover amount), We can offer you" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <telerik:RadButton ID="RadBtnOption4" Visible="false" runat="server" Text="Option4" OnClick="RadBtnOption4_Click">
                                                </telerik:RadButton>
                                            </td>
                                            <td rowspan="2">                                                 
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption4RandValue" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp4_1" runat="server" Text=" rands of LIFE COVER at a premium of" Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption4Premium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp4_2" runat="server" Text=" rands per month AND " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption4DisCover" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp4_3" runat="server" Text=" rands of DISABILITY COVER at a premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption4DisPremium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp4_4" runat="server" Text=" rands per month. So thats a total monthly premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption4Total" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp4_5" runat="server" Text=" rands per month." Visible="False"></asp:Label>
                                                <asp:Label ID="lblOp4_Problem" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:RadioButton ID="RadioButtonOption4" GroupName="Options" runat="server" OnCheckedChanged="RadioButtonOption4_CheckedChanged" AutoPostBack="True" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td>&nbsp; </td>
                                            <td>&nbsp; </td>
                                        </tr>
                                        <tr>
                                            <td>

                                            </td>
                                            <td>
                                                <asp:Label ID="lblSpecificPremium" runat="server" Visible="false" Text="(If specific premium), We can offer you" ForeColor="#0033CC"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <telerik:RadButton ID="RadBtnOption5" runat="server" Text="Option5" OnClick="RadBtnOption5_Click"></telerik:RadButton>
                                            </td>
                                            <td rowspan="2">
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption5RandValue" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp5_1" runat="server" Text=" rands of LIFE COVER at a premium of" Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption5Premium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp5_2" runat="server" Text=" rands per month AND " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption5DisCover" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp5_3" runat="server" Text=" rands of DISABILITY COVER at a premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption5DisPremium" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp5_4" runat="server" Text=" rands per month. So thats a total monthly premium of " Visible="False"></asp:Label>
                                                <telerik:RadNumericTextBox ID="RadNumericTxtOption5Total" Runat="server" Visible="False" Type="Currency" NumberFormat-DecimalDigits="2" Width="120px" ReadOnly="True" Culture="en-ZA" Font-Bold="True" Font-Size="Medium">
                                                    <NegativeStyle Resize="None" />
                                                    <NumberFormat ZeroPattern="R n" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblOp5_5" runat="server" Text=" rands per month." Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:RadioButton ID="RadioButtonOption5" Visible="false" GroupName="Options" runat="server" OnCheckedChanged="RadioButtonOption5_CheckedChanged" AutoPostBack="True" />
                                            </td>
                                        </tr>
                                    </table>
                               </td>
                           </tr>
                       </table>
                       
                   </asp:Panel> 
                </td>
             </tr>
         </table> 
        <table width=100% border=0>
            <tr>
                <td width="100%">                                    
                    <table border=0 align=center>
                        <tr>
                            <td>
                           <telerik:RadGrid ID="RadGridSummary" runat="server"></telerik:RadGrid>
                            </td>
                        </tr>                       
                    </table>
                </td>
             </tr>
         </table>
        <table width=100% border=0>
            <tr>
                <td width="100%">                                    
                    <table border=0 align=center>
                        <tr>
                            <td>                                
                                <telerik:RadButton ID="RadButtonGenerateLetter" runat="server" Text="Generate quote letter" OnClick="RadButtonGenerateLetter_Click"></telerik:RadButton>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                            </td>
                            <td>
                                <telerik:RadButton ID="RadButtonSendLetter" runat="server" Text="Send quote letter" OnClick="RadButtonSendLetter_Click" Visible="false"></telerik:RadButton>
                            </td>
                        </tr>                       
                    </table>
                </td>
             </tr>
         </table>
    </asp:Panel>
    </div>
     <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                </telerik:RadWindowManager>
    <asp:HiddenField ID="HiddenFieldClassOfLife" runat="server" />    
    <asp:HiddenField ID="HiddenFieldRiskModifier" runat="server" />
    <asp:HiddenField ID="HiddenFieldRMDiabetesType" runat="server" />
    <asp:HiddenField ID="HiddenFieldRBDOB" runat="server" />            
    <asp:HiddenField ID="HiddenFieldQualifyClient" runat="server" />
    <asp:HiddenField ID="HiddenFieldEscalationLife" runat="server" />
    <asp:HiddenField ID="HiddenFieldEscalationDisablility" runat="server" />
    <asp:HiddenField ID="HiddenFieldRadComboBoxTypeBenefitLife" runat="server" />
    <asp:HiddenField ID="HiddenFieldRadComboBoxTypeBenefitDisability" runat="server" />
    <asp:HiddenField ID="HiddenFieldOccupation" runat="server" />
    <telerik:RadNotification ID="RadNotification1" runat="server" AutoCloseDelay="6000">
    </telerik:RadNotification>
    </asp:Content>
