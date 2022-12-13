<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ManageRoleMapping.aspx.vb" Inherits="GMSUI.ManageRoleMapping" Title="Manage Role Mapping" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />

    <script type="text/javascript" src="Javascript/js_MangeRoleMapping.js?d=15"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js"></script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $('#chkEnableActiveDirectoryRoles').click(function () {
                if ($(this).is(':checked')) {
                    $("#tblRoles").find("input,button,textarea,select").attr("disabled", false);
                    $("#txtSSOIAttribute").attr("disabled", false);
                }
                else {
                    $("#tblRoles").find("input,button,textarea,select").attr("disabled", true);
                    $("#txtSSOIAttribute").attr("disabled", true);
                }
            });
        });

        this.onload = function () {
            Load_RoleMappingInfo();
            document.getElementById("trRole").style.display = "";
        }

        function redirectToAdminSettings() {
            location.href = 'AdminSettings.aspx';
        }

        function Cancel() {
            $('#chkEnableActiveDirectoryRoles').prop('checked', false);

            if (g_SSOIAttribute != $('#txtSSOIAttribute').val())
                $('#txtSSOIAttribute').val('');

            if (g_CustomerRole != $('#txtCustomerRole').val())
                $('#txtCustomerRole').val('');

            if (g_MaintenanceRole != $('#txtMaintenanceRole').val())
                $('#txtMaintenanceRole').val('');

            if (g_PartnerRole != $('#txtPartnerRole').val())
                $('#txtPartnerRole').val('');

            if (g_EnginneringRole != $('#txtEnginneringRole').val())
                $('#txtEnginneringRole').val('');

            if (g_AdminRole != $('#txtAdminRole').val())
                $('#txtAdminRole').val('');

            if (g_TechnicalAdminRole != $('#txtTechnicalAdminRole').val())
                $('#txtTechnicalAdminRole').val('');

            if (g_MaintenancePrism != $('#txtTBDRole').val())
                $('#txtTBDRole').val('');

            document.getElementById("tdMsg").innerHTML = "";
            LoadRoleMappingList();
        }        
                               
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr style="height: 20px;">
        </tr>
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
                                                    Manage Roles Mapping
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
        <tr id="trRole" style="display: none;">
            <td>
                <table>
                    <tr>
                        <td valign="top" style="padding-left: 40px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td align="left">
                                        <input type="checkbox" id="chkEnableActiveDirectoryRoles" style="vertical-align: middle;" /><label
                                            style="vertical-align: middle;" class="clsLALabel">
                                            Enable Active Directory Roles</label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-left: 40px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="clsLALabel" style="width: 160px;" align="left">
                                        SSOI Attribute for AD Role
                                    </td>
                                    <td>
                                        <input type="text" id="txtSSOIAttribute" style="width: 240px; font-size: 15px;" /><label
                                            class="clsMapErrorTxt" style="padding-left: 10px; vertical-align: middle; font-weight: bold;
                                            font-size: 20px;">*</label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                    </tr>
                    <tr>
                        <td valign="top" style="padding-left: 20px;">
                            <table border="0" cellpadding="5" cellspacing="5" width="100%" class="clsFilterTable"
                                id="tblRoles">
                                <tr>
                                    <td style="width: 60px; vertical-align: middle; padding-left: 20px;" class="clsLabel">
                                        GMS Role
                                    </td>
                                    <td colspan="3" class="clsLabel">
                                        Active Directory Role ( seperate mutiple roles by commas )
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Customer</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtCustomerRole" name="txtCustomerRole" style="width: 530px; height: 30px;
                                            text-align: left; font-size: 15px;"></textarea>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Maintenance</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtMaintenanceRole" name="txtMaintenanceRole" style="width: 530px;
                                            height: 30px; text-align: left; font-size: 15px;"></textarea>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Partner</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtPartnerRole" name="txtPartnerRole" style="width: 530px; text-align: left;
                                            height: 30px; font-size: 15px;"></textarea>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Engineering</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtEnginneringRole" name="txtEnginneringRole" style="width: 530px;
                                            height: 30px; text-align: left; font-size: 15px;"></textarea>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Admin</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtAdminRole" name="txtAdminRole" style="width: 530px; text-align: left;
                                            height: 30px; font-size: 15px;"></textarea>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Technical Admin</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtTechnicalAdminRole" name="txtTechnicalAdminRole" style="width: 530px;
                                            height: 30px; text-align: left; font-size: 15px;"></textarea>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                </tr>
                                <tr>
                                    <td class="clsVALabel" style="width: 60px; padding-left: 20px;" align="left" valign="top">
                                        <b>Maintenance/Prism</b>
                                    </td>
                                    <td style="width: 180px;">
                                        <textarea id="txtTBDRole" name="txtTBDRole" style="width: 530px; height: 30px; text-align: left;
                                            font-size: 15px;"></textarea>
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
                                            class="clsExportExcel" onclick="UpdateRoleMapping();" />
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
