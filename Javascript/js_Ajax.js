//*******************************************************************
//	Function Name	:	CreateXMLObj
//	Input			:	None
//	Description		:	Create XML Object For Ajax call
//*******************************************************************
function CreateXMLObj() {
    var obj = null;
    if (window.ActiveXObject) {
        try {
            obj = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                obj = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                obj = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
        obj = new XMLHttpRequest();
        obj.overrideMimeType('text/xml');
    }
    return obj;
}

function CreateXMLObj2() {
    var obj2 = null;
    if (window.ActiveXObject) {
        try {
            obj2 = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
            try {
                obj2 = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e1) {
                obj2 = null;
            }
        }
    }
    else if (window.XMLHttpRequest) {
        obj2 = new XMLHttpRequest();
        obj2.overrideMimeType('text/xml');
    }
    return obj2;
}

// Checking For Browser
function GetBrowserType() {
    var isIE = ((document.all) ? true : false); //for Internet Explorer
    var isFF = ((document.getElementById && !document.all) ? true : false); //for Mozilla Firefox

    if (!(window.ActiveXObject) && "ActiveXObject" in window) {
        return "isIE";
    }

    if (isIE) {
        return "isIE";
    }
    else if (isFF) {
        return "isFF";
    }
}

function ReloadtoBartenderQueue() {
    location.href = 'BartenderQueue.aspx';
}

var g_Obj;

function LoadAlert() {
    g_Obj = CreateXMLObj();

    if (g_Obj != null) {
        g_Obj.onreadystatechange = ajaxAlertLoadList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=LoadUpdates";

        if (GetBrowserType() == "isIE") {
            g_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Obj.open("GET", DbConnectorPath, true);
        }
        g_Obj.send(null);
    }

    return false;
}

function ajaxAlertLoadList() {
    if (g_Obj.readyState == 4) {
        if (g_Obj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(g_Obj.responseXML.documentElement);

            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('ctl00_leftmenu_tblcriticalAlert');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('ctl00_leftmenu_tblcriticalAlert');
            }

            sTblLen = sTbl.rows.length;

            var dsRoot = g_Obj.responseXML.documentElement;
            var sitelist = dsRoot.getElementsByTagName('SiteId')
            nRootLength = sitelist.length;

            var sTbl = document.getElementById("ctl00_leftmenu_tblcriticalAlert");
            clearTableRows(sTbl, sTblLen)

            if (dsRoot != null && nRootLength > 0) {
                var osite_id = dsRoot.getElementsByTagName('SiteId');
                var osite_Name = dsRoot.getElementsByTagName('SiteName');

                if (osite_id.length > 0) {
                    document.getElementById("ctl00_headerBanner_alerttd").style.display = "";
                    document.getElementById("ctl00_headerBanner_alerttd").style.cursor = "pointer";
                }
                else {
                    document.getElementById("ctl00_headerBanner_alerttd").style.display = "none";
                }

                row = document.createElement('tr');
                AddCell(row, "Critical Alerts Sites ", 'sText3');
                sTbl.appendChild(row);

                for (var i = 0; i < nRootLength; i++) {
                    var site_id = (osite_id[i].textContent || osite_id[i].innerText || osite_id[i].text);
                    var site_Name = (osite_Name[i].textContent || osite_Name[i].innerText || osite_Name[i].text);

                    var spilt_sitename = site_Name.split("(")
                    var s_Name = ""

                    if (spilt_sitename.length == 2)
                        s_Name = spilt_sitename[0];
                    else
                        s_Name = site_Name;

                    row = document.createElement('tr');
                    var imgtd = "#img-" + site_id

                    var str = "<a  class='sCriticalAlertsList' onClick='loadAlertsBySiteId(" + site_id + ");'>" + s_Name + "</a>";
                    AddCell(row, str, 'sText4');
                    sTbl.appendChild(row);
                }
            }
        }
    }
}

function highLightAlertSite(siteid) {

    var tdid = "td-" + siteid;
    var sTd = document.getElementById(tdid);

    if (sTd != null && sTd != "undefined" && sTd != "") {

        sTd.className = "HeaderTabOutRed";
        var imgtd = "img-" + siteid
        var imgtag = document.getElementById(imgtd);

        imgtag.src = "images/alert-white.png";
    }
    else {
        location.href = "Alertlist.aspx?sid=" + siteid;
    }
}

function showPwddiv() {
    var dv = document.getElementById("divPwd")
    if (dv.style.display == "none") {
        dv.style.display = "";
    }
}

function hidePwddiv() {
    var dv = document.getElementById("divPwd");

    if (dv.style.display == "") {
        dv.style.display = "none";
        document.getElementById("txtOldpwd").value = "";
        document.getElementById("txtNewpwd").value = "";
        document.getElementById("txtRetypePwd").value = "";
    }
}

