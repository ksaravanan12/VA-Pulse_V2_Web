Namespace GMSUI
    Partial Class ApplicationError
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim sErrorNumber, sDescription As String
            'Put user code to initialize the page here
            Session.Clear()
            Session.RemoveAll()
            Session.Abandon()
            ExpiresCookies() 'Session Fixation
            If IsSecure() = False Then RedirectToSecurePage()

            sErrorNumber = Request.QueryString("ErrorValue")

            sDescription = "Your session has been expired, Please sign-in again"
            lblErrDescription.InnerHtml = sDescription


        End Sub
    End Class
End Namespace

