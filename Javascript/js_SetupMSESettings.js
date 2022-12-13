// JScript File

function CreateInfraXMLObj() {

    var infraobj = null;

    if (window.ActiveXObject) {
        try {
            infraobj = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                infraobj = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e1) {
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

function Load_Setup_MSESettings() {

    $("#tblMSESettingsHistoryListInfo").empty();
    $("#tblMSESettingsHistoryListInfo").parent().parent().prev().prev().prev().hide();
    $("#tblMSESettingsHistoryListInfo").parent().parent().prev().prev().hide();
    $("#tblMSESettingsHistoryListInfo").parent().parent().prev().hide();

    var SiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();
    
    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = ajaxMSESettingsList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=MSESettingsList&SiteId=" + SiteId;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        } else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }

        inf_Obj.send(null);
    }
    return false;
}

function ajaxMSESettingsList() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblMSESettingsListInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblMSESettingsListInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblMSESettingsListInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);
            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_SiteId = dsRoot.getElementsByTagName('SiteId')
            var o_TagTracking = dsRoot.getElementsByTagName('TagTracking')
            var o_ClientTracking = dsRoot.getElementsByTagName('ClientTracking')
            var o_QueryInterval = dsRoot.getElementsByTagName('QueryInterval')

            nRootLength = o_SiteId.length;

            var head = "";

            head = "<thead><tr><th class='siteOverview_TopLeft_Box_wrap' style='width:300px; 'height:30px;>Tag Tracking</th><th class='siteOverview_Box' style='width:200px; height:30px;'>Client Tracking</th>" +
                   "<th class='siteOverview_Box' style='width:200px; height:30px;'>Query Interval</th>" +
                   "</tr></thead>";

            sTbl.innerHTML = head;

            if (nRootLength > 0) {

                body = document.createElement('tbody');

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');
                    var SiteId = setundefined(o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
                    var TagTracking = setundefined(o_TagTracking[i].textContent || o_TagTracking[i].innerText || o_TagTracking[i].text);
                    var ClientTracking = setundefined(o_ClientTracking[i].textContent || o_ClientTracking[i].innerText || o_ClientTracking[i].text);
                    var QueryInterval = setundefined(o_QueryInterval[i].textContent || o_QueryInterval[i].innerText || o_QueryInterval[i].text);

                    var sTagTracking = strMSE(TagTracking);
                    var sClientTracking = strMSE(ClientTracking);

                    AddCell(row, sTagTracking, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
                    AddCell(row, sClientTracking, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
                    AddCell(row, QueryInterval, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");

                    body.appendChild(row);
                }

                sTbl.appendChild(body);
                var SiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();

                if (SiteId != "0") {
                    Load_Setup_MSESettingsHistory(SiteId);
                }
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        }
    } 
}

function Load_Setup_MSESettingsHistory(SiteId, imgid) {

    $(".siteOverview_Box_UserLog").css({ "background-color": "", "color": "" });

    $(imgid).parent().css({ "background-color": "#245e90", "color": "#fff" });
    $(imgid).parent().siblings().css({ "background-color": "#245e90", "color": "#fff" });

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = ajaxMSESettingsHistoryList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=MSESettingsHistoryList&SiteId=" + SiteId;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        } else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }

        inf_Obj.send(null);
    }

    return false;
}

function ajaxMSESettingsHistoryList() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {
            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblMSESettingsHistoryListInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblMSESettingsHistoryListInfo');
            } else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblMSESettingsHistoryListInfo');
            }

            $("#tblMSESettingsHistoryListInfo").parent().parent().prev().prev().prev().show();
            $("#tblMSESettingsHistoryListInfo").parent().parent().prev().prev().show();
            $("#tblMSESettingsHistoryListInfo").parent().parent().prev().show();

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);
            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_SiteId = dsRoot.getElementsByTagName('SiteId')
            var o_TagTracking = dsRoot.getElementsByTagName('TagTracking')
            var o_ClientTracking = dsRoot.getElementsByTagName('ClientTracking')
            var o_QueryInterval = dsRoot.getElementsByTagName('QueryInterval')
            var o_Updatedon = dsRoot.getElementsByTagName('Updatedon')

            nRootLength = o_SiteId.length;

            var head = "";

            head = "<thead><tr><th class='siteOverview_TopLeft_Box_wrap' style='width:300px; 'height:30px;>Tag Tracking</th><th class='siteOverview_Box' style='width:200px; height:30px;'>Client Tracking</th>" +
                "<th class='siteOverview_Box' style='width:200px; height:30px;'>Query Interval</th><th class='siteOverview_Box' style='width:200px; height:30px;'>Updated on</th>" +
                "</tr></thead>";        

            sTbl.innerHTML = head;

            if (nRootLength > 0) {

                body = document.createElement('tbody');

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');

                    var SiteId = setundefined(o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
                    var TagTracking = setundefined(o_TagTracking[i].textContent || o_TagTracking[i].innerText || o_TagTracking[i].text);
                    var ClientTracking = setundefined(o_ClientTracking[i].textContent || o_ClientTracking[i].innerText || o_ClientTracking[i].text);
                    var QueryInterval = setundefined(o_QueryInterval[i].textContent || o_QueryInterval[i].innerText || o_QueryInterval[i].text);
                    var Updatedon = setundefined(o_Updatedon[i].textContent || o_Updatedon[i].innerText || o_Updatedon[i].text);

                    var sTagTracking = strMSE(TagTracking);
                    var sClientTracking = strMSE(ClientTracking);

                    AddCell(row, sTagTracking, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
                    AddCell(row, sClientTracking, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
                    AddCell(row, QueryInterval, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");
                    AddCell(row, Updatedon, "siteOverview_Box_UserLog", "", "", "left", "", "30px", "");

                    body.appendChild(row);
                }
                sTbl.appendChild(body);
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 4, "", "center", "700px", "40px", "");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        } 
    } 
}