Imports System.Data
Namespace GMSUI
    Partial Class StarHourlyDetail
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            hdnSiteId.Value = Request.QueryString("qSiteId")
            hdnSiteName.Value = Request.QueryString("qSiteName")
            hdnMacId.Value = Request.QueryString("qMacId")
            hdnStarId.Value = Request.QueryString("qStarId")
            hdnBeaconSlot.Value = GetSpecialBeaconSlot(hdnStarId.Value)
            hdnFromDate.Value = Request.QueryString("qDate")
            hdnStarType.Value = Request.QueryString("qStarType")
        End Sub

    End Class

End Namespace