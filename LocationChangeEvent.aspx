<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="LocationChangeEvent.aspx.vb" Inherits="GMSUI.LocationChangeEvent" Title="LocationChangeEvent" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/locationChangeEvent.js"></script>
    <script type="text/javascript">

        $('#datetimepicker').datetimepicker()
            .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });

        $(function () {
            $('#txtEnterdOnDateFrm').datetimepicker({
                format: 'Y/m/d H:i',
                step: 10,
                timepicker: true
            });

            $('#txtEnterdOnDateTo').datetimepicker({
                format: 'Y/m/d H:i',
                step: 10,
                timepicker: true
            });
            $('#LeftOnOnDateFrm').datetimepicker({
                format: 'Y/m/d H:i',
                step: 10,
                timepicker: true
            });

            $('#LeftOnOnDateTo').datetimepicker({
                format: 'Y/m/d H:i',
                step: 10,
                timepicker: true
            });
        });
	
    </script>
    <script language="javascript" type="text/javascript">
        var g_UserRole = "";
        var SiteId = "";
        var curEmailId = "";
        var isButtonClicked = 0;
        var isalertsChanged = 0;
        var GSiteId = "";
        this.onload = function () {
            SiteId = document.getElementById('ctl00_ContentPlaceHolder1_h_SiteId').value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            LoadLocationChangeEvent(SiteId);
        }

        $(document).on('change', "#ctl00_headerBanner_drpsitelist", function () {
            LastFetchedDateTime = "";
            SiteId = $("#ctl00_headerBanner_drpsitelist").val();

            if (isSearch == true) {
                document.getElementById('txtTagId').value = "";
                document.getElementById('txtCurrentRoom').value = "";
                document.getElementById('txtLastRoom').value = "";
                isSearch = false;
                isSearchNum = "0";
            }

            LoadLocationChangeEvent(SiteId);
            $("#divLocationHistory").hide();
            $("#divLocationLive").show();

        });

        //Set Interval for Get Location Change page Live data
        setInterval(function () {
            LocationChageEventLive(SiteId, "", "", "", "25", "1", 1, LastFetchedDateTime, g_LiveSort);
            $("#divLoading").hide();
        }, 10000);

        function LoadLocationChangeEvent(SiteId) {
            $("#divLocationLive").show();

            $('#tdLive25').removeClass("clsPageSize").addClass("clsPageSizeCurrent");
            $('#tdLive50').removeClass("clsPageSizeCurrent").addClass("clsPageSize");
            $('#tdLive100').removeClass("clsPageSizeCurrent").addClass("clsPageSize");

            $('#tdHistory25').removeClass("clsPageSize").addClass("clsPageSizeCurrent");
            $('#tdHistory50').removeClass("clsPageSizeCurrent").addClass("clsPageSize");
            $('#tdHistory100').removeClass("clsPageSizeCurrent").addClass("clsPageSize");


            LocationChageEventLive(SiteId, "", "", "", "25", "1", 1, "", "TagId desc");
            LocationChageEventHistory(SiteId, "", "", "", "25", "1", 0, "", "", "", "", "", "");

            document.getElementById("ctl00_headerBanner_drpsitelist").value = SiteId;
            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("LocationLiveHeader").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
            document.getElementById("LocationHistoryHeader").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
            document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value = "1";
            document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value = "1";
        }

        function showTip3(text, lf, tp) {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip3").css({ left: tempX, top: tempY })
        }
        function hideTip3() {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.display = 'none';
        }
        function CancelSearchInfo() {
            if (isSearch == true) {
                document.getElementById('txtTagId').value = "";
                document.getElementById('txtCurrentRoom').value = "";
                document.getElementById('txtLastRoom').value = "";
                isSearch = false;
                isSearchNum = "0";
                LocationChageEventLive(SiteId, "", "", "", "25", "1", 1, "", "TagId desc");
            }
        }
        function Validate() {
            var Tagid = document.getElementById('txtTagId').value;
            var CurrentRoom = document.getElementById('txtCurrentRoom').value;
            var LastRoom = document.getElementById('txtLastRoom').value;
            if (Tagid == "" && CurrentRoom == "" && LastRoom == "") {
                document.getElementById('txtTagId').focus();
                alert("Must Entered One Field");
            }
            else {
                LocationChangeLivePgView('show');
            }
        }
        function ReturntoHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
            location.href = "Home.aspx?sid=" + GSiteId;
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 10px;">
        </tr>
        <tr>
            <td>
                <div id="tooltip3" class="tool3" style="display: none;">
                </div>
                <!-- LocationHistory -->
                <div id="divLocationHistory" style="display: none; top: auto; left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="tblHeader"
                                    runat="server">
                                    <tr style="height: 10px;">
                                        <td>
                                            <input type="hidden" id="hid_userrole" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                                    <a onclick="ReturntoHome();">
                                                                        <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                                                </td>
                                                                <td style='width: 15px;' valign="top">
                                                                </td>
                                                                <td align='left' valign="top" style="width: 581px;">
                                                                    <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                                        <tr>
                                                                            <td id="LocationHistoryHeader" align='left' class='SHeader1'>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Location Change Event
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 75px;" align="center">
                                                                    <img src="Images/Live.png" width="32px" height="32px" />
                                                                    <br />
                                                                    <label class="clsLALabel" onclick="DisplayLocationChange(2);" style="cursor: pointer;">
                                                                        Live</label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="2" cellspacing="2" width="650px" class="clsFilterTable">
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Tag&nbsp;id&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <input type="text" id="txtTagHistoryId" style="width: 480px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Current&nbsp;Room&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <input type="text" id="txtCurrentRoomHistoryId" style="width: 480px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Last&nbsp;Room&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <input type="text" id="txtLastRoomHistoryId" style="width: 480px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>EnterdOn&nbsp;:&nbsp;</b>
                                        </td>
                                        <td class="clsLALabel" align="left">
                                            <table border="0">
                                                <tr>
                                                    <td>
                                                        <b>From&nbsp;:&nbsp;</b><input type="text" id="txtEnterdOnDateFrm" style="width: 120px;" />
                                                    </td>
                                                    <td>
                                                        <b>&nbsp;&nbsp;To&nbsp;:&nbsp;</b><input type="text" id="txtEnterdOnDateTo" style="width: 120px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>LeftOn&nbsp;:&nbsp;</b>
                                        </td>
                                        <td class="clsLALabel" align="left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <b>From&nbsp;:&nbsp;</b><input type="text" id="LeftOnOnDateFrm" style="width: 120px;" />
                                                    </td>
                                                    <td>
                                                        <b>&nbsp;&nbsp;To&nbsp;:&nbsp;</b>
                                                        <input type="text" id="LeftOnOnDateTo" style="width: 120px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td colspan="2" align="right">
                                            <input type="button" id="Button1" class="clsExportExcel" value=" Show " onclick="LocationChangeHistoryPgView('show')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 20px;">
                            </td>
                        </tr>
                        <tr id="trLocationChangeHistoryView">
                            <td>
                                <!-- PREVIOUS/NEXT -->
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 650px;">
                                    <tr>
                                        <td align="right">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr style="height: 40px;">
                                                    <td class="txttotalpage" style="width: 275px;" valign="middle">
                                                        <asp:label id="lblCount_LocationChangeHistory" runat="server"></asp:label>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <div style="display: none;" id="divLoadingHistory">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td id="tdHistory25" class="clsPageSize" onclick="getPageSizeHistoryData(this,'25');">
                                                                    25
                                                                </td>
                                                                <td id="tdHistory50" class="clsPageSize" onclick="getPageSizeHistoryData(this,'50');">
                                                                    50
                                                                </td>
                                                                <td id="tdHistory100" class="clsPageSize" onclick="getPageSizeHistoryData(this,'100');">
                                                                    100
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="clsTableTitleText" align="right">
                                                        <input type="button" id="btnPrev_LocationChangeHistoryView" class="clsPrev" title="Previous"
                                                            onclick="LocationChangeHistoryPgView(3);" />
                                                        <asp:label id="Label2" runat="server" cssclass="clsCntrlTxt"> Page </asp:label>
                                                        <input id="txtPageNo_LocationChangeHistory" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                            type="text" size="1" maxlength="10" runat="server" name="txtPageNo" value="1" />
                                                        <asp:label id="lblPgCnt_LocationChangeHistoryView" runat="server" cssclass="clsCntrlTxt">&nbsp;</asp:label>&nbsp;
                                                        <input type="button" id="btnGo_LocationChangeHistoryView" class="btnGO" value="Go"
                                                            onclick="LocationChangeHistoryPgView(1);" />&nbsp;&nbsp;
                                                        <input type="button" id="btnNext_LocationChangeHistoryView" class="clsNext" title="Next"
                                                            onclick="LocationChangeHistoryPgView(2);" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellspacing="0" cellpadding="5" id="tblLocationChangeHistory" style="width: 650px;
                                    border: solid 0px #BAB9B9;">
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                    </table>
                </div>
                <!-- LocationLive -->
                <div id="divLocationLive" style="top: auto; display: none; left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="Table1"
                                    runat="server">
                                    <tr style="height: 10px;">
                                        <td>
                                            <input type="hidden" id="Hidden1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                                    <a onclick="ReturntoHome();">
                                                                        <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                                                </td>
                                                                <td style='width: 15px;' valign="top">
                                                                </td>
                                                                <td align='left' valign="top" style="width: 581px;">
                                                                    <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                                        <tr>
                                                                            <td id="LocationLiveHeader" align='left' class='SHeader1'>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Location Change Event
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 75px;" align="center">
                                                                    <img src="Images/hiistoryimg.png" width="30px" height="32px" />
                                                                    <br />
                                                                    <label class="clsLALabel" onclick="DisplayLocationChange(1);" style="cursor: pointer;">
                                                                        History</label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <input type="hidden" id="h_SiteId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="2" cellspacing="2" width="650px" class="clsFilterTable">
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Tag&nbsp;id&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <input type="text" id="txtTagId" style="width: 480px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Current&nbsp;Room&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <input type="text" id="txtCurrentRoom" style="width: 480px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Last&nbsp;Room&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <input type="text" id="txtLastRoom" style="width: 480px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td colspan="2" align="right">
                                            <input type="button" id="Button3" class="clsExportExcel" value=" Show " onclick="Validate();" />
                                            <input type="button" id="Button2" class="clsExportExcel" value=" Cancel " onclick="CancelSearchInfo();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 20px;">
                            </td>
                        </tr>
                        <tr id="trLocationChangeLiveView">
                            <td>
                                <!-- PREVIOUS/NEXT -->
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 650px;">
                                    <tr>
                                        <td align="right">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr style="height: 40px;">
                                                    <td class="txttotalpage" style="width: 275px;" valign="middle">
                                                        <asp:label id="lblCount_LocationChangeLive" runat="server"></asp:label>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <div style="display: none;" id="divLoading">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td id="tdLive25" class="clsPageSize" onclick="getPageSizeLiveData(this,'25');">
                                                                    25
                                                                </td>
                                                                <td id="tdLive50" class="clsPageSize" onclick="getPageSizeLiveData(this,'50');">
                                                                    50
                                                                </td>
                                                                <td id="tdLive100" class="clsPageSize" onclick="getPageSizeLiveData(this,'100');">
                                                                    100
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="clsTableTitleText" align="right">
                                                        <input type="button" id="btnPrev_LocationChangeView" class="clsPrev" title="Previous"
                                                            onclick="LocationChangeLivePgView(3);" />
                                                        <asp:label id="Label4" runat="server" cssclass="clsCntrlTxt"> Page </asp:label>
                                                        <input id="txtPageNo_LocationChange" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                            type="text" size="1" maxlength="10" runat="server" name="txtPageNo" value="1" />
                                                        <asp:label id="lblPgCnt_LocationChangeView" runat="server" cssclass="clsCntrlTxt">&nbsp;</asp:label>&nbsp;
                                                        <input type="button" id="btnGo_LocationChangeView" class="btnGO" value="Go" onclick="LocationChangeLivePgView(1);" />&nbsp;&nbsp;
                                                        <input type="button" id="btnNext_LocationChangeView" class="clsNext" title="Next"
                                                            onclick="LocationChangeLivePgView(2);" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellspacing="0" cellpadding="5" id="tblLocationChangeLive" style="width: 650px;
                                    border: solid 0px #BAB9B9;">
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:content>
