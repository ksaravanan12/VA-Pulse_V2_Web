Imports GMSUI
Imports System.Data
Imports System.IO
Imports System.Web.HttpContext
Imports WebSupergoo.ABCpdf11

Partial Class GeneratePdfView
    Inherits System.Web.UI.Page
    Dim RptType As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("RptType") = 1 Then
                RptType = "Less Than 90 Days - "
            ElseIf Request.QueryString("RptType") = 2 Then
                RptType = "Less Than 30 Days - "
            ElseIf Request.QueryString("RptType") = 4 Then
                RptType = "All LBI - "
            End If
            LoadTable()
        End If
    End Sub

    Protected Sub btnGeneratePdf_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneratePdf.ServerClick
        Dim txtBatteryChanged As String = Request.Form("txtBatteryChanged")
        Dim chkUnableToLocate As String = Request.Form("chkUnableToLocate")
        Dim chkLocated As String = Request.Form("chkLocated")
        Dim chkBatteryReplaced As String = Request.Form("chkBatteryReplaced")
        Dim BatteryChanged As String() = Nothing

        If txtBatteryChanged <> "" Then
            BatteryChanged = txtBatteryChanged.Split(",")
        End If

        Dim txtBatteryChangedMonitor As String = Request.Form("txtBatteryChangedMonitor")
        Dim chkUnableToLocateMonitor As String = Request.Form("chkUnableToLocateMonitor")
        Dim chkLocatedMonitor As String = Request.Form("chkLocatedMonitor")
        Dim chkBatteryReplacedMonitor As String = Request.Form("chkBatteryReplacedMonitor")
        Dim BatteryChangedMonitor As String() = Nothing

        If txtBatteryChangedMonitor <> "" Then
            BatteryChangedMonitor = txtBatteryChangedMonitor.Split(",")
        End If

        Try
             PageVisitDetails(g_UserId, "Home - Generate Report", enumPageAction.View, RptType & "Online input - Report exported for site - " & Request.QueryString("SiteName"))
        Catch ex As Exception
            WriteLog(" Online input - Report Viewed PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
        End Try
      
        DownloadPdf(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"), True, BatteryChanged, chkUnableToLocate, chkLocated, chkBatteryReplaced, BatteryChangedMonitor, chkUnableToLocateMonitor, chkLocatedMonitor, chkBatteryReplacedMonitor)
    End Sub

    Private Sub LoadTable()
        tdSiteName_GeneratePdf.InnerHtml = Request.QueryString("SiteName")
        lblDate_GeneratePdf.InnerHtml = Now.Date()
        If Request.QueryString("RptType") = "1" Then
            lblReportType_GeneratePdf.InnerHtml = "Underwatch"
        ElseIf Request.QueryString("RptType") = "2" Then
            lblReportType_GeneratePdf.InnerHtml = "Lbi"
        ElseIf Request.QueryString("RptType") = "4" Or Request.QueryString("RptType") = "" Then
            lblReportType_GeneratePdf.InnerHtml = "All LBI"
        End If

        LoadTableForPdf(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("apikey"), tblTagInfo, tblMonitorInfo)

        Try
            PageVisitDetails(g_UserId, "Home - Generate Report", enumPageAction.View, RptType & "Online input - Report viewed for site - " & Request.QueryString("SiteName"))
        Catch ex As Exception
            WriteLog(" Online input - Report Downloaded PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
        End Try

    End Sub
End Class
