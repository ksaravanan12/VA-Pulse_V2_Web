// JScript File
//*********************************************************
//	Function Name	:	UpdateAnnouncements
//	Input			:	SiteId
//	Description		:	ajax call UpdateAnnouncements
//*********************************************************
var g_UAObj;

function UpdateAnnouncements()
{
    document.getElementById("divLoading_AnnouncementList").style.display = "";
    
    g_UAObj = CreateXMLObj();
    
    var sMessage = $('#txtMessage').val();
    var sStartDate = $('#date_timepicker_start').val();
    var sEndDate = $('#date_timepicker_end').val();
    var sUserRoles = $('#ctl00_ContentPlaceHolder1_drpUserRole').val();  
    var sHTMLMessage = $("#editor").val().replace(/&quot;/g, "'");
    var sShowIn = $("#ctl00_ContentPlaceHolder1_SelShowIn").val()
    var sUserAssociatedViews = '';

    if (setundefined(sUserRoles) != "" && sUserRoles != null) {
        sUserRoles = sUserRoles.toString();
    }
    else
        sUserRoles = "";

    var bIsShow = 0;

    if($('#chkIsShow').is(":checked"))
        bIsShow = 1;
    else
        bIsShow = 0;

    var bIsActive = 0;

    if ($('#chkIsActive').is(":checked"))
        bIsActive = 1;
    else
        bIsActive = 0;

    if($('#chkBatterySummary').is(":checked"))
      sUserAssociatedViews = $('#chkBatterySummary').val();

    var bIsDispAfterExpire = 0;
    if ($('#ChkdispafterExpire').is(":checked"))
        bIsDispAfterExpire = 1;

    var sdaysDispAfterExpire = $('#txtdaysDispafterExpire').val();
    if (setundefined(sdaysDispAfterExpire) == "") {
        sdaysDispAfterExpire = "0";
    }
    var bIsDispHistory = 0;
    if ($('#chkDispHistory').is(":checked"))
        bIsDispHistory = 1;

    $.post("AjaxConnector.aspx?cmd=UpdateAnnouncements",
    {
        Message: setundefined(sMessage),
        StartDate: setundefined(sStartDate),
        EndDate: setundefined(sEndDate),
        UserRoles: setundefined(sUserRoles),
        IsShow: setundefined(bIsShow),
        IsDispAfterExpire: setundefined(bIsDispAfterExpire),
        daysDispAfterExpire: setundefined(sdaysDispAfterExpire),
        IsDispHistory: setundefined(bIsDispHistory),
        AnnouncementId: setundefined($('#hdnAnnouncementId').val()),
        UserAssociatedViews: setundefined(sUserAssociatedViews),
        HtmlMsg: encodeURIComponent(sHTMLMessage),
        IsActive: setundefined(bIsActive),
        ShowIn: setundefined(sShowIn)
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxUpdateAnnouncements(dsRoot);
        }
        else {
            document.getElementById("divLoading_AnnouncementList").style.display = "none";
        }
    });
}

