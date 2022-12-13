// JScript File

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

var nType;
var siteName;

function doLoadInfrastructure(siteid, alertId, bin, curpag, typeId) {

    Load_Infrastructure_inforamtion(siteid, alertId, bin, curpag, typeId);
}

var inf_Obj;
var g_Bin = 0;

function Load_Infrastructure_inforamtion(siteid, alertId, bin, curpag, typeId) {

    inf_Obj = CreateInfraXMLObj();

    g_Bin = bin;

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = ajaxInfraList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=InfraList&sid=" + siteid + "&alertId=" + alertId + "&Bin=" + bin + "&curpage=" + curpag + "&typId=" + typeId;

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

var g_InfraRoot;
function ajaxInfraList() {

    if (inf_Obj.readyState == 4) {

        if (inf_Obj.status == 200) {

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            g_InfraRoot = inf_Obj.responseXML.documentElement;

            loadInfraList(0);

            g_InfraAllDeviceRoot = g_InfraRoot;
        }
    }
}

//FOR SERACH
function Load_Infrastructure_inforamtionSearch(siteid, alertId, bin, curpag, typeId, deviceIds) {

    $.post("AjaxConnector.aspx?cmd=InfraListSearch",
      {
          sid: siteid,
          alertId: alertId,
          Bin: bin,
          curpage: curpag,
          typId: typeId,
          DeviceId: deviceIds
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              g_InfraRoot = data.documentElement;
              loadInfraList(1);
          }
          else {
              document.getElementById("divLoading").style.display = "none";
          }
      });
}

