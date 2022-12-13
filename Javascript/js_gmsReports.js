
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
var g_ReportType;
var g_IsDownload;
var g_IsDeviceSummary;

function LoadAllReports(SiteId, ReportType, DeviceType, StartDate, EndDate, DeviceId, IsDownload, StarId, IsChkTTSyncError, IsConnFailed, SiteName, PagingCount, LocationCount, TriggerCount, FilterCond, LocationFilterCond, TriggerFilterCond, GroupCond, sStarsUpgradeMode, sStarsTTSyncError, sStarsNotReceivingData, sStarsNotReceiving24hr, sStarsSeenNetworkIssue) {

    g_ReportType = ReportType;
    g_IsDownload = IsDownload;

    document.getElementById("divLoading").style.display = "";

    inf_Obj = CreateTagXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxReports;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetAllReports&SiteId=" + SiteId + "&ReportType=" + ReportType + "&DeviceType=" + DeviceType + "&FromDate=" + StartDate + "&ToDate=" + EndDate + "&IsDownload=" + IsDownload + "&StarId=" + StarId + "&IsChkTTSyncError=" + IsChkTTSyncError + "&DeviceId=" + DeviceId + "&IsConnFailed=" + IsConnFailed + "&SiteName=" + SiteName + "&PagingCount=" + PagingCount + "&LocationCount=" + LocationCount + "&TriggerCount=" + TriggerCount + "&FilterCond=" + FilterCond + "&LocationFilterCond=" + LocationFilterCond + "&TriggerFilterCond=" + TriggerFilterCond + "&GroupCond=" + GroupCond + "&StarsUpgradeMode=" + sStarsUpgradeMode + "&StarsTTSyncError=" + sStarsTTSyncError + "&StarsNotReceivingData=" + sStarsNotReceivingData + "&StarsNotReceiving24hr=" + sStarsNotReceiving24hr + "&StarsSeenNetworkIssue=" + sStarsSeenNetworkIssue;

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
var g_dsRoot;
function ajaxReports() {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {
            var dsRoot = inf_Obj.responseXML.documentElement;

            if (g_ReportType == enumReportType.MonitorAnalysisReport) {
                g_dsRoot = dsRoot;
                if (g_IsDeviceSummary == 0) {
                    if (g_IsDownload == "0") {
                        var spanID = $('.current').attr('id');
                        if (spanID == "spnPagingSummary") {
                            spanID = "spnCollisionSummary";
                        }
                        else if (spanID == "spnCollisionSummary") {
                            spanID = "spnPagingSummary";
                        }
                        $('#' + spanID).click();
                    }
                    else if (g_IsDownload > 0) {
                        var spanID = $('.current').attr('id');
                        if (spanID == "spnPagingSummary") {
                            ExportCollisionAnalysis(g_dsRoot);
                        }
                        else if (spanID == "spnCollisionSummary") {
                            ExportPagingAnalysis(g_dsRoot);
                        }
                        else {
                            document.getElementById("divLoading").style.display = "none";
                        }
                    }
                }
            }
            else if (g_ReportType == enumReportType.DeviceSummary) {
                LoadDeviceSummary(dsRoot);
            }
            else if (g_ReportType == enumReportType.TTSyncErrReport && g_IsDownload == "0") {
                LoadTTSyncErrReportsStar(dsRoot);
            }

            else if (g_ReportType == enumReportType.TTSyncErrReport && g_IsDownload > 0) {
                DownloadTTSyncERRORReport(dsRoot);
            }
            else if (g_ReportType == enumReportType.ConnectivityReport && g_IsDownload == "0") {
                LoadConnectivityReport(dsRoot, IsConnFailed);
            }
            else if (g_ReportType == enumReportType.ConnectivityReport && g_IsDownload > 0) {
                DownloadConnectivityReport(dsRoot);
            }
            else if (g_ReportType == enumReportType.DefectiveReport) {
                if (g_IsDownload > 0) {
                    ExportDefectiveDeviceReports(dsRoot);
                }
                else {
                    LoadDefectiveReport(dsRoot);
                }
            }
        }
    }
}

function LoadTTSyncErrReportsStar(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    var sTbl = document.getElementById("tblTTSyncErrReportsStar");
    var sTblTblody = document.getElementById("tblTTSyncErrReportsStarbody");

    if (GetBrowserType() == "isIE") {
        sTbl = document.getElementById('tblTTSyncErrReportsStarbody');
    }
    else if (GetBrowserType() == "isFF") {
        sTbl = document.getElementById('tblTTSyncErrReportsStarbody');
    }

    sTblLen = sTbl.rows.length;

    clearTableRows(sTbl, sTblLen);

    var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
    var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();

    var dsRoot = inf_Obj.responseXML.documentElement;

    var TTSyncERRORR = $(dsRoot).find("TTSyncERRORReport");

    var o_StarId = $(TTSyncERRORR).children().filter('StarId');
    var o_Slot = $(TTSyncERRORR).children().filter('Slot');
    var o_StarType = $(TTSyncERRORR).children().filter('StarType');
    var o_BeaconSlot = $(TTSyncERRORR).children().filter('BeaconSlot');
    var o_TTSyncSlot = $(TTSyncERRORR).children().filter('TTSyncSlot');
    var o_TTSyncRepsSlot = $(TTSyncERRORR).children().filter('TTSyncRepsSlot');
    var o_PCComDelay = $(TTSyncERRORR).children().filter('PCComDelay');
    var o_GetTimeDelay = $(TTSyncERRORR).children().filter('GetTimeDelay');
    var o_MacId = $(TTSyncERRORR).children().filter('MacId');
    var o_IPAddr = $(TTSyncERRORR).children().filter('IPAddr');
    var o_Version = $(TTSyncERRORR).children().filter('Version');
    var o_ResCnt = $(TTSyncERRORR).children().filter('ResCnt');
    var o_ErrorCnt = $(TTSyncERRORR).children().filter('ErrorCnt');
    var o_TTSyncError = $(TTSyncERRORR).children().filter('TTSyncError');
    var o_PagingResCnt = $(TTSyncERRORR).children().filter('PagingCnt');
    var o_PagingDataCnt = $(TTSyncERRORR).children().filter('PDataCnt');
    var o_LocationResCnt = $(TTSyncERRORR).children().filter('LocationCnt');
    var o_LocationDataCnt = $(TTSyncERRORR).children().filter('LDataCnt');
    var o_Updatedon = $(TTSyncERRORR).children().filter('Updatedon');
    var o_StarCount = $(TTSyncERRORR).children().filter('StarCount');

    nRootLength = o_StarId.length;

    //Header
    var row;

    document.getElementById('lblStarsInUpgradeMode').innerHTML = "";
    document.getElementById('lblStarsInTimeSyncError').innerHTML = "";
    document.getElementById('lblStarsNotReceivingData').innerHTML = "";
    document.getElementById('lblStarsInUpgradeModeCount').innerHTML = "";
    document.getElementById('lblStarsInTimeSyncErrorCount').innerHTML = "";
    document.getElementById('lblStarsNotReceivingDataCount').innerHTML = "";
    document.getElementById('lblTotalStars').innerHTML = "";
    document.getElementById('lblStarsNotReceiving24hourDataCount').innerHTML = "";
    document.getElementById('lblStarsNotReceiving24hourData').innerHTML = "";
    document.getElementById('lblStarsSeenNetworkIssueCount').innerHTML = "";
    document.getElementById('lblStarsSeenNetworkIssue').innerHTML = "";

    if (nRootLength > 0) {

        $('#tblTTSyncErrReportsStar').css('width', '1800px');
        $('#tr_TTSyncSearch').css('width', '60%');
        $('#divSyncGraph').show();

        var cssClass = "";
        var cssLeftClass = "";
        var sno = 0;

        var UpgradeStarId = "";
        var StarsInTTSyncError = "";
        var StarsNotReceivingData = "";
        var StarLast10HrData = "";
        var StarsNotReceiving24hourData = "";
        var StarsInError = "";

        var sVersionCount = 0;
        var sTimeSyncErrorCount = 0;
        var sStarsnotreceivingCount = 0;
        var sStarsnotreceiving24hourCount = 0;
        var StarsInErrorCount = 0;

        for (var i = 0; i < nRootLength; i++) {

            var StarId = setundefined((o_StarId[i].textContent || o_StarId[i].innerText || o_StarId[i].text));
            var Slot = setundefined((o_Slot[i].textContent || o_Slot[i].innerText || o_Slot[i].text));
            var StarType = setundefined((o_StarType[i].textContent || o_StarType[i].innerText || o_StarType[i].text));
            var MacId = setundefined((o_MacId[i].textContent || o_MacId[i].innerText || o_MacId[i].text));
            var IPAddr = setundefined((o_IPAddr[i].textContent || o_IPAddr[i].innerText || o_IPAddr[i].text));
            var Version = setundefined((o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text));
            var ResCnt = setundefined((o_ResCnt[i].textContent || o_ResCnt[i].innerText || o_ResCnt[i].text));
            var ErrorCnt = setundefined((o_ErrorCnt[i].textContent || o_ErrorCnt[i].innerText || o_ErrorCnt[i].text));
            var TTSyncError = setundefined((o_TTSyncError[i].textContent || o_TTSyncError[i].innerText || o_TTSyncError[i].text));
            var PagingResCnt = setundefined((o_PagingResCnt[i].textContent || o_PagingResCnt[i].innerText || o_PagingResCnt[i].text));
            var PagingDataCnt = setundefined((o_PagingDataCnt[i].textContent || o_PagingDataCnt[i].innerText || o_PagingDataCnt[i].text));
            var LocationResCnt = setundefined((o_LocationResCnt[i].textContent || o_LocationResCnt[i].innerText || o_LocationResCnt[i].text));
            var LocationDataCnt = setundefined((o_LocationDataCnt[i].textContent || o_LocationDataCnt[i].innerText || o_LocationDataCnt[i].text));
            var Updatedon = setundefined((o_Updatedon[i].textContent || o_Updatedon[i].innerText || o_Updatedon[i].text));
            var StarCount = setundefined((o_StarCount[i].textContent || o_StarCount[i].innerText || o_StarCount[i].text));

            row = document.createElement('tr');

            cssClass = "tableData_cell";
            cssLeftClass = "tableData_cell_left";

            sno = sno + 1;

            if (Version > 200) {
                sVersionCount = sVersionCount + 1;
                UpgradeStarId += "<a style='cursor:pointer; text-decoration:none;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + g_SiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>" + ", ";
                cssClass = "tableData_cell_StarUpgradeModel";
                cssLeftClass = "tableData_cell_left_StarUpgradeModel";
            }

            if (ResCnt > 0 && PagingDataCnt == 0 && LocationDataCnt == 0 && (StarType == 'Beacon Generator' || StarType == 'Regular' || StarType == 'Ethernet')) {
                sStarsnotreceivingCount = sStarsnotreceivingCount + 1;
                StarsNotReceivingData += "<a style='cursor:pointer; text-decoration:none;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + g_SiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>" + ", ";
                cssClass = "tableData_cell_StarsNotReceiving";
                cssLeftClass = "tableData_cell_left_StarsNotReceiving";
            }

            if (StarCount < 24) {
                sStarsnotreceiving24hourCount = sStarsnotreceiving24hourCount + 1;
                StarsNotReceiving24hourData += "<a style='cursor:pointer; text-decoration:none;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + g_SiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>" + ", ";
                cssClass = "tableData_cell_StarsNotReceiving24Hr";
                cssLeftClass = "tableData_cell_left_StarsNotReceiving24Hr";
            }

            if (ErrorCnt > 200 && (StarType == 'Beacon Generator' || StarType == 'Regular' || StarType == 'Ethernet')) {
                StarsInErrorCount = StarsInErrorCount + 1;
                StarsInError += "<a style='cursor:pointer; text-decoration:none;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + g_SiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>" + ", ";
                cssClass = "tableData_cell_StarsSeenNetworkIssue";
                cssLeftClass = "tableData_cell_left_StarsSeenNetworkIssue";
            }

            if (TTSyncError > 100) {
                sTimeSyncErrorCount = sTimeSyncErrorCount + 1;
                StarsInTTSyncError += "<a style='cursor:pointer; text-decoration:none;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + g_SiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>" + ", ";
                cssClass = "tableData_cell_StarTTSyncError";
                cssLeftClass = "tableData_cell_left_StarTTSyncError";
            }

            StarLast10HrData = "<a style='cursor:pointer; text-decoration:none;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + g_SiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>";

            AddCell(row, sno, cssLeftClass, "", "", "right", "60px", "20px", "");
            AddCell(row, StarLast10HrData, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, Version, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, Slot, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, StarType, cssClass, "", "", "left", "100px", "20px", "");
            AddCell(row, MacId, cssClass, "", "", "left", "150px", "20px", "");
            AddCell(row, IPAddr, cssClass, "", "", "left", "150px", "20px", "");
            AddCell(row, Updatedon, cssClass, "", "", "left", "120px", "20px", "");
            AddCell(row, ResCnt, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, PagingResCnt, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, PagingDataCnt, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, LocationDataCnt, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, LocationResCnt, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, TTSyncError, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, ErrorCnt, cssClass, "", "", "right", "60px", "20px", "");
            AddCell(row, StarCount, cssClass, "", "", "right", "60px", "20px", "");

            sTblTblody.appendChild(row);
        }

        DatatablesLoadGraph("#tblTTSyncErrReportsStar", [8], 0, "asc", 10);

        UpgradeStarId = UpgradeStarId.substring(0, UpgradeStarId.length - 2);
        StarsInTTSyncError = StarsInTTSyncError.substring(0, StarsInTTSyncError.length - 2);
        StarsNotReceivingData = StarsNotReceivingData.substring(0, StarsNotReceivingData.length - 2);
        StarsNotReceiving24hourData = StarsNotReceiving24hourData.substring(0, StarsNotReceiving24hourData.length - 2);
        StarsInError = StarsInError.substring(0, StarsInError.length - 2);


        if (UpgradeStarId == "") {
            UpgradeStarId = "--";
            $("#tr_StarsUpgradeMode").hide();
        }
        else {
            $("#tr_StarsUpgradeMode").show();
        }

        if (StarsInTTSyncError == "") {
            StarsInTTSyncError = "--";
            $("#tr_StarsTimeSyncError").hide();
        }
        else {
            $("#tr_StarsTimeSyncError").show();
        }

        if (StarsNotReceivingData == "") {
            StarsNotReceivingData = "--";
            $("#tr_StarsNotReceivingData").hide();
        }
        else {
            $("#tr_StarsNotReceivingData").show();
        }

        if (StarsNotReceiving24hourData == "") {
            StarsNotReceiving24hourData = "--";
            $("#tr_StarsNotReceiving24hrData").hide();
        }
        else {
            $("#tr_StarsNotReceiving24hrData").show();
        }

        if (StarsInError == "") {
            StarsInError = "--";
            $("#tr_StarsNetworkIssue").hide();
        }
        else {
            $("#tr_StarsNetworkIssue").show();
        }

        document.getElementById('lblStarsInUpgradeMode').innerHTML = UpgradeStarId;
        document.getElementById('lblStarsInTimeSyncError').innerHTML = StarsInTTSyncError;
        document.getElementById('lblStarsNotReceivingData').innerHTML = StarsNotReceivingData;
        document.getElementById('lblStarsInUpgradeModeCount').innerHTML = sVersionCount;
        document.getElementById('lblStarsInTimeSyncErrorCount').innerHTML = sTimeSyncErrorCount;
        document.getElementById('lblStarsNotReceivingDataCount').innerHTML = sStarsnotreceivingCount;
        document.getElementById('lblTotalStars').innerHTML = sno;
        document.getElementById('lblStarsNotReceiving24hourDataCount').innerHTML = sStarsnotreceiving24hourCount;
        document.getElementById('lblStarsNotReceiving24hourData').innerHTML = StarsNotReceiving24hourData;
        document.getElementById('lblStarsSeenNetworkIssueCount').innerHTML = StarsInErrorCount;
        document.getElementById('lblStarsSeenNetworkIssue').innerHTML = StarsInError;


        $("#tblTTSyncErrReportsStarCounts").show();
    }
    else {

        $("#tblTTSyncErrReportsStarCounts").hide();

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 16, "", "center", "700px", "40px", "");
        sTblTblody.appendChild(row);

        document.getElementById('lblStarsInUpgradeMode').innerHTML = "--";
        document.getElementById('lblStarsInTimeSyncError').innerHTML = "--";
        document.getElementById('lblStarsNotReceivingData').innerHTML = "--";
        document.getElementById('lblStarsInUpgradeModeCount').innerHTML = 0;
        document.getElementById('lblStarsInTimeSyncErrorCount').innerHTML = 0;
        document.getElementById('lblStarsNotReceivingDataCount').innerHTML = 0;
        document.getElementById('lblTotalStars').innerHTML = 0;
        document.getElementById('lblStarsNotReceiving24hourDataCount').innerHTML = 0;
        document.getElementById('lblStarsNotReceiving24hourData').innerHTML = "--";
        document.getElementById('lblStarsSeenNetworkIssueCount').innerHTML = 0;
        document.getElementById('lblStarsSeenNetworkIssue').innerHTML = "--";
    }

    //Star Version Count
    var sTblSummary = document.getElementById("tblStarVersionSummary");

    if (GetBrowserType() == "isIE") {
        sTblSummary = document.getElementById('tblStarVersionSummary');
    }
    else if (GetBrowserType() == "isFF") {
        sTblSummary = document.getElementById('tblStarVersionSummary');
    }

    var sTblLenSummary = sTblSummary.rows.length;

    clearTableRows(sTblSummary, sTblLenSummary);

    var StarSummary = $(dsRoot).find("StarSummary");

    var o_UniqueVersion = $(StarSummary).children().filter('UniqueVersion');
    var o_Count = $(StarSummary).children().filter('Count');
    var o_TotalStars = $(StarSummary).children().filter('TotalStars');
    var o_InActiveStars = $(StarSummary).children().filter('InActiveStars');

    nRootLength = o_UniqueVersion.length;

    row = document.createElement('tr');
    AddCell(row, "Version", "Summary_header_cell", "", "", "right", "60px", "", "");
    AddCell(row, "#", "Summary_header_cell", "", "", "right", "60px", "", "");
    sTblSummary.appendChild(row);

    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {

            var UniqueVersion = setundefined((o_UniqueVersion[i].textContent || o_UniqueVersion[i].innerText || o_UniqueVersion[i].text));
            var Count = setundefined((o_Count[i].textContent || o_Count[i].innerText || o_Count[i].text));
            var TotalStars = setundefined((o_TotalStars[i].textContent || o_TotalStars[i].innerText || o_TotalStars[i].text));
            var InActiveStars = setundefined((o_InActiveStars[i].textContent || o_InActiveStars[i].innerText || o_InActiveStars[i].text));

            if ($('#chkStarsUpgradeMode').prop('checked') || $('#chkStarsTTSyncError').prop('checked') || $('#chkStarsNotReceivingData').prop('checked') || $('#chkStarsNotReceiving24hr').prop('checked') || $('#chkStarsSeenNetworkIssue').prop('checked')) {
                TotalStars = document.getElementById("hdnTotalStars").value;
                InActiveStars = document.getElementById("hdnInactiveStars").value;
                document.getElementById('lblTotalStars').innerHTML = TotalStars;
                document.getElementById('lblInActiveStars').innerHTML = InActiveStars;
            } else {
                document.getElementById("hdnTotalStars").value = TotalStars;
                document.getElementById("hdnInactiveStars").value = InActiveStars;
                document.getElementById('lblTotalStars').innerHTML = document.getElementById("hdnTotalStars").value;
                document.getElementById('lblInActiveStars').innerHTML = document.getElementById("hdnInactiveStars").value;
            }

            row = document.createElement('tr');
            AddCell(row, UniqueVersion, "Summary_cell", "", "", "right", "60px", "", "");
            AddCell(row, Count, "Summary_cell", "", "", "right", "60px", "", "");
            sTblSummary.appendChild(row);
        }
        $("#tblTTSyncErrReportsStarCounts").show();

    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
        sTblSummary.appendChild(row);
        $("#tblTTSyncErrReportsStarCounts").show();
        document.getElementById('lblInActiveStars').innerHTML = "0";
    }

    // Page and Location Count graph

    var PageLocationCount = $(dsRoot).find("PageLocationCount");

    var o_Hour = $(PageLocationCount).children().filter('Hour');
    var o_LocationCount = $(PageLocationCount).children().filter('LocationCount');
    var o_PageCount = $(PageLocationCount).children().filter('PageCount');

    nRootLength = o_Hour.length;

    var sCategory;
    var sLocationCount;
    var sPagingCount;

    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {

            var Hour = setundefined((o_Hour[i].textContent || o_Hour[i].innerText || o_Hour[i].text));
            var LocationCount = setundefined((o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text));
            var PageCount = setundefined((o_PageCount[i].textContent || o_PageCount[i].innerText || o_PageCount[i].text));

            sCategory = "<category label='" + Hour + "' />" + sCategory;
            sLocationCount = "<set value='" + LocationCount + "' />" + sLocationCount;
            sPagingCount = "<set value='" + PageCount + "' />" + sPagingCount;

        }

        sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
        sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
        sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";

        MakePagingLocationChart(sCategory, sPagingCount, sLocationCount);
    }

    document.getElementById("divLoading").style.display = "none";
}

