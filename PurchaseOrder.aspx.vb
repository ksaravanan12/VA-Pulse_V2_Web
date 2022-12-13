Imports System
Imports System.IO
Imports System.Data

Namespace GMSUI
    Partial Class PurchaseOrder
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then
	    
                If Val(Request.QueryString("SiteId")) > 0 Then
                    hdSiteId.Value = Val(Request.QueryString("SiteId"))
                End If

                Try
                    PageVisitDetails(g_UserId, "Purchase Order", enumPageAction.View, "user visited Purchase Order Summary")
                Catch ex As Exception
                    WriteLog("Purchase Order - UserId " & g_UserId & ex.Message.ToString())
                End Try
		
            End If
        End Sub
    End Class
End Namespace

