Imports System
Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class UserList
        Inherits System.Web.UI.Page

        Public Enum EnumUserTypeID
	
            enum_Admin = 1
            enum_SuperAdmin = 2
            enum_CompanyAdmin = 3
            enum_SiteUser = 4
            enum_CompanyUser = 5
            enum_RMAUser = 6
	    
        End Enum

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Support Then
                    PageVisitDetails(g_UserId, "User List", enumPageAction.AccessViolation, "user try to access User List")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                doLoadCompanylist()
                doLoadUserTypelist()
                doLoadUserRolelist()
                doLoadsitlist()

                Try
                    PageVisitDetails(g_UserId, "User List", enumPageAction.View, "user visited Manage Users")
                Catch ex As Exception
                    WriteLog(" User List PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
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

        Sub doLoadCompanylist()

            Dim dtCompany As New DataTable

            Try

                'API CALL
                dtCompany = loadcompanyList()

                If dtCompany.Rows.Count > 0 Then

                    selCompany.Items.Add(New ListItem("Select", 0))

                    For nidx As Integer = 0 To dtCompany.Rows.Count - 1
                        With (dtCompany.Rows(nidx))
                            selCompany.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next

                End If

            Catch ex As Exception
                WriteLog(" doLoadCompanylist " & ex.Message.ToString())
            End Try

        End Sub

        Sub doLoadUserTypelist()

            Dim dt As New DataTable

            Try

                'API CALL
                dt = loadusertypeList()

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then

                        selUser.Items.Add(New ListItem("Select", 0))

                        For nIdx As Integer = 0 To dt.Rows.Count - 1
                            With dt.Rows(nIdx)

                                If Not (.Item("UserTypeId") = EnumUserTypeID.enum_CompanyUser Or .Item("UserTypeId") = EnumUserTypeID.enum_Admin Or .Item("UserTypeId") = EnumUserTypeID.enum_RMAUser) Then
                                    selUser.Items.Add(New ListItem(.Item("UserType"), .Item("UserTypeId")))
                                End If

                            End With
                        Next

                    End If
                End If

            Catch ex As Exception
                WriteLog(" doLoadUserTypelist " & ex.Message.ToString())
            End Try

        End Sub

        Sub doLoadUserRolelist()

            Dim dt As New DataTable

            Try

                'API CALL
                dt = loaduserroleList()

                If dt.Rows.Count > 1 Then

                    selUserRole.Items.Add(New ListItem("Select", "-1"))

                    For idx As Integer = 0 To dt.Rows.Count - 1
                        If Not dt.Rows(idx).Item("UserRole") = "Admin" Then

                            If dt.Rows(idx).Item("UserRole") = "Centrak User" Then
                                selUserRole.Items.Add(New ListItem("Engineering", dt.Rows(idx).Item("UserRoleId")))
                            Else
                                selUserRole.Items.Add(New ListItem(dt.Rows(idx).Item("UserRole"), dt.Rows(idx).Item("UserRoleId")))
                            End If

                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doLoadUserRolelist " & ex.Message.ToString())
            End Try

        End Sub

        Sub doLoadsitlist()

            Dim dt As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try
                dt = loadsiteList(sCompanys, sSites)

                selSite.Items.Clear()
                selSite.Items.Add(New ListItem("All Sites", "0"))

                If dt.Rows.Count > 0 Then
                    For nidx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nidx))
                            selSite.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

    End Class

End Namespace