function DownloadTTSyncERRORReport(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    if (dsRoot != null) {
        var o_Excel = dsRoot.getElementsByTagName('Excel');
        var o_Filename = dsRoot.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divLoading").style.display = "none";
    }

    document.getElementById("divLoading").style.display = "none";
}

function DevicePageCount(deviceId, fromDate, toDate) {

    var sTblDeviceSummary = document.getElementById('tblDevicePageCount');
    row = document.createElement('tr');
    AddCell(row, "Device Id", 'siteOverview_TopLeft_Box', "", "", "center", "200px", "20px", "");

    var startDate = fromDate.split('/');
    var endDate = toDate.split('/');

    var StartDate = new Date(startDate[0], startDate[1], startDate[2]);
    var EndDate = new Date(endDate[0], endDate[1], endDate[2]);
    var dCount = 1;

    for (var d = StartDate; d <= EndDate; d.setDate(d.getDate() + 1)) {

        AddCell(row, "D" + dCount, 'siteOverview_TopLeft_Box', "", "", "center", "200px", "20px", "");
        dCount++;
    }

    sTblDeviceSummary.appendChild(row);
}

var g_EDeviceSummary;

function LoadDeviceSummary(dsRoot) {

    var DeviceSummary = $(dsRoot).find("Monitor");

    g_EDeviceSummary = DeviceSummary;

    var o_DeviceId = $(DeviceSummary).children().filter('DeviceId');
    var o_IRId = $(DeviceSummary).children().filter('IRId');
    var o_Version = $(DeviceSummary).children().filter('Version');
    var o_PagingCnt = $(DeviceSummary).children().filter('PagingCnt');
    var o_LocationCnt = $(DeviceSummary).children().filter('LocationCnt');
    var o_WiFiCnt = $(DeviceSummary).children().filter('WiFiCnt');
    var o_Name = $(DeviceSummary).children().filter('Name');
    var o_AStar = $(DeviceSummary).children().filter('AStar');
    var o_LBI = $(DeviceSummary).children().filter('LBI');
    var o_Min = $(DeviceSummary).children().filter('Min');
    var o_Max = $(DeviceSummary).children().filter('Max');
    var o_UpdatedOn = $(DeviceSummary).children().filter('UpdatedOn');
    var o_PagedOn = $(DeviceSummary).children().filter('PagedOn');
    var o_InServerIni = $(DeviceSummary).children().filter('InServerIni');
    var o_OperatingMode = $(DeviceSummary).children().filter('OperatingMode');
    var o_AvgRssi = $(DeviceSummary).children().filter('AvgRssi');
    var o_StarsSeen = $(DeviceSummary).children().filter('StarsSeen');
    var o_StarsSeenCnt = $(DeviceSummary).children().filter('StarsSeenCnt');
    var o_SuperSyncCnt = $(DeviceSummary).children().filter('SuperSyncCnt');
    var o_TriggerCnt = $(DeviceSummary).children().filter('TriggerCnt');
    var o_BeaconCollisionStarCount = $(DeviceSummary).children().filter('BeaconCollisionStarCount');
    var o_BeaconCollisionStars = $(DeviceSummary).children().filter('BeaconCollisionStars');
    var o_LastSeen = $(DeviceSummary).children().filter('LastSeen');

    var nRootLength = o_DeviceId.length;

    var sTblDeviceSummary = document.getElementById('tblDeviceSummaryTbody');

    sTblLen = sTblDeviceSummary.rows.length;
    clearTableRows(sTblDeviceSummary, sTblLen);

    if (nRootLength > 0) {

        $('#trDeviceHeader').show();
        $('#btnExportDevice').show();
        $('#tblDeviceSummary').css('width', '1800px');
        $("#tblDeviceConnectivityChartReport").show();

        //Header        
        var body;
        if (g_IsPaging == 0) {
            document.getElementById("tdBeaconCollisionStarCount").style.display = "";
            document.getElementById("tdBeaconCollisionStars").style.display = "";
        }
        else {
            document.getElementById("tdBeaconCollisionStarCount").style.display = "none";
            document.getElementById("tdBeaconCollisionStars").style.display = "none";
        }

        var strgraphPagingCnt = "";
        var strgraphLocationCnt = "";
        var strgraphStarsSeenCnt = "";
        var strgraphTriggerCnt = "";
        var color = "";
        var FolderName = "";
        var sno = 0;
        var dPart = "";
        var sDate = "";
        var IsGroup;
        var strStatus = "";

        FolderName = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteFolderName").value;

        IsStatus = $("#ctl00_ContentPlaceHolder1_hdnStatus").val();

        if (IsStatus == 0) {
            strStatus = " - - ";
        }
        else if (IsStatus == 1) {
            strStatus = "This is undefined monitor and also allow undefined monitor option disabled, So this will not enter into the network";
        }
        else if (IsStatus == 2) {
            strStatus = "Group Star is inactive/not visible to this monitor";
        }

        document.getElementById("SpnStatus").innerHTML = "<label>" + strStatus + "</label>";

        for (var i = 0; i <= nRootLength - 1; i++) {

            var DeviceId = setundefined((o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text));
            var IRId = setundefined((o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text));
            var Version = setundefined((o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text));
            var PagingCnt = setundefined((o_PagingCnt[i].textContent || o_PagingCnt[i].innerText || o_PagingCnt[i].text));
            var LocationCnt = setundefined((o_LocationCnt[i].textContent || o_LocationCnt[i].innerText || o_LocationCnt[i].text));
            var WiFiCnt = setundefined((o_WiFiCnt[i].textContent || o_WiFiCnt[i].innerText || o_WiFiCnt[i].text));
            var Name = setundefined((o_Name[i].textContent || o_Name[i].innerText || o_Name[i].text));
            var AStar = setundefined((o_AStar[i].textContent || o_AStar[i].innerText || o_AStar[i].text));
            var LBI = setundefined((o_LBI[i].textContent || o_LBI[i].innerText || o_LBI[i].text));
            var Min = setundefined((o_Min[i].textContent || o_Min[i].innerText || o_Min[i].text));
            var Max = setundefined((o_Max[i].textContent || o_Max[i].innerText || o_Max[i].text));
            var UpdatedOn = setundefined((o_UpdatedOn[i].textContent || o_UpdatedOn[i].innerText || o_UpdatedOn[i].text));
            var PagedOn = setundefined((o_PagedOn[i].textContent || o_PagedOn[i].innerText || o_PagedOn[i].text));
            var InServerIni = setundefined((o_InServerIni[i].textContent || o_InServerIni[i].innerText || o_InServerIni[i].text));
            var OperatingMode = setundefined((o_OperatingMode[i].textContent || o_OperatingMode[i].innerText || o_OperatingMode[i].text));
            var AvgRssi = setundefined((o_AvgRssi[i].textContent || o_AvgRssi[i].innerText || o_AvgRssi[i].text));
            var StarsSeen = setundefined((o_StarsSeen[i].textContent || o_StarsSeen[i].innerText || o_StarsSeen[i].text));
            var StarsSeenCnt = setundefined((o_StarsSeenCnt[i].textContent || o_StarsSeenCnt[i].innerText || o_StarsSeenCnt[i].text));
            var SuperSyncCnt = setundefined((o_SuperSyncCnt[i].textContent || o_SuperSyncCnt[i].innerText || o_SuperSyncCnt[i].text));
            var TriggerCnt = setundefined((o_TriggerCnt[i].textContent || o_TriggerCnt[i].innerText || o_TriggerCnt[i].text));
            var BeaconCollisionStarCount = setundefined((o_BeaconCollisionStarCount[i].textContent || o_BeaconCollisionStarCount[i].innerText || o_BeaconCollisionStarCount[i].text));
            var BeaconCollisionStars = setundefined((o_BeaconCollisionStars[i].textContent || o_BeaconCollisionStars[i].innerText || o_BeaconCollisionStars[i].text));
            var LastSeen = setundefined((o_LastSeen[i].textContent || o_LastSeen[i].innerText || o_LastSeen[i].text));

            var strName = "";
            if (Name != "")
                var strName = " - " + Name;

            if (InServerIni == "Undefined") {
                color = "Red";
            }

            document.getElementById("spnDeviceId").innerHTML = "<label style='color:#0B2321'>M: " + DeviceId + "</label> " + strName;
            document.getElementById("spnInServerINI").innerHTML = "<label style='color:" + color + "'>" + InServerIni + "</label>";
            document.getElementById("spnOperatingMode").innerHTML = "<label>" + OperatingMode + "</label>";
            var datFileName;

            if (UpdatedOn != "") {

                datFileName = UpdatedOn;

                var d1 = new Date(UpdatedOn);

                if (PagedOn != "") {
                    var d2 = new Date(PagedOn);

                    if (d2 > d1) {
                        datFileName = PagedOn;
                    }
                }

                dPart = datFileName.split(' ');
                sDate = dPart[1].split(':');
                var sSiteURL = "http://gmsdata.centrak.com/httplog/";
                var sRemotePathName = sSiteURL + FolderName + "/" + dPart[0] + "-" + sDate[0] + ".rar";
                sno = "<a>" + Number(i + 1) + "</a><img src='images/text-x-log.png' alt='Log Viewer' title='Log Viewer' style='cursor:pointer; width:16px' onclick=CallLogViewer('" + encodeURIComponent(sRemotePathName.replace("http://", "")) + "','" + DeviceId + "')>";
            }
            else {

                dPart = PagedOn.split(' ');
                sDate = dPart[1].split(':');
                var sSiteURL = "http://gmsdata.centrak.com/httplog/";
                var sRemotePathName = sSiteURL + FolderName + "/" + dPart[0] + "-" + sDate[0] + ".rar";
                sno = "<a>" + Number(i + 1) + "</a><img src='images/text-x-log.png' alt='Log Viewer' title='Log Viewer' style='cursor:pointer; width:16px' onclick=CallLogViewer('" + encodeURIComponent(sRemotePathName.replace("http://", "")) + "','" + DeviceId + "')>";
            }
            row = document.createElement('tr');
            AddCell(row, sno, 'tableData_cell_left', "", "", "right", "30px", "20px", "");
            AddCell(row, IRId, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, AStar, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, Version, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, PagingCnt, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, LocationCnt, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, TriggerCnt, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, WiFiCnt, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, SuperSyncCnt, 'tableData_cell', "", "", "right", "30px", "20px", "");

            if (g_IsPaging == 0) {
                if (BeaconCollisionStarCount > 0)
                    AddCell(row, "<label style='color:red;'>" + BeaconCollisionStarCount + "</label>", 'tableData_cell', "", "", "right", "30px", "20px", "");
                else
                    AddCell(row, "", 'tableData_cell', "", "", "right", "30px", "20px", "");

                if (BeaconCollisionStars == 0)
                    BeaconCollisionStars = "";

                AddCell(row, BeaconCollisionStars, 'tableData_cell', "", "", "right", "100px", "20px", "");
            }
            else {
                AddCell(row, "", 'tableData_cell_none', "", "", "right", "30px", "20px", "");
                AddCell(row, "", 'tableData_cell_none', "", "", "right", "30px", "20px", "");
            }

            AddCell(row, StarsSeenCnt, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, StarsSeen, 'tableData_cell_Min', "", "", "left", "100px", "20px", "");
            AddCell(row, UpdatedOn, 'tableData_cell', "", "", "left", "100px", "20px", "");
            AddCell(row, PagedOn, 'tableData_cell', "", "", "left", "100px", "20px", "");
            AddCell(row, LBI, 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, parseFloat(Min).toFixed(1), 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, parseFloat(Max).toFixed(1), 'tableData_cell', "", "", "right", "30px", "20px", "");
            AddCell(row, parseFloat(AvgRssi).toFixed(1), 'tableData_cell', "", "", "right", "30px", "20px", "");
            sTblDeviceSummary.appendChild(row);

            //Chart
            strgraphPagingCnt += "<set label='" + LastSeen + "' value='" + PagingCnt + "' tooltext='Paging Count-" + PagingCnt + "," + LastSeen + "' />";
            strgraphLocationCnt += "<set label='" + LastSeen + "' value='" + LocationCnt + "' tooltext='Location Count-" + LocationCnt + "," + LastSeen + "' />";
            strgraphStarsSeenCnt += "<set label='" + LastSeen + "' value='" + StarsSeenCnt + "' tooltext='Stars Seen Count-" + StarsSeenCnt + "," + LastSeen + "' />";
            strgraphTriggerCnt += "<set label='" + LastSeen + "' value='" + TriggerCnt + "' tooltext='Trigger Count-" + TriggerCnt + "," + LastSeen + "' />";

        }

        DatatablesLoadGraph("#tblDeviceSummary", [8], 0, "asc", 10);

        strgraphPagingCnt += "<trendlines><line startvalue='500' color='#1aaf5d' valueonright='1' tooltext='Above 500' displayvalue='Above 500' /></trendlines>";

        MakeHistogramDeviceDetailsChart(strgraphPagingCnt, "Hour", 'Paging Count', nRootLength * 20, 200, 'PagingCount');
        MakeHistogramDeviceDetailsChart(strgraphLocationCnt, "Hour", 'Location Count', nRootLength * 20, 200, 'LocationCount');
        MakeHistogramDeviceDetailsChart(strgraphTriggerCnt, "Hour", 'Trigger Count', nRootLength * 20, 200, 'TriggerCount');
    }
    else {
        $('#tblDeviceSummary').css('width', '1800px');
        $('#trDeviceHeader').show();
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTblDeviceSummary.appendChild(row);
    }

    document.getElementById("divLoading").style.display = "none";
}

function LoadConnectivityReport(dsRoot, IsConnFailed) {

    document.getElementById("divLoading").style.display = "";

    var sTbl = document.getElementById("tblDeviceConnectivityReport");

    if (GetBrowserType() == "isIE") {
        sTbl = document.getElementById('tblDeviceConnectivityReport');
    }
    else if (GetBrowserType() == "isFF") {
        sTbl = document.getElementById('tblDeviceConnectivityReport');
    }

    sTblLen = sTbl.rows.length;

    clearTableRows(sTbl, sTblLen);
    var dsRoot = inf_Obj.responseXML.documentElement;

    var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
    var o_IRId = dsRoot.getElementsByTagName('IRId');
    var o_Version = dsRoot.getElementsByTagName('Version');
    var o_PagingCnt = dsRoot.getElementsByTagName('PagingCnt');
    var o_TriggerCnt = dsRoot.getElementsByTagName('TriggerCnt');
    var o_RFLCount = dsRoot.getElementsByTagName('RFLCount');
    var o_Counts = dsRoot.getElementsByTagName('Counts');
    var o_Mins70 = dsRoot.getElementsByTagName('Mins70');
    var o_Mins120 = dsRoot.getElementsByTagName('Mins120');
    var o_UpdatedOn = dsRoot.getElementsByTagName('UpdatedOn');

    nRootLength = o_MonitorId.length;

    //Header
    row = document.createElement('tr');
    AddCell(row, "SNo", 'siteOverview_TopLeft_Box', "", "", "left", "50px", "40px", "");
    AddCell(row, "Device Id", 'siteOverview_Box', "", "", "left", "100px", "40px", "");
    AddCell(row, "IRId", 'siteOverview_Box', "", "", "left", "100px", "40px", "");
    AddCell(row, "Version", 'siteOverview_Box', "", "", "left", "150px", "40px", "");
    AddCell(row, "Page Count", 'siteOverview_Box', "", "", "left", "100px", "40px", "");
    AddCell(row, "RF.Location Count", 'siteOverview_Box', "", "", "left", "100px", "40px", "");
    AddCell(row, "Trigger Count", 'siteOverview_Box', "", "", "left", "100px", "40px", "");
    AddCell(row, "Monitor did not report more than 70 mins", 'siteOverview_Box', "", "", "left", "150px", "40px", "");
    AddCell(row, "Monitor did not report more than 120 mins", 'siteOverview_Box', "", "", "left", "150px", "40px", "");
    AddCell(row, "Updated On", 'siteOverview_Box', "", "", "left", "150px", "40px", "");

    sTbl.appendChild(row);

    var Cnt0 = 0;
    var Cnt70 = 0;
    var Cnt120 = 0;
    var XMLSetPageCnt = "";
    var XMLSetRFLCnt = "";
    var XMLSetTriggerCnt = "";
    var href = "";
    var dPart = "";
    var sDate = "";

    if (nRootLength > 0) {

        for (var i = 0; i < nRootLength; i++) {

            var MonitorId = setundefined((o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text));
            var IRId = setundefined((o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text));
            var Version = setundefined((o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text));
            var PagingCnt = setundefined((o_PagingCnt[i].textContent || o_PagingCnt[i].innerText || o_PagingCnt[i].text));
            var TriggerCnt = setundefined((o_TriggerCnt[i].textContent || o_TriggerCnt[i].innerText || o_TriggerCnt[i].text));
            var RFLCount = setundefined((o_RFLCount[i].textContent || o_RFLCount[i].innerText || o_RFLCount[i].text));
            var Counts = setundefined((o_Counts[i].textContent || o_Counts[i].innerText || o_Counts[i].text));
            var Mins70 = setundefined((o_Mins70[i].textContent || o_Mins70[i].innerText || o_Mins70[i].text));
            var Mins120 = setundefined((o_Mins120[i].textContent || o_Mins120[i].innerText || o_Mins120[i].text));
            var UpdatedOn = setundefined((o_UpdatedOn[i].textContent || o_UpdatedOn[i].innerText || o_UpdatedOn[i].text));

            var ClsCells = 'siteOverview_cell_Green';

            href = MonitorId;
            if (Mins70 > 1 || Mins120 > 1) {
                ClsCells = 'siteOverview_cell_Red';

                //Paging Count
                XMLSetPageCnt += "<set label='" + MonitorId + "' value='" + PagingCnt + "' />";
                //RF.L Count
                XMLSetRFLCnt += "<set label='" + MonitorId + "' value='" + RFLCount + "' />";
                //Trigger Count
                XMLSetTriggerCnt += "<set label='" + MonitorId + "' value='" + TriggerCnt + "' />";

                dPart = UpdatedOn.split('/');
                sDate = dPart[2] + "-" + dPart[1] + "-" + dPart[0];

                href = "<a href='GMSReportDetails.aspx?qSiteId=" + sSiteId + "&qFromDate=" + sDate + "&qDeviceId=" + MonitorId + "&qSiteName=" + g_SiteName + "&qispaging=1' title='View hourly log' class='DeviceDetailsLink_red' target='_blank' href=#devicedetails>" + MonitorId + "</a>";
            }

            // Reporting Count
            if (Mins70 > 1) {
                Cnt70++;
            }
            else if (Mins120 > 1) {
                Cnt120++;
            }
            else {
                Cnt0++;
            }

            row = document.createElement('tr');

            AddCell(row, i + 1, "tableData_cell_left", "", "", "right", "50px", "30px", "");
            AddCell(row, href, ClsCells, "", "", "right", "100px", "30px", "");
            AddCell(row, IRId, "tableData_cell", "", "", "right", "100px", "30px", "");
            AddCell(row, Version, "tableData_cell", "", "", "right", "150px", "30px", "");
            AddCell(row, PagingCnt, "tableData_cell", "", "", "right", "100px", "30px", "");
            AddCell(row, RFLCount, "tableData_cell", "", "", "right", "100px", "30px", "");
            AddCell(row, TriggerCnt, "tableData_cell", "", "", "right", "100px", "30px", "");
            AddCell(row, Mins70, ClsCells, "", "", "right", "150px", "30px", "");
            AddCell(row, Mins120, ClsCells, "", "", "right", "150px", "30px", "");
            AddCell(row, UpdatedOn, "tableData_cell", "", "", "left", "150px", "30px", "");

            sTbl.appendChild(row);
        }

        if (IsConnFailed == true) {
            var cnt;
            cnt = " : " + nRootLength;
            $('#Conn').text(cnt);
        }
        else {
            $('#Conn').text("");
        }
        //Draw Graph
        $("#tblDeviceConnectivityReportCount").show();

        //Load charts
        MakeConnectivityReport(Cnt0, Cnt70, Cnt120);
        MakeReportCountChart("divPageCntChat", "Page Count", XMLSetPageCnt, "line", "810", "190");
        MakeReportCountChart("divRFLCntChart", "RF.Location Count", XMLSetRFLCnt, "line", "810", "200");
        MakeReportCountChart("divTriggerChtChart", "Trigger Count", XMLSetTriggerCnt, "line", "810", "200");

    }
    else {

        $("#tblDeviceConnectivityReportCount").hide();

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 10, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
    }

    document.getElementById("divLoading").style.display = "none";

}

