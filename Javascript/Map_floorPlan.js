//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////                 FLOOR PLAN                 ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//  CONSTANTS
var STARHIGHLIGHT_LAYER_RADIUS = 75;
var REPORTLAYER_RADIUS  = 50;
var SEARCH_REPORTLAYER_RADIUS  = 25;

var DISTANCE_AWAY_FROM_STAR  = 50;

//  LAYERS
var tagMarKerLayer,starMarKerLayer,Rooms,WifiZoneLayer,snapGridLayer,snapControl,starRangeLayer,starMonitorLinkLayer,starTagLinkLayer,multiTagLayer,addMonitorLayer;
var InfraStructureHighLightLayer;

var map,popup,controls;
var polygonPoints ='';

var LAyersArray   =  new Array ();
var tagDetailsArr =  new Array ();
var deviceInfoArr =  new Array ();
var roomDetails   =  new Array ();
var roomCoordinatesArr = new Array() ;
var InfraStructureCoordinatesArr = new Array() ;
var InfraStructureResponseArr = new Array() ;
var tagInvisibleLayerArray = new Array();
var taglayersArray = new Array();
var editModelayersArray = new Array();  //for edit Monitors

var multiTagsLoadedRoomsArray = new Array();
var existingTagStarLinkLayer = new Array();
var reportLayers = new Array();
var layerswitcher;

var imageWidth, ImageHeight;
var HighLightedRoom;
var mouseclick;
var UpdateTimer;
var selectedFeature;
var PreviousStyleForHighlighedRoom;
var g_starRoot,g_monitorRoot,g_accessPointRoot;
var g_wifiZones;
var tempEventForTag;
var isInfraStructureLoaded = false;
var isMapMoves = false;

var defaultRoomStyle = OpenLayers.Util.applyDefaults(defaultRoomStyle, OpenLayers.Feature.Vector.style['default']);
defaultRoomStyle.fill = true;
defaultRoomStyle.fillColor = "#FFCC66";
defaultRoomStyle.fillOpacity = 0.6;

var defaultZoneStyle = OpenLayers.Util.applyDefaults(defaultZoneStyle, OpenLayers.Feature.Vector.style['default']);
defaultZoneStyle.fill = true;
defaultZoneStyle.fillColor = "#01B928";
defaultZoneStyle.fillOpacity = 0.6;
defaultZoneStyle.strokeColor= "#045104";
defaultZoneStyle.strokeOpacity= 1;
defaultZoneStyle.strokeDashstyle= "dash";
defaultZoneStyle.strokeWidth= 2;

var highlightedRoomStyle = OpenLayers.Util.applyDefaults(highlightedRoomStyle, OpenLayers.Feature.Vector.style['default']);
highlightedRoomStyle.fill = true;
highlightedRoomStyle.fillColor = "#F4E003";
highlightedRoomStyle.fillOpacity = 0.8;


var highlightedZoneStyle = OpenLayers.Util.applyDefaults(highlightedZoneStyle, OpenLayers.Feature.Vector.style['default']);
highlightedZoneStyle.fill = true;
highlightedZoneStyle.fillColor = "#FF69B4";
highlightedZoneStyle.fillOpacity = 0.7;
highlightedZoneStyle.strokeColor= "#024402";
highlightedZoneStyle.strokeOpacity= 1;
highlightedZoneStyle.strokeWidth= 2;


var zoomify_url='';
var zoomify_width;
var zoomify_height;
var widthratio,heightratio;


var rectControl;
var polygonControl;


var initialDevicecoords_x;
var initialDevicecoords_y;
var unSavedfeatues = new Array();
var lastDrawnRoom;
var lastDrawnDevice;
var graphic;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////      LOAD FLOOR, INFRASTRUCTURE AND TAGS   ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function init()
{
    
    //LoadFloorMap(document.getElementById('map'),"Basement-New.svg","Basement.png");
    UpdateTags(tagDetailsArr);
   
}


function LoadFloorMap(mapid,svgFileUrl,floorBgFileUrl,nimageWidth,nImageHeight)
{
     lastDrawnDevice = null;
     lastDrawnRoom = null;
     
     if(floorBgFileUrl.length == 0)
        g_floorPlanBgLoaded = 1;
        
     if (svgFileUrl.length == 0)
     {
        if(parseInt(nimageWidth) < 900)
        {
           imageWidth = parseInt(nimageWidth);
           ImageHeight = parseInt(nImageHeight);
        }
        else
        {
           imageWidth = 900;
           if(nImageHeight.length > 0)
           {
                ImageHeight = parseInt(nImageHeight);
                ImageHeight *= (imageWidth / parseInt(nimageWidth));
                ImageHeight = Math.round(ImageHeight);
           }
           else
           {
                ImageHeight=0;
           }
           
        }
     }
     
     selectedFeature = null;
     g_oldDeviceId='';
     
     mapid.innerHTML = "";
     STARHIGHLIGHT_LAYER_RADIUS = 75;
     for(var i = 0; i < LAyersArray.length ; i++)
     {
        if(LAyersArray[i]!=undefined)
        {
        LAyersArray[i].destroyFeatures();
        }
     }
     
     for(var i=0;i<taglayersArray.length;i++)
    {
    if(taglayersArray[i]!=undefined)
        {
        taglayersArray[i].destroyFeatures();
        }
    } 
     taglayersArray.splice(0,taglayersArray.length);
    
     editModelayersArray.splice(0,editModelayersArray.length);
     unSavedfeatues.splice(0,unSavedfeatues.length);
    
     reportLayers.splice(0,reportLayers.length);
     tagDetailsArr.splice(0,tagDetailsArr.length);
     tagInvisibleLayerArray.splice(0,tagInvisibleLayerArray.length);

     if(multiTagLayer)
     
        multiTagLayer.destroyFeatures();
     
     if(starRangeLayer)
     {
        starRangeLayer.destroyFeatures();
        
     }
     if(starMonitorLinkLayer)
     {
        starMonitorLinkLayer.destroyFeatures();
     }
     if(starTagLinkLayer)
     {
        starTagLinkLayer.destroyFeatures();
     }
     isInfraStructureLoaded = false;
     isMapMoves = false;
        
     roomDetails  = new Array() ;
     roomCoordinatesArr = new Array() ;
     InfraStructureCoordinatesArr = new Array();
     LAyersArray = new Array();

    
    
      OpenLayers.Control.Click = OpenLayers.Class(OpenLayers.Control, {                
                defaultHandlerOptions: {
                    'single': true,
                    'double': false,
                    'pixelTolerance': 0,
                    'stopSingle': false,
                    'stopDouble': false
                },

                initialize: function(options) {
                    this.handlerOptions = OpenLayers.Util.extend(
                        {}, this.defaultHandlerOptions
                    );
                    OpenLayers.Control.prototype.initialize.apply(
                        this, arguments
                    ); 
                    this.handler = new OpenLayers.Handler.Click(
                        this, {
                            'click': this.trigger
                        }, this.handlerOptions
                    );
                }, 

                trigger: function(e) {
                
                   
                    var lonlat = map.getLonLatFromPixel(e.xy);
                    
                     var pixel = e.xy;
                     var location = map.getLonLatFromPixel(pixel);
                     
                      
                      if(addMonitorLayer)
                      {
                               if(lastDrawnDevice) //clear previously drawn unsaved device
                               {
                                    if(selectedFeature == lastDrawnDevice)
                                        selectedFeature = null;

                                  addMonitorLayer.removeFeatures([lastDrawnDevice]);
                               }
                        
                              var imgDetail;
                              var fDescription="";
                              
                              if(g_svgDType == 1)
                              {
                                  imgDetail =  getDeviceImage( 'Monitor', 'Monitor');
                                  fDescription = "Infrastructure-Monitor";
                              }
                              else if(g_svgDType == 2)
                              {
                                  imgDetail = getDeviceImage( 'Star', 'Star');
                                  fDescription = "Infrastructure-Star";
                              }
                               else if(g_svgDType == 3)
                              {
                                 imgDetail =  getDeviceImage( 'Accesspoint', 'Accesspoint');
                                  fDescription = "Infrastructure-AccessPoints";
                              }
                                                  
                               var feature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point(location.lon,location.lat).transform(epsg4326, projectTo));
                                
                                  feature.attributes = {
                                  
                                  align: "cm",
                                  labelYOffset: -(imgDetail[0].imageheight),
                                  display: "block",
                                  externalGraphic :imgDetail[0].imageName,
                                  graphicHeight: imgDetail[0].imageheight,
                                  graphicWidth: imgDetail[0].imageWidht,
                                  graphicXOffset:-(imgDetail[0].imageWidht/2),
                                  graphicYOffset:-(imgDetail[0].imageheight/2),
                                  description: fDescription,
                                  name: ''
                                  
                                  };
                                                  
                                                  
                                monitorX = location.lon;
                                monitorY = location.lat;   
                                
                                monitorX = convertToSvgEcllipseXCoordinate(parseFloat(monitorX));
                                monitorY = convertToSvgEcllipseYCoordinate(parseFloat(monitorY));
                                monitorW = 15;
                                monitorH = 15;
                                 if (navigator.userAgent.indexOf('MSIE') != -1)                                    
                                         feature.attributes.fillOpacity = 1.0;                                    
                                                         
                                addMonitorLayer.addFeatures(feature);
                                unSavedfeatues[unSavedfeatues.length] = feature;
                                lastDrawnDevice = feature;
                                mouseclick.deactivate();
                                controls['selector'].activate();
                                
                                getFeatureLocationInFeet(feature);
                                                  
                      }
                
                }

            });
     
     if(svgFileUrl.length > 0)
     {
         if (navigator.userAgent.indexOf('Chrome') != -1 || navigator.userAgent.indexOf("Firefox") != -1 || navigator.userAgent.indexOf('Safari') != -1)
        {
    
             readAndParseSVGFile(svgFileUrl);
        }
        else
        {
   
        readAndParseSVGFileforIE(svgFileUrl);
        }              
     }      
   
   if(zoomify_url != undefined && zoomify_url.length > 0)
   {
    /* First we initialize the zoomify pyramid (to get number of tiers) */
        var zoomify = new OpenLayers.Layer.Zoomify( "Floor Plan", zoomify_url,
	  		new OpenLayers.Size( zoomify_width, zoomify_height ) );

       /* Map with raster coordinates (pixels) from Zoomify image */
        var options = {
                        controls: [
                            new OpenLayers.Control.Navigation(),
                            new OpenLayers.Control.PanZoom(),
                            new OpenLayers.Control.NavToolbar()
                            ],
                        maxExtent: new OpenLayers.Bounds(0, 0, zoomify_width, zoomify_height),
                        maxResolution: Math.pow(2, zoomify.numberOfTiers-1 ),
                        numZoomLevels: zoomify.numberOfTiers,
                        units: 'pixels',
                        eventListeners: {                                                                         
                                        "changelayer": mapLayerChanged
                                    }
                                  
        };

        map = new OpenLayers.Map("map", options);
        map.addLayer(zoomify);

        map.setBaseLayer(zoomify);
        map.zoomToMaxExtent();
   }
   else
   {
        map = new OpenLayers.Map('map', {
                             controls: [
                                        new OpenLayers.Control.Navigation(),
                                        new OpenLayers.Control.PanZoom(),
                                        new OpenLayers.Control.NavToolbar(),
                                        new OpenLayers.Control.Attribution()
                                        ],
                             numZoomLevels: 5
                             ,
                             eventListeners: {                                                                         
                                        "changelayer": mapLayerChanged
                                    }}
                                  );
                             
        graphic = new OpenLayers.Layer.Image(
                                             'Floor Plan',
                                             floorBgFileUrl,
                                             new OpenLayers.Bounds(0, 0, imageWidth, ImageHeight),
                                             new OpenLayers.Size(imageWidth, ImageHeight),
                                             {numZoomLevels: 5}
                                             );
    
        graphic.events.on({
                      loadstart: function() {
                        g_floorPlanBgLoaded = 0;
                      },
                      loadend: function() {
                        g_floorPlanBgLoaded = 1;
                        if(g_isInfrasturctureLoaded == 1)
                            document.getElementById("divLoadingMap").style.display="none";
                      }
                      });      
                      
        map.addLayers([graphic]);                           
       
   }
  
    mouseclick = new OpenLayers.Control.Click();
    map.addControl(mouseclick);
    mouseclick.deactivate();
   
    
    map.events.register("movestart", null, function(evt) { 
                    isMapMoves = true;                   
                } 
            )
            
    map.events.register("moveend", null, function(evt) { 
                    isMapMoves = false;                
                } 
            )
    
    
    
    //////////////////////////////////////////////////////////////////////////
    
    Rooms  = new OpenLayers.Layer.Vector( "Infrastructure - Rooms" ,{
                                                  styleMap: new OpenLayers.StyleMap({'default':{
                                                                                    strokeColor: "#00FF00",
                                                                                    strokeOpacity: 1,
                                                                                    strokeWidth: 3,
                                                                                    fillColor: "#FF5500",
                                                                                    fillOpacity: 0.7,
                                                                                    pointRadius:  "${Radius}",
                                                                                    pointerEvents: "visiblePainted",
                                                                                    // label with \n linebreaks
                                                                                    label : "${name}",
                                                                                    fontColor: "${favColor}",
                                                                                    fontSize: "15px",
                                                                                    fontFamily: "Courier New, monospace",
                                                                                    fontWeight: "bold",
                                                                                    labelAlign: "${align}",
                                                                                    labelXOffset: "${xOffset}",
                                                                                    labelYOffset: "${yOffset}",
                                                                                    labelOutlineColor: "white",
                                                                                    labelOutlineWidth: 0,
                                                                                    graphicZIndex: 0
                                                                                    }}),
                                                                                    
                                                   rendererOptions: { zIndexing: true },
                                                  renderers: renderer
                                                  });
    
    map.addLayers([Rooms]);
    if(floorBgFileUrl.length > 0)
        map.zoomToMaxExtent(5);
    
    //////////////////////////////////////////////////////////////////////////
    
    WifiZoneLayer  = new OpenLayers.Layer.Vector( "Infrastructure - Wifi Zones" ,{
                                         styleMap: new OpenLayers.StyleMap({'default':{
                                                                           strokeColor: "#00FF00",
                                                                           strokeOpacity: 1,
                                                                           strokeWidth: 3,
                                                                           fillColor: "#FF5500",
                                                                           fillOpacity: 0.7,
                                                                           pointRadius:  "${Radius}",
                                                                           pointerEvents: "visiblePainted",
                                                                           // label with \n linebreaks
                                                                           label : "${name}",
                                                                           fontColor: "${favColor}",
                                                                           fontSize: "15px",
                                                                           fontFamily: "Courier New, monospace",
                                                                           fontWeight: "bold",
                                                                           labelAlign: "${align}",
                                                                           labelXOffset: "${xOffset}",
                                                                           labelYOffset: "${yOffset}",
                                                                           labelOutlineColor: "white",
                                                                           labelOutlineWidth: 0,
                                                                           graphicZIndex: 0
                                                                           }}),
                                         
                                         rendererOptions: { zIndexing: true },
                                         renderers: renderer
                                         });
    
    map.addLayers([WifiZoneLayer]);
    editModelayersArray[editModelayersArray.length] = WifiZoneLayer;

    
    snapGridLayer = new OpenLayers.Layer.PointGrid({
                                                   name: "Snap Grid",
                                                   dx: 25, dy: 25,
                                                   styleMap: new OpenLayers.StyleMap({
                                                                                     pointRadius: 0.5,
                                                                                     strokeColor: "#0000FF",
                                                                                     strokeWidth: 0.5,
                                                                                     fillOpacity: 1,
                                                                                     fillColor: "#00FF00",
                                                                                     graphicName: "square"
                                                                                     })
                                                   });
    
    snapGridLayer.displayInLayerSwitcher = false;
    
    snapGridLayer.visibility = false;
    
    map.addLayer(snapGridLayer);
    
    
    var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
    renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;
    
    
    tagMarKerLayer = new OpenLayers.Layer.Vector("Infrastructure - Tags", {
                                                 styleMap: new OpenLayers.StyleMap({'default':{
                                                                                   strokeColor: "#00FF00",
                                                                                   strokeOpacity: 1,
                                                                                   strokeWidth: 3,
                                                                                   fillColor: "#FF5500",
                                                                                   fillOpacity: 0.5,
                                                                                   pointRadius: 0,
                                                                                   pointerEvents: "visiblePainted",
                                                                                   // label with \n linebreaks
                                                                                   label : "${name}",
                                                                                   
                                                                                   fontColor: "${favColor}",
                                                                                   fontSize: "15px",
                                                                                   fontFamily: "Courier New, monospace",
                                                                                   fontWeight: "bold",
                                                                                   labelAlign: "${align}",
                                                                                   labelXOffset: "${xOffset}",
                                                                                   labelYOffset: "${yOffset}",
                                                                                   labelOutlineColor: "white",
                                                                                   labelOutlineWidth: 0
                                                                                   }}),
                                                 renderers: renderer
                                                 });
    
    
    
    var editLayerstyle = new OpenLayers.Style(
                                     // the first argument is a base symbolizer
                                     // all other symbolizers in rules will extend this one
                                     {
                                         graphicWidth: "${graphicWidth}",
                                         graphicHeight: "${graphicHeight}",
                                         graphicYOffset: "${graphicYOffset}",
                                         graphicXOffset: "${graphicXOffset}",
                                         labelYOffset: "${labelYOffset}",
                                         labelXOffset: "${labelXOffset}",
                                         labelOutlineWidth: 1,
                                         fontColor: "#000000",

                                         fontOpacity: 1,
                                         fontSize: "10px",
                                         externalGraphic: "${externalGraphic}",
                                         color: "#000000",
                                         // shift graphic up 28 pixels
                                         label: "${name}", // label will be foo attribute value
                                         description : "${description}",
                                         strokeColor: "${strokeColor}",// "#3366FF",
                                         strokeOpacity: "${strokeOpacity}",
                                         strokeWidth: 3,
                                         fillColor:  "${fillColor}", //"#ADFF2F",
                                         fillOpacity: "${fillOpacity}",
                                         strokeDashstyle:"dash",
                                         graphicZIndex: 10
                                     });
    
    var editSelectLayerstyle = new OpenLayers.Style(
                                              // the first argument is a base symbolizer
                                              // all other symbolizers in rules will extend this one
                                              {
                                                  graphicWidth: "${graphicWidth}",
                                                  graphicHeight: "${graphicHeight}",
                                                  graphicYOffset: "${graphicYOffset}",
                                                  graphicXOffset: "${graphicXOffset}",
                                                  labelYOffset: "${labelYOffset}",
                                                  labelXOffset: "${labelXOffset}",
                                                  labelOutlineWidth: 1,
                                                  fontColor: "#000000",
                                                  
                                                  fontOpacity: 1,
                                                  fontSize: "10px",
                                                  externalGraphic: "${externalGraphic}",
                                                  color: "#000000",
                                                  // shift graphic up 28 pixels
                                                  label: "${name}", // label will be foo attribute value
                                                  description : "${description}",
                                                  strokeColor: "${strokeColor}",// "#336600",
                                                  strokeOpacity: "${strokeOpacity}",
                                                  strokeWidth: 3,
                                                  fillColor:  "${fillColor}", //"#ADFFF2",
                                                  fillOpacity: "${fillOpacity}",
                                                  strokeDashstyle:"dash",
                                                  graphicZIndex: 100
                                                    
                                              });
    
    
  addMonitorLayer = new OpenLayers.Layer.Vector("AddMonitor", { rendererOptions: { zIndexing: true },
                                                styleMap: new OpenLayers.StyleMap({'default':editLayerstyle , 'select':editSelectLayerstyle}),
                                                 renderers: renderer
                                                 });
    addMonitorLayer.displayInLayerSwitcher = false;                                       
    addRoomRectControl();
    addRoomPolygonControl();
    addDragForDrawnFeature();
    
                                                 
  starMarKerLayer = new OpenLayers.Layer.Vector("Infrastructure - Stars", {
                                                                 styleMap: new OpenLayers.StyleMap({'default':{
                                                                                                   strokeColor: "#3366FF",
                                                                                                   strokeOpacity: 0.5,
                                                                                                   strokeWidth: 3,
                                                                                                   fillColor: "#ADFF2F",
                                                                                                   fillOpacity: 0.4,
                                                                                                   pointRadius: "${Radius}",
                                                                                                   pointerEvents: "visiblePainted",
                                                                                                   // label with \n linebreaks
                                                                                                   label : "${name}",
                                                                                                   
                                                                                                   fontColor: "${favColor}",
                                                                                                   fontSize: "15px",
                                                                                                   fontFamily: "Courier New, monospace",
                                                                                                   fontWeight: "bold",
                                                                                                   labelAlign: "${align}",
                                                                                                   labelXOffset: "${xOffset}",
                                                                                                   labelYOffset: "${yOffset}",
                                                                                                   labelOutlineColor: "white",
                                                                                                   labelOutlineWidth: 0
                                                                                                   }}),
                                                                 renderers: renderer
             });
             
             
         var styleSel = new OpenLayers.Style({
            strokeColor: "#008000",
            strokeOpacity: 0.3,
            strokeWidth: 1.5,
            fillColor: "#ADFF2F",
            fillOpacity: 0.2,
            pointRadius: "${radius}",
            pointerEvents: "visiblePainted",
            display : "$(display)"         

        }, {
            context: {
                radius: function (feature) {
                    var pix = STARHIGHLIGHT_LAYER_RADIUS / map.getResolution() * .703125 ; // ten time the zoo level
                    return pix;
                }
            }
        });
             
             
          starRangeLayer = new OpenLayers.Layer.Vector("Analytics - Star Zones", {
                                                             styleMap: new OpenLayers.StyleMap({'default':styleSel})
                                                           });
    
    
    epsg4326 =  new OpenLayers.Projection("EPSG:4326"); //WGS 1984 projection
    projectTo = map.getProjectionObject(); //The map projection (Spherical Mercator)
    
    multiTagLayer =  new OpenLayers.Layer.Vector("Multi-Tag-Layer", {
                                                                 styleMap: new OpenLayers.StyleMap({'default':{
                                                                                                   strokeColor: "#00FF00",
                                                                                                   strokeOpacity: 1,
                                                                                                   strokeWidth: 3,
                                                                                                   fillColor: "#FF5500",
                                                                                                   fillOpacity: 0.5,
                                                                                                   pointRadius: 0,
                                                                                                   pointerEvents: "visiblePainted",
                                                                                                   // label with \n linebreaks
                                                                                                   label : "${name}",
                                                                                                   
                                                                                                   fontColor: "${favColor}",
                                                                                                   fontSize: "15px",
                                                                                                   fontFamily: "Courier New, monospace",
                                                                                                   fontWeight: "bold",
                                                                                                   labelAlign: "${align}",
                                                                                                   labelXOffset: "${xOffset}",
                                                                                                   labelYOffset: "${yOffset}",
                                                                                                   labelOutlineColor: "white",
                                                                                                   labelOutlineWidth: 0
                                                                                                   }}),
                                                                 renderers: renderer
             });
             
     

    
    for (var i = 0; i < roomCoordinatesArr.length; i++)
    {
   
        if(roomCoordinatesArr[i].length == 4)//(roomDetails[i][5] == 'none')
        {
            ext = roomCoordinatesArr[i];
            bounds = OpenLayers.Bounds.fromArray(ext);
            
            box = new OpenLayers.Feature.Vector(bounds.toGeometry());
            
            box.attributes = { "roomId" : roomDetails[i][4], "unique_id": i , "roomNO": "room"+ roomDetails[i][4]};
            
            box.style = defaultRoomStyle;
            
            Rooms.addFeatures(box);

        }
        else  if(roomCoordinatesArr[i].length == 5)
        {
                
                   var source =  roomCoordinatesArr[i][4];
               
                var polygonList = [];
                var pointList = [];
                 for (var j=0; j<source.length; j++) 
                {
                    var poloygonCoord = source[j].split(',');       
                    if(poloygonCoord[0] != '' && poloygonCoord[1] != '') 
                    {
                       var point = new OpenLayers.Geometry.Point(parseFloat(poloygonCoord[0]), getboxFromYpos(parseFloat(poloygonCoord[1]),ImageHeight));
                       pointList.push(point);
                    }        
                   
                }
                var linearRing = new OpenLayers.Geometry.LinearRing(pointList);
                var polygon = new OpenLayers.Geometry.Polygon([linearRing]);
                polygonList.push(polygon);
               
                multuPolygonGeometry = new OpenLayers.Geometry.MultiPolygon(polygonList);
                multiPolygonFeature = new OpenLayers.Feature.Vector(multuPolygonGeometry);
                multiPolygonFeature.style = defaultRoomStyle;
                multiPolygonFeature.attributes = { "roomId" : roomDetails[i][4], "unique_id": i , "roomNO": "room"+ roomDetails[i][4]};
                Rooms.addFeatures(multiPolygonFeature);
        }
       
        
            /*if(i > 20)
               tagDetailsArr[i] = {SvgId: roomDetails[i][4],tagID:i+1,TagTypeName:"Staff tag"};
            else
               tagDetailsArr[i] = {SvgId: roomDetails[i][4],tagID:i+1,TagTypeName:"Patient tag"};*/           
                
        
    }
    
    if(WifiZoneLayer.features.length == 0)
    {
        WifiZoneLayer.displayInLayerSwitcher = false;
    }
    else
    {
        WifiZoneLayer.displayInLayerSwitcher = true;
    }
  
    
    var layerswitcher = (OpenLayers.Control.LayerSwitcher) // layer swicher control reference
    OpenLayers.Lang[OpenLayers.Lang.getCode()]['Base Layer'] = "Map";
    // remove current instance of layers switcher from map if attached.
    map.addControl( new OpenLayers.Control.LayerSwitcher( { /* options here */ } ) );
    
    
    
    starMonitorLinkLayer = new OpenLayers.Layer.Vector("Analytics - Star-Monitor Link");
    starTagLinkLayer = new OpenLayers.Layer.Vector("Analytics - Star-Tag Link");
    
    map.addLayer(starMonitorLinkLayer);
    map.addControl(new OpenLayers.Control.DrawFeature(starMonitorLinkLayer, OpenLayers.Handler.Path));
    
    map.addLayer(starTagLinkLayer);
    map.addControl(new OpenLayers.Control.DrawFeature(starTagLinkLayer, OpenLayers.Handler.Path));
    
    map.events.register("buttonclick", null, function(event)
    {         
           if(!tempEventForTag)
               return;
               
           var res = tempEventForTag.layer.name.substring(0, 3);
           showOrHide_FetureInLayer(tempEventForTag.layer,true);
            
          if(res == 'Tag')
          {
//             var index =   tagInvisibleLayerArray.indexOf(tempEventForTag.layer.name.substring(6,  tempEventForTag.layer.name.length));
                var index=$.inArray(tempEventForTag.layer.name.substring(6,  tempEventForTag.layer.name.length).toString(),tagInvisibleLayerArray);
             
             if (index > -1)
             {               
                 if(tempEventForTag.layer.visibility == true)
                    tagInvisibleLayerArray.splice(index,1);
             }
             else if(tempEventForTag.layer.visibility == false)
             {
                   tagInvisibleLayerArray[tagInvisibleLayerArray.length] = tempEventForTag.layer.name.substring(6,  tempEventForTag.layer.name.length);
             }
                          
                  addTagMarkerinroom('0');
          }
       
    });
   
    
    map.div.oncontextmenu = function noContextMenu(e)
    {
        if(!e)
        { //dear IE...
            var e = window.event;
            e.returnValue = false;
        }
        
        var f = addMonitorLayer.getFeatureFromEvent(e);
        
        if(f)
          createPopupForRightClick(f);
        
        return false; //Prevent display of browser context menu
    }
    
    
   function mapLayerChanged(event) {
   
    
       var res = event.layer.name.substring(0, 3);
       
       tempEventForTag = event;     
   
       
       if(event.layer.name == "Reports - StarDensity" && event.layer.visibility == true)
       {
            starRangeLayer.setVisibility(false);
       }
       
       if(event.layer == starRangeLayer && event.layer.visibility == true)
       {   
           if(map.getLayersByName("Reports - StarDensity")[0])  
              map.getLayersByName("Reports - StarDensity")[0].setVisibility(false); 
       }
        
      if(event.layer.name.substring(0, 7) == 'Reports')
       {     
          hideLayersForReport(event.layer);
       }
         
     
      
          
       
      if(1)
      {
             var isNeedToRedraw = 0;
             var features =  starMonitorLinkLayer.getFeaturesByAttribute('monitorType',event.layer.name);   
               
              for(var t = 0 ; t < features.length; t++)
              {
                  isNeedToRedraw = 1;
                  if(event.layer.visibility == false)
                  {
                       features[t].style.display = 'none';
                       features[t].attributes['isShowMonitorLink'] = "no";
                  }
                  else
                  {
                       features[t].style.display = 'block';
                       features[t].attributes['isShowMonitorLink'] = "Yes";
                  }
              }  
               
               
              features =  starMonitorLinkLayer.getFeaturesByAttribute('starType',event.layer.name);   
               
              for(var t = 0 ; t < features.length; t++)
              {
                  isNeedToRedraw = 1;
                  if(features[t].attributes['isShowMonitorLink'] == "Yes")
                  {
                         if(event.layer.visibility == false)
                            features[t].style.display = 'none';
                         else
                            features[t].style.display = 'block';
                  }
              
              }  
               
             if(isNeedToRedraw == 1)
                 starMonitorLinkLayer.redraw();
        
               features =  starRangeLayer.getFeaturesByAttribute('starType',event.layer.name);   
               for(var t = 0 ; t < features.length; t++)
               {
                             if(event.layer.visibility == false)
                                  features[t].style = { visibility: 'hidden' }; 
                             else
                                  features[t].style = ''; 
                                
                              
               } 
            
             if(features.length > 0)
               starRangeLayer.redraw();
           
 
            
              features =  starTagLinkLayer.getFeaturesByAttribute('starType',event.layer.name);   
               
              for(var t = 0 ; t < features.length; t++)
              {
                              
                         if(event.layer.visibility == false)
                            features[t].style.display = 'none';
                         else
                            features[t].style.display = 'block';
                               
              }  
               
             if(features.length > 0)
               starTagLinkLayer.redraw();
          
          
          
          
              //density Layers
              
              for(var b = 0; b < reportLayers.length; b++)
              {
                  features =  reportLayers[b].getFeaturesByAttribute('Type',event.layer.name);
                  for(var t = 0 ; t < features.length; t++)
                  {
                      if(event.layer.visibility == false)
                          features[t].style = { visibility: 'hidden' };
                      else
                          features[t].style = '';
                      
                      
                  }
                  
                  if(features.length > 0)
                      reportLayers[b].redraw();
                      
                  
                  var isNeedToRedraw = 0;
                  var features =  reportLayers[b].getFeaturesByAttribute('FromType',event.layer.name);
                  
                  for(var t = 0 ; t < features.length; t++)
                  {
                      isNeedToRedraw = 1;
                      
                      var fromlayer = map.getLayersByName(features[t].attributes['FromType'])
                      var tolayer = map.getLayersByName(features[t].attributes['ToType'])
                      
                      if(features[t].attributes['isCircle']) 
                      {
                          if(fromlayer[0].visibility == true)
                          {           
                               features[t].style = ''; 
                          }
                          else
                          {
                               features[t].style = { visibility: 'hidden' }; 
                          }
                      }
                      else
                      {
                          if(fromlayer[0].visibility == true && tolayer[0].visibility == true)
                          {
                              features[t].style.display = 'block';                                              
                          }
                          else
                          {
                              features[t].style.display = 'none';                           
                          }
                       
                      }
                      
                     
                  }
                  
                  
                  features =  reportLayers[b].getFeaturesByAttribute('ToType',event.layer.name);
                  
                  for(var t = 0 ; t < features.length; t++)
                  {
                      isNeedToRedraw = 1;
                      
                      var fromlayer = map.getLayersByName(features[t].attributes['FromType'])
                      var tolayer = map.getLayersByName(features[t].attributes['ToType'])
                      
                      if(features[t].attributes['isCircle']) 
                      {
                      }
                      else
                      {
                          if(fromlayer[0])
                          {
                              if(fromlayer[0].visibility == true && tolayer[0].visibility == true)
                              {
                                  features[t].style.display = 'block';
                              }
                              else
                              {
                                  features[t].style.display = 'none';
                              }
                          }
                          else
                          {
                              if(tolayer[0].visibility == true)
                              {
                                  features[t].style.display = 'block';
                              }
                              else
                              {
                                  features[t].style.display = 'none';
                              }
                          }
                        
                      }
                      
                      
                     
                
                  }
                  
                  if(isNeedToRedraw == 1)
                      reportLayers[b].redraw();
                  
                  
                  
                features =  reportLayers[b].getFeaturesByAttribute('FromType','Star-Tag-type3');
                  
                for(var t = 0 ; t < features.length; t++)
                {
                    var tolayer = map.getLayersByName(features[t].attributes['ToType'])
                    
                    if(features[t].attributes['tagTypes'])
                    {
                        var isNeedToShow = false;
                        
                        var arr = features[t].attributes['tagTypes'].split(' || ');
                        
                        for(var k = 0 ; k < arr.length; k++)
                        {
                            //var index =   tagInvisibleLayerArray.indexOf(arr[k]);
                                                       
                             var index =   $.inArray(arr[k],tagInvisibleLayerArray);
                            
                            if (index < 0)
                            {
                                isNeedToShow = true;
                            }
                        }
                        
                        
                        
                        if(features[t].attributes['isCircle'] == "Yes")
                        {
                            if(isNeedToShow)
                            {
                                features[t].style = '';
                            }
                            else
                            {
                                features[t].style = { visibility: 'hidden' };
                            }
                        }
                        else
                        {
                            if(isNeedToShow && tolayer[0].visibility == true)
                            {
                                features[t].style.display = 'block';
                            }
                            else
                            {
                                features[t].style.display = 'none';
                            }
                            
                        }


                    }
                }
                  
                  
             reportLayers[b].redraw();
                  

                 // multiTagLayer.setVisibility(true);
                 // multiTagLayer.redraw();

                   
                  
                  //
               
              }  

      }
       
    }
    
    // updateInfraStructureMarkerinroom(InfraStructureResponseArr,InfraStructureResponseArr);
    
}