//*********************************************************
//	Function Name	:	ajaxUpdateAnnouncements
//	Input			:	none
//	Description		:	Load Announcements Datas from ajax Response
//*********************************************************
function ajaxUpdateAnnouncements(dsRoot)
{   
    document.getElementById("divLoading_AnnouncementList").style.display = "none";
                           
    var o_Result=dsRoot.getElementsByTagName('Result')
    var Result = getTagNameValue(o_Result[0]);
            
    if(Result == "0")
    {
        alert("Added Successfully!!!");
    }
    else if(Result == "1")
    {
        alert("Error in Adding Announcements!!!");
    }
            
    ClearAnnouncementValues();
    GetAnnouncements(document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_Announcement").value);     
}

//Purchase Order Pagination View
function AnnouncementPgView(pgType)
{
    var CurrPg = document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_Announcement").value;

    if(CurrPg == "")
        CurrPg = 0;

    if(pgType == 2){
        CurrPg = parseInt(CurrPg) + 1;
    } else if(pgType == 3){
        CurrPg = parseInt(CurrPg) - 1;
    }

    document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_Announcement").value = CurrPg;
    GetAnnouncements(CurrPg);
}

//*******************************************************************
//	Function Name	:	GetAnnouncements
//	Input			:	SiteId,DeviceType,DeviceId
//	Description		:	ajax call GetAnnouncements
//*******************************************************************
var g_AnnObj;
function GetAnnouncements(CurrPg)
{
    document.getElementById('divLoading_AnnouncementList').style.display = "";
    
    g_AnnObj = CreateXMLObj();
    
    if(g_AnnObj!=null)
    {
        g_AnnObj.onreadystatechange = ajaxGetAnnouncements;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetAnnouncements&ListForAdmin=1&curpage=" + CurrPg;
      
        if(GetBrowserType()=="isIE")
        {	
            g_AnnObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_AnnObj.open("GET",DbConnectorPath, true);
        }
        g_AnnObj.send(null);         
    }
    return false;
}

//*******************************************************************
//	Function Name	:	ajaxGetAnnouncements
//	Input			:	none
//	Description		:	Load Announcements from ajax Response
//*******************************************************************
var g_AnnRoot;
function ajaxGetAnnouncements()
{
    if(g_AnnObj.readyState==4)
    {
        if(g_AnnObj.status==200)
        {
            var nRootLength;
            var sTbl,sTblLen;
            
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_AnnObj.responseXML.documentElement);
            
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblAnnouncements');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblAnnouncements');
            }
            
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
            
            var dsRoot = g_AnnObj.responseXML.documentElement;
            g_AnnRoot = dsRoot;
            
            //Header
            row=document.createElement('tr');
            AddCell(row,"Message",'siteOverview_TopLeft_Box',"","","center","200px","40px","");
            AddCell(row,"Start Date",'siteOverview_Box',"","","center","140px","40px","");
            AddCell(row,"End Date",'siteOverview_Box',"","","center","140px","40px","");
            AddCell(row,"Displaying",'siteOverview_Box',"","","center","60px","40px","");
            sTbl.appendChild(row);
            
            var o_TotalPage = dsRoot.getElementsByTagName('TotalPage');
            var o_TotalCount = dsRoot.getElementsByTagName('TotalCount');
            var o_AnnouncementId = dsRoot.getElementsByTagName('AnnouncementId');
            var o_Message = dsRoot.getElementsByTagName('Message');
            var o_StartDatetime = dsRoot.getElementsByTagName('StartDatetime');
            var o_EndDateTime = dsRoot.getElementsByTagName('EndDateTime');
            var o_IsShow = dsRoot.getElementsByTagName('IsShow');           
            
            nRootLength = o_Message.length;
            
            if(nRootLength >0)
            {
                var TotalPage = getTagNameValue(o_TotalPage[0]);
                var TotalCount = getTagNameValue(o_TotalCount[0]);
                
                document.getElementById("ctl00_ContentPlaceHolder1_lblCount_Announcement").innerHTML = "Total Records : " + TotalCount;
                document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_Announcement").innerHTML = " of " + TotalPage;
                
                for(var i = 0; i < nRootLength; i++)
                {
                    var AnnouncementId = getTagNameValue(o_AnnouncementId[i]);
                    var Message = getTagNameValue(o_Message[i]);
                    var StartDatetime = getTagNameValue(o_StartDatetime[i]);
                    var EndDateTime = getTagNameValue(o_EndDateTime[i]);
                    var IsShow = getTagNameValue(o_IsShow[i]);
                                        
                    var AnnRowId = "tr_" + AnnouncementId;
                    
                    row=document.createElement('tr');
                    row.setAttribute("id", AnnRowId);
                    
                    AddCellforAnnouncementDetails(row,Message,'DeviceList_leftBox',"","","center","","40px","pointer",AnnouncementId);
                    AddCellforAnnouncementDetails(row,StartDatetime,'siteOverview_cell',"","","center","","40px","pointer",AnnouncementId);
                    AddCellforAnnouncementDetails(row,EndDateTime,'siteOverview_cell',"","","center","","40px","pointer",AnnouncementId);
                    
                    if(IsShow == "True")
                    {
                        AddCellforAnnouncementDetails(row,"Yes",'siteOverview_cell',"","","center","","40px","pointer",AnnouncementId);
                    }
                    else
                    {
                        AddCellforAnnouncementDetails(row,"No",'siteOverview_cell',"","","center","","40px","pointer",AnnouncementId);
                    }
                    
                    sTbl.appendChild(row);
                }
            }
            else
            {
                 document.getElementById("ctl00_ContentPlaceHolder1_lblCount_Announcement").innerHTML = "Total Records : 0";
                 document.getElementById("ctl00_ContentPlaceHolder1_lblPgCnt_Announcement").innerHTML = " of 0";
                
                 row=document.createElement('tr');
                 AddCell(row,"No records found...",'siteOverview_cell_Full',"4","","center","","40px","");
                 sTbl.appendChild(row);
            }
            
            doTblEnableButton(TotalPage, document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_Announcement").value);
            document.getElementById('divLoading_AnnouncementList').style.display = "none";
        }
    }
}

