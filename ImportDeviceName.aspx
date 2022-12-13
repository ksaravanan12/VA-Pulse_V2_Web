<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ImportDeviceName.aspx.vb" Inherits="GMSUI.ImportDeviceName" Title="Import Device Name" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript">

        this.onload = function () {
            document.getElementById('trImportLocation').style.display = "";
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
        }

        function UpdateLocation() {

            var siteid = $("#ctl00_ContentPlaceHolder1_drpSites").val();
            var deviceType = $("#ctl00_ContentPlaceHolder1_ddlDeviceType").val();

            if (siteid == "0") {
                alert("Select Site!.");
                return;
            }
            else if (deviceType == "0") {
                alert("Select Device Type!.");
                return;
            }

            var stitle = "";

            if (deviceType == "1")
                stitle = "Import Tag Name";
            else if (deviceType == "2")
                stitle = "Import Monitor Name";
            else if (deviceType == "3")
                stitle = "Import Star Name";

            $("#ifrmUploadDeviceLocation").attr("src", "uploadFile.aspx?Cmd=ImportDeviceLocations&SiteId=" + siteid + "&DeviceType=" + deviceType);

            //Open Dialog
            $("#divUploadDeviceLocation").dialog({
                height: 425,
                width: 600,
                modal: true,
                title: stitle,
                show: {
                    effect: "fade",
                    duration: 500
                },
                hide: {
                    effect: "fade",
                    duration: 500
                }
            });
        }

        function DownloadDeviceLocations() {

            var siteid = $("#ctl00_ContentPlaceHolder1_drpSites").val();
            var deviceType = $("#ctl00_ContentPlaceHolder1_ddlDeviceType").val();
           
            if (deviceType == "0") {
                alert("Select Device Type!.");
                return false;
            }

            $.post("AjaxConnector.aspx?cmd=GetDeviceLocationsForSite",
            {
                sid: siteid,
                devicetype: deviceType,
            },
            function (data, status) {
                if (status == "success") {

                    AjaxMsgReceiver(data.documentElement);
                    var dsRoot = data.documentElement;

                    var o_FilePath = dsRoot.getElementsByTagName('CSVPath');
                    var filePath = setundefined($($(o_FilePath[0])).text());

                    if (filePath != "")
                       tableToCSV1(filePath, "");
                }
                else {
                    
                }
            });
        }        

        function tableToCSV1(e, t) {

            var n = document.createElement('a');
            n.setAttribute("href", e);

            var sFile = e.split("/");
            var sFileName = "";

            if (sFile.length > 0) {
                sFileName = sFile[sFile.length - 1];
            }

            n.setAttribute("download", sFileName);
            document.body.appendChild(n);
            n.click()
        }

    </script>

    <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;">
        <tr style="height: 10px;">
            <td>
                <input type="hidden" id="hid_userrole" runat="server" />
                <input type="hidden" id="hid_userid" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="3">
                            <table width="100%">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' onclick='Redirect();'>
                                        <a href="AdminSettings.aspx">
                                            <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                    </td>
                                    <td style='width: 15px;' valign="top">
                                    </td>
                                    <td align='left' valign="top" style="width: 581px;">
                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                            <tr>
                                                <td align='left' class='SHeader1'>
                                                    Settings
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='left' class='subHeader_black'>
                                                    Import Device Name
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                        <td class="bordertop" valign="top" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trImportLocation" style="display: none;">
                        <td valign="top" style="padding-left: 30px;">
                            <table width="100%" cellpadding="5" cellspacing="1" class="clsFilterCriteria" border="0">
                                <tr>
                                    <td align="left" style="width: 100px;">
                                        Site:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList CssClass="wrapper-dropdown" ID="drpSites" runat="server" Width="550px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Device Type:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList CssClass="wrapper-dropdown" ID="ddlDeviceType" runat="server" Width="160px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <input type="button" id="btnImportLocation" value="Import" style="width: 100px;"
                                            class="clsButton" onclick="UpdateLocation();" />
                                        <img id="imgdownload" src="Images/Download.png" style="vertical-align: middle;
                                            padding-left: 10px; cursor: pointer;" title="Download Device Name" alt="Download Device Name"
                                            onclick="DownloadDeviceLocations();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="divUploadDeviceLocation" title="Import Device Name" style="display: none;">
        <iframe id="ifrmUploadDeviceLocation" style="border: none; height: 280px; width: 460px;">
        </iframe>
    </div>
</asp:Content>
