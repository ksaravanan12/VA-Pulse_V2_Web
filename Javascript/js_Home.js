//*********************************************************
//	Function Name	:	LoadHeartBeatContent
//	Input			:	SiteId
//	Description		:	ajax call LoadHeartBeatContent
//*********************************************************
var g_HBObj;
var g_HBSiteId;

function LoadHeartBeatContent(SiteId)
{
    document.getElementById("divLoading_HeartBeatContent").style.display = "";
    
    g_HBObj = CreateXMLObj();
    g_HBSiteId = SiteId;
    
    if(g_HBObj!=null)
    {
        g_HBObj.onreadystatechange = ajaxLoadHeartBeatContent;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=HeartBeatContent&Site=" + SiteId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_HBObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_HBObj.open("GET",DbConnectorPath, true);
        }
        g_HBObj.send(null);         
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxLoadHeartBeatContent
//	Input			:	none
//	Description		:	Load Heart Beat Datas from ajax Response
//*********************************************************
function ajaxLoadHeartBeatContent()
{
    if(g_HBObj.readyState==4)
    {
        if(g_HBObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_HBObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblHeartBeatContent');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblHeartBeatContent');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var sTbl2,sTblLen2;
            if(GetBrowserType()=="isIE")
            {
                sTbl2=document.getElementById('tblServiceStatus');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl2=document.getElementById('tblServiceStatus');
            }
            sTblLen2=sTbl2.rows.length;
            clearTableRows(sTbl2,sTblLen2);
            
            var dsRoot = g_HBObj.responseXML.documentElement;
             
            var o_Lastupdated=dsRoot.getElementsByTagName('Lastupdated');
            var o_Localtime=dsRoot.getElementsByTagName('Localtime');
            var o_Updated=dsRoot.getElementsByTagName('Updated');
            var o_Freehdspace=dsRoot.getElementsByTagName('Freehdspace');
            var o_Processor=dsRoot.getElementsByTagName('Processor');
            var o_RAM=dsRoot.getElementsByTagName('RAM');
            var o_Systemuptime=dsRoot.getElementsByTagName('Systemuptime');
            var o_ServiceStatus=dsRoot.getElementsByTagName('ServiceStatus');
            var o_AlertHistory=dsRoot.getElementsByTagName('AlertHistory');
            var o_Servicename=dsRoot.getElementsByTagName('Servicename');
            var o_ServiceId=dsRoot.getElementsByTagName('ServiceId');
            var o_Statusason=dsRoot.getElementsByTagName('Statusason');
            var o_Version=dsRoot.getElementsByTagName('Version');

            nRootLength = o_Lastupdated.length;

            document.getElementById('lblStatus').innerHTML = '';
            if (nRootLength == 0) {
                document.getElementById('lblStatus').innerHTML = 'Server & Service Status Not Available for this Site';
            }
             
            if(nRootLength >0)
            {
                row=document.createElement('tr');
                AddCell(row,"&nbsp;&nbsp;Server Status",'siteOverview_TopLeft_Box DeviceDetailsHeaderText',7,"","left","800px","40px","");
                sTbl.appendChild(row);
                
                row=document.createElement('tr');
                AddCell(row,"&nbsp;&nbsp;Service Status",'siteOverview_TopLeft_Box DeviceDetailsHeaderText',5,"","left","800px","40px","");
                sTbl2.appendChild(row);
                
                //Header
                row=document.createElement('tr');
                AddCell(row,"Last Updated",'siteOverview_leftBox',"","","center","100px","40px","");
                AddCell(row,"Local Time",'siteOverview_Box',"","","center","125px","40px","");
                AddCell(row,"Updated",'siteOverview_Box',"","","center","125px","40px","");
                AddCell(row,"Free HD Space",'siteOverview_Box',"","","center","125px","40px","");
                AddCell(row,"Processor",'siteOverview_Box',"","","center","100px","40px","");
                AddCell(row,"RAM",'siteOverview_Box',"","","center","75px","40px","");
                AddCell(row,"System Up Time",'siteOverview_Box',"","","center","125px","40px","");
                sTbl.appendChild(row);
                
                row=document.createElement('tr');
                AddCell(row,"Status",'siteOverview_leftBox',"","","center","100px","40px","");
                AddCell(row,"Alert History",'siteOverview_Box',"","","center","100px","40px","");
                AddCell(row,"Service Name",'siteOverview_Box',"","","center","200px","40px","");
                AddCell(row,"Status As On",'siteOverview_Box',"","","center","200px","40px","");
                AddCell(row,"Version",'siteOverview_Box',"","","center","175px","40px","");
                sTbl2.appendChild(row);
                
                //Datas
                var Lastupdated = (o_Lastupdated[0].textContent || o_Lastupdated[0].innerText || o_Lastupdated[0].text);
                var Localtime = (o_Localtime[0].textContent || o_Localtime[0].innerText || o_Localtime[0].text);
                var Updated = (o_Updated[0].textContent || o_Updated[0].innerText || o_Updated[0].text);
                var Freehdspace = (o_Freehdspace[0].textContent || o_Freehdspace[0].innerText || o_Freehdspace[0].text);
                var Processor = (o_Processor[0].textContent || o_Processor[0].innerText || o_Processor[0].text);
                var RAM = (o_RAM[0].textContent || o_RAM[0].innerText || o_RAM[0].text);
                var Systemuptime = (o_Systemuptime[0].textContent || o_Systemuptime[0].innerText || o_Systemuptime[0].text);
                
                row=document.createElement('tr');
                AddCell(row,Lastupdated,'DeviceList_leftBox',"","","center","100px","40px","");
                AddCell(row,Localtime,'siteOverview_cell',"","","center","125px","40px","");
                AddCell(row,Updated,'siteOverview_cell',"","","center","125px","40px","");
                AddCell(row,Freehdspace,'siteOverview_cell',"","","center","125px","40px","");
                AddCell(row,Processor,'siteOverview_cell',"","","center","100px","40px","");
                AddCell(row,RAM,'siteOverview_cell',"","","center","75px","40px","");
                AddCell(row,Systemuptime,'siteOverview_cell',"","","center","125px","40px","");
                sTbl.appendChild(row);
                
                for(var i=0; i<nRootLength; i++)
                {
                    var ServiceStatus = (o_ServiceStatus[i].textContent || o_ServiceStatus[i].innerText || o_ServiceStatus[i].text);
                    var AlertHistory = (o_AlertHistory[i].textContent || o_AlertHistory[i].innerText || o_AlertHistory[i].text);
                    var Servicename = (o_Servicename[i].textContent || o_Servicename[i].innerText || o_Servicename[i].text);
                    var ServiceId = (o_ServiceId[i].textContent || o_ServiceId[i].innerText || o_ServiceId[i].text);
                    var Statusason = (o_Statusason[i].textContent || o_Statusason[i].innerText || o_Statusason[i].text);
                    var Version = (o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text);
                    var ServiceStatusTemp = ServiceStatus;
                    if(ServiceStatus == "1")
                    {
                        ServiceStatus = "<img src='Images/active.png' height='20' width='20' id='activeimg-" + i + "' onmouseover='btnPrev_LAServiceOver(this)' onmouseout='btnPrev_LAServiceOut(this)' />";
                    }
                    else if(ServiceStatus == "2")
                    {
                        ServiceStatus = "<img src='Images/stopped.png' height='20' width='20' id='stoppedimg-" + i + "' onmouseover='btnPrev_LAServiceOver(this)' onmouseout='btnPrev_LAServiceOut(this)' />";
                    }
                    else if(ServiceStatus == "0")
                    {
                        ServiceStatus = "<img src='Images/uninstalled.png' height='20' width='20' id='uninstallimg-" + i + "' onmouseover='btnPrev_LAServiceOver(this)' onmouseout='btnPrev_LAServiceOut(this)' />";
                    }
                     else if(ServiceStatus == "-1")
                    {
                        ServiceStatus = "<img src='Images/UnknownStatus.png' height='20' width='20' id='UnknownStatusimg-" + i + "' onmouseover='btnPrev_LAServiceOver(this)' onmouseout='btnPrev_LAServiceOut(this)' />";
                    }
                    
                    if(setundefined(AlertHistory)!= '' )
                    {
                        AlertHistory = "<img src='Images/imgAlerts.png' height='20' width='20' id='alerthistory-" + i + "' onclick=ShowAlertHistoryForService(" + g_HBSiteId + ",'" + encodeURIComponent(Servicename) + "'," + ServiceId + ",'" + AlertHistory + "') onmouseover='btnPrev_LAServiceOver(this)' onmouseout='btnPrev_LAServiceOut(this)' />";
                    }
                    else
                    {
                        AlertHistory = "&nbsp;";
                    }
                    
                    row=document.createElement('tr');
                    AddCell(row,ServiceStatus,'DeviceList_leftBox',"","","center","100px","40px","");
                    AddCell(row,AlertHistory,'siteOverview_cell',"","","center","100px","40px","");
                    AddCell(row,Servicename,'siteOverview_cell',"","","center","200px","40px","");
                    if(ServiceStatusTemp == "-1")
                    {
                        AddCell(row,"&nbsp;",'siteOverview_cell',"","","center","200px","40px","");
                    }
                    else
                    {
                        AddCell(row,Statusason,'siteOverview_cell',"","","center","200px","40px","");   
                    }
                    AddCell(row,Version,'siteOverview_cell',"","","center","175px","40px","");
                    sTbl2.appendChild(row);
                }
            }
            
            document.getElementById("divLoading_HeartBeatContent").style.display = "none";
        }
    }
}


