Imports System.IO
Imports System.Data
Imports System.Data.OleDb

Namespace GMSUI
    Public Class Excel

        Sub CreateHeader(ByVal sTitle As String, ByRef act As StringBuilder)
            act.Append("<html xmlns:x=""urn:schemas-microsoft-com:office:excel"">")
            act.Append("<head>")
            act.Append("<!--[if gte mso 9]><xml>")
            act.Append("<x:ExcelWorkbook>")
            act.Append("<x:ExcelWorksheets>")
            act.Append("<x:ExcelWorksheet>")
            act.Append("<x:Name>" & sTitle & "</x:Name>")
            act.Append("<x:WorksheetOptions>")
            act.Append("<x:Print>")
            act.Append("<x:ValidPrinterInfo/>")
            act.Append("</x:Print>")
            act.Append("</x:WorksheetOptions>")
            act.Append("</x:ExcelWorksheet>")
            act.Append("</x:ExcelWorksheets>")
            act.Append("</x:ExcelWorkbook>")
            act.Append("</xml>")
            act.Append("<![endif]--> ")
            act.Append("</head>")
            act.Append("<body>")
        End Sub

        Sub CreateWorkSheetDatas(ByRef act As StringBuilder)
            act.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
        End Sub

        Sub InsertData(ByVal Val As String, ByRef act As StringBuilder, Optional ByVal IsBold As Boolean = False, Optional ByVal IsImg As Boolean = False, Optional ByVal nColspan As Integer = 1, Optional ByVal nImgHeight As Integer = 0, Optional ByVal nImgWidth As Integer = 0)
            Dim nHht As Integer

            If IsImg Then
                If nImgHeight > 0 Then
                    nHht = nImgHeight / 16.5
                End If

                act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif;'>")
                act.Append("<td width=" & nImgWidth & " rowspan=" & nHht & " align='left'>")
                act.Append("<Img src=" & Val & " border=0 align='left'>")
                act.Append("</td>")
                act.Append("</tr>")
            Else
                If IsBold Then
                    act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-weight:bold; text-decoration:underline; background-color:#CCFFFF;'>")
                Else
                    act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif;'>")
                End If

                act.Append("<td colspan=" & nColspan & ">")
                act.Append(Val & "&nbsp;")
                act.Append("</td>")
                act.Append("</tr>")
            End If
        End Sub

        Sub InsertDataArray(ByVal Val() As String, ByRef act As StringBuilder, Optional ByVal IsBold As Boolean = False, Optional ByVal Colspan As Integer = 1)
            Dim Idx As Integer

            If IsBold Then
                act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-size: 12px; font-weight:bold; color:#646464; height:25px; vertical-align:middle;'>")
            Else
                act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-size: 11px;'>")
            End If

            For Idx = 0 To Val.Length - 1
                act.Append("<td colspan='" & Colspan & "'>")
                'act.Append(Val(Idx) & "&nbsp;")
                act.Append(Val(Idx))
                act.Append("</td>")
            Next

            act.Append("</tr>")
        End Sub

        Sub InsertDataHeader(ByVal Val As String, ByRef act As StringBuilder, Optional ByVal IsBold As Boolean = False, Optional ByVal Colspan As Integer = 1, Optional ByVal Rowspan As Integer = 1)
            If IsBold Then
                act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-size: 16px; color: #1a1a1a; height:30px; vertical-align:bottom; font-weight:bold;'>")
            Else
                act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-size: 16px; color: #1a1a1a; height:30px; vertical-align:bottom;'>")
            End If

            act.Append("<td colspan='" & Colspan & "' rowspan='" & Rowspan & "'>")
            act.Append(Val & "&nbsp;")
            act.Append("</td>")
            act.Append("</tr>")
        End Sub

        Sub InsertRowHeader(ByVal Val As String, ByRef act As StringBuilder, Optional ByVal IsBold As Boolean = False, Optional ByVal Colspan As Integer = 1, Optional ByVal Rowspan As Integer = 1, Optional IsFirstCol As Boolean = False, Optional IsLastCol As Boolean = False, Optional width As String = "")

            If IsFirstCol Then
                If IsBold Then
                    act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-size: 12px; color: #1a1a1a; height:30px; vertical-align:bottom; font-weight:bold;'>")
                Else
                    act.Append("<tr style='font-family:Helvetica,MyriadPro,Verdana,Arial,sans-serif; font-size: 12px; color: #1a1a1a; height:30px; vertical-align:bottom;'>")
                End If
            End If

            act.Append("<td colspan='" & Colspan & "' rowspan='" & Rowspan & "' style='width:" & width & "'>")
            act.Append(Val & "&nbsp;")
            act.Append("</td>")

            If IsLastCol Then
                act.Append("</tr>")
            End If

        End Sub

        Sub CloseWorkSheetDatas(ByRef act As StringBuilder)
            act.Append("</table>")
        End Sub

        Sub CloseHeader(ByRef act As StringBuilder)
            act.Append("</body>")
            act.Append("</html>")
        End Sub

        Sub DownloadExcel(ByVal Fname As String, ByVal act As StringBuilder)
          
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.Buffer = True
            HttpContext.Current.Response.ContentType = "application/ms-excel"
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & Fname & ".xls")
            HttpContext.Current.Response.Write(act.ToString())
            HttpContext.Current.Response.End()
        End Sub

