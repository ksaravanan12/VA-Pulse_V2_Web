//*******************************************************************
//	Function Name	:	CreateXMLObj
//	Input			:	None
//	Description		:	Create XML Object For Ajax call
//*******************************************************************
function CreateDeviceXMLObj() {
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

//Conn Obj
var g_DeviceObj;
var g_DeviceGraphObj;
var g_Device10HrObj;

//Global Values for Graph & 10 Hour Data
var nDeviceType;
var nDeviceId;
var nSiteId;
//*******************************************************************
//	Function Name	:	DeviceList
//	Input			:	SiteId,DeviceType,DeviceId
//	Description		:	ajax call DeviceList
//*******************************************************************
function DeviceList(SiteId, DeviceType, DeviceId) {
    nSiteId = SiteId;
    nDeviceType = DeviceType;
    nDeviceId = DeviceId;

    g_DeviceObj = CreateDeviceXMLObj();

    if (g_DeviceObj != null) {
        g_DeviceObj.onreadystatechange = ajaxDeviceList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=DeviceList&sid=" + SiteId + "&DeviceType=" + DeviceType + "&DeviceId=" + DeviceId;

        if (GetBrowserType() == "isIE") {
            g_DeviceObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_DeviceObj.open("GET", DbConnectorPath, true);
        }
        g_DeviceObj.send(null);
    }
    return false;
}

//*******************************************************************
//	Function Name	:	ajaxDeviceList
//	Input			:	none
//	Description		:	Load Device List from ajax Response
//*******************************************************************
function ajaxDeviceList() {
    if (g_DeviceObj.readyState == 4) {
        if (g_DeviceObj.status == 200) {
            var sTbl, sTblLen;
            var sTblShip, sTblLenShip;
            var sTblWifi, sTblLenWifi;

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_DeviceObj.responseXML.documentElement);

            sTbl = document.getElementById('tblTagDetails');
            sTblProfile = document.getElementById('tblProfile');
            sTblWifiProfile = document.getElementById('tblWifiProfile');
            sTblTemperatureProfile = document.getElementById('tblTemperatureProfile');
            sTblBattery = document.getElementById('tblBattery');
            sTblStatus = document.getElementById('tblStatus');
            sTblShip = document.getElementById('tblShippingDetails');
            sTblWifi = document.getElementById('tblWiFiDetails');

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            sTblProfileLen = sTblProfile.rows.length;
            clearTableRows(sTblProfile, sTblProfileLen);

            sTblWifiProfileLen = sTblWifiProfile.rows.length;
            clearTableRows(sTblWifiProfile, sTblWifiProfileLen);

            sTblTemperatureProfileLen = sTblTemperatureProfile.rows.length;
            clearTableRows(sTblTemperatureProfile, sTblTemperatureProfileLen);

            sTblBatteryLen = sTblBattery.rows.length;
            clearTableRows(sTblBattery, sTblBatteryLen);

            sTblStatusLen = sTblStatus.rows.length;
            clearTableRows(sTblStatus, sTblStatusLen);

            sTblLenShip = sTblShip.rows.length;
            clearTableRows(sTblShip, sTblLenShip);

            sTbl.style.display = "none";
            sTblProfile.style.display = "none";
            sTblWifiProfile.style.display = "none";
            sTblTemperatureProfile.style.display = "none";
            sTblBattery.style.display = "none";
            sTblStatus.style.display = "none";
            sTblShip.style.display = "none";

            var dsRoot = g_DeviceObj.responseXML.documentElement;

            sTblShip.style.display = "";

            if (nDeviceType == 1)
                document.getElementById('tblPictureDetails').style.display = "";

            if (nDeviceType == 1) {
                LoadTagProfile(dsRoot, sTbl, sTblProfile, sTblWifiProfile, sTblTemperatureProfile, sTblBattery, sTblStatus, sTblShip);
            }
            else if (nDeviceType == 2) {
                LoadMonitorProfile(dsRoot, sTbl, sTblShip);
            }
            else if (nDeviceType == 3) {
                LoadStarProfile(dsRoot, sTbl, sTblShip);
            }

            document.getElementById('divLoading_TagDetails').style.display = "none";

            //Load Device Graph
            if (nDeviceType == 3) {
                g_DateRangeType = 1;
            }
            else {
                g_DateRangeType = 2;
            }

            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, "", 1);

            //Load 10 Hour Data
            Last10hrdata(nSiteId, nDeviceType, nDeviceId);
        }
    }
}

