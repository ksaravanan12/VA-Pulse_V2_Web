<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeFile="UserList.aspx.vb" Title="User List" Inherits="GMSUI.UserList" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=5"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_SetupUser.js?d=155"></script>

    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css?d=201" />
    <link rel="stylesheet" type="text/css" href="scripts/demo_table_jui.css" />
    <link rel="stylesheet" type="text/css" href="scripts/jquery-ui-1.7.2.custom.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />

    <script type="text/javascript" src="scripts/Pagingjquery.dataTables.min.js"></script>
    <script type="text/javascript" src="scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" language="javascript" src="scripts/PagingDatatablesLoadGraph.js?d=10"></script>
    <script type="text/javascript" language="javascript" src="scripts/DatatablesLoadGraph.js?d=10"></script>

    <script language="javascript" type="text/javascript">

        function EditValidate() {

            var strUserName = document.getElementById("<%=txtUserName.ClientID%>");
            var strFirstName = document.getElementById("<%=txtFirstName.ClientID%>");
            var strLastName = document.getElementById("<%=txtLastName.ClientID%>");
            var strUserType = document.getElementById("<%=selUser.ClientID%>");
            var strEmail = document.getElementById("<%=txtAssociatedEmail.ClientID%>");
            var strUserRole = document.getElementById("<%=selUserRole.ClientID%>");
            var strUserEmail = document.getElementById("<%=txtUserEmail.ClientID%>");
            var strPhone = document.getElementById("<%=txtPhnos.ClientID%>");
            var strpwd = document.getElementById("<%=txtpwd.ClientID%>");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

            if (Trim(strUserType.value) == 0) {
                alert("User type should not be empty!");
                strUserType.value = 0;
                strUserType.focus();
                return false;
            }
            else if (Trim(strUserName.value) == "") {
                alert("User name should not be empty!");
                strUserName.value = "";
                strUserName.focus();
                return false;
            }
            else if (Trim(strFirstName.value) == "") {
                alert("First name should not be empty!");
                strFirstName.value = "";
                strFirstName.focus();
                return false;
            }
            else if (Trim(strLastName.value) == "") {
                alert("Last name should not be empty!");
                strLastName.value = "";
                strLastName.focus();
                return false;
            }
            else if (Trim(strUserRole.value) == -1 || Trim(strUserRole.value) == "") {
                alert("Select User Role!");
                strUserRole.value = -1;
                strUserRole.focus();
                return false;
            }
            else if (Trim(strUserEmail.value) == "") {
                alert("Email address should not be empty!");
                strUserEmail.value = "";
                strUserEmail.focus();
                return false;
            }
            else if (EmailCheck(strUserEmail.value) == false) {
                alert('Invalid email address : ' + strUserEmail.value);
                strUserEmail.focus();
                return false;
            }
            else if (Trim(strEmail.value) != "") {
                var emailArr = new Array();
                emailArr = String(strEmail.value).split(",");
                for (var i = 0; i <= emailArr.length - 1; i++) {
                    var sEmail = emailArr[i];
                    if (sEmail != "") {
                        if (EmailCheck(sEmail) == false) {
                            alert('Invalid email address : ' + sEmail);
                            strEmail.focus();
                            return false;
                        }
                    }
                }
            }

            if (Trim(strPhone.value) != "") {
                var sPhoneNos = '';
                var PhonesArr = new Array();
                PhonesArr = String(strPhone.value).split(",");

                for (var i = 0; i <= PhonesArr.length - 1; i++) {
                    var nPhone = PhonesArr[i];
                    var PhoneVal = nPhone.replace(/\D+/g, '');

                    if (i == 0)
                        sPhoneNos = PhoneVal;
                    else
                        sPhoneNos = sPhoneNos + ',' + PhoneVal;
                }

                strPhone.value = sPhoneNos;

                for (var i = 0; i <= PhonesArr.length - 1; i++) {
                    var nPhone = PhonesArr[i];
                    var PhoneVal = nPhone.replace(/\D+/g, '');

                    if (PhoneVal != "") {
                        if (PhoneVal.length < 10) {
                            alert('Please Enter Valid Phone Number:' + PhoneVal);
                            strPhone.focus();
                            return false;
                        }
                        else if (PhoneVal.length == 10) {
                            alert('Please Enter Country Code:' + PhoneVal);
                            strPhone.focus();
                            return false;
                        }
                    }
                }
            }
            if (Trim(strpwd.value) == "") {
                alert("User Verification Password should not be empty!");
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

            var strCompany = document.getElementById("<%=selCompany.ClientID%>");
            var strUserName = document.getElementById("<%=txtUserName.ClientID%>");
            var strFirstName = document.getElementById("<%=txtFirstName.ClientID%>");
            var strLastName = document.getElementById("<%=txtLastName.ClientID%>");
            var strPassword = document.getElementById("<%=txtPassword.ClientID%>");
            var strPassword1 = document.getElementById("<%=txtPassword1.ClientID%>");
            var strUserType = document.getElementById("<%=selUser.ClientID%>");
            var strUserRole = document.getElementById("<%=selUserRole.ClientID%>");
            var strUserEmail = document.getElementById("<%=txtUserEmail.ClientID%>");
            var strEmail = document.getElementById("<%=txtAssociatedEmail.ClientID%>");
            var strPhone = document.getElementById("<%=txtPhnos.ClientID%>");
            var strpwd = document.getElementById("<%=txtpwd.ClientID%>");
            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>");

            var re = new RegExp("^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[0-9])|(?=.*[A-Z])(?=.*[a-z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])).{8,32}$");
            if (Trim(strCompany.value) == 0) {
                alert("Select company name!");
                strCompany.value = 0;
                strCompany.focus();
                return false;
            }
            else if (Trim(strUserType.value) == 0) {
                alert("Select User type!");
                strUserType.value = 0;
                strUserType.focus();
                return false;
            }
            else if (Trim(strUserRole.value) == -1) {
                alert("Select User Role!");
                strUserRole.value = -1;
                strUserRole.focus();
                return false;
            }
            else if (Trim(strUserName.value) == "") {
                alert("User name should not be empty!");
                strUserName.value = "";
                strUserName.focus();
                return false;
            }
            else if (Trim(strFirstName.value) == "") {
                alert("First name should not be empty!");
                strFirstName.value = "";
                strFirstName.focus();
                return false;
            }
            else if (Trim(strLastName.value) == "") {
                alert("Last name should not be empty!");
                strLastName.value = "";
                strLastName.focus();
                return false;
            }
            else if (Trim(strUserEmail.value) == "") {
                alert("Email address should not be empty!");
                strUserEmail.value = "";
                strUserEmail.focus();
                return false;
            }
            else if (EmailCheck(strUserEmail.value) == false) {
                alert('Invalid email address : ' + strUserEmail.value);
                strUserEmail.focus();
                return false;
            }
            else if (Trim(strPassword.value) == "") {
                alert("Password should not be empty!");
                strPassword.focus();
                return false;
            }
            else if (Trim(strPassword1.value) == "") {
                alert("Retype password should not be empty!");
                strPassword1.focus();
                return false;
            }
            else if (!strPassword.value.match(re)) {
                alert('Password is weak. Password must contain: Minimum 8 characters. It must contain characters from 3 of the following 4 categories atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number or 1 Special Character');
                strPassword.value = "";
                strPassword.focus();
                return false;
            }
            else if (Trim(strPassword.value) != Trim(strPassword1.value)) {
                alert("Passwords didnt match !");
                strPassword.value = "";
                strPassword1.value = "";
                strPassword.focus();
                return false;
            }
            else if (Trim(strEmail.value) != "") {
                var emailArr = new Array();
                emailArr = String(strEmail.value).split(",");
                for (var i = 0; i <= emailArr.length - 1; i++) {
                    var sEmail = emailArr[i];
                    if (sEmail != "") {
                        if (EmailCheck(sEmail) == false) {
                            alert('Invalid email address : ' + sEmail);
                            strEmail.focus();
                            return false;
                        }
                    }
                }
            }

            if (Trim(strPhone.value) != "") {
                var sPhoneNos = '';
                var PhonesArr = new Array();
                PhonesArr = String(strPhone.value).split(",");

                for (var i = 0; i <= PhonesArr.length - 1; i++) {
                    var nPhone = PhonesArr[i];
                    var PhoneVal = nPhone.replace(/\D+/g, '');

                    if (i == 0)
                        sPhoneNos = PhoneVal;
                    else
                        sPhoneNos = sPhoneNos + ',' + PhoneVal;
                }

                strPhone.value = sPhoneNos;

                for (var i = 0; i <= PhonesArr.length - 1; i++) {
                    var nPhone = PhonesArr[i];
                    var PhoneVal = nPhone.replace(/\D+/g, '');

                    if (PhoneVal != "") {
                        if (PhoneVal.length < 10) {
                            alert('Please Enter Valid Phone Number: ' + PhoneVal);
                            strPhone.focus();
                            return false;
                        }
                        else if (PhoneVal.length == 10) {
                            alert('Please Enter Country Code: ' + PhoneVal);
                            strPhone.focus();
                            return false;
                        }
                    }
                }
            }
            if (Trim(strpwd.value) == "") {
                alert("User Verification Password should not be empty!");
                strpwd.value = "";
                strpwd.focus();
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
        var curUserid = "";

        this.onload = function () {
            document.getElementById('tdLeftMenu').style.display = "none";
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;

            LoadUserData(0);
        }

        function Redirect(sid) {
            location.href = "Setting.aspx";
        }

        function ShowAddUser() {

            clearCtrlData();

            document.getElementById("<%=btnSave.ClientID%>").style.display = '';
            document.getElementById("<%=trPwd.ClientID%>").style.display = '';
            document.getElementById("<%=trConPwd.ClientID%>").style.display = '';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = 'none';
            document.getElementById("<%=trStatus.ClientID%>").style.display = 'none';

            $('#divSiteUserConfig').hide('slide', { direction: 'right' }, 500);
            $('#tdAddUser').show('slide', { direction: 'left' }, 500);
        }

        function HideAddUser() {
            clearCtrlData();
            $('#tdAddUser').hide('slide', { direction: 'left' }, 500);
        }

        function LoadUserData(isExport) {
            document.getElementById("divLoading").style.display = "";
            Load_Setup_User(isExport);
        }

        function CallUserSetup(IsAdd) {

            var companyid, firstname, lastname, username, password, status, usertype, email, batteryreplacement,
                usertypeid, userrole, userroleid, associatedemail, associatedphone, authuserid, authpassword, IsPrismView,
                AllowAccess, IsPrismAuditView, AssetNotification, DesktopNotifications, IsPrismReadOnly, AllowAccessKPI, IsPulseReport;

            $('#ctl00_ContentPlaceHolder1_lblMessage').html('');

            if (IsAdd == "1") {

                if (!Validate()) return false;

                var d = document.getElementById("<%=selUserRole.ClientID%>");
                userrole = d.options[d.selectedIndex].text;
                userroleid = d.options[d.selectedIndex].value;

                try {
                    PageVisitDetails(g_UserId, "User List", enumPageAction.Add, "Created a new user [" + username + "] with the role of [" + userrole + "] ");
                }
                catch (e) {

                }
            }
            else {

                if (!EditValidate()) return false;
                UserDataId = curUserid;

                var d = document.getElementById("<%=selUserRole.ClientID%>");
                userrole = d.options[d.selectedIndex].text;
                userroleid = d.options[d.selectedIndex].value;

                try {
                    PageVisitDetails(g_UserId, "User List", enumPageAction.Edit, "Edit user profile : User Id [" + UserDataId + "]");
                }
                catch (e) {

                }

                if (userstatus == "True") {
                    userstatus = "1";
                }
                else {
                    userstatus = "0";
                }

                if (userstatus != status) {
                    try {
                        PageVisitDetails(g_UserId, "User List", enumPageAction.Edit, "Status changed for user [" + username + "] with the role of [" + userrole + "] ");
                    }
                    catch (e) {

                    }
                }
            }

            companyid = document.getElementById("<%=selCompany.ClientID%>").value;
            firstname = document.getElementById("<%=txtFirstName.ClientID%>").value;
            lastname = document.getElementById("<%=txtLastName.ClientID%>").value;
            username = document.getElementById("<%=txtUserName.ClientID%>").value;
            password = document.getElementById("<%=txtPassword.ClientID%>").value;
            status = document.getElementById("<%=selStatus.ClientID%>").value;
            var e = document.getElementById("<%=selUser.ClientID%>");
            usertype = e.options[e.selectedIndex].text;
            usertypeid = e.options[e.selectedIndex].value;
            email = document.getElementById("<%=txtUserEmail.ClientID%>").value;
            batteryreplacement = document.getElementById("<%=chkbatteryreplace.ClientID%>").checked;
            associatedemail = document.getElementById("<%=txtAssociatedEmail.ClientID%>").value;
            associatedphone = document.getElementById("<%=txtPhnos.ClientID%>").value;
            authpassword = document.getElementById("<%=txtpwd.ClientID%>").value;
            IsTempMonitoring = document.getElementById("<%=chkIsTempMonitoring.ClientID%>").checked;
            IsPrismView = document.getElementById("<%=chkIsPrismView.ClientID%>").checked;
            IsPrismAuditView = document.getElementById("<%=chkPrismAuditView.ClientID%>").checked;
            AllowAccess = document.getElementById("<%=ChkAllowAccess.ClientID%>").checked;
            AssetNotification = document.getElementById("<%=chkAssetNotification.ClientID%>").checked;
            DesktopNotifications = document.getElementById("<%=chkDesktopNotifications.ClientID%>").checked;
            IsPrismReadOnly = document.getElementById("<%=chkIsPrismReadOnly.ClientID%>").checked;
            AllowAccessKPI = document.getElementById("<%=ChkAllowAccessKPI.ClientID%>").checked;
            IsPulseReport = document.getElementById("<%=chkPulseReport.ClientID%>").checked;

            UserDataId = document.getElementById("<%=hdnUserDataId.ClientID%>").value;

            if (UserDataId == "")
                IsAdd = "1";
            else
                IsAdd = "2";

            var isTempMonitoring = GetBoolData(IsTempMonitoring);
            var isPrismAccess = GetBoolData(IsPrismView);
            var isPrismAuditAccess = GetBoolData(IsPrismAuditView);
            var isAllowStarAccess = GetBoolData(AllowAccess);
            var isNotification = GetBoolData(AssetNotification);
            var isDesktopNotification = GetBoolData(DesktopNotifications);
            var isPrismReadOnly = GetBoolData(IsPrismReadOnly);
            var isAllowAccessKPI = GetBoolData(AllowAccessKPI);
            var isPulseReport = GetBoolData(IsPulseReport);

            var PulseReportIds = "";

            $('#tblPulseReports tr td input.clsCheckBoxPulseReport:checkbox').each(function () {
                var ids = this.id;

                if ($('#' + ids).is(':checked'))
                    PulseReportIds += "," + $('#' + ids).val();
            });

            AddUserSetup(UserDataId, IsAdd, companyid, username, password, status, usertype, email,
                batteryreplacement, usertypeid, userroleid, associatedemail, associatedphone, authpassword,
                isTempMonitoring, isPrismAccess, isPrismAuditAccess, isAllowStarAccess, isNotification, isDesktopNotification, isPrismReadOnly, isAllowAccessKPI, isPulseReport, PulseReportIds, firstname, lastname);
        }

        function GetBoolData(bChkView) {

            if (bChkView == true) {
                return 1;
            }
            else {
                return 0;
            }
        }

        function doAfterAddedUser() {
            window.location = "UserList.aspx";
        }

        //Reset Password
        function generatePassword() {

            if (confirm("Are you sure do you want to reset the password?") == true) {

                var UserName, UserDataId, password, authpassword;
                UserDataId = curUserid;
                authpassword = document.getElementById("<%=txtpwd.ClientID%>").value;
                UserName = document.getElementById("<%=txtUserName.ClientID%>").value;
                document.getElementById("lblPassword").innerHTML = "";

                if (authpassword == "") {
                    document.getElementById("<%=txtpwd.ClientID%>").focus();
                    alert("Password for user verification should not be empty!");
                    return false;
                }

                password = randomPass();
                document.getElementById("divLoading").style.display = "";
                ResetPassword(UserName, UserDataId, password, authpassword);
            }
        }

        function clearCtrlData() {

            document.getElementById("<%=selCompany.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=txtUserName.ClientID%>").value = "";
            document.getElementById("<%=txtFirstName.ClientID%>").value = "";
            document.getElementById("<%=txtLastName.ClientID%>").value = "";
            document.getElementById("<%=txtPassword.ClientID%>").value = "";
            document.getElementById("<%=txtPassword1.ClientID%>").value = "";
            document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=txtPhnos.ClientID%>").value = "";
            document.getElementById("<%=txtUserEmail.ClientID%>").value = "";
            document.getElementById("<%=chkbatteryreplace.ClientID%>").checked = false;
            document.getElementById("<%=selUser.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=selUserRole.ClientID%>").selectedIndex = 0;
            document.getElementById("<%=txtAssociatedEmail.ClientID%>").value = "";
            document.getElementById("<%=txtpwd.ClientID%>").value = "";
            document.getElementById("ctl00_ContentPlaceHolder1_hdnUserDataId").value = "";
            document.getElementById("<%=ChkAllowAccess.ClientID%>").checked = false;
            document.getElementById("<%=chkIsPrismView.ClientID%>").checked = false;

            $('#tblPulseReports tr td input[type="checkbox"]').each(function () {
                $(this).prop('checked', false);
            });
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

        function deleteConfiguration(userId, siteId) {

            if (siteId != "0") {
                alert("User associated with sites.");
            }
            else {
                if (confirm("Are you sure do you want to delete this user?") == true) {

                    document.getElementById("<%=hid_userdeleteid.ClientID%>").value = userId;
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
        }

        function conformDelete() {

            var conpwd = document.getElementById("<%=hid_pwd.ClientID%>").value;
            var userId = document.getElementById("<%=hid_userdeleteid.ClientID%>").value;
            var password = document.getElementById('txtUserPassword').value;

            if (password == conpwd) {
                DeleteConfigurationSetting(userId);

            }
            else if (password == "") {
                alert("Password should not be empty!");
            }
            else {
                alert("Incorrect Password");
                document.getElementById('txtUserPassword').value = "";
            }

        }

        var userstatus = "";

        function showUserConfigurationForEdit(UserDataId, UserName, Email, UserType, BatteryReplacement, Status, CompanyId, UserTypeID,
                 UserRoleId, AssoEmail, AssoPhone, IsTempMonitoring, IsPrismView, IsPrismAuditView, AllowAccesForStar, EnableNotification,
                 DesktopNotifications, IsPrismReadOnly, AllowAccesForKPI, isPulseReport, PulseReportIds, FirstName, LastName) {

            clearCtrlData();

            curUserid = UserDataId;
            document.getElementById("<%=btnSave.ClientID%>").style.display = 'none';
            document.getElementById("<%=trPwd.ClientID%>").style.display = 'none';
            document.getElementById("<%=trConPwd.ClientID%>").style.display = 'none';
            document.getElementById("<%=btnUpdate.ClientID%>").style.display = '';
            document.getElementById("<%=trStatus.ClientID%>").style.display = '';

            document.getElementById("tdError").innerHTML = "";
            document.getElementById("lblPassword").innerHTML = "";

            $('#divSiteUserConfig').hide('slide', { direction: 'right' }, 500);
            $('#tdAddUser').show('slide', { direction: 'left' }, 500)

            userstatus = Status;

            document.getElementById("btnChangePassword").style.display = '';
            $('#ctl00_ContentPlaceHolder1_lblMessage').html('');

            if (Status == "True")
                document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 0;
            else
                document.getElementById("<%=selStatus.ClientID%>").selectedIndex = 1;

            if (BatteryReplacement == "True")
                document.getElementById("<%=chkbatteryreplace.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkbatteryreplace.ClientID%>").checked = false;

            if (IsPrismView == 1)
                document.getElementById("<%=chkIsPrismView.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsPrismView.ClientID%>").checked = false;

            if (IsTempMonitoring == 0)
                document.getElementById("<%=ChkIsTempMonitoring.ClientID%>").checked = false;
            else
                document.getElementById("<%=ChkIsTempMonitoring.ClientID%>").checked = true;

            if (AllowAccesForStar == 1)
                document.getElementById("<%=ChkAllowAccess.ClientID%>").checked = true;
            else
                document.getElementById("<%=ChkAllowAccess.ClientID%>").checked = false;

            if (IsPrismAuditView == 1)
                document.getElementById("<%=chkPrismAuditView.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkPrismAuditView.ClientID%>").checked = false;

            if (EnableNotification == 1)
                document.getElementById("<%=chkAssetNotification.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkAssetNotification.ClientID%>").checked = false;

            if (DesktopNotifications == 1)
                document.getElementById("<%=chkDesktopNotifications.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkDesktopNotifications.ClientID%>").checked = false;

            if (IsPrismReadOnly == 1)
                document.getElementById("<%=chkIsPrismReadOnly.ClientID%>").checked = true;
            else
                document.getElementById("<%=chkIsPrismReadOnly.ClientID%>").checked = false;

            if (AllowAccesForKPI == 1)
                document.getElementById("<%=ChkAllowAccessKPI.ClientID%>").checked = true;
            else
                document.getElementById("<%=ChkAllowAccessKPI.ClientID%>").checked = false;

            if (isPulseReport == 1)
                document.getElementById("<%=ChkPulseReport.ClientID%>").checked = true;
            else
                document.getElementById("<%=ChkPulseReport.ClientID%>").checked = false;

            var element_CompanyId = document.getElementById("<%=selCompany.ClientID%>");
            element_CompanyId.value = CompanyId;

            var element_TypeID = document.getElementById("<%=selUser.ClientID%>");
            element_TypeID.value = UserTypeID;

            var element_RoleId = document.getElementById("<%=selUserRole.ClientID%>")
            element_RoleId.value = UserRoleId;

            document.getElementById("<%=txtUserName.ClientID%>").value = decodeURIComponent(setundefined(UserName));
            document.getElementById("<%=txtFirstName.ClientID%>").value = decodeURIComponent(setundefined(FirstName));
            document.getElementById("<%=txtLastName.ClientID%>").value = decodeURIComponent(setundefined(LastName));
            document.getElementById("<%=txtUserEmail.ClientID%>").value = decodeURIComponent(setundefined(Email));
            document.getElementById("<%=txtAssociatedEmail.ClientID%>").value = decodeURIComponent(setundefined(AssoEmail));
            document.getElementById("<%=txtPhnos.ClientID%>").value = setundefined(AssoPhone);
            document.getElementById("ctl00_ContentPlaceHolder1_hdnUserDataId").value = UserDataId;

            $('#tblPulseReports tr td input.clsCheckBoxPulseReport:checkbox').each(function () {
                $(this).prop('checked', false);
            });

            if (setundefined(PulseReportIds) != "") {

                var arrPulseReportId = PulseReportIds.split(',');

                $('#tblPulseReports tr td input.clsCheckBoxPulseReport:checkbox').each(function () {
                    var ids = this.id;
                    var arrid = ids.split('_');
                    if ($.inArray(arrid[1], arrPulseReportId) != -1) {
                        $('#' + ids).prop('checked', true);
                    }
                });
            }

            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }

        function showSiteUserConfiguration(UserId, UserName) {

            clearCtrlData();
            curUserid = UserId;
            document.getElementById("lblUserName").innerHTML = decodeURIComponent(UserName);

            document.getElementById("ctl00_ContentPlaceHolder1_hdnUserDataId").value = UserId;
            $('#tdAddUser').hide('slide', { direction: 'left' }, 150);
            $('#divSiteUserConfig').show('slide', { direction: 'right' }, 500);
            GetAvailableSitesForUser(UserId);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }

        function AddRemoveSiteUser(isAdd) {

            var e;
            var UserSiteID = "";
            var mode = 0;
            var opt;

            var UserDataId = document.getElementById("<%=hdnUserDataId.ClientID%>").value;

            if (isAdd == "0") {
                e = document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated");
                if (e.selectedIndex == -1) {
                    alert("Please select an site from associated sites");
                    return;
                }
                else {
                    for (var i = 0, iLen = e.length; i < iLen; i++) {
                        opt = e[i];
                        if (opt.selected) {
                            UserSiteID = UserSiteID + opt.value + ",";
                        }
                    }
                }
            }
            else if (isAdd == "1") {
                e = document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable");
                if (e.selectedIndex == -1) {
                    alert("Please select an site from available sites");
                    return;
                }
                else {
                    for (var i = 0, iLen = e.length; i < iLen; i++) {
                        opt = e[i];
                        if (opt.selected) {
                            UserSiteID = UserSiteID + opt.value + ",";
                        }
                    }
                }
            }

            EditSitesForUser(UserSiteID, UserDataId, isAdd);
        }

        $(document).ready(function () {

            //Events
            $('#btnExportUsers').on('click', function () {
                OpenUsersDialog();
            });

        });

        function ClearPasswordPopup() {
            $('#dialog-ChangePassword').dialog('close');
        }
		
    </script>
    <div id="tooltip3" class="tool3" style="display: none;">
    </div>
    <!-- User LIST -->
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr style="height: 10px;">
                        <td>
                            <input type="hidden" id="hid_userrole" runat="server" />
                            <input type="hidden" id="hid_pwd" runat="server" />
                            <input type="hidden" id="hid_userid" runat="server" />
                            <input type="hidden" id="hid_userdeleteid" runat="server" />
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
                                                        <img src='images/Left-Arrow.png' alt='' title='Settings' border="0" /></a>
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
                                                                Manage Users
                                                            </td>
                                                            <td align="right" style="width: 700px;">
                                                                <input type="button" id="btnExportUsers" value="Export Users" class="clsButton" style="width: 130px;" />
                                                            </td>
                                                            <td align="right">
                                                                <input type="button" id="btnAdd" value="Add" class="clsButton" onclick="ShowAddUser();" />
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
                                        <div id="tdAddUser" class="cssTable" style="display: none;">
                                            <table cellpadding="3" cellspacing="5" border="0" id="tblTools">
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Company Name :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <asp:dropdownlist class="cssDropDown" id="selCompany" name="selCompany" runat="server"
                                                            width="270px">
                                                        </asp:dropdownlist>
                                                        &nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        User Type :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <asp:dropdownlist class="cssDropDown" id="selUser" name="selUser" runat="server"
                                                            width="140px">
                                                        </asp:dropdownlist>
                                                        &nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        User Role :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <asp:dropdownlist class="cssDropDown" id="selUserRole" name="selUserRole" runat="server"
                                                            width="140px">
                                                        </asp:dropdownlist>
                                                        &nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        User Name :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <input class="cssTextbox" type="text" id="txtUserName" name="txtUserName" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        First Name :
                                                    </td>
                                                    <td align="left" style="width: 280px;">
                                                        <input class="cssTextbox" type="text" id="txtFirstName" name="txtFirstName" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                    <td class="clsLALabel" style="width: 70px;" align="left">
                                                        Last Name :
                                                    </td>
                                                    <td align="left">
                                                        <input class="cssTextbox" type="text" id="txtLastName" name="txtLastName" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        User Email :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <input class="cssTextbox" type="text" id="txtUserEmail" name="txtUserEmail" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr id="trPwd" runat="server">
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Password :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <input class="cssTextbox" type="password" id="txtPassword" name="txtPassword" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr id="trConPwd" runat="server">
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Retype Password :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <input class="cssTextbox" type="password" id="txtPassword1" name="txtPassword1" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr id="trStatus" style="display: none;" runat="server">
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Status :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <select id="selStatus" class="cssDropDown" name="selStatus" runat="server" style="width: 100px;">
                                                            <option value="1" selected="selected">Active</option>
                                                            <option value="0">Inactive</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Battery Replacement :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <asp:checkbox id="chkbatteryreplace" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Associated Email :
                                                    </td>
                                                    <td align="left">
                                                        <input class="cssTextbox" type="text" id="txtAssociatedEmail" name="txtAssociatedEmail"
                                                            runat="server" style="width: 250px;" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td class="clsLALabel" style="width: 170px;" align="left">
                                                        Associated Phone Numbers :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <input class="cssTextbox" type="text" id="txtPhnos" name="txtPhnos" runat="server"
                                                            style="width: 250px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsLALabel" style="width: 120px;" align="left">
                                                        Password :
                                                    </td>
                                                    <td align="left" colspan="3">
                                                        <input class="cssTextbox" type="password" id="txtpwd" name="txtpwd" runat="server"
                                                            style="width: 250px;" />&nbsp;<span class="clsLALabel">(For User Verification)</span>&nbsp;<span
                                                                class="clsErrorTxt">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="Left">
                                                                    <span class="subHeader_black" style="color: green;">Special Permissions</span>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px;">
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <table cellpadding="3" cellspacing="3" border="0">
                                                                        <tr>
                                                                            <td align="Left" class="subHeader_black3">
                                                                                Pulse
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <input type="checkbox" id="ChkIsTempMonitoring" name="ChkIsTempMonitoring" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">EM Data View</label></input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <input type="checkbox" id="ChkAllowAccess" name="ChkAllowAccess" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">Star Settings</label></input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <input type="checkbox" id="ChkAllowAccessKPI" name="ChkAllowAccessKPI" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">Allow Access to KPIs</label></input>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td valign="top">
                                                                    <table cellpadding="3" cellspacing="3" border="0">
                                                                        <tr>
                                                                            <td align="Left" class="subHeader_black3">
                                                                                Prism
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trIsPrismView">
                                                                            <td align="left">
                                                                                <input type="checkbox" id="chkIsPrismView" name="chkIsPrismView" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">Prism Access</label></input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trIsPrismAuditView">
                                                                            <td align="left">
                                                                                <input type="checkbox" id="chkPrismAuditView" name="chkPrismAuditView" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">Prism Audit View</label></input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trIsPrismReadOnly">
                                                                            <td align="left">
                                                                                <input type="checkbox" id="chkIsPrismReadOnly" name="chkIsPrismReadOnly" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">Prism Read Only View</label></input>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td valign="top">
                                                                    <table cellpadding="3" cellspacing="3" border="0">
                                                                        <tr>
                                                                            <td align="Left" class="subHeader_black3">
                                                                                Asset Viewer
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <input type="checkbox" id="chkAssetNotification" name="chkAssetNotification" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox" value="1">Notification</label></input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <input type="checkbox" id="chkDesktopNotifications" name="chkDesktopNotifications"
                                                                                    class="clsCheckBox1" runat="server"><label class="clsCheckBox" value="1">Desktop Notifications</label></input>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td valign="top">
                                                                    <table cellpadding="3" cellspacing="3" border="0" id="tblPulseReports">
                                                                        <tr>
                                                                            <td align="Left" class="subHeader_black3">
                                                                                Pulse Reports
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <input type="checkbox" id="chkPulseReport" name="chkPulseReport" class="clsCheckBox1"
                                                                                    runat="server"><label class="clsCheckBox">Pulse Report</label></input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="padding-left: 20px;">
                                                                                <input type="checkbox" id="chkPulseReport_1" name="1" value="1" class="clsCheckBoxPulseReport"><label
                                                                                    class="clsCheckBox">Location History Report</label></input>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="4">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" value="Save" runat="server" class="clsExportExcel" id="btnSave"
                                                                        onclick="CallUserSetup(1);" />
                                                                </td>
                                                                <td>
                                                                    <input type="button" value="Update" runat="server" class="clsExportExcel" id="btnUpdate"
                                                                        onclick="CallUserSetup(2)" />
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnCancel" runat="server" value="Cancel" class="clsExportExcel"
                                                                        onclick="HideAddUser();" />
                                                                    <input type="hidden" name="hdnUserDataId" id="hdnUserDataId" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnChangePassword" value="Change Password" style="display: none;
                                                                        width: 130px; height: 30px;" class="clsExportExcel" onclick="OpenChangePasswordDialog();" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td colspan="1">
                                                                    <span id="lblPassword" class="clsLALabel" style="color: #3b9603"></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="4" id="tdError" class="clsMapErrorTxt">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divSiteUserConfig" class="clsFilterTable" style="display: none;">
                                            <table width="100%" cellpadding="3" cellspacing="1" class="clsFilterCriteria" border="0">
                                                <tr>
                                                    <td colspan="3">
                                                        <table border="0" cellpadding="6" cellspacing="0" width="100%" class="emailBorderStyle">
                                                            <tr>
                                                                <td align="left" style="width: 15%">
                                                                    <b>User Name:</b>
                                                                </td>
                                                                <td align="left" id="lblUserName" style="color: Black; font-weight: bold;">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="clsCntrlTxt">
                                                        Available Sites
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td class="clsCntrlTxt">
                                                        Associated Sites
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:listbox id="lstAvailable" selectionmode="Multiple" style="height: 300px; width: 390px;"
                                                            runat="server"></asp:listbox>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" class="clsButton" id="btnAddTOAsso" value="Add" onclick="AddRemoveSiteUser(1);" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" class="clsButton" id="btnRemove" value="Remove" onclick="AddRemoveSiteUser(0);" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:listbox id="lstAssociated" selectionmode="Multiple" style="height: 300px; width: 390px;"
                                                            runat="server"></asp:listbox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 15px;">
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
        </tr>
        <tr>
            <td valign="top">
                <table id="tblUserListInfo" cellspacing="1" cellpadding="3" style="width: 100%; padding-top: 10px;"
                    class="display">
                </table>
            </td>
        </tr>
        <tr style="height: 20px;">
        </tr>
    </table>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divUpdate">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div style="position: fixed; top: 360px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <div id="Password_dialog" title="Confirm Password" style="display: none;">
        <table cellpadding="5">
            <tr>
                <td>
                    Enter Password
                </td>
                <td>
                    <input class="clsLAText" type="password" id="txtUserPassword" name="txtPassword"
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
    <div id="dialog-ChangePassword" style="width: auto; min-height: 0px; max-height: none;
        height: 132px; display: none;">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr align="left">
                    <td class="clsLALabel" style="width: 120px;" align="right">
                        Old Password &nbsp;:&nbsp;
                    </td>
                    <td style="width: 320px;" align="left">
                        <input type="password" id="txtOldPassword" style="width: 300px;" />
                    </td>
                </tr>
                <tr style="height: 10px;">
                </tr>
                <tr align="left">
                    <td class="clsLALabel" style="width: 120px;" align="right">
                        New Password &nbsp;:&nbsp;
                    </td>
                    <td style="width: 320px;" align="left">
                        <input type="password" id="txtNewPassword" style="width: 300px;" />
                    </td>
                </tr>
                <tr style="height: 10px;">
                </tr>
                <tr align="left">
                    <td class="clsLALabel" style="width: 120px;" align="right">
                        Confirm Password &nbsp;:&nbsp;
                    </td>
                    <td style="width: 320px;" align="left">
                        <input type="password" id="txtConfirmPassword" style="width: 300px;" />
                    </td>
                </tr>
                <tr style="height: 10px;">
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <input type="button" id="btnAddPassword" class="clsExportExcel" value="Save" onclick="ChangePassword_ForSeletedUser();" />
                        <input type="button" id="btnCancelPassword" class="clsExportExcel" value="Cancel"
                            onclick="ClearPasswordPopup();" />
                    </td>
                </tr>
                <tr style="height: 10px;">
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <label id="lblAddPasswordMsg">
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="display: none;" id="divLoading_ChangePassword">
            <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;">
        </div>
    </div>
    <!-- Export Users !-->
    <div id="divExportUsers" title="Export Users" style="display: none;">
        <table cellpadding="5" cellspacing="5" border="0" width="530px">
            <tr style="height: 10px;">
                <td class="clsLALabel" style="width: 80px;" align="left" valign="middle">
                    Site:
                </td>
                <td align="left">
                    <select id="selSite" runat="server" style="width: 500px;">
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="button" value="Generate" class="clsExportExcel" id="btnGenerateUsers"
                        style="width: 120px;" onclick="Load_Setup_User(1);" />
                </td>
            </tr>
        </table>
    </div>
</asp:content>
