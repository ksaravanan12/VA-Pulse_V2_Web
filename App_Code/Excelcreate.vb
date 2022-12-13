Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Data
Imports System.IO
Imports System
Imports System.Web.HttpContext
Namespace GMSUI
    Public Class Excelcreate
        Public Function ExcelHeader()

            Dim sb As New StringBuilder()
            sb.Append("<?xml version='1.0'?>\n")
            sb.Append("<?mso-application progid='Excel.Sheet'?>\n")
            sb.Append("<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet' ")
            sb.Append("xmlns:o='urn:schemas-microsoft-com:office:office' ")
            sb.Append("xmlns:x='urn:schemas-microsoft-com:office:excel' ")
            sb.Append("xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' ")
            sb.Append("xmlns:html='http://www.w3.org/TR/REC-html40'>\n")
            sb.Append("<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>\n")
            sb.Append("<Author></Author>\n")
            sb.Append("<LastAuthor> </LastAuthor>\n")
            sb.Append("<Created> </Created>\n")
            sb.Append("<Version> </Version>\n")
            sb.Append("</DocumentProperties>\n")
            sb.Append("<ExcelWorkbook xmlns='urn:schemas-microsoft-com:office:excel'>\n")
            sb.Append("<ProtectStructure>False</ProtectStructure>\n")
            sb.Append("<ProtectWindows>False</ProtectWindows>\n")
            sb.Append("</ExcelWorkbook>\n")
            sb.Append("<Styles>\n<Style ss:ID='s21'>\n<Font x:Family='Swiss' ss:Bold='1'/>\n</Style>\n<Style ss:ID='s23'>\n<Alignment ss:Horizontal='Right' ss:Vertical='Bottom'/>\n</Style>\n<Style ss:ID='s24'>\n<Alignment ss:Horizontal='Right' ss:Vertical='Bottom'/>/n<Font x:Family='Swiss' ss:Bold='1'/>\n</Style>\n<Style ss:ID='s26'>\n<Alignment ss:Horizontal='Center' ss:Vertical='Bottom'/>/n<Font x:Family='Swiss' ss:Bold='1'/>\n</Style>\n<Style ss:ID='s28'>\n<Alignment ss:Horizontal='Left' ss:Vertical='Bottom' ss:WrapText='1'/>\n</Style>\n</Styles>\n")
            Return sb.ToString()

        End Function
        Public Function ExcelWorkSheetOptions()

            Dim sb As New StringBuilder()
            sb.Append("\n<WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'>\n<Selected/>\n </WorksheetOptions>\n")
            Return sb.ToString()
        End Function
        Public Function ExcelFooter()

            Dim sb As New StringBuilder()
            sb.Append("</Table>")
            sb.Append("</Worksheet>")
            Return sb.ToString()

        End Function
        Public Function ExcelFooterBookClose()

            Dim sb As New StringBuilder()
            sb.Append("</Workbook>\n")
            Return sb.ToString()

        End Function
        Public Sub loadHeader(ByVal Csvtext As StringBuilder, ByVal dt As DataTable)

            Dim sColName As String = ""
            Dim nInc As Integer = 0

            For nInc = 0 To dt.Columns.Count - 1
                sColName = (dt.Columns(nInc).ColumnName)
                AddExcelCell(Csvtext, sColName, False, False, True)
            Next
        End Sub
        Public Function addDataToTable_CSV(ByVal Csvtext As StringBuilder, ByVal dt As DataTable) As StringBuilder

            Dim sColName As String = ""
            Dim sColValue As String = ""
            Dim nInc As Integer = 0
            Dim nIdx As Integer = 0
            Try
                If Not IsDBNull(dt) Then
                    If dt.Rows.Count > 0 Then
                        For nInc = 0 To dt.Rows.Count - 1
                            AddExcelCell(Csvtext, "", True, False)
                            For nIdx = 0 To dt.Columns.Count - 1
                                sColName = dt.Columns(nIdx).ColumnName
                                sColValue = dt.Rows(nInc).Item(sColName).ToString()
                                If sColName <> "TimeStamp" And sColName <> "FileInfo" Then
                                    AddExcelCell(Csvtext, sColValue)
                                End If

                            Next
                            AddExcelCell(Csvtext, "", False, True)
                        Next

                    End If
                End If
            Catch ex As Exception

            End Try

            Return Csvtext
        End Function
        Public Function ConvertHTMLToExcelXML(ByVal strHtml As String) As String

            'Just to replace TR with Row
            strHtml = strHtml.Replace("<tr>", "<Row ss:AutoFitHeight='1' >\n")
            strHtml = strHtml.Replace("</tr>", "</Row>\n")

            'Replace the cell tags
            strHtml = strHtml.Replace("<td>", "<Cell><Data ss:Type='String'>")
            strHtml = strHtml.Replace("</td>", "</Data></Cell>\n")
            Return strHtml
        End Function
        Public Function ExcelScriptReplace(ByVal sScript As String) As String

            Dim sScriptNew As String = ""
            sScriptNew = sScript
            sScriptNew = sScriptNew.Replace("\n", System.Environment.NewLine)
            Return sScriptNew
        End Function
        Public Sub ExcelFileGen(ByVal sScript As String, ByVal sFilename As String)
            'File.WriteAllText("C:\\blah.xls", sScript.ToString())
            Try
                Current.Response.Clear()
                Current.Response.ContentType = "application/vnd.ms-excel"
                Current.Response.AddHeader("content-disposition", "attachment; filename=" & sFilename)
                Current.Response.AddHeader("content-length", sScript.Length.ToString())
                Current.Response.Write(sScript.ToString())
                Current.Response.End()
            Catch ex As Exception
                WriteLog(" Exception ExcelFileGen " & ex.Message.ToString())
            End Try

        End Sub
        Public Sub AddExcelCell(ByRef Csvtext As StringBuilder, Optional ByVal sName As String = "", Optional ByVal bOpenTr As Boolean = False, Optional ByVal bCloseTr As Boolean = False, Optional ByVal bHeading As Boolean = False, Optional ByVal nColSpan As Integer = 0, Optional ByVal Align As String = "Left", Optional ByVal sDataType As String = "String")
            Dim temp As String
            If bOpenTr = True Then
                Csvtext.Append("<tr>")
                Exit Sub
            End If
            If bCloseTr = True Then
                Csvtext.Append("</tr>")
                Exit Sub
            End If

            If bHeading Then
                Csvtext.Append("<Cell ss:MergeAcross='" & nColSpan & "' ss:StyleID='s21'><Data ss:Type='String'>" & sName & "</Data></Cell>\n")
            ElseIf Align = "right" Then
                Csvtext.Append("<Cell ss:StyleID='s23'><Data ss:Type='" & sDataType & "'>" & sName & "</Data></Cell>\n")
            ElseIf Align = "rightBold" Then
                Csvtext.Append("<Cell ss:MergeAcross='" & nColSpan & "' ss:StyleID='s24'><Data ss:Type='" & sDataType & "'>" & sName & "</Data></Cell>\n")
            ElseIf Align = "Center" Then
                Csvtext.Append("<Cell ss:MergeAcross='" & nColSpan & "' ss:StyleID='s26'><Data ss:Type='" & sDataType & "'>" & sName & "</Data></Cell>\n")
            ElseIf Align = "Comma" And StrContainsComma(sName) Then
                temp = sName.Replace(",", "  ")
                Csvtext.Append("<Cell ss:MergeAcross='" & nColSpan & "' ss:StyleID='s28'><Data ss:Type='String'>" & temp & "</Data></Cell>\n")
            ElseIf Align = "break" Then
                sName = Trim(sName)
                temp = sName.Replace("<br>", "&#10;")
                temp = sName.Replace(vbCrLf, "&#10;")
                Csvtext.Append("<Cell ss:MergeAcross='" & nColSpan & "' ss:StyleID='s28'><Data ss:Type='String'>" & temp & "</Data></Cell>\n")

            Else
                'Csvtext.Append("<td>")
                Csvtext.Append("<Cell><Data ss:Type='" & sDataType & "'>" & sName & "</Data></Cell>\n")
                'Csvtext.Append(sName)
                'Csvtext.Append("</td>")
            End If
        End Sub
        Public Sub InitiateCSV(ByRef CSVtext As System.Web.HttpContext, ByVal sFileName As String)

            sFileName = sFileName.Replace(",", "")

            CSVtext.Response.Clear()
            CSVtext.Response.ContentType = "text/csv"
            sFileName = Replace(sFileName, " ", "_")
            CSVtext.Response.AddHeader("content-disposition", "attachment; filename=" & sFileName & ".CSV")
        End Sub
        Public Sub AddCSVCell(ByRef CSVtext As System.Web.HttpContext, ByVal stext As String, Optional ByVal bColSep As Boolean = False)
            Dim sInfo As String
            stext = Replace(stext, "<br>", ",")
            stext = Replace(stext, "<BR>", ",")
            sInfo = Chr(34) & stext & Chr(34)
            CSVtext.Response.Write(sInfo)
            If bColSep Then CSVtext.Response.Write(",")
        End Sub
        Public Sub AddCSVNewLine(ByRef CSVtext As System.Web.HttpContext)
            CSVtext.Response.Write(vbNewLine)
        End Sub
    End Class

End Namespace
