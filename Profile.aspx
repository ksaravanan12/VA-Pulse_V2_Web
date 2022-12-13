<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Profile.aspx.vb" Inherits="GMSUI.Profile" Title="User Profile" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        var g_UserRole = 0;
        var g_UpdateProfileObj;
        var g_UserId;
        var GSiteId = 0;
        this.onload = function () {
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;
        }

        $(function () {

            $('#btnupdate').click(function () {

                if (document.getElementById("txtOldpwd").value == "") {
                    alert("Please enter Old Password");
                    return;
                }
                else if (document.getElementById("txtNewpwd").value == "") {
                    alert("Please enter New Password");
                    return;
                }
                else if (document.getElementById("txtRetypePwd").value == "") {
                    alert("Please re-enter New Password");
                    return;
                }
                else if (document.getElementById("txtNewpwd").value != document.getElementById("txtRetypePwd").value) {
                    alert("New Password's didnt matched");
                    return;
                }

                $('#lblsavePasswordResult').html("");
                if (confirm("Are you sure do you want to change the password?") == true) {
                    $("#div_SavePasswordLoader").show();
                    UpdateProfile();
                }
            });
        });

        function UpdateProfile() {

            $.post("AjaxConnector.aspx?cmd=UpdateProfiles",
            {
                OldPassword: setundefined(document.getElementById("txtOldpwd").value),
                NewPassWord: setundefined(document.getElementById("txtNewpwd").value)
            },
            function (data, status) {
                if (status == "success") {
                    AjaxMsgReceiver(data.documentElement);
                    dsRoot = data.documentElement;
                    ajaxUpdateProfile(dsRoot);
                }
                else {
                    document.getElementById("div_SavePasswordLoader").style.display = "none";
                }
            });
        }

        //Ajax Request for Update Profile
        function ajaxUpdateProfile(dsRoot) {

            document.getElementById("div_SavePasswordLoader").style.display = "none";

            var o_Result = dsRoot.getElementsByTagName('Result')
            var Result = getTagNameValue(o_Result[0]);

            var o_Error = dsRoot.getElementsByTagName('Error')
            var Error = getTagNameValue(o_Error[0]);

            if (Result == "0") {

                $('#lblsavePasswordResult').removeClass('clsMapErrorTxt');
                $('#lblsavePasswordResult').html('Successfully Updated').addClass('clsMapSuccessTxt');

                try {
                    PageVisitDetails(g_UserId, "User Profile", enumPageAction.Edit, "Password reset username - " + document.getElementById('<%=lblUsername.ClientID%>').innerText + " old password - " + document.getElementById("txtOldpwd").value + " new password - " + document.getElementById("txtNewpwd").value + "");
                }
                catch (e) {

                }
            }
            else {
                $('#lblsavePasswordResult').removeClass('clsMapSuccessTxt');
                $('#lblsavePasswordResult').html(Error).addClass('clsMapErrorTxt');
            }
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
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="tblprofile"
                    runat="server">
                    <tr style="height: 20px;">
                        <td>
                            <input type="hidden" id="hid_userrole" runat="server" />
                            <input type="hidden" id="hid_userid" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='Redirect();'>
                                        <a href="Home.aspx">
                                            <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                    </td>
                                    <td style='width: 15px;' valign="top">
                                    </td>
                                    <td align='left' valign="top" style="width: 581px;">
                                        <table border='0' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td align='left' class='SHeader1'>
                                                    My Profile
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr style="height: 5px;">
                                    <td class="bordertop" valign="top" colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left">
                            <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                <tr>
                                    <td style="width: 10px;">
                                        &nbsp;
                                    </td>
                                    <td align="left" class="Profile">
                                        User name:
                                        <asp:label id="lblUsername" cssclass="ProfileText" runat="server"></asp:label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;">
                                        &nbsp;
                                    </td>
                                    <td align="left" class="Profile">
                                        User role:
                                        <asp:label id="lblUserType" cssclass="ProfileText" runat="server"></asp:label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;">
                                        &nbsp;
                                    </td>
                                    <td align="left" class="Profile">
                                        User email:
                                        <asp:label id="lblUserEmail" cssclass="ProfileText" runat="server"></asp:label>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <input id="btnchangepwd" type="button" class="clschangepwd" value="Change Password"
                                            runat="server" onclick="showPwddiv();" />
                                    </td>
                                </tr>
                                <tr style="height: 20px;">
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divPwd" style="display: none;">
                                            <table border="0" cellpadding="3" cellspacing="0" style="padding-left: 30px;">
                                                <tr>
                                                    <td class="Profile">
                                                        Old Password :
                                                    </td>
                                                    <td>
                                                        <input id="txtOldpwd" class="txtBox" type="password" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Profile">
                                                        New Password :
                                                    </td>
                                                    <td>
                                                        <input id="txtNewpwd" class="txtBox" type="password" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Profile">
                                                        Retype Password :
                                                    </td>
                                                    <td>
                                                        <input id="txtRetypePwd" class="txtBox" type="password" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <input id="btncancel" type="button" runat="server" class="clsbtn" value="Cancel"
                                                            onclick="hidePwddiv();" />&nbsp;&nbsp;&nbsp;<input id="btnupdate" type="button" class="clsbtn"
                                                                value="Update" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <div style="display: none;" id="div_SavePasswordLoader">
                                                            <img src="Images/712.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                        <label id="lblsavePasswordResult">
                                                        </label>
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
            </td>
        </tr>
        <script type="text/javascript">
            LoadGlossaryInfo("Profile", document.getElementById("<%=hid_userrole.ClientID%>").value);    
        </script>
    </table>
</asp:content>
