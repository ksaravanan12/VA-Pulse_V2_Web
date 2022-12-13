// JScript File

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////        DISPLAY SLIDER         //////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var g_PurchaseOrderSummaryLoaded = false;
var g_PurchaseOrderViewLoaded = false;
var g_DialogView = "";

//Show Home view from Purchase Order
function redirectToHome() {
    location.href = "Home.aspx";
}

//Display view by viewType
function DisplayPurchaseOrder(viewType)
{
    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

    if(viewType === 1){
        $("#ctl00_ContentPlaceHolder1_divPurchaseOrderView").hide();
        $("#ctl00_ContentPlaceHolder1_divPurchaseOrderSummaryView").show();
        
        if(g_PurchaseOrderSummaryLoaded == false) { 
            LoadPoSummaryOverview();
            g_PurchaseOrderSummaryLoaded = true;
        }
    } else if(viewType === 2){
        $("#ctl00_ContentPlaceHolder1_divPurchaseOrderSummaryView").hide();
        $("#ctl00_ContentPlaceHolder1_divPurchaseOrderView").show();
        
        if(g_PurchaseOrderViewLoaded == false){
            g_POSort =  "PONo desc";
            g_POSortColumn = "PONo";
            g_POSortOrder = " desc";
            g_POSortImg = "<image src='Images/downarrow.png' valign='middle' />";
    
            PurchaseOrderPgView("1");
            g_PurchaseOrderViewLoaded = true;
        }
    }
}

function LoadPoSummaryOverview()
{
    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;
    
    document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName_POSummary").innerHTML = siteText;
    LoadPurchaseSummaryView(siteVal);
}

//Clear Purchase Order
function ClearPurchaseOrder()
{
    var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblPurchaseOrderView');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblPurchaseOrderView');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
            
    //document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex = 0;
    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value = 1;
    
    document.getElementById("tdTotalPurchaseOrder").innerHTML = "";
    document.getElementById("tdTotalPurchaseQty").innerHTML = "";
    document.getElementById("tdFirstPurchaseDate").innerHTML = "";
    document.getElementById("tdLastPurchaseDate").innerHTML = "";
    
    $("#drpPODevices").empty();
    $("#drpPODevices").multipleSelect("refresh");
    
    $("#drpModelItem").empty();
    $("#drpModelItem").multipleSelect("refresh");
    
    g_PurchaseOrderSummaryLoaded = false;
    g_PurchaseOrderViewLoaded = false;
}

//Show Purchase Order from Home view
function loadPurchaseOrderView(siteId)
{
    $('#ctl00_ContentPlaceHolder1_divHome').hide('slide', {direction: 'left'}, 100);
    $('#ctl00_ContentPlaceHolder1_divPurchaseOrderView').show('slide', {direction: 'right'}, 600);

    var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblPurchaseOrderView');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblPurchaseOrderView');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
    
    document.getElementById("ctl00_headerBanner_drpsitelist").value = siteId;
    
    g_POSort =  "PODate desc";
    g_POSortColumn = "PODate";
    g_POSortOrder = " desc";
    g_POSortImg = "<image src='Images/downarrow.png' valign='middle' />";

    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

    document.getElementById("ctl00_ContentPlaceHolder1_lblSiteName_PurchaseOrderView").innerHTML = siteText;
    
    LoadPurchaseOrderView(siteId,"1",g_POSort)
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////        LOAD PURCHASE SUMMARY CTLS      /////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//load PONo into Dropdown
function loadPONoDropdown(poList)
{
    $select = $("#drpPODevices");
    $select.empty();
    
    var poListArr = new Array();
    poListArr = poList.split(",");
    
    for(var idx = 0; idx <= poListArr.length - 1; idx++)
    {
        $select.append($("<option>", {"value": poListArr[idx], "text": poListArr[idx] }));
    }
    
    $('#drpPODevices').multipleSelect('refresh');
    /*$('#drpPODevices').multipleSelect('checkAll');*/
}

