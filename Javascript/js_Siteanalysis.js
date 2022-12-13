var g_Obj;
var g_isExport = 0;
var g_ReportType = 0;

function GetSiteAnalysisReport(SiteId, ReportType, DeviceType, Products) {

    g_ReportType = ReportType;

    if (ReportType == enumSiteAnalysisReport.RAWData || ReportType == enumSiteAnalysisReport.DeviceDetails) 
        g_isExport = 1;    
    else
        document.getElementById("divSiteanalysisChart").innerHTML = "";

    g_Obj = CreateDeviceXMLObj();    

    if (g_Obj != null) {

        g_Obj.onreadystatechange = ajaxSiteanalysischart;

        DbConnectorPath = "AjaxConnector.aspx?cmd=SiteAnalysisReport&sid=" + SiteId + "&ReportType=" + ReportType + "&DeviceType=" + DeviceType + "&Products=" + Products + "&isExport=" + g_isExport;

        if (GetBrowserType() == "isIE") {
            g_Obj.open("GET", DbConnectorPath, true);
        }
        else if (GetBrowserType() == "isFF") {
            g_Obj.open("GET", DbConnectorPath, true);
        }
        g_Obj.send(null);
    }
    return false;
}

var dsRoot;

function ajaxSiteanalysischart() {

    if (g_Obj.readyState == 4) {
        if (g_Obj.status == 200) {

            dsRoot = g_Obj.responseXML.documentElement;

            if (g_ReportType == enumSiteAnalysisReport.DeviceDetails)
                ajaxDownloadDetailsReport();
            else if (g_isExport == 1)
                ExportSiteAnalysisReport(dsRoot);
            else
                LoadajaxSiteAnalysisChart(dsRoot);
        }
    }
}

function ajaxDownloadDetailsReport() {
    if (g_Obj.readyState == 4) {
        if (g_Obj.status == 200) {

            AjaxMsgReceiver(g_Obj.responseXML.documentElement);

            dsRoot = g_Obj.responseXML.documentElement;

            var o_FilePath = dsRoot.getElementsByTagName('CSVPath');
            var filePath = setundefined($($(o_FilePath[0])).text());

            if (filePath != "")
               tableToCSV1(filePath, "");

            document.getElementById("divLoading").style.display = "none";     
        }
    }
}

function tableToCSV1(e, t) {
    var n = document.createElement('a');
    n.setAttribute("href", e);

    var sFile = e.split("/");
    var sFileName = "";

    if (sFile.length > 0) {
        sFileName = sFile[sFile.length - 1];
    }

    n.setAttribute("download", sFileName);
    document.body.appendChild(n);
    n.click()
}

function ExportSiteAnalysisReport(Excel_Root) {

    if (Excel_Root != null) {

        var o_Excel = Excel_Root.getElementsByTagName('Excel');
        var o_Filename = Excel_Root.getElementsByTagName('Filename');

        var Excel = (o_Excel[0].textContent || o_Excel[0].innerText || o_Excel[0].text);
        var Filename = (o_Filename[0].textContent || o_Filename[0].innerText || o_Filename[0].text);

        //Export table string to CSV
        tableToCSV(Excel, Filename);

        document.getElementById("divLoading").style.display = "none";
    }
}

