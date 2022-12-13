Imports System
Imports System.IO
Imports System.Data
Imports System.xml

Namespace GMSUI
    Partial Class Alertlist
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI
            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            ' used when site dropdown list changed
            Dim selectedid As String = ""
            selectedid = Request.QueryString("sid")

            If (selectedid <> "") Then
                Session("drpSiteid") = selectedid
            Else
                Session("drpSiteid") = "0"
            End If


            Session("n_alertId") = ""
            Session("n_bin") = ""
            Session("n_siteId") = ""
            Session("t_alertId") = ""
            Session("t_bin") = ""
            Session("t_siteId") = ""

            If Not IsPostBack Then
                doLoad_Alert_site_list(selectedid)
            End If
        End Sub
        Sub doLoad_Alert_site_list(Optional ByVal sid As String = "")
            '{"SiteId", "Site", "DeviceType", "AlertId", "AlertCount", "Description", "Status"}
            Dim tblRow As New HtmlTableRow
            Dim sitename As String = ""
            Dim siteid As String = ""
            Dim oldsiteiD As String = ""
            Dim DeviceType, AlertId, AlertCount, Description, Status As String
            Dim site_dt As New DataTable
            Dim sitecount As Integer = 0
            If (sid = 0) Then
                sid = ""
            End If

            site_dt = Load_Alert_site_list(sid)
            siteAlertinfo.Rows.Clear()
            If (site_dt.Rows.Count > 0) Then
                sitecount = site_dt.Rows.Count
                For nidx As Integer = 0 To sitecount - 1
                    siteid = CheckIsDBNull(site_dt.Rows(nidx).Item("SiteId"), False, "")
                    sitename = CheckIsDBNull(site_dt.Rows(nidx).Item("Site"), False, "")
                    DeviceType = CheckIsDBNull(site_dt.Rows(nidx).Item("DeviceType"), False, "")
                    AlertId = CheckIsDBNull(site_dt.Rows(nidx).Item("AlertId"), False, "")
                    AlertCount = CheckIsDBNull(site_dt.Rows(nidx).Item("AlertCount"), False, "")
                    Description = CheckIsDBNull(site_dt.Rows(nidx).Item("Description"), False, "")
                    Status = CheckIsDBNull(site_dt.Rows(nidx).Item("Status"), False, "")

                    If (siteid <> oldsiteiD) Then
                        oldsiteiD = siteid
                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 3, , , "top", "20px")
                        siteAlertinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, sitename, , 3, , , "top", , "SHeader1")
                        siteAlertinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 3, , , "top", , "bottomBorder")
                        siteAlertinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, " ", , 3, , , "top", "20px")
                        siteAlertinfo.Rows.Add(tblRow)
                    End If
                    'ProfileText
                    'alert_devicetype_RED
                    'alert_devicetype_Green
                    'alert_Normal_Text
                    Dim statusclassname As String = "alert_devicetype_Green"
                    If (Status = "1") Then
                        statusclassname = "alert_devicetype_RED"
                    End If

                    tblRow = New HtmlTableRow
                    AddCell(tblRow, DeviceType, , , 250, , "top", " 40px", statusclassname)

                    If (DeviceType.ToLower() = "tag" Or DeviceType.ToLower() = "monitor" Or DeviceType.ToLower() = "star") Then
                        Dim infotxt As String = ""
                        'AlertCount
                        If (DeviceType.ToLower() = "tag") Then
                            infotxt = "<a href=PatientTagList.aspx?alertId=" + AlertId + "&sid=" + siteid + " class='alert_normal_Blue' title='Offline Tag'>" & AlertCount & "</a>"
                        ElseIf (DeviceType.ToLower() = "monitor") Then
                            infotxt = "<a href=DeviceList1.aspx?alertId=" + AlertId + "&sid=" + siteid + " class='alert_normal_Blue' title='Offline Monitor'>" & AlertCount & "</a>"
                        ElseIf (DeviceType.ToLower() = "star") Then
                            infotxt = AlertCount
                        End If

                        AddCell(tblRow, infotxt, , , 250, , "top", " 40px", "alert_normal_Blue")

                    End If


                    Dim clsp As Integer = 0
                    If (DeviceType.ToLower() <> "tag" Or DeviceType.ToLower() <> "monitor" Or DeviceType.ToLower() <> "star") Then
                        clsp = 2
                    End If

                    AddCell(tblRow, Description, , clsp, 260, , "top", " 40px", "alert_Normal_Text")
                    siteAlertinfo.Rows.Add(tblRow)
                Next
            End If
        End Sub
    End Class

End Namespace
