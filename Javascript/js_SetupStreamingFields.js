// JScript File

function CreateInfraXMLObj() {

    var infraobj = null;

    if (window.ActiveXObject) {

        try {
            infraobj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
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

function Load_Setup_StreamingFields() {

    var SiteId = "";

    SiteId = $("#ctl00_ContentPlaceHolder1_ddlSites option:selected").val();

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = ajaxStreamingFieldsList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=StreamingFieldsList&SiteId=" + SiteId;

        if (GetBrowserType() == "isIE") {
            inf_Obj.open("GET", DbConnectorPath, true);
        } else if (GetBrowserType() == "isFF") {
            inf_Obj.open("GET", DbConnectorPath, true);
        }

        inf_Obj.send(null);
    }
    return false;
}

function ajaxStreamingFieldsList() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {
            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblStreamingFieldsListInfo");
            sTbl = document.getElementById('tblStreamingFieldsListInfo');          
            sTblLen = sTbl.rows.length;
            clearTableRows(sTbl, sTblLen);

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_StreamingIdx = dsRoot.getElementsByTagName('StreamingIdx')
            var o_IPAddress = dsRoot.getElementsByTagName('IPAddress')
            var o_Port = dsRoot.getElementsByTagName('Port')
            var o_Name = dsRoot.getElementsByTagName('Name')
            var o_TagFilter = dsRoot.getElementsByTagName('TagFilter')
            var o_MonitorFilter = dsRoot.getElementsByTagName('MonitorFilter')
            var o_TagFields = dsRoot.getElementsByTagName('TagFields')
            var o_TagBytes = dsRoot.getElementsByTagName('TagBytes')
            var o_MonitorFields = dsRoot.getElementsByTagName('MonitorFields')
            var o_MonitorBytes = dsRoot.getElementsByTagName('MonitorBytes')
            var o_HeaderFields = dsRoot.getElementsByTagName('HeaderFields')
            var o_HeaderBytes = dsRoot.getElementsByTagName('HeaderBytes')
            var o_WPSStreaming = dsRoot.getElementsByTagName('WPSStreaming')
            var o_TempGDDStream = dsRoot.getElementsByTagName('TempGDDStream')

            nRootLength = o_StreamingIdx.length;

            var streamingDetailsHTML = "";
            streamingDetailsHTML = "<table border='0'; cellpadding='0' cellspacing='0' style='width: 100%'>" +
                                        "<thead><tr><th class='streaming_Box' style='width:50px; height:30px;'>S.No</th>" +
                                        "<th class='streaming_Box' style='width: 200px; height:30px;'>IP Address/Name</th>" +
                                        "<th class='streaming_Box' style='width: 70px; height:30px;'>Type</th>" +
                                        "<th class='streaming_Box' style='width: 100px; height:30px;'>Filter</th>" +
                                        "<th class='streaming_Box' style='width: 600px; height:30px;'>Streaming Fields</th>" +
                                        "<th class='streaming_Box' style='width: 100px; height:30px;'>Bytes</th>" +
                                        "<th class='streaming_Box' style='width: 75px; height:30px;'>WPS Stream</th>" +
                                        "<th class='streaming_Box' style='width: 75px; height:30px;'>GDD Stream</th>" +
                                        "</tr></thead>"

            if (nRootLength > 0) {

                for (var i = 0; i < nRootLength; i++) {

                    var StreamingIdx = setundefined(o_StreamingIdx[i].textContent || o_StreamingIdx[i].innerText || o_StreamingIdx[i].text);
                    var IPAddress = setundefined(o_IPAddress[i].textContent || o_IPAddress[i].innerText || o_IPAddress[i].text);
                    var Port = setundefined(o_Port[i].textContent || o_Port[i].innerText || o_Port[i].text);
                    var Name = setundefined(o_Name[i].textContent || o_Name[i].innerText || o_Name[i].text);
                    var TagFilter = setundefined(o_TagFilter[i].textContent || o_TagFilter[i].innerText || o_TagFilter[i].text);
                    var MonitorFilter = setundefined(o_MonitorFilter[i].textContent || o_MonitorFilter[i].innerText || o_MonitorFilter[i].text);
                    var TagFields = setundefined(o_TagFields[i].textContent || o_TagFields[i].innerText || o_TagFields[i].text);
                    var TagBytes = setundefined(o_TagBytes[i].textContent || o_TagBytes[i].innerText || o_TagBytes[i].text);
                    var MonitorFields = setundefined(o_MonitorFields[i].textContent || o_MonitorFields[i].innerText || o_MonitorFields[i].text);
                    var MonitorBytes = setundefined(o_MonitorBytes[i].textContent || o_MonitorBytes[i].innerText || o_MonitorBytes[i].text);
                    var HeaderFields = setundefined(o_HeaderFields[i].textContent || o_HeaderFields[i].innerText || o_HeaderFields[i].text);
                    var HeaderBytes = setundefined(o_HeaderBytes[i].textContent || o_HeaderBytes[i].innerText || o_HeaderBytes[i].text);
                    var WPSStreaming = setundefined(o_WPSStreaming[i].textContent || o_WPSStreaming[i].innerText || o_WPSStreaming[i].text);
                    var TempGDDStream = setundefined(o_TempGDDStream[i].textContent || o_TempGDDStream[i].innerText || o_TempGDDStream[i].text);

                    var tr_classname;
                    var IPInfo = "";

                    if (i % 2 != 0)
                        tr_classname = 'clstrEven';
                    else
                        tr_classname = 'clstrOdd';

                    if (IPAddress == "") {
                        IPInfo = "Not Configured";
                        Port = "";
                        Name = "";
                        TagFilter = "";
                        MonitorFilter = "";
                        TagFields = "";
                        TagBytes = "";
                        MonitorFields = "";
                        MonitorBytes = "";
                        HeaderFields = "";
                        HeaderBytes = "";
                        WPSStreaming = "";
                        TempGDDStream = "";
                        tr_classname = "clstrStreamGray";
                    }
                    else {
                        IPInfo = IPAddress + ":" + Port;
                    }

                    streamingDetailsHTML += "<tr style='height: 22px;' class='" + tr_classname + "'>" +
                                               "<td rowspan=3 style='padding-top: 3px; padding-left: 5px; height:30px;' align='center' class='streaming_Box_UserLog'>" + StreamingIdx + "</td>" +
                                               "<td rowspan=3 style='padding-top: 3px; padding-left: 5px; height:25px;' align='left' class='streaming_Box_UserLog'>" + IPInfo + "<br>" + "<b>" + Name + "</b>" + "</br>" + "</td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'><b>Tag<b></td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + TagFilter + "</td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + TagFields + "</td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + TagBytes + "</td>" +
                                               "<td rowspan='3'style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + WPSStreaming + "</td>" +
                                               "<td rowspan='3'style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + TempGDDStream + "</td>" +
                                            "</tr>" +
                                            "<tr style='height: 22px;' class='" + tr_classname + "'>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:25px;' align='left' class='streaming_Box_UserLog'><b>Monitor</b></td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + MonitorFilter + "</td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + MonitorFields + "</td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + MonitorBytes + "</td>" +
                                            "</tr>" +
                                            "<tr style='height: 22px;' class='" + tr_classname + "'>" +
                                               "<td style='padding-top: 3px; padding-left: 5px;' align='left' class='streaming_Box_UserLog'><b>Header</b></td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'> </td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + HeaderFields + "</td>" +
                                               "<td style='padding-top: 3px; padding-left: 5px; height:30px;' align='left' class='streaming_Box_UserLog'>" + HeaderBytes + "</td>" +
                                            "</tr>"
                }

                streamingDetailsHTML += "</table>";

                row = document.createElement('tr');
                AddCell(row, streamingDetailsHTML, '', "", "", "center", "", "", "");
                sTbl.appendChild(row);
            }
            else {
                row = document.createElement('tr');
                streamingDetailsHTML += "<tr style='height: 22px;'><td class='siteOverview_cell_Full' colspan='8'>No Record Found.<td></tr>"
                AddCell(row, streamingDetailsHTML, '', "", "", "center", "", "", "");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        } 
    } 
}