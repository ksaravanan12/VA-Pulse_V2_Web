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

function AddCompanyGroup(GroupId, GroupName, CompanyId, IsAdd) {

    g_IsAdd = IsAdd;

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=AddCompanyGroup",
    {
        GroupId: GroupId,
        GroupName: GroupName,
        CompanyId: CompanyId,
        IsAdd: IsAdd
    },
    function (data, status) {
        if (status == "success") {

            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddCompanyGroup(dsRoot);
        }
        else {
            document.getElementById("divLoading").style.display = "none";
        }
    });
}

function ajaxAddCompanyGroup(dsRoot) {

    try
    {
        var o_Msg = dsRoot.getElementsByTagName('Msg');
        var msg = setundefined(o_Msg[0].textContent || o_Msg[0].innerText || o_Msg[0].text);

        if (msg == "" || msg == null) {               
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").value = "Group Added Successfully.";
        }

        if (g_IsAdd == "1") {
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").value = msg;
            doAfterAddedGroup();
        }
        else if (g_IsAdd == "2") {
            document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").value = msg;
            doAfterAddedGroup();                
        }
        else {
            document.getElementById("tdAddCompany").style.display = "none";
            doAfterAddedGroup();
        }
    }
    catch (e) {
        window.location = "UserErrorPage.aspx";
    }
}

function Load_Company_Association() {

    inf_Obj = CreateInfraXMLObj();

    if (inf_Obj != null) {

        inf_Obj.onreadystatechange = ajaxCompanyAssociation;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=GetCompanyGroup";

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

function ajaxCompanyAssociation() {

    if (inf_Obj.readyState == 4) {
        if (inf_Obj.status == 200) {

            var sTbl, sTblLen, sEmailRow, sShowStatus;

            //Ajax Msg Receiver
            AjaxMsgReceiver(inf_Obj.responseXML.documentElement);

            var sTbl = document.getElementById("tblCompanyGroupInfo");

            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblCompanyGroupInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblCompanyGroupInfo');
            }

            sTblLen = sTbl.rows.length;

            clearTableRows(sTbl, sTblLen);

            var dsRoot = inf_Obj.responseXML.documentElement;
            var o_VHAGroupId = dsRoot.getElementsByTagName('VHAGroupId')
            var o_VHAGroupName = dsRoot.getElementsByTagName('VHAGroupName')
            var o_CompanyId = dsRoot.getElementsByTagName('CompanyId')
            var o_CompanyName = dsRoot.getElementsByTagName('CompanyName')
            var o_Updatedon = dsRoot.getElementsByTagName('Updatedon')

            nRootLength = o_VHAGroupId.length;

            var sno = 0;
       
            if (nRootLength > 0) {

                var head = "<thead><tr>" +
                           "<th class='siteOverview_TopLeft_Box' style='padding-right: 17px;'>S.No</th>" +
                           "<th class='siteOverview_Box'>Company Name</th><th class='siteOverview_Box'>Group Name</th>" +
                           "<th class='siteOverview_Box' style='padding-right: 17px;'>Edit</th>" +
                           "<th class='siteOverview_Box' style='padding-right: 17px;'>Delete</th>" +
                           "</tr></thead>";                 
                
		        sTbl.innerHTML = head;
		      
                body = document.createElement('tbody');
                                
                for (var i = 0; i < nRootLength; i++) {

                    row = document.createElement('tr');

                    var VHAGroupId = setundefined(o_VHAGroupId[i].textContent || o_VHAGroupId[i].innerText || o_VHAGroupId[i].text);
                    var VHAGroupName = setundefined(o_VHAGroupName[i].textContent || o_VHAGroupName[i].innerText || o_VHAGroupName[i].text);
                    var CompanyId = setundefined(o_CompanyId[i].textContent || o_CompanyId[i].innerText || o_CompanyId[i].text);
                    var CompanyName = setundefined(o_CompanyName[i].textContent || o_CompanyName[i].innerText || o_CompanyName[i].text);
                    var Updatedon = setundefined(o_Updatedon[i].textContent || o_Updatedon[i].innerText || o_Updatedon[i].text);

                    sno = sno + 1;
                    
                    AddCell(row, sno, 'siteOverview_Box', "", "", "left", "", "30px", "");
                    AddCell(row, CompanyName, 'siteOverview_Box', "", "", "left", "", "30px", "");
                    AddCell(row, VHAGroupName, 'siteOverview_Box', "", "", "left", "", "30px", "");
                    AddCell(row, "<img id='editCompany-" + i + "' src='images/imgEdit.png' style='cursor:pointer;' onclick=showCompanyAssociationForEdit(" + VHAGroupId + "," + CompanyId + ",'" + encodeURIComponent(VHAGroupName) + "') />", "siteOverview_Box_wrap", "", "", "center", "60px", "30px", "8px");
                    AddCell(row, "<img id='deleteCompany-" + i + "' src='images/imgDelete.png' style='cursor:pointer;' onclick=deleteGroup(" + VHAGroupId + ") />", "siteOverview_Box_wrap", "", "", "center", "60px", "30px", "8px");
                  
                    body.appendChild(row);
                } // for

                sTbl.appendChild(body);

                DatatablesLoadGraph("#tblCompanyGroupInfo", [8], 0, "asc", 10);
            }
            else 
            {
                row = document.createElement('tr');
                AddCell(row, "No Record Found.", 'siteOverview_cell_Full', 3, "", "center", "500px", "40px", "");
                sTbl.appendChild(row);
            }
           
            document.getElementById("divLoading").style.display = "none";           
        } // end if  g_Obj.status
    } // end if  g_Obj.readyState
}

function DeleteCompanyGroup(VHAGroupId) {

    document.getElementById("divLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=DeleteCompanyGroup",
    {
        GroupId: VHAGroupId
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

    document.getElementById("ctl00_ContentPlaceHolder1_lblMessage").innerHTML = "Group Name deleted successfully.";

    $('#Password_dialog').dialog('close');

    doAfterAddedGroup();
}

