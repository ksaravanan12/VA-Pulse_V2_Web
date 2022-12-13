<%@ Page Title="Device Details Report" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="DeviceDetailsReport.aspx.vb" Inherits="GMSUI.DeviceDetailsReport" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />

    <script type="text/javascript" language="javascript" src="Javascript/js_Siteanalysis.js?d=100"></script>
    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>

    <script type="text/javascript">

        function GoBack() {
            window.history.back();
        }

        function DeviceDetailsReport() {

            var siteId = document.getElementById("ctl00_ContentPlaceHolder1_ddlsite").value;
            document.getElementById("txtDeviceIds").value = GetDeviceIdFormat($("#txtDeviceIds").val());

            if ($("#selDeviceType").val() == "0") {
                alert("Select a device type!.");
                $("#selDeviceType").focus();
                return false;
            }
            else if (siteId == "0" && $("#txtDeviceIds").val() == "") {
                alert("Device ID must be entered when searching All Sites");
                $("#txtDeviceIds").focus();
                return false;
            }

            document.getElementById("divLoading").style.display = "";

            GetSiteAnalysisReport(siteId, enumSiteAnalysisReport.DeviceDetails, $("#selDeviceType").val(), $("#txtDeviceIds").val());
        }

        this.onload = function () {
            document.getElementById("divDetailReport").style.display = "";
        }

    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <div style="display: none;" id="divDetailReport">
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
                                                                                Device Details Report
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
                                        <td class="clsLALabel" align="left" style="width: 80px;">
                                            Device Type:
                                        </td>
                                        <td align="left">
                                            <select id="selDeviceType" style="width: 150px;" class="wrapper-dropdown">
                                                <option value="0">Select</option>
                                                <option value="1">Tag</option>
                                                <option value="2">Monitor</option>
                                                <option value="3">Star</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" align="left" style="width: 80px;">
                                            Select Site:
                                        </td>
                                        <td align="left">
                                            <asp:dropdownlist id="ddlsite" runat="server" cssclass="wrapper-dropdown" width="500px">
                                            </asp:dropdownlist>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" align="left" style="width: 80px;" valign="top">
                                            Device Ids:
                                        </td>
                                        <td align="left">
                                            <textarea id="txtDeviceIds" style="width: 493px; height: 74px; margin: 0px;" rows=""
                                                cols=""></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <input type="button" value="Generate" style="width: 100px" class="clsButton" onclick="return DeviceDetailsReport();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position: fixed; top: 400px; z-index: 1; display: none; left: 1000px;"
                    id="divLoading">
                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                </div>
            </td>
        </tr>
    </table>
</asp:content>
