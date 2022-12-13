var SortOrder='';
var SortColumn='';
var SortImg='';

SortColumn = "UpdatedOn";
SortOrder = "desc";
SortImg = "<image src='Images/downarrow.png' valign='middle' />";

// JScript File
function CreateXMLObj() {
    var obj = null;
    if (window.ActiveXObject) {
        try {
            obj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                obj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                obj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
        obj = new XMLHttpRequest();
        obj.overrideMimeType('text/xml');
    }
    return obj;
}

// Checking For Browser
function GetBrowserType() {
    var isIE = ((document.all) ? true : false); //for Internet Explorer
    var isFF = ((document.getElementById && !document.all) ? true : false); //for Mozilla Firefox


    if (!(window.ActiveXObject) && "ActiveXObject" in window) {
        return "isIE";
    }


    if (isIE) {
        return "isIE";
    }
    else if (isFF) {
        return "isFF";
    }
}

function doNext() {
    var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;
    var cnt = Number(currentpage) + 1;
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = cnt;

    document.getElementById("divLoading").style.display = "";

    GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), cnt)
}

function doPrev() {
    var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;
    var cnt = currentpage - 1;
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = cnt;

    document.getElementById("divLoading").style.display = "";

    GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), cnt)
}

function gotoPage() {
    var cnt = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

    if (cnt < 1)
        cnt = 1;

    document.getElementById("divLoading").style.display = "";

    GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), cnt)
}

var DbConnectorPath;
var g_Con_INI_Obj;

function GetIniHistoryInfo(SiteId, DeviceType, Date, DevicedIds, CurPage) {

    g_Con_INI_Obj = CreateXMLObj();

    document.getElementById("divLoading").style.display = "";

    if (g_Con_INI_Obj != null) {
        g_Con_INI_Obj.onreadystatechange = ajaxIniHistory;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetIniHistoryInfo&SiteId=" + SiteId + "&DeviceType=" + DeviceType + "&IDate=" + Date + "&sortcolumn=" + SortColumn + "&sortorder=" + SortOrder + "&DevicedIds=" + DevicedIds + "&CurPage=" + CurPage;

        if (GetBrowserType() == "isIE") {
            g_Con_INI_Obj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_Con_INI_Obj.open("GET", DbConnectorPath, true);
        }

        g_Con_INI_Obj.send(null);
    }
    return false;
}

var dsRoot;

function ajaxIniHistory() {
    if (g_Con_INI_Obj.readyState == 4) {
        if (g_Con_INI_Obj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Con_INI_Obj.responseXML.documentElement);
            dsRoot = g_Con_INI_Obj.responseXML.documentElement;

            var sTbl, sTblLen;

            sTbl = document.getElementById('tblIniTagHistoryInfo');
            document.getElementById("tblIniTagHistoryInfo").style.display = "";
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            sTbl = document.getElementById('tblIniMonitorHistoryInfo');
            document.getElementById("tblIniMonitorHistoryInfo").style.display = "";
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            sTbl = document.getElementById('tblIniStarHistoryInfo');
            document.getElementById("tblIniStarHistoryInfo").style.display = "";
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            if ($('#selDeviceType').val() == 1)
                LoadIniTagHistoryList();
            else if ($('#selDeviceType').val() == 2)
                LoadIniMonitorHistoryList();
            else if ($('#selDeviceType').val() == 3)
                LoadIniStarHistoryList();
            else {
                LoadIniTagHistoryList();
                LoadIniMonitorHistoryList();
                LoadIniStarHistoryList();
            }
        }
    }
}

