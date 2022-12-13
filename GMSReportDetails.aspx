<%@ Page Title="GMS Report Details" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="GMSReportDetails.aspx.vb" Inherits="GMSUI.GMSReportDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="Javascript/jquery.plugin.js"></script>
    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
    <script src="Javascript/js_gmsReports.js?d=84" type="text/javascript"></script>
    <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>
    <script language="javascript" type="text/javascript">

        var sReportType = '';
        var sSiteId = '';
        var sFromDate = '';
        var sToDate = '';
        var IsDownload = '0';
        var sStarId = '';
        var IsChkTTSyncError = '0';
        var SearchDeviceIds = "";
        var IsConnFailed = true;
        var g_SumFromDate;
        var g_SumToDate;
        var g_DeviceId;
        var g_SiteName;
        var g_IsPaging = "";
        var g_sGroupCond = "";

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";
            sReportType = $("#ctl00_ContentPlaceHolder1_hdnReportType").val();
            document.getElementById('spnReportType').innerHTML = '';
            ReportView(sReportType);

            if (sReportType != "") {
                var today = new Date();
                var newdate = new Date();
                newdate.setDate(today.getDate() - 1);

                var month = newdate.getMonth() + 1;
                var day = newdate.getDate();

                var output = newdate.getFullYear() + '/' +
                (('' + month).length < 2 ? '0' : '') + month + '/' +
                (('' + day).length < 2 ? '0' : '') + day;
                document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate").value = output;
                document.getElementById("ctl00_ContentPlaceHolder1_txtToDate").value = output;
            }

            g_SiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteId").value;
            g_SiteName = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteName").value;
            g_SumFromDate = document.getElementById("ctl00_ContentPlaceHolder1_hdnFromDate").value;
            g_SumToDate = document.getElementById("ctl00_ContentPlaceHolder1_hdnToDate").value;
            g_DeviceId = document.getElementById("ctl00_ContentPlaceHolder1_hdnDeviceId").value;
            g_IsPaging = document.getElementById("ctl00_ContentPlaceHolder1_hdnIsPaging").value;


            if (sReportType == "") {
                LoadDeviceReport(g_SiteId, g_SiteName, 2, g_SumFromDate, g_SumToDate, g_DeviceId);
            }
            else
                $("#tblFilterRow").show();

            document.getElementById('txtPagingCount').value = 500;
            document.getElementById('txtLocationCount').value = 0;
            document.getElementById('txtTriggerCount').value = 0;
        }

        $(document).ready(function () {
            $('#ChkIsConnectivityFailed').change(function () {
                if ($(this).is(":checked")) {
                    IsConnFailed = true;
                }
                else {
                    IsConnFailed = false;
                }
            });
        });

        function Redirect(sid) {
            location.href = "GMSReports.aspx";
        }

        function GetReport(IsDownload) {
            if (IsDownload == 0) {
                Pagecallcount = 1;
                Collisioncallcount = 1;
                g_PagingVersion = "";
                g_CollisionVersion = "";
            }
            ShowReport(sReportType, IsDownload);
        }

        function ReportClear() {
            $("#txtStarId").val("");
            $("#txtDeviceIds").val("");
            $("#txtPagingCount").val("500");
            $("#txtLocationCount").val("0");
            $("#txtTriggerCount").val("0");
            ClearCheckingFilter();
            $("#ddlMonitorFilter").val(2);
            $("#ddlLocationMonitorFilter").val(2);
            $("#ddlTriggerMonitorFilter").val(2);

            document.getElementById('chkStarsUpgradeMode').checked = false;
            document.getElementById('chkStarsTTSyncError').checked = false;
            document.getElementById('chkStarsNotReceivingData').checked = false;
            document.getElementById('chkStarsNotReceiving24hr').checked = false;
            document.getElementById('chkStarsSeenNetworkIssue').checked = false;
            $("#ddlStarFilter").val(2);

            GetReport(0);
        }

        function PagingSummary() {
            DeviceSummary();
            $('#spnPagingSummary').removeAttr('class');
            $("#spnCollisionSummary").removeAttr('class').addClass('current');           
            if (Pagecallcount == 1) {
                $('#tblMonitorAnalysisPagingReportTbody').empty();
                setTimeout(function () { LoadMonitorAnalysisPagingReport(); PagingTotalSummary(); CollisionTotalSummary(); CheckingFilter(); $("#divLoading").hide(); }, 10);
            }
            else {
                $("#divLoading").hide();
            }
            Pagecallcount++;
        }

        function CollisionSummary() {
            DeviceSummary();

            $("#tblMonitorAnalysisPagingReports").hide();
            $("#tblMonitorAnalysisCollisionReports").show();

            $('#spnCollisionSummary').removeAttr('class');
            $("#spnPagingSummary").removeAttr('class').addClass('current');

            if (Collisioncallcount == 1) {
                $('#tblMonitorAnalysisCollisionReportTbody').empty();
                setTimeout(function () { LoadMonitorAnalysisCollisionReport(); PagingTotalSummary(); CollisionTotalSummary(); CheckingFilter(); $("#divLoading").hide(); }, 10);
            }
            else {
                $("#divLoading").hide();
            }
            Collisioncallcount++;
        }

        function DeviceSummary() {
            $("#divLoading").show();
            $('#spnPagingSummary').removeAttr('class').addClass('current');
            $("#spnCollisionSummary").removeAttr('class');
            $("#trMonitorAnalysisReport").show();

            $('#subtabs1').show();
            $("#tblMonitorAnalysisPagingReports").show();
            $("#tblMonitorAnalysisCollisionReports").hide();
            $("#tblMonitorAnalysisReport").hide();
            $("#trDeviceSummary").hide();
        }

        function ReportView(ReportType) {

            if (ReportType == 1) {
                $("#trMenu").show();
                $("#trMonitorAnalysisReport").show();
                $("#trDeviceSummary").hide();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").hide();
                $("#btnExportReport").show();
                $("#trConnectivitysearch").show();
                $("#tdConnectivity").hide();
                $("#ctl00_ContentPlaceHolder1_txtToDate").hide();
                $("#lblDate").hide();
                $("#tdMonitorFilter").show();

                document.getElementById('spnReportType').innerHTML = 'Monitor Analysis Report';
            }
            else if (ReportType == 2) {

                $('#spnPagingSummary').removeAttr('class');
                $('#spnCollisionSummary').removeAttr('class');

                $("#trMenu").show();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").show();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").hide();
                $("#trConnectivitysearch").show();
                $("#tdConnectivity").hide();
                $("#tdMonitorFilter").hide();

                document.getElementById('spnReportType').innerHTML = 'Device Summary Report';
            }
            else if (ReportType == 3) {
                $("#trMenu").hide();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").hide();
                $("#trTTSyncErrReportsStar").show();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").hide();
                $("#btnExportReport").show();
                $("#tr_TTSyncSearch").show();
                $("#tdMonitorFilter").hide();
                $("#ctl00_ContentPlaceHolder1_txtToDate").hide();
                $("#lblDate").hide();
                $("#tdStarFilter").hide();
                document.getElementById('spnReportType').innerHTML = 'Star Analysis Report';
            }
            else if (ReportType == 4) {
                $("#trMenu").hide();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").hide();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").show();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").hide();
                $("#btnExportReport").show();
                $("#trConnectivitysearch").show();
                $("#tdMonitorFilter").hide();

                document.getElementById('spnReportType').innerHTML = 'Connectivity Report For Devices';
            }
            else if (ReportType == 5) {
                $("#trMenu").hide();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").hide();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").show();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").hide();
                $("#tdMonitorFilter").hide();
            }
            else if (ReportType == 6) {
                $("#trMenu").hide();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").hide();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").show();
                $("#trEMQualificationReport").hide();
                $("#tdMonitorFilter").hide();
            }
            else if (ReportType == 7) {
                $("#trMenu").hide();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").hide();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").show();
                $("#tdMonitorFilter").hide();
            }
            else {
                $('#spnPagingSummary').removeAttr('class');
                $('#spnCollisionSummary').removeAttr('class');

                $("#trMenu").show();
                $("#trMonitorAnalysisReport").hide();
                $("#trDeviceSummary").show();
                $("#trTTSyncErrReportsStar").hide();
                $("#trDeviceConnectivityReport").hide();
                $("#trRaWDataReport").hide();
                $("#trInactiveDevicesReport").hide();
                $("#trEMQualificationReport").hide();
                $("#trConnectivitysearch").show();
                $("#tdConnectivity").hide();
                $("#tdMonitorFilter").hide();

                document.getElementById('spnReportType').innerHTML = 'Device Summary Report';
            }
        }

        function ShowReport(ReportType, IsDownload, FromDate, ToDate, DeviceId) {

            ReportView(ReportType);

            g_IsDeviceSummary = 0;

            var PagingCount = "";
            var LocationCount = "";
            var TriggerCount = "";
            var sFilterCond = 0;
            var sGroupCond = 0;

            var sStarsUpgradeMode = 0;
            var sStarsTTSyncError = 0;
            var sStarsNotReceivingData = 0;
            var sStarsNotReceiving24hr = 0;
            var sStarsSeenNetworkIssue = 0;
            var sLocationFilterCond = 0;
            var sTriggerFilterCond = 0;

            sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
            sFromDate = $("#ctl00_ContentPlaceHolder1_txtFromDate").val();
            sToDate = $("#ctl00_ContentPlaceHolder1_txtToDate").val();
            sStarId = $("#txtStarId").val();

            if (setundefined(FromDate) != "") {
                g_IsDeviceSummary = 1;
                var startDate = FromDate.split('-');
                sFromDate = startDate[2] + "/" + startDate[1] + "/" + startDate[0];
            }

            if (setundefined(ToDate) != "") {
                var endDate = ToDate.split('-');
                sToDate = endDate[2] + "/" + endDate[1] + "/" + endDate[0];
            }

            if (sSiteId == 0) {
                alert('Please select site');
                document.getElementById("<%=ddlSites.ClientID%>").focus();
                return false;
            }

            g_SiteName = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").text();

            if (sFromDate == '') {
                alert('Please select From date');
                document.getElementById("<%=txtFromDate.ClientID%>").focus();
                return false;
            }

            if (sToDate == '') {
                alert('Please select To date');
                document.getElementById("<%=txtToDate.ClientID%>").focus();
                return false;
            }

            if (setundefined(DeviceId) != "")
                SearchDeviceIds = DeviceId;
            else
                SearchDeviceIds = $("#txtDeviceIds").val();

            //PagingCount LocationCount TriggerCount sCond for Monitor
            sGroupCond = "";
            g_sGroupCond = "";
            if (ReportType == "1") {
                PagingCount = document.getElementById('txtPagingCount').value;
                LocationCount = document.getElementById('txtLocationCount').value;
                TriggerCount = document.getElementById('txtTriggerCount').value;
                sFilterCond = document.getElementById('ddlMonitorFilter').value;
                sLocationFilterCond = document.getElementById('ddlLocationMonitorFilter').value;
                sTriggerFilterCond = document.getElementById('ddlTriggerMonitorFilter').value;

                if ($('#PagingDefined').prop("checked")) {
                    sGroupCond += "1,";
                }
                else {
                    sGroupCond += "0,";
                }
                if ($('#PagingUndefined').prop("checked")) {
                    sGroupCond += "1,";
                }
                else {
                    sGroupCond += "0,";
                }
                if ($('#PagingInGroups').prop("checked")) {
                    sGroupCond += "1,";
                }
                else {
                    sGroupCond += "0,";
                }
                if ($('#PagingNotInGroups').prop("checked")) {
                    sGroupCond += "1,";
                }
                else {
                    sGroupCond += "0,";
                }
                sGroupCond = sGroupCond.slice(0, -1);
                g_sGroupCond = sGroupCond;
            }
            //Filter condition for Star
            if (ReportType == "3") {

                if (document.getElementById('chkStarsUpgradeMode').checked) {
                    sStarsUpgradeMode = 1;
                }
                if (document.getElementById('chkStarsTTSyncError').checked) {
                    sStarsTTSyncError = 2;
                }
                if (document.getElementById('chkStarsNotReceivingData').checked) {
                    sStarsNotReceivingData = 3;
                }
                if (document.getElementById('chkStarsNotReceiving24hr').checked) {
                    sStarsNotReceiving24hr = 4;
                }
                if (document.getElementById('chkStarsSeenNetworkIssue').checked) {
                    sStarsSeenNetworkIssue = 5;
                }
            }

            LoadAllReports(sSiteId, ReportType, 1, sFromDate, sToDate, SearchDeviceIds, IsDownload, sStarId, IsChkTTSyncError, IsConnFailed, g_SiteName, PagingCount, LocationCount, TriggerCount, sFilterCond, sLocationFilterCond, sTriggerFilterCond, sGroupCond, sStarsUpgradeMode, sStarsTTSyncError, sStarsNotReceivingData, sStarsNotReceiving24hr, sStarsSeenNetworkIssue);
        }

        var sDFromDate, sSToDate;

        function LoadDeviceReport(sSiteId, sSiteName, ReportType, FromDate, ToDate, SearchDeviceIds) {

            $("#tblFilterRow").hide();
            $("#trMenu").hide();
            $("#trDeviceHeader").show();

            if (setundefined(FromDate) != "") {
                g_IsDeviceSummary = 1;
                var startDate = FromDate.split('-');
                sDFromDate = startDate[2] + "/" + startDate[1] + "/" + startDate[0];
            }

            if (setundefined(ToDate) != "") {
                var endDate = ToDate.split('-');
                sSToDate = endDate[2] + "/" + endDate[1] + "/" + endDate[0];
            }

            document.getElementById('spnReportType').innerHTML = "Device Hourly Info";
            document.getElementById('spnSiteName').innerHTML = sSiteName;
            document.getElementById('spnDate').innerHTML = sDFromDate;

            LoadAllReports(sSiteId, ReportType, 1, sDFromDate, sSToDate, SearchDeviceIds, 0, 0, 0, 0);
        }

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }

        function CallLogViewer(rarUrl, sDeviceId) {
            var DeviceType = "2";
            rarUrl = rarUrl.replace(/_/g, ' ');
            window.open("LogViewer.aspx?Url=" + decodeURIComponent(rarUrl) + "&DeviceId=" + sDeviceId + "&SiteName=" + g_SiteName + "&SiteId=" + g_SiteId + "&DeviceType=" + DeviceType);
            return false;
        }

        function StarPLData() {
            sSiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();

            var d = new Date();
            var year = d.getFullYear();
            var month = (d.getMonth() + 1);
            var day = d.getDate();
            var sDate = year + '/' + month + '/' + day;
            window.open("StarHourlyDetail.aspx?qSiteId=" + sSiteId + "&qDate=" + sDate + "&qStarId=0");
        }

        // Date Picker control
        $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39', step: 1 });
        $(function () {

            $('#ctl00_ContentPlaceHolder1_txtFromDate').datetimepicker({
                format: 'Y/m/d',
                step: 10,
                timepicker: false
            });

            $('#ctl00_ContentPlaceHolder1_txtToDate').datetimepicker({
                format: 'Y/m/d',
                step: 10,
                timepicker: false
            });
        });

    </script>
    <style type="text/css">
        #tabs1, #subtabs1
        {
            float: left;
            width: 50%;
            background: #FFFFFF;
            line-height: normal;
        }
        #tabs1 ul, #subtabs1 ul
        {
            margin: 0;
            padding: 10px 10px 0 0px;
            list-style: none;
        }
        #tabs1 li, #subtabs1 li
        {
            display: inline;
            margin: 0;
            padding: 0;
        }
        #tabs1 a, #subtabs1 a
        {
            float: left;
            background: url("Images/tableft1.gif") no-repeat left top;
            margin: 0;
            padding: 0 0 0 4px;
            text-decoration: none;
            cursor: pointer;
        }
        #tabs1 a span, #subtabs1 a span
        {
            float: left;
            display: block;
            background: url("Images/tabright1.gif") no-repeat right top;
            padding: 5px 5px 4px 6px;
            color: #005695;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 12px;
        }
        /* Commented Backslash Hack hides rule from IE5-Mac \*/
        #tabs1 a span, #subtabs1 a span
        {
            float: none;
        }
        /* End IE5-Mac hack */
        #tabs a:hover span, #subtabs1 a:hover span
        {
            color: #005695;
        }
        #tabs1 a:hover, #subtabs1 a:hover
        {
            background-position: 0% -42px;
        }
        #tabs1 a:hover span, #subtabs1 a:hover span
        {
            background-position: 100% -42px;
        }
        #tabs1 .current a, #subtabs1 .current a
        {
            background-position: 0% -42px;
        }
        #tabs1 .current a span, #subtabs1 .current a span
        {
            background-position: 100% -42px;
        }
    </style>
    <div id="divReportDetails" style="top: auto; left: auto; height: 850px;">
        <input id="hdnReportType" type="hidden" runat="server" />
        <input id="hdnFromDate" type="hidden" runat="server" />
        <input id="hdnToDate" type="hidden" runat="server" />
        <input id="hdnDeviceId" type="hidden" runat="server" />
        <input id="hdnSiteId" type="hidden" runat="server" />
        <input id="hdnSiteName" type="hidden" runat="server" />
        <input id="hdnIsPaging" type="hidden" runat="server" />
        <input id="hdnSiteFolderName" type="hidden" runat="server" />
        <input id="hdnStatus" type="hidden" runat="server" />
        <input id="hdnTotalStars" type="hidden" />
        <input id="hdnInactiveStars" type="hidden" />
        <input id="hdnPagingVersionMonitor" type="hidden" runat="server" />
        <input id="hdnCollisionVersionMonitor" type="hidden" runat="server" />
        <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="tblHeader"
                        runat="server">
                        <tr style="height: 10px;">
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='Redirect();'>
                                                        <a href="GMSReports.aspx">
                                                            <img src='images/Left-Arrow.png' alt='' title='Settings' border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    Connect Pulse Reports
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>
                                                                    <span id="spnReportType"></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top" colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="5" cellspacing="5" width="1300px" class="clsFilterRow"
                                                id="tblFilterRow" align="center" style="display: none;">
                                                <tbody>
                                                    <tr>
                                                        <td class="clsLALabel" style="width: 50px;">
                                                            Sites :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSites" runat="server" Style="width: 400px;">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="clsLALabel" style="width: 50px;">
                                                            <b>&nbsp;Date&nbsp;:&nbsp;</b>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtFromDate" name="txtFromDate" runat="server" style="width: 100px;
                                                                text-align: left; font-size: 13px;" class="clsLATextbox" />
                                                        </td>
                                                        <td class="clsLALabel" style="width: 50px;">
                                                            <label id="lblDate">
                                                                <b>To&nbsp;Date&nbsp;:&nbsp;</b></label>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtToDate" name="txtToDate" runat="server" style="width: 100px;
                                                                text-align: left; font-size: 13px;" class="clsLATextbox" />
                                                        </td>
                                                        <td colspan="4" align="center">
                                                            <input type="button" value="Show" style="width: 80px; height: 30px;" class="clsExportExcel"
                                                                name="btnShowActivity" id="btnactivity" onclick="GetReport(0);" />
                                                            <input type="button" value="Export" style="width: 80px; height: 30px; display: none;"
                                                                class="clsExportExcel" name="btnExportReport" id="btnExportReport" onclick="GetReport(1);" />
                                                            <input type="button" value="Refresh" style="width: 80px; height: 30px;" class="clsExportExcel"
                                                                name="btnClear" id="btnClear" onclick="ReportClear();" />
                                                        </td>
                                                        <td colspan="4" align="center">
                                                        </td>
                                                    </tr>
                                                    <tr id="tr_TTSyncSearch" class="clsLALabel" style="display: none;">
                                                        <td valign="top" class="clsLALabel" style="width: 50px;">
                                                            Star Id :
                                                        </td>
                                                        <td style="width: 300px;">
                                                            <textarea id="txtStarId" name="txtStarId" style="width: 295px; height: 50px; text-align: left;
                                                                font-size: 13px;" class="clsLATextbox"></textarea>
                                                        </td>
                                                        <td id="tdStarFilter" colspan="12" style="text-align: left; vertical-align: top;
                                                            display: none;">
                                                            <table style="width: 60%;">
                                                                <tr>
                                                                    <td>
                                                                        P.Cnt :
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txtStarPagingCount" name="txtStarPagingCount" style="width: 50px;
                                                                            font-size: 13px; text-align: left;" class="clsLATextbox" onkeypress="return isNumber(event)" />
                                                                    </td>
                                                                    <td>
                                                                        L.Cnt :
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txtStarLocationCount" name="txtStarLocationCount" style="width: 50px;
                                                                            font-size: 13px; text-align: left;" class="clsLATextbox" onkeypress="return isNumber(event)" />
                                                                    </td>
                                                                    <td>
                                                                        Error Count :
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txtStarErrorCount" name="txtStarErrorCount" style="width: 50px;
                                                                            font-size: 13px; text-align: left;" class="clsLATextbox" onkeypress="return isNumber(event)" />
                                                                    </td>
                                                                    <td>
                                                                        <select id="ddlStarFilter" name="ddlStarFilter" style="height: 25px">
                                                                            <option value="2" selected="selected">&gt;=</option>
                                                                            <option value="3">&lt;=</option>
                                                                            <option value="1">=</option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trConnectivitysearch" class="clsLALabel" style="display: none;">
                                                        <td style="vertical-align: top; width: 80px;">
                                                            <b>Device Id : </b>
                                                        </td>
                                                        <td style="width: 300px;" valign="top" align="left" colspan="1">
                                                            <textarea id="txtDeviceIds" style="width: 294px;" class="clsTextAreaBox"></textarea>
                                                        </td>
                                                        <td id="tdConnectivity" colspan="5" style="text-align: left; vertical-align: top;">
                                                            <input class="clsLAText" type="checkbox" id="ChkIsConnectivityFailed" name="ChkIsConnectivityFailed"
                                                                style="vertical-align: middle; display: none;" />
                                                            <span><b>Is Connectivity Failed</b></span><span id="Conn" style="color: Red;"></span>
                                                        </td>
                                                        <td id="tdMonitorFilter" colspan="12" style="text-align: left; vertical-align: top;
                                                            display: none;">
                                                            <table style="width: 70%;">
                                                                <tr>
                                                                    <td>
                                                                        P.Cnt :
                                                                    </td>
                                                                    <td>
                                                                        <select id="ddlMonitorFilter" name="ddlMonitorFilter" style="height: 25px">
                                                                            <option value="2" selected="selected">&gt;=</option>
                                                                            <option value="3">&lt;=</option>
                                                                            <option value="1">=</option>
                                                                        </select>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txtPagingCount" name="txtPagingCount" style="width: 50px;
                                                                            font-size: 13px; text-align: left;" class="clsLATextbox" onkeypress="return isNumber(event)" />
                                                                    </td>                                                                    
                                                                    <td>
                                                                        L.Cnt :
                                                                    </td>
                                                                    <td>
                                                                        <select id="ddlLocationMonitorFilter" name="ddlLocationMonitorFilter" style="height: 25px">
                                                                            <option value="2" selected="selected">&gt;=</option>
                                                                            <option value="3">&lt;=</option>
                                                                            <option value="1">=</option>
                                                                        </select>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txtLocationCount" name="txtLocationCount" style="width: 50px;
                                                                            font-size: 13px; text-align: left;" class="clsLATextbox" onkeypress="return isNumber(event)" />
                                                                    </td>                                                                    
                                                                    <td>
                                                                        Trigger Count :
                                                                    </td>
                                                                    <td>
                                                                        <select id="ddlTriggerMonitorFilter" name="ddlTriggerMonitorFilter" style="height: 25px">
                                                                            <option value="2" selected="selected">&gt;=</option>
                                                                            <option value="3">&lt;=</option>
                                                                            <option value="1">=</option>
                                                                        </select>
                                                                    </td>
                                                                    <td>
                                                                        <input type="text" id="txtTriggerCount" name="txtTriggerCount" style="width: 50px;
                                                                            font-size: 13px; text-align: left;" class="clsLATextbox" onkeypress="return isNumber(event)" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 5px;">
                <td valign="top" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr id="trMenu" style="display: none;">
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="300px">
                        <tr>
                            <td>
                                <div id="tabs1" style="width: 600px;">
                                    <ul>
                                        <!-- CSS Tabs -->
                                        <li id="spnPagingSummary" onclick="PagingSummary()" ><a><span>Paging
                                            Analysis</span></a></li>
                                        <li id="spnCollisionSummary" onclick="CollisionSummary()" class="current"><a><span>Beacon Collision
                                            Analysis</span></a></li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trMonitorAnalysisReport" style="display: none;">
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table id="tblMonitorAnalysisReport" cellspacing="1" cellpadding="3" style="width: 100%;
                                    padding-top: 10px;" class="display">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblMonitorAnalysisPagingReports" style="width: 1330px;">
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <table id="tblMonitorVersionSummary" cellspacing="0" cellpadding="3" class="display">
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table id="tblMonitorSummary" cellspacing="0" cellpadding="3" style="width: 100%"
                                                class="display">
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table id="tblMonitorAnalysisPagingReport" cellspacing="1" cellpadding="3" style="width: 100%;
                                                display: none; padding-top: 10px;" class="display">
                                                <thead>
                                                    <tr>
                                                        <th class='siteOverview_TopLeft_Box' align='center' height='30px'>
                                                            S.No
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Monitor&nbsp;Id
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            IRID
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Locked Star
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Beacon Slot
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Version
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            First Seen
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Last Seen
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            P.Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            L.Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Trigger Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Defined
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Star Seen Count
                                                        </th>
                                                        <th class='siteOverview_Box' height='30px'>
                                                            Group
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id='tblMonitorAnalysisPagingReportTbody'>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblMonitorAnalysisCollisionReports" style="width: 1330px;">
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <table id="tblMonitorCollisionVersionSummary" cellspacing="0" cellpadding="3" class="display">
                                            </table>
                                        </td>
                                        <td style="vertical-align: top;">
                                            <table id="tblMonitorCollisionSummary" cellspacing="0" cellpadding="3" style="width: 100%"
                                                class="display">
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table id="tblMonitorAnalysisCollisionReport" cellspacing="1" cellpadding="3" style="width: 100%;
                                                display: none; padding-top: 10px;" class="display">
                                                <thead>
                                                    <tr>
                                                        <th class='siteOverview_TopLeft_Box' align='center' height='40px'>
                                                            S.No
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Monitor&nbsp;Id
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            IRID
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Locked Star
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Beacon Slot
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Version
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            First Seen
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Last Seen
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Beacon Collision Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            P.Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            L.Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Trigger Cnt
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Defined
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Star Seen Count
                                                        </th>
                                                        <th class='siteOverview_Box' align='center' height='40px'>
                                                            Group
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tblMonitorAnalysisCollisionReportTbody">
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trDeviceHeader" style="display: none;">
                <td>
                    <table cellpadding="5" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td colspan="3">
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td class="subHeader_black4" style="text-align: left; width: 350px; max-width: 350px;
                                            white-space: nowrap;">
                                            Site Name : <span id="spnSiteName" style="color: Green; font-weight: normal;"></span>
                                        </td>
                                        <td class="subHeader_black2" style="text-align: right;">
                                            <input type="button" id="btnExportDevice" value="Export" class="clsExportExcel" onclick="ExportMonitor();"
                                                style="display: none;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td class="subHeader_black4" style="text-align: left; width: 350px; max-width: 350px">
                                            Date : <span id="spnDate" style="color: Green; font-weight: normal;"></span>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td class="subHeader_black4" style="text-align: left; width: 350px; max-width: 350px">
                                            <span id="spnDeviceId" style="color: Green; font-weight: normal;"></span>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;" />
                                    <tr>
                                        <td class="subHeader_black4" style="text-align: left; width: 350px; max-width: 350px">
                                            InServer.Ini : <span id="spnInServerINI" style="color: Green; font-weight: normal;">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;" />
                                    <tr>
                                        <td class="subHeader_black4" style="text-align: left; width: 350px; max-width: 350px">
                                            Operating Mode : <span id="spnOperatingMode" style="color: Green; font-weight: normal;">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;" />
                                    <tr>
                                        <td class="subHeader_black4" style="text-align: left; width: 400px; max-width: 400px;
                                            white-space: nowrap;">
                                            Status : <span id="SpnStatus" style="color: Green; white-space: nowrap; font-weight: normal;">
                                            </span>
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
            <tr id="trDeviceSummary">
                <td valign="top" colspan="3" align="center">
                    <table id="tblDeviceConnectivityChartReport" cellspacing="1" cellpadding="3" style="width: 1000px;
                        display: none;" class="display">
                        <tr>
                            <td style="display: table-cell;" align="center" class="cell_text_Red">
                                Page Count
                            </td>
                            <td style="display: table-cell;" align="center" class="cell_text_Red">
                                Location Count
                            </td>
                            <td style="display: table-cell;" align="center" class="cell_text_Red">
                                Trigger Count
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div id='DivPagingCount_MSLine2D' style='height: 250px; width: 250px; border: 2px solid rgb(218, 218, 218);'>
                                </div>
                            </td>
                            <td align="center">
                                <div id='DivLocationCount_MSLine2D' style='height: 250px; width: 250px; border: 2px solid rgb(218, 218, 218);'>
                                </div>
                            </td>
                            <td align="center">
                                <div id='DivTriggerCount_MSLine2D' style='height: 250px; width: 250px; border: 2px solid rgb(218, 218, 218);'>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 1800px; overflow-x: auto; overflow-y: hidden;">
                        <table id="tblDeviceSummary" border="0" cellspacing="1" cellpadding="3" width="1800px">
                            <thead>
                                <tr>
                                    <th class="siteOverview_TopLeft_Box" width="30px" height="30px" colspan="1">
                                        S.No
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        IRId
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Locked StarId
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Version
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        P.Cnt
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        L.Cnt
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Trigger Count
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        WiFi Count
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Super Sync Count
                                    </th>
                                    <th class="siteOverview_Box" id="tdBeaconCollisionStarCount" width="30px" height="30px"
                                        colspan="1">
                                        Beacon Collision Star Count
                                    </th>
                                    <th class="siteOverview_Box" id="tdBeaconCollisionStars" width="100px" height="30px"
                                        colspan="1">
                                        Beacon Collision Stars
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Star Seen Count
                                    </th>
                                    <th class="siteOverview_Box" width="100px" height="30px" colspan="1" style="min-width: 150px;
                                        max-width: 150px">
                                        Star Seen
                                    </th>
                                    <th class="siteOverview_Box" width="100px" height="30px" colspan="1">
                                        Last Received Time
                                    </th>
                                    <th class="siteOverview_Box" width="100px" height="30px" colspan="1">
                                        Last Paged Time
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        LBI Value
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Min Rssi
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Max Rssi
                                    </th>
                                    <th class="siteOverview_Box" width="30px" height="30px" colspan="1">
                                        Avg Rssi
                                    </th>
                                </tr>
                            </thead>
                            <tbody id='tblDeviceSummaryTbody'>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
            <tr id="trTTSyncErrReportsStar">
                <td valign="top">
                    <table id="tblTTSyncErrReportsStarCounts" style="display: none;" border="0">
                        <tr>
                            <td style="display: table-cell; width: 150px;" valign="top">
                                <table id="tblStarVersionSummary" cellspacing="0" cellpadding="6" style="width: 100%"
                                    class="display">
                                </table>
                            </td>
                            <td style="display: table-cell; padding-left: 10px; width: 650px;" valign="top">
                                <table cellspacing="0" cellpadding="6" class="display" style="width: 100%;" border="0">
                                    <tr>
                                        <td class="Summary_header_cell" style='width: 240px'>
                                            Status
                                        </td>
                                        <td align="right" class="Summary_header_cell" style='width: 50px'>
                                            #
                                        </td>
                                        <td class="Summary_header_cell" style='width: 300px'>
                                            Star Ids
                                        </td>
                                        <td class="Summary_header_cell" style='width: 30px'>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Summary_cell">
                                            Total Stars
                                        </td>
                                        <td align="right" id="lblTotalStars" class="Summary_cell">
                                        </td>
                                        <td class="Summary_cell">
                                        </td>
                                        <td class="Summary_cell">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Summary_cell">
                                            InActive Stars
                                        </td>
                                        <td align="right" id="lblInActiveStars" class="Summary_cell">
                                        </td>
                                        <td class="Summary_cell">
                                        </td>
                                        <td class="Summary_cell">
                                        </td>
                                    </tr>
                                    <tr id="tr_StarsUpgradeMode" style="display: none;">
                                        <td class="Summary_cell" style='color: #1ddcdc; vertical-align: top;'>
                                            Stars in Upgrade Mode
                                        </td>
                                        <td align="right" id="lblStarsInUpgradeModeCount" class="Summary_cell" style='vertical-align: top;'>
                                        </td>
                                        <td align="left" class="Summary_cell" style='vertical-align: top;'>
                                            <div id="lblStarsInUpgradeMode" style="min-height: 20px; max-height: 100px; overflow: auto;">
                                            </div>
                                        </td>
                                        <td align="center" class="Summary_cell" style='vertical-align: top;'>
                                            <input type="checkbox" id="chkStarsUpgradeMode" name="chkStarsUpgradeMode" />
                                        </td>
                                    </tr>
                                    <tr id="tr_StarsTimeSyncError" style="display: none;">
                                        <td class="Summary_cell" style='color: #ef596c; vertical-align: top;'>
                                            Stars in Time Sync Error
                                        </td>
                                        <td align="right" id="lblStarsInTimeSyncErrorCount" class="Summary_cell" style='vertical-align: top;'>
                                        </td>
                                        <td align="left" class="Summary_cell" style='vertical-align: top;'>
                                            <div id="lblStarsInTimeSyncError" style="min-height: 20px; max-height: 100px; overflow: auto;">
                                            </div>
                                        </td>
                                        <td align="center" class="Summary_cell" style='vertical-align: top;'>
                                            <input type="checkbox" id="chkStarsTTSyncError" name="chkStarsTTSyncError" />
                                        </td>
                                    </tr>
                                    <tr id="tr_StarsNotReceivingData" style="display: none;">
                                        <td class="Summary_cell" style='color: #ffc04d; vertical-align: top;'>
                                            Stars not receiving any data
                                        </td>
                                        <td align="right" id="lblStarsNotReceivingDataCount" class="Summary_cell" style='vertical-align: top;'>
                                        </td>
                                        <td align="left" class="Summary_cell" style='vertical-align: top;'>
                                            <div id="lblStarsNotReceivingData" style="min-height: 20px; max-height: 100px; overflow: auto;">
                                            </div>
                                        </td>
                                        <td align="center" class="Summary_cell" style='vertical-align: top;'>
                                            <input type="checkbox" id="chkStarsNotReceivingData" name="chkStarsNotReceivingData" />
                                        </td>
                                    </tr>
                                    <tr id="tr_StarsNotReceiving24hrData" style="display: none;">
                                        <td class="Summary_cell" style='color: #7BB7DF; vertical-align: top;'>
                                            Stars did not receiving 24 hour data
                                        </td>
                                        <td align="right" id="lblStarsNotReceiving24hourDataCount" class="Summary_cell" style='vertical-align: top;'>
                                        </td>
                                        <td align="left" class="Summary_cell" style='vertical-align: top;'>
                                            <div id="lblStarsNotReceiving24hourData" style="min-height: 20px; max-height: 100px;
                                                overflow: auto;">
                                            </div>
                                        </td>
                                        <td align="center" class="Summary_cell" style='vertical-align: top;'>
                                            <input type="checkbox" id="chkStarsNotReceiving24hr" name="chkStarsNotReceiving24hr" />
                                        </td>
                                    </tr>
                                    <tr id="tr_StarsNetworkIssue" style="display: none;">
                                        <td class="Summary_cell" style='color: #bdbd29; vertical-align: top;'>
                                            Stars in Network Issue
                                        </td>
                                        <td align="right" id="lblStarsSeenNetworkIssueCount" class="Summary_cell" style='vertical-align: top;'>
                                        </td>
                                        <td align="left" class="Summary_cell" style='vertical-align: top;'>
                                            <div id="lblStarsSeenNetworkIssue" style="min-height: 20px; max-height: 100px; overflow: auto;">
                                            </div>
                                        </td>
                                        <td align="center" class="Summary_cell" style='vertical-align: top;'>
                                            <input type="checkbox" id="chkStarsSeenNetworkIssue" name="chkStarsSeenNetworkIssue" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" align="right">
                                <input type="button" value="Show Star Daily Data" style="width: 150px; height: 30px;"
                                    class="clsExportExcel" name="btnShowStarDaily" id="btnShowStarDaily" onclick="StarPLData();" />
                                <div id="Div_PageLocationCount">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan='4'>
                                <table id="tblTTSyncErrReportsStar" cellspacing="1" cellpadding="3" style="width: 100%;
                                    padding-top: 10px;" class="display">
                                    <thead>
                                        <tr>
                                            <th class='siteOverview_TopLeft_Box' height='30px'>
                                                S.No
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Star Id
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Version
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Beacon Slot
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Star Type
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Mac Id
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                IP Address
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Updated On
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Response Cnt
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Paging Cnt
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Paging Data Cnt
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Location Cnt
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Location Data Cnt
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                TT Sync Error
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Error Cnt
                                            </th>
                                            <th class='siteOverview_Box' height='30px'>
                                                Reporting Hrs
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody id="tblTTSyncErrReportsStarbody">
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trDeviceConnectivityReport">
                <td valign="top">
                    <table id="tblDeviceConnectivityReportCount" style="display: none;">
                        <tr>
                            <td style="text-align: center;">
                                <div id="divConnectivityReport" style="width: 601px; height: 210px; border: 1px solid #DADADA;
                                    overflow-y: hidden; overflow-x: hidden;">
                                </div>
                            </td>
                            <td style="text-align: center;">
                                <div id="divPageCntChat" style="width: 601px; border: 1px solid #DADADA; overflow-y: hidden;
                                    overflow-x: auto;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <div id="divRFLCntChart" style="width: 601px; border: 1px solid #DADADA; overflow-y: hidden;
                                    overflow-x: auto;">
                                </div>
                            </td>
                            <td style="text-align: center;">
                                <div id="divTriggerChtChart" style="width: 601px; border: 1px solid #DADADA; overflow-y: hidden;
                                    overflow-x: auto;">
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table id="tblDeviceConnectivityReport" cellspacing="1" cellpadding="3" style="width: 100%;"
                                    class="display">
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trRaWDataReport">
                <td valign="top">
                    <table id="tblRaWDataReport" cellspacing="1" cellpadding="3" style="width: 100%;
                        padding-top: 10px;" class="display">
                    </table>
                </td>
            </tr>
            <tr id="trEMQualificationReport">
                <td valign="top">
                    <table id="tblEMQualificationReport" cellspacing="1" cellpadding="3" style="width: 100%;
                        padding-top: 10px;" class="display">
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog_BeaconStarSlot" title="Beacon Star Details" style="display: none;">
        <table cellpadding="3" cellspacing="0" border="0" width="200px" id="tblBeaconStar">
        </table>
    </div>
    <div id="dialog_StarSlot" title="Star Seen Details" style="display: none;">
        <span style='float: left; font-size: 12px; font-weight: bold;'>Status:&nbsp;<label
            id="lblSlotStatus" style='color: red;'></label></span>
        <br />
        <table cellpadding="3" cellspacing="0" border="0" width="200px" id="tblStarSeen">
        </table>
    </div>
    <div id="dialog_GroupMonitor" title="Group Monitor Details" style="display: none;">
        <table cellpadding="3" cellspacing="0" border="0" width="100%" id="tblGroupMonitors">
        </table>
    </div>
    <div id="dialog_MonitorInfo" title="Monitor Details" style="display: none;">
        <table cellpadding="3" cellspacing="0" border="0" width="100%" id="tblMonitorsInfo">
        </table>
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none; z-index: 1000"
        id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:Content>
