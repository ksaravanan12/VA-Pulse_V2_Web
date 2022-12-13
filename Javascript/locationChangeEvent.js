//Display view by viewType
var g_LocationLiveLoaded = false;
var g_LocationHistoryLoaded = false;
var LastFetchedDateTime = "";
var isSearch = false;
var isSearchNum = 0;
function DisplayLocationChange(viewType)
{
    if(viewType === 1){
    
        $("#divLocationLive").hide();
        $("#divLocationHistory").show();

    } else if(viewType === 2){
    
        $("#divLocationHistory").hide();
        $("#divLocationLive").show();
    }
}
//*********************************************************
    //	Function Name	:	LocationChageEventLive
    //	Input			:	none
    //	Description		:	Load Location Chage Event Live Data from ajax Response
//*********************************************************

var g_LocationEvent;
function LocationChageEventLive(siteId,Tagids,CurrentRoom,LastRoom,PageSize,CurPage,isLiveData,LastFetchedDateTime,LocationEventSort)
{
    g_LocationEvent = CreateDeviceXMLObj();
    $("#divLoading").show();         
    if(g_LocationEvent!=null)
    {
        g_LocationEvent.onreadystatechange = ajaxLocationChageEventLive;
        DbConnectorPath = "AjaxConnector.aspx?cmd=LocationChageEvent&sid=" + siteId+"&Tagids=" + Tagids + "&CurrentRoom=" + CurrentRoom + "&LastRoom=" + LastRoom + "&PageSize=" + PageSize + "&CurPage=" + CurPage + "&isLiveData=" + isLiveData + "&LastFetchedDateTime=" + LastFetchedDateTime +"&LocationEventSort=" + LocationEventSort;
      
        if(GetBrowserType()=="isIE")
        {	
            g_LocationEvent.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_LocationEvent.open("GET",DbConnectorPath, true);
        }
        g_LocationEvent.send(null);         
    }
    return false;   
}
    
//*******************************************************************
    //	Function Name	:	ajaxLocationChageEventLive
    //	Input			:	none
    //	Description		:	Load Location Change Event Live Data from ajax Response
//*******************************************************************

function ajaxLocationChageEventLive()
{
     if(g_LocationEvent.readyState==4)
     {
        if(g_LocationEvent.status==200)
        {
            var dsRoot = g_LocationEvent.responseXML.documentElement;
            var o_TagId=dsRoot.getElementsByTagName('TagId');
            var nRootLength=o_TagId.length;
               
            if(isSearch == true)
            {
                if(isSearchNum == "0")
                {
                    isSearchNum = 1;
                    DisplayDataForLivePage(dsRoot);
                }
            }           
            else if(LastFetchedDateTime != "" && nRootLength == "0")
            {
                $("#divLoading").hide(); 
            }
            else
            {
              DisplayDataForLivePage(dsRoot);
            }
         
       }
    }
    
}

