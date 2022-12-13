Imports System.IO
Imports System.Data
Imports System.Xml
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports WebSupergoo.ABCpdf11

Namespace GMSUI
    Partial Class AjaxConnector
        Inherits System.Web.UI.Page

        'Testbed Declarations
        Dim dtReq As New DataTable
        Dim drReq As DataRow = Nothing

        Dim timeBefore As Date = Nothing
        Dim timeAfter As Date = Nothing

        Dim siteUrl As String = ""
        Dim siteId As String = ""
        Dim sTestbedAction As String = ""
        Dim sResponsefromServer As String = ""

        Dim respCount As Integer = 0
        Dim failed As Integer = 0

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Session Timeout Reload
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                Dim dtSession As New DataTable
                dtSession = SessionExpired()
                SendXMLToAJAXCall(dtSession)
            End If

            'Ajax Load
            Dim sQuery As String = ""

            Dim sCmd As String
            Dim sid As String = ""
            Dim Masterid As String = ""
            Dim UserName As String = ""
            Dim Password As String = ""
            Dim DeviceId As String = ""
            Dim Bin As String = ""
            Dim FilterCriteria As String = ""
            Dim refResponseFromServer As String = ""
            Dim DeviceType As String = ""
            Dim Duration As String = ""
            Dim Type As String = ""
            Dim dtResp As New DataTable
            Dim alertId As String = ""
            Dim curpage As String = "1"
            Dim sorColumnname As String = ""
            Dim SorOrder As String = ""
            Dim typId As String = ""
            Dim orgtypeId As String = "0"
            Dim PageNo As Integer = 0
            Dim EmailId As String
            Dim FromDate As String = ""
            Dim ToDate As String = ""
            Dim isExport As Boolean = False
            Dim ServiceId As String = ""
            Dim DateRngType As String = ""
            Dim StartDate As String = ""
            Dim PgType As String = ""
            Dim AlertGraphType As String = ""
            Dim ServiceName As String = ""
            Dim LAServiceId As String = ""
            Dim isAdd As String = ""
            Dim mode As String = ""
            Dim Recurrence As String = ""
            Dim ScheduleTime As String = ""
            Dim WeekInterval As String = ""
            Dim MonthInterval As String = ""
            Dim YearInterval1 As String = ""
            Dim YearInterval2 As String = ""
            Dim AlertsList As String = ""
            Dim EmailList As String = ""
            Dim PhNoList As String = ""
            Dim SortAlerts As String = ""
            Dim EndDate As String = ""
            Dim AlertIds As String = ""
            Dim sortPO As String = ""
            Dim PONo As String = ""
            Dim ModelItem As String = ""
            Dim mapType As String = ""
            Dim mapName As String = ""
            Dim mapDesc As String = ""
            Dim mapId As String = ""
            Dim mapBuildingId As String = ""
            Dim mapFloorId As String = ""
            Dim mapUnitId As String = ""
            Dim isDelete As String = ""
            Dim TagMetaIds As String = ""

            Dim Tagids As String = ""
            Dim CurrentRoom As String = ""
            Dim LastRoom As String = ""
            Dim PageSize As String = ""
            Dim isLiveData As String = ""
            Dim LastFetchedDateTime As String = ""

            Dim EnteredOnFromDate As String = ""
            Dim EnteredOnToDate As String = ""
            Dim LeftOnFromDate As String = ""
            Dim LeftOnToDate As String = ""

            Dim sortTMI As String = ""
            Dim TMIPgSize As String = ""

            Dim MetaInfoForFloorId As String = ""
            Dim LocationEventSort As String = ""

            Dim MapMonitorId As String = ""
            Dim Location As String = ""
            Dim Notes As String = ""
            Dim isHallway As String = ""
            Dim monitorX As String = ""
            Dim monitorY As String = ""
            Dim monitorW As String = ""
            Dim monitorH As String = ""
            Dim roomX As String = ""
            Dim roomY As String = ""
            Dim roomW As String = ""
            Dim roomH As String = ""
            Dim OldPassword As String = ""
            Dim NewPassWord As String = ""
            Dim UserId As String = ""
            Dim SearchId As String = ""

            Dim FilterType As String = ""
            Dim CndType As String = ""
            Dim Filter1 As String = ""
            Dim Filter2 As String = ""
            Dim CampusId As String = ""
            Dim BuildingId As String = ""
            Dim mapReportType As String = ""

            Dim AlertType As String = ""
            Dim Status As String = ""
            Dim EmailDataId As String
            Dim EmailAlertList As String = ""

            Dim IsIE As String = "0"
            Dim ACampusId As String = ""
            Dim ABuildingId As String = ""
            Dim AFloorId As String = ""
            Dim AUnitId As String = ""
            Dim ADeviceId As String = ""
            Dim ASortColumn As String = ""
            Dim ASortOrder As String = ""

            Dim CompanyId As String = ""
            Dim AuthUserId As String = ""
            Dim AuthPassword As String = ""
            CompanyId = Request.QueryString("CompanyId")
            AuthUserId = Request.QueryString("AuthUserId")
            AuthPassword = Request.QueryString("AuthPassword")

            Dim SiteFolder As String = ""
            Dim FileFormat As String = ""
            Dim GMTOffset As String = ""
            Dim IsGroup As String = ""
            Dim IsGroupMember As String = ""
            Dim ServerIP As String = ""
            Dim DatabaseName As String = ""
            Dim Chanel As String = ""
            Dim LocationCode As String = ""
            Dim QBN As String = ""

            Dim tagid As String = Request.QueryString("Tag")
            Dim ReDate As String = Request.QueryString("RDate")
            Dim comments As String = Request.QueryString("comments")
            Dim ListForAdmin As Integer = Val(Request.QueryString("ListForAdmin"))
            Dim SiteName As String = Request.QueryString("SiteName")


            Dim polygonPoints As String = ""
            polygonPoints = Request.QueryString("polygonPoints")
            Dim uMode As String = ""
            uMode = Request.QueryString("uMode")
            Dim dsvgId As String = ""
            dsvgId = Request.QueryString("dsvgId")
            Dim svgDType As String = ""
            svgDType = Request.QueryString("svgDType")
            Dim oldDeviceId As String = ""
            oldDeviceId = Request.QueryString("oldDeviceId")
            Dim UpdatedOn As String

            Tagids = Request.QueryString("Tagids")
            CurrentRoom = Request.QueryString("CurrentRoom")
            LastRoom = Request.QueryString("LastRoom")
            PageSize = Request.QueryString("PageSize")
            isLiveData = Request.QueryString("isLiveData")
            LastFetchedDateTime = Request.QueryString("LastFetchedDateTime")
            EnteredOnFromDate = Request.QueryString("EnteredOnFromDate")
            EnteredOnToDate = Request.QueryString("EnteredOnToDate")
            LeftOnFromDate = Request.QueryString("LeftOnFromDate")
            LeftOnToDate = Request.QueryString("LeftOnToDate")
            LocationEventSort = Request.QueryString("LocationEventSort")

            Response.CacheControl = "no-cache"
            sCmd = Request.QueryString("cmd")
            sid = Request.QueryString("sid")
            UserId = Request.QueryString("UserId")
            alertId = Request.QueryString("alertId")
            curpage = Request.QueryString("curpage")
            typId = Request.QueryString("typId")

            sorColumnname = Request.QueryString("sorColumnname")
            SorOrder = Request.QueryString("SorOrder")
            UpdatedOn = Request.QueryString("UpdatedOn")

            If typId = "" Or IsNothing(typId) Then
                orgtypeId = 0
            Else
                orgtypeId = typId
            End If

            UserName = Request.QueryString("Un")
            Password = Request.QueryString("Pw")
            DeviceId = Request.QueryString("DeviceId")
            Bin = Request.QueryString("Bin")
            DeviceType = Request.QueryString("DeviceType")
            Type = Request.QueryString("Type")
            Duration = Request.QueryString("Duration")
            FilterCriteria = Request.QueryString("qFilterCriteria")
            PageNo = Val(Request.QueryString("PageNo"))
            EmailId = Request.QueryString("EmailId")
            FromDate = Request.QueryString("FromDate")
            ToDate = Request.QueryString("ToDate")
            isExport = CBool(Val(Request.QueryString("isExportSearch")))

            siteId = Request.QueryString("Site")
            sTestbedAction = Request.QueryString("Action")
            siteUrl = ConfigurationManager.AppSettings("GMSAPI_New.GMSAPI_New").ToString()
            ServiceId = Request.QueryString("ServiceId")
            DateRngType = Request.QueryString("DateRngType")
            StartDate = Request.QueryString("StartDate")
            PgType = Request.QueryString("PgType")
            AlertGraphType = Val(Request.QueryString("AlertGraphType"))
            ServiceName = Request.QueryString("ServiceName")
            LAServiceId = Request.QueryString("LAServiceId")
            isAdd = Request.QueryString("isAdd")
            mode = Request.QueryString("mode")

            Recurrence = Request.QueryString("Recurrence")
            ScheduleTime = Request.QueryString("ScheduleTime")
            WeekInterval = Request.QueryString("WeekInterval")
            MonthInterval = Request.QueryString("MonthInterval")
            YearInterval1 = Request.QueryString("YearInterval1")
            YearInterval2 = Request.QueryString("YearInterval2")
            AlertsList = Request.QueryString("AlertList")
            EmailList = Request.QueryString("EmailList")
            PhNoList = Request.QueryString("PhNoList")

            SortAlerts = Request.QueryString("SortAlerts")
            EndDate = Request.QueryString("EndDate")
            AlertIds = Request.QueryString("AlertIds")
            sortPO = Request.QueryString("sortPO")
            PONo = Request.QueryString("PONo")
            ModelItem = Request.QueryString("ModelItem")

            mapType = Val(Request.QueryString("mapType"))
            mapName = Request.QueryString("mapName")
            mapDesc = Request.QueryString("mapDesc")
            mapId = Val(Request.QueryString("mapId"))
            mapBuildingId = Val(Request.QueryString("mapBuildingId"))
            mapFloorId = Val(Request.QueryString("mapFloorId"))
            mapUnitId = Val(Request.QueryString("mapUnitId"))
            isDelete = Val(Request.QueryString("isDelete"))
            TagMetaIds = Request.QueryString("TagMetaIds")

            sortTMI = Request.QueryString("sortTMI")
            TMIPgSize = Request.QueryString("TMIPgSize")

            MetaInfoForFloorId = Request.QueryString("FloorId")

            MapMonitorId = Request.QueryString("MapMonitorId")
            Location = Request.QueryString("Location")
            Notes = Request.QueryString("Notes")
            isHallway = Request.QueryString("isHallway")
            monitorX = Request.QueryString("monitorX")
            monitorY = Request.QueryString("monitorY")
            monitorW = Request.QueryString("monitorW")
            monitorH = Request.QueryString("monitorH")
            roomX = Request.QueryString("roomX")
            roomY = Request.QueryString("roomY")
            roomW = Request.QueryString("roomW")
            roomH = Request.QueryString("roomH")
            OldPassword = Request.QueryString("OldPassword")
            NewPassWord = Request.QueryString("NewPassWord")
            SearchId = Request.QueryString("SearchId")

            FilterType = Request.QueryString("FilterType")
            CndType = Request.QueryString("CndType")
            Filter1 = Request.QueryString("Filter1")
            Filter2 = Request.QueryString("Filter2")
            CampusId = Request.QueryString("CampusId")
            BuildingId = Request.QueryString("BuildingId")
            mapReportType = Request.QueryString("mapReportType")

            AlertType = Request.QueryString("AlertType")
            Status = Request.QueryString("Status")
            EmailDataId = Request.QueryString("EmailDataId")
            EmailAlertList = Request.QueryString("AlertList")

            IsIE = Request.QueryString("IsIE")
            ACampusId = Request.QueryString("campusid")
            ABuildingId = Request.QueryString("buildid")
            AFloorId = Request.QueryString("floorid")
            AUnitId = Request.QueryString("unitid")
            ADeviceId = Request.QueryString("deviceid")
            ASortColumn = Request.QueryString("sortcolumn")
            ASortOrder = Request.QueryString("sortorder")

            Masterid = Request.QueryString("Masterid")
            SiteFolder = Request.QueryString("SiteFolder")
            FileFormat = Request.QueryString("FileFormat")
            GMTOffset = Request.QueryString("GMTOffset")
            IsGroup = Request.QueryString("IsGroup")
            IsGroupMember = Request.QueryString("IsGroupMember")
            ServerIP = Request.QueryString("ServerIP")
            DatabaseName = Request.QueryString("DatabaseName")
            Chanel = Request.QueryString("Chanel")
            LocationCode = Request.QueryString("LocationCode")
            QBN = Request.QueryString("QBN")

            If (curpage = "") Then
                curpage = 1
            End If

            If (typId = "" Or typId = "Nothing") Then
                typId = ""
            End If

            If (IsNothing(DeviceId)) Then
                DeviceId = ""
            End If

            If sCmd = "TagListSearch" Or sCmd = "InfraListSearch" Or sCmd = "StarListSearch" Then

                If sCmd = "TagListSearch" Then
                    sCmd = "TagList"
                ElseIf sCmd = "InfraListSearch" Then
                    sCmd = "InfraList"
                ElseIf sCmd = "StarListSearch" Then
                    sCmd = "StarList"
                End If

                sid = Request.Form("sid")
                typId = Request.Form("typId")
                orgtypeId = Request.Form("orgtypeId")
                DeviceId = Request.Form("DeviceId")
                curpage = Request.Form("curpage")
                alertId = Request.Form("alertId")
                Bin = Request.Form("Bin")
                sorColumnname = Request.Form("sorColumnname")
                SorOrder = Request.Form("SorOrder")

            End If

            '''''check authorization user role'''''
            If sCmd = "CompanyList" Or sCmd = "AddCompanySetup" Or sCmd = "SiteList" Or sCmd = "AddSiteSetup" Or sCmd = "UserList" Or sCmd = "AddUserSetup" Or sCmd = "DeleteUserConfiguration" Or sCmd = "ChangePassword" Or sCmd = "GetMangeRolesInfo" Or sCmd = "UpdateMangeRoles" Or sCmd = "UpdateAnnouncements" Or sCmd = "ConfigADDirectory" Or sCmd = "GetCompanyGroup" Or sCmd = "AddCompanyGroup" Or sCmd = "DeleteCompanyGroup" Or sCmd = "GetSiteADGroup" Or sCmd = "AddAssociationSiteSetup" Or sCmd = "DeleteSiteAssociation" Then

                If g_UserRole <> enumUserRole.Admin Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

            End If

            If sCmd = "UpdateBattery" Or sCmd = "GetLBIListForBatteryTech" Or sCmd = "LoadExcelForBatteryTech" Or sCmd = "LoadExcelForBatteryTech_IE" Or sCmd = "LoadExcelReportForBatteryTech" Or sCmd = "LoadExcelReportForBatteryTech_IE" Then

                If g_UserRole <> enumUserRole.BatteryTech Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

            End If

            If sCmd = "EmailList" Or sCmd = "AddEmailSetup" Or sCmd = "GetAvailableAlertsForEmail" Or sCmd = "EditAlertsForEmail" Then

                If Not (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.MaintenancePrism Or g_UserRole = enumUserRole.TechnicalAdmin) Then
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

            End If

            Select Case sCmd
                Case "LoadUpdates"

                    Dim alertList As New DataTable

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    If g_UserRole <> enumUserRole.Customer Then
                        alertList = Loadalertinfo(sid, sCompanys, sSites)
                    End If

                    SendXMLToAJAXCall(alertList)

                Case "SiteOverview"

                    Dim dtSiteOverview As New DataTable
                    Dim dtOverview As New DataTable

                    dtSiteOverview = LoadsiteOverview(sid)

                    dtOverview = CreateSiteOverview(dtSiteOverview)

                    SendXMLToAJAXCall(dtOverview)

                Case "TagList"

                    Dim dt As New DataTable
                    Dim strDeviceids As String = ""

                    If orgtypeId = "" Or IsNothing(orgtypeId) Then
                        orgtypeId = 0
                    End If

                    strDeviceids = GetDeviceIds(DeviceId)

                    dt = LoadDeviceList(sid, "1", typId, strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)

                    FillNIStCertificateInfo(dt)

                    SendXMLToAJAXCall(dt)

                Case "InfraList"

                    Dim dt As New DataTable
                    Dim strDeviceids As String = ""

                    strDeviceids = GetDeviceIds(DeviceId)

                    dt = LoadDeviceList(sid, "2", typId, strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)

                    SendXMLToAJAXCall(dt)

                Case "StarList"

                    Dim dt As New DataTable
                    Dim strDeviceids As String = ""

                    strDeviceids = GetDeviceIds(DeviceId)

                    dt = LoadDeviceList(sid, "3", typId, strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)

                    SendXMLToAJAXCall(dt)

                Case "DownloadExcel"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    Dim strDeviceids As String = ""
                    Dim UIId As Integer

                    sid = Request.Form("sid")
                    DeviceType = Request.Form("devicetype")
                    typId = Request.Form("typId")
                    DeviceId = Request.Form("DeviceId")
                    curpage = Request.Form("curpage")
                    alertId = Request.Form("alertId")
                    Bin = Request.Form("Bin")
                    IsIE = Request.Form("IsIE")
                    UIId = Val(Request.Form("UIId"))

                    If (typId = "") Then
                        typId = "0"
                    End If

                    strDeviceids = GetDeviceIds(DeviceId)

                    If strDeviceids.Trim = "" Then
                        dt = LoadDeviceList(sid, DeviceType, typId, strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)
                    Else
                        dt = LoadDeviceList(sid, DeviceType, "0", strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)
                    End If

                    dtReturn = MakeCSV(dt, DeviceType, Bin, typId, , UIId)

                    SendXMLToAJAXCall(dtReturn)

                Case "DownloadExcel_ForIE"

                    Dim dt As New DataTable
                    Dim strDeviceids As String = ""
                    Dim UIId As Integer
                    UIId = Val(Request.Form("UIId"))

                    If (typId = "") Then
                        typId = "0"
                    End If

                    strDeviceids = GetDeviceIds(DeviceId)

                    If strDeviceids.Trim = "" Then
                        dt = LoadDeviceList(sid, DeviceType, typId, strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)
                    Else
                        dt = LoadDeviceList(sid, DeviceType, "0", strDeviceids, curpage, alertId, Bin, sorColumnname, SorOrder)
                    End If

                    MakeCSV_IE(dt, DeviceType, Bin, CInt(typId), , UIId)

                Case "DeviceSearch"

                    Dim ssiteId As String = Request.Form("Site")
                    Dim sDeviceType As String = Request.Form("DeviceType")
                    Dim sDeviceId As String = Request.Form("DeviceId")
                    Dim sFilterCriteria As String = Request.Form("qFilterCriteria")
                    Dim sPageNo As String = Request.Form("PageNo") '
                    Dim sisExport As String = CBool(Val(Request.Form("isExportSearch")))

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    Dim xmlElet As XmlElement = GetDeviceSearchResult(ssiteId, sDeviceType, GetDeviceList(sDeviceType, sDeviceId), sPageNo, sFilterCriteria, sCompanys, sSites)

                    If sDeviceType = enumDeviceType.Tag Then
                        dtResp = LoadTagInfoXMLintoTable(xmlElet, True)
                    ElseIf sDeviceType = enumDeviceType.Monitor Then
                        dtResp = LoadMonitorInfoXMLintoTable(xmlElet, True)
                    ElseIf sDeviceType = enumDeviceType.Star Then
                        dtResp = LoadStarInfoXMLintoTable(xmlElet, True)
                    End If

                    If sisExport Then
                        dtResp = MakeCSV_GlobalSearch(dtResp, sDeviceType)
                    End If

                    SendXMLToAJAXCall(dtResp)

                Case "DeviceSearch_IE"

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    Dim xmlElet As XmlElement = GetDeviceSearchResult(siteId, DeviceType, GetDeviceList(DeviceType, DeviceId), PageNo, FilterCriteria, "", "")

                    If DeviceType = enumDeviceType.Tag Then
                        dtResp = LoadTagInfoXMLintoTable(xmlElet, True)
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        dtResp = LoadMonitorInfoXMLintoTable(xmlElet, True)
                    ElseIf DeviceType = enumDeviceType.Star Then
                        dtResp = LoadStarInfoXMLintoTable(xmlElet, True)
                    End If

                    MakeCSV_IE_GlobalSearch(dtResp, DeviceType)

                Case "DeviceProfile"

                    Dim dtProfile As New DataTable

                    dtProfile = Load_DeviceProfile_List(sid, DeviceType, DeviceId)

                    SendXMLToAJAXCall(dtProfile)

                Case "Device_GetPhoto"

                    Dim Photolist As New DataTable
                    Dim strDeviceids As String = ""

                    strDeviceids = GetDeviceIds(DeviceId)

                    Photolist = LoadDevicePhotoList(sid, strDeviceids, "1")

                    SendXMLToAJAXCall(Photolist)

                Case "Device_DeletePhoto"

                    Dim DataId As String = ""

                    DataId = Request.QueryString("DataId")

                    Dim API As New GmsAPICall

                    API.DeleteDevicePhotoList(DataId, sid, DeviceId, "1")

                Case "EMTagReport"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    sid = Request.Form("sid")

                    dt = LoadEmTags(sid)

                    dtReturn = MakeCSVforEMTAG(dt, Val(sid))

                    SendXMLToAJAXCall(dtReturn)

                Case "EMTagDetailReport"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    dt = LoadEMTagDetail(Request.Form("sid"), Request.Form("EMReportType"))

                    dtReturn = MakeCSVforEMTagDetail(dt, Request.Form("EMReportType"))

                    SendXMLToAJAXCall(dtReturn)

                Case "AlertList"

                    Dim dtAlerts As New DataTable

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    dtAlerts = Load_Alert_site_list(sid, sCompanys, sSites)
                    SendXMLToAJAXCall(dtAlerts)

                Case "DeviceList"

                    Dim dtActiveList As New DataTable
                    Dim dtProfile As New DataTable
                    Dim dtDeviceList As New DataTable

                    Dim dSet As New DataSet

                    Dim orgProtypeId As String = Request.QueryString("orgtypeId")

                    dSet = LoadDeviceDetails(sid, DeviceType, "", DeviceId, "", "", "", "", "")

                    dtActiveList = dSet.Tables(1)

                    dtProfile = dSet.Tables(2)

                    dtDeviceList = DeviceInfo(dtActiveList, dtProfile)

                    SendXMLToAJAXCall(dtDeviceList)

                Case "DeviceGraph"

                    Dim dtGraph As New DataTable
                    Dim fDate As String = ""

                    If PgType = PREV_PAGE Then
                        If DateRngType = HOURLY Then
                            fDate = CDate(FromDate).AddSeconds(-1)
                        ElseIf DateRngType = WEEKLY Then
                            fDate = CDate(FromDate).AddDays(-7)
                        ElseIf DateRngType = MONTHLY Then
                            fDate = CDate(FromDate).AddMonths(-1).AddDays(-1)
                        End If
                    End If

                    If PgType = NEXT_PAGE Then
                        If DateRngType = HOURLY Then
                            fDate = CDate(FromDate).AddHours(1)
                        ElseIf DateRngType = WEEKLY Then
                            fDate = CDate(FromDate).AddDays(140)
                        ElseIf DateRngType = MONTHLY Then
                            fDate = CDate(FromDate).AddMonths(22).AddDays(-1)
                        End If
                    End If

                    dtGraph = Load_DeviceActivity_List(sid, DeviceType, DeviceId, DateRngType, fDate)

                    SendXMLToAJAXCall(dtGraph)

                Case "Last10hrdata"

                    Dim dt10hrdata As New DataTable

                    dt10hrdata = Load_10hrdata(sid, DeviceType, DeviceId)

                    SendXMLToAJAXCall(dt10hrdata)

                Case "AddEmailSetup"

                    Dim dtEmail As New DataTable

                    sid = Request.Form("sid")
                    EmailId = Request.Form("EmailId")
                    AlertType = Request.Form("AlertType")
                    Status = Request.Form("Status")
                    isAdd = Request.Form("IsAdd")
                    EmailDataId = Request.Form("EmailDataId")

                    dtEmail = AddEmail(sid, EmailId, AlertType, Status, isAdd, EmailDataId)
                    SendXMLToAJAXCall(dtEmail)

                Case "AddEditEmailWithAlertsForSite"

                    Dim dtEmail As New DataTable

                    dtEmail = AddEditEmailWithAlertsForSite(sid, EmailId, AlertType, Status, EmailAlertList, EmailDataId)

                    SendXMLToAJAXCall(dtEmail)

                Case "EmailList"

                    Dim dtEmail As New DataTable

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    dtEmail = ListEmail(sid, curpage, sCompanys, sSites)

                    SendXMLToAJAXCall(dtEmail)

                Case "TagMovements"

                    Dim dtTag As New DataTable
                    Dim dtChart As New DataTable

                    dtTag = TagMovementsList(siteId, DeviceId, FromDate, ToDate)
                    dtChart = MakeTagMovementChartContent(dtTag, FromDate, ToDate)

                    SendXMLToAJAXCall(dtTag, dtChart)

                Case "HeartBeatContent"

                    Dim dtHeartBeatContent As New DataTable
                    dtHeartBeatContent = HeartBeatContentList(siteId)

                    SendXMLToAJAXCall(dtHeartBeatContent)

                Case "AlertHistoryForService"

                    Dim dtAlertHistory As New DataTable
                    Dim dtChart As New DataTable

                    dtAlertHistory = AlertHistoryForService(siteId, "", "", ServiceId)
                    dtChart = MakeAlertHistoryChartContent(dtAlertHistory)

                    SendXMLToAJAXCall(dtAlertHistory, dtChart)

                Case "LocalAlertsForSite"

                    Dim dtLAAlerts As New DataTable
                    Dim dtChart As New DataTable

                    Dim fDate As String = ""
                    Dim tDate As String = ""

                    If StartDate = "" Then
                        StartDate = Now
                    End If

                    If PgType = CURR_PAGE Then
                        If DateRngType = HOURLY Then
                            fDate = CDate(StartDate).Date.ToString()
                            tDate = CDate(fDate).AddDays(1).AddSeconds(-1).ToString()
                        ElseIf DateRngType = DAILY Then
                            fDate = CDate(StartDate).AddDays(-4).ToString()
                            tDate = CDate(StartDate).AddDays(1).AddSeconds(-1).ToString()
                        ElseIf DateRngType = WEEKLY Then
                            fDate = CDate(StartDate).AddDays(-CDate(StartDate).DayOfWeek).ToString()
                            tDate = CDate(fDate).AddDays(6).ToString()
                            fDate = CDate(tDate).AddDays(-34).ToString()
                        ElseIf DateRngType = MONTHLY Then
                            fDate = CDate(Month(StartDate) & "/01/" & Year(StartDate)).ToString()
                            tDate = CDate(fDate).AddMonths(1).AddDays(-1).ToString()
                            fDate = CDate(tDate).AddMonths(-5).AddDays(1).ToString()
                        End If
                    End If

                    If PgType = NEXT_PAGE Then
                        If DateRngType = HOURLY Then
                            fDate = CDate(StartDate).AddDays(1).Date.ToString()
                            tDate = CDate(fDate).AddDays(1).AddSeconds(-1).ToString()
                        ElseIf DateRngType = DAILY Then
                            fDate = CDate(StartDate).AddDays(5).ToString()
                            tDate = CDate(fDate).AddDays(5).AddSeconds(-1).ToString()
                        ElseIf DateRngType = WEEKLY Then
                            fDate = CDate(StartDate).AddDays(35).ToString()
                            tDate = CDate(fDate).AddDays(34).ToString()
                        ElseIf DateRngType = MONTHLY Then
                            fDate = CDate(StartDate).AddMonths(5).ToString()
                            tDate = CDate(fDate).AddMonths(4).ToString()
                        End If
                    End If

                    If PgType = PREV_PAGE Then
                        If DateRngType = HOURLY Then
                            fDate = CDate(StartDate).AddDays(-1).Date.ToString()
                            tDate = CDate(fDate).AddDays(1).AddSeconds(-1).ToString()
                        ElseIf DateRngType = DAILY Then
                            tDate = CDate(StartDate).AddSeconds(-1).ToString()
                            fDate = CDate(StartDate).AddDays(-5).ToString()
                        ElseIf DateRngType = WEEKLY Then
                            tDate = CDate(StartDate).AddDays(-7).ToString()
                            fDate = CDate(tDate).AddDays(-28).ToString()
                        ElseIf DateRngType = MONTHLY Then
                            tDate = CDate(StartDate).AddMonths(-1).ToString()
                            fDate = CDate(tDate).AddMonths(-4).ToString()
                        End If
                    End If

                    If LAServiceId <> "" Then
                        dtLAAlerts = AlertHistoryForService(siteId, CDate(fDate).ToString("yyyy-MM-dd HH:mm:ss"), CDate(tDate).ToString("yyyy-MM-dd HH:mm:ss"), LAServiceId)
                        dtChart = MakeLocalAlertForServiceChartContent(dtLAAlerts, fDate, tDate, DateRngType, AlertGraphType, ServiceName, LAServiceId)
                    Else
                        dtLAAlerts = LocalAlertForSite(siteId, CDate(fDate).ToString("yyyy-MM-dd HH:mm:ss"), CDate(tDate).ToString("yyyy-MM-dd HH:mm:ss"), DeviceId, AlertGraphType, "")
                        dtChart = MakeLocalAlertForSiteChartContent(dtLAAlerts, fDate, tDate, DateRngType, AlertGraphType, ServiceName)
                    End If

                    SendXMLToAJAXCall(dtLAAlerts, dtChart)

                Case "LoadAlertsServiceTableView"

                    Dim dtReport As New DataTable
                    If AlertIds = "null" Then
                        AlertIds = ""
                    End If

                    dtReport = LoadLocalAlertsForTableView(siteId, curpage, SortAlerts, StartDate, EndDate, AlertIds, DeviceId)
                    SendXMLToAJAXCall(dtReport)

                Case "DeleteEmailConfiguration"

                    Dim dtEmail As New DataTable

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    sid = Request.Form("sid")
                    EmailId = Request.Form("EmailId")
                    curpage = Request.Form("curpage")

                    dtEmail = DeleteEmailConfiguration(sid, EmailId, curpage, sCompanys, sSites)
                    SendXMLToAJAXCall(dtEmail)

                Case "GetAvailableAlertsForEmail"

                    Dim dtEmail As New DataTable

                    sid = Request.Form("sid")
                    EmailId = Request.Form("EmailId")

                    dtEmail = GetAvailableAlertsForEmail(sid, EmailId)
                    SendXMLToAJAXCall(dtEmail)

                Case "GetSchuledReports"

                    Dim dtEmail As New DataTable

                    dtEmail = GetSchuledReports(EmailId)

                    SendXMLToAJAXCall(dtEmail)

                Case "EditAlertsForEmail"

                    Dim dtEmail As New DataTable

                    sid = Request.Form("sid")
                    EmailId = Request.Form("EmailId")
                    alertId = Request.Form("alertId")
                    isAdd = Request.Form("isAdd")
                    mode = Request.Form("mode")

                    dtEmail = EditAlertsForEmail(sid, EmailId, alertId, isAdd, mode, AlertType)
                    SendXMLToAJAXCall(dtEmail)

                Case "SaveAlertContent"

                    Dim ResponseError As String = ""

                    Dim dtReturn As New DataTable

                    dtReturn = UpdateAlertSettings(siteId, AlertsList, EmailList, PhNoList, AlertType)

                    If Not dtReturn Is Nothing Then
                        If dtReturn.Rows.Count > 0 Then
                            If dtReturn.Rows(0).Item("Result") = "1" Then
                                ResponseError = dtReturn.Rows(0).Item("Error")
                            Else
                                ResponseError = ""
                            End If
                        End If
                    End If

                    Response.Clear()
                    Response.ContentType = "text/xml"
                    Response.CacheControl = "no-cache"
                    Response.Write(ResponseError)
                    Response.End()

                Case "GetAlertContent"

                    Dim dtReport As New DataTable

                    dtReport = GetAlertSettings(siteId)

                    SendXMLToAJAXCall(dtReport)

                Case "DeviceListPrintPage"

                    Dim dtTag As New DataTable
                    Dim dtMonitor As New DataTable
                    Dim dtStar As New DataTable

                    LoadDeviceListPrintPage(dtTag, dtMonitor, sid, "0", typId, DeviceId, curpage, alertId, Bin, sorColumnname, SorOrder)

                    SendXMLToAJAXCall(dtTag, dtMonitor)

                Case "DownloadExcel_All"

                    Dim dtTag As New DataTable
                    Dim dtMonitor As New DataTable
                    Dim dtReturn As New DataTable
                    Dim bNoRecordFound As Boolean = False

                    LoadDeviceListPrintPage(dtTag, dtMonitor, sid, "0", "", "", "0", "", Bin, "", "", "", bNoRecordFound)

                    If bNoRecordFound Then
                        dtReturn = MakeCSVforNoRecordFound(dtTag)
                    Else
                        dtReturn = MakeCSV(dtTag, 0, Bin, typId, dtMonitor)
                    End If

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadPurchaseOrderView"

                    Dim dtReturn As New DataTable

                    dtReturn = LoadPurchaseOrderView(siteId, curpage, sortPO, PONo, ModelItem)

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadPurchaseDetailsView"

                    Dim dtReturn As New DataTable

                    dtReturn = LoadPurchaseDetailsView(PONo, "")

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadPurchaseSummaryView"

                    Dim dtReturn As New DataTable

                    dtReturn = LoadPurchaseSummaryView(siteId, "")

                    SendXMLToAJAXCall(dtReturn)

                Case "LocationChageEvent"

                    Dim dtReturn As New DataTable

                    dtReturn = LocationChageEvent(sid, Tagids, CurrentRoom, LastRoom, PageSize, curpage, isLiveData, LastFetchedDateTime, "", LocationEventSort, EnteredOnFromDate, EnteredOnToDate, LeftOnFromDate, LeftOnToDate)

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadMapListView"

                    Dim dsReturn As New DataSet

                    dsReturn = LoadMapListView(siteId)

                    SendXMLToAJAXCall(dsReturn)

                Case "LoadFiltersforMap"

                    Dim dt As New DataTable

                    dt = SearchXMLNodeToDataTable(GetSearchResult("0"))

                    dt.TableName = "FilterforMap"

                    SendXMLToAJAXCall(dt)

                Case "AddEditDeleteMap"

                    Dim bFlag As Boolean = False
                    Dim dtReturn As New DataTable
                    Dim ResponseError As String = "Error"

                    mapType = Request.Form("mapType")
                    siteId = Request.Form("Site")
                    mapName = Request.Form("mapName")
                    mapDesc = Request.Form("mapDesc")
                    mapId = Request.Form("mapId")
                    mapBuildingId = Request.Form("mapBuildingId")
                    mapFloorId = Request.Form("mapFloorId")
                    mapUnitId = Request.Form("mapUnitId")
                    isDelete = Request.Form("isDelete")

                    Dim floorSqft As String = Request.Form("floorSqft")
                    Dim Widthft As String = Request.Form("Widthft")

                    dtReturn = AddEditDeleteMap(mapType, siteId, mapName, mapDesc, mapId, mapBuildingId, mapFloorId, mapUnitId, isDelete, floorSqft, Widthft)
                    SendXMLToAJAXCall(dtReturn)

                Case "uploadFile"

                    Dim bFlag As Boolean = False
                    Dim hdnMapCmd As String = Request.Form("hdnMapCmd")

                    If hdnMapCmd = "UploadFile" Then

                        Dim file As HttpPostedFile = Request.Files("photoimg")
                        Dim bgfile As HttpPostedFile = Request.Files("bgimg")

                        Dim hdnsite As String = Request.Form("hdnMapSiteId")
                        Dim hdnsource As String = Request.Form("hdnSource")
                        Dim hdnSourceType As String = Request.Form("hdnSourceType")
                        Dim isSVGEdit As String = Request.Form("hdnSVGisEdit")
                        Dim isBGEdit As String = Request.Form("hdnBGisEdit")

                        hdnSourceType = hdnSourceType.Replace(",", "")

                        If hdnSourceType = "7" Then 'svg
                            bFlag = UploadMetaFile(hdnsite, ReadFile(file), file.FileName, hdnsource, hdnSourceType, isSVGEdit)
                        ElseIf hdnSourceType = "8" Then
                            bFlag = UploadMetaFile(hdnsite, ReadFile(bgfile), bgfile.FileName, hdnsource, hdnSourceType, isBGEdit)
                        End If

                    End If

                Case "LoadRoomInfo"

                    Dim dtReturn As New DataTable

                    dtReturn = LoadRoomListView(siteId, mapUnitId, ASortColumn, ASortOrder)

                    SendXMLToAJAXCall(dtReturn)

                Case "TagMetaInfo"

                    Dim dtReturn As New DataTable

                    dtReturn = LoadTagListView(siteId, TMIPgSize, curpage, TagMetaIds, sortTMI)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetInfrastructureMetaInfoForFloor"

                    Dim dtReturn As New DataSet

                    dtReturn = LoadInfrastructureMetaInfoForFloor(siteId, MetaInfoForFloorId)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetTagMetaInfoForFloor"

                    Dim dtReturn As New DataTable

                    dtReturn = LoadTagMetaInfoForFloor(siteId, MetaInfoForFloorId)

                    SendXMLToAJAXCall(dtReturn)

                Case "UpdateAnnouncements"

                    Dim dtReturn As New DataTable

                    Dim Message As String = ""
                    Dim UserRoles As String = ""
                    Dim AnnouncementId As String = ""
                    Dim IsShow As Integer = 0
                    Dim IsLive As Integer = 0
                    Dim HTMLMessage As String = ""
                    Dim IsActive As Integer = 0
                    Dim ShowIn As Integer = 0
                    Dim aStartDate As String
                    Dim aEndDate As String
                    Dim UserAssociatedViews As String = ""
                    Dim IsDispAfterExpire As Integer = 0
                    Dim daysDispAfterExpire As Integer = 0
                    Dim IsDispHistory As Integer = 0

                    ''Announcements
                    Message = Request.Form("Message")
                    UserRoles = Request.Form("UserRoles")
                    IsShow = Val(Request.Form("IsShow"))
                    AnnouncementId = Request.Form("AnnouncementId")
                    IsLive = Val(Request.Form("IsLive"))
                    HTMLMessage = Request.Form("HtmlMsg")
                    IsActive = Val(Request.Form("IsActive"))
                    ShowIn = Request.Form("ShowIn")
                    aStartDate = Request.Form("StartDate")
                    aEndDate = Request.Form("EndDate")
                    UserAssociatedViews = Request.Form("UserAssociatedViews")
                    IsDispAfterExpire = Val(Request.Form("IsDispAfterExpire"))
                    daysDispAfterExpire = Val(Request.Form("daysDispAfterExpire"))
                    IsDispHistory = Val(Request.Form("IsDispHistory"))
                    Dim html = Server.UrlDecode(HTMLMessage)

                    dtReturn = UpdateAnnouncements(AnnouncementId, Message, aStartDate, aEndDate, IsShow, UserRoles, UserAssociatedViews, html, IsActive, ShowIn, IsDispAfterExpire, daysDispAfterExpire, IsDispHistory)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetAnnouncements"

                    Dim dsReturn As New DataSet
                    Dim IsLive As Integer = 0

                    IsLive = Val(Request.QueryString("IsLive"))

                    dsReturn = GetAnnouncements("", "", IsLive, curpage, ListForAdmin)

                    SendXMLToAJAXCall(dsReturn)

                Case "GetPastAnnouncements"

                    Dim dsReturn As New DataSet
                    Dim IsLive As Integer = 0

                    IsLive = Val(Request.QueryString("IsLive"))
                    dsReturn = GetPastAnnouncements()

                    SendXMLToAJAXCall(dsReturn)

                Case "AddThresHoldTime"

                    Dim dsReturn As New DataTable
                    Dim TTime As String = ""

                    TTime = Request.QueryString("TTime")

                    dsReturn = AddThresHoldTime(TTime)

                    SendXMLToAJAXCall(dsReturn)

                Case "GetHTMLAnnouncement"

                    Dim dsReturn As New DataSet
                    Dim AnnouncementId As String = ""

                    AnnouncementId = Request.QueryString("AnnouncementId")

                    dsReturn = GetHTMLAnnouncement(AnnouncementId)

                    SendXMLToAJAXCall(dsReturn)

                Case "SaveMonitor"

                    Dim ResponseError As String = ""
                    Dim dtReturn As New DataTable

                    siteId = Request.Form("Site")
                    MetaInfoForFloorId = Request.Form("FloorId")
                    MapMonitorId = Request.Form("MapMonitorId")
                    Location = Request.Form("Location")
                    Notes = Request.Form("Notes")
                    isHallway = Request.Form("isHallway")
                    monitorX = Request.Form("monitorX")
                    monitorY = Request.Form("monitorY")
                    monitorW = Request.Form("monitorW")
                    monitorH = Request.Form("monitorH")
                    polygonPoints = Request.Form("polygonPoints")
                    uMode = Request.Form("uMode")
                    dsvgId = Request.Form("dsvgId")
                    svgDType = Request.Form("svgDType")
                    oldDeviceId = Request.Form("oldDeviceId")

                    Dim unitName As String = Request.Form("unitName")
                    Dim Xaxis As String = Request.Form("Xaxis")
                    Dim Yaxis As String = Request.Form("Yaxis")
                    Dim RoomXaxis As String = Request.Form("RoomXaxis")
                    Dim RoomYaxis As String = Request.Form("RoomYaxis")
                    Dim WidthFt As String = Request.Form("WidthFt")
                    Dim LengthFt As String = Request.Form("LengthFt")
                    Dim MonitorType As String = Request.Form("MonitorType")

                    dtReturn = SaveMonitor(siteId, MetaInfoForFloorId, MapMonitorId, Location, Notes, isHallway, monitorX, monitorY, monitorW, monitorH, polygonPoints, uMode, dsvgId, svgDType, oldDeviceId, unitName, Xaxis, Yaxis, RoomXaxis, RoomYaxis, WidthFt, LengthFt, MonitorType)
                    SendXMLToAJAXCall(dtReturn)

                Case "UpdateProfiles"

                    Dim ResponseError As String = ""
                    Dim dtReturn As New DataTable
                    Dim isNotSanitized As Boolean = False
                    Dim comparer As CompareInfo = CultureInfo.InvariantCulture.CompareInfo

                    NewPassWord = Request.Form("NewPassWord")
                    OldPassword = Request.Form("OldPassword")

                    For i As Integer = 0 To fromblackList.Length - 1
                        If (comparer.IndexOf(NewPassWord, fromblackList(i), CompareOptions.IgnoreCase) >= 0) Then
                            isNotSanitized = True

                            ResponseError = "Password not sanitized"
                            Response.Clear()
                            Response.ContentType = "text/xml"
                            Response.CacheControl = "no-cache"
                            Response.Write(ResponseError)
                            Response.End()
                            Exit For
                            Exit Sub
                        End If
                    Next

                    If isNotSanitized = False Then
                        dtReturn = UpdateProfiles(OldPassword, NewPassWord)
                        SendXMLToAJAXCall(dtReturn)
                    End If

                Case "SearchDeviceMap"

                    Dim dtReturn As New DataTable

                    dtReturn = SearchDeviceMap(siteId, SearchId.Trim)

                    SendXMLToAJAXCall(dtReturn)

                Case "AddEditDeleteDevice"

                    Dim dtReturn As New DataTable

                    siteId = Request.Form("Site")
                    DeviceType = Request.Form("DeviceType")
                    DeviceId = Request.Form("DeviceId")
                    isDelete = Request.Form("isDelete")

                    If DeviceType = enumDeviceType.Star Then
                        DeviceId = DeviceId.Replace(".", "_").Replace("-", "_").Replace(" ", "_")
                    End If

                    dtReturn = AddEditDeleteDevice(siteId, DeviceType, DeviceId, isDelete)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetReportsForMap"

                    Dim dtReturn As New DataTable

                    dtReturn = GetReportsForMap(siteId, MetaInfoForFloorId, mapReportType)

                    SendXMLToAJAXCall(dtReturn)

                Case "ReportFloorView"

                    Dim dtReturn As New DataTable
                    Dim sFilterCriteria As String = ""

                    If Filter1 <> "" And Filter2 = "" Then
                        sFilterCriteria = FilterType & "~" & CndType & "~" & Filter1 & "~|"
                    ElseIf Filter1 <> "" And Filter2 <> "" Then
                        sFilterCriteria = FilterType & "~" & CndType & "~" & Filter1 & "~" & Filter2 & "|"
                    End If

                    dtReturn = GetReportFloorView(siteId, DeviceType, Bin, CampusId, BuildingId, "", sFilterCriteria)

                    SendXMLToAJAXCall(dtReturn)

                Case "AssetsTagList"

                    Dim dtAssetTagList As New DataTable

                    dtAssetTagList = LoadAssetDeviceList(sid, ACampusId, ABuildingId, AFloorId, AUnitId, ASortColumn, ASortOrder, curpage, ADeviceId)

                    SendXMLToAJAXCall(dtAssetTagList)

                Case "AssetTagDownloadExcel"

                    Dim dtAssetTagList As New DataTable
                    Dim dtReturn As New DataTable

                    dtAssetTagList = LoadExcelAssetReportList(sid, ADeviceId, FromDate, ToDate)

                    If dtAssetTagList.Rows.Count > 0 Then
                        dtReturn = MakeCSVForAssetPage(dtAssetTagList, FromDate, ToDate)
                    Else
                        dtReturn = NoRcordCSVForAssetPage(dtAssetTagList, ADeviceId, FromDate, ToDate)
                    End If

                    SendXMLToAJAXCall(dtReturn)

                Case "AssetTagDownloadExcel_ForIE"

                    Dim Taglist As New DataTable

                    Taglist = LoadExcelAssetReportList(sid, ADeviceId, FromDate, ToDate)

                    If Taglist.Rows.Count > 0 Then
                        AssetTagListPrepareCSVForIE(Taglist, FromDate, FromDate)
                    Else
                        PrepareAssetTagListCSVForNoRecordFound(Taglist, FromDate, ToDate, ADeviceId)
                    End If

                Case "BatterySummary"

                    Dim dtBatterySummaryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatterySummaryList = LoadBatterySummaryList(sid, DeviceType, typId, ACampusId, ABuildingId, AFloorId, AUnitId)

                    SendXMLToAJAXCall(dtBatterySummaryList)

                Case "LoadExcelBatterySummary"

                    Dim dtBatterySummaryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatterySummaryList = LoadBatterySummaryList(sid, DeviceType, typId, ACampusId, ABuildingId, AFloorId, AUnitId)

                    Dim TypeName As String = GetDeviceTypeName(DeviceType, typId)

                    dtReturn = MakeCSVForBatterySummary(dtBatterySummaryList, TypeName)

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadExcelBatterySummary_ForIE"

                    Dim dtBatterySummaryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatterySummaryList = LoadBatterySummaryList(sid, DeviceType, typId, ACampusId, ABuildingId, AFloorId, AUnitId)

                    Dim TypeName As String = GetDeviceTypeName(DeviceType, typId)

                    BatterySummaryPrepareCSVForIE(dtBatterySummaryList, TypeName)

                Case "UpdateBattery"

                    Dim dtUpdateBatteryReplace As New DataTable

                    sid = Request.Form("sid")
                    tagid = Request.Form("Tag")
                    ReDate = Request.Form("RDate")
                    comments = Request.Form("comments")

                    dtUpdateBatteryReplace = UpdateBatteryReplaceForSite(sid, tagid, ReDate, comments)

                    SendXMLToAJAXCall(dtUpdateBatteryReplace)

                Case "GetBatteryList"

                    Dim dtBatterySummaryList As New DataTable

                    dtBatterySummaryList = LoadBatteryList(sid, DeviceType, typId, ACampusId, ABuildingId, AFloorId, AUnitId, curpage, ASortColumn, ASortOrder, DeviceId)

                    SendXMLToAJAXCall(dtBatterySummaryList)

                Case "LoadExcelBatteryList"

                    Dim dtBatterySummaryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatterySummaryList = LoadBatteryList(sid, DeviceType, typId, ACampusId, ABuildingId, AFloorId, AUnitId, curpage, ASortColumn, ASortOrder, DeviceId)

                    Dim TypeName As String = GetDeviceTypeName(DeviceType, typId)

                    dtReturn = MakeCSVForBatteryList(dtBatterySummaryList, TypeName, DeviceType)

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadExcelBatteryListFor_IE"

                    Dim dtBatterySummaryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatterySummaryList = LoadBatteryList(sid, DeviceType, typId, ACampusId, ABuildingId, AFloorId, AUnitId, curpage, ASortColumn, ASortOrder, DeviceId)

                    Dim TypeName As String = GetDeviceTypeName(DeviceType, typId)

                    MakeCSVForBatteryList_IE(dtBatterySummaryList, TypeName, DeviceType)

                Case "GetLBIListForBatteryTech"

                    Dim dtBatteryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatteryList = LoadLbiListForBatteryTech(sid, ASortColumn, ASortOrder)

                    SendXMLToAJAXCall(dtBatteryList)

                Case "LoadExcelForBatteryTech"

                    Dim dtBatteryList As New DataTable
                    Dim dtReturn As New DataTable

                    dtBatteryList = LoadLbiListForBatteryTech(sid, ASortColumn, ASortColumn)

                    If dtBatteryList.Rows.Count > 0 Then
                        dtReturn = PrepareCSVForBatteryTechLbiList(dtBatteryList)
                    Else
                        dtReturn = NoRecordFoundForBatteryTechLbiList()
                    End If

                    SendXMLToAJAXCall(dtReturn)

                Case "LoadExcelForBatteryTech_IE"

                    Dim dtBatteryList As New DataTable

                    dtBatteryList = LoadLbiListForBatteryTech(sid, ASortColumn, ASortColumn)

                    If dtBatteryList.Rows.Count > 0 Then
                        PrepareCSVForBatteryTechLbiListForIE(dtBatteryList)
                    Else
                        NoRecordFoundForBatteryTech_IE()
                    End If

                Case "LoadExcelReportForBatteryTech"

                    Dim dtBatteryList As New DataTable
                    Dim dt As New DataTable

                    dtBatteryList = LoadTagReportForBatteryTech(sid, FromDate, ToDate)

                    If dtBatteryList.Rows.Count > 0 Then
                        dt = MakeCSVReportForBatteryTech(dtBatteryList, FromDate, ToDate)
                    Else
                        dt = NoRecord_ReportForBatteryTech(SiteName, FromDate, ToDate)
                    End If

                    SendXMLToAJAXCall(dt)

                Case "LoadExcelReportForBatteryTech_IE"

                    Dim dtBatteryList As New DataTable
                    Dim dt As New DataTable

                    dtBatteryList = LoadTagReportForBatteryTech(sid, FromDate, ToDate)

                    If dtBatteryList.Rows.Count > 0 Then
                        MakeCSVReportForBatteryTech_IE(dtBatteryList, FromDate, ToDate)
                    Else
                        NoRecordTagReportForBatteryTech_IE(SiteName, FromDate, ToDate)
                    End If

                    ''ASSET
                Case "Asset_SaveSearch"

                    Dim dtReturn As New DataTable
                    Dim ResponseError As String = ""

                    Dim Asset_DeviceName As String = Request.QueryString("DeviceName")
                    Dim Asset_DeviceIds As String = Request.QueryString("DeviceIds")

                    dtReturn = Asset_AddDeviceInfo(siteId, Asset_DeviceIds, Asset_DeviceName)

                    If Not dtReturn Is Nothing Then
                        If dtReturn.Rows.Count > 0 Then
                            If dtReturn.Rows(0).Item("Result") = "1" Then
                                If InStr(dtReturn.Rows(0).Item("Error"), "already") Then
                                    ResponseError = dtReturn.Rows(0).Item("Error")
                                ElseIf InStr(dtReturn.Rows(0).Item("Error"), "Exceed") Then
                                    ResponseError = dtReturn.Rows(0).Item("Error")
                                Else
                                    ResponseError = "Error"
                                End If
                            Else
                                ResponseError = ""
                            End If
                        End If
                    End If

                    Response.Clear()
                    Response.ContentType = "text/xml"
                    Response.CacheControl = "no-cache"
                    Response.Write(ResponseError)
                    Response.End()

                    SendXMLToAJAXCall(dtReturn)

                Case "Asset_GetSavedSearchesr"

                    Dim dtReturn As New DataTable

                    Dim Asset_DeviceIds As String = Request.QueryString("DeviceIds")

                    dtReturn = Asset_GetSavedSearches(sid, Asset_DeviceIds)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetAssetDeviceDetailsForSearch"

                    Dim dtReturn As New DataSet

                    Dim SaveId As String = Request.QueryString("SaveId")
                    Dim SearchDeviceIDs As String = Request.QueryString("SearchDeviceIDs")

                    dtReturn = LoadAssetDeviceDetailsForSearch(siteId, SaveId, SearchDeviceIDs)

                    SendXMLToAJAXCall(dtReturn)

                Case "Asset_EditSavedSearch"

                    Dim dtReturn As New DataTable

                    Dim ResponseError As String = ""

                    Dim Asset_DeviceName As String = Request.QueryString("DeviceName")
                    Dim Asset_DeviceIds As String = Request.QueryString("DeviceIds")
                    Dim Asset_SaveId As String = Request.QueryString("SaveId")
                    Dim Asset_IsDelete As String = Request.QueryString("Asset_IsDelete")

                    dtReturn = Asset_EditDeviceInfo(Asset_SaveId, siteId, Asset_DeviceIds, Asset_DeviceName, Asset_IsDelete)

                    If Not dtReturn Is Nothing Then
                        If dtReturn.Rows.Count > 0 Then
                            If dtReturn.Rows(0).Item("Result") = "1" Then
                                If InStr(dtReturn.Rows(0).Item("Error"), "already") Then
                                    ResponseError = dtReturn.Rows(0).Item("Error")
                                ElseIf InStr(dtReturn.Rows(0).Item("Error"), "Exceed") Then
                                    ResponseError = dtReturn.Rows(0).Item("Error")
                                Else
                                    ResponseError = "Error"
                                End If
                            Else
                                ResponseError = ""
                            End If
                        End If
                    End If

                    Response.Clear()
                    Response.ContentType = "text/xml"
                    Response.CacheControl = "no-cache"
                    Response.Write(ResponseError)
                    Response.End()
                    SendXMLToAJAXCall(dtReturn)

                Case "GetIniHistoryInfo"

                    Dim dtReturn As New DataSet
                    Dim SiteId As String = Request.QueryString("SiteId")
                    Dim IDate As String = Request.QueryString("IDate")
                    Dim IDeviceType As String = Request.QueryString("DeviceType")
                    Dim ISortColumn As String = Request.QueryString("sortcolumn")
                    Dim ISortOrder As String = Request.QueryString("sortorder")
                    Dim IDeviceIds As String = Request.QueryString("DevicedIds")
                    Dim ICurPage As String = Request.QueryString("CurPage")

                    dtReturn = GetIniHistoryInfo(SiteId, IDeviceType, IDate, ISortColumn, ISortOrder, IDeviceIds, ICurPage)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetIniHistoryProfileInfo"

                    Dim dtReturn As New DataTable

                    Dim SiteId As String = Request.QueryString("SiteId")
                    Dim IDeviceType As String = Request.QueryString("DeviceType")
                    Dim DataId As String = Request.QueryString("DataId")

                    dtReturn = GetIniProfileInfoForSite(SiteId, IDeviceType, DataId)

                    SendXMLToAJAXCall(dtReturn)

                Case "UpdateAnnualCalibration"

                    Dim dtReturn As New DataTable

                    sid = Request.Form("Annual_sid")
                    DeviceId = Request.Form("Annual_DeviceId")

                    dtReturn = UpdateAnnualCalibration(sid, DeviceId.Trim())

                    SendXMLToAJAXCall(dtReturn)

                Case "DownloadAnnualCalibration"

                    Dim Taglist As New DataTable
                    Dim dtReturn As New DataTable

                    Taglist = LoadDeviceList(sid, "1", typId, DeviceId, curpage, alertId, Bin, sorColumnname, SorOrder)

                    dtReturn = MakeCSVReportForAnnualCalibration(Taglist, enumDeviceType.Tag)

                    SendXMLToAJAXCall(dtReturn)

                Case "UpdateMangeRoles"

                    Dim EnableActiveDirectoryRoles As String = Request.Form("EnableActiveDirectoryRoles")
                    Dim SSOIAttribute As String = Request.Form("SSOIAttribute")
                    Dim Roles As String = Request.Form("Roles")
                    Dim dtReturn As New DataTable

                    dtReturn = UpdateRoleMappingInfo(EnableActiveDirectoryRoles, SSOIAttribute, Roles)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetMangeRolesInfo"

                    Dim ds As New DataSet

                    ds = GetMangeRoleMappingInfo()

                    SendXMLToAJAXCall(ds)

                Case "CompanyList"

                    Dim dtCompanies As New DataTable
                    dtCompanies = ListCompanies()

                    SendXMLToAJAXCall(dtCompanies)

                Case "AddCompanySetup"

                    Dim dtCompany As New DataTable
                    Dim CompanyName As String = Request.QueryString("CompanyName")

                    isAdd = Request.Form("isAdd")
                    CompanyId = Request.Form("CompanyId")
                    CompanyName = Request.Form("CompanyName")
                    Status = Request.Form("Status")
                    AuthPassword = Request.Form("AuthPassword")

                    dtCompany = AddCompany(isAdd, CompanyId, CompanyName, Status, AuthPassword)

                    SendXMLToAJAXCall(dtCompany)

                Case "InsertPageVisitDetails"

                    '***************************************************************
                    Dim dtTrackingHistory As New DataTable

                    Dim IPAddress As String = System.Web.HttpContext.Current.Request.UserHostAddress

                    If IPAddress = "127.0.0.1" Then
                        IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
                    End If

                    UserId = Request.QueryString("UserId")
                    Dim PageName As String = Request.QueryString("PageName")
                    Dim nPageAction As String = Request.QueryString("nPageAction")
                    Dim PageActionHistory As String = Request.QueryString("PageActionHistory")

                    dtTrackingHistory = AddPageVisitDetails(UserId, EscapeString(PageName), nPageAction, EscapeString(PageActionHistory), IPAddress)
                    SendXMLToAJAXCall(dtTrackingHistory)

                Case "SiteList"

                    Dim dtSites As New DataTable
                    Dim dtReturn As New DataTable

                    Dim isExportSites As String = Request.QueryString("isExportSites")

                    dtSites = ListSite()

                    If isExportSites = "1" Then
                        dtReturn = MakeCSVforSiteList(dtSites)
                        SendXMLToAJAXCall(dtReturn)
                    Else
                        SendXMLToAJAXCall(dtSites)
                    End If

                Case "CampusMapList"

                    Dim dtSites As New DataTable
                    dtSites = CampusMapList(sid)
                    SendXMLToAJAXCall(dtSites)

                Case "DeleteCampusMap"

                    Dim DataId As String = ""
                    DataId = Request.QueryString("DataId")
                    Dim dtCampusMap As New DataTable

                    dtCampusMap = DeleteCampusMap(sid, DataId)
                    SendXMLToAJAXCall(dtCampusMap)

                Case "AddSiteSetup"

                    Dim dtSite As New DataTable
		    
                    Dim IsPrismView, TimeZone, IsUnDeleteTags, IsUnDeleteMonitors, IsDefinedTagsinCore, IsDefinedInfrastructureinCore As String

                    Masterid = Request.Form("Masterid")
                    sid = Request.Form("sid")
                    isAdd = Request.Form("isAdd")
                    CompanyId = Request.Form("CompanyId")
                    SiteName = Request.Form("SiteName")
                    SiteFolder = Request.Form("SiteFolder")
                    FileFormat = Request.Form("FileFormat")
                    Status = Request.Form("Status")
                    IsGroup = Request.Form("IsGroup")
                    IsGroupMember = Request.Form("IsGroupMember")
                    ServerIP = Request.Form("ServerIP")
                    AuthPassword = Request.Form("AuthPassword")
                    LocationCode = Request.Form("LocationCode")
                    QBN = Request.Form("QBN")
                    IsPrismView = Request.Form("IsPrismView")
                    TimeZone = Request.Form("TimeZone")
                    IsUnDeleteTags = Request.Form("IsUnDeleteTags")
                    IsUnDeleteMonitors = Request.Form("IsUnDeleteMonitors")
                    IsDefinedTagsinCore = Request.Form("IsDefinedTagsinCore")
                    IsDefinedInfrastructureinCore = Request.Form("IsDefinedInfrastructureinCore")

                    dtSite = AddSite(Masterid, sid, isAdd, CompanyId, SiteName, SiteFolder, FileFormat, Status, IsGroup, IsGroupMember, ServerIP,
                                     AuthPassword, LocationCode, QBN, IsPrismView, TimeZone, IsUnDeleteTags, IsUnDeleteMonitors, IsDefinedTagsinCore, IsDefinedInfrastructureinCore)

                    SendXMLToAJAXCall(dtSite)

                Case "DeleteSiteConfiguration"

                    Dim dtEmail As New DataTable

                    dtEmail = DeleteSiteConfiguration(sid, AuthUserId)

                    SendXMLToAJAXCall(dtEmail)

                Case "UserList"

                    Dim dtCSV As New DataTable
                    Dim ds As New DataSet

                    Dim isExportUser As String = Request.QueryString("isExport")
                    Dim siteId As String = Request.QueryString("siteId")

                    ds = ListUser(siteId)

                    If isExportUser = "1" Then
                        dtCSV = MakeUserListReport(ds)
                        SendXMLToAJAXCall(dtCSV)
                    Else
                        SendXMLToAJAXCall(ds)
                    End If

                Case "UserActivityLog"

                    Dim dt As New DataTable

                    Dim EventType As String = ""

                    EventType = Request.QueryString("EventType")
                    FromDate = Request.QueryString("From")
                    ToDate = Request.QueryString("To")

                    dt = UserActivityLog(UserId, FromDate, EventType, ToDate)

                    SendXMLToAJAXCall(dt)

                Case "AddUserSetup"

                    Dim dtUser As New DataTable

                    Dim UserType As String = ""
                    Dim Email As String = ""
                    Dim AssociatedEmail As String = ""
                    Dim AssociatedPhone As String = ""
                    Dim UserTypeId As String = ""
                    Dim UserRoleId As String = ""
                    Dim Batteryreplacement As String = ""
                    Dim IsTempMonitoring As String
                    Dim IsPrismView As String
                    Dim AllowAccessForStar As String
                    Dim IsPrismAuditView As String
                    Dim AssetNotification As String
                    Dim DesktopNotification As String
                    Dim IsPrismReadOnly As String
                    Dim AllowAccessForKPI As String
                    Dim isPulseReport As String
                    Dim PulseReportIds As String
                    Dim FirstName As String
                    Dim LastName As String

                    UserId = Request.Form("UserId")
                    isAdd = Request.Form("isAdd")
                    CompanyId = Request.Form("CompanyId")
                    UserName = Request.Form("Un")
                    Password = Request.Form("Pw")
                    Status = Request.Form("Status")
                    UserType = Request.Form("UserType")
                    Email = Request.Form("Email")
                    Batteryreplacement = Request.Form("Batteryreplacement")
                    UserTypeId = Request.Form("UserTypeId")
                    UserRoleId = Request.Form("UserRoleId")
                    AssociatedEmail = Request.Form("AssociatedEmail")
                    AssociatedPhone = Request.Form("AssociatedPhone")
                    AuthPassword = Request.Form("AuthPassword")
                    IsTempMonitoring = Request.Form("IsTempMonitoring")
                    IsPrismView = Request.Form("IsPrismView")
                    IsPrismAuditView = Request.Form("IsPrismAuditView")

                    AllowAccessForStar = Request.Form("AllowAcess")
                    AssetNotification = Request.Form("AssetNotifications")
                    DesktopNotification = Request.Form("DesktopNotification")
                    IsPrismReadOnly = Request.Form("IsPrismReadOnly")
                    AllowAccessForKPI = Request.Form("AllowAcessKPI")
                    isPulseReport = Request.Form("isPulseReport")
                    PulseReportIds = Request.Form("PulseReportIds")
                    FirstName = Request.Form("FirstName")
                    LastName = Request.Form("LastName")

                    dtUser = AddUser(UserId, isAdd, CompanyId, UserName, Password, Status, UserType, Email, Batteryreplacement, UserTypeId, UserRoleId, AssociatedEmail, AssociatedPhone,
                                     AuthPassword, IsTempMonitoring, IsPrismView, IsPrismAuditView, AllowAccessForStar, AssetNotification, DesktopNotification, IsPrismReadOnly, AllowAccessForKPI, isPulseReport, PulseReportIds, FirstName, LastName)

                    SendXMLToAJAXCall(dtUser)

                Case "AddAlertSiteList"

                    Dim qInfraAlertIds As String = Request.Form("qInfraAlertIds")
                    Dim qInfraSiteIds As String = Request.Form("qInfraSiteIds")
                    Dim qLowBatteryAlert As String = Request.Form("qLowBatteryAlert")
                    Dim qLowBatteryReportid As String = Request.Form("qLowBatteryReportid")
                    Dim qEmail As String = Request.Form("qEmail")

                    Dim dtAlert As New DataTable

                    dtAlert = AddSiteAlert(qInfraAlertIds, qInfraSiteIds, qLowBatteryAlert, qLowBatteryReportid, qEmail, g_UserAPI)

                    SendXMLToAJAXCall(dtAlert)

                Case "GetAvailableSitesForUser"

                    Dim dtSites As New DataTable

                    UserId = Request.Form("UserId")

                    dtSites = GetSitesForUser(UserId)

                    SendXMLToAJAXCall(dtSites)

                Case "EditSitesForUser"

                    Dim dtEmail As New DataTable

                    sid = Request.Form("sid")
                    UserId = Request.Form("UserId")
                    isAdd = Request.Form("isAdd")

                    dtEmail = EditSitesForUser(sid, UserId, isAdd)

                    SendXMLToAJAXCall(dtEmail)

                Case "DeleteUserConfiguration"

                    Dim dtEmail As New DataTable

                    UserId = Request.Form("UserId")

                    dtEmail = DeleteUserConfiguration(UserId)

                    SendXMLToAJAXCall(dtEmail)

                Case "LogViewer"

                    Dim dtSites As New DataTable

                    dtSites = ListLogViewer()

                    SendXMLToAJAXCall(dtSites)

                Case "ServerResponse"

                    Dim dtResponse As New DataTable

                    dtResponse = GetResponseHistory()

                    SendXMLToAJAXCall(dtResponse)

                Case "ThresholdConfiguration"

                    Dim dtThresholdConfiguration As New DataTable

                    dtThresholdConfiguration = ListThresholdConfiguration()

                    SendXMLToAJAXCall(dtThresholdConfiguration)

                Case "AddThresholdSetup"

                    Dim DataId As String = ""
                    Dim SiteDownEmail As String = ""
                    Dim SoftwareDownEmail As String = ""
                    Dim LowHardDiskMemory As String = ""
                    Dim LastUpdateThreshold As String = ""
                    Dim SMSSenderID As String = ""
                    Dim SWSMSSenderID As String = ""
                    Dim ActiveListFailedCount As String = ""

                    DataId = Request.QueryString("DataId")
                    SiteDownEmail = Request.QueryString("SiteDownEmail")
                    SoftwareDownEmail = Request.QueryString("SoftwareDownEmail")
                    LowHardDiskMemory = Request.QueryString("LowHardDiskMemory")
                    LastUpdateThreshold = Request.QueryString("LastUpdateThreshold")
                    SMSSenderID = Request.QueryString("SMSSenderID")
                    SWSMSSenderID = Request.QueryString("SWSMSSenderID")
                    ActiveListFailedCount = Request.QueryString("ActiveListFailedCount")

                    Dim dtUser As New DataTable

                    dtUser = AddThresholdConfiguration(DataId, isAdd, SiteDownEmail, SoftwareDownEmail, LowHardDiskMemory, LastUpdateThreshold, SMSSenderID, SWSMSSenderID, ActiveListFailedCount)

                    SendXMLToAJAXCall(dtUser)

                Case "UpdateLocalIdInfo"

                    Dim Local_sid As String = Request.Form("Local_sid")
                    Dim Local_DeviceId As String = Request.Form("Local_DeviceId")
                    Dim LocalId As String = Request.Form("LocalId")
                    Dim L_MonitorId As String = Request.Form("MonitorId")
                    Dim L_Location As String = Request.Form("Location")
                    Dim dt As New DataTable

                    dt = UpdateLocalIdInfo(Local_sid, enumDeviceType.Tag, Local_DeviceId, LocalId, L_MonitorId, L_Location)

                    SendXMLToAJAXCall(dt)

                Case "ConfigAutomatedReports"

                    Dim aAlertId As Integer = Val(Request.QueryString("AlertId"))
                    Dim Emails As String = Request.QueryString("Emails")

                    Dim dt As New DataTable

                    dt = ConfigAutomatedReportEmails(Emails, aAlertId)

                    SendXMLToAJAXCall(dt)

                Case "SitePendingFiles"

                    Dim dtSites As New DataTable

                    Dim Server As String = ""
                    Dim DBName As String = ""
                    Dim nCount As Integer

                    Server = Request.QueryString("ServerIP")
                    DBName = Request.QueryString("DBName")

                    dtSites = ListSitePendingFiles(Server, DBName)

                    nCount = dtSites.Rows.Count

                    For nCount = 0 To dtSites.Rows.Count - 1
                        If dtSites.Rows(nCount)(0).ToString() = "" Then
                            dtSites.Rows(nCount).Delete()
                        End If
                    Next

                    SendXMLToAJAXCall(dtSites)

                Case "GetServerAction"

                    Dim dt As New DataTable

                    Dim ActionStatus As String = Request.QueryString("Status")
                    Dim ActionMessage As String = Request.QueryString("Message")

                    dt = GetServerStatus(ActionStatus, ActionMessage)

                    SendXMLToAJAXCall(dt)

                Case "GetSiteList"

                    Dim dt As New DataTable

                    Dim sCompanys As String = Session("VACompanys")
                    Dim sSites As String = Session("VASites")

                    dt = loadsiteList(sCompanys, sSites)

                    SendXMLToAJAXCall(dt)

                Case "GetOnDemandReportsInfo"

                    Dim dt As New DataTable

                    Dim SiteId As String = Request.QueryString("SiteId")
                    Dim TypeIds As String = Request.QueryString("TypeIds")
                    Dim strdate As String = Request.QueryString("Date")

                    dt = GetOnDemandReportsInfo(SiteId, TypeIds, strdate)

                    SendXMLToAJAXCall(dt)

                Case "UpgradeGMSServiceList"

                    Dim dtGMSService As New DataTable

                    dtGMSService = ListUpgradeGMSService()

                    SendXMLToAJAXCall(dtGMSService)

                Case "DeleteParserService"

                    Dim dtService As New DataTable

                    Dim DataId As String = Request.QueryString("DataId")

                    dtService = DeleteParserService(DataId)

                    SendXMLToAJAXCall(dtService)

                Case "OpenUpgradeHistory"

                    Dim dtGMSService As New DataTable

                    Dim DataId As String = Request.QueryString("DataId")

                    dtGMSService = ListUpgradeHistory(DataId)

                    SendXMLToAJAXCall(dtGMSService)

                Case "GetRDBServices"

                    Dim dtRDBService As New DataTable

                    dtRDBService = ListRDBServices()

                    SendXMLToAJAXCall(dtRDBService)

                Case "RestartParserService"

                    Dim dtService As New DataTable

                    Dim sServerIP As String = Request.QueryString("ServerIP")
                    Dim DBName As String = Request.QueryString("DBName")
                    Dim sServiceId As String = Request.QueryString("ServiceId")

                    dtService = RestartParserService(sServerIP, DBName, sServiceId)

                    SendXMLToAJAXCall(dtService)

                Case "GetGMSStarSlotDetails"

                    Dim dtStar As New DataTable

                    Dim sStarSiteId As String = Request.QueryString("SiteId")
                    Dim sStarDeviceId As String = Request.QueryString("DeviceId")
                    Dim sStarDate As String = Request.QueryString("Date")

                    dtStar = GetGMSStarSlotDetails(sStarSiteId, sStarDeviceId, sStarDate)

                    SendXMLToAJAXCall(dtStar)

                Case "GetGMSBeaconStarSlotDetails"

                    Dim dtBeaconStar As New DataTable

                    Dim sStarSiteId As String = Request.QueryString("SiteId")
                    Dim sStarDeviceId As String = Request.QueryString("DeviceId")
                    Dim sStarDate As String = Request.QueryString("Date")

                    dtBeaconStar = GetGMSBeaconStarSlotDetails(sStarSiteId, sStarDeviceId, sStarDate)

                    SendXMLToAJAXCall(dtBeaconStar)
                Case "GetGMSMonitorGroupsInfoByDeviceId"

                    Dim dtGroup As New DataTable

                    Dim sGroupSiteId As String = Request.QueryString("SiteId")
                    Dim sGroupDeviceId As String = Request.QueryString("DeviceId")
                    Dim sDate As String = Request.QueryString("Date")

                    dtGroup = GetGMSMonitorGroupsInfoByDeviceId(sGroupSiteId, sGroupDeviceId, sDate)

                    SendXMLToAJAXCall(dtGroup)

                Case "GetDeviceMonitorHourlyInfo"

                    Dim dtDeviceMonitorHourly As New DataTable
                    Dim dtDeviceStarHourly As New DataTable
                    Dim dtReturn As New DataTable

                    Dim sMonitorHourlySiteId As String = Request.QueryString("SiteId")
                    Dim sMonitorHourlyDeviceId As String = Request.QueryString("DeviceId")
                    Dim sDate As String = Request.QueryString("Date")
                    Dim sDeviceType As String = Request.QueryString("DeviceType")
                    Dim IsDownload As String = Request.QueryString("IsDownload")

                    If sDeviceType = enumDeviceType.Star Then
                        dtDeviceMonitorHourly = GetDeviceMonitorHourlyInfo(sMonitorHourlySiteId, sMonitorHourlyDeviceId, enumDeviceType.Star, "500")

                        Dim dv As New DataView(dtDeviceMonitorHourly)
                        dv.RowFilter = "UpdatedOn>=#" + Convert.ToDateTime(sDate) + "# And UpdatedOn<#" + Convert.ToDateTime(sDate).AddDays(1) + "#"
                        dtDeviceMonitorHourly = dv.ToTable()

                    Else
                        dtDeviceMonitorHourly = GetDeviceMonitorHourlyInfo(sMonitorHourlySiteId, sMonitorHourlyDeviceId, enumDeviceType.Monitor, "24")
                    End If

                    If IsDownload > 0 Then
                        If dtDeviceMonitorHourly.Rows.Count > 0 Then
                            dtReturn = MakeCSVForStarOneHrReport(dtDeviceMonitorHourly, sDate, SiteName)
                        Else
                            dtReturn = NoRcordCSVForStarOneHrReport(dtDeviceMonitorHourly, sDate, SiteName)
                        End If

                        SendXMLToAJAXCall(dtReturn)

                    Else
                        SendXMLToAJAXCall(dtDeviceMonitorHourly)
                    End If

                Case "GetAllReports"

                    Dim dtDeviceSummary As New DataSet
                    Dim dsDeviceSummaryReport As New DataSet
                    Dim dtDeviceReport As New DataTable
                    Dim dtReturn As New DataTable
                    Dim sSiteId As String = Request.QueryString("SiteId")
                    Dim sReportType As String = Request.QueryString("ReportType")
                    Dim sDeviceType As String = Request.QueryString("DeviceType")
                    Dim sDeviceId As String = Request.QueryString("DeviceId")
                    Dim sFromDate As String = Request.QueryString("FromDate")
                    Dim sToDate As String = Request.QueryString("ToDate")
                    Dim IsDownload As String = Request.QueryString("IsDownload")
                    Dim sStarId As String = Request.QueryString("StarId")
                    Dim IsChkTTSyncError As Integer = Request.QueryString("IsChkTTSyncError")
                    Dim IsConnFailed As String = Request.QueryString("IsConnFailed")
                    Dim sPagingCount As String = Request.QueryString("PagingCount")
                    Dim sLocationCount As String = Request.QueryString("LocationCount")
                    Dim sTriggerCount As String = Request.QueryString("TriggerCount")
                    Dim sFilterCond As String = Request.QueryString("FilterCond")
                    Dim sLocationFilterCond As String = Request.QueryString("LocationFilterCond")
                    Dim sTriggerFilterCond As String = Request.QueryString("TriggerFilterCond")
                    Dim sGroupCond As String = Request.QueryString("GroupCond")
                    Dim sStarsUpgradeMode As String = Request.QueryString("StarsUpgradeMode")
                    Dim sStarsTTSyncError As String = Request.QueryString("StarsTTSyncError")
                    Dim sStarsNotReceivingData As String = Request.QueryString("StarsNotReceivingData")
                    Dim sStarsNotReceiving24hr As String = Request.QueryString("StarsNotReceiving24hr")
                    Dim sStarsSeenNetworkIssue As String = Request.QueryString("StarsSeenNetworkIssue")

                    Session("ReportSiteId") = sSiteId

                    If sReportType = enumReportType.DefectiveReport Then

                        SendXMLToAJAXCall(dtDeviceSummary)

                    Else

                        If sReportType = 1 Or sReportType = 3 Then
                            sToDate = sFromDate
                        End If

                        dtDeviceSummary = GetReports(sSiteId, sReportType, sDeviceType, sDeviceId, sFromDate, sToDate,
                                                     sStarId, IsChkTTSyncError, IsConnFailed, sPagingCount, sLocationCount,
                                                     sTriggerCount, sFilterCond, sLocationFilterCond, sTriggerFilterCond,
                                                     sGroupCond, sStarsUpgradeMode, sStarsTTSyncError, sStarsNotReceivingData, sStarsNotReceiving24hr, sStarsSeenNetworkIssue)
                        dsDeviceSummaryReport = GetDeviceSummaryReport(dtDeviceSummary, sGroupCond)

                        If IsDownload > 0 And sReportType <> 1 Then

                            If Not dtDeviceSummary Is Nothing Then
                                If dtDeviceSummary.Tables.Count > 0 Then
                                    dtDeviceReport = dtDeviceSummary.Tables(0)
                                End If
                            End If

                            If sReportType = 3 Then

                                If dtDeviceReport.Rows.Count > 0 Then
                                    dtReturn = MakeCSVForTTSyncErrReport(dtDeviceReport, sFromDate, sToDate, SiteName)
                                Else
                                    dtReturn = NoRcordCSVForTTSyncErrReport(dtDeviceReport, sFromDate, sToDate, SiteName)
                                End If

                            ElseIf sReportType = 4 Then

                                If dtDeviceReport.Rows.Count > 0 Then
                                    dtReturn = MakeCSVForConnectivityReport(dtDeviceReport, sFromDate, sToDate, SiteName)
                                Else
                                    dtReturn = NoRcordCSVForConnectivityReport(dtDeviceReport, sFromDate, sToDate, SiteName)
                                End If

                            End If

                            SendXMLToAJAXCall(dtReturn)

                        Else
                            SendXMLToAJAXCall(dsDeviceSummaryReport)
                        End If
                    End If

                Case "GetStarDailyPLCount"

                    Dim dtStarDailyData As New DataTable

                    Dim sGroupSiteId As String = Request.QueryString("SiteId")
                    Dim sDate As String = Request.QueryString("Date")

                    dtStarDailyData = GetStarDailyPLCount(sGroupSiteId, sDate)

                    SendXMLToAJAXCall(dtStarDailyData)

                Case "ResetWPSAdminPassword"

                    Dim sWPSSiteId As String = Request.QueryString("SiteId")
                    Dim sWPSAdminId As String = Request.QueryString("UserId")
                    Dim sWPSAdminNewPwd As String = Request.QueryString("NewPwd")
                    Dim sAdminUserName As String = Request.QueryString("adminUserName")

                    Dim dt As New DataTable

                    dt = UpdateWPSAdminNewPassword(sWPSSiteId, sWPSAdminId, sAdminUserName, sWPSAdminNewPwd)

                    SendXMLToAJAXCall(dt)

                Case "ConfigureWPSSiteInGMS"

                    Dim sWPSSiteId As String = Request.QueryString("SiteId")
                    Dim sInsert As String = Request.QueryString("isInsert")

                    Dim dt As New DataTable

                    dt = ConfigureWPSSiteInGMS(sWPSSiteId, sInsert)

                    SendXMLToAJAXCall(dt)

                Case "SupportList"

                    Dim dtSites As New DataTable

                    dtSites = ListSupport(Status)

                    SendXMLToAJAXCall(dtSites)

                Case "AddSupportSetup"

                    Dim dtSite As New DataTable

                    dtSite = AddSupport(Masterid, sid, SiteName, isAdd, Status)

                    SendXMLToAJAXCall(dtSite)

                Case "DownloadExcelDisasterRecovery"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    sid = Request.Form("sid")

                    dt = LoadDisasterRecovery(sid)

                    dtReturn = MakeDisasterRecoveryCSV(dt)

                    SendXMLToAJAXCall(dtReturn)

                Case "DownloadExcelDisasterRecovery_ForIE"

                    Dim dt As New DataTable

                    dt = LoadDisasterRecovery(sid)

                    PrepareCSVLoadDisasterRecovery(dt)

                Case "AddSiteForSettings"

                    Dim dtSite As New DataTable
                    Dim SiteType As String = ""

                    Dim Statusstr As String = Request.QueryString("Status")
                    Dim Sitelist As String = Request.QueryString("SiteList")

                    SiteType = Request.QueryString("FileFormat")

                    dtSite = AddSiteForSettings(Sitelist, Statusstr, SiteType)

                    SendXMLToAJAXCall(dtSite)

                Case "GetSiteForSettings"

                    Dim dtSite As New DataTable
                    Dim SiteType As String = ""

                    Dim Statusstr As String = Request.QueryString("Status")
                    SiteType = Request.QueryString("FileFormat")

                    dtSite = GetSiteForSettings(Statusstr, SiteType)

                    SendXMLToAJAXCall(dtSite)

                Case "UpdateDeviceLocation"

                    Dim dt As New DataTable

                    Dim SiteId As String = Request.Form("Location_sid")
                    Dim sDeviceType As String = Request.Form("Location_DeviceType")
                    Dim sDeviceId As String = Request.Form("Location_DeviceId")
                    Dim DeviceLocation As String = Request.Form("DeviceLocation")

                    dt = UpdateDeviceLocation(SiteId, sDeviceType, sDeviceId, DeviceLocation)

                    SendXMLToAJAXCall(dt)

                Case "StreamingFieldsList"

                    Dim dtStreamingFields As New DataTable

                    Dim sSiteId As String = Request.QueryString("SiteId")

                    dtStreamingFields = ListStreamingFields(sSiteId)
                    SendXMLToAJAXCall(dtStreamingFields)

                Case "MSESettingsList"

                    Dim dtMSESettings As New DataTable
                    Dim sSiteId As String = Request.QueryString("SiteId")

                    dtMSESettings = ListMSESettings(sSiteId)

                    SendXMLToAJAXCall(dtMSESettings)

                Case "MSESettingsHistoryList"

                    Dim dtMSESettingsHistory As New DataTable

                    Dim sSiteId As String = Request.QueryString("SiteId")

                    dtMSESettingsHistory = ListMSESettingsHistory(sSiteId)

                    SendXMLToAJAXCall(dtMSESettingsHistory)

                Case "ChangePassword"

                    Dim VA_UserId As String = Request.Form("UserId")
                    Dim VA_OldPassword As String = Request.Form("OldPassword")
                    Dim VA_NewPassword As String = Request.Form("NewPassword")
                    Dim VA_RetypePassword As String = Request.Form("RetypePassword")

                    Dim ResponseError As String = ""
                    Dim dtReturn As New DataTable
                    Dim isNotSanitized As Boolean = False
                    Dim comparer As CompareInfo = CultureInfo.InvariantCulture.CompareInfo

                    For i As Integer = 0 To fromblackList.Length - 1
                        If (comparer.IndexOf(VA_NewPassword, fromblackList(i), CompareOptions.IgnoreCase) >= 0) Then
                            isNotSanitized = True

                            ResponseError = "Password not sanitized"
                            Response.Clear()
                            Response.ContentType = "text/xml"
                            Response.CacheControl = "no-cache"
                            Response.Write(ResponseError)
                            Response.End()
                            Exit For
                            Exit Sub
                        End If
                    Next

                    If isNotSanitized = False Then
                        'dtReturn = ChangePasswordForUser(VA_UserId, VA_OldPassword, VA_NewPassword)
                        SendXMLToAJAXCall(dtReturn)
                    End If

                Case "ConfigADDirectory"

                    Dim AD_ServerIp As String = Request.Form("ServerIp")
                    Dim AD_UserName As String = Request.Form("UserName")
                    Dim AD_Password As String = Request.Form("Password")

                    Dim dtReturn As New DataTable
                    dtReturn = AddADDirectory(AD_ServerIp, AD_UserName, AD_Password)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetADDirectoryInfo"

                    Dim ds As New DataSet
                    ds = GetADDirectory()

                    SendXMLToAJAXCall(ds)

                Case "GetCompanyGroup"

                    Dim dtCompanyGroup As New DataTable

                    dtCompanyGroup = GetCompanyGroup()

                    SendXMLToAJAXCall(dtCompanyGroup)

                Case "AddCompanyGroup"

                    Dim dtCompanyGroup As New DataTable
                    Dim GroupId As String = Request.Form("GroupId")
                    Dim GroupName As String = Request.Form("GroupName")

                    CompanyId = Request.Form("CompanyId")
                    isAdd = Request.Form("IsAdd")

                    dtCompanyGroup = AddCompanyGroup(GroupId, GroupName, CompanyId, isAdd)

                    SendXMLToAJAXCall(dtCompanyGroup)

                Case "DeleteCompanyGroup"

                    Dim dtCompanyGroup As New DataTable
                    Dim GroupId As String = Request.Form("GroupId")

                    dtCompanyGroup = DeleteCompanyGroup(GroupId)

                    SendXMLToAJAXCall(dtCompanyGroup)

                Case "GetSiteADGroup"

                    Dim dtAssociatedSite As New DataTable

                    dtAssociatedSite = ListAssociatedSite()

                    SendXMLToAJAXCall(dtAssociatedSite)

                Case "AddAssociationSiteSetup"

                    Dim AD_GroupId As String = Request.Form("GroupId")
                    Dim AD_GroupName As String = Request.Form("GroupName")
                    Dim VHA_GroupId As String = Request.Form("VHAGroupId")

                    isAdd = Request.Form("IsAdd")
                    sid = Request.Form("sid")

                    Dim dtSite As New DataTable

                    dtSite = AddAssociationSiteSetup(sid, AD_GroupName, AD_GroupId, VHA_GroupId, isAdd)

                    SendXMLToAJAXCall(dtSite)

                Case "DeleteSiteAssociation"

                    Dim AD_GroupId As String = Request.Form("GroupId")
                    Dim dtEmail As New DataTable

                    dtEmail = DeleteSiteAssociation(AD_GroupId)

                    SendXMLToAJAXCall(dtEmail)

                Case "GetAlertMaster"

                    Dim dtAlerts As New DataTable

                    dtAlerts = LoadAlertList()

                    SendXMLToAJAXCall(dtAlerts)

                Case "BatteryReplacementFailure"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    sid = Request.Form("sid")

                    dt = LoadBatteryReplacementFailure(sid)

                    dtReturn = MakeCSVforBatteryReplacementFailureReport(dt, Val(sid))

                    SendXMLToAJAXCall(dtReturn)

                Case "SiteAnalysisReport"

                    Dim ds As New DataSet
                    Dim dtReturn As New DataTable

                    sid = Request.QueryString("sid")

                    Dim ReportType As String = Request.QueryString("ReportType")
                    Dim DType As String = Request.QueryString("DeviceType")
                    Dim isExportReport As String = Request.QueryString("isExport")
                    Dim Products As String = Request.QueryString("Products")

                    ds = LoadSiteanalysisReport(sid, ReportType, DType, Products)

                    SendXMLToAJAXCall(ds)

                Case "CentrakVoltDetailReport"

                    Dim ds As DataSet
                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    Dim LoginUserId As Integer = 0
                    Dim ShowonlydevicesImanaged As Boolean

                    sid = Request.Form("sid")
                    FromDate = Request.Form("FromDate")
                    ToDate = Request.Form("ToDate")
                    ShowonlydevicesImanaged = Request.Form("ShowonlydevicesImanaged")

                    If ShowonlydevicesImanaged Then
                        LoginUserId = g_UserId
                    End If

                    ds = GetCenTrakVoltDetailReport(sid, "", FromDate, ToDate, LoginUserId)

                    dtReturn = MakeCSVforCentrakVoltDetailReport(sid, ds)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetCetaniMetadata"

                    Dim dtReturn As New DataTable

                    dtReturn = GetCetaniMetaDataInfo(Request.Form("SiteId"), Request.Form("SearchValue"), Request.Form("curpage"), Request.Form("PageSize"), Request.Form("devicetype"))

                    SendXMLToAJAXCall(dtReturn)

                Case "MonitorGroupReport"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    dt = MonitorGroupReport(Request.Form("sid"))

                    dtReturn = MakeCSVforMonitorGroupDetail(dt)

                    SendXMLToAJAXCall(dtReturn)

                Case "GetDeviceLocationsForSite"

                    Dim dt As New DataTable

                    dt = GetDeviceLocationsForSite(Request.Form("sid"), Request.Form("devicetype"))

                    SendXMLToAJAXCall(dt)

                Case "EMTagActivityReport"

                    Dim dt As New DataTable
                    Dim dtCSV As New DataTable

                    Dim ReportType As Integer = Request.QueryString("ReportType")
                    Dim ReportDate As String = Request.QueryString("ReportDate")
                    Dim Email As String = Request.QueryString("Email")
                    Dim SiteId As String = Request.QueryString("SiteId")
                    Dim SubType As String = Request.QueryString("SubType")

                    Dim isDailyData As String = Request.QueryString("isDailyData")
                    Dim tTagId As String = Request.QueryString("TagId")
                    Dim ProbeType As String = Request.QueryString("ProbeType")

                    '' EM In Activity Report
                    dt = LoadEMActivityReport(ReportType, ReportDate, Email, SiteId, SubType, isDailyData, tTagId)

                    ' Make CSV
                    dtCSV = MakeCSVforCentrakEMActivityReport(ReportType, dt, isDailyData, tTagId, ProbeType, SubType)

                    SendXMLToAJAXCall(dtCSV)

                Case "GetLocationChangeEvent"

                    Dim dt As New DataTable
                    Dim dNewRow As DataRow = Nothing

                    Dim lSiteId As String = Request.QueryString("SiteId")
                    Dim lDeviceId As String = Request.QueryString("DeviceId")
                    Dim lFromDate As String = Request.QueryString("FromDate")
                    Dim lToDate As String = Request.QueryString("ToDate")
                    Dim lType As String = Request.QueryString("Type")
                    Dim EventTreshold As String = Request.QueryString("EventTreshold")
                    Dim inMonitorIds As String = Request.QueryString("inMonitorIds")
                    Dim exMonitorIds As String = Request.QueryString("exMonitorIds")
                    Dim TimeSpend As String = Request.QueryString("TimeSpend")
                    Dim SpendType As String = Request.QueryString("SpendType")

                    lDeviceId = lDeviceId.Trim.Trim(",")
                    inMonitorIds = inMonitorIds.Trim.Trim(",")
                    exMonitorIds = exMonitorIds.Trim.Trim(",")

                    lFromDate = lFromDate & " 00:00"
                    lToDate = lToDate & " 23:59"

                    Dim nDay As Integer = DateDiff(DateInterval.Day, CDate(lFromDate), CDate(lToDate))

                    nDay = nDay + 1

                    If (nDay > 7) Then

                        Dim addColumn() As String = {"Result"}
                        dt = addColumntoDataTable(addColumn)

                        dNewRow = dt.NewRow()
                        dNewRow("Result") = 1
                        dt.Rows.Add(dNewRow)

                        SendXMLToAJAXCall(dt)

                    End If

                    Dim xmlElet As XmlElement = LoadLocationChangeEvent(lSiteId, lDeviceId, lFromDate, lToDate, lType, EventTreshold, inMonitorIds, exMonitorIds, TimeSpend, SpendType)

                    dt = MakeCSVForLocationChangeEvent(xmlElet, lFromDate, lToDate, lType, lDeviceId, inMonitorIds, exMonitorIds, TimeSpend)

                    SendXMLToAJAXCall(dt)

                Case "GetUserPulseReport"

                    Dim dt As New DataTable

                    dt = GetUserPulseReports()

                    SendXMLToAJAXCall(dt)

                Case "HistoricalTemperatureReport"

                    Dim dt As New DataTable
                    Dim dtReturn As New DataTable

                    Dim isERROR As Boolean = False

                    sid = Request.QueryString("sid")
                    Dim sSitename As String = Request.QueryString("Sitename")
                    Dim sDeviceId As String = Request.QueryString("DeviceId")
                    Dim sFromDate As String = Request.QueryString("fromdate")
                    Dim sTodate As String = Request.QueryString("todate")

                    dt = LoadHistoricalTemperatureReport(sid, sDeviceId, sFromDate, sTodate, isERROR)

                    If Not isERROR Then
                        dtReturn = MakeHistoricalTemperatureReport(dt, sid, sSitename, sDeviceId, sFromDate, sTodate)
                        SendXMLToAJAXCall(dtReturn)
                    Else
                        SendXMLToAJAXCall(dt)
                    End If

            End Select

        End Sub

        Private Function ReadFile(ByVal file As HttpPostedFile) As Byte()

            Dim data As Byte() = New [Byte](file.ContentLength - 1) {}

            file.InputStream.Read(data, 0, file.ContentLength)

            Return data

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : MakeCSV                                                                                                                              '
        ' Input         : Datatable , devicetype,Bin,Type                                                                                                      ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Function MakeCSV(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal Bin As String, ByVal Type As String, Optional ByVal dtMonitor As DataTable = Nothing, Optional ByVal PulseUIId As Integer = 0) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim isTagdataFound As Boolean = True
            Dim isMonitorDataFound As Boolean = True

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim Version As String = ""
            Dim isUHFTags As Boolean = False

            If (Bin = "0") Then
                Sheetname = "Good Device List"
            ElseIf (Bin = "1") Then
                Sheetname = "UnderWatch List"
            ElseIf (Bin = "2") Then
                Sheetname = "LBI List"
            ElseIf (Bin = "3") Then
                Sheetname = "Offline List"
            ElseIf (Bin = "4") Then
                Sheetname = "All"
            ElseIf (Bin = "") Then
                Sheetname = "All"
            Else
                Sheetname = ""
            End If

            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If dt.Rows.Count > 0 Then

                scontext.Append(CSVCell("Site Name : " & dt.Rows(0).Item("Sitename").Replace(",", "")))
                scontext.Append(CSVNewLine())

                If DeviceType = enumDeviceType.Tag Then
                    isUHFTags = dt.Rows(0).Item("isUHFTags")
                    scontext.Append(CSVCell("Type: " & dt.Rows(0).Item("TagType").Replace(",", "")))
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    scontext.Append(CSVCell("Type: " & dt.Rows(0).Item("MonitorType").Replace(",", "")))
                ElseIf DeviceType = enumDeviceType.Star Then
                    scontext.Append(CSVCell("Type: " & dt.Rows(0).Item("StarSubType").Replace(",", "")))
                ElseIf DeviceType = 0 Then
                    scontext.Append(CSVCell("Type: All"))
                End If

                scontext.Append(CSVNewLine())

                If DeviceType = 0 Then
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("TAG"))
                    scontext.Append(CSVNewLine())
                End If

                If DeviceType = enumDeviceType.Tag Or DeviceType = 0 Then

                    If IsEMTag(Type) Then

                        scontext.Append(CSVCell("TAG ID", True))
                        scontext.Append(CSVCell("P1 ID", True))
                        scontext.Append(CSVCell("P2 ID", True))

                        If Bin = "0" Or Bin = "" Then
                            scontext.Append(CSVCell("Good", True))
                        End If

                        GetGoodIndication(scontext, siteId)

                        If Bin = "1" Or Bin = "4" Or Bin = "" Then
                            scontext.Append(CSVCell("Less than 90 Days", True))
                        End If

                        If Bin = "2" Or Bin = "4" Or Bin = "" Then
                            scontext.Append(CSVCell("Less than 30 Days", True))
                        End If

                        scontext.Append(CSVCell("Local ID", True))
                        scontext.Append(CSVCell("Cal Frequency", True))
                        scontext.Append(CSVCell("Last Cal", True))
                        scontext.Append(CSVCell("CenTrak Cal Due", True))
                        scontext.Append(CSVCell("Client Cal Due", True))
                        scontext.Append(CSVCell("Star Address", True))
                        scontext.Append(CSVCell("Last Seen", True))
                        scontext.Append(CSVCell("Model Item", True))
                        scontext.Append(CSVCell("Location", True))

                        If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                            scontext.Append(CSVCell("Voltage", True))
                            scontext.Append(CSVCell("% Batt", True))
                        End If

                        If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Or g_IsTempMonitoring = "True" Then
                            scontext.Append(CSVCell("P1 Value", True))
                            scontext.Append(CSVCell("P1 Units", True))
                            scontext.Append(CSVCell("P2 Value", True))
                            scontext.Append(CSVCell("P2 Units", True))
                            scontext.Append(CSVCell("AVG RSSI", True))
                        End If

                    Else

                        scontext.Append(CSVCell("TAG ID", True))
                        scontext.Append(CSVCell("Monitor Location", True))
                        scontext.Append(CSVCell("Monitor ID", True))

                        If Bin = "4" Or Bin = "" Then
                            scontext.Append(CSVCell("Star Address", True))
                        End If

                        If Bin = "0" Or Bin = "" Then
                            scontext.Append(CSVCell("Good", True))
                        End If

                        GetGoodIndication(scontext, siteId)

                        If Bin = "1" Or Bin = "4" Or Bin = "" Then
                            scontext.Append(CSVCell("Less than 90 Days", True))
                        End If

                        If Bin = "2" Or Bin = "4" Or Bin = "" Then
                            scontext.Append(CSVCell("Less than 30 Days", True))
                        End If

                        If Bin = "3" Or Bin = "" Or Bin = "7" Then
                            scontext.Append(CSVCell("Date Last seen by Monitor", True))
                        End If

                        scontext.Append(CSVCell("Date Last seen by Network", True))
                        scontext.Append(CSVCell("Model Item", True))

                        If Bin = "3" Or Bin = "" Then
                            If g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.Maintenance Then
                                scontext.Append(CSVCell("Offline (Battery Depleted)", True))
                            Else
                                scontext.Append(CSVCell("Offline", True))
                            End If
                        End If

                        If Bin = "7" Then
                            scontext.Append(CSVCell("Offline (Battery Depleted)", True))
                        End If

                        If Bin = "" Then
                            scontext.Append(CSVCell("Battery Replaced On", True))
                        End If

                        If Bin = "" And isUHFTags Then
                            If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                scontext.Append(CSVCell("Cumulative Activity Level", True))
                            End If
                        End If

                    End If

                    scontext.Append(CSVNewLine())

                    If isTagdataFound Then

                        For nIdx As Integer = 0 To dt.Rows.Count - 1
                            With (dt.Rows(nIdx))

                                Dim s30daysCell As String = "No"
                                Dim s90DaysCell As String = "No"
                                Dim sgood As String = "No"
                                Dim sgoodindication As String = "No"
                                Dim offline As String = ""
                                Dim Units As String = ""

                                Dim CatastrophicCases As String = ""
                                Dim CCatastrophicCases As String = ""

                                CCatastrophicCases = CheckIsDBNull(.Item("CCatastrophicCases"))
                                CatastrophicCases = CheckIsDBNull(.Item("CatastrophicCases"))
                                offline = CheckIsDBNull(.Item("offline"))
                                Version = CheckIsDBNull(.Item("Version"))

                                If CCatastrophicCases = "5" Then
                                    sgoodindication = "Yes"
                                ElseIf (CatastrophicCases = "1" Or CatastrophicCases = "2") Then
                                    s30daysCell = "Yes"
                                ElseIf (CatastrophicCases = "4") Then
                                    s90DaysCell = "Yes"
                                ElseIf (CatastrophicCases = "0") Then
                                    sgood = "Yes"
                                End If

                                If (offline = "1") Then
                                    offline = "Yes"
                                ElseIf (offline = "0") Then
                                    offline = "No"
                                End If

                                If IsEMTag(Type) Then

                                    scontext.Append(CSVCell(.Item("TagId"), True))
                                    scontext.Append(CSVCell(.Item("ProbeId"), True))
                                    scontext.Append(CSVCell(.Item("ProbeId2"), True))

                                    If Bin = "0" Or Bin = "" Then
                                        scontext.Append(CSVCell(sgood, True))
                                    End If

                                    GetGoodIndicationValues(scontext, siteId, sgoodindication)

                                    If Bin = "1" Or Bin = "4" Or Bin = "" Then
                                        scontext.Append(CSVCell(s90DaysCell, True))
                                    End If

                                    If Bin = "2" Or Bin = "4" Or Bin = "" Then
                                        scontext.Append(CSVCell(s30daysCell, True))
                                    End If

                                    scontext.Append(CSVCell(.Item("LocalId").Replace(",", ""), True))
                                    scontext.Append(CSVCell(.Item("CalFrequency"), True))
                                    scontext.Append(CSVCell(.Item("CertificateDate"), True))
                                    scontext.Append(CSVCell(.Item("MFRCalibrationDue"), True))
                                    scontext.Append(CSVCell(.Item("ClientCalDue"), True))
                                    scontext.Append(CSVCell(.Item("StarAddress"), True))
                                    scontext.Append(CSVCell(.Item("LastSeen"), True))
                                    scontext.Append(CSVCell(.Item("ModelItem"), True))
                                    scontext.Append(CSVCell(.Item("Location").Replace(",", ""), True))

                                    If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                        scontext.Append(CSVCell(.Item("Voltage"), True))
                                        scontext.Append(CSVCell(.Item("BatteryCapacity"), True))
                                    End If

                                    If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Or g_IsTempMonitoring = "True" Then

                                        Dim Temp1 As String = .Item("Probe1Temperature")
                                        Dim Units1 As String = .Item("P1Units")

                                        If Units1 <> "" Then
                                            Units1 = Units1.Replace("�C", "Celsius")
                                        End If

                                        Dim Temp2 As String = .Item("Probe2Temperature")
                                        Dim Units2 As String = .Item("P2Units")

                                        If Units2 <> "" Then
                                            Units2 = Units2.Replace("�C", "Celsius")
                                        End If

                                        scontext.Append(CSVCell(Temp1, True))
                                        scontext.Append(CSVCell(Units1, True))
                                        scontext.Append(CSVCell(Temp2, True))
                                        scontext.Append(CSVCell(Units2, True))

                                        scontext.Append(CSVCell(.Item("AvgRSSI"), True))

                                    End If

                                Else

                                    scontext.Append(CSVCell(.Item("TagId"), True))
                                    scontext.Append(CSVCell(.Item("MonitorLocation").Replace(",", ""), True))
                                    scontext.Append(CSVCell(.Item("MonitorId"), True))

                                    If Bin = "4" Or Bin = "" Then
                                        scontext.Append(CSVCell(.Item("StarAddress"), True))
                                    End If

                                    If Bin = "0" Or Bin = "" Then
                                        scontext.Append(CSVCell(sgood, True))
                                    End If

                                    GetGoodIndicationValues(scontext, siteId, sgoodindication)

                                    If Bin = "1" Or Bin = "4" Or Bin = "" Then
                                        scontext.Append(CSVCell(s90DaysCell, True))
                                    End If

                                    If Bin = "2" Or Bin = "4" Or Bin = "" Then
                                        scontext.Append(CSVCell(s30daysCell, True))
                                    End If

                                    If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                        scontext.Append(CSVCell(.Item("MonitorLastSeen"), True))
                                    End If

                                    scontext.Append(CSVCell(.Item("LastSeen"), True))
                                    scontext.Append(CSVCell(.Item("ModelItem"), True))

                                    If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                        scontext.Append(CSVCell(offline, True))
                                    End If

                                    If Bin = "" Then
                                        scontext.Append(CSVCell(.Item("BatteryReplacementOn"), True))
                                    End If

                                    If Bin = "" And isUHFTags Then
                                        If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                            scontext.Append(CSVCell(.Item("CActivityLevel"), True))
                                        End If
                                    End If

                                End If

                            End With
                            scontext.Append(CSVNewLine())
                        Next
                    End If
                End If

                If DeviceType = 0 Then
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("MONITOR"))
                    scontext.Append(CSVNewLine())
                End If

                If DeviceType = enumDeviceType.Monitor Or DeviceType = 0 Then

                    If Not dtMonitor Is Nothing Then dt = dtMonitor

                    scontext.Append(CSVCell("Devices", True))
                    scontext.Append(CSVCell("Monitor ID", True))
                    scontext.Append(CSVCell("Monitor Location", True))

                    If g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.Maintenance Then
                        scontext.Append(CSVCell("Configured in Core", True))
                    End If

                    If Bin = "0" Or Bin = "" Or Bin = "3" Or Bin = "7" Then
                        scontext.Append(CSVCell("Good", True))
                    End If

                    If Bin = "1" Or Bin = "4" Or Bin = "" Or Bin = "3" Or Bin = "7" Then
                        scontext.Append(CSVCell("Change In 90 Days", True))
                    End If

                    If Bin = "2" Or Bin = "4" Or Bin = "" Or Bin = "3" Or Bin = "7" Then
                        scontext.Append(CSVCell("Change In 30 Days", True))
                    End If

                    scontext.Append(CSVCell("Date Last seen by Network", True))
                    scontext.Append(CSVCell("Model Item", True))

                    If Bin = "3" Or Bin = "" Then
                        scontext.Append(CSVCell("Offline", True))
                    End If

                    If Bin = "7" Then
                        scontext.Append(CSVCell("Offline (Battery Depleted)", True))
                    End If

                    If Bin = "" Then
                        scontext.Append(CSVCell("Battery Replaced On", True))
                    End If

                    scontext.Append(CSVNewLine())

                    If isMonitorDataFound Then
                        For nIdx As Integer = 0 To dt.Rows.Count - 1

                            With (dt.Rows(nIdx))

                                Dim sgoodCell As String = "No"
                                Dim s30daysCell As String = "No"
                                Dim s90DaysCell As String = "No"
                                Dim offline As String = ""
                                Dim CatastrophicCases As String = ""

                                CatastrophicCases = .Item("CatastrophicCases")
                                offline = .Item("offline")

                                If CatastrophicCases = "1" Then
                                    s30daysCell = "Yes"
                                ElseIf CatastrophicCases = "2" Then
                                    s90DaysCell = "Yes"
                                ElseIf CatastrophicCases = "0" Then
                                    sgoodCell = "Yes"
                                End If

                                If (offline = "1") Then
                                    offline = "Yes"
                                ElseIf (offline = "0") Then
                                    offline = "No"
                                End If

                                scontext.Append(CSVCell(.Item("DeviceName"), True))
                                scontext.Append(CSVCell(.Item("DeviceId"), True))
                                scontext.Append(CSVCell(.Item("RoomName").Replace(",", ""), True))

                                If g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.Maintenance Then
                                    scontext.Append(CSVCell(.Item("IsConfiguredinCore"), True))
                                End If

                                If Bin = "0" Or Bin = "" Or Bin = "3" Or Bin = "7" Then
                                    scontext.Append(CSVCell(sgoodCell, True))
                                End If

                                If Bin = "1" Or Bin = "4" Or Bin = "" Or Bin = "3" Or Bin = "7" Then
                                    scontext.Append(CSVCell(s90DaysCell, True))
                                End If

                                If Bin = "2" Or Bin = "4" Or Bin = "" Or Bin = "3" Or Bin = "7" Then
                                    scontext.Append(CSVCell(s30daysCell, True))
                                End If

                                scontext.Append(CSVCell(.Item("LastSeen"), True))
                                scontext.Append(CSVCell(.Item("ModelItem"), True))

                                If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                    scontext.Append(CSVCell(offline, True))
                                End If

                                If Bin = "" Then
                                    scontext.Append(CSVCell(.Item("BatteryReplacementOn"), True))
                                End If

                            End With

                            scontext.Append(CSVNewLine())

                        Next
                    End If

                End If

                If DeviceType = enumDeviceType.Star Then

                    scontext.Append(CSVCell("Star Id", True))
                    scontext.Append(CSVCell("Mac Id", True))
                    scontext.Append(CSVCell("Star Name", True))

                    If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                        scontext.Append(CSVCell("Star Type", True))
                        scontext.Append(CSVCell("IP Addr", True))
                        scontext.Append(CSVCell("Version", True))
                    End If

                    scontext.Append(CSVCell("Date Last Seen By Network", True))
                    scontext.Append(CSVCell("Model Item", True))
                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            scontext.Append(CSVCell(.Item("StarId"), True))
                            scontext.Append(CSVCell(.Item("MacId"), True))
                            scontext.Append(CSVCell(.Item("DeviceName").Replace(",", ""), True))

                            If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                scontext.Append(CSVCell(.Item("StarType"), True))
                                scontext.Append(CSVCell(.Item("IPAddr"), True))
                                scontext.Append(CSVCell(.Item("Version"), True))
                            End If

                            scontext.Append(CSVCell(.Item("LastReceivedTime"), True))
                            scontext.Append(CSVCell(.Item("ModelItem"), True))
                            scontext.Append(CSVNewLine())

                        End With
                    Next
                End If
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : MakeCSV_IE                                                                                                                              '
        ' Input         : Datatable , devicetype,Bin,Type                                                                                                      ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Sub MakeCSV_IE(ByVal dt As DataTable, ByVal DeviceType As Integer, ByVal Bin As String, ByVal Type As String, Optional ByVal dtMonitor As DataTable = Nothing, Optional ByVal PulseUIId As Integer = 0)

            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder

            Dim sFileName As String
            Dim Version As String
            Dim sScript As String = ""
            Dim Sheetname As String = "Centrak"

            Dim isTagdataFound As Boolean = True
            Dim isMonitorDataFound As Boolean = True

            Dim SiteId As Integer = 0
            Dim isUHFTags As Boolean = False

            Try

                If (Bin = "0") Then
                    Sheetname = "Good Device List"
                ElseIf (Bin = "1") Then
                    Sheetname = "UnderWatch List"
                ElseIf (Bin = "2") Then
                    Sheetname = "LBI List"
                ElseIf (Bin = "3") Then
                    Sheetname = "Offline List"
                ElseIf (Bin = "4") Then
                    Sheetname = "All"
                ElseIf (Bin = "") Then
                    Sheetname = "All"
                Else
                    Sheetname = ""
                End If

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                excl.InitiateCSV(Context, sFileName)

                If dt.Rows.Count > 0 Then

                    Dim context As HttpContext
                    context = HttpContext.Current

                    excl.AddCSVCell(context, "Site Name: " & dt.Rows(0).Item("Sitename").Replace(",", ""), True)
                    excl.AddCSVNewLine(context)

                    If DeviceType = enumDeviceType.Tag Then
                        isUHFTags = dt.Rows(0).Item("isUHFTags")
                        excl.AddCSVCell(context, "Type: " & dt.Rows(0).Item("TagType").Replace(",", ""), True)
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        excl.AddCSVCell(context, "Type: " & dt.Rows(0).Item("MonitorType").Replace(",", ""), True)
                    ElseIf DeviceType = enumDeviceType.Star Then
                        excl.AddCSVCell(context, "Type: " & dt.Rows(0).Item("StarType").Replace(",", ""), True)
                    ElseIf DeviceType = 0 Then
                        excl.AddCSVCell(context, "Type: All", True)
                    End If

                    excl.AddCSVNewLine(context)

                    If DeviceType = 0 Then
                        excl.AddCSVNewLine(context)
                        excl.AddCSVCell(context, "TAG")
                        excl.AddCSVNewLine(context)
                    End If

                    If DeviceType = enumDeviceType.Tag Or DeviceType = 0 Then

                        If IsEMTag(Type) Then

                            excl.AddCSVCell(context, "TAG ID", True)
                            excl.AddCSVCell(context, "P1 ID", True)
                            excl.AddCSVCell(context, "P2 ID", True)

                            If Bin = "0" Or Bin = "" Then
                                excl.AddCSVCell(context, "Good", True)
                            End If

                            GetGoodIndication_IE(excl, SiteId)

                            If Bin = "1" Or Bin = "4" Or Bin = "" Then
                                excl.AddCSVCell(context, "Less than 90 Days", True)
                            End If

                            If Bin = "2" Or Bin = "4" Or Bin = "" Then
                                excl.AddCSVCell(context, "Less than 30 Days", True)
                            End If

                            excl.AddCSVCell(context, "Local ID", True)
                            excl.AddCSVCell(context, "Cal Frequency", True)
                            excl.AddCSVCell(context, "Last Cal", True)
                            excl.AddCSVCell(context, "CenTrak Cal Due", True)
                            excl.AddCSVCell(context, "Client Cal Due", True)
                            excl.AddCSVCell(context, "Star Address", True)
                            excl.AddCSVCell(context, "Last Seen", True)
                            excl.AddCSVCell(context, "Model Item", True)
                            excl.AddCSVCell(context, "Location", True)

                            If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                excl.AddCSVCell(context, "Voltage", True)
                                excl.AddCSVCell(context, "% Batt", True)
                            End If

                            If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Or g_IsTempMonitoring = "True" Then
                                excl.AddCSVCell(context, "P1 Value", True)
                                excl.AddCSVCell(context, "P1 Units", True)
                                excl.AddCSVCell(context, "P2 Value", True)
                                excl.AddCSVCell(context, "P2 Units", True)

                                excl.AddCSVCell(context, "AVG RSSI", True)
                            End If

                        Else

                            excl.AddCSVCell(context, "TAG ID", True)
                            excl.AddCSVCell(context, "Monitor Location", True)
                            excl.AddCSVCell(context, "Monitor ID", True)

                            If Bin = "4" Or Bin = "" Then
                                excl.AddCSVCell(context, "Star Address", True)
                            End If

                            If Bin = "0" Or Bin = "" Then
                                excl.AddCSVCell(context, "Good", True)
                            End If

                            GetGoodIndication_IE(excl, SiteId)

                            If Bin = "1" Or Bin = "4" Or Bin = "" Then
                                excl.AddCSVCell(context, "Less than 90 Days", True)
                            End If

                            If Bin = "2" Or Bin = "4" Or Bin = "" Then
                                excl.AddCSVCell(context, "Less than 30 Days", True)
                            End If

                            If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                excl.AddCSVCell(context, "Date Last seen by Monitor", True)
                            End If

                            excl.AddCSVCell(context, "Date Last seen by Network", True)
                            excl.AddCSVCell(context, "Model Item", True)

                            If Bin = "3" Or Bin = "" Then
                                If g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.Maintenance Then
                                    excl.AddCSVCell(context, "Offline (Battery Depleted)", True)
                                Else
                                    excl.AddCSVCell(context, "Offline", True)
                                End If
                            End If

                            If Bin = "7" Then
                                excl.AddCSVCell(context, "Offline (Battery Depleted)", True)
                            End If

                            If Bin = "" Then
                                excl.AddCSVCell(context, "Battery Replaced On", True)
                            End If

                            If Bin = "" And isUHFTags Then
                                If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                    excl.AddCSVCell(context, "Cumulative Activity Level", True)
                                End If
                            End If

                        End If

                        excl.AddCSVNewLine(context)

                        If isTagdataFound Then
                            For nIdx As Integer = 0 To dt.Rows.Count - 1
                                With (dt.Rows(nIdx))

                                    Dim s30daysCell As String = "No"
                                    Dim s90DaysCell As String = "No"
                                    Dim sgood As String = "No"
                                    Dim sgoodindication As String = "No"
                                    Dim offline As String = ""
                                    Dim Units As String = ""

                                    Dim CatastrophicCases As String = ""
                                    Dim CCatastrophicCases As String = ""

                                    CCatastrophicCases = CheckIsDBNull(.Item("CCatastrophicCases"))
                                    CatastrophicCases = CheckIsDBNull(.Item("CatastrophicCases"))
                                    offline = CheckIsDBNull(.Item("offline"))
                                    Version = CheckIsDBNull(.Item("Version"))

                                    If CCatastrophicCases = "5" Then
                                        sgoodindication = "Yes"
                                    ElseIf (CatastrophicCases = "1" Or CatastrophicCases = "2") Then
                                        s30daysCell = "Yes"
                                    ElseIf (CatastrophicCases = "4") Then
                                        s90DaysCell = "Yes"
                                    ElseIf (CatastrophicCases = "0") Then
                                        sgood = "Yes"
                                    End If

                                    If (offline = "1") Then
                                        offline = "Yes"
                                    ElseIf (offline = "0") Then
                                        offline = "No"
                                    End If

                                    If IsEMTag(Type) Then

                                        excl.AddCSVCell(context, .Item("TagId"), True)
                                        excl.AddCSVCell(context, .Item("ProbeId"), True)
                                        excl.AddCSVCell(context, .Item("ProbeId2"), True)

                                        If Bin = "0" Or Bin = "" Then
                                            excl.AddCSVCell(context, sgood, True)
                                        End If

                                        GetGoodIndicationValuesIE(excl, SiteId, sgoodindication)

                                        If Bin = "1" Or Bin = "4" Or Bin = "" Then
                                            excl.AddCSVCell(context, s90DaysCell, True)
                                        End If

                                        If Bin = "2" Or Bin = "4" Or Bin = "" Then
                                            excl.AddCSVCell(context, s30daysCell, True)
                                        End If

                                        excl.AddCSVCell(context, .Item("LocalId").Replace(",", ""), True)
                                        excl.AddCSVCell(context, .Item("CalFrequency"), True)
                                        excl.AddCSVCell(context, .Item("CertificateDate"), True)
                                        excl.AddCSVCell(context, .Item("MFRCalibrationDue"), True)
                                        excl.AddCSVCell(context, .Item("ClientCalDue"), True)
                                        excl.AddCSVCell(context, .Item("StarAddress"), True)
                                        excl.AddCSVCell(context, .Item("LastSeen"), True)
                                        excl.AddCSVCell(context, .Item("ModelItem"), True)
                                        excl.AddCSVCell(context, .Item("Location").Replace(",", ""), True)

                                        If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                            excl.AddCSVCell(context, .Item("Voltage"), True)
                                            excl.AddCSVCell(context, .Item("BatteryCapacity"), True)
                                        End If

                                        If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Or g_IsTempMonitoring = "True" Then

                                            Dim Temp1 As String = .Item("Probe1Temperature")
                                            Dim Units1 As String = .Item("P1Units")

                                            If Units1 <> "" Then
                                                Units1 = Units1.Replace("�C", "Celsius")
                                            End If

                                            Dim Temp2 As String = .Item("Probe2Temperature")
                                            Dim Units2 As String = .Item("P2Units")

                                            If Units2 <> "" Then
                                                Units2 = Units2.Replace("�C", "Celsius")
                                            End If

                                            If Type = EnumFilterType.enum_G2TempTag Then
                                                excl.AddCSVCell(context, Temp1, True)
                                                excl.AddCSVCell(context, Units1, True)
                                            Else
                                                excl.AddCSVCell(context, .Item("Temperature"), True)
                                                excl.AddCSVCell(context, Units1, True)
                                            End If

                                            If Type = EnumFilterType.enum_G2TempTag Or Type = EnumFilterType.enum_AmbientTempRH Then

                                                If Type = EnumFilterType.enum_G2TempTag Then
                                                    excl.AddCSVCell(context, Temp2, True)
                                                    excl.AddCSVCell(context, Units2, True)
                                                Else
                                                    excl.AddCSVCell(context, .Item("Humidity"), True)
                                                    excl.AddCSVCell(context, Units2, True)
                                                End If

                                            End If

                                            excl.AddCSVCell(context, .Item("AvgRSSI"), True)

                                        End If
                                    Else

                                        excl.AddCSVCell(context, .Item("TagId"), True)
                                        excl.AddCSVCell(context, .Item("MonitorLocation").Replace(",", ""), True)
                                        excl.AddCSVCell(context, .Item("MonitorId"), True)

                                        If Bin = "4" Or Bin = "" Then
                                            excl.AddCSVCell(context, .Item("StarAddress"), True)
                                        End If

                                        If Bin = "0" Or Bin = "" Then
                                            excl.AddCSVCell(context, sgood, True)
                                        End If

                                        GetGoodIndicationValuesIE(excl, SiteId, sgoodindication)

                                        If Bin = "1" Or Bin = "4" Or Bin = "" Then
                                            excl.AddCSVCell(context, s90DaysCell, True)
                                        End If

                                        If Bin = "2" Or Bin = "4" Or Bin = "" Then
                                            excl.AddCSVCell(context, s30daysCell, True)
                                        End If

                                        If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                            excl.AddCSVCell(context, .Item("MonitorLastSeen"), True)
                                        End If

                                        excl.AddCSVCell(context, .Item("LastSeen"), True)
                                        excl.AddCSVCell(context, .Item("ModelItem"), True)

                                        If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                            excl.AddCSVCell(context, offline, True)
                                        End If

                                        If Bin = "" Then
                                            excl.AddCSVCell(context, .Item("BatteryReplacementOn"), True)
                                        End If

                                        If Bin = "" And isUHFTags Then
                                            If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then
                                                excl.AddCSVCell(context, .Item("CActivityLevel"), True)
                                            End If
                                        End If

                                    End If
                                End With

                                excl.AddCSVNewLine(context)

                            Next
                        End If
                    End If

                    If DeviceType = 0 Then
                        excl.AddCSVNewLine(context)
                        excl.AddCSVCell(context, "MONITOR")
                        excl.AddCSVNewLine(context)
                    End If

                    If DeviceType = enumDeviceType.Monitor Or DeviceType = 0 Then

                        If Not dtMonitor Is Nothing Then dt = dtMonitor

                        excl.AddCSVCell(context, "Devices", True)
                        excl.AddCSVCell(context, "Monitor ID", True)
                        excl.AddCSVCell(context, "Monitor Location", True)

                        If g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.Maintenance Then
                            excl.AddCSVCell(context, "Configured in Core", True)
                        End If

                        If Bin = "0" Or Bin = "" Or Bin = "3" Then
                            excl.AddCSVCell(context, "Good", True)
                        End If

                        If Bin = "1" Or Bin = "4" Or Bin = "" Or Bin = "3" Then
                            excl.AddCSVCell(context, "Change In 90 Days", True)
                        End If

                        If Bin = "2" Or Bin = "4" Or Bin = "" Or Bin = "3" Then
                            excl.AddCSVCell(context, "Change In 30 Days", True)
                        End If

                        excl.AddCSVCell(context, "Date Last seen by Network", True)
                        excl.AddCSVCell(context, "Model Item", True)

                        If Bin = "3" Or Bin = "" Then
                            excl.AddCSVCell(context, "Offline", True)
                        End If

                        If Bin = "7" Then
                            excl.AddCSVCell(context, "Offline (Battery Depleted)", True)
                        End If

                        If Bin = "" Then
                            excl.AddCSVCell(context, "Battery Replaced On", True)
                        End If

                        excl.AddCSVNewLine(context)

                        If isMonitorDataFound Then
                            For nIdx As Integer = 0 To dt.Rows.Count - 1
                                With (dt.Rows(nIdx))

                                    Dim sgoodCell As String = "No"
                                    Dim s30daysCell As String = "No"
                                    Dim s90DaysCell As String = "No"
                                    Dim offline As String = ""
                                    Dim CatastrophicCases As String = ""

                                    CatastrophicCases = .Item("CatastrophicCases")
                                    offline = .Item("offline")

                                    If CatastrophicCases = "1" Then
                                        s30daysCell = "Yes"
                                    ElseIf CatastrophicCases = "2" Then
                                        s90DaysCell = "Yes"
                                    ElseIf CatastrophicCases = "0" Then
                                        sgoodCell = "Yes"
                                    End If

                                    If (offline = "1") Then
                                        offline = "Yes"
                                    ElseIf (offline = "0") Then
                                        offline = "No"
                                    End If

                                    excl.AddCSVCell(context, .Item("DeviceName"), True)
                                    excl.AddCSVCell(context, .Item("DeviceId"), True)
                                    excl.AddCSVCell(context, .Item("RoomName").Replace(",", ""), True)

                                    If g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.Maintenance Then
                                        excl.AddCSVCell(context, .Item("IsConfiguredinCore"), True)
                                    End If

                                    If Bin = "0" Or Bin = "" Or Bin = "3" Then
                                        excl.AddCSVCell(context, sgoodCell, True)
                                    End If

                                    If Bin = "1" Or Bin = "4" Or Bin = "" Or Bin = "3" Then
                                        excl.AddCSVCell(context, s90DaysCell, True)
                                    End If

                                    If Bin = "2" Or Bin = "4" Or Bin = "" Or Bin = "3" Then
                                        excl.AddCSVCell(context, s30daysCell, True)
                                    End If

                                    excl.AddCSVCell(context, .Item("LastSeen"), True)
                                    excl.AddCSVCell(context, .Item("ModelItem"), True)

                                    If Bin = "3" Or Bin = "" Or Bin = "7" Then
                                        excl.AddCSVCell(context, offline, True)
                                    End If

                                    If Bin = "" Then
                                        excl.AddCSVCell(context, .Item("BatteryReplacementOn"), True)
                                    End If

                                End With

                                excl.AddCSVNewLine(context)

                            Next
                        End If
                    End If

                    If DeviceType = enumDeviceType.Star Then

                        excl.AddCSVCell(context, "Star Id", True)
                        excl.AddCSVCell(context, "Mac Id", True)
                        excl.AddCSVCell(context, "Star Name", True)

                        If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                            excl.AddCSVCell(context, "Star Type", True)
                            excl.AddCSVCell(context, "IP Addr", True)
                            excl.AddCSVCell(context, "Version", True)
                        End If

                        excl.AddCSVCell(context, "Date Last Seen By Network", True)
                        excl.AddCSVCell(context, "Model Item", True)
                        excl.AddCSVNewLine(context)

                        For nIdx As Integer = 0 To dt.Rows.Count - 1
                            With (dt.Rows(nIdx))

                                excl.AddCSVCell(context, .Item("StarId"), True)
                                excl.AddCSVCell(context, .Item("MacId"), True)
                                excl.AddCSVCell(context, .Item("DeviceName").Replace(",", ""), True)

                                If g_UserRole <> enumUserRole.Maintenance And g_UserRole <> enumUserRole.Customer And g_UserRole <> enumUserRole.TechnicalAdmin Then
                                    excl.AddCSVCell(context, .Item("StarType"), True)
                                    excl.AddCSVCell(context, .Item("IPAddr"), True)
                                    excl.AddCSVCell(context, .Item("Version"), True)
                                End If

                                excl.AddCSVCell(context, .Item("LastReceivedTime"), True)
                                excl.AddCSVCell(context, .Item("ModelItem"), True)
                                excl.AddCSVNewLine(context)

                            End With
                        Next
                    End If
                End If

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception MakeCSV_IE file " & ex.Message.ToString())
            End Try

        End Sub

        '******************************************************************************************************************************************************'
        ' Function Name : MakeCSVForSearchPage                                                                                                                 '
        ' Input         : Datatable , devicetype                                                                                                               ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Function MakeCSV_GlobalSearch(ByVal dt As DataTable, ByVal DeviceType As Integer) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Search"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If (dt.Rows.Count > 0) Then

                If DeviceType = enumDeviceType.Tag Then
                    scontext.Append(CSVCell("Device Type: Tag"))
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    scontext.Append(CSVCell("Device Type: Monitor"))
                ElseIf DeviceType = enumDeviceType.Star Then
                    scontext.Append(CSVCell("Device Type: Star"))
                End If

                scontext.Append(CSVNewLine())

                If DeviceType = enumDeviceType.Tag Then

                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("Tag Id", True))
                    scontext.Append(CSVCell("Monitor Location", True))
                    scontext.Append(CSVCell("Model Number", True))
                    scontext.Append(CSVCell("Good", True))

                    GetGoodIndication(scontext, siteId)

                    scontext.Append(CSVCell("Less than 90 Days", True))
                    scontext.Append(CSVCell("LESS than 30 Days", True))
                    scontext.Append(CSVCell("Offline", True))

                    If (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support) Then
                        scontext.Append(CSVCell("Cumulative Activity Level", True))
                    End If

                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            scontext.Append(CSVCell(.Item("Sitename").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("TagId"), True))
                            scontext.Append(CSVCell(.Item("MonitorLocation").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("ModelItem"), True))

                            Dim s30daysCell As String = "No"
                            Dim s90DaysCell As String = "No"
                            Dim sgood As String = "No"
                            Dim offline As String = ""
                            Dim sgoodindication As String = "No"

                            Dim CatastrophicCases As String = ""
                            Dim CCatastrophicCases As String = ""

                            CCatastrophicCases = CheckIsDBNull(.Item("CCatastrophicCases"))
                            CatastrophicCases = CheckIsDBNull(.Item("CatastrophicCases"))
                            offline = CheckIsDBNull(.Item("offline"))

                            'CCatastrophicCases
                            If (CCatastrophicCases = "5") Then
                                sgoodindication = "Yes"
                            ElseIf (CatastrophicCases = "1" Or CatastrophicCases = "2") Then
                                s30daysCell = "Yes"
                            ElseIf (CatastrophicCases = "4") Then
                                s90DaysCell = "Yes"
                            ElseIf (CatastrophicCases = "0") Then
                                sgood = "Yes"
                            End If

                            If (offline = "1") Then
                                offline = "Yes"
                            ElseIf (offline = "0") Then
                                offline = "No"
                            End If

                            scontext.Append(CSVCell(sgood, True))

                            GetGoodIndicationValues(scontext, siteId, sgoodindication)

                            scontext.Append(CSVCell(s90DaysCell, True))
                            scontext.Append(CSVCell(s30daysCell, True))
                            scontext.Append(CSVCell(offline, True))

                            If (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support) Then
                                scontext.Append(CSVCell(.Item("CActivityLevel"), True))
                            End If

                        End With

                        scontext.Append(CSVNewLine())

                    Next
                End If

                If DeviceType = enumDeviceType.Monitor Then

                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("Monitor Id", True))
                    scontext.Append(CSVCell("Monitor Location", True))
                    scontext.Append(CSVCell("Model Number", True))
                    scontext.Append(CSVCell("Good", True))
                    scontext.Append(CSVCell("Less than 90 Days", True))
                    scontext.Append(CSVCell("Less than 30 Days", True))
                    scontext.Append(CSVCell("Offline", True))

                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            scontext.Append(CSVCell(.Item("Sitename").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("DeviceId"), True))
                            scontext.Append(CSVCell(.Item("RoomName").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("ModelItem"), True))

                            Dim s30daysCell As String = "No"
                            Dim s90DaysCell As String = "No"
                            Dim sgood As String = "No"
                            Dim offline As String = ""

                            Dim CatastrophicCases As String = ""

                            CatastrophicCases = .Item("CatastrophicCases")
                            offline = .Item("offline")

                            'CatastrophicCases
                            If (CatastrophicCases = "1") Then
                                s30daysCell = "Yes"
                            ElseIf (CatastrophicCases = "2") Then
                                s90DaysCell = "Yes"
                            ElseIf (CatastrophicCases = "0") Then
                                sgood = "Yes"
                            End If

                            If (offline = "1") Then
                                offline = "Yes"
                            ElseIf (offline = "0") Then
                                offline = "No"
                            End If

                            scontext.Append(CSVCell(sgood, True))
                            scontext.Append(CSVCell(s90DaysCell, True))
                            scontext.Append(CSVCell(s30daysCell, True))
                            scontext.Append(CSVCell(offline, True))

                        End With

                        scontext.Append(CSVNewLine())

                    Next
                End If

                If DeviceType = enumDeviceType.Star Then

                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("Mac Id", True))
                    scontext.Append(CSVCell("Star Type", True))
                    scontext.Append(CSVCell("IP", True))
                    scontext.Append(CSVCell("Model Number"))
                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            scontext.Append(CSVCell(.Item("Sitename").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("MacId"), True))
                            scontext.Append(CSVCell(.Item("StarType"), True))
                            scontext.Append(CSVCell(.Item("IPAddr"), True))
                            scontext.Append(CSVCell(.Item("ModelItem")))
                            scontext.Append(CSVNewLine())
                        End With
                    Next

                End If
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : MakeCSV_GlobalSearch                                                                                                                              '
        ' Input         : Datatable , devicetype,Bin,Type                                                                                                      ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Sub MakeCSV_IE_GlobalSearch(ByVal dt As DataTable, ByVal DeviceType As Integer)

            Try

                Dim excl As New Excelcreate
                Dim Csvtext As StringBuilder = New StringBuilder

                Dim sFileName As String = ""
                Dim sScript As String = ""
                Dim Sheetname As String = "Centrak"

                Dim dtExcel As New DataTable

                Sheetname = "Search"

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                excl.InitiateCSV(Context, sFileName)

                If (dt.Rows.Count > 0) Then

                    Dim context As HttpContext
                    context = HttpContext.Current

                    If DeviceType = enumDeviceType.Tag Then
                        excl.AddCSVCell(context, "Device Type: Tag", True)
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        excl.AddCSVCell(context, "Device Type: Monitor", True)
                    ElseIf DeviceType = enumDeviceType.Star Then
                        excl.AddCSVCell(context, "Device Type: Star", True)
                    End If

                    excl.AddCSVNewLine(context)

                    If DeviceType = enumDeviceType.Tag Then

                        excl.AddCSVCell(context, "Site Name", True)
                        excl.AddCSVCell(context, "Tag Id", True)
                        excl.AddCSVCell(context, "Monitor Location", True)
                        excl.AddCSVCell(context, "Model Number", True)
                        excl.AddCSVCell(context, "Good", True)

                        GetGoodIndication_IE(excl, siteId)

                        excl.AddCSVCell(context, "Less than 90 Days", True)
                        excl.AddCSVCell(context, "Less than 30 Days", True)
                        excl.AddCSVCell(context, "Offline", True)

                        If (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support) Then
                            excl.AddCSVCell(context, "Cumulative Activity Level", True)
                        End If

                        excl.AddCSVNewLine(context)

                        For nIdx As Integer = 0 To dt.Rows.Count - 1
                            With (dt.Rows(nIdx))

                                excl.AddCSVCell(context, .Item("Sitename").Replace(",", ""), True)
                                excl.AddCSVCell(context, .Item("TagId"), True)
                                excl.AddCSVCell(context, .Item("MonitorLocation").Replace(",", ""), True)
                                excl.AddCSVCell(context, .Item("ModelItem"), True)

                                Dim s30daysCell As String = "No"
                                Dim s90DaysCell As String = "No"
                                Dim sgood As String = "No"
                                Dim sgoodindication As String = "No"
                                Dim offline As String = ""

                                Dim CatastrophicCases As String = ""
                                Dim CCatastrophicCases As String = ""

                                CCatastrophicCases = .Item("CCatastrophicCases")
                                CatastrophicCases = .Item("CatastrophicCases")
                                offline = .Item("offline")

                                If (CCatastrophicCases = "5") Then
                                    sgoodindication = "Yes"
                                ElseIf (CatastrophicCases = "1" Or CatastrophicCases = "2") Then
                                    s30daysCell = "Yes"
                                ElseIf (CatastrophicCases = "4") Then
                                    s90DaysCell = "Yes"
                                ElseIf (CatastrophicCases = "0") Then
                                    sgood = "Yes"
                                End If

                                If (offline = "1") Then
                                    offline = "Yes"
                                ElseIf (offline = "0") Then
                                    offline = "No"
                                End If

                                excl.AddCSVCell(context, sgood, True)

                                GetGoodIndicationValuesIE(excl, siteId, sgoodindication)

                                excl.AddCSVCell(context, s90DaysCell, True)
                                excl.AddCSVCell(context, s30daysCell, True)
                                excl.AddCSVCell(context, offline, True)

                                If (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support) Then
                                    excl.AddCSVCell(context, .Item("CActivityLevel"), True)
                                End If

                            End With

                            excl.AddCSVNewLine(context)
                        Next
                    End If

                    If DeviceType = enumDeviceType.Monitor Then

                        excl.AddCSVCell(context, "Site Name", True)
                        excl.AddCSVCell(context, "Monitor Id", True)
                        excl.AddCSVCell(context, "Monitor Location", True)
                        excl.AddCSVCell(context, "Model Number", True)
                        excl.AddCSVCell(context, "Good", True)
                        excl.AddCSVCell(context, "Less Less 90 Days", True)
                        excl.AddCSVCell(context, "Less Less 30 Days", True)
                        excl.AddCSVCell(context, "Offline", True)

                        excl.AddCSVNewLine(context)

                        For nIdx As Integer = 0 To dt.Rows.Count - 1
                            With (dt.Rows(nIdx))

                                excl.AddCSVCell(context, .Item("Sitename").Replace(",", ""), True)
                                excl.AddCSVCell(context, .Item("DeviceId"), True)
                                excl.AddCSVCell(context, .Item("RoomName").Replace(",", ""), True)
                                excl.AddCSVCell(context, .Item("ModelItem"), True)

                                Dim s30daysCell As String = "No"
                                Dim s90DaysCell As String = "No"
                                Dim sgood As String = "No"
                                Dim offline As String = ""

                                Dim CatastrophicCases As String = ""

                                CatastrophicCases = .Item("CatastrophicCases")
                                offline = .Item("offline")

                                'CatastrophicCases
                                If CatastrophicCases = "1" Then
                                    s30daysCell = "Yes"
                                ElseIf CatastrophicCases = "2" Then
                                    s90DaysCell = "Yes"
                                ElseIf CatastrophicCases = "0" Then
                                    sgood = "Yes"
                                End If

                                If offline = "1" Then
                                    offline = "Yes"
                                ElseIf offline = "0" Then
                                    offline = "No"
                                End If

                                excl.AddCSVCell(context, sgood, True)
                                excl.AddCSVCell(context, s90DaysCell, True)
                                excl.AddCSVCell(context, s30daysCell, True)
                                excl.AddCSVCell(context, offline, True)

                            End With

                            excl.AddCSVNewLine(context)

                        Next
                    End If

                    If DeviceType = enumDeviceType.Star Then

                        excl.AddCSVCell(context, "Site Name", True)
                        excl.AddCSVCell(context, "Mac Id", True)
                        excl.AddCSVCell(context, "Star Type", True)
                        excl.AddCSVCell(context, "IP", True)
                        excl.AddCSVCell(context, "Model Number")
                        excl.AddCSVNewLine(context)

                        For nIdx As Integer = 0 To dt.Rows.Count - 1
                            With (dt.Rows(nIdx))

                                excl.AddCSVCell(context, .Item("Sitename").Replace(",", ""), True)
                                excl.AddCSVCell(context, .Item("MacId"), True)
                                excl.AddCSVCell(context, .Item("StarType"), True)
                                excl.AddCSVCell(context, .Item("IPAddr"), True)
                                excl.AddCSVCell(context, .Item("ModelItem"))
                                excl.AddCSVNewLine(context)

                            End With
                        Next
                    End If
                End If

                Context.Response.End()

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" PrepareCSVForSearchPage " & ex.Message.ToString())
            End Try

        End Sub

        '******************************************************************************************************'
        ' Function Name : SendXMLToAJAXCall                                                                    '
        ' Input         : DataTable                                                                            ' 
        ' Output        : XML String                                                                           '
        ' Description   : It will return XML to ajax calls from aspx client side pages                         '
        '******************************************************************************************************'
        ''' <summary> Function to return XML to ajax calls from aspx client side pages</summary>
        ''' <param name="dtReturn"></param>
        Private Sub SendXMLToAJAXCall(ByVal dtReturn As DataTable)
            Dim dsReturn As New DataSet

            dsReturn.Merge(dtReturn)
            Response.Clear()
            Response.ContentType = "text/xml"
            Response.CacheControl = "no-cache"
            Response.Write(dsReturn.GetXml())
            Response.End()
        End Sub


        '******************************************************************************************************************************************************'
        ' Function Name : SendXMLToAJAXCall                                                                                                                    '
        ' Input         : dtReturn1,dtReturn2                                                                                                                  ' 
        ' Output        : XML String                                                                                                                           '
        ' Description   : It will Merge the two datatable in data set and it will return XML to ajax calls from aspx client side pages                         '
        '******************************************************************************************************************************************************'
        ''' <summary> Function to return XML to ajax calls from aspx client side pages</summary>
        ''' <param name="dtReturn1"></param>
        ''' <param name="dtReturn2"></param>
        Private Sub SendXMLToAJAXCall(ByVal dtReturn1 As DataTable, ByVal dtReturn2 As DataTable)
            Dim dsReturn As New DataSet

            dsReturn.Merge(dtReturn1)
            dsReturn.Merge(dtReturn2)

            dsReturn.Tables(0).TableName = "Request"
            dsReturn.Tables(1).TableName = "Response"

            Response.Clear()
            Response.ContentType = "text/xml"
            Response.CacheControl = "no-cache"
            Response.Write(dsReturn.GetXml())
            Response.End()
        End Sub

        '******************************************************************************************************************************************************'
        ' Function Name : SendXMLToAJAXCall                                                                                                                    '
        ' Input         : dtReturn1,dtReturn2,dtReturn3                                                                                                        ' 
        ' Output        : XML String                                                                                                                           '
        ' Description   : It will Merge the two datatable in data set and it will return XML to ajax calls from aspx client side pages                         '
        '******************************************************************************************************************************************************'
        ''' <summary> Function to return XML to ajax calls from aspx client side pages</summary>
        ''' <param name="dtReturn1"></param>
        ''' <param name="dtReturn2"></param>
        Private Sub SendXMLToAJAXCall(ByVal dtReturn1 As DataTable, ByVal dtReturn2 As DataTable, ByVal dtReturn3 As DataTable)
            Dim dsReturn As New DataSet

            dsReturn.Merge(dtReturn1)
            dsReturn.Merge(dtReturn2)
            dsReturn.Merge(dtReturn3)

            Response.Clear()
            Response.ContentType = "text/xml"
            Response.CacheControl = "no-cache"
            Response.Write(dsReturn.GetXml())
            Response.End()
        End Sub

        '******************************************************************************************************'
        ' Function Name : SendXMLToAJAXCall                                                                    '
        ' Input         : DataTable                                                                            ' 
        ' Output        : XML String                                                                           '
        ' Description   : It will return XML to ajax calls from aspx client side pages                         '
        '******************************************************************************************************'
        ''' <summary> Function to return XML to ajax calls from aspx client side pages</summary>
        ''' <param name="dsReturn"></param>
        Private Sub SendXMLToAJAXCall(ByVal dsReturn As DataSet)
            Response.Clear()
            Response.ContentType = "text/xml"
            Response.CacheControl = "no-cache"
            Response.Write(dsReturn.GetXml())
            Response.End()
        End Sub

        '**********************************************************************************'
        ' Function Name : AddColumnsToTable                                                '
        ' Input         : dtReq - new datatable                                            ' 
        ' Output        : Custom colum added in to datatable                               '
        ' Description   : It will Add the new column in to datatable                       '
        '**********************************************************************************'
        Private Sub AddColumnsToTable(ByRef dtReq As DataTable)
            dtReq.Columns.Add(New DataColumn("respCount", Type.[GetType]("System.Int32")))
            dtReq.Columns.Add(New DataColumn("failed", Type.[GetType]("System.Int32")))
            dtReq.Columns.Add(New DataColumn("avgTime", Type.[GetType]("System.Double")))
            dtReq.Columns.Add(New DataColumn("timeBefore", Type.[GetType]("System.String")))
            dtReq.Columns.Add(New DataColumn("timeAfter", Type.[GetType]("System.String")))
            dtReq.Columns.Add(New DataColumn("recordCount", Type.[GetType]("System.Int32")))
        End Sub
        '******************************************************************************************************************************************************'
        ' Function Name : FillDataTable                                                                                                                        '
        ' Input         : dtReq - new datatable ,respCount,  failed  ,timeBefore ,timeAfter,dtResp                                                             ' 
        ' Output        : Custom colum added in to datatable                                                                                                   '
        ' Description   : It will Add the new column  and it values in to datatable                                                                            '
        '******************************************************************************************************************************************************'
        Private Sub FillDataTable(ByRef dtReq As DataTable, ByVal respCount As Integer, ByVal failed As Integer, ByVal timeBefore As Date, ByVal timeAfter As Date, ByVal dtResp As DataTable)
            Dim drReq As DataRow
            Dim tsDiff As TimeSpan = timeAfter - timeBefore

            drReq = dtReq.NewRow()
            drReq("respCount") = respCount
            drReq("failed") = failed
            drReq("avgTime") = Format(tsDiff.TotalSeconds, "#.000")
            drReq("timeBefore") = timeBefore.ToString("dd/MM/yyyy HH:mm:ss.fff")
            drReq("timeAfter") = timeAfter.ToString("dd/MM/yyyy HH:mm:ss.fff")
            drReq("recordCount") = dtResp.Rows.Count
            dtReq.Rows.Add(drReq)

            SendXMLToAJAXCall(dtReq, dtResp)
        End Sub
        '******************************************************************************************************************************************************'
        ' Function Name : RequestTestbed                                                                                                                       '
        ' Input         : Url ,respCount,  refResponseFromServer                                                                                               ' 
        ' Output        : Server Response as XML String                                                                                                        '
        ' Description   : Requesting web service based on URL Mehtod                                                                                           '
        '******************************************************************************************************************************************************'
        Private Sub RequestTestbed(ByVal Url As String, ByRef refResponseFromServer As String)

            Try
                Dim requestSiteSummary As WebRequest = WebRequest.Create(Url)

                requestSiteSummary.Timeout = 60000
                requestSiteSummary.CachePolicy = New Net.Cache.RequestCachePolicy(Net.Cache.RequestCacheLevel.NoCacheNoStore)

                Dim response As WebResponse = requestSiteSummary.GetResponse()
                Dim dataStream As Stream = response.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                Dim responseFromServer As String = reader.ReadToEnd()

                refResponseFromServer = responseFromServer

                reader.Close()
                response.Close()
            Catch ex As Exception

            End Try

        End Sub
        '******************************************************************************************************************************************************'
        ' Function Name : LoadSiteSummaryXMLintoTable                                                                                                          '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable  for SiteSummary                                                                               '
        '******************************************************************************************************************************************************'

        Private Function LoadSiteSummaryXMLintoTable(ByVal responseFromServer As String) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing
            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("System", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TagLbiCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TagUnderWatchCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("InfrastructureLbiCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("InfrastructureTagUnderWatchCount", Type.[GetType]("System.Int32")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.GetElementsByTagName("SiteDetails")

                For nElemIdx = 1 To elemList.Count
                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(nElemIdx).Item("SiteId").ChildNodes(0).InnerText
                    drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(nElemIdx).Item("SiteName").ChildNodes(0).InnerText)
                    drResp("System") = root.ChildNodes(nElemIdx).Item("System").ChildNodes(0).InnerText
                    drResp("TagLbiCount") = root.ChildNodes(nElemIdx).Item("Tag").Item("LbiCount").InnerText
                    drResp("TagUnderWatchCount") = root.ChildNodes(nElemIdx).Item("Tag").Item("UnderWatchCount").InnerText
                    drResp("InfrastructureLbiCount") = root.ChildNodes(nElemIdx).Item("Infrastructure").Item("LbiCount").InnerText
                    drResp("InfrastructureTagUnderWatchCount") = root.ChildNodes(nElemIdx).Item("Infrastructure").Item("UnderWatchCount").InnerText
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception
            End Try

            Return dtResp
        End Function
        '******************************************************************************************************************************************************'
        ' Function Name : LoadDetailedOverviewXMLintoTable                                                                                                     '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Detailed Over View                                                                        '
        '******************************************************************************************************************************************************'

        Private Function LoadDetailedOverviewXMLintoTable(ByVal responseFromServer As String) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nSiteId As Integer = 0
            Dim sSiteName As String = ""
            Dim nElemIdx As Integer = 0
            Dim nDeviceTypeIdx As Integer = 4

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("GroupId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("GroupName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Good", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen180Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen90Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen30Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.Int32")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.ChildNodes(0).SelectNodes("Device")

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(0).Item("SiteId").InnerText
                    drResp("SiteName") = root.ChildNodes(0).Item("SiteName").InnerText
                    drResp("DeviceType") = elemList.Item(nElemIdx).Item("DeviceType").InnerText
                    drResp("GroupId") = elemList.Item(nElemIdx).Item("TypeId").InnerText
                    drResp("GroupName") = elemList.Item(nElemIdx).Item("Type").InnerText
                    drResp("Good") = elemList.Item(nElemIdx).Item("Good").InnerText
                    drResp("LessThen180Days") = elemList.Item(nElemIdx).Item("LessThen180Days").InnerText
                    drResp("LessThen90Days") = elemList.Item(nElemIdx).Item("LessThen90Days").InnerText
                    drResp("LessThen30Days") = elemList.Item(nElemIdx).Item("LessThen30Days").InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("OffLine").InnerText
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception
            End Try

            Return dtResp
        End Function
        '******************************************************************************************************************************************************'
        ' Function Name : LoadDeviceProfileXMLintoTable                                                                                                        '
        ' Input         : XML String , DeviceType                                                                                                              ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Device Profile information                                                                '
        '******************************************************************************************************************************************************'

        Private Function LoadDeviceProfileXMLintoTable(ByVal responseFromServer As String, ByVal DeviceType As Integer) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            If DeviceType = enumDeviceType.Tag Then
                dtResp.Columns.Add(New DataColumn("TagId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("TagType", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Profile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRProfile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRReportingTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("NoIRReportingTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRRXValue", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LFRegister", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MotionSensorScanLogic", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("EnableFastPushbutton", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LFRX", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PagingProfile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("OperationMode", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("WiFiReportRates", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("EnableWifiin900MHZ", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("AlertDelay", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TagClass", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MinimumTemp", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MaximumTemp", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("HighTemp", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LowTemp", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TemperatureReportRate", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocalAlert", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("X2L", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("XL", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IPL", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LongIROpen", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probes", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probe1AlertMin", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probe1AlertMax", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probe2AlertMin", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probe2AlertMax", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ShipDate", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("BuildingName", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("CampusName", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("FloorName", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("DoorAjarDetection", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probe1Temperature", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Probe2Temperature", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("WiFiDataCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LastWiFiDataReceivedTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("AlertType", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Temperature", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Humidity", Type.[GetType]("System.String")))

            ElseIf DeviceType = enumDeviceType.Monitor Then
                dtResp.Columns.Add(New DataColumn("MonitorId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("RoomId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MonitorType", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Profile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRProfile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PowerLevel", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("RoomBleeding", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("NoiseLevel", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Masking", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MasterSlave", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("SpecialProfile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("OperatingMode", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Modes", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("AlertSupressionTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ShipDate", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("BuildingName", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("CampusName", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("FloorName", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("WiFiDataCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LastWiFiDataReceivedTime", Type.[GetType]("System.String")))
            ElseIf DeviceType = enumDeviceType.Star Then
                dtResp.Columns.Add(New DataColumn("MacId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("StarType", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("DHCP", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("SaveSettings", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("StaticIP", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Subnet", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Gateway", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TimeServerIP", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ServerIP", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PagingServerIP", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationServerIP1", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationServerIP2", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("SuperSyncProfile", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LockedStarId", Type.[GetType]("System.Int32")))
            End If

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("Record")

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    If DeviceType = enumDeviceType.Tag Then
                        drResp("TagId") = root.ChildNodes(nElemIdx).Item("TagId").InnerText
                        drResp("TagType") = root.ChildNodes(nElemIdx).Item("TagType").InnerText
                        drResp("Profile") = root.ChildNodes(nElemIdx).Item("Profile").InnerText
                        drResp("IRProfile") = root.ChildNodes(nElemIdx).Item("IRProfile").InnerText
                        drResp("IRReportingTime") = root.ChildNodes(nElemIdx).Item("IRReportingTime").InnerText
                        drResp("NoIRReportingTime") = root.ChildNodes(nElemIdx).Item("NoIRReportingTime").InnerText
                        drResp("IRRXValue") = root.ChildNodes(nElemIdx).Item("IRRXValue").InnerText
                        drResp("LFRegister") = root.ChildNodes(nElemIdx).Item("LFRegister").InnerText
                        drResp("MotionSensorScanLogic") = root.ChildNodes(nElemIdx).Item("MotionSensorScanLogic").InnerText
                        drResp("EnableFastPushbutton") = root.ChildNodes(nElemIdx).Item("EnableFastPushbutton").InnerText
                        drResp("LFRX") = root.ChildNodes(nElemIdx).Item("LFRX").InnerText
                        drResp("PagingProfile") = root.ChildNodes(nElemIdx).Item("PagingProfile").InnerText
                        drResp("OperationMode") = root.ChildNodes(nElemIdx).Item("OperationMode").InnerText
                        drResp("WiFiReportRates") = root.ChildNodes(nElemIdx).Item("WiFiReportRates").InnerText
                        drResp("EnableWifiin900MHZ") = root.ChildNodes(nElemIdx).Item("EnableWifiin900MHZ").InnerText
                        drResp("AlertDelay") = root.ChildNodes(nElemIdx).Item("AlertDelay").InnerText
                        drResp("TagClass") = root.ChildNodes(nElemIdx).Item("TagClass").InnerText
                        drResp("MinimumTemp") = root.ChildNodes(nElemIdx).Item("MinimumTemp").InnerText
                        drResp("MaximumTemp") = root.ChildNodes(nElemIdx).Item("MaximumTemp").InnerText
                        drResp("HighTemp") = root.ChildNodes(nElemIdx).Item("HighTemp").InnerText
                        drResp("LowTemp") = root.ChildNodes(nElemIdx).Item("LowTemp").InnerText
                        drResp("TemperatureReportRate") = root.ChildNodes(nElemIdx).Item("TemperatureReportRate").InnerText
                        drResp("LocalAlert") = root.ChildNodes(nElemIdx).Item("LocalAlert").InnerText
                        drResp("X2L") = root.ChildNodes(nElemIdx).Item("X2L").InnerText
                        drResp("XL") = root.ChildNodes(nElemIdx).Item("XL").InnerText
                        drResp("IPL") = root.ChildNodes(nElemIdx).Item("IPL").InnerText
                        drResp("LongIROpen") = root.ChildNodes(nElemIdx).Item("LongIROpen").InnerText
                        drResp("Probes") = root.ChildNodes(nElemIdx).Item("Probes").InnerText
                        drResp("Probe1AlertMin") = root.ChildNodes(nElemIdx).Item("Probe1AlertMin").InnerText
                        drResp("Probe1AlertMax") = root.ChildNodes(nElemIdx).Item("Probe1AlertMax").InnerText
                        drResp("Probe2AlertMin") = root.ChildNodes(nElemIdx).Item("Probe2AlertMin").InnerText
                        drResp("Probe2AlertMax") = root.ChildNodes(nElemIdx).Item("Probe2AlertMax").InnerText
                        drResp("ModelItem") = root.ChildNodes(nElemIdx).Item("ModelItem").InnerText
                        drResp("ShipDate") = root.ChildNodes(nElemIdx).Item("ShipDate").InnerText

                        drResp("BuildingName") = root.ChildNodes(nElemIdx).Item("BuildingName").InnerText
                        drResp("CampusName") = root.ChildNodes(nElemIdx).Item("CampusName").InnerText
                        drResp("FloorName") = root.ChildNodes(nElemIdx).Item("FloorName").InnerText
                        drResp("Probe1Temperature") = root.ChildNodes(nElemIdx).Item("Probe1Temperature").InnerText
                        drResp("Probe2Temperature") = root.ChildNodes(nElemIdx).Item("Probe2Temperature").InnerText
                        drResp("WiFiDataCount") = root.ChildNodes(nElemIdx).Item("WiFiDataCount").InnerText
                        drResp("LastWiFiDataReceivedTime") = root.ChildNodes(nElemIdx).Item("LastWiFiDataReceivedTime").InnerText
                        drResp("AlertType") = root.ChildNodes(nElemIdx).Item("AlertType").InnerText
                        drResp("Temperature") = root.ChildNodes(nElemIdx).Item("Temperature").InnerText
                        drResp("Humidity") = root.ChildNodes(nElemIdx).Item("Humidity").InnerText
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        drResp("MonitorId") = root.ChildNodes(nElemIdx).Item("MonitorId").InnerText
                        drResp("RoomId") = root.ChildNodes(nElemIdx).Item("RoomId").InnerText
                        drResp("MonitorType") = root.ChildNodes(nElemIdx).Item("MonitorType").InnerText
                        drResp("Profile") = root.ChildNodes(nElemIdx).Item("Profile").InnerText
                        drResp("IRProfile") = root.ChildNodes(nElemIdx).Item("IRProfile").InnerText
                        drResp("PowerLevel") = root.ChildNodes(nElemIdx).Item("PowerLevel").InnerText
                        drResp("RoomBleeding") = root.ChildNodes(nElemIdx).Item("RoomBleeding").InnerText
                        drResp("NoiseLevel") = root.ChildNodes(nElemIdx).Item("NoiseLevel").InnerText
                        drResp("Masking") = root.ChildNodes(nElemIdx).Item("Masking").InnerText
                        drResp("MasterSlave") = root.ChildNodes(nElemIdx).Item("MasterSlave").InnerText
                        drResp("SpecialProfile") = root.ChildNodes(nElemIdx).Item("SpecialProfile").InnerText
                        drResp("OperatingMode") = root.ChildNodes(nElemIdx).Item("OperatingMode").InnerText
                        drResp("Modes") = root.ChildNodes(nElemIdx).Item("Modes").InnerText
                        drResp("AlertSupressionTime") = root.ChildNodes(nElemIdx).Item("AlertSupressionTime").InnerText
                        drResp("ModelItem") = root.ChildNodes(nElemIdx).Item("ModelItem").InnerText
                        drResp("ShipDate") = root.ChildNodes(nElemIdx).Item("ShipDate").InnerText
                    ElseIf DeviceType = enumDeviceType.Star Then
                        drResp("MacId") = root.ChildNodes(nElemIdx).Item("MacId").InnerText
                        drResp("StarType") = root.ChildNodes(nElemIdx).Item("StarType").InnerText
                        drResp("DHCP") = root.ChildNodes(nElemIdx).Item("DHCP").InnerText
                        drResp("SaveSettings") = root.ChildNodes(nElemIdx).Item("SaveSettings").InnerText
                        drResp("StaticIP") = root.ChildNodes(nElemIdx).Item("StaticIP").InnerText
                        drResp("Subnet") = root.ChildNodes(nElemIdx).Item("Subnet").InnerText
                        drResp("Gateway") = root.ChildNodes(nElemIdx).Item("Gateway").InnerText
                        drResp("TimeServerIP") = root.ChildNodes(nElemIdx).Item("TimeServerIP").InnerText
                        drResp("ServerIP") = root.ChildNodes(nElemIdx).Item("ServerIP").InnerText
                        drResp("PagingServerIP") = root.ChildNodes(nElemIdx).Item("PagingServerIP").InnerText
                        drResp("LocationServerIP1") = root.ChildNodes(nElemIdx).Item("LocationServerIP1").InnerText
                        drResp("LocationServerIP2") = root.ChildNodes(nElemIdx).Item("LocationServerIP2").InnerText
                        drResp("SuperSyncProfile") = root.ChildNodes(nElemIdx).Item("SuperSyncProfile").InnerText
                    End If
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception
            End Try

            Return dtResp
        End Function
        '******************************************************************************************************************************************************'
        ' Function Name : LoadDeviceActivityXMLintoTable                                                                                                       '
        ' Input         : XML String , DeviceType                                                                                                              ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Device Acitivity information                                                              '
        '******************************************************************************************************************************************************'
        Private Function LoadDeviceActivityXMLintoTable(ByVal responseFromServer As String, ByVal DeviceType As Integer) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList = Nothing
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            If DeviceType = enumDeviceType.Tag Then
                dtResp.Columns.Add(New DataColumn("TagId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("ActivityDate", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PagingCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("BatteryValue", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBIDiff", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationActivity", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PagingActivity", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IR", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("RF", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IAvg", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("BatteryCapacity", Type.[GetType]("System.String")))
            ElseIf DeviceType = enumDeviceType.Monitor Then
                dtResp.Columns.Add(New DataColumn("MonitorId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("ActivityDate", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PagingCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TriggerCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Lbi", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("C", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("BatteryCapacity", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("InitialBatteryCapacity", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ActiveHrs", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("MissingHrs", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("Im", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Ia", Type.[GetType]("System.String")))
            ElseIf DeviceType = enumDeviceType.Star Then
                dtResp.Columns.Add(New DataColumn("MacId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Firmwareversion", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IPAddress", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("UpdatedOn", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LockedStarId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("Locationdatareceived", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("Pagedatareceived", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("NonSyncCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("EthernetOffsetCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("LocationDataCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("PageDataCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("TimeDiff", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("ResCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("RequestCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("DownTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("DATFileName", Type.[GetType]("System.String")))
            End If

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement

                If DeviceType = enumDeviceType.Tag Then
                    elemList = root.SelectNodes("Tag")
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    elemList = root.SelectNodes("Monitor")
                ElseIf DeviceType = enumDeviceType.Star Then
                    elemList = root.SelectNodes("Star")
                End If

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    If DeviceType = enumDeviceType.Tag Then
                        drResp("TagId") = elemList.Item(nElemIdx).Item("TagId").InnerText
                        drResp("ActivityDate") = elemList.Item(nElemIdx).Item("ActivityDate").InnerText
                        drResp("LocationCount") = elemList.Item(nElemIdx).Item("LocationCount").InnerText
                        drResp("PagingCount") = elemList.Item(nElemIdx).Item("PagingCount").InnerText
                        drResp("BatteryValue") = elemList.Item(nElemIdx).Item("BatteryValue").InnerText
                        drResp("LBIDiff") = elemList.Item(nElemIdx).Item("LBIDiff").InnerText
                        drResp("LocationActivity") = elemList.Item(nElemIdx).Item("LocationActivity").InnerText
                        drResp("PagingActivity") = elemList.Item(nElemIdx).Item("PagingActivity").InnerText
                        drResp("IR") = elemList.Item(nElemIdx).Item("IR").InnerText
                        drResp("RF") = elemList.Item(nElemIdx).Item("RF").InnerText
                        drResp("IAvg") = elemList.Item(nElemIdx).Item("IAvg").InnerText
                        drResp("BatteryCapacity") = elemList.Item(nElemIdx).Item("BatteryCapacity").InnerText
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        drResp("MonitorId") = elemList.Item(nElemIdx).Item("MonitorId").InnerText
                        drResp("ActivityDate") = elemList.Item(nElemIdx).Item("ActivityDate").InnerText
                        drResp("LocationCount") = elemList.Item(nElemIdx).Item("LocationCount").InnerText
                        drResp("PagingCount") = elemList.Item(nElemIdx).Item("PagingCount").InnerText
                        drResp("TriggerCount") = elemList.Item(nElemIdx).Item("TriggerCount").InnerText
                        drResp("Lbi") = elemList.Item(nElemIdx).Item("Lbi").InnerText
                        drResp("C") = elemList.Item(nElemIdx).Item("C").InnerText
                        drResp("BatteryCapacity") = elemList.Item(nElemIdx).Item("BatteryCapacity").InnerText
                        drResp("InitialBatteryCapacity") = elemList.Item(nElemIdx).Item("InitialBatteryCapacity").InnerText
                        drResp("ActiveHrs") = elemList.Item(nElemIdx).Item("ActiveHrs").InnerText
                        drResp("MissingHrs") = elemList.Item(nElemIdx).Item("MissingHrs").InnerText
                        drResp("Im") = elemList.Item(nElemIdx).Item("Im").InnerText
                        drResp("Ia") = elemList.Item(nElemIdx).Item("Ia").InnerText
                    ElseIf DeviceType = enumDeviceType.Star Then
                        drResp("MacId") = elemList.Item(nElemIdx).Item("MacId").InnerText
                        drResp("Firmwareversion") = elemList.Item(nElemIdx).Item("Firmwareversion").InnerText
                        drResp("IPAddress") = elemList.Item(nElemIdx).Item("IPAddress").InnerText
                        drResp("UpdatedOn") = elemList.Item(nElemIdx).Item("UpdatedOn").InnerText
                        drResp("LockedStarId") = elemList.Item(nElemIdx).Item("LockedStarId").InnerText
                        drResp("Locationdatareceived") = elemList.Item(nElemIdx).Item("Locationdatareceived").InnerText
                        drResp("Pagedatareceived") = elemList.Item(nElemIdx).Item("Pagedatareceived").InnerText
                        drResp("NonSyncCount") = elemList.Item(nElemIdx).Item("NonSyncCount").InnerText
                        drResp("EthernetOffsetCount") = elemList.Item(nElemIdx).Item("EthernetOffsetCount").InnerText
                        drResp("LocationDataCount") = elemList.Item(nElemIdx).Item("LocationDataCount").InnerText
                        drResp("PageDataCount") = elemList.Item(nElemIdx).Item("PageDataCount").InnerText
                        drResp("TimeDiff") = elemList.Item(nElemIdx).Item("TimeDiff").InnerText
                        drResp("ResCount") = elemList.Item(nElemIdx).Item("ResCount").InnerText
                        drResp("RequestCount") = elemList.Item(nElemIdx).Item("RequestCount").InnerText
                        drResp("DownTime") = elemList.Item(nElemIdx).Item("DownTime").InnerText
                        drResp("DATFileName") = elemList.Item(nElemIdx).Item("DATFileName").InnerText
                    End If
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadDeviceHourlyInfoXMLintoTable                                                                                                     '
        ' Input         : XML String , DeviceType                                                                                                              ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Device Hourly information                                                                 '
        '******************************************************************************************************************************************************'
        Private Function LoadDeviceHourlyInfoXMLintoTable(ByVal responseFromServer As String, ByVal DeviceType As Integer) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList = Nothing
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            If DeviceType = enumDeviceType.Tag Then
                dtResp.Columns.Add(New DataColumn("DeviceId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("FirmwareVersion", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MonitorId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("RSSI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBIValue", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MinLBI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MaxLBI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBIDiff", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LockedStarId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LastPagingTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ReceivedTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationDataReceived", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PageDataReceived", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("AliveCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("AvgRssi", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRSeen", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("StarCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("StarSeen", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TempC", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TempADC", Type.[GetType]("System.String")))
            ElseIf DeviceType = enumDeviceType.Monitor Then
                dtResp.Columns.Add(New DataColumn("DeviceId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("FirmwareVersion", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LastSeenTagId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IRId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("RSSI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBIValue", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MinLBI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("MaxLBI", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LBIDiff", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LockedStarId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LastPagingTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("ReceivedTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LocationDataReceived", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("PageDataReceived", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("TriggerCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("AvgRssi", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("StarCount", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("StarSeen", Type.[GetType]("System.String")))
            ElseIf DeviceType = enumDeviceType.Star Then
                dtResp.Columns.Add(New DataColumn("MacId", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("Firmwareversion", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("IPAddress", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("UpdatedOn", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("LockedStarId", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("Locationdatareceived", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("Pagedatareceived", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("NonSyncCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("EthernetOffsetCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("LocationDataCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("PageDataCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("TimeDiff", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("ResCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("RequestCount", Type.[GetType]("System.Int32")))
                dtResp.Columns.Add(New DataColumn("DownTime", Type.[GetType]("System.String")))
                dtResp.Columns.Add(New DataColumn("DATFileName", Type.[GetType]("System.String")))
            End If

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("Device")

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    If DeviceType = enumDeviceType.Tag Then
                        drResp("DeviceId") = elemList.Item(nElemIdx).Item("DeviceId").InnerText
                        drResp("FirmwareVersion") = elemList.Item(nElemIdx).Item("FirmwareVersion").InnerText
                        drResp("IRId") = elemList.Item(nElemIdx).Item("IRId").InnerText
                        drResp("MonitorId") = elemList.Item(nElemIdx).Item("MonitorId").InnerText
                        drResp("RSSI") = elemList.Item(nElemIdx).Item("RSSI").InnerText
                        drResp("LBI") = elemList.Item(nElemIdx).Item("LBI").InnerText
                        drResp("LBIValue") = elemList.Item(nElemIdx).Item("LBIValue").InnerText
                        drResp("MinLBI") = elemList.Item(nElemIdx).Item("MinLBI").InnerText
                        drResp("MaxLBI") = elemList.Item(nElemIdx).Item("MaxLBI").InnerText
                        drResp("LBIDiff") = elemList.Item(nElemIdx).Item("LBIDiff").InnerText
                        drResp("LockedStarId") = elemList.Item(nElemIdx).Item("LockedStarId").InnerText
                        drResp("LastPagingTime") = elemList.Item(nElemIdx).Item("LastPagingTime").InnerText
                        drResp("ReceivedTime") = elemList.Item(nElemIdx).Item("ReceivedTime").InnerText
                        drResp("LocationDataReceived") = elemList.Item(nElemIdx).Item("LocationDataReceived").InnerText
                        drResp("PageDataReceived") = elemList.Item(nElemIdx).Item("PageDataReceived").InnerText
                        drResp("AliveCount") = elemList.Item(nElemIdx).Item("AliveCount").InnerText
                        drResp("AvgRssi") = elemList.Item(nElemIdx).Item("AvgRssi").InnerText
                        drResp("IRCount") = elemList.Item(nElemIdx).Item("IRCount").InnerText
                        drResp("IRSeen") = elemList.Item(nElemIdx).Item("IRSeen").InnerText
                        drResp("StarCount") = elemList.Item(nElemIdx).Item("StarCount").InnerText
                        drResp("StarSeen") = elemList.Item(nElemIdx).Item("StarSeen").InnerText
                        drResp("TempC") = elemList.Item(nElemIdx).Item("TempC").InnerText
                        drResp("TempADC") = elemList.Item(nElemIdx).Item("TempADC").InnerText
                    ElseIf DeviceType = enumDeviceType.Monitor Then
                        drResp("DeviceId") = elemList.Item(nElemIdx).Item("DeviceId").InnerText
                        drResp("FirmwareVersion") = elemList.Item(nElemIdx).Item("FirmwareVersion").InnerText
                        drResp("LastSeenTagId") = elemList.Item(nElemIdx).Item("LastSeenTagId").InnerText
                        drResp("IRId") = elemList.Item(nElemIdx).Item("IRId").InnerText
                        drResp("RSSI") = elemList.Item(nElemIdx).Item("RSSI").InnerText
                        drResp("LBI") = elemList.Item(nElemIdx).Item("LBI").InnerText
                        drResp("LBIValue") = elemList.Item(nElemIdx).Item("LBIValue").InnerText
                        drResp("MinLBI") = elemList.Item(nElemIdx).Item("MinLBI").InnerText
                        drResp("MaxLBI") = elemList.Item(nElemIdx).Item("MaxLBI").InnerText
                        drResp("LBIDiff") = elemList.Item(nElemIdx).Item("LBIDiff").InnerText
                        drResp("LockedStarId") = elemList.Item(nElemIdx).Item("LockedStarId").InnerText
                        drResp("LastPagingTime") = elemList.Item(nElemIdx).Item("LastPagingTime").InnerText
                        drResp("ReceivedTime") = elemList.Item(nElemIdx).Item("ReceivedTime").InnerText
                        drResp("LocationDataReceived") = elemList.Item(nElemIdx).Item("LocationDataReceived").InnerText
                        drResp("PageDataReceived") = elemList.Item(nElemIdx).Item("PageDataReceived").InnerText
                        drResp("TriggerCount") = elemList.Item(nElemIdx).Item("TriggerCount").InnerText
                        drResp("AvgRssi") = elemList.Item(nElemIdx).Item("AvgRssi").InnerText
                        drResp("StarCount") = elemList.Item(nElemIdx).Item("StarCount").InnerText
                        drResp("StarSeen") = elemList.Item(nElemIdx).Item("StarSeen").InnerText
                    ElseIf DeviceType = enumDeviceType.Star Then
                        drResp("MacId") = elemList.Item(nElemIdx).Item("MacId").InnerText
                        drResp("Firmwareversion") = elemList.Item(nElemIdx).Item("Firmwareversion").InnerText
                        drResp("IPAddress") = elemList.Item(nElemIdx).Item("IPAddress").InnerText
                        drResp("UpdatedOn") = elemList.Item(nElemIdx).Item("UpdatedOn").InnerText
                        drResp("LockedStarId") = elemList.Item(nElemIdx).Item("LockedStarId").InnerText
                        drResp("Locationdatareceived") = elemList.Item(nElemIdx).Item("Locationdatareceived").InnerText
                        drResp("Pagedatareceived") = elemList.Item(nElemIdx).Item("Pagedatareceived").InnerText
                        drResp("NonSyncCount") = elemList.Item(nElemIdx).Item("NonSyncCount").InnerText
                        drResp("EthernetOffsetCount") = elemList.Item(nElemIdx).Item("EthernetOffsetCount").InnerText
                        drResp("LocationDataCount") = elemList.Item(nElemIdx).Item("LocationDataCount").InnerText
                        drResp("PageDataCount") = elemList.Item(nElemIdx).Item("PageDataCount").InnerText
                        drResp("TimeDiff") = elemList.Item(nElemIdx).Item("TimeDiff").InnerText
                        drResp("ResCount") = elemList.Item(nElemIdx).Item("ResCount").InnerText
                        drResp("RequestCount") = elemList.Item(nElemIdx).Item("RequestCount").InnerText
                        drResp("DownTime") = elemList.Item(nElemIdx).Item("DownTime").InnerText
                        drResp("DATFileName") = elemList.Item(nElemIdx).Item("DATFileName").InnerText
                    End If
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadAlertInfoXMLintoTable                                                                                                            '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Alert information                                                                         '
        '******************************************************************************************************************************************************'
        Private Function LoadAlertInfoXMLintoTable(ByVal responseFromServer As String) As DataTable

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim elemListSub As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nSiteId As Integer = 0
            Dim sSiteName As String = ""
            Dim nElemIdx As Integer = 0
            Dim nElemIdxSub As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("AlertId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("AlertCount", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Description", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Status", Type.[GetType]("System.String")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("Alerts")

                For nElemIdx = 0 To elemList.Count - 1
                    elemListSub = root.ChildNodes(nElemIdx).SelectNodes("Alert")
                    For nElemIdxSub = 0 To elemListSub.Count - 1
                        drResp = dtResp.NewRow()
                        drResp("SiteId") = elemList.Item(nElemIdx).Item("SiteId").InnerText
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Site").InnerText
                        drResp("DeviceType") = elemListSub.Item(nElemIdxSub).Item("DeviceType").InnerText
                        drResp("AlertId") = elemListSub.Item(nElemIdxSub).Item("AlertId").InnerText
                        drResp("AlertCount") = elemListSub.Item(nElemIdxSub).Item("AlertCount").InnerText
                        drResp("Description") = elemListSub.Item(nElemIdxSub).Item("Description").InnerText
                        drResp("Status") = elemListSub.Item(nElemIdxSub).Item("Status").InnerText
                        dtResp.Rows.Add(drResp)
                    Next
                Next
            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadGlossaryInfoXMLintoTable                                                                                                         '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Glossary information                                                                      '
        '******************************************************************************************************************************************************'
        Private Function LoadGlossaryInfoXMLintoTable(ByVal responseFromServer As String) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim elemList2 As XmlNodeList
            Dim elemList3 As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim sGloassryName As String = ""
            Dim nElemIdx As Integer = 0
            Dim nElemIdx2 As Integer = 0
            Dim nElemIdx3 As Integer = 0

            dtResp.Columns.Add(New DataColumn("GloassryName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ExpandStory", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Description", Type.[GetType]("System.String")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.ChildNodes

                For nElemIdx = 0 To elemList.Count - 1
                    elemList2 = root.ChildNodes(nElemIdx).ChildNodes
                    sGloassryName = root.ChildNodes(nElemIdx).Name

                    For nElemIdx2 = 0 To elemList2.Count - 1
                        If sGloassryName = "UserTagDetails" Or sGloassryName = "LbiHistory" Then
                            elemList3 = root.ChildNodes(nElemIdx).ChildNodes(nElemIdx2).ChildNodes

                            For nElemIdx3 = 0 To elemList3.Count - 1
                                drResp = dtResp.NewRow()
                                drResp("GloassryName") = sGloassryName
                                drResp("DeviceType") = EncodeAsXMLString(root.ChildNodes(nElemIdx).ChildNodes(nElemIdx2).Name)
                                drResp("ExpandStory") = EncodeAsXMLString(root.ChildNodes(nElemIdx).ChildNodes(nElemIdx2).ChildNodes(nElemIdx3).ChildNodes(0).InnerText)
                                drResp("Description") = EncodeAsXMLString(root.ChildNodes(nElemIdx).ChildNodes(nElemIdx2).ChildNodes(nElemIdx3).ChildNodes(1).InnerText)
                                dtResp.Rows.Add(drResp)
                            Next
                        Else
                            drResp = dtResp.NewRow()
                            drResp("GloassryName") = sGloassryName
                            drResp("DeviceType") = ""
                            drResp("ExpandStory") = EncodeAsXMLString(root.ChildNodes(nElemIdx).ChildNodes(nElemIdx2).ChildNodes(0).InnerText)
                            drResp("Description") = EncodeAsXMLString(root.ChildNodes(nElemIdx).ChildNodes(nElemIdx2).ChildNodes(1).InnerText)
                            dtResp.Rows.Add(drResp)
                        End If
                    Next
                Next
            Catch ex As Exception
            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadSiteListInfoXMLintoTable                                                                                                         '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- List of site and its information                                                           '
        '******************************************************************************************************************************************************'
        Private Function LoadSiteListInfoXMLintoTable(ByVal responseFromServer As String) As DataTable
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.ChildNodes

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(nElemIdx).Item("SiteId").InnerText
                    drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(nElemIdx).Item("SiteName").InnerText)
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception
            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadUserAuthXMLintoTable                                                                                                             '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- User Authendication information                                                           '
        '******************************************************************************************************************************************************'
        Private Function LoadUserAuthXMLintoTable(ByVal responseFromServer As String) As DataTable

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim elemListSub As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0
            Dim nElemIdxSub As Integer = 0

            dtResp.Columns.Add(New DataColumn("Response", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("AuthenticationKey", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("UserID", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("Username", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("usertype", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("UserRoleId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("UserRoleType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))

            Try

                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("SiteIdList")
                elemListSub = elemList.Item(0).ChildNodes

                For nElemIdxSub = 0 To elemListSub.Count - 1
                    drResp = dtResp.NewRow()
                    drResp("Response") = root.Item("Response").InnerText
                    drResp("AuthenticationKey") = root.Item("AuthenticationKey").InnerText
                    drResp("UserID") = root.Item("UserID").InnerText
                    drResp("Username") = root.Item("Username").InnerText
                    drResp("usertype") = root.Item("usertype").InnerText
                    drResp("UserRoleId") = root.Item("UserRoleId").InnerText
                    drResp("UserRoleType") = root.Item("UserRoleType").InnerText
                    drResp("SiteId") = elemListSub.Item(nElemIdxSub).Attributes.Item(0).InnerText
                    drResp("SiteName") = EncodeAsXMLString(elemListSub.Item(nElemIdxSub).InnerText)
                    dtResp.Rows.Add(drResp)
                Next

            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadUpdatedOnXMLintoTable                                                                                                            '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- site last update time                                                                     '
        '******************************************************************************************************************************************************'
        Private Function LoadUpdatedOnXMLintoTable(ByVal responseFromServer As String) As DataTable

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("GMSLastupdate", Type.[GetType]("System.String")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.ChildNodes

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    drResp("GMSLastupdate") = root.ChildNodes(0).InnerText
                    dtResp.Rows.Add(drResp)
                Next

            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadTagInfoXMLintoTable                                                                                                              '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Tag information                                                                           '
        '******************************************************************************************************************************************************'
        Private Function LoadTagInfoXMLintoTable(ByVal root As XmlElement, ByVal isSearch As Boolean) As DataTable

            Dim doc As New XmlDocument
            Dim elemList As XmlNodeList

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TotalPage", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TotalCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TagId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("CatastrophicCases", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("CCatastrophicCases", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceSubTypeId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("MonitorLocation", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LBIActivity", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("CActivityLevel", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("MedianLBIValue", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("MedianLBIDiff", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("IsAlgorithmcovered", Type.[GetType]("System.Boolean")))

            Try

                elemList = root.SelectNodes("Tag")

                For nElemIdx = 0 To elemList.Count - 1

                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(0).InnerText

                    If isSearch Then
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Sitename").InnerText
                        drResp("SiteId") = elemList.Item(nElemIdx).Item("DeviceSiteId").InnerText
                    Else
                        drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(1).InnerText)
                    End If

                    drResp("TotalPage") = root.ChildNodes(2).InnerText
                    drResp("TotalCount") = root.ChildNodes(3).InnerText
                    drResp("TagId") = elemList.Item(nElemIdx).Item("TagId").InnerText
                    drResp("CatastrophicCases") = elemList.Item(nElemIdx).Item("CatastrophicCases").InnerText
                    drResp("CCatastrophicCases") = elemList.Item(nElemIdx).Item("CCatastrophicCases").InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("offline").InnerText
                    drResp("ModelItem") = elemList.Item(nElemIdx).Item("ModelItem").InnerText
                    drResp("DeviceSubTypeId") = elemList.Item(nElemIdx).Item("DeviceSubTypeId").InnerText
                    drResp("MonitorLocation") = elemList.Item(nElemIdx).Item("MonitorLocation").InnerText
                    drResp("LBIActivity") = elemList.Item(nElemIdx).Item("Last20WeekData").InnerText
                    drResp("CActivityLevel") = elemList.Item(nElemIdx).Item("CActivityLevel").InnerText
                    drResp("MedianLBIValue") = elemList.Item(nElemIdx).Item("MedianLBIValue").InnerText
                    drResp("MedianLBIDiff") = elemList.Item(nElemIdx).Item("MedianLBIDiff").InnerText
                    drResp("IsAlgorithmcovered") = elemList.Item(nElemIdx).Item("IsAlgorithmcovered").InnerText

                    dtResp.Rows.Add(drResp)

                Next

            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : Testbed_LoadTagInfoXMLintoTable                                                                                                              '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Tag information                                                                           '
        '******************************************************************************************************************************************************'
        Private Function Testbed_LoadTagInfoXMLintoTable(ByVal responseFromServer As String, ByVal isSearch As Boolean) As DataTable

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TotalPage", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TotalCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TagId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("MonitorId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("FirmwareVersion", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("FirstSeen", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("PageDataReceived", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LocationDataReceived", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("IRID", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("CatastrophicCases", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen30Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen90Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen180Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("ShipDate", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LastSeen", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ActionRequired", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryCapacity", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ActivityLevel", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("PoNumber", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ADCValue", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("SWVersion", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryReplacementCount", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryReplacementOn", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TempADC", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Voltage", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryStatus", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Prediction", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LastIRTime", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("RoomSeen", Type.[GetType]("System.String")))

            Try

                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("Tag")

                For nElemIdx = 0 To elemList.Count - 1

                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(0).InnerText

                    If isSearch Then
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Sitename").InnerText
                    Else
                        drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(1).InnerText)
                    End If

                    drResp("TotalPage") = root.ChildNodes(2).InnerText
                    drResp("TotalCount") = root.ChildNodes(3).InnerText
                    drResp("TagId") = elemList.Item(nElemIdx).Item("TagId").InnerText
                    drResp("MonitorId") = elemList.Item(nElemIdx).Item("MonitorId").InnerText
                    drResp("FirmwareVersion") = EncodeAsXMLString(elemList.Item(nElemIdx).Item("Firmware_Version").InnerText)
                    drResp("FirstSeen") = elemList.Item(nElemIdx).Item("FirstSeen").InnerText
                    drResp("PageDataReceived") = elemList.Item(nElemIdx).Item("PageDataReceived").InnerText
                    drResp("LocationDataReceived") = elemList.Item(nElemIdx).Item("LocationDataReceived").InnerText
                    drResp("IRID") = elemList.Item(nElemIdx).Item("IRID").InnerText
                    drResp("CatastrophicCases") = elemList.Item(nElemIdx).Item("CatastrophicCases").InnerText
                    drResp("LessThen30Days") = elemList.Item(nElemIdx).Item("LessThen30Days").InnerText
                    drResp("LessThen90Days") = elemList.Item(nElemIdx).Item("LessThen90Days").InnerText
                    drResp("LessThen180Days") = elemList.Item(nElemIdx).Item("LessThen180Days").InnerText
                    drResp("ShipDate") = elemList.Item(nElemIdx).Item("ShipDate").InnerText
                    drResp("LastSeen") = elemList.Item(nElemIdx).Item("LastSeen").InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("offline").InnerText
                    drResp("ActionRequired") = elemList.Item(nElemIdx).Item("ActionRequired").InnerText
                    drResp("BatteryCapacity") = elemList.Item(nElemIdx).Item("BatteryCapacity").InnerText
                    drResp("ActivityLevel") = elemList.Item(nElemIdx).Item("ActivityLevel").InnerText
                    drResp("ModelItem") = elemList.Item(nElemIdx).Item("ModelItem").InnerText
                    drResp("PoNumber") = elemList.Item(nElemIdx).Item("PoNumber").InnerText
                    drResp("ADCValue") = elemList.Item(nElemIdx).Item("ADCValue").InnerText
                    drResp("SWVersion") = elemList.Item(nElemIdx).Item("SWVersion").InnerText
                    drResp("BatteryReplacementCount") = elemList.Item(nElemIdx).Item("BatteryReplacementCount").InnerText
                    drResp("BatteryReplacementOn") = elemList.Item(nElemIdx).Item("BatteryReplacementOn").InnerText
                    drResp("TempADC") = elemList.Item(nElemIdx).Item("TempADC").InnerText
                    drResp("Voltage") = elemList.Item(nElemIdx).Item("Voltage").InnerText
                    drResp("BatteryStatus") = elemList.Item(nElemIdx).Item("BatteryStatus").InnerText
                    drResp("Prediction") = elemList.Item(nElemIdx).Item("Prediction").InnerText
                    drResp("LastIRTime") = elemList.Item(nElemIdx).Item("LastIRTime").InnerText
                    drResp("RoomSeen") = elemList.Item(nElemIdx).Item("RoomSeen").InnerText

                    dtResp.Rows.Add(drResp)
                Next

            Catch ex As Exception
                WriteLog(" Testbed_LoadTagInfoXMLintoTable" & ex.Message.ToString())
            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadMonitorInfoXMLintoTable                                                                                                          '
        ' Input         : XML Element                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Monitor information                                                                       '
        '******************************************************************************************************************************************************'
        Private Function LoadMonitorInfoXMLintoTable(ByVal root As XmlElement, ByVal isSearch As Boolean) As DataTable

            Dim doc As New XmlDocument
            Dim elemList As XmlNodeList

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TotalPage", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TotalCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("DeviceId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("CatastrophicCases", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("PoNumber", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("RoomName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LBIActivity", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceSubTypeId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LBIValue", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("IsAlgorithmcovered", Type.[GetType]("System.Boolean")))
            dtResp.Columns.Add(New DataColumn("CCatastrophicCases", Type.[GetType]("System.Int32")))

            Try

                elemList = root.SelectNodes("Monitor")

                For nElemIdx = 0 To elemList.Count - 1

                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(0).InnerText

                    If isSearch Then
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Sitename").InnerText
                        drResp("SiteId") = elemList.Item(nElemIdx).Item("DeviceSiteId").InnerText
                    Else
                        drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(1).InnerText)
                    End If

                    drResp("TotalPage") = root.ChildNodes(2).InnerText
                    drResp("TotalCount") = root.ChildNodes(3).InnerText
                    drResp("DeviceId") = elemList.Item(nElemIdx).Item("DeviceId").InnerText
                    drResp("CatastrophicCases") = elemList.Item(nElemIdx).Item("CatastrophicCases").InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("offline").InnerText
                    drResp("ModelItem") = elemList.Item(nElemIdx).Item("ModelItem").InnerText
                    drResp("RoomName") = elemList.Item(nElemIdx).Item("RoomName").InnerText
                    drResp("LBIActivity") = elemList.Item(nElemIdx).Item("Last20WeekData").InnerText
                    drResp("DeviceSubTypeId") = elemList.Item(nElemIdx).Item("DeviceSubTypeId").InnerText
                    drResp("LBIValue") = elemList.Item(nElemIdx).Item("LBIValue").InnerText
                    drResp("IsAlgorithmcovered") = elemList.Item(nElemIdx).Item("IsAlgorithmcovered").InnerText
                    drResp("CCatastrophicCases") = elemList.Item(nElemIdx).Item("CCatastrophicCases").InnerText
                    dtResp.Rows.Add(drResp)

                Next
            Catch ex As Exception
                WriteLog(" LoadMonitorInfoXMLintoTable" & ex.Message.ToString())
            End Try

            Return dtResp

        End Function
        '******************************************************************************************************************************************************'
        ' Function Name : Testbed_LoadMonitorInfoXMLintoTable                                                                                                          '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Monitor information                                                                       '
        '******************************************************************************************************************************************************'
        Private Function Testbed_LoadMonitorInfoXMLintoTable(ByVal responseFromServer As String, ByVal isSearch As Boolean) As DataTable

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TotalPage", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TotalCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("DeviceId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("DeviceName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("FirmwareVersion", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("FirstSeen", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("PageDataReceived", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LocationDataReceived", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("IRID", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("CatastrophicCases", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen30Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen90Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LessThen180Days", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("ShipDate", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LastSeen", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ActionRequired", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryCapacity", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ActivityLevel", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("PoNumber", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryReplacementCount", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryReplacementOn", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("RoomName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("BatteryStatus", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("SWVersion", Type.[GetType]("System.String")))

            Try

                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("Monitor")

                For nElemIdx = 0 To elemList.Count - 1

                    drResp = dtResp.NewRow()

                    drResp("SiteId") = root.ChildNodes(0).InnerText

                    If isSearch Then
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Sitename").InnerText
                        drResp("SiteId") = elemList.Item(nElemIdx).Item("DeviceSiteId").InnerText
                    Else
                        drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(1).InnerText)
                    End If

                    drResp("TotalPage") = root.ChildNodes(2).InnerText
                    drResp("TotalCount") = root.ChildNodes(3).InnerText
                    drResp("DeviceId") = elemList.Item(nElemIdx).Item("DeviceId").InnerText
                    drResp("DeviceName") = elemList.Item(nElemIdx).Item("DeviceName").InnerText
                    drResp("FirmwareVersion") = EncodeAsXMLString(elemList.Item(nElemIdx).Item("Firmware_Version").InnerText)
                    drResp("BatteryType") = elemList.Item(nElemIdx).Item("BatteryType").InnerText
                    drResp("FirstSeen") = elemList.Item(nElemIdx).Item("FirstSeen").InnerText
                    drResp("PageDataReceived") = elemList.Item(nElemIdx).Item("PageDataReceived").InnerText
                    drResp("LocationDataReceived") = elemList.Item(nElemIdx).Item("LocationDataReceived").InnerText
                    drResp("IRID") = elemList.Item(nElemIdx).Item("IRID").InnerText
                    drResp("CatastrophicCases") = elemList.Item(nElemIdx).Item("CatastrophicCases").InnerText
                    drResp("LessThen30Days") = elemList.Item(nElemIdx).Item("LessThen30Days").InnerText
                    drResp("LessThen90Days") = elemList.Item(nElemIdx).Item("LessThen90Days").InnerText
                    drResp("LessThen180Days") = elemList.Item(nElemIdx).Item("LessThen180Days").InnerText
                    drResp("ShipDate") = elemList.Item(nElemIdx).Item("ShipDate").InnerText
                    drResp("LastSeen") = elemList.Item(nElemIdx).Item("LastSeen").InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("offline").InnerText
                    drResp("ActionRequired") = elemList.Item(nElemIdx).Item("ActionRequired").InnerText
                    drResp("BatteryCapacity") = elemList.Item(nElemIdx).Item("BatteryCapacity").InnerText
                    drResp("ModelItem") = elemList.Item(nElemIdx).Item("ModelItem").InnerText
                    drResp("PoNumber") = elemList.Item(nElemIdx).Item("PoNumber").InnerText
                    drResp("BatteryReplacementCount") = elemList.Item(nElemIdx).Item("BatteryReplacementCount").InnerText
                    drResp("BatteryReplacementOn") = elemList.Item(nElemIdx).Item("BatteryReplacementOn").InnerText
                    drResp("RoomName") = elemList.Item(nElemIdx).Item("RoomName").InnerText
                    drResp("BatteryStatus") = elemList.Item(nElemIdx).Item("BatteryStatus").InnerText
                    drResp("SWVersion") = elemList.Item(nElemIdx).Item("SWVersion").InnerText
                    dtResp.Rows.Add(drResp)

                Next

            Catch ex As Exception
                WriteLog(" Testbed_LoadMonitorInfoXMLintoTable" & ex.Message.ToString())
            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : LoadStarInfoXMLintoTable                                                                                                          '
        ' Input         : XML Element                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Star information                                                                       '
        '******************************************************************************************************************************************************'
        Private Function LoadStarInfoXMLintoTable(ByVal root As XmlElement, ByVal isSearch As Boolean) As DataTable

            Dim doc As New XmlDocument
            Dim elemList As XmlNodeList

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TotalPage", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TotalCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("StarId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("MACId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("StarType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("IPAddr", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("ModelItem", Type.[GetType]("System.String")))

            Try

                'root = doc.DocumentElement
                elemList = root.SelectNodes("Star")

                For nElemIdx = 0 To elemList.Count - 1

                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(0).InnerText

                    If isSearch Then
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Sitename").InnerText
                        drResp("SiteId") = elemList.Item(nElemIdx).Item("DeviceSiteId").InnerText
                    Else
                        drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(1).InnerText)
                    End If

                    drResp("TotalPage") = root.ChildNodes(2).InnerText
                    drResp("TotalCount") = root.ChildNodes(3).InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("offline").InnerText
                    drResp("StarId") = elemList.Item(nElemIdx).Item("StarId").InnerText
                    drResp("MACId") = elemList.Item(nElemIdx).Item("MACId").InnerText
                    drResp("DeviceName") = elemList.Item(nElemIdx).Item("DeviceName").InnerText
                    drResp("StarType") = elemList.Item(nElemIdx).Item("StarType").InnerText
                    drResp("IPAddr") = elemList.Item(nElemIdx).Item("IPAddr").InnerText
                    drResp("ModelItem") = elemList.Item(nElemIdx).Item("ModelItem").InnerText
                    dtResp.Rows.Add(drResp)

                Next

            Catch ex As Exception

            End Try

            Return dtResp

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : Testbed_LoadStarInfoXMLintoTable                                                                                                          '
        ' Input         : XML String                                                                                                                           ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : From Server XML String into Datatable for- Star information                                                                       '
        '******************************************************************************************************************************************************'

        Private Function Testbed_LoadStarInfoXMLintoTable(ByVal responseFromServer As String, ByVal isSearch As Boolean) As DataTable

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim elemList As XmlNodeList
            Dim reader As XmlTextReader

            Dim dtResp As New DataTable
            Dim drResp As DataRow = Nothing

            Dim nElemIdx As Integer = 0

            dtResp.Columns.Add(New DataColumn("SiteId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("SiteName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("TotalPage", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("TotalCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("Offline", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("CompanyName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("StarId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("MACId", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("DeviceName", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("StarType", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("IPAddr", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("LastReceivedTime", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("PageDataReceived", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LocationDataReceived", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("StarPageCount", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LockedStarId", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("LockedStarCnt", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("ErrorCnt", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("NonSyncCnt", Type.[GetType]("System.Int32")))
            dtResp.Columns.Add(New DataColumn("FileVersion", Type.[GetType]("System.String")))
            dtResp.Columns.Add(New DataColumn("Version", Type.[GetType]("System.String")))

            Try
                reader = New XmlTextReader(New System.IO.StringReader(responseFromServer))
                reader.Read()
                doc.Load(reader)

                root = doc.DocumentElement
                elemList = root.SelectNodes("Star")

                For nElemIdx = 0 To elemList.Count - 1
                    drResp = dtResp.NewRow()
                    drResp("SiteId") = root.ChildNodes(0).InnerText
                    If isSearch Then
                        drResp("SiteName") = elemList.Item(nElemIdx).Item("Sitename").InnerText
                    Else
                        drResp("SiteName") = EncodeAsXMLString(root.ChildNodes(1).InnerText)
                    End If

                    drResp("TotalPage") = root.ChildNodes(2).InnerText
                    drResp("TotalCount") = root.ChildNodes(3).InnerText
                    drResp("Offline") = elemList.Item(nElemIdx).Item("offline").InnerText
                    drResp("CompanyName") = elemList.Item(nElemIdx).Item("CompanyName").InnerText
                    drResp("StarId") = elemList.Item(nElemIdx).Item("StarId").InnerText
                    drResp("MACId") = elemList.Item(nElemIdx).Item("MACId").InnerText
                    drResp("DeviceName") = elemList.Item(nElemIdx).Item("DeviceName").InnerText
                    drResp("StarType") = elemList.Item(nElemIdx).Item("StarType").InnerText
                    drResp("IPAddr") = elemList.Item(nElemIdx).Item("IPAddr").InnerText
                    drResp("LastReceivedTime") = elemList.Item(nElemIdx).Item("LastReceivedTime").InnerText
                    drResp("PageDataReceived") = elemList.Item(nElemIdx).Item("PageDataReceived").InnerText
                    drResp("LocationDataReceived") = elemList.Item(nElemIdx).Item("LocationDataReceived").InnerText
                    drResp("StarPageCount") = elemList.Item(nElemIdx).Item("StarPageCount").InnerText
                    drResp("LockedStarId") = elemList.Item(nElemIdx).Item("LockedStarId").InnerText
                    drResp("LockedStarCnt") = elemList.Item(nElemIdx).Item("LockedStarCnt").InnerText
                    drResp("ErrorCnt") = elemList.Item(nElemIdx).Item("ErrorCnt").InnerText
                    drResp("NonSyncCnt") = elemList.Item(nElemIdx).Item("NonSyncCnt").InnerText
                    drResp("FileVersion") = elemList.Item(nElemIdx).Item("FileVersion").InnerText
                    drResp("Version") = elemList.Item(nElemIdx).Item("Version").InnerText
                    dtResp.Rows.Add(drResp)
                Next
            Catch ex As Exception
            End Try

            Return dtResp

        End Function
        '******************************************************************************************************************************************************'
        ' Function Name : CreateSiteOverview                                                                                                                   '
        ' Input         : DataTable                                                                                                                            ' 
        ' Output        : DataTable                                                                                                                            '
        ' Description   : Making Html data for Site Overview                                                                                                   '
        '******************************************************************************************************************************************************'
        Public Function CreateSiteOverview(ByVal dtSiteOverview As DataTable) As DataTable

            Dim dtOverview As New DataTable
            Dim dr As DataRow = Nothing

            Dim nSiteIdx As Integer = 0

            Dim SiteName, DeviceType, GroupId, GroupName, Good, LessThen180Days, LessThen90Days, LessThen30Days, OfflineBatteryDepleted, Typ, OfflineOther As String
            Dim BatSum As String = ""
            Dim Lastupdate As String = ""
            Dim DefinedTagsinCore As String = ""
            Dim DefinedInfrastructureinCore As String = ""
            Dim ImageName As String = ""
            Dim PulseUIId As Integer = 0

            dtOverview.Columns.Add(New DataColumn("UserRole", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("SiteName", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("DeviceType", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("TypeId", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("Type", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("TypeImage", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("Good", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("s180Days", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("s90Days", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("s30Days", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("OfflineBatteryDepleted", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("OfflineOther", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("BatterySummary", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("Lastupdate", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("DefinedTagsinCore", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("DefinedInfrastructureinCore", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("PulseUIId", Type.GetType("System.Int32")))
            dtOverview.Columns.Add(New DataColumn("ImageName", Type.GetType("System.String")))
            dtOverview.Columns.Add(New DataColumn("LBI", Type.GetType("System.Boolean")))
            dtOverview.Columns.Add(New DataColumn("UW", Type.GetType("System.Boolean")))

            If Not dtSiteOverview Is Nothing Then
                If dtSiteOverview.Rows.Count > 0 Then

                    For nSiteIdx = 0 To dtSiteOverview.Rows.Count - 1
                        With dtSiteOverview.Rows(nSiteIdx)

                            siteId = .Item("SiteId")
                            SiteName = .Item("SiteName")
                            DeviceType = .Item("DeviceType")
                            GroupId = .Item("TypeID")
                            Typ = .Item("Type")
                            Lastupdate = .Item("Lastupdate")
                            DefinedTagsinCore = .Item("DefinedTagsinCore")
                            DefinedInfrastructureinCore = .Item("DefinedInfrastructureinCore")
                            ImageName = .Item("ImageName")
                            PulseUIId = .Item("PulseUIId")

                            If (DeviceType = "1") Then
                                GroupName = getTagImage(GroupId, Typ, siteId, ImageName, PulseUIId)
                            ElseIf (DeviceType = "2") Then
                                GroupName = getInfraImage(GroupId, Typ, siteId, ImageName)
                            ElseIf (DeviceType = "3") Then
                                GroupName = getStarImage(GroupId, Typ, siteId, ImageName)
                            End If

                            Good = .Item("Good")

                            If (Good = "0") Then
                                Good = "-"
                            Else
                                If (DeviceType = "3") Then
                                    Good = "<a  class='cell_text_green' href=#divPatientTag onclick=loadStarInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",0,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & Good & "</a>"
                                ElseIf (DeviceType = "2") Then
                                    Good = "<a  class='cell_text_green' href=#divPatientTag onclick=loadInfraInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",0,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & Good & "</a>"
                                ElseIf (DeviceType = "1") Then
                                    Good = "<a  class='cell_text_green' href=#divPatientTag onclick=loadTagInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",0,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & Good & "</a>"
                                End If
                            End If

                            LessThen180Days = .Item("LessThen180Days")

                            If (LessThen180Days = "0") Then
                                LessThen180Days = "-"
                            End If

                            LessThen90Days = .Item("LessThen90Days")

                            If (LessThen90Days = "0") Then
                                LessThen90Days = "-"
                            Else
                                If (DeviceType = "2") Then
                                    LessThen90Days = "<a  class='cell_text_Yellow' href=#divPatientTag onclick=loadInfraInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",1,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & LessThen90Days & "</a>"
                                ElseIf (DeviceType = "1") Then
                                    LessThen90Days = "<a  class='cell_text_Yellow' href=#divPatientTag onclick=loadTagInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",1,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & LessThen90Days & "</a>"
                                End If
                            End If

                            LessThen30Days = .Item("LessThen30Days")

                            If (LessThen30Days = "0") Then
                                LessThen30Days = "-"
                            Else
                                If (DeviceType = "2") Then
                                    LessThen30Days = "<a  class='cell_text_Red' href=#divPatientTag onclick=loadInfraInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",2,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & LessThen30Days & "</a>"
                                ElseIf (DeviceType = "1") Then
                                    LessThen30Days = "<a  class='cell_text_Red' href=#divPatientTag onclick=loadTagInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",2,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ")>" & LessThen30Days & "</a>"
                                End If
                            End If

                            OfflineOther = .Item("OfflineOther")

                            If (OfflineOther = "0") Then
                                OfflineOther = "-"
                            Else
                                If (DeviceType = "3") Then
                                    OfflineOther = "<a id=Offline_" & nSiteIdx & " class='cell_text_offline' href=#divPatientTag onclick=loadStarInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",3,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ") onmouseover='btnsiteOverviewHover(this);' onmouseout='btnsiteOverviewOut(this);'>" & OfflineOther & "</a>"
                                ElseIf (DeviceType = "2") Then
                                    OfflineOther = "<a id=Offline_" & nSiteIdx & " class='cell_text_offline' href=#divPatientTag onclick=loadInfraInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",3,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ") onmouseover='btnsiteOverviewHover(this);' onmouseout='btnsiteOverviewOut(this);'>" & OfflineOther & "</a>"
                                ElseIf (DeviceType = "1") Then
                                    OfflineOther = "<a id=Offline_" & nSiteIdx & " class='cell_text_offline' href=#divPatientTag onclick=loadTagInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",3,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ") onmouseover='btnsiteOverviewHover(this);' onmouseout='btnsiteOverviewOut(this);'>" & OfflineOther & "</a>"
                                End If
                            End If

                            OfflineBatteryDepleted = .Item("OfflineBatteryDepleted")

                            If (OfflineBatteryDepleted = "0") Then
                                OfflineBatteryDepleted = "-"
                            Else
                                If (DeviceType = "3") Then
                                    OfflineBatteryDepleted = "<a id=Offline_" & nSiteIdx & " class='cell_text_offline' href=#divPatientTag onclick=loadStarInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",7,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ") onmouseover='btnsiteOverviewHover(this);' onmouseout='btnsiteOverviewOut(this);'>" & OfflineBatteryDepleted & "</a>"
                                ElseIf (DeviceType = "2") Then
                                    OfflineBatteryDepleted = "<a id=Offline_" & nSiteIdx & " class='cell_text_offline' href=#divPatientTag onclick=loadInfraInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",7,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ") onmouseover='btnsiteOverviewHover(this);' onmouseout='btnsiteOverviewOut(this);'>" & OfflineBatteryDepleted & "</a>"
                                ElseIf (DeviceType = "1") Then
                                    OfflineBatteryDepleted = "<a id=Offline_" & nSiteIdx & " class='cell_text_offline' href=#divPatientTag onclick=loadTagInfoOnClickfromSIteOverview(" & siteId & "," & GroupId & ",7,'" & Typ.Replace(" ", "_") & "'," & PulseUIId & ") onmouseover='btnsiteOverviewHover(this);' onmouseout='btnsiteOverviewOut(this);'>" & OfflineBatteryDepleted & "</a>"
                                End If
                            End If

                            If (DeviceType = "1") Then
                                BatSum = "<a id=BatSum_" & nSiteIdx & " class='cell_text_offline' href=#BatterySummary onmouseover ='TabOver(this);' onmouseout='TabOut(this);' onclick=loadBatteryInfoOnClickfromSIteOverview(" & GroupId & ",1,'" & Typ.Replace(" ", "") & "'); id='systd-" + siteId + "'><img src='images/batterySummaryNew.png' style='border: 0;' /></a>"
                            ElseIf (DeviceType = "2") Then
                                BatSum = "<a id=BatSum_" & nSiteIdx & " class='cell_text_offline' href=#BatterySummary onmouseover ='TabOver(this);' onmouseout='TabOut(this);' onclick=loadBatteryInfoOnClickfromSIteOverview(" & GroupId & ",2,'" & Typ.Replace(" ", "") & "'); id='systd-" + siteId + "'><img src='images/batterySummaryNew.png' style='border: 0;' /></a>"
                            End If

                            dr = dtOverview.NewRow()
                            dr("UserRole") = g_UserRole
                            dr("SiteName") = SiteName
                            dr("DeviceType") = DeviceType
                            dr("TypeId") = GroupId
                            dr("Type") = Typ
                            dr("TypeImage") = GroupName
                            dr("Good") = Good
                            dr("s180Days") = LessThen180Days
                            dr("s90Days") = LessThen90Days
                            dr("s30Days") = LessThen30Days
                            dr("OfflineOther") = OfflineOther
                            dr("OfflineBatteryDepleted") = OfflineBatteryDepleted
                            dr("BatterySummary") = BatSum
                            dr("Lastupdate") = Lastupdate
                            dr("DefinedTagsinCore") = DefinedTagsinCore
                            dr("DefinedInfrastructureinCore") = DefinedInfrastructureinCore
                            dr("PulseUIId") = .Item("PulseUIId")
                            dr("UW") = .Item("UW")
                            dr("LBI") = .Item("LBI")

                            Dim typId As String = Request.QueryString("typId")

                            If ((typId = "tag" Or typId = "system" Or typId = "") And DeviceType = enumDeviceType.Tag) Then
                                dtOverview.Rows.Add(dr)
                            ElseIf ((typId = "infrastructure" Or typId = "system" Or typId = "") And DeviceType <> enumDeviceType.Tag) Then
                                dtOverview.Rows.Add(dr)
                            End If

                        End With
                    Next
                End If
            End If

            If dtOverview.Rows.Count = 0 Then

                dr = dtOverview.NewRow()
                dr("UserRole") = g_UserRole
                dr("SiteName") = SiteName
                dr("DeviceType") = ""
                dr("TypeId") = "0"
                dr("Type") = ""
                dr("TypeImage") = ""
                dr("Good") = ""
                dr("s180Days") = ""
                dr("s90Days") = ""
                dr("s30Days") = ""
                dr("OfflineOther") = ""
                dr("OfflineBatteryDepleted") = ""
                dr("BatterySummary") = ""
                dr("Lastupdate") = ""
                dr("DefinedTagsinCore") = ""
                dr("DefinedInfrastructureinCore") = ""
                dr("PulseUIId") = 0
                dr("UW") = 0
                dr("LBI") = 0

                dtOverview.Rows.Add(dr)

            End If

            Return dtOverview

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : EncodeAsXMLString                                                                                                                   '
        ' Input         : String                                                                                                                              ' 
        ' Output        : Encoded Xml string                                                                                                                  '
        ' Description   : Some of the charecter will be consider key value of xml, so that to be modified Hex value                                           '
        '******************************************************************************************************************************************************'
        Public Function EncodeAsXMLString(ByVal inStr As String) As String

            Dim XMLStr As String
            Dim i As Integer
            Dim charCode As Integer

            XMLStr = ""

            For i = 1 To inStr.Length
                charCode = Asc(Mid(inStr, i, 1))
                Select Case charCode
                    Case 34 '"
                        XMLStr += "&#x" & Hex(34) & ";"
                    Case 39 ''
                        XMLStr += "&#x" & Hex(39) & ";"
                    Case 60 '<
                        XMLStr += "&#x" & Hex(60) & ";"
                    Case 62 '>
                        XMLStr += "&#x" & Hex(62) & ";"""
                    Case 40 '(
                        XMLStr += "&#x" & Hex(40) & ";"
                    Case 41 ')
                        XMLStr += "&#x" & Hex(41) & ";"
                    Case 35 '#
                        XMLStr += "&#x" & Hex(35) & ";"
                    Case 37 '%
                        XMLStr += "&#x" & Hex(37) & ";"
                    Case 38 '&
                        XMLStr += "&#x" & Hex(38) & ";"
                    Case 59 ';
                        XMLStr += "&#x" & Hex(59) & ";"
                    Case 43 '+
                        XMLStr += "&#x" & Hex(43) & ";"
                    Case 45 '-
                        XMLStr += "&#x" & Hex(45) & ";"
                    Case Else
                        XMLStr += Chr(charCode)
                End Select
            Next

            Return XMLStr

        End Function

        Public Sub GetGoodIndication(ByVal scontext As StringBuilder, ByVal SiteId As Integer)

            scontext.Append(CSVCell("Battery replacement indication", True))

        End Sub

        Public Sub GetGoodIndicationValues(ByVal scontext As StringBuilder, ByVal SiteId As Integer, ByVal sgoodindication As String)

            scontext.Append(CSVCell(sgoodindication, True))

        End Sub

        Public Sub GetGoodIndication_IE(ByRef excl As Excelcreate, ByVal SiteId As Integer)

            excl.AddCSVCell(Context, "Battery replacement indication", True)

        End Sub

        Public Sub GetGoodIndicationValuesIE(ByRef excl As Excelcreate, ByVal SiteId As Integer, ByVal sgoodindication As String)

            excl.AddCSVCell(Context, sgoodindication, True)

        End Sub

        Function IsEMTag(ByVal TypeId As Integer) As Boolean

            If TypeId = enumTagSubType_New.AmbientSensor_TempHumidity Or TypeId = enumTagSubType_New.AmbientSensor_TempHumNonDsp Or TypeId = enumTagSubType_New.CO2Sensor_Display Or TypeId = enumTagSubType_New.CO2Sensor_G1Display Or TypeId = enumTagSubType_New.DiffAirPressureSensor_G1 Or TypeId = enumTagSubType_New.DifferentialAirPressureSensor Or TypeId = enumTagSubType_New.O2Sensor_Display Or TypeId = enumTagSubType_New.O2Sensor_G1Display Or TypeId = enumTagSubType_New.TemperatureSensor_G1NonDsp Or TypeId = enumTagSubType_New.TemperatureSensor_Standard Or TypeId = enumTagSubType_New.TemperatureSensor_StndNonDsp Or TypeId = enumTagSubType_New.TemperatureSensor_Ultra_Low Or TypeId = enumTagSubType_New.TemperatureSensor_VAC Then

                Return True

            End If

            Return False

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : MakeEMTAGCSV                                                                                                                              '
        ' Input         : Datatable , devicetype,Bin,Type                                                                                                      ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Function MakeCSVforEMTAG(ByVal dt As DataTable, ByVal SiteId As Integer) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim Version As String = ""

            Dim sBin As String

            Dim Units As String = ""
            Dim CatastrophicCases As String = ""
            Dim bRMA As Boolean = False

            Dim nGoodCnt As Integer = 0
            Dim n30DayCnt As Integer = 0
            Dim n90DayCnt As Integer = 0
            Dim nRMACnt As Integer = 0
            Dim nOfflneCnt As Integer = 0
            Dim nCheckCnt As Integer = 0
            Dim RMADate As String = ""

            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            Try

                If dt.Rows.Count > 0 Then

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            CatastrophicCases = .Item("CatastrophicCases")

                            sBin = "Good"

                            bRMA = False
                            RMADate = .Item("RMADate")

                            If .Item("InRMA") = "Yes" And .Item("offline") = 1 And RMADate <> "" Then
                                If CDate(.Item("LastSeen")) < CDate(.Item("RMADate")) Then
                                    bRMA = True
                                End If
                            End If

                            If .Item("offline") = 1 Or .Item("PhantomData") > 0 Or bRMA Then
                                If bRMA = True Then
                                    nRMACnt += 1
                                ElseIf .Item("offline") = 1 And .Item("PhantomData") = 0 Then
                                    nOfflneCnt += 1
                                ElseIf .Item("PhantomData") > 0 Then
                                    nCheckCnt += 1
                                End If
                            Else
                                If (CatastrophicCases = "1" Or CatastrophicCases = "2") Then
                                    n30DayCnt += 1
                                ElseIf (CatastrophicCases = "4") Then
                                    n90DayCnt += 1
                                ElseIf (CatastrophicCases = "0") Then
                                    nGoodCnt += 1
                                End If

                            End If
                        End With
                    Next

                    scontext.Append(CSVCell("Report Type: EM Tag Report"))
                    scontext.Append(CSVNewLine())

                    scontext.Append(CSVCell("Total Tags: " & dt.Rows.Count))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("Good  " & nGoodCnt))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("30 Day: " & n30DayCnt))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("90 Day: " & n90DayCnt))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("Offline : " & nOfflneCnt))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("RMA: " & nRMACnt))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("Check [Less than 50 hits in the past 30 days]: " & nCheckCnt, True))
                    scontext.Append(CSVNewLine())

                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("Type", True))
                    scontext.Append(CSVCell("Model Number", True))
                    scontext.Append(CSVCell("TAG ID", True))
                    scontext.Append(CSVCell("Battery Replacement On", True))
                    scontext.Append(CSVCell("Ship Date", True))
                    scontext.Append(CSVCell("ADC Value", True))
                    scontext.Append(CSVCell("Voltage", True))
                    scontext.Append(CSVCell("Online/Offline status", True))
                    scontext.Append(CSVCell("P1 ID", True))
                    scontext.Append(CSVCell("P1 Model Number", True))
                    scontext.Append(CSVCell("P2 ID", True))
                    scontext.Append(CSVCell("P2 Model Number", True))
                    scontext.Append(CSVCell("Local ID", True))
                    scontext.Append(CSVCell("Cal Frequency", True))
                    scontext.Append(CSVCell("Last Cal Date", True))
                    scontext.Append(CSVCell("Expiration Date", True))
                    scontext.Append(CSVCell("Client Cal Due", True))
                    scontext.Append(CSVCell("Forecast Expiration date", True))
                    scontext.Append(CSVCell("Last Seen", True))
                    scontext.Append(CSVCell("Bin", True))
                    scontext.Append(CSVCell("RMA Date", True))
                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            Units = ""
                            CatastrophicCases = ""
                            sBin = ""

                            CatastrophicCases = .Item("CatastrophicCases")

                            If (CatastrophicCases = "1" Or CatastrophicCases = "2") Then
                                sBin = "30 Day"
                            ElseIf (CatastrophicCases = "4") Then
                                sBin = "90 Day"
                            ElseIf (CatastrophicCases = "0") Then
                                sBin = "Good"
                            End If

                            If .Item("offline") = 1 Then
                                sBin = "Offline"
                            End If

                            If .Item("PhantomData") > 0 Then
                                sBin = "Check"
                            End If

                            bRMA = False
                            RMADate = .Item("RMADate")

                            If .Item("InRMA") = "Yes" And .Item("offline") = 1 And RMADate <> "" Then
                                If CDate(.Item("LastSeen")) < CDate(.Item("RMADate")) Then
                                    bRMA = True
                                End If
                            End If

                            If bRMA = True Then
                                sBin = "RMA"
                            End If

                            scontext.Append(CSVCell(.Item("Sitename").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("TagType"), True))
                            scontext.Append(CSVCell(.Item("ModelItem"), True))
                            scontext.Append(CSVCell(.Item("TagId"), True))
                            scontext.Append(CSVCell(.Item("BatteryReplacementOn"), True))
                            scontext.Append(CSVCell(.Item("ShipDate"), True))
                            scontext.Append(CSVCell(.Item("ADCValue"), True))
                            scontext.Append(CSVCell(.Item("Voltage"), True))

                            If .Item("offline") = 1 Then
                                scontext.Append(CSVCell("Offline", True))
                            Else
                                scontext.Append(CSVCell("Online", True))
                            End If

                            scontext.Append(CSVCell(.Item("ProbeId"), True))
                            scontext.Append(CSVCell(.Item("P1ModelNumber"), True))
                            scontext.Append(CSVCell(.Item("ProbeId2"), True))
                            scontext.Append(CSVCell(.Item("P2ModelNumber"), True))
                            scontext.Append(CSVCell(.Item("LocalId").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("CalFrequency"), True))

                            If sBin <> "RMA" Then
                                scontext.Append(CSVCell(.Item("CertificateDate"), True))
                                scontext.Append(CSVCell(.Item("MFRCalibrationDue"), True))
                                scontext.Append(CSVCell(.Item("ClientCalDue"), True))
                                scontext.Append(CSVCell(.Item("MFRCalibrationDueNew"), True))
                            Else
                                scontext.Append(CSVCell("", True))
                                scontext.Append(CSVCell("", True))
                                scontext.Append(CSVCell("", True))
                                scontext.Append(CSVCell("", True))
                            End If

                            scontext.Append(CSVCell(.Item("LastSeen"), True))
                            scontext.Append(CSVCell(sBin, True))

                            If bRMA = True Then
                                scontext.Append(CSVCell(.Item("RMADate"), True))
                            Else
                                scontext.Append(CSVCell("", True))
                            End If

                        End With

                        scontext.Append(CSVNewLine())
                    Next
                Else
                    scontext.Append(CSVCell("Report Type : EM Tag Report"))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforEMTAG file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : MakeEMTAGCSV                                                                                                                              '
        ' Input         : Datatable , devicetype,Bin,Type                                                                                                      ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Function MakeCSVforEMTagDetail(ByVal dt As DataTable, ByVal EMReportType As String) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Try

                If EMReportType = "3" Then

                    sFileName = "EM-Tag-Detailed-Site-Report" & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                    scontext.Append(CSVCell("SiteId", True))
                    scontext.Append(CSVCell("Sitename", True))
                    scontext.Append(CSVCell("Tag Id", True))
                    scontext.Append(CSVCell("Model Item", True))
                    scontext.Append(CSVCell("TagType", True))
                    scontext.Append(CSVCell("Version", True))
                    scontext.Append(CSVCell("Location Count", True))
                    scontext.Append(CSVCell("Paging Count", True))
                    scontext.Append(CSVCell("Wifi Data Count", True))
                    scontext.Append(CSVCell("Operating Mode", True))
                    scontext.Append(CSVCell("Firstseen", True))
                    scontext.Append(CSVCell("Lastseen", True))
                    scontext.Append(CSVNewLine())

                ElseIf EMReportType = "2" Then

                    sFileName = "EM-Tag-Summary-Report" & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                    scontext.Append(CSVCell("SiteId", True))
                    scontext.Append(CSVCell("Sitename", True))
                    scontext.Append(CSVCell("EM Count", True))
                    scontext.Append(CSVNewLine())
                Else

                    sFileName = "EM-Sensor-Engineering-Detail-Report" & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                    scontext.Append(CSVCell("Sensor ID", True))
                    scontext.Append(CSVCell("Model Number", True))
                    scontext.Append(CSVCell("Software Version", True))
                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("Probe 1 Reading", True))
                    scontext.Append(CSVCell("Probe 1 Reading (24 hr average)", True))
                    scontext.Append(CSVCell("Probe 1 Reading Status", True))
                    scontext.Append(CSVCell("Probe 2 Reading", True))
                    scontext.Append(CSVCell("Probe 2 Reading (24 hr average)", True))
                    scontext.Append(CSVCell("Probe 2 Reading Status", True))
                    scontext.Append(CSVCell("First Seen", True))
                    scontext.Append(CSVCell("Last Seen", True))
                    scontext.Append(CSVCell("LBI Diff", True))
                    scontext.Append(CSVCell("Bin", True))
                    scontext.Append(CSVCell("LBI/Voltage", True))
                    scontext.Append(CSVCell("Page Request", True))
                    scontext.Append(CSVCell("Star Address", True))
                    scontext.Append(CSVCell("Probe 1 Alert Min", True))
                    scontext.Append(CSVCell("Probe 1 Alert Max", True))
                    scontext.Append(CSVCell("Probe 2 Alert Min", True))
                    scontext.Append(CSVCell("Probe 2 Alert Max", True))
                    scontext.Append(CSVCell("Door Ajar Status 1", True))
                    scontext.Append(CSVCell("Door Ajar Status 2", True))
                    scontext.Append(CSVCell("Report Rate", True))
                    scontext.Append(CSVCell("Measurement Rate", True))
                    scontext.Append(CSVCell("Date placed in the Good bin", True))
                    scontext.Append(CSVCell("Date placed in the LBI bin", True))
                    scontext.Append(CSVCell("LBI Value in the Good bin", True))
                    scontext.Append(CSVCell("LBI Value in the LBI bin", True))
                    'scontext.Append(CSVCell("Date flagged for RMA", True))
                    scontext.Append(CSVCell("LBI ADC when flagged for the RMA", True))
                    scontext.Append(CSVCell("No of Battery Replacements", True))
                    scontext.Append(CSVCell("Battery Replacement Date", True))
                    scontext.Append(CSVCell("Current Status", True))
                    scontext.Append(CSVCell("Last daily median LBI ADC value", True))
                    scontext.Append(CSVNewLine())
                End If

                If dt.Rows.Count > 0 Then

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            If EMReportType = "3" Then

                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteId")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteName")).Replace(",", ""), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("ModelItem")).Replace(",", ""), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("TagType")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SoftwareVersion")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LocationCount")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("PageRequest")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("WifiDataCount")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("OperatingMode")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("FirstSeen")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LastSeen")), True))

                            ElseIf EMReportType = "2" Then

                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteId")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteName")).Replace(",", ""), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("EMCount")), True))
                            Else

                                scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("ModelItem")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SoftwareVersion")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteName")).Replace(",", ""), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe1Reading")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe1ReadingAverage")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe1ReadingStatus")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe2Reading")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe2ReadingAverage")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe2ReadingStatus")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("FirstSeen")).Replace(",", ""), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LastSeen")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LBIDiff")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Bin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LBIVoltage")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("PageRequest")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("StarAddress")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe1AlertMin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe1AlertMax")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe2AlertMin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Probe2AlertMax")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("DoorAjarStatus1")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("DoorAjarStatus2")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("ReportRate")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("MeasurementRate")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("DateplacedintheGoodbin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("DateplacedintheLBIbin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LBIValueintheGoodbin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LBIValueintheLBIbin")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("DateOfRMA")), True))
                                'scontext.Append(CSVCell("", True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("NoofBatteryReplacements")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("BatteryReplacementOn")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("IsActive")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("LastMedianLBIValue")), True))

                            End If

                        End With

                        scontext.Append(CSVNewLine())
                    Next
                Else
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforEMTagDetail file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        ' Function Name : MakeCSV                                                                                                                              '
        ' Input         : Datatable , devicetype,Bin,Type                                                                                                      ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Device type and its bin value                                                                               '
        '******************************************************************************************************************************************************'
        Private Function MakeCSVforNoRecordFound(ByVal dt As DataTable) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "All"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename"))))
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("Type:All"))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("TAG"))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("TAG ID", True))
                scontext.Append(CSVCell("MONITOR LOCATION", True))
                scontext.Append(CSVCell("MONITOR ID", True))
                scontext.Append(CSVCell("LESS THAN 90 DAYS", True))
                scontext.Append(CSVCell("LESS THAN 30 DAYS", True))
                scontext.Append(CSVCell("OFFLINE", True))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("MONITOR"))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("DEVICES", True))
                scontext.Append(CSVCell("MONITOR ID", True))
                scontext.Append(CSVCell("MONITOR LOCATION", True))
                scontext.Append(CSVCell("CHANGE IN 90 DAYS", True))
                scontext.Append(CSVCell("CHANGE IN 30 DAYS", True))
                scontext.Append(CSVCell("MODEL ITEM", True))
                scontext.Append(CSVCell("OFFLINE", True))
                scontext.Append(CSVNewLine())
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : CSVCell                                                                                                                              '
        ' Input         : stext , bColSep                                                                                                                      ' 
        ' Output        : CSV String                                                                                                                           '
        ' Description   : It will make csv cell string                                                                                                         '
        '******************************************************************************************************************************************************'
        Public Function CSVCell(ByVal stext As String, Optional ByVal bColSep As Boolean = False) As String

            Dim ret As String = ""

            'ret = sInfo
            If bColSep Then
                ret = stext & ","
            Else
                ret = stext
            End If
            Return ret

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : CSVNewLine                                                                                                                           '
        ' Input         : -                                                                                                                                    ' 
        ' Output        : Make new line for CSV file                                                                                                           '
        ' Description   : It will make csv Neline                                                                                                         '
        '******************************************************************************************************************************************************'
        Public Function CSVNewLine() As String

            Dim ret As String = ""
            ret = ret & vbCrLf
            Return ret

        End Function
        Private Function MakeCSVForAssetPage(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Asset-Track"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If ToDate = "" Then
                ToDate = Now.ToString("yyyy/MM/dd HH:mm")
            End If

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename"))))
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("From Date : " & FromDate & " " & " " & "To Date : " & ToDate))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("ID NUMBER", True))
                scontext.Append(CSVCell("DEVICE TYPE", True))
                scontext.Append(CSVCell("DEVICE DESCRIPTION", True))
                scontext.Append(CSVCell("LOCATION [MONITORID]", True))
                scontext.Append(CSVCell("LAST LOCATION [MONITORID]", True))
                scontext.Append(CSVCell("ENTERED ON", True))
                scontext.Append(CSVCell("LEFT ON", True))
                scontext.Append(CSVCell("TIME SPENT", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagType")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagName")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("CurrentLocation")) & " " & "[" & CheckIsDBNull(.Item("MonitorId")) & "]", True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("LastLocation")) & " " & "[" & CheckIsDBNull(.Item("LastRoom")) & "]", True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("EnteredOn")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("LeftOn")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TimeSpend")), True))
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        Private Function NoRcordCSVForAssetPage(ByVal dt As DataTable, ByVal TagId As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Asset-Track"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy")

            If ToDate = "" Then
                ToDate = Now.ToString("yyyy/MM/dd HH:mm")
            End If

            scontext.Append(CSVCell("Tag Id : " & TagId))
            scontext.Append(CSVNewLine())
            scontext.Append(CSVCell("From Date : " & FromDate & " " & " " & "To Date : " & ToDate))
            scontext.Append(CSVNewLine())

            scontext.Append(CSVCell("ID NUMBER", True))
            scontext.Append(CSVCell("DEVICE TYPE", True))
            scontext.Append(CSVCell("DEVICE DESCRIPTION", True))
            scontext.Append(CSVCell("LOCATION [MONITORID]", True))
            scontext.Append(CSVCell("LAST LOCATION [MONITORID]", True))
            scontext.Append(CSVCell("ENTERED ON", True))
            scontext.Append(CSVCell("LEFT ON", True))
            scontext.Append(CSVCell("TIME SPENT", True))

            scontext.Append(CSVNewLine())

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        Private Sub AssetTagListPrepareCSVForIE(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String)
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder

            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = ""
            Dim isTagdataFound As Boolean = True
            Dim isMonitorDataFound As Boolean = True

            Try

                If ToDate = "" Then
                    ToDate = Now.ToString("yyyy/MM/dd HH:mm")
                End If

                Sheetname = "Asset-Track"

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                excl.InitiateCSV(Context, sFileName)

                If dt.Rows.Count > 0 Then
                    Dim context As HttpContext
                    context = HttpContext.Current

                    excl.AddCSVCell(context, "Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")), True)
                    excl.AddCSVNewLine(context)
                    excl.AddCSVCell(context, "From Date : " & FromDate & " " & " " & "To Date : " & ToDate)
                    excl.AddCSVNewLine(context)

                    excl.AddCSVCell(context, "ID NUMBER", True)
                    excl.AddCSVCell(context, "DEVICE TYPE", True)
                    excl.AddCSVCell(context, "DEVICE DESCRIPTION", True)
                    excl.AddCSVCell(context, "LOCATION [MONITORID]", True)
                    excl.AddCSVCell(context, "LAST LOCATION [MONITORID]", True)
                    excl.AddCSVCell(context, "ENTERED ON", True)
                    excl.AddCSVCell(context, "LEFT ON", True)
                    excl.AddCSVCell(context, "TIME SPENT", True)

                    excl.AddCSVNewLine(context)

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagId")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagType")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagName")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("CurrentLocation")) & " " & "[" & CheckIsDBNull(.Item("MonitorId")) & "]", True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("LastLocation")) & " " & "[" & CheckIsDBNull(.Item("LastRoom")) & "]", True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("EnteredOn")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("LeftOn")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TimeSpend")), True)
                            excl.AddCSVNewLine(context)
                        End With
                    Next
                End If

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
        End Sub
        Private Sub PrepareAssetTagListCSVForNoRecordFound(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal TagId As String)
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder

            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = ""

            Try

                If ToDate = "" Then
                    ToDate = Now.ToString("yyyy/MM/dd HH:mm:ss")
                End If

                Sheetname = "Asset-Track"
                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                excl.InitiateCSV(Context, sFileName)

                Dim hContext As HttpContext
                hContext = HttpContext.Current

                excl.AddCSVCell(Context, "Tag Id : " & TagId)
                excl.AddCSVNewLine(hContext)
                excl.AddCSVCell(Context, "From Date : " & FromDate & " " & " " & "To Date : " & ToDate)
                excl.AddCSVNewLine(hContext)

                excl.AddCSVCell(Context, "ID NUMBER", True)
                excl.AddCSVCell(Context, "DEVICE TYPE", True)
                excl.AddCSVCell(Context, "DEVICE DESCRIPTION", True)
                excl.AddCSVCell(Context, "LOCATION [MONITORID]", True)
                excl.AddCSVCell(Context, "LAST LOCATION [MONITORID]", True)
                excl.AddCSVCell(Context, "ENTERED ON", True)
                excl.AddCSVCell(Context, "LEFT ON", True)
                excl.AddCSVCell(Context, "TIME SPENT", True)
                excl.AddCSVNewLine(hContext)

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
        End Sub
        Private Function MakeCSVForBatterySummary(ByVal dt As DataTable, ByVal TypeName As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim previousyear As String = ""

            Sheetname = "Battery-Summary"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")).Replace(",", "")))
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("Type : " & TypeName & " Battery Summary"))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("YEAR", True))
                scontext.Append(CSVCell("QUANTITY", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        If .Item("Estimated") <> previousyear Then
                            If .Item("Estimated") = "0.5" Then
                                scontext.Append(CSVCell("6 Month", True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Quantity")), True))
                            ElseIf .Item("Estimated") = "N/A" Then
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Estimated")), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Quantity")), True))
                            ElseIf .Item("Estimated") = "5.5" Then
                                scontext.Append(CSVCell("5Yrs +", True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Quantity")), True))
                            Else
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Estimated") & " " & "Year"), True))
                                scontext.Append(CSVCell(CheckIsDBNull(.Item("Quantity")), True))
                            End If
                            scontext.Append(CSVNewLine())
                        End If
                        previousyear = .Item("Estimated")
                    End With
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
        Private Sub BatterySummaryPrepareCSVForIE(ByVal dt As DataTable, ByVal TypeName As String)
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder

            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = ""
            Dim isTagdataFound As Boolean = True
            Dim isMonitorDataFound As Boolean = True
            Dim previousyear As String = ""

            Try

                Sheetname = "Battery-Summary"

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                excl.InitiateCSV(Context, sFileName)

                If dt.Rows.Count > 0 Then
                    Dim context As HttpContext
                    context = HttpContext.Current

                    excl.AddCSVCell(context, "Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")).Replace(",", ""), True)
                    excl.AddCSVNewLine(context)
                    excl.AddCSVCell(context, "Type : " & TypeName & " Battery Summary")
                    excl.AddCSVNewLine(context)

                    excl.AddCSVCell(context, "YEAR", True)
                    excl.AddCSVCell(context, "QUANTITY", True)

                    excl.AddCSVNewLine(context)

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            If .Item("Estimated") <> previousyear Then
                                If .Item("Estimated") = "0.5" Then
                                    excl.AddCSVCell(context, "6 Month", True)
                                    excl.AddCSVCell(context, CheckIsDBNull(.Item("Quantity")), True)
                                ElseIf .Item("Estimated") = "N/A" Then
                                    excl.AddCSVCell(context, CheckIsDBNull(.Item("Estimated")), True)
                                    excl.AddCSVCell(context, CheckIsDBNull(.Item("Quantity")), True)
                                ElseIf .Item("Estimated") = "5.5" Then
                                    excl.AddCSVCell(context, "5Yrs +", True)
                                    excl.AddCSVCell(context, CheckIsDBNull(.Item("Quantity")), True)
                                Else
                                    excl.AddCSVCell(context, CheckIsDBNull(.Item("Estimated") & " " & "Year"), True)
                                    excl.AddCSVCell(context, CheckIsDBNull(.Item("Quantity")), True)
                                End If
                                excl.AddCSVNewLine(context)
                            End If
                            previousyear = .Item("Estimated")
                        End With
                    Next
                End If

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
        End Sub
        Private Function MakeCSVForBatteryList(ByVal dt As DataTable, ByVal TypeName As String, ByVal devicetype As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Battery-Summary"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")).Replace(",", "")))
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("Type : " & TypeName))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Filterd Location", True))
                scontext.Append(CSVCell("Details", True))
                If devicetype = enumDeviceType.Tag Then
                    scontext.Append(CSVCell("TagId", True))
                Else
                    scontext.Append(CSVCell("MonitorId", True))
                End If

                scontext.Append(CSVCell("Remaining (approx) BatteryLife", True))

                If devicetype = enumDeviceType.Tag Then
                    scontext.Append(CSVCell("Update Rate", True))
                    scontext.Append(CSVCell("Activity Level", True))
                Else
                    scontext.Append(CSVCell("Power Level", True))
                    scontext.Append(CSVCell("Noise Level", True))
                    scontext.Append(CSVCell("Update Rate", True))
                End If

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Location")), True))
                        If devicetype = enumDeviceType.Tag Then
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("TagTypeName")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                        Else
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorTypeName")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorId")), True))
                        End If

                        If .Item("Estimated") = "0.5" Then
                            scontext.Append(CSVCell("6 Month", True))
                        ElseIf .Item("Estimated") = "1" Then
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("Estimated")) & " " & "Year", True))
                        ElseIf .Item("Estimated") = "N/A" Then
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("Estimated")), True))
                        ElseIf .Item("Estimated") = "5.5" Then
                            scontext.Append(CSVCell(CheckIsDBNull("5Yrs +"), True))
                        Else
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("Estimated")) & " " & "Years", True))
                        End If

                        If devicetype = enumDeviceType.Tag Then
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("UpdateRate")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("ActivityLevel")), True))
                        Else
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("PowerLevel")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("NoiseLevel")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("UpdateRate")), True))
                        End If

                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
        Private Sub MakeCSVForBatteryList_IE(ByVal dt As DataTable, ByVal TypeName As String, ByVal devicetype As String)
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder
            Dim sFileName As String
            Dim Sheetname As String = ""
            Dim strProfiles As String = ""

            Try

                Sheetname = "Battery-Summary"
                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                excl.InitiateCSV(Context, sFileName)

                If dt.Rows.Count > 0 Then
                    Dim context As HttpContext
                    context = HttpContext.Current

                    excl.AddCSVCell(context, "Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")).Replace(",", ""), True)
                    excl.AddCSVNewLine(context)
                    excl.AddCSVCell(context, "Type : " & TypeName)
                    excl.AddCSVNewLine(context)

                    excl.AddCSVCell(context, "Filterd Location", True)
                    excl.AddCSVCell(context, "Details", True)

                    If devicetype = enumDeviceType.Tag Then
                        excl.AddCSVCell(context, "TagId", True)
                    Else
                        excl.AddCSVCell(context, "MonitorId", True)
                    End If

                    excl.AddCSVCell(context, "Remaining (approx) BatteryLife", True)

                    If devicetype = enumDeviceType.Tag Then
                        excl.AddCSVCell(context, "Update Rate", True)
                        excl.AddCSVCell(context, "Activity Level", True)
                    Else
                        excl.AddCSVCell(context, "Power Level", True)
                        excl.AddCSVCell(context, "Noise Level", True)
                        excl.AddCSVCell(context, "Update Rate", True)
                    End If

                    excl.AddCSVNewLine(context)

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("Location")), True)

                            If devicetype = enumDeviceType.Tag Then
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("TagTypeName")), True)
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("TagId")), True)
                            ElseIf devicetype = enumDeviceType.Monitor Then
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("MonitorTypeName")), True)
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("MonitorId")), True)
                            End If

                            If .Item("Estimated") = "0.5" Then
                                excl.AddCSVCell(context, "6 Month", True)
                            ElseIf .Item("Estimated") = "1" Then
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("Estimated") & " " & "Year"), True)
                            ElseIf .Item("Estimated") = "N/A" Then
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("Estimated")), True)
                            ElseIf .Item("Estimated") = "5.5" Then
                                excl.AddCSVCell(context, CheckIsDBNull("5Yrs +"), True)
                            Else
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("Estimated") & " " & "Years"), True)
                            End If

                            If devicetype = enumDeviceType.Tag Then
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("UpdateRate")), True)
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("ActivityLevel")), True)
                            Else
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("PowerLevel")), True)
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("NoiseLevel")), True)
                                excl.AddCSVCell(context, CheckIsDBNull(.Item("UpdateRate")), True)
                            End If
                        End With
                        excl.AddCSVNewLine(context)
                    Next
                End If

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
        End Sub
        Private Function PrepareCSVForBatteryTechLbiList(ByVal dt As DataTable) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim SiteName As String = ""
            Dim SSiteCorrectFormat As String = ""

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    SiteName = dt.Rows(0).Item("Sitename").ToString.Replace("&", "-")
                    SSiteCorrectFormat = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(SiteName))
                    SSiteCorrectFormat = SSiteCorrectFormat.Replace("?", "-")
                    SSiteCorrectFormat = SSiteCorrectFormat.Replace(" ", "")
                    Sheetname = SSiteCorrectFormat
                End If
            End If

            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Now.ToString("MM-dd-yyyy")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("TAG TYPE", True))
                scontext.Append(CSVCell("ITEM NAME", True))
                scontext.Append(CSVCell("TAG ID", True))
                scontext.Append(CSVCell("FLOOR", True))
                scontext.Append(CSVCell("LOCATION", True))
                scontext.Append(CSVCell("IN LOCATION", True))
                scontext.Append(CSVCell("BATTERY REPLACEMENT ON", True))
                scontext.Append(CSVCell("COMMENTS", False))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagType")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagTypeName")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Floor")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Location")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("InLocationHrs")), False))
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
        Private Sub PrepareCSVForBatteryTechLbiListForIE(ByVal dt As DataTable)
            Dim excl As New Excelcreate
            Dim scontext As New StringBuilder
            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = ""
            Dim isTagdataFound As Boolean = True
            Dim isMonitorDataFound As Boolean = True

            Try

                Dim SiteName As String = ""
                Dim SSiteCorrectFormat As String = ""

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        SiteName = dt.Rows(0).Item("Sitename").ToString.Replace("&", "-")
                        SSiteCorrectFormat = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(SiteName))
                        SSiteCorrectFormat = SSiteCorrectFormat.Replace("?", "-")
                        SSiteCorrectFormat = SSiteCorrectFormat.Replace(" ", "")
                        Sheetname = SSiteCorrectFormat
                    End If
                End If

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Now.ToString("MM-dd-yyyy")

                excl.InitiateCSV(Context, sFileName)

                If dt.Rows.Count > 0 Then
                    Dim context As HttpContext
                    context = HttpContext.Current

                    excl.AddCSVCell(context, "TAG TYPE", True)
                    excl.AddCSVCell(context, "ITEM NAME", True)
                    excl.AddCSVCell(context, "TAG ID", True)
                    excl.AddCSVCell(context, "FLOOR", True)
                    excl.AddCSVCell(context, "LOCATION", True)
                    excl.AddCSVCell(context, "IN LOCATION", True)
                    excl.AddCSVCell(context, "BATTERY REPLACEMENT ON", True)
                    excl.AddCSVCell(context, "COMMENTS", False)

                    excl.AddCSVNewLine(context)

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagType")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagTypeName")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagId")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("Floor")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("Location")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("InLocationHrs")), False)
                            excl.AddCSVNewLine(context)
                        End With
                    Next
                End If

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog("Exception PrepareCSVForBatteryTechLbiListForIE file " & ex.Message.ToString())
            End Try
        End Sub
        Private Sub NoRecordFoundForBatteryTech_IE()
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder
            Dim sFileName As String
            Dim Sheetname As String = ""

            Try

                sFileName = "CenTrak-GMS-" & Now.ToString("MM-dd-yyyy")

                excl.InitiateCSV(Context, sFileName)

                Dim contexst As HttpContext
                contexst = HttpContext.Current
                excl.AddCSVCell(Context, "TAG TYPE", True)
                excl.AddCSVCell(Context, "ITEM NAME", True)
                excl.AddCSVCell(Context, "TAG ID", True)
                excl.AddCSVCell(Context, "FLOOR", True)
                excl.AddCSVCell(Context, "LOCATION", True)
                excl.AddCSVCell(Context, "IN LOCATION", True)
                excl.AddCSVCell(Context, "BATTERY REPLACEMENT ON", True)
                excl.AddCSVCell(Context, "COMMENTS", False)
                excl.AddCSVNewLine(Context)

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog("Exception BatteryTechNoRecordFoundForIE file " & ex.Message.ToString())
            End Try
        End Sub
        Private Function NoRecordFoundForBatteryTechLbiList() As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            sFileName = "CenTrak-GMS-" & Now.ToString("MM-dd-yyyy")

            scontext.Append(CSVCell("TAG TYPE", True))
            scontext.Append(CSVCell("ITEM NAME", True))
            scontext.Append(CSVCell("TAG ID", True))
            scontext.Append(CSVCell("FLOOR", True))
            scontext.Append(CSVCell("LOCATION", True))
            scontext.Append(CSVCell("IN LOCATION", True))
            scontext.Append(CSVCell("BATTERY REPLACEMENT ON", True))
            scontext.Append(CSVCell("COMMENTS", False))

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
        Private Function MakeCSVReportForBatteryTech(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim SiteName As String = ""

            Sheetname = "Battery-Replacement-Report"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Now.ToString("MM-dd-yyyy")

            If ToDate = "" Then
                ToDate = Now.ToString("yyyy/MM/dd HH:mm")
            End If

            If (dt.Rows.Count > 0) Then
                SiteName = CheckIsDBNull(dt.Rows(0).Item("Sitename"), , "").Replace(" ", "")
                scontext.Append(CSVCell("Site Name : " & SiteName))
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("From Date : " & FromDate & " " & " " & "To Date : " & ToDate))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("TAG ID", True))
                scontext.Append(CSVCell("TAG TYPE", True))
                scontext.Append(CSVCell("LOCATION", True))
                scontext.Append(CSVCell("BATTERY REPLACEMENT ON", True))
                scontext.Append(CSVCell("BATTERY REPLACED BY", True))
                scontext.Append(CSVCell("COMMENTS"))
                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagType")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Location")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("BatteryReplacementOn")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("BatteryReplacedby")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Comments")), False))
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
        Private Function NoRecord_ReportForBatteryTech(ByVal SiteName As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Battery-Replacement-Report"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Now.ToString("MM-dd-yyyy")

            If ToDate = "" Then
                ToDate = Now.ToString("yyyy/MM/dd HH:mm")
            End If

            scontext.Append(CSVCell("Site Name : " & SiteName))
            scontext.Append(CSVNewLine())
            scontext.Append(CSVCell("From Date : " & FromDate & " " & " " & "To Date : " & ToDate))
            scontext.Append(CSVNewLine())

            scontext.Append(CSVCell("TAG ID", True))
            scontext.Append(CSVCell("TAG TYPE", True))
            scontext.Append(CSVCell("LOCATION", True))
            scontext.Append(CSVCell("BATTERY REPLACEMENT ON", True))
            scontext.Append(CSVCell("BATTERY REPLACED BY", True))
            scontext.Append(CSVCell("COMMENTS"))
            scontext.Append(CSVNewLine())

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
        Private Sub MakeCSVReportForBatteryTech_IE(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String)
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder

            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = ""
            Dim isTagdataFound As Boolean = True
            Dim isMonitorDataFound As Boolean = True

            Try

                If ToDate = "" Then
                    ToDate = Now.ToString("yyyy/MM/dd HH:mm")
                End If

                Sheetname = "Battery-Replacement-Report"
                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Now.ToString("MM-dd-yyyy")

                excl.InitiateCSV(Context, sFileName)

                If dt.Rows.Count > 0 Then
                    Dim context As HttpContext
                    context = HttpContext.Current

                    excl.AddCSVCell(context, "Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")), True)
                    excl.AddCSVNewLine(context)
                    excl.AddCSVCell(context, "From Date : " & FromDate & " " & " " & "To Date : " & ToDate)
                    excl.AddCSVNewLine(context)

                    excl.AddCSVCell(context, "TAG ID", True)
                    excl.AddCSVCell(context, "TAG TYPE", True)
                    excl.AddCSVCell(context, "LOCATION", True)
                    excl.AddCSVCell(context, "BATTERY REPLACEMENT ON", True)
                    excl.AddCSVCell(context, "BATTERY REPLACED BY", True)
                    excl.AddCSVCell(context, "COMMENTS", False)
                    excl.AddCSVNewLine(context)

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagId")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("TagType")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("Location")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("BatteryreplacementOn")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("BatteryReplacedby")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("Comments")), False)
                            excl.AddCSVNewLine(context)
                        End With
                    Next
                End If

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
        End Sub
        Private Sub NoRecordTagReportForBatteryTech_IE(ByVal SiteName As String, ByVal FromDate As String, ByVal ToDate As String)
            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder
            Dim sFileName As String
            Dim Sheetname As String = ""

            Try
                If ToDate = "" Then
                    ToDate = Now.ToString("yyyy/MM/dd HH:mm")
                End If

                Sheetname = "Battery-Replacement-Report"

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Now.ToString("MM-dd-yyyy")

                excl.InitiateCSV(Context, sFileName)
                excl.AddCSVCell(Context, "Site Name : " & SiteName)
                excl.AddCSVNewLine(Context)
                excl.AddCSVCell(Context, "From Date : " & FromDate & " " & " " & "To Date : " & ToDate)
                excl.AddCSVNewLine(Context)

                Dim contexst As HttpContext
                contexst = HttpContext.Current
                excl.AddCSVCell(Context, "TAG ID", True)
                excl.AddCSVCell(Context, "TAG TYPE", True)
                excl.AddCSVCell(Context, "LOCATION", True)
                excl.AddCSVCell(Context, "BATTERY REPLACEMENT ON", True)
                excl.AddCSVCell(Context, "BATTERY REPLACED BY", True)
                excl.AddCSVCell(Context, "COMMENTS", False)
                excl.AddCSVNewLine(Context)

                Context.Response.End()
            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog("Exception NoRecordTagReportForBatteryTech_IE file " & ex.Message.ToString())
            End Try
        End Sub

        Private Function MakeCSVReportForAnnualCalibration(ByVal dt As DataTable, ByVal DeviceType As Integer) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Annual-Calibration-Report"

            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy")

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    scontext.Append(CSVCell("Report : Annual-Calibration-Report"))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")).Replace(",", "")))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVNewLine())

                    If DeviceType = enumDeviceType.Tag Then
                        scontext.Append(CSVCell("TAG ID", True))
                        scontext.Append(CSVCell("LAST CALIBRATION DATE", True))
                        scontext.Append(CSVNewLine())
                    End If

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            If CheckIsDBNull(.Item("TagRecalibrationDate"), , "") <> "" Then

                                If .Item("IsAnnualCalibration") = 1 Then
                                    If DateDiff("d", CheckIsDBNull(.Item("TagRecalibrationDate"), , CDate("01/01/1900")), Now.Date) >= 365 Then
                                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagRecalibrationDate"), False, ""), True))
                                        scontext.Append(CSVNewLine())
                                    End If
                                Else
                                    If DateDiff("d", CheckIsDBNull(.Item("TagRecalibrationDate"), , CDate("01/01/1900")), Now.Date) >= 730 Then
                                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagId")), True))
                                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TagRecalibrationDate"), False, ""), True))
                                        scontext.Append(CSVNewLine())
                                    End If

                                End If
                            End If
                        End With
                    Next
                Else

                    scontext.Append(CSVCell("Report : Annual-Calibration-Report"))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("Site Name : "))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVNewLine())

                    If DeviceType = enumDeviceType.Tag Then
                        scontext.Append(CSVCell("TAG ID", True))
                        scontext.Append(CSVCell("LAST CALIBRATION DATE", True))
                        scontext.Append(CSVNewLine())
                    End If

                End If
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function MakeCSVForTTSyncErrReport(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal SiteName As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "_TTSyncERRORReport"
            sFileName = SiteName.Replace("-", "").Replace(" ", "") & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & SiteName.Replace(",", "")))
                scontext.Append(CSVNewLine())

                If FromDate <> "" Then
                    scontext.Append(CSVCell("Date : " & FromDate))
                    scontext.Append(CSVNewLine())
                End If
                'scontext.Append(CSVCell("From Date : " & FromDate & " " & " " & "To Date : " & ToDate))

                scontext.Append(CSVCell("Star Id", True))
                scontext.Append(CSVCell("Version", True))
                scontext.Append(CSVCell("Beacon Slot", True))
                scontext.Append(CSVCell("Star Type", True))
                scontext.Append(CSVCell("Mac Id", True))
                scontext.Append(CSVCell("IP Address", True))
                scontext.Append(CSVCell("Updated On", True))
                scontext.Append(CSVCell("Response Cnt", True))
                scontext.Append(CSVCell("Paging Cnt", True))
                scontext.Append(CSVCell("Paging Data Cnt", True))
                scontext.Append(CSVCell("Location Cnt", True))
                scontext.Append(CSVCell("Location Data Cnt", True))
                scontext.Append(CSVCell("TT Sync Error", True))
                scontext.Append(CSVCell("Error Cnt", True))
                scontext.Append(CSVCell("Reporting Hrs", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("StarId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Version")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Slot")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("StarType")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("MacId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("IPAddr")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Updatedon")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("ResCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("PagingCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("PDataCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("LocationCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("LDataCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TTSyncError")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("ErrorCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("StarCount")), True))
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function NoRcordCSVForTTSyncErrReport(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal SiteName As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "_TTSyncERRORReport"
            sFileName = SiteName.Replace("-", "").Replace(" ", "") & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")


            If FromDate <> "" Then
                scontext.Append(CSVCell("Date : " & FromDate))
                scontext.Append(CSVNewLine())
            End If

            scontext.Append(CSVCell("Star Id", True))
            scontext.Append(CSVCell("Version", True))
            scontext.Append(CSVCell("Beacon Slot", True))
            scontext.Append(CSVCell("Star Type", True))
            scontext.Append(CSVCell("Mac Id", True))
            scontext.Append(CSVCell("IP Address", True))
            scontext.Append(CSVCell("Updated On", True))
            scontext.Append(CSVCell("Response Cnt", True))
            scontext.Append(CSVCell("Paging Cnt", True))
            scontext.Append(CSVCell("Paging Data Cnt", True))
            scontext.Append(CSVCell("Location Cnt", True))
            scontext.Append(CSVCell("Location Data Cnt", True))
            scontext.Append(CSVCell("TT Sync Error", True))
            scontext.Append(CSVCell("Error Cnt", True))
            scontext.Append(CSVCell("Reporting Hrs", True))

            scontext.Append(CSVNewLine())

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function MakeCSVForConnectivityReport(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal SiteName As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim Sno As Integer = 1

            Sheetname = "-ConnectivityReport"
            sFileName = SiteName.Replace("-", "").Replace(" ", "") & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & SiteName))
                scontext.Append(CSVNewLine())

                If FromDate <> "" Then
                    scontext.Append(CSVCell("Date : " & FromDate))
                    scontext.Append(CSVNewLine())
                End If

                scontext.Append(CSVCell("SNo", True))
                scontext.Append(CSVCell("Device Id", True))
                scontext.Append(CSVCell("IRId", True))
                scontext.Append(CSVCell("Version", True))
                scontext.Append(CSVCell("Page Count", True))
                scontext.Append(CSVCell("RF.Location Count", True))
                scontext.Append(CSVCell("Trigger Count", True))
                scontext.Append(CSVCell("Monitor did not report more than 70 mins", True))
                scontext.Append(CSVCell("Monitor did not report more than 120 mins", True))
                scontext.Append(CSVCell("Updated On", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(Convert.ToString(Sno), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("IRId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Version")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("PagingCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("RFLCount")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TriggerCnt")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Mins70")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Mins120")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("UpdatedOn")), True))
                        Sno = Sno + 1
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function NoRcordCSVForConnectivityReport(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String, ByVal SiteName As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "-ConnectivityReport"
            sFileName = SiteName.Replace("-", "").Replace(" ", "") & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            scontext.Append(CSVCell("Site Name : " & SiteName))
            scontext.Append(CSVNewLine())

            If FromDate <> "" Then
                scontext.Append(CSVCell("Date : " & FromDate))
                scontext.Append(CSVNewLine())
            End If

            scontext.Append(CSVCell("SNo", True))
            scontext.Append(CSVCell("Device Id", True))
            scontext.Append(CSVCell("IRId", True))
            scontext.Append(CSVCell("Version", True))
            scontext.Append(CSVCell("Page Count", True))
            scontext.Append(CSVCell("RF.Location Count", True))
            scontext.Append(CSVCell("Trigger Count", True))
            scontext.Append(CSVCell("Monitor did not report more than 70 mins", True))
            scontext.Append(CSVCell("Monitor did not report more than 120 mins", True))
            scontext.Append(CSVCell("Updated On", True))

            scontext.Append(CSVNewLine())

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function MakeCSVForMonitorAnalysisReport(ByVal dt As DataSet, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim Sno As Integer = 1

            Dim dtList As New DataTable
            Dim dtPaging As New DataTable
            Dim dtBeacon As New DataTable


            dtList = dt.Tables("list")
            dtPaging = dt.Tables("Monitor")
            dtBeacon = dt.Tables("MonitorCollision")

            Sheetname = "MonitorAnalysisReport"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            'Site Summary
            If (dtList.Rows.Count > 0) Then
                'scontext.Append(CSVCell("Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename"))))
                'scontext.Append(CSVNewLine())
                If FromDate <> "" Then
                    scontext.Append(CSVCell("Date : " & FromDate))
                    scontext.Append(CSVNewLine())
                End If

                scontext.Append(CSVCell("SNo", True))
                scontext.Append(CSVCell("Monitors Count Above 500 Paging", True))
                scontext.Append(CSVCell("Undefined Monitors Seen", True))
                scontext.Append(CSVCell("Total Number of Monitors Configured inIni", True))
                scontext.Append(CSVCell("Total Number of Monitors Configuredin Groups", True))
                scontext.Append(CSVCell("Total Monitors Actively Reporting", True))
                scontext.Append(CSVCell("Monitors Count in BeaconCollision", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dtList.Rows.Count - 1
                    With (dtList.Rows(nIdx))
                        scontext.Append(CSVCell(Convert.ToString(Sno), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorsCountAbove500Paging")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("UndefinedMonitorsSeen")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TotalNumberofMonitorsConfiguredinIni")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TotalNumberofMonitorsConfiguredinGroups")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TotalMonitorsActivelyReporting")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorsCountinBeaconCollision")), True))
                        Sno = Sno + 1
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Paging Summary

            If (dtPaging.Rows.Count > 0) Then
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("Paging Summary", True))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("SNo", True))
                scontext.Append(CSVCell("Date", True))
                scontext.Append(CSVCell("Device Id", True))
                scontext.Append(CSVCell("Pageing Count", True))

                scontext.Append(CSVNewLine())

                Sno = 1
                For nIdx As Integer = 0 To dtPaging.Rows.Count - 1
                    With (dtPaging.Rows(nIdx))
                        scontext.Append(CSVCell(Convert.ToString(Sno), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Date")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("DeviceId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("PagingCount")), True))

                        Sno = Sno + 1
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Beacon Collision Summary

            If (dtBeacon.Rows.Count > 0) Then
                scontext.Append(CSVNewLine())
                scontext.Append(CSVCell("Beacon Collision Summary", True))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("SNo", True))
                scontext.Append(CSVCell("Date", True))
                scontext.Append(CSVCell("Device Id", True))
                scontext.Append(CSVCell("Beacon Collision Star Count", True))

                scontext.Append(CSVNewLine())

                Sno = 1
                For nIdx As Integer = 0 To dtBeacon.Rows.Count - 1
                    With (dtBeacon.Rows(nIdx))
                        scontext.Append(CSVCell(Convert.ToString(Sno), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Date")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("DeviceId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("BeaconCollisionStarCount")), True))

                        Sno = Sno + 1
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function NoRcordCSVForMakeCSVForMonitorAnalysisReport(ByVal dt As DataTable, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "ConnectivityReport"
            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If FromDate <> "" Then
                scontext.Append(CSVCell("Date : " & FromDate))
                scontext.Append(CSVNewLine())
            End If

            scontext.Append(CSVCell("SNo", True))
            scontext.Append(CSVCell("Monitors Count Above 500 Paging", True))
            scontext.Append(CSVCell("Undefined Monitors Seen", True))
            scontext.Append(CSVCell("Total Number of Monitors Configured inIni", True))
            scontext.Append(CSVCell("Total Number of Monitors Configuredin Groups", True))
            scontext.Append(CSVCell("Total Monitors Actively Reporting", True))
            scontext.Append(CSVCell("Monitors Count in BeaconCollision", True))

            scontext.Append(CSVNewLine())

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function MakeCSVForStarOneHrReport(ByVal dt As DataTable, ByVal sDate As String, ByVal SiteName As String) As DataTable
            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "_StarOneHrReport"
            sFileName = SiteName.Replace("-", "").Replace(" ", "") & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If (dt.Rows.Count > 0) Then
                scontext.Append(CSVCell("Site Name : " & SiteName.Replace(",", "")))
                scontext.Append(CSVNewLine())

                If sDate <> "" Then
                    scontext.Append(CSVCell("Date : " & sDate))
                    scontext.Append(CSVNewLine())
                End If

                scontext.Append(CSVCell("Version", True))
                scontext.Append(CSVCell("Mac Id", True))
                scontext.Append(CSVCell("IP Address", True))
                scontext.Append(CSVCell("Updated On", True))
                scontext.Append(CSVCell("Response Cnt", True))
                scontext.Append(CSVCell("Paging Cnt", True))
                scontext.Append(CSVCell("Paging Data Cnt", True))
                scontext.Append(CSVCell("Location Cnt", True))
                scontext.Append(CSVCell("Location Data Cnt", True))
                scontext.Append(CSVCell("TT Sync Error", True))
                scontext.Append(CSVCell("Error Cnt", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Firmwareversion")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("MacId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("IPAddress")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("UpdatedOn")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("ResCount")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Pagedatareceived")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("PageDataCount")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("LocationDataCount")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("Locationdatareceived")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("TTSyncError")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("ErrorCnt")), True))
                    End With
                    scontext.Append(CSVNewLine())
                Next
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function NoRcordCSVForStarOneHrReport(ByVal dt As DataTable, ByVal sDate As String, ByVal SiteName As String) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "_StarOneHrReport"
            sFileName = SiteName.Replace("-", "").Replace(" ", "") & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            If sDate <> "" Then
                scontext.Append(CSVCell("Date : " & sDate))
                scontext.Append(CSVNewLine())
            End If

            scontext.Append(CSVCell("Version", True))
            scontext.Append(CSVCell("Mac Id", True))
            scontext.Append(CSVCell("IP Address", True))
            scontext.Append(CSVCell("Updated On", True))
            scontext.Append(CSVCell("Response Cnt", True))
            scontext.Append(CSVCell("Paging Cnt", True))
            scontext.Append(CSVCell("Paging Data Cnt", True))
            scontext.Append(CSVCell("Location Cnt", True))
            scontext.Append(CSVCell("Location Data Cnt", True))
            scontext.Append(CSVCell("TT Sync Error", True))
            scontext.Append(CSVCell("Error Cnt", True))

            scontext.Append(CSVNewLine())

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        Private Function MakeCSVForDefectiveSiteReport(ByVal dt As DataTable) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim Sheetname As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Sheetname = "Defective-Device-List "
            sFileName = Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            scontext.Append(CSVCell("Report: Defective Device List (All Sites Summary)", True))
            scontext.Append(CSVNewLine())
            scontext.Append(CSVNewLine())

            If (dt.Rows.Count > 0) Then

                scontext.Append(CSVCell("Site Name", True))
                scontext.Append(CSVCell("Total Number of Devices", True))

                scontext.Append(CSVNewLine())

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteName")).Replace(",", " "), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("NoOfDevice")), True))
                    End With
                    scontext.Append(CSVNewLine())
                Next
            Else
                scontext.Append(CSVCell("No Record Found!", True))
            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function

        '******************************************************************************************************'
        ' Function Name : PrepareCSVLoadDisasterRecovery                                                                           '
        ' Input         : DataTable                                           ' 
        ' Output        : CSV File                                                                             '
        ' Description   : Prepare CSV file based on the Datatable , bsed on bin value csv filoe name created   '
        '******************************************************************************************************'
        ''' <summary> Function to Genrate CSV File </summary>
        Private Sub PrepareCSVLoadDisasterRecovery(ByVal dt As DataTable)

            Dim excl As New Excelcreate
            Dim Csvtext As StringBuilder = New StringBuilder

            Dim sFileName As String
            Dim sScript As String = ""
            Dim Sheetname As String = "Centrak"

            Dim SiteId As Integer = 0

            Try

                Sheetname = "DisasterRecovery"

                sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                Dim context As HttpContext
                context = HttpContext.Current

                excl.InitiateCSV(context, sFileName)

                SiteId = CheckIsDBNull(dt.Rows(0).Item("SiteId"))

                excl.AddCSVCell(context, "Site Name : " & CheckIsDBNull(dt.Rows(0).Item("Sitename")).Replace(",", ""), True)
                excl.AddCSVNewLine(context)

                excl.AddCSVNewLine(context)

                excl.AddCSVCell(context, "MacId", True)
                excl.AddCSVCell(context, "IPAddress", True)
                excl.AddCSVNewLine(context)

                If dt.Rows.Count > 0 Then
                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("MacId")), True)
                            excl.AddCSVCell(context, CheckIsDBNull(.Item("IPAddress")), True)
                            excl.AddCSVNewLine(context)
                        End With
                    Next
                Else
                    excl.AddCSVCell(context, "No record found", True)
                    excl.AddCSVNewLine(context)
                    context.Response.End()
                End If

                context.Response.End()

            Catch ex As System.Threading.ThreadAbortException
            Catch ex As Exception
                WriteLog(" Exception PrepareExcel file " & ex.Message.ToString())
            End Try
	    
        End Sub

        '******************************************************************************************************************************************************'
        ' Function Name : MakeDisasterRecoveryCSV                                                                                                                              '
        ' Input         : Datatable                                                                                                    ' 
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file for disaster recovery                                                                             '
        '******************************************************************************************************************************************************'
        Private Function MakeDisasterRecoveryCSV(ByVal dt As DataTable) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow
            Dim isTagdataFound As Boolean = True
            Dim SiteId As Integer = 0

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder
            Dim Version As String = ""

            Sheetname = "DisasterRecovery"

            sFileName = "CenTrak-GMS-" & Sheetname & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            scontext.Append(CSVCell("MacId", True))
            scontext.Append(CSVCell("IPAddress", True))
            scontext.Append(CSVNewLine())

            If dt.Rows.Count > 0 Then

                SiteId = CheckIsDBNull(dt.Rows(0).Item("SiteId"))

                For nIdx As Integer = 0 To dt.Rows.Count - 1
                    With (dt.Rows(nIdx))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("MacId")), True))
                        scontext.Append(CSVCell(CheckIsDBNull(.Item("IPAddress")), True))
                        scontext.Append(CSVNewLine())
                    End With
                Next

            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
	    
        End Function

        Private Function MakeUserListReport(ByVal ds As DataSet) As DataTable

            Dim dtSite As New DataTable
            Dim dt As New DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim SiteName As String = ""
            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""

            Dim scontext As New StringBuilder

            Try

                If ds.Tables.Count > 0 Then

                    dtSite = ds.Tables(0)

                    If Not dtSite Is Nothing Then
                        If dtSite.Rows.Count > 0 Then
                            SiteName = dtSite.Rows(0).Item("SiteName")
                        End If
                    End If

                    dt = ds.Tables(1)

                End If

                If SiteName <> "" Then
                    sFileName = "Pulse-User-Account-" & Replace(SiteName, " ", "") & "-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                Else
                    sFileName = "Pulse-User-Account-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                End If

                If dt.Rows.Count > 0 Then

                    scontext.Append(CSVCell("Username", True))
                    scontext.Append(CSVCell("First Name", True))
                    scontext.Append(CSVCell("Last Name", True))
                    scontext.Append(CSVCell("Email", True))
                    scontext.Append(CSVCell("User Role", True))
                    scontext.Append(CSVCell("User Type", True))
                    scontext.Append(CSVCell("Company Name", True))
                    scontext.Append(CSVCell("Asset Viewer Access", True))
                    scontext.Append(CSVCell("Prism Access", True))
                    scontext.Append(CSVCell("CenTrak Staff", True))
                    scontext.Append(CSVCell("Status ", True))
                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            If CheckIsDBNull(.Item("Email")).ToLower() Like "*test@test.com" Or CheckIsDBNull(.Item("Email")).ToLower() Like "*swteam@centrak.com" Then
                                Continue For
                            End If

                            scontext.Append(CSVCell(.Item("UserName").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("FirstName").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("LastName").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("Email"), True))
                            scontext.Append(CSVCell(.Item("UserRole"), True))
                            scontext.Append(CSVCell(.Item("UserType"), True))
                            scontext.Append(CSVCell(.Item("CompanyName"), True))

                            If .Item("IsAssetView") = 1 Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("IsPrismView") = 1 Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("Email").ToLower() Like "*@centrak.com*" Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("Status") = "True" Then
                                scontext.Append(CSVCell("Active", True))
                            Else
                                scontext.Append(CSVCell("Inactive", True))
                            End If

                        End With

                        scontext.Append(CSVNewLine())
                    Next
                Else
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeUserListReport file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

#Region "For BatteryReplacementFailureReport"

        '******************************************************************************************************************************************************'
        ' Function Name : MakeCSVforBatteryReplacementFailureReport                                                                                            '
        ' Input         : Datatable , SiteId
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file based on Site Id                                                                                                     '
        '******************************************************************************************************************************************************'
        Private Function MakeCSVforBatteryReplacementFailureReport(ByVal dt As DataTable, ByVal SiteId As Integer) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            sFileName = "Pulse-BatteryReplacementFailureReport-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            Try

                If dt.Rows.Count > 0 Then

                    scontext.Append(CSVCell("Report Type: Battery Replacement Failure Report"))
                    scontext.Append(CSVNewLine())

                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("Device ID", True))
                    scontext.Append(CSVCell("Type", True))
                    scontext.Append(CSVCell("Device Type", True))
                    scontext.Append(CSVCell("Model Number", True))
                    scontext.Append(CSVCell("Failure Reason", True))
                    scontext.Append(CSVCell("Date of Failure", True))
                    scontext.Append(CSVCell("Replacement Indicated By", True))
                    scontext.Append(CSVCell("Replacement Indication Date", True))
                    scontext.Append(CSVCell("Current Battery Status", True))
                    scontext.Append(CSVCell("Current Reporting Status", True))
                    scontext.Append(CSVCell("Battery Replaced On", True))

                    If g_UserId = 6416 Then
                        scontext.Append(CSVCell("Last known daily median ADC", True))
                        scontext.Append(CSVCell("Last known daily median LBI Diff", True))
                    End If

                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            scontext.Append(CSVCell(.Item("Sitename").Replace(",", ""), True))
                            scontext.Append(CSVCell(.Item("DeviceId"), True))
                            scontext.Append(CSVCell(.Item("DeviceType"), True))
                            scontext.Append(CSVCell(.Item("DeviceSubType"), True))
                            scontext.Append(CSVCell(.Item("ModelItem"), True))
                            scontext.Append(CSVCell(.Item("FailureReason"), True))
                            scontext.Append(CSVCell(.Item("DateOfFailure"), True))
                            scontext.Append(CSVCell(.Item("BatteryReplacedby"), True))
                            scontext.Append(CSVCell(.Item("Updatedon"), True))
                            scontext.Append(CSVCell(.Item("Bin"), True))
                            scontext.Append(CSVCell(.Item("Status"), True))
                            scontext.Append(CSVCell(.Item("BatteryReplacementOn"), True))

                            If g_UserId = 6416 Then
                                scontext.Append(CSVCell(.Item("MedianLBIValue"), True))
                                scontext.Append(CSVCell(.Item("MedianLBIDiff"), True))
                            End If

                            scontext.Append(CSVNewLine())

                        End With
                    Next
                Else
                    scontext.Append(CSVCell("Report Type : Battery Replacement Failure Report"))
                    scontext.Append(CSVNewLine())
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforBatteryReplacementFailureReport file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
        End Function
#End Region

        Private Function MakeCSVforSiteList(ByVal dt As DataTable) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""

            Dim scontext As New StringBuilder

            sFileName = "SiteList-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

            Try

                If dt.Rows.Count > 0 Then

                    scontext.Append(CSVCell("Site Id", True))
                    scontext.Append(CSVCell("Site Name", True))
                    scontext.Append(CSVCell("File Format", True))
                    scontext.Append(CSVCell("Core Version", True))
                    scontext.Append(CSVCell("Company Name", True))
                    scontext.Append(CSVCell("Site Folder", True))
                    scontext.Append(CSVCell("Time Zone", True))
                    scontext.Append(CSVCell("Status", True))
                    scontext.Append(CSVNewLine())

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With dt.Rows(nIdx)

                            scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteId")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("Sitename")).Replace(",", ""), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("FileFormat")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("PCVersion")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("CompanyName")).Replace(",", ""), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("SiteFolder")).Replace(",", ""), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("TimeZone")).Replace(",", ""), True))

                            If .Item("Status") = "True" Then
                                scontext.Append(CSVCell("Active", True))
                            Else
                                scontext.Append(CSVCell("Inactive", True))
                            End If

                        End With

                        scontext.Append(CSVNewLine())
                    Next
                Else
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforSiteList file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
	    
        End Function
	
        Private Function MakeCSVforCentrakVoltDetailReport(ByVal sid As Integer, ByVal ds As DataSet) As DataTable

            Dim dtExcel As New DataTable
            Dim dtSort As New DataTable
            Dim dtSiteName As New DataTable
            Dim dt As New DataTable

            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Dim BatteryStatus As String = ""
            Dim Sitename As String = ""

            If Not ds Is Nothing Then

                dtSiteName = ds.Tables(0)
                If dtSiteName.Rows.Count > 0 Then
                    Sitename = dtSiteName.Rows(0).Item("Sitename")
                End If

                If ds.Tables.Count > 1 Then
                    dt = ds.Tables(1)
                End If

            End If

            sFileName = "ConnectPulseMobileAuditReport-" & Sitename.Replace(" ", "_") & "-" & DateTime.UtcNow.ToString("yyyyMMddHHmmss")

            Try

                scontext.Append(CSVCell("Device ID", True))
                scontext.Append(CSVCell("Device Type", True))
                scontext.Append(CSVCell("Device SubType", True))
                scontext.Append(CSVCell("Device Location", True))
                scontext.Append(CSVCell("Device Name", True))
                scontext.Append(CSVCell("Site Name", True))
                scontext.Append(CSVCell("User ID", True))
                scontext.Append(CSVCell("Date/Time", True))
                scontext.Append(CSVCell("Last Seen by Network Time", True))
                scontext.Append(CSVCell("Battery Bin", True))
                scontext.Append(CSVCell("Offline/Online Status", True))
                scontext.Append(CSVCell("Camera scanned?", True))
                scontext.Append(CSVCell("Device Page Opened?", True))
                scontext.Append(CSVCell("Access From", True))
                scontext.Append(CSVCell("Confirmed Battery Replacement?", True))
                scontext.Append(CSVCell("Confirmed Battery Replacement Date", True))
                scontext.Append(CSVCell("Offline Status Rechecked?", True))
                scontext.Append(CSVCell("Offline Resolved to Online?", True))
                scontext.Append(CSVCell("Device Not Found for Site?", True))
                scontext.Append(CSVCell("Device not Recognized?", True))
                scontext.Append(CSVCell("Device Battery Replacement Under Review?", True))
                scontext.Append(CSVCell("Device Swapped With", True))
                scontext.Append(CSVCell("Notes", True))
                scontext.Append(CSVNewLine())

                If dt.Rows.Count > 0 Then

                    'dt.DefaultView.RowFilter = "DeviceType<>0"
                    'dt = dt.DefaultView.ToTable()

                    dt.Columns.Add(New DataColumn("DeviceAccessOn_New", System.Type.[GetType]("System.DateTime")))

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            dt.Rows(nIdx).Item("DeviceAccessOn_New") = .Item("DeviceAccessOn")
                        End With
                    Next

                    dt.DefaultView.Sort = "DeviceAccessOn_New desc"
                    dt = dt.DefaultView.ToTable()

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))

                            scontext.Append(CSVCell(.Item("DeviceId"), True))

                            If .Item("DeviceType") = enumDeviceType.Tag Then

                                scontext.Append(CSVCell("Tag", True))

                                If .Item("bin") = 1 Then
                                    BatteryStatus = "30 Days"
                                ElseIf .Item("bin") = 4 Then
                                    BatteryStatus = "90 Days"
                                Else
                                    BatteryStatus = "Good"
                                End If

                            ElseIf .Item("DeviceType") = enumDeviceType.Monitor Then

                                scontext.Append(CSVCell("Monitor", True))

                                If .Item("bin") = 1 Then
                                    BatteryStatus = "30 Days"
                                ElseIf .Item("bin") = 4 Then
                                    BatteryStatus = "90 Days"
                                Else
                                    BatteryStatus = "Good"
                                End If

                            Else

                                scontext.Append(CSVCell("Unknown", True))
                                BatteryStatus = "Unknown"

                            End If
                            If .Item("DeviceSubType") <> "" Then
                                scontext.Append(CSVCell(.Item("DeviceSubType").Replace(",", ""), True))
                            Else
                                scontext.Append(CSVCell("Unknown", True))
                            End If

                            If .Item("DeviceLocation") <> "" Then
                                scontext.Append(CSVCell(.Item("DeviceLocation").Replace(",", ""), True))
                            Else
                                scontext.Append(CSVCell("Unknown", True))
                            End If

                            If .Item("DeviceName") <> "" Then
                                scontext.Append(CSVCell(.Item("DeviceName").Replace(",", ""), True))
                            Else
                                scontext.Append(CSVCell("Unknown", True))
                            End If

                            scontext.Append(CSVCell(.Item("SiteName").Replace(",", ""), True))

                            If .Item("Username") Like "*mathan*" Or .Item("Username") Like "*senthilnathan*" Then
                                scontext.Append(CSVCell("SWTeam", True))
                            Else
                                scontext.Append(CSVCell(.Item("Username").Replace(",", ""), True))
                            End If

                            scontext.Append(CSVCell(.Item("DeviceAccessOn"), True))
                            scontext.Append(CSVCell(.Item("LastSeen"), True))
                            scontext.Append(CSVCell(BatteryStatus, True))

                            If .Item("IsOffline") = True Then
                                scontext.Append(CSVCell("Offline", True))
                            ElseIf .Item("IsOffline") = False And .Item("bin") <> 100 Then
                                scontext.Append(CSVCell("Online", True))
                            Else
                                scontext.Append(CSVCell("Unknown", True))
                            End If

                            If .Item("IsCameraScan") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("IsDevicePageOpened") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("AccessFrom") = 1 Then
                                scontext.Append(CSVCell("list view", True))
                            ElseIf .Item("AccessFrom") = 2 Then
                                scontext.Append(CSVCell("camera", True))
                            ElseIf .Item("AccessFrom") = 3 Then
                                scontext.Append(CSVCell("barcode search", True))
                            End If

                            If .Item("IsConfirmedBatteryReplacement") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            scontext.Append(CSVCell(.Item("BatteryReplacementOn"), True))

                            If .Item("IsOfflineRechecked") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("IsOfflineResolved") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("IsDeviceNotFound") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("IsDeviceNotRecognized") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("IsUnderReview") Then
                                scontext.Append(CSVCell("Yes", True))
                            Else
                                scontext.Append(CSVCell("No", True))
                            End If

                            If .Item("SwapDeviceId") = "0" Then
                                scontext.Append(CSVCell("", True))
                            Else
                                scontext.Append(CSVCell(.Item("SwapDeviceId"), True))
                            End If

                            scontext.Append(CSVCell(.Item("Notes").Replace(",", ""), True))

                        End With

                        scontext.Append(CSVNewLine())
                    Next
                Else
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforCentrakVoltDetailReport file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        '******************************************************************************************************************************************************'
        ' Function Name : MakeCSVforMonitorGroupDetail                                                                                                                              '
        ' Input         : Datatable
        ' Output        : CSV File                                                                                                                             '
        ' Description   : Making csv file                                                                                                                      '
        '******************************************************************************************************************************************************'
        Private Function MakeCSVforMonitorGroupDetail(ByVal dt As DataTable) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Try

                sFileName = "Monitor Group Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                scontext.Append(CSVCell("Parent Monitor ID", True))
                scontext.Append(CSVCell("Child Monitor ID", True))
                scontext.Append(CSVNewLine())

                If dt.Rows.Count > 0 Then

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With (dt.Rows(nIdx))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorId")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("SlaveMonitorID")).Replace(",", ""), True))
                        End With

                        scontext.Append(CSVNewLine())
                    Next
                Else
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())
                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforMonitorGroupDetail file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        Private Function MakeCSVforCentrakEMActivityReport(ByVal ReportType As Integer, ByVal dt As DataTable, ByVal isDailyData As String, ByVal tTagId As String, ByVal ProbeType As String, ByVal SubType As String) As DataTable

            Dim dtExcel As New DataTable
            Dim drExcel As DataRow

            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Try

                If ReportType = enumEMTempReport.Inactivity Then

                    sFileName = "CenTrak EM In Activity Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                    scontext.Append(CSVCell("EM Tag Inactivity Report", True))

                ElseIf ReportType = enumEMTempReport.Connectivity Then

                    sFileName = "CenTrak EM Connectivity Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                    scontext.Append(CSVCell("EM Tag Connectivity Report - Missing > 5%", True))

                ElseIf ReportType = enumEMTempReport.PCE Then

                    If SubType = "1" Or SubType = "2" Then

                        sFileName = "CenTrak EM Probe Connect Errors Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                        scontext.Append(CSVCell("EM Probe Connect Errors Report", True))

                    ElseIf SubType = "3" Then

                        sFileName = "CenTrak EM Temp Spike Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                        scontext.Append(CSVCell("EM Temp Spike Report", True))

                    ElseIf SubType = "" Then

                        sFileName = "CenTrak EM Probe Connect Errors and Temp Spike Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                        scontext.Append(CSVCell("EM Probe Connect Errors and Temp Spike Report", True))

                    End If

                ElseIf ReportType = enumEMTempReport.ExcessivePaging Then

                    sFileName = "CenTrak Excessive Paging Report-" & Format(Now, "MMddyyyy") & Format(Now, "hms")
                    scontext.Append(CSVCell("Excessive Paging Report", True))

                End If

                scontext.Append(CSVNewLine())
                scontext.Append(CSVNewLine())

                If isDailyData = "1" Then

                    scontext.Append(CSVCell("Date", True))

                    If SubType = "3" Then
                        If ProbeType = 1 Then
                            scontext.Append(CSVCell("Probe1 Temp Spike Count", True))
                        Else
                            scontext.Append(CSVCell("Probe2 Temp Spike Count", True))
                        End If
                    Else
                        If ProbeType = 1 Then
                            scontext.Append(CSVCell("Probe1 Connect Errors Count", True))
                        Else
                            scontext.Append(CSVCell("Probe2 Connect Errors Count", True))
                        End If
                    End If

                Else

                scontext.Append(CSVCell("Site Name", True))
                scontext.Append(CSVCell("Tag Id", True))
                scontext.Append(CSVCell("Location", True))
                scontext.Append(CSVCell("Tag Type", True))
                scontext.Append(CSVCell("Version", True))
                scontext.Append(CSVCell("Model Item", True))
                scontext.Append(CSVCell("Report Rate", True))
                scontext.Append(CSVCell("Measurement Rate", True))
                scontext.Append(CSVCell("Mode Of Transport", True))
                scontext.Append(CSVCell("Probes", True))
                scontext.Append(CSVCell("Is Defined", True))
                scontext.Append(CSVCell("Bin Status", True))
                scontext.Append(CSVCell("First Seen", True))
                scontext.Append(CSVCell("Last Seen", True))

                    If ReportType = enumEMTempReport.Inactivity Then

                        scontext.Append(CSVCell("Last Activity Ago", True))

                    ElseIf ReportType = enumEMTempReport.Connectivity Then

                        scontext.Append(CSVCell("Expected Reporting Count", True))
                        scontext.Append(CSVCell("Reported Count", True))
                        scontext.Append(CSVCell("Missing Connectivity by %", True))

                    ElseIf ReportType = enumEMTempReport.PCE Then

                        If SubType = "1" Or SubType = "2" Or SubType = "" Then

                            scontext.Append(CSVCell("Probe1 Connect Errors Count (past day)", True))
                            scontext.Append(CSVCell("Probe2 Connect Errors Count (past day)", True))
                            scontext.Append(CSVCell("Probe1 Connect Errors Count (30 days)", True))
                            scontext.Append(CSVCell("Probe2 Connect Errors Count (30 days)", True))

                        End If

                        If SubType = "3" Or SubType = "" Then

                            scontext.Append(CSVCell("Probe1 Temp Spike Count (past day)", True))
                            scontext.Append(CSVCell("Probe2 Temp Spike Count (past day)", True))
                            scontext.Append(CSVCell("Probe1 Temp Spike Count (30 days)", True))
                            scontext.Append(CSVCell("Probe2 Temp Spike Count (30 days)", True))

                        End If

                    ElseIf ReportType = enumEMTempReport.ExcessivePaging Then

                        scontext.Append(CSVCell("Excessive Paging Count", True))
                    End If

                End If

                scontext.Append(CSVNewLine())

                If dt.Rows.Count > 0 Then

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With dt.Rows(nIdx)

                            If isDailyData = "1" Then

                                scontext.Append(CSVCell(.Item("DailyDate"), True))

                                If SubType = "3" Then
                                    If ProbeType = 1 Then
                                        scontext.Append(CSVCell(.Item("Probe1TempSpikeCount"), True))
                                    Else
                                        scontext.Append(CSVCell(.Item("Probe2TempSpikeCount"), True))
                                    End If
                                Else
                                    If ProbeType = 1 Then
                                        scontext.Append(CSVCell(.Item("Probe1PCECount"), True))
                                    Else
                                        scontext.Append(CSVCell(.Item("Probe2PCECount"), True))
                                    End If
                                End If

                            Else

                                scontext.Append(CSVCell(.Item("SiteName").Replace(",", ""), True))
                                scontext.Append(CSVCell(.Item("TagId"), True))
                                scontext.Append(CSVCell(.Item("Location").Replace(",", ""), True))
                                scontext.Append(CSVCell(.Item("TagTypeDesc").Replace(",", ""), True))
                                scontext.Append(CSVCell(.Item("Version"), True))
                                scontext.Append(CSVCell(.Item("ModelItem"), True))
                                scontext.Append(CSVCell(.Item("ReportRate"), True))
                                scontext.Append(CSVCell(.Item("MeasurementRate"), True))
                                scontext.Append(CSVCell(.Item("ModeOfTransport"), True))
                                scontext.Append(CSVCell(.Item("Probes"), True))
                                scontext.Append(CSVCell(.Item("IsDefined"), True))
                                scontext.Append(CSVCell(.Item("BinStatus"), True))
                                scontext.Append(CSVCell(.Item("FirstSeen"), True))
                                scontext.Append(CSVCell(.Item("LastSeen"), True))

                                If ReportType = enumEMTempReport.Inactivity Then

                                    scontext.Append(CSVCell(.Item("LastActivityAgo"), True))

                                ElseIf ReportType = enumEMTempReport.Connectivity Then

                                    scontext.Append(CSVCell(.Item("ExpectedCount"), True))
                                    scontext.Append(CSVCell(.Item("ReportedCount"), True))
                                    scontext.Append(CSVCell(.Item("MissingCountPercen"), True))

                                ElseIf ReportType = enumEMTempReport.PCE Then

                                    If SubType = "1" Or SubType = "2" Or SubType = "" Then

                                        scontext.Append(CSVCell(.Item("Probe1PCECount"), True))
                                        scontext.Append(CSVCell(.Item("Probe2PCECount"), True))
                                        scontext.Append(CSVCell(.Item("Probe1PCECount30Day"), True))
                                        scontext.Append(CSVCell(.Item("Probe2PCECount30Day"), True))

                                    End If

                                    If SubType = "3" Or SubType = "" Then

                                        scontext.Append(CSVCell(.Item("Probe1TempSpikeCount"), True))
                                        scontext.Append(CSVCell(.Item("Probe2TempSpikeCount"), True))
                                        scontext.Append(CSVCell(.Item("Probe1TempSpikeCount30Day"), True))
                                        scontext.Append(CSVCell(.Item("Probe2TempSpikeCount30Day"), True))

                                    End If

                                ElseIf ReportType = enumEMTempReport.ExcessivePaging Then

                                    scontext.Append(CSVCell(.Item("ExcessivePagingCount"), True))
                                End If

                            End If

                        End With

                        scontext.Append(CSVNewLine())

                    Next

                Else
                    scontext.Append(CSVCell("No Record Found"))
                    scontext.Append(CSVNewLine())

                End If

            Catch ex As Exception
                WriteLog(" Exception MakeCSVforCentrakEMActivityReport file " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function

        Private Function MakeCSVForLocationChangeEvent(ByVal XmlElement As XmlElement, ByVal lFromDate As String, ByVal lToDate As String,
                                                       ByVal lType As String, ByVal lDeviceId As String, ByVal inMonitorIds As String, ByVal exMonitorIds As String, ByVal TimeSpend As String) As DataTable

            Dim dtExcel As New DataTable
            Dim dt As New DataTable
            Dim dtSite As New DataTable
            Dim drExcel As DataRow
            Dim dSet As New DataSet

            Dim SheetName As String = "Centrak"
            Dim SiteName As String = ""
            Dim sFileName As String = ""
            Dim scontext As New StringBuilder

            Dim lTypeDesc As String

            SheetName = "LocationChangeEvent"

            sFileName = "CenTrak-GMS-" & SheetName & "-" & Format(Now, "MMddyyyy") & Format(Now, "HHmmss")

            If XmlElement IsNot Nothing Then
                Dim xmlReader As New XmlTextReader(XmlElement.OuterXml, XmlNodeType.Element, Nothing)
                dSet.ReadXml(xmlReader)
            End If

            If Not dSet Is Nothing Then
                If dSet.Tables.Count > 0 Then
                    dtSite = dSet.Tables(0)

                    If dSet.Tables.Count > 1 Then
                        dt = dSet.Tables(1)
                    End If

                End If
            End If

            If dtSite.Rows.Count > 0 Then

                SiteName = CheckIsDBNull(dtSite.Rows(0).Item("SiteName").replace(",", " "), , "")

                scontext.Append(CSVCell("Site Name: " & SiteName))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Date Range: " & CDate(lFromDate).Date & " - " & CDate(lToDate).Date))
                scontext.Append(CSVNewLine())

                lTypeDesc = "All Tags"
                Select Case lType
                    Case 1
                        lTypeDesc = "All Asset Tags"
                    Case 2
                        lTypeDesc = "All Patient Tags"
                    Case 3
                        lTypeDesc = "All Staff Tags"
                End Select

                scontext.Append(CSVCell("Tag Type: " & lTypeDesc))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Tags: " & Replace(lDeviceId, ",", ";")))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Monitors to be included: " & Replace(inMonitorIds, ",", ";")))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Monitors to be excluded: " & Replace(exMonitorIds, ",", ";")))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Time Spend in seconds: " & TimeSpend))
                scontext.Append(CSVNewLine())
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Tag ID", True))
                scontext.Append(CSVCell("Room ID", True))
                scontext.Append(CSVCell("Monitor ID", True))
                scontext.Append(CSVCell("Room Description (Monitor Description)", True))
                scontext.Append(CSVCell("Entered On (Date & Time)", True))
                scontext.Append(CSVCell("Exit On (Date & Time)", True))
                scontext.Append(CSVCell("Time Spend in seconds", True))
                scontext.Append(CSVCell("New Monitor ID", True))
                scontext.Append(CSVCell("New Room Description (Monitor Description)", True))
                scontext.Append(CSVCell("Duration Minutes", True))
                scontext.Append(CSVNewLine())

                If dt.Rows.Count > 0 Then

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With dt.Rows(nIdx)

                            scontext.Append(CSVCell(CheckIsDBNull(.Item("TagID")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("RoomID")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("MonitorID")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("RoomDescription")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("EnteredOn")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("ExitOn")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("TimeSpend")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("NewMonitorId")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("NewMonitorName")), True))
                            scontext.Append(CSVCell(CheckIsDBNull(.Item("DurationMinutes")), True))
                            scontext.Append(CSVNewLine())

                        End With
                    Next

                Else
                    scontext.Append(CSVCell("No Data Found!", True))
                    scontext.Append(CSVNewLine())
                End If

            End If

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "-")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel
	    
        End Function

        Private Function MakeHistoricalTemperatureReport(ByVal dt As DataTable, ByVal SiteId As String, ByVal Sitename As String, ByVal sDeviceId As String,
                                                         ByVal sFromDate As String, ByVal sTodate As String) As DataTable

            Dim dtExcel As New DataTable
            Dim dtSite As New DataTable

            Dim drExcel As DataRow

            Dim Sheetname As String = "Centrak"
            Dim sFileName As String = ""
            Dim sSitename As String = ""
            Dim TimeZone As String = ""
            Dim scontext As New StringBuilder

            Try

                sSitename = GetValidSiteName(Sitename)

                sFileName = sSitename & "_Historical Temperature Report" & Format(Now, "MMddyyyy") & Format(Now, "hms")

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        TimeZone = dt.Rows(0).Item("TimeZone")
                    End If
                End If

                scontext.Append(CSVCell("Site Name: " & sSitename))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("TagId: " & sDeviceId))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Date Range: " & sFromDate & " - " & sTodate))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("TimeZone: " & TimeZone))
                scontext.Append(CSVNewLine())

                scontext.Append(CSVCell("Probe Type", True))
                scontext.Append(CSVCell("Time", True))
                scontext.Append(CSVCell("Temperature", True))
                scontext.Append(CSVCell("GMT Time", True))
                scontext.Append(CSVCell("Status", True))
                scontext.Append(CSVCell("Measurement Rate", True))
                scontext.Append(CSVNewLine())

                If dt.Rows.Count > 0 Then

                    For nIdx As Integer = 0 To dt.Rows.Count - 1
                        With dt.Rows(nIdx)
                            scontext.Append(CSVCell(.Item("ProbType"), True))
                            scontext.Append(CSVCell(.Item("Time"), True))
                            scontext.Append(CSVCell(.Item("Temp"), True))
                            scontext.Append(CSVCell(.Item("GMTTime"), True))
                            scontext.Append(CSVCell(.Item("Status"), True))
                            scontext.Append(CSVCell(.Item("MeasurementRate"), True))
                            scontext.Append(CSVNewLine())
                        End With
                    Next
                Else
                    scontext.Append(CSVCell("No Data Found!", True))
                    scontext.Append(CSVNewLine())
                End If


            Catch ex As Exception
                WriteLog(" Exception MakeHistoricalTemperatureReport " & ex.Message.ToString())
            End Try

            'Add Datas to dtExcel
            dtExcel.Columns.Add(New DataColumn("Excel", System.Type.[GetType]("System.String")))
            dtExcel.Columns.Add(New DataColumn("Filename", System.Type.[GetType]("System.String")))

            drExcel = dtExcel.NewRow()
            drExcel("Excel") = scontext.ToString
            drExcel("Filename") = sFileName.Replace(" ", "_")
            dtExcel.Rows.Add(drExcel)

            Return dtExcel

        End Function
    End Class
End Namespace

