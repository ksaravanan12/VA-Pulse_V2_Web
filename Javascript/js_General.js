// JScript File
function trim(str) {
    if (str != undefined) {
        return str.replace(/^\s+|\s+$/g, "");
    }
    else {
        return "";
    }
}

//Function for Required Field Validation
//======================================
function IsEmpty(ctl, err) {
    var str = ctl.value;
    str = trim(str);
    ctl.value = str;
    if (str == "") {
        alert(err);
        ctl.focus();
        return true;
    }
    return false;
}

function GetDeviceIdFormat(sDevice_Ids) {
    var sResults = '';

    sResults = sDevice_Ids.replace(/[ ,]+/g, ",");
    sResults = sResults.replace(/\n/g, ",").replace(/,,/g, ',');
    sResults = sResults.replace(/^\s+|\s+$/g, "").replace(/\s+/g, ",");

    return sResults.replace(/(^,)|(,$)/g, "")
}

//Function for Email Validation
//=============================
function checkEmail(myForm) {
    if (myForm.value == "") {
        return (true);
    }

    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(myForm.value)) {
        return (true)
    }
    alert("Invalid E-mail Address! Please re-enter.")
    myForm.value = "";
    myForm.focus();
    return false;
}

function EmailCheck(str) {

    var at = "@"
    var dot = "."
    var lat = str.indexOf(at)
    var lstr = str.length
    var ldot = str.indexOf(dot)

    if (str.indexOf(at) == -1)
        return false;
    if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr)
        return false;
    if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr)
        return false;
    if (str.indexOf(at, (lat + 1)) != -1)
        return false;
    if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot)
        return false;
    if (str.indexOf(dot, (lat + 2)) == -1)
        return false;
    if (str.indexOf(" ") != -1)
        return false;

    return true;
}

//Function for Check Control Is Integer
//=====================================
function isInteger(ctrl, Err) {
    var i;
    var s = ctrl.value;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) {
            alert(Err);
            ctrl.value = '';
            ctrl.focus();
            return false;
        }
    }
    // All characters are numbers.
    return true;
}

//Allow Numeric Only Support IE && FireFox
function allowNumberOnly(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 47 && charCode < 58)
        return true;
    return false;
}

function randomPass() {

    var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
    var string_length = 8;
    var randomstring = '';
    for (var i = 0; i < string_length; i++) {
        var rnum = Math.floor(Math.random() * chars.length);
        randomstring += chars.substring(rnum, rnum + 1);
    }
    return randomstring;
}

//maskChange			
function maskChange(objEvent) {

}

//Function for Dont Allow Space
//=============================
function DontAllowSpace(e) {
    var iKeyCode = 0;
    if (window.event)
        iKeyCode = window.event.keyCode
    else if (e)
        iKeyCode = e.which;
    if (iKeyCode == 32)
        return false;
    else
        return true;
}

function ShowColorPicker(txtBoxId, lblFontStyle) {

    txtCtrlId = document.getElementById(txtBoxId);
    lblCtrlId = document.getElementById(lblFontStyle);

    colorwindow = open('colorpicker.aspx?scolor=' + txtCtrlId.value, 'colorpicker', 'resizable=no,left=200,top=200,width=600,height=310');
    if (colorwindow.opener == null) colorwindow.opener = self;
}

function searchrestart(txtBoxId) {

    txtCtrlId.value = pickcolor;
    if (lblCtrlId.type != "text")
        lblCtrlId.style.color = '#' + pickcolor;

    colorwindow.close();
}

function Gradientsearchrestart(txtBoxId) {

    txtGradientCtrlId.value = Gradientcolor;
    Gradientcolorwindow.close();
}

//*********************************************************
//	Function Name	:	getParameterByName
//	Input			:	none
//	Description		:	ajax call for Get Query String Value
//*********************************************************
function getParameterByName(name) {
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
}

//*******************************************************************
//	Function Name	:	AddCell
//	Input			:	row,InnerHTML,className,cellId 
//	OutPut			:   Top postion (Y)
//	Description		:	Give the Y position of the Given Object
//*******************************************************************
function AddCell(row, val, clsName, scolspan, CellId, CellAlign, sWidth, sHeight, sPaddingLeft) {
    var cell = document.createElement("td");

    if (clsName != "") {
        cell.className = clsName;
    }
    cell.id = CellId
    if (CellAlign == "") {
        cell.setAttribute("align", "left");
    }
    else {
        cell.setAttribute("align", CellAlign);
    }
    if (sWidth != "") {
        cell.setAttribute("width", sWidth)
    }
    if (sHeight != "") {
        cell.setAttribute("height", sHeight)
    }
    if (scolspan == "")
        scolspan = 1;

    if (sPaddingLeft != "" && sPaddingLeft != undefined)
        cell.style.paddingLeft = sPaddingLeft;

    cell.setAttribute("align", CellAlign)
    cell.setAttribute("colSpan", scolspan)

    cell.style.bgcolor = "#FFFFFF"

    cell.innerHTML = setundefined(val);

    row.appendChild(cell);
}