//*******************************************************************
//	Function Name	:	LoadTagProfile
//	Input			:	dsRoot,sTbl,sTblShip
//	Description		:	Load Tag Profile from ajax Response
//*******************************************************************
function LoadTagProfile(dsRoot, sTbl, sTblProfile, sTblWifiProfile, sTblTemperatureProfile, sTblBattery, sTblStatus, sTblShip) {
    var o_DeviceId = dsRoot.getElementsByTagName('TagId');
    var o_DeviceType = dsRoot.getElementsByTagName('TagType');
    var o_SWVersion = dsRoot.getElementsByTagName('SWVersion');
    var o_Firmware_Version = dsRoot.getElementsByTagName('Firmware_Version');
    var o_FirstSeen = dsRoot.getElementsByTagName('FirstSeen');
    var o_LastSeen = dsRoot.getElementsByTagName('LastSeen');
    var o_LocationDataReceived = dsRoot.getElementsByTagName('LocationDataReceived');
    var o_PageDataReceived = dsRoot.getElementsByTagName('PageDataReceived');
    var o_ModelItem = dsRoot.getElementsByTagName('ModelItem');
    var o_ShipDate = dsRoot.getElementsByTagName('ShipDate');
    var o_PoNumber = dsRoot.getElementsByTagName('PoNumber');
    var o_Profile = dsRoot.getElementsByTagName('Profile');
    var o_IRProfile = dsRoot.getElementsByTagName('IRProfile');
    var o_IRReportingTime = dsRoot.getElementsByTagName('IRReportingTime');
    var o_NoIRReportingTime = dsRoot.getElementsByTagName('NoIRReportingTime');
    var o_IRRXValue = dsRoot.getElementsByTagName('IRRXValue');
    var o_LFRegister = dsRoot.getElementsByTagName('LFRegister');
    var o_MotionSensorScanLogic = dsRoot.getElementsByTagName('MotionSensorScanLogic');
    var o_EnableFastPushbutton = dsRoot.getElementsByTagName('EnableFastPushbutton');
    var o_LFRX = dsRoot.getElementsByTagName('LFRX');
    var o_PagingProfile = dsRoot.getElementsByTagName('PagingProfile');
    var o_OperationMode = dsRoot.getElementsByTagName('OperationMode');
    var o_WiFiReportRates = dsRoot.getElementsByTagName('WiFiReportRates');
    var o_EnableWifiin900MHZ = dsRoot.getElementsByTagName('EnableWifiin900MHZ');
    var o_AlertDelay = dsRoot.getElementsByTagName('AlertDelay');
    var o_TagClass = dsRoot.getElementsByTagName('TagClass');
    var o_MinimumTemp = dsRoot.getElementsByTagName('MinimumTemp');
    var o_MaximumTemp = dsRoot.getElementsByTagName('MaximumTemp');
    var o_HighTemp = dsRoot.getElementsByTagName('HighTemp');
    var o_LowTemp = dsRoot.getElementsByTagName('LowTemp');
    var o_TemperatureReportRate = dsRoot.getElementsByTagName('TemperatureReportRate');
    var o_LocalAlert = dsRoot.getElementsByTagName('LocalAlert');
    var o_X2L = dsRoot.getElementsByTagName('X2L');
    var o_XL = dsRoot.getElementsByTagName('XL');
    var o_IPL = dsRoot.getElementsByTagName('IPL');
    var o_LongIROpen = dsRoot.getElementsByTagName('LongIROpen');
    var o_Probes = dsRoot.getElementsByTagName('Probes');
    var o_Probe1AlertMin = dsRoot.getElementsByTagName('Probe1AlertMin');
    var o_Probe1AlertMax = dsRoot.getElementsByTagName('Probe1AlertMax');
    var o_Probe2AlertMin = dsRoot.getElementsByTagName('Probe2AlertMin');
    var o_Probe2AlertMax = dsRoot.getElementsByTagName('Probe2AlertMax');
    var o_LessThen30Days = dsRoot.getElementsByTagName('LessThen30Days');
    var o_LessThen90Days = dsRoot.getElementsByTagName('LessThen90Days');
    var o_offline = dsRoot.getElementsByTagName('offline');

    var o_BatteryCapacity = dsRoot.getElementsByTagName('BatteryCapacity');
    var o_ActionRequired = dsRoot.getElementsByTagName('ActionRequired');
    var o_Voltage = dsRoot.getElementsByTagName('Voltage');
    var o_IRID = dsRoot.getElementsByTagName('IRID');
    var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
    var o_MonitorLastSeen = dsRoot.getElementsByTagName('MonitorLastSeen');

    var o_BuildingName = dsRoot.getElementsByTagName('BuildingName');
    var o_CampusName = dsRoot.getElementsByTagName('CampusName');
    var o_FloorName = dsRoot.getElementsByTagName('FloorName');
    var o_DoorAjarDetection = dsRoot.getElementsByTagName('DoorAjarDetection');
    var o_Probe1Temperature = dsRoot.getElementsByTagName('Probe1Temperature');
    var o_Probe2Temperature = dsRoot.getElementsByTagName('Probe2Temperature');
    var o_Probe2Temperature = dsRoot.getElementsByTagName('Probe2Temperature');
    var o_WiFiDataCount = dsRoot.getElementsByTagName('WiFiDataCount');
    var o_LastWiFiDataReceivedTime = dsRoot.getElementsByTagName('LastWiFiDataReceivedTime');
    var o_AlertType = dsRoot.getElementsByTagName('AlertType');
    var o_Temperature = dsRoot.getElementsByTagName('Temperature');
    var o_Humidity = dsRoot.getElementsByTagName('Humidity');
    var o_DeviceSubTypeId = dsRoot.getElementsByTagName('DeviceSubTypeId');
    var o_NewDeviceSubTypeId = dsRoot.getElementsByTagName('NewDeviceSubTypeId');

    var nRootLength = o_DeviceId.length;

    if (nRootLength > 0) {
        var DeviceId = '';
        if (o_DeviceId[0] != undefined)
            DeviceId = (o_DeviceId[0].textContent || o_DeviceId[0].innerText || o_DeviceId[0].text);

        var DeviceType = '';
        if (o_DeviceType[0] != undefined)
            DeviceType = (o_DeviceType[0].textContent || o_DeviceType[0].innerText || o_DeviceType[0].text);

        var Firmware_Version = '';
        if (o_Firmware_Version[0] != undefined)
            Firmware_Version = (o_Firmware_Version[0].textContent || o_Firmware_Version[0].innerText || o_Firmware_Version[0].text);

        var FirstSeen = '';
        if (o_FirstSeen[0] != undefined)
            FirstSeen = (o_FirstSeen[0].textContent || o_FirstSeen[0].innerText || o_FirstSeen[0].text);

        var LastSeen = '';
        if (o_LastSeen[0] != undefined)
            LastSeen = (o_LastSeen[0].textContent || o_LastSeen[0].innerText || o_LastSeen[0].text);

        var LocationDataReceived = '';
        if (o_LocationDataReceived[0] != undefined)
            LocationDataReceived = (o_LocationDataReceived[0].textContent || o_LocationDataReceived[0].innerText || o_LocationDataReceived[0].text);

        var PageDataReceived = '';
        if (o_PageDataReceived[0] != undefined)
            PageDataReceived = (o_PageDataReceived[0].textContent || o_PageDataReceived[0].innerText || o_PageDataReceived[0].text);

        var ModelItem = '';
        if (o_ModelItem[0] != undefined)
            ModelItem = (o_ModelItem[0].textContent || o_ModelItem[0].innerText || o_ModelItem[0].text);

        var ShipDate = '';
        if (o_ShipDate[0] != undefined)
            ShipDate = (o_ShipDate[0].textContent || o_ShipDate[0].innerText || o_ShipDate[0].text);

        var SWVersion = '';
        if (o_SWVersion[0] != undefined)
            SWVersion = (o_SWVersion[0].textContent || o_SWVersion[0].innerText || o_SWVersion[0].text);

        var PoNumber = '';
        if (o_PoNumber[0] != undefined)
            PoNumber = (o_PoNumber[0].textContent || o_PoNumber[0].innerText || o_PoNumber[0].text);

        var Profile = '';
        if (o_Profile[0] != undefined)
            Profile = (o_Profile[0].textContent || o_Profile[0].innerText || o_Profile[0].text);


        var IRProfile = '';
        if (o_IRProfile[0] != undefined)
            IRProfile = (o_IRProfile[0].textContent || o_IRProfile[0].innerText || o_IRProfile[0].text);

        var IRReportingTime = '';
        if (o_IRReportingTime[0] != undefined)
            IRReportingTime = (o_IRReportingTime[0].textContent || o_IRReportingTime[0].innerText || o_IRReportingTime[0].text);

        var NoIRReportingTime = '';
        if (o_NoIRReportingTime[0] != undefined)
            NoIRReportingTime = (o_NoIRReportingTime[0].textContent || o_NoIRReportingTime[0].innerText || o_NoIRReportingTime[0].text);


        var IRRXValue = '';
        if (o_IRRXValue[0] != undefined)
            IRRXValue = (o_IRRXValue[0].textContent || o_IRRXValue[0].innerText || o_IRRXValue[0].text);

        var LFRegister = '';
        if (o_LFRegister[0] != undefined)
            LFRegister = (o_LFRegister[0].textContent || o_LFRegister[0].innerText || o_LFRegister[0].text);

        var MotionSensorScanLogic = '';
        if (o_MotionSensorScanLogic[0] != undefined)
            MotionSensorScanLogic = (o_MotionSensorScanLogic[0].textContent || o_MotionSensorScanLogic[0].innerText || o_MotionSensorScanLogic[0].text);

        var EnableFastPushbutton = '';
        if (o_EnableFastPushbutton[0] != undefined)
            EnableFastPushbutton = (o_EnableFastPushbutton[0].textContent || o_EnableFastPushbutton[0].innerText || o_EnableFastPushbutton[0].text);

        var LFRX = '';
        if (o_LFRX[0] != undefined)
            LFRX = (o_LFRX[0].textContent || o_LFRX[0].innerText || o_LFRX[0].text);

        var PagingProfile = '';
        if (o_PagingProfile[0] != undefined)
            PagingProfile = (o_PagingProfile[0].textContent || o_PagingProfile[0].innerText || o_PagingProfile[0].text);

        var OperationMode = '';
        if (o_OperationMode[0] != undefined)
            OperationMode = (o_OperationMode[0].textContent || o_OperationMode[0].innerText || o_OperationMode[0].text);

        var WiFiReportRates = '';
        if (o_WiFiReportRates[0] != undefined)
            WiFiReportRates = (o_WiFiReportRates[0].textContent || o_WiFiReportRates[0].innerText || o_WiFiReportRates[0].text);

        var EnableWifiin900MHZ = '';
        if (o_EnableWifiin900MHZ[0] != undefined)
            EnableWifiin900MHZ = (o_EnableWifiin900MHZ[0].textContent || o_EnableWifiin900MHZ[0].innerText || o_EnableWifiin900MHZ[0].text);

        var AlertDelay = '';
        if (o_AlertDelay[0] != undefined)
            AlertDelay = (o_AlertDelay[0].textContent || o_AlertDelay[0].innerText || o_AlertDelay[0].text);

        var TagClass = '';
        if (o_TagClass[0] != undefined)
            TagClass = (o_TagClass[0].textContent || o_TagClass[0].innerText || o_TagClass[0].text);

        var MinimumTemp = '';
        if (o_MinimumTemp[0] != undefined)
            MinimumTemp = (o_MinimumTemp[0].textContent || o_MinimumTemp[0].innerText || o_MinimumTemp[0].text);

        var MaximumTemp = '';
        if (o_MaximumTemp[0] != undefined)
            MaximumTemp = (o_MaximumTemp[0].textContent || o_MaximumTemp[0].innerText || o_MaximumTemp[0].text);

        var HighTemp = '';
        if (o_HighTemp[0] != undefined)
            HighTemp = (o_HighTemp[0].textContent || o_HighTemp[0].innerText || o_HighTemp[0].text);

        var LowTemp = '';
        if (o_LowTemp[0] != undefined)
            LowTemp = (o_LowTemp[0].textContent || o_LowTemp[0].innerText || o_LowTemp[0].text);

        var TemperatureReportRate = '';
        if (o_TemperatureReportRate[0] != undefined)
            TemperatureReportRate = (o_TemperatureReportRate[0].textContent || o_TemperatureReportRate[0].innerText || o_TemperatureReportRate[0].text);

        var LocalAlert = '';
        if (o_LocalAlert[0] != undefined)
            LocalAlert = (o_LocalAlert[0].textContent || o_LocalAlert[0].innerText || o_LocalAlert[0].text);

        var X2L = '';
        if (o_X2L[0] != undefined)
            X2L = (o_X2L[0].textContent || o_X2L[0].innerText || o_X2L[0].text);

        var XL = '';
        if (o_XL[0] != undefined)
            XL = (o_XL[0].textContent || o_XL[0].innerText || o_XL[0].text);

        var IPL = '';
        if (o_IPL[0] != undefined)
            IPL = (o_IPL[0].textContent || o_IPL[0].innerText || o_IPL[0].text);

        var LongIROpen = '';
        if (o_LongIROpen[0] != undefined)
            LongIROpen = (o_LongIROpen[0].textContent || o_LongIROpen[0].innerText || o_LongIROpen[0].text);

        var Probes = '';
        if (o_Probes[0] != undefined)
            Probes = (o_Probes[0].textContent || o_Probes[0].innerText || o_Probes[0].text);

        var Probe1AlertMin = '';
        if (o_Probe1AlertMin[0] != undefined)
            Probe1AlertMin = (o_Probe1AlertMin[0].textContent || o_Probe1AlertMin[0].innerText || o_Probe1AlertMin[0].text);

        var Probe1AlertMax = '';
        if (o_Probe1AlertMax[0] != undefined)
            Probe1AlertMax = (o_Probe1AlertMax[0].textContent || o_Probe1AlertMax[0].innerText || o_Probe1AlertMax[0].text);

        var Probe2AlertMin = '';
        if (o_Probe2AlertMin[0] != undefined)
            Probe2AlertMin = (o_Probe2AlertMin[0].textContent || o_Probe2AlertMin[0].innerText || o_Probe2AlertMin[0].text);

        var Probe2AlertMax = '';
        if (o_Probe2AlertMax[0] != undefined)
            Probe2AlertMax = (o_Probe2AlertMax[0].textContent || o_Probe2AlertMax[0].innerText || o_Probe2AlertMax[0].text);

        var LessThen30Days = '';
        if (o_LessThen30Days[0] != undefined)
            LessThen30Days = (o_LessThen30Days[0].textContent || o_LessThen30Days[0].innerText || o_LessThen30Days[0].text);

        var LessThen90Days = '';
        if (o_LessThen90Days[0] != undefined)
            LessThen90Days = (o_LessThen90Days[0].textContent || o_LessThen90Days[0].innerText || o_LessThen90Days[0].text);

        var offline = '';
        if (o_offline[0] != undefined)
            offline = (o_offline[0].textContent || o_offline[0].innerText || o_offline[0].text);

        var BatteryCapacity = 0.0;
        if (o_BatteryCapacity[0] != undefined)
            BatteryCapacity = (o_BatteryCapacity[0].textContent || o_BatteryCapacity[0].innerText || o_BatteryCapacity[0].text);

        if (setundefined(BatteryCapacity) == "--")
            BatteryCapacity = " -- ";
        else
            BatteryCapacity = setundefined(BatteryCapacity) + "%";

        var Voltage = '';
        if (o_Voltage[0] != undefined)
            Voltage = (o_Voltage[0].textContent || o_Voltage[0].innerText || o_Voltage[0].text);

        var MonitorId = '';
        if (o_MonitorId[0] != undefined)
            MonitorId = (o_MonitorId[0].textContent || o_MonitorId[0].innerText || o_MonitorId[0].text);

        var MonitorLastSeen = '';
        if (o_MonitorLastSeen[0] != undefined)
            MonitorLastSeen = (o_MonitorLastSeen[0].textContent || o_MonitorLastSeen[0].innerText || o_MonitorLastSeen[0].text);

        var IRID = '';
        if (o_IRID[0] != undefined)
            IRID = (o_IRID[0].textContent || o_IRID[0].innerText || o_IRID[0].text);

        var ActionRequired = '';
        if (o_ActionRequired[0] != undefined)
            ActionRequired = (o_ActionRequired[0].textContent || o_ActionRequired[0].innerText || o_ActionRequired[0].text);

        var BuildingName = '';
        if (o_BuildingName[0] != undefined)
            BuildingName = (o_BuildingName[0].textContent || o_BuildingName[0].innerText || o_BuildingName[0].text);

        var CampusName = '';
        if (o_CampusName[0] != undefined)
            CampusName = (o_CampusName[0].textContent || o_CampusName[0].innerText || o_CampusName[0].text);

        var FloorName = '';
        if (o_FloorName[0] != undefined)
            FloorName = (o_FloorName[0].textContent || o_FloorName[0].innerText || o_FloorName[0].text);

        var DoorAjarDetection = '';
        if (o_DoorAjarDetection[0] != undefined)
            DoorAjarDetection = (o_DoorAjarDetection[0].textContent || o_DoorAjarDetection[0].innerText || o_DoorAjarDetection[0].text);

        var Probe1Temperature = '';
        if (o_Probe1Temperature[0] != undefined)
            Probe1Temperature = (o_Probe1Temperature[0].textContent || o_Probe1Temperature[0].innerText || o_Probe1Temperature[0].text);

        var Probe2Temperature = '';
        if (o_Probe2Temperature[0] != undefined)
            Probe2Temperature = (o_Probe2Temperature[0].textContent || o_Probe2Temperature[0].innerText || o_Probe2Temperature[0].text);

        var WiFiDataCount = '';
        if (o_WiFiDataCount[0] != undefined)
            WiFiDataCount = (o_WiFiDataCount[0].textContent || o_WiFiDataCount[0].innerText || o_WiFiDataCount[0].text);

        var LastWiFiDataReceivedTime = '';
        if (o_LastWiFiDataReceivedTime[0] != undefined)
            LastWiFiDataReceivedTime = (o_LastWiFiDataReceivedTime[0].textContent || o_LastWiFiDataReceivedTime[0].innerText || o_LastWiFiDataReceivedTime[0].text);

        var AlertType = '';
        if (o_AlertType[0] != undefined)
            AlertType = (o_AlertType[0].textContent || o_AlertType[0].innerText || o_AlertType[0].text);

        var Temperature = '';
        if (o_Temperature[0] != undefined)
            Temperature = (o_Temperature[0].textContent || o_Temperature[0].innerText || o_Temperature[0].text);

        var Humidity = '';
        if (o_Humidity[0] != undefined)
            Humidity = (o_Humidity[0].textContent || o_Humidity[0].innerText || o_Humidity[0].text);

        var g_tagDeviceSubType = '';
        if (o_DeviceSubTypeId[0] != undefined)
            g_tagDeviceSubType = (o_DeviceSubTypeId[0].textContent || o_DeviceSubTypeId[0].innerText || o_DeviceSubTypeId[0].text);

        var NewDeviceSubTypeId = '';
        if (o_NewDeviceSubTypeId[0] != undefined)
            NewDeviceSubTypeId = (o_NewDeviceSubTypeId[0].textContent || o_NewDeviceSubTypeId[0].innerText || o_NewDeviceSubTypeId[0].text);                    

        //Header
        row = document.createElement('tr');
        AddCell(row, "Type & Version", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTbl.appendChild(row);
        sTbl.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Profile", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblProfile.appendChild(row);
        sTblProfile.style.display = "";

        if (IsMMTag(NewDeviceSubTypeId) == "1") {
            row = document.createElement('tr');
            AddCell(row, "WiFi Profile", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
            sTblWifiProfile.appendChild(row);
            sTblWifiProfile.style.display = "";
        }

        if (IsEMTag(NewDeviceSubTypeId) == "1") {
            sTblTemperatureProfile.style.display = "";
            row = document.createElement('tr');
            AddCell(row, "Temperature Profile", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
            sTblTemperatureProfile.appendChild(row);
            sTblTemperatureProfile.style.display = "";
        }

        row = document.createElement('tr');
        AddCell(row, "Battery", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblBattery.appendChild(row);
        sTblBattery.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Status", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblStatus.appendChild(row);
        sTblStatus.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Shipping Info", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblShip.appendChild(row);
        sTblShip.style.display = "";

        //Datas
        var Bin = "";

        if (LessThen30Days == "1" && LessThen90Days == "0" && offline == "0")
            Bin = "Less Than 30 Days (LBI)";
        if (LessThen30Days == "0" && LessThen90Days == "1" && offline == "0")
            Bin = "Less Than 90 Days (Underwatch)";
        //if(LessThen30Days == "0" && LessThen90Days == "0" && offline == "1")
        if (offline == "1")
            Bin = "Offline";
        if (LessThen30Days == "0" && LessThen90Days == "0" && offline == "0")
            Bin = "Good";

        sTblProfile, sTblWifiProfile, sTblTemperatureProfile, sTblBattery, sTblStatus

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Firmware Version : </span>" + setundefined(Firmware_Version), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Tag Type : </span>" + setundefined(DeviceType), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        row = document.createElement('tr');

        if (g_tagDeviceSubType != 5 && g_tagDeviceSubType != 7) {
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Fast Push button : </span>" + setundefined(EnableFastPushbutton), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType == 1 || g_tagDeviceSubType == 2 || g_tagDeviceSubType == 9 || g_tagDeviceSubType == 8) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Profile : </span>" + setundefined(IRProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Motion Sensor Scan Logic : </span>" + setundefined(MotionSensorScanLogic), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_tagDeviceSubType == 3) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Profile : </span>" + setundefined(IRProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Washout Timer : </span>" + setundefined(TagClass), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType == 1 || g_tagDeviceSubType == 9 || g_tagDeviceSubType == 8) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR RX Profile: </span>" + setundefined(IRRXValue), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Tag Class : </span>" + setundefined(TagClass), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_tagDeviceSubType == 2) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR RX Profile: </span>" + setundefined(IRRXValue), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Long IR : </span>" + setundefined(LongIROpen), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_tagDeviceSubType == 4) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Profile : </span>" + setundefined(IRProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR RX Profile: </span>" + setundefined(IRRXValue), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType != 5 && g_tagDeviceSubType != 7) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Reporting Time : </span>" + setundefined(IRReportingTime), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>LF RX : </span>" + setundefined(LFRX), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>RF Reporting Time : </span>" + setundefined(NoIRReportingTime), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>LF Register : </span>" + setundefined(LFRegister), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType == 3 || g_tagDeviceSubType == 4) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Alert Delay : </span>" + setundefined(AlertDelay), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Paging Profile : </span>" + setundefined(PagingProfile), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            //AddCell(row,"<span class='DeviceList_DeviceDetailsDatasLabel'>Alert Type : </span>" + setundefined(AlertType),'DeviceList_rightBox_DeviceDetailsDatasText',"","","left","400px","25px","");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType == 5 || g_tagDeviceSubType == 9) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Local Alert : </span>" + setundefined(LocalAlert), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperationMode), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_tagDeviceSubType == 7) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperationMode), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Measurement Rate : </span>" + setundefined(TemperatureReportRate), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType == 9) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Enable High Accuracy : </span>" + setundefined(LocalAlert), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Door Ajar Detection : </span>" + setundefined(DoorAjarDetection), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Measurement Rate : </span>" + setundefined(TemperatureReportRate), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Paging Profile : </span>" + setundefined(PagingProfile), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        if (g_tagDeviceSubType == 5 || g_tagDeviceSubType == 7) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Paging Profile : </span>" + setundefined(PagingProfile), 'DeviceList_rightBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_tagDeviceSubType == 4) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Long IR : </span>" + setundefined(LongIROpen), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_tagDeviceSubType != 7 && g_tagDeviceSubType != 9 && g_tagDeviceSubType != 3 && g_tagDeviceSubType != 4) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Paging Profile : </span>" + setundefined(PagingProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Bin : </span>" + setundefined(Bin), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        //AddCell(row,"<span class='DeviceList_DeviceDetailsDatasLabel'>Battery Capacity : </span>" + setundefined(BatteryCapacity),'DeviceList_rightBox_DeviceDetailsDatasText',"","","left","400px","25px","");        
        //sTblBattery.appendChild(row);

        //row = document.createElement('tr');        
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Status : </span>" + setundefined(ActionRequired), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        //AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Battery Value : </span>" + setundefined(Voltage), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");        
        sTblBattery.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>First Seen : </span>" + setundefined(FirstSeen), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Last Seen : </span>" + setundefined(LastSeen), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblStatus.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Location Data Received : </span>" + setundefined(LocationDataReceived), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Page Data Received : </span>" + setundefined(PageDataReceived), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblStatus.appendChild(row);

        if (g_tagDeviceSubType == 2 || g_tagDeviceSubType == 4) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>WiFi Data Count : </span>" + setundefined(WiFiDataCount), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Last WiFi Data Received Time : </span>" + setundefined(LastWiFiDataReceivedTime), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblStatus.appendChild(row);
        }

        if (g_tagDeviceSubType != 5 && g_tagDeviceSubType != 7) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR ID : </span>" + setundefined(IRID), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Last Seen Monitor Id : </span>" + setundefined(MonitorId), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblStatus.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Monitor Last Seen : </span>" + setundefined(MonitorLastSeen), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblStatus.appendChild(row);
        }

        if (g_tagDeviceSubType == 7) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Humidity : </span>" + setundefined(Humidity), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Temperature : </span>" + setundefined(Temperature), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblStatus.appendChild(row);
        }
        else if (g_tagDeviceSubType == 5) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Temperature : </span>" + setundefined(Temperature), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblStatus.appendChild(row);
        }

        //WiFi
        if (IsMMTag(NewDeviceSubTypeId) == "1") {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperationMode), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Wifi in 900MHZ : </span>" + setundefined(EnableWifiin900MHZ), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblWifiProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>WiFi Report Rates : </span>" + setundefined(WiFiReportRates), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Building Name : </span>" + setundefined(BuildingName), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblWifiProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Campus Name : </span>" + setundefined(CampusName), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Floor Name : </span>" + setundefined(FloorName), 'DeviceList_rightBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblWifiProfile.appendChild(row);
        }

        //Temperature 
        if (IsEMTag(g_tagDeviceSubType) == "1") {

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Minimum Temp : </span>" + setundefined(MinimumTemp), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Maximum Temp : </span>" + setundefined(MaximumTemp), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);
                       
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>High Temp : </span>" + setundefined(HighTemp), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Low Temp : </span>" + setundefined(LowTemp), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Measurement Rate : </span>" + setundefined(TemperatureReportRate), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>XL : </span>" + setundefined(XL), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IPL : </span>" + setundefined(IPL), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>X2L : </span>" + setundefined(X2L), 'DeviceList_rightBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);            
      
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probes : </span>" + setundefined(Probes), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe1 Alert Min : </span>" + setundefined(Probe1AlertMin), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe1 Alert Max : </span>" + setundefined(Probe1AlertMax), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe2 Alert Min : </span>" + setundefined(Probe2AlertMin), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe2 Alert Max : </span>" + setundefined(Probe2AlertMax), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe1 Temperature : </span>" + setundefined(Probe1Temperature), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Probe2 Temperature : </span>" + setundefined(Probe2Temperature), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblTemperatureProfile.appendChild(row);
        }

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Model Item : </span>" + setundefined(ModelItem), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Ship Date : </span>" + setundefined(ShipDate), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblShip.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Software Version : </span>" + setundefined(SWVersion), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Po Number : </span>" + setundefined(PoNumber), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblShip.appendChild(row);
    }

    try {
        PageVisitDetails(g_UserId, "Home - Tag Device Detail", enumPageAction.View, "Tag Device detail viewed for DeviceId :" + nDeviceId + ", In Site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
    }
    catch (e) {

    }
}

