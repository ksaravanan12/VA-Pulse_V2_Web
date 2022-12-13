Imports System.IO
Imports System.Collections.Generic

Namespace GMSUI
    Partial Class Announcements
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If IsSecure() = False Then RedirectToSecurePage()
            Dim apiKey As String

            apiKey = g_UserAPI

            If apiKey = "" Then
                RedirectToErrorPage(LOGIN_SESSION_ERROR)
            End If

            If Not IsPostBack Then

                If Not g_UserRole = enumUserRole.Admin Then
                    PageVisitDetails(g_UserId, "Announcements", enumPageAction.AccessViolation, "user try to access Announcements")
                    Response.Redirect("AccessDenied.aspx")
                    Exit Sub
                End If

                hid_userid.Value = g_UserId
                LoadUserRole(drpUserRole)
                Try
                    PageVisitDetails(g_UserId, "Announcements", enumPageAction.View, "user visited announcements")
                Catch ex As Exception
                    WriteLog(" Exception PageVisitDetails UserId : " & g_UserId & ex.Message.ToString())
                End Try
            End If
	    GetImages()
        End Sub

        '******************************************************************************************************'
        ' Function Name : LoadUserRole                                                                       '
        ' Input         : Html dropdown control                                                                ' 
        ' Output        : List of user role                                                                  '
        ' Description   : Based on Enum value drop doen items loaded                                          '
        '******************************************************************************************************'
        Sub LoadUserRole(ByVal drpDeviceType As HtmlSelect)

            Dim Type As Integer = 0
            Dim EnumType As Integer()

            drpDeviceType.Items.Clear()
            EnumType = [Enum].GetValues(GetType(enumUserRole))
            For Each Type In EnumType
                drpDeviceType.Items.Add(New ListItem([Enum].GetName(GetType(enumUserRole), Type), Type))
            Next
        End Sub
	
        Protected Sub UploadFile(sender As Object, e As System.EventArgs) Handles btnUpload.Click

            Dim folderPath As String = Server.MapPath("~/AnnouncementFiles/")

            'Check whether Directory (Folder) exists.
            If Not Directory.Exists(folderPath) Then
                'If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(folderPath)
            End If

            If (FileUpload1.FileName <> "") Then
                'Save the File to the Directory (Folder).
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                GetImages()
                lblMessage.ForeColor = Drawing.Color.Green
                'Display the success message.
                lblMessage.Text = Path.GetFileName(FileUpload1.FileName) + " has been uploaded."

            Else
                lblMessage.Text = "Pleae select image to upload."
                lblMessage.ForeColor = Drawing.Color.Red
            End If

        End Sub

        Sub GetImages()

            lblMessage.Text = ""

            Dim folderPath As String = Server.MapPath("~/AnnouncementFiles/")

            If (Not System.IO.Directory.Exists(folderPath)) Then
                System.IO.Directory.CreateDirectory(folderPath)
            End If

            Dim filePaths() As String = Directory.GetFiles(Server.MapPath("~/AnnouncementFiles/"))
            Dim files As List(Of ListItem) = New List(Of ListItem)
            Dim htmlstring As String = ""

            For Each filePath As String In filePaths
                Dim URL = GetServerPath() & "AnnouncementFiles/" + Path.GetFileName(filePath)

                htmlstring += "<a style='Padding:3px;' href='" + URL + "' target='_blank'>" + URL + "</a><br/>"
                divImageList.InnerHtml = htmlstring
            Next

        End Sub

        Sub DeleteImage(ByVal URL As String)

            Dim uri As Uri = New Uri(URL)

            Dim filename As String = ""
            If uri.IsFile Then
                filename = Path.GetFileName(uri.LocalPath)
            End If

            Dim delfilename = "~/AnnouncementFiles/" + filename
            System.IO.File.Delete(Server.MapPath(delfilename))

        End Sub
    End Class
End Namespace
