// JScript File

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////        DISPLAY VIEW         ///////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var g_TagSiteId = 0;
var g_TagRowCount = 0;

var g_Curr = 1;
var g_Next = 2;
var g_Prev = 3;

//On Tag Add Button Click
$(document).on('click','#btnAddTag',function(){
    $("#ifrmTagUpload").attr("src","uploadFile.aspx?Cmd=AddTag&TagSiteId=" + g_TagSiteId + "&TagCSVUrl=" + (HoverTagCSVUrl()));
    
    //Open Dialog
    $( "#dialog-Tag" ).dialog({
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
        }
    });
});

//Pagination for Purchase Order View
function doTblEnableButton(MaxPg, CurrPg)
{
    if(parseInt(MaxPg) == 1)
    {
        document.getElementById("btnPrev_TagMetaView").disabled = true;
        document.getElementById("btnNext_TagMetaView").disabled = true;
    }
    else
    {
        document.getElementById("btnNext_TagMetaView").disabled = false;
        document.getElementById("btnPrev_TagMetaView").disabled = false;
    }
    
    if(parseInt(CurrPg) <= 1)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_TagMetaView").value = "1";
        document.getElementById("btnPrev_TagMetaView").disabled = true;
    }
    
    if(parseInt(CurrPg) >= parseInt(MaxPg))
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_TagMetaView").value = MaxPg;
        document.getElementById("btnNext_TagMetaView").disabled = true;
    }
}

//Purchase Order Pagination View
function TagMetaPgView(pgType)
{
    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

    var CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_TagMetaView").value;

    if(pgType == "show")
    {
        CurrPg = 1;
        g_TMISort =  "TagId desc";
        g_TMISortColumn = "TagId";
        g_TMISortOrder = " desc";
        g_TMISortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }

    if(CurrPg == "")
        CurrPg = 0;

    if(pgType == 2){
        CurrPg = parseInt(CurrPg) + 1;
    } else if(pgType == 3){
        CurrPg = parseInt(CurrPg) - 1;
    }

    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_TagMetaView").value = CurrPg;
    tagListView(CurrPg,g_TMISort);
}

//Sort PO
var g_TMISort = "";
var g_TMISortColumn = "";
var g_TMISortOrder = "";
var g_TMISortImg = "";

