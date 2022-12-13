Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class CentrakDevices
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If
          
            If Not (g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Support) Then
                PageVisitDetails(g_UserId, "Centrak Devices", enumPageAction.AccessViolation, "user try to access Centrak Devices Report")
                Response.Redirect("AccessDenied.aspx")
                Exit Sub
            End If

            If Not IsPostBack Then
	    
                LoadDeviceTypes()
                ModelItemBindGrid(False)

                Try
                    PageVisitDetails(g_UserId, "Centrak Devices", enumPageAction.View, "Centrak Decvices")
                Catch ex As Exception
                    WriteLog("Centrak Devices" & g_UserId & ex.Message.ToString())
                End Try

            End If

            hid_userrole.Value = g_UserRole
            hid_userid.Value = g_UserId

        End Sub

        Private Sub LoadDeviceTypes()

            ddlDeviceType.Items.Add(New ListItem("Tag", enumDeviceType.Tag))
            ddlDeviceType.Items.Add(New ListItem("Monitor", enumDeviceType.Monitor))

        End Sub

        Private Sub ModelItemBindGrid(Optional ByVal bIsCSV As Boolean = False)

            Dim dt As New DataTable
            Dim tbRow As HtmlTableRow
            Dim idx As Integer
            Dim context As HttpContext
            Dim ModelItem As String = ""
            Dim DeviceSubType As String = ""
            Dim ORG_BatteryCapacity As String = ""
            Dim IsLBIRule As String = ""
            Dim IsBatteryCapacity As String = ""
            Dim IsAutoReplacement As String = ""
            Dim IsFDK As String = ""
            Dim IsDetectionofBatteryDischarge As String = ""
            Dim sort As String = Request.QueryString("sort")
            Dim ord As String = Request.QueryString("ord")
            Dim imgUrl As String = ""

            dt = Get_CenTrakModelItems(ddlDeviceType.SelectedValue, 0, False)

            If sort = "" Then
                sort = "ModelItem ASC"
            End If

            dt.DefaultView.Sort = sort & " " & ord
            dt = dt.DefaultView.ToTable

            If dt Is Nothing Then
                tbRow = New HtmlTableRow
                AddCell(tbRow, "No Record Found", "center", 3)
                tblBatteryCalculation.Rows.Add(tbRow)
                Exit Sub
            End If

            If dt.Rows.Count = 0 Then
                tbRow = New HtmlTableRow
                AddCell(tbRow, "No Record Found", "center", 3)
                tblBatteryCalculation.Rows.Add(tbRow)
            End If

            tbRow = New HtmlTableRow
            AddCell(tbRow, "#")
            AddCell(tbRow, "Model")
            AddCell(tbRow, "Type")
            AddCell(tbRow, "Battery Capacity")
            AddCell(tbRow, "Is LBI Rule")
            AddCell(tbRow, "Battery.Capacity Rule")
            AddCell(tbRow, "Auto Replacement")
            AddCell(tbRow, "Is FDK")
            AddCell(tbRow, "Battery.Discharge Rule")
            tbRow.Attributes.Add("class", "clstblTagHeader")
            tblBatteryCalculation.Rows.Add(tbRow)

            ViewState("data1") = dt

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then

                    If bIsCSV Then
                        context = HttpContext.Current
                        InitiateCSV(context, "CentrakDevices_" & Now.ToString("MMddyyyyhms"))
                        AddCSVCell(context, "#", True)
                        AddCSVCell(context, "Model", True)
                        AddCSVCell(context, "Type", True)
                        AddCSVCell(context, "Battery Capacity", True)
                        AddCSVCell(context, "Is LBI Rule", True)
                        AddCSVCell(context, "Battery.Capacity Rule", True)
                        AddCSVCell(context, "Auto Replacement", True)
                        AddCSVCell(context, "Is FDK", True)
                        AddCSVCell(context, "Battery.Discharge Rule", True)
                        AddCSVNewLine(context)
                    End If

                    For idx = 0 To dt.Rows.Count - 1
                        With dt.Rows(idx)

                            ModelItem = CheckIsDBNull(.Item("ModelItem"), False, "--")
                            DeviceSubType = CheckIsDBNull(.Item("DeviceSubType"), False, "--")
                            ORG_BatteryCapacity = CheckIsDBNull(.Item("ORG_BatteryCapacity"), False, "--")
                            IsLBIRule = CheckIsDBNull(.Item("IsLBIRule"), False, "--")
                            IsBatteryCapacity = CheckIsDBNull(.Item("IsBatteryCapacity"), False, "--")
                            IsAutoReplacement = CheckIsDBNull(.Item("IsAutoReplacement"), False, "--")
                            IsFDK = CheckIsDBNull(.Item("IsFDK"), False, "--")
                            IsDetectionofBatteryDischarge = CheckIsDBNull(.Item("IsDetectionofBatteryDischarge"), False, "--")

                            If bIsCSV Then

                                AddCSVCell(context, idx + 1, True)
                                AddCSVCell(context, ModelItem, True)
                                AddCSVCell(context, DeviceSubType, True)
                                AddCSVCell(context, ORG_BatteryCapacity, True)
                                AddCSVCell(context, IsLBIRule, True)
                                AddCSVCell(context, IsBatteryCapacity, True)
                                AddCSVCell(context, IsAutoReplacement, True)
                                AddCSVCell(context, IsFDK, True)
                                AddCSVCell(context, IsDetectionofBatteryDischarge, True)
                                AddCSVNewLine(context)

                            Else

                                tbRow = New HtmlTableRow

                                AddCell(tbRow, idx + 1)
                                AddCell(tbRow, ModelItem)
                                AddCell(tbRow, DeviceSubType)
                                AddCell(tbRow, ORG_BatteryCapacity)
                                AddCell(tbRow, IsLBIRule)
                                AddCell(tbRow, IsBatteryCapacity)
                                AddCell(tbRow, IsAutoReplacement)
                                AddCell(tbRow, IsFDK)
                                AddCell(tbRow, IsDetectionofBatteryDischarge)

                                If idx Mod 2 = 0 Then
                                    tbRow.Attributes.Add("class", "clstblRowEven")
                                Else
                                    tbRow.Attributes.Add("class", "clstblRowOdd")
                                End If

                                tblBatteryCalculation.Rows.Add(tbRow)

                            End If
                        End With
                    Next

                    If bIsCSV Then
                        context.Response.End()
                    End If

                End If
            End If
        End Sub

        Protected Sub ddlDeviceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDeviceType.SelectedIndexChanged
            ModelItemBindGrid(False)
        End Sub

        Protected Sub btnExport_Click(sender As Object, e As EventArgs)
            ModelItemBindGrid(True)
        End Sub

    End Class
End Namespace
