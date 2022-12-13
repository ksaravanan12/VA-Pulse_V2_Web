Imports System.IO
Imports System.Data
Imports System.Xml

Namespace GMSUI

    Partial Class UserControls_gms_Header
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then

                If g_UserRole = enumUserRole.Admin Or g_UserRole = enumUserRole.Engineering Or g_UserRole = enumUserRole.Support Then

                    tdEmailTab.Visible = True
                    tdGMSReport.Visible = True
                    tdSettings.Visible = True

                Else

                    tdEmailTab.Visible = False
                    tdSettings.Visible = False

                    If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.TechnicalAdmin Or g_UserRole = enumUserRole.Partner _
                        Or g_IsPulseReport = 1 Then
                        tdGMSReport.Visible = True
                    Else
                        tdGMSReport.Visible = False
                    End If

                End If

                If g_UserRole = enumUserRole.Admin Then
                    tdAlertMaintenance.Visible = True
                Else
                    tdAlertMaintenance.Visible = False
                End If

                If g_UserRole = enumUserRole.TechnicalAdmin Then
                    tdSettings.Visible = True
                End If

                If g_UserRole = enumUserRole.Maintenance Or g_UserRole = enumUserRole.MaintenancePrism Or g_UserRole = enumUserRole.TechnicalAdmin Then
                    tdEmailTab.Visible = True
                End If

                doLoadsitlist()

            End If

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Try

                Dim sCompanys As String = Session("VACompanys")
                Dim sSites As String = Session("VASites")

                siteList = loadsiteList(sCompanys, sSites)

                If siteList.Rows.Count > 0 Then

                    If siteList.Rows.Count > 1 Then
                        drpsitelist.Items.Add(New ListItem("All Sites", 0))
                    Else
                        If siteList.Rows.Count = 1 Then Session("drpSiteid") = siteList.Rows(0).Item("siteid")
                    End If

                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            drpsitelist.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next

                End If

                Dim selectedid As String = ""

                selectedid = Session("drpSiteid")
                drpsitelist.SelectedValue = selectedid

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

        Protected Sub drpsitelist_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpsitelist.SelectedIndexChanged
            Dim siteid As String = drpsitelist.SelectedValue

            Session("drpSiteid") = siteid

            Dim url = Request.Url.GetLeftPart(UriPartial.Path)

            If Request.Url.Segments(2) <> "GeneratePdf.aspx" Then
                If (siteid <> "0") Then
                    url = url & "?sid=" & siteid
                Else
                    url = "Home.aspx"
                End If
                Page.Response.Redirect(url, True)
            End If

        End Sub

        Sub doLoadGlossaryInfo()

            Try

                Dim htmlstr As String = ""
                Dim mainHtmlStr As String = ""
                Dim dt As New DataTable
                Dim sPageName As String = Request.Url.AbsolutePath

                Dim fileName As String = System.IO.Path.GetFileName(sPageName)
                dt = LoadGlossaryinfo(fileName)

                htmlstr = LoadGlossary(dt)

                mainHtmlStr = "<div class='glsTitle'>"
                mainHtmlStr &= "<div  style='float:left;'>Glossary</div>"
                mainHtmlStr &= "<div style='float:right;'><img style='cursor: pointer;width:20px;height:20px;' id='glsClose' src='images/Close.png' /></div>"
                mainHtmlStr &= "</div>"
                mainHtmlStr &= "<div class='glossaryContent'>"
                mainHtmlStr &= htmlstr
                mainHtmlStr &= "</div>"
                glsdiv.InnerHtml = mainHtmlStr

            Catch ex As Exception
                WriteLog(" doLoadGlossaryInfo " & ex.Message.ToString())
            End Try

        End Sub
	
    End Class
    
End Namespace

