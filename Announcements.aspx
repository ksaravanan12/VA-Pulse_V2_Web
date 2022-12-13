<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Announcements.aspx.vb" Inherits="GMSUI.Announcements" Title="Announcements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <script language="javascript" type="text/javascript" src="Javascript/js_Announcements.js?d=17"></script>
    <%--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
    <link rel="stylesheet" href="https://www.jqwidgets.com/public/jqwidgets/styles/jqx.base.css"
        type="text/css" />
    <link rel="stylesheet" href="https://www.jqwidgets.com/public/jqwidgets/styles/jqx.energyblue.css"
        type="text/css" />
    <script type="text/javascript" src="https://www.jqwidgets.com/public/jqwidgets/jqx-all.js"></script>
    <script type="text/javascript" src="https://www.jqwidgets.com/public/jqwidgets/globalization/globalize.js"></script>
    <script src="jqHtmlEditor/app.js" type="text/javascript"></script>
    <link href="jqHtmlEditor/app.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var g_UserId = "";
        $(document).ready(function () {
            ClearAnnouncementValues()
            GetAnnouncements(1);
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
        });

        function redirectToHome() {
            location.href = 'AdminSettings.aspx';
        }

        $(document).ready(function () {
            //Events
            $('#btnShow').on('click', function () {
                if ($("#txtMessage").val() == '') {
                    alert("Enter Message!!!");
                    $("#txtMessage").focus();
                    return false;
                }

                if ($("#date_timepicker_start").val() == '') {
                    alert("Select Start Date!!!");
                    $("#date_timepicker_start").focus();
                    return false;
                }

                if ($("#date_timepicker_end").val() == '') {
                    alert("Select End Date!!!");
                    $("#date_timepicker_end").focus();
                    return false;
                }

                if ($("#ctl00_ContentPlaceHolder1_drpUserRole").val() == 0) {
                    alert("Select any User Role!!!");
                    $("#ctl00_ContentPlaceHolder1_drpUserRole").focus();
                    return false;
                }
                if ($('input[name="ChkdispafterExpire"]').is(':checked')) {
                    if ($("#txtdaysDispafterExpire").val() == '') {
                        alert("Enter Days After End Date to continue displaying until seen !!!");
                        $("#txtdaysDispafterExpire").focus();
                        return false;
                    }
                    if ($("#txtdaysDispafterExpire").val() == '' || $("#txtdaysDispafterExpire").val() == 0) {
                        alert("Days After End Date to continue displaying until seen should be greater than 0");
                        $("#txtdaysDispafterExpire").focus();
                        return false;
                    }
                }
                UpdateAnnouncements();

                try {
                    PageVisitDetails(g_UserId, "Announcements", enumPageAction.Edit, "Announcements Updated");
                }
                catch (e) {

                }
            });

            $(document).on('click', '#btnCancel', function () {
                ClearAnnouncementValues();
                $('#tblAnnouncements tr:not(:first-child) td').css({ "background-color": "#FFFFFF", "color": "#454545" });
            });
            $("#btnEdit").click(function () {
                $("#trlongDesc").show();
            });
                    
			$("#ctl00_ContentPlaceHolder1_btnUpload").click(function () {
                if ($("#ctl00_ContentPlaceHolder1_FileUpload1").val() == '') {
                    alert("Pleae select file to upload.!!!");
                    $("#ctl00_ContentPlaceHolder1_FileUpload1").focus();
                    return false;
                }
            });
        });
    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <input type="hidden" id="hid_userid" runat="server" />
                <div id="divAnnouncements" runat="server">
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
                                                            <img src='images/Left-Arrow.png' title='Settings' style="width: 16px; height: 24px;"
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
                                                                                Announcements
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
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="5" cellspacing="5" width="100%" class="clsFilterTable">
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px; vertical-align: middle;">
                                            Message&nbsp;:&nbsp;
                                        </td>
                                        <td class="clsLALabel" colspan="3">
                                            <textarea id="txtMessage" cols="70" rows="2" style="border: solid 1px #D8D8D8; width: 520px;"
                                                class="clsTextAreaBox" maxlength="8000"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px; vertical-align: middle;">
                                            Long Description&nbsp;:&nbsp;
                                        </td>
                                        <td class="clsLALabel" style="width: 180px;" colspan="3">
                                            <span>
                                                <label for="chkIsActive">
                                                    Active
                                                </label>
                                                <input type="checkbox" id="chkIsActive" style="vertical-align: middle;" /></span>
                                            <span>
                                                <input type="button" id="btnEdit" value="Edit" style="width: 130px; height: 30px;"
                                                    class="clsExportExcel" /></span>
                                        </td>
                                    </tr>
                                    <tr id="trlongDesc">
                                        <td class="clsLALabel" colspan="4">
                                           <%-- <div style="text-align: right; padding-bottom: 8px;">
                                                <span>
                                                    <input type="button" id="btnSave" value="Save" style="width: 130px; height: 30px;"
                                                        class="clsExportExcel" /></span><span>
                                                            <input type="button" id="btnCancels" value="Cancel" style="width: 130px; height: 30px;"
                                                                class="clsExportExcel" /></span>
                                            </div>--%>                                           

                                            <textarea id="editor"></textarea>
                                            <div>
                                                <div id="divImageList" style="padding-top: 20px;" runat="server">
                                                </div>
                                                <hr />
                                                <br />
                                                <br />
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                                <asp:Button ID="btnUpload" Text="Upload" runat="server" OnClick="UploadFile" class="clsExportExcel" />
                                                <br />
                                                <br />
                                                <asp:Label ID="lblMessage" ForeColor="Green" runat="server" />
                                                <hr />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px;">
                                            <b>Start&nbsp;Date&nbsp;:&nbsp;</b>
                                        </td>
                                        <td style="width: 180px;">
                                            <input type="text" id="date_timepicker_start" style="width: 140px; text-align: left;"
                                                class="clsLATextbox" />
                                        </td>
                                        <td class="clsLALabel" style="width: 60px;">
                                            <b>End&nbsp;Date&nbsp;:&nbsp;</b>
                                        </td>
                                        <td style="width: 180px;">
                                            <input type="text" id="date_timepicker_end" style="width: 140px; text-align: left;"
                                                class="clsLATextbox" />
                                        </td>
                                    </tr>
                                         <tr>
                                        <td class="clsLALabel" style="width: 60px;">
                                           <b>Display&nbsp;After&nbsp;Expiration:&nbsp;</b></td>
                                        <td colspan="3" class="clsLALabel">
                                         <span>
                                            <label >
                                                    Display for users who have not logged in:
                                                </label>&nbsp; <input type="checkbox" id="ChkdispafterExpire" name="ChkdispafterExpire" style="vertical-align: middle;" /></span>
                                                </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px;">
                                            &nbsp;</td>
                                        <td colspan="3" class="clsLALabel">
                                         <span>
                                            <label >
                                                    Days after End Date to continue displaying until seen :
                                                </label>&nbsp;   <input type="text" id="txtdaysDispafterExpire" style="width: 50px; text-align: left;"
                                                class="clsLATextbox" /></span>
                                                </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px;">
                                              <b>Display&nbsp;in&nbsp;History:&nbsp;Screen:</b></td>
                                        <td style="width: 180px;">
                                            <input type="checkbox" id="chkDispHistory" style="vertical-align: middle;" /></td>
                                        <td class="clsLALabel" style="width: 60px;">
                                            &nbsp;</td>
                                        <td style="width: 180px;">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px;">
                                            <b>User&nbsp;Role&nbsp;:&nbsp;</b>
                                        </td>
                                        <td>
                                            <select multiple id="drpUserRole" runat="server" style="width: 150px;">
                                            </select>
                                        </td>
                                        <td class="clsLALabel" style="width: 60px;">
                                            <b>Displaying&nbsp;:&nbsp;</b>
                                        </td>
                                        <td style="width: 180px;">
                                            <input type="checkbox" id="chkIsShow" style="vertical-align: middle;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="clsLALabel" style="width: 60px;">
                                            <b>User's&nbsp;Associated&nbsp;With&nbsp;Views&nbsp;:&nbsp;</b>
                                        </td>
                                        <td>
                                            <input type="checkbox" id="chkBatterySummary" style="vertical-align: middle;" value="15" /><span
                                                class="clsLALabel">Battery&nbsp;Summary&nbsp;View&nbsp;</span>
                                        </td>
                                        <td class="clsLALabel" style="width: 60px;">
                                            <b>Show In</b>
                                        </td>
                                        <td style="width: 180px;">
                                            <select id="SelShowIn" runat="server" style="width: 150px;">
                                                <option value="1">Both</option>
                                                <option value="2">2X</option>
                                                <option value="3">3X</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <input type="hidden" id="hdnAnnouncementId" />
                                            <input type="button" id="btnShow" value="Save" style="width: 130px; height: 30px;"
                                                class="clsExportExcel" />
                                            <input type="button" id="btnCancel" value="Cancel" style="width: 130px; height: 30px;"
                                                class="clsExportExcel" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divAnnouncementsList" runat="server">
                    <table cellpadding="0" border="0" cellspacing="0" width="98%">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td align="right">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr style="height: 40px;">
                                                    <td class="txttotalpage" style="width: 275px;" valign="middle">
                                                        <asp:Label ID="lblCount_Announcement" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div style="display: none;" id="divLoading_AnnouncementList">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                    </td>
                                                    <td class="clsTableTitleText" align="right">
                                                        <input type="button" id="btnPrev_Announcement" class="clsPrev" onclick="AnnouncementPgView('3');"
                                                            title="Previous" />
                                                        <asp:Label ID="lblPg_Announcement" runat="server" CssClass="clsCntrlTxt"> Page </asp:Label>
                                                        <input id="txtPgNo_Announcement" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                            type="text" size="1" maxlength="4" runat="server" name="txtPageNo" value="1" />
                                                        <asp:Label ID="lblPgCnt_Announcement" runat="server" CssClass="clsCntrlTxt">&nbsp;</asp:Label>&nbsp;
                                                        <input type="button" id="btnGo_Announcement" class="btnGO" value="Go" onclick="AnnouncementPgView('1');" />&nbsp;&nbsp;
                                                        <input type="button" id="btnNext_Announcement" class="clsNext" onclick="AnnouncementPgView('2');"
                                                            title="Next" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" border="0" cellspacing="0" width="100%" id="tblAnnouncements">
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
        <script type="text/javascript">
            $('#datetimepicker').datetimepicker()
	            .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });

            $(function () {
                $('#date_timepicker_start').datetimepicker({
                    format: 'Y/m/d H:i',
                    step: 10,
                    timepicker: true
                });

                $('#date_timepicker_end').datetimepicker({
                    format: 'Y/m/d H:i',
                    step: 10,
                    timepicker: true
                });
            });
        </script>
        <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script>
        <script type="text/javascript">
            $('#ctl00_ContentPlaceHolder1_drpUserRole').multipleSelect();
        </script>
    </table>
</asp:Content>
