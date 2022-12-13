Imports System
Imports System.IO
Imports System.Data
Imports System.Xml
Namespace GMSUI
    Partial Class AllowStarSettings
        Inherits System.Web.UI.Page
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If Not (g_UserRole = enumUserRole.Admin) Then
                    PageVisitDetails(g_UserId, "Allow Star settings", enumPageAction.AccessViolation, "user try to access Allow Star settings")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                Try
                    PageVisitDetails(g_UserId, "Allow Star settings", enumPageAction.View, "user visited Allow Star settings")
                Catch ex As Exception
                    WriteLog("Automated Reports Email Config - UserId " & g_UserId & ex.Message.ToString())
                End Try

                hid_userid.Value = g_UserId

            End If

        End Sub
      
    End Class
End Namespace
