<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeFile="PulseInactiveStatus.aspx.vb" Inherits="GMSUI.PulseInactiveStatus" Title="Pulse Inactive Status Duration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript" src="Javascript/js_Home.js?d=15"></script>
    <script type="text/javascript">

        this.onload = function () {
            UpdatethesholdTime("-1");
        };

        function redirectToHome() {
            location.href = 'AdminSettings.aspx';
        }

        //Events
        $(document).ready(function () {
            $('#btnSave').on('click', function () {
                if ($("#txt_time").val() == '') {
                    alert("Please provide a threshold");
                    $("#txt_time").focus();
                    return false;
                }
                var TTime = $("#txt_time").val();
                UpdatethesholdTime(TTime);
            });
        });

        $(document).on('click', '#btnCancel', function () {
            $("#txt_time").val("");
        });

    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
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
                                                                                Pulse Inactive Status Duration
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
                                <table cellpadding="5" cellspacing="5" border="0" style="border: 0px solid #DADADA;
                                    font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 13px;">
                                    <tr>
                                        <td>
                                            <table cellpadding="3" cellspacing="3" border="0" width="600px">
                                                <tr>
                                                    <td class="wrapper-text" align="left" style="width: 180px;">
                                                        Pulse Inactive Status Duration:
                                                    </td>
                                                    <td align="left" style="width: 150px;">
                                                        <input id="txt_time" style="width: 60px;" type="text" class="wrapper-textbox" onkeypress="return onlyNumeric(event,this);" />
                                                        &nbsp;<label style="color: Red;">(in Hours)</label>
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnSave" value="Save" style="width: 80px; height: 30px;"
                                                            class="clsExportExcel" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <label id="lblError" class="clsMapErrorTxt" runat="server">
                                                        </label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div style="position: fixed; top: 300px; left: 1000px; display: none;" id="divLoading_Status">
                                                <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
