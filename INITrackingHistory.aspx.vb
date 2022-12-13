Namespace GMSUI
    Partial Class INITrackingHistory
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not (g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "INI Tracking History", enumPageAction.AccessViolation, "user try to access INI Tracking History")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then

                If Val(Request.QueryString("sid")) > 0 Then
                    hdSiteId.Value = Val(Request.QueryString("sid"))
                    hid_userid.Value = g_UserId

                    Try
                        PageVisitDetails(g_UserId, "INI Change History", enumPageAction.View, "user visited INI Change History")
                    Catch ex As Exception
                        WriteLog(" INI Change History PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                    End Try
                End If

            End If
        End Sub
    End Class
End Namespace

