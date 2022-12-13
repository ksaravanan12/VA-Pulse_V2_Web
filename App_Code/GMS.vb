Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI

    Public Module GMS
    
        Dim API As New GmsAPICall
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function loadsiteList(ByVal VACompanys As String, ByVal VASites As String, Optional ByVal sid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getSiteListInfo(VACompanys, VASites, g_UserAPI, sid)
                dt = siteList_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function loadsiteInfo(ByVal VACompanys As String, ByVal VASites As String, Optional ByVal sid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getSiteSummary(g_UserAPI, sid, VACompanys, VASites)

                If sStrXml Is Nothing Then
                    sStrXml = API.getSiteSummary(g_UserAPI, sid, VACompanys, VASites)
                End If

                If Not sStrXml Is Nothing Then
                    dt = siteinfo_XMLNodetoDataTable(sStrXml)
                Else
                    WriteLog(" loadsiteInfo No return XML for API.getSiteSummary, g_UserAPI= " & g_UserAPI & " and sid=" & sid)
                End If

            Catch ex As Exception
                WriteLog(" loadsiteInfo : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
               
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadsiteOverview(ByVal sId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
                sStrXml = API.SiteDetailedOverview(g_UserAPI, sId)
                dt = siteOverview_XMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadsiteOverview : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All alert Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function Loadalertinfo(ByVal sId As String, ByVal VACompanys As String, ByVal VASites As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.SiteAlertList(g_UserAPI, sId, VACompanys, VASites)
                dt = SiteAlertList_XMLNodetoDataTable(sStrXml)
            Catch ex As Exception
                WriteLog(" LoadsiteOverview : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadDevicePhotoList                                                                        '
        '   DESCRIPTION     :   Used to List specific device of site photo list                                            '   
        '   PARAM           :   SiteId,DeviceId,DeviceType		                                                           ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadDevicePhotoList(ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceType As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.DevicePhotoList(SiteId, DeviceId, DeviceType)
		
                If (devicetype = "1") Then
                    dt = SiteDevicePhotoLsit_XMLNodetoDataTable(sStrXml)
                ElseIf (devicetype = "2") Then
                    dt = SiteDevicePhotoLsit_XMLNodetoDataTable(sStrXml)
                ElseIf (devicetype = "3") Then
                    dt = SiteDevicePhotoLsit_XMLNodetoDataTable(sStrXml)
                End If

            Catch ex As Exception
                WriteLog(" LoadDeviceList : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AddDevice_Image                                                                            '
        '   DESCRIPTION     :   Used to add photo/images to device of site                                                 '   
        '   PARAM           :   DataId,DeviceId,SiteId,DeviceInfo,Image,ImageName,DeviceType,EditMode,IsActive             ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddDevice_Image(ByVal DataId As String, ByVal DeviceId As String, ByVal SiteId As String, ByVal DeviceInfo As String, ByVal Image As Byte(), ByVal ImageName As String, ByVal DeviceType As String, ByVal EditMode As String, ByVal IsActive As String) As DataTable
            
	    Dim sStrXml As XmlNode
	    
            Dim ds As New DataSet
            Dim dt As New DataTable

            Try
	    
                sStrXml = API.Device_AddDevice_Image(DataId, DeviceId, SiteId, DeviceInfo, Image, ImageName, DeviceType, EditMode, IsActive)
                dt = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                'exception
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AddDevice_Image                                                                            '
        '   DESCRIPTION     :   Used to add photo/images to device of site                                                 '   
        '   PARAM           :   DataId,DeviceId,SiteId,DeviceInfo,Image,ImageName,DeviceType,EditMode,IsActive             ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function Add_Image(ByVal SiteId As String, ByVal Image As Byte(), ByVal FileName As String, ByVal UserName As String, ByVal Description As String) As DataTable
            
	    Dim sStrXml As XmlNode
	    
            Dim ds As New DataSet
            Dim dt As New DataTable

            Try
	    
                sStrXml = API.Campus_Add_Image(SiteId, Image, FileName, UserName, Description)
                dt = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                'exception
            End Try

            Return dt
	    
        End Function
   
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadTagList                                                                               '
        '   DESCRIPTION     :   Used to List specific site tag list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadDeviceList(ByVal SiteId As String, ByVal devicetype As String, ByVal typeId As String, ByVal DeviceId As String,
                                ByVal CurPage As String, ByVal AlertId As String, ByVal bin As String, Optional ByVal sorclname As String = "TagId",
                                Optional ByVal sortOrder As String = "ASC") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.DeviceList(SiteId, devicetype, typeId, DeviceId, CurPage, AlertId, g_UserAPI, bin, sorclname, sortOrder)

                If (devicetype = "1") Then
                    dt = SiteDeviceListtag_XMLNodetoDataTable(sStrXml)
                ElseIf (devicetype = "2") Then
                    dt = SiteDeviceLsit_XMLNodetoDataTable(sStrXml)
                ElseIf (devicetype = "3") Then
                    dt = SiteDevice_Star_XMLNodetoDataTable(sStrXml)
                End If

            Catch ex As Exception
                WriteLog(" LoadDeviceList : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadEmTags                                                                               '
        '   DESCRIPTION     :   Used to List specific site tag list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadEmTags(ByVal SiteId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.EMTagsDeviceList(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

                'dt = EMTag_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadDeviceList : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function

        Function LoadEMTagDetail(ByVal SiteId As String, ByVal EMReportType As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.EMTagDetailReport(SiteId, EMReportType, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadEMTagDetail : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function LoadStarList(ByVal SiteId As String, ByVal devicetype As String, ByVal apikey As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.DeviceList(SiteId, devicetype, "", "", "0", "", apikey, "3", "", "")
                dt = SiteDevice_Star_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadStarList : " & ex.Message.ToString())
            End Try
            Return dt

        End Function

        Function LoadDeviceDetails(ByVal SiteId As String, ByVal devicetype As String, ByVal typeId As String, ByVal DeviceId As String, ByVal CurPage As String, ByVal AlertId As String,
                                   ByVal bin As String, Optional ByVal sorclname As String = "TagId", Optional ByVal sortOrder As String = "ASC") As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Dim xmlDoc As New XmlDocument

            Try

                sStrXml = API.DeviceList(SiteId, devicetype, typeId, DeviceId, CurPage, AlertId, g_UserAPI, bin, sorclname, sortOrder)
                xmlDoc.LoadXml(sStrXml.OuterXml)

                Dim xmlReader As XmlNodeReader = New XmlNodeReader(xmlDoc)
                ds.ReadXml(xmlReader)

            Catch ex As Exception
                WriteLog(" LoadDeviceList : " & ex.Message.ToString())
            End Try

            Return ds

        End Function
                '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadGlossaryinfo
        '   DESCRIPTION     :   it will get GMS Page Glossary info                                                       '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadGlossaryinfo(ByVal pagename As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.GMSGlossaryinfo(g_UserAPI)
                dt = Glossaryinfo_XMLNodetoDataTable(sStrXml, pagename)
		
            Catch ex As Exception
                WriteLog(" LoadPatientTagList : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   Load_Alert_site_list                                                                                   '
        '   DESCRIPTION     :   Used to List All Alert Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function Load_Alert_site_list(Optional ByVal sid As String = "", Optional ByVal VACompanys As String = "", Optional ByVal VASites As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetAlertInfo(g_UserAPI, sid, VACompanys, VASites)
                dt = siteAlertinfo_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   Load_Alert_site_list                                                                                   '
        '   DESCRIPTION     :   Used to List All Alert Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function Load_DeviceProfile_List(Optional ByVal sid As String = "", Optional ByVal devicetype As String = "", Optional ByVal deviceid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.DeviceProfile(g_UserAPI, sid, devicetype, deviceid)
                dt = General_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   Load_DeviceActivity_List                                                                   '
        '   DESCRIPTION     :   Used to List All Alert Site                                                                '
        '   PARAM           :   sid,devicetype,deviceid                                                                    '
        '   RETURN TYPE     :   DataTable                                                                                     '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function Load_DeviceActivity_List(Optional ByVal sid As String = "", Optional ByVal devicetype As String = "",
                                          Optional ByVal deviceid As String = "", Optional ByVal period As String = "", Optional ByVal fromDate As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Dim sXml As String = ""

            Try

                sStrXml = API.DeviceActivity(g_UserAPI, sid, devicetype, deviceid, period, fromDate)
                dt = ActivityXMLNodetoDataTable(sStrXml, devicetype, period)

            Catch ex As Exception
                WriteLog(" Load_DeviceActivity_List : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   Load_10 hour data                                                                        '
        '   DESCRIPTION     :   10 hr data                                                                   '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function Load_10hrdata(Optional ByVal sid As String = "", Optional ByVal devicetype As String = "", Optional ByVal deviceid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.Load10hrdata(g_UserAPI, sid, devicetype, deviceid)
                dt = General_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AddEmail                                                                                   '
        '   DESCRIPTION     :   Insert email to database.
        '   PARAM           :   Email, Status		                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddEmail(ByVal siteid As Integer, ByVal Email As String, ByVal AlertType As String, ByVal Status As String, ByVal IsAdd As String, ByVal EmailDataId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.SetupEmail(siteid, Email, AlertType, Status, g_UserAPI, IsAdd, EmailDataId)

                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddEmail : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function AddEditEmailWithAlertsForSite(ByVal siteid As Integer, ByVal Email As String, ByVal AlertType As String, ByVal Status As String, ByVal emailAlertList As String, ByVal EmailDataId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AddEditEmailWithAlertsForSite(siteid, Email, AlertType, Status, g_UserAPI, emailAlertList, EmailDataId)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddEmailWithAlertsForSite : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetSearchResult                                                                                   '
        '   DESCRIPTION     :   Reurn result for filter and devices type 
        '   PARAM           :   DeviceType		                                                                       ' 
        '   RETURN TYPE     :   Xml Element
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetSearchResult(ByVal DeviceType As String) As XmlElement

            Dim dt As New DataTable
            Dim sStrXml As XmlElement

            Try
                sStrXml = API.Search(DeviceType, g_UserAPI)
            Catch ex As Exception
                WriteLog(" GetSearchResult : " & ex.Message.ToString())
            End Try

            Return sStrXml

        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetDeviceSearchResult                                                                                   '
        '   DESCRIPTION     :   Reurn result for filter and devices type 
        '   PARAM           :   DeviceType		                                                                       ' 
        '   RETURN TYPE     :   Xml Element
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetDeviceSearchResult(ByVal siteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal CurPage As String, ByVal FilterCriteria As String, ByVal sCompanys As String, ByVal sSites As String) As XmlElement

            Dim dt As New DataTable
            Dim sStrXml As XmlElement = Nothing

            Try

                sStrXml = API.Device_Search(siteId, DeviceType, DeviceId, CurPage, g_UserAPI, FilterCriteria, sCompanys, sSites)

            Catch ex As Exception
                WriteLog(" GetDeviceSearchResult : " & ex.Message.ToString())
            End Try
            Return sStrXml
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   EmailList                                                                                   '
        '   DESCRIPTION     :   Insert email to database.
        '   PARAM           :   Email, Status		                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ListEmail(ByVal siteid As Integer, ByVal CurPage As String, ByVal VACompanys As String, ByVal VASites As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.getEmailList(siteid, CurPage, VACompanys, VASites, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" EmailList : " & ex.Message.ToString())
            End Try
            Return dt
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   TagMovementsList                                                                                   '
        '   DESCRIPTION     :   Insert email to database.
        '   PARAM           :   Email, Status		                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function TagMovementsList(ByVal SiteId As Integer, ByVal DeviceId As Integer, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.getTagMovementsList(SiteId, DeviceId, FromDate, ToDate, g_UserAPI)
                dt = TagMovementXMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" TagMovementsList : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   HeartBeatContentList                                                                                   '
        '   DESCRIPTION     :   List Heart Beat Content
        '   PARAM           :   Email, Status		                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function HeartBeatContentList(ByVal SiteId As Integer) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.getHeartBeatList(SiteId, g_UserAPI)
                dt = HeartBeatXMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" HeartBeatContentList : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AlertHistory                                                                                   '
        '   DESCRIPTION     :   List Alert History
        '   PARAM           :   None	                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AlertHistoryForService(ByVal siteId As String, ByVal fromDate As String, ByVal ToDate As String, ByVal ServiceId As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.getAlertHistoryList(g_UserAPI, siteId, fromDate, ToDate, ServiceId)
                dt = AlertHistoryXMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" AlertHistoryForService : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LocalAlertForSite                                                                                   '
        '   DESCRIPTION     :   List Local Alert History for Site
        '   PARAM           :   None	                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LocalAlertForSite(ByVal siteId As String, ByVal fromDate As String, ByVal ToDate As String, ByVal deviceId As String, ByVal deviceType As String, ByVal serviceId As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.getLocalAlertForSite(g_UserAPI, siteId, fromDate, ToDate, deviceId, deviceType, serviceId)
                dt = LAAlertsForSiteXMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LocalAlertForSite : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   SESSION EXPIRED                                                                                   '
        '   DESCRIPTION     :   Insert email to database.
        '   PARAM           :   Email, Status		                                                                       ' 
        '   RETURN TYPE     :   No return type
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function SessionExpired() As DataTable

            Dim dt As New DataTable

            Try
                dt = GetSessionExpireDataTable()
            Catch ex As Exception
                WriteLog(" SessionExpired : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   DeviceRemovalfromMasterList                                                                                   '
        '   DESCRIPTION     :   Deletes the Devices
        '   PARAM           :   DeviceType, SiteId, sDeviceIds	                                                                       ' 
        '   RETURN TYPE     :   Returns the DeviceType List
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function DeviceRemovalfromMasterList(ByVal DeviceType As String, ByVal SiteId As String, ByVal sDeviceIds As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.DeviceRemoval(g_UserAPI, DeviceType, SiteId, sDeviceIds)
                dt = XMLNodeToDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" DeviceRemovalfromMasterList : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
        '   NAME            :   DeleteEmailConfiguration                                                                                   '
        '   DESCRIPTION     :   Deletes the email configuration
        '   PARAM           :   SiteId, emailId, curpage	                                                                       ' 
        '   RETURN TYPE     :   Returns the email List
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function DeleteEmailConfiguration(ByVal siteId As String, ByVal emailId As String, ByVal curPage As String, ByVal VACompanys As String, ByVal VASites As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.deleteEmailConfiguration(g_UserAPI, siteId, emailId, curPage, VACompanys, VASites)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" DeleteEmailConfiguration : " & ex.Message.ToString())
            End Try

            Return dt
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetAvailableAlertsForEmail                                                                                   '
        '   DESCRIPTION     :   Gets the Alerts configuration for The Email
        '   PARAM           :   SiteId, emailId	                                                                       ' 
        '   RETURN TYPE     :   Returns the Alert list for the email
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetAvailableAlertsForEmail(ByVal siteId As String, ByVal emailId As String) As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.getAvailableAlertsForEmail(g_UserAPI, siteId, emailId)
                dt = XMLNodeToDataTable(sStrXml)
            Catch ex As Exception
                WriteLog(" GetAvailableAlertsForEmail : " & ex.Message.ToString())
            End Try

            Return dt
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetAlertidForEmail                                                                                   '
        '   DESCRIPTION     :   Gets the Alerts configuration for The Email
        '   PARAM           :   emailId	                                                                       ' 
        '   RETURN TYPE     :   Returns the Alertid for the email
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetSchuledReports(ByVal emailId As String) As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.GetSchuledReports(g_UserAPI, emailId)
                dt = XMLNodeToDataTable(sStrXml)
            Catch ex As Exception
                WriteLog(" GetAvailableAlertsForEmail : " & ex.Message.ToString())
            End Try

            Return dt
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   EditAlertsForEmail                                                                                   '
        '   DESCRIPTION     :   Configures the alerts for Email
        '   PARAM           :   SiteId, emailId,AlertId,isAdd	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function EditAlertsForEmail(ByVal siteId As String, ByVal emailId As String, ByVal AlertId As String, ByVal isAdd As String, ByVal mode As String, ByVal alertType As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.ConfigureAlertsForSite(g_UserAPI, siteId, emailId, AlertId, isAdd, mode, alertType)
                dt = XMLNodeToDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" EditAlertsForEmail : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetUserReportSchedule                                                                                   '
        '   DESCRIPTION     :   Configures the user for Email
        '   PARAM           :   none             	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateUserReportSchedule(ByVal SiteId As Integer, ByVal recurrence As String, ByVal scheduletime As String, ByVal weeklyinterval As String, ByVal monthlyinterval As String,
                                          ByVal yearlyinterval1 As String, ByVal yearlyinterval2 As String) As Boolean
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateUserReportSchedule(SiteId, recurrence, scheduletime, weeklyinterval, monthlyinterval, yearlyinterval1, yearlyinterval2, g_UserAPI)
            
	    Catch ex As Exception
                WriteLog(" GetUserReportSchedule : " & ex.Message.ToString())
            End Try

            Return True
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetUserReportSchedule                                                                                   '
        '   DESCRIPTION     :   Configures the user for Email
        '   PARAM           :   none             	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateAlertSettings(ByVal SiteId As Integer, ByVal alertList As String, ByVal emailList As String, ByVal phnoList As String, ByVal alertType As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateAlertSettings(SiteId, alertList, emailList, phnoList, alertType, g_UserAPI)
                dt = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" GetUserReportSchedule : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetUserReportSchedule                                                                                   '
        '   DESCRIPTION     :   Configures the user for Email
        '   PARAM           :   none             	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetUserReportSchedule(ByVal SiteId As Integer) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.GetUserReportSchedule(SiteId, g_UserAPI)
                dt = GetUserReportXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" GetUserReportSchedule : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetUserReportSchedule                                                                                   '
        '   DESCRIPTION     :   Configures the user for Email
        '   PARAM           :   none             	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetAlertSettings(ByVal SiteId As Integer) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.GetAlertSettings(SiteId, g_UserAPI)
                dt = GetAlertSettingsXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" GetUserReportSchedule : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadDeviceListPrintPage                                                                               '
        '   DESCRIPTION     :   Used to List specific site tag list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadDeviceListPrintPage(ByRef dtTag As DataTable, ByRef dtMonitor As DataTable, ByVal SiteId As String, ByVal devicetype As String, ByVal typeId As String, ByVal DeviceId As String,
                                         ByVal CurPage As String, ByVal AlertId As String, ByVal bin As String, Optional ByVal sorclname As String = "TagId",
                                         Optional ByVal sortOrder As String = "ASC", Optional ByVal apiKey As String = "", Optional ByRef bNoRecordFound As Boolean = False) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                If apiKey <> "" Then
                    sStrXml = API.DeviceList(SiteId, devicetype, typeId, DeviceId, CurPage, AlertId, apiKey, bin, sorclname, sortOrder)
                    SiteDeviceListPrint_XMLNodetoDataTable(sStrXml, dtTag, dtMonitor, bNoRecordFound)
                Else
                    sStrXml = API.DeviceList(SiteId, devicetype, typeId, DeviceId, CurPage, AlertId, g_UserAPI, bin, sorclname, sortOrder)
                    SiteDeviceListPrint_XMLNodetoDataTable(sStrXml, dtTag, dtMonitor, bNoRecordFound)
                End If

            Catch ex As Exception
                WriteLog(" LoadDeviceListPrintPage : " & ex.Message.ToString())
            End Try

            If bin = "4" And (g_UserRole = enumUserRole.Customer Or g_UserRole = enumUserRole.Maintenance) Then
                    If Not dtTag Is Nothing Then
                        If dtTag.Rows.Count > 0 Then
                            dtTag.DefaultView.RowFilter = "offline=0 Or (offline=1 And CatastrophicCases in(1,2))"
                            dtTag = dtTag.DefaultView.ToTable
                        End If
                    End If

            End If

            Return dt
        End Function

        Function LoadAllDeviceListPrintPage(ByRef dtTag As DataTable, ByRef dtMonitor As DataTable, ByRef dtStar As DataTable, ByVal SiteId As String, ByVal devicetype As String,
                                            ByVal typeId As String, ByVal DeviceId As String, ByVal CurPage As String, ByVal AlertId As String, ByVal bin As String,
                                            Optional ByVal sorclname As String = "TagId", Optional ByVal sortOrder As String = "ASC", Optional ByVal apiKey As String = "",
                                            Optional ByRef bNoRecordFound As Boolean = False) As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Try

                If apiKey <> "" Then
                    sStrXml = API.getAllDevicesforSite(SiteId, apiKey)
                    SiteAllDeviceListPrint_XMLNodetoDataTable(sStrXml, dtTag, dtMonitor, dtStar, bNoRecordFound)
                Else
                    sStrXml = API.getAllDevicesforSite(SiteId, g_UserAPI)
                    SiteAllDeviceListPrint_XMLNodetoDataTable(sStrXml, dtTag, dtMonitor, dtStar, bNoRecordFound)
                End If
            Catch ex As Exception
                WriteLog(" LoadAllDeviceListPrintPage : " & ex.Message.ToString())
            End Try

            Return dt
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadPurchaseOrderView                                                                               '
        '   DESCRIPTION     :   Used to List specific site tag list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadPurchaseOrderView(ByVal SiteId As Integer, ByVal CurrPage As String, ByVal sortPO As String, ByVal PONo As String, ByVal ModelItem As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.PurchaseOrder(SiteId, CurrPage, sortPO, PONo, ModelItem, g_UserAPI)
                dt = GetPurchaseOrderXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadPurchaseOrderView : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadPurchaseDetailsView                                                                               '
        '   DESCRIPTION     :   Used to List specific site tag list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadPurchaseDetailsView(ByVal PONo As String, ByVal CurrPage As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.PurchaseDetail(PONo, CurrPage, g_UserAPI)
                dt = GetPurchaseDetailXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadPurchaseDetailsView : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadPurchaseSummaryView                                                                               '
        '   DESCRIPTION     :   Used to List specific site tag list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadPurchaseSummaryView(ByVal SiteId As Integer, ByVal CurrPage As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.PurchaseSummary(SiteId, g_UserAPI)
                dt = GetPurchaseSummaryXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadPurchaseSummaryView : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadLocalAlertsForTableView                                                                                   '
        '   DESCRIPTION     :   Configures the user for Email
        '   PARAM           :   none             	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadLocalAlertsForTableView(ByVal SiteId As Integer, ByVal CurrPage As String, ByVal SortAlerts As String, ByVal StartDate As String, ByVal EndDate As String,
                                             ByVal AlertIds As String, ByVal DeviceId As String) As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.LoadLocalAlertsForTableView(SiteId, CurrPage, SortAlerts, StartDate, EndDate, AlertIds, DeviceId, g_UserAPI)
                dt = GetLocalAlertsForTableViewXMLToTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadLocalAlertsForTableView : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LocationChageEvent                                                                                   '
        '   DESCRIPTION     :   Used to show Location Chage Event data
        '   PARAM           :   none             	                                                                       ' 
        '   RETURN TYPE     :   Returns the result
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LocationChageEvent(ByVal SiteId As String, ByVal Tagids As String, ByVal CurrentRoom As String, ByVal LastRoom As String, ByVal PageSize As String, ByVal CurPage As String,
                                    ByVal IsLiveData As String, ByVal LastFetchedDateTime As String, ByVal NeedAll As String, ByVal Sorting As String, ByVal EnteredOnFromDate As String,
                                    ByVal EnteredOnToDate As String, ByVal LeftOnFromDate As String, ByVal LeftOnToDate As String) As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.LocationChageEvent(SiteId, Tagids, CurrentRoom, LastRoom, PageSize, CurPage, IsLiveData, LastFetchedDateTime, NeedAll, Sorting, EnteredOnFromDate, EnteredOnToDate, LeftOnFromDate, LeftOnToDate, g_UserAPI)
                dt = GetLocationChangeEvent_XMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LocationChageEvent : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadMapListView                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadMapListView(ByVal SiteId As Integer) As DataSet
	
            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.MapView(SiteId, g_UserAPI)
                ds = GetMapXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadMapListView : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AddEditDeleteMap                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddEditDeleteMap(ByVal MapType As String, ByVal SiteId As String, ByVal MapName As String, ByVal MapDescription As String, ByVal MapId As String, ByVal MapBuildingId As String,
                                  ByVal MapFloorId As String, ByVal mapUnitId As String, ByVal isDelete As String, ByVal floorSqft As String, ByVal Widthft As String) As DataTable
            
            Dim dt As New DataTable
            Dim sStrXml As XmlNode = Nothing

            Try
	    
                If MapType = enumMapType.Campus Then
                    sStrXml = API.AddCampus(SiteId, MapName, MapDescription, MapId, isDelete, g_UserAPI)
                ElseIf MapType = enumMapType.Building Then
                    sStrXml = API.AddBuilding(SiteId, MapName, MapDescription, MapId, MapBuildingId, isDelete, g_UserAPI)
                ElseIf MapType = enumMapType.Floor Then
                    sStrXml = API.AddFloor(SiteId, MapName, MapDescription, MapId, MapBuildingId, MapFloorId, isDelete, floorSqft, Widthft, g_UserAPI)
                ElseIf MapType = enumMapType.Unit Then
                    sStrXml = API.AddUnit(SiteId, MapName, MapDescription, MapId, MapBuildingId, MapFloorId, mapUnitId, isDelete, g_UserAPI)
                End If

                dt = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" AddEditDeleteMap : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   UploadFile                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UploadMetaFile(ByVal SiteId As Integer, ByVal file As Byte(), ByVal filename As String, ByVal source As String, ByVal sourceType As String, ByVal isEdit As String) As Boolean
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UploadFile(SiteId, file, filename, source, sourceType, isEdit, g_UserAPI)
		
            Catch ex As Exception
                WriteLog(" UploadFile : " & ex.Message.ToString())
                Return False
            End Try

            Return True
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   UpdateMetaInfoForRooms                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateMetaInfoForRooms(ByVal SiteId As String, ByVal CampusId As String, ByVal BuildingId As String, ByVal FloorId As String, ByVal UnitId As String, ByVal file As Byte(), ByVal filename As String) As DataSet
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateMetaInfoForRooms(SiteId, CampusId, BuildingId, FloorId, UnitId, file, filename, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" UpdateMetaInfoForRooms : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadRoomListView                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadRoomListView(ByVal SiteId As Integer, ByVal mapUnitId As String, ByVal SorColumn As String, ByVal SOrtOrder As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.RoomView(SiteId, mapUnitId, SorColumn, SOrtOrder, g_UserAPI)
                dt = GetRoomXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadMapListView : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadTagListView                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadTagListView(ByVal SiteId As Integer, ByVal PageSize As String, ByVal CurrPage As String, ByVal TagMetaIds As String, ByVal Sorting As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.TagView(SiteId, PageSize, CurrPage, TagMetaIds, Sorting, g_UserAPI)
                dt = GetTagXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadMapListView : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   UpdateMetaInfoForTags                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateMetaInfoForTags(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateMetaInfoForTags(SiteId, file, fileName, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" UpdateMetaInfoForRooms : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   UpdateMetaInfoForDevices                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateMetaInfoForDevices(ByVal SiteId As String, ByVal FloorId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateMetaInfoForDevices(SiteId, FloorId, file, fileName, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" UpdateMetaInfoForDevices : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadInfrastructureMetaInfoForFloor                                                                            '
        '   DESCRIPTION     :   Used to List specific Floor details                                                         '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadInfrastructureMetaInfoForFloor(ByVal SiteId As Integer, ByVal FloorId As String) As DataSet
	
            Dim dt As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.GetInfrastructureMetaInfoForFloor(SiteId, FloorId, g_UserAPI)
                dt = GetInfrastructureMetaInfoForFloorXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadInfrastructureMetaInfoForFloor : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadTagMetaInfoForFloor                                                                            '
        '   DESCRIPTION     :   Used to List specific Floor details for Tag                                                       '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadTagMetaInfoForFloor(ByVal SiteId As Integer, ByVal FloorId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.GetTagMetaInfoForFloor(SiteId, FloorId, g_UserAPI)
                dt = GetTagMetaInfoForFloorXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadTagMetaInfoForFloor : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function SaveMonitor(ByVal siteId As String, ByVal MetaInfoForFloorId As String, ByVal MapMonitorId As String, ByVal Location As String, ByVal Notes As String,
                             ByVal isHallway As String, ByVal monitorX As String, ByVal monitorY As String, ByVal monitorW As String, ByVal monitorH As String,
                             ByVal polygonPoints As String, ByVal uMode As String, ByVal dsvgId As String, ByVal svgDType As String, ByVal oldDeviceId As String,
                             ByVal UnitName As String, ByVal Xaxis As String, ByVal Yaxis As String, ByVal RoomXaxis As String, ByVal RoomYaxis As String, ByVal WidthFt As String,
                             ByVal LengthFt As String, ByVal MonitorType As String) As DataTable
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateMetaInfoForMonitor(siteId, MetaInfoForFloorId, MapMonitorId, Location, Notes, isHallway, monitorX, monitorY, monitorW, monitorH,
                                                       polygonPoints, uMode, dsvgId, svgDType, oldDeviceId, UnitName, Xaxis, Yaxis, RoomXaxis, RoomYaxis, WidthFt, LengthFt, MonitorType, g_UserAPI)
                dt = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" SaveMonitor : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function UpdateProfiles(ByVal OldPassword As String, ByVal NewPassWord As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.UpdateProfiles(OldPassword, NewPassWord, g_UserAPI)
                dt = GetResponseToTable(sStrXml)
            Catch ex As Exception
                WriteLog(" UpdateProfiles : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

         Function ChangePasswordForUser(ByVal UserId As String, ByVal OldPassword As String, ByVal NewPassWord As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.ChangePasswordInfo(UserId, OldPassword, NewPassWord, g_UserAPI)
                dt = GetResponseToTable(sStrXml)
            Catch ex As Exception
                WriteLog(" ChangePassword : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   UpdateAnnouncements                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateAnnouncements(ByVal AnnouncementId As String, ByVal Message As String, ByVal StartDate As String, ByVal EndDate As String,
                                     ByVal IsShow As String, ByVal UserRoles As String, ByVal UserAssociatedViews As String, ByVal HTMLMessage As String,
                                     ByVal IsActive As String, ByVal ShowIn As String, ByVal IsDispAfterExpire As String, ByVal daysDispAfterExpire As String, ByVal IsDispHistory As String) As DataTable
	    	Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.UpdateAnnouncements(AnnouncementId, Message, StartDate, EndDate, IsShow, UserRoles, UserAssociatedViews, HTMLMessage, IsActive, ShowIn, IsDispAfterExpire, daysDispAfterExpire, IsDispHistory, g_UserAPI)
                dt = GetResponseToTable(sStrXml)
            Catch ex As Exception
                WriteLog(" UpdateAnnouncements : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetAnnouncements                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetAnnouncements(ByVal StartDate As String, ByVal EndDate As String, ByVal IsLive As String, ByVal curpage As String, Optional ByVal ListForAdmin As Integer = 0) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                If ListForAdmin = 1 Then
                    sStrXml = API.GetAnnouncements(StartDate, EndDate, IsLive, curpage, "", g_UserAPI)
                Else
                    sStrXml = API.GetAnnouncements(StartDate, EndDate, IsLive, curpage, g_UserRole, g_UserAPI)
                End If

                ds = GetAnnouncementsXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetAnnouncements : " & ex.Message.ToString())
            End Try

            Return ds

        End Function
	
        Function GetPastAnnouncements() As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetPastAnnouncements(g_UserRole, g_UserAPI)
                ds = GetPastAnnouncementsXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetPastAnnouncements : " & ex.Message.ToString())
            End Try

            Return ds

        End Function
	
        Function AddThresHoldTime(ByVal TTime As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AddThresHoldTime(TTime, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddThresHoldTime : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
        Function GetHTMLAnnouncement(ByVal AnnouncementId As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetHTMLAnnouncements(AnnouncementId, g_UserAPI)
                ds = GetHTMLAnnouncementsXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetAnnouncements : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetAnnouncements                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function SearchDeviceMap(ByVal SiteId As String, ByVal SearchId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.SearchDeviceMap(SiteId, SearchId, g_UserAPI)
                dt = GetSearchDeviceMapXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetAnnouncements : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetForgotPassword                                                                            '
        '   DESCRIPTION     :   Used to get User details                                                       '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetForgotPassword(ByVal UserName As String) As XmlNode

            Dim dt As New DataTable

            Dim sStrXml As XmlNode

            Try
                sStrXml = API.ForgotPassword(UserName)

            Catch ex As Exception
                WriteLog(" GetForgotPassword : " & ex.Message.ToString())
            End Try

            Return sStrXml

        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetUserNameByResetKey                                                                            '
        '   DESCRIPTION     :   Used to UserName  using  ResetKey                                                    '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetUserNameByResetKey(ByVal ResetKey As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UserNameByResetKey(ResetKey)
                dt = GetResponseText_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetUserNameByResetKey : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   SetUpdatePassword                                                                            '
        '   DESCRIPTION     :   Used to Update Password   using  ResetKey                                                    '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function SetUpdatePassword(ByVal ResetKey As String, ByVal Password As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UpdatePassword(ResetKey, Password)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" SetUpdatePassword : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AddEditDeleteDevice                                                                            '
        '   DESCRIPTION     :   Used to List specific site tag list                                                        '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddEditDeleteDevice(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal IsDelete As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AddOrRemoveMasterList(SiteId, DeviceType, DeviceId, IsDelete, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddEditDeleteDevice : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetReportsForMap(ByVal SiteId As String, ByVal Floorid As String, ByVal reportType As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetReportsForMap(SiteId, Floorid, reportType, g_UserAPI)
                dt = GetMapReportResponseXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetStarDensityReport : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetReportFloorView(ByVal SiteId As String, ByVal DeviceType As String, ByVal Bin As String, ByVal CampusId As String, ByVal BuildingId As String, ByVal FloorId As String, ByVal FilterCriteria As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode = Nothing

            Try

                sStrXml = API.GetDeviceLBIReportForMap(SiteId, DeviceType, Bin, CampusId, BuildingId, "", FilterCriteria, g_UserAPI)
                dt = GetReportFloorViewXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReportFloorView : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UpdateBatteryReplaceForSite(ByVal SiteId As String, ByVal TagId As String, ByVal ReDate As String, ByVal Comments As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UpdateBatteryReplace(SiteId, TagId, ReDate, Comments, g_UserAPI)
                dt = GetResponseToTable(sStrXml)

            Catch ex As Exception
                WriteLog(" UpdateBatteryReplaceForSite : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
      
        Function LoadAssetDeviceList(ByVal SiteId As String, ByVal CampusId As String, ByVal BuildingId As String, ByVal FloorId As String, ByVal UnitId As String,
                                     Optional ByVal sorclname As String = "TagId", Optional ByVal sortOrder As String = "ASC",
                                     Optional ByVal CurPage As String = "0", Optional ByVal DeviceId As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AssetDeviceList(SiteId, CampusId, BuildingId, FloorId, UnitId, sorclname, sortOrder, CurPage, DeviceId, g_UserAPI)
                dt = GetAssetTagMetaInfoForFloorXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadAssetDeviceList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function LoadAssetReportList(ByVal SiteId As String, ByVal DeviceId As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AssetReportList(SiteId, DeviceId, FromDate, ToDate, g_UserAPI)
                dt = GetAssetTagMetaInfoForFloorXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadAssetReportList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
       
        Function LoadExcelAssetReportList(ByVal SiteId As String, ByVal DeviceId As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AssetReportList(SiteId, DeviceId, FromDate, ToDate, g_UserAPI)
                dt = GetExportExcelAssetTagMetaInfoForFloorXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadExcelAssetReportList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function LoadBatterySummaryList(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal CampusId As String, ByVal BuildId As String, ByVal FloorId As String, ByVal UnitId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.BatterySummaryList(SiteId, DeviceType, TypeId, CampusId, BuildId, FloorId, UnitId, g_UserAPI)
                dt = GetBatterySummaryXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadBatterySummaryList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function LoadBatteryList(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal CampusId As String, ByVal BuildId As String,
                                 ByVal FloorId As String, ByVal UnitId As String, Optional ByVal CurPage As String = "0", Optional ByVal sorclname As String = "TagId",
                                 Optional ByVal sortOrder As String = "ASC", Optional ByVal DeviceId As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetBatteryList(SiteId, DeviceType, TypeId, CampusId, BuildId, FloorId, UnitId, CurPage, sorclname, sortOrder, DeviceId, g_UserAPI)

                If DeviceType = enumDeviceType.Tag Then
                    dt = GetTagBatteryListXMLtoTable(sStrXml)
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    dt = GetInfraBatteryListXMLtoTable(sStrXml)
                End If

            Catch ex As Exception
                WriteLog(" LoadBatteryList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function LoadExcelBatteryList(ByVal SiteId As String, ByVal DeviceType As String, ByVal TypeId As String, ByVal CampusId As String, ByVal BuildId As String,
                                      ByVal FloorId As String, ByVal UnitId As String, Optional ByVal CurPage As String = "0", Optional ByVal sorclname As String = "TagId",
                                      Optional ByVal sortOrder As String = "ASC", Optional ByVal DeviceId As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetBatteryList(SiteId, DeviceType, TypeId, CampusId, BuildId, FloorId, UnitId, CurPage, sorclname, sortOrder, DeviceId, g_UserAPI)

                If DeviceType = enumDeviceType.Tag Then
                    dt = GetTagBatteryListXMLtoTable(sStrXml)
                ElseIf DeviceType = enumDeviceType.Monitor Then
                    dt = GetInfraBatteryListXMLtoTable(sStrXml)
                End If

            Catch ex As Exception
                WriteLog(" LoadBatteryList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
        Function LoadLbiListForBatteryTech(ByVal SiteId As String, ByVal SortColumn As String, ByVal SortOrder As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.GetLbiListForBatteryTech(SiteId, SortColumn, SortOrder, g_UserAPI)
                dt = GetTagLbiListXMLtoTableForBatteryTech(sStrXml)

            Catch ex As Exception
                WriteLog("LoadLbiListForBatteryTech : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function
	
        Function UpdateLbiListForBatteryTech(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateLbiForBatteryTech(SiteId, file, fileName, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" UpdateMetaInfoForRooms : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function
	
        Function LoadTagReportForBatteryTech(ByVal SiteId As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.GetTagReportForBatteryTech(SiteId, FromDate, ToDate, g_UserAPI)
                dt = GetTagReportXMLtoTableForBatteryTech(sStrXml)

            Catch ex As Exception
                WriteLog("LoadTagReportForBatteryTech : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function

        Function UpdateFloorForBatteryTech(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.UpdateFloorInfoBatteryTech(SiteId, file, fileName, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
            Catch ex As Exception
                WriteLog("UpdateFloorForBatteryTech : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        Function GetIniHistoryInfo(ByVal SiteId As String, ByVal DeviceType As String, ByVal IDate As String, ByVal SortColumn As String, ByVal SortOrder As String, ByVal DeviceIds As String, ByVal CurPage As String) As DataSet

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Dim ds As New DataSet

            Try

                sStrXml = API.GetIniHistoryInfo(SiteId, DeviceType, IDate, SortColumn, SortOrder, DeviceIds, CurPage, g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

            Catch ex As Exception
                WriteLog("GetIniHistoryInfo : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        Function GetIniProfileInfoForSite(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Dim ds As New DataSet

            Try

                sStrXml = API.GetIniHistoryProfileInfo(SiteId, DeviceType, DeviceId, g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

                If Not ds Is Nothing Then
                    dt = ds.Tables(0)
                End If

            Catch ex As Exception
                WriteLog("GetIniProfileInfoForSite : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function GetSUPTTagInfoForSite(ByVal SiteId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Dim ds As New DataSet

            Try

                sStrXml = API.GetSUPTTagDiedReportInfo(SiteId, g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then
                        dt = ds.Tables(0)
                    End If
                End If

            Catch ex As Exception
                WriteLog("GetSUPTTagInfoForSite : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
	
        Function UpdateAnnualCalibration(ByVal SiteId As String, ByVal DeviceId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.UpdateAnnualCalibrationForSite(SiteId, DeviceId, g_UserAPI)
                dt = SiteDeviceListtag_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("UpdateAnnualCalibration : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
	
#Region "-------------------------------------ASSET TRACKING----------------------------------------"

        Function Asset_AddDeviceInfo(ByVal SiteId As String, ByVal DeviceId As String, ByVal Name As String) As DataTable

            Dim ds As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.Asset_AddDevice(SiteId, DeviceId, Name, g_UserAPI)
                ds = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                WriteLog("Asset_AddDeviceInfo : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        Function Asset_GetSavedSearches(ByVal SiteId As String, ByVal DeviceId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.Asset_GetSavedSearchesForUser(SiteId, DeviceId, g_UserAPI)

                Dim theDataSet As New DataSet
                theDataSet.ReadXml(New XmlNodeReader(sStrXml))

                dt = theDataSet.Tables(0)

            Catch ex As Exception
                WriteLog("Asset_GetSavedSearches : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadAssetDeviceDetailsForSearch                                                                            '
        '   DESCRIPTION     :                                                           '   
        '   PARAM           :   			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadAssetDeviceDetailsForSearch(ByVal SiteId As Integer, ByVal SaveId As String, ByVal SearchDeviceIDs As String) As DataSet
            
	    Dim dt As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.GetAssetDeviceDetailsForSearch(SiteId, SaveId, SearchDeviceIDs, g_UserAPI)
                dt = GetAssetDeviceDetailsForSearchXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" LoadAssetDeviceDetailsForSearch : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function Asset_EditDeviceInfo(ByVal SaveId As String, ByVal SiteId As String, ByVal DeviceId As String, ByVal Name As String, ByVal IsDelete As String) As DataTable

            Dim ds As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.Asset_EditDevice(SaveId, SiteId, DeviceId, Name, IsDelete, g_UserAPI)
                ds = GetResponseToTable(sStrXml)
		
            Catch ex As Exception
                WriteLog("Asset_EditDeviceInfo : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

#End Region
	 
        Function UploadRTLSDetails(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet
           
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UploadRTLSDetail(SiteId, file, fileName, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog("UpdateFloorForBatteryTech : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function

        Function ExportI2AssetMetaData(ByVal SiteId As String, Optional ByVal apiKey As String = "") As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode
            
	    Try

                sStrXml = API.ExportI2AssetMetaData(SiteId, apiKey)
                dt = ExportI2AssetMetaData_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ExportI2AssetMetaData : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
	
        Function UpdateRoleMappingInfo(ByVal EnableActiveDirectoryRoles As String, ByVal SSOIAttribute As String, ByVal Roles As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateMangeRoles(EnableActiveDirectoryRoles, SSOIAttribute, Roles, g_UserAPI)
                dt = GetResponseToTable(sStrXml)

            Catch ex As Exception
                WriteLog("UpdateRoleMappingInfo : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function GetMangeRoleMappingInfo() As DataSet
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Dim ds As New DataSet

            Try
                sStrXml = API.GetMangeRolesInfo()

                ds.ReadXml(New XmlNodeReader(sStrXml))

            Catch ex As Exception
                WriteLog("GetMangeRoleMappingInfo : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function
	
        Function AddADDirectory(ByVal ServerIp As String, ByVal UserName As String, ByVal Password As String) As DataTable
            
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.UpdateADDirectory(ServerIp, UserName, Password, g_UserAPI)
                dt = GetResponseToTable(sStrXml)

            Catch ex As Exception
                WriteLog("AddADDirectory : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
	
        Function GetADDirectory() As DataSet

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Dim ds As New DataSet

            Try
	    
                sStrXml = API.GetADDirectory(g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

            Catch ex As Exception
                WriteLog("GetADDirectory : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function loadserveripList(Optional ByVal cid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getServerIPList(g_UserAPI, cid)
                dt = serverIPList_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   GetPasswordInfo                                                                               '
        '   DESCRIPTION     :   Used to Get PasswordInfo                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function GetPasswordInfo(ByVal UserId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.GetPasswordInfo(UserId, g_UserAPI)
                dt = Password_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetPasswordInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                               '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function loadcompanyList(Optional ByVal cid As String = "") As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Try
                sStrXml = API.getCompanyList(g_UserAPI, cid)
                dt = comanyList_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try
            Return dt
        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function loadusertypeList(Optional ByVal cid As String = "") As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Try
                sStrXml = API.getUserTypeList(g_UserAPI, cid)
                dt = usertypeList_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try
            Return dt
        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   loadsiteInfo                                                                                   '
        '   DESCRIPTION     :   Used to List All Site                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function loaduserroleList(Optional ByVal cid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getUserRoleList(g_UserAPI, cid)
                dt = userroleList_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function AddSite(ByVal Masterid As String, ByVal SiteId As String, ByVal IsAdd As String, ByVal CompanyId As String, ByVal SiteName As String, ByVal SiteFolder As String, ByVal FileFormat As String,
                         ByVal Status As String, ByVal IsGroup As String, ByVal IsGroupMember As String, ByVal ServerIP As String, ByVal AuthPassword As String, ByVal LocationCode As String,
                         ByVal QBN As String, ByVal IsPrismView As String, ByVal TimeZone As String, ByVal IsUnDeleteTags As String, ByVal IsUnDeleteMonitors As String,
                         ByVal IsDefinedTagsinCore As String, ByVal IsDefinedInfrastructureinCore As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Dim IPAddress As String = System.Web.HttpContext.Current.Request.UserHostAddress
	    
            If IPAddress = "127.0.0.1" Then
                IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
            End If
	    
            Try

                sStrXml = API.SetupSite(Masterid, SiteId, IsAdd, CompanyId, SiteName, SiteFolder, FileFormat, Status, IsGroup, IsGroupMember, ServerIP, AuthPassword,
                                        LocationCode, QBN, IsPrismView, TimeZone, IsUnDeleteTags, IsUnDeleteMonitors, IsDefinedTagsinCore, IsDefinedInfrastructureinCore, IPAddress, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddSite : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
       
        Function AddUser(ByVal UserId As String, ByVal IsAdd As String, ByVal CompanyId As String, ByVal UserName As String, ByVal Password As String, ByVal Status As String, ByVal UserType As String,
                         ByVal Email As String, ByVal Batteryreplacement As String, ByVal UserTypeId As String, ByVal UserRoleId As String, ByVal AssociatedEmail As String, ByVal AssociatedPhone As String,
                         ByVal AuthPassword As String, ByVal IsTempMonitoring As String, ByVal IsPrismView As String, ByVal IsPrismAuditView As String, ByVal AllowAccessForStar As String, ByVal AssetNotification As String,
                         ByVal DesktopNotification As String, ByVal IsPrismReadOnly As String, ByVal AllowAccessForKPI As String, ByVal isPulseReport As String, ByVal PulseReportIds As String,
                         ByVal FirstName As String, ByVal LastName As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.SetupUser(UserId, IsAdd, CompanyId, UserName, Password, Status, UserType, Email, Batteryreplacement, UserTypeId, UserRoleId, AssociatedEmail, AssociatedPhone,
                                        AuthPassword, IsTempMonitoring, IsPrismView, IsPrismAuditView, AllowAccessForStar, AssetNotification, DesktopNotification, IsPrismReadOnly,
                                        AllowAccessForKPI, isPulseReport, PulseReportIds, FirstName, LastName, g_UserAPI)

                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddUser : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   AddSitealert                                                                              '
        '   DESCRIPTION     :   Insert User to database.
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddSiteAlert(ByVal InfrAlertIds As String, ByVal InfrAlertSiteIds As String, ByVal ReportIds As String, ByVal ReportISiteIds As String, ByVal Email As String, ByVal AuthenticationKey As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
                sStrXml = API.SetUpSiteAlert(InfrAlertIds, InfrAlertSiteIds, ReportIds, ReportISiteIds, Email, AuthenticationKey)
                dt = XMLNodeToDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" AddUser : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ListCompanies                                                                                   '
        '   DESCRIPTION     :   Retrive Company List.
        '   PARAM           :   No Input		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Function ListCompanies() As DataTable

            Dim dtCompanies As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getCompanyList(g_UserAPI)
                dtCompanies = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListCompanies : " & ex.Message.ToString())
            End Try

            Return dtCompanies

        End Function

        '   NAME            :   AddCompany                                                                               '
        '   DESCRIPTION     :   Insert Company to database.
        '   PARAM           :   SiteId, IsAdd, CompanyId, SiteName, SiteFolder, FileFormat, Status, GMTOffset, IsGroup, IsGroupMember, ServerIP, DatabaseName, AuthUserId, AuthPassword, Chanel, LocationCode, QBN	                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddCompany(ByVal IsAdd As String, ByVal CompanyId As String, ByVal CompanyName As String, ByVal Status As String, ByVal AuthPassword As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.SetupCompany(IsAdd, CompanyId, CompanyName, Status, AuthPassword, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddCompany : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        '   NAME            :   Add Page VisitDetails                                                                               '
        '   DESCRIPTION     :   Insert Page VisitDetails to database.
        '   PARAM           :   SiteId, IsAdd, CompanyId, SiteName, SiteFolder, FileFormat, Status, GMTOffset, IsGroup, IsGroupMember, ServerIP, DatabaseName, AuthUserId, AuthPassword, Chanel, LocationCode, QBN	                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function AddPageVisitDetails(ByVal UserId As String, ByVal PageName As String, ByVal nPageAction As String, ByVal PageActionHistory As String, ByVal IPAddress As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.PageVisitDetails(UserId, PageName, nPageAction, PageActionHistory, IPAddress, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddPageVisitDetails : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ListSite                                                                                   '
        '   DESCRIPTION     :   Retrive Site List.
        '   PARAM           :   No Input		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ListSite() As DataTable

            Dim dtSites As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getSiteList(g_UserAPI)
                dtSites = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" SitesList : " & ex.Message.ToString())
            End Try
	    
            Return dtSites
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   CampusMapList                                                                                   '
        '   DESCRIPTION     :   Retrive Map List.
        '   PARAM           :   No Input		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function CampusMapList(ByVal siteId As String) As DataTable
	
            Dim dtSites As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.getCampusList(siteId, g_UserAPI)
                dtSites = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("CampusMapList : " & ex.Message.ToString())
            End Try
	    
            Return dtSites
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   DeleteCampusMap                                                                                   '
        '   DESCRIPTION     :   Deletes the Campus Map
        '   PARAM           :   SiteId, DataId	                                                                       ' 
        '   RETURN TYPE     :   Returns the email List
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function DeleteCampusMap(ByVal siteId As String, ByVal DataId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.deleteCampusMap(g_UserAPI, siteId, DataId)
                dt = XMLNodeToDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" DeleteCampusMap : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   DeleteSiteConfiguration                                                                                   '
        '   DESCRIPTION     :   Deletes the Site configuration
        '   PARAM           :   SiteId, UserId	                                                                       ' 
        '   RETURN TYPE     :   Returns the email List
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function DeleteSiteConfiguration(ByVal siteId As String, ByVal UserId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.deleteSiteConfiguration(g_UserAPI, siteId, UserId)

                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" DeleteSiteConfiguration : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ListUser                                                                                   '
        '   DESCRIPTION     :   Retrive  User List.
        '   PARAM           :   CurPage		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ListUser(ByVal siteId As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getUserList(siteId, g_UserAPI)
                ds = XMLNodeToDataSet(sStrXml)

            Catch ex As Exception
                WriteLog(" UserList : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadUser                                                                                   '
        '   DESCRIPTION     :   Retrive  User List.
        '   PARAM           :   NA		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadUser() As DataTable

            Dim dtUserList As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.LoadUser(g_UserAPI)
                dtUserList = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ActiveList : " & ex.Message.ToString())
            End Try

            Return dtUserList

        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   UserActivityLog                                                                                   '
        '   DESCRIPTION     :   Retrive  User Activity log.
        '   PARAM           :   UserId, UpdatedOn		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function UserActivityLog(ByVal UserId As Integer, ByVal FromDate As String, ByVal EventType As String, ByVal ToDate As String) As DataTable

            Dim dtActivity As New DataTable

            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getUserActivityLog(g_UserAPI, UserId, FromDate, EventType, ToDate)
                dtActivity = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" UserActivityLog : " & ex.Message.ToString())
            End Try

            Return dtActivity
	    
        End Function

        Function PageVisitDetails(ByVal UserId As String, ByVal PageName As String, ByVal nPageAction As String, ByVal PageActionHistory As String) As DataTable

            Dim dt As New DataTable
            Dim IPAddress As String = System.Web.HttpContext.Current.Request.UserHostAddress

            If IPAddress = "127.0.0.1" Then
                IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString()
            End If

            dt = AddPageVisitDetails(UserId, PageName, nPageAction, PageActionHistory, IPAddress)

            Return dt

        End Function

        Function GetSitesForUser(ByVal UserId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetAvailableSitesForUser(g_UserAPI, UserId)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetSitesForUser : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function EditSitesForUser(ByVal siteId As String, ByVal UserId As String, ByVal isAdd As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.ConfigureSitesForUser(g_UserAPI, siteId, UserId, isAdd)
                dt = XMLNodeToDataTable(sStrXml)
            Catch ex As Exception
                WriteLog(" EditAlertsForEmail : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
   
        Function DeleteUserConfiguration(ByVal UserId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.deleteUserConfiguration(g_UserAPI, UserId)
                dt = XMLNodeToDataTable(sStrXml)
            Catch ex As Exception
                WriteLog(" DeleteUserConfiguration : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function ListLogViewer() As DataTable

            Dim dtLogViewer As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetLogList(g_UserAPI)
                dtLogViewer = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LogList : " & ex.Message.ToString())
            End Try

            Return dtLogViewer

        End Function

        Function GetResponseHistory() As DataTable

            Dim dtResponse As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetResponseHistory(g_UserAPI)
                dtResponse = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetResponseHistory : " & ex.Message.ToString())
            End Try

            Return dtResponse

        End Function

        Function ListThresholdConfiguration() As DataTable

            Dim dtThresholdConfiguration As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetThresholdConfiguration(g_UserAPI)
                dtThresholdConfiguration = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LogList : " & ex.Message.ToString())
            End Try

            Return dtThresholdConfiguration

        End Function

        Function AddThresholdConfiguration(ByVal DataId As String, ByVal IsAdd As String, ByVal SiteDownEmail As String, ByVal SoftwareDownEmail As String, ByVal LowHardDiskMemory As String,
                                           ByVal LastUpdateThreshold As String, ByVal SMSSenderID As String, ByVal SWSMSSenderID As String, ByVal ActiveListFailedCount As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.SetupThresholdConfiguration(DataId, IsAdd, SiteDownEmail, SoftwareDownEmail, LowHardDiskMemory, LastUpdateThreshold, SMSSenderID, SWSMSSenderID, ActiveListFailedCount, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddUser : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        Function Update_TagProfiles(ByVal SiteId As Integer, ByVal DeviceType As String, ByVal TagId As String, ByVal TagCategory As String, _
                                                  ByVal IRProfile As String, ByVal LongIR As String, ByVal IRReportRate As String, ByVal RFReportRate As String, ByVal IRRX As String, _
                                                  ByVal FastPushbutton As String, ByVal LFRange As String, ByVal DisableLF As String, ByVal OperatingMode As String, ByVal WiFiReportingTime As String, _
                                                  ByVal WiFiIn900MHz As String, ByVal PagingProfile As String, ByVal UpdateRate As String, ByVal IsDefaultProfile As String) As DataTable
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.Update_TagProfiles(SiteId, DeviceType, TagId, TagCategory, IRProfile, LongIR, IRReportRate, RFReportRate, IRRX, FastPushbutton, LFRange, DisableLF, OperatingMode, WiFiReportingTime, WiFiIn900MHz, PagingProfile, UpdateRate, IsDefaultProfile, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetQuidBySite : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetDefaultProfiles(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String) As DataTable
           
	    Dim dt As New DataTable
            Dim sStrXml As XmlNode
            Dim ds As New DataSet

            Try
	    
                sStrXml = API.GetDefaultProfileInfo_Site(SiteId, DeviceType, DeviceId, g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then
                        dt = ds.Tables(0)
                    End If
                End If

            Catch ex As Exception
                WriteLog(" GetDefaultProfiles : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function GetCompanyGroup() As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.GetCompanyGroup(g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetCompanyGroup : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        Function AddCompanyGroup(ByVal GroupId As String, ByVal GroupName As String, ByVal CompanyId As String, ByVal IsAdd As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
                sStrXml = API.AddCompanyGroup(GroupId, GroupName, CompanyId, IsAdd, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddSite : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        Function UpdateLocalIdInfo(ByVal SiteId As String, ByVal DeviceType As String, ByVal TagId As String, ByVal LocalId As String, ByVal MonitorId As String, ByVal Location As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UpdateLocalId(SiteId, DeviceType, TagId, LocalId, MonitorId, Location, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("UpdateLocalIdInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function ListSitePendingFiles(ByVal ServerIP As String, ByVal DBName As String) As DataTable

            Dim dtSitePendingFiles As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetSitePendingFiles(ServerIP, DBName, g_UserAPI)
                dtSitePendingFiles = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListSitePendingFiles : " & ex.Message.ToString())
            End Try

            Return dtSitePendingFiles

        End Function

        Function UploadG2SummaryInfo(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UploadG2Summary(SiteId, file, fileName, g_UserAPI)
                ds = GetUploadResultXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" UploadG2Summary : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function
	
        Function GetServerStatus(ByVal Status As String, ByVal Message As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.GetServerAction(Status, Message)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("GetServerAction : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Sub GetServerAction(ByVal Status As String, ByVal Message As String)

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetServerAction(Status, Message)
                dt = XMLNodeToDataTable(sStrXml)

                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then

                        Status = dt.Rows(0).Item("Status")
                        Message = dt.Rows(0).Item("Message")

                        If Status = False Then
                            HttpContext.Current.Response.Redirect("Temporarilydown.htm?Msg=" & Message & "")
                        End If

                    End If
                End If

            Catch ex As Exception
                WriteLog("GetServerAction : " & ex.Message.ToString())
            End Try

        End Sub

        Function DBCleanBuffer() As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.DBCleanBuffer(g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("DBCleanBuffer : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function DownloadLogFile(ByVal SiteId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.DownloadLogFile(SiteId, g_UserAPI)
                dt = siteFolder_XMLNodetoDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" DownloadLogFile : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function GetLogFile(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String) As String

            Dim results As String = ""

            Try
                results = API.GetFiles(strSitePath, strSiteFolder, strLastParsedFile, strFileFormat, g_UserAPI)
            Catch ex As Exception
                WriteLog(" GetLogFile : " & ex.Message.ToString())
            End Try

            Return results
        End Function

        Function GetPCServerLogFile(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String) As String

            Dim results As String = ""

            Try
                results = API.GetPCServerLogFiles(strSitePath, strSiteFolder, strLastParsedFile, g_UserAPI)
            Catch ex As Exception
                WriteLog(" GetPCServerLogFile : " & ex.Message.ToString())
            End Try

            Return results
	    
        End Function

        Function ConfigAutomatedReportEmails(ByVal Emails As String, ByVal aAlertId As Integer) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.ConfigEmailForAutomatedReports(Emails, aAlertId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("ConfigAutomatedReportEmails : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetDeviceSummary(ByVal SiteId As Integer, ByVal DeviceType As Integer, ByVal Days As Integer) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.DeviceSummary(SiteId, DeviceType, Days, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog("GetDeviceSummary : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Sub GetJunkDevices(ByVal SiteId As Integer, ByVal Count As Integer, ByVal Days As Integer, ByRef dtTags As DataTable, ByRef dtMonitors As DataTable)

            Dim sStrXml As XmlNode

            Try

                sStrXml = API.JunkDeviceCount(SiteId, Count, Days, g_UserAPI)

                dtTags = XMLNodeToDataTable(sStrXml, 0)

                dtMonitors = XMLNodeToDataTable(sStrXml, 1)

            Catch ex As Exception
                WriteLog("GetDeviceSummary : " & ex.Message.ToString())
            End Try

        End Sub

        Sub ArchiveJunkDevice(ByVal Siteid As Integer, ByVal DeviceType As Integer)

            Dim sStrXml As XmlNode

            Try
                sStrXml = API.ArchiveJunk(Siteid, DeviceType, g_UserAPI)
            Catch ex As Exception
                WriteLog("ArchiveJunkDevice : " & ex.Message.ToString())
            End Try

        End Sub
	
        Function DeleteCompanyGroup(ByVal GroupId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.DeleteCompanyGroup(g_UserAPI, GroupId)
                dt = XMLNodeToDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" DeleteSiteConfiguration : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
        ''
        Function ListAssociatedSite() As DataTable
	
            Dim dtSites As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.GetSiteADGroup(g_UserAPI)
                dtSites = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" SitesList : " & ex.Message.ToString())
            End Try
	    
            Return dtSites
	    
        End Function
              
        Function DeleteSiteAssociation(ByVal GroupId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.DeleteSiteAssociation(g_UserAPI, GroupId)
                dt = XMLNodeToDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" DeleteSiteAssociation : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function
       '
        Function AddAssociationSiteSetup(ByVal SiteId As String, ByVal GroupName As String, ByVal GroupId As String, ByVal VHAGroupId As String, ByVal IsAdd As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.AssociationSite(SiteId, GroupName, GroupId, VHAGroupId, IsAdd, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddSite : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
        
        Function loadADGroupList(Optional ByVal sid As String = "") As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.getADGroupListInfo(g_UserAPI, sid)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
    
        Function loadEvent() As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getEventType(g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetOnDemandReportsInfo(ByVal SiteId As String, ByVal TypeIds As String, ByVal strDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlElement
            Dim sSearchData As String = ""

            Try

                sStrXml = API.GetOnDemandReports(SiteId, TypeIds, strDate, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetOnDemandReportsInfo : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function ListUpgradeGMSService() As DataTable

            Dim dtUser As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getGMSServiceList(g_UserAPI)
                dtUser = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" UserList : " & ex.Message.ToString())
            End Try

            Return dtUser

        End Function

        Function loadGMSServiceList(Optional ByVal cid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getGMSService(g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function AddGMSService(ByVal ServiceId As String, ByVal Version As String, ByVal fileUrl As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.AddGMSService(ServiceId, Version, fileUrl, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog(" UpdateMetaInfoForRooms : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        Function DeleteParserService(ByVal DataId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.DeleteParserService(g_UserAPI, DataId)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" DeleteUserConfiguration : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function ListUpgradeHistory(ByVal DataId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetUpgradeHistory(DataId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListUpgradeHistory : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function ListRDBServices() As DataTable

            Dim dtRDBService As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetRDBServices(g_UserAPI)
                dtRDBService = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListUpgradeHistory : " & ex.Message.ToString())
            End Try

            Return dtRDBService

        End Function
       
        Function RestartParserService(ByVal ServerIP As String, ByVal DBName As String, ByVal ServiceId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.RestartParserService(g_UserAPI, ServerIP, DBName, ServiceId)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" DeleteUserConfiguration : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
        Function GetDeviceMonitorHourlyInfo(ByVal SiteId As String, ByVal DeviceId As String, ByVal DeviceType As String, ByVal Duration As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetDeviceMonitorHourlyInfo(g_UserAPI, SiteId, DeviceId, DeviceType, Duration)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetGMSBeaconStarSlotDetails(ByVal SiteId As String, ByVal DeviceId As String, ByVal sDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetGMSBeaconStarSlotDetails(g_UserAPI, SiteId, DeviceId, sDate)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetGMSStarSlotDetails(ByVal SiteId As String, ByVal DeviceId As String, ByVal sDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetGMSStarSlotDetails(g_UserAPI, SiteId, DeviceId, sDate)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetGMSMonitorGroupsInfoByDeviceId(ByVal SiteId As String, ByVal DeviceId As String, ByVal sDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetGMSMonitorGroupsInfoByDeviceId(g_UserAPI, SiteId, DeviceId, sDate)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetReports(ByVal SiteId As String, ByVal ReportType As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal FromDate As String, ByVal ToDate As String,
                            ByVal StarId As String, ByVal IsChkTTSyncError As Integer, ByVal IsConnFailed As String, ByVal PagingCount As String, ByVal LocationCount As String,
                            ByVal TriggerCount As String, ByVal FilterCond As String, ByVal LocationFilterCond As String, ByVal TriggerFilterCond As String, ByVal GroupCond As String,
                            ByVal StarsUpgradeMode As String, ByVal StarsTTSyncError As String, ByVal StarsNotReceivingData As String, ByVal StarsNotReceiving24hr As String,
                            ByVal StarsSeenNetworkIssue As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetGMSReportForSite(g_UserAPI, SiteId, ReportType, DeviceType, DeviceId, FromDate, ToDate, StarId, IsChkTTSyncError, IsConnFailed, PagingCount,
                                                  LocationCount, TriggerCount, FilterCond, LocationFilterCond, TriggerFilterCond, GroupCond, StarsUpgradeMode, StarsTTSyncError,
                                                  StarsNotReceivingData, StarsNotReceiving24hr, StarsSeenNetworkIssue)
                ds.ReadXml(New XmlNodeReader(sStrXml))

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        Function GetStarDailyPLCount(ByVal SiteId As String, ByVal sDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetStarDailyPLCount(g_UserAPI, SiteId, sDate)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
        Function GetWPSUILogFile(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String) As String

            Dim results As String = ""

            Try
                results = API.GetWPSUIFiles(strSitePath, strSiteFolder, strLastParsedFile, strFileFormat, g_UserAPI)
            Catch ex As Exception
                WriteLog(" GetWPSUILogFile : " & ex.Message.ToString())
            End Try

            Return results
	    
        End Function
	
        Function GetWPSUISettingsFile(ByVal strSitePath As String, ByVal strSiteFolder As String, ByVal strLastParsedFile As String, ByVal strFileFormat As String) As String

            Dim results As String = ""

            Try
                results = API.GetWPSUISetttingsFiles(strSitePath, strSiteFolder, strLastParsedFile, strFileFormat, g_UserAPI)
            Catch ex As Exception
                WriteLog(" GetWPSUISettingsFile : " & ex.Message.ToString())
            End Try

            Return results
	    
        End Function
	
        Function loadguardiansiteList(Optional ByVal sid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.getGuardianSiteListInfo(g_UserAPI, sid)
                dt = siteList_XMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        Function loadconfiguredguardiansiteList(Optional ByVal sid As String = "") As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.GetConfiguredGuardianSiteList(g_UserAPI, sid)
                dt = configuresiteList_XMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog(" loadsiteList : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        Function UpdateWPSAdminNewPassword(ByVal SiteId As String, ByVal sAdminUserId As String, ByVal sAdminUserName As String, ByVal sAdminUserNewPassword As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UpdateWPSAdminNewPassword(g_UserAPI, SiteId, sAdminUserId, sAdminUserName, sAdminUserNewPassword)
                dt = GetResponseToTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
        Function ConfigureWPSSiteInGMS(ByVal SiteId As String, ByVal sInsert As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.ConfigureWPSSiteInGMS(g_UserAPI, SiteId, sInsert)
                dt = GetResponseToTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
   	
        Function AddSupport(ByVal SptName As String, ByVal SiteId As String, ByVal SptUrl As String, ByVal IsAdd As String, ByVal IsStatus As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.SetupSupport(SptName, SiteId, SptUrl, IsAdd, IsStatus, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddSupport : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function ListSupport(ByVal statval As String) As DataTable

            Dim dtSites As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getSupportList(statval, g_UserAPI)
                dtSites = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListSupport : " & ex.Message.ToString())
            End Try

            Return dtSites
	    
        End Function
	
		''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadDisasterRecovery                                                                               '
        '   DESCRIPTION     :   Used to List specific site star list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadDisasterRecovery(ByVal SiteId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.LoadDisasterRecovery(SiteId, g_UserAPI)

                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadDisasterRecovery : " & ex.Message.ToString())
            End Try
	    
            Return dt

        End Function
	
		''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ImportDisasterRecovery                                                                               '
        '   DESCRIPTION     :   Used to Import specific site star list                                                                      '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   DataSet                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ImportDisasterRecovery(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String) As DataSet
            
	    Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try
	    
                sStrXml = API.ImportDisasterRecovery(SiteId, file, fileName, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)
		
            Catch ex As Exception
                WriteLog("ImportDisasterRecovery : " & ex.Message.ToString())
            End Try

            Return ds
	    
        End Function
	
        Function AddSiteForSettings(ByVal SiteList As String, ByVal Status As String, ByVal FileFormat As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.AddSitesForSettings(SiteList, Status, FileFormat, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" AddSites : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        Function GetSiteForSettings(ByVal Status As String, ByVal FileFormat As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.GETSitesForSettings(Status, FileFormat, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetSites : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        Function GetHealthOverviewReportInfo(ByVal SiteId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetHealthOverviewReport(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetHealthOverviewReportInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function GetHealthDetailReportInfo(ByVal SiteId As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetHealthDetailReport(SiteId, g_UserAPI)
                ds = XMLNodeToDataSet(sStrXml)

            Catch ex As Exception
                WriteLog(" GetHealthDetailReportInfo : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

        Function UpdateDeviceLocation(ByVal SiteId As String, ByVal DeviceType As String, ByVal DeviceId As String, ByVal Location As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try
                sStrXml = API.UpdateDevLocationForSite(SiteId, DeviceType, DeviceId, Location, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" UpdateDeviceLocation : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function Get_CenTrakModelItems(ByVal DeviceType As String, ByVal siteid As String, ByVal IsModelItemOnly As Boolean) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try
	    
                sStrXml = API.Get_CenTrakModelItems(g_UserAPI, DeviceType)
                dt = CentrakDevices_XMLNodetoDataTable(sStrXml)
		
            Catch ex As Exception
                WriteLog("Get_CenTrakModelItems : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

		''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ListStreamingFields                                                                                   '
        '   DESCRIPTION     :   Retrive  Streaming Fields List.
        '   PARAM           :   SiteId		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ListStreamingFields(ByVal SiteId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.getStreamingFieldsList(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListStreamingFields : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function
	
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ListMSESettings                                                                                   '
        '   DESCRIPTION     :   Retrive  MSE Settings List.
        '   PARAM           :   SiteId		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ListMSESettings(ByVal SiteId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.getMSESettingsList(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListMSESettings : " & ex.Message.ToString())
            End Try
	    
            Return dt
	    
        End Function

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   ListMSESettingsHistory                                                                                   '
        '   DESCRIPTION     :   Retrive  MSE Settings History List.
        '   PARAM           :   SiteId		                                                                       ' 
        '   RETURN TYPE     :   DataTable
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function ListMSESettingsHistory(ByVal SiteId As String) As DataTable
	
            Dim dt As New DataTable
            Dim sStrXml As XmlNode
	    
            Try

                sStrXml = API.getMSESettingsHistoryList(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" ListMSESettingsHistory : " & ex.Message.ToString())
            End Try

            Return dt
	    
        End Function

        Function GetOutboundReport(ByVal SiteId As Integer, ByVal nMonth As Integer, ByVal nYear As Integer, ByVal IsDetailReport As Boolean) As DataTable

            Dim dtOutboundata As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetOutboundReport(SiteId, nMonth, nYear, IsDetailReport, g_UserAPI)
                dtOutboundata = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetOutboundReport : " & ex.Message.ToString())
            End Try

            Return dtOutboundata

        End Function 
	
        Function LoadAlertList() As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.getAlertMasterListInfo(g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadAlertList : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        
	Function MonitorGroupReport(ByVal SiteId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.LoadMonitorGroupReport(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadMonitorGroupReport : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	 
        Function UpdateDeviceLocationsForSite(ByVal SiteId As String, ByVal file As Byte(), ByVal fileName As String, ByVal DeviceType As String) As DataSet

            Dim ds As New DataSet
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.UpdateDeviceLocations(SiteId, file, fileName, DeviceType, g_UserAPI)
                ds = GetResultXMLtoTable(sStrXml)

            Catch ex As Exception
                WriteLog("UpdateDeviceLocationsForSite : " & ex.Message.ToString())
            End Try

            Return ds
        End Function

        Function GetDeviceLocationsForSite(ByVal SiteId As String, ByVal DeviceType As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.LoadDeviceLocations(SiteId, DeviceType, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetDeviceLocationsForSite : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
	
#Region "For BatteryReplacementFailureReport"

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   NAME            :   LoadBatteryReplacementFailure                                                              '
        '   DESCRIPTION     :   Get Battery Replacement Failure Device list                                                '   
        '   PARAM           :   No param			                                                                       ' 
        '   RETURN TYPE     :   Datatable                                                                                  '       
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Function LoadBatteryReplacementFailure(ByVal SiteId As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.BatteryReplacementFailure(SiteId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)
            Catch ex As Exception

                WriteLog(" LoadBatteryReplacementFailure : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
#End Region

#Region "For Site Analysis Report"
     
        Function LoadSiteanalysisReport(ByVal SiteId As String, ByVal ReportType As String, ByVal DeviceType As String, ByVal Products As String) As DataSet

            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.SiteAnalysisReport(SiteId, ReportType, DeviceType, Products, g_UserAPI)

                ds = XMLNodeToDataSet(sStrXml)

                If Not ds Is Nothing Then
                    If ds.Tables.Count > 1 Then
                        dt = ds.Tables(1)
                    End If
                End If

            Catch ex As Exception
                WriteLog(" LoadSiteanalysisReport : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

#End Region

#Region "For CenTrak Volt Detail Report"

        Function GetCenTrakVoltDetailReport(ByVal SiteId As String, ByVal DeviceType As String, ByVal FromDate As String, ByVal ToDate As String, ByVal UserId As String) As DataSet

            Dim sStrXml As XmlNode

            Dim dt As New DataTable
            Dim ds As New DataSet

            Try

                sStrXml = API.CenTrakVoltDetailReport(SiteId, DeviceType, FromDate, ToDate, UserId, g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

            Catch ex As Exception
                WriteLog("GetCenTrakVoltDetailReport : " & ex.Message.ToString())
            End Try

            Return ds

        End Function

#End Region

#Region "For Cetani Meta Data"

        Function GetCetaniMetaDataInfo(ByVal SiteId As String, ByVal SearchValue As String, ByVal CurrPage As String, ByVal PageSize As String, ByVal devicetype As String) As DataTable

            Dim sStrXml As XmlNode

            Dim dt As New DataTable
            Dim ds As New DataSet

            Try

                sStrXml = API.GetCetaniMetadataForSite(SiteId, SearchValue, CurrPage, PageSize, devicetype, g_UserAPI)
                ds.ReadXml(New XmlNodeReader(sStrXml))

                If Not ds Is Nothing Then
                    If ds.Tables.Count > 0 Then
                        dt = ds.Tables(0)
                    End If
                End If

            Catch ex As Exception
                WriteLog("GetCetaniMetaDataInfo : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

#End Region

        Function LoadEMActivityReport(ByVal ReportType As Integer, ByVal ReportDate As String, ByVal Email As String,
                                      ByVal SiteId As String, ByVal SubType As String, ByVal isDailyData As String, ByVal TagId As String) As DataTable

            Dim sStrXml As XmlNode
            Dim dt As New DataTable

            Try

                sStrXml = API.GetEMTagActivityReport(ReportType, ReportDate, Email, SiteId, SubType, isDailyData, TagId, g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadEMActivityReport : " & ex.Message.ToString())
            End Try

            Return dt

        End Function

        Function LoadLocationChangeEvent(ByVal SiteId As String, ByVal DeviceId As String, ByVal FromDate As String, ByVal ToDate As String,
                                      ByVal Type As String, ByVal EventTreshold As String, ByVal inMonitorIds As String, ByVal exMonitorIds As String,
                                      ByVal TimeSpend As String, ByVal SpendType As String) As XmlElement

            Dim sStrXml As XmlElement

            Try

                sStrXml = API.GetLocationChangeEvent(SiteId, DeviceId, FromDate, ToDate, Type, EventTreshold, inMonitorIds, exMonitorIds, TimeSpend, SpendType, g_UserAPI)

            Catch ex As Exception
                WriteLog(" Load Location Change Event : " & ex.Message.ToString())
            End Try

            Return sStrXml

        End Function

        Function GetUserPulseReports() As DataTable

            Dim sStrXml As XmlNode
            Dim dt As New DataTable

            Try

                sStrXml = API.GetUserPulseReport(g_UserAPI)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" GetUserPulseReports : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        Function GetEMRMAreport(ByVal SiteId As String, ByVal FromDate As String, ByVal ToDate As String) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.GetEMRMAreport(SiteId, FromDate, ToDate)
                dt = XMLNodeToDataTable(sStrXml)

            Catch ex As Exception
                WriteLog(" LoadSiteAnalysisDailyAlerts : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
        Function LoadHistoricalTemperatureReport(ByVal SiteId As String, ByVal sDeviceId As String, ByVal FromDate As String, ByVal ToDate As String, Optional ByRef isERROR As Boolean = False) As DataTable

            Dim dt As New DataTable
            Dim sStrXml As XmlNode

            Try

                sStrXml = API.HistoricalTemperatureReport(SiteId, sDeviceId, FromDate, ToDate, g_UserAPI)

                dt = HistoricalXMLNodeToDataTable(sStrXml, isERROR)

            Catch ex As Exception
                WriteLog(" LoadHistoricalTemperatureReport : " & ex.Message.ToString())
            End Try

            Return dt

        End Function
    End Module
End Namespace

