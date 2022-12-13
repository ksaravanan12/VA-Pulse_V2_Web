<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CentrakDevices.aspx.vb" Inherits="GMSUI.CentrakDevices" Title="Centrak Devices" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        var g_UserRole = 0;
        var g_UserId;
        var GSiteId = 0;

        this.onload = function () {
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
        }

        function sortFunc(id) {
                          
            var src = $("#" + id).attr('src').split('/');
            var file = src[src.length - 1];
           
            if (file == "uparrow.png") {             
                location.href = "CentrakDevices.aspx?sort=ModelItem&ord=desc";
            }
            else {              
                location.href = "CentrakDevices.aspx?sort=ModelItem&ord=asc";
            }                
        }

    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="85%">
        <tr style="height: 20px;">
        </tr>
        <tr>
            <td valign="top">
                <input type="hidden" id="hid_userrole" runat="server" />
                <input type="hidden" id="hid_userid" runat="server" />
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                            <a onclick="GoBack()">
                                <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                    border="0" /></a>
                        </td>
                        <td style='width: 15px;' valign="top">
                        </td>
                        <td align='left' valign="top" style="width: 581px;">
                            <table border='0' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td align='left' class='SHeader1'>
                                        Centrak Devices
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                            <tr>
                                                <td align='left' class='subHeader_black'>
                                                    Centrak Devices
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
                                Device Type:
                            </label>
                            <asp:dropdownlist id="ddlDeviceType" runat="server" width="90px" autopostback="true">
                            </asp:dropdownlist>
                        </td>
                        <td style="text-align: right">
                            <asp:button id="btnExport" runat="server" text="Export" style="width: 80px; height: 30px;"
                                cssclass="clsExportExcel" onclick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 10px;">
                <table id="tblBatteryCalculation" runat="server" border="0" cellpadding="5" cellspacing="0"
                    width="100%">    
                </table>
            </td>
        </tr>
    </table>
</asp:content>
