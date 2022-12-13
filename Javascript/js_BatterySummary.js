// JScript File
var floorId = 0;
var g_pie_chart_loaded = 0;
var enumMapType = { "Campus": 1, "Building": 2, "Floor": 3, "Unit": 4, "Room": 5 }

$(document).on('change', '#selCampus', function() {
    getRootElement(enumMapType.Building);
});
$(document).on('change', '#selBuilding', function() {
    getRootElement(enumMapType.Floor);     
});

function loadunits(floorid,floorname)
{
   $select = $("#selUnits"); 
   $select.empty();   
   if (floorid != null) 
   {  
   for(var i = 0; i <= floorid.length - 1; i++)
     {
         var rootObj = $(g_MPRoot).find("Unit").children().filter("FloorId_Unit").filter(function () { return $( this ).text() == String(floorid[i]);}).parent();
         ids = rootObj.children().filter("UnitId");
         names = rootObj.children().filter("UnitName");
         descriptions = rootObj.children().filter("UnitDescription");
         images = rootObj.children().filter("RoomCsvFilePath");           
         loadUnitDropdown(ids,names,floorid[i],floorname[i]);
    } 
  }
  else
  {
    $('#selUnits').multipleSelect('refresh');   
  }                         
}

function loadMapsDropdown()
{   
    getRootElement(enumMapType.Campus);
    getRootElement(enumMapType.Building);
    getRootElement(enumMapType.Floor);
    getRootElement(enumMapType.Unit);
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
        $("#selUnits").empty();  
        ids = g_MPRoot.getElementsByTagName('CampusId');
        names = g_MPRoot.getElementsByTagName('CampusName');
        descriptions = g_MPRoot.getElementsByTagName('CampusDescription');
        images = g_MPRoot.getElementsByTagName('CampusImage');
        combobox = $("#selCampus");       
        loadintoCombobox(ids,names,combobox);
    }
    if(mapType === enumMapType.Building)
    {
        $("#selBuilding").empty();
        $("#selFloor").empty();
        $("#selUnits").empty();  
        refId = $("#selCampus").val();       
        var rootObj = $(g_MPRoot).find("Building").children().filter("CampusId_Building").filter(function () { return $( this ).text() == String(refId);}).parent();
        ids = rootObj.children().filter("BuildingId");
        names = rootObj.children().filter("BuildingName");
        descriptions = rootObj.children().filter("BuildingDescription");
        images = rootObj.children().filter("BuildingImage");
        combobox = $("#selBuilding");
        loadintoCombobox(ids,names,combobox);
    }      
    if(mapType === enumMapType.Floor)
    {
        $("#selFloor").empty();
        $("#selUnits").empty();  
        refId = $("#selBuilding").val();       
        var rootObj = $(g_MPRoot).find("Floor").children().filter("BuildingId_Floor").filter(function () { return $( this ).text() == String(refId);}).parent();
        ids = rootObj.children().filter("FloorId");
        names = rootObj.children().filter("FloorName");
        descriptions = rootObj.children().filter("FloorDescription");
        images = rootObj.children().filter("svgFile");            
        loadFloorDropdown(ids,names);   
    }  
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

//load Unit into Dropdown
function loadUnitDropdown(ids,names,floorid,floorname)
{
   $select = $("#selUnits"); 
        
    var $optgroup = $("<optgroup>", {"id": floorid, "label": floorname });        
    
    for(var idx = 0; idx <= ids.length - 1; idx++)
    {
        var id = getTagNameValue(ids[idx]);
        var name = getTagNameValue(names[idx]);    
                                                       
        $optgroup.append($("<option>", {"value": id, "text": name, "selected": "selected" }));
        
        $('#selUnits').append($optgroup);
    }
    $('#selUnits').multipleSelect('refresh');   
}

//get value from htmlCollection
function getTagNameValue(nodeElem)
{
    return (nodeElem.textContent || nodeElem.innerText || nodeElem.text || nodeElem.innerHTML);
}

//load unit into Dropdown
function loadFloorDropdown(ids,names)
{   
    $select = $("#selFloor");
    $select.empty();   
    for(var idx = 0; idx <= ids.length - 1; idx++)
    {
        var id = getTagNameValue(ids[idx]);
        var name = getTagNameValue(names[idx]);      
        $select.append($("<option>", {"value": id, "text": name }));
    }
    $('#selFloor').multipleSelect('refresh');    
}