//*********************************************************
//	Function Name	:	LoadAlertHistory
//	Input			:	SiteId
//	Description		:	ajax call LoadAlertHistory
//*********************************************************
var g_AHObj;
function LoadAlertHistoryForService(SiteId,ServiceId)
{
    document.getElementById("divLOading_AlertHistory").style.display = "";
    
    g_AHObj = CreateXMLObj();
    
    if(g_AHObj!=null)
    {
        g_AHObj.onreadystatechange = ajaxLoadAlertHistory;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=AlertHistoryForService&Site=" + SiteId + "&ServiceId=" + ServiceId ;
      
        if(GetBrowserType()=="isIE")
        {	
            g_AHObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_AHObj.open("GET",DbConnectorPath, true);
        }
        g_AHObj.send(null);         
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxLoadHeartBeatContent
//	Input			:	none
//	Description		:	Load Heart Beat Datas from ajax Response
//*********************************************************
function ajaxLoadAlertHistory()
{
    if(g_AHObj.readyState==4)
    {
        if(g_AHObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_AHObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblAlertHistory');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblAlertHistory');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_AHObj.responseXML.documentElement;
             
            var o_Name=dsRoot.getElementsByTagName('Name');
            var o_ToTalCount=dsRoot.getElementsByTagName('ToTalCount');
            var o_DeviceId=dsRoot.getElementsByTagName('DeviceId');
            var o_SiteName=dsRoot.getElementsByTagName('SiteName');
            var o_SiteId=dsRoot.getElementsByTagName('SiteId');
            var o_AlertDescription=dsRoot.getElementsByTagName('AlertDescription');
            var o_AlertId=dsRoot.getElementsByTagName('AlertId');
            var o_AlertTime=dsRoot.getElementsByTagName('AlertTime');
            var o_DeviceSubType=dsRoot.getElementsByTagName('DeviceSubType');
            var o_MacId=dsRoot.getElementsByTagName('MacId');
            var o_AlertDuration=dsRoot.getElementsByTagName('AlertDuration');
            var o_ResolvedOn=dsRoot.getElementsByTagName('ResolvedOn');
            var o_XmlData=dsRoot.getElementsByTagName('XmlData');
             
            nRootLength=o_DeviceId.length;
             
            if(nRootLength >0)
            {
                row=document.createElement('tr');
                AddCell(row,"&nbsp;Alert&nbsp;History",'siteOverview_TopLeft_Box DeviceDetailsHeaderText',5,"","left","800px","40px","");
                sTbl.appendChild(row);
                
                //Header
                row=document.createElement('tr');
                AddCell(row,"Alert Id",'siteOverview_leftBox',"","","center","75px","40px","");
                AddCell(row,"Alert Time",'siteOverview_Box',"","","center","150px","40px","");
                AddCell(row,"Resolved On",'siteOverview_Box',"","","center","150px","40px","");
                AddCell(row,"Alert Duration",'siteOverview_Box',"","","center","125px","40px","");
                sTbl.appendChild(row);
                
                if(o_XmlData.length > 0)
                {
                    XmlData = (o_XmlData[0].textContent || o_XmlData[0].innerText || o_XmlData[0].text);
                    
                    FusionCharts.setCurrentRenderer('javascript');
                    var myChart = new FusionCharts("Gantt","myChartId","650","170","0");
                    myChart.setXMLData(XmlData);
                    myChart.render("divChart_AlertHistory"); 
                }
                
                for(var i=0; i<nRootLength; i++)
                {
                    var AlertId = (o_AlertId[i].textContent || o_AlertId[i].innerText || o_AlertId[i].text);
                    var AlertTime = (o_AlertTime[i].textContent || o_AlertTime[i].innerText || o_AlertTime[i].text);
                    var ResolvedOn = (o_ResolvedOn[i].textContent || o_ResolvedOn[i].innerText || o_ResolvedOn[i].text);
                    var AlertDuration = (o_AlertDuration[i].textContent || o_AlertDuration[i].innerText || o_AlertDuration[i].text);
                    
                    row=document.createElement('tr');
                    AddCell(row,AlertId,'DeviceList_leftBox',"","","center","100px","40px","");
                    AddCell(row,AlertTime,'siteOverview_cell',"","","center","200px","40px","");
                    AddCell(row,ResolvedOn,'siteOverview_cell',"","","center","200px","40px","");
                    AddCell(row,AlertDuration,'siteOverview_cell',"","","center","175px","40px","");
                    sTbl.appendChild(row);
                }
            }
            
            document.getElementById("divLOading_AlertHistory").style.display = "none";
        }
    }
}


//*********************************************************
//	Function Name	:	LoadLocalAlertsForAllService
//	Input			:	SiteId,dtType,CurrPg,Date,PgType,AlertGraphType
//	Description		:	ajax call LoadLocalAlertsForAllService
//*********************************************************
var g_LAObj;
var g_ServiceName = "";
var g_dtVal = 0;
var g_AlertRoot;
var g_AlertPopupData_tag;
var g_AlertPopupData_monitor;
var g_AlertPopupData_star;
var g_AlertPopupData_services;

function LoadLocalAlertsForAllService(SiteId,dtType,CurrPg,Date,PgType,AlertGraphType,ServiceName,LAServiceId,DeviceId,bIsPageLoad)
{
    g_LAObj = CreateXMLObj();
    g_ServiceName = ServiceName;

    if (document.getElementById("divLOading_AlertHistory") != null) { document.getElementById("divLOading_AlertHistory").style.display = ""; }

    if (document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails") != null) {
        var dtIdx = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails").selectedIndex;
        var dtVal = document.getElementById("ctl00_ContentPlaceHolder1_ddlDeviceType_DeviceDetails").options[dtIdx].value;

        if (g_dtVal != dtVal) {
            g_dtVal = dtVal;
            dtType = 1;
            if (dtVal == -1) { AlertGraphType = 0; }
            if (!bIsPageLoad)
                setDefaultLocalAlerts(DeviceId, AlertGraphType);
        }
    }
    
    if(g_LAObj!=null)
    {
        g_LAObj.onreadystatechange = ajaxLoadLocalAlerts;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LocalAlertsForSite&Site=" + SiteId + "&curpage=" + CurrPg + "&DateRngType=" + dtType + "&StartDate=" + Date + "&PgType=" + PgType + "&AlertGraphType=" + AlertGraphType + "&ServiceName=" + ServiceName + "&LAServiceId=" + LAServiceId + "&DeviceId=" + DeviceId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_LAObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_LAObj.open("GET",DbConnectorPath, true);
        }
        g_LAObj.send(null);         
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxLoadLocalAlerts
//	Input			:	none
//	Description		:	Load Local Alerts Datas from ajax Response
//*********************************************************
function ajaxLoadLocalAlerts()
{
    if(g_LAObj.readyState==4)
    {
        if(g_LAObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_LAObj.responseXML.documentElement);
            
            var dsRoot = g_LAObj.responseXML.documentElement;
             
            var o_XmlData_Service=dsRoot.getElementsByTagName('XmlData_Service');
            var o_XmlData_Tag=dsRoot.getElementsByTagName('XmlData_Tag');
            var o_XmlData_Monitor=dsRoot.getElementsByTagName('XmlData_Monitor');
            var o_XmlData_Star=dsRoot.getElementsByTagName('XmlData_Star');
            var o_StartDate = dsRoot.getElementsByTagName('StartDate');
             
            //Graph for Service
            if(o_XmlData_Service.length > 0)
            {
                if (document.getElementById("trLocalServices") != null) { document.getElementById("trLocalServices").style.display = "" };
                
                XmlData_Service = (o_XmlData_Service[0].textContent || o_XmlData_Service[0].innerText || o_XmlData_Service[0].text);
                
                if(g_ServiceName != "All")
                {
                    if(FusionCharts("myChartIdHistory")!=undefined)
                        FusionCharts("myChartIdHistory").dispose();
                    FusionCharts.setCurrentRenderer('javascript');
                    var myChart = new FusionCharts("Gantt","myChartIdHistory","650","125","0");
                    myChart.setXMLData(XmlData_Service);
                    myChart.render("divChart_AlertHistory"); 
                    myChart = null;
                    document.getElementById("divLOading_AlertHistory").style.display = "none";
                    
                    StartDate = (o_StartDate[0].textContent || o_StartDate[0].innerText || o_StartDate[0].text);
                    //g_AlertHistoryDate = StartDate;
                    document.getElementById("ctl00_ContentPlaceHolder1_txtDate_AlertHistory").value = StartDate;
                    
                    g_AlertPopupData_services= dsRoot;
                    
                    LoadAlertHistoryTable(dsRoot);
                }
                else
                {
                    if(FusionCharts("myChartId1")!=undefined)
                        FusionCharts("myChartId1").dispose();
                    FusionCharts.setCurrentRenderer('javascript');
                    var myChart = new FusionCharts("Gantt","myChartId1","650","375","0");
                    myChart.setXMLData(XmlData_Service);
                    myChart.render("divGraph_LocalAlerts_Service");
                    
                    myChart = null;
                    
                    StartDate = (o_StartDate[0].textContent || o_StartDate[0].innerText || o_StartDate[0].text);
                    g_Date  = StartDate;
                    document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAServiceAlerts").value = StartDate;
                    
                    document.getElementById("divLoading_LocalAlerts").style.display = "none";
                    
                    g_AlertPopupData_services = dsRoot;
                }
            }
            
            //Graph for Tag
            if(o_XmlData_Tag.length > 0)
            {
                document.getElementById("trTag_Alerts").style.display = "";
                
                XmlData_Tag = (o_XmlData_Tag[0].textContent || o_XmlData_Tag[0].innerText || o_XmlData_Tag[0].text);
                if(FusionCharts("myChartId2")!=undefined)
                    FusionCharts("myChartId2").dispose();
                FusionCharts.setCurrentRenderer('javascript');
                var myChart = new FusionCharts("Gantt","myChartId2","650","180","0");
                myChart.setXMLData(XmlData_Tag);
                myChart.render("divGraph_LocalAlerts_Tag"); 
               
                myChart = null; 
                
                StartDate = (o_StartDate[0].textContent || o_StartDate[0].innerText || o_StartDate[0].text);
                document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LATag").value = StartDate;
                
                document.getElementById("divLoading_TagAlerts").style.display = "none";
                
                g_AlertPopupData_tag=dsRoot;

            }
            
            //Graph for Monitor
            if(o_XmlData_Monitor.length > 0)
            {
                document.getElementById("trMonitor_Alerts").style.display = "";
                
                XmlData_Monitor = (o_XmlData_Monitor[0].textContent || o_XmlData_Monitor[0].innerText || o_XmlData_Monitor[0].text);
                if(FusionCharts("myChartId3")!=undefined)
                    FusionCharts("myChartId3").dispose();
                FusionCharts.setCurrentRenderer('javascript');
                var myChart = new FusionCharts("Gantt","myChartId3","650","230","0");
                myChart.setXMLData(XmlData_Monitor);
                myChart.render("divGraph_LocalAlerts_Monitor"); 
                myChart = null;
                
                StartDate = (o_StartDate[0].textContent || o_StartDate[0].innerText || o_StartDate[0].text);
                document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAMonitor").value = StartDate;
                
                document.getElementById("divLoading_MonitorAlerts").style.display = "none";

                g_AlertPopupData_monitor = dsRoot;
            }
            
            //Graph for Star
            if(o_XmlData_Star.length > 0)
            {
                document.getElementById("trStar_Alerts").style.display = "";
                
                XmlData_Star = (o_XmlData_Star[0].textContent || o_XmlData_Star[0].innerText || o_XmlData_Star[0].text);
                if(FusionCharts("myChartId4")!=undefined)
                    FusionCharts("myChartId4").dispose();
                FusionCharts.setCurrentRenderer('javascript');
                var myChart = new FusionCharts("Gantt","myChartId4","650","330","0");
                myChart.setXMLData(XmlData_Star);
                myChart.render("divGraph_LocalAlerts_Star"); 
                myChart = null;
               
               
                StartDate = (o_StartDate[0].textContent || o_StartDate[0].innerText || o_StartDate[0].text);
                document.getElementById("ctl00_ContentPlaceHolder1_txtDate_LAStar").value = StartDate;
                
                document.getElementById("divLoading_StarAlerts").style.display = "none";
                
                g_AlertPopupData_star = dsRoot;
                
            }
        }
    }
}

//*********************************************************
//	Function Name	:	setDefaultLocalAlerts
//	Input			:	dsRoot
//	Description		:	Hide All Local Alerts
//*********************************************************
function setDefaultLocalAlerts(deviceId,deviceType)
{
    document.getElementById("trLocalServices").style.display = "none";
    document.getElementById("trTag_Alerts").style.display = "none";
    document.getElementById("trMonitor_Alerts").style.display = "none";
    document.getElementById("trStar_Alerts").style.display = "none";
}

//*********************************************************
//	Function Name	:	LoadAlertHistoryTable
//	Input			:	dsRoot
//	Description		:	Load Alert History Html Table
//*********************************************************
function LoadAlertHistoryTable(dsRoot)
{
    var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblAlertHistory');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblAlertHistory');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
            
    var o_AlertId=dsRoot.getElementsByTagName('AlertId');
    var o_AlertTime=dsRoot.getElementsByTagName('AlertTime');
    var o_ResolvedOn=dsRoot.getElementsByTagName('ResolvedOn');
    var o_AlertDuration=dsRoot.getElementsByTagName('AlertDuration');
    var o_AlertTimeStr=dsRoot.getElementsByTagName('AlertTimeStr');
    var o_ResolvedOnStr=dsRoot.getElementsByTagName('ResolvedOnStr');
    
    var AlertDate;
    var ResolvedDate;
     
    nRootLength=o_AlertId.length;
    
    row=document.createElement('tr');
    AddCell(row,"&nbsp;Alert&nbsp;History",'siteOverview_TopLeft_Box DeviceDetailsHeaderText',5,"","left","800px","40px","");
    sTbl.appendChild(row);
    
    //Header
    row=document.createElement('tr');
    AddCell(row,"Alert Id",'siteOverview_leftBox',"","","center","75px","40px","");
    AddCell(row,"Alert Time",'siteOverview_Box',"","","center","150px","40px","");
    AddCell(row,"Resolved On",'siteOverview_Box',"","","center","150px","40px","");
    AddCell(row,"Alert Duration",'siteOverview_Box',"","","center","125px","40px","");
    sTbl.appendChild(row);
        
    if(nRootLength >0)
    {
        for(var i=0; i<nRootLength; i++)
        {
            var AlertId = (o_AlertId[i].textContent || o_AlertId[i].innerText || o_AlertId[i].text);
            var AlertTime = (o_AlertTime[i].textContent || o_AlertTime[i].innerText || o_AlertTime[i].text);
            var ResolvedOn = (o_ResolvedOn[i].textContent || o_ResolvedOn[i].innerText || o_ResolvedOn[i].text);
            var AlertDuration = (o_AlertDuration[i].textContent || o_AlertDuration[i].innerText || o_AlertDuration[i].text);
            var AlertTimeStr = (o_AlertTimeStr[i].textContent || o_AlertTimeStr[i].innerText || o_AlertTimeStr[i].text);
            var ResolvedOnStr = (o_ResolvedOnStr[i].textContent || o_ResolvedOnStr[i].innerText || o_ResolvedOnStr[i].text);
            
            //AlertDate = new Date(AlertTime);
            //AlertDate = (AlertDate.getMonth() + 1) + "/" + AlertDate.getDate() + "/" + AlertDate.getFullYear() + " " + AlertDate.getHours() + ":" + AlertDate.getMinutes() + ":" + AlertDate.getSeconds();
            
            //ResolvedDate = new Date(ResolvedOn);
            //ResolvedDate = (ResolvedDate.getMonth() + 1) + "/" + ResolvedDate.getDate() + "/" + ResolvedDate.getFullYear() + " " + ResolvedDate.getHours() + ":" + ResolvedDate.getMinutes() + ":" + ResolvedDate.getSeconds();
            
            AlertDate = AlertTimeStr;
            ResolvedDate = ResolvedOnStr;
            
            row=document.createElement('tr');
            AddCell(row,AlertId,'DeviceList_leftBox',"","","center","100px","40px","");
            AddCell(row,AlertDate,'siteOverview_cell',"","","center","200px","40px","");
            AddCell(row,ResolvedDate,'siteOverview_cell',"","","center","200px","40px","");
            AddCell(row,AlertDuration,'siteOverview_cell',"","","center","175px","40px","");
            sTbl.appendChild(row);
        }
    }
    else
    {
        row=document.createElement('tr');
        AddCell(row,"No Alerts",'DeviceList_leftBox',4,"","center","100px","40px","");
        sTbl.appendChild(row);
    }
    
    document.getElementById("divLOading_AlertHistory").style.display = "none";
}

