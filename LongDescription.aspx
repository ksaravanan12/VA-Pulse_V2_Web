<%@ Page Title="Announcement Detail" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" ValidateRequest="false" CodeFile="LongDescription.aspx.vb"
    Inherits="LongDescription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript" src="Javascript/js_Announcements.js"></script>
    <script language="javascript" type="text/javascript">

        var Pageref = 0;
        var GSiteId = 0;

        $(document).ready(function () {
          
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;

            var AnnouncementId = document.getElementById("<%=hdnAnnouncementId.ClientID%>").value;
            Pageref = document.getElementById("<%=hdnpageref.ClientID%>").value;

            if (AnnouncementId != "0" && AnnouncementId != "") {               
                GetHTMLAnnouncement(AnnouncementId);
            }           
        });

        function redirectToHome() {

            if (Pageref == 0) {
                if (setundefined(GSiteId) == "") {
                    GSiteId = 0;
                }
                location.href = "Home.aspx?sid=" + GSiteId;
            }
            else {
                location.href = 'PastAnnouncements.aspx';
            }
        }

    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <input type="hidden" id="hdnAnnouncementId" runat="server" />
                <input type="hidden" id="hdnpageref" runat="server" />
                <div id="divAnnouncements" runat="server">
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
                                                        <a onclick="redirectToHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1' id="txtUpdatedOn">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Announcement Details
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align='left' style="font-weight: 400;" class='subHeader_black'>
                                                                                Message will expire <span id='spnExpireOn'></span>
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
                                                        <div id="divContent">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="display: none; position: fixed; top: 325px; left: 1000px;" id="divLoading_AnnouncementList">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
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
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
