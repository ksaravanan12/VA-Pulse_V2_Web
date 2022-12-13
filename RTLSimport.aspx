<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RTLSimport.aspx.vb" Inherits="GMSUI.RTLSimport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Javascript/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="Javascript/js_General.js" type="text/javascript"></script>
    <script src="Javascript/jquery.form.js" type="text/javascript"></script>
    <link href="Styles/gms_StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        this.onload = function () {
            $("#divUploadFile").hide();
            //parent.increaseDialogHeight(400);
            if (getParameterByName("Cmd") === "RTLSDetail") {
                $("#divHeader").show();
                $("#divUploadFile").show();
                $("#hdnCmd").val("ImportTemperatureMetaInfo");
                $("#hdnTemperatureMetaSiteId").val(getParameterByName("SiteId"));
                $("#divPreviewSVG").html('');
            }
        }

        $('#TemperatureMetaExcel').live('change', function () {
            if (g_IsValidationExcel === false) {
                $("#divPreviewSVG").html('');
                $("#divPreviewSVG").html('<img src="Images/377.GIF" alt="Uploading...."/>');

                $("#UploadFiles").ajaxForm({
                    target: '#divPreviewSVG',
                    success: function () {
                        if (document.getElementById("trTotalErrors").style.display == "") {
                            $("#divUploadFile").css("height", 600);
                            parent.increaseDialogHeight(700);
                        }

                        if (document.getElementById("trTotalWarnings").style.display == "") {
                            $("#divUploadFile").css("height", 600);
                            parent.increaseDialogHeight(700);
                        }
                    }
                }).submit();
            }
        });

        $('#TemperatureMetaExcel').live('click', function () {
            document.getElementById("tdIsVerified").innerHTML = "";
            document.getElementById("tdUplodedFileName").innerHTML = "";
            this.value = null;
        });

        function submitUpload() {
            parent.increaseDialogHeight(400);
            $("#divUploadFile").css("height", 250);

            $("#divTagMetaSummary").hide();
            $("#divUploading").show();
            $("#UploadFiles").ajaxForm({
                url: "TempUploadFile.aspx?isSubmit=1&SiteId=" + document.getElementById("hdnTemperatureMetaSiteId").value + "&isVerfied=" + $("#tdIsVerified").text() + "&UplodedFileName=" + $("#tdUplodedFileName").text(),
                target: '#divPreviewSVG',
                success: function () {
                    $("#divUploading").hide();
                    if (document.getElementById("trTotalErrors").style.display == "") {
                        $("#divUploadFile").css("height", 600);
                        parent.increaseDialogHeight(700);
                    }

                    if (document.getElementById("trTotalWarnings").style.display == "") {
                        $("#divUploadFile").css("height", 600);
                        parent.increaseDialogHeight(700);
                    }

                }
            }).submit();
        }

        function cancleUpload() {
            document.getElementById("trConfirmUpload").style.display = "none";
            document.getElementById("tdIsVerified").innerHTML = "";
            document.getElementById("tdUplodedFileName").innerHTML = "";
            parent.CompletedUpload();
        }

        var g_IsValidationExcel;
        function checkfilexls(sender, filttype) {
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

        function closeFrame() {
            parent.closeFrameForUploadFile();
            return false;
        }
        
    </script>
</head>
<body>
    <form id="UploadFiles" method="post" enctype="multipart/form-data" action="RTLSimport.aspx">
    <input type="hidden" name="hdnCmd" id="hdnCmd" />
    <div class="col-xs-12" id="divUploadFile" style="height: 100px; display: block; margin-top: 1px;">
        <input type="hidden" name="hdnTemperatureMetaSiteId" id="hdnTemperatureMetaSiteId" />
        <div id="divSVGUpload" style="margin-top: 20px; text-align:center;">
            <label class="clsLALabel">
                Impot RTLS final hardware details excel file.
            </label>
            <div style="height: 10px;">
            </div>
            <div style="padding-left: 10px;" align="center">
                <label class="clsLALabel">
                </label>
                <input type="file" name="TemperatureMetaExcel" id="TemperatureMetaExcel" accept=".xlsx"
                    onchange="return checkfilexls(this,'xlsx');" /><br />
            </div>
            <div id="divPreviewSVG" style="height: 30px;">
            </div>
        </div>
    </div>
    </form>
    <div id="divTagMetaSummary" style="display: none; overflow:hidden; padding-left: 10px;
        padding-right: 10px;height: 50px; " runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="height: 15px;">
            </tr>
            <tr id="trcsvTotalRec" runat="server">
                <td class="clsLALabel" style="text-align: left; width: 150px; display:none;">
                    Total Records&nbsp;:&nbsp;
                </td>
                <td class="clsLALabelGrey" id="tdcsvTotalRec" runat="server" style="text-align: left;">
                </td>
            </tr>
            <tr id="trcsvTotalRecInserted" runat="server">
                <td class="clsLALabel" style="text-align: right; width: 160px;">
                    Total Records Updated&nbsp;:&nbsp;
                </td>
                <td class="clsLALabel" id="tdcsvTotalRecInserted" runat="server" style="text-align: left;">
                </td>
            </tr>
            <tr id="trTotalErrors" runat="server" style="display: none;">
                <td class="clsLALabel" style="text-align: left; width: 150px;">
                    Total Errors&nbsp;:&nbsp;
                </td>
                <td class="clsLALabelGrey" id="tdTotalErrors" runat="server" style="text-align: left;
                    color: #C81305;">
                </td>
            </tr>
            <tr id="trTotalWarnings" runat="server" style="display: none;">
                <td class="clsLALabel" style="text-align: left; width: 150px;">
                    Total Warnings&nbsp;:&nbsp;
                </td>
                <td class="clsLALabelGrey" id="tdTotalWarnings" runat="server" style="text-align: left;
                    color: #D7A714">
                </td>
            </tr>
            <tr style="height: 2px;">
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
                                <div style="overflow: auto; height: 300px;">
                                    <table id="tblErrorRecords" runat="server" border="0" cellpadding="0" cellspacing="0"
                                        width="100%">
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 20px;">
            </tr>
            <tr id="trWarningRecords" runat="server">
                <td colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 100px; font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
                                color: #D7A714; font-size: 12px; font-weight: bold;" align="left">
                                Warnings
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="overflow: auto; height: 100px;">
                                    <table id="tblWarningRecords" runat="server" border="0" cellpadding="0" cellspacing="0"
                                        width="100%">
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;">
            </tr>
            <tr id="trConfirmUpload" runat="server" style="display: none;">
                <td colspan="2">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                        <tr>
                            <td class="clsLALabel" style="color: #069521;">
                                Do you want to continue with the upload?
                            </td>
                            <td>
                                <input type="button" value="Yes" onclick="submitUpload();" class="clsButton"
                                    style="width: 60px;" />&nbsp;<input type="button" value="No" class="clButtonCancel"
                                        style="width: 60px;" onclick="cancleUpload();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="display: none;">
                <td id="tdIsVerified" runat="server" style="display: none;">
                </td>
                <td id="tdUplodedFileName" runat="server" style="display: none;">
                </td>
            </tr>
        </table>
    </div>
    <div id="divUploading" style="display: none;">
        <img src="Images/377.GIF" alt="Uploading...." /></div>
</body>
</html>