// add star and monitor to map.
function updateInfraStructureMarkerinroom(starRoot,monitorRoot,accesspointroot,zoneRoot)
{
 
    g_starRoot = starRoot;
  /* var parser = new DOMParser();
     
    starRoot = "<star><dtStar><MacId>00_24_DD_00_31_06 </MacId><SvgId>3000</SvgId><CSVDeviceType>Star</CSVDeviceType><SubType>star</SubType><DeviceName>Star</DeviceName><Description>star description2</Description><StarType>Ethernet</StarType><DHCP>No</DHCP><SaveSettings>Yes</SaveSettings><StaticIP>10.31.17.243</StaticIP><Subnet>255.255.255.0</Subnet><Gateway>10.31.17.1</Gateway><TimeServerIP>10.40.70.239</TimeServerIP><ServerIP /><PagingServerIP>10.20.124.112</PagingServerIP><LocationServerIP1>10.20.124.112</LocationServerIP1><LocationServerIP2>10.20.124.112</LocationServerIP2></dtStar> <dtStar><MacId>00_24_DD_00_31_06 </MacId><SvgId></SvgId><CSVDeviceType>Star</CSVDeviceType><SubType>star</SubType><DeviceName>Star</DeviceName><Description>star description2</Description><StarType>Ethernet</StarType><DHCP>No</DHCP><SaveSettings>Yes</SaveSettings><StaticIP>10.31.17.243</StaticIP><Subnet>255.255.255.0</Subnet><Gateway>10.31.17.1</Gateway><TimeServerIP>10.40.70.239</TimeServerIP><ServerIP /><PagingServerIP>10.20.124.112</PagingServerIP><LocationServerIP1>10.20.124.112</LocationServerIP1><LocationServerIP2>10.20.124.112</LocationServerIP2></dtStar><star>";
    
    starRoot = parser.parseFromString(starRoot,"text/xml");
 
    starRoot =$(starRoot).children().filter('star');
    starRoot =$(starRoot).children().filter('dtStar');
    
    console.log(starRoot);
*/
    
 var deviceType = '';
    
 var SvgId=$(starRoot).children().filter('SvgId');
 var deviceName=$(starRoot).children().filter('DeviceName');
 
 
 //LAyersArray[LAyersArray.length] = starRangeLayer;
 map.addLayer(starRangeLayer);
    
 
 LAyersArray[LAyersArray.length] = starMarKerLayer;
 LAyersArray[LAyersArray.length - 1].addOptions({deviceType: "Star"});
    
 editModelayersArray[editModelayersArray.length] = LAyersArray[LAyersArray.length-1];

    
 deviceType='-';
 for (var i = 0; i < SvgId.length; i++)
     {    
    
           for (var j = 0; j < InfraStructureCoordinatesArr.length; j++)
           {               
               if(InfraStructureCoordinatesArr[j][4] == (SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text))
               {
                   
                         /*if((deviceType != $(starRoot).children().filter('StarType')[i].textContent))
                         {
                            if ($(starRoot).children().filter('StarType')[i].textContent != '')
                                addNewMarkerLayerToMap('Star - ' + $(starRoot).children().filter('StarType')[i].textContent);
                            else
                                addNewMarkerLayerToMap('Stars');
                         }*/
                   
                             
                             
                       //  deviceType = $(starRoot).children().filter('StarType')[i].textContent;
                   
                        var imgDetail =  getDeviceImage( $(starRoot).children().filter('StarType')[i].textContent || $(starRoot).children().filter('StarType')[i].innerText || $(starRoot).children().filter('StarType')[i].text, $(starRoot).children().filter('CSVDeviceType')[i].textContent || $(starRoot).children().filter('CSVDeviceType')[i].innerText || $(starRoot).children().filter('CSVDeviceType')[i].text);
                        var feature = new OpenLayers.Feature.Vector
                        (                                                  
                         new OpenLayers.Geometry.Point((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2).transform(epsg4326, projectTo),
                         {description: "Infrastructure-Star"} ,
                         {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)   }
                        );
                        
                         feature .attributes['infraStructureID'] = InfraStructureCoordinatesArr[j][4];
                         feature .attributes['layerIndex'] = LAyersArray.length - 1;
                         feature .attributes['SVGID'] = SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text;
                       
                        
                        InfraStructureResponseArr[j] = {layerIndex:LAyersArray.length - 1,deviceId:$(starRoot).children().filter('MacId')[i].textContent || $(starRoot).children().filter('MacId')[i].innerText || $(starRoot).children().filter('MacId')[i].text};
                        
                        
                         var labelOffsetPoint = new OpenLayers.Geometry.Point(((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2),((InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2)).transform(epsg4326, projectTo);
                         var HighLightLayer = new OpenLayers.Feature.Vector(labelOffsetPoint);
                            HighLightLayer.attributes = {
                           
                            Radius : STARHIGHLIGHT_LAYER_RADIUS,
                            align: "cm",
                          
                            xOffset: STARHIGHLIGHT_LAYER_RADIUS/2,
                            yOffset: STARHIGHLIGHT_LAYER_RADIUS/2,
                            display: "block"
                           
                            };
                            
                       HighLightLayer.attributes['starType'] = "Infrastructure - Stars"; 
                  
                      
                       starRangeLayer.addFeatures(HighLightLayer);
                       
                       LAyersArray[LAyersArray.length - 1].addFeatures(feature);
                   
                   
                   
                       //icon for edit layer for Stars
                       var editFeature = new OpenLayers.Feature.Vector(
                                                                       new OpenLayers.Geometry.Point((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2).transform(epsg4326, projectTo));
                       editFeature.attributes = {
                           
                       align: "cm",
                           
                       labelYOffset: -(imgDetail[0].imageheight),
                       display: "block",
                       externalGraphic :imgDetail[0].imageName,
                       graphicHeight: imgDetail[0].imageheight,
                       graphicWidth: imgDetail[0].imageWidht,
                       graphicXOffset:-(imgDetail[0].imageWidht/2),
                       graphicYOffset:-(imgDetail[0].imageheight/2),
                       description: "Infrastructure-Star",
                       name: $(starRoot).children().filter('MacId')[i].textContent || $(starRoot).children().filter('MacId')[i].innerText || $(starRoot).children().filter('MacId')[i].text   //MacId for Stars
                           
                       };
                     if (navigator.userAgent.indexOf('MSIE') != -1)
                          editFeature.attributes.fillOpacity = 1.0; 
                   
                       editFeature .attributes['infraStructureID'] = InfraStructureCoordinatesArr[j][4];
                       editFeature .attributes['layerIndex'] = LAyersArray.length - 1;
                       editFeature .attributes['SVGID'] = SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text;
                       addMonitorLayer.addFeatures(editFeature);
                        //   addMonitorLayer.addFeatures(feature.clone());

                   
                       break;
                        
                   
               }
           }
     }
     
     //Monitors
      g_monitorRoot = monitorRoot;
    
    /*monitorRoot = "<monitor><dtMonitor><DeviceId>103124</DeviceId><SvgId>200</SvgId><CSVDeviceType>Monitor</CSVDeviceType><SubType /><DeviceName>Regular Monitor</DeviceName><Description>monitor desciption2</Description><RoomId>871</RoomId><MonitorType>Regular Monitor</MonitorType><Profile>VW MasterA Profile</Profile><IRProfile>3 Seconds</IRProfile><PowerLevel>Medium</PowerLevel><RoomBleeding>No</RoomBleeding><NoiseLevel>Low</NoiseLevel><Masking>Full Masking</Masking><MasterSlave>Master</MasterSlave><SpecialProfile>Disabled</SpecialProfile><OperatingMode>NA</OperatingMode><Modes>NA</Modes><AlertSupressionTime>NA</AlertSupressionTime></dtMonitor><dtMonitor><DeviceId>111349</DeviceId><SvgId>201</SvgId><CSVDeviceType>Monitor</CSVDeviceType><SubType /><DeviceName>Regular Monitor</DeviceName><Description>monitor desciption1</Description><RoomId>757</RoomId><MonitorType>EGRESS</MonitorType><Profile>VW MasterB Profile</Profile><IRProfile>3 Seconds</IRProfile><PowerLevel>Low</PowerLevel><RoomBleeding>No</RoomBleeding><NoiseLevel>Low</NoiseLevel><Masking>Full Masking</Masking><MasterSlave>Master</MasterSlave><SpecialProfile>Disabled</SpecialProfile><OperatingMode>NA</OperatingMode><Modes>NA</Modes><AlertSupressionTime>NA</AlertSupressionTime></dtMonitor></monitor>";
    
    g_monitorRoot = monitorRoot;
    
    monitorRoot = parser.parseFromString(monitorRoot,"text/xml");
    
    monitorRoot =$(monitorRoot).children().filter('monitor');
    monitorRoot =$(monitorRoot).children().filter('dtMonitor');*/

    
    var SvgId= $(monitorRoot).children().filter("SvgId");
    var deviceName=$(monitorRoot).children().filter("DeviceName");
 
  
    deviceType ='-';
    
     for (var i = 0; i < SvgId.length; i++)
     {
                  
           for (var j = 0; j < InfraStructureCoordinatesArr.length; j++)
           {
                          
               if(InfraStructureCoordinatesArr[j][4] == (SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text))
               {
                  
                   
                   if(deviceType != ($(monitorRoot).children().filter('MonitorType')[i].textContent || $(monitorRoot).children().filter('MonitorType')[i].innerText || $(monitorRoot).children().filter('MonitorType')[i].text))
                   {
                   
                       addNewMarkerLayerToMap("Infrastructure - " + ($(monitorRoot).children().filter('MonitorType')[i].textContent || $(monitorRoot).children().filter('MonitorType')[i].innerText || $(monitorRoot).children().filter('MonitorType')[i].text));
                       LAyersArray[LAyersArray.length - 1].addOptions({deviceType: ($(monitorRoot).children().filter('CSVDeviceType')[i].textContent || $(monitorRoot).children().filter('CSVDeviceType')[i].innerText || $(monitorRoot).children().filter('CSVDeviceType')[i].text)});
                       
                       editModelayersArray[editModelayersArray.length] = LAyersArray[LAyersArray.length-1];
                       

                   }
                 
                   //Change the Room Color accourding to the Monitor Profile
                   //var curRoom = Rooms.getFeaturesByAttribute('roomNO',"room"+  monitorRoot.children().filter('DeviceId')[i].textContent || monitorRoot.children().filter('DeviceId')[i].innerText || monitorRoot.children().filter('DeviceId')[i].text)[0];
                   var r = (monitorRoot.children().filter('DeviceId')[i].textContent || monitorRoot.children().filter('DeviceId')[i].innerText || monitorRoot.children().filter('DeviceId')[i].text);
                   var curRoom = Rooms.getFeaturesByAttribute('roomNO',"room"+ r)[0];
                  // var curRoom = Rooms.getFeaturesByAttribute('roomNO',"room"+  monitorRoot.children().filter('DeviceId')[i].textContent || monitorRoot.children().filter('DeviceId')[i].innerText || monitorRoot.children().filter('DeviceId')[i].text)[0];
                   
                   if(curRoom)
                   {
                        curRoom.style = getRoomColorForProfileId((monitorRoot.children().filter('ProfileId')[i].textContent || monitorRoot.children().filter('ProfileId')[i].innerText || monitorRoot.children().filter('ProfileId')[i].text));
                        Rooms.drawFeature(curRoom,getRoomColorForProfileId((monitorRoot.children().filter('ProfileId')[i].textContent || monitorRoot.children().filter('ProfileId')[i].innerText || monitorRoot.children().filter('ProfileId')[i].text))); 
                   }
                   
                   deviceType = ($(monitorRoot).children().filter('MonitorType')[i].textContent || $(monitorRoot).children().filter('MonitorType')[i].innerText || $(monitorRoot).children().filter('MonitorType')[i].text);
                   var imgDetail =  getDeviceImage((monitorRoot.children().filter('MonitorType')[i].textContent || monitorRoot.children().filter('MonitorType')[i].innerText || monitorRoot.children().filter('MonitorType')[i].text),(monitorRoot.children().filter('CSVDeviceType')[i].textContent || monitorRoot.children().filter('CSVDeviceType')[i].innerText || monitorRoot.children().filter('CSVDeviceType')[i].text));
                   var feature = new OpenLayers.Feature.Vector
                    (
                     
                     new OpenLayers.Geometry.Point((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2).transform(epsg4326, projectTo),
                      {description: "Infrastructure-Monitor"} ,
                     {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                     );
                   

                       feature .attributes['infraStructureID'] = InfraStructureCoordinatesArr[j][4];
                       feature .attributes['layerIndex'] = LAyersArray.length - 1;
                       feature .attributes['SVGID'] = SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text;
                   
                       LAyersArray[LAyersArray.length - 1].addFeatures(feature);
                   
                   
                       //icon for edit layer for Monitors
                       var editFeature = new OpenLayers.Feature.Vector(
                                                                   new OpenLayers.Geometry.Point((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2).transform(epsg4326, projectTo));
                       editFeature.attributes = {
                           
                       align: "cm",
                           
                       labelYOffset: -(imgDetail[0].imageheight),
                       display: "block",
                       externalGraphic :imgDetail[0].imageName,
                       graphicHeight: imgDetail[0].imageheight,
                       graphicWidth: imgDetail[0].imageWidht,
                       graphicXOffset:-(imgDetail[0].imageWidht/2),
                       graphicYOffset:-(imgDetail[0].imageheight/2),
                       description: "Infrastructure-Monitor",
                       name: r   //deviceid for monitors
                           
                       };
                                if (navigator.userAgent.indexOf('MSIE') != -1)
                                     editFeature.attributes.fillOpacity = 1.0;                     
                       
                       editFeature .attributes['infraStructureID'] = InfraStructureCoordinatesArr[j][4];
                       editFeature .attributes['layerIndex'] = LAyersArray.length - 1;
                       editFeature .attributes['SVGID'] = SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text;
                       addMonitorLayer.addFeatures(editFeature);

                   
                       InfraStructureResponseArr[j] = {layerIndex:LAyersArray.length - 1,deviceId:(monitorRoot.children().filter('DeviceId')[i].textContent || monitorRoot.children().filter('DeviceId')[i].innerText || monitorRoot.children().filter('DeviceId')[i].text)};
                       
                       //Add Star->Monitor Link Line
                       var monitorStarId = (monitorRoot.children().filter('LockedStarId')[i].textContent || monitorRoot.children().filter('LockedStarId')[i].innerText || monitorRoot.children().filter('LockedStarId')[i].text);
                       
                       var LockedStar = $(starRoot).find("MacId").filter("macid").filter(function () { return $( this ).text() == String(monitorStarId);}).parent(); 
                       
                       if(LockedStar.length > 0)
                       {
                            var starSVGID=($(LockedStar).children().filter('SvgId')[0].textContent || $(LockedStar).children().filter('SvgId')[0].innerText || $(LockedStar).children().filter('SvgId')[0].text);
                           if(starSVGID > -1)
                           {
                               addLinkLine((SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text),starSVGID,"1",[(monitorRoot.children().filter('MonitorType')[i].textContent || monitorRoot.children().filter('MonitorType')[i].innerText || monitorRoot.children().filter('MonitorType')[i].text),($(LockedStar).children().filter('SvgId')[0].textContent || $(LockedStar).children().filter('SvgId')[0].innerText || $(LockedStar).children().filter('SvgId')[0].text)]);
                               
                           } 
                       }
                       
                       break;
                       
               }
           }
     }
    
    
    
    
    //accessPoint
    g_accessPointRoot = accesspointroot;
    
    
    var APSvgId= $(accesspointroot).children().filter("SvgId");
    var deviceName=$(accesspointroot).children().filter("DeviceName");
    
    
    deviceType ='-';
    
    for (var i = 0; i < APSvgId.length; i++)
    {
        
        for (var j = 0; j < InfraStructureCoordinatesArr.length; j++)
        {
            
            if(InfraStructureCoordinatesArr[j][4] == (APSvgId[i].textContent || APSvgId[i].innerText || APSvgId[i].text))
            {
                
                
                if(deviceType != ($(accesspointroot).children().filter('CSVDeviceType')[i].textContent || $(accesspointroot).children().filter('CSVDeviceType')[i].innerText || $(accesspointroot).children().filter('CSVDeviceType')[i].text))
                {
                    
                    addNewMarkerLayerToMap("Infrastructure - " + ($(accesspointroot).children().filter('CSVDeviceType')[i].textContent || $(accesspointroot).children().filter('CSVDeviceType')[i].innerText || $(accesspointroot).children().filter('CSVDeviceType')[i].text));
                    LAyersArray[LAyersArray.length - 1].addOptions({deviceType: ($(accesspointroot).children().filter('CSVDeviceType')[i].textContent || $(accesspointroot).children().filter('CSVDeviceType')[i].innerText || $(accesspointroot).children().filter('CSVDeviceType')[i].text)});
                    
                    editModelayersArray[editModelayersArray.length] = LAyersArray[LAyersArray.length-1];
                    
                    
                }
                
                var r = (accesspointroot.children().filter('DeviceId')[i].textContent || accesspointroot.children().filter('DeviceId')[i].innerText || accesspointroot.children().filter('DeviceId')[i].text);

                
                deviceType = ($(accesspointroot).children().filter('CSVDeviceType')[i].textContent || $(accesspointroot).children().filter('CSVDeviceType')[i].innerText || $(accesspointroot).children().filter('CSVDeviceType')[i].text);
                
                var imgDetail =  getDeviceImage((accesspointroot.children().filter('CSVDeviceType')[i].textContent || accesspointroot.children().filter('CSVDeviceType')[i].innerText || accesspointroot.children().filter('CSVDeviceType')[i].text),(accesspointroot.children().filter('CSVDeviceType')[i].textContent || accesspointroot.children().filter('CSVDeviceType')[i].innerText || accesspointroot.children().filter('CSVDeviceType')[i].text));
                
                
                var feature = new OpenLayers.Feature.Vector
                (
                 
                 new OpenLayers.Geometry.Point((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2).transform(epsg4326, projectTo),
                 {description: "Infrastructure-AccessPoints"} ,
                 {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                 );
                
                
                feature .attributes['infraStructureID'] = InfraStructureCoordinatesArr[j][4];
                feature .attributes['layerIndex'] = LAyersArray.length - 1;
                feature .attributes['SVGID'] = APSvgId[i].textContent || APSvgId[i].innerText || APSvgId[i].text;
                
                LAyersArray[LAyersArray.length - 1].addFeatures(feature);
                
                
                //icon for edit layer for Monitors
                var editFeature = new OpenLayers.Feature.Vector(
                                                                new OpenLayers.Geometry.Point((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2).transform(epsg4326, projectTo));
                editFeature.attributes = {
                    
                align: "cm",
                    
                labelYOffset: -(imgDetail[0].imageheight),
                display: "block",
                    externalGraphic :imgDetail[0].imageName,
                graphicHeight: imgDetail[0].imageheight,
                graphicWidth: imgDetail[0].imageWidht,
                graphicXOffset:-(imgDetail[0].imageWidht/2),
                graphicYOffset:-(imgDetail[0].imageheight/2),
                description: "Infrastructure-AccessPoints",
                name: r   //deviceid for monitors
                    
                };
                if (navigator.userAgent.indexOf('MSIE') != -1)
                    editFeature.attributes.fillOpacity = 1.0;
                
                editFeature .attributes['infraStructureID'] = InfraStructureCoordinatesArr[j][4];
                editFeature .attributes['layerIndex'] = LAyersArray.length - 1;
                editFeature .attributes['SVGID'] = APSvgId[i].textContent || APSvgId[i].innerText || APSvgId[i].text;
                addMonitorLayer.addFeatures(editFeature);
                
                
                InfraStructureResponseArr[j] = {layerIndex:LAyersArray.length - 1,deviceId:(accesspointroot.children().filter('DeviceId')[i].textContent || accesspointroot.children().filter('DeviceId')[i].innerText || accesspointroot.children().filter('DeviceId')[i].text)};
                
                
                break;
                
            }
        }
    }
    
    //wifiZones
    g_wifiZones = zoneRoot;
    
    
    var SvgId=$(zoneRoot).children().filter('SvgId');
    var deviceName=$(zoneRoot).children().filter('DeviceName');
    
    editModelayersArray[editModelayersArray.length] = WifiZoneLayer;
    
    
    for (var i = 0; i < SvgId.length; i++)
    {
        for (var j = 0; j < roomDetails.length; j++)
        {
            if(roomDetails[j][4] == (SvgId[i].textContent || SvgId[i].innerText || SvgId[i].text))
            {
                
                var ZoneId = $(zoneRoot).children().filter('ZoneId')[i].textContent || $(zoneRoot).children().filter('ZoneId')[i].innerText || $(zoneRoot).children().filter('ZoneId')[i].text;
                var _SvgId = $(zoneRoot).children().filter('SvgId')[i].textContent || $(zoneRoot).children().filter('SvgId')[i].innerText || $(zoneRoot).children().filter('SvgId')[i].text;
                var CSVDeviceType = $(zoneRoot).children().filter('CSVDeviceType')[i].textContent || $(zoneRoot).children().filter('CSVDeviceType')[i].innerText || $(zoneRoot).children().filter('CSVDeviceType')[i].text;
                var SubType = $(zoneRoot).children().filter('SubType')[i].textContent || $(zoneRoot).children().filter('SubType')[i].innerText || $(zoneRoot).children().filter('SubType')[i].text;
                var DeviceName = $(zoneRoot).children().filter('DeviceName')[i].textContent || $(zoneRoot).children().filter('DeviceName')[i].innerText || $(zoneRoot).children().filter('DeviceName')[i].text;
                var Description = $(zoneRoot).children().filter('Description')[i].textContent || $(zoneRoot).children().filter('Description')[i].innerText || $(zoneRoot).children().filter('Description')[i].text;
                var Location = $(zoneRoot).children().filter('Location')[i].textContent || $(zoneRoot).children().filter('Location')[i].innerText || $(zoneRoot).children().filter('Location')[i].text;
                var Notes = $(zoneRoot).children().filter('Notes')[i].textContent || $(zoneRoot).children().filter('Notes')[i].innerText || $(zoneRoot).children().filter('Notes')[i].text;
                var IsHallWay = $(zoneRoot).children().filter('IsHallWay')[i].textContent || $(zoneRoot).children().filter('IsHallWay')[i].innerText || $(zoneRoot).children().filter('IsHallWay')[i].text;
                var UnitName = $(zoneRoot).children().filter('UnitName')[i].textContent || $(zoneRoot).children().filter('UnitName')[i].innerText || $(zoneRoot).children().filter('UnitName')[i].text;
                
                
                var source =  roomCoordinatesArr[j][4];
                
                var polygonList = [];
                var pointList = [];
                var editPolygonList = [];
                for (var k=0; k<source.length; k++)
                {
                    var poloygonCoord = source[k].split(',');
                    if(poloygonCoord[0] != '' && poloygonCoord[1] != '')
                    {
                        var point = new OpenLayers.Geometry.Point(parseFloat(poloygonCoord[0]), getboxFromYpos(parseFloat(poloygonCoord[1]),ImageHeight));
                        pointList.push(point);
                    }
                    
                }
                var linearRing = new OpenLayers.Geometry.LinearRing(pointList);
                var polygon = new OpenLayers.Geometry.Polygon([linearRing]);
                polygonList.push(polygon);
                
                multuPolygonGeometry = new OpenLayers.Geometry.MultiPolygon(polygonList);
                multiPolygonFeature = new OpenLayers.Feature.Vector(multuPolygonGeometry);
                multiPolygonFeature.style = defaultZoneStyle;
                multiPolygonFeature.attributes = { "ZoneId" :ZoneId , "SvgId" :_SvgId ,"CSVDeviceType" :CSVDeviceType ,"SubType" :SubType ,"DeviceName" :DeviceName ,"Description" :Description ,"Location" :Location ,"Notes" :Notes ,"IsHallWay" :IsHallWay,"UnitName" :UnitName ,"featureType" :"wifi-zones" , "description": "Wifi-Zones",device: "wifiZones"};
                multiPolygonFeature.style.label = "";

                WifiZoneLayer.addFeatures(multiPolygonFeature);
                
                
                
                
                //feature for edit layer
                var editlinearRing = new OpenLayers.Geometry.LinearRing(pointList);
                var editPolygon = new OpenLayers.Geometry.Polygon([editlinearRing]);
                editPolygonList.push(editPolygon);
                
                editMultuPolygonGeometry = new OpenLayers.Geometry.MultiPolygon(editPolygonList);
                editMultuPolygonGeometry = new OpenLayers.Feature.Vector(editMultuPolygonGeometry);
                
                
                editMultuPolygonGeometry.attributes = {
                    
                align: "cm",
                display: "block",
                name: ZoneId,   //deviceid for monitors
                fill : true,
                fillColor : "#01B928",
                fillOpacity : 0.7,
                strokeColor: "#024402",
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeDashstyle:"dash"
                    
                };
                
                 editMultuPolygonGeometry .attributes['SvgId'] = _SvgId;
                editMultuPolygonGeometry .attributes['featureType'] = "wifi-zones";
                editMultuPolygonGeometry .attributes['description'] = "Wifi-Zones";
                editMultuPolygonGeometry .attributes['device'] = "wifiZones";


                //editMultuPolygonGeometry.style.label = ZoneId;

                addMonitorLayer.addFeatures(editMultuPolygonGeometry);
                
                
                break;
                
                
            }
        }
    }
    
    
    isInfraStructureLoaded = true;
    if(multiTagLayer!=undefined)
    {
     LAyersArray[LAyersArray.length] = multiTagLayer;
     LAyersArray[LAyersArray.length - 1].addOptions({deviceType: "Tag"});
     taglayersArray[taglayersArray.length]= multiTagLayer;
     multiTagLayer.displayInLayerSwitcher = false;
     LAyersArray[LAyersArray.length] = WifiZoneLayer;
    }
    addAllLayersToMap(LAyersArray);
    
   changeMapMode();
    
}



// response from api .
function UpdateTags(tagArr)
{
    tagDetailsArr = tagArr;
   addTagMarkerinroom('1');
}


// Add tags to map.
function addTagMarkerinroom(isNeedToAddLayer)
{
       isInfraStructureLoaded = false;
       multiTagLayer.setVisibility(true);
       multiTagLayer.redraw();
      if(starTagLinkLayer)
         starTagLinkLayer.destroyFeatures();
       
       existingTagStarLinkLayer.splice(0,existingTagStarLinkLayer.length);
       multiTagsLoadedRoomsArray.splice(0,multiTagsLoadedRoomsArray.length);
       for(var i=0 ; i < taglayersArray.length ; i++ )
       {
            taglayersArray[i].destroyFeatures();
       }        

         multiTagLayer.destroyFeatures();  
       
        var tagType = "-";
        var layerIndex = -1;
        
        var tempCoordinatesArray;  
        var awayDistance;   
        for (var j = 0; j < tagDetailsArr.length; j++)
        {
          
         var i = 0;
         tempCoordinatesArray = getTagCoordinatesInfoArray(tagDetailsArr[j].SvgId);
         
             if(tempCoordinatesArray.length > 0)
             {
               awayDistance = tempCoordinatesArray[1];                                        
         
                if(tempCoordinatesArray[i][4] == tagDetailsArr[j].SvgId)
                {                       
                              
                          var popupDetailArr = checkMultiTagRoom(tagDetailsArr[j].SvgId);
                                                    
                                        
                            if(isNeedToAddLayer == '1')
                            {
                                  if(tagType != tagDetailsArr[j].TagTypeName)
                                 {
                                    if (tagDetailsArr[j].tagType != '')
                                    {
                                            var layer = map.getLayersByName('Tag - ' + tagDetailsArr[j].TagTypeName )[0];
                                            if(!layer) // create a new tag layer for newly arrived tags.
                                            {            
                                                addNewMarkerLayerToMap('Tag - ' + tagDetailsArr[j].TagTypeName);                                        
                                                 LAyersArray[LAyersArray.length - 1].addOptions({deviceType: 'Tag'});                                               
                                                 taglayersArray[taglayersArray.length] = LAyersArray[LAyersArray.length-1];
                                                 layerIndex = LAyersArray.length - 1;
                                            }
                                            else
                                            {
                                               //layerIndex = LAyersArray.indexOf(layer);
                                               layerIndex = $.inArray(layer,LAyersArray);
                                                      
                                            }
                                    }
                                    else
                                    {
                                        addNewMarkerLayerToMap('Tags');
                                        LAyersArray[LAyersArray.length - 1].addOptions({deviceType: 'Tag'});
                                     }                                       
                                        
                                        layerIndex = LAyersArray.length - 1;
                                 }
                                 
                                    
                            }
                            else
                            {
                                 if(tagType != tagDetailsArr[j].TagTypeName)
                                 {
                                    if (tagDetailsArr[j].tagType != '')
                                    {
                                                                       
                                        for(var m=0; m < LAyersArray.length ; m++)
                                        {
                                                                                
                                          var res = LAyersArray[m].name.substring(6,  LAyersArray[m].name.length);
                                          
                                          if(res == tagDetailsArr[j].TagTypeName)
                                          {
                                            layerIndex = m;
                                            break;
                                       
                                          } 
     
                                        }
    
                                    }
                          
                                 }
                            } 
                                        
                            tagType = tagDetailsArr[j].TagTypeName;   
                                               
                            if(popupDetailArr[0] > 1) //put MultiTag Icon
                            {
                            
                               var index = $.inArray(tagDetailsArr[j].SvgId.toString(),multiTagsLoadedRoomsArray);
                               if($.inArray(tagDetailsArr[j].SvgId.toString(),multiTagsLoadedRoomsArray) < 0)
                                {
                                        var imgDetail =  getTagImageByType(-1);
                                        var feature = new OpenLayers.Feature.Vector
                                        (
                                         new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2] + awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3] + awayDistance)/2).transform(epsg4326, projectTo),
                                         {description: popupDetailArr[1]} ,
                                         {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                                         );
                                        

                                        
                                        feature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                        feature .attributes['tagsID'] = popupDetailArr[2];
                                        feature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);

                                        multiTagLayer.addFeatures(feature);
                                        
                                        var MultiTagxOffset = 5;
                                        var MultiTagWidth = 35;
                                        
                                        if(popupDetailArr[0] > 999)
                                        {
                                            MultiTagxOffset = 5;
                                            MultiTagWidth = 45;
                                        }
                                        
                                        
                                        feature = new OpenLayers.Feature.Vector
                                        (
                                         new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2] + awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3] + awayDistance)/2).transform(epsg4326, projectTo),
                                         {description: popupDetailArr[1]} ,
                                         {externalGraphic: 'Images/MultiTag.png', graphicHeight: 25, graphicWidth: MultiTagWidth, graphicXOffset:MultiTagxOffset-5, graphicYOffset:-30  }
                                         );
                                        
                                        
                                        feature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                        feature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);
                                        feature .attributes['tagsID'] = popupDetailArr[2];
                                        multiTagLayer.addFeatures(feature);
                                        
                                        
                                        // Create a point feature to show the label offset options
                                        var labelOffsetPoint = new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2] + awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3] + awayDistance)/2).transform(epsg4326, projectTo);
                                        var labelOffsetFeature = new OpenLayers.Feature.Vector(labelOffsetPoint);
                                        labelOffsetFeature.attributes = {
                                        name: popupDetailArr[0],
                                        favColor: 'white',
                                        align: "cm",
                                            // positive value moves the label to the right
                                        xOffset: 16,
                                            // negative value moves the label down
                                        yOffset: 19
                                        };
                                        
                                        labelOffsetFeature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                        labelOffsetFeature .attributes['tagsID'] = popupDetailArr[2];
                                        labelOffsetFeature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);
                                        labelOffsetFeature .attributes['isCircle'] = true;
                                        multiTagLayer.addFeatures(labelOffsetFeature);
                                        
                                        multiTagsLoadedRoomsArray[multiTagsLoadedRoomsArray.length] = tagDetailsArr[j].SvgId;
                                        
                                }
                               
                                  //Add Star->Tag Link Line
                                  
                                 var TagStarId = tagDetailsArr[j].LockedStarId;
                                           
                                 var LockedStar = $(g_starRoot).find("MacId").filter("macid").filter(function () { return $( this ).text() == String(TagStarId);}).parent(); 
                                           
                                   if(LockedStar.length > 0)
                                   {
                                        var starSVGID=$(LockedStar).children().filter('SvgId')[0].textContent || $(LockedStar).children().filter('SvgId')[0].innerText || $(LockedStar).children().filter('SvgId')[0].text;
                                        
                                        // var starSVGID=($(LockedStar).children().filter('SvgId')[0].textContent) || ($(LockedStar).children().filter('SvgId')[0].innerText) || ($(LockedStar).children().filter('SvgId')[0].text);
                                       if(starSVGID > -1)
                                       {
                                       
                                          // if(tagInvisibleLayerArray.indexOf(tagDetailsArr[j].TagTypeName) < 0) 
                                          if($.inArray(tagDetailsArr[j].TagTypeName,tagInvisibleLayerArray) < 0)
                                           {
                                                    if($.inArray(tempCoordinatesArray[i][4] + "_" + starSVGID,existingTagStarLinkLayer) < 0)
                                                 // if(existingTagStarLinkLayer.indexOf(tempCoordinatesArray[i][4] + "_" + starSVGID) < 0)
                                                   {                                  
                                                      addLinkLine(tempCoordinatesArray[i],starSVGID,"0",[tagDetailsArr[j].TagTypeName,$(LockedStar).children().filter('StarType')[0].textContent || $(LockedStar).children().filter('StarType')[0].innerText || $(LockedStar).children().filter('StarType')[0].text]);
                                                            
                                                      existingTagStarLinkLayer[existingTagStarLinkLayer.length] = tempCoordinatesArray[i][4] + "_" + starSVGID;    
                                                   }
                                           }
                                           
                                       } 
                                   }
                                   
                          
                          
                                
                            }
                            else
                            {
                               
                                var imgDetail =  getTagImageByType(tagDetailsArr[j].TagType);
                                var feature = new OpenLayers.Feature.Vector
                                (
                                 new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2] + awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3] + awayDistance)/2 ).transform(epsg4326, projectTo),
                                 {description: "Tag-Popup" } ,
                                 {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                                 );
                                
                                feature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                feature .attributes['tagsID'] = popupDetailArr[2];
                                feature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);
                                LAyersArray[layerIndex].addFeatures(feature);
                                
                                 //Add Star->Tag Link Line
                                  
                                 var TagStarId = tagDetailsArr[j].LockedStarId;
                                           
                                 var LockedStar = $(g_starRoot).find("MacId").filter("macid").filter(function () { return $( this ).text() == String(TagStarId);}).parent(); 
                                           
                                   if(LockedStar.length > 0)
                                   {
                                        var starSVGID=$(LockedStar).children().filter('SvgId')[0].textContent || $(LockedStar).children().filter('SvgId')[0].innerText || $(LockedStar).children().filter('SvgId')[0].text;
                                       if(starSVGID > -1)
                                       {
                                       
                                       
                                           //if(tagInvisibleLayerArray.indexOf(tagDetailsArr[j].TagTypeName) < 0)
                                          if($.inArray(tagDetailsArr[j].TagTypeName.toString(),tagInvisibleLayerArray) < 0) 
                                           {
                                          
                                                 // if(existingTagStarLinkLayer.indexOf(tempCoordinatesArray[i][4] + "_" + starSVGID) < 0)
                                                  if( $.inArray(tempCoordinatesArray[i][4] + "_" + starSVGID,existingTagStarLinkLayer) < 0)
                                                   {                                  
                                                        addLinkLine(tempCoordinatesArray[i],starSVGID,"0",[tagDetailsArr[j].TagTypeName,$(LockedStar).children().filter('StarType')[0].textContent || $(LockedStar).children().filter('StarType')[0].innerText || $(LockedStar).children().filter('StarType')[0].text]);
                                                            
                                                      existingTagStarLinkLayer[existingTagStarLinkLayer.length] = tempCoordinatesArray[i][4] + "_" + starSVGID;    
                                                   }
                                           }
                                           
                                       } 
                                   }
                                
                           
                  }
                
            }
                
             }
              
        }
        
        isInfraStructureLoaded = true;
           if(isNeedToAddLayer == '1')
           {
                addAllLayersToMap(taglayersArray);
           }
        showOnlyFilteredLiveTags();
}


