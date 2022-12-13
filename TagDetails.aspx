<%@ Page Title="Tag Details" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TagDetails.aspx.vb" Inherits="GMSUI.TagDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        var siteid = "";
        var devicetype = "";
        var tagid = "";
        var typeid="0";
        
        this.onload = function() {
            siteid = getParameterByName("siteid");
            devicetype = getParameterByName("devicetype");
            tagid = getParameterByName("tagid");
            typeid = getParameterByName("TagTypeId");            
        }
        function GotoHome() {
            location.href = 'Home.aspx';
        }

        function GotoEMStatus(folderid) {
            if (folderid == 1)
                location.href = "TagInfo.aspx?detail=1&siteid=" + siteid + "&devicetype=" + devicetype + "&tagid=" + tagid + "&typeid=" + typeid; //Diagnostics
            else if (folderid == 2)
                location.href = "TagInfo.aspx?detail=2&siteid=" + siteid + "&devicetype=" + devicetype + "&tagid=" + tagid + "&typeid=" + typeid; //Configuration
            else if (folderid == 3)
                location.href = "TagInfo.aspx?detail=3&siteid=" + siteid + "&devicetype=" + devicetype + "&tagid=" + tagid + "&typeid=" + typeid; //Certification
            else if (folderid == 4)
                location.href = "TagInfo.aspx?detail=4&siteid=" + siteid + "&devicetype=" + devicetype + "&tagid=" + tagid + "&typeid=" + typeid; //Images
            else if (folderid == 5)
                location.href = "TagInfo.aspx?detail=5&siteid=" + siteid + "&devicetype=" + devicetype + "&tagid=" + tagid + "&typeid=" + typeid; //History
        }
        
    </script>

    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td style="padding-left: 25px;">
                <!-- Products PAGE-->
                <div id="divProductDetails" style="top: auto; left: auto; height: 850px;">
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
                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' style="padding-left: 10px;">
                                                        <a>
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" onclick="GotoHome();" /></a>
                                                    </td>
                                                    <td style='width: 10px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 700px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <label>
                                                                        Device
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>
                                                                    Status Summary
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
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="Product_TestTables">
                                <label>
                                    Status Summary</label>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; font-family: Calibri,Helvetica,MyriadPro,Verdana, Arial, sans-serif;
                                    font-size: 17px; color: #202121; font-weight: normal;">
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr>
                                        <td onclick="GotoEMStatus(1);" align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="Folderlogo">
                                                        Diagnostics
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td onclick="GotoEMStatus(2);" align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="Folderlogo">
                                                        Configuration
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td onclick="GotoEMStatus(3);" align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="Folderlogo">
                                                        Certification
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td onclick="GotoEMStatus(4);" align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="Folderlogo">
                                                        Images
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; font-family: Calibri,Helvetica,MyriadPro,Verdana, Arial, sans-serif;
                                    font-size: 17px; color: #202121; font-weight: normal;">
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr>
                                        <td onclick="GotoEMStatus(5);" align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="Folderlogo" style=" color:lightgray">
                                                        History
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="FolderEmptylogo">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="FolderEmptylogo">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50px;">
                                        </td>
                                        <td align="right" style="padding-right: 50px; cursor: pointer;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="center" class="FolderEmptylogo">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
