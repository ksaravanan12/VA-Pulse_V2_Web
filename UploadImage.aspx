<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="UploadImage.aspx.vb" Inherits="GMSUI.UploadImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <link rel="stylesheet" href="Styles/multiple-select.css" />
	
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js"></script>
    <script type="text/javascript" src="Javascript/inventory.js"></script>
    <script type="text/javascript" src="Javascript/js_CampusMap.js"></script>
	
    <script language="javascript" type="text/javascript">

        var g_UserRole = 0;
        var GSiteId = "";

        this.onload = function () {

            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;

            Load_Setup_Site();
            getQueryStrings();

            var hdSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;
            document.getElementById("ctl00_headerBanner_drpsitelist").value = hdSiteId;

            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;

            if (g_UserRole == enumUserRoleArr.Admin || g_UserRole == enumUserRoleArr.Engineering || g_UserRole == enumUserRoleArr.Support) {
                $('#tr_admin').show();
            }
            else {
                $('#tr_admin').hide();
            }
        }

        function ReturntoHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }

            location.href = "Home.aspx?sid=" + GSiteId;
        }

    </script>
    <script>

        function leftTrim(element) {
            if (element)
                element.value = element.value.replace(/^\s+/, "");
        }

        var validFilesTypes = ["pdf", "png", "jpg"];

        function ValidateFile() {

            var file = document.getElementById("<%=flpimg.ClientID%>");
            var Description = document.getElementById("<%=txtDescription.ClientID%>");
			
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";

            var path = file.value;
            var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
            var isValidFile = false;

            var path = getParameterByName("path");

            if (typeof path == 'object' || path == "") {
                path = "";
            }
            for (var i = 0; i < validFilesTypes.length; i++) {
                if ((ext == validFilesTypes[i] || ext == "") && Description.value != "") {
                    isValidFile = true;
                    break;
                }
            }

            var filetype = new Array("pdf", "png", "jpg");

            if (!isValidFile) {

                if ((ext == validFilesTypes[i] || ext == "")) {
                    alert("Enter Description");
                }
                else if ((jQuery.inArray(ext, filetype) == -1)) {
                    alert("Invalid file selected!. Please select valid file type: " + validFilesTypes);
                }
                else if (ext == "") {
                    alert("Invalid file selected!. Please select valid file type: " + validFilesTypes);
                }
                else if (Description.value == "") {
                    alert("Enter Description");
                }
            }

            if (ext == "" && Description.value != "") {
                alert("Invalid file selected!. Please select valid file type: " + validFilesTypes);
                isValidFile = false;
            }

            return isValidFile;
        }
    </script>
	
    <asp:Label ID="lblDescriptionMessage" class="clsMapErrorTxt" runat="server" />
    <br />
    <asp:Label ID="lblMsg" class="clsMapErrorTxt" runat="server" />
    <asp:Label ID="lblUserName" Visible="false" class="clsMapErrorTxt" runat="server" />
    <input type="hidden" id="hid_userrole" runat="server" />
    <input type="hidden" id="hdSiteId" runat="server" />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 10px;">
        </tr>
        <tr>
            <td style="padding-left: 20px; padding-right: 20px;" align="center">
                <table cellpadding="0" border="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="subHeader_black" align="center" id="td1" style="height: 25px;">
                        </td>
                    </tr>
                    <tr>
                        <td align='left' valign="middle" class='siteOverviwe_Home_arrow'>
                            <a onclick="ReturntoHome();">
                                <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                    border="0" /></a>
                        </td>
                        <td style='width: 15px;' valign="top">
                        </td>
                        <td align='left' class='SHeader1'>
                            Campus Map
                        </td>
                    </tr>
                    <tr id="tr_admin" style="display: none;">
                        <td align='left' valign="middle">
                            <br />
                            <input type="button" id="btnAddFile" onclick="AddCampusFile();" value="Add File"
                                class="clsExportExcel" />
                        </td>
                    </tr>
                    <tr>
                        <table id="tblAddCampusFile" style="display: none;" cellpadding="0" border="0" cellspacing="0"
                            width="100%">
                            <tr>
                                <td class="sText" align="left">
                                    <asp:Label ID="lblDescription" class="sText" Text="Description" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="sText" align="left">
                                    <asp:TextBox ID="txtDescription" onkeyup="leftTrim(this)" runat="server" Width="300px"
                                        Height="100px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="sText" align="left">
                                    <br />
                                    <label class="clsLALabel">
                                        Upload File</label>
                                    <asp:FileUpload ID="flpimg" Style="width: 200px;" runat="server" />
                                    <%--accept="image/*"--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="sText" align="left">
                                    <br />
                                    <asp:Button ID="btnUploadImg" runat="server" OnClientClick="return ValidateFile();"
                                        class="clsExportExcel" Text="Upload" />
                                    <input type="button" id="btnUploadImgCancel" onclick="CancelCampusFile();" value="Cancel"
                                        class="clsExportExcel" />
                                </td>
                            </tr>
                        </table>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; margin-left: 2%;"
                                id="tblFileInfo">
                            </table>
                        </td>
                    </tr>
                </table>
                <div id="dialogCampus" title="Campus Image" style="display: none;">
                    <label id="lblShowDescription">
                    </label>
                    <br />
                    <img src="" id="image" alt="image" name="image" />
                </div>
            </td>
        </tr>
    </table>
    <div style="position: fixed; top: 325px; left: 900px; display: none;" id="divLoading">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
</asp:Content>
