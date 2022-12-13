// JScript File
var SortColumn='';
var SortOrder='';
var SortImg='';

SortColumn = "TagId";
SortOrder = "desc";
SortImg = "<image src='Images/downarrow.png' valign='middle' />";    

var g_MPObj;
var g_MPRoot;

var siteid;

function loadUpdateBatteryReplace(SiteId,TagId,ReDate,comments)
{
   document.getElementById("divLoadBattery").style.display="";   

    $.post("AjaxConnector.aspx?cmd=UpdateBattery",
    {
        sid: SiteId,
        Tag: TagId,
        RDate: ReDate,
        comments: comments
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxloadUpdateBatteryReplace();
        }
        else {
            $("#divLoadBattery").hide();
        }
    });
}

var g_TagRoot;
function ajaxloadUpdateBatteryReplace()
{
    document.getElementById("divBatteryReplacementTable").style.display = "none";
    document.getElementById("divBatteryReplacementSubmit").style.display = "";
    document.getElementById("hdtblBatteryTechLbiList").style.display = "";
    document.getElementById("hdScrolltblBatteryTechLbiList").style.display = "";
    document.getElementById("divLoadBattery").style.display = "none";
    document.getElementById("tblBatteryTechLbiList").style.display = "";
    $("#txtBattery").val('');
    $("#txtBattery").focus();
    document.getElementsByName('txtBattery')[0].placeholder = 'Tag ID';
    document.getElementById("divGoodDevice").style.display = "none";

    //Remove Save Id from xml and table

    var lbiTagIdList = g_BatteryTechTagRoot.getElementsByTagName('TagId');

    for (var idx = lbiTagIdList.length - 1; idx >= 0; idx--) {
        var strTagid = (lbiTagIdList[idx].textContent || lbiTagIdList[idx].innerText || lbiTagIdList[idx].text);
        if (g_CurrentReplaceTagId == strTagid) {
            if (GetBrowserType() == "isIE")
                g_BatteryTechTagRoot.removeChild(g_BatteryTechTagRoot.childNodes[idx]);
            else
                g_BatteryTechTagRoot.removeChild(g_BatteryTechTagRoot.children[idx]);
            break;
        }
    }

    var sTbl, sTblLen;
    sTbl = document.getElementById('tblBatteryTechLbiList');
    sTblLen = sTbl.rows.length;

    for (var idx = sTblLen - 2; idx >= 0; idx--) {
        if (g_CurrentReplaceTagId == sTbl.children[1].childNodes[idx].children[2].innerHTML) {
            sTbl.children[1].removeChild(sTbl.children[1].childNodes[idx]);
            break;
        }
    }
}

function CreateTagXMLObj()
{
	var obj = null;
	if (window.ActiveXObject) 
	{
		try
		{ 
			obj = new ActiveXObject("Msxml2.XMLHTTP"); 
		}
		catch(e)
		{
			try
			{
				obj = new ActiveXObject("Microsoft.XMLHTTP");
			}
		catch(e1)
			{
				obj = null;	
			}
		}
	}
	else if(window.XMLHttpRequest) 
	{
		obj = new XMLHttpRequest();
		obj.overrideMimeType('text/xml');
	}
	return obj;
}

