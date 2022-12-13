Namespace GMSUI
    Partial Class ErrorPage
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	
            Dim sErrorMsg As String

            sErrorMsg = Request.QueryString("ErrMsg")
            lblError.Text = "We are sorry, the page you requested cannot be found."
            If Len(sErrorMsg) > 0 Then lblError.Text = sErrorMsg
	    
        End Sub
    End Class
End Namespace