//*******************************************************************
//	Function Name	:	GetHTMLAnnouncements
//	Input			:	AnnouncementId
//	Description		:	ajax call GetAnnouncements
//*******************************************************************
var g_HTMLAnnObj;

function GetHTMLAnnouncement(AnnouncementId) {
    document.getElementById('divLoading_AnnouncementList').style.display = "";

    g_HTMLAnnObj = CreateXMLObj();

    if (g_HTMLAnnObj != null) {
        g_HTMLAnnObj.onreadystatechange = ajaxGetHTMLAnnouncements;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetHTMLAnnouncement&AnnouncementId=" + AnnouncementId;

        if (GetBrowserType() == "isIE") {
            g_HTMLAnnObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_HTMLAnnObj.open("GET", DbConnectorPath, true);
        }
        g_HTMLAnnObj.send(null);
    }
    return false;
}

//*******************************************************************
//	Function Name	:	ajaxGetAnnouncements
//	Input			:	none
//	Description		:	Load Announcements from ajax Response
//*******************************************************************
var g_HTMLAnnRoot;
function ajaxGetHTMLAnnouncements() {
    if (g_HTMLAnnObj.readyState == 4) {
        if (g_HTMLAnnObj.status == 200) {
            var nRootLength;
            var sTbl, sTblLen;

            //Ajax Msg Receiver
            AjaxMsgReceiver(g_HTMLAnnObj.responseXML.documentElement);

            var dsRoot = g_HTMLAnnObj.responseXML.documentElement;
            g_HTMLAnnRoot = dsRoot;

            var o_AnnouncementId = dsRoot.getElementsByTagName('AnnouncementId');
            var o_HTMLMessage = dsRoot.getElementsByTagName('HTMLMessage');
            var o_IsActive = dsRoot.getElementsByTagName('IsActive');
            var o_StartDatetime = dsRoot.getElementsByTagName('StartDatetime');
            var o_EndDateTime = dsRoot.getElementsByTagName('EndDateTime');
            
            nRootLength = o_HTMLMessage.length;

            if (nRootLength > 0) {

                var AnnouncementId = getTagNameValue(o_AnnouncementId[0]);
                var HTMLMessage = getTagNameValue(o_HTMLMessage[0]);
                var StartDatetime = getTagNameValue(o_StartDatetime[0]);
                var EndDateTime = getTagNameValue(o_EndDateTime[0]);
                var IsActive = getTagNameValue(o_IsActive[0]);

                if (IsActive == "True") {
                    $("#txtUpdatedOn").text(StartDatetime);
                    $("#spnExpireOn").text(EndDateTime);
                    $("#divContent").html(HTMLMessage);
                }
            }
            else {

            }
            document.getElementById('divLoading_AnnouncementList').style.display = "none";
        }
    }
}
//Edit on Click List
function EditOnList(AnnouncementId)
{
    $('#btnShow').val('Update');
    $('#hdnAnnouncementId').val(AnnouncementId);
    
    var AnnRowId = "tr_" + AnnouncementId;
    var AnnEdit = $(g_AnnRoot).find("Announcement").children().filter("AnnouncementId").filter(function () { return $( this ).text() == String(AnnouncementId);}).parent();
    
    var Message = getTagNameValue(AnnEdit.children().filter("Message")[0]);
    var StartDatetime = getTagNameValue(AnnEdit.children().filter("StartDatetime")[0]);
    var EndDateTime = getTagNameValue(AnnEdit.children().filter("EndDateTime")[0]);
    var UserRoleToShow = getTagNameValue(AnnEdit.children().filter("UserRoleToShow")[0]);
    var IsShow = getTagNameValue(AnnEdit.children().filter("IsShow")[0]);
    var UserAssociatedViews = getTagNameValue(AnnEdit.children().filter("UserAssociatedViews")[0]);
    var IsActive = getTagNameValue(AnnEdit.children().filter("IsActive")[0]);
    var HTMLMessage = getTagNameValue(AnnEdit.children().filter("HTMLMessage")[0]);
    var ShowIn = getTagNameValue(AnnEdit.children().filter("ShowIn")[0]);
    var dispAfterExpire = getTagNameValue(AnnEdit.children().filter("afterExpireDisp")[0]);
    var dispDaysAfterExpire = getTagNameValue(AnnEdit.children().filter("afterExpireDispDays")[0]);
    var dispHistory = getTagNameValue(AnnEdit.children().filter("DispinHistory")[0]);

    $('#txtMessage').val(Message);
    $('#date_timepicker_start').val(StartDatetime);
    $('#date_timepicker_end').val(EndDateTime);
    $("#editor").val(HTMLMessage);
    $("#ctl00_ContentPlaceHolder1_SelShowIn").val(ShowIn);
    
    UserRoles = UserRoleToShow.split(",");
    $('#ctl00_ContentPlaceHolder1_drpUserRole').val(UserRoles);
    $("#ctl00_ContentPlaceHolder1_drpUserRole").multipleSelect("refresh");

    if(IsShow == "True")
    {
        $('#chkIsShow').prop('checked', true);
    }
    else
    {
        $('#chkIsShow').prop('checked', false);
    }
    
    if (IsActive == "True") {
        $('#chkIsActive').prop('checked', true);
    }
    else {
        $('#chkIsActive').prop('checked', false);
    }

//    if (HTMLMessage) {
//        btnEdit.click();
//    }
//    else {
//        $('#btnCancels').click();
//    }
    if(UserAssociatedViews.length > 0)
    {
         var allUserAssociatedViewsArr = new Array();
         allUserAssociatedViewsArr = UserAssociatedViews.split(",");
	 
         if(allUserAssociatedViewsArr.indexOf("15") > -1)
         {
            $('#chkBatterySummary').prop('checked', true);
         }  
         else
         {
              $('#chkBatterySummary').prop('checked', false);
         }
    }
    else
    {
         $('#chkBatterySummary').prop('checked', false);
    }

    if (dispAfterExpire == "True") {
       $('#ChkdispafterExpire').prop('checked', true);
       $('#txtdaysDispafterExpire').val(setundefined(dispDaysAfterExpire));
    }
    else {
       $('#ChkdispafterExpire').prop('checked', false);
       $('#txtdaysDispafterExpire').val('');
    }
    if (dispHistory == "True") {
       $('#chkDispHistory').prop('checked', true);
         
    }
    else {
       $('#chkDispHistory').prop('checked', false);
    }
    
    //Clear Before Set Color
    $('#tblAnnouncements tr:not(:first-child) td').css({"background-color": "#FFFFFF", "color": "#454545"});
    $('#' + AnnRowId + ' td').css({"background-color": "#A5A2A2", "color": "#FFFFFF"});
}