//load Model Item into Dropdown
function loadModelItemDropdown(miList)
{
    $select = $("#drpModelItem");
    $select.empty();
    
    var miListArr = new Array();
    miListArr = miList.split(",");
    
    for(var idx = 0; idx <= miListArr.length - 1; idx++)
    {
        $select.append($("<option>", {"value": miListArr[idx], "text": miListArr[idx] }));
    }
    
    $('#drpModelItem').multipleSelect('refresh');
    /*$('#drpModelItem').multipleSelect('checkAll');*/
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////        PURCHASE SUMMARY VIEW         //////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Ajax Call for PS
var g_PSObj;
function LoadPurchaseSummaryView(SiteId)
{
    document.getElementById("divLoading_PurchaseSummaryView").style.display = "";
    
    g_PSObj = CreateXMLObj();
    
    if(g_PSObj!=null)
    {
        g_PSObj.onreadystatechange = ajaxLoadPurchaseSummaryView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadPurchaseSummaryView&Site=" + SiteId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_PSObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_PSObj.open("GET",DbConnectorPath, true);
        }
        g_PSObj.send(null);         
    }
    return false;
}

//Ajax Readystate Change for PS
function ajaxLoadPurchaseSummaryView()
{
    if(g_PSObj.readyState==4)
    {
        if(g_PSObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_PSObj.responseXML.documentElement);
            
            var dsRoot = g_PSObj.responseXML.documentElement;
             
            var o_TotalPO=dsRoot.getElementsByTagName('TotalPO');
            var o_FirstPODate=dsRoot.getElementsByTagName('FirstPODate');
            var o_LastPODate=dsRoot.getElementsByTagName('LastPODate');
            var o_PONO=dsRoot.getElementsByTagName('PONO');
            var o_POQty=dsRoot.getElementsByTagName('POQty');
            var o_ModelItem=dsRoot.getElementsByTagName('ModelItem');
             
            nRootLength=o_PONO.length;
            
            if(nRootLength >0)
            {
                document.getElementById("tdTotalPurchaseOrder").innerHTML = (o_TotalPO[0].textContent || o_TotalPO[0].innerText || o_TotalPO[0].text);
                document.getElementById("tdTotalPurchaseQty").innerHTML = (o_POQty[0].textContent || o_POQty[0].innerText || o_POQty[0].text);
                document.getElementById("tdFirstPurchaseDate").innerHTML = setundefined(o_FirstPODate[0].textContent || o_FirstPODate[0].innerText || o_FirstPODate[0].text);
                document.getElementById("tdLastPurchaseDate").innerHTML = setundefined(o_LastPODate[0].textContent || o_LastPODate[0].innerText || o_LastPODate[0].text);
                
                if(setundefined(o_PONO[0].textContent || o_PONO[0].innerText || o_PONO[0].text) != "")
                    loadPONoDropdown((o_PONO[0].textContent || o_PONO[0].innerText || o_PONO[0].text));
                    
                if(setundefined(o_ModelItem[0].textContent || o_ModelItem[0].innerText || o_ModelItem[0].text) != "")
                    loadModelItemDropdown((o_ModelItem[0].textContent || o_ModelItem[0].innerText || o_ModelItem[0].text));
            }
            
            document.getElementById("divLoading_PurchaseSummaryView").style.display = "none";
        }
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////        PURCHASE ORDER VIEW         ///////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Ajax Call for PO
var g_POObj;
function LoadPurchaseOrderView(SiteId,CurrPage,Sort)
{
    document.getElementById("divLoading_PurchaseOrderView").style.display = "";
    
    var PONo = $('#drpPODevices').val();
    var ModelItem = $('#drpModelItem').val();
    
    if(PONo == null){ PONo = ""; };
    if(ModelItem == null){ ModelItem = ""; };
    
    g_POObj = CreateXMLObj();
    
    if(g_POObj!=null)
    {
        g_POObj.onreadystatechange = ajaxLoadPurchaseOrderView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadPurchaseOrderView&Site=" + SiteId + "&curpage=" + CurrPage + "&sortPO=" + Sort + "&PONo=" + PONo + "&ModelItem=" + ModelItem;
      
        if(GetBrowserType()=="isIE")
        {	
            g_POObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_POObj.open("GET",DbConnectorPath, true);
        }
        g_POObj.send(null);         
    }
    return false;
}

//Ajax Readystate Change for PO
function ajaxLoadPurchaseOrderView()
{
    if(g_POObj.readyState==4)
    {
        if(g_POObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_POObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblPurchaseOrderView');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblPurchaseOrderView');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_POObj.responseXML.documentElement;
             
            var o_TotalPage=dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount=dsRoot.getElementsByTagName('TotalCount');
            var o_PONO=dsRoot.getElementsByTagName('PONO');
            var o_Date=dsRoot.getElementsByTagName('Date');
            var o_Company=dsRoot.getElementsByTagName('Company');
            var o_CityState=dsRoot.getElementsByTagName('CityState');
            var o_Zip=dsRoot.getElementsByTagName('Zip');
            var o_Notes=dsRoot.getElementsByTagName('Notes');
             
            nRootLength=o_PONO.length;
            
            //Header
            row=document.createElement('tr');
            AddCellForSorting(row,"PO No",'siteOverview_TopLeft_Box',"","","center","120px","40px","","PONo",g_POSortColumn,g_POSortImg,enumSortingArr.PurchaseOrderView);
            AddCellForSorting(row,"Date",'siteOverview_Box',"","","center","120px","40px","","PODate",g_POSortColumn,g_POSortImg,enumSortingArr.PurchaseOrderView);
            AddCellForSorting(row,"Company",'siteOverview_Box',"","","center","120px","40px","","CompanyName",g_POSortColumn,g_POSortImg,enumSortingArr.PurchaseOrderView);
            AddCellForSorting(row,"City/State",'siteOverview_Box',"","","center","70px","40px","","City_state",g_POSortColumn,g_POSortImg,enumSortingArr.PurchaseOrderView);
            sTbl.appendChild(row);
            
            if(nRootLength >0)
            {
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_PurchaseOrderView").innerHTML = "Total Records : " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
                //document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_PurchaseOrderView").innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
                
                getMaxPages(o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);
                
                for(var i = 0; i < nRootLength; i++)
                {
                    var PONO = (o_PONO[i].textContent || o_PONO[i].innerText || o_PONO[i].text);
                    var Date = (o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text);
                    var Company = (o_Company[i].textContent || o_Company[i].innerText || o_Company[i].text);
                    var CityState = (o_CityState[i].textContent || o_CityState[i].innerText || o_CityState[i].text);
                    var Zip = (o_Zip[i].textContent || o_Zip[i].innerText || o_Zip[i].text);
                    var Notes = (o_Notes[i].textContent || o_Notes[i].innerText || o_Notes[i].text);
                    
                    row=document.createElement('tr');
                    AddCell(row,"<span onclick=\"OpenPDDialog('" + PONO + "');\" style='cursor: pointer;'>" + PONO + "</span>",'DeviceList_leftBox DeviceDetailsLink',"","","","","40px","");
                    AddCell(row,Date,'DeviceList_leftBox',"","","","","40px","");
                    AddCell(row,Company,'siteOverview_cell',"","","","","40px","");
                    AddCell(row,CityState,'siteOverview_cell',"","","","","40px","");
                    sTbl.appendChild(row);
                }
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_PurchaseOrderView").innerHTML = "Total Records : 0";
                document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_PurchaseOrderView").innerHTML = " of 1";
                
                row=document.createElement('tr');
                AddCell(row,"No Records found!!!",'siteOverview_cell_Full',"5","","","","40px","");
                sTbl.appendChild(row);
                document.getElementById("divLoading_PurchaseOrderView").style.display = "none";
            }
            
            doTblEnableButton(nTotalPg, document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value);
            document.getElementById("divLoading_PurchaseOrderView").style.display = "none";
        }
    }
}

