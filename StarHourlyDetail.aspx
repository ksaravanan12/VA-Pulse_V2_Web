<%@ Page Title="GMS Report Details" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="StarHourlyDetail.aspx.vb" Inherits="GMSUI.StarHourlyDetail" %>

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
    <script src="Javascript/js_gmsReports.js?d=78" type="text/javascript"></script>
    <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>
    <script language="javascript" type="text/javascript">

        var g_SiteName;
        var g_IsPaging = "";
        var g_MacId = "";
        var g_FromDate = "";
        var g_StarId = "";
        var g_BeaconSlot = "";
        var g_StarType = "";

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";
            g_MacId = document.getElementById("ctl00_ContentPlaceHolder1_hdnMacId").value;
            g_SiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteId").value;
            g_SiteName = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteName").value;
            g_FromDate = document.getElementById("ctl00_ContentPlaceHolder1_hdnFromDate").value;
            g_StarId = document.getElementById("ctl00_ContentPlaceHolder1_hdnStarId").value;
            g_BeaconSlot = document.getElementById("ctl00_ContentPlaceHolder1_hdnBeaconSlot").value;
            g_StarType = document.getElementById("ctl00_ContentPlaceHolder1_hdnStarType").value;



            if (g_MacId != "") {                                
                document.getElementById('spnReportType').innerHTML = "Star Hourly Information";
                document.getElementById('spnSiteName').innerHTML = g_SiteName;
                document.getElementById('spnStarId').innerHTML = g_StarId;
                document.getElementById('spnBeaconSlot').innerHTML = g_BeaconSlot;
                document.getElementById('spnStarType').innerHTML = g_StarType;

                StarOnehrdata(g_SiteId, 3, g_MacId, g_FromDate, 0, g_SiteName);
                
            }
            else {
                StarDailydata(g_SiteId, g_FromDate);
                document.getElementById('spnReportType').innerHTML = "Star Daily Summary";
                $("#trDeviceHeader").hide();
                $("#trStarHourlyInfo").hide();
                $("#trStarHourlyInfoExport").hide();
                $("#trStarDailyInfo").show();                
            }
        }

        function GetStarOneHrcsv() {
            StarOnehrdata(g_SiteId, 3, g_MacId, g_FromDate, 1, g_SiteName);
        }

    </script>
    <div id="divReportDetails" style="top: auto; left: auto; height: 850px;">
        <input id="hdnSiteId" type="hidden" runat="server" />
        <input id="hdnSiteName" type="hidden" runat="server" />
        <input id="hdnMacId" type="hidden" runat="server" />
        <input id="hdnStarId" type="hidden" runat="server" />
        <input id="hdnBeaconSlot" type="hidden" runat="server" />
        <input id="hdnFromDate" type="hidden" runat="server" />
        <input id="hdnStarType" type="hidden" runat="server" />
        <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
            <tr>
                <td valign="top" colspan='2'>
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
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trDeviceHeader" style="display: none;">
                <td valign="top" style="width: 500px;">                
                    <table cellspacing="0" cellpadding="6" class="display" style="width: 450px;" border="0">                                                
                        <tr style="height:20px;">
                            <td class="Summary_header_cell">
                            </td>
                            <td align="right" class="Summary_header_cell">
                            </td>                            
                        </tr>
                        <tr>
                            <td class="Summary_cell">
                                Site Name
                            </td>
                            <td class="Summary_cell">
                                <span id="spnSiteName" style="color: Green; font-weight: normal;"></span>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="Summary_cell">
                                Star Id
                            </td>
                            <td class="Summary_cell">
                                <span id="spnStarId" style="color: Green; font-weight: normal;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="Summary_cell">
                                Beacon Slot
                            </td>
                            <td class="Summary_cell">
                                <span id="spnBeaconSlot" style="color: Green; font-weight: normal;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="Summary_cell">
                                Star Type
                            </td>
                            <td class="Summary_cell">
                                <span id="spnStarType" style="color: Green; font-weight: normal;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="Summary_cell">
                                Reporting Hrs
                            </td>
                            <td class="Summary_cell">
                                <span id="spnReportingHrs" style="color: Green; font-weight: normal;"></span>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <div id="Div_PageLocationCount">
                    </div>
                </t d>
            </tr>
            <tr id="trStarHourlyInfoExport" style="display: none;">
                <td colspan='2' align="right">
                    <input type="button" value="Export" style="width: 80px; height: 30px;" class="clsExportExcel"
                        name="btnExportReport" id="btnExportReport" onclick="GetStarOneHrcsv();" />
                </td>
            </tr>
            <tr style="height:10px;"></tr>
            <tr id="trStarHourlyInfo" style="display: none;">
                <td valign="top" colspan='2'>
                    <table id="tblStarOneHourData" cellspacing="1" cellpadding="3" style="width: 1700px;
                        padding-top: 10px;" class="display">
                        <thead>
                        <tr>
                        <th class='siteOverview_TopLeft_Box' height='30px'>#</th><th class='siteOverview_Box' height='30px'>Version</th><th class='siteOverview_Box' height='30px'>Mac Id</th>
                        <th class='siteOverview_Box' height='30px'>IP Address</th><th class='siteOverview_Box' height='30px'>Updated On</th><th class='siteOverview_Box' height='30px'>Response Cnt</th>
                        <th class='siteOverview_Box' height='30px'>Paging Cnt</th><th class='siteOverview_Box' height='30px'>Paging Data Cnt</th><th class='siteOverview_Box' height='30px'>Location Cnt</th>
                        <th class='siteOverview_Box' height='30px'>Location Data Cnt</th><th class='siteOverview_Box' height='30px'>TT Sync Error</th><th class='siteOverview_Box' height='30px'>Error Cnt</th>
                        </thead>
                        <tbody id="tblStarOneHourDatabody"></tbody>
                    </table>
                </td>
            </tr>
            <tr id="trStarDailyInfo" style="display: none;">
                <td valign="top" colspan='2'>
                    <input type="button" id="btnPrevGraph" value=" << " onclick="StarPLData();" style="display:none;" />
                    <input type="button" id="btnNextGraph" value=" >> " onclick="StarPLData();" style="display:none;" />
                    <div id="Div_StarPageLocationCount">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:Content>
