<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AllowStarSettings.aspx.vb" Inherits="GMSUI.AllowStarSettings" Title="Allow Star Settings" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupSite.js?d=21"></script>
    <script src="Javascript/jquery.multiple.select.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link href="jqHtmlEditor/app.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" type="text/css" />
    <script type="text/javascript">

        this.onload = function () {
            $('#selSiteList').multipleSelect('refresh');
            GetSiteList(0);
        }

        var g_SObj;
        var g_SRoot;

        function GetSiteList(SiteId) {

            if (g_SObj != null) { g_SObj = null; }

            g_SObj = CreateXMLObj();

            document.getElementById("divLoading").style.display = "";

            if (g_SObj != null) {
                g_SObj.onreadystatechange = ajaxLoadSiteListView;

                DbConnectorPath = "AjaxConnector.aspx?cmd=GetSiteList&Site=" + SiteId;

                if (GetBrowserType() == "isIE") {
                    g_SObj.open("GET", DbConnectorPath, true);
                }
                else if (GetBrowserType() == "isFF") {
                    g_SObj.open("GET", DbConnectorPath, true);
                }
                g_SObj.send(null);
            }
            return false;
        }

        //Ajax Request for Site View
        function ajaxLoadSiteListView() {
            if (g_SObj.readyState == 4) {
                if (g_SObj.status == 200) {

                    //Ajax Msg Receiver
                    AjaxMsgReceiver(g_SObj.responseXML.documentElement);
                    g_SRoot = g_SObj.responseXML.documentElement;
                    LoadSites();

                }
            }
        }

        function LoadSites() {

            $select = $("#selSiteList");
            $select.empty();

            var o_SiteId = g_SRoot.getElementsByTagName('SiteId');
            var o_SiteName = g_SRoot.getElementsByTagName('SiteName');

            var nRootLength = o_SiteId.length;

            if (nRootLength > 0) {

                for (var i = 0; i <= nRootLength - 1; i++) {
                    var SiteId = (o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
                    var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);

                    $select.append($("<option>", { "value": SiteId, "text": SiteName }));
                }

                $('#selSiteList').multipleSelect('refresh');
                $('input:radio[id=rdoOn]').prop('checked', true);

                getSitelistForsettings(1);
            }
        }

        function SaveSites() {

            var selSites = $("#selSiteList").val();

            if (setundefined(selSites) == '') {
                alert("Select Atleast One Site!");
                $("#selSiteList").focus();
                return false;
            }

            if ((document.getElementById("rdoOn").checked == false) && (document.getElementById("rdoOff").checked == false)) {
                alert(" please select Status!");
                return false;
            }

            var Status = $("input[name='rdoStatus']:checked").val();
            addSitesForSettings(selSites, Status);

            return true;
        }

        function Cancel() {
            $('#selSiteList').multipleSelect('refresh');
        }

        $(document).ready(function () {
            $("input[name='rdoStatus']").change(function () {

                var Stat = $("input[name='rdoStatus']:checked").val();

                if (Stat == "0") {
                    document.getElementById("selSiteList").value = 0;
                    $("#selSiteList").multipleSelect("refresh");
                }
                else {
                    getSitelistForsettings(1);
                }
            });
        });

    </script>
    <script type="text/javascript">

        $('#selSiteList').multipleSelect({
            styler: function (value) {
                return 'font-size: 13px;';
            }
        });      

    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td style="padding-left: 25px;">
                <div id="tooltip3" class="tool3" style="display: none;">
                </div>
                <div id="divAllowStarSetting" style="top: auto; left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="tblHeader"
                                    runat="server">
                                    <tr style="height: 10px;">
                                        <td>
                                            <input type="hidden" id="hid_userrole" runat="server" />
                                            <input type="hidden" id="hid_userid" runat="server" />
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
                                                                    <a href="AdminSettings.aspx">
                                                                        <img src='images/Left-Arrow.png' alt='' title='Settings' border="0" /></a>
                                                                </td>
                                                                <td style='width: 15px;' valign="top">
                                                                </td>
                                                                <td align='left' valign="top" style="width: 581px;">
                                                                    <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                                        <tr>
                                                                            <td align='left' class='SHeader1'>
                                                                                Settings
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Allow Star Settings
                                                                            </td>
                                                                            <td colspan="2" align="left" style="color: Green;" class="clsFilterCriteria">
                                                                            </td>
                                                                            <td align="right">
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
                        <tr>
                            <td valign="top">
                                <table id="tblStarSettings" cellspacing="1" cellpadding="1" style="width: 100%; padding-top: 10px;"
                                    class="display">
                                    <tr>
                                        <td align="left" style="width: 100px;" class="clsLALabel">
                                            Site :
                                        </td>
                                        <td align="left">
                                            <select multiple="multiple" id="selSiteList" name="selSiteList" style="width: 550px;">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="clsLALabel" style="width: 100px;">
                                                            Status :
                                                        </td>
                                                        <td class="clsFilterCriteria" style="text-align: left; font-weight: bold; font-size: 12px;
                                                            line-height: 2;">
                                                            <input type="radio" id="rdoOn" name="rdoStatus" value="1" />
                                                            <span>On</span>&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td class="clsFilterCriteria" style="text-align: left; font-weight: bold; font-size: 12px;
                                                            line-height: 2;">
                                                            <input type="radio" id="rdoOff" name="rdoStatus" value="0" /><span>Off</span>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <input type="button" value="Save" style="width: 100px; height: 30px;" class="clsExportExcel"
                                                name="btnSave" id="btnSave" runat="server" onclick="SaveSites();" />
                                            &nbsp;&nbsp;
                                            <input type="button" id="btnCancel" value="Cancel" style="width: 100px; height: 30px;"
                                                class="clsExportExcel" onclick="Cancel();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 15px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="clsLALabel">
                                <span>Allowed Sites :</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divSiteList" runat="server">
                                    <table cellpadding="0" border="0" cellspacing="0" width="98%">
                                        <tr>
                                            <td>
                                                <table cellpadding="0" border="0" cellspacing="0" width="100%" id="tblSites">
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                </div>
            </td>
        </tr>
    </table>
</asp:content>
