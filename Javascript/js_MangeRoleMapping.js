var g_RoleObj;

function UpdateRoleMapping() {
    var Roles = "";
    var bIsEnableRoles = 0;

    if ($('#chkEnableActiveDirectoryRoles').is(":checked")) {
        bIsEnableRoles = 1;

        if ($('#txtSSOIAttribute').val() == '') {
            alert("Please enter a SSOI Attribute");
            return;
        }

        if ($('#txtCustomerRole').val() != '' || $('#txtMaintenanceRole').val() != '' || $('#txtPartnerRole').val() != '' ||
           $('#txtEnginneringRole').val() != '' || $('#txtAdminRole').val() != '' || $('#txtTechnicalAdminRole').val() != '' || $('#txtTBDRole').val() != '')
            Roles = "";
        else {
            alert("Please enter a role");
            return;
        }
    }

    var SSOIAttribute = $('#txtSSOIAttribute').val();

    //Cutomer
    if ($('#txtCustomerRole').val() != '')
        Roles = enumUserRoleArr.Customer + "~" + $('#txtCustomerRole').val();

    //Maintenance    
    if ($('#txtMaintenanceRole').val() != '') {
        if (Roles != "")
            Roles = Roles + "|" + enumUserRoleArr.Maintenance + "~" + $('#txtMaintenanceRole').val();
        else
            Roles = enumUserRoleArr.Maintenance + "~" + $('#txtMaintenanceRole').val();
    }

    //Partner
    if ($('#txtPartnerRole').val() != '') {
        if (Roles != "")
            Roles = Roles + "|" + enumUserRoleArr.Partner + "~" + $('#txtPartnerRole').val();
        else
            Roles = enumUserRoleArr.Partner + "~" + $('#txtPartnerRole').val();
    }

    //Enginnering
    if ($('#txtEnginneringRole').val() != '') {
        if (Roles != "")
            Roles = Roles + "|" + enumUserRoleArr.Engineering + "~" + $('#txtEnginneringRole').val();
        else
            Roles = enumUserRoleArr.Engineering + "~" + $('#txtEnginneringRole').val();
    }

    //Admin
    if ($('#txtAdminRole').val() != '') {
        if (Roles != "")
            Roles = Roles + "|" + enumUserRoleArr.Admin + "~" + $('#txtAdminRole').val();
        else
            Roles = enumUserRoleArr.Admin + "~" + $('#txtAdminRole').val();
    }

    //Technical Admin
    if ($('#txtTechnicalAdminRole').val() != '') {
        if (Roles != "")
            Roles = Roles + "|" + enumUserRoleArr.TechnicalAdmin + "~" + $('#txtTechnicalAdminRole').val();
        else
            Roles = enumUserRoleArr.TechnicalAdmin + "~" + $('#txtTechnicalAdminRole').val();
    }

    //Maintenance/Prism
    if ($('#txtTBDRole').val() != '') {
        if (Roles != "")
            Roles = Roles + "|" + enumUserRoleArr.MaintenancePrism + "~" + $('#txtTBDRole').val();
        else
            Roles = enumUserRoleArr.MaintenancePrism + "~" + $('#txtTBDRole').val();
    }

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=UpdateMangeRoles",
    {
        EnableActiveDirectoryRoles: bIsEnableRoles,
        SSOIAttribute: SSOIAttribute,
        Roles: Roles
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxRoleMappingInfo(dsRoot);
        }
        else {
            document.getElementById("divLoading").style.display = "none";
        }
    });
}

//*********************************************************
//	Function Name	:	ajaxRoleMappingInfo
//	Input			:	none
//	Description		:	Load Announcements Datas from ajax Response
//*********************************************************
function ajaxRoleMappingInfo(dsRoot) {
    var o_Result = dsRoot.getElementsByTagName('Result')
    var Result = getTagNameValue(o_Result[0]);
    document.getElementById("divLoading").style.display = "none";

    if (Result == "0") {
        document.getElementById("tdMsg").innerHTML = "Successfully Updated ..!";
    }
    else if (Result == "1") {
        document.getElementById("tdMsg").innerHTML = "Error in Adding!!!";
    }
}

var Role_Obj;