function AddCellEx(row, val, clsName, scolspan, CellId, CellAlign, sWidth, sHeight, sPaddingLeft, rowSpan) {
    var cell = document.createElement("td");

    if (clsName != "") {
        cell.className = clsName;
    }
    cell.id = CellId
    if (CellAlign == "") {
        cell.setAttribute("align", "left");
    }
    else {
        cell.setAttribute("align", CellAlign);
    }
    if (sWidth != "") {
        cell.setAttribute("width", sWidth)
    }
    if (sHeight != "") {
        cell.setAttribute("height", sHeight)
    }
    if (scolspan == "")
        scolspan = 1;

    if (sPaddingLeft != "" && sPaddingLeft != undefined)
        cell.style.paddingLeft = sPaddingLeft;

    cell.setAttribute("align", CellAlign)
    cell.setAttribute("colSpan", scolspan)
    cell.setAttribute("rowSpan", rowSpan)

    cell.style.bgcolor = "#FFFFFF"

    cell.innerHTML = setundefined(val);

    row.appendChild(cell);
}

//*******************************************************************
//	Function Name	:	delchild
//	Input			:	tableObject,RowLenght
//	OutPut			:   None
//	Description		:	Delete the Rows of the Given Table obj
//*******************************************************************
function delchild(tabobj, RowLength) {
    if (GetBrowserType() == "isIE") {
        for (var idx = RowLength - 1; idx >= 0; idx--) {
            tabobj.removeChild(tabobj.childNodes[idx]);
        }
    }
    else if (GetBrowserType() == "isFF") {
        for (var idx = RowLength; idx > 0; idx--) {
            tabobj.removeChild(tabobj.childNodes[idx]);
        }
    }
}

//*******************************************************************
//	Function Name	:	moveitems
//	Input			:	tbFrom,tbTo
//	OutPut			:   None
//	Description		:	move items between listboxes
//*******************************************************************
function moveitems(tbFrom, tbTo) {
    var arrFrom = new Array(); var arrTo = new Array();
    var arrLU = new Array();
    var i;
    for (i = 0; i < tbTo.options.length; i++) {
        arrLU[tbTo.options[i].text] = tbTo.options[i].value;
        arrTo[i] = tbTo.options[i].text;
    }

    var fLength = 0;
    var tLength = arrTo.length;

    for (i = 0; i < tbFrom.options.length; i++) {
        arrLU[tbFrom.options[i].text] = tbFrom.options[i].value;
        if (tbFrom.options[i].selected && tbFrom.options[i].value != "") {
            arrTo[tLength] = tbFrom.options[i].text;
            tLength++;
        }
        else {
            arrFrom[fLength] = tbFrom.options[i].text;
            fLength++;
        }
    }

    tbFrom.length = 0;
    tbTo.length = 0;
    var ii;

    for (ii = 0; ii < arrFrom.length; ii++) {
        var no = new Option();
        no.value = arrLU[arrFrom[ii]];
        no.text = arrFrom[ii];
        tbFrom[ii] = no;
    }

    for (ii = 0; ii < arrTo.length; ii++) {
        var no = new Option();
        no.value = arrLU[arrTo[ii]];
        no.text = arrTo[ii];
        tbTo[ii] = no;
    }
}

//*******************************************************************
//	Function Name	:	ClearDropDown
//	Input			:	ddl
//	OutPut			:   None
//	Description		:	clear the items in dropdown
//*******************************************************************
function ClearDropDown(ddl) {
    var len = ddl.options.length;
    for (i = 0; i < len; i++) {
        ddl.remove(0);
    }
}

//*******************************************************************
//	Function Name	:	clearTableRows
//	Input			:	tabobj,RowLength
//	OutPut			:   None
//	Description		:	clear the rows in table
//*******************************************************************
function clearTableRows(tabobj, RowLength) {

    if (tabobj)
        $("#" + tabobj.id).html("");
    return;

    if (GetBrowserType() == "isIE") {

        var rows = tabobj.getElementsByTagName('tr');
        var rowlength = rows.length;

        if (rowlength > 0) {
            for (i = 0; i < rowlength; i++) {
                if (tabobj.firstChild != null)
                    tabobj.removeChild(tabobj.firstChild);
            }
        }
    }

    if (GetBrowserType() == "isFF") {
        for (i = 0; i < RowLength; i++) {
            tabobj.deleteRow(0);
        }
    }
}

//*******************************************************************
//	Function Name	:	clearTableRows
//	Input			:	tabobj,RowLength
//	OutPut			:   None
//	Description		:	clear the rows in table except header
//*******************************************************************
function clearTableRowsWithoutHeader(tabobj, RowLength) {
    for (var i = RowLength; i > 1; i--) {
        tabobj.deleteRow(1);
    }
}

