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

var inf_Obj;
var g_IsAdd;

function Load_Setup_Site() {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = loadCampusMapList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=CampusMapList&sid=" + sid;

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

function AjaxMsgReceiver(dsRoot) {

    try 
    {
        var o_AjaxMsg = dsRoot.getElementsByTagName('AjaxMsg');

        if (o_AjaxMsg != null) {
            var sAjaxMsg = (o_AjaxMsg[0].textContent || o_AjaxMsg[0].innerText || o_AjaxMsg[0].text);

            if (sAjaxMsg == null) {
                //Redirect to Login
                location.href = 'ApplicationError.aspx?ErrorValue=101';
            }
        }
    }
    catch (e) {

    }
}

function loadCampusMapList() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var sTbl, sTblLen;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblFileInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblFileInfo');
            }
            else if (GetBrowserType() == "isFF") {
                 sTbl = document.getElementById('tblFileInfo');
            }

            sTblLen = sTbl.rows.length;

            //clearTableRows(sTbl, sTblLen);
            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_DataId = dsRoot.getElementsByTagName('DataId')
            var o_FileName = dsRoot.getElementsByTagName('FileName')
            var o_Path = dsRoot.getElementsByTagName('Path')
            var o_UserName = dsRoot.getElementsByTagName('UserName')
            var o_Description = dsRoot.getElementsByTagName('Description')
            var o_Updatedon = dsRoot.getElementsByTagName('Updatedon')

            nRootLength = o_DataId.length;

            //sitename
            if (nRootLength > 0) {
                var head = "";

                head = "<thead><tr><th class='siteOverview_TopLeft_Box' style='padding-right: 17px; height:40px;'>Description</th><th class='siteOverview_Topright_Box' style='padding-right: 15px;'>View</th><th class='siteOverview_Topright_Box' style='padding-right: 15px;'>Delete</th></tr></thead>";               

                sTbl.innerHTML = head;

                body = document.createElement('tbody');

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');

                    var DataId = (o_DataId[i].textContent || o_DataId[i].innerText || o_DataId[i].text);
                    var FileName = (o_FileName[i].textContent || o_FileName[i].innerText || o_FileName[i].text);
                    var Path = (o_Path[i].textContent || o_Path[i].innerText || o_Path[i].text);
                    var UserName = (o_UserName[i].textContent || o_UserName[i].innerText || o_UserName[i].text);
                    var Description = (o_Description[i].textContent || o_Description[i].innerText || o_Description[i].text);

                    AddCell(row, Description, 'siteOverview_Box', "", "", "left", "275px", "30px", "");
                    var imgProfile = "<img src='Images/imgEdit.png' id='CampusMap-" + i + "' onclick=\"ShowImage('" + Path + "','" + Description + "');\" style='cursor: pointer'/>";

                    AddCell(row, imgProfile, "siteOverview_Box", "", "", "left", "50px", "30px", "");

                    var imgDelete = "<img src='Images/delete.png' id='DeleteCampusMap-" + i + "' onclick=\"DeleteFile('" + DataId + "');\" style='cursor: pointer'/>";
                    AddCell(row, imgDelete, "siteOverview_Box", "", "", "left", "50px", "30px", "");
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
            
            document.getElementById("divLoading").style.display = "none";
        } 
    } 
}

function ShowImage(Path, Description) {

    var imageSrc = Path;
    var ext = Path.split('.').pop();

    if (ext == "svg") {

    }
    else if (ext == "pdf") {
        var urlToPdfFile = imageSrc;
        window.open(urlToPdfFile);       
    }
    else {
        document.getElementById('image').src = imageSrc;
        $('#lblShowDescription').text(Description);        

        $("#dialogCampus").dialog({
            //title: titleVal,
            height: 900,
            width: 1400,
            modal: true,
            show: {
                effect: "fade",
                duration: 500
            },
            hide: {
                effect: "fade",
                duration: 500
            },
            close: function(event) {
                onCloseDialog();
            }
        }); 
    }
}

function DeleteFile(DataId) {

    var retVal = confirm("Do you want to delete this Campus Map?");

    if (!retVal) return false;

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = AfterDeleteAjax;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=DeleteCampusMap&sid=" + sid + "&DataId=" + DataId;

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

function AddCampusFile() {
    $('#tblAddCampusFile').show();
    document.getElementById('ctl00_ContentPlaceHolder1_txtDescription').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_flpimg').value = '';
}

function CancelCampusFile() {

    $('#tblAddCampusFile').hide();
}  

//On Close Dialog
function onCloseDialog() {
    inside = false; 
}   

var qs = getQueryStrings();
var sid = qs["sid"]; 

function getQueryStrings() {
    var assoc = {};
    var decode = function(s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }
    
    return assoc;
}

//*********************************************************
//	Function Name	:	AfterDeleteAjax
//	Input			:	none
//	Description		:	Go back to Campus list
//*********************************************************
function AfterDeleteAjax() {
    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {
            Load_Setup_Site();

        } // end if  g_Obj.status
    } // end if  g_Obj.readyState
}