//*******************************************************************
//	Function Name	:	LoadMonitorProfile
//	Input			:	dsRoot,sTbl,sTblShip
//	Description		:	Load Monitor Profile from ajax Response
//*******************************************************************
function LoadMonitorProfile(dsRoot, sTbl, sTblShip) {

    var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
    var o_DeviceType = dsRoot.getElementsByTagName('MonitorType');
    var o_Firmware_Version = dsRoot.getElementsByTagName('Firmware_Version');
    var o_FirstSeen = dsRoot.getElementsByTagName('FirstSeen');
    var o_LastSeen = dsRoot.getElementsByTagName('LastSeen');
    var o_LocationDataReceived = dsRoot.getElementsByTagName('LocationDataReceived');
    var o_PageDataReceived = dsRoot.getElementsByTagName('PageDataReceived');
    var o_ModelItem = dsRoot.getElementsByTagName('ModelItem');
    var o_ShipDate = dsRoot.getElementsByTagName('ShipDate');
    var o_PoNumber = dsRoot.getElementsByTagName('PoNumber');
    var o_Profile = dsRoot.getElementsByTagName('Profile');
    var o_IRProfile = dsRoot.getElementsByTagName('IRProfile');
    var o_PowerLevel = dsRoot.getElementsByTagName('PowerLevel');
    var o_RoomBleeding = dsRoot.getElementsByTagName('RoomBleeding');
    var o_NoiseLevel = dsRoot.getElementsByTagName('NoiseLevel');
    var o_Masking = dsRoot.getElementsByTagName('Masking');
    var o_MasterSlave = dsRoot.getElementsByTagName('MasterSlave');
    var o_SpecialProfile = dsRoot.getElementsByTagName('SpecialProfile');
    var o_OperatingMode = dsRoot.getElementsByTagName('OperatingMode');
    var o_Modes = dsRoot.getElementsByTagName('Modes');
    var o_AlertSupressionTime = dsRoot.getElementsByTagName('AlertSupressionTime');
    var o_LessThen30Days = dsRoot.getElementsByTagName('LessThen30Days');
    var o_LessThen90Days = dsRoot.getElementsByTagName('LessThen90Days');
    var o_offline = dsRoot.getElementsByTagName('offline');
    var o_RoomId = dsRoot.getElementsByTagName('RoomId');

    var o_BatteryCapacity = dsRoot.getElementsByTagName('BatteryCapacity');
    var o_ActionRequired = dsRoot.getElementsByTagName('ActionRequired');
    var o_SWVersion = dsRoot.getElementsByTagName('SWVersion');
    var o_Voltage = dsRoot.getElementsByTagName('BatteryValue');
    var o_BuildingName = dsRoot.getElementsByTagName('BuildingName');
    var o_CampusName = dsRoot.getElementsByTagName('CampusName');
    var o_FloorName = dsRoot.getElementsByTagName('FloorName');
    var o_WiFiDataCount = dsRoot.getElementsByTagName('WiFiDataCount');
    var o_LastWiFiDataReceivedTime = dsRoot.getElementsByTagName('LastWiFiDataReceivedTime');
    var o_DeviceSubTypeId = dsRoot.getElementsByTagName('DeviceSubTypeId');

    var nRootLength = o_DeviceId.length;

    if (nRootLength > 0) {
        var DeviceId = '';
        if (o_DeviceId[0] != undefined)
            DeviceId = (o_DeviceId[0].textContent || o_DeviceId[0].innerText || o_DeviceId[0].text);

        var RoomId = '';
        if (o_RoomId[0] != undefined)
            RoomId = (o_RoomId[0].textContent || o_RoomId[0].innerText || o_RoomId[0].text);

        var DeviceType = '';
        if (o_DeviceType[0] != undefined)
            DeviceType = (o_DeviceType[0].textContent || o_DeviceType[0].innerText || o_DeviceType[0].text);

        var Firmware_Version = '';
        if (o_Firmware_Version[0] != undefined)
            Firmware_Version = (o_Firmware_Version[0].textContent || o_Firmware_Version[0].innerText || o_Firmware_Version[0].text);

        var FirstSeen = '';
        if (o_FirstSeen[0] != undefined)
            FirstSeen = (o_FirstSeen[0].textContent || o_FirstSeen[0].innerText || o_FirstSeen[0].text);

        var LastSeen = '';
        if (o_LastSeen[0] != undefined)
            LastSeen = (o_LastSeen[0].textContent || o_LastSeen[0].innerText || o_LastSeen[0].text);

        var LocationDataReceived = '';
        if (o_LocationDataReceived[0] != undefined)
            LocationDataReceived = (o_LocationDataReceived[0].textContent || o_LocationDataReceived[0].innerText || o_LocationDataReceived[0].text);

        var PageDataReceived = '';
        if (o_PageDataReceived[0] != undefined)
            PageDataReceived = (o_PageDataReceived[0].textContent || o_PageDataReceived[0].innerText || o_PageDataReceived[0].text);

        var ModelItem = '';
        if (o_ModelItem[0] != undefined)
            ModelItem = (o_ModelItem[0].textContent || o_ModelItem[0].innerText || o_ModelItem[0].text);

        var ShipDate = '';
        if (o_ShipDate[0] != undefined)
            ShipDate = (o_ShipDate[0].textContent || o_ShipDate[0].innerText || o_ShipDate[0].text);

        var PoNumber = '';
        if (o_PoNumber[0] != undefined)
            PoNumber = (o_PoNumber[0].textContent || o_PoNumber[0].innerText || o_PoNumber[0].text);

        var Profile = '';
        if (o_Profile[0] != undefined)
            Profile = (o_Profile[0].textContent || o_Profile[0].innerText || o_Profile[0].text);

        var IRProfile = '';
        if (o_IRProfile[0] != undefined)
            IRProfile = (o_IRProfile[0].textContent || o_IRProfile[0].innerText || o_IRProfile[0].text);

        var PowerLevel = '';
        if (o_PowerLevel[0] != undefined)
            PowerLevel = (o_PowerLevel[0].textContent || o_PowerLevel[0].innerText || o_PowerLevel[0].text);

        var RoomBleeding = '';
        if (o_RoomBleeding[0] != undefined)
            RoomBleeding = (o_RoomBleeding[0].textContent || o_RoomBleeding[0].innerText || o_RoomBleeding[0].text);

        var NoiseLevel = '';
        if (o_NoiseLevel[0] != undefined)
            NoiseLevel = (o_NoiseLevel[0].textContent || o_NoiseLevel[0].innerText || o_NoiseLevel[0].text);

        var Masking = '';
        if (o_Masking[0] != undefined)
            Masking = (o_Masking[0].textContent || o_Masking[0].innerText || o_Masking[0].text);

        var MasterSlave = '';
        if (o_MasterSlave[0] != undefined)
            MasterSlave = (o_MasterSlave[0].textContent || o_MasterSlave[0].innerText || o_MasterSlave[0].text);

        var SpecialProfile = '';
        if (o_SpecialProfile[0] != undefined)
            SpecialProfile = (o_SpecialProfile[0].textContent || o_SpecialProfile[0].innerText || o_SpecialProfile[0].text);

        var OperatingMode = '';
        if (o_OperatingMode[0] != undefined)
            OperatingMode = (o_OperatingMode[0].textContent || o_OperatingMode[0].innerText || o_OperatingMode[0].text);

        var Modes = '';
        if (o_Modes[0] != undefined)
            Modes = (o_Modes[0].textContent || o_Modes[0].innerText || o_Modes[0].text);

        var AlertSupressionTime = '';
        if (o_AlertSupressionTime[0] != undefined)
            AlertSupressionTime = (o_AlertSupressionTime[0].textContent || o_AlertSupressionTime[0].innerText || o_AlertSupressionTime[0].text);

        var LessThen30Days = '';
        if (o_LessThen30Days[0] != undefined)
            LessThen30Days = (o_LessThen30Days[0].textContent || o_LessThen30Days[0].innerText || o_LessThen30Days[0].text);

        var LessThen90Days = '';
        if (o_LessThen90Days[0] != undefined)
            LessThen90Days = (o_LessThen90Days[0].textContent || o_LessThen90Days[0].innerText || o_LessThen90Days[0].text);

        var offline = '';
        if (o_offline[0] != undefined)
            offline = (o_offline[0].textContent || o_offline[0].innerText || o_offline[0].text);

        var BatteryCapacity = 0.0;
        if (o_BatteryCapacity[0] != undefined)
            BatteryCapacity = (o_BatteryCapacity[0].textContent || o_BatteryCapacity[0].innerText || o_BatteryCapacity[0].text);

        if (setundefined(BatteryCapacity) == "--")
            BatteryCapacity = " -- ";
        else
            BatteryCapacity = setundefined(BatteryCapacity) + "%";
        var ActionRequired = '';
        if (o_ActionRequired[0] != undefined)
            ActionRequired = (o_ActionRequired[0].textContent || o_ActionRequired[0].innerText || o_ActionRequired[0].text);

        var SWVersion = '';
        if (o_SWVersion[0] != undefined)
            SWVersion = (o_SWVersion[0].textContent || o_SWVersion[0].innerText || o_SWVersion[0].text);

        var Voltage = '';
        if (o_Voltage[0] != undefined)
            Voltage = (o_Voltage[0].textContent || o_Voltage[0].innerText || o_Voltage[0].text);

        var BuildingName = '';
        if (o_BuildingName[0] != undefined)
            BuildingName = (o_BuildingName[0].textContent || o_BuildingName[0].innerText || o_BuildingName[0].text);

        var CampusName = '';
        if (o_CampusName[0] != undefined)
            CampusName = (o_CampusName[0].textContent || o_CampusName[0].innerText || o_CampusName[0].text);

        var FloorName = '';
        if (o_FloorName[0] != undefined)
            FloorName = (o_FloorName[0].textContent || o_FloorName[0].innerText || o_FloorName[0].text);

        var WiFiDataCount = '';
        if (o_WiFiDataCount[0] != undefined)
            WiFiDataCount = (o_WiFiDataCount[0].textContent || o_WiFiDataCount[0].innerText || o_WiFiDataCount[0].text);

        var LastWiFiDataReceivedTime = '';
        if (o_LastWiFiDataReceivedTime[0] != undefined)
            LastWiFiDataReceivedTime = (o_LastWiFiDataReceivedTime[0].textContent || o_LastWiFiDataReceivedTime[0].innerText || o_LastWiFiDataReceivedTime[0].text);

        var g_infraDeviceSubType = '';
        if (o_DeviceSubTypeId[0] != undefined)
            g_infraDeviceSubType = (o_DeviceSubTypeId[0].textContent || o_DeviceSubTypeId[0].innerText || o_DeviceSubTypeId[0].text);

        //Header
        row = document.createElement('tr');
        AddCell(row, "Type & Version ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTbl.appendChild(row);
        sTbl.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Profile ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblProfile.appendChild(row);
        sTblProfile.style.display = "";

        if (g_infraDeviceSubType == 2) {
            row = document.createElement('tr');
            AddCell(row, "WiFi Profile ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
            sTblWifiProfile.appendChild(row);
            sTblWifiProfile.style.display = "";
        }

        row = document.createElement('tr');
        AddCell(row, "Battery ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblBattery.appendChild(row);
        sTblBattery.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Status ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblStatus.appendChild(row);
        sTblStatus.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Shipping Info ", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblShip.appendChild(row);
        sTblShip.style.display = "";

        //Datas
        var Bin = "";

        if (LessThen30Days == "1" && LessThen90Days == "0" && offline == "0")
            Bin = "Less Than 30 Days (LBI)";
        if (LessThen30Days == "0" && LessThen90Days == "1" && offline == "0")
            Bin = "Less Than 90 Days (Underwatch)";
        //if(LessThen30Days == "0" && LessThen90Days == "0" && offline == "1")
        if (offline == "1")
            Bin = "Offline";
        if (LessThen30Days == "0" && LessThen90Days == "0" && offline == "0")
            Bin = "Good";

        sTblProfile, sTblWifiProfile, sTblTemperatureProfile, sTblBattery, sTblStatus

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Firmware Version : </span>" + setundefined(Firmware_Version), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Monitor Type : </span>" + setundefined(DeviceType), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        if (g_infraDeviceSubType != 3 && g_infraDeviceSubType != 4) {

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Power Level : </span>" + setundefined(PowerLevel), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR Profile : </span>" + setundefined(IRProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>NoiseLevel : </span>" + setundefined(NoiseLevel), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>MasterSlave : </span>" + setundefined(MasterSlave), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Room Bleeding : </span>" + setundefined(RoomBleeding), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Super Sync/Marker : </span>" + setundefined(Masking), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>SpecialProfile : </span>" + setundefined(SpecialProfile), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperatingMode), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR ID : </span>" + setundefined(RoomId), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_infraDeviceSubType == 3) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Power Level : </span>" + setundefined(PowerLevel), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR ID : </span>" + setundefined(RoomId), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }
        else if (g_infraDeviceSubType == 4) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Profile : </span>" + setundefined(Profile), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Operating Mode : </span>" + setundefined(OperatingMode), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Modes : </span>" + setundefined(Modes), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Alert Supression Time : </span>" + setundefined(AlertSupressionTime), 'DeviceList_rightBox_DeviceDetailsDatasText', '', "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);

            row = document.createElement('tr');
            //AddCell(row,"<span class='DeviceList_DeviceDetailsDatasLabel'>LF Tx rate : </span>" + setundefined(Modes),'DeviceList_leftBox_DeviceDetailsDatasText',"","","left","400px","25px","");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IR ID : </span>" + setundefined(RoomId), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblProfile.appendChild(row);
        }

        //WiFi
        if (g_infraDeviceSubType == 2) {
            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>WiFi Data Count : </span>" + setundefined(WiFiDataCount), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Last WiFi Data Received Time : </span>" + setundefined(LastWiFiDataReceivedTime), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblWifiProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Building Name : </span>" + setundefined(BuildingName), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Campus Name : </span>" + setundefined(CampusName), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
            sTblWifiProfile.appendChild(row);

            row = document.createElement('tr');
            AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Floor Name : </span>" + setundefined(FloorName), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
            sTblWifiProfile.appendChild(row);
        }

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Bin : </span>" + setundefined(Bin), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        //AddCell(row,"<span class='DeviceList_DeviceDetailsDatasLabel'>Battery Capacity : </span>" + setundefined(BatteryCapacity),'DeviceList_rightBox_DeviceDetailsDatasText',"","","left","400px","25px","");
        //sTblBattery.appendChild(row);

        //row=document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Status : </span>" + setundefined(ActionRequired), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        //AddCell(row,"<span class='DeviceList_DeviceDetailsDatasLabel'>Battery Value : </span>" + setundefined(Voltage),'DeviceList_rightBox_DeviceDetailsDatasText',"","","left","400px","25px","");
        sTblBattery.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>First Seen : </span>" + setundefined(FirstSeen), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Location Data Received : </span>" + setundefined(LocationDataReceived), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblStatus.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Last Seen : </span>" + setundefined(LastSeen), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Page Data Received : </span>" + setundefined(PageDataReceived), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblStatus.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Model Item : </span>" + setundefined(ModelItem), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Ship Date : </span>" + setundefined(ShipDate), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblShip.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Software Version : </span>" + setundefined(SWVersion), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Po Number : </span>" + setundefined(PoNumber), 'DeviceList_rightBox_DeviceDetailsDatasText', 2, "", "left", "800px", "25px", "");
        sTblShip.appendChild(row);
    }

    try {
        PageVisitDetails(g_UserId, "Home - Monitor Device Detail", enumPageAction.View, "Monitor Device detail viewed for DeviceId :" + nDeviceId + ", In Site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
    }
    catch (e) {

    }
}

var g_StarId;

//*******************************************************************
//	Function Name	:	LoadStarProfile
//	Input			:	dsRoot,sTbl
//	Description		:	Load Star Profile from ajax Response
//*******************************************************************
function LoadStarProfile(dsRoot, sTbl, sTblShip) {
    var o_MacId = dsRoot.getElementsByTagName('MACId');
    var o_StarType = dsRoot.getElementsByTagName('StarType');
    var o_DHCP = dsRoot.getElementsByTagName('DHCP');
    var o_SaveSettings = dsRoot.getElementsByTagName('SaveSettings');
    var o_StaticIP = dsRoot.getElementsByTagName('StaticIP');
    var o_Subnet = dsRoot.getElementsByTagName('Subnet');
    var o_Gateway = dsRoot.getElementsByTagName('Gateway');
    var o_TimeServerIP = dsRoot.getElementsByTagName('TimeServerIP');
    var o_ServerIP = dsRoot.getElementsByTagName('ServerIP');
    var o_PagingServerIP = dsRoot.getElementsByTagName('PagingServerIP');
    var o_LocationServerIP1 = dsRoot.getElementsByTagName('LocationServerIP1');
    var o_LocationServerIP2 = dsRoot.getElementsByTagName('LocationServerIP2');
    var o_StarId = dsRoot.getElementsByTagName('StarId');
    var o_IPAddr = dsRoot.getElementsByTagName('IPAddr');
    var o_StarName = dsRoot.getElementsByTagName('DeviceName');
    var o_SuperSyncProfile = dsRoot.getElementsByTagName('SuperSyncProfile');
    var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
    var o_Version = dsRoot.getElementsByTagName('Version');
    var o_ModelItem = dsRoot.getElementsByTagName('ModelItem');
    var o_ShipDate = dsRoot.getElementsByTagName('ShipDate');
    var o_PoNumber = dsRoot.getElementsByTagName('PoNumber');
    var o_SWVersion = dsRoot.getElementsByTagName('SWVersion');

    var nRootLength = o_MacId.length;

    if (nRootLength > 0) {
        var MacId = '';
        if (o_MacId[0] != undefined)
            MacId = (o_MacId[0].textContent || o_MacId[0].innerText || o_MacId[0].text);

        var StarType = '';
        if (o_StarType[0] != undefined)
            StarType = (o_StarType[0].textContent || o_StarType[0].innerText || o_StarType[0].text);

        var DHCP = '';
        if (o_DHCP[0] != undefined)
            DHCP = (o_DHCP[0].textContent || o_DHCP[0].innerText || o_DHCP[0].text);

        var SaveSettings = '';
        if (o_SaveSettings[0] != undefined)
            SaveSettings = (o_SaveSettings[0].textContent || o_SaveSettings[0].innerText || o_SaveSettings[0].text);

        var StaticIP = '';
        if (o_StaticIP[0] != undefined)
            StaticIP = (o_StaticIP[0].textContent || o_StaticIP[0].innerText || o_StaticIP[0].text);

        var Subnet = '';
        if (o_Subnet[0] != undefined)
            Subnet = (o_Subnet[0].textContent || o_Subnet[0].innerText || o_Subnet[0].text);

        var Gateway = '';
        if (o_Gateway[0] != undefined)
            Gateway = (o_Gateway[0].textContent || o_Gateway[0].innerText || o_Gateway[0].text);

        var TimeServerIP = '';
        if (o_TimeServerIP[0] != undefined)
            TimeServerIP = (o_TimeServerIP[0].textContent || o_TimeServerIP[0].innerText || o_TimeServerIP[0].text);

        var ServerIP = '';
        if (o_ServerIP[0] != undefined)
            ServerIP = (o_ServerIP[0].textContent || o_ServerIP[0].innerText || o_ServerIP[0].text);

        var PagingServerIP = '';
        if (o_PagingServerIP[0] != undefined)
            PagingServerIP = (o_PagingServerIP[0].textContent || o_PagingServerIP[0].innerText || o_PagingServerIP[0].text);

        var LocationServerIP1 = '';
        if (o_LocationServerIP1[0] != undefined)
            LocationServerIP1 = (o_LocationServerIP1[0].textContent || o_LocationServerIP1[0].innerText || o_LocationServerIP1[0].text);

        var LocationServerIP2 = '';
        if (o_LocationServerIP2[0] != undefined)
            LocationServerIP2 = (o_LocationServerIP2[0].textContent || o_LocationServerIP2[0].innerText || o_LocationServerIP2[0].text);

        var LocationServerIP2 = '';
        if (o_LocationServerIP2[0] != undefined)
            LocationServerIP2 = (o_LocationServerIP2[0].textContent || o_LocationServerIP2[0].innerText || o_LocationServerIP2[0].text);

        var StarId = '';
        if (o_StarId[0] != undefined)
            StarId = (o_StarId[0].textContent || o_StarId[0].innerText || o_StarId[0].text);
        g_StarId = StarId;
        var IPAddr = '';
        if (o_IPAddr[0] != undefined)
            IPAddr = (o_IPAddr[0].textContent || o_IPAddr[0].innerText || o_IPAddr[0].text);

        var StarName = '';
        if (o_StarName[0] != undefined)
            StarName = (o_StarName[0].textContent || o_StarName[0].innerText || o_StarName[0].text);

        var SuperSyncProfile = '';
        if (o_SuperSyncProfile[0] != undefined)
            SuperSyncProfile = (o_SuperSyncProfile[0].textContent || o_SuperSyncProfile[0].innerText || o_SuperSyncProfile[0].text);

        var LockedStarId = '';
        if (o_LockedStarId[0] != undefined)
            LockedStarId = (o_LockedStarId[0].textContent || o_LockedStarId[0].innerText || o_LockedStarId[0].text);

        var Version = '';
        if (o_Version[0] != undefined)
            Version = (o_Version[0].textContent || o_Version[0].innerText || o_Version[0].text);

        var ModelItem = '';
        if (o_ModelItem[0] != undefined)
            ModelItem = (o_ModelItem[0].textContent || o_ModelItem[0].innerText || o_ModelItem[0].text);

        var ShipDate = '';
        if (o_ShipDate[0] != undefined)
            ShipDate = (o_ShipDate[0].textContent || o_ShipDate[0].innerText || o_ShipDate[0].text);

        var SWVersion = '';
        if (o_SWVersion[0] != undefined)
            SWVersion = (o_SWVersion[0].textContent || o_SWVersion[0].innerText || o_SWVersion[0].text);

        var PoNumber = '';
        if (o_PoNumber[0] != undefined)
            PoNumber = (o_PoNumber[0].textContent || o_PoNumber[0].innerText || o_PoNumber[0].text);

        //Header
        row = document.createElement('tr');
        AddCell(row, "Type & Version", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTbl.appendChild(row);
        sTbl.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Profile", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblProfile.appendChild(row);
        sTblProfile.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "Status", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblStatus.appendChild(row);
        sTblStatus.style.display = "";

        //Datas
        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Firmware Version : </span>" + setundefined(Version), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Star Type : </span>" + setundefined(StarType), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTbl.appendChild(row);

        //Datas
        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Mac Id : </span>" + setundefined(MacId), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Star Name : </span>" + setundefined(StarName), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Star Id : </span>" + setundefined(StarId), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IP Mode : </span>" + setundefined(DHCP), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>StaticIP : </span>" + setundefined(StaticIP), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Subnet : </span>" + setundefined(Subnet), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Gateway : </span>" + setundefined(Gateway), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Timing Server IP : </span>" + setundefined(TimeServerIP), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Super Sync Profile : </span>" + setundefined(SuperSyncProfile), 'DeviceList_leftBox_DeviceDetailsDatasText', "2", "", "left", "400px", "25px", "");
        sTblProfile.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>IP Address : </span>" + setundefined(IPAddr), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Locked Star Id : </span>" + setundefined(LockedStarId), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblStatus.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "Shipping Info", 'siteOverview_TopLeft_Box_DeviceDetailsHeaderText', 2, "", "left", "800px", "30px", "");
        sTblShip.appendChild(row);
        sTblShip.style.display = "";

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Model Item : </span>" + setundefined(ModelItem), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Ship Date : </span>" + setundefined(ShipDate), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblShip.appendChild(row);

        row = document.createElement('tr');
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Software Version : </span>" + setundefined(SWVersion), 'DeviceList_leftBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        AddCell(row, "<span class='DeviceList_DeviceDetailsDatasLabel'>Po Number : </span>" + setundefined(PoNumber), 'DeviceList_rightBox_DeviceDetailsDatasText', "", "", "left", "400px", "25px", "");
        sTblShip.appendChild(row);
    }

    try {
        PageVisitDetails(g_UserId, "Home - Star Device Detail", enumPageAction.View, "Star Device detail viewed for DeviceId :" + nDeviceId + ", In Site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
    }
    catch (e) {

    }
}


//*********************************************************
//	Function Name	:	SetFromToDate
//	Input			:	Type
//	Description		:	Set From/To Index 
//*********************************************************
var g_StartDate;
var g_EndDate;

var g_Curr = 1;
var g_Next = 2;
var g_Prev = 3;

var g_StartIdx = 0;
var g_EndIdx = 0;
var g_Length = 20;

var g_dsRoot;
var g_DateRangeType = 1;

var g_HrFromTime;
var g_HrToTime;

var g_WeekFrmDate;
var g_WeekToDate;

var g_MnthFrmDate;
var g_MnthToDate;

function SetFromToDate(Type) {
    if (Type == g_Curr) {
        g_StartIdx = 0;
        g_EndIdx = 20;
    }

    if (Type == g_Prev) {
        g_StartIdx = g_StartIdx + 20;
        g_EndIdx = g_EndIdx + 20;
    }

    if (Type == g_Next) {
        g_StartIdx = g_StartIdx - 20;
        g_EndIdx = g_EndIdx - 20;
    }
}

//*********************************************************
//	Function Name	:	ShowPreviousNextGraph
//	Input			:	Type
//	Description		:	ajax Call ShowPreviousNextGraph
//*********************************************************
function ShowPreviousNextGraph(Type) {
    if (g_DateRangeType == 1) {
        if (nDeviceType == 3) {
            LoadStarDeviceGraph(g_dsRoot, Type);
        }
        else if (Type == 2) {
            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, g_HrToTime, Type);
        }
        else if (Type == 3) {
            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, g_HrFromTime, Type);
        }
    }
    else if (g_DateRangeType == 2) {
        if (nDeviceType == 3) {
            LoadStarDeviceGraph(g_dsRoot, Type);
        }
        else {
            LoadDeviceGraph(g_dsRoot, Type);
        }
    }
    else if (g_DateRangeType == 3) {
        if (Type == 2) {
            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, g_WeekToDate, Type);
        }
        else if (Type == 3) {
            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, g_WeekFrmDate, Type);
        }
    }
    else if (g_DateRangeType == 4) {
        if (Type == 2) {
            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, g_MnthToDate, Type);
        }
        else if (Type == 3) {
            DeviceGraph(nSiteId, nDeviceType, nDeviceId, g_DateRangeType, g_MnthFrmDate, Type);
        }
    }
}