//*******************************************************************
//	Function Name	:	checkuncheckAll
//	Input			:	checktoggle
//	OutPut			:   Integer
//	Description		:	check uncheck all checkboxes
//*******************************************************************
function checkuncheckAll(checktoggle) {
    var checkboxes = new Array();
    checkboxes = document.getElementsByTagName('input');

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox') {
            checkboxes[i].checked = checktoggle;
        }
    }
}

//*******************************************************************
//	Function Name	:	setundefined
//	Input			:	value
//	OutPut			:   String
//	Description		:	set undefined value empty
//*******************************************************************
function setundefined(value) {
    if (value != "" && value != undefined)
        return value;
    else
        return "";
}

//AllowFloat with Intgeer
function AllowFloat(e) {
    var iKeyCode = 0;
    if (window.event) {
        iKeyCode = window.event.keyCode
        if (iKeyCode > 44 && iKeyCode < 58)
            return true

        else
            return false;
    }
    else if (e) {
        iKeyCode = e.which;
        if ((iKeyCode > 44 && iKeyCode < 58) || iKeyCode == 0 || iKeyCode == 8)
            return true
        else
            return false;
    }
}

//Interger only without float Support IE && FireFox
function KeyNumericwithHypen(evt) {
    var BrowserType = GetBrowserType();
    akert(evt);
    return false;

    if (BrowserType == "isIE") {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            if ((event.keyCode == 45) || (event.keyCode == 46)) {
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }
    }
    else if (BrowserType == "isFF") {
        eVal = evt.which;

        if ((eVal > 47 && eVal < 58) || (eVal == 8) || (eVal == 0))
            return true;

        return false;
    }
}

function showGlossaryInfo(page) {
    if (page == "Home") {
        document.getElementById("glHome").style.display = "";
        document.getElementById("glSiteOverview").style.display = "none";
        document.getElementById("glDeviceList").style.display = "none";
        document.getElementById("glDeviceDetails").style.display = "none";
        document.getElementById("glHeartBeatContent").style.display = "none";
        document.getElementById("glLocalAlerts").style.display = "none";

    }
    else if (page == "SiteOverview") {
        document.getElementById("glHome").style.display = "none";
        document.getElementById("glSiteOverview").style.display = "";
        document.getElementById("glDeviceList").style.display = "none";
        document.getElementById("glDeviceDetails").style.display = "none";
        document.getElementById("glHeartBeatContent").style.display = "none";
        document.getElementById("glLocalAlerts").style.display = "none";
    }
    else if (page == "DeviceList") {
        document.getElementById("glHome").style.display = "none";
        document.getElementById("glSiteOverview").style.display = "none";
        document.getElementById("glDeviceList").style.display = "";
        document.getElementById("glDeviceDetails").style.display = "none";
        document.getElementById("glHeartBeatContent").style.display = "none";
        document.getElementById("glLocalAlerts").style.display = "none";
    }
    else if (page == "DeviceDetails") {
        document.getElementById("glHome").style.display = "none";
        document.getElementById("glSiteOverview").style.display = "none";
        document.getElementById("glDeviceList").style.display = "none";
        document.getElementById("glDeviceDetails").style.display = "";
        document.getElementById("glHeartBeatContent").style.display = "none";
        document.getElementById("glLocalAlerts").style.display = "none";
    }
    else if (page == "HeartBeatContent") {
        document.getElementById("glHome").style.display = "none";
        document.getElementById("glSiteOverview").style.display = "none";
        document.getElementById("glDeviceList").style.display = "none";
        document.getElementById("glDeviceDetails").style.display = "none";
        document.getElementById("glHeartBeatContent").style.display = "";
        document.getElementById("glLocalAlerts").style.display = "none";
    }
    else if (page == "LocalAlerts") {
        document.getElementById("glHome").style.display = "none";
        document.getElementById("glSiteOverview").style.display = "none";
        document.getElementById("glDeviceList").style.display = "none";
        document.getElementById("glDeviceDetails").style.display = "none";
        document.getElementById("glHeartBeatContent").style.display = "none";
        document.getElementById("glLocalAlerts").style.display = "";
    }
}

//Function for Load Glossary info
function LoadGlossaryInfo(typ, userRole) {
    var htmlstr = getGlossaryInfo(typ, userRole);
    document.getElementById("ctl00_headerBanner_divGlossary").innerHTML = htmlstr;
}

