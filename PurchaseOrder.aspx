<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PurchaseOrder.aspx.vb" Inherits="GMSUI.PurchaseOrder" Title="Purchase Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="Styles/jquery-ui-1.10.4.custom.css" rel="stylesheet">
    <link rel="stylesheet" href="Styles/multiple-select.css" />
	
    <script language="javascript" type="text/javascript" src="Javascript/jquery-ui-1.10.4.custom.js"></script>
    <script type="text/javascript" language="javascript" src="Javascript/js_General.js?d=2"></script>
    <script type="text/javascript" src="Javascript/inventory.js"></script>
	
    <script language="javascript" type="text/javascript">

        var GSiteId = "";

        this.onload = function () {
            var hdSiteId = document.getElementById("ctl00_ContentPlaceHolder1_hdSiteId").value;
            GSiteId = document.getElementById("ctl00_headerBanner_drpsitelist").value;

            document.getElementById("ctl00_headerBanner_drpsitelist").value = hdSiteId;

            $("#ctl00_ContentPlaceHolder1_divPurchaseOrderView").hide();
            $("#ctl00_ContentPlaceHolder1_divPurchaseOrderSummaryView").show();

            LoadPoSummaryOverview();
        }

        $(document).on('change', '#ctl00_headerBanner_drpsitelist', function () {
            ClearPurchaseOrder();

            var siteIdx = document.getElementById("ctl00_headerBanner_drpsitelist").selectedIndex;
            var siteVal = document.getElementById("ctl00_headerBanner_drpsitelist").options[siteIdx].value;

            g_PurchaseOrderSummaryLoaded = false;
            DisplayPurchaseOrder(1);
        });

        function ReturntoHome() {
            if (setundefined(GSiteId) == "") {
                GSiteId = 0;
            }
            location.href = "Home.aspx?sid=" + GSiteId;
        }
		
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 10px;">
        </tr>
        <tr>
            <td style="padding-left: 20px; padding-right: 20px;" align="center">
                <input type="hidden" id="hdSiteId" runat="server" />
                <!-- PURCHASE ORDER SUMMARY VIEW -->
                <div id="divPurchaseOrderSummaryView" runat="server" style="display: none; top: auto;
                    left: auto; height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 85%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr style="height: 20px;">
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="ReturntoHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <asp:Label ID="lblSiteName_POSummary" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                PO Summary
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        <img src="Images/list.png" width="30px" height="23px" />
                                                        <br />
                                                        <label class="clsLALabel" onclick="DisplayPurchaseOrder(2);" style="cursor: pointer;">
                                                            PO&nbsp;List</label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 30px;" valign="middle">
                            <td align="center">
                                <div style="display: none;" id="divLoading_PurchaseSummaryView">
                                    <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="center">
                                <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="background-color: #FAFAFA; border: solid 1px #CCC; padding: 4px;">
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="subHeader_black" align="center" id="tdTotalPurchaseOrder" style="height: 25px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="sText" align="center">
                                                        Total Purchase Order
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td style="background-color: #FAFAFA; border: solid 1px #CCC; padding: 4px;">
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="subHeader_black" align="center" id="tdTotalPurchaseQty" style="height: 25px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="sText" align="center">
                                                        Total Purchase Qty
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px;">
                                    </tr>
                                    <tr>
                                        <td style="background-color: #FAFAFA; border: solid 1px #CCC; padding: 4px;">
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="subHeader_black" align="center" id="tdFirstPurchaseDate" style="height: 25px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="sText" align="center">
                                                        First Purchase Date
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td style="background-color: #FAFAFA; border: solid 1px #CCC; padding: 4px;">
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="subHeader_black" align="center" id="tdLastPurchaseDate" style="height: 25px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="sText" align="center">
                                                        Last Purchase Date
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 30px;">
                        </tr>
                    </table>
                </div>
                <!-- PURCHASE ORDER LIST VIEW -->
                <div id="divPurchaseOrderView" runat="server" style="display: none; top: auto; left: auto;
                    height: 850px;">
                    <table cellpadding="0" cellspacing="0" style="width: 85%;">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr style="height: 20px;">
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align='center' valign="middle" class='siteOverviwe_Home_arrow'>
                                                        <a onclick="ReturntoHome();">
                                                            <img src='images/Left-Arrow.png' title='Home' style="width: 16px; height: 24px;"
                                                                border="0" /></a>
                                                    </td>
                                                    <td style='width: 15px;' valign="top">
                                                    </td>
                                                    <td align='left' valign="top" style="width: 581px;">
                                                        <table border='0' cellpadding='0' cellspacing='0'>
                                                            <tr>
                                                                <td align='left' class='SHeader1'>
                                                                    <asp:Label ID="lblSiteName_PurchaseOrderView" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border='0' cellpadding='0' cellspacing='0' width="100%">
                                                                        <tr>
                                                                            <td align='left' class='subHeader_black'>
                                                                                PO&nbsp;List
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 75px;" align="center">
                                                        <img src="Images/imgGraphicview.png" width="30px" height="23px" />
                                                        <br />
                                                        <label class="clsLALabel" onclick="DisplayPurchaseOrder(1);" style="cursor: pointer;">
                                                            Summary</label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px;">
                                                </tr>
                                                <tr style="height: 5px;">
                                                    <td class="bordertop" valign="top" colspan="4">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="2" cellspacing="2" width="100%" class="clsFilterTable">
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>PO&nbsp;No&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <select multiple="multiple" id="drpPODevices" style="width: 400px;">
                                            </select>
                                        </td>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr style="height: 36px;">
                                        <td class="clsLALabel" style="padding-left: 5px; width: 65px;">
                                            <b>Model&nbsp;Item&nbsp;:&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <select multiple="multiple" id="drpModelItem" style="width: 400px;">
                                            </select>
                                        </td>
                                        <td align="right">
                                            <input type="button" id="btnShow_PurchaseOrder" class="clsExportExcel" value=" Show "
                                                onclick="PurchaseOrderPgView('show')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                        <tr id="trPurchaseOrderView">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <!-- PREVIOUS/NEXT -->
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr>
                                                    <td align="right">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr style="height: 40px;">
                                                                <td class="txttotalpage" style="width: 275px;" valign="middle">
                                                                    <asp:Label ID="lblCount_PurchaseOrderView" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <div style="display: none;" id="divLoading_PurchaseOrderView">
                                                                        <img src="Images/377.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                                                                    </div>
                                                                </td>
                                                                <td class="clsTableTitleText" align="right">
                                                                    <input type="button" id="btnPrev_PurchaseOrderView" class="clsPrev" title="Previous"
                                                                        onclick="PurchaseOrderPgView(3);" />
                                                                    <asp:Label ID="Label4" runat="server" CssClass="clsCntrlTxt"> Page </asp:Label>
                                                                    <input id="txtPageNo_PurchaseOrderView" onblur="maskChange(event);" onkeypress="return allowNumberOnly(event)"
                                                                        type="text" size="1" maxlength="10" runat="server" name="txtPageNo" value="1" />
                                                                    <asp:Label ID="lblPgCnt_PurchaseOrderView" runat="server" CssClass="clsCntrlTxt">&nbsp;</asp:Label>&nbsp;
                                                                    <input type="button" id="btnGo_PurchaseOrderView" class="btnGO" value="Go" onclick="PurchaseOrderPgView(1);" />&nbsp;&nbsp;
                                                                    <input type="button" id="btnNext_PurchaseOrderView" class="clsNext" title="Next"
                                                                        onclick="PurchaseOrderPgView(2);" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%" id="tblPurchaseOrderView">
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 30px;">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                        </tr>
                    </table>
                </div>
                <!-- PURCHASE ORDER DETAILS VIEW -->
                <div id="dialog-PurchaseOrder" title="Purchase Order Details" style="display: none;">
                    <div style="display: none;" id="divLoading_PD">
                        <img src="Images/712Gray.GIF" alt="loading...." style="height: 24px; width: 24px;" />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblPurchaseOrderDialog">
                    </table>
                </div>
            </td>
        </tr>
        <script src="javascript/jquery.multiple.select.js" type="text/javascript"></script>
        <script type="text/javascript">
            $('#drpPODevices').multipleSelect();
            $('#drpModelItem').multipleSelect();
        </script>
    </table>
</asp:Content>
