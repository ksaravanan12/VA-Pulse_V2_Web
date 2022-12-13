// JScript File

var MonitorSortOrder='';
var MonitorSortColumn='';
var MonitorSortImg='';

MonitorSortOrder = "desc";
MonitorSortColumn = "MonitorId";

MonitorSortImg = "<image src='Images/downarrow.png' valign='middle' />";        

var nDeviceType,ntypeId,nSiteId;

function LoadInfraBatteryList(siteid,devicetype,typeid,campusid,buildid,floorid,unitid,curpage)
{   
   nDeviceType=devicetype;
   ntypeId=typeid;
   nSiteId=siteid;
           
   LoadBatteryInfraListInformation(siteid,devicetype,typeid,setundefined(campusid),setundefined(buildid),setundefined(floorid),setundefined(unitid),curpage)        
   
}
var g_Obj;

function LoadBatteryInfraListInformation(siteid,devicetype,typeid,campusid,buildid,floorid,unitid,curpage)
{   
    
    g_Obj = CreateTagXMLObj();     
    var unitids=setundefined(unitid);     
      
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxBatteryInfraList;        
            
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetBatteryList&sid="+ siteid +"&DeviceType="+ devicetype +"&typId="+ typeid +"&campusid="+campusid+"&buildid="+buildid+"&floorid="+floorid+"&unitid="+unitids+"&curpage="+curpage+"&sortcolumn="+MonitorSortColumn+"&sortorder="+MonitorSortOrder;
      
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

var g_TagRoot;
function ajaxBatteryInfraList()
{
    if(g_Obj.readyState==4)
    {
        if(g_Obj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Obj.responseXML.documentElement);
            g_TagRoot = g_Obj.responseXML.documentElement;    
            MakeInfraBatteryList();
        }
    }
}