function TabOver(Obj) {
    Obj.setAttribute("class", "selectedOver");
    setTip(Obj);
}

function TabOut(Obj, bgcolor) {
    Obj.setAttribute("class", "selectedOut");
    hideTip();
}

function findPosX(obj) {
    var curleft = 0;

    if (obj.offsetParent) {
        while (1) {
            curleft += obj.offsetLeft;
            if (!obj.offsetParent)
                break;
            obj = obj.offsetParent;
        }
    } 
    else if (obj.x) {
        curleft += obj.x;
    }

    obj.style.position = "static";

    return curleft;
}

function findPosY(obj) {
    var curtop = 0;

    if (obj.offsetParent) {
        while (1) {
            curtop += obj.offsetTop;
            if (!obj.offsetParent)
                break;
            obj = obj.offsetParent;
        }
    } else if (obj.y) {
        curtop += obj.y;
    }

    return curtop;
}

function btnPrev_LAServiceOver(Obj) {
    var clsname = Obj.className;
    if (clsname == "") {
        Obj.setAttribute("class", "btnPrev_LAServiceOver");
    }
    setTip3(Obj);
}

function btnPrev_LAServiceOut(Obj) {
    var clsname = Obj.className;
    if (clsname == "btnPrev_LAServiceOver") {
        Obj.setAttribute("class", "");
    }
    hideTip3(Obj);
}

function btnPagination_DeviceDetailsOver(Obj) {
    var clsname = Obj.className;
    Obj.setAttribute("class", "btnPrev_LAServiceOver");
    setTip4(Obj);
}

function btnPagination_DeviceDetailsOut(Obj) {
    var clsname = Obj.className;
    Obj.setAttribute("class", "");
    hideTip4(Obj);
}

function btnsiteOverviewHover(Obj) {
    var clsname = Obj.className;
    setTipsiteOveriew(Obj);
}

function btnsiteOverviewOut(Obj) {
    var clsname = Obj.className;
    hideTipSiteOverview();
}

function btnAddEdit_DeviceDetailsOver(Obj) {
    var clsname = Obj.className;
    Obj.setAttribute("class", "btnPrev_LAServiceOver");
    setTipDeviceDetails(Obj);
}

function btnAddEdit_DeviceDetailsOut(Obj) {
    var clsname = Obj.className;
    Obj.setAttribute("class", "");
    hideTipDeviceDetails();
}

function setTip(Obj) {

    var id = Obj.id;
    var lf = findPosX(Obj);
    var tp = findPosY(Obj);

    if (id.length > 0) {

        var dt = id.split("-")
        var str = dt[0];
        var txt = ""

        if (str == "systd") {
            lf += 110;
            tp += 130;
            txt = "Good Devices"
        } else if (str == "tagundertd" || str == "infraundertd") {
            lf += 110;
            tp += 130;
            txt = "Underwatch (90days)"
        } else if (str == "taglbitd" || str == "infrataglbitd") {
            lf += 110;
            tp += 130;
            txt = "LBI (30days)"
        } else if (str == "tagtd") {
            lf += 150;
            tp += 20;
            txt = "Tag overview"
        } else if (str == "Infratd") {
            lf += 150;
            tp += 20;
            txt = "Infrastructure overview"
        } else if (str == "heartBeatSpan") {
            lf += -30;
            tp += 30;
            txt = "Server Status"
        } else if (str == "localAlerts") {
            lf += -30;
            tp += 30;
            txt = "Local Alerts"
        } else if (str == "td") {
            lf += 150;
            tp += 35;
            var clsname = Obj.className;
            if (clsname != "HeaderTabOutRed") {
                txt = "Site overview"
            } else {
                txt = "Site offline overview"
            }

        } else if (str == "purchaseOrder") {
            lf += -30;
            tp += 30;
            txt = "Purchase Order"
        } else if (str == "map") {
            lf += -30;
            tp += 30;
            txt = "Map"
        }
        else if (str == "LocationChangeEvent") {
            lf += -30;
            tp += 30;
            txt = "Location Change Event"
        }
        else if (str == "INITrackingHistory") {
            lf += -35;
            tp += 30;
            txt = "INI Change History"
        }
        else if (str == "Reports") {
            lf += -35;
            tp += 30;
            txt = "Tags Not Seen Recently Report"
        }
                else if (str == "CetaniReports") {
                    lf += -35;
                    tp += 30;
                    txt = "Cetani MetaData (List)"
                }
        if (txt != "") {
            showTip(txt, lf, tp);
        }
    }
}

