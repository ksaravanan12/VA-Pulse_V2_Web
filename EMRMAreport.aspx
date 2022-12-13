<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="EMRMAreport.aspx.vb" Inherits="GMSUI.EMRMAreport" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>
    <script type="text/javascript">

        $('#datetimepicker').datetimepicker()
	       .datetimepicker({ mask: '9999/19/39 29:59', step: 1 });

        $(function () {
            $('#ctl00_ContentPlaceHolder1_txtFromDate').datetimepicker({
                format: 'Y/m/d',
                step: 60,
                timepicker: false
            });

            $('#ctl00_ContentPlaceHolder1_txtToDate').datetimepicker({
                format: 'Y/m/d',
                step: 60,
                timepicker: false
            });
        });

        function Validate() {

            var siteid = document.getElementById("ctl00_ContentPlaceHolder1_ddlsitelist").value;
            var fromdate = document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate").value;
            var ToDate = document.getElementById("ctl00_ContentPlaceHolder1_txtToDate").value;

            if (fromdate == "") {
                alert("From date should not be empty");
                document.getElementById("ctl00_ContentPlaceHolder1_txtFromDate").focus();
                return false;
            }
            else if (ToDate == "") {
                alert("To date should not be empty");
                document.getElementById("ctl00_ContentPlaceHolder1_txtToDate").focus();
                return false;
            }

            return true;
        }

        function GoBack() {
            window.history.back();
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
                                        <a onclick="GoBack()">
                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                border="0" /></a>
                                    </td>
                                    <td style='width: 15px;' valign="top">
                                    </td>
                                    <td align='left' valign="top" style="width: 581px;">
                                        <table border='0' cellpadding='0' cellspacing='0'>
                                            <tr>
                                                <td align='left' class='SHeader1'>
                                                    EM Display Sensors Requiring RMA
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                        <tr>
                                                            <td align='left' class='subHeader_black'>
                                                                EM Display Sensors Requiring RMA
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
            <td valign="top" style="padding-left: 30px;">
                <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            Site :
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="ddlsitelist" runat="server" cssclass="wrapper-dropdown" width="550px">
                            </asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            From Date :
                        </td>
                        <td align="left">
                            <input id="txtFromDate" type="text" name="txtFromDate" runat="server" class="wrapper-textbox"
                                style="width: 120px;" autocomplete="off" />
                        </td>
                    </tr>
                    <tr>
                        <td class="clsLALabel" align="left" style="width: 150px;">
                            To Date :
                        </td>
                        <td align="left">
                            <input id="txtToDate" type="text" name="txtToDate" runat="server" class="wrapper-textbox"
                                style="width: 120px;" autocomplete="off" />
                        </td>
                    </tr>
                    <tr style="height: 20px;">
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                            <asp:button id="btnGenearate" runat="server" text="Generate" class="clsExportExcel" OnClientClick="if(!Validate())return false;" />
                        </td>
                    </tr>
                </table>
                <div style="position: fixed; top: 350px; z-index: 1; display: none;" id="divExcelLoading">
                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                </div>
            </td>
        </tr>
    </table>
</asp:content>
