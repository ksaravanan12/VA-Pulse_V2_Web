Imports System.IO
Imports System.Data

Namespace GMSUI
    Partial Class RTLSimport
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Dim ds As New DataSet()
            Dim dtResult As New DataTable()
            Dim dtNoResult As New DataTable()

            Dim apicall As New GmsAPICall()

            Dim noResultRow As HtmlTableRow = Nothing

            Dim ResponseError As String = ""
            Dim Command As String = Request.Form("hdnCmd")

            If Command = "ImportTemperatureMetaInfo" Then

                Dim file As HttpPostedFile = Request.Files("TemperatureMetaExcel")
                Dim SiteId As String = Request.Form("hdnTemperatureMetaSiteId")


                If Request.QueryString("isSubmit") = "1" Then
                    SiteId = Request.QueryString("SiteId")
                End If

                divTagMetaSummary.Style.Add("display", "")
                trErrorRecords.Style.Add("display", "none")
                trWarningRecords.Style.Add("display", "none")

                Try

                    ds = UploadRTLSDetails(SiteId, ReadFile(file), file.FileName)

                Catch ex As Exception
                    WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
                End Try



                If (ds IsNot Nothing) Then

                    dtResult = ds.Tables(0)
                    dtNoResult = ds.Tables(1)


                    If (dtResult IsNot Nothing) Then
                        If dtResult.Rows.Count > 0 Then

                            tdcsvTotalRecInserted.InnerHtml = dtResult.Rows(0)("csvTotalRecInserted").ToString()
                            trcsvTotalRec.Style.Add("display", "")
                            trcsvTotalRecInserted.Style.Add("display", "")

                            trTotalErrors.Style.Add("display", "none")
                            trTotalWarnings.Style.Add("display", "none")


                            If dtResult.Rows(0)("Result").ToString() = "3" Then
                                tblErrorRecords.Rows.Clear()
                                noResultRow = New HtmlTableRow()

                                Dim ErrMsg As String = ""
                                If String.IsNullOrEmpty(dtResult.Rows(0)("error").ToString()) Then
                                    ErrMsg = "Invalid File."
                                Else
                                    ErrMsg = dtResult.Rows(0)("error").ToString()
                                End If
                                AddCell(noResultRow, ErrMsg, , , 200, , "top", " 20px", "TagMeta_HeaderBoxRed")
                                tblErrorRecords.Rows.Add(noResultRow)
                                trErrorRecords.Style.Add("display", "")
                                trcsvTotalRec.Style.Add("display", "none")
                                trcsvTotalRecInserted.Style.Add("display", "none")
                            End If
                        End If
                    Else
                        tblErrorRecords.Rows.Clear()
                        noResultRow = New HtmlTableRow()
                        General.AddCell(noResultRow, "Error in uploading file. Please try again.", "", "2", "400px", "", _
                         "", "20px", "TagMeta_HeaderBoxRed", "", "", False, _
                         0)
                        tblErrorRecords.Rows.Add(noResultRow)
                        trErrorRecords.Style.Add("display", "")
                        trcsvTotalRec.Style.Add("display", "none")
                        trcsvTotalRecInserted.Style.Add("display", "none")
                        Return
                    End If
                End If
            End If

        End Sub
        Private Function ReadFile(ByVal file As HttpPostedFile) As Byte()
            Dim data As Byte() = New [Byte](file.ContentLength - 1) {}
            file.InputStream.Read(data, 0, file.ContentLength)
            Return data
        End Function
    End Class
End Namespace