#Region "CreateExcelUsingTemplate"

        Public Shared Sub DownloadExcelFile(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal Bin As String)

            Dim distFileName As String, sourceFile As String
            Dim dtDeviceList As New DataTable()
            Dim ReplaceSiteName As String = ""
            Dim SiteName As String = ""

            Try

                dtDeviceList = LoadDeviceList(SiteId, DeviceType, TypeId, "", "0", "", Bin, "LastSeen", "desc")
                dtDeviceList.TableName = "DeviceList"

                If Not dtDeviceList Is Nothing Then
                    If dtDeviceList.Rows.Count > 0 Then
                        SiteName = CheckIsDBNull(dtDeviceList.Rows(0).Item("SiteName"), , "")
                    End If
                End If

                'Check Folder
                Dim saveFolder As String = System.Web.Hosting.HostingEnvironment.MapPath("~/Excel/Download_Excel/")

                For Each f In Directory.GetFiles(saveFolder)
                    File.Delete(f)
                Next

                If Not Directory.Exists(saveFolder) Then
                    Directory.CreateDirectory(saveFolder)
                End If

                ReplaceSiteName = SiteName.Replace(", ", "_")
                ReplaceSiteName = ReplaceSiteName.Replace(" - ", "-")
                ReplaceSiteName = ReplaceSiteName.Replace(",", "_")
                ReplaceSiteName = ReplaceSiteName.Replace(" ", "_")

                distFileName = saveFolder + "\\MASTER-G1-G2-Probe-Swap-" + ReplaceSiteName + "_" + DateTime.Now.ToString("MMddyyyy") + ".xlsx"

                If File.Exists(distFileName) Then
                    File.Delete(distFileName)
                End If

                Dim RelativePath As String = MapPathReverse(distFileName)

                sourceFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Excel/Templates") + "\DeviceList_Template.xlsx"

                File.Copy(sourceFile, distFileName)

                'Write Excel
                WriteExcel_DeviceList(dtDeviceList, distFileName)

                'DownLoad
                DownloadFile(distFileName)

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadExcelFile " + SiteId + " : " & ex.Message.ToString())
            End Try

        End Sub
        Public Shared Sub WriteExcel_DeviceList(dt As DataTable, strFileName As String)

            Dim strQuery As String = ""
            Dim SheetName As String = "Temp Certification & Probe Swap$"

            Dim dtColumn As New DataTable()

            Dim dtTemp As New DataTable()
            Dim myCommand As New System.Data.OleDb.OleDbCommand()

            Try

                Dim sQuery As String = (Convert.ToString("Select * from [") & SheetName) + "]"

                Dim ConnectionString As String = (Convert.ToString("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=") & strFileName) + ";Extended Properties=""Excel 12.0 Xml;HDR=YES;"""

                Dim ExcelConnection As New OleDbConnection(ConnectionString)
                ExcelConnection.Open()

                strQuery = (Convert.ToString("INSERT INTO [") & SheetName) + "] ([Tag ID],[OLD Probe '1' ID],[OLD Probe '2' ID],[Location_1],[Location_2],[NIST Frequency (months)],[Calibration Date (mm/dd/yyyy)]) Values([@TagId],[@ProbeId],[@ProbeId2],[@LocalId],[@Location],[@CalFrequency],[@CertificateDate])"

                Dim OleAdp As New OleDbDataAdapter(sQuery, ExcelConnection)
                OleAdp.Fill(dtTemp)

                OleAdp.InsertCommand = New System.Data.OleDb.OleDbCommand(strQuery)

                OleAdp.InsertCommand.Parameters.Add("[@TagId]", OleDbType.VarChar, 8000, "TagId")
                OleAdp.InsertCommand.Parameters.Add("[@ProbeId]", OleDbType.VarChar, 8000, "ProbeId")
                OleAdp.InsertCommand.Parameters.Add("[@ProbeId2]", OleDbType.VarChar, 8000, "ProbeId2")
                OleAdp.InsertCommand.Parameters.Add("[@LocalId]", OleDbType.VarChar, 8000, "LocalId")
                OleAdp.InsertCommand.Parameters.Add("[@Location]", OleDbType.VarChar, 8000, "Location")
                OleAdp.InsertCommand.Parameters.Add("[@CalFrequency]", OleDbType.VarChar, 8000, "CalFrequency")
                OleAdp.InsertCommand.Parameters.Add("[@CertificateDate]", OleDbType.VarChar, 8000, "CertificateDate")

                OleAdp.InsertCommand.Connection = ExcelConnection

                OleAdp.Update(dt)
                ExcelConnection.Close()

            Catch ex As Exception
                WriteLog(" WriteExcel_DeviceList : " & ex.Message.ToString())
            End Try

        End Sub

        Public Shared Function MapPathReverse(ByVal fullServerPath As String) As String
            Dim applicationPath As String = System.Web.Hosting.HostingEnvironment.MapPath("~/")
            Dim baseurl As String = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath

            Dim url As String = baseurl & fullServerPath.Substring(applicationPath.Length).Replace("\"c, "/"c).Insert(0, "/")

            Return (url)
        End Function

        Public Shared Sub DownloadFile(filePath As String)

            Dim name As String = Path.GetFileName(filePath)
            Dim ext As String = Path.GetExtension(filePath)
            Dim type As String = "application/vnd.ms-excel"

            Try

                HttpContext.Current.Response.AppendHeader("content-disposition", Convert.ToString("attachment; filename=") & name)

                If type <> "" Then

                    HttpContext.Current.Response.ContentType = type
                    HttpContext.Current.Response.WriteFile(filePath)
                    HttpContext.Current.Response.[End]()

                End If

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadFile : " & ex.Message.ToString())
            End Try

        End Sub

#End Region

    End Class
End Namespace