//Function for Get Glossary info for Pages
function getGlossaryInfo(typ, userRole) {
    var htmlStr = ""
    if (typ == "Home") {
        //Home
        htmlStr = "<div id='glHome' style='display:none'>"
        htmlStr += "<div class='expandstory'>System</div>"
        htmlStr += "<div class='description hide'>Number of devices in the site that have been registered in Connect Pulse and are good.</div>"

        htmlStr += "<div class='expandstory'>Tags</div>"
        htmlStr += "<div class='description hide'>Number of tags in the site that have been registered in Connect Pulse, grouped by status Under Watch and LBI.</div>"

        htmlStr += "<div class='expandstory'>Infrastructure</div>"
        htmlStr += "<div class='description hide'>Number of Monitors,Dim and Star in the site that have been registered in Connect Pulse, grouped by status Under Watch and LBI.</div>"

        htmlStr += "<div class='expandstory'>Underwatch</div>"
        htmlStr += "<div class='description hide'>Battery has less than (<) 90 days remaining for the device. Performance should be monitored for future replacement.</div>"

        htmlStr += "<div class='expandstory'>Low Battery Indication (LBI)</div>"
        htmlStr += "<div class='description hide'>Battery has less than (<) 30 days remaining for a device. Needs attention/need to replace the battery.</div>"

        htmlStr += "<div class='expandstory'>View Details</div>"
        htmlStr += "<div class='description hide'>Shows overview of the site with devices grouped by type.</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }

        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"

        htmlStr += "</div>"

        //glSiteOverview
        htmlStr += "<div id='glSiteOverview' style='display:none'>"
        htmlStr += "<div class='expandstory' >Tags</div>"
        if (userRole == "3") {
            htmlStr += "<div class='description hide'>Number of tags in the site that have been registered in Connect Pulse, grouped by status Good,Under Watch,LBI.</div>"
        } else {
            htmlStr += "<div class='description hide'>Number of tags in the site that have been registered in Connect Pulse, grouped by status Good,Under Watch,LBI and Offline .</div>"
        }

        htmlStr += "<div class='expandstory'>Infrastructure</div>"
        if (userRole == "3") {
            htmlStr += "<div class='description hide'>Number of Monitors,Dim and Star in the site that have been registered in Connect Pulse, grouped by status Good,Under Watch,LBI.</div>"
        } else {
            htmlStr += "<div class='description hide'>Number of Monitors,Dim and Star in the site that have been registered in Connect Pulse, grouped by status Good,Under Watch,LBI and Offline.</div>"
        }

        htmlStr += "<div class='expandstory'>Good</div>"
        htmlStr += "<div class='description hide'>Battery Level of the Device is Good. There is no need for any action.</div>"

        htmlStr += "<div class='expandstory'>Less than 90 Days</div>"
        htmlStr += "<div class='description hide'>Battery has less than (<) 90 days remaining for the device. Performance should be monitored for future replacement.</div>"

        htmlStr += "<div class='expandstory'>Less than 30 Days</div>"
        htmlStr += "<div class='description hide'>Battery has less than (<) 30 days remaining for a device. Needs attention/need to replace the battery.</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Offline</div>"
            htmlStr += "<div class='description hide'>Number of Monitors,Dim and Star in the site that have been registered in Connect Pulse, grouped by status Good,Under Watch,LBI.</div>"

            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }

        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"

        htmlStr += "</div>"

        //glDeviceList
        htmlStr += "<div id='glDeviceList' style='display:none'>"
        htmlStr += "<div class='expandstory'>Export Excel</div>"
        htmlStr += "<div class='description hide'>Exports the list to excel and download.</div>"

        htmlStr += "<div class='expandstory'>Go</div>"
        htmlStr += "<div class='description hide'>Navigates to the page entered.</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"

        htmlStr += "</div>"

        //glDeviceDetails
        htmlStr += "<div id='glDeviceDetails' style='display:none'>"
        htmlStr += "<div class='expandstory'>Profile Info</div>"
        htmlStr += "<div class='description hide'>List all the profile information of the device</div>"

        htmlStr += "<div class='expandstory'>Shipping Info</div>"
        htmlStr += "<div class='description hide'>List all the shipping information of the device</div>"

        htmlStr += "<div class='expandstory'>Last 10 Hr Location Data</div>"
        htmlStr += "<div class='description hide'>Last 10 hour location data of the device is listed</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"

        htmlStr += "</div>"

        //glHeartBeatContent
        htmlStr += "<div id='glHeartBeatContent' style='display:none'>"
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last updated date time of the heart beat service</div>"

        htmlStr += "<div class='expandstory'>Local Time</div>"
        htmlStr += "<div class='description hide'>Local system time</div>"

        htmlStr += "<div class='expandstory'>Updated</div>"
        htmlStr += "<div class='description hide'>Time since last update</div>"

        htmlStr += "<div class='expandstory'>Free HD Space</div>"
        htmlStr += "<div class='description hide'>Available Free HD space of the system</div>"

        htmlStr += "<div class='expandstory'>Processor</div>"
        htmlStr += "<div class='description hide'>Available Processor of the system</div>"

        htmlStr += "<div class='expandstory'>RAM</div>"
        htmlStr += "<div class='description hide'>Available RAM of the system</div>"

        htmlStr += "<div class='expandstory'>System Up Time</div>"
        htmlStr += "<div class='description hide'>System up time period</div>"

        htmlStr += "<div class='expandstory'>Status</div>"
        htmlStr += "<div class='description hide'>Indicates the status of the service - Running or Stopped or Uninstalled</div>"

        htmlStr += "<div class='expandstory'>Alert History</div>"
        htmlStr += "<div class='description hide'>Link to view the alert history for the service</div>"

        htmlStr += "<div class='expandstory'>Service Name</div>"
        htmlStr += "<div class='description hide'>Name of the service</div>"

        htmlStr += "<div class='expandstory'>Status As On</div>"
        htmlStr += "<div class='description hide'>Provides the time period of the status</div>"

        htmlStr += "<div class='expandstory'>Version</div>"
        htmlStr += "<div class='description hide'>Version of the service</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"

        htmlStr += "</div>"


        //glLocalAlerts
        htmlStr += "<div id='glLocalAlerts' style='display:none'>"
        htmlStr += "<div class='expandstory'>Hourly</div>"
        htmlStr += "<div class='description hide'>Groups the local alerts by hours of the day</div>"

        htmlStr += "<div class='expandstory'>Daily</div>"
        htmlStr += "<div class='description hide'>Groups the local alerts by day</div>"

        htmlStr += "<div class='expandstory'>Weekly</div>"
        htmlStr += "<div class='description hide'>Groups the local alerts by week</div>"

        htmlStr += "<div class='expandstory'>Monthly</div>"
        htmlStr += "<div class='description hide'>Groups the local alerts by month</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"

        htmlStr += "</div>"


    }
    else if (typ == "Search") {
        htmlStr += "<div class='expandstory'>Filter</div>"
        htmlStr += "<div class='description hide'>List of filters that can be used for search criteria. Filters will be loaded depending on the device type selected. Can add multiple filters.</div>"

        htmlStr += "<div class='expandstory'>Device Type</div>"
        htmlStr += "<div class='description hide'>Search can be performed on Tags or Monitors or Stars.</div>"

        htmlStr += "<div class='expandstory'>Device Id</div>"
        htmlStr += "<div class='description hide'>Id of device can be used to make the search. Multiple id's can be entered seperated by comma.</div>"

        htmlStr += "<div class='expandstory'>Show</div>"
        htmlStr += "<div class='description hide'>List's the search result performed using the search criteria.</div>"

        htmlStr += "<div class='expandstory'>Clear</div>"
        htmlStr += "<div class='description hide'>Remove's all the filters selected.</div>"

        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"
    }
    else if (typ == "Profile") {
        htmlStr += "<div class='expandstory'>Username</div>"
        htmlStr += "<div class='description hide'>Username of the currently logged in user.</div>"

        htmlStr += "<div class='expandstory'>User Role</div>"
        htmlStr += "<div class='description hide'>Role of the current user.</div>"

        htmlStr += "<div class='expandstory'>Change Password</div>"
        htmlStr += "<div class='description hide'>Updates the old password with the new password for the user.</div>"
        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"
    }
    else if (typ == "Email") {
        htmlStr += "<div class='expandstory'>Email</div>"
        htmlStr += "<div class='description hide'>Email Address to whom the alert should be sent.</div>"

        htmlStr += "<div class='expandstory'>Add Email</div>"
        htmlStr += "<div class='description hide'>Setup email address for the site to receive alerts.</div>"

        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"
    }
    else if (typ == "EmailReport") {

        htmlStr += "<div class='expandstory'>Time</div>"
        htmlStr += "<div class='description hide'>Time upon which email should be sent</div>"

        htmlStr += "<div class='expandstory'>Recurrence Pattern</div>"
        htmlStr += "<div class='description hide'>User can choose from the available recurrence pattern for receiving the email report</div>"

        htmlStr += "<div class='expandstory'>Save Settings</div>"
        htmlStr += "<div class='description hide'>Saves the user configured settings to the system</div>"
        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"
    }
    else if (typ == "AlertSetting") {

        htmlStr += "<div class='expandstory'>Alerts Settings</div>"
        htmlStr += "<div class='description hide'>User can select single or multiple alerts from the available alerts</div>"

        htmlStr += "<div class='expandstory'>Alert Recipients</div>"
        htmlStr += "<div class='description hide'>Maintenance users cannot add email addresses or phone numbers. Partner's can add email/phone numbers</div>"

        htmlStr += "<div class='expandstory'>Save Settings</div>"
        htmlStr += "<div class='description hide'>Saves the user configured settings to the system</div>"
        if (userRole != "3") {
            htmlStr += "<div class='expandstory'>Critical Alerts</div>"
            htmlStr += "<div class='description hide'>List of sites that have active critical alerts.</div>"
        }
        htmlStr += "<div class='expandstory'>Last Updated</div>"
        htmlStr += "<div class='description hide'>Last date and time of Connect Pulse update.</div>"

        htmlStr += "<div class='expandstory'>Update Available</div>"
        htmlStr += "<div class='description hide'>Indication about the new update available for site and lets the user to get the latest.</div>"

        htmlStr += "<div class='expandstory'>Log Out</div>"
        htmlStr += "<div class='description hide'>Log out the user and ends the current session.</div>"
    }

    return htmlStr;
}

