// JScript File

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////        DISPLAY SLIDER         //////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var g_MapSiteId = 0;
var g_MapView = 0;
var g_MapType = 0;
var g_MapId = 0;
var g_MapBuildingId = 0;
var g_MapFloorId = 0;
var g_MapUnitId = 0;
var g_FloorWidthFt='';
var g_FloorLengthFt='';

var g_CampusStartIndex = 0;
var g_CampusEndIndex = 3;

var g_BuildingStartIndex = 0;
var g_BuildingEndIndex = 3;

var g_FloorStartIndex = 0;
var g_FloorEndIndex = 3;

var g_UnitStartIndex = 0;
var g_UnitEndIndex = 3;

var g_MapConfigureViewLoaded = false;

var g_Curr = 1;
var g_Next = 2;
var g_Prev = 3;

var isFromReport = 0;
var SortColumn='';
var SortOrder='';
var SortImg='';
var g_Unsaved=0;

SortColumn = "RoomId";
SortOrder = "desc";
SortImg = "<image src='Images/downarrow.png' valign='middle' />";

var reportResponseArr = new Array();

var enumMapType = { "Campus": 1, "Building": 2, "Floor": 3, "Unit": 4, "Room": 5 }
var enumMapView = { "Map": 1, "Configure": 2 }

//Show Home view from Purchase Order
function redirectToHome()
{
    location.href = "Home.aspx";
}

//Show Hide Map View
function showHideMapView()
{
    g_MapView = enumMapView.Map;
    document.getElementById("ctl00_ContentPlaceHolder1_divMapView").style.display = "none";
    document.getElementById("ctl00_ContentPlaceHolder1_divMap").style.display = "";
    
}

//Show Hide Configure View
function showHideConfigureView()
{
    document.getElementById("ctl00_ContentPlaceHolder1_divMap").style.display = "none";
    document.getElementById("ctl00_ContentPlaceHolder1_divMapView").style.display = "";
    
   
    if(g_MapConfigureViewLoaded == false)
    {
        g_MapConfigureViewLoaded = true;
        loadMapView(g_MapSiteId);
    }
}

//Show Map View from Home view
function loadMapView(siteId)
{
    var refId;
    
    if(g_MapType === enumMapType.Campus)
    {
        $('#tblMapView').empty();
    }
    else if(g_MapType === enumMapType.Building)
    {
        refId = g_MapId;
        $('#tblMapView_Building').empty();
    }
    else if(g_MapType === enumMapType.Floor)
    {
        refId = g_MapBuildingId;
        $('#tblMapView_Floor').empty();
    }
    else if(g_MapType === enumMapType.Unit)
    {
        refId = g_MapFloorId;
        $('#tblMapView_Unit').empty();
    }
    else
    {
        $('#tblMapView').empty();
        $('#tblMapView_Building').empty();
        $('#tblMapView_Floor').empty();
        $('#tblMapView_Unit').empty();
        $('#tblMapView_Room').empty();
    }
    
    if(siteId > 0)
    {
        document.getElementById("ctl00_headerBanner_drpsitelist").value = siteId;
        var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
        var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
        var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
        
        document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName_MapView").innerHTML = siteText;
        
        if(g_MapType != 0)
            setPgAndgetRootbyMap(g_MapType, g_Curr, refId);
        else
            setPgAndgetRootbyMap(enumMapType.Campus, g_Curr, 0);
    }
}

//Open Map Dialog
function OpenMapDialog(titleVal,titleMapType,btnText)
{
    g_DialogView = "dialog-Map";
    g_MapType = titleMapType;
   
    var winWidth = 400;
    var winHeight = 200;
    
    if(g_MapType === enumMapType.Floor)
    {
        document.getElementById("trSqFt").style.display="";
         winHeight = 250;
    }
    else
    {
        document.getElementById("trSqFt").style.display="none";
    }
    
    
    
    $('#btnAddMap').val(btnText);
    $('#lblAddMapMsg').html('');
    
    //Open Dialog
    $( "#dialog-Map" ).dialog({
        title: titleVal,
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
            onCloseDialog();
        }
    });
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////        MAP VIEW         ////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Ajax Call for Map View
var g_MPObj;
var g_MPRoot;
function loadMapListView(SiteId)
{
    if(g_MPObj != null) { g_MPObj = null; }
    g_MPObj = CreateXMLObj();
   
    if(g_designMode != 1)
        $("#divLoading_MapFloor").show();
    
    
    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
    
    document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName_Map").innerHTML = siteText;
    
    if(g_MPObj!=null)
    {
        g_MPObj.onreadystatechange = ajaxLoadMapListView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadMapListView&Site=" + SiteId;
      
        if(GetBrowserType()=="isIE")
        {
            g_MPObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {
            g_MPObj.open("GET",DbConnectorPath, true);
        }
        g_MPObj.send(null);         
    }
    return false;
}

//Ajax Request for Map View
function ajaxLoadMapListView()
{
    if(g_MPObj.readyState==4)
    {
        if(g_MPObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_MPObj.responseXML.documentElement);
            
            var dsRoot = g_MPObj.responseXML.documentElement;
            g_MPRoot = dsRoot;
            
            var o_campusId=dsRoot.getElementsByTagName('CampusId');
            
            if(g_designMode === 1)
            {
                loadMapForselectedFloor();
                return;
            }
            loadMapsDropdown();
            
            var rootObj = $(g_MPRoot).find("Floor").children().filter("FloorId");
            
            if(o_campusId.length > 0)
            {
                //Map View
                if(g_MapView === enumMapView.Map)
                {
                   
                    getRootElement(enumMapType.Campus);
                    
                    if (rootObj.length == 0)
                    {
                        document.getElementById("tdSelectFloor").innerHTML = "Configure floor to view map.";
                    }
                    else
                    {
                        document.getElementById("tdSelectFloor").innerHTML = "Please select the floor to view map.";
                        
                        document.getElementById("selCampus").selectedIndex = 1;
                        getRootElement(enumMapType.Building);
                        
                        document.getElementById("selBuilding").selectedIndex = 1;
                        getRootElement(enumMapType.Floor);
                        
                        document.getElementById("selFloor").selectedIndex = 1;
                        FilterFloor();
                        loadMapForselectedFloor();
                    }
                }
                
                //Configure View
                if(g_MapView === enumMapView.Configure)
                {
                    g_MapConfigureViewLoaded = false;
                    showHideConfigureView();
                }
            }
            else
                   document.getElementById("tdSelectFloor").innerHTML = "Configure floor to view map.";
            //document.getElementById("divLoading_MapFloor").style.display="none";
            $("#divLoading_MapFloor").hide();
           
        }
    }
}

//Events
$(document).on('change', '#selCampus', function() {
    getRootElement(enumMapType.Building);
});

$(document).on('change', '#selBuilding', function() {
    getRootElement(enumMapType.Floor);
});

$(document).on('change', '#selFloor', function() {

    $('#txtUnitName').attr('value', '').focus(); 

    document.getElementById("chkDeviceDensity").checked = false;
    document.getElementById("chkHeatMaps").checked = false;
    document.getElementById("chkPagingFrequency").checked = false;
     document.getElementById("chkVirtualWalls").checked = false;
   loadMapForselectedFloor();
});

$(document).on('change', '#chkDeviceDensity', function() {

    $('#dialogStarReport').css({ 'display':'none' })
    
    var layerIndex = -1;
    for(var l = 0; l < reportLayers.length; l++)
    {
       if(reportLayers[l].name == 'Reports - StarDensity')
       {
         layerIndex = l;
         break;
       }
    }
    
    if(document.getElementById("chkDeviceDensity").checked == true)
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(true);
            hideLayersForReport(reportLayers[layerIndex]);
        }
        else
        {
            loadStarDensityselectedFloor();
        }
    }
    else
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(false);
        }
    }
        
   
});

$(document).on('change', '#chkHeatMaps', function() {

    $('#dialogStarReport').css({ 'display':'none' })
    
    var layerIndex = -1;
    for(var l = 0; l < reportLayers.length; l++)
    {
       if(reportLayers[l].name == 'Reports - StarHeatMap')
       {
         layerIndex = l;
         break;
       }
    }
    
    if(document.getElementById("chkHeatMaps").checked == true)
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(true);
            hideLayersForReport(reportLayers[layerIndex]);
        }
        else
        {
            loadStarHeatMapselectedFloor();
        }
    }
    else
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(false);
        }
    }
        
   
});


$(document).on('change', '#chkPagingFrequency', function() {

    $('#dialogMonitorReport').css({ 'display':'none' })
    
    var layerIndex = -1;
    for(var l = 0; l < reportLayers.length; l++)
    {
       if(reportLayers[l].name == 'Reports - PagingFrequency')
       {
         layerIndex = l;
         break;
       }
    }
    
    if(document.getElementById("chkPagingFrequency").checked == true)
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(true);
            hideLayersForReport(reportLayers[layerIndex]);
        }
        else
        {
            loadMonitorPagingFreqselectedFloor();
        }
    }
    else
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(false);
        }
    }
        
   
});


$(document).on('change', '#chkVirtualWalls', function() {

    $('#dialogMonitorReport').css({ 'display':'none' })
    
    var layerIndex = -1;
    for(var l = 0; l < reportLayers.length; l++)
    {
       if(reportLayers[l].name == 'Reports - VirtualWalls')
       {
         layerIndex = l;
         break;
       }
    }
    
    if(document.getElementById("chkVirtualWalls").checked == true)
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(true);
            hideLayersForReport(reportLayers[layerIndex]);
        }
        else
        {
            loadMonitorVirtualWallsselectedFloor();
        }
    }
    else
    {
        if(layerIndex > -1)
        {
            reportLayers[layerIndex].setVisibility(false);
        }
    }
        
   
});



function loadMapForselectedFloor()
{
   var svgFile,bgFile;
   var refId = $('#selFloor').val();
   g_SearchFloorId = refId;
   
   if($('#selFloor').val()>0)
   {
        document.getElementById("tdSelectedFloor").style.display="";
        document.getElementById("tdSelectedFloor").innerHTML =  $("#selFloor option:selected").text();
   }
   else
   {
        document.getElementById("tdSelectedFloor").style.display="none";
        document.getElementById("tdSelectedFloor").innerHTML =  "";
        return;
   }
   
   
   var rootObj = $(g_MPRoot).find("Floor").children().filter("FloorId").filter(function () { return $( this ).text() == String(refId);}).parent();
   
   svgFile = setundefined(getTagNameValue(rootObj.children().filter("svgFile")[0]));
   bgFile = setundefined(getTagNameValue(rootObj.children().filter("BGFile")[0]));
   g_FloorWidthFt = setundefined(getTagNameValue(rootObj.children().filter("widthInft")[0]));
   if (g_FloorWidthFt == 0)
        g_FloorWidthFt ='';
   g_FloorLengthFt = setundefined(getTagNameValue(rootObj.children().filter("lengthInft")[0]));
   if(g_FloorLengthFt == 0)
        g_FloorLengthFt='';
   //For TileLoad
   zoomify_url = setundefined(trim(getTagNameValue(rootObj.children().filter("BGFileTile")[0])));
   zoomify_width = setundefined(trim(getTagNameValue(rootObj.children().filter("BGFileWidth")[0])));
   zoomify_height = setundefined(trim(getTagNameValue(rootObj.children().filter("BGFileHeight")[0])));
   
   if(g_designMode == 1)
   {
         document.getElementById("divAddMonitors").style.display="";
        if(zoomify_width.length == 0) // no bg image uploaded
        {
            document.getElementById("trBgUploadRow").style.display="";
            document.getElementById("trMonitorRow").style.display="none";
        }
        else
        {
            document.getElementById("trBgUploadRow").style.display="none";
            document.getElementById("trMonitorRow").style.display="";
        }
   }
   
   
   if(g_UserRole == 1 || bgFile.length > 0)
   {
        document.getElementById("map").style.display="";
        document.getElementById("mapDiv").style.display="";
        document.getElementById("tdSelectFloor").style.display="none";
        
        //Set Map Loading Top & Left
       AdjustLoadingDiv();
        document.getElementById("divLoadingMap").style.display="";
            
       $("#lblLoadingMap").html('Loading Map...');
        setTimeout(function(){
                LoadFloorMap(document.getElementById('map'),svgFile,bgFile,zoomify_width,zoomify_height);
            }, 100);
        
      setTimeout(function(){
        //Load the infrastructures for the floor
        $("#lblLoadingMap").html('Loading Infrastructure...');
        g_isInfrasturctureLoaded = 0;
        g_isShowTags = 0;
        g_TagsLoaded = 0;
        $('#btnShowTag').removeClass("mapShowTags").addClass("mapNoShowTags");
        $('#btnReports').prop("disabled",true);
        $('#btnMonitorShow').prop("disabled",true);
        $('#btnStarShow').prop("disabled",true);
        $('#btnRegular').prop("disabled",true);
        $('#btnLarge').prop("disabled",true);
        $('#btnFullScreen').prop("disabled",true);
        GetInfrastructureMetaInfoForFloor(g_MapSiteId,refId);
         }, 100);
   }
   else
   {
        document.getElementById("map").style.display="none";
        document.getElementById("mapDiv").style.display="none";
        document.getElementById("tdSelectFloor").style.display="";
        
        if(svgFile.length == 0 && bgFile.length == 0)
            document.getElementById("tdSelectFloor").innerHTML = "No SVG and floor plan image uploaded";
        else
            document.getElementById("tdSelectFloor").innerHTML = "No floor image uploaded";
   }
}

//Load Maps On Add/Edit/Delete
function loadMapsDropdown()
{
    $('#map').html('');
    getRootElement(enumMapType.Campus);
    getRootElement(enumMapType.Building);
    getRootElement(enumMapType.Floor);
}

//Get Root Element from Xml Element
function getRootElement(mapType)
{
    var ids, names, descriptions, images, combobox, refId;
    
    if(mapType === enumMapType.Campus)
    {
        $("#selCampus").empty();
        $("#selBuilding").empty();
        $("#selFloor").empty();
        
        ids = g_MPRoot.getElementsByTagName('CampusId');
        names = g_MPRoot.getElementsByTagName('CampusName');
        descriptions = g_MPRoot.getElementsByTagName('CampusDescription');
        images = g_MPRoot.getElementsByTagName('CampusImage');
        combobox = $("#selCampus");
    }
    
    if(mapType === enumMapType.Building)
    {
        $("#selBuilding").empty();
        $("#selFloor").empty();
        
        refId = $("#selCampus").val();
        
        var rootObj = $(g_MPRoot).find("Building").children().filter("CampusId_Building").filter(function () { return $( this ).text() == String(refId);}).parent();
    
        ids = rootObj.children().filter("BuildingId");
        names = rootObj.children().filter("BuildingName");
        descriptions = rootObj.children().filter("BuildingDescription");
        images = rootObj.children().filter("BuildingImage");
        combobox = $("#selBuilding");
    }
    
    if(mapType === enumMapType.Floor)
    {
        $("#selFloor").empty();
        
        refId = $("#selBuilding").val();
        
        var rootObj = $(g_MPRoot).find("Floor").children().filter("BuildingId_Floor").filter(function () { return $( this ).text() == String(refId);}).parent();
    
        ids = rootObj.children().filter("FloorId");
        names = rootObj.children().filter("FloorName");
        descriptions = rootObj.children().filter("FloorDescription");
        images = rootObj.children().filter("svgFile");
        combobox = $("#selFloor");
    }
    
    loadintoCombobox(ids,names,combobox);
}