//Fill ComboBox
var g_MPObj;
var g_MPRoot;
function loadCmbBatterySummary(SiteId)
{
    if(g_MPObj != null) { g_MPObj = null; }
    g_MPObj = CreateXMLObj();    
     document.getElementById("divLoading_BatteryOverview").style.display = "";    
    if(g_MPObj!=null)
    {
        g_MPObj.onreadystatechange = ajaxloadCmbBatterySummary;
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadMapListView&Site=" + SiteId;
        if(GetBrowserType()=="isIE")
        {
            g_MPObj.open("GET",DbConnectorPath, false);
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
function ajaxloadCmbBatterySummary()
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
            var rootObj = $(g_MPRoot).find("Floor").children().filter("FloorId");                   
            if(o_campusId.length > 0)
            {
                getRootElement(enumMapType.Campus);
            }           
          }       
    }        
}

var DeviceName ='';

//*********************************************************
//	Function Name	:	doLoadTagBatterySummary
//	Input			:	SiteId,DeviceType,DeviceId,DateRangeType,FromDate,PgType
//	Description		:	ajax Call DeviceGraph
//*********************************************************
function doLoadTagBatterySummary(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{      
    BatterySummaryGraph(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId));
          
    DeviceName=strbatterytypename;
}

//*********************************************************
//	Function Name	:	doLoadInfraBatterySummary
//	Input			:	SiteId,DeviceType,DeviceId,DateRangeType,FromDate,PgType
//	Description		:	ajax Call DeviceGraph
//*********************************************************
function doLoadInfraBatterySummary(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{ 
    BatterySummaryGraph(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId));
    DeviceName=strbatterytypename;
    
}

//*********************************************************
//	Function Name	:	DeviceGraph
//	Input			:	SiteId,DeviceType,DeviceId,DateRangeType,FromDate,PgType
//	Description		:	ajax Call DeviceGraph
//*********************************************************