function DisplayDataForLivePage(dsRootTemp)
{
    var sTbl,sTblLen;
            
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblLocationChangeLive');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblLocationChangeLive');
    }
    sTblLen = sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
    
    var dsRoot = dsRootTemp;
    var o_TagId=dsRoot.getElementsByTagName('TagId');
    var o_CurrentRoom=dsRoot.getElementsByTagName('CurrentRoom');
    var o_EnteredOn=dsRoot.getElementsByTagName('EnteredOn');
    var o_LastRoom=dsRoot.getElementsByTagName('LastRoom');
    var o_LeftOn=dsRoot.getElementsByTagName('LeftOn');
    var o_TimeSpend=dsRoot.getElementsByTagName('TimeSpend');
    var o_TotalPage=dsRoot.getElementsByTagName('TotalPage');
    var o_TotalCount=dsRoot.getElementsByTagName('TotalCount');
    var o_LastUpdatedOn=dsRoot.getElementsByTagName('LastUpdatedOn');
    var nRootLength=o_TagId.length;
    
    if(g_LiveSortColumn == "")
    {
        g_LiveSort = "TagId desc";
        g_LiveSortColumn = "TagId";
        g_LiveSortOrder = " desc";
        g_LiveSortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    //Header
    row=document.createElement('tr');
    
    AddCellForSorting(row,"Tag Id",'siteOverview_TopLeft_Box',"","","center","80px","40px","","TagId",g_LiveSortColumn,g_LiveSortImg,enumSortingArr.LocationLive);
    AddCellForSorting(row,"Current Room",'siteOverview_Box',"","","center","110px","40px","","CurrentRoom",g_LiveSortColumn,g_LiveSortImg,enumSortingArr.LocationLive);
    AddCellForSorting(row,"EnteredOn",'siteOverview_Box',"","","center","110px","40px","","EnteredOn",g_LiveSortColumn,g_LiveSortImg,enumSortingArr.LocationLive);
    AddCellForSorting(row,"LastRoom",'siteOverview_Box',"","","center","90px","40px","","LastRoom",g_LiveSortColumn,g_LiveSortImg,enumSortingArr.LocationLive);
    AddCellForSorting(row,"LeftOn",'siteOverview_Box',"","","center","110px","40px","","LeftOn",g_LiveSortColumn,g_LiveSortImg,enumSortingArr.LocationLive);
    AddCellForSorting(row,"TimeSpend",'siteOverview_Box',"","","center","90px","40px","","TimeSpend",g_LiveSortColumn,g_LiveSortImg,enumSortingArr.LocationLive);
    sTbl.appendChild(row);
    
    if(nRootLength > 0)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_lblCount_LocationChangeLive").innerHTML = "Total Records : " + setundefined(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
        //getMaxPages(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
        getMaxPages(setundefined(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text));
        for(var i=0; i<nRootLength; i++)
        {
            var TagId=setundefined(o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
            var CurrentRoom=setundefined(o_CurrentRoom[i].textContent || o_CurrentRoom[i].innerText || o_CurrentRoom[i].text);
            var EnteredOn=setundefined(o_EnteredOn[i].textContent || o_EnteredOn[i].innerText || o_EnteredOn[i].text);
            var LastRoom=setundefined(o_LastRoom[i].textContent || o_LastRoom[i].innerText || o_LastRoom[i].text);
            var LeftOn=setundefined(o_LeftOn[i].textContent || o_LeftOn[i].innerText || o_LeftOn[i].text);
            var TimeSpend=setundefined(o_TimeSpend[i].textContent || o_TimeSpend[i].innerText || o_TimeSpend[i].text);
            var TotalPage = setundefined(o_TotalPage[i].textContent || o_TotalPage[i].innerText || o_TotalPage[i].text);
            var TotalCount = setundefined(o_TotalCount[i].textContent || o_TotalCount[i].innerText || o_TotalCount[i].text);
            var LastUpdatedOn = setundefined(o_LastUpdatedOn[i].textContent || o_LastUpdatedOn[i].innerText || o_LastUpdatedOn[i].text);
            LastFetchedDateTime = LastUpdatedOn;
            
            row=document.createElement('tr');
            AddCell(row,TagId,'DeviceList_leftBox',"","","center","","25px","");
            AddCell(row,CurrentRoom,'siteOverview_cell',"","","center","","25px","");            
            AddCell(row,EnteredOn,'siteOverview_cell',"","","center","","25px","");
            AddCell(row,LastRoom,'siteOverview_cell',"","","center","","25px","");
            AddCell(row,LeftOn,'siteOverview_cell',"","","center","","25px","");
            AddCell(row,TimeSpend,'siteOverview_cell',"","","center","","25px","");
            sTbl.appendChild(row);
            
        }
    }
    else
    {
        row=document.createElement('tr');
        AddCell(row,"No records found...",'siteOverview_cell_Full',16,"","left","100px","40px","");
        sTbl.appendChild(row);
        getMaxPages(setundefined(0));
        document.getElementById("ctl00_ContentPlaceHolder1_lblCount_LocationChangeLive").innerHTML = "Total Records : 0";
    }
    doTblEnableButton(nTotalPg, document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value);
    $("#divLoading").hide();
             
}

