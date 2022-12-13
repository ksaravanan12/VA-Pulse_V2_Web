<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeFile="StreamingFields.aspx.vb" Title="Streaming Fields" Inherits="GMSUI.StreamingFields" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/DatatablesLoadGraph.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=11"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupStreamingFields.js?d=107"></script>
    <script language="javascript" type="text/javascript">

        var g_UserRole = "";
        var g_UserId = "";
        var curDataid = "";

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            LoadStreamingFields();

        }

        function Redirect(sid) {
            location.href = "Setting.aspx";
        }

        function showTip3(text, lf, tp) {

            var elementRef = document.getElementById('tooltip3');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip3").css({ left: tempX, top: tempY })
        }

        function hideTip3() {
            var elementRef = document.getElementById('tooltip3');
            elementRef.style.display = 'none';
        }

        function LoadStreamingFields() {
            var SiteId = "";
            SiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
            if (SiteId != "0") {
                document.getElementById("divLoading").style.display = "";
                Load_Setup_StreamingFields();
            }
        }
        
    </script>
    <div id="tooltip3" class="tool3" style="display: none;">
    </div>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <input id="hid_userrole" type="hidden" runat="server" />
        <input id="hid_userid" type="hidden" runat="server" />
        <tr>
            <td>
                <div>
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
                                                                    Connect Pulse Reports
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Streaming Fields
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
                            <td>
                                <table border="0" cellpadding="5" cellspacing="5" width="1200px" class="clsFilterRow"
                                    id="tblFilterRow" align="center">
                                    <tbody>
                                        <tr>
                                            <td class="clsLALabel" style="width: 80px;">
                                                Select Site :
                                            </td>
                                            <td>
                                                <asp:dropdownlist id="ddlSites" runat="server" style="width: 600px;" onchange="LoadStreamingFields()">
                                                </asp:dropdownlist>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                            <td class="bordertop" valign="top" colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table id="tblStreamingFieldsListInfo" cellspacing="1" cellpadding="3" style="width: 100%;
                                    padding-top: 10px;" class="display">
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
                    <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                </div>
            </td>
        </tr>
    </table>
</asp:content>
