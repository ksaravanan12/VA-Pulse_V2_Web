<%@ Page Title="Tag Details" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TagInfo.aspx.vb" Inherits="GMSUI.TagInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="Styles/elastislide.css" />

    <script type="text/javascript" src="Javascript/js_DevicePhotos.js?d=2"></script>

    <script type="text/javascript" src="Javascript/jquery.tmpl.min.js"></script>

    <script type="text/javascript" src="Javascript/jquery.device_elastislide.js"></script>

    <script type="text/javascript" src="Javascript/gallery_device.js"></script>

    <script id="img-wrapper-tmpl" type="text/x-jquery-tmpl"></script>

    <script language="javascript" type="text/javascript">
        var siteid = "";
        var devicetype = "";
        var tagid = "";
        var g_UserRole = 0;
        var detail = "";
        var typeid = "";

        this.onload = function() {
            g_UserRole = document.getElementById("<%=hid_userrole.ClientID%>").value;

            siteid = getParameterByName("siteid");
            document.getElementById('ctl00_headerBanner_drpsitelist').value = siteid;
            tagid = getParameterByName("tagid");
            typeid = getParameterByName("TagTypeId");           
            loadDeviceDetailsInfoOnClick(siteid, 1, tagid, typeid);
            Device_GetPhoto(siteid, 1, tagid);
            document.getElementById('lblDeviceId_DeviceDetails').innerHTML = tagid;
        }
        function detailsview() {
            detail = 1;
            document.getElementById('trDiagnostics').style.display = "";
            if (detail == 1) {
                document.getElementById('trDiagnostics').style.display = "";
                document.getElementById('divLoading_Graph').style.display = "";
                ShowDateRangeGraph(2);                                
                document.getElementById('trConfiguration').style.display = "none";
                document.getElementById('trCertification').style.display = "none";
                document.getElementById('trImages').style.display = "none";                    
            }
            else if (detail == 2) {
                document.getElementById('trConfiguration').style.display = "";
                document.getElementById('trDiagnostics').style.display = "none";
                document.getElementById('trCertification').style.display = "none";
                document.getElementById('trImages').style.display = "none";       
            }
            else if (detail == 3) {
                document.getElementById('trCertification').style.display = "";
                document.getElementById('trDiagnostics').style.display = "none";
                document.getElementById('trConfiguration').style.display = "none";
                document.getElementById('trImages').style.display = "none";             
            }
            else if (detail == 4) {
                document.getElementById('trImages').style.display = "";
                document.getElementById('trDiagnostics').style.display = "none";
                document.getElementById('trConfiguration').style.display = "none";
                document.getElementById('trCertification').style.display = "none";            
            }
            else if (detail == 5) {
                document.getElementById('trImages').style.display = "";
                document.getElementById('trDiagnostics').style.display = "none";
                document.getElementById('trConfiguration').style.display = "none";
                document.getElementById('trCertification').style.display = "none";
                document.getElementById('trImages').style.display = "none";         
            }
            else {
                document.getElementById('trDiagnostics').style.display = "";
                document.getElementById('trConfiguration').style.display = "";
                document.getElementById('trCertification').style.display = "";
                document.getElementById('trImages').style.display = "";
            }
            $('#imgOtherSupport1').attr('src', 'Images/imgShowOthSupport.png');
            $('#imgOtherSupport2').attr('src', 'Images/imgShowOthSupport.png');
            $('#imgOtherSupport3').attr('src', 'Images/imgShowOthSupport.png');
            $('#imgOtherSupport4').attr('src', 'Images/imgShowOthSupport.png');
            $('#imgOtherSupport5').attr('src', 'Images/imgShowOthSupport.png');
            $('#imgOtherSupport' + detail).attr('src', 'Images/imgHideOthSupport.png');

        }

        function GotoHome() {
            
            if (encodeURIComponent(getParameterByName('IsSearch')) == "1")
                window.history.back();
            else     
                window.location.href = 'Home.aspx#divPatientTag?redirect=' + encodeURIComponent("TagInfo") + '&siteid=' + encodeURIComponent(getParameterByName('siteid')) + '&typeId=' + encodeURIComponent(getParameterByName('TagTypeId')) + '&tagfilter=' + encodeURIComponent(getParameterByName('tagfilter')) + '&pcversion=' + encodeURIComponent(getParameterByName('pcversion')) + '&connectivity=' + encodeURIComponent(getParameterByName('connectivity')) + '&type=' + encodeURIComponent(getParameterByName('type'));
        }
        
        function show_NewStatus(id) {
            if (id == 1) {
                $('#trDiagnostics').toggle();
                if ($('#trDiagnostics').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                    document.getElementById('divLoading_Graph').style.display = "";
                    ShowDateRangeGraph(2);
                }
            }
            else if (id == 2) {
                $('#trConfiguration').toggle();
                if ($('#trConfiguration').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
            else if (id == 3) {
                $('#trCertification').toggle();
                if ($('#trCertification').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
            else if (id == 4) {
                $('#trImages').toggle();
                if ($('#trImages').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
            else if (id == 5) {
                $('#trHistory').toggle();
                if ($('#trHistory').css('display') == 'none') {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgShowOthSupport.png');
                }
                else {
                    $('#imgOtherSupport' + id).attr('src', 'Images/imgHideOthSupport.png');
                }
            }
        }
        
        function loadDeviceDetailsInfoOnClick(siteid, devicetype, deviceid, typeid) {

            var sTblProfile = "", sTblWifi = "";
            sTblProfile = document.getElementById('tblProfile');
            sTblWifi = document.getElementById('tblWiFiDetails');

            sTblProfileLen = sTblProfile.rows.length;
            clearTableRows(sTblProfile, sTblProfileLen);
            var sTbl, sTblLen;
            if (GetBrowserType() == "isIE") {
                sTbl = document.getElementById('tblTagInfo');
            }
            else if (GetBrowserType() == "isFF") {
                sTbl = document.getElementById('tblTagInfo');
            }
            sTblLenWifi = sTblWifi.rows.length;
            clearTableRows(sTbl, sTblLen);
            document.getElementById('Div_MSStackedColumn2D').innerHTML = "";
            DeviceList(siteid, devicetype, tagid, typeid);
        };

        function setRangeLable(lbl, type) {
            if (type == 1)
                document.getElementById(lbl).innerHTML = "Hourly ";
            else if (type == 2)
                document.getElementById(lbl).innerHTML = " Daily ";
            else if (type == 3)
                document.getElementById(lbl).innerHTML = "Weekly ";
            else if (type == 4)
                document.getElementById(lbl).innerHTML = "Monthly";
        }

        function showTip4(text, lf, tp) {
            var elementRef = document.getElementById('tooltip4');
            elementRef.style.position = 'absolute';
            elementRef.innerHTML = text;
            elementRef.style.display = '';
            tempX = lf;
            tempY = tp;
            $("#tooltip4").css({ left: tempX, top: tempY });
        }

        function hideTip4() {
            var elementRef = document.getElementById('tooltip4');
            elementRef.style.display = 'none';
        }

        function chkExpand_onchange() {
            if ($('#chkExpand').prop("checked") == true) {
                document.getElementById('divLoading_Graph').style.display = "";
                ShowDateRangeGraph(2);
                document.getElementById('trDiagnostics').style.display = "";
                document.getElementById('trConfiguration').style.display = "";
                document.getElementById('trCertification').style.display = "";
                document.getElementById('trImages').style.display = "";
                $('#imgOtherSupport1').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport2').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport3').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport4').attr('src', 'Images/imgHideOthSupport.png');
                $('#imgOtherSupport5').attr('src', 'Images/imgHideOthSupport.png');
            }
            else {
                document.getElementById('trDiagnostics').style.display = "none";
                document.getElementById('trConfiguration').style.display = "none";
                document.getElementById('trCertification').style.display = "none";
                document.getElementById('trImages').style.display = "none";
                document.getElementById('trHistory').style.display = "none";
                detail = getParameterByName("detail");
                show_NewStatus(detail);
            }
        }

    </script>

    <script type="text/javascript" src="Javascript/js_TagDetail.js?d=2"></script>

    <script type="text/javascript" src="FusionWidgets/FusionCharts.js"></script>

    <div id="tooltip4" class="tool3" style="display: none;">
    </div>
    <div style="position: fixed; width: 100%; top: 50%; left: 50%" id="divLoading_TagDetails">
        <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
    </div>
    <table id="tblStatusSummary" style="width: 100%;">
        <tr>
            <td>
                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                <tr>
                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow' style="padding-left: 10px;">
                                        <a>
                                            <img src='images/Left-Arrow.png' title='Home' alt='Home' style="width: 16px; height: 24px;"
                                                border="0" onclick="GotoHome();" /></a>
                                    </td>
                                    <td style='width: 10px;' valign="top">
                                    </td>
                                    <td align='left' valign="top">
                                        <table border='0' cellpadding='0' cellspacing='0' style="width: 100%;">
                                            <tr>
                                                <td align='left' style="width: 80%;" class='SHeader1'>
                                                    <label>
                                                        Status Summary
                                                    </label>
                                                </td>
                                                <td class='SHeader1'>
                                                    <input type="checkbox" id="chkExpand" name="chkExpand" onchange="chkExpand_onchange()" /><label>Expand
                                                        All</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='left' style="width: 80%;" class='subHeader_black' id="lblDeviceId_DeviceDetails">
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                    </tr>
                    <tr style="height: 5px;">
                        <td class="bordertop" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblTag" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                    <tr style="height: 10px;">
                    </tr>
                    <tr>
                        <td style="width: 22px;">
                            <img id="imgOtherSupport1" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                width: 30px;" onclick="show_NewStatus(1);" />
                        </td>
                        <td colspan="3" class="Prod_subTitle">
                            Diagnostics
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trDiagnostics" style="display: none;">
            <td>
                <table id="tblEMTemperatureProfile" cellspacing="1" cellpadding="5" style="width: 95.5%;
                    padding-top: 15px;">
                </table>
                <table id="tblEMBattery" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                </table>
                <table id="tblEMStatus" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                </table>
                <table id="tblEMWiFiDetailsHeader" cellspacing="1" cellpadding="5" style="width: 95.5%;
                    padding-top: 15px;">
                    <tr style="height: 20px;">
                    </tr>
                    <tr>
                        <td class="subHeader_black">
                            Last&nbsp;10&nbsp;Hr&nbsp;Location&nbsp;Data:&nbsp;
                        </td>
                    </tr>
                </table>
                <table id="tblEMWiFiDetails" cellspacing="1" cellpadding="5" style="width: 950px; table-layout: fixed;
                    padding-top: 15px;">
                </table>
                <table id="tblEMShippingDetails" cellspacing="1" cellpadding="5" style="width: 95.5%;
                    padding-top: 15px;">
                </table>
                <table id="tblEMPageLocationGraph" cellspacing="1" cellpadding="5" style="width: 95.5%;
                    padding-top: 15px;">
                    <tr style="height: 20px;">
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td style="height: 10px;" class="subHeader_black">
                                        Paging and Location Data&nbsp;:&nbsp;<asp:Label ID="lblEMPagingDataDateFrom" runat="Server"></asp:Label>
                                        <asp:Label ID="lblEMPagingDataDateTo" runat="Server"></asp:Label>
                                        &nbsp;<asp:Label ID="lblEMPagingDataNoOfDays" runat="Server"></asp:Label>
                                    </td>
                                    <td style="width: 10px;" valign="top">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 20px;">
                    </tr>
                    <tr id="trEMPaginationGraph">
                        <td align="right">
                            <table border="0" cellpadding="0" cellspacing="0" width="300px" style="background-color: #F0EDED;">
                                <tr style="height: 36px;">
                                    <td align="left">
                                        <input type="button" id="btnEMPrevGraph" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                            onmouseout="btnPagination_DeviceDetailsOut(this);" runat="server" value=" << "
                                            onclick="ShowPreviousNextGraph(3);" />
                                    </td>
                                    <td style="width: 15px;">
                                        <label id="lblEMDeviceDetails_DateType" class="clsLALabel" style="width: 150px;">
                                        </label>
                                    </td>
                                    <td style="width: 15px;">
                                    </td>
                                    <td>
                                        <img src="images/imgLAAlertsMonthly.png" id="btnEMMonthly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(4);" />
                                    </td>
                                    <td>
                                        <img src="images/imgLAAlertsWeekly.png" id="btnEMWeekly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(3);" />
                                    </td>
                                    <td>
                                        <img src="images/imgLAAlertsDaily.png" id="btnEMDaily_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(2);" />
                                    </td>
                                    <td>
                                        <img src="images/imgLAAlertsHourly.png" id="btnEMHourly_DeviceDetails" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                            onmouseout="btnPagination_DeviceDetailsOut(this);" width="30" height="30" onclick="ShowDateRangeGraph(1);" />
                                    </td>
                                    <td style="width: 15px;">
                                    </td>
                                    <td>
                                        <input type="button" id="btnEMNextGraph" onmouseover="btnPagination_DeviceDetailsOver(this);"
                                            onmouseout="btnPagination_DeviceDetailsOut(this);" runat="server" value=" >> "
                                            onclick="ShowPreviousNextGraph(2);" />
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 20px;">
                    </tr>
                    <tr runat="server">
                        <td align='left' id="tdEMTrendGraph" style="width: 5%; height: 20px; text-align: left;">
                            <div style="position: relative; display: none; width: 100%; top: 200px; left: 400px;"
                                id="divLoading_Graph">
                                <img src="Images/712.GIF" alt="loading...." style="height: 30px; width: 30px;" />
                            </div>
                            <div id="DivEM_MSStackedColumn2D" style="height: 600px;">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                    <tr style="height: 10px;">
                    </tr>
                    <tr>
                        <td style="width: 22px;">
                            <img id="imgOtherSupport2" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                width: 30px;" onclick="show_NewStatus(2);" />
                        </td>
                        <td colspan="3" class="Prod_subTitle">
                            Configuration
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trConfiguration" style="display: none;">
            <td>
                <table id="tblEMTagDetails" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                </table>
                <table id="tblEMProfile" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                    <tr style="height: 10px;">
                    </tr>
                    <tr>
                        <td style="width: 22px;">
                            <img id="imgOtherSupport3" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                width: 30px;" onclick="show_NewStatus(3);" />
                        </td>
                        <td colspan="3" class="Prod_subTitle">
                            Certification
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trCertification" style="display: none;">
            <td>
                <table id="tblEMCertification" cellspacing="1" cellpadding="5" style="width: 95.5%;
                    padding-top: 15px;">
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                    <tr style="height: 10px;">
                    </tr>
                    <tr>
                        <td style="width: 22px;">
                            <img id="imgOtherSupport4" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                width: 30px;" onclick="show_NewStatus(4);" />
                        </td>
                        <td colspan="3" class="Prod_subTitle">
                            Images
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trImages" style="display: none;">
            <td valign="top" style="width: 100%;">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 95.5%; padding-top: 15px;"
                    id="tblPictureDetails">
                    <tr style="height: 5px;">
                        <td style="padding-top: 5px;" align="right" valign="top">
                            <div id="Device_dialog_UploadImage" title="Upload Image" style="display: none;">
                                <iframe id="ifrmDeviceUploadIamge" style="border: none; width: 100%; height: 100%;">
                                </iframe>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <input type="hidden" id="hid_userrole" runat="server" />
                        <td class="siteOverview_TopLeft_Box_DeviceDetailsHeaderText" style="height: 40px;">
                            <div style="float: left;">
                                Images</div>
                            <div style="float: right;">
                                <input type="button" value="Edit" class="clsExportExcel" id="btnEditDeviceImage"
                                    style="width: 100px; display: none; background-color: #4F903A;" />
                                <input type="button" value="Delete" class="clsExportExcel" id="btnDeleteDeviceImage"
                                    style="width: 100px; display: none; background-color: #D22F2F;" />
                                <input type="button" value="Upload Image" class="clsExportExcel" id="btnAddDeviceImage"
                                    style="width: 100px; display: none; margin-right: 4px;" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="DeviceList_leftBox_DeviceDetailsDatasText" id="tdNoData" align="center"
                            height="40px" style="width: 100%; text-align: center; display: none;">
                            No Records found.....
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="DeviceList_leftBox_DeviceDetailsDatasText" style="width: 100%;
                            display: none;" valign="top" rowspan="5" id="tdImage">
                            <div id="rg-gallery" class="rg-gallery" style="width: 100%;">
                                <div style="float: left; width: 49.5%; height: 410px;">
                                    <div class="rg-info">
                                    </div>
                                </div>
                                <div style="float: right; width: 49.5%; height: 410px;">
                                    <div class="rg-image-wrapper" style="height: 380px;">
                                        <div class="rg-image" style="width: 320px; text-align: center; vertical-align: middle;
                                            height: 330px;" id="divImage">
                                        </div>
                                        <div class="rg-thumbs">
                                            <div class="es-carousel-wrapper" style="width: 262px;">
                                                <div class="es-nav">
                                                    <span class="es-nav-prev">Previous</span> <span class="es-nav-next">Next</span>
                                                </div>
                                                <div class="es-carousel">
                                                    <ul id="uiImage">
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                    <tr style="height: 10px;">
                    </tr>
                    <tr>
                        <td style="width: 22px;">
                            <img id="imgOtherSupport5" src="Images/imgShowOthSupport.png" alt="Show" style="height: 30px;
                                width: 30px;" onclick="show_NewStatus(5);" />
                        </td>
                        <td colspan="3" class="Prod_subTitle" style="color: lightgray">
                            History
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trHistory" style="display: none;">
            <td>
                <table id="tblHistory" cellspacing="1" cellpadding="5" style="width: 95.5%; padding-top: 15px;">
                </table>
            </td>
        </tr>
    </table>
 
    
</asp:Content>
