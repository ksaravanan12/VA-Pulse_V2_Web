<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PastAnnouncements.aspx.vb" Inherits="GMSUI.PastAnnouncements" Title="PastAnnouncements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript" src="Javascript/js_Announcements.js?d=22"></script>
    <script type="text/javascript">
        var GSiteId = 0;
        $(window).load(function () {
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
            PastAnnouncements();
        });

        function redirectToHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
            location.href = "Home.aspx?sid=" + GSiteId;
        }
       
    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <div id="divPastAnnouncements" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" width="85%">
                        <tr style="height: 20px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="redirectToHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;" class="subHeader_black">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeaderAssetTrack' style="height: 50px;" valign="middle">
                                                                    Past Announcements
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        &nbsp;
                                                    </td>
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
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblpastannouncement"
                                    style="border: 1px solid #DADADA; padding-left: 10px;">
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position: fixed; top: 400px; left: 1100px; display: none;" id="divLoading">
                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
