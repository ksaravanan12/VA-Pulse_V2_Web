<%@ Application Language="VB" %>
<%@ Import Namespace="System.IO" %>

<script Language="VB" runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        'Server.ScriptTimeout = 3600 'in seconds, equal to 10 Hrs
    End Sub
    
    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        '' Fires at the beginning of each request

        Dim sUrl As String = Request.Url.PathAndQuery
        If sUrl.ToUpper.Contains("GMS_LOG.TXT") Then
            Response.Redirect("ErrorPage.aspx?ErrMsg=File does not exist")
            Return
        End If
        
        
        For Each key As String In Request.QueryString
            GMSUI.General.CheckInput(Request.QueryString(key))
        Next

        For Each key As String In Request.Form
            GMSUI.General.CheckInput(Request.Form(key), True)
        Next

        For Each key As String In Request.Cookies
            GMSUI.General.CheckInput(Request.Cookies(key).Value)
        Next
    End Sub
    
    Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
        'Dim sCookie, sHTTPONLY, sPath As String

        'For Each sCookie In Response.Cookies
        '    sHTTPONLY = ";HttpOnly"
        '    sPath = Response.Cookies(sCookie).Path

        '    If (sPath.EndsWith(sHTTPONLY) = False) Then

        '        'force HttpOnly to be added to the cookie           
        '        Response.Cookies(sCookie).HttpOnly = True
        '        Response.Cookies(sCookie).Path += sHTTPONLY
        '    End If
        'Next

        'If Response.Cookies.Count > 0 Then
        '    For Each sCookie In Response.Cookies.AllKeys
        '        If (sCookie = FormsAuthentication.FormsCookieName Or LCase(sCookie) = "asp.net_sessionid") Then
        '            'If (sCookie = FormsAuthentication.FormsCookieName) Then
        '            Response.Cookies(sCookie).Secure = True
        '        End If
        '    Next
        'End If

    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
        Dim myRegex As Regex

        Try

            myRegex = New Regex("^(http|https):\/\/[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(([0-9]{1,5})?\/.*)?$/")

            If (myRegex.IsMatch(HttpContext.Current.Request.Url.ToString())) = False Then
                'Context.ClearError()
                'Context.RewritePath("ErrorPage.aspx?ErrMsg=" & Server.GetLastError.Message)
                If Not HttpContext.Current.Request.Url.ToString().Contains("cmd=UpdateProfiles") Then
                    GMSUI.General.WriteLog("SERVER ERROR = " & Server.GetLastError.Message)
                    HttpContext.Current.Response.Redirect("UserInputErrorPage.aspx")
                    Return
                End If
                
              
            End If

        Catch ex As Exception
            'Context.ClearError()
            'Context.RewritePath("ErrorPage.aspx?ErrMsg=" & Server.GetLastError.Message)
            GMSUI.General.WriteLog("SERVER ERROR = " & Server.GetLastError.Message)
            HttpContext.Current.Response.Redirect("UserInputErrorPage.aspx")
                   
            Return
        End Try
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        'Session.Timeout = 1440 'equal to 24 Hrs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
</script>