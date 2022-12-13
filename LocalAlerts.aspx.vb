Imports System
Imports System.IO
Imports System.Data

Namespace GMSUI
    Partial Class LocalAlerts
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then
                hid_userrole.Value = g_UserRole

                txtDate_LAServiceAlerts.Value = CDate(Now).ToString("MM/dd/yyyy")
                txtDate_LATag.Value = CDate(Now).ToString("MM/dd/yyyy")
                txtDate_LAMonitor.Value = CDate(Now).ToString("MM/dd/yyyy")
                txtDate_LAStar.Value = CDate(Now).ToString("MM/dd/yyyy")

                If Val(Request.QueryString("SiteId")) > 0 Then
                    hdSiteId.Value = Val(Request.QueryString("SiteId"))
                End If

                LoadDeviceType(ddlDeviceType_DeviceDetails)
                LoadDeviceTypeForTableView(ddlDeviceType_DeviceDetailsTableView)
            End If
        End Sub

        Sub LoadDeviceType(ByVal drpDeviceType As HtmlSelect)
            Dim Type As Integer = 0
            Dim EnumType As Integer()

            drpDeviceType.Items.Clear()
            drpDeviceType.Items.Add(New ListItem("Select", "-1"))
            EnumType = [Enum].GetValues(GetType(enumAlertGraphType))
            For Each Type In EnumType
                If Type <> 0 Then
                    drpDeviceType.Items.Add(New ListItem([Enum].GetName(GetType(enumAlertGraphType), Type), Type))
                End If
            Next
        End Sub

        Sub LoadDeviceTypeForTableView(ByVal drpDeviceType As HtmlSelect)
            Try
                drpDeviceType.Items.Add(New ListItem("All", 0))
                drpDeviceType.Items.Add(New ListItem("TAG", 1))
                drpDeviceType.Items.Add(New ListItem("MONITOR", 2))
                drpDeviceType.Items.Add(New ListItem("STAR", 3))
                drpDeviceType.Items.Add(New ListItem("TIME SERVER", 4))
                drpDeviceType.Items.Add(New ListItem("BACKUP UTILITY", 5))
                drpDeviceType.Items.Add(New ListItem("STREAMING SERVER", 6))
                drpDeviceType.Items.Add(New ListItem("PAGING SERVER", 7))
                drpDeviceType.Items.Add(New ListItem("LOCATION SERVER", 8))
                drpDeviceType.Items.Add(New ListItem("STAR LOG", 9))
                drpDeviceType.Items.Add(New ListItem("PC SERVER", 10))
                drpDeviceType.Items.Add(New ListItem("RAULAND SERVICE", 11))
                drpDeviceType.Items.Add(New ListItem("WIFICONNECTOR SERVICE", 12))
                drpDeviceType.Items.Add(New ListItem("UCS CONNECTOR SERVICE", 15))
                drpDeviceType.Items.Add(New ListItem("GMS SERVER", 16))
                drpDeviceType.Items.Add(New ListItem("GMS SERVICE", 17))
                drpDeviceType.Items.Add(New ListItem("HL7 CONNECTOR", 18))
                drpDeviceType.Items.Add(New ListItem("INI", 19))
                drpDeviceType.Items.Add(New ListItem("HEART BEAT", 20))
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace

