Imports System.Data
Imports System.Net.Sockets

Namespace GMSUI
    Partial Class INIEditor
        Inherits System.Web.UI.Page

        Dim centrak_guid As String
        Dim IPAddress As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.TechnicalAdmin Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                LoadIRRXProfile()
            End If

        End Sub
        Sub LoadIRRXProfile()

            Try
                selIRRXProfile.Items.Clear()

                For nidx As Integer = 0 To 20
                    selIRRXProfile.Items.Add(New ListItem(nidx, nidx))
                Next

            Catch ex As Exception
                WriteLog(" LoadIRRXProfile " & ex.Message.ToString())
            End Try

        End Sub

        Sub GetGuid(ByVal SiteId As Integer)

            Dim dt As New DataTable

            Try
                'dt = GetQuidBySite(SiteId)

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        centrak_guid = dt.Rows(0).Item("Quid")
                        IPAddress = dt.Rows(0).Item("IPAddress")
                    End If
                End If

            Catch ex As Exception
                WriteLog(" GetGuid " & ex.Message.ToString())
            End Try

        End Sub

        Protected Shared Sub SendCommandToPcServer(ByVal server As String, ByVal message As String, ByRef sResponse As String)
            Dim port As Int32 = 8185
            Dim client As TcpClient = New TcpClient(server, port)
            Dim data() As Byte = System.Text.Encoding.ASCII.GetBytes(message)

            Dim bytes As Int32

            Dim stream As NetworkStream = client.GetStream()

            Try
                'Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length)

                ' Buffer to store the response bytes.
                data = New Byte(100000) {}

                bytes = stream.Read(data, 0, data.Length)
                sResponse = System.Text.Encoding.ASCII.GetString(data, 0, bytes)

                stream.Close()
                client.Close()

            Catch ex As Exception
                WriteLog("SendCommandToPcServer - " & ex.Message)
            End Try

        End Sub

        Protected Shared Function GetProfileFromPcServer(ByVal server As String, ByVal message As String) As String
            Dim port As Int32 = 8185
            Dim client As TcpClient = New TcpClient(server, port)
            Dim data() As Byte = System.Text.Encoding.ASCII.GetBytes(message)

            Dim bytes As Int32
            Dim sResponse As String = ""

            Dim stream As NetworkStream = client.GetStream()

            Try
                'Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length)

                ' Buffer to store the response bytes.
                data = New Byte(100000) {}

                bytes = stream.Read(data, 0, data.Length)
                sResponse = System.Text.Encoding.ASCII.GetString(data, 0, bytes)

                stream.Close()
                client.Close()

            Catch ex As Exception
                WriteLog("GetProfileFromPcServer - " & ex.Message)
            End Try

            Return sResponse

        End Function
        Protected Sub btnReceive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReceive.Click

            Dim txtTagIds As String = txtTagId.Value
            Dim SiteId As Integer = hdnSiteId.Value
            Dim dt As New DataTable

            lblErrormsg.InnerText = ""
            lblNoTagInINI.InnerText = ""

            Try

                If SiteId > 0 Then
                    'Get Profile Data
                    GetProfileData(SiteId, txtTagIds)
                End If

            Catch ex As Exception
                WriteLog("btnReceive_Click - " & ex.Message)
            End Try

        End Sub

        Sub GetProfileData(ByVal SiteId As Integer, ByVal TagId As String)
            Dim dt As New DataTable
            Dim sProfileData As String
            Dim sResponse As String = ""
            Dim Type As Integer = 0

            'Get Guid
            GetGuid(SiteId)

            sProfileData = "gettagprofile {" & _
                            """centrak_guid"":""" & centrak_guid & """," & _
                            """device_id"":""" & TagId & """ " & _
                            "}" & _
                            "}"

            'Get Profile From PC Server
            sResponse = GetProfileFromPcServer(IPAddress, sProfileData)

            Try

                If sResponse.Contains("errorcode") Then
                    dt = ConvertJSONToDataTable(sResponse)

                    If Not dt Is Nothing Then
                        If dt.Rows.Count > 0 Then

                            lblNoTagInINI.InnerText = dt.Rows(0).Item("errormessage")

                            'Check If Id is not defined in INI.
                            If dt.Rows(0).Item("errormessage").contains("not defined in in") Then

                                dt = GetDefaultProfiles(SiteId, enumDeviceType.Tag, TagId)

                                If Not dt Is Nothing Then
                                    If dt.Rows.Count > 0 Then
                                        Type = dt.Rows(0).Item("TagType")

                                        If Not Type = enumDeviceCategory.AssetandPatientTag And Not Type = enumDeviceCategory.G1TempTag And Not Type = enumDeviceCategory.G2TempTag And Not Type = enumDeviceCategory.HumidityTag Then
                                            lblNoTagInINI.InnerText = "Profile configuration only support for  Multi-Mode Asset Tag..!"
                                            trProfileSettings.Style.Add("display", "none")
                                            Return
                                        End If

                                        'Hide Functions
                                        funHide(Type)

                                        hdnTagType.Value = Type
                                    Else
                                        hdnTagType.Value = Type
                                    End If
                                Else
                                    hdnTagType.Value = Type
                                End If

                                hdnIsDefaultProfile.Value = True

                                'VA Default Profile

                                selTagCategory.Value = 2
                                selIRProfile.Value = 0
                                selIRReportRate.Value = 2
                                selRFReportRate.Value = 2
                                selIRRXProfile.Value = 3
                                chkFastPushbtn.Checked = False
                                selDisableLF.Value = 0
                                selOperatingMode.Value = 1
                                selWiFiReporting.Value = 5
                                chkWifi900MHz.Checked = True
                                selPagingProfile.Value = 3
                                selLFRange.Value = 0
                                selLongIR.Value = 1
                                selUpdateRate.Value = 1

                            Else

                                lblNoTagInINI.InnerText = dt.Rows(0).Item("errormessage")
                                trProfileSettings.Style.Add("display", "none")
                                Return

                            End If

                        End If
                    End If
                Else

                    dt = ConvertJSONToDataTable(sResponse)
                    lblNoTagInINI.InnerText = ""
                    hdnIsDefaultProfile.Value = False

                    If Not dt Is Nothing Then
                        If dt.Rows.Count > 0 Then
                            For nIdx As Integer = 0 To dt.Rows.Count - 1
                                With dt.Rows(nIdx)

                                    'Hide Functions
                                    funHide(.Item("device_category"))
                                    hdnTagType.Value = .Item("device_category")

                                    If Not .Item("device_category") = enumDeviceCategory.AssetandPatientTag And Not .Item("device_category") = enumDeviceCategory.G1TempTag And Not .Item("device_category") = enumDeviceCategory.G2TempTag And Not .Item("device_category") = enumDeviceCategory.HumidityTag Then
                                        lblNoTagInINI.InnerText = "Profile configuration only support for  Multi-Mode Asset Tag..!"
                                        trProfileSettings.Style.Add("display", "none")
                                        Return
                                    ElseIf .Item("device_category") = enumDeviceCategory.AssetandPatientTag Then

                                        selIRProfile.Value = .Item("ir_profile")
                                        selIRReportRate.Value = .Item("ir_report_time")
                                        selRFReportRate.Value = .Item("rf_report_time")
                                        selIRRXProfile.Value = .Item("ir_rx_profile")
                                        chkFastPushbtn.Checked = .Item("enable_fpb")
                                        selDisableLF.Value = .Item("disable_lf")
                                        selOperatingMode.Value = .Item("operating_mode")
                                        selWiFiReporting.Value = .Item("wifi_reporting_time")
                                        chkWifi900MHz.Checked = .Item("wifi_in_900mhz")
                                        selPagingProfile.Value = .Item("paging_profile")
                                        selLFRange.Value = .Item("lf_reg_config")

                                        If dt.Columns.Contains("long_ir_open") = True Then
                                            selLongIR.Value = .Item("long_ir_open")
                                        End If

                                    ElseIf .Item("device_category") = enumDeviceCategory.G2TempTag Then

                                        selIRProfile.Value = .Item("ir_profile")
                                        selIRReportRate.Value = .Item("ir_report_time")
                                        selRFReportRate.Value = .Item("rf_report_time")
                                        selIRRXProfile.Value = .Item("ir_rx_profile")
                                        selDisableLF.Value = .Item("disable_lf")
                                        selOperatingMode.Value = .Item("operating_mode")
                                        selWiFiReporting.Value = .Item("wifi_reporting_time")
                                        chkWifi900MHz.Checked = .Item("wifi_in_900mhz")
                                        selPagingProfile.Value = .Item("paging_profile")
                                        selLFRange.Value = .Item("lf_reg_config")

                                        If dt.Columns.Contains("long_ir_open") = True Then
                                            selLongIR.Value = .Item("long_ir_open")
                                        End If

                                        selUpdateRate.Value = .Item("temp_report_rate")

                                    ElseIf .Item("device_category") = enumDeviceCategory.HumidityTag Then

                                        selOperatingMode.Value = .Item("operating_mode")
                                        selPagingProfile.Value = .Item("paging_profile")
                                        selUpdateRate.Value = .Item("measurement_rate")

                                    ElseIf .Item("device_category") = enumDeviceCategory.G1TempTag Then

                                        selOperatingMode.Value = .Item("operating_mode")

                                    End If

                                End With
                            Next
                        End If
                    End If
                End If

                trProfileSettings.Style.Add("display", "")

            Catch ex As Exception
                WriteLog("GetProfileData - " & ex.Message)
            End Try

        End Sub
        Sub funHide(ByVal Type As Integer)

            If Type = enumDeviceCategory.AssetandPatientTag Then

                trUpdateRate.Style.Add("display", "none")

                trIRProfile.Style.Add("display", "")
                trLongIR.Style.Add("display", "")
                trIRReportRate.Style.Add("display", "")
                trRFReportRate.Style.Add("display", "")
                trIRRX.Style.Add("display", "")
                trFastPushButton.Style.Add("display", "")
                trLFRange.Style.Add("display", "")
                trDisableLF.Style.Add("display", "")
                trOperatingMode.Style.Add("display", "")
                trWifiReportingTime.Style.Add("display", "")
                trWIFI.Style.Add("display", "")
                trPagingPrfile.Style.Add("display", "")
                trPagingPrfile.Style.Add("display", "")

            ElseIf Type = enumDeviceCategory.G2TempTag Then

                trFastPushButton.Style.Add("display", "none")

                trUpdateRate.Style.Add("display", "")
                trIRProfile.Style.Add("display", "")
                trLongIR.Style.Add("display", "")
                trIRReportRate.Style.Add("display", "")
                trRFReportRate.Style.Add("display", "")
                trIRRX.Style.Add("display", "")
                trLFRange.Style.Add("display", "")
                trDisableLF.Style.Add("display", "")
                trOperatingMode.Style.Add("display", "")
                trWifiReportingTime.Style.Add("display", "")
                trWIFI.Style.Add("display", "")
                trPagingPrfile.Style.Add("display", "")
                trPagingPrfile.Style.Add("display", "")
            ElseIf Type = enumDeviceCategory.HumidityTag Then

                trOperatingMode.Style.Add("display", "")
                trPagingPrfile.Style.Add("display", "")
                trUpdateRate.Style.Add("display", "")

                trIRProfile.Style.Add("display", "none")
                trLongIR.Style.Add("display", "none")
                trIRReportRate.Style.Add("display", "none")
                trRFReportRate.Style.Add("display", "none")
                trIRRX.Style.Add("display", "none")
                trFastPushButton.Style.Add("display", "none")
                trLFRange.Style.Add("display", "none")
                trDisableLF.Style.Add("display", "none")
                trWifiReportingTime.Style.Add("display", "none")
                trWIFI.Style.Add("display", "none")
            ElseIf Type = enumDeviceCategory.G1TempTag Then
                trOperatingMode.Style.Add("display", "")

                trIRProfile.Style.Add("display", "none")
                trLongIR.Style.Add("display", "none")
                trIRReportRate.Style.Add("display", "none")
                trRFReportRate.Style.Add("display", "none")
                trIRRX.Style.Add("display", "none")
                trFastPushButton.Style.Add("display", "none")
                trLFRange.Style.Add("display", "none")
                trDisableLF.Style.Add("display", "none")
                trWifiReportingTime.Style.Add("display", "none")
                trWIFI.Style.Add("display", "none")
                trPagingPrfile.Style.Add("display", "none")
                trUpdateRate.Style.Add("display", "none")
            End If

        End Sub
        Private Function ConvertJSONToDataTable(ByVal jsonString As String) As DataTable
            Dim dt As New DataTable

            'strip out bad characters
            Dim jsonParts As String() = jsonString.Replace("[", "").Replace("]", "").Split("},{")

            'hold column names
            Dim dtColumns As New ArrayList

            Try

                'get columns
                For Each jp As String In jsonParts
                    'only loop thru once to get column names
                    Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                    For Each rowData As String In propData
                        Try
                            Dim idx As Integer = rowData.IndexOf(":")
                            Dim n As String = rowData.Substring(0, idx - 1)
                            Dim v As String = rowData.Substring(idx + 1)
                            If Not dtColumns.Contains(n) Then
                                dtColumns.Add(n.Replace("""", ""))
                            End If
                        Catch ex As Exception
                            Throw New Exception(String.Format("Error Parsing Column Name : {0}", rowData))
                        End Try

                    Next
                    Exit For
                Next

                'build dt
                For Each c As String In dtColumns
                    dt.Columns.Add(c)
                Next

                'get table data
                For Each jp As String In jsonParts
                    Dim propData As String() = jp.Replace("{", "").Replace("}", "").Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                    Dim nr As DataRow = dt.NewRow
                    For Each rowData As String In propData
                        Try
                            Dim idx As Integer = rowData.IndexOf(":")
                            Dim n As String = rowData.Substring(0, idx - 1).Replace("""", "")
                            Dim v As String = rowData.Substring(idx + 1).Replace("""", "")
                            nr(n) = v
                        Catch ex As Exception
                            Continue For
                        End Try

                    Next

                    If Len(jp) > 0 Then
                        dt.Rows.Add(nr)
                    End If
                Next

            Catch ex As Exception
                WriteLog("ConvertJSONToDataTable - " & ex.Message)
            End Try

            Return dt
        End Function
        Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
            Dim sProfileData As String = ""
            Dim sResponse As String = ""
            Dim SiteId As Integer = hdnSiteId.Value

            Dim dtError As New DataTable

            Dim device_type As Integer = enumDeviceType.Tag
            Dim device_id As Integer = txtTagId.Value
            Dim ir_profile As Integer = selIRProfile.Value
            Dim ir_report_time As Integer = selIRReportRate.Value
            Dim rf_report_time As Integer = selRFReportRate.Value
            Dim ir_rx_profile As Integer = selIRRXProfile.Value
            Dim enable_fpb As Boolean = chkFastPushbtn.Checked
            Dim operating_mode As Integer = selOperatingMode.Value
            Dim wifi_reporting_time As Integer = selWiFiReporting.Value
            Dim wifi_in_900mhz As Boolean = chkWifi900MHz.Checked
            Dim paging_profile As Integer = selPagingProfile.Value
            Dim disable_lf As String = selDisableLF.Value
            Dim lf_reg_config As String = selLFRange.Value
            Dim long_ir_open As String = selLongIR.Value
            Dim temp_report_rate As String = selUpdateRate.Value

            Dim nenable_fpb As Integer = 0
            Dim nwifi_in_900mhz As Integer = 0

            If enable_fpb = True Then
                nenable_fpb = 1
            Else
                nenable_fpb = 0
            End If

            If wifi_in_900mhz = True Then
                nwifi_in_900mhz = 1
            Else
                nwifi_in_900mhz = 0
            End If

            Try

                'Get Guid
                GetGuid(SiteId)

                'ASSET TAG
                If hdnTagType.Value = enumDeviceCategory.AssetandPatientTag Then

                    sProfileData = "PROFILE {" & _
                                   """DeviceProfile"":" & _
                                    "{" & _
                                    """centrak_guid"":""" & centrak_guid & """," & _
                                    """device_type"":""" & device_type & """," & _
                                    """device_category"":""0""," & _
                                    """device_id"":""" & device_id & """," & _
                                    """ir_profile"":""" & ir_profile & """," & _
                                    """ir_report_time"":""" & ir_report_time & """," & _
                                    """rf_report_time"":""" & rf_report_time & """," & _
                                    """ir_rx_profile"":""" & ir_rx_profile & """," & _
                                    """enable_fpb"":""" & nenable_fpb & """," & _
                                    """operating_mode"":""" & operating_mode & """," & _
                                    """wifi_reporting_time"":""" & wifi_reporting_time & """," & _
                                    """lf_reg_config"":""" & lf_reg_config & """," & _
                                    """wifi_in_900mhz"":""" & nwifi_in_900mhz & """," & _
                                    """paging_profile"":""" & paging_profile & """," & _
                                    """disable_lf"":""" & disable_lf & """," & _
                                    """long_ir_open"":""" & long_ir_open & """," & _
                                    "}" & _
                                    "}"

                    'G2 TEMP TAG
                ElseIf hdnTagType.Value = enumDeviceCategory.G2TempTag Then

                    sProfileData = "PROFILE {" & _
                                      """DeviceProfile"":" & _
                                       "{" & _
                                       """centrak_guid"":""" & centrak_guid & """," & _
                                       """device_type"":""" & device_type & """," & _
                                       """device_category"":""" & hdnTagType.Value & """," & _
                                       """device_id"":""" & device_id & """," & _
                                       """ir_profile"":""" & ir_profile & """," & _
                                       """ir_report_time"":""" & ir_report_time & """," & _
                                       """rf_report_time"":""" & rf_report_time & """," & _
                                       """ir_rx_profile"":""" & ir_rx_profile & """," & _
                                       """operating_mode"":""" & operating_mode & """," & _
                                       """wifi_reporting_time"":""" & wifi_reporting_time & """," & _
                                       """lf_reg_config"":""" & lf_reg_config & """," & _
                                       """wifi_in_900mhz"":""" & nwifi_in_900mhz & """," & _
                                       """paging_profile"":""" & paging_profile & """," & _
                                       """disable_lf"":""" & disable_lf & """," & _
                                       """long_ir_open"":""" & long_ir_open & """," & _
                                       """temp_report_rate"":""" & temp_report_rate & """," & _
                                       "}" & _
                                       "}"
                    'HUMIDITY TAG    
                ElseIf hdnTagType.Value = enumDeviceCategory.HumidityTag Then

                    sProfileData = "PROFILE {" & _
                                           """DeviceProfile"":" & _
                                            "{" & _
                                            """centrak_guid"":""" & centrak_guid & """," & _
                                            """device_type"":""" & device_type & """," & _
                                            """device_category"":""" & hdnTagType.Value & """," & _
                                            """device_id"":""" & device_id & """," & _
                                            """operating_mode"":""" & operating_mode & """," & _
                                            """paging_profile"":""" & paging_profile & """," & _
                                            """measurement_rate"":""" & temp_report_rate & """," & _
                                            "}" & _
                                            "}"
                    'G1 TEMP TAG
                ElseIf hdnTagType.Value = enumDeviceCategory.G1TempTag Then
                    sProfileData = "PROFILE {" & _
                                             """DeviceProfile"":" & _
                                              "{" & _
                                              """centrak_guid"":""" & centrak_guid & """," & _
                                              """device_type"":""" & device_type & """," & _
                                              """device_category"":""" & hdnTagType.Value & """," & _
                                              """device_id"":""" & device_id & """," & _
                                              """operating_mode"":""" & operating_mode & """," & _
                                              "}" & _
                                              "}"

                End If

                'Send Profile Data to PC Server
                SendCommandToPcServer(IPAddress, sProfileData, sResponse)

                'If Sucessfully config to server then Update Ini table 
                If sResponse.Contains("successfully") Then

                    Update_TagProfiles(SiteId, device_type, device_id, 0, ir_profile, long_ir_open, ir_report_time, rf_report_time, ir_rx_profile, nenable_fpb, lf_reg_config, disable_lf, operating_mode, wifi_reporting_time, nwifi_in_900mhz, paging_profile, temp_report_rate, hdnIsDefaultProfile.Value)
                    hdnIsDefaultProfile.Value = False

                    'ReLoad Update Profile Data
                    GetProfileData(SiteId, device_id)

                    lblErrormsg.InnerText = "Successfully Updated ..!"
                Else

                    'Error 
                    dtError = ConvertJSONToDataTable(sResponse)

                    If Not dtError Is Nothing Then
                        If dtError.Rows.Count > 0 Then
                            lblErrormsg.InnerText = dtError.Rows(0).Item("errormessage")
                        End If
                    End If

                End If
            Catch ex As Exception
                WriteLog("btnSend_Click - " & ex.Message)
            End Try

        End Sub
    End Class
End Namespace