function sortTMI(sortCol)
{
    if(g_TMISortColumn != sortCol)
    {
        g_TMISortOrder = " desc";
        g_TMISortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(g_TMISortOrder == " desc")
        {
            g_TMISortOrder = " asc";
            g_TMISortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(g_TMISortOrder == " asc")
        {
            g_TMISortOrder = " desc";
            g_TMISortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }

    if(sortCol != "")
    {
        g_TMISortColumn = sortCol;
    }

    g_TMISort = g_TMISortColumn + g_TMISortOrder;
    tagListView(g_Curr,g_TMISort);
}

//Set Page Row Size Display
var g_TMIPgSize = 0;

function pageSizeTMI(pgSize)
{
    $("#tdLive25").removeClass("clsPageSizeCurrent").addClass("clsPageSize");
    $("#tdLive50").removeClass("clsPageSizeCurrent").addClass("clsPageSize");
    $("#tdLive100").removeClass("clsPageSizeCurrent").addClass("clsPageSize");
    
    g_TMIPgSize = pgSize;
    
    if(pgSize === 25)
    {    
        $("#tdLive25").removeClass("clsPageSize").addClass("clsPageSizeCurrent");
    }
    else if(pgSize === 50)
    {
        $("#tdLive50").removeClass("clsPageSize").addClass("clsPageSizeCurrent");
    }
    else if(pgSize === 100)
    {
        $("#tdLive100").removeClass("clsPageSize").addClass("clsPageSizeCurrent");
    }
    
    tagListView(g_Curr,g_TMISort);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////        TAG META INFO         //////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Ajax Call for Map View [Tag]
var g_TMIObj;
function tagListView(CurrPg,Sort)
{
    document.getElementById("divLoading_TagView").style.display = "";
    
    g_TMIObj = CreateXMLObj();
    
    if(g_TMIObj!=null)
    {
        g_TMIObj.onreadystatechange = ajaxTagListView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=TagMetaInfo&Site=" + g_TagSiteId + "&TagMetaIds=" + $('#txtTagIds').val() + "&curpage=" + CurrPg + "&sortTMI=" + Sort + "&TMIPgSize=" + g_TMIPgSize;
      
        if(GetBrowserType()=="isIE")
        {
            g_TMIObj.open("GET",DbConnectorPath, false);
        } 
        else if(GetBrowserType()=="isFF")
        {
            g_TMIObj.open("GET",DbConnectorPath, true);
        }
        g_TMIObj.send(null);         
    }
    return false;
}

//Ajax Response for Map View [Tag]
function ajaxTagListView()
{
    if(g_TMIObj.readyState==4)
    {
        if(g_TMIObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_TMIObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblTagMetaInfo');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblTagMetaInfo');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_TMIObj.responseXML.documentElement;

            var o_TotalPage = dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount = dsRoot.getElementsByTagName('TotalCount');
            
            var o_TagId=dsRoot.getElementsByTagName('TagId');
            var o_TagName=dsRoot.getElementsByTagName('TagName');
            var o_TagType=dsRoot.getElementsByTagName('TagType');
            var o_Description=dsRoot.getElementsByTagName('Description');
             
            nRootLength=o_TagId.length;
            
            //Header
            row=document.createElement('tr');
            AddCellForSorting(row,"Tag Id",'siteOverview_TopLeft_Box',"","","center","100px","40px","","TagId",g_TMISortColumn,g_TMISortImg,enumSortingArr.TagMetaInfo);
            AddCell(row,"Tag Name",'siteOverview_Box',"","","center","150px","40px","");
            AddCell(row,"Tag Type",'siteOverview_Box',"","","center","150px","40px","");
            AddCell(row,"Description",'siteOverview_Box',"","","center","200px","40px","");
            sTbl.appendChild(row);
            
            var TotalPg = 0;
            
            if(nRootLength > 0)
            {
                TotalPg = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
                TotalCount = (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_TagMetaView").innerHTML = "Total Records : " + TotalCount;
                
                for(var i = 0; i < nRootLength; i++)
                {
                    var TagId = (o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
                    var TagName = (o_TagName[i].textContent || o_TagName[i].innerText || o_TagName[i].text);
                    var TagType = (o_TagType[i].textContent || o_TagType[i].innerText || o_TagType[i].text);
                    var Description = (o_Description[i].textContent || o_Description[i].innerText || o_Description[i].text);
                    
                    row=document.createElement('tr');
                    AddCell(row,TagId,'DeviceList_leftBox',"","","","","40px","");
                    AddCell(row,TagName,'siteOverview_cell',"","","","","40px","");
                    AddCell(row,TagType,'siteOverview_cell',"","","","","40px","");
                    AddCell(row,Description,'siteOverview_cell',"","","","","40px","");
                    sTbl.appendChild(row);
                }
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_TagMetaView").innerHTML = "Total Records : 0";

                row=document.createElement('tr');
                AddCell(row,"No records found...",'siteOverview_cell_Full',4,"","left","","40px","");
                sTbl.appendChild(row);
            }
            
            doTblEnableButton(TotalPg, document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_TagMetaView").value);
            document.getElementById("divLoading_TagView").style.display = "none";
        }
    }
}

//On Show Button Click
$(document).on('click','#btnShowTagIds',function(){
    TagMetaPgView(g_Curr);
});

//Current Csv Link for Tags
function HoverTagCSVUrl()
{
    return getTagNameValue($(g_MPRoot).find("Tag").children().filter("TagCsvFilePath")[0]);
}