function LoadajaxSiteAnalysisChart(dsRoot) {
  
    var nRootLength;
    var GraphData = "";
    var AvgActivityLevel = 0;
    var GoodPer, UWPer, LBIPer;
    var AvgBatteryLife = 0;

    $("#trBinStatus").hide();

    if (g_ReportType == enumSiteAnalysisReport.ActivityLevelOnline || g_ReportType == enumSiteAnalysisReport.ActivityLevelOffline 
        || g_ReportType == enumSiteAnalysisReport.PagingLocationdatacountratio || g_ReportType == enumSiteAnalysisReport.Triggercountratio) {

        var o_Data = dsRoot.getElementsByTagName('Data');
        var o_NoofDevices = dsRoot.getElementsByTagName('NoofDevices');
        var o_AvgActivityLevel = dsRoot.getElementsByTagName('AvgActivityLevel');

        nRootLength = o_Data.length;

        //Graph XML
        if (nRootLength > 0) {

            AvgActivityLevel = setundefined(o_AvgActivityLevel[0].textContent || o_AvgActivityLevel[0].innerText || o_AvgActivityLevel[0].text);
           
            for (var i = 0; i < nRootLength; i++) {

                Data = setundefined(o_Data[i].textContent || o_Data[i].innerText || o_Data[i].text);
                NoofDevices = setundefined(o_NoofDevices[i].textContent || o_NoofDevices[i].innerText || o_NoofDevices[i].text);

                GraphData += "<set label='" + Data + "' value='" + NoofDevices + "' color='245e90' />";
            }
        }
    }
    else if (g_ReportType == enumSiteAnalysisReport.LBIOnline || g_ReportType == enumSiteAnalysisReport.LBIOffline) {

        var o_LBIValue = dsRoot.getElementsByTagName('LBIValue');
        var o_NoofDevices = dsRoot.getElementsByTagName('NoofDevices');
        var o_GoodPer = dsRoot.getElementsByTagName('GoodPer');
        var o_UWPer = dsRoot.getElementsByTagName('UWPer');
        var o_LBIPer = dsRoot.getElementsByTagName('LBIPer');

        nRootLength = o_LBIValue.length;

        //Graph XML
        if (nRootLength > 0) {

            GoodPer = setundefined(o_GoodPer[0].textContent || o_GoodPer[0].innerText || o_GoodPer[0].text);
            UWPer = setundefined(o_UWPer[0].textContent || o_UWPer[0].innerText || o_UWPer[0].text);
            LBIPer = setundefined(o_LBIPer[0].textContent || o_LBIPer[0].innerText || o_LBIPer[0].text);

            for (var i = 0; i < nRootLength; i++) {

                LBIValue = setundefined(o_LBIValue[i].textContent || o_LBIValue[i].innerText || o_LBIValue[i].text);
                NoofDevices = setundefined(o_NoofDevices[i].textContent || o_NoofDevices[i].innerText || o_NoofDevices[i].text);

                if (g_DeviceType == 2) {

                    if (LBIValue <= 660)
                        GraphData += "<set label='" + LBIValue + "' value='" + NoofDevices + "' color='ff0000' />";
                    else if (LBIValue > 660 && LBIValue <= 710)
                        GraphData += "<set label='" + LBIValue + "' value='" + NoofDevices + "' color='ffc000' />";
                    else
                        GraphData += "<set label='" + LBIValue + "' value='" + NoofDevices + "' color='00b050' />";
                }
                else {
                    if (LBIValue <= 580)
                        GraphData += "<set label='" + LBIValue + "' value='" + NoofDevices + "' color='ff0000' />";
                    else if (LBIValue > 580 && LBIValue <= 660)
                        GraphData += "<set label='" + LBIValue + "' value='" + NoofDevices + "' color='ffc000' />";
                    else
                        GraphData += "<set label='" + LBIValue + "' value='" + NoofDevices + "' color='00b050' />";
                }                
            }
        }
    }
    else if (g_ReportType == enumSiteAnalysisReport.BatteryLife)
    {
        var o_Yrs = dsRoot.getElementsByTagName('Yrs');
        var o_NoofDevices = dsRoot.getElementsByTagName('NoofDevices');
        var o_AvgBatteryLife = dsRoot.getElementsByTagName('AvgBatteryLife');
        
        nRootLength = o_Yrs.length;

        //Graph XML
        if (nRootLength > 0) {

            AvgBatteryLife = setundefined(o_AvgBatteryLife[0].textContent || o_AvgBatteryLife[0].innerText || o_AvgBatteryLife[0].text);

            for (var i = 0; i < nRootLength; i++) {

                Yrs = setundefined(o_Yrs[i].textContent || o_Yrs[i].innerText || o_Yrs[i].text);
                NoofDevices = setundefined(o_NoofDevices[i].textContent || o_NoofDevices[i].innerText || o_NoofDevices[i].text);

                GraphData += "<set label='" + Yrs + "' value='" + NoofDevices + "' color='245e90' />";
            }
        }
    }
    else if (g_ReportType == enumSiteAnalysisReport.BinStatus) {

        var o_Total = dsRoot.getElementsByTagName('Total');
        var o_TotalOnlinePer = dsRoot.getElementsByTagName('TotalOnlinePer');
        var o_TotalOfflinePer = dsRoot.getElementsByTagName('TotalOfflinePer');

        var o_OnlineGoodPer = dsRoot.getElementsByTagName('OnlineGoodPer');
        var o_OfflineGoodPer = dsRoot.getElementsByTagName('OfflineGoodPer');

        var o_OnlineUWPer = dsRoot.getElementsByTagName('OnlineUWPer');
        var o_OfflineUWPer = dsRoot.getElementsByTagName('OfflineUWPer');

        var o_OnlineLBIPer = dsRoot.getElementsByTagName('OnlineLBIPer');
        var o_OfflineLBIPer = dsRoot.getElementsByTagName('OfflineLBIPer');

        var o_TotalOnline = dsRoot.getElementsByTagName('TotalOnline');
        var o_TotalOffline = dsRoot.getElementsByTagName('TotalOffline');

        var o_OnlineGood = dsRoot.getElementsByTagName('OnlineGood');
        var o_OfflineGood = dsRoot.getElementsByTagName('OfflineGood');

        var o_OnlineUW = dsRoot.getElementsByTagName('OnlineUW');
        var o_OfflineUW = dsRoot.getElementsByTagName('OfflineUW');

        var o_OnlineLBI = dsRoot.getElementsByTagName('OnlineLBI');
        var o_OfflineLBI = dsRoot.getElementsByTagName('OfflineLBI');

        nRootLength = o_TotalOnlinePer.length;

        //Graph XML
        if (nRootLength > 0) {

            $("#trBinStatus").show();

            for (var i = 0; i < nRootLength; i++) {

                Total = setundefined(o_Total[i].textContent || o_Total[i].innerText || o_Total[i].text);

                TotalOnlinePer = setundefined(o_TotalOnlinePer[i].textContent || o_TotalOnlinePer[i].innerText || o_TotalOnlinePer[i].text);
                TotalOfflinePer = setundefined(o_TotalOfflinePer[i].textContent || o_TotalOfflinePer[i].innerText || o_TotalOfflinePer[i].text);

                OnlineGoodPer = setundefined(o_OnlineGoodPer[i].textContent || o_OnlineGoodPer[i].innerText || o_OnlineGoodPer[i].text);
                OfflineGoodPer = setundefined(o_OfflineGoodPer[i].textContent || o_OfflineGoodPer[i].innerText || o_OfflineGoodPer[i].text);

                OnlineUWPer = setundefined(o_OnlineUWPer[i].textContent || o_OnlineUWPer[i].innerText || o_OnlineUWPer[i].text);
                OfflineUWPer = setundefined(o_OfflineUWPer[i].textContent || o_OfflineUWPer[i].innerText || o_OfflineUWPer[i].text);

                OnlineLBIPer = setundefined(o_OnlineLBIPer[i].textContent || o_OnlineLBIPer[i].innerText || o_OnlineLBIPer[i].text);
                OfflineLBIPer = setundefined(o_OfflineLBIPer[i].textContent || o_OfflineLBIPer[i].innerText || o_OfflineLBIPer[i].text);
                                
                TotalOnline = setundefined(o_TotalOnline[i].textContent || o_TotalOnline[i].innerText || o_TotalOnline[i].text);
                TotalOffline = setundefined(o_TotalOffline[i].textContent || o_TotalOfflinePer[i].innerText || o_TotalOffline[i].text);

                OnlineGood = setundefined(o_OnlineGood[i].textContent || o_OnlineGood[i].innerText || o_OnlineGood[i].text);
                OfflineGood = setundefined(o_OfflineGood[i].textContent || o_OfflineGood[i].innerText || o_OfflineGood[i].text);

                OnlineUW = setundefined(o_OnlineUW[i].textContent || o_OnlineUWPer[i].innerText || o_OnlineUWPer[i].text);
                OfflineUW = setundefined(o_OfflineUW[i].textContent || o_OfflineUWPer[i].innerText || o_OfflineUWPer[i].text);

                OnlineLBI = setundefined(o_OnlineLBI[i].textContent || o_OnlineLBI[i].innerText || o_OnlineLBI[i].text);
                OfflineLBI = setundefined(o_OfflineLBI[i].textContent || o_OfflineLBI[i].innerText || o_OfflineLBI[i].text);

                document.getElementById("tdOnlineGoodPer").innerHTML = OnlineGoodPer + "%";
                document.getElementById("tdOnlineUWPer").innerHTML = OnlineUWPer + "%";
                document.getElementById("tdOnlineLBIPer").innerHTML = OnlineLBIPer + "%";

                document.getElementById("tdOfflineGoodPer").innerHTML = OfflineGoodPer + "%";
                document.getElementById("tdOfflineUWPer").innerHTML = OfflineUWPer + "%";
                document.getElementById("tdOfflineLBIPer").innerHTML = OfflineLBIPer + "%";

                document.getElementById("tdtTotalOnlinePer").innerHTML = TotalOnlinePer + "%";
                document.getElementById("tdtTotalOfflinePer").innerHTML = TotalOfflinePer + "%";
                
                document.getElementById("tdOnlineGood").innerHTML = OnlineGood;
                document.getElementById("tdOnlineUW").innerHTML = OnlineUW;
                document.getElementById("tdOnlineLBI").innerHTML = OnlineLBI;

                document.getElementById("tdOfflineGood").innerHTML = OfflineGood;
                document.getElementById("tdOfflineUW").innerHTML = OfflineUW;
                document.getElementById("tdOfflineLBI").innerHTML = OfflineLBI;

                document.getElementById("tdtTotalOnline").innerHTML = TotalOnline;
                document.getElementById("tdtTotalOffline").innerHTML = TotalOffline;

                document.getElementById("tdtTotal").innerHTML = Total;

                GraphData += "<set label='Online Good' value='" + OnlineGoodPer + "%" + "' color='00b050' />";
                GraphData += "<set label='Online UW' value='" + OnlineUWPer + "%" + "' color='ffc000' />";
                GraphData += "<set label='Online LBI' value='" + OnlineLBIPer + "%" + "' color='ff0000' />";

                GraphData += "<set label='Offline Good' value='" + OfflineGoodPer + "%" + "' color='00b050' />";
                GraphData += "<set label='Offline UW' value='" + OfflineUWPer + "%" + "' color='ffc000' />";
                GraphData += "<set label='Offline LBI' value='" + OfflineLBIPer + "%" + "' color='ff0000' />";

                GraphData += "<set label='Total Online' value='" + TotalOnlinePer + "%" + "' color='245e90' />";
                GraphData += "<set label='Total Offline' value='" + TotalOfflinePer + "%" + "' color='245e90' />";
            }
        }
    }

    MakeSiteanalysisHistogramChart(GraphData, AvgActivityLevel, GoodPer, UWPer, LBIPer, AvgBatteryLife);
}

