Imports System.Data
Namespace GMSUI
    Partial Class GMSReportDetails
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not (g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "EmaGMS Report Details", enumPageAction.AccessViolation, "user try to access report details List")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            Dim dtFolderName As New DataTable

            hdnSiteId.Value = Request.QueryString("qSiteId")
            hdnSiteName.Value = Request.QueryString("qSiteName")
            hdnReportType.Value = Request.QueryString("ReportType")
            hdnFromDate.Value = Request.QueryString("qFromDate")
            hdnToDate.Value = Request.QueryString("qFromDate")
            hdnDeviceId.Value = Request.QueryString("qDeviceId")
            hdnIsPaging.Value = Request.QueryString("qispaging")
            hdnStatus.Value = Request.QueryString("qStatus")

            If Not IsPostBack Then
                doLoadsitlist()

                If Len(hdnSiteId.Value) > 0 Then
                    dtFolderName = DownloadLogFile(hdnSiteId.Value)

                    If Not dtFolderName Is Nothing Then
                        If dtFolderName.Rows.Count > 0 Then
                            hdnSiteFolderName.Value = CheckIsDBNull(dtFolderName.Rows(0).Item("SiteFolder"), False, "")
                        End If
                    End If
                End If

            End If

            ddlSites.SelectedValue = Session("ReportSiteId")

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                siteList = loadsiteList(sCompanys, sSites)

                ddlSites.Items.Clear()
                ddlSites.Items.Add(New ListItem("Select Site", "0"))

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

    End Class
End Namespace