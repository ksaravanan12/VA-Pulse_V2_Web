Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Xml
Imports System.Net

Namespace GMSUI

    Public Class GmsAPICall

        Dim api As New GMSAPI_New.GMSAPI_New
        Dim GMSAPI As New gmsapi.GMSAPI

        Function getSiteSummary(ByVal apikey As String, ByVal sid As String, ByVal VACompanys As String, ByVal VASites As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.SiteSummary(apikey, sid, VACompanys, VASites)

            Return sStrXml

        End Function
     
        Function getSiteListInfo(ByVal VACompanys As String, ByVal VASites As String, ByVal apikey As String, ByVal sid As String) As XmlNode

            Dim sStrXml As XmlNode
            sStrXml = api.GetSiteListInfo(sid, VACompanys, VASites, apikey)
            Return sStrXml

        End Function

        Function SiteDetailedOverview(ByVal apikey As String, ByVal sid As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.GetSiteDetailedOverviewNew(apikey, sid)

            Catch ex As Exception

            End Try

            Return sStrXml

        End Function
       
        Function SiteAlertList(ByVal apikey As String, ByVal sid As String, ByVal VACompanys As String, ByVal VASites As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAlertList(apikey, sid, VACompanys, VASites)

            Return sStrXml

        End Function
      
        Function DeviceListMonitor(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal DeviceId As String, ByVal CurPage As String, ByVal AlertId As String,
                                   ByVal apikey As String, ByVal bin As String, Optional ByVal sortingClName As String = "", Optional ByVal sorOrder As String = "") As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.DeviceList(SiteId, DeviceType, TypeId, DeviceId, CurPage, AlertId, apikey, bin, sortingClName, sorOrder)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
  
        Function Device_AddDevice_Image(ByVal DataId As String, ByVal DeviceId As String, ByVal SiteId As String, ByVal DeviceInfo As String, ByVal Image As Byte(), ByVal ImageName As String,
                                        ByVal DeviceType As String, ByVal EditMode As String, ByVal IsActive As String) As XmlNode
            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.AddDeviceImage(DataId, DeviceId, SiteId, DeviceInfo, Image, ImageName, DeviceType, EditMode, IsActive, g_UserAPI)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function Campus_Add_Image(ByVal SiteId As String, ByVal Image As Byte(), ByVal FileName As String, ByVal UserName As String, ByVal Description As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.AddCampusImage(SiteId, Image, FileName, UserName, Description, g_UserAPI) ', Path, UserName
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
      
        Function DevicePhotoList(ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceType As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.DevicePhotoList(SiteId, DeviceId, DeviceType, g_UserAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
      
        Function DeleteDevicePhotoList(ByVal DataId As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceType As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.DeleteDeviceImage(DataId, SiteId, DeviceId, DeviceType, g_UserAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
 
        Function DeviceList(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal DeviceId As String,
                            ByVal CurPage As String, ByVal AlertId As String, ByVal apikey As String, ByVal bin As String,
                            Optional ByVal sortingClName As String = "", Optional ByVal sorOrder As String = "") As XmlNode
            Dim sStrXml As XmlNode = Nothing

            Try
                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 

                api.Timeout = 60 * 10 * 1000 '10 minutes

                sStrXml = api.DeviceList(SiteId, DeviceType, TypeId, DeviceId, CurPage, AlertId, apikey, bin, sortingClName, sorOrder)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function EMTagsDeviceList(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.EmTagDeviceList(SiteId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml
	    
        End Function

        Function EMTagDetailReport(ByVal SiteId As String, ByVal EMReportType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.EMTagDetailReport(SiteId, EMReportType, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function getAllDevicesforSite(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 
                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.GetAllDevicesForSite(SiteId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
  
        Function PurchaseOrder(ByVal SiteId As String, ByVal CurrPage As String, ByVal sortPO As String, ByVal PONo As String, ByVal ModelItem As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetInventoryForSite(SiteId, PONo, apikey, CurrPage, "", sortPO, ModelItem)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
        
        Function PurchaseDetail(ByVal PONo As String, ByVal CurrPage As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetPODetailsForSite(PONo, apikey, CurrPage, "")
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
        
        Function PurchaseSummary(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetPOSummaryForSite(SiteId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
        
        Function MapView(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetMapMetaInfoForSite(SiteId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
        
        Function RoomView(ByVal SiteId As String, ByVal mapUnitId As String, ByVal SortColumn As String, ByVal SortOrder As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetRoomMapMetaInfoForSite(SiteId, "", "", "", "", mapUnitId, SortColumn, SortOrder, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GMSGlossaryinfo(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGlossaryInfo(apikey)

            Return sStrXml

        End Function
      
        Function GetAlertInfo(ByVal apikey As String, ByVal sid As String, ByVal VACompanys As String, ByVal VASites As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAlertInfo(sid, apikey, VACompanys, VASites)
            Return sStrXml

        End Function

        Function DeviceProfile(ByVal apikey As String, ByVal sid As String, ByVal devicetype As String, ByVal deviceid As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            'sStrXml = api.DeviceProfile(apikey, sid, devicetype, deviceid)

            Return sStrXml

        End Function

        Function DeviceActivity(ByVal apikey As String, ByVal sid As String, ByVal devicetype As String, ByVal deviceid As String, ByVal period As String, ByVal fromDate As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeviceActivity(sid, devicetype, deviceid, period, fromDate, apikey)

            Return sStrXml

        End Function

        Function Load10hrdata(ByVal apikey As String, ByVal sid As String, ByVal devicetype As String, ByVal deviceid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeviceHourlyInfo(sid, devicetype, deviceid, "10", apikey)

            Return sStrXml

        End Function

        Function getEmailList(ByVal sid As String, ByVal CurPage As String, ByVal VACompanys As String, ByVal VASites As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListEmailsForAlert(sid, CurPage, VACompanys, VASites, apikey)

            Return sStrXml

        End Function

        Function getTagMovementsList(ByVal SiteId As Integer, ByVal DeviceId As Integer, ByVal FromDate As String, ByVal ToDate As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.CheckTagMovements(SiteId, DeviceId, CDate(FromDate).ToString("yyyy-MM-dd HH:mm:ss"), CDate(ToDate).ToString("yyyy-MM-dd HH:mm:ss"), apikey)

            Return sStrXml

        End Function

        Function getHeartBeatList(ByVal SiteId As Integer, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetHeartbeatStatusForSite(SiteId, apikey)

            Return sStrXml

        End Function

        Function getAlertHistoryList(ByVal apikey As String, ByVal siteId As String, ByVal fromDate As String, ByVal ToDate As String, ByVal ServiceId As String) As XmlNode

            Dim sStrXml As XmlNode
	    
            sStrXml = api.AlertHistory(siteId, "", "", "", "", fromDate, ToDate, "", "", "", "1", ServiceId, apikey)

            Return sStrXml
	    
        End Function

        Function getLocalAlertForSite(ByVal apikey As String, ByVal siteId As String, ByVal fromDate As String, ByVal ToDate As String, ByVal deviceId As String, ByVal deviceType As String, ByVal serviceId As String) As XmlNode

            Dim sStrXml As XmlNode
            sStrXml = api.GetLocalAlertsForSite(siteId, fromDate, ToDate, deviceId, deviceType, serviceId, apikey)

            Return sStrXml

        End Function

        Function SetupEmail(ByVal sid As String, ByVal EmailId As String, ByVal AlertType As String, ByVal Status As String, ByVal apikey As String, ByVal IsAdd As String, ByVal EmailDataId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.SetupEmailForAlert(sid, EmailId, apikey, Status, AlertType, IsAdd, EmailDataId)

            Return sStrXml

        End Function

        Function AddEditEmailWithAlertsForSite(ByVal sid As String, ByVal EmailId As String, ByVal AlertType As String, ByVal Status As String, ByVal apikey As String, ByVal emailAlertList As String, ByVal EmailDataId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.AddEditEmailWithAlertsForSite(sid, EmailId, apikey, Status, AlertType, emailAlertList, EmailDataId)

            Return sStrXml

        End Function

        Function Search(ByVal DeviceType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetSearchFilters(DeviceType, apikey)

            Return sStrXml

        End Function

        Function Device_Search(ByVal siteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal CurPage As String, ByVal apikey As String, ByVal FilterCriteria As String, ByVal sCompanys As String, ByVal sSites As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeviceSearch(siteId, DeviceType, DeviceId, CurPage, apikey, FilterCriteria, sCompanys, sSites)

            Return sStrXml

        End Function

        Function DeviceRemoval(ByVal apikey As String, ByVal DeviceType As String, ByVal SiteId As String, ByVal sDeviceIds As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            'sStrXml = api.DeviceRemoval(DeviceType, SiteId, sDeviceIds, apikey)

            Return sStrXml

        End Function

        Function deleteEmailConfiguration(ByVal apikey As String, ByVal siteId As String, ByVal emailId As String, ByVal curPage As String, ByVal VACompanys As String, ByVal VASites As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeleteEmailConfiguration(siteId, emailId, curPage, VACompanys, VASites, apikey)

            Return sStrXml

        End Function

        Function GetSchuledReports(ByVal apikey As String, ByVal emailId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetScheduledReports(emailId, apikey)

            Return sStrXml

        End Function
	
        Function getAvailableAlertsForEmail(ByVal apikey As String, ByVal siteId As String, ByVal emailId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAvailableAlertsForEmail(siteId, emailId, apikey)

            Return sStrXml

        End Function

        Function ConfigureAlertsForSite(ByVal apikey As String, ByVal siteId As String, ByVal emailId As String,
                                        ByVal AlertId As String, ByVal isAdd As String, ByVal mode As String, ByVal alertType As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ConfigureAlertsForSite(siteId, emailId, AlertId, isAdd, mode, "", alertType, apikey)

            Return sStrXml

        End Function

        Function GetUserReportSchedule(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUserReportSchedule(SiteId, apikey)

            Return sStrXml

        End Function

        Function UpdateUserReportSchedule(ByVal SiteId As String, ByVal recurrence As String, ByVal scheduletime As String, ByVal weeklyinterval As String,
                                          ByVal monthlyinterval As String, ByVal yearlyinterval1 As String, ByVal yearlyinterval2 As String, ByVal apikey As String) As XmlNode
            Dim sStrXml As XmlNode

            sStrXml = api.UpdateUserReportSchedule(SiteId, recurrence, scheduletime, weeklyinterval, monthlyinterval, yearlyinterval1, yearlyinterval2, apikey)

            Return sStrXml

        End Function

        Function GetAlertSettings(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAlertSettingsForSite(SiteId, apikey)

            Return sStrXml

        End Function

        Function UpdateAlertSettings(ByVal SiteId As String, ByVal alertList As String, ByVal emailList As String,
                                     ByVal phnoList As String, ByVal alertType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ConfigureAlertsForSite(SiteId, emailList, alertList, "3", "", phnoList, alertType, apikey)

            Return sStrXml

        End Function

        Function LoadLocalAlertsForTableView(ByVal SiteId As String, ByVal CurrPage As String, ByVal SortAlerts As String, ByVal StartDate As String,
                                             ByVal EndDate As String, ByVal AlertIds As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode
            Dim sStrXml As XmlNode

            sStrXml = api.GetLocalAlertsForSiteForTextView(SiteId, StartDate, EndDate, DeviceId, "", "", AlertIds, SortAlerts, CurrPage, "", apikey)

            Return sStrXml

        End Function

        Function LocationChageEvent(ByVal SiteId As String, ByVal Tagids As String, ByVal CurrentRoom As String, ByVal LastRoom As String, ByVal PageSize As String,
                                    ByVal CurPage As String, ByVal IsLiveData As String, ByVal LastFetchedDateTime As String, ByVal NeedAll As String, ByVal Sorting As String,
                                    ByVal EnteredOnFromDate As String, ByVal EnteredOnToDate As String, ByVal LeftOnFromDate As String, ByVal LeftOnToDate As String, ByVal apikey As String) As XmlNode
            Dim sStrXml As XmlNode

            sStrXml = api.GetLocationChangeEvent(SiteId, Tagids, CurrentRoom, LastRoom, PageSize, CurPage, IsLiveData, LastFetchedDateTime, NeedAll, Sorting, EnteredOnFromDate, EnteredOnToDate, LeftOnFromDate, LeftOnToDate, apikey)

            Return sStrXml

        End Function

        Function AddCampus(ByVal SiteId As String, ByVal MapName As String, ByVal MapDescription As String, ByVal MapId As String, ByVal isDelete As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateMetaInfoForCampus(MapId, SiteId, MapName, MapDescription, isDelete, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function AddBuilding(ByVal SiteId As String, ByVal MapName As String, ByVal MapDescription As String, ByVal MapId As String, ByVal BuildingId As String, ByVal isDelete As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateMetaInfoForBuilding(BuildingId, SiteId, MapId, MapName, MapDescription, isDelete, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function AddFloor(ByVal SiteId As String, ByVal MapName As String, ByVal MapDescription As String, ByVal MapId As String, ByVal BuildingId As String, ByVal FloorId As String,
                          ByVal isDelete As String, ByVal floorSqft As String, ByVal Widthft As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateMetaInfoForFloor(FloorId, SiteId, MapId, BuildingId, MapName, MapDescription, isDelete, Widthft, floorSqft, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function AddUnit(ByVal SiteId As String, ByVal MapName As String, ByVal MapDescription As String, ByVal MapId As String, ByVal BuildingId As String, ByVal FloorId As String,
                         ByVal UnitId As String, ByVal isDelete As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateMetaInfoForUnit(UnitId, SiteId, MapId, BuildingId, FloorId, MapName, MapDescription, isDelete, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UploadFile(ByVal SiteId As String, ByVal file As Byte(), ByVal filename As String, ByVal source As String, ByVal sourceType As String, ByVal isEdit As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.UpdateMetaFileForSite(SiteId, file, filename, sourceType, source, isEdit, "", apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateMetaInfoForRooms(ByVal SiteId As String, ByVal CampusId As String, ByVal BuildingId As String, ByVal FloorId As String, ByVal UnitId As String, ByVal file As Byte(), ByVal filename As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.UpdateMetaInfoForRooms(SiteId, CampusId, BuildingId, FloorId, UnitId, file, filename, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateMetaInfoForDevices(ByVal SiteId As String, ByVal FloorId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateMetaInfoForInfrastructure(SiteId, FloorId, file, fileName, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateMetaInfoForTags(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateMetaInfoForTags(SiteId, file, fileName, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function TagView(ByVal SiteId As String, ByVal PageSize As String, ByVal CurrPage As String, ByVal TagMetaIds As String, ByVal Sorting As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetMetaInfoForTag(SiteId, TagMetaIds, PageSize, CurrPage, "", Sorting, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetInfrastructureMetaInfoForFloor(ByVal SiteId As String, ByVal FloorId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetInfrastructureMetaInfoForFloor(SiteId, FloorId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetTagMetaInfoForFloor(ByVal SiteId As String, ByVal FloorId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetTagMetaInfoForFloor(SiteId, FloorId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateAnnouncements(ByVal AnnouncementId As String, ByVal Message As String, ByVal StartDateTime As String, ByVal EndDateTime As String, ByVal IsShow As String,
                                     ByVal UserRoleToShow As String, ByVal UserAssociatedViews As String, ByVal HTMLMessage As String, ByVal IsActive As String, ByVal ShowIn As String,
                                     ByVal IsDispAfterExpire As String, ByVal daysDispAfterExpire As String, ByVal IsDispHistory As String, ByVal UseAPI As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateAnnoncements(AnnouncementId, Message, StartDateTime, EndDateTime, IsShow, UserRoleToShow, UserAssociatedViews, HTMLMessage, IsActive, ShowIn, IsDispAfterExpire, daysDispAfterExpire, IsDispHistory, UseAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetAnnouncements(ByVal StartDateTime As String, ByVal EndDateTime As String, ByVal IsLive As String, ByVal CurrPage As String, ByVal UserRole As String, ByVal UseAPI As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetAnnoncements(StartDateTime, EndDateTime, IsLive, CurrPage, "", UserRole, UseAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetHTMLAnnouncements(ByVal AnnouncementId As String, ByVal UseAPI As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetHTMLAnnoncements(AnnouncementId, UseAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetPastAnnouncements(ByVal UserRole As String, ByVal UseAPI As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetPastAnnoncements(UserRole, UseAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function AddThresHoldTime(ByVal TTime As String, ByVal UseAPI As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.AddThresHoldTime(TTime, UseAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function SearchDeviceMap(ByVal SiteId As String, ByVal SearchId As String, ByVal UserAPI As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.SearchDeviceForSiteForMap(SiteId, SearchId, UserAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateMetaInfoForMonitor(ByVal siteId As String, ByVal MetaInfoForFloorId As String, ByVal MapMonitorId As String, ByVal Location As String, ByVal Notes As String,
                                          ByVal isHallway As String, ByVal monitorX As String, ByVal monitorY As String, ByVal monitorW As String, ByVal monitorH As String,
                                          ByVal polygonPoints As String, ByVal uMode As String, ByVal dsvgId As String, ByVal svgDType As String, ByVal oldDeviceId As String,
                                          ByVal UnitName As String, ByVal Xaxis As String, ByVal Yaxis As String, ByVal RoomXaxis As String, ByVal RoomYaxis As String,
                                          ByVal WidthFt As String, ByVal LengthFt As String, ByVal MonitorType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.UpdateMetaInfoForMonitor(siteId, MetaInfoForFloorId, MapMonitorId, Location, Notes, isHallway, monitorX, monitorY, monitorW, monitorH,
                                                       polygonPoints, uMode, dsvgId, svgDType, oldDeviceId, UnitName, Xaxis, Yaxis, RoomXaxis, RoomYaxis, WidthFt, LengthFt,
                                                       "", "", MonitorType, "", "", "", "", "", "", apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateProfiles(ByVal OldPassword As String, ByVal NewPassWord As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateProfiles(OldPassword, NewPassWord, NewPassWord, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function ChangePasswordInfo(ByVal UserId As String, ByVal OldPassword As String, ByVal NewPassWord As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.ChangePassword(UserId, OldPassword, NewPassWord, NewPassWord, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function ForgotPassword(ByVal UserName As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.ForgotPassword(UserName)
            Catch ex As Exception
            End Try

            Return sStrXml

        End Function

        Function UserNameByResetKey(ByVal ResetKey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetUserNameByResetKey(ResetKey)
            Catch ex As Exception
            End Try

            Return sStrXml

        End Function

        Function UpdatePassword(ByVal ResetKey As String, ByVal Password As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdatePassword(ResetKey, Password)
            Catch ex As Exception
            End Try

            Return sStrXml

        End Function

        Function AddOrRemoveMasterList(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal IsDelete As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.AddOrRemoveMasterList(SiteId, DeviceType, DeviceId, IsDelete, apikey)

            Catch ex As Exception
            End Try

            Return sStrXml

        End Function

        Function GetReportsForMap(ByVal SiteId As String, ByVal Floorid As String, ByVal reportType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.GetReportsForMap(SiteId, Floorid, reportType, apikey)

            Catch ex As Exception
            End Try

            Return sStrXml

        End Function

        Function GetDeviceLBIReportForMap(ByVal SiteId As String, ByVal DeviceType As String, ByVal Bin As String, ByVal CampusId As String, ByVal BuildingId As String,
                                          ByVal FloorId As String, ByVal FilterCriteria As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetDeviceLBIReportForMap(SiteId, DeviceType, Bin, CampusId, BuildingId, FloorId, FilterCriteria, apikey)
            Catch ex As Exception
            End Try

            Return sStrXml

        End Function

        Function AssetDeviceList(ByVal SiteId As String, ByVal CampusId As String, ByVal BuildingId As String, ByVal FloorId As String, ByVal UnitId As String, ByVal sortingClName As String,
                                 ByVal sorOrder As String, ByVal CurPage As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 
                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.GetAssetsForSite_Offline(SiteId, CampusId, BuildingId, FloorId, UnitId, sorOrder, sortingClName, CurPage, DeviceId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function AssetReportList(ByVal SiteId As String, ByVal DeviceId As String, ByVal FromDate As String, ByVal ToDate As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 
                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.GetAssetReportsForSite(SiteId, DeviceId, FromDate, ToDate, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function BatterySummaryList(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal CampusId As String, ByVal BuildId As String, ByVal FloorId As String, ByVal UnitId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 
                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.GetBatterySummaryForSite(SiteId, DeviceType, TypeId, CampusId, BuildId, FloorId, UnitId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateBatteryReplace(ByVal SiteId As String, ByVal TagId As String, ByVal ReDate As String, ByVal cmts As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateBatteryReplace(SiteId, TagId, ReDate, cmts, g_UserAPI)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetBatteryList(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal CampusId As String, ByVal BuildId As String, ByVal FloorId As String,
                                ByVal UnitId As String, ByVal Curpage As String, ByVal SortColumn As String, ByVal SortOrder As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 
                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.GetBatteryListForSite(SiteId, DeviceType, TypeId, CampusId, BuildId, FloorId, UnitId, Curpage, SortColumn, SortOrder, DeviceId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetLbiListForBatteryTech(ByVal SiteId As String, ByVal SortColumn As String, ByVal SortOrder As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                api.Timeout = 60 * 10 * 1000 '10 minutes

                sStrXml = api.GetLBIListForBatteryTech(SiteId, SortColumn, SortOrder, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateLbiForBatteryTech(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.UpdateLbiInfoForBatteryTech(SiteId, file, fileName, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateFloorInfoBatteryTech(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                api.Timeout = 60 * 30 * 1000 '10 minutes
                sStrXml = api.UpdateMonitorLocationForSiteByBatteryTech(SiteId, file, fileName, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetTagReportForBatteryTech(ByVal SiteId As String, ByVal FromDate As String, ByVal ToDate As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.GetBatteryReportForSite(SiteId, FromDate, ToDate, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetIniHistoryInfo(ByVal SiteId As String, ByVal DeviceType As String, ByVal IDate As String, ByVal SortColumn As String, ByVal SortOrder As String, ByVal DeviceIds As String, ByVal CurPage As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.GetIniHistoryInfo(SiteId, DeviceType, IDate, SortColumn, SortOrder, DeviceIds, CurPage, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetIniHistoryProfileInfo(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.GetIniHistoryProfileInfo(SiteId, DeviceType, DeviceId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetSUPTTagDiedReportInfo(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.GetSUPTTagDiedReportInfo(SiteId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateAnnualCalibrationForSite(ByVal SiteId As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.UpdateAnnualCalibrationForSite(SiteId, DeviceId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

#Region "-------------------------------------ASSET TRACKING----------------------------------------"

        Function Asset_AddDevice(ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.Asset_SaveSearch(SiteId, DeviceId, DeviceName, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function Asset_GetSavedSearchesForUser(ByVal SiteId As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.Asset_GetSavedSearchesForUser(SiteId, DeviceId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        '*************************************************************************
        ' Function Name : GetAssetDeviceDetailsForSearch
        ' Input         : Siteid,saveid
        ' Output        : XML Node
        '**************************************************************************
        Function GetAssetDeviceDetailsForSearch(ByVal SiteId As String, ByVal SaveId As String, ByVal SearchDeviceIDs As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.Asset_GetDeviceInfoForSearch(SiteId, SaveId, SearchDeviceIDs, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function Asset_EditDevice(ByVal SaveId As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceName As String, ByVal IsDelete As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.Asset_EditSavedSearch(SaveId, SiteId, DeviceId, DeviceName, IsDelete, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

#End Region

        Function ExportI2AssetMetaData(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                'sStrXml = api.ExportI2AssetMetaData(SiteId, apikey)
            Catch ex As Exception

            End Try

            Return sStrXml

        End Function

        Function UpdateMangeRoles(ByVal EnableActiveDirectoryRoles As String, ByVal SSOIAttribute As String, ByVal Roles As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.UpdateMangeRoles(EnableActiveDirectoryRoles, SSOIAttribute, Roles, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetMangeRolesInfo() As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.GetMangeRolesInfo()

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateADDirectory(ByVal ServerIp As String, ByVal UserName As String, ByVal Password As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.ConfigADDirectory(ServerIp, UserName, Password, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetADDirectory(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.GetADDirectory()

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function getServerIPList(ByVal apikey As String, ByVal sid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetDBServerList(sid, apikey)

            Return sStrXml

        End Function

        Function GetPasswordInfo(ByVal apikey As String, ByVal UserId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetPasswordInfo(apikey, UserId)

            Return sStrXml

        End Function

        Function getCompanyList(ByVal apikey As String, ByVal cid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetCompanyList(cid, apikey)

            Return sStrXml

        End Function

        Function getUserTypeList(ByVal apikey As String, ByVal cid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUserTypeListInfo(cid, apikey)

            Return sStrXml

        End Function

        Function getUserRoleList(ByVal apikey As String, ByVal cid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUserRoleListInfo(cid, apikey)

            Return sStrXml

        End Function

        Function getCompanyList(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListCompanies(apikey)

            Return sStrXml

        End Function

        Function getSiteList(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListSites(apikey)

            Return sStrXml

        End Function

        Function getCampusList(ByVal siteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListCampusMap(siteId, apikey)

            Return sStrXml

        End Function

        Function deleteCampusMap(ByVal apikey As String, ByVal siteId As String, ByVal DataId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeleteCampusMap(siteId, DataId, apikey)

            Return sStrXml

        End Function

        Function deleteSiteConfiguration(ByVal apikey As String, ByVal siteId As String, ByVal UserId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeleteSiteConfiguration(siteId, UserId, apikey)

            Return sStrXml

        End Function

        Function getUserList(ByVal siteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListUsers(siteId, apikey)

            Return sStrXml

        End Function

        Function LoadUser(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAllUsers(apikey)

            Return sStrXml

        End Function

        Function getUserActivityLog(ByVal apikey As String, ByVal UserId As String, ByVal FromDate As String, ByVal EventType As String, ByVal ToDate As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUserActivityDetails(UserId, FromDate, EventType, ToDate, apikey)
            Return sStrXml

        End Function

        Function GetAvailableSitesForUser(ByVal apikey As String, ByVal UserId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAvailableSitesForUser(UserId, apikey)

            Return sStrXml

        End Function

        Function ConfigureSitesForUser(ByVal apikey As String, ByVal siteId As String, ByVal UserId As String, ByVal isAdd As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ConfigureSitesForUser(siteId, UserId, isAdd, apikey)

            Return sStrXml

        End Function

        Function deleteUserConfiguration(ByVal apikey As String, ByVal UserId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeleteUserConfiguration(UserId, apikey)

            Return sStrXml

        End Function

        Function SetupSite(ByVal Masterid As String, ByVal SiteId As String, ByVal IsAdd As String, ByVal CompanyId As String, ByVal SiteName As String, ByVal SiteFolder As String, ByVal FileFormat As String,
                           ByVal Status As String, ByVal IsGroup As String, ByVal IsGroupMember As String, ByVal ServerIP As String, ByVal AuthPassword As String, ByVal LocationCode As String,
                           ByVal QBN As String, ByVal IsPrismView As String, ByVal TimeZone As String, ByVal IsUnDeleteTags As String, ByVal IsUnDeleteMonitors As String,
                           ByVal IsDefinedTagsinCore As String, ByVal IsDefinedInfrastructureinCore As String, ByVal IPAddress As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.SetupSite(Masterid, SiteId, IsAdd, CompanyId, SiteName, SiteFolder, FileFormat, Status, IsGroup, IsGroupMember, ServerIP,
                                    AuthPassword, LocationCode, QBN, IsPrismView, TimeZone, IsUnDeleteTags, IsUnDeleteMonitors, IsDefinedTagsinCore, IsDefinedInfrastructureinCore, IPAddress, apikey)

            Return sStrXml

        End Function

        Function SetupCompany(ByVal IsAdd As String, ByVal CompanyId As String, ByVal CompanyName As String, ByVal Status As String, ByVal AuthPassword As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.SetupCompany(IsAdd, CompanyId, CompanyName, Status, AuthPassword, apikey)

            Return sStrXml

        End Function

        Function PageVisitDetails(ByVal UserId As String, ByVal PageName As String, ByVal nPageAction As String, ByVal PageActionHistory As String, ByVal IPAddress As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.PageVisitDetails(UserId, PageName, nPageAction, PageActionHistory, IPAddress, apikey)

            Return sStrXml

        End Function

        Function SetupUser(ByVal UserId As String, ByVal IsAdd As String, ByVal CompanyId As String, ByVal UserName As String, ByVal Password As String, ByVal Status As String, ByVal UserType As String,
                           ByVal Email As String, ByVal Batteryreplacement As String, ByVal UserTypeId As String, ByVal UserRoleId As String, ByVal AssociatedEmail As String, ByVal AssociatedPhone As String,
                           ByVal AuthPassword As String, ByVal IsTempMonitoring As String, ByVal IsPrismView As String, ByVal IsPrismAuditView As String, ByVal AllowAccessForStar As String,
                           ByVal AssetNotification As String, ByVal DesktopNotification As String, ByVal IsPrismReadOnly As String, ByVal AllowAccessForKPI As String, ByVal isPulseReport As String,
                           ByVal PulseReportIds As String, ByVal FirstName As String, ByVal LastName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.SetupUser(UserId, IsAdd, CompanyId, UserName, Password, Status, UserType, Email, Batteryreplacement, UserTypeId, UserRoleId, AssociatedEmail, AssociatedPhone, AuthPassword,
                                    IsTempMonitoring, IsPrismView, IsPrismAuditView, AllowAccessForStar, AssetNotification, DesktopNotification, IsPrismReadOnly, AllowAccessForKPI,
                                    isPulseReport, PulseReportIds, FirstName, LastName, apikey)
            Return sStrXml

        End Function

        '*************************************************************************
        ' Function Name : SetupUser
        ' Input         : UserId, IsAdd, CompanyId, UserName, Password, Status, UserType, Email, Batteryreplacement, UserTypeId, UserRoleId, AuthUserId, AuthPassword, apikey
        ' Output        : XML Node
        '**************************************************************************
        Function SetUpSiteAlert(ByVal InfrAlertIds As String, ByVal InfrAlertSiteIds As String, ByVal ReportIds As String, ByVal ReportISiteIds As String, ByVal Email As String, ByVal AuthenticationKey As String) As XmlNode

            Dim sStrXml As XmlNode
            sStrXml = api.SetupScheduleAlerts(InfrAlertIds, InfrAlertSiteIds, ReportIds, ReportISiteIds, Email, AuthenticationKey)
            Return sStrXml

        End Function

        Function AddEditUserWithSite(ByVal sid As String, ByVal EmailId As String, ByVal AlertType As String, ByVal Status As String, ByVal apikey As String, ByVal emailAlertList As String, ByVal EmailDataId As String) As XmlNode

            Dim sStrXml As XmlNode
            'sStrXml = api.AddEditUserWithSite(sid, EmailId, apikey, Status, AlertType, emailAlertList, EmailDataId)
            Return sStrXml

        End Function
        Function GetLogList(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListLogViewer(apikey)

            Return sStrXml

        End Function

        Function GetResponseHistory(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetServerResponseHistory(0, apikey)

            Return sStrXml

        End Function

        Function GetThresholdConfiguration(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            'sStrXml = api.ListThresholdConfiguration(apikey)

            Return sStrXml

        End Function

        Function SetupThresholdConfiguration(ByVal DataId As String, ByVal IsAdd As String, ByVal SiteDownEmail As String, ByVal SoftwareDownEmail As String, ByVal LowHardDiskMemory As String,
                                             ByVal LastUpdateThreshold As String, ByVal SMSSenderID As String, ByVal SWSMSSenderID As String, ByVal ActiveListFailedCount As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            'sStrXml = api.AddThresholdConfiguration(DataId, IsAdd, SiteDownEmail, SoftwareDownEmail, LowHardDiskMemory, LastUpdateThreshold, SMSSenderID, SWSMSSenderID, ActiveListFailedCount, apikey)

            Return sStrXml

        End Function

        Function UpdateLocalId(ByVal SiteId As String, ByVal DeviceType As String, ByVal TagId As String, ByVal LocalId As String, ByVal MonitorId As String,
                               ByVal Location As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.UpdateLocalIdForTag(SiteId, TagId, MonitorId, Location, LocalId, DeviceType, apikey)

            Return sStrXml

        End Function

        Function Update_TagProfiles(ByVal SiteId As Integer, ByVal DeviceType As String, ByVal TagId As String, ByVal TagCategory As String, _
                                                  ByVal IRProfile As String, ByVal LongIR As String, ByVal IRReportRate As String, ByVal RFReportRate As String, ByVal IRRX As String, _
                                                  ByVal FastPushbutton As String, ByVal LFRange As String, ByVal DisableLF As String, ByVal OperatingMode As String, ByVal WiFiReportingTime As String, _
                                                  ByVal WiFiIn900MHz As String, ByVal PagingProfile As String, ByVal UpdateRate As String, ByVal IsDefaultProfile As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.Configure_TagProfilesInfo(SiteId, DeviceType, TagId, TagCategory, IRProfile, LongIR, IRReportRate, RFReportRate, IRRX, FastPushbutton, LFRange, DisableLF, OperatingMode, WiFiReportingTime, WiFiIn900MHz, PagingProfile, UpdateRate, IsDefaultProfile, apikey)

            Return sStrXml

        End Function

        Function GetDefaultProfileInfo_Site(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.GetDefaultProfileInfo(SiteId, DeviceType, DeviceId, apikey)
            Catch ex As Exception

            End Try

            Return sStrXml

        End Function

        Function GetCompanyGroup(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetCompanyGroup(apikey)

            Return sStrXml

        End Function

        Function AddCompanyGroup(ByVal GroupId As String, ByVal GroupName As String, ByVal CompanyId As String, ByVal IsAdd As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.AddCompanyGroup(GroupId, GroupName, CompanyId, IsAdd, apikey)

            Return sStrXml

        End Function

        Function GetSitePendingFiles(ByVal ServerIP As String, ByVal DBName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListSitePendingFiles(ServerIP, DBName, apikey)

            Return sStrXml

        End Function

        Function UploadG2Summary(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.ImportG2Summary(SiteId, file, fileName, "", apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function ConfigEmailForAutomatedReports(ByVal Emails As String, ByVal aAlertId As Integer, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ConfigEmailForAutomatedReports(Emails, aAlertId, apikey)

            Return sStrXml

        End Function

        Function GetServerAction(ByVal Status As String, ByVal Message As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetServerAction(Status, Message)

            Return sStrXml

        End Function

        Function DBCleanBuffer(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DBCleanBuffer(apikey)

            Return sStrXml

        End Function

        Function DownloadLogFile(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                sStrXml = api.DownloadLogFile(SiteId, apikey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetFiles(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String, ByVal apikey As String) As String

            Dim sFiles As String = ""

            'sFiles = apiFileCount.GetFiles(strSitePath, strSiteFolder, strLastParsedFile, strFileFormat)
            Return sFiles

        End Function

        Function GetPCServerLogFiles(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal apikey As String) As String

            Dim sFiles As String = ""

            'sFiles = apiFileCount.GetFilesUnparsedServerLog(strSitePath, strSiteFolder, strLastParsedFile)
            Return sFiles

        End Function

        Function DeviceSummary(ByVal Siteid As Integer, ByVal DeviceType As Integer, ByVal Days As Integer, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                sStrXml = api.GetDeviceSummary(Siteid, DeviceType, Days, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function JunkDeviceCount(ByVal SiteId As Integer, ByVal Count As Integer, ByVal Days As Integer, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                sStrXml = api.JunkDeviceList(SiteId, Count, Days, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function ArchiveJunk(ByVal Siteid As Integer, ByVal DeviceType As Integer, ByVal apikey As String) As XmlNode

            Dim SstrXml As XmlNode

            Try

                SstrXml = api.ArchiveDevices(Siteid, DeviceType, apikey)

            Catch ex As Exception

            End Try

            Return SstrXml

        End Function

        Function getEventType(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetAllEventType(apikey)

            Return sStrXml

        End Function

        Function GetOnDemandReports(ByVal SiteId As String, ByVal TypeIds As String, ByVal strdate As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            api.Timeout = 60 * 30 * 1000 '30 minutes

            sStrXml = api.GetOnDemandReports(SiteId, TypeIds, strdate, apikey)

            Return sStrXml

        End Function

        Function getGMSServiceList(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetNewUpgradeVersion(apikey)

            Return sStrXml

        End Function

        Function getGMSService(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGMSServiceInfo(apikey)

            Return sStrXml

        End Function

        Function AddGMSService(ByVal ServiceId As String, ByVal Version As String, ByVal fileUrl As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                sStrXml = api.AddUpgradeVersion(ServiceId, Version, fileUrl, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function DeleteParserService(ByVal apikey As String, ByVal DataId As String) As XmlNode
            Dim sStrXml As XmlNode

            sStrXml = api.DeleteParserService(DataId, apikey)

            Return sStrXml

        End Function

        Function GetUpgradeHistory(ByVal DataId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode
            sStrXml = api.GetUpgradeHistory(DataId, apikey)
            Return sStrXml

        End Function

        Function GetRDBServices(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetRDBServices(apikey)

            Return sStrXml

        End Function

        Function RestartParserService(ByVal apikey As String, ByVal ServerIP As String, ByVal DBName As String, ByVal ServiceId As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.RestartParserService(ServerIP, DBName, ServiceId, apikey)

            Return sStrXml

        End Function

        Function GetDeviceMonitorHourlyInfo(ByVal apikey As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceType As String, ByVal Duration As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.DeviceHourlyInfo(SiteId, DeviceType, DeviceId, Duration, apikey)

            Return sStrXml

        End Function

        Function GetGMSBeaconStarSlotDetails(ByVal apikey As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal sDate As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGMSBeaconStarSlotDetails(SiteId, DeviceId, sDate, apikey)

            Return sStrXml

        End Function

        Function GetGMSStarSlotDetails(ByVal apikey As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal sDate As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGMSStarSlotDetails(SiteId, DeviceId, sDate, apikey)

            Return sStrXml

        End Function

        Function GetGMSMonitorGroupsInfoByDeviceId(ByVal apikey As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal sDate As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGMSMonitorGroupsInfoByDeviceId(SiteId, DeviceId, sDate, apikey)
            Return sStrXml

        End Function

        Function GetGMSReportForSite(ByVal apikey As String, ByVal SiteId As String, ByVal ReportType As String, ByVal DeviceType As String, ByVal DeviceId As String,
                                     ByVal FromDate As String, ByVal ToDate As String, ByVal StarId As String, ByVal IsChkTTSyncError As Integer, ByVal IsConnFailed As String,
                                     ByVal PagingCount As String, ByVal LocationCount As String, ByVal TriggerCount As String, ByVal FilterCond As String, ByVal LocationFilterCond As String,
                                     ByVal TriggerFilterCond As String, ByVal GroupCond As String, ByVal StarsUpgradeMode As String, ByVal StarsTTSyncError As String,
                                     ByVal StarsNotReceivingData As String, ByVal StarsNotReceiving24hr As String, ByVal StarsSeenNetworkIssue As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGMSReportForSite(SiteId, ReportType, DeviceType, DeviceId, FromDate, ToDate, StarId, IsChkTTSyncError, IsConnFailed, PagingCount,
                                              LocationCount, TriggerCount, FilterCond, LocationFilterCond, TriggerFilterCond, GroupCond, StarsUpgradeMode, StarsTTSyncError,
                                              StarsNotReceivingData, StarsNotReceiving24hr, StarsSeenNetworkIssue, apikey)
            Return sStrXml

        End Function

        Function GetStarDailyPLCount(ByVal apikey As String, ByVal SiteId As String, ByVal sDate As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetStarDailyPLCount(SiteId, sDate, apikey)
            Return sStrXml

        End Function

        Function GetWPSUIFiles(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String, ByVal apikey As String) As String

            Dim sFiles As String = ""

            ' sFiles = apiWPSFileCount.GetWPSUILogFiles(strSitePath, strSiteFolder, strLastParsedFile, strFileFormat)
            Return sFiles

        End Function

        Function GetWPSUISetttingsFiles(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String, ByVal apikey As String) As String

            Dim sFiles As String = ""

            ' sFiles = apiWPSFileCount.GetWPSUISettingsFiles(strSitePath, strSiteFolder, strLastParsedFile, strFileFormat)
            Return sFiles

        End Function

        Function getGuardianSiteListInfo(ByVal apikey As String, ByVal sid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetGuardianSiteListInfo(sid, apikey)

            Return sStrXml

        End Function

        Function GetConfiguredGuardianSiteList(ByVal apikey As String, ByVal sid As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetConfiguredGuardianSiteList(sid, apikey)

            Return sStrXml

        End Function

        Function UpdateWPSAdminNewPassword(ByVal apikey As String, ByVal sid As String, ByVal sAdminUserId As String, ByVal sAdminUserName As String, ByVal sAdminPassword As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.UpdateWPSAdminNewPassword(apikey, sid, sAdminUserId, sAdminUserName, sAdminPassword)

            Return sStrXml

        End Function

        Function ConfigureWPSSiteInGMS(ByVal apikey As String, ByVal sid As String, ByVal sInsert As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ConfigureWPSSiteInGMS(apikey, sid, sInsert)

            Return sStrXml

        End Function

        Function getSupportList(ByVal statval As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUsersupportLog(statval, apikey)

            Return sStrXml

        End Function

        Function SetupSupport(ByVal Masterid As String, ByVal SiteId As String, ByVal SupportUrl As String, ByVal IsAdd As String, ByVal IsStatus As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.SetupSupport(Masterid, SiteId, SupportUrl, IsAdd, IsStatus, apikey)

            Return sStrXml

        End Function

        Function LoadUsersupport(ByVal StatVal As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUsersupportLog(StatVal, apikey)

            Return sStrXml

        End Function

        Function LoadDisasterRecovery(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                'DeviceType 1-Tag, 2-monitor, 3-Star
                'bin        0-Good, 1- underwatch(90days), 2-Lbi(30days) 
                api.Timeout = 60 * 10 * 1000 '10 minutes

                sStrXml = api.ExportDisasterRecovery(SiteId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function ImportDisasterRecovery(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                api.Timeout = 60 * 30 * 1000 '10 minutes

                sStrXml = api.ImportDisasterRecoveryForSite(SiteId, file, fileName, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function AddSitesForSettings(ByVal SiteList As String, ByVal Status As String, ByVal FileFormat As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.AddSitesForSettings(SiteList, Status, FileFormat, apikey)

            Return sStrXml

        End Function

        Function GETSitesForSettings(ByVal Status As String, ByVal FileFormat As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetSitesForSettings(Status, FileFormat, apikey)

            Return sStrXml

        End Function

        Function GetHealthOverviewReport(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetHealthOverviewReport(apikey, SiteId)

            Return sStrXml

        End Function

        Function GetHealthDetailReport(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            api.Timeout = 60 * 30 * 1000 '10 minutes

            sStrXml = api.GetHealthDetailReport(SiteId, apikey)

            Return sStrXml

        End Function

        Function UpdateDevLocationForSite(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal Location As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.UpdateDeviceLocationForSite(SiteId, DeviceType, DeviceId, Location, apikey)

            Return sStrXml

        End Function

        Function Get_CenTrakModelItems(ByVal apikey As String, ByVal Devicetype As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetCenTrakModelItems(apikey, Devicetype)

            Return sStrXml

        End Function

        Function getStreamingFieldsList(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListStreamingFields(SiteId, apikey)

            Return sStrXml

        End Function

        Function getMSESettingsList(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListMSESettings(SiteId, apikey)

            Return sStrXml

        End Function

        Function getMSESettingsHistoryList(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.ListMSESettingsHistory(SiteId, apikey)

            Return sStrXml

        End Function

        Function GetOutboundReport(ByVal SiteId As Integer, ByVal nMonth As Integer, ByVal nYear As Integer, ByVal IsDetailReport As Boolean, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetOutboundReport(SiteId, nMonth, nYear, IsDetailReport, apikey)

            Return sStrXml

        End Function
        
	Function LoadMonitorGroupReport(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                api.Timeout = 60 * 10 * 1000 '10 minutes
                sStrXml = api.MonitorGroupReport(SiteId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function UpdateDeviceLocations(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal DeviceType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                api.Timeout = 60 * 30 * 1000 '10 minutes

                sStrXml = api.ImportDeviceLocations(SiteId, file, fileName, DeviceType, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function LoadDeviceLocations(ByVal SiteId As String, ByVal DeviceType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                sStrXml = api.GetDeviceLocationsForSite(SiteId, DeviceType, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
	
        Function DeleteCompanyGroup(ByVal apikey As String, ByVal GroupId As String) As XmlNode
	
            Dim sStrXml As XmlNode
	    
            sStrXml = api.DeleteCompanyGroup(GroupId, apikey)
	    
            Return sStrXml
	    
        End Function

        Function GetSiteADGroup(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode
	    
            sStrXml = api.GetSiteADGroup(apikey)
	    
            Return sStrXml

        End Function

        Function AssociationSite(ByVal SiteId As String, ByVal GroupName As String, ByVal GroupId As String, ByVal VHAGroupId As String, ByVal IsAdd As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode
	    
            sStrXml = api.AssociationSite(SiteId, GroupName, GroupId, VHAGroupId, IsAdd, apikey)
	    
            Return sStrXml

        End Function
    
        Function DeleteSiteAssociation(ByVal apikey As String, ByVal GroupId As String) As XmlNode
	
            Dim sStrXml As XmlNode
	    
            sStrXml = api.DeleteSiteAssociation(GroupId, apikey)
	    
            Return sStrXml
	    
        End Function
       
        Function getADGroupListInfo(ByVal apikey As String, ByVal sid As String) As XmlNode

            Dim sStrXml As XmlNode
	    
            sStrXml = api.GetVHAGroupListInfo(sid, apikey)
	    
            Return sStrXml

        End Function

        Function UploadRTLSDetail(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                'sStrXml = api.UploadRTLSDetail(SiteId, file, fileName, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function getAlertMasterListInfo(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode
	    
            sStrXml = api.GetAlertMaster(apikey)
	    
            Return sStrXml

        End Function

#Region "For BatteryReplacementFailureReport"

        '*************************************************************************
        ' Function Name : BatteryReplacementFailure
        ' Input         : Siteid, api key
        ' Output        : XML Node
        '**************************************************************************
        Function BatteryReplacementFailure(ByVal SiteId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                api.Timeout = 60 * 10 * 1000 '10 minutes

                sStrXml = api.BatteryReplacementFailureReport(SiteId, apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml
        End Function

#End Region

#Region "For Site Analysis Report"

        Function SiteAnalysisReport(ByVal SiteId As String, ByVal ReportType As String, ByVal DeviceType As String, ByVal Products As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = api.GetSiteAnalysisReport(SiteId, ReportType, DeviceType, Products, "2X", apikey)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml
        End Function

#End Region

#Region "For Centrak Volt Detail Report"

        Function CenTrakVoltDetailReport(ByVal SiteId As String, ByVal DeviceType As String, ByVal FromDate As String, ByVal ToDate As String, ByVal UserId As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try

                'sStrXml = api3.getAuditLog(apikey, SiteId, DeviceType, FromDate, ToDate, UserId, "2X")

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml
	    
        End Function

#End Region

#Region "For Cetani Meta Data"

        Function GetCetaniMetadataForSite(ByVal SiteId As String, ByVal SearchValue As String, ByVal CurrPage As String, ByVal PageSize As String, ByVal devicetype As String, ByVal AuthenticationKey As String) As XmlNode

            Dim sStrXml As XmlNode = Nothing

            Try
                sStrXml = api.GetCetaniMetaData(SiteId, SearchValue, CurrPage, PageSize, devicetype, AuthenticationKey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

#End Region

        Function GetEMTagActivityReport(ByVal ReportType As Integer, ByVal ReportDate As String, ByVal Email As String, ByVal SiteId As String,
                                 ByVal SubType As String, ByVal isDailyData As String, ByVal TagId As String, ByVal AuthenticationKey As String) As XmlNode

            Dim sStrXml As XmlNode

            Try
                ''sStrXml = api.EMTagActivityReport(ReportType, ReportDate, Email, SiteId, SubType, isDailyData, TagId, AuthenticationKey)
            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function

        Function GetLocationChangeEvent(ByVal SiteId As String, ByVal DeviceId As String, ByVal FromDate As String, ByVal ToDate As String, ByVal Type As String, ByVal EventTreshold As String,
                                         ByVal inMonitorIds As String, ByVal exMonitorIds As String, ByVal TimeSpend As String, ByVal SpendType As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            api.Timeout = 60 * 20 * 1000 '20 minutes

            sStrXml = api.GetLocationChangeEventReport(SiteId, DeviceId, FromDate, ToDate, Type, EventTreshold, exMonitorIds, inMonitorIds, TimeSpend, SpendType, apikey)

            Return sStrXml

        End Function

        Function GetUserPulseReport(ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode

            sStrXml = api.GetUserPulseReport(apikey)

            Return sStrXml

        End Function

        Function GetEMRMAreport(ByVal SiteId As String, ByVal FromDate As String, ByVal ToDate As String) As XmlNode

            Dim sStrXml As XmlNode

            Try

                ''sStrXml = api.GetEMRMAreport(SiteId, FromDate, ToDate, g_UserAPI)
            Catch ex As Exception
                WriteLog("GetEMEMRMAreport : " & ex.Message.ToString())
            End Try

            Return sStrXml

        End Function
        Function HistoricalTemperatureReport(ByVal SiteId As String, ByVal sDeviceId As String, ByVal FromDate As String, ByVal ToDate As String, ByVal apikey As String) As XmlNode

            Dim sStrXml As XmlNode
            Dim sReturnxml As XmlNode

            Dim AuthKey As String = ""

            Try

                sReturnxml = GMSAPI.UserAuthentication("gmsapiuser", "gmsapiuser123!")

                Dim responsefromServer As String = sReturnxml.Item("Response").InnerText

                If responsefromServer.ToLower() = "true" Then
                    apikey = sReturnxml.Item("AuthenticationKey").InnerText
                End If

                sStrXml = GMSAPI.GetHistoricalTemperature(SiteId, apikey, sDeviceId, FromDate, ToDate)

            Catch ex As Exception
                'exception
            End Try

            Return sStrXml

        End Function
    End Class

End Namespace

