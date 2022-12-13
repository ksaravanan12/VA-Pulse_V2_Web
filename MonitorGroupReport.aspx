<%@ Page Title="Monitor Group Report" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="MonitorGroupReport.aspx.vb" Inherits="GMSUI.MonitorGroupReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <script type="text/javascript" language="javascript" src="Javascript/js_taginfo.js?d=60"></script>

    <script type="text/javascript">
        function GoBack() {
            window.history.back();
        }

        function MonitorGroupReport() {
            if ($("#ctl00_ContentPlaceHolder1_ddlsite").val() == 0) {
                alert("Please select a Site !!!");
                $("#ctl00_ContentPlaceHolder1_ddlsite").focu();
                return;
            } else {
                DownloadMonitorGroupReport();
            }
        }
         
    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <div>
                    <input type="hidden" runat="server" id="hdnEMReportType" />
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
                                                                            <td align='left' class='subHeader_black' id="tdHeader">
                                                                                Monitor Group Report
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
                                            Select Site:
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlsite" runat="server" CssClass="wrapper-dropdown" Width="500px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <input type="button" id="btnDownloadFile" runat="server" value="Generate" style="width: 110px"
                                                class="clsButton" onclick="return MonitorGroupReport();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding-right: 60px;">
                                <table>
                                    <tr>
                                        <td>
                                            <div style="position: fixed; top: 350px; z-index: 1; display: none;" id="divExcelLoading">
                                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