function setTip3(Obj) {

    var id = Obj.id;

    var lf = findPosX(Obj);
    var tp = findPosY(Obj);

    if (id.length != "") {
        var txt = "";
        if (id == "btnPrev_LAServiceAlerts" || id == "btnPrev_LATag" || id == "btnPrev_LAMonitor" || id == "btnPrev_LAStar" || id == "btnPrev_AlertHistory") {
            lf += 0;
            tp += -30;
            txt = "Previous";
        }
        else if (id == "btnMonthly_LAServiceAlerts" || id == "btnMonthly_LATag" || id == "btnMonthly_LAMonitor" || id == "btnMonthly_LAStar" || id == "btnMonthly_AlertHistory") {
            lf += 0;
            tp += -30;
            txt = "Monthly";
        }
        else if (id == "Img1" || id == "btnWeekly_LATag" || id == "btnWeekly_LAMonitor" || id == "btnWeekly_LAStar" || id == "btnWeekly_AlertHistory") {
            lf += 0;
            tp += -30;
            txt = "Weekly";
        }
        else if (id == "btnDaily_LAServiceAlerts" || id == "btnDaily_LATag" || id == "btnDaily_LAMonitor" || id == "btnDaily_LAStar" || id == "btnDaily_AlertHistory") {
            lf += 0;
            tp += -30;
            txt = "Daily";
        }
        else if (id == "btnHourly_LAServiceAlerts" || id == "btnHourly_LATag" || id == "btnHourly_LAMonitor" || id == "btnHourly_LAStar" || id == "btnHourly_AlertHistory") {
            lf += 0;
            tp += -30;
            txt = "Hourly";
        }
        else if (id == "btnNext_ServiceAlerts" || id == "btnNext_LATag" || id == "btnNext_LAMonitor" || id == "btnNext_LAStar" || id == "btnNext_AlertHistory") {
            lf += 0;
            tp += -30;
            txt = "Next";
        }
        else {
            var dt = id.split("-")
            var str = dt[0];
            var txt = ""
            if (str == "activeimg") {
                lf -= 15;
                tp += 35;
                txt = "Running";
            }
            else if (str == "stoppedimg") {
                lf -= 15;
                tp += 35;
                txt = "Stopped";
            }
            else if (str == "uninstallimg") {
                lf -= 20;
                tp += 35;
                txt = "Uninstalled";
            }
            else if (str == "alerthistory") {
                lf -= 25;
                tp += 35;
                txt = "Alert History";
            }
            else if (str == "editConfig") {
                lf -= 5;
                tp += 35;
                txt = "Edit";
            }

            else if (str == "editStatus") {
                lf -= 5;
                tp += 35;
                txt = "Edit";
            }
            else if (str == "deleteConfig") {
                lf -= 9;
                tp += 35;
                txt = "Delete";
            }
            else if (str == "UnknownStatusimg") {
                lf -= 9;
                tp += 35;
                txt = "Unknown";
            }
        }
    }
    if (txt != "") {
        showTip3(txt, lf, tp);
    }
}

function setTip2(Obj) {

    var id = Obj.id;
    var lf = findPosX(Obj);
    var tp = findPosY(Obj);

    if (id.length > 0) {

        var dt = id.split("-")
        var str = dt[0];
        var txt = ""

        if (str == "activeimg") {
            lf += 20;
            tp += 20;
            txt = "Running";
        } else if (str == "stoppedimg") {
            lf += 20;
            tp += 20;
            txt = "Stopped";
        }
        else if (str == "uninstallimg") {
            lf += 20;
            tp += 20;
            txt = "Uninstalled";
        }
        else if (str == "alerthistory") {
            lf += 20;
            tp += 20;
            txt = "Alert History";
        }
        
        if (txt != "") {

            showTip2(txt, lf, tp);
        }
    }
}

function setTipsiteOveriew(Obj) {

    var id = Obj.id;
    var lf = findPosX(Obj);
    var tp = findPosY(Obj);

    if (id.length > 0) {

        var dt = id.split("_")
        var str = dt[0];
        var txt = ""

        if (str == "Offline") {
            lf -= 60;
            tp += 30;
            txt = "Device is not seen by the Network";
        }

        if (txt != "") {

            showTipSiteOverview(txt, lf, tp);
        }
    }
}

function setTip4(Obj) {

    var id = Obj.id;
    var lf = findPosX(Obj);
    var tp = findPosY(Obj);

    if (id.length != "") {
        var txt = "";
        if (id == "ctl00_ContentPlaceHolder1_btnNextGraph") {
            lf += 0;
            tp += -30;
            txt = "Next";
        }
        else if (id == "ctl00_ContentPlaceHolder1_btnPrevGraph") {
            lf += 0;
            tp += -30;
            txt = "Previous";
        }
        else if (id == "btnMonthly_DeviceDetails") {
            lf += 0;
            tp += -30;
            txt = "Monthly";
        }
        else if (id == "btnWeekly_DeviceDetails") {
            lf += 0;
            tp += -30;
            txt = "Weekly";
        }
        else if (id == "btnDaily_DeviceDetails") {
            lf += 0;
            tp += -30;
            txt = "Daily";
        }
        else if (id == "btnHourly_DeviceDetails") {
            lf += 0;
            tp += -30;
            txt = "Hourly";
        }
    }

    if (txt != "") {
        showTip4(txt, lf, tp);
    }
}

