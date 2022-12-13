Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class MonitorLBIActivityReport
        Inherits System.Web.UI.Page

        Public sXMLStackData, sXMLData, sXMLSlopeDiff, sXMLSlopDiff2, sXMLBattery As String

        Dim GMSAPI As New GMSAPI_New.GMSAPI_New

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()

            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "Monitor LBI Activity Report", enumPageAction.AccessViolation, "user try to access Monitor LBI Activity Report")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                DDListLBI.Items.Add(New ListItem("LBI", LBI.LBIValue))
                DDListLBI.Items.Add(New ListItem("Average LBI", LBI.AverageLBIValue))
                DDListLBI.SelectedValue = LBI.LBIValue   'default is average lbi

                CheckIsFlashPlayerInstalled()

                Dim qExport As Integer = Val(Request.QueryString("qExport"))

                If qExport = 1 Then
                    FillData(False, True)
                Else
                    FillData()
                End If

            End If

        End Sub

        Enum LBI
            LBIValue = 1
            AverageLBIValue = 2
        End Enum

        Sub FillData(Optional ByVal IsChkLbiFilter As Boolean = False, Optional ByVal bExportExcel As Boolean = False)

            Dim tblRow As HtmlTableRow
            Dim sStrXml As XmlNode

            Dim dtMonitorList As New DataTable
            Dim dt_ActiveMonitors As New DataTable

            Dim ds As New DataSet

            Dim nMinLBIYAxis, nMaxLBIYAxis, nCurrentLBIMax,  nBreakPointIdx As Integer
            Dim IsInitial, IsInitialSlope, IsDrawnSlopeVertical As Boolean
            Dim sDifferenceBreakPoint, Category, sLabelStep, Category_LBI, LBIValue, AVGLBIValue, CssSlopeInfo, CssColumnInfo As String
            Dim nLong As Long
            Dim YDataLbiValue, YDataAvgLbiValue, YDataBatteryValue, Category_BreakPointDate, YShorTimevalue, YLongTimeValue As String

            Dim MonitorType As Integer
            Dim nBatteryTypeId As Integer

            Dim SiteName As String = ""
            Dim sFileName As String = ""

            Dim nMinBatteryCapacityYAxis, nMaxBatteryCapacityYAxis, nCurrentBatteryCapacityMax As Double

            YDataLbiValue = ""
            YDataAvgLbiValue = ""
            YShorTimevalue = ""
            YLongTimeValue = ""
            Category_BreakPointDate = ""

            Dim nSiteId As Integer = Val(Request.QueryString("qSiteId"))
            Dim nMonitorId As Integer = Val(Request.QueryString("qMonitorId"))

            sStrXml = GMSAPI.GetLBIBatteryCalc(nSiteId, 2, nMonitorId, g_UserAPI)
            ds = XMLNodeToDataSet(sStrXml)

            If ds.Tables.Count > 0 Then

                dtMonitorList = ds.Tables(0)

                If dtMonitorList.Rows.Count > 0 Then
                    With dtMonitorList.Rows(0)
                        SiteName = CheckIsDBNull(.Item("SiteName"), , "")
                        MonitorType = CheckIsDBNull(.Item("DeviceSubTypeId"), , "")
                        nBatteryTypeId = CheckIsDBNull(.Item("BatteryTypeId"), , "")
                    End With
                End If

                tdSiteName.InnerHtml = SiteName
                spnTagId.InnerHtml = nMonitorId & " - Monitor LBI Activity"

                If ds.Tables.Count > 1 Then
                    dt_ActiveMonitors = ds.Tables(1)
                End If

            End If

            dt_ActiveMonitors.Columns.Add(New DataColumn("RowNo", Type.[GetType]("System.Int32")))

            If IsChkLbiFilter Then
                dt_ActiveMonitors.DefaultView.RowFilter = "IsNull(LbiValue,0)>0"
                dt_ActiveMonitors = dt_ActiveMonitors.DefaultView.ToTable()
            End If

            If bExportExcel Then

                Dim Csvtext As StringBuilder = New StringBuilder
                Dim context As HttpContext

                sFileName = "Monitor_LBI_Report" & "_" & nSiteId & "_" & nMonitorId & "_" & Format(Now, "MMddyyyy")

                context = HttpContext.Current
                InitiateCSV(context, sFileName)

                AddCSVCell(context, "Report: Monitor LBI Activity Report", True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Site Name: " & SiteName, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Device Id: " & nMonitorId, True)
                AddCSVNewLine(context)
                AddCSVNewLine(context)

                AddCSVCell(context, "Received Time", True)
                AddCSVCell(context, "Location Count", True)
                AddCSVCell(context, "Paging Count", True)
                AddCSVCell(context, "LBI Value", True)
                AddCSVCell(context, "Average LBI Value", True)

                If MonitorType = EnumMonitorFilterType.enum_EGRESS Or MonitorType = EnumMonitorFilterType.enum_MMMonitor Or MonitorType = EnumMonitorFilterType.enum_RegularMonitor Then
                    'mean lbi
                    AddCSVCell(Context, "mean lbi", True)
                    AddCSVCell(Context, "mean lbi diff", True)
                    AddCSVCell(Context, "daily deviation", True)
                    AddCSVCell(Context, "s today", True)
                    AddCSVCell(Context, "delta s", True)
                End If

                If MonitorType = EnumMonitorFilterType.enum_MMMonitor Then
                    AddCSVCell(Context, "Wifidatacount", True)
                End If

                AddCSVCell(context, "BatteryCapacity", True)

                If MonitorType = EnumMonitorFilterType.enum_Dim Then
                    AddCSVCell(Context, "Im", True)
                    AddCSVCell(Context, "Ia", True)
                End If

                'Alkaline Battery Capacity Calculation
                '---------------------------------------------------------------------------------------------------------------------------------------------------

                If nBatteryTypeId = EnumMonitorBatteryType.enum_Alkaline Then
                    AddCSVCell(context, "Projected_Lbi", True)
                    AddCSVCell(context, "Intercept", True)
                    AddCSVCell(context, "Slope LBI", True)
                End If
                '---------------------------------------------------------------------------------------------------------------------------------------------------

                AddCSVCell(context, "Battery Status", True)
                AddCSVNewLine(context)
            Else

                tblRow = New HtmlTableRow
                AddCell(tblRow, "#")
                AddCell(tblRow, "Received&nbsp;Time", , , "150px")
                AddCell(tblRow, "Location&nbsp;Count", , , "150px")
                AddCell(tblRow, "Paging&nbsp;Count", , , "150px")
                AddCell(tblRow, "LBI&nbsp;Value", , , "150px")
                AddCell(tblRow, "Average&nbsp;LBI&nbsp;Value", , , "150px")

                If MonitorType = EnumMonitorFilterType.enum_EGRESS Or MonitorType = EnumMonitorFilterType.enum_MMMonitor Or MonitorType = EnumMonitorFilterType.enum_RegularMonitor Then
                    'mean lbi
                    AddCell(tblRow, "mean lbi", , , "150px")
                    AddCell(tblRow, "mean lbi diff", , , "150px")
                    AddCell(tblRow, "daily deviation", , , "150px")
                    AddCell(tblRow, "s today", , , "150px")
                    AddCell(tblRow, "delta s", , , "150px")
                End If

                If MonitorType = EnumMonitorFilterType.enum_MMMonitor Then
                    AddCell(tblRow, "Wifidatacount", , , "150px")
                End If

                AddCell(tblRow, "batteryCapacity", , , "150px")

                If MonitorType = EnumMonitorFilterType.enum_Dim Then
                    AddCell(tblRow, "Im", , , "150px")
                    AddCell(tblRow, "Ia", , , "150px")
                End If

                'Alkaline Battery Capacity Calculation
                '---------------------------------------------------------------------------------------------------------------------------------------------------

                If nBatteryTypeId = EnumMonitorBatteryType.enum_Alkaline Then
                    AddCell(tblRow, "Projected_Lbi", , , "150px")
                    AddCell(tblRow, "Intercept", , , "150px")
                    AddCell(tblRow, "Slope LBI", , , "150px")
                End If
                '---------------------------------------------------------------------------------------------------------------------------------------------------
                AddCell(tblRow, "Battery Status")
                tblRow.Attributes("class") = "clstblTagHeader"
                tblLogViewerInfo.Rows.Add(tblRow)
            End If

            LBIValue = ""
            AVGLBIValue = ""
            Category = ""
            sDifferenceBreakPoint = ""
            CssSlopeInfo = ""
            sLabelStep = ""
            Category_LBI = ""
            IsDrawnSlopeVertical = False
            CssColumnInfo = ""
            YDataBatteryValue = ""

            Dim nCount As Integer

            Try

                IsInitial = True
                IsInitialSlope = True

                For idx As Integer = 0 To dt_ActiveMonitors.Rows.Count - 1

                    dt_ActiveMonitors.Rows(idx).Item("RowNo") = idx + 1

                    Dim nDay As Integer = 0
                    Dim nLBIvalue As Double

                    With dt_ActiveMonitors.Rows(idx)

                        If DDListLBI.SelectedValue = LBI.LBIValue Then
                            nLBIvalue = .Item("LbiValue")
                        Else
                            nLBIvalue = .Item("AvgLbiValue")
                        End If

                        If idx = 2540 Then
                            nLBIvalue = .Item("AvgLbiValue")
                        End If

                        nDay = idx + 1
                        tblRow = New HtmlTableRow
                        nCount = idx

                        If FlashEnable Then
                            LBIValue &= nLBIvalue & "|"
                            AVGLBIValue &= FormatNumber(.Item("AvgLbiValue"), 2) & "|"
                            Category_LBI &= Format(CDate(.Item("ReceivedTime")), "MM/dd/yyyy") & "|"
                        Else
                            LBIValue = nLBIvalue
                            AVGLBIValue = FormatNumber(.Item("AvgLbiValue"), 2)
                            Category_LBI = Format(CDate(.Item("ReceivedTime")), "MM/dd/yyyy")

                            nLong = DateDiff(DateInterval.Second, CDate("01/01/1970"), CDate(Category_LBI))
                            YDataLbiValue &= "[" & nLong & "000," & LBIValue & "],"
                            YDataAvgLbiValue &= "[" & nLong & "000," & AVGLBIValue & "],"
                            YDataBatteryValue &= "[" & nLong & "000," & .Item("BatteryCapacity") & "],"
                        End If

                        If DDListLBI.SelectedValue = LBI.LBIValue Then
                            nCurrentLBIMax = FormatNumber(.Item("LbiValue"), 0)
                        Else
                            nCurrentLBIMax = FormatNumber(.Item("AvgLbiValue"), 2)
                        End If

                        nCurrentBatteryCapacityMax = FormatNumber(.Item("BatteryCapacity"), 2)

                        If IsInitial Then
                            nMinLBIYAxis = nCurrentLBIMax
                            nMaxLBIYAxis = nCurrentLBIMax

                            nMinBatteryCapacityYAxis = nCurrentBatteryCapacityMax
                            nMaxBatteryCapacityYAxis = nCurrentBatteryCapacityMax
                            IsInitial = False
                        End If

                        ' LBI
                        If nMinLBIYAxis > nCurrentLBIMax Then nMinLBIYAxis = nCurrentLBIMax
                        If nMaxLBIYAxis < nCurrentLBIMax Then nMaxLBIYAxis = nCurrentLBIMax

                        'Battery Capacity
                        If nMinBatteryCapacityYAxis > nCurrentBatteryCapacityMax Then nMinBatteryCapacityYAxis = nCurrentBatteryCapacityMax
                        If nMaxBatteryCapacityYAxis < nCurrentBatteryCapacityMax Then nMaxBatteryCapacityYAxis = nCurrentBatteryCapacityMax

                        If FlashEnable Then
                            If nBatteryTypeId = EnumMonitorBatteryType.enum_Alkaline Then
                                If .Item("IsBreakPoint") = False Then
                                    sDifferenceBreakPoint = 0
                                    IsDrawnSlopeVertical = False
                                Else
                                    If .Item("IsBreakPoint") = True And IsDrawnSlopeVertical = False Then
                                        CssSlopeInfo = "SlopeInfo"
                                        IsDrawnSlopeVertical = True
                                        nBreakPointIdx = idx + 1
                                    End If
                                End If
                            Else
                                If .Item("IsLBIBreakPoint") = False Then
                                    sDifferenceBreakPoint = 0
                                    IsDrawnSlopeVertical = False
                                Else
                                    If .Item("IsLBIBreakPoint") = True And IsDrawnSlopeVertical = False And .Item("CatastrophicCases") <> 0 Then
                                        CssSlopeInfo = "SlopeInfo"
                                        IsDrawnSlopeVertical = True
                                        nBreakPointIdx = idx + 1
                                    End If
                                End If
                            End If
                        Else
                            If nBatteryTypeId = EnumMonitorBatteryType.enum_Alkaline Then
                                If .Item("IsBreakPoint") = False Then
                                    sDifferenceBreakPoint = 0
                                Else
                                    If .Item("IsLBIBreakPoint") = True And IsDrawnSlopeVertical = False Then
                                        CssSlopeInfo = "SlopeInfo"
                                        IsDrawnSlopeVertical = True
                                    Else
                                        sDifferenceBreakPoint = 0
                                    End If
                                End If
                            Else
                                If .Item("IsLBIBreakPoint") = False Then
                                    sDifferenceBreakPoint = 0
                                Else
                                    If .Item("IsLBIBreakPoint") = True And IsDrawnSlopeVertical = False And .Item("CatastrophicCases") <> 0 Then
                                        CssSlopeInfo = "SlopeInfo"
                                        IsDrawnSlopeVertical = True
                                    Else
                                        sDifferenceBreakPoint = 0
                                    End If
                                End If
                            End If
                        End If

                        CssColumnInfo = "CssShort"

                        If Len(CssSlopeInfo) > 0 Then
                            CssColumnInfo = CssSlopeInfo
                        ElseIf .Item("CatastrophicCases") <> 0 Then
                            CssSlopeInfo = "CssLBI"
                        End If

                        If bExportExcel Then

                            AddCSVCell(Context, Format(CDate(.Item("ReceivedTime"))), True)
                            AddCSVCell(Context, .Item("LocationCount"), True)
                            AddCSVCell(Context, .Item("PagingCount"), True)
                            AddCSVCell(Context, FormatNumber(.Item("LbiValue"), 0), True)
                            AddCSVCell(Context, FormatNumber(.Item("AvgLbiValue"), 2), True)

                            If MonitorType = EnumMonitorFilterType.enum_EGRESS Or MonitorType = EnumMonitorFilterType.enum_MMMonitor Or MonitorType = EnumMonitorFilterType.enum_RegularMonitor Then
                                AddCSVCell(Context, FormatNumber(.Item("mean_lbi"), 4), True)
                                AddCSVCell(Context, FormatNumber(.Item("mean_lbi_diff"), 4), True)
                                AddCSVCell(Context, FormatNumber(.Item("daily_deviation"), 4), True)
                                AddCSVCell(Context, FormatNumber(.Item("s_today"), 4), True)
                                AddCSVCell(Context, FormatNumber(.Item("delta_s"), 4), True)
                            End If

                            If MonitorType = EnumMonitorFilterType.enum_MMMonitor Then
                                AddCSVCell(Context, CheckIsDBNull(.Item("Wifidatacount")), True)
                            End If

                            AddCSVCell(Context, CheckIsDBNull(.Item("batteryCapacity")), True)

                            If MonitorType = EnumMonitorFilterType.enum_Dim Then
                                AddCSVCell(Context, CheckIsDBNull(.Item("Im"), , "--"), True)
                                AddCSVCell(Context, CheckIsDBNull(.Item("Ia"), , "--"), True)
                            End If

                            If nBatteryTypeId = EnumMonitorBatteryType.enum_Alkaline Then
                                AddCSVCell(Context, CheckIsDBNull(.Item("projected_lbi"), , "--"), True)
                                AddCSVCell(Context, CheckIsDBNull(.Item("intercept"), , "--"), True)
                                AddCSVCell(Context, CheckIsDBNull(.Item("slope_lbi"), , "--"), True)
                            End If

                            'If .Item("CatastrophicCases") = 0 Then
                            '    AddCSVCell(Context, "Good", True)
                            'ElseIf .Item("CatastrophicCases") = 1 Then
                            '    AddCSVCell(Context, "Replace battery immediately", True)
                            'ElseIf .Item("CatastrophicCases") = 2 Then
                            '    AddCSVCell(Context, "Under Watch", True)
                            'End If

                            If .Item("CatastrophicCases") = 0 Then
                                AddCSVCell(Context, "Good", True)
                            Else
                                AddCSVCell(Context, CheckIsDBNull(.Item("BatteryStatus"), , ""), True)
                            End If

                            AddCSVNewLine(Context)
                        Else

                            AddCell(tblRow, nCount + 1)
                            AddCell(tblRow, Format(CDate(.Item("ReceivedTime")), "MM/dd/yyyy"), , , "150px")
                            AddCell(tblRow, .Item("LocationCount"), "right", , "150px")
                            AddCell(tblRow, .Item("PagingCount"), "right", , "150px")
                            AddCell(tblRow, FormatNumber(.Item("LbiValue"), 0), "right", , "150px")
                            AddCell(tblRow, FormatNumber(.Item("AvgLbiValue"), 2), "right", , "150px")

                            If MonitorType = EnumMonitorFilterType.enum_EGRESS Or MonitorType = EnumMonitorFilterType.enum_MMMonitor Or MonitorType = EnumMonitorFilterType.enum_RegularMonitor Then
                                AddCell(tblRow, FormatNumber(.Item("mean_lbi"), 4), "right", , "150px")
                                AddCell(tblRow, FormatNumber(.Item("mean_lbi_diff"), 4), "right", , "150px")
                                AddCell(tblRow, FormatNumber(.Item("daily_deviation"), 4), "right", , "150px")
                                AddCell(tblRow, FormatNumber(.Item("s_today"), 4), "right", , "150px")
                                AddCell(tblRow, FormatNumber(.Item("delta_s"), 4), "right", , "150px")
                            End If

                            If MonitorType = EnumMonitorFilterType.enum_MMMonitor Then
                                AddCell(tblRow, CheckIsDBNull(.Item("Wifidatacount"), , "--"), , , "150px")
                            End If

                            AddCell(tblRow, CheckIsDBNull(.Item("batteryCapacity"), , "--"), , , "150px")

                            If MonitorType = EnumMonitorFilterType.enum_Dim Then
                                AddCell(tblRow, CheckIsDBNull(.Item("Im"), , "--"), , , "150px")
                                AddCell(tblRow, CheckIsDBNull(.Item("Ia"), , "--"), , , "150px")
                            End If

                            If nBatteryTypeId = EnumMonitorBatteryType.enum_Alkaline Then
                                AddCell(tblRow, CheckIsDBNull(.Item("projected_lbi"), , "--"), "center", , "150px")
                                AddCell(tblRow, CheckIsDBNull(.Item("intercept"), , "--"), "center", , "150px")
                                AddCell(tblRow, CheckIsDBNull(.Item("slope_lbi"), , "--"), "center", , "150px")
                            End If

                            'If .Item("CatastrophicCases") = 0 Then
                            '    AddCell(tblRow, "Good", "right")
                            'ElseIf .Item("CatastrophicCases") = 1 Then
                            '    AddCell(tblRow, "Replace battery immediately", "right")
                            'ElseIf .Item("CatastrophicCases") = 2 Then
                            '    AddCell(tblRow, "Under Watch", "right")
                            'End If

                            If .Item("CatastrophicCases") = 0 Then
                                AddCell(tblRow, "Good")
                            Else
                                AddCell(tblRow, CheckIsDBNull(.Item("BatteryStatus"), , ""))
                            End If

                            If CssSlopeInfo = "" Then
                                CssSlopeInfo = IIf(idx Mod 2 = 0, "CssEven", "")
                            End If

                            CssSlopeInfo = IIf(idx Mod 2 = 0, "clsTagRowEven " & CssSlopeInfo, "clsTagRowOdd " & CssSlopeInfo)

                            tblRow.Attributes("class") = CssSlopeInfo
                            tblLogViewerInfo.Rows.Add(tblRow)
                            CssSlopeInfo = ""

                        End If

                    End With
                Next

            Catch ex As Exception
                WriteLog(ex.Message)
            End Try

            If bExportExcel Then
                HttpContext.Current.Response.BufferOutput = True
                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.[End]()
            Else
                If FlashEnable Then

                    'LBI Chart
                    SetLBIandAVGLBIChart(nMaxLBIYAxis, nMinLBIYAxis, Category_LBI, LBIValue, AVGLBIValue, nBreakPointIdx)

                    'Battery Chart
                    SetBatteryCapacityChart(nMaxBatteryCapacityYAxis, nMinBatteryCapacityYAxis, Category_LBI, YDataBatteryValue)
                Else

                    'LBI Chart
                    SetLBIandAVGLBIChart(nMaxLBIYAxis, nMinLBIYAxis, Category_LBI, YDataLbiValue, YDataAvgLbiValue, nBreakPointIdx)

                    'Battery Chart
                    SetBatteryCapacityChart(nMaxBatteryCapacityYAxis, nMinBatteryCapacityYAxis, Category_LBI, YDataBatteryValue)
                End If
            End If

            'Location and Paging Bar Chart
            SetBarChart(dt_ActiveMonitors)

        End Sub

        Protected Sub DDListLBI_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDListLBI.SelectedIndexChanged
            FillData()
        End Sub

        Public Sub SetLBIandAVGLBIChart(ByVal LBIMax As Double, ByVal LbIMin As Double, ByVal Category_LBI As String, ByVal LBIValue As String, ByVal AVGLBIValue As String, ByVal nBreakPointIdx As Integer)

            Dim sCategory_LBI As String = ""
            Dim sAVGLBIValue As String = ""
            Dim sLBIValue As String = ""
            Dim Seriesname As String = ""
            Dim TdLineGraphValue As String
            Dim GridMinorAlternateColor As String = "#A8A6BC,#F7F7FB,#A6ADD2"

            Dim nTargetIdx As Integer

            Try
                If FlashEnable Then
                    TRFusionZoomongLBI.Visible = True
                    sXMLSlopeDiff = "<chart YAxisMaxValue ='" & (LBIMax + 100) & "' yAxisMinValue='" & (LbIMin - 100) & "' compactDataMode='1' dataSeparator='|' paletteThemeColor='5D57A5' divLineColor='5D57A5' divLineAlpha='70' vDivLineAlpha='70' allowPinMode='1' pixelsPerPoint='1'>"
                    sXMLSlopeDiff &= "<categories>"
                    If Category_LBI.Length > 0 Then
                        sCategory_LBI = Category_LBI.Substring(0, Category_LBI.Length - 1)
                    End If
                    sXMLSlopeDiff &= sCategory_LBI
                    sXMLSlopeDiff &= "</categories>"
                    If DDListLBI.SelectedValue = LBI.LBIValue Then
                        If LBIValue.Length > 0 Then
                            sLBIValue = LBIValue.Substring(0, LBIValue.Length - 1)
                        End If
                        sXMLSlopeDiff &= "<dataset seriesName='LBI Value'>" & sLBIValue & "</dataset>"
                    Else
                        If AVGLBIValue.Length > 0 Then
                            sAVGLBIValue = AVGLBIValue.Substring(0, AVGLBIValue.Length - 1)
                        End If
                        sXMLSlopeDiff &= "<dataset seriesName='Average LBI Value'>" & sAVGLBIValue & "</dataset>"
                    End If


                    If nBreakPointIdx >= 2 Then 'Line should be greater the slope calculation
                        nTargetIdx = nBreakPointIdx

                        sXMLSlopeDiff &= "<vtrendlines>"
                        If nTargetIdx > 1 Then
                            sXMLSlopeDiff &= "<line startindex='" & nTargetIdx & "' displayalways='1' displayvalue='Break Point' valueontop='1' color='ED320D' thickness='4' />"
                        Else
                            sXMLSlopeDiff &= "<line startindex='1' endindex='2' displayalways='1' displayvalue='Break Point' valueontop='1' color='ED320D' thickness='4' />"
                        End If
                        sXMLSlopeDiff &= "</vtrendlines>"
                    End If

                    sXMLSlopeDiff &= "</chart>"
                Else
                    TRHighChartZoomingLBI.Visible = True
                    If DDListLBI.SelectedValue = LBI.LBIValue Then
                        If LBIValue.Length > 0 Then
                            sLBIValue = LBIValue.Substring(0, LBIValue.Length - 1)
                            Seriesname = "{name:'LBI Value',color: '#99CCFF',lineWidth: 1.5,marker: {radius:3},data:[" & sLBIValue & "]}"

                        End If
                    Else
                        If AVGLBIValue.Length > 0 Then
                            sAVGLBIValue = AVGLBIValue.Substring(0, AVGLBIValue.Length - 1)
                            Seriesname = "{name:'Average LBI Value',color: '#99CCFF',lineWidth: 1.5,marker: {radius:3},data:[" & sAVGLBIValue & "]}"
                        End If
                    End If
                    'Load Graph
                    If Seriesname <> "" Then
                        TdLineGraphValue = Highchart_ZoomedLineGraph("ctl00_ContentPlaceHolder1_LBIAndAVGValueZoom", "", Seriesname, GridMinorAlternateColor)
                        LBIAndAVGValueZoom.InnerHtml = TdLineGraphValue
                    Else
                        LBIAndAVGValueZoom.InnerHtml = Empty_HighChart("ctl00_ContentPlaceHolder1_LBIAndAVGValueZoom")
                    End If
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub SetBatteryCapacityChart(ByVal BatteryMax As Double, ByVal BatteryMin As Double, ByVal Category_Capacity As String, ByVal BatteryCapacity As String)

            Dim sCategory_BatteryCapacity As String = ""
            Dim sBatteryCapacityValue As String = ""
            Dim Seriesname As String = ""
            Dim TdLineGraphValue As String
            Dim GridMinorAlternateColor As String = "#A8A6BC,#F7F7FB,#A6ADD2"

            Try

                If FlashEnable Then

                    TRFusionZoomongBattery.Visible = True
                    sXMLBattery = "<chart YAxisMaxValue ='" & (BatteryMax + 100) & "' yAxisMinValue='" & (BatteryMin - 100) & "' compactDataMode='1' dataSeparator='|' paletteThemeColor='5D57A5' divLineColor='5D57A5' divLineAlpha='70' vDivLineAlpha='70' allowPinMode='1' pixelsPerPoint='1'>"
                    sXMLBattery &= "<categories>"

                    If Category_Capacity.Length > 0 Then
                        sCategory_BatteryCapacity = Category_Capacity.Substring(0, Category_Capacity.Length - 1)
                    End If

                    sXMLBattery &= sCategory_BatteryCapacity
                    sXMLBattery &= "</categories>"

                    If BatteryCapacity.Length > 0 Then
                        sBatteryCapacityValue = BatteryCapacity.Substring(0, BatteryCapacity.Length - 1)
                    End If
                    sXMLBattery &= "<dataset seriesName='Battery Capacity'>" & sBatteryCapacityValue & "</dataset>"

                    sXMLBattery &= "</chart>"
                Else

                    TRHighChartZoomingBattery.Visible = True

                    If BatteryCapacity.Length > 0 Then
                        sBatteryCapacityValue = BatteryCapacity.Substring(0, BatteryCapacity.Length - 1)
                        Seriesname = "{name:'Battery Capacity',color: '#99CCFF',lineWidth: 1.5,marker: {radius:3},data:[" & sBatteryCapacityValue & "]}"
                    End If

                    'Load Graph
                    If Seriesname <> "" Then
                        TdLineGraphValue = Highchart_ZoomedLineGraph("ctl00_ContentPlaceHolder1_BatteryValueZoom", "", Seriesname, GridMinorAlternateColor)
                        BatteryValueZoom.InnerHtml = TdLineGraphValue
                    Else
                        BatteryValueZoom.InnerHtml = Empty_HighChart("ctl00_ContentPlaceHolder1_BatteryValueZoom")
                    End If
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

        Protected Sub ChkShowFilter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkRemoveFilter.CheckedChanged

            If ChkRemoveFilter.Checked = True Then
                FillData(True)
            Else
                FillData()
            End If

        End Sub

        Public Enum EnumMonitorBatteryType
            enum_Fanso = 1
            enum_Lithium = 2
            enum_Tadiran = 3
            enum_Alkaline = 4
        End Enum

    End Class

End Namespace