//*********************************************************
    //	Function Name	:	LocationChageEventHistory
    //	Input			:	none
    //	Description		:	Load Location Chage Event History Data from ajax Response
    //  12 Arguments
//*********************************************************
var g_LocationEventHistory;

function LocationChageEventHistory(siteId,Tagids,CurrentRoom,LastRoom,PageSize,CurPage,isLiveData,LastFetchedDateTime,EnteredOnFromDate,EnteredOnToDate,LeftOnFromDate,LeftOnToDate,LocationEventSort)
{
    
    g_LocationEventHistory = CreateDeviceXMLObj();
    $("#divLoadingHistory").show();     

    if(g_LocationEventHistory!=null)
    {
        g_LocationEventHistory.onreadystatechange = ajaxLocationChageEventHistory;
        DbConnectorPath = "AjaxConnector.aspx?cmd=LocationChageEvent&sid=" + siteId+"&Tagids=" + Tagids + "&CurrentRoom=" + CurrentRoom + "&LastRoom=" + LastRoom + "&PageSize=" + PageSize + "&CurPage=" + CurPage + "&isLiveData=" + isLiveData + "&LastFetchedDateTime=" + LastFetchedDateTime + "&EnteredOnFromDate=" + EnteredOnFromDate + "&EnteredOnToDate=" + EnteredOnToDate + "&LeftOnFromDate=" + LeftOnFromDate + "&LeftOnToDate=" +LeftOnToDate + "&LocationEventSort=" + LocationEventSort;
      
        if(GetBrowserType()=="isIE")
        {	
            g_LocationEventHistory.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_LocationEventHistory.open("GET",DbConnectorPath, true);
        }
        g_LocationEventHistory.send(null);         
    }
    return false;   
}

//*******************************************************************
    //	Function Name	:	ajaxLocationChageEventHistory
    //	Input			:	none
    //	Description		:	Load Location Change Event History Data from ajax Response
//*******************************************************************

