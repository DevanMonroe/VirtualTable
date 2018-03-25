Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

''' <summary>This is a test comment for git</summary>
Public Class Background
   Inherits Disposable

   Private Shared _testPattern As Image

   Shared Sub New()
      Dim width As Integer = 500
      Dim height As Integer = 500
      _testPattern = New Bitmap(width, height)
      Using g As Graphics = Graphics.FromImage(_testPattern)
         g.CompositingMode = CompositingMode.SourceCopy
         g.CompositingQuality = CompositingQuality.HighQuality
         g.InterpolationMode = InterpolationMode.HighQualityBicubic
         g.SmoothingMode = SmoothingMode.HighQuality
         g.PixelOffsetMode = PixelOffsetMode.HighQuality

         Using pen As New Pen(Color.Red, 3.0F)
            g.DrawLine(pen, Point.Empty, New Point(_testPattern.Size))
            g.DrawLine(pen, New Point(0, _testPattern.Height), New Point(width, 0))
         End Using
         Using Pen As New Pen(Color.Blue, 3.0F)
            Dim xOffset As Integer = 20
            Dim yOffset As Integer = 30
            Dim diameter As Integer = 100
            g.DrawEllipse(Pen, New Rectangle(xOffset, yOffset, diameter, diameter))
            g.DrawEllipse(Pen, New Rectangle(width - diameter - xOffset, yOffset, diameter, diameter))
            g.DrawEllipse(Pen, New Rectangle(xOffset, height - diameter - yOffset, diameter, diameter))
            g.DrawEllipse(Pen, New Rectangle(width - diameter - xOffset, height - diameter - yOffset, diameter, diameter))
         End Using
      End Using
   End Sub

   Private _rawImage As Image
   Private _size As Size
   Private _resizedImage As Bitmap

   Public Property Size As Size
      Get
         If _size.IsEmpty Then Return Image.Size
         Return _size
      End Get
      Set(value As Size)
         _size = value
         If _resizedImage IsNot Nothing Then _resizedImage.Dispose()
         _resizedImage = Nothing
      End Set
   End Property

   ''' <summary>The map image seen by players</summary>
   Property Image As Image
      Get
         If _rawImage Is Nothing Then Return _testPattern
         Return _rawImage
      End Get
      Set(value As Image)
         If _resizedImage IsNot Nothing Then _resizedImage.Dispose()
         _resizedImage = Nothing
         If _rawImage IsNot Nothing Then _rawImage.Dispose()
         _rawImage = Nothing
         If value IsNot Nothing Then _rawImage = value.Clone
      End Set
   End Property

   Private ReadOnly Property ResizedImage
      Get
         If _size.IsEmpty Then Return Image
         If _resizedImage Is Nothing Then _resizedImage = ImageResize(Image, _size)
         Return _resizedImage
      End Get
   End Property

   Private Function ImageResize(srcImage As Image, destSize As Size) As Image
      Dim destRect As New Rectangle(Point.Empty, destSize)
      Dim destImage As New Bitmap(destSize.Width, destSize.Height)

      destImage.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution)

      Using graphics As Graphics = Graphics.FromImage(destImage)
         graphics.CompositingMode = CompositingMode.SourceCopy
         graphics.CompositingQuality = CompositingQuality.HighQuality
         graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
         graphics.SmoothingMode = SmoothingMode.HighQuality
         graphics.PixelOffsetMode = PixelOffsetMode.HighQuality

         Using attributes As ImageAttributes = New ImageAttributes()
            attributes.SetWrapMode(WrapMode.TileFlipXY)
            graphics.DrawImage(srcImage, destRect, 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, attributes)
         End Using
      End Using

      Return destImage
   End Function

   Public Sub Draw(g As Graphics, srcLocation As Point, destSize As Size)
      Dim destRect As New Rectangle(Point.Empty, destSize)
      Dim srcRect As New Rectangle(srcLocation, destSize)
      g.DrawImage(ResizedImage, destRect, srcRect, GraphicsUnit.Pixel)
   End Sub

   Private Function SrcSize(destSize As Size) As Size
      Return New Size(destSize.Width * xScale(), destSize.Height * yScale())
   End Function

   Private Function xScale() As Single
      Return Image.Width / Size.Width
   End Function

   Private Function yScale() As Single
      Return Image.Height / Size.Height
   End Function

   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(disposing As Boolean)
      Try
         If _resizedImage IsNot Nothing Then _resizedImage.Dispose()
         _resizedImage = Nothing
         If _rawImage IsNot Nothing Then _rawImage.Dispose()
         _rawImage = Nothing
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

End Class