function showAlertsInfo(ServiceId,ServiceName,StartDate,EndDate,bIsCategory,deviceType)
{
    var isCategory = false;
    var dsRoot;
    isCategory = bIsCategory;
    if(deviceType == "1")
        dsRoot = g_AlertPopupData_tag;
    else if (deviceType == "2")
        dsRoot = g_AlertPopupData_monitor;
    else if (deviceType == "3")
        dsRoot = g_AlertPopupData_star;
    AlertInit(ServiceId,ServiceName,StartDate,EndDate,true,"",5,true,isCategory,dsRoot);
}

function showAlertsInfoforService(ServiceId,ServiceName,StartDate,EndDate,bIsCategory)
{
    var isCategory = false;
    isCategory = bIsCategory;
    AlertInit(ServiceId,ServiceName,StartDate,EndDate,true,"",4,false,isCategory,g_AlertPopupData_services);
}

function showNoAlertsInfo(ServiceName,StartDate,EndDate)
{
    AlertInit("",ServiceName,StartDate,EndDate,"",true,5,true,false,"");
}

function showNoAlertsInfoforService(ServiceName,StartDate,EndDate)
{
    AlertInit("",ServiceName,StartDate,EndDate,"",true,4,"",false,"");
}

function AlertInit(ServiceId,ServiceName,StartDate,EndDate,isAddData,isNoRecordfound,nColspan,bShowDeviceId,bIsCategory,sRoot)
{
    var sTblSiteAlertInfo = document.getElementById("siteAlertinfo");
    var sTblAlertInfo = document.getElementById("tblAlertInfo");
    
    displayAlertInfo(sTblSiteAlertInfo,sTblAlertInfo,ServiceName);
    AddDateRangeRow(StartDate,EndDate,sTblAlertInfo,nColspan);
    AddHeader(sTblAlertInfo,bShowDeviceId);
    
    if(isAddData)
    {
        AddDatas(ServiceId,ServiceName,StartDate,EndDate,bShowDeviceId,sTblAlertInfo,bIsCategory,nColspan,sRoot);
    }
    
    if(isNoRecordfound)
    {
        NoRecordsFoundRow(sTblAlertInfo,nColspan);
    }
    
    document.getElementById("divLoading_DeviceDetail").style.display = "none";
}