//Check Email
function checkEmail(myForm) {
    if (myForm == "") {
        return (true);
    }

    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/.test(myForm)) {
        return (true)
    }

    return false;
}

function getDateFromWeekNoAndYear(w, y) {
    // 1st of January + 7 days for each week
    var d = (1 + (w - 1) * 7);
    return new Date(y, 0, d);
}

function getDateFromMnthNoAndYear(m, y) {
    var d = 1
    return new Date(y, m, d);
}

function formatAMPM(date) {
    var fDate = new Date(date);

    var months = (fDate.getMonth() + 1);
    var days = fDate.getDate();
    var years = fDate.getFullYear();
    var hours = fDate.getHours();
    var minutes = fDate.getMinutes();
    var seconds = fDate.getSeconds();
    var ampm = hours >= 12 ? 'pm' : 'am';

    hours = hours % 12;
    hours = hours ? hours : 12;
    months = months < 10 ? '0' + months : months;
    days = days < 10 ? '0' + days : days;
    hours = hours < 10 ? '0' + hours : hours;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;

    var strDateTime = months + '/' + days + '/' + years + ' ' + hours + ':' + minutes + ':' + seconds + ' ' + ampm;
    return strDateTime;
}

//*******************************************************************
//	Function Name	:	AddCell
//	Input			:	row,InnerHTML,className,cellId 
//	OutPut			:   Top postion (Y)
//	Description		:	Give the Y position of the Given Object
//*******************************************************************
function AddCellForSorting(row, val, clsName, scolspan, CellId, CellAlign, sWidth, sHeight, sPaddingLeft, SortCol, CurrSortCol, SortImg, SortType) {
    var cell = document.createElement("td");
    if (clsName != "") {
        cell.className = clsName;
    }
    cell.id = CellId
    if (CellAlign == "") {
        cell.setAttribute("align", "left");
    }
    else {
        cell.setAttribute("align", CellAlign);
    }
    if (sWidth != "") {
        cell.setAttribute("width", sWidth)
    }
    if (sHeight != "") {
        cell.setAttribute("height", sHeight)
    }
    if (scolspan == "")
        scolspan = 1;

    if (sPaddingLeft != "" && sPaddingLeft != undefined)
        cell.style.paddingLeft = sPaddingLeft;

    cell.style.cursor = 'pointer';

    if (SortType == enumSortingArr.LocalAlertsTableView)
        cell.setAttribute('onclick', "sortAlerts('" + SortCol + "')");
    else if (SortType == enumSortingArr.PurchaseOrderView)
        cell.setAttribute('onclick', "sortPO('" + SortCol + "')");
    else if (SortType == enumSortingArr.TagMetaInfo)
        cell.setAttribute('onclick', "sortTMI('" + SortCol + "')");
    else if (SortType == enumSortingArr.LocationLive)
        cell.setAttribute('onclick', "sortLocationLive('" + SortCol + "')");
    else if (SortType == enumSortingArr.LocationHistory)
        cell.setAttribute('onclick', "sortLocationHistory('" + SortCol + "')");
    else if (SortType == enumSortingArr.AssetTracking)
        cell.setAttribute('onclick', "sortAssetTracking('" + SortCol + "')");
    else if (SortType == enumSortingArr.TagBatteryView)
        cell.setAttribute('onclick', "sortTagBatteryList('" + SortCol + "')");
    else if (SortType == enumSortingArr.MonitorBatteryView)
        cell.setAttribute('onclick', "sortInfraBatteryList('" + SortCol + "')");
    else if (SortType == enumSortingArr.BatteryTechView)
        cell.setAttribute('onclick', "sortBatteryTechView('" + SortCol + "')");
    else if (SortType == enumSortingArr.RoomView)
        cell.setAttribute('onclick', "sortRoomView('" + SortCol + "')");
    else if (SortType == enumSortingArr.INI_History)
        cell.setAttribute('onclick', "sortINI_History('" + SortCol + "')");
    else if (SortType == enumSortingArr.TagView)
        cell.setAttribute('onclick', "sortTagInfo('" + SortCol + "')");

    cell.setAttribute("align", CellAlign)
    cell.setAttribute("colSpan", scolspan)

    cell.style.bgcolor = "#FFFFFF"

    if (SortCol == CurrSortCol)
        cell.innerHTML = setundefined(val) + "&nbsp;&nbsp;" + SortImg;
    else
        cell.innerHTML = setundefined(val);

    row.appendChild(cell);
}

