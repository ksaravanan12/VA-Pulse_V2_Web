Imports System
Imports System.IO
Imports System.Data
Imports System.Xml
Namespace GMSUI

    Partial Class SiteAssociationAD
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

            Dim DeviceId As String = ""
            Dim CurPage As String = ""

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                doLoadMasterSitelist()
                doLoadADGrouplist()

            End If

            hid_userid.Value = g_UserId
            hid_pwd.Value = doGetPassword()
            hid_userrole.Value = g_UserRole

        End Sub

        Sub doLoadMasterSitelist()

            Dim siteList As New DataTable

            Try

                Dim sCompanys As String = Session("VACompanys")
                Dim sSites As String = Session("VASites")

                siteList = loadsiteList(sCompanys, sSites)

                If siteList.Rows.Count > 0 Then

                    selAssociationSite.Items.Add(New ListItem("Select", 0))

                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            selAssociationSite.Items.Add(New ListItem(.Item("SiteName"), .Item("SiteId")))
                        End With
                    Next

                End If

            Catch ex As Exception
                WriteLog(" doLoadMasterSitelist " & ex.Message.ToString())
            End Try

        End Sub

        Sub doLoadADGrouplist()

            Dim ADGroupList As New DataTable

            Try

                ADGroupList = loadADGroupList()

                If ADGroupList.Rows.Count > 0 Then

                    selAssociationVHAGroup.Items.Add(New ListItem("Select", 0))
                    For nidx As Integer = 0 To ADGroupList.Rows.Count - 1
                        With (ADGroupList.Rows(nidx))
                            selAssociationVHAGroup.Items.Add(New ListItem(.Item("VHAGroupName"), .Item("VHAGroupId")))
                        End With
                    Next

                End If

            Catch ex As Exception
                WriteLog(" doLoadMasterSitelist " & ex.Message.ToString())
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