//Device connectivity report
function MakeConnectivityReport(Cnt0, Cnt70, Cnt120) {
    var chartcontent = "";
    var sXML;
    var strgraph;
    var myChartId = 'myChart';
    var PCnt0 = 0;
    var PCnt70 = 0;
    var PCnt120 = 0;
    var Tot = 0;

    Tot = Cnt0 + Cnt70 + Cnt120;
    PCnt0 = (Cnt0 / Tot) * 100;
    PCnt70 = (Cnt70 / Tot) * 100;
    PCnt120 = (Cnt120 / Tot) * 100;

    strgraph = "<dataset seriesname='0 Mins'><set label='" + Cnt0 + "' value='" + Cnt0 + "' displayValue='" + PCnt0.toFixed(2) + "%'/></dataset>" +
                "<dataset seriesname='> 70 Mins'><set label='" + Cnt70 + "' value='" + Cnt70 + "' displayValue='" + PCnt70.toFixed(2) + "%'/></dataset>" +
                "<dataset seriesname='> 120 Mins'><set label='" + Cnt120 + "' value='" + Cnt120 + "' displayValue='" + PCnt120.toFixed(2) + "%'/></dataset>";

    sXML = "<chart caption='Device Connectivity' palettecolors='#1aaf5d,#0075c2,#e82a41' bgcolor='#ffffff' showborder='0' showshadow='0' showcanvasborder='0' useplotgradientcolor='0' legendborderalpha='0' legendshadow='0' showXAxisValues='0' " +
            " showaxislines='0' showalternatehgridcolor='0' lineThickness='1' divlinethickness='0' divlinedashed='0' divlinedashlen='0' divlinegaplen='0' xaxisname='' showvalues='1' formatNumberScale='0' formatNumber='0' showLegend='1' showYAxisValues='1'> " +
            "<categories>" +
            "<category label='' />" +
            "</categories>";

    sXML += strgraph + "</chart>";

    //Dispose Charts
    var chart;
    if (FusionCharts(myChartId) == undefined) {
        chart = new FusionCharts("mscolumn2d", myChartId, "600", "200", "0", "0");
    }

    $("#divConnectivityReport").empty();
    FusionCharts(myChartId).setDataXML(sXML);
    FusionCharts(myChartId).render("divConnectivityReport");

}

function DownloadConnectivityReport(dsRoot) {
    document.getElementById("divLoading").style.display = "";

    if (dsRoot != null) {
        var o_Excel = dsRoot.getElementsByTagName('Excel');
        var o_Filename = dsRoot.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divLoading").style.display = "none";
    }

    document.getElementById("divLoading").style.display = "none";
}

function MakeReportCountChart(DivName, Title, XMLSet, ChartType, Width, Height) {
    var sXML;

    var myChartId = DivName + "_Chart";

    sXML = "<chart caption='" + Title + "' subcaption='' xaxisname='' yaxisname='' linethickness='2' formatNumber='0' formatNumberScale='0'" +
          " palettecolors='#008ee4,#6baa01' basefontcolor='#333333' basefont='Helvetica Neue,Arial' captionfontsize='14' subcaptionfontsize='14' subcaptionfontbold='0'" +
          " showborder='0' showYAxisValues='1' showLabels='0' showvalues='0' bgcolor='#ffffff' showshadow='0' canvasbgcolor='#ffffff' canvasborderalpha='0' divlinealpha='100' divlinecolor='#999999'" +
          " divlinethickness='0' divlineisdashed='0' divlinedashlen='0' divlinegaplen='0' showxaxisline='0' xaxislinethickness='1' xaxislinecolor='#999999' showalternatehgridcolor='0'>";


    sXML += XMLSet + "</chart>";

    //Dispose Charts
    var chart1;

    if (FusionCharts(myChartId) == undefined) {
        chart1 = new FusionCharts(ChartType, myChartId, Width, Height, "0", "0");
    }

    $("#" + DivName).empty();
    FusionCharts(myChartId).setDataXML(sXML);
    FusionCharts(myChartId).render(DivName);

}

var gMonitorRoot;

///MonitorAnalysisReport Paging Count
var Pagecallcount = 0;
var g_PagingVersion = "";
var g_CollisionVersion = "";

function LoadMonitorAnalysisPagingReport() {
    var sTbl = document.getElementById("tblMonitorAnalysisPagingReportTbody");
    sTbl = document.getElementById('tblMonitorAnalysisPagingReportTbody');
    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var tableheadContent = "<thead><tr><th class='siteOverview_TopLeft_Box' align='center' height='30px'>S.No</th>" +
                            "<th class='siteOverview_Box' height='30px'>Monitor&nbsp;Id</th>" +
                            "<th class='siteOverview_Box' height='30px'>IRID</th>" +
                            "<th class='siteOverview_Box' height='30px'>Locked Star</th>" +
                            "<th class='siteOverview_Box' height='30px'>Beacon Slot</th>" +
                            "<th class='siteOverview_Box' height='30px'>Version</th>" +
                            "<th class='siteOverview_Box' height='30px'>First Seen</th>" +
                            "<th class='siteOverview_Box' height='30px'>Last Seen</th>" +
                            "<th class='siteOverview_Box' height='30px'>P.Cnt</th>" +
                            "<th class='siteOverview_Box' height='30px'>L.Cnt</th>" +
                            "<th class='siteOverview_Box' height='30px'>Trigger Cnt</th>" +
                            "<th class='siteOverview_Box' height='30px'>Defined</th>" +
                            "<th class='siteOverview_Box' height='30px'>Star Seen Count</th>" +
                            "<th class='siteOverview_Box' height='30px'>Group</th></tr></thead><tbody id='tblMonitorAnalysisPagingReportTbody'></tbody>";

    var color = "";

    //Table Content 
    var DateHeader = $(g_dsRoot).find("Monitor");
    var o_Date = $(DateHeader).children().filter('Date');
    var o_DeviceId = $(DateHeader).children().filter('DeviceId');
    var o_IRID = $(DateHeader).children().filter('IRID');
    var o_LockedStarId = $(DateHeader).children().filter('LockedStarId');
    var o_Beaconslot = $(DateHeader).children().filter('Beaconslot');
    var o_Version = $(DateHeader).children().filter('Version');
    var o_FirstSeen = $(DateHeader).children().filter('FirstSeen');
    var o_LastSeen = $(DateHeader).children().filter('LastSeen');
    var o_PagingCount = $(DateHeader).children().filter('PagingCount');
    var o_LocationCount = $(DateHeader).children().filter('LocationCount');
    var o_TriggerCount = $(DateHeader).children().filter('TriggerCount');
    var o_InServerini = $(DateHeader).children().filter('InServerini');
    var o_InGroup = $(DateHeader).children().filter('InGroup');
    var o_StarSeenCount = $(DateHeader).children().filter('StarSeenCount');

    gMonitorRoot = g_dsRoot;

    nLen = o_DeviceId.length;
    var tcnt = 0;

    if (nLen > 0) {
        for (var j = 0; j < nLen; j++) {

            var PagingDate = setundefined((o_Date[0].textContent || o_Date[0].innerText || o_Date[0].text));
            var DeviceId = setundefined((o_DeviceId[j].textContent || o_DeviceId[j].innerText || o_DeviceId[j].text));
            var IRID = setundefined((o_IRID[j].textContent || o_IRID[j].innerText || o_IRID[j].text));
            var LockedStarId = setundefined((o_LockedStarId[j].textContent || o_LockedStarId[j].innerText || o_LockedStarId[j].text));
            var Beaconslot = setundefined((o_Beaconslot[j].textContent || o_Beaconslot[j].innerText || o_Beaconslot[j].text));
            var Version = setundefined((o_Version[j].textContent || o_Version[j].innerText || o_Version[j].text));
            var FirstSeen = setundefined((o_FirstSeen[j].textContent || o_FirstSeen[j].innerText || o_FirstSeen[j].text));
            var LastSeen = setundefined((o_LastSeen[j].textContent || o_LastSeen[j].innerText || o_LastSeen[j].text));
            var PagingCount = setundefined((o_PagingCount[j].textContent || o_PagingCount[j].innerText || o_PagingCount[j].text));
            var LocationCount = setundefined((o_LocationCount[j].textContent || o_LocationCount[j].innerText || o_LocationCount[j].text));
            var TriggerCount = setundefined((o_TriggerCount[j].textContent || o_TriggerCount[j].innerText || o_TriggerCount[j].text));
            var InServerini = setundefined((o_InServerini[j].textContent || o_InServerini[j].innerText || o_InServerini[j].text));
            var InGroup = setundefined((o_InGroup[j].textContent || o_InGroup[j].innerText || o_InGroup[j].text));
            var StarSeenCount = setundefined((o_StarSeenCount[j].textContent || o_StarSeenCount[j].innerText || o_StarSeenCount[j].text));

            if (g_PagingVersion == "" || Version == g_PagingVersion) {
                var strStatus = 0;
                if (InGroup == "Yes") {
                    sGroup = "<img style='cursor: pointer; width:18px;' id='imgGroup' alt='Group Monitors' title='Group Monitors' src='images/imgViewMore.png' onclick='OpenGroupMonitorDialog(" + DeviceId + ");' />";
                }
                else {
                    sGroup = "";
                }

                var sDefined;
                if (InServerini == "Defined") {
                    INIcolor = "Green";
                    sDefined = "Yes";
                }
                else {
                    INIcolor = "Red";
                    sDefined = "No";
                }

                var chartname = 'Paging' + DeviceId;

                if (Number(PagingCount) > 0 && Number(LocationCount) == 0 && InServerini == 'Undefined') {
                    strStatus = 1;
                }
                else if (Number(PagingCount) > 0 && Number(LocationCount) == 0 && InServerini == 'Defined' && InGroup == "Yes") {
                    strStatus = 2;
                }

                var href = "<a href='GMSReportDetails.aspx?qSiteId=" + sSiteId + "&qFromDate=" + PagingDate + "&qDeviceId=" + DeviceId + "&qSiteName=" + g_SiteName + "&qStatus=" + strStatus + "&qispaging=1' title='Hourly log data' class='DeviceDetailsLink' target='_blank' href=#devicedetails>" + DeviceId + "</a>";

                tableContent = "<tr>" +
                "<td class='tableData_cell_left' align='right' width='30px' height='20px'>" + Number(++tcnt) + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + href + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + IRID + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LockedStarId + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + Beaconslot + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + Version + "</td>" +
                "<td class='tableData_cell' align='left' width='150px' height='20px'>" + FirstSeen + "</td>" +
                "<td class='tableData_cell' align='left' width='150px' height='20px'>" + LastSeen + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + PagingCount + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LocationCount + "</td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px'>" + TriggerCount + "</td>" +
                "<td class='tableData_cell' align='left' width='60px' height='20px'><label style='color:" + INIcolor + "'>" + sDefined + "</label></td>" +
                "<td class='tableData_cell' align='right' width='60px' height='20px' style='color: #1592F2; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 12px; font-weight: bold; cursor: pointer;' onclick=OpenStarSlotDialog(" + DeviceId + "," + LockedStarId + "," + Beaconslot + ") >" + StarSeenCount + "</td>" +
                "<td class='tableData_cell' align='center' width='50px' height='20px'>" + sGroup + "</td>";

                tableContent += "</tr>";

                $('#tblMonitorAnalysisPagingReportTbody').append(tableContent);
            }
        }
        DatatablesLoadGraph("#tblMonitorAnalysisPagingReport", [8], 0, "asc", 10);
    }
    else {
        $('#tblMonitorAnalysisPagingReportTbody').append("<tr><td style='height:40px;' colspan='14' class='siteOverview_cell_Full' align='center'>No Record Found.</td></tr>");
    }
    if (tableheadContent == $('#tblMonitorAnalysisPagingReport').html()) {
        $('#tblMonitorAnalysisPagingReportTbody').append("<tr><td colspan='14' style='height:40px;' class='siteOverview_cell_Full' align='center'>No Record Found.</td></tr>");
    }
    $('#tblMonitorAnalysisPagingReport').show();
}

function OpenGroupMonitorDialog(DeviceId) {
    var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();

    var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
    inf_Obj = CreateTagXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxOpenGroupMonitorDialog;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetGMSMonitorGroupsInfoByDeviceId&SiteId=" + sSiteId + "&DeviceId=" + DeviceId + "&Date=" + sDate;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }

    document.getElementById("divLoading").style.display = "";
    return false;

}

function ajaxOpenGroupMonitorDialog(dsRoot) {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
            var o_IRId = dsRoot.getElementsByTagName('IRId');
            var o_StarId1 = dsRoot.getElementsByTagName('StarId1');
            var o_Slot = dsRoot.getElementsByTagName('Slot');
            var o_Version = dsRoot.getElementsByTagName('Version');
            var o_LastSeen = dsRoot.getElementsByTagName('LastSeen');
            var o_PagingCount = dsRoot.getElementsByTagName('PagingCount');
            var o_LocationCount = dsRoot.getElementsByTagName('LocationCount');
            var o_BeaconCollisionStars = dsRoot.getElementsByTagName('BeaconCollisionStars');
            var o_BeaconCollisionStarCount = dsRoot.getElementsByTagName('BeaconCollisionStarCount');
            var o_Master = dsRoot.getElementsByTagName('Master');
            var o_SelectedMonitorId = dsRoot.getElementsByTagName('SelectedMonitorId');
            var o_StarSeen = dsRoot.getElementsByTagName('StarSeen');
            var o_StarSlot = dsRoot.getElementsByTagName('StarSlot');
            var o_StarSeenCount = dsRoot.getElementsByTagName('StarSeenCount');

            nRootLength = o_MonitorId.length;

            //Header
            var tableheadContent = "";
            tableheadContent += "<thead><tr><th class='siteOverview_TopLeft_Box' align='center' width='30px' height='20px'>S.No</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Device Id</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>IRId</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Version</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>StarId1</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Becon Slot</th>" +
            "<th class='siteOverview_Box' width='150px' height='20px' style='min-width:135px;'>Last Seen</th>" +
            "<th class='siteOverview_Box' width='100px' height='20px'>Beacon Collision Count</th>" +
	       "<th class='siteOverview_Box' width='200px' height='20px'>Beacon Collision Star</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>L.Cnt</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>P.Cnt</th>" +
            "<th class='siteOverview_Box' width='300px' height='20px'>Star Seen</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Star Seen Count</th>" +
            "</tr></thead><tbody>";

            $('#tblGroupMonitors').empty();
            $('#tblGroupMonitors').append(tableheadContent);

            var selectedcolor = "";

            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {
                    var MonitorId = setundefined((o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text));
                    var IRId = setundefined((o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text));
                    var StarId1 = setundefined((o_StarId1[i].textContent || o_StarId1[i].innerText || o_StarId1[i].text));
                    var Version = setundefined((o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text));
                    var Slot = setundefined((o_Slot[i].textContent || o_Slot[i].innerText || o_Slot[i].text));
                    var LastSeen = setundefined((o_LastSeen[i].textContent || o_LastSeen[i].innerText || o_LastSeen[i].text));
                    var PagingCount = setundefined((o_PagingCount[i].textContent || o_PagingCount[i].innerText || o_PagingCount[i].text));
                    var LocationCount = setundefined((o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text));
                    var BeaconCollisionStars = setundefined((o_BeaconCollisionStars[i].textContent || o_BeaconCollisionStars[i].innerText || o_BeaconCollisionStars[i].text));
                    var BeaconCollisionStarCount = setundefined((o_BeaconCollisionStarCount[i].textContent || o_BeaconCollisionStarCount[i].innerText || o_BeaconCollisionStarCount[i].text));
                    var Master = setundefined((o_Master[i].textContent || o_Master[i].innerText || o_Master[i].text));
                    var SelectedMonitorId = setundefined((o_SelectedMonitorId[i].textContent || o_SelectedMonitorId[i].innerText || o_SelectedMonitorId[i].text));
                    var StarSeen = setundefined((o_StarSeen[i].textContent || o_StarSeen[i].innerText || o_StarSeen[i].text));
                    var StarSlot = setundefined((o_StarSlot[i].textContent || o_StarSlot[i].innerText || o_StarSlot[i].text));
                    var StarSeenCount = setundefined((o_StarSeenCount[i].textContent || o_StarSeenCount[i].innerText || o_StarSeenCount[i].text));

                    selectedcolor = "";

                    var arrBeaconStarSeen;
                    var BeaconStarSeen = "";

                    BeaconStarSeen = BreakSlotStarSeen(StarSeen, StarSlot, Slot);
                    arrBeaconStarSeen = BeaconStarSeen.split('-');
                    if (arrBeaconStarSeen.length > 1) {
                        StarSeen = arrBeaconStarSeen[0];
                        BeaconStarSeen = arrBeaconStarSeen[1];
                    }

                    if (SelectedMonitorId == "Yes")
                        selectedcolor = "#80df8e";
                    if (Master == "Yes")
                        selectedcolor = "#E3ECF5";

                    tableContent = "";
                    tableContent = "<tr>" +
                            "<td class='tableData_cell_left' align='center' width='30px' height='20px' style='background-color:" + selectedcolor + "'>" + Number(i + 1) + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "' onclick='OpenMonitorInfoDialog(" + MonitorId + ");' ><label class='DeviceDetailsLink'>" + MonitorId + "</label></td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + IRId + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + Version + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + StarId1 + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + Slot + "</td>" +
                            "<td class='tableData_cell' align='left' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + LastSeen + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + BeaconCollisionStarCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='text-align:left; word-break: break-word; background-color:" + selectedcolor + "'>" + BeaconCollisionStars + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + LocationCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + PagingCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='text-align:left; word-break: break-word; background-color:" + selectedcolor + "'>" + StarSeen + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + StarSeenCount + "</td>" +
                            "</tr>";

                    $('#tblGroupMonitors').append(tableContent);
                }
            }
            $('#tblGroupMonitors').append("</tbody>");

            //Open Dialog
            $("#dialog_GroupMonitor").dialog({
                height: 450,
                width: 1100,
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
    }
    document.getElementById("divLoading").style.display = "none";
}

