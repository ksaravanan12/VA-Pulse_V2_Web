Imports System
Imports System.IO
Imports System.Data
Imports System.Xml
Imports System.Net

Namespace GMSUI
    Partial Class ImportDeviceName
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If Not (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                    PageVisitDetails(g_UserId, "Import Device Name", enumPageAction.AccessViolation, "user try to access Import Device Name")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                doLoadsitlist()
                LoadDeviceType(ddlDeviceType)

                Try
                    PageVisitDetails(g_UserId, "Import Device Name", enumPageAction.View, "user visited Import Device Name")
                Catch ex As Exception
                    WriteLog("Import Device Name - UserId " & g_UserId & ex.Message.ToString())
                End Try

                hid_userid.Value = g_UserId

            End If

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

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

        Sub LoadDeviceType(ByVal drpDeviceType As DropDownList)

            Dim Type As Integer = 0
            Dim EnumType As Integer()

            drpDeviceType.Items.Clear()
            EnumType = [Enum].GetValues(GetType(enumDeviceType))

            drpDeviceType.Items.Add(New ListItem("Select Device Type", 0))

            For Each Type In EnumType
                drpDeviceType.Items.Add(New ListItem([Enum].GetName(GetType(enumDeviceType), Type), Type))
            Next

        End Sub

    End Class

End Namespace

