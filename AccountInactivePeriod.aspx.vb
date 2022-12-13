Imports System.Data
Imports System.Net.Sockets
Imports System.Xml
Namespace GMSUI
    Partial Class AccountInactivePeriod
        Inherits System.Web.UI.Page
        Dim sStrXml As XmlNode
        Dim s As New GMSAPI_New.GMSAPI_New

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim apiKey As String
            Dim ds As New DataSet

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                Try

                    If Not g_UserRole = enumUserRole.Admin Then
                        PageVisitDetails(g_UserId, "Configuration", enumPageAction.AccessViolation, "user try to access Configuration")
                        Response.Redirect("AccessDenied.aspx")
                        Exit Sub
                    End If

                    'Get in active period
                    sStrXml = s.UpdateLoginPeriod(0, 0)

                    txtPeriod.Value = sStrXml.Item("Period").InnerText
                    txtLoginattempt.Value = sStrXml.Item("configattemptCnt").InnerText

                Catch ex As Exception
                    WriteLog("Page_Load - " & ex.Message)
                End Try

            End If

        End Sub

        Protected Sub btSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btSave.Click

            Dim Period As String = txtPeriod.Value
            Dim AttemptCount As String = txtLoginattempt.Value

            Try

                If Period > 0 Then
                    sStrXml = s.UpdateLoginPeriod(Period, AttemptCount)
                End If

            Catch ex As Exception
                WriteLog("btSave_Click - " & ex.Message)
            End Try

        End Sub

        Protected Sub btLoginattemptSave_Click(sender As Object, e As System.EventArgs) Handles btLoginattemptSave.Click

            Dim Period As String = txtPeriod.Value
            Dim AttemptCount As String = txtLoginattempt.Value

            Try

                If AttemptCount > 0 Then
                    sStrXml = s.UpdateLoginPeriod(Period, AttemptCount)
                End If

            Catch ex As Exception
                WriteLog("btLoginattemptSave_Click - " & ex.Message)
            End Try

        End Sub
    End Class
End Namespace