//*********************************************************
//	Function Name	:	ShowDateRangeGraph
//	Input			:	DateRangeType
//	Description		:	ajax Call ShowDateRangeGraph
//*********************************************************
function ShowDateRangeGraph(DateRangeType) {
    g_DateRangeType = DateRangeType;
    DeviceGraph(nSiteId, nDeviceType, nDeviceId, DateRangeType, "", 1);
}

//*********************************************************
//	Function Name	:	DeviceGraph
//	Input			:	SiteId,DeviceType,DeviceId,DateRangeType,FromDate,PgType
//	Description		:	ajax Call DeviceGraph
//*********************************************************
function DeviceGraph(SiteId, DeviceType, DeviceId, DateRangeType, FromDate, PgType) {
    g_DeviceGraphObj = CreateDeviceXMLObj();

    document.getElementById("divLoading_Graph").style.display = "";

    document.getElementById("Div_MSStackedColumn2D").innerHTML = "";

    setRangeLable("lblDeviceDetails_DateType", g_DateRangeType);

    if (g_DeviceGraphObj != null) {
        g_DeviceGraphObj.onreadystatechange = ajaxDeviceGraph;

        DbConnectorPath = "AjaxConnector.aspx?cmd=DeviceGraph&sid=" + SiteId + "&DeviceType=" + DeviceType + "&DeviceId=" + DeviceId + "&DateRngType=" + DateRangeType + "&FromDate=" + FromDate + "&PgType=" + PgType;

        if (GetBrowserType() == "isIE") {
            g_DeviceGraphObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_DeviceGraphObj.open("GET", DbConnectorPath, true);
        }
        g_DeviceGraphObj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxDeviceGraph
//	Input			:	None
//	Description		:	Load Device Graph from ajax Response
//*********************************************************
function ajaxDeviceGraph() {
    if (g_DeviceGraphObj.readyState == 4) {
        if (g_DeviceGraphObj.status == 200) {
            var sTbl, sTblLen;

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblWiFiDetails');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblWiFiDetails');
            }
            sTblLen = sTbl.rows.length;
            //clearTableRows(sTbl,sTblLen);            

            var dsRoot = g_DeviceGraphObj.responseXML.documentElement;

            g_dsRoot = dsRoot;
            if (nDeviceType == 3) {
                LoadStarDeviceGraph(dsRoot, g_Curr);
            }
            else {
                LoadDeviceGraph(dsRoot, g_Curr);
            }

            document.getElementById('divLoading_Graph').style.display = "none";

            //Load 10 Hour Data
            // Last10hrdata(nSiteId,nDeviceType,nDeviceId);
        }
    }
}