// check if a room have multitag and prepare html content for multitag popup.
function checkMultiTagRoom (roomId)
{
    var popupDetailArr = new Array();
    
    var tagCount = 0;
    
    var tagIdArray = new Array ();
    
    
    for(var i = 0;i < tagDetailsArr.length; i++)
    {
        if(tagDetailsArr[i].SvgId == roomId)
        {
        var t=tagDetailsArr[i].TagTypeName;
        var index = $.inArray(t.toString(),tagInvisibleLayerArray);
      // var index = tagInvisibleLayerArray.prototype.indexOf(tagDetailsArr[i].TagTypeName);
                    
            if(index < 0)
            {                 
                tagCount ++;

                tagIdArray[tagIdArray.length] = String(tagDetailsArr[i].tagID);
            }
        }
    }
    
   /* if(tagCount == 1)
        popupContent = "";*/
    
    popupDetailArr[0] = tagCount;
    popupDetailArr[1] = "Tag-Popup";
    popupDetailArr[2] = tagIdArray;
    
    return popupDetailArr;
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////      READ AND PARSE SVG FILE               ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function readAndParseSVGFileforIE(file)
{
  
     var coordinates = [];    
     var rawFile;     
    rawFile = new ActiveXObject("Microsoft.XMLHTTP"); 
    rawFile.open("GET",file,false);
    rawFile.onreadystatechange = function ()
    {
        if(rawFile.readyState === 4)
        {            
            var allText = rawFile.responseText;            
            if(rawFile.status === 200)
            {
                var doc;
                    try
                    {
                        doc = new ActiveXObject("Microsoft.XMLDOM");
                        doc.async = false;
                        doc.loadXML(allText);
                    }
                    catch (e)
                    {
                        return false;
                    }             
                
               var xpos,ypos,width,height,boxID,fillcolor,isRoomCoord; 
              // if (navigator.userAgent.indexOf('MSIE') != -1)
              //  {    
                var y=doc.documentElement.childNodes;                  
                
                    for (var i = 0; i < y.length ; i++)
                    {
                        
                        if(i==0)
                        {
                            imageWidth  = parseFloat(y[i].getAttribute("width"));
                            ImageHeight = parseFloat(y[i].getAttribute("height"));
                            
                            if(zoomify_url != undefined && zoomify_url.length > 0)
                            {
                               if(imageWidth > 0)
                                    widthratio = zoomify_width/imageWidth;
                               if(ImageHeight > 0)
                                    heightratio = zoomify_height/ImageHeight;
                                    
                                imageWidth = zoomify_width;
                                ImageHeight = zoomify_height;
                                
                                STARHIGHLIGHT_LAYER_RADIUS = STARHIGHLIGHT_LAYER_RADIUS * widthratio;
                            }  
                            
                        }
                        else
                        {
                            if(y[i].getAttribute("x"))                       
                            {
                                xpos   = parseFloat(y[i].getAttribute("x"));
                               ypos   = parseFloat(y[i].getAttribute("y"));
                               width  = parseFloat(y[i].getAttribute("width"));
                               height = parseFloat(y[i].getAttribute("height")); 
                                                             
                                if(y[i].getAttribute("id"))
                                     boxID  = parseFloat(y[i].getAttribute("id"));                                     
                                 else
                                     boxID = 0;
                               
                               if(zoomify_url != undefined && zoomify_url.length > 0)
                               {
                                    xpos = xpos*widthratio;
                                    ypos = ypos*heightratio;
                                    width = width*widthratio;
                                    height = height*heightratio;
                               }
                               
                                roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight)];                               
                                 roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,y[i].getAttribute("fill")];
                            }
                            if(y[i].getAttribute("points"))
                            {
                              var str = y[i].getAttribute("points");
                      
                              var points = str.split(' ');
                            
                              var firstCoord = points[0].split(',');
                              var thirdCoord = points[2].split(',');
                              
                             
                               xpos   = (parseFloat(firstCoord[0]) + parseFloat(thirdCoord[0])) / 2  ;
                               ypos   = (parseFloat(firstCoord[1]) + parseFloat(thirdCoord[1])) / 2 ;
                               width  = 0;
                               height = 0;

                                if(y[i].getAttribute("id"))
                                     boxID  = parseFloat(y[i].getAttribute("id"));
                                 else
                                     boxID = 0;
                               
                               if(zoomify_url != undefined && zoomify_url.length > 0)
                               {
                                    xpos = xpos*widthratio;
                                    ypos = ypos*heightratio;
                                    width = width*widthratio;
                                    height = height*heightratio;
                               }
                               
                               roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points];
                               roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,y[i].getAttribute("fill")];
                            }
                            if(y[i].getAttribute("rx"))
                            {
                                xpos   = parseFloat(y[i].getAttribute("cx"));
                                ypos   = parseFloat(y[i].getAttribute("cy"));
                                width  = parseFloat(y[i].getAttribute("rx"));
                                height = parseFloat(y[i].getAttribute("ry"));
                                
                                if(y[i].getAttribute("id"))
                                    boxID  = parseFloat(y[i].getAttribute("id"));
                                else
                                    boxID = 0;
                                    
                                if(zoomify_url != undefined && zoomify_url.length > 0)
                                {
                                    xpos = xpos*widthratio;
                                    ypos = ypos*heightratio;
                                    width = width*widthratio;
                                    height = height*heightratio;
                                }
                                InfraStructureCoordinatesArr[InfraStructureCoordinatesArr.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,y[i].getAttribute("fill")];
                               
                                                       
                            }
                        }
                    }
               // }
                
                

            }
            
        }
        
       STARHIGHLIGHT_LAYER_RADIUS = getStarRangeRadius(g_FloorWidthFt,g_FloorLengthFt);
        
    }
    rawFile.send();
    
}




function readAndParseSVGFile(file)
{
    var coordinates = [];
    
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", file, false);
    rawFile.onreadystatechange = function ()
    {
        if(rawFile.readyState === 4)
        {
            
            var allText = rawFile.responseText;
            
            if(rawFile.status === 200 || rawFile.status == 0)
            {
                var doc;
                    var parser = new DOMParser();
                    doc = parser.parseFromString(allText, "image/svg+xml");                    
       
                
                var xpos,ypos,width,height,boxID,fillcolor,isRoomCoord,roomType;
                
                
               if (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1)
               {
                    
                    for (var i = 0; i < doc.rootElement.childNodes.length ; i++)
                    {
                        
                        if(doc.rootElement.childNodes[i].nodeName != '#text')
                        {
                            
                            if(doc.rootElement.childNodes[i].nodeName == 'image')
                            {
                                
                                for(var j = 0; j < doc.rootElement.childNodes[i].attributes.length ; j++)
                                {
                                    if(doc.rootElement.childNodes[i].attributes[j].nodeName == "width")
                                    {
                                        imageWidth =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "height")
                                    {
                                        ImageHeight =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                    }
                                }
                                
                            }
                            else
                            {
                                boxID = 0;
                                isRoomCoord = 0;
                                roomType = null;
                                
                                for(var j = 0; j < doc.rootElement.childNodes[i].attributes.length ; j++)
                                {
                                    // for rectangels
                                    if(doc.rootElement.childNodes[i].attributes[j].nodeName == "x")
                                    {
                                        xpos =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 1;
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "y")
                                    {
                                        ypos =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 1;
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "width")
                                    {
                                        width =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 1;
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "height")
                                    {
                                        height =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 1;
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "id")
                                    {
                                        boxID =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                    }
                                    
                                    
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "points")
                                    {
                                        polygonPoints =  doc.rootElement.childNodes[i].attributes[j].nodeValue;
                                        isRoomCoord = 2;
                                        
                                        
                                        
                                    }
                                   
                                    
                                    //f0r ecllipse
                                                                        
                                    if(doc.rootElement.childNodes[i].attributes[j].nodeName == "cx")
                                    {
                                        xpos =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 0;
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "cy")
                                    {
                                        ypos =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 0;
                                        
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "rx")
                                    {
                                        width =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 0;
                                        
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "ry")
                                    {
                                        height =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                        isRoomCoord = 0;
                                    }
                                    else if(doc.rootElement.childNodes[i].attributes[j].nodeName == "id")
                                    {
                                        boxID =  parseFloat(doc.rootElement.childNodes[i].attributes[j].nodeValue);
                                    }
                                    
                                    
                                    if(doc.rootElement.childNodes[i].attributes[j].nodeName == "fill")
                                    {
                                        fillcolor =  doc.rootElement.childNodes[i].attributes[j].nodeValue;

                                    }
                                    
                                    if(doc.rootElement.childNodes[i].attributes[j].nodeName == "type")
                                    {
                                        roomType =  doc.rootElement.childNodes[i].attributes[j].nodeValue;
                                        
                                    }
                                    
                                }
                                
                               
                                if(isRoomCoord == 1)
                                {
                                  roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight)];
                                
                                  roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
                                }
                                else if(isRoomCoord == 2)
                                {
                                    
                                    var points = polygonPoints.split(' ');
                                    
                                    var firstCoord = points[0].split(',');
                                    var thirdCoord = points[2].split(',');
                                    
                                    xpos   = (parseFloat(firstCoord[0]) + parseFloat(thirdCoord[0])) / 2  ;
                                    ypos   = (parseFloat(firstCoord[1]) + parseFloat(thirdCoord[1])) / 2 ;
                                    
                                    if(roomType)
                                    {
                                        roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points,roomType];
                                        
                                        roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor,roomType];
                                    }
                                    else
                                    {
                                        roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points];
                                        
                                        roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
                                    }
                              
                                
                                }
                                else
                                {
                                     InfraStructureCoordinatesArr[InfraStructureCoordinatesArr.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
                                    
                                }
                               
                                                           
                            }
                           
                            
                        }
                        
                    }
                    
                    
                }
              else //CHROME
               {
                    
                    for (var i = 0; i < doc.children[0].children.length ; i++)
                    {
                        
                        if(i==0)
                        {
                            imageWidth  = parseFloat(doc.children[0].children[i].attributes.width.nodeValue);
                            ImageHeight = parseFloat(doc.children[0].children[i].attributes.height.nodeValue);
                            
                            if(zoomify_url.length > 0)
                            {
                               if(imageWidth > 0)
                                    widthratio = zoomify_width/imageWidth;
                               if(ImageHeight > 0)
                                    heightratio = zoomify_height/ImageHeight;
                                    
                                imageWidth = zoomify_width;
                                ImageHeight = zoomify_height;
                                
                                STARHIGHLIGHT_LAYER_RADIUS = STARHIGHLIGHT_LAYER_RADIUS * widthratio;
                            }  
                            
                        }
                        else
                        {
                            if(doc.children[0].children[i].attributes.x)
                            {
                               xpos   = parseFloat(doc.children[0].children[i].attributes.x.nodeValue);
                               ypos   = parseFloat(doc.children[0].children[i].attributes.y.nodeValue);
                               width  = parseFloat(doc.children[0].children[i].attributes.width.nodeValue);
                               height = parseFloat(doc.children[0].children[i].attributes.height.nodeValue);
                               
                                 if(doc.children[0].children[i].attributes.id)
                                     boxID  = parseFloat(doc.children[0].children[i].attributes.id.nodeValue);
                                 else
                                     boxID = 0;
                               
                               if(zoomify_url.length > 0)
                               {
                                    xpos = xpos*widthratio;
                                    ypos = ypos*heightratio;
                                    width = width*widthratio;
                                    height = height*heightratio;
                               }
                               
                               roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight)];
                               
                               roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,doc.children[0].children[i].attributes.fill.nodeValue];
                            }
                            if(doc.children[0].children[i].attributes.points)
                            {
                              var str = doc.children[0].children[i].attributes.points.nodeValue;
                      
                              var points = str.split(' ');
                            
                              var firstCoord = points[0].split(',');
                              var thirdCoord = points[2].split(',');
                              
                             
                               xpos   = (parseFloat(firstCoord[0]) + parseFloat(thirdCoord[0])) / 2  ;//parseFloat(doc.children[0].children[i].attributes.x.nodeValue);
                               ypos   = (parseFloat(firstCoord[1]) + parseFloat(thirdCoord[1])) / 2 ;//parseFloat(doc.children[0].children[i].attributes.y.nodeValue);
                               width  = 0;//parseFloat(doc.children[0].children[i].attributes.width.nodeValue);
                               height = 0;//parseFloat(doc.children[0].children[i].attributes.height.nodeValue);
                               
                                 if(doc.children[0].children[i].attributes.id)
                                     boxID  = parseFloat(doc.children[0].children[i].attributes.id.nodeValue);
                                 else
                                     boxID = 0;
                               
                               if(zoomify_url.length > 0)
                               {
                                    xpos = xpos*widthratio;
                                    ypos = ypos*heightratio;
                                    width = width*widthratio;
                                    height = height*heightratio;
                               }
                               
                               var roomType='';
                                if(doc.children[0].children[i].attributes.type)
                                    roomType= doc.children[0].children[i].attributes.type.nodeValue;
                                
                                if(roomType)
                                {
                                    roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points,roomType];
                                    
                                    roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,doc.children[0].children[i].attributes.fill.nodeValue,roomType];
                                }
                                else
                                {
                                    roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points];
                                    
                                    roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,doc.children[0].children[i].attributes.fill.nodeValue];
                                }
                                
                            
                            }
                            if(doc.children[0].children[i].attributes.rx)
                            {
                                xpos   = parseFloat(doc.children[0].children[i].attributes.cx.nodeValue);
                                ypos   = parseFloat(doc.children[0].children[i].attributes.cy.nodeValue);
                                width  = parseFloat(doc.children[0].children[i].attributes.rx.nodeValue);
                                height = parseFloat(doc.children[0].children[i].attributes.ry.nodeValue);
                                
                                if(doc.children[0].children[i].attributes.id)
                                    boxID  = parseFloat(doc.children[0].children[i].attributes.id.nodeValue);
                                else
                                    boxID = 0;
                                    
                                if(zoomify_url.length > 0)
                                {
                                    xpos = xpos*widthratio;
                                    ypos = ypos*heightratio;
                                    width = width*widthratio;
                                    height = height*heightratio;
                                }
                                
                                InfraStructureCoordinatesArr[InfraStructureCoordinatesArr.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,doc.children[0].children[i].attributes.fill.nodeValue];
                                //roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight)];
                                
                                //roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,doc.children[0].children[i].attributes.fill.nodeValue];
                                                       
                            }
                        }
                    }
                }
                
                

            }
        }
         STARHIGHLIGHT_LAYER_RADIUS = getStarRangeRadius(g_FloorWidthFt,g_FloorLengthFt);
    }
    rawFile.send();
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////                CO ORDINATE CALCULATIONS               /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function getboxToXpos (xpos,width)
{
    return toXpos = xpos + width;
}

function getboxFromYpos (ypos,imageHeight)
{
    return fromYpos = imageHeight - ypos;
}

function getboxToYpos (ypos ,height ,imageHeight)
{
    return toYpos = imageHeight - (ypos+height);
}

function convertToSvgYCoordinate(ypos)
{
   ypos = ImageHeight - ypos;
   return ypos;
}





//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////             ADD NEW LAYER                  ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addAllLayersToMap(layerArrToAdd)
{
    layerArrToAdd[layerArrToAdd.length] = addMonitorLayer;
    map.addLayers(layerArrToAdd);     
    addControlsToMap(map,tagMarKerLayer);  
    
     var  vectorLayer = map.getLayersByName('Tag - Live Updates')[0];  
     if(vectorLayer)
     {
          map.setLayerIndex(vectorLayer,map.layers.length - 1);
     }
}

