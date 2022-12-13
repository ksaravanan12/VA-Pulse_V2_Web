<%@ Control Language="VB" AutoEventWireup="false" CodeFile="gms_Leftmenu.ascx.vb"
    Inherits="GMSUI.UserControls_gms_Leftmenu" %>

<script type="text/javascript">
  
    $(document).ready(function() {
        $("#ctl00_leftmenu_selExportType").val(2);
    });
    
	var isMobile = {
		CheckAnyMobile : function(a){
						a= a.toLowerCase();
						var testBrowser = a.match(/android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|bada|kindle|iris|maemo|ip(hone|od)|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|symbian|vodafone|wap|iPhone|iPad|iPod/i)
						return testBrowser;
					},

	    Android: function() {
	        return navigator.userAgent.match(/Android/i);
	    },
	    BlackBerry: function() {
	        return navigator.userAgent.match(/BlackBerry/i);
	    },
	    iOS: function() {
	        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
	    },
	    Opera: function() {
	        return navigator.userAgent.match(/Opera Mini/i);
	    },
	    Windows: function() {
	        return navigator.userAgent.match(/IEMobile/i);
	    },
	    any: function() {
	        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
	    }
	};


    function checkBrowser(){

    	if(isMobile.CheckAnyMobile(navigator.userAgent)){
            return true;		
    	}
        else
            return false;
    }
    
    $(document).on('change', '#ctl00_leftmenu_selReportforExport', function() {
	         $("#ctl00_leftmenu_selExportType").prop('disabled', false)
	        if($(this).val() == "5" || $(this).val() == "6") {
            $('#ctl00_leftmenu_chkOnlineInput').prop('checked', false);
            $('#ctl00_leftmenu_chkOnlineInput').prop('disabled', true);
            if($(this).val() == "6")
            {
                $("#ctl00_leftmenu_selExportType").prop('disabled', true)
                $("#ctl00_leftmenu_selExportType").val(1);
            }
        } else {
            if($("option:selected",$("#ctl00_leftmenu_selExportType")).val() == "1") {
                $('#ctl00_leftmenu_chkOnlineInput').prop('checked', false);
                $('#ctl00_leftmenu_chkOnlineInput').prop('disabled', true);
            } else {
                $('#ctl00_leftmenu_chkOnlineInput').prop('disabled', false);
            }
        }
    });
    
    $(document).on('change', '#ctl00_leftmenu_selExportType', function() {
        if($(this).val() == "1") {
            $('#ctl00_leftmenu_chkOnlineInput').prop('checked', false);
            $('#ctl00_leftmenu_chkOnlineInput').prop('disabled', true);
        } else {
            if($("option:selected",$("#ctl00_leftmenu_selReportforExport")).val() == "5") {
                $('#ctl00_leftmenu_chkOnlineInput').prop('checked', false);
                $('#ctl00_leftmenu_chkOnlineInput').prop('disabled', true);
            } else {
                $('#ctl00_leftmenu_chkOnlineInput').prop('disabled', false);
            }
        }
    });
	
    function validateInputs() {
        if ($("#ctl00_leftmenu_selSiteForExport").val() == 0) {
            alert("Select Site !!!");
            return false;
        }

        if ($("#ctl00_leftmenu_selReportforExport").val() == 0) {
            alert("Select any Report type !!!");
            return false;
        }

        if ($("#ctl00_leftmenu_selExportType").val() == 0) {
            alert("Select any Export type !!!");
            return false;
        }

        if (document.getElementById("ctl00_leftmenu_chkOnlineInput").checked == true) {
            if (checkBrowser()) {
                location.href = "GeneratePdfView.aspx?SiteId=" + $("#ctl00_leftmenu_selSiteForExport").val() + "&RptType=" + $("#ctl00_leftmenu_selReportforExport").val() + "&SiteName=" + $("option:selected", $("#ctl00_leftmenu_selSiteForExport")).text();
                return false;
            }
            else {
                location.href = "GeneratePdf.aspx?SiteId=" + $("#ctl00_leftmenu_selSiteForExport").val() + "&RptType=" + $("#ctl00_leftmenu_selReportforExport").val() + "&SiteName=" + $("option:selected", $("#ctl00_leftmenu_selSiteForExport")).text();
                return false;
            }
        }
        else {
            window.open("ReportPdf.aspx?SiteId=" + $("#ctl00_leftmenu_selSiteForExport").val() + "&RptType=" + $("#ctl00_leftmenu_selReportforExport").val() + "&SiteName=" + $("option:selected", $("#ctl00_leftmenu_selSiteForExport")).text() + "&ExportType=" + $("option:selected", $("#ctl00_leftmenu_selExportType")).val());
            return false;
        }

        return false;
    }
    
    $(document).on('click','#btnExport',function(){
        if($.fn.validation()){
            if(GetBrowserType()=="isIE")
            {
                DownloadExcel_AllDeviceType_ForIE();
            }
            else if(GetBrowserType()=="isFF")
            {
                document.getElementById("divLoading_ExportPrint").style.display = "";
                DownloadExcel_AllDeviceType();
            }
        }
    });
    
    $(document).on('click','#btnPrint',function(){
        if($.fn.validation()){
            SiteId = $("#ctl00_leftmenu_selSiteForExport").val();
            RptType = $("#ctl00_leftmenu_selReportforExport").val();
            var SiteName = $("#ctl00_leftmenu_selSiteForExport option:selected").text();
            
            var a = document.createElement('a');
            a.href='Print.aspx?SiteId=' + SiteId + '&RptType=' + RptType;
           
            a.target = '_blank';
            document.body.appendChild(a);
            a.click();
            
            $("#ctl00_leftmenu_selSiteForExport").val("0")
            $("#ctl00_leftmenu_selReportforExport").val("0")
        }
    });
    
    $(document).on('click','#ctl00_leftmenu_btnGenerate2', function(){
        $('#ctl00_leftmenu_hdnSiteId').val($("#ctl00_leftmenu_selSiteForExport").val());
        $('#ctl00_leftmenu_hdnSiteName').val($("#ctl00_leftmenu_selSiteForExport option:selected").text());
        $('#ctl00_leftmenu_hdnRptType').val($("#ctl00_leftmenu_selReportforExport").val());
        location.href = "GeneratePdfView.aspx?SiteId=" + siteid + "&SiteName=" + sitetxt + "&RptType=" + RptType;
    });
    
    $.fn.validation = function(){
        if($("#ctl00_leftmenu_selSiteForExport").val() == 0){
            alert("Select Site !!!");
            return false;
        } 
        
        if($("#ctl00_leftmenu_selReportforExport").val() == 0){
            alert("Select any Report type !!!");
            return false;
        }
        
        return true;
    }
    
    function DownloadExcel_AllDeviceType_ForIE()
    {
        SiteId = $("#ctl00_leftmenu_selSiteForExport").val();
        RptType = $("#ctl00_leftmenu_selReportforExport").val();
        
        $("#ctl00_leftmenu_selSiteForExport").val("0")
        $("#ctl00_leftmenu_selReportforExport").val("0")
        
        location.href = "AjaxConnector.aspx?cmd=DownloadExcel_AllDeviceType_ForIE&sid=" + SiteId + "&Bin=" + RptType;
    }
    
    var g_DTAllObj;

    function DownloadExcel_AllDeviceType() {
        SiteId = $("#ctl00_leftmenu_selSiteForExport").val();
        RptType = $("#ctl00_leftmenu_selReportforExport").val();

        if (g_DTAllObj == null) {
            g_DTAllObj = CreateTagXMLObj();
        }

        if (g_DTAllObj != null) {
            g_DTAllObj.onreadystatechange = ajaxDownloadExcel_AllDeviceType;

            DbConnectorPath = "AjaxConnector.aspx?cmd=DownloadExcel_All&sid=" + SiteId + "&Bin=" + RptType;

            if (GetBrowserType() == "isIE") {
                g_DTAllObj.open("GET", DbConnectorPath, true);
            }
            else if (GetBrowserType() == "isFF") {
                g_DTAllObj.open("GET", DbConnectorPath, true);
            }
            g_DTAllObj.send(null);
        }
        return false;
    }

    function ajaxDownloadExcel_AllDeviceType() {
        if (g_DTAllObj.readyState == 4) {
            if (g_DTAllObj.status == 200) {
                var dsRoot = g_DTAllObj.responseXML.documentElement;

                if (dsRoot != null) {
                    var o_Excel = dsRoot.getElementsByTagName('Excel');
                    var o_Filename = dsRoot.getElementsByTagName('Filename');

                    var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
                    var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

                    //Export table string to CSV
                    tableToCSV(Excel, Filename);

                    document.getElementById("divLoading_ExportPrint").style.display = "none";

                    $("#ctl00_leftmenu_selSiteForExport").val("0")
                    $("#ctl00_leftmenu_selReportforExport").val("0")
                }
            }
        }
    }
    
    var g_DPdfObj;

    function DownloadPdf() {
        SiteId = $("#ctl00_leftmenu_selSiteForExport").val();
        RptType = $("#ctl00_leftmenu_selReportforExport").val();

        if (g_DPdfObj == null) {
            g_DPdfObj = CreateTagXMLObj();
        }

        if (g_DPdfObj != null) {
            g_DPdfObj.onreadystatechange = ajaxDownloadPdf;

            DbConnectorPath = "AjaxConnector.aspx?cmd=DownloadPdf&sid=" + SiteId + "&Bin=" + RptType;

            if (GetBrowserType() == "isIE") {
                g_DPdfObj.open("GET", DbConnectorPath, true);
            }
            else if (GetBrowserType() == "isFF") {
                g_DPdfObj.open("GET", DbConnectorPath, true);
            }
            g_DPdfObj.send(null);
        }
        return false;
    }

    function ajaxDownloadPdf() {
        if (g_DPdfObj.readyState == 4) {
            if (g_DPdfObj.status == 200) {

                var dsRoot = g_DPdfObj.responseXML.documentElement;

                if (dsRoot != null) {
                    var o_Pdf = dsRoot.getElementsByTagName('Pdf');
                    var o_Filename = dsRoot.getElementsByTagName('Filename');

                    var Pdf = (o_Pdf[0].textContent || o_Pdf[0].innerText || o_Pdf[0].text);
                    var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

                    //Export table string to CSV
                    tableToPdf(Pdf, Filename);
                }
            }
        }
    }

