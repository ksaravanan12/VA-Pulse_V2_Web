<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AlertMaintenance.aspx.vb" Inherits="GMSUI.AlertMaintenance" Title="Alert Settings" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="Javascript/jquery.min.js"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui.min.js"></script>

    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">

    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=1"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupEmail.js?d=11"></script>

    <script type="text/javascript" language="javascript">

        var g_Email;
        var siteId;

        this.onload = function () {

            g_Email = document.getElementById("<%=hid_useremail.ClientID%>").value;
            LoadSiteList();
        };

        function LoadData() {

            var InfrArrid = [];
            var InfraAlertid;

            $.each($("input[name='chkInfrAlert']:checked"), function () {
                InfrArrid.push($(this).val());
            });

            InfraAlertid = InfrArrid.join(",");

            var InfrSiteArrid = [];
            var InfrSiteAlert;

            $.each($("input[name='chkAlertSiteList']:checked"), function () {
                InfrSiteArrid.push($(this).val());
            });

            InfrSiteAlert = InfrSiteArrid.join(",");

            var LowBatteryArrid = [];
            var LowBatteryAlert;

            $.each($("input[name='chkLowBatteryAlert']:checked"), function () {
                LowBatteryArrid.push($(this).val());
            });

            LowBatteryAlert = LowBatteryArrid.join(",");

            var ReportsiteArrid = [];
            var LowBatteryReportid;

            $.each($("input[name='chkReportSiteList']:checked"), function () {
                ReportsiteArrid.push($(this).val());
            });

            LowBatteryReportid = ReportsiteArrid.join(","); ;

            SetupAlertSiteList(InfraAlertid, InfrSiteAlert, LowBatteryAlert, LowBatteryReportid, g_Email)
        }
                    
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <div id="tooltip3" class="tool3" style="display: none;">
        </div>
        <tr style="height: 10px;">
            <td>
                <input type="hidden" id="hid_userrole" runat="server" />
                <input type="hidden" id="hid_useremail" runat="server" />
                <input type="hidden" id="hid_siteid" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="padding-left: 20px; padding-right: 20px;" align="center">
                <div id="divAlertMaintenance" runat="server" style="height: 850px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="height: 20px;">
                            <td>
                                <input type="hidden" id="hidUserRole" runat="server" />
                                <div style="position: fixed; top: 400px; left: 800px; display: none;" id="divLoading_AlertSettings">
                                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class='SHeader1' align="left">
                                                        Scheduled Alert Reports
                                                    </td>
                                                    <td align="right">
                                                        <div style="position: relative; display: none;" id="divLoading_UserReport">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='SHeader2'>
                                            Infrastructure Offline Alert Report (Daily)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="padding-left: 55px">
                                            <table border="0" cellpadding="2" cellspacing="2" width="100%" class="Tblborder">
                                                <tr>
                                                    <td>
                                                        <table id="tblAlerts" border="0" cellpadding="0" cellspacing="0">
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 2px;">
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr id="trchksitelist1" valign="top">
                                                    <td style="text-align: left; color: #282828;">
                                                        <label for="lblLFExciter" class="lblText">
                                                            Select Site(s) : &nbsp;
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 75px">
                                                        <table id="tblAlertSiteList" border="0" cellpadding="0" cellspacing="0">
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='SHeader2'>
                                            Low Battery 30-Day Alert Report (Weekly)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="padding-left: 55px">
                                            <table border="0" cellpadding="2" cellspacing="2" width="100%" class="Tblborder">
                                                <tr>
                                                    <td>
                                                        <table id="tblReports" border="0" cellpadding="0" cellspacing="0">
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; color: #282828;" valign="top">
                                                        <label for="lblLFExciter" class="lblText">
                                                            Select Site(s) : &nbsp;
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 75px">
                                                        <table id="tblReportSiteList" border="0" cellpadding="0" cellspacing="0" style="display: inline-block;
                                                            float: left">
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td align="center">
                                <input id="btnSave" type="button" value="Save" class="clsbtn" onclick="LoadData();" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding-top: 30px;">
                                <label id="lblMessage" class="clsMapSuccessTxt">
                                </label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        LoadGlossaryInfo("AlertSetting", document.getElementById("<%=hid_userrole.ClientID%>").value);  
    </script>
</asp:content>
