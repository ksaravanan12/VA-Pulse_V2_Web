<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="LocationHistoryreport.aspx.vb" Inherits="GMSUI.LocationHistoryreport"
    Title="Location History Report" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="jqwidgets/styles/jqx.base.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="Javascript/js_General.js"></script>
    <script language="javascript" type="text/javascript" src="Javascript/Reports.js?d=21"></script>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 90%;">
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <input type="hidden" id="hdSiteId" runat="server" />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                        <a href='GMSReports.aspx'>
                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;" /></a>
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
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                        <tr>
                                                            <td align='left' class='subHeader_black'>
                                                                Location History Report
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr style="height: 5px;">
                                    <td class="bordertop" valign="top" colspan="4" style="padding-left: 20px;">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- Location Change Event-->
                    <tr>
                        <td>
                            <table cellspacing="3" cellpadding="3" border="0" class="bordertable" style="border: solid 1px #cccccc;
                                height: 320px;">
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td class="clsLALabel" style="width: 120px">
                                                    Select Site
                                                </td>
                                                <td style="width: 520px;">
                                                    <select id="ddlLocationSites" style="width: 500px;" runat="server" class="wrapper-dropdown">
                                                    </select>
                                                </td>
                                                <td class="clsLALabel" style="width: 100px;">
                                                    From Date
                                                </td>
                                                <td style="width: 120px;">
                                                    <input id="txtLocationFromDate" type="text" class="txtSearchDevice" style="width: 90px;
                                                        height: 23px; padding-left: 5px;">
                                                </td>
                                                <td class="clsLALabel" style="width: 80px;">
                                                    To Date
                                                </td>
                                                <td style="width: 150px;">
                                                    <input id="txtLocationToDate" type="text" class="txtSearchDevice" style="width: 90px;
                                                        height: 23px; padding-left: 5px;">
                                                </td>
                                                <td class="clsLALabel" style="width: 150px; display: none;">
                                                    Tag Type
                                                </td>
                                                <td>
                                                    <select id="ddlLocationType" style="width: 150px; height: 23px; display: none;">
                                                    </select>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 5px;">
                                </tr>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="0" border="0" width="760px">
                                            <tr>
                                                <td class="clsLALabel" style="width: 100px" valign="top">
                                                    Tags
                                                </td>
                                                <td style="width: 400px;" colspan="3">
                                                    <textarea id="txtLocationId" name="txtLocationId" style="margin: 0px; width: 675px;
                                                        height: 52px;" placeholder="Tags (Multiple values by comma separated)"></textarea>
                                                </td>
                                            </tr>
                                            <tr style="height: 10px;">
                                            </tr>
                                            <tr>
                                                <td class="clsLALabel" style="width: 100px" valign="top">
                                                    Monitors
                                                </td>
                                                <td style="width: 400px;" colspan="3">
                                                    <textarea id="txtinMonitorIds" name="txtinMonitorIds" style="margin: 0px; width: 675px;
                                                        height: 52px;" placeholder="Monitors to be included (Multiple values by comma separated)"></textarea>
                                                </td>
                                            </tr>
                                            <tr style="height: 10px;">
                                            </tr>
                                            <tr>
                                                <td class="clsLALabel" style="width: 100px" valign="top">
                                                    Monitors
                                                </td>
                                                <td style="width: 400px;" colspan="3">
                                                    <textarea id="txtexMonitorIds" name="txtexMonitorIds" style="margin: 0px; width: 675px;
                                                        height: 52px;" placeholder="Monitors to be excluded (Multiple values by comma separated)"></textarea>
                                                </td>
                                            </tr>
                                            <tr style="height: 10px;">
                                            </tr>
                                            <tr>
                                                <td class="clsLALabel">
                                                    Time Spend
                                                </td>
                                                <td style="width: 70px;">
                                                    <input id="txtTimeSpend" type="text" class="txtSearchDevice" style="width: 50px;
                                                        height: 23px; padding-left: 5px;" onkeypress="return onlyNumeric(event,this);"
                                                        maxlength="3" />
                                                </td>
                                                <td style="width: 140px;">
                                                    <select id="selSpendType" style="height: 25px; width: 50px;">
                                                        <option value="1">=</option>
                                                        <option value="2">>=</option>
                                                        <option value="3"><=</option>
                                                    </select>&nbsp;<label class="clsLALabel">(in seconds)</label>
                                                </td>
                                                <td>
                                                    <input type="button" id="btnLocationChangeEvent" class="clsExportExcel" title="Generate Report"
                                                        value="Generate" onclick="GenerateLocationChangeReport();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div style="position: fixed; top: 550px; left: 850px; display: none;" id="divLoading">
                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
        <script type="text/javascript">
            $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });
            $(function () {

                $('#txtLocationFromDate').datetimepicker({
                    format: 'Y/m/d',
                    step: 60,
                    timepicker: false
                });

                $('#txtLocationToDate').datetimepicker({
                    format: 'Y/m/d',
                    step: 60,
                    timepicker: false
                });

            });
        </script>
        <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script>
        <script type="text/javascript">
            $('#ddlSites').multipleSelect({
                styler: function (value) {
                    return 'height:20px; margin: 0 auto;  color: #000; outline: none; cursor: pointer; font-weight: bold; font-size: 12px; width:500px;';
                },
                onClick: function (view) {
                    LoadTagTypes(g_3xsite);
                }
            });

            $('.ms-parent ul li:first-child').remove();

            $('#ddlTagType').multipleSelect({
                styler: function (value) {
                    return 'height:20px; margin: 0 auto;  color: #000; outline: none; cursor: pointer; font-weight: bold; font-size: 12px;';
                }
            });

        </script>
    </table>
</asp:content>
