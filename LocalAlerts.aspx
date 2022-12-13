<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="LocalAlerts.aspx.vb" Inherits="GMSUI.LocalAlerts" Title="Local Alerts" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>
    <script language="javascript" type="text/javascript">
        var GSiteId = "";
        this.onload = function () {
            var g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            var hdSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            document.getElementById("ctl00_headerBanner_drpsitelist").value = hdSiteId;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            $("#ctl00_ContentPlaceHolder1_divLocalAlertsTableView").show();

            loadAlertsServiceTableView(siteVal);
        }

        $(document).on('change', '#ctl00_headerBanner_drpsitelist', function () {
            if ($('#ctl00_ContentPlaceHolder1_divLocalAlerts').is(":visible")) {
                $('#ctl00_ContentPlaceHolder1_divLocalAlerts').hide();
                $('#ctl00_ContentPlaceHolder1_divLocalAlertsTableView').show();
            }

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            loadAlertsServiceTableView(siteVal);
        });

        function redirectToHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
            location.href = "Home.aspx?sid=" + GSiteId;
        }

        var g_SiteId;
        var g_DateRngType = 1;
        var g_DateTagRngType = 1;
        var g_DateMonitorRngType = 1;
        var g_DateStarRngType = 1;
        var g_CurrPg = 1;
        var g_Date;
        var g_DefaultDate = '';

        function PaginationLocalAlerts(PgType, GraphDeviceType) {
            var AlertGraphType = 0;
            var RngType = 1;
            var sdate;

            var DeviceId = setundefined(document.getElementById("txtDeviceId_DeviceDetails").value);

            if (GraphDeviceType == 4) {
                sdate = document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAServiceAlerts").value;
                AlertGraphType = 4;
                RngType = g_DateRngType;
                document.getElementById("divLoading_LocalAlerts").style.display = "";
            }
            else if (GraphDeviceType == 1) {
                sdate = document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LATag").value;
                AlertGraphType = 1;
                RngType = g_DateTagRngType;
                document.getElementById("divLoading_TagAlerts").style.display = "";
            }
            else if (GraphDeviceType == 2) {
                sdate = document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAMonitor").value;
                AlertGraphType = 2;
                RngType = g_DateMonitorRngType;
                document.getElementById("divLoading_MonitorAlerts").style.display = "";
            }
            else if (GraphDeviceType == 3) {
                sdate = document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAStar").value;
                AlertGraphType = 3;
                RngType = g_DateStarRngType;
                document.getElementById("divLoading_StarAlerts").style.display = "";
            }

            LoadLocalAlertsForAllService(g_SiteId, RngType, g_CurrPg, sdate, PgType, AlertGraphType, "All", "", DeviceId, false);
        }

        function setRangeLable(lbl, type) {
            if (type == 1)
                document.getElementById(lbl).innerHTML = "Hourly ";
            else if (type == 2)
                document.getElementById(lbl).innerHTML = " Daily ";
            else if (type == 3)
                document.getElementById(lbl).innerHTML = "Weekly ";
            else if (type == 4)
                document.getElementById(lbl).innerHTML = "Monthly";
        }

        function DateRangeTypeLocalAlerts(RngType, GraphDeviceType) {
            var AlertGraphType = 0;
            var DeviceId = setundefined(document.getElementById("txtDeviceId_DeviceDetails").value);

            if (GraphDeviceType == 4) {
                AlertGraphType = 4;
                g_DateRngType = RngType;
                document.getElementById("divLoading_LocalAlerts").style.display = "";
                setRangeLable("lblDateType_LAServiceAlerts", RngType)
            }
            else if (GraphDeviceType == 1) {
                AlertGraphType = 1;
                g_DateTagRngType = RngType;
                document.getElementById("divLoading_TagAlerts").style.display = "";
                setRangeLable("lblDateType_LATag", RngType)
            }
            else if (GraphDeviceType == 2) {
                AlertGraphType = 2;
                g_DateMonitorRngType = RngType;
                document.getElementById("divLoading_MonitorAlerts").style.display = "";
                setRangeLable("lblDateType_LAMonitor", RngType)
            }
            else if (GraphDeviceType == 3) {
                AlertGraphType = 3;
                g_DateStarRngType = RngType;
                document.getElementById("divLoading_StarAlerts").style.display = "";
                setRangeLable("lblDateType_LAStar", RngType)
            }

            LoadLocalAlertsForAllService(g_SiteId, RngType, g_CurrPg, g_DefaultDate, 1, AlertGraphType, "All", "", DeviceId, false);
        }

        function disableEnableOnDeviceTypeChange(drpCtrl) {
            if (drpCtrl.value == 4) {
                document.getElementById("txtDeviceId_DeviceDetails").value = "";
                document.getElementById("txtDeviceId_DeviceDetails").disabled = true;
            }
            else {
                document.getElementById("txtDeviceId_DeviceDetails").disabled = false;
            }
        }

        function disableEnableOnDeviceTypeChangeTableView(drpCtrl) {
            if (drpCtrl.value == 0) {
                document.getElementById("lblAlertType").display = "none";
                document.getElementById("drpAlertType").display = "none";
            }
            else if (drpCtrl.value == 1) {
                document.getElementById("lblAlertType").display = "";
                document.getElementById("drpAlertType").display = "";


            }
            else if (drpCtrl.value == 2) {
                document.getElementById("lblAlertType").display = "";
                document.getElementById("drpAlertType").display = "";
            }
            else if (drpCtrl.value == 3) {
                document.getElementById("lblAlertType").display = "";
                document.getElementById("drpAlertType").display = "";
            }
            else if (drpCtrl.value == 4) {
                document.getElementById("lblAlertType").display = "";
                document.getElementById("drpAlertType").display = "";
            }
        }

        function ShowLocalAlerts() {
            var RngType = 1;
            var DeviceId = setundefined(document.getElementById("txtDeviceId_DeviceDetails").value);
            var dtIdx = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails").selectedIndex;
            var dtVal = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails").options[dtIdx].value;

            if (DeviceId != "") {
                if (dtVal <= 0) {
                    alert("Select Device Type!!!");
                    document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails").focus();
                    return false;
                }
            }

            if (dtVal == 4) {
                RngType = g_DateRngType;
                document.getElementById("divLoading_LocalAlerts").style.display = "";
            }
            else if (dtVal == 1) {
                RngType = g_DateTagRngType;
                document.getElementById("divLoading_TagAlerts").style.display = "";
            }
            else if (dtVal == 2) {
                RngType = g_DateMonitorRngType;
                document.getElementById("divLoading_MonitorAlerts").style.display = "";
            }
            else if (dtVal == 3) {
                RngType = g_DateStarRngType;
                document.getElementById("divLoading_StarAlerts").style.display = "";
            }

            LoadLocalAlertsForAllService(g_SiteId, RngType, g_CurrPg, g_DefaultDate, 1, dtVal, "All", "", DeviceId, false);
        }

        function showLocalAlertContent(siteId, nGrphType) {
            $('#ctl00_ContentPlaceHolder1_divHome').hide('slide', { direction: 'left' }, 100);

            $('#ctl00_ContentPlaceHolder1_divLocalAlerts').show('slide', { direction: 'right' }, 600);

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteId;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName_LocalAlerts").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails").selectedIndex = 0;

            g_Date = document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAServiceAlerts").value;
            if (g_DefaultDate == '')
                g_DefaultDate = g_Date;
            g_SiteId = siteId;

            document.getElementById("divGraph_LocalAlerts_Service").innerHTML = "";
            document.getElementById("divGraph_LocalAlerts_Tag").innerHTML = "";
            document.getElementById("divGraph_LocalAlerts_Monitor").innerHTML = "";
            document.getElementById("divGraph_LocalAlerts_Star").innerHTML = "";

            document.getElementById("divLoading_LocalAlerts").style.display = "";
            document.getElementById("divLoading_TagAlerts").style.display = "";
            document.getElementById("divLoading_MonitorAlerts").style.display = "";
            document.getElementById("divLoading_StarAlerts").style.display = "";

            g_DateRngType = 1;

            LoadLocalAlertsForAllService(siteId, g_DateRngType, g_CurrPg, g_DefaultDate, 1, nGrphType, "All", "", "", true);
        }

        //Clear Alert Graph
        function clearLAGraph() {
            document.getElementById("divGraph_LocalAlerts_Service").innerHTML = "";
            document.getElementById("divGraph_LocalAlerts_Tag").innerHTML = "";
            document.getElementById("divGraph_LocalAlerts_Monitor").innerHTML = "";
            document.getElementById("divGraph_LocalAlerts_Star").innerHTML = "";
            g_DateRngType = 1;
            document.getElementById("lblDateType_LAServiceAlerts").innerHTML = "Hourly ";
            document.getElementById("lblDateType_LATag").innerHTML = "Hourly ";
            document.getElementById("lblDateType_LAMonitor").innerHTML = "Hourly ";
            document.getElementById("lblDateType_LAStar").innerHTML = "Hourly ";

            document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAServiceAlerts").value = g_DefaultDate;
            document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LATag").value = g_DefaultDate;
            document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAMonitor").value = g_DefaultDate;
            document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAStar").value = g_DefaultDate;

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblLA_TableView');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblLA_TableView');
            }
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_TableView").value = 1;

            $("#drpAlertType").empty();
            $("#drpAlertType").multipleSelect("refresh");
            $("#ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetailsTableView").val("");

            $("#txtDeviceId_TableView").val("");
            $("#txtDeviceId_TableView").prop('disabled', true);

            g_GraphicalViewLoaded = false;
            g_TableViewLoaded = false;
        }

        var g_SortAlerts = "";
        var g_SortColumn = "";
        var g_SortOrder = "";
        var g_SortImg = "";
        var g_GraphicalViewLoaded = false;
        var g_TableViewLoaded = false;

        function DisplayLocalAlertsView(viewType) {
            if ($('#lblStatus').html() == "") {
                var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
                var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
                var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

                $("#ctl00_ContentPlaceHolder1_lblSiteName_LocalAlertsTableView")[0].innerHTML = siteText;

                if (viewType === 1) {
                    $("#ctl00_ContentPlaceHolder1_divLocalAlertsTableView").hide();
                    $("#ctl00_ContentPlaceHolder1_divLocalAlerts").show();

                    if (g_GraphicalViewLoaded == false) {
                        showLocalAlertContent(siteVal, "0");
                        g_GraphicalViewLoaded = true;
                    }
                } else if (viewType === 2) {
                    $("#ctl00_ContentPlaceHolder1_divLocalAlerts").hide();
                    $("#ctl00_ContentPlaceHolder1_divLocalAlertsTableView").show();

                    if (g_TableViewLoaded == false) {
                        AlertsServiceTableView("1");
                        g_TableViewLoaded = true;
                    }
                }
            }
        }

        function sortAlerts(sortCol) {
            if (g_SortColumn != sortCol) {
                g_SortOrder = " desc";
                g_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
            }
            else {
                if (g_SortOrder == " desc") {
                    g_SortOrder = " asc";
                    g_SortImg = "<image src='Images/uparrow.png' valign='middle' />";
                }
                else if (g_SortOrder == " asc") {
                    g_SortOrder = " desc";
                    g_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
                }
            }

            if (sortCol != "") {
                g_SortColumn = sortCol;
            }

            g_SortAlerts = g_SortColumn + g_SortOrder;
            AlertsServiceTableView("1");
        }

        function loadAlertsServiceTableView(siteId) {
            isButtonClicked = 1;

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteId;

            g_SortAlerts = "AlertedOn desc";
            g_SortColumn = "AlertedOn";
            g_SortOrder = " desc";
            g_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
            g_TableViewLoaded = true;
            $("#txtDeviceId_TableView").prop('disabled', true);

            AlertsServiceTableView("1");
        }

        function AlertsServiceTableView(pgType) {
            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
            var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            var CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_TableView").value;

            $("#ctl00_ContentPlaceHolder1_lblSiteName_LocalAlertsTableView")[0].innerHTML = siteText;

            if (pgType == "show") {
                CurrPg = 1;
                g_SortAlerts = "AlertedOn desc";
                g_SortColumn = "AlertedOn";
                g_SortOrder = " desc";
            }

            if (CurrPg == "")
                CurrPg = 0;

            if (pgType == 2) {
                CurrPg = parseInt(CurrPg) + 1;
            } else if (pgType == 3) {
                CurrPg = parseInt(CurrPg) - 1;
            }

            document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_TableView").value = CurrPg;
            LoadAlertsServiceTableView(siteVal, CurrPg, g_SortAlerts, $('#date_timepicker_start').val(), $('#date_timepicker_end').val(), $('#drpAlertType').val(), $('#txtDeviceId_TableView').val());
        }

        var enumTagArr = { "Tag not reporting / missing (Regular Tag)": 1, "Tag not reporting / missing (No Sleep Tag)": 2, "IR Profile conflict: All tags are not in same IR Profile": 29, "Active Tags Count less than the allowed percentage": 48 };
        var enumMonitorArr = { "Monitor not reporting / missing": 3, "Monitor not seen by any Tags": 4, "Monitor Reports too many times due to Wakeup / Paging": 30, "DIM Reports too many times due to Wakeup / Paging": 31, "DIM Reports too many times due to Trigger": 32, "Active Monitor Count less than the allowed percentage": 49 };
        var enumStarArr = { "Star does not see any Devices": 5, "Star does not associated with any Devices": 6, "Star is InActive": 7, "Star exits ethernet offset over limit": 8, "Star non sync with Timeserver": 9, "Star association changed frequently": 10, "One or more Stars in the network is in NON SYNC": 11, "Beacon Star inactive": 44, "All Stars are inactive": 45, "Active Star Count less than the allowed percentage": 50 };
        var enumTimeServerArr = { "Master Timing Star is not reporting": 12, "Slave Timing Star is not reporting": 13 };
        var enumBackupUtilityArr = { "BackupUtility is not able to access FTP": 14, "BackupUtility is not running / stopped": 15 };
        var enumStreamingServerArr = { "Streaming Server is not running / stopped": 16, "Delay in Streaming": 26, "Service restarted.  All pending alerts are cleared": 61, "Streaming server stopped manually": 69 };
        var enumPagingServerArr = { "Paging Server is not running / stopped": 17, "Paging Server is running but not receiving data": 18, "Service restarted.  All pending alerts are cleared": 60, "Paging server stopped manually": 70 };
        var enumLocationServerArr = { "Location Server is not running / stopped": 19, "Location Server is running but not receiving data": 20, "Service restarted.  All pending alerts are cleared": 59, "Paging server stopped manually": 71 };
        var enumStarLogArr = { "Star is in continuous Discovery": 21, "Star is in continuous TCP Failure": 22, "Star is in Beacon Failure": 23, "Star didn't get any response from Time Server": 24, "Star is in Beacon Search": 25 };
        var enumPCServerArr = { "PC Server is not running / stopped": 27, "PC Server is running but not receiving data": 28, "Packets in queue exceeds the limit": 46, "Socket value exceeds the limit": 47 };
        var enumRaulandArr = { "Rauland Connector Service not able to connect DB": 33, "Rauland Connector Service TCP Error": 34, "Rauland Connector Service is not running / stopped": 39, "Rauland Connector Service is running but not receiving any data": 40, "Failed to connect database": 55, "No clients are connected": 56, "Delay in streaming": 58, "Service restarted.  All pending alerts are cleared": 63, "Rauland connector service stopped manually": 72 };
        var enumWifiConnArr = { "Wifi Connector failed to Login into MSE": 35, "Wifi Connector failed to get Tag count from MSE": 36, "Wifi Connector failed to get Tag Information from MSE": 37, "Wifi Connector failed to get network design from MSE": 38, "WIFI Connector Service is not running": 41, "WIFI Connector Service is running but not receiving any data": 42, "Wifi Connector failed to get client count from MSE": 57, "Service restarted.  All pending alerts are cleared": 64, "Wifi connector manually stopped": 76 };
        var enumUCSConnArr = { "Service restarted.  All pending alerts are cleared": 65, "Universal connector service is not running": 73, "Universal connector failed to bind": 74, "Universal connector manually stopped": 75 };
        var enumGMSServerArr = { "Upload Server Status failed": 54, "Failed to get site name from GMS": 62 };
        var enumGMSServcieArr = { "GMS Service is not running": 51, "Failed to get SiteId from GMS": 52, "Upload Service Status failed": 53 };
        var enumHL7ConnArr = { "Universal Connector Service failed to connect Database": 66, "Universal Connector Service failed to connect listener": 67, "Universal Connector Service failed to parse HL7 Message": 68, "HL7 Connector Service is running but not receiving any data": 77 };
        var enumINIArr = { "Stars are not defined in INI File": 43 };
        var enumHeartBeatArr = { "Heart beat signal is stopped for more than 10 mins.": 78 };

        $(document).on('change', "#ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetailsTableView", function () {
            $select = $("#drpAlertType");
            $select.empty();

            $("#txtDeviceId_TableView").prop('disabled', true);
            $("#txtDeviceId_TableView").val("");

            if ($(this).val() == 1 || $(this).val() == 2 || $(this).val() == 3)
                $("#txtDeviceId_TableView").prop('disabled', false);

            if ($(this).val() == 1)
                $.fn.appendArr($select, enumTagArr);
            else if ($(this).val() == 2)
                $.fn.appendArr($select, enumMonitorArr);
            else if ($(this).val() == 3)
                $.fn.appendArr($select, enumStarArr);
            else if ($(this).val() == 4)
                $.fn.appendArr($select, enumTimeServerArr);
            else if ($(this).val() == 5)
                $.fn.appendArr($select, enumBackupUtilityArr);
            else if ($(this).val() == 6)
                $.fn.appendArr($select, enumStreamingServerArr);
            else if ($(this).val() == 7)
                $.fn.appendArr($select, enumPagingServerArr);
            else if ($(this).val() == 8)
                $.fn.appendArr($select, enumLocationServerArr);
            else if ($(this).val() == 9)
                $.fn.appendArr($select, enumStarLogArr);
            else if ($(this).val() == 10)
                $.fn.appendArr($select, enumPCServerArr);
            else if ($(this).val() == 11)
                $.fn.appendArr($select, enumRaulandArr);
            else if ($(this).val() == 12)
                $.fn.appendArr($select, enumWifiConnArr);
            else if ($(this).val() == 15)
                $.fn.appendArr($select, enumUCSConnArr);
            else if ($(this).val() == 16)
                $.fn.appendArr($select, enumGMSServerArr);
            else if ($(this).val() == 17)
                $.fn.appendArr($select, enumGMSServcieArr);
            else if ($(this).val() == 18)
                $.fn.appendArr($select, enumHL7ConnArr);
            else if ($(this).val() == 19)
                $.fn.appendArr($select, enumINIArr);
            else if ($(this).val() == 20)
                $.fn.appendArr($select, enumHeartBeatArr);

            //  $('#drpAlertType').multipleSelect();
            $('#drpAlertType').multipleSelect('refresh');
            $('#drpAlertType').multipleSelect('checkAll');
        });

        $.fn.appendArr = function ($select, $enumArr) {
            $.each($enumArr, function (key, value) {
                $select.append($("<option>", { "value": value, "text": key, "selected": "selected" }));
            });
        }

        function showTip3(text, lf, tp) {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip3").css({ left: tempX, top: tempY });
        }

        function hideTip3() {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.display = 'none';
        }
    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <input type="hidden" id="hdSiteId" runat="server" />
                <input type="hidden" id="hid_userrole" runat="server" />
                <!-- LOCAL ALERTS -->
                <div id="divLocalAlerts" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 85%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr style="height: 20px;">
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="redirectToHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <asp:label id="lblSiteName_LocalAlerts" runat="server"></asp:label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0'>
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Local Alerts
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        <img src="Images/imgTextview.png" width="28px" height="24px" />
                                                        <br />
                                                        <label class="clsLALabel" onclick="DisplayLocalAlertsView(2);" style="cursor: pointer;">
                                                            &nbsp;&nbsp;Table&nbsp;View&nbsp;&nbsp;</label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 15px; padding-right: 15px;">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #F0EDED;">
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px;">
                                            <b>Device&nbsp;Type&nbsp;:&nbsp;</b>
                                            <select id="ddlDeviceType_DeviceDetails" runat="server" style="width: 120px;" onchange="disableEnableOnDeviceTypeChange(this);">
                                            </select>
                                        </td>
                                        <td class="clsLALabel">
                                            <b>Device&nbsp;Id&nbsp;:&nbsp;</b>
                                            <input type="text" id="txtDeviceId_DeviceDetails" />
                                        </td>
                                        <td align="right" style="padding-right: 5px;">
                                            <input type="button" id="btnShow_DeviceDetails" class="clsExportExcel" value=" Show "
                                                onclick="return ShowLocalAlerts();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr id="trLocalServices">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <!-- PREVIOUS/NEXT -->
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="250px">
                                                            <tr>
                                                                <td class="subHeader_black" style="padding-left: 15px;">
                                                                    Local Services
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <div style="position: relative; display: none;" id="divLoading_LocalAlerts">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td align="right" style="padding-right: 15px;">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="330px" style="background-color: #F0EDED;">
                                                            <tr style="height: 36px;">
                                                                <td style="width: 10px;">
                                                                </td>
                                                                <td align="left">
                                                                    <img src="images/btnLAAlertPrev.png" id="btnPrev_LAServiceAlerts" width="30px" height="30px"
                                                                        onclick="PaginationLocalAlerts(3,4);" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td style="width: 15px;">
                                                                    <input type="text" id="txtDate_LAServiceAlerts" class="clsLATextbox" runat="server"
                                                                        readonly />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <label id="lblDateType_LAServiceAlerts" class="clsLALabel" style="width: 150px;" />
                                                                    Hourly</label>
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsMonthly.png" id="btnMonthly_LAServiceAlerts" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(4,4);" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsWeekly.png" id="Img1" width="30px" height="30px" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" onclick="DateRangeTypeLocalAlerts(3,4);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsDaily.png" id="btnDaily_LAServiceAlerts" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(2,4);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsHourly.png" id="btnHourly_LAServiceAlerts" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(1,4);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertNext.png" id="btnNext_ServiceAlerts" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="PaginationLocalAlerts(2,4);" />
                                                                </td>
                                                                <td style="width: 10px;">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divGraph_LocalAlerts_Service">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr id="trTag_Alerts">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <!-- PREVIOUS/NEXT -->
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="250px">
                                                            <tr>
                                                                <td class="subHeader_black" style="padding-left: 15px;">
                                                                    Tag
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <div style="position: relative; display: none;" id="divLoading_TagAlerts">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td align="right" style="padding-right: 15px;">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="330px" style="background-color: #F0EDED;">
                                                            <tr style="height: 36px;">
                                                                <td style="width: 10px;">
                                                                </td>
                                                                <td align="left">
                                                                    <img src="images/btnLAAlertPrev.png" id="btnPrev_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="PaginationLocalAlerts(3,1);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td style="width: 15px;">
                                                                    <input type="text" id="txtDate_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" class="clsLATextbox" runat="server"
                                                                        readonly />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <label id="lblDateType_LATag" onmouseover="btnPrev_LAServiceOver(this);" onmouseout="btnPrev_LAServiceOut(this);"
                                                                        class="clsLALabel" />
                                                                    Hourly</label>
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsMonthly.png" id="btnMonthly_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(4,1);" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsWeekly.png" id="btnWeekly_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(3,1);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsDaily.png" id="btnDaily_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(2,1);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsHourly.png" id="btnHourly_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(1,1);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertNext.png" id="btnNext_LATag" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="PaginationLocalAlerts(2,1);" />
                                                                </td>
                                                                <td style="width: 10px;">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divGraph_LocalAlerts_Tag">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr id="trMonitor_Alerts">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <!-- PREVIOUS/NEXT -->
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="250px">
                                                            <tr>
                                                                <td class="subHeader_black" style="padding-left: 15px;">
                                                                    Monitor
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <div style="position: relative; display: none;" id="divLoading_MonitorAlerts">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td align="right" style="padding-right: 15px;">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="330px" style="background-color: #F0EDED;">
                                                            <tr style="height: 36px;">
                                                                <td style="width: 10px;">
                                                                </td>
                                                                <td align="left">
                                                                    <img src="images/btnLAAlertPrev.png" id="btnPrev_LAMonitor" width="30px" height="30px"
                                                                        onclick="PaginationLocalAlerts(3,2);" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td style="width: 15px;">
                                                                    <input type="text" id="txtDate_LAMonitor" class="clsLATextbox" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" runat="server" readonly />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <label id="lblDateType_LAMonitor" onmouseover="btnPrev_LAServiceOver(this);" onmouseout="btnPrev_LAServiceOut(this);"
                                                                        class="clsLALabel" />
                                                                    Hourly</label>
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsMonthly.png" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" id="btnMonthly_LAMonitor" width="30px"
                                                                        height="30px" onclick="DateRangeTypeLocalAlerts(4,2);" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsWeekly.png" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" id="btnWeekly_LAMonitor" width="30px"
                                                                        height="30px" onclick="DateRangeTypeLocalAlerts(3,2);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsDaily.png" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" id="btnDaily_LAMonitor" width="30px"
                                                                        height="30px" onclick="DateRangeTypeLocalAlerts(2,2);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsHourly.png" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" id="btnHourly_LAMonitor" width="30px"
                                                                        height="30px" onclick="DateRangeTypeLocalAlerts(1,2);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertNext.png" onmouseover="btnPrev_LAServiceOver(this);" onmouseout="btnPrev_LAServiceOut(this);"
                                                                        id="btnNext_LAMonitor" width="30px" height="30px" onclick="PaginationLocalAlerts(2,2);" />
                                                                </td>
                                                                <td style="width: 10px;">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divGraph_LocalAlerts_Monitor">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr id="trStar_Alerts">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <!-- PREVIOUS/NEXT -->
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="250px">
                                                            <tr>
                                                                <td class="subHeader_black" style="padding-left: 15px;">
                                                                    Star
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <div style="position: relative; display: none;" id="divLoading_StarAlerts">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td align="right" style="padding-right: 15px;">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="330px" style="background-color: #F0EDED;">
                                                            <tr style="height: 36px;">
                                                                <td style="width: 10px;">
                                                                </td>
                                                                <td align="left">
                                                                    <img src="images/btnLAAlertPrev.png" id="btnPrev_LAStar" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="PaginationLocalAlerts(3,3);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td style="width: 15px;">
                                                                    <input type="text" id="txtDate_LAStar" class="clsLATextbox" runat="server" readonly />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <label id="lblDateType_LAStar" class="clsLALabel" />
                                                                    Hourly</label>
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsMonthly.png" id="btnMonthly_LAStar" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(4,3);" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsWeekly.png" id="btnWeekly_LAStar" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(3,3);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsDaily.png" id="btnDaily_LAStar" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(2,3);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsHourly.png" id="btnHourly_LAStar" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="DateRangeTypeLocalAlerts(1,3);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertNext.png" id="btnNext_LAStar" onmouseover="btnPrev_LAServiceOver(this);"
                                                                        onmouseout="btnPrev_LAServiceOut(this);" width="30px" height="30px" onclick="PaginationLocalAlerts(2,3);" />
                                                                </td>
                                                                <td style="width: 10px;">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divGraph_LocalAlerts_Star">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- LOCAL ALERTS TABLE VIEW -->
                <div id="divLocalAlertsTableView" runat="server" style="display: none; top: auto;
                    left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 85%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr style="height: 20px;">
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="redirectToHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <asp:label id="lblSiteName_LocalAlertsTableView" runat="server"></asp:label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Local Alerts
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        <img src="Images/imgGraphicview.png" width="30px" height="23px" />
                                                        <br />
                                                        <label class="clsLALabel" onclick="DisplayLocalAlertsView(1);" style="cursor: pointer;">
                                                            Graphical&nbsp;View</label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <label id="lblStatus" class="sText" style="color: #646464;" />
                            </td>
                        </tr>
                        <tr id="trFilter">
                            <td>
                                <table border="0" cellpadding="2" cellspacing="2" width="100%" class="clsFilterTable">
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px;">
                                            <b>Device&nbsp;:&nbsp;</b>
                                        </td>
                                        <td>
                                            <select id="ddlDeviceType_DeviceDetailsTableView" runat="server" style="width: 150px;"
                                                onchange="disableEnableOnDeviceTypeChangeTableView(this);">
                                            </select>
                                        </td>
                                        <td class="clsLALabel" style="padding-left: 5px;">
                                            <label id="lblAlertType">
                                                <b>Alert&nbsp;:&nbsp;</b></label>
                                        </td>
                                        <td colspan="2">
                                            <select multiple="multiple" id="drpAlertType" style="width: 320px;">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel">
                                            <b>Start&nbsp;Date&nbsp;:&nbsp;</b>
                                        </td>
                                        <td>
                                            <input type="text" id="date_timepicker_start" style="width: 125px;" />
                                        </td>
                                        <td class="clsLALabel">
                                            <b>End&nbsp;Date&nbsp;:&nbsp;</b>
                                        </td>
                                        <td>
                                            <input type="text" id="date_timepicker_end" style="width: 125px;" />
                                        </td>
                                        <td align="right" style="padding-right: 5px;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel">
                                            <b>Device&nbsp;Id&nbsp;:&nbsp;</b>
                                        </td>
                                        <td>
                                            <input type="text" id="txtDeviceId_TableView" style="width: 125px;" />
                                        </td>
                                        <td class="clsLALabel">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="right" style="padding-right: 5px;">
                                            <input type="button" id="btnShowTableView" class="clsExportExcel" value=" Show "
                                                onclick="AlertsServiceTableView('show')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trLoalServicesTableView">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <!-- PREVIOUS/NEXT -->
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td align="right">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr style="height: 40px;">
                                                                <td class="txttotalpage" style="width: 275px;" valign="middle">
                                                                    <asp:label id="lblCount_TableView" runat="server"></asp:label>
                                                                </td>
                                                                <td>
                                                                    <div style="display: none;" id="divLoading_TableView">
                                                                        <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                                    </div>
                                                                </td>
                                                                <td class="clsTableTitleText" align="right">
                                                                    <input type="button" id="btnPrev_TableView" class="clsPrev" onclick="AlertsServiceTableView('3');"
                                                                        title="Previous" />
                                                                    <asp:label id="lblPg_TableView" runat="server" cssclass="clsCntrlTxt"> Page </asp:label>
                                                                    <input id="txtPgNo_TableView" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                                        type="text" size="1" maxlength="10" runat="server" name="txtPageNo" value="1" />
                                                                    <asp:label id="lblPgCnt_TableView" runat="server" cssclass="clsCntrlTxt">&nbsp;</asp:label>&nbsp;
                                                                    <input type="button" id="btnGo_TableView" class="btnGO" value="Go" onclick="AlertsServiceTableView('1');" />&nbsp;&nbsp;
                                                                    <input type="button" id="btnNext_TableView" class="clsNext" onclick="AlertsServiceTableView('2');"
                                                                        title="Next" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%" id="tblLA_TableView">
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
        <script type="text/javascript">

            $('#datetimepicker').datetimepicker()
	            .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });

            $(function () {
                $('#date_timepicker_start').datetimepicker({
                    format: 'Y/m/d H:i',
                    step: 10,
                    timepicker: true
                });

                $('#date_timepicker_end').datetimepicker({
                    format: 'Y/m/d H:i',
                    step: 10,
                    timepicker: true
                });
            });
	
        </script>
        <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script>
        <script type="text/javascript">
            $('#drpAlertType').multipleSelect();
        </script>
        <script type="text/javascript">
            LoadGlossaryInfo("Home", document.getElementById("<%=hid_userrole.ClientID%>").value);
            showGlossaryInfo("LocalAlerts");
        </script>
    </table>
</asp:content>
