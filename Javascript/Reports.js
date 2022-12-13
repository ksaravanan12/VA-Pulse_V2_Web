    
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

//Ajax Call for Site View
var g_SObj;
var g_SRoot;

function GetSiteList(SiteId) {
   
    if (g_SObj != null) { g_SObj = null; }

    g_SObj = CreateXMLObj();

    if (g_SObj != null) {

        g_SObj.onreadystatechange = ajaxLoadSiteListView;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetSiteList&Site=" + SiteId;

        if (GetBrowserType() == "isIE") {
            g_SObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_SObj.open("GET", DbConnectorPath, true);
        }
        g_SObj.send(null);
    }
    return false;
}

//Ajax Request for Site View
function ajaxLoadSiteListView() {

    if (g_SObj.readyState == 4) {

        if (g_SObj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_SObj.responseXML.documentElement);

            g_SRoot = g_SObj.responseXML.documentElement;

            LoadSites();
        }
    }
}

function LoadSites() {
 
    $select = $("#ddlSites");
    $select.empty();

    var o_SiteId = g_SRoot.getElementsByTagName('SiteId');
    var o_SiteName = g_SRoot.getElementsByTagName('SiteName');

    var nRootLength = o_SiteId.length;

    if (nRootLength > 0) {

        for (var i = 0; i <= nRootLength - 1; i++) {

            var SiteId = (o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
            var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);    

            $select.append($("<option>", { "value": SiteId, "text": SiteName }));
        }

        $('#ddlSites').val(siteVal);
        $('#ddlSites').multipleSelect('refresh');
    }
}

function LoadTagTypes() {

    $select = $("#ddlTagType");
    $select.empty();

    elem = $('#ddlTagType');

    listItems = { 'ASSET TAG': 1, 'MM ASSET TAG': 2, 'HHC/STAFF TAG': 3, 'MM STAFF TAG': 4, 'TEMP TAG': 5, 'ERU TAG': 6, 'AMBIENT TEMP TAG': 7, 'PATIENT TAG': 8, 'G2TEMP TAG': 9, 'SUPT': 10, 'AMBIENT TEMP RH TAG': 11, 'MOTHER TAG': 12 };
  
    $.each(listItems, function (key, value) {
        $(elem)
                  .append($("<option selected='selected'></option>")
                  .attr("value", value)
                  .text(key));
    });

    $('#ddlTagType').multipleSelect('refresh');
}

var g_Dev_Obj;

function GetCentrakDevices(SiteId, DeviceType, ModelItem, DeviceSubType, SortColumn, SortOrder) {

    g_Dev_Obj = CreateXMLObj();

    document.getElementById("divLoading").style.display = "";

    if (g_Dev_Obj != null) {

        g_Dev_Obj.onreadystatechange = ajaxIniHistory;

        DbConnectorPath = "AjaxConnector.aspx?cmd=CenTrakModelItems&SiteId=" + SiteId + "&DeviceType=" + DeviceType + "&ModelItem=" + ModelItem + "&sortcolumn=" + SortColumn + "&sortorder=" + SortOrder + "&DeviceSubType=" + DeviceSubType;

        if (GetBrowserType() == "isIE") {
            g_Dev_Obj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_Dev_Obj.open("GET", DbConnectorPath, true);
        }

        g_Dev_Obj.send(null);
    }
    return false;
}

var dsRoot;

function ajaxCentrakDevices() {

    if (g_Dev_Obj.readyState == 4) {

        if (g_Dev_Obj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Dev_Obj.responseXML.documentElement);

            dsRoot = g_Dev_Obj.responseXML.documentElement;

            var sTbl, sTblLen;

            sTbl = document.getElementById('tblBatteryCalculation');

            document.getElementById("tblBatteryCalculation").style.display = "";

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);
                       
            LoadIniTagHistoryList();                     
        }
    }
}