function setTipDeviceDetails(Obj) {

    var id = Obj.id;
    var lf = findPosX(Obj);
    var tp = findPosY(Obj);

    if (id.length > 0) {
        var str = id;
        var txt = ""

        if (str == "DeleteDevice") {
            lf += 0;
            tp += 32;
            txt = "Delete Device"
        } else if (str == "AddDevice") {
            lf += 0;
            tp += 32;
            txt = "Add Device"
        }

        if (txt != "") {
            showTipDeviceDetails(txt, lf, tp);
        }
    }
}

function HeaderTabOver(Obj) {
    var clsname = Obj.className;

    if (clsname != "HeaderTabOutRed") {
        Obj.setAttribute("class", "HeaderTabOver");
    }
    setTip(Obj);
}

function HeaderTabOut(Obj, bgcolor) {
    var clsname = Obj.className;

    if (clsname != "HeaderTabOutRed") {
        Obj.setAttribute("class", "HeaderTabOut");
    }

    hideTip();
}

function HeaderTabOutred(Obj) {
    Obj.setAttribute("class", "HeaderTabOutRed");
    hideTip();
}

function HeaderHeartBeatOver(Obj) {
    var clsname = Obj.className;
    if (clsname != "HeaderTabOutRed") {
        Obj.setAttribute("class", "HeaderTabOver");
    }
    setTip(Obj);
}

function HeaderHeartBeatOut(Obj, bgcolor) {
    var clsname = Obj.className;
    if (clsname != "HeaderTabOutRed") {
        Obj.setAttribute("class", "HeaderTabOut");
    }

    hideTip();
}

function TabImageOver(Obj) {
    var clsname = Obj.className;
    if (clsname != "HeaderTabOutRed") {
        Obj.setAttribute("class", "TabImageOver");
    }
    setTip2(Obj);
}

function TabImageOut(Obj, bgcolor) {

    var clsname = Obj.className;
    if (clsname != "HeaderTabOutRed") {
        Obj.setAttribute("class", "TabImageOut");
    }

    hideTip2();
}

function DeviceRedirect(sid, typ) {

    if (typ == "infra90days") {
        location.href = "DeviceList1.aspx?sid=" + sid + "&Bin=1"
    } else if (typ == "infra30days") {
        location.href = "DeviceList1.aspx?sid=" + sid + "&Bin=2"
    } else if (typ == "tag90days") {
        location.href = "PatientTagList.aspx?sid=" + sid + "&Bin=1"
    } else if (typ == "tag30days") {
        location.href = "PatientTagList.aspx?sid=" + sid + "&Bin=2"
    } else if (typ == "system") {
        location.href = "siteoverview.aspx?sid=" + sid + "&Bin=" + typ
    } else if (typ == "infrastructure") {
        location.href = "siteoverview.aspx?sid=" + sid + "&typ=" + typ
    } else {
        location.href = "siteoverview.aspx?sid=" + sid + "&typ=" + typ
    }
}

function reloadPage() {
    location.href = "Home.aspx";
}

function TagHeaderTabOver(Obj) {
    if (g_UserRole == "3" || g_UserRole == "4")
        Obj.setAttribute("class", "CusTagTabOver");
    else
        Obj.setAttribute("class", "TagTabOver");
    setTip(Obj);
}      

function TagHeaderTabOut(Obj, bgcolor) {
    Obj.setAttribute("class", "Tags");
    hideTip();
}


function InfraHeaderTabOver(Obj) {
    setTip(Obj);
    Obj.setAttribute("class", "infrastructureTabOver");
}

function InfraHeaderTabOut(Obj, bgcolor) {
    Obj.setAttribute("class", "infrastructure");
    hideTip();
}

function AjaxMsgReceiver(dsRoot) {

    try {

        var o_AjaxMsg = dsRoot.getElementsByTagName('AjaxMsg');

        if (o_AjaxMsg != null) {
            var sAjaxMsg = (o_AjaxMsg[0].textContent || o_AjaxMsg[0].innerText || o_AjaxMsg[0].text);

            if (sAjaxMsg != null) {
                //Redirect to Login
                location.href = 'ApplicationError.aspx?ErrorValue=101';
            }
        }
    }
    catch (e) {

    }
}
