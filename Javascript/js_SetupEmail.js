function CreateInfraXMLObj() {

    var infraobj = null;

    if (window.ActiveXObject) {

        try {
            infraobj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                infraobj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                infraobj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {

        infraobj = new XMLHttpRequest();
        infraobj.overrideMimeType('text/xml');
    }

    return infraobj;
}

var inf_Obj;
var g_IsAdd;

function AddEmailSetup(SiteId,EmailId,AlertType,Status,IsAdd,EmailDataId)
{    
    g_IsAdd = IsAdd;    

    $.post("AjaxConnector.aspx?cmd=AddEmailSetup",
    {
        sid: SiteId,
        EmailId: EmailId,
        AlertType: AlertType,
        Status: Status,
        IsAdd: IsAdd,
        EmailDataId: EmailDataId
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddEmail(dsRoot);
        }
        else {
            document.getElementById("divUpdate").style.display = "none";
        }
    });
}

function ajaxAddEmail(dsRoot) {

    var o_Msg = dsRoot.getElementsByTagName('Msg');
    var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);

    if (msg == "" || msg == null)
        msg = "Email has been added.";

    if (g_IsAdd == "1") {
        document.getElementById("lblMessage").innerHTML = msg;
        doAfterAddedEmail();
    }
    else {
        document.getElementById("divUpdate").style.display = "none";
        var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;
        LoadEmailData(currentpage);
    }
}