var Collisioncallcount = 0;

//MonitorAnalysisReport Collision Count
function LoadMonitorAnalysisCollisionReport() {
    var sTbl = document.getElementById("tblMonitorAnalysisCollisionReportTbody");
    sTbl = document.getElementById('tblMonitorAnalysisCollisionReportTbody');
    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var tableheadContent = "<thead><tr><th class='siteOverview_TopLeft_Box' align='center' height='40px'>S.No</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Monitor&nbsp;Id</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>IRID</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Locked Star</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Beacon Slot</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Version</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>First Seen</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Last Seen</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Beacon Collision Cnt</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>P.Cnt</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>L.Cnt</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Trigger Cnt</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Defined</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Star Seen Count</th>" +
                               "<th class='siteOverview_Box' align='center' height='40px'>Group</th></tr></thead><tbody id='tblMonitorAnalysisCollisionReportTbody'></tbody>";

    var color = "";
    var INIcolor = "";

    //Table Content 
    var DateHeader = $(g_dsRoot).find("MonitorCollision");
    var o_Date = $(DateHeader).children().filter('Date');
    var o_DeviceId = $(DateHeader).children().filter('DeviceId');
    var o_IRID = $(DateHeader).children().filter('IRID');
    var o_LockedStarId = $(DateHeader).children().filter('LockedStarId');
    var o_Beaconslot = $(DateHeader).children().filter('Beaconslot');
    var o_Version = $(DateHeader).children().filter('Version');
    var o_FirstSeen = $(DateHeader).children().filter('FirstSeen');
    var o_LastSeen = $(DateHeader).children().filter('LastSeen');
    var o_BeaconCollisionStarCount = $(DateHeader).children().filter('BeaconCollisionStarCount');
    var o_LocationCount = $(DateHeader).children().filter('LocationCount');
    var o_PagingCount = $(DateHeader).children().filter('PagingCount');
    var o_TriggerCount = $(DateHeader).children().filter('TriggerCount');
    var o_InServerini = $(DateHeader).children().filter('InServerini');
    var o_InGroup = $(DateHeader).children().filter('InGroup');
    var o_StarSeenCount = $(DateHeader).children().filter('StarSeenCount');

    nLen = o_DeviceId.length;
    var tcnt = 0;

    if (nLen > 0) {
        for (var j = 0; j < nLen; j++) {
            var PagingDate = setundefined((o_Date[0].textContent || o_Date[0].innerText || o_Date[0].text));
            var DeviceId = setundefined((o_DeviceId[j].textContent || o_DeviceId[j].innerText || o_DeviceId[j].text));
            var IRID = setundefined((o_IRID[j].textContent || o_IRID[j].innerText || o_IRID[j].text));
            var LockedStarId = setundefined((o_LockedStarId[j].textContent || o_LockedStarId[j].innerText || o_LockedStarId[j].text));
            var Beaconslot = setundefined((o_Beaconslot[j].textContent || o_Beaconslot[j].innerText || o_Beaconslot[j].text));
            var Version = setundefined((o_Version[j].textContent || o_Version[j].innerText || o_Version[j].text));
            var FirstSeen = setundefined((o_FirstSeen[j].textContent || o_FirstSeen[j].innerText || o_FirstSeen[j].text));
            var LastSeen = setundefined((o_LastSeen[j].textContent || o_LastSeen[j].innerText || o_LastSeen[j].text));
            var BeaconCollisionStarCount = setundefined((o_BeaconCollisionStarCount[j].textContent || o_BeaconCollisionStarCount[j].innerText || o_BeaconCollisionStarCount[j].text));
            var LocationCount = setundefined((o_LocationCount[j].textContent || o_LocationCount[j].innerText || o_LocationCount[j].text));
            var PagingCount = setundefined((o_PagingCount[j].textContent || o_PagingCount[j].innerText || o_PagingCount[j].text));
            var TriggerCount = setundefined((o_TriggerCount[j].textContent || o_TriggerCount[j].innerText || o_TriggerCount[j].text));
            var InServerini = setundefined((o_InServerini[j].textContent || o_InServerini[j].innerText || o_InServerini[j].text));
            var InGroup = setundefined((o_InGroup[j].textContent || o_InGroup[j].innerText || o_InGroup[j].text));
            var StarSeenCount = setundefined((o_StarSeenCount[j].textContent || o_StarSeenCount[j].innerText || o_StarSeenCount[j].text));

            if (g_CollisionVersion == "" || Version == g_CollisionVersion) {
                var strStatus = 0;
                if (InGroup == "Yes")
                    sGroup = "<img style='cursor: pointer; width:18px;' id='imgGroup' alt='Group Monitors' title='Group Monitors' src='images/imgViewMore.png' onclick='OpenGroupMonitorDialog(" + DeviceId + ");' />";
                else
                    sGroup = "";

                var sDefined;
                if (InServerini == "Defined") {
                    INIcolor = "Green";
                    sDefined = "Yes";
                }
                else {
                    INIcolor = "Red";
                    sDefined = "No";
                }

                var chartname = 'Collision' + DeviceId;

                if (Number(PagingCount) > 0 && Number(LocationCount) == 0 && InServerini == 'Undefined') {
                    strStatus = 1;
                }
                else if (Number(PagingCount) > 0 && Number(LocationCount) == 0 && InServerini == 'Defined' && InGroup == "Yes") {
                    strStatus = 2;
                }

                var href = "<a href='GMSReportDetails.aspx?qSiteId=" + sSiteId + "&qFromDate=" + PagingDate + "&qDeviceId=" + DeviceId + "&qSiteName=" + g_SiteName + "&qStatus=" + strStatus + "&qispaging=0' title='Hourly log data' class='DeviceDetailsLink' target='_blank' href=#devicedetails>" + DeviceId + "</a>";

                tableContent = "<tr>" +
                            "<td class='tableData_cell_left' align='right' width='30px' height='20px'>" + Number(++tcnt) + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + href + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + IRID + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LockedStarId + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + Beaconslot + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + Version + "</td>" +
                            "<td class='tableData_cell' align='left' width='180px' height='20px'>" + FirstSeen + "</td>" +
                            "<td class='tableData_cell' align='left' width='180px' height='20px'>" + LastSeen + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px' style='color: #1592F2; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 12px; font-weight: bold; cursor: pointer;' onclick=OpenBeaconStarSlotDialog(" + DeviceId + "," + LockedStarId + ") >" + BeaconCollisionStarCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + PagingCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LocationCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'>" + TriggerCount + "</td>" +
                            "<td class='tableData_cell' align='right' width='60px' height='20px'><label style='color:" + INIcolor + "'>" + sDefined + "</label></td>" +
                            "<td class='tableData_cell' align='center' width='60px' height='20px' style='color: #1592F2; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 12px; font-weight: bold; cursor: pointer;' onclick=OpenStarSlotDialog(" + DeviceId + "," + LockedStarId + "," + Beaconslot + ") >" + StarSeenCount + "</td>" +
                            "<td class='tableData_cell' align='center' width='60px' height='20px'>" + sGroup + "</td>";

                tableContent += "</tr>";

                $('#tblMonitorAnalysisCollisionReportTbody').append(tableContent);
            }
        }
        DatatablesLoadGraph("#tblMonitorAnalysisCollisionReport", [8], 0, "asc", 10);
    }
    else {
        $('#tblMonitorAnalysisCollisionReportTbody').append("<tr><td colspan='15' style='height:40px;' class='siteOverview_cell_Full' align='center'>No Record Found.</td></tr>");
    }
    if (tableheadContent == $('#tblMonitorAnalysisCollisionReport').html()) {
        $('#tblMonitorAnalysisCollisionReportTbody').append("<tr><td colspan='15' style='height:40px;' class='siteOverview_cell_Full' align='center'>No Record Found.</td></tr>");
    }
    $('#tblMonitorAnalysisCollisionReport').show();
}


//Create Histogram Chart
function MakeHistogramDeviceDetailsChart(strgraph, xname, yname, width, height, id) {

    var chartcontent = "";
    var sXML;

    var myChartId = 'myChart' + id;

    sXML = "<chart palettecolors='#AAC94E' bgcolor='#ffffff' xAxisName='" + xname + "' yAxisName='" + yname + "' showborder='0' showshadow='0' showcanvasborder='0' useplotgradientcolor='0' legendborderalpha='0' legendshadow='0' " +
            " showaxislines='1'  showalternatehgridcolor='0' lineThickness='0' divlinethickness='0' divlinedashed='0' divlinedashlen='0' divlinegaplen='0' showvalues='0' formatNumberScale='0' formatNumber='0' showLegend='1' showYAxisValues='1' showXAxisValues='1'> ";

    sXML += strgraph + "</chart>";

    //Dispose Charts
    var chart;
    var chartwidth = width;
    if (chartwidth < 220) chartwidth = 220;

    if (FusionCharts(myChartId) == undefined) {
        chart = new FusionCharts("column2d", myChartId, chartwidth, height, "0", "0");
    }
    $('#Div' + id + '_MSLine2D').empty().width(480);
    FusionCharts(myChartId).setDataXML(sXML);
    FusionCharts(myChartId).render("Div" + id + "_MSLine2D");
}

//export report to csv file
function ExportMonitor() {

    document.getElementById("divLoading").style.display = "";

    var csvstringbuilder = [];
    var d = new Date();
    var year = d.getFullYear();
    var month = (d.getMonth() + 1);
    var day = d.getDate();
    var DownloadDate = year + '/' + month + '/' + day;
    var DateRange = "";
    var DeviceId;

    csvstringbuilder.push(CSVCell("Site Name : " + g_SiteName.replace(',', ''), false, true));
    csvstringbuilder.push(CSVNewLine());

    csvstringbuilder.push(CSVCell("Date : " + sDFromDate, false, true));
    csvstringbuilder.push(CSVNewLine());

    var o_DeviceId = $(g_EDeviceSummary).children().filter('DeviceId');
    var o_IRId = $(g_EDeviceSummary).children().filter('IRId');
    var o_Version = $(g_EDeviceSummary).children().filter('Version');
    var o_PagingCnt = $(g_EDeviceSummary).children().filter('PagingCnt');
    var o_LocationCnt = $(g_EDeviceSummary).children().filter('LocationCnt');
    var o_WiFiCnt = $(g_EDeviceSummary).children().filter('WiFiCnt');
    var o_Name = $(g_EDeviceSummary).children().filter('Name');
    var o_AStar = $(g_EDeviceSummary).children().filter('AStar');
    var o_LBI = $(g_EDeviceSummary).children().filter('LBI');
    var o_Min = $(g_EDeviceSummary).children().filter('Min');
    var o_Max = $(g_EDeviceSummary).children().filter('Max');
    var o_UpdatedOn = $(g_EDeviceSummary).children().filter('UpdatedOn');
    var o_PagedOn = $(g_EDeviceSummary).children().filter('PagedOn');
    var o_InServerIni = $(g_EDeviceSummary).children().filter('InServerIni');
    var o_OperatingMode = $(g_EDeviceSummary).children().filter('OperatingMode');
    var o_AvgRssi = $(g_EDeviceSummary).children().filter('AvgRssi');
    var o_StarsSeen = $(g_EDeviceSummary).children().filter('StarsSeen');
    var o_StarsSeenCnt = $(g_EDeviceSummary).children().filter('StarsSeenCnt');
    var o_SuperSyncCnt = $(g_EDeviceSummary).children().filter('SuperSyncCnt');
    var o_TriggerCnt = $(g_EDeviceSummary).children().filter('TriggerCnt');
    var o_BeaconCollisionStarCount = $(g_EDeviceSummary).children().filter('BeaconCollisionStarCount');
    var o_BeaconCollisionStars = $(g_EDeviceSummary).children().filter('BeaconCollisionStars');
    var o_LastSeen = $(g_EDeviceSummary).children().filter('LastSeen');

    var nRootLength = o_DeviceId.length;

    nRootLength = o_DeviceId.length;

    for (var i = 0; i < nRootLength; i++) {
        var DeviceId = setundefined((o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text));
        var IRId = setundefined((o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text));
        var Version = setundefined((o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text));
        var PagingCnt = setundefined((o_PagingCnt[i].textContent || o_PagingCnt[i].innerText || o_PagingCnt[i].text));
        var LocationCnt = setundefined((o_LocationCnt[i].textContent || o_LocationCnt[i].innerText || o_LocationCnt[i].text));
        var WiFiCnt = setundefined((o_WiFiCnt[i].textContent || o_WiFiCnt[i].innerText || o_WiFiCnt[i].text));
        var Name = setundefined((o_Name[i].textContent || o_Name[i].innerText || o_Name[i].text));
        var AStar = setundefined((o_AStar[i].textContent || o_AStar[i].innerText || o_AStar[i].text));
        var LBI = setundefined((o_LBI[i].textContent || o_LBI[i].innerText || o_LBI[i].text));
        var Min = setundefined((o_Min[i].textContent || o_Min[i].innerText || o_Min[i].text));
        var Max = setundefined((o_Max[i].textContent || o_Max[i].innerText || o_Max[i].text));
        var UpdatedOn = setundefined((o_UpdatedOn[i].textContent || o_UpdatedOn[i].innerText || o_UpdatedOn[i].text));
        var PagedOn = setundefined((o_PagedOn[i].textContent || o_PagedOn[i].innerText || o_PagedOn[i].text));
        var InServerIni = setundefined((o_InServerIni[i].textContent || o_InServerIni[i].innerText || o_InServerIni[i].text));
        var OperatingMode = setundefined((o_OperatingMode[i].textContent || o_OperatingMode[i].innerText || o_OperatingMode[i].text));
        var AvgRssi = setundefined((o_AvgRssi[i].textContent || o_AvgRssi[i].innerText || o_AvgRssi[i].text));
        var StarsSeen = setundefined((o_StarsSeen[i].textContent || o_StarsSeen[i].innerText || o_StarsSeen[i].text));
        var StarsSeenCnt = setundefined((o_StarsSeenCnt[i].textContent || o_StarsSeenCnt[i].innerText || o_StarsSeenCnt[i].text));
        var SuperSyncCnt = setundefined((o_SuperSyncCnt[i].textContent || o_SuperSyncCnt[i].innerText || o_SuperSyncCnt[i].text));
        var TriggerCnt = setundefined((o_TriggerCnt[i].textContent || o_TriggerCnt[i].innerText || o_TriggerCnt[i].text));
        var BeaconCollisionStarCount = setundefined((o_BeaconCollisionStarCount[i].textContent || o_BeaconCollisionStarCount[i].innerText || o_BeaconCollisionStarCount[i].text));
        var BeaconCollisionStars = setundefined((o_BeaconCollisionStars[i].textContent || o_BeaconCollisionStars[i].innerText || o_BeaconCollisionStars[i].text));
        var LastSeen = setundefined((o_LastSeen[i].textContent || o_LastSeen[i].innerText || o_LastSeen[i].text));

        if (Name == "")
            Name = "";

        if (i == 0) {
            csvstringbuilder.push(CSVCell("M: " + DeviceId + " - " + Name, false, true));
            csvstringbuilder.push(CSVNewLine());
            csvstringbuilder.push(CSVCell("InServer.Ini :" + InServerIni, false, true));
            csvstringbuilder.push(CSVNewLine());
            csvstringbuilder.push(CSVCell("Operating Mode :" + OperatingMode, false, true));
            csvstringbuilder.push(CSVNewLine());
            csvstringbuilder.push(CSVNewLine());

            csvstringbuilder.push(CSVCell("S.No", true));
            csvstringbuilder.push(CSVCell("IRId", true));
            csvstringbuilder.push(CSVCell("Locked StarId", true));
            csvstringbuilder.push(CSVCell("Version", true));
            csvstringbuilder.push(CSVCell("P.Cnt", true));
            csvstringbuilder.push(CSVCell("L.Cnt", true));
            csvstringbuilder.push(CSVCell("Trigger Count", true));
            csvstringbuilder.push(CSVCell("WiFi Count", true));
            csvstringbuilder.push(CSVCell("Super Sync Count", true));

            if (g_IsPaging == 0) {
                csvstringbuilder.push(CSVCell("Beacon Collision Star Count", true));
                csvstringbuilder.push(CSVCell("Beacon Collision Stars", true));
            }

            csvstringbuilder.push(CSVCell("Star Seen Count", true));
            csvstringbuilder.push(CSVCell("Star Seen", true));
            csvstringbuilder.push(CSVCell("Last Received Time", true));
            csvstringbuilder.push(CSVCell("Last Paged Time", true));
            csvstringbuilder.push(CSVCell("LBI Value", true));
            csvstringbuilder.push(CSVCell("Min Rssi", true));
            csvstringbuilder.push(CSVCell("Max Rssi", true));
            csvstringbuilder.push(CSVCell("Avg Rssi", false));
            csvstringbuilder.push(CSVNewLine());
        }

        csvstringbuilder.push(CSVCell(String(Number(i + 1)), true));
        csvstringbuilder.push(CSVCell(String(IRId), true));
        csvstringbuilder.push(CSVCell(String(AStar), true));
        csvstringbuilder.push(CSVCell(String(Version), true, true));
        csvstringbuilder.push(CSVCell(String(PagingCnt), true));
        csvstringbuilder.push(CSVCell(String(LocationCnt), true));
        csvstringbuilder.push(CSVCell(String(TriggerCnt), true));
        csvstringbuilder.push(CSVCell(String(setundefined(WiFiCnt)), true));
        csvstringbuilder.push(CSVCell(String(SuperSyncCnt), true));

        if (g_IsPaging == 0) {
            csvstringbuilder.push(CSVCell(String(BeaconCollisionStarCount), true));
            csvstringbuilder.push(CSVCell(String(BeaconCollisionStars), true, true));
        }

        csvstringbuilder.push(CSVCell(String(StarsSeenCnt), true));
        csvstringbuilder.push(CSVCell(String(StarsSeen), true, true));
        csvstringbuilder.push(CSVCell(String(UpdatedOn), true));
        csvstringbuilder.push(CSVCell(String(PagedOn), true));
        csvstringbuilder.push(CSVCell(String(LBI), true));
        csvstringbuilder.push(CSVCell(String(Min), true));
        csvstringbuilder.push(CSVCell(String(Max), true));
        csvstringbuilder.push(CSVCell(String(AvgRssi), false));
        csvstringbuilder.push(CSVNewLine());
    }

    //join array as string
    csvstringbuilder = csvstringbuilder.join("");

    //download csv
    tableToCSV(csvstringbuilder, DeviceId + "-Device-List-" + DownloadDate);
    document.getElementById("divLoading").style.display = "none";
}

