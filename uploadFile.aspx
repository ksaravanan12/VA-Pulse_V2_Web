<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uploadFile.aspx.vb" Inherits="GMSUI.uploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Upload File</title>
	
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
	
    <script type="text/javascript" src="Javascript/jquery.min.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" src="Javascript/jquery.form.js"></script>
	
    <script type="text/javascript">

        this.onload = function () {

            $("#divAddRoom").hide();
            $("#divUploadFile").hide();
            $("#divAddTag").hide();
            $("#divAddMetaInfo").hide();
            $("#divAddLbiList").hide();
            $("#divBatteryTechAddFloorList").hide();
            $("#divImportG2Summary").hide();
            $("#divDisasterRecoveryList").hide();
            $("#divImportDeviceLocation").hide();

            if (getParameterByName("Cmd") === "UploadFile") {

                $("#divUploadFile").show();
                $("#hdnMapCmd").val("UploadFile");
                $("#hdnMapSiteId").val(getParameterByName("SiteId"));
                $("#hdnSource").val(getParameterByName("FloorId"));
                $("#hdnSVGisEdit").val(getParameterByName("isSVGEdit"));
                $("#hdnBGisEdit").val(getParameterByName("isBGEdit"));
                $("#hdnBGUploadFromMap").val(getParameterByName("BGUploadFromMap"));

                $("#divSVG").hide();

                if (getParameterByName("SVGUrl") != "") {
                    $("#divSVG").show();
                    $("#imgSVG").attr({ "src": getParameterByName("SVGUrl") });
                    $("#downloadSVG").attr({ "href": setundefined(getParameterByName("SVGUrl")), "download": setundefined(getParameterByName("SVGUrl")).toString().lastIndexOf('/'), "target": "_blank" });
                }

                $("#divBG").hide();

                if (getParameterByName("BGUrl") != "") {
                    $("#divBG").show();
                    $("#imgBG").attr({ "src": getParameterByName("BGUrl") });
                }

                if (getParameterByName("BGUploadFromMap") == "1") {
                    $("#divSVGUpload").hide();
                }
                else {
                    $("#divSVGUpload").show();
                }
            }
            else if (getParameterByName("Cmd") === "AddRoom") {

                $("#divAddRoom").show();
                $("#hdnTagMapCmd").val("AddRoom");
                $("#hdnRoomSiteId").val(getParameterByName("RoomSiteId"));
                $("#hdnCampusId").val(getParameterByName("CampusId"));
                $("#hdnBuildingId").val(getParameterByName("BuildingId"));
                $("#hdnFloorId").val(getParameterByName("FloorId"));
                $("#hdnUnitId").val(getParameterByName("UnitId"));

                $("#divRoomCSV").hide();

                if (getParameterByName("RoomCSVUrl") != "") {
                    $("#divRoomCSV").show();
                    $("#roomcsvdownload").attr({ "href": getParameterByName("RoomCSVUrl") });
                }
            }
            else if (getParameterByName("Cmd") === "AddTag") {

                $("#divAddTag").show();
                $("#hdnTagMapCmd").val("AddTag");
                $("#hdnTagSiteId").val(getParameterByName("TagSiteId"));

                $("#divTagCSV").hide();

                if (getParameterByName("TagCSVUrl") != "") {
                    $("#divTagCSV").show();
                    $("#tagcsvDownload").attr({ "href": getParameterByName("TagCSVUrl") });
                }
            }
            else if (getParameterByName("Cmd") === "AddDeviceMetaInfo") {

                $("#divAddMetaInfo").show();
                $("#hdnTagMapCmd").val("AddDeviceMetaInfo");
                $("#hdnMetaSiteId").val(getParameterByName("MetaSiteId"));
                $("#hdnMetaFloorId").val(getParameterByName("MetaFloorId"));

                $("#divInfraCSV").hide();

                if (getParameterByName("MetaCSVUrl") != "") {
                    $("#divInfraCSV").show();
                    $("#infracsvDownload").attr({ "href": getParameterByName("MetaCSVUrl") });
                }
            }
            else if (getParameterByName("Cmd") === "AddLbiListForBatteryTech") {

                $("#divAddLbiList").show();
                $("#hdnTagMapCmd").val("AddLbiListForBatteryTech");
                $("#hdnLbiSiteId").val(getParameterByName("SiteId"));
                $("#divLbiCsv").hide();

                if (getParameterByName("TagCSVUrl") == "") {
                    $("#divLbiCsv").show();
                    $("#lbicsvDownload").attr({ "href": getParameterByName("TagCSVUrl") });
                }
            }
            else if (getParameterByName("Cmd") === "AddFloorForBatteryTech") {

                $("#divBatteryTechAddFloorList").show();
                $("#hdnTagMapCmd").val("AddFloorForBatteryTech");
                $("#hdnBatteryTechSiteId").val(getParameterByName("SiteId"));
                $("#divBatteryTechFloorcsv").hide();

                if (getParameterByName("TagCSVUrl") == "") {
                    $("#divBatteryTechFloorcsv").show();
                    $("#BatteryTechcsvFloorDownload").attr({ "href": getParameterByName("TagCSVUrl") });
                }
            }
            else if (getParameterByName("Cmd") === "UploadG2TempData") {

                $("#divImportG2Summary").show();
                $("#hdnTagMapCmd").val("UploadG2TempData");
                $("#hdnBatteryTechSiteId").val(getParameterByName("SiteId"));

            }
            else if (getParameterByName("Cmd") === "ImportDisasterRecovery") {

                $("#divDisasterRecoveryList").show();
                $("#hdnTagMapCmd").val("ImportDisasterRecovery");
                $("#hdnDisasterRecoverySiteId").val(getParameterByName("SiteId"));
            }
            else if (getParameterByName("Cmd") === "ImportDeviceLocations") {

                $("#divImportDeviceLocation").show();

                $("#hdnTagMapCmd").val("ImportDeviceLocations");
                $("#hdnBatteryTechSiteId").val(getParameterByName("SiteId"));
                $("#hdnDeviceType").val(getParameterByName("DeviceType"));
            }
        }

        $('#photoimg').live('change', function () {

            if (g_IsValidation === false) {
                $("#divPreviewSVG").html('');
                $("#divPreviewSVG").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#hdnSourceType").val('7');

                $("#cropimage").ajaxForm({
                    target: '#divPreviewSVG'
                }).submit();
            }
        });

        $('#bgimg').live('change', function () {

            if (g_IsValidation === false) {

                $("#divPreviewBG").html('');
                $("#divPreviewBG").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#hdnSourceType").val('8');

                $("#cropimage").ajaxForm({
                    beforeSend: function () {
                    },
                    uploadProgress: function (event, position, total, percentComplete) {
                    },
                    success: function () {
                        if (getParameterByName("BGUploadFromMap") == "1") {
                            parent.CloseUploadWindow();
                        }
                    },
                    target: '#divPreviewBG'
                }).submit();
            }
        });

        $('#roomcsv').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        $('#metacsv').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        $('#tagcsv').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        $('#lbicsv').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();

            }
        });

        $('#batteryTechcsv').live('change', function () {

            if (g_IsValidationExcel === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        $('#DeviceLocationFile').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        $('#G2TempFile').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');
                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        $('#DisasterRecoverycsv').live('change', function () {

            if (g_IsValidationCSV === false) {
                $("#preview").html('');
                $("#preview").html('<img src="Images/377.GIF" alt="Uploading...."//>');

                $("#tagView").ajaxForm({
                    target: '#preview'
                }).submit();
            }
        });

        var g_IsValidation;

        function checkfile(sender, filttype) {

            g_IsValidation = false;
            var validExts = new Array(".svg", ".jpg", ".png", ".gif");
            var fileExt = sender.value;

            fileExt = fileExt.substring(fileExt.lastIndexOf('.'));

            if (filttype == 'svg' || filttype == 'jpg' || filttype == 'png' || filttype == 'gif') {
                if (validExts.indexOf(fileExt) < 0) {
                    alert("Invalid file selected, valid file type is ." + filttype);
                    g_IsValidation = true;
                    return false;
                }
                else return true;
            }
            else {
                if (fileExt.toLowerCase() != filttype) {
                    alert("Invalid file selected, valid files are of " + validExts.toString() + " types.");
                    g_IsValidation = true;
                    return false;
                }
                else return true;
            }
        }

        var g_IsValidationCSV;

        function checkfileCSV(sender, filttype) {

            g_IsValidationCSV = false;

            var validExts = new Array(".csv");
            var fileExt = sender.value;

            fileExt = fileExt.substring(fileExt.lastIndexOf('.'));

            if (filttype == 'csv') {
                if (validExts.indexOf(fileExt) < 0) {
                    alert("Invalid file selected, valid file type is ." + filttype);
                    g_IsValidationCSV = true;
                    return false;
                }
                else return true;
            }
            else {
                if (fileExt.toLowerCase() != filttype) {
                    alert("Invalid file selected, valid files are of " + validExts.toString() + " types.");
                    g_IsValidationCSV = true;
                    return false;
                }
                else return true;
            }
        }

        var g_IsValidationExcel;

        function checkfileExcel(sender, filttype) {

            g_IsValidationExcel = false;

            var validExts = new Array(".xlsx");
            var fileExt = sender.value;
            fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
            if (filttype == 'xlsx') {
                if (validExts.indexOf(fileExt) < 0) {
                    alert("Invalid file selected, valid file type is ." + filttype);
                    g_IsValidationExcel = true;
                    return false;
                }
                else return true;
            }
            else {
                if (fileExt.toLowerCase() != filttype) {
                    alert("Invalid file selected, valid files are of " + validExts.toString() + " types.");
                    g_IsValidationExcel = true;
                    return false;
                }
                else return true;
            }
        }          
        
    </script>
	
</head>

<body>

    <form id="cropimage" method="post" enctype="multipart/form-data" action="AjaxConnector.aspx?cmd=uploadFile">
    <input type="hidden" name="hdnMapCmd" id="hdnMapCmd" />
	
    <div id="divUploadFile">
        <input type="hidden" name="hdnMapSiteId" id="hdnMapSiteId" />
        <input type="hidden" name="hdnSource" id="hdnSource" />
        <input type="hidden" name="hdnSourceType" id="hdnSourceType" />
        <input type="hidden" name="hdnSVGisEdit" id="hdnSVGisEdit" />
        <input type="hidden" name="hdnBGisEdit" id="hdnBGisEdit" />
        <input type="hidden" name="hdnBGUploadFromMap" id="hdnBGUploadFromMap" />
        <div id="divSVG">
            <a href="#" id="downloadSVG" style="text-align: left;" class="DeviceDetailsLink">Download
                SVG</a>
            <br />
            <img id="imgSVG" height="260px" width="400px" />
            <br />
        </div>
        <div id="divSVGUpload">
            <div id="divPreviewSVG" style="float: left">
            </div>
            <label class="clsLALabel">
                Upload SVG for the floor
            </label>
            <input type="file" name="photoimg" id="photoimg" accept=".svg" onchange="return checkfile(this,'svg');" /><br />
        </div>
        <br />
        <br />
        <div id="divBG">
            <img id="imgBG" height="260px" width="400px" />
            <br />
        </div>
        <div>
            <div id="divPreviewBG" style="float: left">
            </div>
            <label class="clsLALabel">
                Upload Floor Plan Image</label>
            <input type="file" name="bgimg" id="bgimg" accept="image/*" onchange="return checkfile(this,'jpg');" /><br />
        </div>
    </div>
    </form>
    <form id="tagView" method="post" enctype="multipart/form-data" action="uploadFile.aspx">
    <input type="hidden" name="hdnTagMapCmd" id="hdnTagMapCmd" />
    <div id="divAddRoom" style="display: none;">
        <input type="hidden" name="hdnRoomSiteId" id="hdnRoomSiteId" />
        <input type="hidden" name="hdnCampusId" id="hdnCampusId" />
        <input type="hidden" name="hdnBuildingId" id="hdnBuildingId" />
        <input type="hidden" name="hdnFloorId" id="hdnFloorId" />
        <input type="hidden" name="hdnUnitId" id="hdnUnitId" />
        <div id="divRoomCSV">
            <a id="roomcsvdownload" class="DeviceDetailsLink">
                <label style="cursor: pointer;">
                    Download the current room .csv from here</label></a>
            <br />
        </div>
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="roomcsv" id="roomcsv" accept=".csv" onchange="return checkfileCSV(this,'csv');" /><br />
        </div>
    </div>
    <div id="divAddTag" style="display: none;">
        <input type="hidden" name="hdnTagSiteId" id="hdnTagSiteId" />
        <div id="divTagCSV">
            <a id="tagcsvDownload" class="DeviceDetailsLink">
                <label style="cursor: pointer;">
                    Download the current tag .csv from here</label></a>
            <br />
        </div>
        <div style="height: 10px;">
        </div>
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="tagcsv" id="tagcsv" accept=".csv" onchange="return checkfileCSV(this,'csv');" /><br />
        </div>
    </div>
    <div id="divAddMetaInfo" style="display: none;">
        <input type="hidden" name="hdnMetaSiteId" id="hdnMetaSiteId" />
        <input type="hidden" name="hdnMetaFloorId" id="hdnMetaFloorId" />
        <div id="divInfraCSV">
            <a id="infracsvDownload" class="DeviceDetailsLink">
                <label style="cursor: pointer;">
                    Download the current tag .csv from here</label></a><br />
        </div>
        <div style="height: 10px;">
        </div>
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="metacsv" id="metacsv" accept=".csv" onchange="return checkfileCSV(this,'csv');" /><br />
        </div>
    </div>
    <div id="divAddLbiList" style="display: none;">
        <div id="divLbiCsv">
            <input type="hidden" name="hdnLbiSiteId" id="hdnLbiSiteId" />
            <a id="lbicsvDownload" class="DeviceDetailsLink">
                <label style="cursor: pointer;">
                    Download the current Lbi .csv from here</label></a><br />
        </div>
        <div style="height: 10px;">
        </div>
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="lbicsv" id="lbicsv" accept=".csv" onchange="return checkfileCSV(this,'csv');" />
        </div>
    </div>
    <div id="divBatteryTechAddFloorList" style="display: none;">
        <div id="divBatteryTechFloorcsv">
            <input type="hidden" name="hdnBatteryTechSiteId" id="hdnBatteryTechSiteId" />
            <a id="BatteryTechcsvFloorDownload" class="DeviceDetailsLink">
                <label style="cursor: pointer;">
                    Download the current Lbi .csv from here</label></a><br />
        </div>
        <div style="height: 10px;">
        </div>
        <div>
            <input type="hidden" name="hdnDeviceType" id="hdnDeviceType" />
            <label class="clsLALabel">
                Upload .Excel file</label>
            <input type="file" name="batteryTechcsv" id="batteryTechcsv" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                onchange="return checkfileExcel(this,'xlsx');" />
        </div>
    </div>
    <div id="divImportG2Summary" style="display: none;">
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="G2TempFile" id="G2TempFile" accept=".csv" onchange="return checkfileCSV(this,'csv');" />
        </div>
    </div>
    <div id="divDisasterRecoveryList" style="display: none;">
        <input type="hidden" name="hdnDisasterRecoverySiteId" id="hdnDisasterRecoverySiteId" />
        <div style="height: 10px;">
        </div>
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="DisasterRecoverycsv" id="DisasterRecoverycsv" accept=".csv"
                onchange="return checkfileCSV(this,'csv');" />
        </div>
    </div>
    <div id="divImportDeviceLocation" style="display: none;">
        <div>
            <label class="clsLALabel">
                Upload .csv file</label>
            <input type="file" name="DeviceLocationFile" id="DeviceLocationFile" accept=".csv"
                onchange="return checkfileCSV(this,'csv');" />
        </div>
    </div>
    </form>
    <div id="divTagMetaSummary" style="display: none; overflow: auto; padding-left: 10px;
        padding-right: 10px;" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td class="clsLALabel" style="text-align: left; width: 150px;">
                    Total Records&nbsp;:&nbsp;
                </td>
                <td class="clsLALabelGrey" id="tdcsvTotalRec" runat="server" style="text-align: left;">
                </td>
            </tr>
            <tr>
                <td class="clsLALabel" style="text-align: left; width: 150px;">
                    Total Records Inserted&nbsp;:&nbsp;
                </td>
                <td class="clsLALabelGrey" id="tdcsvTotalRecInserted" runat="server" style="text-align: left;">
                </td>
            </tr>
            <tr style="height: 20px;">
            </tr>
            <tr id="trRecordsNotInserted" runat="server">
                <td colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="clsLALabel" style="width: 100px;">
                                Records Not Inserted
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; height: 100px;">
                                    <table id="tblNoRecordsFound" runat="server" border="0" cellpadding="0" cellspacing="0"
                                        width="100%">
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trErrorRecords" runat="server">
                <td colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 100px; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
                                color: #C81305; font-size: 12px; font-weight: bold;" align="left">
                                Errors
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; height: 100px;">
                                    <table id="tblErrorRecords" runat="server" border="0" cellpadding="0" cellspacing="0"
                                        width="100%">
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <br />
    <div id="preview">
    </div>
</body>
</html>
