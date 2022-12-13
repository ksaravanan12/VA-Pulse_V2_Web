// JScript File
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


var device_photo_Obj;
var DbConnectorPath;
var PhotoDeviceId = 0;

function Device_GetPhoto(SiteId, DeviceType, DeviceId) {
 
    if (document.getElementById("divImage"))
        document.getElementById("divImage").innerHTML="";

    var g_UserRole = 0;
    PhotoDeviceId = DeviceId;

    g_UserRole = document.getElementById("ctl00_ContentPlaceHolder1_hid_userrole").value;
    
    document.getElementById('btnAddDeviceImage').style.display = "none";

    if (g_UserRole != enumUserRoleArr.Customer || g_IsTempMonitoring == "True") {
        document.getElementById('btnAddDeviceImage').style.display = "";   
    }
    
    device_photo_Obj = CreateXMLObj();

    if (device_photo_Obj != null) {
        device_photo_Obj.onreadystatechange = ajaxMetaDataInfo;

        DbConnectorPath = "AjaxConnector.aspx?cmd=Device_GetPhoto&sid=" + SiteId + "&DeviceType=" + DeviceType + "&DeviceId=" + DeviceId;

        if (GetBrowserType() == "isIE") {
            device_photo_Obj.open("GET", DbConnectorPath, false);
        }
        else if (GetBrowserType() == "isFF") {
            device_photo_Obj.open("GET", DbConnectorPath, true);
        }

        device_photo_Obj.send(null);
    }
    return false;
}

var dsRoot;

function ajaxMetaDataInfo() {
    if (device_photo_Obj.readyState == 4) {
        if (device_photo_Obj.status == 200) {
            //Ajax Msg Receiver
            AjaxMsgReceiver(device_photo_Obj.responseXML.documentElement);
            dsRoot = device_photo_Obj.responseXML.documentElement;

            Device_GetPhotoInfo();
        }
    }
}

//*********************************************************
//	Function Name	:	AfterDeleteAjax
//	Input			:	none
//	Description		:	Go back to user list
//*********************************************************
function AfterDeleteAjax() {
    if (device_photo_Obj.readyState == 4) {
        if (device_photo_Obj.status == 200) {
            
            document.getElementById("divLoading").style.display = "none";           
                      
            $("#divLoading_TagDetails").show();
            Device_GetPhoto(SiteId, "1", PhotoDeviceId);
            
        } 
    } 
}

//*********************************************************
//	Function Name	:	Delete device photo
//	Input			:	DataId
//	Description		:	DeviceDeletePhoto (ajax Call)
//*********************************************************
function DeviceDeletePhoto(SiteId, DeviceType, DeviceId, DataId) {

    device_photo_Obj = CreateXMLObj();

    if (device_photo_Obj != null) {
        device_photo_Obj.onreadystatechange = AfterDeleteAjax;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=Device_DeletePhoto&sid=" + SiteId + "&DeviceType=" + DeviceType + "&DeviceId=" + DeviceId + "&DataId=" + DataId;

        if (GetBrowserType() == "isIE") {
            device_photo_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            device_photo_Obj.open("GET", DbConnectorPath, true);
        }
        device_photo_Obj.send(null);
    }
}

