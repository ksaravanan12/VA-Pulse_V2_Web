// ------------------------------------------------ Battery Replacement Failure Report -----------------------------------------------------------//
var Excel_Root;

function DownloadBatteryReplacementFailure() {

    document.getElementById("divExcelLoading").style.display = "";

    $.post("AjaxConnector.aspx?cmd=BatteryReplacementFailure",
      {
          sid: $("#ctl00_ContentPlaceHolder1_ddlsite").val()
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              Excel_Root = data.documentElement;
              ajaxDownloadinforamtion();
          }
          else {
              document.getElementById("divExcelLoading").style.display = "none";
          }
      });
}

// ------------------------------------------------ CenTrak Volt Detail Progress Report -----------------------------------------------------------//

function DownloadVoltDetail() {

    if ($("#txtToDate").val() < $("#txtFromDate").val()) {
        alert("End date must be greater than start date!");
        $("#txtToDate").focus();
        return false;
    }

    document.getElementById("divExcelLoading").style.display = "";

    var IsChkShowonlydevicesImanaged = document.getElementById("ChkShowonlydevicesImanaged").checked;
  
    $.post("AjaxConnector.aspx?cmd=CentrakVoltDetailReport",
      {
          sid: $("#ctl00_ContentPlaceHolder1_ddlsite").val(),
          FromDate: $("#txtFromDate").val(),
          ToDate: $("#txtToDate").val(),
          ShowonlydevicesImanaged: IsChkShowonlydevicesImanaged,
      },
      function (data, status) {
          if (status == "success") {
              AjaxMsgReceiver(data.documentElement);
              Excel_Root = data.documentElement;
              ajaxDownloadinforamtion();
          }
          else {
              document.getElementById("divExcelLoading").style.display = "none";
          }
      });
}

/////////////////////////////////////// Historical Temperature Report///////////////////////////////////////

var g_tempObj;

function GetHistoricalTempReport(SiteId,Sitename, DeviceId, fromdate, todate) {

    g_tempObj = CreateDeviceXMLObj();

    if (g_tempObj != null) {

        g_tempObj.onreadystatechange = ajaxHistoricalTempReport;

        document.getElementById("divLoading").style.display = "";

        DbConnectorPath = "AjaxConnector.aspx?cmd=HistoricalTemperatureReport&sid=" + SiteId + "&Sitename=" + Sitename + "&DeviceId=" + DeviceId + "&fromdate=" + fromdate + "&todate=" + todate;

        if (GetBrowserType() == "isIE") {
            g_tempObj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_tempObj.open("GET", DbConnectorPath, true);
        }
        g_tempObj.send(null);
    }
    return false;
}

var dsTempRoot;

function ajaxHistoricalTempReport() {

    if (g_tempObj.readyState == 4) {

        if (g_tempObj.status == 200) {

            dsTempRoot = g_tempObj.responseXML.documentElement;

            ExportHistoricalTempReport(dsTempRoot);
        }
    }
}

function ExportHistoricalTempReport(Excel_Root) {

    if (Excel_Root != null) {

        var o_Error = Excel_Root.getElementsByTagName('Error');
      
        var Error = '';

        if (o_Error[0] != undefined)
        {
            Error = (o_Error[0].textContent || o_Error[0].innerText || o_Error[0].text);
            alert(Error);
        }
        else
        {
            var o_Excel = Excel_Root.getElementsByTagName('Excel');
            var o_Filename = Excel_Root.getElementsByTagName('Filename');

            var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
            var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

            //Export table string to CSV
            tableToCSV(Excel, Filename);
        }

        document.getElementById("divLoading").style.display = "none";
    }
}