//load ids and names into dropdown
function loadintoCombobox(ids,names,combobox)
{
    $(combobox).append($('<option/>', { "text": "Select", "value": "0" }));
    
    for(var idx = 0; idx <= ids.length - 1; idx++)
    {
        var id = getTagNameValue(ids[idx]);
        var name = getTagNameValue(names[idx]);
        
        $(combobox).append($('<option/>', { "text": name, "value": id }));
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////        CONFIGURE VIEW         /////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Load Map View into Table
function loadMapViewintoTable(Ids,Names,Descriptions,indexStart,indexEnd,mapType,pgType,refId)
{
    var selText;
    $RootLength = parseInt(Ids.length) + 1;

    g_MapType = mapType;
    if(mapType == enumMapType.Campus)
    {
        $('#tblMapView').empty();
        $('#tblMapView_Building').empty();
        $('#tblMapView_Floor').empty();
        $('#tblMapView_Unit').empty();
        $('#tblMapView_Room').empty();
        $('#tdBuildingHeader').hide();
        $('#tdFloorHeader').hide();
        $('#tdUnitHeader').hide();
        $('#tdRoomHeader').hide();
        $("#spnBuildingCount").html("");
        $("#spnFloorCount").html("");
        $("#spnUnitCount").html("");
        $("#spnRoomCount").html("");
        $("#spnBuildingCount").removeClass("clsSpanMapCount");
        $("#spnFloorCount").removeClass("clsSpanMapCount");
        $("#spnUnitCount").removeClass("clsSpanMapCount");
        $("#spnRoomCount").removeClass("clsSpanMapCount");
        g_MapId = 0;
        g_MapBuildingId = 0;
        g_MapFloorId = 0;
        g_MapUnitId = 0;
        
        selText = "Campus";
        $("#spnCampusCount").html(parseInt($RootLength) - 1);
        $("#spnCampusCount").addClass("clsSpanMapCount");
    }
    else if(mapType == enumMapType.Building)
    {
        g_MapId = refId;
        $('#tblMapView_Building').empty();
        $('#tblMapView_Floor').empty();
        $('#tblMapView_Unit').empty();
        $('#tblMapView_Room').empty();
        $('#tdFloorHeader').hide();
        $('#tdUnitHeader').hide();
        $('#tdRoomHeader').hide();
        $("#spnFloorCount").html("");
        $("#spnUnitCount").html("");
        $("#spnRoomCount").html("");
        $("#spnFloorCount").removeClass("clsSpanMapCount");
        $("#spnUnitCount").removeClass("clsSpanMapCount");
        $("#spnRoomCount").removeClass("clsSpanMapCount");
        g_MapBuildingId = 0;
        g_MapFloorId = 0;
        g_MapUnitId = 0;
        
        selText = "Building";
        $("#spnBuildingCount").html(parseInt($RootLength) - 1);
        $("#spnBuildingCount").addClass("clsSpanMapCount");
    }
    else if(mapType == enumMapType.Floor)
    {
        g_MapBuildingId = refId;
        $('#tblMapView_Floor').empty();
        $('#tblMapView_Unit').empty();
        $('#tblMapView_Room').empty();
        $('#tdUnitHeader').hide();
        $('#tdRoomHeader').hide();
        $("#spnUnitCount").html("");
        $("#spnRoomCount").html("");
        $("#spnUnitCount").removeClass("clsSpanMapCount");
        $("#spnRoomCount").removeClass("clsSpanMapCount");
        g_MapFloorId = 0;
        g_MapUnitId = 0;
        
        selText = "Floor";
        $("#spnFloorCount").html(parseInt($RootLength) - 1);
        $("#spnFloorCount").addClass("clsSpanMapCount");
    }
    else if(mapType == enumMapType.Unit)
    {
        g_MapFloorId = refId;
        $('#tblMapView_Unit').empty();
        $('#tblMapView_Room').empty();
        $('#tdRoomHeader').hide();
        $("#spnRoomCount").html("");
        $("#spnRoomCount").removeClass("clsSpanMapCount");
        g_MapUnitId = 0;
        
        selText = "Unit";
        $("#spnUnitCount").html(parseInt($RootLength) - 1);
        $("#spnUnitCount").addClass("clsSpanMapCount");
    }
    
    $colAdd = 4;
    
    $row = $('<tr/>');
    
    $innerTbl = $('<table/>');
    $innerRow = $('<tr/>');
    
    $col = $('<td/>');
    $col.append('&nbsp;').css({ 'width': '10px' });
    $innerRow.append($col);
    
    $col = $('<td/>');
    if(setdisabledval($RootLength,g_Prev,mapType) != "")
    {
        $col.append($('<input/>', { type:'button', id:'btnPrev_' + selText, 'class': 'clsPrevMap', 'style': 'float:left', 'disabled': setdisabledval($RootLength,g_Prev,mapType) })).css({
                'width': '20px',
                'align': 'center'
        });
    }
    else
    {
        $col.append($('<input/>', { type:'button', id:'btnPrev_' + selText, onclick: 'setPgAndgetRootbyMap(' + mapType + ',' + g_Prev + ',' + refId + ')', 'class': 'clsPrevMap', 'style': 'float:left' })).css({
                'width': '20px',
                'align': 'center'
        });
    }
    $innerRow.append($col);
    
    for(var idx = indexStart; idx <= indexEnd; idx++)
    {
        if($(Ids)[idx] != null)
        {
            $id = getTagNameValue($(Ids)[idx]);
            $name = getTagNameValue($(Names)[idx]);
            $description = getTagNameValue($(Descriptions)[idx]);
            
            $innerCell = $('<td/>');
            $innerCell.append('&nbsp;').css({ 'width': '10px' });
            $innerRow.append($innerCell);
            
            $innerCell = $('<td/>');
            $innerCelldiv = $('<div/>');
            $innerCelldiv.append($name).css({ 
                    'width': '160px', 
                    'height' : '75px', 
                    'line-height': '75px',
                    'background-color' : '#FAFAFA',
                    'text-align' : 'center',
                    'vertical-align': 'middle',
                    'font-family': 'Helvetica,MyriadPro,Verdana, Arial, sans-serif',
                    'font-size': '12px',
                    'border' : 'solid 1px #CCC', 
                    'font-weight' : 'bold',
                    'cursor': 'pointer',
                    'white-space': 'nowrap',
                    'overflow': 'hidden',
                    'text-overflow': 'ellipsis'
            }).addClass("innerCellDiv");
            
            //$innerCelldiv.attr({ id:selText + '_' + $id, onmouseover: 'onmapmouseenter("' + selText + '_' + $id + '")' });
            $innerCelldiv.attr({ id:selText + '_' + $id });

            if(mapType == enumMapType.Campus)
            {
                $innerCelldiv.attr('onclick','loadBuilding(' + $id + ')');
            }
            else if(mapType == enumMapType.Building)
            {
                $innerCelldiv.attr('onclick','loadFloor(' + $id + ')');
            }
            else if(mapType == enumMapType.Floor)
            {
                $innerCelldiv.attr('onclick','loadUnit(' + $id + ')');
            }
            else if(mapType == enumMapType.Unit)
            {
                $innerCelldiv.attr('onclick','loadRooms(' + $id + ')');
            }
            $innerCell.append($innerCelldiv);
            
            $innerRow.append($innerCell);
            $innerCell = $('<td/>');
            $innerCell.append('&nbsp;').css({ 'width': '10px' });
            $innerRow.append($innerCell);
            
            $colAdd = parseInt($colAdd) - 1;
        }
    }
    
    if(indexEnd == (parseInt($RootLength) - 1))
    {
        $col = $('<td/>');
        $col.append('&nbsp;').css({ 'width': '10px' });
        $innerRow.append($col);
        
        $col = $('<td/>');
        $coldiv = $('<div/>');
        $coldiv.append('Add').css({ 
                'width': '160px', 
                'height' : '75px', 
                'line-height': '75px',
                'background-color' : '#FAFAFA',
                'font-family': 'Helvetica,MyriadPro,Verdana, Arial, sans-serif',
                'font-size': '12px',
                'text-align' : 'center',
                'vertical-align': 'middle',
                'border' : 'solid 1px #CCC', 
                'font-weight' : 'bold',
                'cursor': 'pointer',
                'white-space': 'nowrap',
                'overflow': 'hidden',
                'text-overflow': 'ellipsis'
        }).attr({
            'onclick': 'OpenMapDialog("Add ' + selText + '",' + mapType + ',"Add")'
        });
        $col.append($coldiv);
        $innerRow.append($col);
        
        $col = $('<td/>');
        $col.append('&nbsp;').css({ 'width': '10px' });
        $innerRow.append($col);
        
        if(mapType == enumMapType.Campus)
        {
            g_CampusEndIndex = parseInt(g_CampusEndIndex) + 1;
        }
        else if(mapType == enumMapType.Building)
        {
            g_BuildingEndIndex = parseInt(g_BuildingEndIndex) + 1;
        }
        else if(mapType == enumMapType.Floor)
        {
            g_FloorEndIndex = parseInt(g_FloorEndIndex) + 1;
        }
        else if(mapType == enumMapType.Unit)
        {
            g_UnitEndIndex = parseInt(g_UnitEndIndex) + 1;
        }
        
        $colAdd = parseInt($colAdd) - 1;
    }
    
    if($colAdd > 0)
    {
        for(var idx = 0; idx <= $colAdd - 1; idx++)
        {
            $col = $('<td/>');
            $col.append('&nbsp;').css({ 'width': '10px' });
            $innerRow.append($col);
            
            $col = $('<td/>');
            $col.append('&nbsp;').css({ 
                    'width': '160px', 
                    'height' : '80px'
            });
            $innerRow.append($col);
            
            $col = $('<td/>');
            $col.append('&nbsp;').css({ 'width': '10px' });
            $innerRow.append($col);
        }
    }
    
    $col = $('<td/>');
    if(setdisabledval($RootLength,g_Next,mapType) != "")
    {
        $col.append($('<input/>', { type:'button', id:'btnNext_' + selText, 'class': 'clsNextMap', 'disabled': setdisabledval($RootLength,g_Next,mapType) })).css({
                'width': '20px',
                'align': 'center'
        });
    }
    else
    {
        $col.append($('<input/>', { type:'button', id:'btnNext_' + selText, onclick: 'setPgAndgetRootbyMap(' + mapType + ',' + g_Next + ',' + refId + ')', 'class': 'clsNextMap' })).css({
                'width': '20px',
                'align': 'center'
        });
    }
    $innerRow.append($col);
    $innerTbl.append($innerRow);
    
    $col = $('<td/>');
    $col.append('<table >' + $innerTbl.html() + '</table>').css({ 
            'vertical-align': 'middle'
    });
    $row.append($col);
    
    if(mapType == enumMapType.Campus)
        $('#tblMapView').append($row);
    else if(mapType == enumMapType.Building)
        $('#tblMapView_Building').append($row);
    else if(mapType == enumMapType.Floor)
        $('#tblMapView_Floor').append($row);
    else if(mapType == enumMapType.Unit)
        $('#tblMapView_Unit').append($row);
}

//load Building
function loadBuilding(campusid)
{
    g_MapId = campusid;
    SetHeaderName(enumMapType.Building,campusid);
    setPgAndgetRootbyMap(enumMapType.Building, g_Curr, campusid);
}

//load Floor
function loadFloor(buildingid)
{
    g_MapBuildingId = buildingid;
    SetHeaderName(enumMapType.Floor,buildingid);
    setPgAndgetRootbyMap(enumMapType.Floor, g_Curr, buildingid);
}

//load Unit
function loadUnit(floorid)
{
    g_MapFloorId = floorid;
    SetHeaderName(enumMapType.Unit,floorid);
    setPgAndgetRootbyMap(enumMapType.Unit, g_Curr, floorid);
}

//load Rooms
function loadRooms(unitid)
{
    g_MapUnitId = unitid;
    SetHeaderName(enumMapType.Room,unitid);
    roomListView(g_Curr, unitid);
}

//Get Root from Xml Element
function setPgAndgetRootbyMap(mapType, pgType, refId)
{
    $("#tblRoom_Add").hide();
    $("#btnAddRoom").hide();
    
    if(mapType === enumMapType.Campus)
    {
        var o_campusId=g_MPRoot.getElementsByTagName('CampusId');
        var o_CampusName=g_MPRoot.getElementsByTagName('CampusName');
        var o_CampDescription=g_MPRoot.getElementsByTagName('CampusDescription');
            
        SetDisplayCount(o_campusId, o_CampusName, o_CampDescription, mapType, pgType);
    }
    
    if(mapType === enumMapType.Building)
    {
        var rootObj = $(g_MPRoot).find("Building").children().filter("CampusId_Building").filter(function () { return $( this ).text() == String(refId);}).parent();
    
        o_CampusId_Building = rootObj.children().filter("CampusId_Building");
        o_BuildingId = rootObj.children().filter("BuildingId");
        o_BuildingName = rootObj.children().filter("BuildingName");
        o_BuildingDescription = rootObj.children().filter("BuildingDescription");
            
        SetDisplayCountBuilding(refId, o_BuildingId, o_BuildingName, o_BuildingDescription, mapType, pgType);
    }
    
    if(mapType === enumMapType.Floor)
    {
        var rootObj = $(g_MPRoot).find("Floor").children().filter("BuildingId_Floor").filter(function () { return $( this ).text() == String(refId);}).parent();
    
        o_BuildingId_Floor = rootObj.children().filter("BuildingId_Floor");
        o_FloorId = rootObj.children().filter("FloorId");
        o_FloorName = rootObj.children().filter("FloorName");
        o_FloorDescription = rootObj.children().filter("FloorDescription");
            
        SetDisplayCountFloor(refId, o_FloorId, o_FloorName, o_FloorDescription, mapType, pgType);
    }
    
    if(mapType === enumMapType.Unit)
    {
        var rootObj = $(g_MPRoot).find("Unit").children().filter("FloorId_Unit").filter(function () { return $( this ).text() == String(refId);}).parent();
    
        o_FloorId_Unit = rootObj.children().filter("FloorId_Unit");
        o_UnitId = rootObj.children().filter("UnitId");
        o_UnitName = rootObj.children().filter("UnitName");
        o_UnitDescription = rootObj.children().filter("UnitDescription");
            
        SetDisplayCountUnit(refId, o_UnitId, o_UnitName, o_UnitDescription, mapType, pgType);
    }
}

//Set Display Count for Campus
function SetDisplayCount(Ids,Names,Descriptions,mapType,pgType)
{
    var nLength = parseInt(Ids.length) - 1;
    
    if(pgType == "1")
    {
        g_CampusStartIndex = 0;
        g_CampusEndIndex = 3;
    }
    else if(pgType == "2")
    {
        g_CampusStartIndex = g_CampusEndIndex + 1;
        g_CampusEndIndex = g_CampusEndIndex + 4;
    }
    else if(pgType == "3")
    {
        g_CampusEndIndex = g_CampusStartIndex - 1;
        g_CampusStartIndex = g_CampusEndIndex - 3;
    }
    
    if(g_CampusEndIndex > nLength)
    {
        g_CampusEndIndex = nLength;
    }
    
    if(pgType == g_Next)
    {
        if(nLength == g_CampusEndIndex)
        {
            g_CampusEndIndex = g_CampusEndIndex + 1;
        }
    }
    else
    {
        if(nLength == g_CampusEndIndex && nLength < 3)
        {
            g_CampusEndIndex = g_CampusEndIndex + 1;
        }
    }
    
    loadMapViewintoTable(Ids,Names,Descriptions,g_CampusStartIndex,g_CampusEndIndex,mapType,pgType,0);
}

//Set Display Count for Building
function SetDisplayCountBuilding(CampusId,Ids,Names,Descriptions,mapType,pgType)
{
    var nLength = parseInt(Ids.length) - 1;
    
    if(pgType == "1")
    {
        g_BuildingStartIndex = 0;
        g_BuildingEndIndex = 3;
    }
    else if(pgType == "2")
    {
        g_BuildingStartIndex = g_BuildingEndIndex + 1;
        g_BuildingEndIndex = g_BuildingEndIndex + 4;
    }
    else if(pgType == "3")
    {
        g_BuildingEndIndex = g_BuildingStartIndex - 1;
        g_BuildingStartIndex = g_BuildingEndIndex - 3;
    }
    
    if(g_BuildingEndIndex > nLength)
    {
        g_BuildingEndIndex = nLength;
    }
    
    if(pgType == g_Next)
    {
        if(nLength == g_BuildingEndIndex)
        {
            g_BuildingEndIndex = g_BuildingEndIndex + 1;
        }
    }
    else
    {
        if(nLength == g_BuildingEndIndex && nLength < 3)
        {
            g_BuildingEndIndex = g_BuildingEndIndex + 1;
        }
    }
    
    if(CampusId > 0)
        loadMapViewintoTable(Ids,Names,Descriptions,g_BuildingStartIndex,g_BuildingEndIndex,mapType,pgType,CampusId);
    else
        loadMapViewintoTable(Ids,Names,Descriptions,g_BuildingStartIndex,g_BuildingEndIndex,mapType,pgType,0);
}

//Set Display Count for Floor
function SetDisplayCountFloor(BuildingId,Ids,Names,Descriptions,mapType,pgType)
{
    var nLength = parseInt(Ids.length) - 1;
    
    if(pgType == "1")
    {
        g_FloorStartIndex = 0;
        g_FloorEndIndex = 3;
    }
    else if(pgType == "2")
    {
        g_FloorStartIndex = g_FloorEndIndex + 1;
        g_FloorEndIndex = g_FloorEndIndex + 4;
    }
    else if(pgType == "3")
    {
        g_FloorEndIndex = g_FloorStartIndex - 1;
        g_FloorStartIndex = g_FloorEndIndex - 3;
    }
    
    if(g_FloorEndIndex > nLength)
    {
        g_FloorEndIndex = nLength;
    }
    
    if(pgType == g_Next)
    {
        if(nLength == g_FloorEndIndex)
        {
            g_FloorEndIndex = g_FloorEndIndex + 1;
        }
    }
    else
    {
        if(nLength == g_FloorEndIndex && nLength < 3)
        {
            g_FloorEndIndex = g_FloorEndIndex + 1;
        }
    }
    
    if(BuildingId > 0)
        loadMapViewintoTable(Ids,Names,Descriptions,g_FloorStartIndex,g_FloorEndIndex,mapType,pgType,BuildingId);
    else
        loadMapViewintoTable(Ids,Names,Descriptions,g_FloorStartIndex,g_FloorEndIndex,mapType,pgType,0);
}

//Set Display Count for Unit
function SetDisplayCountUnit(FloorId,Ids,Names,Descriptions,mapType,pgType)
{
    var nLength = parseInt(Ids.length) - 1;
    
    if(pgType == "1")
    {
        g_UnitStartIndex = 0;
        g_UnitEndIndex = 3;
    }
    else if(pgType == "2")
    {
        g_UnitStartIndex = g_UnitEndIndex + 1;
        g_UnitEndIndex = g_UnitEndIndex + 4;
    }
    else if(pgType == "3")
    {
        g_UnitEndIndex = g_UnitStartIndex - 1;
        g_UnitStartIndex = g_UnitEndIndex - 3;
    }
    
    if(g_UnitEndIndex > nLength)
    {
        g_UnitEndIndex = nLength;
    }
    
    if(pgType == g_Next)
    {
        if(nLength == g_UnitEndIndex)
        {
            g_UnitEndIndex = g_UnitEndIndex + 1;
        }
    }
    else
    {
        if(nLength == g_UnitEndIndex && nLength < 3)
        {
            g_UnitEndIndex = g_UnitEndIndex + 1;
        }
    }
    
    if(FloorId > 0)
        loadMapViewintoTable(Ids,Names,Descriptions,g_UnitStartIndex,g_UnitEndIndex,mapType,pgType,FloorId);
    else
        loadMapViewintoTable(Ids,Names,Descriptions,g_UnitStartIndex,g_UnitEndIndex,mapType,pgType,0);
}

//Set Buttons Enabled/Disabled
function setdisabledval(nLength,pgType,mapType)
{
    var startIndex, endIndex;
    
    if(mapType == enumMapType.Campus)
    {
        startIndex = g_CampusStartIndex;
        endIndex = g_CampusEndIndex;
    }
    else if(mapType == enumMapType.Building)
    {
        startIndex = g_BuildingStartIndex;
        endIndex = g_BuildingEndIndex;
    }
    else if(mapType == enumMapType.Floor)
    {
        startIndex = g_FloorStartIndex;
        endIndex = g_FloorEndIndex;
    }
    else if(mapType == enumMapType.Unit)
    {
        startIndex = g_UnitStartIndex;
        endIndex = g_UnitEndIndex;
    }
    
    if(pgType == 3)
    {
        if(startIndex == 0)
        {
            return "disabled";
        }
    }
    
    if(pgType == 2)
    {
        if(parseInt(endIndex) + 1 >= nLength)
        {
            return "disabled";
        }
    }
    
    return ""
}

function SetHeaderName(mapType,mapId)
{
    if(mapType === enumMapType.Building)
    {
        $("#tblMapView td div").each(function() {
            $(this).css({ 'border': 'solid 1px #CCC' });
        })
        $('#Campus_' + mapId).css({ 'border': 'solid 2px #027DD7' });
        $('#tdBuildingHeader').show();
    }
    
    if(mapType === enumMapType.Floor)
    {
        $("#tblMapView_Building td div").each(function() {
            $(this).css({ 'border': 'solid 1px #CCC' });
        })
        $('#Building_' + mapId).css({ 'border': 'solid 2px #027DD7' });
        $('#tdFloorHeader').show();
    }
    
    if(mapType === enumMapType.Unit)
    {
        $("#tblMapView_Floor td div").each(function() {
            $(this).css({ 'border': 'solid 1px #CCC' });
        })
        $('#Floor_' + mapId).css({ 'border': 'solid 2px #027DD7' });
        $('#tdUnitHeader').show();
    }
    
    if(mapType === enumMapType.Room)
    {
        $("#tblMapView_Unit td div").each(function() {
            $(this).css({ 'border': 'solid 1px #CCC' });
        })
        $('#Unit_' + mapId).css({ 'border': 'solid 2px #027DD7' });
        $('#tdRoomHeader').show();
    }
}

//Mouse Over Map Elements
$(document).on("mouseover", ".innerCellDiv", function(e) {
    onmapmouseenter(this.id);
});

//Mouse Leave Map Elements
$(document).on("mouseleave", ".innerCellDiv", function(e) {
    onmapmouseleave(e, this.id);
});

//Mouse Leave Edit,Delete Icon
$(document).on("mouseleave", "#divCommands", function(e) {
    onmapmouseleave(e, this.id);
});

//Mouse Over Map Elements
var inside = false;
var g_HoverRefId;
var g_HoverMapType;
var g_isSVGEdit="0";
var g_isBGEdit = "0";
function onmapmouseenter(ctrl)
{
    var mapType, refId;
    
    var top = $("#" + ctrl).position().top;
    var left = $("#" + ctrl).position().left;
    
    if(ctrl.split("_")[0] == "Campus")
        mapType = 1;
    else if(ctrl.split("_")[0] == "Building")
        mapType = 2;
    else if(ctrl.split("_")[0] == "Floor")
        mapType = 3;
    else if(ctrl.split("_")[0] == "Unit")
        mapType = 4;
    refId = ctrl.split("_")[1];
    
    if(g_HoverRefId != refId || g_HoverMapType != mapType)
    {
        inside = false;
    }
    
    g_HoverRefId = refId;
    g_HoverMapType = mapType;
    
    $("#imgUpload").hide();
    $("#imgUploadCSV").hide();
    if(mapType === enumMapType.Floor)
    {
        $("#imgUpload").show();
        $("#imgUploadCSV").show();
        $("#ifrmUpload").attr("src","uploadFile.aspx?Cmd=UploadFile&SiteId=" + g_MapSiteId + "&FloorId=" + g_HoverRefId + "&isSVGEdit=" + g_isSVGEdit + "&isBGEdit=" + g_isBGEdit + "&BGUploadFromMap=0");
    }
    
    if(inside === false)
    {
        inside = true;
        
        $('#divCommands').css({ 'top': top + 'px', 'left': + left + 'px' });
        $('#divCommands').show();
    }
    
}

//Mouse Leave Map Elements
function onmapmouseleave(evt, ctrl)
{
    var top = $("#" + ctrl).position().top;
    var left = $("#" + ctrl).position().left;
    
    var topend = $("#" + ctrl).position().top + 70;
    var leftend = $("#" + ctrl).position().left + 90;
    
    var bFlag = false;
    
    if((evt.pageX >= left && evt.pageX <= leftend) && (evt.pageY >= top && evt.pageY <= topend))
    {
        bFlag = true;
    }
    
    if(bFlag === false)
    {
        inside = false;
        $('#divCommands').hide();
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////        ADD / EDIT / DELETE MAP         ////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var g_Map;
var g_IsDelete = 0;
//Ajax Call for Add / Edit / Delete Map View
function addEditDeleteMap(mapType,isDelete,mapId,mapBuildingId,mapFloorId,mapUnitId)
{
    $("#divLoading_MapView").show();
    
    $mapName = $("#txtMapName").val();
    $mapDesc = $("#txtMapDescription").val();
    $mapsqft = $("#txtSqft").val();
    $mapWidthft = $("#txtWidthInFt").val();
    
    g_IsDelete = isDelete;

    $.post("AjaxConnector.aspx?cmd=AddEditDeleteMap",
    {
        Site: g_MapSiteId,
        mapType: mapType,
        mapName: $mapName,
        mapDesc: $mapDesc,
        mapId: mapId,
        mapBuildingId: mapBuildingId,
        mapFloorId: mapFloorId,
        mapUnitId: mapUnitId,
        isDelete: isDelete,
        floorSqft: $mapsqft,
        Widthft: $mapWidthft
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddEditDeleteMap(dsRoot);
        }
        else {
            $("#divLoading_MapView").hide();
        }
    });
}

//Ajax Response for Add / Edit / Delete Map View
function ajaxAddEditDeleteMap(dsRoot)
{
    //var response = dsRoot.responseText;

    var o_Msg = dsRoot.getElementsByTagName('Error');
    var response = setundefined((o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text));

    $("#divLoading_MapView").hide();

    if (g_IsDelete == '0') {
        if ($('#btnAddMap').val() === "Add") {
            $("#txtMapName").val("");
            $("#txtMapDescription").val("");
            $("#txtSqft").val("");
            $("#txtWidthInFt").val("");
        }

        /*if($('#btnAddMap').val() === "Update")
        {
        $("#dialog-Map").dialog("close");
        }*/
    }

    if (response == "") {
        if (g_IsDelete == '0' && $('#btnAddMap').val() === "Add")
            $('#lblAddMapMsg').html('Successfully Added').addClass('clsMapSuccessTxt');
        else if (g_IsDelete == '0' && $('#btnAddMap').val() === "Update")
            $('#lblAddMapMsg').html('Successfully Updated').addClass('clsMapSuccessTxt');
        else if (g_IsDelete == '1')
            alert('Successfully Deleted');

        g_MapView = enumMapView.Configure;
        loadMapListView(g_MapSiteId);
        if ($('#dialog-Map'))
            $('#dialog-Map').dialog('close');
    }
    else if (response == "Error") {
        if (g_IsDelete == '0' && $('#btnAddMap').val() === "Add")
            $('#lblAddMapMsg').html('Error in Add').addClass('clsMapErrorTxt');
        else if (g_IsDelete == '0' && $('#btnAddMap').val() === "Update")
            $('#lblAddMapMsg').html('Error in Update').addClass('clsMapErrorTxt');
        else if (g_IsDelete == '1')
            alert('Error in Delete');
    }
    else {
        if (g_IsDelete == '0' && $('#btnAddMap').val() === "Add")
            $('#lblAddMapMsg').html(response).addClass('clsMapErrorTxt');
        else if (g_IsDelete == '0' && $('#btnAddMap').val() === "Update")
            $('#lblAddMapMsg').html(response).addClass('clsMapErrorTxt');
        else if (g_IsDelete == '1')
            alert(response);
    }
}

//Events
//On Add Button Click
$(document).on('click','#btnAddMap',function(){
    var mapId = 0;
    var mapBuildingId = 0;
    var mapFloorId = 0;
    var mapUnitId = 0;
    var mapType = 0;
    
    mapId = g_MapId;
    mapBuildingId = g_MapBuildingId;
    mapFloorId = g_MapFloorId;
    mapUnitId = g_MapUnitId;
    
    var mapName = document.getElementById("txtMapName").value;
    var txt;
   
    
    $('#lblAddMapMsg').html('');
    
    if($('#btnAddMap').val() === "Add")
    {
        mapType = g_MapType;
    }
    else
    {
        mapType = g_HoverMapType;
        
        if(g_HoverMapType === enumMapType.Campus)
            mapId = g_HoverRefId;
        else if(g_HoverMapType === enumMapType.Building)
            mapBuildingId = g_HoverRefId;
        else if(g_HoverMapType === enumMapType.Floor)
            mapFloorId = g_HoverRefId;
        else if(g_HoverMapType === enumMapType.Unit)
            mapUnitId = g_HoverRefId;
    }
    
    if(g_MapType == enumMapType.Campus)
    {
    
        if($('#btnAddMap').val() === "Add")
        {        
            mapId = 0;
            mapBuildingId = 0;
            mapFloorId = 0;
            mapUnitId = 0; 
            txt = "Campus";
        }
    }
    else if(g_MapType == enumMapType.Building)
    {
        if($('#btnAddMap').val() === "Add")
        {
            mapBuildingId = 0;
            mapFloorId = 0;
            mapUnitId = 0;
             txt = "Building";
        }
    }
    else if(g_MapType == enumMapType.Floor)
    {
        if($('#btnAddMap').val() === "Add")
        {
            mapFloorId = 0;
            mapUnitId = 0;
            txt = "Floor";
        }
    }
    else if(g_MapType == enumMapType.Unit)
    {
        if($('#btnAddMap').val() === "Add")
        {
            mapUnitId = 0;
            txt = "Unit";
        }
    }
   if(mapName == "")
            {
            alert(txt + " name should not be empty!");
            mapName.focus();            
            }
            else
            {
                addEditDeleteMap(mapType,'0',mapId,mapBuildingId,mapFloorId,mapUnitId);
            }    
            
   // addEditDeleteMap(mapType,'0',mapId,mapBuildingId,mapFloorId,mapUnitId);
});

//On Edit Image Click
$(document).on('click','#imgEdit',function(){
    var selText = "";
    var refId = g_HoverRefId;
    var mapType = g_HoverMapType;
    
    if(mapType === enumMapType.Campus)
    {
        g_MapId = refId;
        selText = "Campus";
    }
    else if(mapType === enumMapType.Building)
    {
        g_MapBuildingId = refId;
        selText = "Building";
    }
    else if(mapType === enumMapType.Floor)
    {
        g_MapFloorId = refId;
        selText = "Floor";
    }
    else if(mapType === enumMapType.Unit)
    {
        g_MapUnitId = refId;
        selText = "Unit";
    }
    
    loadEditValuesonDialog(refId,mapType);
    OpenMapDialog('Edit ' + selText,mapType,'Update');
});

//Load Values on Dialog
function loadEditValuesonDialog(refId,mapType)
{
    var rootObj;
    if(mapType === enumMapType.Campus)
    {
        rootObj = $(g_MPRoot).find("Campus").children().filter("CampusId").filter(function () { return $( this ).text() == String(refId);}).parent();
        $("#txtMapName").val(rootObj.children().filter("CampusName").text());
        $("#txtMapDescription").val(rootObj.children().filter("CampusDescription").text());
    }
    else if(mapType === enumMapType.Building)
    {
        rootObj = $(g_MPRoot).find("Building").children().filter("BuildingId").filter(function () { return $( this ).text() == String(refId);}).parent();
        $("#txtMapName").val(rootObj.children().filter("BuildingName").text());
        $("#txtMapDescription").val(rootObj.children().filter("BuildingDescription").text());
    }
    else if(mapType === enumMapType.Floor)
    {
        rootObj = $(g_MPRoot).find("Floor").children().filter("FloorId").filter(function () { return $( this ).text() == String(refId);}).parent();
        $("#txtMapName").val(rootObj.children().filter("FloorName").text());
        $("#txtMapDescription").val(rootObj.children().filter("FloorDescription").text());
        $("#txtSqft").val(setundefined(rootObj.children().filter("lengthInft").text()));
        $("#txtWidthInFt").val(setundefined(rootObj.children().filter("widthInft").text()));
    }
    else if(mapType === enumMapType.Unit)
    {
        rootObj = $(g_MPRoot).find("Unit").children().filter("UnitId").filter(function () { return $( this ).text() == String(refId);}).parent();
        $("#txtMapName").val(rootObj.children().filter("UnitName").text());
        $("#txtMapDescription").val(rootObj.children().filter("UnitDescription").text());
    }
}

//On Delete Image Click
$(document).on('click','#imgDelete',function(){
    var deleItem='';
     var mapType = g_HoverMapType;
     if(mapType === enumMapType.Campus)
        {
            deleItem = "Campus"; 
        }
        else if(mapType === enumMapType.Building)
        {
            deleItem = "Building";
        }
        else if(mapType === enumMapType.Floor)
        {
            deleItem = "Floor";
        }
        else if(mapType === enumMapType.Unit)
        {
            deleItem = "Unit";
        }
    
    var strOption=confirm("Are you sure do you want to delete this " + deleItem + " ?");
        
    if(strOption)
    {
        var refId = g_HoverRefId;
       
        
        mapId = g_MapId;
        mapBuildingId = g_MapBuildingId;
        mapFloorId = g_MapFloorId;
        mapUnitId = g_MapUnitId;
        
        if(mapType === enumMapType.Campus)
        {
            mapId = refId;
        }
        else if(mapType === enumMapType.Building)
        {
            mapBuildingId = refId;
        }
        else if(mapType === enumMapType.Floor)
        {
            mapFloorId = refId;
        }
        else if(mapType === enumMapType.Unit)
        {
            mapUnitId = refId;
        }
        
        g_MapType = mapType;
        addEditDeleteMap(mapType,'1',mapId,mapBuildingId,mapFloorId,mapUnitId);
    }
    else
    {
        return false;
    }
});

//On Upload Image Click
$(document).on('click','#imgUpload',function(){
   g_isSVGEdit="0";
   g_isBGEdit = "0";
   
   var svgFile,bgFile;
   var refId = g_HoverRefId;
   
   var rootObj = $(g_MPRoot).find("Floor").children().filter("FloorId").filter(function () { return $( this ).text() == String(refId);}).parent();
   
   if(GetBrowserType()=="isIE")
   {
       svgFile = rootObj.children().filter("svgFile")[0];
       bgFile = rootObj.children().filter("BGFile")[0];
       
       if(svgFile.childNodes.length > 0)
       {
            svgFile = getTagNameValue(svgFile);
            g_isSVGEdit="1";
       }
       
       if(bgFile.childNodes.length > 0)
       {
            bgFile = getTagNameValue(bgFile);
            g_isBGEdit="1";
       }
   }
   else if(GetBrowserType()=="isFF")
   {
       svgFile = getTagNameValue(rootObj.children().filter("svgFile")[0]);
       bgFile = getTagNameValue(rootObj.children().filter("BGFile")[0]);
       
       if(svgFile.length > 0)
       {
            g_isSVGEdit="1";    
       }
       
       if(bgFile.length > 0)
       {
            g_isBGEdit="1";    
       }
    }
   
    if(g_HoverMapType === enumMapType.Floor)
    {
        $("#ifrmUpload").attr("src","uploadFile.aspx?Cmd=UploadFile&SiteId=" + g_MapSiteId + "&FloorId=" + g_HoverRefId + "&isSVGEdit=" + g_isSVGEdit + "&isBGEdit=" + g_isBGEdit + "&SVGUrl=" + svgFile + "&BGUrl=" + bgFile + "&BGUploadFromMap=0");
    }
    
    //Open Dialog
    $( "#dialog-UploadFile" ).dialog({
        height: 400,
        width: 500,
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
            g_MapView = enumMapView.Configure;
            loadMapListView(g_MapSiteId);

            inside = false;
            $('#divCommands').hide();
        }
    });
});

//On Upload bg Image Clicked
$(document).on('click','#btnFloorBgUploadForSetup',function(){
   
    var FloorId = $('#selFloor').val();
   $("#ifrmUpload").attr("src","uploadFile.aspx?Cmd=UploadFile&SiteId=" + g_MapSiteId + "&FloorId=" + FloorId + "&isSVGEdit=&isBGEdit=1&SVGUrl=&BGUrl=&BGUploadFromMap=1");
  
    //Open Dialog
    $( "#dialog-UploadFile" ).dialog({
        height: 200,
        width: 600,
        title: "Upload Floor Plan Image",
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
});


//On Upload CSV Image Click
$(document).on('click','#imgUploadCSV',function(){
    var refId = g_HoverRefId;
   
    if(g_HoverMapType === enumMapType.Floor)
    {
        $("#ifrmUploadCSV").attr("src","uploadFile.aspx?Cmd=AddDeviceMetaInfo&MetaSiteId=" + g_MapSiteId + "&MetaFloorId=" + g_HoverRefId + "&MetaCSVUrl=" + (HoverFloorCSVUrl(g_HoverRefId)));
    }
    
    //Open Dialog
    $( "#dialog-UploadFileCSV" ).dialog({
        height: 400,
        width: 500,
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
            g_MapView = enumMapView.Configure;
            loadMapListView(g_MapSiteId);

            inside = false;
            $('#divCommands').hide();
        }
    });
});

//On Room Add Button Click
$(document).on('click','#btnAddRoom',function(){
    $("#ifrmRoomUpload").attr("src","uploadFile.aspx?Cmd=AddRoom&RoomSiteId=" + g_MapSiteId + "&CampusId=" + g_MapId + "&BuildingId=" + g_MapBuildingId + "&FloorId=" + g_MapFloorId + "&UnitId=" + g_MapUnitId + "&RoomCSVUrl=" + (GetRoomCSVUrl(g_MapUnitId)));
    
    //Open Dialog
    $( "#dialog-Room" ).dialog({
        height: 400,
        width: 500,
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
            roomListView(g_Curr, g_MapUnitId);
        }
    });
});

//Current Csv Link for Floor Id
function HoverFloorCSVUrl(refId)
{
    var InfrastructureCsvFilePath;
    var rootObj = $(g_MPRoot).find("Floor").children().filter("FloorId").filter(function () { return $( this ).text() == String(refId);}).parent();
    InfrastructureCsvFilePath = getTagNameValue(rootObj.children().filter("InfrastructureCsvFilePath")[0]);
    return InfrastructureCsvFilePath;
}

function GetRoomCSVUrl(refId)
{
    var RoomCsvFilePath;
    var rootObj = $(g_MPRoot).find("Unit").children().filter("UnitId").filter(function () { return $( this ).text() == String(refId);}).parent();
    RoomCsvFilePath = getTagNameValue(rootObj.children().filter("RoomCsvFilePath")[0]);
    return RoomCsvFilePath;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////       ROOM VIEW         ////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Ajax Call for Map View [Room]
var g_MPRObj;
function roomListView(pgType, unitId)
{
    $("#tblRoom_Add").show();
    $("#btnAddRoom").show();
    document.getElementById("divLoading_RoomView").style.display = "";
    
    g_MPRObj = CreateXMLObj();
    
    if(g_MPRObj!=null)
    {
        g_MPRObj.onreadystatechange = ajaxRoomListView;        

        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadRoomInfo&Site=" + g_MapSiteId + "&mapUnitId=" + unitId + "&sortcolumn=" + SortColumn + "&sortorder=" + SortOrder;

        if(GetBrowserType()=="isIE")
        {
            g_MPRObj.open("GET",DbConnectorPath, false);
        } 
        else if(GetBrowserType()=="isFF")
        {
            g_MPRObj.open("GET",DbConnectorPath, true);
        }
        g_MPRObj.send(null);         
    }
    return false;
}

//Ajax Response for Map View [Room]
function ajaxRoomListView()
{
    if(g_MPRObj.readyState==4)
    {
        if(g_MPRObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_MPRObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblMapView_Room');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblMapView_Room');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_MPRObj.responseXML.documentElement;
            
            var o_RoomId=dsRoot.getElementsByTagName('RoomId');
            var o_RoomName = dsRoot.getElementsByTagName('RoomName');
            var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
            var o_RoomDescription=dsRoot.getElementsByTagName('RoomDescription');
            var o_RoomHallway=dsRoot.getElementsByTagName('RoomHallway');
             
            nRootLength=o_RoomId.length;
            
            $("#spnRoomCount").html(nRootLength);
            $("#spnRoomCount").addClass("clsSpanMapCount");
            
            //Header
            row=document.createElement('tr');

            AddCellForSorting(row,"Room Id",'siteOverview_TopLeft_Box',"","","center","75px","40px","","RoomId",SortColumn,SortImg,enumSortingArr.RoomView);
            AddCellForSorting(row, "Name", 'siteOverview_Box', "", "", "center", "150px", "40px", "","RoomName",SortColumn,SortImg,enumSortingArr.RoomView);
            AddCellForSorting(row, "Monitor Id", 'siteOverview_Box', "", "", "center", "75px", "40px", "","MonitorId",SortColumn,SortImg,enumSortingArr.RoomView);
            AddCellForSorting(row,"Description",'siteOverview_Box',"","","center","250px","40px","","Description",SortColumn,SortImg,enumSortingArr.RoomView);
            AddCellForSorting(row,"Is Group",'siteOverview_Box',"","","center","100px","40px","","IsHallway",SortColumn,SortImg,enumSortingArr.RoomView);
            sTbl.appendChild(row);
            
            if(nRootLength >0)
            {
                for(var i = 0; i < nRootLength; i++)
                {
                    RoomId = (o_RoomId[i].textContent || o_RoomId[i].innerText || o_RoomId[i].text);
                    RoomName = (o_RoomName[i].textContent || o_RoomName[i].innerText || o_RoomName[i].text);
                    MonitorId = (o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text);
                    RoomDescription = (o_RoomDescription[i].textContent || o_RoomDescription[i].innerText || o_RoomDescription[i].text);
                    RoomHallway = (o_RoomHallway[i].textContent || o_RoomHallway[i].innerText || o_RoomHallway[i].text);
                    
                    row=document.createElement('tr');
                    AddCell(row,RoomId,'DeviceList_leftBox',"","","","","40px","");
                    AddCell(row, RoomName, 'siteOverview_cell', "", "", "", "", "40px", "");
                    AddCell(row, MonitorId, 'siteOverview_cell', "", "", "", "", "40px", "");
                    AddCell(row,RoomDescription,'siteOverview_cell',"","","","","40px","");
                    AddCell(row,RoomHallway,'siteOverview_cell',"","","","","40px","");
                    sTbl.appendChild(row);
                }
            }
            else
            {
                row=document.createElement('tr');
                AddCell(row,"No records found...",'siteOverview_cell_Full',4,"","left","","40px","");
                sTbl.appendChild(row);
            }
            
            document.getElementById("divLoading_RoomView").style.display = "none";
        }
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////        DEVICE META INFO         ////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Ajax Call for GetInfrastructureMetaInfoForFloor
var g_MetaFloorObj;
var g_MetaFloorObjRoot;
function GetInfrastructureMetaInfoForFloor(SiteId, FloorId) {
    if (g_MetaFloorObj != null) { g_MetaFloorObj = null; }
    g_MetaFloorObj = CreateXMLObj();

    if (g_MetaFloorObj != null) {
        g_MetaFloorObj.onreadystatechange = ajaxInfrastructureMetaInfoForFloor;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetInfrastructureMetaInfoForFloor&Site=" + SiteId + "&FloorId=" + FloorId;

        if (GetBrowserType() == "isIE") {
            g_MetaFloorObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_MetaFloorObj.open("GET", DbConnectorPath, true);
        }
        g_MetaFloorObj.send(null);
    }
    return false;
}


//Ajax Request for GetInfrastructureMetaInfoForFloor
function ajaxInfrastructureMetaInfoForFloor() {
    if (g_MetaFloorObj.readyState == 4) {
        if (g_MetaFloorObj.status == 200) {
            g_isInfrasturctureLoaded = 1;
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_MetaFloorObj.responseXML.documentElement);

            var dsRoot = g_MetaFloorObj.responseXML.documentElement;
            g_MetaFloorObjRoot = dsRoot;
            
           var monitorTable = $(dsRoot).find("dtMonitor");
           var starTable = $(dsRoot).find("dtStar");
           var accesspointTable = $(dsRoot).find("dtAccessPoint");
           var zoneTable = $(dsRoot).find("dtZone");
            
           load_UnitDetails(monitorTable,accesspointTable,zoneTable);
           
           updateInfraStructureMarkerinroom(starTable,monitorTable,accesspointTable,zoneTable);
           
           if(isFromReport == 1)
           {
                isFromReport = 0;
                getReportByFloor($('#selFloor').val());
           }
           
           if(g_isShowTags)
           {
                var refId = $('#selFloor').val();
                GetTagMetaInfoForFloor(g_MapSiteId,refId);
                g_TagsLoaded = 1;
           }
           else
           {
                if(g_floorPlanBgLoaded  == 1)
                    document.getElementById("divLoadingMap").style.display="none";
                else
                    $("#lblLoadingMap").html('Loading Floor Plan...');
           }
           
           if(g_IsSearch == "1")
           {
               if(g_SearchDeviceType == "1")
               {
                   GetTagMetaInfoForFloor(g_SearchSite,g_SearchFloorId);
                   document.getElementById("divLoadingMap").style.display="";
               }
               else
               {
                   searchDevice(g_SearchId);
                   g_IsSearch = 0;
               }
           }
           
           if(screenMode == 0) {
               regularSize();
           }
           
           if($('#btnReports').is(":disabled")) {
               $('#btnReports').prop("disabled",false);
               $('#btnMonitorShow').prop("disabled",false);
               $('#btnStarShow').prop("disabled",false);
               $('#btnRegular').prop("disabled",false);
               $('#btnLarge').prop("disabled",false);
               $('#btnFullScreen').prop("disabled",false);
            }
        }
    }
}


//Ajax Call for GetTagMetaInfoForFloor
var g_TagMetaFloorObj;
var g_TagMetaFloorObjRoot;
var g_TagLoaded = 0;
function GetTagMetaInfoForFloor(SiteId,FloorId)
{
   $("#lblLoadingMap").html('Loading Tags...');
    if(g_TagMetaFloorObj != null) { g_TagMetaFloorObj = null; }

    g_TagMetaFloorObj = CreateXMLObj();

    setTimeout(function () {
        if (g_TagMetaFloorObj != null) {
            g_TagMetaFloorObj.onreadystatechange = ajaxTagMetaInfoForFloor;

            DbConnectorPath = "AjaxConnector.aspx?cmd=GetTagMetaInfoForFloor&Site=" + SiteId + "&FloorId=" + FloorId;

            if (GetBrowserType() == "isIE") {
                g_TagMetaFloorObj.open("GET", DbConnectorPath, false);
            }
            else if (GetBrowserType() == "isFF") {
                g_TagMetaFloorObj.open("GET", DbConnectorPath, true);
            }
            g_TagMetaFloorObj.send(null);
        }
    }, 1);
    return false;
}


//Ajax Request for GetTagMetaInfoForFloor
function ajaxTagMetaInfoForFloor() {
    if (g_TagMetaFloorObj.readyState == 4) {
        if (g_TagMetaFloorObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_TagMetaFloorObj.responseXML.documentElement);

            var dsRoot = g_TagMetaFloorObj.responseXML.documentElement;

            var o_MonitorId = dsRoot.getElementsByTagName('MonitorId');
            var o_RoomId = dsRoot.getElementsByTagName('RoomId');
            var o_SvgId = dsRoot.getElementsByTagName('SvgId');
            var o_RoomName = dsRoot.getElementsByTagName('RoomName');
            var o_Description = dsRoot.getElementsByTagName('Description');
            var o_TagId = dsRoot.getElementsByTagName('TagId');
            var o_TagType = dsRoot.getElementsByTagName('TagType');
            var o_TagTypeName = dsRoot.getElementsByTagName('TagTypeName');
            var o_ModelItem = dsRoot.getElementsByTagName('ModelItem');
            var o_BatteryCapacity = dsRoot.getElementsByTagName('BatteryCapacity');
            var o_LastIRTime = dsRoot.getElementsByTagName('LastIRTime');
            var o_RoomSeen = dsRoot.getElementsByTagName('RoomSeen');
            var o_LockedStarId = dsRoot.getElementsByTagName('LockedStarId');
            var o_StarLocation = dsRoot.getElementsByTagName('StarLocation');
            var TagDetails = new Array();

            var nRootLength = o_MonitorId.length;

            for (var i = 0; i < nRootLength; i++) {
                var MonitorId = setundefined(o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text);
                var RoomId = setundefined(o_RoomId[i].textContent || o_RoomId[i].innerText || o_RoomId[i].text);
                var SvgId = setundefined(o_SvgId[i].textContent || o_SvgId[i].innerText || o_SvgId[i].text);
                var RoomName = setundefined(o_RoomName[i].textContent || o_RoomName[i].innerText || o_RoomName[i].text);
                var TagId = setundefined(o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
                var TagType = setundefined(o_TagType[i].textContent || o_TagType[i].innerText || o_TagType[i].text);
                var TagTypeName = setundefined(o_TagTypeName[i].textContent || o_TagTypeName[i].innerText || o_TagTypeName[i].text);
                var ModelItem = setundefined(o_ModelItem[i].textContent || o_ModelItem[i].innerText || o_ModelItem[i].text);
                var BatteryCapacity = setundefined(o_BatteryCapacity[i].textContent || o_BatteryCapacity[i].innerText || o_BatteryCapacity[i].text);
                var LastIRTime = setundefined(o_LastIRTime[i].textContent || o_LastIRTime[i].innerText || o_LastIRTime[i].text);
                var RoomSeen = setundefined(o_RoomSeen[i].textContent || o_RoomSeen[i].innerText || o_RoomSeen[i].text);
                var LockedStarId = setundefined(o_LockedStarId[i].textContent || o_LockedStarId[i].innerText || o_LockedStarId[i].text);
                var StarLocation = setundefined(o_StarLocation[i].textContent || o_StarLocation[i].innerText || o_StarLocation[i].text);
                
                TagDetails[i] = { "MonitorId": MonitorId, "roomId": RoomId, "SvgId": SvgId, "RoomName": RoomName, "tagID": TagId, "TagType": TagType, "ModelItem": ModelItem, "BatteryCapacity": BatteryCapacity, "LastIRTime": LastIRTime, "RoomSeen": RoomSeen,"TagTypeName" : TagTypeName,"LockedStarId" : LockedStarId, "StarLocation" : StarLocation }
            }
            
             UpdateTags(TagDetails);
             document.getElementById("divLoadingMap").style.display="none";
             g_TagsLoaded = 1;
             
             if(g_IsSearch == "1")
             {
                 searchDevice(g_SearchId);
                 g_isShowTags = 1;
                 $('#btnShowTag').removeClass('mapNoShowTags').addClass('mapShowTags');
                 g_IsSearch = 0;
                 g_TagLoaded = 1;
             }
        }
    }
}

//Save Monitors
function saveMonitor(SiteId,FloorId,MonitorId,Location,Notes,isHallway,monitorX,monitorY,monitorW,monitorH,polyPonits,g_uMode,g_dsvgId,g_svgDType,g_oldDeviceId,unitName,Xaxis,Yaxis,RoomXaxis,RoomYaxis,WidthFt,LengthFt)
{
    $.post("AjaxConnector.aspx?cmd=SaveMonitor",
    {
        Site: SiteId,
        FloorId: FloorId,
        MapMonitorId: MonitorId,
        Location: Location,
        Notes: Notes,
        isHallway: isHallway,
        monitorX: monitorX,
        monitorY: monitorY,
        monitorW: monitorW,
        monitorH: monitorH,
        polygonPoints: polyPonits,
        uMode: g_uMode,
        dsvgId: g_dsvgId,
        svgDType: g_svgDType,
        oldDeviceId: g_oldDeviceId,
        unitName: unitName,
        Xaxis: Xaxis,
        Yaxis: Yaxis,
        RoomXaxis: RoomXaxis,
        RoomYaxis: RoomYaxis,
        WidthFt: WidthFt,
        LengthFt: LengthFt
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxSaveMonitor(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}


//Ajax Request for GetInfrastructureMetaInfoForFloor
function ajaxSaveMonitor(dsRoot) {

    var o_Msg = dsRoot.getElementsByTagName('Error');
    var response = setundefined((o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text));

    $("#div_SaveMonitorLoader").hide();

    $("#divLoading_MapView").hide();

    g_Unsaved = 0;
    if (response == "") {
        monitorX = -1;
        monitorY = -1;
        monitorW = -1;
        monitorH = -1;
        RoompolygonPoints = '';

        document.getElementById("tblDimensions").style.display = "none";
        document.getElementById("lblsaveMonitorResult").style.display = "";

        $('#lblsaveMonitorResult').removeClass('clsMapErrorTxt');

        if (g_uMode == 3) {
            if (g_svgDType == 1)
                $('#lblsaveMonitorResult').html('Successfully Deleted the monitor [' + document.getElementById("txtMonitorId").value + ']').addClass('clsMapSuccessTxt');
            else if (g_svgDType == 2)
                $('#lblsaveMonitorResult').html('Successfully Deleted the star [' + document.getElementById("txtMonitorId").value + ']').addClass('clsMapSuccessTxt');
        }
        else if (g_uMode == 2) {
            if (g_svgDType == 1)
                $('#lblsaveMonitorResult').html('Successfully Updated the monitor [' + document.getElementById("txtMonitorId").value + ']').addClass('clsMapSuccessTxt');
            else if (g_svgDType == 2)
                $('#lblsaveMonitorResult').html('Successfully Updated the star [' + document.getElementById("txtMonitorId").value + ']').addClass('clsMapSuccessTxt');

        }
        else {
            if (g_svgDType == 1)
                $('#lblsaveMonitorResult').html('Successfully Added the monitor [' + document.getElementById("txtMonitorId").value + ']').addClass('clsMapSuccessTxt');
            else if (g_svgDType == 2)
                $('#lblsaveMonitorResult').html('Successfully Added the star [' + document.getElementById("txtMonitorId").value + ']').addClass('clsMapSuccessTxt');

        }

        g_dsvgId = 0;
        ClearTheEntries();
        ShowControlsForSelectedDevice();

        loadMapListView(g_MapSiteId);
        AdjustLoadingDiv();
        document.getElementById("divLoadingMap").style.display = "";
        if (g_designMode === 1) {

            $("#lblLoadingMap").html('Re-Loading Map...');
            //loadMapListView(g_MapSiteId);
        }
    }
    else {
        $('#lblsaveMonitorResult').removeClass('clsMapSuccessTxt');
        $('#lblsaveMonitorResult').html(response).addClass('clsMapErrorTxt');
    }
}

//Search Device
var g_SearchIdObj;
var g_SearchSite;
var g_SearchId;
var g_IsSearch = 0;
var g_SearchDeviceType;
var g_SearchFloorId;
function SearchDeviceMap(SiteId,SearchId)
{
    $("#divLoading_MapFloor").show();
    //document.getElementById("divLoading_MapFloor").style.display="";
    if (g_SearchIdObj != null) { g_SearchIdObj = null; }
    g_SearchIdObj = CreateXMLObj();
    g_SearchSite = SiteId;
    g_SearchId = SearchId;

    if (g_SearchIdObj != null) {
        g_SearchIdObj.onreadystatechange = ajaxSearchDeviceMap;

        DbConnectorPath = "AjaxConnector.aspx?cmd=SearchDeviceMap&Site=" + SiteId + "&SearchId=" + SearchId;

        if (GetBrowserType() == "isIE") {
            g_SearchIdObj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_SearchIdObj.open("GET", DbConnectorPath, true);
        }
        g_SearchIdObj.send(null);
    }
    return false;
}


//Ajax Request for Search Device
function ajaxSearchDeviceMap() 
{
    if (g_SearchIdObj.readyState == 4) 
    {
        if (g_SearchIdObj.status == 200) 
        {
            $("#divLoading_MapFloor").hide();
            //document.getElementById("divLoading_MapFloor").style.display="none";
            
            var dsRoot = g_SearchIdObj.responseXML.documentElement;
            
            if(dsRoot != null)
            {
                var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
                var o_FloorId = dsRoot.getElementsByTagName('FloorId');
                var o_CampusId = dsRoot.getElementsByTagName('CampusId');
                var o_BuildingId = dsRoot.getElementsByTagName('BuildingId');
                var o_SvgId = dsRoot.getElementsByTagName('SvgId');
                var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
                
                var nRootLength = o_DeviceId.length;
                
                document.getElementById("tdSelectFloor").innerHTML = "";
                document.getElementById("tdSelectFloor").style.display = "none";
                $("#tdSelectFloor").removeClass('clsMapErrorTxt').addClass('clsLALabelGrey');
                
                if(nRootLength > 0)
                {
                    var DeviceId = setundefined(getTagNameValue(o_DeviceId[0]));
                    var FloorId = setundefined(getTagNameValue(o_FloorId[0]));
                    var CampusId = setundefined(getTagNameValue(o_CampusId[0]));
                    var BuildingId = setundefined(getTagNameValue(o_BuildingId[0]));
                    var DeviceType = setundefined(getTagNameValue(o_DeviceType[0]));
                    
                    if(DeviceType == "2")
                        var SvgId = setundefined(getTagNameValue(o_SvgId[0]));
                    
                    if(g_SearchFloorId === undefined)
                        g_SearchFloorId = 0;
                        
                    if(g_SearchFloorId != FloorId)
                    {
                        $('#map').html('');
                        $("#selCampus").val('0');
                        $("#selBuilding").val('0');
                        $("#selFloor").val('0');
                        
                        $("#selCampus").val(CampusId);
                        getRootElement(enumMapType.Building);
                        
                        $("#selBuilding").val(BuildingId);
                        getRootElement(enumMapType.Floor);
                        
                        $("#selFloor").val(FloorId);
                        loadMapForselectedFloor();
                        
                        g_IsSearch = 1;
                        g_SearchDeviceType = DeviceType; 
                        g_SearchFloorId = FloorId;
                        
                        //GetInfrastructureMetaInfoForFloor(g_SearchSite,FloorId);
                    }
                    else
                    {
                        if(DeviceType == "1")
                        {
                            if(g_isShowTags == 0)
                            {
                                if(g_TagsLoaded == 0)
                                {
                                    g_IsSearch = 1;
                                    g_SearchDeviceType = DeviceType; 
                                    g_SearchFloorId = FloorId;
                                    GetTagMetaInfoForFloor(g_SearchSite,g_SearchFloorId);
                                    document.getElementById("divLoadingMap").style.display = "";
                                }
                                else
                                {
                                    g_isShowTags = 1;
                                    $('#btnShowTag').removeClass('mapNoShowTags').addClass('mapShowTags');
                                    searchDevice(g_SearchId);
                                    for(var i=0;i<taglayersArray.length;i++)
                                    {
                                        taglayersArray[i].setVisibility(true);
                                    }
                                    multiTagLayer.setVisibility(true);
                                }
                            }
                            else
                            {
                                g_isShowTags = 1;
                                $('#btnShowTag').removeClass('mapNoShowTags').addClass('mapShowTags');
                                searchDevice(g_SearchId);
                            }
                        }
                        else
                        {
                            searchDevice(g_SearchId);
                        }
                    }
                }
                else
                {
                    document.getElementById("tdSelectFloor").style.display = "";
                    $("#tdSelectFloor").removeClass('clsLALabelGrey').addClass('clsMapErrorTxt');
                    document.getElementById("tdSelectFloor").innerHTML = "Device not found in any floors.";
                }
            }
        }
    }
}

function resetSaveMonitor()
{
      document.getElementById("txtMonitorId").value ="";
      document.getElementById("txtLocation").value="";
      document.getElementById("txtNotes").value ="";
      document.getElementById("chkIsHallway").checked = false;
      g_Unsaved = 0;
      monitorX = -1;
      monitorY = -1;
      monitorW = -1;
      monitorH = -1;
      roomX = -1;
      roomY = -1;
      roomW = -1;
      roomH = -1;
      
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////        GENERAL         /////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//On Close Dialog
function onCloseDialog() 
{
     $("#txtMapName").val("");
     $("#txtMapDescription").val("");
     $("#txtSqft").val("");
     
     inside = false;
     $('#divCommands').hide();
}

//get value from htmlCollection
function getTagNameValue(nodeElem)
{
if (navigator.userAgent.indexOf('MSIE') != -1 || navigator.userAgent.indexOf('Firefox') != -1)
{
   return setundefined(nodeElem.textContent || nodeElem.innerText || nodeElem.text || nodeElem.innerHTML);
}
else
{
    return (nodeElem.textContent || nodeElem.innerText || nodeElem.text || nodeElem.innerHTML);
}
    
}

//Adjust Loading Div
function AdjustLoadingDiv()
{
     var mapTop = parseInt($("#map").offset().top) + 200;
     var mapLeft = parseInt($("#map").offset().left);
     var mapWidth = parseInt($("#map").css("width"));
     var mapHeight = parseInt($("#map").css("height"));
        
     $("#divLoadingMap").css({ 'top': mapTop + 'px', 'left': mapLeft + 'px','width' : mapWidth + 'px','height':mapHeight + 'px', 'z-index': 5000 });
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////        REPORTS         /////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//STAR DENISTY
var g_ReportStarDensityFloorObj;
function loadStarDensityselectedFloor()
{
     document.getElementById("divLoadingMap").style.display="";
    $("#lblLoadingMap").html('Loading Star Density...');
    if(g_ReportStarDensityFloorObj != null) { g_ReportStarDensityFloorObj = null; }
    var FloorId = $('#selFloor').val();
    
    g_ReportStarDensityFloorObj = CreateXMLObj();

    setTimeout(function () {
        if (g_ReportStarDensityFloorObj != null) {
            g_ReportStarDensityFloorObj.onreadystatechange = ajaxStarDensityForFloor;

            DbConnectorPath = "AjaxConnector.aspx?cmd=GetReportsForMap&Site=" + g_MapSiteId + "&FloorId=" + FloorId + "&mapReportType=StarDensity";

            if (GetBrowserType() == "isIE") {
                g_ReportStarDensityFloorObj.open("GET", DbConnectorPath, false);
            }
            else if (GetBrowserType() == "isFF") {
                g_ReportStarDensityFloorObj.open("GET", DbConnectorPath, true);
            }
            g_ReportStarDensityFloorObj.send(null);
        }
    }, 1);
    return false;
}

//Ajax Request for STAR DENISTY
function ajaxStarDensityForFloor() {
    if (g_ReportStarDensityFloorObj.readyState == 4) {
        if (g_ReportStarDensityFloorObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_ReportStarDensityFloorObj.responseXML.documentElement);

            var dsRoot = g_ReportStarDensityFloorObj.responseXML.documentElement;

            var o_ReportName = dsRoot.getElementsByTagName('ReportName');
            var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
            var o_ShapeType = dsRoot.getElementsByTagName('ShapeType');
            var o_ShapeColor = dsRoot.getElementsByTagName('ShapeColor');
            var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
            var o_SvgId = dsRoot.getElementsByTagName('SvgId');
            var o_Volume = dsRoot.getElementsByTagName('Volume');
            var o_ToSvgId = dsRoot.getElementsByTagName('ToSvgId');
           
            var nRootLength = o_DeviceId.length;

            for (var i = 0; i < nRootLength; i++) {
                var ReportName = setundefined(o_ReportName[i].textContent || o_ReportName[i].innerText || o_ReportName[i].text);
                var DeviceType = setundefined(o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
                var ShapeType = setundefined(o_ShapeType[i].textContent || o_ShapeType[i].innerText || o_ShapeType[i].text);
                var ShapeColor = setundefined(o_ShapeColor[i].textContent || o_ShapeColor[i].innerText || o_ShapeColor[i].text);
                var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
                var SvgId = setundefined(o_SvgId[i].textContent || o_SvgId[i].innerText || o_SvgId[i].text);
                var Volume = setundefined(o_Volume[i].textContent || o_Volume[i].innerText || o_Volume[i].text);
                var ToSvgId = setundefined(o_ToSvgId[i].textContent || o_ToSvgId[i].innerText || o_ToSvgId[i].text);
                
                DrawReportLayer(ReportName,DeviceId,DeviceType,SvgId,ShapeType,ShapeColor,Volume,ToSvgId);     
                  
            }
            
            if(nRootLength > 0)
            {
                 addAllLayersToMap(reportLayers);
                
                 for(var m = 0 ; m < reportLayers.length; m++)
                 {
                    map.setLayerIndex(reportLayers[m],m+5);
                 }
            }
            
             document.getElementById("divLoadingMap").style.display="none";
        }
    }
}


//STAR HEAT MAPS
var g_ReportStarHeatMapObj;
function loadStarHeatMapselectedFloor()
{
     document.getElementById("divLoadingMap").style.display="";
    $("#lblLoadingMap").html('Loading Heat Map...');
    if(g_ReportStarHeatMapObj != null) { g_ReportStarHeatMapObj = null; }
    var FloorId = $('#selFloor').val();
    
    g_ReportStarHeatMapObj = CreateXMLObj();

    setTimeout(function () {
        if (g_ReportStarHeatMapObj != null) {
            g_ReportStarHeatMapObj.onreadystatechange = ajaxStarHeatMapForFloor;

            DbConnectorPath = "AjaxConnector.aspx?cmd=GetReportsForMap&Site=" + g_MapSiteId + "&FloorId=" + FloorId + "&mapReportType=StarHeatMap";

            if (GetBrowserType() == "isIE") {
                g_ReportStarHeatMapObj.open("GET", DbConnectorPath, false);
            }
            else if (GetBrowserType() == "isFF") {
                g_ReportStarHeatMapObj.open("GET", DbConnectorPath, true);
            }
            g_ReportStarHeatMapObj.send(null);
        }
    }, 1);
    return false;
}

//Ajax Request for STAR DENISTY
function ajaxStarHeatMapForFloor() {
    if (g_ReportStarHeatMapObj.readyState == 4) {
        if (g_ReportStarHeatMapObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_ReportStarHeatMapObj.responseXML.documentElement);

            var dsRoot = g_ReportStarHeatMapObj.responseXML.documentElement;

            var o_ReportName = dsRoot.getElementsByTagName('ReportName');
            var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
            var o_ShapeType = dsRoot.getElementsByTagName('ShapeType');
            var o_ShapeColor = dsRoot.getElementsByTagName('ShapeColor');
            var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
            var o_SvgId = dsRoot.getElementsByTagName('SvgId');
            var o_Volume = dsRoot.getElementsByTagName('Volume');
            var o_ToSvgId = dsRoot.getElementsByTagName('ToSvgId');
           
            var nRootLength = o_DeviceId.length;

            for (var i = 0; i < nRootLength; i++) {
                var ReportName = setundefined(o_ReportName[i].textContent || o_ReportName[i].innerText || o_ReportName[i].text);
                var DeviceType = setundefined(o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
                var ShapeType = setundefined(o_ShapeType[i].textContent || o_ShapeType[i].innerText || o_ShapeType[i].text);
                var ShapeColor = setundefined(o_ShapeColor[i].textContent || o_ShapeColor[i].innerText || o_ShapeColor[i].text);
                var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
                var SvgId = setundefined(o_SvgId[i].textContent || o_SvgId[i].innerText || o_SvgId[i].text);
                var Volume = setundefined(o_Volume[i].textContent || o_Volume[i].innerText || o_Volume[i].text);
                var ToSvgId = setundefined(o_ToSvgId[i].textContent || o_ToSvgId[i].innerText || o_ToSvgId[i].text);
                
                DrawReportLayer(ReportName,DeviceId,DeviceType,SvgId,ShapeType,ShapeColor,Volume,ToSvgId);     
                  
            }
            
            if(nRootLength > 0)
            {
                 addAllLayersToMap(reportLayers);
                
                 for(var m = 0 ; m < reportLayers.length; m++)
                 {
                    map.setLayerIndex(reportLayers[m],m+5);
                 }
            }
            
             document.getElementById("divLoadingMap").style.display="none";
        }
    }
}


//MONITOR PAGING FREQUENCY
var g_ReportMonitorPagingFreqObj;
function loadMonitorPagingFreqselectedFloor()
{
     document.getElementById("divLoadingMap").style.display="";
    $("#lblLoadingMap").html('Loading Paging Frequency...');
    if(g_ReportMonitorPagingFreqObj != null) { g_ReportMonitorPagingFreqObj = null; }
    var FloorId = $('#selFloor').val();
    
    g_ReportMonitorPagingFreqObj = CreateXMLObj();

  setTimeout(function () {   if (g_ReportMonitorPagingFreqObj != null) {
        g_ReportMonitorPagingFreqObj.onreadystatechange = ajaxMonitorPagingFreqForFloor; 

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetReportsForMap&Site=" + g_MapSiteId + "&FloorId=" + FloorId + "&mapReportType=PagingFrequency";

        if (GetBrowserType() == "isIE") {
            g_ReportMonitorPagingFreqObj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_ReportMonitorPagingFreqObj.open("GET", DbConnectorPath, true);
        }
        g_ReportMonitorPagingFreqObj.send(null);
    }
}, 1);
    return false;
}

//Ajax Request for STAR DENISTY
function ajaxMonitorPagingFreqForFloor() {
    if (g_ReportMonitorPagingFreqObj.readyState == 4) {
        if (g_ReportMonitorPagingFreqObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_ReportMonitorPagingFreqObj.responseXML.documentElement);

            var dsRoot = g_ReportMonitorPagingFreqObj.responseXML.documentElement;

            var o_ReportName = dsRoot.getElementsByTagName('ReportName');
            var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
            var o_ShapeType = dsRoot.getElementsByTagName('ShapeType');
            var o_ShapeColor = dsRoot.getElementsByTagName('ShapeColor');
            var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
            var o_SvgId = dsRoot.getElementsByTagName('SvgId');
            var o_Volume = dsRoot.getElementsByTagName('Volume');
            var o_ToSvgId = dsRoot.getElementsByTagName('ToSvgId');
           
            var nRootLength = o_DeviceId.length;

            for (var i = 0; i < nRootLength; i++) {
                var ReportName = setundefined(o_ReportName[i].textContent || o_ReportName[i].innerText || o_ReportName[i].text);
                var DeviceType = setundefined(o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
                var ShapeType = setundefined(o_ShapeType[i].textContent || o_ShapeType[i].innerText || o_ShapeType[i].text);
                var ShapeColor = setundefined(o_ShapeColor[i].textContent || o_ShapeColor[i].innerText || o_ShapeColor[i].text);
                var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
                var SvgId = setundefined(o_SvgId[i].textContent || o_SvgId[i].innerText || o_SvgId[i].text);
                var Volume = setundefined(o_Volume[i].textContent || o_Volume[i].innerText || o_Volume[i].text);
                var ToSvgId = setundefined(o_ToSvgId[i].textContent || o_ToSvgId[i].innerText || o_ToSvgId[i].text);
                
                DrawReportLayer(ReportName,DeviceId,DeviceType,SvgId,ShapeType,ShapeColor,Volume,ToSvgId);     
                  
            }
            
            if(nRootLength > 0)
            {
                 addAllLayersToMap(reportLayers);
                
                 for(var m = 0 ; m < reportLayers.length; m++)
                 {
                    map.setLayerIndex(reportLayers[m],m+5);
                 }
            }
            
             document.getElementById("divLoadingMap").style.display="none";
        }
    }
}



//MONITOR VIRTUAL WALLS
var g_ReportMonitorVirtualWallsObj;
function loadMonitorVirtualWallsselectedFloor()
{
     document.getElementById("divLoadingMap").style.display="";
    $("#lblLoadingMap").html('Loading Virtual Walls...');
    if(g_ReportMonitorVirtualWallsObj != null) { g_ReportMonitorVirtualWallsObj = null; }
    var FloorId = $('#selFloor').val();
    
    g_ReportMonitorVirtualWallsObj = CreateXMLObj();

    if (g_ReportMonitorVirtualWallsObj != null) {
        g_ReportMonitorVirtualWallsObj.onreadystatechange = ajaxMonitorVirtualWallsForFloor;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetReportsForMap&Site=" + g_MapSiteId + "&FloorId=" + FloorId + "&mapReportType=VirtualWalls";

        if (GetBrowserType() == "isIE") {
            g_ReportMonitorVirtualWallsObj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_ReportMonitorVirtualWallsObj.open("GET", DbConnectorPath, true);
        }
        g_ReportMonitorVirtualWallsObj.send(null);
    }
    return false;
}

//Ajax Request for STAR DENISTY
function ajaxMonitorVirtualWallsForFloor() {
    if (g_ReportMonitorVirtualWallsObj.readyState == 4) {
        if (g_ReportMonitorVirtualWallsObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_ReportMonitorVirtualWallsObj.responseXML.documentElement);

            var dsRoot = g_ReportMonitorVirtualWallsObj.responseXML.documentElement;

            var o_ReportName = dsRoot.getElementsByTagName('ReportName');
            var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
            var o_ShapeType = dsRoot.getElementsByTagName('ShapeType');
            var o_ShapeColor = dsRoot.getElementsByTagName('ShapeColor');
            var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
            var o_SvgId = dsRoot.getElementsByTagName('SvgId');
            var o_Volume = dsRoot.getElementsByTagName('Volume');
            var o_ToSvgId = dsRoot.getElementsByTagName('ToSvgId');
           
            var nRootLength = o_DeviceId.length;

            for (var i = 0; i < nRootLength; i++) {
                var ReportName = setundefined(o_ReportName[i].textContent || o_ReportName[i].innerText || o_ReportName[i].text);
                var DeviceType = setundefined(o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
                var ShapeType = setundefined(o_ShapeType[i].textContent || o_ShapeType[i].innerText || o_ShapeType[i].text);
                var ShapeColor = setundefined(o_ShapeColor[i].textContent || o_ShapeColor[i].innerText || o_ShapeColor[i].text);
                var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
                var SvgId = setundefined(o_SvgId[i].textContent || o_SvgId[i].innerText || o_SvgId[i].text);
                var Volume = setundefined(o_Volume[i].textContent || o_Volume[i].innerText || o_Volume[i].text);
                var ToSvgId = setundefined(o_ToSvgId[i].textContent || o_ToSvgId[i].innerText || o_ToSvgId[i].text);
                
                DrawReportLayer(ReportName,DeviceId,DeviceType,SvgId,ShapeType,ShapeColor,Volume,ToSvgId);     
                  
            }
            
            if(nRootLength > 0)
            {
                 addAllLayersToMap(reportLayers);
                
                 for(var m = 0 ; m < reportLayers.length; m++)
                 {
                    map.setLayerIndex(reportLayers[m],m+5);
                 }
            }
            
             document.getElementById("divLoadingMap").style.display="none";
        }
    }
}

//REPORT FLOOR VIEW
var g_ReportFloorViewObj;
function ReportFloorView()
{
    $('#divLoading_FloorView').show();
    $('#tblFloorView').html('');
    
    if(g_ReportFloorViewObj != null) { g_ReportFloorViewObj = null; }
   
    g_ReportFloorViewObj = CreateXMLObj();

   setTimeout(function(){ if (g_ReportFloorViewObj != null) {
        g_ReportFloorViewObj.onreadystatechange = ajaxReportFloorView;
        
        var Bin = 0;
        if($('#selFilterType option:selected').val() == "LessThan90Days") {
            Bin = 1;
        }
        if($('#selFilterType option:selected').val() == "LessThan30Days") {
            Bin = 2;
        }

        if($('#txtFilter1').is(":visible") == true && $('#txtFilter1').is(":visible") == false && Bin == 0)
            DbConnectorPath = "AjaxConnector.aspx?cmd=ReportFloorView&Site=" + g_MapSiteId + "&DeviceType=" + $('#selDeviceType').val() + "&FilterType=" + $('#selFilterType option:selected').val() + "&CndType=" + $('#selCndType option:selected').val() + "&Filter1=" + $('#txtFilter1').val() + "&Filter2=" + "&CampusId=" + $("#selCampus").val() + "&BuildingId=" + $("#selBuilding").val() + "&Bin=0";
        else if($('#txtFilter1').is(":visible") == true && $('#txtFilter1').is(":visible") == true && Bin == 0)
            DbConnectorPath = "AjaxConnector.aspx?cmd=ReportFloorView&Site=" + g_MapSiteId + "&DeviceType=" + $('#selDeviceType').val() + "&FilterType=" + $('#selFilterType option:selected').val() + "&CndType=" + $('#selCndType option:selected').val() + "&Filter1=" + $('#txtFilter1').val() + "&Filter2=" + $('#txtFilter2').val() + "&CampusId=" + $("#selCampus").val() + "&BuildingId=" + $("#selBuilding").val() + "&Bin=0";
        else if($('#txtFilter1').is(":visible") == false && $('#txtFilter1').is(":visible") == false && Bin == 0)
            DbConnectorPath = "AjaxConnector.aspx?cmd=ReportFloorView&Site=" + g_MapSiteId + "&DeviceType=" + $('#selDeviceType').val() + "&FilterType=" + $('#selFilterType option:selected').val() + "&CndType=" + $('#selCndType option:selected').val() + "&Filter1=" + $('#selFilterFloor option:selected').val() + "&Filter2=" + "&CampusId=" + $("#selCampus").val() + "&BuildingId=" + $("#selBuilding").val() + "&Bin=0";
        else if(Bin > 0)
            DbConnectorPath = "AjaxConnector.aspx?cmd=ReportFloorView&Site=" + g_MapSiteId + "&DeviceType=" + $('#selDeviceType').val() + "&FilterType=" + $('#selFilterType option:selected').val() + "&CndType=" + $('#selCndType option:selected').val() + "&Filter1=&Filter2=&CampusId=" + $("#selCampus").val() + "&BuildingId=" + $("#selBuilding").val() + "&Bin=" + Bin;

        if (GetBrowserType() == "isIE") {
            g_ReportFloorViewObj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_ReportFloorViewObj.open("GET", DbConnectorPath, true);
        }
        g_ReportFloorViewObj.send(null);
    }},10);
    return false;
}

//Ajax Request for REPORT FLOOR VIEW
var g_FloorRootObj = null;
function ajaxReportFloorView() {
    if (g_ReportFloorViewObj.readyState == 4) {
        if (g_ReportFloorViewObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_ReportFloorViewObj.responseXML.documentElement);
            var dsRoot = g_ReportFloorViewObj.responseXML.documentElement;
            g_FloorRootObj = g_ReportFloorViewObj.responseXML.documentElement;
            
            if(dsRoot != null)
                loadFloorView(dsRoot);
        }
    }
}

//load floor on regular, large and full screen
function loadFlooronScreenDiffer()
{
    $('#tblFloorView').html('');
    if(g_FloorRootObj != null)
        loadFloorView(g_FloorRootObj);
}

//load floor view into division of Floor
function loadFloorView(dsRoot) 
{
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblFloorView');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblFloorView');
    }
    
    var height = $('#map').css('height');
    var heightforFloorView = 160;
    if($('#selFilterType').is(":visible"))
        heightforFloorView = 180;
    if($('#selCndType').is(":visible"))
        heightforFloorView = 200;
    if($('#txtFilter1').is(":visible"))
        heightforFloorView = 220;
    if($('#selFilterFloor').is(":visible"))
        heightforFloorView = 220;
    if($('#txtFilter2').is(":visible"))
        heightforFloorView = 240;
    $('#divFloorview').css('height', height.replace('px','') - heightforFloorView + 'px');
    if($('#tdResult').is(":visible") == false) {
        $('#tdResult').show();
        $('#tdCSV').show();
    }

    var o_FloorName = dsRoot.getElementsByTagName('FloorName');
    var o_DeviceId = dsRoot.getElementsByTagName('DeviceId');
    var o_FloorId = dsRoot.getElementsByTagName('FloorId');
    
    var o_ReportName = dsRoot.getElementsByTagName('ReportName');
	var o_SvgId = dsRoot.getElementsByTagName('SvgId');
	var o_DeviceType = dsRoot.getElementsByTagName('DeviceType');
	var o_Bin = dsRoot.getElementsByTagName('Bin');
	var o_Volume = dsRoot.getElementsByTagName('Volume');
	
	reportResponseArr.splice(0,reportResponseArr.length);
   
    var nRootLength = o_FloorId.length;
    var outerrow;
    var outercell;
    var table;
    var row;
    var cell;
    var OldFloorName = "";
    var OldFloorId = 0;
    var innerRow = '';
    
    var floorDetailArr = new Array();
    for (var i = 0; i < nRootLength; i++) {
        var FloorId = setundefined(o_FloorId[i].textContent || o_FloorId[i].innerText || o_FloorId[i].text);
        var FloorName = setundefined(o_FloorName[i].textContent || o_FloorName[i].innerText || o_FloorName[i].text);
        var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
        
        
        var SvgId = setundefined(o_SvgId[i].textContent || o_SvgId[i].innerText || o_SvgId[i].text);
	    var DeviceType = setundefined(o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
	    var Bin = setundefined(o_Bin[i].textContent || o_Bin[i].innerText || o_Bin[i].text);
	    var Volume = setundefined(o_Volume[i].textContent || o_Volume[i].innerText || o_Volume[i].text);
        var ReportName = setundefined(o_ReportName[0].textContent || o_ReportName[0].innerText || o_ReportName[0].text);
            
        var expandCollapseId = "tr" + FloorId;
        var expandCollapseImgId = "img" + FloorId;
        if(FloorName != OldFloorName)
        {
            if(OldFloorName != "") 
            {
                
                row = document.createElement("tr");
                cell = document.createElement("td");                                     
                table = document.createElement('table'); 
               
                $(table).append(innerRow);
                table.setAttribute("style","border-right: solid 1px #9C9C9C; border-left: solid 1px #9C9C9C;");
                $(table).attr({ "border":"0", "cellpadding":"0", "cellspacing":"0", "width":"100%" });
                $(table).css({ "height":"10px" });
                cell.appendChild(table);
                row.appendChild(cell);
                row.setAttribute("id", "tr" + OldFloorId);
                row.setAttribute("style", "display:none;");
                sTbl.appendChild(row);
                
                row = document.createElement('tr');
                row.setAttribute('height', '10px');
                sTbl.appendChild(row);
                reportResponseArr[reportResponseArr.length] = {FloorId: OldFloorId, FloorName:OldFloorName,responseArr:floorDetailArr};  
                innerRow="";
                floorDetailArr = new Array();
            }
            outerrow = document.createElement('tr');
            outercell = document.createElement('td');
            table = document.createElement('table');            
            row = document.createElement('tr');
            cell = document.createElement('td');
            cell.innerHTML = "<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td style='width: 20px;'><img id='" + expandCollapseImgId + "' src='Images/uparrow.png' onclick='expandCollapseFloor(" + expandCollapseId + "," + expandCollapseImgId + ")' /></td><td class='clsLinkMap' onclick='showFlooronMaponFloorViewClick(" + FloorId + ")'><a class='clsLinkMap'>" + FloorName + "</a></td></tr></table>";
            cell.setAttribute("style","padding-left: 5px; color: #000000; font-size: 11px; font-weight: bold; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; cursor: pointer; width: 240px; height: 25px; vertical-align: middle;");
            row.appendChild(cell);
            table.appendChild(row);
            table.setAttribute("style","background-color: #C5C5C7;border-right: solid 1px #9C9C9C; border-left: solid 1px #9C9C9C;border-top: solid 1px #9C9C9C; border-bottom: solid 1px #9C9C9C;");
            $(table).attr({ "border":"0", "cellpadding":"0", "cellspacing":"0", "width":"100%" });
            $(table).css({ "height":"20px" });
            outercell.appendChild(table);
            outerrow.appendChild(outercell);
            sTbl.appendChild(outerrow);
        }
        
        row = document.createElement('tr');
        cell = document.createElement('td');
        cell.innerHTML = DeviceId;
        cell.setAttribute("style","padding-left: 20px; background-color: #FFFFFF; color: #000000; font-size: 11px; font-weight: normal; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; width: 120px; height: 20px; vertical-align: middle; border-bottom: solid 1px #9C9C9C;");
        row.appendChild(cell);
        cell = document.createElement('td');
        cell.innerHTML = Volume;
        cell.setAttribute("style","padding-left: 20px; background-color: #FFFFFF; color: #000000; font-size: 11px; font-weight: normal; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif; width: 120px; height: 20px; vertical-align: middle; border-bottom: solid 1px #9C9C9C;");
        row.appendChild(cell);
        innerRow = innerRow + row.outerHTML;
        
        OldFloorName = FloorName;
        OldFloorId = FloorId;
        
        floorDetailArr[floorDetailArr.length] = {LayerName:ReportName, FloorId: FloorId, FloorName:FloorName, DeviceId:DeviceId, SvgId:SvgId, DeviceType:DeviceType, Bin:Bin , Volume:Volume};
        
        if(i == nRootLength-1)
        {
            reportResponseArr[reportResponseArr.length] = {FloorId: FloorId, FloorName:FloorName,responseArr:floorDetailArr};  
        }   
              
    }
    
    if(OldFloorName != "") 
    {
        row = document.createElement('tr');
        cell = document.createElement('td');
        table = document.createElement('table');
        $(table).append(innerRow);
        table.setAttribute("style","border-right: solid 1px #9C9C9C; border-left: solid 1px #9C9C9C;");
        $(table).attr({ "border":"0", "cellpadding":"0", "cellspacing":"0", "width":"100%" });
        table.setAttribute("style","background-color: #FFFFFF;border-right: solid 1px #9C9C9C; border-left: solid 1px #9C9C9C;");
        $(table).css({ "height":"20px" });
        cell.appendChild(table);
        row.appendChild(cell);
        row.setAttribute("id", "tr" + OldFloorId);
        row.setAttribute("style", "display:none;");
        sTbl.appendChild(row);        
        row = document.createElement('tr');
        row.setAttribute('height', '20px');
        sTbl.appendChild(row);
    }
    
    
    if(nRootLength == 0) {
        row = document.createElement('tr');
        cell = document.createElement('td');
        table = document.createElement('table');  
              
       
        
         if (navigator.userAgent.indexOf('Chrome') != -1 || navigator.userAgent.indexOf("Firefox") != -1 )
        {
             table.innerHTML = "No Records found!!!";
        }
        else
        {
            table.innerText="No Records found!!!";
        } 
        $(table).attr({ "border":"0", "cellpadding":"0", "cellspacing":"0", "width":"100%" });
        cell.appendChild(table);
        cell.setAttribute("align","center");
        cell.setAttribute("class","clsMapErrorTxt");
        row.appendChild(cell);
        sTbl.appendChild(row);
        
        //disable csv
        $('#exportfloorcsv').prop("disabled",true);
    } else {
        $('#exportfloorcsv').prop("disabled",false);
    }
    
    if($('#divLoading_FloorView').is(":visible"))
        $('#divLoading_FloorView').hide();
}

//showFlooronMaponFloorViewClick
function showFlooronMaponFloorViewClick(floorId)
{
    $("#selFloor").val(floorId);
    isFromReport = 1;
    loadMapForselectedFloor();
}

//expand/collapse floor
function expandCollapseFloor(rowId, imgId)
{
    if(rowId.style.display == 'inline' || rowId.style.display == '') {
        rowId.style.display = 'none';
        imgId.src = 'Images/uparrow.png';
    } else {
        rowId.style.display = 'inline';
        imgId.src = 'Images/downarrow.png';
    }
}

//ajax request for filter floor
var g_FilterFloorObj;
function FilterFloor()
{
    if(g_FilterFloorObj != null) { g_FilterFloorObj = null; }
   
    g_FilterFloorObj = CreateXMLObj();

    if (g_FilterFloorObj != null) {
        g_FilterFloorObj.onreadystatechange = ajaxFilterFloor;

        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadFiltersforMap";

        if (GetBrowserType() == "isIE") {
            g_FilterFloorObj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            g_FilterFloorObj.open("GET", DbConnectorPath, true);
        }
        g_FilterFloorObj.send(null);
    }
    return false;
}

//ajax response for filter floor
var g_FloorFilterRoot;
function ajaxFilterFloor() {
    if (g_FilterFloorObj.readyState == 4) {
        if (g_FilterFloorObj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_FilterFloorObj.responseXML.documentElement);
            g_FloorFilterRoot = g_FilterFloorObj.responseXML.documentElement;
            loadDropdownforAllDeviceType();
        }
    }
}

//load dropdown for all device type
function loadDropdownforAllDeviceType()
{
    //For Tag
    var filterRootTagObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Tag");}).parent();
    enumTagType["Less Than 90 Days"] = "LessThan90Days";
    enumTagType["Less Than 30 Days"] = "LessThan30Days";
    $.each(filterRootTagObj, function (i, rootObj) {
    var cname = getTagNameValue($(rootObj).children().filter("DisplayColumn")[0]);
        if(cname !="Building Name" && cname !="Campus Name" && cname !="Floor Id" && cname !="Floor Name")
            enumTagType[getTagNameValue($(rootObj).children().filter("DisplayColumn")[0])] = getTagNameValue($(rootObj).children().filter("DBColumn")[0]);
    });
    
    //For Monitor
    var filterRootMonitorObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Monitor");}).parent();
    enumMonitorType["Less Than 90 Days"] = "LessThan90Days";
    enumMonitorType["Less Than 30 Days"] = "LessThan30Days";
    $.each(filterRootMonitorObj, function (i, rootObj) {
    var cname = getTagNameValue($(rootObj).children().filter("DisplayColumn")[0]);
    if(cname !="Floor Id")
        enumMonitorType[getTagNameValue($(rootObj).children().filter("DisplayColumn")[0])] = getTagNameValue($(rootObj).children().filter("DBColumn")[0]);
    });
    
    //For Star
    var filterRootStarObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Star");}).parent();
    $.each(filterRootStarObj, function (i, rootObj) {
        enumStarType[getTagNameValue($(rootObj).children().filter("DisplayColumn")[0])] = getTagNameValue($(rootObj).children().filter("DBColumn")[0]);
    });
}

//load into dropdown if values are found in db column
function fillDropdownifValuesfound()
{
    var filterRootObj;
    
    if($('#selDeviceType option:selected').val() == String(enumDeviceType.Tag))
        filterRootObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Tag");}).parent();
    if($('#selDeviceType option:selected').val() == String(enumDeviceType.Monitor))
        filterRootObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Monitor");}).parent();
    if($('#selDeviceType option:selected').val() == String(enumDeviceType.Star))
        filterRootObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Star");}).parent();
    
    if(filterRootObj.length > 0)
    {
        var filterRootObj2 = $(filterRootObj).children().filter("DBColumn").filter(function () { return $( this ).text() == String($('#selFilterType option:selected').val());}).parent();
        
        if(filterRootObj2.length > 0) {
            if(getTagNameValue($(filterRootObj2).children().filter("Values")[0]) != "") {
                g_IsValuesFound = true;
                enumFilterType = {};
                var valueList = new Array();
                valueList = (getTagNameValue($(filterRootObj2).children().filter("Values")[0])).split(",");
                for(var idx=0; idx<valueList.length; idx++) {
                    enumFilterType[valueList[idx]] = String(valueList[idx]);
                }
                $('#selFilterFloor').show();
                loadValuesintoDropdown($('#selFilterFloor'),enumFilterType);
            } else {
                $('#txtFilter2').hide();
                $('#txtFilter1').show();
                
                if($('#selCndType option:selected').val() == String(enumCndtType.between)) {
                    $('#txtFilter2').show();
                    var height = $('#map').css('height');
                    $('#divFloorview').css('height', height.replace('px','') - 240 + 'px');
                }
            }
        }
    }
}

//check values are found for db column
function checkValueExists()
{
    var filterRootObj;
    var g_IsValuesFound = false;
    
    if($('#selDeviceType option:selected').val() == String(enumDeviceType.Tag))
        filterRootObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Tag");}).parent();
    if($('#selDeviceType option:selected').val() == String(enumDeviceType.Monitor))
        filterRootObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Monitor");}).parent();
    if($('#selDeviceType option:selected').val() == String(enumDeviceType.Star))
        filterRootObj = $(g_FloorFilterRoot).children().children().filter("DeviceType").filter(function () { return $( this ).text() == String("Star");}).parent();
    
    if(filterRootObj.length > 0)
    {
        var filterRootObj2 = $(filterRootObj).children().filter("DBColumn").filter(function () { return $( this ).text() == String($('#selFilterType option:selected').val());}).parent();
        
        if(filterRootObj2.length > 0) {
            if(getTagNameValue($(filterRootObj2).children().filter("Values")[0]) != "" || getTagNameValue($(filterRootObj2).children().filter("Values")[0]) == undefined) {
                g_IsValuesFound = true;
            }
        }
    }
    
    return g_IsValuesFound;
}

//onclick floor csv button
$(document).on('click','#exportfloorcsv',function(){
    exportFloortoCSV();
});

//export report to csv file
function exportFloortoCSV() {
    var csvstringbuilder = [];
    
    //load into array
    //site name, campus, building
    csvstringbuilder.push(CSVCell("Site Name : " + $('#ctl00_ContentPlaceHolder1_lblSiteName_Map').html(),false,true));
    csvstringbuilder.push(CSVNewLine());
    csvstringbuilder.push(CSVCell("Campus : " + $('#selCampus option:selected').text(),false,true));
    csvstringbuilder.push(CSVNewLine());
    csvstringbuilder.push(CSVCell("Building : " + $('#selBuilding option:selected').text(),false,true));
    csvstringbuilder.push(CSVNewLine());
    csvstringbuilder.push(CSVNewLine());
    
    //reports
    csvstringbuilder.push(CSVCell("Report : ", true));
    csvstringbuilder.push(CSVCell($('#selDeviceType option:selected').text() + " - "));
    
    if($('#selFilterType option:selected').val() != "0") {
        csvstringbuilder.push(CSVCell($('#selFilterType option:selected').text()));
    }
    
    if($('#selCndType option:selected').val() > 0) {
        csvstringbuilder.push(CSVCell(" " + $('#selCndType option:selected').text()));
    }
    
    if($('#selFilterFloor option:selected').val() > 0) {
        csvstringbuilder.push(CSVCell(" " + $('#selFilterFloor option:selected').text()));
    }
    
    if($('#txtFilter1').val() != "") {
        csvstringbuilder.push(" " + CSVCell($('#txtFilter1').val()));
    }
    
    if($('#txtFilter2').val() != "") {
        csvstringbuilder.push(CSVCell(" And " + $('#txtFilter2').val()));
    }
    csvstringbuilder.push(CSVNewLine());
    csvstringbuilder.push(CSVNewLine());
    
    //results
    csvstringbuilder.push(CSVCell("Results"));
    
    var o_FloorName = g_FloorRootObj.getElementsByTagName('FloorName');
    var o_DeviceId = g_FloorRootObj.getElementsByTagName('DeviceId');
    var o_FloorId = g_FloorRootObj.getElementsByTagName('FloorId');
    var o_Volume = g_FloorRootObj.getElementsByTagName('Volume');
    var nRootLength = o_FloorId.length;
    var oldFloorName = "";
    
    for (var i = 0; i < nRootLength; i++) {
        var FloorName = setundefined(o_FloorName[i].textContent || o_FloorName[i].innerText || o_FloorName[i].text);
        var DeviceId = setundefined(o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
        var Volume = setundefined(o_Volume[i].textContent || o_Volume[i].innerText || o_Volume[i].text);
        
        if(oldFloorName != FloorName) {
            csvstringbuilder.push(CSVNewLine());

            csvstringbuilder.push(CSVCell("Floor Name : " + FloorName));
            csvstringbuilder.push(CSVNewLine());
        }
        csvstringbuilder.push(CSVCell(DeviceId, true));
        csvstringbuilder.push(CSVCell(Volume));
        csvstringbuilder.push(CSVNewLine());
        
        oldFloorName = FloorName;
    }
    
    //join array as string
    csvstringbuilder = csvstringbuilder.join("");
    
    //file name
    var m_names = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
    var reldate = new Date();
    var curr_date = reldate.getDate();
    var curr_month = reldate.getMonth();
    var curr_year = reldate.getFullYear();
    var curr_hour = reldate.getHours();
    var curr_min = reldate.getMinutes();
    var curr_sec = reldate.getSeconds();
    if(curr_date < 10) {curr_date = '0' + curr_date };
    if(curr_hour < 10) {curr_hour = '0' + curr_hour };
    if(curr_min < 10) {curr_min = '0' + curr_min };
    if(curr_sec < 10) {curr_sec = '0' + curr_sec };
    var DownloadDate = String(curr_date) + String(curr_month) + String(curr_year) + String(curr_hour) + String(curr_min) + String(curr_sec);
    
    //download csv
    tableToCSV(csvstringbuilder, "Report-" + DownloadDate);
}

function getReportByFloor(floorID)
{
    for(var i=0; i < reportResponseArr.length; i++ )
    {
        if(reportResponseArr[i].FloorId == floorID)
        {
            var tempArr = reportResponseArr[i].responseArr;
             
            for(var j=0; j < tempArr.length; j++ )
            {
                DrawSearchReportLayer(tempArr[j].LayerName,tempArr[j].DeviceId,tempArr[j].DeviceType,tempArr[j].SvgId,tempArr[j].Bin,tempArr[j].FloorId,tempArr[j].FloorName,tempArr[j].Volume);
            }
           
        }
    }
    
    addAllLayersToMap(reportLayers);
            
    for(var m = 0 ; m < reportLayers.length; m++)
    {
        map.setLayerIndex(reportLayers[m],m+5);
    }
}


 function getTimeWithMeridian(date) {
    var d = new Date(date);
    var hh = d.getHours();
    var m = d.getMinutes();
    var s = d.getSeconds();
    var dd = "AM";
    var h = hh;
    if (h >= 12) {
        h = hh-12;
        dd = "PM";
    }
    if (h == 0) {
        h = 12;
    }
    m = m<10?"0"+m:m;
    s = s<10?"0"+s:s;
    
    var pattern = new RegExp("0?"+hh+":"+m+":"+s);

    var replacement = h+":"+m+":"+s;   
    replacement += " "+dd;    
    
   // return date.replace(pattern,replacement);

    return(replacement);
    
    
}
function sortRoomView(sortCol)
{ 
    document.getElementById("divLoading_RoomView").style.display = "";    
    
    if(SortColumn != sortCol)
    {
        SortOrder = "desc";
        SortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(SortOrder == "desc")
        {
           
            SortOrder = "asc";
            SortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(SortOrder == "asc")
        {
            SortOrder = "desc";
            SortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }
    if(sortCol != "")
    {
        SortColumn = sortCol;
    }

    roomListView(g_Curr, g_MapUnitId);

 } 
 
function load_UnitDetails(g_MonitorRoot,accesspointRootpr,zoneRootpr) 
{
    var UnitArr = new Array();
    var UnitNameArr= new Array();
    var strUnitName="";
    var O_UnitName=$(g_MonitorRoot).children().filter('UnitName');  
    var O_APUnitName=$(accesspointRootpr).children().filter('UnitName'); 
    var O_ZoneUnitName=$(zoneRootpr).children().filter('UnitName'); 
    
    var nRootLength = O_UnitName.length; 
    var nAPRootLength = O_APUnitName.length; 
    var nZoneRootLength = O_ZoneUnitName.length;     

    
    //$( "#txtUnitName" ).autocomplete( "destroy" );
    //$ ('#txtinput').unbind('.autocomplete').autocomplete() ;
    $( "#txtinput" ).autocomplete( "destroy" );
       
    document.getElementById("txtUnitName").innerHTML ="";
   
    if (nRootLength > 0) 
    {      
        for (var i = 0; i < nRootLength; i++)
         {
            var UnitName = (O_UnitName[i].textContent || O_UnitName[i].innerText || O_UnitName[i].text);
            
            UnitName = setundefined(UnitName);
            
            if(UnitName != "")
            {
                 if(strUnitName=="")
                    strUnitName=UnitName;         
                 else
                    strUnitName=strUnitName + ',' + UnitName ;        
            }
         }         
    }
    
    if (nAPRootLength > 0) 
    {      
        for (var i = 0; i < nAPRootLength; i++)
         {
            var UnitName = (O_APUnitName[i].textContent || O_APUnitName[i].innerText || O_APUnitName[i].text);
            
            UnitName = setundefined(UnitName);
            
            if(UnitName != "")
            {
                 if(strUnitName=="")
                    strUnitName=UnitName;         
                 else
                    strUnitName=strUnitName + ',' + UnitName ;        
            }
         }         
    }
    
    if (nZoneRootLength > 0) 
    {      
        for (var i = 0; i < nZoneRootLength; i++)
         {
            var UnitName = (O_ZoneUnitName[i].textContent || O_ZoneUnitName[i].innerText || O_ZoneUnitName[i].text);
            
            UnitName = setundefined(UnitName);
            
            if(UnitName != "")
            {
                 if(strUnitName=="")
                    strUnitName=UnitName;         
                 else
                    strUnitName=strUnitName + ',' + UnitName ;        
            }
         }         
    }
    
    UnitArr = strUnitName.split(",");
    var uniqueUnitName=UnitArr.filter(function(itm,i,UnitArr)
    {
        return i==UnitArr.indexOf(itm);
    });  
  
    if (uniqueUnitName.indexOf(",") > -1)
    {      
        UnitNameArr = uniqueUnitName.split(",");
    }
    else
    {
        UnitNameArr = uniqueUnitName;
    }
    
    $( "#txtUnitName" ).autocomplete({
      source: UnitNameArr
    });
    
   /* $('#txtUnitName').selectToAutocomplete();
    document.getElementById("txtinput").value = "";   */         
}
     

                   
                   
                   
                   
      
