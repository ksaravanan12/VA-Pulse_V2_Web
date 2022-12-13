Imports System
Imports System.Data
Imports System.IO
Imports System.Xml

Namespace GMSUI

    Partial Class Logout
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

	    Dim api As New GMSAPI_New.GMSAPI_New          

            Try
                ' Update Last Login Time
                api.UpdateLoginTime(g_UserName, 0, 0, "0")

                'Update Announcement seen time
                api.UpdateAnnouncementTime(g_UserAPI)

            Catch ex As Exception
                WriteLog(" Login : " & ex.Message.ToString)
            End Try
	    
            Session.Clear()
            Session.RemoveAll()
            Session.Abandon()
            ExpiresCookies() 'Session Fixation

            Dim sSSOLoginUrl As String = ""

            WriteLog("GMS Logout Page Load")

            Try
                sSSOLoginUrl = ConfigurationManager.AppSettings("SSOLoginPage").ToString()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception

            End Try

            WriteLog(" SSO Logout Page " & sSSOLoginUrl & "")

            Try

                If Len(sSSOLoginUrl) > 0 Then
                    Response.Redirect(sSSOLoginUrl)
                End If

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" ERROR SSO Logout Page " & sSSOLoginUrl & "")
            End Try

        End Sub
    End Class
End Namespace
