<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AdminSettings.aspx.vb" Inherits="GMSUI.AdminSettings" Title="Admin Settings" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <script type="text/javascript">

        var GSiteId;

        this.onload = function () {
            document.getElementById('trSettings').style.display = "";
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
        }

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
                <div id="divAdminSettings" runat="server">
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
                                                                <td align='left' class='SHeader1'>
                                                                    Settings
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Admin Settings
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
                        <tr id="trSettings" style="display: none;">
                            <td valign="top" style="padding-left: 30px;">
                                <table border="0" cellpadding="7" cellspacing="7" width="100%">
                                    <tr runat="server" id="trManageCompanies">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="CompanyList.aspx">Manage Companies</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trManageSites">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="SiteList.aspx">Manage Sites</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trManageUsers">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="UserList.aspx">Manage Users</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trRoleMapping">
                                        <td>
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="ManageRoleMapping.aspx">Manage Role Mapping</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trManageAnnouncements">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="Announcements.aspx">Manage Announcements</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trImportDeviceLocation">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="ImportDeviceName.aspx">Import Device Name</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trAuditLog">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="UserActivityDetails.aspx">User Audit Log</a>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trConfig">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="AccountInactivePeriod.aspx">Configuration</a>
                                        </td>
                                    </tr>
                                    <tr id="trPuseInactive" runat="server">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="PulseInactiveStatus.aspx">Pulse Connection Inactive
                                                Status Duration</a>
                                        </td>
                                    </tr>
                                    <tr id="trstarsetting" runat="server">
                                        <td style="width: 10px;" align="center">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="AllowStarSettings.aspx">Allow Star Settings</a>
                                        </td>
                                    </tr>
                                    <tr id="trTagProfiles" runat="server">
                                        <td style="width: 10px;">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="INIEditor.aspx">Configure Tag Profiles</a>
                                        </td>
                                    </tr>
                                    <tr id="trAD" runat="server">
                                        <td style="width: 10px;">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="ADConfiguration.aspx">Active Directory Configuration</a>
                                        </td>
                                    </tr>
                                    <tr id="trCompanyAssociation" runat="server">
                                        <td style="width: 10px;">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="CompanyAssociation.aspx">AD Group Association</a>
                                        </td>
                                    </tr>
                                    <tr id="trSiteAssociation" runat="server">
                                        <td style="width: 10px;">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="SiteAssociationAD.aspx">GMS Site Association With
                                                AD Group</a>
                                        </td>
                                    </tr>
                                    <tr id="trRTLSDetails" runat="server">
                                        <td style="width: 10px;">
                                            <div class='rmaBlueCircle'>
                                            </div>
                                        </td>
                                        <td align='left' class='subHeader_black'>
                                            <a class='alert_normal_Blue' href="ImportHWDetails.aspx">Import RTLS Details</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:content>
