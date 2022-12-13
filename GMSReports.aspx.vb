Namespace GMSUI

    Partial Class GMSReports

        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Engineering And g_UserRole <> enumUserRole.Support _
                    And g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Partner And g_IsPulseReport = 0 And g_UserRole <> enumUserRole.TechnicalAdmin Then

                    PageVisitDetails(g_UserId, "Pulse Reports", enumPageAction.AccessViolation, "user try to access Pulse Reports")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub

                End If

                hdnUserRole.Value = g_UserRole

                Try
                    PageVisitDetails(g_UserId, "Pulse Reports", enumPageAction.View, "user visited Pulse Reports")
                Catch ex As Exception
                    WriteLog(" Pulse Reports PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

            End If

        End Sub

    End Class

End Namespace

