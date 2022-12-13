﻿Imports System.IO
Imports System.Data
Imports System.Xml
Imports System.Net

Namespace GMSUI
    Partial Class MonitorGroupReport
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If g_UserRole <> enumUserRole.Admin And g_UserRole <> enumUserRole.Engineering And g_UserRole <> enumUserRole.Support Then
                    PageVisitDetails(g_UserId, "Monitor Group Report", enumPageAction.AccessViolation, "user try to access Monitor Group Report")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                doLoadsitlist()

                Try
                    PageVisitDetails(g_UserId, "Monitor Group Report", enumPageAction.View, "user visited Monitor Group Report")
                Catch ex As Exception
                    WriteLog("Monitor Group Report PageVisitDetails UserId " & g_UserId & ex.Message.ToString())
                End Try

            End If

        End Sub

        Sub doLoadsitlist()

            Dim siteList As New DataTable

            Dim sCompanys As String = Session("VACompanys")
            Dim sSites As String = Session("VASites")

            Try

                siteList = loadsiteList(sCompanys, sSites)

                ddlsite.Items.Clear()
                ddlsite.Items.Add(New ListItem("Select Site", "0"))

                If siteList.Rows.Count > 0 Then
                    For nidx As Integer = 0 To siteList.Rows.Count - 1
                        With (siteList.Rows(nidx))
                            ddlsite.Items.Add(New ListItem(.Item("sitename"), .Item("siteid")))
                        End With
                    Next
                End If

            Catch ex As Exception
                WriteLog(" doloadsitelist " & ex.Message.ToString())
            End Try

        End Sub

    End Class

End Namespace
