Imports System.Data

Namespace GMSUI
    Partial Class Reports
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Engineering And g_UserRole <> enumUserRole.Support Then
                    PageVisitDetails(g_UserId, "Tags Not Seen Recently", enumPageAction.AccessViolation, "user try to access reports")
                    Response.Redirect("AccessDenied.aspx")
                End If

                If Val(Request.QueryString("SiteId")) > 0 Then
                    hdSiteId.Value = Val(Request.QueryString("SiteId"))
                End If

            End If
        End Sub

    End Class
End Namespace

