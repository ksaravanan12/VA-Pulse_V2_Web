<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SiteAssociationAD.aspx.vb" Inherits="GMSUI.SiteAssociationAD" Title="GMS Site Association With AD Group" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_Association.js?d=10"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>

    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link rel="stylesheet" type="text/css" href="scripts/jquery-ui-1.7.2.custom.css" />

    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="Javascript/jquery-ui.min.js"></script>

    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js"></script>

    <script language="javascript" type="text/javascript">

        var g_UserRole = "";
        var g_UserId = "";
        var curSiteid = "";

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";

            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;

            LoadAssociationSiteData();
        }

        function AvoidSubmit(e) {

            if (e.keyCode == 13) {
                event.preventDefault();
                return false;
            }
        }

        function HideAddAssociationSite() {

            clearCtrlData();
            $('#tdAddAssociationSite').hide('slide', { direction: 'left' }, 500);
        }

        function CallAssociationSiteSetup(IsAdd) {

            var VHAGroupId, SiteId, GroupName;
            var GroupId = "";

            if (IsAdd == "1") {
                if (!EditValidate()) return false;
                SiteId = document.getElementById("ctl00_ContentPlaceHolder1_selAssociationSite").value;
                VHAGroupId = document.getElementById("ctl00_ContentPlaceHolder1_selAssociationVHAGroup").value;
                GroupName = document.getElementById("txtAssociationGroup").value;
            }
            else {
                if (!EditValidate()) return false;
                GroupId = curSiteid;
                SiteId = document.getElementById("ctl00_ContentPlaceHolder1_selAssociationSite").value;
                VHAGroupId = document.getElementById("ctl00_ContentPlaceHolder1_selAssociationVHAGroup").value;
                GroupName = document.getElementById("txtAssociationGroup").value;
            }

            GroupId = document.getElementById("<%=hdnDataId.ClientID%>").value;

            if (GroupId == "")
                IsAdd = "1";
            else
                IsAdd = "2";

            AddAssociationSiteSetup(SiteId, GroupName, GroupId, VHAGroupId, IsAdd);
        }

        function clearCtrlData() {

            document.getElementById("ctl00_ContentPlaceHolder1_selAssociationSite").selectedIndex = 0;
            document.getElementById("txtAssociationGroup").value = "";
            document.getElementById("txtpwd").value = "";
            document.getElementById("ctl00_ContentPlaceHolder1_selAssociationVHAGroup").selectedIndex = 0;
        }

        function Redirect(sid) {

            location.href = "Setting.aspx";
        }

        function deleteConfiguration(DataId) {

            if (confirm("Are you sure do you want to delete this Association?") == true) {
                var GroupId = document.getElementById("txtAssociationGroup").value;
                DeleteAssociationSite(GroupId);
            }
        }

        function showAssociationSiteForEdit(GroupId, GroupName, SiteId, VHAGroupId) {

            clearCtrlData();
            curSiteid = GroupId;

            GroupId = setundefined(GroupId);
            SiteId = setundefined(SiteId);
            VHAGroupId = setundefined(VHAGroupId);

            clearCtrlData();

            $('#tdAddAssociationSite').show('slide', { direction: 'left' }, 500);
            document.getElementById("ctl00_ContentPlaceHolder1_hdnDataId").value = GroupId;
            document.getElementById("txtAssociationGroup").value = GroupName;
            document.getElementById("ctl00_ContentPlaceHolder1_selAssociationSite").value = SiteId;
            document.getElementById("ctl00_ContentPlaceHolder1_selAssociationVHAGroup").value = VHAGroupId;
            document.getElementById("ctl00_ContentPlaceHolder1_btnUpdate").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_btnSave").style.display = "none";
            document.getElementById("ctl00_ContentPlaceHolder1_selAssociationSite").focus();
        }

        function LoadAssociationSiteData() {

            document.getElementById("divLoading").style.display = "";
            Load_Setup_AssociationSite();
        }

        function doAfterAssociatedSite() {

            var msg = document.getElementById("<%=lblMessage.ClientID%>").value;
            if (typeof msg === 'undefined' || msg == "undefined") {
                window.location = "SiteAssociationAD.aspx";
            }
            else {
                window.location = "SiteAssociationAD.aspx?msg=" + msg;
            }
        }

        function ShowAddAssociationSite() {

            clearCtrlData();
            $('#tdAddAssociationSite').show('slide', { direction: 'left' }, 500);
            document.getElementById("ctl00_ContentPlaceHolder1_btnSave").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_btnUpdate").style.display = "none";
        }

        function EditValidate() {

            var strpwd = document.getElementById("txtpwd");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");
            var strAssociationGroup = document.getElementById("txtAssociationGroup");
            var strAssociationSite = document.getElementById("ctl00_ContentPlaceHolder1_selAssociationSite");
            var strAssociationVHAGroup = document.getElementById("ctl00_ContentPlaceHolder1_selAssociationVHAGroup");

            if (strAssociationSite.selectedIndex == 0) {
                alert("Select any Site!");
                strAssociationSite.focus();
                return false;
            }
            if (strAssociationVHAGroup.selectedIndex == 0) {
                alert("Select any VHA Group!");
                strAssociationVHAGroup.focus();
                return false;
            }
            if (Trim(strAssociationGroup.value) == "") {
                alert("AD Group name should not be empty!");
                strAssociationGroup.value = "";
                strAssociationGroup.focus();
                return false;
            }
            if (Trim(strpwd.value) == "") {
                alert("Password should not be empty!");
                strpwd.value = "";
                strpwd.focus();
                return false;
            }
            else if (Trim(strpwd.value) != Trim(conpwd.value)) {
                alert("Incorrect Password!");
                strpwd.value = "";
                strpwd.focus();
                return false;
            }
            return true;
        }

        function deleteGroup(VHAGroupId) {

            if (confirm("Are you sure do you want to delete this group?") == true) {

                document.getElementById('hid_deleteGroupId').value = VHAGroupId;
                document.getElementById('txtUserPassword').value = "";

                $("#Password_dialog").dialog({
                    height: 200,
                    width: 450,
                    modal: true,
                    show: {
                        effect: "fade",
                        duration: 500
                    },
                    hide: {
                        effect: "fade",
                        duration: 500
                    },
                    close: function (event) {
                    }
                });
            }
        }

        function conformDelete() {

            var GroupId = document.getElementById('hid_deleteGroupId').value;
            var password = document.getElementById('txtUserPassword').value;
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>").value;

            if (password == conpwd) {
                DeleteSiteAssociation(GroupId);
            }
            else if (password == "") {
                alert("Password should not be empty!");
            }
            else {
                alert("Incorrect Password");
                document.getElementById('txtUserPassword').value = "";
            }
        }

    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">      
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr style="height: 10px;">
                        <td>
                            <input type="hidden" id="hid_userrole" runat="server" />
                            <input type="hidden" id="hid_userid" runat="server" />
                            <input type="hidden" id="hid_pwd" runat="server" />
                            <input type="hidden" id="hid_deleteGroupId" />
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
                                                        <img src='images/Left-Arrow.png' title='Settings' border="0" /></a>
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
                                                                GMS Site Association With AD Group
                                                            </td>
                                                            <td colspan="2" align="left" style="color: Green;" class="clsFilterCriteria">
                                                                <asp:label id="lblMessage" runat="server" text=""></asp:label>
                                                            </td>
                                                            <td align="right">
                                                                <input id="btnAdd" runat="server" class="clsButton" onclick="ShowAddAssociationSite();"
                                                                    type="button" value="Add" />
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
                                <tr>
                                    <td>
                                        <div id="tdAddAssociationSite" class="clsFilterTable" style="display: none;">
                                            <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Site Name :
                                                    </td>
                                                    <td align="left">
                                                        <asp:dropdownlist class="clsLADrop" id="selAssociationSite" runat="server" height="21px"
                                                            width="203px">
                                                        </asp:dropdownlist>&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        VHA Group Name :
                                                    </td>
                                                    <td align="left">
                                                        <asp:dropdownlist class="clsLADrop" id="selAssociationVHAGroup" runat="server" height="21px"
                                                            width="203px">
                                                        </asp:dropdownlist>&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        AD Group Name :
                                                    </td>
                                                    <td align="left">
                                                        <input class="clsLAText" type="text" id="txtAssociationGroup" name="txtAssociationGroup"
                                                            style="width: 200px; background-color: #ffffff;" onkeypress="return AvoidSubmit(event)" />&nbsp;<span
                                                                class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Password :
                                                    </td>
                                                    <td align="left">
                                                        <input class="clsLAText" type="password" id="txtpwd" name="txtpwd" style="width: 200px;
                                                            background-color: #ffffff;" onkeypress="return AvoidSubmit(event)" />&nbsp;(For
                                                        User Verification)&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left" style="height: 24px">
                                                        <input type="button" value="Save" style="width: 130px; height: 30px;" class="clsExportExcel"
                                                            name="btnSave" id="btnSave" runat="server" onclick="CallAssociationSiteSetup(1);" />
                                                        <input type="button" value="Update" style="width: 130px; height: 30px;" class="clsExportExcel"
                                                            name="btnUpdate" id="btnUpdate" runat="server" onclick="CallAssociationSiteSetup(2)" />
                                                        <input type="button" id="btnCancel" value="Cancel" style="width: 130px; height: 30px;"
                                                            class="clsExportExcel" onclick="HideAddAssociationSite();" />
                                                        <input type="hidden" name="hdnDataId" id="hdnDataId" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" style="padding-left: 5px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left" id="tdError" runat="server">
                                                    </td>
                                                </tr>
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
        <tr style="height: 20px;">
        </tr>
        <tr>
            <td valign="top">
                <table id="tblSiteAssociationListInfo" cellspacing="1" cellpadding="1" style="width: 100%;
                    padding-top: 10px;" class="display">
                </table>
            </td>
        </tr>
    </table>
    <div id="Password_dialog" title="Confirm Password" style="display: none;">
        <table cellpadding="5">
            <tr>
                <td>
                    Enter Password
                </td>
                <td>
                    <input class="clsLAText" type="password" id="txtUserPassword" name="txtUserPassword"
                        style="width: 200px;" />
                </td>
            </tr>
            <tr style="height: 20px;">
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;
                    <input type="button" class="clsButton" id="btnConform" value="Ok" onclick="conformDelete();" />
                </td>
            </tr>
        </table>
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:content>