Namespace GMSUI
    Partial Class TagInfo
        Inherits System.Web.UI.Page
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If
      
            hid_userrole.Value = g_UserRole

            Session("n_alertId") = ""
            Session("n_bin") = ""
            Session("n_siteId") = ""
            Session("n_typeId") = ""
            Session("t_alertId") = ""
            Session("t_bin") = ""
            Session("t_siteId") = ""

            ' used when site dropdown list changed
            Dim selectedid As String = ""
            selectedid = Request.QueryString("sid")

            If (selectedid <> "") Then
            Else
                Session("drpSiteid") = "0"
            End If

            If Not IsPostBack Then
                If g_UserRole = enumUserRole.AssetTrackUser Then
                    PageVisitDetails(g_UserId, "Tag Info", enumPageAction.AccessViolation, "user try to access Tag Info")
                    Response.Redirect("AccessDenied.aspx")
                End If
            End If

        End Sub

    End Class

End Namespace
