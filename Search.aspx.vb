Imports System
Imports System.IO
Imports System.Data
Imports System.xml
Imports System.Net

Namespace GMSUI

    Partial Class Search

        Inherits System.Web.UI.Page

        Dim nSite As Integer = 0
        Dim apiKey As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If Val(Request.QueryString("sid")) > 0 Then
                    nSite = Val(Request.QueryString("sid"))
                ElseIf Session("drpSiteid") Then
                    nSite = Session("drpSiteid")
                End If

                hid_userid.Value = g_UserId
                hdnSiteId.Value = nSite
                hid_userrole.Value = g_UserRole
                hid_IsTempMonitoring.Value = g_IsTempMonitoring

                LoadSitesIntoDropdown()
                LoadDeviceType(ddlDeviceType)
                LoadFilters()

                Try
                    PageVisitDetails(g_UserId, "Search", enumPageAction.View, "user visited Device Search")
                Catch ex As Exception
                    WriteLog(" Home - Search PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

                searchmenu.Style.Add("display", "")

                If g_UserRole <> enumUserRole.Engineering And g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Support Then
                    searchmenu.Style.Add("display", "none")
                End If

            End If

        End Sub

        '******************************************************************************************************'
        ' Function Name : LoadDeviceType                                                                       '
        ' Input         : Html dropdown control                                                                ' 
        ' Output        : List of device type                                                                  '
        ' Description   : Based on Enum value drop doen items loaded                                          '
        '******************************************************************************************************'

        Sub LoadDeviceType(ByVal drpDeviceType As HtmlSelect)

            Dim Type As Integer = 0
            Dim EnumType As Integer()

            drpDeviceType.Items.Clear()

            EnumType = [Enum].GetValues(GetType(enumDeviceType))

            For Each Type In EnumType
                drpDeviceType.Items.Add(New ListItem([Enum].GetName(GetType(enumDeviceType), Type), Type))
            Next

        End Sub

        '******************************************************************************************************'
        ' Function Name : LoadFilters                                                                          '
        ' Input         :                                                                                      ' 
        ' Output        : List of device type                                                                  '
        ' Description   : (API- GetSearchFilters ) using load the filter criteria into Drop down               '
        '******************************************************************************************************'
        Protected Sub LoadFilters()
            RenderFilterCriteria(GetSearchResult("0"))
        End Sub

        '******************************************************************************************************'
        ' Function Name : GetXmlElement                                                                        '
        ' Input         : Server response XML STRING                                                           ' 
        ' Output        : Root element of XML                                                                  '
        ' Description   : Read the server xml string  and  return the XML root element                         '
        '******************************************************************************************************'

        Protected Function GetXmlElement(ByVal strXMLElement As String) As XmlElement

            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim reader As XmlTextReader

            reader = New XmlTextReader(New System.IO.StringReader(strXMLElement))
            reader.Read()
            doc.Load(reader)

            root = doc.DocumentElement

            Return root

        End Function

        '**************************************************************************************************************************************'
        ' Function Name : RenderFilterCriteria                                                                                                   '
        ' Input         : Server response XmlElement                                                                                         ' 
        ' Output        : List of filter criteria in to drop down                                                                              '
        ' Description   : Read the server Xml Element  and  Make the filter criteria for the device type drop dowm items                        '
        '**************************************************************************************************************************************'
        Protected Sub RenderFilterCriteria(ByVal root As XmlElement)

            Dim elemList As XmlNodeList = Nothing
            Dim elemValuesList As XmlNodeList = Nothing

            Dim tblRow As HtmlTableRow = Nothing

            Dim nElemIdx As Integer = 0
            Dim nSubElemIdx As Integer = 0
            Dim DisplayColumn As String = ""
            Dim FilterColumn As String = ""
            Dim DataType As String = ""

            ddlFilter.Items.Add(New ListItem("All", "0"))
            ddlFilterMonitor.Items.Add(New ListItem("All", "0"))
            ddlFilterStar.Items.Add(New ListItem("All", "0"))

            For nElemIdx = 0 To root.ChildNodes.Count - 1

                elemList = root.ChildNodes(nElemIdx).SelectNodes("DeviceFilter")

                For nSubElemIdx = 0 To elemList.Count - 1

                    DisplayColumn = root.ChildNodes(nElemIdx).ChildNodes(nSubElemIdx).ChildNodes(0).InnerText
                    DataType = root.ChildNodes(nElemIdx).ChildNodes(nSubElemIdx).ChildNodes(1).InnerText
                    FilterColumn = root.ChildNodes(nElemIdx).ChildNodes(nSubElemIdx).ChildNodes(2).InnerText
                    elemValuesList = root.ChildNodes(nElemIdx).ChildNodes(nSubElemIdx).SelectNodes("Values")

                    tblRow = New HtmlTableRow()

                    If root.ChildNodes(nElemIdx).LocalName.ToString = enumDeviceType.Tag.ToString Then
                        AddCell(tblRow, MakeFilterCriteria(tblRow, FilterColumn, DisplayColumn, DataType, elemValuesList, enumDeviceType.Tag.ToString()))
                        tblFilterCriteria.Rows.Insert(tblFilterCriteria.Rows.Count, tblRow)
                        ddlFilter.Items.Add(New ListItem(DisplayColumn, FilterColumn))
                    ElseIf root.ChildNodes(nElemIdx).LocalName.ToString = enumDeviceType.Monitor.ToString Then
                        AddCell(tblRow, MakeFilterCriteria(tblRow, FilterColumn, DisplayColumn, DataType, elemValuesList, enumDeviceType.Monitor.ToString()))
                        tblFilterCriteriaMonitor.Rows.Insert(tblFilterCriteriaMonitor.Rows.Count, tblRow)
                        ddlFilterMonitor.Items.Add(New ListItem(DisplayColumn, FilterColumn))
                    ElseIf root.ChildNodes(nElemIdx).LocalName.ToString = enumDeviceType.Star.ToString Then
                        AddCell(tblRow, MakeFilterCriteria(tblRow, FilterColumn, DisplayColumn, DataType, elemValuesList, enumDeviceType.Star.ToString()))
                        tblFilterCriteriaStar.Rows.Insert(tblFilterCriteriaStar.Rows.Count, tblRow)
                        ddlFilterStar.Items.Add(New ListItem(DisplayColumn, FilterColumn))
                    End If

                Next

            Next

        End Sub

        Protected Function MakeFilterCriteria(ByVal tr As HtmlTableRow, ByVal FilterColumn As String, ByVal DisplayColumn As String, ByVal DataType As String, ByVal elemValuesList As XmlNodeList, ByVal nDeviceType As String) As String

            Dim nDataType As Integer = 0

            Dim sInnerHtml As String = ""
            Dim sFieldCondition As String = ""
            Dim sFieldvalue1 As String = ""
            Dim sFieldvalue2 As String = ""
            Dim sField2 As String = ""

            Dim sClientCheckFirst, sClientCheckSecond, sCheckedFlag As String

            sClientCheckFirst = ""
            sClientCheckSecond = ""
            sCheckedFlag = ""

            tr.ID = FilterColumn & nDeviceType

            If DataType = "decimal" Or DataType = "float" Then
                nDataType = enumFilterDataType.enum_NumberWithFraction
            ElseIf DataType = "int" Or DataType = "tinyint" Or DataType = "smallint" Or DataType = "bigint" Or DataType = "bit" Then
                nDataType = enumFilterDataType.enum_NumericOnly
            ElseIf DataType = "datetime" Then
                nDataType = enumFilterDataType.enum_Date
            End If

            If FilterColumn = "FirstSeen" Or FilterColumn = "ReceivedTime" Or FilterColumn = "BatteryReplacementOn" Or FilterColumn = "FirstReceivedTime" Or FilterColumn = "LastSeen" Or FilterColumn = "UpdatedOn" Then
                nDataType = enumFilterDataType.enum_Date
            End If

            If nDataType = enumFilterDataType.enum_NumericOnly Then
                sClientCheckFirst = " onkeypress='return allowNumberOnly(event);'"
                sClientCheckSecond = " onkeypress='return allowNumberOnly(event);' onblur=""NumericRangeCheck('txt" & FilterColumn & nDeviceType & "1','txt" & FilterColumn & nDeviceType & "2');"""
            ElseIf nDataType = enumFilterDataType.enum_NumberWithFraction Then
                sClientCheckFirst = " onkeypress='return AllowFloat(event);'"
                sClientCheckSecond = " onkeypress='return AllowFloat(event);' onblur=""NumericRangeCheck('txt" & FilterColumn & nDeviceType & "1','txt" & FilterColumn & nDeviceType & "2');"""
            ElseIf nDataType = enumFilterDataType.enum_Date Then
                sClientCheckFirst = " onkeypress='return KeyNumericwithHypen(event);' onblur='isDate(this.value,this);'"
                sClientCheckSecond = " onkeypress='return KeyNumericwithHypen(event);' onblur=""isDate(this.value,this);DateRangeCheckwithMessage('txt" & FilterColumn & nDeviceType & "1','txt" & FilterColumn & nDeviceType & "2','To date is less than From date');"""
            End If

            tr.Style.Add("display", "none")
            sFieldCondition = ""
            sFieldvalue1 = ""
            sFieldvalue2 = ""
            sField2 = " style='display: none' "

            sCheckedFlag = " checked "
            sInnerHtml = "<td>" & _
                         "<table border='0'>" & _
                         "<tr>" & _
                         "<td style='width: 170px; text-align: left;' class='clsFilterCriteria'>" & _
                         "<input id='chk" & FilterColumn & nDeviceType & "' name='chk" & FilterColumn & nDeviceType & "' type='checkbox' " & sCheckedFlag & " onclick=showHideFilterCtrl(this,'" & FilterColumn & nDeviceType & "') />" & DisplayColumn & "</td>"

            If nDataType = enumFilterDataType.enum_Date Then
                sInnerHtml &= "<td style='width: 70px; text-align: left;' nowrap>" & _
                              "<select id='lst" & FilterColumn & nDeviceType & "Filter' name='lst" & FilterColumn & nDeviceType & "Filter' onchange=showFilterCondCtrl(this,'txt" & FilterColumn & nDeviceType & "1','txt" & FilterColumn & nDeviceType & "2','btn" & FilterColumn & nDeviceType & "2') style='width: 70px;' >&nbsp;"
            Else
                sInnerHtml &= "<td style='width: 70px; text-align: left;'>" & _
                              "<select id='lst" & FilterColumn & nDeviceType & "Filter' name='lst" & FilterColumn & nDeviceType & "Filter' onchange=showFilterCondCtrl(this,'txt" & FilterColumn & nDeviceType & "1','txt" & FilterColumn & nDeviceType & "2') style='width: 70px;' > "
            End If

            If elemValuesList.Count > 1 Then
                sInnerHtml &= "<option value='2'>=</option>"
            Else
                sInnerHtml &= "<option value='2'>=</option>" & _
                              "<option value='3'>>=</option>" & _
                              "<option value='4'><=</option>" & _
                              "<option value='5'>between</option>"
            End If

            sInnerHtml &= "</select>" & _
                          "</td>" & _
                          "<td>" & _
                          "<table>" & _
                          "<tr>"

            If elemValuesList.Count > 1 Then

                If FilterColumn = "TagType" Or FilterColumn = "MonitorType" Then
                    sInnerHtml &= "<td>" & _
                                    "<select id='txt" & FilterColumn & nDeviceType & "1' name='txt" & FilterColumn & nDeviceType & "1' style='width: 150px;' />" & LoadOptionsIntoDropdown(elemValuesList, True) & "</td>"
                Else
                    sInnerHtml &= "<td>" & _
                                    "<select id='txt" & FilterColumn & nDeviceType & "1' name='txt" & FilterColumn & nDeviceType & "1' />" & LoadOptionsIntoDropdown(elemValuesList) & "</td>"
                End If

            Else

                sInnerHtml &= "<td nowrap>" & _
                              "<input type='text' " & sClientCheckFirst & " id='txt" & FilterColumn & nDeviceType & "1' name='txt" & FilterColumn & nDeviceType & "1' size='8' " & sFieldvalue1 & " /> "

                If nDataType = enumFilterDataType.enum_Date Then
                    sInnerHtml &= "<script type='text/javascript'>" & _
                                  "Calendar.setup({ " & _
                                  "inputField: ""txt" & FilterColumn & nDeviceType & "1""," & _
                                  "ifFormat: ""%m/%d/%Y""," & _
                                  "button: ""txt" & FilterColumn & nDeviceType & "1""," & _
                                  "align: ""Br""" & _
                                  "});" & _
                                  "</script>"
                End If
                sInnerHtml &= "</td>"

                sInnerHtml &= "<td>" & _
                              "<input type='text' " & sClientCheckSecond & " id='txt" & FilterColumn & nDeviceType & "2' name='txt" & FilterColumn & nDeviceType & "2' size='8' " & sField2 & " " & sFieldvalue2 & " />"

                If nDataType = enumFilterDataType.enum_Date Then
                    sInnerHtml &= "<script type='text/javascript'>" & _
                                  "Calendar.setup({ " & _
                                  "inputField    : 'txt" & FilterColumn & nDeviceType & "2'," & _
                                  "ifFormat: '%m/%d/%Y'," & _
                                  "button: 'txt" & FilterColumn & nDeviceType & "2'," & _
                                  "align: 'Br'" & _
                                  "});" & _
                                  "</script>"
                End If
                sInnerHtml &= "</td>"
            End If

            sInnerHtml &= "</tr>" & _
                          "</table>" & _
                          "</td>"

            sInnerHtml &= "</tr>" & _
                          "</table>" & _
                          "</td>"

            Return sInnerHtml

        End Function

        '*********************************************************************************************'
        ' Function Name : LoadOptionsIntoDropdown                                                     '
        ' Input         : XML Node                                                                    ' 
        ' Output        : return;s the set of option for drop down                                    '
        ' Description   : Read the xml node and add in to drop dwon control                           '
        '*********************************************************************************************'
        Protected Function LoadOptionsIntoDropdown(ByVal elemValuesList As XmlNodeList, Optional ByVal isDeviceType As Boolean = False) As String

            Dim nElemIdx As Integer = 0

            Dim sOptions As String = ""
            Dim sElemVal As String = ""

            Dim aElemVal() As String

            If elemValuesList.Count > 1 Then

                For nElemIdx = 0 To elemValuesList.Count - 1

		If Not elemValuesList.Item(nElemIdx).ChildNodes(0) Is Nothing Then
                    If isDeviceType Then
                        sElemVal = elemValuesList.Item(nElemIdx).ChildNodes(0).InnerText

                        aElemVal = sElemVal.Split("~")

                        sOptions &= "<option value='" & aElemVal(0) & "'>" & aElemVal(1) & "</option>"

                    Else

                        sElemVal = elemValuesList.Item(nElemIdx).ChildNodes(0).InnerText
                        sOptions &= "<option value='" & sElemVal & "'>" & sElemVal & "</option>"

                    End If
		End If
                Next

            End If

            Return sOptions

        End Function

        '**************************************************************************************************'
        ' Function Name : RequestAPI                                                                       '
        ' Input         : APIURL ,ResponseString                                                           ' 
        ' Output        : XML String                                                                       '
        ' Description   : Call specified api method from the  wen service url, it will return the xml data '
        '**************************************************************************************************'

        Private Sub RequestAPI(ByVal Url As String, ByRef refResponseFromServer As String)

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

        End Sub

        '**************************************************************************************************'
        ' Function Name : RequestAPI                                                                       '
        ' Input         : APIURL ,ResponseString                                                           ' 
        ' Output        : XML String                                                                       '
        ' Description   : Call specified api method from the  wen service url, it will return the xml data '
        '**************************************************************************************************'
        Sub LoadSitesIntoDropdown()

            Dim dtSite As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                dtSite = loadsiteList(sCompanys, sSites)

                If dtSite.Rows.Count > 0 Then

                    If dtSite.Rows.Count > 1 Then
                        ddlSiteTagMovements.Items.Add(New ListItem("All Sites", 0))
                    Else
                        If dtSite.Rows.Count = 1 Then Session("drpSiteid") = dtSite.Rows(0).Item("siteid")
                    End If

                    For nidx As Integer = 0 To dtSite.Rows.Count - 1
                        With (dtSite.Rows(nidx))
                            ddlSiteTagMovements.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

                ddlSiteTagMovements.Value = 0

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

    End Class

End Namespace