//export report to csv file
function ExportPagingAnalysis(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    var csvstringbuilder = [];
    var d = new Date();
    var year = d.getFullYear();
    var month = (d.getMonth() + 1);
    var day = d.getDate();
    var DownloadDate = year + '/' + month + '/' + day;
    var DateRange = "";
    var Condition = "";
    var sCondition = "";

    csvstringbuilder.push(CSVCell("Site Name : " + g_SiteName.replace(',', ''), false, true));
    csvstringbuilder.push(CSVNewLine());

    csvstringbuilder.push(CSVCell("Date : " + $('#ctl00_ContentPlaceHolder1_txtFromDate').val(), false, true));
    csvstringbuilder.push(CSVNewLine());

    ////Collision Count Summary
    csvstringbuilder.push(CSVCell("Paging Analysis", false, true));
    csvstringbuilder.push(CSVNewLine());

    csvstringbuilder.push(CSVCell("S.No", true));
    csvstringbuilder.push(CSVCell("Monitor Id", true));
    csvstringbuilder.push(CSVCell("IRID", true));
    csvstringbuilder.push(CSVCell("LockedStarId", true));
    csvstringbuilder.push(CSVCell("Beacon Slot", true));
    csvstringbuilder.push(CSVCell("Version", true));
    csvstringbuilder.push(CSVCell("First Seen", true));
    csvstringbuilder.push(CSVCell("Last Seen", true));
    csvstringbuilder.push(CSVCell("P.Cnt", true));
    csvstringbuilder.push(CSVCell("L.Cnt", true));
    csvstringbuilder.push(CSVCell("Trigger Cnt", true));
    csvstringbuilder.push(CSVCell("Defined", true));
    csvstringbuilder.push(CSVCell("Star Seen Count", true));
    csvstringbuilder.push(CSVCell("Group", false));
    csvstringbuilder.push(CSVNewLine());

    var DateHeader = $(g_dsRoot).find("Monitor");
    var o_Date = $(DateHeader).children().filter('Date');
    var o_DeviceId = $(DateHeader).children().filter('DeviceId');
    var o_IRID = $(DateHeader).children().filter('IRID');
    var o_LockedStarId = $(DateHeader).children().filter('LockedStarId');
    var o_Beaconslot = $(DateHeader).children().filter('Beaconslot');
    var o_Version = $(DateHeader).children().filter('Version');
    var o_FirstSeen = $(DateHeader).children().filter('FirstSeen');
    var o_LastSeen = $(DateHeader).children().filter('LastSeen');
    var o_PagingCount = $(DateHeader).children().filter('PagingCount');
    var o_LocationCount = $(DateHeader).children().filter('LocationCount');
    var o_TriggerCount = $(DateHeader).children().filter('TriggerCount');
    var o_InServerini = $(DateHeader).children().filter('InServerini');
    var o_InGroup = $(DateHeader).children().filter('InGroup');
    var o_StarSeenCount = $(DateHeader).children().filter('StarSeenCount');

    var nLen = o_DeviceId.length;
    var tcnt = 0;

    if (nLen > 0) {
        for (var j = 0; j < nLen; j++) {
            var PagingDate = setundefined((o_Date[0].textContent || o_Date[0].innerText || o_Date[0].text));
            var DeviceId = setundefined((o_DeviceId[j].textContent || o_DeviceId[j].innerText || o_DeviceId[j].text));
            var IRID = setundefined((o_IRID[j].textContent || o_IRID[j].innerText || o_IRID[j].text));
            var LockedStarId = setundefined((o_LockedStarId[j].textContent || o_LockedStarId[j].innerText || o_LockedStarId[j].text));
            var Beaconslot = setundefined((o_Beaconslot[j].textContent || o_Beaconslot[j].innerText || o_Beaconslot[j].text));
            var Version = setundefined((o_Version[j].textContent || o_Version[j].innerText || o_Version[j].text));
            var FirstSeen = setundefined((o_FirstSeen[j].textContent || o_FirstSeen[j].innerText || o_FirstSeen[j].text));
            var LastSeen = setundefined((o_LastSeen[j].textContent || o_LastSeen[j].innerText || o_LastSeen[j].text));
            var PagingCount = setundefined((o_PagingCount[j].textContent || o_PagingCount[j].innerText || o_PagingCount[j].text));
            var LocationCount = setundefined((o_LocationCount[j].textContent || o_LocationCount[j].innerText || o_LocationCount[j].text));
            var TriggerCount = setundefined((o_TriggerCount[j].textContent || o_TriggerCount[j].innerText || o_TriggerCount[j].text));
            var InServerini = setundefined((o_InServerini[j].textContent || o_InServerini[j].innerText || o_InServerini[j].text));
            var InGroup = setundefined((o_InGroup[j].textContent || o_InGroup[j].innerText || o_InGroup[j].text));
            var StarSeenCount = setundefined((o_StarSeenCount[j].textContent || o_StarSeenCount[j].innerText || o_StarSeenCount[j].text));

            if (g_PagingVersion == "" || Version == g_PagingVersion) {
                var Defined;
                if (InServerini == "Defined") {
                    Defined = "Yes";
                }
                else {
                    Defined = "No";
                }
                csvstringbuilder.push(CSVCell(String(Number(++tcnt)), true));
                csvstringbuilder.push(CSVCell(String(DeviceId), true));
                csvstringbuilder.push(CSVCell(String(IRID), true, true));
                csvstringbuilder.push(CSVCell(String(LockedStarId), true));
                csvstringbuilder.push(CSVCell(String(Beaconslot), true));
                csvstringbuilder.push(CSVCell(String(Version), true));
                csvstringbuilder.push(CSVCell(String(FirstSeen), true));
                csvstringbuilder.push(CSVCell(String(LastSeen), true));
                csvstringbuilder.push(CSVCell(String(PagingCount), true));
                csvstringbuilder.push(CSVCell(String(LocationCount), true));
                csvstringbuilder.push(CSVCell(String(TriggerCount), true));
                csvstringbuilder.push(CSVCell(String(Defined), true));
                csvstringbuilder.push(CSVCell(String(StarSeenCount), true));
                csvstringbuilder.push(CSVCell(String(InGroup), true, false));
                csvstringbuilder.push(CSVNewLine());
            }
        }
    }

    //join array as string
    csvstringbuilder = csvstringbuilder.join("");

    //download csv
    tableToCSV(csvstringbuilder, "CenTrak-GMS-Paging-Analysis-" + DownloadDate);
    document.getElementById("divLoading").style.display = "none";
}

function ExportCollisionAnalysis(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    var csvstringbuilder = [];
    var d = new Date();
    var year = d.getFullYear();
    var month = (d.getMonth() + 1);
    var day = d.getDate();
    var DownloadDate = year + '/' + month + '/' + day;
    var DateRange = "";

    csvstringbuilder.push(CSVCell("Site Name : " + g_SiteName.replace(',', ''), false, true));
    csvstringbuilder.push(CSVNewLine());

    csvstringbuilder.push(CSVCell("Date : " + $('#ctl00_ContentPlaceHolder1_txtFromDate').val(), false, true));
    csvstringbuilder.push(CSVNewLine());

    ////Collision Count Summary
    csvstringbuilder.push(CSVCell("Beacon Collision Analysis", false, true));
    csvstringbuilder.push(CSVNewLine());

    csvstringbuilder.push(CSVCell("S.No", true));
    csvstringbuilder.push(CSVCell("Monitor Id", true));
    csvstringbuilder.push(CSVCell("IRID", true));
    csvstringbuilder.push(CSVCell("LockedStarId", true));
    csvstringbuilder.push(CSVCell("Beacon Slot", true));
    csvstringbuilder.push(CSVCell("Version", true));
    csvstringbuilder.push(CSVCell("First Seen", true));
    csvstringbuilder.push(CSVCell("Last Seen", true));
    csvstringbuilder.push(CSVCell("Beacon Collision Cnt", true));
    csvstringbuilder.push(CSVCell("P.Cnt", true));
    csvstringbuilder.push(CSVCell("L.Cnt", true));
    csvstringbuilder.push(CSVCell("Trigger Cnt", true));
    csvstringbuilder.push(CSVCell("Defined", true));
    csvstringbuilder.push(CSVCell("Star Seen Count", true));
    csvstringbuilder.push(CSVCell("Group", false));
    csvstringbuilder.push(CSVNewLine());

    var DateHeader = $(g_dsRoot).find("MonitorCollision");
    var o_Date = $(DateHeader).children().filter('Date');
    var o_DeviceId = $(DateHeader).children().filter('DeviceId');
    var o_IRID = $(DateHeader).children().filter('IRID');
    var o_LockedStarId = $(DateHeader).children().filter('LockedStarId');
    var o_Beaconslot = $(DateHeader).children().filter('Beaconslot');
    var o_Version = $(DateHeader).children().filter('Version');
    var o_FirstSeen = $(DateHeader).children().filter('FirstSeen');
    var o_LastSeen = $(DateHeader).children().filter('LastSeen');
    var o_BeaconCollisionStarCount = $(DateHeader).children().filter('BeaconCollisionStarCount');
    var o_LocationCount = $(DateHeader).children().filter('LocationCount');
    var o_PagingCount = $(DateHeader).children().filter('PagingCount');
    var o_TriggerCount = $(DateHeader).children().filter('TriggerCount');
    var o_InServerini = $(DateHeader).children().filter('InServerini');
    var o_InGroup = $(DateHeader).children().filter('InGroup');
    var o_StarSeenCount = $(DateHeader).children().filter('StarSeenCount');

    var nLen = o_DeviceId.length;
    var tcnt = 0;

    if (nLen > 0) {
        for (var j = 0; j < nLen; j++) {
            var PagingDate = setundefined((o_Date[0].textContent || o_Date[0].innerText || o_Date[0].text));
            var DeviceId = setundefined((o_DeviceId[j].textContent || o_DeviceId[j].innerText || o_DeviceId[j].text));
            var IRID = setundefined((o_IRID[j].textContent || o_IRID[j].innerText || o_IRID[j].text));
            var LockedStarId = setundefined((o_LockedStarId[j].textContent || o_LockedStarId[j].innerText || o_LockedStarId[j].text));
            var Beaconslot = setundefined((o_Beaconslot[j].textContent || o_Beaconslot[j].innerText || o_Beaconslot[j].text));
            var Version = setundefined((o_Version[j].textContent || o_Version[j].innerText || o_Version[j].text));
            var FirstSeen = setundefined((o_FirstSeen[j].textContent || o_FirstSeen[j].innerText || o_FirstSeen[j].text));
            var LastSeen = setundefined((o_LastSeen[j].textContent || o_LastSeen[j].innerText || o_LastSeen[j].text));
            var BeaconCollisionStarCount = setundefined((o_BeaconCollisionStarCount[j].textContent || o_BeaconCollisionStarCount[j].innerText || o_BeaconCollisionStarCount[j].text));
            var LocationCount = setundefined((o_LocationCount[j].textContent || o_LocationCount[j].innerText || o_LocationCount[j].text));
            var PagingCount = setundefined((o_PagingCount[j].textContent || o_PagingCount[j].innerText || o_PagingCount[j].text));
            var TriggerCount = setundefined((o_TriggerCount[j].textContent || o_TriggerCount[j].innerText || o_TriggerCount[j].text));
            var InServerini = setundefined((o_InServerini[j].textContent || o_InServerini[j].innerText || o_InServerini[j].text));
            var InGroup = setundefined((o_InGroup[j].textContent || o_InGroup[j].innerText || o_InGroup[j].text));
            var StarSeenCount = setundefined((o_StarSeenCount[j].textContent || o_StarSeenCount[j].innerText || o_StarSeenCount[j].text));

            if (g_CollisionVersion == "" || Version == g_CollisionVersion) {
                var Defined;
                if (InServerini == "Defined") {
                    Defined = "Yes";
                }
                else {
                    Defined = "No";
                }

                csvstringbuilder.push(CSVCell(String(Number(++tcnt)), true));
                csvstringbuilder.push(CSVCell(String(DeviceId), true));
                csvstringbuilder.push(CSVCell(String(IRID), true, true));
                csvstringbuilder.push(CSVCell(String(LockedStarId), true));
                csvstringbuilder.push(CSVCell(String(Beaconslot), true));
                csvstringbuilder.push(CSVCell(String(Version), true));
                csvstringbuilder.push(CSVCell(String(FirstSeen), true));
                csvstringbuilder.push(CSVCell(String(LastSeen), true));
                csvstringbuilder.push(CSVCell(String(BeaconCollisionStarCount), true));
                csvstringbuilder.push(CSVCell(String(PagingCount), true));
                csvstringbuilder.push(CSVCell(String(LocationCount), true));
                csvstringbuilder.push(CSVCell(String(TriggerCount), true));
                csvstringbuilder.push(CSVCell(String(Defined), true));
                csvstringbuilder.push(CSVCell(String(StarSeenCount), true));
                csvstringbuilder.push(CSVCell(String(InGroup), true, false));
                csvstringbuilder.push(CSVNewLine());
            }
        }
    }

    //join array as string
    csvstringbuilder = csvstringbuilder.join("");

    //download csv
    tableToCSV(csvstringbuilder, "CenTrak-GMS-Beacon-Collision-Analysis-" + DownloadDate);
    document.getElementById("divLoading").style.display = "none";
}

function OpenMonitorInfoDialog(DeviceId) {
    var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();

    var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();

    inf_Obj = CreateTagXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxOpenMonitorInfoDialog;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetDeviceMonitorHourlyInfo&SiteId=" + sSiteId + "&DeviceId=" + DeviceId;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    document.getElementById("divLoading").style.display = "";
    return false;

}

function ajaxOpenMonitorInfoDialog(dsRoot) {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
            var o_FirmwareVersion = dsRoot.getElementsByTagName('FirmwareVersion');
            var o_LastSeenTagId = dsRoot.getElementsByTagName('LastSeenTagId');
            var o_IRId = dsRoot.getElementsByTagName('IRId');
            var o_RSSI = dsRoot.getElementsByTagName('RSSI');
            var o_LBI = dsRoot.getElementsByTagName('LBI');
            var o_LBIValue = dsRoot.getElementsByTagName('LBIValue');
            var o_MinLBI = dsRoot.getElementsByTagName('MinLBI');
            var o_MaxLBI = dsRoot.getElementsByTagName('MaxLBI');
            var o_LBIDiff = dsRoot.getElementsByTagName('LBIDiff');
            var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
            var o_LastPagingTime = dsRoot.getElementsByTagName('LastPagingTime');
            var o_ReceivedTime = dsRoot.getElementsByTagName('ReceivedTime');
            var o_LocationDataReceived = dsRoot.getElementsByTagName('LocationDataReceived');
            var o_PageDataReceived = dsRoot.getElementsByTagName('PageDataReceived');
            var o_TriggerCount = dsRoot.getElementsByTagName('TriggerCount');
            var o_AvgRssi = dsRoot.getElementsByTagName('AvgRssi');
            var o_StarCount = dsRoot.getElementsByTagName('StarCount');
            var o_StarSeen = dsRoot.getElementsByTagName('StarSeen');
            var o_BeaconCollisionStarCount = dsRoot.getElementsByTagName('BeaconCollisionStarCount');

            nRootLength = o_DeviceId.length;

            //Header
            var tableheadContent = "";
            tableheadContent += "<thead><tr><th class='siteOverview_TopLeft_Box' width='60px' height='20px'>S.No</th>" +
            "<th class='siteOverview_Box' width='100px' height='20px'>Device Id</th>" +
            "<th class='siteOverview_Box' width='100px' height='20px'>Firmware Version</th>" +
            "<th class='siteOverview_Box' width='100px' height='20px'>Last Seen TagId</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>IRId</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>RSSI</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>LBI</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>LBI Value</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Min LBI</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Max LBI</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>LBI Diff</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Locked StarId</th>" +
            "<th class='siteOverview_Box' width='100px' height='20px' style='min-width: 135px;'>Last Paging Time</th>" +
            "<th class='siteOverview_Box' width='100px' height='20px' style='min-width: 135px;'>Received Time</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Location Data Received</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Page Data Received</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Trigger Count</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Avg Rssi</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Star Seen Count</th>" +
            "<th class='siteOverview_Box' width='2000px' height='20px'>StarSeen</th>" +
            "<th class='siteOverview_Box' width='60px' height='20px'>Beacon Collision Count</th>" +
            "</tr></thead><tbody>";

            $('#tblMonitorsInfo').empty();
            $('#tblMonitorsInfo').append(tableheadContent);

            var color = "";
            var selectedcolor = "";

            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {
                    var DeviceId = setundefined((o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text));
                    var FirmwareVersion = setundefined((o_FirmwareVersion[i].textContent || o_FirmwareVersion[i].innerText || o_FirmwareVersion[i].text));
                    var LastSeenTagId = setundefined((o_LastSeenTagId[i].textContent || o_LastSeenTagId[i].innerText || o_LastSeenTagId[i].text));
                    var IRId = setundefined((o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text));
                    var RSSI = setundefined((o_RSSI[i].textContent || o_RSSI[i].innerText || o_RSSI[i].text));
                    var LBI = setundefined((o_LBI[i].textContent || o_LBI[i].innerText || o_LBI[i].text));
                    var LBIValue = setundefined((o_LBIValue[i].textContent || o_LBIValue[i].innerText || o_LBIValue[i].text));
                    var MinLBI = setundefined((o_MinLBI[i].textContent || o_MinLBI[i].innerText || o_MinLBI[i].text));
                    var MaxLBI = setundefined((o_MaxLBI[i].textContent || o_MaxLBI[i].innerText || o_MaxLBI[i].text));
                    var LBIDiff = setundefined((o_LBIDiff[i].textContent || o_LBIDiff[i].innerText || o_LBIDiff[i].text));
                    var LockedStarId = setundefined((o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text));
                    var LastPagingTime = setundefined((o_LastPagingTime[i].textContent || o_LastPagingTime[i].innerText || o_LastPagingTime[i].text));
                    var ReceivedTime = setundefined((o_ReceivedTime[i].textContent || o_ReceivedTime[i].innerText || o_ReceivedTime[i].text));
                    var LocationDataReceived = setundefined((o_LocationDataReceived[i].textContent || o_LocationDataReceived[i].innerText || o_LocationDataReceived[i].text));
                    var PageDataReceived = setundefined((o_PageDataReceived[i].textContent || o_PageDataReceived[i].innerText || o_PageDataReceived[i].text));
                    var TriggerCount = setundefined((o_TriggerCount[i].textContent || o_TriggerCount[i].innerText || o_TriggerCount[i].text));
                    var AvgRssi = setundefined((o_AvgRssi[i].textContent || o_AvgRssi[i].innerText || o_AvgRssi[i].text));
                    var StarCount = setundefined((o_StarCount[i].textContent || o_StarCount[i].innerText || o_StarCount[i].text));
                    var StarSeen = setundefined((o_StarSeen[i].textContent || o_StarSeen[i].innerText || o_StarSeen[i].text));
                    var BeaconCollisionStarCount = setundefined((o_BeaconCollisionStarCount[i].textContent || o_BeaconCollisionStarCount[i].innerText || o_BeaconCollisionStarCount[i].text));

                    selectedcolor = "";
                    tableContent = "";

                    tableContent = "<tr><td class='tableData_cell_left' align='right' width='60px' height='20px'>" + Number(i + 1) + "</td>" +
                                    "<td class='tableData_cell' align='right' width='100px' height='20px'>" + DeviceId + "</td>" +
                                    "<td class='tableData_cell' align='right' width='100px' height='20px'>" + FirmwareVersion + "</td>" +
                                    "<td class='tableData_cell' align='right' width='100px' height='20px'>" + LastSeenTagId + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + IRId + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + parseFloat(RSSI).toFixed(1) + "</td>" +
                                    "<td class='tableData_cell' align='left' width='60px' height='20px'>" + LBI + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LBIValue + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + MinLBI + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + MaxLBI + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LBIDiff + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LockedStarId + "</td>" +
                                    "<td class='tableData_cell' align='left' width='100px' height='20px'>" + LastPagingTime + "</td>" +
                                    "<td class='tableData_cell' align='left' width='100px' height='20px'>" + ReceivedTime + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + LocationDataReceived + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + PageDataReceived + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + TriggerCount + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + parseFloat(AvgRssi).toFixed(1) + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + StarCount + "</td>" +
                                    "<td class='tableData_cell' style='text-align:left; word-break: break-word;' width='2000px' height='20px'>" + StarSeen + "</td>" +
                                    "<td class='tableData_cell' align='right' width='60px' height='20px'>" + BeaconCollisionStarCount + "</td>" +
                                    "</tr>";

                    $('#tblMonitorsInfo').append(tableContent);
                }
            }
            $('#tblMonitorsInfo').append("</tbody>");

            //Open Dialog
            $("#dialog_MonitorInfo").dialog({
                height: 450,
                width: 1400,
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
    }
    document.getElementById("divLoading").style.display = "none";
}