var enumSortingArr = { 
    LocalAlertsTableView: 1,
    PurchaseOrderView: 2,
    TagMetaInfo: 3,
    LocationLive: 4,
    LocationHistory:5,
    AssetTracking:6,
    TagBatteryView:7,
    MonitorBatteryView:8,
    BatteryTechView:9,
    RMA_LIST_View:10,
    QC_LIST_View:11,
    RoomView:12,
    RMA_Report_View:13,
    INI_History:14,
    TagView: 15      
};

var enumUserRoleArr = { 
    Admin:1,
    Partner:2,
    Customer:3,
    Maintenance:4,
    Engineering:5,
    API_User:6,
    AssetTrackUser:7,
    BatteryTech:8,
    TechnicalAdmin:25, 
    Support:32,  
    MaintenancePrism: 33    
};

var enum_TagType = { 
        enum_AssetTAG:1,
        enum_MMAssetTAG:2,
        enum_StaffTAG:3,
        enum_MMStaffTAG:4,
        enum_TempTag:5,
        enum_ERUTag:6,
        enum_HumidityTag:7,
        enum_PatientTag:8,
        enum_G2TempTag:9,
        enum_SUPT:10,
        enum_AmbientTempRH:11,
        enum_InterfaceTag:12,
        enum_DisplayTag:13
}