//Clear Announcement Control Values
function ClearAnnouncementValues()
{
    $('#txtMessage').val('');
    $('#date_timepicker_start').val('');
    $('#date_timepicker_end').val('');
    $('#ctl00_ContentPlaceHolder1_drpUserRole').val('');
    $("#ctl00_ContentPlaceHolder1_drpUserRole").multipleSelect("refresh");
    $('#chkIsShow').prop('checked', false);
    $('#chkIsActive').prop('checked', false);
    $('#btnShow').val('Save');
    $('#hdnAnnouncementId').val('0');
    $('#chkBatterySummary').prop('checked', false);
    $("#editor").val('');
    $("#trlongDesc").hide();
    $('#ChkdispafterExpire').prop('checked', false);
    $('#txtdaysDispafterExpire').val('');
    $('#chkDispHistory').prop('checked', false);
    $("#ctl00_ContentPlaceHolder1_SelShowIn").val('1');
}

//Pagination for Purchase Order View
function doTblEnableButton(MaxPg, CurrPg)
{
    if(parseInt(MaxPg) == 1)
    {
        document.getElementById("btnPrev_Announcement").disabled = true;
        document.getElementById("btnNext_Announcement").disabled = true;
    }
    else
    {
        document.getElementById("btnNext_Announcement").disabled = false;
        document.getElementById("btnPrev_Announcement").disabled = false;
    }
    
    if(parseInt(CurrPg) <= 1)
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_Announcement").value = "1";
        document.getElementById("btnPrev_Announcement").disabled = true;
    }
    
    if(parseInt(CurrPg) >= parseInt(MaxPg))
    {
        document.getElementById("ctl00_ContentPlaceHolder1_txtPgNo_Announcement").value = MaxPg;
        document.getElementById("btnNext_Announcement").disabled = true;
    }
}

