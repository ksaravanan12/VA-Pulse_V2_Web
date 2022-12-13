Imports System
Imports System.Data
Imports System.IO
Imports System.xml
Namespace GMSUI
    Partial Class GMSAdminlogin
        Inherits System.Web.UI.Page

        Dim s As New GMSAPI_New.GMSAPI_New

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim IsAdminPageAccess As String = ConfigurationManager.AppSettings("IsAdminPageAccess")

            If IsAdminPageAccess <> "1" Then
                Response.Redirect("AccessDenied.aspx")
            End If

            Session.Clear()
            txtPassword.Attributes.Add("type", "password")
        End Sub
        '*************************************************************************************************************************************************************************'
        ' Function Name : Login                                                                                                                                                   '
        ' Input         : Username and Paswword                                                                                                                                   '
        ' Output        : XML Data                                                                                                                                                '
        ' Description   : (API -UserAuthentication) Used to login. It will return XML data with                                                                                   '
        '                 (Response and API Access Key and Associated Sites and User role and User Type)                                                                          '
        '*************************************************************************************************************************************************************************'
        Protected Sub btnLogin_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.ServerClick

            Dim sUsername, sPwd As String
            Dim sStrXml As XmlNode
         
            sUsername = txtUsername.Value
            sPwd = txtPassword.Value

            Try

                sStrXml = s.UserAuthentication(sUsername, sPwd)

                Dim responsefromServer As String = sStrXml.Item("Response").InnerText

                If responsefromServer.ToLower() = "true" Then

                    Dim ApiKey As String = sStrXml.Item("AuthenticationKey").InnerText
                    Dim username As String = sStrXml.Item("Username").InnerText
                    Dim usertype As String = sStrXml.Item("usertype").InnerText
                    Dim UserID As String = sStrXml.Item("UserID").InnerText
                    Dim UserRole As String = sStrXml.Item("UserRoleId").InnerText
                    Dim userviews As String = sStrXml.Item("UserViews").InnerText
                    Dim UserCompanyId As String = sStrXml.Item("UserCompanyId").InnerText
                    Dim email As String = sStrXml.Item("Email").InnerText

                    g_IsADUser = 0

                    WriteLog(" UserName : " & username & " UserType : " & usertype & " UserRole : " & UserRole & "")

                    CreateAuthToken() 'Session Fixation Prevention

                    g_UserRole = UserRole
                    g_UserName = username
                    g_TypeId = usertype
                    g_UserAPI = ApiKey
                    g_UserId = UserID
                    g_UserViews = userviews
                    g_UserCompanyId = UserCompanyId
                    g_UserEmail = email
                    g_IsTempMonitoring = sStrXml.Item("IsTempMonitoring").InnerText
                    g_AllowAccessForStar = sStrXml.Item("AllowAccessForStar").InnerText
                    g_AllowAccessForKPI = sStrXml.Item("IsAllowAccessForKPI").InnerText
                    g_IsPulseReport = sStrXml.Item("IsPulseReport").InnerText

                    Response.Redirect("Home.aspx")

                Else
                    tdAlert.InnerHtml = responsefromServer
                    tdAlert.Style("display") = ""
                    txtPassword.Attributes.Add("type", "password")
                    tdAlert.Attributes.Add("class", "login-alertBox")
                    txtUsername.Value = ""
                    txtPassword.Value = ""
                End If

            Catch ex As Exception
                WriteLog(" Login : " & ex.Message.ToString)
            End Try
        End Sub

    End Class

End Namespace