function addNewMarkerLayerToMap(layerName)
{
   
    var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
    renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;
    
    
    LAyersArray[LAyersArray.length] = new OpenLayers.Layer.Vector(layerName, {
                                                  styleMap: new OpenLayers.StyleMap({'default':{
                                                                                    strokeColor: "#00FF00",
                                                                                    strokeOpacity: 1,
                                                                                    strokeWidth: 3,
                                                                                    fillColor: "#FF5500",
                                                                                    fillOpacity: 0.5,
                                                                                    pointRadius: 0,
                                                                                    pointerEvents: "visiblePainted",
                                                                                    // label with \n linebreaks
                                                                                    graphicname : "${name}",                                                                                    
                                                                                    fontColor: "${favColor}",
                                                                                    fontSize: "15px",
                                                                                    fontFamily: "Courier New, monospace",
                                                                                    fontWeight: "bold",
                                                                                    labelAlign: "${align}",
                                                                                    graphicXOffset: "${xOffset}",
                                                                                    graphicYOffset: "${yOffset}",
                                                                                    labelOutlineColor: "white",
                                                                                    labelOutlineWidth: 0
                                                                                    }}),
                                                  renderers: renderer
                                                  });
    
    

}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////         ADD CONTROLS TO LAYER IN MAP         ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function addControlsToMap(map,layerToaddControl)
{
    
    //Add a selector control to the tagMarKerLayer with popup functions
    controls = {
    selector: new OpenLayers.Control.SelectFeature(LAyersArray, { onSelect: createPopup, onUnselect: destroyPopup })
    
    };
      if (typeof(controls['selector'].handlers) != "undefined")
      { // OL 2.7
				controls['selector'].handlers.feature.stopDown = false;				
	  } 
	  else if (typeof(controls['selector'].handler) != "undefined") 
	  { // OL < 2.7
		        controls['selector'].handler.stopDown = false; 
		        controls['selector'].handler.stopUp = false; 				
	 }
    map.addControl(controls['selector']); 
    
    controls['selector'].activate();
    
    
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////            CREATE AND DESTROY POPUP          ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Select feature to change position or edit.
function clicKEventForEditMode(feature)
{
    
        var deviceRoot;
        var i = 0;
        
        var t_svgid            ;
        var t_layerIndex       ;
        var t_infraStructureID ;
        
        
        
        if(selectedFeature)  //Deselect previously Selected Feature
        {
            if(selectedFeature == lastDrawnDevice)
            {
                var imgDetail;
                var t_description;
                
                if(selectedFeature.attributes.description == "Infrastructure-Monitor" )
                {
                    imgDetail =  getDeviceImage( 'Monitor', 'Monitor');
                    t_description = "Infrastructure-Monitor";
                }
                else if(selectedFeature.attributes.description == "Infrastructure-Star" )
                {
                    imgDetail =  getDeviceImage( 'Star', 'Star');
                    t_description = "Infrastructure-Star";

                }
                else if(selectedFeature.attributes.description == "Infrastructure-AccessPoints" )
                {
                    imgDetail =  getDeviceImage( 'AccessPoint', 'AccessPoint');
                    t_description = "Infrastructure-AccessPoints";

                }
                
                selectedFeature.attributes = {
                align: "cm",
                labelYOffset: -(imgDetail[0].imageheight),
                externalGraphic: imgDetail[0].imageName ,
                graphicHeight: imgDetail[0].imageheight,
                graphicWidth: imgDetail[0].imageWidht,
                graphicXOffset:-(imgDetail[0].imageWidht/2),
                graphicYOffset:-(imgDetail[0].imageheight/2),
                fillOpacity:1.0,
                description: t_description,
                name:  ''    //deviceid for monitors
                }
                
                cancelEditForDevice();
                
                if(selectedFeature == feature)
                {
                    selectedFeature = '';
                    drag.interesting_feature = '';
                    controls['selector'].unselect(feature);
                    return;
                }
                
            }
            else if (selectedFeature != lastDrawnDevice)
            {
                
                if(selectedFeature.attributes.description == "Infrastructure-Monitor" )
                {
                    
                    deviceRoot = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SVGID']);}).parent();
                    
                    var imgDetail =  getDeviceImage((deviceRoot.children().filter('MonitorType')[i].textContent || deviceRoot.children().filter('MonitorType')[i].innerText || deviceRoot.children().filter('MonitorType')[i].text),(deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text));
                    
                    
                    // get attributes
                    t_svgid             = selectedFeature .attributes['SVGID'];
                    t_layerIndex        = selectedFeature .attributes['layerIndex'];
                    t_infraStructureID  = selectedFeature .attributes['infraStructureID'];
                    
                    
                    selectedFeature.attributes = {
                    align: "cm",
                    labelYOffset: -(imgDetail[0].imageheight),
                    externalGraphic: imgDetail[0].imageName ,
                    graphicHeight: imgDetail[0].imageheight,
                    graphicWidth: imgDetail[0].imageWidht,
                    graphicXOffset:-(imgDetail[0].imageWidht/2),
                    graphicYOffset:-(imgDetail[0].imageheight/2),
                    fillOpacity:1.0,
                    description: "Infrastructure-Monitor",
                    name:  $(deviceRoot).children().filter('DeviceId')[i].textContent || $(deviceRoot).children().filter('DeviceId')[i].innerText || $(deviceRoot).children().filter('DeviceId')[i].text    //deviceid for monitors
                    }
                    
                    selectedFeature .attributes['infraStructureID'] = t_infraStructureID;
                    selectedFeature .attributes['layerIndex'] = t_layerIndex;
                    selectedFeature .attributes['SVGID'] = t_svgid;
                    
                    selectedFeature.layer.redraw();
                    cancelEditForDevice();
                    
                }
                else if(selectedFeature.attributes.description == "Infrastructure-AccessPoints" )
                {
                    
                    deviceRoot = $(g_accessPointRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SVGID']);}).parent();
                    
                    var imgDetail =  getDeviceImage((deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text),(deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text));
                    
                    
                    // get attributes
                    t_svgid             = selectedFeature .attributes['SVGID'];
                    t_layerIndex        = selectedFeature .attributes['layerIndex'];
                    t_infraStructureID  = selectedFeature .attributes['infraStructureID'];
                    
                    
                    selectedFeature.attributes = {
                    align: "cm",
                    labelYOffset: -(imgDetail[0].imageheight),
                    externalGraphic: imgDetail[0].imageName ,
                    graphicHeight: imgDetail[0].imageheight,
                    graphicWidth: imgDetail[0].imageWidht,
                    graphicXOffset:-(imgDetail[0].imageWidht/2),
                    graphicYOffset:-(imgDetail[0].imageheight/2),
                    fillOpacity:1.0,
                    description: "Infrastructure-AccessPoints",
                    name:  $(deviceRoot).children().filter('DeviceId')[i].textContent || $(deviceRoot).children().filter('DeviceId')[i].innerText || $(deviceRoot).children().filter('DeviceId')[i].text    //deviceid for monitors
                    }
                    
                    selectedFeature .attributes['infraStructureID'] = t_infraStructureID;
                    selectedFeature .attributes['layerIndex'] = t_layerIndex;
                    selectedFeature .attributes['SVGID'] = t_svgid;
                    
                    selectedFeature.layer.redraw();
                    cancelEditForDevice();
                    
                }
                else if (selectedFeature.attributes.description == "Infrastructure-Star")
                {
                    deviceRoot = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SVGID']);}).parent();
                    
                    var imgDetail =  getDeviceImage( $(deviceRoot).children().filter('StarType')[i].textContent || $(deviceRoot).children().filter('StarType')[i].innerText || $(deviceRoot).children().filter('StarType')[i].text, $(deviceRoot).children().filter('CSVDeviceType')[i].textContent || $(deviceRoot).children().filter('CSVDeviceType')[i].innerText || $(deviceRoot).children().filter('CSVDeviceType')[i].text);
                    
                    
                    // get attributes
                    t_svgid             = selectedFeature .attributes['SVGID'];
                    t_layerIndex        = selectedFeature .attributes['layerIndex'];
                    t_infraStructureID  = selectedFeature .attributes['infraStructureID'];
                    
                    
                    selectedFeature.attributes = {
                    align: "cm",
                    labelYOffset: -(imgDetail[0].imageheight),
                    externalGraphic: imgDetail[0].imageName ,
                    graphicHeight:  imgDetail[0].imageheight,
                    graphicWidth: imgDetail[0].imageWidht,
                    graphicXOffset:-(imgDetail[0].imageWidht/2),
                    graphicYOffset:-(imgDetail[0].imageheight/2),
                    fillOpacity:1.0,
                    description: "Infrastructure-Star",
                    name: $(deviceRoot).children().filter('MacId')[i].textContent || $(deviceRoot).children().filter('MacId')[i].innerText || $(deviceRoot).children().filter('MacId')[i].text   //deviceid for monitors
                    }
                    
                    selectedFeature .attributes['infraStructureID'] = t_infraStructureID;
                    selectedFeature .attributes['layerIndex'] = t_layerIndex;
                    selectedFeature .attributes['SVGID'] = t_svgid;
                    
                    selectedFeature.layer.redraw();
                    cancelEditForDevice();
                    
                }
                else if(selectedFeature.attributes.description == "Wifi-Zones")
                {
                    deviceRoot =  $(g_wifiZones).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SvgId']);}).parent();
                    
                    
                    selectedFeature.attributes = {
                        
                    align: "cm",
                    display: "block",
                    name: $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text,   //ZoneId for monitors
                        fill : true,
                        fillColor : "#01B928",
                        fillOpacity : 0.7,
                    strokeColor: "#024402",
                    strokeOpacity: 1,
                    strokeWidth: 2,
                    strokeDashstyle:"dash"
                        
                    };
                    
                    selectedFeature .attributes['SvgId']        =  $(deviceRoot).children().filter('SvgId')[i].textContent || $(deviceRoot).children().filter('SvgId')[i].innerText || $(deviceRoot).children().filter('SvgId')[i].text;;
                    selectedFeature .attributes['featureType']  = "wifi-zones";
                    selectedFeature .attributes['description'] = "Wifi-Zones";
                    selectedFeature .attributes['device']      = "wifiZones";
                    

                    
                    addMonitorLayer.drawFeature(selectedFeature);
                    cancelEditForDevice();
                    
                    controls['selector'].unselect(selectedFeature);
                    selectedFeature = '';
                    drag.interesting_feature = '';
                }
                else
                {
                    controls['selector'].unselect(feature);
                    
                    selectedFeature.attributes = {
                    align: "cm",
                    display: "block",
                    name: "",
                    strokeOpacity: 0.5,
                    fillOpacity: 0.4,
                    fillColor: "#ADFF2F",
                    strokeColor: "#3366FF",
                    };
                    
                    selectedFeature.layer.redraw();
                }
                
                
                
                if(selectedFeature == feature)
                {
                    selectedFeature = '';
                    drag.interesting_feature = '';
                    controls['selector'].unselect(feature);
                    
                    return;
                }
            }
           
            
        }
    
        if (feature == lastDrawnDevice)
        {
            if((feature.attributes.description == "Infrastructure-Monitor" || feature.attributes.description == "Infrastructure-Star" || feature.attributes.description == "Infrastructure-AccessPoints") && feature.layer == addMonitorLayer)
            {
                
                if(selectedFeature == feature)
                {
                    return;
                }
                
                var t_description;
                var selImage;

                if(feature.attributes.description == "Infrastructure-Monitor")
                {
                    selImage = 'images/map/mapMonitorSel.png';
                    t_description = "Infrastructure-Monitor";
                    g_svgDType = 1;

                }
                else if(feature.attributes.description == "Infrastructure-AccessPoints")
                {
                    selImage = 'images/map/mapAccessPointSel.png';
                    t_description = "Infrastructure-AccessPoints";
                    g_svgDType = 3;
                    
                }
                else if(feature.attributes.description == "Infrastructure-Star")
                {
                    selImage =  'images/map/mapStarsSel.png';
                    t_description = "Infrastructure-Star";
                    g_svgDType = 2;
                }
                
                
                feature.attributes = {
                    align: "cm",
                    labelYOffset:       -40,
                    externalGraphic:    selImage,
                    graphicHeight:       40,
                    graphicWidth:        40,
                    graphicXOffset:     -(40/2),
                    graphicYOffset:     -(40/2),
                    description:        t_description,
                    fillOpacity: 1.0,
                    name:               ''//  //deviceid for monitors
                }
                
                controls['selector'].unselect(feature);
                feature.layer.redraw();
                
                drag.interesting_feature = feature;
                activateDrag();
                
                
                selectedFeature = feature;
                
            }
            
             ShowControlsForSelectedDevice();
             $('#btnDrawRoom').removeClass('mapDrawRoom').removeAttr('class').addClass('mapDrawRoomDis');
             $("#btnDrawRoom").prop("disabled",true);
             
             if(g_svgDType == 1)
             {
                 $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitorDis');
             }
             else if(g_svgDType == 2)
             {
                 $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateStarDis');
             }
             else if(g_svgDType == 3)
             {
                 $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateAccesspointDis');
             }  
                $("#btnPlaceMonitor").prop("disabled",true);
                
            return;
        }
        
    
    //select currently clicked feature
    if((feature.attributes.description == "Infrastructure-Monitor" || feature.attributes.description == "Infrastructure-Star" || feature.attributes.description == "Infrastructure-AccessPoints") && feature.layer == addMonitorLayer)
    {
        if(lastDrawnDevice || lastDrawnRoom)
            return;
            
        if(selectedFeature == feature)
        {
            
            return;
        }
        
        var t_description = '';
        var t_deviceID = '';
        var t_imgDetail;
        var selImage;
        
        
        var bounds = feature.geometry.getBounds();
        monitorX = bounds.left;
        monitorY = bounds.top;
        monitorW = bounds.right;
        monitorH = bounds.bottom;
        
        monitorX = convertToSvgEcllipseXCoordinate(parseFloat(monitorX));
        monitorY = convertToSvgEcllipseYCoordinate(parseFloat(monitorY));
        monitorW = 15;
        monitorH = 15;
        
        
        
        if(feature.attributes.description == "Infrastructure-Monitor" )
        {
            g_svgDType = 1;
            deviceRoot =  $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(feature .attributes['SVGID']);}).parent();
            t_description = "Infrastructure-Monitor";
            t_deviceID = $(deviceRoot).children().filter('DeviceId')[i].textContent || $(deviceRoot).children().filter('DeviceId')[i].innerText || $(deviceRoot).children().filter('DeviceId')[i].text
            t_imgDetail =  getDeviceImage((deviceRoot.children().filter('MonitorType')[i].textContent || deviceRoot.children().filter('MonitorType')[i].innerText || deviceRoot.children().filter('MonitorType')[i].text),(deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text));
            selImage = 'images/map/mapMonitorSel.png';
        }
        else if(feature.attributes.description == "Infrastructure-AccessPoints" )
        {
            g_svgDType = 3;
            deviceRoot =  $(g_accessPointRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(feature .attributes['SVGID']);}).parent();
            t_description = "Infrastructure-AccessPoints";
            t_deviceID = $(deviceRoot).children().filter('DeviceId')[i].textContent || $(deviceRoot).children().filter('DeviceId')[i].innerText || $(deviceRoot).children().filter('DeviceId')[i].text
            t_imgDetail =  getDeviceImage((deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text),(deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text));
            selImage = 'images/map/mapAccessPointSel.png';
        }
        else if (feature.attributes.description == "Infrastructure-Star")
        {
            g_svgDType=2;
            deviceRoot = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(feature .attributes['SVGID']);}).parent();
            t_description = "Infrastructure-Star";
            t_deviceID = $(deviceRoot).children().filter('MacId')[i].textContent || $(deviceRoot).children().filter('MacId')[i].innerText || $(deviceRoot).children().filter('MacId')[i].text
            t_imgDetail =  getDeviceImage( $(deviceRoot).children().filter('StarType')[i].textContent || $(deviceRoot).children().filter('StarType')[i].innerText || $(deviceRoot).children().filter('StarType')[i].text, $(deviceRoot).children().filter('CSVDeviceType')[i].textContent || $(deviceRoot).children().filter('CSVDeviceType')[i].innerText || $(deviceRoot).children().filter('CSVDeviceType')[i].text);
            selImage =  'images/map/mapStarsSel.png';
        }
        
        g_oldDeviceId = t_deviceID;
        
        t_svgid             = feature .attributes['SVGID'];
        t_layerIndex        = feature .attributes['layerIndex'];
        t_infraStructureID  = feature .attributes['infraStructureID'];
        
        feature.attributes = {
        align: "cm",
        labelYOffset:       -(30),
        externalGraphic:    selImage ,
        graphicHeight:       40,
        graphicWidth:        40,
        graphicXOffset:     -(40/2),
        graphicYOffset:     -(40/2),
        description:        t_description,
        fillOpacity: 1.0,
        name:               t_deviceID  //deviceid for monitors
        }
        
        
        feature .attributes['infraStructureID'] = t_infraStructureID;
        feature .attributes['layerIndex'] = t_layerIndex;
        feature .attributes['SVGID'] = t_svgid;
        
        controls['selector'].unselect(feature);
        feature.layer.redraw();
        
        
        drag.interesting_feature = feature;
        activateDrag();
        
        
        
        selectedFeature = feature;
        
        initialDevicecoords_x = feature.geometry.x;
        initialDevicecoords_y = feature.geometry.y;
        
        
        if(!deviceRoot || deviceRoot.length == 0)
        {
            document.getElementById("txtMonitorId").value ="";
            document.getElementById("txtLocation").value="";
            document.getElementById("txtNotes").value ="";
            document.getElementById("chkIsHallway").checked = false;
            document.getElementById("txtUnitName").value="";
            g_dsvgId = 0;
            return;
        }
        
        
        if(feature.attributes.description == "Infrastructure-Monitor" || feature.attributes.description == "Infrastructure-AccessPoints" )
        {
            document.getElementById("txtMonitorId").value = t_deviceID;
            document.getElementById("txtLocation").value = setundefined(deviceRoot.children().filter('Location')[i].textContent || deviceRoot.children().filter('Location')[i].innerText || deviceRoot.children().filter('Location')[i].text);
            document.getElementById("txtNotes").value = setundefined(deviceRoot.children().filter('Notes')[i].textContent || deviceRoot.children().filter('Notes')[i].innerText || deviceRoot.children().filter('Notes')[i].text);
            document.getElementById("txtUnitName").value= setundefined(deviceRoot.children().filter('UnitName')[i].textContent || deviceRoot.children().filter('UnitName')[i].innerText || deviceRoot.children().filter('UnitName')[i].text);
            if((deviceRoot.children().filter('IsHallWay')[i].textContent || deviceRoot.children().filter('IsHallWay')[i].innerText || deviceRoot.children().filter('IsHallWay')[i].text) == "True")
                document.getElementById("chkIsHallway").checked = true;
            else
                document.getElementById("chkIsHallway").checked = false;
            
            
            g_dsvgId =  feature .attributes['SVGID'];
            
        }
        else
        {
            document.getElementById("txtMonitorId").value = t_deviceID;
            //document.getElementById("txtLocation").value = setundefined(deviceRoot.children().filter('Location')[i].textContent || deviceRoot.children().filter('Location')[i].innerText || deviceRoot.children().filter('Location')[i].text);
            //document.getElementById("txtNotes").value = setundefined(deviceRoot.children().filter('Notes')[i].textContent || deviceRoot.children().filter('Notes')[i].innerText || deviceRoot.children().filter('Notes')[i].text);
            
            /*if(deviceRoot.children().filter('MasterSlave')[i].textContent || deviceRoot.children().filter('MasterSlave')[i].innerText || deviceRoot.children().filter('MasterSlave')[i].text)
             document.getElementById("chkIsHallway").checked = true;
             else
             document.getElementById("chkIsHallway").checked = false;
             */
            
            g_dsvgId =  feature .attributes['SVGID'];
            
        }
        
         ShowControlsForSelectedDevice();
         $('#btnDrawRoom').removeClass('mapDrawRoom').removeAttr('class').addClass('mapDrawRoomDis');
         $("#btnDrawRoom").prop("disabled",true);
         
         if(g_svgDType == 1)
         {
             $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitorDis');
         }
         else
         {
             $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateStarDis');
         }  
            $("#btnPlaceMonitor").prop("disabled",true);
        
        return;
    }
    else
    {
        if(selectedFeature == feature)
        {
            return;
        }
        
        selectedFeature = feature;
        drag.interesting_feature = feature;
        activateDrag();
        controls['selector'].unselect(feature);
        
        //get center for polygons
        initialDevicecoords_x = (feature.geometry.getBounds().left + feature.geometry.getBounds().right)/2;
        initialDevicecoords_y = (feature.geometry.getBounds().top + feature.geometry.getBounds().bottom)/2;
        
        selectedFeature.attributes = {
        align: "cm",
        display: "block",
        name: "",
        strokeOpacity: 1,
        fillOpacity: 0.4,
        fillColor: "#F51111",
        strokeColor: "#FCF305",
        };
        
        selectedFeature.layer.redraw();
        
        return;
    }
}


function clicKEventForEditModePolygons(feature)
{
    
    var deviceRoot;
    var i = 0;
    
    var t_svgid            ;
    var t_layerIndex       ;
    var t_infraStructureID ;
    
    
    if(selectedFeature)  //Deselect previously Selected Feature
    {
        if(selectedFeature == lastDrawnRoom)
        {
            
            deviceRoot =  $(g_wifiZones).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(feature .attributes['SvgId']);}).parent();
            
            
            feature.attributes = {
                
            align: "cm",
            display: "block",
            name: $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text,   //ZoneId for monitors
            fill : true,
            fillColor : "#01B928",
            fillOpacity : 0.7,
            strokeColor: "#024402",
            strokeOpacity: 1,
            strokeWidth: 2,
            strokeDashstyle:"dash"
                
            };
            
            feature .attributes['SvgId']        =  $(deviceRoot).children().filter('SvgId')[i].textContent || $(deviceRoot).children().filter('SvgId')[i].innerText || $(deviceRoot).children().filter('SvgId')[i].text;;
            feature .attributes['featureType']  = "wifi-zones";
            feature .attributes['description'] = "Wifi-Zones";
            feature .attributes['device']      = "wifiZones";
            

            
            addMonitorLayer.drawFeature(feature);
            cancelEditForDevice();
            
            if(selectedFeature == feature)
            {
                selectedFeature = '';
                drag.interesting_feature = '';
                controls['selector'].unselect(feature);
                return;
            }
            
        }
        else if (selectedFeature != lastDrawnDevice)
        {
            
            if(selectedFeature.attributes.description == "Infrastructure-Monitor" )
            {
                
                deviceRoot = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SVGID']);}).parent();
                
                var imgDetail =  getDeviceImage((deviceRoot.children().filter('MonitorType')[i].textContent || deviceRoot.children().filter('MonitorType')[i].innerText || deviceRoot.children().filter('MonitorType')[i].text),(deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text));
                
                
                // get attributes
                t_svgid             = selectedFeature .attributes['SVGID'];
                t_layerIndex        = selectedFeature .attributes['layerIndex'];
                t_infraStructureID  = selectedFeature .attributes['infraStructureID'];
                
                
                selectedFeature.attributes = {
                align: "cm",
                labelYOffset: -(imgDetail[0].imageheight),
                externalGraphic: imgDetail[0].imageName ,
                graphicHeight: imgDetail[0].imageheight,
                graphicWidth: imgDetail[0].imageWidht,
                graphicXOffset:-(imgDetail[0].imageWidht/2),
                graphicYOffset:-(imgDetail[0].imageheight/2),
                description: "Infrastructure-Monitor",
                name:  $(deviceRoot).children().filter('DeviceId')[i].textContent || $(deviceRoot).children().filter('DeviceId')[i].innerText || $(deviceRoot).children().filter('DeviceId')[i].text    //deviceid for monitors
                }
                
                selectedFeature .attributes['infraStructureID'] = t_infraStructureID;
                selectedFeature .attributes['layerIndex'] = t_layerIndex;
                selectedFeature .attributes['SVGID'] = t_svgid;
                
                selectedFeature.layer.redraw();
                cancelEditForDevice();
                
            }
            else if(selectedFeature.attributes.description == "Infrastructure-AccessPoints" )
            {
                
                deviceRoot = $(g_accessPointRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SVGID']);}).parent();
                
                var imgDetail =  getDeviceImage((deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text),(deviceRoot.children().filter('CSVDeviceType')[i].textContent || deviceRoot.children().filter('CSVDeviceType')[i].innerText || deviceRoot.children().filter('CSVDeviceType')[i].text));
                
                
                // get attributes
                t_svgid             = selectedFeature .attributes['SVGID'];
                t_layerIndex        = selectedFeature .attributes['layerIndex'];
                t_infraStructureID  = selectedFeature .attributes['infraStructureID'];
                
                
                selectedFeature.attributes = {
                align: "cm",
                labelYOffset: -(imgDetail[0].imageheight),
                externalGraphic: imgDetail[0].imageName ,
                graphicHeight: imgDetail[0].imageheight,
                graphicWidth: imgDetail[0].imageWidht,
                graphicXOffset:-(imgDetail[0].imageWidht/2),
                graphicYOffset:-(imgDetail[0].imageheight/2),
                fillOpacity:1.0,
                description: "Infrastructure-AccessPoints",
                name:  $(deviceRoot).children().filter('DeviceId')[i].textContent || $(deviceRoot).children().filter('DeviceId')[i].innerText || $(deviceRoot).children().filter('DeviceId')[i].text    //deviceid for monitors
                }
                
                selectedFeature .attributes['infraStructureID'] = t_infraStructureID;
                selectedFeature .attributes['layerIndex'] = t_layerIndex;
                selectedFeature .attributes['SVGID'] = t_svgid;
                
                selectedFeature.layer.redraw();
                cancelEditForDevice();
                
            }
            else if (selectedFeature.attributes.description == "Infrastructure-Star")
            {
                deviceRoot = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SVGID']);}).parent();
                
                var imgDetail =  getDeviceImage( $(deviceRoot).children().filter('StarType')[i].textContent || $(deviceRoot).children().filter('StarType')[i].innerText || $(deviceRoot).children().filter('StarType')[i].text, $(deviceRoot).children().filter('CSVDeviceType')[i].textContent || $(deviceRoot).children().filter('CSVDeviceType')[i].innerText || $(deviceRoot).children().filter('CSVDeviceType')[i].text);
                
                
                // get attributes
                t_svgid             = selectedFeature .attributes['SVGID'];
                t_layerIndex        = selectedFeature .attributes['layerIndex'];
                t_infraStructureID  = selectedFeature .attributes['infraStructureID'];
                
                
                selectedFeature.attributes = {
                align: "cm",
                labelYOffset: -(imgDetail[0].imageheight),
                externalGraphic: imgDetail[0].imageName ,
                graphicHeight:  imgDetail[0].imageheight,
                graphicWidth: imgDetail[0].imageWidht,
                graphicXOffset:-(imgDetail[0].imageWidht/2),
                graphicYOffset:-(imgDetail[0].imageheight/2),
                description: "Infrastructure-Star",
                name: $(deviceRoot).children().filter('MacId')[i].textContent || $(deviceRoot).children().filter('MacId')[i].innerText || $(deviceRoot).children().filter('MacId')[i].text   //deviceid for monitors
                }
                
                selectedFeature .attributes['infraStructureID'] = t_infraStructureID;
                selectedFeature .attributes['layerIndex'] = t_layerIndex;
                selectedFeature .attributes['SVGID'] = t_svgid;
                
                selectedFeature.layer.redraw();
                cancelEditForDevice();
                
            }
            else if(selectedFeature.attributes.description == "Wifi-Zones")
            {
                deviceRoot =  $(g_wifiZones).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(selectedFeature .attributes['SvgId']);}).parent();
                
                
                selectedFeature.attributes = {
                    
                align: "cm",
                display: "block",
                name: $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text,   //ZoneId for monitors
                    fill : true,
                    fillColor : "#01B928",
                    fillOpacity : 0.7,
                strokeColor: "#024402",
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeDashstyle:"dash"
                    
                };
                
                selectedFeature .attributes['SvgId']        =  $(deviceRoot).children().filter('SvgId')[i].textContent || $(deviceRoot).children().filter('SvgId')[i].innerText || $(deviceRoot).children().filter('SvgId')[i].text;;
                selectedFeature .attributes['featureType']  = "wifi-zones";
                selectedFeature .attributes['description'] = "Wifi-Zones";
                selectedFeature .attributes['device']      = "wifiZones";

                
                
                addMonitorLayer.drawFeature(selectedFeature);
                cancelEditForDevice();
                
                controls['selector'].unselect(selectedFeature);
                drag.interesting_feature = '';
            }
            else
            {
                controls['selector'].unselect(feature);
                
                selectedFeature.attributes = {
                align: "cm",
                display: "block",
                name: "",
                strokeOpacity: 0.5,
                fillOpacity: 0.4,
                fillColor: "#ADFF2F",
                strokeColor: "#3366FF",
                };
                
                selectedFeature.layer.redraw();
            }
            
            
            
            if(selectedFeature == feature)
            {
                selectedFeature = '';
                drag.interesting_feature = '';
                controls['selector'].unselect(feature);
                
                return;
            }
        }
        
        
    }
    
    if (feature == lastDrawnRoom)
    {
        if(feature.attributes.description == "Wifi-Zones" && feature.layer == addMonitorLayer)
        {
            
            if(selectedFeature == feature)
            {
                return;
            }
            
            deviceRoot =  $(g_wifiZones).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(feature .attributes['SvgId']);}).parent();

            
            feature.attributes = {
                
            align: "cm",
            display: "block",
            name: $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text,   //ZoneId for monitors
            fill : true,
            fillColor : "#FF69B4",
            fillOpacity : 0.7,
            strokeColor: "#024402",
            strokeOpacity: 1,
            strokeWidth: 2,
            strokeDashstyle:"dash"
                
            };
            
            feature .attributes['SvgId']        =  $(deviceRoot).children().filter('SvgId')[i].textContent || $(deviceRoot).children().filter('SvgId')[i].innerText || $(deviceRoot).children().filter('SvgId')[i].text;;
            feature .attributes['featureType']  = "wifi-zones";
            feature .attributes['description'] = "Wifi-Zones";
            feature .attributes['device']      = "wifiZones";
            

            addMonitorLayer.drawFeature(feature);
            
            controls['selector'].unselect(feature);
            feature.layer.redraw();
            
            drag.interesting_feature = feature;
            activateDrag();
            
            selectedFeature = feature;
            
            //get center for polygons
            initialDevicecoords_x = (feature.geometry.getBounds().left + feature.geometry.getBounds().right)/2;
            initialDevicecoords_y = (feature.geometry.getBounds().top + feature.geometry.getBounds().bottom)/2;
            
            g_svgDType = 4;
            
            ShowControlsForSelectedDevice();
            $('#btnDrawRoom').removeClass('mapDrawRoom').removeAttr('class').addClass('mapDrawRoomDis');
            $("#btnDrawRoom").prop("disabled",true);
            
            if(g_svgDType == 4)
            {
                $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitorDis');
            }
           
            $("#btnPlaceMonitor").prop("disabled",true);

            
        }
        return;
    }
    
    
    //select currently clicked feature
    if(feature.attributes.description == "Wifi-Zones" && feature.layer == addMonitorLayer)
    {
        if(selectedFeature == feature)
        {
            
            return;
        }
        
        
        deviceRoot =  $(g_wifiZones).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(feature .attributes['SvgId']);}).parent();

        
        feature.attributes = {
            
        align: "cm",
        display: "block",
        name: $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text,   //ZoneId for monitors
        fill : true,
        fillColor : "#FF69B4",
        fillOpacity : 0.7,
        strokeColor: "#024402",
        strokeOpacity: 1,
        strokeWidth: 2,
        strokeDashstyle:"dash"
            
        };
        
        feature .attributes['SvgId']        =  $(deviceRoot).children().filter('SvgId')[i].textContent || $(deviceRoot).children().filter('SvgId')[i].innerText || $(deviceRoot).children().filter('SvgId')[i].text;;
        feature .attributes['featureType']  = "wifi-zones";
        feature .attributes['description'] = "Wifi-Zones";
        feature .attributes['device']      = "wifiZones";
        
        
        addMonitorLayer.drawFeature(feature);
        
        controls['selector'].unselect(feature);
        feature.layer.redraw();
        
        drag.interesting_feature = feature;
        activateDrag();
        
        selectedFeature = feature;
        
        g_oldDeviceId = $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text;

        //get center for polygons
        initialDevicecoords_x = (feature.geometry.getBounds().left + feature.geometry.getBounds().right)/2;
        initialDevicecoords_y = (feature.geometry.getBounds().top + feature.geometry.getBounds().bottom)/2;
        
        g_svgDType = 4;

        
        if(!deviceRoot || deviceRoot.length == 0)
        {
            document.getElementById("txtMonitorId").value ="";
            document.getElementById("txtLocation").value="";
            document.getElementById("txtNotes").value ="";
            document.getElementById("chkIsHallway").checked = false;
            g_dsvgId = 0;
            return;
        }
        
        
        document.getElementById("txtMonitorId").value = $(deviceRoot).children().filter('ZoneId')[i].textContent || $(deviceRoot).children().filter('ZoneId')[i].innerText || $(deviceRoot).children().filter('ZoneId')[i].text;
        document.getElementById("txtLocation").value = setundefined($(deviceRoot).children().filter('Location')[i].textContent || $(deviceRoot).children().filter('Location')[i].innerText || $(deviceRoot).children().filter('Location')[i].text);
        document.getElementById("txtNotes").value = setundefined(deviceRoot.children().filter('Notes')[i].textContent || deviceRoot.children().filter('Notes')[i].innerText || deviceRoot.children().filter('Notes')[i].text);
        document.getElementById("txtUnitName").value= setundefined(deviceRoot.children().filter('UnitName')[i].textContent || deviceRoot.children().filter('UnitName')[i].innerText || deviceRoot.children().filter('UnitName')[i].text);
            
            
        
            g_dsvgId =  feature .attributes['SvgId'];
            
        
            ShowControlsForSelectedDevice();
            $('#btnDrawRoom').removeClass('mapDrawRoom').removeAttr('class').addClass('mapDrawRoomDis');
            $("#btnDrawRoom").prop("disabled",true);
            
            if(g_svgDType == 4)
            {
                $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitorDis');
            }
            
            $("#btnPlaceMonitor").prop("disabled",true);
        
        return;
    }
   
}

