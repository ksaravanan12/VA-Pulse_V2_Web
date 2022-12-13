// JScript File

//*******************************************************************
//	Function Name	:	CreateXMLObj
//	Input			:	None
//	Description		:	Create XML Object For Ajax call
//*******************************************************************
function CreateAlertXMLObj()
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

// Checking For Browser		
function GetBrowserType()
{
	var isIE = ((document.all)? true  : false ); //for Internet Explorer
	var isFF = ((document.getElementById && !document.all)? true: false ); //for Mozilla Firefox
	
	
	if(!(window.ActiveXObject) && "ActiveXObject" in window)
	{
	    return "isIE";
	}
	
	
	if (isIE)
	{
		return "isIE";
	}
	else if(isFF)
	{
		return "isFF";
	}
}

//*********************************************************
//	Function Name	:	LoadAlertInfo
//	Input			:	sDate
//	Description		:	ajax call LoadAlertInfo
//*********************************************************
var g_AlertObj;
function LoadAlertInfo(SiteId)
{   
    g_AlertObj = CreateAlertXMLObj();
    
    if(g_AlertObj!=null)
    {
        g_AlertObj.onreadystatechange = ajaxAlertList;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=AlertList&sid=" + SiteId;
      
        if(GetBrowserType()=="isIE")
        {	
            g_AlertObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_AlertObj.open("GET",DbConnectorPath, true);
        }
        g_AlertObj.send(null);         
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxAlertList
//	Input			:	sDate
//	Description		:	Load Alert List from ajax Response
//*********************************************************
function ajaxAlertList()
{
    if(g_AlertObj.readyState==4)
    {
        if(g_AlertObj.status==200)
        {
            var OldSiteId = 0 ;
            var sTbl,sTblLen;
            var sTblAlertInfo,sTblAlertInfoLen;
            
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_AlertObj.responseXML.documentElement);
            
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('siteAlertinfo');
                sTblAlertInfo = document.getElementById("tblAlertInfo");
            }
            else if(GetBrowserType()=="isFF")
            {
                sTblAlertInfo = document.getElementById("tblAlertInfo");
                sTbl=document.getElementById('siteAlertinfo');
            }
            sTblLen=sTbl.rows.length;
            sTblAlertInfoLen = sTblAlertInfo.rows.length;
            clearTableRows(sTbl,sTblLen);
            clearTableRows(sTblAlertInfo,sTblAlertInfoLen);
            
            document.getElementById("lblHeaderName").innerHTML = "Critical Alerts";
            
            var dsRoot = g_AlertObj.responseXML.documentElement;
         
            var o_SiteId=dsRoot.getElementsByTagName('SiteId');
            var o_Site=dsRoot.getElementsByTagName('Site');
            var o_DeviceType=dsRoot.getElementsByTagName('DeviceType');
            var o_AlertId=dsRoot.getElementsByTagName('AlertId');
            var o_AlertCount=dsRoot.getElementsByTagName('AlertCount');
            var o_Description=dsRoot.getElementsByTagName('Description');
            var o_Status=dsRoot.getElementsByTagName('Status');
         
            var nRootLength=o_SiteId.length;
            var sTbl=document.getElementById("siteAlertinfo");
                        
            if(nRootLength >0)
            {
                for(var i=0; i<nRootLength; i++)
                {
                    var SiteId=(o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
                    var Site=(o_Site[i].textContent || o_Site[i].innerText || o_Site[i].text);
                    var DeviceType=(o_DeviceType[i].textContent || o_DeviceType[i].innerText || o_DeviceType[i].text);
                    var AlertId=(o_AlertId[i].textContent || o_AlertId[i].innerText || o_AlertId[i].text);
                    var AlertCount=(o_AlertCount[i].textContent || o_AlertCount[i].innerText || o_AlertCount[i].text);
                    var Description=(o_Description[i].textContent || o_Description[i].innerText || o_Description[i].text);
                    var Status=(o_Status[i].textContent || o_Status[i].innerText || o_Status[i].text);
                    
                    if(SiteId != OldSiteId)
                    {
                        row=document.createElement('tr');
                        AddCell(row,"",'',3,"","center","","10px","");
                        sTbl.appendChild(row);
                        
                        row=document.createElement('tr');
                        AddCell(row,Site,'SHeader1',3,"","","","","");
                        sTbl.appendChild(row);
                        
                        row=document.createElement('tr');
                        AddCell(row,"",'bottomBorder',3,"","","","","");
                        sTbl.appendChild(row);
                    }
                    
                    var statusClsName = "alert_devicetype_Green";
                    var ncolspan  = 1;
                    var infoText = "";
                    
                    if(Status == 1)
                    {
                        statusClsName = "alert_devicetype_RED";
                    }

                    row=document.createElement('tr');
                    AddCell(row,DeviceType,statusClsName,"","","","","","");
                    
                    if(DeviceType.toLowerCase() == "tag")
                    {
                        infoText = "<a href=PatientTagList.aspx?alertId=" + AlertId + "&sid=" + siteid + " class='alert_normal_Blue' title='Offline Tag'>" + AlertCount + "</a>";
                        AddCell(row,infoText,'alert_normal_Blue',"","","","250px","20px","");
                    }
                    else if(DeviceType.toLowerCase() == "monitor")
                    {
                        infoText = "<a href=DeviceList1.aspx?alertId=" + AlertId + "&sid=" + siteid + " class='alert_normal_Blue' title='Offline Monitor'>" + AlertCount + "</a>";
                        AddCell(row,infoText,'alert_normal_Blue',"","","","","20px","");
                    }
                    else if(DeviceType.toLowerCase() == "star")
                    {
                        infoText = AlertCount
                        AddCell(row,infoText,'alert_normal_Blue',"","","","250px","20px","");
                    }
                    else
                    {
                        ncolspan = 2;
                    }
                    
                    AddCell(row,Description,'alert_Normal_Text',ncolspan,"","","250px","20px","");
                    sTbl.appendChild(row);
                    
                    OldSiteId = SiteId;
                }
            }
            else
            {
                row=document.createElement('tr');
                AddCell(row,"No Record Found.", 'siteOverview_cell_Full',6,"","center","700px","40px","");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading_DeviceDetail").style.display = "none";

            try {
                PageVisitDetails(g_UserId, "Home", enumPageAction.View, "Alert Viewed");
            }
            catch (e) {

            }         
        }
    }
}