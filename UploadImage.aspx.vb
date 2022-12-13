Imports System
Imports System.IO
Imports System.Data
Imports System.IO.File
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing

Namespace GMSUI
    Partial Class UploadImage
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then

                If Val(Request.QueryString("sid")) > 0 Then
                    hdSiteId.Value = Val(Request.QueryString("sid"))
                    lblUserName.Text = g_UserName
                    hid_userrole.Value = g_UserRole
                End If

                Try
                    PageVisitDetails(g_UserId, "Upload Image", enumPageAction.View, "user visited Upload Image")
                Catch ex As Exception
                    WriteLog("UploadImage - UserId " & g_UserId & ex.Message.ToString())
                End Try

            End If
        End Sub

        Protected Sub btnUploadImg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadImg.Click
            Dim dt As New DataTable

            If flpimg.HasFile Then
                Try

                    Dim imagefile As HttpPostedFile = flpimg.PostedFile
                    Dim extension As String = Path.GetExtension(imagefile.FileName)
                    If extension = ".pdf" Then
                        Dim filename As String = Path.GetFileName(flpimg.PostedFile.FileName)
                        Dim contentType As String = flpimg.PostedFile.ContentType
                        Using fs As Stream = flpimg.PostedFile.InputStream
                            Using br As New BinaryReader(fs)
                                Dim bytes As Byte() = br.ReadBytes(CType(fs.Length, Long))
                                dt = Add_Image(hdSiteId.Value, bytes, imagefile.FileName, lblUserName.Text, txtDescription.Text)
                            End Using
                        End Using

                        'lblMsg.Text = "Device image added successfully."
                    Else

                        Dim imgPhoto As System.Drawing.Image = System.Drawing.Image.FromStream(imagefile.InputStream)
                        'imgPhoto = FixedSize(imgPhoto, 270, 330)
                        dt = Add_Image(hdSiteId.Value, ReadImageToBytes(imgPhoto), imagefile.FileName, lblUserName.Text, txtDescription.Text)
                        'lblMsg.Text = "Device image added successfully."
                        imgPhoto.Dispose()
                    End If

                Catch ex As Exception
                    'Label1.Text = "ERROR: " & ex.Message.ToString()
                End Try
            Else
                'Label1.Text = "You have not specified a file."
            End If
        End Sub

        Private Function ReadFile(ByVal file As HttpPostedFile) As Byte()
            Dim data As Byte() = New [Byte](file.ContentLength - 1) {}
            file.InputStream.Read(data, 0, file.ContentLength)
            Return data
        End Function

        Private Function ReadImageToBytes(ByVal img1 As System.Drawing.Image) As Byte()
            Dim imgCon As New ImageConverter()
            Return DirectCast(imgCon.ConvertTo(img1, GetType(Byte())), Byte())
        End Function

        Private Function FixedSize(ByVal imgPhoto As System.Drawing.Image, ByVal Width As Integer, ByVal Height As Integer) As System.Drawing.Image
            Try
                Dim sourceWidth As Integer = imgPhoto.Width
                Dim sourceHeight As Integer = imgPhoto.Height
                Dim sourceX As Integer = 0
                Dim sourceY As Integer = 0
                Dim destX As Integer = 0
                Dim destY As Integer = 0


                Dim nPercent As Single = 0
                Dim nPercentW As Single = 0
                Dim nPercentH As Single = 0

                nPercentW = (Convert.ToSingle(Width) / Convert.ToSingle(sourceWidth))
                nPercentH = (Convert.ToSingle(Height) / Convert.ToSingle(sourceHeight))

                'if we have to pad the height pad both the top and the bottom
                'with the difference between the scaled height and the desired height
                If nPercentH < nPercentW Then
                    nPercent = nPercentH
                    destX = Convert.ToInt32(Math.Truncate((Width - (sourceWidth * nPercent)) / 2))
                Else
                    nPercent = nPercentW
                    destY = Convert.ToInt32(Math.Truncate((Height - (sourceHeight * nPercent)) / 2))
                End If

                Dim destWidth As Integer = Convert.ToInt32(Math.Truncate(sourceWidth * nPercent))
                Dim destHeight As Integer = Convert.ToInt32(Math.Truncate(sourceHeight * nPercent))

                Dim bmPhoto As New Bitmap(Width, Height, PixelFormat.Format24bppRgb)
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution)

                Dim grPhoto As Graphics = Graphics.FromImage(bmPhoto)
                grPhoto.Clear(Color.White)
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic

                grPhoto.DrawImage(imgPhoto, New Rectangle(destX, destY, destWidth, destHeight), New Rectangle(sourceX - 1, sourceY - 1, sourceWidth, sourceHeight), GraphicsUnit.Pixel)

                grPhoto.Dispose()
                Return bmPhoto
            Catch ex As Exception
                WriteLog("Exception From FixedSize: " + ex.Message)
            End Try

            Return imgPhoto

        End Function

    End Class
End Namespace