function SaveLbiList(siteid,TagId)
{   
   var o_Tag = $(g_BatteryTechTagRoot).find("TagId").filter("TagId").filter(function () { return $( this ).text() == String(TagId);}).parent(); 
   
       if (o_Tag.length>0) 
        {
              document.getElementById("tblBatteryTechLbiList").style.display="none";
              document.getElementById("divLbiBatteryTechList").style.display="none";
              document.getElementById("hdtblBatteryTechLbiList").style.display="none";    
              document.getElementById("hdScrolltblBatteryTechLbiList").style.display="none";   
              document.getElementById('txtDate').value="";
              document.getElementById('txtcmnt').value ="";       
                            
              var strTagId=$(o_Tag).children().filter('TagId')[0].textContent || $(o_Tag).children().filter('TagId')[0].innerText || $(o_Tag).children().filter('TagId')[0].text;
              var strTagType=$(o_Tag).children().filter('TagType')[0].textContent || $(o_Tag).children().filter('TagType')[0].innerText || $(o_Tag).children().filter('TagType')[0].text;
              var strCatastrophicCases=$(o_Tag).children().filter('CatastrophicCases')[0].textContent || $(o_Tag).children().filter('CatastrophicCases')[0].innerText || $(o_Tag).children().filter('CatastrophicCases')[0].text;
              var strBatteryReplacementOn=$(o_Tag).children().filter('BatteryReplacementOn')[0].textContent || $(o_Tag).children().filter('BatteryReplacementOn')[0].innerText || $(o_Tag).children().filter('BatteryReplacementOn')[0].text;

              strBatteryReplacementOn = setundefined(strBatteryReplacementOn);
              
              var s30DaysCell = "";
              var s90DaysCell = "";
              var sgood = "";
                        
              if (strCatastrophicCases == "1" || strCatastrophicCases == "2")
              {
                s30DaysCell = "<img src='images/Battery-Red.png' border='0' />";
              }
              else if(strCatastrophicCases == "4")
              {
                s90DaysCell = "<img src='images/Batter-yellow.png' border='0' />";
              }
              else if(strCatastrophicCases == "0")
              {
               sgood = "<img src='images/Battery-Green.png' border='0' />";
              }            
              sTbl=document.getElementById("divBatteryReplacementTable");      
              var cells = sTbl.getElementsByTagName('td');
              cells[0].innerHTML=strTagId;
              var cnt;
              var imgname;
              var tag=strTagType.toUpperCase(); 

              imgname=GetDeviceImage(tag,"1");

              cnt="<table cellpadding='10' style='padding-left: 20px;'><tr><td><img src='Images/" + imgname + "'/></td><td>" + strTagType +"</td></tr></table>";
              cells[1].innerHTML=  cnt; 

              if (strCatastrophicCases == "1" || strCatastrophicCases == "2")
                cnt="<table cellpadding='10' style='padding-left: 20px;'><tr><td>< 30 Days</td><td>" + s30DaysCell + "</td></tr></table>";
              else if(strCatastrophicCases == "4")   
                cnt="<table cellpadding='10' style='padding-left: 20px;'><tr><td>< 90 Days</td><td>" + s90DaysCell + "</td></tr></table>";
              else
                cnt="<table cellpadding='10' style='padding-left: 35px;'><tr><td>Good</td><td>" + sgood + "</td></tr></table>";
              
              if (strBatteryReplacementOn=='') 
                  strBatteryReplacementOn='';
            
              var replacementdt = strBatteryReplacementOn.split(" ");
              document.getElementById("txtDate").value = replacementdt[0];
              
              cells[4].innerHTML=cnt; 
              document.getElementById("divBatteryReplacementTable").style.display="";
              document.getElementById("divBatteryReplacementSubmit").style.display="none";
              document.getElementById("divLoadBattery").style.display="none";
              
             $("#txtDate").focus();
                 
             $('#txtDate').on('paste', function(e) {
                  if(document.getElementById("divBatteryReplacementTable").style.display=="")
                   {
                        e.preventDefault();
                        var pastedText = '';
                        if (window.clipboardData && window.clipboardData.getData) { // IE
                            pastedText = window.clipboardData.getData('Text');
                          } else if (e.clipboardData && e.clipboardData.getData) {
                            pastedText = e.clipboardData.getData('text/plain');
                          }
                        //this.value = pastedText.replace(/\D/g, '');
                        var fDate = new Date();
                        document.getElementById('txtDate').value= (fDate.getMonth() + 1) + '/' +  fDate.getDate() + '/' + fDate.getFullYear();
                        $('#txtcmnt').focus();
                         document.getElementById("btnsave").disabled=false;
                         document.getElementById("btnsave").style.color="#ffffff";
                   }
             });
                
            $('#txtDate').keydown(function(e){
                if (e.keyCode == 13) { // barcode scanned!
                    var fDate = new Date();
                        document.getElementById('txtDate').value= (fDate.getMonth() + 1) + '/' +  fDate.getDate() + '/' + fDate.getFullYear();
                    $('#txtcmnt').focus();
                     document.getElementById("btnsave").disabled=false;
                     document.getElementById("btnsave").style.color="#ffffff";
                    return false; // block form from being submitted yet
                }
                else
                {
                    e.preventDefault();
                }
            });
            
            document.getElementById("btnsave").disabled=true;
             document.getElementById("btnsave").style.color="#313131";
              
      }
      else
      {
         LoadTagDetails(siteid,TagId)
      } 
    
}

