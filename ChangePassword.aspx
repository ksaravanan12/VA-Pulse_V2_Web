<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb"
    Inherits="GMSUI.ChangePassword" Title="GMS-ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Connect Pulse - Change Password</title>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="shortcut icon" href="centrakicon.ico" type="image/x-icon" />
    <script src="Javascript/jquery.js" type="text/javascript"></script>
    <script language="javascript">

        $(document).ready(function () {
            $("#txtUsername").focus(function (srcc) {
                if ($(this).val() == $(this)[0].title) {
                    $(this).removeClass("defaultTextActive");
                    $(this).val("");
                }
            });

            $("#txtUsername").blur(function () {
                if ($(this).val() == "") {
                    $(this).addClass("defaultTextActive");
                    $(this).val($(this)[0].title);
                }
            });
            $("#txtPassword").focus(function (srcc) {
                if ($(this).val() == $(this)[0].title) {

                    $(this).removeClass("defaultTextActive");
                    $(this).val("");
                    $(this)[0].type = "password";
                }
            });

            $("#txtPassword").blur(function () {
                if ($(this).val() == "") {
                    $(this).addClass("defaultTextActive");
                    $(this).val($(this)[0].title);
                    // $(this)[0].type="text";
                }
            });

            $("#txtChangePassword").focus(function (srcc) {
                if ($(this).val() == $(this)[0].title) {

                    $(this).removeClass("defaultTextActive");
                    $(this).val("");
                    $(this)[0].type = "password";
                }
            });

            $("#txtChangePassword").blur(function () {
                if ($(this).val() == "") {
                    $(this).addClass("defaultTextActive");
                    $(this).val($(this)[0].title);
                    // $(this)[0].type="text";
                }
            });

            $("#txtUsername").blur();
            $("#txtPassword").blur();
            $("#txtChangePassword").blur();

        });

        function Validate() {
            var Password = document.getElementById('txtPassword').value;
            var ConfirmPassword = document.getElementById('txtConfirmPassword').value;

            if (Password == "") {
                alert("Password should not be empty");
                return false;
            }
            else if (ConfirmPassword == "") {
                alert("Confirm Password should not be empty");
                return false;
            }
            else if (Password != ConfirmPassword) {
                alert("Password and Confirm Password should be equal");
                return false;
            }
            return true;

        }
       
     
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="tblMain" cellpadding="0" cellspacing="0" border="0" style="width: 100%;
            height: 732px; vertical-align: middle;" align="center">
            <tr>
                <td align="center" valign="middle">
                    <table id="tblChangedPw" runat="server" style="display: none;" cellpadding="5" cellspacing="0"
                        border="0">
                        <tr>
                            <td class="login-logo">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="defaultTextOnly">
                                Reset password link is aleardy used
                            </td>
                        </tr>
                    </table>
                    <table id="tblBeforeChangedPw" runat="server" cellpadding="5" cellspacing="0" border="0">
                        <tr>
                            <td class="login-logo">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="10" cellspacing="0" border="0" style="width: 400px">
                                    <tr>
                                        <td class="defaultTextOnly" align="left">
                                            New Password :
                                        </td>
                                        <td>
                                            <input type="text" maxlength="50" class="defaultText" id="txtPassword" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="defaultTextOnly" align="left">
                                            Confirm Password :
                                        </td>
                                        <td>
                                            <input type="text" maxlength="50" class="defaultText" id="txtConfirmPassword" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="10" cellspacing="0" border="0" style="width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <input id="btnChangePwd" type="submit" runat="server" onclick="return Validate();"
                                                class="forgotpwd" value="Change Password" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:regularexpressionvalidator id="Regex3" runat="server" controltovalidate="txtPassword"
                                    validationexpression="^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[0-9])|(?=.*[A-Z])(?=.*[a-z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])).{8,32}$"
                                    errormessage="New password must contain: Minimum 8 characters and  must contain characters from 3 of the following 4 categories - UpperCase Alphabet,LowerCase Alphabet,Number and Special Character"
                                    forecolor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td id="tdAlert" runat="server" class="login-alertBox" style="display: none;">
                                Incorrect User Name or<br />
                                Password
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
