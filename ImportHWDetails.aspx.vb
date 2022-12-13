Imports System.Data

Namespace GMSUI
    Partial Class ImportHWDetails
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If Not (g_UserRole = enumUserRole.Admin) Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                Try
                    doLoadsitlist()
                Catch ex As Exception
                    WriteLog("Automated Reports Email Config - UserId " & g_UserId & ex.Message.ToString())
                End Try

                hid_userid.Value = g_UserId

            End If
        End Sub

        Sub doLoadsitlist()
            Try
                Dim siteList As New DataTable
                Dim sCompanys As String = Session("VACompanys")
                Dim sSites As String = Session("VASites")
                'API CALL
                siteList = loadsiteList(sCompanys, sSites)
                If siteList.Rows.Count > 0 Then
                    drpSites.Items.Add(New ListItem("Select", 0))
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            drpSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try
        End Sub
    End Class
End Namespace