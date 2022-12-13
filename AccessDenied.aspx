<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AccessDenied.aspx.vb" Inherits="AccessDenied" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Connect Pulse - Access Denied</title>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <script type="text/javascript">
        function GoBack() {
            window.history.back();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table id="tblMain" cellpadding="0" cellspacing="0" border="0" style="width: 100%;
        height: 200px;" align="center">
        <tr style="height: 80px;">
            <td align="center" valign="top">
                <table cellpadding="0" cellspacing="0" border="0" class="shadow">
                    <tr>
                        <td valign="middle">
                            <table cellpadding="0" cellspacing="0" border="0" style="width: 300px; border: solid 0px red;">
                                <tr>
                                    <td valign="top" align="center">
                                        <img src="Images/Logo.png" style="width: 233px; height: 40px;" alt="Logo" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="middle">
                            <table cellpadding="0" cellspacing="0" border="0" style="width: 800px;">
                                <tr>
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 685px; padding-left: 45px;">
                                            <tr>
                                                <td align="right" style="width: 50px;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center" valign="top" style="height: 8px; background-color: #005695; width: 1100px;">
            <td>
            </td>
        </tr>
        <tr>
            <td style="height: 50px">
            </td>
        </tr>      
        <tr>
            <td align="center">
                <div style="border: solid 1px #cccccc; height: 150px; width: 400px; background-color: #fff7f8;">
                    <table cellpadding="10" cellspacing="0" border="0">
                        <tr>
                            <td class="login-security" id="tdAlerttitile" runat="server">
                                Privilege security
                            </td>
                        </tr>
                        <tr style="height: 100px;">
                            <td id="tdAlertmsg" runat="server" class="login-alertmessage">
                                <span>Access denied - You are not authorized to access this page</span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" onclick="GoBack();" value="Back" class="clsExportExcel" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
