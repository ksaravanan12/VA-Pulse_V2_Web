Imports System.IO
Imports System.Diagnostics
Imports System.Web
Imports System.Data
Imports System.Web.HttpContext
Imports System.Net.Mail
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports WebSupergoo.ABCpdf11
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Namespace GMSUI

    Public Module WPSGeneral

        Function WPSDownloadLogsBySiteAndType(ByVal SiteId As String, ByVal type As Integer, ByVal findDate As Date) As String

            Dim sSiteURL As String = "https://gmsdata.centrak.com/httplog"
            Dim sServerLogPath, sServerFolderName, sLogFileName, sLocalRARFilePath, sLocalLogFileName, sLocalLogFilePath, sLocalDirectory, sExtension As String
            Dim dtFolderDt As New DataTable

            sLocalRARFilePath = ""
            sLocalLogFilePath = ""
            sServerFolderName = ""
            sLocalDirectory = ""
            sLogFileName = ""
            sExtension = ""
            sServerLogPath = ""
            sLocalLogFileName = ""
            Try
                If SiteId > 0 And g_UserId > 0 Then

                    ' get site folder name
                    dtFolderDt = DownloadLogFile(SiteId)

                    If Not dtFolderDt Is Nothing Then
                        If dtFolderDt.Rows.Count > 0 Then
                            sServerFolderName = CheckIsDBNull(dtFolderDt.Rows(0).Item("SiteFolder"), False, "")
                        End If
                    End If

                    If type = WPSLOGTYPE.CONTROLLER Then
                        sLogFileName = "CNTRLVALIDLOG" & "-" & findDate.ToString("yyyy-MM-dd")
                        sExtension = "csv"
                    ElseIf type = WPSLOGTYPE.ALERT Then
                        sLogFileName = "GUARDIANALERTLOG" & "-" & findDate.ToString("yyyy-MM-dd")
                        sExtension = "csv"
                    ElseIf type = WPSLOGTYPE.TRANSPORT Then
                        sLogFileName = "TATSVALIDLOG" & "-" & findDate.ToString("yyyy-MM-dd")
                        sExtension = "csv"
                    ElseIf type = WPSLOGTYPE.UISETTINGS Then
                        sLogFileName = "UISETTINGS" & "-" & findDate.ToString("yyyy-MM-dd")
                        sExtension = "ini"
                    ElseIf type = WPSLOGTYPE.SYSTEMSTATUS Then
                        sLogFileName = "UISETTINGS" & "-" & findDate.ToString("yyyy-MM-dd")
                        sExtension = "ini"
                    ElseIf type = WPSLOGTYPE.UILOG Then
                        sLogFileName = "WPSUILOG" & "-" & findDate.ToString("yyyy-MM-dd")
                        sExtension = "dat"
                    End If

                    sServerLogPath = sSiteURL & "/" & sServerFolderName & "/" & sLogFileName & ".rar"

                    sLocalDirectory = GetAppPath() & "\LogFiles\" & SiteId.ToString() & "\" & g_UserId.ToString() & "\" & type.ToString()

                    If (Not IO.Directory.Exists(sLocalDirectory)) Then
                        Directory.CreateDirectory(sLocalDirectory)
                    ElseIf (Directory.Exists(sLocalDirectory)) Then
                        ' if any previous dowload files it will delete all the files. Need to display the selected date log files only
                        For Each _file As String In Directory.GetFiles(sLocalDirectory)
                            IO.File.Delete(_file)
                        Next
                    End If
                   
                    If type = WPSLOGTYPE.UILOG Then
                        ' If type is UI Log, need to download 24 hour log files from server
                        Dim hhidx As Integer = 0
                        Dim shhidx As String = ""
                        While hhidx < 24
                            Try
                                If hhidx < 10 Then
                                    shhidx = "0" & hhidx.ToString()
                                Else
                                    shhidx = hhidx.ToString()
                                End If

                                sLogFileName = "WPSUILOG" & "-" & findDate.ToString("yyyy-MM-dd") & "-" & shhidx
                                sServerLogPath = sSiteURL & "/" & sServerFolderName & "/" & sLogFileName & ".rar"
                                sLocalRARFilePath = sLocalDirectory & "\" + sLogFileName & ".rar"

                                'Download log files from server
                                If sLogFileName.Length > 0 Then
                                    Using WC As New WebClient()
                                        WC.DownloadFile(sServerLogPath, sLocalRARFilePath)
                                        WC.Dispose()
                                    End Using
                                End If

                                'Unrar log files
                                If File.Exists(sLocalRARFilePath) Then
                                    doWPSUnrarFile(sLocalDirectory, sLocalRARFilePath)

                                    sLocalLogFilePath = sLocalDirectory

                                End If

                                If Directory.Exists(sLocalLogFilePath) = True Then
                                    'File unrared successfully so delete the rar file
                                    File.Delete(sLocalRARFilePath)
                                Else
                                    sLocalLogFilePath = ""
                                End If

                            Catch hhex As Exception
                            End Try
                            hhidx = hhidx + 1
                        End While
                        

                    Else
                        sLocalRARFilePath = sLocalDirectory & "\" + sLogFileName & ".rar"

                        'Download log files from server
                        If sLogFileName.Length > 0 Then
                            Using WC As New WebClient()
                                WC.DownloadFile(sServerLogPath, sLocalRARFilePath)
                                WC.Dispose()
                            End Using
                        End If

                        'Unrar log files
                        If File.Exists(sLocalRARFilePath) Then
                            doWPSUnrarFile(sLocalDirectory, sLocalRARFilePath)

                            sLocalLogFilePath = sLocalDirectory & "\" + sLogFileName & "." & sExtension

                        End If


                        If File.Exists(sLocalLogFilePath) = True Then
                            'File unrared successfully so delete the rar file
                            File.Delete(sLocalRARFilePath)
                        Else
                            sLocalLogFilePath = ""
                        End If

                    End If

                   

                End If

            Catch ex1 As Exception
                sLocalLogFilePath = ""
                WriteLog("Error WPSDownloadLogsBySiteAndType " & ex1.Message)
            End Try

            Return sLocalLogFilePath
        End Function
        'Private Function doWPSUnrarFile(destinationpath As String, sourcepath As String) As [Boolean]
        '    Try
        '        Dim objRegKey As RegistryKey
        '        Dim ProcessId As Integer = 0
        '        objRegKey = Registry.ClassesRoot.OpenSubKey("WinRAR\Shell\Open\Command")
        '        Dim obj As Object = objRegKey.GetValue("")
        '        Dim objRarPath As String = obj.ToString()
        '        objRarPath = objRarPath.Substring(1, (objRarPath.Length - 7))
        '        objRegKey.Close()
        '        Dim objArguments As String
        '        objArguments = (Convert.ToString((Convert.ToString(" X -o+ " + ChrW(34)) & sourcepath) + ChrW(34) + " " + ChrW(34)) & destinationpath) + "\" + ChrW(34)
        '        'objArguments = " X " & " " & sourcepath & " " + " " + destinationpath

        '        Dim objStartInfo As New ProcessStartInfo("")
        '        objStartInfo.FileName = objRarPath
        '        objStartInfo.Arguments = objArguments
        '        objStartInfo.UseShellExecute = False
        '        objStartInfo.WindowStyle = ProcessWindowStyle.Hidden

        '        Dim objProcess As New Process()
        '        objProcess.StartInfo = objStartInfo
        '        objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        '        objProcess.Start()
        '        ProcessId = objProcess.Id
        '        objProcess.WaitForExit(300000)
        '        objProcess.Close()
        '        objProcess.Dispose()
        '    Catch ex As Exception

        '    End Try


        '    Return True
        'End Function
        Private Function doWPSUnrarFile(destinationpath As String, sourcepath As String) As [Boolean]
            Try
                Dim p As Process = New Process()
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                p.StartInfo.FileName = GetWinRarInstallPathfromRegistry() & "\UnRAR.exe"
                'p.StartInfo.Arguments = "x -ep " & extractsourcePath & "  """ & destFile & """"
                p.StartInfo.Arguments = " x -ep " & Chr(34) & sourcepath & Chr(34) & " " & Chr(34) & destinationpath & Chr(34)
                p.Start()
                p.WaitForExit(300000)
                p.Close()
                p.Dispose()
            Catch ex As Exception
                Dim err As String = ex.Message
            End Try
        End Function
        Private Function GetWinRarInstallPathfromRegistry() As String
            Dim registryContent As String = ""
            Dim registryObj As RegistryKey
            Try
                If Environment.Is64BitProcess = True Then
                    registryObj = Registry.LocalMachine.OpenSubKey("Software\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths\WinRaR.exe")
                Else
                    registryObj = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\App Paths\WinRaR.exe")
                End If

                registryContent = (registryObj.GetValue("Path")).ToString()
            Catch ex As Exception
                registryContent = "C:\Program Files (x86)\WinRAR"
            End Try

            Return registryContent
        End Function
        Function GetTransportTransferLogDataFromCSV(ByVal logFilePath As String) As List(Of TATSModel)

            Dim fulllogtext As String = ""
            Dim UpdatedOn As New Date
            Dim type As New Byte
            Dim message As String = ""
            Dim fullpath As String = ""
            Dim selectedPath As String = ""
            Dim logList As New List(Of TATSModel)
            Dim lgmdl As New TATSModel

            Try
                If logFilePath <> "" Then
                    Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(logFilePath)
                        MyReader.TextFieldType = FileIO.FieldType.Delimited
                        MyReader.SetDelimiters(",")
                        Dim currentRow As String()
                        While Not MyReader.EndOfData
                            Try
                                currentRow = MyReader.ReadFields()
                                If currentRow.Length > 0 And currentRow(0) <> "Idx" Then
                                    lgmdl = New TATSModel
                                    lgmdl.IDX = currentRow(0)
                                    lgmdl.Tag = Convert.ToInt32(currentRow(1))
                                    lgmdl.Type = currentRow(2)
                                    lgmdl.PathControllers = currentRow(3)
                                    lgmdl.DestinationControllers = currentRow(4)
                                    lgmdl.CompletionType = currentRow(5)
                                    lgmdl.Duration = currentRow(6)
                                    lgmdl.CreatedTime = Convert.ToDateTime(currentRow(7))
                                    lgmdl.EndTime = Convert.ToDateTime(currentRow(8))
                                    lgmdl.InitiatedTime = currentRow(9)
                                    lgmdl.Status = currentRow(10)
                                    logList.Add(lgmdl)
                                End If

                            Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException

                            End Try
                        End While
                    End Using
                End If
                
            Catch ex As Exception

            End Try
            Return logList
        End Function
        Function GetControllerLogDataFromCSV(ByVal logFilePath As String) As List(Of ControllerLogModel)

            Dim fulllogtext As String = ""
            Dim UpdatedOn As New Date
            Dim type As New Byte
            Dim message As String = ""
            Dim fullpath As String = ""
            Dim selectedPath As String = ""
            Dim logList As New List(Of ControllerLogModel)
            Dim lgmdl As New ControllerLogModel

            Try
                If logFilePath <> "" Then
                    Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(logFilePath)
                        MyReader.TextFieldType = FileIO.FieldType.Delimited
                        MyReader.SetDelimiters(",")
                        Dim currentRow As String()
                        While Not MyReader.EndOfData
                            Try
                                currentRow = MyReader.ReadFields()
                                If currentRow.Length > 0 And currentRow(0) <> "S.No" Then
                                    lgmdl = New ControllerLogModel
                                    lgmdl.SNO = currentRow(0)
                                    lgmdl.Controller = currentRow(1)
                                    lgmdl.DateTime = Convert.ToDateTime(currentRow(2))
                                    lgmdl.ControllerState = currentRow(3)
                                    lgmdl.TagInFields = currentRow(4)
                                    lgmdl.MagLock = currentRow(5)
                                    lgmdl.DoorStatus = currentRow(6)
                                    lgmdl.Buzzer = currentRow(7)
                                    lgmdl.Relay1 = currentRow(8)
                                    lgmdl.Relay2 = currentRow(9)
                                    lgmdl.Relay3 = currentRow(10)
                                    lgmdl.UIControllerState = currentRow(11)
                                    lgmdl.UIAlert = currentRow(12)
                                    lgmdl.DoorAjar = currentRow(13)
                                    lgmdl.FireAlarmIO = currentRow(14)
                                    lgmdl.EmergencyDoorIO = currentRow(15)
                                    lgmdl.GeneralInput1IO = currentRow(16)
                                    lgmdl.GeneralInput2IO = currentRow(17)
                                    lgmdl.KeyPad1State = currentRow(18)
                                    lgmdl.KeyPad2State = currentRow(19)
                                    lgmdl.KeyCode = currentRow(20)
                                    lgmdl.Validation = currentRow(21)
                                    lgmdl.Message = currentRow(22)
                                    logList.Add(lgmdl)
                                End If

                            Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException

                            End Try
                        End While
                    End Using
                End If
                

            Catch ex As Exception

            End Try
            Return logList
        End Function
        Function GetUILogDataFromCSV(ByVal logFilePath As String) As List(Of LogModel)

            Dim fulllogtext As String = ""
            Dim UpdatedOn As New Date
            Dim type As New Byte
            Dim message As String = ""
            Dim fullpath As String = ""
            Dim selectedPath As String = ""
            Dim logList As New List(Of LogModel)

            Try
                If logFilePath <> "" Then
                    Dim filePaths() As String
                    filePaths = System.IO.Directory.GetFiles(logFilePath, "*.dat")
                    For Each singlelog In filePaths
                        Dim fulllogbytes As Byte() = File.ReadAllBytes(singlelog)
                        Dim nByteIdx As Integer = 0
                        While nByteIdx < fulllogbytes.Length
                            Try
                                Dim uUpdatedOn As UInteger = System.BitConverter.ToUInt32(fulllogbytes, nByteIdx)
                                nByteIdx = nByteIdx + 4
                                UpdatedOn = New DateTime(1970, 1, 1).AddSeconds(uUpdatedOn)

                                type = Convert.ToByte(fulllogbytes(nByteIdx))
                                nByteIdx = nByteIdx + 1

                                Dim datalength As Integer = Convert.ToUInt16(System.BitConverter.ToUInt16(fulllogbytes, nByteIdx))
                                nByteIdx = nByteIdx + 2

                                message = System.Text.Encoding.ASCII.GetString(fulllogbytes, nByteIdx, datalength)

                                'If type = 11 Then
                                '    logUIAdminList.Add(New LogModel() With {.Updatedon = UpdatedOn, .TypeId = type, .Log = message})
                                '    nByteIdx = nByteIdx + datalength
                                'Else

                                logList.Add(New LogModel() With {.Updatedon = UpdatedOn, .TypeId = type, .Log = message})
                                nByteIdx = nByteIdx + datalength
                                'End If

                            Catch ex As Exception
                                ex.Message.ToString()
                            End Try
                        End While
                    Next
                End If


            Catch ex As Exception

            End Try
            Return logList
        End Function
        Function GetUIAlertDataFromCSV(ByVal logFilePath As String) As List(Of AlertModel)

            Dim fulllogtext As String = ""
            Dim UpdatedOn As New Date
            Dim type As New Byte
            Dim message As String = ""
            Dim fullpath As String = ""
            Dim selectedPath As String = ""
            Dim lgmdl As New AlertModel
            Dim logList As New List(Of AlertModel)

            Try
                If logFilePath <> "" Then
                    Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(logFilePath)
                        MyReader.TextFieldType = FileIO.FieldType.Delimited
                        MyReader.SetDelimiters(",")
                        Dim currentRow As String()
                        While Not MyReader.EndOfData
                            Try
                                currentRow = MyReader.ReadFields()
                                If currentRow.Length > 0 And currentRow(0) <> "LOG Id" Then
                                    lgmdl = New AlertModel
                                    lgmdl.LOGID = Convert.ToInt32(currentRow(0))
                                    lgmdl.LogType = currentRow(1)
                                    lgmdl.DateTime = Convert.ToDateTime(currentRow(2))
                                    lgmdl.Events = currentRow(3)
                                    lgmdl.AlertType = currentRow(4)
                                    lgmdl.Tag = currentRow(5)
                                    lgmdl.Device = currentRow(6)
                                    lgmdl.Annotation = currentRow(7)
                                    lgmdl.ActionBy = currentRow(8)
                                    logList.Add(lgmdl)
                                End If

                            Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException

                            End Try
                        End While
                    End Using
                End If
                
      
            Catch ex As Exception

            End Try
            Return logList
        End Function
        Function GetUISystemStatusDataFromINI(ByVal logFilePath As String) As List(Of SystemStatusModel)
            Dim fullpath As String = ""
            Dim statusCnt As Integer = 20
            Dim prevContent As String = ""
            Dim currentContent As String = ""
            Dim systemStatusList As New List(Of SystemStatusModel)
            Dim systemStatus As New SystemStatusModel
            Try
                If logFilePath <> "" Then
                    Dim ini As New IniFile(logFilePath)
                    'statusCnt = CInt(ini.GetString("SYSTEMSTATUS", "STATUSCOUNT", ""))
                    For i As Integer = 0 To statusCnt
                        Dim text As String = ini.GetString("SYSTEMSTATUS", "SYSTEMID_" & i, "")
                        If Not text Is Nothing Then
                            Dim array As String() = text.Split(";".ToCharArray())

                            systemStatus = New SystemStatusModel
                            systemStatus.Type = array(0).ToString()
                            systemStatus.SubType = array(1).ToString()
                            systemStatus.Description = array(2).ToString()
                            systemStatus.Status = array(3).ToString()
                            Dim textUIVersion As String = ini.GetString("SYSTEMSTATUS", "GuardianVersion", "")
                            systemStatus.UIVersion = textUIVersion
                            systemStatusList.Add(systemStatus)

                        End If
                    Next
                End If


            Catch ex As Exception

            End Try
            Return systemStatusList
        End Function
        Function GetControllersFromINI(ByVal logFilePath As String) As List(Of String)
            Dim fullpath As String = ""
            Dim controllerCnt As Integer = 300
            Dim prevContent As String = ""
            Dim currentContent As String = ""
            Dim controllerList As New List(Of String)
            Dim controller As String = ""
            Try
                If logFilePath <> "" Then
                    Dim ini As New IniFile(logFilePath)
                    'controllerCnt = CInt(ini.GetString("SYSTEMSTATUS", "CONTROLLERCOUNT", ""))
                    For i As Integer = 1 To controllerCnt
                        Dim text As String = ini.GetString("CONTROLLERINFO", "CONTROLLER_" & i, "")
                        If Not text Is Nothing Then
                            Dim array As String() = text.Split(";".ToCharArray())
                            controller = array(0).ToString()
                            controllerList.Add(controller)
                        Else
                            Exit For
                        End If
                    Next
                End If


            Catch ex As Exception

            End Try
            Return controllerList
        End Function
        Function GetAccessSettingsByControllerFromINI(ByVal logFilePath As String, ByVal controller As Integer) As AccessSettingModel
            Dim settingsLogMdl As New AccessSettingModel
            Dim controllerCnt As Integer = 300
            Dim securityInfoCnt As Integer = 100
            Dim nAccessId As Integer = 0
            Dim cntrller As Integer = 0
            Dim nAccessLevelInfoCnt As Integer = 0
            Dim nEscortInfoCnt As Integer = 0
            Dim nAssetInfoCnt As Integer = 0
            Dim nOtherInfoCnt As Integer = 0
            Dim staffMdl As New AccessLevelInfo
            Dim patientMdl As New AccessLevelInfo
            Dim undefinedMdl As New AccessLevelInfo
            Dim ioMdl As New AccessLevelInfo
            Dim escortMdl As New EscortInfo
            Dim assetMdl As New AssetInfo
            Dim otherMdl As New SecurityInfo
            Try
                If logFilePath <> "" Then
                    Dim ini As New IniFile(logFilePath)
                    For i As Integer = 1 To controllerCnt
                        Dim text As String = ini.GetString("CONTROLLERINFO", "CONTROLLER_" & i, "")
                        If Not text Is Nothing Then
                            Dim array As String() = text.Split(";".ToCharArray())
                            cntrller = Convert.ToInt32(array(0).ToString())
                            If cntrller = controller Then
                                nAccessId = array(1).ToString()
                                Exit For
                            End If

                        End If
                    Next
                    If nAccessId > 0 Then
                        settingsLogMdl.patientlst = New List(Of AccessLevelInfo)
                        settingsLogMdl.stafflist = New List(Of AccessLevelInfo)
                        settingsLogMdl.undefinedlst = New List(Of AccessLevelInfo)
                        settingsLogMdl.iolst = New AccessLevelInfo
                        nAccessLevelInfoCnt = CInt(ini.GetString("ACCESSSETTINGS", "ACCESSINFO_" & nAccessId & "_COUNT", ""))



                        For accesslevelidx As Integer = 1 To nAccessLevelInfoCnt


                            Dim sStaffContent As String = ini.GetString("ACCESSSETTINGS", "ACCESS_INFO_" & nAccessId & "_IDX_" & accesslevelidx, "")
                            If Not sStaffContent Is Nothing Then
                                Dim accesslevelArray As String() = sStaffContent.Split(",".ToCharArray())

                                If Convert.ToInt32(accesslevelArray(0).ToString()) = 1 Then 'patient 
                                    patientMdl = New AccessLevelInfo
                                    patientMdl.RiskClearanceId = Convert.ToInt32(accesslevelArray(1).ToString())
                                    patientMdl.DoorLock = Convert.ToInt32(accesslevelArray(2).ToString())
                                    patientMdl.Buzzer = Convert.ToInt32(accesslevelArray(3).ToString())
                                    settingsLogMdl.patientlst.Add(patientMdl)
                                ElseIf Convert.ToInt32(accesslevelArray(0).ToString()) = 2 Then 'staff 
                                    staffMdl = New AccessLevelInfo
                                    staffMdl.RiskClearanceId = Convert.ToInt32(accesslevelArray(1).ToString())
                                    staffMdl.DoorLock = Convert.ToInt32(accesslevelArray(2).ToString())
                                    staffMdl.Buzzer = Convert.ToInt32(accesslevelArray(3).ToString())
                                    staffMdl.Egress = Convert.ToInt32(accesslevelArray(4).ToString())
                                    settingsLogMdl.stafflist.Add(staffMdl)
                                ElseIf Convert.ToInt32(accesslevelArray(0).ToString()) = 4 Then 'undefined 
                                    undefinedMdl = New AccessLevelInfo
                                    undefinedMdl.TypeId = Convert.ToInt32(accesslevelArray(1).ToString())
                                    undefinedMdl.DoorLock = Convert.ToInt32(accesslevelArray(2).ToString())
                                    undefinedMdl.Buzzer = Convert.ToInt32(accesslevelArray(3).ToString())
                                    undefinedMdl.Egress = Convert.ToInt32(accesslevelArray(4).ToString())
                                    settingsLogMdl.undefinedlst.Add(undefinedMdl)
                                ElseIf Convert.ToInt32(accesslevelArray(0).ToString()) = 7 Then 'IO
                                    ioMdl = New AccessLevelInfo
                                    ioMdl.RiskClearanceId = Convert.ToInt32(accesslevelArray(1).ToString())
                                    ioMdl.DoorLock = Convert.ToInt32(accesslevelArray(2).ToString())
                                    ioMdl.Buzzer = Convert.ToInt32(accesslevelArray(4).ToString())
                                    ioMdl.ISOOneFireOnOff = Convert.ToInt32(accesslevelArray(6).ToString())
                                    ioMdl.ISOOneDoorAjarOnOff = Convert.ToInt32(accesslevelArray(7).ToString())
                                    ioMdl.ISOOneTgOnOff = Convert.ToInt32(accesslevelArray(8).ToString())
                                    ioMdl.ISOOneTgNoBzOnOff = Convert.ToInt32(accesslevelArray(9).ToString())
                                    ioMdl.ISOOneUnauthOnOff = Convert.ToInt32(accesslevelArray(10).ToString())
                                    ioMdl.ISOOneEmergencyOnOff = Convert.ToInt32(accesslevelArray(11).ToString())
                                    ioMdl.ISOOneLoiterOnOff = Convert.ToInt32(accesslevelArray(12).ToString())
                                    ioMdl.ISOOneIdleEgressOnOff = Convert.ToInt32(accesslevelArray(13).ToString())
                                    ioMdl.ISOOneTamperOnOff = Convert.ToInt32(accesslevelArray(14).ToString())
                                    ioMdl.ISOOneLockCmdOnOff = Convert.ToInt32(accesslevelArray(15).ToString())
                                    ioMdl.ISOOneUnLockCmdOnOff = Convert.ToInt32(accesslevelArray(16).ToString())
                                    ioMdl.ISOTwoFireOnOff = Convert.ToInt32(accesslevelArray(17).ToString())
                                    ioMdl.ISOTwoDoorAjarOnOff = Convert.ToInt32(accesslevelArray(18).ToString())
                                    ioMdl.ISOTwoTgOnOff = Convert.ToInt32(accesslevelArray(19).ToString())
                                    ioMdl.ISOTwoTgNoBzOnOff = Convert.ToInt32(accesslevelArray(20).ToString())
                                    ioMdl.ISOTwoUnauthOnOff = Convert.ToInt32(accesslevelArray(21).ToString())
                                    ioMdl.ISOTwoEmergencyOnOff = Convert.ToInt32(accesslevelArray(22).ToString())
                                    ioMdl.ISOTwoLoiterOnOff = Convert.ToInt32(accesslevelArray(23).ToString())
                                    ioMdl.ISOTwoIdleEgressOnOff = Convert.ToInt32(accesslevelArray(24).ToString())
                                    ioMdl.ISOTwoTamperOnOff = Convert.ToInt32(accesslevelArray(25).ToString())
                                    ioMdl.ISOTwoLockCmdOnOff = Convert.ToInt32(accesslevelArray(26).ToString())
                                    ioMdl.ISOTwoUnLockCmdOnOff = Convert.ToInt32(accesslevelArray(27).ToString())
                                    ioMdl.ISOThreeFireOnOff = Convert.ToInt32(accesslevelArray(28).ToString())
                                    ioMdl.ISOThreeDoorAjarOnOff = Convert.ToInt32(accesslevelArray(29).ToString())
                                    ioMdl.ISOThreeTgOnOff = Convert.ToInt32(accesslevelArray(30).ToString())
                                    ioMdl.ISOThreeTgNoBzOnOff = Convert.ToInt32(accesslevelArray(31).ToString())
                                    ioMdl.ISOThreeUnauthOnOff = Convert.ToInt32(accesslevelArray(32).ToString())
                                    ioMdl.ISOThreeEmergencyOnOff = Convert.ToInt32(accesslevelArray(33).ToString())
                                    ioMdl.ISOThreeLoiterOnOff = Convert.ToInt32(accesslevelArray(34).ToString())
                                    ioMdl.ISOThreeIdleEgressOnOff = Convert.ToInt32(accesslevelArray(35).ToString())
                                    ioMdl.ISOThreeTamperOnOff = Convert.ToInt32(accesslevelArray(36).ToString())
                                    ioMdl.ISOThreeLockCmdOnOff = Convert.ToInt32(accesslevelArray(37).ToString())
                                    ioMdl.ISOThreeUnLockCmdOnOff = Convert.ToInt32(accesslevelArray(38).ToString())
                                    ioMdl.GeneralIO1 = Convert.ToInt32(accesslevelArray(39).ToString())
                                    ioMdl.IO1_EgressConditional = Convert.ToInt32(accesslevelArray(40).ToString())
                                    ioMdl.IO1_Conditional = Convert.ToInt32(accesslevelArray(41).ToString())
                                    ioMdl.GeneralIO2 = Convert.ToInt32(accesslevelArray(42).ToString())
                                    ioMdl.IO2_EgressConditional = Convert.ToInt32(accesslevelArray(43).ToString())
                                    ioMdl.IO2_Conditional = Convert.ToInt32(accesslevelArray(44).ToString())
                                    ioMdl.ISOOneText = accesslevelArray(45).ToString()
                                    ioMdl.ISOTwoText = accesslevelArray(46).ToString()
                                    ioMdl.ISOThreeText = accesslevelArray(47).ToString()
                                    ioMdl.EgressBtnCmb = Convert.ToInt32(accesslevelArray(48).ToString())
                                    settingsLogMdl.iolst = ioMdl
                                End If
                            End If
                        Next


                        'escort
                        settingsLogMdl.escortlst = New List(Of EscortInfo)

                        nEscortInfoCnt = CInt(ini.GetString("ACCESSSETTINGS", "ESCORTINFO_" & nAccessId & "_COUNT", ""))
                        For escortlevelidx As Integer = 1 To nEscortInfoCnt
                            escortMdl = New EscortInfo
                            Dim sEscortContent As String = ini.GetString("ACCESSSETTINGS", "ESCORT_INFO_" & nAccessId & "_IDX_" & escortlevelidx, "")
                            If Not sEscortContent Is Nothing Then
                                Dim escortlevelArray As String() = sEscortContent.Split(",".ToCharArray())
                                escortMdl.EscortTypeId = Convert.ToInt32(escortlevelArray(0).ToString())
                                escortMdl.OptionId = Convert.ToInt32(escortlevelArray(1).ToString())
                                escortMdl.CustomPatientLevel = Convert.ToInt32(escortlevelArray(2).ToString())
                                escortMdl.CustomStaffLevel = Convert.ToInt32(escortlevelArray(3).ToString())
                                escortMdl.DoorLock = Convert.ToInt32(escortlevelArray(4).ToString())
                                escortMdl.SystemTimeout = Convert.ToInt32(escortlevelArray(5).ToString())
                                escortMdl.Buzzer = Convert.ToInt32(escortlevelArray(6).ToString())
                                escortMdl.Egress = Convert.ToInt32(escortlevelArray(7).ToString())
                                settingsLogMdl.escortlst.Add(escortMdl)
                            End If
                        Next
                        'asset
                        settingsLogMdl.asssetinf = New AssetInfo

                        nAssetInfoCnt = CInt(ini.GetString("ACCESSSETTINGS", "ASSETINFO_" & nAccessId & "_COUNT", ""))
                        For assetlevelidx As Integer = 1 To nAssetInfoCnt
                            assetMdl = New AssetInfo
                            Dim sAssetContent As String = ini.GetString("ACCESSSETTINGS", "ASSET_INFO_" & nAccessId & "_IDX_" & assetlevelidx, "")
                            If Not sAssetContent Is Nothing Then
                                Dim assetlevelArray As String() = sAssetContent.Split(",".ToCharArray())
                                assetMdl.DoorLock = Convert.ToInt32(assetlevelArray(0).ToString())
                                assetMdl.SystemTimeout = Convert.ToInt32(assetlevelArray(1).ToString())
                                assetMdl.Buzzer = Convert.ToInt32(assetlevelArray(2).ToString())
                                assetMdl.Egress = assetlevelArray(3).ToString()
                                assetMdl.StaffEscort = Convert.ToBoolean(assetlevelArray(4).ToString())
                                assetMdl.PatientEscort = Convert.ToBoolean(assetlevelArray(5).ToString())
                                settingsLogMdl.asssetinf = assetMdl
                            End If
                        Next
                        'Other
                        settingsLogMdl.secinf = New SecurityInfo

                        Dim sAccessId As String = ""
                        For j As Integer = 1 To securityInfoCnt
                            otherMdl = New SecurityInfo
                            Dim sOtherContent As String = ini.GetString("SECURITY", "SECURITY_" & j, "")
                            If Not sOtherContent Is Nothing Then
                                Dim array As String() = sOtherContent.Split(",".ToCharArray())
                                '33 column (index starts from 0) for accessid
                                sAccessId = Convert.ToInt32(array(33).ToString())
                                If nAccessId = Convert.ToInt32(sAccessId) Then
                                    Dim otherlevelArray As String() = sOtherContent.Split(",".ToCharArray())
                                    otherMdl.GlobalKeyCode = If(otherlevelArray(0).ToString() = "null", "", otherlevelArray(0).ToString())
                                    otherMdl.LDoorLock = If(otherlevelArray(1).ToString() = "null", 0, Integer.Parse(otherlevelArray(1).ToString()))
                                    otherMdl.LSystemTimeout = If(otherlevelArray(2).ToString() = "null", 0, Integer.Parse(otherlevelArray(2).ToString()))
                                    otherMdl.LBuzzer = If(otherlevelArray(3).ToString() = "null", 0, Integer.Parse(otherlevelArray(3).ToString()))
                                    otherMdl.LEgress = If(otherlevelArray(4).ToString() = "null", 0, Integer.Parse(otherlevelArray(4).ToString()))
                                    otherMdl.ULDoorLock = If(otherlevelArray(5).ToString() = "null", 0, Integer.Parse(otherlevelArray(5).ToString()))
                                    otherMdl.ULSystemTimeout = If(otherlevelArray(6).ToString() = "null", 0, Integer.Parse(otherlevelArray(6).ToString()))
                                    otherMdl.ULBuzzer = If(otherlevelArray(7).ToString() = "null", 0, Integer.Parse(otherlevelArray(7).ToString()))
                                    otherMdl.ULEgress = If(otherlevelArray(8).ToString() = "null", 0, Integer.Parse(otherlevelArray(8).ToString()))
                                    otherMdl.TDoorLock = If(otherlevelArray(9).ToString() = "null", 0, Integer.Parse(otherlevelArray(9).ToString()))
                                    otherMdl.TSystemTimeout = If(otherlevelArray(10).ToString() = "null", 0, Integer.Parse(otherlevelArray(10).ToString()))
                                    otherMdl.TBuzzer = If(otherlevelArray(11).ToString() = "null", 0, Integer.Parse(otherlevelArray(11).ToString()))
                                    otherMdl.TEgress = If(otherlevelArray(12).ToString() = "null", 0, Integer.Parse(otherlevelArray(12).ToString()))
                                    otherMdl.FDoorLock = If(otherlevelArray(13).ToString() = "null", 0, Integer.Parse(otherlevelArray(13).ToString()))
                                    otherMdl.FSystemTimeout = If(otherlevelArray(14).ToString() = "null", 0, Integer.Parse(otherlevelArray(14).ToString()))
                                    otherMdl.FBuzzer = If(otherlevelArray(15).ToString() = "null", 0, Integer.Parse(otherlevelArray(15).ToString()))
                                    otherMdl.FEgress = If(otherlevelArray(16).ToString() = "null", 0, Integer.Parse(otherlevelArray(16).ToString()))
                                    otherMdl.LoiterTimeout = If(otherlevelArray(17).ToString() = "null", 0, Integer.Parse(otherlevelArray(17).ToString()))
                                    otherMdl.DoorAjarTimeout = If(otherlevelArray(18).ToString() = "null", 0, Integer.Parse(otherlevelArray(18).ToString()))
                                    otherMdl.KeyCodeButtonPressTimeout = If(otherlevelArray(19).ToString() = "null", 0, Integer.Parse(otherlevelArray(19).ToString()))
                                    otherMdl.EmergencyWaitTime = If(otherlevelArray(20).ToString() = "null", 0, Integer.Parse(otherlevelArray(20).ToString()))
                                    otherMdl.UnauthorizedEgressAlertBuzzer = If(otherlevelArray(21).ToString() = "null", 0, Integer.Parse(otherlevelArray(21).ToString()))
                                    otherMdl.UnauthorizedEgressAlertType = If(otherlevelArray(22).ToString() = "null", 0, Integer.Parse(otherlevelArray(22).ToString()))
                                    otherMdl.EmergencyType = If(otherlevelArray(23).ToString() = "null", 0, Integer.Parse(otherlevelArray(23).ToString()))
                                    otherMdl.TagRelinquishedTimer = If(otherlevelArray(24).ToString() = "null", 0, Integer.Parse(otherlevelArray(24).ToString()))
                                    otherMdl.IsButtonPress = If(otherlevelArray(25).ToString() = "null", 0, Integer.Parse(otherlevelArray(25).ToString()))
                                    otherMdl.DoorAjarBuzzer = If(otherlevelArray(26).ToString() = "null", 0, Integer.Parse(otherlevelArray(26).ToString()))
                                    otherMdl.UnResponsiveTamperTag = If(otherlevelArray(27).ToString() = "null", 0, Integer.Parse(otherlevelArray(27).ToString()))
                                    otherMdl.SensorTamperTag = If(otherlevelArray(28).ToString() = "null", 0, Integer.Parse(otherlevelArray(28).ToString()))
                                    otherMdl.IsEnableIpCamera = If(otherlevelArray(29).ToString() = "null", 0, Integer.Parse(otherlevelArray(29).ToString()))
                                    otherMdl.UnresponsiveWaitTime = If(otherlevelArray(30).ToString() = "null", 0, Integer.Parse(otherlevelArray(30).ToString()))
                                    otherMdl.AlertTimeout = If(otherlevelArray(31).ToString() = "null", 0, Integer.Parse(otherlevelArray(31).ToString()))
                                    otherMdl.LoiterBlinkAndChrip = If(otherlevelArray(32).ToString() = "null", 0, Integer.Parse(otherlevelArray(32).ToString()))
                                    otherMdl.AccessId = If(otherlevelArray(33).ToString() = "null", 0, Integer.Parse(otherlevelArray(33).ToString()))
                                    otherMdl.KeycodeStaffRequire = If(otherlevelArray(34).ToString() = "null", 0, Integer.Parse(otherlevelArray(34).ToString()))
                                    otherMdl.TransportWrngTime = If(otherlevelArray(35).ToString() = "null", 0, Integer.Parse(otherlevelArray(35).ToString()))
                                    otherMdl.TransferWrngTime = If(otherlevelArray(36).ToString() = "null", 0, Integer.Parse(otherlevelArray(36).ToString()))
                                    otherMdl.TamperWrngTime = If(otherlevelArray(37).ToString() = "null", 0, Integer.Parse(otherlevelArray(37).ToString()))
                                    otherMdl.UnresponsiveHeartBeatTime = If(otherlevelArray(38).ToString() = "null", 0, Integer.Parse(otherlevelArray(38).ToString()))
                                    otherMdl.EnableUnresponsiveDeviceAlert = If(otherlevelArray(39).ToString() = "null", 0, Integer.Parse(otherlevelArray(39).ToString()))
                                    otherMdl.UnresponsiveDeviceTimeMin = If(otherlevelArray(40).ToString() = "null", 0, Integer.Parse(otherlevelArray(40).ToString()))
                                    otherMdl.MaxAttempt = If(otherlevelArray(41).ToString() = "null", 0, Integer.Parse(otherlevelArray(41).ToString()))
                                    otherMdl.StaffDuressBtns = If(otherlevelArray(42).ToString() = "null", 0, Integer.Parse(otherlevelArray(42).ToString()))
                                    otherMdl.UnresponsiveStarInMin = If(otherlevelArray(43).ToString() = "null", 0, Integer.Parse(otherlevelArray(43).ToString()))
                                    otherMdl.UnresponsiveControllerInMin = If(otherlevelArray(44).ToString() = "null", 0, Integer.Parse(otherlevelArray(44).ToString()))
                                    otherMdl.TagLooseUnitType = If(otherlevelArray(45).ToString() = "null", 0, Integer.Parse(otherlevelArray(45).ToString()))
                                    otherMdl.StaffDuressType = If(otherlevelArray(46).ToString() = "null", 0, Integer.Parse(otherlevelArray(46).ToString()))
                                    otherMdl.StaffTagRelinquishedTimer = If(otherlevelArray(47).ToString() = "null", 0, Integer.Parse(otherlevelArray(47).ToString()))
                                    otherMdl.LoiterAlarmBlinkAndChrip = If(otherlevelArray(48).ToString() = "null", 0, Integer.Parse(otherlevelArray(48).ToString()))
                                    settingsLogMdl.secinf = otherMdl
                                    Exit For
                                End If

                            End If
                        Next

                    End If
                End If
            Catch ex As Exception
                ex.Message.ToString()
            End Try
            Return settingsLogMdl
        End Function
    End Module
End Namespace