function loadInfraList(isSearch) {

    var sTbl, sTblLen;
    var sSpanCtrlId = "";

    var Last20WeekDateArr = new Array();
    var arrSplitWeekData = new Array();

    if (isSearch == 0)
        sTbl = document.getElementById('tblInfraInfo');
    else
        sTbl = document.getElementById('tblInfraInfoSearch');

    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl, sTblLen);

    var o_UserRole = g_InfraRoot.getElementsByTagName('UserRole')
    var o_siteId = g_InfraRoot.getElementsByTagName('SiteId')
    var o_SiteName = g_InfraRoot.getElementsByTagName('SiteName')
    var o_TotalPage = g_InfraRoot.getElementsByTagName('TotalPage')
    var o_TotalCount = g_InfraRoot.getElementsByTagName('TotalCount')
    var o_DeviceName = g_InfraRoot.getElementsByTagName('DeviceName')
    var o_DeviceId = g_InfraRoot.getElementsByTagName('DeviceId')
    var o_CatastrophicCases = g_InfraRoot.getElementsByTagName('CatastrophicCases')
    var o_IRID = g_InfraRoot.getElementsByTagName('IRID')
    var o_RoomName = g_InfraRoot.getElementsByTagName('RoomName')
    var o_ModelItem = g_InfraRoot.getElementsByTagName('ModelItem')
    var o_LessThen90Days = g_InfraRoot.getElementsByTagName('LessThen90Days')
    var o_LessThen30Days = g_InfraRoot.getElementsByTagName('LessThen30Days')
    var o_offline = g_InfraRoot.getElementsByTagName('offline')
    var o_BatteryReplacementOn = g_InfraRoot.getElementsByTagName('BatteryReplacementOn')
    var o_Last20WeekData = g_InfraRoot.getElementsByTagName('Last20WeekData');
    var o_IsConfiguredinCore = g_InfraRoot.getElementsByTagName('IsConfiguredinCore');
    var o_LastSeen = g_InfraRoot.getElementsByTagName('LastSeen');
    var o_CCatastrophicCases = g_InfraRoot.getElementsByTagName('CCatastrophicCases');

    hideRecalibration();

    var hidval;

    if (isSearch == 0)
        hidval = document.getElementById('hid_Show').value;
    else
        hidval = document.getElementById('hdn_Search_Show').value;

    nRootLength = o_siteId.length;

    if (isSearch == 1) {
        document.getElementById("btnExportSearch").style.display = "none";

        if (nRootLength > 0)
            document.getElementById("btnExportSearch").style.display = "";
    }

    //sitename
    if (nRootLength > 0) {

        if (isSearch != 1) {
            document.getElementById("trDeviceListRow").style.display = "";
            document.getElementById("tdPagination").style.display = "";
        }

        document.getElementById("btnSearchDeviceList").style.display = "";

        //User Role
        var UserRole = (o_UserRole[0].textContent || o_UserRole[0].innerText || o_UserRole[0].text);

        if (isSearch == 0)
            document.getElementById("btnExportExcel").disabled = false;

        row = document.createElement('tr');

        if (hidval == 1) {
            var chk_Allbox = "<input type='checkbox' id='chkAll' name='chkAll' style='vertical-align: middle;'>";
            AddCell(row, chk_Allbox, "siteOverview_TopLeft_Box", "", "center", "20px", "40px", "");
            AddCell(row, "Devices", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
        }
        else
            AddCell(row, "Devices", 'siteOverview_TopLeft_Box', "", "", "center", "100px", "40px", "");

        AddCell(row, "Monitor ID", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
        AddCell(row, "Monitor Location", 'siteOverview_Box', "", "", "center", "150px", "40px", "");

        if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance) 
           AddCell(row, "Configured in Core", 'siteOverview_Box', "", "", "center", "50px", "40px", "");

        if (g_Bin == 0 || g_Bin === "" || g_Bin == 3 || g_Bin == 7)
            AddCell(row, "Good", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

        if (g_Bin == 1 || g_Bin === "" || g_Bin == 3 || g_Bin == 7)
            AddCell(row, "Change In <br/> 90 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

        if (g_Bin == 2 || g_Bin === "" || g_Bin == 3 || g_Bin == 7)
            AddCell(row, "Change In <br/> 30 Days", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

        AddCell(row, "Date Last seen</br>by Network", "siteOverview_Box", "", "", "center", "100px", "40px", "");
        AddCell(row, "Model Item", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

        if (g_Bin == 3 || g_Bin === "")
            AddCell(row, "Offline", 'siteOverview_Box', "", "", "center", "100px", "40px", "");

        if (g_Bin == 7)
            AddCell(row, "Offline (Battery Depleted)", "siteOverview_Box", "", "", "center", "100px", "40px", ""); 

        if (g_Bin === "")
            AddCell(row, "Battery Replaced On", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
               
        if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Support) {

            AddCell(row, "LBI Activity", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
            AddCell(row, "Data", 'siteOverview_Topright_Box', "", "", "center", "70px", "40px", "");
        }        

        sTbl.appendChild(row);

        if (isSearch == 0) {

            //totalcount
            var ttcnt_lable = document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount")
            ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

            //Totalpage
            var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
            ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

            var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
            doInfraEnableButton(MaxPageCnt);
        }

        for (var i = 0; i < nRootLength; i++) {

            var siteid = (o_siteId[i].textContent || o_siteId[i].innerText || o_siteId[i].text);
            var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);
            var TotalPage = (o_TotalPage[i].textContent || o_TotalPage[i].innerText || o_TotalPage[i].text);
            var TotalCount = (o_TotalCount[i].textContent || o_TotalCount[i].innerText || o_TotalCount[i].text);
            var DeviceName = (o_DeviceName[i].textContent || o_DeviceName[i].innerText || o_DeviceName[i].text);
            var DeviceId = (o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
            var CatastrophicCases = (o_CatastrophicCases[i].textContent || o_CatastrophicCases[i].innerText || o_CatastrophicCases[i].text);
            var ModelItem = (o_ModelItem[i].textContent || o_ModelItem[i].innerText || o_ModelItem[i].text);
            var LessThen90Days = (o_LessThen90Days[i].textContent || o_LessThen90Days[i].innerText || o_LessThen90Days[i].text);
            var LessThen30Days = (o_LessThen30Days[i].textContent || o_LessThen30Days[i].innerText || o_LessThen30Days[i].text);
            var offline = (o_offline[i].textContent || o_offline[i].innerText || o_offline[i].text);
            var IRID = (o_IRID[i].textContent || o_IRID[i].innerText || o_IRID[i].text);
            var RoomName = setundefined(o_RoomName[i].textContent || o_RoomName[i].innerText || o_RoomName[i].text);
            var BatteryReplacementOn = (o_BatteryReplacementOn[i].textContent || o_BatteryReplacementOn[i].innerText || o_BatteryReplacementOn[i].text);
            var Last20WeekData = setundefined((o_Last20WeekData[i].textContent || o_Last20WeekData[i].innerText || o_Last20WeekData[i].text));
            var IsConfiguredinCore = setundefined((o_IsConfiguredinCore[i].textContent || o_IsConfiguredinCore[i].innerText || o_IsConfiguredinCore[i].text));
            var LastSeenNetwork = setundefined((o_LastSeen[i].textContent || o_LastSeen[i].innerText || o_LastSeen[i].text));
            var CCatastrophicCases = setundefined((o_CCatastrophicCases[i].textContent || o_CCatastrophicCases[i].innerText || o_CCatastrophicCases[i].text));

            var sgood = ""
            var s30DaysCell = ""
            var s90DaysCell = ""

            var clsoffline = "DeviceList_leftBox";

            if (CatastrophicCases == "1")
                s30DaysCell = "<img src='images/Battery-Red.png' border='0' />"
            else if (CatastrophicCases == "2")
                s90DaysCell = "<img src='images/Batter-yellow.png' border='0' />"
            else if (CatastrophicCases == "0") {
                if (CCatastrophicCases == 5)
                  sgood = "<img src='images/Battery-Blue.png' border='0' />"
                else
                   sgood = "<img src='images/Battery-Green.png' border='0' />"
            }

            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering) {
                sgood = "<a href=MonitorLBIActivityReport.aspx?qSiteId=" + siteid + "&qMonitorId=" + DeviceId + " target='_blank'>" + sgood + "</a>"
                s30DaysCell = "<a href=MonitorLBIActivityReport.aspx?qSiteId=" + siteid + "&qMonitorId=" + DeviceId + " target='_blank'>" + s30DaysCell + "</a>"
                s90DaysCell = "<a href=MonitorLBIActivityReport.aspx?qSiteId=" + siteid + "&qMonitorId=" + DeviceId + " target='_blank'>" + s90DaysCell + "</a>"
            }

            if (offline == "1") {
                offline = "<img src='images/Close_1.png' border='0' />"
                clsoffline = "DeviceList_leftBox_Gray"
            }
            else
                offline = ""

            row = document.createElement('tr');

            if (hidval == 1) {

                var chk_box = "<input type ='checkbox'  id='chk_hid' name='chk_hid' value='" + DeviceId + "' />"
                AddCell(row, chk_box, 'DeviceList_leftBox', "", "", "center", "20px", "40px", "");
                AddCell(row, DeviceName, clsoffline, "", "", "center", "150px", "40px", "");
            }
            else
                AddCell(row, DeviceName, 'DeviceList_leftBox', "", "", "center", "150px", "40px", "");

            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {

                var href = "<a  class='DeviceDetailsLink' href=#deviceDetail onclick=loadDeviceDetailsInfoOnClick(" + siteid + ",2," + DeviceId + ")>" + DeviceId + "</a>";
                AddCell(row, href, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
            }
            else 
                AddCell(row, DeviceId, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
            
            var sLocation = "";
            var LocationName = setundefined(RoomName);

            if (LocationName != "")
                sLocation = "<a class='DeviceDetailsLink' title='Update Monitor Location' onclick=OpenLocationChangeDialog(2," + DeviceId + ",\&quot;" + encodeURIComponent(LocationName) + "\&quot;)>" + LocationName + "</a>";
            else
                sLocation = "<img style='cursor: pointer; width: 18px;' alt='Add Monitor Location' title='Add Monitor Location' src='images/img_edit.png' onclick='OpenLocationChangeDialog(2," + DeviceId + ",\&quot;" + encodeURIComponent(LocationName) + "\&quot;);' />"

            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support || UserRole == enumUserRoleArr.Partner)
                AddCell(row, sLocation, 'siteOverview_cell', "", "", "center", "150px", "40px", "");
            else
                AddCell(row, RoomName, 'siteOverview_cell', "", "", "center", "150px", "40px", "");

            if (UserRole != enumUserRoleArr.Customer && UserRole != enumUserRoleArr.Maintenance) 
               AddCell(row, IsConfiguredinCore, "siteOverview_cell", "", "", "center", "100px", "40px", "");

            if (g_Bin == 0 || g_Bin === "" || g_Bin === 3 || g_Bin == 7)
                AddCell(row, sgood, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

            if (g_Bin == 1 || g_Bin === "" || g_Bin === 3 || g_Bin == 7)
                AddCell(row, s90DaysCell, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

            if (g_Bin == 2 || g_Bin === "" || g_Bin === 3 || g_Bin == 7)
                AddCell(row, s30DaysCell, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

            AddCell(row, LastSeenNetwork, "siteOverview_cell", "", "", "center", "100px", "40px", "");
            AddCell(row, ModelItem, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

            if (g_Bin == 3 || g_Bin === "" || g_Bin == 7)
                AddCell(row, offline, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

            if (g_Bin === "")
                AddCell(row, BatteryReplacementOn, 'siteOverview_cell', "", "", "center", "100px", "40px", "");                        

            // Feature #28732 LBI Data in device table (Internal Support+ only)
            if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Support) {

                // LBI Activity for spark line chart
                sSpanCtrlId = "dynamicInfrasparkline" + DeviceId;
                var sSpanCtrl = "<span id='" + sSpanCtrlId + "'>...</span>";
                AddCell(row, sSpanCtrl, "siteOverview_cell", "", "", "center", "110px", "40px", "");

                var sActivityUrl = "<a href=MonitorLBIActivityReport.aspx?qSiteId=" + siteid + "&qMonitorId=" + DeviceId + "&qExport=1><img style='cursor: pointer;' src='images/downloadfile.png' /></a>"
                AddCell(row, sActivityUrl, "siteOverview_cell", "", "", "center", "100px", "40px", "");
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
        AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 6, "", "center", "700px", "40px", "");
        sTbl.appendChild(row);
    }

    document.getElementById("divLoading").style.display = "none";

    if (isSearch == 0) {

        document.getElementById("btnExportExcel").disabled = false;

        if (nRootLength === 0) {
            document.getElementById("btnExportExcel").disabled = true;
        }
    }

    // Page Visit Tracking
    try {
        nType = $("#subHeader").text();

        siteName = $("#ctl00_ContentPlaceHolder1_lblsitename").text();

        PageVisitDetails(g_UserId, "Home - Monitor List", enumPageAction.View, nType + " list viewed in site - " + siteName);
    }
    catch (e) {

    }
}

function doInfraEnableButton(MaxPageCnt) {

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

var title = "";
var DType = "";

function OpenLocationChangeDialog(DeviceType, DeviceId, DeviceLocation) {

    DType = DeviceType;
    DeviceLocation = setundefined(DeviceLocation);

    if (DeviceLocation == "") 
        document.getElementById("trOldLocation").style.display = "none";    
    else 
        document.getElementById("trOldLocation").style.display = "";
    
    if (DeviceType == 2) {
        document.getElementById("txtDevid").style.display = "";
        document.getElementById("txtDeviceLocation").disabled = false;
        title = "Update Monitor Location";
    }
    else {
        document.getElementById("txtDevid").style.display = "";
        document.getElementById("txtDeviceLocation").disabled = false;
        title = "Update Star Location";
    }

    if (DeviceType == 2) 
        $('#lbldeviceid').text("Monitor Id:");    
    else 
        $('#lbldeviceid').text("Mac Id:");    

    $('#txtDevid').val(DeviceId);
    $('#txtDeviceLocation').val(decodeURIComponent(DeviceLocation));
    $('#txtOldDeviceLocation').val(decodeURIComponent(DeviceLocation));

    //Open Dialog
    $("#dialog-UpdateDeviceLocation").dialog({

        height: 280,
        width: 560,
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

//For Update Location
function SaveDeviceLocation() {

    var DeviceId = setundefined($('#txtDevid').val());
    var DeviceLocation = setundefined($('#txtDeviceLocation').val());

    if (DeviceLocation == "") {
        alert("Please enter a Location");
        return;
    }

    document.getElementById("divUpdateLocation").style.display = "";

    $.post("AjaxConnector.aspx?cmd=UpdateDeviceLocation",
    {
        Location_sid: SiteId,
        Location_DeviceType: DType,
        Location_DeviceId: DeviceId,
        DeviceLocation: DeviceLocation
    },
    function (data, status) {
        if (status == "success") {

            document.getElementById("divUpdateLocation").style.display = "none";

            document.getElementById("divLoading").style.display = "";

            CloseLocationDialogWindow();
        }
    });
}

function CloseLocationDialogWindow() {

    var currentpage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value;

    $('#dialog-UpdateDeviceLocation').dialog('close');

    if (DType == 2) {

        if (document.getElementById("txtSearchDeviceIds").value != "") {

            loadInfraInfoSearch(SiteId, "0", document.getElementById("ddSearchBin").value, document.getElementById("txtSearchDeviceIds").value);
        }
        else {
            doLoadInfrastructure(SiteId, Alertid, bin, currentpage, typeId);
        }
    }
    else {
        if (document.getElementById("txtSearchDeviceIds").value != "") {

            loadStarInfoSearch(SiteId, "0", document.getElementById("ddSearchBin").value, document.getElementById("txtSearchDeviceIds").value);
        }
        else {
            doLoadStar(SiteId, Alertid, bin, currentpage, typeId);
        }
    }
}

function CancelLocationdialog() {

    $('#dialog-UpdateDeviceLocation').dialog('close');
}
