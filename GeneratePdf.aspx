<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="GeneratePdf.aspx.vb" Inherits="GMSUI.GeneratePdf" Title="Reports - Online Input Pdf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        var g_PdfTop;
        this.onload = function() {
            var siteId = document.getElementById("<%=hdnSiteId.ClientID%>").value;
            $('#ctl00_headerBanner_drpsitelist').val(siteId);
            $('#tdSiteName_GeneratePdf').html($('#ctl00_headerBanner_drpsitelist option:selected').text());
            g_PdfTop = $('#divPdf').offset().top;
        }
        
        var g_PdfBtnClicked = 0;
        var g_ChangeControls = 0;
         function setFlag()
            {
                 g_PdfBtnClicked = 1;
                 return true;    
            }
        
        $(function() {
            $(window).on('beforeunload', function() {
                if(!g_PdfBtnClicked && g_ChangeControls == 1) {
                    return 'Navigating away from this page will clear your data.';
                }
            });
            
           /* $(document).on('click','#ctl00_ContentPlaceHolder1_btnGeneratePdf', function() {
                g_PdfBtnClicked = 1;
            });*/
            
            $(document).on('change','input[type="checkbox"]', function() {
                g_ChangeControls = 1;
                g_PdfBtnClicked = 0;
            });
            
            $(document).on('change','input[type="text"]', function() {
                g_ChangeControls = 1;
                g_PdfBtnClicked = 0;
            });
            
            $(document).on('change', "#ctl00_headerBanner_drpsitelist", function () {
                if(g_ChangeControls == 0)
                    g_PdfBtnClicked = 1;
                
                location.href = "GeneratePdf.aspx?SiteId=" + $(this).val() + "&RptType=" + getParameterByName("RptType") + "&SiteName=" + $("#ctl00_headerBanner_drpsitelist option:selected").text();
            });
        });
    </script>

    <table cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 40px;">
        <tr>
            <td>
                <div id="divGeneratePdf">
                    <table border="0" cellpadding="0" cellspacing="0" width="85%">
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td valign="top">
                                <input type="hidden" id="hdnSiteId" runat="server" />
                                <input type="hidden" id="hdntblHtml" runat="server" />
                                <div id="divPdf" style="background-color: #fff;">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" class="clsLALabel" id="tdSiteName_GeneratePdf" style="width: 40%;">
                                            </td>
                                            <td align="left" class="clsLALabelGrey" style="width: 20%;">
                                                Date&nbsp;:&nbsp;<label id="lblDate_GeneratePdf" runat="server"></label>
                                            </td>
                                            <td align="left" class="clsLALabelGrey" style="width: 20%;">
                                                Report&nbsp;Type&nbsp;:&nbsp;<label id="lblReportType_GeneratePdf" runat="server"></label>
                                            </td>
                                            <td style="width: 20%;" align="right">
                                                <asp:Button id="btnGeneratePdf" runat="server" class="clsSavePdf" Text="Save Pdf"  OnClientClick="return setFlag();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr>
                            <td valign="top">
                                <div style="overflow: auto; height: 500px;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
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
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