var IsLBIADCChecked = false;
var IsLBIDiffChecked = false;

function showHideLBIADC() {
    IsLBIDiffChecked = false;

    if (document.getElementById('chkIsShowLBIADC').checked) {
        IsLBIADCChecked = true;
    }
    else {
        IsLBIADCChecked = false;
    }

    document.getElementById("ChkIsShowLBIDiff").checked = false;
    LoadDeviceGraph(g_dsRoot, g_Curr);
}

function showHideLBIDiff() {
    IsLBIADCChecked = false;

    if (document.getElementById('ChkIsShowLBIDiff').checked) {
        IsLBIDiffChecked = true;
    }
    else {
        IsLBIDiffChecked = false;
    }

    document.getElementById("chkIsShowLBIADC").checked = false;
    LoadDeviceGraph(g_dsRoot, g_Curr);
}

//*********************************************************
//	Function Name	:	LoadDeviceGraph
//	Input			:	dsRoot, Type
//	Description		:	Make XML String for Tag & Monitor Graph from ajax Response
//*********************************************************
function LoadDeviceGraph(dsRoot, Type) {

    if (nDeviceType == 2) {
        if (g_DateRangeType == 1) {
            document.getElementById("ctl00_ContentPlaceHolder1_LblLBIDiff").style.display = "";
            document.getElementById("ChkIsShowLBIDiff").style.display = "";
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder1_LblLBIDiff").style.display = "none";
            document.getElementById("ChkIsShowLBIDiff").style.display = "none";
        }
    }
    else {
        document.getElementById("ctl00_ContentPlaceHolder1_LblLBIDiff").style.display = "";
        document.getElementById("ChkIsShowLBIDiff").style.display = "";
    }

    if (dsRoot != null) {
        if (g_DateRangeType == 1) {
            GenerateHourlyXML(dsRoot, Type);
        }
        if (g_DateRangeType == 2) {
            GenerateDailyXML(dsRoot, Type);
        }
        else if (g_DateRangeType == 3) {
            GenerateWeeklyXML(dsRoot, Type);
        }
        else if (g_DateRangeType == 4) {
            GenerateMonthlyXML(dsRoot, Type);
        }

        document.getElementById("divLoading_Graph").style.display = "none";
    }
}

function GenerateHourlyXML(dsRoot, Type) {
    var nRootLength;
    var n;
    var nMaxLbiVal = 0;
    var nDateVal = 1;

    var sCurrDate;
    var CurrDate;

    var sCategory = "";
    var sLocationCount = "";
    var sPagingCount = "";
    var sLbiVal = "";
    var sXML = "";
    var strGraphData = "";
    var sWifiCount = "";
    var sTriggerCount = "";
    var o_BatteryValue;
    var o_TriggerCount;

    var o_SiteId = dsRoot.getElementsByTagName('SiteId');
    var o_ActivityDate = dsRoot.getElementsByTagName('ReceivedTime');
    var o_LocationCount = dsRoot.getElementsByTagName('LocationDataReceived');
    var o_PagingCount = dsRoot.getElementsByTagName('PageDataReceived');
    var o_WifiCount = dsRoot.getElementsByTagName('WiFiDataCount');
    var o_bShowLBIADC = dsRoot.getElementsByTagName('bShowLBIADC');

    if (IsLBIDiffChecked == true) {
        o_BatteryValue = dsRoot.getElementsByTagName('LBIDiff');
    }
    else if (IsLBIADCChecked == true) {
        o_BatteryValue = dsRoot.getElementsByTagName('ADCValue');
    }
    else {
        o_BatteryValue = dsRoot.getElementsByTagName('LBIValue');
    }

    if (nDeviceType == 2) {
        o_TriggerCount = dsRoot.getElementsByTagName('TriggerCount');
    }

    nRootLength = o_SiteId.length;
    n = nRootLength - 1;

    if (nRootLength > 0) {
        g_EndDate = setundefined(o_ActivityDate[0].textContent || o_ActivityDate[0].innerText || o_ActivityDate[0].text);
        g_StartDate = setundefined(o_ActivityDate[n].textContent || o_ActivityDate[n].innerText || o_ActivityDate[n].text);
        g_HrToTime = g_EndDate;
        g_HrFromTime = g_StartDate;
    }

    SetFromToDate(Type);

    //Max Lbi Val
    if (nDeviceType == 1) {
        nMaxLbiVal = 4;
    }
    else if (nDeviceType == 2) {
        nMaxLbiVal = 10;
    }

    //Disable Next Prev
    doGraphEnableButton(nRootLength, true);

    //Graph XML
    if (nRootLength > 0) {
        var bShowLBIADC = setundefined(o_bShowLBIADC[0].textContent || o_bShowLBIADC[0].innerText || o_bShowLBIADC[0].text);

        if (IsLBIDiffChecked == true) {
            sLegend = "LBI Diff";
        }
        else if (IsLBIADCChecked == true || bShowLBIADC == "True") {
            sLegend = "ADC Value";
        }
        else {
            sLegend = "Battery Value";
        }

        for (var i = 0; i <= nRootLength - 1; i++) {
            var ActivityDate = setundefined(o_ActivityDate[i].textContent || o_ActivityDate[i].innerText || o_ActivityDate[i].text);
            var LocationCount = setundefined(o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text);
            var PagingCount = setundefined(o_PagingCount[i].textContent || o_PagingCount[i].innerText || o_PagingCount[i].text);
            var BatteryValue = setundefined(o_BatteryValue[i].textContent || o_BatteryValue[i].innerText || o_BatteryValue[i].text);
            var WifiCount = setundefined(o_WifiCount[i].textContent || o_WifiCount[i].innerText || o_WifiCount[i].text);
            var TriggerCount;

            if (nDeviceType == 2) {
                TriggerCount = setundefined(o_TriggerCount[i].textContent || o_TriggerCount[i].innerText || o_TriggerCount[i].text);
            }

            if (WifiCount == 0) {
                WifiCount = "";
            }

            sCategory = "<category label='" + ActivityDate + "' />" + sCategory;
            sLocationCount = "<set value='" + LocationCount + "' />" + sLocationCount;
            sPagingCount = "<set value='" + PagingCount + "' />" + sPagingCount;

            if (BatteryValue == 0)
                sLbiVal = "<set value='' />" + sLbiVal;
            else
                sLbiVal = "<set value='" + BatteryValue + "' />" + sLbiVal;

            sWifiCount = "<set value='" + WifiCount + "' />" + sWifiCount;

            if (nDeviceType == 2) {
                sTriggerCount = "<set value='" + TriggerCount + "' />" + sTriggerCount;
            }

            if (BatteryValue > nMaxLbiVal) {
                nMaxLbiVal = BatteryValue;
            }
        }

        sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
        sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";
        sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
        sLbiVal = "<lineSet seriesName='" + sLegend + "' color='F6BD0F' anchorSides='10' anchorBorderColor='F6BD0F' showValues='1'>" + sLbiVal + "</lineSet>";
        sWifiCount = "<dataSet seriesName='Wifi Data Count' color='D15800' showValues='1'>" + sWifiCount + "</dataSet>";

        if (nDeviceType == 2) {
            sTriggerCount = "<lineSet seriesName='Trigger Count' color='EB71C3' anchorSides='10' anchorBorderColor='EB71C3' showValues='1'>" + sTriggerCount + "</lineSet>"
        }

        MakeChart(strGraphData, sCategory, sPagingCount, sLocationCount, sLbiVal, nMaxLbiVal, sWifiCount, sTriggerCount);
    }
    else {
        MakeChart("", sCategory, "", "", "", 0, "", "");
    }
}

//*********************************************************
//	Function Name	:	GenerateDailyXML
//	Input			:	dsRoot, Type
//	Description		:	Make XML String for Tag & Monitor Graph for Daily from ajax Response
//*********************************************************
function GenerateDailyXML(dsRoot, Type) {

    if (document.getElementById("trLBIDiff"))
        document.getElementById("trLBIDiff").style.display = "";

    var nRootLength;
    var n;
    var nMaxLbiVal = 0;
    var nDateVal = 1;

    var sCurrDate;
    var CurrDate;

    var sCategory = "";
    var sLocationCount = "";
    var sPagingCount = "";
    var sWifiCount = "";
    var sLbiVal = "";
    var sXML = "";
    var strGraphData = "";
    var sTriggerCount = "";
    var o_BatteryValue;
    var sLegend = "";
    var o_TriggerCount;

    var o_SiteId = dsRoot.getElementsByTagName('SiteId');
    var o_ActivityDate = dsRoot.getElementsByTagName('ActivityDate');
    var o_LocationCount = dsRoot.getElementsByTagName('LocationCount');
    var o_PagingCount = dsRoot.getElementsByTagName('PagingCount');
    var o_bShowLBIADC = dsRoot.getElementsByTagName('bShowLBIADC');
    var o_WifiCount = dsRoot.getElementsByTagName('WiFiDataCount');

    if (nDeviceType == 2)
        o_TriggerCount = dsRoot.getElementsByTagName('TriggerCount');

    if (nDeviceType == 1) {

        if (IsLBIDiffChecked == true) {
            o_BatteryValue = dsRoot.getElementsByTagName('LBIDiff');
        }
        else if (IsLBIADCChecked == true) {
            o_BatteryValue = dsRoot.getElementsByTagName('ADCValue');
        }
        else {
            o_BatteryValue = dsRoot.getElementsByTagName('BatteryValue');
        }
    }
    else if (nDeviceType == 2) {

        if (IsLBIADCChecked == true) {
            o_BatteryValue = dsRoot.getElementsByTagName('ADCValue');
        }
        else {
            o_BatteryValue = dsRoot.getElementsByTagName('Lbi');
        }
    }

    nRootLength = o_SiteId.length;
    n = nRootLength - 1;

    if (nRootLength > 0) {
        g_EndDate = setundefined(o_ActivityDate[0].textContent || o_ActivityDate[0].innerText || o_ActivityDate[0].text);
        g_StartDate = setundefined(o_ActivityDate[n].textContent || o_ActivityDate[n].innerText || o_ActivityDate[n].text);
    }

    SetFromToDate(Type);

    //Max Lbi Val
    if (nDeviceType == 1) {
        nMaxLbiVal = 4;
    }
    else if (nDeviceType == 2) {
        nMaxLbiVal = 10;
    }

    //Disable Next Prev
    doGraphEnableButton(nRootLength, true);

    //Graph XML
    if (nRootLength > 0) {

        var bShowLBIADC = setundefined(o_bShowLBIADC[0].textContent || o_bShowLBIADC[0].innerText || o_bShowLBIADC[0].text);

        if (IsLBIDiffChecked == true) {
            sLegend = "LBI Diff";
        }
        else if (IsLBIADCChecked == true || bShowLBIADC == "True") {
            sLegend = "ADC Value";
        }
        else {
            sLegend = "Battery Value";
        }

        for (var i = g_StartIdx; i <= g_EndIdx - 1; i++) {
            if (i <= nRootLength - 1) {
                var ActivityDate = setundefined(o_ActivityDate[i].textContent || o_ActivityDate[i].innerText || o_ActivityDate[i].text);
                var LocationCount = setundefined(o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text);
                var PagingCount = setundefined(o_PagingCount[i].textContent || o_PagingCount[i].innerText || o_PagingCount[i].text);
                var WifiCount = setundefined(o_WifiCount[i].textContent || o_WifiCount[i].innerText || o_WifiCount[i].text);
                var BatteryValue = setundefined(o_BatteryValue[i].textContent || o_BatteryValue[i].innerText || o_BatteryValue[i].text);
                var TriggerCount;

                if (nDeviceType == 2) {
                    TriggerCount = setundefined(o_TriggerCount[i].textContent || o_TriggerCount[i].innerText || o_TriggerCount[i].text);
                }

                if (WifiCount == 0) {
                    WifiCount = "";
                }

                sCategory = "<category label='" + ActivityDate + "' />" + sCategory;
                sLocationCount = "<set value='" + LocationCount + "' />" + sLocationCount;
                sPagingCount = "<set value='" + PagingCount + "' />" + sPagingCount;
                sWifiCount = "<set value='" + WifiCount + "' />" + sWifiCount;

                if (nDeviceType == 2) {
                    sTriggerCount = "<set value='" + TriggerCount + "' />" + sTriggerCount;
                }

                if (BatteryValue == 0)
                    sLbiVal = "<set value='' />" + sLbiVal;
                else
                    sLbiVal = "<set value='" + BatteryValue + "' />" + sLbiVal;

                if (BatteryValue > nMaxLbiVal) {
                    nMaxLbiVal = BatteryValue;
                }
            }
            else {
                sCurrDate = new Date(g_StartDate);
                sCurrDate = new Date(sCurrDate.setDate(sCurrDate.getDate() - nDateVal));
                CurrDate = (sCurrDate.getMonth() + 1) + "/" + sCurrDate.getDate() + "/" + sCurrDate.getFullYear();

                nDateVal = nDateVal + 1;

                sCategory = "<category label='" + CurrDate + "' />" + sCategory;
                sLocationCount = "<set value='' />" + sLocationCount;
                sPagingCount = "<set value='' />" + sPagingCount;
                sLbiVal = "<set value='' />" + sLbiVal;
                sWifiCount = "<set value='' />" + sWifiCount;

                if (nDeviceType == 2) {
                    sTriggerCount = "<set value='' />" + sTriggerCount;
                }
            }
        }

        sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
        sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";
        sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
        sWifiCount = "<dataSet seriesName='Wifi Data Count' color='D15800' showValues='1'>" + sWifiCount + "</dataSet>";

        sLbiVal = "<lineSet seriesName='" + sLegend + "' color='F6BD0F' anchorSides='10' anchorBorderColor='F6BD0F' showValues='1'>" + sLbiVal + "</lineSet>";

        if (nDeviceType == 2) {
            sTriggerCount = "<lineSet seriesName='Trigger Count' color='EB71C3' anchorSides='10' anchorBorderColor='EB71C3' showValues='1'>" + sTriggerCount + "</lineSet>"
        }

        MakeChart(strGraphData, sCategory, sPagingCount, sLocationCount, sLbiVal, nMaxLbiVal, sWifiCount, sTriggerCount);
    }
}

