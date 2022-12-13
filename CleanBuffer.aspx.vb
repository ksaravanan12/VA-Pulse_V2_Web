
Namespace GMSUI
    Partial Class CleanBuffer
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If
            If Not IsPostBack Then
                If g_UserRole <> enumUserRole.Admin Then
                    PageVisitDetails(g_UserId, "Clean Buffer", enumPageAction.AccessViolation, "user try to access Clean Buffer")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If
            End If
        End Sub

        Protected Sub btnCleanBuffer_ServerClick(sender As Object, e As System.EventArgs) Handles btnCleanBuffer.ServerClick
            DBCleanBuffer()
        End Sub

    End Class
End Namespace
