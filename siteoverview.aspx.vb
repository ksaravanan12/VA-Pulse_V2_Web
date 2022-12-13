Imports System
Imports System.IO
Imports System.Data
Imports System.xml
Namespace GMSUI
    Partial Class siteoverview
        Inherits System.Web.UI.Page
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
            apiKey = g_UserAPI
            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            Dim sid As String
            Dim typ As String
            sid = Request.QueryString("sid")
            typ = Request.QueryString("typ")
            If (IsNothing(typ) Or typ = "") Then
                typ = ""
            End If

            Session("drpSiteid") = sid
            Session("n_alertId") = ""
            Session("n_bin") = ""
            Session("n_siteId") = ""
            Session("t_alertId") = ""
            Session("t_bin") = ""
            Session("t_siteId") = ""

            If Not IsPostBack Then
                doLoadSiteOverView(sid, typ)
            End If
        End Sub
        Sub doLoadSiteOverView(ByVal sid As String, ByVal argType As String)
            Dim sitename As String = ""
            Dim siteid As String
            Dim DeviceType, GroupId, GroupName, Good, LessThen180Days, LessThen90Days, LessThen30Days, Offline, Typ As String
            Dim tblRow As New HtmlTableRow
            Dim bl_deviceOther As Boolean = False


            Dim dt As New DataTable
            dt = LoadsiteOverview(sid)

            If (dt.Rows.Count > 0) Then
                tblRow = New HtmlTableRow
                AddCell(tblRow, "", , 9, , , "top", "20px")
                siteOverview.Rows.Add(tblRow)

                For idx As Integer = 0 To dt.Rows.Count - 1
                    If (idx = 0) Then
                        siteid = CheckIsDBNull(dt.Rows(idx).Item("SiteId"), False, "")
                        sitename = CheckIsDBNull(dt.Rows(idx).Item("SiteName"), False, "")

                        tblRow = New HtmlTableRow
                        Dim headerStr As String = "<table border='0' cellpadding='0' cellspacing='0' width='100%' >" & _
                                                        "<tr>" & _
                                                         "<td align='left' class='siteOverviwe_Home_arrow' onclick='Redirect();'>" & _
                                                                "<img  src='images/Left-Arrow.png' title='Home' border='0' />" & _
                                                            "</td>" & _
                                                            "<td style='width:15px;'></td>" & _
                                                            "<td align='left'>" & _
                                                                "<table border='0' cellpadding='0' cellspacing='0' >" & _
                                                                    "<tr>" & _
                                                                        "<td align='left' class='SHeader1'>" & _
                                                                                sitename & _
                                                                        "</td>" & _
                                                                    "</tr>" & _
                                                                     "<tr>" & _
                                                                        "<td align='left' class='subHeader_black'>Site Overview</td>" & _
                                                                    "</tr>" & _
                                                               "</table>" & _
                                                            "</td>" & _
                                                        "</tr>" & _
                                                    "</table>"

                        AddCell(tblRow, headerStr, , 6, , , "top", , "SHeader1")
                        siteOverview.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 6, , , "top", "10px", "bottomBorder")
                        siteOverview.Rows.Add(tblRow)

                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "", , 6, , , "top", "10px")
                        siteOverview.Rows.Add(tblRow)
                        If (argType = "tag" Or argType = "") Then
                            tblRow = New HtmlTableRow
                            AddCell(tblRow, "Tags", , 0, "200px", , "top", "40px", "siteOverview_TopLeft_Box")
                            AddCell(tblRow, "Good", "center", 0, "100px", , "top", "40px", "siteOverview_Box")
                            AddCell(tblRow, "Less than<br />180 days", "center", 0, "100px", , "top", "40px", "siteOverview_Box")
                            AddCell(tblRow, "Less than<br /> 90 days", "center", 0, "100px", , "top", "40px", "siteOverview_Box")

                            If g_UserRole <> enumUserRole.Customer Then
                                AddCell(tblRow, "Less than<br /> 30 days", "center", 0, "100px", , "top", "40px", "siteOverview_Box")
                                AddCell(tblRow, "Offline", "center", 0, "100px", , "top", "40px", "siteOverview_Topright_Box")
                            Else
                                AddCell(tblRow, "Less than<br /> 30 days", "center", 0, "100px", , "top", "40px", "siteOverview_Topright_Box")
                            End If

                            siteOverview.Rows.Add(tblRow)
                        End If
                    End If

                    DeviceType = CheckIsDBNull(dt.Rows(idx).Item("DeviceType"), False, "")
                    GroupId = CheckIsDBNull(dt.Rows(idx).Item("TypeID"), False, "")
                    Typ = CheckIsDBNull(dt.Rows(idx).Item("Type"), False, "")
                    If (DeviceType = "1") Then
                        GroupName = getTagImage(GroupId, Typ)
                    Else
                        GroupName = getInfraImage(GroupId, Typ)
                    End If

                    Good = CheckIsDBNull(dt.Rows(idx).Item("Good"), False, "")
                    If (Good = "0") Then
                        Good = "-"
                    Else
                        If (DeviceType = "2") Then
                            Good = "<a  class='cell_text_green' href=DeviceList1.aspx?sid=" & siteid & "&bin=0&typeId=" & GroupId & ">" & Good & "</a>"
                        ElseIf (DeviceType = "1") Then
                            Good = "<a  class='cell_text_green' href=PatientTagList.aspx?sid=" & siteid & "&bin=0&typeId=" & GroupId & ">" & Good & "</a>"
                        End If

                    End If
                    LessThen180Days = CheckIsDBNull(dt.Rows(idx).Item("LessThen180Days"), False, "")
                    If (LessThen180Days = "0") Then
                        LessThen180Days = "-"
                    End If
                    LessThen90Days = CheckIsDBNull(dt.Rows(idx).Item("LessThen90Days"), False, "")
                    If (LessThen90Days = "0") Then
                        LessThen90Days = "-"
                    Else
                        If (DeviceType = "2") Then
                            LessThen90Days = "<a  class='cell_text_Yellow' href=DeviceList1.aspx?sid=" & siteid & "&bin=1&typeId=" & GroupId & ">" & LessThen90Days & "</a>"
                        ElseIf (DeviceType = "1") Then
                            LessThen90Days = "<a  class='cell_text_Yellow' href=PatientTagList.aspx?sid=" & siteid & "&bin=1&typeId=" & GroupId & ">" & LessThen90Days & "</a>"
                        End If

                    End If

                    LessThen30Days = CheckIsDBNull(dt.Rows(idx).Item("LessThen30Days"), False, "")

                    If (LessThen30Days = "0") Then
                        LessThen30Days = "-"
                    Else
                        If (DeviceType = "2") Then
                            LessThen30Days = "<a  class='cell_text_Red' href=DeviceList1.aspx?sid=" & siteid & "&bin=2&typeId=" & GroupId & ">" & LessThen30Days & "</a>"
                        ElseIf (DeviceType = "1") Then
                            LessThen30Days = "<a  class='cell_text_Red' href=PatientTagList.aspx?sid=" & siteid & "&bin=2&typeId=" & GroupId & ">" & LessThen30Days & "</a>"
                        End If

                    End If

                    Offline = CheckIsDBNull(dt.Rows(idx).Item("Offline"), False, "")
                    If (Offline = "0") Then
                        Offline = "-"
                    Else
                        If (DeviceType = "2") Then
                            Offline = "<a  class='cell_text_offline' href=DeviceList1.aspx?sid=" & siteid & "&bin=3&typeId=" & GroupId & ">" & Offline & "</a>"
                        ElseIf (DeviceType = "1") Then
                            Offline = "<a  class='cell_text_offline' href=PatientTagList.aspx?sid=" & siteid & "&bin=3&typeId=" & GroupId & ">" & Offline & "</a>"
                        End If

                    End If
                    If (bl_deviceOther = False) Then
                        If (DeviceType <> "1") Then
                            bl_deviceOther = True
                            If (argType = "infrastructure" Or argType = "") Then
                                tblRow = New HtmlTableRow
                                AddCell(tblRow, "Infrastructure", "center", 0, "200px", , "top", "40px", "siteOverview_TopLeft_Box")
                                AddCell(tblRow, "Good", "center", 0, "100px", , "top", "40px", "siteOverview_Box")
                                AddCell(tblRow, "Less than<br />180 days", "center", 0, "100px", , "top", "40px", "siteOverview_Box")
                                AddCell(tblRow, "Less than<br /> 90 days", "center", 0, "100px", , "top", "40px", "siteOverview_Box")

                                If g_UserRole <> enumUserRole.Customer Then
                                    AddCell(tblRow, "Less than<br /> 30 days", "center", 0, "100px", , "top", "40px", "siteOverview_Box")
                                    AddCell(tblRow, "Offline", "center", 0, "100px", , "top", "40px", "siteOverview_Topright_Box")
                                Else
                                    AddCell(tblRow, "Less than<br /> 30 days", "center", 0, "100px", , "top", "40px", "siteOverview_Topright_Box")
                                End If
                                
                                siteotherDevice.Rows.Add(tblRow)
                            End If
                        End If
                    End If

                    If (DeviceType = "1") Then
                        If (argType = "tag" Or argType = "") Then
                            tblRow = New HtmlTableRow
                            AddCell(tblRow, GroupName, "center", 0, "200px", , "middle", "40px", "siteOverview_leftBox")
                            Dim gud As String = "<a href='PateintTagList.aspx?sid='" & siteid & "'&"
                            AddCell(tblRow, Good, "center", 0, "100px", , "top", "40px", "siteOverview_cell_Green")
                            AddCell(tblRow, LessThen180Days, "center", 0, "100px", , "top", "40px", "siteOverview_cell")
                            AddCell(tblRow, LessThen90Days, "center", 0, "100px", , "top", "40px", "siteOverview_cell_Yellow")
                            AddCell(tblRow, LessThen30Days, "center", 0, "100px", , "top", "40px", "siteOverview_cell_Red")

                            If g_UserRole <> enumUserRole.Customer Then
                                AddCell(tblRow, Offline, "center", 0, "100px", , "top", "40px", "siteOverview_cell_offline")
                            End If
                            
                            siteOverview.Rows.Add(tblRow)
                        End If
                    Else
                        If (argType = "infrastructure" Or argType = "") Then
                            tblRow = New HtmlTableRow
                            AddCell(tblRow, GroupName, "center", 0, "200px", , "middle", "40px", "siteOverview_leftBox")
                            AddCell(tblRow, Good, "center", 0, "100px", , "top", "40px", "siteOverview_cell_Green")
                            AddCell(tblRow, LessThen180Days, "center", 0, "100px", , "top", "40px", "siteOverview_cell")
                            AddCell(tblRow, LessThen90Days, "center", 0, "100px", , "top", "40px", "siteOverview_cell_Yellow")
                            AddCell(tblRow, LessThen30Days, "center", 0, "100px", , "top", "40px", "siteOverview_cell_Red")

                            If g_UserRole <> enumUserRole.Customer Then
                                AddCell(tblRow, Offline, "center", 0, "100px", , "top", "40px", "siteOverview_cell_offline")
                            End If

                            siteotherDevice.Rows.Add(tblRow)
                        End If
                    End If
                Next
            Else
                tblRow = New HtmlTableRow
                AddCell(tblRow, "No Records Found.", "center", 0, "300px", 5, , "40px", "noRecoredfound")
                siteOverview.Rows.Add(tblRow)
            End If
        End Sub
    End Class
End Namespace

