Imports System
Imports System.IO
Imports System.Data
Imports System.xml
Namespace GMSUI
    Partial Class LocationChangeEvent
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
           
	    If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
	    
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If
	    
            h_SiteId.Value = Request.QueryString("sid")

            Try
                PageVisitDetails(g_UserId, "LocationChangeEvent", enumPageAction.View, "user visited Location Change Event for siteid - " & h_SiteId.Value)
            Catch ex As Exception
                WriteLog(" LocationChangeEvent - UserId " & g_UserId & ex.Message.ToString())
            End Try

        End Sub

    End Class
End Namespace