function Device_GetPhotoInfo() {

    $("#tdImage").hide();
    
    $("#btnEditDeviceImage").hide();
    $("#btnDeleteDeviceImage").hide();    
    
    var g_UserRole = 0;
    var sImageHTML = "";

    var O_DeviceId = dsRoot.getElementsByTagName('DeviceId');
    var O_Picture = dsRoot.getElementsByTagName('Path');
    var O_Info = dsRoot.getElementsByTagName('Info');
    var O_DataId = dsRoot.getElementsByTagName('DataId');
    var O_ThumbnailsPicture = dsRoot.getElementsByTagName('thumbnailUrl');      
  
    var nRootLength = O_DataId.length;

    if (nRootLength > 0) {
       
        $("#tdNoData").hide();       
        
        for (var i = 0; i <= nRootLength - 1; i++) {
        
            var Picture = (O_Picture[i].textContent || O_Picture[i].innerText || O_Picture[i].text);
            var Info = (O_Info[i].textContent || O_Info[i].innerText || O_Info[i].text);
            var DataId = (O_DataId[i].textContent || O_DataId[i].innerText || O_DataId[i].text);
            var ThumbnailsPicture = (O_ThumbnailsPicture[i].textContent || O_ThumbnailsPicture[i].innerText || O_ThumbnailsPicture[i].text);
            
            g_UserRole = document.getElementById("ctl00_ContentPlaceHolder1_hid_userrole").value;

            if (setundefined(Picture) != "") {
                                   
                Info = setundefined(Info);

                if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support || g_IsTempMonitoring == "True") {
                    document.getElementById('btnEditDeviceImage').style.display = "";
                    document.getElementById('btnDeleteDeviceImage').style.display = "";
                }
                
                DataId = setundefined(DataId);
                sImageHTML += "<li><a href='#'><img src='" + ThumbnailsPicture + "' style='width:36px; height:44px;' data-ids='" + DataId + "' data-description='" + Info + "' data-large='" + Picture + "' alt='image" + i + "' onclick='GetImageIdForDeleteImage(" + DataId + ");' /></a></li>";
            }
            else {

                if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support || g_IsTempMonitoring == "True") {                
                
                    document.getElementById('btnEditDeviceImage').style.display = "";
                    document.getElementById('btnDeleteDeviceImage').style.display = "";
                }
                
                Info = setundefined(Info);
                DataId = setundefined(DataId);
                Picture = "Images/NoImages.png";
                sImageHTML += "<li><a href='#'><img src='" + ThumbnailsPicture + "' style='width:36px; height:44px;' data-ids='" + DataId + "' data-description='" + Info + "' data-large='" + Picture + "' alt='image" + i + "' onclick='GetImageIdForDeleteImage(" + DataId + ");'/></a></li>";
            }
            
            if (i == 0)           
                GetImageIdForDeleteImage(DataId);                
        }
        
        sImageHTML += "<li></li>";
        var $new = $(sImageHTML);
        var $rgGallery = $('#rg-gallery'),
	    $esCarousel = $rgGallery.find('div.es-carousel-wrapper');
        $esCarousel.find('ul').empty();
        Gallery.addItems($new);
        Gallery.init();
        $("#tdImage").show();       
    }
    else {
        $("#tdNoData").show();
        $("#tdImage").hide();        
    }
    
    if(trim(document.getElementById("tblTagDetails").innerHTML).length > 0)
        $("#divLoading_TagDetails").hide();        
}

var ImageId="0";

function GetImageIdForDeleteImage(DataId)
{
    ImageId="0";
    ImageId=DataId;
}

$(document).on('click', '#btnAddDeviceImage', function() {     
     
    Editmode = "0";

    $("#ifrmDeviceUploadIamge").attr("src", "Device_uploadFile.aspx?Cmd=UploadDeviceImage&DeviceId=" + PhotoDeviceId + "&SiteId=" + SiteId + "&IsEditMode=" + Editmode);
    var ifr = document.getElementById("ifrmDeviceUploadIamge");
    ifr.contentWindow.location.replace("Device_uploadFile.aspx?Cmd=UploadDeviceImage&DeviceId=" + PhotoDeviceId + "&SiteId=" + SiteId + "&IsEditMode=" + Editmode);

    $("#Device_dialog_UploadImage").dialog({
        height: 400,
        width: 600,
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
        }
    });
    
});

$(document).on('click', '#btnEditDeviceImage', function() {    
 
    var $rgGallery = $('#rg-gallery');   
    var $img = $rgGallery.find('img.rg-img');  
    var path = "";      
     
    path = $img.attr('src');
    
    if (path == "Images/NoImages.png") {
        path = "";
    }
    
    Editmode = "1";

    $("#ifrmDeviceUploadIamge").attr("src", "Device_uploadFile.aspx?Cmd=UploadDeviceImage&DeviceId=" + PhotoDeviceId + "&SiteId=" + SiteId + "&IsEditMode=" + Editmode + "&dataid=" + ImageId + "&path=" + path);
    var ifr = document.getElementById("ifrmDeviceUploadIamge");

    ifr.contentWindow.location.replace("Device_uploadFile.aspx?Cmd=UploadDeviceImage&DeviceId=" + PhotoDeviceId + "&SiteId=" + SiteId + "&IsEditMode=" + Editmode + "&dataid=" + ImageId + "&path=" + path);

    if (path == "") {
        $("#Device_dialog_UploadImage").dialog({
            height: 400,
            width: 600,
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
            }
        });
    }
    else {
        $("#Device_dialog_UploadImage").dialog({
            height: 700,
            width: 600,
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
            }
        });
    }

});

$(document).on('click', '#btnDeleteDeviceImage', function() {
    
    if (confirm("Are you sure do you want to delete this photo from the list?") == true) {
        DeviceDeletePhoto(SiteId, "1", PhotoDeviceId, ImageId);        
    }
    
});

function CloseUploadDeviceIamgeWindow(siteid, DeviceType, DeviceId, msg) {
    
    $('#Device_dialog_UploadImage').dialog('close');
    
    if (msg == "Added") {      
           
            $("#divLoading_TagDetails").show();
            Device_GetPhoto(siteid, "1", PhotoDeviceId);
    }
}