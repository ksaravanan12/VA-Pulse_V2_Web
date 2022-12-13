//*********************************************************
//	Function Name	:	LoadTagMovements
//	Input			:	SiteId,DeviceId,FromDate,ToDate
//	Description		:	ajax call LoadTagMovements
//*********************************************************
var g_TObj;
function LoadTagMovements(SiteId,DeviceId,FromDate,ToDate)
{
    document.getElementById("divLoading_TagMovement").style.display = "";
    
    g_TObj = CreateXMLObj();
    
    if(g_TObj!=null)
    {
        g_TObj.onreadystatechange = ajaxTagMovements;
        
        DbConnectorPath = "AjaxConnector.aspx?cmd=TagMovements&Site=" + SiteId + "&DeviceId=" + DeviceId + "&FromDate=" + FromDate + "&ToDate=" + ToDate;
      
        if(GetBrowserType()=="isIE")
        {	
            g_TObj.open("GET",DbConnectorPath, true);
        } 
        else if(GetBrowserType()=="isFF")
        {	    
            g_TObj.open("GET",DbConnectorPath, true);
        }
        g_TObj.send(null);         
    }
    return false;
}

//*********************************************************
//	Function Name	:	ajaxTagMovements
//	Input			:	none
//	Description		:	Load Tag Movements Datas from ajax Response
//*********************************************************
function ajaxTagMovements()
{
    if(g_TObj.readyState==4)
    {
        if(g_TObj.status==200)
        {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_TObj.responseXML.documentElement);
            
            var sTbl,sTblLen;
            if(GetBrowserType()=="isIE")
            {
                sTbl=document.getElementById('tblTagMovementList');
            }
            else if(GetBrowserType()=="isFF")
            {
                sTbl=document.getElementById('tblTagMovementList');
            }
            sTblLen=sTbl.rows.length;
            clearTableRows(sTbl,sTblLen);
                
            var dsRoot = g_TObj.responseXML.documentElement;
             
            var o_isMoved=dsRoot.getElementsByTagName('IsMoved');
            var o_StartDateTime=dsRoot.getElementsByTagName('StartDateTime');
            var o_EndDateTime=dsRoot.getElementsByTagName('EndDateTime');
            var o_RoomIds=dsRoot.getElementsByTagName('RoomIds');
            var o_XmlData=dsRoot.getElementsByTagName('XmlData');
             
            nRootLength=o_isMoved.length;
            
            if(o_XmlData.length > 0)
            {
                /*XmlData = (o_XmlData[0].textContent || o_XmlData[0].innerText || o_XmlData[0].text);
                
                FusionCharts.setCurrentRenderer('javascript');
                var myChart = new FusionCharts("Gantt","myChartId","600","200","0");
                myChart.setXMLData(XmlData);
                myChart.render("divChart_TagMovement"); */
            }
            
             //Header
                row=document.createElement('tr');
                AddCell(row,"Start Date Time",'siteOverview_TopLeft_Box',"","","center","300px","40px","");
                AddCell(row,"End Date Time",'siteOverview_Box',"","","center","300px","40px","");
                AddCell(row,"Status",'siteOverview_Topright_Box',"","","center","100px","40px","");
                sTbl.appendChild(row);
             
            if(nRootLength >0)
            {
               
                
                //Datas
                for(var i=0; i<nRootLength; i++)
                {
                    var isMoved = (o_isMoved[i].textContent || o_isMoved[i].innerText || o_isMoved[i].text);
                    var StartDateTime = (o_StartDateTime[i].textContent || o_StartDateTime[i].innerText || o_StartDateTime[i].text);
                    var EndDateTime = (o_EndDateTime[i].textContent || o_EndDateTime[i].innerText || o_EndDateTime[i].text);
                    var RoomIds = (o_RoomIds[i].textContent || o_RoomIds[i].innerText || o_RoomIds[i].text);
                    
                    if(isMoved == "1")
                    {
                        isMoved = "Movement";
                    }
                    else
                    {
                        isMoved = "No Movement";
                    }
                     
                    row=document.createElement('tr');
                    AddCell(row,StartDateTime,'DeviceList_leftBox',"","","center","300px","40px","");
                    AddCell(row,EndDateTime,'siteOverview_cell',"","","center","300px","40px","");
                    AddCell(row,isMoved,'siteOverview_cell',"","","center","100px","40px","");
                    sTbl.appendChild(row);
                }
            }
            else
            {
                 row=document.createElement('tr');
                    AddCell(row,"No records found...",'siteOverview_cell_Full',"3","","center","300px","40px","");
                    sTbl.appendChild(row);
            }
            
            document.getElementById("divLoading_TagMovement").style.display = "none";
        }
    }
}


