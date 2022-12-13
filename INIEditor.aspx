<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="INIEditor.aspx.vb" Inherits="GMSUI.INIEditor" Title="INI Editor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="Javascript/js_General.js"></script>

    <script type="text/javascript">
        var siteIdx; 
        var siteVal;
    
        //Onload Function
        this.onload = function () {
            document.getElementById("trINI").style.display = "";                
            siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
            document.getElementById("<%=hdnSiteId.ClientID%>").value = siteVal;                        
        };
        
        function CancelProfileSettings()
        {
            document.getElementById("ctl00_ContentPlaceHolder1_trProfileSettings").style.display="none"; 
        }
        
        function Validate()
        {
            var DeviceId;            
            DeviceId= document.getElementById('ctl00_ContentPlaceHolder1_txtTagId');
                 
            if(siteVal == 0)
            {
                alert("Please select a site");            
                return false;
            }
            else if(DeviceId.value == "")
            {
                alert("Please provide a device id");
                DeviceId.focus();
                return false;                
            }
             
            return true;
        }
         function GotoSettings() {
            location.href = 'AdminSettings.aspx';
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
                                                                    Configure Tag Profiles
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
                                    <tr id="trINI" style="display:none;">
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <fieldset style="border: 1px solid #DADADA;">
                                                            <legend class="SHeader1">Device Search</legend>
                                                            <table cellpadding="5" cellspacing="5" border="0" style="border: 0px solid #DADADA;
                                                                font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 13px;">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="3" cellspacing="3" border="0" width="600px">
                                                                            <tr>
                                                                                <td class="wrapper-text" align="left" style="width: 120px;">
                                                                                    Device Id
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left" style="width: 150px;">
                                                                                    <input id="txtTagId" style="width: 244px;" type="text" runat="server" class="wrapper-textbox" />
                                                                                </td>
                                                                                <td colspan="3" align="center">
                                                                                    <asp:Button ID="btnReceive" runat="server" Text="Search Tag ID" Style="width: 120px;" CssClass="clsExportExcel" OnClientClick="if(!Validate())return false;" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" align="center">
                                                                                    <label id="lblNoTagInINI" class="clsMapErrorTxt" runat="server">
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <!-- INI EDITOR Details -->
                                    <tr id="trProfileSettings" runat="server" style="display: none;">
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;" class="clsFilterTable">
                                                <tr>
                                                    <td>
                                                        <fieldset style="border: 1px solid #DADADA;">
                                                            <legend class="SHeader1">Tag Profile</legend>
                                                            <table cellpadding="5" cellspacing="5" border="0" style="border: 0px solid #DADADA;
                                                                font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; font-size: 13px;">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="5" cellspacing="5" border="0" width="600px">
                                                                            <tr>
                                                                                <td class="wrapper-text" align="left" style="width: 120px;">
                                                                                    Tag Category
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selTagCategory" style="width: 244px;" class="clsDropDown" runat="server">                                                                             
                                                                                        <option value="2">Normal Tags</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trIRProfile" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    IR Profile
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selIRProfile" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">1.5 Seconds</option>
                                                                                        <option value="1">3 Seconds</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trLongIR" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    Long IR
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selLongIR" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">Disable Long IR</option>
                                                                                        <option value="1">Enable Long IR</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trIRReportRate" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    IR Report Rate
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selIRReportRate" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">12 Seconds</option>
                                                                                        <option value="1">24 Seconds</option>
                                                                                        <option value="2">48 Seconds</option>
                                                                                        <option value="3">96 Seconds</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trRFReportRate" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    RF Report Rate
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selRFReportRate" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">12 Seconds</option>
                                                                                        <option value="1">3 Seconds/6 Seconds</option>
                                                                                        <option value="2">24 Seconds</option>
                                                                                        <option value="3">48 Seconds</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trIRRX" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    IR-RX Profile
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selIRRXProfile" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trFastPushButton" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <input type="checkbox" id="chkFastPushbtn" style='vertical-align: middle;' runat="server" /><label
                                                                                        class="wrapper-text">Fast Push Button
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trLFRange" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    LF Range
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selLFRange" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">Default Range</option>
                                                                                        <option value="1">User Defined Range</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trDisableLF" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    Disable LF
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selDisableLF" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">No</option>
                                                                                        <option value="1">Yes</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trOperatingMode"  runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    Operating Mode
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selOperatingMode" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">Wifi No IR</option>
                                                                                        <option value="1">Wifi IR</option>
                                                                                        <option value="2">900 MHZ Only</option>
                                                                                        <option value="3">Wifi Marker IR mode</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trWifiReportingTime" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    WiFi Reporting Time
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selWiFiReporting" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">Active - 9 sec, Sleep - 5 mins</option>
                                                                                        <option value="1">Active - 15 sec, Sleep - 5 mins</option>
                                                                                        <option value="2">Active - 30 sec, Sleep - 5 mins</option>
                                                                                        <option value="3">Active - 1 min, Sleep - 5 mins</option>
                                                                                        <option value="4">Active - 2 mins, Sleep - 5 mins</option>
                                                                                        <option value="5">Active - 5 mins, Sleep - 5 mins</option>
                                                                                        <option value="6">Active - 15 mins, Sleep - 15 mins</option>
                                                                                        <option value="7">Active - 1 hour, Sleep - 1 hour</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trWIFI" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <input type="checkbox" id="chkWifi900MHz" style='vertical-align: middle;' runat="server" /><label
                                                                                        class="wrapper-text">
                                                                                        WiFi In 900MHz
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trPagingPrfile" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    Paging Profile
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selPagingProfile" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">900MHz - 12.5 secs, WiFi - 12 secs</option>
                                                                                        <option value="1">900MHz - 9.5 secs, WiFi - 24 secs</option>
                                                                                        <option value="2">900MHz - 6.5 secs, WiFi - 5 mins</option>
                                                                                        <option value="3">900MHz - 24.5 secs, WiFi - 24 secs</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                             <tr style="display:none;" id="trUpdateRate" runat="server">
                                                                                <td class="wrapper-text" align="left">
                                                                                    Update Rate
                                                                                </td>
                                                                                <td>
                                                                                    :
                                                                                </td>
                                                                                <td align="left">
                                                                                    <select id="selUpdateRate" style="width: 244px;" class="clsDropDown" runat="server">
                                                                                        <option value="0">1 minute</option>
                                                                                        <option value="1">5 miutes</option>
                                                                                        <option value="2">10 minutes</option>
                                                                                        <option value="3">15 minutes</option>
                                                                                        <option value="4">30 minutes</option>
                                                                                        <option value="5">1 hour</option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 10px;">
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" align="center">
                                                                                    <asp:Button ID="btnSend" runat="server" Text="Save Profile" CssClass="clsExportExcel" />
                                                                                    <input type="button" id="btnCancel" value="Cancel" class="clsExportExcel" onclick="CancelProfileSettings(); " />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" align="center">
                                                                                    <label id="lblErrormsg" class="clsMapSuccessTxt" runat="server">
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" align="center">
                                                                                    <input type="hidden" id="hdnSiteId" runat="server" />
                                                                                    <input type="hidden" id="hdnIsDefaultProfile" runat="server" />
                                                                                    <input type="hidden" id="hdnTagType" runat="server" />
                                                                                    
                                                                                    <div style="display: none;" id="divLoading">
                                                                                        <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 50px;">
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
