Imports System
Imports System.IO
Imports System.Data

Namespace GMSUI
    Partial Class AlertMaintenance
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            hidUserRole.Value = g_UserRole

            If Not IsPostBack Then
	    
                If g_UserRole <> enumUserRole.TechnicalAdmin And g_UserRole <> enumUserRole.Admin Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                hid_userrole.Value = g_UserRole
                hid_useremail.Value = g_UserEmail

            End If

        End Sub

    End Class
End Namespace

