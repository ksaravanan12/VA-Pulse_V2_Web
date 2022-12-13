Imports System
Imports System.IO
Imports System.Data
Imports System.xml

Namespace GMSUI
    Partial Class Home
    
        Inherits System.Web.UI.Page
        Dim dtCASites As DataTable
	
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            If IsAuthTokenValid() = False Then RedirectToErrorPage(LOGIN_SESSION_ERROR) 'Session Fixation Prevention

            Dim apiKey As String
            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            hid_userid.Value = g_UserId
            hid_userrole.Value = g_UserRole
            hid_IsTempMonitoring.value = g_IsTempMonitoring
            hid_userViews.Value = g_UserViews
            hid_AccessForStar.Value = g_AllowAccessForStar
	    
            Session("n_alertId") = ""
            Session("n_bin") = ""
            Session("n_siteId") = ""
            Session("n_typeId") = ""
            Session("t_alertId") = ""
            Session("t_bin") = ""
            Session("t_siteId") = ""

            Dim selectedid As String = ""

            selectedid = Request.QueryString("sid")
            If (selectedid <> "") Then
            Else
                Session("drpSiteid") = "0"
            End If

            If Not IsPostBack Then

                If g_UserRole = enumUserRole.AssetTrackUser Then
                    PageVisitDetails(g_UserId, "Site Summary", enumPageAction.AccessViolation, "user try to access Site Summary")
                    Response.Redirect("AccessDenied.aspx")
                End If

                Try
                    PageVisitDetails(g_UserId, "Login", enumPageAction.Logon, "A user successfully logged on.")
                Catch ex As Exception
                    WriteLog(" Login PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

                doLoadsitelist(selectedid)

            End If
        End Sub
	
        '**************************************************************************************************'
        ' Function Name : doLoadsitelist                                                                   '
        ' Input         : sid( SiteId ) or empty                                                           '
        ' Output        : Site information  as DataTable                                                   '     
        ' Description   : (API - SiteSummary)                                                           '
        '                 If the parameter is empty it will return all site for the user,                  '
        '                 If the SiteID(sid) is given it will return specifc site summary information      '
        '**************************************************************************************************'
        Sub doLoadsitelist(Optional ByVal sid As String = "")

            Dim site_dt As New DataTable
            Dim tblRow As New HtmlTableRow

            Dim siteHeaderCell As HtmlTableCell
            Dim tagCell As HtmlTableCell
            Dim Infra_Cell As HtmlTableCell

            Dim sitename As String = ""
            Dim siteid As String = ""
            Dim tag_lbiCount As String = ""
            Dim tag_UnderWatchCount As String = ""
            Dim Infra_StarActiveLbiCount As String = ""
            Dim Infra_StarUnderWatchCount As String = ""
            Dim strSystem As String = ""
            Dim lbi As String = ""
            Dim underwatch As String = ""
            Dim taglbi As String = ""
            Dim tagunderwatch As String = ""
            Dim showHearBeatContent As String = ""
            Dim showAlertContent As String = ""
            Dim systemcnt As String = ""
            Dim isHearbeatAvailable As String = "0"
            Dim isAlertAvailable As String = "0"
            Dim headerStr As String = ""
            Dim events As String = ""
            Dim heartbeatevents As String = ""
            Dim system As String = ""
            Dim tag As String = ""
            Dim Infrastructure As String = ""
            Dim sitecount As Integer = 0
            Dim pcServerVersion As String = ""
            Dim IsSiteConnectivityLost As String = "0"
            Dim isCusMaintView As Boolean = False
            Dim dRow() As DataRow
            Dim tblCAHtml As HtmlTable
            Dim tdHtml As HtmlTableCell

            Dim str As String
            Dim sTimeZone As String = ""

            Dim scssSiteStatus As String = ""
            Dim sSiteStatus As String = ""
            Dim PrismURL As String = ConfigurationManager.AppSettings("PrismURL").ToString()     

            If (sid = 0) Then
                sid = ""
            End If

            Dim IsPrismView As Boolean

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            'API CALL
            site_dt = loadsiteInfo(sCompanys, sSites, sid)

            Try

                If g_UserRole <> enumUserRole.Customer Then

                    'Load Critical Alert Sites in Left Menu
                    If Not site_dt Is Nothing Then
                        If site_dt.Rows.Count > 0 Then

                            dtCASites = site_dt.Clone()

                            dRow = site_dt.Select("IsCriticalAlert")

                            For Each row As DataRow In dRow
                                dtCASites.ImportRow(row)
                            Next

                            If Not dtCASites Is Nothing Then
                                If dtCASites.Rows.Count > 0 Then

                                    tblCAHtml = Master.FindControl("leftmenu").FindControl("tblcriticalAlert")

                                    tdHtml = Master.FindControl("headerBanner").FindControl("alerttd")
                                    tdHtml.Style.Add("Display", "")

                                    tblRow = New HtmlTableRow
                                    AddCell(tblRow, "Critical Alerts Sites ", , , , , , , "sText3")
                                    tblCAHtml.Rows.Add(tblRow)

                                    For Each row As DataRow In dtCASites.Rows
                                        With row
                                            tblRow = New HtmlTableRow
                                            siteid = CheckIsDBNull(.Item("SiteId"), False, "")
                                            sitename = CheckIsDBNull(.Item("SiteName"), False, "")

                                            If (sitename.Length > 35) Then
                                                sitename = sitename.Substring(0, 35) & " ..."
                                            End If

                                            str = "<a  class='sCriticalAlertsList' style='overflow:hidden !important; text-overflow: ellipsis; display:inline-block;' title='" & CheckIsDBNull(.Item("SiteName"), False, "") & "' onClick='loadAlertsBySiteId(" & siteid & ");'>" & sitename & "</a>"
                                            AddCell(tblRow, str, , , , , , , "sText4")

                                            tblCAHtml.Rows.Add(tblRow)
                                        End With
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If

            Catch ex As Exception
                WriteLog(" Critical ALert Site List : " & ex.Message.ToString())
            End Try

            If Not site_dt Is Nothing Then
                If site_dt.Columns.Count = 0 Then
                    Response.Redirect("Temporarilydown.htm")
                End If
            End If

            If g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                isCusMaintView = False
            End If

            siteinfo.Rows.Clear()

            If (site_dt.Rows.Count > 0) Then

                sitecount = site_dt.Rows.Count

                For nidx As Integer = 0 To sitecount - 1

                    With site_dt.Rows(nidx)

                        siteid = CheckIsDBNull(.Item("SiteId"), False, "")
                        sitename = CheckIsDBNull(.Item("SiteName"), False, "")
                        strSystem = CheckIsDBNull(.Item("System"), False, "")
                        pcServerVersion = CheckIsDBNull(.Item("PCServerVersion"), False, "").ToString.Replace(" ", "~")
                        IsPrismView = CheckIsDBNull(.Item("IsPrismView"), False, False)

                        'less than 30 days
                        tag_lbiCount = CheckIsDBNull(.Item("Tag_lbiCount"), False, "")

                        'less than 90 days
                        tag_UnderWatchCount = CheckIsDBNull(.Item("Tag_UnderWatchCount"), False, "")

                        'less than 30 days
                        Infra_StarActiveLbiCount = CheckIsDBNull(.Item("Infras_lbiCount"), False, "")

                        'less than 90 days
                        Infra_StarUnderWatchCount = CheckIsDBNull(.Item("Infras_UnderWatchCount"), False, "")

                        isHearbeatAvailable = CheckIsDBNull(.Item("isHeartbeatAvailable"), False, "0")
                        isAlertAvailable = CheckIsDBNull(.Item("isLAAlertsAvailable"), False, "0")
                        IsSiteConnectivityLost = CheckIsDBNull(.Item("IsSiteConnectivityLost"), False, "0")
                        sTimeZone = CheckIsDBNull(.Item("TimeZone"), False, "")

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 10, , , "top", "20px")
                        siteinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow

                        events = "onclick=Redirect('" & siteid & "','" & pcServerVersion & "','" & IsSiteConnectivityLost & "','" & Uri.EscapeDataString(sTimeZone) & "'); onmouseover='HeaderTabOver(this)' onmouseout='HeaderTabOut(this)'"
                        showHearBeatContent = ""
                        showAlertContent = ""

                        If IsPrismView And (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism) Then
                            showAlertContent = "<td align='center'><span onclick=RedirectPrismPage(" & g_UserId & ",'" & g_UserName & "'," & g_UserRole & ",'" & g_UserEmail & "','" & g_UserAPI & "'," & siteid & ",'" & PrismURL & "'); id='spnIcons_" + siteid + "'><img title='Centrak Prism' src='images/Prism.png' style='cursor: pointer;'  /></span></td>"
                        End If

                        scssSiteStatus = "clsOnlineSiteStatus"
                        sSiteStatus = "Active"

                        If CheckIsDBNull(site_dt.Rows(nidx).Item("IsSiteConnectivityLost"), False, "0") = 1 Then
                            scssSiteStatus = "clsOfflineSiteStatus"
                            sSiteStatus = "Inactive"
                        End If

                        headerStr = "<table border='0' cellpadding='0' cellspacing='0' width='100%' >" & _
                                        "<tr>" & _
                                            "<td id='td-" & siteid & "' " & events & ">" & _
                                                "<table border='0' cellpadding='0' cellspacing='0' width='100%' >" & _
                                                    "<tr>" & _
                                                        "<td align='left'>" & _
                                                                sitename & _
                                                        "</td>" & _
                                                        "<td align='right' class='subHeader1'>" & _
                                                            "<img  src='images/Signal-strength.png' style='width:12px;height:12px;' id='img-" + siteid + "' />&nbsp;&nbsp;View details" & _
                                                        "</td>" & _
                                                    "</tr>" & _
                                                "</table>" & _
                                            "</td>" & _
                                            showAlertContent & _
                                        "</tr>" & _
                                        "<tr>" & _
                                            "<td style='height: 25px;' class='" + scssSiteStatus + "'>" & _
                                                "Pulse Connection Status:&nbsp;<span>" + sSiteStatus + "</span>&nbsp;|&nbsp;Last Update:&nbsp;<span>" + site_dt.Rows(nidx).Item("Lastupdate") + "</span>" & _
                                            "</td>" & _
                                        "</tr>" & _
                                    "</table>"

                        If IsSiteConnectivityLost = "1" Then
                            siteHeaderCell = AddCell(tblRow, headerStr, , 10, , , "top", , "SHeader1Red")
                        Else
                            siteHeaderCell = AddCell(tblRow, headerStr, , 10, , , "top", , "SHeader1")
                        End If

                        siteinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 10, , , "top", , "bottomBorder")
                        siteinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 10, , , "top", "10px")
                        siteinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow

                        system = "<span style='padding-left: 25px;'>System</span>"
                        tag = "<span style='padding-left: 30px;'>Tags</span>"
                        Infrastructure = "<span style='padding-left: 30px;'>Infrastructure</span>"

                        AddCell(tblRow, "", , 1, "10px", , "top")
                        AddCell(tblRow, system, , 1, , , "top", , "system")
                        AddCell(tblRow, "", , 1, "35px", , "top")

                        If isCusMaintView Then
                            tagCell = AddCell(tblRow, tag, , 1, , , "top", , "CusTags")
                        Else
                            tagCell = AddCell(tblRow, tag, , 3, , , "top", , "Tags")
                        End If

                        tagCell.Attributes.Add("onmouseover", "TagHeaderTabOver(this)")
                        tagCell.Attributes.Add("onmouseout", "TagHeaderTabOut(this)")
                        tagCell.Attributes.Add("onclick", "loadSiteOverviewInfoOnClick('" & siteid & "','tag','" & pcServerVersion & "','" & IsSiteConnectivityLost & "','" & Uri.EscapeDataString(sTimeZone) & "');")
                        tagCell.Attributes.Add("id", "tagtd-" + siteid)
                        AddCell(tblRow, "", , 1, "35px", , "top")

                        If isCusMaintView Then
                            Infra_Cell = AddCell(tblRow, Infrastructure, , 1, , , "top", , "Cusinfrastructure")
                        Else
                            Infra_Cell = AddCell(tblRow, Infrastructure, , 3, , , "top", , "infrastructure")
                        End If

                        Infra_Cell.Attributes.Add("onmouseover", "InfraHeaderTabOver(this)")
                        Infra_Cell.Attributes.Add("onmouseout", "InfraHeaderTabOut(this)")
                        Infra_Cell.Attributes.Add("onclick", "loadSiteOverviewInfoOnClick('" & siteid & "','infrastructure','" & pcServerVersion & "','" & IsSiteConnectivityLost & "','" & Uri.EscapeDataString(sTimeZone) & "');")
                        Infra_Cell.Attributes.Add("id", "Infratd-" + siteid)

                        siteinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 10, , , "top", "5px")
                        siteinfo.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 1, "10px", , "top")

                        Dim htmlTagunderWatch As String = ""
                        Dim htmlMonitorunderWatch As String = ""

                        If Not isCusMaintView Then
                            htmlTagunderWatch = "<td style=' width:35px;' valign='top'></td>" & _
                                                "<td onmouseover ='TabOver(this);' onmouseout='TabOut(this);' onclick=loadTagInfoOnClick('" + siteid + "','" + tagunderwatch + "',1,'" & Uri.EscapeDataString(sTimeZone) & "'); id='tagundertd-" + siteid + "' >" & _
                                                                                "<table border='0' cellpadding='0' cellspacing='0'>" & _
                                                                                        "<tr>" & _
                                                                                           "<td class='infotd'>" & tag_UnderWatchCount & "</td>" & _
                                                                                        "</tr>" & _
                                                                                        "<tr>" & _
                                                                                            "<td class='infotd-battery-yellow'></td>" & _
                                                                                        "</tr>" & _
                                                                                "</table>" & _
                                                                            "</td>" & _
                                                                            "<td style=' width:8px;' valign='top'></td>"

                            htmlMonitorunderWatch = "<td style=' width:35px;' valign='top' ></td>" & _
                                                        "<td onmouseover ='TabOver(this);' onmouseout='TabOut(this);' onclick=loadInfraInfoOnClick('" + siteid + "','" + underwatch + "',1,'" & Uri.EscapeDataString(sTimeZone) & "');  id='infraundertd-" + siteid + "'>" & _
                                                        "<table border='0' cellpadding='0' cellspacing='0'>" & _
                                                                "<tr>" & _
                                                                    "<td class='infotd'>" & Infra_StarUnderWatchCount & "</td>" & _
                                                                "</tr>" & _
                                                                 "<tr>" & _
                                                                    "<td class='infotd-battery-yellow'></td>" & _
                                                                "</tr>" & _
                                                        "</table>" & _
                                                    "</td>" & _
                                                    "<td style=' width:8px;' valign='top'></td>"
                        Else
                            htmlTagunderWatch = "<td style='width:115px;' valign='top' ></td>"
                            htmlMonitorunderWatch = "<td style='width:120px;' valign='top' ></td>"
                        End If

                        Dim htmlStr As String = "<table border='0' cellpadding='0' cellspacing='0' >" & _
                                                "<tr>" & _
                                                    "<td onmouseover ='TabOver(this);' onmouseout='TabOut(this);' onclick=loadSiteOverviewInfoOnClick('" + siteid + "','system','" + pcServerVersion + "','" + IsSiteConnectivityLost + "','" & Uri.EscapeDataString(sTimeZone) & "'); id='systd-" + siteid + "'>" & _
                                                        "<table border='0' cellpadding='0' cellspacing='0'>" & _
                                                                "<tr>" & _
                                                                    "<td class='infotd'>" & strSystem & "</td>" & _
                                                                "</tr>" & _
                                                                "<tr>" & _
                                                                    "<td class='infotd-green'></td>" & _
                                                                "</tr>" & _
                                                        "</table>" & _
                                                   "</td>" & _
                                                     htmlTagunderWatch & _
                                                    "<td onmouseover ='TabOver(this);' onmouseout='TabOut(this);' onclick=loadTagInfoOnClick('" + siteid + "','" + taglbi + "',2,'" & Uri.EscapeDataString(sTimeZone) & "'); id='taglbitd-" + siteid + "' >" & _
                                                        "<table border='0' cellpadding='0' cellspacing='0'>" & _
                                                                "<tr>" & _
                                                                    "<td class='infotd'>" & tag_lbiCount & "</td>" & _
                                                                "</tr>" & _
                                                                 "<tr>" & _
                                                                     "<td class='infotd-battery-red'></td>" & _
                                                                "</tr>" & _
                                                        "</table>" & _
                                                    "</td>" & _
                                                    htmlMonitorunderWatch & _
                                                    "<td onmouseover ='TabOver(this);'  onmouseout='TabOut(this);' onclick=loadInfraInfoOnClick('" + siteid + "','" + lbi + "',2,'" & Uri.EscapeDataString(sTimeZone) & "'); id='infrataglbitd-" + siteid + "'>" & _
                                                        "<table border='0' cellpadding='0' cellspacing='0'>" & _
                                                                "<tr>" & _
                                                                    "<td class='infotd'>" & Infra_StarActiveLbiCount & "</td>" & _
                                                                "</tr>" & _
                                                                 "<tr>" & _
                                                                    "<td class='infotd-battery-red'></td>" & _
                                                                "</tr>" & _
                                                        "</table>" & _
                                                    "</td>" & _
                                                "</tr>" & _
                                                "</table>"

                        AddCell(tblRow, htmlStr, , 9)
                        siteinfo.Rows.Add(tblRow)

                    End With

                Next
            Else
                tblRow = New HtmlTableRow
                AddCell(tblRow, "No Records Found.", "center", 0, "300px", 5, , "40px", "noRecoredfound")
                siteinfo.Rows.Add(tblRow)
            End If

            Try
                PageVisitDetails(g_UserId, "Home - Site Summary", enumPageAction.View, "User visited site summary")
            Catch ex As Exception
                WriteLog(" Site Summary PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
            End Try

        End Sub
    End Class
End Namespace
