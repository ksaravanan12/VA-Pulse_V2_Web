Imports System
Imports System.Data
Imports System.IO
Imports System.xml
Namespace GMSUI
    Partial Class ChangePassword
        Inherits System.Web.UI.Page
        Dim dt As New DataTable

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim nResetKey As String = ""
            Session.Clear()

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                Try
                    nResetKey = Request.QueryString("zsgz25g2")

                    If nResetKey = "" Or nResetKey = "0" Then
                        tblChangedPw.Style.Add("display", "")
                        tblBeforeChangedPw.Style.Add("display", "none")
                        Exit Sub
                    Else
                        dt = GetUserNameByResetKey(nResetKey)
                        If Not dt Is Nothing Then
                            If dt.Rows.Count > 0 Then
                                If dt.Rows(0).Item("ResponseText") = "admin" Or dt.Rows(0).Item("ResponseText") = "" Then
                                    tblChangedPw.Style.Add("display", "")
                                    tblBeforeChangedPw.Style.Add("display", "none")
                                End If
                            End If
                        End If
                    End If

                Catch ex As Exception
                    tblChangedPw.Style.Add("display", "")
                    tblBeforeChangedPw.Style.Add("display", "none")
                End Try

            End If
            
            txtPassword.Attributes.Add("type", "password")
            txtConfirmPassword.Attributes.Add("type", "password")

        End Sub

       
        Protected Sub btnChangePwd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePwd.ServerClick
            Dim sUsername, sPwd As String
            Dim sStrXml As XmlNode
            Dim nResetKey As Integer

            ' sUsername = lblUserName.Text
            sPwd = txtPassword.Value

            Try

                nResetKey = Convert.ToInt32(Request.QueryString("zsgz25g2"))
                sPwd = txtPassword.Value

                Dim s As New GMSAPI_New.GMSAPI_New

                sStrXml = s.GetUserNameByResetKey(nResetKey)
                sUsername = sStrXml.InnerText

                dt = GetUserNameByResetKey(nResetKey)

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0).Item("ResponseText") = "" Then
                            tdAlert.InnerHtml = "Password Not Expired"
                            tdAlert.Style("display") = ""
                            Exit Sub
                        End If
                    End If
                End If

                dt = SetUpdatePassword(nResetKey, sPwd)
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0).Item("Response").ToString().ToLower = "true" Then
                            Dim ApiKey As String = dt.Rows(0).Item("AuthenticationKey")
                            Dim username As String = dt.Rows(0).Item("Username")
                            Dim usertype As String = dt.Rows(0).Item("usertype")
                            Dim UserID As String = dt.Rows(0).Item("UserID")
                            Dim UserRole As String = dt.Rows(0).Item("UserRoleId")

                            'check if the user role is API user type

                            If UserRole = enumUserRole.API_User Then
                                tdAlert.InnerHtml = "Access denied"
                                tdAlert.Style("display") = ""
                                txtPassword.Attributes.Add("type", "password")

                                txtPassword.Value = ""
                                Return
                            End If

                            'xmlnode = sStrXml.LastChild.ChildNodes

                            g_UserRole = UserRole
                            g_UserName = username
                            g_TypeId = usertype
                            g_UserAPI = ApiKey
                            g_UserId = UserID
                            Response.Redirect("Home.aspx")
                        Else
                            tdAlert.InnerHtml = "Incorrect User Name or<br />Password"
                            tdAlert.Style("display") = ""
                            txtPassword.Attributes.Add("type", "password")

                        End If

                    End If
                End If

                'sStrXml = s.UpdatePassword(nResetKey, sPwd)
                'Dim responsefromServer As String = sStrXml.Item("Response").InnerText


                'If responsefromServer.ToLower() = "true" Then
                '    Dim ApiKey As String = sStrXml.Item("AuthenticationKey").InnerText
                '    Dim username As String = sStrXml.Item("Username").InnerText
                '    Dim usertype As String = sStrXml.Item("usertype").InnerText
                '    Dim UserID As String = sStrXml.Item("UserID").InnerText
                '    Dim UserRole As String = sStrXml.Item("UserRoleId").InnerText

                '    'check if the user role is API user type

                '    If UserRole = enumUserRole.API_User Then
                '        tdAlert.InnerHtml = "Access denied"
                '        tdAlert.Style("display") = ""
                '        txtPassword.Attributes.Add("type", "password")

                '        txtPassword.Value = ""
                '        Return
                '    End If

                '    xmlnode = sStrXml.LastChild.ChildNodes

                '    g_UserRole = UserRole
                '    g_UserName = username
                '    g_TypeId = usertype
                '    g_UserAPI = ApiKey
                '    g_UserId = UserID
                '    Response.Redirect("Home.aspx")
                'Else
                '    tdAlert.InnerHtml = "Incorrect User Name or<br />Password"
                '    tdAlert.Style("display") = ""
                '    txtPassword.Attributes.Add("type", "password")
                'End If

            Catch ex As Exception
                WriteLog(" Login : " & ex.Message.ToString)
            End Try

        End Sub
    End Class

End Namespace
