Imports System.Data

Namespace GMSUI
    Partial Class MSESettings
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()

            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not (g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "MSE Settings Report Details", enumPageAction.AccessViolation, "user try to access report details List")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then
                doLoadsitlist()
            End If

            ddlSites.SelectedValue = Session("ReportSiteId")

            hid_userid.Value = g_UserId
            hid_userrole.Value = g_UserRole

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                siteList = loadsiteList(sCompanys, sSites)

                ddlSites.Items.Clear()
                ddlSites.Items.Add(New ListItem("Select Site", "0"))

                If siteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            ddlSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub
    End Class
End Namespace