var enum_BatteryActivity =
{
    BatteryActivity: 1,
    LBI: 2
}
        
//function to trim the preceedind and trailing spaces in that given string
function Trim(str) {
    while (str.substring(0, 1) == ' ') {
        str = str.substring(1, str.length);
    }
    while (str.substring(str.length - 1, str.length) == ' ') {
        str = str.substring(0, str.length - 1);
    }
    return str;
}

function CheckUserAssociatedForView(viewToCheck, Userviews) {

    if (Userviews.length == 0)
        return false;

    var allUserAssociatedViewsArr = new Array();
    allUserAssociatedViewsArr = Userviews.split(",");

    if (!Array.prototype.indexOf) {
        for (var i = 0; i < allUserAssociatedViewsArr.length - 1; i++) {
            if (this[i] === viewToCheck) {
                return true;
            }
        }
        return false;
    }
    else {
        if (allUserAssociatedViewsArr.indexOf(viewToCheck) > -1) {
            return true;
        }
        else
            return false;
    }
}
//******************************************************************************************************************************
//	Function Name	:	getTagNameValue
//	Input			:	
//	Description		:	
//******************************************************************************************************************************
function getTagNameValue(nodeElem) {
    if (navigator.userAgent.indexOf('MSIE') != -1 || navigator.userAgent.indexOf('Firefox') != -1) {
        return setundefined(nodeElem.textContent || nodeElem.innerText || nodeElem.text || nodeElem.innerHTML);
    }
    else {
        return (nodeElem.textContent || nodeElem.innerText || nodeElem.text || nodeElem.innerHTML);
    }
}

//Enumerations
var enumPageAction = {
    Add: 1,
    Edit: 2,
    Delete: 3,
    View: 4,
    Search: 5,
    Filter: 6,
    Login: 7,
    Logout: 8,
    AccessViolation: 9,
    Security: 10
};

var enumReportType = {
    MonitorAnalysisReport: 1,
    DeviceSummary: 2,
    TTSyncErrReport: 3,
    ConnectivityReport: 4,
    DefectiveReport:5
};

//*********************************************************
//	Function Name	:	AddPageVisitDetails
//	Input			:	UserId, PageName, PageAction, PageActionHistory
//	Description		:	ajax call AddPageVisitDetails
//*********************************************************
var PageVisit_Obj;
var g_IsAdd;

function PageVisitDetails(UserId, PageName, PageAction, PageActionHistory) {
    PageVisit_Obj = CreateInfraXMLObj();

    if (PageVisit_Obj != null) {
        PageVisit_Obj.onreadystatechange = AddPageVisitDetails;

        DbConnectorPath = "AjaxConnector.aspx?cmd=InsertPageVisitDetails&UserId=" + UserId + "&PageName=" + PageName + "&nPageAction=" + PageAction + "&PageActionHistory=" + PageActionHistory;

        if (GetBrowserType() == "isIE") {
            PageVisit_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            PageVisit_Obj.open("GET", DbConnectorPath, true);
        }
        PageVisit_Obj.send(null);
    }
    return false;
}

//*********************************************************
//	Function Name	:	AddPageVisitDetails
//	Input			:	none
//	Description		:	Add PageVisitDetails Datas and result from ajax Response
//*********************************************************
function AddPageVisitDetails() {
    if (PageVisit_Obj.readyState == 4) {
        if (PageVisit_Obj.status == 200) {


        }
    }
}

function funEMSection(TypeId) {
    var IsEMSec = "0";
    
    if (TypeId == enum_TagType.enum_TempTag || TypeId == enum_TagType.enum_G2TempTag || TypeId == enum_TagType.enum_HumidityTag || TypeId == enum_TagType.enum_AmbientTempRH || TypeId == enum_TagType.enum_InterfaceTag || TypeId == enum_TagType.enum_DisplayTag) {
        IsEMSec = "1";
    }    
    return IsEMSec;
}

var enumTagType = {
    TAG: 0,
    HHCTAG: 1,
    TEMPERATURETAG: 2,
    ERUTAG: 3,
    HUMINITYTEMPTAG: 4,
    TWOGTEMPTAG: 5,
    WIFITAG: 6
}
//Enumerations
var enumMSE = {
    MSEPolling: 0,
    MSEPollingCMX: 1,
    NorthBound: 2,
    None: 3
};

