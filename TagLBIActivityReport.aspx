<%@ Page Title="Tag LBI Activity Report" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="TagLBIActivityReport.aspx.vb" Inherits="GMSUI.TagLBIActivityReport" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
	
    <script src="Javascript/jquery.min.js?d=10" language="javascript" type="text/javascript"></script>
    <script src="highchart/highcharts.js?d=10" language="javascript" type="text/javascript"></script>
	
    <script type="text/javascript" language="javascript">

        function GoBack() {
            window.history.back();
        }

        //Flash Player Enabled or Not
        function IsChkFlashPlayerAlreadyInstalled() {
            var flashEnabled = !!(navigator.mimeTypes["application/x-shockwave-flash"] || window.ActiveXObject && new ActiveXObject('ShockwaveFlash.ShockwaveFlash'));
            var txtHideCtrl = document.getElementById("<%=FlasEnabled.ClientID%>");
            txtHideCtrl.value = "No";
            if (flashEnabled) {
                txtHideCtrl.value = "Yes";
            }
        }

        window.onload = IsChkFlashPlayerAlreadyInstalled;

    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="85%">
        <tr style="height: 20px;">
        </tr>
        <tr>
            <td valign="top">
                <input type="hidden" runat="server" id="FlasEnabled" value="No" />
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style='width: 15px;' valign="top">
                        </td>
                        <td align='left' valign="top" style="width: 581px;">
                            <table border='0' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td align='left' class='SHeader1' id="tdSiteName" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                            <tr>
                                                <td align='left' class='subHeader_black'>
                                                    <span id="spnTagId" runat="server"></span>
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
        <tr>
            <td>
                <table style="padding-left: 10px; padding-bottom: 10px; width: 1200px">
                    <tr>
                        <td>
                            <label class="clsLALabel">
                                <b>LBI Activity Chart:</b><asp:checkbox id="ChkShowFilter" runat="server" autopostback="true"
                                    text="Remove Zero Filter" class="clsCheckBox" />
                            </label>
                        </td>
                        <td style="text-align: right">
                            <asp:button id="btnExport" runat="server" text="CSV" class="clsExportExcel" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 10px; padding-right: 10px;">
                <table cellpadding="5" cellspacing="0" style="width: 70%;" border="0">
                    <tr id="trFusionChart" runat="server" visible="false">
                        <td>
                            <div id="BarGraph2" style="width: 100%; height: 400px;">
                            </div>
                            <script type="text/javascript">
                                var bchart = new FusionCharts("Swf/ZoomLine.swf", "ChartId1", "1200", "400", "0", "0");
                                bchart.setDataXML("<%=sXMLLBIDiff%>");
                                bchart.render("BarGraph2");
                            </script>
                        </td>
                    </tr>
                    <tr id="TRHighchart" runat="server" visible="false">
                        <td align="left" id="ZoomType" runat="server">
                            <table id="ZoomTypeGraph" runat="server" cellpadding="0" cellspacing="0" border="0"
                                width="100%">
                                <tr>
                                    <td id="TdZoomedGraphData" runat="server">
                                        <div id="LBIValueZoom" style='width: 1200px; height: 450px; vertical-align: top;'
                                            runat="server">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="clsLALabel">
                                <b>Battery Capacity Chart:</b>
                            </label>
                        </td>
                    </tr>
                    <tr id="trBatteryFusionChart" runat="server" visible="false">
                        <td>
                            <div id="BarGraph3" style="width: 100%; height: 400px;">
                            </div>
                            <script type="text/javascript">
                                var bchart = new FusionCharts("Swf/ZoomLine.swf", "ChartId2", "1200", "400", "0", "0");
                                bchart.setDataXML("<%=sXMLBatteryDiff%>");
                                bchart.render("BarGraph3");
                            </script>
                        </td>
                    </tr>
                    <tr id="TRBatteryHighchart" runat="server" visible="false">
                        <td align="left" id="BatteryZoomType" runat="server">
                            <table id="BatteryZoomTypeGraph" runat="server" cellpadding="0" cellspacing="0" border="0"
                                width="100%">
                                <tr>
                                    <td id="TdZoomedBatteryGraphData" runat="server">
                                        <div id="BatteryValueZoom" style='width: 1200px; height: 450px; vertical-align: top;'
                                            runat="server">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="clsLALabel">
                                <b>Paging/Location Chart:</b>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td runat="server">
                                        <div id="divPagingLocationChart" style='width: 1200px; height: 450px; vertical-align: top;'
                                            runat="server">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height: 10px;">
        </tr>
        <tr>
            <td style="padding-left: 10px;">
                <table id="tblLogViewerInfo" runat="server" border="0" cellpadding="5" cellspacing="0"
                    width="100%">
                </table>
            </td>
        </tr>
    </table>
</asp:content>