var g_Obj;
function LoadTagDetails(siteid,TagId)
{    
    document.getElementById("divLoadBattery").style.display=""; 
      
    g_Obj = CreateTagXMLObj(); 
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxLoadTagDetails;        

        DbConnectorPath = "AjaxConnector.aspx?cmd=TagList&sid="+ siteid +"&curpage=0&DeviceId="+TagId;     
                      
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
//	Function Name	:	ajaxLoadTagDetails
//	Input			:	None
//	Description		:	Load Tag Datas from ajax Response
//*******************************************************************
var g_TagRoot;
function ajaxLoadTagDetails()
{
    if(g_Obj.readyState==4)
    {
        if(g_Obj.status==200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Obj.responseXML.documentElement);
            g_TagRoot = g_Obj.responseXML.documentElement;
            LoadTagDetailsList();            
            document.getElementById("divLoadBattery").style.display="none"; 
            document.getElementById("btnsave").disabled=true;
             document.getElementById("btnsave").style.color="#313131";
        }
    }
}

function LoadTagDetailsList()
{
    var sTbl;      
    var o_siteId=g_TagRoot.getElementsByTagName('SiteID');
    var o_TagId=g_TagRoot.getElementsByTagName('TagId');
    var o_LessThen90Days=g_TagRoot.getElementsByTagName('LessThen90Days');
    var o_LessThen30Days=g_TagRoot.getElementsByTagName('LessThen30Days');
    var o_CatastrophicCases=g_TagRoot.getElementsByTagName('CatastrophicCases');
    var o_TagType = g_TagRoot.getElementsByTagName('TagType');
    var o_BatteryReplaced = g_TagRoot.getElementsByTagName('BatteryReplacementOn');
        
    var nTagLength=o_TagId.length;
    
    if(nTagLength==0)
    {   
         document.getElementById("divLbiBatteryTechList").style.display="";    
        if(GetBrowserType()=="isIE")
        {
             document.getElementById("trrecnotfound").innerText="Tag Id not found.";
        }
        else
        {
             document.getElementById("trrecnotfound").innerHTML="Tag Id not found.";
        }
    }
    else
    {
         document.getElementById("tblBatteryTechLbiList").style.display="none"; 
         document.getElementById("divLbiBatteryTechList").style.display="none";
         document.getElementById("hdtblBatteryTechLbiList").style.display="none";    
         document.getElementById("hdScrolltblBatteryTechLbiList").style.display="none";   
         
         nRootLength=o_siteId.length;
   
           if(nRootLength >0)
           {
               for(var i=0; i<nRootLength; i++)
               {
                var siteid=(o_siteId[i].textContent || o_siteId[i].innerText || o_siteId[i].text);
                var TagId=(o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
                var LessThen90Days=(o_LessThen90Days[i].textContent || o_LessThen90Days[i].innerText || o_LessThen90Days[i].text);
                var LessThen30Days=(o_LessThen30Days[i].textContent || o_LessThen30Days[i].innerText || o_LessThen30Days[i].text);
                var CatastrophicCases=(o_CatastrophicCases[i].textContent || o_CatastrophicCases[i].innerText || o_CatastrophicCases[i].text);
                var TagType=(o_TagType[i].textContent || o_TagType[i].innerText || o_TagType[i].text);
                var BatteryReplaced=(o_BatteryReplaced[i].textContent || o_BatteryReplaced[i].innerText || o_BatteryReplaced[i].text);
         
                BatteryReplaced = setundefined(BatteryReplaced);
                if (CatastrophicCases == "0")
                {
                  document.getElementById("divGoodDevice").style.display="";           
                 
                }
                else
                {
                
                 document.getElementById("divGoodDevice").style.display="none";           
                 
                }        
         
                var s30DaysCell = "";
                var s90DaysCell = "";
                var sgood = "";
                
                if (CatastrophicCases == "1" || CatastrophicCases == "2")
                {
                    s30DaysCell = "<img src='images/Battery-Red.png' border='0' />";
                }
                else if(CatastrophicCases == "4")
                {
                    s90DaysCell = "<img src='images/Batter-yellow.png' border='0' />";
                }
                else if(CatastrophicCases == "0")
                {
                    sgood = "<img src='images/Battery-Green.png' border='0' />";
                }            
                   
                 sTbl=document.getElementById("divBatteryReplacementTable");      
                 var cells = sTbl.getElementsByTagName('td');
                
                 cells[0].innerHTML=TagId;
                 var cnt;
                 var imgname;
                 var tag=TagType.toUpperCase(); 
         
                 imgname=GetDeviceImage(tag,"1");
         
                 cnt="<table cellpadding='10' style='padding-left: 20px;'><tr><td><img src='Images/" + imgname + "'/></td><td>" + TagType +"</td></tr></table>";
                 cells[1].innerHTML=  cnt; 
         
                 if(LessThen30Days > 0)
                    cnt="<table cellpadding='10' style='padding-left: 20px;'><tr><td>< 30 Days</td><td>" + s30DaysCell + "</td></tr></table>";
                 else if(LessThen90Days > 0)        
                    cnt="<table cellpadding='10' style='padding-left: 20px;'><tr><td>< 90 Days</td><td>" + s90DaysCell + "</td></tr></table>";
                 else
                    cnt="<table cellpadding='10' style='padding-left: 35px;'><tr><td>Good</td><td>" + sgood + "</td></tr></table>";
                  
                 if (BatteryReplaced=='') 
                      BatteryReplaced='';
                 
                  var replacementdt = BatteryReplaced.split(" "); 
                 document.getElementById("txtDate").value = replacementdt;
                  
                 cells[4].innerHTML=cnt; 
                 document.getElementById("divBatteryReplacementTable").style.display="";
                 document.getElementById("divBatteryReplacementSubmit").style.display="none";                 
                 $("#txtDate").focus();
                 
                 $('#txtDate').on('paste', function(e) {
                  if(document.getElementById("divBatteryReplacementTable").style.display=="")
                   {
                        e.preventDefault();
                        var pastedText = '';
                        if (window.clipboardData && window.clipboardData.getData) { // IE
                            pastedText = window.clipboardData.getData('Text');
                          } else if (e.clipboardData && e.clipboardData.getData) {
                            pastedText = e.clipboardData.getData('text/plain');
                          }
                        //this.value = pastedText.replace(/\D/g, '');
                        var fDate = new Date();
                        document.getElementById('txtDate').value= (fDate.getMonth() + 1) + '/' +  fDate.getDate() + '/' + fDate.getFullYear();
                        $('#txtcmnt').focus();
                        document.getElementById("btnsave").disabled=false;
                         document.getElementById("btnsave").style.color="#FFFFFF";
                   }
                });
                    
                $('#txtDate').keydown(function(e){
                    if (e.keyCode == 13) { // barcode scanned!
                        var fDate = new Date();
                            document.getElementById('txtDate').value= (fDate.getMonth() + 1) + '/' +  fDate.getDate() + '/' + fDate.getFullYear();
                        $('#txtcmnt').focus();
                         document.getElementById("btnsave").disabled=false;
                          document.getElementById("btnsave").style.color="#FFFFFF";
                        return false; // block form from being submitted yet
                    }
                    else
                    {
                        e.preventDefault();
                    }
                });
             }
       }

    }
}


var g_Obj;
var g_Bin = 0;

function LoadBatteryTechTaginforamtion(siteid,SortColumn,SortOrder)
{ 
    document.getElementById("divLoadBattery").style.display="";            
    
    g_Obj = CreateTagXMLObj();     
           
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxBatteryTechTagList;        
     
        DbConnectorPath = "AjaxConnector.aspx?cmd=GetLBIListForBatteryTech&sid="+ siteid+"&sortcolumn="+SortColumn+"&sortorder="+SortOrder;
      
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
var g_BatteryTechTagRoot;
function ajaxBatteryTechTagList()
{
    if(g_Obj.readyState==4)
    {
        if(g_Obj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Obj.responseXML.documentElement);
            g_BatteryTechTagRoot = g_Obj.responseXML.documentElement;                         
            MakeBatteryTechTagList();
        }
    }
}
function MakeBatteryTechTagList()
{
    var sTbl,sTblLen;         
    
    sTbl=document.getElementById('tblBatteryTechLbiList');
      
    sTblLen=sTbl.rows.length;
    clearTableRows(sTbl,sTblLen);    
  
    $('#tblBatteryTechLbiList').css('width','100%');
   
    var o_TagId=g_BatteryTechTagRoot.getElementsByTagName('TagId');
    var o_TagType=g_BatteryTechTagRoot.getElementsByTagName('TagType');  
    var o_TagName=g_BatteryTechTagRoot.getElementsByTagName('TagTypeName');  
    var o_RoomName=g_BatteryTechTagRoot.getElementsByTagName('Location');
    var o_InLocationHours=g_BatteryTechTagRoot.getElementsByTagName('InLocationHrs');
    var o_CatastrophicCases=g_BatteryTechTagRoot.getElementsByTagName('CatastrophicCases');
    var o_Floor=g_BatteryTechTagRoot.getElementsByTagName('Floor');
         
    nRootLength=o_TagId.length;
           
    if(nRootLength == 0)
    {     
        $(document.getElementById("tblBatteryTechLbiList")).html("");
        row=document.createElement('tr');
        AddCell(row,"No LBI Tags Found.", "siteOverview_cell_Full",8,"","center","890px","40px","");
        sTbl.appendChild(row);
        document.getElementById("divLoadBattery").style.display="none";  
       
        return;
    }  
    
  if(nRootLength >0)   { 
                   
        /*row=document.createElement('tr');       
      
        AddCellForSorting(row,"Tag Type",'siteOverview_TopLeft_Box',0,"","center","60px","40px","","TagType",SortColumn,SortImg,enumSortingArr.BatteryTechView); 
        AddCellForSorting(row,"Item Name",'siteOverview_Box',0,"","center","60px","40px","","TagTypeName",SortColumn,SortImg,enumSortingArr.BatteryTechView);           
        AddCellForSorting(row,"Tag Id",'siteOverview_Box',"",0,"center","60px","40px","","TagId",SortColumn,SortImg,enumSortingArr.BatteryTechView);
        AddCellForSorting(row,"Location",'siteOverview_Box',0,"","center","30px","40px","","Location",SortColumn,SortImg,enumSortingArr.BatteryTechView);    
        AddCellForSorting(row,"In Location",'siteOverview_Box',0,"","center","60px","40px","","InLocationDays",SortColumn,SortImg,enumSortingArr.BatteryTechView);       
                        
        sTbl.appendChild(row);
        
        var sRightClass;*/
        
        
        
        var htmlString = "<thead><tr>";
        htmlString = htmlString + "<th style='cursor:pointer;' onclick=sortBatteryTechView('TagType');>Tag Type "; 
        if(SortColumn == 'TagType')
	       htmlString = htmlString + "&nbsp;" + SortImg;
	    htmlString = htmlString + "</th>";
	    
	    htmlString = htmlString + "<th style='cursor:pointer;' onclick=sortBatteryTechView('TagTypeName');>Item Name "; 
        if(SortColumn == 'TagTypeName')
	       htmlString = htmlString + "&nbsp;" + SortImg;
	    htmlString = htmlString + "</th>";
	    
	    htmlString = htmlString + "<th style='cursor:pointer;' onclick=sortBatteryTechView('TagId');>Tag Id "; 
        if(SortColumn == 'TagId')
	       htmlString = htmlString + "&nbsp;" + SortImg;
	    htmlString = htmlString + "</th>";
	    
	    htmlString = htmlString + "<th style='cursor:pointer;' onclick=sortBatteryTechView('Floor');>Floor"; 
        if(SortColumn == 'Floor')
	       htmlString = htmlString + "&nbsp;" + SortImg;
	    htmlString = htmlString + "</th>";
	    
	    htmlString = htmlString + "<th style='cursor:pointer;' onclick=sortBatteryTechView('Location');>Location "; 
        if(SortColumn == 'Location')
	       htmlString = htmlString + "&nbsp;" + SortImg;
	    htmlString = htmlString + "</th>";
	    
	    htmlString = htmlString + "<th style='cursor:pointer;' onclick=sortBatteryTechView('InLocationDays');>In Location For"; 
        if(SortColumn == 'InLocationDays')
	       htmlString = htmlString + "&nbsp;" + SortImg;
	    htmlString = htmlString + "</th></tr></thead><tbody>";  
    	
        for(var i=0; i<nRootLength; i++)
        {         
           
            var TagId=(o_TagId[i].textContent || o_TagId[i].innerText || o_TagId[i].text);
            var TagName=(o_TagName[i].textContent || o_TagName[i].innerText || o_TagName[i].text);
            var TagType=(o_TagType[i].textContent || o_TagType[i].innerText || o_TagType[i].text);
            var RoomName=(o_RoomName[i].textContent || o_RoomName[i].innerText || o_RoomName[i].text);
            var InLocationHours=(o_InLocationHours[i].textContent || o_InLocationHours[i].innerText || o_InLocationHours[i].text);
            var CatastrophicCases=(o_CatastrophicCases[i].textContent || o_CatastrophicCases[i].innerText || o_CatastrophicCases[i].text);
            var Floor=(o_Floor[i].textContent || o_Floor[i].innerText || o_Floor[i].text);
                                
            /*row=document.createElement('tr');
                            
            AddCell(row,TagType,'DeviceList_leftBox',"","","center","60px","1px","");
            
            AddCell(row,TagName,'siteOverview_cell',"","","center","60px","1px","");                
                 
            if (CatastrophicCases == "1" || CatastrophicCases == "2")
            {
               sRightClass='clsBatteryTech_Lbi';
            }
            else if(CatastrophicCases == "4")
            {
                sRightClass='clsBatteryTech_UnderWatch';
            }         
                    
            AddCell(row,TagId, sRightClass,"","","center","60px","1px","");
                                  
            AddCell(row,RoomName,'siteOverview_cell',"","","center","30px","1px","");   
            
            AddCell(row,InLocationHours,'siteOverview_cell',"","","center","60px","1px","");         
                                      
            sTbl.appendChild(row);   */
            
            
            if (CatastrophicCases == "1" || CatastrophicCases == "2")
            {
                sRightClass= "style='background-color:#FF4E45;color:#FFFFFF;font-weight:bold;'";
            }
            else if(CatastrophicCases == "4")
            {
               sRightClass= "style='background-color:#FFFF88;color:#000000;font-weight:bold;'";
            }        
            
            if(i%2 == 0)
            {
                htmlString = htmlString + "<tr class='grid' style='height:27px;'><td>" + setundefined(TagType) + "</td><td>" + setundefined(TagName) + "</td><td " +  sRightClass + ">" + setundefined(TagId) + "</td><td>" + setundefined(Floor) + "</td><td>" + setundefined(RoomName) + "</td><td>" + setundefined(InLocationHours) + "</td></tr>";
            }
            else
            {
                htmlString = htmlString + "<tr class='gridAlternada' style='height:27px;'><td>" + setundefined(TagType) + "</td><td>" + setundefined(TagName) + "</td><td " + sRightClass + ">" + setundefined(TagId) + "</td><td>" + setundefined(Floor) + "</td><td>" + setundefined(RoomName) + "</td><td>" + setundefined(InLocationHours) + "</td></tr>";
            }
        }
        
       // sTbl.innerHTML = htmlString;
        $(document.getElementById("tblBatteryTechLbiList")).html(htmlString);
        $("#tblBatteryTechLbiList").freezeHeader({ 'height': '510px' });
        
         setTimeout(function(){
               $("#tblBatteryTechLbiList").scroll();
            }, 300);
        
    }
    else
    {
        $(document.getElementById("tblBatteryTechLbiList")).html("");
        row=document.createElement('tr');
        AddCell(row,"No Record Found.", 'siteOverview_cell_Full',6,"","center","700px","40px","");
        sTbl.appendChild(row);        
    }
    
    document.getElementById("divLoadBattery").style.display="none";
 }
 
function sortBatteryTechView(sortCol)
{ 
    siteid=document.getElementById("ctl00_headerBanner_drpsitelist").value;   
            
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
    
    LoadBatteryTechTaginforamtion(siteid,SortColumn,SortOrder)
}


function DownLoadExcelForBatteryTech(siteid)
{

     
    if(g_Obj == null)
    {
        g_Obj = CreateTagXMLObj();
    }
          
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxDownloaddBatteryTechTaginforamtion;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadExcelForBatteryTech&sid="+ siteid;
      
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

function ajaxDownloaddBatteryTechTaginforamtion()
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
                
                document.getElementById("divLoadBattery").style.display = "none";
            }
        }
    }
}