function ajaxLocationChageEventHistory()
{
     if(g_LocationEventHistory.readyState==4)
     {
        if(g_LocationEventHistory.status==200)
        {
            var sTbl,sTblLen;
            
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblLocationChangeHistory');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblLocationChangeHistory');
            }
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);            
            
            var dsRoot = g_LocationEventHistory.responseXML.documentElement;
            
            var o_TagId=dsRoot.getElementsByTagName('TagId');
            var o_CurrentRoom=dsRoot.getElementsByTagName('CurrentRoom');
            var o_EnteredOn=dsRoot.getElementsByTagName('EnteredOn');
            var o_LastRoom=dsRoot.getElementsByTagName('LastRoom');
            var o_LeftOn=dsRoot.getElementsByTagName('LeftOn');
            var o_TimeSpend=dsRoot.getElementsByTagName('TimeSpend');
            var o_TotalPage=dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount=dsRoot.getElementsByTagName('TotalCount');
            var o_LastUpdatedOn=dsRoot.getElementsByTagName('LastUpdatedOn');
            var nRootLength=o_TagId.length;
            
            if(g_HistorySortColumn == "")
            {
                g_HistorySort = "TagId desc";
                g_HistorySortColumn = "TagId";
                g_HistorySortOrder = " desc";
                g_HistorySortImg = "<image src='Images/downarrow.png' valign='middle' />";
            }        
            
             //Header
            row=document.createElement('tr');
            AddCellForSorting(row,"Tag Id",'siteOverview_TopLeft_Box',"","","center","80px","40px","","TagId",g_HistorySortColumn,g_HistorySortImg,enumSortingArr.LocationHistory);
            AddCellForSorting(row,"Current Room",'siteOverview_Box',"","","center","110px","40px","","CurrentRoom",g_HistorySortColumn,g_HistorySortImg,enumSortingArr.LocationHistory);
            AddCellForSorting(row,"EnteredOn",'siteOverview_Box',"","","center","110px","40px","","EnteredOn",g_HistorySortColumn,g_HistorySortImg,enumSortingArr.LocationHistory);
            AddCellForSorting(row,"LastRoom",'siteOverview_Box',"","","center","100px","40px","","LastRoom",g_HistorySortColumn,g_HistorySortImg,enumSortingArr.LocationHistory);
            AddCellForSorting(row,"LeftOn",'siteOverview_Box',"","","center","110px","40px","","LeftOn",g_HistorySortColumn,g_HistorySortImg,enumSortingArr.LocationHistory);
            AddCellForSorting(row,"TimeSpend",'siteOverview_Box',"","","center","90px","40px","","TimeSpend",g_HistorySortColumn,g_HistorySortImg,enumSortingArr.LocationHistory);
            sTbl.appendChild(row);
                     
            if(nRootLength > 0)
            {
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_LocationChangeHistory").innerHTML = "Total Records : " + setundefined(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
                getMaxPagesHistory(setundefined(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text));
                
                for(var i=0; i<nRootLength; i++)
                {
                    var TagId=setundefined(o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
                    var CurrentRoom=setundefined(o_CurrentRoom[i].textContent || o_CurrentRoom[i].innerText || o_CurrentRoom[i].text);
                    var EnteredOn=setundefined(o_EnteredOn[i].textContent || o_EnteredOn[i].innerText || o_EnteredOn[i].text);
                    var LastRoom=setundefined(o_LastRoom[i].textContent || o_LastRoom[i].innerText || o_LastRoom[i].text);
                    var LeftOn=setundefined(o_LeftOn[i].textContent || o_LeftOn[i].innerText || o_LeftOn[i].text);
                    var TimeSpend=setundefined(o_TimeSpend[i].textContent || o_TimeSpend[i].innerText || o_TimeSpend[i].text);
                    var TotalPage = setundefined(o_TotalPage[i].textContent || o_TotalPage[i].innerText || o_TotalPage[i].text);
                    var TotalCount = setundefined(o_TotalCount[i].textContent || o_TotalCount[i].innerText || o_TotalCount[i].text);
                    var LastUpdatedOn = setundefined(o_LastUpdatedOn[i].textContent || o_LastUpdatedOn[i].innerText || o_LastUpdatedOn[i].text);
                    
                    row=document.createElement('tr');
                    AddCell(row,TagId,'DeviceList_leftBox',"","","center","","25px","");
                    AddCell(row,CurrentRoom,'siteOverview_cell',"","","center","","25px","");            
                    AddCell(row,EnteredOn,'siteOverview_cell',"","","center","","25px","");
                    AddCell(row,LastRoom,'siteOverview_cell',"","","center","","25px","");
                    AddCell(row,LeftOn,'siteOverview_cell',"","","center","","25px","");
                    AddCell(row,TimeSpend,'siteOverview_cell',"","","center","","25px","");
                    sTbl.appendChild(row);
                    
                }
            }
            else
            {
                row=document.createElement('tr');
                AddCell(row,"No records found...",'siteOverview_cell_Full',16,"","left","100px","40px","");
                sTbl.appendChild(row);
                getMaxPagesHistory(setundefined(0));
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_LocationChangeHistory").innerHTML = "Total Records : 0";
            }
            doTblEnableButtonHistory(nTotalPgHis, document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value);
            $("#divLoadingHistory").hide();
            
        }
    }
    
}

//Get Set Max Page
var nTotalPg;
var nRowCntLive = 25;
function getMaxPages(ttlRecord)
{
    var nPgCnt = 0;
    var CurrPg;
    
    CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value;
    nPgCnt = Math.ceil(ttlRecord / nRowCntLive);
    
    document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_LocationChangeView").innerHTML = " of " + nPgCnt;
    nTotalPg = nPgCnt;
}

