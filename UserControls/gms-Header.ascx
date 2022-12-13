<%@ Control Language="VB" AutoEventWireup="false" CodeFile="gms-Header.ascx.vb" Inherits="GMSUI.UserControls_gms_Header" %>
<%--<script type="text/javascript">
    function headerRedirect(arg){
       // alert("Redirect "+arg);
        var url=""
        if(arg=="home"){
            url="home.aspx";
        }else if(arg=="profile"){
            url="Profile.aspx";
        }
        location.href=url;
        
    }
</script>--%>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $(document).mouseup(function (e) {
            var container = $("#ctl00_headerBanner_glsdiv");
            var container2 = $("#ctl00_headerBanner_divGlossary");
            var container3 = $(".expandstory");
            var container4 = $("#glsClose");

            if (container.has(e.target).length === 0 && container2.has(e.target).length === 0 && container3.has(e.target).length === 0 && container4.has(e.target).length === 0) {
                if ($("#ctl00_headerBanner_glsdiv").length > 0) {
                    if (document.getElementById("ctl00_headerBanner_glsdiv").style.display != "none") {
                        $("#ctl00_headerBanner_glsdiv").hide(300);
                    }
                }
            }
        });

        //Stop Click
        $('#ctl00_headerBanner_divAlertList').click(function (e) {
            e.stopPropagation();
        });

        $('#ctl00_headerBanner_alertList').click(function (e) {
            e.stopPropagation();
        });

        $('#ctl00_headerBanner_alerttd').click(function (e) {
            e.stopPropagation();
        });

        $('#ctl00_leftmenu_tblcriticalAlert').click(function (e) {
            e.stopPropagation();
        });

        //Show & Hide Alerts
        $(document).click(function () {
            $("#ctl00_headerBanner_divAlertList").hide(300);
        });

        $("#alertList").click(function () {
            showAlerts();
        });

    });

    function onChangeSite() {
        var locArr = window.location.pathname.split("/");

        if (locArr[locArr.length - 1] == "EmailReportSchedule.aspx" || locArr[locArr.length - 1] == "AlertMaintenance.aspx" || locArr[locArr.length - 1] == "Map.aspx" || locArr[locArr.length - 1] == "LocationChangeEvent.aspx" || locArr[locArr.length - 1] == "PurchaseOrder.aspx" || locArr[locArr.length - 1] == "ServerStatus.aspx" || locArr[locArr.length - 1] == "LocalAlerts.aspx" || locArr[locArr.length - 1] == "GeneratePdf.aspx")
            return false;
        else
            return true;
    }

    //Show & Hide Alerts
    function showAlerts() {
        $("#ctl00_headerBanner_divAlertList").css({
            'top': '60px',
            'left': '450px',
            'position': 'fixed',
            'background': '#FFFFFF'
        });

        $("#ctl00_headerBanner_divAlertList").show(300);
    }

    function hideOnClose() {
        $("#ctl00_headerBanner_divAlertList").hide(300);
    }

    //Load Alerts
    function loadAlerts() {
        SetAlertInfo();
        LoadAlertInfo("");
    }

    function loadAlertsBySiteId(SiteId) {
        SetAlertInfo();
        LoadAlertInfo(SiteId);
        showAlerts();
    }

    function SetAlertInfo() {
        document.getElementById("divLoading_DeviceDetail").style.display = "";
        document.getElementById("lblHeaderName").innerHTML = "";

        var sTbl, sTblLen;
        var sTblAlertInfo, sTblAlertInfoLen;

        if (GetBrowserType() == "isIE") {
            sTbl = document.getElementById('siteAlertinfo');
            sTblAlertInfo = document.getElementById("tblAlertInfo");
        }
        else if (GetBrowserType() == "isFF") {
            sTblAlertInfo = document.getElementById("tblAlertInfo");
            sTbl = document.getElementById('siteAlertinfo');
        }
        sTblLen = sTbl.rows.length;
        sTblAlertInfoLen = sTblAlertInfo.rows.length;
        clearTableRows(sTbl, sTblLen);
        clearTableRows(sTblAlertInfo, sTblAlertInfoLen);
    }
