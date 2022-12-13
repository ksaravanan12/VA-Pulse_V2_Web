<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CompanyList.aspx.vb" Inherits="GMSUI.CompanyList" Title="Company List" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupCompany.js"></script>

    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link rel="stylesheet" type="text/css" href="scripts/jquery-ui-1.7.2.custom.css" />
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />

    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="Javascript/jquery-ui.min.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js"></script>

    <script language="javascript" type="text/javascript">

        var g_isUserVerified = false;

        function EditValidate() {

            var strpwd = document.getElementById("<%=txtpwd.ClientID%>");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

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

        function Validate() {

            var strCompanyName = document.getElementById("<%=txtCompanyName.ClientID%>");
            var strpwd = document.getElementById("<%=txtpwd.ClientID%>");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

            if (Trim(strCompanyName.value) == "") {
                alert("Company name should not be empty!");
                strSiteName.value = "";
                strSiteName.focus();
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

    <script language="javascript" type="text/javascript">

        var g_UserRole = "";
        var g_UserId = "";
        var curSiteid = "";

        var isButtonClicked = 0;
        var isChanged = 0;

        this.onload = function () {

            document.getElementById('tdLeftMenu').style.display = "none";
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;

            LoadCompanyData();
        }

        function Redirect(sid) {

            location.href = "Setting.aspx";
        }

        function LoadCompanyData() {

            document.getElementById("divLoading").style.display = "";
            Load_Setup_Company();
        }

        function ShowAddCompany() {

            clearCtrlData();
            document.getElementById("<%=btnSave.ClientID%>").style.display = '';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = 'none';
            document.getElementById("<%=trStatus.ClientID%>").style.display = 'none';
            $('#tdAddCompany').show('slide', { direction: 'left' }, 500);
        }

        function HideAddSite() {
            clearCtrlData();
            $('#tdAddCompany').hide('slide', { direction: 'left' }, 500);
        }

        function LoadSiteData() {

            document.getElementById("divLoading").style.display = "";
            Load_Setup_Site();
        }

        function CallCompanySetup(IsAdd) {

            var companyid, companyname, status, authpassword;

            if (IsAdd == "1") {

                if (!Validate()) return false;

                companyid = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteDataId").value;
                companyname = document.getElementById("<%=txtCompanyName.ClientID%>").value;
                status = document.getElementById("<%=selStatus.ClientID%>").value;
                authpassword = document.getElementById("<%=txtpwd.ClientID%>").value;

                try {
                    PageVisitDetails(g_UserId, "Company List", enumPageAction.Add, "New Company Added [" + companyname + "]");
                }
                catch (e) {

                }
            }
            else {

                if (!EditValidate()) return false;

                SiteDataId = curSiteid;
                companyid = document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteDataId").value;
                companyname = document.getElementById("<%=txtCompanyName.ClientID%>").value;
                status = document.getElementById("<%=selStatus.ClientID%>").value;
                authpassword = document.getElementById("<%=txtpwd.ClientID%>").value;

                try {
                    PageVisitDetails(g_UserId, "Company List", enumPageAction.Edit, "Company Detail Edited [" + companyname + "]");
                }
                catch (e) {

                }
            }

            SiteDataId = document.getElementById("<%=hdnSiteDataId.ClientID%>").value;

            if (SiteDataId == "")
                IsAdd = "1";
            else
                IsAdd = "2";

            if (status == true)
                nStatus = 1;
            else
                nStatus = 0;
            
            AddCompanySetup(IsAdd, SiteDataId, companyname, status, authpassword);
        }

        function doAfterAddedCompany() {
                     
            window.location = "CompanyList.aspx";           
        }

        function clearCtrlData() {

            document.getElementById("<%=hdnSiteDataId.ClientID%>").value = "";
            document.getElementById("<%=txtCompanyName.ClientID%>").value = "";
            document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 0;
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

        function deleteConfiguration(siteId) {

            if (confirm("Are you sure do you want to delete this company?") == true) {
                var AuthUserId = document.getElementById("<%=hid_userid.ClientID%>").value;
                DeleteConfigurationSetting(siteId, AuthUserId);
            }
        }

        function showCompanyConfigurationForEdit(CompId, CompanyName, Status) {

            clearCtrlData();
            curSiteid = CompId;                      

            document.getElementById("<%=btnSave.ClientID%>").style.display = 'none';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = '';
            document.getElementById("<%=trStatus.ClientID%>").style.display = '';

            $('#tdAddCompany').show('slide', { direction: 'left' }, 500);

            var element_Status = document.getElementById("<%=selStatus.ClientID%>");

            if (Status == "True") {
                document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 0;
            }
            else {
                document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 1;
            }

            document.getElementById("<%=txtCompanyName.ClientID%>").value = CompanyName.replace(new RegExp("_", "gm"), " ");

            document.getElementById("ctl00_ContentPlaceHolder1_hdnSiteDataId").value = CompId;
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        } 
            
        
    </script>
    <div id="tooltip3" class="tool3" style="display: none;">
    </div>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="tblHeader"
                    runat="server">
                    <tr style="height: 10px;">
                        <td>
                            <input type="hidden" id="hid_userrole" runat="server" />
                            <input type="hidden" id="hid_pwd" runat="server" />
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
                                                                Manage Companies
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
                                                        <input class="clsLAText" type="text" id="txtCompanyName" name="txtCompanyName" runat="server" />&nbsp;<span
                                                            class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr id="trStatus" style="display: none" runat="server">
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Status :
                                                    </td>
                                                    <td align="left">
                                                        <select id="selStatus" class="clsLADrop" name="selStatus" runat="server">
                                                            <option value="1" selected="selected">Active</option>
                                                            <option value="0">Inactive</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Password :
                                                    </td>
                                                    <td align="left">
                                                        <input class="clsLAText" type="password" id="txtpwd" name="txtpwd" runat="server" />&nbsp;(For
                                                        User Verification)&nbsp;<span class="clsErrorTxt">*</span>
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
                                                    <td align="center" colspan="4" id="tdError" class="clsMapErrorTxt">
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
        <tr style="height: 10px;">
            <td>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table id="tblCompanyListInfo" cellspacing="1" cellpadding="1" style="width: 100%;
                    padding-top: 10px;" class="display">
                </table>
            </td>
        </tr>
        <tr style="height: 20px;">
        </tr>
    </table>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:content>
