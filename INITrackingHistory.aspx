<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="INITrackingHistory.aspx.vb" Inherits="GMSUI.INITrackingHistory" Title="Connect Pulse - INI Change History" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="jqwidgets/styles/jqx.base.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <script type="text/javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" src="Javascript/js_INI_history.js"></script>
    <script type="text/javascript">

        var siteVal = 0;
        var date;
        var g_UserId;
        var GSiteId = "";

        this.onload = function () {
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            var hdSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;
            document.getElementById("ctl00_headerBanner_drpsitelist").value = hdSiteId;
            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
            var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;

            document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName").innerHTML = siteText;
            GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), "1")
        }

        $(document).ready(function () {
            $("#btnFilter").click(function () {
                var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

                SortColumn = "UpdatedOn";
                SortOrder = "desc";
                SortImg = "<image src='Images/downarrow.png' valign='middle' />";

                GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), currentpage)
            });

            $("#btnClear").click(function () {
                var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

                SortColumn = "UpdatedOn";
                SortOrder = "desc";
                SortImg = "<image src='Images/downarrow.png' valign='middle' />";

                $('#txtDate').val('');
                $('#txtDeviceIds').val('');

                GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), currentpage)
            });

            $(document).on('change', '#selDeviceType', function () {
                var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

                SortColumn = "UpdatedOn";
                SortOrder = "desc";
                SortImg = "<image src='Images/downarrow.png' valign='middle' />";

                $('#txtDeviceIds').val('');
                GetIniHistoryInfo(siteVal, setundefined($('#selDeviceType').val()), setundefined($('#txtDate').val()), setundefined($('#txtDeviceIds').val()), currentpage)
            });
        });

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
                    <tr>
                        <td valign="top">
                            <input type="hidden" id="hdSiteId" runat="server" />
                            <input type="hidden" id="hid_userid" runat="server" />
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
                                                    <asp:Label ID="lblSiteName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                        <tr>
                                                            <td align='left' class='subHeader_black'>
                                                                INI Change History
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
                                <tr style="height: 5px;">
                                    <td class="bordertop" valign="top" colspan="4" style="padding-left: 20px;">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- INI CHANGE HISTORY TABLE-->
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="center" style="padding-left: 30px; padding-right: 22px;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td colspan="6">
                                                    <div style="border: solid 1px #cccccc; height: 90px;">
                                                        <table style="font-size: 13px; font-weight: bold; color: #959595; padding-top: 25px;
                                                            padding-left: 15px; padding-right: 10px; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;"
                                                            cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td class="clsLALabel" style="width: 100px;">
                                                                    Device&nbsp;Type&nbsp;:
                                                                </td>
                                                                <td class="clsLALabel">
                                                                    <select id="selDeviceType" style="width: 144px;" class="dropdownbox">
                                                                        <option value="1" selected="selected">Tag</option>
                                                                        <option value="2">Monitor</option>
                                                                        <option value="3">Star</option>
                                                                    </select>
                                                                </td>
                                                                <td class="clsLALabel" style="width: 100px;">
                                                                    Device&nbsp;Id&nbsp;:
                                                                </td>
                                                                <td class="clsLALabel">
                                                                    <textarea id="txtDeviceIds" class="clsTextAreaBox" cols="" rows=""></textarea>
                                                                </td>
                                                                <td class="clsLALabel" style="width: 50px;">
                                                                    Date&nbsp;:
                                                                </td>
                                                                <td class="clsLALabel">
                                                                    <input id="txtDate" type="text" name="txtDate" class="wrapper-textbox" style="width: 140px;" />
                                                                </td>
                                                                <td style="width: 20px;">
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnFilter" class="clsExportExcel" value="Filter" title="Filter"
                                                                        style="width: 90px;" />
                                                                </td>
                                                                <td style="width: 20px;">
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnClear" class="clsExportExcel" value="Clear" title="Clear Filters"
                                                                        style="width: 90px;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 15px;">
                                </tr>
                                <tr>
                                    <td style="padding-left: 30px; padding-right: 20px;">
                                        <div id="divPaginationRow">
                                            <table cellpadding="0" cellspacing="0" style="width: 100%;" border="0">
                                                <tr class="clsPageNavigationHeader">
                                                    <td class="txttotalpage" style="width: 360px; height: 35px;" valign="middle" id="tdtotalrec">
                                                    </td>
                                                    <td class="txttotalpage" style="float: right;" valign="middle">
                                                        <input type="button" id="btnPrev" class="clsPrev" onclick="doPrev();" title="Previous" />
                                                        <asp:Label ID="lblPage" runat="server" CssClass="clsCntrlTxt"> Page </asp:Label>
                                                        <input id="txtPageNo" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                            type="text" size="1" maxlength="10" runat="server" name="txtPageNo" value="1" />
                                                        <asp:Label ID="lblTotalpage" runat="server" CssClass="clsCntrlTxt">&nbsp;</asp:Label>&nbsp;
                                                        <input type="button" id="btnGo" class="btnGO" value="Go" onclick="gotoPage();" />&nbsp;&nbsp;
                                                        <input type="button" id="btnNext" class="clsNext" onclick="doNext();" title="Next" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="height: 15px;">
                                </tr>
                                <tr>
                                    <td align="center" style="padding-left: 30px; padding-right: 20px;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                            id="tblIniTagHistoryInfo">
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding-left: 30px; padding-right: 20px;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                            id="tblIniMonitorHistoryInfo">
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding-left: 30px; padding-right: 20px;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                            id="tblIniStarHistoryInfo">
                                        </table>
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
                <!-- INI Tracking History  -->
                <div id="Profile_dialog" style="display: none;" title="Profile" align="center">
                    <table cellspacing="1" cellpadding="3" border="0" id="tblProfileInfo" style="width: 100%;">
                    </table>
                    <div style="display: none;" id="divLoadingPop">
                        <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                    </div>
                </div>
            </td>
        </tr>
        <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
        <script type="text/javascript">
            $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });
            $(function () {

                $('#txtDate').datetimepicker({
                    format: 'Y/m/d H:i',
                    step: 10,
                    timepicker: true
                });


            });
        </script>
    </table>
</asp:Content>
