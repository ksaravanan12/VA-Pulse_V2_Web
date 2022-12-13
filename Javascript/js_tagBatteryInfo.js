// JScript File
var AT_SortOrder='';
var AT_SortColumn='';
var AT_SortOrder='';
var AT_SortImg='';

AT_SortColumn = "TagId";
AT_SortOrder = "desc";
AT_SortImg = "<image src='Images/downarrow.png' valign='middle' />";        

var nDeviceType,ntypeId,nSiteId;

//TAG BATTERY LIST
function LoadTagBatteryList(siteid,devicetype,typeid,campusid,buildid,floorid,unitid,curpage,deviceid)
{ 
    nDeviceType=devicetype;
    ntypeId=typeid;
    nSiteId=siteid;
          
    LoadBatteryTagInformation(siteid,devicetype,typeid,setundefined(campusid),setundefined(buildid),setundefined(floorid),setundefined(unitid),curpage,deviceid)        
    
}

var g_Obj;

function LoadBatteryTagInformation(siteid,devicetype,typeid,campusid,buildid,floorid,unitid,curpage,deviceid)
{   
    
    g_Obj = CreateTagXMLObj();     
       
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxTagBatteryList;        
            
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetBatteryList&sid="+ siteid +"&DeviceType="+ devicetype +"&typId="+ typeid +"&campusid="+campusid+"&buildid="+buildid+"&floorid="+floorid+"&unitid="+unitid+"&curpage="+curpage+"&sortcolumn="+AT_SortColumn+"&sortorder="+AT_SortOrder+"&DeviceId="+deviceid;
      
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
function ajaxTagBatteryList()
{
    if(g_Obj.readyState==4)
    {
        if(g_Obj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Obj.responseXML.documentElement);
            g_TagRoot = g_Obj.responseXML.documentElement;    
            MakeTagBatteryList();
        }
    }
}

