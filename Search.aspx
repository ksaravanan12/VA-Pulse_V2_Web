<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Search.aspx.vb" Inherits="GMSUI.Search" Title="Search" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <style type="text/css">
        @import url( Javascript/Calendar/calendar-blue2.css );
    </style>
    <link rel="stylesheet" type="text/css" href="Styles/elastislide.css" />
    <script type="text/javascript" src="Javascript/Calendar/calendar.js"></script>
    <script type="text/javascript" src="Javascript/Calendar/calendar-setup.js"></script>
    <script type="text/javascript" src="Javascript/Calendar/lang/calendar-en.js"></script>
    <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>
    <script type="text/javascript" src="Javascript/jquery.plugin.js"></script>
    <script type="text/javascript" src="Javascript/jquery.timeentry.js"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="Javascript/js_DevicePhotos.js?d=12"></script>
    <script type="text/javascript" src="Javascript/jquery.tmpl.min.js"></script>
    <script type="text/javascript" src="Javascript/jquery.device_elastislide.js"></script>
    <script type="text/javascript" src="Javascript/gallery_device.js"></script>
    <script type="text/javascript" src="Javascript/js_TagDetail.js?d=7"></script>
    <script id="img-wrapper-tmpl" type="text/x-jquery-tmpl"></script>
    <style type="text/css">
        body
        {
            font: bold 12px Helvetica;
        }
        
        /*- Menu Tabs 1--------------------------- */#tabs1
        {
            float: left;
            width: 100%;
            background: #FFFFFF;
            font-size: 93%;
            line-height: normal;
            border-bottom: 1px solid #CCCCCC;
        }
        
        #tabs1 ul
        {
            margin: 0;
            padding: 10px 10px 0 20px;
            list-style: none;
        }
        
        #tabs1 li
        {
            display: inline;
            margin: 0;
            padding: 0;
        }
        
        #tabs1 a
        {
            float: left;
            background: url("Images/tableft1.gif") no-repeat left top;
            margin: 0;
            padding: 0 0 0 4px;
            text-decoration: none;
            cursor: pointer;
        }
        
        #tabs1 a span
        {
            float: left;
            display: block;
            background: url("Images/tabright1.gif") no-repeat right top;
            padding: 5px 15px 4px 6px;
            color: #005695;
        }
        
        /* Commented Backslash Hack hides rule from IE5-Mac \*/#tabs1 a span
        {
            float: none;
        }
        /* End IE5-Mac hack */#tabs a:hover span
        {
            color: #005695;
        }
        /*#tabs1 a:hover {
            background-position:0% -42px;
        }
        
        #tabs1 a:hover span {
            background-position:100% -42px;
        }

        #tabs1 #current a {
              background-position:0% -42px;
        }
        
        #tabs1 #current a span {
              background-position:100% -42px;
        }*/#tabs1 a:hover
        {
            background-position: 0% -42px;
        }
        #tabs1 a:hover span
        {
            background-position: 100% -42px;
        }
        #tabs1 .current a
        {
            background-position: 0% -42px;
        }
        #tabs1 .current a span
        {
            background-position: 100% -42px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function setRangeLable(lbl, type) {

            if (type == 1)
                document.getElementById(lbl).innerHTML = "Hourly ";
            else if (type == 2)
                document.getElementById(lbl).innerHTML = " Daily ";
            else if (type == 3)
                document.getElementById(lbl).innerHTML = "Weekly ";
            else if (type == 4)
                document.getElementById(lbl).innerHTML = "Monthly";
        }

        $.fn.validation = function () {

            if ($("#ctl00_leftmenu_selSiteForExport").val() == 0) {
                alert("Select Site !!!");
                return false;
            }

            if ($("#ctl00_leftmenu_selReportforExport").val() == 0) {
                alert("Select any Report type !!!");
                return false;
            }

            return true;
        }

        // Temporary variables to hold mouse x-y pos.s
        var isButtonClicked = 0;
        var g_UserRole = 0;
        var g_reachedSearch = 0;
        var g_OldSite = 0;
        var g_UserId = 0;
        var SiteId = 0;
        var g_IsTempMonitoring = 0;

        //Onload Function
        this.onload = function () {

            document.getElementById("<%=divSearch.ClientID%>").style.display = "";
            document.getElementById("<%=tblFilterCriteria.ClientID%>").style.display = "";
            document.getElementById("<%=ddlFilter.ClientID%>").style.display = "";

            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            g_IsTempMonitoring = document.getElementById("<%=hid_IsTempMonitoring.ClientID%>").value;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            g_OldSite = siteVal;
            SiteId = siteVal;

            var txtDeviceIds = document.getElementById("<%=txtDeviceIds.ClientID%>").value;

            if (txtDeviceIds != "")
                NewInvokeReqCount_DeviceSearch();
        };

        //Hash Change Event
        window.onhashchange = function () {

            if (location.hash == "#divDeviceInfo" || location.hash == "#EMdeviceDetail")
                document.getElementById('tdLeftMenu').style.display = "none";
            else
                document.getElementById('tdLeftMenu').style.display = "";

            if (g_reachedSearch == 1 && isButtonClicked == 0)
                return;

            if (location.hash == "" || location.hash == "#" || location.hash == "#divSearch") {
                showGlossaryInfo("Search")
            }

            if (isButtonClicked == 1) {
                if (location.hash == "" || location.hash == "#" || location.hash == "#divSearch") {
                    g_reachedSearch = 1;
                }
                else
                    g_reachedSearch = 0;

                isButtonClicked = 0;
                return;
            }

            if (location.hash == "" || location.hash == "#" || location.hash == "#divSearch") {
                g_reachedSearch = 1;
                DisplaySearchForBrowserBack();
            }
        }

        //Search Filter Options
        function showFilterCtrl(ctrl) {

            var ddlDeviceType = document.getElementById("<%=ddlDeviceType.ClientID%>");
            var ddlDeviceTypeText = ddlDeviceType.options[ddlDeviceType.selectedIndex].text;

            var objid1 = 'ctl00_ContentPlaceHolder1_' + ctrl.value + ddlDeviceTypeText;
            var objid2 = 'chk' + ctrl.value + ddlDeviceTypeText;

            if (ctrl.value != 0) {
                document.getElementById(objid1).style.display = 'inline';
                document.getElementById(objid2).checked = true;
            }
        }

        //Show/Hide Filter Criteria on Select Filters
        function showHideFilterCtrl(ctrl, tr) {

            tr = "ctl00_ContentPlaceHolder1_" + tr;

            if (ctrl.checked == true) {
                document.getElementById(tr).style.display = 'inline';
            }
            else if (ctrl.checked == false) {
                document.getElementById(tr).style.display = 'none';
            }
        }

        //Operators Control Show/Hide on Change Operator
        function showFilterCondCtrl(ctrl, searcCtrl1, searcCtrl2, btnCtrl2) {

            document.getElementById(searcCtrl1).style.display = 'none';
            document.getElementById(searcCtrl2).style.display = 'none';

            if (ctrl.value == 5) {
                document.getElementById(searcCtrl1).style.display = 'inline';
                document.getElementById(searcCtrl2).style.display = 'inline';
            }
            else if (ctrl.value > 1) {
                document.getElementById(searcCtrl1).style.display = 'inline';
            }
        }

        //Show Current Device Type Filter Criteria and Clear Previous on Change
        function ShowHideDeviceTypeCtrls() {

            var ddlDeviceType = document.getElementById("<%=ddlDeviceType.ClientID%>");
            var ddlDeviceTypeVal = ddlDeviceType.options[ddlDeviceType.selectedIndex].value;

            var tblFilterCriteria = document.getElementById("<%=tblFilterCriteria.ClientID%>");
            var tblFilterCriteriaMonitor = document.getElementById("<%=tblFilterCriteriaMonitor.ClientID%>");
            var tblFilterCriteriaStar = document.getElementById("<%=tblFilterCriteriaStar.ClientID%>");

            tblFilterCriteria.style.display = "none";
            tblFilterCriteriaMonitor.style.display = "none";
            tblFilterCriteriaStar.style.display = "none";

            var ddlFilter = document.getElementById("<%=ddlFilter.ClientID%>");
            var ddlFilterMonitor = document.getElementById("<%=ddlFilterMonitor.ClientID%>");
            var ddlFilterStar = document.getElementById("<%=ddlFilterStar.ClientID%>");

            ddlFilter.style.display = "none";
            ddlFilterMonitor.style.display = "none";
            ddlFilterStar.style.display = "none";

            if (ddlDeviceTypeVal == 1) {
                tblFilterCriteria.style.display = "";
                ddlFilter.style.display = "";
                ResetCriteria(tblFilterCriteriaMonitor, ddlFilterMonitor);
                ResetCriteria(tblFilterCriteriaStar, ddlFilterStar);
            }
            else if (ddlDeviceTypeVal == 2) {
                tblFilterCriteriaMonitor.style.display = "";
                ddlFilterMonitor.style.display = "";
                ResetCriteria(tblFilterCriteria, ddlFilter);
                ResetCriteria(tblFilterCriteriaStar, ddlFilterStar);
            }
            else if (ddlDeviceTypeVal == 3) {
                tblFilterCriteriaStar.style.display = "";
                ddlFilterStar.style.display = "";
                ResetCriteria(tblFilterCriteria, ddlFilter);
                ResetCriteria(tblFilterCriteriaMonitor, ddlFilterMonitor);
            }
        }

        //Clear Filter Criteria on Change
        function ResetCriteria(tblCriteria, drpFilter) {

            //Display None for All Row Elements in a Table
            for (var rowIdx = 0; rowIdx <= tblCriteria.rows.length - 1; rowIdx++) {
                var RowId = tblCriteria.rows[rowIdx].id;
                document.getElementById(RowId).style.display = "none";
            }

            //Set Index 0
            drpFilter.selectedIndex = 0;

            //Clear All Input Elements Values in a table
            var input = tblCriteria.getElementsByTagName('input');
            for (var inputIdx = 0; inputIdx <= input.length - 1; inputIdx++) {
                var InputCtrl = input[inputIdx];
                InputCtrl.value = "";
            }

            //Set All Select Elements Index 0 in a table
            var select = tblCriteria.getElementsByTagName('select');
            for (var selectIdx = 0; selectIdx <= select.length - 1; selectIdx++) {
                var selectCtrl = select[selectIdx];
                selectCtrl.selectedIndex = 0;
            }
        }

        //Slider for Browser Back Button Click
        function DisplaySearchForBrowserBack() {

            if (g_OldSite <= 0) {
                document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
            }
            else {
                document.getElementById("ctl00_headerBanner_drpsitelist").value = g_OldSite;
            }

            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'right' }, 200);

            location.href = "#";
            $('#ctl00_ContentPlaceHolder1_divSearch').show('slide', { direction: 'left' }, 900);
        }

        //Display Search Slider from Device Details
        function DisplaySearchfromDeviceDetails(isClick) {

            isButtonClicked = isClick;
            g_reachedSearch = 1;

            if (g_OldSite <= 0) {
                document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
            }
            else {
                document.getElementById("ctl00_headerBanner_drpsitelist").value = g_OldSite;
            }

            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divSearch').show('slide', { direction: 'left' }, 700);
        }

        //Display Device Details Slider on Click Device Ids
        function DisplayDeviceDetailsfromSearch(isClick) {

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
            g_OldSite = siteVal;

            isButtonClicked = isClick;
            $('#ctl00_ContentPlaceHolder1_divSearch').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').show('slide', { direction: 'right' }, 700);
        }

        //Load Device Details Grpah, Profile & 10 Hour Data
        function loadDeviceDetailsInfoOnClick(siteid, devicetype, deviceid) {

            SiteId = siteid;

            document.getElementById('tdLeftMenu').style.display = "none";
            document.getElementById('tblPictureDetails').style.display = "none";

            if (devicetype == "1")
                Device_GetPhoto(siteid, devicetype, deviceid);
            document.getElementById('divLoading_Graph').style.display = "";
            document.getElementById('Div_MSStackedColumn2D').innerHTML = "";

            DisplayDeviceDetailsfromSearch(1);

            document.getElementById('lblDeviceId_DeviceDetails').innerHTML = deviceid;

            var sTbl, sTblLen;
            var sTblShip, sTblLenShip;
            var sTblWifi, sTblLenWifi;

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

            sTblLenWifi = sTblWifi.rows.length;
            clearTableRows(sTblWifi, sTblLenWifi);

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteid;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            document.getElementById('divLoading_TagDetails').style.display = "";
            document.getElementById('divLoading_WifiDetails').style.display = "";

            document.getElementById("ctl00_ContentPlaceHolder1_trPaginationGraph").style.display = "none";
            document.getElementById('Div_MSStackedColumn2D').innerHTML = "";

            DeviceList(siteid, devicetype, deviceid);

            $('#divImages').appendTo('#div_append_Images');
            document.getElementById("divImages").style.display = "";
        };

        //EM Details
        function show_NewStatus(id) {

            if (id == 1) {
                $('#trDiagnostics').toggle();
                if ($('#trDiagnostics').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                    document.getElementById('divLoading_Graph').style.display = "";
                    ShowDateRangeGraph(2);
                }
            }
            else if (id == 2) {
                $('#trConfiguration').toggle();
                if ($('#trConfiguration').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
            else if (id == 3) {
                $('#trCertification').toggle();
                if ($('#trCertification').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
            else if (id == 4) {
                $('#divImages').toggle();
                document.getElementById('tblPictureDetails').style.display = "";

                if ($('#divImages').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
            else if (id == 5) {
                $('#trHistory').toggle();
                if ($('#trHistory').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
        }

        function chkExpand_onchange() {

            if ($('#chkExpand').prop("checked") == true) {

                document.getElementById('divLoading_Graph').style.display = "";

                ShowDateRangeGraph(2);

                document.getElementById('trDiagnostics').style.display = "";
                document.getElementById('trConfiguration').style.display = "";
                document.getElementById('trCertification').style.display = "";
                document.getElementById('divImages').style.display = "";

                $('#imgOtherSupport1').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport2').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport3').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport4').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport5').attr('src', 'Images/imgHideOthSupport.png');
            }
            else {

                document.getElementById('trDiagnostics').style.display = "none";
                document.getElementById('trConfiguration').style.display = "none";
                document.getElementById('trCertification').style.display = "none";
                document.getElementById('divImages').style.display = "none";
                document.getElementById('trHistory').style.display = "none";

                detail = getParameterByName("detail");

                show_NewStatus(detail);
            }
        }

        function loadEMDeviceDetailsInfoOnClick(siteid, devicetype, deviceid, typeid) {

            SiteId = siteid;
            document.getElementById('tdLeftMenu').style.display = "none";
            document.getElementById('tblPictureDetails').style.display = "none";

            $('#tblPictureDetails').css('width', '95.5%');
            Device_GetPhoto(siteid, devicetype, deviceid);

            isButtonClicked = 1;

            document.getElementById('lblDeviceId_DeviceDetails').innerHTML = "";
            document.getElementById('lblEMDeviceId_DeviceDetails').innerHTML = deviceid;

            $('#ctl00_ContentPlaceHolder1_divSearch').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').show('slide', { direction: 'right' }, 600);

            var sEMTTagIdbl = document.getElementById('tblTag');
            var sEMTbl = document.getElementById('tblEMTagDetails');
            var sTbEMlProfile = document.getElementById('tblEMProfile');
            var sTblEMTemperatureProfile = document.getElementById('tblEMTemperatureProfile');
            var sTblEMBattery = document.getElementById('tblEMBattery');
            var sTblEMStatus = document.getElementById('tblEMStatus');

            var sEMTblShip = document.getElementById('tblEMShippingDetails');
            var sTblEMWifi = document.getElementById('tblEMWiFiDetails');

            var sEMTTagIdblLen = sEMTTagIdbl.rows.length;
            clearTableRows(sEMTTagIdbl, sEMTTagIdblLen);

            var sEMTblLen = sEMTbl.rows.length;
            clearTableRows(sEMTbl, sEMTblLen);

            var sTblProfileLen = sTbEMlProfile.rows.length;
            clearTableRows(sTbEMlProfile, sTblProfileLen);

            var sTblTemperatureProfileLen = sTblEMTemperatureProfile.rows.length;
            clearTableRows(sTblEMTemperatureProfile, sTblTemperatureProfileLen);

            var sTblBatteryLen = sTblEMBattery.rows.length;
            clearTableRows(sTblEMBattery, sTblBatteryLen);

            var sTblEMStatusLen = sTblEMStatus.rows.length;
            clearTableRows(sTblEMStatus, sTblEMStatusLen);

            var sEMTblLenShip = sEMTblShip.rows.length;
            clearTableRows(sEMTblShip, sEMTblLenShip);

            var sEMTblLenWifi = sTblEMWifi.rows.length;
            clearTableRows(sTblEMWifi, sEMTblLenWifi);

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteid;

            document.getElementById('divLoadingEMDetails').style.display = "";
            document.getElementById('divLoadingEMGraph').style.display = "";

            document.getElementById("trEMPaginationGraph").style.display = "none";
            document.getElementById('DivEM_MSStackedColumn2D').innerHTML = "";

            EMDeviceList(siteid, devicetype, deviceid, typeid);

            $('#divImages').appendTo('#div_EM_append_Images');
            document.getElementById("divImages").style.display = "";
        };

        //Global Conn & Constant Values
        var g_ReqObj_DeviceSearch;
        var g_btnType = 1;
        var g_Page = 1;

        //Clear List and Invoke New Device Search Ajax Call
        function NewInvokeReqCount_DeviceSearch() {

            g_ReqObj_DeviceSearch = null;

            var sTbl;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblList');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblList');
            }

            delchild(sTbl, document.getElementById('tblList').getElementsByTagName("tr").length);

            document.getElementById("divLoading").style.display = "inline";
            g_Page = 1;

            InvokeReqCount_DeviceSearch();
        }

        //Call Ajax after set Pagination Control Values
        function PaginationButtonEvents(btnType) {

            var sTbl;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblList');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblList');
            }

            delchild(sTbl, document.getElementById('tblList').getElementsByTagName("tr").length);

            g_btnType = btnType;

            g_Page = document.getElementById("txtPageNo").value;

            if (g_btnType == 2) {
                g_Page = parseInt(g_Page) - 1;
            }
            else if (g_btnType == 3) {
                g_Page = parseInt(g_Page) + 1;
            }

            if (parseInt(g_Page) > parseInt(document.getElementById('lblTotalpage').innerHTML)) {
                g_Page = document.getElementById('lblTotalpage').innerHTML;
            }

            document.getElementById("divLoading").style.display = "inline";

            InvokeReqCount_DeviceSearch();
        }

        //Ajax Call Device Search
        function InvokeReqCount_DeviceSearch() {

            var drpSiteVal = document.getElementById("<%=hdnSiteId.ClientID%>").value;

            var DeviceIds = document.getElementById("<%=txtDeviceIds.ClientID%>").value;

            document.getElementById("<%=txtDeviceIds.ClientID%>").value = GetDeviceIdFormat(DeviceIds);

            var txtDeviceIds = document.getElementById("<%=txtDeviceIds.ClientID%>").value;
            var drpDTIdx = document.getElementById("<%=ddlDeviceType.ClientID%>").selectedIndex;
            var drpDTVal = document.getElementById("<%=ddlDeviceType.ClientID%>").options[drpDTIdx].value;
            var drpDTTxt = document.getElementById("<%=ddlDeviceType.ClientID%>").options[drpDTIdx].text;
            var sFilterCriteria = CreateFilterCriteria(drpDTVal, drpDTTxt);

            try {
                PageVisitDetails(g_UserId, "Search", enumPageAction.Search, "Device Searched  [" + txtDeviceIds + sFilterCriteria + "]");
            }
            catch (e) {

            }

            var dsRoot;

            $.post("AjaxConnector.aspx?cmd=DeviceSearch",
              {
                  Site: drpSiteVal,
                  DeviceType: drpDTVal,
                  DeviceId: txtDeviceIds,
                  qFilterCriteria: sFilterCriteria,
                  PageNo: g_Page
              },
              function (data, status) {
                  if (status == "success") {
                      AjaxMsgReceiver(data.documentElement);
                      dsRoot = data.documentElement;
                      ajaxInvokeReqCount_DeviceSearch(dsRoot);
                  }
                  else {
                      document.getElementById("divLoading").style.display = "none";
                  }
              });
        }

        //Load Search List from Ajax Response
        function ajaxInvokeReqCount_DeviceSearch(dsRoot) {
            var sTbl;

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblList');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblList');
            }

            var nRootLength = dsRoot.getElementsByTagName('SiteId').length;

            var PgNo = g_Page;
            document.getElementById("txtPageNo").value = PgNo;

            document.getElementById("btnExportSearch").style.display = "none";

            var nTotCount = 0;
            if (nRootLength > 0) {
                nTotCount = GetValfromRootElement(dsRoot.getElementsByTagName('TotalCount')[0]);
                document.getElementById("btnExportSearch").style.display = "";
            }

            var nPg = SetPages(nTotCount, PgNo);

            var drpDTIdx = document.getElementById("<%=ddlDeviceType.ClientID%>").selectedIndex;
            var drpDTVal = document.getElementById("<%=ddlDeviceType.ClientID%>").options[drpDTIdx].value;

            if (drpDTVal == 1) {
                var rowHeader = LoadTagInfoHeader(sTbl);
                var rowdata = LoadTagInfoOnPopup(dsRoot, sTbl);
            }
            else if (drpDTVal == 2) {
                var rowHeader = LoadMonitorInfoHeader(sTbl);
                var rowdata = LoadMonitorInfoOnPopup(dsRoot, sTbl);
            }
            else if (drpDTVal == 3) {
                var rowHeader = LoadStarInfoHeader(sTbl);
                var rowdata = LoadStarInfoOnPopup(dsRoot, sTbl);
            }

            document.getElementById("divLoading").style.display = "none";
            document.getElementById("tblPagination").style.display = "inline";
        }

        //Load Tag Header on List
        function LoadTagInfoHeader(sTbl) {

            var row = document.createElement("tr");

            AddCell(row, "Site Name", "siteOverview_TopLeft_Box", "", "", "", "250px", "40px")
            AddCell(row, "Tag Id", "siteOverview_Box", "", "", "", "140px", "40px")
            AddCell(row, "Monitor Location", "siteOverview_Box", "", "", "", "140px", "40px")
            AddCell(row, "Model Number", "siteOverview_Box", "", "", "", "150px", "40px")
            AddCell(row, "Good", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            AddCell(row, "Less than <br /> 90 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            AddCell(row, "Less than <br /> 30 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            AddCell(row, "Offline", "siteOverview_Box", "", "", "center", "100px", "40px", "");

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Support) {
                AddCell(row, "LBI Activity", "siteOverview_Box", "", "", "center", "100px", "40px", "");
                AddCell(row, "Data", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            }

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support)
                AddCell(row, "Cumulative <br />Activity Level", "siteOverview_Box", "", "", "", "150px", "40px");

            sTbl.appendChild(row);
        }

        //Load Tag Info on List
        function LoadTagInfoOnPopup(dsRoot, sTbl) {

            var row;
            var nRootLength = dsRoot.getElementsByTagName('SiteId').length;

            var sSpanCtrlId = "";
            var Last20WeekDateArr = new Array();
            var arrSplitWeekData = new Array();
            var href = "";

            if (nRootLength > 0) {

                for (var nRootIdx = 0; nRootIdx <= nRootLength - 1; nRootIdx++) {

                    var SiteId = GetValfromRootElement(dsRoot.getElementsByTagName('SiteId')[nRootIdx]);
                    var SiteName = GetValfromRootElement(dsRoot.getElementsByTagName('SiteName')[nRootIdx]);
                    var TotalPage = GetValfromRootElement(dsRoot.getElementsByTagName('TotalPage')[nRootIdx]);
                    var TotalCount = GetValfromRootElement(dsRoot.getElementsByTagName('TotalCount')[nRootIdx]);
                    var TagId = GetValfromRootElement(dsRoot.getElementsByTagName('TagId')[nRootIdx]);
                    var CatastrophicCases = GetValfromRootElement(dsRoot.getElementsByTagName('CatastrophicCases')[nRootIdx]);
                    var Offline = GetValfromRootElement(dsRoot.getElementsByTagName('Offline')[nRootIdx]);
                    var ModelItem = GetValfromRootElement(dsRoot.getElementsByTagName('ModelItem')[nRootIdx]);
                    var DeviceSubTypeId = GetValfromRootElement(dsRoot.getElementsByTagName('DeviceSubTypeId')[nRootIdx]);
                    var MonitorLocation = GetValfromRootElement(dsRoot.getElementsByTagName('MonitorLocation')[nRootIdx]);
                    var CCatastrophicCases = GetValfromRootElement(dsRoot.getElementsByTagName('CCatastrophicCases')[nRootIdx]);
                    var Last20WeekData = GetValfromRootElement(dsRoot.getElementsByTagName('LBIActivity')[nRootIdx]);
                    var CActivityLevel = GetValfromRootElement(dsRoot.getElementsByTagName('CActivityLevel')[nRootIdx]);

                    var s30DaysCell = "";
                    var s90DaysCell = "";
                    var offline = "";
                    var sgood = ""

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

                    if (Offline == "1")
                        offline = "<img src='images/Close_1.png' border='0' />"
                    else
                        offline = ""

                    row = document.createElement("tr");

                    AddCell(row, SiteName, "DeviceList_leftBox", "", "", "", "250px", "40px");

                    IsEM = IsEMTag(DeviceSubTypeId);

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {

                        if (IsEM == "1")
                            href = "<a  class='DeviceDetailsLink' href=#EMdeviceDetail onclick=loadEMDeviceDetailsInfoOnClick(" + SiteId + ",1," + TagId + ")>" + TagId + "</a>";
                        else 
                            href = "<a  class='DeviceDetailsLink' href=#divDeviceInfo onclick=loadDeviceDetailsInfoOnClick(" + SiteId + ",1," + TagId + ")>" + TagId + "</a>";

                        AddCell(row, href, 'siteOverview_cell', "", "", "center", "140px", "40px", "");
                    }
                    else
                        AddCell(row, TagId, 'siteOverview_cell', "", "", "center", "140px", "40px", "");

                    AddCell(row, MonitorLocation, "siteOverview_cell", "", "", "", "150px", "40px");
                    AddCell(row, ModelItem, "siteOverview_cell", "", "", "", "150px", "40px");
                    AddCell(row, sgood, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                    AddCell(row, s90DaysCell, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                    AddCell(row, s30DaysCell, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                    AddCell(row, offline, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Support) {

                        // LBI Activity for spark line chart
                        sSpanCtrlId = "dynamicsparkline" + TagId + SiteId;
                        var sSpanCtrl = "<span id='" + sSpanCtrlId + "'>...</span>";
                        AddCell(row, sSpanCtrl, "siteOverview_cell", "", "", "center", "110px", "40px", "");

                        var sActivityUrl = "<a href=TagLBIActivityReport.aspx?qSiteId=" + SiteId + "&qTagId=" + TagId + "&qExport=1><img style='cursor: pointer;' src='images/downloadfile.png' /></a>"
                        AddCell(row, sActivityUrl, "siteOverview_cell", "", "", "center", "70px", "40px", "");
                    }

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support)
                        AddCell(row, CActivityLevel, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

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

                row = document.createElement("tr");
                AddCell(row, "No Records found.....", "noRecoredfound", 4, "", "center", "", "40px")
                sTbl.appendChild(row);
            }
        }

        //Load Monitor Header on List
        function LoadMonitorInfoHeader(sTbl) {

            var row = document.createElement("tr");
            AddCell(row, "Site Name", "siteOverview_TopLeft_Box", "", "", "", "250px", "40px")
            AddCell(row, "Monitor Id", "siteOverview_Box", "", "", "140px", "", "40px")
            AddCell(row, "Monitor Location", "siteOverview_Box", "", "", "140px", "", "40px")
            AddCell(row, "Model Number", "siteOverview_Box", "", "", "", "160px", "40px")
            AddCell(row, "Good", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            AddCell(row, "Less than <br /> 90 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            AddCell(row, "Less than <br /> 30 Days", "siteOverview_Box", "", "", "center", "100px", "40px", "");
            AddCell(row, "Offline", "siteOverview_Box", "", "", "center", "100px", "40px", "");

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Support) {
                AddCell(row, "LBI Activity", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
                AddCell(row, "Data", 'siteOverview_Topright_Box', "", "", "center", "70px", "40px", "");
            }

            sTbl.appendChild(row);
        }

        //Load Monitor Info on List
        function LoadMonitorInfoOnPopup(dsRoot, sTbl) {

            var row;
            var nRootLength = dsRoot.getElementsByTagName('SiteId').length;

            var sSpanCtrlId = "";
            var Last20WeekDateArr = new Array();
            var arrSplitWeekData = new Array();

            if (nRootLength > 0) {

                for (var nRootIdx = 0; nRootIdx <= nRootLength - 1; nRootIdx++) {

                    var SiteId = GetValfromRootElement(dsRoot.getElementsByTagName('SiteId')[nRootIdx]);
                    var SiteName = GetValfromRootElement(dsRoot.getElementsByTagName('SiteName')[nRootIdx]);
                    var TotalPage = GetValfromRootElement(dsRoot.getElementsByTagName('TotalPage')[nRootIdx]);
                    var TotalCount = GetValfromRootElement(dsRoot.getElementsByTagName('TotalCount')[nRootIdx]);
                    var DeviceId = GetValfromRootElement(dsRoot.getElementsByTagName('DeviceId')[nRootIdx]);
                    var CatastrophicCases = GetValfromRootElement(dsRoot.getElementsByTagName('CatastrophicCases')[nRootIdx]);
                    var Offline = GetValfromRootElement(dsRoot.getElementsByTagName('Offline')[nRootIdx]);
                    var ModelItem = GetValfromRootElement(dsRoot.getElementsByTagName('ModelItem')[nRootIdx]);
                    var MonitorLocation = GetValfromRootElement(dsRoot.getElementsByTagName('RoomName')[nRootIdx]);
                    var Last20WeekData = GetValfromRootElement(dsRoot.getElementsByTagName('LBIActivity')[nRootIdx]);
                    var CCatastrophicCases = GetValfromRootElement(dsRoot.getElementsByTagName('CCatastrophicCases')[nRootIdx]);

                    row = document.createElement("tr");
                    AddCell(row, SiteName, "DeviceList_leftBox", "", "", "", "250px", "40px");

                    var s30DaysCell = "";
                    var s90DaysCell = "";
                    var offline = "";
                    var sgood = ""

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

                    if (Offline == "1")
                        offline = "<img src='images/Close_1.png' border='0' />"
                    else
                        offline = ""

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {
                        var href = "<a  class='DeviceDetailsLink' href=#divDeviceInfo onclick=loadDeviceDetailsInfoOnClick(" + SiteId + ",2," + DeviceId + ")>" + DeviceId + "</a>";
                        AddCell(row, href, 'siteOverview_cell', "", "", "center", "200px", "40px", "");
                    }
                    else
                        AddCell(row, DeviceId, 'siteOverview_cell', "", "", "center", "140px", "40px", "");

                    AddCell(row, MonitorLocation, "siteOverview_cell", "", "", "", "150px", "40px");
                    AddCell(row, ModelItem, "siteOverview_cell", "", "", "", "160px", "40px");
                    AddCell(row, sgood, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                    AddCell(row, s90DaysCell, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                    AddCell(row, s30DaysCell, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
                    AddCell(row, offline, 'siteOverview_cell', "", "", "center", "100px", "40px", "");

                    // Feature #28732 LBI Data in device table (Internal Support+ only)
                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Support) {

                        // LBI Activity for spark line chart
                        sSpanCtrlId = "dynamicInfrasparkline" + DeviceId;
                        var sSpanCtrl = "<span id='" + sSpanCtrlId + "'>...</span>";
                        AddCell(row, sSpanCtrl, "siteOverview_cell", "", "", "center", "110px", "40px", "");

                        var sActivityUrl = "<a href=MonitorLBIActivityReport.aspx?qSiteId=" + SiteId + "&qMonitorId=" + DeviceId + "&qExport=1><img style='cursor: pointer;' src='images/downloadfile.png' /></a>"
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

                row = document.createElement("tr");
                AddCell(row, "No Records found.....", "noRecoredfound", 4, "", "center", "", "40px")
                sTbl.appendChild(row);
            }
        }

        //Load Star Header on List
        function LoadStarInfoHeader(sTbl) {
            
            $('#tblList').css('width', '100%');

            var row = document.createElement("tr");
            AddCell(row, "Site Name", "siteOverview_TopLeft_Box", "", "", "", "250px", "40px");
            AddCell(row, "Mac Id", "siteOverview_Box", "", "", "", "140px", "40px");
            AddCell(row, "Star Type", "siteOverview_Box", "", "", "", "150px", "40px");
            AddCell(row, "IP", "siteOverview_Box", "", "", "", "160px", "40px");
            AddCell(row, "Model Number", "siteOverview_Topright_Box", "", "", "", "150px", "40px");
            sTbl.appendChild(row);
        }

        //Load Star info on List
        function LoadStarInfoOnPopup(dsRoot, sTbl) {

            var row = "";
            var nRootLength = dsRoot.getElementsByTagName('SiteId').length;

            if (nRootLength > 0) {

                for (var nRootIdx = 0; nRootIdx <= nRootLength - 1; nRootIdx++) {

                    var SiteId = GetValfromRootElement(dsRoot.getElementsByTagName('SiteId')[nRootIdx]);
                    var SiteName = GetValfromRootElement(dsRoot.getElementsByTagName('SiteName')[nRootIdx]);
                    var TotalPage = GetValfromRootElement(dsRoot.getElementsByTagName('TotalPage')[nRootIdx]);
                    var MACId = GetValfromRootElement(dsRoot.getElementsByTagName('MACId')[nRootIdx]);
                    var DeviceName = GetValfromRootElement(dsRoot.getElementsByTagName('DeviceName')[nRootIdx]);
                    var StarType = GetValfromRootElement(dsRoot.getElementsByTagName('StarType')[nRootIdx]);
                    var IPAddr = GetValfromRootElement(dsRoot.getElementsByTagName('IPAddr')[nRootIdx]);
                    var ModelItem = GetValfromRootElement(dsRoot.getElementsByTagName('ModelItem')[nRootIdx]);

                    row = document.createElement("tr");

                    AddCell(row, SiteName, "DeviceList_leftBox", "", "", "", "250px", "40px")

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {
                        var href = "<a  class='DeviceDetailsLink' href=#divDeviceInfo onclick=loadDeviceDetailsInfoOnClick(" + SiteId + ",3,'" + MACId + "')>" + MACId + "</a>";
                        AddCell(row, href, 'DeviceList_leftBox', "", "", "center", "140px", "40px", "");
                    }
                    else
                        AddCell(row, MACId, 'DeviceList_leftBox', "", "", "center", "140px", "40px", "");

                    AddCell(row, StarType, "siteOverview_cell", "", "", "", "150px", "40px");
                    AddCell(row, IPAddr, "siteOverview_cell", "", "", "", "160px", "40px");
                    AddCell(row, ModelItem, "siteOverview_cell", "", "", "", "160px", "40px");

                    sTbl.appendChild(row);
                }
            }
            else {
                row = document.createElement("tr");
                AddCell(row, "No Records found.....", "noRecoredfound", 3, "", "center", "", "40px")
                sTbl.appendChild(row);
            }
        }

        //Get Value from Xml Root Element
        function GetValfromRootElement(rootElement) {

            return setundefined((rootElement.textContent || rootElement.innerText || rootElement.text));
        }

        //Create Filter Criteria for Selected Filters
        function CreateFilterCriteria(DeviceType, DeviceTypeTxt) {

            var tblCriteria;
            var ddlFilter;
            var sFilterCriteria = "";

            var isdate = false;

            if (DeviceType == 1)
                ddlFilter = document.getElementById("<%=ddlFilter.ClientID%>");
            else if (DeviceType == 2)
                ddlFilter = document.getElementById("<%=ddlFilterMonitor.ClientID%>");
            else if (DeviceType == 3)
                ddlFilter = document.getElementById("<%=ddlFilterStar.ClientID%>");

            for (var rowIdx = 0; rowIdx <= ddlFilter.options.length - 1; rowIdx++) {

                var _selectedFilterFeild = ddlFilter.options[rowIdx].value;
                var FilterText = ddlFilter.options[rowIdx].text;

                if (_selectedFilterFeild != "0") {

                    var strCtrl = _selectedFilterFeild + DeviceTypeTxt;

                    //Row Control
                    var RowCtrl = document.getElementById("ctl00_ContentPlaceHolder1_" + strCtrl);
                    var RowId = RowCtrl.id;

                    if (RowCtrl != null) {

                        if (RowCtrl.style.display == "inline" || RowCtrl.style.display == "") {

                            var selectCndtCtrl = document.getElementById("lst" + strCtrl + "Filter");
                            var selectCndtId = selectCndtCtrl.id;

                            //Filter Operator
                            var _selectedOperator = selectCndtCtrl.options[selectCndtCtrl.selectedIndex].value;
                            var selectCndtText = selectCndtCtrl.options[selectCndtCtrl.selectedIndex].text;

                            //Filter Value 1
                            var _selectedFilterValue = document.getElementById("txt" + strCtrl + "1").value;

                            //Filter Value 2
                            var _selectedFilterValue2 = "";

                            if (_selectedOperator == "5") // between
                            {
                                _selectedFilterValue2 = document.getElementById("txt" + strCtrl + "2").value;
                            }

                            sFilterCriteria = sFilterCriteria + _selectedFilterFeild + "~" + _selectedOperator + "~" + _selectedFilterValue + "~" + _selectedFilterValue2 + "|";
                        }
                    }
                }
            }

            return sFilterCriteria;
        }

        //Add Zeros in Date(Month & Day) Or Integer
        function AddZeros(val) {

            if (val < 10) {
                val = "0" + val;
            }

            return val;
        }

        //Set Pagination
        function SetPages(nTotCount, nCurrPage) {

            var nPgCnt = 0;
            var nRowCnt = 10;

            nPgCnt = Math.ceil(nTotCount / nRowCnt);

            if (nPgCnt == 1) {
                nCurrPage = 1;
                document.getElementById('btnPrev').visible = false;
                document.getElementById('btnNext').visible = false;
            }
            else {
                document.getElementById('btnPrev').visible = true;
                document.getElementById('btnNext').visible = true;
            }

            if (nCurrPage <= 1) {
                nCurrPage = 1;
                document.getElementById('btnPrev').style.display = "none";
            }
            else {
                document.getElementById('btnPrev').style.display = "";
            }

            if (nCurrPage >= nPgCnt) {
                nCurrPage = nPgCnt;
                document.getElementById('btnNext').style.display = "none";
            }
            else {
                document.getElementById('btnNext').style.display = "";
            }

            document.getElementById('lbltotalcount').innerHTML = nTotCount;
            document.getElementById('txtPageNo').value = nCurrPage;
            document.getElementById('lblTotalpage').innerHTML = nPgCnt;

            return nCurrPage
        }

        //Clear Filters on Click Clear Button
        function ClearFilters() {

            var tblFilterCriteria = document.getElementById("<%=tblFilterCriteria.ClientID%>");
            var tblFilterCriteriaMonitor = document.getElementById("<%=tblFilterCriteriaMonitor.ClientID%>");
            var tblFilterCriteriaStar = document.getElementById("<%=tblFilterCriteriaStar.ClientID%>");

            tblFilterCriteria.style.display = "none";
            tblFilterCriteriaMonitor.style.display = "none";
            tblFilterCriteriaStar.style.display = "none";

            var ddlFilter = document.getElementById("<%=ddlFilter.ClientID%>");
            var ddlFilterMonitor = document.getElementById("<%=ddlFilterMonitor.ClientID%>");
            var ddlFilterStar = document.getElementById("<%=ddlFilterStar.ClientID%>");

            ddlFilter.style.display = "none";
            ddlFilterMonitor.style.display = "none";
            ddlFilterStar.style.display = "none";

            ResetCriteria(tblFilterCriteria, ddlFilter);
            ResetCriteria(tblFilterCriteriaMonitor, ddlFilterMonitor);
            ResetCriteria(tblFilterCriteriaStar, ddlFilterStar);

            tblFilterCriteria.style.display = "";
            ddlFilter.style.display = "";

            var sTbl;

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblList');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblList');
            }

            delchild(sTbl, document.getElementById('tblList').getElementsByTagName('tr').length);

            document.getElementById("tblPagination").style.display = "none";
            document.getElementById('lbltotalcount').innerHTML = "";
            document.getElementById('txtPageNo').value = "1";
            document.getElementById('lblTotalpage').innerHTML = "";

            //Enable Tag
            document.getElementById("<%=ddlDeviceType.ClientID%>").value = "1";

            tblFilterCriteria.style.display = "";
            ddlFilter.style.display = "";
        }

        //Search Menu Styles
        var defaultClass = "";

        $(document).ready(function () {

            $('#spnSearch').on('click', function () {

                $('#TagMovements').hide();
                $('#SearchFilter').show(300);
                $('#spnSearch').removeClass('current');
                $('#spnTagMovements').addClass('current');
            });

            $('#spnTagMovements').on('click', function () {

                $('#SearchFilter').hide();
                $('#TagMovements').show(300);
                $('#spnTagMovements').removeClass('current');
                $('#spnSearch').addClass('current');
            });

        });

        //Display Tag Movements on Button Click
        function ShowTagMovements() {

            var SiteId = document.getElementById("<%=ddlSiteTagMovements.ClientID%>").options[document.getElementById("<%=ddlSiteTagMovements.ClientID%>").selectedIndex].value;
            var DeviceId = document.getElementById("txtDeviceId").value;
            var FromDate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
            var FromTime = document.getElementById("txtFromTime").value;
            var ToTime = document.getElementById("txtToTime").value;

            LoadTagMovements(SiteId, DeviceId, FromDate + " " + FromTime, ToDate + " " + ToTime);
        }

        //*******************************************************************
        //	Function Name	:	ExportSearch
        //	Input			:	None
        //	Description		:	Export Search (ajax Call)
        //*******************************************************************
        var g_ExportObj;

        function ExportSearch() {

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            var typeIdx = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType").selectedIndex;
            var typeVal = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType").options[typeIdx].value;
            var typeTxt = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType").options[typeIdx].text;

            var DeviceId = document.getElementById("ctl00_ContentPlaceHolder1_txtDeviceIds").value;

            var sFilterCriteria = CreateFilterCriteria(typeVal, typeTxt);

            if (GetBrowserType() == "isIE") {
                location.href = "AjaxConnector.aspx?cmd=DeviceSearch_IE&Site=" + siteVal + "&DeviceType=" + typeVal + "&isExportSearch=1&DeviceId=" + DeviceId + "&qFilterCriteria=" + sFilterCriteria + "&IsIE=1";
                return;
            }

            var dsRoot;
            document.getElementById("divExcelLoading_Search").style.display = "";

            $.post("AjaxConnector.aspx?cmd=DeviceSearch",
              {
                  Site: siteVal,
                  DeviceType: typeVal,
                  isExportSearch: "1",
                  DeviceId: DeviceId,
                  qFilterCriteria: sFilterCriteria,
                  PageNo: "0",
                  IsIE: "0"
              },
              function (data, status) {
                  if (status == "success") {
                      AjaxMsgReceiver(data.documentElement);
                      dsRoot = data.documentElement;
                      ajaxExportSearch(dsRoot);
                  }
                  else {
                      document.getElementById("divExcelLoading_Search").style.display = "none";
                  }
              });
        }

        //*******************************************************************
        //	Function Name	:	ajaxExportSearch
        //	Input			:	None
        //	Description		:	Export Infomation into Excel from ajax Response
        //*******************************************************************
        function ajaxExportSearch(dsRoot) {

            var o_Excel = dsRoot.getElementsByTagName('Excel');
            var o_Filename = dsRoot.getElementsByTagName('Filename');

            var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
            var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

            //Export table string to CSV
            tableToCSV(Excel, Filename);

            document.getElementById("divExcelLoading_Search").style.display = "none";
        }

        function Validate() {

            var DeviceId, FrmDate, ToDate, Site, FromTime, ToTime;

            DeviceId = document.getElementById('txtDeviceId');
            FrmDate = document.getElementById('txtFromDate');
            ToDate = document.getElementById('txtToDate');
            var Site = document.getElementById("ctl00_ContentPlaceHolder1_ddlSiteTagMovements");
            var SiteValue = Site.options[Site.selectedIndex].value;
            FromTime = document.getElementById('txtFromTime');
            ToTime = document.getElementById('txtToTime');

            if (SiteValue == 0) {
                alert("Select Site");
                Site.focus();
                return false;
            }
            if (DeviceId.value == "") {
                alert("Enter DeviceID");
                DeviceId.focus();
                return false;
            }
            if (DeviceId.value != "") {
                var isNum = OnlyNumValidation(DeviceId.value)
                if (!isNum) {
                    alert("Enter Numeric DeviceID");
                    return false;
                }
            }
            if (FrmDate.value == "") {
                alert("Select FromDate");
                FrmDate.focus();
                return false;
            }
            if (FromTime.value == "") {
                alert("Select From Time");
                FromTime.focus();
                return false;
            }
            if (FrmDate.value != "") {
                var isDateVali = dateValidation(FrmDate.value);
                if (!isDateVali) {
                    alert("Enter Valid Date Format");
                    return false;
                }
            }
            if (ToDate.value == "") {
                alert("Select To Date");
                ToDate.focus();
                return false;
            }
            if (ToTime.value == "") {
                alert("Select To Time");
                ToTime.focus();
                return false;
            }
            if (ToDate.value != "") {
                var isDateVali = dateValidation(ToDate.value);
                if (!isDateVali) {
                    alert("Enter Valid To Date Format");
                    return false;
                }
            }

            return true;
        }

        function dateValidation(date_val) {

            var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
            return date_regex.test(date_val);
        }

        function OnlyNumValidation(Num) {

            var NumVal = /^[0-9+]*$/;
            return NumVal.test(Num);
        }

        function showTip4(text, lf, tp) {

            var elementRef = document.getElementById('tooltip4');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip4").css({ left: tempX, top: tempY });
        }

        function hideTip4() {
            var elementRef = document.getElementById('tooltip4');
            elementRef.style.display = 'none';
        }
       
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 10px;">
        </tr>
        <tr>
            <td style="padding-left: 20px; padding-right: 20px;" align="center">
                <div id="divSearch" runat="server" style="display: none; top: auto; left: auto; height: 850px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="height: 10px;">
                            <td>
                                &nbsp;
                                <input type="hidden" id="hdnSiteId" runat="server" />
                                <input type="hidden" id="hid_userrole" runat="server" />
                                <input type="hidden" id="hid_userid" runat="server" />
                                <input type="hidden" id="hid_devicetype" />
                                <input type="hidden" id="hid_IsTempMonitoring" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr id="searchmenu" runat="server">
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <div id="tabs1">
                                                            <ul>
                                                                <!-- CSS Tabs -->
                                                                <li id="spnSearch"><a><span>Device Search</span></a></li>
                                                                <li id="spnTagMovements" class="current"><a><span>Tag Movements</span></a></li>
                                                            </ul>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;">
                                        </td>
                                    </tr>
                                    <tr id="SearchFilter">
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 40%;" valign="top">
                                                        <table border="0" cellpadding="2" cellspacing="2" width="400px">
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;">
                                                                    <b>Filter&nbsp;:</b>
                                                                </td>
                                                                <td style="width: 300px;" align="left">
                                                                    <select id="ddlFilter" runat="server" style="width: 220px; display: none;" onchange="showFilterCtrl(this)">
                                                                    </select>
                                                                    <select id="ddlFilterMonitor" runat="server" style="width: 220px; display: none;"
                                                                        onchange="showFilterCtrl(this)">
                                                                    </select>
                                                                    <select id="ddlFilterStar" runat="server" style="width: 220px; display: none;" onchange="showFilterCtrl(this)">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;">
                                                                    <b>Device&nbsp;Type&nbsp;:</b>
                                                                </td>
                                                                <td style="width: 300px;" align="left">
                                                                    <select id="ddlDeviceType" runat="server" style="width: 80px;" onchange="ShowHideDeviceTypeCtrls();">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;" valign="top">
                                                                    <b>Device&nbsp;Id&nbsp;:</b>
                                                                </td>
                                                                <td style="width: 300px;" align="left">
                                                                    <textarea id="txtDeviceIds" runat="server" style="width: 220px" class="clsTextAreaBox"
                                                                        cols="60" rows="3" maxlength="8000"></textarea>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 60%;" valign="top">
                                                        <table id="tblFilterCriteria" runat="server" border="0" cellpadding="0" cellspacing="0"
                                                            width="100%" style="display: none;">
                                                        </table>
                                                        <table id="tblFilterCriteriaMonitor" runat="server" border="0" cellpadding="0" cellspacing="0"
                                                            width="100%" style="display: none;">
                                                        </table>
                                                        <table id="tblFilterCriteriaStar" runat="server" border="0" cellpadding="0" cellspacing="0"
                                                            width="100%" style="display: none;">
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 30px;">
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 20px; padding-right: 20px;" align="right">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <input type="button" id="btnShow" runat="server" value="Show" class="clsButton" onclick="NewInvokeReqCount_DeviceSearch();" />
                                                                    &nbsp;&nbsp;
                                                                    <input type="button" id="btnClear" runat="server" value="Clear" class="clsButton"
                                                                        onclick="ClearFilters();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="display: none;"
                                                                        id="tblPagination">
                                                                        <tr>
                                                                            <td class="txttotalpage" style="width: 350px;">
                                                                                Total&nbsp;Devices&nbsp;:&nbsp;<label id="lbltotalcount"></label>
                                                                                &nbsp;&nbsp;
                                                                                <input type="button" id="btnExportSearch" class="clsExportExcel" value="Export Excel"
                                                                                    title="Export Excel" onclick="ExportSearch();" />
                                                                                &nbsp;&nbsp;
                                                                                <div style="display: none; float: right;" id="divExcelLoading_Search">
                                                                                    <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                                                </div>
                                                                            </td>
                                                                            <td style="width: 100px;">
                                                                            </td>
                                                                            <td class="clsTableTitleText" align="right" style="width: 400px;">
                                                                                <input type="button" id="btnPrev" class="clsPrev" onclick="PaginationButtonEvents(2);"
                                                                                    title="Previous" />
                                                                                <label id="lblPage" class="clsCntrlTxt">
                                                                                    Page
                                                                                </label>
                                                                                <input id="txtPageNo" onkeypress="return allowNumberOnly(event)" type="text" size="1"
                                                                                    maxlength="4" name="txtPageNo" value="1" />
                                                                                <label id="lblTotalpage" class="clsCntrlTxt">
                                                                                    &nbsp;</label>
                                                                                <input type="button" id="btnGo" class="btnGO" value="Go" onclick="PaginationButtonEvents(1);" />
                                                                                <input type="button" id="btnNext" class="clsNext" onclick="PaginationButtonEvents(3);"
                                                                                    title="Next" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                                        <td>
                                            <table id="tblList" border="0" cellpadding="0" cellspacing="0" width="150%">
                                            </table>
                                            <div style="position: fixed; top: 400px; left: 1200px; display: none;" id="divLoading">
                                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="TagMovements" style="display: none;">
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 40%;" valign="top">
                                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;">
                                                                    <b>Site&nbsp;:</b>
                                                                </td>
                                                                <td align="left">
                                                                    <select id="ddlSiteTagMovements" runat="server" style="width: 450px;">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;">
                                                                    <b>Device&nbsp;Id&nbsp;:</b>
                                                                </td>
                                                                <td style="width: 300px;" align="left">
                                                                    <input type="text" id="txtDeviceId" style="width: 220px;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;" valign="middle">
                                                                    <b>From&nbsp;Date&nbsp;:</b>
                                                                </td>
                                                                <td style="width: 300px;" align="left">
                                                                    <input type="text" id="txtFromDate" style="width: 80px;" />
                                                                    <input type="text" id="txtFromTime" value="00:00" style="width: 80px;" />
                                                                    <script type='text/javascript'>
                                                                        Calendar.setup({
                                                                            inputField: 'txtFromDate',
                                                                            ifFormat: '%m/%d/%Y',
                                                                            button: 'txtFromDate',
                                                                            align: 'Br'
                                                                        });

                                                                        $(document).ready(function () {
                                                                            $('#' + "txtFromTime").timeEntry({ show24Hours: true, showSeconds: true });
                                                                        });
                                                                    </script>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdSubText" style="width: 100px;" valign="middle">
                                                                    <b>To&nbsp;Date&nbsp;:</b>
                                                                </td>
                                                                <td style="width: 300px;" align="left">
                                                                    <input type="text" id="txtToDate" style="width: 80px;" />
                                                                    <input type="text" id="txtToTime" value="00:00" style="width: 80px;" />
                                                                    <script type='text/javascript'>
                                                                        Calendar.setup({
                                                                            inputField: 'txtToDate',
                                                                            ifFormat: '%m/%d/%Y',
                                                                            button: 'txtToDate',
                                                                            align: 'Br'
                                                                        });

                                                                        $(document).ready(function () {
                                                                            $('#' + "txtToTime").timeEntry({ show24Hours: true, showSeconds: true });
                                                                        });
                                                                    </script>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 60%;" valign="top">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="height: 30px;">
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-left: 20px; padding-right: 20px;" align="right">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <input type="button" id="btnShowTag" value="Show" class="clsButton" onclick="if(! Validate()) return false; ShowTagMovements(); " />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <!-- <div id="divChart_TagMovement">
                                                        </div>-->
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 20px; padding-right: 20px;" align="center" colspan="2">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="height: 850px;" valign="top">
                                                                    <div style="position: fixed; top: 500px; left: 800px; display: none;" id="divLoading_TagMovement">
                                                                        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                                                    </div>
                                                                    <table id="tblTagMovementList" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <script type="text/javascript">
                            LoadGlossaryInfo("Search", document.getElementById("<%=hid_userrole.ClientID%>").value);
                            showGlossaryInfo("Search");
                        </script>
                    </table>
                </div>
                <!-- TOOL TIP-->
                <div id="tooltip4" class="tool3" style="display: none;">
                </div>
                <div id="divDeviceDetails" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellspacing="0" cellpadding="2" border="0" style="width: 85%;">
                        <tr>
                            <td>
                                <table cellpadding="0" style="width: 100%">
                                    <tr style="height: 20px;">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a href="#divSearch" onclick="DisplaySearchfromDeviceDetails(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <label id="lblDeviceId_DeviceDetails">
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>
                                                                    Device Details
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="div_append_Images">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 100%;">
                                            <div style="position: relative; display: none; width: 100%; left: 350px;" id="divLoading_TagDetails">
                                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                            </div>
                                            <table id="tblTagDetails" cellspacing="1" cellpadding="5" style="width: 100%; padding-top: 15px;
                                                display: none;">
                                            </table>
                                            <table id="tblProfile" cellspacing="1" cellpadding="5" style="width: 100%; padding-top: 15px;
                                                display: none;">
                                            </table>
                                            <table id="tblWifiProfile" cellspacing="1" cellpadding="5" style="width: 100%; padding-top: 15px;
                                                display: none;">
                                            </table>
                                            <table id="tblTemperatureProfile" cellspacing="1" cellpadding="5" style="width: 100%;
                                                padding-top: 15px; display: none;">
                                            </table>
                                            <table id="tblBattery" cellspacing="1" cellpadding="5" style="width: 100%; padding-top: 15px;
                                                display: none;">
                                            </table>
                                            <table id="tblStatus" cellspacing="1" cellpadding="5" style="width: 100%; padding-top: 15px;
                                                display: none;">
                                            </table>
                                            <table id="tblShippingDetails" cellspacing="1" cellpadding="5" style="width: 100%;
                                                padding-top: 15px; display: none;">
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 25px;">
                                    </tr>                                   
                                    <tr>
                                        <td valign="top" align="left">
                                            <table id="Table1" runat="server" cellspacing="0" cellpadding="0" border="0" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td style="height: 10px;" class="subHeader_black">
                                                                    Paging and Location Data&nbsp;:&nbsp;<asp:label id="lblPagingDataDateFrom" runat="Server"></asp:label>
                                                                    <asp:label id="lblPagingDataDateTo" runat="Server"></asp:label>
                                                                    &nbsp;<asp:label id="lblPagingDataNoOfDays" runat="Server"></asp:label>
                                                                </td>
                                                                <td style="width: 10px;" valign="top">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                </tr>
                                                <tr id="trPaginationGraph" style="display: none;">
                                                    <td align="right">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="300px" style="background-color: #F0EDED;">
                                                            <tr style="height: 36px;">
                                                                <td align="left">
                                                                    <%--<img src="images/btnLAAlertPrev.png" id="btnPrevGraph" runat="server" width="30"
                                                                                height="30" onclick="ShowPreviousNextGraph(2);" />--%>
                                                                    <input type="button" id="btnPrevGraph" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                                        onmouseout="btnPagination_DeviceDetailsOut(this);" runat="server" value=" << "
                                                                        onclick="ShowPreviousNextGraph(3);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                    <label id="lblDeviceDetails_DateType" class="clsLALabel" style="width: 150px;">
                                                                    </label>
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsMonthly.png" id="btnMonthly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                                        onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(4);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsWeekly.png" id="btnWeekly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                                        onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(3);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsDaily.png" id="btnDaily_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                                        onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(2);" />
                                                                </td>
                                                                <td>
                                                                    <img src="images/imgLAAlertsHourly.png" id="btnHourly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                                        onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(1);" />
                                                                </td>
                                                                <td style="width: 15px;">
                                                                </td>
                                                                <td>
                                                                    <%--<img src="images/imgLAAlertNext.png" id="btnNextGraph" runat="server" width="30"
                                                                                height="30" onclick="ShowPreviousNextGraph(3);" />--%>
                                                                    <input type="button" id="btnNextGraph" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                                        onmouseout="btnPagination_DeviceDetailsOut(this);" runat="server" value=" >> "
                                                                        onclick="ShowPreviousNextGraph(2);" />
                                                                </td>
                                                                <td style="width: 10px;">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                </tr>
                                                <tr valign="top" style="height: 5px;">
                                                    <td>
                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                            <tr id="trLBIDiff" style="display: none;">
                                                                <td align="left">
                                                                    <input type="checkbox" id="chkIsShowLBIADC" onchange="showHideLBIADC();" style="vertical-align: middle;" />&nbsp;
                                                                    <asp:label id="lblShowLBIADC" runat="server" cssclass="clsLALabel" style="font-size: 12px;
                                                                        color: #222222;">Show&nbsp;LBI&nbsp;ADC</asp:label>
                                                                    <input type="checkbox" id="ChkIsShowLBIDiff" onchange="showHideLBIDiff();" style="vertical-align: middle;" />&nbsp;
                                                                    <asp:label id="LblLBIDiff" runat="server" cssclass="clsLALabel" style="font-size: 12px;
                                                                        color: #222222;">Show&nbsp;LBI&nbsp;Diff</asp:label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                </tr>
                                                <tr id="tr3" runat="server">
                                                    <td align='left' id="tdTrendGraph" style="width: 5%; height: 20px; text-align: left;">
                                                        <div style="position: relative; display: none; width: 100%; left: 350px;" id="divLoading_Graph">
                                                            <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                                        </div>
                                                        <div id="Div_MSStackedColumn2D" style="height: 480px;">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr id="trWiFiDetailsHeader" runat="server">
                                        <td class="subHeader_black">
                                            Last&nbsp;10&nbsp;Hr&nbsp;Location&nbsp;Data:&nbsp;
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 100%;">
                                            <div style="position: relative; display: none; width: 100%; left: 350px;" id="divLoading_WifiDetails">
                                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                            </div>
                                            <div style="width: 900px; overflow-x: auto; overflow-y: hidden;">
                                                <table id="tblWiFiDetails" border="0" cellspacing="1" cellpadding="5" width="2500px">
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- EM DEVICE DETAILS PAGE-->
                <div id="divEMDeviceDetails" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <div style="position: fixed; width: 50%; top: 50%;" id="divLoadingEMDetails">
                        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                    </div>
                    <table id="tblEMStatusSummary" style="width: 100%;">
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a href="#divSearch" onclick="DisplaySearchfromDeviceDetails(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' border="0" /></a>
                                                    </td>
                                                    <td style='width: 10px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top">
                                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                            <tr>
                                                                <td align='left' style="width: 80%;" class='SHeader1'>
                                                                    <label>
                                                                        Status Summary
                                                                    </label>
                                                                </td>
                                                                <td class='SHeader1' align="right">
                                                                    <input type="checkbox" id="chkExpand" name="chkExpand" onchange="chkExpand_onchange()" /><label>Expand
                                                                        All</label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' style="width: 80%;" class='subHeader_black' id="lblEMDeviceId_DeviceDetails">
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 5px;">
                                    </tr>
                                    <tr style="height: 5px;">
                                        <td class="bordertop" valign="top">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblTag" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td style="width: 22px;">
                                            <img id="imgOtherSupport1" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                                width: 30px;" onclick="show_NewStatus(1);" />
                                        </td>
                                        <td colspan="3" class="Prod_subTitle">
                                            Diagnostics
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trDiagnostics" style="display: none;">
                            <td>
                                <table id="tblEMTemperatureProfile" cellspacing="1" cellpadding="5" style="width: 95.5%;
                                    padding-top: 15px;">
                                </table>
                                <table id="tblEMBattery" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                </table>
                                <table id="tblEMStatus" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                </table>
                                <table id="tblEMWiFiDetailsHeader" cellspacing="1" cellpadding="5" style="width: 95.5%;
                                    padding-top: 15px;">                                   
                                    <tr>
                                        <td class="subHeader_black">
                                            Last&nbsp;10&nbsp;Hr&nbsp;Location&nbsp;Data:&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblEMWiFiDetails" cellspacing="1" cellpadding="5" style="width: 950px;
                                    table-layout: fixed; padding-top: 15px;">
                                </table>
                                <table id="tblEMShippingDetails" cellspacing="1" cellpadding="5" style="width: 95.5%;
                                    padding-top: 15px;">
                                </table>
                                <table id="tblEMPageLocationGraph" cellspacing="1" cellpadding="5" style="width: 95.5%;
                                    padding-top: 15px;">
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td style="height: 10px;" class="subHeader_black">
                                                        Paging and Location Data&nbsp;:&nbsp;<asp:label id="lblEMPagingDataDateFrom" runat="Server"></asp:label>
                                                        <asp:label id="lblEMPagingDataDateTo" runat="Server"></asp:label>
                                                        &nbsp;<asp:label id="lblEMPagingDataNoOfDays" runat="Server"></asp:label>
                                                    </td>
                                                    <td style="width: 10px;" valign="top">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr id="trEMPaginationGraph" style="display: none;">
                                        <td align="right">
                                            <table border="0" cellpadding="0" cellspacing="0" width="300px" style="background-color: #F0EDED;">
                                                <tr style="height: 36px;">
                                                    <td align="left">
                                                        <input type="button" id="btnEMPrevGraph" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                            onmouseout="btnPagination_DeviceDetailsOut(this);" runat="server" value=" << "
                                                            onclick="ShowEMPreviousNextGraph(3);" />
                                                    </td>
                                                    <td style="width: 15px;">
                                                        <label id="lblEMDeviceDetails_DateType" class="clsLALabel" style="width: 150px;">
                                                        </label>
                                                    </td>
                                                    <td style="width: 15px;">
                                                    </td>
                                                    <td>
                                                        <img src="images/imgLAAlertsMonthly.png" id="btnEMMonthly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowEMDateRangeGraph(4);" />
                                                    </td>
                                                    <td>
                                                        <img src="images/imgLAAlertsWeekly.png" id="btnEMWeekly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowEMDateRangeGraph(3);" />
                                                    </td>
                                                    <td>
                                                        <img src="images/imgLAAlertsDaily.png" id="btnEMDaily_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowEMDateRangeGraph(2);" />
                                                    </td>
                                                    <td>
                                                        <img src="images/imgLAAlertsHourly.png" id="btnEMHourly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowEMDateRangeGraph(1);" />
                                                    </td>
                                                    <td style="width: 15px;">
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEMNextGraph" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                                            onmouseout="btnPagination_DeviceDetailsOut(this);" runat="server" value=" >> "
                                                            onclick="ShowEMPreviousNextGraph(2);" />
                                                    </td>
                                                    <td style="width: 10px;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr valign="top" style="height: 5px;">
                                        <td>
                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                <tr id="trEMLBIDiff" style="display: none;">
                                                    <td align="left">
                                                        <input type="checkbox" id="chkEMIsShowLBIADC" onchange="showEMHideLBIADC();" style="vertical-align: middle;" />&nbsp;
                                                        <asp:label id="lblEMShowLBIADC" runat="server" cssclass="clsLALabel" style="font-size: 12px;
                                                            color: #222222;">Show&nbsp;LBI&nbsp;ADC</asp:label>
                                                        <input type="checkbox" id="ChkEMIsShowLBIDiff" onchange="showEMHideLBIDiff();" style="vertical-align: middle;" />&nbsp;
                                                        <asp:label id="LblEMLBIDiff" runat="server" cssclass="clsLALabel" style="font-size: 12px;
                                                            color: #222222;">Show&nbsp;LBI&nbsp;Diff</asp:label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr id="trEMGraph" runat="server">
                                        <td align='left' id="tdEMTrendGraph" style="width: 5%; height: 20px; text-align: left;">
                                            <div style="position: relative; display: none; width: 100%; top: 200px; left: 400px;"
                                                id="divLoadingEMGraph">
                                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                            </div>
                                            <div id="DivEM_MSStackedColumn2D" style="height: 600px;">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td style="width: 22px;">
                                            <img id="imgOtherSupport2" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                                width: 30px;" onclick="show_NewStatus(2);" />
                                        </td>
                                        <td colspan="3" class="Prod_subTitle">
                                            Configuration
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trConfiguration" style="display: none;">
                            <td>
                                <table id="tblEMTagDetails" cellspacing="1" cellpadding="5" style="width: 95.5%;
                                    padding-top: 15px;">
                                </table>
                                <table id="tblEMProfile" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td style="width: 22px;">
                                            <img id="imgOtherSupport3" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                                width: 30px;" onclick="show_NewStatus(3);" />
                                        </td>
                                        <td colspan="3" class="Prod_subTitle">
                                            Certification
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trCertification" style="display: none;">
                            <td>
                                <table id="tblEMCertification" cellspacing="1" cellpadding="5" style="width: 95.5%;
                                    padding-top: 15px;">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="tblimages" cellspacing="1" cellpadding="5" style="width: 95.5%;">
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td style="width: 22px;">
                                            <img id="imgOtherSupport4" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                                width: 30px;" onclick="show_NewStatus(4);" />
                                        </td>
                                        <td colspan="3" class="Prod_subTitle">
                                            Images
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="div_EM_append_Images">
                                </div>
                                <div id="divImages" style="display: none;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; display: none;
                                        padding-top: 15px;" id="tblPictureDetails">
                                        <tr>
                                            <td class="siteOverview_TopLeft_Box_DeviceDetailsHeaderText" style="height: 40px;">
                                                <div style="float: left;">
                                                    Photos</div>
                                                <div style="float: right;">
                                                    <input type="button" value="Edit" class="clsExportExcel" id="btnEditDeviceImage"
                                                        style="width: 100px; display: none; background-color: #4F903A;" />
                                                    <input type="button" value="Delete" class="clsExportExcel" id="btnDeleteDeviceImage"
                                                        style="width: 100px; display: none; background-color: #D22F2F;" />
                                                    <input type="button" value="Upload Image" class="clsExportExcel" id="btnAddDeviceImage"
                                                        style="width: 100px; display: none; margin-right: 4px;" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="DeviceList_leftBox_DeviceDetailsDatasText" id="tdNoData" align="center"
                                                height="40px" style="width: 100%; text-align: center; display: none;">
                                                No Records found.....
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="DeviceList_leftBox_DeviceDetailsDatasText" style="width: 100%;
                                                display: none;" valign="top" rowspan="5" id="tdImage">
                                                <div id="rg-gallery" class="rg-gallery" style="width: 100%;">
                                                    <div style="float: left; width: 49.5%; height: 410px;">
                                                        <div class="rg-info">
                                                        </div>
                                                    </div>
                                                    <div style="float: right; width: 49.5%; height: 410px;">
                                                        <div class="rg-image-wrapper" style="height: 380px;">
                                                            <div class="rg-image" style="width: 320px; text-align: center; vertical-align: middle;
                                                                height: 330px;" id="divImage">
                                                            </div>
                                                            <div class="rg-thumbs">
                                                                <div class="es-carousel-wrapper" style="width: 262px;">
                                                                    <div class="es-nav">
                                                                        <span class="es-nav-prev">Previous</span> <span class="es-nav-next">Next</span>
                                                                    </div>
                                                                    <div class="es-carousel">
                                                                        <ul id="uiImage">
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td style="width: 22px;">
                                            <img id="imgOtherSupport5" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                                width: 30px;" onclick="show_NewStatus(5);" />
                                        </td>
                                        <td colspan="3" class="Prod_subTitle" style="color: lightgray">
                                            History
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trHistory" style="display: none;">
                            <td>
                                <table id="tblHistory" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Device_dialog_UploadImage" title="Upload Image" style="display: none;">
                    <iframe id="ifrmDeviceUploadIamge" style="border: none; width: 100%; height: 100%;">
                    </iframe>
                </div>
            </td>
        </tr>
    </table>
</asp:content>
