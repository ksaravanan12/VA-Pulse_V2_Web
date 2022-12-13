<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Home.aspx.vb" Inherits="GMSUI.Home" Title="Home" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />
    <link rel="stylesheet" href="Styles/multiple-select.css" />
    <link rel="stylesheet" type="text/css" href="Styles/elastislide.css" />
    <script language="javascript" type="text/javascript" src="Javascript/Dialog.js?d=11"></script>
    <script language="javascript" type="text/javascript" src="Javascript/js_BatterySummary.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_InfraBatteryInfo.js?d=10"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_tagBatteryInfo.js?d=10"></script>
    <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script>
    <script type="text/javascript" src="Javascript/js_DevicePhotos.js?d=12"></script>
    <script type="text/javascript" src="Javascript/jquery.tmpl.min.js"></script>
    <script type="text/javascript" src="Javascript/jquery.device_elastislide.js"></script>
    <script type="text/javascript" src="Javascript/gallery_device.js"></script>
    <script id="img-wrapper-tmpl" type="text/x-jquery-tmpl"></script>
    <script type="text/javascript" src="Javascript/js_TagDetail.js?d=31"></script>
    <script language="javascript" type="text/javascript">

        var IE = document.all ? true : false

        // If NS -- that is, !IE -- then set up for mouse capture
        if (!IE) document.captureEvents(Event.MOUSEMOVE)

        // Set-up to use getMouseXY function onMouseMove
        document.onmousemove = getMouseXY;

        // Temporary variables to hold mouse x-y pos.s
        var tempX = 0
        var tempY = 0
        var isButtonClicked = 0;
        var g_UserRole = 0;
        var g_UserId = 0;
        var g_reachedHome = 0;
        var g_CurBin = 0;
        var g_curDeviceName = "";
        var g_UserViews = "";
        var g_TagAllDeviceRoot;
        var g_InfraAllDeviceRoot;
        var g_StarAllDeviceRoot;
        var g_IsTempMonitoring = 0;
        var g_AccessForStar = 0;
        var GSiteId = "";
        var gSettings_obj;
        var SiteAllowedForSettings = '';
        var g_UIId = 0;

        this.onload = function () {

            document.getElementById("<%=divHome.ClientID%>").style.display = "";
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_UserId = document.getElementById("<%=hid_userid.ClientID%>").value;
            g_UserViews = document.getElementById("<%=hid_userViews.ClientID%>").value;
            g_AccessForStar = document.getElementById("<%=hid_AccessForStar.ClientID%>").value;

            document.getElementById("DeleteDevice").style.display = "none";
            document.getElementById("AddDevice").style.display = "none";
            document.getElementById('hid_Show').value = 0;

            //Search Device
            document.getElementById("DeleteSearchDevice").style.display = "none";
            document.getElementById("AddSearchDevice").style.display = "none";
            document.getElementById('hdn_Search_Show').value = 0;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;

            checkBrowser();

            hideRecalibration();
            getSitelistForsettings(1);
        };

        $(document).on('click', '#chkAll', function () {

            if ($('input:checkbox[name=chkAll]').is(':checked')) {
                $('input:checkbox[name=chk_hid]').attr('checked', true);
            } else {
                $('input:checkbox[name=chk_hid]').attr('checked', false);
            }
        });

        window.onhashchange = function () {

            if ((location.hash == "#deviceDetail" || location.hash == "#EMdeviceDetail" || location.hash == "#divPatientTag" || location.hash == "#StarSettings") && document.getElementById("ctl00_ContentPlaceHolder1_divHome").style.display == "none")
                document.getElementById('tdLeftMenu').style.display = "none";
            else
                document.getElementById('tdLeftMenu').style.display = "";

            if (g_reachedHome == 1 && isButtonClicked == 0)
                return;

            // Load Glossary
            if (location.hash == "#divSiteOverview1") {
                showGlossaryInfo("SiteOverview")
            }
            else if (location.hash == "#divPatientTag" || location.hash == "#StarSettings") {
                showGlossaryInfo("DeviceList")
            }
            else if (location.hash == "#deviceDetail" || location.hash == "#EMdeviceDetail") {
                showGlossaryInfo("DeviceDetails")
            }
            else if (location.hash == "#BatterySummary") {
                showGlossaryInfo("BatterySummary")
            }
            else if (location.hash == "" | location.hash == "#") {
                showGlossaryInfo("Home")
            }

            if (isButtonClicked == 1) {

                if (location.hash == "" || location.hash == "#") {
                    g_reachedHome = 1;
                }
                else
                    g_reachedHome = 0;

                isButtonClicked = 0;

                return;
            }

            if (location.hash == "" || location.hash == "#") {

                g_reachedHome = 1;
                DisplayHomeForBrowserBack();
            }
            else if (location.hash == "#divPatientTag") {
                DisplayPatientTagfromDeviceDetails(0);
            }
            else if (location.hash == "#divSiteOverview1") {
                DisplaySiteOverview(0);
            }
            else if (location.hash == "#StarSettings") {
                DisplayStarSettingsDetails(0)
            }
        }

        function getMouseXY(e) {

            if (IE) {
                // grab the x-y pos.s if browser is IE
                tempX = event.clientX + document.body.scrollLeft
                tempY = event.clientY + document.body.scrollTop
            }
            else {
                // grab the x-y pos.s if browser is NS
                tempX = e.pageX
                tempY = e.pageY
            }

            // catch possible negative values in NS4
            if (tempX < 0) { tempX = 0 }
            if (tempY < 0) { tempY = 0 }

            return true
        }

        function showTip(text, lf, tp) {

            var elementRef = document.getElementById('tooltip1');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.visibility = 'visible';
            tempX = lf;
            tempY = tp;

            $("#tooltip1").css({ left: tempX, top: tempY });
        }

        function hideTip() {

            var elementRef = document.getElementById('tooltip1');
            elementRef.style.visibility = 'hidden';
        }

        function showTip2(text, lf, tp) {

            var elementRef = document.getElementById('tooltip2');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip2").css({ left: tempX, top: tempY });
        }

        function hideTip2() {

            var elementRef = document.getElementById('tooltip2');
            elementRef.style.display = 'none';
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

        function showTipDeviceDetails(text, lf, tp) {

            var elementRef = document.getElementById('tooltipdeviceDetail');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.visibility = 'visible';
            tempX = lf;
            tempY = tp;

            $("#tooltipdeviceDetail").css({ left: tempX, top: tempY });
        }

        function hideTipDeviceDetails() {

            var elementRef = document.getElementById('tooltipdeviceDetail');
            elementRef.style.visibility = 'hidden';
        }

        function showTipSiteOverview(text, lf, tp) {

            var elementRef = document.getElementById('divSiteOverviewToolTip');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;

            $("#divSiteOverviewToolTip").css({ left: tempX, top: tempY });
        }

        function hideTipSiteOverview() {
            var elementRef = document.getElementById('divSiteOverviewToolTip');
            elementRef.style.display = 'none';
        }

        function Redirect(sid, pcversion, IsSiteConnectivityLost, TimeZone) {

            var tdid = "td-" + sid;
            var sTd = document.getElementById(tdid);
            var clsname = sTd.className

            if (clsname == "HeaderTabOutRed") {
                location.href = "Alertlist.aspx?sid=" + sid
            }
            else {
                loadSiteOverviewInfoOnClick(sid, "", pcversion, IsSiteConnectivityLost, TimeZone);
            }
        }

        function DisplayPreviousPage(isClick) {

            if (PrevPage == "Home") {
                DisplayHome(isClick);
            }
            else {
                DisplaySiteOverview(isClick);
            }

            document.getElementById("DeleteDevice").style.display = "none";
            document.getElementById("AddDevice").style.display = "none";
            document.getElementById('hid_Show').value = 0;

            document.getElementById("DeleteSearchDevice").style.display = "none";
            document.getElementById("AddSearchDevice").style.display = "none";
            document.getElementById('hdn_Search_Show').value = 0;
        }

        function DisplaySiteOverview(isClick) {

            isButtonClicked = isClick;

            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'right' }, 50);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 50);
            $('#ctl00_ContentPlaceHolder1_BatterySummary').hide('slide', { direction: 'right' }, 50);

            if (isClick)
                location.href = "#divSiteOverview1";

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').show('slide', { direction: 'left' }, 300);

            clearDeviceListSearchRow();

            if (FusionCharts("myChartId")) FusionCharts("myChartId").dispose();
            if (FusionCharts("myPieChartId")) FusionCharts("myPieChartId").dispose();

            document.getElementById("divchartBatterySummary").className = "page1";
            document.getElementById("divPieChartBatterySummary").className = "page2";

            g_pie_chart_loaded = 0;
        }

        function DisplayHomeForBrowserBack() {

            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'right' }, 200);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 200);
            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'right' }, 200);
            $('#ctl00_ContentPlaceHolder1_divHome').show('slide', { direction: 'left' }, 900);

            clearDeviceListSearchRow();

            clearLAGraph();
        }

        function DisplayHome(isClick)//PatientTag
        {
            isButtonClicked = isClick;
            g_reachedHome = 1;

            if (isClick)
                location.href = "#";

            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'right' }, 200);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 200);
            $('#ctl00_ContentPlaceHolder1_divHome').show('slide', { direction: 'left' }, 900);

            clearDeviceListSearchRow();
        }

        function DisplayHome2(isClick)//SiteOverview
        {
            isButtonClicked = isClick;
            g_reachedHome = 1;

            document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'right' }, 200);
            $('#ctl00_ContentPlaceHolder1_divHome').show('slide', { direction: 'left' }, 900);

            if (setundefined(GSiteId) != "") {
                document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = GSiteId;
            }
            else {
                document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
            }
        }

        function DisplayPatientTagfromDeviceDetails(isClick) {

            isButtonClicked = isClick;

            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').hide('slide', { direction: 'right' }, 100);
            $('#divSersorImagesView').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'left' }, 700);
        }

        //Patient Tag List

        var stopTimer;
        var ctrlVal;
        var SiteId, Alertid, bin, typeId;
        var PrevPage = "Home";
        var g_CurrDeviceType = 0;

        function loadTagInfoOnClick(siteid, type, nbin, TimeZone) {

            hideRecalibration();

            TAG_SortColumn = "LastSeen";
            TAG_SortOrder = "desc";

            typeId = type;
            g_CurrDeviceType = 1;
            isButtonClicked = 1;
            PrevPage = "Home";
            g_curDeviceName = "Tags";
            DeviceName = g_curDeviceName;

            TimeZone = decodeURIComponent(TimeZone);
            document.getElementById('lblsiteTimeZone').innerHTML = setundefined(TimeZone);
            document.getElementById('lblsiteTagTimeZone').innerHTML = setundefined(TimeZone);

            loadTagInfo(siteid, type, nbin);

            $('#ctl00_ContentPlaceHolder1_divHome').hide('slide', { direction: 'left' }, 100);
            location.href = "#divPatientTag";
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);
        }

        function loadTagInfoOnClickfromSIteOverview(siteid, type, nbin, typename, UIId) {

            hideRecalibration();
            g_CurrDeviceType = 1;
            isButtonClicked = 1;
            PrevPage = "SiteOverview";
            typeId = type;
            g_curDeviceName = typename.replace(/_/g, ' ');
            DeviceName = g_curDeviceName;
           
            TAG_SortColumn = "LastSeen";
            TAG_SortOrder = "desc";

            loadTagInfo(siteid, type, nbin, UIId);

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);
        }

        function loadTagInfoOnClickfromSIteOverviewforAllRpt(siteid, type, typename, UIId) {

            hideRecalibration();

            TAG_SortColumn = "LastSeen";
            TAG_SortOrder = "desc";
            g_curDeviceName = typename.replace(/_/g, ' ');
            DeviceName = g_curDeviceName;

            g_CurrDeviceType = 1;
            isButtonClicked = 1;
            PrevPage = "SiteOverview";

            loadTagInfo(siteid, type, "", UIId);

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);
        }

        function ShowRecalibration() {

            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            g_IsTempMonitoring = document.getElementById("<%=hid_IsTempMonitoring.ClientID%>").value;

            document.getElementById('tdCalibration').style.display = "none";
            document.getElementById('tdSearchCalibration').style.display = "none";

            document.getElementById("tdSearchImport").style.display = "none";
            document.getElementById("tdImport").style.display = "none";

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support || g_IsTempMonitoring == "True") {

                if (g_CurrDeviceType == 1 && g_UIId == enumPulseUITable.EnvironmentalTags) {

                    document.getElementById('tdCalibration').style.display = "";
                    document.getElementById('tdSearchCalibration').style.display = "";

                    document.getElementById("tdImport").style.display = "";
                    document.getElementById("tdSearchImport").style.display = "";
                }
            }
        }

        function hideRecalibration() {

            document.getElementById('tdCalibration').style.display = "none";
            document.getElementById('tdSearchCalibration').style.display = "none";

            document.getElementById("tdSearchImport").style.display = "none";
            document.getElementById("tdImport").style.display = "none";
        }

        function loadTagInfo(siteid, type, nbin, UIId) {

            g_CurBin = nbin;

            document.getElementById('btnEditDevice').style.display = "none";
            document.getElementById("DeleteDevice").style.display = "none";
            document.getElementById("AddDevice").style.display = "none";
            document.getElementById('btnEditSearchDevice').style.display = "none";
            document.getElementById("DeleteSearchDevice").style.display = "none";
            document.getElementById("AddSearchDevice").style.display = "none";

            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;

            if (nbin === "" || nbin == "3" || nbin == "7") {
                if (g_UserRole != enumUserRoleArr.Customer) {

                    document.getElementById('btnEditDevice').style.display = "inline";
                    document.getElementById('btnEditSearchDevice').style.display = "inline";
                }
            }

            document.getElementById('tblInfraInfo').style.display = "none";
            document.getElementById('tblStarInfo').style.display = "none";
            document.getElementById('tblTagInfo').style.display = "";

            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML = "";
            document.getElementById("subHeader").innerHTML = DeviceName;
            document.getElementById("btnSettingsDevice").style.display = "none";

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblTagInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblTagInfo');
            }

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1";

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteid;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            SiteId = siteid;
            bin = nbin;
            Alertid = "";
            typeId = type;
            g_UIId = UIId;

            doLoadTagInformation();
        }

        function doLoadTagInformation() {

            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            doLoadTag(SiteId, Alertid, bin, "1", typeId)
        }

        //Load Tags For Search
        function loadTagInfoForSearch(siteid, type, nbin, deviceids, orgtypeId) {

            document.getElementById('tblInfraInfoSearch').style.display = "none";
            document.getElementById('tblStarInfoSearch').style.display = "none";
            document.getElementById('tblTagInfoSearch').style.display = "";

            var sTbl, sTblLen;

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblTagInfoSearch');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblTagInfoSearch');
            }

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading").style.display = "";

            doLoadTagSearch(siteid, "", nbin, "0", type, deviceids, orgtypeId);
        }

        function loadInfraInfoOnClick(siteid, type, nbin, TimeZone) {

            typeId = type;
            g_CurrDeviceType = 2;
            isButtonClicked = 1;
            PrevPage = "Home";
            hideRecalibration();
            g_curDeviceName = "Infrastructure";
            DeviceName = g_curDeviceName;

            TimeZone = decodeURIComponent(TimeZone);
            document.getElementById('lblsiteTimeZone').innerHTML = setundefined(TimeZone);
            document.getElementById('lblsiteTagTimeZone').innerHTML = setundefined(TimeZone);

            loadInfraInfo(siteid, type, nbin);

            $('#ctl00_ContentPlaceHolder1_divHome').hide('slide', { direction: 'left' }, 100);
            location.href = "#divPatientTag";
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);
        };

        function loadInfraInfoOnClickfromSIteOverview(siteid, type, nbin, typename) {

            typeId = type;
            g_CurrDeviceType = 2;
            isButtonClicked = 1;

            g_curDeviceName = typename.replace(/_/g, ' ');
            DeviceName = g_curDeviceName;

            PrevPage = "SiteOverview";
            hideRecalibration();

            loadInfraInfo(siteid, type, nbin);

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            location.href = "#divPatientTag";
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);
        };

        function loadInfraInfo(siteid, type, nbin) {

            g_CurBin = nbin;
            isButtonClicked = 1;

            document.getElementById('tblInfraInfo').style.display = "";
            document.getElementById("tdPagination").style.display = "";
            document.getElementById('tblTagInfo').style.display = "none";
            document.getElementById('tblStarInfo').style.display = "none";

            document.getElementById('btnEditDevice').style.display = "none";
            document.getElementById("DeleteDevice").style.display = "none";
            document.getElementById("AddDevice").style.display = "none";

            document.getElementById('btnEditSearchDevice').style.display = "none";
            document.getElementById("DeleteSearchDevice").style.display = "none";
            document.getElementById("AddSearchDevice").style.display = "none";

            if (nbin === "" || nbin == "3" || nbin == "7") {

                if (g_UserRole != enumUserRoleArr.Customer) {
                    document.getElementById('btnEditDevice').style.display = "inline";
                    document.getElementById('btnEditSearchDevice').style.display = "inline";
                }
            }

            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML = "";
            document.getElementById("subHeader").innerHTML = DeviceName;
            document.getElementById("btnSettingsDevice").style.display = "none";

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblInfraInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblInfraInfo');
            }

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1";

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteid;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            SiteId = siteid;
            bin = nbin;
            Alertid = "";
            typeId = type;

            doLoadInfraInformation();
        }

        function loadInfraInfoSearch(siteid, type, nbin, deviceIds) {

            document.getElementById('tdCalibration').style.display = "none";
            document.getElementById('tdSearchCalibration').style.display = "none";

            g_CurBin = nbin;
            document.getElementById('tblInfraInfoSearch').style.display = "";
            document.getElementById('tblTagInfoSearch').style.display = "none";
            document.getElementById('tblStarInfoSearch').style.display = "none";

            var sTbl, sTblLen;
            sTbl = document.getElementById('tblInfraInfoSearch');
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading").style.display = "";

            Load_Infrastructure_inforamtionSearch(siteid, "", nbin, "0", type, deviceIds);
        }

        function loadInfraInfoOnClickfromSIteOverviewforAllRpt(siteid, type, typename) {

            g_CurrDeviceType = 2;
            isButtonClicked = 1;
            typeId = type;
                   
            PrevPage = "SiteOverview";        
            g_curDeviceName = typename.replace(/_/g, ' ');
            DeviceName = g_curDeviceName;
           
            hideRecalibration();
            loadInfraInfo(siteid, type, "");

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            location.href = "#divPatientTag";
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);
        };

        function doLoadInfraInformation() {

            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            doLoadInfrastructure(SiteId, Alertid, bin, "1", typeId);
        }

        function loadStarInfoOnClickfromSIteOverview(siteid, type, nbin, typename) {

            document.getElementById("btnSettingsDevice").style.display = "none";

            g_CurrDeviceType = 3;
            hideRecalibration();
            PrevPage = "SiteOverview";
            g_curDeviceName = typename.replace("_", " ");
            DeviceName = g_curDeviceName;

            loadStarInfo(siteid, nbin, type);

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            location.href = "#divPatientTag";

            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);

            try {
                PageVisitDetails(g_UserId, "Home - Star List", enumPageAction.View, $("#subHeader").text() + " list viewed in site " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
            }
            catch (e) {

            }
        };

        function loadStarInfoOnClickfromSIteOverviewforAllRpt(siteid, type, typename) {

            document.getElementById("btnSettingsDevice").style.display = "none";

            g_CurrDeviceType = 3;
            isButtonClicked = 1;
            hideRecalibration();
            PrevPage = "SiteOverview";
            g_curDeviceName = typename.replace(/_/g, ' ');
            DeviceName = g_curDeviceName;

            loadStarInfo(siteid, "", type);

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            location.href = "#divPatientTag";
            $('#ctl00_ContentPlaceHolder1_divPatientTag').show('slide', { direction: 'right' }, 600);

            try {
                PageVisitDetails(g_UserId, "Home - Star List", enumPageAction.View, $("#subHeader").text() + " list viewed in site " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
            }
            catch (e) {

            }
        }

        function loadStarInfo(siteid, nbin, type) {

            g_CurBin = nbin;

            document.getElementById('tblStarInfo').style.display = "";
            document.getElementById('tblInfraInfo').style.display = "none";
            document.getElementById('tblTagInfo').style.display = "none";

            document.getElementById('btnEditDevice').style.display = "none";
            document.getElementById("DeleteDevice").style.display = "none";
            document.getElementById("AddDevice").style.display = "none";

            document.getElementById('btnEditSearchDevice').style.display = "none";
            document.getElementById("DeleteSearchDevice").style.display = "none";
            document.getElementById("AddSearchDevice").style.display = "none";

            if (nbin === "" || nbin == "3") {
                if (g_UserRole != enumUserRoleArr.Customer) {
                    document.getElementById('btnEditDevice').style.display = "inline";
                    document.getElementById('btnEditSearchDevice').style.display = "inline";
                }
            }

            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML = "";

            hideStarSettings(3, siteid);  
                    
            document.getElementById("subHeader").innerHTML = g_curDeviceName;

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblStarInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblStarInfo');
            }

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading").style.display = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage").innerHTML = "";
            document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1";

            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteid;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            SiteId = siteid;
            bin = nbin;
            Alertid = "";
            typeId = type;

            doLoadStarInformation();
        }

        function hideStarSettings(DeviceType, siteid) {

            document.getElementById("btnSettingsDevice").style.display = "none";

            if (DeviceType == 3 && SiteAllowedForSettings != '') {

                if (SiteAllowedForSettings.indexOf(siteid) >= 0) {

                    if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_AccessForStar === "True") {
                        document.getElementById("btnSettingsDevice").style.display = "";
                    }
                }
            }
        }

        function loadStarInfoSearch(siteid, nbin, type, deviceIds) {

            g_CurBin = nbin;

            document.getElementById('tblStarInfoSearch').style.display = "";
            document.getElementById('tblInfraInfoSearch').style.display = "none";
            document.getElementById('tblTagInfoSearch').style.display = "none";

            var sTbl, sTblLen;
            sTbl = document.getElementById('tblStarInfoSearch');
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading").style.display = "";
            Load_Star_inforamtionSearch(siteid, "", nbin, "0", type, deviceIds);
        }

        function doLoadStarInformation() {

            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            doLoadStar(SiteId, Alertid, bin, "1", typeId)
        }

        function ExportSearchByBrowserType() {

            if (g_CurrDeviceType == 1)
                typeId = TagTypeId;

            if (GetBrowserType() == "isIE") {
                DownloadInforamtion_ForIE(SiteId, Alertid, document.getElementById("ddSearchBin").value, "0", typeId, document.getElementById("txtSearchDeviceIds").value, "", g_CurrDeviceType, g_UIId);
            }
            else if (GetBrowserType() == "isFF") {
                DownloadInforamtion(SiteId, Alertid, document.getElementById("ddSearchBin").value, "0", typeId, document.getElementById("txtSearchDeviceIds").value, "", g_CurrDeviceType, g_UIId);
            }

            try {
                PageVisitDetails(g_UserId, "Home - Device Details", enumPageAction.View, " Search result device list exported for site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
            }
            catch (e) {

            }
        }

        function doShow() {

            var hidval = document.getElementById('hid_Show').value

            if (hidval == 0) {
                document.getElementById("DeleteDevice").style.display = "inline";
                document.getElementById("AddDevice").style.display = "inline";
                document.getElementById('hid_Show').value = 1;
            }
            else if (hidval == 1) {
                document.getElementById("DeleteDevice").style.display = "none";
                document.getElementById("AddDevice").style.display = "none";
                document.getElementById('hid_Show').value = 0;
            }

            if (g_CurrDeviceType == 1) {
                g_TagRoot = g_TagAllDeviceRoot;
                loadTagList(0);
            }
            else if (g_CurrDeviceType == 2) {
                g_InfraRoot = g_InfraAllDeviceRoot;
                loadInfraList(0);
            }
            else if (g_CurrDeviceType == 3) {
                g_StarRoot = g_StarAllDeviceRoot;
                loadStarList(0);
            }
        }

        function doSearchShow() {

            var hidSearchval = document.getElementById('hdn_Search_Show').value

            if (hidSearchval == 0) {
                document.getElementById("DeleteSearchDevice").style.display = "inline";
                document.getElementById("AddSearchDevice").style.display = "inline";
                document.getElementById('hdn_Search_Show').value = 1;
            }
            else if (hidSearchval == 1) {
                document.getElementById("DeleteSearchDevice").style.display = "none";
                document.getElementById("AddSearchDevice").style.display = "none";
                document.getElementById('hdn_Search_Show').value = 0;
            }

            if (g_CurrDeviceType == 1)
                loadTagList(1);
            else if (g_CurrDeviceType == 2)
                loadInfraList(1);
            else if (g_CurrDeviceType == 3)
                loadStarList(1);
        }

        function doNext() {

            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            var cnt = Number(currentpage) + 1;
            document.getElementById("<%=txtPageNo.ClientID%>").value = cnt;

            document.getElementById("divLoading").style.display = "";

            if (document.getElementById('tblInfraInfo').style.display == "")
                doLoadInfrastructure(SiteId, Alertid, bin, cnt, typeId)
            else if (document.getElementById('tblStarInfo').style.display == "")
                doLoadStar(SiteId, Alertid, bin, cnt, typeId)
            else if (document.getElementById('tblTagInfo').style.display == "")
                doLoadTag(SiteId, Alertid, bin, cnt, typeId)
        }

        function doPrev() {

            var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
            var cnt = currentpage - 1;
            document.getElementById("<%=txtPageNo.ClientID%>").value = cnt;

            document.getElementById("divLoading").style.display = "";

            if (document.getElementById('tblInfraInfo').style.display == "")
                doLoadInfrastructure(SiteId, Alertid, bin, cnt, typeId)
            else if (document.getElementById('tblStarInfo').style.display == "")
                doLoadStar(SiteId, Alertid, bin, cnt, typeId)
            else if (document.getElementById('tblTagInfo').style.display == "")
                doLoadTag(SiteId, Alertid, bin, cnt, typeId)
        }

        function gotoPage() {

            var cnt = document.getElementById("<%=txtPageNo.ClientID%>").value;

            if (cnt < 1)
                cnt = 1;

            document.getElementById("divLoading").style.display = "";

            if (document.getElementById('tblInfraInfo').style.display == "")
                doLoadInfrastructure(SiteId, Alertid, bin, cnt, typeId)
            else if (document.getElementById('tblStarInfo').style.display == "")
                doLoadStar(SiteId, Alertid, bin, cnt, typeId)
            else if (document.getElementById('tblTagInfo').style.display == "")
                doLoadTag(SiteId, Alertid, bin, cnt, typeId)
        }

        function doDownload() {
            if (document.getElementById('tblInfraInfo').style.display == "") {
                if (GetBrowserType() == "isIE") {
                    DownloadInforamtion_ForIE(SiteId, Alertid, bin, "0", typeId, "", "", 2);
                }
                else if (GetBrowserType() == "isFF") {
                    document.getElementById("divExcelLoading").style.display = "";
                    DownloadInforamtion(SiteId, Alertid, bin, "0", typeId, "", "", 2);
                }
            }
            else if (document.getElementById('tblStarInfo').style.display == "") {
                if (GetBrowserType() == "isIE") {
                    DownloadInforamtion_ForIE(SiteId, Alertid, bin, "0", typeId, "", "", 3);
                }
                else if (GetBrowserType() == "isFF") {
                    document.getElementById("divExcelLoading").style.display = "";
                    DownloadInforamtion(SiteId, Alertid, bin, "0", typeId, "", "", 3);
                }
            }
            else if (document.getElementById('tblTagInfo').style.display == "") {
                if (GetBrowserType() == "isIE") {
                    DownloadInforamtion_ForIE(SiteId, Alertid, bin, "0", typeId, "", "", 1, g_UIId);
                }
                else if (GetBrowserType() == "isFF") {
                    document.getElementById("divExcelLoading").style.display = "";
                    DownloadInforamtion(SiteId, Alertid, bin, "0", typeId, "", "", 1, g_UIId);
                }
            }

            try {
                PageVisitDetails(g_UserId, "Home - Device Details", enumPageAction.View, $("#subHeader").text() + " list exported for site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
            }
            catch (e) {

            }
        }

        var Overviewtype;
        var g_IsSiteConnectivityLost = 0

        function loadSiteOverviewInfoOnClick(siteid, type, pcserverVersion, IsSiteConnectivityLost, TimeZone) {

            g_IsSiteConnectivityLost = IsSiteConnectivityLost;
            isButtonClicked = 1;
            Overviewtype = type;

            if (setundefined(Overviewtype) == "")
                Overviewtype = "system";

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('siteOverview');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('siteOverview');
            }
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            var sTbl2, sTblLen2;
            if (GetBrowserType() == "isIE") {
                sTbl2 = document.getElementById('siteotherDevice');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl2 = document.getElementById('siteotherDevice');
            }
            sTblLen2 = sTbl2.rows.length;
            clearTableRows(sTbl2, sTblLen2);

            var sTbl3, sTblLen3;
            if (GetBrowserType() == "isIE") {
                sTbl3 = document.getElementById('siteEnvOverview');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl3 = document.getElementById('siteEnvOverview');
            }
            sTblLen3 = sTbl3.rows.length;
            clearTableRows(sTbl3, sTblLen3);

            document.getElementById("divLoading_SiteOverview").style.display = "";
            document.getElementById("ctl00_headerBanner_drpsitelist").value = siteid;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("lblSiteName_SiteOverview").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {
                document.getElementById("trPlatformversion").style.display = "";
                pcserverVersion = pcserverVersion.replace(/~/g, " ");
                document.getElementById("lblPCServerVersion").innerHTML = "CenTrak Platform Version " + pcserverVersion;
            }
            else
                document.getElementById("trPlatformversion").style.display = "none";

            SiteId = siteid;
            typeId = type;

            TimeZone = decodeURIComponent(TimeZone);
            document.getElementById('lblsiteTimeZone').innerHTML = setundefined(TimeZone);
            document.getElementById('lblsiteTagTimeZone').innerHTML = setundefined(TimeZone);

            LoaddoLoadSiteOverviewinforamtion(SiteId, typeId);

            $('#ctl00_ContentPlaceHolder1_divHome').hide('slide', { direction: 'left' }, 100);
            location.href = "#divSiteOverview1";
            $('#ctl00_ContentPlaceHolder1_divSiteOverview').show('slide', { direction: 'right' }, 600);

            // Page Visit Tracking
            try {
                PageVisitDetails(g_UserId, "Home - Site OverView", enumPageAction.View, "Site OverViewed - " + $("#ctl00_headerBanner_drpsitelist option:selected").text());
            }
            catch (e) {

            }
        };

        function doLoadSiteOverview() {
            LoaddoLoadSiteOverviewinforamtion(SiteId, typeId);
        }

        var deviceid;

        function hideLBICheckbox() {

            if (document.getElementById("trLBIDiff"))
                document.getElementById("trLBIDiff").style.display = "none";

            IsLBIDiffChecked = false;
            IsLBIADCChecked = false;

            document.getElementById("ChkIsShowLBIDiff").checked = false;
            document.getElementById("chkIsShowLBIADC").checked = false;
        }

        function loadDeviceDetailsInfoOnClick(siteid, devicetype, deviceid) {
            document.getElementById("divImages").style.display = "none";

            hideLBICheckbox(devicetype);

            document.getElementById('tblPictureDetails').style.display = "none";
            $('#tblPictureDetails').css('width', '100%');

            Device_GetPhoto(siteid, devicetype, deviceid);
            isButtonClicked = 1;

            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#divSersorImagesView').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').show('slide', { direction: 'right' }, 600);

            document.getElementById('lblDeviceId_DeviceDetails').innerHTML = deviceid;

            document.getElementById('lblEMDeviceId_DeviceDetails').innerHTML = "";


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
            document.getElementById("lblSiteName_SiteOverview").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            document.getElementById('divLoading_TagDetails').style.display = "";
            document.getElementById('divLoading_Graph').style.display = "";
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

        function hideEMLBICheckbox() {

            if (document.getElementById("trEMLBIDiff"))
                document.getElementById("trEMLBIDiff").style.display = "none";

            IsLBIDiffChecked = false;
            IsLBIADCChecked = false;

            document.getElementById("ChkEMIsShowLBIDiff").checked = false;
            document.getElementById("chkEMIsShowLBIADC").checked = false;
        }

        function loadEMDeviceDetailsInfoOnClick(siteid, devicetype, deviceid, typeid) {

            document.getElementById("divImages").style.display = "none";

            hideEMLBICheckbox(devicetype);

            document.getElementById('tblPictureDetails').style.display = "none";
            $('#tblPictureDetails').css('width', '95.5%');

            Device_GetPhoto(siteid, devicetype, deviceid);
            $("#chkExpand").prop("checked", false);

            isButtonClicked = 1;

            document.getElementById('lblDeviceId_DeviceDetails').innerHTML = "";
            document.getElementById('lblEMDeviceId_DeviceDetails').innerHTML = deviceid;

            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#divSersorImagesView').hide('slide', { direction: 'right' }, 100);
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

        function LoadImages(devicetype, deviceid) {

            document.getElementById('lblDeviceId_Images').innerHTML = deviceid;
            document.getElementById('tblPictureDetails').style.display = "none";
            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'left' }, 100);
            $('#divSersorImagesView').show('slide', { direction: 'left' }, 600);

            Device_GetPhoto(SiteId, devicetype, deviceid);
            $('#divImages').appendTo('#divSensorImages');
            document.getElementById("divImages").style.display = "";
            document.getElementById('tblPictureDetails').style.display = "";
        }

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

        //Show Icons List
        var g_OpenSiteId;
        function showHideImageIcons(imgCtrl, siteId, isLocalAlertsAvailable, isHeartBeatAvailable) {
            $("#divIcons").html("");

            var nWidth = 0;

            var top = $(imgCtrl).offset().top;
            var left = $(imgCtrl).offset().left;

            //Local Alerts
            if (isLocalAlertsAvailable == "1") {
                $("#divIcons").append("&nbsp;&nbsp;<span onclick='redirectToLocalAlerts(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='localAlerts-" + siteId + "'><img  src='images/imgActiveAlerts.png'/></span>");
                nWidth = parseInt(nWidth) + 40;
            }

            //Heart Beat
            if (isHeartBeatAvailable == "1") {
                $("#divIcons").append("&nbsp;&nbsp;<span onclick='redirectToServerStatus(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='heartBeatSpan-" + siteId + "'><img  src='images/imgHeartBeat.png'/></span>");
                nWidth = parseInt(nWidth) + 40;
            }

            //Purchase Order, Map
            $("#divIcons").append("&nbsp;&nbsp;<span onclick='redirectToPO(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='purchaseOrder-" + siteId + "'><img  src='images/purchaseorder.png'/></span>");
            $("#divIcons").append("&nbsp;&nbsp;<span onclick='redirectToMap(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='map-" + siteId + "'><img  src='images/map.png'/></span>");
            nWidth = parseInt(nWidth) + 80;

            $("#divIcons").append("&nbsp;&nbsp;<span onclick='RedirectLocationChage(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='LocationChangeEvent-" + siteId + "'><img  src='images/LocationChange.png' width='26px' height='26px'/></span>");
            nWidth = parseInt(nWidth) + 40;

            $("#divIcons").append("&nbsp;&nbsp;<span onclick='RedirectINITrackingHistory(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='INITrackingHistory-" + siteId + "'><img  src='images/ImginiChanges.png' width='26px' height='26px' alt='INI Tracking History'/></span>");
            nWidth = parseInt(nWidth) + 20;

            $("#divIcons").append("&nbsp;&nbsp;<span onclick='UploadImageFile(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='UploadImageFile-" + siteId + "'><img  src='images/file_upload.png' width='26px' height='26px' alt='Upload File'/></span>");
            nWidth = parseInt(nWidth) + 50;

            $("#divIcons").append("&nbsp;&nbsp;<span onclick='redirectToReports(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='Reports-" + siteId + "'><img  src='images/Reports.png' width='26px' height='26px' alt='Tags Not Seen Recently'/></span>");
            nWidth = parseInt(nWidth) + 20;

            $("#divIcons").append("&nbsp;&nbsp;<span onclick='redirectToCetaniMetadataReports(" + siteId + ");' onmouseover='HeaderHeartBeatOver(this)' onmouseout='HeaderHeartBeatOut(this)' id='CetaniReports-" + siteId + "'><img  src='images/CetaniReports.png' width='26px' height='26px' alt='Cetani MetaData (List)'/></span>");
            nWidth = parseInt(nWidth) + 40;

            $("#divIcons").css("width", nWidth);

            $("#divIcons").css("top", parseInt(top) + 28);
            $("#divIcons").css("left", parseInt(left) - (parseInt(nWidth) - 15));

            if (g_OpenSiteId != siteId) {
                $("#divIcons").slideDown(200);
            } else {
                if ($("#divIcons").is(":visible")) {
                    $("#divIcons").slideUp(200);
                } else {
                    $("#divIcons").slideDown(200);
                }
            }
            g_OpenSiteId = siteId;
        }

        $(document).click(function (event) {
            if (event.target.parentElement !== $("#spnIcons_" + g_OpenSiteId)[0] && event.target !== $("#divIcons")[0]) {
                $("#divIcons").slideUp(200);
            }
        });

        function RedirectLocationChage(siteId) {
            location.href = "LocationChangeEvent.aspx?sid=" + siteId;
        }

        function RedirectINITrackingHistory(siteId) {
            location.href = "INITrackingHistory.aspx?sid=" + siteId;
        }

        function UploadImageFile(siteId) {
            location.href = "UploadImage.aspx?sid=" + siteId;
        }

        function redirectToPO(siteId) {
            location.href = "PurchaseOrder.aspx?SiteId=" + siteId;
        }

        function redirectToMap(siteId) {
            location.href = "Map.aspx?SiteId=" + siteId;
        }

        function redirectToServerStatus(siteId) {
            location.href = "ServerStatus.aspx?SiteId=" + siteId;
        }

        function redirectToLocalAlerts(siteId) {
            location.href = "LocalAlerts.aspx?SiteId=" + siteId;
        }

        function redirectToReports(siteId) {
            location.href = "Reports.aspx?SiteId=" + siteId;
        }

        function redirectToCetaniMetadataReports(siteId) {
            window.open('CetaniMetadata.aspx?sid=' + siteId + "&devicetype=1", '_blank');
        }

        function CnacelDevices() {
            $("#dialogAddDevice").dialog("close");
        }

        function AddDelDevices() {

            $('#lblAddDialogMsg').html('');

            if ($('#txtDeviceId').val() == "") {
                $('#lblAddDialogMsg').html('Enter Device Id');
                $('#lblAddDialogMsg').css({ 'visibility': 'visible' });
                return false;
            }

            var deviceid, devicetype, isDelete;

            deviceid = GetDeviceIdFormat($("#txtDeviceId").val());

            document.getElementById("txtDeviceId").value = deviceid;

            if (deviceid.split(",").length > 500) {

                alert("Please enter less than 500 ids and try again.");
                return;
            }

            addEditDeleteDevice(SiteId, g_CurrDeviceType, deviceid, "0");

            // Page Visit Tracking
            try {
                PageVisitDetails(g_UserId, "Home - Device List", enumPageAction.Add, "DeviceId - " + deviceid + " added in site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
            }
            catch (e) {

            }
        }

        function DeleteDevices() {

            if ($('input[name="chk_hid"]:checked').length == 0) {
                alert('Select any Device to Delete !!!');
            }
            else {
                if (confirm("Are you sure do you want to delete this device?") == true) {

                    //Get Selected Checkbox Values
                    var chkDelDeviceId = $.map($('input[name="chk_hid"]:checked'), function (n, i) {
                        return n.value;
                    }).join(',');
                    addEditDeleteDevice(SiteId, g_CurrDeviceType, chkDelDeviceId, "1");

                    try {
                        PageVisitDetails(g_UserId, "Home - Device List", enumPageAction.Delete, "DeviceId - " + chkDelDeviceId + " deleted in site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
                    }
                    catch (e) {

                    }
                }
            }
        }

        //Device List Search Option ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        function showDeviceListSearchRow() {

            document.getElementById('trSearchDeviceListRow').style.display = "none";

            if (document.getElementById("btnSearchDeviceList").value == "Search") {

                document.getElementById("hidUpdateLocalId").value = "0";
                document.getElementById('trDeviceListSearchRow').style.display = "";
                document.getElementById("btnSearchDeviceList").value = "Cancel";

                if (g_CurrDeviceType == 3) {

                    document.getElementById("ddSearchBin").value = "";
                    document.getElementById("optUnderwatch").style.display = "none";
                    document.getElementById("optLBI").style.display = "none";
                }
                else {

                    document.getElementById("optUnderwatch").style.display = "";
                    document.getElementById("optLBI").style.display = "";
                }

                document.getElementById("ddSearchBin").value = g_CurBin;
                document.getElementById("ddSearchBin").disabled = false;
            }
            else//Cancel
            {
                document.getElementById('trDeviceListSearchRow').style.display = "none";
                document.getElementById("btnSearchDeviceList").value = "Search";
                document.getElementById('trDeviceListRow').style.display = "";
                document.getElementById("subHeader").innerHTML = g_curDeviceName;
                document.getElementById("divLoading").style.display = "none";

                hideStarSettings(g_CurrDeviceType, SiteId);

                if (document.getElementById("txtSearchDeviceIds").value != "" && document.getElementById("hidUpdateLocalId").value != "0") {
                    var currentpage = document.getElementById("<%=txtPageNo.ClientID%>").value;
                    doLoadTag(SiteId, Alertid, bin, currentpage, OldTypeId);
                }

                if (OldTypeId != 0)
                    typeId = OldTypeId;

                ShowRecalibration();
                document.getElementById("txtSearchDeviceIds").value = "";
            }
        }

        function clearDeviceListSearchRow() {

            document.getElementById('trDeviceListSearchRow').style.display = "none";
            document.getElementById("btnSearchDeviceList").value = "Search";
            document.getElementById('trDeviceListRow').style.display = "";
            document.getElementById('txtSearchDeviceIds').value = "";
        }

        var OldTypeId = 0;

        function btnSearchDeviceListGO_Clicked() {

            OldTypeId = typeId;

            if (document.getElementById("txtSearchDeviceIds").value == "") {

                alert("Please enter device id to search");
                return;
            }
            else {
                var ids = (document.getElementById("txtSearchDeviceIds").value).split(',');
                if (ids.length > 500) {
                    alert("Too many id's to search. Please enter less than 500 ids and try again.");
                    return;
                }
            }

            document.getElementById('trDeviceListRow').style.display = "none";

            var DeviceIds = document.getElementById("txtSearchDeviceIds").value;
            document.getElementById("txtSearchDeviceIds").value = GetDeviceIdFormat(DeviceIds);

            var DeviceId = 0;
            DeviceId = document.getElementById("txtSearchDeviceIds").value;

            //Call API and fill the results
            if (g_CurrDeviceType == 1) {

                loadTagInfoForSearch(SiteId, "0", document.getElementById("ddSearchBin").value, DeviceId, typeId);

                try {
                    PageVisitDetails(g_UserId, "Home - Tag List", enumPageAction.Search, "Tag List searched for siteId :" + SiteId + ", TagIds :" + DeviceId);
                }
                catch (e) {

                }
            }
            else if (g_CurrDeviceType == 2) {

                loadInfraInfoSearch(SiteId, "0", document.getElementById("ddSearchBin").value, DeviceId);

                try {
                    PageVisitDetails(g_UserId, "Home - Monitor List", enumPageAction.Search, "Monitor List searched in site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text() + ", MonitorIds :" + DeviceId);
                }
                catch (e) {

                }
            }
            else if (g_CurrDeviceType == 3) {

                loadStarInfoSearch(SiteId, document.getElementById("ddSearchBin").value, "0", DeviceId);

                try {
                    PageVisitDetails(g_UserId, "Home - Star List", enumPageAction.Search, "Star List searched in site - " + $("#ctl00_ContentPlaceHolder1_lblsitename").text() + ", StarIds :" + DeviceId);
                }
                catch (e) {

                }
            }

            document.getElementById('trSearchDeviceListRow').style.display = "";
            document.getElementById("subHeader").innerHTML = "Search (" + $("#ddSearchBin option:selected").text() + ")";

            hideStarSettings(g_CurrDeviceType, SiteId);
        }

        //Battery Summary View       
        var batdevicetype;
        var batsubtypeid;
        var strbatterytypename;

        function loadBatteryInfoOnClickfromSIteOverview(subtypeid, devicetype, batterytypename) {

            batdevicetype = devicetype;
            batsubtypeid = subtypeid;
            strbatterytypename = batterytypename;
            document.getElementById('btnBackToBarGraph').style.display = "none";

            isButtonClicked = 1;
            PrevPage = "SiteOverview";
            $("#selCampus").empty();
            $("#selBuilding").empty();
            $("#selUnits").empty();
            $("#selFloor").empty();
            $('#selUnits').multipleSelect('refresh');
            $('#selFloor').multipleSelect('refresh');
            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            document.getElementById("lblSiteName_BatterySummary").innerHTML = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {
                document.getElementById("trPlatformversionBatterySummary").style.display = "";
                document.getElementById("lblPCServerVersionBatterySummary").innerHTML = document.getElementById("lblPCServerVersion").innerHTML;
            }
            else

                var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
            loadCmbBatterySummary(siteVal);
            document.getElementById('divchartBatterySummary').style.display = "none";
            document.getElementById('divTableBatterySummary').style.display = "none";

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {

                document.getElementById('btnviewload').style.display = "";
                document.getElementById('btnlist').style.display = "";
                document.getElementById('btngraph').style.display = "none";
            }

            $('#ctl00_ContentPlaceHolder1_divSiteOverview').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_BatterySummary').show('slide', { direction: 'right' }, 600);
            document.getElementById("btnExportBatteryList").style.display = "none";
            document.getElementById("btnExportGraphReport").style.display = "";
            if (IE) {
                setTimeout(function () {
                    LoadBatterySummaryGraph();
                }, 500);

            }
            else {
                LoadBatterySummaryGraph();
            }
        }

        function AfterStarLocationUpdate() {
            gotoPage();
        }

        function LoadBatterySummaryGraph() {
            if (document.getElementById("divchartBatterySummary").style.display != "" && document.getElementById("divchartBatterySummary").style.display != "block")
                FlipCharts();

            document.getElementById('divPaginationRow').style.display = "none";
            document.getElementById("divLoading_BatteryOverview").style.display = "";
            document.getElementById('btngraph').style.display = "none";
            document.getElementById('btnlist').style.display = "";
            document.getElementById('divTableBatterySummary').style.display = "none";

            if (batdevicetype == "1")
                doLoadTagBatterySummary(SiteId, "1", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val());
            else if (batdevicetype == "2")
                doLoadInfraBatterySummary(SiteId, "2", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val());

            var siteName = $("#lblSiteName_BatterySummary").text();

            try {
                PageVisitDetails(g_UserId, "Home - Battery Summary ", enumPageAction.View, "Battery Summary Viewed in site - " + siteName);
            }
            catch (e) {

            }

        }

        function DownLoadExcelGraphReport() {
            document.getElementById("btnExportGraphReport").style.display = "";

            if (GetBrowserType() == "isIE") {
                if (batdevicetype == "1") {
                    doLoadTagExcelBatterySummary_IE(SiteId, "1", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val());
                }
                else if (batdevicetype == "2") {
                    doLoadInfraExcelBatterySummary_IE(SiteId, "2", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val());
                }

            }
            else if (GetBrowserType() == "isFF") {
                document.getElementById("divExcelReportLoading").style.display = "";

                if (batdevicetype == "1") {
                    doLoadTagExcelBatterySummary(SiteId, "1", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val());
                }
                else if (batdevicetype == "2") {
                    doLoadInfraExcelBatterySummary(SiteId, "2", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val());
                }
            }

            try {
                PageVisitDetails(g_UserId, "Home - Battery Summary", enumPageAction.View, "Battery Summary - Graph Report - exported in site - " + $("#lblSiteName_BatterySummary").text());
            }
            catch (e) {

            }
        }

        function GetBatteryList() {
            document.getElementById('divPaginationRow').style.display = "none";
            document.getElementById("<%=txtBatteryPageNo.ClientID%>").value = 1;
            document.getElementById("divPieChartBatterySummary").style.display = "none";
            document.getElementById('divchartBatterySummary').style.display = "none";
            document.getElementById('btngraph').style.display = "none";
            document.getElementById("divTableBatterySummary").style.display = "";
            document.getElementById("btnExportBatteryList").style.display = "";
            document.getElementById("btnExportGraphReport").style.display = "none";
            document.getElementById('btnBackToBarGraph').style.display = "none";

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblBatterySummary');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblBatterySummary');
            }
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            document.getElementById("divLoading_BatteryOverview").style.display = "";
            if (batdevicetype == "1") {
                LoadTagBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), "1", "");
            }
            else if (batdevicetype == "2") {
                LoadInfraBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), "1", "");
            }

            document.getElementById('btngraph').style.display = "";
            document.getElementById('btnlist').style.display = "none";
        }

        function doBatteryNext() {
            var currentpage = document.getElementById("<%=txtBatteryPageNo.ClientID%>").value;
            var cnt = Number(currentpage) + 1;
            document.getElementById("<%=txtBatteryPageNo.ClientID%>").value = cnt;

            document.getElementById("divLoading_BatteryOverview").style.display = "";

            if (document.getElementById('tblBatterySummary').style.display == "") {
                if (batdevicetype == "1") {
                    LoadTagBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), cnt, "");
                }
                else if (batdevicetype == "2") {
                    LoadInfraBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), cnt, "");

                }
            }

        }

        function doBatteryPrev() {
            var currentpage = document.getElementById("<%=txtBatteryPageNo.ClientID%>").value;
            var cnt = currentpage - 1;
            document.getElementById("<%=txtBatteryPageNo.ClientID%>").value = cnt;

            document.getElementById("divLoading_BatteryOverview").style.display = "";

            if (document.getElementById('tblBatterySummary').style.display == "") {
                if (batdevicetype == "1") {
                    LoadTagBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), cnt, "");
                }
                else if (batdevicetype == "2") {
                    LoadInfraBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), cnt, "");

                }
            }
        }

        function gotoBatteryPage() {
            var cnt = document.getElementById("<%=txtBatteryPageNo.ClientID%>").value;

            if (cnt < 1)
                cnt = 1;

            document.getElementById("divLoading_BatteryOverview").style.display = "";

            if (document.getElementById('tblBatterySummary').style.display == "") {
                if (batdevicetype == "1") {
                    LoadTagBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), cnt, "");
                }
                else if (batdevicetype == "2") {
                    LoadInfraBatteryList(SiteId, batdevicetype, batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), cnt, "");

                }
            }
        }

        function DownLoadExcelBatteryListReport() {

            if (GetBrowserType() == "isIE") {
                if (batdevicetype == "1") {
                    doLoadTagExcelBatteryList_IE(SiteId, "1", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), "0", "");
                }
                else if (batdevicetype == "2") {
                    doLoadInfraExcelBatteryList_IE(SiteId, "2", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), "0", "");
                }

            }
            else if (GetBrowserType() == "isFF") {

                document.getElementById("divExcelReportLoading").style.display = "";

                if (batdevicetype == "1") {
                    doLoadTagExcelBatteryList(SiteId, "1", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), "0", "");
                }
                else if (batdevicetype == "2") {
                    doLoadInfraExcelBatteryList(SiteId, "2", batsubtypeid, $('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(), "0", "");
                }
            }

            try {
                PageVisitDetails(g_UserId, "Home - Battery Summary", enumPageAction.View, "Battery Summary - List Report - exported in site - " + $("#lblSiteName_BatterySummary").text());
            }
            catch (e) {

            }
        }

        function GetBatteryGraph() {
            document.getElementById("divPieChartBatterySummary").style.display = "none";
            document.getElementById('divPaginationRow').style.display = "none";
            document.getElementById('btngraph').style.display = "none";
            document.getElementById('btnlist').style.display = "";
            document.getElementById('divTableBatterySummary').style.display = "none";
            document.getElementById('divchartBatterySummary').style.display = "";
            document.getElementById("btnExportBatteryList").style.display = "none";
            document.getElementById("btnExportGraphReport").style.display = "";
            document.getElementById("divchartBatterySummary").className = "page1";
            document.getElementById("divPieChartBatterySummary").className = "page2";
        }

        function BackToBarGraph() {
            document.getElementById('btnBackToBarGraph').style.display = "none";
            FlipCharts();
        }

        function funBin() {
            g_Bin = document.getElementById("ddSearchBin").value;
        }

        function deviceListDownload(DeviceType) {
            window.open("ReportPdf.aspx?SiteId=" + SiteId + "&DeviceType=" + DeviceType + "&TypeId=" + typeId + "&Bin=" + g_CurBin + "&CertExport=1");
            return false;
        }

        function DisplayStarSettingsDetails(isClick) {

            isButtonClicked = isClick;

            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divEMDeviceDetails').hide('slide', { direction: 'right' }, 100);
            $('#divSersorImagesView').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divPatientTag').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divStarSettings').show('slide', { direction: 'left' }, 700);

            document.getElementById("ctl00_ContentPlaceHolder1_lblsitename_Star").innerHTML = document.getElementById("ctl00_ContentPlaceHolder1_lblsitename").innerHTML;
            document.getElementById("subHeaderStar").innerHTML = "Star Settings";
            document.getElementById('lblsiteTagTimeZone_Star').innerHTML = document.getElementById('lblsiteTagTimeZone').innerHTML;
        }

        function doDownloadDisasterRecovery() {
            if (GetBrowserType() == "isIE") {
                location.href = "AjaxConnector.aspx?cmd=DownloadExcelDisasterRecovery_ForIE&sid=" + $("#ctl00_headerBanner_drpsitelist").val();
            }
            else if (GetBrowserType() == "isFF") {
                document.getElementById("divExcelLoading").style.display = "";
                DownloadDisasterRecovery(SiteId);
            }
            try {
                PageVisitDetails(g_UserId, "Home - Disaster Recovery", enumPageAction.View, $("#subHeaderStar").text() + " list exported for site - " + $("#ctl00_ContentPlaceHolder1_lblsitename_Star").text());
            }
            catch (e) {

            }
        }

        function ImportDisasterRecovery() {

            var deviceType = $("#ctl00_ContentPlaceHolder1_ddlDeviceType").val();

            if ($("#ctl00_headerBanner_drpsitelist").val() == "0") {
                alert("Select Site!.");
                return;
            }

            var stitle = "";

            $("#ifrmUploadExcel").attr("src", "uploadFile.aspx?Cmd=ImportDisasterRecovery&SiteId=" + $("#ctl00_headerBanner_drpsitelist").val());

            //Open Dialog
            $("#dialog-UploadExcelFile").dialog({
                height: 425,
                width: 600,
                modal: true,
                title: 'Import Disaster Recovery CSV File',
                show: {
                    effect: "fade",
                    duration: 500
                },
                hide: {
                    effect: "fade",
                    duration: 500
                }
            });
        }

        function gotoSettings() {
            location.hash = "#StarSettings";
        }

        function getSitelistForsettings(stat) {
            gSettings_obj = CreateInfraXMLObj();

            if (gSettings_obj != null) {

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

                    var o_SiteName = dsRoot.getElementsByTagName('SiteID')
                    nRootLength = o_SiteName.length;
                    var SiteId = "";
                    if (nRootLength > 0) {

                        for (var i = 0; i < nRootLength; i++) {

                            SiteAllowedForSettings = SiteAllowedForSettings + setundefined((o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text)) + ',';

                        }


                    }
                }
            }
        }   
    </script>
    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <!-- HOME - SITE SUMMARY PAGE-->
                <div id="divHome" runat="server" style="display: none; top: auto; left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 85%;" id="siteinfo"
                                    runat="server">
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div id="tooltip1" class="tooltip11" style="visibility: hidden;">
                    </div>
                    <div id="divIcons" style="display: none; position: absolute; height: 30px; border: solid 1px #848484;
                        background-color: #FFFFFF; vertical-align: middle; padding: 4px; border-radius: 10px;
                        moz-border-radius: 10px; -webkit-border-radius: 10px;">
                    </div>
                </div>
                <!-- DEVICE LIST PAGE -->
                <div id="divPatientTag" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 86%;" id="tagtblHeader"
                                    runat="server">
                                    <tr style="height: 20px;">
                                        <td>
                                            <div id="tooltipdeviceDetail" class="tooltip11" style="visibility: hidden;">
                                            </div>
                                            <input type="hidden" id="hid_sid" runat="server" />
                                            <input type="hidden" id="hid_alertid" runat="server" />
                                            <input type="hidden" id="hid_typeId" runat="server" />
                                            <input type="hidden" id="hid_bin" runat="server" />
                                            <input type="hidden" id="hid_ttcnt" runat="server" />
                                            <input type="hidden" id="hid_userrole" runat="server" />
                                            <input type="hidden" id="hid_IsTempMonitoring" runat="server" />
                                            <input type="hidden" id="hid_AccessForStar" runat="server" />
                                            <input type="hidden" id="hid_userid" runat="server" />
                                            <input type="hidden" id="hid_userViews" runat="server" />
                                            <input type="hidden" id="hidUpdateLocalId" />
                                            <input type="hidden" id="hidLogFileDate" />
                                            <input type="hidden" id="hidLogFileId" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="145%" id="tdHeaderBorder">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="DisplayPreviousPage(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                            <tr>
                                                                <td align='left' class='SHeader1' colspan='2'>
                                                                    <asp:label id="lblsitename" runat="server"></asp:label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black' id="subHeader">
                                                                </td>
                                                                <td align='right' class='subHeader_black'>
                                                                    <label id="lblsiteTagTimeZone" style="color: #159b48;">
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <input type="button" value="Search" id="btnSearchDeviceList" class="clsExportExcel"
                                    style="display: none;" onclick="showDeviceListSearchRow();" />
                            </td>
                        </tr>
                        <!-- Device List Search ROW -->
                        <tr id="trDeviceListSearchRow" style="display: none;">
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 125%;" id="tblDeviceListRowSearch">
                                    <tr>
                                        <td style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;" class="clsDeviceListSearch">
                                                <tr>
                                                    <td>
                                                        <b>Devices:</b>
                                                    </td>
                                                    <td style="width: 50%;">
                                                    </td>
                                                    <td>
                                                        <b>Search&nbsp;in:</b>
                                                    </td>
                                                    <td>
                                                        <select id="ddSearchBin" onchange="funBin();">
                                                            <option value="">All Devices</option>
                                                            <option value="0">Good</option>
                                                            <option value="1" id="optUnderwatch">Less than 90 days</option>
                                                            <option value="2" id="optLBI">Less than 30 days</option>
                                                            <option value="7">Offline (Battery Depleted)</option>
                                                            <option value="3">Offline (Other)</option>
                                                        </select>
                                                    </td>
                                                    <td align="right">
                                                        <input type="button" id="btnSearchDeviceListGO" value="GO" class="btnGO" onclick="btnSearchDeviceListGO_Clicked();" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="10">
                                                        <textarea id="txtSearchDeviceIds" rows="2" cols="1" style="width: 99%;" class="clsDeviceListSearchtxt"></textarea>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 15px;">
                                        </td>
                                    </tr>
                                    <!-- Search Device List ROW -->
                                    <tr id="trSearchDeviceListRow">
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td valign="top">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                            <tr>
                                                                <td class="txttotalpage" style="width: 360px; height: 35px;" valign="middle">
                                                                    <label class="subheader">
                                                                        Search Results:
                                                                    </label>
                                                                    &nbsp;&nbsp;<input type="button" id="btnExportSearch" value="Export" class="clsExportExcel"
                                                                        onclick="ExportSearchByBrowserType();" />
                                                                    &nbsp;&nbsp;<input type="button" id="btnEditSearchDevice" class="clsExportExcel"
                                                                        value="Edit Devices" onclick="doSearchShow();" />&nbsp;&nbsp;
                                                                    <input type="hidden" id="hdn_Search_Show" value="0" />
                                                                </td>
                                                                <td valign="middle">
                                                                    <img src="Images/Device_Delete.png" alt="Add" id="AddSearchDevice" class="clsAddDevice"
                                                                        onclick="DeleteDevices()" style="height: 30px; width: 30px;" onmouseover="btnAddEdit_DeviceDetailsOver(this);"
                                                                        onmouseout="btnAddEdit_DeviceDetailsOut(this);" />&nbsp;&nbsp;
                                                                    <img src="Images/Device_Add.png" alt="Add" id="DeleteSearchDevice" class="clsAddDevice"
                                                                        onclick="OpenAddDeviceDialog('Add Device','Add')" style="height: 30px; width: 30px;"
                                                                        onmouseover="btnAddEdit_DeviceDetailsOver(this);" onmouseout="btnAddEdit_DeviceDetailsOut(this);" />
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 400px; display: none;" id="tdSearchCalibration" valign="middle"
                                                                                align="right">
                                                                                <input type="button" id="btnSearchImportRecalibration" class="clsExportExcel" value="Re-calibration Frequency"
                                                                                    onclick="OpenAnnualCalibrationDialog();" title="Import Sensor Re-calibration Frequency"
                                                                                    style="width: 160px;" />&nbsp;&nbsp;
                                                                                <input type="button" id="btnSearchCalibrationReport" class="clsExportExcel" value="Report"
                                                                                    onclick="DownLoaddAnnualCalibration();" title="Report for sensors needing calibration"
                                                                                    style="width: 80px;" />&nbsp;&nbsp;
                                                                            </td>
                                                                            <td style="width: 100px; display: none;" id="tdImport" valign="middle">
                                                                                <input type="button" id="btnImportG2Data" class="clsExportExcel" value="Import" title="Import"
                                                                                    style="width: 80px;" onclick="UploadG2Data();" />
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
                                                <tr>
                                                    <td valign="top">
                                                        <div style="width: 100%; overflow: auto;">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                                                id="tblTagInfoSearch">
                                                            </table>
                                                        </div>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                                            id="tblInfraInfoSearch">
                                                        </table>
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                                            id="tblStarInfoSearch">
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 15px;">
                            </td>
                        </tr>
                        <!-- Device List ROW -->
                        <tr id="trDeviceListRow" style="display: none;">
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 125%;" id="tblDeviceListRow">
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td class="txttotalpage" style="height: 35px" valign="middle">
                                                        <asp:label id="lbltotalcount" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;
                                                        <input type="button" id="btnExportExcel" class="clsExportExcel" value="Export" title="Export Excel"
                                                            onclick="doDownload();" disabled="disabled" style="width: 60px;" />&nbsp;&nbsp;
                                                        <div style="display: none; float: right;" id="divExcelLoading">
                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                        </div>
                                                        <input type="button" id="btnEditDevice" class="clsExportExcel" value="Edit Devices"
                                                            onclick="doShow();" />&nbsp;&nbsp;<img src="Images/Device_Delete.png" alt="Add" id="DeleteDevice"
                                                                class="clsAddDevice" onclick="DeleteDevices()" style="height: 30px; width: 30px;
                                                                vertical-align: middle;" onmouseover="btnAddEdit_DeviceDetailsOver(this);" onmouseout="btnAddEdit_DeviceDetailsOut(this);" />
                                                        <img src="Images/Device_Add.png" alt="Add" id="AddDevice" class="clsAddDevice" onclick="OpenAddDeviceDialog('Add Device','Add')"
                                                            style="height: 30px; width: 30px; vertical-align: middle;" onmouseover="btnAddEdit_DeviceDetailsOver(this);"
                                                            onmouseout="btnAddEdit_DeviceDetailsOut(this);" />
                                                        <input type="button" id="btnSettingsDevice" class="clsExportExcel" value="Settings"
                                                            title="Settings" onclick="gotoSettings();" style="width: 80px; display: none;" />
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 400px; display: none;" id="tdCalibration" valign="middle" align="right">
                                                                    <input type="button" id="btnImportRecalibration" class="clsExportExcel" value="Re-calibration Frequency"
                                                                        onclick="OpenAnnualCalibrationDialog();" title="Import Sensor Re-calibration Frequency"
                                                                        style="width: 160px;" />&nbsp;&nbsp;
                                                                    <input type="button" id="btnCalibrationReport" class="clsExportExcel" value="Report"
                                                                        onclick="DownLoaddAnnualCalibration();" title="Report for sensors needing calibration"
                                                                        style="width: 80px;" />&nbsp;&nbsp;
                                                                    <input type="button" id="btnCERTEXPORT" class="clsExportExcel" value="CERT EXPORT"
                                                                        onclick="deviceListDownload(1);" title="Download Device list into excel" style="width: 100px;" />&nbsp;&nbsp;
                                                                    <input type="hidden" id="hid_Show" value="0" />
                                                                </td>
                                                                <td style="width: 100px; display: none" valign="middle" id="tdSearchImport">
                                                                    <input type="button" id="btnSearchImport" class="clsExportExcel" value="Import" title="Import"
                                                                        style="width: 80px;" onclick="UploadG2Data();" />
                                                                </td>
                                                                <td class="clsTableTitleText" align="right" style="width: 450px; display: none;"
                                                                    id="tdPagination">
                                                                    <input type="button" id="btnPrev" class="clsPrev" onclick="doPrev();" title="Previous" />
                                                                    <asp:label id="lblPage" runat="server" cssclass="clsCntrlTxt"> Page </asp:label>
                                                                    <input id="txtPageNo" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                                        type="text" size="1" maxlength="4" runat="server" name="txtPageNo" value="1" />
                                                                    <asp:label id="lblTotalpage" runat="server" cssclass="clsCntrlTxt">&nbsp;</asp:label>&nbsp;
                                                                    <input type="button" id="btnGo" class="btnGO" value="Go" onclick="gotoPage();" />&nbsp;&nbsp;
                                                                    <input type="button" id="btnNext" class="clsNext" onclick="doNext();" title="Next" />
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
                                            <div style="width: 100%; overflow: auto;">
                                                <table border="0" cellpadding="0" cellspacing="0" style="display: none;" id="tblTagInfo">
                                                </table>
                                            </div>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                                id="tblInfraInfo">
                                            </table>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; display: none;"
                                                id="tblStarInfo">
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                    </table>
                    <div style="position: fixed; top: 400px; left: 1100px; display: none;" id="divLoading">
                        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                    </div>
                </div>
                <!-- STAR SETTINGS PAGE -->
                <div id="divStarSettings" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 86%;" id="tagtblHeaderStar"
                                    runat="server">
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="145%" id="tdHeaderBorderStar">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a href="#divPatientTag" onclick="DisplayPatientTagfromDeviceDetails(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                            <tr>
                                                                <td align='left' class='SHeader1' colspan='2'>
                                                                    <asp:label id="lblsitename_Star" runat="server"></asp:label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black' id="subHeaderStar">
                                                                </td>
                                                                <td align='right' class='subHeader_black'>
                                                                    <label id="lblsiteTagTimeZone_Star" style="color: #159b48;">
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="3">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="siteOverview_TopLeft_Box_DeviceDetailsHeaderText" style="border: none;
                                background-color: transparent; color: black;" id="" align="left" width="800px"
                                height="30px" colspan="2">
                                Disaster Recovery
                            </td>
                        </tr>
                        <tr>
                            <td class="DeviceList_leftBox_DeviceDetailsDatasText" style="border: none; padding: 20px;
                                padding-top: 10px; padding-left: 10px;" align="left" width="400px" height="25px"
                                colspan="1">
                                <span class="DeviceList_DeviceDetailsDatasLabel" style="font-size: 500; font-style: italic;">
                                    If a star cannot connect to its default server, it will attempt to connect to<br />
                                    the server specified for the star imported from this page.</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="DeviceList_leftBox_DeviceDetailsDatasText" style="border: none; padding-left: 40px;"
                                align="left" width="400px" height="25px" colspan="1">
                                <input type="button" id="btnExportDisasterRecovery" class="clsExportExcel" value="Export"
                                    onclick="doDownloadDisasterRecovery();" title="Export" style="width: 60px;" />
                                <input type="button" id="btnImportDisasterRecovery" class="clsExportExcel" value="Import"
                                    onclick="ImportDisasterRecovery();" style="margin-left: 20px; width: 60px;" title="Import" />
                            </td>
                        </tr>
                        <tr>
                            <td class="DeviceList_leftBox_DeviceDetailsDatasText" style="border: none; padding: 20px;
                                padding-left: 40px;" align="left" width="400px" height="25px" colspan="1">
                                <span class="DeviceList_DeviceDetailsDatasLabel" style="font-size: 500; font-style: italic;">
                                    Export the template to map star MAC address to server IP. If<br />
                                    mappings are currently configured they will appear in this export.<br />
                                    Edit the file, then import it to save changes.</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- ADD DEVICEID-->
                <div id="dialogAddDevice" title="" style="display: none;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="height: 20px;">
                        </tr>
                        <tr align="left">
                            <td class="clsLALabel" style="width: 80px;" align="right">
                                Device Id&nbsp;:&nbsp;
                            </td>
                            <td style="width: 320px;" align="left">
                                <textarea id="txtDeviceId" style="width: 450px; height: 70px" class="text ui-widget-content ui-corner-all"></textarea>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <input type="button" id="btnAddDevice" class="clsExportExcel" onclick="return AddDelDevices();"
                                    value=" Add " />
                                <input type="button" id="btnCancelDevice" class="clsExportExcel" onclick="return CnacelDevices();"
                                    value=" Cancel " />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <label id="lblAddDialogMsg" class="clsMapErrorTxt">
                                </label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none;" id="divLoading_AddDialogView">
                        <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                    </div>
                </div>
                <!-- SITE OVERVIEW PAGE-->
                <div id="divSiteOverview" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 92%;" border="0">
                        <tr style="height: 20px;">
                            <td>
                                <div id="divSiteOverviewToolTip" class="tool3" style="display: none;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                            <a href="#" onclick="DisplayHome2(1);">
                                                <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                    border="0" /></a>
                                        </td>
                                        <td style='width: 15px;' valign="top">
                                        </td>
                                        <td align='left' valign="top" style="width: 581px;">
                                            <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                                <tr>
                                                    <td align='left' class='SHeader1' colspan='2'>
                                                        <label id="lblSiteName_SiteOverview">
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr id="trPlatformversion" style="display: none;">
                                                    <td class='subHeader_black1' colspan='2'>
                                                        <label id="lblPCServerVersion" class='subHeader_black1'>
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left' class='subHeader_black'>
                                                        Site Overview
                                                    </td>
                                                    <td align='right' class='subHeader_black'>
                                                        <label id="lblsiteTimeZone" style="color: #159b48;">
                                                        </label>
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
                        <tr style="height: 5px;">
                            <td class="bordertop" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="siteOverview">
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="siteEnvOverview">
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="siteotherDevice">
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr id="trDefinedinConnectCore" style="display: none;">
                            <td>
                                <label class="cssItalic" id="lblDefinedinConnectCore">                                   
                                </label>
                            </td>
                        </tr>
                    </table>
                    <div style="position: fixed; top: 400px; left: 1100px; display: none;" id="divLoading_SiteOverview">
                        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                    </div>
                </div>
                <!-- BATTERY SUMMARY -->
                <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>
                <div id="BatterySummary" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 85%;" border="0">
                        <tr style="height: 20px;">
                            <td>
                                <div id="divBatterySummaryToolTip" class="tool3" style="display: none;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                            <a onclick="DisplayPreviousPage(1);">
                                                <img src='images/Left-Arrow.png' title='Site Summary' style="width: 16px; height: 24px;"
                                                    border="0" /></a>
                                        </td>
                                        <td style='width: 15px;' valign="top">
                                        </td>
                                        <td align='left' valign="top" style="width: 800px;">
                                            <table border='0' cellpadding='0' cellspacing='0'>
                                                <tr>
                                                    <td align='left' class='SHeader1'>
                                                        <label id="lblSiteName_BatterySummary">
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr id="trPlatformversionBatterySummary" style="display: none;">
                                                    <td class='subHeader_black1'>
                                                        <label id="lblPCServerVersionBatterySummary" class='subHeader_black1'>
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left' class='subHeader_black'>
                                                        Battery Summary
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
                        <tr style="height: 5px;">
                            <td class="bordertop" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0" style="width: 100%;" border="0" id="tblBatteryListView">
                                    <tr>
                                        <td style="height: 5px; width: 70px;" class='clsAssetTrackingSelection'>
                                            Campus :
                                        </td>
                                        <td>
                                            <select id="selCampus" style="width: 210px;">
                                            </select>
                                        </td>
                                        <td style="width: 20px;">
                                        </td>
                                        <td style="height: 5px; width: 70px;" class='clsAssetTrackingSelection' align="right">
                                            Building :
                                        </td>
                                        <td>
                                            <select id="selBuilding" style="width: 210px;">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px; width: 70px;" class='clsAssetTrackingSelection' align="right">
                                            Floors :
                                        </td>
                                        <td>
                                            <select multiple="multiple" id="selFloor" style="width: 210px;">
                                            </select>
                                        </td>
                                        <td style="width: 20px;">
                                        </td>
                                        <td style="height: 5px; width: 70px;" class='clsAssetTrackingSelection' align="right">
                                            Units :
                                        </td>
                                        <td>
                                            <select multiple="multiple" id="selUnits" style="width: 210px;">
                                            </select>
                                        </td>
                                        <td style="width: 20px;">
                                        </td>
                                        <td style="height: 5px;">
                                            <input type="button" id="btnFilter" class="clsExportExcel" value="Filter" style="width: 80px;"
                                                onclick="LoadBatterySummaryGraph()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="height: 5px; width: 120px;" align="left">
                                            <input type="button" id="btnExportGraphReport" class="clsExportExcel" value="Export Report"
                                                style="width: 100px;" onclick="DownLoadExcelGraphReport()" />
                                            <input type="button" id="btnExportBatteryList" class="clsExportExcel" value="Export Report"
                                                style="width: 100px;" onclick="DownLoadExcelBatteryListReport()" />
                                        </td>
                                        <td style="height: 5px; width: 120px;" align="left">
                                            <div id="btnviewload" style="display: none;">
                                                <input type="button" id="btnlist" class="clsExportExcel" value="List View" style="width: 100px;"
                                                    onclick="GetBatteryList() " />
                                                <input type="button" id="btngraph" class="clsExportExcel" value="Graph View" style="width: 100px;"
                                                    onclick="GetBatteryGraph() " />
                                            </div>
                                        </td>
                                        <td>
                                            <input type="button" id="btnBackToBarGraph" class="clsExportExcel" value="Back" style="width: 100px;
                                                display: none;" onclick="BackToBarGraph() " />
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td>
                                            <div style="display: none; float: right;" id="divExcelReportLoading">
                                                <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                                <div id="divPaginationRow" style="display: none;">
                                    <table id="Table1" cellpadding="0" cellspacing="0" style="width: 100%;" border="0"
                                        runat="Server">
                                        <tr class="clsPageNavigationHeader">
                                            <td class="txttotalpage">
                                                <asp:label id="lblBatteryTotalcnt" runat="server"></asp:label>
                                            </td>
                                            <td class="txttotalpage" style="float: right;" valign="middle">
                                                <input type="button" id="btnBatteryPrev" class="clsPrev" onclick="doBatteryPrev();"
                                                    title="Previous" />
                                                <asp:label id="lblBatteryPage" runat="server" cssclass="clsCntrlTxt"> Page </asp:label>
                                                <input id="txtBatteryPageNo" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                    type="text" size="1" maxlength="4" runat="server" name="txtPageNo" value="1" />
                                                <asp:label id="lblBatteryTotalpage" runat="server" cssclass="clsCntrlTxt">&nbsp;</asp:label>&nbsp;
                                                <input type="button" id="btnBatteryGo" class="btnGO" value="Go" onclick="gotoBatteryPage();" />&nbsp;&nbsp;
                                                <input type="button" id="btnBatteryNext" class="clsNext" onclick="doBatteryNext();"
                                                    title="Next" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <div id="divTableBatterySummary" style="display: none;">
                                    <table cellpadding="0" cellspacing="0" border="0" id="tblBatterySummary" style="width: 100%">
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <!-- BATTERY SUMMARY PIE CHART -->
                        <tr>
                            <td valign="top">
                                <div class="container">
                                    <div id="divchartBatterySummary" class="page1">
                                    </div>
                                    <div id="divPieChartBatterySummary" class="page2">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <!--  <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script> -->
                        <script type="text/javascript">
                            $('#selFloor').multipleSelect({
                                onClick: function (view) {
                                    loadunits($('#selFloor').val(), $('#selFloor').multipleSelect('getSelects', 'text'))

                                },
                                onCheckAll: function () {
                                    loadunits($('#selFloor').val(), $('#selFloor').multipleSelect('getSelects', 'text'))

                                },
                                onUncheckAll: function () {

                                    loadunits($('#selFloor').val(), $('#selFloor').multipleSelect('getSelects', 'text'))

                                }
                            });

                            $("#selUnits").multipleSelect({
                                onOptgroupClick: function (view) {

                                    var values = $.map(view.children, function (child) {
                                        return child.value;
                                    }).join(',');
                                },
                                onClick: function (view) {
                                    var unitids = $('#selUnits').val();
                                }
                            });
                        </script>
                    </table>
                    <!-- BATTERY SUMMARY ADVANCED VIEW -->
                    <div id="dialog-BatterySummaryListView" title="Battery List View" style="display: none;">
                        <div style="display: none;" id="divLoading_PD">
                            <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblBatteryListDialog">
                        </table>
                    </div>
                    <div style="position: fixed; top: 400px; left: 800px; display: none;" id="divLoading_BatteryOverview">
                        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                    </div>
                </div>
                <!-- TOOL TIP-->
                <div id="tooltip4" class="tool3" style="display: none;">
                </div>
                <!-- SENSOR IMAGES-->
                <div id="divSersorImagesView" style="display: none; top: auto; left: auto; height: 850px;">
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
                                                        <a href="#divPatientTag" onclick="DisplayPatientTagfromDeviceDetails(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <label id="lblDeviceId_Images">
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align='left' class='subHeader_black'>
                                                                    Sensor Images
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
                                            <div id="divSensorImages">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- DEVICE DETAILS PAGE-->
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
                                                        <a href="#divPatientTag" onclick="DisplayPatientTagfromDeviceDetails(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
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
                                            <table runat="server" cellspacing="0" cellpadding="0" border="0" style="width: 100%;">
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
                <!-- Import Sensor Recalibration Frequency !-->
                <div id="dialog-AnnualCalibration" title="Import Sensor Recalibration Frequency"
                    style="display: none;">
                    <table cellpadding="5" cellspacing="5" border="0" width="600px">
                        <tr>
                            <td colspan="2">
                                Enter the tag ids that needs annual recalibration.
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td class="clsLALabel" style="width: 80px;" align="right" valign="top">
                                Tag Ids :
                            </td>
                            <td align="left">
                                <textarea id="txtAnnualDevices" name="txtDesc" style="width: 500px; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
                                    font-weight: bold; font-size: 13px; height: 150px;" cols="200" rows="6" maxlength="7000"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <input type="button" value="Save" class="clsExportExcel" id="btnAddAnnualDevices"
                                    style="width: 130px;" onclick="SaveAnnualCalibration(0);" />
                                <input type="button" value="Cancel" class="clsExportExcel" id="Button4" style="width: 100px;"
                                    onclick="CancelAnnualCalibrationDialog();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="clsErrorTxt">
                                Notes:Cal Frequency should be entered in whole number of months (e.g., 6, 12, 24
                                months)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div style="display: none;" id="divLoading_SensorRecalibration">
                                    <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="dialog-TagAnnualCalibration" title="Add/Edit Sensor Recalibration Frequency"
                    style="display: none;">
                    <table cellpadding="5" cellspacing="5" border="0" width="600px">
                        <tr style="height: 10px;">
                            <td class="clsLALabel" style="width: 80px;" align="right" valign="middle">
                                Tag Id :
                            </td>
                            <td align="left">
                                <input type="text" id="txtCalFreqTagId" name="txtCalFreqTagId" style="width: 260px;"
                                    disabled="disabled" />
                            </td>
                        </tr>
                        <tr style="height: 10px;" id="tr1">
                            <td class="clsLALabel" style="width: 180px;" align="right" valign="middle">
                                Cal Frequency :
                            </td>
                            <td align="left">
                                <input type="text" id="txtCalFrequency" name="txtCalFrequency" style="width: 60px;"
                                    onkeypress="return allowNumberOnly(event)" maxlength="3" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <input type="button" value="Save" class="clsExportExcel" id="btnAddCalFre" style="width: 130px;"
                                    onclick="SaveAnnualCalibration(1);" />
                                <input type="button" value="Cancel" class="clsExportExcel" id="btnCALFreCancel" style="width: 100px;"
                                    onclick="CancelSingleAnnualCalibrationDialog();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="clsErrorTxt">
                                Notes:Cal Frequency should be entered in whole number of months (e.g., 6, 12, 24
                                months)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div style="display: none;" id="divLoading_CalFre">
                                    <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- Update Tag Local Id !-->
                <div id="dialog-UpdateLocalId" title="Update Location and Local ID" style="display: none;">
                    <table cellpadding="5" cellspacing="5" border="0" width="500px">
                        <tr style="height: 10px;">
                            <td class="clsLALabel" style="width: 80px;" align="right" valign="middle">
                                Tag Id :
                            </td>
                            <td align="left">
                                <input type="text" id="txtTagId" name="txtTagId" style="width: 260px;" disabled="disabled" />
                            </td>
                        </tr>
                        <tr style="height: 10px;" id="trLocation">
                            <td class="clsLALabel" style="width: 80px;" align="right" valign="middle">
                                Location :
                            </td>
                            <td align="left">
                                <input type="text" id="txtLocation" name="txtLocation" style="width: 260px;" />
                            </td>
                        </tr>
                        <tr style="height: 10px;" id="trLocalId">
                            <td class="clsLALabel" style="width: 80px;" align="right" valign="middle">
                                Local ID :
                            </td>
                            <td align="left">
                                <input type="text" id="txtLocalId" name="txtLocalId" style="width: 260px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <input type="button" value="Save" class="clsExportExcel" id="btnUpdateLocalId" style="width: 100px;"
                                    onclick="SaveLocalId();" />
                                <input type="button" value="Cancel" class="clsExportExcel" id="btnCancel" style="width: 100px;"
                                    onclick="CancelLocaldialog();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div style="display: none;" id="divUpdateLocal">
                                    <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- Update Monitor Location !-->
                <div id="dialog-UpdateDeviceLocation" title="Update Device Location" style="display: none;">
                    <table cellpadding="5" cellspacing="5" border="0" width="530px">
                        <tr style="height: 10px;">
                            <td class="clsLALabel" style="width: 80px;" align="left" valign="middle">
                                <label id="lbldeviceid">
                                </label>
                            </td>
                            <td align="left">
                                <input type="text" id="txtDevid" name="txtDevid" style="width: 150px;" disabled="disabled" />
                            </td>
                        </tr>
                        <tr style="height: 10px; display: none;" id="trOldLocation">
                            <td class="clsLALabel" style="width: 80px;" align="left" valign="middle">
                                Location:
                            </td>
                            <td align="left">
                                <input type="text" id="txtOldDeviceLocation" name="txtOldDeviceLocation" style="width: 400px;"
                                    disabled="disabled" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td class="clsLALabel" style="width: 100px;" align="left" valign="middle">
                                New Location:
                            </td>
                            <td align="left">
                                <input type="text" id="txtDeviceLocation" name="txtDeviceLocation" style="width: 400px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <input type="button" value="Save" class="clsExportExcel" id="btnAddDeviceLocation"
                                    style="width: 100px;" onclick="SaveDeviceLocation();" />
                                <input type="button" value="Cancel" class="clsExportExcel" id="btnCancelDeviceLocation"
                                    style="width: 100px;" onclick="CancelLocationdialog();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <div style="display: none;" id="divUpdateLocation">
                                    <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- EM DEVICE DETAILS PAGE-->
                <div id="divEMDeviceDetails" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <div style="position: fixed; width: 100%; top: 50%; left: 50%" id="divLoadingEMDetails">
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
                                                        <a href="#divPatientTag" onclick="DisplayPatientTagfromDeviceDetails(1);">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
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
                <div id="divImportG2TempData" title="Upload G2Temp Info" style="display: none;">
                    <iframe id="ifrmUploadG2TempData" style="border: none; width: 100%; height: 100%;">
                    </iframe>
                </div>
                <!-- UPLOAD Excel VIEW -->
                <div id="dialog-UploadExcelFile" title="Import Disaster Recovery CSV File" style="display: none;">
                    <iframe id="ifrmUploadExcel" style="border: none; height: 280px; width: 460px;">
                    </iframe>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        LoadGlossaryInfo("Home", document.getElementById("<%=hid_userrole.ClientID%>").value);
        showGlossaryInfo("Home")
    </script>
</asp:content>