</script>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td valign="middle" align="left">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 299px; padding-left: 40px;">
                <tr style="height: 20px;">
                </tr>
                <tr style="height: 40px;">
                    <td>
                        <img src="Images/Connect_Pulse-01.png" style="height: 68px;" alt="Connect Pulse" />
                    </td>
                </tr>
                <tr style="height: 15px;">
                </tr>
                <tr style="height: 40px;">
                    <td valign="top" align="left" class="Lheader1" id="tdLeftHeader" runat="server">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td class="line2">
                                </td>
                            </tr>
                            <tr>
                                <td class="sText_Green">
                                    <a href="#" style="color: #0a9917; text-decoration: none; display: none;" id="txtupdateAvailable"
                                        onclick="reloadPage();">Update Available</a>
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td class="SHeader1">
                                    Profile
                                </td>
                            </tr>
                            <tr>
                                <td class="line1">
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td class="sText1">
                                    <a href="Profile.aspx" style="color: #005695; text-decoration: none;" id="lblPFnameLink" runat="server">
                                        <asp:Label ID="lblPFname" runat="server"></asp:Label></a></td>
                            </tr>
                            <%--<tr>
                                <td class="line2">
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td class="sText2">
                                    Switch Profiles</td>
                            </tr>--%>
                            <tr>
                                <td class="line2">
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td class="sText2">
                                    <a href="Logout.aspx" style="color: #005695; text-decoration: none;">Log Out</a></td>
                            </tr>
                            <tr style="height: 40px;">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td class='SHeader1'>
                                    Generate Report
                                </td>
                                <td align="right" style="padding-right: 20px;">
                                    <div style="position: relative; display: none;" id="divLoading_ExportPrint">
                                        <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="line1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <select id="selSiteForExport" runat="server" style="width: 220px;">
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <select id="selReportforExport" runat="server" style="width: 220px;">
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>
                                                <input type="button" value="Export" class="clsExportExcel" id="btnExport" style="width: 100px;" />
                                                <input type="button" value="Print" class="clsExportExcel" id="btnPrint" style="width: 100px;" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                <select id="selExportType" runat="server" style="width: 220px;">
                                                    <option value="2">PDF</option>
                                                    <option value="1">Excel</option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                            </td>
                                        </tr>
                                        <tr id="trOnlineInput">
                                            <td class="clsLALabel" valign="middle">
                                                <input type="checkbox" id="chkOnlineInput" runat="server" />&nbsp;Online&nbsp;Input
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button type="button" value="Generate" Text="Generate" class="clsExportExcel"
                                                    ID="btnGenerate" runat="server" Style="width: 100px;" OnClientClick="return validateInputs();" />
                                                <%--<input type="button" value="Generate" class="clsExportExcel" id="btnGenerate2" runat="server" style="width: 100px;" />--%>
                                            </td>
                                        </tr>
                                        <tr style="height: 40px;">
                                            <td>
                                                <input type="hidden" id="hid_TrackDevice" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="display:none;">
                    <td valign="top">
                        <a href="http://www.centrakstore.com/" target="_blank">
                            <img src="images/order-battery-bg.png" style="width: 230px; height: 34px;" alt=""
                                border="0" /></a>
                    </td>
                </tr>
                <tr style="height: 20px;">
                </tr>
                <tr>
                    <td class="sText2">
                        <a href="PastAnnouncements.aspx" style="color: #005695; text-decoration: none;">View
                            past announcements</a>
                    </td>
                </tr>
                <tr style="height: 20px;">
                </tr>
                <tr>
                    <td valign="top">
                        <table border="0" cellpadding="0" cellspacing="0" id="tblcriticalAlert" class="BgRed"
                            runat="server">
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td class="linegradiant">
        </td>
    </tr>
</table>