function LoadIniTagHistoryList() {

    var sTbl, sTblLen;

    sTbl = document.getElementById('tblIniTagHistoryInfo');

    document.getElementById("tblIniTagHistoryInfo").style.display = "";

    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var Tagtable = $(dsRoot).find("Tag");

    var o_DataId = $(Tagtable).children().filter('DataId');
    var o_TagId = $(Tagtable).children().filter('TagId');
    var o_Date = $(Tagtable).children().filter('Date');
    var o_TotalPage = $(Tagtable).children().filter('TotalPage');
    var o_TotalCount = $(Tagtable).children().filter('TotalCount');

    var nTagRootLength = o_Date.length;

    row = document.createElement('tr');
    AddCellForSorting(row, "Date", 'siteOverview_TopLeft_Box', "", "", "center", "300px", "40px", "", "UpdatedOn", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCellForSorting(row, "Tag Id", 'siteOverview_Box', "", "", "center", "300px", "40px", "", "TagId", SortColumn, SortImg, enumSortingArr.INI_History);
    AddCell(row, "Profile", 'siteOverview_Topright_Box', "", "", "center", "100px", "40px", "");
    sTbl.appendChild(row);

    if (nTagRootLength == 0) {

        var total_cnt = document.getElementById("tdtotalrec")
        total_cnt.innerHTML = "Total Records : " + nTagRootLength;

        row = document.createElement('tr');
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
        document.getElementById("divLoading").style.display = "none";
        return;
    }

    //totalcount
    var ttcnt_lable = document.getElementById("tdtotalrec")
    ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

    //Totalpage
    var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
    ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

    var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
    doEnableButtonTag(MaxPageCnt);

    if (nTagRootLength > 0) {

        for (var i = 0; i <= nTagRootLength - 1; i++) {

            var DataId = (o_DataId[i].textContent || o_DataId[i].innerText || o_DataId[i].text);
            var TagId = (o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
            var Date = (o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text);

            row = document.createElement('tr');
            AddCell(row, Date, 'DeviceList_leftBox', "", "", "", "600px", "40px", "10px");
            AddCell(row, TagId, 'siteOverview_cell', "", "", "", "600px", "40px", "10px");

            var imgProfile = "<img src='Images/imgViewMore.png' title='Profile' onclick=\"OpenProfileDialog('" + DataId + "','" + TagId + "');\" border='0' style='width: 28px; height: 28px; padding-top: 10px; padding-bottom: 10px; cursor: pointer'/>";
            AddCell(row, imgProfile, 'siteOverview_cell', "", "", "", "200px", "20px", "10px");

            sTbl.appendChild(row);
        }
    }

    document.getElementById("divLoading").style.display = "none";

    try {
        PageVisitDetails(g_UserId, "INI Change History", enumPageAction.Filter, "History filtered for TagId " + TagId);
    }
    catch (e) {

    }
}

var g_LocationObj;

function GenerateLocationChangeReport() {

    var FromDate = "";
    var ToDate = "";

    var SiteId = setundefined($('#ctl00_ContentPlaceHolder1_ddlLocationSites').val());

    TagIds = setundefined($('#txtLocationId').val());

    $('#txtLocationId').val(GetDeviceIdFormat(TagIds));

    TagIds = setundefined($('#txtLocationId').val());
    FromDate = setundefined($('#txtLocationFromDate').val());
    ToDate = setundefined($('#txtLocationToDate').val());

    if (SiteId == 0) {
        alert("Please select a site.");
        $('#ddlSites').focus();
        return;
    }
    else if (FromDate == '') {
        alert("Please enter a from date");
        $('#txtLocationFromDate').focus();
        return;
    }
    else if (ToDate == '') {
        alert("Please enter a to date");
        $('#txtLocationToDate').focus();
        return;
    }
    else if (Date.parse(FromDate) > Date.parse(ToDate)) {
        alert("To date must be greater than from date!");
        $("#txtLocationFromDate").focus();
        return false;
    }
    else if (TagIds == '' || TagIds == null) {
        alert("Please Enter Tag Ids.");
        $('#txtLocationId').focus();
        return;
    }

    var arrayTagId = TagIds.split(',');
    
    if (arrayTagId.length > 10) {
        alert("At any given time, the location change report can be generated for a maximum of 10 devices.!");
        $('#txtLocationId').focus();
        return;
    }

    GetLocationChangeEvent(SiteId, setundefined($('#txtLocationId').val()), FromDate, ToDate, "-1", "8",
                    setundefined($('#txtinMonitorIds').val()), setundefined($('#txtexMonitorIds').val()), setundefined($('#txtTimeSpend').val()), setundefined($('#selSpendType').val()));
}

function datediff(first, second) {
    // Take the difference between the dates and divide by milliseconds per day.
    // Round to nearest whole number to deal with DST.
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}

function GetLocationChangeEvent(SiteId, DeviceId, FromDate, ToDate, Type, EventTreshold, inMonitorIds, exMonitorIds, TimeSpend, SpendType) {

    document.getElementById("divLoading").style.display = "";

    if (g_LocationObj != null) { g_LocationObj = null; }

    g_LocationObj = CreateXMLObj();

    if (g_LocationObj != null) {

        g_LocationObj.onreadystatechange = ajaxLocationChangeEvent;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetLocationChangeEvent&SiteId=" + SiteId + "&DeviceId=" + DeviceId
                          + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&Type=" + Type + "&EventTreshold=" + EventTreshold
                          + "&inMonitorIds=" + inMonitorIds + "&exMonitorIds=" + exMonitorIds + "&TimeSpend=" + TimeSpend + "&SpendType=" + SpendType;

        if (GetBrowserType() == "isIE") {
            g_LocationObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_LocationObj.open("GET", DbConnectorPath, true);
        }
        g_LocationObj.send(null);
    }
    return false;
}

function ajaxLocationChangeEvent() {

    if (g_LocationObj.readyState == 4) {

        if (g_LocationObj.status == 200) {

            var dsLocRoot = g_LocationObj.responseXML.documentElement;

            if (dsLocRoot != null) {

                var o_Excel = dsLocRoot.getElementsByTagName('Excel');
                var o_Filename = dsLocRoot.getElementsByTagName('Filename');

                if (o_Excel.length == 0) {
                    var o_Result = dsLocRoot.getElementsByTagName('Result');

                    if (o_Result.length > 0) {
                        var Result = (o_Result[0].textContent || o_Result[0].innerText || o_Result[0].text);

                        if (Result == 1) {
                            document.getElementById("divLoading").style.display = "none";
                            alert("Invalid date range. Maximum of 7 days is allowed.");
                            $('#txtLocationToDate').focus();
                            return;
                        }
                    }
                }

                var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
                var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

                //Export table string to CSV
                tableToCSV(Excel, Filename);

                document.getElementById("divLoading").style.display = "none";
            }
        }
    }
}