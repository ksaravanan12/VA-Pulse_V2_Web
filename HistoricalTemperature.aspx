<%@ Page Title="Historical Temperature Report" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="HistoricalTemperature.aspx.vb" Inherits="GMSUI.HistoricalTemperature" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css?10" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />

    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
    <script type="text/javascript" src="Javascript/js_downloadcsvreport.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js"></script>

    <script type="text/javascript">

        $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });

        $(function () {

            $('#ctl00_ContentPlaceHolder1_txtFromDate').datetimepicker({
                format: 'Y/m/d H:i',
                step: 60,
                timepicker: true
            });

            $('#ctl00_ContentPlaceHolder1_txtToDate').datetimepicker({
                format: 'Y/m/d H:i',
                step: 60,
                timepicker: true
            });

        });

        function Validate() {

            var siteid = document.getElementById("ctl00_ContentPlaceHolder1_ddlsitelist").value;
            var sitename = $("#ctl00_ContentPlaceHolder1_ddlsitelist option:selected").text();
            var fromdate = document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate").value;
            var todate = document.getElementById("ctl00_ContentPlaceHolder1_txtToDate").value;
            var DeviceId = document.getElementById("txtDeviceIds").value;

            if (siteid == "0") {
                alert("Please select site");
                document.getElementById("ctl00_ContentPlaceHolder1_ddlsitelist").focus();
                return false;
            }
            if (fromdate == "") {
                alert("From date should not be empty");
                document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate").focus();
                return false;
            }

            if (todate == "") {
                alert("To date should not be empty");
                document.getElementById("ctl00_ContentPlaceHolder1_txtToDate").focus();
                return false;
            }

            if (todate != "") {
                var from = Date.parse(fromdate);
                var to = Date.parse(todate);
                if (from > to) {
                    alert("To date must be greater than From date");
                    return false;
                }
            }

            if (DeviceId == "") {
                alert("Please enter the device id");
                document.getElementById("txtDeviceIds").focus();
                return false;
            }

            DeviceLen = new Array();
            DeviceLen = DeviceId.split(",");

            if (DeviceLen.length > 1) {
                alert("Multiple Device ids should not allowed");
                document.getElementById("txtDeviceIds").value = "";
                document.getElementById("txtDeviceIds").focus();
                return false;
            }

            GetHistoricalTempReport(siteid, sitename, DeviceId, fromdate, todate);
        }

        function GoBack() {
            window.history.back();
        }

    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="85%">
        <tr style="height: 20px;">
        </tr>
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                        <a onclick="GoBack()">
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
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                        <tr>
                                                            <td align='left' class='subHeader_black'>
                                                                Historical Temperature Report
                                                            </td>
                                                        </tr>
                                                    </table>
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
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" style="padding-left: 30px;">
                <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            Site:
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="ddlsitelist" runat="server" cssclass="wrapper-dropdown" width="550px">
                            </asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            From Date:
                        </td>
                        <td align="left">
                            <input id="txtFromDate" type="text" name="txtFromDate" runat="server" class="wrapper-textbox"
                                style="width: 120px;" autocomplete="off" />
                        </td>
                    </tr>
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            To Date:
                        </td>
                        <td align="left">
                            <input id="txtToDate" type="text" name="txtToDate" runat="server" class="wrapper-textbox"
                                style="width: 120px;" autocomplete="off" />
                        </td>
                    </tr>
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            Device Id:
                        </td>
                        <td class="clsLALabel" align="left">
                            <textarea id="txtDeviceIds" style="width: 200px; height: 50px; margin: 0px;" rows=""
                                cols=""></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <input type="button" id="btnExport" runat="server" value="Export" style="width: 85px"
                                class="clsButton" onclick="if(! Validate()) return false;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="position: fixed; top: 450px; z-index: 1; display: none; left: 700px;"
        id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:content>