function strMSE(Tracking) {

    var str = "";

    if (Tracking == enumMSE.MSEPolling)    
        str = "MSE Polling";
    if (Tracking == enumMSE.MSEPollingCMX)    
        str = "MSE Polling [CMX]";
    if (Tracking == enumMSE.NorthBound)    
        str = "NorthBound";
    if (Tracking == enumMSE.None)    
        str = "None"; 
        
    return str;   
}

function AddParameter(form, name, value) {
    var $input = $("<input />").attr("type", "hidden")
                            .attr("name", name)
                            .attr("value", value);
    form.append($input);
}

function RedirectPrismPage(UserId, UserName, UserRole, UserEmail, APIkey, SiteId, PrismURL) {

    //Create a Form
    var $form = $("<form/>").attr("id", "data_form")
                            .attr("action", PrismURL)
                            .attr("method", "post");
    $("body").append($form);

    //Append the values to be send              
    AddParameter($form, "UserId", UserId);
    AddParameter($form, "UserName", UserName);
    AddParameter($form, "UserRole", UserRole);
    AddParameter($form, "UserEmail", UserEmail);
    AddParameter($form, "APIkey", APIkey);
    AddParameter($form, "SiteId", SiteId);

    //Send the Form
    $form[0].submit();
}
//Enumerations
var enumPulseUITable = {

    WorkflowTags: 1,
    EnvironmentalTags: 2,
    Infrastructure: 3 
};

//Enumerations

var enumTagSubType = {

    AssetTag: 1,
    AssetTag_Autoclave: 2,
    AssetTag_DuraTag: 3,
    AssetTag_Micro: 4,
    AssetTag_MiniMM: 5,
    AssetTag_Mini: 6,
    AssetTag_MM: 7,    
    CallPoint: 8,
    CastellModule: 9,
    GeoPendant: 10,
    GeoPendant_Black: 11,
    GeoPendant_White: 12,
    OnewayBinarymodule: 13,
    PatientTag: 14,
    PatientTag_31_DayBlue: 15,
    PatientTag_31_DayGreen: 16,
    PatientTag_31_DayOrange: 17,
    PatientTag_Micro: 18,
    PatientTag_MiniMM: 19,
    PatientTag_MiniSealed: 20,
    PatientTag_MiniStandalone: 21,
    PatientTag_Mini: 22,
    PatientTag_MM: 23,
    PatientTag_newbabyUmbilical: 24,
    PatientTag_newmom: 25,
    PatientTag_SecureTagAdult: 26,
    PatientTag_SecureTagnewbaby: 27,
    PatientTag_SecureTagRugged: 28,
    PatientTag_SingleUse: 29,
    StaffTag: 30,
    StaffTag_Duress: 31,
    StaffTag_MM: 32,
    SurveyTag: 33,
    UniversalTransmitter: 34,
    AmbientSensor_TempHumidity: 35,
    AmbientSensor_TempHumNonDsp: 36,
    CO2Sensor_Display: 37,
    CO2Sensor_G1Display: 38,
    DiffAirPressureSensor_G1: 39,
    DifferentialAirPressureSensor: 40,
    O2Sensor_Display: 41,
    O2Sensor_G1Display: 42,
    TemperatureSensor_G1NonDsp: 43,
    TemperatureSensor_Standard: 44,
    TemperatureSensor_StndNonDsp: 45,
    TemperatureSensor_Ultra_Low: 46,
    TemperatureSensor_VAC: 47    
};

function IsEMTag(TypeId) {

    if (TypeId == enumTagSubType.AmbientSensor_TempHumidity ||
        TypeId == enumTagSubType.AmbientSensor_TempHumNonDsp ||
        TypeId == enumTagSubType.CO2Sensor_Display ||
        TypeId == enumTagSubType.CO2Sensor_G1Display ||
        TypeId == enumTagSubType.DiffAirPressureSensor_G1 ||
        TypeId == enumTagSubType.DifferentialAirPressureSensor ||
        TypeId == enumTagSubType.O2Sensor_Display ||
        TypeId == enumTagSubType.O2Sensor_G1Display ||
        TypeId == enumTagSubType.TemperatureSensor_G1NonDsp ||
        TypeId == enumTagSubType.TemperatureSensor_Standard ||
        TypeId == enumTagSubType.TemperatureSensor_StndNonDsp ||
        TypeId == enumTagSubType.TemperatureSensor_Ultra_Low ||
        TypeId == enumTagSubType.TemperatureSensor_VAC) 
    {
        return "1";
    }

    return "0"; ;
}

function IsMMTag(TypeId) {

    if (TypeId == enumTagSubType.AssetTag_Autoclave ||
        TypeId == enumTagSubType.AssetTag_MiniMM ||
        TypeId == enumTagSubType.AssetTag_MM ||
        TypeId == enumTagSubType.PatientTag_MiniMM ||
        TypeId == enumTagSubType.PatientTag_MM ||
        TypeId == enumTagSubType.StaffTag_MM) {

        return "1";
    }

    return "0"; ;
}