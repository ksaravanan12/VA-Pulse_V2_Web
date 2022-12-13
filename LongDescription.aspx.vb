
Partial Class LongDescription
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AnnouncementId As String = Request.QueryString("AnnouncementId")
        Dim pageref As String = Request.QueryString("pageref")

        hdnAnnouncementId.Value = AnnouncementId
        hdnpageref.Value = pageref

    End Sub

End Class