function MakeTagBatteryList()
{
    var sTbl,sTblLen;   
    var Settings="";   
   
    sTbl=document.getElementById('tblBatterySummary');
   
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);    
  
    $('#tblBatterySummary').css('width','100%');
   
    var o_SiteName=g_TagRoot.getElementsByTagName('Sitename');
    var o_TotalPage=g_TagRoot.getElementsByTagName('TotalPage');
    var o_TotalCount=g_TagRoot.getElementsByTagName('TotalCount');
    var o_Location=g_TagRoot.getElementsByTagName('Location');    
    var o_TagTypeName=g_TagRoot.getElementsByTagName('TagTypeName');
    var o_TagId=g_TagRoot.getElementsByTagName('TagId');    
    var o_Estimated=g_TagRoot.getElementsByTagName('Estimated');   
    var o_UpdateRate=g_TagRoot.getElementsByTagName('UpdateRate');
    var o_ActivityLevel=g_TagRoot.getElementsByTagName('ActivityLevel'); 
       
    nRootLength=o_SiteName.length;        
    
    if(nRootLength == 0)
    {
        //totalcount
        var ttcnt_lable=document.getElementById("ctl00_ContentPlaceHolder1_lblBatteryTotalcnt")
        ttcnt_lable.innerHTML="Total Pages: " + 0;
       
        //Totalpage
        var ttPage_lable=document.getElementById("ctl00_ContentPlaceHolder1_lblBatteryTotalpage")
        ttPage_lable.innerHTML=" of " + 0;
        
        var MaxPageCnt=0;
        doTagBatteryEnableButton(0);
       
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
    doTagBatteryEnableButton(MaxPageCnt);      
               
   if(nRootLength >0)
   {      
       
         row=document.createElement('tr');             
           
         AddCellForSorting(row,"Filtered Location",'ATDeviceList_Header',"","","center","700px","40px","","CampusName,BuildingName,UnitName,RoomName",AT_SortColumn,AT_SortImg,enumSortingArr.TagBatteryView);            
         AddCellForSorting(row,"Details",'ATDeviceList_Header',"","","center","120px","40px","","TagName",AT_SortColumn,AT_SortImg,enumSortingArr.TagBatteryView);
         AddCellForSorting(row,"Tag Id",'ATDeviceList_Header',"","","center"," 80px","40px","","TagId",AT_SortColumn,AT_SortImg,enumSortingArr.TagBatteryView);       
         AddCellForSorting(row,"Remaining (approx) Battery Life",'ATDeviceList_Header',"","","center","150px","40px","","Yrs",AT_SortColumn,AT_SortImg,enumSortingArr.TagBatteryView);          
         AddCellForSorting(row,"Settings (Update Rate,Activity Level)",'ATDeviceList_Header',"","","center","150px","40px","","IRProfile,ActivityLevel",AT_SortColumn,AT_SortImg,enumSortingArr.TagBatteryView);    
         //AddCell(row,"",'siteOverview_Topright_Box',"","","center","100px","40px","","",AT_SortColumn,AT_SortImg,enumSortingArr.TagBatteryView);             

         sTbl.appendChild(row);
 
        for(var i=0; i<nRootLength; i++)
        {         
           
            var Location=(o_Location[i].textContent || o_Location[i].innerText || o_Location[i].text);
            var TagTypeName=(o_TagTypeName[i].textContent || o_TagTypeName[i].innerText || o_TagTypeName[i].text);
            var TagId=(o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);        
            var Estimated=(o_Estimated[i].textContent || o_Estimated[i].innerText || o_Estimated[i].text);
            var UpdateRate=(o_UpdateRate[i].textContent || o_UpdateRate[i].innerText || o_UpdateRate[i].text);                                     
            var ActivityLevel=(o_ActivityLevel[i].textContent || o_ActivityLevel[i].innerText || o_ActivityLevel[i].text);  
            
            row=document.createElement('tr');    
            
            if (Estimated==0.5)
            {
              Estimated="6 Months";
            }
            else if (Estimated==1)
            {
              Estimated=Estimated + ' ' + "Year";
            }
            else if (Estimated=="N/A")
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
            
            Settings=UpdateRate + ',' + ActivityLevel;
            
            AddCell(row,Location,'ATDeviceList_Row',"","","center","400px","40px","");
            AddCell(row,TagTypeName,'ATDeviceList_Row',"","","center","300px","40px","");
            AddCell(row,TagId,'ATDeviceList_Row',"","","center","300px","40px","");
            AddCell(row,Estimated,'ATDeviceList_Row',"","","center","300px","40px","");
            AddCell(row,Settings,'ATDeviceList_Row',"","","center","300px","40px",""); 
                     
            //var maping="<a onmouseover ='TabOver(this);' onmouseout='TabOut(this);' class='cell_text_green' onclick=\"OpenPDDialog('" + TagId + "');\" style='cursor: pointer;'><img src='images/chart.PNG'/></a>"
            //AddCell(row,maping,'ATDeviceList_Row',"","","center","300px","40px","");    
            sTbl.appendChild(row);
           
        }
    }  
     document.getElementById("divLoading_BatteryOverview").style.display = "none";  
       
}


