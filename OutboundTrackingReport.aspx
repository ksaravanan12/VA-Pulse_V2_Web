<%@ Page Title="Outbound Tracking Report" Language="VB" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="false" CodeFile="OutboundTrackingReport.aspx.vb" Inherits="GMSUI.OutboundTrackingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   

    <div id="divReportDetails" style="top: auto; left: auto; height: 850px;">
        <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="tblHeader"
                        runat="server">
                        <tr style="height: 10px;">
                            <td>&nbsp;
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
                                                    <td style='width: 15px;' valign="top"></td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                            <tr>
                                                                <td align='left' class='SHeader1'>Connect Pulse Reports
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>Outbound Tracking Report
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top" colspan="3">&nbsp;
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 5px;">
                <td valign="top" colspan="3">&nbsp;
                </td>
            </tr>
            <tr valign="top" id="trFilter" runat="server" style="width: 176px; font-size:13px;">
                <td style="padding-left: 10px;">
                    <table>
                        <tr style="height: 20px;">
                            <td class="clsLALabel" align="left" style="width: 145px;">Select Site:
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlSites" runat="server" Width="466px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 25px;"></td>
                            <td class="clsLALabel" align="left" style="width: 40px;">Month:
                            </td>                           
                            <td class="clsDispText">
                                <asp:DropDownList ID="ddMonth" runat="Server" Width="100px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 25px;"></td>
                           <td class="clsLALabel" align="left" style="width: 30px;">Year:
                            </td> 
                            <td class="clsDispText">
                                <asp:DropDownList ID="ddlYear" runat="Server" Width="100px" AutoPostBack="True">                               
                                </asp:DropDownList>
                            </td>                           
                            <td style="width: 25px;"></td>                             
                            <td class="clsDispText">
                                <asp:Button ID="btnExport" runat="server" Text="Export" style="width: 80px; height: 30px;"  CssClass="clsExportExcel" OnClick="btnExport_Click" />
                            </td>
                            <td class="clsDispText"></td>
                            <td class="clsDispText" style="visibility:hidden">
                                <asp:CheckBox ID="chkIsDetailReport" Text="IsDetailReport" TextAlign="Right" runat="server" AutoPostBack="True" Checked="false" />
                            </td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="divSummaryReport" runat="server" style="width: 1300px; height: 370px; vertical-align: top; padding-top: 10px;">
                <td>
                    <table style="width: 100%; height: 250px;" cellspacing="0" cellpadding="0" border="0"
                        align="center">                      
                        <tr valign="top">
                            <td style="padding-left: 8px;" valign="top">
                                <table id="tblCountData" class="clsFilterTable"  runat="server" cellspacing="0" cellpadding="10">
                                    <tr>
                                        <td>#</td>
                                        <td style="width: 700px;font-size:13px;" class="clsLALabel">Site Name</td>
                                        <td style="width: 200px;font-size:13px;" class="clsLALabel">Communication Type</td>
                                        <td style="width: 110px;font-size:13px;" class="clsLALabel">No of Email Sent</td>
                                    </tr>
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
                                <div style="position: fixed; top: 350px; z-index: 1; display: none;" id="divExcelLoading">
                                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
