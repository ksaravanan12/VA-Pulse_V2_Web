<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ADConfiguration.aspx.vb" Inherits="GMSUI.ADConfiguration" Title="Active Directory Configuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <script type="text/javascript" src="Javascript/js_MangeRoleMapping.js?d=10"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js"></script>
    <script language="javascript" type="text/javascript">

        var re = new RegExp("^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[0-9])|(?=.*[A-Z])(?=.*[a-z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])).{8,32}$");

        function ConfigADDirectory() {
            UpdateADDirectory();
        }

        function Cancel() {

            if (g_ADServerIp != $('#txtServerIp').val()) 
               $('#txtServerIp').val('');

            if (g_ADUserName != $('#txtUserName').val())
               $('#txtUserName').val('');

            if (g_ADPwd != $('#txtPwd').val())
               $('#txtPwd').val('');

            document.getElementById("tdMsg").innerHTML = "";
        }

        function redirectToAdminSettings() {
            location.href = 'AdminSettings.aspx';
        }

        this.onload = function () {
            Load_ADDirectoryInfo();
        }
            
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="85%">
        <tr style="height: 20px;">
        </tr>
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                        <a onclick="redirectToAdminSettings();">
                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
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
                                                                Active Directory Configuration
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
        <tr id="trRole" style="display: ;">
            <td>
                <table>
                    <tr>
                        <td valign="top" style="padding-left: 20px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%" class="clsFilterTable"
                                id="tblRoles">
                                <tr>
                                    <td class="clsVALabel" style="width: 130px; padding-left: 20px;" align="left">
                                        AD Server IP :
                                    </td>
                                    <td style="width: 180px;">
                                        <input type="text" id="txtServerIp" style="width: 250px; text-align: left; font-size: 15px;" />
                                    </td>
                                </tr>
                                <tr style="height: 5px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 130px; padding-left: 20px;" align="left">
                                        AD User Name :
                                    </td>
                                    <td style="width: 180px;">
                                        <input type="text" id="txtUserName" style="width: 250px; text-align: left; font-size: 15px;" />
                                    </td>
                                </tr>
                                <tr style="height: 5px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 130px; padding-left: 20px;" align="left">
                                        AD Password :
                                    </td>
                                    <td style="width: 180px;">
                                        <input id="txtPwd" title="Password" type="password" style="width: 250px; text-align: left;
                                            font-size: 15px;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 15px;">
                    </tr>
                    <tr>
                        <td valign="top" style="padding-left: 20px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td align="left" style="width: 100px;">
                                        <input type="button" id="btnSave" value="Save" style="width: 100px; height: 30px;"
                                            class="clsExportExcel" onclick="ConfigADDirectory();" />
                                    </td>
                                    <td align="left">
                                        <input type="button" id="btnCancel" value="Clear" style="width: 100px; height: 30px;"
                                            class="clsExportExcel" onclick="Cancel();" />
                                    </td>
                                    <td id="tdMsg" style="color: Green;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="position: fixed; top: 400px; left: 800px; display: none;" id="divLoading">
                                            <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
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
</asp:Content>