function AddDateRangeRow(StartDate,EndDate,sTblAlertInfo,nColspan)
{
    var row;
    var fdate = new Date(StartDate);
    var edate = new Date(EndDate);
                        
    var sFromDate = formatAMPM(fdate);
    var sEndDate = formatAMPM(edate);
    
    row=document.createElement('tr');
    AddCell(row,sFromDate + " - " + sEndDate,'subHeader_black',nColspan,"","center","","40px","");
    sTblAlertInfo.appendChild(row);
}

function AddHeader(sTblAlertInfo,bShowDeviceId)
{
    row=document.createElement('tr');
    AddCell(row,"Alert Id",'siteOverview_TopLeft_Box',"","","center","75px","40px","");
    if(bShowDeviceId)
        AddCell(row,"Device Id",'siteOverview_Box',"","","center","100px","40px","");
    AddCell(row,"Alert Time",'siteOverview_Box',"","","center","175px","40px","");
    AddCell(row,"Resolved On",'siteOverview_Box',"","","center","175px","40px","");
    AddCell(row,"Alert Duration",'siteOverview_Box',"","","center","75px","40px","");
    sTblAlertInfo.appendChild(row);
}

function NoRecordsFoundRow(sTblAlertInfo,nColspan)
{
    row=document.createElement('tr');
    AddCell(row,"No Alerts",'DeviceList_leftBox',nColspan,"","center","100px","40px","");
    sTblAlertInfo.appendChild(row);
}