function BatterySummaryGraph(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{   
    g_DeviceGraphObj = CreateDeviceXMLObj();
    
    document.getElementById("divchartBatterySummary").innerHTML = "";
       
    if(g_DeviceGraphObj!=null)
    {
        g_DeviceGraphObj.onreadystatechange = ajaxDeviceGraphBattSummary;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=BatterySummary&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_DeviceGraphObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_DeviceGraphObj.open("GET",DbConnectorPath, true);
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
 var dsRoot;
function ajaxDeviceGraphBattSummary()
{
    if(g_DeviceGraphObj.readyState==4)
    {
        if(g_DeviceGraphObj.status==200)
        {              
             dsRoot = g_DeviceGraphObj.responseXML.documentElement;
            
            g_dsRoot = dsRoot;
         
            LoadTagBatterySummaryGraph(dsRoot);      
                                  
        }
    }
}

//*********************************************************
//	Function Name	:	GenerateMonthlyXML
//	Input			:	dsRoot, Type
//	Description		:	Make XML String for Tag & Monitor Graph for Month from ajax Response
//*********************************************************

function LoadTagBatterySummaryGraph(dsRoot)
{
    var nRootLength;
    var n;
      
    var Year;
    var Quantity;
    var maxvalue;
    
    var sCategory = "";   
    var sXML = "";
    var strGraphData = "";
    var maxyvalue=0;
    var prevoiusYear;
    
    var o_Year = dsRoot.getElementsByTagName('Estimated');
    var o_Quantity = dsRoot.getElementsByTagName('Quantity');
        
    nRootLength = o_Year.length;
    n = nRootLength - 1;    
      
    g_pie_chart_loaded = 0;
         
    //Graph XML
    if(nRootLength > 0)
    {
        for(var i = 0; i <= nRootLength - 1; i++)
        {
            Year = setundefined(o_Year[i].textContent || o_Year[i].innerText || o_Year[i].text);
            Quantity = setundefined(o_Quantity[i].textContent || o_Quantity[i].innerText || o_Quantity[i].text);                              
                   
          if (prevoiusYear !=Year)
          {          
            if (Year==0.5)
            {             
              sCategory =sCategory + "<set label='6 Month" + "' value='" + Quantity + "' color='245e90' link='j-MakePieChartBattSummary-" + Year + "'/>" ;
            }  
            else
            {              
              if (Year=="N/A")
              {            
                sCategory =sCategory + "<set label='" + Year + "' value='" + Quantity + "' color='245e90' />" ; 
              }
              else if (Year==5.5)
              {
                sCategory =sCategory + "<set label='5Yrs +" + "' value='" + Quantity + "' color='245e90' link='j-MakePieChartBattSummary-" + Year + "' />" ; 
              }
              else 
              {
                sCategory =sCategory + "<set label='" + Year + "Yr" + "' value='" + Quantity + "' color='245e90' link='j-MakePieChartBattSummary-" + Year + "' />" ; 
              }            
            }
            prevoiusYear=Year;
          }            
                        
        if(parseInt(Quantity) > maxyvalue) 
        {
            maxyvalue = Quantity;
        }           
       }       
      MakeChartBattSummary(sCategory,maxyvalue);         
    }
    else
    {
        MakeChartBattSummary("","");
    }    
}

//********************************************************
//	Function Name	:	MakeChart
//	Input			:	dsRoot, Type
//	Description		:	Make Chart from XML String
//*********************************************************
var piechartlbl="";
function MakeChartBattSummary(sCategory,maxyvalue)
{
            document.getElementById('divTableBatterySummary').style.display = "none";  
            document.getElementById('divchartBatterySummary').style.display = "";           
            document.getElementById("divLoading_BatteryOverview").style.display = "";
            document.getElementById("divPieChartBatterySummary").style.display = "none"; 
             
  var sXML;    
  var divline;
  var scon='';
  var MaxY='';
  scon='';  
  
 /*if (maxyvalue>=3000)
  {  
    divline =Math.round(maxyvalue/500);
    MaxY=divline*500;
    scon="numDivLines='"+ divline +"'"; 
    scon+=" yAxisMaxValue='" + MaxY + "'";  
 }
 else
 {
   scon="numDivLines='5'";    
   scon+=" yAxisMaxValue='3000'";   
 }*/  
 
  sXML ="<chart caption='" + DeviceName + "' subCaption='Remaining Battery Life Summary' xAxisName='Year' yAxisName='Quantity' showValues='0' " + scon + " showPlotBorder='0' yAxisMinValue='0' formatNumberScale='0' " +
    "plotGradientColor='' animation='0' labelDisplay='Rotate' slantLabels='1' bgColor='ffffff' canvasBorderThickness='0' canvasBgColor='ffffff' " +
   " showAlternateHGridColor='0' plotSpacePercent='60' formatNumber='0' showVLineLabelBorder='1' rotateYAxisName='0' >" + sCategory +             
          " <styles>" +
    "  <definition>" +
           " <style name='myCaptionFont' type='font' font='Arial' size='16' color='245e90' bold='1' />" +
           " <style name='myAxisTitlesFont' type='font' font='Arial' size='14' bold='1' color='245e90' />" + 
           " <style name='mylblFont' type='font' font='Arial' size='14' color='#000000' bold='1'/>" + 
           " <style name='subCaptionFont' type='font' font='Arial' size='16' color='245e90' bold='0'/>" + 
     " </definition>" +
     " <application>" +
           " <apply toObject='caption' styles='myCaptionFont' />" +
           " <apply toObject='yAxisName' styles='myAxisTitlesFont' />" +
           " <apply toObject='XAxisName' styles='myAxisTitlesFont' />" +
           " <apply toObject='datalabels' styles='mylblFont' />" +
           " <apply toObject='subCaption' styles='subCaptionFont' />" +
     " </application>" +
 " </styles>" +
        "</chart>";
             if ( FusionCharts("myChartId") ) FusionCharts("myChartId").dispose(); 
                   FusionCharts.setCurrentRenderer('javascript');
                   var myChart=new FusionCharts("Column2D","myChartId","700","450","0","1");
                   myChart.setDataXML(sXML);
                   myChart.render("divchartBatterySummary");
                   document.getElementById("divLoading_BatteryOverview").style.display = "none";
                   
 }

function MakePieChartBattSummary(nYear)
{
    piechartlbl="";
    
    var nlength;
    var o_Estimated_Life_Span = $(dsRoot).find("Estimated").filter("Estimated").filter(function () { return $( this ).text() == String(nYear);}).parent(); 
    
    nlength=o_Estimated_Life_Span.length;
    
    if (nlength >0)
    {        
      for(var i=0; i<nlength; i++)
       {     
         var Year=$(o_Estimated_Life_Span).children().filter('Est_Bat_LifeSpan')[i].textContent || $(o_Estimated_Life_Span).children().filter('Est_Bat_LifeSpan')[i].innerText || $(o_Estimated_Life_Span).children().filter('Est_Bat_LifeSpan')[i].text;
         var Quantity=$(o_Estimated_Life_Span).children().filter('Est_Bat_LifeSpan_Quantity')[i].textContent || $(o_Estimated_Life_Span).children().filter('Est_Bat_LifeSpan_Quantity')[i].innerText || $(o_Estimated_Life_Span).children().filter('Est_Bat_LifeSpan_Quantity')[i].text;
               
         if (Year==0.5 && parseInt(Quantity)>0)
          {                 
            piechartlbl="<set labelFontBold='1'  label='" + Quantity + " @ 0.5 Yr Avg. Life' value='" + Quantity + "' showNames='0' showValues='0' link='j-BackGraph-1'/>" ;
          }  
         else 
           {
               if (parseInt(Quantity)>0)
                {
                     if (Year=="N/A")
                     {
                      piechartlbl=piechartlbl + "<set labelFontBold='1'  label='" + Quantity + " @ " + Year + "' value='" + Quantity + "' showNames='0' showValues='0' />" ; 
                     }
                     else
                     {
                       piechartlbl=piechartlbl + "<set labelFontBold='1'  label='" + Quantity + " @ " + Year + "Yr " + "Avg. Life' value='" + Quantity + "' showNames='0' showValues='0' link='j-BackGraph-1' />" ; 
                     }       
                }            
          }
       }
    }     
    
    document.getElementById("divPieChartBatterySummary").style.display = ""; 
    document.getElementById('btnBackToBarGraph').style.display=""; 
    
    FlipCharts();  
     
    var sXML;   
         
      sXML ="<chart animation='0' caption='" + DeviceName + "' subCaption='Estimated Battery Life Summary' smartLabelClearance='10' slicingDistance='0' enableRotation='1' bgColor='#ffffff' showLabels='1' showBorder='1' use3DLighting ='0' paletteColors= '#008299,#00A600,#00A0B1,#008A00,#2672EC,#77B900,#094AB2,#2E8DEF,#F3B200,#6A46C1,#952D2D,' showLegend= '0' showShadow='0' showPercentageValues='1'>" + piechartlbl + 
           " <styles>" +
    "  <definition>" +
           " <style name='myCaptionFont' type='font' font='Arial' size='16' color='245e90' bold='1' />" +
           " <style name='myAxisTitlesFont' type='font' font='Arial' size='14' bold='1' color='245e90' />" + 
           " <style name='lblFont' type='font' font='Arial' size='16' color='#000000' bold='1'/>" + 
           " <style name='subCaptionFont' type='font' font='Arial' size='16' color='245e90' bold='1'/>" + 
     " </definition>" +
     " <application>" +
           " <apply toObject='caption' styles='myCaptionFont' />" +
           " <apply toObject='yAxisName' styles='myAxisTitlesFont' />" +
           " <apply toObject='XAxisName' styles='myAxisTitlesFont' />" +
           " <apply toObject='datalabels' styles='lblFont' />" +
           " <apply toObject='subCaption' styles='subCaptionFont' />" +
     " </application>" +
 " </styles>" +
      "</chart>";
            if (FusionCharts("myPieChartId")) FusionCharts("myPieChartId").dispose(); 
            FusionCharts.setCurrentRenderer('javascript');
            
            var PieChart=new FusionCharts("pie2d","myPieChartId","700","450");
            PieChart.setDataXML(sXML);
            PieChart.render("divPieChartBatterySummary");
}  
        
    
function BackGraph()
{
    document.getElementById('btnBackToBarGraph').style.display="none"; 
    FlipCharts();
}

function FlipCharts()
{
    var page1 = $('.page1');
    var page2 = $('.page2');
    var toHide = page1.is(':visible') ? page1 : page2;
    var toShow = page2.is(':visible') ? page1 : page2;
        
    toHide.removeClass('flip in').addClass('flip out').hide();
    toShow.removeClass('flip out').addClass('flip in').show();   
}



function doLoadTagExcelBatterySummary(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{ 
    doLoadExcelBattery(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId));
}

function doLoadInfraExcelBatterySummary(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{ 
    doLoadExcelBattery(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId));
}

function doLoadExcelBattery(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{
    if(g_Obj == null)
    {
        g_Obj = CreateTagXMLObj();
    }      
    
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxDownLoadBatteryinforamtion;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadExcelBatterySummary&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_Obj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_Obj.open("GET",DbConnectorPath, true);
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
function ajaxDownLoadBatteryinforamtion()
{
    if(g_Obj.readyState==4)
    {
        if(g_Obj.status==200)
        {
            var dsRoot = g_Obj.responseXML.documentElement;
            
            if(dsRoot != null)
            {
                var o_Excel=dsRoot.getElementsByTagName('Excel');
                var o_Filename=dsRoot.getElementsByTagName('Filename');
                
                var Excel=(o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
                var Filename=(o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);
                
                //Export table string to CSV
                tableToCSV(Excel, Filename);  
                              
                document.getElementById("divExcelReportLoading").style.display = "none";

            }
        }
    }
}

function doLoadTagExcelBatterySummary_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{ 
    doLoadExcelBattery_IE(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId));
}

function doLoadInfraExcelBatterySummary_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{ 
    doLoadExcelBattery_IE(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId));
}

function doLoadExcelBattery_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId)
{
    location.href = "AjaxConnector.aspx?cmd=LoadExcelBatterySummary_ForIE&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId;
}





