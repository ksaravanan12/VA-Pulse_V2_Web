Imports System
Imports System.IO
Imports System.Data
Imports System.Xml
Namespace GMSUI
    Partial Class UserActivityDetails
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
                    PageVisitDetails(g_UserId, "User Audit Log", enumPageAction.AccessViolation, "user try to access User Audit Log")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                LoadUserList()
                LoadEventType()

            End If
           
        End Sub

        Sub LoadUserList()

            Dim UserList As New DataTable

            Try

                'API CALL
                UserList = LoadUser()

                If UserList.Rows.Count > 0 Then
                    ddlUserList.Items.Add(New ListItem("All", 0))
                    For nidx As Integer = 0 To UserList.Rows.Count - 1
                        With (UserList.Rows(nidx))
                            ddlUserList.Items.Add(New ListItem(.Item("UserName"), .Item("UserId")))
                        End With
                    Next
                End If

            Catch ex As Exception

            End Try
        End Sub

        Sub LoadEventType()

            Dim dteventtype As New DataTable

            Try

                'API CALL
                dteventtype = loadEvent()

                If dteventtype.Rows.Count > 0 Then
                    ddlEventtype.Items.Add(New ListItem("All", 0))
                    For nidx As Integer = 0 To dteventtype.Rows.Count - 1
                        With (dteventtype.Rows(nidx))
                            ddlEventtype.Items.Add(New ListItem(.Item("PageAction"), .Item("PageActionId")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" LoadEventType " & ex.Message.ToString())
            End Try
        End Sub

    End Class
End Namespace