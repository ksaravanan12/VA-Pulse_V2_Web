<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ImportHWDetails.aspx.vb" Inherits="GMSUI.ImportHWDetails" Title="Import Device Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript">

        this.onload = function () {
            document.getElementById('trImportLocation').style.display = "";
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
        }

        
        function UpdateLocation() {

            var SiteId = $("#ctl00_ContentPlaceHolder1_drpSites").val();
            if (SiteId == 0) {
                alert("Select Site!.");
                return;
            }
           
            var stitle = "Import RTLS certified devices";
            SiteId = 2;
            $("#ifrmUploadExcel").attr("src", "RTLSimport.aspx?Cmd=RTLSDetail&SiteId=" + SiteId);

            //Open Dialog
            $("#dialog-UploadExcelFile").dialog({
                height: 200,
                width: 600,
                modal: true,
                title: stitle,
                show: {
                    effect: "fade",
                    duration: 500
                },
                hide: {
                    effect: "fade",
                    duration: 500
                }
            });
        }

    </script>
    <!-- EMAIL LIST -->
    <div style="top: auto; left: auto; height: 850px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="tblHeader"
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
                                        <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='Redirect();'>
                                            <a href="AdminSettings.aspx">
                                                <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
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
                                                        Import RTLS Hardware details&nbsp;
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
                        <tr id="trImportLocation" style="display: none;">
                            <td valign="top" style="padding-left: 30px;">
                                <div>
                                    <table width="100%" cellpadding="5" cellspacing="1" class="clsFilterCriteria" border="0">
                                        <tr>
                                            <td align="left" style="width: 100px;">
                                                <b>Site :</b>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList CssClass="wrapper-dropdown" ID="drpSites" runat="server" Width="280px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="left">
                                                <input type="button" id="btnimportLocation" value="Import" style="width: 100px;"
                                                    class="clsButton" onclick="UpdateLocation();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <!-- UPLOAD Excel VIEW -->
    <div id="dialog-UploadExcelFile" title="Import Device Location Excel File" style="display: none;">
        <iframe id="ifrmUploadExcel" style="border: none; height: 132px; width: 500px;">
        </iframe>
    </div>
</asp:Content>