function displayAlertInfo(sTblSiteAlertInfo,sTblAlertInfo,ServiceName)
{
    var sTblSiteAlertInfoLen = sTblSiteAlertInfo.rows.length;
    var sTblAlertInfoLen = sTblAlertInfo.rows.length;
    
    clearTableRows(sTblSiteAlertInfo,sTblSiteAlertInfoLen);
    clearTableRows(sTblAlertInfo,sTblAlertInfoLen);
    
    document.getElementById("lblHeaderName").innerHTML = ServiceName;
    document.getElementById("divLoading_DeviceDetail").style.display = "";
    showAlerts();
}

function AddDatas(ServiceId,ServiceName,StartDate,EndDate,bShowDeviceId,sTblAlertInfo,bIsCategory,nColspan,sRoot)
{
    var RootLength = 0;
    var nAdded = 0;
    var ChkId;
    var bAddData = false;
    
    g_AlertRoot = sRoot;
    //g_AlertRoot = g_LAObj.responseXML.documentElement;
    
    if(g_AlertRoot != null)
    {
        var o_ComponentId = g_AlertRoot.getElementsByTagName('ComponentId');
        var o_SiteId = g_AlertRoot.getElementsByTagName('SiteId');
        var o_SiteName = g_AlertRoot.getElementsByTagName('SiteName');
        var o_DeviceId = g_AlertRoot.getElementsByTagName('DeviceId');
        var o_AlertId = g_AlertRoot.getElementsByTagName('AlertId');
        var o_AlertDescription = g_AlertRoot.getElementsByTagName('AlertDescription');
        var o_AlertTypeId = g_AlertRoot.getElementsByTagName('AlertTypeId');
        var o_AlertTime = g_AlertRoot.getElementsByTagName('AlertTime');
        var o_ResolvedOn = g_AlertRoot.getElementsByTagName('ResolvedOn');
        var o_AlertDuration = g_AlertRoot.getElementsByTagName('AlertDuration');
        var o_AlertTimeInSecs = g_AlertRoot.getElementsByTagName('AlertTimeInSecs');
        var o_ResolvedOnInSecs = g_AlertRoot.getElementsByTagName('ResolvedOnInSecs');
        
        RootLength = o_AlertId.length;
        nAdded = 0;
        
        if(RootLength > 0)
        {
            for(var i = 0; i < RootLength; i++)
            {
                var ComponentId = (o_ComponentId[i].textContent || o_ComponentId[i].innerText || o_ComponentId[i].text);
                var SiteId = (o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
                var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);
                var DeviceId = (o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
                var AlertId = (o_AlertId[i].textContent || o_AlertId[i].innerText || o_AlertId[i].text);
                var AlertDescription = (o_AlertDescription[i].textContent || o_AlertDescription[i].innerText || o_AlertDescription[i].text);
                var AlertTypeId = (o_AlertTypeId[i].textContent || o_AlertTypeId[i].innerText || o_AlertTypeId[i].text);
                var AlertTime = (o_AlertTime[i].textContent || o_AlertTime[i].innerText || o_AlertTime[i].text);
                var ResolvedOn = (o_ResolvedOn[i].textContent || o_ResolvedOn[i].innerText || o_ResolvedOn[i].text);
                var AlertDuration = (o_AlertDuration[i].textContent || o_AlertDuration[i].innerText || o_AlertDuration[i].text);
                var AlertTimeInSecs = (o_AlertTimeInSecs[i].textContent || o_AlertTimeInSecs[i].innerText || o_AlertTimeInSecs[i].text);
                var ResolvedOnInSecs = (o_ResolvedOnInSecs[i].textContent || o_ResolvedOnInSecs[i].innerText || o_ResolvedOnInSecs[i].text);
                
                ChkId = "";
                bAddData = false;
               
               // var sEndDateTime = formatAMPM(ResolvedOn);
                //var sStartDateTime = formatAMPM(AlertTime);
                
                var sEndDateTime = formatAMPM(ResolvedOnInSecs);
                var sStartDateTime = formatAMPM(AlertTimeInSecs);
                
                
                var endDate = new Date(EndDate).getTime()/1000;
                var startDate = new Date(StartDate).getTime()/1000;
                
                if(bShowDeviceId)
                    ChkId = AlertTypeId;
                else
                    ChkId = ComponentId;
                
                
               if(Date.parse(sStartDateTime) < Date.parse(EndDate) && Date.parse(sEndDateTime) > Date.parse(StartDate))
                    bAddData = true;
                
                if(ServiceId == ChkId)
                {
                    if(bAddData == true)
                    {
                        if(setundefined(AlertDuration) == "")
                            sEndDateTime = "Active till now";
                            
                        row=document.createElement('tr');
                        AddCell(row,AlertId,'DeviceList_leftBox',"","","","","40px","");
                        if(bShowDeviceId)
                            AddCell(row,DeviceId,'siteOverview_cell',"","","","","40px","");
                        AddCell(row,sStartDateTime,'siteOverview_cell',"","","","","40px","");
                        AddCell(row,sEndDateTime,'siteOverview_cell',"","","","","40px","");
                        AddCell(row,AlertDuration,'DeviceList_leftBox',"","","","","40px","");
                        sTblAlertInfo.appendChild(row);
                        
                        nAdded = parseInt(nAdded) + 1;
                    }
                }
            }
        }
    }
        
    if(nAdded == 0)
    {
        NoRecordsFoundRow(sTblAlertInfo,nColspan);
    }
}

//*********************************************************
//	Function Name	:	LoadAlertsServiceTableView
//	Input			:	SiteId
//	Description		:	ajax call LoadAlertHistory
//*********************************************************
var g_ATVObj;
function LoadAlertsServiceTableView(SiteId,CurrPage,Sort,startDate,endDate,alertIds,deviceId)
{
    document.getElementById("divLoading_TableView").style.display = "";
    
    g_ATVObj = CreateXMLObj();
    
    if(g_ATVObj!=null)
    {
        g_ATVObj.onreadystatechange = ajaxLoadAlertsServiceTableView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadAlertsServiceTableView&Site=" + SiteId + "&curpage=" + CurrPage + "&SortAlerts=" + Sort + "&StartDate=" + startDate + "&EndDate=" + endDate + "&AlertIds=" + alertIds + "&DeviceId=" + deviceId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_ATVObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_ATVObj.open("GET",DbConnectorPath, true);
        }
        g_ATVObj.send(null);         
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxLoadHeartBeatContent
//	Input			:	none
//	Description		:	Load Heart Beat Datas from ajax Response
//*********************************************************
function ajaxLoadAlertsServiceTableView()
{
    if(g_ATVObj.readyState==4)
    {
        if(g_ATVObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_ATVObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblLA_TableView');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblLA_TableView');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_ATVObj.responseXML.documentElement;
             
            var o_AlertStartDateForSite=dsRoot.getElementsByTagName('AlertStartDateForSite');
            var o_TotalPage=dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount=dsRoot.getElementsByTagName('TotalCount');
            var o_ComponentId=dsRoot.getElementsByTagName('ComponentId');
            var o_DeviceId=dsRoot.getElementsByTagName('DeviceId');
            var o_SiteName=dsRoot.getElementsByTagName('SiteName');
            var o_SiteId=dsRoot.getElementsByTagName('SiteId');
            var o_AlertDescription=dsRoot.getElementsByTagName('AlertDescription');
            var o_AlertTypeId=dsRoot.getElementsByTagName('AlertTypeId');
            var o_AlertId=dsRoot.getElementsByTagName('AlertId');
            var o_AlertTime=dsRoot.getElementsByTagName('AlertTime');
            var o_DeviceSubType=dsRoot.getElementsByTagName('DeviceSubType');
            var o_MacId=dsRoot.getElementsByTagName('MacId');
            var o_AlertDuration=dsRoot.getElementsByTagName('AlertDuration');
            var o_ResolvedOn=dsRoot.getElementsByTagName('ResolvedOn');
            var o_ResolvedBy=dsRoot.getElementsByTagName('ResolvedBy');
             
            nRootLength=o_DeviceId.length;
            
            //Header
            row=document.createElement('tr');
            AddCellForSorting(row,"Device",'siteOverview_TopLeft_Box',"","","center","120px","40px","","DeviceSubType",g_SortColumn,g_SortImg,enumSortingArr.LocalAlertsTableView);
            AddCellForSorting(row,"Alert Description",'siteOverview_Box',"","","center","120px","40px","","AlertDesc",g_SortColumn,g_SortImg,enumSortingArr.LocalAlertsTableView);
            AddCellForSorting(row,"Alert Time",'siteOverview_Box',"","","center","120px","40px","","AlertedOn",g_SortColumn,g_SortImg,enumSortingArr.LocalAlertsTableView);
            AddCellForSorting(row,"Resolved On",'siteOverview_Box',"","","center","120px","40px","","ResolvedOn",g_SortColumn,g_SortImg,enumSortingArr.LocalAlertsTableView);
            AddCellForSorting(row,"Alert Duration",'siteOverview_Box',"","","center","70px","40px","","AlertDuration",g_SortColumn,g_SortImg,enumSortingArr.LocalAlertsTableView);
            sTbl.appendChild(row);

            document.getElementById('lblStatus').innerHTML = '';
            if (nRootLength == 0) {
                $('#trFilter').hide();
                $('#trLoalServicesTableView').hide();
                document.getElementById('lblStatus').innerHTML = 'Local Alerts Not Available for this Site';
            }
            
            if(nRootLength >0)
            {
                $('#trFilter').show();
                $('#trLoalServicesTableView').show();

                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_TableView").innerHTML = "Total Records : " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
                document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_TableView").innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
                
                for(var i = 0; i < nRootLength; i++)
                {
                    var AlertStartDateForSite = (o_AlertStartDateForSite[i].textContent || o_AlertStartDateForSite[i].innerText || o_AlertStartDateForSite[i].text);
                    var ComponentId = (o_ComponentId[i].textContent || o_ComponentId[i].innerText || o_ComponentId[i].text);
                    var DeviceId = (o_DeviceId[i].textContent || o_DeviceId[i].innerText || o_DeviceId[i].text);
                    var DeviceSubType = (o_DeviceSubType[i].textContent || o_DeviceSubType[i].innerText || o_DeviceSubType[i].text);
                    var AlertId = (o_AlertId[i].textContent || o_AlertId[i].innerText || o_AlertId[i].text);
                    var AlertDescription = (o_AlertDescription[i].textContent || o_AlertDescription[i].innerText || o_AlertDescription[i].text);
                    var AlertTime = (o_AlertTime[i].textContent || o_AlertTime[i].innerText || o_AlertTime[i].text);
                    var ResolvedOn = (o_ResolvedOn[i].textContent || o_ResolvedOn[i].innerText || o_ResolvedOn[i].text);
                    var AlertDuration = setundefined(o_AlertDuration[i].textContent || o_AlertDuration[i].innerText || o_AlertDuration[i].text);
                    
                    row=document.createElement('tr');
                    AddCell(row,DeviceSubType,'DeviceList_leftBox',"","","","","40px","");
                    AddCell(row,AlertDescription,'DeviceList_leftBox',"","","","","40px","");
                    AddCell(row,AlertTime,'siteOverview_cell',"","","","","40px","");
                    if(AlertDuration == "")
                        AddCell(row,"Open",'siteOverview_cell',"","","","","40px","");
                    else
                        AddCell(row,ResolvedOn,'siteOverview_cell',"","","","","40px","");
                    AddCell(row,AlertDuration,'siteOverview_cell',"","","","","40px","");
                    sTbl.appendChild(row);
                }
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_TableView").innerHTML = "Total Records : 0";
                document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_TableView").innerHTML = " of 0";
                
                row=document.createElement('tr');
                AddCell(row,"No Records found!!!",'siteOverview_cell_Full',"5","","","","40px","");
                sTbl.appendChild(row);
                document.getElementById("divLoading_TableView").style.display = "none";
            }

            if (nRootLength > 0) {
                doPOEnableButton((o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text), document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_TableView").value);
            }
            document.getElementById("divLoading_TableView").style.display = "none";
        }
    }
}

function doPOEnableButton(MaxPg, CurrPg)
{    
    if(parseInt(MaxPg) == 1)
    {
        document.getElementById("btnPrev_TableView").disabled = true;
        document.getElementById("btnNext_TableView").disabled = true;
    }
    else
    {
        document.getElementById("btnNext_TableView").disabled = false;
        document.getElementById("btnPrev_TableView").disabled = false;
    }
    
    if(parseInt(CurrPg) == 0)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_TableView").value = 1;
    }
    
    if(parseInt(CurrPg) <= 1)
    {
        document.getElementById("btnPrev_TableView").disabled = true;
    }
    
    if(parseInt(CurrPg) >= parseInt(MaxPg))
    {
        document.getElementById("btnNext_TableView").disabled = true;
    }
}

//*******************************************************************
//	Function Name	:	HomePageAnnouncements
//	Input			:	none
//	Description		:	ajax call HomePageAnnouncements
//*******************************************************************
var g_AnnHomeObj;

function HomePageAnnouncements()
{
    g_AnnHomeObj = CreateXMLObj();
    
    if(g_AnnHomeObj!=null)
    {
        g_AnnHomeObj.onreadystatechange = ajaxHomePageAnnouncements;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetAnnouncements&IsLive=1";
      
        if(GetBrowserType()=="isIE")
        {	
            g_AnnHomeObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_AnnHomeObj.open("GET",DbConnectorPath, true);
        }
        g_AnnHomeObj.send(null);         
    }
    return false;
}

//*******************************************************************
//	Function Name	:	ajaxHomePageAnnouncements
//	Input			:	none
//	Description		:	Load Announcements from ajax Response
//*******************************************************************
function ajaxHomePageAnnouncements() {
    if (g_AnnHomeObj.readyState == 4) {
        if (g_AnnHomeObj.status == 200) {

            var nRootLength;
            var sMarqueeTag = "";

            var dsRoot = g_AnnHomeObj.responseXML.documentElement;

            var o_Message = dsRoot.getElementsByTagName('Message');
            var o_HTMLMessage = dsRoot.getElementsByTagName('HTMLMessage');
            var o_IsActive = dsRoot.getElementsByTagName('IsActive');
            var o_AnnouncementId = dsRoot.getElementsByTagName('AnnouncementId');
            var o_ShowIn = dsRoot.getElementsByTagName('ShowIn');

            nRootLength = o_Message.length;

            if (nRootLength > 0) {

                $('#marqueeAnnouncements').css("display", "block");

                for (var i = 0; i < nRootLength; i++) {
                    var Message = getTagNameValue(o_Message[i]);
                    var HTMLMessage = getTagNameValue(o_HTMLMessage[i]);
                    var IsActive = getTagNameValue(o_IsActive[i]);
                    var AnnouncementId = getTagNameValue(o_AnnouncementId[i]);
                    var ShowIn = getTagNameValue(o_ShowIn[i]);

                    if (ShowIn == 1 || ShowIn == 2) {//check show in 2X
                        if (IsActive == "True") {
                            Message += "<a style='padding-left: 10px;' href='LongDescription.aspx?AnnouncementId=" + AnnouncementId + "&pageref=0'>View more details >></a>";
                        }
                        if (sMarqueeTag == "")
                            sMarqueeTag = "<span style='padding-right: 50px;'>" + Message + "</span>";
                        else
                            sMarqueeTag = sMarqueeTag + "<br/><span style='padding-right: 50px;'>" + Message + "</span>";
                    }

                    $('#announcementtag').html(sMarqueeTag);
                }
            }            
        }
    }
}

function UpdatethesholdTime(sTime) {

    document.getElementById("divLoading_Status").style.display = "";
    
    g_UAObj = CreateXMLObj();

    if (g_UAObj != null) {

        g_UAObj.onreadystatechange = ajaxThresHoldTime;

        DbConnectorPath = "AjaxConnector.aspx?cmd=AddThresHoldTime&TTime=" + sTime;
       
        if (GetBrowserType() == "isIE") {
            g_UAObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_UAObj.open("GET", DbConnectorPath, true);
        }
        g_UAObj.send(null);
    }
    return false;
}

function ajaxThresHoldTime() {
    if (g_UAObj.readyState == 4) {
        if (g_UAObj.status == 200) {
            var dsRoot = g_UAObj.responseXML.documentElement;

            AjaxMsgReceiver(dsRoot);
            document.getElementById("divLoading_Status").style.display = "none";

            var o_Time = dsRoot.getElementsByTagName('Time')
            var Time = getTagNameValue(o_Time[0]);
            
            document.getElementById("txt_time").value = Time;                               
        }
    }
}