function BreakStarSeen(StarSeen, Cnt) {
    if (StarSeen != "") {
        var arrStarSeen = StarSeen.split(",");
        StarSeen = "";
        for (var i = 0; i < arrStarSeen.length; i++) {
            if (i % Cnt == 0 && i > 0) {
                StarSeen += "<br>" + arrStarSeen[i] + ",";
            }
            else {
                StarSeen += arrStarSeen[i] + ",";
            }
        }
        StarSeen = StarSeen.slice(0, -1);
    }
    return StarSeen;
}

function BreakSlotStarSeen(StarSeen, StarSlot, Slot) {
    if (StarSeen != "" && StarSlot != "") {
        var arrStarSeen = StarSeen.split(",");
        var arrStarSlot = StarSlot.split(",");
        var BeaconStarSeen = "";
        StarSeen = "";

        for (var i = 0; i < arrStarSeen.length; i++) {
            if (arrStarSlot[i] == Slot) {
                StarSeen += "<span style='color:#0b7bea;'>" + arrStarSeen[i] + "</span>,";
                BeaconStarSeen += arrStarSeen[i] + ",";
            }
            else {
                StarSeen += arrStarSeen[i] + ",";
            }
        }
        StarSeen = StarSeen.slice(0, -1);
        BeaconStarSeen = BeaconStarSeen.slice(0, -1);
    }
    return StarSeen + "-" + BeaconStarSeen;
}

//*********************************************************
//	Function Name	:	Last10hrdata
//	Input			:	SiteId,DeviceType,DeviceId
//	Description		:	ajax call Last10hrdata
//*********************************************************
var g_Star24HrObj;
function StarOnehrdata(SiteId, DeviceType, DeviceId, sFromDate, IsDownload, SiteName) {
    g_Star24HrObj = CreateDeviceXMLObj();
    g_IsDownload = IsDownload;
    if (g_Star24HrObj != null) {
        document.getElementById('divLoading').style.display = "";
        g_Star24HrObj.onreadystatechange = ajaxStarOnehrList;
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetDeviceMonitorHourlyInfo&SiteId=" + SiteId + "&DeviceType=" + DeviceType + "&DeviceId=" + DeviceId + "&Date=" + sFromDate + "&IsDownload=" + IsDownload + "&SiteName=" + SiteName;

        if (GetBrowserType() == "isIE") {
            g_Star24HrObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Star24HrObj.open("GET", DbConnectorPath, true);
        }
        g_Star24HrObj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajax10hrList
//	Input			:	none
//	Description		:	Load 10 Hr Data from ajax Response
//*********************************************************
function ajaxStarOnehrList() {
    if (g_Star24HrObj.readyState == 4) {
        if (g_Star24HrObj.status == 200) {

            var sTbl, sTblLen;

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Star24HrObj.responseXML.documentElement);
                        
            var dsRoot = g_Star24HrObj.responseXML.documentElement;



            if (g_IsDownload > 0) {
                DownloadStarOneHrReport(dsRoot);
            }
            else {

                if (GetBrowserType() == "isIE") {
                    sTbl = document.getElementById('tblStarOneHourDatabody');
                }
                else if (GetBrowserType() == "isFF") {
                    sTbl = document.getElementById('tblStarOneHourDatabody');
                }
                sTblLen = sTbl.rows.length;
                clearTableRows(sTbl, sTblLen);

                LoadStarOnehourData(dsRoot, sTbl);
            }

            document.getElementById('divLoading').style.display = "none";
        }
    }
}

//*********************************************************
//	Function Name	:	LoadStarOnehourData
//	Input			:	dsRoot,sTbl
//	Description		:	Load Star One Hr Data from ajax Response
//*********************************************************
function LoadStarOnehourData(dsRoot, sTbl) {

    var o_MacId = dsRoot.getElementsByTagName('MacId');
    var o_FirmwareVersion = dsRoot.getElementsByTagName('Firmwareversion');
    var o_IPAddress = dsRoot.getElementsByTagName('IPAddress');
    var o_UpdatedOn = dsRoot.getElementsByTagName('UpdatedOn');
    var o_ResCount = dsRoot.getElementsByTagName('ResCount');
    var o_Pagedatareceived = dsRoot.getElementsByTagName('Pagedatareceived');
    var o_PageDataCount = dsRoot.getElementsByTagName('PageDataCount');
    var o_Locationdatareceived = dsRoot.getElementsByTagName('Locationdatareceived');
    var o_LocationDataCount = dsRoot.getElementsByTagName('LocationDataCount');
    var o_TTSyncError = dsRoot.getElementsByTagName('TTSyncError');
    var o_ErrorCnt = dsRoot.getElementsByTagName('ErrorCnt');

    var nRootLength = o_MacId.length;
    var body;

    //Header
    var sCategory = "";
    var sLocationCount = "";
    var sPagingCount = "";

    document.getElementById('spnReportingHrs').innerHTML = nRootLength;

    //Datas
    if (nRootLength > 0) {

        for (var i = 0; i < nRootLength; i++) {
            var MacId = setundefined(o_MacId[i].textContent || o_MacId[i].innerText || o_MacId[i].text);
            var FirmwareVersion = setundefined(o_FirmwareVersion[i].textContent || o_FirmwareVersion[i].innerText || o_FirmwareVersion[i].text);
            var IPAddress = setundefined(o_IPAddress[i].textContent || o_IPAddress[i].innerText || o_IPAddress[i].text);
            var UpdatedOn = setundefined(o_UpdatedOn[i].textContent || o_UpdatedOn[i].innerText || o_UpdatedOn[i].text);
            var ResCount = setundefined(o_ResCount[i].textContent || o_ResCount[i].innerText || o_ResCount[i].text);
            var PageDataReceived = setundefined(o_Pagedatareceived[i].textContent || o_Pagedatareceived[i].innerText || o_Pagedatareceived[i].text);
            var PageDataCount = setundefined(o_PageDataCount[i].textContent || o_PageDataCount[i].innerText || o_PageDataCount[i].text);
            var LocationDataReceived = setundefined(o_Locationdatareceived[i].textContent || o_Locationdatareceived[i].innerText || o_Locationdatareceived[i].text);
            var LocationDataCount = setundefined(o_LocationDataCount[i].textContent || o_LocationDataCount[i].innerText || o_LocationDataCount[i].text);
            var TTSyncError = setundefined(o_TTSyncError[i].textContent || o_TTSyncError[i].innerText || o_TTSyncError[i].text);
            var ErrorCnt = setundefined(o_ErrorCnt[i].textContent || o_ErrorCnt[i].innerText || o_ErrorCnt[i].text);

            row = document.createElement('tr');
            AddCell(row, i + 1, 'tableData_cell_left', "", "", "right", "", "20px", "");
            AddCell(row, FirmwareVersion, 'tableData_cell', "", "", "left", "", "20px", "");
            AddCell(row, MacId, 'tableData_cell', "", "", "left", "", "20x", "");
            AddCell(row, IPAddress, 'tableData_cell', "", "", "left", "", "20px", "");
            AddCell(row, UpdatedOn, 'tableData_cell', "", "", "left", "", "20px", "");
            AddCell(row, ResCount, 'tableData_cell', "", "", "right", "", "20px", "");
            AddCell(row, PageDataReceived, 'tableData_cell', "", "", "right", "", "20px", "");
            AddCell(row, PageDataCount, 'tableData_cell', "", "", "right", "", "20px", "");
            AddCell(row, LocationDataCount, 'tableData_cell', "", "", "right", "", "20px", "");
            AddCell(row, LocationDataReceived, 'tableData_cell', "", "", "right", "", "20px", "");
            AddCell(row, TTSyncError, 'tableData_cell', "", "", "right", "", "20px", "");
            AddCell(row, ErrorCnt, 'tableData_cell', "", "", "right", "", "20px", "");

            var dDate = new Date(UpdatedOn);
            sCategory = "<category label='" + dDate.getHours() + "' />" + sCategory;
            sLocationCount = "<set value='" + LocationDataCount + "' />" + sLocationCount;
            sPagingCount = "<set value='" + PageDataReceived + "' />" + sPagingCount;

            sTbl.appendChild(row);
        }
        DatatablesLoadGraph("#tblStarOneHourData", [8], 0, "asc", 10);

        sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
        sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
        sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";

        MakePagingLocationChart(sCategory, sPagingCount, sLocationCount);

    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No records found...", 'siteOverview_cell_Full', 16, "", "left", "100px", "40px", "");
        sTbl.appendChild(row);
    }

    $("#trDeviceHeader").show();
    $("#trStarHourlyInfoExport").show();
    $("#trStarHourlyInfo").show();
}

var g_LockedStarId = "";
var g_StarSlot = "";
function OpenStarSlotDialog(DeviceId, LockedStarId, Slot) {
    var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();
    var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
    g_LockedStarId = LockedStarId;
    g_StarSlot = Slot;
    inf_Obj = CreateTagXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxOpenStarSlotDialog;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetGMSStarSlotDetails&SiteId=" + sSiteId + "&DeviceId=" + DeviceId + "&Date=" + sDate;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }

    document.getElementById("divLoading").style.display = "";
    return false;

}

function ajaxOpenStarSlotDialog(StarSeen) {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            document.getElementById("divLoading").style.display = "";


            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_StarId = dsRoot.getElementsByTagName('StarId');
            var o_Slot = dsRoot.getElementsByTagName('Slot');
            var o_MacId = dsRoot.getElementsByTagName('MacId');
            var o_StarType = dsRoot.getElementsByTagName('StarType');
            var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
            var o_LockedSlot = dsRoot.getElementsByTagName('LockedSlot');

            var sSiteName = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").text();
            var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
            var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();

            nRootLength = o_StarId.length;

            //Header
            var tableheadContent = "";
            var selectedcolor = "";

            tableheadContent += "<thead><tr><th class='siteOverview_TopLeft_Box' align='center' width='60px' height='20px'>S.No</th>" +
            "<th class='siteOverview_Box' align='center' width='149px' height='20px' style='min-width:149px'>Star Seen</th>" +
            "<th class='siteOverview_Box' align='center' width='149px' height='20px' style='min-width:149px'>Beacon Slot</th>" +
            "</tr></thead><tbody>";
            $('#tblStarSeen').empty();
            $('#tblStarSeen').append(tableheadContent);

            var LockedStarId = g_LockedStarId;
            var sSlot = g_StarSlot;
            var tStarCnt = 0;
            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {
                    var StarId = setundefined((o_StarId[i].textContent || o_StarId[i].innerText || o_StarId[i].text));
                    var Slot = setundefined((o_Slot[i].textContent || o_Slot[i].innerText || o_Slot[i].text));
                    var MacId = setundefined((o_MacId[i].textContent || o_MacId[i].innerText || o_MacId[i].text));
                    var StarType = setundefined((o_StarType[i].textContent || o_StarType[i].innerText || o_StarType[i].text));
                    LockedStarId = setundefined((o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text));
                    sSlot = setundefined((o_LockedSlot[i].textContent || o_LockedSlot[i].innerText || o_LockedSlot[i].text));

                    tableContent = "";
                    selectedcolor = ""
                    if (sSlot == Slot) {
                        tStarCnt++;
                        selectedcolor = "#80df8e";
                    }
                    if (StarId == LockedStarId) selectedcolor = "#E3ECF5";

                    var StarIdLink = "<a style='cursor:pointer; text-decoration:none; color: #1592F2;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + sSiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>";

                    tableContent = "<tr>" +

                            "<td class='tableData_cell_left' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + Number(i + 1) + "</td>" +
                            "<td class='tableData_cell' width='120px' height='20px' style='background-color:" + selectedcolor + "'>" + StarIdLink + "</td>" +
                            "<td class='tableData_cell' width='120px' height='20px' style='background-color:" + selectedcolor + "'>" + Slot + "</td>" +
                            "</tr>";

                    $('#tblStarSeen').append(tableContent);
                }
            }
            $('#lblSlotStatus').text('');
            if (tStarCnt == 0) {
                $('#lblSlotStatus').text('Group star not visible');
            }
            else if (tStarCnt == 1) {
                $('#lblSlotStatus').text('No beacon collision');
            }
            else if (tStarCnt > 1) {
                $('#lblSlotStatus').text('Beacon collision');
            }

            $('#tblStarSeen').append("</tbody>");

            //Open Dialog
            $("#dialog_StarSlot").dialog({
                height: 450,
                width: 400,
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
    }
    document.getElementById("divLoading").style.display = "none";
}

var g_LockedBeaconStarId = "";
function OpenBeaconStarSlotDialog(DeviceId, LockedStarId) {
    var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();
    var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
    g_LockedBeaconStarId = LockedStarId;

    inf_Obj = CreateTagXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxOpenBeaconStarSlotDialog;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetGMSBeaconStarSlotDetails&SiteId=" + sSiteId + "&DeviceId=" + DeviceId + "&Date=" + sDate;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }

    document.getElementById("divLoading").style.display = "";
    return false;

}

function ajaxOpenBeaconStarSlotDialog(StarSeen) {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            document.getElementById("divLoading").style.display = "";

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_StarId = dsRoot.getElementsByTagName('StarId');
            var o_Slot = dsRoot.getElementsByTagName('Slot');
            var o_MacId = dsRoot.getElementsByTagName('MacId');
            var o_StarType = dsRoot.getElementsByTagName('StarType');
            var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');

            var sSiteName = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").text();
            var sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
            var sDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();

            nRootLength = o_StarId.length;

            //Header
            var tableheadContent = "";
            var selectedcolor = "";

            tableheadContent += "<thead><tr><th class='siteOverview_TopLeft_Box' align='center' width='60px' height='20px'>S.No</th>" +
            "<th class='siteOverview_Box' align='center' width='149px' height='20px' style='min-width:149px'>Beacon Star</th>" +
            "<th class='siteOverview_Box' align='center' width='149px' height='20px' style='min-width:149px'>Beacon Slot</th>" +
            "</tr></thead><tbody>";

            $('#tblBeaconStar').empty();
            $('#tblBeaconStar').append(tableheadContent);

            var LockedStarId = g_LockedBeaconStarId;
            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {
                    var StarId = setundefined((o_StarId[i].textContent || o_StarId[i].innerText || o_StarId[i].text));
                    var Slot = setundefined((o_Slot[i].textContent || o_Slot[i].innerText || o_Slot[i].text));
                    var MacId = setundefined((o_MacId[i].textContent || o_MacId[i].innerText || o_MacId[i].text));
                    var StarType = setundefined((o_StarType[i].textContent || o_StarType[i].innerText || o_StarType[i].text));
                    LockedStarId = setundefined((o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text));

                    tableContent = "";
                    selectedcolor = ""
                    if (StarId == LockedStarId) selectedcolor = "#E3ECF5";

                    var StarIdLink = "<a style='cursor:pointer; text-decoration:none; color: #1592F2;' class='DeviceDetailsLink' href='StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qSiteName=" + sSiteName + "&qMacId=" + MacId + "&qDate=" + sDate + "&qStarId=" + StarId + "&qStarType=" + StarType + "' title='Star Hourly Information' target='_blank'>" + StarId + "</a>";

                    tableContent = "<tr>" +

                            "<td class='tableData_cell_left' width='60px' height='20px' style='background-color:" + selectedcolor + "'>" + Number(i + 1) + "</td>" +
                            "<td class='tableData_cell' width='120px' height='20px' style='background-color:" + selectedcolor + "'>" + StarIdLink + "</td>" +
                            "<td class='tableData_cell' width='120px' height='20px' style='background-color:" + selectedcolor + "'>" + Slot + "</td>" +
                            "</tr>";

                    $('#tblBeaconStar').append(tableContent);
                }
            }
            $('#tblBeaconStar').append("</tbody>");

            //Open Dialog
            $("#dialog_BeaconStarSlot").dialog({
                height: 350,
                width: 400,
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
    }
    document.getElementById("divLoading").style.display = "none";
}

//*********************************************************
//	Function Name	:	MakePagingLocationChart
//	Input			:	sCategory, sPagingCount, sLocationCount
//	Description		:	Make Chart from XML String
//*********************************************************
function MakePagingLocationChart(sCategory, sPagingCount, sLocationCount) {

    var sXML;
    var strGraphData;

    strGraphData = sCategory + "<dataset>" + sPagingCount + sLocationCount + "</dataset>";

    sXML = "<chart palette='1' plotBorderThickness ='1' SYAxisMaxValue='' baseFontSize='10' " +
           "caption='' rotateLabels='1' rotateValues='1' showSum='0'  xaxisname='Hours' Yaxisname='' " +
           "numdivlines='3' useRoundEdges='1' legendBorderAlpha='0' connectNullData='1' " +
           "bgColor='FFFFFF' lineColor='F90327' showValues='2' seriesNameInToolTip='1' showBorder='0'" +
           " showLabels='1' anchorRadius='2' outCnvBaseFontSize='10' animation='1' " +
           "showAlternateHGridColor='1' AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' " +
           "alternateHGridAlpha='5' canvasBorderColor='666666' baseFontColor='000000' lineThickness='2' " +
           "formatNumberScale='0' formatNumber='0'>" + strGraphData + "</chart>";


    FusionCharts.setCurrentRenderer('javascript');
    var chart = new FusionCharts({
        "type": "msstackedcolumn2d",
        "renderAt": "Div_MSStackedColumn2D",
        "width": "950",
        "height": "250"
    });

    chart.setDataXML(sXML);
    chart.render("Div_PageLocationCount");

}

function PagingTotalSummary() {
    //Monitor Version Count
    var sTblSummary = document.getElementById("tblMonitorVersionSummary");

    if (GetBrowserType() == "isIE") {
        sTblSummary = document.getElementById('tblMonitorVersionSummary');
    }
    else if (GetBrowserType() == "isFF") {
        sTblSummary = document.getElementById('tblMonitorVersionSummary');
    }

    var sTblLenSummary = sTblSummary.rows.length;

    clearTableRows(sTblSummary, sTblLenSummary);

    var MonitorSummary = $(g_dsRoot).find("MonitorPagingVersionSummary");

    var o_UniqueVersion = $(MonitorSummary).children().filter('UniqueVersion');
    var o_Count = $(MonitorSummary).children().filter('Count');

    nRootLength = o_UniqueVersion.length;

    row = document.createElement('tr');
    AddCell(row, "Version", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "#", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    sTblSummary.appendChild(row);

    var strUniqueVersion = "";
    var totalcnt = 0;
    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {

            var UniqueVersion = setundefined((o_UniqueVersion[i].textContent || o_UniqueVersion[i].innerText || o_UniqueVersion[i].text));
            var Count = setundefined((o_Count[i].textContent || o_Count[i].innerText || o_Count[i].text));
            strUniqueVersion = "<span style='cursor:pointer;text-decoration:underline' onclick=PagingVersionBasedData(this," + UniqueVersion + ") >" + UniqueVersion + "</span>";

            row = document.createElement('tr');
            AddCell(row, strUniqueVersion, "Summary_cell", "", "", "right", "60px", "20px", "");
            AddCell(row, Count, "Summary_cell", "", "", "right", "60px", "20px", "");
            sTblSummary.appendChild(row);
            totalcnt += parseInt(Count);
            if (i == nRootLength - 1) {
                strUniqueVersion = "";
                row = document.createElement('tr');
                AddCell(row, strUniqueVersion, "Summary_cell", "", "", "right", "60px", "20px", "");
                AddCell(row, totalcnt, "Summary_cell", "", "", "right", "60px", "20px", "");
                sTblSummary.appendChild(row);
            }
        }

    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 2, "", "center", "700px", "40px", "");
        sTblSummary.appendChild(row);
    }

    // Monitor Summary table
    var sTblSummary = document.getElementById("tblMonitorSummary");

    if (GetBrowserType() == "isIE") {
        sTblSummary = document.getElementById('tblMonitorSummary');
    }
    else if (GetBrowserType() == "isFF") {
        sTblSummary = document.getElementById('tblMonitorSummary');
    }

    var sTblLenSummary = sTblSummary.rows.length;

    clearTableRows(sTblSummary, sTblLenSummary);

    var MonitorSummary = $(g_dsRoot).find("List");

    var o_Date = $(MonitorSummary).children().filter('Date');
    var o_MonitorsCountAbove500Paging = $(MonitorSummary).children().filter('MonitorsCountAbove500Paging');
    var o_UndefinedMonitorsSeen = $(MonitorSummary).children().filter('UndefinedMonitorsSeen');
    var o_TotalNumberofMonitorsConfiguredinIni = $(MonitorSummary).children().filter('TotalNumberofMonitorsConfiguredinIni');
    var o_TotalNumberofMonitorsConfiguredinGroups = $(MonitorSummary).children().filter('TotalNumberofMonitorsConfiguredinGroups');

    var o_PageUndefinedMonitorsSeen = $(MonitorSummary).children().filter('PagingUndefinedMonitorsSeen');
    var o_PageTotalNumberofMonitorsConfiguredinIni = $(MonitorSummary).children().filter('PagingTotalNumberofMonitorsConfiguredinIni');
    var o_PageTotalNumberofMonitorsConfiguredinGroups = $(MonitorSummary).children().filter('PagingTotalNumberofMonitorsConfiguredinGroups');

    var o_BCUndefinedMonitorsSeen = $(MonitorSummary).children().filter('BCUndefinedMonitorsSeen');
    var o_BCTotalNumberofMonitorsConfiguredinIni = $(MonitorSummary).children().filter('BCTotalNumberofMonitorsConfiguredinIni');
    var o_BCTotalNumberofMonitorsConfiguredinGroups = $(MonitorSummary).children().filter('BCTotalNumberofMonitorsConfiguredinGroups');

    var o_TotalMonitorsActivelyReporting = $(MonitorSummary).children().filter('TotalMonitorsActivelyReporting');
    var o_MonitorsCountinBeaconCollision = $(MonitorSummary).children().filter('MonitorsCountinBeaconCollision');


    nRootLength = o_UniqueVersion.length;

    row = document.createElement('tr');
    AddCell(row, "Monitor Summary", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "All", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "Paging", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "Beacon collision", "Summary_header_cell", "", "", "right", "60px", "20px", "");

    sTblSummary.appendChild(row);

    if (nRootLength > 0) {
        //for (var i = 0; i < nRootLength; i++) {
        var i = 0;
        var Date = setundefined((o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text));
        var MonitorsCountAbove500Paging = setundefined((o_MonitorsCountAbove500Paging[i].textContent || o_MonitorsCountAbove500Paging[i].innerText || o_MonitorsCountAbove500Paging[i].text));

        var UndefinedMonitorsSeen = setundefined((o_UndefinedMonitorsSeen[i].textContent || o_UndefinedMonitorsSeen[i].innerText || o_UndefinedMonitorsSeen[i].text));
        var TotalNumberofMonitorsConfiguredinIni = setundefined((o_TotalNumberofMonitorsConfiguredinIni[i].textContent || o_TotalNumberofMonitorsConfiguredinIni[i].innerText || o_TotalNumberofMonitorsConfiguredinIni[i].text));
        var TotalNumberofMonitorsConfiguredinGroups = setundefined((o_TotalNumberofMonitorsConfiguredinGroups[i].textContent || o_TotalNumberofMonitorsConfiguredinGroups[i].innerText || o_TotalNumberofMonitorsConfiguredinGroups[i].text));

        var PageUndefinedMonitorsSeen = setundefined((o_PageUndefinedMonitorsSeen[i].textContent || o_PageUndefinedMonitorsSeen[i].innerText || o_PageUndefinedMonitorsSeen[i].text));
        var PageTotalNumberofMonitorsConfiguredinIni = setundefined((o_PageTotalNumberofMonitorsConfiguredinIni[i].textContent || o_PageTotalNumberofMonitorsConfiguredinIni[i].innerText || o_PageTotalNumberofMonitorsConfiguredinIni[i].text));
        var PageTotalNumberofMonitorsConfiguredinGroups = setundefined((o_PageTotalNumberofMonitorsConfiguredinGroups[i].textContent || o_PageTotalNumberofMonitorsConfiguredinGroups[i].innerText || o_PageTotalNumberofMonitorsConfiguredinGroups[i].text));

        var BCUndefinedMonitorsSeen = setundefined((o_BCUndefinedMonitorsSeen[i].textContent || o_BCUndefinedMonitorsSeen[i].innerText || o_BCUndefinedMonitorsSeen[i].text));
        var BCTotalNumberofMonitorsConfiguredinIni = setundefined((o_BCTotalNumberofMonitorsConfiguredinIni[i].textContent || o_BCTotalNumberofMonitorsConfiguredinIni[i].innerText || o_BCTotalNumberofMonitorsConfiguredinIni[i].text));
        var BCTotalNumberofMonitorsConfiguredinGroups = setundefined((o_BCTotalNumberofMonitorsConfiguredinGroups[i].textContent || o_BCTotalNumberofMonitorsConfiguredinGroups[i].innerText || o_BCTotalNumberofMonitorsConfiguredinGroups[i].text));

        var TotalMonitorsActivelyReporting = setundefined((o_TotalMonitorsActivelyReporting[i].textContent || o_TotalMonitorsActivelyReporting[i].innerText || o_TotalMonitorsActivelyReporting[i].text));
        var MonitorsCountinBeaconCollision = setundefined((o_MonitorsCountinBeaconCollision[i].textContent || o_MonitorsCountinBeaconCollision[i].innerText || o_MonitorsCountinBeaconCollision[i].text));


        row = document.createElement('tr');
        AddCell(row, "Total Monitors", "Summary_cell", "", "", "left", "60px", "20px", "25px");
        AddCell(row, TotalMonitorsActivelyReporting, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountAbove500Paging, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountinBeaconCollision, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='PagingDefined' style='vertical-align: middle;' onchange='MonitorDefined(this)'>Defined in INI", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, TotalNumberofMonitorsConfiguredinIni, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, PageTotalNumberofMonitorsConfiguredinIni, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, BCTotalNumberofMonitorsConfiguredinIni, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='PagingUndefined' style='vertical-align: middle;' onchange='MonitorUnDefined(this)'>Undefined", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, UndefinedMonitorsSeen, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, PageUndefinedMonitorsSeen, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, BCUndefinedMonitorsSeen, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='PagingInGroups' style='vertical-align: middle;' onchange='MonitorInGroups(this)'>Monitors in Groups", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, TotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, PageTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, BCTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='PagingNotInGroups' style='vertical-align: middle;' onchange='MonitorNotInGroups(this)'>Monitors Not in Groups", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, TotalMonitorsActivelyReporting - TotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountAbove500Paging - PageTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountinBeaconCollision - BCTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        // }

    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 4, "", "center", "700px", "40px", "");
        sTblSummary.appendChild(row);
    }
}

