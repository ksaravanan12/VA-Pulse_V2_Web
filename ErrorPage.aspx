<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ErrorPage.aspx.vb" Inherits="GMSUI.ErrorPage"
    Title="Error Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Error</title>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="shortcut icon" href="centrakicon.ico" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="tblMain" cellpadding="0" cellspacing="0" border="0" style="width: 100%;
            height: 732px;" align="center">
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
            <tr valign="top">
                <td align="center" valign="top">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 1100px;">
                        <tr style="height: 150px;">
                        </tr>
                        <tr>
                            <td align="center" class="Lheader1">
                                Error
                            </td>
                        </tr>
                        <tr style="height: 15px;">
                        </tr>
                        <tr class="clsMapErrorTxt">
                            <td align="center" style="width: 100%;">
                                Error Desc :
                                <asp:label id="lblError" runat="server"></asp:label>
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
