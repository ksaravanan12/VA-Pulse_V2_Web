Imports System
Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class SiteList
    
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
	    
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin Then
                    PageVisitDetails(g_UserId, "Site List", enumPageAction.AccessViolation, "user try to access Site List")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                Try
                    PageVisitDetails(g_UserId, "Site List", enumPageAction.View, "user visited Manage Sites")
                Catch ex As Exception
                    WriteLog(" Site List PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

                doLoadCompanylist()
                doLoadServerIPlist()

            End If

            hid_userid.Value = g_UserId
            hid_pwd.Value = doGetPassword()
            hid_userrole.Value = g_UserRole

        End Sub

        Function doGetPassword() As String
	
            Dim password As String = ""
            Dim dtPassword As New DataTable
 
            Try
	    
               
                'API CALL
                dtPassword = GetPasswordInfo(g_UserId)
		
                If dtPassword.Rows.Count > 0 Then
                    For nidx As Integer = 0 To dtPassword.Rows.Count - 1
                        password = dtPassword.Rows(0).Item("Password")
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doGetPassword " & ex.Message.ToString())
            End Try

            Return password
	    
        End Function

        Sub doLoadCompanylist()
	
	    Dim siteList As New DataTable
	
            Try
                
                'API CALL
                siteList = loadcompanyList()

                If siteList.Rows.Count > 0 Then
		
                    selCompany.Items.Add(New ListItem("Select", 0))
		    
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            selCompany.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
		    
                End If

            Catch ex As Exception
                WriteLog(" doLoadCompanylist " & ex.Message.ToString())
            End Try
	    
        End Sub
	
        Sub doLoadServerIPlist()
	
            Dim siteList As New DataTable

            Try
                'API CALL
                SiteList = loadserveripList()

                If SiteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To SiteList.Rows.Count - 1
                        With (SiteList.Rows(nidx))

                            If .Item("IsActiveIP") = True Then
                                ddServerName.Items.Add(New ListItem(.Item("ServerIP"), .Item("ServerIP"), True))
                            Else
                                ddServerName.Items.Add(New ListItem(.Item("ServerIP"), .Item("ServerIP"), False))
                            End If

                            ddEditServerName.Items.Add(New ListItem(.Item("ServerIP"), .Item("ServerIP"), True))

                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doLoadServerIPlist " & ex.Message.ToString())
            End Try
	    
        End Sub

        Protected Sub chkType_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkType.CheckedChanged
            'doLoadMasterSitelist()
        End Sub

        Protected Sub selCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles selCompany.SelectedIndexChanged
            'doLoadMasterSitelist()
        End Sub

    End Class
End Namespace