function CollisionTotalSummary() {

    //Monitor Version Count
    var sTblSummary = document.getElementById("tblMonitorCollisionVersionSummary");

    if (GetBrowserType() == "isIE") {
        sTblSummary = document.getElementById('tblMonitorCollisionVersionSummary');
    }
    else if (GetBrowserType() == "isFF") {
        sTblSummary = document.getElementById('tblMonitorCollisionVersionSummary');
    }

    var sTblLenSummary = sTblSummary.rows.length;

    clearTableRows(sTblSummary, sTblLenSummary);

    var MonitorSummary = $(g_dsRoot).find("MonitorCollisionVersionSummary");

    var o_UniqueVersion = $(MonitorSummary).children().filter('UniqueVersion');
    var o_Count = $(MonitorSummary).children().filter('Count');

    nRootLength = o_UniqueVersion.length;

    row = document.createElement('tr');
    AddCell(row, "Version", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "#", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    sTblSummary.appendChild(row);

    var strUniqueVersion = "";
    var totalcnt = 0;
    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {

            var UniqueVersion = setundefined((o_UniqueVersion[i].textContent || o_UniqueVersion[i].innerText || o_UniqueVersion[i].text));
            var Count = setundefined((o_Count[i].textContent || o_Count[i].innerText || o_Count[i].text));
            strUniqueVersion = "<span style='cursor:pointer;text-decoration:underline' onclick=CollisionPagingVersionBasedData(this," + UniqueVersion + ") >" + UniqueVersion + "</span>";

            row = document.createElement('tr');
            AddCell(row, strUniqueVersion, "Summary_cell", "", "", "right", "60px", "20px", "");
            AddCell(row, Count, "Summary_cell", "", "", "right", "60px", "20px", "");
            sTblSummary.appendChild(row);
            totalcnt += parseInt(Count);
            if (i == nRootLength - 1) {
                strUniqueVersion = "";
                row = document.createElement('tr');
                AddCell(row, strUniqueVersion, "Summary_cell", "", "", "right", "60px", "20px", "");
                AddCell(row, totalcnt, "Summary_cell", "", "", "right", "60px", "20px", "");
                sTblSummary.appendChild(row);
            }
        }

    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 2, "", "center", "700px", "40px", "");
        sTblSummary.appendChild(row);
    }

    // Monitor Summary table
    var sTblSummary = document.getElementById("tblMonitorCollisionSummary");

    if (GetBrowserType() == "isIE") {
        sTblSummary = document.getElementById('tblMonitorCollisionSummary');
    }
    else if (GetBrowserType() == "isFF") {
        sTblSummary = document.getElementById('tblMonitorCollisionSummary');
    }

    var sTblLenSummary = sTblSummary.rows.length;

    clearTableRows(sTblSummary, sTblLenSummary);

    var MonitorSummary = $(g_dsRoot).find("List");

    var o_Date = $(MonitorSummary).children().filter('Date');
    var o_MonitorsCountAbove500Paging = $(MonitorSummary).children().filter('MonitorsCountAbove500Paging');
    var o_UndefinedMonitorsSeen = $(MonitorSummary).children().filter('UndefinedMonitorsSeen');
    var o_TotalNumberofMonitorsConfiguredinIni = $(MonitorSummary).children().filter('TotalNumberofMonitorsConfiguredinIni');
    var o_TotalNumberofMonitorsConfiguredinGroups = $(MonitorSummary).children().filter('TotalNumberofMonitorsConfiguredinGroups');

    var o_PageUndefinedMonitorsSeen = $(MonitorSummary).children().filter('PagingUndefinedMonitorsSeen');
    var o_PageTotalNumberofMonitorsConfiguredinIni = $(MonitorSummary).children().filter('PagingTotalNumberofMonitorsConfiguredinIni');
    var o_PageTotalNumberofMonitorsConfiguredinGroups = $(MonitorSummary).children().filter('PagingTotalNumberofMonitorsConfiguredinGroups');

    var o_BCUndefinedMonitorsSeen = $(MonitorSummary).children().filter('BCUndefinedMonitorsSeen');
    var o_BCTotalNumberofMonitorsConfiguredinIni = $(MonitorSummary).children().filter('BCTotalNumberofMonitorsConfiguredinIni');
    var o_BCTotalNumberofMonitorsConfiguredinGroups = $(MonitorSummary).children().filter('BCTotalNumberofMonitorsConfiguredinGroups');

    var o_TotalMonitorsActivelyReporting = $(MonitorSummary).children().filter('TotalMonitorsActivelyReporting');
    var o_MonitorsCountinBeaconCollision = $(MonitorSummary).children().filter('MonitorsCountinBeaconCollision');


    nRootLength = o_UniqueVersion.length;

    row = document.createElement('tr');
    AddCell(row, "Monitor Summary", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "All", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "Paging", "Summary_header_cell", "", "", "right", "60px", "20px", "");
    AddCell(row, "Beacon collision", "Summary_header_cell", "", "", "right", "60px", "20px", "");

    sTblSummary.appendChild(row);

    if (nRootLength > 0) {
        var i = 0;
        var Date = setundefined((o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text));
        var MonitorsCountAbove500Paging = setundefined((o_MonitorsCountAbove500Paging[i].textContent || o_MonitorsCountAbove500Paging[i].innerText || o_MonitorsCountAbove500Paging[i].text));

        var UndefinedMonitorsSeen = setundefined((o_UndefinedMonitorsSeen[i].textContent || o_UndefinedMonitorsSeen[i].innerText || o_UndefinedMonitorsSeen[i].text));
        var TotalNumberofMonitorsConfiguredinIni = setundefined((o_TotalNumberofMonitorsConfiguredinIni[i].textContent || o_TotalNumberofMonitorsConfiguredinIni[i].innerText || o_TotalNumberofMonitorsConfiguredinIni[i].text));
        var TotalNumberofMonitorsConfiguredinGroups = setundefined((o_TotalNumberofMonitorsConfiguredinGroups[i].textContent || o_TotalNumberofMonitorsConfiguredinGroups[i].innerText || o_TotalNumberofMonitorsConfiguredinGroups[i].text));

        var PageUndefinedMonitorsSeen = setundefined((o_PageUndefinedMonitorsSeen[i].textContent || o_PageUndefinedMonitorsSeen[i].innerText || o_PageUndefinedMonitorsSeen[i].text));
        var PageTotalNumberofMonitorsConfiguredinIni = setundefined((o_PageTotalNumberofMonitorsConfiguredinIni[i].textContent || o_PageTotalNumberofMonitorsConfiguredinIni[i].innerText || o_PageTotalNumberofMonitorsConfiguredinIni[i].text));
        var PageTotalNumberofMonitorsConfiguredinGroups = setundefined((o_PageTotalNumberofMonitorsConfiguredinGroups[i].textContent || o_PageTotalNumberofMonitorsConfiguredinGroups[i].innerText || o_PageTotalNumberofMonitorsConfiguredinGroups[i].text));

        var BCUndefinedMonitorsSeen = setundefined((o_BCUndefinedMonitorsSeen[i].textContent || o_BCUndefinedMonitorsSeen[i].innerText || o_BCUndefinedMonitorsSeen[i].text));
        var BCTotalNumberofMonitorsConfiguredinIni = setundefined((o_BCTotalNumberofMonitorsConfiguredinIni[i].textContent || o_BCTotalNumberofMonitorsConfiguredinIni[i].innerText || o_BCTotalNumberofMonitorsConfiguredinIni[i].text));
        var BCTotalNumberofMonitorsConfiguredinGroups = setundefined((o_BCTotalNumberofMonitorsConfiguredinGroups[i].textContent || o_BCTotalNumberofMonitorsConfiguredinGroups[i].innerText || o_BCTotalNumberofMonitorsConfiguredinGroups[i].text));

        var TotalMonitorsActivelyReporting = setundefined((o_TotalMonitorsActivelyReporting[i].textContent || o_TotalMonitorsActivelyReporting[i].innerText || o_TotalMonitorsActivelyReporting[i].text));
        var MonitorsCountinBeaconCollision = setundefined((o_MonitorsCountinBeaconCollision[i].textContent || o_MonitorsCountinBeaconCollision[i].innerText || o_MonitorsCountinBeaconCollision[i].text));


        row = document.createElement('tr');
        AddCell(row, "Total Monitors", "Summary_cell", "", "", "left", "60px", "20px", "25px");
        AddCell(row, TotalMonitorsActivelyReporting, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountAbove500Paging, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountinBeaconCollision, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='BeaconDefined' style='vertical-align: middle;' onchange='MonitorDefined(this)'>Defined in INI", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, TotalNumberofMonitorsConfiguredinIni, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, PageTotalNumberofMonitorsConfiguredinIni, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, BCTotalNumberofMonitorsConfiguredinIni, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='BeaconUndefined' style='vertical-align: middle;' onchange='MonitorUnDefined(this)'>Undefined", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, UndefinedMonitorsSeen, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, PageUndefinedMonitorsSeen, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, BCUndefinedMonitorsSeen, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='BeaconInGroups' style='vertical-align: middle;' onchange='MonitorInGroups(this)'>Monitors in Groups", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, TotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, PageTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, BCTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<input type='checkbox' id='BeaconNotInGroups' style='vertical-align: middle;' onchange='MonitorNotInGroups(this)'>Monitors Not in Groups", "Summary_cell", "", "", "left", "60px", "20px", "");
        AddCell(row, TotalMonitorsActivelyReporting - TotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountAbove500Paging - PageTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        AddCell(row, MonitorsCountinBeaconCollision - BCTotalNumberofMonitorsConfiguredinGroups, "Summary_cell", "", "", "right", "60px", "20px", "");
        sTblSummary.appendChild(row);
    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 4, "", "center", "700px", "40px", "");
        sTblSummary.appendChild(row);
    }
}

