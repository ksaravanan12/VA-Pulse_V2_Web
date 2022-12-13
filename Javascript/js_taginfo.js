// JScript File
var TAG_SortOrder = '';
var TAG_SortColumn = '';
var TAG_SortImg = '';

TAG_SortColumn = "LastSeen";
TAG_SortOrder = "desc";
TAG_SortImg = "<image src='Images/downarrow.png' valign='middle' />";

function CreateTagXMLObj() {

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

function doLoadTag(siteid,alertId,bin,curpag,typeId){
    LoadTaginforamtion(siteid, alertId, bin, curpag, typeId);  
}

var g_TagObj;
var g_Bin = 0;
var TagTypeId;

function LoadTaginforamtion(siteid, alertId, bin, curpag, typeId) {

    g_TagObj = CreateTagXMLObj();

    g_Bin = bin;
    TagTypeId = typeId;

    if (g_TagObj != null) {

        g_TagObj.onreadystatechange = ajaxTagList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=TagList&sid=" + siteid + "&alertId=" + alertId + "&Bin=" + bin + "&curpage=" + curpag + "&typId=" + typeId + "&sorColumnname=" + TAG_SortColumn + "&SorOrder=" + TAG_SortOrder;

        if (GetBrowserType() == "isIE") {
            g_TagObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_TagObj.open("GET", DbConnectorPath, true);
        }
        g_TagObj.send(null);
    }
    return false;
}

var g_TagRoot;

function ajaxTagList() {
    if (g_TagObj.readyState == 4) {
        if (g_TagObj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_TagObj.responseXML.documentElement);
            g_TagRoot = g_TagObj.responseXML.documentElement;

            loadTagList(0);

            g_TagAllDeviceRoot = g_TagRoot;
        }
    }
}

var g_AnnualTags = "";

