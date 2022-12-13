// JScript File

var g_DeviceDialogView = "";

//Open Add Device Dialog
function OpenAddDeviceDialog(titleVal,btnText)
{
    g_DeviceDialogView = "dialogAddDevice";
            
    var winWidth = 600;
    var winHeight = 250;
    
    $('#btnAddDevice').val(btnText);
    $('#lblAddDialogMsg').html('');
    
    //Open Dialog
    $( "#dialogAddDevice" ).dialog({
        title: titleVal,
        height: winHeight,
        width: winWidth,
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

//On Close Dialog
function onCloseDialog() 
{
     $("#txtDeviceId").val("");
     
     inside = false;
     $('#divCommands').hide();
     
     if(g_IsReloadtoHomePage == 1)
     {
        g_IsReloadtoHomePage = 0;
        $("#divLoading").show();
        location.href = 'Home.aspx';
     }
}   
    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////        ADD / EDIT / DELETE DEVICE         ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var g_AddDevice;
var g_IsDelete = 0;
var g_IsReloadtoHomePage = 0;
//Ajax Call for Add / Edit / Delete Devices
function addEditDeleteDevice(SiteId,DeviceType,DeviceId,isDelete)
{
    if(isDelete == '0')
        $("#divLoading_AddDialogView").show();
    if(isDelete == '1')
        $("#divLoading").show();
                    
    g_IsReloadtoHomePage = 0;
    g_IsDelete = isDelete;

    $.post("AjaxConnector.aspx?cmd=AddEditDeleteDevice",
    {
        Site: SiteId,
        DeviceType: DeviceType,
        DeviceId: DeviceId,
        isDelete: isDelete
    },
    function (data, status) {
        if (status == "success") {
            AjaxMsgReceiver(data.documentElement);
            dsRoot = data.documentElement;
            ajaxAddEditDeleteDevice(dsRoot);
        }
        else {
            if (isDelete == '0')
                $("#divLoading_AddDialogView").hide();
            if (isDelete == '1')
                $("#divLoading").hide();
        }
    });

}       
       
    
//-----------------------
//Add/Delete Device
//-----------------------
//Ajax Response for Add / Edit / Delete Device
function ajaxAddEditDeleteDevice(dsRoot) {    

    var o_Result = dsRoot.getElementsByTagName('Result');
    var Result = (o_Result[0].textContent || o_Result[0].innerText || o_Result[0].text);

    if (g_IsDelete == "0") {
        $('#lblAddDialogMsg').html('');
        if (Result == 0) {
            $('#lblAddDialogMsg').html('Device Added Successfully');
            $('#lblAddDialogMsg').addClass('clsLALabel').removeClass('clsMapErrorTxt');
            g_IsReloadtoHomePage = 1;
        }
        else if (Result == 1) {
            $('#lblAddDialogMsg').html('Error in Adding Device');
            $('#lblAddDialogMsg').removeClass('clsLALabel').addClass('clsMapErrorTxt');
        }
        else if (Result == 2) {
            $('#lblAddDialogMsg').html('Device Already Exists');
            $('#lblAddDialogMsg').removeClass('clsLALabel').addClass('clsMapErrorTxt');
        }
        $('#lblAddDialogMsg').css({ 'visibility': 'visible' });
        $("#divLoading_AddDialogView").hide();

        if ($('#btnAddDevice').val() === "Add") {
            $("#txtDeviceId").val("");
        }
    }

    if (g_IsDelete == "1") {
        $("#divLoading").hide();
        $("#divLoading").show();
        location.href = "Home.aspx";
    }

    /*if(g_CurrDeviceType == 1)
    doLoadTag(SiteId,"",bin,$('#ctl00_ContentPlaceHolder1_txtPageNo').val(),typeId);
    else if(g_CurrDeviceType == 2)
    doLoadInfrastructure(SiteId,"",bin,$('#ctl00_ContentPlaceHolder1_txtPageNo').val(),typeId);
    else if(g_CurrDeviceType == 3)
    doLoadStar(SiteId,"",bin,$('#ctl00_ContentPlaceHolder1_txtPageNo').val(),typeId);*/
    
}