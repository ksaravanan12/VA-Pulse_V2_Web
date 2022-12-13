<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Map.aspx.vb" Inherits="GMSUI.Map" Title="Map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .clsLALabeltitle
        {
            font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
            font-size: 15px;
            color: #313232;
            font-weight: bold;
            width: 275px;
        }
        
        .clsLALabelMap
        {
            font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
            font-size: 13px;
            color: #313232;
            font-weight: bold;
            width: 275px;
        }
        
        .clsLALabelVal
        {
            font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
            font-size: 13px;
            color: #313232;
            font-weight: normal;
            width: 275px;
        }
        .olControlLayerSwitcher
        {
            background-color: transparent;
            color: white;
            font-family: sans-serif;
            font-size: smaller;
            font-weight: bold;
            margin-bottom: 3px;
            margin-left: 3px;
            margin-top: 3px;
            position: absolute;
            right: 0;
            top: 42px;
            width: 15em;
        }
    </style>
    <link rel="stylesheet" href="Javascript/theme/default/style.css" type="text/css" />
    <!--<script type="text/javascript"  src="https://gms.centrak.com:3000/socket.io/socket.io.js"></script>-->
    <script type="text/javascript" src="Javascript/OpenLayers.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/jquery-1.10.2.js"></script>
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>
    <script type="text/javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" src="Javascript/Map_floorPlan.js"></script>
    <script type="text/javascript" src="Javascript/js_map.js"></script>
    <script type="text/javascript" src="Javascript/js_TagMetaInfo.js"></script>
    <script type="text/javascript" src="Javascript/jquery.fullscreen-0.4.1.js"></script>
    <script type="text/javascript" src="Javascript/js_deviceDetails.js"></script>
    <style>
        body
        {
            font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
        }
        .ui-autocomplete
        {
            padding: 0;
            list-style: none;
            background-color: #fff;
            width: 218px;
            border: 1px solid #B0BECA;
            max-height: 350px;
            overflow-x: hidden;
            text-align: left;
            font-size: 13px;
            font-family: Arial, Verdana, sans-serif;
            font-weight: bold;
        }
        .ui-autocomplete .ui-menu-item
        {
            border-top: 1px solid #B0BECA;
            display: block;
            padding: 4px 6px;
            color: #353D44;
            cursor: pointer;
        }
        .ui-autocomplete .ui-menu-item:first-child
        {
            border-top: none;
        }
        .ui-autocomplete .ui-menu-item.ui-state-focus
        {
            color: #161A1C;
        }
    </style>
    <script type="text/javascript">
        var g_mapWindowTop = 0;
        var g_mapWindowLeft = 0;

        var g_mapWindowHeight = 0;
        var g_mapWindowWidth = 0;

        var g_mapDivWindowTop = 0;
        var g_mapDivWindowLeft = 0;

        var g_fullScreen = 0;

        var g_isShowTags = 0;
        var g_TagsLoaded = 0;

        var monitorX = -1;
        var monitorY = -1;
        var monitorW = -1;
        var monitorH = -1;
        var RoompolygonPoints = '';
        var g_uMode = 0;
        var g_dsvgId = 0;
        var g_svgDType = 0;
        var g_oldDeviceId = '';

        var g_floorPlanBgLoaded = 0;
        var g_isInfrasturctureLoaded = 0;
        var g_SnapToGrid = 0;

        var g_designMode = 0;
        var screenMode = 0;
        var nReportWidth = 250;
        var nLiveViewRowCount = 50;
        var liveDataFilterArr = new Array();
        var tblLiveUpdateMsgArr = new Array();
        var Xaxis = 0;
        var Yaxis = 0;
        var RoomXaxis = 0;
        var RoomYaxis = 0;
        var WidthFt = 0;
        var LengthFt = 0;

        $('html').keyup(function (e) {
            if (e.keyCode == 46) //For Delete Key
            {
                if (g_designMode == 1 && g_MapView === enumMapView.Map)
                    deleteEditlayerFeature();

            }
            else if (e.keyCode == 27) //For Esc Key
            {
                if (g_designMode == 1 && g_MapView === enumMapView.Map) {
                    //To Escape from Polygon Control
                    if (polygonControl.active) {
                        polygonControl.deactivate();
                        polygonControl.activate();
                    }

                }

            }
        });


        function CloseUploadWindow() {
            $('#dialog-UploadFile').dialog('close');
            loadMapListView(g_MapSiteId);
        }

        function refreshTblLiveUpdateForFilter() {
            $('#tblLiveUpdate').empty();
            var currentRoom;
            var lastRoom;
            var jObj;

            for (var j = 0; j < tblLiveUpdateMsgArr.length; j++) {
                jObj = tblLiveUpdateMsgArr[j];

                if (checkTagId_InliveDataFilterArr(jObj.CenTrakEvent.tagid) == true) {
                    currentRoom = getRoomNameByMonitorId(jObj.CenTrakEvent.monitorid);
                    lastRoom = getRoomNameByMonitorId(jObj.CenTrakEvent.last_monitorid);
                    ShowLog(jObj, currentRoom, lastRoom, 0);
                }

            }
            showFilteredTagRoute();
        }
        function setliveDataFilterArr() {
            var filterStr = document.getElementById("txtliveFilters").value;
            if (trim(filterStr) == '')
                liveDataFilterArr.splice(0, liveDataFilterArr.length);
            else
                liveDataFilterArr = filterStr.split(',');

            refreshTblLiveUpdateForFilter();
            showOnlyFilteredLiveTags();
        }

        function clearLiveUpdates() {
            $('#tblLiveUpdate').empty();
            tblLiveUpdateMsgArr.splice(0, tblLiveUpdateMsgArr.length);
            liveDataFilterArr.splice(0, liveDataFilterArr.length);
            document.getElementById("txtliveFilters").value = '';
        }

        function checkTagId_InliveDataFilterArr(tagId) {
            if (liveDataFilterArr.length == 0)
                return true;
            return liveDataFilterArr.indexOf(tagId) > -1 ? true : false;
        }

        window.onhashchange = function () {
            if (isButtonClicked == 1) {
                isButtonClicked = 0;
                return;
            }
            if (location.hash == "" || location.hash == "#") {
                DisplayMapfromDeviceDetails(0);
            }
        }

        function MapDesignMode() {

            if (g_designMode == 0) {
                g_Unsaved = 0;
                g_designMode = 1;
                showHideMapDesignMode(1, 0);
                openSetupPanel();
            }
            else {
                //Check for un saved data
                if (g_Unsaved == 1) {
                    if (confirm("Do you want to save the changes?")) {
                        $('#btnSaveMonitor').click();
                        return;
                    }
                    else {
                        g_Unsaved = 0;
                    }
                }
                g_designMode = 0;
                monitorX = -1;
                monitorY = -1;
                monitorW = -1;
                monitorH = -1;
                RoompolygonPoints = '';
                cancelClicked();
                document.getElementById("lblsaveMonitorResult").innerHTML = "";
                ClearTheEntries();
                showHideMapDesignMode(0, 0);
                g_SnapToGrid = 0;
                showHideSnapGridLayer(0);
            }
        }

        function showHideMapDesignMode(flag, isInitialLoad) {
            if (flag == 1) {
                document.getElementById("shReports").style.display = "none";
                document.getElementById("shSearch").style.display = "none";
                document.getElementById("shtdBeforeDesignMode").style.display = "none";
                document.getElementById("btnDesignMode").value = "Exit Installation Mode";
                showHideReports(0, 0);
            }
            else {
                document.getElementById("shReports").style.display = "";
                document.getElementById("shSearch").style.display = "";
                document.getElementById("shtdBeforeDesignMode").style.display = "";
                document.getElementById("btnDesignMode").value = "Enter Installation Mode";
                document.getElementById("divAddMonitors").style.display = "none";

                if (isInitialLoad == 0) {
                    showHideReports(0, 0);
                }
            }
            if (isInitialLoad != 1)
                changeMapMode();
        }

        $(document).ready(function () {
            $("#txtMonitorId").on('change keyup paste', function () {
                g_Unsaved = 1;
            });
            $("#txtLocation").on('change keyup paste', function () {
                g_Unsaved = 1;
            });
            $("#txtUnitName").on('change keyup paste', function () {
                g_Unsaved = 1;
            });
            $("#chkIsHallway").on('change', function () {
                g_Unsaved = 1;
            });
            $("#txtNotes").on('change keyup paste', function () {
                g_Unsaved = 1;
            });

            $(document).mouseup(function (e) {
                var container = $("#dialogMonitorReport");
                var container2 = $("#dialogStarReport");

                if (container.has(e.target).length === 0) {
                    if (document.getElementById("dialogMonitorReport").style.display != "none") {
                        $("#dialogMonitorReport").hide(300);
                    }
                }

                if (container2.has(e.target).length === 0) {
                    if (document.getElementById("dialogStarReport").style.display != "none") {
                        $("#dialogStarReport").hide(300);
                    }
                }
            });

            g_MapSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;
            //createSocketConForLiveUpdates();
        });

        var enumDeviceType = { Tag: 1, Monitor: 2, Star: 3 };
        var enumTagType = {};
        var enumMonitorType = {};
        var enumStarType = {};
        var enumCndtType = { '=': 2, '<=': 4, '>=': 3, 'between': 5 };
        var enumCndtTypeIfValuefound = { '=': 2 };
        var enumFilterType = {};

        var g_LVTop = 0;
        var scrolled = true;
        var bgcolor = '';


        function ShowLog(jObj, currentRoom, lastRoom, isneedtoUpdateInMap) {

            var msg;
            if (lastRoom == "Unknown")
                msg = "<b>" + jObj.CenTrakEvent.tagid + "</b> entered '" + currentRoom + "' at " + getTimeWithMeridian(jObj.CenTrakEvent.time);
            else
                msg = "<b>" + jObj.CenTrakEvent.tagid + "</b> entered '" + currentRoom + "' from '" + getRoomNameByMonitorId(jObj.CenTrakEvent.last_monitorid) + "' at " + getTimeWithMeridian(jObj.CenTrakEvent.time);

            if (isneedtoUpdateInMap == 1)
                checkTagLiveDataIsForCurrentFloor(jObj, msg);

            if (checkTagId_InliveDataFilterArr(jObj.CenTrakEvent.tagid) == false) // no need to show if filters is ON
                return;


            if (bgcolor == '' || bgcolor == '#E0F2FE') {
                bgcolor = "#FFFFFF";
            }
            else
                bgcolor = "#E0F2FE";

            if ($('#tblLiveUpdate tr').length > 0) {
                if ($('#tblLiveUpdate tr').length >= nLiveViewRowCount) {
                    $('#tblLiveUpdate tr:first-child').remove();
                    $('#tblLiveUpdate tr:first-child td').css({ 'border-top': 'solid 1px #9C9C9C' });
                }

                $('#tblLiveUpdate').append('<tr><td style="background-color:' + bgcolor + '; color: #000000; font-size: 11px; font-weight: normal; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; width: 120px; height: 20px; vertical-align: middle; border-bottom: solid 1px #9C9C9C; border-right: solid 1px #9C9C9C; border-left: solid 1px #9C9C9C;">' + msg + '</td></tr>');
            } else {
                $('#tblLiveUpdate').append('<tr><td style="background-color:' + bgcolor + '; color: #000000; font-size: 11px; font-weight: normal; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; width: 120px; height: 20px; vertical-align: middle; border: solid 1px #9C9C9C;">' + msg + '</td></tr>');
            }

            if (parseInt($("#tabs-Liveview").css("height").replace('px', '')) != parseInt($('#map').css('height').replace('px', '')) - 75) {
                $("#tabs-Liveview").css("height", parseInt($('#map').css('height').replace('px', '')) - 75 + 'px');
            }
            else {
                $("#tabs-Liveview").css("height", parseInt($('#tabs-Liveview').css('height').replace('px', '')) + 'px');
            }

            g_LVTop = document.getElementById("tabs-Liveview").scrollTop;
            if (scrolled) {
                $("#tabs-Liveview").animate({ scrollTop: document.getElementById("tabs-Liveview").scrollHeight }, 'slow');
            }
        }

        $(function () {
            $("#tabs-Liveview").on('scroll', function () {
                var top = document.getElementById("tabs-Liveview").scrollTop;

                if (parseInt(g_LVTop) < parseInt(top)) {
                    scrolled = true;
                } else {
                    scrolled = false;
                }
            });

            //Dropdown in Reports
            loadValuesintoDropdown($('#selDeviceType'), enumDeviceType);

            $('#selDeviceType').change(function () {
                $('#selFilterType').hide();
                $('#selCndType').hide();
                $('#txtFilter1').hide();
                $('#txtFilter2').hide();
                $('#selFilterFloor').hide();

                $('#selFilterType').val('0');
                $('#selCndType').val('0');
                $('#txtFilter1').val('');
                $('#txtFilter2').val('');
                $('#selFilterFloor').val('0');

                if ($('#selDeviceType option:selected').val() > 0) {
                    $('#selFilterType').show();

                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px', '') - 180 + 'px');
                } else {
                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px', '') - 160 + 'px');
                }

                if ($('#selDeviceType option:selected').val() == String(enumDeviceType.Tag))
                    loadValuesintoDropdown($('#selFilterType'), enumTagType);
                else if ($('#selDeviceType option:selected').val() == String(enumDeviceType.Monitor))
                    loadValuesintoDropdown($('#selFilterType'), enumMonitorType);
                else if ($('#selDeviceType option:selected').val() == String(enumDeviceType.Star))
                    loadValuesintoDropdown($('#selFilterType'), enumStarType);
            });

            $('#selFilterType').change(function () {
                $('#selCndType').hide();
                $('#txtFilter1').hide();
                $('#txtFilter2').hide();
                $('#selFilterFloor').hide();

                $('#selCndType').val('0');
                $('#txtFilter1').val('');
                $('#txtFilter2').val('');
                $('#selFilterFloor').val('0');

                if ($('#selFilterType option:selected').val() != "0") {
                    $('#selCndType').show();

                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px', '') - 200 + 'px');
                } else {
                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px', '') - 180 + 'px');
                }

                if ($('#selDeviceType option:selected').val() == String(enumDeviceType.Tag) || $('#selDeviceType option:selected').val() == String(enumDeviceType.Monitor)) {
                    if ($('#selFilterType option:selected').val() == "LessThan90Days" || $('#selFilterType option:selected').val() == "LessThan30Days") {
                        $('#selCndType').hide();
                        $('#selCndType').val('0');
                    }
                    else {
                        $('#selCndType').show();
                        if (checkValueExists())
                            loadValuesintoDropdown($('#selCndType'), enumCndtTypeIfValuefound);
                        else
                            loadValuesintoDropdown($('#selCndType'), enumCndtType);
                    }
                } else {
                    $('#selCndType').show();
                    if (checkValueExists())
                        loadValuesintoDropdown($('#selCndType'), enumCndtTypeIfValuefound);
                    else
                        loadValuesintoDropdown($('#selCndType'), enumCndtType);
                }
            });

            $('#selCndType').change(function () {
                $('#txtFilter1').hide();
                $('#txtFilter2').hide();
                $('#selFilterFloor').hide();

                $('#txtFilter1').val('');
                $('#txtFilter2').val('');
                $('#selFilterFloor').val('0');

                if ($('#selCndType option:selected').val() > 0) {
                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px', '') - 220 + 'px');

                    fillDropdownifValuesfound();
                } else {
                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px', '') - 200 + 'px');
                }
            });
            //End Dropdown in Reports

            document.getElementById("tdSelectedFloor").style.display = "none";
            // check native support
            $('#support').text($.fullscreen.isNativelySupported() ? 'supports' : 'doesn\'t support');

            $('#btnRegular').click(function () {
                regularSize();
                exitfullScreen();
            });

            $('#btnSearch').click(function () {
                if (document.getElementById("txtSearchDevices").value == "")
                    alert("Please provide a device id or mac id to search");
                else {
                    document.getElementById("tdSelectFloor").style.display = "none";
                    SearchDeviceMap($('#ctl00_headerBanner_drpsitelist').val(), $('#txtSearchDevices').val());
                }
            });

            $('#btnShowTag').click(function () {
                if (g_isShowTags)//hide tags
                {
                    g_isShowTags = 0;
                    $('#btnShowTag').removeClass("mapShowTags").addClass("mapNoShowTags");
                    for (var i = 0; i < taglayersArray.length; i++) {
                        taglayersArray[i].setVisibility(false);
                    }
                    multiTagLayer.setVisibility(false);
                }
                else {
                    g_isShowTags = 1;
                    $('#btnShowTag').removeClass("mapNoShowTags").addClass("mapShowTags");
                    if (g_TagsLoaded === 0) {
                        AdjustLoadingDiv();
                        document.getElementById("divLoadingMap").style.display = "";
                        var refId = $('#selFloor').val();
                        GetTagMetaInfoForFloor(g_MapSiteId, refId);
                    }
                    else {
                        //hide the Tag layer
                        for (var i = 0; i < taglayersArray.length; i++) {
                            taglayersArray[i].setVisibility(true);
                        }
                        multiTagLayer.setVisibility(true);
                    }
                }
            });

            $('#btnSearchDevice').click(function () {
                if (document.getElementById("txtSearchDevice").value == "")
                    alert("Please provide a device id to search");
                else {

                    searchDevice(document.getElementById("txtSearchDevice").value);
                }

            });

            $('#btnSearchCancel').click(function () {
                document.getElementById("txtSearchDevice").value = "";
                clearHighLightLayer();
            });

            $('#btnReports').click(function () {
                btnMapOut(document.getElementById("btnReports"));
                showHideReports(0, 1);

            });

            $('#btnLarge').click(function () {
                largeSize();
                exitfullScreen();
            });

            // open in fullscreen
            $('#btnFullScreen').click(function () {
                $('#dialogMonitorReport').css({ 'display': 'none' })
                $('#dialogStarReport').css({ 'display': 'none' })

                if (g_fullScreen == 0) {
                    g_fullScreen = 1;

                    var width = screen.width;
                    var height = screen.height;

                    if (g_designMode == 1) {
                        $('#mapDiv').css({ 'position': 'fixed', 'top': '0px', 'left': '0px', 'height': '40px', 'width': width + 'px' });

                        $('#divAddMonitors').css({ 'position': 'fixed', 'top': '41px', 'left': '0px', 'width': width + 'px', 'height': '100px' });
                        $('#map').css({ 'position': 'fixed', 'top': '142px', 'left': '0px', 'width': width + 'px', 'height': (height - 60 - 100) + 'px' });
                    }
                    else {
                        $('#mapDiv').css({ 'position': 'fixed', 'top': '0px', 'left': '0px', 'height': '40px', 'width': width + 'px' });
                        $('#map').css({ 'position': 'fixed', 'top': '42px', 'left': '0px', 'width': width + 'px', 'height': (height - 50) + 'px' });
                    }
                    resizeMap('0');
                    $('#mapWindow').fullscreen();

                    screenMode = 2;

                    if ($('#dialogReport').is(":visible")) {
                        showHideReports(1, 0);
                        loadFlooronScreenDiffer();
                    }
                    $("#tabs-Liveview").css("height", parseInt($('#map').css('height').replace('px', '')) - 75 + 'px');
                    return false;
                }
                else if (g_fullScreen == 1) {
                    regularSize();
                    loadFlooronScreenDiffer();
                    exitfullScreen();
                }
            });

            // exit fullscreen
            $('#mapWindow .exitfullscreen').click(function () {
                $.fullscreen.exit();
                resizeMap('0');
                g_fullScreen = 0;
                return false;
            });

            // document's event
            $(document).bind('fscreenchange', function (e, state, elem) {
                // if we currently in fullscreen mode
                if ($.fullscreen.isFullScreen()) {
                    $('#mapWindow .requestfullscreen').hide();
                    $('#mapWindow .exitfullscreen').show();
                } else {
                    $('#mapWindow .requestfullscreen').show();
                    $('#mapWindow .exitfullscreen').hide();

                    if ($('#dialogReport').is(":visible")) {
                        screenMode = 0;
                        showHideReports(1, 0);
                        loadFlooronScreenDiffer();
                    }
                }

                $('#state').text($.fullscreen.isFullScreen() ? '' : 'not');
            });
        });

        function showHideReports(isShow, reportBtnClick) {
            var dialogVisible = false;

            if ($('#dialogReport').is(":visible")) {
                dialogVisible = true;
            }

            if (isShow == 1) {
                dialogVisible = false;
            }

            if (dialogVisible) {
                //$('#dialogReport').hide();
                $("#dialogReport").animate({ width: 'toggle' }, function () {
                    if (g_designMode == 1) return;

                    if (screenMode == 0) {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });

                    } else if (screenMode == 1) {
                        var width = $(document).width();
                        var height = $(document).height();

                        var mapWidth = width - g_mapWindowLeft;
                        var mapHeight = height - g_mapWindowTop;

                        var mapDivWidth = width - g_mapDivWindowLeft;
                        var mapDivHeight = height - g_mapDivWindowTop;

                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': parseInt(mapDivWidth) - 15 + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': parseInt(mapWidth) - 15, 'height': parseInt(mapHeight) - 15 + 'px' });
                    } else if (screenMode == 2) {
                        var width = screen.width;
                        var height = screen.height;

                        $('#mapDiv').css({ 'position': 'fixed', 'top': '0px', 'left': '0px', 'height': '40px', 'width': width + 'px' });
                        $('#map').css({ 'position': 'fixed', 'top': '42px', 'left': '0px', 'width': width + 'px', 'height': (height - 50) + 'px' });
                    }
                });
            } else {
                //$('#dialogReport').show();
                if (reportBtnClick == 1)
                    $("#dialogReport").animate({ width: 'toggle' });
                if (screenMode == 0) {
                    screenMode = 0;
                    if (isShow == 0 && reportBtnClick != 1) {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });

                    }
                    else {
                        $('#dialogReport').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': +'px', 'height': g_mapWindowHeight + 'px' });
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 42 + 'px', 'left': parseInt(g_mapDivWindowLeft) + (parseInt(nReportWidth) + 9) + 'px', 'width': parseInt(g_mapWindowWidth) - (parseInt(nReportWidth) + 10) + 'px', 'height': g_mapWindowHeight + 'px' });
                    }

                } else if (screenMode == 1) {
                    var width = $(document).width();
                    var height = $(document).height();

                    if (isShow == 1) {
                        g_mapWindowTop = $('#map').offset().top;
                        g_mapWindowLeft = $('#map').offset().left;

                        g_mapDivWindowTop = $('#mapDiv').offset().top;
                        g_mapDivWindowLeft = $('#mapDiv').offset().left;
                    }

                    var mapWidth = width - g_mapWindowLeft;
                    var mapHeight = height - g_mapWindowTop;

                    var mapDivWidth = width - g_mapDivWindowLeft;
                    var mapDivHeight = height - g_mapDivWindowTop;

                    screenMode = 1;
                    if (isShow == 0 && reportBtnClick != 1) {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': parseInt(mapDivWidth) - 15 + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': parseInt(mapWidth) - 15, 'height': parseInt(mapHeight) - 15 + 'px' });
                    }
                    else {
                        $('#dialogReport').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': nReportWidth + 'px', 'height': parseInt(mapHeight) - 15 + 'px' });
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': parseInt(mapDivWidth) - 15 + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 42 + 'px', 'left': parseInt(g_mapDivWindowLeft) + (parseInt(nReportWidth) + 10) + 'px', 'width': parseInt(mapWidth) - (parseInt(nReportWidth) + 23) + 'px', 'height': parseInt(mapHeight) - 15 + 'px' });
                    }

                } else if (screenMode == 2) {
                    var width;
                    var height;
                    if (isShow == 0 && reportBtnClick != 1) {
                        width = $(document).width();
                        height = $(document).height();
                        $('#mapDiv').css({ 'position': 'fixed', 'top': '0px', 'left': '0px', 'height': '40px', 'width': width + 'px' });
                        $('#map').css({ 'position': 'fixed', 'top': '42px', 'left': '0px', 'width': width + 'px', 'height': (height - 50) + 'px' });

                    } else {
                        width = screen.width;
                        height = screen.height;
                        $('#dialogReport').css({ 'position': 'absolute', 'top': '42px', 'left': '0px', 'width': nReportWidth + 'px', 'height': (height - 50) + 'px' });
                        $('#mapDiv').css({ 'position': 'fixed', 'top': '0px', 'left': '0px', 'height': '40px', 'width': width + 'px' });
                        $('#map').css({ 'position': 'fixed', 'top': '42px', 'left': (parseInt(nReportWidth) + 10) + 'px', 'width': width - (parseInt(nReportWidth) + 10) + 'px', 'height': (height - 50) + 'px' });
                    }

                }
            }

            resizeMap('0');
        }

        function showReport() {
            if (!($('#tabs-Report').is(':visible'))) {
                $('#tabs-Liveview').hide();
                $('#tab-Liveview').removeClass('clsMaptdActive');
                $('#divLiveView').hide();
                $('#tabs-Report').show();
                $('#tab-Report').addClass('clsMaptdActive');
            }
        }

        function showLiveview() {
            if (!($('#tabs-Liveview').is(':visible'))) {
                $('#tabs-Report').hide();
                $('#tab-Report').removeClass('clsMaptdActive');
                $('#tabs-Liveview').show();
                $('#tab-Liveview').addClass('clsMaptdActive');
                $('#divLiveView').show();
            }
        }

        function regularSize() {
            $('#dialogMonitorReport').css({ 'display': 'none' })
            $('#dialogStarReport').css({ 'display': 'none' })

            if ($('#dialogReport').is(":visible") && g_fullScreen == 0) {
                screenMode = 0;
                showHideReports(1, 0);
                loadFlooronScreenDiffer();
            } else {
                screenMode = 0;

                if (g_fullScreen === 0) {
                    g_mapWindowTop = $('#map').offset().top;
                    g_mapWindowLeft = $('#map').offset().left;

                    g_mapDivWindowTop = $('#mapDiv').offset().top;
                    g_mapDivWindowLeft = $('#mapDiv').offset().left;

                    if (g_designMode == 1) {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#divAddMonitors').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': '100px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 140 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });
                    }
                    else {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });
                    }

                } else {
                    if (g_designMode == 1) {
                        //g_mapDivWindowTop = $('#mapDiv').offset().top;
                        //g_mapDivWindowLeft = $('#mapDiv').offset().left;

                        $('#divAddMonitors').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': '100px' });
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 140 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });
                    }
                    else {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': g_mapWindowWidth + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });
                    }

                }

                resizeMap('0');
                $("#tabs-Liveview").css("height", parseInt($('#map').css('height').replace('px', '')) - 75 + 'px');
            }
        }

        function largeSize() {
            $('#dialogMonitorReport').css({ 'display': 'none' })
            $('#dialogStarReport').css({ 'display': 'none' })

            if (screenMode == 2) {
                screenMode = 0;
                regularSize();
                loadFlooronScreenDiffer();
            } else {
                if ($('#dialogReport').is(":visible")) {
                    screenMode = 1;
                    showHideReports(1, 0);
                    loadFlooronScreenDiffer();
                } else {
                    var width = $(document).width();
                    var height = $(document).height();

                    if (g_fullScreen === 0) {
                        g_mapWindowTop = $('#map').offset().top;
                        g_mapWindowLeft = $('#map').offset().left;

                        g_mapDivWindowTop = $('#mapDiv').offset().top;
                        g_mapDivWindowLeft = $('#mapDiv').offset().left;
                    }

                    var mapWidth = width - g_mapWindowLeft;
                    var mapHeight = height - g_mapWindowTop;

                    var mapDivWidth = width - g_mapDivWindowLeft;
                    var mapDivHeight = height - g_mapDivWindowTop;

                    screenMode = 1;

                    if (g_designMode == 1) {
                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': parseInt(mapDivWidth) - 15 + 'px' });

                        $('#divAddMonitors').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'height': '100px', 'width': parseInt(mapWidth) - 15 + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 140 + 'px', 'left': g_mapDivWindowLeft + 'px', 'height': parseInt(mapHeight) - 15 + 'px', 'width': parseInt(mapWidth) - 15 + 'px' });
                    }
                    else {

                        $('#mapDiv').css({ 'position': 'absolute', 'top': g_mapDivWindowTop, 'left': g_mapDivWindowLeft, 'height': '40px', 'width': parseInt(mapDivWidth) - 15 + 'px' });
                        $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'height': parseInt(mapHeight) - 15 + 'px', 'width': parseInt(mapWidth) - 15 + 'px' });
                    }

                    AdjustLoadingDiv();
                    resizeMap('0');
                    $("#tabs-Liveview").css("height", parseInt($('#map').css('height').replace('px', '')) - 75 + 'px');
                }
            }
        }

        function exitfullScreen() {
            if (g_fullScreen === 1) {
                g_fullScreen = 0;
                $.fullscreen.exit();
                screenMode = 0;
                if ($('#dialogReport').is(":visible")) {
                    showHideReports(1, 0);
                }
                return false;
            }
        }

        //Display Map from Device Details
        function DisplayMapfromDeviceDetails(isClick) {
            isButtonClicked = isClick;
            //document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').hide('slide', { direction: 'right' }, 100);
            $('#ctl00_ContentPlaceHolder1_divMap').show('slide', { direction: 'left' }, 700);
        }

        //Display Device Details from Map
        function loadDeviceDetailsInfoOnClick(siteid, devicetype, deviceid) {
            isButtonClicked = 1;

            location.href = "#divDeviceDetails";

            $('#ctl00_ContentPlaceHolder1_divMap').hide('slide', { direction: 'left' }, 100);
            $('#ctl00_ContentPlaceHolder1_divDeviceDetails').show('slide', { direction: 'right' }, 600);

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

            document.getElementById('divLoading_TagDetails').style.display = "";
            document.getElementById('divLoading_Graph').style.display = "";
            document.getElementById('divLoading_WifiDetails').style.display = "";

            document.getElementById("ctl00_ContentPlaceHolder1_trPaginationGraph").style.display = "none";
            document.getElementById('Div_MSStackedColumn2D').innerHTML = "";

            DeviceList(siteid, devicetype, deviceid);
        };

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

        function loadValuesintoDropdown(elem, selectValues) {
            $(elem).empty();
            $(elem)
                 .append($("<option></option>")
                 .attr("value", "0")
                 .text("Select"));
            $.each(selectValues, function (key, value) {
                $(elem)
                     .append($("<option></option>")
                     .attr("value", value)
                     .text(key));
            });
        }

        function snapToGrid() {
            if (g_SnapToGrid == 0)
                g_SnapToGrid = 1;
            else
                g_SnapToGrid = 0;
            showHideSnapGridLayer(g_SnapToGrid);
        }
    </script>
    <script type="text/javascript">
        var g_TagViewLoaded = false;
        var g_UserRole = 0;

        // INSTALLATION MODE ///////////////////////////////////////////////////////////////////////////////////////////////////

        function ClearTheEntries() {
            document.getElementById("txtMonitorId").value = "";
            document.getElementById("txtLocation").value = "";
            document.getElementById("txtNotes").value = "";
            document.getElementById("txtUnitName").value = "";
            document.getElementById("chkIsHallway").checked = false;
        }

        function ShowControlsForSelectedDevice() {
            setTimeout(function () { document.getElementById("txtMonitorId").focus(); }, 0);
            $("#btnDrawRoom").removeAttr('class').addClass('mapDrawRoom');
            $("#tblDimensions").hide();
            if (g_svgDType == 1) {
                document.getElementById("tdDeviceIdLable").innerHTML = "Monitor Id:";
                document.getElementById("tdPlaceDevice").style.visibility = "visible";
                document.getElementById("btnPlaceMonitor").style.visibility = "visible";
                document.getElementById("tdPlaceDevice").innerHTML = "Place Monitor";
                document.getElementById("tdDrawRoomText").innerHTML = "Draw Room";
                $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitor');
                document.getElementById("tdlocationLabel").style.visibility = "visible";
                document.getElementById("txtLocation").style.visibility = "visible";
                document.getElementById("tdNotesLabel").style.visibility = "visible";
                document.getElementById("tdUnitLabel").style.visibility = "visible";
                document.getElementById("txtNotes").style.visibility = "visible";
                document.getElementById("txtUnitName").style.visibility = "visible";
                document.getElementById("tdIsHallway").style.visibility = "visible";
                document.getElementById("tdDrawRoom").style.visibility = "visible";

            }
            else if (g_svgDType == 2) {
                document.getElementById("tdDeviceIdLable").innerHTML = "MAC Id:";
                document.getElementById("tdPlaceDevice").style.visibility = "visible";
                document.getElementById("btnPlaceMonitor").style.visibility = "visible";
                document.getElementById("tdPlaceDevice").innerHTML = "Place Star";
                $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateStar');
                document.getElementById("tdlocationLabel").style.visibility = "hidden";
                document.getElementById("txtLocation").style.visibility = "hidden";
                document.getElementById("tdNotesLabel").style.visibility = "hidden";
                document.getElementById("tdUnitLabel").style.visibility = "hidden";
                document.getElementById("txtNotes").style.visibility = "hidden";
                document.getElementById("txtUnitName").style.visibility = "hidden";
                document.getElementById("tdIsHallway").style.visibility = "hidden";
                document.getElementById("tdDrawRoom").style.visibility = "hidden";
            }
            else if (g_svgDType == 3) {
                document.getElementById("tdDeviceIdLable").innerHTML = "AP MAC Id:";
                document.getElementById("tdPlaceDevice").style.visibility = "visible";
                document.getElementById("btnPlaceMonitor").style.visibility = "visible";
                document.getElementById("tdPlaceDevice").innerHTML = "Place AP";
                $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateAccesspoint');
                document.getElementById("tdlocationLabel").style.visibility = "visible";
                document.getElementById("txtLocation").style.visibility = "visible";
                document.getElementById("tdNotesLabel").style.visibility = "visible";
                document.getElementById("tdUnitLabel").style.visibility = "visible";
                document.getElementById("txtNotes").style.visibility = "visible";
                document.getElementById("txtUnitName").style.visibility = "visible";
                document.getElementById("tdIsHallway").style.visibility = "hidden";
                document.getElementById("tdDrawRoom").style.visibility = "hidden";

            }
            else if (g_svgDType == 4) {
                document.getElementById("tdDeviceIdLable").innerHTML = "Zone Id:";
                document.getElementById("tdPlaceDevice").style.visibility = "hidden";
                document.getElementById("btnPlaceMonitor").style.visibility = "hidden";
                document.getElementById("tdlocationLabel").style.visibility = "visible";
                document.getElementById("txtLocation").style.visibility = "visible";
                document.getElementById("tdNotesLabel").style.visibility = "visible";
                document.getElementById("tdUnitLabel").style.visibility = "visible";
                document.getElementById("txtNotes").style.visibility = "visible";
                document.getElementById("txtUnitName").style.visibility = "visible";
                document.getElementById("tdIsHallway").style.visibility = "hidden";
                document.getElementById("tdDrawRoom").style.visibility = "visible";
                document.getElementById("tdDrawRoomText").innerHTML = "Draw Zone";
            }

            $("#btnDrawRoom").prop("disabled", false);
            $("#btnPlaceMonitor").prop("disabled", false);
        }

        function openSetupPanel() {
            g_svgDType = 1;
            ClearTheEntries();
            ShowControlsForSelectedDevice();

            document.getElementById("divAddMonitors").style.display = "";

            if (zoomify_width.length == 0) // no bg image uploaded
            {
                document.getElementById("trBgUploadRow").style.display = "";
                document.getElementById("trMonitorRow").style.display = "none";
            }
            else {
                document.getElementById("trBgUploadRow").style.display = "none";
                document.getElementById("trMonitorRow").style.display = "";
            }


            var width;
            var height;
            width = screen.width;
            height = screen.height;

            if (screenMode == 2)//fulscreen
            {
                $('#divAddMonitors').css({ 'position': 'absolute', 'top': '41px', 'left': '0px', 'width': width + 'px', 'height': '100px' });
                $('#map').css({ 'position': 'absolute', 'top': '142px', 'left': '0px', 'width': width + 'px', 'height': height - 60 + 'px' });
            }
            else if (screenMode == 1)//larger screen
            {
                var mapWidth = width - g_mapWindowLeft;
                var mapHeight = height - g_mapWindowTop;
                g_mapDivWindowTop = $('#mapDiv').offset().top;
                g_mapDivWindowLeft = $('#mapDiv').offset().left;

                $('#divAddMonitors').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'height': '100px', 'width': parseInt(mapWidth) - 31 + 'px' });
                $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 140 + 'px', 'left': g_mapDivWindowLeft + 'px', 'height': parseInt(mapHeight) + 'px', 'width': parseInt(mapWidth) - 31 + 'px' });
            }
            else if (screenMode == 0)//normal screen
            {
                g_mapDivWindowTop = $('#mapDiv').offset().top;
                g_mapDivWindowLeft = $('#mapDiv').offset().left;

                $('#divAddMonitors').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 40 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': '100px' });
                $('#map').css({ 'position': 'absolute', 'top': g_mapDivWindowTop + 140 + 'px', 'left': g_mapDivWindowLeft + 'px', 'width': g_mapWindowWidth + 'px', 'height': g_mapWindowHeight + 'px' });
            }
            resizeMap('0');
            //showAddDeviceInfo();
        }

        var GSiteId = "";
        this.onload = function () {

            g_MapView = enumMapView.Map;

            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;

            //Feature #22513 Grant Engineering and Support roles access to Enter Installation Mode screen
            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {
                document.getElementById("btnDesignMode").style.display = "";
            }
            else {
                document.getElementById("btnDesignMode").style.display = "none";
            }
            
            $('#btnTabMonitor').click(function () {
                g_svgDType = 1;
                g_dsvgId = 0;
                monitorX = -1;
                monitorY = -1;
                monitorW = -1;
                monitorH = -1;
                RoompolygonPoints = '';
                cancelClicked();
                document.getElementById("lblsaveMonitorResult").innerHTML = "";
                ClearTheEntries();
                ShowControlsForSelectedDevice();
            });

            $('#btnTabStar').click(function () {
                g_svgDType = 2;
                g_dsvgId = 0;
                monitorX = -1;
                monitorY = -1;
                monitorW = -1;
                monitorH = -1;
                RoompolygonPoints = '';
                cancelClicked();
                document.getElementById("lblsaveMonitorResult").innerHTML = "";
                ClearTheEntries();
                ShowControlsForSelectedDevice();
            });

            $('#btnTabAccessPoint').click(function () {
                g_svgDType = 3;
                g_dsvgId = 0;
                monitorX = -1;
                monitorY = -1;
                monitorW = -1;
                monitorH = -1;
                RoompolygonPoints = '';
                cancelClicked();
                document.getElementById("lblsaveMonitorResult").innerHTML = "";
                ClearTheEntries();
                ShowControlsForSelectedDevice();
            });

            $('#btnTabZone').click(function () {
                g_svgDType = 4;
                g_dsvgId = 0;
                monitorX = -1;
                monitorY = -1;
                monitorW = -1;
                monitorH = -1;
                RoompolygonPoints = '';
                cancelClicked();
                document.getElementById("lblsaveMonitorResult").innerHTML = "";
                ClearTheEntries();
                ShowControlsForSelectedDevice();
            });



            // check native support
            $('#support').text($.fullscreen.isNativelySupported() ? 'supports' : 'doesn\'t support');


            $('#btnDrawRoom').click(function () {
                $('#lblsaveMonitorResult').html("Click on the map, move mouse to draw room. Double click to complete the drawing.").addClass('clsHelpMsg');
                //DrawRoom();
                activatePolygonControl();
            });

            $('#btnPlaceMonitor').click(function () {
                if (RoompolygonPoints.length == 0 && g_svgDType == 1 && document.getElementById("chkIsHallway").checked == false) {
                    alert("Please draw the room for the monitor");
                    return;
                }
                polygonControl.deactivate();

                g_uMode = 1; //add
                if (g_svgDType == 1)
                    $('#lblsaveMonitorResult').html("Click on a position on the map to place the monitor").addClass('clsHelpMsg');
                else if (g_svgDType == 2)
                    $('#lblsaveMonitorResult').html("Click on a position on the map to place the star").addClass('clsHelpMsg');
                PlaceMonitor();
            });

            $('#btnDeleteDevice').click(function () {

                if (!selectedFeature) {
                    alert("Please select a monitor or star to delete")
                    return;
                }
                deleteEditlayerFeature();

            });



            $('#btnSaveMonitor').click(function () {
                var SiteId = g_MapSiteId;
                var FloorId = $('#selFloor').val();
                var MonitorId = document.getElementById("txtMonitorId").value;
                var Location = document.getElementById("txtLocation").value;
                var Notes = document.getElementById("txtNotes").value;
                var UnitName;
                /*if(document.getElementById("txtUnitName").value == "")
                UnitName = document.getElementById("txtinput").value;
                else
                UnitName = document.getElementById("txtUnitName").value;*/

                UnitName = document.getElementById("txtUnitName").value;
                var isHallway = "0";
                if (document.getElementById("chkIsHallway").checked == true)
                    isHallway = "1";

                //check the inputs
                if (MonitorId == "") {
                    if (g_svgDType == 1)
                        alert("Please provide the monitor id");
                    else if (g_svgDType == 3)
                        alert("Please provide the access point id");
                    else if (g_svgDType == 4)
                        alert("Please provide the zone id");
                    else
                        alert("Please provide the MAC id for star");
                    return;
                }
                else if ((g_svgDType == 1 || g_svgDType == 4) && allnumeric(document.getElementById("txtMonitorId")) == false) {
                    if (g_svgDType == 1)
                        alert("Please provide a valid monitor id");
                    else if (g_svgDType == 4)
                        alert("Please provide a valid zone id");
                    document.getElementById("txtMonitorId").focus();
                    return;
                }
                else if (Location == "" && g_svgDType == 1 && document.getElementById("chkIsHallway").checked == false) {
                    alert("Please provide the location or room name for the monitor");
                    return;
                }
                else if ((monitorX === -1 || monitorY === -1) && g_svgDType != 4) {
                    if (g_svgDType == 1 && document.getElementById("chkIsHallway").checked == false)
                        alert("Please place a monitor in the room drawn");
                    else if (g_svgDType == 1 && document.getElementById("chkIsHallway").checked == true)
                        alert("Please place a monitor");
                    else {
                        if (g_svgDType == 3)
                            alert("Please place an access point in the map");
                        else if (g_svgDType == 2)
                            alert("Please place a Star in the map");
                    }
                    return;
                }
                else if (RoompolygonPoints.length == 0 && g_dsvgId == 0 && g_svgDType == 1 && document.getElementById("chkIsHallway").checked == false) {
                    alert("Please draw the room for the monitor");
                    return;
                }
                else if (RoompolygonPoints.length == 0 && g_svgDType == 4) {
                    alert("Please draw the zone on the map");
                    return;
                }

                $("#tblDimensions").hide();
                $("#div_SaveMonitorLoader").show();

                /*if(isErrorResponse === 0)
                {
                var temproomY = roomY;
                roomY = convertToSvgYCoordinateForRoom(parseFloat(roomY),parseFloat(roomH));
                roomW = convertToSvgWidth(parseFloat(roomX),parseFloat(roomW));
                roomH = convertToSvgHeight(parseFloat(temproomY),parseFloat(roomH));
                
                
                monitorX = convertToSvgEcllipseXCoordinate(parseFloat(monitorX));
                monitorY = convertToSvgEcllipseYCoordinate(parseFloat(monitorY));
                monitorW = 15;
                monitorH = 15;
                }*/

                if (g_dsvgId > 0)
                    g_uMode = 2; //Update
                else
                    g_uMode = 1; //Add

                $('#lblsaveMonitorResult').html("").removeClass('clsHelpMsg');

                Xaxis = document.getElementById("lblMapX").innerHTML;
                Yaxis = document.getElementById("lblMapY").innerHTML;
                RoomXaxis = document.getElementById("lblRoomMapX").innerHTML;
                RoomYaxis = document.getElementById("lblRoomMapY").innerHTML;
                WidthFt = document.getElementById("lblWidth").innerHTML;
                LengthFt = document.getElementById("lblLength").innerHTML;

                saveMonitor(SiteId, FloorId, MonitorId, Location, Notes, isHallway, monitorX, monitorY, monitorW, monitorH, RoompolygonPoints, g_uMode, g_dsvgId, g_svgDType, g_oldDeviceId, UnitName, Xaxis, Yaxis, RoomXaxis, RoomYaxis, WidthFt, LengthFt);

            });

            $('#btnCancelMonitor').click(function () {

                $('#lblsaveMonitorResult').html("");
                // resizeMap('0');        

                resetSaveMonitor();
                // document.getElementById("divAddMonitors").style.display="none";
                g_dsvgId = 0;
                monitorX = -1;
                monitorY = -1;
                monitorW = -1;
                monitorH = -1;
                RoompolygonPoints = '';
                cancelClicked();

                //showAddDeviceInfo();
            });


            // INSTALATION MODE


            showHideMapDesignMode(g_designMode, 1);
            var hdSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;

            document.getElementById("ctl00_headerBanner_drpsitelist").value = hdSiteId;

            g_MapSiteId = hdSiteId;
            g_TagSiteId = hdSiteId;
            g_MapView = enumMapView.Map;

            g_mapWindowTop = $('#map').offset().top;
            g_mapWindowLeft = $('#map').offset().left;

            g_mapWindowHeight = '600';
            g_mapWindowWidth = '974'; $('#tdHeader').css('width');
            document.getElementById("div10hrTable").style.width = g_mapWindowWidth + 'px';

            g_mapDivWindowTop = $('#mapDiv').offset().top;
            g_mapDivWindowLeft = $('#mapDiv').offset().left;

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            loadMapListView(siteVal);

            document.getElementById("map").style.display = "none";
            document.getElementById("mapDiv").style.display = "none";

            document.getElementById("tdSelectFloor").style.display = "";

            //Configure View
            $('#aRoomMetaInfo').addClass('clsmenuselected');
            $('#aTagMetaInfo').addClass('clslink');
            $('#divRoomView').show();

            $('#txtTagIds').val('');
        }

        $(document).on('change', '#ctl00_headerBanner_drpsitelist', function () {

            document.getElementById("mapDiv").style.display = "none";

            showHideMapView();
            g_MapConfigureViewLoaded = false;

            document.getElementById("tdSelectedFloor").style.display = "none";
            document.getElementById("tdSelectedFloor").innerHTML = "";

            $("#tdSelectFloor").removeClass('clsMapErrorTxt').addClass('clsLALabelGrey');

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            $("#selCampus").empty();
            $("#selBuilding").empty();
            $("#selFloor").empty();

            $('#tblMapView').empty();
            $('#tblMapView_Building').empty();
            $('#tblMapView_Floor').empty();
            $('#tblMapView_Unit').empty();
            $('#tblMapView_Room').empty();

            $('#map').html('');

            $('#tdBuildingHeader').hide();
            $('#tdFloorHeader').hide();
            $('#tdUnitHeader').hide();
            $('#tdRoomHeader').hide();

            g_MapId = 0;
            g_MapBuildingId = 0;
            g_MapFloorId = 0;
            g_MapUnitId = 0;
            g_MapView = enumMapView.Map;
            g_MapType = enumMapType.Campus;

            $('#tdSelectFloor').html('Loading...');

            $("#ifrmUpload").attr("src", "uploadFile.aspx?SiteId=" + siteVal);

            g_MapSiteId = siteVal;

            //Close reports panel
            showHideReports(0, 0);
            $('#mapDiv').css({ 'position': 'absolute', 'left': '278.5px', 'height': '40px', 'width': '974px' });
            $('#map').css({ 'position': 'absolute', 'left': '278.5' + 'px', 'width': '974px', 'height': '600' + 'px' });
            screenMode = 0;
            //closeSocketConForLiveUpdates();
            //self.setTimeout("createSocketConForLiveUpdates()",500);
            clearLiveUpdates();
            loadMapListView(g_MapSiteId);
        });

        function loadRoomView() {
            if ($('#divRoomView').is(":hidden")) {
                $('#aRoomMetaInfo').removeClass('clslink').addClass('clsmenuselected');
                $('#aTagMetaInfo').removeClass('clsmenuselected').addClass('clslink');
                $('#divTagView').hide();
                $('#divRoomView').show();
            }
        }

        function loadTagView() {
            if ($('#divTagView').is(":hidden")) {
                $('#aRoomMetaInfo').removeClass('clsmenuselected').addClass('clslink');
                $('#aTagMetaInfo').removeClass('clslink').addClass('clsmenuselected');
                $('#divRoomView').hide();
                $('#divTagView').show();

                if (g_TagViewLoaded === false) {
                    g_TagViewLoaded = true;
                    CurrPg = 1;
                    g_TMISort = "TagId desc";
                    g_TMISortColumn = "TagId";
                    g_TMISortOrder = " desc";
                    g_TMISortImg = "<image src='Images/downarrow.png' valign='middle' />";

                    g_TMIPgSize = 25;
                    $("#tdLive25").removeClass("clsPageSize").addClass("clsPageSizeCurrent");
                    $("#ctl00_ContentPlaceHolder1_txtPageNo_TagMetaView").val(1);

                    tagListView(CurrPg, g_TMISort);
                }
            }
        }

        function showTipMap(text, lf, tp) {
            var elementRef = document.getElementById('tooltipmap');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltipmap").css({ left: tempX, top: tempY, 'z-index': 5001 });
        }

        function hideTipMap() {
            var elementRef = document.getElementById('tooltipmap');
            elementRef.style.display = 'none';
        }

        function btnMapOut(Obj) {
            var clsname = Obj.className;
            hideTipMap(Obj);
        }

        function btnMapHover(Obj) {
            var clsname = Obj.className;
            setTipMap(Obj);
        }

        function setTipMap(Obj) {
            var id = Obj.id;
            var lf = findPosX(Obj);
            var tp = findPosY(Obj);

            if (id.length != "") {
                var txt = "";

                if (id == "btnRegular") {
                    lf += -25;
                    tp += 40;
                    txt = "Regular Size";
                }
                else if (id == "btnLarge") {
                    lf += -25;
                    tp += 40;
                    txt = "Large Size";
                }
                else if (id == "btnFullScreen") {
                    lf += -25;
                    tp += 40;
                    txt = "Full Screen";
                }
                else if (id == "btnSearchDevice") {
                    lf += -25;
                    tp += 32;
                    txt = "Search Device";
                }
                else if (id == "btnSearchCancel") {
                    lf += -10;
                    tp += 32;
                    txt = "Cancel";
                }
                else if (id == "btnShowTag") {
                    lf += -20;
                    tp += 40;
                    if (g_isShowTags)
                        txt = "Hide Tags";
                    else
                        txt = "Show Tags";

                }
                else if (id == "btnMonitorShow") {
                    lf += -10;
                    tp += 40;
                    txt = "Monitors";
                }
                else if (id == "btnStarShow") {
                    lf += -10;
                    tp += 40;
                    txt = "Stars";
                }
                else if (id == "btnReports") {
                    lf += -10;
                    tp += 40;
                    txt = "Reports";
                }
                else if (id == "btnAddMonitorOnMap") {
                    lf += -20;
                    tp += 40;
                    txt = "Add Monitor";
                }
                else if (id == "btnDrawRoom") {
                    lf += -20;
                    tp += 40;
                    txt = "Draw Room";
                }
                else if (id == "btnPlaceMonitor") {
                    lf += -20;
                    tp += 40;
                    if (g_svgDType == 1)
                        txt = "Place Monitor";
                    else if (g_svgDType == 2)
                        txt = "Place Star";
                }
                else if (id == "btnCancelMonitor") {
                    lf += -20;
                    tp += 40;
                    txt = "Cancel";
                }
                else if (id == "btnSnapToGrid") {
                    lf += -20;
                    tp += 40;
                    txt = "Snap To Grid";
                }

            }

            if (txt != "") {
                showTipMap(txt, lf, tp);
            }
        }

        function showHideMonitorReport() {

            btnMapOut(document.getElementById("dialogMonitorReport"));
            if ($('#dialogMonitorReport').is(':visible')) {
                $('#dialogMonitorReport').hide(300);
            } else {
                if ($('#dialogStarReport').is(':visible'))
                    $('#dialogStarReport').hide(300);
                $('#dialogMonitorReport').show(300);
                $('#dialogMonitorReport').css({ 'position': 'absolute', 'border': 'solid 4px #013D69', 'top': parseInt($('#btnMonitorShow').offset().top) + 35, 'left': $('#btnMonitorShow').offset().left, 'z-index': 1005, 'box-shadow': '8px 8px 8px #777575' })
            }
        }

        function showHideStarReport() {
            btnMapOut(document.getElementById("dialogStarReport"));
            if ($('#dialogStarReport').is(':visible')) {
                $('#dialogStarReport').hide(300);
            } else {
                if ($('#dialogMonitorReport').is(':visible'))
                    $('#dialogMonitorReport').hide(300);
                $('#dialogStarReport').show(300);
                $('#dialogStarReport').css({ 'position': 'absolute', 'border': 'solid 4px #013D69', 'top': parseInt($('#btnStarShow').offset().top) + 35, 'left': $('#btnStarShow').offset().left, 'z-index': 1005, 'box-shadow': '8px 8px 8px #777575' })
            }
        }

        function deleteSelectedDevice() {

            if (g_dsvgId > 0)
                g_uMode = 3; //Update

            var SiteId = g_MapSiteId;
            var FloorId = $('#selFloor').val();
            var MonitorId = document.getElementById("txtMonitorId").value;
            var Location = document.getElementById("txtLocation").value;
            var Notes = document.getElementById("txtNotes").value;
            var UnitName = document.getElementById("txtUnitName").value;
            var isHallway = "0";
            if (document.getElementById("chkIsHallway").checked == true)
                isHallway = "1";

            $('#lblsaveMonitorResult').html("").removeClass('clsHelpMsg');

            saveMonitor(SiteId, FloorId, MonitorId, Location, Notes, isHallway, monitorX, monitorY, monitorW, monitorH, RoompolygonPoints, g_uMode, g_dsvgId, g_svgDType, g_oldDeviceId, UnitName);
        }

        function allnumeric(inputtxt) {
            var numbers = /^[0-9]+$/;
            if (inputtxt.value.match(numbers)) {
                return true;
            }
            else {
                return false;
            }
        }
        function ReturntoHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
            location.href = "Home.aspx?sid=" + GSiteId;
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 17px;">
        <tr style="height: 10px;">
        </tr>
        <tr>
            <td align="left">
                <input type="hidden" id="hdSiteId" runat="server" />
                <input type="hidden" id="hid_userrole" runat="server" />
                <!-- TOOL TIP-->
                <div id="tooltipmap" class="tool3" style="display: none;">
                </div>
                <div id="divMap" runat="server" style="top: auto; left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 90%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr style="height: 20px;">
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tdHeader">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="ReturntoHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <asp:Label ID="lblSiteName_Map" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Map View
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        <img src="Images/configure.png" width="24px" height="24px" />
                                                        <br />
                                                        <label class="clsLALabel" onclick="showHideConfigureView();" style="cursor: pointer;">
                                                            Configure</label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="4" style="padding-left: 20px;">
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
                            <td align="center" style="width: 100%;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 20px;
                                    padding-right: 10px;">
                                    <tr>
                                        <td class="clsLALabel" align="left" style="width: 220px;">
                                            Campus&nbsp;:&nbsp;
                                            <select id="selCampus" style="width: 150px;">
                                            </select>
                                        </td>
                                        <td class="clsLALabel" align="center" style="width: 220px;">
                                            Building&nbsp;:&nbsp;
                                            <select id="selBuilding" style="width: 150px;">
                                            </select>
                                        </td>
                                        <td class="clsLALabel" align="right" style="width: 220px;">
                                            Floor&nbsp;:&nbsp;
                                            <select id="selFloor" style="width: 150px;">
                                            </select>
                                        </td>
                                        <td style="width: 10px;">
                                        </td>
                                        <td align="left" valign="middle" class="clsMapSearchDiv" style="padding-left: 5px;
                                            height: 30px;">
                                            Search&nbsp;:&nbsp;
                                            <input type="text" id="txtSearchDevices" placeholder="Device Id" style="width: 145px;
                                                padding: 2px;" />
                                            <input type="button" id="btnSearch" class="btnGO" value="GO" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                        </tr>
                        <!-- MAP -->
                        <tr>
                            <td valign="middle">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; padding-left: 20px;">
                                    <tr>
                                        <td id="tdSelectedFloor" class='SHeader1' style="width: 70%" valign="middle">
                                        </td>
                                        <td align="center" class="clsLALabelGrey" style="width: 30%" valign="middle">
                                            <div style="position: relative; display: none;" id="divLoading_MapFloor">
                                                <img src="Images/377.GIF" alt="loading...." style="height: 19px; width: 19px;" /></div>
                                            &nbsp;<span id="tdSelectFloor" class="clsLALabelGrey">Loading... </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px;">
                                <div id="divLoadingMap" style="position: absolute; float: inherit; width: 100%; height: 60px;
                                    display: none; z-index: 1;">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="middle" align="center">
                                                <div class="divShadow">
                                                    <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                    <div id="lblLoadingMap" class="clsLALabelMapLocading">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="mapWindow">
                                    <div id="mapDiv" style="display: none;">
                                        <table style="width: 100.3%;" cellpadding="0" cellspacing="0" border="0">
                                            <tr style="height: 40px;">
                                                <td valign="top">
                                                    <table style="width: 100%" class="clsMapSearchDiv1" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td style="padding-left: 3px;" id="shSearch">
                                                                <input type="button" id="btnReports" class="mapShReports" />
                                                            </td>
                                                            <td style="display: none;">
                                                                <input type="text" id="txtSearchDevice" placeholder="Search Device in this Floor"
                                                                    style="width: 160px; padding: 2px;" />
                                                            </td>
                                                            <td style="display: none;">
                                                                <input type="button" id="btnSearchDevice" class="mapSearch" onmouseout="btnMapOut(this);"
                                                                    onmouseover="btnMapHover(this);" />
                                                            </td>
                                                            <td style="display: none;">
                                                                <input type="button" id="btnSearchCancel" class="mapSearchCancel" onmouseout="btnMapOut(this);"
                                                                    onmouseover="btnMapHover(this);" />
                                                            </td>
                                                            <td style="width: 3%;" id="shtdBeforeDesignMode">
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <input style="display: none;" type="button" id="btnDesignMode" class="mapShDesignMode"
                                                                    onclick="MapDesignMode();" onmouseout="btnMapOut(this);" onmouseover="btnMapHover(this);" />
                                                            </td>
                                                            <td style="width: 55%;">
                                                            </td>
                                                            <td id="shReports">
                                                                <table cellpadding="1" cellspacing="1" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnMonitorShow" class="mapShMonitor" onclick="showHideMonitorReport();"
                                                                                onmouseout="btnMapOut(this);" onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" id="btnStarShow" class="mapShStars" onclick="showHideStarReport();"
                                                                                onmouseout="btnMapOut(this);" onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" id="btnShowTag" class="mapNoShowTags" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 50px;">
                                                            </td>
                                                            <td align="right">
                                                                <table cellpadding="1" cellspacing="1" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnRegular" class="mapRegular" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" id="btnLarge" class="mapLarge" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" id="btnFullScreen" class="mapFullscreen" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divAddMonitors" style="display: none;" class="divAddMonitor">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%;">
                                            <tr id="trBgUploadRow" style="display: none;">
                                                <td align="center">
                                                    <table cellpadding="2" cellspacing="2" border="0" style="width: 100%;">
                                                        <tr style="height: 100px;">
                                                            <td valign="middle" style="width: 45%" align="right" style="padding-right: 20px;">
                                                                <img src="images/imgUploadbgforMap.png" width="60px" height="60px" />
                                                            </td>
                                                            <td valign="middle" style="width: 55%">
                                                                <input type="button" id="btnFloorBgUploadForSetup" class="clsuploadFloorPlan" value="Upload Floor Plan Image" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trMonitorRow">
                                                <td style="background-color: #BABFC6; width: 150px; border-right: 1px solid #000000;">
                                                    <table cellpadding="2" cellspacing="2" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <img src="images/map/mapmonitors.png" width="18px" height="18px" />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnTabMonitor" class="clsMapDeviceAdd" value="Add Monitor" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <img src="images/map/mapStars.png" width="17px" height="17px" />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnTabStar" class="clsMapDeviceAdd" value="Add Star" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <img src="images/map/mapAccesspoint.png" width="16px" height="16px" />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnTabAccessPoint" class="clsMapDeviceAdd" value="Add Access Point" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <img src="images/map/imgZone.png" width="18px" height="18px" />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnTabZone" class="clsMapDeviceAdd" value="Add Zone" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="2" cellspacing="2" border="0">
                                                                    <tr>
                                                                        <td class="clsLALabel" id="tdDeviceIdLable" style="width: 100px;">
                                                                            Monitor Id:
                                                                        </td>
                                                                        <td>
                                                                            <input type="text" id="txtMonitorId" style="width: 250px;" tabindex="1" />
                                                                        </td>
                                                                        <td class="clsLALabel" id="tdlocationLabel" style="width: 250px;">
                                                                            Location/Room Name:
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <input type="text" id="txtLocation" style="width: 230px;" tabindex="2" />
                                                                        </td>
                                                                        <td style="width: 100px;" align="center">
                                                                            <input type="button" tabindex="8" id="btnSaveMonitor" class="clsbtn" value="Save" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="clsLALabel" id="tdUnitLabel">
                                                                            Unit Name:
                                                                        </td>
                                                                        <td class="ui-widget">
                                                                            <input id="txtUnitName" style="width: 250px;" tabindex="3" />
                                                                        </td>
                                                                        <td class="clsLALabel" id="tdIsHallway">
                                                                            <input type="checkbox" id="chkIsHallway" name="isHallway" value="1" tabindex="4" />
                                                                            Is Group
                                                                        </td>
                                                                        <td id="tdDrawRoom" valign="middle">
                                                                            <input type="button" id="btnDrawRoom" tabindex="5" class="mapDrawRoom" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" /><span class="clsLALabel" style="padding-left: 4px;"
                                                                                    id="tdDrawRoomText">Draw Room</span>
                                                                        </td>
                                                                        <td>
                                                                            <input type="button" id="btnPlaceMonitor" tabindex="6" class="mapLocateMonitor" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" /><span class="clsLALabel" style="padding-left: 4px;"
                                                                                    id="tdPlaceDevice">Place Monitor</span>
                                                                        </td>
                                                                        <td align="center">
                                                                            <input type="button" id="btnDeleteDevice" tabindex="9" class="clsbtn" value="Delete" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="clsLALabel" id="tdNotesLabel">
                                                                            Notes:
                                                                        </td>
                                                                        <td>
                                                                            <input type="text" id="txtNotes" style="width: 250px;" tabindex="7" />
                                                                        </td>
                                                                        <td align="center" colspan="3">
                                                                            <div style="display: none;" id="div_SaveMonitorLoader">
                                                                                <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                                            </div>
                                                                            <label id="lblsaveMonitorResult" style="display: none;">
                                                                            </label>
                                                                            <table cellpadding="1px" cellspacing="1px" border="0" width="100%" id="tblDimensions"
                                                                                style="display: none;">
                                                                                <tr>
                                                                                    <td class="clsLALabel">
                                                                                        X&nbsp;:<label id="lblMapX" style="padding-left: 2px; color: #064EBA; display: none;"></label><label
                                                                                            id="lblRoomMapX" style="padding-left: 2px; color: #064EBA; display: none;"></label>
                                                                                    </td>
                                                                                    <td class="clsLALabel">
                                                                                        Y&nbsp;:<label id="lblMapY" style="padding-left: 2px; color: #064EBA; display: none;"></label><label
                                                                                            id="lblRoomMapY" style="padding-left: 2px; color: #064EBA; display: none;"></label>
                                                                                    </td>
                                                                                    <td class="clsLALabel" id="tdlblWidth">
                                                                                        Width&nbsp;:<label id="lblWidth" style="padding-left: 2px; color: #064EBA;"></label>
                                                                                    </td>
                                                                                    <td class="clsLALabel" id="tdlblLength">
                                                                                        Length&nbsp;:<label id="lblLength" style="padding-left: 2px; color: #064EBA;"></label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td align="center">
                                                                            <input type="button" id="btnSnapToGrid" tabindex="10" class="mapSnapToGrid" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" onclick="snapToGrid();" />&nbsp;&nbsp;
                                                                            <input type="button" id="btnCancelMonitor" tabindex="11" class="mapCancel" onmouseout="btnMapOut(this);"
                                                                                onmouseover="btnMapHover(this);" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <!-- MONITOR REPORT -->
                                    <div id="dialogMonitorReport" style="display: none; width: 200px; height: 100px;
                                        background-color: #fff;">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="clsLALabel" style="padding: 4px;">
                                                    <input type="checkbox" id="chkPagingFrequency" /><span valign="middle">&nbsp;Paging&nbsp;Frequency</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 5px;">
                                            </tr>
                                            <tr>
                                                <td class="clsLALabel" style="padding: 4px;">
                                                    <input type="checkbox" id="chkVirtualWalls" /><span valign="middle">&nbsp;Virtual&nbsp;Walls</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <!-- STAR REPORT -->
                                    <div id="dialogStarReport" style="display: none; width: 200px; height: 100px; background-color: #fff;">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="clsLALabel" style="padding: 4px;">
                                                    <input type="checkbox" id="chkDeviceDensity" /><span valign="top">&nbsp;Star&nbsp;Density</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 5px;">
                                            </tr>
                                            <tr>
                                                <td class="clsLALabel" style="padding: 4px;">
                                                    <input type="checkbox" id="chkHeatMaps" /><span valign="top">&nbsp;Heat&nbsp;Maps</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="dialogReport" style="display: none; width: 250px; background-color: #E7ECF0;
                                        border: solid 4px #013D69;">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="padding-left: 5px; padding-right: 5px;">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 5px;">
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <div id="tabsMap">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td id="tab-Report" onclick="showReport();" class="clsMaptdActive">
                                                                                Reports
                                                                            </td>
                                                                            <td style="width: 4px; border: none;">
                                                                            </td>
                                                                            <td id="tab-Liveview" onclick="showLiveview();">
                                                                                Live Updates
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="tabs-Report">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr style="height: 5px;">
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 240px;">
                                                                                <select id="selDeviceType" style="width: 100%;">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 240px;">
                                                                                <select id="selFilterType" style="display: none; width: 100%;">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 240px;">
                                                                                <select id="selCndType" style="display: none; width: 100%;">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 240px;">
                                                                                <select id="selFilterFloor" style="display: none; width: 100%;">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 240px;">
                                                                                <input type="text" id="txtFilter1" style="display: none; width: 98%;" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 240px;">
                                                                                <input type="text" id="txtFilter2" style="display: none; width: 98%;" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 5px;">
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100%;" align="left">
                                                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <input type="button" id="btnGo" value="GO" class="btnGO" onclick="ReportFloorView();" />
                                                                                        </td>
                                                                                        <td style="width: 10px;">
                                                                                        </td>
                                                                                        <td align="left" style="width: 60%;">
                                                                                            <div style="display: none;" id="divLoading_FloorView">
                                                                                                <img src="Images/712_1.GIF" alt="loading...." style="height: 20px; width: 20px;" />
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="height: 20px;">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                <tr style="height: 5px;">
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td id="tdResult" class="clsLALabelMapReport" align="center" style="display: none;">
                                                                                                        Results
                                                                                                    </td>
                                                                                                    <td id="tdCSV" align="left" class="clsLALabelMapReport" style="display: none; width: 25px;">
                                                                                                        <img id="exportfloorcsv" src="Images/csv.png" alt="upload CSV" height="16" width="16"
                                                                                                            style="cursor: pointer;" onclick="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="height: 5px;">
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <div id="divFloorview" style="overflow-x: hidden; overflow-y: auto;">
                                                                                    <table id="tblFloorView" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div style="height: 8px;">
                                                                </div>
                                                                <div id="divLiveView" style="display: none; overflow-y: auto; text-overflow: ellipsis;
                                                                    width: 240px;">
                                                                    <input type="text" id="txtliveFilters" placeholder="Filter Tags" style="height: 20px;
                                                                        width: 180px; padding-left: 2px;" />&nbsp;<input type="button" id="btnFilterLive"
                                                                            class="btnGO" value="GO" onclick="setliveDataFilterArr();" />
                                                                    <div style="height: 5px;">
                                                                    </div>
                                                                    <div id="tabs-Liveview" style="display: none; overflow-y: auto; text-overflow: ellipsis;
                                                                        width: 240px;">
                                                                        <div style="height: 5px;">
                                                                        </div>
                                                                        <table id="tblLiveUpdate" border="0" cellpadding="5" cellspacing="0" width="223px;">
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="map" style="width: 100%; height: 600px; display: none;" class="mapView">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- MAP VIEW -->
                <div id="divMapView" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <div id="divCommands" style="position: absolute; vertical-align: top; width: 100px;
                        height: 20px; display: none; padding: 2px;">
                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr style="height: 20px;">
                                <td align="left" style="padding-left: 5px;">
                                    <img src="Images/upload.png" alt="Upload" height="20px" width="20px" id="imgUploadCSV"
                                        style="cursor: pointer;" />
                                    <img src="Images/svgUpload.png" alt="Upload" height="14px" width="20px" id="imgUpload"
                                        style="cursor: pointer;" />
                                    <img src="Images/edit.png" alt="Edit" height="12px" width="12px" id="imgEdit" style="cursor: pointer;" />
                                    <img src="Images/delete.png" alt="Delete" height="12px" width="12px" id="imgDelete"
                                        style="cursor: pointer;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 90%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr style="height: 20px;">
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="ReturntoHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <asp:Label ID="lblSiteName_MapView" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                Map Configure
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        <img src="Images/map.png" width="24px" height="24px" />
                                                        <br />
                                                        <label class="clsLALabel" onclick="showHideMapView();" style="cursor: pointer;">
                                                            Map View</label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="4" style="padding-left: 20px;">
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
                            <td align="right">
                                <div class="navigation">
                                    <ul>
                                        <li><a href="#" id="aRoomMetaInfo" onclick="loadRoomView();">Room Meta Info</a></li>
                                        <li style="padding-left: 15px;"><a href="#" id="aTagMetaInfo" onclick="loadTagView();">
                                            Tag Meta Info</a></li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 5px;">
                        </tr>
                        <tr>
                            <td>
                                <!-- ROOM META INFO -->
                                <div id="divRoomView" style="display: none; padding-left: 20px;">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td class="subHeader_black">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="width: 100px;">
                                                            Campus&nbsp;<span id="spnCampusCount"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="3" cellspacing="3" width="100%" id="tblMapView" class="mapViewSections">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                        </tr>
                                        <tr>
                                            <td class="subHeader_black">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td id="tdBuildingHeader" style="width: 100px;">
                                                            Buildings&nbsp;<span id="spnBuildingCount"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="3" cellspacing="3" width="100%" id="tblMapView_Building"
                                                    class="mapViewSections">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                        </tr>
                                        <tr>
                                            <td class="subHeader_black">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td id="tdFloorHeader" style="width: 100px;">
                                                            Floors&nbsp;<span id="spnFloorCount"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="3" cellspacing="3" width="100%" id="tblMapView_Floor"
                                                    class="mapViewSections">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                        </tr>
                                        <tr>
                                            <td class="subHeader_black">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td id="tdUnitHeader" style="width: 100px;">
                                                            Units&nbsp;<span id="spnUnitCount"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="3" cellspacing="3" width="100%" id="tblMapView_Unit"
                                                    class="mapViewSections">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                        </tr>
                                        <tr>
                                            <td class="subHeader_black">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td id="tdRoomHeader" style="width: 100px;">
                                                            Rooms&nbsp;<span id="spnRoomCount"></span>
                                                        </td>
                                                        <td align="right">
                                                            <div style="display: none;" id="divLoading_RoomView">
                                                                <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                            </div>
                                                        </td>
                                                        <td align="right" style="display: none">
                                                            <!-- <input type="button" id="btnAddRoom" class="clsExportExcel" value="Update Rooms" />-->
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblMapView_Room">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                        </tr>
                                    </table>
                                </div>
                                <!-- TAG META INFO -->
                                <div id="divTagView" style="display: none; padding-left: 20px;">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr style="height: 30px;">
                                            <td>
                                                <input type="button" id="btnAddTag" class="clsAddTagMetaInfo" value="Add Tag Meta Info" />
                                            </td>
                                        </tr>
                                        <tr style="height: 5px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr style="background-color: #EEE; border: solid 1px #666; height: 40px;">
                                                        <td class="clsLALabel" style="padding-left: 6px">
                                                            TagIds&nbsp;:&nbsp;
                                                            <input type="text" id="txtTagIds" style="width: 400px;" />
                                                        </td>
                                                        <td align="right" style="width: 120px; padding-right: 6px">
                                                            <input type="button" id="btnShowTagIds" class="clsExportExcel" value="Show" style="width: 100px;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px;">
                                        </tr>
                                        <!-- PREVIOUS/NEXT -->
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td align="right">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr style="height: 40px;">
                                                                    <td class="txttotalpage" style="width: 100px;" valign="middle">
                                                                        <asp:Label ID="lblCount_TagMetaView" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 180px;" align="center">
                                                                        <div style="display: none;" id="divLoading_TagView">
                                                                            <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                                        </div>
                                                                    </td>
                                                                    <td align="right" style="width: 60px;">
                                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td id="tdLive25" class="clsPageSize" onclick="pageSizeTMI(25);">
                                                                                    25
                                                                                </td>
                                                                                <td id="tdLive50" class="clsPageSize" onclick="pageSizeTMI(50);">
                                                                                    50
                                                                                </td>
                                                                                <td id="tdLive100" class="clsPageSize" onclick="pageSizeTMI(100);">
                                                                                    100
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td class="clsTableTitleText" align="right" style="width: 160px;">
                                                                        <input type="button" id="btnPrev_TagMetaView" class="clsPrev" title="Previous" onclick="TagMetaPgView(3);" />
                                                                        <asp:Label ID="Label4" runat="server" CssClass="clsCntrlTxt"> Page </asp:Label>
                                                                        <input id="txtPageNo_TagMetaView" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                                            type="text" size="1" maxlength="10" runat="server" name="txtPageNo" value="1" />
                                                                        <asp:Label ID="lblPgCnt_TagMetaView" runat="server" CssClass="clsCntrlTxt">&nbsp;</asp:Label>&nbsp;
                                                                        <input type="button" id="btnGo_TagMetaView" class="btnGO" value="Go" onclick="TagMetaPgView(1);" />&nbsp;&nbsp;
                                                                        <input type="button" id="btnNext_TagMetaView" class="clsNext" title="Next" onclick="TagMetaPgView(2);" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblTagMetaInfo">
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- ADD MAP VIEW -->
                <div id="dialog-Map" title="" style="display: none;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr align="left">
                            <td class="clsLALabel" style="width: 80px;" align="right">
                                Name&nbsp;:&nbsp;
                            </td>
                            <td style="width: 320px;" align="left">
                                <input type="text" id="txtMapName" style="width: 300px;" class="text ui-widget-content ui-corner-all" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr align="left">
                            <td class="clsLALabel" style="width: 80px;" align="right">
                                Description&nbsp;:&nbsp;
                            </td>
                            <td style="width: 320px;" align="left">
                                <input type="text" id="txtMapDescription" style="width: 300px;" class="text ui-widget-content ui-corner-all" />
                            </td>
                        </tr>
                        <tr align="left" id="trSqFt" style="display: none; height: 45px;">
                            <td class="clsLALabel" style="width: 80px;" align="right">
                                Width&nbsp;:&nbsp;
                            </td>
                            <td style="width: 320px;" align="left">
                                <input type="text" placeholder="Feet" id="txtWidthInFt" style="width: 105px;" class="text ui-widget-content ui-corner-all" />
                                <span class="clsLALabel" style="width: 80px; padding-left: 20px;">Length&nbsp;:&nbsp;</span><input
                                    type="text" id="txtSqft" style="width: 105px;" placeholder="Feet" class="text ui-widget-content ui-corner-all" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <input type="button" id="btnAddMap" class="clsExportExcel" value=" Add " />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <label id="lblAddMapMsg">
                                </label>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none;" id="divLoading_MapView">
                        <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                    </div>
                </div>
                <!-- DEVICE DETAILS PAGE-->
                <div id="divDeviceDetails" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellspacing="0" cellpadding="2" border="0" style="width: 90%;">
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
                                                        <a href="#" onclick="DisplayMapfromDeviceDetails(1);">
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
                                    <tr style="height: 20px;">
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left">
                                            <table id="Table1" runat="server" cellspacing="0" cellpadding="0" border="0" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <table cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td style="height: 10px;" class="subHeader_black">
                                                                    Paging and Location Data&nbsp;:&nbsp;<asp:Label ID="lblPagingDataDateFrom" runat="Server"></asp:Label>
                                                                    <asp:Label ID="lblPagingDataDateTo" runat="Server"></asp:Label>
                                                                    &nbsp;<asp:Label ID="lblPagingDataNoOfDays" runat="Server"></asp:Label>
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
                                                    <td align="center">
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
                                                <tr id="tr3" runat="server">
                                                    <td align='center' id="tdTrendGraph" style="width: 5%; height: 20px; text-align: left;">
                                                        <div style="position: relative; display: none; width: 100%; left: 350px;" id="divLoading_Graph">
                                                            <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                                        </div>
                                                        <center>
                                                            <div id="Div_MSStackedColumn2D" style="height: 480px;">
                                                            </div>
                                                        </center>
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
                                    <tr>
                                        <td valign="top" style="width: 100%">
                                            <div style="position: relative; display: none; width: 100%; left: 350px;" id="divLoading_WifiDetails">
                                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                                            </div>
                                            <div style="overflow-x: auto; overflow-y: hidden;" id="div10hrTable">
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
                <!-- UPLOAD VIEW -->
                <div id="dialog-UploadFile" title="Upload Floor SVG & BG File" style="display: none;">
                    <iframe id="ifrmUpload" style="border: none; height: 300px; width: 460px;"></iframe>
                </div>
                <!-- UPLOAD CSV VIEW -->
                <div id="dialog-UploadFileCSV" title="Upload Infrastructure CSV File" style="display: none;">
                    <iframe id="ifrmUploadCSV" style="border: none; height: 280px; width: 460px;"></iframe>
                </div>
                <!-- UPLOAD ROOM VIEW -->
                <div id="dialog-Room" title="Upload Room CSV File" style="display: none;">
                    <iframe id="ifrmRoomUpload" style="border: none; height: 280px; width: 460px;"></iframe>
                </div>
                <!-- UPLOAD TAG VIEW -->
                <div id="dialog-Tag" title="Upload Tag CSV File" style="display: none;">
                    <iframe id="ifrmTagUpload" style="border: none; height: 280px; width: 460px;"></iframe>
                </div>
            </td>
        </tr>
    </table>
    <!-- LIVE DATA UPDATE -->
    <!-- <script type="text/javascript">
    var socket;
    
    function createSocketConForLiveUpdates()
    {
        
        socket = io.connect('https://gms.centrak.com:3000/',{ secure: true,'forceNew':true ,query: "registrationKey=3A3CD974A53E49ba&siteid=" +  g_MapSiteId});

          socket.on('connect', function(data){
            setStatus('connected');
            socket.emit('subscribe', {channel:'CenTrakData'});
          });

      socket.on('reconnecting', function(data){
        setStatus('reconnecting');
      });

      socket.on('message', function (data) {
        console.log('received a message: ', data);
        var jObj = jQuery.parseJSON(data);    
         if(!jObj)
            return;
         var currentRoom = getRoomNameByMonitorId(jObj.CenTrakEvent.monitorid);
         var lastRoom = getRoomNameByMonitorId(jObj.CenTrakEvent.last_monitorid);
         if(currentRoom == "Unknown" && jObj.CenTrakEvent.evt_type != 4)
             return;   
         
         if([tblLiveUpdateMsgArr.length] >= nLiveViewRowCount)
                 tblLiveUpdateMsgArr.splice(0,1);
             
         tblLiveUpdateMsgArr[tblLiveUpdateMsgArr.length] = jObj;
            
        ShowLog(jObj,currentRoom,lastRoom,1);
        //addMessage(data);
      });
    }
    
   
    
    
     
      function closeSocketConForLiveUpdates()
      {
        if(socket)
            socket.disconnect();
      }
      
      
      function addMessage(data) {
          $('#online').html(data);
      }

      function setStatus(msg) {
         // console.log('Connection Status : ' + msg);
      }
        this.onbeforeunload = function () {
            closeSocketConForLiveUpdates();
        }
    </script>-->
    <!-- LIVE DATA UPDATE -->
</asp:Content>