//Pagination for Location Change Event For live Data
function doTblEnableButton(MaxPg, CurrPg)
{
    if(parseInt(MaxPg) == 1)
    {
        document.getElementById("btnPrev_LocationChangeView").style.display = "none";
        document.getElementById("btnNext_LocationChangeView").style.display = "none";
    }
    else if(parseInt(MaxPg) == 0)
    {
        document.getElementById("btnPrev_LocationChangeView").style.display = "none";
        document.getElementById("btnNext_LocationChangeView").style.display = "none";
    }
    else
    {
        document.getElementById("btnPrev_LocationChangeView").style.display = "";
        document.getElementById("btnNext_LocationChangeView").style.display = "";
    }
    if(parseInt(CurrPg) <= 1)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value = "1";
        document.getElementById("btnPrev_LocationChangeView").style.display = "none";
    }
    
    if(parseInt(CurrPg) == parseInt(MaxPg))
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value = nTotalPg;
        //document.getElementById("btnPrev_LocationChangeView").style.display = "";
        document.getElementById("btnNext_LocationChangeView").style.display = "none";
    }
}

//Location Change Event Pagination View for live data
function LocationChangeLivePgView(pgType)
{
    var Tagids = "";
    var CurrentRoom = "";
    var LastRoom = "";
    
    
    var siteVal = ReturnSiteIdName(0);
    var CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value;
    
    Tagids = document.getElementById("txtTagId").value;
    CurrentRoom = document.getElementById("txtCurrentRoom").value;
    LastRoom = document.getElementById("txtLastRoom").value;

    if(CurrPg == "")
        CurrPg = 0;
    if(pgType == 1)
    {
        CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value;
        if(CurrPg > nTotalPg)
        {
          CurrPg =  nTotalPg;
        }
    }
    else if(pgType == 2){
        CurrPg = parseInt(CurrPg) + 1;
    } else if(pgType == 3){
        CurrPg = parseInt(CurrPg) - 1;
    }
    else if(pgType == "show")
    {
         //set empty to LastFetchedDateTime when without call automatically ( 10sec)
         //LastFetchedDateTime = "";
         CurrPg = 1;
         isSearch = true;
         isSearchNum = "0";
    }
    else if(pgType == "sort")
    {
       CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value;
    }
    
    //ClearLocationChangeLive();
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value = CurrPg;
    var id = $('.clsPageSizeCurrent').attr('id');
    var PageSize = document.getElementById(id).innerText;
    LocationChageEventLive(siteVal,Tagids,CurrentRoom,LastRoom,PageSize,CurrPg,1,"",g_LiveSort);
}

//Get Set Max Page for Location Change Event History Data
var nTotalPgHis;
var nRowCntHistory = 25;
function getMaxPagesHistory(ttlRecord)
{
    var nPgCnt = 0;
    var CurrPg;
    
    CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value;
    nPgCnt = Math.ceil(ttlRecord / nRowCntHistory);
    
    document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_LocationChangeHistoryView").innerHTML = " of " + nPgCnt;
    nTotalPgHis = nPgCnt;
}

//Pagination for Location Change Event History Data
function doTblEnableButtonHistory(MaxPg, CurrPg)
{
    if(parseInt(MaxPg) == 1)
    {
        document.getElementById("btnPrev_LocationChangeHistoryView").style.display = "none";
        document.getElementById("btnNext_LocationChangeHistoryView").style.display = "none";
    }
    else if(parseInt(MaxPg) == 0)
    {
        document.getElementById("btnPrev_LocationChangeHistoryView").style.display = "none";
        document.getElementById("btnNext_LocationChangeHistoryView").style.display = "none";
    }
    else
    {
        document.getElementById("btnPrev_LocationChangeHistoryView").style.display = "";
        document.getElementById("btnNext_LocationChangeHistoryView").style.display = "";
    }
    
    if(parseInt(CurrPg) <= 1)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value = "1";
        document.getElementById("btnPrev_LocationChangeHistoryView").style.display = "none";
    }
    
    if(parseInt(CurrPg) >= parseInt(MaxPg))
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value = nTotalPgHis;
        document.getElementById("btnNext_LocationChangeHistoryView").style.display = "none";
    }
}

