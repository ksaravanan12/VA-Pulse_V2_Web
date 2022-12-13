<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GeneratePdfView.aspx.vb"
    Inherits="GeneratePdfView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate Pdf</title>
    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <style type="text/css">
        .siteOverview_TopLeft_Box
        {
	        border-top-left-radius: 5px;
	        -webkit-border-top-left-radius: 5px;
	        -moz-border-top-left-radius: 5px;
	        border-top-left-radius: 5px;
	        border: 1px solid #DADADA;
	        background-color:#245e90;
	        color:White;
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        text-align:center;
	        vertical-align: middle;
        }
        
        .siteOverview_Topright_Box
        {
	        border-top-right-radius: 5px;
	        -webkit-border-top-right-radius: 5px;
	        -moz-border-top-right-radius: 5px;
	        border-top-right-radius: 5px;
	        border-bottom:1px solid #DADADA;
	        border-right:1px solid #DADADA;
	        border-top:1px solid #DADADA;
	        background-color:#f5f5f5;
	        color:#454545;
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        text-align:center;
	        vertical-align: middle;
        }
        
        .siteOverview_Box
        {
	        border-top:1px solid #DADADA;
	        border-bottom:1px solid #DADADA;
	        border-right:1px solid #DADADA;
	        background-color:#f5f5f5;
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        text-align:center;
	        vertical-align: middle;
	        color: #454545;
        }
        
        .DeviceList_leftBox
        {
	        border-bottom:1px solid #DADADA;
	        border-left:1px solid #DADADA;
	        border-right:1px solid #DADADA;
	        background-color:#ffffff;
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        text-align:center;
	        vertical-align: middle;
	        color: #454545;	
        }
        
        .siteOverview_cell
        {
	        border-bottom:1px solid #DADADA;
	        border-right:1px solid #DADADA;
	        background-color:#ffffff;
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        text-align:center;
	        vertical-align: middle;
	        color: #454545;	
        }
        
        .clsExportExcel
        {
	        background-color: #245e90;
	        -moz-border-radius: 5px;
	        -webkit-border-radius: 5px;
	        border-radius: 5px;
	        border: none;
	        cursor:pointer;
            font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        color: #ffffff;
	        width:100px;
	        height:25px;
        }
        
        .clsSavePdf
        {
	        background-color: #01970E;
	        -moz-border-radius: 5px;
	        -webkit-border-radius: 5px;
	        border-radius: 5px;
	        border: none;
	        cursor:pointer;
            font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        font-weight:bold;
	        color: #ffffff;
	        width:100px;
	        height:25px;
        }
        
        .clsLALabel
        {
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        color:#313232;
	        font-weight: bold;
	        width:275px;
        }
        
        .clsLALabelGrey
        {
	        font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;
	        font-size: 12px;
	        color:#AAA9A9;
	        font-weight: bold;
	        width:275px;
        }
        
        .popup
        {
            background: #fff;
            position: absolute;
            width: 230px;
            height: 120px;
            z-index: 102;
        }
        
        /*************************************POP UP***********************************************/
        .clsOverlay
        {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000;
            opacity: .80;
            filter: alpha(opacity=80);
            -moz-opacity: .80;
            z-index: 101;
            display: none;
        }
        
        body
        {
            padding: 0px;
            margin: 0px;
        }
    </style>

    <script type="text/javascript" src="Javascript/jquery-1.8.2.min.js"></script>

    <script type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>

    <script type="text/javascript">
        var g_PdfBtnClicked = 0;
        $(function() {
            $(document).on('click', '#btnBack', function(event) {
                if(!g_PdfBtnClicked == 1) {
                    event.preventDefault();
                    
                    var winWidth = (parseInt($(window).width()) / 2) - 115;
                    var winHeight = (parseInt($(window).height()) / 2) - 60;
                    
                    $( "#dialog-Message" ).css({
                        display: 'block',
                        top: winHeight + 'px',
                        left: winWidth + 'px',
                        position: 'fixed'
                    });
                    
                    $( "#divOverlay" ).css({
                        display: 'block'
                    });
                }
                else {
                    location.href = "home.aspx";
                }
            });
            
            $(document).on('click','#btnStay', function() {
                hideDialog();
            });
            
            $(document).on('click','#btnLeave', function() {
                window.history.back();
            });
            
            $(document).on('click','#btnGeneratePdf', function() {
                g_PdfBtnClicked = 1;
            });
            
            $(document).on('change','input[type="checkbox"]', function() {
                g_PdfBtnClicked = 0;
            });
            
            $(document).on('change','input[type="text"]', function() {
                g_PdfBtnClicked = 0;
            });
        });
        
        function hideDialog() {
            $("#divOverlay").hide();
            $("#dialog-Message").fadeOut(300);
            $("#dialog-Message").hide();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td align="center">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="height: 90px;">
                                <td>
                                    <div id="divPdf" style="background-color: #fff; position: fixed; top: 0px; width: 100%;">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="height: 10px;">
                                            </tr>
                                            <tr>
                                                <td valign="middle" style="padding-bottom: 10px; border-bottom: solid 5px #005695;"
                                                    align="center">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 96%; border: solid 0px red;">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <img src="Images/Logo.png" style="width: 233px; height: 40px;" alt="Logo" />
                                                            </td>
                                                            <td align="right">
                                                                <input type="button" id="btnBack" class="clsExportExcel" value="Back" style="width: 80px;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="height: 5px;">
                                            </tr>
                                            <tr>
                                                <td valign="top" align="center">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="96%">
                                                        <tr>
                                                            <td align="left" class="clsLALabel" id="tdSiteName_GeneratePdf" runat="server" style="width: 50%;">
                                                            </td>
                                                            <td align="left" class="clsLALabelGrey" style="width: 15%;">
                                                                Date&nbsp;:&nbsp;<label id="lblDate_GeneratePdf" runat="server"></label>
                                                            </td>
                                                            <td align="left" class="clsLALabelGrey" style="width: 15%;">
                                                                Report&nbsp;Type&nbsp;:&nbsp;<label id="lblReportType_GeneratePdf" runat="server"></label>
                                                            </td>
                                                            <td style="width: 20%;" align="right">
                                                                <input type="button" id="btnGeneratePdf" runat="server" class="clsSavePdf" value="Save Pdf" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="height: 5px;">
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 96%;">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%;" id="tblTagInfo" runat="server">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 30px;">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%;" id="tblMonitorInfo"
                                                    runat="server">
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <!-- MESSAGE -->
        <div id="dialog-Message" style="display: none;">
            <div id="divOverlay" class="clsOverlay">
            </div>
            <div id="popup" class="popup" style="padding: 30px;">
                <div style="color: #2A6493; font-size: 24px; font-weight: bold; font-family: Arial;
                    padding-bottom: 5px">
                    Attention
                </div>
                <div style="color: #373737; font-size: 13px; font-family: Arial; font-weight: bold;
                    padding-bottom: 20px;">
                    Navigating away from this page<br />
                    will clear your data
                </div>
                <div>
                    <input type="button" id="btnStay" class="clsExportExcel" value="Stay on this Page"
                        style="width: 130px;" />
                    <input type="button" id="btnLeave" class="clsExportExcel" value="Leave Page" style="width: 90px;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
