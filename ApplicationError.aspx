<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ApplicationError.aspx.vb" Inherits="GMSUI.ApplicationError" Title="CenTrak :: Application Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CenTrak :: Application Error</title>
      <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="shortcut icon" href="centrakicon.ico" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table id="tblMain" cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 732px; vertical-align:middle;" align="center">
            <tr>
                <td align="center" valign="middle">
                    <table cellpadding="5" cellspacing="0" border="0">
                        <tr>
                            <td class="login-logo">
                            </td>
                        </tr>
                        <tr>
                            <td class="clsErrorinfo" align="center">Application Error</td>
                        </tr>
                        <tr>
                            <td id="lblErrDescription" runat="server" class="clsErrorinfo" align="center"></td>
                        </tr>
                        <tr>
                            <td align="center" class="clsErrorinfo">
                                Go back to the <a href="login.aspx">login </a>page
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