//Location Change Event Pagination View for History data
function LocationChangeHistoryPgView(pgType)
{
    var Tagids = "";
    var CurrentRoom = "";
    var LastRoom = "";
    var EnterdOnFromDate = "";
    var EnterdOnToDate = "";
    var LeftOnFromDate = "";
    var LeftOnToDate = "";

    var siteVal = ReturnSiteIdName(0);
    var CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value;
    
    Tagids = document.getElementById("txtTagHistoryId").value;
    CurrentRoom = document.getElementById("txtCurrentRoomHistoryId").value;
    LastRoom = document.getElementById("txtLastRoomHistoryId").value;
    
    EnterdOnFromDate = document.getElementById("txtEnterdOnDateFrm").value;
    EnterdOnToDate = document.getElementById("txtEnterdOnDateTo").value;
    
    LeftOnFromDate = document.getElementById("LeftOnOnDateFrm").value;
    LeftOnToDate = document.getElementById("LeftOnOnDateTo").value;
  

    if(CurrPg == "")
        CurrPg = 0;
    
    if(pgType == 1)
    {   
        CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value;
        if(CurrPg > nTotalPgHis)
        {
            CurrPg = nTotalPgHis;
        }
    }
    else if(pgType == 2){
        CurrPg = parseInt(CurrPg) + 1;
    } else if(pgType == 3){
        CurrPg = parseInt(CurrPg) - 1;
    }
    else if(pgType == "show")
    {
        CurrPg = 1;
        var id = $('.clsPageSizeCurrent').attr('id');
        if(id != "tdHistory25")
        {
            $( '#'+id ).removeClass( "clsPageSizeCurrent" ).addClass( "clsPageSize" );
            $('#tdHistory25').addClass("clsPageSizeCurrent");
        }
        var PageSize = document.getElementById(id).innerText;
    }
    else if(pgType == "sort")
    {
        CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value;
    }
        
    //ClearLocationChangeHistory();
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value = CurrPg;
    var id = $('.clsPageSizeCurrent').attr('id');
    var PageSize = document.getElementById(id).innerText;
    LocationChageEventHistory(siteVal,Tagids,CurrentRoom,LastRoom,PageSize,CurrPg,0,"",EnterdOnFromDate,EnterdOnToDate,LeftOnFromDate,LeftOnToDate,g_HistorySort);
}
//used to show 25,50,100 of LiveData
var oldTdId = "";
function getPageSizeLiveData(ID,PageSize)
{
    var tdId = ID.id;
    var CurrPg = 1;
    var Tagids = "";
    var CurrentRoom = "";
    var LastRoom = "";
    
    $( '#tdLive25' ).removeClass( "clsPageSizeCurrent" ).addClass( "clsPageSize" );
    
    if(oldTdId != "")
    {
        $( '#'+oldTdId ).removeClass( "clsPageSizeCurrent" ).addClass( "clsPageSize" );;
    }
    else
    {
        $( '#tdLive25' ).removeClass( "clsPageSizeCurrent" ).addClass( "clsPageSize" );
    }
    
    $( '#'+tdId ).removeClass( "clsPageSize" ).addClass( "clsPageSizeCurrent" );
    oldTdId = tdId;
    nRowCntLive = PageSize;
    
    var SiteId = ReturnSiteIdName(0);
    //ClearLocationChangeLive();
    Tagids = document.getElementById("txtTagHistoryId").value;
    CurrentRoom = document.getElementById("txtCurrentRoomHistoryId").value;
    LastRoom = document.getElementById("txtLastRoomHistoryId").value;
    
    LocationChageEventLive(SiteId,Tagids,CurrentRoom,LastRoom,PageSize,CurrPg,1,"",g_LiveSort);
    
}