function cancelClicked()
{
    
    if(unSavedfeatues.length > 0)
    {
        addMonitorLayer.removeFeatures(unSavedfeatues);
        unSavedfeatues.splice(0,unSavedfeatues.length);
        
        if(selectedFeature == lastDrawnRoom)
            selectedFeature = null;
        
        if(selectedFeature == lastDrawnDevice)
            selectedFeature = null;
        
        lastDrawnRoom = null;
        lastDrawnDevice = null;

    }
    
    polygonControl.deactivate();
    
    if(selectedFeature)
    {
        if(selectedFeature.attributes.description == "Wifi-Zones")
        {
            clicKEventForEditModePolygons(selectedFeature);
        }
        else
        {
            clicKEventForEditMode(selectedFeature);
        }
    }
 
}

function cancelEditForDevice()
{
    // move dragged feature to old position
    
     if((selectedFeature != lastDrawnDevice) && (selectedFeature != lastDrawnRoom))
     {
        if(initialDevicecoords_x)
        {
      
            var oldLonLat = new OpenLayers.LonLat(initialDevicecoords_x, initialDevicecoords_y);
            var oldPx = map.getViewPortPxFromLonLat(oldLonLat);
            selectedFeature.move(oldPx);
 
        }
        document.getElementById("txtMonitorId").value ="";
        document.getElementById("txtLocation").value="";
        document.getElementById("txtNotes").value ="";
        document.getElementById("chkIsHallway").checked = false;
        document.getElementById("txtUnitName").value ="";
        initialDevicecoords_x = null;
        initialDevicecoords_y = null;
        g_dsvgId = 0;
        g_oldDeviceId='';    
        
        
        $('#btnDrawRoom').removeAttr('class').addClass('mapDrawRoom');
         $("#btnDrawRoom").prop("disabled",false);
         
         if(g_svgDType == 1)
         {
             $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitor');
         }
         else
         {
             $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateStar');
         }  
            $("#btnPlaceMonitor").prop("disabled",false);
        
        
    }
 
}

// to create popup on feature Right click.

function createPopupForRightClick(feature)
{
    if(map.popups[0])
        map.removePopup(map.popups[0]);
    
    var popupDescription = "<div style ='width:230px;height:125px;'><b class = 'clsLALabeltitle'><u>Right Click</u></b><br>"
    + "<p>" + feature.attributes.description + "</p>"
    + "</div>";
    
    
    popup = new OpenLayers.Popup.FramedCloud("popup",
                                             feature.geometry.getBounds().getCenterLonLat(),
                                             null,
                                             popupDescription,
                                             null, true,removeHighlight );
    
    popup.setContentHTML(popupDescription);

    popup.autoSize = true;
    feature.popup = popup
    selectedFeature = feature;
    map.addPopup(feature.popup);
}

// to create popup on feature click.
function createPopup(feature)
{
    
    if(g_designMode == 1 && g_MapView === enumMapView.Map)
    {
        if(feature.attributes.description == "Wifi-Zones")
        {
            clicKEventForEditModePolygons(feature);
        }
        else
        {
            clicKEventForEditMode(feature);
        }
        
        getFeatureLocationInFeet(feature);
        
        return;
    }
    
    if(feature.layer.name.substring(0, 7) == 'Reports' && !feature.attributes.description)    {   
      
         var index = $.inArray(feature.layer,reportLayers);       
         controls['selector'].unselect(feature);  
        
        if(feature.layer.styleMap.styles['default'].defaultStyle.fontSize == '0px')
        {
            for(var m = 0 ; m < reportLayers[index].features.length; m++)
           {             
                if (navigator.userAgent.indexOf('MSIE') != -1) 
                {
                     reportLayers[index].features[m].layer.styleMap.styles['default'].defaultStyle.fontColor = "black";
                     reportLayers[index].features[m].layer.styleMap.styles.select.defaultStyle.fontColor = "black";   
                     reportLayers[index].features[m].layer.styleMap.styles['default'].defaultStyle.labelOutlineColor = "white"; 
                     reportLayers[index].features[m].layer.styleMap.styles.select.defaultStyle.labelOutlineColor = "white";                              
             
                }             
                     reportLayers[index].features[m].layer.styleMap.styles['default'].defaultStyle.fontSize = '17px';
                     reportLayers[index].features[m].layer.styleMap.styles.select.defaultStyle.fontSize = '17px';
                
                   
           }
        }
        else
        {      
           for(var m = 0 ; m < reportLayers[index].features.length; m++)
           {
               reportLayers[index].features[m].layer.styleMap.styles['default'].defaultStyle.fontSize = '0px';
               reportLayers[index].features[m].layer.styleMap.styles.select.defaultStyle.fontSize = '0px';
           }
        }                                        
            
        reportLayers[index].redraw();
        
        return;
        
    } 
   
    if(map.popups[0])
       map.removePopup(map.popups[0]);    
    
    clearHighLightLayer();
   
    
    var pos = feature.geometry;
    

    if(feature.attributes.description == "Infrastructure-Star")
    {  
        feature.attributes.description = getStarPopupDescription(feature .attributes['SVGID']);
    }
    else if(feature.attributes.description == "Infrastructure-Monitor")
    {
 
        feature.attributes.description = getMonitorPopupDescription(feature .attributes['SVGID']);
    }
    else if(feature.attributes.description == "Infrastructure-AccessPoints")
    {
        
        feature.attributes.description = getAccessPointPopupDescription(feature .attributes['SVGID']);
    }
    else if(feature.attributes.description == "Wifi-Zones")
    {
        feature.attributes.description = getWifiZonePopupDescription(feature .attributes['SvgId']);
    }
    
    
    
    
    if(feature.attributes.description == "Tag-Popup")
    {
         var popupContent = getTagPopupDescription(feature .attributes['SVGID']);
         
         popup = new OpenLayers.Popup.FramedCloud("popup",
                                             feature.geometry.getBounds().getCenterLonLat(),
                                             null,
                                             feature.attributes.description,
                                             null, true,removeHighlight );
    
         popup.setContentHTML(popupContent);
         
    }
    else
    {
   
         popup = new OpenLayers.Popup.FramedCloud("popup",
                                             feature.geometry.getBounds().getCenterLonLat(),
                                             null,
                                             feature.attributes.description,
                                            null, true,removeHighlight );
    
         popup.setContentHTML(feature.attributes.description);
    }
    
    popup.autoSize = true;      
    feature.popup = popup
    selectedFeature = feature;
    //feature.popup.closeOnMove = true;  
    map.addPopup(feature.popup);
}


// to destroy popup .
function destroyPopup(feature)
{
    if(feature.popup)
    {
          map.removePopup(feature.popup);
          feature.popup.destroy();
          // delete feature.popup;
          feature.popup = null;
    }
 
}

function getStarPopupDescription(svgID)
{
     var starRoot = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgID);}).parent(); 
     
     var i = 0;
     
     if(!starRoot)
        return "";
        
      if(starRoot.length == 0)
        return "";
   
     
     var starDeviceDetailLink = "<b class='clsLALabelMap'>Mac Id : </b><span class='DeviceDetailsLink' onclick=\"loadDeviceDetailsInfoOnClick(" + g_MapSiteId + ",3,'" + ($(starRoot).children().filter('MacId')[i].textContent || $(starRoot).children().filter('MacId')[i].innerText || $(starRoot).children().filter('MacId')[i].text)+ "')\">" +( $(starRoot).children().filter('MacId')[i].textContent || $(starRoot).children().filter('MacId')[i].innerText || $(starRoot).children().filter('MacId')[i].text )+ "</span><br>"
                             
     var popupDescription = "<div style ='width:230px;height:285px;'><b class = 'clsLALabeltitle'><u>Star Details</u></b><br>"
     + starDeviceDetailLink
     + "<b class = 'clsLALabelMap'>X  : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('Xaxis')[i].textContent || $(starRoot).children().filter('Xaxis')[i].innerText ||$(starRoot).children().filter('Xaxis')[i].text) +"</span><br>"
     + "<b class = 'clsLALabelMap'>Y  : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('Yaxis')[i].textContent || $(starRoot).children().filter('Yaxis')[i].innerText ||$(starRoot).children().filter('Yaxis')[i].text) +"</span><br>"
     + "<b class = 'clsLALabelMap'>Type  : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('CSVDeviceType')[i].textContent || $(starRoot).children().filter('CSVDeviceType')[i].innerText ||$(starRoot).children().filter('CSVDeviceType')[i].text) +"</span><br>"
     + "<b class = 'clsLALabelMap'>Sub Type  : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('StarType')[i].textContent || $(starRoot).children().filter('StarType')[i].innerText || $(starRoot).children().filter('StarType')[i].text) +"</span><br>"
     + "<b class = 'clsLALabelMap'>DHCP : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('DHCP')[i].textContent || $(starRoot).children().filter('DHCP')[i].innerText || $(starRoot).children().filter('DHCP')[i].text)+ "</span><br>"
     + "<b class = 'clsLALabelMap'>Save Settings : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('SaveSettings')[i].textContent || $(starRoot).children().filter('SaveSettings')[i].innerText || $(starRoot).children().filter('SaveSettings')[i].text) + "</span><br>"
     + "<b class = 'clsLALabelMap'>Static IP : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('StaticIP')[i].textContent || $(starRoot).children().filter('StaticIP')[i].innerText || $(starRoot).children().filter('StaticIP')[i].text) + "</span><br>"
     + "<b class = 'clsLALabelMap'>Subnet : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('Subnet')[i].textContent || $(starRoot).children().filter('Subnet')[i].innerText || $(starRoot).children().filter('Subnet')[i].text )+ "</span><br>"
     + "<b class = 'clsLALabelMap'>Gateway : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('Gateway')[i].textContent || $(starRoot).children().filter('Gateway')[i].innerText || $(starRoot).children().filter('Gateway')[i].text) + "</span><br>"
     + "<b class = 'clsLALabelMap'>Time Server IP : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('TimeServerIP')[i].textContent || $(starRoot).children().filter('TimeServerIP')[i].innerText || $(starRoot).children().filter('TimeServerIP')[i].text) + "</span><br>"
     + "<b class = 'clsLALabelMap'>Server IP : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('ServerIP')[i].textContent || $(starRoot).children().filter('ServerIP')[i].innerText || $(starRoot).children().filter('ServerIP')[i].text) + "</span><br>"
     + "<b class = 'clsLALabelMap'>Paging Server IP : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('PagingServerIP')[i].textContent || $(starRoot).children().filter('PagingServerIP')[i].innerText || $(starRoot).children().filter('PagingServerIP')[i].text) + "</span><br>"
     + "<b class = 'clsLALabelMap'>Location Server IP1 : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('LocationServerIP1')[i].textContent || $(starRoot).children().filter('LocationServerIP1')[i].innerText || $(starRoot).children().filter('LocationServerIP1')[i].text)+ "</span><br>"
     + "<b class = 'clsLALabelMap'>Location Server IP2 : </b><span  class = 'clsLALabelVal'>" + setundefined($(starRoot).children().filter('LocationServerIP2')[i].textContent || $(starRoot).children().filter('LocationServerIP2')[i].innerText || $(starRoot).children().filter('LocationServerIP2')[i].text)+ "</span><br>"
     + "</div>";
     
     return popupDescription;
}

function getMonitorPopupDescription(svgID)
{

   var monitorRoot = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgID);}).parent(); 
   
   var i = 0;
     
     if(!monitorRoot)
        return "";
        
      if(monitorRoot.length == 0)
        return "";

  var monitorDeviceDetailLink = "<b class = 'clsLALabelMap'>Monitor Id : </b><span  class = 'clsLALabelVal' >" +( monitorRoot.children().filter('DeviceId')[i].textContent || monitorRoot.children().filter('DeviceId')[i].innerText || monitorRoot.children().filter('DeviceId')[i].text)+ "</span><br>";
  var profile=  monitorRoot.children().filter('Profile')[i].textContent || monitorRoot.children().filter('Profile')[i].innerText || monitorRoot.children().filter('Profile')[i].text;               
  var deviceid = monitorRoot.children().filter('DeviceId')[i].textContent || monitorRoot.children().filter('DeviceId')[i].innerText || monitorRoot.children().filter('DeviceId')[i].text;
   if(profile != "" || profile != null)   
    monitorDeviceDetailLink = "<b class = 'clsLALabelMap'>Monitor Id : </b><span  class = 'DeviceDetailsLink' onclick='loadDeviceDetailsInfoOnClick(" + g_MapSiteId + ",2," + deviceid + ")'>" + deviceid + "</span><br>";
   var popupDescription = "<div  style ='width:230px;height:345px;'><b class = 'clsLALabeltitle'><u>Monitor Details</u></b><br>"
   + monitorDeviceDetailLink
   + "<b class = 'clsLALabelMap'>Type  : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('CSVDeviceType')[i].textContent || monitorRoot.children().filter('CSVDeviceType')[i].innerText || monitorRoot.children().filter('CSVDeviceType')[i].text)+ "</span><br>"
   + "<b class = 'clsLALabelMap'>Sub Type  : </b><span  class = 'clsLALabelVal'>" + setundefined(monitorRoot.children().filter('MonitorType')[i].textContent || monitorRoot.children().filter('MonitorType')[i].innerText || monitorRoot.children().filter('MonitorType')[i].text)+ "</span><br>"
   + "<b class = 'clsLALabelMap'>Location : </b><span  class = 'clsLALabelVal'>" + setundefined(monitorRoot.children().filter('Location')[i].textContent || monitorRoot.children().filter('Location')[i].innerText || monitorRoot.children().filter('Location')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Notes : </b><span  class = 'clsLALabelVal'>" + setundefined(monitorRoot.children().filter('Notes')[i].textContent || monitorRoot.children().filter('Notes')[i].innerText || monitorRoot.children().filter('Notes')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>X : </b><span  class = 'clsLALabelVal'>" + setundefined(monitorRoot.children().filter('Xaxis')[i].textContent || monitorRoot.children().filter('Xaxis')[i].innerText || monitorRoot.children().filter('Xaxis')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Y : </b><span  class = 'clsLALabelVal'>" + setundefined(monitorRoot.children().filter('Yaxis')[i].textContent || monitorRoot.children().filter('Yaxis')[i].innerText || monitorRoot.children().filter('Yaxis')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Profile : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('Profile')[i].textContent || monitorRoot.children().filter('Profile')[i].innerText || monitorRoot.children().filter('Profile')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>IRProfile : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('IRProfile')[i].textContent || monitorRoot.children().filter('IRProfile')[i].innerText || monitorRoot.children().filter('IRProfile')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>PowerLevel : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('PowerLevel')[i].textContent || monitorRoot.children().filter('PowerLevel')[i].innerText || monitorRoot.children().filter('PowerLevel')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap' >Room Bleeding : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('RoomBleeding')[i].textContent || monitorRoot.children().filter('RoomBleeding')[i].innerText || monitorRoot.children().filter('RoomBleeding')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Noise Level : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('NoiseLevel')[i].textContent || monitorRoot.children().filter('NoiseLevel')[i].innerText || monitorRoot.children().filter('NoiseLevel')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Masking : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('Masking')[i].textContent || monitorRoot.children().filter('Masking')[i].innerText || monitorRoot.children().filter('Masking')[i].text)+ "</span><br>"
   + "<b class = 'clsLALabelMap'>Master Slave : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('MasterSlave')[i].textContent || monitorRoot.children().filter('MasterSlave')[i].innerText || monitorRoot.children().filter('MasterSlave')[i].text)+ "</span><br>"
   + "<b class = 'clsLALabelMap'>Special Profile : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('SpecialProfile')[i].textContent || monitorRoot.children().filter('SpecialProfile')[i].innerText || monitorRoot.children().filter('SpecialProfile')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Operating Mode : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('OperatingMode')[i].textContent || monitorRoot.children().filter('OperatingMode')[i].innerText || monitorRoot.children().filter('OperatingMode')[i].text)+ "</span><br>"
   + "<b class = 'clsLALabelMap'>Modes : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('Modes')[i].textContent || monitorRoot.children().filter('Modes')[i].innerText || monitorRoot.children().filter('Modes')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Alert Supression Time : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('AlertSupressionTime')[i].textContent || monitorRoot.children().filter('AlertSupressionTime')[i].innerText || monitorRoot.children().filter('AlertSupressionTime')[i].text)+ "</span><br>"
   + "<b class = 'clsLALabelMap'>Locked Star : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('LockedStarId')[i].textContent || monitorRoot.children().filter('LockedStarId')[i].innerText || monitorRoot.children().filter('LockedStarId')[i].text) + "</span><br>"
   + "<b class = 'clsLALabelMap'>Star Location : </b><span  class = 'clsLALabelVal'>" +  setundefined(monitorRoot.children().filter('StarLocation')[i].textContent || monitorRoot.children().filter('StarLocation')[i].innerText || monitorRoot.children().filter('StarLocation')[i].text) + "</span><br>"
   + "</div>";
   
   return popupDescription;
}


function getAccessPointPopupDescription(svgID)
{
    
    var accessPointRoot = $(g_accessPointRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgID);}).parent();
    
    var i = 0;
    
    if(!accessPointRoot)
        return "";
    
    if(accessPointRoot.length == 0)
        return "";
    
    var popupDescription = "<div  style ='width:230px;height:150px;'><b class = 'clsLALabeltitle'><u>AccessPoint Details</u></b><br>"
    + "<b class = 'clsLALabelMap'>MAC Id: </b><span  class = 'clsLALabelVal'>" +  setundefined(accessPointRoot.children().filter('DeviceId')[i].textContent || accessPointRoot.children().filter('DeviceId')[i].innerText || accessPointRoot.children().filter('DeviceId')[i].text)+ "</span><br>"
    + "<b class = 'clsLALabelMap'>Description  : </b><span  class = 'clsLALabelVal'>" + setundefined(accessPointRoot.children().filter('Description')[i].textContent || accessPointRoot.children().filter('Description')[i].innerText || accessPointRoot.children().filter('Description')[i].text)+ "</span><br>"
    + "<b class = 'clsLALabelMap'>Location : </b><span  class = 'clsLALabelVal'>" + setundefined(accessPointRoot.children().filter('Location')[i].textContent || accessPointRoot.children().filter('Location')[i].innerText || accessPointRoot.children().filter('Location')[i].text) + "</span><br>"
    + "<b class = 'clsLALabelMap'>Notes : </b><span  class = 'clsLALabelVal'>" + setundefined(accessPointRoot.children().filter('Notes')[i].textContent || accessPointRoot.children().filter('Notes')[i].innerText || accessPointRoot.children().filter('Notes')[i].text) + "</span><br>"
    + "<b class = 'clsLALabelMap'>UnitName : </b><span  class = 'clsLALabelVal'>" +  setundefined(accessPointRoot.children().filter('UnitName')[i].textContent || accessPointRoot.children().filter('UnitName')[i].innerText || accessPointRoot.children().filter('UnitName')[i].text) + "</span><br>"
    + "<b class = 'clsLALabelMap'>X : </b><span  class = 'clsLALabelVal'>" +  setundefined(accessPointRoot.children().filter('Xaxis')[i].textContent || accessPointRoot.children().filter('Xaxis')[i].innerText || accessPointRoot.children().filter('Xaxis')[i].text) + "</span><br>"
    + "<b class = 'clsLALabelMap'>Y : </b><span  class = 'clsLALabelVal'>" +  setundefined(accessPointRoot.children().filter('Yaxis')[i].textContent || accessPointRoot.children().filter('Yaxis')[i].innerText || accessPointRoot.children().filter('Yaxis')[i].text) + "</span><br>"
    + "</div>";
    
    return popupDescription;
}
function getWifiZonePopupDescription(svgID)
{
    var zoneRoot = $(g_wifiZones).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgID);}).parent();
    
    var i = 0;
    
   if(!zoneRoot)
        return "";
    
    if(zoneRoot.length == 0)
        return "";
    
    
    
    
    var popupDescription = "<div style ='width:230px;height:150px;'><b class = 'clsLALabeltitle'><u>Zone Details</u></b><br>"
    + "<b class = 'clsLALabelMap'>Zone Id  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('ZoneId')[i].textContent || $(zoneRoot).children().filter('ZoneId')[i].innerText ||$(zoneRoot).children().filter('ZoneId')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>Type  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('CSVDeviceType')[i].textContent || $(zoneRoot).children().filter('CSVDeviceType')[i].innerText ||$(zoneRoot).children().filter('CSVDeviceType')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>Location  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('Location')[i].textContent || $(zoneRoot).children().filter('Location')[i].innerText ||$(zoneRoot).children().filter('Location')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>Notes  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('Notes')[i].textContent || $(zoneRoot).children().filter('Notes')[i].innerText ||$(zoneRoot).children().filter('Notes')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>UnitName  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('UnitName')[i].textContent || $(zoneRoot).children().filter('UnitName')[i].innerText ||$(zoneRoot).children().filter('UnitName')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>X  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('Xaxis')[i].textContent || $(zoneRoot).children().filter('Xaxis')[i].innerText ||$(zoneRoot).children().filter('Xaxis')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>Y  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('Yaxis')[i].textContent || $(zoneRoot).children().filter('Yaxis')[i].innerText ||$(zoneRoot).children().filter('Yaxis')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>Width  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('WidthFt')[i].textContent || $(zoneRoot).children().filter('WidthFt')[i].innerText ||$(zoneRoot).children().filter('WidthFt')[i].text) +"</span><br>"
    + "<b class = 'clsLALabelMap'>Length  : </b><span  class = 'clsLALabelVal'>" + setundefined($(zoneRoot).children().filter('LengthFt')[i].textContent || $(zoneRoot).children().filter('LengthFt')[i].innerText ||$(zoneRoot).children().filter('LengthFt')[i].text) +"</span><br>"
    + "</div>";
    
    return popupDescription;
}