function Load_RoleMappingInfo() {

    Role_Obj = CreateStarXMLObj();

    document.getElementById("divLoading").style.display = "";

    if (Role_Obj != null) {
        Role_Obj.onreadystatechange = ajaxRoleMapping;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetMangeRolesInfo&sid=0";

        if (GetBrowserType() == "isIE") {
            Role_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            Role_Obj.open("GET", DbConnectorPath, true);
        }

        Role_Obj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxStarList
//	Input			:	none
//	Description		:	Load Star Datas from ajax Response
//*********************************************************
var g_RoleRoot;
function ajaxRoleMapping() {
    if (Role_Obj.readyState == 4) {
        if (Role_Obj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(Role_Obj.responseXML.documentElement);
            g_RoleRoot = Role_Obj.responseXML.documentElement;

            LoadRoleMappingList();
        }
    }
}

var g_CustomerRole, g_MaintenanceRole, g_PartnerRole, g_EnginneringRole, g_AdminRole, g_TechnicalAdminRole, g_MaintenancePrism, g_SSOIAttribute;

function LoadRoleMappingList() {
    var o_EnableActiveDirectoryRoles = g_RoleRoot.getElementsByTagName('EnableActiveDirectoryRoles')
    var o_SSOIAttribute = g_RoleRoot.getElementsByTagName('SSOIAttribute')

    nRootLength = o_EnableActiveDirectoryRoles.length;

    //sitename
    if (nRootLength > 0) {
        var EnableActiveDirectoryRoles = (o_EnableActiveDirectoryRoles[0].textContent || o_EnableActiveDirectoryRoles[0].innerText || o_EnableActiveDirectoryRoles[0].text);
        var SSOIAttribute = (o_SSOIAttribute[0].textContent || o_SSOIAttribute[0].innerText || o_SSOIAttribute[0].text);
        g_SSOIAttribute = SSOIAttribute;

        if (EnableActiveDirectoryRoles == "True") {
            $('#chkEnableActiveDirectoryRoles').prop('checked', true);
            $('#txtSSOIAttribute').val(SSOIAttribute);
        }
        else {
            $('#txtSSOIAttribute').val(SSOIAttribute);
            $('#chkEnableActiveDirectoryRoles').prop('checked', false);
            $("#tblRoles").find("input,button,textarea,select").attr("disabled", true);
            $("#txtSSOIAttribute").attr("disabled", true);
        }

        var RoleTable = $(g_RoleRoot).find("List");
        nRootLength = RoleTable.length;

        var o_GMSRoleId = $(RoleTable).children().filter('GMSRoleId');
        var o_RoleName = $(RoleTable).children().filter('RoleName');

        if (nRootLength > 0) {
            for (var i = 0; i < nRootLength; i++) {
                var GMSRoleId = (o_GMSRoleId[i].textContent || o_GMSRoleId[i].innerText || o_GMSRoleId[i].text);
                var RoleName = (o_RoleName[i].textContent || o_RoleName[i].innerText || o_RoleName[i].text);

                //Cutomer
                if (GMSRoleId == enumUserRoleArr.Customer) {
                    $('#txtCustomerRole').val(RoleName);
                    g_CustomerRole = RoleName;
                }

                //Maintenance
                if (GMSRoleId == enumUserRoleArr.Maintenance) {
                    $('#txtMaintenanceRole').val(RoleName);
                    g_MaintenanceRole = RoleName;
                }

                //Partner
                if (GMSRoleId == enumUserRoleArr.Partner) {
                    $('#txtPartnerRole').val(RoleName);
                    g_PartnerRole = RoleName;
                }

                //Enginnering
                if (GMSRoleId == enumUserRoleArr.Engineering) {
                    $('#txtEnginneringRole').val(RoleName);
                    g_EnginneringRole = RoleName;
                }

                //Admin
                if (GMSRoleId == enumUserRoleArr.Admin) {
                    $('#txtAdminRole').val(RoleName);
                    g_AdminRole = RoleName;
                }

                //Technical Admin
                if (GMSRoleId == enumUserRoleArr.TechnicalAdmin) {
                    $('#txtTechnicalAdminRole').val(RoleName);
                    g_TechnicalAdminRole = RoleName;
                }
                //Maintenance/Prism
                if (GMSRoleId == enumUserRoleArr.MaintenancePrism) {
                    $('#txtTBDRole').val(RoleName);
                    g_MaintenancePrism = RoleName;
                }
            }
        }
    }
    else {
        $("#tblRoles").find("input,button,textarea,select").attr("disabled", true);
        $("#txtSSOIAttribute").attr("disabled", true);
    }

    document.getElementById("divLoading").style.display = "none";
}

var g_ADObj;

function UpdateADDirectory() {

    g_ADObj = CreateXMLObj();

    var ADServerIp = "";
    var ADUserName = "";
    var ADPassword = "";

    ADServerIp = setundefined($('#txtServerIp').val());
    ADUserName = setundefined($('#txtUserName').val());
    ADPassword = setundefined($('#txtPwd').val());

    if (ADServerIp == '') {
        alert("Please enter a server");
        $('#txtServerIp').focus();
        return;
    }
    else if (ADUserName == '') {
        alert("Please enter a user name");
        $('#txtUserName').focus();
        return;
    }
    else if (ADPassword == '') {
        alert("Please enter a password");
        $('#txtPwd').focus();
        return;
    }
    else if (!ADPassword.match(re)) {
        alert('Password is weak. Password must contain: Minimum 8 characters. It must contain characters from 3 of the following 4 categories atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number or 1 Special Character');
        $('#txtPwd').val('');
        $('#txtPwd').focus();
        return false;
    }

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=ConfigADDirectory",
    {
        ServerIp: ADServerIp,
        UserName: ADUserName,
        Password: ADPassword
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxUpdateADDirectory(dsRoot);
        }
        else {
            document.getElementById("divLoading").style.display = "none";
        }
    });
}

