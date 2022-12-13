Imports System.IO
Imports System.Data
Imports System.Xml
Imports System.Net

Namespace GMSUI
    Partial Class EMRMAreport
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Engineering And g_UserRole <> enumUserRole.Support Then
                    PageVisitDetails(g_UserId, "EM RMA Report", enumPageAction.AccessViolation, "user try to access EM RMA Report")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                doLoadsitlist()

                Try
                    PageVisitDetails(g_UserId, "EM RMA Report", enumPageAction.View, "EM RMA report generation")
                Catch ex As Exception
                    WriteLog("EM RMA Report Page Visit Details - UserId " & g_UserId & ex.Message.ToString())
                End Try

            End If

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Try

                Dim sCompanys As String = Session("VACompanys")
                Dim sSites As String = Session("VASites")

                siteList = loadsiteList(sCompanys, sSites)

                ddlsitelist.Items.Clear()
                ddlsitelist.Items.Add(New ListItem("All Sites", "0"))

                If siteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            ddlsitelist.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

        Protected Sub btnGenearate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenearate.Click

            DownloadCSV_GetEMRMAreport(ddlsitelist.SelectedValue, txtFromDate.Value, txtToDate.Value)

        End Sub

        Public Sub DownloadCSV_GetEMRMAreport(ByVal SiteId As String, ByVal sFromDate As String, ByVal sToDate As String)

            Dim dt As New DataTable
            Dim sFileName As String = ""

            sFileName = "EM_RMA_Report" & "_" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            Dim Csvtext As StringBuilder = New StringBuilder
            Dim context As HttpContext

            dt = GetEMRMAreport(SiteId, sFromDate, sToDate)

            If dt.Rows.Count > 0 Then

                If SiteId > 0 Then
                    sFileName = GetValidSiteName(dt.Rows(0).Item("SiteName")) & "_EM_RMA_Report" & "_" & CDate(sFromDate).ToString("MMddyyyy") & "to" & CDate(sToDate).ToString("MMddyyyy") & "_" & Format(Now, "hms")
                Else
                    sFileName = "All_Sites_EM_RMA_Report" & "_EM_RMA_Report" & "_" & CDate(sFromDate).ToString("MMddyyyy") & "to" & CDate(sToDate).ToString("MMddyyyy") & "_" & Format(Now, "hms")
                End If

            End If

            context = HttpContext.Current
            InitiateCSV(context, sFileName)

            Try

                AddCSVCell(context, "Site Name", True)
                AddCSVCell(context, "Device ID", True)
                AddCSVCell(context, "Device Type", True)
                AddCSVCell(context, "Model Number", True)
                AddCSVCell(context, "Date Flagged for RMA", True)
                AddCSVCell(context, "First Seen by Network Date", True)
                AddCSVCell(context, "Last Seen by Network Date", True)
                AddCSVCell(context, "Reporting Status", True)
                AddCSVCell(context, "Ship Date", True)
                AddCSVCell(context, "PO Number", True)
                AddCSVNewLine(context)

                If dt.Rows.Count > 0 Then

                    For nRowIdx = 0 To dt.Rows.Count - 1
                        With dt.Rows(nRowIdx)
                            AddCSVCell(context, .Item("SiteName").Replace(",", ""), True)
                            AddCSVCell(context, .Item("TagId"), True)
                            AddCSVCell(context, .Item("TagType"), True)
                            AddCSVCell(context, .Item("ModelNumber"), True)
                            AddCSVCell(context, .Item("RMAON"), True)
                            AddCSVCell(context, .Item("FirstSeen"), True)
                            AddCSVCell(context, .Item("LastSeen"), True)
                            AddCSVCell(context, .Item("IsActive"), True)
                            AddCSVCell(context, .Item("ShipDate"), True)
                            AddCSVCell(context, .Item("PoNumber").Replace(",", ""), True)
                            AddCSVNewLine(context)
                        End With

                    Next

                End If
            Catch ex As Exception

            End Try

            HttpContext.Current.Response.BufferOutput = True
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.[End]()

        End Sub

    End Class
End Namespace
