<%@ Page Title="Log Viewer" Language="VB" AutoEventWireup="false" CodeFile="ActivityLog.aspx.vb"
    Inherits="GMSUI.ActivityLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log Viewer</title>
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!-- Log Viewer LIST -->
    <div id="divLogList" style="top: auto; left: auto; height: 850px;">
        <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 5px;">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; vertical-align: top;">
                        <tr>
                            <td colspan="2" align="left" class="subHeader_black3">
                                Location data &nbsp;
                                <br />
                                <br />
                                <div style="width: 1800px; height: 500px; overflow: auto;">
                                    <table id="tblLocation" class="dashboardText" border="0" cellspacing="0" cellpadding="5"
                                        style="border: solid 1px #D3E5F3; vertical-align: top;" runat="server">
                                    </table>
                                </div>
                            </td>
                            <td style="width: 50px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <img style="height: 20px; width: 100%" src="Images\hLine.png" />
                            </td>
                        </tr>
                        <tr id="trPage_Res" runat="server">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" class="subHeader_black3">
                                            Page request&nbsp;#<span id="noofPages" runat="server" class="logSummary3"></span><br />
                                            <br />
                                            <div style="height: 300px; overflow: auto;">
                                                <table id="tblPageReq" class="dashboardText" border="0" cellspacing="0" cellpadding="5"
                                                    style="border: solid 1px #D3E5F3;" runat="server">
                                                </table>
                                            </div>
                                        </td>
                                        <td style="width: 100px;">
                                        </td>
                                        <td align="left" class="subHeader_black3">
                                            Page response&nbsp;#<span id="noofRes" runat="server"></span> <br />
                                            <br />
                                            <div style="height: 300px; overflow: auto;">
                                                <table id="tblPageRes" class="dashboardText" border="0" cellspacing="0" cellpadding="5"
                                                    style="border: solid 1px #D3E5F3;" runat="server">
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
        </table>
    </div>
    </form>
</body>
</html>
