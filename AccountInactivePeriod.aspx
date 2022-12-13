<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AccountInactivePeriod.aspx.vb" Inherits="GMSUI.AccountInactivePeriod"
    Title="Account Inactive Period" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="Javascript/js_General.js"></script>

    <script type="text/javascript">
        
        var siteIdx;
        var siteVal;

        //Onload Function
        this.onload = function() {
            document.getElementById("trConfiguration").style.display = "";
        };

        function Validate() {
            var Period;            
            Period = document.getElementById("<%=txtPeriod.ClientID%>");

            if (Period.value == "") {
                alert("Please provide a Period");
                Period.focus();
                return false;
            }

            return true;
        }

        function GotoSettings() {
            location.href = 'AdminSettings.aspx';
        }

        function UserAttemptValidate() {
            var Loginattempt;            
            Loginattempt = document.getElementById("<%=txtLoginattempt.ClientID%>");

            if (Loginattempt.value == "") {
                alert("Please provide a count");
                Loginattempt.focus();
                return false;
            }

            return true;
        }

        function onlyNumeric(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            catch (err) {
                alert(err.Description);
            }
        }
            
    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td style="padding-left: 25px;">
                <!-- INI EDITOR PAGE-->
                <div id="divProductDetails" runat="server" style="top: auto; left: auto; height: 850px;">
                    <table cellspacing="0" cellpadding="2" border="0" style="width: 90%;">
                        <tr>
                            <td>
                                <table cellpadding="0" style="width: 100%">
                                    <tr style="height: 20px;">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a>
                                                            <img src='images/Left-Arrow.png' title='Back' style="width: 16px; height: 24px;"
                                                                border="0" onclick="GotoSettings();" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <label>
                                                                        Settings
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>
                                                                    Configuration
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <!-- TOOL TIP-->
                                    <div id="tooltip4" class="tool3" style="display: none;">
                                    </div>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr id="trConfiguration" style="display: none;">
                                        <td valign="top">
                                            <table cellpadding="5" cellspacing="5" border="0" style="border: 0px solid #DADADA;
                                                font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 13px;">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="3" cellspacing="3" border="0" width="600px">
                                                            <tr>
                                                                <td class="wrapper-text" align="left" style="width: 160px;">
                                                                    Account inactive Period :
                                                                </td>
                                                                <td align="left" style="width: 150px;">
                                                                    <input id="txtPeriod" style="width: 60px;" type="text" runat="server" class="wrapper-textbox" onkeypress="return onlyNumeric(event,this);" />
                                                                    &nbsp;<label style="color: Red;">(in Days)</label>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btSave" runat="server" Text="Save" Style="width: 80px;" CssClass="clsExportExcel"
                                                                        OnClientClick="if(!Validate())return false;" />
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="wrapper-text" align="left" style="width: 160px;">
                                                                    User Login attempt count :
                                                                </td>
                                                                <td align="left" style="width: 150px;">
                                                                    <input id="txtLoginattempt" style="width: 60px;" type="text" runat="server" class="wrapper-textbox" onkeypress="return onlyNumeric(event,this);" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btLoginattemptSave" runat="server" Text="Save" Style="width: 80px;" CssClass="clsExportExcel"
                                                                        OnClientClick="if(!UserAttemptValidate())return false;" />
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td colspan="3" align="center">
                                                                    <label id="lblError" class="clsMapErrorTxt" runat="server">
                                                                    </label>
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
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
