<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Device_uploadFile.aspx.vb" Inherits="GMSUI.Device_uploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Device Upload File</title>
    <link rel="stylesheet" type="text/css" href="Styles/gms_StyleSheet.css" />
    <link rel="stylesheet" type="text/css" href="Styles/jquery.datetimepicker.css" />

    <script type="text/javascript" src="Javascript/jquery.min.js"></script>

    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>

    <script type="text/javascript" src="Javascript/jquery.form.js"></script>

    <script type="text/javascript" src="Javascript/jquery.datetimepicker.js"></script>

    <script type="text/javascript">

        this.onload = function () {
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
            $("#divUploadImage").hide();
            if (getParameterByName("Cmd") === "UploadDeviceImage") {
                $("#divUploadImage").show();
                $("#hdnCmd").val("UploadDeviceImage");
            }
            var $loading = $('#divLoading');
            $loading.hide();
            $("#btnSubmitView").hide();
            $("#btnSubmit").show();
            var $rgGallery = $('#rg-image');

            $rgGallery.empty();
            $rgGallery.hide();

            $("#hdnDeviceId").val(getParameterByName("DeviceId"));
            $("#hdnSiteId").val(getParameterByName("SiteId"));
            $("#hdnIsEditMode").val(getParameterByName("IsEditMode"));
            $("#hdnIsAddType").val(getParameterByName("AddType"));

            if (document.getElementById("<%=lblMsg.ClientID%>").innerHTML == "") {
                var path = getParameterByName("path");
                if (typeof path == 'object' || path == "") {
                    document.getElementById("<%=lblFile.ClientID%>").innerHTML = "";
                    $rgGallery.empty();
                    $rgGallery.hide();
                }
                else {
                    $rgGallery.empty().append('<img class="rg-img"  src="' + path + '"/>');
                    $rgGallery.show();
                }
                if (getParameterByName("IsEditMode") == "1") {
                    $("#hdnDataId").val(getParameterByName("dataid"));
                    var deviceinfo = ""
                    deviceinfo = parent.document.getElementsByClassName("rg-label")[0].innerHTML;                    
                    document.getElementById("<%=deviceinfo.ClientID%>").innerHTML = deviceinfo;
                }
                else {
                    $("#hdnDataId").val("");
                    document.getElementById("<%=deviceinfo.ClientID%>").innerHTML = "";
                }
            }
            else if (document.getElementById("<%=lblMsg.ClientID%>").innerHTML == "The device image is not added.") {
                $("#divUploadImage *").attr("disabled", "disabled").off('click');
                setTimeout(function() {   //calls click event after a certain time
                    Close();
                }, 1500);
            }


        }

        function Close() {
            parent.CloseUploadDeviceIamgeWindow(getParameterByName("SiteId"), "1", getParameterByName("DeviceId"), document.getElementById("<%=hdnIsAdd.ClientID%>").value);
            return true;
        }



        var validFilesTypes = ["gif", "png", "jpg"];
        function ValidateFile() {

            var file = document.getElementById("<%=bgimg.ClientID%>");
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
            var path = file.value;
            var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
            var isValidFile = false;
            var path = getParameterByName("path");
            if (typeof path == 'object' || path == "") {
                path = "";
            }
            for (var i = 0; i < validFilesTypes.length; i++) {
                if (ext == validFilesTypes[i] || ext == "") {
                    isValidFile = true;
                    break;
                }
            }
            if (!isValidFile) {
                document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "Invalid file selected!. Please select valid file type: " + validFilesTypes;
            }

            if (ext == "" && path == "" && document.getElementById("<%=deviceinfo.ClientID%>").value == "") {
                isValidFile = false;
                document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "Please upload file or give info!";
            }
            if (document.getElementById("<%=hdnIsAdd.ClientID%>").value == "Added" && document.getElementById("<%=lblMsg.ClientID%>").innerHTML == "") {
                isValidFile = false;
            }
            else if (document.getElementById("<%=lblMsg.ClientID%>").innerHTML == "") {
                document.getElementById("<%=hdnIsAdd.ClientID%>").value = "Added";
                isValidFile = true;
                var $loading = $('#divLoading');
                $loading.show();
                $("#btnSubmitView").show();
                $("#btnSubmit").hide();
            }
            return isValidFile;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function AddMoreImages() {
            if (!document.getElementById && !document.createElement)
                return false;
            var fileUploadarea = document.getElementById("fileUploadarea");
            if (!fileUploadarea)
                return false;
            var newLine = document.createElement("br");
            fileUploadarea.appendChild(newLine);
            var newFile = document.createElement("input");
            newFile.type = "file";
            newFile.setAttribute("class", "fileUpload");
            newFile.setAttribute("multiple", "true");

            if (!AddMoreImages.lastAssignedId)
                AddMoreImages.lastAssignedId = 100;
            newFile.setAttribute("id", "FileUpload" + AddMoreImages.lastAssignedId);
            newFile.setAttribute("name", "FileUpload" + AddMoreImages.lastAssignedId);
            var div = document.createElement("div");
            div.appendChild(newFile);
            div.setAttribute("id", "div" + AddMoreImages.lastAssignedId);
            fileUploadarea.appendChild(div);
            AddMoreImages.lastAssignedId++;
        }
   
    </script>
</head>
<body>
    <form id="DeviceUploadFiles" runat="server">
    <%--method="post" enctype="multipart/form-data" action="Device_uploadFile.aspx">--%>
    <input type="hidden" runat="server" name="hdnCmd" id="hdnCmd" />
    <input type="hidden" runat="server" name="hdnDeviceId" id="hdnDeviceId" />
    <input type="hidden" runat="server" name="hdnSiteId" id="hdnSiteId" />
    <input type="hidden" runat="server" name="hdnDataId" id="hdnDataId" />
    <input type="hidden" runat="server" name="hdnIsEditMode" id="hdnIsEditMode" />
    <input type="hidden" runat="server" name="hdnIsAdd" id="hdnIsAdd" />
    <div id="divUploadImage">
        <div id="divImage">
            <table>
                <tr>
                    <td>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblMsg" class="clsMapErrorTxt" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="rg-image" id="rg-image" style="width: 465px; text-align: center; display: none;
                            vertical-align: middle; height: 330px;">
                            <img class="rg-img" src=""></img></div>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="vertical-align: top;">
                        <label class="clsLALabel">
                            Info :
                        </label>
                    </td>
                    <td align="left">
                        <textarea name="deviceinfo" runat="server" id="deviceinfo" style="resize: none;"
                            rows="6" cols="60"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="vertical-align: top; padding-top: 18px;">
                        <label class="clsLALabel">
                            Upload File :
                        </label>
                    </td>
                    <td align="left">
                        </br>
                        <div id="fileUploadarea">
                            <label id="lblFile" runat="server" class="clsMapErrorTxt"></label>
                            <asp:FileUpload ID="bgimg" CssClass="fileUpload" runat="server" multiple="true" accept="image/*" />
                            </br>
                        </div>
                        </br>
                        <input style="display: block;" id="btnAddMoreFiles" type="button" value="Add more images"
                            onclick="AddMoreImages();" /></br>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSubmit" runat="server" OnClientClick="return ValidateFile();"
                            Text="Submit" />
                        <asp:Button ID="btnSubmitView" runat="server" Enabled="false" Text="Submit" />
                        <asp:Button ID="btnCancel" OnClientClick="return Close();" runat="server" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
            <tr>
                <td style="height: 10px;">
                </td>
                <td colspan="1" align="center">
                    <label id="lblAddMsg" runat="server" class="clsMapSuccessTxt">
                    </label>
                </td>
            </tr>
        </table>
    </div>
    <div id="divPreviewSVG" align="center">
    </div>
    <div style="position: absolute; width: 500px; top: 100px; vertical-align: top; text-align: center;"
        id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    </form>
</body>
</html>

