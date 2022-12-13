Imports System.Data
Imports System.IO

Namespace GMSUI

    Partial Class uploadFile
    
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim ds As New DataSet

            Dim dtResult As New DataTable
            Dim dtNoResult As New DataTable

            Dim noResultRow As HtmlTableRow

            Dim hdnMapCmd As String = Request.Form("hdnTagMapCmd")

            If hdnMapCmd = "AddTag" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnTagSiteId As String = Request.Form("hdnTagSiteId")
                Dim file As HttpPostedFile = Request.Files("tagcsv")

                If Path.GetExtension(file.FileName).ToLower <> ".csv" Then

                    tblNoRecordsFound.Rows.Clear()
                    trRecordsNotInserted.Style.Add("display", "")

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "", , , "100px", , , "20px", "TagMeta_LeftBox")
                    AddCell(noResultRow, "Invalid File", , , , , , "20px", "TagMeta_Box")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                Else

                    ds = UpdateMetaInfoForTags(hdnTagSiteId, ReadFile(file), file.FileName)

                    If Not ds Is Nothing Then

                        dtResult = ds.Tables(0)
                        dtNoResult = ds.Tables(1)

                        If Not dtResult Is Nothing Then
                            If dtResult.Rows.Count > 0 Then
                                tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                                tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")
                            End If
                        End If

                        tblNoRecordsFound.Rows.Clear()

                        'Header
                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                        AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                        tblNoRecordsFound.Rows.Add(noResultRow)

                        If Not dtNoResult Is Nothing Then
                            If dtNoResult.Rows.Count > 0 Then

                                trRecordsNotInserted.Style.Add("display", "")

                                For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                    With dtNoResult.Rows(nIdx)

                                        noResultRow = New HtmlTableRow()
                                        AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                        AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                        tblNoRecordsFound.Rows.Add(noResultRow)

                                    End With
                                Next
                            End If
                        End If
                    End If

                End If

            ElseIf hdnMapCmd = "AddRoom" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnRoomSiteId As String = Request.Form("hdnRoomSiteId")
                Dim hdnCampusId As String = Request.Form("hdnCampusId")
                Dim hdnBuildingId As String = Request.Form("hdnBuildingId")
                Dim hdnFloorId As String = Request.Form("hdnFloorId")
                Dim hdnUnitId As String = Request.Form("hdnUnitId")
                Dim file As HttpPostedFile = Request.Files("roomcsv")

                ds = UpdateMetaInfoForRooms(hdnRoomSiteId, hdnCampusId, hdnBuildingId, hdnFloorId, hdnUnitId, ReadFile(file), file.FileName)

                If Not ds Is Nothing Then

                    dtResult = ds.Tables(0)
                    dtNoResult = ds.Tables(1)

                    If Not dtResult Is Nothing Then
                        If dtResult.Rows.Count > 0 Then
                            tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                            tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")
                        End If
                    End If

                    tblNoRecordsFound.Rows.Clear()

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    If Not dtNoResult Is Nothing Then
                        If dtNoResult.Rows.Count > 0 Then
                            trRecordsNotInserted.Style.Add("display", "")
                            For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                With dtNoResult.Rows(nIdx)

                                    noResultRow = New HtmlTableRow()
                                    AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                    AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                    tblNoRecordsFound.Rows.Add(noResultRow)

                                End With
                            Next
                        End If
                    End If
                End If
            ElseIf hdnMapCmd = "AddDeviceMetaInfo" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnMetaSiteId As String = Request.Form("hdnMetaSiteId")
                Dim hdnMetaFloorId As String = Request.Form("hdnMetaFloorId")
                Dim file As HttpPostedFile = Request.Files("metacsv")

                ds = UpdateMetaInfoForDevices(hdnMetaSiteId, hdnMetaFloorId, ReadFile(file), file.FileName)

                If Not ds Is Nothing Then
                    dtResult = ds.Tables(0)
                    dtNoResult = ds.Tables(1)

                    If Not dtResult Is Nothing Then
                        If dtResult.Rows.Count > 0 Then

                            tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                            tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")

                        End If
                    End If

                    tblNoRecordsFound.Rows.Clear()

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    If Not dtNoResult Is Nothing Then
                        If dtNoResult.Rows.Count > 0 Then

                            trRecordsNotInserted.Style.Add("display", "")

                            For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                With dtNoResult.Rows(nIdx)

                                    noResultRow = New HtmlTableRow()
                                    AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                    AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                    tblNoRecordsFound.Rows.Add(noResultRow)

                                End With
                            Next
                        End If
                    End If
                End If
                'BATTERY TECH
            ElseIf hdnMapCmd = "AddLbiListForBatteryTech" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnlbiSiteId As String = Request.Form("hdnLbiSiteId")
                Dim file As HttpPostedFile = Request.Files("lbicsv")

                If Path.GetExtension(file.FileName).ToLower <> ".csv" Then

                    tblNoRecordsFound.Rows.Clear()
                    trRecordsNotInserted.Style.Add("display", "")

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "", , , "100px", , , "20px", "TagMeta_LeftBox")
                    AddCell(noResultRow, "Invalid File", , , , , , "20px", "TagMeta_Box")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                Else

                    ds = UpdateLbiListForBatteryTech(hdnlbiSiteId, ReadFile(file), file.FileName)

                    If Not ds Is Nothing Then

                        dtResult = ds.Tables(0)
                        dtNoResult = ds.Tables(1)

                        If Not dtResult Is Nothing Then
                            If dtResult.Rows.Count > 0 Then

                                tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                                tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")

                            End If
                        End If

                        tblNoRecordsFound.Rows.Clear()

                        'Header
                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "TagId", , , "100px", , , "20px", "TagMeta_HeaderBox")
                        AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                        tblNoRecordsFound.Rows.Add(noResultRow)

                        If Not dtNoResult Is Nothing Then
                            If dtNoResult.Rows.Count > 0 Then

                                trRecordsNotInserted.Style.Add("display", "")

                                For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                    With dtNoResult.Rows(nIdx)

                                        noResultRow = New HtmlTableRow()
                                        AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                        AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                        tblNoRecordsFound.Rows.Add(noResultRow)

                                    End With
                                Next
                            End If
                        End If

                    End If
                End If
            ElseIf hdnMapCmd = "AddFloorForBatteryTech" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnBatteryTechSiteId As String = Request.Form("hdnBatteryTechSiteId")
                Dim file As HttpPostedFile = Request.Files("batteryTechcsv")

                If Path.GetExtension(file.FileName).ToLower <> ".xlsx" Then

                    tblNoRecordsFound.Rows.Clear()
                    trRecordsNotInserted.Style.Add("display", "")

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "", , , "100px", , , "20px", "TagMeta_LeftBox")
                    AddCell(noResultRow, "Invalid File", , , , , , "20px", "TagMeta_Box")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    Try
                        PageVisitDetails(g_UserId, "Import Monitor Location", enumPageAction.Edit, "for site - " & Request.Form("SiteId"))
                    Catch ex As Exception
                        WriteLog("UploadFile -Import Monitor Location - UserId " & g_UserId & ex.Message.ToString())
                    End Try

                Else

                    ds = UpdateFloorForBatteryTech(hdnBatteryTechSiteId, ReadFile(file), file.FileName)

                    trErrorRecords.Style.Add("display", "none")

                    If Not ds Is Nothing Then

                        dtResult = ds.Tables(0)
                        dtNoResult = ds.Tables(1)

                        If Not dtResult Is Nothing Then
                            If dtResult.Rows.Count > 0 Then

                                tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                                tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")

                            End If
                        End If

                        tblNoRecordsFound.Rows.Clear()

                        'Header
                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                        AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                        tblNoRecordsFound.Rows.Add(noResultRow)

                        If Not dtNoResult Is Nothing Then
                            If dtNoResult.Rows.Count > 0 Then

                                trRecordsNotInserted.Style.Add("display", "")

                                For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                    With dtNoResult.Rows(nIdx)

                                        noResultRow = New HtmlTableRow()
                                        AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                        AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                        tblNoRecordsFound.Rows.Add(noResultRow)

                                    End With
                                Next
                            End If
                        End If
                    End If
                End If
            ElseIf hdnMapCmd = "UploadG2TempData" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnSiteId As String = Request.Form("hdnBatteryTechSiteId")
                Dim file As HttpPostedFile = Request.Files("G2TempFile")

                ds = UploadG2SummaryInfo(hdnSiteId, ReadFile(file), file.FileName)

                If Not ds Is Nothing Then

                    dtResult = ds.Tables(0)
                    dtNoResult = ds.Tables(1)

                    If Not dtResult Is Nothing Then
                        If dtResult.Rows.Count > 0 Then

                            If dtResult.Rows(0).Item("Result") = "0" Then

                                tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                                tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")
                                trErrorRecords.Style.Add("display", "none")

                            Else

                                If dtResult.Rows(0).Item("Result") = "3" Then

                                    tblErrorRecords.Rows.Clear()
                                    noResultRow = New HtmlTableRow()

                                    Dim ErrMsg As String = ""
                                    If dtResult.Rows(0).Item("error") = "" Then
                                        ErrMsg = "Invalid File."
                                    Else
                                        ErrMsg = dtResult.Rows(0).Item("error")
                                    End If

                                    AddCell(noResultRow, ErrMsg, , 2, "400px", , , "20px", "Error_HeaderBoxRed")
                                    tblErrorRecords.Rows.Add(noResultRow)
                                    trErrorRecords.Style.Add("display", "")
                                    tdcsvTotalRec.Style.Add("display", "none")
                                    tdcsvTotalRecInserted.Style.Add("display", "none")

                                End If
                            End If

                        End If
                    Else

                        tblErrorRecords.Rows.Clear()

                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "Error in uploading file. Please try again.", , 2, "400px", , , "20px", "TagMeta_HeaderBoxRed")
                        tblErrorRecords.Rows.Add(noResultRow)

                        trErrorRecords.Style.Add("display", "")
                        tdcsvTotalRec.Style.Add("display", "none")
                        tdcsvTotalRecInserted.Style.Add("display", "none")

                        Exit Sub
                    End If

                    tblNoRecordsFound.Rows.Clear()

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    If Not dtNoResult Is Nothing Then
                        If dtNoResult.Rows.Count > 0 Then

                            trRecordsNotInserted.Style.Add("display", "")

                            For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                With dtNoResult.Rows(nIdx)

                                    noResultRow = New HtmlTableRow()
                                    AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                    AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                    tblNoRecordsFound.Rows.Add(noResultRow)

                                End With
                            Next
                        End If
                    End If
                End If

            ElseIf hdnMapCmd = "ImportDisasterRecovery" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnDisasterRecoverySiteId As String = Request.Form("hdnDisasterRecoverySiteId")
                Dim file As HttpPostedFile = Request.Files("DisasterRecoverycsv")

                If Path.GetExtension(file.FileName).ToLower <> ".csv" Then

                    tblNoRecordsFound.Rows.Clear()
                    trRecordsNotInserted.Style.Add("display", "")

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "", , , "100px", , , "20px", "TagMeta_LeftBox")
                    AddCell(noResultRow, "Invalid File", , , , , , "20px", "TagMeta_Box")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    Try
                        PageVisitDetails(g_UserId, "Disaster Recovery", enumPageAction.Edit, "for site - " & Request.Form("SiteId"))
                    Catch ex As Exception
                        WriteLog("UploadFile -Disaster Recovery - UserId " & g_UserId & ex.Message.ToString())
                    End Try

                Else

                    Dim hdnDeviceType As String = Request.Form("hdnDeviceType")
                    Dim Isvalidfile As Boolean

                    Dim FileAsByte As Byte()
                    FileAsByte = ReadFile(file)
                    Isvalidfile = checkCSVValidateEmpty(FileAsByte)

                    If (Isvalidfile = True) Then

                        ds = ImportDisasterRecovery(hdnDisasterRecoverySiteId, FileAsByte, file.FileName)

                        trErrorRecords.Style.Add("display", "none")

                        If Not ds Is Nothing Then

                            dtResult = ds.Tables(0)
                            dtNoResult = ds.Tables(1)

                            If Not dtResult Is Nothing Then
                                If dtResult.Rows.Count > 0 Then

                                    tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                                    tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")

                                End If
                            End If

                            tblNoRecordsFound.Rows.Clear()

                            'Header
                            noResultRow = New HtmlTableRow()
                            AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                            AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                            tblNoRecordsFound.Rows.Add(noResultRow)

                            If Not dtNoResult Is Nothing Then
                                If dtNoResult.Rows.Count > 0 Then

                                    trRecordsNotInserted.Style.Add("display", "")

                                    For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                        With dtNoResult.Rows(nIdx)

                                            noResultRow = New HtmlTableRow()
                                            AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                            AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                            tblNoRecordsFound.Rows.Add(noResultRow)

                                        End With
                                    Next
                                End If
                            End If

                        End If
                    Else

                        tblNoRecordsFound.Rows.Clear()
                        trRecordsNotInserted.Style.Add("display", "")

                        'Header
                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                        AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                        tblNoRecordsFound.Rows.Add(noResultRow)

                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "", , , "100px", , , "20px", "TagMeta_LeftBox")
                        AddCell(noResultRow, "No records found", , , , , , "20px", "TagMeta_Box")
                        tblNoRecordsFound.Rows.Add(noResultRow)

                        Try
                            PageVisitDetails(g_UserId, "Disaster Recovery", enumPageAction.Edit, "for site - " & Request.Form("SiteId"))
                        Catch ex As Exception
                            WriteLog("UploadFile -Disaster Recovery - UserId " & g_UserId & ex.Message.ToString())
                        End Try

                    End If
                End If

            ElseIf hdnMapCmd = "ImportDeviceLocations" Then

                divTagMetaSummary.Style.Add("display", "")
                trRecordsNotInserted.Style.Add("display", "none")

                Dim hdnBatteryTechSiteId As String = Request.Form("hdnBatteryTechSiteId")
                Dim file As HttpPostedFile = Request.Files("DeviceLocationFile")
                Dim hdnDeviceType As String = Request.Form("hdnDeviceType")

                If Path.GetExtension(file.FileName).ToLower <> ".csv" Then

                    tblNoRecordsFound.Rows.Clear()
                    trRecordsNotInserted.Style.Add("display", "")

                    'Header
                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                    AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    noResultRow = New HtmlTableRow()
                    AddCell(noResultRow, "", , , "100px", , , "20px", "TagMeta_LeftBox")
                    AddCell(noResultRow, "Invalid File", , , , , , "20px", "TagMeta_Box")
                    tblNoRecordsFound.Rows.Add(noResultRow)

                    Try
                        PageVisitDetails(g_UserId, "Import Device Location", enumPageAction.Edit, "for site - " & Request.Form("SiteId"))
                    Catch ex As Exception
                        WriteLog("UploadFile -Import Device Location - UserId " & g_UserId & ex.Message.ToString())
                    End Try

                Else

                    ds = UpdateDeviceLocationsForSite(hdnBatteryTechSiteId, ReadFile(file), file.FileName, hdnDeviceType)

                    trErrorRecords.Style.Add("display", "none")

                    If Not ds Is Nothing Then

                        dtResult = ds.Tables(0)
                        dtNoResult = ds.Tables(1)

                        If Not dtResult Is Nothing Then
                            If dtResult.Rows.Count > 0 Then
                                tdcsvTotalRec.InnerHtml = dtResult.Rows(0).Item("csvTotalRec")
                                tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0).Item("csvTotalRecInserted")
                            End If
                        End If

                        tblNoRecordsFound.Rows.Clear()

                        'Header
                        noResultRow = New HtmlTableRow()
                        AddCell(noResultRow, "#", , , "100px", , , "20px", "TagMeta_HeaderBox")
                        AddCell(noResultRow, "Reason", , , , , , "20px", "TagMeta_HeaderBox")
                        tblNoRecordsFound.Rows.Add(noResultRow)

                        If Not dtNoResult Is Nothing Then
                            If dtNoResult.Rows.Count > 0 Then

                                trRecordsNotInserted.Style.Add("display", "")

                                For nIdx As Integer = 0 To dtNoResult.Rows.Count - 1
                                    With dtNoResult.Rows(nIdx)

                                        noResultRow = New HtmlTableRow()
                                        AddCell(noResultRow, .Item("num"), , , , , , "20px", "TagMeta_LeftBox")
                                        AddCell(noResultRow, .Item("Reason"), , , , , , "20px", "TagMeta_Box")
                                        tblNoRecordsFound.Rows.Add(noResultRow)

                                    End With
                                Next
                            End If
                        End If
                    End If

                End If

            End If

        End Sub
	
        Private Function checkCSVValidateEmpty(ByVal tfile As Byte()) As Boolean
	
            Dim IsValid As Boolean
            Dim csvTotalRecords As Integer = 0
            Dim csvTotalRecordsInstered As Integer = 0
            Dim SplitRows As String()
            Dim SplitDataArr() As String
            Dim SplitData As String = ""
            Dim csvContent As String = ""
            Dim noofrow As Int32
            IsValid = False
            csvContent = System.Text.Encoding.UTF8.GetString(tfile, 0, tfile.Length)

            SplitRows = csvContent.Split(vbCrLf)

            ' csvTotalRecords = SplitRows.Length - 1
            If SplitRows.Length > 1 Then
                noofrow = SplitRows.Length - 1
                For id As Integer = 0 To noofrow - 1
                    If (id > 0) Then

                        SplitData = SplitRows(id)
                        SplitDataArr = SplitData.Split(",")
                        If SplitDataArr.Length < 2 Then Continue For
                        If SplitDataArr.Length > 0 Then
                            For idx As Integer = 0 To SplitDataArr.Length - 1
                                If idx = 1 Then
                                    Exit For
                                Else
                                    If Val(SplitDataArr(idx)) = 0 Then

                                        If SplitDataArr(idx).ToLower <> "" Or Not IsDBNull(SplitDataArr(idx)) Then
                                            If (SplitDataArr(idx) <> "" & vbLf & "") Then
                                                csvTotalRecords = csvTotalRecords + 1
                                            End If
                                        End If
                                    End If

                                End If
                            Next
                        End If
                    End If
                Next
            End If
	    
            If (csvTotalRecords > 0) Then
                IsValid = True
            End If
	    
            Return IsValid
	    
        End Function
	
        Private Function ReadFile(ByVal file As HttpPostedFile) As Byte()
	
            Dim data As Byte() = New [Byte](file.ContentLength - 1) {}
            file.InputStream.Read(data, 0, file.ContentLength)
	    
            Return data
	    
        End Function
	
    End Class
End Namespace
