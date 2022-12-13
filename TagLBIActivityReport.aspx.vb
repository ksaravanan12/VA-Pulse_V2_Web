Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class TagLBIActivityReport
        Inherits System.Web.UI.Page

        Public sXMLLBIDiff As String
        Public sXMLBatteryDiff As String

        Dim GMSAPI As New GMSAPI_New.GMSAPI_New

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()

            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "Tag LBI Activity Report", enumPageAction.AccessViolation, "user try to access Tag LBI Activity Report")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                CheckIsFlashPlayerInstalled()

                Dim qExport As Integer = Val(Request.QueryString("qExport"))

                If qExport = 1 Then
                    FillData(False, True)
                Else
                    FillData()
                End If

            End If

        End Sub

        Sub FillData(Optional ByVal IsChkLbiFilter As Boolean = False, Optional ByVal bExportExcel As Boolean = False)

            Dim sStrXml As XmlNode

            Dim dtTagList As New DataTable
            Dim dt As New DataTable

            Dim ds As New DataSet
            Dim tblRow As New HtmlTableRow
            Dim IsDrawnSlopeVertical As Boolean

            Dim nRow, nLBIvalue, nVoltageDiffValue, nSlopIdReplace, nSlopIdUnderWatch, nSlopId50, nSlopId, nIrRate, nReportingInterval, nNoOfHours As Integer
            Dim nMaxLBI, nMinLBI, nMinVoltageDiff, nMaxVoltageDiff, nNoSleepMode, nDays, nBatteryOriginalCapacity, nPagingCount, nLocationCount, nPreviousLBIvalue, nPreviousVoltageDiffValue As Integer
            Dim nPeriod, nRate, nRowIndx As Integer
            Dim LbiValue, VoltageDiffValue, Category_LBI, CssSlopeInfo, sBreakGraph As String
            Dim AvgWiFiLbi, Avg900Lbi As String
            Dim IsInitial As Boolean
            Dim dActivityLocation, dActivityPaging, dIR, dRF, dIAvg, dVoltage, nAvgLBIDiff As Double
            Dim dPreviousBatteryCapacity, dNPreviousBatteryCapacity As Double
            Dim bRegistoryBatteryReplace As Boolean = False
            Dim bRegistoryBatteryOnce As Boolean = False
            Dim nRegBatRep As Integer = 0
            Dim PlotLine As Boolean = False
            Dim storevalue As String = ""
            Dim storetempvalue As String = ""
            Dim nLong As Long
            Dim sBreakPointVal As String = ""
            Dim storebatteryvalue As String = ""

            LbiValue = ""
            Category_LBI = ""
            CssSlopeInfo = ""
            VoltageDiffValue = ""
            AvgWiFiLbi = ""
            Avg900Lbi = ""

            Dim Date_Category As String = ""

            Dim nSiteId As Integer = Val(Request.QueryString("qSiteId"))
            Dim nTagId As Integer = Val(Request.QueryString("qTagId"))

            Dim IsTempTag As Boolean = False
            Dim IsMMTag As Boolean = False

            Dim PredictionOn, CatastrophicCases, BatteryFailureMsg As String
            Dim SiteName As String = ""
            Dim sFileName As String = ""

            Dim isLBIDiff As Boolean = False

            Dim BatteryCapacity As Double = 0
            Dim nMinBatteryCapacity As Double = 0
            Dim nMaxBatteryCapacity As Double = 0

            Try

                sStrXml = GMSAPI.GetLBIBatteryCalc(nSiteId, 1, nTagId, g_UserAPI)
                ds = XMLNodeToDataSet(sStrXml)

                If ds.Tables.Count > 0 Then
                    dtTagList = ds.Tables(0)

                    If dtTagList.Rows.Count > 0 Then
                        With dtTagList.Rows(0)
                            SiteName = CheckIsDBNull(.Item("SiteName"), , "")
                            IsTempTag = .Item("IsTempTag")
                            IsMMTag = .Item("IsMMTag")
                            PredictionOn = .Item("PredictionOn")
                            CatastrophicCases = .Item("CatastrophicCases")
                            BatteryFailureMsg = .Item("BatteryFailureMsg")
                        End With
                    End If

                    tdSiteName.InnerHtml = SiteName
                    spnTagId.InnerHtml = nTagId & " - Tag LBI Activity"

                    If ds.Tables.Count > 1 Then
                        dt = ds.Tables(1)
                    End If

                End If

                dt.Columns.Add(New DataColumn("RowNo", Type.[GetType]("System.Int32")))

                If bExportExcel Then

                    Dim Csvtext As StringBuilder = New StringBuilder
                    Dim context As HttpContext

                    sFileName = "Tag_LBI_Report" & "_" & nSiteId & "_" & nTagId & "_" & Format(Now, "MMddyyyy")

                    context = HttpContext.Current
                    InitiateCSV(context, sFileName)

                    AddCSVCell(context, "Report: Tag LBI Activity Report", True)
                    AddCSVNewLine(context)

                    AddCSVCell(context, "Site Name: " & SiteName, True)
                    AddCSVNewLine(context)

                    AddCSVCell(context, "Device Id: " & nTagId, True)
                    AddCSVNewLine(context)
                    AddCSVNewLine(context)

                    AddCSVCell(Context, "#", True)
                    AddCSVCell(Context, "Date", True)
                    AddCSVCell(context, "LBI", True)

                    If IsMMTag Then
                        AddCSVCell(context, "LBI Diff", True)
                    End If

                    AddCSVCell(context, "Paging Count", True)
                    AddCSVCell(context, "Location Count", True)
                    AddCSVCell(context, "Activity Level Paging (RF)", True)
                    AddCSVCell(context, "Activity Level Location (IR)", True)
                    AddCSVCell(context, "IR", True)
                    AddCSVCell(context, "RF", True)
                    AddCSVCell(context, "I_Avg", True)
                    AddCSVCell(context, "I_Missing", True)
                    AddCSVCell(context, "Battery Capacity %", True)
                    AddCSVCell(context, "Battery Status", True)
                    AddCSVNewLine(context)
                Else

                    tblRow = New HtmlTableRow
                    AddCell(tblRow, "#")
                    AddCell(tblRow, "Date")
                    AddCell(tblRow, "LBI")

                    If IsMMTag Then
                        AddCell(tblRow, "LBI Diff")
                    End If

                    AddCell(tblRow, "Paging Count")
                    AddCell(tblRow, "Location Count")
                    AddCell(tblRow, "Activity Level Paging (RF)")
                    AddCell(tblRow, "Activity Level Location (IR)")
                    AddCell(tblRow, "IR")
                    AddCell(tblRow, "RF")
                    AddCell(tblRow, "I_Avg")
                    AddCell(tblRow, "I_Missing")
                    AddCell(tblRow, "Battery Capacity %")
                    AddCell(tblRow, "Battery Status")

                    tblRow.Attributes("class") = "clstblTagHeader"
                    tblLogViewerInfo.Rows.Add(tblRow)
                End If

                If IsMMTag Then
                    ChkShowFilter.Visible = True
                Else
                    ChkShowFilter.Visible = False
                End If

                If IsChkLbiFilter Then
                    dt.DefaultView.RowFilter = "IsNull(LbiDiff,0)>'0'"
                    dt = dt.DefaultView.ToTable()
                End If

                IsInitial = True
                IsDrawnSlopeVertical = False
                nSlopIdReplace = 0
                nSlopIdUnderWatch = 0
                nSlopId50 = 0
                nSlopId = 0
                nMaxLBI = 0
                nMinLBI = 0
                nMinVoltageDiff = 0
                nMaxVoltageDiff = 0
                dPreviousBatteryCapacity = 0
                nDays = 0

                nPagingCount = 0
                nLocationCount = 0
                nNoOfHours = 0

                'Slope posstion finding
                Dim bSlop580 As Boolean = True
                Dim bSlop700 As Boolean = True
                Dim bSlopDiff50 As Boolean = True

                sBreakPointVal = ""

                If Not bExportExcel Then
                    For nRow = 0 To dt.Rows.Count - 1

                        dt.Rows(nRow).Item("RowNo") = nRow + 1

                        nLBIvalue = dt.Rows(nRow).Item("LBI")
                        nVoltageDiffValue = dt.Rows(nRow).Item("LbiDiff")
                        BatteryCapacity = dt.Rows(nRow).Item("BatteryCapacity")

                        If nRow = 0 Then
                            nPreviousLBIvalue = nLBIvalue
                            nPreviousVoltageDiffValue = nVoltageDiffValue
                        Else
                            nPreviousLBIvalue = dt.Rows(nRow - 1).Item("LBI")
                            nPreviousVoltageDiffValue = dt.Rows(nRow).Item("LbiDiff")
                        End If

                        If nRow = 0 Then
                            nMinLBI = nLBIvalue
                            nMaxLBI = nLBIvalue

                            nMinVoltageDiff = nVoltageDiffValue
                            nMaxVoltageDiff = nVoltageDiffValue

                            nMinBatteryCapacity = BatteryCapacity
                            nMaxBatteryCapacity = BatteryCapacity
                        End If

                        If nLBIvalue < nMinLBI Then
                            nMinLBI = nLBIvalue
                        End If

                        If nLBIvalue > nMaxLBI Then
                            nMaxLBI = nLBIvalue
                        End If

                        If BatteryCapacity < nMinBatteryCapacity Then
                            nMinBatteryCapacity = BatteryCapacity
                        End If

                        If BatteryCapacity > nMaxBatteryCapacity Then
                            nMaxBatteryCapacity = BatteryCapacity
                        End If

                        If nVoltageDiffValue < nMinVoltageDiff Then
                            nMinVoltageDiff = nVoltageDiffValue
                        End If

                        If nVoltageDiffValue > nMaxVoltageDiff Then
                            nMaxVoltageDiff = nVoltageDiffValue
                        End If

                        If nSlopId = 0 Then
                            If dt.Rows(nRow).Item("CatastrophicCases") = 1 Or dt.Rows(nRow).Item("CatastrophicCases") = 2 Then
                                sBreakPointVal = "Replace battery immediately"
                                nSlopId = dt.Rows(nRow).Item("DataId")
                            ElseIf dt.Rows(nRow).Item("CatastrophicCases") = 4 Then
                                sBreakPointVal = "Underwatch"
                                nSlopId = dt.Rows(nRow).Item("DataId")
                            End If
                        Else
                            If dt.Rows(nRow).Item("CatastrophicCases") = 0 Then
                                nSlopId = 0
                            ElseIf nRow > 0 Then
                                If dt.Rows(nRow - 1).Item("CatastrophicCases") <> dt.Rows(nRow).Item("CatastrophicCases") Then
                                    If dt.Rows(nRow).Item("CatastrophicCases") = 1 Or dt.Rows(nRow).Item("CatastrophicCases") = 2 Then
                                        sBreakPointVal = "Replace battery immediately"
                                        nSlopId = dt.Rows(nRow).Item("DataId")
                                    ElseIf dt.Rows(nRow).Item("CatastrophicCases") = 4 Then
                                        sBreakPointVal = "Underwatch"
                                        nSlopId = dt.Rows(nRow).Item("DataId")
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If

                sBreakGraph = ""

                'Dim dtProfileData As DataTable
                'Dim DAL_Shipping As New DAL_Shipping
                'Dim nProfile As Integer = 0
                'Dim nIrProfile As Integer = 1
                'dtProfileData = DAL_Shipping.GetDeviceProfileData(nSiteId, nSiteId, nTagId, enumDeviceType.Tag)
                'If Not dtProfileData Is Nothing Then
                '    If dtProfileData.Rows.Count > 0 Then
                '        With dtProfileData.Rows(0)
                '            nProfile = CheckIsDBNull(.Item("Profile"), False, 0)
                '            nIrProfile = CheckIsDBNull(.Item("IRProfile"), False, 1)
                '        End With
                '    End If
                'End If
                'nReportingInterval = 12000
                'nBatteryOriginalCapacity = 290
                'nNoSleepMode = nProfile
                'nPeriod = 1
                'nRate = 12
                For nRow = 0 To dt.Rows.Count - 1
                    With dt.Rows(nRow)

                        tblRow = New HtmlTableRow
                        nRowIndx = nRow + 1

                        nLBIvalue = .Item("LBI")
                        nVoltageDiffValue = .Item("LBIDiff")
                        nAvgLBIDiff = .Item("AvgLBIDiff")
                        dVoltage = dt.Rows(nRow).Item("LbiValue")
                        nPagingCount = .Item("PagingCount")
                        nLocationCount = .Item("LocationCount")
                        nNoOfHours = .Item("NoOfHours")

                        dActivityPaging = .Item("Activitylevel_Paging")
                        dActivityLocation = .Item("ActivityLevel_Location")

                        If bExportExcel Then

                            AddCSVCell(Context, nRowIndx, True)
                            AddCSVCell(Context, .Item("ReceivedTime"), True)
                            AddCSVCell(Context, nLBIvalue & "[" & dVoltage & "]", True)

                            If IsMMTag Then
                                AddCSVCell(Context, nVoltageDiffValue, True)
                            End If

                            AddCSVCell(Context, nPagingCount, True)
                            AddCSVCell(Context, nLocationCount, True)
                            AddCSVCell(Context, dActivityPaging, True)
                            AddCSVCell(Context, dActivityLocation, True)
                            AddCSVCell(Context, .Item("IR"), True)
                            AddCSVCell(Context, .Item("RF"), True)
                            AddCSVCell(Context, .Item("I_avg"), True)
                            AddCSVCell(Context, .Item("I_avg_NotActive"), True)
                            AddCSVCell(Context, .Item("BatteryCapacity"), True)
                            If .Item("CatastrophicCases") = 0 Then
                                AddCSVCell(Context, "Good", True)
                            ElseIf .Item("CatastrophicCases") = 1 Or .Item("CatastrophicCases") = 2 Then
                                AddCSVCell(Context, "Replace battery immediately", True)
                            ElseIf .Item("CatastrophicCases") = 4 Then
                                AddCSVCell(Context, "Under Watch", True)
                            End If

                            AddCSVNewLine(Context)
                        Else

                            AddCell(tblRow, nRowIndx)
                            AddCell(tblRow, .Item("ReceivedTime"), "right")
                            AddCell(tblRow, nLBIvalue & "[" & dVoltage & "]", "right")

                            If IsMMTag Then
                                AddCell(tblRow, nVoltageDiffValue, "right")
                            End If

                            AddCell(tblRow, nPagingCount, "right")
                            AddCell(tblRow, nLocationCount, "right")
                            AddCell(tblRow, dActivityPaging, "right")
                            AddCell(tblRow, dActivityLocation, "right")
                            AddCell(tblRow, .Item("IR"), "right")
                            AddCell(tblRow, .Item("RF"), "right")
                            AddCell(tblRow, .Item("I_avg"), "right")
                            AddCell(tblRow, .Item("I_avg_NotActive"), "right")
                            AddCell(tblRow, .Item("BatteryCapacity"), "right")

                            If .Item("CatastrophicCases") = 0 Then
                                AddCell(tblRow, "Good", "right")
                            ElseIf .Item("CatastrophicCases") = 1 Or .Item("CatastrophicCases") = 2 Then
                                AddCell(tblRow, "Replace battery immediately", "right")
                            ElseIf .Item("CatastrophicCases") = 4 Then
                                AddCell(tblRow, "Under Watch", "right")
                            End If
                        End If

                        If Not bExportExcel Then
                            If nSlopId = .Item("dataId") Then
                                CssSlopeInfo = "SlopeInfo"
                                If FlashEnable Then
                                    sBreakGraph &= "<line startindex='" & nRow + 2 & "' displayalways='1' displayvalue='" & sBreakPointVal & "' valueontop='1' color='ED320D' thickness='4' />"
                                Else
                                    PlotLine = True
                                End If
                            Else
                                'drow = Nothing
                                'drow = dtTagReplaceBatteryHistory.Select("Updatedon='" & Format(CDate(.Item("ReceivedTime")), "MM/dd/yyyy") & "'")

                                'If drow.Length > 0 Then
                                '    CssSlopeInfo = "BatteryReplace"
                                'Else
                                '    CssSlopeInfo = IIf(nRow Mod 2 = 0, "clstblRowEven", "clstblRowOdd")
                                'End If
                            End If

                            If FlashEnable Then
                                If IsMMTag Then
                                    If PredictionOn <> "" And CatastrophicCases > 0 Then

                                        If nSlopId = dt.Rows(nRow).Item("DataId") Then
                                            CssSlopeInfo = "SlopeInfo"

                                            If CatastrophicCases = 1 Then
                                                sBreakGraph &= "<line startindex='" & nRow + 2 & "' displayalways='1' displayvalue='" & BatteryFailureMsg & "' valueontop='1' color='ED320D' thickness='4' />"
                                            Else
                                                sBreakGraph &= "<line startindex='" & nRow + 2 & "' displayalways='1' displayvalue='" & BatteryFailureMsg & "' valueontop='1' color='ED320D' thickness='4' />"
                                            End If
                                        End If

                                    End If
                                End If
                            End If

                            If CssSlopeInfo = "" Then
                                CssSlopeInfo = IIf(nRow Mod 2 = 0, "CssEven", "")
                            End If

                            CssSlopeInfo = IIf(nRow Mod 2 = 0, "clsTagRowEven " & CssSlopeInfo, "clsTagRowOdd " & CssSlopeInfo)

                            tblRow.Attributes("class") = CssSlopeInfo
                            tblLogViewerInfo.Rows.Add(tblRow)
                            CssSlopeInfo = ""

                            If FlashEnable Then
                                LbiValue &= nLBIvalue & "|"
                                If IsMMTag Then VoltageDiffValue &= nVoltageDiffValue & "|"
                                Category_LBI &= .Item("ReceivedTime") & "|"
                            Else

                                LbiValue = nLBIvalue

                                If IsMMTag Then VoltageDiffValue = nVoltageDiffValue

                                Category_LBI = .Item("ReceivedTime")
                                'Identify Break Point
                                If PlotLine And sBreakGraph = "" Then
                                    sBreakGraph = DateDiff(DateInterval.Second, CDate("01/01/1970"), CDate(Category_LBI))
                                    sBreakGraph &= "000"
                                    PlotLine = False
                                End If
                                If Category_LBI <> "" Then
                                    nLong = DateDiff(DateInterval.Second, CDate("01/01/1970"), CDate(Category_LBI))

                                    If IsMMTag Then
                                        storevalue &= "[" & nLong & "000," & VoltageDiffValue & "],"
                                        storetempvalue &= "[" & nLong & "000," & LbiValue & "],"

                                        If VoltageDiffValue > 0 Then
                                            isLBIDiff = True
                                        End If

                                    Else
                                        storevalue &= "[" & nLong & "000," & LbiValue & "],"
                                    End If

                                    storebatteryvalue &= "[" & nLong & "000," & .Item("BatteryCapacity") & "],"

                                End If
                            End If
                        End If

                    End With
                Next

                If bExportExcel Then

                    HttpContext.Current.Response.BufferOutput = True
                    HttpContext.Current.Response.Flush()
                    HttpContext.Current.Response.[End]()
                Else

                    If IsMMTag Then
                        If isLBIDiff = False Then
                            storevalue = storetempvalue
                            VoltageDiffValue = ""
                        End If
                    End If

                    If FlashEnable Then
                        If sBreakGraph <> "" Then
                            sBreakGraph = "<vtrendlines>" & sBreakGraph & "</vtrendlines>"
                        End If

                        ' LBI Chart
                        SetLBIandAVGLBIChart(nMaxLBI + 10, nMinLBI - 10, LbiValue, sBreakGraph, Category_LBI, VoltageDiffValue, AvgWiFiLbi, Avg900Lbi)

                        ' Battery Chart
                        SetBatteryChart(nMaxBatteryCapacity + 10, nMinBatteryCapacity - 10, storebatteryvalue, Category_LBI)
                    Else

                        ' LBI Chart
                        SetLBIandAVGLBIChart(nMaxLBI + 10, nMinLBI - 10, storevalue, sBreakGraph, Category_LBI, VoltageDiffValue)

                        ' Battery Chart
                        SetBatteryChart(nMaxBatteryCapacity + 10, nMinBatteryCapacity - 10, storebatteryvalue, Category_LBI)
                    End If

                End If

                'Location and Paging Bar Chart
                SetBarChart(dt)

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadCSVReport " & ex.Message.ToString())
            End Try
        End Sub

        Public Sub SetLBIandAVGLBIChart(ByVal LBIMax As Double, ByVal LbIMin As Double, ByVal DataStoreValue As String, ByVal sBreakGraphPlot As String, Optional ByVal Category_LBI As String = "",
                                        Optional ByVal LBIDiffVal As String = "", Optional ByVal AvgWiFiLbi As String = "", Optional ByVal Avg900Lbi As String = "")

            Dim TdLineGraphValue As String
            Dim SeriesData As String = ""
            Dim GridMinorAlternateColor As String = "#A8A6BC,#F7F7FB,#A6ADD2"

            Try
                If FlashEnable Then
                    trFusionChart.Visible = True
                    sXMLLBIDiff = "<chart YAxisMaxValue ='" & (LBIMax + 100) & "' yAxisMinValue='" & (LbIMin - 100) & "' compactDataMode='1' dataSeparator='|' paletteThemeColor='5D57A5' divLineColor='5D57A5' divLineAlpha='70' vDivLineAlpha='70' allowPinMode='1' pixelsPerPoint='1'>"
                    sXMLLBIDiff &= "<categories>"

                    If Len(Trim(Category_LBI)) > 0 Then sXMLLBIDiff &= "|" & Category_LBI
                    sXMLLBIDiff &= "</categories>"

                    sXMLLBIDiff &= "<dataset seriesName='LBI Value'>"
                    If Len(Trim(DataStoreValue)) > 0 Then sXMLLBIDiff &= "|" & DataStoreValue
                    sXMLLBIDiff &= "</dataset>"
                    If Len(LBIDiffVal) > 0 Then
                        sXMLLBIDiff &= "<dataset seriesName='LBI Diff Value'>"
                        If Len(Trim(DataStoreValue)) > 0 Then sXMLLBIDiff &= "|" & LBIDiffVal
                        sXMLLBIDiff &= "</dataset>"
                    End If
                    sXMLLBIDiff &= sBreakGraphPlot
                    sXMLLBIDiff &= "</chart>"
                Else
                    TRHighchart.Visible = True
                    If DataStoreValue <> "" Then
                        DataStoreValue = DataStoreValue.Substring(0, DataStoreValue.Length - 1)

                        If Len(LBIDiffVal) > 0 Then
                            SeriesData = "{name:'LBI Diff Value',color: '#99CCFF',marker: {radius: 3},lineWidth: 1.5,data:[" & DataStoreValue & "]}"
                        Else
                            SeriesData = "{name:'LBI Value',color: '#99CCFF',marker: {radius: 3},lineWidth: 1.5,data:[" & DataStoreValue & "]}"
                        End If

                        TdLineGraphValue = Highchart_ZoomedLineGraph("ctl00_ContentPlaceHolder1_LBIValueZoom", sBreakGraphPlot, SeriesData, GridMinorAlternateColor)
                    Else
                        TdLineGraphValue = Empty_HighChart("ctl00_ContentPlaceHolder1_LBIValueZoom")
                    End If
                    LBIValueZoom.InnerHtml = TdLineGraphValue
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub SetBatteryChart(ByVal BatteryMax As Double, ByVal BatteryMin As Double, ByVal DataStoreValue As String, Optional ByVal Category_Battery As String = "")

            Dim TdLineGraphValue As String
            Dim SeriesData As String = ""
            Dim GridMinorAlternateColor As String = "#A8A6BC,#F7F7FB,#A6ADD2"

            Try

                If FlashEnable Then
                    trBatteryFusionChart.Visible = True
                    sXMLBatteryDiff = "<chart YAxisMaxValue ='" & (BatteryMax + 100) & "' yAxisMinValue='" & (BatteryMin - 100) & "' compactDataMode='1' dataSeparator='|' paletteThemeColor='5D57A5' divLineColor='5D57A5' divLineAlpha='70' vDivLineAlpha='70' allowPinMode='1' pixelsPerPoint='1'>"
                    sXMLBatteryDiff &= "<categories>"

                    If Len(Trim(Category_Battery)) > 0 Then sXMLBatteryDiff &= "|" & Category_Battery
                    sXMLBatteryDiff &= "</categories>"

                    sXMLBatteryDiff &= "<dataset seriesName='Battery Capacity'>"
                    If Len(Trim(DataStoreValue)) > 0 Then sXMLBatteryDiff &= "|" & DataStoreValue
                    sXMLBatteryDiff &= "</dataset>"
                    sXMLBatteryDiff &= "</chart>"
                Else
                    TRBatteryHighchart.Visible = True
                    If DataStoreValue <> "" Then
                        DataStoreValue = DataStoreValue.Substring(0, DataStoreValue.Length - 1)

                        SeriesData = "{name:'Battery Capacity',color: '#99CCFF',marker: {radius: 3},lineWidth: 1.5,data:[" & DataStoreValue & "]}"

                        TdLineGraphValue = Highchart_ZoomedLineGraph("ctl00_ContentPlaceHolder1_BatteryValueZoom", "", SeriesData, GridMinorAlternateColor)
                    Else
                        TdLineGraphValue = Empty_HighChart("ctl00_ContentPlaceHolder1_BatteryValueZoom")
                    End If

                    BatteryValueZoom.InnerHtml = TdLineGraphValue

                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Function Highchart_ZoomedLineGraph(ByVal Div_ID As String, ByVal PlotlinesValues As String, ByVal seriesData As String, ByVal GridMinorAlterColor As String) As String
            Dim DrawString As String
            Dim SPlotLine As String = ""
            Dim ChartColor() As String
            ChartColor = GridMinorAlterColor.Split(",")
            If PlotlinesValues <> "" Then
                Val(PlotlinesValues)
                SPlotLine = "plotLines: [" & _
                                        "{" & _
                                            "color:  '#FF0000', " & _
                                            "width: 2, " & _
                                            "value: " & PlotlinesValues & "," & _
                                            "zIndex: 3, " & _
                                            "label : { " & _
                                            "text:   'Break Point' " & _
                                        "} " & _
                                    "}], "
            End If

            DrawString = "<script type='text/javascript'> " & _
                             "$(function () " & _
                                    "{" & _
                                        "var chart;$(document).ready(function() " & _
                                        "{" & _
                                            "chart = new Highcharts.Chart(" & _
                                            "{" & _
                                                "chart:{" & _
                                                            "borderWidth: 1," & _
                                                            "borderColor: 'black'," & _
                                                            "backgroundColor:'#EBEBF4'," & _
                                                            "renderTo:'" & Div_ID & "'," & _
                                                            "type:'line'," & _
                                                            "zoomType: 'x'," & _
                                                            "reflow:true" & _
                                                       "}," & _
                                                 "credits:{enabled:false}," & _
                                                 "title:{text:''}," & _
                                                 "xAxis:{" & _
                                                            "gridLineWidth:0.5,gridLineColor:'#336699',gridLineDashStyle: 'longdash'," & _
                                                            "type:'datetime', " & _
                                                            "maxZoom: 2000 * 5000 * 24 * 5," & _
                                                            "labels: {" & _
                                                                    "rotation: -90," & _
                                                                    "y:38," & _
                                                                    "style:{color: '#070707',font: '11px Verdana',fontWeight:'lighter'}," & _
                                                                "formatter: function() {" & _
                                                                    "return Highcharts.dateFormat('%m/%d/%Y', this.value);" & _
                                                                "}" & _
                                                            "}," & _
                                                            SPlotLine & _
                                                           "title:{" & _
                                                                       "text:''," & _
                                                                       "margin: 10," & _
                                                                       "style:{fontWeight: 'normal',color: 'Black',fontSize: '15px'}" & _
                                                                    "}" & _
                                                                "}," & _
                                                                   "scrollbar: {" & _
                                                                   "enabled:true" & _
                                                                   "}," & _
                                                        "yAxis:{" & _
                                                            "gridLineWidth:1,gridLineColor:'" & ChartColor(0) & "'," & _
                                                            "minorGridLineColor: '" & ChartColor(1) & "',minorGridLineWidth:0.0,minorTickLength: 0,minorTickInterval: 'auto',minorGridLineDashStyle: 'Solid'," & _
                                                            "alternateGridColor: '" & ChartColor(2) & "'," & _
                                                            "title:{" & _
                                                                    "text:''," & _
                                                                    "style:{" & _
                                                                            "fontWeight:'normal'," & _
                                                                            "color: 'Black'," & _
                                                                            "fontSize: '15px'" & _
                                                                            "}" & _
                                                                    "}" & _
                                                            "}," & _
                                                    "plotOptions:{" & _
                                                                    "series:{" & _
                                                                                "connectNulls: true," & _
                                                                                "cursor:'pointer'," & _
                                                                                "marker: {" & _
                                                                                            "states: {" & _
                                                                                                        "hover: { " & _
                                                                                                                    "fillColor:  'white', " & _
                                                                                                                    "lineColor:  'black', " & _
                                                                                                                    "lineWidth: 3" & _
                                                                                                                "}" & _
                                                                                                       "} " & _
                                                                                           "} " & _
                                                                             "}," & _
                                                                       "line: {" & _
                                                                             "dataLabels: " & _
                                                                                        "{" & _
                                                                                            "enabled: false, " & _
                                                                                            "style: {" & _
                                                                                                        "fontWeight :'bold'," & _
                                                                                                        "color: '#221A90'," & _
                                                                                                        "fontSize: '10px'" & _
                                                                                                    "}" & _
                                                                                         "}" & _
                                                                               "}" & _
                                                                   "}," & _
                                                       "series: [" & seriesData & "]" & _
                                            "});" & _
                                       "});" & _
                                    "});</script>"
            Return DrawString
        End Function

        Public Function Empty_HighChart(ByVal Div_Id As String) As String
            Dim EmptyData As String
            EmptyData = "<script type='text/javascript'> " & _
                                 "$(function () " & _
                                        "{" & _
                                            "var chart;$(document).ready(function() " & _
                                            "{" & _
                                                "chart = new Highcharts.Chart(" & _
                                                "{" & _
                                                    "chart:{" & _
                                                                "borderWidth: 1," & _
                                                                "borderColor: 'black'," & _
                                                                "backgroundColor:'white'," & _
                                                                "renderTo:'" & Div_Id & "'," & _
                                                                "width:1200," & _
                                                                "height:400," & _
                                                           "}," & _
                                                     "credits:{enabled:false}," & _
                                                     "title:" & _
                                                            "{" & _
                                                                "text:'No Data to Display', " & _
                                                                "align:'center', " & _
                                                                "x:10," & _
                                                                "y:150, " & _
                                                                "floating: true " & _
                                                             "}," & _
                                                     "xAxis:{" & _
                                                                "categories:[]" & _
                                                              "}," & _
                                                     "series:[]" & _
                                                  "});" & _
                                           "});" & _
                                        "});</script>"
            Return EmptyData
        End Function

        Sub SetBarChart(ByVal dt As DataTable)

            Dim SeriesData1, SeriesData2 As String
            Dim Category As String = ""
            Dim nLong As Long

            Dim dtSort As New DataTable

            Try

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then

                        dt.DefaultView.Sort = "RowNo DESC"
                        dt = dt.DefaultView.ToTable()

                        SeriesData1 = ""
                        SeriesData2 = ""

                        For nIdx As Integer = 0 To 29
                            With dt.Rows(nIdx)

                                nLong = DateDiff(DateInterval.Second, CDate("01/01/1970"), CDate(.Item("ReceivedTime")))

                                Category &= nLong & "000 ,"
                                SeriesData1 &= .Item("LocationCount") & ","
                                SeriesData2 &= .Item("PagingCount") & ","

                            End With
                        Next

                        Highchart_BarGraph("ctl00_ContentPlaceHolder1_divPagingLocationChart", Category, SeriesData1, SeriesData2)

                    End If
                End If

            Catch ex As Exception
                WriteLog(" SetBarChart " & ex.Message.ToString())
            End Try
        End Sub

        Public Sub Highchart_BarGraph(ByVal Div_ID As String, ByVal Category As String, ByVal SeriesData1 As String, ByVal SeriesData2 As String)

            Dim DrawString As String = ""

            DrawString = "<script type='text/javascript'> " & _
                            "$(function () { " & _
                                  "$('#" & Div_ID & "').highcharts({ " & _
                                        "chart: { " & _
                                            "borderWidth: 1," & _
                                            "borderColor: 'black'," & _
                                            "backgroundColor:'#EBEBF4'," & _
                                            "renderTo:'" & Div_ID & "'," & _
                                            "type:'column'," & _
                                        "}, " & _
                                        "title: { " & _
                                            "text: '', " & _
                                            "style: { " & _
                                              "color: '#fff' " & _
                                            "} " & _
                                        "}, " & _
                                        "xAxis: { " & _
                                            "tickWidth: 0, " & _
                                            "type:'datetime', " & _
                                            "labels: {" & _
                                                    "rotation: -90," & _
                                                    "y:38," & _
                                                    "style:{color: '#070707',font: '11px Verdana',fontWeight:'lighter'}," & _
                                                    "formatter: function() {" & _
                                                        "return Highcharts.dateFormat('%m/%d/%Y', this.value);" & _
                                                    "}" & _
                                            "}," & _
                                            "categories: [" & Category & "] " & _
                                        "}, " & _
                                        "yAxis: { " & _
                                            "gridLineWidth: 0.5, " & _
                                            "gridLineDashStyle: 'dash', " & _
                                            "gridLineColor: '336699', " & _
                                            "title: { " & _
                                                "text: '', " & _
                                                "style: { " & _
                                                  "color: '#333' " & _
                                                 "} " & _
                                            "}, " & _
                                            "labels: { " & _
                                              "formatter: function() { " & _
                                                        "return Highcharts.numberFormat(this.value, 0, '', ','); " & _
                                                    "}, " & _
                                              "style: { " & _
                                                  "color: '#333', " & _
                                                 "} " & _
                                              "} " & _
                                            "}, " & _
                                        "legend: { " & _
                                            "enabled: true, " & _
                                        "}, " & _
                                        "credits: { " & _
                                            "enabled: false " & _
                                        "}, " & _
                                        "plotOptions: { " & _
                                        "column: { " & _
                                         "borderRadius: 0, " & _
                                             "pointPadding: -0.12," & _
                                         "groupPadding: 0.1 " & _
                                            "} " & _
                                      "}, " & _
                                      "series: [" & _
                                         "{ " & _
                                            "name: 'Location Count', " & _
                                            "data: [" & SeriesData1 & "],color: '#99a5fa' " & _
                                         "}," & _
                                         "{ " & _
                                            "name: 'Paging Count', " & _
                                            "data: [" & SeriesData2 & "],color: '#80800b' " & _
                                        "}] " & _
                                    "}); " & _
                                "});</script>"

            divPagingLocationChart.InnerHtml = DrawString

        End Sub

        Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
            FillData(False, True)
        End Sub

        Sub CheckIsFlashPlayerInstalled()
            FlashEnable = False
            If FlasEnabled.Value = "Yes" Then FlashEnable = True
        End Sub

        Property FlashEnable() As Boolean
            Get
                Return Session("IsFlashPlayerInstalled")
            End Get
            Set(ByVal Value As Boolean)
                Session("IsFlashPlayerInstalled") = Value
            End Set
        End Property

        Protected Sub ChkShowFilter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkShowFilter.CheckedChanged

            If ChkShowFilter.Checked = True Then
                FillData(True)
            Else
                FillData()
            End If
        End Sub
        
    End Class

End Namespace
