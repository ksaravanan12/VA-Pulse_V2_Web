Imports System.Data

Namespace GMSUI
    Partial Class LocationHistoryreport
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()

            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            HttpContext.Current.Server.ScriptTimeout = 90000

            If Not IsPostBack Then

                doLoadsitlist()

            End If

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                siteList = loadsiteList(sCompanys, sSites)

                ddlLocationSites.Items.Clear()

                If siteList.Rows.Count > 1 Then
                    ddlLocationSites.Items.Add(New ListItem("Select Site", "0"))
                End If

                If siteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))

                            ddlLocationSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))

                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

    End Class
End Namespace

