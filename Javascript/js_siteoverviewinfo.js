// JScript File

//*******************************************************************
//	Function Name	:	CreateXMLObj
//	Input			:	None
//	Description		:	Create XML Object For Ajax call
//*******************************************************************
function CreateSiteOverviewXMLObj() {

    var obj = null;

    if (window.ActiveXObject) {
        try {
            obj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                obj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                obj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
        obj = new XMLHttpRequest();
        obj.overrideMimeType('text/xml');
    }

    return obj;
}

// Checking For Browser
function GetBrowserType() {

    var isIE = ((document.all) ? true : false); //for Internet Explorer
    var isFF = ((document.getElementById && !document.all) ? true : false); //for Mozilla Firefox
    
    if (!(window.ActiveXObject) && "ActiveXObject" in window) {
        return "isIE";
    }
    
    if (isIE) {
        return "isIE";
    }
    else if (isFF) {
        return "isFF";
    }
}

//*********************************************************
//	Function Name	:	LoaddoLoadSiteOverviewinforamtion
//	Input			:	siteid,typeId
//	Description		:	ajax call LoaddoLoadSiteOverviewinforamtion
//*********************************************************

var g_SOObj;
var g_Type = "";

function LoaddoLoadSiteOverviewinforamtion(siteid, typeId) {

    document.getElementById("lblDefinedinConnectCore").innerHTML = "";

    g_SOObj = CreateSiteOverviewXMLObj();

    if (setundefined(typeId) == "" || setundefined(typeId) == "undefined")
        typeId = "system"

    g_Type = typeId;

    if (g_SOObj != null) {

        g_SOObj.onreadystatechange = ajaxSiteOverviewList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=SiteOverview&sid=" + siteid + "&typId=" + typeId;

        if (GetBrowserType() == "isIE") {
            g_SOObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_SOObj.open("GET", DbConnectorPath, true);
        }

        g_SOObj.send(null);
    }

    return false;
}

//*********************************************************
//	Function Name	:	ajaxSiteOverviewList
//	Input			:	none
//	Description		:	Load Overview Datas from ajax Response
//*********************************************************
function ajaxSiteOverviewList() {

    if (g_SOObj.readyState == 4) {

        if (g_SOObj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_SOObj.responseXML.documentElement);

            var sTbl, sTblLen;
            sTbl = document.getElementById('siteOverview');
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            var sTbl2, sTblLen2;
            sTbl2 = document.getElementById('siteotherDevice');
            sTblLen2 = sTbl2.rows.length;
            clearTableRows(sTbl2, sTblLen2);

            var sTbl3, sTblLen3;
            sTbl3 = document.getElementById('siteEnvOverview');
            sTblLen3 = sTbl3.rows.length;
            clearTableRows(sTbl3, sTblLen3);

            var dsRoot = g_SOObj.responseXML.documentElement;

            var o_UserRole = dsRoot.getElementsByTagName('UserRole');
            var o_SiteName = dsRoot.getElementsByTagName('SiteName');
            var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
            var o_TypeId = dsRoot.getElementsByTagName('TypeId');
            var o_Type = dsRoot.getElementsByTagName('Type');
            var o_TypeImage = dsRoot.getElementsByTagName('TypeImage');
            var o_Good = dsRoot.getElementsByTagName('Good');
            var o_s90Days = dsRoot.getElementsByTagName('s90Days');
            var o_s30Days = dsRoot.getElementsByTagName('s30Days');
            var o_OfflineOther = dsRoot.getElementsByTagName('OfflineOther');
            var o_OfflineBatteryDepleted = dsRoot.getElementsByTagName('OfflineBatteryDepleted');
            var o_BatterySummary = dsRoot.getElementsByTagName('BatterySummary');
            var o_DefinedTagsinCore = dsRoot.getElementsByTagName('DefinedTagsinCore');
            var o_DefinedInfrastructureinCore = dsRoot.getElementsByTagName('DefinedInfrastructureinCore');
            var o_PulseUIId = dsRoot.getElementsByTagName('PulseUIId');
            var o_UW = dsRoot.getElementsByTagName('UW');
            var o_LBI = dsRoot.getElementsByTagName('LBI');

            nRootLength = o_SiteName.length;

            if (nRootLength > 0) {

                var UserRole = (o_UserRole[0].textContent || o_UserRole[0].innerText || o_UserRole[0].text);
                var SiteName = (o_SiteName[0].textContent || o_SiteName[0].innerText || o_SiteName[0].text);
                var DefinedTagsinCore = (o_DefinedTagsinCore[0].textContent || o_DefinedTagsinCore[0].innerText || o_DefinedTagsinCore[0].text);
                var DefinedInfrastructureinCore = (o_DefinedInfrastructureinCore[0].textContent || o_DefinedInfrastructureinCore[0].innerText || o_DefinedInfrastructureinCore[0].text);

                if (DefinedTagsinCore == "True" && DefinedInfrastructureinCore == "True") {

                    $("#trDefinedinConnectCore").show();
                    document.getElementById("lblDefinedinConnectCore").innerHTML = "Note: Tags and infrastructure must be defined in Connect Core to be created for this site.";
                }
                else if (DefinedTagsinCore == "True") {

                    $("#trDefinedinConnectCore").show();
                    document.getElementById("lblDefinedinConnectCore").innerHTML = "Note: Tags must be defined in Connect Core to be created for this site.";
                }
                else if (DefinedInfrastructureinCore == "True") {

                    $("#trDefinedinConnectCore").show();
                    document.getElementById("lblDefinedinConnectCore").innerHTML = "Note: Infrastructure must be defined in Connect Core to be created for this site.";
                }
                else
                    $("#trDefinedinConnectCore").hide();

                // Work Flow Tags
                if (g_Type == "tag" || g_Type == "system" || g_Type == "") {

                    row = document.createElement('tr');

                    AddCell(row, "Workflow Tags", 'siteOverview_TopLeft_Box', "", "", "center", "250px", "40px", "");
                    AddCell(row, "Good", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Less than <br /> 90 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Less than <br /> 30 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Offline <br /> (Battery Depleted)", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

                    if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance)
                        AddCell(row, "Offline <br /> (Other)", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

                    //#22423 - Remove access to Battery Summary view 
                    if (UserRole == enumUserRoleArr.Admin)
                        AddCell(row, "Battery <br /> Summary", 'siteOverview_Topright_Box', "", "", "center", "50px", "40px", "");

                    sTbl.appendChild(row);
                }

                // Environment Tags
                if (g_Type == "tag" || g_Type == "system" || g_Type == "") {

                    row = document.createElement('tr');
                    AddCell(row, "Environmental Tags", 'siteOverview_TopLeft_Box', "", "", "center", "250px", "40px", "");
                    AddCell(row, "Good", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Less than <br /> 90 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Less than <br /> 30 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Offline <br /> (Battery Depleted)", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

                    if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance)
                        AddCell(row, "Offline <br /> (Other)", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

                    //#22423 - Remove access to Battery Summary view   
                    if (UserRole == enumUserRoleArr.Admin)
                        AddCell(row, "Battery <br /> Summary", 'siteOverview_Topright_Box', "", "", "center", "50px", "40px", "");

                    sTbl3.appendChild(row);
                }

                // Infrastructures
                if (g_Type == "infrastructure" || g_Type == "system" || g_Type == "") {

                    row = document.createElement('tr');
                    AddCell(row, "Infrastructure", 'siteOverview_TopLeft_Box', "", "", "center", "250px", "40px", "");
                    AddCell(row, "Good", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Less than <br /> 90 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Less than <br /> 30 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Offline <br /> (Battery Depleted)", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                    AddCell(row, "Offline <br /> (Other)", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

                    //#22423 - Remove access to Battery Summary view 
                    if (UserRole == enumUserRoleArr.Admin)
                        AddCell(row, "Battery <br /> Summary", 'siteOverview_Topright_Box', "", "", "center", "50px", "40px", "");

                    sTbl2.appendChild(row);
                }

                //Datas
                for (var i = 0; i < nRootLength; i++) {

                    var DeviceType = (o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
                    var Type = (o_Type[i].textContent || o_Type[i].innerText || o_Type[i].text);
                    var TypeId = (o_TypeId[i].textContent || o_TypeId[i].innerText || o_TypeId[i].text);
                    var TypeImage = (o_TypeImage[i].textContent || o_TypeImage[i].innerText || o_TypeImage[i].text);
                    var Good = (o_Good[i].textContent || o_Good[i].innerText || o_Good[i].text);
                    var n90Days = (o_s90Days[i].textContent || o_s90Days[i].innerText || o_s90Days[i].text);
                    var n30Days = (o_s30Days[i].textContent || o_s30Days[i].innerText || o_s30Days[i].text);
                    var OfflineOther = (o_OfflineOther[i].textContent || o_OfflineOther[i].innerText || o_OfflineOther[i].text);
                    var OfflineBatteryDepleted = (o_OfflineBatteryDepleted[i].textContent || o_OfflineBatteryDepleted[i].innerText || o_OfflineBatteryDepleted[i].text);
                    var BatSum = (o_BatterySummary[i].textContent || o_BatterySummary[i].innerText || o_BatterySummary[i].text);
                    var PulseUIId = (o_PulseUIId[i].textContent || o_PulseUIId[i].innerText || o_PulseUIId[i].text);
                    var UW = (o_UW[i].textContent || o_UW[i].innerText || o_UW[i].text);
                    var LBI = (o_LBI[i].textContent || o_LBI[i].innerText || o_LBI[i].text);

                    if (DeviceType == "1" && (UserRole == enumUserRoleArr.Customer || UserRole == enumUserRoleArr.Maintenance))
                        OfflineOther = "-";

                    if (Good != "-" || n90Days != "-" || n30Days != "-" || OfflineOther != "-" || OfflineBatteryDepleted != "-") {

                        row = document.createElement('tr');

                        AddCell(row, TypeImage, 'DeviceList_Box', "", "", "left", "200px", "40px", "");
                        AddCell(row, Good, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                        if (UW == "true")
                            AddCell(row, n90Days, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                        else
                            AddCell(row, "N/A", 'siteOverview_cell cell_text_offline', "", "", "center", "100px", "40px", "");

                        if (LBI == "true")
                            AddCell(row, n30Days, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                        else
                            AddCell(row, "N/A", 'siteOverview_cell cell_text_offline', "", "", "center", "100px", "40px", "");

                        AddCell(row, OfflineBatteryDepleted, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                        if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance)
                            AddCell(row, OfflineOther, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                        else if (DeviceType != "1")
                            AddCell(row, OfflineOther, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                        //#22423 - Remove access to Battery Summary view
                        if (UserRole == enumUserRoleArr.Admin) {
                            if (DeviceType != "3")
                                AddCell(row, BatSum, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                            else
                                AddCell(row, "", 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                        }

                        if (PulseUIId == enumPulseUITable.EnvironmentalTags)
                            sTbl3.appendChild(row);
                        else if (PulseUIId == enumPulseUITable.WorkflowTags)
                            sTbl.appendChild(row);
                        else if (PulseUIId == enumPulseUITable.Infrastructure)
                            sTbl2.appendChild(row);
                    }
                }
            }

            document.getElementById("divLoading_SiteOverview").style.display = "none";
        }
    }
}

