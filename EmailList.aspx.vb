Imports System
Imports System.IO
Imports System.Data
Imports System.xml

Namespace GMSUI

    Partial Class EmailList
    
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String
	    
            apiKey = g_UserAPI
	    
            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            Dim SiteId As String = Request.QueryString("sid")
            Dim alertId As String = Request.QueryString("alertId")
            Dim typeId As String = Request.QueryString("typeId")

            Session("drpSiteid") = SiteId
            Dim type As String = Request.QueryString("Bin")
            Dim bin As String = ""

            If (alertId <> "" Or Session("n_alertId") <> "") Then
	    
                bin = ""
		
                If (alertId = "") Then
                    alertId = Session("n_alertId")
                Else
                    Session("n_alertId") = alertId
                End If

            Else
	    
                Session("n_alertId") = ""
		
                If (type = "1") Then
                    bin = "1"
                ElseIf (type = "2") Then
                    bin = "2"
                Else
                    bin = Session("n_bin")
                End If
            End If

            Dim DeviceId As String = ""
            Dim CurPage As String = ""

            If Not IsPostBack Then
	    
                If Not (g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Maintenance _
                        Or g_UserRole = enumUserRole.MaintenancePrism Or g_UserRole = enumUserRole.Support Or g_UserRole = enumUserRole.TechnicalAdmin) Then
                    PageVisitDetails(g_UserId, "Email List", enumPageAction.AccessViolation, "user try to access Email List")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                Session("n_siteId") = SiteId
                Session("n_bin") = bin
                Session("n_typeId") = typeId

                Try
                    PageVisitDetails(g_UserId, "Email List", enumPageAction.View, "user visited Setup Email")
                Catch ex As Exception
                    WriteLog(" Email List PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

                doLoadsitlist()
                LoadAlertType()

                hid_userid.Value = g_UserId

            End If

            hid_userrole.Value = g_UserRole
            hid_pwd.Value = doGetPassword()

        End Sub

        Function doGetPassword() As String
	
            Dim dtPassword As New DataTable
            Dim password As String = ""
	    
            Try
                
                'API CALL
                dtPassword = GetPasswordInfo(g_UserId)
		
                If dtPassword.Rows.Count > 0 Then
                    For nidx As Integer = 0 To dtPassword.Rows.Count - 1
                        password = dtPassword.Rows(0).Item("Password")
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doGetPassword " & ex.Message.ToString())
            End Try
	    
            Return password
	    
        End Function

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                'API CALL
                siteList = loadsiteList(sCompanys, sSites)

                If siteList.Rows.Count > 0 Then

                    drpSites.Items.Add(New ListItem("Select", 0))

                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            drpSites.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next

                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try
	    
        End Sub
       
        Sub LoadAlertType()
	
            ddlAlertType.Items.Clear()
            ddlAlertType.Items.Add(New ListItem("Continuous Alert", enumAlertType.ContinuousAlert))
            ddlAlertType.Items.Add(New ListItem("Single Alert", enumAlertType.SingleAlert))

            ddlAlertTypeUpdate.Items.Clear()
            ddlAlertTypeUpdate.Items.Add(New ListItem("Continuous Alert", enumAlertType.ContinuousAlert))
            ddlAlertTypeUpdate.Items.Add(New ListItem("Single Alert", enumAlertType.SingleAlert))
	    
        End Sub
	
    End Class
    
End Namespace