function Load_Setup_Email(siteid, curpag) {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {
    
        inf_Obj.onreadystatechange = ajaxEmailList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=EmailList&sid=" + siteid + "&curpage=" + curpag;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    
    return false;
}

var beforeAlertType = "";
var changedAlertType = "";

var beforestaus = 0;
var changedstaus = "";

function ajaxEmailList() {

    if (inf_Obj.readyState == 4) {
    
        if (inf_Obj.status == 200) {
	
            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblEmailListInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblEmailListInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblEmailListInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);
            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_siteId = dsRoot.getElementsByTagName('SiteId')
            var o_SiteName = dsRoot.getElementsByTagName('SiteName')
            var o_TotalPage = dsRoot.getElementsByTagName('TotalPages')
            var o_TotalCount = dsRoot.getElementsByTagName('TotalCount')
            var o_Email = dsRoot.getElementsByTagName('Email')
            var o_AlertType = dsRoot.getElementsByTagName('AlertType')
            var o_Status = dsRoot.getElementsByTagName('Status')
            var o_EmailDataId = dsRoot.getElementsByTagName('DataId')
            var o_Alerts = dsRoot.getElementsByTagName('Alerts')
            var o_bDelete = dsRoot.getElementsByTagName('bDelete')

            nRootLength = o_siteId.length;

            if (nRootLength > 0) {
                row = document.createElement('tr');

                if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support
                    || g_UserRole == enumUserRoleArr.Maintenance || g_UserRole == enumUserRoleArr.MaintenancePrism || g_UserRole == enumUserRoleArr.TechnicalAdmin) {

                    AddCell(row, "SiteName", 'siteOverview_TopLeft_Box', "", "", "left", "275px", "30px", "");
                    AddCell(row, "Email", 'siteOverview_Box', "", "", "left", "275px", "30px", "");
                    AddCell(row, "Edit", 'siteOverview_Topright_Box', "", "", "left", "50px", "30px", "");
                }
                else {

                    AddCell(row, "SiteName", 'siteOverview_TopLeft_Box', "", "", "left", "250px", "30px", "");
                    AddCell(row, "Email", 'siteOverview_Topright_Box', "", "", "left", "150px", "30px", "");
                }

                sTbl.appendChild(row);

                //totalcount
                var ttcnt_lable = document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount")
                ttcnt_lable.innerHTML = "Total : " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

                //Totalpage
                var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")

                ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

                var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

                doEmailEnableButton(MaxPageCnt);

                for (var i = 0; i < nRootLength; i++) {

                    var siteid = (o_siteId[i].textContent || o_siteId[i].innerText || o_siteId[i].text);
                    var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);

                    var Email = (o_Email[i].textContent || o_Email[i].innerText || o_Email[i].text);
                    var AlertType = (o_AlertType[i].textContent || o_AlertType[i].innerText || o_AlertType[i].text);
                    var AlertStatus = (o_Status[i].textContent || o_Status[i].innerText || o_Status[i].text);
                    var EmailDataId = (o_EmailDataId[i].textContent || o_EmailDataId[i].innerText || o_EmailDataId[i].text);
                    var bDelete = (o_bDelete[i].textContent || o_bDelete[i].innerText || o_bDelete[i].text);

                    var alertT = 0;
                    var alertS = 0;

                    if (AlertType == "Single Alert")
                        alertT = 1;

                    if (AlertStatus == "Active")
                        alertS = 1;

                    beforeAlertType = AlertType;
                    beforestaus = alertS;
                    var Alerts = (o_Alerts[i].textContent || o_Alerts[i].innerText || o_Alerts[i].text);

                    row = document.createElement('tr');

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support
                       || g_UserRole == enumUserRoleArr.Maintenance || g_UserRole == enumUserRoleArr.MaintenancePrism || g_UserRole == enumUserRoleArr.TechnicalAdmin) {

                        if (AlertStatus == "Active")
                            sShowStatus = "";
                        else
                            sShowStatus = ",&nbsp;<font color='#ce0034' style='font-weight:normal;'>In-Active</font>";

                        sEmailRow = "<span><font>" + Email + "</font></span><br><font style='font-weight:normal;'>(" + AlertType + sShowStatus + ")</font>";

                        AddCell(row, SiteName, "general_leftBox_black", "", "", "left", "275px", "30px", "");
                        AddCell(row, sEmailRow, "general_left_cell_black", "", "", "left", "275px", "30px", "");

                        var Edit = "";
                        var Delete = "";

                        Edit = "<img id='editConfig-" + i + "' src='images/imgEdit.png' onclick=showEmailConfigurationForEmail(" + siteid + ",'" + SiteName.replace(new RegExp(" ", "gm"), "_") + "','" + Email + "','1'," + EmailDataId + ",'" + alertT + "','" + alertS + "'); onmouseover='btnPrev_LAServiceOver(this);' onmouseout='btnPrev_LAServiceOut(this);' />";

                        if (bDelete == "True")
                            Delete = "<img id='deleteConfig-" + i + "' src='images/imgDelete.png' onmouseover='btnPrev_LAServiceOver(this);' onmouseout='btnPrev_LAServiceOut(this);' onclick=deleteConfiguration(" + siteid + ",'" + Email + "'); />";

                        AddCell(row, Edit + "&nbsp;&nbsp;&nbsp;&nbsp;" + Delete, "general_left_cell_black", "", "", "center", "50px", "30px", "8px");
                    }
                    else {
                        AddCell(row, SiteName, "general_leftBox", "", "", "left", "250px", "30px", "");
                        AddCell(row, Email, "general_left_cell", "", "", "left", "150px", "30px", "");
                    }

                    sTbl.appendChild(row);

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support
                        || g_UserRole == enumUserRoleArr.Maintenance || g_UserRole == enumUserRoleArr.MaintenancePrism || g_UserRole == enumUserRoleArr.TechnicalAdmin) {

                        var assignedAlerts = setundefined(Alerts).split("|");

                        if (assignedAlerts.length > 0) {

                            for (var l = 0; l < assignedAlerts.length; l++) {

                                if (assignedAlerts[l] != "") {

                                    var alertsInfo = assignedAlerts[l].split("~");

                                    if (alertsInfo.length > 1) {

                                        row = document.createElement('tr');
                                        AddCell(row, alertsInfo[1], "general_leftBox", "3", "", "left", "250px", "30px", "25px");
                                        sTbl.appendChild(row);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        }
    }
}

function doEmailEnableButton(MaxPageCnt) {

    var curnPage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo");
    var curPage = curnPage.value;


    if (MaxPageCnt == "1" || Number(MaxPageCnt) == 1) {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1";
        document.getElementById("btnNext").style.display = "none";
        document.getElementById("btnPrev").style.display = "none";
    } else {
        document.getElementById("btnNext").style.display = "";
        document.getElementById("btnPrev").style.display = "";
    }

    if (Number(curPage) <= 1) {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1"
        document.getElementById("btnPrev").style.display = "none";
    }

    if (Number(curPage) >= Number(MaxPageCnt)) {
        document.getElementById("btnNext").style.display = "none";
    }
}

function DeleteConfigurationSetting(siteId, emailId, curpag) {

    emaildel = emailId;

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=DeleteEmailConfiguration",
    {
        sid: siteId,
        EmailId: emailId,
        curpage: curpag
    },
    function (data, status) {
        if (status == "success") {

            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            AfterDeleteAjax();
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function AfterDeleteAjax() {

    if (inf_Obj.readyState == 4) {

        if (inf_Obj.status == 200) {

            doAfterAddedEmail();

            try {
                PageVisitDetails(g_UserId, "Alert Settings", enumPageAction.Delete, "Email " + emaildel + " Deleted for Site - " + $("#ctl00_headerBanner_drpsitelist option:selected").text());
            }
            catch (e) {

            }
        }
    }
}

function GetAvailableAlertsForEmail(siteId, emailId) {

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=GetAvailableAlertsForEmail",
    {
        sid: siteId,
        EmailId: emailId
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxGetAvailableAlertsForEmailCallback(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function ajaxGetAvailableAlertsForEmailCallback(dsRoot) {

    var o_siteId = dsRoot.getElementsByTagName('SiteID');
    var o_SiteName = dsRoot.getElementsByTagName('Sitename');
    var o_AvailableAlerts = dsRoot.getElementsByTagName('AvailableAlerts');
    var o_AssociatedAlerts = dsRoot.getElementsByTagName('AssociatedAlerts');
    var o_ToDo = dsRoot.getElementsByTagName('ToDo');
    var o_LowBatteryList = dsRoot.getElementsByTagName('LowBatteryList');
    var o_PatientTagOfflineList = dsRoot.getElementsByTagName('PatientTagOfflineList');
    var o_UnderWatchAndLowBattery = dsRoot.getElementsByTagName('UnderWatchAndLowBattery');

    document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable").innerHTML = "";
    document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated").innerHTML = "";

    nRootLength = o_siteId.length;

    if (nRootLength > 0) {

        for (var i = 0; i < nRootLength; i++) {

            var AvailableAlerts = (o_AvailableAlerts[i].textContent || o_AvailableAlerts[i].innerText || o_AvailableAlerts[i].text);
            var AssociatedAlerts = (o_AssociatedAlerts[i].textContent || o_AssociatedAlerts[i].innerText || o_AssociatedAlerts[i].text);
            var ToDo = (o_ToDo[i].textContent || o_ToDo[i].innerText || o_ToDo[i].text);
            var LowBatteryList = (o_LowBatteryList[i].textContent || o_LowBatteryList[i].innerText || o_LowBatteryList[i].text);
            var PatientTagOfflineList = (o_PatientTagOfflineList[i].textContent || o_PatientTagOfflineList[i].innerText || o_PatientTagOfflineList[i].text);
            var UnderWatchAndLowBattery = (o_UnderWatchAndLowBattery[i].textContent || o_UnderWatchAndLowBattery[i].innerText || o_UnderWatchAndLowBattery[i].text);

            //ToDo List
            document.getElementById("rdNone").checked = false;
            document.getElementById("rdDaily").checked = false;
            document.getElementById("rdWeekly").checked = false;

            if (ToDo == "0")
                document.getElementById("rdNone").checked = true;
            else if (ToDo == "1")
                document.getElementById("rdDaily").checked = true;
            else if (ToDo == "2")
                document.getElementById("rdWeekly").checked = true;
            else
                document.getElementById("rdNone").checked = true;

            //Low Battery List
            document.getElementById("rdLowNone").checked = false;
            document.getElementById("rdLowDaily").checked = false;
            document.getElementById("rdLowWeekly").checked = false;

            if (LowBatteryList == "0")
                document.getElementById("rdLowNone").checked = true;
            else if (LowBatteryList == "1")
                document.getElementById("rdLowDaily").checked = true;
            else if (LowBatteryList == "2")
                document.getElementById("rdLowWeekly").checked = true;
            else
                document.getElementById("rdLowNone").checked = true;

            //Pateint Tag Offline Report  
            document.getElementById("rdPatientTagOfflineNone").checked = false;
            document.getElementById("rdPatientTagOfflineDaily").checked = false;
            document.getElementById("rdPatientTagOfflineWeekly").checked = false;

            if (PatientTagOfflineList == "0")
                document.getElementById("rdPatientTagOfflineNone").checked = true;
            else if (PatientTagOfflineList == "1")
                document.getElementById("rdPatientTagOfflineDaily").checked = true;
            else if (PatientTagOfflineList == "2")
                document.getElementById("rdPatientTagOfflineWeekly").checked = true;
            else
                document.getElementById("rdPatientTagOfflineNone").checked = true;

            //Underwatch & Low Battery List 
            document.getElementById("rdUnderLowNone").checked = false;
            document.getElementById("rdUnderLowDaily").checked = false;
            document.getElementById("rdUnderLowWeekly").checked = false;

            if (UnderWatchAndLowBattery == "0")
                document.getElementById("rdUnderLowNone").checked = true;
            else if (UnderWatchAndLowBattery == "1")
                document.getElementById("rdUnderLowDaily").checked = true;
            else if (UnderWatchAndLowBattery == "2")
                document.getElementById("rdUnderLowWeekly").checked = true;
            else
                document.getElementById("rdUnderLowNone").checked = true;

            //Available Alerts
            var availAlerts = setundefined(AvailableAlerts).split("|");
            if (availAlerts.length > 0) {
                for (var l = 0; l < availAlerts.length; l++) {
                    if (availAlerts[l] != "") {
                        var alertsInfo = availAlerts[l].split("~");
                        if (alertsInfo.length > 1) {
                            var id = alertsInfo[0];
                            var desc = alertsInfo[1];
                            var oLstField = document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable");
                            var oOption = document.createElement("OPTION");
                            oLstField.options.add(oOption);
                            oOption.text = desc;
                            oOption.value = id;
                        }
                    }
                }
            }

            //Associated Alerts
            var associatAlerts = setundefined(AssociatedAlerts).split("|");
            if (associatAlerts.length > 0) {
                for (var l = 0; l < associatAlerts.length; l++) {
                    if (associatAlerts[l] != "") {
                        var alertsInfo = associatAlerts[l].split("~");
                        if (alertsInfo.length > 1) {
                            var id = alertsInfo[0];
                            var desc = alertsInfo[1];
                            var oLstField = document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated");
                            var oOption = document.createElement("OPTION");
                            oLstField.options.add(oOption);
                            oOption.text = desc;
                            oOption.value = id;
                        }
                    }
                }
            }
        }
    }

    document.getElementById("divLoading").style.display = "none";
}

function EditAlertsForEmail(siteId, emailId, AlertId, isAdd, mode) {
    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=EditAlertsForEmail",
    {
        sid: siteId,
        EmailId: emailId,
        alertId: AlertId,
        isAdd: isAdd,
        mode: mode
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxGetAvailableAlertsForEmailCallback(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function EditEmailSetup(SiteId, EmailId, AlertType, AlertStatus, EmailDataId) {
    var nAlertType, bStatus;

    //Show Email Box
    ShowAddEmail();

    //Put Values
    if (AlertType == "Single_Alert")
        nAlertType = 1
    else
        nAlertType = 0

    if (AlertStatus == "Active")
        bStatus = true
    else
        bStatus = false

    document.getElementById("ctl00_ContentPlaceHolder1_drpSites").value = SiteId;
    document.getElementById("ctl00_ContentPlaceHolder1_txtEmail").value = EmailId;
    document.getElementById("ctl00_ContentPlaceHolder1_hdnEmailDataId").value = EmailDataId;
    document.getElementById("ctl00_ContentPlaceHolder1_ddlAlertType").value = nAlertType;
    document.getElementById("ctl00_ContentPlaceHolder1_chkStatus").checked = bStatus;
}

var inf_Obj;
var g_IsAdd;

function SetupAlertSiteList(InfraAlertid, InfraSite, LowBatteryAlert, LowBatteryReportid, Email) {

    document.getElementById("divLoading_AlertSettings").style.display = "";

    $.post("AjaxConnector.aspx?cmd=AddAlertSiteList",
    {
        qInfraAlertIds: InfraAlertid,
        qInfraSiteIds: InfraSite,
        qLowBatteryAlert: LowBatteryAlert,
        qLowBatteryReportid: LowBatteryReportid,
        qEmail: Email
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddSitealert(dsRoot);
        }
        else {
            document.getElementById("divLoading_AlertSettings").style.display = "none";
        }
    });
}

function ajaxAddSitealert(dsRoot) {

    try {
        var o_Result = dsRoot.getElementsByTagName('Result');
        var o_Error = dsRoot.getElementsByTagName('Error');

        var Result = (o_Result[0].textContent || o_Result[0].innerText || o_Result[0].text);
        var Error = (o_Error[0].textContent || o_Error[0].innerText || o_Error[0].text);

        if (Result == 0) {
            $('#lblMessage').html('Configured Sucessfully!.').addClass('clsMapSuccessTxt');
        }
        else {
            $('#lblMessage').html(Error).addClass('clsMapErrorTxt');
        }

        document.getElementById("divLoading_AlertSettings").style.display = "none";
    }
    catch (e) {
        window.location = "UserErrorPage.aspx";
    }
}

function GetScheduledEmailAlert(emailId) {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxScheduledEmailAlert;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetSchuledReports&EmailId=" + emailId;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    return false;
}

function ajaxScheduledEmailAlert() {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {
            var sTbl, sTblLen;
            var sTb2, sTb2Len;

            var arrAlertSiteId = [];
            var arrReportSiteId = [];

            var arrAlertIds = [];
            var arrReportIds = [];

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);
            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_AlertSiteId = dsRoot.getElementsByTagName('AlertSiteId');
            var o_ReportSiteId = dsRoot.getElementsByTagName('ReportSiteId');

            var o_AlertIds = dsRoot.getElementsByTagName('AlertIds');
            var o_ReportIds = dsRoot.getElementsByTagName('ReportIds');

            nRootLength = o_AlertSiteId.length;

            //sitename
            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {

                    var AlertSiteId = setundefined(o_AlertSiteId[i].textContent || o_AlertSiteId[i].innerText || o_AlertSiteId[i].text);
                    var ReportSiteId = setundefined(o_ReportSiteId[i].textContent || o_ReportSiteId[i].innerText || o_ReportSiteId[i].text);
                    var AlertIds = setundefined(o_AlertIds[i].textContent || o_AlertIds[i].innerText || o_AlertIds[i].text);
                    var ReportIds = setundefined(o_ReportIds[i].textContent || o_ReportIds[i].innerText || o_ReportIds[i].text);

                    if (AlertSiteId != "") {
                        arrAlertSiteId = String(AlertSiteId).split(",");

                        if (arrAlertSiteId.length > 0) {
                            for (var i = 0; i < arrAlertSiteId.length; i++) {
                                if (document.getElementById("chkAlert" + arrAlertSiteId[i]))
                                    document.getElementById("chkAlert" + arrAlertSiteId[i]).checked = true;
                            }
                        }
                    }

                    if (ReportSiteId != "") {
                        arrReportSiteId = String(ReportSiteId).split(",");

                        if (arrReportSiteId.length > 0) {
                            for (var i = 0; i < arrReportSiteId.length; i++) {
                                if (document.getElementById("chkReport" + arrReportSiteId[i]))
                                    document.getElementById("chkReport" + arrReportSiteId[i]).checked = true;
                            }
                        }
                    }

                    if (AlertIds != "") {
                        arrAlertIds = String(AlertIds).split(",");

                        if (arrAlertIds.length > 0) {
                            for (var i = 0; i < arrAlertIds.length; i++) {
                                if (document.getElementById("chkOfflineAlert_" + arrAlertIds[i]))
                                    document.getElementById("chkOfflineAlert_" + arrAlertIds[i]).checked = true;
                            }
                        }
                    }

                    if (ReportIds != "") {
                        arrReportIds = String(ReportIds).split(",");

                        if (arrReportIds.length > 0) {
                            for (var i = 0; i < arrReportIds.length; i++) {
                                if (document.getElementById("chkLowBattery_" + arrReportIds[i]))
                                    document.getElementById("chkLowBattery_" + arrReportIds[i]).checked = true;
                            }
                        }
                    }
                }
            }

        } // end if  g_Obj.status
    } // end if  g_Obj.readyState
}

function LoadSiteList() {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxGetAllSites;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetSiteList"; 

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    return false;
}

function ajaxGetAllSites() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var sTbl, sTblLen;
            var sTb2, sTb2Len;

            var AlertSiteList = "";
            var ReportSiteList = "";
            var sCnd = "";

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblAlertSiteList");
            var sTbl2 = document.getElementById("tblReportSiteList");

            sTblLen = sTbl.rows.length;
            sTbl2Len = sTbl2.rows.length;

            clearTableRows(sTbl, sTblLen);
            clearTableRows(sTbl2, sTbl2Len);

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_SiteId = dsRoot.getElementsByTagName('SiteId')
            var o_SiteName = dsRoot.getElementsByTagName('SiteName')

            nRootLength = o_SiteId.length;

            //sitename
            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {

                    var SiteId = setundefined((o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text));
                    var SiteName = setundefined((o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text));

                    var Alerts = "";
                    var Reports = "";

                    AlertSiteList += "<td align='left' height='30px' width='90px' colspan='6'><input type='checkbox' name='chkAlertSiteList'  id='chkAlert" + SiteId + "' value='" + SiteId + "'><label class='lblText' style='padding-right:30px;'>" + SiteName + "</label></td>";
                    ReportSiteList += "<td align='left' height='30px'  width='90px' colspan='6'><input type='checkbox' name='chkReportSiteList'  id='chkReport" + SiteId + "' value='" + SiteId + "'><label class='lblText' style='padding-right:30px;'>" + SiteName + "</label></td>";

                    Alerts += "<tr>" + AlertSiteList + "</tr>";
                    row = document.createElement('tr');
                    AddCell(row, Alerts, "", "6", "", "left", "", "30px", "");
                    sTbl.appendChild(row);
                    AlertSiteList = "";

                    Reports += "<tr>" + ReportSiteList + "</tr>";
                    row = document.createElement('tr');
                    AddCell(row, Reports, "", "6", "", "left", "", "30px", "");
                    sTbl2.appendChild(row);
                    ReportSiteList = "";
                }
            }

            //Get Alert List
            LoadAlertist();
        }
    }
}

function LoadAlertist() {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxGetAlertMaster;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetAlertMaster";

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    return false;
}

function ajaxGetAlertMaster() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var sTbl, sTblLen;
            var sTb2, sTb2Len;

            var AlertList = "";
            var ReportList = "";

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblAlerts");
            var sTbl2 = document.getElementById("tblReports");

            sTblLen = sTbl.rows.length;
            sTbl2Len = sTbl2.rows.length;

            clearTableRows(sTbl, sTblLen);
            clearTableRows(sTbl2, sTbl2Len);

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_AlertId = dsRoot.getElementsByTagName('AlertId')
            var o_AlertName = dsRoot.getElementsByTagName('AlertName')
            var o_AlertCategory = dsRoot.getElementsByTagName('AlertCategory')

            nRootLength = o_AlertId.length;

            //sitename
            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {

                    var AlertId = setundefined((o_AlertId[i].textContent || o_AlertId[i].innerText || o_AlertId[i].text));
                    var AlertName = setundefined((o_AlertName[i].textContent || o_AlertName[i].innerText || o_AlertName[i].text));
                    var AlertCategory = setundefined((o_AlertCategory[i].textContent || o_AlertCategory[i].innerText || o_AlertCategory[i].text));

                    AlertList = ""
                    ReportList = ""

                    if (AlertCategory == 1)
                        AlertList = "<input type='checkbox' name='chkInfrAlert'  id='chkOfflineAlert_" + AlertId + "' value='" + AlertId + "'><label class='lblText'>" + AlertName + "</label>";

                    if (AlertCategory == 2)
                        ReportList = "<input type='checkbox' name='chkLowBatteryAlert'  id='chkLowBattery_" + AlertId + "' value='" + AlertId + "'><label class='lblText'>" + AlertName + "</label>";

                    if (AlertList != "") {
                        row = document.createElement('tr');
                        AddCell(row, AlertList, "", "6", "", "left", "", "30px", "");
                        sTbl.appendChild(row);
                    }

                    if (ReportList != "") {
                        row = document.createElement('tr');
                        AddCell(row, ReportList, "", "6", "", "left", "", "30px", "");
                        sTbl2.appendChild(row);
                    }
                }
            }

            //Get Alert List
            GetScheduledEmailAlert(g_Email);
        }
    }
}
