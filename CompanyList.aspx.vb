Imports System
Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI
    Partial Class CompanyList
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Support Then
                    PageVisitDetails(g_UserId, "Company List", enumPageAction.AccessViolation, "user try to access Company List")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                Try
                    PageVisitDetails(g_UserId, "Company List", enumPageAction.View, "user visited Company List")
                Catch ex As Exception
                    WriteLog(" Company List PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

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

    End Class

End Namespace
