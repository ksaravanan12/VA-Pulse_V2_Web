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

var g_AddSite_Obj;
var g_IsAdd;

function AddSiteSetup(Masterid, siteId, IsAdd, CompanyId, SiteName, SiteFolder, FileFormat, Status, IsGroup,
                     IsGroupMember, ServerIP, AuthPassword, LocationCode, QBN, IsPrismView, TimeZone, IsUnDeleteTags, IsUnDeleteMonitors, 
                     IsDefinedTagsinCore, IsDefinedInfrastructureinCore) {

    g_IsAdd = IsAdd;

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=AddSiteSetup",
    {
        Masterid: Masterid,
        sid: siteId,
        isAdd: IsAdd,
        CompanyId: CompanyId,
        SiteName: SiteName,
        SiteFolder: SiteFolder,
        FileFormat: FileFormat,
        Status: Status,
        IsGroup: IsGroup,
        IsGroupMember: IsGroupMember,
        ServerIP: ServerIP,
        AuthPassword: AuthPassword,
        LocationCode: LocationCode,
        QBN: QBN,
        IsPrismView: IsPrismView,
	TimeZone: TimeZone,
	IsUnDeleteTags: IsUnDeleteTags,
	IsUnDeleteMonitors: IsUnDeleteMonitors,
	IsDefinedTagsinCore: IsDefinedTagsinCore,
	IsDefinedInfrastructureinCore: IsDefinedInfrastructureinCore
	
	
    },
    function (data, status) {

        if (status == "success") {

            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddSite(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function ajaxAddSite(dsRoot) {

    var o_Result = dsRoot.getElementsByTagName('Result');
    var o_Msg = dsRoot.getElementsByTagName('Msg');

    if (o_Result.length > 0) {

        var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);
        var Result = (o_Result[0].textContent || o_Result[0].innerText || o_Result[0].text);

        if (g_IsAdd == "1" || g_IsAdd == "2") {

            if (Result == "0") {

                document.getElementById("tdError").innerHTML = msg;
                doAfterAddedSite();
            }
            else {
                document.getElementById("tdError").innerHTML = msg;
            }
        }
        else {
            document.getElementById("tdAddUser").style.display = "none";
            doAfterAddedSite();
        }
    }
    else {
        document.getElementById("tdError").innerHTML = "Error in Add";
    }

    document.getElementById("divLoading").style.display = "none";
}

var g_Site_Obj;
var g_ExportSite = 0;

function Load_Setup_Site(isExportSites) {

    g_Site_Obj = CreateInfraXMLObj();

    g_ExportSite = isExportSites;

    if (g_Site_Obj != null) {

        g_Site_Obj.onreadystatechange = ajaxSiteList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=SiteList&isExportSites=" + isExportSites;

        if (GetBrowserType() == "isIE") {
            g_Site_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Site_Obj.open("GET", DbConnectorPath, true);
        }

        g_Site_Obj.send(null);
    }

    return false;
}

function ajaxSiteList() {

    if (g_Site_Obj.readyState == 4) {

        if (g_Site_Obj.status == 200) {

            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Site_Obj.responseXML.documentElement);

            var dsRoot = g_Site_Obj.responseXML.documentElement;

            if (g_ExportSite == 1) {
                return ExportSites(dsRoot);
            }     
                   
            var sTbl = document.getElementById("tblSiteListInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblSiteListInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblSiteListInfo');
            }

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            var o_SiteName = dsRoot.getElementsByTagName('SiteName');
            var o_Status = dsRoot.getElementsByTagName('Status');
            var o_ServerIP = dsRoot.getElementsByTagName('ServerIP');
            var o_CompanyName = dsRoot.getElementsByTagName('CompanyName');
            var o_SiteFolder = dsRoot.getElementsByTagName('SiteFolder');
            var o_LocationCode = dsRoot.getElementsByTagName('LocationCode');
            var o_SiteId = dsRoot.getElementsByTagName('SiteId');
            var o_CompanyId = dsRoot.getElementsByTagName('CompanyId');
            var o_FileFormat = dsRoot.getElementsByTagName('FileFormat');
            var o_IsGroup = dsRoot.getElementsByTagName('IsGroup');
            var o_IsGroupMember = dsRoot.getElementsByTagName('IsGroupMember');
            var o_QBN = dsRoot.getElementsByTagName('QBN');
            var o_IsPrismView = dsRoot.getElementsByTagName('IsPrismView');
            var o_IsUnDeleteTags = dsRoot.getElementsByTagName('IsUnDeleteTags');
            var o_IsUnDeleteMonitors = dsRoot.getElementsByTagName('IsUnDeleteMonitors');
            var o_PCVersion = dsRoot.getElementsByTagName('PCVersion');
            var o_TimeZone = dsRoot.getElementsByTagName('TimeZone');
            var o_IsLTCSite = dsRoot.getElementsByTagName('IsLTCSite');
            var o_DefinedTagsinCore = dsRoot.getElementsByTagName('DefinedTagsinCore');
            var o_DefinedInfrastructureinCore = dsRoot.getElementsByTagName('DefinedInfrastructureinCore');

            nRootLength = o_SiteId.length;

            if (nRootLength > 0) {
                var head = "";

                head = "<thead>" +
                           "<tr>" +
                               "<th class='siteOverview_TopLeft_Box'>Site Id</th>" +
                               "<th class='siteOverview_Box'>Site Name</th>" +
                               "<th class='siteOverview_Box'>Site Type</th>" +
                               "<th class='siteOverview_Box'>Core Version</th>" +                   
                               "<th class='siteOverview_Box'>Company Name</th>" +
                               "<th class='siteOverview_Box'>Site Folder</th>" +
                               "<th class='siteOverview_Box'>Time Zone</th>" +
                               "<th class='siteOverview_Box'>Status</th>" + 
			       "<th class='siteOverview_Box'>Edit</th>" +                           
                           "</tr>" +
                       "</thead>";     
                        
                sTbl.innerHTML = head;
              
                body = document.createElement('tbody');

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');
                    var SiteName =setundefined((o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text));
                    var Status = setundefined((o_Status[i].textContent || o_Status[i].innerText || o_Status[i].text));
                    var ServerIP = setundefined((o_ServerIP[i].textContent || o_ServerIP[i].innerText || o_ServerIP[i].text));
                    var CompanyName = setundefined((o_CompanyName[i].textContent || o_CompanyName[i].innerText || o_CompanyName[i].text));
                    var SiteFolder = setundefined((o_SiteFolder[i].textContent || o_SiteFolder[i].innerText || o_SiteFolder[i].text));                  
                    var LocationCode = setundefined((o_LocationCode[i].textContent || o_LocationCode[i].innerText || o_LocationCode[i].text));
                    var SiteId = setundefined((o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text));
                    var CompanyId = setundefined((o_CompanyId[i].textContent || o_CompanyId[i].innerText || o_CompanyId[i].text));
                    var FileFormat = setundefined((o_FileFormat[i].textContent || o_FileFormat[i].innerText || o_FileFormat[i].text));
                    var IsGroup = setundefined((o_IsGroup[i].textContent || o_IsGroup[i].innerText || o_IsGroup[i].text));
                    var IsGroupMember = setundefined((o_IsGroupMember[i].textContent || o_IsGroupMember[i].innerText || o_IsGroupMember[i].text));
                    var QBN = setundefined((o_QBN[i].textContent || o_QBN[i].innerText || o_QBN[i].text));
                    var IsPrismView = setundefined((o_IsPrismView[i].textContent || o_IsPrismView[i].innerText || o_IsPrismView[i].text));
                    var IsUnDeleteTags = setundefined((o_IsUnDeleteTags[i].textContent || o_IsUnDeleteTags[i].innerText || o_IsUnDeleteTags[i].text));
                    var IsUnDeleteMonitors = setundefined((o_IsUnDeleteMonitors[i].textContent || o_IsUnDeleteMonitors[i].innerText || o_IsUnDeleteMonitors[i].text));
                    var PCVersion = setundefined((o_PCVersion[i].textContent || o_PCVersion[i].innerText || o_PCVersion[i].text));
                    var TimeZone = setundefined((o_TimeZone[i].textContent || o_TimeZone[i].innerText || o_TimeZone[i].text));
                    var IsLTCSite = setundefined((o_IsLTCSite[i].textContent || o_IsLTCSite[i].innerText || o_IsLTCSite[i].text));
                    var DefinedTagsinCore = setundefined((o_DefinedTagsinCore[i].textContent || o_DefinedTagsinCore[i].innerText || o_DefinedTagsinCore[i].text));
                    var DefinedInfrastructureinCore = setundefined((o_DefinedInfrastructureinCore[i].textContent || o_DefinedInfrastructureinCore[i].innerText || o_DefinedInfrastructureinCore[i].text));

                    var imgStatus = "";

                    if (Status == "True") {
                       Status = "Active"
                       imgStatus = "<img style='width: 24px;' src='images/green-button_24x24.png' alt='Enabled' title='Enabled' />"
                    }
                    else if (Status == "False") {
                       Status = "Inactive"
                       imgStatus = "<img style='width: 24px;' src='images/gray-button_24x24.png' alt='Disabled' title='Disabled' />"
                    }

                    AddCell(row, SiteId, 'siteOverview_Box_UserLog', "", "", "left", "50px", "30px", "");
                    AddCell(row, SiteName, 'siteOverview_Box_UserLog', "", "", "left", "300px", "30px", "");
                    
                    if (IsLTCSite == "1")
                       AddCell(row, "LTC", 'siteOverview_Box_UserLog', "", "", "left", "50px", "30px", "");
                    else
                       AddCell(row, FileFormat, 'siteOverview_Box_UserLog', "", "", "left", "50px", "30px", "");

                    AddCell(row, PCVersion, 'siteOverview_Box_UserLog', "", "", "left", "220px", "30px", "");                                  
                    AddCell(row, CompanyName, 'siteOverview_Box_UserLog', "", "", "left", "100px", "30px", "");
                    AddCell(row, SiteFolder, 'siteOverview_Box_UserLog', "", "", "left", "180px", "30px", "");
                    AddCell(row, TimeZone, 'siteOverview_Box_UserLog', "", "", "left", "220px", "30px", "");
                    AddCell(row, imgStatus, "siteOverview_Box_wrap", "", "", "center", "", "30px", "8px");
                    AddCell(row, "<img id='editConfig-" + i + "' src='images/imgEdit.png' onclick=showSiteConfigurationForEdit(" + SiteId + ",\&quot;" + encodeURIComponent(SiteName) + "\&quot,'" + Status + "','" + ServerIP + "',\&quot;" + encodeURIComponent(SiteFolder) + "\&quot,\&quot;" + encodeURIComponent(LocationCode) + "\&quot,'" + FileFormat + "','" + IsGroup + "','" + IsGroupMember + "',\&quot;" + encodeURIComponent(QBN) + "\&quot," + CompanyId + ",'" + IsPrismView + "',\&quot;" + encodeURIComponent(TimeZone) + "\&quot,'" + IsUnDeleteTags + "','" + IsUnDeleteMonitors + "','" + DefinedTagsinCore + "','" + DefinedInfrastructureinCore + "'); onmouseover='btnPrev_LAServiceOver(this);' onmouseout='btnPrev_LAServiceOut(this);' />", "siteOverview_Box_wrap", "", "", "center", "50px", "30px", "8px");
                    body.appendChild(row);
                } 

                sTbl.appendChild(body);

                DatatablesLoadGraph_New("#tblSiteListInfo", [8], 1, "asc", 20); 
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

function ExportSites(Excel_Root) {

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

var g_DelObj;

function DeleteConfigurationSetting(siteId, AuthUserId) {

    g_DelObj = CreateInfraXMLObj();

    if (g_DelObj != null) {

        g_DelObj.onreadystatechange = AfterDeleteAjax;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=DeleteSiteConfiguration&sid=" + siteId + "&AuthUserId=" + AuthUserId;

        if (GetBrowserType() == "isIE") {
            g_DelObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_DelObj.open("GET", DbConnectorPath, true);
        }

        g_DelObj.send(null);
    }

    return false;
}

function AfterDeleteAjax() {

    if (g_DelObj.readyState == 4) {

        if (g_DelObj.status == 200) {

            doAfterAddedSite();
        } 
    } 
}

var setupSupport_obj;

function AddSupportSetup(Masterid, supportId, supportName, IsAdd, Status) {

    setupSupport_obj = CreateInfraXMLObj();

    if (setupSupport_obj != null) {

        setupSupport_obj.onreadystatechange = ajaxAddSupport;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=AddSupportSetup&Masterid=" + Masterid + "&SiteName=" + supportName + "&sid=" + supportId + "&isAdd=" + IsAdd + "&Status=" + Status;

        if (GetBrowserType() == "isIE") {
            setupSupport_obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            setupSupport_obj.open("GET", DbConnectorPath, true);
        }
	
        setupSupport_obj.send(null);
    }
    
    return false;
}

function ajaxAddSupport() {

    if (setupSupport_obj.readyState == 4) {
    
        if (setupSupport_obj.status == 200) {

            var dsRoot = setupSupport_obj.responseXML.documentElement;
      
            AjaxMsgReceiver(dsRoot);

            var o_Msg = dsRoot.getElementsByTagName('Msg');
            var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);
            
            if (msg == "" || msg == null)
                msg = "Added Successfully.";

            if (msg == "Name already exist!") {
                document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").value = msg; 
                doAfterAddedSupport();
            }           
            else {
                document.getElementById("tdAddsupport").style.display = "none";
                doAfterAddedSupport();
            }         
        } 
    }
}

var Support_obj;

function Load_Setup_Support(siteId) {

    Support_obj = CreateInfraXMLObj();

    if (Support_obj != null) {

        Support_obj.onreadystatechange = ajaxSupportList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=SupportList&Status=" + siteId ;

        if (GetBrowserType() == "isIE") {
            Support_obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            Support_obj.open("GET", DbConnectorPath, true);
        }
        Support_obj.send(null);
    }
    
    return false;
}

function ajaxSupportList() {

    if (Support_obj.readyState == 4) {
    
        if (Support_obj.status == 200) {
	
            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(Support_obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblSupportInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblSupportInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblSupportInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);
            var dsRoot = Support_obj.responseXML.documentElement;

            var o_SiteName = dsRoot.getElementsByTagName('Name')
            var o_Url = dsRoot.getElementsByTagName('Url')
            var o_status = dsRoot.getElementsByTagName('Status')
            var o_CreatedOn = dsRoot.getElementsByTagName('Created')
            var o_ModifiedOn = dsRoot.getElementsByTagName('Modified')
            var o_SupportId = dsRoot.getElementsByTagName('DataId')

            nRootLength = o_SiteName.length;

            var statusTag = document.getElementById("ddlsupportstatus").value;

            if (nRootLength > 0) {

                var head = "";

                if (statusTag != "-1") {
                    head = "<thead><tr><th class='siteOverview_TopLeft_Box' style='padding-right: 17px; width:200px; height:30px;'>Name </th><th class='siteOverview_TopLeft_Box' style='width:120px;'>Created On</th><th class='siteOverview_TopLeft_Box' style='width:120px;'>Modified On</th><th class='siteOverview_TopLeft_Box' style='padding-right: 17px; width:30px;'>Browse</th></tr></thead>";
                }
                else {
                    head = "<thead><tr><th class='siteOverview_TopLeft_Box' style='padding-right: 17px; width:200px; height:30px;'>Name </th><th class='siteOverview_TopLeft_Box' style='width:60px;'>Status</th><th class='siteOverview_TopLeft_Box' style='width:120px;'>Created On</th><th class='siteOverview_TopLeft_Box' style='width:120px;'>Modified On</th><th class='siteOverview_TopLeft_Box' style='padding-right: 17px; width:30px;'>Browse</th></tr></thead>";
                }

                sTbl.innerHTML = head;

                body = document.createElement('tbody');

                var Statusv;

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');

                    var SupportName = setundefined((o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text));
                    var SupportUrl = setundefined((o_Url[i].textContent || o_Url[i].innerText || o_Url[i].text));
                    var Status = setundefined((o_status[i].textContent || o_status[i].innerText || o_status[i].text));
                    var CreatedOn = setundefined((o_CreatedOn[i].textContent || o_CreatedOn[i].innerText || o_CreatedOn[i].text));
                    var ModifiedOn = setundefined((o_ModifiedOn[i].textContent || o_ModifiedOn[i].innerText || o_ModifiedOn[i].text));
                    var SupportId = setundefined((o_SupportId[i].textContent || o_SupportId[i].innerText || o_SupportId[i].text));

                    if (Status == "True") {
                        Statusv = "Active"
                    }
                    else if (Status == "False") {
                        Statusv = "Inactive"
                    }

                    AddCell(row, "<a style='cursor:pointer;' onclick=showSupportConfigurationForEdit(" + SupportId + ",'" + SupportName.replace(new RegExp(" ", "gm"), "_") + "','" + SupportUrl.replace(new RegExp(" ", "gm"), "_") + "','" + Status + "'); >" + SupportName + "</a>", 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");

                    if (statusTag == "-1") {
                        AddCell(row, Statusv, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    }

                    AddCell(row, CreatedOn, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, ModifiedOn, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, "<a href='" + SupportUrl + "' target='_blank'><img id='editConfig-" + i + "' src='images/internet.png' /></a>", "siteOverview_Box_wrap", "", "", "center", "50px", "30px", "8px");

                    body.appendChild(row);
                }
                sTbl.appendChild(body);
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        }
    }
}

var Settings_obj;

function addSitesForSettings(SiteList,Status) {

    Settings_obj = CreateInfraXMLObj();
    
    document.getElementById("divLoading").style.display = "";
    
    if (Settings_obj != null) {

        Settings_obj.onreadystatechange = ajaxSiteListForSettings;

        DbConnectorPath = "AjaxConnector.aspx?cmd=AddSiteForSettings&Status=" + Status + "&SiteList=" + SiteList + "&FileFormat=2X";

        if (GetBrowserType() == "isIE") {
            Settings_obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            Settings_obj.open("GET", DbConnectorPath, true);
        }
	
        Settings_obj.send(null);
    }
    
    return false;
}

function ajaxSiteListForSettings() {

    if (Settings_obj.readyState == 4) {
    
        if (Settings_obj.status == 200) {

            var dsRoot = Settings_obj.responseXML.documentElement;

            AjaxMsgReceiver(dsRoot);

            var o_Msg = dsRoot.getElementsByTagName('Msg');
            var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);

            document.getElementById("divLoading").style.display = "none";

            if (msg == "" || msg == null)
                msg = "Added Successfully.";
        }

        document.getElementById("selSiteList").value = 0;
        $("#selSiteList").multipleSelect("refresh");

        getSitelistForsettings(1);

        $('input:radio[id=rdoOn]').prop('checked', true);
    }
}

var gSettings_obj;

function getSitelistForsettings(stat) {

    gSettings_obj = CreateInfraXMLObj();

    if (gSettings_obj != null) {
    
        document.getElementById("divLoading").style.display = "";
	
        gSettings_obj.onreadystatechange = ajaxGetSiteListForSettings;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetSiteForSettings&Status=" + stat + "&FileFormat=2X";

        if (GetBrowserType() == "isIE") {
            gSettings_obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            gSettings_obj.open("GET", DbConnectorPath, true);
        }
        gSettings_obj.send(null);
    }
    
    return false;
}

function ajaxGetSiteListForSettings() {

    if (gSettings_obj.readyState == 4) {
    
        if (gSettings_obj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(gSettings_obj.responseXML.documentElement);
            var dsRoot = gSettings_obj.responseXML.documentElement;

            var sTbl = document.getElementById("tblSites");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblSites');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblSites');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);

            var o_SiteID = dsRoot.getElementsByTagName('SiteID')
            var o_SiteName = dsRoot.getElementsByTagName('SiteName')
            var o_SiteStatus = dsRoot.getElementsByTagName('Status')
            nRootLength = o_SiteID.length;

            var SiteId = "";
            head = "<thead><tr><th class='siteOverview_TopLeft_Box' style='padding-right: 17px; width:50px; height : 30px;'>Site Id </th><th class='siteOverview_Box' style='width:300px;'>Site Name</th></tr></thead>";

            sTbl.innerHTML = head;

            body = document.createElement('tbody');

            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');

                    var Site = setundefined((o_SiteID[i].textContent || o_SiteID[i].innerText || o_SiteID[i].text))
                    var SiteName = setundefined((o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text));
                    var Status = setundefined((o_SiteStatus[i].textContent || o_SiteStatus[i].innerText || o_SiteStatus[i].text));

                    AddCell(row, Site, 'DeviceList_leftBox', "", "", "left", "30px", "40px", "");
                    AddCell(row, SiteName, 'siteOverview_cell', "", "", "left", "200px", "40px", "10px");

                    body.appendChild(row);
                }

                sTbl.appendChild(body);
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 2, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);
            }

            // To load in select list
            for (var i = 0; i < nRootLength; i++) {
                SiteId = SiteId + setundefined((o_SiteID[i].textContent || o_SiteID[i].innerText || o_SiteID[i].text)) + ',';
            }

            if (SiteId != '') {
                var valArr = SiteId.split(',');

                i = 0, size = valArr.length;

                for (i; i < size; i++) {

                    if (valArr[i] != '') {
                        $("#selSiteList option[value='" + valArr[i] + "']").attr("selected", 1);
                        $("#selSiteList").multipleSelect("refresh");
                    }
                }
            }

            document.getElementById("divLoading").style.display = "none";
        }
    }
}