function LoadIniTagHistoryList() {
    var sTbl, sTblLen;

    sTbl = document.getElementById('tblIniTagHistoryInfo');
    document.getElementById("tblIniTagHistoryInfo").style.display = "";
    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var Tagtable = $(dsRoot).find("Tag");
    var o_DataId = $(Tagtable).children().filter('DataId');
    var o_TagId = $(Tagtable).children().filter('TagId');
    var o_Date = $(Tagtable).children().filter('Date');
    var o_TotalPage = $(Tagtable).children().filter('TotalPage');
    var o_TotalCount = $(Tagtable).children().filter('TotalCount');

    var nTagRootLength = o_Date.length;

    row = document.createElement('tr');
    AddCellForSorting(row, "Date", 'siteOverview_TopLeft_Box', "", "", "center", "300px", "40px", "", "UpdatedOn", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCellForSorting(row, "Tag Id", 'siteOverview_Box', "", "", "center", "300px", "40px", "", "TagId", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCell(row, "Profile", 'siteOverview_Topright_Box', "", "", "center", "100px", "40px", "");
    sTbl.appendChild(row);


    if (nTagRootLength == 0) {
        var total_cnt = document.getElementById("tdtotalrec")
        total_cnt.innerHTML = "Total Records : " + nTagRootLength;

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
        document.getElementById("divLoading").style.display = "none";
        return;
    }

    //totalcount
    var ttcnt_lable = document.getElementById("tdtotalrec")
    ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

    //Totalpage
    var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
    ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

    var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
    doEnableButtonTag(MaxPageCnt);

    if (nTagRootLength > 0) {
        for (var i = 0; i <= nTagRootLength - 1; i++) {
            var DataId = (o_DataId[i].textContent || o_DataId[i].innerText || o_DataId[i].text);
            var TagId = (o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
            var Date = (o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text);

            row = document.createElement('tr');
            AddCell(row, Date, 'DeviceList_leftBox', "", "", "", "600px", "40px", "10px");
            AddCell(row, TagId, 'siteOverview_cell', "", "", "", "600px", "40px", "10px");

            var imgProfile = "<img src='Images/imgViewMore.png' title='Profile' onclick=\"OpenProfileDialog('" + DataId + "','" + TagId + "');\" border='0' style='width: 28px; height: 28px; padding-top: 10px; padding-bottom: 10px; cursor: pointer'/>";
            AddCell(row, imgProfile, 'siteOverview_cell', "", "", "", "200px", "20px", "10px");

            sTbl.appendChild(row);
        }
    }

    document.getElementById("divLoading").style.display = "none";

    try {
        PageVisitDetails(g_UserId, "INI Change History", enumPageAction.Filter, "History filtered for TagId " + TagId);
    }
    catch (e) {

    }
}

function LoadIniMonitorHistoryList() {

    sTbl = document.getElementById('tblIniMonitorHistoryInfo');
    document.getElementById("tblIniMonitorHistoryInfo").style.display = "";
    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    if ($('#selDeviceType').val() == 0) {
        row = document.createElement('tr');
        AddCell(row, "", '', "", "", "center", "", "20px");
        sTbl.appendChild(row);
    }

    row = document.createElement('tr');
    AddCellForSorting(row, "Date", 'siteOverview_TopLeft_Box', "", "", "center", "300px", "40px", "", "UpdatedOn", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCellForSorting(row, "Monitor Id", 'siteOverview_Box', "", "", "center", "300px", "40px", "", "MonitorId", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCell(row, "Profile", 'siteOverview_Topright_Box', "", "", "center", "100px", "40px", "");
    sTbl.appendChild(row);

    var Monitortable = $(dsRoot).find("Monitor");
    var o_DataId = $(Monitortable).children().filter('DataId');
    var o_MonitorId = $(Monitortable).children().filter('MonitorId');
    var o_Date = $(Monitortable).children().filter('Date');
    var o_TotalPage = $(Monitortable).children().filter('TotalPage');
    var o_TotalCount = $(Monitortable).children().filter('TotalCount');

    var nMonitorRootLength = o_Date.length;

    if (nMonitorRootLength == 0) {
        var total_cnt = document.getElementById("tdtotalrec")
        total_cnt.innerHTML = "Total Records : " + nMonitorRootLength;

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
        document.getElementById("divLoading").style.display = "none";
        return;
    }

    //totalcount
    var ttcnt_lable = document.getElementById("tdtotalrec")
    ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

    //Totalpage
    var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
    ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

    var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
    doEnableButtonTag(MaxPageCnt);

    if (nMonitorRootLength > 0) {
        for (var i = 0; i <= nMonitorRootLength - 1; i++) {
            var DataId = (o_DataId[i].textContent || o_DataId[i].innerText || o_DataId[i].text);
            var Date = (o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text);
            var MonitorId = (o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text);

            row = document.createElement('tr');
            AddCell(row, Date, 'DeviceList_leftBox', "", "", "", "600px", "40px", "10px");
            AddCell(row, MonitorId, 'siteOverview_cell', "", "", "", "600px", "40px", "10px");

            var imgProfile = "<img src='Images/imgViewMore.png' title='Profile' border='0' onclick=\"OpenProfileDialog('" + DataId + "','" + MonitorId + "');\" style='width: 28px; height: 28px; padding-top: 10px; padding-bottom: 10px; cursor: pointer'/>";
            AddCell(row, imgProfile, 'siteOverview_cell', "", "", "", "200px", "20px", "10px");

            sTbl.appendChild(row);
        }
    }

    document.getElementById("divLoading").style.display = "none";

    try {
        PageVisitDetails(g_UserId, "INI Change History", enumPageAction.Filter, "History filtered for MoniterId " + MonitorId);
    }
    catch (e) {

    }
}

function LoadIniStarHistoryList() {
    sTbl = document.getElementById('tblIniStarHistoryInfo');
    document.getElementById("tblIniStarHistoryInfo").style.display = "";
    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    if ($('#selDeviceType').val() == 0) {
        row = document.createElement('tr');
        AddCell(row, "", '', "", "", "center", "", "20px");
        sTbl.appendChild(row);
    }

    row = document.createElement('tr');
    AddCellForSorting(row, "Date", 'siteOverview_TopLeft_Box', "", "", "center", "300px", "40px", "", "UpdatedOn", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCellForSorting(row, "Mac Id", 'siteOverview_Box', "", "", "center", "300px", "40px", "", "MacId", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCell(row, "Profile", 'siteOverview_Topright_Box', "", "", "center", "100px", "40px", "");
    sTbl.appendChild(row);

    var Startable = $(dsRoot).find("Star");
    var o_DataId = $(Startable).children().filter('DataId');
    var o_MacId = $(Startable).children().filter('MacId');
    var o_Date = $(Startable).children().filter('Date');
    var o_TotalPage = $(Startable).children().filter('TotalPage');
    var o_TotalCount = $(Startable).children().filter('TotalCount');

    var nStarRootLength = o_Date.length;

    if (nStarRootLength == 0) {
        var total_cnt = document.getElementById("tdtotalrec")
        total_cnt.innerHTML = "Total Records : " + nStarRootLength;

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
        document.getElementById("divLoading").style.display = "none";
        return;
    }

    //totalcount
    var ttcnt_lable = document.getElementById("tdtotalrec")
    ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

    //Totalpage
    var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
    ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

    var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
    doEnableButtonTag(MaxPageCnt);

    if (nStarRootLength > 0) {
        for (var i = 0; i <= nStarRootLength - 1; i++) {
            var DataId = (o_DataId[i].textContent || o_DataId[i].innerText || o_DataId[i].text);
            var MacId = (o_MacId[i].textContent || o_MacId[i].innerText || o_MacId[i].text);
            var Date = (o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text);

            row = document.createElement('tr');
            AddCell(row, Date, 'DeviceList_leftBox', "", "", "", "600px", "40px", "10px");
            AddCell(row, MacId, 'siteOverview_cell', "", "", "", "600px", "40px", "10px");

            var imgProfile = "<img src='Images/imgViewMore.png' title='Profile' border='0' onclick=\"OpenProfileDialog('" + DataId + "','" + MacId + "');\" style='width: 28px; height: 28px; padding-top: 10px; padding-bottom: 10px; cursor: pointer'/>";
            AddCell(row, imgProfile, 'siteOverview_cell', "", "", "", "200px", "20px", "10px");

            sTbl.appendChild(row);
        }
    }

    document.getElementById("divLoading").style.display = "none";

    try {
        PageVisitDetails(g_UserId, "INI Change History", enumPageAction.Filter, "History filtered for MoniterId " + MacId);
    }
    catch (e) {

    }
}

function sortINI_History(sortCol) {

    document.getElementById("divLoading").style.display = "";

    if (SortColumn != sortCol) {
        SortOrder = "desc";
        SortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else {
        if (SortOrder == "desc") {

            SortOrder = "asc";
            SortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if (SortOrder == "asc") {
            SortOrder = "desc";
            SortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }
    if (sortCol != "") {
        SortColumn = sortCol;
    }

    var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

    GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), currentpage)
}

//Open Profile Details Dialog
function OpenProfileDialog(DataId, DeviceId) {

    var winWidth = $(window).width() - 700;
    var winHeight = $(window).height() - 50;

    var sTbl, sTblLen;
    if (GetBrowserType() == "isIE") {
        sTbl = document.getElementById('tblProfileInfo');
    }
    else if (GetBrowserType() == "isFF") {
        sTbl = document.getElementById('tblProfileInfo');
    }

    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var strtitle = "Profile Details";

    LoadProfileDetailsView(DataId);

    try {
        PageVisitDetails(g_UserId, "GMS - INI Change History", enumPageAction.View, $("#selDeviceType option:selected").text() + " Profile viewed in site - " + $("#ctl00_ContentPlaceHolder1_lblSiteName").text());
    }
    catch (e) {

    }

    //Open Dialog
    $("#Profile_dialog").dialog({
        height: winHeight,
        width: winWidth,
        title: strtitle,
        modal: true,
        show: {
            effect: "fade",
            duration: 500
        },
        hide: {
            effect: "fade",
            duration: 500
        }
    });
}

//Ajax Call for Profile
var g_Pbj;

function LoadProfileDetailsView(DataId) {
    g_Pbj = CreateXMLObj();

    document.getElementById("divLoadingPop").style.display = "";

    if (g_Pbj != null) {
        g_Pbj.onreadystatechange = ajaxLoadProfileDetailsView;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetIniHistoryProfileInfo&SiteId=" + siteVal + "&DeviceType=" + $('#selDeviceType').val() + "&DataId=" + DataId;

        if (GetBrowserType() == "isIE") {
            g_Pbj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Pbj.open("GET", DbConnectorPath, true);
        }
        g_Pbj.send(null);
    }

    return false;
}

//Ajax Readystate Change for Profile
function ajaxLoadProfileDetailsView() {
    if (g_Pbj.readyState == 4) {
        if (g_Pbj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Pbj.responseXML.documentElement);

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblProfileInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblProfileInfo');
            }

            var dsRoot = g_Pbj.responseXML.documentElement;

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            if ($('#selDeviceType').val() == 1)
                LoadTagProfile(dsRoot, sTbl);
            else if ($('#selDeviceType').val() == 2)
                LoadMonitorProfile(dsRoot, sTbl);
            else if ($('#selDeviceType').val() == 3)
                LoadStarProfile(dsRoot, sTbl);

            document.getElementById("divLoadingPop").style.display = "none";

        }
    }
}

function LoadTagProfile(dsRoot, sTbl) {

    var o_TagId = dsRoot.getElementsByTagName('TagId');
    var o_TagType = dsRoot.getElementsByTagName('TagType');
    var o_Profile = dsRoot.getElementsByTagName('Profile');
    var o_IRProfile = dsRoot.getElementsByTagName('IRProfile');
    var o_IRReportingTime = dsRoot.getElementsByTagName('IRReportingTime');
    var o_NoIRReportingTime = dsRoot.getElementsByTagName('NoIRReportingTime');
    var o_IRRXValue = dsRoot.getElementsByTagName('IRRXValue');
    var o_LFRegister = dsRoot.getElementsByTagName('LFRegister');
    var o_MotionSensorScanLogic = dsRoot.getElementsByTagName('MotionSensorScanLogic');
    var o_EnableFastPushbutton = dsRoot.getElementsByTagName('EnableFastPushbutton');
    var o_LFRX = dsRoot.getElementsByTagName('LFRX');
    var o_PagingProfile = dsRoot.getElementsByTagName('PagingProfile');
    var o_OperationMode = dsRoot.getElementsByTagName('OperationMode');
    var o_WiFiReportRates = dsRoot.getElementsByTagName('WiFiReportRates');
    var o_EnableWifiin900MHZ = dsRoot.getElementsByTagName('EnableWifiin900MHZ');
    var o_AlertDelay = dsRoot.getElementsByTagName('AlertDelay');
    var o_TagClass = dsRoot.getElementsByTagName('TagClass');
    var o_MinimumTemp = dsRoot.getElementsByTagName('MinimumTemp');
    var o_MaximumTemp = dsRoot.getElementsByTagName('MaximumTemp');
    var o_HighTemp = dsRoot.getElementsByTagName('HighTemp');
    var o_LowTemp = dsRoot.getElementsByTagName('LowTemp');
    var o_TemperatureReportRate = dsRoot.getElementsByTagName('TemperatureReportRate');
    var o_LocalAlert = dsRoot.getElementsByTagName('LocalAlert');
    var o_X2L = dsRoot.getElementsByTagName('X2L');
    var o_XL = dsRoot.getElementsByTagName('XL');
    var o_IPL = dsRoot.getElementsByTagName('IPL');
    var o_LongIROpen = dsRoot.getElementsByTagName('LongIROpen');
    var o_Probes = dsRoot.getElementsByTagName('Probes');
    var o_Probe1AlertMin = dsRoot.getElementsByTagName('Probe1AlertMin');
    var o_Probe1AlertMax = dsRoot.getElementsByTagName('Probe1AlertMax');
    var o_Probe2AlertMin = dsRoot.getElementsByTagName('Probe2AlertMin');
    var o_Probe2AlertMax = dsRoot.getElementsByTagName('Probe2AlertMax');
    var o_DoorAjarDetection = dsRoot.getElementsByTagName('DoorAjarDetection');
    var o_Probe1Temperature = dsRoot.getElementsByTagName('Probe1Temperature');
    var o_Probe2Temperature = dsRoot.getElementsByTagName('Probe2Temperature');
    var o_EnableHighAccuracy = dsRoot.getElementsByTagName('EnableHighAccuracy');

    var nRootLength = o_Profile.length;

    if (nRootLength > 0) {
        var TagId = '';
        if (o_TagId[0] != undefined)
            TagId = (o_TagId[0].textContent || o_TagId[0].innerText || o_TagId[0].text);

        var TagType = '';
        if (o_TagType[0] != undefined)
            TagType = (o_TagType[0].textContent || o_TagType[0].innerText || o_TagType[0].text);

        var Profile = '';
        if (o_Profile[0] != undefined)
            Profile = (o_Profile[0].textContent || o_Profile[0].innerText || o_Profile[0].text);

        var IRProfile = '';
        if (o_IRProfile[0] != undefined)
            IRProfile = (o_IRProfile[0].textContent || o_IRProfile[0].innerText || o_IRProfile[0].text);

        var IRReportingTime = '';
        if (o_IRReportingTime[0] != undefined)
            IRReportingTime = (o_IRReportingTime[0].textContent || o_IRReportingTime[0].innerText || o_IRReportingTime[0].text);

        var NoIRReportingTime = '';
        if (o_NoIRReportingTime[0] != undefined)
            NoIRReportingTime = (o_NoIRReportingTime[0].textContent || o_NoIRReportingTime[0].innerText || o_NoIRReportingTime[0].text);

        var IRRXValue = '';
        if (o_IRRXValue[0] != undefined)
            IRRXValue = (o_IRRXValue[0].textContent || o_IRRXValue[0].innerText || o_IRRXValue[0].text);

        var LFRegister = '';
        if (o_LFRegister[0] != undefined)
            LFRegister = (o_LFRegister[0].textContent || o_LFRegister[0].innerText || o_LFRegister[0].text);

        var MotionSensorScanLogic = '';
        if (o_MotionSensorScanLogic[0] != undefined)
            MotionSensorScanLogic = (o_MotionSensorScanLogic[0].textContent || o_MotionSensorScanLogic[0].innerText || o_MotionSensorScanLogic[0].text);

        var EnableFastPushbutton = '';
        if (o_EnableFastPushbutton[0] != undefined)
            EnableFastPushbutton = (o_EnableFastPushbutton[0].textContent || o_EnableFastPushbutton[0].innerText || o_EnableFastPushbutton[0].text);

        var LFRX = '';
        if (o_LFRX[0] != undefined)
            LFRX = (o_LFRX[0].textContent || o_LFRX[0].innerText || o_LFRX[0].text);

        var PagingProfile = '';
        if (o_PagingProfile[0] != undefined)
            PagingProfile = (o_PagingProfile[0].textContent || o_PagingProfile[0].innerText || o_PagingProfile[0].text);

        var OperationMode = '';
        if (o_OperationMode[0] != undefined)
            OperationMode = (o_OperationMode[0].textContent || o_OperationMode[0].innerText || o_OperationMode[0].text);

        var WiFiReportRates = '';
        if (o_WiFiReportRates[0] != undefined)
            WiFiReportRates = (o_WiFiReportRates[0].textContent || o_WiFiReportRates[0].innerText || o_WiFiReportRates[0].text);

        var EnableWifiin900MHZ = '';
        if (o_EnableWifiin900MHZ[0] != undefined)
            EnableWifiin900MHZ = (o_EnableWifiin900MHZ[0].textContent || o_EnableWifiin900MHZ[0].innerText || o_EnableWifiin900MHZ[0].text);

        var AlertDelay = '';
        if (o_AlertDelay[0] != undefined)
            AlertDelay = (o_AlertDelay[0].textContent || o_AlertDelay[0].innerText || o_AlertDelay[0].text);

        var TagClass = '';
        if (o_TagClass[0] != undefined)
            TagClass = (o_TagClass[0].textContent || o_TagClass[0].innerText || o_TagClass[0].text);

        var MinimumTemp = '';
        if (o_MinimumTemp[0] != undefined)
            MinimumTemp = (o_MinimumTemp[0].textContent || o_MinimumTemp[0].innerText || o_MinimumTemp[0].text);

        var MaximumTemp = '';
        if (o_MaximumTemp[0] != undefined)
            MaximumTemp = (o_MaximumTemp[0].textContent || o_MaximumTemp[0].innerText || o_MaximumTemp[0].text);

        var HighTemp = '';
        if (o_HighTemp[0] != undefined)
            HighTemp = (o_HighTemp[0].textContent || o_HighTemp[0].innerText || o_HighTemp[0].text);

        var LowTemp = '';
        if (o_LowTemp[0] != undefined)
            LowTemp = (o_LowTemp[0].textContent || o_LowTemp[0].innerText || o_LowTemp[0].text);

        var TemperatureReportRate = '';
        if (o_TemperatureReportRate[0] != undefined)
            TemperatureReportRate = (o_TemperatureReportRate[0].textContent || o_TemperatureReportRate[0].innerText || o_TemperatureReportRate[0].text);

        var LocalAlert = '';
        if (o_LocalAlert[0] != undefined)
            LocalAlert = (o_LocalAlert[0].textContent || o_LocalAlert[0].innerText || o_LocalAlert[0].text);

        var X2L = '';
        if (o_X2L[0] != undefined)
            X2L = (o_X2L[0].textContent || o_X2L[0].innerText || o_X2L[0].text);

        var XL = '';
        if (o_XL[0] != undefined)
            XL = (o_XL[0].textContent || o_XL[0].innerText || o_XL[0].text);

        var IPL = '';
        if (o_IPL[0] != undefined)
            IPL = (o_IPL[0].textContent || o_IPL[0].innerText || o_IPL[0].text);

        var LongIROpen = '';
        if (o_LongIROpen[0] != undefined)
            LongIROpen = (o_LongIROpen[0].textContent || o_LongIROpen[0].innerText || o_LongIROpen[0].text);

        var Probes = '';
        if (o_Probes[0] != undefined)
            Probes = (o_Probes[0].textContent || o_Probes[0].innerText || o_Probes[0].text);

        var Probe1AlertMin = '';
        if (o_Probe1AlertMin[0] != undefined)
            Probe1AlertMin = (o_Probe1AlertMin[0].textContent || o_Probe1AlertMin[0].innerText || o_Probe1AlertMin[0].text);

        var Probe1AlertMax = '';
        if (o_Probe1AlertMax[0] != undefined)
            Probe1AlertMax = (o_Probe1AlertMax[0].textContent || o_Probe1AlertMax[0].innerText || o_Probe1AlertMax[0].text);

        var Probe2AlertMin = '';
        if (o_Probe2AlertMin[0] != undefined)
            Probe2AlertMin = (o_Probe2AlertMin[0].textContent || o_Probe2AlertMin[0].innerText || o_Probe2AlertMin[0].text);

        var Probe2AlertMax = '';
        if (o_Probe2AlertMax[0] != undefined)
            Probe2AlertMax = (o_Probe2AlertMax[0].textContent || o_Probe2AlertMax[0].innerText || o_Probe2AlertMax[0].text);

        var DoorAjarDetection = '';
        if (o_DoorAjarDetection[0] != undefined)
            DoorAjarDetection = (o_DoorAjarDetection[0].textContent || o_DoorAjarDetection[0].innerText || o_DoorAjarDetection[0].text);

        var Probe1Temperature = '';
        if (o_Probe1Temperature[0] != undefined)
            Probe1Temperature = (o_Probe1Temperature[0].textContent || o_Probe1Temperature[0].innerText || o_Probe1Temperature[0].text);

        var Probe2Temperature = '';
        if (o_Probe2Temperature[0] != undefined)
            Probe2Temperature = (o_Probe2Temperature[0].textContent || o_Probe2Temperature[0].innerText || o_Probe2Temperature[0].text);

        var EnableHighAccuracy = '';
        if (o_EnableHighAccuracy[0] != undefined)
            EnableHighAccuracy = (o_EnableHighAccuracy[0].textContent || o_EnableHighAccuracy[0].innerText || o_EnableHighAccuracy[0].text);

        row = document.createElement('tr');
        AddCell(row, "Tag Profile", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTbl.appendChild(row);
        sTbl.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Tag Id : </span>" + setundefined(TagId), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Type : </span>" + setundefined(TagType), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Fast Push button : </span>" + setundefined(EnableFastPushbutton), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Profile : </span>" + setundefined(IRProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Motion Sensor Scan Logic : </span>" + setundefined(MotionSensorScanLogic), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Long IR : </span>" + setundefined(LongIROpen), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Washout Timer : </span>" + setundefined(TagClass), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR RX Profile: </span>" + setundefined(IRRXValue), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Tag Class : </span>" + setundefined(TagClass), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Reporting Time : </span>" + setundefined(IRReportingTime), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>LF RX : </span>" + setundefined(LFRX), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>RF Reporting Time : </span>" + setundefined(NoIRReportingTime), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>LF Register : </span>" + setundefined(LFRegister), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Alert Delay : </span>" + setundefined(AlertDelay), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Paging Profile : </span>" + setundefined(PagingProfile), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Local Alert : </span>" + setundefined(LocalAlert), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperationMode), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Enable High Accuracy : </span>" + setundefined(EnableHighAccuracy), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Door Ajar Detection : </span>" + setundefined(DoorAjarDetection), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>WiFi Report Rates : </span>" + setundefined(WiFiReportRates), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Wifi in 900MHZ : </span>" + setundefined(EnableWifiin900MHZ), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Minimum Temp : </span>" + setundefined(MinimumTemp), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Maximum Temp : </span>" + setundefined(MaximumTemp), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>High Temp : </span>" + setundefined(HighTemp), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Low Temp : </span>" + setundefined(LowTemp), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Measurement Rate : </span>" + setundefined(TemperatureReportRate), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>XL : </span>" + setundefined(XL), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IPL : </span>" + setundefined(IPL), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>X2L : </span>" + setundefined(X2L), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe1 Alert Min : </span>" + setundefined(Probe1AlertMin), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe1 Alert Max : </span>" + setundefined(Probe1AlertMax), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe2 Alert Min : </span>" + setundefined(Probe2AlertMin), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe2 Alert Max : </span>" + setundefined(Probe2AlertMax), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probes : </span>" + setundefined(Probes), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);
    }
}


//*******************************************************************
//	Function Name	:	LoadMonitorProfile
//	Input			:	dsRoot,sTbl,sTblShip
//	Description		:	Load Monitor Profile from ajax Response
//*******************************************************************
function LoadMonitorProfile(dsRoot, sTblProfile) {
    var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
    var o_MonitorType = dsRoot.getElementsByTagName('MonitorType');
    var o_Profile = dsRoot.getElementsByTagName('Profile');
    var o_IRProfile = dsRoot.getElementsByTagName('IRProfile');
    var o_PowerLevel = dsRoot.getElementsByTagName('PowerLevel');
    var o_RoomBleeding = dsRoot.getElementsByTagName('RoomBleeding');
    var o_NoiseLevel = dsRoot.getElementsByTagName('NoiseLevel');
    var o_Masking = dsRoot.getElementsByTagName('Masking');
    var o_MasterSlave = dsRoot.getElementsByTagName('MasterSlave');
    var o_SpecialProfile = dsRoot.getElementsByTagName('SpecialProfile');
    var o_OperatingMode = dsRoot.getElementsByTagName('OperatingMode');
    var o_Modes = dsRoot.getElementsByTagName('Modes');
    var o_AlertSupressionTime = dsRoot.getElementsByTagName('AlertSupressionTime');
    var o_RoomId = dsRoot.getElementsByTagName('RoomId');

    var nRootLength = o_Profile.length;

    if (nRootLength > 0) {
        var MonitorId = '';
        if (o_MonitorId[0] != undefined)
            MonitorId = (o_MonitorId[0].textContent || o_MonitorId[0].innerText || o_MonitorId[0].text);

        var MonitorType = '';
        if (o_MonitorType[0] != undefined)
            MonitorType = (o_MonitorType[0].textContent || o_MonitorType[0].innerText || o_MonitorType[0].text);

        var RoomId = '';
        if (o_RoomId[0] != undefined)
            RoomId = (o_RoomId[0].textContent || o_RoomId[0].innerText || o_RoomId[0].text);

        var Profile = '';
        if (o_Profile[0] != undefined)
            Profile = (o_Profile[0].textContent || o_Profile[0].innerText || o_Profile[0].text);

        var IRProfile = '';
        if (o_IRProfile[0] != undefined)
            IRProfile = (o_IRProfile[0].textContent || o_IRProfile[0].innerText || o_IRProfile[0].text);

        var PowerLevel = '';
        if (o_PowerLevel[0] != undefined)
            PowerLevel = (o_PowerLevel[0].textContent || o_PowerLevel[0].innerText || o_PowerLevel[0].text);

        var RoomBleeding = '';
        if (o_RoomBleeding[0] != undefined)
            RoomBleeding = (o_RoomBleeding[0].textContent || o_RoomBleeding[0].innerText || o_RoomBleeding[0].text);

        var NoiseLevel = '';
        if (o_NoiseLevel[0] != undefined)
            NoiseLevel = (o_NoiseLevel[0].textContent || o_NoiseLevel[0].innerText || o_NoiseLevel[0].text);

        var Masking = '';
        if (o_Masking[0] != undefined)
            Masking = (o_Masking[0].textContent || o_Masking[0].innerText || o_Masking[0].text);

        var MasterSlave = '';
        if (o_MasterSlave[0] != undefined)
            MasterSlave = (o_MasterSlave[0].textContent || o_MasterSlave[0].innerText || o_MasterSlave[0].text);

        var SpecialProfile = '';
        if (o_SpecialProfile[0] != undefined)
            SpecialProfile = (o_SpecialProfile[0].textContent || o_SpecialProfile[0].innerText || o_SpecialProfile[0].text);

        var OperatingMode = '';
        if (o_OperatingMode[0] != undefined)
            OperatingMode = (o_OperatingMode[0].textContent || o_OperatingMode[0].innerText || o_OperatingMode[0].text);

        var Modes = '';
        if (o_Modes[0] != undefined)
            Modes = (o_Modes[0].textContent || o_Modes[0].innerText || o_Modes[0].text);

        var AlertSupressionTime = '';
        if (o_AlertSupressionTime[0] != undefined)
            AlertSupressionTime = (o_AlertSupressionTime[0].textContent || o_AlertSupressionTime[0].innerText || o_AlertSupressionTime[0].text);

        row = document.createElement('tr');
        AddCell(row, "Monitor Profile ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblProfile.appendChild(row);
        sTblProfile.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Monitor Id : </span>" + setundefined(MonitorId), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Type : </span>" + setundefined(MonitorType), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Power Level : </span>" + setundefined(PowerLevel), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Profile : </span>" + setundefined(IRProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>NoiseLevel : </span>" + setundefined(NoiseLevel), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>MasterSlave : </span>" + setundefined(MasterSlave), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Room Bleeding : </span>" + setundefined(RoomBleeding), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Super Sync/Marker : </span>" + setundefined(Masking), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>SpecialProfile : </span>" + setundefined(SpecialProfile), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperatingMode), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR ID : </span>" + setundefined(RoomId), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Modes : </span>" + setundefined(Modes), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Alert Supression Time : </span>" + setundefined(AlertSupressionTime), 'DeviceList_rightBox_DeviceDetailsDatasText', '', "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);
    }
}

//*******************************************************************
//	Function Name	:	LoadStarProfile
//	Input			:	dsRoot,sTbl
//	Description		:	Load Star Profile from ajax Response
//*******************************************************************
function LoadStarProfile(dsRoot, sTblProfile) {
    var o_MacId = dsRoot.getElementsByTagName('MacId');
    var o_StarType = dsRoot.getElementsByTagName('StarType');
    var o_DHCP = dsRoot.getElementsByTagName('DHCP');
    var o_SaveSettings = dsRoot.getElementsByTagName('SaveSettings');
    var o_StaticIP = dsRoot.getElementsByTagName('StaticIP');
    var o_Subnet = dsRoot.getElementsByTagName('Subnet');
    var o_Gateway = dsRoot.getElementsByTagName('Gateway');
    var o_TimeServerIP = dsRoot.getElementsByTagName('TimeServerIP');
    var o_ServerIP = dsRoot.getElementsByTagName('ServerIP');
    var o_PagingServerIP = dsRoot.getElementsByTagName('PagingServerIP');
    var o_LocationServerIP1 = dsRoot.getElementsByTagName('LocationServerIP1');
    var o_LocationServerIP2 = dsRoot.getElementsByTagName('LocationServerIP2');
    var o_StarId = dsRoot.getElementsByTagName('StarId');
    var o_IPAddr = dsRoot.getElementsByTagName('IPAddr');
    var o_SuperSyncProfile = dsRoot.getElementsByTagName('SuperSyncProfile');
    var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
    var o_Version = dsRoot.getElementsByTagName('Version');

    var nRootLength = o_MacId.length;

    if (nRootLength > 0) {
        var MacId = '';
        if (o_MacId[0] != undefined)
            MacId = (o_MacId[0].textContent || o_MacId[0].innerText || o_MacId[0].text);

        var StarType = '';
        if (o_StarType[0] != undefined)
            StarType = (o_StarType[0].textContent || o_StarType[0].innerText || o_StarType[0].text);

        var DHCP = '';
        if (o_DHCP[0] != undefined)
            DHCP = (o_DHCP[0].textContent || o_DHCP[0].innerText || o_DHCP[0].text);

        var SaveSettings = '';
        if (o_SaveSettings[0] != undefined)
            SaveSettings = (o_SaveSettings[0].textContent || o_SaveSettings[0].innerText || o_SaveSettings[0].text);

        var StaticIP = '';
        if (o_StaticIP[0] != undefined)
            StaticIP = (o_StaticIP[0].textContent || o_StaticIP[0].innerText || o_StaticIP[0].text);

        var Subnet = '';
        if (o_Subnet[0] != undefined)
            Subnet = (o_Subnet[0].textContent || o_Subnet[0].innerText || o_Subnet[0].text);

        var Gateway = '';
        if (o_Gateway[0] != undefined)
            Gateway = (o_Gateway[0].textContent || o_Gateway[0].innerText || o_Gateway[0].text);

        var TimeServerIP = '';
        if (o_TimeServerIP[0] != undefined)
            TimeServerIP = (o_TimeServerIP[0].textContent || o_TimeServerIP[0].innerText || o_TimeServerIP[0].text);

        var ServerIP = '';
        if (o_ServerIP[0] != undefined)
            ServerIP = (o_ServerIP[0].textContent || o_ServerIP[0].innerText || o_ServerIP[0].text);

        var PagingServerIP = '';
        if (o_PagingServerIP[0] != undefined)
            PagingServerIP = (o_PagingServerIP[0].textContent || o_PagingServerIP[0].innerText || o_PagingServerIP[0].text);

        var LocationServerIP1 = '';
        if (o_LocationServerIP1[0] != undefined)
            LocationServerIP1 = (o_LocationServerIP1[0].textContent || o_LocationServerIP1[0].innerText || o_LocationServerIP1[0].text);

        var LocationServerIP2 = '';
        if (o_LocationServerIP2[0] != undefined)
            LocationServerIP2 = (o_LocationServerIP2[0].textContent || o_LocationServerIP2[0].innerText || o_LocationServerIP2[0].text);

        var StarId = '';
        if (o_StarId[0] != undefined)
            StarId = (o_StarId[0].textContent || o_StarId[0].innerText || o_StarId[0].text);

        var IPAddr = '';
        if (o_IPAddr[0] != undefined)
            IPAddr = (o_IPAddr[0].textContent || o_IPAddr[0].innerText || o_IPAddr[0].text);

        var SuperSyncProfile = '';
        if (o_SuperSyncProfile[0] != undefined)
            SuperSyncProfile = (o_SuperSyncProfile[0].textContent || o_SuperSyncProfile[0].innerText || o_SuperSyncProfile[0].text);

        var LockedStarId = '';
        if (o_LockedStarId[0] != undefined)
            LockedStarId = (o_LockedStarId[0].textContent || o_LockedStarId[0].innerText || o_LockedStarId[0].text);

        var Version = '';
        if (o_Version[0] != undefined)
            Version = (o_Version[0].textContent || o_Version[0].innerText || o_Version[0].text);

        row = document.createElement('tr');
        AddCell(row, "Star Profile", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblProfile.appendChild(row);
        sTblProfile.style.display = "";

        //Datas
        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Mac Id : </span>" + setundefined(MacId), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Star Id : </span>" + setundefined(StarId), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Super Sync Profile : </span>" + setundefined(SuperSyncProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IP Mode : </span>" + setundefined(DHCP), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>StaticIP : </span>" + setundefined(StaticIP), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Subnet : </span>" + setundefined(Subnet), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Gateway : </span>" + setundefined(Gateway), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Timing Server IP: </span>" + setundefined(TimeServerIP), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>ServerIP : </span>" + setundefined(ServerIP), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Paging Server IP: </span>" + setundefined(PagingServerIP), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Location Server IP1 : </span>" + setundefined(LocationServerIP1), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Location Server IP2: </span>" + setundefined(LocationServerIP2), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Save Settings : </span>" + setundefined(SaveSettings), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);
    }
}

function doEnableButtonTag(MaxPageCnt) {

    var curnPage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo")
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