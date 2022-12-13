function CreateInfraXMLObj() {

    var infraobj = null;
    
    if (window.ActiveXObject) {
    
        try {
            infraobj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                infraobj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                infraobj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
    
        infraobj = new XMLHttpRequest();
        infraobj.overrideMimeType('text/xml');
    }
    
    return infraobj;
}

var inf_Obj;
var g_IsAdd;

function AddCompanySetup(IsAdd, CompanyId, CompanyName, Status, AuthPassword) {

    g_IsAdd = IsAdd;

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=AddCompanySetup",
    {
        isAdd: IsAdd,
        CompanyId: CompanyId,
        CompanyName: CompanyName,
        Status: Status,
        AuthPassword: AuthPassword
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddCompany(dsRoot);
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function ajaxAddCompany(dsRoot) {

    var o_Result = dsRoot.getElementsByTagName('Result');
    var o_Msg = dsRoot.getElementsByTagName('Msg');

    if (o_Result.length > 0) {

        var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);
        var Result = (o_Result[0].textContent || o_Result[0].innerText || o_Result[0].text);

        if (g_IsAdd == "1" || g_IsAdd == "2") {

            if (Result == "0") {
                document.getElementById("tdError").innerHTML = msg;
                doAfterAddedCompany();
            }
            else {
                document.getElementById("tdError").innerHTML = msg;
            }
        }
        else {
            document.getElementById("tdAddCompany").style.display = "none";
            doAfterAddedCompany();
        }
    }
    else {
        document.getElementById("tdError").innerHTML = "Error in Add";
    }

    document.getElementById("divLoading").style.display = "none";
}

function Load_Setup_Company() {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {
    
        inf_Obj.onreadystatechange = ajaxCompanyList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=CompanyList";

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
	
        inf_Obj.send(null);
    }
    return false;
}

function ajaxCompanyList() {

    if (inf_Obj.readyState == 4) {
    
        if (inf_Obj.status == 200) {
	
            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblCompanyListInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblCompanyListInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblCompanyListInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);
            var dsRoot = inf_Obj.responseXML.documentElement;
            var o_CompanyId = dsRoot.getElementsByTagName('CompanyId')
            var o_CompanyName = dsRoot.getElementsByTagName('CompanyName') 
            var o_Count2X = dsRoot.getElementsByTagName('Count2X')
            var o_UserCount = dsRoot.getElementsByTagName('UserCount')
            var o_Status = dsRoot.getElementsByTagName('Status')
           
            nRootLength = o_CompanyId.length;

            //sitename
            if (nRootLength > 0) {
                var head = "";               
                
                head = "<thead style='height:42px;'><tr style='height:42px;'>" + 
                            "<th class='siteOverview_TopLeft_Box' style='padding-right: 17px;'>Id </th>" + 
                            "<th class='siteOverview_Box'>Company&nbsp;Name</th>" +             
                            "<th class='siteOverview_Box'>2X&nbsp;Sites</th>" + 
                            "<th class='siteOverview_Box'>User&nbsp;Count</th>" + 
                            "<th class='siteOverview_Box'>Status</th>" + 
                            "<th class='siteOverview_Topright_Box' style='padding-right: 17px;'>Edit</th>" + 
                       "</tr></thead>";                

                sTbl.innerHTML = head;
              
                body = document.createElement('tbody');

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');

                    var CompanyId = (o_CompanyId[i].textContent || o_CompanyId[i].innerText || o_CompanyId[i].text);
                    var CompanyName = (o_CompanyName[i].textContent || o_CompanyName[i].innerText || o_CompanyName[i].text);                   
                    var Count2X = (o_Count2X[i].textContent || o_Count2X[i].innerText || o_Count2X[i].text);
                    var UserCount = (o_UserCount[i].textContent || o_UserCount[i].innerText || o_UserCount[i].text);
                    var Status = (o_Status[i].textContent || o_Status[i].innerText || o_Status[i].text);

                    var strStatus = "InActive";

                    if(Status ==  "True")
                      strStatus = "Active";
                  
                    AddCell(row, CompanyId, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, CompanyName, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");                  
                    AddCell(row, Count2X, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, UserCount, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, strStatus, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, "<img id='editConfig-" + i + "' src='images/imgEdit.png' onclick=showCompanyConfigurationForEdit(" + CompanyId + ",'" + CompanyName.replace(new RegExp(" ", "gm"), "_") + "','" + Status + "'); onmouseover='btnPrev_LAServiceOver(this);' onmouseout='btnPrev_LAServiceOut(this);' />", "siteOverview_Box_wrap", "", "", "center", "70px", "30px", "8px");                        
                  
                    body.appendChild(row);
                }
                
                sTbl.appendChild(body);
            }
            else
             {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);
            }

            DatatablesLoadGraph("#tblCompanyListInfo", [8], 0, "asc", 10);   
                   
            document.getElementById("divLoading").style.display = "none";
        } 
    } 
}

function DeleteConfigurationSetting(siteId, AuthUserId) {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = AfterDeleteAjax;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=DeleteSiteConfiguration&sid=" + siteId + "&AuthUserId=" + AuthUserId;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }
        inf_Obj.send(null);
    }
    return false;
}

function AfterDeleteAjax() {

    if (inf_Obj.readyState == 4) {
    
        if (inf_Obj.status == 200) {
	
            doAfterAddedCompany();
        } 
   } 
}

