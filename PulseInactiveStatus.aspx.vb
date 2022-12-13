Imports System.IO
Imports System.Collections.Generic
Namespace GMSUI
    Partial Class PulseInactiveStatus
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
                    PageVisitDetails(g_UserId, "Pulse Inactive Status", enumPageAction.AccessViolation, "user try to Pulse Inactive Status")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                Try
                    PageVisitDetails(g_UserId, "Pulse Inactive Status", enumPageAction.View, "user visited Pulse Inactive Status")
                Catch ex As Exception
                    WriteLog(" Pulse Inactive Status PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

            End If
        End Sub

    End Class
End Namespace
