<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Reports.aspx.vb" Inherits="GMSUI.Reports" Title="Connect Pulse - Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="jqwidgets/styles/jqx.base.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <link rel="stylesheet" href="Styles/multiple-select.css" />

    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>

    <script language="javascript" type="text/javascript" src="Javascript/Reports.js"></script>

    <script type="text/javascript">

        var siteVal = 0;  
        var date;
        var curReportType = 0;
        var ReportName = '';
        var GSiteId = "";
        window.onhashchange = function () {
            // Load Glossary
            if (location.hash == "") {
                showReportSummary();
            }
        }

        this.onload = function () {
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            var hdSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;
            document.getElementById("ctl00_headerBanner_drpsitelist").value = hdSiteId;
            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
            var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName_Map").innerHTML = siteText;

            document.getElementById("trOnDemandReports").style.display = "none";

            showReport(9);
            GetSiteList();
            LoadTagTypes(1);
        }

        function showReport(reportType) {

            location.hash = "#reports";
            curReportType = reportType;
            
            $('#tblReportSummary').hide('slide', { direction: 'left' }, 100);

            if (reportType == 9) //Tags Not Seen Recently
            {
                ReportName = 'Tags Not Seen Recently';               
                document.getElementById("tdReportTitle").innerHTML = "Tags Not Seen Recently";
                $('#trOnDemandReports').show();
                $('#trOnDemandReports').show('slide', { direction: 'right' }, 600);               
            }
        }

        function showReportSummary() {          
            $('#tblReportSummary').show('slide', { direction: 'left' }, 900);           
            document.getElementById("trOnDemandReports").style.display = "none";          
        }
        
        function DownloaOnDemandPDF() {

            if (setundefined($('#ddlSites').val()) == '' || setundefined($('#ddlSites').val()) == null || setundefined($('#ddlSites').val()) == 0) {
                alert("Please select a site.");
                $('#ddlSites').focus();
                return;
            }
            else if (setundefined($('#txtLastSeen').val() == '')) {
                alert("Please enter a date");
                $('#txtLastSeen').focus();
                return;
            }

            window.open("ReportPdf.aspx?SiteId=" + $('#ddlSites').val() + "&TypeIds=" + $('#ddlTagType').val() + "&IsOnDemand=1&Date=" + setundefined($('#txtLastSeen').val()));
            return false;
        }

        function redirectToHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
            location.href = "Home.aspx?sid=" + GSiteId;
        }

    </script>

    <table cellpadding="0" cellspacing="0" border="0" style="width: 90%;">
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr style="height: 10px;">
                    </tr>
                    <!-- REPORT HEADER PAGE-->
                    <tr>
                        <td valign="top">
                            <input type="hidden" id="hdSiteId" runat="server" />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tdHeader">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                        <a onclick="redirectToHome();">
                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;" /></a>
                                    </td>
                                    <td style='width: 15px;' valign="top">
                                    </td>
                                    <td align='left' valign="top" style="width: 581px;">
                                        <table border='0' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td align='left' class='SHeader1'>
                                                    <asp:Label ID="lblSiteName_Map" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                        <tr>
                                                            <td align='left' class='subHeader_black' id="tdReportTitle">
                                                                Reports
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 75px; visibility: hidden;" align="center" id="tdReportSwitch">
                                        <img src="Images/configure.png" width="24px" height="24px" onclick="showReportSummary();"
                                            style="visibility: hidden;" />
                                        <br />
                                        <label class="clsLALabel" onclick="showReportSummary();" style="cursor: pointer;
                                            visibility: hidden;">
                                            All Reports</label>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr style="height: 5px;">
                                    <td class="bordertop" valign="top" colspan="4" style="padding-left: 20px;">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- on-demand REPORT -->
                    <tr id="trOnDemandReports" style="display: none;">
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="padding-left: 30px; padding-right: 20px;">
                                        <div style="border: solid 1px #cccccc; height: 85px;">
                                            <table cellspacing="0" cellpadding="0" border="0" class="bordertable">
                                                <tr>
                                                    <td style="text-align: right; width: 100px" class="clsLALabel">
                                                        Site(s) :
                                                    </td>
                                                    <td style="padding-left: 10px; width: 100px">
                                                        <select id="ddlSites" multiple="multiple" style="width: 350px;">
                                                        </select>
                                                    </td>
                                                    <td style="text-align: right; width: 250px" class="clsLALabel">
                                                        Tag Type(s) :
                                                    </td>
                                                    <td style="padding-left: 10px; width: 100px">
                                                        <select id="ddlTagType" multiple="multiple" style="width: 200px;">
                                                        </select>
                                                    </td>
                                                    <td style="width: 300px" align="center">
                                                        <input type="button" id="btnCollision" class="clsExportExcel" title="Generate Report"
                                                            value="Generate" onclick="DownloaOnDemandPDF();" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; width: 100px; white-space: nowrap;" class="clsLALabel">
                                                        Last Seen Before :
                                                    </td>
                                                    <td style="padding-left: 10px; width: 100px">
                                                        <input id="txtLastSeen" type="text" name="txtLastSeen" class="txtSearchDevice" style="width: 150px;
                                                            height: 25px; padding-left: 5px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
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
                                        <div style="position: fixed; top: 350px; z-index: 1; display: none;" id="divLoading">
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
        <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
        <script type="text/javascript">
            $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });
            $(function () {

                $('#txtLastSeen').datetimepicker({
                    format: 'Y/m/d H:i',
                    step: 10,
                    timepicker: true
                });
            });
        </script>
        <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script>
        <script type="text/javascript">
            $('#ddlSites').multipleSelect({
                styler: function (value) {
                    return ' height:20px; margin: 0 auto;  color: #000; outline: none; cursor: pointer; font-weight: bold; font-size: 12px; width:600px;';
                }
            });
            $('.ms-parent ul li:first-child').remove();
            $('#ddlTagType').multipleSelect({
                styler: function (value) {
                    return ' height:20px; margin: 0 auto;  color: #000; outline: none; cursor: pointer; font-weight: bold; font-size: 12px;';
                }
            });
        </script>
    </table>
</asp:Content>