function MakeInfraBatteryList()
{
    var sTbl,sTblLen;   
   
    sTbl=document.getElementById('tblBatterySummary');
   
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);    
  
    $('#tblBatterySummary').css('width','100%');
   
    var o_SiteName=g_TagRoot.getElementsByTagName('Sitename');
    var o_TotalPage=g_TagRoot.getElementsByTagName('TotalPage');
    var o_TotalCount=g_TagRoot.getElementsByTagName('TotalCount');
    var o_Location=g_TagRoot.getElementsByTagName('Location');    
    var o_MonitorTypeName=g_TagRoot.getElementsByTagName('MonitorTypeName');
    var o_MonitorId=g_TagRoot.getElementsByTagName('MonitorId');    
    var o_Estimated=g_TagRoot.getElementsByTagName('Estimated');    
    var o_UpdateRate=g_TagRoot.getElementsByTagName('UpdateRate');
    var o_PowerLevel=g_TagRoot.getElementsByTagName('PowerLevel');
    var o_NoiseLevel=g_TagRoot.getElementsByTagName('NoiseLevel'); 
       
    nRootLength=o_SiteName.length;        
    
    if(nRootLength == 0)
    {
        //totalcount
        var ttcnt_lable=document.getElementById("ctl00_ContentPlaceHolder1_lblBatteryTotalcnt")
        ttcnt_lable.innerHTML="Total Records: " + 0;
       
        //Totalpage
        var ttPage_lable=document.getElementById("ctl00_ContentPlaceHolder1_lblBatteryTotalpage")
        ttPage_lable.innerHTML=" of " + 0;
        
        var MaxPageCnt=0;
        doInfraBatteryEnableButton(0);
       
        row=document.createElement('tr');
        AddCell(row,"No Record Found.", 'siteOverview_cell_Full',6,"","center","700px","40px","");
        sTbl.appendChild(row);
        document.getElementById("divLoading_BatteryOverview").style.display = "none";    
              
        return;
    }
    document.getElementById('divPaginationRow').style.display = "";          
    
    //totalcount
    var ttcnt_lable=document.getElementById("ctl00_ContentPlaceHolder1_lblBatteryTotalcnt")
    ttcnt_lable.innerHTML="Total Records: " +(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
       
    //Totalpage
    var ttPage_lable=document.getElementById("ctl00_ContentPlaceHolder1_lblBatteryTotalpage")
    ttPage_lable.innerHTML=" of " +(o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
        
    var MaxPageCnt=(o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
    doInfraBatteryEnableButton(MaxPageCnt);      
               
   if(nRootLength >0)
   {      
       
         row=document.createElement('tr');            
         
         AddCellForSorting(row,"Filtered Location",'ATDeviceList_Header',"","","center","700px","40px","","CampusName,BuildingName,UnitName,RoomName",MonitorSortColumn,MonitorSortImg,enumSortingArr.MonitorBatteryView);            
         AddCellForSorting(row,"Details",'ATDeviceList_Header',"","","center","120px","40px","","",MonitorSortColumn,MonitorSortImg,enumSortingArr.MonitorBatteryView);
         AddCellForSorting(row,"Monitor Id",'ATDeviceList_Header',"","","center"," 80px","40px","","MonitorId",MonitorSortColumn,MonitorSortImg,enumSortingArr.MonitorBatteryView);       
         AddCellForSorting(row,"Remaining (approx) Battery Life",'ATDeviceList_Header',"","","center","150px","40px","","Yrs",MonitorSortColumn,MonitorSortImg,enumSortingArr.MonitorBatteryView);          
         AddCellForSorting(row,"Settings (Power Level,Noise Level,Update Rate)",'ATDeviceList_Header',"","","center","350px","40px","","PowerLevel,NoiseLevel,IRProfile",MonitorSortColumn,MonitorSortImg,enumSortingArr.MonitorBatteryView);    
         //AddCell(row,"",'siteOverview_Topright_Box',"","","center","100px","40px","","",MonitorSortColumn,MonitorSortImg,enumSortingArr.MonitorBatteryView);             
        
         sTbl.appendChild(row);
 
        for(var i=0; i<nRootLength; i++)
        {         
           
            var Location=(o_Location[i].textContent || o_Location[i].innerText || o_Location[i].text);
            var MonitorTypeName=(o_MonitorTypeName[i].textContent || o_MonitorTypeName[i].innerText || o_MonitorTypeName[i].text);                  
            var MonitorId=(o_MonitorId[i].textContent || o_MonitorId[i].innerText || o_MonitorId[i].text);        
            var Estimated=(o_Estimated[i].textContent || o_Estimated[i].innerText || o_Estimated[i].text); 
            var PowerLevel=(o_PowerLevel[i].textContent || o_PowerLevel[i].innerText || o_PowerLevel[i].text);                           
            var NoiseLevel=(o_NoiseLevel[i].textContent || o_NoiseLevel[i].innerText || o_NoiseLevel[i].text);    
            var UpdateRate=(o_UpdateRate[i].textContent || o_UpdateRate[i].innerText || o_UpdateRate[i].text);    
            
            if (Estimated==0.5)
            {
              Estimated="6 Months";
            }
            else if(Estimated==1)
            {
              Estimated=Estimated + ' ' + "Year";
            }
            else if(Estimated=="N/A")
            {
              Estimated=Estimated;
            }
            else if (Estimated==5.5)
            {
              Estimated="5Yrs +";
            }
            else
            {
              Estimated=Estimated + ' ' + "Years";
            }
            
            Settings=PowerLevel + ',' + NoiseLevel + ',' + UpdateRate ;
            
            row=document.createElement('tr');    
            
            AddCell(row,Location,'ATDeviceList_Row',"","","center","200px","40px","");  
            AddCell(row,MonitorTypeName,'ATDeviceList_Row',"","","center","300px","40px","");                 
            AddCell(row,MonitorId,'ATDeviceList_Row',"","","center","300px","40px","");
            AddCell(row,Estimated,'ATDeviceList_Row',"","","center","300px","40px","");
            AddCell(row,Settings,'ATDeviceList_Row',"","","center","200px","40px",""); 
            
            //var maping="<a onmouseover ='TabOver(this);' onmouseout='TabOut(this);' class='cell_text_green' onclick=\"OpenInfraPDDialog('" + MonitorId + "');\" style='cursor: pointer;'><img src='images/chart.PNG'/></a>"
            
            //AddCell(row,maping,'ATDeviceList_Row',"","","center","300px","40px","");              
                    
            sTbl.appendChild(row);
           
        }
    }  
     document.getElementById("divLoading_BatteryOverview").style.display = "none";  
       
}

function doInfraBatteryEnableButton(MaxPageCnt){
    
    var curnPage=document.getElementById("ctl00_ContentPlaceHolder1_txtBatteryPageNo")
    var curPage=curnPage.value;      
                  
    if(MaxPageCnt=="1" || Number(MaxPageCnt)==1){
        document.getElementById("ctl00_ContentPlaceHolder1_txtBatteryPageNo").value="1";
        document.getElementById("btnBatteryNext").style.display="none";
        document.getElementById("btnBatteryPrev").style.display="none";
    }   
    else
    {
        document.getElementById("btnBatteryNext").style.display="";
        document.getElementById("btnBatteryPrev").style.display="";
    }
    
    
    if(Number(curPage) <=1){
        document.getElementById("ctl00_ContentPlaceHolder1_txtBatteryPageNo").value="1"
        document.getElementById("btnBatteryPrev").style.display="none";
    }
    
    if(Number(curPage) >=Number(MaxPageCnt)){
        document.getElementById("btnBatteryNext").style.display="none";
    }
    
    if (MaxPageCnt==0)
    {
      document.getElementById("ctl00_ContentPlaceHolder1_txtBatteryPageNo").value="0";
    } 
    
}

//Open Purchase Details Dialog
function OpenInfraPDDialog(MonitorId)
{
    g_DialogView = "dialog-BatterySummaryListView";
 
    var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblBatteryListDialog');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblBatteryListDialog');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
    
    var winWidth = $(window).width() - 700;
    var winHeight = $(window).height() - 500;
    
    LoadInfraBatteryDetailsView(MonitorId);

    //Open Dialog
    $( "#dialog-BatterySummaryListView" ).dialog({
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
        }
    });
}

