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

function AddAssociationSiteSetup(SiteId, GroupName, GroupId, VHAGroupId, IsAdd) {

    g_IsAdd = IsAdd;

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=AddAssociationSiteSetup",
    {
        sid: SiteId,
        GroupName: GroupName,
        GroupId: GroupId,
        VHAGroupId: VHAGroupId,
        IsAdd: IsAdd
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddAssociationSiteSetup(dsRoot);
        }
        else {
            document.getElementById("divLoading").style.display = "none";
        }
    });
}

function ajaxAddAssociationSiteSetup(dsRoot) {

    try {

        var o_Msg = dsRoot.getElementsByTagName('Msg');

        var msg = (o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);

        if (msg == "" || msg == null)
            msg = "Association Added Successfully.";

        if (g_IsAdd == "1") {
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").value = msg;
            doAfterAssociatedSite();
        }
        else if (g_IsAdd == "2") {
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").value = msg;
            doAfterAssociatedSite();
        }
        else {
            document.getElementById("divAddAssociationSite").style.display = "none";
            doAfterAssociatedSite();
        }
    }
    catch (e) {
        window.location = "UserErrorPage.aspx";
    }
}

function Load_Setup_AssociationSite() {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = ajaxAssociationSiteList;

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetSiteADGroup";

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

function ajaxAssociationSiteList() {

    if (inf_Obj.readyState == 4) {

        if (inf_Obj.status == 200) {

            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblSiteAssociationListInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblSiteAssociationListInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblSiteAssociationListInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);

            var dsRoot = inf_Obj.responseXML.documentElement;

            var o_GroupId = dsRoot.getElementsByTagName('ADGroupId')
            var o_GroupName = dsRoot.getElementsByTagName('ADGroupName')
            var o_VHAGroupId = dsRoot.getElementsByTagName('VHAGroupId')
            var o_VHAGroupName = dsRoot.getElementsByTagName('VHAGroupName')
            var o_SiteId = dsRoot.getElementsByTagName('SiteId')
            var o_SiteName = dsRoot.getElementsByTagName('SiteName')
            var o_Updatedon = dsRoot.getElementsByTagName('Updatedon')

            nRootLength = o_SiteId.length;

            if (nRootLength > 0) {
              
                sTbl.innerHTML = "<thead><tr>" +
                                   "<th class='siteOverview_TopLeft_Box' style='padding-right: 17px;'>#</th>" +
                                   "<th class='siteOverview_Box'>AD Group Name</th>" +
                                   "<th class='siteOverview_Box'>VHA Group Name</th>" +
                                   "<th class='siteOverview_Box'>Site Name</th>" +
                                   "<th class='siteOverview_Box'>Edit</th>" +
                                   "<th class='siteOverview_Box'>Delete</th>" +
                                 "</tr></thead>";

                body = document.createElement('tbody');

                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');
                    var GroupId = setundefined(o_GroupId[i].textContent || o_GroupId[i].innerText || o_GroupId[i].text);
                    var GroupName = setundefined(o_GroupName[i].textContent || o_GroupName[i].innerText || o_GroupName[i].text);
                    var VHAGroupId = setundefined(o_VHAGroupId[i].textContent || o_VHAGroupId[i].innerText || o_VHAGroupId[i].text);
                    var VHAGroupName = setundefined(o_VHAGroupName[i].textContent || o_VHAGroupName[i].innerText || o_VHAGroupName[i].text);
                    var SiteId = setundefined(o_SiteId[i].textContent || o_SiteId[i].innerText || o_SiteId[i].text);
                    var SiteName = setundefined(o_SiteName[i].textContent || o_SiteName[i].innerText || o_SiteName[i].text);
                    var Updatedon = setundefined(o_Updatedon[i].textContent || o_Updatedon[i].innerText || o_Updatedon[i].text);
                                        
                    AddCell(row, i + 1, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, GroupName, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, VHAGroupName, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, SiteName, 'siteOverview_Box_UserLog', "", "", "left", "", "30px", "");
                    AddCell(row, "<img style='cursor:pointer;' id='editAssociation-" + i + "' src='images/imgEdit.png' onclick=showAssociationSiteForEdit(" + GroupId + ",'" + GroupName.replace(new RegExp(" ", "gm"), "_") + "'," + SiteId + "," + VHAGroupId + "); />", "siteOverview_Box_wrap", "", "", "center", "50px", "30px", "8px");
                    AddCell(row, "<img style='cursor:pointer;' id='deleteAssociation-" + i + "' src='images/imgDelete.png' onclick=deleteGroup(" + GroupId + "); />", "siteOverview_Box_wrap", "", "", "center", "50px", "30px", "8px");
                   
                    body.appendChild(row);
                }  
                             
                sTbl.appendChild(body);

                DatatablesLoadGraph("#tblSiteAssociationListInfo", [8], 0, "asc", 10);
            }
            else {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "500px", "40px", "");
                sTbl.appendChild(row);
            }

            document.getElementById("divLoading").style.display = "none";
        } 
    } 
}

function DeleteSiteAssociation(GroupId) {

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=DeleteSiteAssociation",
    {
        GroupId: GroupId
    },
    function (data, status) {
        if (status == "success") {
            AfterDeleteAjax();
        }
        else {
            $("#divLoading").hide();
        }
    });
}

function AfterDeleteAjax() {
    doAfterAssociatedSite();
}

