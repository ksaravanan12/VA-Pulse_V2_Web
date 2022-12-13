Imports System.Data
Imports System.IO
Imports System.Drawing
Imports System.IO.File

Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Namespace GMSUI
    Partial Class Device_uploadFile
        Inherits System.Web.UI.Page
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                bgimg.Dispose()
                deviceinfo.Value = ""
                hdnIsAdd.Value = ""
            End If

            lblMsg.Text = ""
        End Sub


        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Dim divClose As String = ""
            Dim dt As New DataTable
            Dim ResponseError As String = ""
            Try
                If hdnCmd.Value = "UploadDeviceImage" Then
                    If hdnIsEditMode.Value = "0" Then
                        If bgimg.HasFile Then

                            Dim imagefile As HttpPostedFile
                            Dim imgPhoto As Image

                            Dim hfc As HttpFileCollection = Request.Files

                            For i As Integer = 0 To hfc.Count - 1

                                imagefile = hfc(i)
                                imgPhoto = Image.FromStream(imagefile.InputStream)
                                imgPhoto = FixedSize(imgPhoto, 270, 330)
                                dt = AddDevice_Image(hdnDataId.Value, hdnDeviceId.Value, hdnSiteId.Value, deviceinfo.Value, ReadImageToBytes(imgPhoto), imagefile.FileName, "1", hdnIsEditMode.Value, "1")
                            Next

                            lblMsg.Text = "Device image added successfully."
                            imgPhoto.Dispose()
                        Else
                            Dim data As Byte() = New [Byte](0) {}
                            dt = AddDevice_Image(hdnDataId.Value, hdnDeviceId.Value, hdnSiteId.Value, deviceinfo.Value, data, "", "1", hdnIsEditMode.Value, "1")
                            lblMsg.Text = "Device info added successfully."
                        End If
                    Else
                        If bgimg.HasFile Then
                            Dim imagefile As HttpPostedFile = bgimg.PostedFile
                            Dim imgPhoto As Image = Image.FromStream(imagefile.InputStream)
                            imgPhoto = FixedSize(imgPhoto, 270, 330)
                            dt = AddDevice_Image(hdnDataId.Value, hdnDeviceId.Value, hdnSiteId.Value, deviceinfo.Value, ReadImageToBytes(imgPhoto), imagefile.FileName, "1", hdnIsEditMode.Value, "1")
                            lblMsg.Text = "Device image updated successfully."
                            imgPhoto.Dispose()
                        Else
                            Dim data As Byte() = New [Byte](0) {}
                            dt = AddDevice_Image(hdnDataId.Value, hdnDeviceId.Value, hdnSiteId.Value, deviceinfo.Value, data, "", "1", hdnIsEditMode.Value, "1")
                            lblMsg.Text = "Device info updated successfully."
                        End If
                    End If
                End If
                bgimg.Dispose()

                deviceinfo.Value = ""
                hdnDataId.Value = ""
                divClose = "Close();"
                ClientScript.RegisterStartupScript(GetType(Page), "a key", "<script type=""text/javascript"">" & divClose + "</script>")
            Catch ex As Exception
                WriteLog("Exception From btnSubmit_Click Upload Tag Image: " + ex.Message)
                bgimg.Dispose()
                deviceinfo.Value = ""
                hdnDataId.Value = ""
                lblMsg.Text = "The device image is not added."
            End Try
        End Sub


        Private Function ReadFile(ByVal file As HttpPostedFile) As Byte()
            Dim data As Byte() = New [Byte](file.ContentLength - 1) {}
            file.InputStream.Read(data, 0, file.ContentLength)
            Return data
        End Function

        Private Function ReadImageToBytes(ByVal img1 As Image) As Byte()
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