function getTagPopupDescription(svgID)
{

    var popupDescription = "";
    
    for(var j = 0;j < tagDetailsArr.length;j++)
    {
        if(tagDetailsArr[j].SvgId == svgID)
        {
            //var index = tagInvisibleLayerArray.indexOf(tagDetailsArr[j].TagTypeName);
            var index=$.inArray(tagDetailsArr[j].TagTypeName.toString(),tagInvisibleLayerArray);
                    
            if(index < 0)
            {
               popupDescription = popupDescription + "<div  style ='width:230px;height:160px;'><b class = 'clsLALabeltitle'><u>Tag Details</u></b><br>"
               + "<b class = 'clsLALabelMap'>Tag Id : </b><span  class = 'DeviceDetailsLink' onclick='loadDeviceDetailsInfoOnClick(" + g_MapSiteId + ",1," + tagDetailsArr[j].tagID + ")'>" +  tagDetailsArr[j].tagID + "</span><br>"
               + "<b class = 'clsLALabelMap'>Tag Type : </b><span  class = 'clsLALabelVal'>" +  tagDetailsArr[j].TagTypeName + "</span><br>"
               + "<b class = 'clsLALabelMap'>Monitor Id : </b><span  class = 'clsLALabelVal'>" +  tagDetailsArr[j].MonitorId + "</span><br>"
               + "<b class = 'clsLALabelMap'>Model Item : </b><span  class = 'clsLALabelVal'>" +   tagDetailsArr[j].ModelItem + "</span><br>"
               + "<b class = 'clsLALabelMap'>Battery Capacity : </b><span  class = 'clsLALabelVal'>" +  tagDetailsArr[j].BatteryCapacity + "</span><br>"
               + "<b class = 'clsLALabelMap'>Room Seen : </b><span  class = 'clsLALabelVal'>" +  tagDetailsArr[j].RoomSeen + "</span><br>"
               + "<b class = 'clsLALabelMap'>Locked Star : </b><span  class = 'clsLALabelVal'>" +  tagDetailsArr[j].LockedStarId + "</span><br>"
               + "<b class = 'clsLALabelMap'>Star Location : </b><span  class = 'clsLALabelVal'>" +  tagDetailsArr[j].StarLocation + "</span><br>"
               + "</div></br>";
            }
        }
    }
    
    return popupDescription;
  
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////      ADD LINK LINE TO INFRASTRUCTURE AND TAG IN MAP    /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addLinkLine(svgID1,svgId2,isStarMonitorLink,deviceType)
{
    
    var point1 = new Array();
    var point2 = new Array();
    
    
    if(isStarMonitorLink == '0')
    {
       var tagCoordinates = getTagCoordinatesInfoArray(svgID1[4]);
       var awaydistance = 0;  
       awaydistance = tagCoordinates[1];
       
       point1 = [(tagCoordinates[0][0]+tagCoordinates[0][2]+awaydistance)/2,(tagCoordinates[0][1]+tagCoordinates[0][3]+awaydistance)/2]; 
       
       for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
        {
            if(svgId2 == InfraStructureCoordinatesArr[i][4])
            {
                point2 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                break;
            }
            
        }
    
    }
    else
    {
       for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
        {
            if(svgID1 == InfraStructureCoordinatesArr[i][4])
            {
                point1 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
            }
            if(svgId2 == InfraStructureCoordinatesArr[i][4])
            {
                point2 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
            }
            
        }
    
    }
    
    var points = new Array(
                           new OpenLayers.Geometry.Point(point1[0], point1[1]),
                           new OpenLayers.Geometry.Point(point2[0], point2[1])
                           );
    
    var cColor ='#06E9E1';
    if(isStarMonitorLink === "1")
        cColor = '#F23223';
    
    var style = {
    strokeColor: cColor,
    strokeOpacity: 0.7,
    strokeWidth: 3
    };
    
    var line = new OpenLayers.Geometry.LineString(points);
    var lineFeature = new OpenLayers.Feature.Vector(line, null, style);
    
  
    
    if(isStarMonitorLink === "1")
    {
        lineFeature.style.display = true;
        lineFeature.attributes['monitorType'] = "Infrastructure - " + deviceType[0];
        lineFeature.attributes['starType'] = "Infrastructure - Stars";
        lineFeature.attributes['isShowMonitorLink'] = "Yes";
        starMonitorLinkLayer.addFeatures([lineFeature]);
        
        
       
    }
    else
    {
        lineFeature.style.display = true;
        lineFeature.attributes['tagType'] = "Tag - " + deviceType[0];
        lineFeature.attributes['starType'] = "Infrastructure - Stars";
        lineFeature.attributes['SVGID'] = String(svgID1[4]);
        starTagLinkLayer.addFeatures([lineFeature]);   
    } 
    
    point1 = null;
    point2 = null;
    points = null;
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////                      SEARCH DEVICES                   /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function searchDevice(deviceId)
{
    deviceId = deviceId.replace(/-/g, "_");
    deviceId = deviceId.replace(/\./g, "_");
    deviceId = trim(deviceId);
    if(map.popups[0])
    {
        map.removePopup(map.popups[0]);
       
    }
    
    clearHighLightLayer();
   
    
   
          
       for (var j = 0; j < InfraStructureCoordinatesArr.length; j++)
       {
               if(InfraStructureResponseArr[j])
               {
                   if(InfraStructureResponseArr[j].deviceId ==  deviceId)
                   {
                        map.setCenter(new OpenLayers.LonLat((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2,(InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2), 0.5);                      
                         
                         var feature ;
                         feature = LAyersArray[InfraStructureResponseArr[j].layerIndex].getFeaturesByAttribute('infraStructureID',InfraStructureCoordinatesArr[j][4]);
                        
                         if(feature)
                         {
                                 controls['selector'].select(feature[0]);
                               // Create a point feature to show the label offset options
                                    var labelOffsetPoint = new OpenLayers.Geometry.Point(((InfraStructureCoordinatesArr[j][0]+InfraStructureCoordinatesArr[j][2])/2),((InfraStructureCoordinatesArr[j][1]+InfraStructureCoordinatesArr[j][3])/2)).transform(epsg4326, projectTo);
                                    InfraStructureHighLightLayer = new OpenLayers.Feature.Vector(labelOffsetPoint);
                                    InfraStructureHighLightLayer.attributes = {
                                    name:"",
                                    Radius : 50,
                                    align: "cm",                                                                      
                                    xOffset: 25,
                                    yOffset: 25                            
                                    };
                                    
                             Rooms.addFeatures(InfraStructureHighLightLayer);
                            
                             return;
                         }
                         
                        
                        return;
                   }
               }   
               
       }
    
    for( var i = 0, len = tagDetailsArr.length; i < len; i++ )
    {
        if( tagDetailsArr[i].tagID == deviceId)
        {
            for( var j = 0; j < roomDetails.length ; j++ )
            {
              
                if(tagDetailsArr[i].SvgId == roomDetails[j][4])
                {
                        var tagHighLightLayer;
                        var popupDetailArr = checkMultiTagRoom(tagDetailsArr[i].SvgId)
                
                        if(popupDetailArr[0] > 1) // MultiTag
                        {
                            tagHighLightLayer = multiTagLayer;
                        }
                        else
                        {
                              for(var m=0; m < LAyersArray.length ; m++)
                                {                                                     
                                  var res = LAyersArray[m].name.substring(6,  LAyersArray[m].name.length);
                                  
                                      if(res == tagDetailsArr[i].TagTypeName)
                                      {
                                        tagHighLightLayer = LAyersArray[m];
                                        break;
                                   
                                      } 
                                }
                        }
                                                
                  
         
                                 
                    map.setCenter(new OpenLayers.LonLat((roomDetails[j][0]+roomDetails[j][2])/2,(roomDetails[j][1]+roomDetails[j][3])/2), 0.5);
                    
                    controls['selector'].select(tagHighLightLayer.getFeaturesByAttribute('roomID',tagDetailsArr[i].SvgId)[0]);
                    //createPopup(tagMarKerLayer.getFeaturesByAttribute('roomID',tagDetailsArr[i].roomID)[0]);
                    HighLightedRoom = Rooms.getFeaturesByAttribute('roomNO',"room"+  tagDetailsArr[i].SvgId)[0];
                    PreviousStyleForHighlighedRoom = HighLightedRoom.style;
                    
                    HighLightedRoom.style = highlightedRoomStyle;
                    Rooms.drawFeature(HighLightedRoom);
                    
                   multiTagLayer.setVisibility(true);
                  
                    
                    return;
                    
                }

            }
            
            break;
        }
        
    }
    
    alert("Device Not Found.");

}

// clear highlght on a feature (added from search device function).
function clearHighLightLayer()
{
      if(map.popups[0])
      {
        map.removePopup(map.popups[0]);
      }
    
      if(InfraStructureHighLightLayer)
       {
          Rooms.removeFeatures(InfraStructureHighLightLayer);
          InfraStructureHighLightLayer = '';
       }
       
       if(HighLightedRoom)
       {
           HighLightedRoom.style = PreviousStyleForHighlighedRoom;
           Rooms.drawFeature(HighLightedRoom);
       }
       
}


// remove highlght on a feature (added from search device function).
function removeHighlight()
{
    if(selectedFeature)
        destroyPopup(selectedFeature);
    if(HighLightedRoom)
        Rooms.drawFeature(HighLightedRoom, PreviousStyleForHighlighedRoom);
    
    if(map.popups[0])
        map.removePopup(map.popups[0]);

}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////             GET DEVICE IMAGE TO PIN                  /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getDeviceImage(deviceName,deviceType)
{
    var imagedetailArr = new Array();
                           
    imagedetailArr[0] = {imageName:'',imageWidht:0,imageheight:0};
    
       if(deviceName == 'Star')
       {
           imagedetailArr[0] = {imageName:'images/mapStars.png',imageWidht:22,imageheight:22};
           
            return imagedetailArr;
       }
       else if(deviceName == 'Monitor' || deviceName == 'Regular Monitor')
       {
           
           imagedetailArr[0] = {imageName:'images/map/mapmonitors.png',imageWidht:22,imageheight:22};

            return imagedetailArr;
       }
       else if(deviceName == 'MM Monitor')
       {
           
           imagedetailArr[0] = {imageName:'images/map/mapmmmonitors.png',imageWidht:22,imageheight:22};

            return imagedetailArr;
       }
       else if(deviceName == 'DIM')
       {
           imagedetailArr[0] = {imageName:'images/map/mapDimss.png',imageWidht:15,imageheight:24};

            return imagedetailArr;
       }
       else if(deviceName == 'EGRESS')
       {
           imagedetailArr[0] = {imageName:'images/map/mapLF-Excitiers.png',imageWidht:18,imageheight:24};

            return imagedetailArr;
       }
    
    
        if(deviceType == 'Star')
        {
            imagedetailArr[0] = {imageName:'images/map/mapStars.png',imageWidht:22,imageheight:22};
            return imagedetailArr;
        }
        else if(deviceType == 'Monitor')
        {
            imagedetailArr[0] = {imageName:'images/map/mapmonitors.png',imageWidht:22,imageheight:22};
            return imagedetailArr;
        }
        else if(deviceType == 'Accesspoint')
        {
            imagedetailArr[0] = {imageName:'images/map/mapAccesspoint.png',imageWidht:22,imageheight:22};
            return imagedetailArr;
        }
    
    return   imagedetailArr;
   
}

function DrawRoom()
{
    rectDrawControl.activate();
}

function PlaceMonitor()
{
   // tagMarKerLayer.destroyFeatures();
    //document.getElementById("showAddedDeviceBtn").disabled=false;
    //document.getElementById("addDeviceBtn").disabled=true;
    
    //document.getElementById('deviceInfo').style.display = 'block';
    
    controls['selector'].deactivate();
    drag.activate();
    mouseclick.activate();
}

function showAddedDevice()
{
  
    
    tagMarKerLayer.destroyFeatures();
    
    if(deviceInfoArr.length == 0)
    { 
        alert("No devices are added yet");
    }
    
    for(var i=0; i < deviceInfoArr.length; i++)
    {
         var feature = new OpenLayers.Feature.Vector
                                (
                                 new OpenLayers.Geometry.Point(deviceInfoArr[i].xpos, convertToSvgYCoordinate(parseFloat(deviceInfoArr[i].ypos))).transform(epsg4326, projectTo),
                                 {description: "<div style ='height:115px;' ><p><b><u>Tag Details</u><br></b>device ID:" + deviceInfoArr[i].deviceID + "<br>Selected Room ID :" + deviceInfoArr[i].roomID +  "<p></div>"} ,
                                 {externalGraphic: getDeviceImage(deviceInfoArr[i].deviceName), graphicHeight: 30, graphicWidth: 30, graphicXOffset:-15, graphicYOffset:-15  }
                                 );
                               
                         
                    tagMarKerLayer.addFeatures(feature);
    }
    
     document.getElementById('deviceInfo').style.display = 'none';
  
    document.getElementById("showAddedDeviceBtn").disabled=true;
    document.getElementById("addDeviceBtn").disabled=false;
    
    controls['selector'].activate();
    drag.deactivate();
    mouseclick.deactivate();

}

function clearHighLightLayer()
{
      if(map.popups[0])
      {
        map.removePopup(map.popups[0]);
      }
    
      if(InfraStructureHighLightLayer)
       {
          Rooms.removeFeatures(InfraStructureHighLightLayer);
          InfraStructureHighLightLayer = '';
       }
       
       if(HighLightedRoom)
       {
           HighLightedRoom.style = PreviousStyleForHighlighedRoom;
           Rooms.drawFeature(HighLightedRoom);
       }
       
}


function getTagImageByType(deviceType)
{

     /* enum_AssetTAG = 1
        enum_MMAssetTAG = 2
        enum_StaffTAG = 3
        enum_MMStaffTAG = 4
        enum_TempTag = 5
        enum_ERUTag = 6
        enum_HumidityTag = 7
        enum_PatientTag = 8*/
            
                          
      var imagedetailArr = new Array();
                           
       imagedetailArr[0] = {imageName:'Images/map/mapAsset_tag.png',imageWidht:30,imageheight:30};
       
       if(deviceType == "")
       {
          imagedetailArr[0] = {imageName:'Images/map/mapAsset_tag.png',imageWidht:30,imageheight:30};
       }
       else if(deviceType == -1)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapMultiTags.png',imageWidht:33,imageheight:30};

            return imagedetailArr;
       }
       else if(deviceType == 0)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapAsset_tag.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
        else if(deviceType == 2)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapmm_assetTag.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
        else if(deviceType == 3 || deviceType == 1)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapStaff_tag.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
        else if(deviceType == 4)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapmm_Staff_tag.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
       
        else if(deviceType == 5)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapTemperature_icon.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
         else if(deviceType == 6)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapcallpoints_tag.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
         else if(deviceType == 7)
       {
           imagedetailArr[0] = {imageName:'Images/map/mapTemperature_icon.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
         else if(deviceType == 8)
       {
           imagedetailArr[0] = {imageName:'Images/map/mappatient_tag.png',imageWidht:30,imageheight:30};

            return imagedetailArr;
       }
       
        return imagedetailArr;

}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////             GET ROOM COLOR FOR PROFILEID              /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function getRoomColorForProfileId(profileId)
{
    var RoomStyle = OpenLayers.Util.applyDefaults(RoomStyle, OpenLayers.Feature.Vector.style['default']);
        RoomStyle.fill = true;
        RoomStyle.fillOpacity = 0.6;
        
        if(profileId==="0")
            RoomStyle.fillColor = "#DA890A";
        else if (profileId==="1")
            RoomStyle.fillColor = "#694403";
         else if (profileId==="2")
            RoomStyle.fillColor = "#694D03";
         else if (profileId==="3")
            RoomStyle.fillColor = "#FC05C5";
         else if (profileId==="4")
            RoomStyle.fillColor = "#FC051C";
         else if (profileId==="5")
            RoomStyle.fillColor = "#05A8FC";
         else if (profileId==="6")
            RoomStyle.fillColor = "#092720";
         else if (profileId==="7")
            RoomStyle.fillColor = "#A42C03";
         else if (profileId==="8")
            RoomStyle.fillColor = "#05FC4B";
         else if (profileId==="9")
            RoomStyle.fillColor = "#057FFC";
         else if (profileId==="10")
            RoomStyle.fillColor = "#053FFC";
         else if (profileId==="11")
            RoomStyle.fillColor = "#596903";
         else if (profileId==="12")
            RoomStyle.fillColor = "#05FCA2";
         else if (profileId==="13")
            RoomStyle.fillColor = "#3F05FC";
         else if (profileId==="14")
            RoomStyle.fillColor = "#2E6902";
         else if (profileId==="15")
            RoomStyle.fillColor = "#E205FC";
         else if (profileId==="16")
            RoomStyle.fillColor = "#FC0585";
         else if (profileId==="17")
            RoomStyle.fillColor = "#9605FC";
         else if (profileId==="18")
            RoomStyle.fillColor = "#05EDFC";
         else if (profileId==="19")
            RoomStyle.fillColor = "#6F6D76";
         else if (profileId==="20")
            RoomStyle.fillColor = "#7F05FC";
        else
            RoomStyle.fillColor = "#DA890A";

    RoomStyle.strokeOpacity  =  1;
    RoomStyle.strokeWidth = 1.5;
    RoomStyle.strokeColor = RoomStyle.fillColor;
    return RoomStyle;
}

function convertToSvgEcllipseXCoordinate(xpos)
{
    return xpos-7.5;
}

function convertToSvgEcllipseYCoordinate(ypos)
{
   ypos = (ImageHeight - ypos);
   return ypos-7.5;
   
}
function convertToSvgYCoordinateForRoom(ypos,height)
{
   ypos = (ypos + (height -  ypos));
   ypos = (ImageHeight - ypos);
   return ypos;
}

function convertToSvgWidth(xpos,width)
{
   width = (width -  xpos);

   return width;
}

function convertToSvgHeight(ypos,height)
{
   height = (height -  ypos);

   return height;
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////        ADD REPORT LAYER BASED ON SHAPE TYPE           /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addNewDrawReportLayerToMap(layerName)
{
      
    var renderer = OpenLayers.Util.getParameters(window.location.href).renderer;
    renderer = (renderer) ? [renderer] : OpenLayers.Layer.Vector.prototype.renderers;  
   
    
       var styledef = new OpenLayers.Style({

                                        strokeColor: "${strokeColor}",
                                        strokeOpacity: "${strokeOpacity}",
                                        strokeWidth: "${strokeWidth}",
                                        fillColor: "${fillcolor}",
                                        fillOpacity: "${fillopacity}",
                                        pointRadius: "${radius}",
                                        pointerEvents: "visiblePainted",
                                        display : "$(display)",                                     
                                        label : "${name}",
                                        fontColor: "white",
                                        fontSize: "0px",
                                        fontFamily: "Courier New, monospace",
                                        fontWeight: "bold",
                                        labelOutlineColor: "black",
                                        labelOutlineWidth: 5,                                       
                                        labelXOffset: "8",
                                        labelYOffset: "25",
                                        align: "left",
                                        graphicZIndex: 0
                                        
                                        }, {
                                        context: {
                                        radius: function (feature) {
                                        var pix = REPORTLAYER_RADIUS / map.getResolution() * .703125 ; // ten time the zoo level
                                        return pix;
                                        }
                                        }
                                        });
    
    
    var styleSel = new OpenLayers.Style({
                                        strokeColor: "${strokeColor}",
                                        strokeOpacity: "${strokeOpacity}",
                                        strokeWidth: "${strokeWidth}",
                                        fillColor: "${fillcolor}",
                                        fillOpacity: "${fillopacity}",
                                        pointRadius: "${radius}",
                                        pointerEvents: "visiblePainted",
                                        display : "$(display)",                                     
                                        label : "${name}",
                                        fontColor: "white",
                                        fontSize: "17px",
                                        fontFamily: "Courier New, monospace",
                                        fontWeight: "bold",
                                        labelOutlineColor: "black",
                                        labelOutlineWidth: 5,                                    
                                        labelXOffset: "8",
                                        labelYOffset: "25",
                                        align: "left",
                                        graphicZIndex: 0

                                        }, {
                                        context: {
                                        radius: function (feature) {
                                        var pix = REPORTLAYER_RADIUS / map.getResolution() * .703125 ; // ten time the zoo level                                      
                                        return pix;
                                        }
                                        }
                                        });

    var  layer = new OpenLayers.Layer.Vector("Reports - " + layerName, {
                                                 styleMap: new OpenLayers.StyleMap({'default':styledef,'select':styleSel})
                                                 });
    
   layer.visibility = true;
    
    LAyersArray[LAyersArray.length] = layer;   
    reportLayers[reportLayers.length] = layer;
  
    
}

function DrawSearchReportLayer(LayerName,DeviceId,DeviceType,svgid,Bin,FloorId,FloorName,Volume)
{
     // need to calculate
    var shapeOpacity = 0.5;//Volume/100;
    var ShapeColor = "#0681FE";
    
    var isLayerAdded = false;
    for(var l = 0; l < reportLayers.length; l++)
    {
       if(reportLayers[l].name == 'Reports - ' + LayerName)
       {
         isLayerAdded = true;
       }
    }
    
    if(!isLayerAdded)
    {
        addNewDrawReportLayerToMap(LayerName);
        reportLayers[reportLayers.length - 1].addOptions({deviceType: DeviceType});
        reportLayers[reportLayers.length - 1].addOptions({LayerType: "Search"});
        
        reportLayers[reportLayers.length - 1].styleMap.styles['default'].defaultStyle.fontSize = '0px';
        reportLayers[reportLayers.length - 1].styleMap.styles.select.defaultStyle.fontSize = '0px';

    }
    
        var point1 = new Array();
        
        if(DeviceType == "Monitor"  || DeviceType == "Star")
        {
            for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
            {
                if(svgid == InfraStructureCoordinatesArr[i][4])
                {             
                    point1 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                    break;
                }              
                
            } 
        
        }
        else
        {
            for(var i=0;i < roomDetails.length ; i++)
            {
                //console.log(roomDetails[i][4]);
                if(svgid == roomDetails[i][4])
                {             
                    point1 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];
                    break;
                }
                
            } 
        }        
      
        
         var tempFeature = reportLayers[reportLayers.length -1].getFeaturesByAttribute('Search_SVGID',svgid)[0];
         var HighLightLayer ;
         var tagTempfeature ;
         
        if(!tempFeature)
        {
        
            var labelOffsetPoint = new OpenLayers.Geometry.Point(point1[0], point1[1]);
            HighLightLayer = new OpenLayers.Feature.Vector(labelOffsetPoint);
            
            tempFeature = HighLightLayer;
            
            HighLightLayer.attributes = {
                
            name :  Volume,
            Radius : SEARCH_REPORTLAYER_RADIUS,
            align: "cm",
                
            fillcolor : ShapeColor,
            fillopacity: shapeOpacity,
            strokeColor : "#000000",
            xOffset: SEARCH_REPORTLAYER_RADIUS/2,
            yOffset: SEARCH_REPORTLAYER_RADIUS/2,
            display: "block",
            strokeWidth : 1.5,
            strokeOpacity : 0.3
                
            };
            
             tempFeature.attributes['Search_SVGID'] = svgid;
            
             if(DeviceType == "Tag" )
             {
                
                   tempFeature.attributes['description'] = '';
                   tempFeature.attributes['descriptionContent'] = '';
                   
                   
                   if(tagDetailsArr.length == 0)
                   {           var imgDetail =  getTagImageByType(-2);
                               tagTempfeature = new OpenLayers.Feature.Vector
                                (
                                 new OpenLayers.Geometry.Point(point1[0], point1[1]),
                                 {description: '-' } ,
                                 {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                                 );
                                
                            
                                tagTempfeature .attributes['tempTag'] = "tempTag";
                                
                   }
             }
          
        }
        
         if(DeviceType == "Tag" )
         {        
                 tempFeature.attributes['descriptionContent'] = tempFeature.attributes['descriptionContent'] 
                                                                       + "<br>"
                                                                       + "<span><b class = 'clsLALabelMap'>" + DeviceType + " Id  : </b><span  class = 'clsLALabelVal'>" +  DeviceId + "</span><br>"
                                                                       + "<b class = 'clsLALabelMap'>Value  : </b><span  class = 'clsLALabelVal'>" +  Volume + "</span></span>"
                                                                       + "<br>";               
                                                                                           
                        
                       
                       
                tempFeature.attributes['description'] = "<div style ='width:180px;height:70px;'><b class = 'clsLALabeltitle'><u>Reports</u></b>" +
                tempFeature.attributes['descriptionContent'] + "<div>";
                if(tagTempfeature) 
                   tagTempfeature .attributes['description'] = tempFeature.attributes['description'];
         }
       
        
         if(DeviceType == "Star")
         {
            tempFeature.attributes['Type'] = "Infrastructure - Stars";     
         }  
         else if(DeviceType == "Monitor")
         {
            
            var monitorType;
            
            var LockedStar = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                monitorType=$(LockedStar).children().filter('MonitorType')[0].textContent;
            }

            
            tempFeature.attributes['Type'] = "Infrastructure - " + monitorType;  
            
        }
        else 
        {        
            
            for(i = 0; i < tagDetailsArr.length ; i++)
            {
                if(svgid == tagDetailsArr[i].SvgId)
                {             
                     tempFeature.attributes['Type'] = "Tag - " + tagDetailsArr[i].TagTypeName;    
                     break;              
                }
               
            }           
       
        
        }

       if(HighLightLayer)
       {
           reportLayers[reportLayers.length -1].addFeatures(HighLightLayer); 
       }
       
       if(tagTempfeature)
       {
           reportLayers[reportLayers.length -1].addFeatures(tagTempfeature); 
       }
       
  
       
}

function DrawReportLayer(LayerName,Deviceid,DeviceType,svgid,ShapeType,ShapeColor,Volume,ToSvgid)
{
    // need to calculate
    var shapeOpacity = Volume/100;
    
     if(LayerName == "StarHeatMap")
    {
       if(Volume < 0)      
           shapeOpacity = 1.0 - (-(Volume/100));      
       else    
           shapeOpacity = 1.0 ;    
    }
    
    if(LayerName == "PagingFrequency")
    {
       if(shapeOpacity > 1) 
           shapeOpacity = 1.0 ;   
    }
    
    
    
    var isLayerAdded = false;
    for(var l = 0; l < reportLayers.length; l++)
    {
       if(reportLayers[l].name == 'Reports - ' + LayerName)
       {
         isLayerAdded = true;
       }
    }
    
    if(!isLayerAdded)
    {
        addNewDrawReportLayerToMap(LayerName);
        reportLayers[reportLayers.length - 1].addOptions({deviceType: DeviceType});
      
    }
    
   
    
    if(ShapeType == 1) //circle
    {
    
        var point1 = new Array();
        
        if(DeviceType == "Monitor"  || DeviceType == "Star")
        {
            for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
            {
                if(svgid == InfraStructureCoordinatesArr[i][4])
                {             
                    point1 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                    break;
                }              
                
            } 
        
        }
        else
        {
            for(var i=0;i < roomDetails.length ; i++)
            {
                if(svgid == roomDetails[i][4])
                {             
                    point1 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];
                    break;
                }
                
            } 
        }        
      
        
        
        var labelOffsetPoint = new OpenLayers.Geometry.Point(point1[0], point1[1]);
        var HighLightLayer = new OpenLayers.Feature.Vector(labelOffsetPoint);
        HighLightLayer.attributes = {
            
        name :  Volume,
        Radius : REPORTLAYER_RADIUS,
        align: "cm",
            
        fillcolor : ShapeColor,
        fillopacity: shapeOpacity,
        strokeColor : ShapeColor,
        xOffset: REPORTLAYER_RADIUS/2,
        yOffset: REPORTLAYER_RADIUS/2,
        display: "block",
        strokeWidth : 1.5,
        strokeOpacity : 0.3
            
        };
        
        
         if(DeviceType == "Star")
         {
            HighLightLayer.attributes['Type'] = "Infrastructure - Stars";     
         }  
          else if(DeviceType == "Monitor")
         {
            
            var monitorType;
            
            var LockedStar = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                monitorType=($(LockedStar).children().filter('MonitorType')[0].textContent || $(LockedStar).children().filter('MonitorType')[0].innerText || $(LockedStar).children().filter('MonitorType')[0].text);
            }

            
            HighLightLayer.attributes['Type'] = "Infrastructure - " + monitorType;  
            
        }
        else 
        {
        
            
            for(i = 0; i < tagDetailsArr.length ; i++)
            {
                if(svgid == tagDetailsArr[i].SvgId)
                {             
                     HighLightLayer.attributes['Type'] = "Tag - " + tagDetailsArr[i].TagTypeName;    
                     break;              
                }
               
            }           
       
        
        }

        reportLayers[reportLayers.length -1].addFeatures(HighLightLayer);              
              
        
        
    }
    else if(ShapeType == 2) //line
    {
        var point1 = new Array();
        var point2 = new Array();
        
        
  
        if(DeviceType == "Monitor"  || DeviceType == "Star")
        {
            for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
            {
                if(svgid == InfraStructureCoordinatesArr[i][4])
                {             
                    point1 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                }
                if(ToSvgid == InfraStructureCoordinatesArr[i][4])
                {
                    point2 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                }
                
            } 
        
        }
        else
        {
            for(var i=0;i < roomDetails.length ; i++)
            {
                if(svgid == roomDetails[i][4])
                {             
                    point1 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];
                }
                if(ToSvgid == roomDetails[i][4])
                {
                    point2 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];
                }
                
            } 
        }
        
        var points = new Array(
                               new OpenLayers.Geometry.Point(point1[0], point1[1]),
                               new OpenLayers.Geometry.Point(point2[0], point2[1])
                               );
        
        
        var style = {
        strokeColor: ShapeColor,
        strokeOpacity: 1.0,
        strokeWidth: 3
        };
        
        var line = new OpenLayers.Geometry.LineString(points);
        var lineFeature = new OpenLayers.Feature.Vector(line, null, style);
        

        
        if(DeviceType == "Star")
        {
            lineFeature.style.display = true;

            
            var starType = "Stars";
            
          /*  var LockedStar = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                starType=$(LockedStar).children().filter('StarType')[0].textContent;
            }*/

            
            lineFeature.attributes['FromType'] = "Infrastructure - " + starType;         
            
            
           /* LockedStar = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(ToSvgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                starType=$(LockedStar).children().filter('StarType')[0].textContent;
            }*/
            
            lineFeature.attributes['ToType'] = "Infrastructure - " + starType;  
            
            
        }
        else if(DeviceType == "Monitor")
        {
            lineFeature.style.display = true;

            
            var monitorType;
            
            var LockedStar = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                monitorType=($(LockedStar).children().filter('MonitorType')[0].textContent || $(LockedStar).children().filter('MonitorType')[0].innerText || $(LockedStar).children().filter('MonitorType')[0].text);
            }

            
            lineFeature.attributes['FromType'] = "Infrastructure - " + monitorType;
            
            LockedStar = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(ToSvgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                monitorType=($(LockedStar).children().filter('MonitorType')[0].textContent || $(LockedStar).children().filter('MonitorType')[0].innerText || $(LockedStar).children().filter('MonitorType')[0].text);
            }
            
            lineFeature.attributes['ToType'] = "Infrastructure - " + monitorType;
            
            
        }
        else 
        {
        
            lineFeature.style.display = true;
            
            for(i = 0; i < tagDetailsArr.length ; i++)
            {
                if(svgid == tagDetailsArr[i].SvgId)
                {             
                     lineFeature.attributes['FromType'] = "Tag - " + tagDetailsArr[i].TagTypeName;                  
                }
                if(ToSvgid == tagDetailsArr[i].SvgId)
                {
                     lineFeature.attributes['ToType'] = "Tag - " + tagDetailsArr[i].TagTypeName;
                }
            }           
       
        
        }
       
        
        
        reportLayers[reportLayers.length -1].addFeatures([lineFeature]);
        
        point1 = null;
        point2 = null;

    }
    else if(ShapeType == 3) //line and circle
    {
        
        if(!isLayerAdded)
        {
            if(DeviceType == "Star")
                reportLayers[reportLayers.length - 1].addOptions({device2Type: 'Tag'});
            else
                reportLayers[reportLayers.length - 1].addOptions({device2Type: ''});

            
           reportLayers[reportLayers.length - 1].styleMap.styles['default'].defaultStyle.fontSize = '0px';                                         
           reportLayers[reportLayers.length - 1].styleMap.styles.select.defaultStyle.fontSize = '0px';
            
        }
        
        var point1 = new Array();
        var point2 = new Array();
        
        var tagtype = '';        
        
        if(DeviceType == "Monitor"  || DeviceType == "Star")
        {
            if(DeviceType == "Star")
            {
                for(var i=0;i < roomDetails.length ; i++)
                {
                    if(svgid == roomDetails[i][4])
                    {
                        point1 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];                       
                        break;
                    }
                    
                }
                
                
                for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
                {
                    
                    if(ToSvgid == InfraStructureCoordinatesArr[i][4])
                    {
                        point2 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                        break;
                    }
                    
                }
                
                for(var i=0;i < tagDetailsArr.length ; i++)
                {
                    if(svgid == tagDetailsArr[i].SvgId)
                    {
                        tagtype = tagDetailsArr[i].TagTypeName;                      
                        break;
                    }
                    
                }
                
            }
            else
            {
                for(var i=0;i < InfraStructureCoordinatesArr.length ; i++)
                {
                    if(svgid == InfraStructureCoordinatesArr[i][4])
                    {
                        point1 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                    }
                    if(ToSvgid == InfraStructureCoordinatesArr[i][4])
                    {
                        point2 = [(InfraStructureCoordinatesArr[i][0]+InfraStructureCoordinatesArr[i][2])/2,(InfraStructureCoordinatesArr[i][1]+InfraStructureCoordinatesArr[i][3])/2];
                    }
                    
                }
            }
            
        
        }
        else
        {
            for(var i=0;i < roomDetails.length ; i++)
            {
                if(svgid == roomDetails[i][4])
                {
                    point1 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];
                }
                
                if(ToSvgid == roomDetails[i][4])
                {
                    point2 = [(roomDetails[i][0]+roomDetails[i][2])/2,(roomDetails[i][1]+roomDetails[i][3])/2];
                }
                
            }
        }
        
        

        var labelOffsetPoint = new OpenLayers.Geometry.Point(point1[0], point1[1]);
        var HighLightLayer;
        var tempFeature = reportLayers[reportLayers.length -1].getFeaturesByAttribute('room_SVGID',svgid)[0];
        
        if(!tempFeature)
        {
            
            HighLightLayer = new OpenLayers.Feature.Vector(labelOffsetPoint);
            
            tempFeature = HighLightLayer;
            
            HighLightLayer.attributes = {
                
            name :  Volume,
            Radius : REPORTLAYER_RADIUS,
            align: "cm",
                
            fillcolor : ShapeColor,
            fillopacity: shapeOpacity,
            strokeColor : ShapeColor,
            xOffset: REPORTLAYER_RADIUS/2,
            yOffset: REPORTLAYER_RADIUS/2,
            display: "block",
            strokeWidth : 1.5,
            strokeOpacity : 0.6
                
            };
            
            
            reportLayers[reportLayers.length -1].addFeatures(HighLightLayer);
            
            
            if(DeviceType == "Star")
            {
                tempFeature.attributes['room_SVGID'] = svgid;
                tempFeature.attributes['description'] = '';
                tempFeature.attributes['descriptionContent'] = '';
                                
                tempFeature.attributes['tagTypes'] = tagtype;
                
                tempFeature.attributes['isMultiTag'] = false;
            }           
            
        }
        else
        {
            if(DeviceType == "Star")
            {                            
                
             tempFeature.attributes['strokeWidth'] = 10;               
             tempFeature.attributes['tagTypes'] = tagtype;                
             tempFeature.attributes['isMultiTag'] = true;
             tempFeature.attributes['fillopacity'] = 0; 
            }
        }
       
        if(DeviceType == "Star")
        {
            
            if(tempFeature)
            {    
                
                
                 tempFeature.attributes['descriptionContent'] = tempFeature.attributes['descriptionContent'] 
                                                               + "<br><span style='z-index: -5;position: absolute;width:130px;height:50px;background:" + ShapeColor + ";opacity:" + shapeOpacity + ";'></span>"
                                                               + "<span><b class = 'clsLALabelMap'>Tag Id  : </b><span  class = 'clsLALabelVal'>" +  Deviceid + "</span><br>"
                                                               + "<b class = 'clsLALabelMap'>RSS Value  : </b><span  class = 'clsLALabelVal'>" +  Volume + "</span></span>"
                                                               + "<br>";               
                                                                                   
                
               
               
                    tempFeature.attributes['description'] = "<div style ='width:160px;height:100px;'><b class = 'clsLALabeltitle'><u>Tag RSSI</u></b>" +
                    tempFeature.attributes['descriptionContent'] + "<div>";
              
               
                
                tempFeature.attributes['tagTypes'] = tempFeature.attributes['tagTypes'] + " || " + tagtype;
                
                
            }
        }
        
       

       
        
        var points = new Array(
                               new OpenLayers.Geometry.Point(point1[0], point1[1]),
                               new OpenLayers.Geometry.Point(point2[0], point2[1])
                               );
        
        
        var style = {
        strokeColor: "#C95C04" /*ShapeColor*/,
        strokeOpacity: 1.0,
        strokeWidth: 3
        };
        
        var line = new OpenLayers.Geometry.LineString(points);
        var lineFeature = new OpenLayers.Feature.Vector(line, null, style);
        

        if(DeviceType == "Star")
        {
            lineFeature.style.display = true;

            
            var starType = "Stars";
            
          /*  var LockedStar = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                starType=$(LockedStar).children().filter('StarType')[0].textContent;
            }*/

            lineFeature.attributes['FromType'] = "Star-Tag-type3";

            
            if(HighLightLayer)
                HighLightLayer.attributes['FromType'] =  "Star-Tag-type3";

            
            
           /* LockedStar = $(g_starRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(ToSvgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                starType=$(LockedStar).children().filter('StarType')[0].textContent;
            }*/
            
            lineFeature.attributes['ToType'] = "Infrastructure - " + starType;
            
            if(HighLightLayer)
              HighLightLayer.attributes['ToType'] = "Infrastructure - " + starType;
         
            if(HighLightLayer)
              HighLightLayer.attributes['isCircle'] = "Yes";
            
            lineFeature.attributes['tagTypes'] = tempFeature.attributes['tagTypes'];

            
            
        }
        else if(DeviceType == "Monitor")
        {
            lineFeature.style.display = true;

            
            var monitorType;
            
            var LockedStar = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(svgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                monitorType=($(LockedStar).children().filter('MonitorType')[0].textContent || $(LockedStar).children().filter('MonitorType')[0].innerText || $(LockedStar).children().filter('MonitorType')[0].text);
            }

            
            lineFeature.attributes['FromType'] = "Infrastructure - " + monitorType;
            
            if(HighLightLayer)
              HighLightLayer.attributes['FromType'] = "Infrastructure - " + monitorType;
            
            
            LockedStar = $(g_monitorRoot).find("SvgId").filter("svgid").filter(function () { return $( this ).text() == String(ToSvgid);}).parent();
            
            if(LockedStar.length > 0)
            {
                monitorType=($(LockedStar).children().filter('MonitorType')[0].textContent || $(LockedStar).children().filter('MonitorType')[0].innerText || $(LockedStar).children().filter('MonitorType')[0].text);
            }
            
            lineFeature.attributes['ToType'] = "Infrastructure - " + monitorType;
            
            if(HighLightLayer)
              HighLightLayer.attributes['ToType'] = "Infrastructure - " + monitorType;
         
              
            HighLightLayer.attributes['isCircle'] = "Yes";
            
            
        }
        else 
        {
        
            lineFeature.style.display = true;
            
            for(i = 0; i < tagDetailsArr.length ; i++)
            {
                if(svgid == tagDetailsArr[i].SvgId)
                {             
                     lineFeature.attributes['FromType'] = "Tag - " + tagDetailsArr[i].TagTypeName;
                    
                    if(HighLightLayer)
                      HighLightLayer.attributes['FromType'] = "Tag - " + tagDetailsArr[i].TagTypeName;
                }
                if(ToSvgid == tagDetailsArr[i].SvgId)
                {
                     lineFeature.attributes['ToType'] = "Tag - " + tagDetailsArr[i].TagTypeName;
                    
                    if(HighLightLayer)
                      HighLightLayer.attributes['ToType'] = "Tag - " + tagDetailsArr[i].TagTypeName;
                }
            }           
       
            if(HighLightLayer)
              HighLightLayer.attributes['isCircle'] = "Yes";
        
        }
        
       
        
        
        reportLayers[reportLayers.length -1].addFeatures([lineFeature]);
        
        point1 = null;
        point2 = null;

    }
    
}

