<%@ Page Title="Tag and Infrastructure Health Report" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="HealthReport.aspx.vb" Inherits="GMSUI.HealthReport" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">

        this.onload = function () {

            var g_ReportType = document.getElementById("<%=htnReportType.ClientID%>").value;

            if (g_ReportType == 1) {
                $("#trHealthOverviewReport").show();
                $("#trHealthDetailReport").hide();
                document.getElementById("spnReportType").innerHTML = "Tag and Infrastructure Health Overview Report";
            }
            else {
                $("#trHealthOverviewReport").hide();
                $("#trHealthDetailReport").show();
                document.getElementById("spnReportType").innerHTML = "Tag and Infrastructure Health Detailed Site Report";
            }
        }

        function ValidateFile() {

            if ($("#ctl00_ContentPlaceHolder1_ddlDetailSites").val() == "0" || $("#ctl00_ContentPlaceHolder1_ddlDetailSites").val() == "-1" || $("#ctl00_ContentPlaceHolder1_ddlDetailSites").val() == "") {
                alert("Please select a site.");
                return false;
            }

            return true;
        }

        function GoBack() {
            window.history.back();
        }
        
    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <input type="hidden" id="htnReportType" runat="server" />
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
                                                                            <span id="spnReportType">Tag and Infrastructure Health Overview Report</span>
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
                    <tr id="trHealthOverviewReport" style="display: none;">
                        <td valign="top">
                            <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                                <tr>
                                    <td class="clsLALabel" align="left" style="width: 70px;">
                                        Select Site:
                                    </td>
                                    <td align="left">
                                        <asp:dropdownlist id="ddlSites" runat="server" style="width: 550px;" cssclass="wrapper-dropdown">
                                        </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>                                 
                                    <td align="left" style="padding-left: 80px;" colspan="2"> 
                                        <asp:button id="btnGenearateSummary" runat="server" text="Generate" class="clsExportExcel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trHealthDetailReport" style="display: none;">
                        <td valign="top">
                            <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                                <tr>
                                   <td class="clsLALabel" align="left" style="width: 70px;">
                                        Select Site:
                                    </td>
                                    <td align="left">
                                        <asp:dropdownlist id="ddlDetailSites" runat="server" style="width: 550px;" cssclass="wrapper-dropdown">
                                        </asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>                                   
                                    <td align="left" style="padding-left: 80px;" colspan="2"> 
                                        <asp:button id="btnGenerateDetail" runat="server" text="Generate" class="clsExportExcel"
                                            onclientclick="return ValidateFile();" />
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
            </td>
        </tr>
    </table>
</asp:content>