//used to show 25,50,100 of History Data
var oldTdHistoryId = "";
function getPageSizeHistoryData(ID,PageSize)
{
    var tdId = ID.id;
    var CurrPg = 1;
    var Tagids = "";
    var CurrentRoom = "";
    var LastRoom = "";
    var EnterdOnFromDate = "";
    var EnterdOnToDate = "";
    var LeftOnFromDate = "";
    var LeftOnToDate = "";
    
    $( '#tdHistory25' ).removeClass( "clsPageSizeCurrent" ).addClass( "clsPageSize" );
    
    if(oldTdHistoryId != "")
    {
        $( '#'+oldTdHistoryId ).removeClass( "clsPageSizeCurrent" ).addClass( "clsPageSize" );
    }
    
    $( '#'+tdId ).removeClass( "clsPageSize" ).addClass( "clsPageSizeCurrent" );
    oldTdHistoryId = tdId;
    nRowCntHistory = PageSize;
    
    var SiteId = ReturnSiteIdName(0);
    //ClearLocationChangeHistory();
    
    Tagids = document.getElementById("txtTagHistoryId").value;
    CurrentRoom = document.getElementById("txtCurrentRoomHistoryId").value;
    LastRoom = document.getElementById("txtLastRoomHistoryId").value;
    
    EnterdOnFromDate = document.getElementById("txtEnterdOnDateFrm").value;
    EnterdOnToDate = document.getElementById("txtEnterdOnDateTo").value;
    
    LeftOnFromDate = document.getElementById("LeftOnOnDateFrm").value;
    LeftOnToDate = document.getElementById("LeftOnOnDateTo").value;
    
    LocationChageEventHistory(SiteId,Tagids,CurrentRoom,LastRoom,PageSize,CurrPg,0,"",EnterdOnFromDate,EnterdOnToDate,LeftOnFromDate,LeftOnToDate,g_HistorySort);
    
}

//Clear Location Change Page Live Data
function ClearLocationChangeLive()
{
    /*var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblLocationChangeLive');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblLocationChangeLive');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);*/
            
    //document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChange").value = 1;
    document.getElementById("ctl00_ContentPlaceHolder1_lblCount_LocationChangeLive").innerHTML = "Total Records : 0";
    
}

//Clear Location Change Page History Data
function ClearLocationChangeHistory()
{
    var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblLocationChangeHistory');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblLocationChangeHistory');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_LocationChangeHistory").value = 1;
    document.getElementById("ctl00_ContentPlaceHolder1_lblCount_LocationChangeHistory").innerHTML = "Total Records : 0";
    
}

function ReturnSiteIdName(type)
{
    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteId = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteName = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
    
    if(type==0)
    {
        return siteId;
    }
    else if(type==1)
    {
        return siteName;
    }
   
}
//Sort Location Event Live Page
var g_LiveSort = "";
var g_LiveSortColumn = "";
var g_LiveSortOrder = "";
var g_LiveSortImg = "";

function sortLocationLive(sortCol)
{
    if(g_LiveSortColumn != sortCol)
    {
        g_LiveSortOrder = " desc";
        g_LiveSortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(g_LiveSortOrder == " desc")
        {
            g_LiveSortOrder = " asc";
            g_LiveSortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(g_LiveSortOrder == " asc")
        {
            g_LiveSortOrder = " desc";
            g_LiveSortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }

    if(sortCol != "")
    {
        g_LiveSortColumn = sortCol;
    }

    g_LiveSort = g_LiveSortColumn + g_LiveSortOrder;
    
     LocationChangeLivePgView("sort");
}

//Sort Location Event History Page
var g_HistorySort = "";
var g_HistorySortColumn = "";
var g_HistorySortOrder = "";
var g_HistorySortImg = "";

function sortLocationHistory(sortCol)
{
    if(g_HistorySortColumn != sortCol)
    {
        g_HistorySortOrder = " desc";
        g_HistorySortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(g_HistorySortOrder == " desc")
        {
            g_HistorySortOrder = " asc";
            g_HistorySortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(g_HistorySortOrder == " asc")
        {
            g_HistorySortOrder = " desc";
            g_HistorySortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }

    if(sortCol != "")
    {
        g_HistorySortColumn = sortCol;
    }

    g_HistorySort = g_HistorySortColumn + g_HistorySortOrder;
    
     LocationChangeHistoryPgView("sort");
}