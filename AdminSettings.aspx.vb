
Namespace GMSUI
    Partial Class AdminSettings
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	
            If IsSecure() = False Then RedirectToSecurePage()
	    
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If
	    
            If Not IsPostBack Then
	    
                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Engineering And g_UserRole <> enumUserRole.Support And g_UserRole <> enumUserRole.TechnicalAdmin Then

                    PageVisitDetails(g_UserId, "Admin Settings", enumPageAction.AccessViolation, "user try to access Admin Settings")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub

                End If

                If g_UserRole <> enumUserRole.Admin Then

                    trManageCompanies.Visible = False
                    trManageSites.Visible = False
                    trManageUsers.Visible = False
                    trManageAnnouncements.Visible = False
                    trImportDeviceLocation.Visible = False
                    trAuditLog.Visible = False
                    trConfig.Visible = False
                    trPuseInactive.Visible = False
                    trstarsetting.Visible = False
                    trTagProfiles.Visible = False
                    trAD.Visible = False
                    trCompanyAssociation.Visible = False
                    trSiteAssociation.Visible = False
                    trRTLSDetails.Visible = False
                    trRoleMapping.Visible = False

                    If g_UserRole = enumUserRole.Support Then

                        trImportDeviceLocation.Visible = True
                        trManageUsers.Visible = True

                    End If

                    If g_UserRole = enumUserRole.TechnicalAdmin Then
                        trTagProfiles.Visible = True
                    End If

                End If

                Try
                    PageVisitDetails(g_UserId, "Admin Settings", enumPageAction.View, "user visited admin settings")
                Catch ex As Exception
                    WriteLog(" Admin Settings PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

            End If
        End Sub

    End Class
End Namespace