//*********************************************************
//	Function Name	:	ajaxRoleMappingInfo
//	Input			:	none
//	Description		:	Load Announcements Datas from ajax Response
//*********************************************************
function ajaxUpdateADDirectory(dsRoot) {

    var o_Result = dsRoot.getElementsByTagName('Result')
    var Result = getTagNameValue(o_Result[0]);
    document.getElementById("divLoading").style.display = "none";

    if (Result == "0") {
        document.getElementById("tdMsg").innerHTML = "Successfully Updated ..!";
    }
    else if (Result == "1") {
        document.getElementById("tdMsg").innerHTML = "Error in Adding!!!";
    }
}

var Dire_Obj;

function Load_ADDirectoryInfo() {

    Dire_Obj = CreateStarXMLObj();

    document.getElementById("divLoading").style.display = "";

    if (Dire_Obj != null) {
        Dire_Obj.onreadystatechange = ajaxADDirectory;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetADDirectoryInfo&sid=0";

        if (GetBrowserType() == "isIE") {
            Dire_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            Dire_Obj.open("GET", DbConnectorPath, true);
        }

        Dire_Obj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxADDirectory
//	Input			:	none
//	Description		:	Load Star Datas from ajax Response
//*********************************************************
var g_RoleRoot;

function ajaxADDirectory() {
    if (Dire_Obj.readyState == 4) {
        if (Dire_Obj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(Dire_Obj.responseXML.documentElement);
            g_RoleRoot = Dire_Obj.responseXML.documentElement;

            LoadADServerList();
        }
    }
}

var g_ADServerIp = "";
var g_ADUserName = "";
var g_ADPwd = "";

function LoadADServerList() {

    var o_ServerIp = g_RoleRoot.getElementsByTagName('ServerIp')
    var o_UserName = g_RoleRoot.getElementsByTagName('UserName')
    var o_Pwd = g_RoleRoot.getElementsByTagName('Pwd')

    nRootLength = o_ServerIp.length;

    if (nRootLength > 0) {

        var ServerIp = (o_ServerIp[0].textContent || o_ServerIp[0].innerText || o_ServerIp[0].text);
        var UserName = (o_UserName[0].textContent || o_UserName[0].innerText || o_UserName[0].text);
        var Pwd = (o_Pwd[0].textContent || o_Pwd[0].innerText || o_Pwd[0].text);

        g_ADServerIp = ServerIp;
        g_ADUserName = UserName;
        g_ADPwd = Pwd;

        $('#txtServerIp').val(ServerIp);
        $('#txtUserName').val(UserName);
        $('#txtPwd').val(Pwd);
    }

    document.getElementById("divLoading").style.display = "none";
}