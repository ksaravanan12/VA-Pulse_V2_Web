Imports System.IO

Namespace GMSUI

    Partial Class ReportPdf

        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim RptType As String = ""
            Dim ExportType As String = ""

            If Request.QueryString("RptType") = 1 Then
                RptType = "Less Than 90 Days - "
            ElseIf Request.QueryString("RptType") = 2 Then
                RptType = "Less Than 30 Days - "
            ElseIf Request.QueryString("RptType") = 4 Then
                RptType = "All LBI - "
            ElseIf Request.QueryString("RptType") = 5 Then
                RptType = "All Devices - "
            ElseIf Request.QueryString("RptType") = 6 Then
                RptType = "SUPT - "
            End If

            If Request.QueryString("ExportType") = 1 Then
                ExportType = "Excel"
            Else
                ExportType = "PDF"
            End If

            Try
                PageVisitDetails(g_UserId, "Home - Generate Report", enumPageAction.View, RptType & ExportType & " Report exported for site - " & Request.QueryString("SiteName"))
            Catch ex As Exception
                WriteLog(" Home - Generate Report PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
            End Try

            If Request.QueryString("CertExport") = 1 Then
                Excel.DownloadExcelFile(Request.QueryString("SiteId"), Request.QueryString("DeviceType"), Request.QueryString("TypeId"), Request.QueryString("Bin"))
                Exit Sub
            End If

            If Request.QueryString("IsMetadataReport") = "1" Then

                DownloadCSVForMetadataReport(Request.QueryString("SiteName"), Request.QueryString("nSiteId"), Request.QueryString("CurPage"),
                                           Request.QueryString("PageSize"), Request.QueryString("FilterValue"), Request.QueryString("devicetype"))

            ElseIf Request.QueryString("IsOnDemand") = "1" Then

                DownloadOnDemandReport(Request.QueryString("SiteId"), Request.QueryString("TypeIds"), Request.QueryString("Date"))

            ElseIf Request.QueryString("RptType") = "5" Then

                If Request.QueryString("ExportType") = 1 Then
                    DownloadExcelForAllDevices(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                Else
                    DownloadPdfForAllDevices(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                End If

            ElseIf Request.QueryString("RptType") = "6" Then

                If Request.QueryString("ExportType") = 1 Then
                    DownloadExcelForSUPTTag(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                Else
                    DownloadPdfForSUPTTag(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                End If

            ElseIf Request.QueryString("RptType") = "7" Then

                If Request.QueryString("ExportType") = 1 Then
                    DownLoadExcelForInActiveDevice(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                Else
                    DownloadPdfForInActiveDevice(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                End If

            Else

                If Request.QueryString("ExportType") = 1 Then
                    DownloadExcel(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                Else
                    DownloadPdf(Request.QueryString("SiteId"), Request.QueryString("RptType"), Request.QueryString("SiteName"))
                End If

            End If

            Try
                PageVisitDetails(g_UserId, "Home", enumPageAction.View, "Report Downloaded")
            Catch ex As Exception
                WriteLog(" Home - Report Downloaded PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
            End Try

        End Sub
    End Class
End Namespace

