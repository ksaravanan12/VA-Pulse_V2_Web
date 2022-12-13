Namespace GMSUI
    Partial Class UserErrorPage
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            lblError.Text = "User Input not sanitized..!"
            Session.Clear()
            Session.RemoveAll()
            Session.Abandon()
            ExpiresCookies() 'Session Fixation
	    
        End Sub
    End Class
End Namespace
