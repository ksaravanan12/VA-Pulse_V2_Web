// JScript File

function CreateStarXMLObj() {

    var starobj = null;

    if (window.ActiveXObject) {
        try {
            starobj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                starobj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                starobj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
        starobj = new XMLHttpRequest();
        starobj.overrideMimeType('text/xml');
    }
    return starobj;
}

function doLoadStar(siteid, alertId, bin, curpag, typeId) {
    Load_Star_inforamtion(siteid, alertId, bin, curpag, typeId)
}

 var star_Obj;
 var g_Bin = 0;

 function Load_Star_inforamtion(siteid, alertId, bin, curpag, typeId) {

     star_Obj = CreateStarXMLObj();

     g_Bin = bin;

     if (star_Obj != null) {

         star_Obj.onreadystatechange = ajaxStarList;

         DbConnectorPath = "AjaxConnector.aspx?cmd=StarList&sid=" + siteid + "&alertId=" + alertId + "&Bin=" + bin + "&curpage=" + curpag + "&typId=" + typeId;

         if (GetBrowserType() == "isIE") {
             star_Obj.open("GET", DbConnectorPath, true);
         }
         else if (GetBrowserType() == "isFF") {
             star_Obj.open("GET", DbConnectorPath, true);
         }

         star_Obj.send(null);
     }

     return false;
 }

 var g_StarRoot;

 function ajaxStarList() {

     if (star_Obj.readyState == 4) {

         if (star_Obj.status == 200) {

             //Ajax Msg Receiver
             AjaxMsgReceiver(star_Obj.responseXML.documentElement);

             g_StarRoot = star_Obj.responseXML.documentElement;

             loadStarList(0)

             g_StarAllDeviceRoot = g_StarRoot;
         }
     }
 }

//For Search
 function Load_Star_inforamtionSearch(siteid, alertId, bin, curpag, typeId, deviceIds) {

     $.post("AjaxConnector.aspx?cmd=StarListSearch",
      {
          sid: siteid,
          alertId: alertId,
          Bin: bin,
          curpage: curpag,
          typId: typeId,
          DeviceId: deviceIds
      },
      function (data, status) {

          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              g_StarRoot = data.documentElement;
              loadStarList(1)
          }
          else {
              document.getElementById("divLoading").style.display = "none";
          }
      });
 }

 function loadStarList(isSearch) {

     var sTbl, sTblLen;

     if (isSearch == 0)
         sTbl = document.getElementById('tblStarInfo');
     else
         sTbl = document.getElementById('tblStarInfoSearch');

     sTblLen = sTbl.rows.length;
     clearTableRows(sTbl, sTblLen);

     var o_UserRole = g_StarRoot.getElementsByTagName('UserRole')
     var o_siteId = g_StarRoot.getElementsByTagName('SiteId')
     var o_SiteName = g_StarRoot.getElementsByTagName('SiteName')
     var o_TotalPage = g_StarRoot.getElementsByTagName('TotalPage')
     var o_TotalCount = g_StarRoot.getElementsByTagName('TotalCount')
     var o_offline = g_StarRoot.getElementsByTagName('offline')
     var o_CompanyName = g_StarRoot.getElementsByTagName('CompanyName')
     var o_StarId = g_StarRoot.getElementsByTagName('StarId')
     var o_MACId = g_StarRoot.getElementsByTagName('MACId')
     var o_DeviceName = g_StarRoot.getElementsByTagName('DeviceName')
     var o_StarType = g_StarRoot.getElementsByTagName('StarType')
     var o_IPAddr = g_StarRoot.getElementsByTagName('IPAddr')
     var o_FileVersion = g_StarRoot.getElementsByTagName('FileVersion')
     var o_Version = g_StarRoot.getElementsByTagName('Version')
     var o_LastReceivedTime = g_StarRoot.getElementsByTagName('LastReceivedTime')
     var o_ModelNumber = g_StarRoot.getElementsByTagName('ModelItem');

     nRootLength = o_siteId.length;

     var hidval;

     hideRecalibration();

     if (isSearch == 0) 
         hidval = document.getElementById('hid_Show').value;     
     else 
         hidval = document.getElementById('hdn_Search_Show').value;
     
     if (isSearch == 1) {

         document.getElementById("btnExportSearch").style.display = "none";

         if (nRootLength > 0)
             document.getElementById("btnExportSearch").style.display = "";
     }

     //sitename
     if (nRootLength > 0) {

         //User Role
         var UserRole = (o_UserRole[0].textContent || o_UserRole[0].innerText || o_UserRole[0].text);

         if (isSearch != 1) {
             document.getElementById("trDeviceListRow").style.display = "";
             document.getElementById("tdPagination").style.display = "";
         }

         document.getElementById("btnSearchDeviceList").style.display = "";
         document.getElementById("btnExportExcel").disabled = false;

         row = document.createElement('tr');

         if (hidval == 1) {

             var chk_Allbox = "<input type='checkbox' id='chkAll' name='chkAll' style='vertical-align: middle;'>";

             AddCell(row, chk_Allbox, 'siteOverview_TopLeft_Box', "", "", "center", "20px", "40px", "");
             AddCell(row, "Star Id", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
         }
         else 
             AddCell(row, "Star Id", 'siteOverview_TopLeft_Box', "", "", "center", "100px", "40px", "");
         
         AddCell(row, "Mac Id", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
         AddCell(row, "Star Name", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
         AddCell(row, "Star Type", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
         AddCell(row, "IP Addr", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
         AddCell(row, "Version", 'siteOverview_Box', "", "", "center", "100px", "40px", "");
         AddCell(row, "Date Last seen</br>by Network", "siteOverview_Box", "", "", "center", "100px", "40px", "");
         AddCell(row, "Model Item", "siteOverview_Topright_Box", "", "", "center", "100px", "40px", "");

         sTbl.appendChild(row);

         if (isSearch == 0) {

             //totalcount
             var ttcnt_lable = document.getElementById("ctl00_ContentPlaceHolder1_lbltotalcount")
             ttcnt_lable.innerHTML = "Total Devices: " + (o_TotalCount[0].textContent || o_TotalCount[0].innerText || o_TotalCount[0].text);

             //Totalpage
             var ttPage_lable = document.getElementById("ctl00_ContentPlaceHolder1_lblTotalpage")
             ttPage_lable.innerHTML = " of " + (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);

             var MaxPageCnt = (o_TotalPage[0].textContent || o_TotalPage[0].innerText || o_TotalPage[0].text);
             doStarEnableButton(MaxPageCnt);
         }

         for (var i = 0; i < nRootLength; i++) {

             var siteid = (o_siteId[i].textContent || o_siteId[i].innerText || o_siteId[i].text);
             var SiteName = (o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);
             var TotalPage = (o_TotalPage[i].textContent || o_TotalPage[i].innerText || o_TotalPage[i].text);
             var TotalCount = (o_TotalCount[i].textContent || o_TotalCount[i].innerText || o_TotalCount[i].text);
             var offline = (o_offline[i].textContent || o_offline[i].innerText || o_offline[i].text);
             var CompanyName = (o_CompanyName[i].textContent || o_CompanyName[i].innerText || o_CompanyName[i].text);
             var StarId = (o_StarId[i].textContent || o_StarId[i].innerText || o_StarId[i].text);
             var MACId = (o_MACId[i].textContent || o_MACId[i].innerText || o_MACId[i].text);
             var DeviceName = setundefined(o_DeviceName[i].textContent || o_DeviceName[i].innerText || o_DeviceName[i].text);
             var StarType = (o_StarType[i].textContent || o_StarType[i].innerText || o_StarType[i].text);
             var IPAddr = (o_IPAddr[i].textContent || o_IPAddr[i].innerText || o_IPAddr[i].text);
             var FileVersion = (o_FileVersion[i].textContent || o_FileVersion[i].innerText || o_FileVersion[i].text);
             var Version = (o_Version[i].textContent || o_Version[i].innerText || o_Version[i].text);
             var LastReceivedTime = (o_LastReceivedTime[i].textContent || o_LastReceivedTime[i].innerText || o_LastReceivedTime[i].text);
             var ModelNumber = (o_ModelNumber[i].textContent || o_ModelNumber[i].innerText || o_ModelNumber[i].text);

             row = document.createElement('tr');

             if (hidval == 1) {

                 var chk_box = "<input type ='checkbox'  id='chk_hid' name='chk_hid' value='" + MACId + "' />";

                 AddCell(row, chk_box, 'DeviceList_leftBox', "", "", "center", "20px", "40px", "");
                 AddCell(row, StarId, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
             }
             else 
                 AddCell(row, StarId, 'DeviceList_leftBox', "", "", "center", "100px", "40px", "");
             
             if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support) {
                 var href = "<a  class='DeviceDetailsLink' href=#deviceDetail onclick=loadDeviceDetailsInfoOnClick(" + siteid + ",3,'" + MACId + "')>" + MACId + "</a>";
                 AddCell(row, href, 'siteOverview_cell', "", "", "center", "170px", "40px", "");
             }
             else 
                 AddCell(row, MACId, 'siteOverview_cell', "", "", "center", "170px", "40px", "");
             
             var sLocation = "";

             if (DeviceName != "") 
                 sLocation = "<a class='DeviceDetailsLink' title='Update Star Location' onclick=OpenLocationChangeDialog(3,\&quot;" + MACId + "\&quot;,\&quot;" + encodeURIComponent(DeviceName) + "\&quot;)>" + DeviceName + "</a>";
             else 
                 sLocation = "<img style='cursor: pointer; width:18px;' alt='Add Star Location' title='Add Star Location' src='images/img_edit.png' onclick='OpenLocationChangeDialog(3,\&quot;" + MACId + "\&quot;,\&quot;" + encodeURIComponent(DeviceName) + "\&quot;);' />"
             
             if (UserRole == enumUserRoleArr.Admin || UserRole == enumUserRoleArr.Engineering || UserRole == enumUserRoleArr.Support || UserRole == enumUserRoleArr.Partner)
                 AddCell(row, sLocation, 'siteOverview_cell', "", "", "center", "150px", "40px", "");
             else
                 AddCell(row, DeviceName, 'siteOverview_cell', "", "", "center", "170px", "40px", "");

             AddCell(row, StarType, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
             AddCell(row, IPAddr, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
             AddCell(row, Version, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
             AddCell(row, LastReceivedTime, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
             AddCell(row, ModelNumber, 'siteOverview_cell', "", "", "center", "100px", "40px", "");
             sTbl.appendChild(row);
         }
     }
     else {

         if (isSearch != 1) {
             document.getElementById("trDeviceListRow").style.display = "";
         }

         document.getElementById("btnSearchDeviceList").style.display = "";

         row = document.createElement('tr');
         AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 7, "", "center", "700px", "40px", "");
         sTbl.appendChild(row);
     }

     document.getElementById("divLoading").style.display = "none";

     if (isSearch == 0) {

         document.getElementById("btnExportExcel").disabled = false;

         if (nRootLength === 0) {
             document.getElementById("btnExportExcel").disabled = true;
         }
     }

     try {
         PageVisitDetails(g_UserId, "Home - Star List", enumPageAction.View, $("#subHeader").text() + " list viewed in site " + $("#ctl00_ContentPlaceHolder1_lblsitename").text());
     }
     catch (e) {

     }
 }

 function doStarEnableButton(MaxPageCnt) {

     var curnPage = document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo")
     var curPage = curnPage.value;

     if (MaxPageCnt == "1" || Number(MaxPageCnt) == 1) {

         document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1";

         document.getElementById("btnNext").style.display = "none";
         document.getElementById("btnPrev").style.display = "none";
     }
     else {
         document.getElementById("btnNext").style.display = "";
         document.getElementById("btnPrev").style.display = "";
     }

     if (Number(curPage) <= 1) {

         document.getElementById("ctl00_ContentPlaceHolder1_txtPageNo").value = "1"
         document.getElementById("btnPrev").style.display = "none";
     }

     if (Number(curPage) >= Number(MaxPageCnt)) {
         document.getElementById("btnNext").style.display = "none";
     }
 }