//*********************************************************
//	Function Name	:	GenerateWeeklyXML
//	Input			:	dsRoot, Type
//	Description		:	Make XML String for Tag & Monitor Graph for Weekly from ajax Response
//*********************************************************
function GenerateWeeklyXML(dsRoot, Type) {
    var nRootLength;
    var n;
    var nMaxLbiVal = 0;
    var nDateVal = 1;

    var WkNo;
    var Year;
    var sDate;

    var sCategory = "";
    var sLocationCount = "";
    var sPagingCount = "";
    var sLbiVal = "";
    var sXML = "";
    var strGraphData = "";
    var sWifiCount = "";
    var o_LbiValue;

    var o_SiteId = dsRoot.getElementsByTagName('SiteId');
    var o_RWeek = dsRoot.getElementsByTagName('RWeek');
    var o_RYear = dsRoot.getElementsByTagName('RYear');
    var o_LocationCount = dsRoot.getElementsByTagName('LocationCount');
    var o_PagingCount = dsRoot.getElementsByTagName('PagingCount');
    var o_ReceivedTime = dsRoot.getElementsByTagName('ReceivedTime');
    var o_WifiCount = dsRoot.getElementsByTagName('WiFiDataCount');
    var o_bShowLBIADC = dsRoot.getElementsByTagName('bShowLBIADC');

    if (IsLBIDiffChecked == true) {
        o_LbiValue = dsRoot.getElementsByTagName('LBIDiff');
    }
    else if (IsLBIADCChecked == true) {
        o_LbiValue = dsRoot.getElementsByTagName('ADCValue');
    }
    else {
        o_LbiValue = dsRoot.getElementsByTagName('LbiValue');
    }

    nRootLength = o_SiteId.length;
    n = nRootLength - 1;

    if (nRootLength > 0) {
        WkNo = setundefined(o_RWeek[0].textContent || o_RWeek[0].innerText || o_RWeek[0].text);
        Year = setundefined(o_RYear[0].textContent || o_RYear[0].innerText || o_RYear[0].text);
        g_EndDate = setundefined(o_ReceivedTime[0].textContent || o_ReceivedTime[0].innerText || o_ReceivedTime[0].text);
        g_WeekToDate = g_EndDate;

        WkNo = setundefined(o_RWeek[n].textContent || o_RWeek[n].innerText || o_RWeek[n].text);
        Year = setundefined(o_RYear[n].textContent || o_RYear[n].innerText || o_RYear[n].text);
        g_StartDate = setundefined(o_ReceivedTime[n].textContent || o_ReceivedTime[n].innerText || o_ReceivedTime[n].text);
        g_WeekFrmDate = g_StartDate;
    }

    SetFromToDate(Type);

    //Max Lbi Val
    if (nDeviceType == 1) {
        nMaxLbiVal = 4;
    }
    else if (nDeviceType == 2) {
        nMaxLbiVal = 10;
    }

    //Disable Next Prev
    doGraphEnableButton(nRootLength, false);

    //Graph XML
    if (nRootLength > 0) {

        var bShowLBIADC = setundefined(o_bShowLBIADC[0].textContent || o_bShowLBIADC[0].innerText || o_bShowLBIADC[0].text);

        if (IsLBIDiffChecked == true) {
            sLegend = "LBI Diff";
        }
        else if (IsLBIADCChecked == true || bShowLBIADC == "True") {
            sLegend = "ADC Value";
        }
        else {
            sLegend = "Battery Value";
        }

        for (var i = 0; i <= nRootLength - 1; i++) {
            var RWeek = setundefined(o_RWeek[i].textContent || o_RWeek[i].innerText || o_RWeek[i].text);
            var RYear = setundefined(o_RYear[i].textContent || o_RYear[i].innerText || o_RYear[i].text);
            var ActivityDate = setundefined(o_ReceivedTime[i].textContent || o_ReceivedTime[i].innerText || o_ReceivedTime[i].text);
            var LocationCount = setundefined(o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text);
            var PagingCount = setundefined(o_PagingCount[i].textContent || o_PagingCount[i].innerText || o_PagingCount[i].text);
            var LbiValue = setundefined(o_LbiValue[i].textContent || o_LbiValue[i].innerText || o_LbiValue[i].text);
            var WifiCount = setundefined(o_WifiCount[i].textContent || o_WifiCount[i].innerText || o_WifiCount[i].text);

            if (WifiCount == 0) {
                WifiCount = "";
            }

            sCategory = "<category label='" + ActivityDate + "' />" + sCategory;
            sLocationCount = "<set value='" + LocationCount + "' />" + sLocationCount;
            sPagingCount = "<set value='" + PagingCount + "' />" + sPagingCount;

            if (LbiValue == 0)
                sLbiVal = "<set value='' />" + sLbiVal;
            else
                sLbiVal = "<set value='" + LbiValue + "' />" + sLbiVal;

            sWifiCount = "<set value='" + WifiCount + "' />" + sWifiCount;

            if (LbiValue > nMaxLbiVal) {
                nMaxLbiVal = LbiValue;
            }
        }

        sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
        sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";
        sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
        sLbiVal = "<lineSet seriesName='" + sLegend + "' color='F6BD0F' anchorSides='10' anchorBorderColor='F6BD0F' showValues='1'>" + sLbiVal + "</lineSet>";
        sWifiCount = "<dataSet seriesName='Wifi Data Count' color='D15800' showValues='1'>" + sWifiCount + "</dataSet>";

        MakeChart(strGraphData, sCategory, sPagingCount, sLocationCount, sLbiVal, nMaxLbiVal, sWifiCount, "");
    }
    else {
        MakeChart("", "", "", "", "", 0, "", "");
    }
}

//*********************************************************
//	Function Name	:	GenerateMonthlyXML
//	Input			:	dsRoot, Type
//	Description		:	Make XML String for Tag & Monitor Graph for Month from ajax Response
//*********************************************************
function GenerateMonthlyXML(dsRoot, Type) {
    var nRootLength;
    var n;
    var nMaxLbiVal = 0;
    var nDateVal = 1;

    var MnNo;
    var Year;
    var sDate;

    var sCategory = "";
    var sLocationCount = "";
    var sPagingCount = "";
    var sLbiVal = "";
    var sXML = "";
    var strGraphData = "";
    var sWifiCount = "";

    var o_SiteId = dsRoot.getElementsByTagName('SiteId');
    var o_RMonth = dsRoot.getElementsByTagName('RMonth');
    var o_RYear = dsRoot.getElementsByTagName('RYear');
    var o_LocationCount = dsRoot.getElementsByTagName('LocationCount');
    var o_PagingCount = dsRoot.getElementsByTagName('PagingCount');
    var o_ReceivedTime = dsRoot.getElementsByTagName('ReceivedTime');
    var o_WifiCount = dsRoot.getElementsByTagName('WiFiDataCount');
    var o_bShowLBIADC = dsRoot.getElementsByTagName('bShowLBIADC');

    if (IsLBIDiffChecked == true) {
        o_LbiValue = dsRoot.getElementsByTagName('LBIDiff');
    }
    else if (IsLBIADCChecked == true) {
        o_LbiValue = dsRoot.getElementsByTagName('ADCValue');
    }
    else {
        o_LbiValue = dsRoot.getElementsByTagName('LbiValue');
    }

    nRootLength = o_SiteId.length;
    n = nRootLength - 1;

    if (nRootLength > 0) {
        MnNo = setundefined(o_RMonth[0].textContent || o_RMonth[0].innerText || o_RMonth[0].text);
        Year = setundefined(o_RYear[0].textContent || o_RYear[0].innerText || o_RYear[0].text);
        g_EndDate = setundefined(o_ReceivedTime[0].textContent || o_ReceivedTime[0].innerText || o_ReceivedTime[0].text);
        g_MnthToDate = g_EndDate;

        MnNo = setundefined(o_RMonth[n].textContent || o_RMonth[n].innerText || o_RMonth[n].text);
        Year = setundefined(o_RYear[n].textContent || o_RYear[n].innerText || o_RYear[n].text);
        g_StartDate = setundefined(o_ReceivedTime[n].textContent || o_ReceivedTime[n].innerText || o_ReceivedTime[n].text);
        g_MnthFrmDate = g_StartDate;
    }

    SetFromToDate(Type);

    //Max Lbi Val
    if (nDeviceType == 1) {
        nMaxLbiVal = 4;
    }
    else if (nDeviceType == 2) {
        nMaxLbiVal = 10;
    }

    //Disable Next Prev
    doGraphEnableButton(nRootLength, false);

    //Graph XML
    if (nRootLength > 0) {

        var bShowLBIADC = setundefined(o_bShowLBIADC[0].textContent || o_bShowLBIADC[0].innerText || o_bShowLBIADC[0].text);

        if (IsLBIDiffChecked == true) {
            sLegend = "LBI Diff";
        }
        else if (IsLBIADCChecked == true || bShowLBIADC == "True") {
            sLegend = "ADC Value";
        }
        else {
            sLegend = "Battery Value";
        }

        for (var i = 0; i <= nRootLength - 1; i++) {
            var RMonth = setundefined(o_RMonth[i].textContent || o_RMonth[i].innerText || o_RMonth[i].text);
            var RYear = setundefined(o_RYear[i].textContent || o_RYear[i].innerText || o_RYear[i].text);
            var ActivityDate = setundefined(o_ReceivedTime[i].textContent || o_ReceivedTime[i].innerText || o_ReceivedTime[i].text);
            var LocationCount = setundefined(o_LocationCount[i].textContent || o_LocationCount[i].innerText || o_LocationCount[i].text);
            var PagingCount = setundefined(o_PagingCount[i].textContent || o_PagingCount[i].innerText || o_PagingCount[i].text);
            var LbiValue = setundefined(o_LbiValue[i].textContent || o_LbiValue[i].innerText || o_LbiValue[i].text);
            var WifiCount = setundefined(o_WifiCount[i].textContent || o_WifiCount[i].innerText || o_WifiCount[i].text);

            if (WifiCount == 0) {
                WifiCount = "";
            }

            sCategory = "<category label='" + ActivityDate + "' />" + sCategory;
            sLocationCount = "<set value='" + LocationCount + "' />" + sLocationCount;
            sPagingCount = "<set value='" + PagingCount + "' />" + sPagingCount;

            if (LbiValue == 0)
                sLbiVal = "<set value='' />" + sLbiVal;
            else
                sLbiVal = "<set value='" + LbiValue + "' />" + sLbiVal;

            sWifiCount = "<set value='" + WifiCount + "' />" + sWifiCount;

            if (LbiValue > nMaxLbiVal) {
                nMaxLbiVal = LbiValue;
            }
        }

        sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
        sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";
        sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
        sLbiVal = "<lineSet seriesName='" + sLegend + "' color='F6BD0F' anchorSides='10' anchorBorderColor='F6BD0F' showValues='1'>" + sLbiVal + "</lineSet>";
        sWifiCount = "<dataSet seriesName='Wifi Data Count' color='D15800' showValues='1'>" + sWifiCount + "</dataSet>";

        MakeChart(strGraphData, sCategory, sPagingCount, sLocationCount, sLbiVal, nMaxLbiVal, sWifiCount, "");
    }
    else {
        MakeChart("", "", "", "", "", 0, "", "");
    }
}

//*********************************************************
//	Function Name	:	MakeChart
//	Input			:	dsRoot, Type
//	Description		:	Make Chart from XML String
//*********************************************************
function MakeChart(strGraphData, sCategory, sPagingCount, sLocationCount, sLbiVal, nMaxLbiVal, sWifiCount, sTriggerCount) {
    var sXML;

    strGraphData = sCategory + "<dataset>" + sPagingCount + sLocationCount + sWifiCount + "</dataset>" + sLbiVal + sTriggerCount;

    sXML = "<chart palette='1' plotBorderThickness ='1' SYAxisMaxValue='" + nMaxLbiVal + "' baseFontSize='10' " +
           "caption='' rotateLabels='1' rotateValues='1' showSum='0'  xaxisname='Date' Yaxisname='' " +
           "numdivlines='3' useRoundEdges='1' legendBorderAlpha='0' connectNullData='1' " +
           "bgColor='FFFFFF' lineColor='F90327' showValues='2' seriesNameInToolTip='1' showBorder='1'" +
           " showLabels='1' anchorRadius='2' outCnvBaseFontSize='10' animation='1' " +
           "showAlternateHGridColor='1' AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' " +
           "alternateHGridAlpha='5' canvasBorderColor='666666' baseFontColor='000000' lineThickness='2' " +
           "formatNumberScale='0' formatNumber='0'>" + strGraphData + "</chart>";

    FusionCharts.setCurrentRenderer('javascript');

    var chart = new FusionCharts({
        "type": "msstackedcolumn2dlinedy",
        "renderAt": "Div_MSStackedColumn2D",
        "width": "900",
        "height": "450"
    });

    chart.setDataXML(sXML);
    chart.render("Div_MSStackedColumn2D");
}

