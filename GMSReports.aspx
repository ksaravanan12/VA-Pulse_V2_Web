<%@ Page Title="Connect Pulse Reports" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="GMSReports.aspx.vb" Inherits="GMSUI.GMSReports" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />

    <script type="text/javascript">

        var GSiteId = 0;
        var UserRoleId = 0;

        this.onload = function () {

            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            UserRoleId = document.getElementById('ctl00_ContentPlaceHolder1_hdnUserRole').value;

            if (UserRoleId == enumUserRoleArr.Admin || UserRoleId == enumUserRoleArr.Engineering || UserRoleId == enumUserRoleArr.Support) {

                if (UserRoleId == enumUserRoleArr.Admin) {

                    document.getElementById('trLogViewerHeader').style.display = "";
                    document.getElementById('trLogViewer').style.display = "";

                    document.getElementById('trCentrakDevicesHeader').style.display = "";
                    document.getElementById('trCentrakDevicesReports').style.display = "";

                    document.getElementById('trOutboundTrackingReportHeader').style.display = "";
                    document.getElementById('trOutboundTrackingReport').style.display = "";
                }                            

                document.getElementById('trAnalysis').style.display = "";
                document.getElementById('trAnalysisReport').style.display = "";

                document.getElementById('trBatteryReplacementFailure').style.display = "";
                //document.getElementById('trMobileAuditLog').style.display = "";
                document.getElementById('trMonitorAnalysis').style.display = "";
                document.getElementById('trStarAnalysis').style.display = "";
                document.getElementById('trConnectivityReport').style.display = "";
                document.getElementById('trDeviceDetailReport').style.display = "";
                document.getElementById('trMonitorGroup').style.display = "";             
                document.getElementById('trLocationHistoryReport').style.display = "";

                document.getElementById('trEMHeader').style.display = "";
                document.getElementById('trEMReport').style.display = "";

                document.getElementById('trHealthHeader').style.display = "";
                document.getElementById('trHealthReport').style.display = "";

                document.getElementById('trDataStreamsHeader').style.display = "";
                document.getElementById('trDataStreamsReport').style.display = "";
                document.getElementById('trHistoricalTemperatureReport').style.display = "";
            }
            else {

                LoadPulseReports();

                if (UserRoleId == enumUserRoleArr.Maintenance || UserRoleId == enumUserRoleArr.Partner || UserRoleId == enumUserRoleArr.TechnicalAdmin) {

                    document.getElementById('trAnalysis').style.display = "";
                    document.getElementById('trAnalysisReport').style.display = "";

                    document.getElementById('trBatteryReplacementFailure').style.display = "";
                    //document.getElementById('trMobileAuditLog').style.display = "";
                }
            }
        }

        function redirectToHome() {

            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }

            location.href = "Home.aspx?sid=" + GSiteId;
        }

        var Report_Obj;

        function LoadPulseReports() {

            Report_Obj = CreateXMLObj();

            if (Report_Obj != null) {

                Report_Obj.onreadystatechange = ajaxPulseReports;

                DbConnectorPath = "AjaxConnector.aspx?cmd=GetUserPulseReport";

                if (GetBrowserType() == "isIE") {
                    inf_Obj.open("GET", DbConnectorPath, true);
                }
                else if (GetBrowserType() == "isFF") {
                    Report_Obj.open("GET", DbConnectorPath, true);
                }

                Report_Obj.send(null);
            }
            return false;
        }

        function ajaxPulseReports() {

            if (Report_Obj.readyState == 4) {

                if (Report_Obj.status == 200) {

                    //Ajax Msg Receiver
                    AjaxMsgReceiver(Report_Obj.responseXML.documentElement);

                    var dsRoot = Report_Obj.responseXML.documentElement;

                    var o_PulseReportIds = dsRoot.getElementsByTagName('PulseReportIds');

                    nRootLength = o_PulseReportIds.length;

                    if (nRootLength > 0) {

                        PulseReportIds = (o_PulseReportIds[0].textContent || o_PulseReportIds[0].innerText || o_PulseReportIds[0].text);

                        var arrPulseReportId = PulseReportIds.split(',');

                        if ($.inArray("1", arrPulseReportId) != -1) {

                            document.getElementById('trAnalysis').style.display = "";
                            document.getElementById('trAnalysisReport').style.display = "";
                            document.getElementById('trLocationHistoryReport').style.display = "";
                        }
                    }
                }
            }
        }

    </script>

    <input type="hidden" id="hdnUserRole" runat="server" />
    <table border="0" cellpadding="0" cellspacing="0" width="92%">
        <tr style="height: 20px;">
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
                                        Connect Pulse Reports
                                    </td>
                                </tr>
                                <tr>
                                    <td align='left' class='subHeader_black'>
                                        Report List
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 75px;" align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                    </tr>
                    <tr style="height: 5px;">
                        <td class="bordertop" valign="top" colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trSettings">
            <td valign="top" style="padding-left: 30px;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr id="trAnalysis" style="display: none;">
                        <td class="report_text">
                            Analysis
                        </td>
                    </tr>
                    <tr id="trAnalysisReport" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr id="trMonitorAnalysis" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="GMSReportDetails.aspx?ReportType=1">Monitor Analysis
                                            Report</a>
                                    </td>
                                </tr>
                                <tr id="trStarAnalysis" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="GMSReportDetails.aspx?ReportType=3">Star Analysis
                                            Report</a>
                                    </td>
                                </tr>
                                <tr id="trConnectivityReport" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="GMSReportDetails.aspx?ReportType=4">Connectivity
                                            Report For Devices</a>
                                    </td>
                                </tr>
                                <tr id="trBatteryReplacementFailure" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="BatteryReplacementFailureReport.aspx">Battery Replacement
                                            Failure Report</a>
                                    </td>
                                </tr>
                                <tr id="trMobileAuditLog" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="CenTrakVoltReport.aspx">Connect Pulse Mobile Audit
                                            Report</a>
                                    </td>
                                </tr>
                                <tr id="trDeviceDetailReport" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="DeviceDetailsReport.aspx">Device Details Report</a>
                                    </td>
                                </tr>
                                <tr id="trMonitorGroup" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="MonitorGroupReport.aspx">Monitor Group Report</a>
                                    </td>
                                </tr>
                                <tr id="trLocationHistoryReport" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="LocationHistoryreport.aspx">Location History Report</a>
                                    </td>
                                </tr>
                                <tr id="trHistoricalTemperatureReport" style="display: none;">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="HistoricalTemperature.aspx">Historical Temperature
                                            Report</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trLogViewerHeader" style="display: none;">
                        <td class="report_text">
                            Log Viewer
                        </td>
                    </tr>
                    <tr id="trLogViewer" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr id="trRawReport" runat="server">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="Logfiles.aspx">Tag, Monitor Raw Data Report</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trEMHeader" style="display: none;">
                        <td class='report_text'>
                            EM
                        </td>
                    </tr>
                    <tr id="trEMReport" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="EMTagReport.aspx?qEMReportType=4">EM Tags Report</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="EMTagReport.aspx?qEMReportType=2">EM Display Tag
                                            Summary Report</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="EMTagReport.aspx?qEMReportType=3">EM Display Tag
                                            Detailed Site Report</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="EMRMAreport.aspx">EM Display Sensors Requiring RMA
                                            report</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trHealthHeader" style="display: none;">
                        <td class='report_text'>
                            Health
                        </td>
                    </tr>
                    <tr id="trHealthReport" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="HealthReport.aspx?qRptType=1">Tag and Infrastructure
                                            Health Overview Report</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="HealthReport.aspx?qRptType=2">Tag and Infrastructure
                                            Health Detailed Site Report</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trDataStreamsHeader" style="display: none;">
                        <td class='report_text'>
                            Data Streams
                        </td>
                    </tr>
                    <tr id="trDataStreamsReport" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr id="trStreamingFields">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="StreamingFields.aspx">Streaming Fields</a>
                                    </td>
                                </tr>
                                <tr id="trMSESettings">
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="MSESettings.aspx">MSE Settings</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trCentrakDevicesHeader" style="display: none;">
                        <td class='report_text'>
                            Centrak Devices
                        </td>
                    </tr>
                    <tr id="trCentrakDevicesReports" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="CentrakDevices.aspx">Centrak Device Report</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trOutboundTrackingReportHeader" style="display: none;">
                        <td class='report_text'>
                            Tracking Report
                        </td>
                    </tr>
                    <tr id="trOutboundTrackingReport" style="display: none;">
                        <td style="padding-left: 10px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td style="width: 10px;" align="center">
                                        <div class='rmaBlueCircle'>
                                        </div>
                                    </td>
                                    <td align='left' class='subHeader_black'>
                                        <a class='alert_normal_Blue' href="OutboundTrackingReport.aspx">Outbound Tracking Report</a>
                                    </td>
                                </tr>
                                <tr style="height: 10px">
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:content>
