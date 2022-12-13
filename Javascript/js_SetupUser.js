function CreateInfraXMLObj() {

    var infraobj = null;
    if (window.ActiveXObject) {
        try {
            infraobj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                infraobj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                infraobj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
        infraobj = new XMLHttpRequest();
        infraobj.overrideMimeType('text/xml');
    }

    return infraobj;
}

var inf_Obj;
var g_IsAdd;

function AddUserSetup(UserId, IsAdd, CompanyId, UserName, Password, Status, UserType, Email, Batteryreplacement,
    UserTypeId, UserRoleId, AssociatedEmail, AssociatedPhone, AuthPassword, IsTempMonitoring, IsPrismView, IsPrismAuditView,
    AllowAccess, AssetNotifications, DesktopNotification, IsPrismReadOnly, AllowAccessKPI, isPulseReport, PulseReportIds, FirstName, LastName) {

    g_IsAdd = IsAdd;
   
    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=AddUserSetup",
    {
        UserId: UserId,
        isAdd: IsAdd,
        CompanyId: CompanyId,
        Un: UserName,
        Pw: Password,
        Status: Status,
        UserType: UserType,
        Email: Email,
        Batteryreplacement: Batteryreplacement,
        UserTypeId: UserTypeId,
        UserRoleId: UserRoleId,
        AssociatedEmail: AssociatedEmail,
        AssociatedPhone: AssociatedPhone,
        AuthPassword: AuthPassword,
        IsTempMonitoring: IsTempMonitoring,
        IsPrismView: IsPrismView,
        IsPrismAuditView: IsPrismAuditView,
        AllowAcess: AllowAccess,
        AssetNotifications: AssetNotifications,
        DesktopNotification: DesktopNotification,
        IsPrismReadOnly: IsPrismReadOnly,
        AllowAcessKPI: AllowAccessKPI,
	    isPulseReport: isPulseReport,
	    PulseReportIds: PulseReportIds,
	    FirstName: FirstName,
	    LastName: LastName		
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddUser(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function ajaxAddUser(dsRoot) {

    var o_Result = dsRoot.getElementsByTagName('Result');
    var o_Msg = dsRoot.getElementsByTagName('Msg');

    if (o_Result.length > 0) {

        var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);
        var Result = (o_Result[0].textContent || o_Result[0].innerText || o_Result[0].text);

        if (g_IsAdd == "1" || g_IsAdd == "2") {

            if (Result == "0") {
                document.getElementById("tdError").innerHTML = msg;
                doAfterAddedUser();
            }
            else {
                document.getElementById("tdError").innerHTML = msg;
            }
        }
        else {
            document.getElementById("tdAddUser").style.display = "none";
            doAfterAddedUser();
        }
    }
    else {
        document.getElementById("tdError").innerHTML = "Error in Add";
    }

    document.getElementById("divLoading").style.display = "none";
}

var g_isExport = 0;

function Load_Setup_User(isExport) {

    g_isExport = isExport;

    User_Obj = CreateInfraXMLObj();

    if (User_Obj != null) {

        User_Obj.onreadystatechange = ajaxUserList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=UserList&isExport=" + setundefined(isExport) + "&SiteId=" + setundefined($("#ctl00_ContentPlaceHolder1_selSite").val());

        if (GetBrowserType() == "isIE") {
            User_Obj.open("GET", DbConnectorPath, true);
        }

        else if (GetBrowserType() == "isFF") {
            User_Obj.open("GET", DbConnectorPath, true);
        }

        User_Obj.send(null);
    }

    return false;
}

function ajaxUserList() {

    if (User_Obj.readyState == 4) {

        if (User_Obj.status == 200) {

            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(User_Obj.responseXML.documentElement);

            var dsRoot = User_Obj.responseXML.documentElement;

            if (g_isExport == 1)
               ExportUserList(dsRoot);
            else
               PrepareUserList(dsRoot);
        } 
    }
}

function PrepareUserList(dsRoot) {
  
    var sTbl, sTblLen, sEmailRow, sShowStatus;
    
    var sTbl = document.getElementById("tblUserListInfo");

    if (GetBrowserType() == "isIE") {
        sTbl = document.getElementById('tblUserListInfo');
    }
    else if (GetBrowserType() == "isFF") {
        sTbl = document.getElementById('tblUserListInfo');
    }

    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var o_UserName = dsRoot.getElementsByTagName('UserName')
    var o_Email = dsRoot.getElementsByTagName('Email')
    var o_UserRole = dsRoot.getElementsByTagName('UserRole')
    var o_UserType = dsRoot.getElementsByTagName('UserType')
    var o_CompanyName = dsRoot.getElementsByTagName('CompanyName')
    var o_BatteryReplacement = dsRoot.getElementsByTagName('BatteryReplacement')
    var o_Status = dsRoot.getElementsByTagName('Status')
    var o_UserId = dsRoot.getElementsByTagName('UserId')
    var o_CompanyId = dsRoot.getElementsByTagName('CompanyId')
    var o_UserTypeID = dsRoot.getElementsByTagName('UserTypeID')
    var o_UserRoleId = dsRoot.getElementsByTagName('UserRoleId')
    var o_SiteId = dsRoot.getElementsByTagName('SiteId')
    var o_AssoEmail = dsRoot.getElementsByTagName('AssoEmail')
    var o_AssoPhone = dsRoot.getElementsByTagName('AssoPhone')
    var o_IsTempMonitoring = dsRoot.getElementsByTagName('IsTempMonitoring')
    var o_IsPrismView = dsRoot.getElementsByTagName('IsPrismView')
    var o_IsPrismAuditView = dsRoot.getElementsByTagName('IsPrismAuditView')
    var o_AccessStar = dsRoot.getElementsByTagName('AllowAccessForStar');
    var o_EnableNotification = dsRoot.getElementsByTagName('EnableNotification');
    var o_DesktopNotifications = dsRoot.getElementsByTagName('DesktopNotifications');
    var o_IsPrismReadOnly = dsRoot.getElementsByTagName('IsPrismReadOnly');
    var o_AccessKPI = dsRoot.getElementsByTagName('IsAllowAccessForKPI');
    var o_isPulseReport = dsRoot.getElementsByTagName('IsPulseReport');
    var o_PulseReportIds = dsRoot.getElementsByTagName('PulseReportIds');
    var o_FirstName = dsRoot.getElementsByTagName('FirstName');
    var o_LastName = dsRoot.getElementsByTagName('LastName');

    nRootLength = o_UserId.length;

    if (nRootLength > 0) {

        var head = "";

        head = "<thead><tr>" +
                " <th class='siteOverview_TopLeft_Box_wrap' style='width: 300px;'>User Name</th>" +
                " <th class='siteOverview_TopLeft_Box_wrap' style='width: 200px;'>First Name</th>" +
                " <th class='siteOverview_TopLeft_Box_wrap' style='width: 200px;'>Last Name</th>" +
                " <th class='siteOverview_Box' style='width: 200px;'>User Type</th>" +
                " <th class='siteOverview_Box' style='width: 200px;'>User Role</th>" +
                " <th class='siteOverview_Box' style='width: 250px;'>Email</th>" +
                " <th class='siteOverview_Box' style='width: 150px;'>Company Name</th>" +
                " <th class='siteOverview_Box' style='width: 10px;'>Sites</th>" +
                " <th class='siteOverview_Box' style='width: 100px;'>Status</th>" +
                " <th class='siteOverview_Topright_Box' style='width: 100px;'>Edit</th>" +
                "</tr>" +
               "</thead>";

        sTbl.innerHTML = head;

        body = document.createElement('tbody');

        for (var i = 0; i < nRootLength; i++) {

            var UserName = setundefined(o_UserName[i].textContent || o_UserName[i].innerText || o_UserName[i].text);
            var Email = setundefined(o_Email[i].textContent || o_Email[i].innerText || o_Email[i].text);
            var UserType = setundefined(o_UserType[i].textContent || o_UserType[i].innerText || o_UserType[i].text);
            var UserRole = setundefined(o_UserRole[i].textContent || o_UserRole[i].innerText || o_UserRole[i].text);
            var CompanyName = setundefined(o_CompanyName[i].textContent || o_CompanyName[i].innerText || o_CompanyName[i].text);
            var BatteryReplacement = setundefined(o_BatteryReplacement[i].textContent || o_BatteryReplacement[i].innerText || o_BatteryReplacement[i].text);
            var Status = setundefined(o_Status[i].textContent || o_Status[i].innerText || o_Status[i].text);
            var UserId = setundefined(o_UserId[i].textContent || o_UserId[i].innerText || o_UserId[i].text);
            var CompanyId = setundefined(o_CompanyId[i].textContent || o_CompanyId[i].innerText || o_CompanyId[i].text);
            var UserTypeID = setundefined(o_UserTypeID[i].textContent || o_UserTypeID[i].innerText || o_UserTypeID[i].text);
            var UserRoleId = setundefined(o_UserRoleId[i].textContent || o_UserRoleId[i].innerText || o_UserRoleId[i].text);
            var SiteId = setundefined(o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
            var AssoEmail = setundefined(o_AssoEmail[i].textContent || o_AssoEmail[i].innerText || o_AssoEmail[i].text);
            var AssoPhone = setundefined(o_AssoPhone[i].textContent || o_AssoPhone[i].innerText || o_AssoPhone[i].text);
            var IsTempMonitoring = setundefined(o_IsTempMonitoring[i].textContent || o_IsTempMonitoring[i].innerText || o_IsTempMonitoring[i].text);
            var IsPrismView = setundefined(o_IsPrismView[i].textContent || o_IsPrismView[i].innerText || o_IsPrismView[i].text);
            var IsPrismAuditView = setundefined(o_IsPrismAuditView[i].textContent || o_IsPrismAuditView[i].innerText || o_IsPrismAuditView[i].text);
            var AllowAccessForStar = setundefined((o_AccessStar[i].textContent || o_AccessStar[i].innerText || o_AccessStar[i].text));
            var EnableNotification = setundefined((o_EnableNotification[i].textContent || o_EnableNotification[i].innerText || o_EnableNotification[i].text));
            var DesktopNotifications = setundefined((o_DesktopNotifications[i].textContent || o_DesktopNotifications[i].innerText || o_DesktopNotifications[i].text));
            var IsPrismReadOnly = setundefined((o_IsPrismReadOnly[i].textContent || o_IsPrismReadOnly[i].innerText || o_IsPrismReadOnly[i].text));
            var AllowAccessForKPI = setundefined((o_AccessKPI[i].textContent || o_AccessKPI[i].innerText || o_AccessKPI[i].text));
            var isPulseReport = setundefined((o_isPulseReport[i].textContent || o_isPulseReport[i].innerText || o_isPulseReport[i].text));
            var PulseReportIds = setundefined((o_PulseReportIds[i].textContent || o_PulseReportIds[i].innerText || o_PulseReportIds[i].text));
            var FirstName = setundefined((o_FirstName[i].textContent || o_FirstName[i].innerText || o_FirstName[i].text));
            var LastName = setundefined((o_LastName[i].textContent || o_LastName[i].innerText || o_LastName[i].text));

            var statusid = "";

            if (Status == "True")
                statusid = "Active";
            else
                statusid = "Inactive";

            if (SiteId != "")
                SiteId = "[" + SiteId + "]";

            row = document.createElement('tr');
            AddCell(row, UserName, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
            AddCell(row, FirstName, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
            AddCell(row, LastName, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
            AddCell(row, UserType, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
            AddCell(row, UserRole, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
            AddCell(row, Email, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
            AddCell(row, CompanyName, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");

            //Site User
            if (UserTypeID == 4 || (UserTypeID == 3 && IsPrismView == "True"))
                AddCell(row, "<img id='site-" + i + "' src='images/imgEdit.png' onclick=showSiteUserConfiguration(" + UserId + ",\&quot;" + encodeURIComponent(UserName) + "\&quot;) " +
                                " onmouseover='btnPrev_LAServiceOver(this);' onmouseout='btnPrev_LAServiceOut(this);' border=0 title='Show Site config page..'/>&nbsp;" + SiteId, "siteOverview_Box_UserLog", "", "", "center", "75px", "30px", "");
            else
                AddCell(row, "", "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");

            AddCell(row, statusid, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");

            if (SiteId == "")
                SiteId = "0";

            var params = UserId + ",\&quot;" + encodeURIComponent(UserName) + "\&quot,\&quot;" + encodeURIComponent(Email) + "\&quot;,'" + UserType + "','" + BatteryReplacement + "','" + Status + "'," + CompanyId + "," + UserTypeID + "," + UserRoleId + ",\&quot;" + encodeURIComponent(AssoEmail) + "\&quot;,'" + AssoPhone + "'," + IsTempMonitoring + "," + IsPrismView + "," + IsPrismAuditView + "," + AllowAccessForStar + "," + EnableNotification + "," + DesktopNotifications + "," + IsPrismReadOnly + "," + AllowAccessForKPI + "," + isPulseReport + ",'" + PulseReportIds + "','" + encodeURIComponent(FirstName) + "','" + encodeURIComponent(LastName) + "'";

            AddCell(row, "<img src='images/imgEdit.png' title='Edit' onclick=showUserConfigurationForEdit(" + params + "); />&nbsp;<img src='images/imgDelete.png' title='Delete' onclick=deleteConfiguration(" + UserId + "," + SiteId + "); />", "siteOverview_Box_UserLog", "", "", "center", "", "30px", "8px");

            body.appendChild(row);
        }

        sTbl.appendChild(body);

        DatatablesLoadGraph_New("#tblUserListInfo", [8], 0, "asc", 20);
    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
    }

    document.getElementById("divLoading").style.display = "none";      
}

function ExportUserList(Excel_Root) {

    if (Excel_Root != null) {

        var o_Excel = Excel_Root.getElementsByTagName('Excel');
        var o_Filename = Excel_Root.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divLoading").style.display = "none";
    }
}
function DeleteConfigurationSetting(userId) {

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=DeleteUserConfiguration",
    {
        UserId: userId
    },
    function (data, status) {
        if (status == "success") {            
            AfterDeleteAjax();
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function AfterDeleteAjax() {    
    doAfterAddedUser();
}

function GetAvailableSitesForUser(UserId) {
    
    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=GetAvailableSitesForUser",
    {
        UserId: UserId
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxGetAvailableSitesForUserCallback(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function ajaxGetAvailableSitesForUserCallback(dsRoot) {

    var o_UserId = dsRoot.getElementsByTagName('UserId');
    var o_AvailableSites = dsRoot.getElementsByTagName('AvailableSites');
    var o_AssociatedSites = dsRoot.getElementsByTagName('AssociatedSites');

    document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable").innerHTML = "";
    document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated").innerHTML = "";

    nRootLength = o_UserId.length;

    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {

            var AvailableSites = "";
            AvailableSites = (o_AvailableSites[i].textContent || o_AvailableSites[i].innerText || o_AvailableSites[i].text);

            var AssociatedSites = "";
            AssociatedSites = (o_AssociatedSites[i].textContent || o_AssociatedSites[i].innerText || o_AssociatedSites[i].text);

            //Available Sites
            var availSites = setundefined(AvailableSites).split("|");
            if (availSites.length > 0) {
                for (var l = 0; l < availSites.length; l++) {
                    if (availSites[l] != "") {
                        var sitesInfo = availSites[l].split("~");
                        if (sitesInfo.length > 1) {

                            var id = sitesInfo[0];
                            var desc = sitesInfo[1];
                            var oLstField = document.getElementById("ctl00_ContentPlaceHolder1_lstAvailable");
                            var oOption = document.createElement("OPTION");
                            oLstField.options.add(oOption);

                            oOption.text = desc;
                            oOption.value = id;
                            oOption.title = desc;      
                        }
                    }
                }
            }

            //Associated Sites
            var associatSites = setundefined(AssociatedSites).split("|");
            if (associatSites.length > 0) {

                for (var l = 0; l < associatSites.length; l++) {
                    if (associatSites[l] != "") {
                        var sitesInfo = associatSites[l].split("~");

                        if (sitesInfo.length > 1) {
                            var id = sitesInfo[0];
                            var desc = sitesInfo[1];

                            var oLstField = document.getElementById("ctl00_ContentPlaceHolder1_lstAssociated");
                            var oOption = document.createElement("OPTION");

                            oLstField.options.add(oOption);

                            oOption.text = desc;
                            oOption.value = id;
                            oOption.title = desc;          
                        }
                    }
                }
            }
        }
    }

    document.getElementById("divLoading").style.display = "none";
}
 
function EditSitesForUser(siteId, UserId, isAdd) {

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=EditSitesForUser",
    {
        sid: siteId,
        UserId: UserId,
        isAdd: isAdd
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxGetAvailableSitesForUserCallback(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function Load_UserActivity(UserId, FromDate, EventType, ToDate) {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {
        inf_Obj.onreadystatechange = ajaxLoad_UserActivity;

        DbConnectorPath = "AjaxConnector.aspx?cmd=UserActivityLog&UserId=" + UserId + "&From=" + FromDate + "&EventType=" + EventType + "&To=" + ToDate;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    
    return false;
}

function ajaxLoad_UserActivity() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var sTbl, sTblLen;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblUserActivityInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblUserActivityInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblUserActivityInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_UserId = dsRoot.getElementsByTagName('UserId')
            var o_UserName = dsRoot.getElementsByTagName('UserName')
            var o_PageName = dsRoot.getElementsByTagName('PageName')
            var o_PageAction = dsRoot.getElementsByTagName('PageAction')
            var o_PageActionHistory = dsRoot.getElementsByTagName('PageActionHistory')
            var o_UpdatedOn = dsRoot.getElementsByTagName('UpdatedOn')
            var o_IPAddress = dsRoot.getElementsByTagName('IPAddress')
            
            nRootLength = o_UserId.length;
            
            //sitename
            if (nRootLength > 0) {
                var head = "";

                head = "<thead><tr>" +
                       "<th class='siteOverview_Box_UserLog'>#</th>" +
                       "<th class='siteOverview_Box_UserLog'>Date</th>" +
                       "<th class='siteOverview_Box_UserLog'>User</th>" +
                       "<th class='siteOverview_Box_UserLog'>Event</th>" +
                       "<th class='siteOverview_Box_UserLog'>Description</th>" +
                       "<th class='siteOverview_Box_UserLog'>IP Address</th>" +
                       "</tr></thead>";
                sTbl.innerHTML = head;

                body = document.createElement('tbody');                
                
                var sno = 0;

                for (var i = 0; i < nRootLength; i++) {
                                    
                    var UserId=(o_UserId[i].textContent || o_UserId[i].innerText || o_UserId[i].text);
                    var UserName = (o_UserName[i].textContent || o_UserName[i].innerText || o_UserName[i].text);
                    var PageAction = (o_PageAction[i].textContent || o_PageAction[i].innerText || o_PageAction[i].text);
                    var PageActionHistory = (o_PageActionHistory[i].textContent || o_PageActionHistory[i].innerText || o_PageActionHistory[i].text);
                    var UpdatedOn = (o_UpdatedOn[i].textContent || o_UpdatedOn[i].innerText || o_UpdatedOn[i].text);
                    var IPAddress = (o_IPAddress[i].textContent || o_IPAddress[i].innerText || o_IPAddress[i].text);

                    sno = sno + 1;

                    row = document.createElement('tr');
                    AddCell(row, sno, "siteOverview_Box_UserLog", "", "", "left", "50px", "30px", "");
                    AddCell(row, UpdatedOn, "siteOverview_Box_UserLog", "", "", "left", "140px", "30px", "");
                    AddCell(row, UserName, "siteOverview_Box_UserLog", "", "", "left", "100px", "30px", "");
                    AddCell(row, PageAction, "siteOverview_Box_UserLog", "", "", "left", "60px", "30px", "");
                    AddCell(row, PageActionHistory, "siteOverview_Box_UserLog", "", "", "left", "440px", "30px", "");
                    AddCell(row, IPAddress, "siteOverview_Box_UserLog", "", "", "left", "100px", "30px", "");
                    body.appendChild(row);
               }

               sTbl.appendChild(body);

               DatatablesLoadGraph("#tblUserActivityInfo", [8], 0, "asc", 10);
            }
            else 
            {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);               
            }
           
            document.getElementById("divLoading").style.display = "none";          
        } 
    }
}

function OpenUsersDialog() {

    document.getElementById("ctl00_ContentPlaceHolder1_selSite").selectedIndex = 0;

    //Open Dialog
    $("#divExportUsers").dialog({
        height: 200,
        width: 600,
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
 
//Open Password Dialog
function OpenChangePasswordDialog()
{
    var winWidth = 600;
    var winHeight = 250;

    g_DialogView = "dialog-ChangePassword";  

    $('#lblAddPasswordMsg').html('');
    document.getElementById("txtOldPassword").value = "";
    document.getElementById("txtNewPassword").value = "";
    document.getElementById("txtConfirmPassword").value = "";
    
    //Open Dialog
    $( "#dialog-ChangePassword" ).dialog({
        title: "Change Password",
        height: winHeight,
        width: winWidth,
        modal: true,
        show: {
            effect: "fade",
            duration: 500
        },
        hide: {
            effect: "fade",
            duration: 500
        },
        close: function(event) { 
           
        }
    });
}

function ChangePassword_ForSeletedUser() {  

	$('#lblAddPasswordMsg').html('');
        
	if (curUserid == "0") {
	    alert("Please Select User.");
	}
	else if(document.getElementById("txtOldPassword").value =="")
	{
	    alert("Please enter Old Password");
	    return;
	}
	else if(document.getElementById("txtNewPassword").value =="")
	{
	    alert("Please enter New Password");
	    return;
	}
	else if(document.getElementById("txtConfirmPassword").value =="")
	{
	    alert("Please re-enter New Password");
	    return;
	}
	else if(document.getElementById("txtNewPassword").value != document.getElementById("txtConfirmPassword").value)
	{
	    alert("New Password's didnt matched");
	    return;
	}                 
	else {
	    if (confirm("Are you sure do you want to the password has been reset?") == true) {
	        $("#divLoading_ChangePassword").show();
	        ChangePassword(curUserid);
	    }
	}
}

var g_Password_Obj;

function ChangePassword(UserId)
 {
    $.post("AjaxConnector.aspx?cmd=ChangePassword",
    {
        OldPassword: setundefined(document.getElementById("txtOldPassword").value),
        NewPassWord: setundefined(document.getElementById("txtNewPassword").value),
        UserId: UserId
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxChangePassword(dsRoot);
        }
        else {
            document.getElementById("divLoading_ChangePassword").style.display = "none";
        }
    });
}

function ajaxChangePassword(dsRoot) {
            
    $("#divLoading_ChangePassword").hide();

    var o_Result = dsRoot.getElementsByTagName('Result')
    var Result = getTagNameValue(o_Result[0]);

    var o_Error = dsRoot.getElementsByTagName('Error')
    var Error = getTagNameValue(o_Error[0]);
            
    if (Result == "0")
    {
        $('#lblAddPasswordMsg').removeClass('clsMapErrorTxt');
        $('#lblAddPasswordMsg').html('Password Changed Successfully ..!').addClass('clsMapSuccessTxt');
    }
    else
    {
        $('#lblAddPasswordMsg').removeClass('clsMapSuccessTxt');
        $('#lblAddPasswordMsg').html(Error).addClass('clsMapErrorTxt');
    }
}