function PagingVersionBasedData(id, Version) {
    $("#ctl00_ContentPlaceHolder1_hdnPagingVersionMonitor").val(Version);
    g_PagingVersion = Version;
    $('#tblMonitorVersionSummary span').css('color', '#454545').css('font-weight', 'normal').css('text-decoration', 'underline');
    $(id).css('color', '#1592F2').css('font-weight', 'bold').css('text-decoration', 'underline');
    LoadMonitorAnalysisPagingReport();
}

function CollisionPagingVersionBasedData(id, Version) {
    $("#ctl00_ContentPlaceHolder1_hdnCollisionVersionMonitor").val(Version);
    g_CollisionVersion = Version;
    $('#tblMonitorCollisionVersionSummary span').css('color', '#454545').css('font-weight', 'normal').css('text-decoration', 'underline');
    $(id).css('color', '#1592F2').css('font-weight', 'bold').css('text-decoration', 'underline');
    LoadMonitorAnalysisCollisionReport();
}

function MonitorDefined(id) {
    if ($(id).prop("checked")) {
        $('#PagingDefined').prop('checked', true);
        $('#BeaconDefined').prop('checked', true);
    }
    else {
        $('#PagingDefined').prop('checked', false);
        $('#BeaconDefined').prop('checked', false);
    }
}

function MonitorUnDefined(id) {
    if ($(id).prop("checked")) {
        $('#PagingUndefined').prop('checked', true);
        $('#BeaconUndefined').prop('checked', true);
    }
    else {
        $('#PagingUndefined').prop('checked', false);
        $('#BeaconUndefined').prop('checked', false);
    }
}

function MonitorInGroups(id) {
    if ($(id).prop("checked")) {
        $('#PagingInGroups').prop('checked', true);
        $('#BeaconInGroups').prop('checked', true);
    }
    else {
        $('#PagingInGroups').prop('checked', false);
        $('#BeaconInGroups').prop('checked', false);
    }
}

function MonitorNotInGroups(id) {
    if ($(id).prop("checked")) {
        $('#PagingNotInGroups').prop('checked', true);
        $('#BeaconNotInGroups').prop('checked', true);
    }
    else {
        $('#PagingNotInGroups').prop('checked', false);
        $('#BeaconNotInGroups').prop('checked', false);
    }
}

function CheckingFilter() {
    var sGroupCond = g_sGroupCond;
    var arrsGroupCond = sGroupCond.split(',');
    if (arrsGroupCond.length == 4) {
        if (arrsGroupCond[0] == "1") {
            $('#PagingDefined').prop('checked', true);
            $('#BeaconDefined').prop('checked', true);
        }
        else {
            $('#PagingDefined').prop('checked', false);
            $('#BeaconDefined').prop('checked', false);
        }
        if (arrsGroupCond[1] == "1") {
            $('#PagingUndefined').prop('checked', true);
            $('#BeaconUndefined').prop('checked', true);
        }
        else {
            $('#PagingUndefined').prop('checked', false);
            $('#BeaconUndefined').prop('checked', false);
        }
        if (arrsGroupCond[2] == "1") {
            $('#PagingInGroups').prop('checked', true);
            $('#BeaconInGroups').prop('checked', true);
        }
        else {
            $('#PagingInGroups').prop('checked', false);
            $('#BeaconInGroups').prop('checked', false);
        }
        if (arrsGroupCond[3] == "1") {
            $('#PagingNotInGroups').prop('checked', true);
            $('#BeaconNotInGroups').prop('checked', true);
        }
        else {
            $('#PagingNotInGroups').prop('checked', false);
            $('#BeaconNotInGroups').prop('checked', false);
        }
    }
}

function ClearCheckingFilter() {
    g_sGroupCond = "";
    $('#PagingDefined').prop('checked', false);
    $('#BeaconDefined').prop('checked', false);
    $('#PagingUndefined').prop('checked', false);
    $('#BeaconUndefined').prop('checked', false);
    $('#PagingInGroups').prop('checked', false);
    $('#BeaconInGroups').prop('checked', false);
    $('#PagingNotInGroups').prop('checked', false);
    $('#BeaconNotInGroups').prop('checked', false);
}

//*********************************************************
//	Function Name	:	Star Daily Paging Location Data
//	Input			:	SiteId, FromDate
//	Description		:	ajax call Star Daily Paging Location Data
//*********************************************************
var g_StarDailyObj;
function StarDailydata(SiteId, sDate) {
    g_StarDailyObj = CreateDeviceXMLObj();

    if (g_StarDailyObj != null) {
        document.getElementById('divLoading').style.display = "";
        g_StarDailyObj.onreadystatechange = ajaxStarDailydata;
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetStarDailyPLCount&SiteId=" + SiteId + "&Date=" + sDate;

        if (GetBrowserType() == "isIE") {
            g_StarDailyObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_StarDailyObj.open("GET", DbConnectorPath, true);
        }
        g_StarDailyObj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxStarDailydata
//	Input			:	none
//	Description		:	Star Daily Paging Location Data
//*********************************************************
function ajaxStarDailydata() {
    if (g_StarDailyObj.readyState == 4) {
        if (g_StarDailyObj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_StarDailyObj.responseXML.documentElement);

            var dsRoot = g_StarDailyObj.responseXML.documentElement;

            var o_Date = dsRoot.getElementsByTagName('Date');
            var o_LocationCount = dsRoot.getElementsByTagName('LocationCount');
            var o_PageCount = dsRoot.getElementsByTagName('PageCount');

            nRootLength = o_Date.length;

            var sCategory = "";
            var sLocationCount = "";
            var sPagingCount = "";

            if (nRootLength > 0) {
                for (var i = 0; i < nRootLength; i++) {

                    var Date = setundefined((o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text));
                    var LocationCount = setundefined((o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text));
                    var PageCount = setundefined((o_PageCount[i].textContent || o_PageCount[i].innerText || o_PageCount[i].text));

                    sCategory = "<category label='" + Date + "' />" + sCategory;
                    sLocationCount = "<set value='" + LocationCount + "' />" + sLocationCount;
                    sPagingCount = "<set value='" + PageCount + "' />" + sPagingCount;

                }
            }

            sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
            sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
            sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";

            MakeStarPagingLocationChart(sCategory, sPagingCount, sLocationCount);

            document.getElementById('divLoading').style.display = "none";
        }
    }
}

//*********************************************************
//	Function Name	:	MakeStarPagingLocationChart
//	Input			:	sCategory, sPagingCount, sLocationCount
//	Description		:	Make Chart from XML String
//*********************************************************
function MakeStarPagingLocationChart(sCategory, sPagingCount, sLocationCount) {

    var sXML;
    var strGraphData;

    strGraphData = sCategory + "<dataset>" + sPagingCount + sLocationCount + "</dataset>";

    sXML = "<chart palette='1' plotBorderThickness ='1' SYAxisMaxValue='' baseFontSize='10' " +
           "caption='' rotateLabels='1' rotateValues='1' showSum='0'  xaxisname='Date' Yaxisname='' " +
           "numdivlines='3' useRoundEdges='1' legendBorderAlpha='0' connectNullData='1' " +
           "bgColor='FFFFFF' lineColor='F90327' showValues='2' seriesNameInToolTip='1' showBorder='0'" +
           " showLabels='1' anchorRadius='2' outCnvBaseFontSize='10' animation='1' " +
           "showAlternateHGridColor='1' AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' " +
           "alternateHGridAlpha='5' canvasBorderColor='666666' baseFontColor='000000' lineThickness='2' " +
           "formatNumberScale='0' formatNumber='0'>" + strGraphData + "</chart>";


    FusionCharts.setCurrentRenderer('javascript');
    var chart = new FusionCharts({
        "type": "msstackedcolumn2d",
        "renderAt": "Div_StarPageLocationCount",
        "width": "900",
        "height": "450"
    });

    chart.setDataXML(sXML);
    chart.render("Div_StarPageLocationCount");

}

function DownloadStarOneHrReport(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    if (dsRoot != null) {
        var o_Excel = dsRoot.getElementsByTagName('Excel');
        var o_Filename = dsRoot.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divLoading").style.display = "none";
    }

    document.getElementById("divLoading").style.display = "none";
}


function LoadDefectiveReport(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    var sTbl = document.getElementById("tblDevectiveDeviceList");

    if (GetBrowserType() == "isIE") {
        sTbl = document.getElementById('tblDevectiveDeviceList');
    }
    else if (GetBrowserType() == "isFF") {
        sTbl = document.getElementById('tblDevectiveDeviceList');
    }

    sTblLen = sTbl.rows.length;

    clearTableRows(sTbl, sTblLen);
    var dsRoot = inf_Obj.responseXML.documentElement;

    var o_SiteId = dsRoot.getElementsByTagName('SiteId');
    var o_SiteName = dsRoot.getElementsByTagName('SiteName');
    var o_TagId = dsRoot.getElementsByTagName('TagId');
    var o_ModelItem = dsRoot.getElementsByTagName('ModelItem');
    var o_FirstSeen = dsRoot.getElementsByTagName('FirstSeen');
    var o_LastSeen = dsRoot.getElementsByTagName('LastSeen');
    var o_Adcvalue = dsRoot.getElementsByTagName('Adcvalue');
    var o_BatteryCapacity = dsRoot.getElementsByTagName('BatteryCapacity');
    var o_Catastrophiccases = dsRoot.getElementsByTagName('Catastrophiccases');
    var o_BatteryFailureMsg = dsRoot.getElementsByTagName('BatteryFailureMsg');

    nRootLength = o_SiteId.length;

    //Header
    row = document.createElement('tr');
    AddCell(row, "SNo", 'siteOverview_TopLeft_Box', "", "", "left", "30px", "40px", "");
    //AddCell(row, "Site Name", 'siteOverview_Box', "", "", "left", "300px", "40px", "");
    AddCell(row, "Tag Id", 'siteOverview_Box', "", "", "right", "60px", "40px", "");
    AddCell(row, "Model Item", 'siteOverview_Box', "", "", "left", "60px", "40px", "");
    AddCell(row, "First Seen", 'siteOverview_Box', "", "", "left", "120px", "40px", "");
    AddCell(row, "Last Seen", 'siteOverview_Box', "", "", "left", "120px", "40px", "");
    AddCell(row, "LBI ADC", 'siteOverview_Box', "", "", "left", "60px", "40px", "");
    AddCell(row, "Bat. Capacity", 'siteOverview_Box', "", "", "right", "80px", "40px", "");
    AddCell(row, "Battery Status", 'siteOverview_Box', "", "", "left", "180px", "40px", "");

    sTbl.appendChild(row);

    if (nRootLength > 0) {

        for (var i = 0; i < nRootLength; i++) {

            var SiteId = setundefined((o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text));
            var SiteName = setundefined((o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text));
            var TagId = setundefined((o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text));
            var ModelItem = setundefined((o_ModelItem[i].textContent || o_ModelItem[i].innerText || o_ModelItem[i].text));
            var FirstSeen = setundefined((o_FirstSeen[i].textContent || o_FirstSeen[i].innerText || o_FirstSeen[i].text));
            var LastSeen = setundefined((o_LastSeen[i].textContent || o_LastSeen[i].innerText || o_LastSeen[i].text));
            var Adcvalue = setundefined((o_Adcvalue[i].textContent || o_Adcvalue[i].innerText || o_Adcvalue[i].text));
            var BatteryCapacity = setundefined((o_BatteryCapacity[i].textContent || o_BatteryCapacity[i].innerText || o_BatteryCapacity[i].text));
            var Catastrophiccases = setundefined((o_Catastrophiccases[i].textContent || o_Catastrophiccases[i].innerText || o_Catastrophiccases[i].text));
            var BatteryFailureMsg = setundefined((o_BatteryFailureMsg[i].textContent || o_BatteryFailureMsg[i].innerText || o_BatteryFailureMsg[i].text));

            var ClsCells = 'tableData_cell';

            row = document.createElement('tr');

            AddCell(row, i + 1, "tableData_cell_left", "", "", "left", "30px", "30px", "");
            //AddCell(row, SiteName, ClsCells, "", "", "left", "300px", "30px", "");
            AddCell(row, TagId, ClsCells, "", "", "right", "60px", "30px", "");
            AddCell(row, ModelItem, ClsCells, "", "", "left", "60px", "30px", "");
            AddCell(row, FirstSeen, ClsCells, "", "", "left", "120px", "30px", "");
            AddCell(row, LastSeen, ClsCells, "", "", "left", "120px", "30px", "");
            AddCell(row, Adcvalue, ClsCells, "", "", "right", "60px", "30px", "");
            AddCell(row, BatteryCapacity, ClsCells, "", "", "right", "80px", "30px", "");
            AddCell(row, BatteryFailureMsg, ClsCells, "", "", "left", "180px", "30px", "");

            sTbl.appendChild(row);
        }

        $("#tblDevectiveDeviceList").show();
    }
    else {

        $("#tblDevectiveDeviceList").show();

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 8, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
    }

    document.getElementById("divLoading").style.display = "none";

}

function ExportDefectiveDeviceReports(dsRoot) {

    document.getElementById("divLoading").style.display = "";

    var csvstringbuilder = [];
    var d = new Date();
    var year = d.getFullYear();
    var month = (d.getMonth() + 1);
    var day = d.getDate();
    var DownloadDate = year + '/' + month + '/' + day;

    csvstringbuilder.push(CSVCell("S.No", true));
    csvstringbuilder.push(CSVCell("Site Name", true));
    csvstringbuilder.push(CSVCell("Tag Id", true));
    csvstringbuilder.push(CSVCell("Model Item", true));
    csvstringbuilder.push(CSVCell("First Seen", true));
    csvstringbuilder.push(CSVCell("Last Seen", true));
    csvstringbuilder.push(CSVCell("Adc value", true));
    csvstringbuilder.push(CSVCell("Bat. Capacity", true));
    csvstringbuilder.push(CSVCell("Battery Status", true));
    csvstringbuilder.push(CSVNewLine());

    var DateHeader = $(dsRoot).find("List");

    var o_SiteName = $(DateHeader).children().filter("SiteName");
    var o_TagId = $(DateHeader).children().filter('TagId');
    var o_ModelItem = $(DateHeader).children().filter('ModelItem');
    var o_FirstSeen = $(DateHeader).children().filter('FirstSeen');
    var o_LastSeen = $(DateHeader).children().filter('LastSeen');
    var o_Adcvalue = $(DateHeader).children().filter('Adcvalue');
    var o_BatteryCapacity = $(DateHeader).children().filter('BatteryCapacity');
    var o_FailureMessage = $(DateHeader).children().filter('BatteryFailureMsg');

    var nLen = o_SiteName.length;
    var tcnt = 0;

    if (nLen > 0) {
        for (var j = 0; j < nLen; j++) {
            var SiteName = setundefined((o_SiteName[0].textContent || o_SiteName[0].innerText || o_SiteName[0].text));
            var TagId = setundefined((o_TagId[j].textContent || o_TagId[j].innerText || o_TagId[j].text));
            var ModelItem = setundefined((o_ModelItem[j].textContent || o_ModelItem[j].innerText || o_ModelItem[j].text));
            var FirstSeen = setundefined((o_FirstSeen[j].textContent || o_FirstSeen[j].innerText || o_FirstSeen[j].text));
            var LastSeen = setundefined((o_LastSeen[j].textContent || o_LastSeen[j].innerText || o_LastSeen[j].text));
            var Adcvalue = setundefined((o_Adcvalue[j].textContent || o_Adcvalue[j].innerText || o_Adcvalue[j].text));
            var BatteryCapacity = setundefined((o_BatteryCapacity[j].textContent || o_BatteryCapacity[j].innerText || o_BatteryCapacity[j].text));
            var FailureMessage = setundefined((o_FailureMessage[j].textContent || o_FailureMessage[j].innerText || o_FailureMessage[j].text));

            csvstringbuilder.push(CSVCell(String(Number(++tcnt)), true));
            csvstringbuilder.push(CSVCell(String(SiteName.replace(',', '')), true));
            csvstringbuilder.push(CSVCell(String(TagId), true));
            csvstringbuilder.push(CSVCell(String(ModelItem), true));
            csvstringbuilder.push(CSVCell(String(FirstSeen), true));
            csvstringbuilder.push(CSVCell(String(LastSeen), true));
            csvstringbuilder.push(CSVCell(String(Adcvalue), true));
            csvstringbuilder.push(CSVCell(String(BatteryCapacity), true));
            csvstringbuilder.push(CSVCell(String(FailureMessage.replace(',', '')), true));
            csvstringbuilder.push(CSVNewLine());
        }
    }
    else {
        csvstringbuilder.push(CSVCell(String("No records found."), true));
    }

    //join array as string
    csvstringbuilder = csvstringbuilder.join("");

    //download csv
    tableToCSV(csvstringbuilder, "CenTrak-GMS-Defective-Device-Reports-" + DownloadDate);
    document.getElementById("divLoading").style.display = "none";
}

var g_SiteObj;

function GetAllSiteForDefectiveDeviceList(SiteId) {

    var SiteId = document.getElementById("ctl00_ContentPlaceHolder1_ddlSites1").value;

    g_SiteObj = CreateDeviceXMLObj();

    if (g_SiteObj != null) {

        document.getElementById('divLoading').style.display = "";

        g_SiteObj.onreadystatechange = ajakDeviceDefectiveSiteList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetSiteListForDefective&SiteId=" + SiteId;

        if (GetBrowserType() == "isIE") {
            g_SiteObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_SiteObj.open("GET", DbConnectorPath, true);
        }
        g_SiteObj.send(null);
    }
    return false;
}

function ajakDeviceDefectiveSiteList() {
    if (g_SiteObj.readyState == 4) {
        if (g_SiteObj.status == 200) {

            var sTbl, sTblLen;

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_SiteObj.responseXML.documentElement);
            var dsRoot = g_SiteObj.responseXML.documentElement;            

            DownloadStarOneHrReport(dsRoot);

            document.getElementById('divLoading').style.display = "none";
       }
    }
}