//*********************************************************
//	Function Name	:	LoadStarDeviceGraph
//	Input			:	dsRoot, Type
//	Description		:	Make XML String for Star Graph from ajax Response
//*********************************************************
function LoadStarDeviceGraph(dsRoot, Type) {
    if (dsRoot != null) {
        var nRootLength;
        var n;
        var nMaxYVal = 0;

        var sCurrDate;
        var CurrDate;

        var sCategory = "";
        var sLocationCount = "";
        var sPagingCount = "";
        var sLocationVal = "";
        var sPagingVal = "";
        var sXML = "";
        var strGraphData = "";

        var o_SiteId = dsRoot.getElementsByTagName('SiteId');
        var o_Locationdatareceived = dsRoot.getElementsByTagName('Locationdatareceived');
        var o_Pagedatareceived = dsRoot.getElementsByTagName('Pagedatareceived');
        var o_LocationDataCount = dsRoot.getElementsByTagName('LocationDataCount');
        var o_PageDataCount = dsRoot.getElementsByTagName('PageDataCount');
        var o_DATFileName = dsRoot.getElementsByTagName('UpdatedOn');

        nRootLength = o_SiteId.length;
        n = nRootLength - 1;

        if (nRootLength > 0) {
            g_EndDate = setundefined(o_DATFileName[0].textContent || o_DATFileName[0].innerText || o_DATFileName[0].text);
            g_StartDate = setundefined(o_DATFileName[n].textContent || o_DATFileName[n].innerText || o_DATFileName[n].text);
        }

        SetFromToDate(Type);

        //Disable Next Prev
        doGraphEnableButton(nRootLength, true);

        //Graph XML
        if (nRootLength > 0) {
            for (var i = g_StartIdx; i <= g_EndIdx - 1; i++) {
                if (i <= nRootLength - 1) {
                    var Locationdatareceived = setundefined(o_Locationdatareceived[i].textContent || o_Locationdatareceived[i].innerText || o_Locationdatareceived[i].text);
                    var Pagedatareceived = setundefined(o_Pagedatareceived[i].textContent || o_Pagedatareceived[i].innerText || o_Pagedatareceived[i].text);
                    var LocationDataCount = setundefined(o_LocationDataCount[i].textContent || o_LocationDataCount[i].innerText || o_LocationDataCount[i].text);
                    var PageDataCount = setundefined(o_PageDataCount[i].textContent || o_PageDataCount[i].innerText || o_PageDataCount[i].text);
                    var DATFileName = setundefined(o_DATFileName[i].textContent || o_DATFileName[i].innerText || o_DATFileName[i].text);

                    sCategory = "<category label='" + DATFileName + "' />" + sCategory;
                    sLocationCount = "<set value='" + Locationdatareceived + "' />" + sLocationCount;
                    sPagingCount = "<set value='" + Pagedatareceived + "' />" + sPagingCount;
                    sLocationVal = "<set value='" + LocationDataCount + "' />" + sLocationVal;
                    sPagingVal = "<set value='" + PageDataCount + "' />" + sPagingVal;

                    if (Locationdatareceived > nMaxYVal)
                        nMaxYVal = Locationdatareceived;
                    if (Pagedatareceived > nMaxYVal)
                        nMaxYVal = Pagedatareceived;
                    if (LocationDataCount > nMaxYVal)
                        nMaxYVal = LocationDataCount;
                    if (PageDataCount > nMaxYVal)
                        nMaxYVal = PageDataCount;
                }
                else {
                    sCategory = "<category label='' />" + sCategory;
                    sLocationCount = "<set value='' />" + sLocationCount;
                    sPagingCount = "<set value='' />" + sPagingCount;
                    sLocationVal = "<set value='' />" + sLocationVal;
                    sPagingVal = "<set value='' />" + sPagingVal;
                }
            }

            sCategory = "<categories font='Verdana' fontSize='10' fontColor='000000'>" + sCategory + "</categories>";
            sPagingCount = "<dataSet seriesName='Paging Count' color='F6BD0F' showValues='1'>" + sPagingCount + "</dataSet>";
            sLocationCount = "<dataSet seriesName='Location Count' color='8BBA00' showValues='1'>" + sLocationCount + "</dataSet>";
            sPagingVal = "<lineSet seriesName='Paging Data Count' color='F6BD0F' anchorSides='10' anchorBorderColor='F6BD0F' showValues='1'>" + sPagingVal + "</lineSet>";
            sLocationVal = "<lineSet seriesName='Location Data Count' color='8BBA00' anchorSides='10' anchorBorderColor='8BBA00' showValues='1'>" + sLocationVal + "</lineSet>";

            strGraphData = sCategory + "<dataset>" + sPagingCount + sLocationCount + "</dataset>" + sPagingVal + sLocationVal;

            sXML = "<chart palette='1' plotBorderThickness ='1' SYAxisMaxValue='" + nMaxYVal + "' baseFontSize='10' " +
                   "caption='' rotateLabels='1' rotateValues='1' showSum='0'  xaxisname='Date' Yaxisname='' " +
                   "numdivlines='3' useRoundEdges='1' legendBorderAlpha='0' connectNullData='1' " +
                   "bgColor='FFFFFF' lineColor='F90327' showValues='2' seriesNameInToolTip='1' showBorder='1'" +
                   " showLabels='1' anchorRadius='2' outCnvBaseFontSize='10' animation='1' " +
                   "showAlternateHGridColor='1' AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' " +
                   "alternateHGridAlpha='5' canvasBorderColor='666666' baseFontColor='000000' lineThickness='2' " +
                   "formatNumberScale='0' formatNumber='0'>" + strGraphData + "</chart>";

            FusionCharts.setCurrentRenderer('javascript');
            var chart = new FusionCharts({
                "type": "msstackedcolumn2dlinedy",
                "renderAt": "Div_MSStackedColumn2D",
                "width": "900",
                "height": "450"
            });

            chart.setDataXML(sXML);
            chart.render("Div_MSStackedColumn2D");
        }
        else {
            MakeChart("", "", "", "", "", 0, "", "");
        }

        document.getElementById("divLoading_Graph").style.display = "none";
    }
}

//*********************************************************
//	Function Name	:	Last10hrdata
//	Input			:	SiteId,DeviceType,DeviceId
//	Description		:	ajax call Last10hrdata
//*********************************************************
function Last10hrdata(SiteId, DeviceType, DeviceId) {
    g_Device10HrObj = CreateDeviceXMLObj();

    if (g_Device10HrObj != null) {
        g_Device10HrObj.onreadystatechange = ajax10hrList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=Last10hrdata&sid=" + SiteId + "&DeviceType=" + DeviceType + "&DeviceId=" + DeviceId;

        if (GetBrowserType() == "isIE") {
            g_Device10HrObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Device10HrObj.open("GET", DbConnectorPath, true);
        }
        g_Device10HrObj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajax10hrList
//	Input			:	none
//	Description		:	Load 10 Hr Data from ajax Response
//*********************************************************
function ajax10hrList() {
    if (g_Device10HrObj.readyState == 4) {
        if (g_Device10HrObj.status == 200) {
            var sTbl, sTblLen;

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Device10HrObj.responseXML.documentElement);

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblWiFiDetails');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblWiFiDetails');
            }

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            var dsRoot = g_Device10HrObj.responseXML.documentElement;

            if (nDeviceType == 1) {
                LoadTag10HrData(dsRoot, sTbl);
            }
            else if (nDeviceType == 2) {
                LoadMonitor10HrData(dsRoot, sTbl);
            }
            else if (nDeviceType == 3) {
                LoadStar10HrData(dsRoot, sTbl);
            }

            document.getElementById('divLoading_WifiDetails').style.display = "none";
        }
    }
}

function downLoadLogFile(FilePath) {

    FilePath = decodeURIComponent(setundefined(FilePath));

    $("#hrefLogFiles").attr('href', FilePath);
}

