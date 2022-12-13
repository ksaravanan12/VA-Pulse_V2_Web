Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class UserControls_gms_Leftmenu
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

				
                lblPFname.Text = g_UserName

                If g_UserId = "0" Then
                    lblPFnameLink.HRef = ""
                    lblPFnameLink.Style.Add("color", "#313232")
                    lblPFnameLink.Style.Add("cursor", "default")
                Else
                    lblPFnameLink.HRef = "../Profile.aspx"
                    lblPFnameLink.Style.Add("color", "#005695")
                    lblPFnameLink.Style.Add("cursor", "pointer")
                End If

                If g_UserRole = enumUserRole.Admin Then
                    tdLeftHeader.InnerHtml = "System Admin"
                ElseIf g_UserRole = enumUserRole.TechnicalAdmin Then
                    tdLeftHeader.InnerHtml = "Technical Admin"
                ElseIf g_UserRole = enumUserRole.MaintenancePrism Then
                    tdLeftHeader.InnerHtml = "Maintenance/Prism"
                Else
                    tdLeftHeader.InnerHtml = [Enum].GetName(GetType(enumUserRole), g_UserRole)
                End If

                doLoadsitlist()
                doLoadReportType()

            End If
        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Try

                Dim sCompanys As String = Session("VACompanys")
                Dim sSites As String = Session("VASites")

                siteList = loadsiteList(sCompanys, sSites)

                selSiteForExport.Items.Clear()
                selSiteForExport.Items.Add(New ListItem("Select Site", "0"))

                If siteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            selSiteForExport.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

        Sub doLoadReportType()

            selReportforExport.Items.Clear()
            selReportforExport.Items.Add(New ListItem("Select Report Type", "0"))

            'VA GMS 1.0 CUSTOMER VIEW CHANGES
            If g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                selReportforExport.Items.Add(New ListItem("Less Than 30 Days", "2"))
            Else

                selReportforExport.Items.Add(New ListItem("Less Than 90 Days", "1"))
                selReportforExport.Items.Add(New ListItem("Less Than 30 Days", "2"))
                selReportforExport.Items.Add(New ListItem("All LBI", "4"))

                If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Partner Then
                    selReportforExport.Items.Add(New ListItem("Export All Devices", "5"))
                End If

            End If
	    
            If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                selReportforExport.Items.Add(New ListItem("SUPT Report", "6"))
                selReportforExport.Items.Add(New ListItem("All INACTIVE Devices", "7"))
                selReportforExport.Items.Add(New ListItem("Export I2 Tags", "6"))
            End If

        End Sub

        Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click

            Dim SiteId As Integer = selSiteForExport.Value
            Dim SiteItem As ListItem = selSiteForExport.Items.FindByValue(SiteId)
            Dim RptType As Integer = selReportforExport.Value

            If chkOnlineInput.Checked Then
                If hid_TrackDevice.Value = "1" Then
                    Response.Redirect("GeneratePdfView.aspx?SiteId=" & SiteId & "&RptType=" & RptType & "&SiteName=" & SiteItem.Text)
                Else
                    Response.Redirect("GeneratePdf.aspx?SiteId=" & SiteId & "&RptType=" & RptType & "&SiteName=" & SiteItem.Text)
                End If
            Else
                If Not DownloadPdf(SiteItem.Value, RptType, SiteItem.Text) Then
                    Response.Redirect("Home.aspx")
                End If
            End If

        End Sub
    End Class
End Namespace