function DownLoadExcelForBatteryTech_ForIE(siteid)
{
    location.href = "AjaxConnector.aspx?cmd=LoadExcelForBatteryTech_IE&sid="+ siteid;
}

function GetDeviceImage(strtype,type)
{
 if (type=="1")
  {
      switch(strtype)
      {
         case "ASSET TAG":
                     imgname = "Asset_tag.png";
                     break;
         case "MM ASSET TAG":
                      imgname = "mm_assetTag.png";
                      break;
         case "STAFF TAG":
                      imgname = "Staff_tag.png"; 
                      break;
         case "MM STAFF TAG":
                      imgname = "mm_Staff_tag.png";
                      break;
         case "TEMP TAG":
                      imgname = "Temperature_icon.png";
                      break;
         case "PATIENT TAG":
                      imgname = "patient_tag.png";
                      break;
         case "ERU TAG":
                      imgname = "callpoints_tag.png"; 
                      break;
         case "HUMIDITY TAG":
                      imgname = "Temperature_icon.png";
                      break;  
         case "2G TEMP TAG":
                      imgname = "Temperature2_icon.png";
                      break;  
         case "HHC/STAFF TAG":
                 imgname = "Staff_tag.png"; 
                 break;
      } 
    }
    else
       {
         switch(strtype)
         {
          case "REGULAR MONITOR":
                     imgname = "infrastructure.png";
                     break;
          case "MM MONITOR":
                      imgname = "infrastructure.png";
                      break;
          case "LF EXCITER":
                      imgname = "LF-Excitiers.png"; 
                      break;
          case "DIM":
                      imgname = "Dimss.png";            
                      break;
       }            
    }
    
    var rtnimgName=imgname;
    return rtnimgName;  
}