function MakeSiteanalysisHistogramChart(GraphData, AvgActivityLevel, GoodPer, UWPer, LBIPer, AvgBatteryLife) {

    var sXML = "";
    var strxAxisName = "";
    var stryAxisName = "";
    var strsubCaption = "";
    var snumberSuffix = "";
    var sModelItem = $("#selProducts").val().toString();

    if (g_DeviceType == 2) 
        stryAxisName = "number of monitors";    
    else 
        stryAxisName = "number of tags";    

    var siteIdx = document.getElementById("ctl00_ContentPlaceHolder1_ddlsite").selectedIndex;
    var SiteName = document.getElementById("ctl00_ContentPlaceHolder1_ddlsite").options[siteIdx].text;
    
    if (g_ReportType == enumSiteAnalysisReport.ActivityLevelOnline) {

        strsubCaption = "Online Tags Activity Level Histogram";
        strxAxisName = " Activity level in % {br} Average activity of online " + sModelItem + " tag on site: " + AvgActivityLevel + "%";       
    }
    else if (g_ReportType == enumSiteAnalysisReport.ActivityLevelOffline) {

        strsubCaption = "Offline Tags Activity Level Histogram";
        strxAxisName = "Activity level in %";
        strxAxisName = "Activity level in % {br} Average activity of offline " + sModelItem + " tag on site: " + AvgActivityLevel + "%";
    }
    else if (g_ReportType == enumSiteAnalysisReport.LBIOnline) {

        if (g_DeviceType == 2)
            strsubCaption = "Online Monitors LBI Histogram";
        else
            strsubCaption = "Online Tags LBI Histogram";

        strxAxisName = "LBI Values {br} Online Good: " + GoodPer + "%   Online UW: " + UWPer + "%   Online LBI: " + LBIPer + "%";  
    }
    else if (g_ReportType == enumSiteAnalysisReport.LBIOffline) {

        if (g_DeviceType == 2)
            strsubCaption = "Offline Monitors LBI Histogram";
        else
            strsubCaption = "Offline Tags LBI Histogram";

        strxAxisName = "LBI Values {br} Offline Good: " + GoodPer + "%   Offline UW: " + UWPer + "%   Offline LBI: " + LBIPer + "%";  
    }
    else if (g_ReportType == enumSiteAnalysisReport.BinStatus) {

        if (g_DeviceType == 2)
            strsubCaption = "% online status count";
        else
            strsubCaption = "% of Tag Count, Online and Bin Status";

        snumberSuffix = "numberSuffix='%'";
        stryAxisName = "";
    }
    else if (g_ReportType == enumSiteAnalysisReport.PagingLocationdatacountratio) {

        strsubCaption = "Paging/Location data count ratio Histogram";
        strxAxisName = "paging/location data ratio %";                 
    }
    else if (g_ReportType == enumSiteAnalysisReport.Triggercountratio) {

        strsubCaption = "Trigger count ratio Histogram";
        strxAxisName = "Trigger count ratio %";
    }
    else if (g_ReportType == enumSiteAnalysisReport.BatteryLife) {

        strsubCaption = "Online+Offline tag battery life Histogram";
        strxAxisName = "battery life of tags in years {br} tag avg battery life is " + AvgBatteryLife + " years";
    }

    sXML = "<chart " + snumberSuffix + " caption='" + SiteName + " " + sModelItem + "' subCaption='" + strsubCaption + "' xAxisName='" + strxAxisName + "' yAxisName='" + stryAxisName + "' showValues='1' showPlotBorder='0' " +
                "yAxisMinValue='0' formatNumberScale='0' plotGradientColor='' animation='0' labelDisplay='' slantLabels='1' bgColor='ffffff' canvasBorderThickness='0' canvasBgColor='ffffff' " +
                "showAlternateHGridColor='0' plotSpacePercent='60' formatNumber='0' showVLineLabelBorder='1' rotateYAxisName='no of Tags' minvalue='0'>" + GraphData +
                  " <styles>" +
                    "  <definition>" +
                           " <style name='myCaptionFont' type='font' font='Arial' size='16' color='000000' bold='1' />" +
                           " <style name='myAxisTitlesFont' type='font' font='Arial' size='14' bold='1' color='#000000' />" +
                           " <style name='mylblFont' type='font' font='Arial' size='14' color='#000000' bold='1'/>" +
                           " <style name='subCaptionFont' type='font' font='Arial' size='15' color='000000' bold='1'/>" +
                           " <style name='myValuesFont' type='font' size='12' color='000000'/>" +
                     " </definition>" +
                     " <application>" +
                           " <apply toObject='caption' styles='myCaptionFont' />" +
                           " <apply toObject='yAxisName' styles='myAxisTitlesFont' />" +
                           " <apply toObject='XAxisName' styles='myAxisTitlesFont' />" +
                           " <apply toObject='datalabels' styles='mylblFont' />" +
                           " <apply toObject='subCaption' styles='subCaptionFont' />" +
                           " <apply toObject='DataValues' styles='myValuesFont' />" +
                     " </application>" +
                " </styles>" +
            "</chart>";

    if (FusionCharts("myChartId")) FusionCharts("myChartId").dispose();
        FusionCharts.setCurrentRenderer('javascript');

    var myChart = new FusionCharts("Column2D", "myChartId", "890", "450", "0", "1");
    myChart.setDataXML(sXML);
    myChart.render("divSiteanalysisChart");

    document.getElementById("divLoading").style.display = "none";
}

var enumSiteAnalysisReport = {

    ActivityLevelOnline: 1,
    ActivityLevelOffline: 2,
    LBIOnline: 3,
    LBIOffline: 4,
    BinStatus: 5,   
    PagingLocationdatacountratio: 6,
    Triggercountratio: 7,
    BatteryLife: 8,
    RAWData: 9,  
    DeviceDetails: 10
}

