Namespace GMSUI
    Partial Class UserInputErrorPage
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            lblError.Text = "Server encountered an error while processing your request. Please try again later."
	    
            Session.Clear()
            Session.RemoveAll()
            Session.Abandon()
            ExpiresCookies() 'Session Fixation
	    
        End Sub
    End Class
End Namespace
