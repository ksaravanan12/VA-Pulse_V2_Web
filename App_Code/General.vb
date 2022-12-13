Imports System.IO
Imports System.Diagnostics
Imports System.Web
Imports System.Data
Imports System.Web.HttpContext
Imports System.Net.Mail
Imports System.Xml
Imports System.Net
Imports WebSupergoo.ABCpdf11
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Security.Cryptography

Namespace GMSUI
    Public Module General

        Public Const CURR_PAGE As Integer = 1
        Public Const NEXT_PAGE As Integer = 2
        Public Const PREV_PAGE As Integer = 3
        Public Const MAXROWCNT As Integer = 100
        Public AUTHENTICATION_KEY_DEFAULT As String = "4810984da6afc495fd53bd0167c4011aa35ad1a98d7bb2e323d4"
        Public AUTHENTICATION_KEY As String = ""
        Public Const HOURLY As Integer = 1
        Public Const DAILY As Integer = 2
        Public Const WEEKLY As Integer = 3
        Public Const MONTHLY As Integer = 4
        Private key As Byte() = New Byte(7) {1, 2, 3, 4, 5, 6, 7, 8}
        Private iv As Byte() = New Byte(7) {1, 2, 3, 4, 5, 6, 7, 8}
        Public VA_AuthenticationKey As String = "RoleKey***"
        

        Public Enum GMSAPITestbed

            SiteSummary = 1
            SiteDetailedOverview = 2
            AlertList = 3
            AlertInfo = 4
            GlossaryInfo = 5
            SiteListInfo = 6
            UserAuthentication = 7
            LastUpdatedOn = 8
            DeviceListTag = 9
            DeviceListMonitor = 10
            DeviceListStar = 11
            DeviceProfile = 12
            DeviceActivity = 13
            DeviceHourlyInfo = 14
            DeviceSearch = 15

        End Enum
	
        Public Enum EnumFilterType

            enum_AssetTAG = 1
            enum_MMAssetTAG = 2
            enum_StaffTAG = 3
            enum_MMStaffTAG = 4
            enum_TempTag = 5
            enum_ERUTag = 6
            enum_HumidityTag = 7
            enum_PatientTag = 8
            enum_G2TempTag = 9
            enum_SUPT = 10
            enum_AmbientTempRH = 11
            enum_InterfaceTag = 12
            enum_DisplayTag = 13
            enum_MotherTag = 14
            enum_DuraTag = 15
            enum_UmbilicalTag = 16
            enum_SecureTag = 17

        End Enum
	
        Public Enum EnumMonitorFilterType
	
            enum_RegularMonitor = 1
            enum_MMMonitor = 2
            enum_Dim = 3
            enum_EGRESS = 4
            enum_VWMONITOR = 5
            enum_HighPowerDIM = 6
	    
        End Enum
	
        ''' <summary>
        ''' Star Filter Type
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EnumStarFilterType

            enum_Star = 1
            enum_Accesspoint = 2

        End Enum

        Public Enum enumDeviceType

            Tag = 1
            Monitor = 2
            Star = 3

        End Enum

        Public Enum enumPulseUITable

            WorkflowTags = 1
            EnvironmentalTags = 2
            Infrastructure = 3

        End Enum

        Public Enum EnumTagSubTypeNew

            Asset = 1
            Autoclave = 2
            Dura = 3
            AssetMicro = 4
            AssetMiniMultiMode = 5
            AssetMini = 6
            AssetMultiMode = 7
            Staff = 8
            StaffDuress = 9
            StaffMultiMode = 10
            CallPoint = 11
            Patient = 12
            PatientMicro = 13
            PatientMiniMultiMode = 14
            PatientMiniStandalone = 15
            PatientMini = 16
            PatientMultiMode = 17
            PatientnewbabyUmbilical = 18
            Patientnewmom = 19
            PatientSecureAdult = 20
            PatientSecurenewbaby = 21
            PatientSecureRugged = 22
            PatientSingleUse = 23
            Patient31DayBlue = 24
            Patient31DayGreen = 25
            Patient31DayOrange = 26
            GeoPendantBlack = 27
            GeoPendantWhite = 28
            Survey = 29
            G1NonDisplay = 30
            StandardNonDisplay = 31
            Standard = 32
            UltraLow = 33
            VAC = 34
            HumidityNonDisplay = 35
            Humidity = 36
            DAPG1Display = 37
            O2G1Display = 38
            CO2G1Display = 39
            ExternalDisplay = 40

        End Enum     

	   Public Enum enumEventType
           
            eventNone = 0
            eventWiFiLocation = 1
            event900MHzLocation = 2
            event900MHzHygiene = 3
            event900MHzStar = 4
            eventTemperature = 5
            eventHumidity = 6

        End Enum

        Public Enum enumUserRole
	
            Admin = 1
            Partner = 2
            Customer = 3
            Maintenance = 4
            Engineering = 5
            API_User = 6
            AssetTrackUser = 7
            BatteryTech = 8
            TechnicalAdmin = 25
            Support = 32 
            MaintenancePrism = 33
	    
        End Enum
	
        Public Enum EnumUserTypeID

            enum_Admin = 1
            enum_SuperAdmin = 2
            enum_CompanyAdmin = 3
            enum_SiteUser = 4
            enum_CompanyUser = 5

        End Enum

        Public Enum enumFilterDataType

            enum_NumericOnly = 1
            enum_NumberWithFraction = 2
            enum_Date = 3
            enum_String = 4
            enum_Boolean = 5

        End Enum

        Public Enum FusionChartsMaxkeXML

            TagMovements = 1
            AlertHistory = 2

        End Enum

        Public Enum enumLocalAlertServices

            Time_Server = 4
            Backup_Utility = 5
            Streaming_Server = 6
            Paging_Server = 7
            Location_Server = 8
            Star_Log = 9
            PC_Server = 10
            Rauland_Service = 11
            WiFiConnector_Service = 12
            Server_Health = 13
            GMS_Connector_Service = 14
            UCS_Connector_Service = 15
            Heart_Beat = 20

        End Enum

        Public Enum enumTagAlert

            Regular_Tag = 1
            No_Sleep_Tag = 2
            IR_Profile_Conflict = 29
            Active_Count_Less = 48

        End Enum

        Public Enum enumMonitorAlert

            Not_Reporting = 3
            Not_Seen = 4
            Too_Many_Reports = 30
            DIM_Reports = 31
            DIM_Trigger = 32
            Active_Count_Less = 49

        End Enum

        Public Enum enumStarAlert

            Not_Seen = 5
            Not_Associated = 6
            In_Active = 7
            Ethernet_Over_Limit = 8
            Non_Sync_TimeServer = 9
            Association_Changed = 10
            Non_Sync = 11
            Beacon_InActive = 44
            All_InActive = 45
            Active_Count_Less = 50

        End Enum

        Public Enum enumAlertGraphType

            All = 0
            Tag = 1
            Monitor = 2
            Star = 3
            Service = 4

        End Enum

        Public Enum enumMapType

            Campus = 1
            Building = 2
            Floor = 3
            Unit = 4

        End Enum

        Public Enum enumAlertType

            ContinuousAlert = 0
            SingleAlert = 1

        End Enum

        Public Enum enumPageAction

            Add = 1
            Edit = 2
            Delete = 3
            View = 4
            Search = 5
            Filter = 6
            Logon = 7
            Logout = 8
            AccessViolation = 9
            Security = 10

        End Enum
	
        Public Enum enumReportType

            MonitorAnalysisReport = 1
            DeviceSummary = 2
            TTSyncErrReport = 3
            ConnectivityReport = 4
            DefectiveReport = 5

        End Enum

        Public Enum ControllerState

            CONTROLLER_IDLE_STATE = 1
            CONTROLLER_EVENT_STATE = 2
            CONTROLLER_EGRESS_STATE = 3
            CONTROLLER_ALARM_STATE = 4
            CONTROLLER_LOITERING_STATE = 5
            CONTROLLER_STATE_DOOR_AJAR = 6
            CONTROLLER_STATE_FIRE_ALARM = 7
            CONTROLLER_STATE_EMERGENCY = 8
            CONTROLLER_STATE_COMMAND = 9
            CONTROLLER_STATE_TAMPER = 10
            CONTROLLER_STATE_TRANSPORT_TRANSFER = 11
            CONTROLLER_STATE_UNAUTHORISED_ALERT = 12
            CONTROLLER_STATE_UNAUTHORISED_ALERT2 = 13

        End Enum
        
        ''' <summary>
        ''' CompanyId
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EnumCompanyId

            enum_Company_Centrak = 1

        End Enum


        Public Enum enumSiteAnalysisReport
          
            Analysischarts = 1
            Analysisalerts = 2
            RAWData = 9
            DeviceDetail = 10

        End Enum

        Public Function EscapeString(ByVal strvar As String) As String
	
            strvar = Trim(strvar)
            EscapeString = Replace(strvar, "'", "''")
	    
        End Function

        ''' <summary>To convert boolean to integer</summary>
        ''' <param name="bFlag"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CBooltoInt(ByVal bFlag As Boolean) As Integer
	
            If bFlag = True Then Return 1
	    
            Return 0
	    
        End Function

        Public Function GetDeviceTypeName(ByVal nDeviceType As Integer, ByVal nType As Integer) As String

            If nDeviceType = enumDeviceType.Tag Then
                Select Case nType
                    Case EnumFilterType.enum_AssetTAG
                        Return "ASSET TAG"
                    Case EnumFilterType.enum_MMAssetTAG
                        Return "MM ASSET TAG"
                    Case EnumFilterType.enum_StaffTAG
                        Return "STAFF TAG"
                    Case EnumFilterType.enum_MMStaffTAG
                        Return "MM STAFF TAG"
                    Case EnumFilterType.enum_TempTag
                        Return "TEMP TAG"
                    Case EnumFilterType.enum_ERUTag
                        Return "ERU TAG"
                    Case EnumFilterType.enum_HumidityTag
                        Return "AMBIENT TEMP TAG"
                    Case EnumFilterType.enum_AmbientTempRH
                        Return "AMBIENT TEMP RH TAG"
                    Case EnumFilterType.enum_PatientTag
                        Return "PATIENT TAG"
                    Case EnumFilterType.enum_G2TempTag
                        Return "G2TEMP TAG"
                    Case EnumFilterType.enum_SUPT
                        Return "SUPT TAG"
                End Select
                Return "TAG"
            ElseIf nDeviceType = enumDeviceType.Monitor Then
                Select Case nType
                    Case EnumMonitorFilterType.enum_Dim
                        Return "DIM MONITOR"
                    Case EnumMonitorFilterType.enum_EGRESS
                        Return "LF EXCITER"
                    Case EnumMonitorFilterType.enum_MMMonitor
                        Return "MM MONITOR"
                    Case EnumMonitorFilterType.enum_RegularMonitor
                        Return "REGULAR MONITOR"
                End Select
                Return "MONITOR"
            ElseIf nDeviceType = enumDeviceType.Star Then
                Select Case nType
                    Case 1
                        Return "RF Sync Stars"
                    Case 2
                        Return "Reqular Stars"
                    Case 3
                        Return "Ethernet Stars"
                End Select

                Return "STAR"
            End If

            Return ""
        End Function
        Public Function GetDeviceTypeName(ByVal nDeviceType As Integer) As String

            If nDeviceType = enumDeviceType.Tag Then
                Return "TAG"
            ElseIf nDeviceType = enumDeviceType.Monitor Then
                Return "MONITOR"
            ElseIf nDeviceType = enumDeviceType.Star Then
                Return "STAR"
            End If

            Return ""
        End Function

        '''************************************************************************************************ 
        ''' <summary>To Add The Contents In The Cell And  The Cell Added To The Table</summary>
        ''' <param name="tblrow">HtmlTableRow Object</param>
        ''' <param name="strvalue">String will added to current cell</param>
        ''' <param name="salign">Alignment of cell</param>
        ''' <param name="ncolspan">Colspan of cell</param>
        ''' <param name="ncellwidth">Width of cell</param>
        ''' <param name="nrowspan">Rowspan of cell</param>
        ''' <param name="nValign">Vertical align of cell</param>
        ''' <param name="nHeight">Height of the cell</param>
        ''' <param name="clsName">Style of the cell</param>
        ''' <remarks></remarks>
        '''************************************************************************************************ 
        Public Function AddCell(ByVal tblrow As HtmlTableRow, ByVal strvalue As String, Optional ByVal salign As String = "left", Optional ByVal ncolspan As Integer = 1, Optional ByVal ncellwidth As String = "",
                                Optional ByVal nrowspan As Integer = 1, Optional ByVal nValign As String = "middle", Optional ByVal nHeight As String = "", Optional ByVal clsName As String = "",
                                Optional ByVal sVisible As String = "visible", Optional ByVal tdID As String = "", Optional ByVal strFromTagList As Boolean = False, Optional ByVal FormatDigit As Integer = 0) As HtmlTableCell
            Dim tblcell As New HtmlTableCell

            If ncolspan > 1 Then
                tblcell.ColSpan = ncolspan
            End If

            If nrowspan > 1 Then
                tblcell.RowSpan = nrowspan
            End If

            If FormatDigit <> 0 And IsNumeric(strvalue) Then
                strvalue = FormatNumber(strvalue, FormatDigit)
            End If

            tblcell.InnerHtml = strvalue
            tblcell.Align = salign
            tblcell.VAlign = nValign

            If Not ncellwidth = "" Then
                tblcell.Style.Add("Width", ncellwidth)
            End If

            If Not nHeight = "" Then
                tblcell.Height = nHeight
            End If

            If Not clsName = "" Then
                tblcell.Attributes.Add("class", clsName)
            End If

            If strFromTagList Then
                If Not sVisible = "" Then
                    If sVisible = "visible" Then
                        tblcell.Style.Add("display", "table-cell")
                    ElseIf sVisible = "hidden" Then
                        tblcell.Style.Add("display", "none")
                    End If
                End If
            End If

            If Not tdID = "" Then
                tblcell.Attributes.Add("ID", tdID)
            End If

            tblrow.Cells.Add(tblcell)
            Return tblcell

        End Function
        '************************************************************************************************************
        ' RedirectToErrorPage
        ' ===================
        ' Description :  ReDirect the Page where the Page Contatins Errors
        '************************************************************************************************************
        Public Sub RedirectToErrorPage(ByVal strErrorMsg As String)
            Try
                PageVisitDetails(g_UserId, "Logout", enumPageAction.Logout, "user logout with session expired")
            Catch ex As Exception
                WriteLog("Logout " & g_UserId & ex.Message.ToString())
            End Try
            Dim url As String
            url = HttpContext.Current.Request.Url.AbsoluteUri
            HttpContext.Current.Session("RedirectToUrl") = url
            HttpContext.Current.Response.Redirect("./ApplicationError.aspx?ErrorValue=" & strErrorMsg)
        End Sub

        Public Sub RedirectToSecurePage()
            Dim servername, sPath, sRedirect As String
            servername = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
            sPath = HttpContext.Current.Request.ServerVariables("PATH_INFO")
            sRedirect = "http://" & servername & sPath
            HttpContext.Current.Response.Redirect(sRedirect)
        End Sub

        Public Function GetAppPath() As String
            Dim Apppath As String
            Apppath = HttpContext.Current.Server.MapPath(".")
            Return Apppath
        End Function

        Public Function GetServerPath() As String

            Dim sURL As String = ""
            Dim servername As String = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
            Dim sPath As String = HttpContext.Current.Request.ServerVariables("PATH_INFO")

            sPath = sPath.Substring(0, sPath.LastIndexOf("/"))

            If HttpContext.Current.Request.ServerVariables("SERVER_PORT_SECURE") Then
                sURL = "https://" & servername & sPath & "/"
            Else
                sURL = "http://" & servername & sPath & "/"
            End If

            Return sURL
        End Function

        Public Function IsSecure() As Boolean
            Dim isSsl As Boolean = False
            isSsl = True
            Return isSsl
        End Function
	
        Public Function StrContainsComma(ByVal strName As String) As Boolean
            Dim StrArr() As String
            StrArr = strName.Split(",")
            If StrArr.Length > 1 Then
                Return True
            Else
                Return False
            End If

        End Function
        '*************************************************************************
        ' WriteLog
        ' ========
        ' Write Log String for Debugging purpose
        '
        ' Input:    String to log
        '
        ' Output:   None
        '**************************************************************************
        Public Sub WriteLog(ByVal strLog As String, Optional ByVal bNeedSession As Boolean = True)
            Dim FS As FileStream
            Dim SW As StreamWriter
            Dim strLogFileName As String

            strLogFileName = GetAppPath() & "\GMS_log.txt"
            Try
                FS = New FileStream(strLogFileName, FileMode.Append)
                SW = New StreamWriter(FS)

                If (String.Empty.Equals(strLog)) Then
                    SW.WriteLine("")
                Else
                    SW.Write(Format(Now(), "yyyyMMdd hh:mm:ss "))
                    SW.WriteLine(strLog)
                End If
                SW.Close()
                FS.Close()
                SW = Nothing
                FS = Nothing
            Catch ex As Exception

            End Try

        End Sub
        Public Function CheckIsDBNull(ByVal str As Object, Optional ByVal isdate As Boolean = False, Optional ByVal retisnullstr As String = "") As String
            If IsDBNull(str) Then
                CheckIsDBNull = retisnullstr
            ElseIf str Is Nothing Then
                CheckIsDBNull = retisnullstr
            ElseIf CType(str, String).Trim = "" Then
                CheckIsDBNull = retisnullstr
            Else
                If isdate Then
                    If str <> "1/1/1900" And str <> "01/01/1900" Then
                        CheckIsDBNull = Format(CDate(str), "MM/dd/yyyy")
                    Else
                        CheckIsDBNull = retisnullstr
                    End If
                Else
                    CheckIsDBNull = str
                End If
            End If

        End Function
        Public Function GoToPage(ByVal str As String, ByVal Type As Integer) As Integer
            Dim nPage As Integer
            str = Trim(str)
            If str = "" Then str = "0"
            nPage = 1
            Select Case Type
                Case CURR_PAGE
                    nPage = CInt(str)
                Case NEXT_PAGE
                    nPage = CInt(str) + 1
                Case PREV_PAGE
                    nPage = CInt(str) - 1
            End Select
            Return nPage
        End Function
        Public Function SetCurrentPages(ByVal count As Integer, ByRef btnPrev As Button, ByRef btnnext As Button, ByRef currpage As Integer, Optional ByVal MaxRows As Integer = MAXROWCNT) As Integer
            Dim MaxPage As Integer
            MaxPage = Math.Ceiling(count / MaxRows)
            'WriteLog(" SetCurrentPages MaxPage : " & MaxPage & " count : " & count & " MaxRows " & MaxRows)
            If (MaxPage = 1) Then
                currpage = 1
                btnPrev.Visible = False
                btnnext.Visible = False
            End If
            If (currpage <= 1) Then
                currpage = 1
                btnPrev.Enabled = False
            Else
                btnPrev.Enabled = True
            End If

            If (currpage >= MaxPage) Then
                currpage = MaxPage
                btnnext.Enabled = False
            Else
                btnnext.Enabled = True
            End If

            Return MaxPage
        End Function
        Public Sub TableInitialSetUp(ByRef bPrev As Button, ByRef bNext As Button, ByRef bgo As Button, ByRef tPageNo As HtmlInputText, ByRef lTotal As Label, ByRef lPage As Label, ByRef lDevicecount As Label)
            bPrev.Visible = False
            bNext.Visible = False
            tPageNo.Visible = False
            bgo.Visible = False
            lTotal.Text = ""
            lPage.Text = ""
            lDevicecount.Text = ""
        End Sub
        Public Sub TableNextSetUp(ByRef bPrev As Button, ByRef bNext As Button, ByRef bgo As Button, ByRef tPageNo As HtmlInputText)
            bPrev.Visible = True
            bNext.Visible = True
            tPageNo.Visible = True
            bgo.Visible = True
        End Sub
        Function Password_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0
            Try
                Dim addColumn() As String = {"Password"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)
                        If (sitelist_Node.HasChildNodes) Then
                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("Password") = Sitelist_NodeList(0).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" serverIPList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function
        Function serverIPList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0
            Try
                Dim addColumn() As String = {"IsActiveIP", "ServerIP"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)
                        If (sitelist_Node.HasChildNodes) Then
                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("IsActiveIP") = Sitelist_NodeList(0).InnerText
                            dNewRow("ServerIP") = Sitelist_NodeList(1).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" serverIPList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function
        Function usertypeList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0
            Try
                Dim addColumn() As String = {"UserTypeId", "UserType"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)
                        If (sitelist_Node.HasChildNodes) Then
                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("UserTypeId") = Sitelist_NodeList(0).InnerText
                            dNewRow("UserType") = Sitelist_NodeList(1).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" usertypeList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function
        Function userroleList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0
            Try
                Dim addColumn() As String = {"UserRoleId", "UserRole"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)
                        If (sitelist_Node.HasChildNodes) Then
                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("UserRoleId") = Sitelist_NodeList(0).InnerText
                            dNewRow("UserRole") = Sitelist_NodeList(1).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" userroleList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function
	
        Function siteList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim cnt As Long = 0

            Try

                Dim addColumn() As String = {"SiteId", "SiteName", "sitetype"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1

                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)

                        If (sitelist_Node.HasChildNodes) Then

                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("SiteId") = Sitelist_NodeList(0).InnerText
                            dNewRow("SiteName") = Sitelist_NodeList(1).InnerText
                            dNewRow("sitetype") = Sitelist_NodeList(2).InnerText

                            If Sitelist_NodeList(2).InnerText <> "3X" Then
                                dt.Rows.Add(dNewRow)
                            End If

                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" siteList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt

        End Function

        Function comanyList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable

            Dim dt As New DataTable
            Dim cnt As Long = 0

            Try
                Dim addColumn() As String = {"SiteId", "SiteName"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1

                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)

                        If (sitelist_Node.HasChildNodes) Then

                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("SiteId") = Sitelist_NodeList(0).InnerText
                            dNewRow("SiteName") = Sitelist_NodeList(1).InnerText

                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" siteList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt

        End Function

        Function configuresiteList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0

            Try

                Dim addColumn() As String = {"SiteId", "SiteName", "GMSSiteId"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1

                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)

                        If (sitelist_Node.HasChildNodes) Then

                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("SiteId") = Sitelist_NodeList(0).InnerText
                            dNewRow("SiteName") = Sitelist_NodeList(1).InnerText
                            dNewRow("GMSSiteId") = Sitelist_NodeList(2).InnerText
                            dt.Rows.Add(dNewRow)

                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" siteList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt

        End Function

	Function siteFolder_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0
            Try
                Dim addColumn() As String = {"SiteFolder"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)
                        If (sitelist_Node.HasChildNodes) Then
                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("SiteFolder") = Sitelist_NodeList(0).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" siteFolder_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function

        Function emailList_XMLNodetoDataTable(ByVal xmlsitelistNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim cnt As Long = 0
            Try
                Dim addColumn() As String = {"SiteName", "Email"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlsitelistNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim sitelist_Node As XmlNode = xmlsitelistNd.ChildNodes(i)
                        If (sitelist_Node.HasChildNodes) Then
                            Dim Sitelist_NodeList As XmlNodeList
                            Sitelist_NodeList = sitelist_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("SiteName") = Sitelist_NodeList(0).InnerText
                            dNewRow("Email") = Sitelist_NodeList(1).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" siteList_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function

        Function CentrakDevices_XMLNodetoDataTable(ByVal xmlModelItemNd As XmlNode) As DataTable

            Dim dt As New DataTable
            Dim cnt As Long = 0

            Try
                Dim addColumn() As String = {"ModelItem", "DeviceSubType", "ORG_BatteryCapacity", "IsLBIRule", "IsBatteryCapacity", "IsAutoReplacement", "IsFDK", "IsDetectionofBatteryDischarge"}
                dt = addColumntoDataTable(addColumn)
                cnt = xmlModelItemNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        'dim 
                        Dim ModelItem_Node As XmlNode = xmlModelItemNd.ChildNodes(i)
                        If (ModelItem_Node.HasChildNodes) Then
                            Dim ModelItem_NodeList As XmlNodeList
                            ModelItem_NodeList = ModelItem_Node.ChildNodes

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            dNewRow("ModelItem") = ModelItem_NodeList(0).InnerText
                            dNewRow("DeviceSubType") = ModelItem_NodeList(1).InnerText
                            dNewRow("ORG_BatteryCapacity") = ModelItem_NodeList(2).InnerText
                            dNewRow("IsLBIRule") = ModelItem_NodeList(3).InnerText
                            dNewRow("IsBatteryCapacity") = ModelItem_NodeList(4).InnerText
                            dNewRow("IsAutoReplacement") = ModelItem_NodeList(5).InnerText
                            dNewRow("IsFDK") = ModelItem_NodeList(6).InnerText
                            dNewRow("IsDetectionofBatteryDischarge") = ModelItem_NodeList(7).InnerText
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" CentrakDevices_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function

        Function XMLNodeToDataTable(ByVal xmlNodeInput As XmlNode) As DataTable
            Dim dSet As New DataSet
	    
            Dim dt As New DataTable

            Try
                If xmlNodeInput IsNot Nothing Then
                    Dim xmlReader As New XmlTextReader(xmlNodeInput.OuterXml, XmlNodeType.Element, Nothing)
                    dSet.ReadXml(xmlReader)
                End If

            Catch ex As Exception
                WriteLog(" XMLNodeToDataTable " & ex.Message.ToString)
            End Try

            If dSet.Tables.Count > 0 Then
                dt = dSet.Tables(0)
            End If
	    
            Return dt
        End Function

        Function XMLNodeToDataTable(ByVal xmlNodeInput As XmlNode, ByVal tableIdx As Integer) As DataTable
            Dim dSet As New DataSet
	    
	    Dim dt As New DataTable

            Try
                If xmlNodeInput IsNot Nothing Then
                    Dim xmlReader As New XmlTextReader(xmlNodeInput.OuterXml, XmlNodeType.Element, Nothing)
                    dSet.ReadXml(xmlReader)
                End If

            Catch ex As Exception
                WriteLog(" XMLNodeToDataTable - tableIdx " & ex.Message.ToString)
            End Try
            
            If dSet.Tables.Count > tableIdx Then
                dt = dSet.Tables(tableIdx)
            End If
	    
            Return dt
        End Function

        Function XMLNodeToDataSet(ByVal xmlNodeInput As XmlNode) As DataSet

            Dim dSet As New DataSet

            Try
                If xmlNodeInput IsNot Nothing Then
                    Dim xmlReader As New XmlTextReader(xmlNodeInput.OuterXml, XmlNodeType.Element, Nothing)
                    dSet.ReadXml(xmlReader)
                End If

            Catch ex As Exception
                WriteLog(" XMLNodeToDataSet " & ex.Message.ToString)
            End Try

            Return dSet

        End Function

        Function siteinfo_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable

            Dim cnt As Long = 0
            Dim gmslastUpdated As String = ""
            Dim dt As New DataTable

            Dim addColumn() As String = {"SiteId", "SiteName", "PCServerVersion", "System", "Tag_LbiCount", "Tag_UnderWatchCount", "Infras_LbiCount", "Infras_UnderWatchCount", "isHeartbeatAvailable", "isLAAlertsAvailable", "IsSiteConnectivityLost", "IsCriticalAlert", "TimeZone", "Lastupdate", "IsPrismView"}

            dt = addColumntoDataTable(addColumn)

            Try
	    
                cnt = xmlNd.ChildNodes.Count
		
                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        Dim str_xmlchildnode As XmlNodeList
                    
                        str_xmlchildnode = xmlNd.ChildNodes(i).ChildNodes
			
                        Dim dNewRow As DataRow = Nothing
			
                        dNewRow = dt.NewRow
			
                        For n As Integer = 0 To str_xmlchildnode.Count - 1
			
                            Dim Nodename As String = str_xmlchildnode(n).Name
			    
                            If Nodename.ToLower = "tag" Or Nodename.ToLower = "infrastructure" Then
			    
                                If (str_xmlchildnode(n).HasChildNodes) Then
				
                                    Dim sub_xmlchildnode As XmlNodeList
                                    sub_xmlchildnode = str_xmlchildnode(n).ChildNodes
				    
                                    For k As Integer = 0 To sub_xmlchildnode.Count - 1
				    
                                        Dim subNodeName As String = sub_xmlchildnode(k).Name
                                        Dim subNodevalue As String = sub_xmlchildnode(k).InnerText
                                        Dim prefix As String = ""
					
                                        If (Nodename.ToLower = "tag") Then
                                            prefix = "Tag_"
                                        ElseIf (Nodename.ToLower = "infrastructure") Then
                                            prefix = "Infras_"
                                        End If
					
                                        Dim ndstr As String = prefix & subNodeName
					
                                        dNewRow(ndstr) = subNodevalue
					
                                    Next

                                End If
                            Else
			    
                                Dim Nodevalue As String = str_xmlchildnode(n).InnerText
                                dNewRow(Nodename) = Nodevalue
				
                            End If
                        Next

                        dt.Rows.Add(dNewRow)

                    Next
                End If
		
            Catch ex As Exception
                WriteLog(" loadsiteInfo-XMLNodetoDataTable : " & ex.Message.ToString())
            End Try
            Return dt
        End Function

        Public Function siteOverview_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable

            Dim siteid As String = ""
            Dim sitename As String = ""
            Dim Lastupdate As String = ""
            Dim DefinedTagsinCore As String = ""
            Dim DefinedInfrastructureinCore As String = ""

            Dim dt As New DataTable

            Dim addColumn() As String = {"SiteId", "Sitename", "Lastupdate", "DeviceType", "TypeID", "Type", "SortId", "Good", "LessThen180Days", "LessThen90Days", "LessThen30Days", "OfflineBatteryDepleted", "OfflineOther", "DefinedTagsinCore", "DefinedInfrastructureinCore", "PulseUIId", "ImageName", "UW", "LBI"}
            dt = addColumntoDataTable(addColumn)

            Try

                Dim cnt As Long = 0

                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNodeList
                        str_xmlchildnode = xmlNd.ChildNodes(i).ChildNodes

                        For nidx As Integer = 0 To str_xmlchildnode.Count - 1
                            Dim nd As XmlNode = str_xmlchildnode(nidx)
                            Select Case nd.Name.ToString
                                Case "SiteId"
                                    siteid = nd.InnerText
                                Case "SiteName"
                                    sitename = nd.InnerText
                                Case "Lastupdate"
                                    Lastupdate = nd.InnerText
                                Case "DefinedTagsinCore"
                                    DefinedTagsinCore = nd.InnerText
                                Case "DefinedInfrastructureinCore"
                                    DefinedInfrastructureinCore = nd.InnerText
                                Case "Device"
                                    If nd.HasChildNodes Then
                                        Dim str_ChildNodeList As XmlNodeList
                                        str_ChildNodeList = nd.ChildNodes
                                        Dim dNewRow As DataRow = Nothing
                                        dNewRow = dt.NewRow
                                        dNewRow("SiteId") = siteid
                                        dNewRow("SiteName") = sitename
                                        dNewRow("Lastupdate") = Lastupdate
                                        dNewRow("DefinedTagsinCore") = DefinedTagsinCore
                                        dNewRow("DefinedInfrastructureinCore") = DefinedInfrastructureinCore

                                        For n As Integer = 0 To str_ChildNodeList.Count - 1
                                            Dim subNodeName As String = str_ChildNodeList(n).Name
                                            Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                            dNewRow(subNodeName) = subNodevalue
                                        Next
                                        dt.Rows.Add(dNewRow)
                                    End If
                            End Select
                        Next

                    Next
                End If
            Catch ex As Exception
                WriteLog(" LoadsiteOverview - siteOverview_XMLNodetoDataTable" & ex.Message.ToString())
            End Try
            Return dt
        End Function
        Public Function SiteAlertList_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dtReport As New DataTable

            Dim addColumn() As String = {"SiteId", "SiteName", "UnViewCount"}
            dt = addColumntoDataTable(addColumn)

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        If str_xmlchildnode.HasChildNodes Then
                            Dim str_ChildNodeList As XmlNodeList
                            str_ChildNodeList = str_xmlchildnode.ChildNodes
                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow
                            For n As Integer = 0 To str_ChildNodeList.Count - 1
                                Dim Nodename As String = str_ChildNodeList(n).Name
                                Dim Nodevalue As String = str_ChildNodeList(n).InnerText
                                If Nodename = "SiteName" Then
                                    If Nodevalue.Length > 40 Then
                                        Nodevalue = Nodevalue.Substring(0, 37)
                                        Nodevalue = Nodevalue & " ..."

                                    End If
                                End If
                                dNewRow(Nodename) = Nodevalue
                            Next
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" loadsiteInfo - SiteAlertList_XMLNodetoDataTable" & ex.Message.ToString())
            End Try
            Return dt
        End Function

        Public Function SiteDeviceLsit_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim nSiteId As String
            Dim SiteName As String
            Dim totalPage As String
            Dim TotalCount As String

            dt = GetDeviceListCols(enumDeviceType.Monitor)

            Try
                Dim cnt As Long = 0

                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename.ToLower = "monitor") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                Dim dNewRow As DataRow = Nothing
                                dNewRow = dt.NewRow
                                dNewRow("UserRole") = g_UserRole
                                dNewRow("SiteId") = nSiteId
                                dNewRow("SiteName") = SiteName
                                dNewRow("TotalPage") = totalPage
                                dNewRow("TotalCount") = TotalCount
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "siteid") Then
                                nSiteId = Nodevalue
                            ElseIf (Nodename.ToLower = "sitename") Then
                                SiteName = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalPage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                TotalCount = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" SiteDeviceLsit_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt
        End Function
        Public Function SiteDevicePhotoLsit_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable

            Dim dt As New DataTable
            Dim DeviceId As String = ""
            Dim Path As String = ""
            Dim Info As String = ""
            Dim DataId As String = ""
            Dim thumbnailUrl As String = ""

            Dim addColumn() As String = {"DeviceId", "Path", "Info", "DataId", "thumbnailUrl"}
            dt = addColumntoDataTable(addColumn)

            Try

                Dim cnt As Long = 0

                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1

                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name

                        If (Nodename.ToLower = "photolist") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                Dim dNewRow As DataRow = Nothing
                                dNewRow = dt.NewRow
                                dNewRow("DeviceId") = DeviceId
                                dNewRow("Path") = Path
                                dNewRow("Info") = Info
                                dNewRow("DataId") = DataId
                                dNewRow("thumbnailUrl") = thumbnailUrl

                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next

                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "deviceid") Then
                                DeviceId = Nodevalue
                            ElseIf (Nodename.ToLower = "path") Then
                                Path = Nodevalue
                            ElseIf (Nodename.ToLower = "Info") Then
                                Info = Nodevalue
                            ElseIf (Nodename.ToLower = "DataId") Then
                                DataId = Nodevalue
                            ElseIf (Nodename.ToLower = "thumbnailUrl") Then
                                thumbnailUrl = Nodevalue
                            End If

                        End If

                    Next
                End If
            Catch ex As Exception
                WriteLog(" SiteDevicePhotoLsit_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt
        End Function
        Public Function SiteDevice_Star_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim nSiteId As String
            Dim SiteName As String
            Dim totalPage As String
            Dim TotalCount As String

            Dim addColumn() As String = {"UserRole", "SiteId", "SiteName", "TotalPage", "TotalCount", "offline", "CompanyName", "StarId", "MACId", "DeviceName", "StarType", "IPAddr", "LastReceivedTime", "LocationDataReceived", "ModelNumber", "PageDataReceived", "StarPageCount", "LockedStarId", "LockedStarCnt", "ErrorCnt", "NonSyncCnt", "FileVersion", "Version", "ModelItem", "ShipDate", "PoNumber", "SWVersion", "CertifyDate", "CompanyId", "POSiteId", "StarSubType"}
            dt = addColumntoDataTable(addColumn)

            Try
                Dim cnt As Long = 0

                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename.ToLower = "star") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                Dim dNewRow As DataRow = Nothing
                                dNewRow = dt.NewRow
                                dNewRow("UserRole") = g_UserRole
                                dNewRow("SiteId") = nSiteId
                                dNewRow("SiteName") = SiteName
                                dNewRow("TotalPage") = totalPage
                                dNewRow("TotalCount") = TotalCount
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "siteid") Then
                                nSiteId = Nodevalue
                            ElseIf (Nodename.ToLower = "sitename") Then
                                SiteName = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalPage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                TotalCount = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" SiteDevice_Star_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt
        End Function

        Public Function SiteDeviceListtag_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim nSiteId As String
            Dim SiteName As String
            Dim totalPage As String
            Dim TotalCount As String

            'Get Columns
            dt = GetDeviceListCols(enumDeviceType.Tag)

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename.ToLower = "tag") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                Dim dNewRow As DataRow = Nothing
                                dNewRow = dt.NewRow
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("UserRole") = g_UserRole
                                    dNewRow("SiteId") = nSiteId
                                    dNewRow("Sitename") = SiteName
                                    dNewRow("TotalPage") = totalPage
                                    dNewRow("TotalCount") = TotalCount
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "siteid") Then
                                nSiteId = Nodevalue
                            ElseIf (Nodename.ToLower = "sitename") Then
                                SiteName = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalPage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                TotalCount = Nodevalue
                            End If

                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" SiteDeviceListtag_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Public Function GetDeviceListCols(ByVal DeviceType As Integer) As DataTable

            Dim dtColumns As New DataTable
            Dim addColumn() As String

            If DeviceType = enumDeviceType.Tag Then

                addColumn = {"UserRole", "SiteID", "Sitename", "TotalPage", "TotalCount", "TagId", "MonitorId", "Firmware_Version", "FirstSeen", "PageDataReceived", "LocationDataReceived", "IRID", "CatastrophicCases", "LessThen30Days", "LessThen90Days", "LessThen180Days", "ShipDate", "LastSeen", "offline", "ActionRequired", "BatteryCapacity", "ActivityLevel", "ModelItem", "PoNumber", "ADCValue", "SWVersion", "BatteryReplacementCount", "BatteryReplacementOn", "TempADC", "Voltage", "BatteryStatus", "Prediction", "LastIRTime", "RoomSeen", "MonitorLastSeen", "MonitorLocation", "StarAddress", "TagType", "TagRecalibrationDate", "IsAnnualCalibration", "AnnualTags", "Temperature", "Probe1Temperature", "Probe2Temperature", "Humidity", "AvgRSSI", "CertificateDate", "MFRCalibrationDue", "ProbeId", "DeviceSubTypeId", "LocalId", "ProbeId2", "Location", "ClientCalDue", "CalFrequency", "Image", "CCatastrophicCases", "Version", "P1Units", "P2Units", "Last20WeekData", "CActivityLevel", "isUHFTags", "NewDeviceSubTypeId"}

            ElseIf DeviceType = enumDeviceType.Monitor Then

                addColumn = {"UserRole", "SiteId", "SiteName", "TotalPage", "TotalCount", "DeviceId", "DeviceName", "RoomName", "Firmware_Version", "BatteryType", "FirstSeen", "PageDataReceived", "LocationDataReceived", "IRID", "CatastrophicCases", "LessThen30Days", "LessThen90Days", "LessThen180Days", "ShipDate", "LastSeen", "offline", "BatteryCapacity", "ActionRequired", "ModelItem", "PoNumber", "BatteryReplacementCount", "BatteryReplacementOn", "BatteryStatus", "SWVersion", "MonitorType", "BatteryValue", "Last20WeekData", "IsConfiguredinCore", "CCatastrophicCases"}

            End If

            dtColumns = addColumntoDataTable(addColumn)

            Return dtColumns

        End Function

        Public Function GetEMTAGDeviceListCols(ByVal DeviceType As Integer) As DataTable
            Dim dtColumns As New DataTable
            Dim addColumn() As String

            addColumn = {"UserRole", "SiteID", "Sitename", "TagId", "TypeId", "Firmware_Version", "ModelItem", "FirstSeen", "LastSeen", "ADCValue", "BatteryCapacity", "CatastrophicCases", "LessThen30Days", "LessThen90Days", "ShipDate", "offline", "ActionRequired", "ActivityLevel", "PoNumber", "SWVersion", "BatteryReplacementCount", "BatteryReplacementOn", "TempADC", "Voltage", "BatteryStatus", "Prediction", "TagType", "TagRecalibrationDate", "IsAnnualCalibration", "AnnualTags", "Temperature", "Probe1Temperature", "Probe2Temperature", "Humidity", "CertificateDate", "MFRCalibrationDue", "ProbeId", "DeviceSubTypeId", "LocalId", "ProbeId2", "Location", "ClientCalDue", "CalFrequency", "Image", "CCatastrophicCases", "Version", "MFRCalibrationDueNew", "RMADate", "InRMA", "PhantomData"}


            dtColumns = addColumntoDataTable(addColumn)

            Return dtColumns

        End Function

        Public Function Sitelastupdateon_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable

            Dim dt As New DataTable
            Dim newColumnIns As DataColumn
            newColumnIns = Nothing
            newColumnIns = New DataColumn
            newColumnIns.ColumnName = "GMSLastupdate"
            newColumnIns.DataType = System.Type.GetType("System.String")
            dt.Columns.Add(newColumnIns)
            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count
                Dim dNewRow As DataRow = Nothing
                Dim nodename As String = xmlNd.Name
                Dim nodevalue As String = xmlNd.InnerText

                dNewRow = dt.NewRow
                dNewRow(nodename) = nodevalue
                dt.Rows.Add(dNewRow)

            Catch ex As Exception
                WriteLog(" Sitelastupdateon_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        Public Function Glossaryinfo_XMLNodetoDataTable(ByVal xmlNd As XmlNode, ByVal pagename As String) As DataTable

            Dim dt As New DataTable
            Dim addColumn() As String = {"Section", "ExpandStory", "Description"}
            dt = addColumntoDataTable(addColumn)

            Dim thisTypeOnly As String = ""
            If (pagename = "DeviceList.aspx") Then
                thisTypeOnly = "Monitor"
            ElseIf (pagename = "PateintTagList.aspx") Then
                thisTypeOnly = "Tag"
            End If


            Try

                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename = "SiteWiseSummary" And pagename = "Home.aspx") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                For j As Integer = 0 To str_ChildNodeList.Count - 1
                                    Dim str_SubChildNodeList As XmlNodeList
                                    str_SubChildNodeList = str_ChildNodeList(j).ChildNodes
                                    Dim dNewRow As DataRow = Nothing
                                    dNewRow = dt.NewRow
                                    dNewRow("Section") = Nodename
                                    If (str_SubChildNodeList.Count > 0) Then
                                        For k As Integer = 0 To str_SubChildNodeList.Count - 1
                                            Dim subNodename As String = str_SubChildNodeList(k).Name
                                            Dim subNodevalue As String = str_SubChildNodeList(k).InnerText
                                            dNewRow(subNodename) = subNodevalue
                                        Next
                                        dt.Rows.Add(dNewRow)
                                    End If
                                Next
                            End If
                        ElseIf (Nodename = "UserTagDetails" And (pagename = "siteoverview.aspx" Or pagename = "DeviceList.aspx" Or pagename = "PateintTagList.aspx")) Then

                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                For j As Integer = 0 To str_ChildNodeList.Count - 1
                                    Dim str_SubChildNodeList As XmlNodeList
                                    Dim ChildNd_Name As String = str_ChildNodeList(j).Name
                                    If (ChildNd_Name = thisTypeOnly Or thisTypeOnly = "") Then
                                        str_SubChildNodeList = str_ChildNodeList(j).ChildNodes
                                        If (str_SubChildNodeList.Count > 0) Then
                                            For k As Integer = 0 To str_SubChildNodeList.Count - 1
                                                Dim record_chilNd As XmlNodeList
                                                record_chilNd = str_SubChildNodeList(k).ChildNodes
                                                Dim dNewRow As DataRow = Nothing
                                                dNewRow = dt.NewRow
                                                dNewRow("Section") = Nodename
                                                If (record_chilNd.Count > 0) Then
                                                    For l As Integer = 0 To record_chilNd.Count - 1
                                                        Dim subNodename As String = record_chilNd(l).Name
                                                        Dim subNodevalue As String = record_chilNd(l).InnerText
                                                        dNewRow(subNodename) = subNodevalue
                                                    Next
                                                    dt.Rows.Add(dNewRow)
                                                End If
                                            Next
                                        End If
                                    End If

                                Next
                            End If

                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog(" Glossaryinfo_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        Public Function LoadGlossary(ByVal dt As DataTable) As String
            Dim htmlStr As String = ""
            If (dt.Rows.Count > 0) Then
                For n As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(n))
                        Dim expandstory As String = CheckIsDBNull(.Item("ExpandStory"), False, "")
                        Dim description As String = CheckIsDBNull(.Item("Description"), False, "")

                        htmlStr += "<div class='expandstory'>" & expandstory & "</div>"
                        htmlStr += "<div class='description hide'>" & description & "</div>"

                    End With
                Next
            End If
            Return htmlStr
        End Function
        Public Function siteAlertinfo_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim siteid As String = ""
            Dim sitenamae As String = ""
            Dim addColumn() As String = {"SiteId", "Site", "DeviceType", "AlertId", "AlertCount", "Description", "Status"}
            dt = addColumntoDataTable(addColumn)

            Try


                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count
                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim Alerts_Node As XmlNode
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Dim Nodename As String = Alerts_Node.Name
                        If Alerts_Node.HasChildNodes Then
                            Dim AlertList_Node As XmlNodeList
                            AlertList_Node = Alerts_Node.ChildNodes


                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_node As XmlNodeList
                                Dim Alerts_nodename As String = AlertList_Node(j).Name
                                If (Alerts_nodename = "Alert") Then
                                    Alert_node = AlertList_Node(j).ChildNodes
                                    Dim dNewRow As DataRow = Nothing
                                    dNewRow = dt.NewRow
                                    dNewRow("SiteId") = siteid
                                    dNewRow("Site") = sitenamae

                                    For k As Integer = 0 To Alert_node.Count - 1
                                        Dim Alert_nodename As String = Alert_node(k).Name
                                        Dim Alert_nodeValue As String = Alert_node(k).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
                                    Next
                                    dt.Rows.Add(dNewRow)
                                ElseIf (Alerts_nodename = "SiteId") Then
                                    siteid = AlertList_Node(j).InnerText
                                ElseIf (Alerts_nodename = "Site") Then
                                    sitenamae = AlertList_Node(j).InnerText

                                End If
                            Next
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("siteAlertinfo_XMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function General_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim siteid As String = ""
            Dim sitenamae As String = ""
            Dim addColumn() As String

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim Alerts_Node As XmlNode
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Dim Nodename As String = Alerts_Node.Name
                        If Alerts_Node.HasChildNodes Then
                            Dim AlertList_Node As XmlNodeList
                            AlertList_Node = Alerts_Node.ChildNodes

                            If i = 0 Then
                                ReDim Preserve addColumn(AlertList_Node.Count - 1)
                                'First Add the column name
                                For j As Integer = 0 To AlertList_Node.Count - 1
                                    Dim Alerts_nodename As String = AlertList_Node(j).Name
                                    addColumn(j) = Alerts_nodename
                                Next
                                dt = addColumntoDataTable(addColumn)
                            End If

                            Dim dNewRow As DataRow = Nothing
                            dNewRow = dt.NewRow

                            'Add the data value
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alerts_nodename As String = AlertList_Node(j).Name
                                Dim Alert_nodeValue As String = ""

                                If AlertList_Node(j).ChildNodes.Count > 0 Then
                                    Alert_nodeValue = AlertList_Node(j).ChildNodes.Item(0).InnerText
                                End If

                                dNewRow(Alerts_nodename) = Alert_nodeValue
                            Next

                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("General_XMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function DeviceInfo(ByVal dtMain As DataTable, ByVal dtSecond As DataTable) As DataTable
            Dim dtJoin As New DataTable

            Try
                Dim nColCount As Integer = 0
                Dim nColCount2 As Integer = 0
                Dim fFld As Integer
                Dim nrow As Integer
                Dim NameColum As String

                Dim newColumnIns As DataColumn
                Dim dNewRow As DataRow = Nothing

                nColCount = dtMain.Columns.Count - 1
                nColCount2 = dtSecond.Columns.Count - 1

                For fFld = 0 To nColCount
                    newColumnIns = New DataColumn
                    NameColum = dtMain.Columns.Item(fFld).ColumnName
                    newColumnIns.ColumnName = NameColum
                    newColumnIns.DataType = dtMain.Columns.Item(fFld).DataType
                    dtJoin.Columns.Add(newColumnIns)
                Next

                For fFld = 0 To nColCount2
                    newColumnIns = New DataColumn
                    NameColum = dtSecond.Columns.Item(fFld).ColumnName
                    newColumnIns.ColumnName = NameColum
                    newColumnIns.DataType = dtSecond.Columns.Item(fFld).DataType

                    If Not dtJoin.Columns.Contains(NameColum) Then
                        dtJoin.Columns.Add(newColumnIns)
                    End If
                Next

                For nrow = 0 To dtMain.Rows.Count - 1
                    dNewRow = dtJoin.NewRow

                    For fFld = 0 To nColCount
                        NameColum = dtMain.Columns.Item(fFld).ColumnName
                        dNewRow(NameColum) = dtMain.Rows(0).Item(NameColum)
                    Next

                    For fFld = 0 To nColCount2
                        NameColum = dtSecond.Columns.Item(fFld).ColumnName
                        If NameColum <> "TagType" And NameColum <> "MonitorType" Then
                            dNewRow(NameColum) = dtSecond.Rows(0).Item(NameColum)
                        End If
                    Next

                    dtJoin.Rows.Add(dNewRow)
                Next
            Catch ex As Exception
                WriteLog("General.vb - DeviceInfo " & ex.Message.ToString)
            End Try

            Return dtJoin
        End Function

        Function addColumntoDataTable(ByVal strary() As String) As DataTable
            Dim dt As New DataTable
            Dim count As Integer = strary.Length
            Dim newColumnIns As DataColumn
            For i As Integer = 0 To count - 1
                newColumnIns = Nothing
                newColumnIns = New DataColumn
                newColumnIns.ColumnName = strary(i)
                newColumnIns.DataType = System.Type.GetType("System.String")
                dt.Columns.Add(newColumnIns)
            Next
            Return dt
        End Function

        Function addColumntoDataTableForLAAlerts(ByVal strary() As String) As DataTable
            Dim dt As New DataTable
            Dim count As Integer = strary.Length
            Dim newColumnIns As DataColumn
            For i As Integer = 0 To count - 1
                newColumnIns = Nothing
                newColumnIns = New DataColumn
                newColumnIns.ColumnName = strary(i)
                If strary(i) = "AlertTime" Or strary(i) = "ResolvedOn" Then
                    newColumnIns.DataType = System.Type.GetType("System.DateTime")
                Else
                    newColumnIns.DataType = System.Type.GetType("System.String")
                End If
                dt.Columns.Add(newColumnIns)
            Next
            Return dt
        End Function

        Public Sub GetAuthKey()
            Dim API As New GMSAPI_New.GMSAPI_New
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim xmlreader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing
            Dim responseFromServer As String = ""
            Dim siteUrl As String = ""

            Try
                siteUrl = ConfigurationManager.AppSettings("GMSAPI_New.GMSAPI_New").ToString()
                Dim requestSiteSummary As WebRequest = WebRequest.Create(siteUrl & "/UserAuthentication?UserName=gmsadmin&Password=gmsy3pdz")

                requestSiteSummary.Timeout = 60000
                requestSiteSummary.CachePolicy = New Net.Cache.RequestCachePolicy(Net.Cache.RequestCacheLevel.NoCacheNoStore)

                Dim response As WebResponse = requestSiteSummary.GetResponse()
                Dim dataStream As Stream = response.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                responseFromServer = reader.ReadToEnd()

                reader.Close()
                response.Close()

                xmlreader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                xmlreader.Read()
                doc.Load(xmlreader)

                root = doc.DocumentElement
                AUTHENTICATION_KEY = root.SelectNodes("AuthenticationKey").Item(0).ChildNodes(0).InnerText
            Catch ex As Exception
            End Try
        End Sub

        Public Function GetDeviceList(ByVal nDeviceType As Integer, ByVal sDeviceIds As String) As String

            Dim sDeviceList As String = ""
            Dim strResult As String
            Dim strResult1 As String = ""

            Dim arrSplit As String()

            Dim splitter As Char() = {","}

            Dim i As Integer

            If sDeviceIds <> "" Then

                If nDeviceType = enumDeviceType.Tag Or nDeviceType = enumDeviceType.Monitor Then

                    sDeviceIds = sDeviceIds.Replace(vbCr, "")
                    sDeviceIds = sDeviceIds.Replace(vbLf, "")
                    sDeviceIds = sDeviceIds.Replace(vbCrLf, "")

                    sDeviceIds = Regex.Replace(sDeviceIds, ",,+", ",")

                    If sDeviceIds.EndsWith(",") Then
                        sDeviceIds = sDeviceIds.Remove(sDeviceIds.LastIndexOf(","))
                    End If

                    If sDeviceIds.StartsWith(",") Then
                        sDeviceIds = sDeviceIds.Remove(sDeviceIds.IndexOf(","), 1)
                    End If

                    sDeviceList = sDeviceIds

                ElseIf nDeviceType = enumDeviceType.Star Then

                    sDeviceIds = sDeviceIds.Trim
                    sDeviceIds = sDeviceIds.Replace("'", "")
                    arrSplit = sDeviceIds.Split(splitter)

                    For i = 0 To arrSplit.Length - 1
                        If arrSplit(i).Trim <> "" Then
                            strResult = "'" & arrSplit(i).Trim & "'"
                            strResult1 &= strResult & ","
                        End If
                    Next

                    sDeviceList = strResult1.Remove(strResult1.LastIndexOf(","), 1)

                End If

            End If

            Return sDeviceList

        End Function

        Function getTagImage(ByVal typeid As String, ByVal typ As String, Optional ByVal siteId As String = "", Optional ByVal imgname As String = "", Optional ByVal UIId As Integer = 0) As String

            Dim str As String = ""

            str = "<table cellpadding='0' cellspacing='0' border='0' style='width: 250px;'><tr>"
            str &= "<td style='valign:middle; width: 40px;' align='center'>"
            str &= "<img src=images/DeviceTypes/" & Uri.EscapeDataString(imgname) & " />"
            str &= "</td>"
            str &= "<td style='width: 5px;'></td>"
            str &= "<td style='valign: middle;'>"

            If siteId <> "" Then
                str &= "<a class='clsFilterCriteria' style='text-decoration: none; color: #424242; font-weight: bold;' href=#divPatientTag onclick=loadTagInfoOnClickfromSIteOverviewforAllRpt(" & siteId & "," & typeid & ",'" & typ.Replace(" ", "_") & "'," & UIId & ")>" & typ & "</a>"
            Else
                str &= typ
            End If

            str &= "</td>"
            str &= "</tr></table>"

            Return str

        End Function

        Function getInfraImage(ByVal typeid As String, ByVal typ As String, Optional ByVal siteId As String = "", Optional ByVal imgname As String = "") As String

            Dim str As String = ""

            str = "<table cellpadding='0' cellspacing='0' border='0' style='width: 250px;'><tr>"
            str &= "<td style='valign: middle; width: 40px;' align='center'>"
            str &= "<img src=images/DeviceTypes/" & Uri.EscapeDataString(imgname) & " />"
            str &= "</td>"
            str &= "<td style='width: 5px;'></td>"
            str &= "<td style='valign: middle;'>"

            If siteId <> "" Then
                If typeid <> "" Then
                    str &= "<a class='clsFilterCriteria' style='text-decoration: none; color: #424242; font-weight: bold;' href=#divPatientTag onclick=loadInfraInfoOnClickfromSIteOverviewforAllRpt(" & siteId & "," & typeid & ",'" & typ.Replace(" ", "_") & "')>" & typ & "</a>"
                Else
                    str &= "<a class='clsFilterCriteria' style='text-decoration: none; color: #424242; font-weight: bold;' href=#divPatientTag onclick=loadStarInfoOnClickfromSIteOverviewforAllRpt(" & siteId & ",0,'" & typ.Replace(" ", "_") & "')>" & typ & "</a>"
                End If
            Else
                str &= typ
            End If

            str &= "</td>"
            str &= "</tr></table>"

            Return str

        End Function

        Function getStarImage(ByVal typeid As String, ByVal typ As String, Optional ByVal siteId As String = "", Optional ByVal imgname As String = "") As String

            Dim str As String = ""

            str = "<table cellpadding='0' cellspacing='0' border='0' style='width: 250px;'><tr>"
            str &= "<td style='valign: middle; width: 40px;' align='center'>"
            str &= "<img src=images/DeviceTypes/" & Uri.EscapeDataString(imgname) & " />"
            str &= "</td>"
            str &= "<td style='width: 5px;'></td>"
            str &= "<td style='valign: middle;'>"

            If siteId <> "" Then
                If typeid <> "" Then
                    str &= "<a class='clsFilterCriteria' style='text-decoration: none; color: #424242; font-weight: bold;' href=#divPatientTag onclick=loadStarInfoOnClickfromSIteOverviewforAllRpt(" & siteId & "," & typeid & ",'" & typ.Replace(" ", "_") & "')>" & typ & "</a>"
                Else
                    str &= "<a class='clsFilterCriteria' style='text-decoration: none; color: #424242; font-weight: bold;' href=#divPatientTag onclick=loadStarInfoOnClickfromSIteOverviewforAllRpt(" & siteId & "," & typeid & ",'" & typ.Replace(" ", "_") & "')>" & typ & "</a>"
                End If
            Else
                str &= typ
            End If

            str &= "</td>"
            str &= "</tr></table>"

            Return str

        End Function
	
        Function getUserRole(ByVal roleId As Integer) As String

            Dim str = ""

            Select Case roleId
                Case enumUserRole.Admin
                    str = "Admin"
                Case enumUserRole.Partner
                    str = "Partner"
                Case enumUserRole.Customer
                    str = "Customer"
                Case enumUserRole.Maintenance
                    str = "Maintenance"
                Case enumUserRole.Engineering
                    str = "Engineering"
                Case enumUserRole.TechnicalAdmin
                    str = "Technical Admin"
		Case enumUserRole.Support
		    str = "Support"
            End Select

            Return str
        End Function
   
        Public Function GetSessionExpireDataTable() As DataTable

            Dim dtNew As DataTable
            Dim dr As DataRow

            dtNew = New DataTable
            dtNew.Columns.Add(New DataColumn("AjaxMsg", Type.[GetType]("System.String")))

            dr = dtNew.NewRow
            dr("AjaxMsg") = "Your session has been expired, Please sign-in again."
            dtNew.Rows.Add(dr)

            dtNew.TableName = "AjaxConnectTable"

            Return dtNew

        End Function

        Public Function ActivityXMLNodetoDataTable(ByVal xmlNd As XmlNode, ByVal devicetype As String, ByVal period As String) As DataTable

            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Alerts_nodename As String = ""
            Dim bShowLBIADC As String = ""

            Try

                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                If period = HOURLY Then
		
                    If Val(devicetype) = enumDeviceType.Tag Then
                        Dim addColumn() As String = {"SiteId", "DeviceId", "LBIValue", "LBIDiff", "ReceivedTime", "LocationDataReceived", "PageDataReceived", "WiFiDataCount", "bShowLBIADC", "ADCValue"}
                        dt = addColumntoDataTable(addColumn)
                    ElseIf Val(devicetype) = enumDeviceType.Monitor Then
                        Dim addColumn() As String = {"SiteId", "DeviceId", "LBIValue", "LBIDiff", "ReceivedTime", "LocationDataReceived", "PageDataReceived", "TriggerCount", "WiFiDataCount", "bShowLBIADC", "ADCValue"}
                        dt = addColumntoDataTable(addColumn)
                    ElseIf Val(devicetype) = enumDeviceType.Star Then
                        Dim addColumn() As String = {"SiteId", "MacId", "UpdatedOn", "Locationdatareceived", "Pagedatareceived", "LocationDataCount", "PageDataCount", "bShowLBIADC"}
                        dt = addColumntoDataTable(addColumn)
                    End If
		    
                ElseIf period = DAILY Then
		
                    If Val(devicetype) = enumDeviceType.Tag Then
                        Dim addColumn() As String = {"SiteId", "TagId", "ActivityDate", "LocationCount", "PagingCount", "BatteryValue", "LBIDiff", "WiFiDataCount", "bShowLBIADC", "ADCValue"}
                        dt = addColumntoDataTable(addColumn)
                    ElseIf Val(devicetype) = enumDeviceType.Monitor Then
                        Dim addColumn() As String = {"SiteId", "MonitorId", "ActivityDate", "LocationCount", "PagingCount", "TriggerCount", "Lbi", "WiFiDataCount", "ADCValue", "bShowLBIADC"}
                        dt = addColumntoDataTable(addColumn)
                    End If
		    
                ElseIf period = WEEKLY Then
		
                    If Val(devicetype) = enumDeviceType.Tag Then
                        Dim addColumn() As String = {"SiteId", "TagId", "RYear", "RWeek", "LocationCount", "PagingCount", "LbiValue", "ADCValue", "WiFiDataCount", "LBIDiff", "ReceivedTime", "bShowLBIADC"}
                        dt = addColumntoDataTable(addColumn)
                    ElseIf Val(devicetype) = enumDeviceType.Monitor Then
                        Dim addColumn() As String = {"SiteId", "MonitorId", "RYear", "RWeek", "LocationCount", "PagingCount", "LbiValue", "ADCValue", "WiFiDataCount", "LBIDiff", "ReceivedTime", "bShowLBIADC"}
                        dt = addColumntoDataTable(addColumn)
                    End If
		    
                ElseIf period = MONTHLY Then
		
                    If Val(devicetype) = enumDeviceType.Tag Then
                        Dim addColumn() As String = {"SiteId", "TagId", "RYear", "RMonth", "LocationCount", "PagingCount", "LbiValue", "ADCValue", "WiFiDataCount", "LBIDiff", "ReceivedTime", "bShowLBIADC"}
                        dt = addColumntoDataTable(addColumn)
                    ElseIf Val(devicetype) = enumDeviceType.Monitor Then
                        Dim addColumn() As String = {"SiteId", "MonitorId", "RYear", "RMonth", "LocationCount", "PagingCount", "LbiValue", "ADCValue", "WiFiDataCount", "LBIDiff", "ReceivedTime", "bShowLBIADC"}
                        dt = addColumntoDataTable(addColumn)
                    End If
		    
                End If

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name

                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Tag" Or Nodename = "Monitor" Or Nodename = "Star") Then
                            dNewRow = dt.NewRow
                            dNewRow("SiteId") = siteid
                            dNewRow("bShowLBIADC") = bShowLBIADC

                            For k As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(k).Name
                                Dim Alert_nodeValue As String = AlertList_Node(k).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next

                            dt.Rows.Add(dNewRow)
                        ElseIf (Nodename = "SiteId") Then
                            siteid = AlertList_Node(i).InnerText
                        ElseIf (Nodename = "bShowLBIADC") Then
                            bShowLBIADC = AlertList_Node(0).InnerText
                        End If
			
                    Next
		    
                End If
		
            Catch ex As Exception
                WriteLog("General.vb - ActivityXMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt
	    
        End Function

        Public Function TagMovementXMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Alerts_nodename As String = ""

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"IsMoved", "StartDateTime", "EndDateTime", "RoomIds"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name

                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Movement") Then
                            dNewRow = dt.NewRow
                            For k As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(k).Name
                                Dim Alert_nodeValue As String = AlertList_Node(k).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
		
            Catch ex As Exception
                WriteLog("TagMovementXMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt
	    
        End Function

        Public Function HeartBeatXMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Alerts_nodename As String = ""

            Dim LastUpdated As String = ""
            Dim Localtime As String = ""
            Dim Updated As String = ""
            Dim Freehdspace As String = ""
            Dim Processor As String = ""
            Dim RAM As String = ""
            Dim Systemuptime As String = ""

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Lastupdated", "Localtime", "Updated", "Freehdspace", "Processor", "RAM", "Systemuptime", "ServiceStatus", "AlertHistory", "Servicename", "ServiceId", "Statusason", "Version"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name

                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "ServiceInfo") Then
			
                            dNewRow = dt.NewRow
                            dNewRow("Lastupdated") = LastUpdated
                            dNewRow("Localtime") = Localtime
                            dNewRow("Updated") = Updated
                            dNewRow("Freehdspace") = Freehdspace
                            dNewRow("Processor") = Processor
                            dNewRow("RAM") = RAM
                            dNewRow("Systemuptime") = Systemuptime
			    
                            For k As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(k).Name
                                Dim Alert_nodeValue As String = AlertList_Node(k).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
			    
                            dt.Rows.Add(dNewRow)
			    
                        ElseIf Nodename = "Lastupdated" Then
                            LastUpdated = AlertList_Node(0).InnerText
                        ElseIf Nodename = "Localtime" Then
                            Localtime = AlertList_Node(0).InnerText
                        ElseIf Nodename = "Updated" Then
                            Updated = AlertList_Node(0).InnerText
                        ElseIf Nodename = "Freehdspace" Then
                            Freehdspace = AlertList_Node(0).InnerText
                        ElseIf Nodename = "Processor" Then
                            Processor = AlertList_Node(0).InnerText
                        ElseIf Nodename = "RAM" Then
                            RAM = AlertList_Node(0).InnerText
                        ElseIf Nodename = "Systemuptime" Then
                            Systemuptime = AlertList_Node(0).InnerText
                        End If
			
                    Next
                End If
		
            Catch ex As Exception
                WriteLog("HeartBeatXMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt
	    
        End Function

        Public Function AlertHistoryXMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode
            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim Alerts_nodename As String = ""

            Dim Name As String = ""
            Dim ToTalCount As String = ""

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Name", "ToTalCount", "ComponentId", "DeviceId", "SiteName", "SiteId", "AlertDescription", "AlertTypeId", "AlertId", "AlertTime", "DeviceSubType", "MacId", "AlertDuration", "ResolvedOn", "ResolvedBy", "AlertTimeStr", "ResolvedOnStr"}
                dt = addColumntoDataTableForLAAlerts(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
		    
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name

                        AlertList_Node = Alerts_Node.ChildNodes

                        For j As Integer = 0 To AlertList_Node.Count - 1
                            If (Nodename = "Component") Then
			    
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name

                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If (Sub_Nodename = "Alert") Then
				
                                    dNewRow = dt.NewRow
                                    dNewRow("Name") = Name
                                    dNewRow("ToTalCount") = ToTalCount
				    
                                    For k As Integer = 0 To AlertList_Sub_Node.Count - 1
				    
                                        Dim Alert_nodename As String = AlertList_Sub_Node(k).Name
                                        Dim Alert_nodeValue As String = AlertList_Sub_Node(k).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
					
                                        If Alert_nodename = "AlertTime" Then
                                            dNewRow("AlertTimeStr") = CDate(Alert_nodeValue).ToString("MM/dd/yyyy hh:mm:ss tt")
                                        End If
					
                                        If Alert_nodename = "ResolvedOn" Then
                                            dNewRow("ResolvedOnStr") = CDate(Alert_nodeValue).ToString("MM/dd/yyyy hh:mm:ss tt")
                                        End If
					
                                    Next
				    
                                    dt.Rows.Add(dNewRow)
				    
                                ElseIf Sub_Nodename = "Name" Then
                                    Name = AlertList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "ToTalCount" Then
                                    ToTalCount = AlertList_Sub_Node(0).InnerText
                                End If
				
                            End If
                        Next
                    Next
                End If
		
            Catch ex As Exception
                WriteLog("AlertHistoryXMLNodetoDataTable " & ex.Message.ToString)
            End Try
	    
            dt.DefaultView.Sort = "AlertTime"

            Return dt.DefaultView.ToTable
	    
        End Function

        Public Function LAAlertsForSiteXMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode
            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList
            Dim AlertList_Sub_Node_sub As XmlNodeList
            Dim Alerts_Sub_Node_sub As XmlNode

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim Alerts_nodename As String = ""
            Dim Sub_Nodename_sub As String = ""
            Dim strAlertStartDate As String = ""

            Dim Name As String = ""
            Dim ToTalCount As String = ""
            Dim nAlertsCount As Integer = 0

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Name", "ToTalCount", "ComponentId", "DeviceId", "SiteName", "SiteId", "AlertDescription", "AlertTypeId", "AlertId", "AlertTime", "DeviceSubType", "MacId", "AlertDuration", "ResolvedOn", "ResolvedBy", "AlertStartDate", "AlertTimeInSecs", "ResolvedOnInSecs"}
                dt = addColumntoDataTableForLAAlerts(addColumn)

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name

                        AlertList_Node = Alerts_Node.ChildNodes
			
                        If (Nodename = "AlertLive") Or (Nodename = "AlertHistory") Then
                            nAlertsCount = nAlertsCount + AlertList_Node.Count
                        End If

                        For j As Integer = 0 To AlertList_Node.Count - 1
			
                            If Nodename = "AlertStartDateForSite" Then
                                strAlertStartDate = xmlNd.ChildNodes(i).InnerText
                            End If

                            If (Nodename = "AlertLive") Or (Nodename = "AlertHistory") Then
			    
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name

                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                For k As Integer = 0 To AlertList_Sub_Node.Count - 1
				
                                    If (Sub_Nodename = "Component") Then
				    
                                        Alerts_Sub_Node_sub = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                        Sub_Nodename_sub = Alerts_Sub_Node_sub.Name
                                        AlertList_Sub_Node_sub = Alerts_Sub_Node_sub.ChildNodes

                                        If (Sub_Nodename_sub = "Alert") Then
					
                                            dNewRow = dt.NewRow
                                            dNewRow("Name") = Name
                                            dNewRow("ToTalCount") = ToTalCount
					    
                                            For l As Integer = 0 To AlertList_Sub_Node_sub.Count - 1
					    
                                                Dim Alert_nodename As String = AlertList_Sub_Node_sub(l).Name
                                                Dim Alert_nodeValue As String = AlertList_Sub_Node_sub(l).InnerText
                                                dNewRow(Alert_nodename) = Alert_nodeValue
						
                                                If Alert_nodename = "AlertTime" Then
                                                    dNewRow("AlertTimeInSecs") = CDate(Alert_nodeValue).ToString("MM/dd/yyyy hh:mm:ss tt")
                                                End If
						
                                                If Alert_nodename = "ResolvedOn" Then
                                                    dNewRow("ResolvedOnInSecs") = CDate(Alert_nodeValue).ToString("MM/dd/yyyy hh:mm:ss tt")
                                                End If

                                            Next
					    
                                            If (Nodename = "AlertLive") Then
                                                dNewRow("AlertDuration") = ""
                                                dNewRow("ResolvedOn") = Now
                                                dNewRow("ResolvedOnInSecs") = CDate(Now).ToString("MM/dd/yyyy hh:mm:ss tt")
                                                dNewRow("ResolvedBy") = ""
                                            End If
					    
                                            dNewRow("AlertStartDate") = strAlertStartDate
                                            dt.Rows.Add(dNewRow)
					    
                                        ElseIf Sub_Nodename_sub = "Name" Then
                                            Name = AlertList_Sub_Node(0).InnerText
                                        ElseIf Sub_Nodename_sub = "ToTalCount" Then
                                            ToTalCount = AlertList_Sub_Node(0).InnerText
                                        End If
					
                                    End If
                                Next
                            End If
                        Next
                    Next
                End If

                If nAlertsCount = 0 And strAlertStartDate <> "" Then
		
                    dNewRow = dt.NewRow
                    dNewRow("Name") = ""
                    dNewRow("ToTalCount") = ""
                    dNewRow("ComponentId") = ""
                    dNewRow("DeviceId") = ""
                    dNewRow("SiteName") = ""
                    dNewRow("SiteId") = ""
                    dNewRow("AlertDescription") = ""
                    dNewRow("AlertTypeId") = ""
                    dNewRow("AlertId") = ""
                    dNewRow("AlertTime") = CType(Nothing, DateTime)
                    dNewRow("DeviceSubType") = ""
                    dNewRow("MacId") = ""
                    dNewRow("AlertDuration") = ""
                    dNewRow("ResolvedOn") = CType(Nothing, DateTime)
                    dNewRow("ResolvedBy") = ""
                    dNewRow("AlertStartDate") = strAlertStartDate
                    dNewRow("AlertTimeInSecs") = ""
                    dNewRow("ResolvedOnInSecs") = ""
                    dt.Rows.Add(dNewRow)
		    
                End If
		
            Catch ex As Exception
                WriteLog("AlertHistoryXMLNodetoDataTable " & ex.Message.ToString)
            End Try
	    
            Return dt
	    
        End Function

        Public Function getSeconds(ByVal currentDate As DateTime) As Integer
            Dim startDate As New DateTime(1970, 1, 1)
            Dim noOfSeconds As Integer

            noOfSeconds = (currentDate - startDate).TotalSeconds
        End Function

        Public Function GetUserReportXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Alerts_nodename As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Email", "RecurrencePattern", "ScheduleTime", "WeeklyInterval", "MonthlyInterval", "YearlyInterval1", "YearlyInterval2"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "EmailReportSchedule") Then
                            dNewRow = dt.NewRow
                            For k As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(k).Name
                                Dim Alert_nodeValue As String = AlertList_Node(k).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("TagMovementXMLNodetoDataTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetAlertSettingsXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim siteid As String = ""
            Dim sitename As String = ""
            Dim Nodename As String = ""
            Dim Alerts_nodename As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"SiteId", "SiteName", "Email", "Alert", "PhoneNumber", "AlertType"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "AlertSettings") Then
                            dNewRow = dt.NewRow
                            dNewRow("SiteId") = siteid
                            dNewRow("SiteName") = sitename
                            For k As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(k).Name
                                Dim Alert_nodeValue As String = AlertList_Node(k).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        ElseIf (Nodename = "SiteId") Then
                            siteid = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "SiteName") Then
                            sitename = AlertList_Node(0).InnerText
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetAlertSettingsXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetLocalAlertsForTableViewXMLToTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode
            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList

            Dim siteid As String = ""
            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim Alerts_nodename As String = ""
            Dim strAlertStartDate As String = ""

            Dim Name As String = ""
            Dim ToTalCount As String = ""
            Dim TotalPage As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"AlertStartDateForSite", "TotalPage", "TotalCount", "ComponentId", "DeviceId", "SiteName", "SiteId", "AlertDescription", "AlertTypeId", "AlertId", "AlertTime", "DeviceSubType", "MacId", "AlertDuration", "ResolvedOn", "ResolvedBy"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name

                        AlertList_Node = Alerts_Node.ChildNodes

                        For j As Integer = 0 To AlertList_Node.Count - 1
                            If Nodename = "AlertStartDateForSite" Then
                                strAlertStartDate = xmlNd.ChildNodes(i).InnerText
                            End If

                            If Nodename = "Alerts" Then
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name

                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If (Sub_Nodename = "Alert") Then
                                    dNewRow = dt.NewRow
                                    dNewRow("AlertStartDateForSite") = Name
                                    dNewRow("TotalPage") = TotalPage
                                    dNewRow("TotalCount") = ToTalCount
                                    For k As Integer = 0 To AlertList_Sub_Node.Count - 1
                                        Dim Alert_nodename As String = AlertList_Sub_Node(k).Name
                                        Dim Alert_nodeValue As String = AlertList_Sub_Node(k).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
                                    Next
                                    dt.Rows.Add(dNewRow)
                                ElseIf Sub_Nodename = "AlertStartDateForSite" Then
                                    Name = AlertList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "TotalCount" Then
                                    ToTalCount = AlertList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "TotalPage" Then
                                    TotalPage = AlertList_Sub_Node(0).InnerText
                                End If
                            End If
                        Next
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetLocalAlertsForTableViewXMLToTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function

        Public Function MakeTagMovementChartContent(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            Dim dtChart As New DataTable
            Dim dtFilter As New DataTable
            Dim drChart As DataRow = Nothing

            Dim chartContent As New StringBuilder

            Dim CategoryFromDate As String = ""
            Dim CategoryToDate As String = ""
            Dim TaskLabel As String = ""
            Dim TaskStyles As String = ""

            Dim nDayDiff As Integer = 0
            Dim nDayIdx As Integer = 0

            dtChart.Columns.Add(New DataColumn("XmlData", Type.[GetType]("System.String")))

            Try
                nDayDiff = DateDiff(DateInterval.Day, CDate(FromDate), CDate(ToDate)) + 1

                'Create Chart
                chartContent.Append("<chart manageResize='1' canvasBgColor='F1F1FF, FFFFFF'  canvasBgAngle='90' dateFormat='dd/mm/yyyy' ganttLineColor='0372AB' ganttLineAlpha='8' gridBorderColor='0372AB' canvasBorderColor='0372AB' showShadow='0'>")

                'Add Categories
                chartContent.Append("<categories bgColor='0372AB'>")
                chartContent.Append("<category start='" & FromDate & "' end='" & ToDate & "' label='Tag Movements' fontColor='FFFFFF' /></categories>")

                'Add Sub Categories
                chartContent.Append("<categories bgAlpha='0'>")
                For nDayIdx = 0 To nDayDiff - 1
                    CategoryFromDate = CDate(FromDate).AddDays(nDayIdx)
                    chartContent.Append("<category start='" & CategoryFromDate & "' end='" & CategoryFromDate & "' label='" & CategoryFromDate & "' />")
                Next
                chartContent.Append("</categories>")

                'Process (Styles)
                chartContent.Append("<processes isBold='1' headerbgColor='0372AB' fontColor='0372AB' bgColor='FFFFFF'>")
                chartContent.Append("<process label='Movements' id='A' />")
                chartContent.Append("</processes>")

                'Tasks (Datas)
                chartContent.Append("<tasks>")
                If dt.Rows.Count > 0 Then
                    For nDayIdx = 0 To dt.Rows.Count - 1
                        With dt.Rows(nDayIdx)
                            TaskLabel = "No Movement"
                            TaskStyles = "borderColor='F6BD0F' color='F6BD0F'"
                            If .Item("IsMoved") = 1 Then
                                TaskLabel = "Movement"
                                TaskStyles = "borderColor='FF654F' color='FF654F'"
                            End If

                            chartContent.Append("<task label='" & TaskLabel & "' processId='A' start='" & .Item("StartDateTime") & "' end='" & .Item("EndDateTime") & "' taskId='B' " & TaskStyles & " />")
                        End With
                    Next
                End If
                chartContent.Append("</tasks>")

                'Chart Labels
                chartContent.Append("<connectors><connector fromTaskId='2' toTaskId='1'  color='' alpha='' thickness='' isDotted='' /></connectors>")
                chartContent.Append("<legend><item label='Movement' color='FF654F' /><item label='No Movement' color='F6BD0F' /></legend>")

                'End Chart
                chartContent.Append("</chart>")
            Catch ex As Exception
            End Try

            drChart = dtChart.NewRow
            drChart("XmlData") = chartContent.ToString()
            dtChart.Rows.Add(drChart)

            Return dtChart
        End Function

        Public Function MakeAlertHistoryChartContent(ByVal dt As DataTable) As DataTable
            Dim dtChart As New DataTable
            Dim dtFilter As New DataTable
            Dim drChart As DataRow = Nothing

            Dim chartContent As New StringBuilder

            Dim FromDate As String = ""
            Dim ToDate As String = ""
            Dim CategoryFromDate As String = ""
            Dim CategoryToDate As String = ""
            Dim TaskLabel As String = ""
            Dim TaskStyles As String = ""

            Dim nDayDiff As Integer = 0
            Dim nTimeDiff As Integer = 0
            Dim nDayIdx As Integer = 0
            Dim frmDate As DateTime
            Dim endDate As DateTime

            dtChart.Columns.Add(New DataColumn("XmlData", Type.[GetType]("System.String")))

            Try
                FromDate = dt.Rows(0).Item("AlertTime")
                ToDate = dt.Rows(dt.Rows.Count - 1).Item("ResolvedOn")
                nDayDiff = DateDiff(DateInterval.Day, CDate(FromDate), CDate(ToDate)) + 1

                'Create Chart
                chartContent.Append("<chart manageResize='1' canvasBgColor='F1F1FF, FFFFFF'  canvasBgAngle='90' dateFormat='mm/dd/yyyy hh:mn:ss ampm' ganttLineColor='0372AB' ganttLineAlpha='8' ganttPaneDuration='3' ganttPaneDurationUnit='d' gridBorderColor='0372AB' canvasBorderColor='0372AB' showShadow='0'>")

                'Add Categories
                chartContent.Append("<categories bgColor='0372AB'>")
                chartContent.Append("<category start='" & FromDate & "' end='" & ToDate & "' label='Alert History' fontColor='FFFFFF' /></categories>")

                'Add Sub Categories
                chartContent.Append("<categories bgAlpha='0'>")
                For nDayIdx = 0 To nDayDiff - 1
                    CategoryFromDate = CDate(FromDate).AddDays(nDayIdx)
                    chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date & "' label='" & CDate(CategoryFromDate).Date & "' />")
                Next
                chartContent.Append("</categories>")

                'Process (Styles)
                chartContent.Append("<processes isBold='1' headerbgColor='0372AB' fontColor='0372AB' bgColor='FFFFFF'>")
                chartContent.Append("<process label='Alert History' id='A' />")
                chartContent.Append("</processes>")

                'Tasks (Datas)
                chartContent.Append("<tasks>")
                If dt.Rows.Count > 0 Then
                    For nDayIdx = 0 To dt.Rows.Count - 1
                        With dt.Rows(nDayIdx)
                            TaskLabel = "Alert"
                            TaskStyles = "borderColor='FF654F' color='FF654F'"
                            chartContent.Append("<task label='" & TaskLabel & "' processId='A' start='" & .Item("AlertTime") & "' end='" & .Item("ResolvedOn") & "' taskId='B' " & TaskStyles & " />")

                            If nDayIdx < dt.Rows.Count - 1 Then
                                frmDate = CDate(.Item("ResolvedOn"))
                                endDate = CDate(dt.Rows(nDayIdx + 1).Item("AlertTime"))
                                nTimeDiff = DateDiff(DateInterval.Second, frmDate, endDate)
                                If nTimeDiff > 0 Then
                                    TaskStyles = "borderColor='FF654F' color='8BBA00'"
                                    chartContent.Append("<task label='" & TaskLabel & "' processId='A' start='" & .Item("ResolvedOn") & "' end='" & dt.Rows(nDayIdx + 1).Item("AlertTime") & "' taskId='B' " & TaskStyles & " />")
                                End If
                            End If
                        End With
                    Next
                End If
                chartContent.Append("</tasks>")

                'Chart Labels
                'chartContent.Append("<connectors><connector fromTaskId='1' toTaskId='1'  color='' alpha='' thickness='' isDotted='' /></connectors>")
                chartContent.Append("<legend><item label='Alert' color='FF654F' /></legend>")

                'End Chart
                chartContent.Append("</chart>")
            Catch ex As Exception
            End Try

            drChart = dtChart.NewRow
            drChart("XmlData") = chartContent.ToString()
            dtChart.Rows.Add(drChart)

            Return dtChart
        End Function

        Public Function MakeLocalAlertForServiceChartContent(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal dateType As String, ByVal AlertGraphType As String, ByVal ServiceName As String, Optional ByVal ServiceId As String = "")
            Dim dtChart As New DataTable
            Dim drChart As DataRow = Nothing

            'Columns
            dtChart.Columns.Add(New DataColumn("XmlData_Service", Type.[GetType]("System.String")))
            dtChart.Columns.Add(New DataColumn("StartDate", Type.[GetType]("System.String")))

            'Datas
            drChart = dtChart.NewRow
            drChart("XmlData_Service") = MakeLocalAlertServiceChartContentByServiceId(dt, FromDate, ToDate, dateType, ServiceName, ServiceId)
            drChart("StartDate") = CDate(FromDate).ToString("MM/dd/yyyy")
            dtChart.Rows.Add(drChart)
            Return dtChart
        End Function

        Public Function MakeLocalAlertForSiteChartContent(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal dateType As String, ByVal AlertGraphType As String, Optional ByVal ServiceId As String = "")
            Dim dtChart As New DataTable
            Dim drChart As DataRow = Nothing

            'Columns
            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Service Then
                dtChart.Columns.Add(New DataColumn("XmlData_Service", Type.[GetType]("System.String")))
            End If

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Tag Then
                dtChart.Columns.Add(New DataColumn("XmlData_Tag", Type.[GetType]("System.String")))
            End If

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Monitor Then
                dtChart.Columns.Add(New DataColumn("XmlData_Monitor", Type.[GetType]("System.String")))
            End If

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Star Then
                dtChart.Columns.Add(New DataColumn("XmlData_Star", Type.[GetType]("System.String")))
            End If

            dtChart.Columns.Add(New DataColumn("StartDate", Type.[GetType]("System.String")))

            'Datas
            drChart = dtChart.NewRow

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Service Then
                drChart("XmlData_Service") = MakeLocalAlertServiceChartContent(dt, FromDate, ToDate, dateType, ServiceId)
            End If

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Tag Then
                drChart("XmlData_Tag") = MakeLocalAlertDeviceTypeChartContent(dt, FromDate, ToDate, dateType, enumDeviceType.Tag)
            End If

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Monitor Then
                drChart("XmlData_Monitor") = MakeLocalAlertDeviceTypeChartContent(dt, FromDate, ToDate, dateType, enumDeviceType.Monitor)
            End If

            If AlertGraphType = enumAlertGraphType.All Or AlertGraphType = enumAlertGraphType.Star Then
                drChart("XmlData_Star") = MakeLocalAlertDeviceTypeChartContent(dt, FromDate, ToDate, dateType, enumDeviceType.Star)
            End If

            drChart("StartDate") = CDate(FromDate).ToString("MM/dd/yyyy")
            dtChart.Rows.Add(drChart)

            Return dtChart
        End Function

        Public Function MakeLocalAlertServiceChartContent(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal dateType As String, Optional ByVal ServiceId As String = "") As String
            Dim dtFilter As New DataTable

            Dim chartContent As New StringBuilder
            Dim items As Array

            Dim CategoryFromDate As String = ""
            Dim CategoryToDate As String = ""
            Dim TaskLabel As String = ""
            Dim TaskStyles As String = ""
            Dim item As String
            Dim ServiceName As String = ""

            Dim nDayDiff As Integer = 0
            Dim nTimeDiff As Integer = 0
            Dim nDayIdx As Integer = 0
            Dim chartInterval As String = "d"
            Dim strStartDate As String = ""
            Dim strEndDate As String = ""
            Dim strDateFormat As String = ""
            Dim strOutPutDateFormat As String = ""
            Dim ndtFilterIdx As Integer = 0
            Dim NotAddedContent As Boolean = True
            Dim ChartTitle As String = ""
            Dim toolText As String = ""
            Dim alertStartDate As String = ""

            Try

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        alertStartDate = dt.Rows(0).Item("AlertStartDate")
                    End If
                End If

                If dateType = HOURLY Then
                    chartInterval = "h"
                ElseIf dateType = DAILY Or dateType = WEEKLY Then
                    chartInterval = "d"
                ElseIf dateType = MONTHLY Then
                    chartInterval = "m"
                End If

                If dateType = HOURLY Then
                    nDayDiff = DateDiff(DateInterval.Hour, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "dd/mm/yyyy"
                    strOutPutDateFormat = "hh12:mn ampm"
                ElseIf dateType = DAILY Then
                    nDayDiff = DateDiff(DateInterval.Day, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                ElseIf dateType = WEEKLY Then
                    nDayDiff = 5
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                ElseIf dateType = MONTHLY Then
                    nDayDiff = DateDiff(DateInterval.Month, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                End If

                'Create Chart
                chartContent.Append("<chart canvasBgColor='F5F5F5, FFFFFF' dateFormat='" & strDateFormat & "' outputdateformat='" & strOutPutDateFormat & "' ganttLineColor='0372AB' ganttPaneDurationUnit='" & chartInterval & "' ganttLineAlpha='8'  gridBorderColor='245E90' canvasBorderColor='005695' showShadow='0' forceGanttWidthPercent='0' >")

                'Add Categories
                chartContent.Append("<categories bgColor='005695'>")
                If dateType = HOURLY Then
                    ChartTitle = CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & " - " & CDate(FromDate).AddDays(1).AddSeconds(-1).ToString("HH:mm:ss")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddDays(1).AddSeconds(-1).ToString("HH:mm:ss") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = DAILY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = WEEKLY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = MONTHLY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                End If

                'Add Sub Categories
                chartContent.Append("<categories bgAlpha='0' font='Helvetica' fontSize='11' fontColor='454545' >")
                For nDayIdx = 0 To nDayDiff - 1
                    If dateType = HOURLY Then
                        CategoryFromDate = CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss")
                        CategoryToDate = CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss")
                        chartContent.Append("<category start='" & CategoryFromDate & "' end='" & CategoryToDate & "' label='" & CDate(CategoryFromDate).Hour & "' />")
                    ElseIf dateType = DAILY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date & "' label='" & CDate(CategoryFromDate).Date & "' />")
                    ElseIf dateType = WEEKLY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx * 7)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date.AddDays(7).AddSeconds(-1) & "' label='  " & CDate(CategoryFromDate).Date.ToString("MM/dd") & " - " & CDate(CategoryFromDate).Date.AddDays(7).AddSeconds(-1).ToString("MM/dd") & "  ' />")
                    ElseIf dateType = MONTHLY Then
                        CategoryFromDate = CDate(FromDate).AddMonths(nDayIdx)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date.AddMonths(1).AddSeconds(-1) & "' label='  " & CDate(CategoryFromDate).Date.ToString("MMM yyyy") & "  ' />")
                    End If
                Next
                chartContent.Append("</categories>")

                'Get Enumeration Values 
                items = System.Enum.GetValues(GetType(enumLocalAlertServices))

                'Loop through the services and constrat the Process (Styles)
                chartContent.Append("<processes isBold='1' headerbgColor='005695' font='Helvetica' fontSize='11' fontColor='454545' bgColor='FFFFFF' align='left' headerText='Services' headerFontSize='12' headerVAlign='bottom' headerFont='Helvetica' headerFontColor='FFFFFF'>")
                For Each item In items
                    ServiceName = System.Enum.GetName(GetType(enumLocalAlertServices), CInt(item))

                    If ServiceId = "All" Or ServiceId = ServiceName Then
                        chartContent.Append("<process label='" & ServiceName.Replace("_", " ").Replace("Service", "") & "' id='" & item & "' link='JavaScript:showAlertsInfoforService(""" & CInt(item) & """,""" & ServiceName.Replace("_", " ") & """,""" & FromDate & """,""" & ToDate & """,""" & True & """)' />")
                    End If
                Next
                chartContent.Append("</processes>")

                'Loop through the services and constrat the Tasks (Datas)
                Dim tempCountHearbeat As Integer = 0
                Dim frmtime As String = ""
                Dim totime As String = ""


                If ServiceName = "Heart_Beat" Then
                    dtFilter.Rows.Clear()
                    If ServiceId = "All" Or ServiceId = ServiceName Then
                        dt.DefaultView.RowFilter = "Name='" & ServiceName.Replace("_", " ").ToUpper & "'"
                        dtFilter = dt.DefaultView.ToTable
                        dtFilter.DefaultView.Sort = "AlertTime"
                        If Not dtFilter Is Nothing Then
                            If dtFilter.Rows.Count > 0 Then
                                tempCountHearbeat = 1
                                frmtime = dtFilter.Rows(0).Item("AlertTime")
                                totime = dtFilter.Rows(0).Item("ResolvedOn")
                            End If
                        End If
                    End If
                End If

                Dim isHeaertBeat As String = ""

                chartContent.Append("<tasks>")
                For Each item In items
                    ServiceName = System.Enum.GetName(GetType(enumLocalAlertServices), CInt(item))

                    dtFilter.Rows.Clear()
                    If ServiceId = "All" Or ServiceId = ServiceName Then
                        dt.DefaultView.RowFilter = "Name='" & ServiceName.Replace("_", " ").ToUpper & "'"
                        dtFilter = dt.DefaultView.ToTable
                        dtFilter.DefaultView.Sort = "AlertTime"
                    End If

                    For nDayIdx = 0 To nDayDiff - 1
                        NotAddedContent = True

                        If dateType = HOURLY Then
                            CategoryFromDate = CDate(FromDate).AddHours(nDayIdx).ToString()
                            CategoryToDate = CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1)
                        ElseIf dateType = DAILY Then
                            CategoryFromDate = CDate(FromDate).AddDays(nDayIdx).ToString()
                            CategoryToDate = CDate(CategoryFromDate).AddDays(1).AddSeconds(-1)
                        ElseIf dateType = WEEKLY Then
                            CategoryFromDate = CDate(FromDate).AddDays(nDayIdx * 7).ToString()
                            CategoryToDate = CDate(CategoryFromDate).AddDays(7).AddSeconds(-1)
                        ElseIf dateType = MONTHLY Then
                            CategoryFromDate = CDate(FromDate).AddMonths(nDayIdx).ToString()
                            CategoryToDate = CDate(CategoryFromDate).AddMonths(1).AddSeconds(-1)
                        End If

                        If Not dtFilter Is Nothing Then
                            If dtFilter.Rows.Count > 0 Then
                                With dtFilter.Rows(ndtFilterIdx)
                                    Dim nCount As Integer = dtFilter.Select("AlertTime<='" & CategoryToDate & "' And ResolvedOn>='" & CategoryFromDate & "'").Length
                                    If nCount > 0 Then

                                        If tempCountHearbeat = "1" Then
                                            If CDate(frmtime) <= CDate(CategoryToDate) And CDate(totime) >= CDate(CategoryFromDate) And ServiceName <> "Heart_Beat" Then
                                                TaskStyles = "borderColor='727171' color='727171'"
                                                toolText = ServiceName.Replace("_", " ") & ", Unknown Status"
                                                isHeaertBeat = ""
                                            Else
                                                TaskStyles = "borderColor='FF654F' color='FF654F'"
                                                If nCount > 1 Then
                                                    toolText = ServiceName.Replace("_", " ") & ", " & nCount & " Alerts"
                                                Else
                                                    toolText = ServiceName.Replace("_", " ") & ", " & nCount & " Alert"
                                                End If
                                                isHeaertBeat = "link='JavaScript:showAlertsInfoforService(""" & CInt(item) & """,""" & ServiceName.Replace("_", " ") & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)'"
                                            End If

                                        Else
                                            TaskStyles = "borderColor='FF654F' color='FF654F'"
                                            If nCount > 1 Then
                                                toolText = ServiceName.Replace("_", " ") & ", " & nCount & " Alerts"
                                            Else
                                                toolText = ServiceName.Replace("_", " ") & ", " & nCount & " Alert"
                                            End If
                                            isHeaertBeat = "link='JavaScript:showAlertsInfoforService(""" & CInt(item) & """,""" & ServiceName.Replace("_", " ") & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)'"
                                        End If

                                        If dateType = HOURLY Then
                                            chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss") & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' " & isHeaertBeat & "  />")
                                        ElseIf dateType = DAILY Or dateType = WEEKLY Or dateType = MONTHLY Then
                                            chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' " & isHeaertBeat & " />")
                                        End If

                                        NotAddedContent = False
                                    End If
                                End With
                            End If
                        End If

                        If ServiceId = "All" Or ServiceId = ServiceName Then

                            If tempCountHearbeat = "1" Then
                                If CDate(frmtime) <= CDate(CategoryToDate) And CDate(totime) >= CDate(CategoryFromDate) And ServiceName <> "Heart_Beat" Then
                                    TaskStyles = "borderColor='727171' color='727171'"
                                    isHeaertBeat = ""
                                    toolText = ServiceName.Replace("_", " ") & ",  Unknown Status "
                                Else
                                    TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                    toolText = ServiceName.Replace("_", " ") & ",  No Alerts"
                                    If dateType = HOURLY Then
                                        isHeaertBeat = "link='JavaScript:showNoAlertsInfoforService(""" & ServiceName.Replace("_", " ") & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)'"
                                    Else
                                        isHeaertBeat = "link='JavaScript:showNoAlertsInfoforService(""" & ServiceName.Replace("_", " ") & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)'"
                                    End If
                                End If

                            Else
                                TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                toolText = ServiceName.Replace("_", " ") & ",  No Alerts"
                                If dateType = HOURLY Then
                                    isHeaertBeat = "link='JavaScript:showNoAlertsInfoforService(""" & ServiceName.Replace("_", " ") & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)'"
                                Else
                                    isHeaertBeat = "link='JavaScript:showNoAlertsInfoforService(""" & ServiceName.Replace("_", " ") & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)'"
                                End If
                            End If

                            If dateType = HOURLY Then
                                If NotAddedContent And CDate(FromDate).AddHours(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddHours(nDayIdx) >= CDate(alertStartDate) Then
                                    'TaskStyles = "bordercolor='8bba00' color='8bba00'"
                                    chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss") & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' " & isHeaertBeat & " />")
                                End If
                            ElseIf dateType = DAILY Then
                                If NotAddedContent And CDate(FromDate).AddDays(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddDays(nDayIdx) >= CDate(alertStartDate) Then
                                    'TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                    chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' " & isHeaertBeat & " />")
                                End If
                            ElseIf dateType = WEEKLY Then
                                If NotAddedContent And CDate(FromDate).AddDays(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddDays(nDayIdx) >= CDate(alertStartDate).AddDays(-6) Then
                                    'TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                    chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' " & isHeaertBeat & " />")
                                End If
                            ElseIf dateType = MONTHLY Then
                                If NotAddedContent And CDate(FromDate).AddMonths(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddMonths(nDayIdx) >= CDate(alertStartDate).AddDays(-(CDate(alertStartDate).Day) + 1) Then
                                    'TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                    chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' " & isHeaertBeat & "  />")
                                End If
                            End If
                        End If
                    Next
                Next
                chartContent.Append("</tasks>")

                'End Chart
                chartContent.Append("</chart>")
            Catch ex As Exception
            End Try

            Return chartContent.ToString()
        End Function

        Public Function MakeLocalAlertServiceChartContentByServiceId(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal dateType As String, ByVal ServiceName As String, Optional ByVal ServiceId As String = "") As String
            Dim dtFilter As New DataTable

            Dim chartContent As New StringBuilder

            Dim CategoryFromDate As String = ""
            Dim CategoryToDate As String = ""
            Dim TaskLabel As String = ""
            Dim TaskStyles As String = ""
            Dim item As String

            Dim nDayDiff As Integer = 0
            Dim nTimeDiff As Integer = 0
            Dim nDayIdx As Integer = 0
            Dim chartInterval As String = "d"
            Dim strStartDate As String = ""
            Dim strEndDate As String = ""
            Dim strDateFormat As String = ""
            Dim strOutPutDateFormat As String = ""
            Dim ndtFilterIdx As Integer = 0
            Dim NotAddedContent As Boolean = True
            Dim ChartTitle As String = ""
            Dim toolText As String = ""

            Try
                If dateType = HOURLY Then
                    chartInterval = "h"
                ElseIf dateType = DAILY Or dateType = WEEKLY Then
                    chartInterval = "d"
                ElseIf dateType = MONTHLY Then
                    chartInterval = "m"
                End If

                If dateType = HOURLY Then
                    nDayDiff = DateDiff(DateInterval.Hour, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "dd/mm/yyyy"
                    strOutPutDateFormat = "hh12:mn ampm"
                ElseIf dateType = DAILY Then
                    nDayDiff = DateDiff(DateInterval.Day, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                ElseIf dateType = WEEKLY Then
                    nDayDiff = 5
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                ElseIf dateType = MONTHLY Then
                    nDayDiff = DateDiff(DateInterval.Month, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                End If

                'Create Chart
                chartContent.Append("<chart canvasBgColor='F5F5F5, FFFFFF' dateFormat='" & strDateFormat & "' outputdateformat='" & strOutPutDateFormat & "' ganttLineColor='0372AB' ganttPaneDurationUnit='" & chartInterval & "' ganttLineAlpha='8'  gridBorderColor='245E90' canvasBorderColor='005695' showShadow='0' forceGanttWidthPercent='0' >")

                'Add Categories
                chartContent.Append("<categories bgColor='005695'>")
                If dateType = HOURLY Then
                    ChartTitle = CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & " - " & CDate(FromDate).AddDays(1).AddSeconds(-1).ToString("HH:mm:ss")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddDays(1).AddSeconds(-1).ToString("HH:mm:ss") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = DAILY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = WEEKLY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = MONTHLY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                End If

                'Add Sub Categories
                chartContent.Append("<categories bgAlpha='0' font='Helvetica' fontSize='11' fontColor='454545' >")
                For nDayIdx = 0 To nDayDiff - 1
                    If dateType = HOURLY Then
                        CategoryFromDate = CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss")
                        CategoryToDate = CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss")
                        chartContent.Append("<category start='" & CategoryFromDate & "' end='" & CategoryToDate & "' label='" & CDate(CategoryFromDate).Hour & "' />")
                    ElseIf dateType = DAILY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date & "' label='" & CDate(CategoryFromDate).Date & "' />")
                    ElseIf dateType = WEEKLY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx * 7)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date.AddDays(7).AddSeconds(-1) & "' label='  " & CDate(CategoryFromDate).Date.ToString("MM/dd") & " - " & CDate(CategoryFromDate).Date.AddDays(7).AddSeconds(-1).ToString("MM/dd") & "  ' />")
                    ElseIf dateType = MONTHLY Then
                        CategoryFromDate = CDate(FromDate).AddMonths(nDayIdx)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date.AddMonths(1).AddSeconds(-1) & "' label='  " & CDate(CategoryFromDate).Date.ToString("MMM yyyy") & "  ' />")
                    End If
                Next
                chartContent.Append("</categories>")

                'Loop through the services and constrat the Process (Styles)
                chartContent.Append("<processes isBold='1' headerbgColor='005695' font='Helvetica' fontSize='11' fontColor='454545' bgColor='FFFFFF' align='left' headerText='Services' headerFontSize='12' headerVAlign='bottom' headerFont='Helvetica' headerFontColor='FFFFFF'>")
                chartContent.Append("<process label='" & ServiceName.Replace("_", " ").Replace("Service", "") & "' id='" & item & "' />")
                chartContent.Append("</processes>")

                'Loop through the services and constrat the Tasks (Datas)
                chartContent.Append("<tasks>")
                dtFilter.Rows.Clear()
                dtFilter = dt.DefaultView.ToTable
                dtFilter.DefaultView.Sort = "AlertTime"

                For nDayIdx = 0 To nDayDiff - 1
                    NotAddedContent = True

                    If dateType = HOURLY Then
                        CategoryFromDate = CDate(FromDate).AddHours(nDayIdx).ToString()
                        CategoryToDate = CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1)
                    ElseIf dateType = DAILY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx).ToString()
                        CategoryToDate = CDate(CategoryFromDate).AddDays(1).AddSeconds(-1)
                    ElseIf dateType = WEEKLY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx * 7).ToString()
                        CategoryToDate = CDate(CategoryFromDate).AddDays(7).AddSeconds(-1)
                    ElseIf dateType = MONTHLY Then
                        CategoryFromDate = CDate(FromDate).AddMonths(nDayIdx).ToString()
                        CategoryToDate = CDate(CategoryFromDate).AddMonths(1).AddSeconds(-1)
                    End If

                    If Not dtFilter Is Nothing Then
                        If dtFilter.Rows.Count > 0 Then
                            With dtFilter.Rows(ndtFilterIdx)
                                Dim nCount As Integer = dtFilter.Select("AlertTime<='" & CategoryToDate & "' And ResolvedOn>='" & CategoryFromDate & "'").Length
                                If nCount > 0 Then
                                    TaskStyles = "borderColor='FF654F' color='FF654F'"

                                    If nCount > 1 Then
                                        toolText = ServiceName.Replace("_", " ") & ", " & nCount & " Alerts"
                                    Else
                                        toolText = ServiceName.Replace("_", " ") & ", " & nCount & " Alert"
                                    End If

                                    If dateType = HOURLY Then
                                        chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss") & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' />")
                                    ElseIf dateType = DAILY Or dateType = WEEKLY Or dateType = MONTHLY Then
                                        chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' />")
                                    End If

                                    NotAddedContent = False
                                End If
                            End With
                        End If
                    End If

                    toolText = ServiceName.Replace("_", " ") & ",  No Alerts"

                    If dateType = HOURLY Then
                        If NotAddedContent And CDate(FromDate).AddHours(nDayIdx) <= Now Then
                            TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                            chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss") & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' />")
                        End If
                    ElseIf dateType = DAILY Or dateType = WEEKLY Or dateType = MONTHLY Then
                        If NotAddedContent And CDate(FromDate).AddDays(nDayIdx) <= Now Then
                            TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                            chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' />")
                        End If
                    End If
                Next
                chartContent.Append("</tasks>")

                'End Chart
                chartContent.Append("</chart>")
            Catch ex As Exception
            End Try

            Return chartContent.ToString()
        End Function

        Public Function MakeLocalAlertDeviceTypeChartContent(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal dateType As String, ByVal DeviceType As enumDeviceType) As String
            Dim dtFilter As New DataTable
            Dim dtSubFilter As New DataTable

            Dim chartContent As New StringBuilder
            Dim items As Array

            Dim CategoryFromDate As String = ""
            Dim CategoryToDate As String = ""
            Dim TaskLabel As String = ""
            Dim TaskStyles As String = ""
            Dim item As String = ""
            Dim ServiceName As String = ""

            Dim nDayDiff As Integer = 0
            Dim nTimeDiff As Integer = 0
            Dim nDayIdx As Integer = 0
            Dim chartInterval As String = "d"
            Dim strStartDate As String = ""
            Dim strEndDate As String = ""
            Dim strDateFormat As String = ""
            Dim strOutPutDateFormat As String = ""
            Dim ndtFilterIdx As Integer = 0
            Dim ServiceId As Integer = 0
            Dim NotAddedContent As Boolean = True
            Dim ChartTitle As String = ""
            Dim toolText As String = ""
            Dim sName As String = ""
            Dim alertStartDate As String = ""

            Try
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        alertStartDate = dt.Rows(0).Item("AlertStartDate")
                    End If
                End If

                If dateType = HOURLY Then
                    chartInterval = "h"
                ElseIf dateType = DAILY Or dateType = WEEKLY Then
                    chartInterval = "d"
                ElseIf dateType = MONTHLY Then
                    chartInterval = "m"
                End If

                If dateType = HOURLY Then
                    nDayDiff = DateDiff(DateInterval.Hour, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "dd/mm/yyyy"
                    strOutPutDateFormat = "hh12:mn ampm"
                ElseIf dateType = DAILY Then
                    nDayDiff = DateDiff(DateInterval.Day, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                ElseIf dateType = WEEKLY Then
                    nDayDiff = 5
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                ElseIf dateType = MONTHLY Then
                    nDayDiff = DateDiff(DateInterval.Month, CDate(FromDate), CDate(ToDate)) + 1
                    strDateFormat = "mm/dd/yyyy"
                    strOutPutDateFormat = "mm/dd/yyyy hh:mn:ss ampm"
                End If

                'Create Chart
                chartContent.Append("<chart canvasBgColor='F5F5F5, FFFFFF' dateFormat='" & strDateFormat & "' outputdateformat='" & strOutPutDateFormat & "' ganttLineColor='0372AB' ganttPaneDurationUnit='" & chartInterval & "' ganttLineAlpha='8'  gridBorderColor='245E90' canvasBorderColor='005695' showShadow='0' forceGanttWidthPercent='0' >")

                'Add Categories
                chartContent.Append("<categories bgColor='005695'>")
                If dateType = HOURLY Then
                    ChartTitle = CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & " - " & CDate(FromDate).AddDays(1).AddSeconds(-1).ToString("HH:mm:ss")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddDays(1).AddSeconds(-1).ToString("HH:mm:ss") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = DAILY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = WEEKLY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                ElseIf dateType = MONTHLY Then
                    ChartTitle = CDate(FromDate).ToString("MM/dd/yyyy") & " - " & CDate(ToDate).ToString("MM/dd/yyyy")
                    chartContent.Append("<category start='" & CDate(FromDate).AddHours(nDayIdx).ToString("MM/dd/yyyy") & "' end='" & CDate(ToDate).ToString("MM/dd/yyyy") & "' label='" & ChartTitle & "' fontColor='FFFFFF' font='Helvetica' fontSize='12'/></categories>")
                End If

                'Add Sub Categories
                chartContent.Append("<categories bgAlpha='0' font='Helvetica' fontSize='11' fontColor='454545' >")
                For nDayIdx = 0 To nDayDiff - 1
                    If dateType = HOURLY Then
                        CategoryFromDate = CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss")
                        CategoryToDate = CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss")
                        chartContent.Append("<category start='" & CategoryFromDate & "' end='" & CategoryToDate & "' label='" & CDate(CategoryFromDate).Hour & "' />")
                    ElseIf dateType = DAILY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date & "' label='" & CDate(CategoryFromDate).Date & "' />")
                    ElseIf dateType = WEEKLY Then
                        CategoryFromDate = CDate(FromDate).AddDays(nDayIdx * 7)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date.AddDays(7).AddSeconds(-1) & "' label='  " & CDate(CategoryFromDate).Date.ToString("MM/dd") & " - " & CDate(CategoryFromDate).Date.AddDays(7).AddSeconds(-1).ToString("MM/dd") & "  ' />")
                    ElseIf dateType = MONTHLY Then
                        CategoryFromDate = CDate(FromDate).AddMonths(nDayIdx)
                        chartContent.Append("<category start='" & CDate(CategoryFromDate).Date & "' end='" & CDate(CategoryFromDate).Date.AddMonths(1).AddSeconds(-1) & "' label='  " & CDate(CategoryFromDate).Date.ToString("MMM yyyy") & "  ' />")
                    End If
                Next
                chartContent.Append("</categories>")

                'Get Enumeration Values 
                If DeviceType = enumDeviceType.Tag Then
                    items = System.Enum.GetValues(GetType(enumTagAlert))
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    items = System.Enum.GetValues(GetType(enumMonitorAlert))
                ElseIf DeviceType = enumDeviceType.Star Then
                    items = System.Enum.GetValues(GetType(enumStarAlert))
                End If

                'Loop through the services and constrat the Process (Styles)
                chartContent.Append("<processes isBold='1' headerbgColor='005695' font='Helvetica' fontSize='11' fontColor='454545' bgColor='FFFFFF' align='left' headerText='Services' headerFontSize='12' headerVAlign='bottom' headerFont='Helvetica' headerFontColor='FFFFFF'>")
                For Each item In items
                    If DeviceType = enumDeviceType.Tag Then
                        ServiceName = System.Enum.GetName(GetType(enumTagAlert), CInt(item))
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        ServiceName = System.Enum.GetName(GetType(enumMonitorAlert), CInt(item))
                    ElseIf DeviceType = enumDeviceType.Star Then
                        ServiceName = System.Enum.GetName(GetType(enumStarAlert), CInt(item))
                    End If
                    chartContent.Append("<process label='" & ServiceName.Replace("_", " ").Replace("Service", "") & "' id='" & item & "' link='JavaScript:showAlertsInfo(""" & CInt(item) & """,""" & GetLAAlertDescription(ServiceName, DeviceType) & """,""" & FromDate & """,""" & ToDate & """,""" & True & """,""" & DeviceType & """)' />")
                Next
                chartContent.Append("</processes>")

                'Loop through the services and constrat the Tasks (Datas)
                chartContent.Append("<tasks>")
                For Each item In items
                    If DeviceType = enumDeviceType.Tag Then
                        ServiceName = System.Enum.GetName(GetType(enumTagAlert), CInt(item))
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        ServiceName = System.Enum.GetName(GetType(enumMonitorAlert), CInt(item))
                    ElseIf DeviceType = enumDeviceType.Star Then
                        ServiceName = System.Enum.GetName(GetType(enumStarAlert), CInt(item))
                    End If
                    ServiceId = CInt(item)

                    sName = GetLAAlertDescription(ServiceName, DeviceType)

                    dt.DefaultView.RowFilter = "AlertTypeId='" & ServiceId & "'"
                    dtFilter = dt.DefaultView.ToTable
                    dtFilter.DefaultView.Sort = "AlertTime"

                    For nDayIdx = 0 To nDayDiff - 1
                        NotAddedContent = True

                        If dateType = HOURLY Then
                            CategoryFromDate = CDate(FromDate).AddHours(nDayIdx).ToString()
                            CategoryToDate = CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1)
                        ElseIf dateType = DAILY Then
                            CategoryFromDate = CDate(FromDate).AddDays(nDayIdx).ToString()
                            CategoryToDate = CDate(CategoryFromDate).AddDays(1).AddSeconds(-1)
                        ElseIf dateType = WEEKLY Then
                            CategoryFromDate = CDate(FromDate).AddDays(nDayIdx * 7).ToString()
                            CategoryToDate = CDate(CategoryFromDate).AddDays(7).AddSeconds(-1)
                        ElseIf dateType = MONTHLY Then
                            CategoryFromDate = CDate(FromDate).AddMonths(nDayIdx).ToString()
                            CategoryToDate = CDate(CategoryFromDate).AddMonths(1).AddSeconds(-1)
                        End If

                        If Not dtFilter Is Nothing Then
                            If dtFilter.Rows.Count > 0 Then
                                With dtFilter.Rows(ndtFilterIdx)
                                    Dim nCount As Integer = dtFilter.Select("AlertTime<='" & CategoryToDate & "' And ResolvedOn>='" & CategoryFromDate & "'").Length
                                    If nCount > 0 Then
                                        TaskStyles = "borderColor='FF654F' color='FF654F'"
                                        If nCount > 1 Then
                                            toolText = sName & ", " & nCount & " Alerts"
                                        Else
                                            toolText = sName & ", " & nCount & " Alert"
                                        End If

                                        If dateType = HOURLY Then
                                            chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss") & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' link='JavaScript:showAlertsInfo(""" & CInt(item) & """,""" & sName & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """,""" & DeviceType & """)' />")
                                        ElseIf dateType = DAILY Or dateType = WEEKLY Or dateType = MONTHLY Then
                                            chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' link='JavaScript:showAlertsInfo(""" & CInt(item) & """,""" & sName & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """,""" & DeviceType & """)' />")
                                        End If

                                        NotAddedContent = False
                                    End If
                                End With
                            End If
                        End If

                        toolText = sName & ", No Alerts"

                        If dateType = HOURLY Then
                            If NotAddedContent And CDate(FromDate).AddHours(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddHours(nDayIdx) >= CDate(alertStartDate) Then
                                TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CDate(FromDate).AddHours(nDayIdx).ToString("HH:mm:ss") & "' end='" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1).ToString("HH:mm:ss") & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' link='JavaScript:showNoAlertsInfo(""" & sName & """,""" & CDate(FromDate).AddHours(nDayIdx) & """,""" & CDate(FromDate).AddHours(nDayIdx + 1).AddSeconds(-1) & """,""" & False & """)' />")
                            End If
                        ElseIf dateType = DAILY Then
                            If NotAddedContent And CDate(FromDate).AddDays(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddDays(nDayIdx) >= CDate(alertStartDate) Then
                                TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' link='JavaScript:showNoAlertsInfo(""" & sName & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)' />")
                            End If
                        ElseIf dateType = WEEKLY Then
                            If NotAddedContent And CDate(FromDate).AddDays(nDayIdx * 7) <= Now And alertStartDate <> "" And CDate(FromDate).AddDays(nDayIdx * 7) >= CDate(alertStartDate).AddDays(-CDate(alertStartDate).DayOfWeek) Then
                                TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' link='JavaScript:showNoAlertsInfo(""" & sName & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)' />")
                            End If
                        ElseIf dateType = MONTHLY Then
                            If NotAddedContent And CDate(FromDate).AddMonths(nDayIdx) <= Now And alertStartDate <> "" And CDate(FromDate).AddMonths(nDayIdx) >= CDate(alertStartDate).AddDays(-(CDate(alertStartDate).Day) + 1) Then
                                TaskStyles = "borderColor='8BBA00' color='8BBA00'"
                                chartContent.Append("<task label='" & ServiceName.Replace("_", " ") & "' processId='" & CInt(item) & "' start='" & CategoryFromDate & "' end='" & CategoryToDate & "' taskId='B' " & TaskStyles & " tooltext='" & toolText & "' link='JavaScript:showNoAlertsInfo(""" & sName & """,""" & CategoryFromDate & """,""" & CategoryToDate & """,""" & False & """)' />")
                            End If
                        End If
                    Next
                Next
                chartContent.Append("</tasks>")

                'End Chart
                chartContent.Append("</chart>")
            Catch ex As Exception
            End Try

            Return chartContent.ToString()
        End Function

        Function GetLAAlertDescription(ByVal msg As String, ByVal type As String) As String
            Dim str As String = ""
            str = msg
            If msg = "Regular_Tag" And type = "1" Then
                str = "Tag not reporting / missing (Regular Tag)"
            ElseIf msg = "No_Sleep_Tag" And type = "1" Then
                str = "Tag not reporting / missing (No Sleep Tag)"
            ElseIf msg = "IR_Profile_Conflict" And type = "1" Then
                str = "IR Profile conflict: All tags are not in same IR Profile"
            ElseIf msg = "Active_Count_Less" And type = "1" Then
                str = "Active Tags Count less than the allowed percentage"
            ElseIf msg = "Not_Reporting" And type = "2" Then
                str = "Monitor not reporting / missing"
            ElseIf msg = "Not_Seen" And type = "2" Then
                str = "Monitor not seen by any Tags"
            ElseIf msg = "Too_Many_Reports" And type = "2" Then
                str = "Monitor Reports too many times due to Wakeup / Paging"
            ElseIf msg = "DIM_Too_Many_Reports" And type = "2" Then
                str = "DIM Reports too many times due to Wakeup / Paging"
            ElseIf msg = "DIM_Trigger" And type = "2" Then
                str = "DIM Reports too many times due to Trigger"
            ElseIf msg = "Active_Count_Less" And type = "2" Then
                str = "Active Monitor Count less than the allowed percentage"
            ElseIf msg = "Not_Seen" And type = "3" Then
                str = "Star does not see any Devices"
            ElseIf msg = "Not_Associated" And type = "3" Then
                str = "Star does not associated with any Devices"
            ElseIf msg = "In_Active" And type = "3" Then
                str = "Star is InActive"
            ElseIf msg = "Ethernet_Over_Limit" And type = "3" Then
                str = "Star exits ethernet offset over limit"
            ElseIf msg = "Non_Sync_TimeServer" And type = "3" Then
                str = "Star non sync with Timeserver"
            ElseIf msg = "Association_Changed" And type = "3" Then
                str = "Star association changed frequently"
            ElseIf msg = "Non_Sync" And type = "3" Then
                str = "One or more Stars in the network is in NON SYNC"
            ElseIf msg = "Beacon_InActive" And type = "3" Then
                str = "Beacon Star inactive"
            ElseIf msg = "All_InActive" And type = "3" Then
                str = "All Stars are inactive"
            ElseIf msg = "Active_Count_Less" And type = "3" Then
                str = "Active Star Count less than the allowed percentage"

            End If


            Return str


        End Function

        Public Sub SiteDeviceList_XMLNodetoDataTable(ByVal xmlNd As XmlNode, ByRef dtTag As DataTable, ByRef dtMonitor As DataTable)
            Dim dtRef As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim str_xmlchildnode As XmlNode
            Dim str_ChildNodeList As XmlNodeList
            Dim Nodename As String

            Dim nSiteId As String = ""
            Dim SiteName As String = ""
            Dim totalPage As String = ""
            Dim TotalCount As String = ""
            Dim subNodeName As String = ""
            Dim subNodevalue As String = ""

            'Add Tag Columns
            dtTag = GetDeviceListCols(enumDeviceType.Tag)

            'Add Monitor Columns   
            dtMonitor = GetDeviceListCols(enumDeviceType.Monitor)

            Try
                Dim cnt As Long = xmlNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Nodename = str_xmlchildnode.Name

                        If (Nodename.ToLower = "tag" Or Nodename.ToLower = "monitor" Or Nodename.ToLower = "star") Then
                            If str_xmlchildnode.HasChildNodes Then
                                str_ChildNodeList = str_xmlchildnode.ChildNodes

                                If Nodename.ToLower = "tag" Then
                                    dtRef = dtTag
                                ElseIf Nodename.ToLower = "monitor" Then
                                    dtRef = dtMonitor
                                End If

                                dNewRow = dtRef.NewRow
                                dNewRow("UserRole") = g_UserRole
                                dNewRow("SiteId") = nSiteId
                                dNewRow("SiteName") = SiteName
                                dNewRow("TotalPage") = totalPage
                                dNewRow("TotalCount") = TotalCount

                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    subNodeName = str_ChildNodeList(n).Name
                                    subNodevalue = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next

                                dtRef.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText

                            If (Nodename.ToLower = "siteid") Then
                                nSiteId = Nodevalue
                            ElseIf (Nodename.ToLower = "sitename") Then
                                SiteName = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalPage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                TotalCount = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" SiteDeviceLsit_XMLNodetoDataTable " & ex.Message.ToString())
            End Try
        End Sub


        Public Sub SiteDeviceListforAllDevice_XMLNodetoDataTable(ByVal xmlNd As XmlNode, ByRef dtTag As DataTable, ByRef dtMonitor As DataTable, Optional ByRef dtStar As DataTable = Nothing)
            Dim dtRef As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim str_xmlchildnode As XmlNode
            Dim str_xmlchildnode1 As XmlNode
            Dim str_ChildNodeList As XmlNodeList
            Dim str_ChildNodeList1 As XmlNodeList
            Dim Nodename As String
            Dim Nodename1 As String

            Dim nSiteId As String = ""
            Dim SiteName As String = ""
            Dim totalPage As String = ""
            Dim TotalCount As String = ""
            Dim subNodeName As String = ""
            Dim subNodevalue As String = ""

            'Add Tag Columns
            Dim addColumnTag() As String = {"TagId", "MonitorId", "TagType"}
            dtTag = addColumntoDataTable(addColumnTag)

            'Add Monitor Columns
            Dim addColumnMonitor() As String = {"DeviceId", "RoomName", "MonitorType"}
            dtMonitor = addColumntoDataTable(addColumnMonitor)

            'Add Star Columns
            Dim addColumnStar() As String = {"MacId"}
            dtStar = addColumntoDataTable(addColumnStar)

            Try
                Dim cnt As Long = xmlNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Nodename = str_xmlchildnode.Name

                        If (Nodename.ToLower = "tags" Or Nodename.ToLower = "monitors" Or Nodename.ToLower = "stars") Then
                            If str_xmlchildnode.HasChildNodes Then
                                str_ChildNodeList = str_xmlchildnode.ChildNodes

                                For j As Integer = 0 To str_ChildNodeList.Count - 1
                                    str_xmlchildnode1 = xmlNd.ChildNodes(i).ChildNodes(j)
                                    Nodename1 = str_xmlchildnode1.Name
                                    str_ChildNodeList1 = str_xmlchildnode1.ChildNodes

                                    If Nodename1.ToLower = "tag" Then
                                        dNewRow = dtTag.NewRow()
                                    ElseIf Nodename1.ToLower = "monitor" Then
                                        dNewRow = dtMonitor.NewRow()
                                    ElseIf Nodename1.ToLower = "star" Then
                                        dNewRow = dtStar.NewRow()
                                    End If

                                    For n As Integer = 0 To str_ChildNodeList1.Count - 1
                                        subNodeName = str_ChildNodeList1(n).Name
                                        subNodevalue = str_ChildNodeList1(n).InnerText
                                        dNewRow(subNodeName) = subNodevalue
                                    Next

                                    If Nodename1.ToLower = "tag" Then
                                        dtTag.Rows.Add(dNewRow)
                                    ElseIf Nodename1.ToLower = "monitor" Then
                                        dtMonitor.Rows.Add(dNewRow)
                                    ElseIf Nodename1.ToLower = "star" Then
                                        dtStar.Rows.Add(dNewRow)
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog(" SiteDeviceLsit_XMLNodetoDataTable " & ex.Message.ToString())
            End Try
        End Sub

        Public Sub SiteDeviceListPrint_XMLNodetoDataTable(ByVal xmlNd As XmlNode, ByRef dtTag As DataTable, ByRef dtMonitor As DataTable, ByRef bNoRecordFound As Boolean)
            Dim dNewRow As DataRow = Nothing

            Dim str_xmlchildnode As XmlNode
            Dim Nodename As String = ""
            Dim Nodevalue As String = ""

            Dim nSiteId As String = ""
            Dim SiteName As String = ""
            Dim totalPage As String = ""
            Dim TotalCount As String = ""
            Dim subNodeName As String = ""
            Dim subNodevalue As String = ""
            Dim bCnt As Boolean = True

            If xmlNd.Item("Tag") Is Nothing And xmlNd.Item("Monitor") Is Nothing Then
                bCnt = False
                bNoRecordFound = True
            End If

            If bCnt Then
                SiteDeviceList_XMLNodetoDataTable(xmlNd, dtTag, dtMonitor)
                Exit Sub
            End If

            'Add Tag Columns
            Dim addColumn() As String = {"SiteID", "Sitename"}
            dtTag = addColumntoDataTable(addColumn)

            Try
                Dim cnt As Long = xmlNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Nodename = str_xmlchildnode.Name
                        Nodevalue = str_xmlchildnode.InnerText

                        If (Nodename.ToLower = "siteid") Then
                            nSiteId = Nodevalue
                        ElseIf (Nodename.ToLower = "sitename") Then
                            SiteName = Nodevalue
                        End If
                    Next

                    dNewRow = dtTag.NewRow
                    dNewRow("SiteID") = nSiteId
                    dNewRow("SiteName") = SiteName
                    dtTag.Rows.Add(dNewRow)
                End If
            Catch ex As Exception
                WriteLog(" SiteDeviceLsit_XMLNodetoDataTable " & ex.Message.ToString())
            End Try
        End Sub


        Public Sub SiteAllDeviceListPrint_XMLNodetoDataTable(ByVal xmlNd As XmlNode, ByRef dtTag As DataTable, ByRef dtMonitor As DataTable, ByRef dtStar As DataTable, ByRef bNoRecordFound As Boolean)
            Dim dNewRow As DataRow = Nothing

            Dim str_xmlchildnode As XmlNode
            Dim Nodename As String = ""
            Dim Nodevalue As String = ""

            Dim nSiteId As String = ""
            Dim SiteName As String = ""
            Dim totalPage As String = ""
            Dim TotalCount As String = ""
            Dim subNodeName As String = ""
            Dim subNodevalue As String = ""
            Dim bCnt As Boolean = True

            If xmlNd.Item("Tags") Is Nothing And xmlNd.Item("Monitors") Is Nothing And xmlNd.Item("Stars") Is Nothing Then
                bCnt = False
                bNoRecordFound = True
            End If

            If bCnt Then
                SiteDeviceListforAllDevice_XMLNodetoDataTable(xmlNd, dtTag, dtMonitor, dtStar)
                Exit Sub
            End If

            'Add Tag Columns
            Dim addColumn() As String = {"SiteID", "Sitename"}
            dtTag = addColumntoDataTable(addColumn)

            Try
                Dim cnt As Long = xmlNd.ChildNodes.Count

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Nodename = str_xmlchildnode.Name
                        Nodevalue = str_xmlchildnode.InnerText

                        If (Nodename.ToLower = "siteid") Then
                            nSiteId = Nodevalue
                        ElseIf (Nodename.ToLower = "sitename") Then
                            SiteName = Nodevalue
                        End If
                    Next

                    dNewRow = dtTag.NewRow
                    dNewRow("SiteID") = nSiteId
                    dNewRow("SiteName") = SiteName
                    dtTag.Rows.Add(dNewRow)
                End If
            Catch ex As Exception
                WriteLog(" SiteAllDeviceListPrint_XMLNodetoDataTable " & ex.Message.ToString())
            End Try
        End Sub

        Public Function GetPurchaseOrderXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim totalPage As String = ""
            Dim totalCount As String = ""
            Dim Nodename As String = ""
            Dim Alerts_nodename As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"TotalPage", "TotalCount", "PONO", "Date", "Company", "CityState", "Zip", "Notes"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Inventory") Then
                            dNewRow = dt.NewRow
                            dNewRow("TotalPage") = totalPage
                            dNewRow("TotalCount") = totalCount
                            For k As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(k).Name
                                Dim Alert_nodeValue As String = AlertList_Node(k).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        ElseIf (Nodename = "TotalPage") Then
                            totalPage = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "TotalCount") Then
                            totalCount = AlertList_Node(0).InnerText
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetPurchaseOrderXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetPurchaseDetailXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode
            Dim Alerts_Details_Sub_Node As XmlNode

            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList
            Dim AlertList_Details_Sub_Node As XmlNodeList

            Dim totalPage As String = ""
            Dim totalCount As String = ""
            Dim pono As String = ""
            Dim podate As String = ""
            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim Details_Sub_Nodename As String = ""
            Dim List_Sub_Nodename As String = ""
            Dim Alerts_nodename As String = ""
            Dim company As String = ""
            Dim site As String = ""
            Dim modelItem As String = ""
            Dim poQty As String = ""
            Dim poNotes As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"TotalPage", "TotalCount", "PONO", "PODate", "Company", "Site", "ModelItem", "POQty", "PONotes", "BatchNo", "Date", "Qty", "ShippedQty", "SWversion", "Notes", "PORemainingFill"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Purchase") Then
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name
                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If Sub_Nodename = "PONO" Then
                                    pono = AlertList_Node(j).InnerText
                                ElseIf Sub_Nodename = "PODate" Then
                                    podate = AlertList_Node(j).InnerText
                                ElseIf Sub_Nodename = "Company" Then
                                    company = AlertList_Node(j).InnerText
                                ElseIf Sub_Nodename = "Site" Then
                                    site = AlertList_Node(j).InnerText
                                ElseIf Sub_Nodename = "PoDetails" Then
                                    For k As Integer = 0 To AlertList_Sub_Node.Count - 1
                                        Alerts_Details_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                        Details_Sub_Nodename = Alerts_Details_Sub_Node.Name
                                        AlertList_Details_Sub_Node = Alerts_Details_Sub_Node.ChildNodes

                                        If Details_Sub_Nodename = "ModelItem" Then
                                            modelItem = Alerts_Details_Sub_Node.InnerText
                                        ElseIf Details_Sub_Nodename = "POQty" Then
                                            poQty = Alerts_Details_Sub_Node.InnerText
                                        ElseIf Details_Sub_Nodename = "PONotes" Then
                                            poNotes = Alerts_Details_Sub_Node.InnerText
                                        ElseIf Details_Sub_Nodename = "POList" Then
                                            dNewRow = dt.NewRow
                                            dNewRow("TotalPage") = totalPage
                                            dNewRow("TotalCount") = totalCount

                                            dNewRow("PONO") = pono
                                            dNewRow("PODate") = podate
                                            dNewRow("Company") = company
                                            dNewRow("Site") = site
                                            dNewRow("ModelItem") = modelItem
                                            dNewRow("POQty") = poQty
                                            dNewRow("PONotes") = poNotes

                                            For l As Integer = 0 To AlertList_Details_Sub_Node.Count - 1
                                                Dim Alert_nodename As String = AlertList_Details_Sub_Node(l).Name
                                                Dim Alert_nodeValue As String = AlertList_Details_Sub_Node(l).InnerText
                                                dNewRow(Alert_nodename) = Alert_nodeValue
                                            Next

                                            dt.Rows.Add(dNewRow)
                                        End If
                                    Next
                                End If
                            Next
                        ElseIf (Nodename = "TotalPage") Then
                            totalPage = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "TotalCount") Then
                            totalCount = AlertList_Node(0).InnerText
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetPurchaseOrderXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetPurchaseSummaryXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"TotalPO", "FirstPODate", "LastPODate", "PONO", "POQty", "ModelItem"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    dNewRow = dt.NewRow
                    For k As Integer = 0 To xmlNd.ChildNodes.Count - 1
                        Dim Alert_nodename As String = xmlNd.ChildNodes(k).Name
                        Dim Alert_nodeValue As String = xmlNd.ChildNodes(k).InnerText
                        dNewRow(Alert_nodename) = Alert_nodeValue
                    Next
                    dt.Rows.Add(dNewRow)
                End If
            Catch ex As Exception
                WriteLog("GetPurchaseSummaryXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetLocationChangeEvent_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim LocationChange_Node As XmlNode
            Dim LocationChange_Sub_Node As XmlNode
            Dim LocationChangeList_Node As XmlNodeList
            Dim LocationChangeList_Sub_Node As XmlNodeList

            Dim TagId As String = ""
            Dim CurrentRoom As String = ""
            Dim EnteredOn As String = ""
            Dim LastRoom As String = ""
            Dim LeftOn As String = ""
            Dim TimeSpend As String = ""
            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim TotalPage As String = ""
            Dim TotalCount As String = ""
            Dim LastUpdatedOn As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"TagId", "CurrentRoom", "EnteredOn", "LastRoom", "LeftOn", "TimeSpend", "TotalPage", "TotalCount", "LastUpdatedOn"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        dNewRow = dt.NewRow
                        LocationChange_Node = xmlNd.ChildNodes(i)
                        Nodename = LocationChange_Node.Name

                        LocationChangeList_Node = LocationChange_Node.ChildNodes
                        For j As Integer = 0 To LocationChangeList_Node.Count - 1
                            If Nodename = "TotalPage" Then
                                TotalPage = LocationChangeList_Node(j).InnerText
                            ElseIf Nodename = "TotalCount" Then
                                TotalCount = LocationChangeList_Node(j).InnerText
                            ElseIf Nodename = "LastUpdatedOn" Then
                                LastUpdatedOn = LocationChangeList_Node(j).InnerText

                            ElseIf Nodename = "Event" Then
                                LocationChange_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = LocationChange_Sub_Node.Name

                                LocationChangeList_Sub_Node = LocationChange_Sub_Node.ChildNodes

                                If (Sub_Nodename = "TagId") Then
                                    TagId = LocationChangeList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "CurrentRoom" Then
                                    CurrentRoom = LocationChangeList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "EnteredOn" Then
                                    EnteredOn = LocationChangeList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "LastRoom" Then
                                    LastRoom = LocationChangeList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "LeftOn" Then
                                    LeftOn = LocationChangeList_Sub_Node(0).InnerText
                                ElseIf Sub_Nodename = "TimeSpend" Then
                                    TimeSpend = LocationChangeList_Sub_Node(0).InnerText
                                End If
                            End If
                        Next
                        If Nodename = "Event" Then
                            dNewRow("TagId") = TagId
                            dNewRow("CurrentRoom") = CurrentRoom
                            dNewRow("EnteredOn") = EnteredOn
                            dNewRow("LastRoom") = LastRoom
                            dNewRow("LeftOn") = LeftOn
                            dNewRow("TimeSpend") = TimeSpend
                            dNewRow("TotalPage") = TotalPage
                            dNewRow("TotalCount") = TotalCount
                            dNewRow("LastUpdatedOn") = LastUpdatedOn
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetLocationChangeEvent_XMLNodetoDataTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function
        Public Function GetResponseToTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim ResponseList_Node As XmlNodeList
          
            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Method", "Result", "Error"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then

                    dNewRow = dt.NewRow
                    ResponseList_Node = xmlNd.ChildNodes

                    If ResponseList_Node.Count > 0 Then
                        dNewRow("Method") = ResponseList_Node(0).InnerText
                    Else
                        dNewRow("Method") = ""
                    End If

                    If ResponseList_Node.Count > 1 Then
                        dNewRow("Result") = ResponseList_Node(1).InnerText
                    Else
                        dNewRow("Result") = "1"
                    End If

                    If ResponseList_Node.Count > 2 Then
                        dNewRow("Error") = ResponseList_Node(2).InnerText
                    Else
                        dNewRow("Error") = "Invalid response from server"
                    End If

                    dt.Rows.Add(dNewRow)

                End If
            Catch ex As Exception
                WriteLog("GetResponseToTable " & ex.Message.ToString)
            End Try
            Return dt
        End Function

        Public Function GetMapXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            Dim elemList As XmlNodeList
            Dim elemList_Building As XmlNodeList
            Dim elemList_Floor As XmlNodeList
            Dim elemList_Unit As XmlNodeList

            Dim ds As New DataSet
            Dim dtTable As New DataTable
            Dim dtTable2 As New DataTable
            Dim dtTable3 As New DataTable
            Dim dtTable4 As New DataTable
            Dim dtTable5 As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim elemIdx As Integer = 0
            Dim elemIdx2 As Integer = 0
            Dim elemIdx3 As Integer = 0
            Dim elemIdx4 As Integer = 0

            dtTable.Columns.Add(New DataColumn("CampusId", System.Type.[GetType]("System.Int32")))
            dtTable.Columns.Add(New DataColumn("CampusName", System.Type.[GetType]("System.String")))
            dtTable.Columns.Add(New DataColumn("CampusDescription", System.Type.[GetType]("System.String")))
            dtTable.Columns.Add(New DataColumn("CampusImage", System.Type.[GetType]("System.String")))

            dtTable2.Columns.Add(New DataColumn("CampusId_Building", System.Type.[GetType]("System.Int32")))
            dtTable2.Columns.Add(New DataColumn("BuildingId", System.Type.[GetType]("System.Int32")))
            dtTable2.Columns.Add(New DataColumn("BuildingName", System.Type.[GetType]("System.String")))
            dtTable2.Columns.Add(New DataColumn("BuildingDescription", System.Type.[GetType]("System.String")))
            dtTable2.Columns.Add(New DataColumn("BuildingImage", System.Type.[GetType]("System.String")))

            dtTable3.Columns.Add(New DataColumn("BuildingId_Floor", System.Type.[GetType]("System.Int32")))
            dtTable3.Columns.Add(New DataColumn("FloorId", System.Type.[GetType]("System.Int32")))
            dtTable3.Columns.Add(New DataColumn("FloorName", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("FloorDescription", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("svgFile", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("BGFile", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("InfrastructureCsvFilePath", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("BGFileWidth", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("BGFileHeight", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("BGFileTile", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("widthInft", System.Type.[GetType]("System.String")))
            dtTable3.Columns.Add(New DataColumn("lengthInft", System.Type.[GetType]("System.String")))


            dtTable4.Columns.Add(New DataColumn("FloorId_Unit", System.Type.[GetType]("System.Int32")))
            dtTable4.Columns.Add(New DataColumn("UnitId", System.Type.[GetType]("System.Int32")))
            dtTable4.Columns.Add(New DataColumn("UnitName", System.Type.[GetType]("System.String")))
            dtTable4.Columns.Add(New DataColumn("UnitDescription", System.Type.[GetType]("System.String")))
            dtTable4.Columns.Add(New DataColumn("RoomCsvFilePath", System.Type.[GetType]("System.String")))

            dtTable5.Columns.Add(New DataColumn("TagCsvFilePath", System.Type.[GetType]("System.String")))

            Try
                'Tag
                elemList = xmlNd.SelectNodes("Tag")
                dNewRow = dtTable5.NewRow
                dNewRow("TagCsvFilePath") = elemList.Item(elemIdx).Item("TagCsvFilePath").InnerText
                dtTable5.Rows.Add(dNewRow)

                'Campus
                elemList = xmlNd.SelectNodes("Campus")
                If elemList.Count > 0 Then
                    For elemIdx = 0 To elemList.Count - 1
                        dNewRow = dtTable.NewRow
                        dNewRow("CampusId") = elemList.Item(elemIdx).Item("CampusId").InnerText
                        dNewRow("CampusName") = elemList.Item(elemIdx).Item("CampusName").InnerText
                        dNewRow("CampusDescription") = elemList.Item(elemIdx).Item("CampusDescription").InnerText
                        dNewRow("CampusImage") = elemList.Item(elemIdx).Item("CampusImage").InnerText
                        dtTable.Rows.Add(dNewRow)

                        'Buildings
                        If Not elemList(elemIdx).Item("Bulidings") Is Nothing Then
                            elemList_Building = elemList(elemIdx).Item("Bulidings").SelectNodes("Building")
                            If elemList_Building.Count > 0 Then
                                For elemIdx2 = 0 To elemList_Building.Count - 1
                                    dNewRow = dtTable2.NewRow
                                    dNewRow("CampusId_Building") = elemList.Item(elemIdx).Item("CampusId").InnerText
                                    dNewRow("BuildingId") = elemList_Building.Item(elemIdx2).Item("BuildId").InnerText
                                    dNewRow("BuildingName") = elemList_Building.Item(elemIdx2).Item("BuildName").InnerText
                                    dNewRow("BuildingDescription") = elemList_Building.Item(elemIdx2).Item("BuildDescription").InnerText
                                    dNewRow("BuildingImage") = elemList_Building.Item(elemIdx2).Item("BuildingImage").InnerText
                                    dtTable2.Rows.Add(dNewRow)

                                    'Floors
                                    If Not elemList_Building(elemIdx2).Item("Floors") Is Nothing Then
                                        elemList_Floor = elemList_Building(elemIdx2).Item("Floors").SelectNodes("Floor")
                                        If elemList_Floor.Count > 0 Then
                                            For elemIdx3 = 0 To elemList_Floor.Count - 1
                                                dNewRow = dtTable3.NewRow
                                                dNewRow("BuildingId_Floor") = elemList_Building.Item(elemIdx2).Item("BuildId").InnerText
                                                dNewRow("FloorId") = elemList_Floor.Item(elemIdx3).Item("FloorId").InnerText
                                                dNewRow("FloorName") = elemList_Floor.Item(elemIdx3).Item("FloorName").InnerText
                                                dNewRow("FloorDescription") = elemList_Floor.Item(elemIdx3).Item("FloorDescription").InnerText
                                                dNewRow("svgFile") = elemList_Floor.Item(elemIdx3).Item("svgFile").InnerText
                                                dNewRow("BGFile") = elemList_Floor.Item(elemIdx3).Item("BGFile").InnerText
                                                dNewRow("InfrastructureCsvFilePath") = elemList_Floor.Item(elemIdx3).Item("InfrastructureCsvFilePath").InnerText
                                                dNewRow("BGFileWidth") = elemList_Floor.Item(elemIdx3).Item("bgImageWidth").InnerText
                                                dNewRow("BGFileHeight") = elemList_Floor.Item(elemIdx3).Item("bgImageHeight").InnerText
                                                dNewRow("BGFileTile") = elemList_Floor.Item(elemIdx3).Item("TilePath").InnerText
                                                dNewRow("widthInft") = elemList_Floor.Item(elemIdx3).Item("widthft").InnerText
                                                dNewRow("lengthInft") = elemList_Floor.Item(elemIdx3).Item("lengthft").InnerText

                                                dtTable3.Rows.Add(dNewRow)

                                                'Units
                                                If Not elemList_Floor(elemIdx3).Item("Units") Is Nothing Then
                                                    elemList_Unit = elemList_Floor(elemIdx3).Item("Units").SelectNodes("Unit")
                                                    If elemList_Unit.Count > 0 Then
                                                        For elemIdx4 = 0 To elemList_Unit.Count - 1
                                                            dNewRow = dtTable4.NewRow
                                                            dNewRow("FloorId_Unit") = elemList_Floor.Item(elemIdx3).Item("FloorId").InnerText
                                                            dNewRow("UnitId") = elemList_Unit.Item(elemIdx4).Item("UnitId").InnerText
                                                            dNewRow("UnitName") = elemList_Unit.Item(elemIdx4).Item("UnitName").InnerText
                                                            dNewRow("UnitDescription") = elemList_Unit.Item(elemIdx4).Item("UnitDescription").InnerText
                                                            dNewRow("RoomCsvFilePath") = elemList_Unit.Item(elemIdx4).Item("RoomCsvFilePath").InnerText
                                                            dtTable4.Rows.Add(dNewRow)
                                                        Next
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If

                ds.Tables.Add(dtTable)
                ds.Tables(0).TableName = "Campus"

                ds.Tables.Add(dtTable2)
                ds.Tables(1).TableName = "Building"

                ds.Tables.Add(dtTable3)
                ds.Tables(2).TableName = "Floor"

                ds.Tables.Add(dtTable4)
                ds.Tables(3).TableName = "Unit"

                ds.Tables.Add(dtTable5)
                ds.Tables(4).TableName = "Tag"
            Catch ex As Exception
                WriteLog("GetMapXMLtoTable " & ex.Message.ToString)
            End Try

            Return ds
        End Function

        Public Function GetRoomXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode

            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"RoomId", "RoomName", "MonitorId", "RoomDescription", "RoomHallway"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Rooms") Then
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name
                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If Sub_Nodename = "Room" Then
                                    dNewRow = dt.NewRow()
                                    For l As Integer = 0 To AlertList_Sub_Node.Count - 1
                                        Dim Alert_nodename As String = AlertList_Sub_Node(l).Name
                                        Dim Alert_nodeValue As String = AlertList_Sub_Node(l).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
                                    Next
                                    dt.Rows.Add(dNewRow)
                                End If
                            Next
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetRoomXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetResultXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            Dim ds As New DataSet
            Dim dtResult As New DataTable
            Dim dtNoResult As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode

            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim result As String = ""
            Dim sError As String = ""
            Dim csvTotalRec As String = ""
            Dim csvTotalRecInserted As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Result", "error", "csvTotalRec", "csvTotalRecInserted"}
                dtResult = addColumntoDataTable(addColumn)

                Dim addColumnNoResult() As String = {"num", "Reason"}
                dtNoResult = addColumntoDataTable(addColumnNoResult)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Result") Then
                            result = AlertList_Node(0).InnerText
                            'ElseIf (Nodename = "error") Then
                            '    sError = AlertList_Node(0).InnerText.ToString()
                        ElseIf (Nodename = "csvTotalRec") Or (Nodename = "TotalRec") Then
                            csvTotalRec = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "csvTotalRecInserted") Or (Nodename = "TotalRecInserted") Then
                            csvTotalRecInserted = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "RecordsNotInstered") Or (Nodename = "RecordsNotInstered") Then
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name
                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If Sub_Nodename = "Record" Then
                                    dNewRow = dtNoResult.NewRow()
                                    For l As Integer = 0 To AlertList_Sub_Node.Count - 1
                                        Dim Alert_nodename1 As String = AlertList_Sub_Node(l).Name
                                        Dim Alert_nodeValue1 As String = AlertList_Sub_Node(l).InnerText
                                        dNewRow(Alert_nodename1) = Alert_nodeValue1
                                    Next
                                    dtNoResult.Rows.Add(dNewRow)
                                End If
                            Next
                        End If
                    Next

                    dNewRow = dtResult.NewRow()
                    dNewRow("Result") = result
                    dNewRow("error") = sError
                    dNewRow("csvTotalRec") = csvTotalRec
                    dNewRow("csvTotalRecInserted") = csvTotalRecInserted
                    dtResult.Rows.Add(dNewRow)
                End If
            Catch ex As Exception
                WriteLog("GetResultXMLtoTable " & ex.Message.ToString)
            End Try

            ds.Tables.Add(dtResult)
            ds.Tables(0).TableName = "Result"

            ds.Tables.Add(dtNoResult)
            ds.Tables(1).TableName = "NoResult"

            Return ds
        End Function

        Public Function GetTagXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode

            Dim AlertList_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""

            Dim totalPage As String = ""
            Dim totalCount As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"TotalPage", "TotalCount", "TagId", "TagName", "TagType", "Description"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Tag") Then
                            dNewRow = dt.NewRow()
                            dNewRow("TotalPage") = totalPage
                            dNewRow("TotalCount") = totalCount

                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Dim Alert_nodename As String = Alerts_Sub_Node.Name
                                Dim Alert_nodeValue As String = Alerts_Sub_Node.InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        ElseIf Nodename = "TotalPage" Then
                            totalPage = AlertList_Node(0).InnerText
                        ElseIf Nodename = "TotalCount" Then
                            totalCount = AlertList_Node(0).InnerText
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetTagXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetInfrastructureMetaInfoForFloorXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            Dim dtMonitor As New DataTable
            Dim dtStar As New DataTable
            Dim dtAccessPoints As New DataTable
            Dim dtZones As New DataTable

            Dim dNewRow As DataRow = Nothing

            Dim Monitor_Node As XmlNode
            Dim Monitor_Sub_Node As XmlNode
            Dim Monitor_Sub_Child_Node As XmlNode

            Dim MonitorList_Node As XmlNodeList
            Dim MonitorsSubList_Node As XmlNodeList

            Dim MonitorNodename As String = ""
            Dim MonitorSub_Nodename As String = ""
            Dim MonitorSubChild_Nodename As String = ""

            Dim DeviceId As String = ""
            Dim SvgId As String = ""
            Dim CSVDeviceType As String = ""
            Dim SubType As String = ""
            Dim DeviceName As String = ""
            Dim Description As String = ""
            Dim RoomId As String = ""
            Dim MonitorType As String = ""
            Dim Profile As String = ""
            Dim IRProfile As String = ""
            Dim PowerLevel As String = ""
            Dim RoomBleeding As String = ""
            Dim NoiseLevel As String = ""
            Dim Masking As String = ""
            Dim MasterSlave As String = ""
            Dim OperatingMode As String = ""
            Dim Modes As String = ""
            Dim AlertSupressionTime As String = ""
            Dim SpecialProfile As String = ""

            Dim MacId As String = ""
            Dim StarSvgId As String = ""
            Dim StarCSVDeviceType As String = ""
            Dim StarSubType As String = ""
            Dim StarDeviceName As String = ""
            Dim StarType As String = ""
            Dim DHCP As String = ""
            Dim SaveSettings As String = ""
            Dim StaticIP As String = ""
            Dim Subnet As String = ""
            Dim Gateway As String = ""
            Dim TimeServerIP As String = ""
            Dim ServerIP As String = ""
            Dim PagingServerIP As String = ""
            Dim LocationServerIP1 As String = ""
            Dim LocationServerIP2 As String = ""
            Dim Location As String = ""
            Dim Notes As String = ""
            Dim ProfileId As String = ""
            Dim LockedStarId As String = ""
            Dim StarLocation As String = ""
            Dim IsHallWay As String = ""
            Dim UnitName As String = ""
            Dim ZoneId As String = ""
            Dim FloorName As String = ""
            Dim StarId As String = ""
            Dim Xaxis As String = ""
            Dim Yaxis As String = ""
            Dim WidthFt As String = ""
            Dim LengthFt As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumnMonitor() As String = {"DeviceId", "SvgId", "CSVDeviceType", "SubType", "DeviceName", "Description", "RoomId", "MonitorType", "Profile", "IRProfile", "PowerLevel", "RoomBleeding", "NoiseLevel", "Masking", "MasterSlave", "SpecialProfile", "OperatingMode", "Modes", "AlertSupressionTime", "Location", "Notes", "ProfileId", "LockedStarId", "StarLocation", "IsHallWay", "UnitName", "FloorName", "Xaxis", "Yaxis"}
                dtMonitor = addColumntoDataTable(addColumnMonitor)
                Dim addColumnStar() As String = {"MacId", "StarId", "SvgId", "CSVDeviceType", "SubType", "StarName", "Description", "StarType", "DHCP", "SaveSettings", "StaticIP", "Subnet", "Gateway", "TimeServerIP", "ServerIP", "PagingServerIP", "LocationServerIP1", "LocationServerIP2", "FloorName", "Xaxis", "Yaxis"}
                dtStar = addColumntoDataTable(addColumnStar)
                Dim addColumnAccessPoint() As String = {"DeviceId", "SvgId", "CSVDeviceType", "SubType", "DeviceName", "Description", "Location", "Notes", "IsHallWay", "UnitName", "Xaxis", "Yaxis"}
                dtAccessPoints = addColumntoDataTable(addColumnAccessPoint)
                Dim addColumnZone() As String = {"ZoneId", "SvgId", "CSVDeviceType", "SubType", "DeviceName", "Description", "Location", "Notes", "IsHallWay", "UnitName", "Xaxis", "Yaxis", "WidthFt", "LengthFt"}
                dtZones = addColumntoDataTable(addColumnZone)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Monitor_Node = xmlNd.ChildNodes(i)
                        MonitorNodename = Monitor_Node.Name
                        MonitorList_Node = Monitor_Node.ChildNodes

                        If (MonitorNodename = "Monitors") Then
                            For j As Integer = 0 To MonitorList_Node.Count - 1
                                dNewRow = dtMonitor.NewRow()
                                Monitor_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                MonitorSub_Nodename = Monitor_Sub_Node.Name
                                MonitorsSubList_Node = Monitor_Sub_Node.ChildNodes
                                If MonitorSub_Nodename = "Monitor" Then
                                    For k As Integer = 0 To MonitorsSubList_Node.Count - 1
                                        Monitor_Sub_Child_Node = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                        MonitorSubChild_Nodename = Monitor_Sub_Child_Node.Name
                                        If MonitorSubChild_Nodename = "DeviceId" Then
                                            DeviceId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SvgId" Then
                                            SvgId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "CSVDeviceType" Then
                                            CSVDeviceType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SubType" Then
                                            SubType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "DeviceName" Then
                                            DeviceName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Description" Then
                                            Description = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "RoomId" Then
                                            RoomId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "MonitorType" Then
                                            MonitorType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Profile" Then
                                            Profile = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "IRProfile" Then
                                            IRProfile = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "PowerLevel" Then
                                            PowerLevel = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "IRProfile" Then
                                            IRProfile = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "RoomBleeding" Then
                                            RoomBleeding = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "NoiseLevel" Then
                                            NoiseLevel = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Masking" Then
                                            Masking = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "MasterSlave" Then
                                            MasterSlave = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SpecialProfile" Then
                                            SpecialProfile = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "OperatingMode" Then
                                            OperatingMode = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Modes" Then
                                            Modes = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Location" Then
                                            Location = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Notes" Then
                                            Notes = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "ProfileId" Then
                                            ProfileId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "LockedStarId" Then
                                            LockedStarId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "StarLocation" Then
                                            StarLocation = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "IsHallWay" Then
                                            IsHallWay = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "UnitName" Then
                                            UnitName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "FloorName" Then
                                            FloorName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Xaxis" Then
                                            Xaxis = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Yaxis" Then
                                            Yaxis = MonitorsSubList_Node(k).InnerText
                                        End If
                                    Next
                                    dNewRow("DeviceId") = DeviceId
                                    dNewRow("SvgId") = SvgId
                                    dNewRow("CSVDeviceType") = CSVDeviceType
                                    dNewRow("SubType") = SubType
                                    dNewRow("DeviceName") = DeviceName
                                    dNewRow("Description") = Description
                                    dNewRow("RoomId") = RoomId
                                    dNewRow("MonitorType") = MonitorType
                                    dNewRow("Profile") = Profile
                                    dNewRow("IRProfile") = IRProfile
                                    dNewRow("PowerLevel") = PowerLevel
                                    dNewRow("RoomBleeding") = RoomBleeding
                                    dNewRow("NoiseLevel") = NoiseLevel
                                    dNewRow("Masking") = Masking
                                    dNewRow("MasterSlave") = MasterSlave
                                    dNewRow("SpecialProfile") = SpecialProfile
                                    dNewRow("OperatingMode") = OperatingMode
                                    dNewRow("Modes") = Modes
                                    dNewRow("AlertSupressionTime") = AlertSupressionTime
                                    dNewRow("Location") = Location
                                    dNewRow("Notes") = Notes
                                    dNewRow("ProfileId") = ProfileId
                                    dNewRow("LockedStarId") = LockedStarId
                                    dNewRow("StarLocation") = StarLocation
                                    dNewRow("IsHallWay") = IsHallWay
                                    dNewRow("UnitName") = UnitName
                                    dNewRow("FloorName") = FloorName
                                    dNewRow("Xaxis") = Xaxis
                                    dNewRow("Yaxis") = Yaxis
                                    dtMonitor.Rows.Add(dNewRow)
                                End If
                            Next
                        ElseIf (MonitorNodename = "Stars") Then
                            For j As Integer = 0 To MonitorList_Node.Count - 1
                                dNewRow = dtStar.NewRow()
                                Monitor_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                MonitorSub_Nodename = Monitor_Sub_Node.Name
                                MonitorsSubList_Node = Monitor_Sub_Node.ChildNodes
                                If MonitorSub_Nodename = "Star" Then
                                    For k As Integer = 0 To MonitorsSubList_Node.Count - 1
                                        Monitor_Sub_Child_Node = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                        MonitorSubChild_Nodename = Monitor_Sub_Child_Node.Name
                                        If MonitorSubChild_Nodename = "MacId" Then
                                            MacId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SvgId" Then
                                            SvgId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "CSVDeviceType" Then
                                            CSVDeviceType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SubType" Then
                                            SubType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "StarName" Then
                                            DeviceName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Description" Then
                                            Description = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "StarType" Then
                                            StarType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "DHCP" Then
                                            DHCP = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SaveSettings" Then
                                            SaveSettings = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "StaticIP" Then
                                            StaticIP = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Subnet" Then
                                            Subnet = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Gateway" Then
                                            Gateway = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "TimeServerIP" Then
                                            TimeServerIP = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "ServerIP" Then
                                            ServerIP = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "PagingServerIP" Then
                                            PagingServerIP = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "LocationServerIP1" Then
                                            LocationServerIP1 = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "LocationServerIP2" Then
                                            LocationServerIP2 = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "StarId" Then
                                            StarId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "FloorName" Then
                                            FloorName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Xaxis" Then
                                            Xaxis = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Yaxis" Then
                                            Yaxis = MonitorsSubList_Node(k).InnerText
                                        End If
                                    Next
                                    dNewRow("MacId") = MacId
                                    dNewRow("SvgId") = SvgId
                                    dNewRow("CSVDeviceType") = CSVDeviceType
                                    dNewRow("SubType") = SubType
                                    dNewRow("StarName") = DeviceName
                                    dNewRow("Description") = Description
                                    dNewRow("StarType") = StarType
                                    dNewRow("DHCP") = DHCP
                                    dNewRow("SaveSettings") = SaveSettings
                                    dNewRow("StaticIP") = StaticIP
                                    dNewRow("Subnet") = Subnet
                                    dNewRow("Gateway") = Gateway
                                    dNewRow("TimeServerIP") = TimeServerIP
                                    dNewRow("ServerIP") = ServerIP
                                    dNewRow("PagingServerIP") = PagingServerIP
                                    dNewRow("LocationServerIP1") = LocationServerIP1
                                    dNewRow("LocationServerIP2") = LocationServerIP2
                                    dNewRow("StarId") = StarId
                                    dNewRow("FloorName") = FloorName
                                    dNewRow("Xaxis") = Xaxis
                                    dNewRow("Yaxis") = Yaxis
                                    dtStar.Rows.Add(dNewRow)
                                End If
                            Next
                        ElseIf (MonitorNodename = "AccessPoints") Then
                            For j As Integer = 0 To MonitorList_Node.Count - 1
                                dNewRow = dtAccessPoints.NewRow()
                                Monitor_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                MonitorSub_Nodename = Monitor_Sub_Node.Name
                                MonitorsSubList_Node = Monitor_Sub_Node.ChildNodes
                                If MonitorSub_Nodename = "AccessPoint" Then
                                    For k As Integer = 0 To MonitorsSubList_Node.Count - 1
                                        Monitor_Sub_Child_Node = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                        MonitorSubChild_Nodename = Monitor_Sub_Child_Node.Name
                                        If MonitorSubChild_Nodename = "DeviceId" Then
                                            DeviceId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SvgId" Then
                                            SvgId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "CSVDeviceType" Then
                                            CSVDeviceType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SubType" Then
                                            SubType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "DeviceName" Then
                                            DeviceName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Description" Then
                                            Description = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Location" Then
                                            Location = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Notes" Then
                                            Notes = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "IsHallWay" Then
                                            IsHallWay = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "UnitName" Then
                                            UnitName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Xaxis" Then
                                            Xaxis = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Yaxis" Then
                                            Yaxis = MonitorsSubList_Node(k).InnerText
                                        End If

                                    Next
                                    dNewRow("DeviceId") = DeviceId
                                    dNewRow("SvgId") = SvgId
                                    dNewRow("CSVDeviceType") = CSVDeviceType
                                    dNewRow("SubType") = SubType
                                    dNewRow("DeviceName") = DeviceName
                                    dNewRow("Description") = Description
                                    dNewRow("Location") = Location
                                    dNewRow("Notes") = Notes
                                    dNewRow("IsHallWay") = IsHallWay
                                    dNewRow("UnitName") = UnitName
                                    dNewRow("Xaxis") = Xaxis
                                    dNewRow("Yaxis") = Yaxis
                                    dtAccessPoints.Rows.Add(dNewRow)
                                End If
                            Next
                        ElseIf (MonitorNodename = "Zones") Then
                            For j As Integer = 0 To MonitorList_Node.Count - 1
                                dNewRow = dtZones.NewRow()
                                Monitor_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                MonitorSub_Nodename = Monitor_Sub_Node.Name
                                MonitorsSubList_Node = Monitor_Sub_Node.ChildNodes
                                If MonitorSub_Nodename = "Zone" Then
                                    For k As Integer = 0 To MonitorsSubList_Node.Count - 1
                                        Monitor_Sub_Child_Node = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                        MonitorSubChild_Nodename = Monitor_Sub_Child_Node.Name
                                        If MonitorSubChild_Nodename = "ZoneId" Then
                                            ZoneId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SvgId" Then
                                            SvgId = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "CSVDeviceType" Then
                                            CSVDeviceType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "SubType" Then
                                            SubType = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "DeviceName" Then
                                            DeviceName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Description" Then
                                            Description = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Location" Then
                                            Location = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Notes" Then
                                            Notes = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "IsHallWay" Then
                                            IsHallWay = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "UnitName" Then
                                            UnitName = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Xaxis" Then
                                            Xaxis = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "Yaxis" Then
                                            Yaxis = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "WidthFt" Then
                                            WidthFt = MonitorsSubList_Node(k).InnerText
                                        ElseIf MonitorSubChild_Nodename = "LengthFt" Then
                                            LengthFt = MonitorsSubList_Node(k).InnerText
                                        End If

                                    Next
                                    dNewRow("ZoneId") = ZoneId
                                    dNewRow("SvgId") = SvgId
                                    dNewRow("CSVDeviceType") = CSVDeviceType
                                    dNewRow("SubType") = SubType
                                    dNewRow("DeviceName") = DeviceName
                                    dNewRow("Description") = Description
                                    dNewRow("Location") = Location
                                    dNewRow("Notes") = Notes
                                    dNewRow("IsHallWay") = IsHallWay
                                    dNewRow("UnitName") = UnitName
                                    dNewRow("Xaxis") = Xaxis
                                    dNewRow("Yaxis") = Yaxis
                                    dNewRow("WidthFt") = WidthFt
                                    dNewRow("LengthFt") = LengthFt
                                    dtZones.Rows.Add(dNewRow)
                                End If
                            Next
                        End If
                    Next
                End If

            Catch ex As Exception
                WriteLog("GetInfrastructureMetaInfoForFloorXMLtoTable " & ex.Message.ToString)
            End Try
            Dim dsReturn As New DataSet
      
            dsReturn.Tables.Add(dtMonitor)
            dsReturn.Tables(0).TableName = "dtMonitor"

            dsReturn.Tables.Add(dtStar)
            dsReturn.Tables(1).TableName = "dtStar"

            dsReturn.Tables.Add(dtAccessPoints)
            dsReturn.Tables(2).TableName = "dtAccessPoint"

            dsReturn.Tables.Add(dtZones)
            dsReturn.Tables(3).TableName = "dtZone"

            Return dsReturn
        End Function

        Public Function GetTagMetaInfoForFloorXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Tag_Node As XmlNode
            Dim Tag_Sub_Node As XmlNode

            Dim TagList_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""

            Dim TagInfo_Node As XmlNode
            Dim TagInfoNodename As String = ""
            Dim TagInfoList_Node As XmlNodeList

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"MonitorId", "RoomId", "SvgId", "RoomName", "Description", "TagId", "TagType", "ModelItem", "BatteryCapacity", "LastIRTime", "RoomSeen", "TagTypeName", "LockedStarId", "StarLocation"}
                dt = addColumntoDataTable(addColumn)
                Dim nMonitorId As String = ""
                Dim nSvgId As String = ""

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Tag_Node = xmlNd.ChildNodes(i)
                        Nodename = Tag_Node.Name
                        TagList_Node = Tag_Node.ChildNodes

                        If (Nodename = "Tag") Then

                            For j As Integer = 0 To TagList_Node.Count - 1
                                Tag_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Tag_Sub_Node.Name
                                If (Sub_Nodename = "MonitorId") Then
                                    nMonitorId = TagList_Node(j).InnerText
                                ElseIf (Sub_Nodename = "SvgId") Then
                                    nSvgId = TagList_Node(j).InnerText
                                ElseIf (Sub_Nodename = "TagInfo") Then
                                    TagInfo_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                    TagInfoNodename = TagInfo_Node.Name
                                    TagInfoList_Node = TagInfo_Node.ChildNodes
                                    dNewRow = dt.NewRow()
                                    dNewRow("MonitorId") = nMonitorId
                                    dNewRow("SvgId") = nSvgId
                                    For k As Integer = 0 To TagInfoList_Node.Count - 1
                                        Dim Alert_nodename As String = TagInfoList_Node(k).Name
                                        Dim Alert_nodeValue As String = TagInfoList_Node(k).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
                                    Next
                                    If dNewRow("TagId").ToString <> "" Then dt.Rows.Add(dNewRow)
                                End If
                            Next

                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetTagMetaInfoForFloorXMLtoTable " & ex.Message.ToString)
            End Try
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    dt.DefaultView.Sort = "TagTypeName"
                    dt = dt.DefaultView.ToTable()
                End If
            End If
            Return dt
        End Function

        Public Function GetAnnouncementsXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim totalPage As String = ""
            Dim totalCount As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"TotalPage", "TotalCount", "AnnouncementId", "Message", "StartDatetime", "EndDateTime", "IsShow", "UserRoleToShow", "UserAssociatedViews", "HTMLMessage", "IsActive", "ShowIn", "afterExpireDisp", "afterExpireDispDays", "DispInHistory"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Annoncement") Then
                            dNewRow = dt.NewRow()
                            dNewRow("TotalPage") = totalPage
                            dNewRow("TotalCount") = totalCount
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(j).Name
                                Dim Alert_nodeValue As String = AlertList_Node(j).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        ElseIf Nodename = "TotalPage" Then
                            totalPage = AlertList_Node(0).InnerText
                        ElseIf Nodename = "TotalCount" Then
                            totalCount = AlertList_Node(0).InnerText
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetAnnouncementsXMLtoTable " & ex.Message.ToString)
            End Try

            ds.Tables.Add(dt)
            ds.Tables(0).TableName = "Announcement"

            Return ds
        End Function
	
        Public Function GetPastAnnouncementsXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim totalPage As String = ""
            Dim totalCount As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"AnnouncementId", "Message", "StartDatetime", "EndDateTime", "IsShow", "UserRoleToShow", "UserAssociatedViews", "HTMLMessage", "IsActive", "ShowIn"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes


                        dNewRow = dt.NewRow()

                        For j As Integer = 0 To AlertList_Node.Count - 1
                            Dim Alert_nodename As String = AlertList_Node(j).Name
                            Dim Alert_nodeValue As String = AlertList_Node(j).InnerText
                            dNewRow(Alert_nodename) = Alert_nodeValue
                        Next
                        dt.Rows.Add(dNewRow)

                    Next
                End If
            Catch ex As Exception
                WriteLog("GetPastAnnouncementsXMLtoTable " & ex.Message.ToString)
            End Try

            ds.Tables.Add(dt)
            ds.Tables(0).TableName = "Announcement"

            Return ds
        End Function
	
        Public Function GetHTMLAnnouncementsXMLtoTable(ByVal xmlNd As XmlNode) As DataSet

            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim AlertList_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim totalPage As String = ""
            Dim totalCount As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"AnnouncementId", "StartDatetime", "EndDateTime", "HTMLMessage", "IsActive"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Annoncement") Then
                            dNewRow = dt.NewRow()
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Dim Alert_nodename As String = AlertList_Node(j).Name
                                Dim Alert_nodeValue As String = ""

                                Alert_nodeValue = AlertList_Node(j).InnerText
                                dNewRow(Alert_nodename) = Alert_nodeValue
                            Next
                            dt.Rows.Add(dNewRow)
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetAnnouncementsXMLtoTable " & ex.Message.ToString)
            End Try

            ds.Tables.Add(dt)
            ds.Tables(0).TableName = "Announcement"

            Return ds

        End Function

        Public Function GetSearchDeviceMapXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim AlertList_Node As XmlNodeList

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"DeviceId", "FloorId", "CampusId", "BuildingId", "SvgId", "DeviceType"}
                dt = addColumntoDataTable(addColumn)

                AlertList_Node = xmlNd.ChildNodes

                If cnt > 0 Then
                    dNewRow = dt.NewRow()
                    For i As Integer = 0 To cnt - 1
                        Dim Alert_nodename As String = AlertList_Node(i).Name
                        Dim Alert_nodeValue As String = AlertList_Node(i).InnerText
                        dNewRow(Alert_nodename) = Alert_nodeValue
                    Next
                    dt.Rows.Add(dNewRow)
                End If
            Catch ex As Exception
                WriteLog("GetSearchDeviceMapXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt

        End Function

        Public Function GetResponseText_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable

            Dim dt As New DataTable
            Dim dr As DataRow

            dt.Columns.Add(New DataColumn("ResponseText", Type.GetType("System.String")))
            dr = dt.NewRow()

            Try

                dr("ResponseText") = xmlNd.InnerText
                dt.Rows.Add(dr)

            Catch ex As Exception
                WriteLog(" ForgotPassword_XMLNodetoDataTable " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Public Function DownloadPdf(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String, Optional ByVal IsSavePdf As Boolean = False,
                                    Optional ByVal BatteryChanged As String() = Nothing, Optional ByVal chkUnableToLocate As String = "",
                                    Optional ByVal chkLocated As String = "", Optional ByVal chkBatteryReplaced As String = "",
                                    Optional ByVal BatteryChangedMonitor As String() = Nothing, Optional ByVal chkUnableToLocateMonitor As String = "",
                                    Optional ByVal chkLocatedMonitor As String = "", Optional ByVal chkBatteryReplacedMonitor As String = "") As Boolean

            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable

            Dim theDoc As Doc = New Doc()
            theDoc.HtmlOptions.Timeout = 10000000
            theDoc.HtmlOptions.Engine = EngineType.Gecko

            Dim sHTML As New StringBuilder
            Dim HeaderHTML As New StringBuilder

            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim bNoRecordFound As Boolean = False
            Dim HTMLsaveFloder As String = "HTML"
            Dim HTML_filepath As String = ""
            Dim apiKey As String = ""

            Try

                If Not Directory.Exists(GetAppPath() & "\" & HTMLsaveFloder) Then
                    Directory.CreateDirectory(GetAppPath() & "\" & HTMLsaveFloder)
                End If

                If (RptType = "0") Then
                    Sheetname = "Good-Device-List"
                ElseIf (RptType = "1") Then
                    Sheetname = "UnderWatch-List"
                ElseIf (RptType = "2") Then
                    Sheetname = "LBI-List"
                ElseIf (RptType = "3") Then
                    Sheetname = "Offline-List"
                ElseIf (RptType = "4") Then
                    Sheetname = "All-LBI"
                ElseIf (RptType = "") Then
                    Sheetname = "All-LBI"
                Else
                    Sheetname = ""
                End If

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                HTML_filepath = GetAppPath() & "\" & HTMLsaveFloder & "\" & sFileName & "_" & g_UserId & ".html"

                theDoc.Rect.Inset(20, 25)
                theDoc.Page = theDoc.AddPage()

                apiKey = g_UserAPI

                If RptType <> Nothing Or RptType <> "" Then
                    If g_UserAPI = "" Then
                        LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", apiKey, bNoRecordFound)
                    Else
                        LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", , bNoRecordFound)
                    End If
                Else
                    LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", "4", "", "", apiKey, bNoRecordFound)
                End If

                If bNoRecordFound Then
                    dtTag = Nothing
                    dtMonitor = Nothing
                End If

                'Header Content
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td><img src='" & GetServerPath() & "/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td align='left' style='border-bottom: solid 5px #245E90;'>")
                HeaderHTML.Append("&nbsp;</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td valign='top' align='center'>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; color: #373737; border-bottom: solid 1px #D3D3D3;height:40px;' valign='middle' class='sText'>")
                HeaderHTML.Append("<label class='SHeader1'>")
                HeaderHTML.Append(SiteName)
                HeaderHTML.Append("</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%' height='40px'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; width: 20%; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Date&nbsp;:&nbsp;")
                HeaderHTML.Append("<label>" & CDate(Now()).Date & "</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Report&nbsp;Type&nbsp;:&nbsp;")

                If RptType = "1" Then
                    HeaderHTML.Append("<label>Less Than 90 Days</label>")
                ElseIf RptType = "2" Then
                    HeaderHTML.Append("<label>Less Than 30 Days</label>")
                Else
                    HeaderHTML.Append("<label>All LBI</label>")
                End If

                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")

                'BOBY Content
                sHTML.Append("<html>")
                sHTML.Append("<head>")
                sHTML.Append("<style>")
                sHTML.Append(".siteOverview_TopLeft_Box" & _
                              "{" & _
                                    "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "background-image: url('" & GetServerPath() & "/Images/tblHeaderbg.png');" & _
                                  "background-repeat:no-repeat;background-size:130px 60px;" & _
                                 "color:White;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                              "}" & _
                              ".siteOverview_Box" & _
                              "{" & _
                                  "border-top:1px solid #DADADA;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #454545;" & _
                              "}" & _
                              ".siteOverview_Topright_Box" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "border-top:1px solid #DADADA;" & _
                                  "color:#454545;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                              "}" & _
                              ".DeviceList_leftBox" & _
                              "{" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-left:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_cell" & _
                              "{" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #454545;" & _
                              "}" & _
                              ".SHeader1" & _
                              "{" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 19px;" & _
                                  "font-weight:bold;" & _
                                  "height:30px;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1a1a1a;" & _
                              "}")
                sHTML.Append("</style>")
                sHTML.Append("</head>")
                sHTML.Append("<body>")
                sHTML.Append(HeaderHTML.ToString())
                sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                sHTML.Append("<tr style='height: 20px;'>")
                sHTML.Append("</tr>")
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(AddDatasToStringBuilder(dtTag, enumDeviceType.Tag, RptType, IsSavePdf, BatteryChanged, chkUnableToLocate, chkLocated, chkBatteryReplaced, , HeaderHTML.ToString))
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                'MONITOR DATA
                If Not dtTag Is Nothing Then
                    If dtTag.Rows.Count > 0 Then
                        sHTML.Append("<tr style='height: 50px;'><td>&nbsp;</td>")
                        sHTML.Append("</tr>")
                    End If
                    sHTML.Append("<tr>")
                    sHTML.Append("<td>")
                    sHTML.Append(AddDatasToStringBuilder(dtMonitor, enumDeviceType.Monitor, RptType, IsSavePdf, BatteryChangedMonitor, chkUnableToLocateMonitor, chkLocatedMonitor, chkBatteryReplacedMonitor, dtTag.Rows.Count, HeaderHTML.ToString))
                Else
                    sHTML.Append("<tr>")
                    sHTML.Append("<td>")
                    sHTML.Append(AddDatasToStringBuilder(dtMonitor, enumDeviceType.Monitor, RptType, IsSavePdf, BatteryChangedMonitor, chkUnableToLocateMonitor, chkLocatedMonitor, chkBatteryReplacedMonitor, 0, HeaderHTML.ToString))
                End If
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("</table>")
                sHTML.Append("</body>")
                sHTML.Append("</html>")

                File.AppendAllText(HTML_filepath, sHTML.ToString)

                Dim theID As Integer
                theDoc.Rect.String = "50 60 573 750"
                'theDoc.Rect.String = "50 60 573 640"
                'theID = theDoc.AddImageHtml(sHTML.ToString)
                theID = theDoc.AddImageUrl("file:///" & HTML_filepath)

                While True
                    'theDoc.FrameRect() ' add a black border
                    If Not theDoc.Chainable(theID) Then
                        Exit While
                    End If
                    theDoc.Page = theDoc.AddPage()
                    theID = theDoc.AddImageToChain(theID)
                End While


                '// add header
                'For k As Integer = 0 To theDoc.PageCount - 1
                '    theDoc.PageNumber = k
                '    'theDoc.Rect.String = "50 755 573 700"
                '    theDoc.Rect.String = "50 755 573 650"
                '    theDoc.AddImageHtml(HeaderHTML.ToString)
                'Next

                theDoc.Rect.Position(465, 594)
                theDoc.Rect.Width = 100
                theDoc.Rect.Height = 65

                Dim i As Integer
                For i = 1 To theDoc.PageCount
                    theDoc.PageNumber = i
                    theDoc.AddHtml("<p align='right'><font size='3px' color= '#8A8A8A' font-family= 'Arial'>Page&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & i & "</font><font size='3px' color= '#8A8A8A' font-family= 'Arial'>&nbsp;of&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & theDoc.PageCount & "</font></p>")
                    theDoc.Flatten()
                Next

                theDoc.SetInfo(theDoc.Root, "/OpenAction", "[ 1 0 R /XYZ null null 2 ]")

                Dim pdfbytestream As Byte() = theDoc.GetData()
                theDoc.Clear()
                theDoc.Dispose() 'free up unused object
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.Buffer = True
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & sFileName & ".pdf")
                HttpContext.Current.Response.AddHeader("content-length", pdfbytestream.Length.ToString())
                HttpContext.Current.Response.BinaryWrite(pdfbytestream)

                If File.Exists(HTML_filepath) Then
                    File.Delete(HTML_filepath)
                End If

                HttpContext.Current.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                '  HttpContext.Current.Response.Write("Error in generating the Pdf, please try again.")
                WriteLog(" DownloadPdf " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddDatasToStringBuilder(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String, Optional ByVal IsSavePdf As Boolean = False,
                                                Optional ByVal BatteryChanged As String() = Nothing, Optional ByVal chkUnableToLocate As String = "", Optional ByVal chkLocated As String = "",
                                                Optional ByVal chkBatteryReplaced As String = "", Optional ByVal ndtTagCount As Integer = 0, Optional ByVal HeaderHTML As String = "") As String
            Dim sHTML As New StringBuilder
            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim notFirstPageBreakSeen As Boolean
            Dim initialMonitorIdx As Integer = 0
            Dim pageBreakCount As Integer = 0

            Dim sCol As String = "align='center' style='height: 41px; width: 125px;'"
            Dim sCol1 As String = "align='center' style='height: 41px; width: 125px; background-color:#F9F8F8;'"

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")

            For nRowIdx = 0 To dt.Rows.Count - 1
                If DeviceType = enumDeviceType.Tag Then
                    If nRowIdx Mod 19 = 0 Then
                        If nRowIdx <> 0 Then
                            sHTML.Append("<tr>")
                            sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")

                            sHTML.Append("<tr>")
                            sHTML.Append("<td colspan='15'>")
                            sHTML.Append(HeaderHTML)
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                        End If

                        'Header
                        sHTML.Append("<tr>")
                        sHTML.Append("<td class='siteOverview_TopLeft_Box' " & sCol & ">Tag Id</td>")

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            sHTML.Append("<td class='siteOverview_Box' align='center' style='height: 41px; width: 150px; background-color:#F9F8F8;'>Monitor&nbsp;Location</td>")
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Monitor Id</td>")
                        End If

                        If RptType = "1" Or RptType = "4" Or RptType = "" Then
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />90 Days</td>")
                        End If

                        If RptType = "2" Or RptType = "4" Or RptType = "" Then
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />30 Days</td>")
                        End If

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Battery<br />Replaced<br />Date</td>")
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Unable To<br />Locate<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Located/<br />No Tag<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                            sHTML.Append("<td class='siteOverview_Topright_Box' width= 150px;" & sCol1 & ">Battery Replaced/<br />Not Functioning<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                        End If

                        sHTML.Append("</tr>")
                    End If

                    'Datas
                    sHTML.Append("<tr style='height:41px;'>")
                    sHTML.Append("<td class='DeviceList_leftBox' " & sCol & ">" & dt.Rows(nRowIdx).Item("TagId") & "</td>")

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        sHTML.Append("<td class='siteOverview_cell' " & sCol & ">" & TruncateString(CheckIsDBNull(dt.Rows(nRowIdx).Item("MonitorLocation"), False, "&nbsp;")) & "</td>")
                        sHTML.Append("<td class='siteOverview_cell' " & sCol & ">" & dt.Rows(nRowIdx).Item("MonitorId") & "&nbsp;</td>")
                    End If

                    If RptType = "1" Or RptType = "4" Or RptType = "" Then
                        If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen90Days"), False, 0) = 0 Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        Else
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & "><img src='" & GetServerPath() & "/Images/check_mark.png' /></td>")
                        End If
                    End If

                    If RptType = "2" Or RptType = "4" Or RptType = "" Then
                        If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen30Days"), False, 0) = 0 Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        Else
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & "><img src='" & GetServerPath() & "/Images/check_mark.png' /></td>")
                        End If
                    End If

                    If IsSavePdf Then
                        Dim sTxtBoxBatteryChanged As String = ""
                        Dim sChkBoxUnableToLocate As String = ""
                        Dim sChkBoxLocated As String = ""
                        Dim sChkBoxBatteryReplaced As String = ""

                        If Not BatteryChanged Is Nothing Then
                            If BatteryChanged(nRowIdx) <> "" Then
                                sTxtBoxBatteryChanged = BatteryChanged(nRowIdx)
                            Else
                                sTxtBoxBatteryChanged = "&nbsp;"
                            End If
                        Else
                            sTxtBoxBatteryChanged = "&nbsp;"
                        End If

                        If CheckIdExists(chkUnableToLocate, dt.Rows(nRowIdx).Item("TagId")) Then
                            sChkBoxUnableToLocate = "<img src='" & GetServerPath() & "/Images/checked_checkbox.png' />"
                        Else
                            sChkBoxUnableToLocate = "<img src='" & GetServerPath() & "/Images/checkbox_unchecked.png' />"
                        End If

                        If CheckIdExists(chkLocated, dt.Rows(nRowIdx).Item("TagId")) Then
                            sChkBoxLocated = "<img src='" & GetServerPath() & "/Images/checked_checkbox.png' />"
                        Else
                            sChkBoxLocated = "<img src='" & GetServerPath() & "/Images/checkbox_unchecked.png' />"
                        End If

                        If CheckIdExists(chkBatteryReplaced, dt.Rows(nRowIdx).Item("TagId")) Then
                            sChkBoxBatteryReplaced = "<img src='" & GetServerPath() & "/Images/checked_checkbox.png' />"
                        Else
                            sChkBoxBatteryReplaced = "<img src='" & GetServerPath() & "/Images/checkbox_unchecked.png' />"
                        End If

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            sHTML.Append("<td class='siteOverview_cell' >" & sTxtBoxBatteryChanged & "</td>")
                            sHTML.Append("<td class='siteOverview_cell' >" & sChkBoxUnableToLocate & "</td>")
                            sHTML.Append("<td class='siteOverview_cell' >" & sChkBoxLocated & "</td>")
                            sHTML.Append("<td class='siteOverview_cell' >" & sChkBoxBatteryReplaced & "</td>")
                        End If

                    Else
                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        End If
                    End If
                    sHTML.Append("</tr>")
                Else 'MONITOR
                    'Header
                    If Not notFirstPageBreakSeen Then
                        If ndtTagCount = 0 Then
                            notFirstPageBreakSeen = True
                        End If

                        If ((ndtTagCount Mod 19) + nRowIdx + 2) Mod 17 = 0 And ndtTagCount <> 0 Then
                            initialMonitorIdx = (ndtTagCount Mod 19)
                            pageBreakCount = 0
                   
                            notFirstPageBreakSeen = True

                            sHTML.Append("<tr>")
                            sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")

                            sHTML.Append("<tr>")
                            sHTML.Append("<td colspan='15'>")
                            sHTML.Append(HeaderHTML)
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                            notFirstPageBreakSeen = True

                            'Header
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_TopLeft_Box' " & sCol & ">Monitor Id</td>")

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                sHTML.Append("<td class='siteOverview_Box' align='center' style='height: 41px; width: 150px; background-color:#F9F8F8;'>Monitor&nbsp;Location</td>")
                            End If

                            If RptType = "1" Or RptType = "4" Or RptType = "" Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />90 Days</td>")
                            End If

                            If RptType = "2" Or RptType = "4" Or RptType = "" Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />30 Days</td>")
                            End If

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Battery<br />Replaced<br />Date</td>")
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Unable To<br />Locate<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Located/<br />No Tag<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                                sHTML.Append("<td class='siteOverview_Topright_Box' " & sCol1 & ">Battery Replaced/<br />Not Functioning<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                            End If

                            sHTML.Append("</tr>")
                        ElseIf nRowIdx = 0 Then 'Header when no tags
                            initialMonitorIdx = (ndtTagCount Mod 19)

                            If initialMonitorIdx > 14 Then
                                sHTML.Append("<tr>")
                                sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")

                                sHTML.Append("<tr>")
                                sHTML.Append("<td colspan='15'>")
                                sHTML.Append(HeaderHTML)
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                                notFirstPageBreakSeen = True
                            Else
                                initialMonitorIdx = initialMonitorIdx + 3

                            End If

                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_TopLeft_Box' " & sCol & ">Monitor Id</td>")

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                sHTML.Append("<td class='siteOverview_Box' align='center' style='height: 41px; width: 150px; background-color:#F9F8F8;'>Monitor&nbsp;Location</td>")
                            End If

                            If RptType = "1" Or RptType = "4" Or RptType = "" Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />90 Days</td>")
                            End If

                            If RptType = "2" Or RptType = "4" Or RptType = "" Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />30 Days</td>")
                            End If

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Battery<br />Replaced<br />Date</td>")
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Unable To<br />Locate<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Located/<br />No Tag<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                                sHTML.Append("<td class='siteOverview_Topright_Box' " & sCol1 & ">Battery Replaced/<br />Not Functioning<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                            End If

                            sHTML.Append("</tr>")
                        End If
                    Else
                        If pageBreakCount > 19 Then
                            If nRowIdx <> 0 Then
                                pageBreakCount = 0
                                sHTML.Append("<tr>")
                                sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")

                                sHTML.Append("<tr>")
                                sHTML.Append("<td colspan='15'>")
                                sHTML.Append(HeaderHTML)
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                                notFirstPageBreakSeen = True
                            End If

                            'Header
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_TopLeft_Box' " & sCol & ">Monitor Id</td>")

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                sHTML.Append("<td class='siteOverview_Box' align='center' style='height: 41px; width: 150px; background-color:#F9F8F8;'>Monitor&nbsp;Location</td>")
                            End If

                            If RptType = "1" Or RptType = "4" Or RptType = "" Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />90 Days</td>")
                            End If

                            If RptType = "2" Or RptType = "4" Or RptType = "" Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Less Than<br />30 Days</td>")
                            End If

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Battery<br />Replaced<br />Date</td>")
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Unable To<br />Locate<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                                sHTML.Append("<td class='siteOverview_Box' " & sCol1 & ">Located/<br />No Tag<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                                sHTML.Append("<td class='siteOverview_Topright_Box' " & sCol1 & ">Battery Replaced/<br />Not Functioning<br /><img src='" & GetServerPath() & "/Images/check_mark_parantheses.png' /></td>")
                            End If

                            sHTML.Append("</tr>")
                        End If
                    End If

                    'Datas
                    pageBreakCount = pageBreakCount + 1
                    sHTML.Append("<tr>")
                    sHTML.Append("<td class='DeviceList_leftBox' " & sCol & ">" & dt.Rows(nRowIdx).Item("DeviceId") & "</td>")

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        If dt.Rows(nRowIdx).Item("RoomName") = "" Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        Else
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">" & TruncateString(CheckIsDBNull(dt.Rows(nRowIdx).Item("RoomName"), False, "&nbsp;")) & "</td>")
                        End If
                    End If

                    If RptType = "1" Or RptType = "4" Or RptType = "" Then
                        If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen90Days"), False, 0) = 0 Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        Else
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & "><img src='" & GetServerPath() & "/Images/check_mark.png' /></td>")
                        End If
                    End If

                    If RptType = "2" Or RptType = "4" Or RptType = "" Then
                        If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen30Days"), False, 0) = 0 Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        Else
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & "><img src='" & GetServerPath() & "/Images/check_mark.png' /></td>")
                        End If
                    End If

                    If IsSavePdf Then
                        Dim sTxtBoxBatteryChanged As String = ""
                        Dim sChkBoxUnableToLocate As String = ""
                        Dim sChkBoxLocated As String = ""
                        Dim sChkBoxBatteryReplaced As String = ""

                        If Not BatteryChanged Is Nothing Then
                            If BatteryChanged(nRowIdx) <> "" Then
                                sTxtBoxBatteryChanged = BatteryChanged(nRowIdx)
                            Else
                                sTxtBoxBatteryChanged = "&nbsp;"
                            End If
                        Else
                            sTxtBoxBatteryChanged = "&nbsp;"
                        End If


                        If CheckIdExists(chkUnableToLocate, dt.Rows(nRowIdx).Item("DeviceId")) Then
                            sChkBoxUnableToLocate = "<img src='" & GetServerPath() & "/Images/checked_checkbox.png' />"
                        Else
                            sChkBoxUnableToLocate = "<img src='" & GetServerPath() & "/Images/checkbox_unchecked.png' />"
                        End If

                        If CheckIdExists(chkLocated, dt.Rows(nRowIdx).Item("DeviceId")) Then
                            sChkBoxLocated = "<img src='" & GetServerPath() & "/Images/checked_checkbox.png' />"
                        Else
                            sChkBoxLocated = "<img src='" & GetServerPath() & "/Images/checkbox_unchecked.png' />"
                        End If

                        If CheckIdExists(chkBatteryReplaced, dt.Rows(nRowIdx).Item("DeviceId")) Then
                            sChkBoxBatteryReplaced = "<img src='" & GetServerPath() & "/Images/checked_checkbox.png' />"
                        Else
                            sChkBoxBatteryReplaced = "<img src='" & GetServerPath() & "/Images/checkbox_unchecked.png' />"
                        End If

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            sHTML.Append("<td class='siteOverview_cell' >" & sTxtBoxBatteryChanged & "</td>")
                            sHTML.Append("<td class='siteOverview_cell' >" & sChkBoxUnableToLocate & "</td>")
                            sHTML.Append("<td class='siteOverview_cell' >" & sChkBoxLocated & "</td>")
                            sHTML.Append("<td class='siteOverview_cell' >" & sChkBoxBatteryReplaced & "</td>")
                        End If

                    Else
                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">&nbsp;</td>")
                        End If
                    End If
                    sHTML.Append("</tr>")
                End If
            Next

            sHTML.Append("</table>")

            Return sHTML.ToString()
        End Function

        Public Function DownloadExcel(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String, Optional ByVal BatteryChanged As String() = Nothing,
                                      Optional ByVal chkUnableToLocate As String = "", Optional ByVal chkLocated As String = "", Optional ByVal chkBatteryReplaced As String = "",
                                      Optional ByVal BatteryChangedMonitor As String() = Nothing, Optional ByVal chkUnableToLocateMonitor As String = "",
                                      Optional ByVal chkLocatedMonitor As String = "", Optional ByVal chkBatteryReplacedMonitor As String = "") As Boolean
            Dim oExcel As New Excel
            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable

            Dim sHTML As New StringBuilder
            Dim sSubHTML As StringBuilder
            Dim HeaderHTML As New StringBuilder
            Dim sVal As String() = Nothing
            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim apiKey As String = ""

            Dim bNoRecordFound As Boolean = False
            Dim IsExportforAllDevices As Boolean = False

            Try
                If (RptType = "0") Then
                    Sheetname = "Good-Device-List"
                ElseIf (RptType = "1") Then
                    Sheetname = "UnderWatch-List"
                ElseIf (RptType = "2") Then
                    Sheetname = "LBI-List"
                ElseIf (RptType = "3") Then
                    Sheetname = "Offline-List"
                ElseIf (RptType = "4") Then
                    Sheetname = "All-LBI"
                ElseIf (RptType = "6") Then
                    Sheetname = "Asset-Metadata"
                ElseIf (RptType = "") Then
                    Sheetname = "All-LBI"
                Else
                    Sheetname = ""
                End If

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                apiKey = g_UserAPI

                If RptType <> Nothing Or RptType <> "" Then
                    If g_UserAPI = "" Then
                        LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", apiKey, bNoRecordFound)
                    Else
                        LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", , bNoRecordFound)
                    End If
                Else
                    LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", "4", "", "", apiKey, bNoRecordFound)
                End If

                If bNoRecordFound Then
                    dtTag = Nothing
                    dtMonitor = Nothing
                End If

                'Header Content
                oExcel.CreateHeader("Report", sHTML)
                oExcel.CreateWorkSheetDatas(sHTML)
                oExcel.InsertDataHeader("<img src='" & GetServerPath() & "/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />", sHTML, False, 5, 2)
                oExcel.InsertDataHeader("&nbsp;", sHTML)
                oExcel.InsertDataHeader(SiteName, sHTML, True, 5)
                oExcel.InsertDataHeader("&nbsp;", sHTML)

                sSubHTML = New StringBuilder()
                oExcel.CreateWorkSheetDatas(sSubHTML)
                ReDim Preserve sVal(2)
                sVal(0) = "Date&nbsp;:&nbsp;<label>" & CDate(Now()).Date & "</label>"
                If RptType = "1" Then
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>Less Than 90 Days</label>"
                ElseIf RptType = "2" Then
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>Less Than 30 Days</label>"
                ElseIf RptType = "6" Then
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>Asset Meta Data</label>"
                Else
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>All LBI</label>"
                End If
                oExcel.InsertDataArray(sVal, sSubHTML, True)
                oExcel.CloseWorkSheetDatas(sSubHTML)
                oExcel.InsertData(sSubHTML.ToString(), sHTML)

                'Data Content
                oExcel.CreateWorkSheetDatas(sHTML)
                'TAG
                oExcel.InsertData(AddExcelDatasToStringBuilder(dtTag, enumDeviceType.Tag, RptType, BatteryChanged, chkUnableToLocate, chkLocated, chkBatteryReplaced), sHTML)
                'MONITOR
                oExcel.InsertData(AddExcelDatasToStringBuilder(dtMonitor, enumDeviceType.Monitor, RptType, BatteryChangedMonitor, chkUnableToLocateMonitor, chkLocatedMonitor, chkBatteryReplacedMonitor), sHTML)
                oExcel.CloseWorkSheetDatas(sHTML)

                'Download Excel
                oExcel.DownloadExcel(sFileName, sHTML)
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadExcel " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddExcelDatasToStringBuilder(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String, Optional ByVal BatteryChanged As String() = Nothing,
                                                     Optional ByVal chkUnableToLocate As String = "", Optional ByVal chkLocated As String = "",
                                                     Optional ByVal chkBatteryReplaced As String = "") As String
            Dim oExcel As New Excel

            Dim sHTML As New StringBuilder
            Dim sCol As String = "align='center' style='height: 41px; width: 125px;'"
            Dim sCol1 As String = "align='center' style='height: 41px; width: 125px; background-color:#F9F8F8;'"
            Dim sVal() As String = Nothing

            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            oExcel.CreateWorkSheetDatas(sHTML)

            For nRowIdx = 0 To dt.Rows.Count - 1
                If DeviceType = enumDeviceType.Tag Then
                    If nRowIdx = 0 Then
                        If RptType = "1" Then
                            ReDim Preserve sVal(8)
                            sVal(0) = "Tag Id"
                            sVal(1) = "Monitor Location"
                            sVal(2) = "Monitor Id"
                            sVal(3) = "Less Than 90 Days"
                            sVal(4) = "Battery Replaced Date"
                            sVal(5) = "Unable to Locate"
                            sVal(6) = "Located/No Tag"
                            sVal(7) = "Battery Replaced/Not Functioning"
                        ElseIf RptType = "2" Then
                            If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                                ReDim Preserve sVal(8)
                                sVal(0) = "Tag Id"
                                sVal(1) = "Less Than 30 Days"
                                sVal(2) = ""
                                sVal(3) = ""
                                sVal(4) = ""
                                sVal(5) = ""
                                sVal(6) = ""
                                sVal(7) = ""
                            Else
                                ReDim Preserve sVal(8)
                                sVal(0) = "Tag Id"
                                sVal(1) = "Monitor Location"
                                sVal(2) = "Monitor Id"
                                sVal(3) = "Less Than 30 Days"
                                sVal(4) = "Battery Replaced Date"
                                sVal(5) = "Unable to Locate"
                                sVal(6) = "Located/No Tag"
                                sVal(7) = "Battery Replaced/Not Functioning"
                            End If
                        ElseIf RptType = "4" Or RptType = "" Then
                            ReDim Preserve sVal(8)
                            sVal(0) = "Tag Id"
                            sVal(1) = "Monitor Location"
                            sVal(2) = "Monitor Id"
                            sVal(3) = "Less Than 90 Days"
                            sVal(4) = "Less Than 30 Days"
                            sVal(5) = "Battery Replaced Date"
                            sVal(6) = "Unable to Locate"
                            sVal(7) = "Located/No Tag"
                            sVal(8) = "Battery Replaced/Not Functioning"
                        End If

                        oExcel.InsertDataArray(sVal, sHTML, True)
                    End If

                    'Datas
                    If RptType = "4" Or RptType = "" Then
                        ReDim Preserve sVal(8)
                    ElseIf RptType = "1" Or RptType = "2" Then
                        ReDim Preserve sVal(7)
                    End If

                    sVal(0) = dt.Rows(nRowIdx).Item("TagId")

                    sVal(1) = TruncateString(CheckIsDBNull(dt.Rows(nRowIdx).Item("MonitorLocation"), False, "&nbsp;"))
                    sVal(2) = dt.Rows(nRowIdx).Item("MonitorId")

                    If RptType = "1" Or RptType = "4" Or RptType = "" Then
                        If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen90Days"), False, 0) = 0 Then
                            sVal(3) = "No"
                        Else
                            sVal(3) = "Yes"
                        End If
                    End If

                    If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                        If RptType = "2" Then
                            If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen30Days"), False, 0) = 0 Then
                                sVal(1) = "No"
                            Else
                                sVal(1) = "Yes"
                            End If
                        End If
                    Else
                        If RptType = "4" Or RptType = "" Or RptType = "2" Then
                            If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen30Days"), False, 0) = 0 Then
                                sVal(4) = "No"
                            Else
                                sVal(4) = "Yes"
                            End If
                        End If
                    End If

                    Dim sTxtBoxBatteryChanged As String = ""
                    Dim sChkBoxUnableToLocate As String = ""
                    Dim sChkBoxLocated As String = ""
                    Dim sChkBoxBatteryReplaced As String = ""

                    If Not BatteryChanged Is Nothing Then
                        If BatteryChanged(nRowIdx) <> "" Then
                            sTxtBoxBatteryChanged = BatteryChanged(nRowIdx)
                        Else
                            sTxtBoxBatteryChanged = "&nbsp;"
                        End If
                    Else
                        sTxtBoxBatteryChanged = "&nbsp;"
                    End If

                    If CheckIdExists(chkUnableToLocate, dt.Rows(nRowIdx).Item("TagId")) Then
                        sChkBoxUnableToLocate = "Yes"
                    Else
                        sChkBoxUnableToLocate = "No"
                    End If

                    If CheckIdExists(chkLocated, dt.Rows(nRowIdx).Item("TagId")) Then
                        sChkBoxLocated = "Yes"
                    Else
                        sChkBoxLocated = "No"
                    End If

                    If CheckIdExists(chkBatteryReplaced, dt.Rows(nRowIdx).Item("TagId")) Then
                        sChkBoxBatteryReplaced = "Yes"
                    Else
                        sChkBoxBatteryReplaced = "No"
                    End If

                    If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                        sVal(2) = ""
                        sVal(3) = ""
                        sVal(4) = ""
                        sVal(5) = ""
                        sVal(6) = ""
                        sVal(7) = ""
                    Else
                        If RptType = "4" Or RptType = "" Then
                            sVal(5) = ""
                            sVal(6) = ""
                            sVal(7) = ""
                            sVal(8) = ""
                        Else
                            sVal(4) = ""
                            sVal(5) = ""
                            sVal(6) = ""
                            sVal(7) = ""
                        End If
                    End If


                    oExcel.InsertDataArray(sVal, sHTML)
                Else 'MONITOR
                    If nRowIdx = 0 Then
                        If RptType = "1" Then
                            ReDim Preserve sVal(7)
                            sVal(0) = "Monitor Id"
                            sVal(1) = "Monitor Location"
                            sVal(2) = "Less Than 90 Days"
                            sVal(3) = "Battery Replaced Date"
                            sVal(4) = "Unable to Locate"
                            sVal(5) = "Located/No Tag"
                            sVal(6) = "Battery Replaced/Not Functioning"
                        ElseIf RptType = "2" Then
                            If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                                ReDim Preserve sVal(7)
                                sVal(0) = "Monitor Id"
                                sVal(1) = "Less Than 30 Days"
                                sVal(2) = ""
                                sVal(3) = ""
                                sVal(4) = ""
                                sVal(5) = ""
                                sVal(6) = ""
                            Else
                                ReDim Preserve sVal(7)
                                sVal(0) = "Monitor Id"
                                sVal(1) = "Monitor Location"
                                sVal(2) = "Less Than 30 Days"
                                sVal(3) = "Battery Replaced Date"
                                sVal(4) = "Unable to Locate"
                                sVal(5) = "Located/No Tag"
                                sVal(6) = "Battery Replaced/Not Functioning"
                            End If
                        ElseIf RptType = "4" Or RptType = "" Then
                            ReDim Preserve sVal(8)
                            sVal(0) = "Monitor Id"
                            sVal(1) = "Monitor Location"
                            sVal(2) = "Less Than 90 Days"
                            sVal(3) = "Less Than 30 Days"
                            sVal(4) = "Battery Replaced Date"
                            sVal(5) = "Unable to Locate"
                            sVal(6) = "Located/No Tag"
                            sVal(7) = "Battery Replaced/Not Functioning"
                        End If

                        oExcel.InsertDataArray(sVal, sHTML, True)
                    End If

                    'Datas
                    If RptType = "4" Or RptType = "" Then
                        ReDim Preserve sVal(8)
                    ElseIf RptType = "1" Or RptType = "2" Then
                        ReDim Preserve sVal(7)
                    End If

                    sVal(0) = dt.Rows(nRowIdx).Item("DeviceId")
                    sVal(1) = CheckIsDBNull(dt.Rows(nRowIdx).Item("RoomName"), False, "")

                    If RptType = "1" Or RptType = "4" Or RptType = "" Then
                        If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen90Days"), False, 0) = 0 Then
                            sVal(2) = "No"
                        Else
                            sVal(2) = "Yes"
                        End If
                    End If

                    If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                        If RptType = "2" Then
                            If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen30Days"), False, 0) = 0 Then
                                sVal(1) = "No"
                            Else
                                sVal(1) = "Yes"
                            End If
                        End If
                    Else
                        If RptType = "4" Or RptType = "" Or RptType = "2" Then
                            If CheckIsDBNull(dt.Rows(nRowIdx).Item("LessThen30Days"), False, 0) = 0 Then
                                sVal(3) = "No"
                            Else
                                sVal(3) = "Yes"
                            End If
                        End If
                    End If

                    Dim sTxtBoxBatteryChanged As String = ""
                    Dim sChkBoxUnableToLocate As String = ""
                    Dim sChkBoxLocated As String = ""
                    Dim sChkBoxBatteryReplaced As String = ""

                    If Not BatteryChanged Is Nothing Then
                        If BatteryChanged(nRowIdx) <> "" Then
                            sTxtBoxBatteryChanged = BatteryChanged(nRowIdx)
                        Else
                            sTxtBoxBatteryChanged = "&nbsp;"
                        End If
                    Else
                        sTxtBoxBatteryChanged = "&nbsp;"
                    End If

                    If CheckIdExists(chkUnableToLocate, dt.Rows(nRowIdx).Item("DeviceId")) Then
                        sChkBoxUnableToLocate = "Yes"
                    Else
                        sChkBoxUnableToLocate = "No"
                    End If

                    If CheckIdExists(chkLocated, dt.Rows(nRowIdx).Item("DeviceId")) Then
                        sChkBoxLocated = "Yes"
                    Else
                        sChkBoxLocated = "No"
                    End If

                    If CheckIdExists(chkBatteryReplaced, dt.Rows(nRowIdx).Item("DeviceId")) Then
                        sChkBoxBatteryReplaced = "Yes"
                    Else
                        sChkBoxBatteryReplaced = "No"
                    End If

                    If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.MaintenancePrism Then
                        sVal(2) = ""
                        sVal(3) = ""
                        sVal(4) = ""
                        sVal(5) = ""
                        sVal(6) = ""
                        sVal(7) = ""
                    Else
                        If RptType = "4" Or RptType = "" Then
                            sVal(4) = ""
                            sVal(5) = ""
                            sVal(6) = ""
                            sVal(7) = ""
                        Else
                            sVal(3) = ""
                            sVal(4) = ""
                            sVal(5) = ""
                            sVal(6) = ""
                        End If
                    End If
                    oExcel.InsertDataArray(sVal, sHTML)
                End If
            Next

            oExcel.CloseWorkSheetDatas(sHTML)

            Return sHTML.ToString()
        End Function

        Public Function DownloadPdfForAllDevices(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String, Optional ByVal IsSavePdf As Boolean = False,
                                                 Optional ByVal BatteryChanged As String() = Nothing, Optional ByVal chkUnableToLocate As String = "",
                                                 Optional ByVal chkLocated As String = "", Optional ByVal chkBatteryReplaced As String = "", Optional ByVal BatteryChangedMonitor As String() = Nothing,
                                                 Optional ByVal chkUnableToLocateMonitor As String = "", Optional ByVal chkLocatedMonitor As String = "", Optional ByVal chkBatteryReplacedMonitor As String = "",
                                                 Optional ByVal BatteryChangedStar As String() = Nothing, Optional ByVal chkUnableToLocateStar As String = "", Optional ByVal chkLocatedStar As String = "",
                                                 Optional ByVal chkBatteryReplacedStar As String = "") As Boolean
            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable
            Dim dtStar As New DataTable

            Dim theDoc As Doc = New Doc()
            theDoc.HtmlOptions.Timeout = 10000000
            theDoc.HtmlOptions.Engine = EngineType.Gecko

            Dim sHTML As New StringBuilder
            Dim HeaderHTML As New StringBuilder

            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim bNoRecordFound As Boolean = False
            Dim IsExportforAllDevices As Boolean = False

            Dim HTMLsaveFloder As String = "HTML"
            Dim HTML_filepath As String = ""
            Dim apiKey As String = ""

            Try

                If Not Directory.Exists(GetAppPath() & "\" & HTMLsaveFloder) Then
                    Directory.CreateDirectory(GetAppPath() & "\" & HTMLsaveFloder)
                End If

                If (RptType = "0") Then
                    Sheetname = "Good-Device-List"
                ElseIf (RptType = "1") Then
                    Sheetname = "UnderWatch-List"
                ElseIf (RptType = "2") Then
                    Sheetname = "LBI-List"
                ElseIf (RptType = "3") Then
                    Sheetname = "Offline-List"
                ElseIf (RptType = "4") Then
                    Sheetname = "All"
                ElseIf (RptType = "") Then
                    Sheetname = "All"
                Else
                    Sheetname = "AllDevices"
                End If

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                HTML_filepath = GetAppPath() & "\" & HTMLsaveFloder & "\" & sFileName & "_" & g_UserId & ".html"

                theDoc.Rect.Inset(20, 25)
                theDoc.Page = theDoc.AddPage()

                If RptType = "5" Then
                    IsExportforAllDevices = True
                End If

                If IsExportforAllDevices = False Then
                    dtStar = Nothing
                End If

                apiKey = g_UserAPI

                If RptType <> Nothing Or RptType <> "" Then
                    If g_UserAPI = "" Then
                        LoadAllDeviceListPrintPage(dtTag, dtMonitor, dtStar, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", apiKey, bNoRecordFound)
                    Else
                        LoadAllDeviceListPrintPage(dtTag, dtMonitor, dtStar, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", , bNoRecordFound)
                    End If

                Else
                    LoadAllDeviceListPrintPage(dtTag, dtMonitor, dtStar, Val(SiteId), "0", "", "", "0", "", "4", "", "", apiKey, bNoRecordFound)
                End If

                If bNoRecordFound Then
                    dtTag = Nothing
                    dtMonitor = Nothing
                End If

                'Header Content
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td><img src='" & GetServerPath() & "/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td align='left' style='border-bottom: solid 5px #245E90;'>")
                HeaderHTML.Append("&nbsp;</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td valign='top' align='center'>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; color: #373737; border-bottom: solid 1px #D3D3D3;height:40px;' valign='middle' class='sText'>")
                HeaderHTML.Append("<label class='SHeader1'>")
                HeaderHTML.Append(SiteName)
                HeaderHTML.Append("</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%' height='40px'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; width: 20%; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Date&nbsp;:&nbsp;")
                HeaderHTML.Append("<label>" & CDate(Now()).Date & "</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Report&nbsp;Type&nbsp;:&nbsp;")

                If RptType = "1" Then
                    HeaderHTML.Append("<label>Less Than 90 Days</label>")
                ElseIf RptType = "2" Then
                    HeaderHTML.Append("<label>Less Than 30 Days</label>")
                Else
                    HeaderHTML.Append("<label>All Devices</label>")
                End If

                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")

                'BOBY Content
                sHTML.Append("<html>")
                sHTML.Append("<head>")
                sHTML.Append("<style>")
                sHTML.Append(".siteOverview_TopLeft_Box" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "background-image: url('" & GetServerPath() & "/Images/tblHeaderbg.png');" & _
                                  "background-repeat:no-repeat;background-size:130px 60px;" & _
                                 "color:White;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                              "}" & _
                              ".SHeader1" & _
                              "{" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 19px;" & _
                                  "font-weight:bold;" & _
                                  "height:30px;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1a1a1a;" & _
                              "}" & _
                              ".siteOverview_Header" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "-webkit-border-top-left-radius: 5px;" & _
                                  "-moz-border-top-left-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "border-top:1px solid #1F5E93;" & _
                                  "border-left:1px solid #1F5E93;" & _
                                  "border-bottom:1px solid #1F5E93;" & _
                                  "border-right:1px solid #1F5E93;" & _
                                  "color:#FFFFFF;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  " background-color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_HeaderLeft" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "-webkit-border-top-left-radius: 5px;" & _
                                  "-moz-border-top-left-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "border-top:1px solid #1F5E93;" & _
                                  "border-left:1px solid #1F5E93;" & _
                                  "border-bottom:1px solid #1F5E93;" & _
                                  "border-right:1px solid #1F5E93;" & _
                                  "color:#FFFFFF;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "padding-left: 5px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:left;" & _
                                  "vertical-align: middle;" & _
                                  " background-color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_Box" & _
                              "{" & _
                                  "border-top:1px solid #DADADA;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #454545;" & _
                              "}" & _
                              ".siteOverview_Topright_Box" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "border-top:1px solid #DADADA;" & _
                                  "color:#454545;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                              "}" & _
                              ".DeviceList_leftBox" & _
                              "{" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-left:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_cell" & _
                              "{" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1F5E93;" & _
                              "}" & _
                              ".SHeader1" & _
                              "{" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 19px;" & _
                                  "font-weight:bold;" & _
                                  "height:30px;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1a1a1a;" & _
                              "}")
                sHTML.Append("</style>")
                sHTML.Append("</head>")
                sHTML.Append("<body>")
                sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(HeaderHTML.ToString())
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(AddDatasToStringBuilderForAllDevices(dtTag, enumDeviceType.Tag, RptType, IsSavePdf, BatteryChanged, chkUnableToLocate, chkLocated, chkBatteryReplaced, , , HeaderHTML.ToString()))
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                'MONITOR DATA
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                If Not dtTag Is Nothing Then
                    sHTML.Append(AddDatasToStringBuilderForAllDevices(dtMonitor, enumDeviceType.Monitor, RptType, IsSavePdf, BatteryChangedMonitor, chkUnableToLocateMonitor, chkLocatedMonitor, chkBatteryReplacedMonitor, dtTag.Rows.Count, , HeaderHTML.ToString()))
                Else
                    sHTML.Append(AddDatasToStringBuilderForAllDevices(dtMonitor, enumDeviceType.Monitor, RptType, IsSavePdf, BatteryChangedMonitor, chkUnableToLocateMonitor, chkLocatedMonitor, chkBatteryReplacedMonitor, 0, , HeaderHTML.ToString()))
                End If
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                'STAR DATA
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                If Not dtTag Is Nothing And Not dtMonitor Is Nothing Then
                    sHTML.Append(AddDatasToStringBuilderForAllDevices(dtStar, enumDeviceType.Star, RptType, IsSavePdf, BatteryChangedStar, chkUnableToLocateStar, chkLocatedStar, chkBatteryReplacedStar, (dtTag.Rows.Count + dtMonitor.Rows.Count), , HeaderHTML.ToString()))
                Else
                    sHTML.Append(AddDatasToStringBuilderForAllDevices(dtStar, enumDeviceType.Star, RptType, IsSavePdf, BatteryChangedStar, chkUnableToLocateStar, chkLocatedStar, chkBatteryReplacedStar, 0, , HeaderHTML.ToString()))
                End If
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("</table>")
                sHTML.Append("</body>")
                sHTML.Append("</html>")

                File.AppendAllText(HTML_filepath, sHTML.ToString)

                Dim theID As Integer
                'theID = theDoc.AddImageUrl("file:///" & "c:\testsdfsaree\", True, 0, True)

                theDoc.Rect.String = "50 60 573 750"
                'theID = theDoc.AddImageHtml(sHTML.ToString)
                theID = theDoc.AddImageUrl("file:///" & HTML_filepath)

                While True
                    'theDoc.FrameRect() ' add a black border
                    If Not theDoc.Chainable(theID) Then
                        Exit While
                    End If
                    theDoc.Page = theDoc.AddPage()
                    theID = theDoc.AddImageToChain(theID)
                End While


                ''// add header
                'For k As Integer = 0 To theDoc.PageCount - 1
                '    theDoc.PageNumber = k
                '    'theDoc.Rect.String = "50 755 573 700"
                '    theDoc.Rect.String = "50 755 573 650"
                '    theDoc.AddImageHtml(HeaderHTML.ToString)
                'Next

                theDoc.Rect.Position(467, 594)
                theDoc.Rect.Width = 100
                theDoc.Rect.Height = 65

                Dim i As Integer
                For i = 1 To theDoc.PageCount
                    theDoc.PageNumber = i
                    theDoc.AddHtml("<p align='right'><font size='3px' color= '#8A8A8A' font-family= 'Arial'>Page&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & i & "</font><font size='3px' color= '#8A8A8A' font-family= 'Arial'>&nbsp;of&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & theDoc.PageCount & "</font></p>")
                    theDoc.Flatten()
                Next

                theDoc.SetInfo(theDoc.Root, "/OpenAction", "[ 1 0 R /XYZ null null 2 ]")

                Dim pdfbytestream As Byte() = theDoc.GetData()
                theDoc.Clear()
                theDoc.Dispose() 'free up unused object
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.Buffer = True
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & sFileName & ".pdf")
                HttpContext.Current.Response.AddHeader("content-length", pdfbytestream.Length.ToString())
                HttpContext.Current.Response.BinaryWrite(pdfbytestream)

                If File.Exists(HTML_filepath) Then
                    File.Delete(HTML_filepath)
                End If

                HttpContext.Current.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                '  HttpContext.Current.Response.Write("Error in generating the Pdf, please try again.")
                WriteLog(" DownloadPdf " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddDatasToStringBuilderForAllDevices(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String, Optional ByVal IsSavePdf As Boolean = False,
                                                             Optional ByVal BatteryChanged As String() = Nothing, Optional ByVal chkUnableToLocate As String = "", Optional ByVal chkLocated As String = "",
                                                             Optional ByVal chkBatteryReplaced As String = "", Optional ByVal ndtTagCount As Integer = 0, Optional ByVal IsExportforAllDevices As Boolean = False,
                                                             Optional ByVal headerHtml As String = "") As String
            Dim sHTML As New StringBuilder
            Dim sHTMLCell As New StringBuilder
            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim initialMonitorIdx As Integer = 0
            Dim initialStarIdx As Integer = 0
            Dim sClass As String = ""
            Dim previousTagType As String = ""
            Dim previousMonitorType As String = ""
            Dim nRows As Integer = 0
            Dim nColumns As Integer = 0

            Dim sCol As String = "align='center' style='height: 27px; width: 20%;'"
            Dim sCol1 As String = "align='center' style='height: 27px; width: 20%; background-color:#F9F8F8;'"

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
            sClass = "DeviceList_leftBox"
            nRows = 0
            For nRowIdx = 0 To dt.Rows.Count - 1
                If DeviceType = enumDeviceType.Tag Then
                    If nRows >= 30 Then
                        If nRows <> 0 Then
                            nRows = nRows - 30
                            sHTML.Append("<tr>")
                            sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")

                            sHTML.Append("<tr>")
                            sHTML.Append("<td colspan='15'>" & headerHtml)
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")

                        End If

                        'Header
                        sHTML.Append("<tr>")
                        sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("TagType").ToString.ToUpper & "</td>")
                        sHTML.Append("</tr>")
                        previousTagType = dt.Rows(nRowIdx).Item("TagType")
                        nRows = nRows + 1
                    End If

                    'Datas
                    If nColumns = 5 Or dt.Rows(nRowIdx).Item("TagType") <> previousTagType Then
                        nColumns = 0
                        nRows = nRows + 1
                        sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                        sHTMLCell = New StringBuilder()
                        sClass = "DeviceList_leftBox"
                    End If

                    'sub Header
                    If dt.Rows(nRowIdx).Item("TagType") <> previousTagType Then
                        previousTagType = dt.Rows(nRowIdx).Item("TagType")

                        If (nRows + 3) >= 30 Then ' Check to see if header is not printed alone else move to next page
                            If nRows <> 0 Then
                                nRows = 0
                                sHTML.Append("<tr>")
                                sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")

                                sHTML.Append("<tr>")
                                sHTML.Append("<td colspan='15'>" & headerHtml)
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                            End If
                            'Header
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("TagType").ToString.ToUpper & "</td>")
                            sHTML.Append("</tr>")
                            previousTagType = dt.Rows(nRowIdx).Item("TagType")
                            nRows = nRows + 1
                        Else
                            sHTML.Append("<tr style='height:27px;'>")
                            sHTML.Append("<td>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5' >" & dt.Rows(nRowIdx).Item("TagType").ToString.ToUpper & "</td>")
                            sHTML.Append("</tr>")
                            nRows = nRows + 2
                        End If
                    End If



                    sHTMLCell.Append("<td class='" & sClass & "' " & sCol & ">" & dt.Rows(nRowIdx).Item("TagId") & "</td>")
                    sClass = "siteOverview_cell"

                    If nRowIdx >= dt.Rows.Count - 1 Then
                        sHTML.Append("<tr style='height:26px;'>" & sHTMLCell.ToString() & "</tr>")
                        sHTMLCell = New StringBuilder()
                        sClass = "DeviceList_leftBox"
                    End If

                ElseIf DeviceType = enumDeviceType.Monitor Then 'MONITOR
                    If nRows >= 30 Or nRows = 0 Then
                        If ndtTagCount > 0 Or nRows <> 0 Then
                            sHTML.Append("<tr>")
                            sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                            sHTML.Append("<tr>")
                            sHTML.Append("<td colspan='15'>" & headerHtml)
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                            nRows = 0
                        End If

                        'Header
                        sHTML.Append("<tr>")
                        sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("MonitorType").ToString.ToUpper & "</td>")
                        sHTML.Append("</tr>")
                        previousMonitorType = dt.Rows(nRowIdx).Item("MonitorType")
                        nRows = nRows + 1
                    End If

                    'Datas
                    If nColumns = 5 Or dt.Rows(nRowIdx).Item("MonitorType") <> previousMonitorType Then
                        nRows = nRows + 1
                        nColumns = 0
                        sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                        sHTMLCell = New StringBuilder()
                        sClass = "DeviceList_leftBox"
                    End If

                    If dt.Rows(nRowIdx).Item("MonitorType") <> previousMonitorType Then
                        previousMonitorType = dt.Rows(nRowIdx).Item("MonitorType")
                        If nRows + 3 >= 30 Then ' Check to see if header is not printed alone else move to next page
                            If ndtTagCount > 0 Or nRows <> 0 Then
                                sHTML.Append("<tr>")
                                sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                                sHTML.Append("<tr>")
                                sHTML.Append("<td colspan='15'>" & headerHtml)
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                                nRows = 0
                            End If

                            'Header
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("MonitorType").ToString.ToUpper & "</td>")
                            sHTML.Append("</tr>")
                            previousMonitorType = dt.Rows(nRowIdx).Item("MonitorType")
                            nRows = nRows + 1
                        Else
                            'Header
                            sHTML.Append("<tr style='height:27px;'>")
                            sHTML.Append("<td>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("MonitorType").ToString.ToUpper & "</td>")
                            sHTML.Append("</tr>")
                            nRows = nRows + 2

                        End If


                    End If
                    sHTMLCell.Append("<td class='" & sClass & "' " & sCol & ">" & dt.Rows(nRowIdx).Item("DeviceId") & "</td>")
                    sClass = "siteOverview_cell"

                    If nRowIdx >= dt.Rows.Count - 1 Then
                        sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                        sHTMLCell = New StringBuilder()
                        sClass = "DeviceList_leftBox"
                    End If

                ElseIf DeviceType = enumDeviceType.Star Then 'STAR
                    If nRowIdx Mod 150 = 0 Or nRowIdx = 0 Then
                        If ndtTagCount > 0 Or nRowIdx <> 0 Then
                            sHTML.Append("<tr>")
                            sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                            sHTML.Append("<tr>")
                            sHTML.Append("<td colspan='15'>" & headerHtml)
                            sHTML.Append("</td>")
                            sHTML.Append("</tr>")
                        End If

                        'Header
                        sHTML.Append("<tr>")
                        sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>STARS</td>")
                        sHTML.Append("</tr>")
                    End If

                    'Datas
                    If nRowIdx Mod 5 = 0 Then
                        sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                        sHTMLCell = New StringBuilder()
                        sClass = "DeviceList_leftBox"
                    End If

                    sHTMLCell.Append("<td class='" & sClass & "' " & sCol & ">" & dt.Rows(nRowIdx).Item("MacId") & "</td>")
                    sClass = "siteOverview_cell"

                    If nRowIdx >= dt.Rows.Count - 1 Then
                        sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                    End If
                End If
                'nRows = nRows + 1
                nColumns = nColumns + 1
            Next

            sHTML.Append("</table>")

            Return sHTML.ToString()
        End Function

        Public Function DownloadExcelForAllDevices(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String) As Boolean
            Dim oExcel As New Excel
            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable
            Dim dtStar As New DataTable

            Dim sHTML As New StringBuilder
            Dim sSubHTML As StringBuilder
            Dim HeaderHTML As New StringBuilder
            Dim sVal As String() = Nothing
            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim apiKey As String = ""

            Dim bNoRecordFound As Boolean = False
            Dim IsExportforAllDevices As Boolean = False

            Try
                If (RptType = "0") Then
                    Sheetname = "Good-Device-List"
                ElseIf (RptType = "1") Then
                    Sheetname = "UnderWatch-List"
                ElseIf (RptType = "2") Then
                    Sheetname = "LBI-List"
                ElseIf (RptType = "3") Then
                    Sheetname = "Offline-List"
                ElseIf (RptType = "4") Then
                    Sheetname = "All"
                ElseIf (RptType = "") Then
                    Sheetname = "All"
                Else
                    Sheetname = "AllDevices"
                End If

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                If RptType = "5" Then
                    IsExportforAllDevices = True
                End If

                If IsExportforAllDevices = False Then
                    dtStar = Nothing
                End If

                apiKey = g_UserAPI

                If RptType <> Nothing Or RptType <> "" Then
                    If g_UserAPI = "" Then
                        LoadAllDeviceListPrintPage(dtTag, dtMonitor, dtStar, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", apiKey, bNoRecordFound)
                    Else
                        LoadAllDeviceListPrintPage(dtTag, dtMonitor, dtStar, Val(SiteId), "0", "", "", "0", "", Val(RptType), "", "", , bNoRecordFound)
                    End If
                Else
                    LoadAllDeviceListPrintPage(dtTag, dtMonitor, dtStar, Val(SiteId), "0", "", "", "0", "", "4", "", "", apiKey, bNoRecordFound)
                End If

                If bNoRecordFound Then
                    dtTag = Nothing
                    dtMonitor = Nothing
                End If


                'Header Content
                oExcel.CreateHeader("Report", sHTML)
                oExcel.CreateWorkSheetDatas(sHTML)
                oExcel.InsertDataHeader("<img src='" & GetServerPath() & "/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />", sHTML, False, 5, 2)
                oExcel.InsertDataHeader("&nbsp;", sHTML)
                oExcel.InsertDataHeader(SiteName, sHTML, True, 5)
                oExcel.InsertDataHeader("&nbsp;", sHTML)


                sSubHTML = New StringBuilder()
                oExcel.CreateWorkSheetDatas(sSubHTML)
                ReDim Preserve sVal(2)
                sVal(0) = "Date&nbsp;:&nbsp;<label>" & CDate(Now()).Date & "</label>"
                If RptType = "1" Then
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>Less Than 90 Days</label>"
                ElseIf RptType = "2" Then
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>Less Than 30 Days</label>"
                Else
                    sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>All Devices</label>"
                End If
                oExcel.InsertDataArray(sVal, sSubHTML, True, 1)
                oExcel.CloseWorkSheetDatas(sSubHTML)
                oExcel.InsertData(sSubHTML.ToString(), sHTML)

                'Data Content
                oExcel.CreateWorkSheetDatas(sHTML)
                'TAG
                oExcel.InsertData(AddDatasToStringBuilderForExcelAllDevices(dtTag, enumDeviceType.Tag, RptType), sHTML)
                'MONITOR
                oExcel.InsertData(AddDatasToStringBuilderForExcelAllDevices(dtMonitor, enumDeviceType.Monitor, RptType), sHTML)
                'STAR
                oExcel.InsertData(AddDatasToStringBuilderForExcelAllDevices(dtStar, enumDeviceType.Star, RptType), sHTML)
                oExcel.CloseWorkSheetDatas(sHTML)

                'Download Excel
                oExcel.DownloadExcel(sFileName, sHTML)
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadExcelForAllDevices " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddDatasToStringBuilderForExcelAllDevices(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String) As String
            Dim oExcel As New Excel

            Dim sHTML As New StringBuilder
            Dim sVal() As String = Nothing
            Dim sCol As String = "align='center' style='height: 27px; width: 20%;'"
            Dim sCol1 As String = "align='center' style='height: 27px; width: 20%; background-color:#F9F8F8;'"

            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim nInnerIdx As Integer = 0
            Dim nColumns As Integer = 0

            Dim sTagType As String = ""
            Dim OldTagType As String = ""

            Dim sMonitorType As String = ""
            Dim OldMonitorType As String = ""

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            oExcel.CreateWorkSheetDatas(sHTML)

            ReDim Preserve sVal(4)

            For nRowIdx = 0 To dt.Rows.Count - 1
                If DeviceType = enumDeviceType.Tag Then
                    If nRowIdx = 0 Then
                        oExcel.InsertDataHeader("Tag List", sHTML, True, 5)
                        oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("TagType"), sHTML, False, 5)
                        OldTagType = dt.Rows(nRowIdx).Item("TagType")
                    End If

                    sTagType = dt.Rows(nRowIdx).Item("TagType")

                    If (nColumns = 5 And nRowIdx > 0) Or OldTagType <> sTagType Then
                        nColumns = 0
                        oExcel.InsertDataArray(sVal, sHTML)
                        sVal = Nothing
                        nInnerIdx = 0
                        ReDim Preserve sVal(4)
                    End If

                    If OldTagType <> sTagType Then
                        oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("TagType"), sHTML, False, 5)
                        OldTagType = sTagType
                    End If

                    sVal(nInnerIdx) = dt.Rows(nRowIdx).Item("TagId")
                    nInnerIdx += 1
                    nColumns = nColumns + 1


                    If nRowIdx >= dt.Rows.Count - 1 Then
                        oExcel.InsertDataArray(sVal, sHTML)
                        sVal = Nothing
                    End If



                ElseIf DeviceType = enumDeviceType.Monitor Then 'MONITOR
                    If nRowIdx = 0 Then
                        oExcel.InsertDataHeader("Monitor List", sHTML, True, 5)
                        oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("MonitorType"), sHTML, False, 5)
                        OldMonitorType = dt.Rows(nRowIdx).Item("MonitorType")
                        nColumns = 0
                    End If

                    sMonitorType = dt.Rows(nRowIdx).Item("MonitorType")

                    If (nColumns = 5 And nRowIdx > 0) Or OldMonitorType <> sMonitorType Then
                        nColumns = 0
                        oExcel.InsertDataArray(sVal, sHTML)
                        sVal = Nothing
                        nInnerIdx = 0
                        ReDim Preserve sVal(4)
                    End If
                    If OldMonitorType <> sMonitorType Then
                        oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("MonitorType"), sHTML, False, 5)
                        OldMonitorType = sMonitorType
                    End If

                    sVal(nInnerIdx) = dt.Rows(nRowIdx).Item("DeviceId")
                    nInnerIdx += 1
                    nColumns = nColumns + 1

                    If nRowIdx >= dt.Rows.Count - 1 Then
                        oExcel.InsertDataArray(sVal, sHTML)
                        sVal = Nothing
                    End If

                ElseIf DeviceType = enumDeviceType.Star Then 'STAR
                    If nRowIdx = 0 Then
                        oExcel.InsertDataHeader("Star List", sHTML, True, 5)
                    End If

                    If nRowIdx Mod 5 = 0 And nRowIdx > 0 Then
                        oExcel.InsertDataArray(sVal, sHTML)
                        sVal = Nothing
                        nInnerIdx = 0
                        ReDim Preserve sVal(4)
                    End If

                    sVal(nInnerIdx) = dt.Rows(nRowIdx).Item("MacId")
                    nInnerIdx += 1

                    If nRowIdx >= dt.Rows.Count - 1 Then
                        oExcel.InsertDataArray(sVal, sHTML)
                        sVal = Nothing
                    End If
                End If
            Next

            oExcel.CloseWorkSheetDatas(sHTML)

            Return sHTML.ToString()
        End Function

        Public Sub LoadTableForPdf(ByVal SiteId As Integer, ByVal RptType As String, ByVal apikey As String, ByRef tblTagInfo As HtmlTable, ByRef tblMonitorInfo As HtmlTable)

            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable
            Dim tblRow As HtmlTableRow

            Dim sbRpt As New StringBuilder
            Dim nTagIdx As Integer = 0
            Dim nMntrIdx As Integer = 0
            Dim bNoRecordFound As Boolean = False

            If RptType <> Nothing Or RptType <> "" Then
                If apikey <> "" Then
                    LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", RptType, "", "", apikey, bNoRecordFound)
                Else
                    LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", RptType, "", "", , bNoRecordFound)
                End If
            Else

                apikey = g_UserAPI

                LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", "4", "", "", apikey, bNoRecordFound)
            End If

            If Not bNoRecordFound Then
                tblTagInfo.Rows.Clear()

                If dtTag.Rows.Count > 0 Then

                    'Header
                    tblRow = New HtmlTableRow()
                    AddCell(tblRow, "Tag&nbsp;Id", "center", , "70px", , , "40px", "siteOverview_TopLeft_Box")

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        AddCell(tblRow, "Monitor&nbsp;Location", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Monitor&nbsp;Id", "center", , "70px", , , "40px", "siteOverview_Box")
                    End If

                    If RptType = "1" Or RptType = "4" Or RptType = "" Then
                        AddCell(tblRow, "Less&nbsp;Than<br />90&nbsp;Days", "center", , "70px", , , "40px", "siteOverview_Box")
                    End If

                    If RptType = "2" Or RptType = "4" Or RptType = "" Then
                        AddCell(tblRow, "Less&nbsp;Than<br />30&nbsp;Days", "center", , "70px", , , "40px", "siteOverview_Box")
                    End If

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        AddCell(tblRow, "Battery<br />Replaced&nbsp;Date", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Unable&nbsp;To<br />Locate", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Located/<br />No&nbsp;Tag", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Battery&nbsp;Replaced/<br />Not&nbsp;Functioning", "center", , "70px", , , "40px", "siteOverview_Topright_Box")
                    End If

                    tblTagInfo.Rows.Add(tblRow)
                End If

                For nTagIdx = 0 To dtTag.Rows.Count - 1
                    With dtTag.Rows(nTagIdx)
                        tblRow = New HtmlTableRow()
                        AddCell(tblRow, .Item("TagId"), "center", , , , , "40px", "DeviceList_leftBox")

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            AddCell(tblRow, .Item("MonitorLocation"), "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, .Item("MonitorId"), "center", , , , , "40px", "siteOverview_cell")
                        End If

                        If RptType = "1" Or RptType = "4" Or RptType = "" Then
                            If CheckIsDBNull(.Item("LessThen90Days"), False, 0) = 0 Then
                                AddCell(tblRow, "&nbsp;", "center", , , , , "40px", "siteOverview_cell")
                            Else
                                AddCell(tblRow, "<img src='Images/check_mark.png' />", "center", , , , , "40px", "siteOverview_cell")
                            End If
                        End If

                        If RptType = "2" Or RptType = "4" Or RptType = "" Then
                            If CheckIsDBNull(.Item("LessThen30Days"), False, 0) = 0 Then
                                AddCell(tblRow, "&nbsp;", "center", , , , , "40px", "siteOverview_cell")
                            Else
                                AddCell(tblRow, "<img src='Images/check_mark.png' />", "center", , , , , "40px", "siteOverview_cell")
                            End If
                        End If

                        Dim sTxtBoxBatteryChanged As String = ""
                        Dim sChkBoxUnableToLocate As String = ""
                        Dim sChkBoxLocated As String = ""
                        Dim sChkBoxBatteryReplaced As String = ""

                        sTxtBoxBatteryChanged = "<input type='text' id='txtBatteryChanged" & .Item("TagId") & "' name='txtBatteryChanged' style='width: 65px;' />"
                        sChkBoxUnableToLocate = "<input type='checkbox' id='chkUnableToLocate" & .Item("TagId") & "' name='chkUnableToLocate' value='" & .Item("TagId") & "' />"
                        sChkBoxLocated = "<input type='checkbox' id='chkLocated" & .Item("TagId") & "' name='chkLocated' value='" & .Item("TagId") & "' />"
                        sChkBoxBatteryReplaced = "<input type='checkbox' id='chkBatteryReplaced" & .Item("TagId") & "' name='chkBatteryReplaced' value='" & .Item("TagId") & "' />"

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            AddCell(tblRow, sTxtBoxBatteryChanged, "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, sChkBoxUnableToLocate, "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, sChkBoxLocated, "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, sChkBoxBatteryReplaced, "center", , , , , "40px", "siteOverview_cell")
                        End If

                        tblTagInfo.Rows.Add(tblRow)
                    End With
                Next

                'Monitor Data
                tblMonitorInfo.Rows.Clear()

                If dtMonitor.Rows.Count > 0 Then
                    'Header
                    tblRow = New HtmlTableRow()
                    AddCell(tblRow, "Monitor&nbsp;Id", "center", , "70px", , , "40px", "siteOverview_TopLeft_Box")

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        AddCell(tblRow, "Monitor&nbsp;Location", "center", , "70px", , , "40px", "siteOverview_Box")
                    End If

                    If RptType = "1" Or RptType = "4" Or RptType = "" Then
                        AddCell(tblRow, "Less&nbsp;Than<br />90&nbsp;Days", "center", , "70px", , , "40px", "siteOverview_Box")
                    End If

                    If RptType = "2" Or RptType = "4" Or RptType = "" Then
                        AddCell(tblRow, "Less&nbsp;Than<br />30&nbsp;Days", "center", , "70px", , , "40px", "siteOverview_Box")
                    End If

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        AddCell(tblRow, "Battery<br />Replaced&nbsp;Date", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Unable&nbsp;To<br />Locate", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Located/<br />No&nbsp;Tag", "center", , "70px", , , "40px", "siteOverview_Box")
                        AddCell(tblRow, "Battery&nbsp;Replaced/<br />Not&nbsp;Functioning", "center", , "70px", , , "40px", "siteOverview_Topright_Box")
                    End If

                    tblMonitorInfo.Rows.Add(tblRow)
                End If

                For nTagIdx = 0 To dtMonitor.Rows.Count - 1
                    With dtMonitor.Rows(nTagIdx)
                        tblRow = New HtmlTableRow()
                        AddCell(tblRow, .Item("DeviceId"), "center", , , , , "40px", "DeviceList_leftBox")

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            AddCell(tblRow, .Item("RoomName"), "center", , , , , "40px", "siteOverview_cell")
                        End If

                        If RptType = "1" Or RptType = "4" Or RptType = "" Then
                            If CheckIsDBNull(.Item("LessThen90Days"), False, 0) = 0 Then
                                AddCell(tblRow, "&nbsp;", "center", , , , , "40px", "siteOverview_cell")
                            Else
                                AddCell(tblRow, "<img src='Images/check_mark.png' />", "center", , , , , "40px", "siteOverview_cell")
                            End If
                        End If

                        If RptType = "2" Or RptType = "4" Or RptType = "" Then
                            If CheckIsDBNull(.Item("LessThen30Days"), False, 0) = 0 Then
                                AddCell(tblRow, "&nbsp;", "center", , , , , "40px", "siteOverview_cell")
                            Else
                                AddCell(tblRow, "<img src='Images/check_mark.png' />", "center", , , , , "40px", "siteOverview_cell")
                            End If
                        End If

                        Dim sTxtBoxBatteryChangedMonitor As String = ""
                        Dim sChkBoxUnableToLocateMonitor As String = ""
                        Dim sChkBoxLocatedMonitor As String = ""
                        Dim sChkBoxBatteryReplacedMonitor As String = ""

                        sTxtBoxBatteryChangedMonitor = "<input type='text' id='txtBatteryChangedMonitor" & .Item("DeviceId") & "' name='txtBatteryChangedMonitor' style='width: 65px;' />"
                        sChkBoxUnableToLocateMonitor = "<input type='checkbox' id='chkUnableToLocateMonitor" & .Item("DeviceId") & "' name='chkUnableToLocateMonitor' value='" & .Item("DeviceId") & "' />"
                        sChkBoxLocatedMonitor = "<input type='checkbox' id='chkLocatedMonitor" & .Item("DeviceId") & "' name='chkLocatedMonitor' value='" & .Item("DeviceId") & "' />"
                        sChkBoxBatteryReplacedMonitor = "<input type='checkbox' id='chkBatteryReplacedMonitor" & .Item("DeviceId") & "' name='chkBatteryReplacedMonitor' value='" & .Item("DeviceId") & "' />"

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            AddCell(tblRow, sTxtBoxBatteryChangedMonitor, "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, sChkBoxUnableToLocateMonitor, "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, sChkBoxLocatedMonitor, "center", , , , , "40px", "siteOverview_cell")
                            AddCell(tblRow, sChkBoxBatteryReplacedMonitor, "center", , , , , "40px", "siteOverview_cell")
                        End If

                        tblMonitorInfo.Rows.Add(tblRow)
                    End With
                Next
            End If
        End Sub
	
        Public Function ExportI2AssetMetaData_XMLNodetoDataTable(ByVal xmlNd As XmlNode) As DataTable

            Dim dt As New DataTable
	    
            Dim addColumn() As String = {"TagId", "Id"}
	    
            dt = addColumntoDataTable(addColumn)

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count
		
                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
			
                        If (Nodename = "AssetList") Then
			
                            If str_xmlchildnode.HasChildNodes Then
			    
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                Dim dNewRow As DataRow = Nothing
                                dNewRow = dt.NewRow
				
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
				
                                dt.Rows.Add(dNewRow)
				
                            End If
			    
                        End If
			
                    Next
		    
                End If
		
            Catch ex As Exception
                WriteLog("ExportI2AssetMetaData_XMLNodetoDataTable " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        Public Sub LoadExportI2AssetMetaData(ByVal SiteId As String, ByVal RptType As String, ByVal sitename As String)

            Dim dt As New DataTable

            Try
                dt = ExportI2AssetMetaData(SiteId)

                If dt.Rows.Count > 0 Then
                    Preparei2_MetaDataCSV(dt, sitename)
                Else
                    Preparei2_MetaDataCSVNoRecord_Found(sitename)
                End If

            Catch ex As Exception
                WriteLog(" Exception LoadExportI2AssetMetaData  " & ex.Message.ToString())
            End Try

        End Sub
	
        Private Sub Preparei2_MetaDataCSV(ByVal dt As DataTable, ByVal sitename As String)
	
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder
	    
            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = "Tag-Data"

            Try

                sFileName = Sheetname & "-" & Format(Now, "mmddyyyy") & Format(Now, "hms")

                If dt.Rows.Count > 0 Then
		
                    Dim context As HttpContext
                    context = HttpContext.Current
                    excl.InitiateCSV(context, sFileName)
                    excl.AddCSVCell(context, "Site Name : " & sitename, True)
                    excl.AddCSVNewLine(context)
                    excl.AddCSVCell(context, "Tag Id", True)
                    excl.AddCSVCell(context, "Id", True)

                    excl.AddCSVNewLine(context)

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        excl.AddCSVCell(context, CheckIsDBNull(dt.Rows(nIdx).Item("TagId")), True)
                        excl.AddCSVCell(context, CheckIsDBNull(dt.Rows(nIdx).Item("Id")), True)
                        excl.AddCSVNewLine(context)
                    Next
		    
                    context.Response.End()
		    
                End If

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
	    
        End Sub
	
        Private Sub Preparei2_MetaDataCSVNoRecord_Found(ByVal sitename)
	
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder
	    
            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = "Tag-Data"

            Try

                sFileName = Sheetname & "-" & Format(Now, "mmddyyyy") & Format(Now, "hms")

                Dim context As HttpContext
                context = HttpContext.Current

                excl.InitiateCSV(context, sFileName)
                excl.AddCSVCell(context, "Site Name : " & sitename, True)
                excl.AddCSVNewLine(context)
                excl.AddCSVCell(context, "Tag Id", True)
                excl.AddCSVCell(context, "Id", True)

                excl.AddCSVNewLine(context)

                context.Response.End()

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
	    
        End Sub

        Private Function CheckIdExists(ByVal chkArray As String, ByVal chkId As Integer) As Boolean
	
            Dim chkArrayList() As String = Nothing
	    
            Dim chkArrayPos As Integer = 0

            If chkArray <> "" Then
	    
                chkArrayList = chkArray.Split(",")
		
                If chkArrayList.Length > 0 Then
                    For chkIdx As Integer = 0 To chkArrayList.Length - 1
                        If chkId = chkArrayList(chkIdx) Then
                            Return True
                        End If
                    Next
                End If
		
            End If

            Return False
	    
        End Function

        Public Function GetMapReportResponseXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim report_node, reports_node As XmlNode
            Dim report_node_list, reports_node_list As XmlNodeList
            Dim Nodename As String

            Dim ReportName As String
            Dim DeviceType As String
            Dim ShapeType As String
            Dim ShapeColor As String

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"ReportName", "DeviceType", "ShapeType", "ShapeColor", "DeviceId", "SvgId", "Volume", "ToSvgId"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        report_node = xmlNd.ChildNodes(i)
                        Nodename = report_node.Name
                        report_node_list = report_node.ChildNodes

                        If (Nodename = "Reports") Then
			
                            For k As Integer = 0 To report_node_list.Count - 1
			    
                                reports_node = report_node.ChildNodes(k)
                                Nodename = reports_node.Name
                                reports_node_list = reports_node.ChildNodes

                                If Nodename = "ReportName" Then
                                    ReportName = report_node_list(0).InnerText
                                ElseIf Nodename = "DeviceType" Then
                                    DeviceType = report_node_list(1).InnerText.Trim
                                ElseIf Nodename = "ShapeType" Then
                                    ShapeType = report_node_list(2).InnerText
                                ElseIf Nodename = "ShapeColor" Then
                                    ShapeColor = report_node_list(3).InnerText
                                ElseIf Nodename = "Report" Then
                                    dNewRow = dt.NewRow()
                                    dNewRow("ReportName") = ReportName
                                    dNewRow("DeviceType") = DeviceType
                                    dNewRow("ShapeType") = ShapeType
                                    dNewRow("ShapeColor") = ShapeColor

                                    For j As Integer = 0 To reports_node_list.Count - 1
                                        Dim Alert_nodename As String = reports_node_list(j).Name
                                        Dim Alert_nodeValue As String = reports_node_list(j).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
                                    Next
                                    dt.Rows.Add(dNewRow)
                                End If
                            Next
                        End If
                    Next
                End If
		
            Catch ex As Exception
                WriteLog("GetMapReportXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
	    
        End Function

        Public Function GetReportFloorViewXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim report_node As XmlNode
            Dim report_node_list As XmlNodeList

            Dim reports_node As XmlNode
            Dim reports_node_list As XmlNodeList

            Dim Nodename As String = ""
            Dim SubNodename As String = ""
            Dim reportName As String = ""

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"ReportName", "DeviceId", "SvgId", "DeviceType", "Bin", "FloorId", "FloorName", "Volume"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        report_node = xmlNd.ChildNodes(i)
                        Nodename = report_node.Name
                        report_node_list = report_node.ChildNodes

                        If Nodename = "Reports" Then
			
                            For k As Integer = 0 To report_node_list.Count - 1
                                reports_node = xmlNd.ChildNodes(i).ChildNodes(k)
                                SubNodename = reports_node.Name
                                reports_node_list = reports_node.ChildNodes

                                If SubNodename = "Report" Then
                                    dNewRow = dt.NewRow()
                                    dNewRow("ReportName") = reportName
                                    For j As Integer = 0 To reports_node_list.Count - 1
                                        Dim Alert_nodename As String = reports_node_list(j).Name
                                        Dim Alert_nodeValue As String = reports_node_list(j).InnerText
                                        dNewRow(Alert_nodename) = Alert_nodeValue
                                    Next
                                    dt.Rows.Add(dNewRow)
                                ElseIf SubNodename = "ReportName" Then
                                    reportName = reports_node.InnerText
                                End If
                            Next
			    
                        End If
			
                    Next
		    
                End If
		
            Catch ex As Exception
                WriteLog("GetReportFloorViewXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
	    
        End Function

        Function SearchXMLNodeToDataTable(ByVal xmlNd As XmlNode) As DataTable
	
            Dim dtTemp As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim report_node As XmlNode
            Dim report_node_list As XmlNodeList

            Dim reports_node As XmlNode
            Dim reports_node_list As XmlNodeList

            Dim Nodename As String = ""
            Dim SubNodename As String = ""
            Dim valueList As String = ""

            dtTemp.Columns.Add(New DataColumn("DeviceType", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("DisplayColumn", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("DataType", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("DBColumn", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("Values", Type.[GetType]("System.String")))

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        report_node = xmlNd.ChildNodes(i)
                        Nodename = report_node.Name
                        report_node_list = report_node.ChildNodes

                        If Nodename = "Tag" Or Nodename = "Monitor" Or Nodename = "Star" Then
			
                            For k As Integer = 0 To report_node_list.Count - 1
			    
                                reports_node = xmlNd.ChildNodes(i).ChildNodes(k)
                                SubNodename = reports_node.Name
                                reports_node_list = reports_node.ChildNodes

                                If SubNodename = "DeviceFilter" Then
				
                                    dNewRow = dtTemp.NewRow()
                                    dNewRow("DeviceType") = Nodename
				    
                                    For j As Integer = 0 To reports_node_list.Count - 1
				    
                                        If reports_node_list(j).Name = "Values" Then
                                            If valueList = "" Then
                                                valueList = reports_node_list(j).InnerText
                                            Else
                                                valueList &= "," & reports_node_list(j).InnerText
                                            End If
                                        Else
                                            Dim Alert_nodename As String = reports_node_list(j).Name
                                            Dim Alert_nodeValue As String = reports_node_list(j).InnerText
                                            dNewRow(Alert_nodename) = Alert_nodeValue
                                        End If
					
                                    Next
				    
                                    dNewRow("Values") = valueList
                                    dtTemp.Rows.Add(dNewRow)

                                    valueList = ""
				    
                                End If
                            Next
                        End If
                    Next
		    
                End If
		
            Catch ex As Exception
                WriteLog(" SearchXMLNodeToDataTable " & ex.Message.ToString)
            End Try

            Return dtTemp
	    
        End Function
	
        Function TruncateString(ByVal str As String) As String
	
            Dim sResult As String = ""
            Dim tempstr As String = ""
	    
            sResult = str
	    
            Try
	    
                Dim k As Integer = 0
		
                For i As Integer = 0 To str.Length - 1
                    tempstr = tempstr & str(i)
                    If str(i) <> " " Then
                        k = k + 1
                    Else
                        k = 0
                    End If
                    If k = 7 Then
                        k = 0
                        tempstr = tempstr & " "
                    End If
                Next
		
                If tempstr.Length > 40 Then
                    str = tempstr.Substring(0, 18) & "..." & tempstr.Substring(tempstr.Length - 18, (tempstr.Length - (tempstr.Length - 18)))
                Else
                    str = tempstr
                End If
		
                sResult = str
		
            Catch ex As Exception

            End Try
	    
            Return sResult.ToLower
	    
        End Function
	
        Public Function GetAssetTagMetaInfoForFloorXMLtoTable(ByVal xmlNd As XmlNode, Optional ByVal CurPage As Integer = 0) As DataTable
            
	    Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing
	    
            Dim TagInfoNodename As String = ""
            Dim sitename As String = ""
            Dim totalpage As String = ""
            Dim totalcount As String = ""

            Try
	    
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"SiteName", "TagId", "TagType", "FloorName", "UnitName", "RoomName", "BatteryCapacity", "CatastrophicCases", "FloorId", "SvgId", "MonitorId", "TotalPage", "TotalCount", "TagName"}
                dt = addColumntoDataTable(addColumn)
                
		Dim nMonitorId As String = ""
                Dim nSvgId As String = ""

                If cnt > 0 Then
		
                    For i As Integer = 0 To cnt - 1
		    
                        Dim str_xmlchildnode As XmlNode
			
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
			
                        If (Nodename = "TagInfo") Then
			
                            If str_xmlchildnode.HasChildNodes Then
			    
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                dNewRow = dt.NewRow
				
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("Sitename") = sitename
                                    dNewRow("TotalPage") = totalpage
                                    dNewRow("TotalCount") = totalcount
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
				
                                If dNewRow("TagId").ToString <> "" Then dt.Rows.Add(dNewRow)
                            End If
			    
                        Else
			
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "sitename") Then
                                sitename = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalpage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                totalcount = Nodevalue
                            End If
			    
                        End If

                    Next
                End If
            Catch ex As Exception
                WriteLog("GetAssetTagMetaInfoForFloorXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
	    
        End Function
	
        Public Function GetExportExcelAssetTagMetaInfoForFloorXMLtoTable(ByVal xmlNd As XmlNode, Optional ByVal CurPage As Integer = 0) As DataTable
            
	    Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing
            Dim TagInfoNodename As String = ""
	    
            Dim sitename As String = ""
            Dim totalpage As String = ""
            Dim totalcount As String = ""

            Try

                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"SiteName", "TagId", "TagName", "TagType", "MonitorId", "CurrentLocation", "LastLocation", "EnteredOn", "LeftOn", "TimeSpend", "TemperatureValue1", "TemperatureValue2", "LastTemperatureValue1", "LastTemperatureValue2", "Humidity", "LastHumidity", "EventType", "LastRoom"}
                dt = addColumntoDataTable(addColumn)
                Dim nMonitorId As String = ""
                Dim nSvgId As String = ""

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename = "TagInfo") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                dNewRow = dt.NewRow
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("Sitename") = sitename
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "sitename") Then
                                sitename = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetAssetTagMetaInfoForFloorXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function
        Public Function GetBatterySummaryXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode
            Dim Alerts_Details_Sub_Node As XmlNode

            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList
            Dim AlertList_Details_Sub_Node As XmlNodeList

            Dim site As String = ""
            Dim estimated As String = ""
            Dim quantity As String = ""
            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim Details_Sub_Nodename As String = ""
            Dim List_Sub_Nodename As String = ""
            Dim Alerts_nodename As String = ""


            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Sitename", "Estimated", "Quantity", "Est_Bat_LifeSpan", "Est_Bat_LifeSpan_Quantity"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "List") Then
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name
                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes
                                If Sub_Nodename = "Estimated" Then
                                    estimated = AlertList_Node(j).InnerText
                                ElseIf Sub_Nodename = "Quantity" Then
                                    quantity = AlertList_Node(j).InnerText
                                ElseIf Sub_Nodename = "Est_Bat_LifeSpan" Then
                                    If AlertList_Sub_Node.Count > 0 Then
                                        For k As Integer = 0 To AlertList_Sub_Node.Count - 1
                                            Alerts_Details_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j).ChildNodes(k)
                                            Details_Sub_Nodename = Alerts_Details_Sub_Node.Name
                                            AlertList_Details_Sub_Node = Alerts_Details_Sub_Node.ChildNodes
                                            If Details_Sub_Nodename = "LifeSpan" Then
                                                dNewRow = dt.NewRow
                                                dNewRow("Sitename") = site
                                                dNewRow("Estimated") = estimated
                                                dNewRow("Quantity") = quantity
                                                For l As Integer = 0 To AlertList_Details_Sub_Node.Count - 1
                                                    Dim Alert_nodename As String = AlertList_Details_Sub_Node(l).Name
                                                    Dim Alert_nodeValue As String = AlertList_Details_Sub_Node(l).InnerText
                                                    dNewRow(Alert_nodename) = Alert_nodeValue
                                                Next
                                                dt.Rows.Add(dNewRow)
                                            End If
                                        Next
                                    Else
                                        dNewRow = dt.NewRow
                                        dNewRow("Sitename") = site
                                        dNewRow("Estimated") = estimated
                                        dNewRow("Quantity") = quantity
                                        dt.Rows.Add(dNewRow)
                                    End If
                                End If
                            Next
                        ElseIf (Nodename = "Sitename") Then
                            site = AlertList_Node(0).InnerText
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetBatterySummaryXMLtoTable " & ex.Message.ToString)
            End Try

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    dt.DefaultView.Sort = "Estimated"
                    dt = dt.DefaultView.ToTable()
                End If
            End If


            Return dt
        End Function

        Public Function GetTagBatteryListXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing
            Dim TagInfoNodename As String = ""
            Dim sitename As String = ""
            Dim totalpage As String = ""
            Dim totalcount As String = ""
            Dim ds As New DataSet

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Sitename", "TotalPage", "TotalCount", "Location", "TagTypeName", "TagId", "Estimated", "UpdateRate", "ActivityLevel"}
                dt = addColumntoDataTable(addColumn)
                Dim nMonitorId As String = ""
                Dim nSvgId As String = ""

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename = "TagInfo") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                dNewRow = dt.NewRow
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("Sitename") = sitename
                                    dNewRow("TotalPage") = totalpage
                                    dNewRow("TotalCount") = totalcount
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "sitename") Then
                                sitename = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalpage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                totalcount = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetBatteryListXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function
        Public Function GetInfraBatteryListXMLtoTable(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing
            Dim TagInfoNodename As String = ""
            Dim sitename As String = ""
            Dim totalpage As String = ""
            Dim totalcount As String = ""
            Dim ds As New DataSet

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Sitename", "TotalPage", "TotalCount", "Location", "MonitorTypeName", "MonitorId", "Estimated", "UpdateRate", "PowerLevel", "NoiseLevel"}
                dt = addColumntoDataTable(addColumn)
                Dim nMonitorId As String = ""
                Dim nSvgId As String = ""

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename = "MonitorInfo") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                dNewRow = dt.NewRow
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("Sitename") = sitename
                                    dNewRow("TotalPage") = totalpage
                                    dNewRow("TotalCount") = totalcount
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "sitename") Then
                                sitename = Nodevalue
                            ElseIf (Nodename.ToLower = "totalpage") Then
                                totalpage = Nodevalue
                            ElseIf (Nodename.ToLower = "totalcount") Then
                                totalcount = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetBatteryListXMLtoTable " & ex.Message.ToString)
            End Try

            Return dt
        End Function
        Public Function GetTagLbiListXMLtoTableForBatteryTech(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing
            Dim TagInfoNodename As String = ""
            Dim ds As New DataSet
            Dim sitename As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"SiteId", "SiteName", "TagId", "TagType", "TagTypeName", "Location", "InLocationHrs", "CatastrophicCases", "BatteryReplaceMentOn", "Floor"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename = "TagInfo") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                dNewRow = dt.NewRow
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("Sitename") = sitename
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "sitename") Then
                                sitename = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetTagLbiListXMLtoTableForBatteryTech " & ex.Message.ToString)
            End Try

            Return (dt)
        End Function
        Public Function GetTagReportXMLtoTableForBatteryTech(ByVal xmlNd As XmlNode) As DataTable
            Dim dt As New DataTable
            Dim dNewRow As DataRow = Nothing
            Dim TagInfoNodename As String = ""
            Dim ds As New DataSet
            Dim sitename As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"SiteName", "TagId", "Location", "TagType", "BatteryReplaceMentOn", "BatteryReplacedBy", "Comments"}
                dt = addColumntoDataTable(addColumn)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Dim str_xmlchildnode As XmlNode
                        str_xmlchildnode = xmlNd.ChildNodes(i)
                        Dim Nodename As String = str_xmlchildnode.Name
                        If (Nodename = "Tag") Then
                            If str_xmlchildnode.HasChildNodes Then
                                Dim str_ChildNodeList As XmlNodeList
                                str_ChildNodeList = str_xmlchildnode.ChildNodes
                                dNewRow = dt.NewRow
                                For n As Integer = 0 To str_ChildNodeList.Count - 1
                                    dNewRow("Sitename") = sitename
                                    Dim subNodeName As String = str_ChildNodeList(n).Name
                                    Dim subNodevalue As String = str_ChildNodeList(n).InnerText
                                    dNewRow(subNodeName) = subNodevalue
                                Next
                                dt.Rows.Add(dNewRow)
                            End If
                        Else
                            Dim Nodevalue As String = str_xmlchildnode.InnerText
                            If (Nodename.ToLower = "sitename") Then
                                sitename = Nodevalue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                WriteLog("GetTagReportXMLtoTableForBatteryTech " & ex.Message.ToString)
            End Try

            Return dt
        End Function

        Public Function GetAssetDeviceDetailsForSearchXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            
			Dim elemList As XmlNodeList
            Dim elemList_TagInfo As XmlNodeList
            Dim elemList_TagWithNoLocation As XmlNodeList

            Dim ds As New DataSet
            Dim dtLocationInfo As New DataTable
            Dim dtTagInfo As New DataTable
            Dim dtTagWithNoLocation As New DataTable

            Dim dNewRow As DataRow = Nothing

            Dim elemIdx As Integer = 0
            Dim LocationInfo_elemIdx As Integer = 0
            Dim TagInfo_elemIdx As Integer = 0
            Dim TagWithNoLocation_elemIdx As Integer = 0

            dtLocationInfo.Columns.Add(New DataColumn("MonitorId", System.Type.[GetType]("System.Int32")))
            dtLocationInfo.Columns.Add(New DataColumn("RoomName", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("CampusId", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("CampusName", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("BuildingId", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("BuildingName", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("FloorId", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("FloorName", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("UnitId", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("UnitName", System.Type.[GetType]("System.String")))
            dtLocationInfo.Columns.Add(New DataColumn("AssetCount", System.Type.[GetType]("System.String")))


            dtTagInfo.Columns.Add(New DataColumn("TagId", System.Type.[GetType]("System.Int32")))
            dtTagInfo.Columns.Add(New DataColumn("TagId_MonitorId", System.Type.[GetType]("System.Int32")))
            dtTagInfo.Columns.Add(New DataColumn("TagType", System.Type.[GetType]("System.String")))
            dtTagInfo.Columns.Add(New DataColumn("TagName", System.Type.[GetType]("System.String")))
            dtTagInfo.Columns.Add(New DataColumn("CatastrophicCases", System.Type.[GetType]("System.String")))
            dtTagInfo.Columns.Add(New DataColumn("LastUpdateBin", System.Type.[GetType]("System.String")))
            dtTagInfo.Columns.Add(New DataColumn("TamperBin", System.Type.[GetType]("System.String")))
            dtTagInfo.Columns.Add(New DataColumn("LastUpdate", System.Type.[GetType]("System.String")))
            dtTagInfo.Columns.Add(New DataColumn("Arrived", System.Type.[GetType]("System.String")))


            dtTagWithNoLocation.Columns.Add(New DataColumn("TagId", System.Type.[GetType]("System.Int32")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("TagType", System.Type.[GetType]("System.String")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("TagName", System.Type.[GetType]("System.String")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("CatastrophicCases", System.Type.[GetType]("System.String")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("LastUpdateBin", System.Type.[GetType]("System.String")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("TamperBin", System.Type.[GetType]("System.String")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("LastUpdate", System.Type.[GetType]("System.String")))
            dtTagWithNoLocation.Columns.Add(New DataColumn("Arrived", System.Type.[GetType]("System.String")))


            Try
                'RoomInfo
                elemList = xmlNd.SelectNodes("RoomInfo")

                If elemList.Count > 0 Then
                    For elemIdx = 0 To elemList.Count - 1
                        dNewRow = dtLocationInfo.NewRow
                        dNewRow("MonitorId") = elemList.Item(elemIdx).Item("MonitorId").InnerText
                        dNewRow("RoomName") = elemList.Item(elemIdx).Item("RoomName").InnerText
                        dNewRow("CampusId") = elemList.Item(elemIdx).Item("CampusId").InnerText
                        dNewRow("CampusName") = elemList.Item(elemIdx).Item("CampusName").InnerText
                        dNewRow("BuildingId") = elemList.Item(elemIdx).Item("BuildingId").InnerText
                        dNewRow("BuildingName") = elemList.Item(elemIdx).Item("BuildingName").InnerText
                        dNewRow("FloorId") = elemList.Item(elemIdx).Item("FloorId").InnerText
                        dNewRow("FloorName") = elemList.Item(elemIdx).Item("FloorName").InnerText
                        dNewRow("UnitId") = elemList.Item(elemIdx).Item("UnitId").InnerText
                        dNewRow("UnitName") = elemList.Item(elemIdx).Item("UnitName").InnerText
                        dNewRow("AssetCount") = elemList.Item(elemIdx).Item("AssetCount").InnerText
                        dtLocationInfo.Rows.Add(dNewRow)

                        'Tag Info  
                        If Not elemList(elemIdx).Item("TagInfo") Is Nothing Then
                            elemList_TagInfo = elemList.Item(elemIdx).SelectNodes("TagInfo")
                            If elemList_TagInfo.Count > 0 Then
                                For TagInfo_elemIdx = 0 To elemList_TagInfo.Count - 1
                                    dNewRow = dtTagInfo.NewRow
                                    dNewRow("TagId") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("TagId").InnerText
                                    dNewRow("TagId_MonitorId") = elemList.Item(elemIdx).Item("FloorId").InnerText & elemList.Item(elemIdx).Item("MonitorId").InnerText
                                    dNewRow("TagType") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("TagType").InnerText
                                    dNewRow("TagName") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("TagName").InnerText
                                    dNewRow("CatastrophicCases") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("CatastrophicCases").InnerText
                                    dNewRow("LastUpdateBin") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("LastUpdateBin").InnerText
                                    dNewRow("TamperBin") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("TamperBin").InnerText
                                    dNewRow("LastUpdate") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("LastUpdate").InnerText
                                    dNewRow("Arrived") = elemList_TagInfo.Item(TagInfo_elemIdx).Item("Arrived").InnerText
                                    dtTagInfo.Rows.Add(dNewRow)
                                Next
                            End If
                        End If

                    Next
                End If

                'TagWithNoLocationInfo
                elemList = xmlNd.SelectNodes("TagsWithNoLocationInfo")

                If elemList.Count > 0 Then
                    For elemIdx = 0 To elemList.Count - 1
                        'TagWithNoLocationInfo
                        If Not elemList(elemIdx).Item("TagInfo") Is Nothing Then
                            elemList_TagWithNoLocation = elemList.Item(elemIdx).SelectNodes("TagInfo")
                            If elemList_TagWithNoLocation.Count > 0 Then
                                For TagWithNoLocation_elemIdx = 0 To elemList_TagWithNoLocation.Count - 1
                                    dNewRow = dtTagWithNoLocation.NewRow
                                    dNewRow("TagId") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("TagId").InnerText
                                    dNewRow("TagType") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("TagType").InnerText
                                    dNewRow("TagName") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("TagName").InnerText
                                    dNewRow("CatastrophicCases") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("CatastrophicCases").InnerText
                                    dNewRow("LastUpdateBin") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("LastUpdateBin").InnerText
                                    dNewRow("TamperBin") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("TamperBin").InnerText
                                    dNewRow("LastUpdate") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("LastUpdate").InnerText
                                    dNewRow("Arrived") = elemList_TagWithNoLocation.Item(TagWithNoLocation_elemIdx).Item("Arrived").InnerText
                                    dtTagWithNoLocation.Rows.Add(dNewRow)
                                Next
                            End If
                        End If
                    Next
                End If

                ds.Tables.Add(dtLocationInfo)
                ds.Tables(0).TableName = "dtLocationInfo"

                ds.Tables.Add(dtTagInfo)
                ds.Tables(1).TableName = "dtTagInfo"

                ds.Tables.Add(dtTagWithNoLocation)
                ds.Tables(2).TableName = "dtTagWithNoLocation"

            Catch ex As Exception
                WriteLog("GetAssetDeviceDetailsForSearchXMLtoTable " & ex.Message.ToString)
            End Try

            Return ds

        End Function

        Public Function DownloadPdfForSUPTTag(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String) As Boolean
            
			Dim dtTag As New DataTable

            Dim theDoc As Doc = New Doc()
            theDoc.HtmlOptions.Timeout = 10000000
            theDoc.HtmlOptions.Engine = EngineType.Gecko

            Dim sHTML As New StringBuilder
            Dim HeaderHTML As New StringBuilder

            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim bNoRecordFound As Boolean = False
            Dim IsExportforAllDevices As Boolean = False

            Dim HTMLsaveFloder As String = "HTML"
            Dim HTML_filepath As String = ""

            Try

                If Not Directory.Exists(GetAppPath() & "\" & HTMLsaveFloder) Then
                    Directory.CreateDirectory(GetAppPath() & "\" & HTMLsaveFloder)
                End If

                Sheetname = "SUPTTagDied"

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                HTML_filepath = GetAppPath() & "\" & HTMLsaveFloder & "\" & sFileName & "_" & g_UserId & ".html"

                theDoc.Rect.Inset(20, 25)
                theDoc.Page = theDoc.AddPage()

                dtTag = GetSUPTTagInfoForSite(Val(SiteId))

                'Header Content
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td><img src='https://gms.centrak.com/gmsnew/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td align='left' style='border-bottom: solid 5px #245E90;'>")
                HeaderHTML.Append("&nbsp;</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td valign='top' align='center'>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; color: #373737; border-bottom: solid 1px #D3D3D3;height:40px;' valign='middle' class='sText'>")
                HeaderHTML.Append("<label class='SHeader1'>")
                HeaderHTML.Append(SiteName)
                HeaderHTML.Append("</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%' height='40px'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; width: 35%; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Report Date&nbsp;:&nbsp;")
                HeaderHTML.Append("<label>" & CDate(Now()).Date & "</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Report&nbsp;Type&nbsp;:&nbsp;")

                HeaderHTML.Append("<label>SUPT Tag Died Report</label>")

                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")

                'BOBY Content
                sHTML.Append("<html>")
                sHTML.Append("<head>")
                sHTML.Append("<style>")
                sHTML.Append(".siteOverview_TopLeft_Box" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "background-image: url('" & GetServerPath() & "/Images/tblHeaderbg.png');" & _
                                  "background-repeat:no-repeat;background-size:130px 60px;" & _
                                 "color:White;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                              "}" & _
                              ".SHeader1" & _
                              "{" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 19px;" & _
                                  "font-weight:bold;" & _
                                  "height:30px;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1a1a1a;" & _
                              "}" & _
                              ".siteOverview_Header" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "-webkit-border-top-left-radius: 5px;" & _
                                  "-moz-border-top-left-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "border-top:1px solid #1F5E93;" & _
                                  "border-left:1px solid #1F5E93;" & _
                                  "border-bottom:1px solid #1F5E93;" & _
                                  "border-right:1px solid #1F5E93;" & _
                                  "color:#FFFFFF;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  " background-color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_HeaderLeft" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "-webkit-border-top-left-radius: 5px;" & _
                                  "-moz-border-top-left-radius: 5px;" & _
                                  "border-top-left-radius: 5px;" & _
                                  "border-top:1px solid #1F5E93;" & _
                                  "border-left:1px solid #1F5E93;" & _
                                  "border-bottom:1px solid #1F5E93;" & _
                                  "border-right:1px solid #1F5E93;" & _
                                  "color:#FFFFFF;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "padding-left: 5px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:left;" & _
                                  "vertical-align: middle;" & _
                                  " background-color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_Box" & _
                              "{" & _
                                  "border-top:1px solid #DADADA;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #454545;" & _
                              "}" & _
                              ".siteOverview_Topright_Box" & _
                              "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "border-top:1px solid #DADADA;" & _
                                  "color:#454545;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                              "}" & _
                              ".DeviceList_leftBox" & _
                              "{" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-left:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:left;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1F5E93;" & _
                              "}" & _
                              ".siteOverview_cell" & _
                              "{" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1F5E93;" & _
                              "}" & _
                              ".SHeader1" & _
                              "{" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 19px;" & _
                                  "font-weight:bold;" & _
                                  "height:30px;" & _
                                  "vertical-align: middle;" & _
                                  "color: #1a1a1a;" & _
                              "}")
                sHTML.Append("</style>")
                sHTML.Append("</head>")
                sHTML.Append("<body>")
                sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(HeaderHTML.ToString())
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(AddDatasToStringBuilderForSUPTTag(dtTag, enumDeviceType.Tag, RptType, HeaderHTML.ToString()))
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("</table>")
                sHTML.Append("</body>")
                sHTML.Append("</html>")

                File.AppendAllText(HTML_filepath, sHTML.ToString)

                Dim theID As Integer

                theDoc.Rect.String = "50 60 573 750"
                theID = theDoc.AddImageUrl("file:///" & HTML_filepath)

                While True
                    'theDoc.FrameRect() ' add a black border
                    If Not theDoc.Chainable(theID) Then
                        Exit While
                    End If
                    theDoc.Page = theDoc.AddPage()
                    theID = theDoc.AddImageToChain(theID)
                End While

                theDoc.Rect.Position(467, 594)
                theDoc.Rect.Width = 100
                theDoc.Rect.Height = 65

                Dim i As Integer
                For i = 1 To theDoc.PageCount
                    theDoc.PageNumber = i
                    theDoc.AddHtml("<p align='right'><font size='3px' color= '#8A8A8A' font-family= 'Arial'>Page&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & i & "</font><font size='3px' color= '#8A8A8A' font-family= 'Arial'>&nbsp;of&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & theDoc.PageCount & "</font></p>")
                    theDoc.Flatten()
                Next

                theDoc.SetInfo(theDoc.Root, "/OpenAction", "[ 1 0 R /XYZ null null 2 ]")

                Dim pdfbytestream As Byte() = theDoc.GetData()
                theDoc.Clear()
                theDoc.Dispose() 'free up unused object
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.Buffer = True
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & sFileName & ".pdf")
                HttpContext.Current.Response.AddHeader("content-length", pdfbytestream.Length.ToString())
                HttpContext.Current.Response.BinaryWrite(pdfbytestream)

                If File.Exists(HTML_filepath) Then
                    File.Delete(HTML_filepath)
                End If

                HttpContext.Current.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                '  HttpContext.Current.Response.Write("Error in generating the Pdf, please try again.")
                WriteLog(" DownloadPdf " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function
        Public Function AddDatasToStringBuilderForSUPTTag(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String, Optional ByVal headerHtml As String = "") As String
            Dim sHTML As New StringBuilder
            Dim sHTMLCell As New StringBuilder
            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim initialMonitorIdx As Integer = 0
            Dim initialStarIdx As Integer = 0
            Dim sClass As String = ""
            Dim previousTagType As String = ""
            Dim previousMonitorType As String = ""
            Dim nRows As Integer = 0
            Dim nColumns As Integer = 0
            Dim PreviousDate As String = ""

            Dim sCol As String = "align='center' style='height: 27px; width: 20%; padding-right:10px; padding-left:10px;'"
            Dim sCol1 As String = "align='center' style='height: 27px; width: 20%; background-color:#F9F8F8;'"

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
            sClass = "DeviceList_leftBox"
            nRows = 0

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    For nRowIdx = 0 To dt.Rows.Count - 1

                        If DeviceType = enumDeviceType.Tag Then
                            If nRows >= 25 Then
                                If nRows <> 0 Then
                                    nRows = nRows - 25
                                    sHTML.Append("<tr>")
                                    sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                                    sHTML.Append("</td>")
                                    sHTML.Append("</tr>")

                                    sHTML.Append("<tr>")
                                    sHTML.Append("<td colspan='15'>" & headerHtml)
                                    sHTML.Append("</td>")
                                    sHTML.Append("</tr>")
                                End If

                                'Header
                                sHTML.Append("<tr style='height:20px;'>")
                                sHTML.Append("<td>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                                sHTML.Append("<tr>")
                                sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("Date") & "</td>")
                                sHTML.Append("</tr>")
                                nRows = nRows + 1
                            End If

                            'Datas
                            nColumns = 0
                            nRows = nRows + 1
                            sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                            sHTMLCell = New StringBuilder()
                            sClass = "DeviceList_leftBox"

                            If PreviousDate <> dt.Rows(nRowIdx).Item("Date") Or nRowIdx = 0 Then
                                'Header
                                sHTML.Append("<tr style='height:20px;'>")
                                sHTML.Append("<td>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                                sHTML.Append("<tr>")
                                sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>" & dt.Rows(nRowIdx).Item("Date") & "</td>")
                                sHTML.Append("</tr>")
                            End If

                            sHTMLCell.Append("<td align='left' class='" & sClass & "' " & sCol & ">" & dt.Rows(nRowIdx).Item("TagId") & "</td>")
                            sClass = "siteOverview_cell"

                            If nRowIdx >= dt.Rows.Count - 1 Then
                                sHTML.Append("<tr style='height:26px;'>" & sHTMLCell.ToString() & "</tr>")
                                sHTMLCell = New StringBuilder()
                                sClass = "DeviceList_leftBox"
                            End If

                        End If
                        'nRows = nRows + 1
                        nColumns = nColumns + 1
                        PreviousDate = dt.Rows(nRowIdx).Item("Date")
                    Next
                Else
                    sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                    sHTMLCell = New StringBuilder()
                    sHTML.Append("<tr style='height:27px;'>")
                    sHTML.Append("<td>")
                    sHTML.Append("</td>")
                    sHTML.Append("</tr>")
                    sHTML.Append("<tr>")
                    sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>SUPT TAG</td>")
                    sHTML.Append("</tr>")
                    sHTMLCell.Append("<td colspan='5' class='" & sClass & "' " & sCol & ">No tag died</td>")
                    sHTML.Append("<tr style='height:26px;'>" & sHTMLCell.ToString() & "</tr>")
                End If
            Else
                sHTML.Append("<tr style='height:27px;'>" & sHTMLCell.ToString() & "</tr>")
                sHTMLCell = New StringBuilder()
                sHTML.Append("<tr style='height:27px;'>")
                sHTML.Append("<td>")
                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("<tr>")
                sHTML.Append("<td class='siteOverview_HeaderLeft' style='height: 32px; width: 100%;' colspan='5'>SUPT TAG</td>")
                sHTML.Append("</tr>")
                sHTMLCell.Append("<td colspan='5' class='" & sClass & "' " & sCol & ">No tag died</td>")
                sHTML.Append("<tr style='height:26px;'>" & sHTMLCell.ToString() & "</tr>")
            End If

            sHTML.Append("</table>")

            Return sHTML.ToString()
        End Function

        Public Function DownloadExcelForSUPTTag(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String) As Boolean
            Dim oExcel As New Excel
            Dim dtTag As New DataTable

            Dim sHTML As New StringBuilder
            Dim sSubHTML As StringBuilder
            Dim HeaderHTML As New StringBuilder
            Dim sVal As String() = Nothing
            Dim sFileName As String = ""
            Dim Sheetname As String = ""

            Dim bNoRecordFound As Boolean = False
            Dim IsExportforAllDevices As Boolean = False

            Try

                Sheetname = "SUPTTagDied"

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                dtTag = GetSUPTTagInfoForSite(Val(SiteId))

                'Header Content
                oExcel.CreateHeader("Report", sHTML)
                oExcel.CreateWorkSheetDatas(sHTML)
                oExcel.InsertDataHeader("<img src='https://gms.centrak.com/gmsnew/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />", sHTML, False, 5, 2)
                oExcel.InsertDataHeader("&nbsp;", sHTML)
                oExcel.InsertDataHeader(SiteName, sHTML, True, 5)
                oExcel.InsertDataHeader("&nbsp;", sHTML)

                sSubHTML = New StringBuilder()
                oExcel.CreateWorkSheetDatas(sSubHTML)
                ReDim Preserve sVal(2)
                sVal(0) = "Report Date&nbsp;:&nbsp;<label>" & CDate(Now()).Date & "</label>"

                sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>SUPT Tag Died Report</label>"

                oExcel.InsertDataArray(sVal, sSubHTML, True, 1)
                oExcel.CloseWorkSheetDatas(sSubHTML)
                oExcel.InsertData(sSubHTML.ToString(), sHTML)

                'Data Content
                oExcel.CreateWorkSheetDatas(sHTML)
                'TAG
                oExcel.InsertData(AddDatasToStringBuilderForExcelSUPTTag(dtTag, enumDeviceType.Tag, RptType), sHTML)

                oExcel.CloseWorkSheetDatas(sHTML)

                'Download Excel
                oExcel.DownloadExcel(sFileName, sHTML)
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadExcelForAllDevices " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Function GetDeviceIdFormat(ByVal sDevice_Ids As String) As String

            Dim sDevice_txt, sDevice_trim, sResults As String

            sDevice_txt = sDevice_Ids.Replace(vbCrLf, ",").Replace(" ", ",")
            sDevice_trim = sDevice_txt.TrimEnd(",", " ")

            sResults = Regex.Replace(sDevice_trim, ",,+", ",")

            Return sResults
        End Function

        Public Function AddDatasToStringBuilderForExcelSUPTTag(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String) As String
            Dim oExcel As New Excel

            Dim sHTML As New StringBuilder
            Dim sVal() As String = Nothing
            Dim sCol As String = "align='center' style='height: 27px; width: 20%;'"
            Dim sCol1 As String = "align='center' style='height: 27px; width: 20%; background-color:#F9F8F8;'"

            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim nInnerIdx As Integer = 0
            Dim nColumns As Integer = 0

            Dim sTagType As String = ""
            Dim OldTagType As String = ""

            Dim sMonitorType As String = ""
            Dim OldMonitorType As String = ""
            Dim OldDate As String = ""

            oExcel.CreateWorkSheetDatas(sHTML)

            ReDim Preserve sVal(4)

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    For nRowIdx = 0 To dt.Rows.Count - 1
                        If DeviceType = enumDeviceType.Tag Then
                            If nRowIdx = 0 Or dt.Rows(nRowIdx).Item("Date") <> OldDate Then
                                oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("Date"), sHTML, False, 5)
                            End If

                            sVal(nInnerIdx) = dt.Rows(nRowIdx).Item("TagId")
                            nInnerIdx += 1
                            nColumns = nColumns + 1

                            nColumns = 0
                            oExcel.InsertDataArray(sVal, sHTML)
                            sVal = Nothing
                            nInnerIdx = 0
                            ReDim Preserve sVal(4)

                            If nRowIdx >= dt.Rows.Count - 1 Then
                                oExcel.InsertDataArray(sVal, sHTML)
                                sVal = Nothing
                            End If

                        End If
                        OldDate = dt.Rows(nRowIdx).Item("Date")
                    Next
                Else
                    oExcel.InsertDataHeader("SUPT TAG", sHTML, False, 5)
                    sVal(0) = "No tag died"
                    oExcel.InsertDataArray(sVal, sHTML)
                End If
            Else
                oExcel.InsertDataHeader("SUPT TAG", sHTML, False, 5)
                sVal(0) = "No tag died"
                oExcel.InsertDataArray(sVal, sHTML)
            End If

            oExcel.CloseWorkSheetDatas(sHTML)
            Return sHTML.ToString()
        End Function

        Public Function GetDeviceIds(ByVal DeviceId As String) As String

            If DeviceId.Length > 0 Then
                DeviceId = DeviceId.Trim
                Dim tempStr As String() = DeviceId.Split(",")
                DeviceId = ""

                For i As Integer = 0 To tempStr.Length - 1
                    Dim chk As String = tempStr(i).Trim.Replace(" ", "")
                    If chk <> "" Then
                        If DeviceId = "" Then
                            DeviceId = chk
                        Else
                            DeviceId = DeviceId & "," & chk
                        End If

                    End If
                Next

                If DeviceId.Substring(DeviceId.Length - 1, 1) = "," Then
                    DeviceId = DeviceId.Substring(0, DeviceId.Length - 1)
                End If

            End If

            Return DeviceId
        End Function

        Public Function isvalidPassword(ByVal password As String) As Boolean
            Return Regex.IsMatch(password, "^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])(?=.*[0-9])|(?=.*[A-Z])(?=.*[a-z])(?=.*[#$%=@!{},`~&*()'?.:;_|^-])).{8,32}$")
            'Password must contain: Minimum 8 characters and  must contain characters from 3 of the following 4 categories - UpperCase Alphabet,LowerCase Alphabet,Number and Special Character
        End Function

        Public Sub WriteADLog(ByVal strLog As String, Optional ByVal bNeedSession As Boolean = True)

            Dim FS As FileStream
            Dim SW As StreamWriter
            Dim strLogFileName As String

            strLogFileName = GetAppPath() & "\ADLog.txt"

            Try
                FS = New FileStream(strLogFileName, FileMode.Append)
                SW = New StreamWriter(FS)

                If (String.Empty.Equals(strLog)) Then
                    SW.WriteLine("")
                Else
                    SW.Write(Format(Now(), "yyyyMMdd hh:mm:ss "))
                    SW.WriteLine(strLog)
                End If
                SW.Close()
                FS.Close()
                SW = Nothing
                FS = Nothing
            Catch ex As Exception

            End Try

        End Sub

        Public Function GetUploadResultXMLtoTable(ByVal xmlNd As XmlNode) As DataSet
            Dim ds As New DataSet
            Dim dtResult As New DataTable
            Dim dtNoResult As New DataTable
            Dim dtErrors As New DataTable

            Dim dNewRow As DataRow = Nothing

            Dim Alerts_Node As XmlNode
            Dim Alerts_Sub_Node As XmlNode

            Dim AlertList_Node As XmlNodeList
            Dim AlertList_Sub_Node As XmlNodeList

            Dim Nodename As String = ""
            Dim Sub_Nodename As String = ""
            Dim result As String = ""
            Dim sError As String = ""
            Dim csvTotalRec As String = ""
            Dim csvTotalRecInserted As String = ""

            Try
                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                Dim addColumn() As String = {"Result", "error", "csvTotalRec", "csvTotalRecInserted"}
                dtResult = addColumntoDataTable(addColumn)

                Dim addColumnNoResult() As String = {"num", "Reason"}
                dtNoResult = addColumntoDataTable(addColumnNoResult)

                If cnt > 0 Then
                    For i As Integer = 0 To cnt - 1
                        Alerts_Node = xmlNd.ChildNodes(i)
                        Nodename = Alerts_Node.Name
                        AlertList_Node = Alerts_Node.ChildNodes

                        If (Nodename = "Result") Then
                            result = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "error") Then
                            If Not AlertList_Node(0) Is Nothing Then
                                sError = AlertList_Node(0).InnerText.ToString()
                            Else
                                sError = ""
                            End If
                        ElseIf (Nodename = "csvTotalRec") Or (Nodename = "TotalRec") Then
                            csvTotalRec = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "csvTotalRecInserted") Or (Nodename = "TotalRecInserted") Then
                            csvTotalRecInserted = AlertList_Node(0).InnerText
                        ElseIf (Nodename = "RecordsNotInstered") Or (Nodename = "RecordsNotInstered") Then
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name
                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If Sub_Nodename = "Record" Then
                                    dNewRow = dtNoResult.NewRow()
                                    For l As Integer = 0 To AlertList_Sub_Node.Count - 1
                                        Dim Alert_nodename1 As String = AlertList_Sub_Node(l).Name
                                        Dim Alert_nodeValue1 As String = AlertList_Sub_Node(l).InnerText
                                        dNewRow(Alert_nodename1) = Alert_nodeValue1
                                    Next
                                    dtNoResult.Rows.Add(dNewRow)
                                End If
                            Next
                        ElseIf (Nodename = "errors") Then
                            For j As Integer = 0 To AlertList_Node.Count - 1
                                Alerts_Sub_Node = xmlNd.ChildNodes(i).ChildNodes(j)
                                Sub_Nodename = Alerts_Sub_Node.Name
                                AlertList_Sub_Node = Alerts_Sub_Node.ChildNodes

                                If Sub_Nodename = "error" Then
                                    dNewRow = dtErrors.NewRow()
                                    For l As Integer = 0 To AlertList_Sub_Node.Count - 1
                                        Dim Alert_nodename1 As String = AlertList_Sub_Node(l).Name
                                        Dim Alert_nodeValue1 As String = AlertList_Sub_Node(l).InnerText
                                        dNewRow(Alert_nodename1) = Alert_nodeValue1
                                    Next
                                    dtErrors.Rows.Add(dNewRow)
                                End If
                            Next

                        End If
                    Next

                    dNewRow = dtResult.NewRow()
                    dNewRow("Result") = result
                    dNewRow("error") = sError
                    dNewRow("csvTotalRec") = csvTotalRec
                    dNewRow("csvTotalRecInserted") = csvTotalRecInserted
                    dtResult.Rows.Add(dNewRow)
                End If
            Catch ex As Exception
                WriteLog("GetResultXMLtoTable " & ex.Message.ToString)
            End Try

            ds.Tables.Add(dtResult)
            ds.Tables(0).TableName = "Result"

            ds.Tables.Add(dtNoResult)
            ds.Tables(1).TableName = "NoResult"

            Return ds
        End Function

        Public Function GetDeviceSummaryReport(ByVal ds As DataSet, ByVal sGroupCond As String) As DataSet

            Dim dtList As New DataTable
            Dim dtListClone As New DataTable

            Dim dSet As New DataSet
            Dim dSetPageCount As New DataSet
            Dim dSetCollisionCount As New DataSet
            Dim dv As New DataView

            Try

                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then

                        If (ds.Tables.Contains("List")) Then
                            dtList = ds.Tables("List")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("Monitor")) Then
                            dtList = ds.Tables("Monitor")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("MonitorCollision")) Then
                            dtList = ds.Tables("MonitorCollision")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("TTSyncERRORReport")) Then
                            dtList = ds.Tables("TTSyncERRORReport")

                            dtListClone = dtList.Clone()
                            dtListClone.Columns(0).DataType = GetType(Integer)

                            For Each row In dtList.Rows
                                dtListClone.ImportRow(row)
                            Next

                            dtListClone.DefaultView.Sort = "StarId ASC"
                            dtListClone = dtListClone.DefaultView.ToTable()

                            dSet.Merge(dtListClone)
                        End If

                        If (ds.Tables.Contains("StarSummary")) Then
                            dtList = ds.Tables("StarSummary")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("ConnectionReport")) Then
                            dtList = ds.Tables("ConnectionReport")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("MonitorPagingVersionSummary")) Then
                            dtList = ds.Tables("MonitorPagingVersionSummary")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("MonitorCollisionVersionSummary")) Then
                            dtList = ds.Tables("MonitorCollisionVersionSummary")
                            dSet.Merge(dtList)
                        End If

                        If (ds.Tables.Contains("PageLocationCount")) Then
                            dtList = ds.Tables("PageLocationCount")
                            dSet.Merge(dtList)
                        End If

                    End If
                End If

            Catch ex As Exception
                WriteLog("GetBatteryListXMLtoTable " & ex.Message.ToString)
            End Try

            Return dSet
        End Function

        Public Function RetrivePageCount(ByVal dtList As DataTable) As DataSet

            Dim dSet As New DataSet

            Try

                dSet.Tables.Add(dtList)
                dSet.Tables(0).TableName = "DateMonitor"

            Catch ex As Exception
                WriteLog("RetrivePageCount " & ex.Message.ToString)
            End Try

            Return dSet
        End Function

        Public Function RetriveCollisionCount(ByVal dtList As DataTable) As DataSet

            Dim dSet As New DataSet

            Try
                dSet.Tables.Add(dtList)
                dSet.Tables(0).TableName = "DateMonitorCollision"

            Catch ex As Exception
                WriteLog("RetriveCollisionCount " & ex.Message.ToString)
            End Try

            Return dSet
        End Function

#Region "Vulnerabilities"
        Public blackList As String() = {"#", "^", "*", "<", ">", """", "iframe", _
                                       "--", ";--", "/*", "*/", "@@", _
                                        "<script>", "<alert>", "mouseover", _
                                        "power(", "LEN(", "DB_NAME", "ASCII", "SUBSTRING(", " power(", " LEN(", " DB_NAME", " ASCII", " SUBSTRING(", _
                                        " AUX ", " CLOCK$", " COM1", " COM2", " COM3", " COM4", " COM5", " COM6", " COM7", " COM8", " CON ", " CONFIG$", _
                                        " LPT1", " LPT2", " LPT3", " LPT4", " LPT5", " LPT6", " LPT7", " LPT8", " NUL ", " PRN ", _
                                        " alter ", " begin ", " break ", " checkpoint ", " commit ", " create ", " cursor ", " dbcc ", " deny ", " drop ", " escape ", " exec ", _
                                        " execute ", " insert ", " go ", " grant ", " opendatasource ", " openquery ", " openrowset ", " shutdown ", " sp_", " tran ", " transaction ", _
                                        " update ", " while ", "xp_", ";"
                                       }

        Public fromblackList As String() = {"^", ";", "<", ">", """", "iframe", _
                                            ";--", "/*", "*/", "@@", "<script>", "<alert>", "mouseover", _
                                            "power(", "LEN(", "DB_NAME", "ASCII", "SUBSTRING(", " power(", " LEN(", " DB_NAME", " ASCII", " SUBSTRING(", _
                                            " AUX ", " CLOCK$", " COM1", " COM2", " COM3", " COM4", " COM5", " COM6", " COM7", " COM8", " CON ", " CONFIG$", _
                                            " LPT1", " LPT2", " LPT3", " LPT4", " LPT5", " LPT6", " LPT7", " LPT8", " NUL ", " PRN ", _
                                            " alter ", " begin ", " break ", " checkpoint ", " commit ", " create ", " cursor ", " dbcc ", " deny ", " drop ", " escape ", " exec ", _
                                            " execute ", " insert ", " go ", " grant ", " opendatasource ", " openquery ", " openrowset ", " shutdown ", " sp_", " tran ", " transaction ", _
                                            " update ", " while ", "xp_", "--"
                                           }

        'You can change the error handling, and error redirect location to whatever makes sense for your site.
        Public Sub CheckInput(ByVal parameter As String, Optional ByVal isFormData As Boolean = False)
            Dim comparer As CompareInfo = CultureInfo.InvariantCulture.CompareInfo

            If isFormData = True Then

                For i As Integer = 0 To fromblackList.Length - 1
                    If (comparer.IndexOf(parameter, fromblackList(i), CompareOptions.IgnoreCase) >= 0) Then
                        'Handle the discovery of suspicious Sql characters here 
                        If Not HttpContext.Current.Request.Url.ToString().Contains("cmd=UpdateProfiles") And Not HttpContext.Current.Request.Url.ToString().Contains("cmd=ConfigADDirectory") Then
                            HttpContext.Current.Response.Redirect("UserErrorPage.aspx")
                        End If

                    End If
                Next
            Else
                For i As Integer = 0 To blackList.Length - 1
                    If (comparer.IndexOf(parameter, blackList(i), CompareOptions.IgnoreCase) >= 0) Then
                        'Handle the discovery of suspicious Sql characters here 
                        If Not HttpContext.Current.Request.Url.ToString().Contains("cmd=UpdateProfiles") And Not HttpContext.Current.Request.Url.ToString().Contains("cmd=ConfigADDirectory") Then
                            HttpContext.Current.Response.Redirect("UserErrorPage.aspx")
                        End If
                    End If
                Next
            End If
        End Sub
	
		Public Function DownloadOnDemandReport(ByVal SiteId As String, ByVal TypeIds As String, ByVal strDate As String) As Boolean

            Dim oExcel As New Excel
            Dim dtTag As New DataTable

            Dim sHTML As New StringBuilder
            Dim sSubHTML As StringBuilder
            Dim HeaderHTML As New StringBuilder
            Dim sVal As String() = Nothing
            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim SiteName As String = ""
            Dim arrSiteId() As String

            Try

                Sheetname = "Tags-not-seen"

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & "-" & Format(Now, "HHmmss")

                If SiteId.Trim <> "" Then
                    arrSiteId = SiteId.Split(",")

                    If arrSiteId.Length > 0 Then

                        'Header Content
                        oExcel.CreateHeader("Report", sHTML)
                        oExcel.CreateWorkSheetDatas(sHTML)
                        oExcel.InsertDataHeader("<img src='https://gms.centrak.com/gmsnew/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />", sHTML, False, 5, 2)

                        sSubHTML = New StringBuilder()
                        oExcel.CreateWorkSheetDatas(sSubHTML)
                        ReDim Preserve sVal(2)

                        sVal(0) = "Report&nbsp;Date&nbsp;:&nbsp;<label>" & CDate(Now()).Date & "</label>"
                        sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>Tags Not Seen Recently</label>"
                        sVal(2) = "Tags&nbsp;Last&nbsp;Seen&nbsp;Before&nbsp;:&nbsp;<label>" & strDate & "</label>"

                        oExcel.InsertDataArray(sVal, sSubHTML, True, 3)
                        oExcel.CloseWorkSheetDatas(sSubHTML)
                        oExcel.InsertData(sSubHTML.ToString(), sHTML, True)

                        For nIdx As Integer = 0 To arrSiteId.Length - 1
                            dtTag = GetOnDemandReportsInfo(arrSiteId(nIdx), TypeIds, strDate)

                            If Not dtTag Is Nothing Then
                                If dtTag.Rows.Count > 0 Then
                                    SiteName = CheckIsDBNull(dtTag.Rows(0).Item("Site"), , "")
                                End If
                            End If

                            'Data Content
                            oExcel.CreateWorkSheetDatas(sHTML)
                            'TAG
                            oExcel.InsertData(AddDatasToStringBuilderForOnDemandReport(dtTag, SiteName), sHTML)
                            oExcel.CloseWorkSheetDatas(sHTML)
                        Next

                        'Download Excel
                        oExcel.DownloadExcel(sFileName, sHTML)

                    End If
                End If
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadOnDemandReport " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddDatasToStringBuilderForOnDemandReport(ByVal dt As DataTable, ByVal SiteName As String) As String

            Dim oExcel As New Excel

            Dim sHTML As New StringBuilder
            Dim sVal() As String = Nothing
            Dim sCol As String = "align='center' style='height: 27px; width: 20%;'"
            Dim sCol1 As String = "align='center' style='height: 27px; width: 20%; background-color:#F9F8F8;'"

            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim nInnerIdx As Integer = 0
            Dim nColumns As Integer = 0

            Dim sTagType As String = ""
            Dim OldTagType As String = ""

            Dim sMonitorType As String = ""
            Dim OldMonitorType As String = ""

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            oExcel.CreateWorkSheetDatas(sHTML)

            ReDim Preserve sVal(4)

            For nRowIdx = 0 To dt.Rows.Count - 1

                If nRowIdx = 0 Then
                    oExcel.InsertDataHeader(SiteName, sHTML, True, 18)
                    oExcel.InsertRowHeader("Tag ID", sHTML, True, 2, , True)
                    oExcel.InsertRowHeader("Last Seen", sHTML, True, 2)
                    oExcel.InsertRowHeader("Last Moved", sHTML, True, 2)
                    oExcel.InsertRowHeader("Last Location", sHTML, True, 4)
                    oExcel.InsertRowHeader("Last Monitor", sHTML, True, 2)
                    oExcel.InsertRowHeader("Last Star", sHTML, True, 2)
                    oExcel.InsertRowHeader("Battery Bucket", sHTML, True, 2)
                    oExcel.InsertRowHeader("Battery Level", sHTML, True, 2, , , True)
                    oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("TagType"), sHTML, False, 18)
                    OldTagType = dt.Rows(nRowIdx).Item("TagType")
                End If

                sTagType = dt.Rows(nRowIdx).Item("TagType")

                If OldTagType <> sTagType Then
                    oExcel.InsertDataHeader(dt.Rows(nRowIdx).Item("TagType"), sHTML, False, 18)
                    OldTagType = sTagType
                End If

                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("TagId"), sHTML, False, 2, , True)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("LastSeen"), sHTML, False, 2)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("LastMoved"), sHTML, False, 2)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("LastLocation"), sHTML, False, 4)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("LastMonitor"), sHTML, False, 2)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("LastStar"), sHTML, False, 2)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("BatteryBucket"), sHTML, False, 2)
                oExcel.InsertRowHeader(dt.Rows(nRowIdx).Item("BatteryLevel"), sHTML, False, 2, , , True)
            Next

            oExcel.CloseWorkSheetDatas(sHTML)

            Return sHTML.ToString()

        End Function

        Public Function DownLoadExcelForInActiveDevice(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String) As Boolean

            Dim oExcel As New Excel

            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable
            Dim dtStar As New DataTable

            Dim sHTML As New StringBuilder
            Dim sSubHTML As StringBuilder
            Dim HeaderHTML As New StringBuilder
            Dim sVal As String() = Nothing
            Dim sFileName As String = ""
            Dim Sheetname As String = ""

            Dim bNoRecordFound As Boolean = False
            Dim IsExportforAllDevices As Boolean = False
            Dim apiKey As String = ""

            Try

                Sheetname = "All-INACTIVE-Devices"

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                If g_UserAPI = "" Then
                    apiKey = apiKey
                Else
                    apiKey = g_UserAPI
                End If

                LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", "3", "", "", apiKey, bNoRecordFound)

                dtStar = LoadStarList(Val(SiteId), enumDeviceType.Star, apiKey)

                If bNoRecordFound Then
                    dtTag = Nothing
                    dtMonitor = Nothing
                End If

                'Header Content
                oExcel.CreateHeader("Report", sHTML)
                oExcel.CreateWorkSheetDatas(sHTML)
                oExcel.InsertDataHeader("<img src='https://gms.centrak.com/gmsnew/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />", sHTML, False, 5, 2)
                oExcel.InsertDataHeader("&nbsp;", sHTML)
                oExcel.InsertDataHeader(SiteName, sHTML, True, 5)
                oExcel.InsertDataHeader("&nbsp;", sHTML)

                sSubHTML = New StringBuilder()
                oExcel.CreateWorkSheetDatas(sSubHTML)
                ReDim Preserve sVal(2)

                sVal(0) = "Date&nbsp;:&nbsp;<label>" & CDate(Now()).Date & "</label>"
                sVal(1) = "Report&nbsp;Type&nbsp;:&nbsp;<label>All INACTIVE Devices</label>"

                oExcel.InsertDataArray(sVal, sSubHTML, True)
                oExcel.CloseWorkSheetDatas(sSubHTML)
                oExcel.InsertData(sSubHTML.ToString(), sHTML)

                'Data Content
                oExcel.CreateWorkSheetDatas(sHTML)

                'TAG
                oExcel.InsertData(AddInActiveDeviceExcelDatasToStringBuilder(dtTag, enumDeviceType.Tag, RptType), sHTML)

                'MONITOR
                oExcel.InsertData(AddInActiveDeviceExcelDatasToStringBuilder(dtMonitor, enumDeviceType.Monitor, RptType), sHTML)

                'STAR
                oExcel.InsertData(AddInActiveDeviceExcelDatasToStringBuilder(dtStar, enumDeviceType.Star, RptType), sHTML)
                oExcel.CloseWorkSheetDatas(sHTML)

                'Download Excel
                oExcel.DownloadExcel(sFileName, sHTML)
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadExcel " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddInActiveDeviceExcelDatasToStringBuilder(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal RptType As String) As String

            Dim oExcel As New Excel

            Dim sHTML As New StringBuilder
            Dim sCol As String = "align='center' style='height: 41px; width: 125px;'"
            Dim sCol1 As String = "align='center' style='height: 41px; width: 125px; background-color:#F9F8F8;'"
            Dim sVal() As String = Nothing

            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            oExcel.CreateWorkSheetDatas(sHTML)

            For nRowIdx = 0 To dt.Rows.Count - 1

                If nRowIdx = 0 Then
                    ReDim Preserve sVal(3)
                    sVal(0) = "Device ID"
                    sVal(1) = "Device Type"
                    sVal(2) = "Last Seen"
                    sVal(3) = "Device Status"

                    oExcel.InsertDataArray(sVal, sHTML, True)
                End If

                If DeviceType = enumDeviceType.Tag Then
                    sVal(0) = dt.Rows(nRowIdx).Item("TagId")
                    sVal(1) = "TAG"
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    sVal(0) = dt.Rows(nRowIdx).Item("DeviceId")
                    sVal(1) = "MONITOR"
                Else
                    sVal(0) = dt.Rows(nRowIdx).Item("MacId")
                    sVal(1) = "Star"
                End If

                If DeviceType = enumDeviceType.Star Then
                    sVal(2) = dt.Rows(nRowIdx).Item("LastReceivedTime")
                Else
                    sVal(2) = dt.Rows(nRowIdx).Item("LastSeen")
                End If

                sVal(3) = "inactive"
                oExcel.InsertDataArray(sVal, sHTML)
            Next

            oExcel.CloseWorkSheetDatas(sHTML)

            Return sHTML.ToString()
        End Function

        Public Function DownloadPdfForInActiveDevice(ByVal SiteId As String, ByVal RptType As String, ByVal SiteName As String) As Boolean

            Dim dtTag As New DataTable
            Dim dtMonitor As New DataTable
            Dim dtStar As New DataTable

            Dim theDoc As Doc = New Doc()
            theDoc.HtmlOptions.Timeout = 10000000
            theDoc.HtmlOptions.Engine = EngineType.Gecko

            Dim sHTML As New StringBuilder
            Dim HeaderHTML As New StringBuilder

            Dim sFileName As String = ""
            Dim Sheetname As String = ""
            Dim bNoRecordFound As Boolean = False
            Dim HTMLsaveFloder As String = "HTML"
            Dim HTML_filepath As String = ""
            Dim apiKey As String = ""

            Try
                If Not Directory.Exists(GetAppPath() & "\" & HTMLsaveFloder) Then
                    Directory.CreateDirectory(GetAppPath() & "\" & HTMLsaveFloder)
                End If

                Sheetname = "All-INACTIVE-Devices"

                sFileName = "CenTrak-GMS-Reports-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                HTML_filepath = GetAppPath() & "\" & HTMLsaveFloder & "\" & sFileName & "_" & g_UserId & ".html"

                theDoc.Rect.Inset(20, 25)
                theDoc.Page = theDoc.AddPage()

                If g_UserAPI = "" Then
                    apiKey = apiKey
                Else
                    apiKey = g_UserAPI
                End If

                LoadDeviceListPrintPage(dtTag, dtMonitor, Val(SiteId), "0", "", "", "0", "", "3", "", "", apiKey, bNoRecordFound)

                dtStar = LoadStarList(Val(SiteId), enumDeviceType.Star, apiKey)

                If bNoRecordFound Then
                    dtTag = Nothing
                    dtMonitor = Nothing
                End If

                'Header Content
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td><img src='" & GetServerPath() & "/Images/Logo.png' style='width: 233px; height: 40px;' alt='Logo' />")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td align='left' style='border-bottom: solid 5px #245E90;'>")
                HeaderHTML.Append("&nbsp;</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td valign='top' align='center'>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; color: #373737; border-bottom: solid 1px #D3D3D3;height:40px;' valign='middle' class='sText'>")
                HeaderHTML.Append("<label class='SHeader1'>")
                HeaderHTML.Append(SiteName)
                HeaderHTML.Append("</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td>")
                HeaderHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%' height='40px'>")
                HeaderHTML.Append("<tr>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; width: 20%; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Date&nbsp;:&nbsp;")
                HeaderHTML.Append("<label>" & CDate(Now()).Date & "</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("<td style='text-align: left; font-weight:bold;color: #8A8A8A; font-family: sans-serif; font-size: 14px; font-family: Arial;' class='sText'>")
                HeaderHTML.Append("Report&nbsp;Type&nbsp;:&nbsp;")
                HeaderHTML.Append("<label>All INACTIVE Devices</label>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")
                HeaderHTML.Append("</td>")
                HeaderHTML.Append("</tr>")
                HeaderHTML.Append("</table>")

                'BODY Content
                sHTML.Append(GetBodyContent())

                sHTML.Append(HeaderHTML.ToString())

                sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")
                sHTML.Append("<tr style='height: 20px;'>")
                sHTML.Append("</tr>")
                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(AddInActiveDatasToStringBuilder(dtTag, enumDeviceType.Tag, HeaderHTML.ToString))
                sHTML.Append("</td>")
                sHTML.Append("</tr>")

                If Not dtMonitor Is Nothing Then
                    If dtMonitor.Rows.Count > 0 Then
                        sHTML.Append("<tr>")
                        sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                        sHTML.Append("</td>")
                        sHTML.Append("</tr>")

                        sHTML.Append("<tr>")
                        sHTML.Append("<td colspan='15'>")
                        sHTML.Append(HeaderHTML)
                        sHTML.Append("</td>")
                        sHTML.Append("</tr>")
                    End If
                End If

                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(AddInActiveDatasToStringBuilder(dtMonitor, enumDeviceType.Monitor, HeaderHTML.ToString))
                sHTML.Append("</td>")
                sHTML.Append("</tr>")

                If Not dtStar Is Nothing Then
                    If dtStar.Rows.Count > 0 Then
                        sHTML.Append("<tr>")
                        sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                        sHTML.Append("</td>")
                        sHTML.Append("</tr>")

                        sHTML.Append("<tr>")
                        sHTML.Append("<td colspan='15'>")
                        sHTML.Append(HeaderHTML)
                        sHTML.Append("</td>")
                        sHTML.Append("</tr>")
                    End If
                End If

                sHTML.Append("<tr>")
                sHTML.Append("<td>")
                sHTML.Append(AddInActiveDatasToStringBuilder(dtStar, enumDeviceType.Star, HeaderHTML.ToString))

                sHTML.Append("</td>")
                sHTML.Append("</tr>")
                sHTML.Append("</table>")
                sHTML.Append("</body>")
                sHTML.Append("</html>")

                File.AppendAllText(HTML_filepath, sHTML.ToString)

                Dim theID As Integer
                theDoc.Rect.String = "50 60 573 750"
                theID = theDoc.AddImageUrl("file:///" & HTML_filepath)

                While True
                    If Not theDoc.Chainable(theID) Then
                        Exit While
                    End If
                    theDoc.Page = theDoc.AddPage()
                    theID = theDoc.AddImageToChain(theID)
                End While

                theDoc.Rect.Position(465, 594)
                theDoc.Rect.Width = 100
                theDoc.Rect.Height = 65

                Dim i As Integer
                For i = 1 To theDoc.PageCount
                    theDoc.PageNumber = i
                    theDoc.AddHtml("<p align='right'><font size='3px' color= '#8A8A8A' font-family= 'Arial'>Page&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & i & "</font><font size='3px' color= '#8A8A8A' font-family= 'Arial'>&nbsp;of&nbsp;</font><font size='3px' color= '#245E90' font-family= 'Arial'>" & theDoc.PageCount & "</font></p>")
                    theDoc.Flatten()
                Next

                theDoc.SetInfo(theDoc.Root, "/OpenAction", "[ 1 0 R /XYZ null null 2 ]")

                Dim pdfbytestream As Byte() = theDoc.GetData()
                theDoc.Clear()
                theDoc.Dispose() 'free up unused object
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.ClearHeaders()
                HttpContext.Current.Response.ClearContent()
                HttpContext.Current.Response.Buffer = True
                HttpContext.Current.Response.ContentType = "application/pdf"
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & sFileName & ".pdf")
                HttpContext.Current.Response.AddHeader("content-length", pdfbytestream.Length.ToString())
                HttpContext.Current.Response.BinaryWrite(pdfbytestream)

                If File.Exists(HTML_filepath) Then
                    File.Delete(HTML_filepath)
                End If

                HttpContext.Current.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" DownloadPdf " & ex.Message.ToString())
                Return False
            End Try

            Return True
        End Function

        Public Function AddInActiveDatasToStringBuilder(ByVal dt As DataTable, ByVal DeviceType As Integer, Optional ByVal HeaderHTML As String = "") As String

            Dim sHTML As New StringBuilder
            Dim nRowIdx As Integer = 0
            Dim nColIdx As Integer = 0
            Dim initialMonitorIdx As Integer = 0
            Dim pageBreakCount As Integer = 0
            Dim sDeviceType As String = ""

            Dim sCol As String = "align='center' style='height: 41px; width: 200px;'"
            Dim sCol1 As String = "align='center' style='height: 41px; width: 125px; background-color:#F9F8F8;'"

            If dt Is Nothing Then
                Return ""
                Exit Function
            End If

            sHTML.Append("<table border='0' cellpadding='0' cellspacing='0' width='100%'>")

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then

                    For nRowIdx = 0 To dt.Rows.Count - 1
                        If nRowIdx Mod 19 = 0 Then
                            If nRowIdx <> 0 Then
                                sHTML.Append("<tr>")
                                sHTML.Append("<td><div style='page-break-before:always'>&nbsp;</div>")
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")

                                sHTML.Append("<tr>")
                                sHTML.Append("<td colspan='15'>")
                                sHTML.Append(HeaderHTML)
                                sHTML.Append("</td>")
                                sHTML.Append("</tr>")
                            End If

                            'Header
                            sHTML.Append("<tr>")
                            sHTML.Append("<td class='Header_TopLeft_Box' " & sCol & " align='center'>Device ID</td>")
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & " align='center'>Device Type</td>")
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & " align='center'>Last Seen</td>")
                            sHTML.Append("<td class='siteOverview_Box' " & sCol1 & " align='center'>Device Status</td>")
                            sHTML.Append("</tr>")
                        End If

                        'Datas
                        sHTML.Append("<tr style='height:41px;'>")

                        If DeviceType = enumDeviceType.Tag Then
                            sDeviceType = "TAG"
                            sHTML.Append("<td class='DeviceList_leftBox' " & sCol & ">" & dt.Rows(nRowIdx).Item("TagId") & "</td>")
                        ElseIf DeviceType = enumDeviceType.Monitor Then
                            sDeviceType = "MONITOR"
                            sHTML.Append("<td class='DeviceList_leftBox' " & sCol & ">" & dt.Rows(nRowIdx).Item("DeviceId") & "</td>")
                        ElseIf DeviceType = enumDeviceType.Star Then
                            sDeviceType = "STAR"
                            sHTML.Append("<td class='DeviceList_leftBox' " & sCol & ">" & dt.Rows(nRowIdx).Item("MacId") & "</td>")
                        End If

                        sHTML.Append("<td class='siteOverview_cell' " & sCol & ">" & sDeviceType & "</td>")

                        If DeviceType = enumDeviceType.Star Then
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">" & dt.Rows(nRowIdx).Item("LastReceivedTime") & "&nbsp;</td>")
                        Else
                            sHTML.Append("<td class='siteOverview_cell' " & sCol & ">" & dt.Rows(nRowIdx).Item("LastSeen") & "&nbsp;</td>")
                        End If

                        sHTML.Append("<td class='siteOverview_cell' " & sCol & ">inactive</td>")
                        sHTML.Append("</tr>")

                    Next
                End If
            End If

            sHTML.Append("</table>")

            Return sHTML.ToString()
        End Function

        Public Function CSVNewLine() As String
            Dim ret As String = ""
            ret = ret & vbCrLf
            Return ret
        End Function

        Public Function CSVCell(ByVal stext As String, Optional ByVal bColSep As Boolean = False) As String
            Dim ret As String = ""

            If bColSep Then
                ret = stext & ","
            Else
                ret = stext
            End If

            Return ret
        End Function

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

        Public Function GetBodyContent() As StringBuilder

            Dim sHTML As New StringBuilder

            'BODY Content
            sHTML.Append("<html>")
            sHTML.Append("<head>")
            sHTML.Append("<style>")
            sHTML.Append(".siteOverview_TopLeft_Box" & _
                           "{" & _
                                  "border-top-right-radius: 5px;" & _
                                  "-webkit-border-top-right-radius: 5px;" & _
                                  "-moz-border-top-right-radius: 5px;" & _
                                  "border-top-right-radius: 5px;" & _
                                  "border-bottom:1px solid #DADADA;" & _
                                  "border-right:1px solid #DADADA;" & _
                                  "background-image: url('" & GetServerPath() & "/Images/tblHeaderbg.png');" & _
                                  "background-repeat:no-repeat;background-size:130px 60px;" & _
                                  "color:White;" & _
                                  "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                                  "font-size: 12px;" & _
                                  "font-weight:bold;" & _
                                  "text-align:center;" & _
                                  "vertical-align: middle;" & _
                          "}" & _
                          ".Header_TopLeft_Box" & _
                          "{" & _
                              "border-top-right-radius: 5px;" & _
                              "-webkit-border-top-right-radius: 5px;" & _
                              "-moz-border-top-right-radius: 5px;" & _
                              "border-top-right-radius: 5px;" & _
                              "border-bottom:1px solid #DADADA;" & _
                              "border-right:1px solid #DADADA;" & _
                              "background-repeat:no-repeat;background-size:130px 60px;" & _
                              "color:White;" & _
                              "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                              "background-color:#245e90;" & _
                              "font-size: 12px;" & _
                              "font-weight:bold;" & _
                              "text-align:center;" & _
                              "vertical-align: middle;" & _
                          "}" & _
                          ".siteOverview_Box" & _
                          "{" & _
                              "border-top:1px solid #DADADA;" & _
                              "border-bottom:1px solid #DADADA;" & _
                              "border-right:1px solid #DADADA;" & _
                              "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                              "font-size: 12px;" & _
                              "font-weight:bold;" & _
                              "text-align:center;" & _
                              "vertical-align: middle;" & _
                              "color: #454545;" & _
                          "}" & _
                          ".siteOverview_Topright_Box" & _
                          "{" & _
                              "border-top-right-radius: 5px;" & _
                              "-webkit-border-top-right-radius: 5px;" & _
                              "-moz-border-top-right-radius: 5px;" & _
                              "border-top-right-radius: 5px;" & _
                              "border-bottom:1px solid #DADADA;" & _
                              "border-right:1px solid #DADADA;" & _
                              "border-top:1px solid #DADADA;" & _
                              "color:#454545;" & _
                              "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                              "font-size: 12px;" & _
                              "font-weight:bold;" & _
                              "text-align:center;" & _
                              "vertical-align: middle;" & _
                          "}" & _
                          ".DeviceList_leftBox" & _
                          "{" & _
                              "border-bottom:1px solid #DADADA;" & _
                              "border-left:1px solid #DADADA;" & _
                              "border-right:1px solid #DADADA;" & _
                              "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                              "font-size: 12px;" & _
                              "font-weight:bold;" & _
                              "text-align:center;" & _
                              "vertical-align: middle;" & _
                              "color: #1F5E93;" & _
                          "}" & _
                          ".siteOverview_cell" & _
                          "{" & _
                              "border-bottom:1px solid #DADADA;" & _
                              "border-right:1px solid #DADADA;" & _
                              "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                              "font-size: 12px;" & _
                              "font-weight:bold;" & _
                              "text-align:center;" & _
                              "vertical-align: middle;" & _
                              "color: #454545;" & _
                          "}" & _
                          ".SHeader1" & _
                          "{" & _
                              "font-family: Helvetica,MyriadPro,Verdana, Arial, sans-serif;" & _
                              "font-size: 19px;" & _
                              "font-weight:bold;" & _
                              "height:30px;" & _
                              "vertical-align: middle;" & _
                              "color: #1a1a1a;" & _
                          "}")
            sHTML.Append("</style>")
            sHTML.Append("</head>")
            sHTML.Append("<body>")

            Return sHTML
        End Function

#End Region

        Public Function GetBeaconSlot(ByVal StarId As Integer) As Byte

            Dim slot As Byte = StarId Mod 9

            If slot = 0 Then slot = 9
            If slot >= 5 Then slot += 1

            Return slot

        End Function

        Public Function GetSpecialBeaconSlot(ByVal StarId As Integer) As Byte

            Dim slot As Byte = GetBeaconSlot(StarId)
            Dim s1 As Byte

            Try
                s1 = StarId \ 9
            Catch ex As Exception
                Return 0
            End Try


            If slot = 10 And s1 > 0 Then
                s1 -= 1
            End If

            slot = (slot + s1 * 12) Mod 48

            Return slot

        End Function

        Public Function SendEmail(ByVal UserId As Integer, ByVal UserName As String, ByVal Role As String, ByVal BodyMessage As String, ByVal Type As Integer) As Boolean

            Dim sEmailUid, sFromAddress, sSubject, MailServer, sPort, sEmailPwd As String
            Dim objMail As New MailMessage
            Dim Client As New SmtpClient
            Dim blnResult As Boolean

            Dim EmailId As String = ConfigurationManager.AppSettings("Email")

            blnResult = False

            Try

                MailServer = ConfigurationManager.AppSettings("Host")
                sEmailUid = ConfigurationManager.AppSettings("UserName")
                sEmailPwd = ConfigurationManager.AppSettings("Password")
                sPort = ConfigurationManager.AppSettings("Port")

                sFromAddress = "no-reply@centrak.com"

                If Type = 1 Then
                    sSubject = " Connect Pulse Account locked - " + UserName + " - " + Role + " - inactivity"
                Else
                    sSubject = " Connect Pulse Account locked - " + UserName + " - " + Role + " - exceeds limit"
                End If

                objMail.From = New MailAddress(sFromAddress)
                objMail.To.Add(New MailAddress(EmailId))

                objMail.Subject = sSubject

                objMail.Body = GetBodyMsg(UserName, BodyMessage)

                objMail.IsBodyHtml = True
                objMail.Priority = MailPriority.Normal

                'MailConfiguration
                Client.Host = MailServer
                Client.Port = sPort

                Client.Credentials = New System.Net.NetworkCredential(sEmailUid, sEmailPwd)

                'Send Mail
                Client.Send(objMail)

                blnResult = True

                WriteEmailLog("Forgot Password Email Sent-> EmailId: " & EmailId & ", Reset Key: " & UserId)
                WriteEmailLog("Body Msg: " & objMail.Body.ToString())

            Catch ex As Exception

                WriteEmailLog("-------------------------------------")

                If Not ex.InnerException Is Nothing Then
                    WriteEmailLog("Sending mail failed. Error Message: " & ex.Message & ", Inner Exp: " & ex.InnerException.Message)
                Else
                    WriteEmailLog("Sending mail failed. Error Message: " & ex.Message)
                End If

                WriteEmailLog("EmailId:" & EmailId & ", Username:" & UserName & ", Message:" & objMail.Body.ToString())
                WriteEmailLog("-------------------------------------")

                blnResult = False
            End Try

            Return blnResult

        End Function

        ''' <summary>
        ''' To Write the Log
        ''' </summary>
        ''' <param name="str"></param>
        ''' <remarks></remarks>
        Public Sub WriteEmailLog(ByVal str As String)
            Try
                Dim strFilename As String
                Dim fs As FileStream
                Dim sw As StreamWriter
                Dim dt As DateTime

                strFilename = GetAppPath() & "\GMSEmail_log.txt"

                fs = New FileStream(strFilename, FileMode.Append)
                sw = New StreamWriter(fs)
                dt = DateTime.Now
                str = "[" & dt.ToString("dd-MM-yyyy HH:mm:ss") + "] - " & str
                sw.WriteLine(str)
                sw.Close()
                fs.Close()

            Catch ex As Exception
                Dim strMessage As String
                strMessage = ex.Message
            End Try
        End Sub

        Public Function GetBodyMsg(ByVal UserName As String, ByVal msg As String) As String

            Dim sBodyContent As String

            sBodyContent = ""

            Try

                sBodyContent = "<html><body><p><br>"

                sBodyContent &= msg & "<br><br>"
                sBodyContent &= "UserName : " & UserName & "</a></p>"
                sBodyContent &= "<table class='tblList'>"
                sBodyContent &= "<td>This is an Password requisition email. Please do not reply. Thanks</td></tr>"
                sBodyContent &= "<tr><td align='left'><b>CenTrak Connect Pulse.</b></td></tr></table></body></html>"

            Catch ex As Exception
                WriteLog("GetBodyMsg: " & ex.Message)
            End Try

            Return sBodyContent

        End Function
       
       Public Sub DownloadCSVForMetadataReport(ByVal SiteName As String, ByVal SiteId As String, ByVal CurPage As String, ByVal PageSize As String, ByVal SearchValue As String, ByVal devicetype As String)

            Dim dt As New DataTable

            Dim strColumns As String = ""
            Dim sFileName As String = "CenTrak-MetaData-Reports-" & Format(Now, "MMddyyyy") & "-" & Format(Now, "HHmmss")

            Dim Csvtext As StringBuilder = New StringBuilder
            Dim context As HttpContext

            dt = GetCetaniMetaDataInfo(SiteId, SearchValue, CurPage, PageSize, devicetype)

            '' Tag List
            If devicetype = 1 Then

                strColumns = "Tag Id,Mac,Name,Custom,Pos MapId,Pos ZoneId,Pos X,Pos Y,Item Id,Description,Model,Manufacturer,Part Number,Order Number,Po Number," +
                             "Line Number,Serial Number,System Id,Checkout To UserId,Checkout Date,Checkout Due Date,Parent Item Id,Department Id,Map Link Id,Pos Reason," +
                             "Pos Timestamp,Battery,Status Id,Location State,Last Location State Change,Star Id,Monitor Id,Previous Monitor Id,Previous Monitor Time,Pos Zonesince," +
                             "Pos Map Name,Pos Zone Name,Button Timestamp,Tamper Timestamp,Status Name,Item Set Name,Item Set Name2"

            ElseIf devicetype = 2 Then '' maps

                strColumns = "Id,Title,Description,Map Unique,Facility Id,sort_index,map_set_id,image_zoom_level,created_at,timeout_area_id,width_pix,height_pix,width_feet,height_feet," +
                             "horz_offset_feet,vert_offset_feet,External Filename,scale_x_ppf,scale_y_ppf,offset_x_pix,offset_y_pix,wifi_facility_name,wifi_map_name,wifi_map_x,wifi_map_height,visibility,filename"

            ElseIf devicetype = 3 Then '' areas

                strColumns = "Id,map_id,title,polygon,expires_on,description,status,from_system,area_usage,image_file,area_group_id,input_index,input_value,input_last_on,input_last_off," +
                             "ringtone,created_at,updated_at,color,default_area_state_id,current_area_state_id,state_since,c_x,c_y,occupied,rounding_status,jump_map_id,department_id," +
                             "population_flag,battery_state,battery_state_since,block_wifi_locate,closest_waypoint_identifier,x,y,width,height,angle,countdown_date,room_requires_rounding," +
                             "emr_dept,emr_room,emr_room_identifier,emr_dept_number,emr_lastHL7_message,emr_lastmessage,plf_id,locator_id,zone_type"

            End If

            Dim arrcolumns As String() = strColumns.Split(",")

            context = HttpContext.Current
            InitiateCSV(context, sFileName)

            Try

                If arrcolumns.Length > 0 Then

                    For Col As Integer = 0 To arrcolumns.Length - 1
                        With arrcolumns(Col)
                            AddCSVCell(context, arrcolumns(Col), True)
                        End With
                    Next

                    AddCSVNewLine(context)

                    If Not dt Is Nothing Then
                        If dt.Rows.Count > 0 Then

                            For nIdx As Integer = 0 To dt.Rows.Count - 1

                                With dt.Rows(nIdx)
                                    For Col As Integer = 0 To arrcolumns.Length - 1

                                        If devicetype = 1 Then
                                            AddCSVCell(context, .Item(arrcolumns(Col).Replace(" ", "")), True)
                                        ElseIf devicetype = 2 Then
                                            AddCSVCell(context, .Item(arrcolumns(Col).Replace(" ", "_")), True)
                                        ElseIf devicetype = 3 Then
                                            AddCSVCell(context, .Item(arrcolumns(Col).Replace(" ", "_")), True)
                                        End If

                                    Next
                                End With

                                AddCSVNewLine(context)

                            Next

                        End If
                    End If

                End If

            Catch ex As Exception
                WriteLog("DownloadCSVForMetadataReport " & ex.Message.ToString)
            End Try

            HttpContext.Current.Response.BufferOutput = True
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.[End]()

        End Sub
	
        Public Function Crypt(ByVal text As String) As String

            Dim algorithm As SymmetricAlgorithm = DES.Create()
            Dim transform As ICryptoTransform = algorithm.CreateEncryptor(key, iv)
            Dim inputbuffer As Byte() = Encoding.Unicode.GetBytes(text)
            Dim outputBuffer As Byte() = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length)
            Return Convert.ToBase64String(outputBuffer)

        End Function

        Public Function Decrypt(ByVal text As String) As String

            Dim algorithm As SymmetricAlgorithm = DES.Create()
            Dim transform As ICryptoTransform = algorithm.CreateDecryptor(key, iv)
            Dim inputbuffer As Byte() = Convert.FromBase64String(text)
            Dim outputBuffer As Byte() = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length)
            Return Encoding.Unicode.GetString(outputBuffer)

        End Function
	
        Public Sub CreateAuthToken()

            'Create new AuthToken for this session
            Dim NewGuid As String = Guid.NewGuid.ToString()
            HttpContext.Current.Session("AuthToken") = NewGuid
            HttpContext.Current.Response.Cookies.Add(New HttpCookie("AuthToken", NewGuid))

        End Sub

        Public Function IsAuthTokenValid() As Boolean
            Dim Pass As Boolean = True

            If (HttpContext.Current.Session("AuthToken") IsNot Nothing) And (HttpContext.Current.Request.Cookies("AuthToken") IsNot Nothing) Then

                If Not HttpContext.Current.Session("AuthToken").ToString().Equals(HttpContext.Current.Request.Cookies("AuthToken").Value) Then
                    Pass = False
                End If

            End If
            Return Pass
        End Function

        Public Sub ExpiresCookies() 'Session Fixation
            If HttpContext.Current.Request.Cookies("ASP.NET_SessionId") IsNot Nothing Then
                HttpContext.Current.Response.Cookies("ASP.NET_SessionId").Value = String.Empty
                HttpContext.Current.Response.Cookies("ASP.NET_SessionId").Expires = DateTime.Now.AddMonths(-20)
            End If

            If HttpContext.Current.Request.Cookies("AuthToken") IsNot Nothing Then
                HttpContext.Current.Response.Cookies("AuthToken").Value = String.Empty
                HttpContext.Current.Response.Cookies("AuthToken").Expires = DateTime.Now.AddMonths(-20)
            End If
        End Sub

        Sub FillNIStCertificateInfo(ByRef dtTaglist As DataTable)

            Dim sApppath As String = GetAppPath()
            Dim sFileName As String

            Try
                dtTaglist.Columns.Add(New DataColumn("NISTFile", Type.[GetType]("System.String")))
                dtTaglist.Columns.Add(New DataColumn("Probe1NISTFile", Type.[GetType]("System.String")))
                dtTaglist.Columns.Add(New DataColumn("Probe2NISTFile", Type.[GetType]("System.String")))

                For nIdx As Integer = 0 To dtTaglist.Rows.Count - 1

                    'Tag Id
                    sFileName = sApppath & "/Certificate/" & dtTaglist.Rows(nIdx).Item("TagId") & ".pdf"
                    dtTaglist.Rows(nIdx).Item("NISTFile") = "0"
                    If (File.Exists(sFileName)) Then
                        dtTaglist.Rows(nIdx).Item("NISTFile") = "1"
                    End If

                    'Probe Id1
                    sFileName = sApppath & "/Certificate/" & dtTaglist.Rows(nIdx).Item("ProbeId") & ".pdf"
                    dtTaglist.Rows(nIdx).Item("Probe1NISTFile") = "0"
                    If (File.Exists(sFileName)) Then
                        dtTaglist.Rows(nIdx).Item("Probe1NISTFile") = "1"
                    End If

                    'Probe Id2
                    sFileName = sApppath & "/Certificate/" & dtTaglist.Rows(nIdx).Item("ProbeId2") & ".pdf"
                    dtTaglist.Rows(nIdx).Item("Probe2NISTFile") = "0"
                    If (File.Exists(sFileName)) Then
                        dtTaglist.Rows(nIdx).Item("Probe2NISTFile") = "1"
                    End If

                Next
            Catch ex As Exception
                WriteLog("FillNIStCertificateInfo " & ex.Message.ToString())
            End Try

            dtTaglist.AcceptChanges()
	    
        End Sub
        
	Public Function GetValidSiteName(ByVal SiteName As String) As String

            SiteName = SiteName.Replace("(", "").Replace(")", "").Replace(",", "").Replace("'", "").Replace("[", "").Replace("]", "").Replace("-", "_")

            Return SiteName.Replace(" ", "_")

        End Function

        Function HistoricalXMLNodeToDataTable(ByVal xmlNd As XmlNode, Optional ByRef isERROR As Boolean = False) As DataTable

            Dim dtTemp As New DataTable
            Dim dtError As New DataTable

            Dim dNewRow As DataRow = Nothing

            Dim report_node As XmlNode
            Dim report_node_list As XmlNodeList

            Dim reports_node As XmlNode
            Dim reports_node_list As XmlNodeList

            Dim Nodename As String = ""
            Dim SubNodename As String = ""
            Dim valueList As String = ""

            Dim sMeasurmentRate As String = ""
            Dim TimeZone As String = ""
            Dim TagId As Integer

            dtTemp.Columns.Add(New DataColumn("ProbType", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("Time", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("Temp", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("GMTTime", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("MeasurementRate", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("Status", Type.[GetType]("System.String")))
            dtTemp.Columns.Add(New DataColumn("TimeZone", Type.[GetType]("System.String")))

            dtError.Columns.Add(New DataColumn("Error", Type.[GetType]("System.String")))

            Try

                Dim cnt As Long = 0
                cnt = xmlNd.ChildNodes.Count

                If cnt > 0 Then

                    For i As Integer = 0 To cnt - 1

                        report_node = xmlNd.ChildNodes(i)
                        Nodename = report_node.Name
                        report_node_list = report_node.ChildNodes

                        If Nodename = "MeasurmentRate" Then
                            sMeasurmentRate = xmlNd.ChildNodes(i).FirstChild.Value
                        End If

                        If Nodename = "TimeZone" Then
                            TimeZone = xmlNd.ChildNodes(i).FirstChild.Value
                        End If

                        If Nodename = "ERROR" Then

                            isERROR = True
                            dNewRow = dtError.NewRow()
                            dNewRow("Error") = xmlNd.ChildNodes(i).FirstChild.Value
                            dtError.Rows.Add(dNewRow)

                            Return dtError
                        End If

                        If Nodename = "Probe1" Or Nodename = "Probe2" Then
                            For k As Integer = 0 To report_node_list.Count - 1
                                reports_node = xmlNd.ChildNodes(i).ChildNodes(k)
                                SubNodename = reports_node.Name
                                reports_node_list = reports_node.ChildNodes

                                If SubNodename = "Time" Then
                                    dNewRow = dtTemp.NewRow()
                                    dNewRow("ProbType") = Nodename
                                    dNewRow("MeasurementRate") = sMeasurmentRate
                                    dNewRow("TimeZone") = TimeZone
                                End If

                                If SubNodename = "Time" Or SubNodename = "Temp" Or SubNodename = "GMTTime" Or SubNodename = "Status" Then
                                    For j As Integer = 0 To reports_node_list.Count - 1
                                        If reports_node_list(j).Name = "Values" Then
                                            If valueList = "" Then
                                                valueList = reports_node_list(j).InnerText
                                            Else
                                                valueList &= "," & reports_node_list(j).InnerText
                                            End If
                                        Else
                                            Dim Alert_nodename As String = reports_node_list(j).Name
                                            Dim Alert_nodeValue As String = reports_node_list(j).InnerText
                                            dNewRow(SubNodename) = Alert_nodeValue
                                        End If
                                    Next
                                    valueList = ""
                                End If
                                'dNewRow("Values") = valueList
                                If SubNodename = "Temp" Then dtTemp.Rows.Add(dNewRow)
                            Next
                        End If

                    Next
                End If

            Catch ex As Exception
                'WriteLog(" HistoricalXMLNodeToDataTable " & ex.Message.ToString)
            End Try

            Return dtTemp

        End Function

        Public Enum enumEMTempReport

            Inactivity = 1
            Connectivity = 2
            PCE = 3
            ExcessivePaging = 4

        End Enum

        Public Enum enumTagSubType_New

            AssetTag = 1
            AssetTag_Autoclave = 2
            AssetTag_DuraTag = 3
            AssetTag_Micro = 4
            AssetTag_MiniMM = 5
            AssetTag_Mini = 6
            AssetTag_MM = 7

            CallPoint = 8

            CastellModule = 9

            GeoPendant = 10
            GeoPendant_Black = 11
            GeoPendant_White = 12

            OnewayBinarymodule = 13

            PatientTag = 14
            PatientTag_31_DayBlue = 15
            PatientTag_31_DayGreen = 16
            PatientTag_31_DayOrange = 17
            PatientTag_Micro = 18
            PatientTag_MiniMM = 19
            PatientTag_MiniSealed = 20
            PatientTag_MiniStandalone = 21
            PatientTag_Mini = 22
            PatientTag_MM = 23
            PatientTag_newbabyUmbilical = 24
            PatientTag_newmom = 25
            PatientTag_SecureTagAdult = 26
            PatientTag_SecureTagnewbaby = 27
            PatientTag_SecureTagRugged = 28
            PatientTag_SingleUse = 29

            StaffTag = 30
            StaffTag_Duress = 31
            StaffTag_MM = 32

            SurveyTag = 33

            UniversalTransmitter = 34

            AmbientSensor_TempHumidity = 35
            AmbientSensor_TempHumNonDsp = 36
            CO2Sensor_Display = 37
            CO2Sensor_G1Display = 38
            DiffAirPressureSensor_G1 = 39
            DifferentialAirPressureSensor = 40
            O2Sensor_Display = 41
            O2Sensor_G1Display = 42
            TemperatureSensor_G1NonDsp = 43
            TemperatureSensor_Standard = 44
            TemperatureSensor_StndNonDsp = 45
            TemperatureSensor_Ultra_Low = 46
            TemperatureSensor_VAC = 47

        End Enum

        Public Enum enumDeviceCategory

            AssetandPatientTag = 0
            StaffandHHCTag = 1
            G1TempTag = 2
            HumidityTag = 4
            G2TempTag = 5

        End Enum

    End Module

End Namespace

