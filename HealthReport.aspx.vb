Imports System.Data

Namespace GMSUI
    Partial Class HealthReport
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()

            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not (g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "Health Report Details", enumPageAction.AccessViolation, "user try to access report details List")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            htnReportType.Value = Request.QueryString("qRptType")

            If Not IsPostBack Then
                doLoadsitlist()
            End If

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                siteList = loadsiteList(sCompanys, sSites)

                ddlSites.Items.Clear()
                ddlSites.Items.Add(New ListItem("All Sites", "0"))

                ddlDetailSites.Items.Clear()
                ddlDetailSites.Items.Add(New ListItem("Select Site", "0"))

                If siteList.Rows.Count > 0 Then

                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            ddlSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                            ddlDetailSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next

                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

        Protected Sub btnGenearateSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenearateSummary.Click

            DownloadCSV_TagMonitorSummaryReport(ddlSites.SelectedValue)

        End Sub

        Protected Sub btnGenerateDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateDetail.Click

            DownloadCSV_TagMonitorDetailReport(ddlDetailSites.SelectedValue)

        End Sub

        Public Sub DownloadCSV_TagMonitorSummaryReport(ByVal SiteId As String)

            Dim dt As New DataTable
            Dim sFileName As String = ""

            sFileName = "TagandInfrastructureHealthOverviewReport" & "_" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            Dim Csvtext As StringBuilder = New StringBuilder
            Dim context As HttpContext

            dt = GetHealthOverviewReportInfo(SiteId)

            context = HttpContext.Current

            InitiateCSV(context, sFileName)

            Try

                AddCSVCell(context, "Site Name", True)
                AddCSVCell(context, "Location Code", True)
                AddCSVCell(context, "Company Name", True)
                AddCSVCell(context, "Platform", True)
                AddCSVCell(context, "System Version", True)
                AddCSVCell(context, "Pulse Version", True)
                AddCSVCell(context, "Pulse Connection Status", True)
                AddCSVCell(context, "Last Pulse Connection Time", True)
                AddCSVCell(context, "Tag and Infrastructure Health Score", True)
                AddCSVCell(context, "Total Tags", True)
                AddCSVCell(context, "Good Tags", True)
                AddCSVCell(context, "Good Tags %", True)
                AddCSVCell(context, "Less than 90 Days Tags", True)
                AddCSVCell(context, "Less than 90 Days Tags %", True)
                AddCSVCell(context, "Less than 30 Days Tags", True)
                AddCSVCell(context, "Less than 30 Days Tags %", True)
                AddCSVCell(context, "Offline (Battery Depleted) Tags", True)
                AddCSVCell(context, "Offline (Battery Depleted) Tags %", True)
                AddCSVCell(context, "Total Infrastructure", True)
                AddCSVCell(context, "Good Infrastructure", True)
                AddCSVCell(context, "Good Infrastructure %	", True)
                AddCSVCell(context, "Less than 90 Days Infrastructure", True)
                AddCSVCell(context, "Less than 90 Days Infrastructure %", True)
                AddCSVCell(context, "Less than 30 Days Infrastructure", True)
                AddCSVCell(context, "Less than 30 Days Infrastructure %", True)
                AddCSVCell(context, "Offline Infrastructure", True)
                AddCSVCell(context, "Offline Infrastructure %", True)
                AddCSVCell(context, "Online Stars", True)
                AddCSVCell(context, "Online Monitors/VW", True)
                AddCSVCell(context, "Online LF Exciters", True)
                AddCSVCell(context, "Online Asset Tags", True)
                AddCSVCell(context, "Online Staff Tags", True)
                AddCSVCell(context, "Online Patient Tags", True)
                AddCSVCell(context, "Online EM Sensor", True)
                AddCSVNewLine(context)

                If dt.Rows.Count > 0 Then

                    For nRowIdx = 0 To dt.Rows.Count - 1

                        With dt.Rows(nRowIdx)

                            AddCSVCell(context, .Item("SiteName"), True)
                            AddCSVCell(context, .Item("LocationCode"), True)
                            AddCSVCell(context, .Item("CompanyName"), True)
                            AddCSVCell(context, .Item("Platform"), True)
                            AddCSVCell(context, .Item("SystemVersion"), True)
                            AddCSVCell(context, "V2", True)
                            AddCSVCell(context, .Item("Status"), True)
                            AddCSVCell(context, .Item("LastParsedFile"), True)
                            AddCSVCell(context, .Item("Score"), True)
                            AddCSVCell(context, .Item("TotalTags"), True)
                            AddCSVCell(context, .Item("GoodTags"), True)
                            AddCSVCell(context, .Item("GoodTagsPer"), True)
                            AddCSVCell(context, .Item("UnderwatchTags"), True)
                            AddCSVCell(context, .Item("UnderwatchTagsPer"), True)
                            AddCSVCell(context, .Item("LBITags"), True)
                            AddCSVCell(context, .Item("LBITagsPer"), True)
                            AddCSVCell(context, .Item("OfflineTags"), True)
                            AddCSVCell(context, .Item("OfflineTagPer"), True)
                            AddCSVCell(context, .Item("TotalInfras"), True)
                            AddCSVCell(context, .Item("GoodInfras"), True)
                            AddCSVCell(context, .Item("GoodInfraPer"), True)
                            AddCSVCell(context, .Item("UnderwatchInfras"), True)
                            AddCSVCell(context, .Item("UnderwatchInfraPer"), True)
                            AddCSVCell(context, .Item("LBIInfras"), True)
                            AddCSVCell(context, .Item("LBInfraPer"), True)
                            AddCSVCell(context, .Item("OfflineInfras"), True)
                            AddCSVCell(context, .Item("OfflineInfraPer"), True)
                            AddCSVCell(context, .Item("OnlineStars"), True)
                            AddCSVCell(context, .Item("OnlineMonitorsVW"), True)
                            AddCSVCell(context, .Item("OnlineLFExciters"), True)
                            AddCSVCell(context, .Item("OnlineAssetTags"), True)
                            AddCSVCell(context, .Item("OnlineStaffTags"), True)
                            AddCSVCell(context, .Item("OnlinePatientTags"), True)
                            AddCSVCell(context, .Item("OnlineEMSensor"), True)
                            AddCSVNewLine(context)

                        End With

                    Next

                End If
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadCSV_TagMonitorSummaryReport " & ex.Message.ToString())
            End Try

            HttpContext.Current.Response.BufferOutput = True
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.[End]()

        End Sub

        Public Sub DownloadCSV_TagMonitorDetailReport(ByVal SiteId As String)

            Dim TotalCnt, GoodCnt, UnderwatchCnt, LBICnt, OfflineCnt, OfflineLBICnt, OfflineStars As Integer
            Dim ds As New DataSet

            Dim dtSiteSummary As New DataTable
            Dim dtSiteDetail As New DataTable

            Dim sFileName As String = ""
            Dim SiteName As String = ""

            sFileName = "TagandInfrastructureHealthDetailedSiteReport" & "_" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            Dim Csvtext As StringBuilder = New StringBuilder
            Dim context As HttpContext

            Try

                ds = GetHealthDetailReportInfo(SiteId)

                If ds.Tables.Count > 0 Then

                    dtSiteSummary = ds.Tables(0)
                    dtSiteDetail = ds.Tables(1)

                    If dtSiteSummary.Rows.Count > 0 Then

                        With dtSiteSummary.Rows(0)
                            SiteName = CheckIsDBNull(.Item("SiteName"), , "")
                            TotalCnt = CheckIsDBNull(.Item("TotalCount"), , 0)
                            GoodCnt = CheckIsDBNull(.Item("GoodCount"), , 0)
                            UnderwatchCnt = CheckIsDBNull(.Item("UnderwatchCount"), , 0)
                            LBICnt = CheckIsDBNull(.Item("LBICount"), , 0)
                            OfflineLBICnt = CheckIsDBNull(.Item("OfflineLBICount"), , 0)
                            OfflineCnt = CheckIsDBNull(.Item("OfflineCount"), , 0)
                            OfflineStars = CheckIsDBNull(.Item("OfflineStars"), , 0)
                        End With

                    End If

                End If

                context = HttpContext.Current
                InitiateCSV(context, sFileName)

                AddCSVCell(context, "Site Name: " & SiteName, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Total Devices: " & TotalCnt, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Good Devices: " & GoodCnt, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Less than 90 Days Devices: " & UnderwatchCnt, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Less than 30 Days Devices: " & LBICnt, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Offline (Battery Depleted) Devices: " & OfflineLBICnt, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Offline Devices: " & OfflineCnt, True)
                AddCSVNewLine(context)

                AddCSVCell(context, "Offline Stars: " & OfflineStars, True)
                AddCSVNewLine(context)

                AddCSVNewLine(context)

                AddCSVCell(context, "Device ID", True)
                AddCSVCell(context, "Device Type", True)
                AddCSVCell(context, "Monitor Location", True)
                AddCSVCell(context, "Tag Name", True)
                AddCSVCell(context, "Star Name", True)
                AddCSVCell(context, "MAC Address", True)
                AddCSVCell(context, "Good", True)
                AddCSVCell(context, "Less than 90 Days", True)
                AddCSVCell(context, "Less than 30 Days", True)
                AddCSVCell(context, "Offline (Battery Depleted)", True)
                AddCSVCell(context, "Offline", True)
                AddCSVCell(context, "Battery Replaced On", True)
                AddCSVCell(context, "Date Last Seen by Network", True)
                AddCSVCell(context, "First Seen by Network", True)
                AddCSVNewLine(context)

                If dtSiteDetail.Rows.Count > 0 Then

                    For nRowIdx = 0 To dtSiteDetail.Rows.Count - 1

                        With dtSiteDetail.Rows(nRowIdx)

                            If .Item("Type") = enumDeviceType.Tag Then

                                AddCSVCell(context, .Item("DeviceID"), True)
                                AddCSVCell(context, .Item("DeviceType"), True)
                                AddCSVCell(context, .Item("LocationName"), True)
                                AddCSVCell(context, .Item("Name"), True)
                                AddCSVCell(context, "", True)
                                AddCSVCell(context, "", True)

                                If .Item("Catastrophiccases") = 1 Or .Item("Catastrophiccases") = 2 Then
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "Yes", True)
                                ElseIf .Item("Catastrophiccases") = 4 Then
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "Yes", True)
                                    AddCSVCell(context, "No", True)
                                Else
                                    AddCSVCell(context, "Yes", True)
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "No", True)
                                End If

                                If .Item("Offline") = 0 Then

                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "No", True)

                                Else

                                    If .Item("Catastrophiccases") = 1 Or .Item("Catastrophiccases") = 2 Then
                                        AddCSVCell(context, "Yes", True)
                                        AddCSVCell(context, "Yes", True)
                                    Else
                                        AddCSVCell(context, "No", True)
                                        AddCSVCell(context, "Yes", True)
                                    End If

                                End If

                                AddCSVCell(context, .Item("BatteryReplacementOn"), True)
                                AddCSVCell(context, .Item("LastSeen"), True)
                                AddCSVCell(context, .Item("FirstSeen"), True)

                            ElseIf .Item("Type") = enumDeviceType.Monitor Then

                                AddCSVCell(context, .Item("DeviceID"), True)
                                AddCSVCell(context, .Item("DeviceType"), True)
                                AddCSVCell(context, .Item("LocationName"), True)
                                AddCSVCell(context, "", True)
                                AddCSVCell(context, "", True)
                                AddCSVCell(context, "", True)

                                If .Item("Catastrophiccases") = 1 Then
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "Yes", True)
                                ElseIf .Item("Catastrophiccases") = 2 Then
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "Yes", True)
                                    AddCSVCell(context, "No", True)
                                Else
                                    AddCSVCell(context, "Yes", True)
                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "No", True)
                                End If

                                If .Item("Offline") = 0 Then

                                    AddCSVCell(context, "No", True)
                                    AddCSVCell(context, "No", True) '
                                Else

                                    If .Item("Catastrophiccases") = 1 Then
                                        AddCSVCell(context, "Yes", True)
                                        AddCSVCell(context, "Yes", True)
                                    Else
                                        AddCSVCell(context, "No", True)
                                        AddCSVCell(context, "Yes", True)
                                    End If

                                End If

                                AddCSVCell(context, .Item("BatteryReplacementOn"), True)
                                AddCSVCell(context, .Item("LastSeen"), True)
                                AddCSVCell(context, .Item("FirstSeen"), True)
                            Else

                                AddCSVCell(context, "", True)
                                AddCSVCell(context, .Item("DeviceType"), True)
                                AddCSVCell(context, "", True)
                                AddCSVCell(context, "", True)
                                AddCSVCell(context, .Item("Name"), True)
                                AddCSVCell(context, .Item("DeviceID"), True)

                                If .Item("Offline") = 0 Then
                                    AddCSVCell(context, "Yes", True)
                                Else
                                    AddCSVCell(context, "No", True)
                                End If

                                AddCSVCell(context, "No", True)
                                AddCSVCell(context, "No", True)
                                AddCSVCell(context, "No", True)

                                If .Item("Offline") = 1 Then
                                    AddCSVCell(context, "Yes", True)
                                Else
                                    AddCSVCell(context, "No", True)
                                End If

                                AddCSVCell(context, "", True)
                                AddCSVCell(context, .Item("LastSeen"), True)
                                AddCSVCell(context, "", True)

                            End If

                        End With

                        AddCSVNewLine(context)
                    Next

                End If
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadCSV_TagMonitorDetailReport " & ex.Message.ToString())
            End Try

            HttpContext.Current.Response.BufferOutput = True
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.[End]()

        End Sub
    End Class
End Namespace
