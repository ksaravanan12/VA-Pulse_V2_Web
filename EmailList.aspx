<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="EmailList.aspx.vb" Inherits="GMSUI.EmailList" Title="Email List" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
	
    <script language="javascript" type="text/javascript" src="Javascript/Dialog.js"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=3"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupEmail.js?d=2"></script>
    <script language="javascript" type="text/javascript">
       
        var g_UserRole = "";
        var curSiteid = "";
        var curEmailId = "";
        var isButtonClicked = 0;
        var isalertsChanged = 0;
        var g_UserId = 0;
		
        var GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
		
        this.onload = function () {
            LoadEmailData(1);
			
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
        }
		
        function Redirect(sid) {
           
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
			
            location.href = "Home.aspx?sid=" + GSiteId;
        }

        window.onhashchange = function () {
		
            // Load Glossary
            if (location.hash == "#divSiteOverview1") {
                showGlossaryInfo("SiteOverview");
            }
            else if (location.hash == "#divPatientTag") {
                showGlossaryInfo("DeviceList");
            }
            if (isButtonClicked == 1) {
                isButtonClicked = 0;
                return;
            }
            if (location.hash == "#divAlertConfig" || location.hash == "#" || location.hash == "") {
                DisplayEmailListForBack(0);
            }
        }

        function DisplayEmailListForBack(isClick) {
		
            curSiteid = "";
            curEmailId = "";
            isButtonClicked = isClick;

            if (isalertsChanged == 1) {
                isalertsChanged = 0;
                var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
                LoadEmailData(currentpage);
            }

            document.getElementById("rdNone").checked = false;
            document.getElementById("rdDaily").checked = false;
            document.getElementById("rdWeekly").checked = false;
            document.getElementById("rdLowNone").checked = false;
            document.getElementById("rdLowDaily").checked = false;
            document.getElementById("rdLowWeekly").checked = false;

            document.getElementById("rdPatientTagOfflineNone").checked = false;
            document.getElementById("rdPatientTagOfflineDaily").checked = false;
            document.getElementById("rdPatientTagOfflineWeekly").checked = false;

            document.getElementById("rdUnderLowNone").checked = false;
            document.getElementById("rdUnderLowDaily").checked = false;
            document.getElementById("rdUnderLowWeekly").checked = false;

            document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated").innerHTML = "";
            $('#divEditAlertConfig').hide('slide', { direction: 'right' }, 200);
            $('#divEmailList').show('slide', { direction: 'left' }, 400);
        }

        function ShowAddEmail() {
            clearCtrlData();
            $('#tdAddEmail').show('slide', { direction: 'left' }, 500);
        }

        function HideAddEmail() {
            clearCtrlData();
            $('#tdAddEmail').hide('slide', { direction: 'left' }, 500);
        }

        function LoadEmailData(currpage) {
            var siteid;
            siteid = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            document.getElementById("divLoading").style.display = "";
            Load_Setup_Email(siteid, currpage);
        }

        function CallEmailSetup(IsAdd) {

            var siteid, email, alerttype, status, nStatus, Email, sbeforestaus;

            try {
                if (IsAdd == "1") {
                    siteid = document.getElementById("<%=drpSites.ClientID%>").value;
                    email = document.getElementById("<%=txtEmail.ClientID%>").value;
                    alerttype = document.getElementById("<%=ddlAlertType.ClientID%>").value;
                    status = document.getElementById("<%=chkStatus.ClientID%>").checked;

                    PageVisitDetails(g_UserId, "Email List", enumPageAction.Add, "New EmailId Added - EmailId : " + email + " - " + $("#ctl00_ContentPlaceHolder1_drpSites option:selected").text());
                }
                else {
                    siteid = curSiteid;
                    email = curEmailId;
                    alerttype = document.getElementById("<%=ddlAlertTypeUpdate.ClientID%>").value;
                    status = document.getElementById("<%=chkStatusUpdate.ClientID%>").checked;

                    if (alerttype == 0)
                        changedAlertType = "Continuous Alert";
                    else if (alerttype == 1)
                        changedAlertType = "Single Alert";

                    if (changedAlertType == "Continuous Alert")
                        beforeAlertType = "Single Alert";
                    else if (changedAlertType == "Single Alert")
                        beforeAlertType = "Continuous Alert";

                    PageVisitDetails(g_UserId, "Email List", enumPageAction.Edit, "EmailId - " + email + ", Alert Type : " + beforeAlertType + " to " + changedAlertType + " - " + $("#tdcurSiteName").text());

                    if (beforestaus == 1)
                        sbeforestaus = "Active";
                    else if (beforestaus == 0)
                        sbeforestaus = "In Active";

                    if (status == 1)
                        changedstaus = "Active";
                    else if (status == 0)
                        changedstaus = "In Active";

                    if (beforestaus != status)
                        PageVisitDetails(g_UserId, "Email List", enumPageAction.Edit, "EmailId - " + email + ", Status : " + sbeforestaus + " to " + changedstaus + " - " + $("#tdcurSiteName").text());
                }
            }
            catch (e) {

            }

            EmailDataId = document.getElementById("<%=hdnEmailDataId.ClientID%>").value;

            if (EmailDataId == "")
                IsAdd = "1";
            else
                IsAdd = "2";

            if (IsAdd == "1") {
                if (siteid == 0) {
                    alert("Please select site");
                    return false;
                }

                if (email == "") {
                    alert("Email should not be empty");
                    email.focus();
                    return false;
                }

                if (checkEmail(email) == false) {
                    alert("Invalid E-mail Address. Please re-enter.");
                    email.focus();
                    return false;
                }
            }

            document.getElementById("divUpdate").style.display = "";

            if (status == true)
                nStatus = 1;
            else
                nStatus = 0;

            AddEmailSetup(siteid, email, alerttype, nStatus, IsAdd, EmailDataId);
            isalertsChanged = 1;
        }

        function doNext() {
            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            var cnt = Number(currentpage) + 1;
            document.getElementById("<%=txtPageNo.ClientID%>").value = cnt;

            LoadEmailData(cnt);
        }

        function doPrev() {
            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            var cnt = currentpage - 1;
            document.getElementById("<%=txtPageNo.ClientID%>").value = cnt;

            LoadEmailData(cnt);
        }

        function gotoPage() {
            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;

            LoadEmailData(currentpage);
        }

        function doAfterAddedEmail() {
            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            LoadEmailData(currentpage)
            HideAddEmail();

            document.getElementById("divUpdate").style.display = "none";
            window.location = "EmailList.aspx";
        }

        function clearCtrlData() {
            document.getElementById("<%=drpSites.ClientID%>").selectedIndex = "0";
            document.getElementById("<%=txtEmail.ClientID%>").value = "";
            document.getElementById("<%=hdnEmailDataId.ClientID%>").value = "";
        }

        function showTip3(text, lf, tp) {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip3").css({ left: tempX, top: tempY })
        }
        function hideTip3() {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.display = 'none';
        }

        function deleteConfiguration(siteId, emailId, currpage) {
            if (confirm("Are you sure do you want to delete this settings?") == true) {

                document.getElementById("<%=hid_siteid.ClientID%>").value = siteId;
                document.getElementById("<%=hid_emailid.ClientID%>").value = emailId;
                document.getElementById('txtUserPassword').value = "";

                var conpwd = document.getElementById("<%=hid_pwd.ClientID%>").value;

                if (conpwd != "") {
                    $("#Password_dialog").dialog({
                        height: 200,
                        width: 450,
                        modal: true,
                        show: {
                            effect: "fade",
                            duration: 500
                        },
                        hide: {
                            effect: "fade",
                            duration: 500
                        },
                        close: function (event) {
                        }
                    });
                }
                else {
                    conformDelete();
                }
            }
        }

        function conformDelete() {

            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>").value;
            var siteId = document.getElementById("<%=hid_siteid.ClientID%>").value;
            var emailId = document.getElementById("<%=hid_emailid.ClientID%>").value;
            var password = document.getElementById('txtUserPassword').value;
            var currpage = "";

            if (password == conpwd || conpwd == "") {

                DeleteConfigurationSetting(siteId, emailId, currpage);
            }
            else if (password == "") {
                alert("Password should not be empty!");
            }
            else {
                alert("Incorrect Password");
                document.getElementById('txtUserPassword').value = "";
                //document.getElementById("txtUserPassword").focus();
            }

        }

        function showEmailConfigurationForEmail(siteId, sitename, emailId, isClick, EmailDataId, type, status) {
            isButtonClicked = isClick;
            curSiteid = siteId;
            curEmailId = emailId;
            sitename = decodeURIComponent(sitename);  
            document.getElementById("tdcurSiteName").innerHTML = sitename;
            document.getElementById("tdcurEmail").innerHTML = emailId;

            if (type == 1) {
                document.getElementById("<%=ddlAlertTypeUpdate.ClientID%>").selectedIndex = 1;
            }
            else
                document.getElementById("<%=ddlAlertTypeUpdate.ClientID%>").selectedIndex = 0;

            if (status == 1) {
                document.getElementById("<%=chkStatusUpdate.ClientID%>").checked = true;
            }
            else
                document.getElementById("<%=chkStatusUpdate.ClientID%>").checked = false;

            document.getElementById("ctl00_ContentPlaceHolder1_hdnEmailDataId").value = EmailDataId;

            $('#divEmailList').hide('slide', { direction: 'left' }, 150);
            if (isClick == "1")
                location.href = "#divAlertConfig";
            $('#divEditAlertConfig').show('slide', { direction: 'right' }, 500);

            GetAvailableAlertsForEmail(siteId, emailId);
        }

        function AddRemoveAlertForEmail(isAdd) {
            var e;
            var AlertId;
            var mode = 0;

            if (isAdd == "0") {
                e = document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated");
                if (e.selectedIndex == -1) {
                    alert("Please select an alert from associated alerts");
                    return;
                }
                AlertId = e.options[e.selectedIndex].value;
            }
            else if (isAdd == "1") {
                e = document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable");
                if (e.selectedIndex == -1) {
                    alert("Please select an alert from available alerts");
                    return;
                }
                AlertId = e.options[e.selectedIndex].value;
            }
            else if (isAdd == "2") {
                var selectedValueToDo = "";
                var selectedLowBattery = "";
                var selectedPatientTagOffline = "";
                var selectedUnderWatchAndLowBattery = "";

                var radios = document.getElementsByName("todo");
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].checked) selectedValueToDo = radios[i].value;
                }

                radios = document.getElementsByName("LowBattery");
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].checked) selectedLowBattery = radios[i].value;
                }

                radios = document.getElementsByName("PatientTagOffline");
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].checked) selectedPatientTagOffline = radios[i].value;
                }

                radios = document.getElementsByName("UnderWatchAndLowBattery");
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].checked) selectedUnderWatchAndLowBattery = radios[i].value;
                }

                mode = selectedValueToDo + "-" + selectedLowBattery + "-" + selectedPatientTagOffline + "-" + selectedUnderWatchAndLowBattery;
            }

            isalertsChanged = 1;
            EditAlertsForEmail(curSiteid, curEmailId, AlertId, isAdd, mode);

            var AvaiAlertDes = $("#ctl00_ContentPlaceHolder1_lstAvailable>option:selected").text();
            var AssoAlertDes = $("#ctl00_ContentPlaceHolder1_lstAssociated>option:selected").text();
            var Report = "";

            if (selectedValueToDo == "1") {
                Report = "To Do Daily"
            }
            else if (selectedValueToDo == "2") {
                Report = "To Do Weekly"
            }
            else if (selectedValueToDo == "0") {
                Report = "To Do None"
            }

            if (setundefined(Report) != "")
                Report += ","

            if (selectedLowBattery == "1") {
                Report += "Low Battery List Daily"
            }
            else if (selectedLowBattery == "2") {
                Report += "Low Battery List Weekly"
            }
            else if (selectedLowBattery == "0") {
                Report += "Low Battery List None"
            }

            if (setundefined(Report) != "")
                Report += ","

            if (selectedPatientTagOffline == "1") {
                Report += "Underwatch-Low Battery List Daily"
            }
            else if (selectedPatientTagOffline == "2") {
                Report += "Underwatch-Low Battery List Weekly"
            }
            else if (selectedPatientTagOffline == "0") {
                Report += "Underwatch-Low Battery List None"
            }

            if (setundefined(Report) != "")
                Report += ","

            if (selectedUnderWatchAndLowBattery == "1") {
                Report += "Patient Tag Offline Daily"
            }
            else if (selectedUnderWatchAndLowBattery == "2") {
                Report += "Patient Tag Offline Weekly"
            }
            else if (selectedUnderWatchAndLowBattery == "0") {
                Report += "Patient Tag Offline None"
            }

            try {
                if (isAdd == "1")
                    PageVisitDetails(g_UserId, "Email List", enumPageAction.Edit, "EmailId - " + curEmailId + ", Associated Alert : " + AvaiAlertDes + " - " + $("#tdcurSiteName").text());
                else if (isAdd == "0")
                    PageVisitDetails(g_UserId, "Email List", enumPageAction.Edit, "EmailId - " + curEmailId + ", Removed Alert : " + AssoAlertDes + " - " + $("#tdcurSiteName").text());
                else if (isAdd == "2")
                    PageVisitDetails(g_UserId, "Email List", enumPageAction.Edit, "Id - " + curEmailId + ", Report : " + Report + " - " + $("#tdcurSiteName").text());
            }
            catch (e) {

            }
        }
        
    </script>
    <div id="tooltip3" class="tool3" style="display: none;">
    </div>
    <input type="hidden" id="hid_pwd" runat="server" />
    <input type="hidden" id="hid_emailid" runat="server" />
    <input type="hidden" id="hid_siteid" runat="server" />
    <!-- EMAIL LIST -->
    <div id="divEmailList" style="top: auto; left: auto; height: 850px;">
        <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="tblHeader"
                        runat="server">
                        <tr style="height: 10px;">
                            <td>
                                <input type="hidden" id="hid_userrole" runat="server" />
                                <input type="hidden" id="hid_userid" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="3">
                                            <table width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='Redirect();'>
                                                        <a href="Home.aspx">
                                                            <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    Setup Email
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>
                                                                    Associate email to site
                                                                </td>
                                                                <td colspan="2" align="left" id="lblMessage" style="color: Green;" class="clsFilterCriteria">
                                                                </td>
                                                                <td align="right">
                                                                    <input type="button" id="btnAdd" runat="server" value="Add" class="clsButton" onclick="ShowAddEmail();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top" colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="tdAddEmail" style="display: none;">
                                                <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                                                    <tr>
                                                        <td align="left">
                                                            Site :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="drpSites" runat="server" Width="450px">
                                                            </asp:dropdownlist>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Email :
                                                        </td>
                                                        <td align="left">
                                                            <asp:textbox id="txtEmail" runat="server" width="260px" maxlength="250">
                                                            </asp:textbox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Alert Type :
                                                        </td>
                                                        <td align="left">
                                                            <asp:dropdownlist width="260px" id="ddlAlertType" runat="server">
                                                            </asp:dropdownlist>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            Status :
                                                        </td>
                                                        <td align="left">
                                                            <input type="checkbox" id="chkStatus" name="chkStatus" checked="checked" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <input type="button" id="btnSave" value="Save" class="clsButton" onclick="CallEmailSetup(1);" />
                                                            &nbsp;
                                                            <input type="button" id="btnCancel" value="Cancel" class="clsButton" onclick="HideAddEmail();" />
                                                            <input type="hidden" name="hdnEmailDataId" id="hdnEmailDataId" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 10px;">
                                                    </tr>
                                                    <tr style="height: 5px; width: 100%;">
                                                        <td class="bordertop" valign="top" colspan="3">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td class="txttotalpage">
                                                        <asp:label id="lbltotalcount" runat="server"></asp:label>
                                                    </td>
                                                    <td class="clsTableTitleText" align="right">
                                                        <input type="button" id="btnPrev" class="clsPrev" onclick="doPrev();" title="Previous" />
                                                        <asp:label id="lblPage" runat="server" cssclass="clsCntrlTxt"> Page </asp:label><input
                                                            id="txtPageNo" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                            type="text" size="1" maxlength="4" runat="server" name="txtPageNo" value="1"><asp:label
                                                                id="lblTotalpage" runat="server" cssclass="clsCntrlTxt">&nbsp;</asp:label>&nbsp;<input
                                                                    type="button" id="btnGo" class="btnGO" value="Go" onclick="gotoPage();" />&nbsp;&nbsp;<input
                                                                        type="button" id="btnNext" class="clsNext" onclick="doNext();" title="Next" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table border="0" cellspacing="0" cellpadding="5" id="tblEmailListInfo" style="width: 650px;
                        border: solid 0px #BAB9B9;">
                    </table>
                </td>
            </tr>
            <tr style="height: 20px;">
            </tr>
        </table>
    </div>
    <!-- EDIT ALERT CONFIGURATION -->
    <div id="divEditAlertConfig" style="display: none; top: auto; left: auto; height: 850px;">
        <table width="85%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0"
            style="padding-left: 40px;">
            <tr style="height: 10px;">
            </tr>
            <tr>
                <td style="width: 100%" colspan="3">
                    <table width="100%" border="0">
                        <tr>
                            <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='DisplayEmailListForBack(1);'>
                                <a href="#">
                                    <img src='images/Left-Arrow.png' title='back' border="0" /></a>
                            </td>
                            <td style='width: 15px;' valign="top">
                            </td>
                            <td align='left' valign="top" style="width: 90%;">
                                <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                    <tr>
                                        <td align='left' class='SHeader1' id="tdcurSiteName">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left' class='subHeader_black' id="tdcurEmail">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
            </tr>
            <tr style="height: 5px;">
                <td class="bordertop" valign="top" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr style="height: 10px;">
            </tr>
            <tr>
                <td class="clsCntrlTxt">
                    Alert Type
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table border="0" cellpadding="6" cellspacing="0" width="100%" class="emailBorderStyle">
                        <tr>
                            <td align="left" style="width: 60%;">
                                Alert Type&nbsp;:&nbsp;
                                <asp:dropdownlist width="260px" id="ddlAlertTypeUpdate" runat="server">
                                </asp:dropdownlist>
                            </td>
                            <td align="left" style="width: 20%;">
                                Status&nbsp;:&nbsp;
                                <input type="checkbox" id="chkStatusUpdate" name="chkStatusUpdate" checked="checked"
                                    runat="server" />
                            </td>
                            <td align="right" style="width: 20%;">
                                <input type="button" id="btnSaveUpdate" value="Update" class="clsButton" onclick="CallEmailSetup(2);" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 20px;">
            </tr>
            <tr>
                <td class="clsCntrlTxt">
                    Available Alerts
                </td>
                <td>
                </td>
                <td class="clsCntrlTxt">
                    Associated Alerts
                </td>
            </tr>
            <tr>
                <td>
                    <asp:listbox id="lstAvailable" style="height: 300px; width: 270px;" runat="server"></asp:listbox>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <input type="button" class="clsButton" id="btnAddTOAsso" value="Add" onclick="AddRemoveAlertForEmail(1);" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" class="clsButton" id="btnRemove" value="Remove" onclick="AddRemoveAlertForEmail(0);" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:listbox id="lstAssociated" style="height: 300px; width: 270px;" runat="server">
                    </asp:listbox>
                </td>
            </tr>
            <tr>
                <td style="height: 15px;">
                </td>
            </tr>
            <tr style="display: none;">
                <td class="clsCntrlTxt">
                    Email Reports
                </td>
            </tr>
            <tr style="display: none;">
                <td colspan="3">
                    <table border="0" class="emailBorderStyle" cellpadding="3">
                        <tr>
                            <td align="left" style="width: 150px;">
                                To Do :
                            </td>
                            <td style="width: 30px;">
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="todo" id="rdNone" value="0" />None
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="todo" id="rdDaily" value="1" />Daily
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="todo" id="rdWeekly" value="2" />Weekly
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px;">
                                Low Battery List :
                            </td>
                            <td style="width: 30px;">
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="LowBattery" id="rdLowNone" value="0" />None
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="LowBattery" id="rdLowDaily" value="1" />Daily
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="LowBattery" id="rdLowWeekly" value="2" />Weekly
                            </td>
                            <td style="width: 100px;">
                            </td>
                        </tr>
						<tr>
                            <td align="left" style="width: 200px;">
                                Underwatch & Low Battery List :
                            </td>
                            <td style="width: 30px;">
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="UnderWatchAndLowBattery" id="rdUnderLowNone" value="0" />None
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="UnderWatchAndLowBattery" id="rdUnderLowDaily" value="1" />Daily
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="UnderWatchAndLowBattery" id="rdUnderLowWeekly" value="2" />Weekly
                            </td>
                            <td style="width: 100px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px;">
                                Patient Tag Offline :
                            </td>
                            <td style="width: 30px;">
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="PatientTagOffline" id="rdPatientTagOfflineNone" value="0" />None
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="PatientTagOffline" id="rdPatientTagOfflineDaily" value="1" />Daily
                            </td>
                            <td style="width: 100px;">
                                <input type="radio" name="PatientTagOffline" id="rdPatientTagOfflineWeekly" value="2" />Weekly
                            </td>
                            <td align="right" rowspan="3">
                                <input type="button" id="btnUpdate" class="clsButton" value="Update" onclick="AddRemoveAlertForEmail(2)" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px;">
                </td>
            </tr>
        </table>
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <script type="text/javascript">
        LoadGlossaryInfo("Email", g_UserRole);    
    </script>
    <div id="Password_dialog" title="Confirm Password" style="display: none;">
        <table cellpadding="5">
            <tr>
                <td>
                    Enter Password
                </td>
                <td>
                    <input class="clsLAText" type="password" id="txtUserPassword" name="txtPassword"
                        style="width: 200px;" />
                </td>
            </tr>
            <tr style="height: 20px;">
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;
                    <input type="button" class="clsButton" id="btnConform" value="Ok" onclick="conformDelete();" />
                </td>
            </tr>
        </table>
    </div>
</asp:content>