//get value from htmlCollection
function getTagNameValue(nodeElem) {
    try {
        return (nodeElem.textContent || nodeElem.innerText || nodeElem.text || nodeElem.innerHTML);
    }

    catch (e) {
        window.location = "UserErrorPage.aspx";
    }
}

//Add Cell for Announcement Details
function AddCellforAnnouncementDetails(row,val,clsName,scolspan,CellId,CellAlign,sWidth,sHeight,bCursor,AnnouncementId)
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
	
    if(AnnouncementId != "")
        cell.setAttribute('onclick',"EditOnList(" + AnnouncementId + ");");
	
	cell.setAttribute("align",CellAlign)
	
	cell.setAttribute("colSpan",scolspan)
	
	cell.style.bgcolor="#FFFFFF"

	cell.innerHTML=setundefined(val);
	
	row.appendChild(cell);
}

//*******************************************************************
//	Function Name	:	PastAnnouncements
//	Input			:	none
//	Description		:	ajax call PastAnnouncements
//*******************************************************************

var g_AnnPastObj;

function PastAnnouncements() {

    document.getElementById("divLoading").style.display = "";
    
    g_AnnPastObj = CreateXMLObj();

    if (g_AnnPastObj != null) {
    
        g_AnnPastObj.onreadystatechange = ajaxPastAnnouncements;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetPastAnnouncements&IsLive=1";

        if (GetBrowserType() == "isIE") {
            g_AnnPastObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_AnnPastObj.open("GET", DbConnectorPath, true);
        }
        g_AnnPastObj.send(null);
    }
    return false;
}

//*******************************************************************
//	Function Name	:	ajaxPAstAnnouncements
//	Input			:	none
//	Description		:	Load Announcements from ajax Response
//*******************************************************************

function ajaxPastAnnouncements() {
    if (g_AnnPastObj.readyState == 4) {
        if (g_AnnPastObj.status == 200) {
	
            var nRootLength;
            var sTbl, sTblLen, sMarqueeTag;
            sMarqueeTag = "";
            var dsRoot = g_AnnPastObj.responseXML.documentElement;
            sTbl = document.getElementById('tblpastannouncement');

            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);
            
            var o_Message = dsRoot.getElementsByTagName('Message');
            var o_HTMLMessage = dsRoot.getElementsByTagName('HTMLMessage');
            var o_IsActive = dsRoot.getElementsByTagName('IsActive');
            var o_AnnouncementId = dsRoot.getElementsByTagName('AnnouncementId');
            var o_ShowIn = dsRoot.getElementsByTagName('ShowIn');
            var o_StartDate = dsRoot.getElementsByTagName('StartDatetime');
            nRootLength = o_Message.length;

            if (nRootLength > 0) {
                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');
                    
                    var Message = getTagNameValue(o_Message[i]);
                    var HTMLMessage = getTagNameValue(o_HTMLMessage[i]);
                    var IsActive = getTagNameValue(o_IsActive[i]);
                    var AnnouncementId = getTagNameValue(o_AnnouncementId[i]);
                    var ShowIn = getTagNameValue(o_ShowIn[i]);
                    var StartDate = getTagNameValue(o_StartDate[i]);

                    if (ShowIn == 1 || ShowIn == 2) {//check show in 2X
                       
                        AddCell(row, StartDate, 'SHeader2', "", "", "left", "200px", "25px", "");
                        sTbl.appendChild(row);
                        
                        row1 = document.createElement('tr');

                        if (IsActive == "True") {
                            Message += "<a style='padding-left: 10px;  font-size: 13px;' href='LongDescription.aspx?AnnouncementId=" + AnnouncementId + "&pageref=1'>View more details >></a>";
                        }
                        
                        sMarqueeTag = "<span style='padding-right: 50px;'>" + Message + "</span><br/><br/>";

                        AddCell(row1, sMarqueeTag, 'subHeader_black', "", "", "left", "500px", "25px", "");
                        sTbl.appendChild(row1);
                        
                        row2 = document.createElement('tr');
                        AddCell(row2, '', 'subHeader_black', "", "", "left", "200px", "10px", "");
                    }                
                    
                }
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Announcements !.", 'subHeader_black', "", "", "center", "500px", "35px", "");             
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        }
    }
}