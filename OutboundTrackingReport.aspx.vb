Imports System.Data
Imports System.Xml
Namespace GMSUI
    Partial Class OutboundTrackingReport
        Inherits System.Web.UI.Page
        Dim GMSAPI As New GMSAPI_New.GMSAPI_New

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()

            Dim apiKey As String

            Dim dtEmailCount As New DataTable
            Dim dtDevice As New DataTable

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not (g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "Out bound Tracking Report", enumPageAction.AccessViolation, "user try to access Outbound Tracking Report")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                doLoadsitlist()
                ddMonth.Items.Clear()

                For nIdx As Integer = 1 To 12
                    ddMonth.Items.Add(New ListItem(MonthName(nIdx), nIdx))
                Next

                ddMonth.SelectedValue = Now.Month
                LoadYear(ddlYear)
            End If

            divSummaryReport.Visible = True

            LoadData(False)

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable
            Dim sStrXml As XmlNode

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                sStrXml = GMSAPI.GetSiteListInfo(0, sCompanys, sSites, g_UserAPI)

                siteList = XMLNodeToDataTable(sStrXml)

                ddlSites.Items.Clear()
                ddlSites.Items.Add(New ListItem("All Site", "0"))

                If siteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            ddlSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

        Public Sub LoadYear(ByRef ddlYear As DropDownList)
            Dim today As DateTime
            today = DateTime.Now
            Dim todayYear As Integer = today.ToString("yyyy")

            Try
                ddlYear.Items.Add(New ListItem(todayYear - 1, todayYear - 1))
                ddlYear.Items.Add(New ListItem(todayYear, todayYear))

                ddlYear.Items.FindByValue(todayYear).Selected = True

            Catch ex As Exception
                WriteLog("MonthlySupportIssues --> LoadYear : " & ex.Message)
            End Try

        End Sub

        Private Sub LoadData(Optional ByVal bIsCSV As Boolean = False)
            Dim tr As HtmlTableRow
            Dim dtDevice As DataTable = Nothing
            Dim nMonth As Integer
           
            Dim nYear As Integer
            Dim nSiteId As Integer
            Dim ntotalCount As Integer = 0
            Dim Communicationtype As String
            
            Dim bVal As Boolean
            Dim view As DataView
            Dim dataViewSort As DataView
            Dim distinctDT As DataTable
            Dim dtsiterows() As DataRow
            Dim context As HttpContext
            Dim siteTotCnt As Integer

            Communicationtype = "Email"
            nSiteId = ddlSites.SelectedValue
            nMonth = ddMonth.SelectedValue
            nYear = ddlYear.SelectedValue

            Try
                dtDevice = GetOutboundReport(nSiteId, nMonth, nYear, False)

                If dtDevice Is Nothing Then
                    tr = New HtmlTableRow
                    AddCell(tr, "No Record Found", "center", 4, , , , , "clstblRowEven")
                    tblCountData.Rows.Add(tr)
                    Exit Sub
                End If

                If dtDevice.Rows.Count = 0 Then
                    tr = New HtmlTableRow
                    AddCell(tr, "No Record Found", "center", 4, , , , , "clstblRowEven")
                    tblCountData.Rows.Add(tr)
                    Exit Sub
                End If

                If chkIsDetailReport.Checked = False Then

                    divSummaryReport.Visible = True

                    If Not dtDevice Is Nothing Then
                        If dtDevice.Rows.Count > 0 Then
                            view = New DataView(dtDevice)
                            distinctDT = view.ToTable(True, "Siteid", "Sitename")
                        End If
                    End If

                    dataViewSort = New DataView(distinctDT)
                    dataViewSort.Sort = "Sitename ASC"
                    distinctDT = dataViewSort.ToTable()

                    If Not distinctDT Is Nothing Then
                        If distinctDT.Rows.Count > 0 Then
                            If bIsCSV Then

                                context = HttpContext.Current
                                InitiateCSV(context, "OutboundTrackingReport_" & ddMonth.SelectedItem.Text.ToString & "_" & dtDevice.Rows(0).Item("Year"))

                                AddCSVNewLine(context)
                                AddCSVCell(context, "Outbound Tracking Report ", True)
                                AddCSVNewLine(context)

                                AddCSVCell(context, "#", True)
                                AddCSVCell(context, "Site Name", True)
                                AddCSVCell(context, "Communication Type", True)
                                AddCSVCell(context, "No of Email Sent", True)
                                AddCSVNewLine(context)

                                For nIdx As Integer = 0 To distinctDT.Rows.Count - 1
                                    siteTotCnt = 0

                                    dtsiterows = dtDevice.Select("SiteId='" & distinctDT.Rows(nIdx).Item("SiteId") & "'")
                                    If Not dtsiterows Is Nothing Then
                                        For nSubIdx As Integer = 0 To dtsiterows.Length - 1
                                            siteTotCnt = siteTotCnt + Convert.ToInt32(dtsiterows(nSubIdx).Item("Count"))
                                        Next
                                    End If

                                    tr = New HtmlTableRow

                                    With distinctDT.Rows(nIdx)
                                        AddCSVCell(context, nIdx + 1, True)
                                        AddCSVCell(context, CheckIsDBNull(.Item("Sitename"), False, ""), True)
                                        AddCSVCell(context, Communicationtype, True)
                                        AddCSVCell(context, siteTotCnt)
                                        ntotalCount += siteTotCnt
                                    End With
                                    AddCSVNewLine(context)

                                Next

                                AddCSVCell(context, "", True)
                                AddCSVCell(context, "", True)
                                AddCSVCell(context, "Total", True)
                                AddCSVCell(context, ntotalCount, True)
                                AddCSVNewLine(context)
                                context.Response.End()

                            Else
                                For nIdx As Integer = 0 To distinctDT.Rows.Count - 1
                                    siteTotCnt = 0
                                    dtsiterows = dtDevice.Select("SiteId='" & distinctDT.Rows(nIdx).Item("SiteId") & "'")
                                    If Not dtsiterows Is Nothing Then
                                        For nSubIdx As Integer = 0 To dtsiterows.Length - 1
                                            siteTotCnt = siteTotCnt + Convert.ToInt32(dtsiterows(nSubIdx).Item("Count"))
                                        Next
                                    End If

                                    tr = New HtmlTableRow
                                    With distinctDT.Rows(nIdx)
                                        AddCell(tr, nIdx + 1)
                                        AddCell(tr, CheckIsDBNull(.Item("Sitename"), False, ""))
                                        AddCell(tr, Communicationtype, True)
                                        AddCell(tr, siteTotCnt, "right")
                                        ntotalCount += siteTotCnt
                                    End With

                                    If nIdx Mod 2 = 0 Then
                                        tr.Attributes.Add("class", "clstblRowEven")
                                    Else
                                        tr.Attributes.Add("class", "clstblRowOdd")
                                    End If
                                    tblCountData.Rows.Add(tr)
                                Next

                                tr = New HtmlTableRow
                                AddCell(tr, "", False, 2, "", , , , "clstblRowEven")
                                AddCell(tr, "Total", False, , "", , , , "clstblRowEven")
                                AddCell(tr, ntotalCount, "right", 0, , , , , "clsTotalCount")
                                tblCountData.Rows.Add(tr)

                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                WriteLog("LoadData --> Outboudtracking_Report : " & ex.Message)
            End Try

        End Sub

        Protected Sub btnExport_Click(sender As Object, e As EventArgs)
            LoadData(True)
        End Sub

    End Class
End Namespace