function loadTagList(isSearch) {

    var sTbl, sTblLen;
    var sSpanCtrlId = "";
    var Last20WeekDateArr = new Array();
    var arrSplitWeekData = new Array();

    if (isSearch == 1)
        sTbl = document.getElementById('tblTagInfoSearch');
    else
        sTbl = document.getElementById('tblTagInfo');

    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);
    
    var o_UserRole = g_TagRoot.getElementsByTagName('UserRole');
    var o_siteId = g_TagRoot.getElementsByTagName('SiteID');
    var o_SiteName = g_TagRoot.getElementsByTagName('Sitename');
    var o_TotalPage = g_TagRoot.getElementsByTagName('TotalPage');
    var o_TotalCount = g_TagRoot.getElementsByTagName('TotalCount');
    var o_TagId = g_TagRoot.getElementsByTagName('TagId');
    var o_TagType = g_TagRoot.getElementsByTagName('TagType');
    var o_MonitorId = g_TagRoot.getElementsByTagName('MonitorId');
    var o_IRID = g_TagRoot.getElementsByTagName('IRID');
    var o_LessThen90Days = g_TagRoot.getElementsByTagName('LessThen90Days');
    var o_LessThen30Days = g_TagRoot.getElementsByTagName('LessThen30Days');
    var o_CatastrophicCases = g_TagRoot.getElementsByTagName('CatastrophicCases');
    var o_offline = g_TagRoot.getElementsByTagName('offline');
    var o_BatteryReplacementOn = g_TagRoot.getElementsByTagName('BatteryReplacementOn');
    var o_LastSeenMonitor = g_TagRoot.getElementsByTagName('MonitorLastSeen');
    var o_LastSeenNetwork = g_TagRoot.getElementsByTagName('LastSeen');
    var o_MonitorLocation = g_TagRoot.getElementsByTagName('MonitorLocation');
    var o_StarAddress = g_TagRoot.getElementsByTagName('StarAddress');
    var o_TagRecalibrationDate = g_TagRoot.getElementsByTagName('TagRecalibrationDate');
    var o_IsAnnualCalibration = g_TagRoot.getElementsByTagName('IsAnnualCalibration');
    var o_AnnualTags = g_TagRoot.getElementsByTagName('AnnualTags');
    var o_CertificateDate = g_TagRoot.getElementsByTagName('CertificateDate');
    var o_MFRCalibrationDue = g_TagRoot.getElementsByTagName('MFRCalibrationDue');
    var o_ProbeId = g_TagRoot.getElementsByTagName('ProbeId');
    var o_Probe1Temperature = g_TagRoot.getElementsByTagName('Probe1Temperature');
    var o_Probe2Temperature = g_TagRoot.getElementsByTagName('Probe2Temperature');
    var o_AvgRSSI = g_TagRoot.getElementsByTagName('AvgRSSI');
    var o_ModelItem = g_TagRoot.getElementsByTagName('ModelItem');
    var o_DeviceSubTypeId = g_TagRoot.getElementsByTagName('DeviceSubTypeId');
    var o_LocalId = g_TagRoot.getElementsByTagName('LocalId');
    var o_ProbeId2 = g_TagRoot.getElementsByTagName('ProbeId2');
    var o_Location = g_TagRoot.getElementsByTagName('Location');
    var o_Location = g_TagRoot.getElementsByTagName('Location');
    var o_ClientCalDue = g_TagRoot.getElementsByTagName('ClientCalDue');
    var o_CalFrequency = g_TagRoot.getElementsByTagName('CalFrequency');
    var o_Image = g_TagRoot.getElementsByTagName('Image');
    var o_NISTFile = g_TagRoot.getElementsByTagName('NISTFile');
    var o_Probe1NISTFile = g_TagRoot.getElementsByTagName('Probe1NISTFile');
    var o_Probe2NISTFile = g_TagRoot.getElementsByTagName('Probe2NISTFile');
    var sWebUrlPath = window.location.href.split('#')[0].replace('Home.aspx', '') + "Certificate/";
    var o_ADCValue = g_TagRoot.getElementsByTagName('Voltage');
    var o_BatteryCapacity = g_TagRoot.getElementsByTagName('BatteryCapacity');
    var o_CCatastrophicCases = g_TagRoot.getElementsByTagName('CCatastrophicCases');
    var o_Version = g_TagRoot.getElementsByTagName('Version');
    var o_P1Units = g_TagRoot.getElementsByTagName('P1Units');
    var o_P2Units = g_TagRoot.getElementsByTagName('P2Units');
    var o_Last20WeekData = g_TagRoot.getElementsByTagName('Last20WeekData');
    var o_CActivityLevel = g_TagRoot.getElementsByTagName('CActivityLevel');
    var o_isUHFTags = g_TagRoot.getElementsByTagName('isUHFTags');
    var o_NewDeviceSubTypeId = g_TagRoot.getElementsByTagName('NewDeviceSubTypeId');

    var hidval = 0;

    if (isSearch == 0) {
        document.getElementById("btnExportExcel").disabled = false;
        hidval = document.getElementById('hid_Show').value;
    }
    else {
        hidval = document.getElementById('hdn_Search_Show').value;
    }

    nRootLength = o_siteId.length;

    if (isSearch == 1) {
        document.getElementById("btnExportSearch").style.display = "none";

        if (nRootLength > 0)
            document.getElementById("btnExportSearch").style.display = "";
    }

    if (nRootLength == 0) {

        if (isSearch == 0)
            document.getElementById("trDeviceListRow").style.display = "";
        else
            document.getElementById("trDeviceListRow").style.display = "none";

        hideRecalibration();

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);

        document.getElementById("divLoading").style.display = "none";
        document.getElementById("btnExportExcel").disabled = true;

        return;
    }

    //User Role
    var UserRole = (o_UserRole[0].textContent || o_UserRole[0].innerText || o_UserRole[0].text);

    if (isSearch == 0) {

        //totalcount
        var ttcnt_lable = document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount")
        ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

        //Totalpage
        var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
        ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

        var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
        doTagEnableButton(MaxPageCnt);
    }

    var isUHFTags; 

    if (nRootLength > 0) {

        if (isSearch == 1 && g_Bin == "") {
            TagTypeId = (o_NewDeviceSubTypeId[0].textContent || o_NewDeviceSubTypeId[0].innerText || o_NewDeviceSubTypeId[0].text);
            typeId = TagTypeId;
        }

        if (isSearch == 1)
            isUHFTags = "True";
        else
            isUHFTags = setundefined((o_isUHFTags[0].textContent || o_isUHFTags[0].innerText || o_isUHFTags[0].text));

        ShowRecalibration();

        if (isSearch != 1 && document.getElementById("txtSearchDeviceIds").value == "")
            document.getElementById("trDeviceListRow").style.display = "";

        document.getElementById("btnSearchDeviceList").style.display = "";
        document.getElementById("btnExportExcel").disabled = false;

        row = document.createElement('tr');

        var IsEMSec = IsEMTag(TagTypeId);

        //EM Section 
        if (IsEMSec == "1") {

            if ((UserRole == enumUserRoleArr.Customer || UserRole == enumUserRoleArr.Maintenance) && g_IsTempMonitoring != "True") {                
                $('#tblTagInfo').css('width', '100%');
                $('#tblTagInfoSearch').css('width', '100%');
            }
            else {
                $('#tblTagInfo').css('width', '150%');
                $('#tblTagInfoSearch').css('width', '150%');
            }

            if (hidval == 1) {
                var chk_Allbox = "<input type='checkbox' id='chkAll' name='chkAll' style='vertical-align: middle;'>";

                AddCell(row, chk_Allbox, "siteOverview_TopLeft_Box", "", "center", "20px", "40px", "");
                AddCellForSorting(row, "TAG ID", 'siteOverview_Box', "", "", "center", "100px", "40px", "", "TagId", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            }
            else
                AddCellForSorting(row, "TAG ID", 'siteOverview_TopLeft_Box', "", "", "center", "100px", "40px", "", "TagId", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);

            AddCellForSorting(row, "P1 ID", 'siteOverview_BlueBox', "", "", "center", "200px", "40px", "", "InvProbeId", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            AddCellForSorting(row, "P2 ID", 'siteOverview_BlueBox', "", "", "center", "200px", "40px", "", "InvProbeId2", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            
			if (g_Bin == 0 || g_Bin === "") 
                AddCell(row, "Good", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
            
            if (g_Bin == 1 || g_Bin === "") 
                AddCell(row, "Less than <br /> 90 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            
            if (g_Bin == 2 || g_Bin === "") 
                AddCell(row, "Less than <br /> 30 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");                      

            AddCellForSorting(row, "Local ID", 'siteOverview_Box', "", "", "center", "350px", "40px", "", "LocalId", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            AddCellForSorting(row, "Cal Frequency", 'siteOverview_Box', "", "", "center", "100px", "40px", "", "CalFrequency", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            AddCellForSorting(row, "Last Cal", 'siteOverview_Box', "", "", "center", "150px", "40px", "", "InvCertifyDate", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);

            AddCellForSorting(row, "CenTrak Cal Due", 'siteOverview_Box', "", "", "center", "150px", "40px", "", "CenTrakCalDate", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            AddCell(row, "NIST Certificate", 'siteOverview_Box', "", "", "center", "120px", "40px", "");
            AddCellForSorting(row, "Client Cal Due", 'siteOverview_Box', "", "", "center", "150px", "40px", "", "ClientCalDue", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);

            if (g_Bin === "") {
                if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance && UserRole != enumUserRoleArr.MaintenancePrism)
                    AddCellForSorting(row, "Star Address", 'siteOverview_Box', "", "", "center", "120px", "40px", "", "LockedStarMacId", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            }

            AddCellForSorting(row, "Last Seen", 'siteOverview_Box', "", "", "center", "150px", "40px", "", "LastSeen", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            AddCellForSorting(row, "Model Item", 'siteOverview_Box', "", "", "center", "250px", "40px", "", "ModelItem", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            AddCell(row, "Images", 'siteOverview_Box', "", "", "center", "120px", "40px", "");
            AddCellForSorting(row, "Location", 'siteOverview_Box', "", "", "center", "350px", "40px", "", "CLocation", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);

            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {

                AddCellForSorting(row, "Voltage", 'siteOverview_Box', "", "", "center", "200px", "40px", "", "LBIValue", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
                AddCellForSorting(row, "% Batt", 'siteOverview_Box', "", "", "center", "200px", "40px", "", "BatteryCapacity", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            }

            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || g_IsTempMonitoring == "True" || UserRole == enumUserRoleArr.Support) {

                AddCellForSorting(row, "P1 Value", 'siteOverview_Box', "", "", "center", "200px", "40px", "", "RawTemp1", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
                AddCell(row, "P1 Units", 'siteOverview_Box', "", "", "center", "200px", "40px", "");
                AddCellForSorting(row, "P2 Value", 'siteOverview_Box', "", "", "center", "200px", "40px", "", "RawTemp2", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
                AddCell(row, "P2 Units", 'siteOverview_Box', "", "", "center", "200px", "40px", "")

                AddCellForSorting(row, "Avg RSSI", 'siteOverview_Topright_Box', "", "", "center", "200px", "40px", "", "AvgRSSI", TAG_SortColumn, TAG_SortImg, enumSortingArr.TagView);
            }
        }
        else {

            $('#tblTagInfo').css('width', '100%');
            $('#tblTagInfoSearch').css('width', '100%');

            if (hidval == 1) {
                var chk_Allbox = "<input type='checkbox' id='chkAll' name='chkAll' style='vertical-align: middle;'>";

                AddCell(row, chk_Allbox, "siteOverview_TopLeft_Box", "", "center", "20px", "40px", "");
                AddCell(row, "TAG ID", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
            }
            else {
                AddCell(row, "TAG ID", 'siteOverview_TopLeft_Box', "", "", "center", "200px", "40px", "");
            }

            AddCell(row, "Monitor Location", 'siteOverview_Box', "", "", "center", "150px", "40px", "");
            AddCell(row, "Monitor ID", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

            if (g_Bin === "") {
                if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance && UserRole != enumUserRoleArr.MaintenancePrism)
                    AddCell(row, "Star Address", 'siteOverview_Box', "", "", "center", "120px", "40px", "");
            }

            if (g_Bin == 0 || g_Bin === "")
                AddCell(row, "Good", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            
            if (g_Bin == 1 || g_Bin === "")
                AddCell(row, "Less than <br /> 90 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            
            if (g_Bin == 2 || g_Bin === "")
                AddCell(row, "Less than <br /> 30 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");

            if (g_Bin == 3 || g_Bin === "" || g_Bin == 7) 
                AddCell(row, "Date Last seen</br>by Monitor", "siteOverview_Box", "", "", "center", "150px", "40px", "");          
            
            AddCell(row, "Date Last seen</br>by Network", "siteOverview_Box", "", "", "center", "150px", "40px", "");
            AddCell(row, "Model Item", "siteOverview_Box", "", "", "center", "150px", "40px", "");

            if (g_Bin == 3 || g_Bin === "") {

                if (UserRole == enumUserRoleArr.Customer || UserRole == enumUserRoleArr.Maintenance) 
                   AddCell(row, "Offline (Battery Depleted)", "siteOverview_Box", "", "", "center", "100px", "40px", "");
                else
                   AddCell(row, "Offline", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            }           

            if (g_Bin == 7)
               AddCell(row, "Offline (Battery Depleted)", "siteOverview_Box", "", "", "center", "100px", "40px", ""); 

            if (g_Bin === "")            
                AddCell(row, "Battery Replaced On", 'siteOverview_Box', "", "", "center", "100px", "40px", "");            
            
            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Support) {
                AddCell(row, "LBI Activity", "siteOverview_Box", "", "", "center", "100px", "40px", "");
                AddCell(row, "Data", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            }

            if (g_Bin === "" && isUHFTags == "True") {
                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {
                    AddCell(row, "Cumulative <br> Activity Level", "siteOverview_Topright_Box", "", "", "center", "100px", "40px", "");                 
                }
            }
        }

        sTbl.appendChild(row);

        for (var i = 0; i < nRootLength; i++) {

            var siteid = (o_siteId[i].textContent || o_siteId[i].innerText || o_siteId[i].text);
            var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);
            var TotalPage = (o_TotalPage[i].textContent || o_TotalPage[i].innerText || o_TotalPage[i].text);
            var TotalCount = (o_TotalCount[i].textContent || o_TotalCount[i].innerText || o_TotalCount[i].text);
            var TagId = (o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
            var TagType = (o_TagType[i].textContent || o_TagType[i].innerText || o_TagType[i].text);
            var MonitorId = (o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text);
            var IRID = (o_IRID[i].textContent || o_IRID[i].innerText || o_IRID[i].text);
            var LessThen90Days = (o_LessThen90Days[i].textContent || o_LessThen90Days[i].innerText || o_LessThen90Days[i].text);
            var LessThen30Days = (o_LessThen30Days[i].textContent || o_LessThen30Days[i].innerText || o_LessThen30Days[i].text);
            var CatastrophicCases = (o_CatastrophicCases[i].textContent || o_CatastrophicCases[i].innerText || o_CatastrophicCases[i].text);
            var offline = (o_offline[i].textContent || o_offline[i].innerText || o_offline[i].text);
            var BatteryReplacementOn = (o_BatteryReplacementOn[i].textContent || o_BatteryReplacementOn[i].innerText || o_BatteryReplacementOn[i].text);
            var LastSeenMonitor = (o_LastSeenMonitor[i].textContent || o_LastSeenMonitor[i].innerText || o_LastSeenMonitor[i].text);
            var LastSeenNetwork = (o_LastSeenNetwork[i].textContent || o_LastSeenNetwork[i].innerText || o_LastSeenNetwork[i].text);
            var MonitorLocation = (o_MonitorLocation[i].textContent || o_MonitorLocation[i].innerText || o_MonitorLocation[i].text);
            var StarAddress = (o_StarAddress[i].textContent || o_StarAddress[i].innerText || o_StarAddress[i].text);
            var TagRecalibrationDate = (o_TagRecalibrationDate[i].textContent || o_TagRecalibrationDate[i].innerText || o_TagRecalibrationDate[i].text);
            var AnnualCalibration = setundefined((o_IsAnnualCalibration[i].textContent || o_IsAnnualCalibration[i].innerText || o_IsAnnualCalibration[i].text));
            var AnnualTags = setundefined((o_AnnualTags[i].textContent || o_AnnualTags[i].innerText || o_AnnualTags[i].text));
            var Probe1Temperature = setundefined((o_Probe1Temperature[i].textContent || o_Probe1Temperature[i].innerText || o_Probe1Temperature[i].text));
            var Probe2Temperature = setundefined((o_Probe2Temperature[i].textContent || o_Probe2Temperature[i].innerText || o_Probe2Temperature[i].text));
            var AvgRSSI = setundefined((o_AvgRSSI[i].textContent || o_AvgRSSI[i].innerText || o_AvgRSSI[i].text));
            var ModelItem = setundefined((o_ModelItem[i].textContent || o_ModelItem[i].innerText || o_ModelItem[i].text));
            var LocalId = setundefined((o_LocalId[i].textContent || o_LocalId[i].innerText || o_LocalId[i].text));
            var ProbeId2 = setundefined((o_ProbeId2[i].textContent || o_ProbeId2[i].innerText || o_ProbeId2[i].text));
            var Location = setundefined((o_Location[i].textContent || o_Location[i].innerText || o_Location[i].text));
            var ClientCalDue = setundefined((o_ClientCalDue[i].textContent || o_ClientCalDue[i].innerText || o_ClientCalDue[i].text));
            var CalFrequency = setundefined((o_CalFrequency[i].textContent || o_CalFrequency[i].innerText || o_CalFrequency[i].text));
            var Image = setundefined((o_Image[i].textContent || o_Image[i].innerText || o_Image[i].text));
            var ADCValue = setundefined((o_ADCValue[i].textContent || o_ADCValue[i].innerText || o_ADCValue[i].text));
            var BatteryCapacity = setundefined((o_BatteryCapacity[i].textContent || o_BatteryCapacity[i].innerText || o_BatteryCapacity[i].text));
            var CCatastrophicCases = setundefined((o_CCatastrophicCases[i].textContent || o_CCatastrophicCases[i].innerText || o_CCatastrophicCases[i].text));
            var Version = setundefined((o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text));
            var NISTFile = setundefined((o_NISTFile[i].textContent || o_NISTFile[i].innerText || o_NISTFile[i].text));
            var Probe1NISTFile = setundefined((o_Probe1NISTFile[i].textContent || o_Probe1NISTFile[i].innerText || o_Probe1NISTFile[i].text));
            var Probe2NISTFile = setundefined((o_Probe2NISTFile[i].textContent || o_Probe2NISTFile[i].innerText || o_Probe2NISTFile[i].text));
            var P1Units = setundefined((o_P1Units[i].textContent || o_P1Units[i].innerText || o_P1Units[i].text));
            var P2Units = setundefined((o_P2Units[i].textContent || o_P2Units[i].innerText || o_P2Units[i].text));
            var Last20WeekData = setundefined((o_Last20WeekData[i].textContent || o_Last20WeekData[i].innerText || o_Last20WeekData[i].text));
            var CActivityLevel = setundefined((o_CActivityLevel[i].textContent || o_CActivityLevel[i].innerText || o_CActivityLevel[i].text));
            var NewDeviceSubTypeId = setundefined((o_NewDeviceSubTypeId[i].textContent || o_NewDeviceSubTypeId[i].innerText || o_NewDeviceSubTypeId[i].text));
            var OldDeviceSubTypeId = setundefined((o_DeviceSubTypeId[i].textContent || o_DeviceSubTypeId[i].innerText || o_DeviceSubTypeId[i].text));

            var CertificateDate = "";
            var MFRCalibrationDue = "";
            var ProbeId = "";

            if (o_CertificateDate.length > 0)
                CertificateDate = setundefined((o_CertificateDate[i].textContent || o_CertificateDate[i].innerText || o_CertificateDate[i].text));

            if (o_MFRCalibrationDue.length > 0)
                MFRCalibrationDue = setundefined((o_MFRCalibrationDue[i].textContent || o_MFRCalibrationDue[i].innerText || o_MFRCalibrationDue[i].text));

            if (o_ProbeId.length > 0)
                ProbeId = setundefined((o_ProbeId[i].textContent || o_ProbeId[i].innerText || o_ProbeId[i].text));

            if (ProbeId == "0")
                ProbeId = "";

            if (ProbeId2 == "0")
                ProbeId2 = "";

            if (setundefined(AnnualTags) != "") {
                AnnualTags = AnnualTags.replace(/~/g, '\n');
                g_AnnualTags = AnnualTags;
                $('#txtAnnualDevices').val(AnnualTags);
            }
            else {
                g_AnnualTags = '';
                $('#txtAnnualDevices').val('');
            }

            TagRecalibrationDate = setundefined(TagRecalibrationDate);
            MonitorLocation = setundefined(MonitorLocation);

            if (BatteryReplacementOn == undefined) { BatteryReplacementOn = "&nbsp;" }

            var s30DaysCell = ""
            var s90DaysCell = ""
            var sgood = ""
            var cssClass = "";
            var Units = "";
            var cssClassLeft = "";

            if (CatastrophicCases == "1" || CatastrophicCases == "2")
                s30DaysCell = "<img src='images/Battery-Red.png' border='0' />"
            else if (CatastrophicCases == "4")
                s90DaysCell = "<img src='images/Batter-yellow.png' border='0' />"
            else if (CatastrophicCases == "0") {
                if (CCatastrophicCases == 5)
                    sgood = "<img src='images/Battery-Blue.png' border='0' />"
                else
                    sgood = "<img src='images/Battery-Green.png' border='0' />"
            }

            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering) {

                sgood = "<a href=TagLBIActivityReport.aspx?qSiteId=" + siteid + "&qTagId=" + TagId + " target='_blank'>" + sgood + "</a>"
                s30DaysCell = "<a href=TagLBIActivityReport.aspx?qSiteId=" + siteid + "&qTagId=" + TagId + " target='_blank'>" + s30DaysCell + "</a>"
                s90DaysCell = "<a href=TagLBIActivityReport.aspx?qSiteId=" + siteid + "&qTagId=" + TagId + " target='_blank'>" + s90DaysCell + "</a>"
            }

            cssClass = "siteOverview_cell";
            cssClassLeft = "DeviceList_leftBox";

            row = document.createElement('tr');

            //EM Section
            if (IsEMSec == "1") {

                if (offline == "1") {
                    cssClass = "siteOverview_cell_Off";
                    cssClassLeft = "DeviceList_leftBox_Off";
                }

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support || g_IsTempMonitoring == "True") {
                    if (Trim(Probe1Temperature) == "Probe Connection Error" || Trim(Probe2Temperature) == "Probe Connection Error" || Trim(Probe1Temperature) == "Out Of Range" || Trim(Probe2Temperature) == "Out Of Range") {
                        cssClass = "siteOverview_cell_alarm";
                        cssClassLeft = "DeviceList_leftBox_alarm";
                    }
                }

                if (hidval == 1) {
                    var chk_box = "<input type ='checkbox'  id='chk_hid' name='chk_hid' value='" + TagId + "' />"
                    AddCell(row, chk_box, cssClassLeft, "", "", "center", "20px", "40px", "");
                }

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {

                    if (IsEMSec == "1")
                        var href = "<a  class='DeviceDetailsLink' href=#EMdeviceDetail onclick=loadEMDeviceDetailsInfoOnClick(" + siteid + ",1," + TagId + ")>" + TagId + "</a>";
                    else
                        var href = "<a  class='DeviceDetailsLink' href=#deviceDetail onclick=loadDeviceDetailsInfoOnClick(" + siteid + ",1," + TagId + ")>" + TagId + "</a>";

                    if (hidval == 0)
                        AddCell(row, href, cssClassLeft, "", "", "center", "200px", "40px", "");
                    else
                        AddCell(row, href, cssClass, "", "", "center", "200px", "40px", "");
                }
                else 
                    AddCell(row, TagId, cssClassLeft, "", "", "center", "200px", "40px", "");
                
                var sProbUrl = "";
                var sProb2Url = "";

                var Probe1NISTFilePath = sWebUrlPath + ProbeId + ".pdf";

                if (setundefined(ProbeId) != "" && setundefined(ProbeId) != "0") {
                    sProbUrl = ProbeId;

                    if (setundefined(Probe1NISTFile) == "1") {
                        sProbUrl = "<a class='DeviceDetailsLink' target='_blank' href='" + Probe1NISTFilePath + "' title='Click here to download Probe Certificate'>" + ProbeId + "</a>";
                    }
                }

                var Probe2NISTFilePath = sWebUrlPath + ProbeId2 + ".pdf";

                if (setundefined(ProbeId2) != "" && setundefined(ProbeId2) != "0") {
                    sProb2Url = ProbeId2;

                    if (setundefined(Probe2NISTFile) == "1") {
                        sProb2Url = "<a class='DeviceDetailsLink' target='_blank' href='" + Probe2NISTFilePath + "' title='Click here to download Probe Certificate'>" + ProbeId2 + "</a>";
                    }
                }

                AddCell(row, sProbUrl, cssClass, "", "", "center", "100px", "40px", "");
                AddCell(row, sProb2Url, cssClass, "", "", "center", "100px", "40px", "");

                if (g_Bin == 0 || g_Bin === "")
                    AddCell(row, sgood, cssClass, "", "", "center", "100px", "40px", "");

                if (g_Bin == 1 || g_Bin === "")
                    AddCell(row, s90DaysCell, cssClass, "", "", "center", "100px", "40px", "");

                if (g_Bin == 2 || g_Bin === "")
                    AddCell(row, s30DaysCell, cssClass, "", "", "center", "100px", "40px", "");
                
				var sLocalId = "";

                if (LocalId != 0) 
                    sLocalId = "<a class='DeviceDetailsLink' title='Update LocalId' onclick=OpenLocalIdDialog(" + TagId + ",\&quot;" + encodeURIComponent(Location) + "\&quot;,\&quot;" + encodeURIComponent(LocalId) + "\&quot;," + MonitorId + ",2)>" + LocalId + "</a>";                
                else {
                    LocalId = "";
                    sLocalId = "<img style='cursor: pointer; width:18px;' alt='Add LocalId' title='Add LocalId' src='images/img_edit.png' onclick='OpenLocalIdDialog(" + TagId + ",\&quot;" + encodeURIComponent(Location) + "\&quot;,\&quot;" + encodeURIComponent(LocalId) + "\&quot;," + MonitorId + ",2);' />"
                }

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support)
                    AddCell(row, sLocalId, cssClass, "", "", "center", "350px", "40px", "");
                else
                    AddCell(row, LocalId, cssClass, "", "", "center", "350px", "40px", "");

                var sCalFrequency = "";
                if (CalFrequency != "") 
                    sCalFrequency = "<a class='DeviceDetailsLink' title='Update CalFrequency' onclick=OpenCalFrequencyDialog(" + TagId + "," + encodeURIComponent(CalFrequency) + ",1)>" + CalFrequency + "</a>";
                else 
                    sCalFrequency = "<img style='cursor: pointer; width:18px;' alt='Add CalFrequency' title='Add CalFrequency' src='images/img_edit.png' onclick='OpenCalFrequencyDialog(" + TagId + ",\&quot;" + encodeURIComponent(CalFrequency) + "\&quot;,1);' />"
                
                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support)
                    AddCell(row, sCalFrequency, cssClass, "", "", "center", "150px", "40px", "");
                else
                    AddCell(row, CalFrequency, cssClass, "", "", "center", "150px", "40px", "");

                AddCell(row, CertificateDate, cssClass, "", "", "center", "100px", "40px", "");
                AddCell(row, MFRCalibrationDue, cssClass, "", "", "center", "100px", "40px", "");

                var sUrl = "";

                var NISTFilePath = sWebUrlPath + TagId + ".pdf";

                if (NISTFile == "1")
                    sUrl = "<a target='_blank' href='" + NISTFilePath + "'><img style='cursor: pointer; width:28px;' title='Temperature Sensor Calibration Certificate' src='images/certificate.png' /></a>"

                AddCell(row, sUrl, cssClass, "", "", "center", "100px", "40px", "");
                AddCell(row, ClientCalDue, cssClass, "", "", "center", "70px", "40px", "");

                if (g_Bin === "") {
                    if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance && UserRole != enumUserRoleArr.MaintenancePrism)
                        AddCell(row, StarAddress, cssClass, "", "", "center", "200px", "40px", "");
                }

                AddCell(row, LastSeenNetwork, cssClass, "", "", "center", "150px", "40px", "");
                AddCell(row, ModelItem, cssClass, "", "", "center", "250px", "40px", "");

                if (Image == "1")
                    AddCell(row, "<img style='cursor: pointer;' src='images/picture.png' title='view sensor images' onclick='LoadImages(1," + TagId + ");' border='0' />", cssClass, "", "", "center", "100px", "40px", "");
                else if (g_UserRole != enumUserRoleArr.Customer || g_IsTempMonitoring == "True")
                    AddCell(row, "<img style='cursor: pointer;' src='images/uploadimage.png' title='upload images' onclick='LoadImages(1," + TagId + ");' border='0' />", cssClass, "", "", "center", "100px", "40px", "");
                else
                    AddCell(row, "", cssClass, "", "", "center", "100px", "40px", "");

                var sLocation = "";

                if (Location != "")
                    sLocation = "<a class='DeviceDetailsLink' title='Update Location' onclick=OpenLocalIdDialog(" + TagId + ",\&quot;" + encodeURIComponent(Location) + "\&quot;,\&quot;" + encodeURIComponent(LocalId) + "\&quot;," + MonitorId + ",1)>" + Location + "</a>";
                else
                    sLocation = "<img style='cursor: pointer; width:18px;' alt='Add Location' title='Add Location' src='images/img_edit.png' onclick='OpenLocalIdDialog(" + TagId + ",\&quot;" + encodeURIComponent(Location) + "\&quot;,\&quot;" + encodeURIComponent(LocalId) + "\&quot;," + MonitorId + ",1);' />"

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support)
                    AddCell(row, sLocation, cssClass, "", "", "center", "350px", "40px", "");
                else
                    AddCell(row, Location, cssClass, "", "", "center", "350px", "40px", "");

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {
                    AddCell(row, ADCValue, cssClass, "", "", "center", "100px", "40px", "");
                    AddCell(row, BatteryCapacity, cssClass, "", "", "center", "100px", "40px", "");
                }

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support || g_IsTempMonitoring == "True") {

                    AddCell(row, Probe1Temperature, cssClass, "", "", "center", "100px", "40px", "");
                    AddCell(row, P1Units, cssClass, "", "", "center", "100px", "40px", "");
                    AddCell(row, Probe2Temperature, cssClass, "", "", "center", "100px", "40px", "");
                    AddCell(row, P2Units, cssClass, "", "", "center", "100px", "40px", "");
                    AddCell(row, AvgRSSI, cssClass, "", "", "center", "100px", "40px", "");
                }
            }
            else //Workflow Tag
            {
                var ProfileTypeId = (o_NewDeviceSubTypeId[i].textContent || o_NewDeviceSubTypeId[i].innerText || o_NewDeviceSubTypeId[i].text);

                IsEMSec = IsEMTag(ProfileTypeId);

                if (hidval == 1) {
                    var chk_box = "<input type ='checkbox'  id='chk_hid' name='chk_hid' value='" + TagId + "' />"
                    AddCell(row, chk_box, cssClassLeft, "", "", "center", "20px", "40px", "");
                }

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {

                    if (IsEMSec == "1")
                        var href = "<a  class='DeviceDetailsLink' href=#EMdeviceDetail onclick=loadEMDeviceDetailsInfoOnClick(" + siteid + ",1," + TagId + ")>" + TagId + "</a>";
                    else
                        var href = "<a  class='DeviceDetailsLink' href=#deviceDetail onclick=loadDeviceDetailsInfoOnClick(" + siteid + ",1," + TagId + ")>" + TagId + "</a>";

                    if (hidval == 0)
                        AddCell(row, href, cssClassLeft, "", "", "center", "200px", "40px", "");
                    else
                        AddCell(row, href, cssClass, "", "", "center", "200px", "40px", "");
                }
                else 
                    AddCell(row, TagId, cssClassLeft, "", "", "center", "200px", "40px", "");
                
                AddCell(row, MonitorLocation, cssClass, "", "", "center", "100px", "40px", "");
                AddCell(row, MonitorId, cssClass, "", "", "center", "100px", "40px", "");

                if (g_Bin === "") {
                    if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance && UserRole != enumUserRoleArr.MaintenancePrism)
                        AddCell(row, StarAddress, cssClass, "", "", "center", "100px", "40px", "");
                }

                if (g_Bin == 0 || g_Bin === "") 
                    AddCell(row, sgood, cssClass, "", "", "center", "100px", "40px", "");
                
                if (g_Bin == 1 || g_Bin === "") 
                    AddCell(row, s90DaysCell, cssClass, "", "", "center", "100px", "40px", "");
                
                if (g_Bin == 2 || g_Bin === "") 
                    AddCell(row, s30DaysCell, cssClass, "", "", "center", "100px", "40px", "");

                if (g_Bin == 3 || g_Bin === "" || g_Bin == 7)
                    AddCell(row, LastSeenMonitor, cssClass, "", "", "center", "100px", "40px", "");

                AddCell(row, LastSeenNetwork, cssClass, "", "", "center", "100px", "40px", "");
                AddCell(row, ModelItem, cssClass, "", "", "center", "100px", "40px", "");

                if (offline == "1")
                    offline = "<img src='images/Close_1.png' border='0' />"
                else
                    offline = ""

                if (g_Bin == 3 || g_Bin === "" || g_Bin == 7)
                    AddCell(row, offline, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                if (g_Bin === "")
                    AddCell(row, BatteryReplacementOn, cssClass, "", "", "center", "100px", "40px", "");

                if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Support) {

                    // LBI Activity for spark line chart
                    sSpanCtrlId = "dynamicsparkline" + TagId;
                    var sSpanCtrl = "<span id='" + sSpanCtrlId + "'>...</span>";
                    AddCell(row, sSpanCtrl, cssClass, "", "", "center", "110px", "40px", "");

                    var sActivityUrl = "<a href=TagLBIActivityReport.aspx?qSiteId=" + siteid + "&qTagId=" + TagId + "&qExport=1><img style='cursor: pointer;' src='images/downloadfile.png' /></a>"
                    AddCell(row, sActivityUrl, cssClass, "", "", "center", "70px", "40px", "");
                }

                if (g_Bin === "" && isUHFTags == "True") {

                    if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support)
                        AddCell(row, CActivityLevel, cssClass, "", "", "center", "100px", "40px", "");
                }
            }

            sTbl.appendChild(row);

            if (Last20WeekData != "") {

                Last20WeekDateArr = new Array();
                arrSplitWeekData = new Array();

                arrSplitWeekData = Last20WeekData.split(",");

                if (arrSplitWeekData.length > 0) {
                    for (var nIdx = 0; nIdx < arrSplitWeekData.length; nIdx++) {
                        Last20WeekDateArr.push(arrSplitWeekData[nIdx]);
                    }
                }

                $('#' + sSpanCtrlId).sparkline(Last20WeekDateArr);
            }
        }
    }
    else {
        if (isSearch != 1)
            document.getElementById("trDeviceListRow").style.display = "";

        document.getElementById("btnSearchDeviceList").style.display = "";

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "1400px", "40px", "");
        sTbl.appendChild(row);
    }

    document.getElementById("tdPagination").style.display = "";
    document.getElementById("divLoading").style.display = "none";

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Page Visit Tracking
    try {
        var tagType = $("#subHeader").text();
        var siteName = $("#ctl00_ContentPlaceHolder1_lblsitename").text();

        PageVisitDetails(g_UserId, "Home - Tag List", enumPageAction.View, tagType + " list viewed in site - " + siteName);
    }
    catch (e) {

    }
}

function doTagEnableButton(MaxPageCnt) {

    var curnPage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo")
    var curPage = curnPage.value;

    if (MaxPageCnt == "1" || Number(MaxPageCnt) == 1) {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1";
        document.getElementById("btnNext").style.display = "none";
        document.getElementById("btnPrev").style.display = "none";
    }
    else {
        document.getElementById("btnNext").style.display = "";
        document.getElementById("btnPrev").style.display = "";
    }

    if (Number(curPage) <= 1) {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1"
        document.getElementById("btnPrev").style.display = "none";
    }

    if (Number(curPage) >= Number(MaxPageCnt)) {
        document.getElementById("btnNext").style.display = "none";
    }
}

var Excel_Root;

//For Export Tag Search
function DownloadInforamtion(siteid, alertId, bin, curpag, typeId, deviceIds, orgtypeId, devicetype, UIId) {

    $.post("AjaxConnector.aspx?cmd=DownloadExcel",
      {
          sid: siteid,
          alertId: alertId,
          Bin: bin,
          curpage: curpag,
          typId: typeId,
          DeviceId: deviceIds,
          devicetype: devicetype,
          UIId: UIId
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              Excel_Root = data.documentElement;
              ajaxDownloadinforamtion();
          }
          else {
              document.getElementById("divExcelLoading").style.display = "none";
          }
      });
}

  //For Export EM Tag
function DownloadEMTag(siteid) {

    document.getElementById("divExcelLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=EMTagReport",
      {
          sid: $("#ctl00_ContentPlaceHolder1_ddlsite").val()
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              Excel_Root = data.documentElement;
              ajaxDownloadinforamtion();
          }
          else {
              document.getElementById("divExcelLoading").style.display = "none";
          }
      });
}

  //For Export EM Tag
function DownloadEMTagDetailedReport(siteid) {

    document.getElementById("divExcelLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=EMTagDetailReport",
      {
          sid: $("#ctl00_ContentPlaceHolder1_ddlsite").val(),
          EMReportType: g_EMReportType
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              Excel_Root = data.documentElement;
              ajaxDownloadinforamtion();
          }
          else {
              document.getElementById("divExcelLoading").style.display = "none";
          }
      });
}

//*******************************************************************
//	Function Name	:	DownloaddTaginforamtion
//	Input			:	None
//	Description		:	Download Tag Infomation into Excel from ajax Response
//*******************************************************************
function ajaxDownloadinforamtion() {

    if (Excel_Root != null) {

        var o_Excel = Excel_Root.getElementsByTagName('Excel');
        var o_Filename = Excel_Root.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divExcelLoading").style.display = "none";
    }
}

function DownloadInforamtion_ForIE(siteid, alertId, bin, curpag, typeId, deviceIds, orgtypeId, devicetype, UIId) {

    location.href = "AjaxConnector.aspx?cmd=DownloadExcel_ForIE&sid=" + siteid + "&alertId=" + alertId + "&Bin=" + bin + "&curpage=" + curpag + "&typId=" + typeId + "&devicetype=" + devicetype + "&DeviceId=" + deviceIds + "&UIId=" + UIId;
}

//For Tag Search
function doLoadTagSearch(siteid, alertId, bin, curpag, typeId, deviceIds, orgtypeId) {

    $.post("AjaxConnector.aspx?cmd=TagListSearch",
      {
          sid: siteid,
          alertId: alertId,
          Bin: bin,
          curpage: curpag,
          typId: typeId,
          DeviceId: deviceIds,
          orgtypeId: orgtypeId,
          sorColumnname: TAG_SortColumn,
          SorOrder: TAG_SortOrder
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              g_TagRoot = data.documentElement;
              loadTagList(1);
          }
          else {
              document.getElementById("divLoading").style.display = "none";
          }

      });
}

function exportTableToCSV($table, filename) {

    var $rows = $table.find('tr:has(td)'),

    // Temporary delimiter characters unlikely to be typed by keyboard
    // This is to avoid accidentally splitting the actual contents
            tmpColDelim = String.fromCharCode(11), // vertical tab character
            tmpRowDelim = String.fromCharCode(0), // null character

    // actual delimiter characters for CSV format
            colDelim = '","',
            rowDelim = '"\r\n"',

    // Grab text from table into CSV formatted string
            csv = '"' + $rows.map(function (i, row) {
                var $row = $(row),
                    $cols = $row.find('td');

                return $cols.map(function (j, col) {
                    var $col = $(col),
                        text = $col.text();

                    return text.replace('"', '""'); // escape double quotes

                }).get().join(tmpColDelim);

            }).get().join(tmpRowDelim)
                .split(tmpRowDelim).join(rowDelim)
                .split(tmpColDelim).join(colDelim) + '"',

    // Data URI
            csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);

    $(this)
            .attr({
                'download': filename,
                'href': csvData,
                'target': '_blank'
            });
}

function OpenAnnualCalibrationDialog() {

    $('#txtAnnualDevices').val(g_AnnualTags);

    //Open Dialog
    $("#dialog-AnnualCalibration").dialog({
        height: 370,
        width: 700,
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

function CancelAnnualCalibrationDialog() {

    $('#dialog-AnnualCalibration').dialog('close');
}

function CancelSingleAnnualCalibrationDialog() {

    $('#dialog-TagAnnualCalibration').dialog('close');
}

//For Tag Search
function SaveAnnualCalibration(IsSingleclick) {

    if (IsSingleclick == "1") {

        if (setundefined($('#txtCalFrequency').val()) == "")
            deviceIds = setundefined($('#txtCalFreqTagId').val()) + ", 0";
        else
            deviceIds = setundefined($('#txtCalFreqTagId').val()) + ", " + setundefined($('#txtCalFrequency').val());

        document.getElementById("divLoading_CalFre").style.display = "";
    }
    else {
        var deviceIds = setundefined($('#txtAnnualDevices').val());
        document.getElementById("divLoading_SensorRecalibration").style.display = "";
    }

    document.getElementById("hidUpdateLocalId").value = "1";

    $.post("AjaxConnector.aspx?cmd=UpdateAnnualCalibration",
    {
        Annual_sid: SiteId,
        Annual_DeviceId: deviceIds
    },
    function (data, status) {

        if (status == "success") {

            document.getElementById("divLoading_SensorRecalibration").style.display = "none";
            document.getElementById("divLoading_CalFre").style.display = "none";

            CloasAnnualCalibrationDialogWindow(IsSingleclick);
        }
    });
}

function CloasAnnualCalibrationDialogWindow(IsSingleclick) {

    var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

    if (IsSingleclick == "1")
        CancelSingleAnnualCalibrationDialog();
    else
        CancelAnnualCalibrationDialog();

    document.getElementById("divLoading").style.display = "";

    if (document.getElementById("txtSearchDeviceIds").value != "")
        loadTagInfoForSearch(SiteId, "0", document.getElementById("ddSearchBin").value, document.getElementById("txtSearchDeviceIds").value, typeId);
    else
        doLoadTag(SiteId, Alertid, bin, currentpage, typeId);
}

function DownLoaddAnnualCalibration() {

    document.getElementById("divExcelLoading").style.display = "";

    if (g_Obj == null) {
        g_Obj = CreateTagXMLObj();
    }

    if (g_Obj != null) {

        g_Obj.onreadystatechange = ajaxDownloadAnnualCalibration;

        DbConnectorPath = "AjaxConnector.aspx?cmd=DownloadAnnualCalibration&sid=" + SiteId + "&alertId=0&Bin=6&curpage=0&typId=0";

        if (GetBrowserType() == "isIE") {
            g_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Obj.open("GET", DbConnectorPath, true);
        }

        g_Obj.send(null);
    }

    return false;
}

//*******************************************************************
//	Function Name	:	DownloaddTaginforamtion
//	Input			:	None
//	Description		:	Download Tag Infomation into Excel from ajax Response
//*******************************************************************
function ajaxDownloadAnnualCalibration() {

    if (g_Obj.readyState == 4) {

        if (g_Obj.status == 200) {

            var dsRoot = g_Obj.responseXML.documentElement;

            if (dsRoot != null) {

                var o_Excel = dsRoot.getElementsByTagName('Excel');
                var o_Filename = dsRoot.getElementsByTagName('Filename');

                var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
                var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

                //Export table string to CSV
                tableToCSV(Excel, Filename);

                document.getElementById("divExcelLoading").style.display = "none";
            }
        }
    }
}

var title = "";
var L_Type = "";

function OpenLocalIdDialog(TagId, Location, LocalId, MonitorId, Type) {

    L_Type = Type;

    if (Type == 1) {
        document.getElementById("trLocation").style.display = "";
        document.getElementById("trLocalId").style.display = "none";
        title = "Update Location";
    }
    else if (Type == 2) {
        document.getElementById("trLocalId").style.display = "";
        document.getElementById("trLocation").style.display = "none";
        title = "Update LocalID";
    }

    $('#txtTagId').val(TagId);
    $('#txtLocation').val(decodeURIComponent(Location));
    $('#txtLocalId').val(decodeURIComponent(LocalId));
    L_MonitorId = setundefined(MonitorId);

    //Open Dialog
    $("#dialog-UpdateLocalId").dialog({

        height: 300,
        width: 550,
        modal: true,
        title: title,
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

function OpenCalFrequencyDialog(TagId, CalFrequency, Type) {

    $('#txtCalFreqTagId').val(TagId);
    $('#txtCalFrequency').val(decodeURIComponent(CalFrequency));

    //Open Dialog
    $("#dialog-TagAnnualCalibration").dialog({
        height: 300,
        width: 680,
        modal: true,
        title: 'Add/Edit Cal Frequency',
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

//For Update LocalId and Location
function SaveLocalId() {

    var TagId = setundefined($('#txtTagId').val());
    var Location = setundefined($('#txtLocation').val());
    var LocalId = setundefined($('#txtLocalId').val());

    if (L_Type == 1) {
        if (Location == "") {
            alert("Please enter a Location");
            return;
        }
    }
    else if (L_Type == 2 && (LocalId == "0" || LocalId == "")) {
        alert("Please enter a Local ID");
        return;
    }

    document.getElementById("hidUpdateLocalId").value = "1";
    document.getElementById("divUpdateLocal").style.display = "";

    $.post("AjaxConnector.aspx?cmd=UpdateLocalIdInfo",
    {
        Local_sid: SiteId,
        Local_DeviceId: TagId,
        LocalId: LocalId,
        Location: Location,
        MonitorId: L_MonitorId
    },
    function (data, status) {
        if (status == "success") {

            document.getElementById("divUpdateLocal").style.display = "none";
            document.getElementById("divLoading").style.display = "";

            CloasLocalDialogWindow();
        }
    });
}

function CloasLocalDialogWindow() {

    var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

    $('#dialog-UpdateLocalId').dialog('close');

    if (document.getElementById("txtSearchDeviceIds").value != "")
        loadTagInfoForSearch(SiteId, "0", document.getElementById("ddSearchBin").value, document.getElementById("txtSearchDeviceIds").value, typeId);
    else
        doLoadTag(SiteId, Alertid, bin, currentpage, typeId);
}

function CancelLocaldialog() {

    $('#dialog-UpdateLocalId').dialog('close');
}

function sortTagInfo(sortCol) {

    var curpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo")
    var currentpage = curpage.value;

    document.getElementById("divLoading").style.display = "";

    if (TAG_SortColumn != sortCol) {
        TAG_SortOrder = "desc";
        TAG_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else {
        if (TAG_SortOrder == "desc") {

            TAG_SortOrder = "asc";
            TAG_SortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if (TAG_SortOrder == "asc") {
            TAG_SortOrder = "desc";
            TAG_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }
    if (sortCol != "") {
        TAG_SortColumn = sortCol;
    }

    if (document.getElementById("txtSearchDeviceIds").value != "")
        doLoadTagSearch(SiteId, "0", document.getElementById("ddSearchBin").value, "0", "0", document.getElementById("txtSearchDeviceIds").value, typeId);
    else
        doLoadTag(SiteId, Alertid, bin, currentpage, typeId);
}

function UploadG2Data() {

    $("#ifrmUploadG2TempData").attr("src", "uploadFile.aspx?Cmd=UploadG2TempData&SiteId=" + SiteId);

    var ifr = document.getElementById("ifrmUploadG2TempData");

    ifr.contentWindow.location.replace("uploadFile.aspx?Cmd=UploadG2TempData&SiteId=" + SiteId);

    var curpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo")
    var currentpage = curpage.value;

    $("#divImportG2TempData").dialog({

        height: 400,
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
            doLoadTag(SiteId, Alertid, bin, currentpage, typeId);
        }
    });
}

function DownloadDisasterRecovery(siteid) {

    $.post("AjaxConnector.aspx?cmd=DownloadExcelDisasterRecovery",
    {
        sid: siteid       
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            Excel_Root = data.documentElement;
            ajaxDownloadDisasterRecovery();
        }
        else {
            document.getElementById("divExcelLoading").style.display = "none";
        }
    });
}

function ajaxDownloadDisasterRecovery() {

    if (Excel_Root != null) {

        var o_Excel = Excel_Root.getElementsByTagName('Excel');
        var o_Filename = Excel_Root.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divExcelLoading").style.display = "none";
    }
}
//For Export Monitor Group Report
function DownloadMonitorGroupReport(siteid) {

    document.getElementById("divExcelLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=MonitorGroupReport",
    {
        sid: $("#ctl00_ContentPlaceHolder1_ddlsite").val()
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            Excel_Root = data.documentElement;
            ajaxDownloadinforamtion();
        }
        else {
            document.getElementById("divExcelLoading").style.display = "none";
        }
    });
}
