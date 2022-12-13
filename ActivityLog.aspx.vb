Imports System
Imports System.IO
Imports System.Data
Imports System.Xml
Imports System.Net
Imports Centrak_LogExtractor

Namespace GMSUI
    Partial Class ActivityLog
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim ds As New DataSet
            Dim sDeviceId As String
            Dim sDeviceType As String
            Dim sTagType As String = ""
            Dim sCacheKey As String

            If Not IsPostBack Then
                sDeviceType = Request.QueryString("qDeviceType")
                sDeviceId = Request.QueryString("qDeviceId")
                sCacheKey = Request.QueryString("qCacheKey")

                If Not sCacheKey Is Nothing Then
                    ds = Cache(sCacheKey)

                    LoadData(ds, sDeviceId, sDeviceType, sTagType)
                End If
            End If

        End Sub


        Sub LoadData(ByVal ds As DataSet, ByRef DeviceId As String, ByVal DeviceType As String, ByVal TagType As String)

            Dim tblRow As New HtmlTableRow
            Dim ColumnName As String = ""

            Dim dtLocation As New DataTable
            Dim dtPageReq As New DataTable
            Dim dtPageRes As New DataTable

            Dim drLocation() As DataRow
            Dim drPageReq() As DataRow
            Dim drPageRes() As DataRow

            Dim nPageRes As Integer = 0
            Dim nPageReq As Integer = 0

            If ds Is Nothing Then Return
            If ds.Tables.Count = 0 Then Return

            If DeviceType = enumDeviceType.Tag Then
                dtLocation = ds.Tables(1)
                dtPageReq = ds.Tables(2)
                dtPageRes = ds.Tables(3)
                ColumnName = "TagId"
            ElseIf DeviceType = enumDeviceType.Monitor Then
                dtLocation = ds.Tables(5)
                dtPageReq = ds.Tables(6)
                dtPageRes = ds.Tables(7)
                ColumnName = "MonitorId"
            ElseIf DeviceType = enumDeviceType.Star Then
                dtLocation = ds.Tables(9)
                ColumnName = "StarId"

                trPage_Res.Visible = False
            End If



            Try

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '                                                               Location Data
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim LBIvalue As String = "0"

                If Not dtLocation Is Nothing Then

                    If dtLocation.Columns.Contains("SiteId") Then
                        dtLocation.Columns.Remove("SiteId")
                    End If

                    tblRow = New HtmlTableRow
                    AddCell(tblRow, "#")

                    For nColIdx = 0 To dtLocation.Columns.Count - 1
                        If dtLocation.Columns(nColIdx).ColumnName = "Command" Then
                            AddCell(tblRow, dtLocation.Columns(nColIdx).ColumnName, , , "280px")
                        ElseIf dtLocation.Columns(nColIdx).ColumnName = "UpdatedOn" Then
                            AddCell(tblRow, dtLocation.Columns(nColIdx).ColumnName, , , "280px")
                        Else
                            AddCell(tblRow, dtLocation.Columns(nColIdx).ColumnName)
                        End If
                    Next

                    tblRow.Attributes.Add("class", "dashboardStatusEvenRow")
                    tblLocation.Rows.Add(tblRow)

                    If dtLocation.Rows.Count > 0 Then
                        drLocation = dtLocation.Select("" & ColumnName & "='" & DeviceId & "'")

                        If drLocation.Length > 0 Then
                            For nIdx = 0 To drLocation.Length - 1

                                tblRow = New HtmlTableRow
                                AddCell(tblRow, nIdx + 1)

                                For nColIdx = 0 To dtLocation.Columns.Count - 1

                                    If DeviceType = enumDeviceType.Tag Then

                                        'LBI Value
                                        If (nColIdx = 8) Then
                                            Dim currentLBIvalue As String = CheckIsDBNull(drLocation(nIdx).Item(nColIdx), False, "")
                                            If (currentLBIvalue <> 0) Then
                                                LBIvalue = CheckIsDBNull(drLocation(nIdx).Item(nColIdx), False, "")
                                                AddCell(tblRow, LBIvalue)
                                            ElseIf (currentLBIvalue = 0) Then
                                                AddCell(tblRow, LBIvalue)
                                            End If
                                        Else
                                            AddCell(tblRow, CheckIsDBNull(drLocation(nIdx).Item(nColIdx), False, ""))
                                        End If

                                    Else
                                        AddCell(tblRow, CheckIsDBNull(drLocation(nIdx).Item(nColIdx), False, ""))
                                    End If

                                Next

                                If nIdx Mod 2 = 0 Then
                                    tblRow.Attributes.Add("class", "clsLogRowOdd")
                                Else
                                    tblRow.Attributes.Add("class", "clsLogRowEven")
                                End If

                                tblLocation.Rows.Add(tblRow)
                                tblLocation.Style.Add("display", "")

                            Next
                        End If

                    End If
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '                                                               Page Request
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                If Not dtPageReq Is Nothing Then

                    tblRow = New HtmlTableRow
                    AddCell(tblRow, "#")

                    For nColIdx = 0 To dtPageReq.Columns.Count - 1
                        If dtPageReq.Columns(nColIdx).ColumnName <> "Id" Then
                            AddCell(tblRow, dtPageReq.Columns(nColIdx).ColumnName)
                        End If
                    Next

                    tblRow.Attributes.Add("class", "dashboardStatusEvenRow")
                    tblPageReq.Rows.Add(tblRow)

                    If dtPageReq.Rows.Count > 0 Then
                        drPageReq = dtPageReq.Select("Id='" & DeviceId & "'")

                        If drPageReq.Length > 0 Then

                            nPageReq = drPageReq.Length

                            For nIdx = 0 To drPageReq.Length - 1

                                tblRow = New HtmlTableRow
                                AddCell(tblRow, nIdx + 1)

                                For nColIdx = 0 To dtPageReq.Columns.Count - 1
                                    If dtPageReq.Columns(nColIdx).ColumnName <> "Id" Then
                                        AddCell(tblRow, CheckIsDBNull(drPageReq(nIdx).Item(nColIdx), False, ""))
                                    End If
                                Next

                                If nIdx Mod 2 = 0 Then
                                    tblRow.Attributes.Add("class", "clsLogRowOdd")
                                Else
                                    tblRow.Attributes.Add("class", "clsLogRowEven")
                                End If

                                tblPageReq.Rows.Add(tblRow)
                                tblPageReq.Style.Add("display", "")

                            Next
                        Else
                            tblRow = New HtmlTableRow
                            AddCell(tblRow, "<font color='#EA4335'>No record found...!</font>", , dtPageReq.Columns.Count)
                            tblPageReq.Rows.Add(tblRow)
                        End If
                    Else
                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "<font color='#EA4335'>No record found...!</font>", , dtPageReq.Columns.Count)
                        tblPageReq.Rows.Add(tblRow)
                    End If
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '                                                               Page Response
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                If Not dtPageRes Is Nothing Then

                    tblRow = New HtmlTableRow
                    AddCell(tblRow, "#")

                    For nColIdx = 0 To dtPageRes.Columns.Count - 1
                        If dtPageRes.Columns(nColIdx).ColumnName <> "Id" Then
                            AddCell(tblRow, dtPageRes.Columns(nColIdx).ColumnName)
                        End If
                    Next

                    tblRow.Attributes.Add("class", "dashboardStatusEvenRow")
                    tblPageRes.Rows.Add(tblRow)

                    If dtPageRes.Rows.Count > 0 Then

                        drPageRes = dtPageRes.Select("Id='" & DeviceId & "'")

                        If drPageRes.Length > 0 Then

                            nPageRes = drPageRes.Length

                            For nIdx = 0 To drPageRes.Length - 1

                                tblRow = New HtmlTableRow
                                AddCell(tblRow, nIdx + 1)

                                For nColIdx = 0 To dtPageRes.Columns.Count - 1
                                    If dtPageRes.Columns(nColIdx).ColumnName <> "Id" Then
                                        AddCell(tblRow, CheckIsDBNull(drPageRes(nIdx).Item(nColIdx), False, ""))
                                    End If
                                Next

                                If nIdx Mod 2 = 0 Then
                                    tblRow.Attributes.Add("class", "clsLogRowOdd")
                                Else
                                    tblRow.Attributes.Add("class", "clsLogRowEven")
                                End If

                                tblPageRes.Rows.Add(tblRow)

                            Next
                        Else
                            tblRow = New HtmlTableRow
                            AddCell(tblRow, "<font color='#EA4335'>No record found...!</font>", , dtPageRes.Columns.Count)
                            tblPageRes.Rows.Add(tblRow)
                        End If
                    Else
                        tblRow = New HtmlTableRow
                        AddCell(tblRow, "<font color='#EA4335'>No record found...!</font>", , dtPageRes.Columns.Count)
                        tblPageRes.Rows.Add(tblRow)
                    End If
                End If

                'logSummary3.InnerHtml = "No of Page request " & nPageReq & ", No of Page response " & nPageRes
                noofPages.InnerHtml = nPageReq
                noofRes.InnerHtml = nPageRes

            Catch ex As Exception
                WriteLog(" ActivityLog :LoadData :" & ex.Message.ToString())
            End Try
        End Sub
    End Class
End Namespace