function showOrHide_FetureInLayer(Layer,isShow)
{
    if(Layer.features)
    {
         for(var i = 0; i < Layer.features.length; i++)
         {
             if(Layer.features[i].attributes["SVGID"] )
             {
         
                    if(Layer.features[i].attributes["isCircle"])
                     {
                                if(isShow == true)
                                {
                                     Layer.features[i].style = '';
                                    
                                }
                                else
                                {
                                     Layer.features[i].style = { visibility: 'hidden' };
                                   
                                }
                     } 
                     else
                     {
                                 if(isShow == true)
                                 {
                                    Layer.features[i].style.display = '';
                                 }
                                 else
                                 {
                                    Layer.features[i].style.display = 'none';
                                 }
                     }
                         
                
             }
           
         }         
        Layer.redraw();
    }
        
}

// Change layer visibility based on Report Layer
function hideLayersForReport(Layer)
{
   if(Layer.deviceType.length > 0)
   {
      if(Layer.visibility == true)
      {
         for(var t = 0; t < map.layers.length; t++)
         {
              if(map.layers[t].deviceType)
              {  
                  if(map.layers[t].deviceType.length > 0)
                  {
                     if(map.layers[t].deviceType == Layer.deviceType && map.layers[t].name.substring(0, 7) != 'Reports')
                     {                      
                       
                         if(Layer.features)
                         {
                           if(Layer.features.length > 0)
                           {
                             if(Layer.features[0].attributes["Search_SVGID"])
                             {
                                 showOrHide_FetureInLayer(map.layers[t],false);
                                 
                                 for(var i = 0; i < Layer.features.length; i++)
                                 {      
                                       var featArr =  map.layers[t].getFeaturesByAttribute('SVGID',Layer.features[i].attributes["Search_SVGID"]);
                                       for(var k = 0; k < featArr.length; k++)  
                                       {
                                          var fet =   map.layers[t].getFeaturesByAttribute('SVGID',Layer.features[i].attributes["Search_SVGID"])[k];
                                    
                                          if(fet)
                                          {
                                              if(fet.attributes["SVGID"])
                                              {
                                                    if(fet.attributes["isCircle"])
                                                        fet.style = '';
                                                    else 
                                                        fet.style.display = ''; 
                                              }                               
                                          }
                                       
                                       }                        
                                     
                                    
                                 }
                                
                                 map.layers[t].redraw();
                             }
                           }
                         }   
                               map.layers[t].setVisibility(true);                               
                               
                               
                        
                     }
                     else
                     {
                         if(map.layers[t].name != Layer.name)
                             map.layers[t].setVisibility(false);
                     }
                      
                      if(map.layers[t].deviceType == Layer.device2Type && map.layers[t].name.substring(0, 7) != 'Reports')
                      {
                          map.layers[t].setVisibility(true);
                      }
                  }
                  
                 
              
              }  
             else if(map.layers[t].name != 'Floor Plan' &&  map.layers[t].name.substring(0, 10) != 'OpenLayers')
             {
                    map.layers[t].setVisibility(false);
             }                                     
         }        
               
         
      }
   } 
 }
 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////                      PUBLIC FUNCTIONS                 /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function resizeMap(isZoom)
{
   setTimeout(function(){
                if(isZoom === '1')
    {
        map.zoomTo(1);
     
    }
                 map.updateSize();
    
            }, 600);
   
}

function tagUpdateTimer()
{
    if(tagDetailsArr.length > 0)
    {
        var i = parseInt(getRandomTag(0,tagDetailsArr.length));
        
        
        tagDetailsArr[i] = {roomID: parseInt(getRandomRoom(1,86)),tagID:tagDetailsArr[i].tagID};
        
    }
   
    
     UpdateTags(tagDetailsArr);
    
}


function roomIDChanged()
{
  //  addTagMarkerinroom(1);
    
    var roomSelect = document.getElementById("roomIDComboBox");
    
    var tagSelect = document.getElementById("tagIDComboBox");
    
    tagDetailsArr[tagSelect.selectedIndex]  = {roomID: parseFloat(roomSelect.options[roomSelect.selectedIndex].text),tagID:tagDetailsArr[tagSelect.selectedIndex].tagID};
    
    
}

function tagIDChanged()
{
    var tagSelect = document.getElementById("tagIDComboBox");
    
    var myOpts = document.getElementById("roomIDComboBox").options;
    
    
    for( var i = 0, len = roomDetails.length; i < len; i++ )
    {
        if( roomDetails[i][4] === tagDetailsArr[tagSelect.selectedIndex].roomID )
        {
            myOpts[i].selected = true;
            break;
        }
        
    }
}

function roomsComboBoxChanged()
{
    
    var roomSelect = document.getElementById("roomsComboBox");
    
   
    
    /*if( roomSelect.selectedIndex == 0)
    {
        LoadFloorMap(document.getElementById('map'),"Basement-New.svg","Basement.png");
        UpdateTags(tagDetailsArr);
    }
    else if( roomSelect.selectedIndex == 1)
    {
        LoadFloorMap(document.getElementById('map'),"FirstFloor.svg","FirstFloor.jpg");
        UpdateTags(tagDetailsArr);
    }

    else if( roomSelect.selectedIndex == 2)
    {
        LoadFloorMap(document.getElementById('map'),"ThridFloor.svg","ThridFloor.jpg");
        UpdateTags(tagDetailsArr);
    }

    else if( roomSelect.selectedIndex == 3)
    {
        LoadFloorMap(document.getElementById('map'),"FourthFloor.svg","FourthFloor.jpg");
        UpdateTags(tagDetailsArr);
    }  */ 
   
}

function startTimer()
{
    UpdateTimer = setInterval(function(){tagUpdateTimer()},1000);
    document.getElementById("StartTimer").disabled=true;
    document.getElementById("StopTimer").disabled=false;
}

function StopTimer()
{
    window.clearTimeout(UpdateTimer);
    document.getElementById("StartTimer").disabled=false;
    document.getElementById("StopTimer").disabled=true;

}

function refresh()
{
    UpdateTags(tagDetailsArr);

}


function AddRoom()
{
    if(lastDrawnRoom)
    {
        console.log(lastDrawnRoom);
        var xpos,ypos,width,height,boxID,fillcolor,isRoomCoord,polygonPoints;
        
        if(lastDrawnRoom.attributes['isPolygon'] == "YES")
        {
            var vertices = lastDrawnRoom.geometry.getVertices();
            
            var points = new Array();
            
            for(var i=0;i<vertices.length;i++)
            {
                points[points.length] = vertices[i].x + "," + getboxFromYpos(vertices[i].y,ImageHeight);
                
            }
            
            var bounds = lastDrawnRoom.geometry.getBounds();
            
            xpos   = (parseFloat(bounds.left) + parseFloat(bounds.right)) / 2  ;
            ypos   = (parseFloat(bounds.top) + parseFloat(bounds.bottom)) / 2 ;
            width  = 0;
            height = 0;
            
            roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points];
            
            roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
            
            
            var source =  roomCoordinatesArr[roomCoordinatesArr.length - 1][4];
            
            var polygonList = [];
            var pointList = [];
            for (var j=0; j<source.length; j++)
            {
                var poloygonCoord = source[j].split(',');
                if(poloygonCoord[0] != '' && poloygonCoord[1] != '')
                {
                    var point = new OpenLayers.Geometry.Point(parseFloat(poloygonCoord[0]), getboxFromYpos(parseFloat(poloygonCoord[1]),ImageHeight));
                    pointList.push(point);
                }
                
            }
            var linearRing = new OpenLayers.Geometry.LinearRing(pointList);
            var polygon = new OpenLayers.Geometry.Polygon([linearRing]);
            polygonList.push(polygon);
            
            multuPolygonGeometry = new OpenLayers.Geometry.MultiPolygon(polygonList);
            multiPolygonFeature = new OpenLayers.Feature.Vector(multuPolygonGeometry);
            multiPolygonFeature.style = defaultRoomStyle;
            multiPolygonFeature.attributes = { "roomId" : roomDetails[i][4], "unique_id": i , "roomNO": "room"+ roomDetails[i][4]};
            Rooms.addFeatures(multiPolygonFeature);

        }
        else
        {
            
            var bounds = lastDrawnRoom.geometry.getBounds();
            
            
            xpos = bounds.left;
            ypos = bounds.top;
            width = bounds.right;
            height = bounds.bottom;
            
            var temproomY = ypos;
            ypos = convertToSvgYCoordinateForRoom(parseFloat(ypos),parseFloat(height));
            width = convertToSvgWidth(parseFloat(xpos),parseFloat(width));
            height = convertToSvgHeight(parseFloat(temproomY),parseFloat(height));
            
            roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight)];
            
            roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
            
            
            ext = roomCoordinatesArr[roomCoordinatesArr.length];
            bounds = OpenLayers.Bounds.fromArray(ext);
            
            box = new OpenLayers.Feature.Vector(bounds.toGeometry());
            
           // box.attributes = { "roomId" : roomDetails[i][4], "unique_id": i , "roomNO": "room"+ roomDetails[i][4]};
            
            box.style = defaultRoomStyle;
            
            Rooms.addFeatures(box);
            
            
            
        }
    }
        
   
    
   /* if(isRoomCoord == 1)
    {
        roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight)];
        
        roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
    }
    else if(isRoomCoord == 2)
    {
        
        var points = polygonPoints.split(' ');
        
        var firstCoord = points[0].split(',');
        var thirdCoord = points[2].split(',');
        
        xpos   = (parseFloat(firstCoord[0]) + parseFloat(thirdCoord[0])) / 2  ;
        ypos   = (parseFloat(firstCoord[1]) + parseFloat(thirdCoord[1])) / 2 ;
        
        roomCoordinatesArr[roomCoordinatesArr.length] = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),points];
        
        roomDetails[roomDetails.length]  = [xpos,getboxFromYpos(ypos,ImageHeight),getboxToXpos(xpos,width),getboxToYpos(ypos,height,ImageHeight),boxID,fillcolor];
    }*/
}

function checkMonitorWithInARoom()
{
    var isRoomFound = 0;
    var deviceAddedroomID;
    
    var feature;

    for (var i = 0; i < Rooms.features.length; i++)
    {
        var feature = Rooms.features[i];
        
        var linearRing = new OpenLayers.Geometry.LinearRing(feature.geometry.getVertices());
        var polygon = new OpenLayers.Geometry.Polygon([linearRing]);
        

        if(polygon.containsPoint(selectedFeature.geometry.getCentroid()))
        {
            isRoomFound = 1;
        }
        
    }
  
   
    if(isRoomFound == 0)
    {
        if(initialDevicecoords_x)
        {
            var oldLonLat = new OpenLayers.LonLat(initialDevicecoords_x, initialDevicecoords_y);
            var oldPx = map.getViewPortPxFromLonLat(oldLonLat);
            selectedFeature.move(oldPx);
        }
    }
}



function AddDevice()
{

            if(!((document.getElementById("xpos").value > 0 && document.getElementById("xpos").value <= imageWidth) && (document.getElementById("ypos").value > 0 && document.getElementById("ypos").value <= ImageHeight)))
              {
                 alert('Please enter valid coordinates');
                 return;
              }
              
    checkMonitorWithInARoom();

             
            deviceInfoArr[deviceInfoArr.length] = {roomID: deviceAddedroomID ,deviceID: document.getElementById("deviceId").value ,deviceName: document.getElementById("deviceName").value ,description: document.getElementById("description").value ,xpos: document.getElementById("xpos").value ,ypos: document.getElementById("ypos").value}; 
           
            document.getElementById("positionOnImage").innerHTML  = 'Position On Image : xpos = ' + (document.getElementById("xpos").value * imageOSWidthRatio) + " : ypos = " + (document.getElementById("ypos").value * ImageOSHeightRatio)  ;
           
            
            tagMarKerLayer.destroyFeatures();
            
}

function showAddedDevice()
{
  
    
    tagMarKerLayer.destroyFeatures();
    
    if(deviceInfoArr.length == 0)
    { 
        alert("No devices are added yet");
    }
    
    for(var i=0; i < deviceInfoArr.length; i++)
    {
         var feature = new OpenLayers.Feature.Vector
                                (
                                 new OpenLayers.Geometry.Point(deviceInfoArr[i].xpos, convertToSvgYCoordinate(parseFloat(deviceInfoArr[i].ypos))).transform(epsg4326, projectTo),
                                 {description: "<div style ='height:115px;' ><p><b><u>Tag Details</u><br></b>device ID:" + deviceInfoArr[i].deviceID + "<br>Selected Room ID :" + deviceInfoArr[i].roomID +  "<p></div>"} ,
                                 {externalGraphic: getDeviceImage(deviceInfoArr[i].deviceName), graphicHeight: 30, graphicWidth: 30, graphicXOffset:-15, graphicYOffset:-15  }
                                 );
                               
                         
                    tagMarKerLayer.addFeatures(feature);
    }
    
     document.getElementById('deviceInfo').style.display = 'none';
  
    document.getElementById("showAddedDeviceBtn").disabled=true;
    document.getElementById("addDeviceBtn").disabled=false;
    
    controls['selector'].activate();
    drag.deactivate();
    mouseclick.deactivate();

}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////                 ADD TAG LIVE DATA ROUTE               /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function addLiveDataRoute(fromSVGID,ToSVGID,tagId,popupmsg)
{
    
     if (typeof(fromSVGID) == "undefined" || typeof(ToSVGID) == "undefined")
          return;
         
     var  vectorLayer = map.getLayersByName('Tag - Live Updates')[0];     
     
     if(!vectorLayer)
     {
          vectorLayer = new OpenLayers.Layer.Vector('Tag - Live Updates');  
          LAyersArray[LAyersArray.length] = vectorLayer;
          addAllLayersToMap(LAyersArray); 
     }
    
     var strSvGID = "";
     
     if(parseFloat(fromSVGID) > parseFloat(ToSVGID))
         strSvGID = fromSVGID + "," + ToSVGID;
     else 
         strSvGID = ToSVGID + "," + fromSVGID;
         
     var feature  = vectorLayer.getFeaturesByAttribute('fromToSVGID',strSvGID)[0];
     
     if(feature)  
     {
        feature.attributes['tagIds']   =  feature.attributes['tagIds'] + "," + String(tagId);   
        feature.attributes['descriptionContent'] =  feature.attributes['descriptionContent'] + "<br/><br/><span class = 'clsLALabelVal'>" +  popupmsg + "</span>" ;
        
        feature.attributes['description'] = "<div style ='height:70px;width:250px;'><p><b class = 'clsLALabelMap'><u>Live Updates</u><br/><br/></b>" + feature.attributes['descriptionContent'] + "</div>";
        
       
       showFilteredTagRoute();
       return;  
     }

     var tempCoordinatesArray = getTagCoordinatesInfoArray(fromSVGID);     
     var awayDistance = tempCoordinatesArray[1];     
     
     var point1 = new OpenLayers.Geometry.Point((tempCoordinatesArray[0][0]+tempCoordinatesArray[0][2]+awayDistance)/2,(tempCoordinatesArray[0][1]+tempCoordinatesArray[0][3]+awayDistance)/2);
     
        tempCoordinatesArray = getTagCoordinatesInfoArray(ToSVGID);     
        awayDistance = tempCoordinatesArray[1];   
     
     var point2 = new OpenLayers.Geometry.Point((tempCoordinatesArray[0][0]+tempCoordinatesArray[0][2]+awayDistance)/2,(tempCoordinatesArray[0][1]+tempCoordinatesArray[0][3]+awayDistance)/2);
     
         
     var geom = new OpenLayers.Geometry.LineString([
        point1,
        point2
    ]);
    
      var linestyle = {
        'strokeColor':"#777777",
        'strokeWidth':3,
        'strokeDashstyle':"dash",
        'display' : "block" 
    };
    
    var feature  = new OpenLayers.Feature.Vector(geom,null,linestyle);
        feature.attributes['descriptionContent'] = "<span class = 'clsLALabelVal'>" + popupmsg + "</span>" ;
        
        feature.attributes['description'] = "<div style ='height:70px;width:250px;' ><p><b class = 'clsLALabelMap'><u>Live Updates</u><br/><br/></b>" + feature.attributes['descriptionContent'] + "</div>";
        
        
        feature.attributes['fromToSVGID']   = strSvGID;   
        feature.attributes['tagIds']   =  String(tagId);       
      
    vectorLayer.addFeatures(feature);
    
    showFilteredTagRoute();

  


}

function showFilteredTagRoute()
{
   //var filterStr = document.getElementById('txtliveFilters').value;

   var selectedTagList = liveDataFilterArr;
   
   
     var  vectorLayer = map.getLayersByName('Tag - Live Updates')[0];     
     
     if(vectorLayer)
     {     
          var features =  vectorLayer.features;    
                    
          for(var t = 0 ; t < features.length; t++)
          {
               features[t].style.display = 'none';
               
                features[t].attributes['description'] = "<div style ='height:70px;width:250px;' ><p><b class = 'clsLALabelMap'><u>Live Updates</u></b>";
                 
                
                 var tagIds;
                
                if(features[t].attributes['tagIds'])
                   tagIds =  features[t].attributes['tagIds'].split(',');
                          
                var popupDesc;
                
                if(features[t].attributes['descriptionContent'])
                  popupDesc =  features[t].attributes['descriptionContent'].split('<br/><br/>');

                
                
                
                          
               for(var k = 0 ; k < tagIds.length; k++)  // tagIds.length = popupDesc.length
               {
                      if(features[t].attributes['tagIds'])
                      {
                            
                          if(selectedTagList.indexOf(tagIds[k]) > -1 || selectedTagList.length == 0 || selectedTagList == "")
                          {
                                  features[t].style.display = 'block';                             
                              
                                  features[t].attributes['description'] = features[t].attributes['description'] + '<br/><br/>' +  popupDesc[k];
                    
                          }
                      }
               
               }
               
                 features[t].attributes['description'] = features[t].attributes['description']  + "</div>";
              
          }
          
        if(!isMapMoves)
           vectorLayer.redraw();
     }
     
}