//Get Set Max Page
var nTotalPg;
function getMaxPages(ttlRecord)
{
    var nPgCnt = 0;
    var nRowCnt = 10;
    var CurrPg;
    
    CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value;
    nPgCnt = Math.ceil(ttlRecord / nRowCnt);
    
    document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_PurchaseOrderView").innerHTML = " of " + nPgCnt;
    nTotalPg = nPgCnt;
}

//Pagination for Purchase Order View
function doTblEnableButton(MaxPg, CurrPg)
{
    if(parseInt(MaxPg) == 1)
    {
        document.getElementById("btnPrev_PurchaseOrderView").disabled = true;
        document.getElementById("btnNext_PurchaseOrderView").disabled = true;
    }
    else
    {
        document.getElementById("btnNext_PurchaseOrderView").disabled = false;
        document.getElementById("btnPrev_PurchaseOrderView").disabled = false;
    }
    
    if(parseInt(CurrPg) <= 1)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value = "1";
        document.getElementById("btnPrev_PurchaseOrderView").disabled = true;
    }
    
    if(parseInt(CurrPg) >= parseInt(MaxPg))
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value = nTotalPg;
        document.getElementById("btnNext_PurchaseOrderView").disabled = true;
    }
}

//Purchase Order Pagination View
function PurchaseOrderPgView(pgType)
{
    var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
    var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;
    var siteText = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].text;

    var CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value;

    $("#ctl00_ContentPlaceHolder1_lblSiteName_PurchaseOrderView")[0].innerHTML = siteText;

    if(pgType == "show")
    {
        CurrPg = 1;
        g_POSort =  "PODate desc";
        g_POSortColumn = "PODate";
        g_POSortOrder = " desc";
        g_POSortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }

    if(CurrPg == "")
        CurrPg = 0;

    if(pgType == 2){
        CurrPg = parseInt(CurrPg) + 1;
    } else if(pgType == 3){
        CurrPg = parseInt(CurrPg) - 1;
    }

    document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo_PurchaseOrderView").value = CurrPg;
    LoadPurchaseOrderView(siteVal,CurrPg,g_POSort);
}