//On Tag Add Button Click
$(document).on('click','#btnAddLbiList',function(){  
    $("#ifrmLbiUpload").attr("src","uploadFile.aspx?Cmd=AddLbiListForBatteryTech&SiteId=" + $("#drpSelSite").val());
        
    //Open Dialog
    $( "#dialog-Tag" ).dialog({
        height: 425,
        width: 600,
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

//On Tag Add Button Click
$(document).on('click','#btnAddFloor',function(){  
    $("#ifrmUploadExcel").attr("src","uploadFile.aspx?Cmd=AddFloorForBatteryTech&SiteId=" + $("#drpSelSite").val());
        
    //Open Dialog
    $( "#dialog-UploadExcelFile" ).dialog({
        height: 425,
        width: 600,
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

//On Tag Add Button Click
   $(document).on('change', '#drpSelSite', function (){
    $("#ifrmLbiUpload").attr("src","uploadFile.aspx?Cmd=AddLbiListForBatteryTech&SiteId=" + $("#drpSelSite").val());  
});

function LoadExcelReportForBatteryTech(siteid,FromDate,ToDate)
{
 
    if(g_Obj == null)
    {
        g_Obj = CreateTagXMLObj();
    }
          
    if(g_Obj!=null)
    {
        g_Obj.onreadystatechange = ajaxDownloaddBatteryTechTaginforamtion;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadExcelReportForBatteryTech&sid="+ siteid +"&fromdate="+FromDate+"&todate="+ToDate+"&SiteName="+SiteName;
      
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

function LoadExcelReportForBatteryTech_IE(siteid,FromDate,ToDate)
{
    location.href = "AjaxConnector.aspx?cmd=LoadExcelReportForBatteryTech_IE&sid="+ siteid +"&fromdate="+FromDate+"&todate="+ToDate+"&SiteName="+SiteName;
}