function showOnlyFilteredLiveTags()
{
     //var filterStr = document.getElementById('tagIDFilter').value;

     var selectedTagList = liveDataFilterArr;
      //show only filtered tags
     
      for(var i = 0 ; i < taglayersArray.length; i++)
      {
         
           var features =  taglayersArray[i].features;
           
            for(var j = 0 ; j < features.length; j++)
            {
                  var tempArr = features[j].attributes['tagsID'];
                  if(tempArr)
                  {
                      var unique = $.grep(tempArr, function(element) {                                        
                                          return $.inArray(element, selectedTagList) !== -1;
                                   });
                                                   
                      if(unique.length > 0 || selectedTagList.length == 0 || selectedTagList == "")
                      {    
                           if(features[j] .attributes['isCircle']) 
                              features[j].style = '';
                           else    
                              features[j].style.display = 'block';
                              
                              var lineFeature = starTagLinkLayer.getFeaturesByAttribute('SVGID',String(features[j] .attributes['SVGID']));
                              for(var k = 0 ; k < lineFeature.length; k++)
                              {
                                  if(features[j] .attributes['SVGID']) 
                                      lineFeature[k].style.display = 'block';
                              }
                              
                           
                      }
                      else
                      {
                           if(features[j] .attributes['isCircle']) 
                                features[j].style = { visibility: 'hidden' };
                           else
                                features[j].style.display = 'none';
                                
                              var lineFeature = starTagLinkLayer.getFeaturesByAttribute('SVGID',String(features[j] .attributes['SVGID']));
                              for(var k = 0 ; k < lineFeature.length; k++)
                              {
                                  if(features[j] .attributes['SVGID']) 
                                      lineFeature[k].style.display = 'none';
                              }
                             
                      }
                     
                  }
              
            }   
            
            if(!isMapMoves)
            {
                taglayersArray[i].redraw();   
                starTagLinkLayer.redraw(); 
            }
                              
      }
      
     
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////                 UPDATE TAG LIVE DATA                  /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function test()
{
    roomsCount ++;
    var liveDataResponse;
    
     var tagid = "101";
     
     if(roomsCount > 10)
       {
         tagid = "102";
       }
    
    if(roomsCount%2 == 0)
    { 
     
           //  liveDataResponse = '{"CenTrakEvent":{"evt_type":"JSON_900MHzStar","siteid":"23323","tagid":' + String(tagid) + ',"starid":"150","star_macId":"00_24_DD_00_31_06","time":""}}';
      
       
     
       
        liveDataResponse = '{"CenTrakEvent":{"evt_type":"JSON_900MHzStar","siteid":"23323","tagid":' + String(tagid) + ',"starid":"151","star_macId":"00_24_DD_00_31_88","time":""}}'; 
    }
    else
    {
      liveDataResponse = '{"CenTrakEvent":{"evt_type":"JSON_900MHzStar","siteid":"23323","tagid":' + String(tagid) + ',"starid":"150","star_macId":"00_24_DD_00_31_06","time":""}}'; 
       //liveDataResponse = '{"CenTrakEvent":{"evt_type":"","siteid":"23323","tagid":' + String("101") + ',"roomid": ' + roomDetails[roomsCount][4] + ',"monitorid":'+ roomDetails[roomsCount][4] +',"time":""}}';
    }
   
   g_MapSiteId =  "23323";
   checkTagLiveDataIsForCurrentFloor(liveDataResponse);
   
  
  
}

function checkTagLiveDataIsForCurrentFloor(jsonObj,locationChangeMsg)
{       
    if(!map || !isInfraStructureLoaded)
        return;
                         
    var isNewTag = false;  
    
    var result = $.grep(tagDetailsArr, function(v,i) {
                                                    return v["tagID"] == jsonObj.CenTrakEvent.tagid;
                                                });
                                                
    // new tag entered this floor add new tag info on tagDetailArr                                            
    if(result.length == 0)
    {
        isNewTag = true; 
        var liveTagType = GetTagTypeByTypeAndId(jsonObj.CenTrakEvent.tag_type,jsonObj.CenTrakEvent.tagid);
        if(!map.getLayersByName('Tag - ' + liveTagType )[0]) // create a new tag layer for newly arrived tags.
        {            
             addNewMarkerLayerToMap('Tag - ' + liveTagType);                                        
             LAyersArray[LAyersArray.length - 1].addOptions({deviceType: 'Tag'});
             addAllLayersToMap(LAyersArray);
             taglayersArray[taglayersArray.length] = LAyersArray[LAyersArray.length-1];
        }
       
                                        
      var svgid ;
      
      if(jsonObj.CenTrakEvent.star_macId) // get star svgid
      {
         var parentTag = "";   
         parentTag = $(g_starRoot).find("MacId").filter("macid").filter(function () { return $( this ).text() == String(jsonObj.CenTrakEvent.star_macId);}).parent(); 
         
          if(parentTag.length > 0) 
               svgid =  String($(parentTag).children().filter('SvgId')[0].textContent);
      }
      else // get monitor svgid
      {
        svgid = String(jsonObj.CenTrakEvent.monitorid);
      }
      
      tagDetailsArr[tagDetailsArr.length] = {"MonitorId": jsonObj.CenTrakEvent.monitorid, "roomId": svgid, "SvgId": svgid, "RoomName": "", "tagID": String(jsonObj.CenTrakEvent.tagid), "TagType": "", "ModelItem": "", "BatteryCapacity": "", "LastIRTime": "", "RoomSeen": "","TagTypeName" : String(liveTagType),"LockedStarId" : "", "StarLocation" : String(setundefined(jsonObj.CenTrakEvent.floor_name)) };      

    }
        
    if(jsonObj.CenTrakEvent.siteid == g_MapSiteId) //show only selected site tag live data
    {
        
          if(jsonObj.CenTrakEvent.evt_type == 4) // update tag live data by star details - JSON_900MHzStar
          {      
               var temapArr = getTagCoordinatesInfoArray(jsonObj.CenTrakEvent.starid)    
               
                // check STAR SVGID is Present in current floor
               if(temapArr.length > 0)
               {
                    var parentTag = "";   
                    parentTag = $(g_starRoot).find("MacId").filter("macid").filter(function () { return $( this ).text() == String(jsonObj.CenTrakEvent.star_macId);}).parent(); 
                       
                    if(parentTag.length > 0) 
                       UpdateLiveTagPosition(jsonObj.CenTrakEvent.tagid,$(parentTag).children().filter('SvgId')[0].textContent,false,jsonObj.CenTrakEvent.star_macId,isNewTag,locationChangeMsg);
               }
               return; 
          }
                  
         // update tag live data by Moitor details  
         for (var a = 0; a < roomCoordinatesArr.length; a++)
         {          
                     // check MONITOR SVGID is Present in current floor
                     if(roomDetails[a][4] == jsonObj.CenTrakEvent.monitorid)
                     {                   
                         UpdateLiveTagPosition(jsonObj.CenTrakEvent.tagid,jsonObj.CenTrakEvent.monitorid,true,jsonObj.CenTrakEvent.star_macid,isNewTag,locationChangeMsg);
                         break;                   
                     }                 
         }
       
    }    
   
}


function getTagCoordinatesInfoArray(svgId)
{

    var result = $.grep(roomDetails, function(v,i) {
                                                    return v[4] == svgId;
                                                });
                                                
      if(result.length > 0)
      {
         // awy distance from svgid
         result[1] = 0;
         return result;
      } 
      
      
      result = $.grep(InfraStructureCoordinatesArr, function(v,i) {
                                                    return v[4] == svgId;
                                                });
                                                
      if(result.length > 0)
      {
          // awy distance from svgid
         result[1] = DISTANCE_AWAY_FROM_STAR;
         return result;
      } 
      
      return result;
}

// Update Live Tag Info
function UpdateLiveTagPosition(tagId,svgId,isMonitorSvgId,LockedStarId,isNewTag,popupmsg)
{
     var tempCoordinatesArray;
     var awayDistance = 0;

     LockedStarId = setundefined(LockedStarId);
     LockedStarId = LockedStarId.replace(/:/g, "_");

     var newSVGDetailArr = checkMultiTagRoom(svgId);
     
     
     
     if($.inArray(String(tagId),newSVGDetailArr[2]) > -1 && !isNewTag  ) // tag position not updated
     {
       return;
     } 
     else //get tag old room id
     {        
        
         var oldSvgId;
         
         for(var m = 0 ; m < tagDetailsArr.length; m++)
         {
            if(tagDetailsArr[m].tagID == tagId)
            {
               oldSvgId = tagDetailsArr[m].SvgId;
               
               if(!isNewTag)
                  addLiveDataRoute(oldSvgId,svgId,tagId,popupmsg);
               
                if(starTagLinkLayer.getFeaturesByAttribute('SVGID',String(oldSvgId)))
                    starTagLinkLayer.removeFeatures(starTagLinkLayer.getFeaturesByAttribute('SVGID',String(oldSvgId)));
                  
                var OldSvgDetailArr = checkMultiTagRoom(oldSvgId);
                
                var layer;
                
                if(OldSvgDetailArr[0] > 1) // tag present in multi tag layer
                {
                         layer = multiTagLayer;
                }
                else // tag present in  tag type layer
                {
                         layer = map.getLayersByName('Tag - ' + tagDetailsArr[m].TagTypeName)[0];  
                }
                
                tagDetailsArr[m].SvgId = svgId;
                
                if(LockedStarId.length > 0)              
                   tagDetailsArr[m].LockedStarId = LockedStarId; 
                 
                
                if(layer)
                {
                         if(newSVGDetailArr[0] > 0) // need to add tag to multi tag layer
                         {
                            newSVGDetailArr = checkMultiTagRoom(svgId);                             
                           
                            if(!isNewTag && (layer.name != "Multi-Tag-Layer"))
                               layer.removeFeatures(layer.getFeaturesByAttribute('SVGID',String(oldSvgId))); 
                                             
                            var arrFeature =  multiTagLayer.getFeaturesByAttribute('SVGID',String(svgId));
                            
                           
                               for(var j = 0 ; j < tagDetailsArr.length; j++)
                               {
                                     
                               
                                    if(newSVGDetailArr[2].indexOf(tagDetailsArr[j].tagID) > -1)
                                    {
                                       var diffTagLayer =  map.getLayersByName('Tag - ' + tagDetailsArr[j].TagTypeName)[0];
                                        diffTagLayer.removeFeatures(diffTagLayer.getFeaturesByAttribute('SVGID',String(svgId)));    
                                        
                                        if(arrFeature.length == 0) // No Feature Present Need to create new Multi tag feature
                                        {
                                            //create new feature and add it to multitag layer
                                             var i = 0;
                                             tempCoordinatesArray = getTagCoordinatesInfoArray(svgId);
                                             awayDistance = tempCoordinatesArray[1];
                                             
                                                                                        
                                            var imgDetail =  getTagImageByType(-1);
                                            var feature = new OpenLayers.Feature.Vector
                                            (
                                             new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2]+awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3]+awayDistance)/2).transform(epsg4326, projectTo),
                                             {description: newSVGDetailArr[1]} ,
                                             {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                                             );
                                            

                                            
                                            feature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                            feature .attributes['tagsID'] = newSVGDetailArr[2];
                                            feature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);

                                            multiTagLayer.addFeatures(feature);
                                            
                                            var MultiTagxOffset = 5;
                                            var MultiTagWidth = 35;
                                            
                                            if(newSVGDetailArr[0] > 999)
                                            {
                                                MultiTagxOffset = 5;
                                                MultiTagWidth = 45;
                                            }
                                            
                                            
                                            feature = new OpenLayers.Feature.Vector
                                            (
                                             new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2]+awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3]+awayDistance)/2).transform(epsg4326, projectTo),
                                             {description: newSVGDetailArr[1]} ,
                                             {externalGraphic: 'Images/MultiTag.png', graphicHeight: 25, graphicWidth: MultiTagWidth, graphicXOffset:MultiTagxOffset-5, graphicYOffset:-30  }
                                             );
                                            
                                            
                                            feature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                            feature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);
                                            feature .attributes['tagsID'] = newSVGDetailArr[2];
                                            multiTagLayer.addFeatures(feature);
                                            
                                            
                                            // Create a point feature to show the label offset options
                                            var labelOffsetPoint = new OpenLayers.Geometry.Point((tempCoordinatesArray[i][0]+tempCoordinatesArray[i][2]+awayDistance)/2,(tempCoordinatesArray[i][1]+tempCoordinatesArray[i][3]+awayDistance)/2).transform(epsg4326, projectTo);
                                            var labelOffsetFeature = new OpenLayers.Feature.Vector(labelOffsetPoint);
                                            labelOffsetFeature.attributes = {
                                            name: newSVGDetailArr[0],
                                            favColor: 'white',
                                            align: "cm",
                                                // positive value moves the label to the right
                                            xOffset: 16,
                                                // negative value moves the label down
                                            yOffset: 19
                                            };
                                            
                                            labelOffsetFeature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                            labelOffsetFeature .attributes['tagsID'] = newSVGDetailArr[2];
                                            labelOffsetFeature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);
                                            labelOffsetFeature .attributes['isCircle'] = true;
                                            multiTagLayer.addFeatures(labelOffsetFeature);
                                            
                                            multiTagsLoadedRoomsArray[multiTagsLoadedRoomsArray.length] = tagDetailsArr[j].SvgId;
                                            
                                            addStarTagLinkLine_For_UpdatedTags(tagDetailsArr[j],isMonitorSvgId);
                                        
                                    }
                               }
                            }
                            
                             for(var i = 0 ; i < arrFeature.length; i++) //  Feature Present Update Multi tag feature
                             {  
                                    arrFeature[i] .attributes['tagsID'] = newSVGDetailArr[2];
                                    arrFeature[i] .attributes['SVGID'] = String(svgId);
                                    arrFeature[i] .attributes['roomID'] = String(svgId);
                                    arrFeature[i] .attributes['description'] = newSVGDetailArr[1];
                                    arrFeature[i] .attributes['name'] = newSVGDetailArr[0];  
                                        
                                     
                                     multiTagLayer.drawFeature( arrFeature[i]);                               
                             }
                             
                             
                            addStarTagLinkLine_For_UpdatedTags(tagDetailsArr[m],isMonitorSvgId);
                            
                         }
                         
                         if(1) // need to add tag to  tag type layer 
                         {
                         
                            OldSvgDetailArr = checkMultiTagRoom(oldSvgId);
                            newSVGDetailArr = checkMultiTagRoom(svgId);
                            if(OldSvgDetailArr[0] > 0)
                            {
                                                               
                                 if(OldSvgDetailArr[0] > 1) // Update Old SVGID Multi TAg Layer.
                                 {
                                     //layer.removeFeatures(layer.getFeaturesByAttribute('SVGID',String(oldSvgId)));                            
                                     var arrFeature =  multiTagLayer.getFeaturesByAttribute('SVGID',String(oldSvgId));
                                     for(var i = 0 ; i < arrFeature.length; i++)
                                     {  
                                            arrFeature[i] .attributes['tagsID'] = OldSvgDetailArr[2];
                                            arrFeature[i] .attributes['SVGID'] = String(oldSvgId);
                                            arrFeature[i] .attributes['description'] = OldSvgDetailArr[1];
                                            arrFeature[i] .attributes['name'] = OldSvgDetailArr[0];  
                                                 arrFeature[i].attributes['roomID'] = String(oldSvgId);
                                           
                                        
                                            multiTagLayer.drawFeature( arrFeature[i]);                     
                                     }
                                     
                                       for(var j = 0 ; j < tagDetailsArr.length; j++)
                                       {
                                            if(tagDetailsArr[j].tagID == OldSvgDetailArr[2][0])
                                               addStarTagLinkLine_For_UpdatedTags(tagDetailsArr[j],true);
                                       }
                                     
                                     
                                 
                                 }
                                 else // Remove Old SVGID Multi TAg Layer. and create a new feature for two different tag type layer.
                                 {
                                      //Create new feature for Previous SVGID Rooms.
                                     multiTagLayer.removeFeatures(multiTagLayer.getFeaturesByAttribute('SVGID',String(oldSvgId)));       
                                     
                                                         
                                     
                                        var newPoint;
                                        
                                        tempCoordinatesArray = getTagCoordinatesInfoArray(oldSvgId);
                                        awayDistance = tempCoordinatesArray[1];
                                        newPoint = new OpenLayers.Geometry.Point((tempCoordinatesArray[0][0]+tempCoordinatesArray[0][2]+awayDistance)/2,(tempCoordinatesArray[0][1]+tempCoordinatesArray[0][3]+awayDistance)/2).transform(epsg4326, projectTo); 
                               
                                      
                                        
                                        for(var j = 0 ; j < tagDetailsArr.length; j++)
                                       {
                                            if((tagDetailsArr[j].tagID == OldSvgDetailArr[2][0]) && !isNewTag)
                                            {
                                                   var diffTagLayer =  map.getLayersByName('Tag - ' + tagDetailsArr[j].TagTypeName)[0];
                                                    var imgDetail =  getTagImageByType(tagDetailsArr[j].TagType);
                                                    var feature = new OpenLayers.Feature.Vector
                                                    (
                                                      newPoint,
                                                     {description: OldSvgDetailArr[1] } ,
                                                     {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                                                     );
                                                    
                                                    feature .attributes['roomID'] = tagDetailsArr[j].SvgId;
                                                    feature .attributes['tagsID'] = OldSvgDetailArr[2];
                                                    feature .attributes['SVGID'] = String(tagDetailsArr[j].SvgId);
                                                    diffTagLayer.addFeatures(feature);
                                                    
                                                     addStarTagLinkLine_For_UpdatedTags(tagDetailsArr[j],isMonitorSvgId);
                                            }
                                       }
                                
                                      
                                        
                                 }
                                  if(newSVGDetailArr[0] == 1)
                                  {
                                        //Create new feature for current SVGID Rooms.
                                        var newPoint;
                                        
                                        tempCoordinatesArray = getTagCoordinatesInfoArray(svgId);
                                        awayDistance = tempCoordinatesArray[1];
                                        newPoint = new OpenLayers.Geometry.Point((tempCoordinatesArray[0][0]+tempCoordinatesArray[0][2]+awayDistance)/2,(tempCoordinatesArray[0][1]+tempCoordinatesArray[0][3]+awayDistance)/2).transform(epsg4326, projectTo); 
                                                                             
                                     
                                
                                        var diffTagLayer =  map.getLayersByName('Tag - ' + tagDetailsArr[m].TagTypeName)[0];
                                        var imgDetail =  getTagImageByType(tagDetailsArr[m].TagType);
                                        var feature = new OpenLayers.Feature.Vector
                                        (
                                          newPoint,
                                         {description: newSVGDetailArr[1] } ,
                                         {externalGraphic:imgDetail[0].imageName , graphicHeight: imgDetail[0].imageheight, graphicWidth: imgDetail[0].imageWidht, graphicXOffset:-(imgDetail[0].imageWidht/2), graphicYOffset:-(imgDetail[0].imageheight/2)  }
                                         );
                                        
                                        feature .attributes['roomID'] = tagDetailsArr[m].SvgId;
                                        feature .attributes['tagsID'] = newSVGDetailArr[2];
                                        feature .attributes['SVGID'] = String(tagDetailsArr[m].SvgId);
                                        diffTagLayer.addFeatures(feature);
                                        addStarTagLinkLine_For_UpdatedTags(tagDetailsArr[m],isMonitorSvgId);
                                  }
                                 
                            }
                            else // move only position of the feature in same layer.
                            {
                                 var feature =  layer.getFeaturesByAttribute('SVGID',String(oldSvgId))[0];  
                                 
                                   if(feature)
                                   {
                                       tempCoordinatesArray = getTagCoordinatesInfoArray(svgId);
                                       awayDistance = tempCoordinatesArray[1];
                                            
                                       var newPoint = new OpenLayers.LonLat((tempCoordinatesArray[0][0]+tempCoordinatesArray[0][2]+awayDistance)/2,(tempCoordinatesArray[0][1]+tempCoordinatesArray[0][3]+awayDistance)/2).transform(epsg4326, projectTo); 
                                
                                       feature.move(newPoint);
                                       feature.attributes['SVGID'] = String(svgId);
                                       feature.attributes['roomID'] = String(svgId);
                                       feature.attributes['description'] = newSVGDetailArr[1];
                                       addStarTagLinkLine_For_UpdatedTags(tagDetailsArr[m],isMonitorSvgId);
                              
                                   }
                                  
                              
                            }
                         }
                     }
                     
                 }
          
             }
     }
      
       showOnlyFilteredLiveTags();
        
}

function addStarTagLinkLine_For_UpdatedTags(tagDetailsArr,isMonitorSvgId)
{

    //Add Star->Tag Link Line
    
     var tempCoordinatesArray;
     
    
     var i = 0;     
    tempCoordinatesArray = getTagCoordinatesInfoArray(tagDetailsArr.SvgId);       
    
    if(tagDetailsArr.LockedStarId)
    {
         var TagStarId = tagDetailsArr.LockedStarId;
                   
         var LockedStar = $(g_starRoot).find("MacId").filter("macid").filter(function () { return $( this ).text() == String(TagStarId);}).parent(); 
                   
           if(LockedStar.length > 0)
           {
                var starSVGID=$(LockedStar).children().filter('SvgId')[0].textContent;
                
               if(starSVGID > -1)
               {
                   if(tagInvisibleLayerArray.indexOf(tagDetailsArr.TagTypeName) < 0) 
                   {       
                              var tempArr = tempCoordinatesArray[i];
                              tempArr[6] = isMonitorSvgId;
                              
                 
                                                       
                              addLinkLine(tempArr,starSVGID,"0",[tagDetailsArr.TagTypeName,$(LockedStar).children().filter('StarType')[0].textContent]);
                                    
                              existingTagStarLinkLayer[existingTagStarLinkLayer.length] = tempCoordinatesArray[i][4] + "_" + starSVGID;   
                   }
                   
               } 
           }
    }
                                  
    
}

function getRoomNameByMonitorId(moitorId)
{
   var parentTag = "";
   
   parentTag = $(g_monitorRoot).find("DeviceId").filter("deviceid").filter(function () { return $( this ).text() == String(moitorId);}).parent(); 
                       
   if(parentTag.length > 0)
       return ($(parentTag).children().filter('Location')[0].textContent || $(parentTag).children().filter('Location')[0].innerText || $(parentTag).children().filter('Location')[0].text);
      
   return "Unknown";                      
  
}
function activateAllControls()
{
    controls['selector'].activate();
}

function activateDrag()
{
    //controls['selector'].deactivate();
    drag.activate();
}

function activateRectControl()
{
    polygonControl.deactivate();
    rectControl.activate();
    
   // controls['selector'].deactivate();
}

function activatePolygonControl()
{
    polygonControl.activate();
    rectControl.deactivate();
    
   // controls['selector'].deactivate();
}

function addRoomRectControl()
{
    //new control for drawing rectangle//////////////////////////////////////////////////////////////////////
    rectControl = new OpenLayers.Control();
    OpenLayers.Util.extend(rectControl, {
                           draw: function () {
                           // this Handler.Box will intercept the shift-mousedown
                           // before Control.MouseDefault gets to see it
                           this.box = new OpenLayers.Handler.Box( rectControl,
                                                                 {"done": this.notice},
                                                                 {keyMask: OpenLayers.Handler.MOD_SHIFT});
                           this.box.activate();
                           },
                           
                           notice: function (bounds) {
                           var ll = map.getLonLatFromPixel(new OpenLayers.Pixel(bounds.left, bounds.bottom));
                           var ur = map.getLonLatFromPixel(new OpenLayers.Pixel(bounds.right, bounds.top));
                           
                           var arr = new Array();
                           arr[0]=[ll.lon.toFixed(4),ll.lat.toFixed(4),ur.lon.toFixed(4),ur.lat.toFixed(4)];
                           ext = arr[0];
                           bounds = OpenLayers.Bounds.fromArray(ext);
                           
                           //Add the Room
                           box = new OpenLayers.Feature.Vector(bounds.toGeometry());
                           box.attributes = {"device" :"room", "isPolygon": "NO"};
                           box.style = defaultRoomStyle;
                           addMonitorLayer.addFeatures(box);
                           
                           activateDrag();
                           
                           lastDrawnRoom = box;
                         /*  alert(ll.lon.toFixed(4) + ", " +
                                 ll.lat.toFixed(4) + ", " +
                                 ur.lon.toFixed(4) + ", " +
                                 ur.lat.toFixed(4));*/
                           }
                           
                           
                           });
    
    map.addControl(rectControl);
    rectControl.deactivate();
    //////////////////////////////////////////////////////////////////////////
}


function showHideSnapGridLayer (isChecked)
{
    if(isChecked == 1)
    {
        snapGridLayer.setVisibility(true);
        //snapGridLayer.setRotation(Number(0));
        snapGridLayer.setSpacing(Number(15));
        snapGridLayer.setMaxFeatures(Number(2700));
        
        if(snapControl)
           snapControl.activate();

    }
    else
    {
        snapGridLayer.setVisibility(false);
        
        if(snapControl)
          snapControl.deactivate();

    }
}

function addRoomPolygonControl()
{
    polygonControl  = new OpenLayers.Control.DrawFeature(addMonitorLayer, OpenLayers.Handler.Polygon, {
                                                         displayClass: 'olControlDrawFeaturePolygon'
                                                         })
    
    snapControl = new OpenLayers.Control.Snapping({
                                               layer: addMonitorLayer,
                                               targets: [{
                                                         layer: snapGridLayer,
                                                         tolerance: 15
                                                         }]
                                               });
    //snapControl.activate();
    
    
    polygonControl.events.register('featureadded',polygonControl, onAdded);
   
    
    function onAdded(ev)
    {
        if(lastDrawnRoom) //clear previously drawn unsaved room
        {
            if(selectedFeature == lastDrawnRoom)
                selectedFeature = null;
            
            addMonitorLayer.removeFeatures([lastDrawnRoom]);
        }
        
        lastDrawnRoom = ev.feature;

        var polygon=ev.feature.geometry;
        var bounds=polygon.getBounds();
        
        ev.feature.attributes = {
            align: "cm",
            display: "block",
            name: "",
            strokeOpacity: 1,
            fillOpacity: 0.4,
            strokeColor:"#3366FF",
            fillColor: "#ADFF2F"
        };


           ev.feature.layer.redraw();
        
            unSavedfeatues[unSavedfeatues.length] = ev.feature;

        
            var vertices = lastDrawnRoom.geometry.getVertices();
            var points ;
            RoompolygonPoints = '';
            for(var i=0;i<vertices.length;i++)
            {
                if(RoompolygonPoints.length == 0)
                {
                    RoompolygonPoints = vertices[i].x + "," + getboxFromYpos(vertices[i].y,ImageHeight);
                }
                else
                {
                    RoompolygonPoints = RoompolygonPoints + " " + vertices[i].x + "," + getboxFromYpos(vertices[i].y,ImageHeight);
                }
                
            }
        
        activateDrag();
        polygonControl.deactivate();
        if(g_svgDType == 4)
           ev.feature.attributes['device'] = "wifiZones";
        else
           ev.feature.attributes['device'] = "room";
        getFeatureLocationInFeet(ev.feature);
    }
    
    map.addControl(polygonControl);
    polygonControl.deactivate();
    
    
    OpenLayers.Event.observe(document, "keydown", function(evt) {
                             var handled = false;
                             switch (evt.keyCode) {
                             case 90: // z
                             if (evt.metaKey || evt.ctrlKey) {
                             polygonControl.undo();
                             handled = true;
                             }
                             break;
                             case 89: // y
                             if (evt.metaKey || evt.ctrlKey) {
                             polygonControl.redo();
                             handled = true;
                             }
                             break;
                             case 27: // esc
                             polygonControl.cancel();
                             handled = true;
                             break;
                             }
                             if (handled) {
                             OpenLayers.Event.stop(evt);
                             }
                             });
    
}

function addDragForDrawnFeature()
{
    drag = new OpenLayers.Control.DragFeature(addMonitorLayer, {
                                              autoActivate: true,
                                              onComplete: getPixelOnDragging,
                                              onDrag:onDragStart
                                              ,
                                              featureCallbacks: {
                                              // called when a feature is clicked
                                              click: function(feature) {
                                              if (feature === this.interesting_feature) {
                                              return this.clickFeature(feature); // do the default thing
                                              }
                                              // otherwise do nothing
                                              },
                                              // called when mouse goes over a feature
                                              over: function(feature) {
                                              if (feature === this.interesting_feature) {
                                              return this.overFeature(feature); // do the default thing
                                              }
                                              // otherwise do nothing
                                              }
                                              }
                                              });
    
    map.addControl(drag);
    
    function onDragStart(feature)
    {
      /*  var bounds = feature.geometry.getBounds();
        var deviceType = feature.attributes['device'];
        
        if(deviceType === "room")
        {
            if(lastDragedRoom != feature && lastDragedRoom)
            {
                alert('Save Changes and then continue');
            }
            
        }
        else if(lastDraggedDevice != feature && lastDraggedDevice)
        {
               alert('Save location Changes and then continue');
        }*/
        
        getFeatureLocationInFeet(feature);

    }
    
    function getPixelOnDragging(feature){
        
        var bounds = feature.geometry.getBounds();
        var deviceType = feature.attributes['device'];
        g_Unsaved = 1;
        
        if(deviceType === "room"  || deviceType === "wifiZones")
        {
            
            var vertices = feature.geometry.getVertices();            
            var points ;
            RoompolygonPoints = '';
            for(var i=0;i<vertices.length;i++)
            {
                if(RoompolygonPoints.length == 0)
                {
                    RoompolygonPoints = vertices[i].x + "," + getboxFromYpos(vertices[i].y,ImageHeight);
                }
                else
                {
                    RoompolygonPoints = RoompolygonPoints + " " + vertices[i].x + "," + getboxFromYpos(vertices[i].y,ImageHeight);
                }
                
            }
        }
        else
        {
            
            monitorX = bounds.left;
            monitorY = bounds.top;
            monitorW = bounds.right;
            monitorH = bounds.bottom;
            
            monitorX = convertToSvgEcllipseXCoordinate(parseFloat(monitorX));
            monitorY = convertToSvgEcllipseYCoordinate(parseFloat(monitorY));
            monitorW = 15;
            monitorH = 15;
            
            //checkMonitorWithInARoom();

            
        }
    }
    
}

function changeMapMode()
{
    if(g_designMode == 1 && g_MapView === enumMapView.Map)
    {
        
        for(var i=0;i<editModelayersArray.length;i++)
        {
            editModelayersArray[i].displayInLayerSwitcher = false;
            editModelayersArray[i].setVisibility(false);
        }
        
        addMonitorLayer.setVisibility(true);
       
    }
    else
    {
        
        for(var i=0;i<editModelayersArray.length;i++)
        {
            editModelayersArray[i].displayInLayerSwitcher = true;
            editModelayersArray[i].setVisibility(true);
        }
        
        addMonitorLayer.setVisibility(false);
        
    }
    
}


function GetTagTypeByTypeAndId(nTagType,nTagId)
{
    var strReturn = "ASSET TAG";
    if(nTagType == 0)
    {
        if(nTagId < 600000)
            strReturn = "ASSET TAG";
        else
            strReturn = "MM ASSET TAG";
    }
    else if(nTagType == 1)
    {
         if(nTagId < 600000)
            strReturn = "HHC/STAFF TAG";
         else
            strReturn = "MM STAFF TAG";
    }
    else if(nTagType == 2)
    {
        strReturn = "TEMP TAG";
    }
    else if(nTagType == 3)
    {
        strReturn = "ERU TAG";
    }
    else if(nTagType == 4)
    {
        strReturn = "HUMIDITY TAG";
    }
    else if(nTagType == 5)
    {
        strReturn = "2G TEMP TAG";
    }
    else
         strReturn = "Regular Tag";
    
    return strReturn;
}


 function deleteEditlayerFeature()
 {
      if(selectedFeature)
            {
                //Delete last drawn polygon
                    
                    if(selectedFeature.layer == addMonitorLayer)
                    {
                       if(selectedFeature == lastDrawnDevice || selectedFeature == lastDrawnRoom)
                       {
                       
                                 $('#btnDrawRoom').removeAttr('class').addClass('mapDrawRoom');
                                 $("#btnDrawRoom").prop("disabled",false);
                                 
                                 if(selectedFeature.attributes.description == "Infrastructure-Monitor" || selectedFeature == lastDrawnRoom)
                                 {
                                     $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateMonitor');
                                 }
                                 else if(selectedFeature.attributes.description == "Infrastructure-Star" )
                                 {
                                     $('#btnPlaceMonitor').removeAttr('class').addClass('mapLocateStar');
                                 }  
                                    $("#btnPlaceMonitor").prop("disabled",false);
                                    
                                    
                                 if(selectedFeature == lastDrawnRoom)
                                 {   
                                    RoompolygonPoints = '';
                                    lastDrawnRoom = null;
                                 }
                                 
                                 if(selectedFeature == lastDrawnDevice)
                                 {   
                                     monitorX = -1;
                                     monitorY = -1;
                                     monitorW = -1;
                                     monitorH = -1;
                                    lastDrawnDevice = null;
                                 }
                                 
                                 
                                 addMonitorLayer.removeFeatures([selectedFeature]);
                                 selectedFeature = null;
                                 activateAllControls();
                             
                             
                                
                       }
                       else
                       {
                            if(g_dsvgId == 0)
                            {
                                alert("Please select a monitor or star to delete");
                                return;
                            }
                            var smsg = '';
                            if(g_svgDType == 1)
                                smsg = "Do you want to delete the monitor [" + g_oldDeviceId + "]";
                            else if(g_svgDType == 2)
                                smsg = "Do you want to delete the star [" + g_oldDeviceId + "]";
                            if(confirm(smsg))
                            {
                                 addMonitorLayer.removeFeatures([selectedFeature]);
                                 selectedFeature = null;
                                 deleteSelectedDevice();
                                 //activateAllControls();
                            }
                       }
                         
                       
                    }
                 
            }
 }
 
 
 function getStarRangeRadius(actualWidth,actualLength)
{
    var actualBuildingArea=0;
    
    if(actualWidth != '' && actualLength != '')
        actualBuildingArea = actualWidth * actualLength;
    
    if(actualBuildingArea == 0)
        return 75;    // as default radius
    
    
    var circleArea = 8100; //(45 feet radius) (90 * 90)
    
    var feetRatio = circleArea/actualBuildingArea;
    
    var imgArea = (imageWidth * ImageHeight);
    
    var radius = (imgArea * feetRatio);
    
    var radius = 0.565352 * Math.sqrt(radius);
    
    return radius;
}
function getFeatureLocationInFeet(feature)
{
    if(g_FloorWidthFt=='' && g_FloorLengthFt =='')
        return;
    
    document.getElementById("tblDimensions").style.display="";
    document.getElementById("lblsaveMonitorResult").style.display="none";
    
    var buildingWidth_ft = g_FloorWidthFt;
    var buildingHeight_ft = g_FloorLengthFt;
    
    var xposInFeet = ( ((feature.geometry.getBounds().left + feature.geometry.getBounds().right)/2) /  imageWidth  ) * buildingWidth_ft ;
    var yposInFeet = ( (ImageHeight -  ((feature.geometry.getBounds().top + feature.geometry.getBounds().bottom)/2)) / ImageHeight ) * buildingHeight_ft ;
    
    
    var deviceType = feature.attributes['device'];

    if(deviceType === "room"  || deviceType === "wifiZones")
    {
        document.getElementById("tdlblWidth").style.display="";
        document.getElementById("tdlblLength").style.display="";
        document.getElementById("lblRoomMapX").style.display="";
        document.getElementById("lblRoomMapY").style.display="";
        document.getElementById("lblMapX").style.display="none";
        document.getElementById("lblMapY").style.display="none";
        
        xposInFeet = (feature.geometry.getBounds().left /  imageWidth  ) * buildingWidth_ft ;
        yposInFeet = ((ImageHeight -  feature.geometry.getBounds().top) / ImageHeight ) * buildingHeight_ft ;
   
        
        if(xposInFeet > 0)
            document.getElementById("lblRoomMapX").innerHTML = parseFloat(xposInFeet).toFixed(0) + "' " + ((xposInFeet - Math.floor(xposInFeet)) * 12).toFixed(0) + "''";   // 1feet = 12 inches
        else
            document.getElementById("lblRoomMapX").innerHTML = parseFloat(0);

        if(yposInFeet > 0)
            document.getElementById("lblRoomMapY").innerHTML = parseFloat(yposInFeet).toFixed(0) + "' " + ((yposInFeet - Math.floor(yposInFeet)) * 12).toFixed(0) + "''";
        else
            document.getElementById("lblRoomMapY").innerHTML = parseFloat(0);
    
        var nwidth = (((feature.geometry.getBounds().right - feature.geometry.getBounds().left)/ imageWidth) * buildingWidth_ft);
        var nLength = (((feature.geometry.getBounds().top - feature.geometry.getBounds().bottom)/ ImageHeight) * buildingHeight_ft);
        
        document.getElementById("lblWidth").innerHTML = parseFloat(nwidth).toFixed(0) + "' " + ((nwidth - Math.floor(nwidth)) * 12).toFixed(0) + "''";   // 1feet = 12 inches
       
        document.getElementById("lblLength").innerHTML = parseFloat(nLength).toFixed(0) + "' " + ((nLength - Math.floor(nLength)) * 12).toFixed(0) + "''";   // 1feet = 12 inches
        
      
    }
    else
    {
        document.getElementById("tdlblWidth").style.display="none";
        document.getElementById("tdlblLength").style.display="none";
        document.getElementById("lblRoomMapX").style.display="none";
        document.getElementById("lblRoomMapY").style.display="none";
        document.getElementById("lblMapX").style.display="";
        document.getElementById("lblMapY").style.display="";
        
        if(xposInFeet > 0)
            document.getElementById("lblMapX").innerHTML = parseFloat(xposInFeet).toFixed(0)  + "' " + ((xposInFeet - Math.floor(xposInFeet)) * 12).toFixed(0) + "''";   // 1feet = 12 inches
        else
            document.getElementById("lblMapX").innerHTML = parseFloat(0);

        if(yposInFeet > 0)
            document.getElementById("lblMapY").innerHTML = parseFloat(yposInFeet).toFixed(0) + "' " + ((yposInFeet - Math.floor(yposInFeet)) * 12).toFixed(0) + "''";
        else
            document.getElementById("lblMapY").innerHTML = parseFloat(0);

    }
}