//Sort PO
var g_POSort = "";
var g_POSortColumn = "";
var g_POSortOrder = "";
var g_POSortImg = "";

function sortPO(sortCol)
{
    if(g_POSortColumn != sortCol)
    {
        g_POSortOrder = " desc";
        g_POSortImg = "<image src='Images/downarrow.png' valign='middle' />";
    }
    else
    {
        if(g_POSortOrder == " desc")
        {
            g_POSortOrder = " asc";
            g_POSortImg = "<image src='Images/uparrow.png' valign='middle' />";
        }
        else if(g_POSortOrder == " asc")
        {
            g_POSortOrder = " desc";
            g_POSortImg = "<image src='Images/downarrow.png' valign='middle' />";
        }
    }

    if(sortCol != "")
    {
        g_POSortColumn = sortCol;
    }

    g_POSort = g_POSortColumn + g_POSortOrder;
    PurchaseOrderPgView("1");
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////        PURCHASE ORDER DETAILS VIEW         ///////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Open Purchase Details Dialog
function OpenPDDialog(PONo)
{
    g_DialogView = "dialog-PurchaseOrder";
    
    var sTbl,sTblLen;
    if(GetBrowserType()=="isIE")
    {
        sTbl=document.getElementById('tblPurchaseOrderDialog');
    }
    else if(GetBrowserType()=="isFF")
    {
        sTbl=document.getElementById('tblPurchaseOrderDialog');
    }
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);
    
    var winWidth = $(window).width() - 570;
    var winHeight = $(window).height() - 140;
    
    LoadPurchaseDetailsView(PONo);

    //Open Dialog
    $( "#dialog-PurchaseOrder" ).dialog({
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
function LoadPurchaseDetailsView(PONo)
{
    document.getElementById("divLoading_PD").style.display = '';
    
    g_PDObj = CreateXMLObj();
    
    if(g_PDObj!=null)
    {
        g_PDObj.onreadystatechange = ajaxLoadPurchaseDetailsView;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadPurchaseDetailsView&PONo=" + PONo;
      
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
function ajaxLoadPurchaseDetailsView()
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
                sTbl=document.getElementById('tblPurchaseOrderDialog');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblPurchaseOrderDialog');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_PDObj.responseXML.documentElement;
            
            var o_TotalPage=dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount=dsRoot.getElementsByTagName('TotalCount');
            var o_PONO=dsRoot.getElementsByTagName('PONO');
            var o_PODate=dsRoot.getElementsByTagName('PODate');
            var o_Company=dsRoot.getElementsByTagName('Company');
            var o_Site=dsRoot.getElementsByTagName('Site');
            var o_ModelItem=dsRoot.getElementsByTagName('ModelItem');
            var o_POQty=dsRoot.getElementsByTagName('POQty');
            var o_PONotes=dsRoot.getElementsByTagName('PONotes');
            var o_BatchNo=dsRoot.getElementsByTagName('BatchNo');
            var o_Date=dsRoot.getElementsByTagName('Date');
            var o_Qty=dsRoot.getElementsByTagName('Qty');
            var o_ShippedQty=dsRoot.getElementsByTagName('ShippedQty');
            var o_SWversion=dsRoot.getElementsByTagName('SWversion');
            var o_Notes=dsRoot.getElementsByTagName('Notes');
            var o_PORemainingFill=dsRoot.getElementsByTagName('PORemainingFill');
            
            var oldModelItem = "";
            var sReturnTblStr = ""
            var LastBatchNo;
            var bNewPO = false;
             
            nRootLength=o_PONO.length;
            
            if(nRootLength > 0)
            {
                var PONO = (o_PONO[0].textContent || o_PONO[0].innerText || o_PONO[0].text);
                var PODate = (o_PODate[0].textContent || o_PODate[0].innerText || o_PODate[0].text);
                var Company = (o_Company[0].textContent || o_Company[0].innerText || o_Company[0].text);
                var Site = (o_Site[0].textContent || o_Site[0].innerText || o_Site[0].text);
                
                row=document.createElement('tr');
                AddCell(row,"PO No : " + PONO,'clsPDLabel',"","","","200px","40px");
                AddCell(row,"PO Date : " + PODate,'clsPDLabel',"","","","200px","40px");
                AddCell(row,"Company : " + Company,'clsPDLabel',"","","","200px","40px");
                AddCell(row,"Site : " + Site,'clsPDLabel',"","","","200px","40px");
                
                row2=document.createElement('tr');
                AddCell(row2,"<table cellpadding='0' cellspacing='0' border='0' width='100%' style='padding-left: 15px;'><tr><td><table cellpadding='0' cellspacing='0' border='0' width='100%'>" + row.outerHTML + "</table></td></tr></table>",'',"4","","","","");
                sTbl.appendChild(row2);
            }
            
            //Header
            row=document.createElement('tr');
            AddCell(row,"",'PODetails_HeaderBox',"","","center","2px","40px");
            AddCell(row,"Model Item",'PODetails_HeaderBox',"","","center","120px","40px");
            AddCell(row,"PO Qty",'PODetails_HeaderBox',"","","center","120px","40px");
            AddCell(row,"PO Notes",'PODetails_HeaderBox',"","","center","120px","40px");
            sTbl.appendChild(row);
            
            if(nRootLength >0)
            {
                for(var i = 0; i < nRootLength; i++)
                {
                    var ModelItem = (o_ModelItem[i].textContent || o_ModelItem[i].innerText || o_ModelItem[i].text);
                    var POQty = (o_POQty[i].textContent || o_POQty[i].innerText || o_POQty[i].text);
                    var PONotes = (o_PONotes[i].textContent || o_PONotes[i].innerText || o_PONotes[i].text);
                    
                    var BatchNo = (o_BatchNo[i].textContent || o_BatchNo[i].innerText || o_BatchNo[i].text);
                    var Date = (o_Date[i].textContent || o_Date[i].innerText || o_Date[i].text);
                    var Qty = (o_Qty[i].textContent || o_Qty[i].innerText || o_Qty[i].text);
                    var ShippedQty = (o_ShippedQty[i].textContent || o_ShippedQty[i].innerText || o_ShippedQty[i].text);
                    var SWversion = (o_SWversion[i].textContent || o_SWversion[i].innerText || o_SWversion[i].text);
                    var Notes = (o_Notes[i].textContent || o_Notes[i].innerText || o_Notes[i].text);
                    var PORemainingFill = (o_PORemainingFill[i].textContent || o_PORemainingFill[i].innerText || o_PORemainingFill[i].text);
                    
                    if(oldModelItem != ModelItem || i == 0)
                    {
                        bNewPO = true;
                        
                        if(i > 0)
                        {
                            var tblId = "PD_" + LastBatchNo;
                            row=document.createElement('tr');
                            AddCellforPODetails(row,"<table cellpadding='0' cellspacing='0' border='0' width='100%' id='" + tblId + "' style='display: none;'>" + sReturnTblStr + "</table>",'POList_FullCell',"4","","center","","","","","");
                            sTbl.appendChild(row);
                            
                            sReturnTblStr = "";
                        }
                        
                        row=document.createElement('tr');
                        AddCellforPODetails(row,"<img id='img_" + BatchNo + "' src='Images/rightarrow.png' />",'POList_HeaderLeftBox',"","","","","40px","PD_" + BatchNo,"img_" + BatchNo,"pointer");
                        AddCellforPODetails(row,ModelItem,'POList_HeaderLeftBox',"","","","","40px","PD_" + BatchNo,"img_" + BatchNo,"pointer");
                        AddCellforPODetails(row,POQty,'POList_HeaderBox',"","","","","40px","PD_" + BatchNo,"img_" + BatchNo,"pointer");
                        AddCellforPODetails(row,PONotes,'POList_HeaderBox',"","","","","40px","PD_" + BatchNo,"img_" + BatchNo,"pointer");
                        sTbl.appendChild(row);
                        
                        LastBatchNo = BatchNo;
                    }
                    
                    sReturnTblStr = sReturnTblStr + AddPODetailsList(BatchNo,Date,Qty,ShippedQty,SWversion,Notes,PORemainingFill,bNewPO);
                    bNewPO = false;
                    
                    oldModelItem = ModelItem;
                }
                
                var tblId = "PD_" + LastBatchNo;
                row=document.createElement('tr');
                AddCellforPODetails(row,"<table cellpadding='0' cellspacing='0' border='0' width='100%' id='" + tblId + "' style='display: none;'>" + sReturnTblStr + "</table>",'POList_FullCell',"4","","center","","","","","");
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

//Add Cell for PO Details
function AddCellforPODetails(row,val,clsName,scolspan,CellId,CellAlign,sWidth,sHeight,tblId,imgId,bCursor)
{
	var cell = document.createElement("td");
	
	if (clsName!="")
	{
		cell.className=clsName;
	}
	
	if(CellId != "")
	    cell.id=CellId
	
	if(CellAlign=="")
	{
		cell.setAttribute("align","left");
	}
	else
	{
		cell.setAttribute("align",CellAlign);
	}
	
	if(sWidth!="")
	{
	    cell.setAttribute("width",sWidth)
	}
	if(sHeight!=""){
	    cell.setAttribute("height",sHeight)
	}
	
	if(scolspan=="")
	    scolspan=1;
	
	if(bCursor != "")
	    cell.style.cursor = bCursor;
	
    if(tblId != "")
        cell.setAttribute('onclick',"ShowHidePD('" + tblId + "','" + imgId + "');");
	
	cell.setAttribute("align",CellAlign)
	
	cell.setAttribute("colSpan",scolspan)
	
	cell.style.bgcolor="#FFFFFF"

	cell.innerHTML=setundefined(val);
	
	row.appendChild(cell);		
}

function ShowHidePD(ctrl, imgCtrl)
{
    if($("#" + ctrl).is(":visible")){
        $("#" + ctrl).slideUp();
        $("#" + imgCtrl).attr({"src":"Images/rightarrow.png"})
    } else {
        $("#" + ctrl).slideDown();
        $("#" + imgCtrl).attr({"src":"Images/downarrow.png"})
    }
}

function AddPODetailsList(BatchNo,Date,Qty,ShippedQty,SWversion,Notes,PORemainingFill,bNewPO)
{
    var row;
    var row2;
    var sTbl = "";
    
    if(bNewPO)
    {
        row=document.createElement('tr');
        AddCell(row,"Batch No",'siteOverview_leftBox clsPOListHeaderLabel',"","","","120px","40px");
        AddCell(row,"Date",'siteOverview_Box clsPOListHeaderLabel',"","","","160px","40px");
        AddCell(row,"Qty",'siteOverview_Box clsPOListHeaderLabel',"","","","60px","40px");
        AddCell(row,"Shipped Qty",'siteOverview_Box clsPOListHeaderLabel',"","","","120px","40px");
        AddCell(row,"SW Version",'siteOverview_Box clsPOListHeaderLabel',"","","","120px","40px");
        sTbl = sTbl + row.outerHTML;
    }
    
    row2=document.createElement('tr');
    AddCell(row2,BatchNo,'DeviceList_leftBox clsPOListLabel',"","","","","40px");
    AddCell(row2,Date,'siteOverview_cell clsPOListLabel',"","","","","40px");
    AddCell(row2,Qty,'siteOverview_cell clsPOListLabel',"","","","","40px");
    AddCell(row2,ShippedQty,'siteOverview_cell clsPOListLabel',"","","","","40px");
    AddCell(row2,SWversion,'siteOverview_cell clsPOListLabel',"","","","","40px");
    sTbl = sTbl + row2.outerHTML;
    
    return sTbl;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////        GENERAL FUNCTION FOR CLOSE         ////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Close Dialog on mouse up 
$(document).mouseup(function (e)
{
    var container = $("#dialog-PurchaseOrder");
    var container2 = container.parent();
    
    var container3 = $("#dialog-Map");
    var container4 = container3.parent();

    if(g_DialogView === "dialog-PurchaseOrder")
    {
        if (container.has(e.target).length === 0 && container2.has(e.target).length === 0)
        {
            if(document.getElementById("dialog-PurchaseOrder").style.display != "none")
            {
                $("#dialog-PurchaseOrder").dialog("close");
                g_DialogView = "";
            }
        }
    }
    
    if(g_DialogView === "dialog-Map")
    {
        if (container3.has(e.target).length === 0 && container4.has(e.target).length === 0)
        {
            if(document.getElementById("dialog-Map").style.display != "none")
            {
                $("#dialog-Map").dialog("close");
                g_DialogView = "";
            }
        }
    }
});