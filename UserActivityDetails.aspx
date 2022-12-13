<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeFile="UserActivityDetails.aspx.vb" Title="User Activity" Inherits="GMSUI.UserActivityDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
    <style type="text/css">
        @import url( Javascript/Calendar/calendar-blue2.css );
    </style>
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
     <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupUser.js"></script>
       
    <script type="text/javascript" src="Javascript/jquery.plugin.js"></script>
    
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link rel="stylesheet" type="text/css" href="scripts/jquery-ui-1.7.2.custom.css" />    
    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="Javascript/jquery-ui.min.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js"></script> 

    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>

    <script language="javascript" type="text/javascript">
        this.onload = function () {
            document.getElementById('tdLeftMenu').style.display = "none";
        }

        function Redirect(sid) {
            location.href = "Setting.aspx";
        }

        function GetActivity() {

            var UserId = document.getElementById("<%=ddlUserList.ClientID%>").value;
            var FromDate = document.getElementById("<%=txFromDate.ClientID%>").value;
            var ToDate = document.getElementById("<%=txtToDate.ClientID%>").value;
            var UserName = document.getElementById("<%=ddlUserList.ClientID%>").text;
            var EventType = document.getElementById("<%=ddlEventtype.ClientID%>").value;

            document.getElementById("divLoading").style.display = "";
            Load_UserActivity(UserId, FromDate, EventType, ToDate);

        }

        // Date Picker control
        $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39', step: 1 });
        $(function () {

            $('#ctl00_ContentPlaceHolder1_txFromDate').datetimepicker({
                format: 'Y/m/d',
                step: 10,
                timepicker: false
            });

            $('#ctl00_ContentPlaceHolder1_txtToDate').datetimepicker({
                format: 'Y/m/d',
                step: 10,
                timepicker: false
            });
        });
                
    </script>
               
    <div id="tooltip3" class="tool3" style="display: none;">
    </div>
    <!-- User activity list -->
    <div id="divUserActivity" style="top: auto; left: auto; height: 850px;">
        <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="tblHeader"
                        runat="server">
                        <tr style="height: 10px;">
                            <td>
                                &nbsp;
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
                                                                    Admin Actions Audit Log
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
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="5" cellspacing="5" width="100%" class="clsFilterRow">
                                                <tbody>
                                                    <tr>
                                                        <td class="clsLALabel">
                                                            Users :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlUserList" runat="server" Style="width: 200px;">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="clsLALabel">
                                                            <b>From&nbsp;Date&nbsp;:&nbsp;</b>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txFromDate" name="txFromDate" runat="server" style="width: 100px;
                                                                text-align: left;" class="clsLATextbox">
                                                        </td>
                                                        <td class="clsLALabel">
                                                            <b>Event&nbsp;Type&nbsp;:&nbsp;</b>
                                                        </td>
                                                        <td>
                                                            <select class="clsLADrop" id="ddlEventtype" runat="server" style="width: 150px;">
                                                            </select>
                                                        </td>
                                                        <td class="clsLALabel">
                                                            <b>To&nbsp;Date&nbsp;:&nbsp;</b>
                                                        </td>
                                                        <td>
                                                            <input type="text" id="txtToDate" name="txtToDate" runat="server" style="width: 100px;
                                                                text-align: left;" class="clsLATextbox">
                                                        </td>
                                                        <td colspan="4" align="center">
                                                            <input type="button" value="Show" style="width: 80px; height: 30px;" class="clsExportExcel"
                                                                name="btnShowActivity" id="btnactivity" runat="server" onclick="GetActivity();" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 5px;">
                <td valign="top" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table id="tblUserActivityInfo" cellspacing="1" cellpadding="3" style="width: 100%;
                        padding-top: 10px;" class="display">
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:Content>