//Ajax Call for PD
var g_PDObj;
function LoadInfraBatteryDetailsView(MonitorId)
{
    document.getElementById("divLoading_PD").style.display = '';
    
    g_PDObj = CreateXMLObj();
    
    if(g_PDObj!=null)
    {
        g_PDObj.onreadystatechange = ajaxInfraLoadBatteryDetailsView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetBatteryList&sid="+ nSiteId +"&DeviceType="+ nDeviceType +"&typId="+ ntypeId +"&campusid="+setundefined($('#selCampus').val())+"&buildid="+setundefined($('#selBuilding').val())+"&floorid="+setundefined($('#selFloor').val())+"&unitid="+setundefined($('#selUnits').val())+"&curpage=0&sorColumnname="+MonitorSortColumn+"&SorOrder="+MonitorSortOrder+"&DeviceId="+MonitorId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_PDObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_PDObj.open("GET",DbConnectorPath, true);
        }
        g_PDObj.send(null);         
    }
    return false;
}

function sortInfraBatteryList(sortCol)
{
    var curpage=document.getElementById("ctl00_ContentPlaceHolder1_txtBatteryPageNo")
    var currentpage=curpage.value;   
   
    document.getElementById("divLoading_BatteryOverview").style.display = "";
   
    if(MonitorSortColumn != sortCol)
    {
        MonitorSortOrder = "desc";
        MonitorSortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(MonitorSortOrder == "desc")
        {
           
            MonitorSortOrder = "asc";
            MonitorSortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(MonitorSortOrder == "asc")
        {
            MonitorSortOrder = "desc";
            MonitorSortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }
    if(sortCol != "")
    {
        MonitorSortColumn = sortCol;
    } 
    
    LoadInfraBatteryList($('#ctl00_headerBanner_drpsitelist').val(),nDeviceType,ntypeId,$('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(),currentpage,"",MonitorSortColumn,MonitorSortOrder);
}

//Ajax Readystate Change for PD
function ajaxInfraLoadBatteryDetailsView()
{
    if(g_PDObj.readyState==4)
    {
        if(g_PDObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_PDObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblBatteryListDialog');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblBatteryListDialog');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_PDObj.responseXML.documentElement;
            
            var o_SiteName=dsRoot.getElementsByTagName('Sitename');
            var o_TotalPage=dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount=dsRoot.getElementsByTagName('TotalCount');
            var o_Location=dsRoot.getElementsByTagName('Location');    
            var o_MonitorTypeName=dsRoot.getElementsByTagName('MonitorTypeName');
            var o_MonitorId=dsRoot.getElementsByTagName('MonitorId');    
            var o_Estimated=dsRoot.getElementsByTagName('Estimated');
            var o_Settings=dsRoot.getElementsByTagName('Settings');
            var o_UpdateRate=dsRoot.getElementsByTagName('UpdateRate');
            var o_PowerLevel=dsRoot.getElementsByTagName('PowerLevel');
            var o_NoiseLevel=dsRoot.getElementsByTagName('NoiseLevel');             
           
            nRootLength=o_SiteName.length;
            
            if(nRootLength > 0)
            {
                var UpdateRate = (o_UpdateRate[0].textContent || o_UpdateRate[0].innerText || o_UpdateRate[0].text);
                var PowerLevel = (o_PowerLevel[0].textContent || o_PowerLevel[0].innerText || o_PowerLevel[0].text);
                var NoiseLevel = (o_NoiseLevel[0].textContent || o_NoiseLevel[0].innerText || o_NoiseLevel[0].text);
                
                UpdateRate=setundefined(UpdateRate);
                PowerLevel=setundefined(PowerLevel);              
                NoiseLevel=setundefined(NoiseLevel);
                
                row=document.createElement('tr');
                AddCell(row,"Update Rate : " + UpdateRate,'clsPDLabel',"","","","200px","40px");
                sTbl.appendChild(row);
                
                row=document.createElement('tr');
                AddCell(row,"Power Level : " + PowerLevel,'clsPDLabel',"","","","200px","40px");
                sTbl.appendChild(row);
                
                row=document.createElement('tr');
                AddCell(row,"Noise Level : " + NoiseLevel,'clsPDLabel',"","","","200px","40px");
                sTbl.appendChild(row);                
             }       
                      
            else
            {
                row=document.createElement('tr');
                AddCell(row,"No Records found!!!",'siteOverview_cell_Full',"5","","","","40px","");
                sTbl.appendChild(row);
            }
            
            document.getElementById("divLoading_PD").style.display = 'none';
        }
    }
}
function doLoadInfraExcelBatteryList_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{ 
    ajaxInfraExcelBatteryList_IE(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId),curpage,deviceid);
}
function ajaxInfraExcelBatteryList_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{
   location.href = "AjaxConnector.aspx?cmd=LoadExcelBatteryListFor_IE&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId +"&curpage=0&sortcolumn="+AT_SortColumn+"&sortorder="+AT_SortOrder+"&DeviceId="+deviceid;
}

function doLoadInfraExcelBatteryList(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{ 
    ajaxLoadInfraExcelBatteryList(SiteId,DeviceType,typeId,setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId),curpage,deviceid)        
}

function ajaxLoadInfraExcelBatteryList(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{
    if(g_Obj == null)
    {
        g_Obj = CreateTagXMLObj();
    }      
    
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxDownLoadInfraBatteryinforamtion;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadExcelBatteryList&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId +"&curpage=0&sortcolumn="+MonitorSortColumn+"&sortorder="+MonitorSortOrder+"&DeviceId="+deviceid;
      
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

function ajaxDownLoadInfraBatteryinforamtion()
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