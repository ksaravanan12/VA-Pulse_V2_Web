Imports System.Data

Namespace GMSUI
    Partial Class CompanyAssociation
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            Dim msg As String = ""

            If Request.QueryString("msg") <> "" Then
                msg = Request.QueryString("msg")
            End If

            lblMessage.Text = msg

            If Not IsPostBack Then

                If Not g_UserRole = enumUserRole.Admin Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                doLoadCompanylist()
            End If

            hid_userid.Value = g_UserId
            hid_pwd.Value = doGetPassword()
            hid_userrole.Value = g_UserRole

        End Sub

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