</script>
<table cellpadding="0" cellspacing="0" border="0" class="shadow">
    <tr>
        <td valign="middle">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 300px; border: solid 0px red;">
                <tr>
                    <td valign="top" align="center">
                        <img src="Images/Logo.png" style="width: 233px; height: 40px;" alt="Logo" />
                    </td>
                </tr>
            </table>
        </td>
        <td valign="middle">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 800px;">
                <tr>
                    <td align="left">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 725px; padding-left: 45px;">
                            <tr>
                                <td align="right" style="width: 50px;">
                                </td>
                                <td valign="top" style="width: 50px; display: none; cursor: pointer;" align="center"
                                    runat="server" id="alerttd">
                                    <a class="tooltips" id="alertList" onclick="loadAlerts();">
                                        <img src="Images/Alert.png" border="0" id="alerticn" style="width: 38px; height: 38px;" /><span>Alert</span></a>
                                </td>
                                <td valign="top" style="width: 50px; cursor: pointer;" align="center">
                                    <a class="tooltips" href="Home.aspx">
                                        <img src="Images/Home.png" border="0" style="width: 38px; height: 38px;" /><span>Home</span></a>
                                </td>                             
                                <td valign="top" style="width: 50px; cursor: pointer;" align="center">
                                    <a class="tooltips" href="Profile.aspx">
                                        <img src="Images/Person.png" border="0" style="width: 38px; height: 38px;" /><span>Profile</span></a>
                                </td>
                                <td valign="top" style="width: 50px; cursor: pointer;" align="center" id="tdAlertMaintenance"
                                    runat="server">
                                    <a class="tooltips" href="AlertMaintenance.aspx">
                                        <img src="Images/AlertNew.png" border="0" style="width: 38px; height: 38px;" /><span>Alerts</span></a>
                                </td>
                                <td valign="top" style="width: 50px;" align="center">
                                    <a class="tooltips" id="glossary">
                                        <img src="Images/Question.png" border="0" style="width: 38px; height: 38px;" /><span>Glossary</span></a>
                                </td>
                                <td valign="top" style="width: 50px;" align="center">
                                    <a class="tooltips" href="Search.aspx" onclick="">
                                        <img src="Images/Search.png" border="0" style="width: 38px; height: 38px;" /><span>Search</span></a>
                                </td>
                                <td valign="top" style="width: 50px;" align="center" id="tdEmailTab" runat="server">
                                    <%--EmailList.aspx--%>
                                    <a class="tooltips" href="EmailList.aspx" onclick="">
                                        <img src="Images/Email.png" border="0" style="width: 38px; height: 38px;" /><span>Email</span></a>
                                </td>
                                <td valign="top" style="width: 50px;" align="center" id="tdSettings" runat="server">
                                    <a class="tooltips" href="AdminSettings.aspx" onclick="">
                                        <img src="Images/Settings.png" border="0" style="width: 38px; height: 38px;" /><span>Settings</span></a>
                                </td>
                                <td valign="top" style="width: 50px;" align="center" id="tdGMSReport" runat="server">
                                    <a class="tooltips" href="GMSReports.aspx">
                                        <img src="Images/Note.png" border="0" style="width: 38px; height: 38px;" /><span>
                                            Pulse Reports</span></a>
                                </td>
                                <td valign="top" style="width: 50px; cursor: pointer;" align="center">
                                    <a class="tooltips" href="Logout.aspx">
                                        <img src="Images/menulogout.png" border="0" width="38px" height="38px" style="width: 38px;
                                            height: 38px;" /><span>Logout</span></a>
                                </td>
                                <td align="right" style="width: 300px;">
                                    <asp:DropDownList ID="drpsitelist" runat="server" CssClass="wrapper-dropdown" AutoPostBack="true"
                                        onchange="if(!onChangeSite())return false;">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div id="glsdiv" runat="server" style="display: none; z-index: 1;">
    <div style="height: 580px; width: 650px; overflow-y: scroll; overflow-x: hidden;
        border: solid 4px #005695;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td>
                                <img src="images/closepopup.png" alt="close" id="glsClose" style="cursor: pointer;
                                    position: absolute; right: -12px; top: -15px; width: 40px; height: 39px;" onclick="hideOnClose();" />
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                        </tr>
                        <tr>
                            <td class="subHeader_black" style="font-size: 18px;" align="center">
                                Glossary&nbsp;
                                <div style="display: none;" id="divLoading_Glossary">
                                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="3" cellspacing="3" style="width: 100%;">
                        <tr>
                            <td>
                                <div id="divGlossary" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="divAlertList" runat="server" style="display: none; z-index: 5001;">
    <div style="height: 580px; width: 650px; overflow-y: scroll; overflow-x: hidden;
        border: solid 4px #005695;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td>
                                <img src="images/closepopup.png" alt="close" id="glsClose_AlertList" style="cursor: pointer;
                                    position: absolute; right: -12px; top: -15px; width: 40px; height: 39px;" onclick="hideOnClose();" />
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                        </tr>
                        <tr>
                            <td class="subHeader_black" style="font-size: 18px;" align="center">
                                <label id="lblHeaderName">
                                    Critical Alerts</label>
                                <%--<div style='float: right;'>
                                    <img style='cursor: pointer; width: 20px; height: 20px;' id='glsClose_AlertList'
                                        src='images/Close.png' onclick="hideOnClose();" /></div>--%>
                                <div style="display: none;" id="divLoading_DeviceDetail">
                                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <table border="0" cellpadding="3" cellspacing="3" style="width: 100%;" id="siteAlertinfo">
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="3" cellspacing="3" width="100%">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="tblAlertInfo">
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
