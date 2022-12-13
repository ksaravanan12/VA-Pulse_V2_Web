<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CompanyAssociation.aspx.vb" Inherits="GMSUI.CompanyAssociation" Title="AD Group Association" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupCompanyAssociation.js?d=3"></script>
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

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            LoadCompanyAssociation();
        }

        function AvoidSubmit(e) {
            if (e.keyCode == 13) {
                event.preventDefault();
                return false;
            }
        }

        function LoadCompanyAssociation() {
            document.getElementById("divLoading").style.display = "";
            Load_Company_Association();
        }

        function redirectToAdminSettings() {
            location.href = 'AdminSettings.aspx';
        }

        function Validate() {

            var strCompany = document.getElementById("<%=selCompany.ClientID%>");
            var strGroupName = document.getElementById('txtGroupName');
            var strpwd = document.getElementById('txtpwd');
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

            if (Trim(strCompany.value) == 0) {
                alert("Select company name!");
                strCompany.value = 0;
                strCompany.focus();
                return false;
            }
            else if (Trim(strGroupName.value) == "") {
                alert("Group name should not be empty!");
                strGroupName.value = "";
                strGroupName.focus();
                return false;
            }
            else if (Trim(strpwd.value) == "") {
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

        function ShowAddCompany() {
            clearCtrlData();
            document.getElementById("<%=btnSave.ClientID%>").style.display = '';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = 'none';
            document.getElementById("<%=btnSave.ClientID%>").disabled = false;
            $('#tdAddCompany').show();
        }

        function HideAddSite() {
            clearCtrlData();
            $('#tdAddCompany').hide();
        }

        function clearCtrlData() {
            var element_CompanyId = document.getElementById("<%=selCompany.ClientID%>");
            element_CompanyId.value = 0;
            document.getElementById('txtGroupName').value = "";
            document.getElementById('txtpwd').value = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").innerHTML = "";
        }

        function showCompanyAssociationForEdit(VHAGroupId, CompanyId, VHAGroupName) {

            VHAGroupName = decodeURIComponent(setundefined(VHAGroupName));
            document.getElementById('hid_GroupId').value = VHAGroupId;
            var element_CompanyId = document.getElementById("<%=selCompany.ClientID%>");
            element_CompanyId.value = CompanyId;
            document.getElementById('txtGroupName').value = VHAGroupName;

            document.getElementById("<%=btnSave.ClientID%>").style.display = 'none';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = '';

            $('#tdAddCompany').show();
            document.getElementById("ctl00_ContentPlaceHolder1_selCompany").focus();
        }

        function CallCompanySetup(IsAdd) {

            if (!Validate()) return false;

            var GroupId = document.getElementById('hid_GroupId').value;
            var GroupName = document.getElementById('txtGroupName').value;
            var element_CompanyId = document.getElementById("<%=selCompany.ClientID%>");
            var CompanyId = element_CompanyId.value;

            if (IsAdd == 1) {
                document.getElementById("<%=btnSave.ClientID%>").disabled = true;
            }

            AddCompanyGroup(GroupId, GroupName, CompanyId, IsAdd);
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

            var VHAGroupId = document.getElementById('hid_deleteGroupId').value;
            var password = document.getElementById('txtUserPassword').value;
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>").value;

            if (password == conpwd) {
                DeleteCompanyGroup(VHAGroupId);
            }
            else if (password == "") {
                alert("Password should not be empty!");
            }
            else {
                alert("Incorrect Password");
                document.getElementById('txtUserPassword').value = "";
            }
        }

        function doAfterAddedGroup() {
            var msg = document.getElementById("<%=lblMessage.ClientID%>").value;

            if (setundefined(msg) == "") {
                window.location = "CompanyAssociation.aspx";
            }
            else {
                window.location = "CompanyAssociation.aspx?msg=" + msg;
            }
        }

        //function to trim the preceedind and trailing spaces in that given string
        function Trim(str) {
            while (str.substring(0, 1) == ' ') {
                str = str.substring(1, str.length);
            }
            while (str.substring(str.length - 1, str.length) == ' ') {
                str = str.substring(0, str.length - 1);
            }
            return str;
        }
            
    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">       
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr style="height: 10px;">
                        <td>
                            <input type="hidden" id="hid_userrole" runat="server" />
                            <input type="hidden" id="hid_pwd" runat="server" />
                            <input type="hidden" id="hid_userid" runat="server" />
                            <input type="hidden" id="hid_GroupId" />
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
                                                <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                    <a onclick="redirectToAdminSettings();">
                                                        <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                            border="0" /></a>
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
                                                                AD Group Association
                                                            </td>
                                                            <td colspan="2" align="left" style="color: Green;" class="clsFilterCriteria">
                                                                <asp:label id="lblMessage" runat="server" text=""></asp:label>
                                                            </td>
                                                            <td align="right">
                                                                <input id="btnAdd" runat="server" class="clsButton" onclick="ShowAddCompany();" type="button"
                                                                    value="Add" />
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
                                        <div id="tdAddCompany" class="clsFilterTable" style="display: none;">
                                            <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Company Name :
                                                    </td>
                                                    <td align="left">
                                                        <asp:dropdownlist class="clsLADrop" id="selCompany" name="selCompany" runat="server"
                                                            height="21px" width="203px">
                                                        </asp:dropdownlist>&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Group Name :
                                                    </td>
                                                    <td align="left">
                                                        <input class="clsLAText" type="text" id="txtGroupName" name="txtGroupName" style="width: 200px;"
                                                            onkeypress="return AvoidSubmit(event)" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Password :
                                                    </td>
                                                    <td align="left">
                                                        <input class="clsLAText" type="password" id="txtpwd" name="txtpwd" style="width: 200px;"
                                                            onkeypress="return AvoidSubmit(event)" />&nbsp;(For User Verification)&nbsp;<span
                                                                class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left" style="height: 24px">
                                                        <input type="button" value="Save" style="width: 130px; height: 30px;" class="clsExportExcel"
                                                            name="btnSave" id="btnSave" runat="server" onclick="CallCompanySetup(1);" />
                                                        <input type="button" value="Update" style="width: 130px; height: 30px;" class="clsExportExcel"
                                                            name="btnUpdate" id="btnUpdate" runat="server" onclick="CallCompanySetup(2)" />
                                                        <input type="button" id="btnCancel" value="Cancel" style="width: 130px; height: 30px;"
                                                            class="clsExportExcel" onclick="HideAddSite();" />
                                                        <input type="hidden" name="hdnSiteDataId" id="hdnSiteDataId" runat="server" />
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
                <table id="tblCompanyGroupInfo" cellspacing="1" cellpadding="1" style="width: 100%;
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
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:content>
