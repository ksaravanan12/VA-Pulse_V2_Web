Imports System.IO
Imports System.Data
Imports System.Xml
Namespace GMSUI
    Partial Class Profile
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If
	    
            Session("n_alertId") = ""
            Session("n_bin") = ""
            Session("n_siteId") = ""
            Session("t_alertId") = ""
            Session("t_bin") = ""
            Session("t_siteId") = ""
	    
            If Not IsPostBack Then
	    
                lblUsername.Text = g_UserName
                Dim str_userRole As String = getUserRole(g_UserRole)
                lblUserType.Text = str_userRole
		
                lblUserEmail.Text = g_UserEmail

                If g_IsADUser = 1 Then
                    btnchangepwd.Visible = False
                Else
                    btnchangepwd.Visible = True
                End If

            End If
            
            Try
                PageVisitDetails(g_UserId, "User Profile", enumPageAction.View, "user visited Profile")
            Catch ex As Exception
                WriteLog(" User Profile PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
            End Try
		  
            hid_userrole.Value = g_UserRole
            hid_userid.Value = g_UserId

        End Sub
    End Class
End Namespace