//*********************************************************
//	Function Name	:	LoadTag10HrData
//	Input			:	dsRoot,sTbl
//	Description		:	Load Tag 10 Hr Data from ajax Response
//*********************************************************
function LoadTag10HrData(dsRoot, sTbl) {
    var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
    var o_FirmwareVersion = dsRoot.getElementsByTagName('FirmwareVersion');
    var o_IRId = dsRoot.getElementsByTagName('IRId');
    var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
    var o_RSSI = dsRoot.getElementsByTagName('RSSI');
    var o_LBI = dsRoot.getElementsByTagName('LBI');
    var o_LBIValue = dsRoot.getElementsByTagName('LBIValue');
    var o_MinLBI = dsRoot.getElementsByTagName('MinLBI');
    var o_MaxLBI = dsRoot.getElementsByTagName('MaxLBI');
    var o_LBIDiff = dsRoot.getElementsByTagName('LBIDiff');
    var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
    var o_LastPagingTime = dsRoot.getElementsByTagName('LastPagingTime');
    var o_ReceivedTime = dsRoot.getElementsByTagName('ReceivedTime');
    var o_LocationDataReceived = dsRoot.getElementsByTagName('LocationDataReceived');
    var o_PageDataReceived = dsRoot.getElementsByTagName('PageDataReceived');
    var o_AliveCount = dsRoot.getElementsByTagName('AliveCount');
    var o_AvgRssi = dsRoot.getElementsByTagName('AvgRssi');
    var o_IRCount = dsRoot.getElementsByTagName('IRCount');
    var o_IRSeen = dsRoot.getElementsByTagName('IRSeen');
    var o_StarCount = dsRoot.getElementsByTagName('StarCount');
    var o_StarSeen = dsRoot.getElementsByTagName('StarSeen');
    var o_TempC = dsRoot.getElementsByTagName('TempC');
    var o_TempADC = dsRoot.getElementsByTagName('TempADC');
    var o_LogFileName = dsRoot.getElementsByTagName('LogFileName');
    var o_LogFileDate = dsRoot.getElementsByTagName('LogFileDate');
    var o_LogFileId = dsRoot.getElementsByTagName('LogFileId');

    var nRootLength = o_DeviceId.length;

    //Header
    row = document.createElement('tr');
    AddCell(row, "Firmware Version", 'siteOverview_TopLeft_Box', "", "", "center", "150px", "30px", "");
    AddCell(row, "IR Id", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Monitor Id", 'siteOverview_Box', "", "", "center", "200px", "30px", "");
    AddCell(row, "RSSI", 'siteOverview_Box', "", "", "center", "60px", "30px", "");
    AddCell(row, "LBI Value", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Min LBI", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Max LBI", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "LBI Diff", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Locked Star Id", 'siteOverview_Box', "", "", "center", "120px", "30px", "");
    AddCell(row, "Last Paging Time", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Received Time", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Location Data Received", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Page Data Received", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Alive Count", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Avg Rssi", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "IR Count", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "IR Seen", 'siteOverview_Box', "", "", "center", "300px", "30px", "");
    AddCell(row, "Star Count", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Star Seen", 'siteOverview_Box', "", "", "center", "300px", "30px", "");
    AddCell(row, "TempC", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Temp ADC", 'siteOverview_Topright_Box', "", "", "center", "100px", "30px", "");
    sTbl.appendChild(row);

    //Datas
    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {
            var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
            var FirmwareVersion = setundefined(o_FirmwareVersion[i].textContent || o_FirmwareVersion[i].innerText || o_FirmwareVersion[i].text);
            var IRId = setundefined(o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text);
            var MonitorId = setundefined(o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text);
            var RSSI = setundefined(o_RSSI[i].textContent || o_RSSI[i].innerText || o_RSSI[i].text);
            var LBI = setundefined(o_LBI[i].textContent || o_LBI[i].innerText || o_LBI[i].text);
            var LBIValue = setundefined(o_LBIValue[i].textContent || o_LBIValue[i].innerText || o_LBIValue[i].text);
            var MinLBI = setundefined(o_MinLBI[i].textContent || o_MinLBI[i].innerText || o_MinLBI[i].text);
            var MaxLBI = setundefined(o_MaxLBI[i].textContent || o_MaxLBI[i].innerText || o_MaxLBI[i].text);
            var LBIDiff = setundefined(o_LBIDiff[i].textContent || o_LBIDiff[i].innerText || o_LBIDiff[i].text);
            var LockedStarId = setundefined(o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text);
            var LastPagingTime = setundefined(o_LastPagingTime[i].textContent || o_LastPagingTime[i].innerText || o_LastPagingTime[i].text);
            var ReceivedTime = setundefined(o_ReceivedTime[i].textContent || o_ReceivedTime[i].innerText || o_ReceivedTime[i].text);
            var LocationDataReceived = setundefined(o_LocationDataReceived[i].textContent || o_LocationDataReceived[i].innerText || o_LocationDataReceived[i].text);
            var PageDataReceived = setundefined(o_PageDataReceived[i].textContent || o_PageDataReceived[i].innerText || o_PageDataReceived[i].text);
            var AliveCount = setundefined(o_AliveCount[i].textContent || o_AliveCount[i].innerText || o_AliveCount[i].text);
            var AvgRssi = setundefined(o_AvgRssi[i].textContent || o_AvgRssi[i].innerText || o_AvgRssi[i].text);
            var IRCount = setundefined(o_IRCount[i].textContent || o_IRCount[i].innerText || o_IRCount[i].text);
            var IRSeen = setundefined(o_IRSeen[i].textContent || o_IRSeen[i].innerText || o_IRSeen[i].text);
            var StarCount = setundefined(o_StarCount[i].textContent || o_StarCount[i].innerText || o_StarCount[i].text);
            var StarSeen = setundefined(o_StarSeen[i].textContent || o_StarSeen[i].innerText || o_StarSeen[i].text);
            var TempC = setundefined(o_TempC[i].textContent || o_TempC[i].innerText || o_TempC[i].text);
            var TempADC = setundefined(o_TempADC[i].textContent || o_TempADC[i].innerText || o_TempADC[i].text);
            var LogFileDate = setundefined(o_LogFileDate[i].textContent || o_LogFileDate[i].innerText || o_LogFileDate[i].text);
            var LogFileId = setundefined(o_LogFileId[i].textContent || o_LogFileId[i].innerText || o_LogFileId[i].text);
            
            row = document.createElement('tr');
            AddCell(row, FirmwareVersion, 'DeviceList_leftBox', "", "", "center", "", "25px", "");
            AddCell(row, IRId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, MonitorId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, RSSI, 'siteOverview_cell', "", "", "center", "", "25px", "");
            //AddCell(row,LBI,'siteOverview_cell',"","","center","","25px","");
            AddCell(row, LBIValue, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, MinLBI, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, MaxLBI, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LBIDiff, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LockedStarId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LastPagingTime, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, ReceivedTime, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LocationDataReceived, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, PageDataReceived, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, AliveCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, AvgRssi, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, IRCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, IRSeen, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, StarCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, StarSeen, 'siteOverview_cell', "", "", "center", "25px", "");
            AddCell(row, TempC, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, TempADC, 'siteOverview_cell', "", "", "center", "", "25px", "");
            sTbl.appendChild(row);
        }
    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No records found...", 'siteOverview_cell_Full', 23, "", "left", "100px", "40px", "");
        sTbl.appendChild(row);
    }
}

//*********************************************************
//	Function Name	:	LoadMonitor10HrData
//	Input			:	dsRoot,sTbl
//	Description		:	Load Monitor 10 Hr Data from ajax Response
//*********************************************************
function LoadMonitor10HrData(dsRoot, sTbl) {
    var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
    var o_FirmwareVersion = dsRoot.getElementsByTagName('FirmwareVersion');
    var o_LastSeenTagId = dsRoot.getElementsByTagName('LastSeenTagId');
    var o_IRId = dsRoot.getElementsByTagName('IRId');
    var o_RSSI = dsRoot.getElementsByTagName('RSSI');
    var o_LBI = dsRoot.getElementsByTagName('LBI');
    var o_LBIValue = dsRoot.getElementsByTagName('LBIValue');
    var o_MinLBI = dsRoot.getElementsByTagName('MinLBI');
    var o_MaxLBI = dsRoot.getElementsByTagName('MaxLBI');
    var o_LBIDiff = dsRoot.getElementsByTagName('LBIDiff');
    var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
    var o_LastPagingTime = dsRoot.getElementsByTagName('LastPagingTime');
    var o_ReceivedTime = dsRoot.getElementsByTagName('ReceivedTime');
    var o_LocationDataReceived = dsRoot.getElementsByTagName('LocationDataReceived');
    var o_PageDataReceived = dsRoot.getElementsByTagName('PageDataReceived');
    var o_TriggerCount = dsRoot.getElementsByTagName('TriggerCount');
    var o_AvgRssi = dsRoot.getElementsByTagName('AvgRssi');
    var o_StarCount = dsRoot.getElementsByTagName('StarCount');
    var o_StarSeen = dsRoot.getElementsByTagName('StarSeen');
    var o_LogFileName = dsRoot.getElementsByTagName('LogFileName');
    var o_LogFileDate = dsRoot.getElementsByTagName('LogFileDate');
    var o_LogFileId = dsRoot.getElementsByTagName('LogFileId');

    var nRootLength = o_DeviceId.length;

    //Header
    row = document.createElement('tr');
    AddCell(row, "Firmware Version", 'siteOverview_TopLeft_Box', "", "", "center", "150px", "30px", "");
    AddCell(row, "Last Seen Tag Id", 'siteOverview_Box', "", "", "center", "150px", "30px", "");
    AddCell(row, "IR Id", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "RSSI", 'siteOverview_Box', "", "", "center", "60px", "30px", "");
    AddCell(row, "LBI Value", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Min LBI", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Max LBI", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "LBI Diff", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Locked Star Id", 'siteOverview_Box', "", "", "center", "120px", "30px", "");
    AddCell(row, "Last Paging Time", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Received Time", 'siteOverview_Box', "", "", "center", "120px", "30px", "");
    AddCell(row, "Location Data Received", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Page Data Received", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Trigger Count", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Avg Rssi", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Star Count", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Star Seen", 'siteOverview_Box', "", "", "center", "300px", "30px", "");
    sTbl.appendChild(row);

    //Datas
    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {
            var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
            var FirmwareVersion = setundefined(o_FirmwareVersion[i].textContent || o_FirmwareVersion[i].innerText || o_FirmwareVersion[i].text);
            var LastSeenTagId = setundefined(o_LastSeenTagId[i].textContent || o_LastSeenTagId[i].innerText || o_LastSeenTagId[i].text);
            var IRId = setundefined(o_IRId[i].textContent || o_IRId[i].innerText || o_IRId[i].text);
            var RSSI = setundefined(o_RSSI[i].textContent || o_RSSI[i].innerText || o_RSSI[i].text);
            var LBI = setundefined(o_LBI[i].textContent || o_LBI[i].innerText || o_LBI[i].text);
            var LBIValue = setundefined(o_LBIValue[i].textContent || o_LBIValue[i].innerText || o_LBIValue[i].text);
            var MinLBI = setundefined(o_MinLBI[i].textContent || o_MinLBI[i].innerText || o_MinLBI[i].text);
            var MaxLBI = setundefined(o_MaxLBI[i].textContent || o_MaxLBI[i].innerText || o_MaxLBI[i].text);
            var LBIDiff = setundefined(o_LBIDiff[i].textContent || o_LBIDiff[i].innerText || o_LBIDiff[i].text);
            var LockedStarId = setundefined(o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text);
            var LastPagingTime = setundefined(o_LastPagingTime[i].textContent || o_LastPagingTime[i].innerText || o_LastPagingTime[i].text);
            var ReceivedTime = setundefined(o_ReceivedTime[i].textContent || o_ReceivedTime[i].innerText || o_ReceivedTime[i].text);
            var LocationDataReceived = setundefined(o_LocationDataReceived[i].textContent || o_LocationDataReceived[i].innerText || o_LocationDataReceived[i].text);
            var PageDataReceived = setundefined(o_PageDataReceived[i].textContent || o_PageDataReceived[i].innerText || o_PageDataReceived[i].text);
            var TriggerCount = setundefined(o_TriggerCount[i].textContent || o_TriggerCount[i].innerText || o_TriggerCount[i].text);
            var AvgRssi = setundefined(o_AvgRssi[i].textContent || o_AvgRssi[i].innerText || o_AvgRssi[i].text);
            var StarCount = setundefined(o_StarCount[i].textContent || o_StarCount[i].innerText || o_StarCount[i].text);
            var StarSeen = setundefined(o_StarSeen[i].textContent || o_StarSeen[i].innerText || o_StarSeen[i].text);
            var LogFileDate = setundefined(o_LogFileDate[i].textContent || o_LogFileDate[i].innerText || o_LogFileDate[i].text);
            var LogFileId = setundefined(o_LogFileId[i].textContent || o_LogFileId[i].innerText || o_LogFileId[i].text);
                       
            row = document.createElement('tr');
            AddCell(row, FirmwareVersion, 'DeviceList_leftBox', "", "", "center", "", "25px", "");
            AddCell(row, LastSeenTagId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, IRId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, RSSI, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LBIValue, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, MinLBI, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, MaxLBI, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LBIDiff, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LockedStarId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LastPagingTime, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, ReceivedTime, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LocationDataReceived, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, PageDataReceived, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, TriggerCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, AvgRssi, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, StarCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, StarSeen, 'siteOverview_cell', "", "", "center", "25px", "");
            sTbl.appendChild(row);
        }
    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No records found...", 'siteOverview_cell_Full', 18, "", "left", "100px", "40px", "");
        sTbl.appendChild(row);
    }
}

//*********************************************************
//	Function Name	:	LoadStar10HrData
//	Input			:	dsRoot,sTbl
//	Description		:	Load Star 10 Hr Data from ajax Response
//*********************************************************
function LoadStar10HrData(dsRoot, sTbl) {
    var o_MacId = dsRoot.getElementsByTagName('MacId');
    var o_FirmwareVersion = dsRoot.getElementsByTagName('Firmwareversion');
    var o_IPAddress = dsRoot.getElementsByTagName('IPAddress');
    var o_UpdatedOn = dsRoot.getElementsByTagName('UpdatedOn');
    var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
    var o_Locationdatareceived = dsRoot.getElementsByTagName('Locationdatareceived');
    var o_Pagedatareceived = dsRoot.getElementsByTagName('Pagedatareceived');
    var o_NonSyncCount = dsRoot.getElementsByTagName('NonSyncCount');
    var o_EthernetOffsetCount = dsRoot.getElementsByTagName('EthernetOffsetCount');
    var o_LocationDataCount = dsRoot.getElementsByTagName('LocationDataCount');
    var o_PageDataCount = dsRoot.getElementsByTagName('PageDataCount');
    var o_TimeDiff = dsRoot.getElementsByTagName('TimeDiff');
    var o_ResCount = dsRoot.getElementsByTagName('ResCount');
    var o_RequestCount = dsRoot.getElementsByTagName('RequestCount');
    var o_DownTime = dsRoot.getElementsByTagName('DownTime');
    var o_DATFileName = dsRoot.getElementsByTagName('DATFileName');
    var o_LogFileName = dsRoot.getElementsByTagName('LogFileName');
    var o_LogFileDate = dsRoot.getElementsByTagName('LogFileDate');
    var o_LogFileId = dsRoot.getElementsByTagName('LogFileId');

    var nRootLength = o_MacId.length;

    //Header
    row = document.createElement('tr');
    AddCell(row, "Mac Id", 'siteOverview_TopLeft_Box', "", "", "center", "150px", "30px", "");
    AddCell(row, "Firmware Version", 'siteOverview_Box', "", "", "center", "120px", "30px", "");
    AddCell(row, "IP Address", 'siteOverview_Box', "", "", "center", "150px", "30px", "");
    AddCell(row, "Updated On", 'siteOverview_Box', "", "", "center", "150px", "30px", "");
    AddCell(row, "Locked Star Id", 'siteOverview_Box', "", "", "center", "60px", "30px", "");
    AddCell(row, "Location Data Received", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Page Data Received", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Non Sync Count", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Ethernet Offset Count", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Location Data Count", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Page Data Count", 'siteOverview_Box', "", "", "center", "140px", "30px", "");
    AddCell(row, "Time Diff", 'siteOverview_Box', "", "", "center", "60px", "30px", "");
    AddCell(row, "Res Count", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    AddCell(row, "Request Count", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "Down Time", 'siteOverview_Box', "", "", "center", "80px", "30px", "");
    AddCell(row, "DAT FileName", 'siteOverview_Box', "", "", "center", "100px", "30px", "");
    sTbl.appendChild(row);

    //Datas
    if (nRootLength > 0) {
        for (var i = 0; i < nRootLength; i++) {
            var MacId = setundefined(o_MacId[i].textContent || o_MacId[i].innerText || o_MacId[i].text);
            var FirmwareVersion = setundefined(o_FirmwareVersion[i].textContent || o_FirmwareVersion[i].innerText || o_FirmwareVersion[i].text);
            var IPAddress = setundefined(o_IPAddress[i].textContent || o_IPAddress[i].innerText || o_IPAddress[i].text);
            var UpdatedOn = setundefined(o_UpdatedOn[i].textContent || o_UpdatedOn[i].innerText || o_UpdatedOn[i].text);
            var LockedStarId = setundefined(o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text);
            var LocationDataReceived = setundefined(o_Locationdatareceived[i].textContent || o_Locationdatareceived[i].innerText || o_Locationdatareceived[i].text);
            var PageDataReceived = setundefined(o_Pagedatareceived[i].textContent || o_Pagedatareceived[i].innerText || o_Pagedatareceived[i].text);
            var NonSyncCount = setundefined(o_NonSyncCount[i].textContent || o_NonSyncCount[i].innerText || o_NonSyncCount[i].text);
            var EthernetOffsetCount = setundefined(o_EthernetOffsetCount[i].textContent || o_EthernetOffsetCount[i].innerText || o_EthernetOffsetCount[i].text);
            var LocationDataCount = setundefined(o_LocationDataCount[i].textContent || o_LocationDataCount[i].innerText || o_LocationDataCount[i].text);
            var PageDataCount = setundefined(o_PageDataCount[i].textContent || o_PageDataCount[i].innerText || o_PageDataCount[i].text);
            var TimeDiff = setundefined(o_TimeDiff[i].textContent || o_TimeDiff[i].innerText || o_TimeDiff[i].text);
            var ResCount = setundefined(o_ResCount[i].textContent || o_ResCount[i].innerText || o_ResCount[i].text);
            var RequestCount = setundefined(o_RequestCount[i].textContent || o_RequestCount[i].innerText || o_RequestCount[i].text);
            var DownTime = setundefined(o_DownTime[i].textContent || o_DownTime[i].innerText || o_DownTime[i].text);
            var DATFileName = setundefined(o_DATFileName[i].textContent || o_DATFileName[i].innerText || o_DATFileName[i].text);
            var LogFileDate = setundefined(o_LogFileDate[i].textContent || o_LogFileDate[i].innerText || o_LogFileDate[i].text);
            var LogFileId = setundefined(o_LogFileId[i].textContent || o_LogFileId[i].innerText || o_LogFileId[i].text);
                       
            row = document.createElement('tr');
            AddCell(row, MacId, 'DeviceList_leftBox', "", "", "center", "", "25px", "");
            AddCell(row, FirmwareVersion, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, IPAddress, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, UpdatedOn, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LockedStarId, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LocationDataReceived, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, PageDataReceived, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, NonSyncCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, EthernetOffsetCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, LocationDataCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, PageDataCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, TimeDiff, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, ResCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, RequestCount, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, DownTime, 'siteOverview_cell', "", "", "center", "", "25px", "");
            AddCell(row, DATFileName, 'siteOverview_cell', "", "", "center", "", "25px", "");
            sTbl.appendChild(row);
        }
    }
    else {
        row = document.createElement('tr');
        AddCell(row, "No records found...", 'siteOverview_cell_Full', 16, "", "left", "100px", "40px", "");
        sTbl.appendChild(row);
    }
}

function doGraphEnableButton(RootLength, bDisable) {
    document.getElementById("ctl00_ContentPlaceHolder1_trPaginationGraph").style.display = "";

    if (g_StartIdx == 0 && g_EndIdx == 0) {
        document.getElementById("ctl00_ContentPlaceHolder1_btnNextGraph").disabled = true;
        document.getElementById("ctl00_ContentPlaceHolder1_btnPrevGraph").disabled = true;
    }
    else {
        document.getElementById("ctl00_ContentPlaceHolder1_btnNextGraph").disabled = false;
        document.getElementById("ctl00_ContentPlaceHolder1_btnPrevGraph").disabled = false;
    }

    if (bDisable) {
        if (g_StartIdx <= 1) {
            document.getElementById("ctl00_ContentPlaceHolder1_btnNextGraph").disabled = true;
        }
    }

    if (bDisable) {
        if (g_EndIdx >= RootLength) {
            document.getElementById("ctl00_ContentPlaceHolder1_btnPrevGraph").disabled = true;
        }
    }
}