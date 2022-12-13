
Namespace GMSUI
    Partial Class ADConfiguration
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then
                If Not g_UserRole = enumUserRole.Admin Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If
            End If

        End Sub
    End Class
End Namespace