function doTagBatteryEnableButton(MaxPageCnt){
    
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

function sortTagBatteryList(sortCol)
{
    var curpage=document.getElementById("ctl00_ContentPlaceHolder1_txtBatteryPageNo")
    var currentpage=curpage.value;
   
    document.getElementById("divLoading_BatteryOverview").style.display = "";
    
    if(AT_SortColumn != sortCol)
    {
        AT_SortOrder = "desc";
        AT_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(AT_SortOrder == "desc")
        {
           
            AT_SortOrder = "asc";
            AT_SortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(AT_SortOrder == "asc")
        {
            AT_SortOrder = "desc";
            AT_SortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }
    if(sortCol != "")
    {
        AT_SortColumn = sortCol;
    } 
    LoadTagBatteryList($('#ctl00_headerBanner_drpsitelist').val(),nDeviceType,ntypeId,$('#selCampus').val(), $('#selBuilding').val(), $('#selFloor').val(), $('#selUnits').val(),currentpage,"",AT_SortColumn,AT_SortOrder);
}

function doLoadTagExcelBatteryList(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{ 
    LoadBatteryTagExcelInformation(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId),curpage,deviceid);
}

function LoadBatteryTagExcelInformation(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{
    if(g_Obj == null)
    {
        g_Obj = CreateTagXMLObj();
    }      
    
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxDownLoadBatteryTaginforamtion;        
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadExcelBatteryList&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId +"&curpage=0&sortcolumn="+AT_SortColumn+"&sortorder="+AT_SortOrder+"&DeviceId="+deviceid;

      
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
function ajaxDownLoadBatteryTaginforamtion()
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

function doLoadTagExcelBatteryList_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{ 
    LoadTagExcelBatteryList_IE(SiteId,DeviceType,setundefined(typeId),setundefined(CampusId),setundefined(BuildId),setundefined(FloorId),setundefined(UnitId),curpage,deviceid);
}
function LoadTagExcelBatteryList_IE(SiteId,DeviceType,typeId,CampusId,BuildId,FloorId,UnitId,curpage,deviceid)
{
   location.href = "AjaxConnector.aspx?cmd=LoadExcelBatteryListFor_IE&sid=" + SiteId + "&DeviceType=" + DeviceType +"&typId="+typeId + "&campusid=" + CampusId + "&buildid=" + BuildId + "&floorid=" + FloorId +"&unitid=" + UnitId +"&curpage=0&sortcolumn="+AT_SortColumn+"&sortorder="+AT_SortOrder+"&DeviceId="+deviceid;
}

//Open Purchase Details Dialog
function OpenPDDialog(TagId)
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
    
    LoadBatteryTagDetailsView(TagId);

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
function LoadBatteryTagDetailsView(TagId)
{
    document.getElementById("divLoading_PD").style.display = '';
    
    g_PDObj = CreateXMLObj();
    
    if(g_PDObj!=null)
    {
        g_PDObj.onreadystatechange = ajaxLoadTagBatteryDetailsView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetBatteryList&sid="+ nSiteId +"&DeviceType="+ nDeviceType +"&typId="+ ntypeId +"&campusid="+setundefined($('#selCampus').val())+"&buildid="+setundefined($('#selBuilding').val())+"&floorid="+setundefined($('#selFloor').val())+"&unitid="+setundefined($('#selUnits').val())+"&curpage=0&sortcolumn="+AT_SortColumn+"&sortorder="+AT_SortOrder+"&DeviceId="+TagId;
      
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

//Ajax Readystate Change for PD
function ajaxLoadTagBatteryDetailsView()
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
            var o_TagTypeName=dsRoot.getElementsByTagName('TagTypeName');
            var o_TagId=dsRoot.getElementsByTagName('TagId');    
            var o_Estimated=dsRoot.getElementsByTagName('Estimated');           
            var o_UpdateRate=dsRoot.getElementsByTagName('UpdateRate');
            var o_ActivityLevel=dsRoot.getElementsByTagName('ActivityLevel');             
           
            nRootLength=o_SiteName.length;
            
            if(nRootLength > 0)
            {
                var UpdateRate = (o_UpdateRate[0].textContent || o_UpdateRate[0].innerText || o_UpdateRate[0].text);
                var ActivityLevel = (o_ActivityLevel[0].textContent || o_ActivityLevel[0].innerText || o_ActivityLevel[0].text);
                
                UpdateRate=setundefined(UpdateRate);                     
                ActivityLevel=setundefined(ActivityLevel);
                
                row=document.createElement('tr');
                AddCell(row,"Update Rate : " + UpdateRate,'clsPDLabel',"","","","200px","40px");
                sTbl.appendChild(row);                     
                
                row=document.createElement('tr');
                AddCell(row,"Activity Level : " + ActivityLevel,'clsPDLabel',"","","","200px","40px");
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




